#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using System.Data.SqlClient;
using Captain.DatabaseLayer;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using DevExpress.XtraBars;
using Captain.Common.Interfaces;
using DevExpress.XtraRichEdit.Model;
using DevExpress.DashboardExport.Map;
using System.IO;
using DevExpress.Map.Native;
using DevExpress.CodeParser;
using Microsoft.SqlServer.Server;
using Captain.Common.Views.UserControls;
using NPOI.SS.Formula.Functions;
using DevExpress.DataProcessing.InMemoryDataProcessor;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PIPUpdateApplicantForm : _iForm
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        public DataSet _dsAllDSSXML = new DataSet();
        string _isNewApplicant = "";
        string _xmlAppID = "";


        #endregion
        public PIPUpdateApplicantForm(DataTable Leanintakedt, DataTable snpdatadt, BaseForm baseForm, string strAgency, string strDept, string strprogram, string strYear, string strApplNo, string strCloseFormstatus)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propLeanIntakeEntity = new List<Model.Objects.LeanIntakeEntity>();
            BaseForm = baseForm;
            _propSnpDatatable = snpdatadt;
            _propLeanIntakedt = Leanintakedt;
            strFormType = string.Empty;
            _propCloseFormStatus = strCloseFormstatus;
            btnAddress.Visible = false;
            pnlDSSConfirm.Visible = false;
            pnlDispQues.Visible = false;
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "TIOGA")
            {
                btncustomQues.Visible = false;
            }
            if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "1")
            {
                btnServices.Text = "View All Programs Inquired";
            }
            else
            {
                btnServices.Text = "View All Services Inquired";
            }
            Relation = _model.lookupDataAccess.GetRelationship();
            int rowIndex = 0;
            // DataView dv = new DataView(snpdatadt);
            propAgency = strAgency;
            propDept = strDept;
            propProgram = strprogram;
            propYear = strYear;
            propAppl = strApplNo;
            propHierchy = propAgency + propDept + propProgram + propYear + propAppl;
            propCasemstEntity = _model.CaseMstData.GetCaseMST(strAgency, strDept, strprogram, strYear, strApplNo);

            propMainCasemstEntity = _model.CaseMstData.GetCaseMST(strAgency, strDept, strprogram, strYear, strApplNo);

            propCasesnpEntity = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strprogram, strYear, strApplNo);
            propMainCasesnpEntity = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strprogram, strYear, strApplNo);
            propcaseIncomeEntity = _model.CaseMstData.GetCaseIncomeadpynf(strAgency, strDept, strprogram, strYear, strApplNo, string.Empty);
            lblCurrentHead.Text = "Current CAPTAIN Data";
            lblRecentHead.Text = "PIP Intake";


            foreach (DataRow dritem in Leanintakedt.Rows)
            {
                DataTable dtview = snpdatadt;
                propLeanUserId = dritem["PIP_REG_ID"].ToString();
                string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);
                string memberCode = string.Empty;
                CommonEntity rel = Relation.Find(u => u.Code.Equals(dritem["PIP_MEMBER_CODE"].ToString().Trim()));
                if (rel != null) memberCode = rel.Desc;
                string DOB = string.Empty;
                if (dritem["PIP_DOB"].ToString() != string.Empty)
                    DOB = LookupDataAccess.Getdate(dritem["PIP_DOB"].ToString().Trim());
                DataView dv = dtview.DefaultView;
                if (dritem["PIP_DOB"].ToString() != string.Empty)
                {
                    dv.RowFilter = "Fname LIKE '" + dritem["PIP_FNAME"].ToString().FirstOrDefault() + "*'  AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";//;
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        dv = dtview.DefaultView;
                        dv.RowFilter = "Fname = '" + dritem["PIP_FNAME"].ToString() + "'  AND Lname = '" + dritem["PIP_LNAME"].ToString() + "'";
                    }
                }
                else
                {
                    dv.RowFilter = "Fname = '" + dritem["PIP_FNAME"].ToString() + "'  AND Lname = '" + dritem["PIP_LNAME"].ToString() + "'";
                }
                dtview = dv.ToTable();
                if (dtview.Rows.Count > 0)
                {
                    string memberRecentCode = string.Empty;
                    CommonEntity rel1 = Relation.Find(u => u.Code.Equals(dtview.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim()));
                    if (rel1 != null) memberRecentCode = rel1.Desc;

                    propLeanIntakeEntity.Add(new Model.Objects.LeanIntakeEntity(dritem));
                    rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(dtview.Rows[0]["Fname"].ToString(), dtview.Rows[0]["Mname"].ToString(), dtview.Rows[0]["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()),
                        LookupDataAccess.Getdate(dtview.Rows[0]["DOB"].ToString()), memberRecentCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode);
                    gvwCustomer.Rows[rowIndex].Tag = new Model.Objects.LeanIntakeEntity(dritem);
                    CaseSnpEntity SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.NameixFi.ToUpper().FirstOrDefault() == dritem["PIP_FNAME"].ToString().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem["PIP_DOB"].ToString()));
                    gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = SnpCaptainMemEntity;

                }
                else
                {
                    LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(dritem, "TRUE");
                    leanintakedata.PIP_MEMBER_TYPE = "New Member";
                    propLeanIntakeEntity.Add(leanintakedata);
                    rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, memberCode, "N", "Add", ApplicantName, DOB, memberCode);
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    gvwCustomer.Rows[rowIndex].Tag = leanintakedata; gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    gvwCustomer.Rows[rowIndex].Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                    gvwCustomer.Rows[rowIndex].Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                    gvwCustomer.Rows[rowIndex].Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;

                }


                if (dritem["PIP_FAM_SEQ"].ToString() == "0")
                {
                    propLeanServices = dritem["PIP_SERVICES"].ToString();
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";
                }
            }
            foreach (DataRow drsnpitem in snpdatadt.Rows)
            {
                DataTable dtview = Leanintakedt;
                string ApplicantName = LookupDataAccess.GetMemberName(drsnpitem["FNAME"].ToString(), drsnpitem["MNAME"].ToString(), drsnpitem["LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);

                if (drsnpitem["App_Mem_SW"].ToString().Trim() == "A")
                {

                    List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString());
                    string strProgramName = string.Empty;
                    string strShortName = string.Empty;
                    if (programEntityList.Count > 0)
                    {
                        strProgramName = programEntityList[0].ProgramName;
                        strShortName = programEntityList[0].ShortName;
                    }

                    lblApplicantDetails.Text = ApplicantName + " -- Intake from [" + drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString() + drsnpitem["SNP_YEAR"].ToString().Trim() + " - " + strProgramName + "] Application # " + drsnpitem["SNP_APP"].ToString();

                    lblCurrentHie.Text = drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString() + drsnpitem["SNP_YEAR"].ToString().Trim() + " - " + strShortName;
                }

                string memberCode = string.Empty;
                CommonEntity rel = Relation.Find(u => u.Code.Equals(drsnpitem["SNP_MEMBER_CODE"].ToString().Trim()));
                if (rel != null) memberCode = rel.Desc;
                string DOB = string.Empty;
                if (drsnpitem["DOB"].ToString() != string.Empty)
                    DOB = LookupDataAccess.Getdate(drsnpitem["DOB"].ToString().Trim());
                DataView dv = dtview.DefaultView;
                if (drsnpitem["DOB"].ToString() != string.Empty)
                {
                    dv.RowFilter = "PIP_FNAME LIKE '" + drsnpitem["FNAME"].ToString().FirstOrDefault() + "*' AND PIP_DOB = '" + drsnpitem["DOB"].ToString() + "'";
                }
                else
                {
                    dv.RowFilter = "PIP_FNAME = '" + drsnpitem["FNAME"].ToString() + "' AND PIP_LNAME = '" + drsnpitem["LNAME"].ToString() + "'";

                    // dv.RowFilter = "PIP_FNAME LIKE '" + drsnpitem["FNAME"].ToString().FirstOrDefault() + "*' AND PIP_DOB = '" + DateTime.Now.ToString() + "'";

                }

                dtview = dv.ToTable();
                if (dtview.Rows.Count > 0)
                {
                    //rowIndex = gvwCustomer.Rows.Add(ApplicantName, drsnpitem["PIP_SSN"].ToString(), DOB, memberCode, string.Empty);
                }
                else
                {
                    LeanIntakeEntity leanintakeEntitydata = new Model.Objects.LeanIntakeEntity();
                    leanintakeEntitydata.PIP_SEQ = drsnpitem["FAMSEQ"].ToString();
                    leanintakeEntitydata.PIP_FNAME = drsnpitem["FNAME"].ToString();
                    leanintakeEntitydata.PIP_LNAME = drsnpitem["LNAME"].ToString();
                    leanintakeEntitydata.PIP_MEMBER_TYPE = "Inactive Member";
                    leanintakeEntitydata.PIP_Valid_MODE = 1;
                    rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem["SSN"].ToString().Trim()), DOB, memberCode, "I", "Inactivate", "Not in this Household");
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    gvwCustomer.Rows[rowIndex].Tag = leanintakeEntitydata;
                }


            }
            if (gvwCustomer.Rows.Count > 0)
            {
                gvwCustomer.Rows[0].Selected = true;
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
            }

            //Kranthi 09/21/2023 :: Adding logic to change update button to Connect button 
            CheckConnectbutton();
            pnlDSSConfirm.Visible = false;
        }
        //Kranthi 09/21/2023 :: Adding logic to change update button to Connect button 
        void CheckConnectbutton()
        {

            if (gvwCustomer.Rows.Count > 0)
            {

                List<DataGridViewRow> lstNeworInactMem = gvwCustomer.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtType"].Value.ToString() == "N" || x.Cells["gvtType"].Value.ToString() == "I").ToList();
                if (lstNeworInactMem.Count == 0)
                {

                    bool chkValidMember = true;
                    foreach (DataGridViewRow dRow in gvwCustomer.Rows)
                    {


                        LeanIntakeEntity drPIPitem = dRow.Tag as LeanIntakeEntity;
                        string stroldvalue = string.Empty; string strNewvalue = string.Empty;
                        if (drPIPitem != null)
                        {
                            if (dRow.Cells["gvtType"].Value.ToString() != "I")
                            {
                                DataTable dtCAPRec = _propSnpDatatable;
                                DataView dv = dtCAPRec.DefaultView;
                                dv.RowFilter = "Fname LIKE '" + drPIPitem.PIP_FNAME.FirstOrDefault() + "*' AND DOB = '" + drPIPitem.PIP_DOB.ToString() + "'";

                                if (dv.ToTable().Rows.Count == 0)
                                {
                                    dv = dtCAPRec.DefaultView;
                                    dv.RowFilter = "Fname = '" + drPIPitem.PIP_FNAME.ToString() + "' AND Lname = '" + drPIPitem.PIP_LNAME.ToString() + "'";
                                }
                                dtCAPRec = dv.ToTable();

                                if (dtCAPRec.Rows.Count > 0)
                                {
                                    if (drPIPitem.PIP_SSN.ToString().Trim() != string.Empty)
                                    {
                                        if (drPIPitem.PIP_SSN.ToString().Trim() != dtCAPRec.Rows[0]["SSN"].ToString().Trim())
                                        {
                                            chkValidMember = false;
                                            break;
                                        }
                                    }

                                    if (drPIPitem.PIP_FNAME.ToString().Trim() != string.Empty)
                                    {
                                        if (drPIPitem.PIP_FNAME.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["Fname"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false;
                                            break;
                                        }
                                    }
                                    if (drPIPitem.PIP_MNAME.ToString().Trim() != string.Empty)
                                    {
                                        if (drPIPitem.PIP_MNAME.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MName"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_LNAME.ToString().Trim() != string.Empty)
                                    {
                                        if (drPIPitem.PIP_LNAME.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["LName"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_DOB.ToString().Trim() != string.Empty)
                                    {
                                        if (LookupDataAccess.Getdate(drPIPitem.PIP_DOB.ToString().Trim().ToUpper()) != LookupDataAccess.Getdate(dtCAPRec.Rows[0]["DOB"].ToString()))
                                        {

                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_GENDER.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_SEX"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_GENDER.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_GENDER.ToString().Trim() != dtCAPRec.Rows[0]["SNP_SEX"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_PREGNANT.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.PREGNANT, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_PREGNANT"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_PREGNANT.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_PREGNANT.ToString().Trim() != dtCAPRec.Rows[0]["SNP_PREGNANT"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_MARITAL_STATUS.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MARITALSTATUS, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_MARITAL_STATUS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_MARITAL_STATUS.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_MARITAL_STATUS.ToString().Trim() != dtCAPRec.Rows[0]["SNP_MARITAL_STATUS"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_RELATIONSHIP.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_RELATIONSHIP.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_RELATIONSHIP.ToString().Trim() != dtCAPRec.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_ETHNIC.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_ETHNIC"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_ETHNIC.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_ETHNIC.ToString().Trim() != dtCAPRec.Rows[0]["SNP_ETHNIC"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_RACE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_RACE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_RACE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_RACE.ToString().Trim() != dtCAPRec.Rows[0]["SNP_RACE"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_EDUCATION.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_EDUCATION"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_EDUCATION.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_EDUCATION.ToString().Trim() != dtCAPRec.Rows[0]["SNP_EDUCATION"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_SCHOOL.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SCHOOLDISTRICTS, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_SCHOOL_DISTRICT"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_SCHOOL.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_SCHOOL.ToString().Trim() != dtCAPRec.Rows[0]["SNP_SCHOOL_DISTRICT"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_RELITRAN.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELIABLETRANSPORTATION, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_RELITRAN"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_RELITRAN.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_RELITRAN.ToString().Trim() != dtCAPRec.Rows[0]["SNP_RELITRAN"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_DRVLIC.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DRIVERLICENSE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_DRVLIC"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_DRVLIC.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_DRVLIC.ToString().Trim() != dtCAPRec.Rows[0]["SNP_DRVLIC"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_HEALTH_INS.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEALTHINSURANCE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_HEALTH_INS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_HEALTH_INS.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_HEALTH_INS.ToString().Trim() != dtCAPRec.Rows[0]["SNP_HEALTH_INS"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_HEALTH_CODES.ToString().Trim() != string.Empty)
                                    {

                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HealthCodes, "**", "**", "**", string.Empty);

                                        string[] strPipHealthcodes = drPIPitem.PIP_HEALTH_CODES.ToString().Trim().Split(',');
                                        string[] strHealthcodes = dtCAPRec.Rows[0]["SNP_HEALTH_CODES"].ToString().Trim().Split(',');
                                        bool booladdHealthcode = true;
                                        int inthealthcode = 0;
                                        string strNoncashmode = string.Empty;
                                        foreach (string strHealthitem in strPipHealthcodes)
                                        {

                                            if (strHealthitem.ToString() != string.Empty)
                                            {
                                                booladdHealthcode = true;
                                                foreach (string item in strHealthcodes)
                                                {

                                                    if (strHealthitem.Trim() == item.Trim())
                                                    {
                                                        booladdHealthcode = false;
                                                        ListItem lstitem = drPIPitem.Pip_Healthcodes_list_Mode.Find(u => u.Text.ToString().Trim() == item.ToString().Trim());
                                                        drPIPitem.Pip_Healthcodes_list_Mode.Remove(lstitem);
                                                        break;
                                                    }
                                                }
                                                if (booladdHealthcode)
                                                {
                                                    //  comOldvalue = listAgytabls.Find(u => u.Code.Trim() == strincome.Trim());

                                                    foreach (CommonEntity item in listAgytabls)
                                                    {

                                                        if (strHealthitem.Trim() == (item.Code.ToString().Trim()))
                                                        {

                                                            if (drPIPitem.Pip_Healthcodes_list_Mode.Count > 0)
                                                            {
                                                                strNoncashmode = drPIPitem.Pip_Healthcodes_list_Mode[inthealthcode].Value.ToString();
                                                            }
                                                            inthealthcode = inthealthcode + 1;
                                                            chkValidMember = false; break;
                                                        }
                                                    }
                                                }
                                            }


                                        }

                                    }
                                    if (drPIPitem.PIP_MILITARY_STATUS.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MilitaryStatus, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_MILITARY_STATUS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_MILITARY_STATUS.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_MILITARY_STATUS.ToString().Trim() != dtCAPRec.Rows[0]["SNP_MILITARY_STATUS"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_VETERAN.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.VETERAN, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_VET"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_VETERAN.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_VETERAN.ToString().Trim() != dtCAPRec.Rows[0]["SNP_VET"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_FOOD_STAMP.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FOODSTAMPS, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_FOOD_STAMPS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_FOOD_STAMP.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_FOOD_STAMP.ToString().Trim() != dtCAPRec.Rows[0]["SNP_FOOD_STAMPS"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }

                                    if (drPIPitem.PIP_DISABLE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_DISABLE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_DISABLE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_DISABLE.ToString().Trim() != dtCAPRec.Rows[0]["SNP_DISABLE"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_FARMER.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FARMER, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_FARMER"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_FARMER.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_FARMER.ToString().Trim() != dtCAPRec.Rows[0]["SNP_FARMER"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_WORK_STAT.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_WORK_STAT"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_WORK_STAT.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_WORK_STAT.ToString().Trim() != dtCAPRec.Rows[0]["SNP_WORK_STAT"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_WIC.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WIC, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["SNP_WIC"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_WIC.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_WIC.ToString().Trim() != dtCAPRec.Rows[0]["SNP_WIC"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_NCASHBEN.ToString().Trim() != string.Empty)
                                    {

                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, "**", "**", "**", string.Empty);

                                        string[] strPipNoncashbenefit = drPIPitem.PIP_NCASHBEN.ToString().Trim().Split(',');
                                        string[] strNCashben = dtCAPRec.Rows[0]["SNP_NCASHBEN"].ToString().Trim().Split(',');
                                        bool booladdNoncash = true;
                                        int intNoncashbenefit = 0;
                                        string strNoncashmode = string.Empty;
                                        foreach (string strNonbitem in strPipNoncashbenefit)
                                        {

                                            if (strNonbitem.ToString() != string.Empty)
                                            {
                                                booladdNoncash = true;
                                                foreach (string item in strNCashben)
                                                {

                                                    if (strNonbitem.Trim() == item.Trim())
                                                    {
                                                        booladdNoncash = false;
                                                        ListItem lstitem = drPIPitem.Pip_NonCashbenefit_list_Mode.Find(u => u.Text.ToString().Trim() == item.ToString().Trim());
                                                        drPIPitem.Pip_NonCashbenefit_list_Mode.Remove(lstitem);
                                                        break;
                                                    }
                                                }
                                                if (booladdNoncash)
                                                {
                                                    //  comOldvalue = listAgytabls.Find(u => u.Code.Trim() == strincome.Trim());

                                                    foreach (CommonEntity item in listAgytabls)
                                                    {

                                                        if (strNonbitem.Trim() == (item.Code.ToString().Trim()))
                                                        {

                                                            if (drPIPitem.Pip_NonCashbenefit_list_Mode.Count > 0)
                                                            {
                                                                strNoncashmode = drPIPitem.Pip_NonCashbenefit_list_Mode[intNoncashbenefit].Value.ToString();
                                                            }
                                                            intNoncashbenefit = intNoncashbenefit + 1;
                                                            chkValidMember = false; break;
                                                        }
                                                    }
                                                }
                                            }


                                        }
                                    }

                                    if (drPIPitem.PIP_HOUSENO.ToString().Trim() != string.Empty)
                                    {

                                        if (drPIPitem.PIP_HOUSENO.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_HN"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_DIRECTION.ToString().Trim() != string.Empty)
                                    {

                                        if (drPIPitem.PIP_DIRECTION.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_DIRECTION"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_STREET.ToString().Trim() != string.Empty)
                                    {

                                        if (drPIPitem.PIP_STREET.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_STREET"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_SUFFIX.ToString().Trim() != string.Empty)
                                    {
                                        if (drPIPitem.PIP_SUFFIX.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_SUFFIX"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }

                                    }
                                    if (drPIPitem.PIP_APT.ToString().Trim() != string.Empty)
                                    {
                                        if (drPIPitem.PIP_APT.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_APT"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_FLR.ToString().Trim() != string.Empty)
                                    {

                                        if (drPIPitem.PIP_FLR.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_FLR"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_CITY.ToString().Trim() != string.Empty)
                                    {

                                        if (drPIPitem.PIP_CITY.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_CITY"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_ZIP.ToString().Trim() != string.Empty)
                                    {

                                        if (drPIPitem.PIP_ZIP.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_ZIP"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_STATE.ToString().Trim() != string.Empty)
                                    {
                                        if (drPIPitem.PIP_STATE.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_STATE"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }

                                    if (drPIPitem.PIP_COUNTY.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["MST_COUNTY"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_COUNTY.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_COUNTY.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_COUNTY"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }

                                    if (drPIPitem.PIP_TOWNSHIP.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["MST_TOWNSHIP"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_TOWNSHIP.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_TOWNSHIP.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_TOWNSHIP"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }


                                    if (drPIPitem.PIP_PRI_LANGUAGE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["MST_LANGUAGE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_PRI_LANGUAGE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_PRI_LANGUAGE.ToString().Trim() != dtCAPRec.Rows[0]["MST_LANGUAGE"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_FAMILY_TYPE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["MST_FAMILY_TYPE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_FAMILY_TYPE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_FAMILY_TYPE.ToString().Trim() != dtCAPRec.Rows[0]["MST_FAMILY_TYPE"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }


                                    if (drPIPitem.PIP_HOME_PHONE.ToString().Trim() != string.Empty)
                                    {

                                        if (drPIPitem.PIP_AREA.ToString().Trim() + drPIPitem.PIP_HOME_PHONE.ToString().Trim() != (dtCAPRec.Rows[0]["MST_AREA"].ToString().Trim() + dtCAPRec.Rows[0]["MST_PHONE"].ToString().Trim()))
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }
                                    if (drPIPitem.PIP_CELL_NUMBER.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;

                                        if (drPIPitem.PIP_CELL_NUMBER.ToString().Trim().ToUpper() != dtCAPRec.Rows[0]["MST_CELL_PHONE"].ToString().Trim().ToUpper())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }

                                    if (drPIPitem.PIP_HOUSING.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtCAPRec.Rows[0]["MST_HOUSING"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == drPIPitem.PIP_HOUSING.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (drPIPitem.PIP_HOUSING.ToString().Trim() != dtCAPRec.Rows[0]["MST_HOUSING"].ToString().Trim())
                                        {
                                            chkValidMember = false; break;
                                        }
                                    }

                                    if (drPIPitem.PIP_INCOME_TYPES.ToString().Trim() != string.Empty)
                                    {
                                        string pipremovetypes = drPIPitem.PIP_INCOME_TYPES.ToString().Replace(",", "");
                                        string mstremovetypes = dtCAPRec.Rows[0]["MST_INCOME_TYPES"].ToString().Replace(" ", "");
                                        if (pipremovetypes.ToUpper().Trim() != mstremovetypes.ToUpper().Trim())
                                        {
                                            stroldvalue = strNewvalue = string.Empty;
                                            List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.INCOMETYPES, "**", "**", "**", string.Empty);


                                            string strvalue = dtCAPRec.Rows[0]["MST_INCOME_TYPES"].ToString();
                                            List<string> strlist = new List<string>();
                                            for (int i = 0; i < strvalue.Length; i += 2)
                                                if (i + 2 > strvalue.Length)
                                                    strlist.Add(strvalue.Substring(i));
                                                else
                                                    strlist.Add(strvalue.Substring(i, 2));

                                            foreach (var ncashbenitem in strlist)
                                            {

                                                CommonEntity comOldvalue = listAgytabls.Find(u => u.Code.Trim() == ncashbenitem.Trim());
                                                if (comOldvalue != null)
                                                {
                                                    stroldvalue = stroldvalue + comOldvalue.Desc + ", ";
                                                }
                                            }
                                            string[] strLeanNCashben = drPIPitem.PIP_INCOME_TYPES.ToString().Trim().Split(',');

                                            foreach (var ncashbenitem in strLeanNCashben)
                                            {
                                                CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == ncashbenitem.Trim());
                                                if (comNewvalue != null)
                                                {
                                                    strNewvalue = strNewvalue + comNewvalue.Desc + ",";
                                                }
                                            }


                                            if (strlist.Count != strLeanNCashben.Length)
                                            {

                                                chkValidMember = false; break;
                                            }
                                            else
                                            {
                                                bool boolvalid = true;
                                                foreach (var ncashbenitem in strlist)
                                                {
                                                    if (!pipremovetypes.Contains(ncashbenitem.Trim()))
                                                    {
                                                        boolvalid = false;
                                                    }
                                                }
                                                if (boolvalid == false)
                                                {
                                                    chkValidMember = false; break;
                                                }
                                            }
                                        }
                                    }
                                    if (drPIPitem.PIP_SERVICES != string.Empty)
                                    {
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

                                        string strAgy = "00";
                                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                                            strAgy = propAgency;

                                        DataTable dtservice = PIPDATA.GETPIPSERVICES(BaseForm.BaseLeanDataBaseConnectionString, strType, BaseForm.BaseAgencyControlDetails.AgyShortName, strAgy);

                                        DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", propAgency, propDept, propProgram, propYear, propAppl);
                                        DataTable serviceSaveDT = serviceSaveDS.Tables[0];


                                        string[] strServices = drPIPitem.PIP_SERVICES.Split(',');
                                        int intservices = 0;
                                        bool booladdservice = true;
                                        string strservicemode = string.Empty;
                                        foreach (string strserv in strServices)
                                        {
                                            if (strserv.ToString() != string.Empty)
                                            {
                                                booladdservice = true;
                                                foreach (DataRow drservicessave in serviceSaveDT.Rows)
                                                {
                                                    if (strserv.Trim() == (drservicessave["INQ_CODE"].ToString().Trim()))
                                                    {
                                                        booladdservice = false;
                                                        ListItem lstitem = drPIPitem.Pip_service_list_Mode.Find(u => u.Text.ToString().Trim() == drservicessave["INQ_CODE"].ToString().Trim());
                                                        drPIPitem.Pip_service_list_Mode.Remove(lstitem);
                                                        break;
                                                    }
                                                }
                                                if (booladdservice)
                                                {
                                                    foreach (DataRow drservices in dtservice.Rows)
                                                    {

                                                        if (strserv.Trim() == (drservices["CODE"].ToString()))
                                                        {
                                                            if (drPIPitem.Pip_service_list_Mode.Count > 0)
                                                            {
                                                                strservicemode = drPIPitem.Pip_service_list_Mode[intservices].Value.ToString();
                                                            }
                                                            intservices = intservices + 1;
                                                            chkValidMember = false; break;
                                                        }
                                                    }
                                                }
                                            }
                                        }



                                    }


                                }

                            }
                        }


                    }

                    if (chkValidMember)
                    {
                        btnLeanData.Visible = true;
                        btnLeanData.Text = "Connect";
                    }
                    else
                        btnLeanData.Text = "Update";
                }
                else
                {
                    btnLeanData.Text = "Update";
                }


            }
        }

        public PIPUpdateApplicantForm() {
            InitializeComponent();
        }
       PrivilegeEntity _privilegeEntity;
        //Added by Sudheer on 09/04/23
        public PIPUpdateApplicantForm(DataTable DSSXMLdt, DataTable snpdatadt, DataSet dsAllDSSXML, BaseForm baseForm, string strAgency, string strDept, string strprogram, string strYear, string strApplNo, string strCloseFormstatus, string isNewApplicant, string xmlAPPID, string FormStatus, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propLeanIntakeEntity = new List<Model.Objects.LeanIntakeEntity>();
            BaseForm = baseForm;
            _privilegeEntity = privilegeEntity;
            _propSnpDatatable = snpdatadt;
            _propDSSXMLIntakedt = DSSXMLdt;
            _dsAllDSSXML = dsAllDSSXML;
            _isNewApplicant = isNewApplicant;
            _xmlAppID = xmlAPPID;
            _propCloseFormStatus = "";
            strFormType = "DSSXML";
            //_propCloseFormStatus = strCloseFormstatus;
            LblHeader.Text = "Client Intake Recent data";
            this.Text = "Crosscheck DSS Intake to CAPTAIN";
            pnlHeader.Visible = false;
            btncustomQues.Visible = false;
            btnServices.Visible = false;
            btnAddress.Visible = false;
            pnlDSSConfirm.Visible = false;
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "TIOGA")
            {
                btncustomQues.Visible = false;
            }
            if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "1")
            {
                btnServices.Text = "View All Programs Inquired";
            }
            else
            {
                btnServices.Text = "View All Services Inquired";
            }
            Relation = _model.lookupDataAccess.GetRelationship();

            // DataView dv = new DataView(snpdatadt);
            propAgency = strAgency;
            propDept = strDept;
            propProgram = strprogram;
            propYear = strYear;
            propAppl = strApplNo;
            propHierchy = propAgency + propDept + propProgram + propYear + propAppl;

            propCasemstEntity = _model.CaseMstData.GetCaseMST(strAgency, strDept, strprogram, strYear, strApplNo);
            propMainCasemstEntity = _model.CaseMstData.GetCaseMST(strAgency, strDept, strprogram, strYear, strApplNo);
            propCasesnpEntity = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strprogram, strYear, strApplNo);
            propMainCasesnpEntity = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strprogram, strYear, strApplNo);
            //propcaseIncomeEntity = _model.CaseMstData.GetCaseIncomeadpynf(strAgency, strDept, strprogram, strYear, strApplNo, string.Empty);
            CapLandlordInfoEntity = _model.CaseMstData.GetLandlordadpya(strAgency, strDept, strprogram, strYear, strApplNo, "Landlord");

            CAPLihpmQueslist = _model.ZipCodeAndAgency.GetLIHPMQuesData(string.Empty, propYear);//"S0056"
            if (CAPLihpmQueslist.Count > 0)
            {
                if (propCasemstEntity.Source == "02" || propCasemstEntity.Source == "04")
                    CAPLihpmQueslist = CAPLihpmQueslist.Where(x => x.LPMQ_QTYPE == "N").ToList();
                else
                    CAPLihpmQueslist = CAPLihpmQueslist.Where(x => x.LPMQ_QTYPE == "Y").ToList();
            }

            if (CapLandlordInfoEntity == null)
                CapLandlordInfoEntity = new CaseDiffEntity();

            CapCaseDiffMailDetails = _model.CaseMstData.GetCaseDiffadpya(strAgency, strDept, strprogram, strYear, strApplNo, "");
            if (CapCaseDiffMailDetails == null)
                CapCaseDiffMailDetails = new CaseDiffEntity();

            lblRecentHie.Visible = false;
            lblCurrentHead.Text = "DSS Intake Household Members";
            lblCurrentHead.Font = new System.Drawing.Font("Calibri", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            lblCurrentHead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            pnlDSSConfirm.Visible = true;

            if (_isNewApplicant == "R")
                lblRecentHead.Text = "Connected Client Intake Household Members <br/>(" + propAgency + "-" + propDept + "-" + propProgram + " " + propYear + " " + propAppl + ")";//"Current CAPTAIN Data";
            else
            {
                lblRecentHead.Text = "New Client Intake Household Members <br/>(" + propAgency + "-" + propDept + "-" + propProgram + " " + propYear + " " + propAppl + ")";
                pnlDSSConfirm.Visible = false;
            }

            lblRecentHead.Font = new System.Drawing.Font("Calibri", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            lblRecentHead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            gvwSubdetails.Columns["gvtOldValue"].HeaderText = "DSS Intake Value";
            gvwSubdetails.Columns["gvtNewValue"].HeaderText = "Client Intake Value";
            gvwSubdetails.Columns["gvtSelchk"].DisplayIndex = 2;

            gvwSubdetails.Columns["gvtfield"].Width = 250;

            gvwCustomer.Columns["gvtSSN"].Visible = true;

            DataGridViewColumn gvtCAPSSN = new DataGridViewColumn();
            gvtCAPSSN.Name = "gvtcapSSN";
            gvtCAPSSN.HeaderText = "SSN#";
            gvtCAPSSN.Width = 90;
            gvtCAPSSN.Visible = true;
            gvwCustomer.Columns.Add(gvtCAPSSN);


            DataGridViewImageColumn gvtCRSCHKImg = new DataGridViewImageColumn();
            gvtCRSCHKImg.Name = "gvtCRSCHKImg";
            gvtCRSCHKImg.HeaderText = "";
            gvtCRSCHKImg.Width = 30;
            gvwCustomer.Columns.Add(gvtCRSCHKImg);

            DataGridViewColumn gvchkCrs = new DataGridViewColumn();
            gvchkCrs.Name = "gvchkCrs";
            gvchkCrs.Visible = false;
            gvwCustomer.Columns.Add(gvchkCrs);

            DataGridViewColumn gvadddetsDate = new DataGridViewColumn();
            gvadddetsDate.Name = "gvadddetsDate";
            gvadddetsDate.Visible = false;
            gvwCustomer.Columns.Add(gvadddetsDate);
            DataGridViewColumn gvadddetsUser = new DataGridViewColumn();
            gvadddetsUser.Name = "gvadddetsUser";
            gvadddetsUser.Visible = false;
            gvwCustomer.Columns.Add(gvadddetsUser);

            DataGridViewColumn gvSupplierDate = new DataGridViewColumn();
            gvSupplierDate.Name = "gvSupplierDate";
            gvSupplierDate.Visible = false;
            gvwCustomer.Columns.Add(gvSupplierDate);
            DataGridViewColumn gvSupplierUser = new DataGridViewColumn();
            gvSupplierUser.Name = "gvSupplierUser";
            gvSupplierUser.Visible = false;
            gvwCustomer.Columns.Add(gvSupplierUser);


            DataGridViewColumn gvPMDate = new DataGridViewColumn();
            gvPMDate.Name = "gvPMDate";
            gvPMDate.Visible = false;
            gvwCustomer.Columns.Add(gvPMDate);
            DataGridViewColumn gvPMUser = new DataGridViewColumn();
            gvPMUser.Name = "gvPMUser";
            gvPMUser.Visible = false;
            gvwCustomer.Columns.Add(gvPMUser);

            chkSelectAll.Location = new Point(480, 5);
            propCurentShortName = BaseForm.BaseAgencyControlDetails.AgyShortName;
            CreateMODEforDSSXML(_propDSSXMLIntakedt);
            LoadDSSXMLGrid();

            #region Added by Vikash on 10/18/2023 as per "DSS Intake Enhancements & Issues - Crosscheck DSS Intake to CAPTAIN – Add DSS Intake Fields" point

            pnlDispQues.Visible = true;
            lblQA.Text = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString();
            lblQ1A.Text = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Need_For_Fuel__c"].ToString();
            lblQ2A.Text = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Heat_Currently_Shut_Off__c"].ToString();
            lblQ3A.Text = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Without_water_or_WastewaterService__c"].ToString();

            #endregion

           
        }

        string Img_Blank = "blank";  // "Resources/Images/tick.ico";
        string Img_Tick = "icon-tick4"; // "Resources/Images/cross.ico";

        void LoadDSSXMLGrid()
        {

            int rowIndex = 0;
            try
            {
                if (gvwCustomer.Rows.Count > 0)
                {
                    gvwCustomer.SelectionChanged -= gvwCustomer_SelectionChanged;
                    gvwCustomer.Rows.Clear();
                    gvwCustomer.SelectionChanged += gvwCustomer_SelectionChanged;
                }
            }
            catch (Exception ex)
            {

            }

            DataTable dtMIDtable = DSSXMLData.DSSXMLMID_GET(BaseForm.BaseDSSXMLDBConnString, "", "", _xmlAppID, "", "BYAPPID");

            //_propDSSXMLIntakedt.DefaultView.Sort = "Is_Primary_Applicant__c desc";
            //_propDSSXMLIntakedt = _propDSSXMLIntakedt.DefaultView.ToTable();
            foreach (DataRow dritem in _propDSSXMLIntakedt.Rows)
            {

                string MIName = string.Empty;
                if (!string.IsNullOrEmpty(dritem["Middle_Name__c"].ToString().Trim()))
                    MIName = dritem["Middle_Name__c"].ToString().Trim().Substring(0, 1);

                DataTable dtview = _propSnpDatatable;
                //propLeanUserId = dritem["PIP_REG_ID"].ToString();
                string ApplicantName = dritem["First_Name__c"].ToString().Trim() + " " + MIName.ToUpper() + " " + dritem["Last_Name__c"].ToString().Trim();
                string memberCode = string.Empty; string RelCode = string.Empty;
                if (dritem["Is_Primary_Applicant__c"].ToString() == "true")
                {
                    RelCode = "C";

                }
                else
                {
                    RelCode = TMS00141Form.getRelation(dritem["CEAP_Relationships__c"].ToString(), dritem["Sex__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                }

                CommonEntity rel = Relation.Find(u => u.Code.Equals(RelCode.Trim()));
                if (rel != null) memberCode = rel.Desc;
                string DOB = string.Empty;
                if (dritem["Date_of_Birth__c"].ToString() != string.Empty)
                    DOB = LookupDataAccess.Getdate(dritem["Date_of_Birth__c"].ToString().Trim());

                DataView dv = dtview.DefaultView;
                string xmlSSNo = dritem["SSN__c"].ToString().Trim().Replace("-", "");

                if (xmlSSNo != string.Empty)
                {
                    dv.RowFilter = "Ssno = '" + xmlSSNo + "'";
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        if (dritem["Date_of_Birth__c"].ToString() != string.Empty)
                        {
                            dv = dtview.DefaultView;
                            dv.RowFilter = "NameixFi = '" + dritem["First_Name__c"].ToString().Replace("'", "''") + "'  AND AltBdate Like '%" + Convert.ToDateTime(dritem["Date_of_Birth__c"].ToString()).ToString("M/d/yyyy") + "%'";//;
                            if (dv.ToTable().Rows.Count == 0)
                            {
                                dv = dtview.DefaultView;
                                dv.RowFilter = "NameixFi = '" + dritem["First_Name__c"].ToString().Replace("'", "''") + "'  AND NameixLast = '" + dritem["Last_Name__c"].ToString().Replace("'", "''") + "'";
                            }
                        }
                        else
                        {
                            dv = dtview.DefaultView;
                            dv.RowFilter = "NameixFi = '" + dritem["First_Name__c"].ToString().Replace("'", "''") + "'  AND NameixLast = '" + dritem["Last_Name__c"].ToString().Replace("'", "''") + "'";
                        }
                    }
                }
                else
                {
                    if (dritem["Date_of_Birth__c"].ToString() != string.Empty)
                    {
                        dv.RowFilter = "NameixFi = '" + dritem["First_Name__c"].ToString().Replace("'", "''") + "'  AND AltBdate Like '%" + Convert.ToDateTime(dritem["Date_of_Birth__c"].ToString()).ToString("M/d/yyyy") + "%'";//;
                        if (dv.ToTable().Rows.Count == 0)
                        {
                            dv = dtview.DefaultView;
                            dv.RowFilter = "NameixFi = '" + dritem["First_Name__c"].ToString().Replace("'", "''") + "'  AND NameixLast = '" + dritem["Last_Name__c"].ToString().Replace("'", "''") + "'";
                        }
                    }
                    else
                    {
                        dv.RowFilter = "NameixFi = '" + dritem["First_Name__c"].ToString().Replace("'", "''") + "'  AND NameixLast = '" + dritem["Last_Name__c"].ToString().Replace("'", "''") + "'";
                    }
                }

                dtview = dv.ToTable();
                if (dtview.Rows.Count > 0)
                {
                    string memberRecentCode = string.Empty;
                    CommonEntity rel1 = Relation.Find(u => u.Code.Equals(dtview.Rows[0]["MemberCode"].ToString().Trim()));
                    if (rel1 != null) memberRecentCode = rel1.Desc;

                    //propLeanIntakeEntity.Add(new Model.Objects.LeanIntakeEntity(dritem));
                    CaseSnpEntity SnpCaptainMemEntity = null;
                    if (xmlSSNo != "")
                    {
                        SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.Ssno == xmlSSNo);
                        if (SnpCaptainMemEntity == null)
                            SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dritem["First_Name__c"].ToString().ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem["Date_of_Birth__c"].ToString()));
                        if (SnpCaptainMemEntity == null)
                            SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dritem["First_Name__c"].ToString().ToUpper() && u.NameixLast.ToUpper() == dritem["Last_Name__c"].ToString().ToUpper());
                    }
                    else
                    {
                        if (SnpCaptainMemEntity == null)
                            SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dritem["First_Name__c"].ToString().ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem["Date_of_Birth__c"].ToString()));
                        if (SnpCaptainMemEntity == null)
                            SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dritem["First_Name__c"].ToString().ToUpper() && u.NameixLast.ToUpper() == dritem["Last_Name__c"].ToString().ToUpper());
                    }
                    if (SnpCaptainMemEntity != null)
                    {

                        DataRow[] drchkCrs = dtMIDtable.Select("DXM_MEMID='" + dritem["HouseholdmemberSystemID_c"].ToString() + "'");
                        string Imgicon = Img_Blank;
                        string chkCrs = "";
                        if (drchkCrs.Length > 0)
                        {

                            if (drchkCrs[0]["DXM_CRSCHK_USER"].ToString() != "" && drchkCrs[0]["DXM_CRSCHK_DATE"].ToString() != "")
                            {
                                //if (_isNewApplicant == "R")
                                Imgicon = Img_Tick;
                                if (_isNewApplicant != "R")
                                {
                                    Imgicon = Img_Blank;
                                }

                                chkCrs = "Y";
                            }
                        }

                        rowIndex = gvwCustomer.Rows.Add(false,
                       ApplicantName,
                       dritem["SSN__c"].ToString().Trim(),
                       DOB,
                       memberCode,

                       string.Empty, string.Empty,

                       LookupDataAccess.GetMemberName(
                       dtview.Rows[0]["NameixFi"].ToString(),
                       dtview.Rows[0]["NameixMi"].ToString(),
                       dtview.Rows[0]["NameixLast"].ToString(),
                       BaseForm.BaseHierarchyCnFormat),

                       LookupDataAccess.Getdate(dtview.Rows[0]["AltBdate"].ToString()), memberRecentCode, "", "", "", "", "", "", LookupDataAccess.GetPhoneSsnNoFormat(dtview.Rows[0]["Ssno"].ToString()), Imgicon, chkCrs);

                        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = SnpCaptainMemEntity;

                        if (chkCrs == "Y")
                        {
                            // if (_isNewApplicant == "R")
                            foreach (DataGridViewCell drCell in gvwCustomer.Rows[rowIndex].Cells)
                            {
                                drCell.ToolTipText = "Crosschecked by " + drchkCrs[0]["DXM_CRSCHK_USER"].ToString() + " on " + drchkCrs[0]["DXM_CRSCHK_DATE"].ToString() + "";
                            }
                            // gvwCustomer.Rows[rowIndex].Cells["gvtCRSCHKImg"].ToolTipText = "Crosschecked by " + drchkCrs[0]["DXM_CRSCHK_USER"].ToString() + " on " + drchkCrs[0]["DXM_CRSCHK_DATE"].ToString() + "";
                        }

                        if (dritem["Is_Primary_Applicant__c"].ToString().ToUpper() == "TRUE")
                        {
                            gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                            gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";

                            //update Verify fields
                            xmlApplMembID = dritem["HouseholdmemberSystemID_c"].ToString();
                            DataRow[] drVerifyFields = dtMIDtable.Select("DXM_MEMID='" + dritem["HouseholdmemberSystemID_c"].ToString() + "'");
                            gvwCustomer.Rows[rowIndex].Cells["gvadddetsDate"].Value = drVerifyFields[0]["DXM_ADRSDETS_DATE"].ToString();
                            gvwCustomer.Rows[rowIndex].Cells["gvadddetsUser"].Value = drVerifyFields[0]["DXM_ADRSDETS_USER"].ToString();
                            gvwCustomer.Rows[rowIndex].Cells["gvSupplierDate"].Value = drVerifyFields[0]["DXM_SUPPLIER_DATE"].ToString();
                            gvwCustomer.Rows[rowIndex].Cells["gvSupplierUser"].Value = drVerifyFields[0]["DXM_SUPPLIER_USER"].ToString();
                            gvwCustomer.Rows[rowIndex].Cells["gvPMDate"].Value = drVerifyFields[0]["DXM_PM_DATE"].ToString();
                            gvwCustomer.Rows[rowIndex].Cells["gvPMUser"].Value = drVerifyFields[0]["DXM_PM_USER"].ToString();



                        }

                    }
                    else
                    {
                        rowIndex = gvwCustomer.Rows.Add(false,
                            ApplicantName,
                            LookupDataAccess.GetPhoneSsnNoFormat(dritem["SSN__c"].ToString().Trim().Replace("-", "")),
                            DOB,
                            memberCode,
                            "N", "Add", "", "", "");
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = dritem;       //leanintakedata;
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                        gvwCustomer.Rows[rowIndex].Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                        gvwCustomer.Rows[rowIndex].Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                        gvwCustomer.Rows[rowIndex].Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;
                    }

                    gvwCustomer.Rows[rowIndex].Tag = dritem;


                }
                else
                {
                    rowIndex = gvwCustomer.Rows.Add(false,
                          ApplicantName,
                          LookupDataAccess.GetPhoneSsnNoFormat(dritem["SSN__c"].ToString().Trim().Replace("-", "")),
                          DOB,
                          memberCode,
                          "N", "Add", "", "", "");
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = dritem;       //leanintakedata;
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    gvwCustomer.Rows[rowIndex].Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                    gvwCustomer.Rows[rowIndex].Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                    gvwCustomer.Rows[rowIndex].Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;

                    gvwCustomer.Rows[rowIndex].Tag = dritem;
                }


                //if (dritem["PIP_FAM_SEQ"].ToString() == "0")
                //{
                //    propLeanServices = dritem["PIP_SERVICES"].ToString();
                //    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //    gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";
                //}
            }
            foreach (DataRow drsnpitem in _propSnpDatatable.Rows)
            {
                DataTable dtview = _propDSSXMLIntakedt;
                string ApplicantName = LookupDataAccess.GetMemberName(drsnpitem["NameixFi"].ToString(), drsnpitem["NameixMi"].ToString(), drsnpitem["NameixLast"].ToString(), BaseForm.BaseHierarchyCnFormat);
                string CapssnNO = LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem["Ssno"].ToString().Trim());

                if (drsnpitem["MemberCode"].ToString().Trim() == "A")
                {

                    List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, drsnpitem["Agency"].ToString() + drsnpitem["Dept"].ToString() + drsnpitem["Program"].ToString());
                    string strProgramName = string.Empty;
                    string strShortName = string.Empty;
                    if (programEntityList.Count > 0)
                    {
                        strProgramName = programEntityList[0].ProgramName;
                        strShortName = programEntityList[0].ShortName;
                    }

                    lblApplicantDetails.Text = ApplicantName + " -- Intake from [" + drsnpitem["Agency"].ToString() + drsnpitem["Dept"].ToString() + drsnpitem["Program"].ToString() + drsnpitem["Year"].ToString().Trim() + " - " + strProgramName + "] Application # " + drsnpitem["App"].ToString();

                    // lblCurrentHie.Text = drsnpitem["Agency"].ToString() + drsnpitem["Dept"].ToString() + drsnpitem["Program"].ToString() + drsnpitem["Year"].ToString().Trim() + " - " + strShortName;
                }

                string memberCode = string.Empty;
                CommonEntity rel = Relation.Find(u => u.Code.Equals(drsnpitem["MemberCode"].ToString().Trim()));
                if (rel != null) memberCode = rel.Desc;
                string DOB = string.Empty;
                if (drsnpitem["AltBdate"].ToString() != string.Empty)
                    DOB = LookupDataAccess.Getdate(drsnpitem["AltBdate"].ToString().Trim());
                DataView dv = dtview.DefaultView;

                if (CapssnNO != "")
                {
                    dv.RowFilter = "SSN__c = '" + CapssnNO + "'";
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        if (drsnpitem["AltBdate"].ToString() != string.Empty)
                        {
                            dv = dtview.DefaultView;
                            dv.RowFilter = "First_Name__c LIKE '" + drsnpitem["NameixFi"].ToString().Replace("'", "''") + "*' AND Date_of_Birth__c Like '%" + Convert.ToDateTime(drsnpitem["AltBdate"].ToString()).ToString("yyyy-MM-dd") + "%'";
                            if (dv.ToTable().Rows.Count == 0)
                            {
                                dv = dtview.DefaultView;
                                dv.RowFilter = "First_Name__c = '" + drsnpitem["NameixFi"].ToString().Replace("'", "''") + "' AND Last_Name__c = '" + drsnpitem["NameixLast"].ToString().Replace("'", "''") + "'";
                            }
                        }
                        else
                        {
                            dv = dtview.DefaultView;
                            dv.RowFilter = "First_Name__c = '" + drsnpitem["NameixFi"].ToString().Replace("'", "''") + "' AND Last_Name__c = '" + drsnpitem["NameixLast"].ToString().Replace("'", "''") + "'";
                        }
                    }
                }
                else
                {
                    if (drsnpitem["AltBdate"].ToString() != string.Empty)
                    {
                        dv.RowFilter = "First_Name__c LIKE '" + drsnpitem["NameixFi"].ToString().Replace("'", "''") + "*' AND Date_of_Birth__c Like '%" + Convert.ToDateTime(drsnpitem["AltBdate"].ToString()).ToString("yyyy-MM-dd") + "%'";
                        if (dv.ToTable().Rows.Count == 0)
                        {
                            dv = dtview.DefaultView;
                            dv.RowFilter = "First_Name__c = '" + drsnpitem["NameixFi"].ToString().Replace("'", "''") + "' AND Last_Name__c = '" + drsnpitem["NameixLast"].ToString().Replace("'", "''") + "'";
                        }
                    }
                    else
                    {
                        dv.RowFilter = "First_Name__c = '" + drsnpitem["NameixFi"].ToString().Replace("'", "''") + "' AND Last_Name__c = '" + drsnpitem["NameixLast"].ToString().Replace("'", "''") + "'";
                    }
                }

                dtview = dv.ToTable();
                if (dtview.Rows.Count > 0)
                {
                    //rowIndex = gvwCustomer.Rows.Add(ApplicantName, drsnpitem["PIP_SSN"].ToString(), DOB, memberCode, string.Empty);
                }
                else
                {
                    //LeanIntakeEntity leanintakeEntitydata = new Model.Objects.LeanIntakeEntity();
                    //leanintakeEntitydata.PIP_SEQ = drsnpitem["FAMSEQ"].ToString();
                    //leanintakeEntitydata.PIP_FNAME = drsnpitem["FNAME"].ToString();
                    //leanintakeEntitydata.PIP_LNAME = drsnpitem["LNAME"].ToString();
                    //leanintakeEntitydata.PIP_MEMBER_TYPE = "Inactive Member";
                    //leanintakeEntitydata.PIP_Valid_MODE = 1;

                    //rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem["SSN"].ToString().Trim()), DOB, memberCode, "I",
                    //    "Inactivate", "Not in this Household");

                    rowIndex = gvwCustomer.Rows.Add(false, "", "", "", "", "I", "Delete", ApplicantName, DOB, memberCode, "", "", "", "", "", "", LookupDataAccess.GetPhoneSsnNoFormat(CapssnNO.Replace("-", "")));

                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    gvwCustomer.Rows[rowIndex].Tag = drsnpitem;

                    CaseSnpEntity SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.NameixFi.ToUpper() == drsnpitem["NameixFi"].ToString().ToUpper()
                    && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(drsnpitem["AltBdate"].ToString()));

                    gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = SnpCaptainMemEntity;
                }


            }

            gvwCustomer.Columns["gvtcapSSN"].DisplayIndex = 8;

            List<DataGridViewRow> drRows = gvwCustomer.Rows.Cast<DataGridViewRow>().ToList();
            drRows.ForEach(xRow => xRow.Cells["gvcTopSelect"].Value = false);


            List<DataGridViewRow> drSortRows = gvwCustomer.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtType"].Value.ToString() != "").ToList();
            if (drSortRows.Count > 1)
            {
                List<DataGridViewRow> drSortEmptyRows = gvwCustomer.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtType"].Value.ToString() == "").ToList();
                drSortEmptyRows.ForEach(x => x.Cells["gvtType"].Value = "M");
                gvwCustomer.Sort(gvwCustomer.Columns["gvtType"], ListSortDirection.Ascending);
                drSortEmptyRows.ForEach(x => x.Cells["gvtType"].Value = "");
            }
            else
            {
                gvwCustomer.Sort(gvwCustomer.Columns["gvtType"], ListSortDirection.Descending);
            }



            if (gvwCustomer.Rows.Count > 0)
            {
                gvwCustomer.Rows[0].Selected = true;
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
            }

            chkSelectAll.CheckedChanged -= new System.EventHandler(chkSelectAll_CheckedChanged);
            chkSelectAll.Checked = false;
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }


        /// <summary>
        ///  This screen only Most recent applicant data method
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="strAgency"></param>
        /// <param name="strDept"></param>
        /// <param name="strprogram"></param>
        /// <param name="strYear"></param>
        /// <param name="strApplNo"></param>
        public PIPUpdateApplicantForm(BaseForm baseForm, string strAgency, string strDept, string strprogram, string strYear, string strApplNo)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propLeanIntakeEntity = new List<Model.Objects.LeanIntakeEntity>();

            BaseForm = baseForm;
            //  btnLeanData.Visible = false;
            btncustomQues.Visible = false;
            _propCloseFormStatus = string.Empty;
            DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuGetHouseDetails(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear == string.Empty ? "    " : BaseForm.BaseYear) + BaseForm.BaseApplicationNo);
            DataTable dt1 = ds1.Tables[0];
            _propSnpDatatable = dt1;
            pnlDispQues.Visible = false;
            strFormType = "IntakeType";
            LblHeader.Text = "Client Intake Recent data";
            this.Text = "Update Client Intake data";

            if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "1")
            {
                btnServices.Text = "View All Programs Inquired";
            }
            else
            {
                btnServices.Text = "View All Services Inquired";
            }

            List<ProgramDefinitionEntity> baseprogramEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, BaseForm.BaseAgency.ToString() + BaseForm.BaseDept.ToString() + BaseForm.BaseProg.ToString());
            string strbaseProgramName = string.Empty;
            string strbaseShorName = string.Empty;
            if (baseprogramEntityList.Count > 0)
            {
                strbaseProgramName = baseprogramEntityList[0].ProgramName;
                strbaseShorName = baseprogramEntityList[0].ShortName;
                propCurentShortName = baseprogramEntityList[0].ShortName;
            }

            gvtOldValue.HeaderText = "Current Intake Data                " + BaseForm.BaseAgency.ToString() + BaseForm.BaseDept.ToString() + BaseForm.BaseProg.ToString() + BaseForm.BaseYear.Trim() + "[" + strbaseShorName + "]";

            lblCurrentHie.Text = BaseForm.BaseAgency.ToString() + BaseForm.BaseDept.ToString() + BaseForm.BaseProg.ToString() + BaseForm.BaseYear.Trim() + "[" + strbaseShorName + "] Application # " + BaseForm.BaseApplicationNo.ToString();

            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "TIOGA")
            {
                btncustomQues.Visible = false;
            }
            Relation = _model.lookupDataAccess.GetRelationship();

            // DataView dv = new DataView(snpdatadt);

            CaseSnpEntity snpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);

            DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchEMS("APP", "APP", null, null, null, null, snpApplicant.NameixFi, null, null, null, null, null, null, null, null, null, snpApplicant.AltBdate, BaseForm.UserID, "Single", string.Empty, string.Empty);



            gvwCustomer.SelectionChanged -= gvwCustomer_SelectionChanged;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dritem in ds.Tables[0].Rows)
                {
                    propAgency = dritem["Agency"].ToString();
                    propDept = dritem["DEPT"].ToString();
                    propProgram = dritem["PROG"].ToString();
                    propYear = dritem["SNPYEAR"].ToString();
                    propAppl = dritem["APPLICANTNO"].ToString();
                    propHierchy = propAgency + propDept + propProgram + propYear + propAppl;
                    propCasemstEntity = _model.CaseMstData.GetCaseMST(propAgency, propDept, propProgram, propYear, propAppl);
                    propCasesnpEntity = _model.CaseMstData.GetCaseSnpadpyn(propAgency, propDept, propProgram, propYear, propAppl);
                    propcaseIncomeEntity = _model.CaseMstData.GetCaseIncomeadpynf(propAgency, propDept, propProgram, propYear, propAppl, string.Empty);

                    break;

                }
                IntakeCompareGridLoad(string.Empty);
            }

            //foreach (CaseSnpEntity snpdetailsdata in propCasesnpEntity)
            //{
            //    string ApplicantName = LookupDataAccess.GetMemberName(snpdetailsdata.NameixFi.ToString(), snpdetailsdata.NameixMi.ToString(), snpdetailsdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);
            //    string memberCode = string.Empty;
            //    CommonEntity rel = Relation.Find(u => u.Code.Equals(snpdetailsdata.MemberCode.ToString().Trim()));
            //    if (rel != null) memberCode = rel.Desc;
            //    string DOB = string.Empty;
            //    if (snpdetailsdata.AltBdate.ToString() != string.Empty)
            //        DOB = LookupDataAccess.Getdate(snpdetailsdata.AltBdate.ToString().Trim());

            //        rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(snpdetailsdata.Ssno.ToString().Trim()), DOB, memberCode, string.Empty);
            //          gvwCustomer.Rows[rowIndex].Tag = snpdetailsdata;
            //        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdetailsdata;




            //    if (propMainCasesnpEntity[0].FamilySeq == snpdetailsdata.FamilySeq )
            //    {                  
            //        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            //        gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";
            //    }
            //}           

        }
        List<CommonEntity> Relation { get; set; }
        public string propCurentShortName { get; set; }
        public string propRecentShortName { get; set; }

        public void IntakeCompareGridLoad(string strGridTyp)
        {

            if (strGridTyp == string.Empty)
            {
                gvwCustomer.SelectionChanged -= gvwCustomer_SelectionChanged;
                gvwCustomer.Rows.Clear();

                int rowIndex = 0;
                DataSet dsservice = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", propAgency, propDept, propProgram, propYear, propAppl);
                DataTable dtservice = dsservice.Tables[0];

                string strCurrentServices = string.Empty;
                foreach (DataRow dritem in dtservice.Rows)
                {
                    if (strCurrentServices != string.Empty)
                    {
                        strCurrentServices = strCurrentServices + "," + dritem["INQ_CODE"].ToString().Trim();
                    }
                    else
                    {
                        strCurrentServices = dritem["INQ_CODE"].ToString().Trim();
                    }

                }

                foreach (CaseSnpEntity item in propCasesnpEntity)
                {
                    if (propCasemstEntity.FamilySeq.Equals(item.FamilySeq))
                    {
                        string ApplicantName = LookupDataAccess.GetMemberName(item.NameixFi.ToString(), item.NameixMi.ToString(), item.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);

                        List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString());
                        string strProgramName = string.Empty;
                        string strShorName = string.Empty;
                        if (programEntityList.Count > 0)
                        {
                            strProgramName = programEntityList[0].ProgramName;
                            strShorName = programEntityList[0].ShortName;
                            propRecentShortName = programEntityList[0].ShortName;
                        }

                        lblApplicantDetails.Text = ApplicantName + " -- Intake from [" + item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString() + item.Year.ToString().Trim() + " - " + strProgramName + "] Application # " + item.App.ToString();

                        gvtNewValue.HeaderText = "Most Recent Intake Data       " + item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString() + item.Year.ToString().Trim() + "[" + strShorName + "]";

                        lblRecentHie.Text = item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString() + item.Year.ToString().Trim() + "[" + strShorName + "] Application # " + item.App.ToString();

                        string memberCode = string.Empty;
                        CommonEntity rel = Relation.Find(u => u.Code.Equals(item.MemberCode.ToString().Trim()));
                        if (rel != null) memberCode = rel.Desc;
                        string DOB = string.Empty;
                        if (item.AltBdate != string.Empty)
                            DOB = LookupDataAccess.Getdate(item.AltBdate.ToString().Trim());
                        //DataView dv = dtview.DefaultView;
                        //dv.RowFilter = "Fname = '" + dritem["PIP_FNAME"].ToString() + "' AND LName = '" + dritem["PIP_LNAME"].ToString() + "' AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";
                        CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == item.NameixFi.Trim().ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(item.AltBdate));
                        //dtview = dv.ToTable();
                        item.RecentMemberSwitch = "A";
                        LeanIntakeEntity leanintakedata;// = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "FALSE");
                        if (snpdata != null)
                        {
                            propCasemstEntity.OutOfService = strCurrentServices;


                            string membercurrentCode = string.Empty;
                            rel = Relation.Find(u => u.Code.Equals(snpdata.MemberCode.ToString().Trim()));
                            if (rel != null) membercurrentCode = rel.Desc;

                            List<CaseIncomeEntity> _propRecentIncomelist = propcaseIncomeEntity.FindAll(u => u.FamilySeq == item.FamilySeq);
                            string strIncomeType = string.Empty;
                            foreach (CaseIncomeEntity incomeitem in _propRecentIncomelist)
                            {
                                if (!strIncomeType.Contains(incomeitem.Type))
                                    strIncomeType = incomeitem.Type + "," + strIncomeType;
                            }
                            item.SnpIncomeTypes = strIncomeType;
                            leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "FALSE");
                            rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(snpdata.NameixFi.ToString().Trim(), snpdata.NameixMi.ToString(), snpdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, membercurrentCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode, string.Empty, snpdata.NameixFi, item.NameixFi, snpdata.NameixLast, item.NameixLast, DOB);
                            leanintakedata.PIP_MEMBER_TYPE = string.Empty;
                            gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdata;
                            gvwCustomer.Rows[rowIndex].Tag = leanintakedata;


                            gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                            gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";
                        }
                        else
                        {
                            // leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "TRUE");
                            // rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, "New Member", "N");


                            // leanintakedata.PIP_MEMBER_TYPE = "New Member";
                            //// leanintakedata.PIP_Valid_MODE = 1;
                            // gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = leanintakedata;


                            // //propLeanIntakeEntity.Add(leanintakedata);
                            // //rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, "New Member", "N");
                            // gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                            // gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                        }



                    }

                }
                foreach (CaseSnpEntity item in propCasesnpEntity)
                {
                    if (!propCasemstEntity.FamilySeq.Equals(item.FamilySeq))
                    {
                        string ApplicantName = LookupDataAccess.GetMemberName(item.NameixFi.ToString(), item.NameixMi.ToString(), item.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);
                        string memberCode = string.Empty;
                        CommonEntity rel = Relation.Find(u => u.Code.Equals(item.MemberCode.ToString().Trim()));
                        if (rel != null) memberCode = rel.Desc;
                        string DOB = string.Empty;
                        if (item.AltBdate != string.Empty)
                            DOB = LookupDataAccess.Getdate(item.AltBdate.ToString().Trim());
                        //DataView dv = dtview.DefaultView;
                        //dv.RowFilter = "Fname = '" + dritem["PIP_FNAME"].ToString() + "' AND LName = '" + dritem["PIP_LNAME"].ToString() + "' AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";
                        CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == item.NameixFi.Trim().ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(item.AltBdate));
                        //dtview = dv.ToTable();
                        LeanIntakeEntity leanintakedata;//= new Model.Objects.LeanIntakeEntity();// item, propCasemstEntity, "FALSE");

                        if (snpdata != null)
                        {

                            string memberrecentCode = string.Empty;
                            CommonEntity recentl = Relation.Find(u => u.Code.Equals(snpdata.MemberCode.ToString().Trim()));
                            if (recentl != null) memberrecentCode = recentl.Desc;
                            propCasemstEntity.OutOfService = strCurrentServices;

                            List<CaseIncomeEntity> _propRecentIncomelist = propcaseIncomeEntity.FindAll(u => u.FamilySeq == item.FamilySeq);
                            string strIncomeType = string.Empty;
                            foreach (CaseIncomeEntity incomeitem in _propRecentIncomelist)
                            {
                                if (!strIncomeType.Contains(incomeitem.Type))
                                    strIncomeType = incomeitem.Type + "," + strIncomeType;
                            }
                            item.SnpIncomeTypes = strIncomeType;

                            leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "FALSE");
                            rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(snpdata.NameixFi.ToString(), snpdata.NameixMi.ToString(), snpdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, memberrecentCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode, string.Empty, snpdata.NameixFi, item.NameixFi, snpdata.NameixLast, item.NameixLast, DOB);

                            gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdata;
                            gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                        }
                        else
                        {

                            propCasemstEntity.OutOfService = strCurrentServices;
                            List<CaseIncomeEntity> _propRecentIncomelist = propcaseIncomeEntity.FindAll(u => u.FamilySeq == item.FamilySeq);
                            string strIncomeType = string.Empty;
                            foreach (CaseIncomeEntity incomeitem in _propRecentIncomelist)
                            {
                                if (!strIncomeType.Contains(incomeitem.Type))
                                    strIncomeType = incomeitem.Type + "," + strIncomeType;
                            }
                            item.SnpIncomeTypes = strIncomeType;
                            leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "TRUE");
                            rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, memberCode, "N", "Add", ApplicantName, DOB, memberCode, string.Empty, item.NameixFi, item.NameixFi, item.NameixLast, item.NameixLast, DOB);
                            item.RecentMemberSwitch = "N";
                            leanintakedata.PIP_MEMBER_TYPE = "New Member";
                            leanintakedata.PIP_Valid_MODE = 1;
                            gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = leanintakedata;

                            //LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(dritem, "TRUE");
                            //leanintakedata.PIP_MEMBER_TYPE = "New Member";
                            //propLeanIntakeEntity.Add(leanintakedata);
                            //rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, "New Member", "N");
                            gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                            gvwCustomer.Rows[rowIndex].Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                            gvwCustomer.Rows[rowIndex].Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                            gvwCustomer.Rows[rowIndex].Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;
                            gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                            CaseSnpEntity snpdata1to1 = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == item.NameixFi.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(item.AltBdate));
                            if (snpdata1to1 != null)
                            {
                                gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = "Combine";
                                gvwCustomer.Rows[rowIndex].Cells["gvtCFirstName"].Value = snpdata1to1.NameixFi;
                                gvwCustomer.Rows[rowIndex].Cells["gvtCLastName"].Value = snpdata1to1.NameixLast;
                                gvwCustomer.Rows[rowIndex].Cells["gvtRDOB"].Value = LookupDataAccess.Getdate(snpdata1to1.AltBdate);
                            }
                            else
                            {
                                snpdata1to1 = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == item.NameixFi.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == item.NameixLast.Trim().ToUpper());
                                if (snpdata1to1 != null)
                                {
                                    gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = "Combine";
                                    gvwCustomer.Rows[rowIndex].Cells["gvtCFirstName"].Value = snpdata1to1.NameixFi;
                                    gvwCustomer.Rows[rowIndex].Cells["gvtCLastName"].Value = snpdata1to1.NameixLast;
                                    gvwCustomer.Rows[rowIndex].Cells["gvtRDOB"].Value = LookupDataAccess.Getdate(snpdata1to1.AltBdate);
                                }

                            }


                        }

                    }

                }
                foreach (CaseSnpEntity drsnpitem in BaseForm.BaseCaseSnpEntity)
                {
                    string ApplicantName = LookupDataAccess.GetMemberName(drsnpitem.NameixFi.ToString(), drsnpitem.NameixMi.ToString(), drsnpitem.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);

                    string memberrecentCode = string.Empty;
                    CommonEntity recentl = Relation.Find(u => u.Code.Equals(drsnpitem.MemberCode.ToString().Trim()));
                    if (recentl != null) memberrecentCode = recentl.Desc;

                    string DOB = string.Empty;
                    if (drsnpitem.AltBdate.ToString() != string.Empty)
                        DOB = LookupDataAccess.Getdate(drsnpitem.AltBdate.ToString().Trim());

                    CaseSnpEntity snpdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == drsnpitem.NameixFi.Trim().ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(drsnpitem.AltBdate));


                    if (snpdata != null)
                    {
                        //rowIndex = gvwCustomer.Rows.Add(ApplicantName, drsnpitem["PIP_SSN"].ToString(), DOB, memberCode, string.Empty);
                    }
                    else
                    {

                        // drsnpitem.SnpIncomeTypes = string.Empty;
                        LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(drsnpitem, BaseForm.BaseCaseMstListEntity[0], "TRUE");

                        rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem.Ssno.Trim()), DOB, memberrecentCode, "I", "Inactivate", "Not in this Household", string.Empty, string.Empty, string.Empty, drsnpitem.NameixFi.ToString(), drsnpitem.NameixFi, drsnpitem.NameixLast.ToString(), drsnpitem.NameixLast, DOB);
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        leanintakedata.PIP_MEMBER_TYPE = "Inactive Member";
                        leanintakedata.PIP_Valid_MODE = 1;
                        gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdata;
                        gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = string.Empty;

                        CaseSnpEntity snpdata1to1 = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == drsnpitem.NameixFi.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(drsnpitem.AltBdate));
                        if (snpdata1to1 != null)
                        {
                            gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = "Combine";
                            gvwCustomer.Rows[rowIndex].Cells["gvtRFirstName"].Value = snpdata1to1.NameixFi;
                            gvwCustomer.Rows[rowIndex].Cells["gvtRLastName"].Value = snpdata1to1.NameixLast;
                            gvwCustomer.Rows[rowIndex].Cells["gvtRDOB"].Value = LookupDataAccess.Getdate(snpdata1to1.AltBdate);
                        }
                        else
                        {
                            snpdata1to1 = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == drsnpitem.NameixFi.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == drsnpitem.NameixLast.Trim().ToUpper());
                            if (snpdata1to1 != null)
                            {
                                gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = "Combine";
                                gvwCustomer.Rows[rowIndex].Cells["gvtRFirstName"].Value = snpdata1to1.NameixFi;
                                gvwCustomer.Rows[rowIndex].Cells["gvtRLastName"].Value = snpdata1to1.NameixLast;
                                gvwCustomer.Rows[rowIndex].Cells["gvtRDOB"].Value = LookupDataAccess.Getdate(snpdata1to1.AltBdate);
                            }
                        }

                    }


                }

                if (gvwCustomer.Rows.Count > 0)
                {
                    gvwCustomer.Rows[0].Selected = true;
                    gvwCustomer.SelectionChanged += gvwCustomer_SelectionChanged;
                    gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
                }
            }
            else
            {


                LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;
                if (dritem != null)
                {
                    int intValidmode = 0;
                    if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "I" && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N")
                    {



                        foreach (DataGridViewRow gvrowitem in gvwCustomer.Rows)
                        {
                            bool boolValid = false;
                            LeanIntakeEntity leandata = gvrowitem.Tag as LeanIntakeEntity;
                            if (LookupDataAccess.Getdate(dritem.PIP_DOB) == LookupDataAccess.Getdate(leandata.PIP_DOB) && dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() == leandata.PIP_FNAME.Trim().ToUpper().FirstOrDefault())
                            {
                                boolValid = true;
                            }
                            else if (dritem.PIP_FNAME.Trim().ToUpper() == leandata.PIP_FNAME.Trim().ToUpper() && dritem.PIP_FNAME.Trim().ToUpper() == leandata.PIP_FNAME.Trim().ToUpper())
                            {
                                boolValid = true;
                            }
                            if (boolValid)
                            {
                                CaseSnpEntity snpitemdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == leandata.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(leandata.PIP_DOB));
                                if (snpitemdata == null)
                                    snpitemdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == leandata.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == leandata.PIP_LNAME.Trim().ToUpper());

                                List<CaseIncomeEntity> _propRecentIncomelist = propcaseIncomeEntity.FindAll(u => u.FamilySeq == snpitemdata.FamilySeq);
                                string strIncomeType = string.Empty;
                                foreach (CaseIncomeEntity incomeitem in _propRecentIncomelist)
                                {
                                    if (!strIncomeType.Contains(incomeitem.Type))
                                        strIncomeType = incomeitem.Type + "," + strIncomeType;
                                }
                                snpitemdata.SnpIncomeTypes = strIncomeType;
                                LeanIntakeEntity leanintakedata1 = new Model.Objects.LeanIntakeEntity(snpitemdata, propCasemstEntity, "TRUE");
                                // rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, memberCode, "N", "Add", ApplicantName, DOB, memberCode, string.Empty);
                                snpitemdata.RecentMemberSwitch = "N";
                                leanintakedata1.PIP_MEMBER_TYPE = "New Member";
                                leanintakedata1.PIP_Valid_MODE = 1;
                                gvrowitem.Cells["gvtSSN"].Tag = leanintakedata1;

                                gvrowitem.Cells["gvtRecentName"].Value = LookupDataAccess.GetMemberName(snpitemdata.NameixFi, snpitemdata.NameixMi, snpitemdata.NameixLast, BaseForm.BaseHierarchyCnFormat);
                                gvrowitem.Cells["gvtName"].Value = LookupDataAccess.GetMemberName(snpitemdata.NameixFi, snpitemdata.NameixMi, snpitemdata.NameixLast, BaseForm.BaseHierarchyCnFormat);
                                gvrowitem.Cells["gvtType"].Value = "N";
                                gvrowitem.Cells["gvtrecenttype"].Value = "Add";

                                gvrowitem.DefaultCellStyle.ForeColor = Color.Green;
                                gvrowitem.Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                                gvrowitem.Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                                gvrowitem.Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;
                                gvrowitem.Tag = leanintakedata1;

                                break;
                            }

                        }

                        CaseSnpEntity drsnpitem = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                        if (drsnpitem == null)
                            drsnpitem = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                        string ApplicantName = LookupDataAccess.GetMemberName(drsnpitem.NameixFi.ToString(), drsnpitem.NameixMi.ToString(), drsnpitem.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);

                        string memberrecentCode = string.Empty;
                        CommonEntity recentl = Relation.Find(u => u.Code.Equals(drsnpitem.MemberCode.ToString().Trim()));
                        if (recentl != null) memberrecentCode = recentl.Desc;

                        string DOB = string.Empty;
                        if (drsnpitem.AltBdate.ToString() != string.Empty)
                            DOB = LookupDataAccess.Getdate(drsnpitem.AltBdate.ToString().Trim());

                        CaseSnpEntity snpdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == drsnpitem.NameixFi.Trim().ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(drsnpitem.AltBdate));
                        LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(drsnpitem, BaseForm.BaseCaseMstListEntity[0], "TRUE");
                        int rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem.Ssno.Trim()), DOB, memberrecentCode, "I", "Inactivate", "Not in this Household");
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        leanintakedata.PIP_MEMBER_TYPE = "Inactive Member";
                        leanintakedata.PIP_Valid_MODE = 1;
                        gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdata;
                        gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = string.Empty;
                        CaseSnpEntity snpdata1to1 = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == drsnpitem.NameixFi.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(drsnpitem.AltBdate));
                        if (snpdata1to1 == null)
                            snpdata1to1 = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == drsnpitem.NameixFi.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == drsnpitem.NameixLast.Trim().ToUpper());

                        if (snpdata1to1 != null)
                        {
                            gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = "Combine";
                        }

                    }
                    else
                    {
                        foreach (DataGridViewRow item in gvwCustomer.Rows)
                        {
                            LeanIntakeEntity leandata = item.Tag as LeanIntakeEntity;
                            bool boolValid = false;
                            if (LookupDataAccess.Getdate(dritem.PIP_DOB) == LookupDataAccess.Getdate(leandata.PIP_DOB) && dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() == leandata.PIP_FNAME.Trim().ToUpper().FirstOrDefault())
                            {
                                boolValid = true;
                            }
                            else if (dritem.PIP_FNAME.Trim().ToUpper() == leandata.PIP_FNAME.Trim().ToUpper() && dritem.PIP_FNAME.Trim().ToUpper() == leandata.PIP_FNAME.Trim().ToUpper())
                            {
                                boolValid = true;
                            }
                            if (boolValid)
                            {
                                if (item.Cells["gvtType"].Value.ToString().Trim() == "N")
                                {

                                    CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == leandata.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(leandata.PIP_DOB));
                                    if (snpdata == null)
                                        snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == leandata.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == leandata.PIP_LNAME.Trim().ToUpper());


                                    LeanIntakeEntity leanintakedata;

                                    if (snpdata != null)
                                    {
                                        CaseSnpEntity snpitemdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == leandata.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(leandata.PIP_DOB));
                                        if (snpitemdata == null)
                                            snpitemdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == leandata.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == leandata.PIP_LNAME.Trim().ToUpper());

                                        string memberrecentCode = string.Empty;
                                        CommonEntity recentl = Relation.Find(u => u.Code.Equals(snpdata.MemberCode.ToString().Trim()));
                                        if (recentl != null) memberrecentCode = recentl.Desc;
                                        // propCasemstEntity.OutOfService = strCurrentServices;
                                        List<CaseIncomeEntity> _propRecentIncomelist = propcaseIncomeEntity.FindAll(u => u.FamilySeq == snpitemdata.FamilySeq);
                                        string strIncomeType = string.Empty;
                                        foreach (CaseIncomeEntity incomeitem in _propRecentIncomelist)
                                        {
                                            if (!strIncomeType.Contains(incomeitem.Type))
                                                strIncomeType = incomeitem.Type + "," + strIncomeType;
                                        }
                                        snpitemdata.SnpIncomeTypes = strIncomeType;
                                        leanintakedata = new Model.Objects.LeanIntakeEntity(snpitemdata, propCasemstEntity, "FALSE");
                                        // rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(snpdata.NameixFi.ToString(), snpdata.NameixMi.ToString(), snpdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, memberrecentCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode, string.Empty);


                                        item.Cells["gvtDOB"].Value = LookupDataAccess.Getdate(snpdata.AltBdate);
                                        item.DefaultCellStyle.ForeColor = Color.Black;
                                        item.Cells["gvtRecentName"].Value = LookupDataAccess.GetMemberName(snpitemdata.NameixFi, snpitemdata.NameixMi, snpitemdata.NameixLast, BaseForm.BaseHierarchyCnFormat);
                                        item.Cells["gvtName"].Value = LookupDataAccess.GetMemberName(snpdata.NameixFi, snpdata.NameixMi, snpdata.NameixLast, BaseForm.BaseHierarchyCnFormat);
                                        item.Cells["gvtType"].Value = string.Empty;
                                        item.Cells["gvtrecenttype"].Value = string.Empty;

                                        item.Cells["gvtSSN"].Tag = snpdata;
                                        item.Tag = leanintakedata;
                                        item.Cells["gvtAppType"].Value = "separate";
                                    }

                                }
                                if (item.Cells["gvtType"].Value.ToString().Trim() == "I")
                                {
                                    gvwCustomer.Rows.Remove(item);
                                    gvwCustomer.Rows[0].Selected = true;
                                }
                            }
                        }

                    }

                    if (gvwCustomer.Rows.Count > 0)
                        gvwCustomer.Rows[0].Selected = true;

                    gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
                }



                //gvwCustomer.SelectionChanged -= gvwCustomer_SelectionChanged;
                //gvwCustomer.Rows.Clear();

                //int rowIndex = 0;
                //DataSet dsservice = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", propAgency, propDept, propProgram, propYear, propAppl);
                //DataTable dtservice = dsservice.Tables[0];

                //string strCurrentServices = string.Empty;
                //foreach (DataRow dritem in dtservice.Rows)
                //{
                //    if (strCurrentServices != string.Empty)
                //    {
                //        strCurrentServices = strCurrentServices + "," + dritem["INQ_CODE"].ToString().Trim();
                //    }
                //    else
                //    {
                //        strCurrentServices = dritem["INQ_CODE"].ToString().Trim();
                //    }

                //}

                //foreach (CaseSnpEntity item in propCasesnpEntity)
                //{
                //    if (propCasemstEntity.FamilySeq.Equals(item.FamilySeq))
                //    {
                //        string ApplicantName = LookupDataAccess.GetMemberName(item.NameixFi.ToString(), item.NameixMi.ToString(), item.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);

                //        List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString());
                //        string strProgramName = string.Empty;
                //        string strShorName = string.Empty;
                //        if (programEntityList.Count > 0)
                //        {
                //            strProgramName = programEntityList[0].ProgramName;
                //            strShorName = programEntityList[0].ShortName;
                //        }

                //        lblApplicantDetails.Text = ApplicantName + " -- Intake from [" + item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString() + item.Year.ToString().Trim() + " - " + strProgramName + "] Application # " + item.App.ToString();

                //        gvtNewValue.HeaderText = "Most Recent Intake Data       " + item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString() + item.Year.ToString().Trim() + "[" + strShorName + "]";

                //        lblRecentHie.Text = item.Agency.ToString() + item.Dept.ToString() + item.Program.ToString() + item.Year.ToString().Trim() + "[" + strShorName + "]";

                //        string memberCode = string.Empty;
                //        CommonEntity rel = Relation.Find(u => u.Code.Equals(item.MemberCode.ToString().Trim()));
                //        if (rel != null) memberCode = rel.Desc;
                //        string DOB = string.Empty;
                //        if (item.AltBdate != string.Empty)
                //            DOB = LookupDataAccess.Getdate(item.AltBdate.ToString().Trim());
                //        //DataView dv = dtview.DefaultView;
                //        //dv.RowFilter = "Fname = '" + dritem["PIP_FNAME"].ToString() + "' AND LName = '" + dritem["PIP_LNAME"].ToString() + "' AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";
                //        CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == item.NameixFi.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(item.AltBdate));
                //        //dtview = dv.ToTable();
                //        item.RecentMemberSwitch = "A";
                //        LeanIntakeEntity leanintakedata;// = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "FALSE");
                //        if (snpdata != null)
                //        {
                //            propCasemstEntity.OutOfService = strCurrentServices;
                //            leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "FALSE");
                //            rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(snpdata.NameixFi.ToString().Trim(), snpdata.NameixMi.ToString(), snpdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, memberCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode,string.Empty);
                //            leanintakedata.PIP_MEMBER_TYPE = string.Empty;
                //            gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdata;
                //            gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                //        }
                //        else
                //        {
                //            // leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "TRUE");
                //            // rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, "New Member", "N");


                //            // leanintakedata.PIP_MEMBER_TYPE = "New Member";
                //            //// leanintakedata.PIP_Valid_MODE = 1;
                //            // gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = leanintakedata;


                //            // //propLeanIntakeEntity.Add(leanintakedata);
                //            // //rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, "New Member", "N");
                //            // gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                //            // gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                //        }

                //        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //        gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";

                //    }

                //}
                //foreach (CaseSnpEntity item in propCasesnpEntity)
                //{
                //    if (!propCasemstEntity.FamilySeq.Equals(item.FamilySeq))
                //    {
                //        string ApplicantName = LookupDataAccess.GetMemberName(item.NameixFi.ToString(), item.NameixMi.ToString(), item.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);
                //        string memberCode = string.Empty;
                //        CommonEntity rel = Relation.Find(u => u.Code.Equals(item.MemberCode.ToString().Trim()));
                //        if (rel != null) memberCode = rel.Desc;
                //        string DOB = string.Empty;
                //        if (item.AltBdate != string.Empty)
                //            DOB = LookupDataAccess.Getdate(item.AltBdate.ToString().Trim());
                //        //DataView dv = dtview.DefaultView;
                //        //dv.RowFilter = "Fname = '" + dritem["PIP_FNAME"].ToString() + "' AND LName = '" + dritem["PIP_LNAME"].ToString() + "' AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";
                //        CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == item.NameixFi.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(item.AltBdate));
                //        //dtview = dv.ToTable();
                //        LeanIntakeEntity leanintakedata;//= new Model.Objects.LeanIntakeEntity();// item, propCasemstEntity, "FALSE");

                //        if (snpdata != null)
                //        {

                //            string memberrecentCode = string.Empty;
                //            CommonEntity recentl = Relation.Find(u => u.Code.Equals(snpdata.MemberCode.ToString().Trim()));
                //            if (recentl != null) memberrecentCode = recentl.Desc;
                //            propCasemstEntity.OutOfService = strCurrentServices;
                //            leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "FALSE");
                //            rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(snpdata.NameixFi.ToString(), snpdata.NameixMi.ToString(), snpdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, memberrecentCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode,string.Empty);

                //            gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdata;
                //            gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                //        }
                //        else
                //        {
                //            propCasemstEntity.OutOfService = strCurrentServices;
                //            leanintakedata = new Model.Objects.LeanIntakeEntity(item, propCasemstEntity, "TRUE");
                //            rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno.ToString().Trim()), DOB, memberCode, "N", "Add", ApplicantName, DOB, memberCode,string.Empty);
                //            item.RecentMemberSwitch = "N";
                //            leanintakedata.PIP_MEMBER_TYPE = "New Member";
                //            leanintakedata.PIP_Valid_MODE = 1;
                //            gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = leanintakedata;

                //            //LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(dritem, "TRUE");
                //            //leanintakedata.PIP_MEMBER_TYPE = "New Member";
                //            //propLeanIntakeEntity.Add(leanintakedata);
                //            //rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, "New Member", "N");
                //            gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                //            gvwCustomer.Rows[rowIndex].Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                //            gvwCustomer.Rows[rowIndex].Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                //            gvwCustomer.Rows[rowIndex].Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;
                //            gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                //        }

                //    }

                //}
                //foreach (CaseSnpEntity drsnpitem in BaseForm.BaseCaseSnpEntity)
                //{
                //    string ApplicantName = LookupDataAccess.GetMemberName(drsnpitem.NameixFi.ToString(), drsnpitem.NameixMi.ToString(), drsnpitem.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat);

                //    string memberrecentCode = string.Empty;
                //    CommonEntity recentl = Relation.Find(u => u.Code.Equals(drsnpitem.MemberCode.ToString().Trim()));
                //    if (recentl != null) memberrecentCode = recentl.Desc;

                //    string DOB = string.Empty;
                //    if (drsnpitem.AltBdate.ToString() != string.Empty)
                //        DOB = LookupDataAccess.Getdate(drsnpitem.AltBdate.ToString().Trim());

                //    CaseSnpEntity snpdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == drsnpitem.NameixFi.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(drsnpitem.AltBdate));


                //    if (snpdata != null)
                //    {
                //        //rowIndex = gvwCustomer.Rows.Add(ApplicantName, drsnpitem["PIP_SSN"].ToString(), DOB, memberCode, string.Empty);
                //    }
                //    else
                //    {
                //        LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(drsnpitem, BaseForm.BaseCaseMstListEntity[0], "TRUE");

                //        rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem.Ssno.Trim()), DOB, memberrecentCode, "I", "Inactivate", "Not in this Household");
                //        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //        leanintakedata.PIP_MEMBER_TYPE = "Inactive Member";
                //        leanintakedata.PIP_Valid_MODE = 1;
                //        gvwCustomer.Rows[rowIndex].Tag = leanintakedata;
                //        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = snpdata;
                //        gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = string.Empty;
                //    }


                //}

                //if (gvwCustomer.Rows.Count > 0)
                //{
                //    gvwCustomer.SelectionChanged += gvwCustomer_SelectionChanged;
                //    gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
                //}

            }
        }

        public void PIPCompareGridLoad(string strGridType)
        {
            if (strGridType == string.Empty)
            {
                int rowIndex = 0;
                foreach (DataRow dritem in _propLeanIntakedt.Rows)
                {
                    DataTable dtview = _propSnpDatatable;
                    propLeanUserId = dritem["PIP_REG_ID"].ToString();
                    string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);
                    string memberCode = string.Empty;
                    CommonEntity rel = Relation.Find(u => u.Code.Equals(dritem["PIP_MEMBER_CODE"].ToString().Trim()));
                    if (rel != null) memberCode = rel.Desc;
                    string DOB = string.Empty;
                    if (dritem["PIP_DOB"].ToString() != string.Empty)
                        DOB = LookupDataAccess.Getdate(dritem["PIP_DOB"].ToString().Trim());
                    DataView dv = dtview.DefaultView;
                    dv.RowFilter = "Fname = '" + dritem["PIP_FNAME"].ToString() + "'  AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";

                    // dv.RowFilter = "Fname LIKE '" + dritem["PIP_FNAME"].ToString().FirstOrDefault() + "*'  AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";

                    dtview = dv.ToTable();
                    if (dtview.Rows.Count > 0)
                    {
                        string memberRecentCode = string.Empty;
                        CommonEntity rel1 = Relation.Find(u => u.Code.Equals(dtview.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim()));
                        if (rel1 != null) memberRecentCode = rel1.Desc;

                        propLeanIntakeEntity.Add(new Model.Objects.LeanIntakeEntity(dritem));
                        rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(dtview.Rows[0]["Fname"].ToString(), dtview.Rows[0]["Mname"].ToString(), dtview.Rows[0]["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, memberRecentCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode);
                        gvwCustomer.Rows[rowIndex].Tag = new Model.Objects.LeanIntakeEntity(dritem);
                        CaseSnpEntity SnpCaptainMemEntity = propCasesnpEntity.Find(u => u.NameixFi.ToUpper().FirstOrDefault() == dritem["PIP_FNAME"].ToString().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem["PIP_DOB"].ToString()));
                        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = SnpCaptainMemEntity;

                    }
                    else
                    {
                        LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(dritem, "TRUE");
                        leanintakedata.PIP_MEMBER_TYPE = "New Member";
                        propLeanIntakeEntity.Add(leanintakedata);
                        rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, memberCode, "N", "Add", ApplicantName, DOB, memberCode);
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                        gvwCustomer.Rows[rowIndex].Tag = leanintakedata; gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                        gvwCustomer.Rows[rowIndex].Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                        gvwCustomer.Rows[rowIndex].Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                        gvwCustomer.Rows[rowIndex].Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;

                        dv.RowFilter = "Fname LIKE '" + dritem["PIP_FNAME"].ToString().FirstOrDefault() + "*'  AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";
                        dtview = dv.ToTable();
                        if (dtview.Rows.Count > 0)
                        {
                            gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = "Combine";

                        }

                    }


                    if (dritem["PIP_FAM_SEQ"].ToString() == "0")
                    {
                        propLeanServices = dritem["PIP_SERVICES"].ToString();
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";
                    }
                }
                foreach (DataRow drsnpitem in _propSnpDatatable.Rows)
                {
                    DataTable dtview = _propLeanIntakedt;
                    string ApplicantName = LookupDataAccess.GetMemberName(drsnpitem["FNAME"].ToString(), drsnpitem["MNAME"].ToString(), drsnpitem["LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);

                    if (drsnpitem["App_Mem_SW"].ToString().Trim() == "A")
                    {

                        List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString());
                        string strProgramName = string.Empty;
                        string strShortName = string.Empty;
                        if (programEntityList.Count > 0)
                        {
                            strProgramName = programEntityList[0].ProgramName;
                            strShortName = programEntityList[0].ShortName;
                        }

                        lblApplicantDetails.Text = ApplicantName + " -- Intake from [" + drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString() + drsnpitem["SNP_YEAR"].ToString().Trim() + " - " + strProgramName + "] Application # " + drsnpitem["SNP_APP"].ToString();

                        lblCurrentHie.Text = drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString() + drsnpitem["SNP_YEAR"].ToString().Trim() + " - " + strShortName;
                    }

                    string memberCode = string.Empty;
                    CommonEntity rel = Relation.Find(u => u.Code.Equals(drsnpitem["SNP_MEMBER_CODE"].ToString().Trim()));
                    if (rel != null) memberCode = rel.Desc;
                    string DOB = string.Empty;
                    if (drsnpitem["DOB"].ToString() != string.Empty)
                        DOB = LookupDataAccess.Getdate(drsnpitem["DOB"].ToString().Trim());
                    DataView dv = dtview.DefaultView;
                    dv.RowFilter = "PIP_FNAME = '" + drsnpitem["FNAME"].ToString() + "' AND PIP_DOB = '" + drsnpitem["DOB"].ToString() + "'";

                    // dv.RowFilter = "PIP_FNAME LIKE '" + drsnpitem["FNAME"].ToString().FirstOrDefault() + "*' AND PIP_DOB = '" + drsnpitem["DOB"].ToString() + "'";

                    dtview = dv.ToTable();
                    if (dtview.Rows.Count > 0)
                    {
                        //rowIndex = gvwCustomer.Rows.Add(ApplicantName, drsnpitem["PIP_SSN"].ToString(), DOB, memberCode, string.Empty);
                    }
                    else
                    {
                        LeanIntakeEntity leanintakeEntitydata = new Model.Objects.LeanIntakeEntity();
                        leanintakeEntitydata.PIP_SEQ = drsnpitem["FAMSEQ"].ToString();
                        leanintakeEntitydata.PIP_FNAME = drsnpitem["FNAME"].ToString();
                        leanintakeEntitydata.PIP_LNAME = drsnpitem["LNAME"].ToString();
                        leanintakeEntitydata.PIP_MEMBER_TYPE = "Inactive Member";
                        leanintakeEntitydata.PIP_Valid_MODE = 1;
                        rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem["SSN"].ToString().Trim()), DOB, memberCode, "I", "Inactivate", "Not in this Household");
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        gvwCustomer.Rows[rowIndex].Tag = leanintakeEntitydata;
                        dv.RowFilter = "PIP_FNAME LIKE '" + drsnpitem["FNAME"].ToString().FirstOrDefault() + "*' AND PIP_DOB = '" + drsnpitem["DOB"].ToString() + "'";

                        dtview = dv.ToTable();
                        if (dtview.Rows.Count > 0)
                        {
                            gvwCustomer.Rows[rowIndex].Cells["gvtAppType"].Value = "Combine";
                        }


                    }


                }
                if (gvwCustomer.Rows.Count > 0)
                {
                    gvwCustomer.Rows[0].Selected = true;
                    gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
                }


            }
            else
            {




                //LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;
                //if (dritem != null)
                //{
                //    int intValidmode = 0;
                //    if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "I" && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N")
                //    {

                //        foreach (DataGridViewRow gvrowitem in gvwCustomer.Rows)
                //        {
                //            LeanIntakeEntity leandata = gvrowitem.Tag as LeanIntakeEntity;
                //            if (LookupDataAccess.Getdate(dritem.PIP_DOB) == LookupDataAccess.Getdate(leandata.PIP_DOB) && dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() == leandata.PIP_FNAME.Trim().ToUpper().FirstOrDefault())
                //            {


                //            }
                //        }
                //    }
                //}


                //foreach (DataRow dritem in Leanintakedt.Rows)
                //{
                //    DataTable dtview = snpdatadt;
                //    propLeanUserId = dritem["PIP_REG_ID"].ToString();
                //    string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);
                //    string memberCode = string.Empty;
                //    CommonEntity rel = Relation.Find(u => u.Code.Equals(dritem["PIP_MEMBER_CODE"].ToString().Trim()));
                //    if (rel != null) memberCode = rel.Desc;
                //    string DOB = string.Empty;
                //    if (dritem["PIP_DOB"].ToString() != string.Empty)
                //        DOB = LookupDataAccess.Getdate(dritem["PIP_DOB"].ToString().Trim());
                //    DataView dv = dtview.DefaultView;
                //    dv.RowFilter = "Fname LIKE '" + dritem["PIP_FNAME"].ToString().FirstOrDefault() + "*'  AND DOB = '" + dritem["PIP_DOB"].ToString() + "'";

                //    dtview = dv.ToTable();
                //    if (dtview.Rows.Count > 0)
                //    {
                //        string memberRecentCode = string.Empty;
                //        CommonEntity rel1 = Relation.Find(u => u.Code.Equals(dtview.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim()));
                //        if (rel1 != null) memberRecentCode = rel1.Desc;

                //        propLeanIntakeEntity.Add(new Model.Objects.LeanIntakeEntity(dritem));
                //        rowIndex = gvwCustomer.Rows.Add(false, LookupDataAccess.GetMemberName(dtview.Rows[0]["Fname"].ToString(), dtview.Rows[0]["Mname"].ToString(), dtview.Rows[0]["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat), LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, memberRecentCode, string.Empty, string.Empty, ApplicantName, DOB, memberCode);
                //        gvwCustomer.Rows[rowIndex].Tag = new Model.Objects.LeanIntakeEntity(dritem);
                //        CaseSnpEntity SnpCaptainMemEntity = propMainCasesnpEntity.Find(u => u.NameixFi.ToUpper().FirstOrDefault() == dritem["PIP_FNAME"].ToString().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem["PIP_DOB"].ToString()));
                //        gvwCustomer.Rows[rowIndex].Cells["gvtSSN"].Tag = SnpCaptainMemEntity;

                //    }
                //    else
                //    {
                //        LeanIntakeEntity leanintakedata = new Model.Objects.LeanIntakeEntity(dritem, "TRUE");
                //        leanintakedata.PIP_MEMBER_TYPE = "New Member";
                //        propLeanIntakeEntity.Add(leanintakedata);
                //        rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(dritem["PIP_SSN"].ToString().Trim()), DOB, memberCode, "N", "Add", ApplicantName, DOB, memberCode);
                //        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                //        gvwCustomer.Rows[rowIndex].Tag = leanintakedata; gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                //        gvwCustomer.Rows[rowIndex].Cells["gvtrecentDOB"].Style.ForeColor = Color.Black;
                //        gvwCustomer.Rows[rowIndex].Cells["gvtRecentName"].Style.ForeColor = Color.Black;
                //        gvwCustomer.Rows[rowIndex].Cells["gvtrecentRelation"].Style.ForeColor = Color.Black;

                //    }


                //    if (dritem["PIP_FAM_SEQ"].ToString() == "0")
                //    {
                //        propLeanServices = dritem["PIP_SERVICES"].ToString();
                //        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //        gvwCustomer.Rows[rowIndex].Cells["gvtType"].Value = "A";
                //    }
                //}
                //foreach (DataRow drsnpitem in snpdatadt.Rows)
                //{
                //    DataTable dtview = Leanintakedt;
                //    string ApplicantName = LookupDataAccess.GetMemberName(drsnpitem["FNAME"].ToString(), drsnpitem["MNAME"].ToString(), drsnpitem["LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);

                //    if (drsnpitem["App_Mem_SW"].ToString().Trim() == "A")
                //    {

                //        List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString());
                //        string strProgramName = string.Empty;
                //        string strShortName = string.Empty;
                //        if (programEntityList.Count > 0)
                //        {
                //            strProgramName = programEntityList[0].ProgramName;
                //            strShortName = programEntityList[0].ShortName;
                //        }

                //        lblApplicantDetails.Text = ApplicantName + " -- Intake from [" + drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString() + drsnpitem["SNP_YEAR"].ToString().Trim() + " - " + strProgramName + "] Application # " + drsnpitem["SNP_APP"].ToString();

                //        lblCurrentHie.Text = drsnpitem["SNP_AGENCY"].ToString() + drsnpitem["SNP_DEPT"].ToString() + drsnpitem["SNP_PROGRAM"].ToString() + drsnpitem["SNP_YEAR"].ToString().Trim() + " - " + strShortName;
                //    }

                //    string memberCode = string.Empty;
                //    CommonEntity rel = Relation.Find(u => u.Code.Equals(drsnpitem["SNP_MEMBER_CODE"].ToString().Trim()));
                //    if (rel != null) memberCode = rel.Desc;
                //    string DOB = string.Empty;
                //    if (drsnpitem["DOB"].ToString() != string.Empty)
                //        DOB = LookupDataAccess.Getdate(drsnpitem["DOB"].ToString().Trim());
                //    DataView dv = dtview.DefaultView;
                //    dv.RowFilter = "PIP_FNAME LIKE '" + drsnpitem["FNAME"].ToString().FirstOrDefault() + "*' AND PIP_DOB = '" + drsnpitem["DOB"].ToString() + "'";

                //    dtview = dv.ToTable();
                //    if (dtview.Rows.Count > 0)
                //    {
                //        //rowIndex = gvwCustomer.Rows.Add(ApplicantName, drsnpitem["PIP_SSN"].ToString(), DOB, memberCode, string.Empty);
                //    }
                //    else
                //    {
                //        LeanIntakeEntity leanintakeEntitydata = new Model.Objects.LeanIntakeEntity();
                //        leanintakeEntitydata.PIP_SEQ = drsnpitem["FAMSEQ"].ToString();
                //        leanintakeEntitydata.PIP_FNAME = drsnpitem["FNAME"].ToString();
                //        leanintakeEntitydata.PIP_LNAME = drsnpitem["LNAME"].ToString();
                //        leanintakeEntitydata.PIP_MEMBER_TYPE = "Inactive Member";
                //        leanintakeEntitydata.PIP_Valid_MODE = 1;
                //        rowIndex = gvwCustomer.Rows.Add(false, ApplicantName, LookupDataAccess.GetPhoneSsnNoFormat(drsnpitem["SSN"].ToString().Trim()), DOB, memberCode, "I", "Inactivate", "Not in this Household");
                //        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //        gvwCustomer.Rows[rowIndex].Tag = leanintakeEntitydata;
                //    }


                //}
                //if (gvwCustomer.Rows.Count > 0)
                //    gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());


            }
        }

        public string _propCloseFormStatus { get; set; }
        public BaseForm BaseForm { get; set; }
        public DataTable _propSnpDatatable { get; set; }
        public DataTable _propLeanIntakedt { get; set; }

        public DataTable _propDSSXMLIntakedt { get; set; }


        public string propHierchy;
        public string propAgency;
        public string propDept;
        public string propProgram;
        public string propYear;
        public string propAppl;
        public string propLeanUserId;
        public string propLeanServices = string.Empty;

        List<LIHPMQuesEntity> CAPLihpmQueslist { get; set; }
        public static CaseMstEntity propCasemstEntity { get; set; }
        public List<CaseSnpEntity> propCasesnpEntity { get; set; }
        public CaseDiffEntity CapLandlordInfoEntity { get; set; }
        public static CaseDiffEntity CapCaseDiffMailDetails { get; set; }

        public CaseMstEntity propMainCasemstEntity { get; set; }
        public List<CaseSnpEntity> propMainCasesnpEntity { get; set; }
        public List<CaseIncomeEntity> propcaseIncomeEntity { get; set; }
        public List<LeanIntakeEntity> propLeanIntakeEntity;
        public string strFormType { get; set; }

        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        public bool _AddressDetsTABFlag = false;
        public bool _SupplierTABFlag = false;
        public bool _PerfMeasureTABFlag = false;
        public string _hasVerifiedFlag = "";
        bool chkDSSTabsVerify(List<DataGridViewRow> chckdRows)
        {
            //gvwCustomer.SelectedRows[0].Cells["gvchkCrs"].Value.ToString().Trim() == "Y"
            bool isVerified = false;

            if (chckdRows[0].Cells["gvtType"].Value.ToString() =="A")
            {
                DataRow drApp = chckdRows[0].Tag as DataRow;
                if (drApp["Is_Primary_Applicant__c"].ToString().ToUpper() == "TRUE")
                {
                    if (rbtnDetailAddressTab.Checked && rbtnPerfMesureTab.Checked && rbtnSupplierTab.Checked)
                        isVerified = true;
                }
                else
                {
                    isVerified = true;
                }
            }
            else
            {
                isVerified = true;
            }
            return isVerified;
        }

        private void btnLeanData_Click(object sender, EventArgs e)
        {
            if (strFormType == "DSSXML")
            {
                List<DataGridViewRow> chckdRows = gvwCustomer.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvcTopSelect"].Value.ToString().ToUpper() == "TRUE").ToList();
                if (chckdRows.Count > 0)
                {
                    if (chkDSSTabsVerify(chckdRows))
                    {
                        string _chkMessage = checkDssValues();
                        if (_chkMessage == "")
                        {
                            List<DataGridViewRow> greenRows = chckdRows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtType"].Value.ToString().ToUpper() == "N").ToList();
                            List<DataGridViewRow> RedRows = chckdRows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtType"].Value.ToString().ToUpper() == "I").ToList();
                            if (chckdRows.Count == 1)
                            {
                                if (greenRows.Count == 1)
                                {
                                    MessageBox.Show("Are you sure you want to add " + greenRows[0].Cells["gvtName"].Value.ToString() + " to App# " + propAppl + "?",
                                   Consts.Common.ApplicationCaption + " - Add New Member", MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerDSSXML);
                                }
                                else if (RedRows.Count == 1)
                                {
                                    MessageBox.Show("Are you sure you want to Delete Existing Member " + RedRows[0].Cells["gvtRecentName"].Value.ToString() + " in App# " + propAppl + "?",
                                   Consts.Common.ApplicationCaption + " - Add New Member", MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerDSSXML);
                                }
                                else
                                {
                                    MessageBox.Show("Are you sure you want to update the selected fields with the DSS Intake Values for this household member in " + propAgency + "-" + propDept + "-" + propProgram + " " + propYear + " " + propAppl + "?",
                                    Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerDSSXML);
                                }
                                //else if (greenRows.Count > 1)
                                //{
                                //    MessageBox.Show("Are you sure you want to add the selected members to App# " + propAppl + "?",
                                //  Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerDSSXML);
                                //}
                            }
                            else
                            {
                                MessageBox.Show("Are you sure you want to update the selected fields with the DSS Intake Values for this household member in " + propAgency + "-" + propDept + "-" + propProgram + " " + propYear + " " + propAppl + "?",
                                Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerDSSXML);
                            }
                        }
                        else
                        {
                            MessageBox.Show(_chkMessage, "CAPTAIN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please verify the Detail Address, Supplier & Performance Measures fields.", "CAPTAIN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    AlertBox.Show("Please check-off atleast one member", MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (btnLeanData.Text == "Connect")
                {

                    MessageBox.Show("Are you sure you want to Connect the CAPTAIN ?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxConnectHandler);
                }
                else
                {
                    List<DataGridViewRow> SelectedgvRows = (from c in gvwCustomer.Rows.Cast<DataGridViewRow>().ToList()
                                                            where (((DataGridViewCheckBoxCell)c.Cells["gvcTopSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                            select c).ToList();

                    if (SelectedgvRows.Count > 0)
                    {
                        bool boolstatus = true;
                        foreach (DataGridViewRow dritems in SelectedgvRows)
                        {
                            LeanIntakeEntity dritem = dritems.Tag as LeanIntakeEntity;
                            if (dritem != null)
                            {

                                if (dritems.Cells["gvtType"].Value.ToString() != "I")
                                {

                                    if (dritem.PIP_Valid_MODE <= 0)
                                    {

                                        DataTable dtview = _propSnpDatatable;
                                        DataView dv = dtview.DefaultView;
                                        dv.RowFilter = "Fname LIKE '" + dritem.PIP_FNAME.FirstOrDefault() + "*' AND DOB = '" + dritem.PIP_DOB.ToString() + "'";

                                        dtview = dv.ToTable();

                                        if (dtview.Rows.Count == 0)
                                        {
                                            dv = dtview.DefaultView;
                                            dv.RowFilter = "Fname = '" + dritem.PIP_FNAME.ToString() + "' AND LName = '" + dritem.PIP_LNAME.ToString() + "'";
                                            dtview = dv.ToTable();
                                        }
                                        if (dtview.Rows.Count > 0)
                                        {
                                            if (dritem.PIP_SSN.ToString().Trim() != string.Empty)
                                            {
                                                if (dritem.PIP_SSN.ToString().Trim() != dtview.Rows[0]["SSN"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_GENDER.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_GENDER.ToString().Trim() != dtview.Rows[0]["SNP_SEX"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_MARITAL_STATUS.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_MARITAL_STATUS.ToString().Trim() != dtview.Rows[0]["SNP_MARITAL_STATUS"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_RELATIONSHIP.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_RELATIONSHIP.ToString().Trim() != dtview.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_ETHNIC.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_ETHNIC.ToString().Trim() != dtview.Rows[0]["SNP_ETHNIC"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_RACE.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_RACE.ToString().Trim() != dtview.Rows[0]["SNP_RACE"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_EDUCATION.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_EDUCATION.ToString().Trim() != dtview.Rows[0]["SNP_EDUCATION"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_DISABLE.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_DISABLE.ToString().Trim() != dtview.Rows[0]["SNP_DISABLE"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_WORK_STAT.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_WORK_STAT.ToString().Trim() != dtview.Rows[0]["SNP_WORK_STAT"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_SCHOOL.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_SCHOOL.ToString().Trim() != dtview.Rows[0]["SNP_SCHOOL_DISTRICT"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_HEALTH_INS.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_HEALTH_INS.ToString().Trim() != dtview.Rows[0]["SNP_HEALTH_INS"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_VETERAN.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_VETERAN.ToString().Trim() != dtview.Rows[0]["SNP_VET"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_FOOD_STAMP.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_FOOD_STAMP.ToString().Trim() != dtview.Rows[0]["SNP_FOOD_STAMPS"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_FARMER.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_FARMER.ToString().Trim() != dtview.Rows[0]["SNP_FARMER"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_WIC.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_WIC.ToString().Trim() != dtview.Rows[0]["SNP_WIC"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_RELITRAN.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_RELITRAN.ToString().Trim() != dtview.Rows[0]["SNP_RELITRAN"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_DRVLIC.ToString().Trim() != string.Empty)
                                            {


                                                if (dritem.PIP_DRVLIC.ToString().Trim() != dtview.Rows[0]["SNP_DRVLIC"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_MILITARY_STATUS.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_MILITARY_STATUS.ToString().Trim() != dtview.Rows[0]["SNP_MILITARY_STATUS"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_PREGNANT.ToString().Trim() != string.Empty)
                                            {

                                                if (dritem.PIP_PREGNANT.ToString().Trim() != dtview.Rows[0]["SNP_PREGNANT"].ToString().Trim())
                                                {
                                                    boolstatus = false;
                                                }
                                            }

                                            if (dritem.PIP_HEALTH_CODES.ToString().Trim() != string.Empty)
                                            {


                                                if (dritem.PIP_HEALTH_CODES.ToString().Trim().ToUpper() != dtview.Rows[0]["SNP_HEALTH_CODES"].ToString().Trim().ToUpper())
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (dritem.PIP_NCASHBEN.ToString().Trim() != string.Empty)
                                            {
                                                if (dritem.PIP_NCASHBEN.ToString().Trim().ToUpper() != dtview.Rows[0]["SNP_NCASHBEN"].ToString().Trim().ToUpper())
                                                {
                                                    boolstatus = false;
                                                }
                                            }

                                            if (dritems.Cells["gvtType"].Value.ToString() == "A")
                                            {
                                                if (dritem.PIP_PRI_LANGUAGE.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_PRI_LANGUAGE.ToString().Trim() != dtview.Rows[0]["MST_LANGUAGE"].ToString().Trim())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_FAMILY_TYPE.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_FAMILY_TYPE.ToString().Trim() != dtview.Rows[0]["MST_FAMILY_TYPE"].ToString().Trim())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_HOUSING.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_HOUSING.ToString().Trim() != dtview.Rows[0]["MST_HOUSING"].ToString().Trim())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_HOME_PHONE.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_AREA.ToString().Trim() + dritem.PIP_HOME_PHONE.ToString().Trim() != (dtview.Rows[0]["MST_AREA"].ToString().Trim() + dtview.Rows[0]["MST_PHONE"].ToString().Trim()))
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_CELL_NUMBER.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_CELL_NUMBER.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_CELL_PHONE"].ToString().Trim().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_HOUSENO.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_HOUSENO.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_HN"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_DIRECTION.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_DIRECTION.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_DIRECTION"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }

                                                if (dritem.PIP_STREET.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_STREET.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_STREET"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }

                                                if (dritem.PIP_SUFFIX.ToString().Trim() != string.Empty)
                                                {
                                                    if (dritem.PIP_SUFFIX.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_SUFFIX"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_APT.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_APT.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_APT"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_FLR.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_FLR.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_FLR"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_CITY.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_CITY.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_CITY"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_STATE.ToString().Trim() != string.Empty)
                                                {
                                                    if (dritem.PIP_STATE.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_STATE"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_ZIP.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_ZIP.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_ZIP"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }

                                                if (dritem.PIP_TOWNSHIP.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_TOWNSHIP.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_TOWNSHIP"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                                if (dritem.PIP_COUNTY.ToString().Trim() != string.Empty)
                                                {

                                                    if (dritem.PIP_COUNTY.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_COUNTY"].ToString().ToUpper())
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                            }
                                            //if (dritem.PIP_YOUTH.ToString().Trim() != string.Empty)
                                            //{

                                            //    if (dritem.PIP_YOUTH.ToString().Trim() != dtview.Rows[0]["SNP_YOUTH"].ToString().Trim())
                                            //    {
                                            //        boolstatus = false;
                                            //    }
                                            //}

                                            //if (dritem.PIP_INCOME_TYPES.ToString().Trim() != string.Empty)
                                            //{
                                            //    if (dritem.PIP_INCOME_TYPES.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_INCOME_TYPES"].ToString().Trim().ToUpper())
                                            //    {
                                            //        boolstatus = false;
                                            //    }
                                            //}

                                            if (dritem.PIP_SERVICES != string.Empty)
                                            {
                                                int intpipservicecount = 0;
                                                foreach (ListItem strserv in dritem.Pip_service_list_Mode)
                                                {
                                                    if (strserv.Value.ToString().ToUpper() == "FALSE")
                                                    {

                                                        intpipservicecount = intpipservicecount + 1;
                                                    }
                                                }
                                                if (dritem.Pip_service_list_Mode.Count == intpipservicecount)
                                                {
                                                    boolstatus = false;
                                                }
                                            }
                                            if (strFormType != string.Empty)
                                            {
                                                if (dritem.PIP_INCOME_TYPES.ToString().Trim() != string.Empty)
                                                {
                                                    int intpipincomecount = 0;
                                                    foreach (ListItem strserv in dritem.Pip_Income_list_Mode)
                                                    {
                                                        if (strserv.Value.ToString().ToUpper() == "FALSE")
                                                        {

                                                            intpipincomecount = intpipincomecount + 1;
                                                        }
                                                    }
                                                    if (dritem.Pip_Income_list_Mode.Count == intpipincomecount)
                                                    {
                                                        boolstatus = false;
                                                    }
                                                }
                                            }

                                        }
                                        if (!boolstatus)
                                            CommonFunctions.MessageBoxDisplay("Please select atleast one field " + dritem.PIP_FNAME + " " + dritem.PIP_LNAME.ToString());
                                        break;
                                    }

                                }
                            }
                        }
                        if (boolstatus)
                        {
                            if (strFormType == string.Empty)
                            {
                                MessageBox.Show("Are you sure you want to Update client Intake data " + propHierchy + " with data from PIP ?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                            }
                            else
                            {
                                MessageBox.Show("Are you sure you want to Update client Intake data " + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear == string.Empty ? "    " : BaseForm.BaseYear) + BaseForm.BaseApplicationNo + " with data from " + propHierchy, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                            }
                        }
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("Please select atleast one Applicant or Member in top grid");
                    }
                }
            }
        }
        private string strApplNo;
        private string strClientIdOut;
        private string strFamilyIdOut;
        private string strSSNNOOut;
        private string strErrorMsg;

        private void MessageBoxConnectHandler(DialogResult dialogResult)
        {

            if (dialogResult == DialogResult.Yes)
            {
                int i = 0;
                foreach (DataGridViewRow dritem in gvwCustomer.Rows)
                {
                    LeanIntakeEntity dr = dritem.Tag as LeanIntakeEntity;
                    //CaseSnpEntity SnpEntity = propCasesnpEntity.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);


                    CaseSnpEntity SnpEntity = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dr.PIP_FNAME.Trim().ToUpper().FirstOrDefault() &&
                    LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                    if (SnpEntity == null)
                        SnpEntity = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dr.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dr.PIP_LNAME.Trim().ToUpper());


                    PIPDATA.InsertUpdatePIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, SnpEntity.Agency, SnpEntity.Dept, SnpEntity.Program,
                        SnpEntity.Year, SnpEntity.App, SnpEntity.FamilySeq, dr.PIP_AGENCY, dr.PIP_AGY.ToString(), dr.PIP_REG_ID.ToString(), dr.PIP_ID.ToString(), BaseForm.UserID, "A", string.Empty);

                    if (i == 0)
                        UpdateLeanIntakeData(dr.PIP_REG_ID.ToString(), dr.PIP_ID, SnpEntity.Agency, SnpEntity.Dept, SnpEntity.Program, SnpEntity.Year, SnpEntity.App, "C");

                    i++;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
                //AlertBox.Show();
            }
        }

        public string xmlApplMembID = "";
        private void MessageBoxHandlerDSSXML(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                bool _saveFlag = false;
                string _DelFlag = "N";
                List<DataGridViewRow> chckdRows = gvwCustomer.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvcTopSelect"].Value.ToString().ToUpper() == "TRUE").ToList();
                if (chckdRows.Count > 0)
                {
                    foreach (DataGridViewRow _drCust in chckdRows)
                    {
                        DataRow drApp = _drCust.Tag as DataRow;
                        CaseSnpEntity caseSnpEntity = _drCust.Cells["gvtSSN"].Tag as CaseSnpEntity;
                        //CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dr.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                        //if (SnpEntity == null)
                        //    SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dr.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dr.PIP_LNAME.Trim().ToUpper());
                        string CAPAppno = propAppl;

                        if (drApp != null)
                        {

                            if (_drCust.Cells["gvtType"].Value.ToString() == "I")
                            {
                                if (propMainCasemstEntity.FamilySeq == caseSnpEntity.FamilySeq)
                                {

                                    MessageBox.Show("Selected Member is a CAPTAIN Applicant. Please change the CAPTAIN Applicant with DSSXML Applicant first.");
                                }

                                else
                                {

                                    bool boolDeleteObo = true;
                                    bool boolDeleteCAObo = true;
                                    CaseSnpEntity caseSnp = caseSnpEntity; // GetSelectedRow();
                                    CASEACTEntity search_Act_Details = new CASEACTEntity(true);
                                    CASEMSEntity Search_MS_Details = new CASEMSEntity(true);
                                    Search_MS_Details.Agency = caseSnp.Agency;
                                    Search_MS_Details.Dept = caseSnp.Dept;
                                    Search_MS_Details.Program = caseSnp.Program;
                                    Search_MS_Details.Year = caseSnp.Year;                              // Year will be always Four-Spaces in CASEMS
                                    Search_MS_Details.App_no = caseSnp.App;

                                    search_Act_Details.Agency = caseSnp.Agency;
                                    search_Act_Details.Dept = caseSnp.Dept;
                                    search_Act_Details.Program = caseSnp.Program;
                                    search_Act_Details.Year = caseSnp.Year;                              // Year will be always Four-Spaces in CASEMS
                                    search_Act_Details.App_no = caseSnp.App;

                                    List<CASEACTEntity> Tmp_SP_ACT_Details = _model.SPAdminData.Browse_CASEACT(search_Act_Details, "Browse");
                                    if (Tmp_SP_ACT_Details.Count > 0)
                                    {
                                        foreach (CASEACTEntity Actitem in Tmp_SP_ACT_Details)
                                        {
                                            CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                            Search_CAOBO_Entity.ID = Actitem.ACT_ID;
                                            Search_CAOBO_Entity.Fam_Seq = caseSnp.FamilySeq;
                                            Search_CAOBO_Entity.Seq = Search_CAOBO_Entity.CLID = string.Empty;

                                            List<CAOBOEntity> CAOBO_List = _model.SPAdminData.Browse_CAOBO(Search_CAOBO_Entity, "Browse");
                                            if (CAOBO_List.Count > 0)
                                                boolDeleteCAObo = false;
                                        }

                                    }

                                    List<CASEMSEntity> Tmp_SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_MS_Details, "Browse");
                                    if (Tmp_SP_MS_Details.Count > 0)
                                    {
                                        foreach (CASEMSEntity Msitem in Tmp_SP_MS_Details)
                                        {
                                            CASEMSOBOEntity Search_OBO_Entity = new CASEMSOBOEntity();
                                            Search_OBO_Entity.ID = Msitem.ID;
                                            Search_OBO_Entity.Fam_Seq = caseSnp.FamilySeq;
                                            Search_OBO_Entity.Seq = Search_OBO_Entity.CLID = string.Empty;

                                            List<CASEMSOBOEntity> CASEMSOBO_List = _model.SPAdminData.Browse_CASEMSOBO(Search_OBO_Entity, "Browse");
                                            if (CASEMSOBO_List.Count > 0)
                                                boolDeleteObo = false;
                                        }

                                    }

                                    if (boolDeleteCAObo)
                                    {
                                        if (boolDeleteObo)
                                        {
                                            string strMsg = _model.CaseMstData.DeleteCaseSNP(caseSnpEntity, BaseForm.UserID);
                                            if (strMsg == "Success")
                                            {

                                                ////propMainCasesnpEntity
                                                for (int i = _propSnpDatatable.Rows.Count - 1; i >= 0; i--)
                                                {
                                                    DataRow dr = _propSnpDatatable.Rows[i];
                                                    if (
                                                     //dr["NameixFi"].ToString() == caseSnpEntity.NameixFi && dr["NameixLast"].ToString() == caseSnpEntity.NameixLast    && dr["AltBDate"].ToString() == caseSnpEntity.AltBdate &&
                                                     dr["FamilySeq"].ToString() == caseSnpEntity.FamilySeq)
                                                    {
                                                        dr.Delete();
                                                    }
                                                }
                                                _propSnpDatatable.AcceptChanges();

                                                _DelFlag = "Y";
                                                _saveFlag = true;
                                            }
                                            else if (strMsg == "Dependency")
                                            {
                                                MessageBox.Show("You cant delete this member, as there are Dependents", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "CASEACT")
                                            {
                                                MessageBox.Show("'You can't delete this member, as there are CA Postings", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "CASEMS")
                                            {
                                                MessageBox.Show("'You can't delete this member, as there are MS Postings", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "CASESPM")
                                            {
                                                MessageBox.Show("'You can't delete this member, as there are Service Plan Master", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "CASECONT")
                                            {
                                                MessageBox.Show("'You can't delete this member, as there are Postings in CASECONT", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "LIHEAPV")
                                            {
                                                MessageBox.Show("'You can't delete this member, as there are vendors in the Supplier Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "LIHEAPB")
                                            {
                                                MessageBox.Show("You cant delete this member, as there are records in Control Card - Benefit Maintenance Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "CHLDMST")
                                            {
                                                MessageBox.Show("You can't delete this member, as there are records in Medical/Emergency Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "CASEENRL")
                                            {
                                                MessageBox.Show("You cant delete this member, as there are records in Enrollment/Withdrawals Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "MATASMT")
                                            {
                                                MessageBox.Show("You cant delete this member, as there are records in Matrix Assessment Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (strMsg == "EMSRES")
                                            {
                                                MessageBox.Show("You cant delete this member, as there are records in Budget Resource Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("You cant delete this member, as there are MS Postings and OBO", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("You cant delete this member, as there are CA Postings and OBO", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            else if (_drCust.Cells["gvtType"].Value.ToString() == "N")
                            {
                                XML_SNP_TYPE ocaseSNP = new XML_SNP_TYPE();
                                if (drApp["Is_Primary_Applicant__c"].ToString() == "true")
                                {
                                    XML_MST_TYPE ocaseMST = new XML_MST_TYPE();

                                    string tmpSSN1 = string.Empty;
                                    if (!string.IsNullOrEmpty(drApp["SSN__c"].ToString().Trim()))
                                    {
                                        tmpSSN1 = drApp["SSN__c"].ToString().Trim().Replace("-", "");
                                    }

                                    if (drApp["SSNmode"].ToString() == "TRUE")
                                    {
                                        ocaseMST.XML_MST_SSN = tmpSSN1;
                                    }
                                    else { ocaseMST.XML_MST_SSN = caseSnpEntity.Ssno.ToString(); }

                                    #region House Number & Street
                                    //string strValue = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["Address_Line_1__c"].ToString();
                                    //string HouseNumber = "";
                                    //string Street = "";

                                    //if (strValue != "")
                                    //{
                                    //    int firstSpaceIndex = strValue.IndexOf(" ");
                                    //    /*******************************************************/
                                    //    string strtempValue1 = "";
                                    //    if (firstSpaceIndex > -1)
                                    //        strtempValue1 = strValue.Substring(0, firstSpaceIndex);

                                    //    int result;
                                    //    bool isNumeric = int.TryParse(strtempValue1, out result);
                                    //    if (isNumeric)
                                    //    {
                                    //        HouseNumber = strtempValue1;
                                    //        Street = strValue.Substring(firstSpaceIndex + 1);
                                    //    }
                                    //    else
                                    //    {
                                    //        Street = strValue;
                                    //    }

                                    //    if (Street.Length > 25)
                                    //        Street = Street.Substring(0, 24);

                                    //    /*******************************************************/

                                    //}

                                    //if (drApp["HOUSENOmode"].ToString() == "TRUE") { ocaseMST.XML_MST_HN = HouseNumber; } else { ocaseMST.XML_MST_HN = propMainCasemstEntity.Hn.ToString().Trim().ToUpper(); }
                                    //if (drApp["STREETmode"].ToString() == "TRUE") { ocaseMST.XML_MST_STREET = Street; } else { ocaseMST.XML_MST_STREET = propMainCasemstEntity.Street.ToString().Trim().ToUpper(); }
                                    #endregion

                                    #region ZIP & ZIP Code
                                    string _ZIP = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["AddressZip_Code__c"].ToString();
                                    decimal MST_ZIP = 0;
                                    decimal MST_ZIP_PLUS = 0;
                                    if (_ZIP.ToString() != string.Empty)
                                    {
                                        string[] strArgs = null;

                                        if (_ZIP.Contains("-"))
                                            strArgs = _ZIP.Split('-');
                                        else if (_ZIP.Contains(" "))
                                            strArgs = _ZIP.Split(' ');
                                        else
                                            strArgs = _ZIP.Split('-');

                                        string Zipcode = "";
                                        string ZipPlus = "0";
                                        Zipcode = strArgs[0].ToString();
                                        if (strArgs.Length > 1)
                                            ZipPlus = strArgs[1].ToString();

                                        if (Zipcode.Length > 5)
                                        {
                                            Zipcode = Zipcode.Substring(0, 5);
                                            if (Zipcode.Length == 9)
                                                ZipPlus = Zipcode.Substring(5, 4);
                                        }

                                        MST_ZIP = Convert.ToDecimal(Zipcode);
                                        MST_ZIP_PLUS = Convert.ToDecimal(ZipPlus);

                                        if (drApp["ZIPmode"].ToString() == "TRUE") { ocaseMST.XML_MST_ZIP = MST_ZIP; } else { ocaseMST.XML_MST_ZIP = Convert.ToDecimal(propMainCasemstEntity.Zip.ToString() == string.Empty ? "0" : propMainCasemstEntity.Zip.ToString().PadLeft(5, '0')); }
                                        if (drApp["ZIPCODEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_ZIPPLUS = MST_ZIP_PLUS; } else { ocaseMST.XML_MST_ZIPPLUS = Convert.ToDecimal(propMainCasemstEntity.Zipplus.ToString() == string.Empty ? "0" : propMainCasemstEntity.Zipplus.ToString().PadLeft(4, '0')); }
                                    }
                                    #endregion

                                    #region State
                                    string State = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["State__c"].ToString();
                                    if (drApp["STATEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_STATE = State; } else { ocaseMST.XML_MST_STATE = propMainCasemstEntity.State.ToString(); }
                                    #endregion

                                    #region City
                                    string City = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["City__c"].ToString();
                                    if (drApp["CITYmode"].ToString() == "TRUE") { ocaseMST.XML_MST_CITY = City; } else { ocaseMST.XML_MST_CITY = propMainCasemstEntity.City.ToString(); }
                                    #endregion

                                    #region Email
                                    string Email = drApp["Email__c"].ToString().Trim();
                                    if (drApp["Emailmode"].ToString() == "TRUE") { ocaseMST.XML_MST_EMAIL = Email; } else { ocaseMST.XML_MST_EMAIL = propMainCasemstEntity.Email.ToString(); }
                                    #endregion

                                    #region Phone & Cell

                                    if (!string.IsNullOrEmpty(propMainCasemstEntity.Phone.ToString().Trim()) && !string.IsNullOrEmpty(propMainCasemstEntity.Area.ToString()))
                                    {
                                        ocaseMST.XML_MST_PHONE = propMainCasemstEntity.Phone.ToString();
                                        ocaseMST.XML_MST_AREA = propMainCasemstEntity.Area.ToString();
                                    }
                                    if (!string.IsNullOrEmpty(propMainCasemstEntity.CellPhone.ToString().Trim()))
                                        ocaseMST.XML_MST_CELL_PHONE = propMainCasemstEntity.CellPhone.ToString();

                                    string _phone = drApp["Primary_Phone__c"].ToString();
                                    string MST_area = string.Empty;
                                    string MST_phone = string.Empty;
                                    if (_phone.ToString() != string.Empty)
                                    {
                                        _phone = _phone.Replace("-", "");
                                        if (drApp["Primary_Phone_Type__c"].ToString() == "Home")
                                        {
                                            string _area = "";
                                            string _phn = "";

                                            if (_phone.Length == 10)
                                            {
                                                _area = _phone.Substring(0, 3);
                                                if (_phone.Length == 10)
                                                    _phn = _phone.Substring(3, 7);
                                            }
                                            else
                                            {

                                            }

                                            MST_area = _area;
                                            MST_phone = _phn;

                                            string Phone = MST_area + MST_phone;

                                            if (drApp["PHONEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_PHONE = MST_phone; ocaseMST.XML_MST_AREA = MST_area; }
                                            else
                                            {
                                                ocaseMST.XML_MST_PHONE = propMainCasemstEntity.Phone.ToString();
                                                ocaseMST.XML_MST_AREA = propMainCasemstEntity.Area.ToString();
                                            }
                                        }
                                        else if (drApp["Primary_Phone_Type__c"].ToString() == "Cell")
                                        {
                                            //ocaseMST.XML_MST_CELL_PHONE = _phone;

                                            if (drApp["CELLmode"].ToString() == "TRUE") { ocaseMST.XML_MST_CELL_PHONE = _phone; }
                                            else
                                            {
                                                ocaseMST.XML_MST_CELL_PHONE = propMainCasemstEntity.CellPhone.ToString();
                                            }
                                        }
                                    }

                                    #endregion

                                    #region Primary Langugae

                                    string xmlPrimaryLang = TMS00141Form.getLanguageCode(drApp["Primary_Language__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                                    string CAPprimaryLang = propMainCasemstEntity.Language.ToString().Trim();
                                    if (drApp["LANGUAGEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_LANGUAGE = xmlPrimaryLang; } else { ocaseMST.XML_MST_LANGUAGE = propMainCasemstEntity.Language.ToString(); }
                                    #endregion

                                    #region Housing Situation
                                    string xmlHousingsSit = TMS00141Form.getHousingCode(_dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim());

                                    string CAPHousingSit = propMainCasemstEntity.Housing.ToString().Trim();
                                    if (drApp["HOUSINGmode"].ToString() == "TRUE") { ocaseMST.XML_MST_HOUSING = xmlHousingsSit; } else { ocaseMST.XML_MST_HOUSING = propMainCasemstEntity.Housing.ToString(); }
                                    #endregion

                                    #region Rent/Mortgage
                                    string xmlHousingsSitDesc = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim();
                                    string xmlRentMortgaeg = "0.00";
                                    if (xmlHousingsSitDesc.ToLower().Contains("rent"))
                                        xmlRentMortgaeg = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Mortgage_Portion_Amount__c"].ToString().Trim();
                                    else if (xmlHousingsSitDesc.ToLower().Contains("own"))
                                        xmlRentMortgaeg = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Monthly_Payment_for_Mortgage__c"].ToString().Trim();

                                    if (xmlRentMortgaeg == "")
                                        xmlRentMortgaeg = "0.00";

                                    //string CAPprimaryLang = propMainCasemstEntity.Language.ToString().Trim();
                                    if (drApp["RENTMORTmode"].ToString() == "TRUE") { ocaseMST.XML_MST_EXP_RENT = Convert.ToDecimal(xmlRentMortgaeg); } else { ocaseMST.XML_MST_EXP_RENT = Convert.ToDecimal(propMainCasemstEntity.ExpRent); }
                                    #endregion

                                    #region Dwelling Type
                                    string xmlDwellingType = TMS00141Form.getDwellingTypeCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Type_Of_Home_To_Live__c"].ToString().Trim());
                                    string CAPDwellingType = propMainCasemstEntity.Dwelling.ToString().Trim();
                                    if (drApp["DWELLINGMode"].ToString() == "TRUE") { ocaseMST.XML_MST_DWELLING = xmlDwellingType; } else { ocaseMST.XML_MST_DWELLING = CAPDwellingType.ToString(); }
                                    #endregion


                                    #region Primary Method of Paying for Heat
                                    string xmlPPMOPayHeat = TMS00141Form.getHeatIncCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_Method_for_Heat__c"].ToString().Trim());
                                    string CAPPPMOPayHeat = propMainCasemstEntity.HeatIncRent.ToString().Trim();
                                    if (drApp["PRIMPAYHEATMode"].ToString() == "TRUE") { ocaseMST.XML_MST_HEAT_INC_RENT = xmlPPMOPayHeat; } else { ocaseMST.XML_MST_HEAT_INC_RENT = CAPPPMOPayHeat.ToString(); }
                                    #endregion

                                    #region Primary Source of Heat
                                    string xmlPrimeSource = TMS00141Form.getPrimarySourceCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim());
                                    string CAPPrimeSource = propMainCasemstEntity.Source.ToString().Trim();
                                    if (drApp["PRIMSRCHEARMode"].ToString() == "TRUE") { ocaseMST.XML_MST_SOURCE = xmlPrimeSource; } else { ocaseMST.XML_MST_SOURCE = CAPPrimeSource.ToString(); }
                                    #endregion

                                    ocaseMST.XML_MST_FAMILY_SEQ = 1;
                                    ocaseMST.XML_MST_APP_NO = CAPAppno;
                                    ocaseMST.XML_MST_LSTC_OPERATOR_1 = BaseForm.UserID;

                                    //ocaseMST = putLIHEAP(ocaseMST, drApp);

                                    #region INSERT MST RECORDS
                                    string _strRespoutMST = "";
                                    List<XML_MST_TYPE> MstList = new List<XML_MST_TYPE>();
                                    MstList.Add(ocaseMST);
                                    DataTable dtmst = new DataTable();
                                    dtmst = TMS00141Form.ToDataTable(MstList);
                                    string _xmlMEMID = drApp["HouseholdMemberSystemID_c"].ToString();

                                    _strRespoutMST = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram, propYear,
                                         _xmlAppID, _xmlMEMID, dtmst, null, null, null, null, null, null, "UPDDSSMST");

                                    //string[] xFamDetails = _strRespoutMST.Split('|');
                                    //string strFamilyID = xFamDetails[0].ToString().Trim();
                                    //string strAppno = xFamDetails[1].ToString().Trim();
                                    //string strClientID = xFamDetails[2].ToString().Trim();

                                    ocaseSNP.XML_SNP_SS_BIC = "Y";
                                    #endregion


                                    #region LANDLORD Records DATA Building
                                    DataTable dtLLRRec = new DataTable();
                                    dtLLRRec = _dsAllDSSXML.Tables["LandlordSection"];

                                    if (dtLLRRec.Rows.Count > 0)
                                    {
                                        XML_LLR_TYPE ocaseLLR = new XML_LLR_TYPE();
                                        ocaseLLR.XML_LLR_AGENCY = propAgency;
                                        ocaseLLR.XML_LLR_DEPT = propDept;
                                        ocaseLLR.XML_LLR_PROGRAM = propProgram;
                                        ocaseLLR.XML_LLR_YEAR = propYear;
                                        ocaseLLR.XML_LLR_APP_NO = CAPAppno;

                                        if (dtLLRRec.Columns.Contains("Landlord_Company_Name__c"))
                                        {
                                            string xmlLLRFname = "", xmlLLRLName = "";
                                            if (!string.IsNullOrEmpty(dtLLRRec.Rows[0]["Landlord_Company_Name__c"].ToString().Trim()))
                                            {
                                                string[] LLRNAME = dtLLRRec.Rows[0]["Landlord_Company_Name__c"].ToString().Trim().Split(' ');

                                                xmlLLRFname = LLRNAME[0];
                                                if (LLRNAME.Length > 1)
                                                    xmlLLRLName = LLRNAME[1];
                                            }

                                            string CAPLLRFname = CapLandlordInfoEntity.IncareFirst.ToString().Trim().ToUpper();
                                            if (drApp["LLRFnameMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_FIRST_NAME = xmlLLRFname; } else { ocaseLLR.XML_LLR_FIRST_NAME = CAPLLRFname; }

                                            string CAPLLRLname = CapLandlordInfoEntity.IncareLast.ToString().Trim().ToUpper();
                                            if (drApp["LLRLnameMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_LAST_NAME = xmlLLRLName; } else { ocaseLLR.XML_LLR_LAST_NAME = CAPLLRLname; }

                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_Company_Phone__c"))
                                        {
                                            string xmlLLRPhone = dtLLRRec.Rows[0]["Landlord_Company_Phone__c"].ToString().Trim().Replace("-", "");
                                            string CAPLLRPhone = CapLandlordInfoEntity.Phone.ToString().Trim().ToUpper();
                                            if (drApp["LLRPhoneMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_PHONE = xmlLLRPhone; } else { ocaseLLR.XML_LLR_PHONE = CAPLLRPhone; }
                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_City__c"))
                                        {
                                            string xmlLLRCity = dtLLRRec.Rows[0]["Landlord_City__c"].ToString().Trim();
                                            string CAPLLRCity = CapLandlordInfoEntity.City.ToString().Trim().ToUpper();

                                            if (drApp["LLRCityMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_CITY = xmlLLRCity; } else { ocaseLLR.XML_LLR_CITY = CAPLLRCity; }
                                        }
                                        if (dtLLRRec.Columns.Contains("Landlord_State__c"))
                                        {
                                            string xmlLLRState = dtLLRRec.Rows[0]["Landlord_State__c"].ToString().Trim();
                                            string CAPLLRState = CapLandlordInfoEntity.State.ToString().Trim();

                                            if (drApp["LLRStateMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_STATE = xmlLLRState; } else { ocaseLLR.XML_LLR_STATE = CAPLLRState; }
                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_Company_Address__c"))
                                        {
                                            string strValue1 = dtLLRRec.Rows[0]["Landlord_Company_Address__c"].ToString();
                                            string firstPart1 = "";
                                            string secondPart1 = "";

                                            if (strValue1 != "")
                                            {
                                                int firstSpaceIndex = strValue1.IndexOf(" ");
                                                /*******************************************************/
                                                string strtempValue1 = "";
                                                if (firstSpaceIndex > -1)
                                                    strtempValue1 = strValue1.Substring(0, firstSpaceIndex);

                                                int result;
                                                bool isNumeric = int.TryParse(strtempValue1, out result);
                                                if (isNumeric)
                                                {
                                                    firstPart1 = strtempValue1;
                                                    secondPart1 = strValue1.Substring(firstSpaceIndex + 1);
                                                }
                                                else
                                                {
                                                    secondPart1 = strValue1;
                                                }

                                                /*******************************************************/
                                                //   string strtempValue2 = strValue.Substring(firstSpaceIndex + 1);

                                            }

                                            string xmlStreet = secondPart1;
                                            string xmlHNo = firstPart1;

                                            string CAPStreet = CapLandlordInfoEntity.Street.ToString().Trim().ToUpper();
                                            string CAPHNo = CapLandlordInfoEntity.Hn.ToString().Trim();

                                            if (drApp["LLRStreetMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_STREET = xmlStreet; } else { ocaseLLR.XML_LLR_STREET = CAPStreet; }
                                            if (drApp["LLRHouseNoMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_HN = xmlHNo; } else { ocaseLLR.XML_LLR_HN = CAPHNo; }

                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_AddressZip_Code__c"))
                                        {
                                            string _LLRZIP = dtLLRRec.Rows[0]["Landlord_AddressZip_Code__c"].ToString();
                                            decimal LLR_ZIP = 0;
                                            decimal LLR_ZIP_PLUS = 0;
                                            if (_LLRZIP.ToString() != string.Empty)
                                            {
                                                string[] strArgs = null;

                                                if (_LLRZIP.Contains("-"))
                                                    strArgs = _LLRZIP.Split('-');
                                                else if (_LLRZIP.Contains(" "))
                                                    strArgs = _LLRZIP.Split(' ');
                                                else
                                                    strArgs = _LLRZIP.Split('-');

                                                string Zipcode = "";
                                                string ZipPlus = "0";
                                                Zipcode = strArgs[0].ToString();
                                                if (strArgs.Length > 1)
                                                    ZipPlus = strArgs[1].ToString();

                                                if (Zipcode.Length > 5)
                                                {
                                                    Zipcode = Zipcode.Substring(0, 5);
                                                    if (Zipcode.Length == 9)
                                                        ZipPlus = Zipcode.Substring(5, 4);
                                                }

                                                LLR_ZIP = Convert.ToDecimal(Zipcode);
                                                LLR_ZIP_PLUS = Convert.ToDecimal(ZipPlus);
                                            }

                                            int xmlLLR_ZIP = Convert.ToInt32(LLR_ZIP);
                                            int xmlLLR_ZIPPLUS = Convert.ToInt32(LLR_ZIP_PLUS);

                                            string CAPLLRZIP = CapLandlordInfoEntity.Zip.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.Zip.ToString().Trim().PadLeft(5, '0');
                                            string CAPLLRZIPPlus = CapLandlordInfoEntity.ZipPlus.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.ZipPlus.ToString().Trim().PadLeft(4, '0');

                                            if (drApp["LLRZipMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_ZIP = xmlLLR_ZIP; } else { ocaseLLR.XML_LLR_ZIP = Convert.ToInt32(CAPLLRZIP); }
                                            if (drApp["LLRZipCodeMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_ZIPPLUS = xmlLLR_ZIPPLUS; } else { ocaseLLR.XML_LLR_ZIPPLUS = Convert.ToInt32(CAPLLRZIPPlus); }
                                        }

                                        ocaseLLR.XML_LLR_ADD_OPERATOR = BaseForm.UserID;
                                        ocaseLLR.XML_LLR_LSTC_OPERATOR = BaseForm.UserID;

                                        #region INSERT LLR RECORDS

                                        List<XML_LLR_TYPE> LLRList = new List<XML_LLR_TYPE>();
                                        LLRList.Add(ocaseLLR);

                                        DataTable dtllr = new DataTable();
                                        dtllr = TMS00141Form.ToDataTable(LLRList);

                                        string _strRespoutLLR = "";
                                        // string _xmlMEMID = drApp["HouseholdMemberSystemID_c"].ToString();

                                        _strRespoutLLR = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram,
                                            propYear, _xmlAppID, _xmlMEMID, null, null, null, null, dtllr, null, null, "UPDDSSLLR");


                                        #endregion

                                    }

                                    #endregion
                                    #region Mailing Address Stopped
                                    //string isMailingAddress = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Mailing_Address_different_than_Home_Add__c"].ToString().Trim();
                                    //if (isMailingAddress.Trim().ToString().ToUpper() == "YES")
                                    //{
                                    //    #region CASEDIFF Records DATA Building
                                    //    DataTable dtDIFF = new DataTable();
                                    //    dtDIFF = _dsAllDSSXML.Tables["MAILINGDETAILS"];

                                    //    if (dtDIFF.Rows.Count > 0)
                                    //    {

                                    //        XML_DIFF_TYPE ocaseDiff = new XML_DIFF_TYPE();
                                    //        ocaseDiff.XML_DIFF_AGENCY = propAgency;
                                    //        ocaseDiff.XML_DIFF_DEPT = propDept;
                                    //        ocaseDiff.XML_DIFF_PROGRAM = propProgram;
                                    //        ocaseDiff.XML_DIFF_YEAR = propYear;
                                    //        ocaseDiff.XML_DIFF_APP_NO = CAPAppno;

                                    //        if (dtDIFF.Columns.Contains("City__c"))
                                    //        {
                                    //            string xmlDiffCity = dtDIFF.Rows[0]["City__c"].ToString().Trim();
                                    //            string CAPDiffCity = CapCaseDiffMailDetails.City.ToString().Trim().ToUpper();
                                    //            if (drApp["DiffCityMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_CITY = xmlDiffCity; } else { ocaseDiff.XML_DIFF_CITY = CAPDiffCity; }
                                    //        }
                                    //        if (dtDIFF.Columns.Contains("State__c"))
                                    //        {
                                    //            string xmlDiffSate = dtDIFF.Rows[0]["State__c"].ToString().Trim();
                                    //            string CAPdiffSate = CapCaseDiffMailDetails.State.ToString().Trim().ToUpper();
                                    //            if (drApp["DiffSateMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_STATE = xmlDiffSate; } else { ocaseDiff.XML_DIFF_STATE = CAPdiffSate; }
                                    //        }

                                    //        if (dtDIFF.Columns.Contains("Address_Line_1__c"))
                                    //        {
                                    //            string strValue1 = dtDIFF.Rows[0]["Address_Line_1__c"].ToString();
                                    //            string firstPart1 = "";
                                    //            string secondPart1 = "";

                                    //            if (strValue1 != "")
                                    //            {
                                    //                int firstSpaceIndex = strValue1.IndexOf(" ");
                                    //                /*******************************************************/
                                    //                string strtempValue1 = "";
                                    //                if (firstSpaceIndex > -1)
                                    //                    strtempValue1 = strValue1.Substring(0, firstSpaceIndex);

                                    //                int result;
                                    //                bool isNumeric = int.TryParse(strtempValue1, out result);
                                    //                if (isNumeric)
                                    //                {
                                    //                    firstPart1 = strtempValue1;
                                    //                    secondPart1 = strValue1.Substring(firstSpaceIndex + 1);
                                    //                }
                                    //                else
                                    //                {
                                    //                    secondPart1 = strValue1;
                                    //                }

                                    //                /*******************************************************/
                                    //                //   string strtempValue2 = strValue.Substring(firstSpaceIndex + 1);

                                    //            }

                                    //            string xmlStreet = secondPart1;
                                    //            string xmlHNo = firstPart1;    //split street and add HN
                                    //            string CAPStreet = CapCaseDiffMailDetails.Street.ToString().Trim();
                                    //            string CAPHN = CapCaseDiffMailDetails.Hn.ToString().Trim();


                                    //            if (drApp["DiffStreetMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_STREET = xmlStreet; } else { ocaseDiff.XML_DIFF_STREET = CAPStreet; }
                                    //            if (drApp["DiffHNMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_HN = xmlHNo; } else { ocaseDiff.XML_DIFF_HN = CAPHN; }
                                    //        }

                                    //        if (dtDIFF.Columns.Contains("AddressZip_Code__c"))
                                    //        {
                                    //            string _LLRZIP = dtDIFF.Rows[0]["AddressZip_Code__c"].ToString();
                                    //            decimal LLR_ZIP = 0;
                                    //            decimal LLR_ZIP_PLUS = 0;
                                    //            if (_LLRZIP.ToString() != string.Empty)
                                    //            {
                                    //                string[] strArgs = null;

                                    //                if (_LLRZIP.Contains("-"))
                                    //                    strArgs = _LLRZIP.Split('-');
                                    //                else if (_LLRZIP.Contains(" "))
                                    //                    strArgs = _LLRZIP.Split(' ');
                                    //                else
                                    //                    strArgs = _LLRZIP.Split('-');

                                    //                string Zipcode = "";
                                    //                string ZipPlus = "0";
                                    //                Zipcode = strArgs[0].ToString();
                                    //                if (strArgs.Length > 1)
                                    //                    ZipPlus = strArgs[1].ToString();

                                    //                if (Zipcode.Length > 5)
                                    //                {
                                    //                    Zipcode = Zipcode.Substring(0, 5);
                                    //                    if (Zipcode.Length == 9)
                                    //                        ZipPlus = Zipcode.Substring(5, 4);
                                    //                }

                                    //                LLR_ZIP = Convert.ToDecimal(Zipcode);
                                    //                LLR_ZIP_PLUS = Convert.ToDecimal(ZipPlus);
                                    //            }

                                    //            int xmlLLR_ZIP = Convert.ToInt32(LLR_ZIP);
                                    //            int xmlLLR_ZIPPLUS = Convert.ToInt32(LLR_ZIP_PLUS);

                                    //            /*string CAPLLRZIP = CapLandlordInfoEntity.Zip.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.Zip.ToString().Trim();
                                    //            string CAPLLRZIPPlus = CapLandlordInfoEntity.ZipPlus.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.ZipPlus.ToString().Trim();

                                    //            if (drApp["DiffZipMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_ZIP = xmlLLR_ZIP; } else { ocaseDiff.XML_DIFF_ZIP = Convert.ToInt32(CAPLLRZIP); }
                                    //            if (drApp["DiffZipPlusMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_ZIPPLUS = xmlLLR_ZIPPLUS; } else { ocaseDiff.XML_DIFF_ZIPPLUS = Convert.ToInt32(CAPLLRZIPPlus); }*/

                                    //            string CAPLLRZIP = CapLandlordInfoEntity.Zip.ToString().Trim() == string.Empty ? "0" : CapLandlordInfoEntity.Zip.ToString().Trim().PadLeft(5, '0');
                                    //            string CAPLLRZIPPlus = CapLandlordInfoEntity.ZipPlus.ToString().Trim() == string.Empty ? "0" : CapLandlordInfoEntity.ZipPlus.ToString().Trim().PadLeft(4, '0');

                                    //            if (drApp["DiffZipMode"].ToString() == "TRUE")
                                    //            {
                                    //                ocaseDiff.XML_DIFF_ZIP = xmlLLR_ZIP;
                                    //            }
                                    //            else
                                    //            {
                                    //                ocaseDiff.XML_DIFF_ZIP = Convert.ToInt32(CAPLLRZIP);
                                    //            }
                                    //            if (drApp["DiffZipPlusMode"].ToString() == "TRUE")
                                    //            {
                                    //                ocaseDiff.XML_DIFF_ZIPPLUS = xmlLLR_ZIPPLUS;
                                    //            }
                                    //            else
                                    //            {
                                    //                ocaseDiff.XML_DIFF_ZIPPLUS = Convert.ToInt32(CAPLLRZIPPlus);
                                    //            }
                                    //        }
                                    //        ocaseDiff.XML_DIFF_ADD_OPERATOR = BaseForm.UserID;
                                    //        ocaseDiff.XML_DIFF_LSTC_OPERATOR = BaseForm.UserID;

                                    //        #region INSERT DIFF RECORDS

                                    //        List<XML_DIFF_TYPE> DIFFList = new List<XML_DIFF_TYPE>();
                                    //        DIFFList.Add(ocaseDiff);

                                    //        DataTable dtdiff = new DataTable();
                                    //        dtdiff = TMS00141Form.ToDataTable(DIFFList);

                                    //        string _strRespoutDIFF = "";

                                    //        _strRespoutDIFF = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram, propYear,
                                    //            _xmlAppID, _xmlMEMID, null, null, null, dtdiff, null, null, null, "UPDDSSDIFF");

                                    //        #endregion



                                    //    }

                                    //    #endregion
                                    //}
                                    #endregion
                                }


                                //string[] xFamDetails = _strRespoutMST.Split('|');
                                string strFamilyID = propMainCasemstEntity.FamilyId;  //xFamDetails[0].ToString().Trim();
                                string strAppno = CAPAppno;//xFamDetails[1].ToString().Trim();
                                                           //string strClientID = propMainCasemstEntity.ClientId; //xFamDetails[2].ToString().Trim();

                                #region SNP RECORDS DATA BUILDING
                                //DataTable dtSNPRec = new DataTable();
                                //DataRow[] drSNPRec = dsTempXML.Tables["HOUSEHOLDMEMBERS"].Select("Is_Primary_Applicant__c='false'");
                                //if (drSNPRec.Length > 0)
                                //    dtSNPRec = drSNPRec.CopyToDataTable();
                                string AppName = string.Empty;
                                //dtSNPRec =      dsTempXML.Tables["HOUSEHOLDMEMBERS"];
                                //if (dtSNPRec.Rows.Count > 0)
                                //{
                                int famSeq = 1;
                                int snpfamSeq = 0;
                                // foreach (DataRow drSNP in dtSNPRec.Rows)
                                //{


                                if (drApp["Is_Primary_Applicant__c"].ToString() == "true")
                                {
                                    ocaseSNP.XML_SNP_MEMBER_CODE = "C";
                                    AppName = drApp["First_Name__c"].ToString().Trim() + " " + drApp["Last_Name__c"].ToString().Trim();
                                }
                                else
                                {
                                    //famSeq++;
                                    //snpfamSeq = famSeq;
                                    ocaseSNP.XML_SNP_MEMBER_CODE = TMS00141Form.getRelation(drApp["CEAP_Relationships__c"].ToString(), drApp["Sex__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                                }

                                ocaseSNP.XML_SNP_AGENCY = propAgency;
                                ocaseSNP.XML_SNP_DEPT = propDept;
                                ocaseSNP.XML_SNP_PROGRAM = propProgram;
                                ocaseSNP.XML_SNP_YEAR = propYear;
                                ocaseSNP.XML_SNP_APP = strAppno;
                                ocaseSNP.XML_SNP_FAMILY_SEQ = snpfamSeq;

                                ocaseSNP.XML_SNP_CLIENT_ID = 0;

                                string tmpSSN = string.Empty;
                                if (!string.IsNullOrEmpty(drApp["SSN__c"].ToString().Trim()))
                                {
                                    tmpSSN = drApp["SSN__c"].ToString().Trim().Replace("-", "");
                                }

                                ocaseSNP.XML_SNP_SSNO = tmpSSN;
                                ocaseSNP.XML_SNP_NAME_IX_LAST = drApp["Last_Name__c"].ToString();
                                ocaseSNP.XML_SNP_NAME_IX_FI = drApp["First_Name__c"].ToString();

                                string MIName = string.Empty;
                                if (!string.IsNullOrEmpty(drApp["Middle_Name__c"].ToString().Trim()))
                                    MIName = drApp["Middle_Name__c"].ToString().Trim().Substring(0, 1);
                                ocaseSNP.XML_SNP_NAME_IX_MI = MIName;

                                ocaseSNP.XML_SNP_ALT_BDATE = drApp["Date_of_Birth__c"].ToString().Trim();
                                if (!string.IsNullOrEmpty(drApp["Date_of_Birth__c"].ToString().Trim()))
                                    ocaseSNP.XML_SNP_DOB_NA = 0;

                                ocaseSNP.XML_SNP_ALT_LNAME = "";
                                ocaseSNP.XML_SNP_ALT_FI = "";
                                ocaseSNP.XML_SNP_SEX = TMS00141Form.getGenderCode(drApp["Sex__c"].ToString());
                                ocaseSNP.XML_SNP_ETHNIC = TMS00141Form.getEthnicCode(drApp["Ethnicity__c"].ToString());
                                ocaseSNP.XML_SNP_EDUCATION = TMS00141Form.getEducationCode(drApp["Highest_Education_Level__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                                ocaseSNP.XML_SNP_DISABLE = TMS00141Form.getDisableCode(drApp["Are_you_with_disability__c"].ToString());

                                string Race = drApp["Additional_Race__c"].ToString();
                                if (!string.IsNullOrEmpty(Race.Trim()))
                                {
                                    string[] RaceSpilt = Race.Split(';');
                                    if (RaceSpilt.Length > 1) Race = "Multi-Race";
                                }
                                ocaseSNP.XML_SNP_RACE = TMS00141Form.getRaceCode(Race, BaseForm.BaseAgencyControlDetails.AgyShortName);

                                // ocaseSNP.XML_SNP_RACE = TMS00141Form.getRaceCode(drApp["Additional_Race__c"].ToString());
                                ocaseSNP.XML_SNP_DATE_LSTC = DateTime.Now.ToString();
                                ocaseSNP.XML_SNP_LSTC_OPERATOR = BaseForm.UserID;
                                ocaseSNP.XML_SNP_DATE_ADD = DateTime.Now.ToString();
                                ocaseSNP.XML_SNP_ADD_OPERATOR = BaseForm.UserID;
                                ocaseSNP.XML_SNP_MILITARY_STATUS = TMS00141Form.getMiltaryStatusCode(drApp["Have_you_served_in_military__c"].ToString());


                                if (drApp.Table.Columns.Contains("Employment_Status__c"))
                                {
                                    ocaseSNP.XML_SNP_WORK_STAT = TMS00141Form.getWorkStatusCode(drApp["Employment_Status__c"].ToString());
                                    ocaseSNP.XML_SNP_YOUTH = TMS00141Form.getYouthCode(drApp["Student_Status__c"].ToString(), drApp["Employment_Status__c"].ToString());
                                }
                                else
                                {
                                    string Cname = "HOUSEHOLDMEMBERS" + famSeq + "_Id";
                                    if (_dsAllDSSXML.Tables.Contains("Employment_Status__c"))
                                    {
                                        foreach (DataRow drxml in _dsAllDSSXML.Tables["Employment_Status__c"].Rows)
                                        {
                                            if (drxml[Cname].ToString() == "0")
                                            {
                                                ocaseSNP.XML_SNP_WORK_STAT = TMS00141Form.getWorkStatusCode(drxml["Employment_Status__c_Text"].ToString());
                                                ocaseSNP.XML_SNP_YOUTH = TMS00141Form.getYouthCode(drApp["Student_Status__c"].ToString(), drxml["Employment_Status__c_Text"].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }




                                #region INSERT SNP RECORDS

                                List<XML_SNP_TYPE> SNPList = new List<XML_SNP_TYPE>();
                                SNPList.Add(ocaseSNP);

                                DataTable dtsnp = new DataTable();
                                dtsnp = TMS00141Form.ToDataTable(SNPList);

                                string _strRespoutSNP = "";

                                _strRespoutSNP = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram, propYear,
                                    _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["ApplicationId"].ToString(), drApp["HouseholdMemberSystemID_c"].ToString(), null,
                                    dtsnp, null, null, null, null, null, "INSCNTSNP");
                                _saveFlag = true;

                                #endregion





                                // }
                                //}

                                #endregion
                            }
                            else
                            {
                                XML_SNP_TYPE ocaseSNP = new XML_SNP_TYPE();
                                ocaseSNP.XML_SNP_APP = CAPAppno;
                                ocaseSNP.XML_SNP_FAMILY_SEQ = Convert.ToDecimal(caseSnpEntity.FamilySeq);
                                string FamSeqID = caseSnpEntity.FamilySeq;
                                string tmpSSN = string.Empty;
                                if (!string.IsNullOrEmpty(drApp["SSN__c"].ToString().Trim()))
                                {
                                    tmpSSN = drApp["SSN__c"].ToString().Trim().Replace("-", "");
                                }
                                if (drApp["SSNmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_SSNO = tmpSSN; } else { ocaseSNP.XML_SNP_SSNO = caseSnpEntity.Ssno.ToString(); }
                                if (drApp["FNAMEmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_NAME_IX_FI = drApp["First_Name__c"].ToString().Trim().ToUpper(); } else { ocaseSNP.XML_SNP_NAME_IX_FI = caseSnpEntity.NameixFi.ToString().Trim().ToUpper(); }
                                if (drApp["MNAMEmode"].ToString() == "TRUE")
                                {

                                    string MIName = string.Empty;
                                    if (!string.IsNullOrEmpty(drApp["Middle_Name__c"].ToString().Trim()))
                                        MIName = drApp["Middle_Name__c"].ToString().Trim().Substring(0, 1);
                                    ocaseSNP.XML_SNP_NAME_IX_MI = MIName.ToUpper();
                                }
                                else { ocaseSNP.XML_SNP_NAME_IX_MI = caseSnpEntity.NameixMi.ToString().Trim().ToUpper(); }
                                if (drApp["LNAMEmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_NAME_IX_LAST = drApp["Last_Name__c"].ToString().Trim().ToUpper(); } else { ocaseSNP.XML_SNP_NAME_IX_LAST = caseSnpEntity.NameixLast.ToString().Trim().ToUpper(); }

                                if (drApp["DOBmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_ALT_BDATE = LookupDataAccess.Getdate(drApp["Date_of_Birth__c"].ToString().Trim().ToUpper()); } else { ocaseSNP.XML_SNP_ALT_BDATE = LookupDataAccess.Getdate(caseSnpEntity.AltBdate.ToString().Trim().ToUpper()); }
                                if (drApp["GENDERmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_SEX = TMS00141Form.getGenderCode(drApp["Sex__c"].ToString().Trim()); } else { ocaseSNP.XML_SNP_SEX = caseSnpEntity.Sex.ToString().Trim(); }

                                if (drApp["ETHENICmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_ETHNIC = TMS00141Form.getEthnicCode(drApp["Ethnicity__c"].ToString().Trim()); } else { ocaseSNP.XML_SNP_ETHNIC = caseSnpEntity.Ethnic.ToString().Trim(); }
                                if (drApp["RACEmode"].ToString() == "TRUE")
                                {
                                    string Race = drApp["Additional_Race__c"].ToString();
                                    if (!string.IsNullOrEmpty(Race.Trim()))
                                    {
                                        string[] RaceSpilt = Race.Split(';');
                                        if (RaceSpilt.Length > 1) Race = "Multi-Race";
                                    }
                                    ocaseSNP.XML_SNP_RACE = TMS00141Form.getRaceCode(Race, BaseForm.BaseAgencyControlDetails.AgyShortName);
                                }
                                else
                                {
                                    ocaseSNP.XML_SNP_RACE = caseSnpEntity.Race.ToString().Trim();
                                }

                                if (drApp["EDUCATIONmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_EDUCATION = TMS00141Form.getEducationCode(drApp["Highest_Education_Level__c"].ToString().Trim(), BaseForm.BaseAgencyControlDetails.AgyShortName); }
                                else
                                {
                                    ocaseSNP.XML_SNP_EDUCATION = caseSnpEntity.Education.ToString().Trim();
                                }

                                if (drApp["DISABLEDmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_DISABLE = TMS00141Form.getDisableCode(drApp["Are_you_with_disability__c"].ToString().Trim()); }
                                else
                                {
                                    ocaseSNP.XML_SNP_DISABLE = caseSnpEntity.Disable.ToString().Trim();
                                }

                                if (drApp["MILTARYmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_MILITARY_STATUS = TMS00141Form.getMiltaryStatusCode(drApp["Have_you_served_in_military__c"].ToString().Trim()); }
                                else
                                {
                                    ocaseSNP.XML_SNP_MILITARY_STATUS = caseSnpEntity.MilitaryStatus.ToString().Trim();
                                }

                                if (drApp["WORKSTATUSmode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_WORK_STAT = TMS00141Form.getWorkStatusCode(drApp["Employment_Status__c"].ToString().Trim()); }
                                else
                                {
                                    ocaseSNP.XML_SNP_WORK_STAT = caseSnpEntity.WorkStatus.ToString().Trim();
                                }
                                ocaseSNP.XML_SNP_LSTC_OPERATOR = BaseForm.UserID;

                                string XMLRelationCode = TMS00141Form.getRelation(drApp["CEAP_Relationships__c"].ToString(), drApp["Sex__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                                if (XMLRelationCode == string.Empty && _drCust.Cells["gvtType"].Value.ToString() == "A")
                                {
                                    XMLRelationCode = "C";
                                    ocaseSNP.XML_SNP_MEMBER_CODE = XMLRelationCode;
                                }
                                else
                                {
                                    if (drApp["RELATIONMode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_MEMBER_CODE = XMLRelationCode; }
                                    else
                                    {
                                        ocaseSNP.XML_SNP_MEMBER_CODE = caseSnpEntity.MemberCode.ToString().Trim();
                                    }
                                }

                                if (drApp["DISYOUTHMode"].ToString() == "TRUE") { ocaseSNP.XML_SNP_YOUTH = TMS00141Form.getYouthCode(drApp["Student_Status__c"].ToString().Trim(), drApp["Employment_Status__c"].ToString().Trim()); }
                                else
                                {
                                    ocaseSNP.XML_SNP_YOUTH = caseSnpEntity.Youth.ToString().Trim();
                                }


                                #region INSERT SNP RECORDS

                                List<XML_SNP_TYPE> SNPList = new List<XML_SNP_TYPE>();
                                SNPList.Add(ocaseSNP);

                                DataTable dtsnp = new DataTable();
                                dtsnp = TMS00141Form.ToDataTable(SNPList);
                                string _strRespoutSNP = "";
                                string _xmlMEMID = drApp["HouseholdMemberSystemID_c"].ToString();

                                _strRespoutSNP = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram, propYear,
                                    _xmlAppID, _xmlMEMID, null, dtsnp, null, null, null, null, null, "UPDDSSSNP");


                                #endregion
                                _saveFlag = true;
                                if (_drCust.Cells["gvtType"].Value.ToString() == "A")
                                {
                                    XML_MST_TYPE ocaseMST = new XML_MST_TYPE();
                                    if (drApp["SSNmode"].ToString() == "TRUE") { ocaseMST.XML_MST_SSN = tmpSSN; } else { ocaseMST.XML_MST_SSN = caseSnpEntity.Ssno.ToString(); }

                                    #region House Number & Street
                                    //string strValue = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["Address_Line_1__c"].ToString();
                                    //string HouseNumber = "";
                                    //string Street = "";

                                    //if (strValue != "")
                                    //{
                                    //    int firstSpaceIndex = strValue.IndexOf(" ");
                                    //    /*******************************************************/
                                    //    string strtempValue1 = "";
                                    //    if (firstSpaceIndex > -1)
                                    //        strtempValue1 = strValue.Substring(0, firstSpaceIndex);

                                    //    int result;
                                    //    bool isNumeric = int.TryParse(strtempValue1, out result);
                                    //    if (isNumeric)
                                    //    {
                                    //        HouseNumber = strtempValue1;
                                    //        Street = strValue.Substring(firstSpaceIndex + 1);
                                    //    }
                                    //    else
                                    //    {
                                    //        Street = strValue;
                                    //    }
                                    //    if (Street.Length > 25)
                                    //        Street = Street.Substring(0, 24);
                                    //    /*******************************************************/

                                    //}

                                    //if (drApp["HOUSENOmode"].ToString() == "TRUE") { ocaseMST.XML_MST_HN = HouseNumber; } else { ocaseMST.XML_MST_HN = propMainCasemstEntity.Hn.ToString().Trim().ToUpper(); }
                                    //if (drApp["STREETmode"].ToString() == "TRUE") { ocaseMST.XML_MST_STREET = Street; } else { ocaseMST.XML_MST_STREET = propMainCasemstEntity.Street.ToString().Trim().ToUpper(); }
                                    #endregion

                                    #region ZIP & ZIP Code
                                    string _ZIP = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["AddressZip_Code__c"].ToString();
                                    decimal MST_ZIP = 0;
                                    decimal MST_ZIP_PLUS = 0;
                                    if (_ZIP.ToString() != string.Empty)
                                    {
                                        string[] strArgs = null;

                                        if (_ZIP.Contains("-"))
                                            strArgs = _ZIP.Split('-');
                                        else if (_ZIP.Contains(" "))
                                            strArgs = _ZIP.Split(' ');
                                        else
                                            strArgs = _ZIP.Split('-');

                                        string Zipcode = "";
                                        string ZipPlus = "0";
                                        Zipcode = strArgs[0].ToString();
                                        if (strArgs.Length > 1)
                                            ZipPlus = strArgs[1].ToString();

                                        if (Zipcode.Length > 5)
                                        {
                                            Zipcode = Zipcode.Substring(0, 5);
                                            if (Zipcode.Length == 9)
                                                ZipPlus = Zipcode.Substring(5, 4);
                                        }

                                        MST_ZIP = Convert.ToDecimal(Zipcode);
                                        MST_ZIP_PLUS = Convert.ToDecimal(ZipPlus);

                                        if (drApp["ZIPmode"].ToString() == "TRUE") { ocaseMST.XML_MST_ZIP = MST_ZIP; } else { ocaseMST.XML_MST_ZIP = Convert.ToDecimal(propMainCasemstEntity.Zip.ToString() == string.Empty ? "0" : propMainCasemstEntity.Zip.ToString().PadLeft(5, '0')); }
                                        if (drApp["ZIPCODEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_ZIPPLUS = MST_ZIP_PLUS; } else { ocaseMST.XML_MST_ZIPPLUS = Convert.ToDecimal(propMainCasemstEntity.Zipplus.ToString() == string.Empty ? "0" : propMainCasemstEntity.Zipplus.ToString().PadLeft(4, '0')); }
                                    }
                                    #endregion

                                    #region State
                                    string State = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["State__c"].ToString();
                                    if (drApp["STATEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_STATE = State; } else { ocaseMST.XML_MST_STATE = propMainCasemstEntity.State.ToString(); }
                                    #endregion

                                    #region City
                                    string City = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["City__c"].ToString();
                                    if (drApp["CITYmode"].ToString() == "TRUE") { ocaseMST.XML_MST_CITY = City; } else { ocaseMST.XML_MST_CITY = propMainCasemstEntity.City.ToString(); }
                                    #endregion
                                    #region Email
                                    string Email = drApp["Email__c"].ToString().Trim();
                                    if (drApp["Emailmode"].ToString() == "TRUE") { ocaseMST.XML_MST_EMAIL = Email; } else { ocaseMST.XML_MST_EMAIL = propMainCasemstEntity.Email.ToString(); }
                                    #endregion

                                    #region Phone & Cell

                                    if (!string.IsNullOrEmpty(propMainCasemstEntity.Phone.ToString().Trim()) && !string.IsNullOrEmpty(propMainCasemstEntity.Area.ToString()))
                                    {
                                        ocaseMST.XML_MST_PHONE = propMainCasemstEntity.Phone.ToString();
                                        ocaseMST.XML_MST_AREA = propMainCasemstEntity.Area.ToString();
                                    }
                                    if (!string.IsNullOrEmpty(propMainCasemstEntity.CellPhone.ToString().Trim()))
                                        ocaseMST.XML_MST_CELL_PHONE = propMainCasemstEntity.CellPhone.ToString();

                                    string _phone = drApp["Primary_Phone__c"].ToString();
                                    string MST_area = string.Empty;
                                    string MST_phone = string.Empty;
                                    if (_phone.ToString() != string.Empty)
                                    {
                                        _phone = _phone.Replace("-", "");
                                        if (drApp["Primary_Phone_Type__c"].ToString() == "Home")
                                        {
                                            string _area = "";
                                            string _phn = "";

                                            if (_phone.Length == 10)
                                            {
                                                _area = _phone.Substring(0, 3);
                                                if (_phone.Length == 10)
                                                    _phn = _phone.Substring(3, 7);
                                            }
                                            else
                                            {

                                            }

                                            MST_area = _area;
                                            MST_phone = _phn;

                                            string Phone = MST_area + MST_phone;

                                            if (drApp["PHONEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_PHONE = MST_phone; ocaseMST.XML_MST_AREA = MST_area; }
                                            else
                                            {
                                                ocaseMST.XML_MST_PHONE = propMainCasemstEntity.Phone.ToString();
                                                ocaseMST.XML_MST_AREA = propMainCasemstEntity.Area.ToString();
                                            }
                                        }
                                        else if (drApp["Primary_Phone_Type__c"].ToString() == "Cell")
                                        {
                                            //ocaseMST.XML_MST_CELL_PHONE = _phone;

                                            if (drApp["CELLmode"].ToString() == "TRUE") { ocaseMST.XML_MST_CELL_PHONE = _phone; }
                                            else
                                            {
                                                ocaseMST.XML_MST_CELL_PHONE = propMainCasemstEntity.CellPhone.ToString();
                                            }
                                        }
                                    }

                                    #endregion

                                    #region Primary Langugae

                                    string xmlPrimaryLang = TMS00141Form.getLanguageCode(drApp["Primary_Language__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                                    string CAPprimaryLang = propMainCasemstEntity.Language.ToString().Trim();
                                    if (drApp["LANGUAGEmode"].ToString() == "TRUE") { ocaseMST.XML_MST_LANGUAGE = xmlPrimaryLang; } else { ocaseMST.XML_MST_LANGUAGE = propMainCasemstEntity.Language.ToString(); }
                                    #endregion

                                    #region Housing Situation
                                    string xmlHousingsSit = TMS00141Form.getHousingCode(_dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim());
                                    string CAPHousingSit = propMainCasemstEntity.Housing.ToString().Trim();
                                    if (drApp["HOUSINGmode"].ToString() == "TRUE") { ocaseMST.XML_MST_HOUSING = xmlHousingsSit; } else { ocaseMST.XML_MST_HOUSING = propMainCasemstEntity.Housing.ToString(); }
                                    #endregion


                                    #region Rent/Mortgage
                                    string xmlRentMortgaeg = "0.00";
                                    string xmlHousingsSitDesc = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim();
                                    if (xmlHousingsSitDesc.ToLower().Contains("rent"))
                                        xmlRentMortgaeg = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Mortgage_Portion_Amount__c"].ToString().Trim();
                                    else if (xmlHousingsSitDesc.ToLower().Contains("own"))
                                        xmlRentMortgaeg = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Monthly_Payment_for_Mortgage__c"].ToString().Trim();

                                    if (xmlRentMortgaeg == "")
                                        xmlRentMortgaeg = "0.00";

                                    //string CAPprimaryLang = propMainCasemstEntity.Language.ToString().Trim();
                                    if (drApp["RENTMORTmode"].ToString() == "TRUE") { ocaseMST.XML_MST_EXP_RENT = Convert.ToDecimal(xmlRentMortgaeg); } else { ocaseMST.XML_MST_EXP_RENT = Convert.ToDecimal(propMainCasemstEntity.ExpRent); }
                                    #endregion

                                    #region Dwelling Type
                                    string xmlDwellingType = TMS00141Form.getDwellingTypeCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Type_Of_Home_To_Live__c"].ToString().Trim());
                                    string CAPDwellingType = propMainCasemstEntity.Dwelling.ToString().Trim();
                                    if (drApp["DWELLINGMode"].ToString() == "TRUE") { ocaseMST.XML_MST_DWELLING = xmlDwellingType; } else { ocaseMST.XML_MST_DWELLING = CAPDwellingType.ToString(); }
                                    #endregion


                                    #region Primary Method of Paying for Heat
                                    string xmlPPMOPayHeat = TMS00141Form.getHeatIncCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_Method_for_Heat__c"].ToString().Trim());
                                    string CAPPPMOPayHeat = propMainCasemstEntity.HeatIncRent.ToString().Trim();
                                    if (drApp["PRIMPAYHEATMode"].ToString() == "TRUE") { ocaseMST.XML_MST_HEAT_INC_RENT = xmlPPMOPayHeat; } else { ocaseMST.XML_MST_HEAT_INC_RENT = CAPPPMOPayHeat.ToString(); }
                                    #endregion

                                    #region Primary Source of Heat
                                    string xmlPrimeSource = TMS00141Form.getPrimarySourceCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim());
                                    string CAPPrimeSource = propMainCasemstEntity.Source.ToString().Trim();
                                    if (drApp["PRIMSRCHEARMode"].ToString() == "TRUE") { ocaseMST.XML_MST_SOURCE = xmlPrimeSource; } else { ocaseMST.XML_MST_SOURCE = CAPPrimeSource.ToString(); }
                                    #endregion

                                    ocaseMST.XML_MST_FAMILY_SEQ = (Convert.ToDecimal(caseSnpEntity.FamilySeq) == 0 ? 1 : Convert.ToDecimal(caseSnpEntity.FamilySeq));
                                    ocaseMST.XML_MST_APP_NO = CAPAppno;
                                    ocaseMST.XML_MST_LSTC_OPERATOR_1 = BaseForm.UserID;

                                    //ocaseMST = putLIHEAP(ocaseMST, drApp);
                                    #region INSERT MST RECORDS
                                    string _strRespoutMST = "";
                                    List<XML_MST_TYPE> MstList = new List<XML_MST_TYPE>();
                                    MstList.Add(ocaseMST);
                                    DataTable dtmst = new DataTable();
                                    dtmst = TMS00141Form.ToDataTable(MstList);


                                    _strRespoutMST = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram, propYear,
                                        _xmlAppID, _xmlMEMID, dtmst, null, null, null, null, null, null, "UPDDSSMST");

                                    //string[] xFamDetails = _strRespoutMST.Split('|');
                                    //string strFamilyID = xFamDetails[0].ToString().Trim();
                                    //string strAppno = xFamDetails[1].ToString().Trim();
                                    //string strClientID = xFamDetails[2].ToString().Trim();

                                    #endregion


                                    #region LANDLORD Records DATA Building
                                    DataTable dtLLRRec = new DataTable();
                                    dtLLRRec = _dsAllDSSXML.Tables["LandlordSection"];

                                    if (dtLLRRec.Rows.Count > 0)
                                    {
                                        XML_LLR_TYPE ocaseLLR = new XML_LLR_TYPE();
                                        ocaseLLR.XML_LLR_AGENCY = propAgency;
                                        ocaseLLR.XML_LLR_DEPT = propDept;
                                        ocaseLLR.XML_LLR_PROGRAM = propProgram;
                                        ocaseLLR.XML_LLR_YEAR = propYear;
                                        ocaseLLR.XML_LLR_APP_NO = CAPAppno;

                                        if (dtLLRRec.Columns.Contains("Landlord_Company_Name__c"))
                                        {
                                            string xmlLLRFname = "", xmlLLRLName = "";
                                            if (!string.IsNullOrEmpty(dtLLRRec.Rows[0]["Landlord_Company_Name__c"].ToString().Trim()))
                                            {
                                                string[] LLRNAME = dtLLRRec.Rows[0]["Landlord_Company_Name__c"].ToString().Trim().Split(' ');

                                                xmlLLRFname = LLRNAME[0];
                                                if (LLRNAME.Length > 1)
                                                    xmlLLRLName = LLRNAME[1];
                                            }

                                            string CAPLLRFname = CapLandlordInfoEntity.IncareFirst.ToString().Trim().ToUpper();
                                            if (drApp["LLRFnameMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_FIRST_NAME = xmlLLRFname; } else { ocaseLLR.XML_LLR_FIRST_NAME = CAPLLRFname; }

                                            string CAPLLRLname = CapLandlordInfoEntity.IncareLast.ToString().Trim().ToUpper();
                                            if (drApp["LLRLnameMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_LAST_NAME = xmlLLRLName; } else { ocaseLLR.XML_LLR_LAST_NAME = CAPLLRLname; }

                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_Company_Phone__c"))
                                        {
                                            string xmlLLRPhone = dtLLRRec.Rows[0]["Landlord_Company_Phone__c"].ToString().Trim().Replace("-", "");
                                            string CAPLLRPhone = CapLandlordInfoEntity.Phone.ToString().Trim().ToUpper();
                                            if (drApp["LLRPhoneMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_PHONE = xmlLLRPhone; } else { ocaseLLR.XML_LLR_PHONE = CAPLLRPhone; }
                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_City__c"))
                                        {
                                            string xmlLLRCity = dtLLRRec.Rows[0]["Landlord_City__c"].ToString().Trim();
                                            string CAPLLRCity = CapLandlordInfoEntity.City.ToString().Trim().ToUpper();

                                            if (drApp["LLRCityMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_CITY = xmlLLRCity; } else { ocaseLLR.XML_LLR_CITY = CAPLLRCity; }
                                        }
                                        if (dtLLRRec.Columns.Contains("Landlord_State__c"))
                                        {
                                            string xmlLLRState = dtLLRRec.Rows[0]["Landlord_State__c"].ToString().Trim();
                                            string CAPLLRState = CapLandlordInfoEntity.State.ToString().Trim();

                                            if (drApp["LLRStateMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_STATE = xmlLLRState; } else { ocaseLLR.XML_LLR_STATE = CAPLLRState; }
                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_Company_Address__c"))
                                        {
                                            string strValue1 = dtLLRRec.Rows[0]["Landlord_Company_Address__c"].ToString();
                                            string firstPart1 = "";
                                            string secondPart1 = "";

                                            if (strValue1 != "")
                                            {
                                                int firstSpaceIndex = strValue1.IndexOf(" ");
                                                /*******************************************************/
                                                string strtempValue1 = "";
                                                if (firstSpaceIndex > -1)
                                                    strtempValue1 = strValue1.Substring(0, firstSpaceIndex);

                                                int result;
                                                bool isNumeric = int.TryParse(strtempValue1, out result);
                                                if (isNumeric)
                                                {
                                                    firstPart1 = strtempValue1;
                                                    secondPart1 = strValue1.Substring(firstSpaceIndex + 1);
                                                }
                                                else
                                                {
                                                    secondPart1 = strValue1;
                                                }

                                                /*******************************************************/
                                                //   string strtempValue2 = strValue.Substring(firstSpaceIndex + 1);

                                            }

                                            string xmlStreet = secondPart1;
                                            string xmlHNo = firstPart1;

                                            string CAPStreet = CapLandlordInfoEntity.Street.ToString().Trim().ToUpper();
                                            string CAPHNo = CapLandlordInfoEntity.Hn.ToString().Trim();

                                            if (drApp["LLRStreetMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_STREET = xmlStreet; } else { ocaseLLR.XML_LLR_STREET = CAPStreet; }
                                            if (drApp["LLRHouseNoMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_HN = xmlHNo; } else { ocaseLLR.XML_LLR_HN = CAPHNo; }

                                        }

                                        if (dtLLRRec.Columns.Contains("Landlord_AddressZip_Code__c"))
                                        {
                                            string _LLRZIP = dtLLRRec.Rows[0]["Landlord_AddressZip_Code__c"].ToString();
                                            decimal LLR_ZIP = 0;
                                            decimal LLR_ZIP_PLUS = 0;
                                            if (_LLRZIP.ToString() != string.Empty)
                                            {
                                                string[] strArgs = null;

                                                if (_LLRZIP.Contains("-"))
                                                    strArgs = _LLRZIP.Split('-');
                                                else if (_LLRZIP.Contains(" "))
                                                    strArgs = _LLRZIP.Split(' ');
                                                else
                                                    strArgs = _LLRZIP.Split('-');

                                                string Zipcode = "";
                                                string ZipPlus = "0";
                                                Zipcode = strArgs[0].ToString();
                                                if (strArgs.Length > 1)
                                                    ZipPlus = strArgs[1].ToString();

                                                if (Zipcode.Length > 5)
                                                {
                                                    Zipcode = Zipcode.Substring(0, 5);
                                                    if (Zipcode.Length == 9)
                                                        ZipPlus = Zipcode.Substring(5, 4);
                                                }

                                                LLR_ZIP = Convert.ToDecimal(Zipcode);
                                                LLR_ZIP_PLUS = Convert.ToDecimal(ZipPlus);
                                            }

                                            int xmlLLR_ZIP = Convert.ToInt32(LLR_ZIP);
                                            int xmlLLR_ZIPPLUS = Convert.ToInt32(LLR_ZIP_PLUS);

                                            string CAPLLRZIP = CapLandlordInfoEntity.Zip.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.Zip.ToString().Trim();
                                            string CAPLLRZIPPlus = CapLandlordInfoEntity.ZipPlus.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.ZipPlus.ToString().Trim();

                                            if (drApp["LLRZipMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_ZIP = xmlLLR_ZIP; } else { ocaseLLR.XML_LLR_ZIP = Convert.ToInt32(CAPLLRZIP); }
                                            if (drApp["LLRZipCodeMode"].ToString() == "TRUE") { ocaseLLR.XML_LLR_ZIPPLUS = xmlLLR_ZIPPLUS; } else { ocaseLLR.XML_LLR_ZIPPLUS = Convert.ToInt32(CAPLLRZIPPlus); }
                                        }

                                        ocaseLLR.XML_LLR_ADD_OPERATOR = BaseForm.UserID;
                                        ocaseLLR.XML_LLR_LSTC_OPERATOR = BaseForm.UserID;

                                        #region INSERT LLR RECORDS

                                        List<XML_LLR_TYPE> LLRList = new List<XML_LLR_TYPE>();
                                        LLRList.Add(ocaseLLR);

                                        DataTable dtllr = new DataTable();
                                        dtllr = TMS00141Form.ToDataTable(LLRList);

                                        string _strRespoutLLR = "";

                                        _strRespoutLLR = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram, propYear, _xmlAppID, _xmlMEMID, null, null, null, null, dtllr, null, null, "UPDDSSLLR");


                                        #endregion

                                    }

                                    #endregion

                                    #region Mailing Address Stopped
                                    //string isMailingAddress = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Mailing_Address_different_than_Home_Add__c"].ToString().Trim();
                                    //if (isMailingAddress.Trim().ToString().ToUpper() == "YES")
                                    //{
                                    //    #region CASEDIFF Records DATA Building
                                    //    DataTable dtDIFF = new DataTable();
                                    //    dtDIFF = _dsAllDSSXML.Tables["MAILINGDETAILS"];

                                    //    if (dtDIFF.Rows.Count > 0)
                                    //    {

                                    //        XML_DIFF_TYPE ocaseDiff = new XML_DIFF_TYPE();
                                    //        ocaseDiff.XML_DIFF_AGENCY = propAgency;
                                    //        ocaseDiff.XML_DIFF_DEPT = propDept;
                                    //        ocaseDiff.XML_DIFF_PROGRAM = propProgram;
                                    //        ocaseDiff.XML_DIFF_YEAR = propYear;
                                    //        ocaseDiff.XML_DIFF_APP_NO = CAPAppno;

                                    //        if (dtDIFF.Columns.Contains("City__c"))
                                    //        {
                                    //            string xmlDiffCity = dtDIFF.Rows[0]["City__c"].ToString().Trim();
                                    //            string CAPDiffCity = CapCaseDiffMailDetails.City.ToString().Trim().ToUpper();
                                    //            if (drApp["DiffCityMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_CITY = xmlDiffCity; } else { ocaseDiff.XML_DIFF_CITY = CAPDiffCity; }
                                    //        }
                                    //        if (dtDIFF.Columns.Contains("State__c"))
                                    //        {
                                    //            string xmlDiffSate = dtDIFF.Rows[0]["State__c"].ToString().Trim();
                                    //            string CAPdiffSate = CapCaseDiffMailDetails.State.ToString().Trim().ToUpper();
                                    //            if (drApp["DiffSateMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_STATE = xmlDiffSate; } else { ocaseDiff.XML_DIFF_STATE = CAPdiffSate; }
                                    //        }

                                    //        if (dtDIFF.Columns.Contains("Address_Line_1__c"))
                                    //        {
                                    //            string strValue1 = dtDIFF.Rows[0]["Address_Line_1__c"].ToString();
                                    //            string firstPart1 = "";
                                    //            string secondPart1 = "";

                                    //            if (strValue1 != "")
                                    //            {
                                    //                int firstSpaceIndex = strValue1.IndexOf(" ");
                                    //                /*******************************************************/
                                    //                string strtempValue1 = "";
                                    //                if (firstSpaceIndex > -1)
                                    //                    strtempValue1 = strValue1.Substring(0, firstSpaceIndex);

                                    //                int result;
                                    //                bool isNumeric = int.TryParse(strtempValue1, out result);
                                    //                if (isNumeric)
                                    //                {
                                    //                    firstPart1 = strtempValue1;
                                    //                    secondPart1 = strValue1.Substring(firstSpaceIndex + 1);
                                    //                }
                                    //                else
                                    //                {
                                    //                    secondPart1 = strValue1;
                                    //                }

                                    //                /*******************************************************/
                                    //                //   string strtempValue2 = strValue.Substring(firstSpaceIndex + 1);

                                    //            }

                                    //            string xmlStreet = secondPart1;
                                    //            string xmlHNo = firstPart1;    //split street and add HN
                                    //            string CAPStreet = CapCaseDiffMailDetails.Street.ToString().Trim();
                                    //            string CAPHN = CapCaseDiffMailDetails.Hn.ToString().Trim();


                                    //            if (drApp["DiffStreetMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_STREET = xmlStreet; } else { ocaseDiff.XML_DIFF_STREET = CAPStreet; }
                                    //            if (drApp["DiffHNMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_HN = xmlHNo; } else { ocaseDiff.XML_DIFF_HN = CAPHN; }
                                    //        }

                                    //        if (dtDIFF.Columns.Contains("AddressZip_Code__c"))
                                    //        {
                                    //            string _LLRZIP = dtDIFF.Rows[0]["AddressZip_Code__c"].ToString();
                                    //            decimal LLR_ZIP = 0;
                                    //            decimal LLR_ZIP_PLUS = 0;
                                    //            if (_LLRZIP.ToString() != string.Empty)
                                    //            {
                                    //                string[] strArgs = null;

                                    //                if (_LLRZIP.Contains("-"))
                                    //                    strArgs = _LLRZIP.Split('-');
                                    //                else if (_LLRZIP.Contains(" "))
                                    //                    strArgs = _LLRZIP.Split(' ');
                                    //                else
                                    //                    strArgs = _LLRZIP.Split('-');

                                    //                string Zipcode = "";
                                    //                string ZipPlus = "0";
                                    //                Zipcode = strArgs[0].ToString();
                                    //                if (strArgs.Length > 1)
                                    //                    ZipPlus = strArgs[1].ToString();

                                    //                if (Zipcode.Length > 5)
                                    //                {
                                    //                    Zipcode = Zipcode.Substring(0, 5);
                                    //                    if (Zipcode.Length == 9)
                                    //                        ZipPlus = Zipcode.Substring(5, 4);
                                    //                }

                                    //                LLR_ZIP = Convert.ToDecimal(Zipcode);
                                    //                LLR_ZIP_PLUS = Convert.ToDecimal(ZipPlus);
                                    //            }

                                    //            int xmlLLR_ZIP = Convert.ToInt32(LLR_ZIP);
                                    //            int xmlLLR_ZIPPLUS = Convert.ToInt32(LLR_ZIP_PLUS);

                                    //            string CAPLLRZIP = CapLandlordInfoEntity.Zip.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.Zip.ToString().Trim();
                                    //            string CAPLLRZIPPlus = CapLandlordInfoEntity.ZipPlus.ToString().Trim() == "" ? "0" : CapLandlordInfoEntity.ZipPlus.ToString().Trim();

                                    //            if (drApp["DiffZipMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_ZIP = xmlLLR_ZIP; } else { ocaseDiff.XML_DIFF_ZIP = Convert.ToInt32(CAPLLRZIP); }
                                    //            if (drApp["DiffZipPlusMode"].ToString() == "TRUE") { ocaseDiff.XML_DIFF_ZIPPLUS = xmlLLR_ZIPPLUS; } else { ocaseDiff.XML_DIFF_ZIPPLUS = Convert.ToInt32(CAPLLRZIPPlus); }
                                    //        }
                                    //        ocaseDiff.XML_DIFF_ADD_OPERATOR = BaseForm.UserID;
                                    //        ocaseDiff.XML_DIFF_LSTC_OPERATOR = BaseForm.UserID;

                                    //        #region INSERT DIFF RECORDS

                                    //        List<XML_DIFF_TYPE> DIFFList = new List<XML_DIFF_TYPE>();
                                    //        DIFFList.Add(ocaseDiff);

                                    //        DataTable dtdiff = new DataTable();
                                    //        dtdiff = TMS00141Form.ToDataTable(DIFFList);

                                    //        string _strRespoutDIFF = "";

                                    //        _strRespoutDIFF = DSSXMLData.INSUPDEL_DSSXML_CAPTAIN(BaseForm.DataBaseConnectionString, propAgency, propDept, propProgram, propYear, _xmlAppID, _xmlMEMID, null, null, null, dtdiff, null, null, null, "UPDDSSDIFF");

                                    //        #endregion



                                    //    }

                                    //    #endregion
                                    //}

                                    #endregion
                                }
                            }

                        }
                    }
                    if (_saveFlag)
                    {
                        if (_DelFlag == "N")
                            AlertBox.Show("Updated Scuccessfully.");
                        else if (_DelFlag == "Y")
                            AlertBox.Show("Deleted Scuccessfully.");

                        //this.DialogResult = DialogResult.OK;
                        //this.Close();

                        /* Reload the Screen Top Grid */
                        _propCloseFormStatus = "Y";
                        propCasemstEntity = _model.CaseMstData.GetCaseMST(propAgency, propDept, propProgram, propYear, propAppl);
                        propMainCasemstEntity = _model.CaseMstData.GetCaseMST(propAgency, propDept, propProgram, propYear, propAppl);
                        propCasesnpEntity = _model.CaseMstData.GetCaseSnpadpyn(propAgency, propDept, propProgram, propYear, propAppl);
                        propMainCasesnpEntity = _model.CaseMstData.GetCaseSnpadpyn(propAgency, propDept, propProgram, propYear, propAppl);
                        _propSnpDatatable = TMS00141Form.ToDataTable(propMainCasesnpEntity);
                        //propcaseIncomeEntity = _model.CaseMstData.GetCaseIncomeadpynf(propAgency, propDept, propProgram, propYear, propAppl, string.Empty);
                        CapLandlordInfoEntity = _model.CaseMstData.GetLandlordadpya(propAgency, propDept, propProgram, propYear, propAppl, "Landlord");

                        if (CapLandlordInfoEntity == null)
                            CapLandlordInfoEntity = new CaseDiffEntity();

                        CapCaseDiffMailDetails = _model.CaseMstData.GetCaseDiffadpya(propAgency, propDept, propProgram, propYear, propAppl, "");
                        if (CapCaseDiffMailDetails == null)
                            CapCaseDiffMailDetails = new CaseDiffEntity();
                        ClearModeDSSXMLDate();
                        LoadDSSXMLGrid();

                    }
                }
                else
                {
                    AlertBox.Show("Please check-off atleast one member", MessageBoxIcon.Information);
                }
            }


        }

        public XML_MST_TYPE putLIHEAP(XML_MST_TYPE ocaseMST, DataRow drApp)
        {

            XML_MST_TYPE _ocaseMST = ocaseMST;
            #region LIHEAP Questions
            if (CAPLihpmQueslist.Count > 0)
            {
                string hasLHQues = "";
                string strXMLQResp1 = string.Empty;
                string strXMLQResp2 = string.Empty;
                string strXMLQResp3 = string.Empty;
                string strXMLQResp4 = string.Empty;
                string strXMLQResp5 = string.Empty;
                string strXMLQResp6 = string.Empty;
                string strXMLQResp7 = string.Empty;
                string strXMLQResp8 = string.Empty;
                string strXMLQResp9 = string.Empty;
                string strXMLQResp10 = string.Empty;
                string strXMLQResp11 = string.Empty;
                string XML_MST_SOURCE = TMS00141Form.getPrimarySourceCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim());

                strXMLQResp1 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Home_Owner__c"].ToString());
                strXMLQResp2 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["LivedInCurrentHomeForAtleastAYear__c"].ToString());

                strXMLQResp3 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Used_Same_Heating_Vendor__c"].ToString());
                if (!string.IsNullOrEmpty(strXMLQResp3.Trim()) && string.IsNullOrEmpty(strXMLQResp2.Trim()))
                    strXMLQResp2 = TMS00141Form.getS0056codes("YES");
                else if (!string.IsNullOrEmpty(strXMLQResp2.Trim()) && strXMLQResp2.Trim().ToUpper() == "N")
                    strXMLQResp3 = TMS00141Form.getS0056codes("Not Applicable");

                if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                    strXMLQResp4 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_System_currently_operable__c"].ToString());
                else if (XML_MST_SOURCE.Trim() == "02")
                    strXMLQResp4 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_System_currently_operable__c"].ToString());

                if (XML_MST_SOURCE.Trim() != "04")
                {
                    strXMLQResp5 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_system_repaired_or_replaced__c"].ToString());
                    if (!string.IsNullOrEmpty(strXMLQResp5.Trim()) && string.IsNullOrEmpty(strXMLQResp1.Trim()) && string.IsNullOrEmpty(strXMLQResp4.Trim()))
                    {
                        strXMLQResp4 = TMS00141Form.getS0056codes("NO");
                        strXMLQResp1 = TMS00141Form.getS0056codes("YES");
                    }
                    else if (strXMLQResp4 == "N" && strXMLQResp1 == "N")
                    {
                        strXMLQResp5 = TMS00141Form.getS0056codes("Not Applicable");
                    }
                }



                if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                {
                    strXMLQResp6 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Currently_Less_Than_A_Quarter_Tank_Fuel__c"].ToString());
                    if (!string.IsNullOrEmpty(strXMLQResp6.Trim()) && string.IsNullOrEmpty(strXMLQResp4.Trim()))
                        strXMLQResp6 = TMS00141Form.getS0056codes("YES");
                }
                else
                    strXMLQResp6 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Currently_Disconnected__c"].ToString());

                if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                    strXMLQResp7 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Currently_Disconnected__c"].ToString()); //getS0056codes(dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Afford_to_pay_company_to_restore_service__c"].ToString());
                else
                {
                    strXMLQResp7 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Afford_to_pay_company_to_restore_service__c"].ToString());

                    if (!string.IsNullOrEmpty(strXMLQResp7.Trim()) && string.IsNullOrEmpty(strXMLQResp6.Trim()))
                    {
                        strXMLQResp6 = TMS00141Form.getS0056codes("YES");
                    }
                }

                if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                {
                    strXMLQResp8 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Afford_to_pay_company_to_restore_service__c"].ToString());//getS0056codes(dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString());
                    if (!string.IsNullOrEmpty(strXMLQResp8.Trim()) && string.IsNullOrEmpty(strXMLQResp7.Trim()))
                    {
                        strXMLQResp6 = TMS00141Form.getS0056codes("YES");
                    }
                }
                else                                                                                                                                                             //if (!string.IsNullOrEmpty(strXMLQResp8.Trim()) && !string.IsNullOrEmpty(ocaseMST.XML_MST_LPM_0007.Trim()))
                {

                    strXMLQResp8 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString());                                                                                                                                                        //{


                    if (!string.IsNullOrEmpty(strXMLQResp8.Trim()) && string.IsNullOrEmpty(strXMLQResp6.Trim()))
                    {
                        strXMLQResp6 = TMS00141Form.getS0056codes("NO");
                    }
                }

                if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                {
                    strXMLQResp9 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString());//Received_Shut_off_Notice__c//getS0056codes(dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString());
                    if (!string.IsNullOrEmpty(strXMLQResp9.Trim()) && string.IsNullOrEmpty(strXMLQResp7.Trim()))
                    {
                        strXMLQResp7 = TMS00141Form.getS0056codes("NO");
                        //if (!string.IsNullOrEmpty(strXMLQResp7.Trim()) && string.IsNullOrEmpty(strXMLQResp6.Trim()))
                        //{
                        //    strXMLQResp6 = getS0056codes("YES");
                        //}
                    }
                }
                else
                {
                    strXMLQResp9 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString());
                    if (!string.IsNullOrEmpty(strXMLQResp9.Trim()) && string.IsNullOrEmpty(strXMLQResp8.Trim()))
                    {
                        strXMLQResp8 = TMS00141Form.getS0056codes("YES");
                        strXMLQResp6 = TMS00141Form.getS0056codes("NO");
                    }
                }
                if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                {
                    strXMLQResp10 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString());
                    if (!string.IsNullOrEmpty(strXMLQResp10.Trim()) && string.IsNullOrEmpty(strXMLQResp9.Trim()))
                        strXMLQResp9 = TMS00141Form.getS0056codes("YES");
                }
                else
                    strXMLQResp10 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Service_Discon_Protected_By_Medical_Prot__c"].ToString());

                strXMLQResp11 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Free_Weatherization_Services__c"].ToString());



                for (int Q = 1; Q <= 11; Q++)
                {
                    string strLIHQCode = "00" + String.Format("{0:00}", Q);
                    List<LIHPMQuesEntity> lstLHQues = CAPLihpmQueslist.Where(x => x.LPMQ_CODE == strLIHQCode).ToList();
                    if (lstLHQues.Count > 0)
                    {
                        string strXMLQResp = ""; string strCAPQResp = "";
                        #region Question Resp Filter
                        if (strLIHQCode == "0001")
                        {
                            strXMLQResp = strXMLQResp1;
                            strCAPQResp = propMainCasemstEntity.LPM0001;
                            if (drApp["MST_LPM_0001Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0001 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0001 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0002")
                        {
                            strXMLQResp = strXMLQResp2;
                            strCAPQResp = propMainCasemstEntity.LPM0002;
                            if (drApp["MST_LPM_0002Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0002 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0002 = strCAPQResp; }

                        }
                        else if (strLIHQCode == "0003")
                        {
                            strXMLQResp = strXMLQResp3;
                            strCAPQResp = propMainCasemstEntity.LPM0003;
                            if (drApp["MST_LPM_0003Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0003 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0003 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0004")
                        {
                            strXMLQResp = strXMLQResp4;
                            strCAPQResp = propMainCasemstEntity.LPM0004;
                            if (drApp["MST_LPM_0004Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0004 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0004 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0005")
                        {
                            strXMLQResp = strXMLQResp5;
                            strCAPQResp = propMainCasemstEntity.LPM0005;
                            if (drApp["MST_LPM_0005Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0005 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0005 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0006")
                        {
                            strXMLQResp = strXMLQResp6;
                            strCAPQResp = propMainCasemstEntity.LPM0006;
                            if (drApp["MST_LPM_0006Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0006 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0006 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0007")
                        {
                            strXMLQResp = strXMLQResp7;
                            strCAPQResp = propMainCasemstEntity.LPM0007;
                            if (drApp["MST_LPM_0007Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0007 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0007 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0008")
                        {
                            strXMLQResp = strXMLQResp8;
                            strCAPQResp = propMainCasemstEntity.LPM0008;
                            if (drApp["MST_LPM_0008Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0008 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0008 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0009")
                        {
                            strXMLQResp = strXMLQResp9;
                            strCAPQResp = propMainCasemstEntity.LPM0009;
                            if (drApp["MST_LPM_0009Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0009 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0009 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0010")
                        {
                            strXMLQResp = strXMLQResp10;
                            strCAPQResp = propMainCasemstEntity.LPM0010;
                            if (drApp["MST_LPM_0010Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0010 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0010 = strCAPQResp; }
                        }
                        else if (strLIHQCode == "0011")
                        {
                            strXMLQResp = strXMLQResp11;
                            strCAPQResp = propMainCasemstEntity.LPM0011;
                            if (drApp["MST_LPM_0011Mode"].ToString() == "TRUE") { ocaseMST.XML_MST_LPM_0011 = strXMLQResp; } else { ocaseMST.XML_MST_LPM_0011 = strCAPQResp; }
                        }
                        #endregion



                    }
                }

                if (hasLHQues == "Y")
                    gvwSubdetails.Columns[0].Width = 400;


            }
            #endregion

            return _ocaseMST;
        }

        string checkDssValues()
        {

            string strResult = "";
            List<DataGridViewRow> chckdRows = gvwSubdetails.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtSelchk"].Value.ToString().ToUpper() == "TRUE").ToList();
            foreach (DataGridViewRow _drRow in chckdRows)
            {

                string _currValue = _drRow.Cells["gvtOldValue"].Value.ToString().Trim();
                string _FieldName = _drRow.Cells["gvtfield"].Value.ToString().Trim();

                if (_currValue == "")
                {
                    strResult += _FieldName + ", ";
                }

            }
            if (strResult != "")
            {
                strResult = strResult.Trim().TrimEnd(',');

                var arry = strResult.Split(',');
                if (arry.Length > 1)
                    strResult += " fields are blank. Please uncheck and save the record again.";
                else if (arry.Length == 1)
                    strResult += " field is blank. Please uncheck and save the record again.";
            }
            return strResult;

        }

        void ClearModeDSSXMLDate()
        {

            foreach (DataRow _dr in _propDSSXMLIntakedt.Rows)
            {
                _dr["SSNmode"] = "FALSE";
                _dr["FNAMEmode"] = "FALSE";
                _dr["MNAMEmode"] = "FALSE";
                _dr["LNAMEmode"] = "FALSE";
                _dr["DOBmode"] = "FALSE";
                _dr["GENDERmode"] = "FALSE";
                _dr["ETHENICmode"] = "FALSE";
                _dr["RACEmode"] = "FALSE";
                _dr["EDUCATIONmode"] = "FALSE";
                _dr["DISABLEDmode"] = "FALSE";
                _dr["MILTARYmode"] = "FALSE";
                _dr["WORKSTATUSmode"] = "FALSE";
                _dr["HOUSENOmode"] = "FALSE";
                _dr["STREETmode"] = "FALSE";
                _dr["ZIPmode"] = "FALSE";
                _dr["ZIPCODEmode"] = "FALSE";
                _dr["STATEmode"] = "FALSE";
                _dr["CITYmode"] = "FALSE";
                _dr["PHONEmode"] = "FALSE";
                _dr["CELLmode"] = "FALSE";
                _dr["LANGUAGEmode"] = "FALSE";
                _dr["HOUSINGmode"] = "FALSE";
                _dr["Emailmode"] = "FALSE";

                /*Landlrd Info*/
                _dr["LLRFnameMode"] = "FALSE";
                _dr["LLRLnameMode"] = "FALSE";
                _dr["LLRPhoneMode"] = "FALSE";
                _dr["LLRCityMode"] = "FALSE";
                _dr["LLRStateMode"] = "FALSE";
                _dr["LLRStreetMode"] = "FALSE";
                _dr["LLRHouseNoMode"] = "FALSE";
                _dr["LLRZipMode"] = "FALSE";
                _dr["LLRZipCodeMode"] = "FALSE";

                /*CASE DIFF*/
                _dr["DiffSateMode"] = "FALSE";
                _dr["DiffCityMode"] = "FALSE";
                _dr["DiffHNMode"] = "FALSE";
                _dr["DiffStreetMode"] = "FALSE";
                _dr["DiffPhoneMode"] = "FALSE";
                _dr["DiffZipMode"] = "FALSE";
                _dr["DiffZipPlusMode"] = "FALSE";

                /*LEHIP Questions*/
                _dr["MST_LPM_0001Mode"] = "FALSE";
                _dr["MST_LPM_0002Mode"] = "FALSE";
                _dr["MST_LPM_0003Mode"] = "FALSE";
                _dr["MST_LPM_0004Mode"] = "FALSE";
                _dr["MST_LPM_0005Mode"] = "FALSE";
                _dr["MST_LPM_0006Mode"] = "FALSE";
                _dr["MST_LPM_0007Mode"] = "FALSE";
                _dr["MST_LPM_0008Mode"] = "FALSE";
                _dr["MST_LPM_0009Mode"] = "FALSE";
                _dr["MST_LPM_0010Mode"] = "FALSE";
                _dr["MST_LPM_0011Mode"] = "FALSE";

                _dr["RENTMORTMode"] = "FALSE";
                _dr["RELATIONMode"] = "FALSE";
                _dr["DISYOUTHMode"] = "FALSE";
                _dr["DWELLINGMode"] = "FALSE";
                _dr["PRIMPAYHEATMode"] = "FALSE";
                _dr["PRIMSRCHEARMode"] = "FALSE";
            }
        }
        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                bool boolSnpInsert = false;
                string strLAppNo = string.Empty;
                string strUserId = string.Empty;
                int intUpdate = 0;
                List<DataGridViewRow> SelectedgvRows = (from c in gvwCustomer.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvcTopSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();
                foreach (DataGridViewRow dritem in SelectedgvRows)
                {
                    LeanIntakeEntity dr = dritem.Tag as LeanIntakeEntity;
                    if (dr != null)
                    {
                        if (dr.PIP_Valid_MODE > 0)
                        {
                            intUpdate = intUpdate + 1;
                            if (strFormType != string.Empty)
                            {

                                CaseMstEntity mstdata = BaseForm.BaseCaseMstListEntity[0];
                                CaseSnpEntity Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dr.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                                if (Basesnpdata == null)
                                    Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dr.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dr.PIP_LNAME.Trim().ToUpper());

                                if (dritem.Cells["gvtType"].Value.ToString() == "I")
                                {
                                    CaseSnpEntity SnpEntity = new Model.Objects.CaseSnpEntity();
                                    SnpEntity = new Model.Objects.CaseSnpEntity();
                                    SnpEntity.Agency = BaseForm.BaseCaseMstListEntity[0].ApplAgency;
                                    SnpEntity.Dept = BaseForm.BaseCaseMstListEntity[0].ApplDept;
                                    SnpEntity.Program = BaseForm.BaseCaseMstListEntity[0].ApplProgram;
                                    SnpEntity.Year = BaseForm.BaseCaseMstListEntity[0].ApplYr;
                                    SnpEntity.App = BaseForm.BaseCaseMstListEntity[0].ApplNo;
                                    SnpEntity.FamilySeq = Basesnpdata.FamilySeq.ToString();
                                    SnpEntity.SsnSearchType = "I";
                                    SnpEntity.DobNa = "0";
                                    SnpEntity.LstcOperator = BaseForm.UserID;
                                    SnpEntity.AddOperator = BaseForm.UserID;

                                    string strOutFamilyseq = SnpEntity.FamilySeq;
                                    if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                    {
                                        boolSnpInsert = true;
                                    }
                                }
                                else
                                {

                                    if (dritem.Cells["gvtType"].Value.ToString() == "A")
                                    {
                                        //CaseSnpEntity snpCaptainEntity = new Model.Objects.CaseSnpEntity();
                                        //snpCaptainEntity = propCasesnpEntity.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);
                                        CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dr.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                                        if (SnpEntity == null)
                                            SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dr.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dr.PIP_LNAME.Trim().ToUpper());

                                        CheckHistoryIntakeData(dr, BaseForm.BaseCaseMstListEntity[0], Basesnpdata, true);
                                        //BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);
                                        //List<CaseIncomeEntity> caseincomeApp = propcaseIncomeEntity.FindAll(u => u.FamilySeq == propCasemstEntity.FamilySeq);
                                        if (dr.PIP_EMAIL_MODE == "TRUE")
                                            mstdata.Email = dr.PIP_EMAIL;
                                        if (dr.PIP_SSN_MODE == "TRUE")
                                            mstdata.Ssn = dr.PIP_SSN;
                                        if (dr.PIP_PRI_LANGUAGE_MODE == "TRUE")
                                            mstdata.Language = dr.PIP_PRI_LANGUAGE;
                                        if (dr.PIP_FAMILY_TYPE_MODE == "TRUE")
                                            mstdata.FamilyType = dr.PIP_FAMILY_TYPE;
                                        if (dr.PIP_HOUSING_MODE == "TRUE")
                                            mstdata.Housing = dr.PIP_HOUSING;
                                        //  if (dr._MODE == "TRUE")
                                        // mstdata.snpSchoolDistrict = dr.PIP_;
                                        if (dr.PIP_INCOME_TYPES_MODE == "TRUE")
                                            mstdata.IncomeTypes = dr.PIP_INCOME_TYPES;
                                        //  mstdata.AddressDetails = dr.PIP_ADDRESS;
                                        if (dr.PIP_HOME_PHONE_MODE == "TRUE")
                                            mstdata.Area = dr.PIP_AREA;
                                        if (dr.PIP_HOME_PHONE_MODE == "TRUE")
                                            mstdata.Phone = dr.PIP_HOME_PHONE;
                                        if (dr.PIP_CELL_NUMBER_MODE == "TRUE")
                                            mstdata.CellPhone = dr.PIP_CELL_NUMBER;
                                        if (dr.PIP_HOUSENO_MODE == "TRUE")
                                            mstdata.Hn = dr.PIP_HOUSENO;
                                        if (dr.PIP_DIRECTION_MODE == "TRUE")
                                            mstdata.Direction = dr.PIP_DIRECTION;
                                        if (dr.PIP_STREET_MODE == "TRUE")
                                            mstdata.Street = dr.PIP_STREET;
                                        if (dr.PIP_SUFFIX_MODE == "TRUE")
                                            mstdata.Suffix = dr.PIP_SUFFIX;
                                        if (dr.PIP_APT_MODE == "TRUE")
                                            mstdata.Apt = dr.PIP_APT;
                                        if (dr.PIP_FLR_MODE == "TRUE")
                                            mstdata.Flr = dr.PIP_FLR;
                                        if (dr.PIP_CITY_MODE == "TRUE")
                                            mstdata.City = dr.PIP_CITY;
                                        if (dr.PIP_STATE_MODE == "TRUE")
                                            mstdata.State = dr.PIP_STATE;
                                        if (dr.PIP_ZIP_MODE == "TRUE")
                                            mstdata.Zip = dr.PIP_ZIP;
                                        if (dr.PIP_TOWNSHIP_MODE == "TRUE")
                                            mstdata.TownShip = dr.PIP_TOWNSHIP;
                                        if (dr.PIP_COUNTY_MODE == "TRUE")
                                            mstdata.County = dr.PIP_COUNTY;
                                        //if (dr.PIP_NCASHBEN_MODE == "TRUE")
                                        //    mstdata.MstNCashBen = dr.PIP_NCASHBEN;


                                        mstdata.Mode = "Edit";
                                        strUserId = dr.PIP_REG_ID;

                                        mstdata.Type = Consts.CASE2001.ManualType;
                                        boolSnpInsert = _model.CaseMstData.InsertUpdateCaseMstLeanIntake(mstdata, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg, SnpEntity, SnpEntity.NameixFi, SnpEntity.AltBdate);
                                        strLAppNo = strApplNo;
                                        CaseMstEntity mstPIPData = new CaseMstEntity();
                                        mstPIPData = mstdata;
                                        // services insert

                                        // CaseMSTSER MSTSEREntity = new CaseMSTSER();
                                        //if (dr.PIP_SERVICES_MODE == "TRUE")
                                        //{


                                        //DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", dr.PIP_CAP_AGENCY, dr.PIP_DEPT, dr.PIP_PROGRAM, dr.PIP_YEAR, dr.PIP_APP_NO);
                                        //DataTable serviceSaveDT = serviceSaveDS.Tables[0];


                                        //foreach (DataRow strserv in serviceSaveDT.Rows)
                                        //{
                                        //    if (strserv["INQ_CODE"].ToString().Trim() != string.Empty)
                                        //    {
                                        //        MSTSEREntity = new CaseMSTSER();
                                        //        MSTSEREntity.Agency = BaseForm.BaseCaseMstListEntity[0].ApplAgency;
                                        //        MSTSEREntity.Dept = BaseForm.BaseCaseMstListEntity[0].ApplDept;
                                        //        MSTSEREntity.Program = BaseForm.BaseCaseMstListEntity[0].ApplProgram;
                                        //        if (propYear == string.Empty)
                                        //            MSTSEREntity.Year = "    ";
                                        //        else
                                        //            MSTSEREntity.Year = BaseForm.BaseCaseMstListEntity[0].ApplYr;


                                        //        MSTSEREntity.AppNo = BaseForm.BaseCaseMstListEntity[0].ApplNo;
                                        //        MSTSEREntity.Service = strserv["INQ_CODE"].ToString().Trim();
                                        //        MSTSEREntity.AddOperator = BaseForm.UserID;
                                        //        MSTSEREntity.LstcOperator = BaseForm.UserID;
                                        //        MSTSEREntity.Mode = "LEANUPD";
                                        //        _model.CaseMstData.InsertUpdateDelMSTSER(MSTSEREntity);
                                        //    }
                                        //}



                                        CaseMSTSER MSTSEREntity = new CaseMSTSER();
                                        //if (dr.PIP_SERVICES_MODE == "TRUE")
                                        //{
                                        // string[] strServices = dr.PIP_SERVICES.Split(',');

                                        foreach (ListItem strserv in dr.Pip_service_list_Mode)
                                        {
                                            if (strserv.Value.ToString().ToUpper() == "TRUE")
                                            {
                                                MSTSEREntity = new CaseMSTSER();
                                                MSTSEREntity.Agency = BaseForm.BaseCaseMstListEntity[0].ApplAgency;
                                                MSTSEREntity.Dept = BaseForm.BaseCaseMstListEntity[0].ApplDept;
                                                MSTSEREntity.Program = BaseForm.BaseCaseMstListEntity[0].ApplProgram;
                                                if (propYear == string.Empty)
                                                    MSTSEREntity.Year = "    ";
                                                else
                                                    MSTSEREntity.Year = BaseForm.BaseCaseMstListEntity[0].ApplYr;


                                                MSTSEREntity.AppNo = BaseForm.BaseCaseMstListEntity[0].ApplNo;
                                                MSTSEREntity.Service = strserv.Text;
                                                MSTSEREntity.AddOperator = BaseForm.UserID;
                                                MSTSEREntity.LstcOperator = BaseForm.UserID;
                                                MSTSEREntity.Mode = "LEANUPD";
                                                _model.CaseMstData.InsertUpdateDelMSTSER(MSTSEREntity);
                                            }
                                        }


                                        //}

                                        // Custom Questions

                                        //DataTable dtcustomQues = PIPDATA.GETPIPCUSTORRESPORADDCUST(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, string.Empty, dr.PIP_REG_ID, string.Empty, "ADDCUSTRESP");

                                        ////GETLEANADDCUST(string.Empty, dr.PIP_USER_ID, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, "ADDCUSTRESP");
                                        //if (dtcustomQues.Rows.Count > 0)
                                        //{

                                        //    foreach (DataRow drcustom in dtcustomQues.Rows)
                                        //    {
                                        //        CustomQuestionsEntity custEntity = new CustomQuestionsEntity();

                                        //        custEntity.ACTAGENCY = propAgency;
                                        //        custEntity.ACTDEPT = propDept;
                                        //        custEntity.ACTPROGRAM = propProgram;
                                        //        if (propYear == string.Empty)
                                        //            custEntity.ACTYEAR = "    ";
                                        //        else
                                        //            custEntity.ACTYEAR = propYear;
                                        //        custEntity.ACTAPPNO = propAppl;
                                        //        if (drcustom["ADDCUST_SEQ"].ToString() == "0")
                                        //        {
                                        //            custEntity.ACTSNPFAMILYSEQ = propCasemstEntity.FamilySeq;
                                        //        }
                                        //        else
                                        //        {
                                        //            custEntity.ACTSNPFAMILYSEQ = drcustom["ADDCUST_SEQ"].ToString();
                                        //        }
                                        //        custEntity.ACTCODE = drcustom["ADDCUST_CODE"].ToString();

                                        //        custEntity.ACTMULTRESP = drcustom["ADDCUST_MULT_RESP"].ToString();

                                        //        custEntity.ACTNUMRESP = drcustom["ADDCUST_NUM_RESP"].ToString();


                                        //        custEntity.ACTDATERESP = drcustom["ADDCUST_DATE_RESP"].ToString();

                                        //        custEntity.ACTALPHARESP = drcustom["ADDCUST_ALPHA_RESP"].ToString();

                                        //        custEntity.Mode = "LEANUPD";

                                        //        custEntity.addoperator = BaseForm.UserID;
                                        //        custEntity.lstcoperator = BaseForm.UserID;
                                        //        _model.CaseMstData.InsertUpdateADDCUST(custEntity);
                                        //    }

                                        //}



                                        // CheckHistoryIntakeData(dr, BaseForm.BaseCaseMstListEntity[0], Basesnpdata, true);
                                        SnpEntity.Type = "MST";

                                        if (dr.PIP_SSN_MODE == "TRUE")
                                            SnpEntity.Ssno = dr.PIP_SSN;

                                        if (dr.PIP_FNAME_MODE == "TRUE")
                                            SnpEntity.NameixFi = dr.PIP_FNAME;
                                        if (dr.PIP_MNAME_MODE == "TRUE")
                                            SnpEntity.NameixMi = dr.PIP_MNAME;
                                        if (dr.PIP_LNAME_MODE == "TRUE")
                                            SnpEntity.NameixLast = dr.PIP_LNAME;
                                        //if (dr.PIP_DOB_MODE == "TRUE")
                                        //    SnpEntity.AltBdate = dr.PIP_DOB;
                                        if (dr.PIP_DOB_MODE == "TRUE")
                                        {
                                            SnpEntity.AltBdate = dr.PIP_DOB;
                                            if (SnpEntity.AltBdate != string.Empty)
                                            {
                                                SnpEntity.DobNa = "0";
                                            }
                                        }
                                        if (dr.PIP_GENDER_MODE == "TRUE")
                                            SnpEntity.Sex = dr.PIP_GENDER;
                                        if (dr.PIP_STATUS_MODE == "TRUE")
                                            SnpEntity.Status = dr.PIP_STATUS;
                                        if (dr.PIP_MARITAL_STATUS_MODE == "TRUE")
                                            SnpEntity.MaritalStatus = dr.PIP_MARITAL_STATUS;
                                        if (dr.PIP_RELATIONSHIP_MODE == "TRUE")
                                            SnpEntity.MemberCode = dr.PIP_RELATIONSHIP;
                                        if (dr.PIP_ETHNIC_MODE == "TRUE")
                                            SnpEntity.Ethnic = dr.PIP_ETHNIC;
                                        if (dr.PIP_RACE_MODE == "TRUE")
                                            SnpEntity.Race = dr.PIP_RACE;
                                        if (dr.PIP_EDUCATION_MODE == "TRUE")
                                            SnpEntity.Education = dr.PIP_EDUCATION;
                                        if (dr.PIP_DISABLE_MODE == "TRUE")
                                            SnpEntity.Disable = dr.PIP_DISABLE;
                                        if (dr.PIP_WORK_STAT_MODE == "TRUE")
                                            SnpEntity.WorkStatus = dr.PIP_WORK_STAT;
                                        if (dr.PIP_SCHOOL_MODE == "TRUE")
                                            SnpEntity.SchoolDistrict = dr.PIP_SCHOOL;
                                        if (dr.PIP_HEALTH_INS_MODE == "TRUE")
                                            SnpEntity.HealthIns = dr.PIP_HEALTH_INS;
                                        if (dr.PIP_VETERAN_MODE == "TRUE")
                                            SnpEntity.Vet = dr.PIP_VETERAN;
                                        if (dr.PIP_FOOD_STAMP_MODE == "TRUE")
                                            SnpEntity.FootStamps = dr.PIP_FOOD_STAMP;
                                        if (dr.PIP_FARMER_MODE == "TRUE")
                                            SnpEntity.Farmer = dr.PIP_FARMER;
                                        if (dr.PIP_WIC_MODE == "TRUE")
                                            SnpEntity.Wic = dr.PIP_WIC;
                                        if (dr.PIP_RELITRAN_MODE == "TRUE")
                                            SnpEntity.Relitran = dr.PIP_RELITRAN;
                                        if (dr.PIP_DRVLIC_MODE == "TRUE")
                                            SnpEntity.Drvlic = dr.PIP_DRVLIC;
                                        if (dr.PIP_MILITARY_STATUS_MODE == "TRUE")
                                            SnpEntity.MilitaryStatus = dr.PIP_MILITARY_STATUS;
                                        //if (dr.PIP_YOUTH_MODE == "TRUE")
                                        //    SnpEntity.Youth = dr.PIP_YOUTH;
                                        if (dr.PIP_PREGNANT_MODE == "TRUE")
                                            SnpEntity.Pregnant = dr.PIP_PREGNANT;
                                        string strHealthcodes = SnpEntity.Health_Codes;
                                        foreach (ListItem itemNon in dr.Pip_Healthcodes_list_Mode)
                                        {
                                            if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                            {
                                                if (strHealthcodes.Trim() != string.Empty)
                                                    strHealthcodes = strHealthcodes + "," + itemNon.Text;
                                                else
                                                    strHealthcodes = itemNon.Text;
                                            }
                                        }

                                        SnpEntity.Health_Codes = strHealthcodes;

                                        //if (dr.PIP_HEALTH_CODES_MODE == "TRUE")
                                        //    SnpEntity.Health_Codes = dr.PIP_HEALTH_CODES;

                                        string strNoncashbenefit = SnpEntity.NonCashBenefits;
                                        foreach (ListItem itemNon in dr.Pip_NonCashbenefit_list_Mode)
                                        {
                                            if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                            {
                                                if (strNoncashbenefit.Trim() != string.Empty)
                                                    strNoncashbenefit = strNoncashbenefit + "," + itemNon.Text;
                                                else
                                                    strNoncashbenefit = itemNon.Text;
                                            }
                                        }

                                        SnpEntity.NonCashBenefits = strNoncashbenefit;
                                        //if (dr.PIP_SUFFIX_MODE == "TRUE")
                                        //    SnpEntity.SnpSuffix = dr.PIP_SUFFIX;
                                        SnpEntity.LstcOperator = BaseForm.UserID;
                                        SnpEntity.AddOperator = BaseForm.UserID;
                                        //  SnpEntity.Status = "A";

                                        SnpEntity.Mode = "Edit";

                                        SnpEntity.SsnSearchType = string.Empty;

                                        if (SnpEntity.Ssno.Trim() == string.Empty)
                                            SnpEntity.Ssno = "000000000";


                                        string strOutFamilyseq = SnpEntity.FamilySeq;
                                        CaseSnpEntity snpPIPData = SnpEntity;
                                        if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                        {
                                            //UpdateLeanIntakeData(dr.PIP_CONFNO.ToString(), dr.PIP_ID, SnpEntity.Agency, SnpEntity.Dept, SnpEntity.Program, SnpEntity.Year, SnpEntity.App);

                                            //if (caseincomeApp.Count == 0)
                                            // {
                                            //if (dr.PIP_INCOME_TYPES_MODE == "TRUE")
                                            //{
                                            //    if (dr.PIP_INCOME_TYPES.Trim() != string.Empty)
                                            //    {
                                            //        string[] strINCOMETypes = dr.PIP_INCOME_TYPES.ToString().Split(',');
                                            //        foreach (string strIncomeType in strINCOMETypes)
                                            //        {
                                            //            if (strIncomeType.ToString() != string.Empty)
                                            //            {
                                            //                List<CaseIncomeEntity> incometypecount = caseincomeApp.FindAll(u => u.Type == strIncomeType);
                                            //                if (incometypecount.Count == 0)
                                            //                {
                                            //                    CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                            //                    incomeupdatedetails.Agency = propAgency;
                                            //                    incomeupdatedetails.Dept = propDept;
                                            //                    incomeupdatedetails.Program = propProgram;
                                            //                    incomeupdatedetails.App = propCasemstEntity.ApplNo;
                                            //                    incomeupdatedetails.Year = propYear;
                                            //                    incomeupdatedetails.FamilySeq = propCasemstEntity.FamilySeq;
                                            //                    incomeupdatedetails.Type = strIncomeType;
                                            //                    incomeupdatedetails.AddOperator = BaseForm.UserID;
                                            //                    incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                            //                    incomeupdatedetails.Mode = string.Empty;
                                            //                    incomeupdatedetails.Exclude = "N";
                                            //                    incomeupdatedetails.Factor = "1.00";


                                            //                    if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                            //                    {
                                            //                    }
                                            //                }
                                            //            }
                                            //        }
                                            //    }

                                            //}
                                            // }

                                            foreach (ListItem item in dr.Pip_Income_list_Mode)
                                            {
                                                if (item.Value.ToString().ToUpper() == "TRUE")
                                                {
                                                    List<CaseIncomeEntity> _propRecentIncomelist = propcaseIncomeEntity.FindAll(u => u.FamilySeq == dr.PIP_SEQ && u.Type == item.Text.Trim());

                                                    foreach (CaseIncomeEntity incomeitem in _propRecentIncomelist)
                                                    {


                                                        CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                                        incomeupdatedetails.Agency = BaseForm.BaseCaseMstListEntity[0].ApplAgency;
                                                        incomeupdatedetails.Dept = BaseForm.BaseCaseMstListEntity[0].ApplDept;
                                                        incomeupdatedetails.Program = BaseForm.BaseCaseMstListEntity[0].ApplProgram;
                                                        incomeupdatedetails.App = BaseForm.BaseCaseMstListEntity[0].ApplNo;
                                                        incomeupdatedetails.Year = BaseForm.BaseCaseMstListEntity[0].ApplYr;
                                                        incomeupdatedetails.FamilySeq = BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                                                        incomeupdatedetails.Type = item.Text.Trim();
                                                        incomeupdatedetails.Interval = incomeitem.Interval;
                                                        incomeupdatedetails.Exclude = "N";
                                                        incomeupdatedetails.Factor = incomeitem.Factor;
                                                        //incomeupdatedetails.Val1 = incomeitem.Val1;
                                                        //incomeupdatedetails.Date1 = incomeitem.Date1;
                                                        //incomeupdatedetails.Val2 = incomeitem.Val2;
                                                        //incomeupdatedetails.Date2 = incomeitem.Date2;
                                                        //incomeupdatedetails.Val3 = incomeitem.Val3;
                                                        //incomeupdatedetails.Date3 = incomeitem.Date3;
                                                        //incomeupdatedetails.Val4 = incomeitem.Val4;
                                                        //incomeupdatedetails.Date4 = incomeitem.Date4;
                                                        //incomeupdatedetails.Val5 = incomeitem.Val5;
                                                        //incomeupdatedetails.Date5 = incomeitem.Date5;

                                                        //incomeupdatedetails.Source = incomeitem.Source;
                                                        //incomeupdatedetails.Verifier = incomeitem.Verifier;
                                                        //incomeupdatedetails.HowVerified = incomeitem.HowVerified;
                                                        //incomeupdatedetails.TotIncome = incomeitem.TotIncome;
                                                        //incomeupdatedetails.ProgIncome = incomeitem.ProgIncome;
                                                        //incomeupdatedetails.HrRate1 = incomeitem.HrRate1;
                                                        //incomeupdatedetails.HrRate2 = incomeitem.HrRate2;
                                                        //incomeupdatedetails.HrRate3 = incomeitem.HrRate3;
                                                        //incomeupdatedetails.HrRate4 = incomeitem.HrRate4;
                                                        //incomeupdatedetails.HrRate5 = incomeitem.HrRate5;
                                                        //incomeupdatedetails.Average = incomeitem.Average;
                                                        incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                        incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                        incomeupdatedetails.Mode = string.Empty;

                                                        if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                        {
                                                        }
                                                    }

                                                }
                                            }


                                        }
                                        //CaseMstEntity _casemstentity = _model.CaseMstData.GetCaseMST(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

                                        //List<CaseSnpEntity> _snpentitylist = _model.CaseMstData.GetCaseSnpadpyn(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);
                                        //CaseSnpEntity _snpMainCaptainEntity = _snpentitylist.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);

                                        //CaseMstEntity _casemstentity = dritem.Cells["gvtName"].Tag as CaseMstEntity;
                                        //CaseSnpEntity _snpMainCaptainEntity = dritem.Cells["gvtSSN"].Tag as CaseSnpEntity;

                                        //if (_snpMainCaptainEntity != null)
                                        //{
                                        //    CaseSnpEntity SnpBaseMainEntity = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dr.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));

                                        //    CaseSnpEntity SnpMainEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dr.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));


                                        //    // CheckHistoryIntakeData(dr,BaseForm.BaseCaseMstListEntity[0], SnpBaseMainEntity, true);
                                        //}
                                    }
                                    else
                                    {
                                        CaseSnpEntity SnpEntity = new CaseSnpEntity();
                                        // CaseSnpEntity _snpMemberCaptainEntity = dritem.Cells["gvtSSN"].Tag as CaseSnpEntity;
                                        SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dr.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                                        if (SnpEntity == null)
                                            SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dr.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dr.PIP_LNAME.Trim().ToUpper());


                                        // SnpEntity = propCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dr.PIP_FNAME.ToUpper() && u.NameixLast.ToUpper() == dr.PIP_LNAME.ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                                        string strUpdateHistory = string.Empty;
                                        if (SnpEntity == null)
                                        {
                                            SnpEntity = new Model.Objects.CaseSnpEntity();
                                            SnpEntity.Agency = BaseForm.BaseCaseMstListEntity[0].ApplAgency;
                                            SnpEntity.Dept = BaseForm.BaseCaseMstListEntity[0].ApplDept;
                                            SnpEntity.Program = BaseForm.BaseCaseMstListEntity[0].ApplProgram;
                                            SnpEntity.Year = BaseForm.BaseCaseMstListEntity[0].ApplYr;
                                            SnpEntity.App = BaseForm.BaseCaseMstListEntity[0].ApplNo;
                                            SnpEntity.Status = "A";
                                            strUpdateHistory = "New";
                                        }
                                        if (SnpEntity != null)
                                        {
                                            if (dritem.Cells["gvtType"].Value.ToString() == "I")
                                            {
                                                SnpEntity.SsnSearchType = "I";
                                            }
                                            else
                                            {
                                                if (strUpdateHistory == string.Empty)
                                                {
                                                    CheckHistoryIntakeData(dr, BaseForm.BaseCaseMstListEntity[0], Basesnpdata, false);
                                                    if (dr.PIP_STATUS_MODE == "TRUE")
                                                        SnpEntity.Status = dr.PIP_STATUS;
                                                }// caseincomeMembers = new List<CaseIncomeEntity>(); propcaseIncomeEntity.FindAll(u => u.FamilySeq == SnpEntity.FamilySeq);
                                                if (dr.PIP_SSN_MODE == "TRUE")
                                                    SnpEntity.Ssno = dr.PIP_SSN;

                                                if (dr.PIP_FNAME_MODE == "TRUE")
                                                    SnpEntity.NameixFi = dr.PIP_FNAME;
                                                if (dr.PIP_MNAME_MODE == "TRUE")
                                                    SnpEntity.NameixMi = dr.PIP_MNAME;
                                                if (dr.PIP_LNAME_MODE == "TRUE")
                                                    SnpEntity.NameixLast = dr.PIP_LNAME;

                                                if (dr.PIP_DOB_MODE == "TRUE")
                                                {
                                                    SnpEntity.AltBdate = dr.PIP_DOB;
                                                    if (SnpEntity.AltBdate != string.Empty)
                                                    {
                                                        SnpEntity.DobNa = "0";
                                                    }
                                                }
                                                if (dr.PIP_GENDER_MODE == "TRUE")
                                                    SnpEntity.Sex = dr.PIP_GENDER;

                                                if (dr.PIP_MARITAL_STATUS_MODE == "TRUE")
                                                    SnpEntity.MaritalStatus = dr.PIP_MARITAL_STATUS;
                                                if (dr.PIP_RELATIONSHIP_MODE == "TRUE")
                                                    SnpEntity.MemberCode = dr.PIP_RELATIONSHIP;
                                                if (dr.PIP_ETHNIC_MODE == "TRUE")
                                                    SnpEntity.Ethnic = dr.PIP_ETHNIC;
                                                if (dr.PIP_RACE_MODE == "TRUE")
                                                    SnpEntity.Race = dr.PIP_RACE;
                                                if (dr.PIP_EDUCATION_MODE == "TRUE")
                                                    SnpEntity.Education = dr.PIP_EDUCATION;
                                                if (dr.PIP_DISABLE_MODE == "TRUE")
                                                    SnpEntity.Disable = dr.PIP_DISABLE;
                                                if (dr.PIP_WORK_STAT_MODE == "TRUE")
                                                    SnpEntity.WorkStatus = dr.PIP_WORK_STAT;
                                                if (dr.PIP_SCHOOL_MODE == "TRUE")
                                                    SnpEntity.SchoolDistrict = dr.PIP_SCHOOL;
                                                if (dr.PIP_HEALTH_INS_MODE == "TRUE")
                                                    SnpEntity.HealthIns = dr.PIP_HEALTH_INS;
                                                if (dr.PIP_VETERAN_MODE == "TRUE")
                                                    SnpEntity.Vet = dr.PIP_VETERAN;
                                                if (dr.PIP_FOOD_STAMP_MODE == "TRUE")
                                                    SnpEntity.FootStamps = dr.PIP_FOOD_STAMP;
                                                if (dr.PIP_FARMER_MODE == "TRUE")
                                                    SnpEntity.Farmer = dr.PIP_FARMER;
                                                if (dr.PIP_WIC_MODE == "TRUE")
                                                    SnpEntity.Wic = dr.PIP_WIC;
                                                if (dr.PIP_RELITRAN_MODE == "TRUE")
                                                    SnpEntity.Relitran = dr.PIP_RELITRAN;
                                                if (dr.PIP_DRVLIC_MODE == "TRUE")
                                                    SnpEntity.Drvlic = dr.PIP_DRVLIC;
                                                if (dr.PIP_MILITARY_STATUS_MODE == "TRUE")
                                                    SnpEntity.MilitaryStatus = dr.PIP_MILITARY_STATUS;
                                                //if (dr.PIP_YOUTH_MODE == "TRUE")
                                                //    SnpEntity.Youth = dr.PIP_YOUTH;
                                                if (dr.PIP_PREGNANT_MODE == "TRUE")
                                                    SnpEntity.Pregnant = dr.PIP_PREGNANT;
                                                //if (dr.PIP_HEALTH_CODES_MODE == "TRUE")
                                                //    SnpEntity.Health_Codes = dr.PIP_HEALTH_CODES;
                                                string strHealthcodes = SnpEntity.Health_Codes;
                                                foreach (ListItem itemNon in dr.Pip_Healthcodes_list_Mode)
                                                {
                                                    if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                                    {
                                                        if (strHealthcodes.Trim() != string.Empty)
                                                            strHealthcodes = strHealthcodes + "," + itemNon.Text;
                                                        else
                                                            strHealthcodes = itemNon.Text;
                                                    }
                                                }

                                                SnpEntity.Health_Codes = strHealthcodes;
                                                //if (dr.PIP_NCASHBEN_MODE == "TRUE")
                                                //    SnpEntity.NonCashBenefits = dr.PIP_NCASHBEN;

                                                string strNoncashbenefit = SnpEntity.NonCashBenefits;
                                                foreach (ListItem itemNon in dr.Pip_NonCashbenefit_list_Mode)
                                                {
                                                    if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                                    {
                                                        if (strNoncashbenefit.Trim() != string.Empty)
                                                            strNoncashbenefit = strNoncashbenefit + "," + itemNon.Text;
                                                        else
                                                            strNoncashbenefit = itemNon.Text;
                                                    }
                                                }
                                                SnpEntity.NonCashBenefits = strNoncashbenefit;

                                                SnpEntity.LstcOperator = BaseForm.UserID;
                                                SnpEntity.AddOperator = BaseForm.UserID;


                                                /// SnpEntity.Status = "A";

                                                SnpEntity.Type = "SNP";

                                                SnpEntity.SsnSearchType = string.Empty;
                                                if (SnpEntity.Ssno.Trim() == string.Empty)
                                                    SnpEntity.Ssno = "000000000";
                                                // CheckHistoryIntakeData(dr, BaseForm.BaseCaseMstListEntity[0], Basesnpdata, false);
                                            }
                                            string strOutFamilyseq = SnpEntity.FamilySeq;
                                            CaseSnpEntity snpPipData = SnpEntity;
                                            if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                            {
                                                boolSnpInsert = true;
                                                //if (dritem.Cells["gvtType"].Value.ToString() != "I")
                                                //{
                                                //    CaseSnpEntity SnpMemEntity = propCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dr.PIP_FNAME.ToUpper() && u.NameixLast.ToUpper() == dr.PIP_LNAME.ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                                                //    if (SnpMemEntity != null)
                                                //    {
                                                //        List<CaseIncomeEntity> caseincomeMembers = propcaseIncomeEntity.FindAll(u => u.FamilySeq == SnpMemEntity.FamilySeq);
                                                //        //if (caseincomeMembers.Count == 0)
                                                //        //{
                                                //        if (dr.PIP_INCOME_TYPES_MODE == "TRUE")
                                                //        {
                                                //            if (dr.PIP_INCOME_TYPES.Trim() != string.Empty)
                                                //            {
                                                //                string[] strINCOMETypes = dr.PIP_INCOME_TYPES.ToString().Split(',');
                                                //                foreach (string strIncomeType in strINCOMETypes)
                                                //                {

                                                //                    if (strIncomeType.ToString() != string.Empty)
                                                //                    {
                                                //                        List<CaseIncomeEntity> incometypecount = caseincomeMembers.FindAll(u => u.Type == strIncomeType);
                                                //                        if (incometypecount.Count == 0)
                                                //                        {
                                                //                            CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                                //                            incomeupdatedetails.Agency = propAgency;
                                                //                            incomeupdatedetails.Dept = propDept;
                                                //                            incomeupdatedetails.Program = propProgram;
                                                //                            incomeupdatedetails.App = propCasemstEntity.ApplNo;
                                                //                            incomeupdatedetails.Year = propYear;
                                                //                            incomeupdatedetails.FamilySeq = SnpMemEntity.FamilySeq;
                                                //                            incomeupdatedetails.Type = strIncomeType;
                                                //                            incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                //                            incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                //                            incomeupdatedetails.Mode = string.Empty;
                                                //                            incomeupdatedetails.Exclude = "N";
                                                //                            incomeupdatedetails.Factor = "1.00";


                                                //                            if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                //                            {
                                                //                            }
                                                //                        }
                                                //                    }
                                                //                }
                                                //            }
                                                //        }
                                                //        //}
                                                //    }
                                                //}


                                                foreach (ListItem item in dr.Pip_Income_list_Mode)
                                                {
                                                    if (item.Value.ToString().ToUpper() == "TRUE")
                                                    {
                                                        List<CaseIncomeEntity> _propRecentIncomelist = propcaseIncomeEntity.FindAll(u => u.FamilySeq == dr.PIP_SEQ && u.Type == item.Text.Trim());

                                                        foreach (CaseIncomeEntity incomeitem in _propRecentIncomelist)
                                                        {
                                                            CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();

                                                            incomeupdatedetails.Agency = BaseForm.BaseCaseMstListEntity[0].ApplAgency;
                                                            incomeupdatedetails.Dept = BaseForm.BaseCaseMstListEntity[0].ApplDept;
                                                            incomeupdatedetails.Program = BaseForm.BaseCaseMstListEntity[0].ApplProgram;
                                                            incomeupdatedetails.App = BaseForm.BaseCaseMstListEntity[0].ApplNo;
                                                            incomeupdatedetails.Year = BaseForm.BaseCaseMstListEntity[0].ApplYr;
                                                            incomeupdatedetails.FamilySeq = SnpEntity.FamilySeq;
                                                            incomeupdatedetails.Type = item.Text.Trim();
                                                            incomeupdatedetails.Interval = incomeitem.Interval;
                                                            incomeupdatedetails.Exclude = "N";
                                                            incomeupdatedetails.Factor = incomeitem.Factor;
                                                            //incomeupdatedetails.Val1 = incomeitem.Val1;
                                                            //incomeupdatedetails.Date1 = incomeitem.Date1;
                                                            //incomeupdatedetails.Val2 = incomeitem.Val2;
                                                            //incomeupdatedetails.Date2 = incomeitem.Date2;
                                                            //incomeupdatedetails.Val3 = incomeitem.Val3;
                                                            //incomeupdatedetails.Date3 = incomeitem.Date3;
                                                            //incomeupdatedetails.Val4 = incomeitem.Val4;
                                                            //incomeupdatedetails.Date4 = incomeitem.Date4;
                                                            //incomeupdatedetails.Val5 = incomeitem.Val5;
                                                            //incomeupdatedetails.Date5 = incomeitem.Date5;
                                                            //incomeupdatedetails.Factor = incomeitem.Factor;
                                                            //incomeupdatedetails.Source = incomeitem.Source;
                                                            //incomeupdatedetails.Verifier = incomeitem.Verifier;
                                                            //incomeupdatedetails.HowVerified = incomeitem.HowVerified;
                                                            //incomeupdatedetails.TotIncome = incomeitem.TotIncome;
                                                            //incomeupdatedetails.ProgIncome = incomeitem.ProgIncome;
                                                            //incomeupdatedetails.HrRate1 = incomeitem.HrRate1;
                                                            //incomeupdatedetails.HrRate2 = incomeitem.HrRate2;
                                                            //incomeupdatedetails.HrRate3 = incomeitem.HrRate3;
                                                            //incomeupdatedetails.HrRate4 = incomeitem.HrRate4;
                                                            //incomeupdatedetails.HrRate5 = incomeitem.HrRate5;
                                                            //incomeupdatedetails.Average = incomeitem.Average;
                                                            incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                            incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                            incomeupdatedetails.Mode = string.Empty;

                                                            if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                            {
                                                            }
                                                        }

                                                    }
                                                }


                                            }
                                            //CaseMstEntity _casemstentity = _model.CaseMstData.GetCaseMST(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

                                            //List<CaseSnpEntity> _snpentitylist = _model.CaseMstData.GetCaseSnpadpyn(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

                                            //CaseSnpEntity _snpMemberCaptainEntity = _snpentitylist.Find(u => u.NameixFi.ToUpper() == dr.PIP_FNAME.ToUpper() && u.NameixLast.ToUpper() == dr.PIP_LNAME.ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));

                                            //if (_snpMemberCaptainEntity != null)
                                            //{
                                            //    // CaseSnpEntity SnpBaseMainEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim() == dr.PIP_FNAME.Trim() && u.NameixLast.Trim() == dr.PIP_LNAME.Trim() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));

                                            //    CaseSnpEntity SnpBaseMainEntity = propCasesnpEntity.Find(u => u.NameixFi.Trim() == dr.PIP_FNAME.Trim() && u.NameixLast.Trim() == dr.PIP_LNAME.Trim() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));

                                            //    CaseSnpEntity SnpMainEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim() == dr.PIP_FNAME.Trim() && u.NameixLast.Trim() == dr.PIP_LNAME.Trim() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));


                                            //    // CheckHistoryIntakeData(dr,BaseForm.BaseCaseMstListEntity[0], SnpBaseMainEntity, false);

                                            //}
                                        }
                                    }
                                }


                            }
                            else
                            {
                                CaseMstEntity mstdata = propCasemstEntity;
                                if (dritem.Cells["gvtType"].Value.ToString() == "I")
                                {
                                    CaseSnpEntity SnpEntity = new Model.Objects.CaseSnpEntity();
                                    SnpEntity = new Model.Objects.CaseSnpEntity();
                                    SnpEntity.Agency = propCasemstEntity.ApplAgency;
                                    SnpEntity.Dept = propCasemstEntity.ApplDept;
                                    SnpEntity.Program = propCasemstEntity.ApplProgram;
                                    SnpEntity.Year = propCasemstEntity.ApplYr;
                                    SnpEntity.App = propCasemstEntity.ApplNo;
                                    SnpEntity.FamilySeq = dr.PIP_SEQ.ToString();
                                    SnpEntity.SsnSearchType = "I";
                                    SnpEntity.DobNa = "0";
                                    SnpEntity.LstcOperator = BaseForm.UserID;
                                    SnpEntity.AddOperator = BaseForm.UserID;

                                    string strOutFamilyseq = SnpEntity.FamilySeq;
                                    if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                    {
                                        boolSnpInsert = true;
                                        // PIPDATA.InsertUpdatePIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr , propCasemstEntity.ApplNo, dr.PIP_SEQ, dr.PIP_AGENCY, dr.PIP_AGY.ToString(), dr.PIP_CONFNO.ToString(), dr.PIP_ID.ToString());

                                    }
                                }
                                else
                                {


                                    if (dr.PIP_SEQ == "0")
                                    {
                                        CaseSnpEntity snpCaptainEntity = new Model.Objects.CaseSnpEntity();
                                        snpCaptainEntity = propCasesnpEntity.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);
                                        CaseSnpEntity SnpEntity = propCasesnpEntity.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);
                                        List<CaseIncomeEntity> caseincomeApp = propcaseIncomeEntity.FindAll(u => u.FamilySeq == propCasemstEntity.FamilySeq);
                                        if (dr.PIP_EMAIL_MODE == "TRUE")
                                            mstdata.Email = dr.PIP_EMAIL;
                                        if (dr.PIP_SSN_MODE == "TRUE")
                                            mstdata.Ssn = dr.PIP_SSN;
                                        if (dr.PIP_PRI_LANGUAGE_MODE == "TRUE")
                                            mstdata.Language = dr.PIP_PRI_LANGUAGE;
                                        if (dr.PIP_FAMILY_TYPE_MODE == "TRUE")
                                            mstdata.FamilyType = dr.PIP_FAMILY_TYPE;
                                        if (dr.PIP_HOUSING_MODE == "TRUE")
                                            mstdata.Housing = dr.PIP_HOUSING;
                                        //  if (dr._MODE == "TRUE")
                                        // mstdata.snpSchoolDistrict = dr.PIP_;
                                        if (dr.PIP_INCOME_TYPES_MODE == "TRUE")
                                            mstdata.IncomeTypes = dr.PIP_INCOME_TYPES;
                                        //  mstdata.AddressDetails = dr.PIP_ADDRESS;
                                        if (dr.PIP_HOME_PHONE_MODE == "TRUE")
                                            mstdata.Area = dr.PIP_AREA;
                                        if (dr.PIP_HOME_PHONE_MODE == "TRUE")
                                            mstdata.Phone = dr.PIP_HOME_PHONE;
                                        if (dr.PIP_CELL_NUMBER_MODE == "TRUE")
                                            mstdata.CellPhone = dr.PIP_CELL_NUMBER;
                                        if (dr.PIP_HOUSENO_MODE == "TRUE")
                                            mstdata.Hn = dr.PIP_HOUSENO;
                                        if (dr.PIP_DIRECTION_MODE == "TRUE")
                                            mstdata.Direction = dr.PIP_DIRECTION;
                                        if (dr.PIP_STREET_MODE == "TRUE")
                                            mstdata.Street = dr.PIP_STREET;
                                        if (dr.PIP_SUFFIX_MODE == "TRUE")
                                            mstdata.Suffix = dr.PIP_SUFFIX;
                                        if (dr.PIP_APT_MODE == "TRUE")
                                            mstdata.Apt = dr.PIP_APT;
                                        if (dr.PIP_FLR_MODE == "TRUE")
                                            mstdata.Flr = dr.PIP_FLR;
                                        if (dr.PIP_CITY_MODE == "TRUE")
                                            mstdata.City = dr.PIP_CITY;
                                        if (dr.PIP_STATE_MODE == "TRUE")
                                            mstdata.State = dr.PIP_STATE;
                                        if (dr.PIP_ZIP_MODE == "TRUE")
                                            mstdata.Zip = dr.PIP_ZIP;
                                        if (dr.PIP_TOWNSHIP_MODE == "TRUE")
                                            mstdata.TownShip = dr.PIP_TOWNSHIP;
                                        if (dr.PIP_COUNTY_MODE == "TRUE")
                                            mstdata.County = dr.PIP_COUNTY;
                                        if (dr.PIP_NCASHBEN_MODE == "TRUE")
                                            mstdata.MstNCashBen = dr.PIP_NCASHBEN;


                                        mstdata.Mode = "Edit";
                                        strUserId = dr.PIP_REG_ID;

                                        mstdata.Type = Consts.CASE2001.ManualType;
                                        boolSnpInsert = _model.CaseMstData.InsertUpdateCaseMstLeanIntake(mstdata, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg, SnpEntity, SnpEntity.NameixFi, SnpEntity.AltBdate);
                                        strLAppNo = strApplNo;
                                        CaseMstEntity mstPIPData = new CaseMstEntity();
                                        mstPIPData = mstdata;
                                        // services insert

                                        CaseMSTSER MSTSEREntity = new CaseMSTSER();
                                        //if (dr.PIP_SERVICES_MODE == "TRUE")
                                        //{
                                        // string[] strServices = dr.PIP_SERVICES.Split(',');

                                        foreach (ListItem strserv in dr.Pip_service_list_Mode)
                                        {
                                            if (strserv.Value.ToString().ToUpper() == "TRUE")
                                            {
                                                MSTSEREntity = new CaseMSTSER();
                                                MSTSEREntity.Agency = propCasemstEntity.ApplAgency;
                                                MSTSEREntity.Dept = propCasemstEntity.ApplDept;
                                                MSTSEREntity.Program = propCasemstEntity.ApplProgram;
                                                if (propYear == string.Empty)
                                                    MSTSEREntity.Year = "    ";
                                                else
                                                    MSTSEREntity.Year = propCasemstEntity.ApplYr;


                                                MSTSEREntity.AppNo = propCasemstEntity.ApplNo;
                                                MSTSEREntity.Service = strserv.Text;
                                                MSTSEREntity.AddOperator = BaseForm.UserID;
                                                MSTSEREntity.LstcOperator = BaseForm.UserID;
                                                MSTSEREntity.Mode = "LEANUPD";
                                                _model.CaseMstData.InsertUpdateDelMSTSER(MSTSEREntity);
                                            }
                                        }

                                        //foreach (string strserv in strServices)
                                        //{
                                        //    if (strserv.ToString() != string.Empty)
                                        //    {
                                        //        MSTSEREntity = new CaseMSTSER();
                                        //        MSTSEREntity.Agency = propCasemstEntity.ApplAgency;
                                        //        MSTSEREntity.Dept = propCasemstEntity.ApplDept;
                                        //        MSTSEREntity.Program = propCasemstEntity.ApplProgram;
                                        //        if (propYear == string.Empty)
                                        //            MSTSEREntity.Year = "    ";
                                        //        else
                                        //            MSTSEREntity.Year = propCasemstEntity.ApplYr;


                                        //        MSTSEREntity.AppNo = propCasemstEntity.ApplNo;
                                        //        MSTSEREntity.Service = strserv;
                                        //        MSTSEREntity.AddOperator = BaseForm.UserID;
                                        //        MSTSEREntity.LstcOperator = BaseForm.UserID;
                                        //        MSTSEREntity.Mode = "LEANUPD";
                                        //        _model.CaseMstData.InsertUpdateDelMSTSER(MSTSEREntity);
                                        //    }
                                        //}
                                        //}

                                        // Custom Questions

                                        DataTable dtcustomQues = PIPDATA.GETPIPCUSTORRESPORADDCUST(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, string.Empty, dr.PIP_REG_ID, string.Empty, "ADDCUSTRESP");

                                        //GETLEANADDCUST(string.Empty, dr.PIP_USER_ID, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, "ADDCUSTRESP");
                                        if (dtcustomQues.Rows.Count > 0)
                                        {

                                            foreach (DataRow drcustom in dtcustomQues.Rows)
                                            {
                                                CustomQuestionsEntity custEntity = new CustomQuestionsEntity();

                                                custEntity.ACTAGENCY = propAgency;
                                                custEntity.ACTDEPT = propDept;
                                                custEntity.ACTPROGRAM = propProgram;
                                                if (propYear == string.Empty)
                                                    custEntity.ACTYEAR = "    ";
                                                else
                                                    custEntity.ACTYEAR = propYear;
                                                custEntity.ACTAPPNO = propAppl;
                                                if (drcustom["ADDCUST_SEQ"].ToString() == "0")
                                                {
                                                    custEntity.ACTSNPFAMILYSEQ = propCasemstEntity.FamilySeq;
                                                }
                                                else
                                                {
                                                    custEntity.ACTSNPFAMILYSEQ = drcustom["ADDCUST_SEQ"].ToString();
                                                }
                                                custEntity.ACTCODE = drcustom["ADDCUST_CODE"].ToString();

                                                custEntity.ACTMULTRESP = drcustom["ADDCUST_MULT_RESP"].ToString();

                                                custEntity.ACTNUMRESP = drcustom["ADDCUST_NUM_RESP"].ToString();


                                                custEntity.ACTDATERESP = drcustom["ADDCUST_DATE_RESP"].ToString();

                                                custEntity.ACTALPHARESP = drcustom["ADDCUST_ALPHA_RESP"].ToString();

                                                custEntity.Mode = "LEANUPD";

                                                custEntity.addoperator = BaseForm.UserID;
                                                custEntity.lstcoperator = BaseForm.UserID;
                                                _model.CaseMstData.InsertUpdateADDCUST(custEntity);
                                            }

                                        }


                                        SnpEntity.Type = "MST";

                                        if (dr.PIP_SSN_MODE == "TRUE")
                                            SnpEntity.Ssno = dr.PIP_SSN;

                                        if (dr.PIP_FNAME_MODE == "TRUE")
                                            SnpEntity.NameixFi = dr.PIP_FNAME;
                                        if (dr.PIP_MNAME_MODE == "TRUE")
                                            SnpEntity.NameixMi = dr.PIP_MNAME;
                                        if (dr.PIP_LNAME_MODE == "TRUE")
                                            SnpEntity.NameixLast = dr.PIP_LNAME;
                                        //if (dr.PIP_STATUS_MODE == "TRUE")
                                        //    SnpEntity.Status = dr.PIP_STATUS;
                                        //if (dr.PIP_DOB_MODE == "TRUE")
                                        //    SnpEntity.AltBdate = dr.PIP_DOB;
                                        if (dr.PIP_DOB_MODE == "TRUE")
                                        {
                                            SnpEntity.AltBdate = dr.PIP_DOB;
                                            if (SnpEntity.AltBdate != string.Empty)
                                            {
                                                SnpEntity.DobNa = "0";
                                            }
                                        }
                                        if (dr.PIP_GENDER_MODE == "TRUE")
                                            SnpEntity.Sex = dr.PIP_GENDER;
                                        if (dr.PIP_MARITAL_STATUS_MODE == "TRUE")
                                            SnpEntity.MaritalStatus = dr.PIP_MARITAL_STATUS;
                                        if (dr.PIP_RELATIONSHIP_MODE == "TRUE")
                                            SnpEntity.MemberCode = dr.PIP_RELATIONSHIP;
                                        if (dr.PIP_ETHNIC_MODE == "TRUE")
                                            SnpEntity.Ethnic = dr.PIP_ETHNIC;
                                        if (dr.PIP_RACE_MODE == "TRUE")
                                            SnpEntity.Race = dr.PIP_RACE;
                                        if (dr.PIP_EDUCATION_MODE == "TRUE")
                                            SnpEntity.Education = dr.PIP_EDUCATION;
                                        if (dr.PIP_DISABLE_MODE == "TRUE")
                                            SnpEntity.Disable = dr.PIP_DISABLE;
                                        if (dr.PIP_WORK_STAT_MODE == "TRUE")
                                            SnpEntity.WorkStatus = dr.PIP_WORK_STAT;
                                        if (dr.PIP_SCHOOL_MODE == "TRUE")
                                            SnpEntity.SchoolDistrict = dr.PIP_SCHOOL;
                                        if (dr.PIP_HEALTH_INS_MODE == "TRUE")
                                            SnpEntity.HealthIns = dr.PIP_HEALTH_INS;
                                        if (dr.PIP_VETERAN_MODE == "TRUE")
                                            SnpEntity.Vet = dr.PIP_VETERAN;
                                        if (dr.PIP_FOOD_STAMP_MODE == "TRUE")
                                            SnpEntity.FootStamps = dr.PIP_FOOD_STAMP;
                                        if (dr.PIP_FARMER_MODE == "TRUE")
                                            SnpEntity.Farmer = dr.PIP_FARMER;
                                        if (dr.PIP_WIC_MODE == "TRUE")
                                            SnpEntity.Wic = dr.PIP_WIC;
                                        if (dr.PIP_RELITRAN_MODE == "TRUE")
                                            SnpEntity.Relitran = dr.PIP_RELITRAN;
                                        if (dr.PIP_DRVLIC_MODE == "TRUE")
                                            SnpEntity.Drvlic = dr.PIP_DRVLIC;
                                        if (dr.PIP_MILITARY_STATUS_MODE == "TRUE")
                                            SnpEntity.MilitaryStatus = dr.PIP_MILITARY_STATUS;
                                        //if (dr.PIP_YOUTH_MODE == "TRUE")
                                        //    SnpEntity.Youth = dr.PIP_YOUTH;
                                        if (dr.PIP_PREGNANT_MODE == "TRUE")
                                            SnpEntity.Pregnant = dr.PIP_PREGNANT;
                                        //if (dr.PIP_HEALTH_CODES_MODE == "TRUE")
                                        //    SnpEntity.Health_Codes = dr.PIP_HEALTH_CODES;

                                        string strHealthcodes = SnpEntity.Health_Codes;
                                        foreach (ListItem itemNon in dr.Pip_Healthcodes_list_Mode)
                                        {
                                            if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                            {
                                                if (strHealthcodes.Trim() != string.Empty)
                                                    strHealthcodes = strHealthcodes + "," + itemNon.Text;
                                                else
                                                    strHealthcodes = itemNon.Text;
                                            }
                                        }

                                        SnpEntity.Health_Codes = strHealthcodes;

                                        string strNoncashbenefit = SnpEntity.NonCashBenefits;
                                        foreach (ListItem itemNon in dr.Pip_NonCashbenefit_list_Mode)
                                        {
                                            if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                            {
                                                if (strNoncashbenefit.Trim() != string.Empty)
                                                    strNoncashbenefit = strNoncashbenefit + "," + itemNon.Text;
                                                else
                                                    strNoncashbenefit = itemNon.Text;
                                            }
                                        }

                                        //if (dr.PIP_NCASHBEN_MODE == "TRUE")
                                        SnpEntity.NonCashBenefits = strNoncashbenefit;
                                        //if (dr.PIP_SUFFIX_MODE == "TRUE")
                                        //    SnpEntity.SnpSuffix = dr.PIP_SUFFIX;
                                        SnpEntity.LstcOperator = BaseForm.UserID;
                                        SnpEntity.AddOperator = BaseForm.UserID;
                                        //  SnpEntity.Status = "A";

                                        SnpEntity.Mode = "Edit";

                                        SnpEntity.SsnSearchType = string.Empty;

                                        if (SnpEntity.Ssno.Trim() == string.Empty)
                                            SnpEntity.Ssno = "000000000";
                                        string strOutFamilyseq = SnpEntity.FamilySeq;
                                        CaseSnpEntity snpPIPData = SnpEntity;
                                        if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                        {
                                            if (SnpEntity.FamilySeq.Trim() == string.Empty)
                                            {
                                                SnpEntity.FamilySeq = strOutFamilyseq;
                                            }
                                            PIPDATA.InsertUpdatePIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, SnpEntity.Agency, SnpEntity.Dept, SnpEntity.Program, SnpEntity.Year, SnpEntity.App, SnpEntity.FamilySeq, dr.PIP_AGENCY, dr.PIP_AGY.ToString(), dr.PIP_REG_ID.ToString(), dr.PIP_ID.ToString(), BaseForm.UserID, "A", string.Empty);

                                            UpdateLeanIntakeData(dr.PIP_REG_ID.ToString(), dr.PIP_ID, SnpEntity.Agency, SnpEntity.Dept, SnpEntity.Program, SnpEntity.Year, SnpEntity.App, "U");
                                            //if (caseincomeApp.Count == 0)
                                            // {
                                            if (dr.PIP_INCOME_TYPES_MODE == "TRUE")
                                            {
                                                if (dr.PIP_INCOME_TYPES.Trim() != string.Empty)
                                                {
                                                    string[] strINCOMETypes = dr.PIP_INCOME_TYPES.ToString().Split(',');
                                                    foreach (string strIncomeType in strINCOMETypes)
                                                    {
                                                        if (strIncomeType.ToString() != string.Empty)
                                                        {
                                                            List<CaseIncomeEntity> incometypecount = caseincomeApp.FindAll(u => u.Type == strIncomeType);
                                                            if (incometypecount.Count == 0)
                                                            {
                                                                CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                                                incomeupdatedetails.Agency = propAgency;
                                                                incomeupdatedetails.Dept = propDept;
                                                                incomeupdatedetails.Program = propProgram;
                                                                incomeupdatedetails.App = propCasemstEntity.ApplNo;
                                                                incomeupdatedetails.Year = propYear;
                                                                incomeupdatedetails.FamilySeq = propCasemstEntity.FamilySeq;
                                                                incomeupdatedetails.Type = strIncomeType;
                                                                incomeupdatedetails.Interval = "E";
                                                                incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                                incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                                incomeupdatedetails.Mode = string.Empty;
                                                                incomeupdatedetails.Exclude = "N";
                                                                incomeupdatedetails.Factor = "1.00";


                                                                if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                                {
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            // }
                                        }
                                        //CaseMstEntity _casemstentity = _model.CaseMstData.GetCaseMST(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

                                        //List<CaseSnpEntity> _snpentitylist = _model.CaseMstData.GetCaseSnpadpyn(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);
                                        //CaseSnpEntity _snpMainCaptainEntity = _snpentitylist.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);

                                        //CaseMstEntity _casemstentity = dritem.Cells["gvtName"].Tag as CaseMstEntity;
                                        CaseSnpEntity _snpMainCaptainEntity = dritem.Cells["gvtSSN"].Tag as CaseSnpEntity;

                                        if (_snpMainCaptainEntity != null)
                                        {
                                            CheckHistoryTableData(mstPIPData, snpPIPData, propMainCasemstEntity, _snpMainCaptainEntity, true);
                                        }
                                    }
                                    else
                                    {
                                        CaseSnpEntity SnpEntity = new CaseSnpEntity();
                                        CaseSnpEntity _snpMemberCaptainEntity = dritem.Cells["gvtSSN"].Tag as CaseSnpEntity;
                                        SnpEntity = propCasesnpEntity.Find(u => u.NameixFi.ToUpper().FirstOrDefault() == dr.PIP_FNAME.ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                                        if (SnpEntity == null)
                                        {
                                            SnpEntity = propCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dr.PIP_FNAME.ToUpper() && u.NameixLast.ToUpper() == dr.PIP_LNAME.ToUpper());

                                        }

                                        if (SnpEntity == null)
                                        {
                                            SnpEntity = new Model.Objects.CaseSnpEntity();
                                            SnpEntity.Agency = propCasemstEntity.ApplAgency;
                                            SnpEntity.Dept = propCasemstEntity.ApplDept;
                                            SnpEntity.Program = propCasemstEntity.ApplProgram;
                                            SnpEntity.Year = propCasemstEntity.ApplYr;
                                            SnpEntity.App = propCasemstEntity.ApplNo;
                                            SnpEntity.Status = "A";
                                        }
                                        if (SnpEntity != null)
                                        {
                                            if (dritem.Cells["gvtType"].Value.ToString() == "I")
                                            {
                                                SnpEntity.SsnSearchType = "I";
                                            }
                                            else
                                            {
                                                // caseincomeMembers = new List<CaseIncomeEntity>(); propcaseIncomeEntity.FindAll(u => u.FamilySeq == SnpEntity.FamilySeq);
                                                if (dr.PIP_SSN_MODE == "TRUE")
                                                    SnpEntity.Ssno = dr.PIP_SSN;

                                                if (dr.PIP_FNAME_MODE == "TRUE")
                                                    SnpEntity.NameixFi = dr.PIP_FNAME;
                                                if (dr.PIP_MNAME_MODE == "TRUE")
                                                    SnpEntity.NameixMi = dr.PIP_MNAME;
                                                if (dr.PIP_LNAME_MODE == "TRUE")
                                                    SnpEntity.NameixLast = dr.PIP_LNAME;
                                                if (dr.PIP_DOB_MODE == "TRUE")
                                                {
                                                    SnpEntity.AltBdate = dr.PIP_DOB;
                                                    if (SnpEntity.AltBdate != string.Empty)
                                                    {
                                                        SnpEntity.DobNa = "0";
                                                    }
                                                }
                                                if (dr.PIP_GENDER_MODE == "TRUE")
                                                    SnpEntity.Sex = dr.PIP_GENDER;
                                                if (dr.PIP_MARITAL_STATUS_MODE == "TRUE")
                                                    SnpEntity.MaritalStatus = dr.PIP_MARITAL_STATUS;
                                                if (dr.PIP_RELATIONSHIP_MODE == "TRUE")
                                                    SnpEntity.MemberCode = dr.PIP_RELATIONSHIP;
                                                if (dr.PIP_ETHNIC_MODE == "TRUE")
                                                    SnpEntity.Ethnic = dr.PIP_ETHNIC;
                                                if (dr.PIP_RACE_MODE == "TRUE")
                                                    SnpEntity.Race = dr.PIP_RACE;
                                                if (dr.PIP_EDUCATION_MODE == "TRUE")
                                                    SnpEntity.Education = dr.PIP_EDUCATION;
                                                if (dr.PIP_DISABLE_MODE == "TRUE")
                                                    SnpEntity.Disable = dr.PIP_DISABLE;
                                                if (dr.PIP_WORK_STAT_MODE == "TRUE")
                                                    SnpEntity.WorkStatus = dr.PIP_WORK_STAT;
                                                if (dr.PIP_SCHOOL_MODE == "TRUE")
                                                    SnpEntity.SchoolDistrict = dr.PIP_SCHOOL;
                                                if (dr.PIP_HEALTH_INS_MODE == "TRUE")
                                                    SnpEntity.HealthIns = dr.PIP_HEALTH_INS;
                                                if (dr.PIP_VETERAN_MODE == "TRUE")
                                                    SnpEntity.Vet = dr.PIP_VETERAN;
                                                if (dr.PIP_FOOD_STAMP_MODE == "TRUE")
                                                    SnpEntity.FootStamps = dr.PIP_FOOD_STAMP;
                                                if (dr.PIP_FARMER_MODE == "TRUE")
                                                    SnpEntity.Farmer = dr.PIP_FARMER;
                                                if (dr.PIP_WIC_MODE == "TRUE")
                                                    SnpEntity.Wic = dr.PIP_WIC;
                                                if (dr.PIP_RELITRAN_MODE == "TRUE")
                                                    SnpEntity.Relitran = dr.PIP_RELITRAN;
                                                if (dr.PIP_DRVLIC_MODE == "TRUE")
                                                    SnpEntity.Drvlic = dr.PIP_DRVLIC;
                                                if (dr.PIP_MILITARY_STATUS_MODE == "TRUE")
                                                    SnpEntity.MilitaryStatus = dr.PIP_MILITARY_STATUS;
                                                //if (dr.PIP_YOUTH_MODE == "TRUE")
                                                //    SnpEntity.Youth = dr.PIP_YOUTH;
                                                if (dr.PIP_PREGNANT_MODE == "TRUE")
                                                    SnpEntity.Pregnant = dr.PIP_PREGNANT;
                                                //if (dr.PIP_HEALTH_CODES_MODE == "TRUE")
                                                //    SnpEntity.Health_Codes = dr.PIP_HEALTH_CODES;

                                                string strHealthcodes = SnpEntity.Health_Codes;
                                                foreach (ListItem itemNon in dr.Pip_Healthcodes_list_Mode)
                                                {
                                                    if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                                    {
                                                        if (strHealthcodes.Trim() != string.Empty)
                                                            strHealthcodes = strHealthcodes + "," + itemNon.Text;
                                                        else
                                                            strHealthcodes = itemNon.Text;
                                                    }
                                                }

                                                SnpEntity.Health_Codes = strHealthcodes;

                                                //if (dr.PIP_NCASHBEN_MODE == "TRUE")
                                                //    SnpEntity.NonCashBenefits = dr.PIP_NCASHBEN;
                                                string strNoncashbenefit = SnpEntity.NonCashBenefits;
                                                foreach (ListItem itemNon in dr.Pip_NonCashbenefit_list_Mode)
                                                {
                                                    if (itemNon.Value.ToString().ToUpper() == "TRUE")
                                                    {
                                                        if (strNoncashbenefit.Trim() != string.Empty)
                                                            strNoncashbenefit = strNoncashbenefit + "," + itemNon.Text;
                                                        else
                                                            strNoncashbenefit = itemNon.Text;
                                                    }
                                                }
                                                SnpEntity.NonCashBenefits = strNoncashbenefit;
                                                SnpEntity.LstcOperator = BaseForm.UserID;
                                                SnpEntity.AddOperator = BaseForm.UserID;


                                                /// SnpEntity.Status = "A";

                                                SnpEntity.Type = "SNP";

                                                SnpEntity.SsnSearchType = string.Empty;
                                                if (SnpEntity.Ssno.Trim() == string.Empty)
                                                    SnpEntity.Ssno = "000000000";
                                            }
                                            string strOutFamilyseq = SnpEntity.FamilySeq;
                                            CaseSnpEntity snpPipData = SnpEntity;
                                            if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                            {
                                                if (SnpEntity.FamilySeq.Trim() == string.Empty)
                                                {
                                                    SnpEntity.FamilySeq = strOutFamilyseq;
                                                }
                                                PIPDATA.InsertUpdatePIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, SnpEntity.Agency, SnpEntity.Dept, SnpEntity.Program, SnpEntity.Year, SnpEntity.App, SnpEntity.FamilySeq, dr.PIP_AGENCY, dr.PIP_AGY.ToString(), dr.PIP_REG_ID.ToString(), dr.PIP_ID.ToString(), BaseForm.UserID, "A", string.Empty);

                                                UpdateLeanIntakeData(dr.PIP_REG_ID.ToString(), dr.PIP_ID, SnpEntity.Agency, SnpEntity.Dept, SnpEntity.Program, SnpEntity.Year, SnpEntity.App, "U");

                                                boolSnpInsert = true;
                                                if (dritem.Cells["gvtType"].Value.ToString() != "I")
                                                {
                                                    if (dritem.Cells["gvtType"].Value.ToString() == "N")
                                                    {
                                                        if (dr.PIP_INCOME_TYPES.Trim() != string.Empty)
                                                        {
                                                            string[] strINCOMETypes = dr.PIP_INCOME_TYPES.ToString().Split(',');
                                                            foreach (string strIncomeType in strINCOMETypes)
                                                            {

                                                                if (strIncomeType.ToString() != string.Empty)
                                                                {

                                                                    CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                                                    incomeupdatedetails.Agency = propAgency;
                                                                    incomeupdatedetails.Dept = propDept;
                                                                    incomeupdatedetails.Program = propProgram;
                                                                    incomeupdatedetails.App = propCasemstEntity.ApplNo;
                                                                    incomeupdatedetails.Year = propYear;
                                                                    incomeupdatedetails.FamilySeq = strOutFamilyseq;
                                                                    incomeupdatedetails.Type = strIncomeType;
                                                                    incomeupdatedetails.Interval = "E";
                                                                    incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                                    incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                                    incomeupdatedetails.Mode = string.Empty;
                                                                    incomeupdatedetails.Exclude = "N";
                                                                    incomeupdatedetails.Factor = "1.00";


                                                                    if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                                    {
                                                                    }

                                                                }
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {

                                                        CaseSnpEntity SnpMemEntity = propCasesnpEntity.Find(u => u.NameixFi.ToUpper().FirstOrDefault() == dr.PIP_FNAME.ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));
                                                        if (SnpMemEntity != null)
                                                        {
                                                            SnpMemEntity = propCasesnpEntity.Find(u => u.NameixFi.ToUpper() == dr.PIP_FNAME.ToUpper() && u.NameixLast.ToUpper() == dr.PIP_LNAME.ToUpper());
                                                        }

                                                        if (SnpMemEntity != null)
                                                        {
                                                            List<CaseIncomeEntity> caseincomeMembers = propcaseIncomeEntity.FindAll(u => u.FamilySeq == SnpMemEntity.FamilySeq);
                                                            //if (caseincomeMembers.Count == 0)
                                                            //{
                                                            if (dr.PIP_INCOME_TYPES_MODE == "TRUE")
                                                            {
                                                                if (dr.PIP_INCOME_TYPES.Trim() != string.Empty)
                                                                {
                                                                    string[] strINCOMETypes = dr.PIP_INCOME_TYPES.ToString().Split(',');
                                                                    foreach (string strIncomeType in strINCOMETypes)
                                                                    {

                                                                        if (strIncomeType.ToString() != string.Empty)
                                                                        {
                                                                            List<CaseIncomeEntity> incometypecount = caseincomeMembers.FindAll(u => u.Type == strIncomeType);
                                                                            if (incometypecount.Count == 0)
                                                                            {
                                                                                CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                                                                incomeupdatedetails.Agency = propAgency;
                                                                                incomeupdatedetails.Dept = propDept;
                                                                                incomeupdatedetails.Program = propProgram;
                                                                                incomeupdatedetails.App = propCasemstEntity.ApplNo;
                                                                                incomeupdatedetails.Year = propYear;
                                                                                incomeupdatedetails.FamilySeq = SnpMemEntity.FamilySeq;
                                                                                incomeupdatedetails.Type = strIncomeType;
                                                                                incomeupdatedetails.Interval = "E";
                                                                                incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                                                incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                                                incomeupdatedetails.Mode = string.Empty;
                                                                                incomeupdatedetails.Exclude = "N";
                                                                                incomeupdatedetails.Factor = "1.00";


                                                                                if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                                                {
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //}
                                                        }
                                                    }
                                                }

                                            }
                                            //CaseMstEntity _casemstentity = _model.CaseMstData.GetCaseMST(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

                                            //List<CaseSnpEntity> _snpentitylist = _model.CaseMstData.GetCaseSnpadpyn(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

                                            //CaseSnpEntity _snpMemberCaptainEntity = _snpentitylist.Find(u => u.NameixFi.ToUpper() == dr.PIP_FNAME.ToUpper() && u.NameixLast.ToUpper() == dr.PIP_LNAME.ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dr.PIP_DOB));

                                            if (_snpMemberCaptainEntity != null)
                                            {
                                                CheckHistoryTableData(mstdata, snpPipData, propCasemstEntity, _snpMemberCaptainEntity, false);

                                            }
                                        }
                                    }
                                }
                                // first else closed
                            }

                        }
                    }
                }

                if (intUpdate > 0)
                {
                    if (boolSnpInsert)
                    {
                        if (strFormType == string.Empty && _propCloseFormStatus == string.Empty)
                        {
                            BaseForm.BaseAgency = propCasemstEntity.ApplAgency;
                            BaseForm.BaseDept = propCasemstEntity.ApplDept;
                            BaseForm.BaseProg = propCasemstEntity.ApplProgram;
                            BaseForm.BaseYear = propCasemstEntity.ApplYr == string.Empty ? "    " : propCasemstEntity.ApplYr;
                            BaseForm.BaseApplicationNo = propCasemstEntity.ApplNo;
                            BaseForm.BasePIPDragSwitch = "Y";
                        }
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    if (strFormType == string.Empty)
                    {
                        CommonFunctions.MessageBoxDisplay("No Differences in Applicant and Member data both in CAPTAIN and PIP");
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("No Differences in Applicant and Member data both in CAPTAIN and Recent CAPTAIN");
                    }
                }

            }
        }

        bool chkDSSMode(DataRow _drCurr)
        {
            bool _strMode = false;

            if (_drCurr["SSNmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["FNAMEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MNAMEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LNAMEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DOBmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["GENDERmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["ETHENICmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["RACEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["EDUCATIONmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DISABLEDmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MILTARYmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["WORKSTATUSmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["HOUSENOmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["STREETmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["ZIPmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["ZIPCODEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["STATEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["CITYmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["PHONEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["CELLmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LANGUAGEmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["HOUSINGmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["Emailmode"].ToString() == "FALSE") { _strMode = true; return _strMode; }

            /*Landlrd Info*/
            if (_drCurr["LLRFnameMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRLnameMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRPhoneMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRCityMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRStateMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRStreetMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRHouseNoMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRZipMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["LLRZipCodeMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }

            /*CASE DIFF*/
            if (_drCurr["DiffSateMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DiffCityMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DiffHNMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DiffStreetMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DiffPhoneMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DiffZipMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DiffZipPlusMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }

            /*LIHEP Questions*/
            if (_drCurr["MST_LPM_0001Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0002Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0003Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0004Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0005Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0006Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0007Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0008Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0009Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0010Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["MST_LPM_0011Mode"].ToString() == "FALSE") { _strMode = true; return _strMode; }

            if (_drCurr["RENTMORTMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["RELATIONMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DISYOUTHMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["DWELLINGMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["PRIMPAYHEATMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }
            if (_drCurr["PRIMSRCHEARMode"].ToString() == "FALSE") { _strMode = true; return _strMode; }

            return _strMode;

        }



        private void gvwCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwCustomer.Rows.Count > 0)
            {
                // gvwSubdetails.CellValueChanged -= gvwSubdetails_CellValueChanged;
                chkSelectAll.CheckedChanged -= new System.EventHandler(chkSelectAll_CheckedChanged);
                gvwSubdetails.Rows.Clear();
                lblMessage2.Text = string.Empty;
                lblUpdateMessage.Visible = chkSelectAll.Visible = btnLeanData.Visible = true;
                int intRowIndex = 0;
                // chkSelectAll.Checked = false;

                if (strFormType == "DSSXML")
                {
                    CaseSnpEntity caseSnpEntity = gvwCustomer.SelectedRows[0].Cells["gvtSSN"].Tag as CaseSnpEntity;
                    DataRow _drDSSXMLitem = gvwCustomer.SelectedRows[0].Tag as DataRow;

                    btnAddress.Visible = false;
                    pnlDSSConfirm.Visible = false;

                    // LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;
                    if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N" && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "I")
                    {
                        if (gvwCustomer.SelectedRows[0].Selected)
                        {
                            string strChkvalue = gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value.ToString().ToUpper() == null ? "FALSE" : gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value.ToString().ToUpper();
                            if (strChkvalue == "TRUE")
                            {

                                if (chkDSSMode(_drDSSXMLitem))
                                {
                                    chkSelectAll.CheckedChanged -= new EventHandler(chkSelectAll_CheckedChanged);
                                    chkSelectAll.Checked = false;
                                    chkSelectAll.CheckedChanged += new EventHandler(chkSelectAll_CheckedChanged);
                                }
                                else
                                {
                                    chkSelectAll.CheckedChanged -= new EventHandler(chkSelectAll_CheckedChanged);
                                    chkSelectAll.Checked = true;
                                    chkSelectAll.CheckedChanged += new EventHandler(chkSelectAll_CheckedChanged);
                                }
                            }
                            else
                            {
                                chkSelectAll.CheckedChanged -= new EventHandler(chkSelectAll_CheckedChanged);
                                chkSelectAll.Checked = false;
                                chkSelectAll.CheckedChanged += new EventHandler(chkSelectAll_CheckedChanged);
                            }
                        }

                        string stroldvalue = string.Empty; string strNewvalue = string.Empty; string strFieldType = string.Empty; string _DssxmlDesc = "", _CAPDesc = "";
                        if (_drDSSXMLitem != null)
                        {
                            string MIName = string.Empty;
                            if (!string.IsNullOrEmpty(_drDSSXMLitem["Middle_Name__c"].ToString().Trim()))
                                MIName = _drDSSXMLitem["Middle_Name__c"].ToString().Trim().Substring(0, 1);

                            #region SSN
                            string tmpSSN = string.Empty;
                            if (!string.IsNullOrEmpty(_drDSSXMLitem["SSN__c"].ToString().Trim()))
                            {
                                tmpSSN = _drDSSXMLitem["SSN__c"].ToString().Trim().Replace("-", "");
                            }
                            if (tmpSSN != caseSnpEntity.Ssno.ToString().Trim())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("SSN", LookupDataAccess.GetPhoneSsnNoFormat(tmpSSN), LookupDataAccess.GetPhoneSsnNoFormat(caseSnpEntity.Ssno.ToString()), _drDSSXMLitem["SSNmode"].ToString() == "TRUE" ? true : false, string.Empty);
                            }
                            #endregion
                            #region First Name
                            if (_drDSSXMLitem["First_Name__c"].ToString().Trim().ToUpper() != caseSnpEntity.NameixFi.ToString().Trim().ToUpper())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("First Name", _drDSSXMLitem["First_Name__c"].ToString(), caseSnpEntity.NameixFi.ToString(), _drDSSXMLitem["FNAMEmode"].ToString() == "TRUE" ? true : false, string.Empty);
                            }
                            #endregion
                            #region Middle Name
                            if (MIName.ToUpper() != caseSnpEntity.NameixMi.ToString().Trim().ToUpper())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Middle Name", MIName, caseSnpEntity.NameixMi.ToString(), _drDSSXMLitem["MNAMEmode"].ToString() == "TRUE" ? true : false, string.Empty);
                            }
                            #endregion
                            #region Last Name
                            if (_drDSSXMLitem["Last_Name__c"].ToString().Trim().ToUpper() != caseSnpEntity.NameixLast.ToString().Trim().ToUpper())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Last Name", _drDSSXMLitem["Last_Name__c"].ToString(), caseSnpEntity.NameixLast.ToString(), _drDSSXMLitem["LNAMEmode"].ToString() == "TRUE" ? true : false, string.Empty);
                            }
                            #endregion
                            #region DOB

                            if (LookupDataAccess.Getdate(_drDSSXMLitem["Date_of_Birth__c"].ToString().Trim().ToUpper()) != LookupDataAccess.Getdate(caseSnpEntity.AltBdate.ToString().Trim().ToUpper()))
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Dob", LookupDataAccess.Getdate(_drDSSXMLitem["Date_of_Birth__c"].ToString()), LookupDataAccess.Getdate(caseSnpEntity.AltBdate.ToString()), _drDSSXMLitem["DOBmode"].ToString() == "TRUE" ? true : false, string.Empty);
                                //gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                            }
                            #endregion
                            #region Gender
                            /************* GENDER ****************/
                            stroldvalue = strNewvalue = _DssxmlDesc = _CAPDesc = string.Empty;
                            List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                            stroldvalue = TMS00141Form.getGenderCode(_drDSSXMLitem["Sex__c"].ToString().Trim());
                            _DssxmlDesc = _drDSSXMLitem["Sex__c"].ToString().Trim();
                            //CAPTAIN Value
                            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.Sex.ToString().Trim());
                            if (comNewvalue != null)
                            {
                                strNewvalue = comNewvalue.Code;
                                _CAPDesc = comNewvalue.Desc;
                                if (comNewvalue.Active.ToUpper() == "N")
                                    strFieldType = "2";
                            }
                            else
                            {
                                if (_drDSSXMLitem["Sex__c"].ToString() != string.Empty)
                                    strFieldType = "1";
                                else
                                    strFieldType = "0";
                            }

                            if (stroldvalue != strNewvalue)
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Gender", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["GENDERmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                if (strFieldType == "2" || strFieldType == "1")
                                {
                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                    if (strFieldType == "1")
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                }
                            }

                            /*************************************/
                            #endregion
                            #region Ethenic

                            stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, "**", "**", "**", string.Empty);
                            stroldvalue = TMS00141Form.getEthnicCode(_drDSSXMLitem["Ethnicity__c"].ToString().Trim());
                            _DssxmlDesc = _drDSSXMLitem["Ethnicity__c"].ToString().Trim();
                            comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.Ethnic.ToString().Trim());
                            if (comNewvalue != null)
                            {
                                strNewvalue = comNewvalue.Code;
                                _CAPDesc = comNewvalue.Desc;
                                if (comNewvalue.Active.ToUpper() == "N")
                                    strFieldType = "2";
                            }
                            else
                            {
                                if (_drDSSXMLitem["Ethnicity__c"].ToString() != string.Empty)
                                    strFieldType = "1";
                                else
                                    strFieldType = "0";
                            }

                            if (stroldvalue.ToLower() != strNewvalue.ToLower())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Ethnicity", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["ETHENICmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                if (strFieldType == "2" || strFieldType == "1")
                                {
                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                    if (strFieldType == "1")
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                }
                            }

                            #endregion
                            #region Race

                            stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, "**", "**", "**", string.Empty);

                            //comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Race.ToString().Trim());
                            //if (comOldvalue != null)
                            //{
                            //    stroldvalue = comOldvalue.Desc;
                            //}

                            string Race = _drDSSXMLitem["Additional_Race__c"].ToString();
                            if (!string.IsNullOrEmpty(Race.Trim()))
                            {
                                string[] RaceSpilt = Race.Split(';');
                                if (RaceSpilt.Length > 1) Race = "Multi-Race";
                            }

                            stroldvalue = TMS00141Form.getRaceCode(Race, BaseForm.BaseAgencyControlDetails.AgyShortName);
                            if (Race == "Multi-Race")
                                Race = "Biracial/Multi-Race";

                            _DssxmlDesc = Race;// _drDSSXMLitem["Additional_Race__c"].ToString().Trim();

                            comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.Race.ToString().Trim());
                            if (comNewvalue != null)
                            {
                                strNewvalue = comNewvalue.Code;
                                _CAPDesc = comNewvalue.Desc;
                                if (comNewvalue.Active.ToUpper() == "N")
                                    strFieldType = "2";
                            }
                            else
                            {
                                if (_drDSSXMLitem["Additional_Race__c"].ToString() != string.Empty)
                                    strFieldType = "1";
                                else
                                    strFieldType = "0";
                            }

                            if (stroldvalue.ToLower() != strNewvalue.ToLower())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Race", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["RACEmode"].ToString() == "TRUE" ? true : false, strFieldType);

                                if (strFieldType == "2" || strFieldType == "1")
                                {
                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                    if (strFieldType == "1")
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                }
                            }

                            #endregion
                            #region Education
                            stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, "**", "**", "**", string.Empty);

                            //comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Education.ToString().Trim());
                            //if (comOldvalue != null)
                            //{
                            //    stroldvalue = comOldvalue.Desc;
                            //}
                            stroldvalue = TMS00141Form.getEducationCode(_drDSSXMLitem["Highest_Education_Level__c"].ToString().Trim(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                            _DssxmlDesc = _drDSSXMLitem["Highest_Education_Level__c"].ToString().Trim();

                            comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.Education.ToString().Trim());
                            if (comNewvalue != null)
                            {
                                strNewvalue = comNewvalue.Code;
                                _CAPDesc = comNewvalue.Desc;
                                if (comNewvalue.Active.ToUpper() == "N")
                                    strFieldType = "2";
                            }
                            else
                            {
                                if (_drDSSXMLitem["Highest_Education_Level__c"].ToString() != string.Empty)
                                    strFieldType = "1";
                                else
                                    strFieldType = "0";
                            }

                            if (stroldvalue.ToLower() != strNewvalue.ToLower())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Education", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["EDUCATIONmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                if (strFieldType == "2" || strFieldType == "1")
                                {
                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                    if (strFieldType == "1")
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                }

                            }
                            #endregion
                            #region Disabled
                            stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, "**", "**", "**", string.Empty);

                            //comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Disable.ToString().Trim());
                            //if (comOldvalue != null)
                            //{
                            //    stroldvalue = comOldvalue.Desc;
                            //}
                            stroldvalue = TMS00141Form.getDisableCode(_drDSSXMLitem["Are_you_with_disability__c"].ToString().Trim());
                            _DssxmlDesc = _drDSSXMLitem["Are_you_with_disability__c"].ToString().Trim();
                            comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.Disable.ToString().Trim());
                            if (comNewvalue != null)
                            {
                                strNewvalue = comNewvalue.Code; _CAPDesc = comNewvalue.Desc;
                                if (comNewvalue.Active.ToUpper() == "N")
                                    strFieldType = "2";
                            }
                            else
                            {
                                if (_drDSSXMLitem["Are_you_with_disability__c"].ToString() != string.Empty)
                                    strFieldType = "1";
                                else
                                    strFieldType = "0";
                            }

                            if (stroldvalue.ToLower() != strNewvalue.ToLower())
                            {
                                //intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Disable", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["DISABLEDmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                if (strFieldType == "2" || strFieldType == "1")
                                {
                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                    if (strFieldType == "1")
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                }
                            }
                            #endregion
                            #region Miltry Status

                            stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MilitaryStatus, "**", "**", "**", string.Empty);

                            //comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.MilitaryStatus.ToString().Trim());
                            //if (comOldvalue != null)
                            //{
                            //    stroldvalue = comOldvalue.Desc;
                            //}
                            stroldvalue = TMS00141Form.getMiltaryStatusCode(_drDSSXMLitem["Have_you_served_in_military__c"].ToString().Trim());
                            _DssxmlDesc = _drDSSXMLitem["Have_you_served_in_military__c"].ToString().Trim();
                            comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.MilitaryStatus.ToString().Trim());
                            if (comNewvalue != null)
                            {
                                strNewvalue = comNewvalue.Code;
                                _CAPDesc = comNewvalue.Desc;
                                if (comNewvalue.Active.ToUpper() == "N")
                                    strFieldType = "2";
                            }
                            else
                            {
                                if (_drDSSXMLitem["Have_you_served_in_military__c"].ToString() != string.Empty)
                                    strFieldType = "1";
                                else
                                    strFieldType = "0";
                            }

                            if (stroldvalue.ToLower() != strNewvalue.ToLower())
                            {
                                // intValidmode = intValidmode + 1;
                                intRowIndex = gvwSubdetails.Rows.Add("Military Status", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["MILTARYmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                if (strFieldType == "2" || strFieldType == "1")
                                {
                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                    if (strFieldType == "1")
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                }

                            }
                            #endregion
                            #region Relation
                            string XMLRelationCode = TMS00141Form.getRelation(_drDSSXMLitem["CEAP_Relationships__c"].ToString(), _drDSSXMLitem["Sex__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                            string XMLRelationDesc = _drDSSXMLitem["CEAP_Relationships__c"].ToString();
                            if (XMLRelationCode == string.Empty && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() == "A")
                                XMLRelationCode = "C";
                            string CAPRelatioCode = caseSnpEntity.MemberCode;
                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, "**", "**", "**", string.Empty);
                            var tempRlCode = listAgytabls.Find(u => u.Code == caseSnpEntity.MemberCode.ToString().Trim());
                            string CAPRelaionDesc = "";
                            if (tempRlCode != null)
                            {
                                CAPRelaionDesc = tempRlCode.Desc;
                            }

                            if (XMLRelationCode.ToString().Trim().ToUpper() != CAPRelatioCode.Trim().ToUpper())
                            {
                                intRowIndex = gvwSubdetails.Rows.Add("Relation", XMLRelationDesc.ToString().Trim(), CAPRelaionDesc, _drDSSXMLitem["RELATIONmode"].ToString() == "TRUE" ? true : false,
                                    XMLRelationCode.ToString().Trim() == string.Empty ? "0" : string.Empty);
                            }
                            #endregion

                            if (_drDSSXMLitem.Table.Columns.Contains("Employment_Status__c"))
                            {
                                //string x = "";
                                #region Work Status
                                stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, "**", "**", "**", string.Empty);
                                stroldvalue = TMS00141Form.getWorkStatusCode(_drDSSXMLitem["Employment_Status__c"].ToString().Trim());
                                _DssxmlDesc = _drDSSXMLitem["Employment_Status__c"].ToString().Trim();

                                comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.WorkStatus.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Code;
                                    _CAPDesc = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (_drDSSXMLitem["Employment_Status__c"].ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (stroldvalue.ToLower() != strNewvalue.ToLower())
                                {
                                    //intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Work Status", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["WORKSTATUSmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                #endregion
                                #region Disconnected Youth
                                string XMLDisYouthCode = TMS00141Form.getYouthCode(_drDSSXMLitem["Student_Status__c"].ToString(), _drDSSXMLitem["Employment_Status__c"].ToString());
                                string XMLDisYouthDesc = _drDSSXMLitem["Student_Status__c"].ToString();

                                string CAPDiscYouthCode = caseSnpEntity.Youth;

                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DisconnectedYouth, "**", "**", "**", string.Empty);
                                var tempdiscythCode = listAgytabls.Find(u => u.Code == caseSnpEntity.Youth.ToString().Trim());
                                string CAPDiscYouthDesc = "";
                                if (tempdiscythCode != null)
                                {
                                    CAPDiscYouthDesc = tempdiscythCode.Desc;
                                }

                                if (XMLDisYouthCode.ToString().Trim().ToUpper() != CAPDiscYouthCode.Trim().ToUpper())
                                {
                                    intRowIndex = gvwSubdetails.Rows.Add("Disconnected Youth", XMLDisYouthDesc.ToString().Trim(), CAPDiscYouthDesc, _drDSSXMLitem["DISYOUTHMode"].ToString() == "TRUE" ? true : false,
                                        XMLDisYouthCode.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                }
                                #endregion
                            }
                            else
                            {
                                string famSeq = "1";
                                string Cname = "HOUSEHOLDMEMBERS" + famSeq + "_Id";
                                if (_dsAllDSSXML.Tables.Contains("Employment_Status__c"))
                                {
                                    foreach (DataRow drxml in _dsAllDSSXML.Tables["Employment_Status__c"].Rows)
                                    {
                                        if (drxml[Cname].ToString() == "0")
                                        {
                                            #region Work Status
                                            stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, "**", "**", "**", string.Empty);
                                            stroldvalue = TMS00141Form.getWorkStatusCode(drxml["Employment_Status__c_Text"].ToString().Trim());
                                            _DssxmlDesc = drxml["Employment_Status__c_Text"].ToString().ToString().Trim();

                                            comNewvalue = listAgytabls.Find(u => u.Code == caseSnpEntity.WorkStatus.ToString().Trim());
                                            if (comNewvalue != null)
                                            {
                                                strNewvalue = comNewvalue.Code;
                                                _CAPDesc = comNewvalue.Desc;
                                                if (comNewvalue.Active.ToUpper() == "N")
                                                    strFieldType = "2";
                                            }
                                            else
                                            {
                                                if (_drDSSXMLitem["Employment_Status__c"].ToString() != string.Empty)
                                                    strFieldType = "1";
                                                else
                                                    strFieldType = "0";
                                            }

                                            if (stroldvalue.ToLower() != strNewvalue.ToLower())
                                            {
                                                //intValidmode = intValidmode + 1;
                                                intRowIndex = gvwSubdetails.Rows.Add("Work Status", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["WORKSTATUSmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                                if (strFieldType == "2" || strFieldType == "1")
                                                {
                                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                                    if (strFieldType == "1")
                                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                                }
                                            }
                                            #endregion
                                            #region Disconnected Youth
                                            string XMLDisYouthCode = TMS00141Form.getYouthCode(_drDSSXMLitem["Student_Status__c"].ToString(), drxml["Employment_Status__c_Text"].ToString());
                                            string XMLDisYouthDesc = _drDSSXMLitem["Student_Status__c"].ToString();

                                            string CAPDiscYouthCode = caseSnpEntity.Youth;

                                            listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DisconnectedYouth, "**", "**", "**", string.Empty);
                                            var tempdiscythCode = listAgytabls.Find(u => u.Code == caseSnpEntity.Youth.ToString().Trim());
                                            string CAPDiscYouthDesc = "";
                                            if (tempdiscythCode != null)
                                            {
                                                CAPDiscYouthDesc = tempdiscythCode.Desc;
                                            }

                                            if (XMLDisYouthCode.ToString().Trim().ToUpper() != CAPDiscYouthCode.Trim().ToUpper())
                                            {
                                                intRowIndex = gvwSubdetails.Rows.Add("Disconnected Youth", XMLDisYouthDesc.ToString().Trim(), CAPDiscYouthDesc, _drDSSXMLitem["DISYOUTHMode"].ToString() == "TRUE" ? true : false,
                                                    XMLDisYouthCode.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                            }
                                            #endregion

                                            break;
                                        }
                                    }
                                }
                            }

                            #region Applicant Details
                            if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() == "A")
                            {
                                btnAddress.Visible = true;
                                if (_isNewApplicant == "R")
                                    pnlDSSConfirm.Visible = true;

                                #region House Number & Street
                                //string strValue = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["Address_Line_1__c"].ToString();
                                //string HouseNumber = "";
                                //string Street = "";

                                //if (strValue != "")
                                //{
                                //    int firstSpaceIndex = strValue.IndexOf(" ");
                                //    /*******************************************************/
                                //    string strtempValue1 = "";
                                //    if (firstSpaceIndex > -1)
                                //        strtempValue1 = strValue.Substring(0, firstSpaceIndex);

                                //    int result;
                                //    bool isNumeric = int.TryParse(strtempValue1, out result);
                                //    if (isNumeric)
                                //    {
                                //        HouseNumber = strtempValue1;
                                //        Street = strValue.Substring(firstSpaceIndex + 1);
                                //    }
                                //    else
                                //    {
                                //        Street = strValue;
                                //    }

                                //    if (Street.Length > 25)
                                //        Street = Street.Substring(0, 24);

                                //    /*******************************************************/
                                //    //   string strtempValue2 = strValue.Substring(firstSpaceIndex + 1);

                                //}
                                //if (HouseNumber.ToString().Trim().ToUpper() != propMainCasemstEntity.Hn.ToString().Trim().ToUpper())
                                //{
                                //    intRowIndex = gvwSubdetails.Rows.Add("House No", HouseNumber.ToString().Trim(), propMainCasemstEntity.Hn.ToString(), _drDSSXMLitem["HOUSENOmode"].ToString() == "TRUE" ? true : false,
                                //        HouseNumber.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //}
                                //if (Street.ToString().Trim().ToUpper() != propMainCasemstEntity.Street.ToString().Trim().ToUpper())
                                //{
                                //    intRowIndex = gvwSubdetails.Rows.Add("Street", Street.ToString().Trim(), propMainCasemstEntity.Street.ToString(), _drDSSXMLitem["STREETmode"].ToString() == "TRUE" ? true : false,
                                //        Street.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //}

                                #endregion

                                #region City, State and Zip codes

                                //#region ZIP & ZIP Code
                                //string _ZIP = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["AddressZip_Code__c"].ToString();
                                //decimal MST_ZIP = 0;
                                //decimal MST_ZIP_PLUS = 0;
                                //if (_ZIP.ToString() != string.Empty)
                                //{
                                //    string[] strArgs = null;

                                //    if (_ZIP.Contains("-"))
                                //        strArgs = _ZIP.Split('-');
                                //    else if (_ZIP.Contains(" "))
                                //        strArgs = _ZIP.Split(' ');
                                //    else
                                //        strArgs = _ZIP.Split('-');

                                //    string Zipcode = "";
                                //    string ZipPlus = "0";
                                //    Zipcode = strArgs[0].ToString();
                                //    if (strArgs.Length > 1)
                                //        ZipPlus = strArgs[1].ToString();

                                //    if (Zipcode.Length > 5)
                                //    {
                                //        Zipcode = Zipcode.Substring(0, 5);
                                //        if (Zipcode.Length == 9)
                                //            ZipPlus = Zipcode.Substring(5, 4);
                                //    }

                                //    MST_ZIP = Convert.ToDecimal(Zipcode);
                                //    MST_ZIP_PLUS = Convert.ToDecimal(ZipPlus);




                                //    if (MST_ZIP.ToString().Trim().ToUpper() != propMainCasemstEntity.Zip.ToString().Trim().ToUpper())
                                //    {
                                //        intRowIndex = gvwSubdetails.Rows.Add("Zip", MST_ZIP.ToString().Trim().PadLeft(5, '0'), propMainCasemstEntity.Zip.ToString().PadLeft(5, '0'), _drDSSXMLitem["ZIPmode"].ToString() == "TRUE" ? true : false,
                                //            MST_ZIP.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    }

                                //    if (MST_ZIP_PLUS.ToString().Trim().ToUpper() != propMainCasemstEntity.Zipplus.ToString().Trim().ToUpper())
                                //    {
                                //        intRowIndex = gvwSubdetails.Rows.Add("Zip Plus", MST_ZIP_PLUS.ToString().Trim().PadLeft(4, '0'), propMainCasemstEntity.Zipplus.ToString().PadLeft(4, '0'), _drDSSXMLitem["ZIPCODEmode"].ToString() == "TRUE" ? true : false,
                                //            MST_ZIP_PLUS.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    }
                                //}



                                //#endregion

                                //#region State
                                //string State = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["State__c"].ToString();
                                //if (State.ToString().Trim().ToUpper() != propMainCasemstEntity.State.ToString().Trim().ToUpper())
                                //{
                                //    intRowIndex = gvwSubdetails.Rows.Add("State", State.ToString().Trim(), propMainCasemstEntity.State.ToString(), _drDSSXMLitem["STATEmode"].ToString() == "TRUE" ? true : false,
                                //        State.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //}


                                //#endregion

                                //#region City
                                //string City = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["City__c"].ToString();
                                //if (City.ToString().Trim().ToUpper() != propMainCasemstEntity.City.ToString().Trim().ToUpper())
                                //{
                                //    intRowIndex = gvwSubdetails.Rows.Add("City", City.ToString().Trim(), propMainCasemstEntity.City.ToString(), _drDSSXMLitem["CITYmode"].ToString() == "TRUE" ? true : false,
                                //        City.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //}
                                //#endregion

                                #endregion

                                #region Email
                                string Email = _drDSSXMLitem["Email__c"].ToString();
                                if (Email.ToString().Trim().ToUpper() != propMainCasemstEntity.Email.ToString().Trim().ToUpper())
                                {
                                    intRowIndex = gvwSubdetails.Rows.Add("Email", Email.ToString().Trim(), propMainCasemstEntity.Email.ToString(), _drDSSXMLitem["Emailmode"].ToString() == "TRUE" ? true : false,
                                        Email.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                }
                                #endregion

                                #region Phone & Cell

                                string _phone = _drDSSXMLitem["Primary_Phone__c"].ToString();
                                string MST_area = string.Empty;
                                string MST_phone = string.Empty;
                                if (_phone.ToString() != string.Empty)
                                {
                                    _phone = _phone.Replace("-", "");
                                    if (_drDSSXMLitem["Primary_Phone_Type__c"].ToString() == "Home")
                                    {
                                        string _area = "";
                                        string _phn = "";

                                        if (_phone.Length == 10)
                                        {
                                            _area = _phone.Substring(0, 3);
                                            if (_phone.Length == 10)
                                                _phn = _phone.Substring(3, 7);
                                        }
                                        else
                                        {

                                        }

                                        MST_area = _area;
                                        MST_phone = _phn;

                                        string Phone = MST_area + MST_phone;

                                        if (Phone.ToString().Trim().ToUpper() != (propMainCasemstEntity.Area.ToString().Trim().ToUpper() + propMainCasemstEntity.Phone.ToString().Trim().ToUpper()))
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Phone", LookupDataAccess.GetPhoneSsnNoFormat(Phone.ToString().Trim()), LookupDataAccess.GetPhoneSsnNoFormat(propMainCasemstEntity.Phone.ToString()), _drDSSXMLitem["PHONEmode"].ToString() == "TRUE" ? true : false,
                                                Phone.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }
                                    }
                                    else if (_drDSSXMLitem["Primary_Phone_Type__c"].ToString() == "Cell")
                                    {
                                        //ocaseMST.XML_MST_CELL_PHONE = _phone;

                                        if (_phone.ToString().Trim().ToUpper() != propMainCasemstEntity.CellPhone.ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Cell Phone", LookupDataAccess.GetPhoneSsnNoFormat(_phone.ToString().Trim()), LookupDataAccess.GetPhoneSsnNoFormat(propMainCasemstEntity.CellPhone.ToString()), _drDSSXMLitem["CELLmode"].ToString() == "TRUE" ? true : false,
                                                _phone.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }
                                    }
                                }

                                #endregion

                                #region Primary Langugae

                                stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, "**", "**", "**", string.Empty);

                                //comOldvalue = listAgytabls.Find(u => u.Code == propMainCasemstEntity.Language.ToString().Trim());
                                //if (comOldvalue != null)
                                //{
                                //    stroldvalue = comOldvalue.Desc;
                                //}
                                stroldvalue = TMS00141Form.getLanguageCode(_drDSSXMLitem["Primary_Language__c"].ToString(), BaseForm.BaseAgencyControlDetails.AgyShortName);
                                _DssxmlDesc = _drDSSXMLitem["Primary_Language__c"].ToString().Trim();
                                comNewvalue = listAgytabls.Find(u => u.Code == propMainCasemstEntity.Language.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Code;
                                    _CAPDesc = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (_drDSSXMLitem["Primary_Language__c"].ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (stroldvalue.ToLower() != strNewvalue.ToLower())
                                {
                                    //intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Language", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["LANGUAGEmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }

                                #endregion

                                #region Housing Situation
                                stroldvalue = strNewvalue = strFieldType = _DssxmlDesc = _CAPDesc = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, "**", "**", "**", string.Empty);

                                //comOldvalue = listAgytabls.Find(u => u.Code == propMainCasemstEntity.Housing.ToString().Trim());
                                //if (comOldvalue != null)
                                //{
                                //    stroldvalue = comOldvalue.Desc;
                                //}

                                stroldvalue = TMS00141Form.getHousingCode(_dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim());
                                _DssxmlDesc = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim();
                                comNewvalue = listAgytabls.Find(u => u.Code == propMainCasemstEntity.Housing.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Code;
                                    _CAPDesc = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (_dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (stroldvalue.ToLower() != strNewvalue.ToLower())
                                {
                                    intRowIndex = gvwSubdetails.Rows.Add("Housing", _DssxmlDesc, _CAPDesc, _drDSSXMLitem["HOUSINGmode"].ToString() == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                #endregion

                                #region Rent/Mortgage
                                string HousingSituation = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim();
                                string strXMLMortgageAmt = "0.00";
                                if (HousingSituation.ToLower().Contains("rent"))
                                {
                                    strXMLMortgageAmt = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Mortgage_Portion_Amount__c"].ToString().Trim();
                                }
                                else if (HousingSituation.ToLower().Contains("own"))
                                {
                                    strXMLMortgageAmt = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Monthly_Payment_for_Mortgage__c"].ToString().Trim();

                                }
                                if (strXMLMortgageAmt == "")
                                    strXMLMortgageAmt = "0.00";
                                string CAPMortgaegeAmt = propMainCasemstEntity.ExpRent.ToString().Trim();
                                if (strXMLMortgageAmt.ToString().Trim().ToUpper() != CAPMortgaegeAmt.Trim().ToUpper())
                                {
                                    intRowIndex = gvwSubdetails.Rows.Add("Rent/Mortgage", strXMLMortgageAmt.ToString().Trim(), CAPMortgaegeAmt, _drDSSXMLitem["RENTMORTmode"].ToString() == "TRUE" ? true : false,
                                        strXMLMortgageAmt.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                }
                                #endregion

                                #region Dwelling
                                string XMLDwelling = TMS00141Form.getDwellingTypeCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Type_Of_Home_To_Live__c"].ToString());
                                string XMLDwellingDesc = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Type_Of_Home_To_Live__c"].ToString();

                                string CAPDwelling = propMainCasemstEntity.Dwelling.ToString().Trim();
                                // string CAPDwellingDesc = propMainCasemstEntity.DwellingDesc.ToString().Trim();

                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DWELLINGTYPE, "**", "**", "**", string.Empty);
                                var tempDewllingCode = listAgytabls.Find(u => u.Code == propMainCasemstEntity.Dwelling.ToString().Trim());
                                string CAPDwellingDesc = "";
                                if (tempDewllingCode != null)
                                {
                                    CAPDwellingDesc = tempDewllingCode.Desc;
                                }


                                if (XMLDwelling.ToString().Trim().ToUpper() != CAPDwelling.Trim().ToUpper())
                                {
                                    intRowIndex = gvwSubdetails.Rows.Add("Dwelling Type", XMLDwellingDesc.ToString().Trim(), CAPDwellingDesc, _drDSSXMLitem["DWELLINGMode"].ToString() == "TRUE" ? true : false,
                                        XMLDwelling.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                }
                                #endregion

                                #region Primary Method of Paying for Heat 
                                string XMLPayHeat = TMS00141Form.getHeatIncCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_Method_for_Heat__c"].ToString().Trim());
                                string XMLPayHeatDesc = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_Method_for_Heat__c"].ToString().Trim();

                                string CAPPayHeaat = propMainCasemstEntity.HeatIncRent;
                                string CAPPayHeaatDesc = "";

                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.METHODOFPAYINGFORHEAT, "**", "**", "**", string.Empty);
                                var tempPayHeatCode = listAgytabls.Find(u => u.Code == propMainCasemstEntity.HeatIncRent.ToString().Trim());
                                if (tempPayHeatCode != null)
                                {
                                    CAPPayHeaatDesc = tempPayHeatCode.Desc;
                                }

                                if (XMLPayHeat.ToString().Trim().ToUpper() != CAPPayHeaat.Trim().ToUpper())
                                {
                                    intRowIndex = gvwSubdetails.Rows.Add("Primary Method of Paying for Heat", XMLPayHeatDesc.ToString().Trim(), CAPPayHeaatDesc, _drDSSXMLitem["PRIMPAYHEATMode"].ToString() == "TRUE" ? true : false,
                                        XMLPayHeat.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                }

                                #endregion

                                #region Primary Source of Heat
                                string XMLSource = TMS00141Form.getPrimarySourceCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim());
                                string XMLSourceDesc = _dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim();

                                string CAPSource = propMainCasemstEntity.Source;
                                string CAPSourceDesc = "";

                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEATSOURCE, "**", "**", "**", string.Empty);
                                var tempSourceCode = listAgytabls.Find(u => u.Code == propMainCasemstEntity.Source.ToString().PadLeft(2, '0').Trim());
                                if (tempSourceCode != null)
                                {
                                    CAPSourceDesc = tempSourceCode.Desc;
                                }


                                if (XMLSource.ToString().Trim().ToUpper() != CAPSource.Trim().ToUpper())
                                {
                                    intRowIndex = gvwSubdetails.Rows.Add("Primary Source of Heat", XMLSourceDesc.ToString().Trim(), CAPSourceDesc, _drDSSXMLitem["PRIMSRCHEARMode"].ToString() == "TRUE" ? true : false,
                                        XMLSourceDesc.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                }

                                #endregion

                                #region LandLord Information
                                DataTable dtLLRRec = new DataTable();
                                dtLLRRec = _dsAllDSSXML.Tables["LandlordSection"];
                                if (dtLLRRec.Rows.Count > 0)
                                {
                                    #region Landlord First Name & Last Name
                                    string xmlLLRFirstName = "", xmlLLRLastName = "";
                                    if (dtLLRRec.Columns.Contains("Landlord_Company_Name__c"))
                                    {
                                        if (!string.IsNullOrEmpty(dtLLRRec.Rows[0]["Landlord_Company_Name__c"].ToString().Trim()))
                                        {
                                            string[] LLRNAME = dtLLRRec.Rows[0]["Landlord_Company_Name__c"].ToString().Trim().Split(' ');

                                            xmlLLRFirstName = LLRNAME[0];
                                            if (LLRNAME.Length > 1)
                                                xmlLLRLastName = LLRNAME[1];
                                        }

                                    }
                                    if (xmlLLRFirstName.ToString().Trim().ToUpper() != CapLandlordInfoEntity.IncareFirst.ToString().Trim().ToUpper())
                                    {
                                        intRowIndex = gvwSubdetails.Rows.Add("Landlord First Name", xmlLLRFirstName.ToString().Trim(), CapLandlordInfoEntity.IncareFirst.ToString().Trim(),
                                            _drDSSXMLitem["LLRFnameMode"].ToString() == "TRUE" ? true : false,
                                            xmlLLRFirstName.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                    }
                                    if (xmlLLRLastName.ToString().Trim().ToUpper() != CapLandlordInfoEntity.IncareLast.ToString().Trim().ToUpper())
                                    {
                                        intRowIndex = gvwSubdetails.Rows.Add("Landlord Last Name", xmlLLRLastName.ToString().Trim(), CapLandlordInfoEntity.IncareLast.ToString().Trim(),
                                            _drDSSXMLitem["LLRLnameMode"].ToString() == "TRUE" ? true : false,
                                            xmlLLRLastName.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                    }
                                    #endregion

                                    #region Phone
                                    if (dtLLRRec.Columns.Contains("Landlord_Company_Phone__c"))
                                    {
                                        string xmlLLRPhone = dtLLRRec.Rows[0]["Landlord_Company_Phone__c"].ToString().Trim().Replace("-", "");
                                        if (xmlLLRPhone.ToString().Trim().ToUpper() != CapLandlordInfoEntity.Phone.ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Landlord Phone", xmlLLRPhone.ToString().Trim(), CapLandlordInfoEntity.Phone.ToString().Trim(),
                                                _drDSSXMLitem["LLRPhoneMode"].ToString() == "TRUE" ? true : false,
                                                xmlLLRPhone.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }

                                    }



                                    #endregion

                                    #region City
                                    if (dtLLRRec.Columns.Contains("Landlord_City__c"))
                                    {
                                        string xmlLLRCity = dtLLRRec.Rows[0]["Landlord_City__c"].ToString().Trim();
                                        if (xmlLLRCity.ToString().Trim().ToUpper() != CapLandlordInfoEntity.City.ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Landlord City", xmlLLRCity.ToString().Trim(), CapLandlordInfoEntity.City.ToString().Trim(),
                                                _drDSSXMLitem["LLRCityMode"].ToString() == "TRUE" ? true : false,
                                                xmlLLRCity.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }
                                    }
                                    #endregion

                                    #region State
                                    if (dtLLRRec.Columns.Contains("Landlord_State__c"))
                                    {
                                        string xmlLLRState = dtLLRRec.Rows[0]["Landlord_State__c"].ToString().Trim();
                                        if (xmlLLRState.ToString().Trim().ToUpper() != CapLandlordInfoEntity.State.ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Landlord State", xmlLLRState.ToString().Trim(), CapLandlordInfoEntity.State.ToString().Trim(),
                                                _drDSSXMLitem["LLRStateMode"].ToString() == "TRUE" ? true : false,
                                                xmlLLRState.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }
                                    }
                                    #endregion


                                    #region Address, House No & Street
                                    if (dtLLRRec.Columns.Contains("Landlord_Company_Address__c"))
                                    {
                                        string strValue1 = dtLLRRec.Rows[0]["Landlord_Company_Address__c"].ToString();
                                        string firstPart1 = "";
                                        string secondPart1 = "";

                                        if (strValue1 != "")
                                        {
                                            int firstSpaceIndex = strValue1.IndexOf(" ");
                                            /*******************************************************/
                                            string strtempValue1 = "";
                                            if (firstSpaceIndex > -1)
                                                strtempValue1 = strValue1.Substring(0, firstSpaceIndex);

                                            int result;
                                            bool isNumeric = int.TryParse(strtempValue1, out result);
                                            if (isNumeric)
                                            {
                                                firstPart1 = strtempValue1;
                                                secondPart1 = strValue1.Substring(firstSpaceIndex + 1);
                                            }
                                            else
                                            {
                                                secondPart1 = strValue1;
                                            }

                                            /*******************************************************/
                                            //   string strtempValue2 = strValue.Substring(firstSpaceIndex + 1);

                                        }

                                        //ocaseMST.Street = secondPart;
                                        //ocaseMST.Hn = firstPart; //split street and add HN

                                        string xmlStreet = secondPart1;
                                        string xmlHNo = firstPart1;    //split street and add HN

                                        if (xmlStreet.ToString().Trim().ToUpper() != CapLandlordInfoEntity.Street.ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Landlord Street", xmlStreet.ToString().Trim(), CapLandlordInfoEntity.Street.ToString().Trim(),
                                                _drDSSXMLitem["LLRStreetMode"].ToString() == "TRUE" ? true : false,
                                                xmlStreet.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }

                                        if (xmlHNo.ToString().Trim().ToUpper() != CapLandlordInfoEntity.Hn.ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Landlord HN", xmlHNo.ToString().Trim(), CapLandlordInfoEntity.Hn.ToString().Trim(),
                                                _drDSSXMLitem["LLRHouseNoMode"].ToString() == "TRUE" ? true : false,
                                                xmlHNo.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }
                                    }
                                    #endregion

                                    #region Zip

                                    if (dtLLRRec.Columns.Contains("Landlord_AddressZip_Code__c"))
                                    {
                                        string _LLRZIP = dtLLRRec.Rows[0]["Landlord_AddressZip_Code__c"].ToString();
                                        decimal LLR_ZIP = 0;
                                        decimal LLR_ZIP_PLUS = 0;
                                        if (_LLRZIP.ToString() != string.Empty)
                                        {
                                            string[] strArgs = null;

                                            if (_LLRZIP.Contains("-"))
                                                strArgs = _LLRZIP.Split('-');
                                            else if (_LLRZIP.Contains(" "))
                                                strArgs = _LLRZIP.Split(' ');
                                            else
                                                strArgs = _LLRZIP.Split('-');

                                            string Zipcode = "";
                                            string ZipPlus = "0";
                                            Zipcode = strArgs[0].ToString();
                                            if (strArgs.Length > 1)
                                                ZipPlus = strArgs[1].ToString();

                                            if (Zipcode.Length > 5)
                                            {
                                                Zipcode = Zipcode.Substring(0, 5);
                                                if (Zipcode.Length == 9)
                                                    ZipPlus = Zipcode.Substring(5, 4);
                                            }

                                            LLR_ZIP = Convert.ToDecimal(Zipcode);
                                            LLR_ZIP_PLUS = Convert.ToDecimal(ZipPlus);
                                        }

                                        int xmlLLR_ZIP = Convert.ToInt32(LLR_ZIP);
                                        int xmlLLR_ZIPPLUS = Convert.ToInt32(LLR_ZIP_PLUS);

                                        if (xmlLLR_ZIP != Convert.ToInt32(CapLandlordInfoEntity.Zip == "" ? "0" : CapLandlordInfoEntity.Zip))
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Landlord Zip", xmlLLR_ZIP.ToString().Trim().PadLeft(5, '0'), CapLandlordInfoEntity.Zip.ToString().Trim().PadLeft(5, '0'),
                                                _drDSSXMLitem["LLRZipMode"].ToString() == "TRUE" ? true : false,
                                                xmlLLR_ZIP.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }

                                        if (xmlLLR_ZIPPLUS != Convert.ToInt32(CapLandlordInfoEntity.ZipPlus == "" ? "0" : CapLandlordInfoEntity.ZipPlus))
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Landlord ZipPlus", xmlLLR_ZIPPLUS.ToString().Trim().PadLeft(4, '0'), CapLandlordInfoEntity.ZipPlus.ToString().Trim().PadLeft(4, '0'),
                                                _drDSSXMLitem["LLRZipCodeMode"].ToString() == "TRUE" ? true : false,
                                                xmlLLR_ZIPPLUS.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                        }
                                    }


                                    #endregion
                                }
                                #endregion

                                #region CASEDIFF / Mailing Address

                                //string isMailingAddress = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Mailing_Address_different_than_Home_Add__c"].ToString().Trim();
                                //if (isMailingAddress.Trim().ToString().ToUpper() == "YES")
                                //{
                                //    #region CASEDIFF Records DATA Building
                                //    //DataTable dtDIFF = new DataTable();
                                //    //dtDIFF = _dsAllDSSXML.Tables["MAILINGDETAILS"];

                                //    //if (dtDIFF.Rows.Count > 0)
                                //    //{


                                //    //    //if (dtDIFF.Columns.Contains("State__c"))
                                //    //    //{
                                //    //    //    string xmlDiffSate = dtDIFF.Rows[0]["State__c"].ToString().Trim();
                                //    //    //    if (xmlDiffSate.ToString().Trim().ToUpper() != CapCaseDiffMailDetails.State.ToString().Trim().ToUpper())
                                //    //    //    {
                                //    //    //        intRowIndex = gvwSubdetails.Rows.Add("Mailing Address State", xmlDiffSate.ToString().Trim(), CapCaseDiffMailDetails.State.ToString().Trim(),
                                //    //    //            _drDSSXMLitem["DiffSateMode"].ToString() == "TRUE" ? true : false,
                                //    //    //            xmlDiffSate.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    //    //    }

                                //    //    //}
                                //    //    //if (dtDIFF.Columns.Contains("City__c"))
                                //    //    //{
                                //    //    //    string xmlDiffCity = dtDIFF.Rows[0]["City__c"].ToString().Trim();
                                //    //    //    if (xmlDiffCity.ToString().Trim().ToUpper() != CapCaseDiffMailDetails.City.ToString().Trim().ToUpper())
                                //    //    //    {
                                //    //    //        intRowIndex = gvwSubdetails.Rows.Add("Mailing Address City", xmlDiffCity.ToString().Trim(), CapCaseDiffMailDetails.City.ToString().Trim(),
                                //    //    //            _drDSSXMLitem["DiffCityMode"].ToString() == "TRUE" ? true : false,
                                //    //    //            xmlDiffCity.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    //    //    }
                                //    //    //}


                                //    //    //if (dtDIFF.Columns.Contains("Address_Line_1__c"))
                                //    //    //{
                                //    //    //    string strValue1 = dtDIFF.Rows[0]["Address_Line_1__c"].ToString();
                                //    //    //    string firstPart1 = "";
                                //    //    //    string secondPart1 = "";

                                //    //    //    if (strValue1 != "")
                                //    //    //    {
                                //    //    //        int firstSpaceIndex = strValue1.IndexOf(" ");
                                //    //    //        /*******************************************************/
                                //    //    //        string strtempValue1 = "";
                                //    //    //        if (firstSpaceIndex > -1)
                                //    //    //            strtempValue1 = strValue1.Substring(0, firstSpaceIndex);

                                //    //    //        int result;
                                //    //    //        bool isNumeric = int.TryParse(strtempValue1, out result);
                                //    //    //        if (isNumeric)
                                //    //    //        {
                                //    //    //            firstPart1 = strtempValue1;
                                //    //    //            secondPart1 = strValue1.Substring(firstSpaceIndex + 1);
                                //    //    //        }
                                //    //    //        else
                                //    //    //        {
                                //    //    //            secondPart1 = strValue1;
                                //    //    //        }

                                //    //    //        /*******************************************************/
                                //    //    //        //   string strtempValue2 = strValue.Substring(firstSpaceIndex + 1);

                                //    //    //    }

                                //    //    //    string xmlStreet = secondPart1;
                                //    //    //    string xmlHNo = firstPart1;    //split street and add HN

                                //    //    //    if (xmlStreet.ToString().Trim().ToUpper() != CapCaseDiffMailDetails.Street.ToString().Trim().ToUpper())
                                //    //    //    {
                                //    //    //        intRowIndex = gvwSubdetails.Rows.Add("Mailing Address Street", xmlStreet.ToString().Trim(), CapCaseDiffMailDetails.Street.ToString().Trim(),
                                //    //    //            _drDSSXMLitem["DiffStreetMode"].ToString() == "TRUE" ? true : false,
                                //    //    //            xmlStreet.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    //    //    }

                                //    //    //    if (xmlHNo.ToString().Trim().ToUpper() != CapCaseDiffMailDetails.Hn.ToString().Trim().ToUpper())
                                //    //    //    {
                                //    //    //        intRowIndex = gvwSubdetails.Rows.Add("Mailing Address HN", xmlHNo.ToString().Trim(), CapCaseDiffMailDetails.Hn.ToString().Trim(),
                                //    //    //            _drDSSXMLitem["DiffHNMode"].ToString() == "TRUE" ? true : false,
                                //    //    //            xmlHNo.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    //    //    }


                                //    //}

                                //    //if (dtDIFF.Columns.Contains("AddressZip_Code__c"))
                                //    //{
                                //    //    string _LLRZIP = dtDIFF.Rows[0]["AddressZip_Code__c"].ToString();
                                //    //    decimal LLR_ZIP = 0;
                                //    //    decimal LLR_ZIP_PLUS = 0;
                                //    //    if (_LLRZIP.ToString() != string.Empty)
                                //    //    {
                                //    //        string[] strArgs = null;

                                //    //        if (_LLRZIP.Contains("-"))
                                //    //            strArgs = _LLRZIP.Split('-');
                                //    //        else if (_LLRZIP.Contains(" "))
                                //    //            strArgs = _LLRZIP.Split(' ');
                                //    //        else
                                //    //            strArgs = _LLRZIP.Split('-');

                                //    //        string Zipcode = "";
                                //    //        string ZipPlus = "0";
                                //    //        Zipcode = strArgs[0].ToString();
                                //    //        if (strArgs.Length > 1)
                                //    //            ZipPlus = strArgs[1].ToString();

                                //    //        if (Zipcode.Length > 5)
                                //    //        {
                                //    //            Zipcode = Zipcode.Substring(0, 5);
                                //    //            if (Zipcode.Length == 9)
                                //    //                ZipPlus = Zipcode.Substring(5, 4);
                                //    //        }

                                //    //        LLR_ZIP = Convert.ToDecimal(Zipcode);
                                //    //        LLR_ZIP_PLUS = Convert.ToDecimal(ZipPlus);
                                //    //    }

                                //    //    int xmlLLR_ZIP = Convert.ToInt32(LLR_ZIP);
                                //    //    int xmlLLR_ZIPPLUS = Convert.ToInt32(LLR_ZIP_PLUS);

                                //    //    if (xmlLLR_ZIP != Convert.ToInt32(CapCaseDiffMailDetails.Zip == "" ? "0" : CapCaseDiffMailDetails.Zip))
                                //    //    {
                                //    //        intRowIndex = gvwSubdetails.Rows.Add("Mailing Address Zip", xmlLLR_ZIP.ToString().Trim().PadLeft(5, '0'), CapCaseDiffMailDetails.Zip.ToString().Trim().PadLeft(5, '0'),
                                //    //            _drDSSXMLitem["DiffZipMode"].ToString() == "TRUE" ? true : false,
                                //    //            xmlLLR_ZIP.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    //    }

                                //    //    if (xmlLLR_ZIPPLUS != Convert.ToInt32(CapCaseDiffMailDetails.ZipPlus == "" ? "0" : CapCaseDiffMailDetails.ZipPlus))
                                //    //    {
                                //    //        intRowIndex = gvwSubdetails.Rows.Add("Mailing Address ZipPlus", xmlLLR_ZIPPLUS.ToString().Trim().PadLeft(4, '0'), CapCaseDiffMailDetails.ZipPlus.ToString().Trim().PadLeft(4, '0'),
                                //    //            _drDSSXMLitem["DiffZipPlusMode"].ToString() == "TRUE" ? true : false,
                                //    //            xmlLLR_ZIPPLUS.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                //    //    }
                                //    //}
                                //    #endregion
                                //}


                                //}

                                #endregion

                                #region LIHEAP Questions
                                //if (CAPLihpmQueslist.Count > 0)
                                //{
                                //    string hasLHQues = "";
                                //    string strXMLQResp1 = string.Empty;
                                //    string strXMLQResp2 = string.Empty;
                                //    string strXMLQResp3 = string.Empty;
                                //    string strXMLQResp4 = string.Empty;
                                //    string strXMLQResp5 = string.Empty;
                                //    string strXMLQResp6 = string.Empty;
                                //    string strXMLQResp7 = string.Empty;
                                //    string strXMLQResp8 = string.Empty;
                                //    string strXMLQResp9 = string.Empty;
                                //    string strXMLQResp10 = string.Empty;
                                //    string strXMLQResp11 = string.Empty;
                                //    string XML_MST_SOURCE = TMS00141Form.getPrimarySourceCode(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim());

                                //    strXMLQResp1 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Home_Owner__c"].ToString());
                                //    strXMLQResp2 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["LivedInCurrentHomeForAtleastAYear__c"].ToString());

                                //    strXMLQResp3 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Used_Same_Heating_Vendor__c"].ToString());
                                //    if (!string.IsNullOrEmpty(strXMLQResp3.Trim()) && string.IsNullOrEmpty(strXMLQResp2.Trim()))
                                //        strXMLQResp2 = TMS00141Form.getS0056codes("YES");
                                //    else if (!string.IsNullOrEmpty(strXMLQResp2.Trim()) && strXMLQResp2.Trim().ToUpper() == "N")
                                //        strXMLQResp3 = TMS00141Form.getS0056codes("Not Applicable");

                                //    if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                                //        strXMLQResp4 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_System_currently_operable__c"].ToString());
                                //    else if (XML_MST_SOURCE.Trim() == "02")
                                //        strXMLQResp4 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_System_currently_operable__c"].ToString());

                                //    if (XML_MST_SOURCE.Trim() != "04")
                                //    {
                                //        strXMLQResp5 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_system_repaired_or_replaced__c"].ToString());
                                //        if (!string.IsNullOrEmpty(strXMLQResp5.Trim()) && string.IsNullOrEmpty(strXMLQResp1.Trim()) && string.IsNullOrEmpty(strXMLQResp4.Trim()))
                                //        {
                                //            strXMLQResp4 = TMS00141Form.getS0056codes("NO");
                                //            strXMLQResp1 = TMS00141Form.getS0056codes("YES");
                                //        }
                                //        else if (strXMLQResp4 == "N" && strXMLQResp1 == "N")
                                //        {
                                //            strXMLQResp5 = TMS00141Form.getS0056codes("Not Applicable");
                                //        }
                                //    }



                                //    if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                                //    {
                                //        strXMLQResp6 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Currently_Less_Than_A_Quarter_Tank_Fuel__c"].ToString());
                                //        if (!string.IsNullOrEmpty(strXMLQResp6.Trim()) && string.IsNullOrEmpty(strXMLQResp4.Trim()))
                                //            strXMLQResp6 = TMS00141Form.getS0056codes("YES");
                                //    }
                                //    else
                                //        strXMLQResp6 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Currently_Disconnected__c"].ToString());

                                //    if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                                //        strXMLQResp7 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Currently_Disconnected__c"].ToString()); //getS0056codes(dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Afford_to_pay_company_to_restore_service__c"].ToString());
                                //    else
                                //    {
                                //        strXMLQResp7 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Afford_to_pay_company_to_restore_service__c"].ToString());

                                //        if (!string.IsNullOrEmpty(strXMLQResp7.Trim()) && string.IsNullOrEmpty(strXMLQResp6.Trim()))
                                //        {
                                //            strXMLQResp6 = TMS00141Form.getS0056codes("YES");
                                //        }
                                //    }

                                //    if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                                //    {
                                //        strXMLQResp8 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Afford_to_pay_company_to_restore_service__c"].ToString());//getS0056codes(dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString());
                                //        if (!string.IsNullOrEmpty(strXMLQResp8.Trim()) && string.IsNullOrEmpty(strXMLQResp7.Trim()))
                                //        {
                                //            strXMLQResp6 = TMS00141Form.getS0056codes("YES");
                                //        }
                                //    }
                                //    else                                                                                                                                                             //if (!string.IsNullOrEmpty(strXMLQResp8.Trim()) && !string.IsNullOrEmpty(ocaseMST.XML_MST_LPM_0007.Trim()))
                                //    {

                                //        strXMLQResp8 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString());                                                                                                                                                        //{


                                //        if (!string.IsNullOrEmpty(strXMLQResp8.Trim()) && string.IsNullOrEmpty(strXMLQResp6.Trim()))
                                //        {
                                //            strXMLQResp6 = TMS00141Form.getS0056codes("NO");
                                //        }
                                //    }

                                //    if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                                //    {
                                //        strXMLQResp9 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString());//Received_Shut_off_Notice__c//getS0056codes(dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString());
                                //        if (!string.IsNullOrEmpty(strXMLQResp9.Trim()) && string.IsNullOrEmpty(strXMLQResp7.Trim()))
                                //        {
                                //            strXMLQResp7 = TMS00141Form.getS0056codes("NO");
                                //            //if (!string.IsNullOrEmpty(strXMLQResp7.Trim()) && string.IsNullOrEmpty(strXMLQResp6.Trim()))
                                //            //{
                                //            //    strXMLQResp6 = getS0056codes("YES");
                                //            //}
                                //        }
                                //    }
                                //    else
                                //    {
                                //        strXMLQResp9 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString());
                                //        if (!string.IsNullOrEmpty(strXMLQResp9.Trim()) && string.IsNullOrEmpty(strXMLQResp8.Trim()))
                                //        {
                                //            strXMLQResp8 = TMS00141Form.getS0056codes("YES");
                                //            strXMLQResp6 = TMS00141Form.getS0056codes("NO");
                                //        }
                                //    }
                                //    if (XML_MST_SOURCE.Trim() != "02" && XML_MST_SOURCE.Trim() != "04")
                                //    {
                                //        strXMLQResp10 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString());
                                //        if (!string.IsNullOrEmpty(strXMLQResp10.Trim()) && string.IsNullOrEmpty(strXMLQResp9.Trim()))
                                //            strXMLQResp9 = TMS00141Form.getS0056codes("YES");
                                //    }
                                //    else
                                //        strXMLQResp10 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Service_Discon_Protected_By_Medical_Prot__c"].ToString());

                                //    strXMLQResp11 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Free_Weatherization_Services__c"].ToString());

                                //    //strXMLQResp5 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_system_repaired_or_replaced__c"].ToString());
                                //    //strXMLQResp6 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Currently_Less_Than_A_Quarter_Tank_Fuel__c"].ToString());
                                //    //strXMLQResp7 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Currently_Disconnected__c"].ToString());
                                //    //strXMLQResp8 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Afford_to_pay_company_to_restore_service__c"].ToString());
                                //    //strXMLQResp9 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString());
                                //    //strXMLQResp10 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString());
                                //    //strXMLQResp11 = TMS00141Form.getS0056codes(_dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Free_Weatherization_Services__c"].ToString());



                                //    for (int Q = 1; Q <= 11; Q++)
                                //    {
                                //        string strLIHQCode = "00" + String.Format("{0:00}", Q);
                                //        List<LIHPMQuesEntity> lstLHQues = CAPLihpmQueslist.Where(x => x.LPMQ_CODE == strLIHQCode).ToList();
                                //        if (lstLHQues.Count > 0)
                                //        {
                                //            string strXMLQResp = ""; string strCAPQResp = "";
                                //            #region Question Resp Filter
                                //            if (strLIHQCode == "0001")
                                //            {
                                //                strXMLQResp = strXMLQResp1;
                                //                strCAPQResp = propMainCasemstEntity.LPM0001;
                                //            }
                                //            else if (strLIHQCode == "0002")
                                //            {
                                //                strXMLQResp = strXMLQResp2;
                                //                strCAPQResp = propMainCasemstEntity.LPM0002;
                                //            }
                                //            else if (strLIHQCode == "0003")
                                //            {
                                //                strXMLQResp = strXMLQResp3;
                                //                strCAPQResp = propMainCasemstEntity.LPM0003;
                                //            }
                                //            else if (strLIHQCode == "0004")
                                //            {
                                //                strXMLQResp = strXMLQResp4;
                                //                strCAPQResp = propMainCasemstEntity.LPM0004;
                                //            }
                                //            else if (strLIHQCode == "0005")
                                //            {
                                //                strXMLQResp = strXMLQResp5;
                                //                strCAPQResp = propMainCasemstEntity.LPM0005;
                                //            }
                                //            else if (strLIHQCode == "0006")
                                //            {
                                //                strXMLQResp = strXMLQResp6;
                                //                strCAPQResp = propMainCasemstEntity.LPM0006;
                                //            }
                                //            else if (strLIHQCode == "0007")
                                //            {
                                //                strXMLQResp = strXMLQResp7;
                                //                strCAPQResp = propMainCasemstEntity.LPM0007;
                                //            }
                                //            else if (strLIHQCode == "0008")
                                //            {
                                //                strXMLQResp = strXMLQResp8;
                                //                strCAPQResp = propMainCasemstEntity.LPM0008;
                                //            }
                                //            else if (strLIHQCode == "0009")
                                //            {
                                //                strXMLQResp = strXMLQResp9;
                                //                strCAPQResp = propMainCasemstEntity.LPM0009;
                                //            }
                                //            else if (strLIHQCode == "0010")
                                //            {
                                //                strXMLQResp = strXMLQResp10;
                                //                strCAPQResp = propMainCasemstEntity.LPM0010;
                                //            }
                                //            else if (strLIHQCode == "0011")
                                //            {
                                //                strXMLQResp = strXMLQResp11;
                                //                strCAPQResp = propMainCasemstEntity.LPM0011;
                                //            }
                                //            #endregion
                                //            string QuesDesc = lstLHQues[0].LPMQ_DESC.ToString();
                                //            string QuesNo = lstLHQues[0].LPMQ_SNO.ToString();

                                //            if (strXMLQResp.ToString().Trim().ToUpper() != strCAPQResp.ToString().Trim().ToUpper())
                                //            {
                                //                hasLHQues = "Y";
                                //                intRowIndex = gvwSubdetails.Rows.Add(QuesNo + ". " + QuesDesc, TMS00141Form.setS0056codes(strXMLQResp.ToString().Trim()), TMS00141Form.setS0056codes(strCAPQResp.ToString()), _drDSSXMLitem["MST_LPM_0001Mode"].ToString() == "TRUE" ? true : false,
                                //                    strXMLQResp.ToString().Trim() == string.Empty ? "" : string.Empty);

                                //                gvwSubdetails.Rows[intRowIndex].Cells[0].Tag = strLIHQCode;
                                //            }
                                //        }
                                //    }

                                //    if (hasLHQues == "Y")
                                //        gvwSubdetails.Columns[0].Width = 400;


                                //}
                                #endregion


                                //Kranthi:: 11/17/2023:: showing Address fields manually
                                #region Address Fields Mapping

                                string strXMLAddressLine1 = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["Address_Line_1__c"].ToString();
                                string strXMLAddressLine2 = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["Address_Line_2__c"].ToString();
                                string strXMLAddress = strXMLAddressLine1 + " " + strXMLAddressLine2;

                                string strCAPHouse = propMainCasemstEntity.Hn.ToString().Trim().ToUpper() == "" ? "" : propMainCasemstEntity.Hn.ToString().Trim().ToUpper();
                                string strCAPStreet = propMainCasemstEntity.Street.ToString().Trim().ToUpper() == "" ? "" : propMainCasemstEntity.Street.ToString().Trim().ToUpper();
                                string strCAPSuffix = propMainCasemstEntity.Suffix.ToString().Trim().ToUpper() == "" ? "" : propMainCasemstEntity.Suffix.ToString().Trim().ToUpper();
                                string strCAPApt = propMainCasemstEntity.Apt.ToString().Trim().ToUpper() == "" ? "" : ", Apt: " + propMainCasemstEntity.Apt.ToString().Trim().ToUpper() + "";
                                string strCAPFloor = propMainCasemstEntity.Flr.ToString().Trim().ToUpper() == "" ? "" : ", Floor: " + propMainCasemstEntity.Flr.ToString().Trim().ToUpper();

                                string strCAPAddress = "" + strCAPHouse + " " + strCAPStreet + " " + strCAPSuffix + strCAPApt + strCAPFloor;

                                intRowIndex = gvwSubdetails.Rows.Add("Home: House#,Street,Suffix,Apt,Floor", strXMLAddress.ToString(), strCAPAddress.ToString(),
                                    false, string.Empty);

                                gvwSubdetails.Rows[intRowIndex].Cells["gvtSelchk"].ReadOnly = true;

                                /*######################################################################################################################################*/
                                // HOME ADDRESS
                                /*######################################################################################################################################*/

                                string strXMLZip = ""; string strXMLZipCode = "";
                                string _strxmlRZIP = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["AddressZip_Code__c"].ToString();

                                if (_strxmlRZIP.ToString() != string.Empty)
                                {
                                    string[] strArgs = null;

                                    if (_strxmlRZIP.Contains("-"))
                                        strArgs = _strxmlRZIP.Split('-');
                                    else if (_strxmlRZIP.Contains(" "))
                                        strArgs = _strxmlRZIP.Split(' ');
                                    else
                                        strArgs = _strxmlRZIP.Split('-');

                                    string Zipcode = "";
                                    string ZipPlus = "0";
                                    Zipcode = strArgs[0].ToString();
                                    if (strArgs.Length > 1)
                                        ZipPlus = strArgs[1].ToString();

                                    if (Zipcode.Length > 5)
                                    {
                                        Zipcode = Zipcode.Substring(0, 5);
                                        if (Zipcode.Length == 9)
                                            ZipPlus = Zipcode.Substring(5, 4);
                                    }

                                    strXMLZip = ((Zipcode == "" || Zipcode == "0") ? "" : Zipcode.ToString().Trim().PadLeft(5, '0'));
                                    strXMLZipCode = ((ZipPlus == "" || ZipPlus == "0") ? "" : "-" + ZipPlus.ToString().Trim().PadLeft(4, '0'));
                                }

                                string strCAPZip = ((propMainCasemstEntity.Zip.ToString().Trim() == "" || propMainCasemstEntity.Zip.ToString().Trim() == "0") ? "" : propMainCasemstEntity.Zip.ToString().PadLeft(5, '0'));
                                string strCAPZipCode = (propMainCasemstEntity.Zipplus.ToString().Trim() == "" || propMainCasemstEntity.Zipplus.ToString().Trim() == "0") ? "" : "-" + propMainCasemstEntity.Zipplus.ToString().PadLeft(4, '0');

                                string strXMLState = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["State__c"].ToString() == "" ? "" : _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["State__c"].ToString() + " ";
                                string strCAPState = propMainCasemstEntity.State.ToString().ToString() == "" ? "" : propMainCasemstEntity.State.ToString().ToString() + " ";

                                string strXMLCity = _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["City__c"].ToString() == "" ? "" : _dsAllDSSXML.Tables["SERVICEDETAILS"].Rows[0]["City__c"].ToString() + ", ";
                                string strCAPCity = propMainCasemstEntity.City.ToString() == "" ? "" : propMainCasemstEntity.City.ToString() + ", ";

                                string strXMLCitystate = strXMLCity + strXMLState + strXMLZip + strXMLZipCode;

                                string strCAPCitystate = strCAPCity + strCAPState + strCAPZip + strCAPZipCode;

                                intRowIndex = gvwSubdetails.Rows.Add("      City Name,State,Zip", strXMLCitystate.ToString(), strCAPCitystate.ToString(),
                                    false, string.Empty);

                                gvwSubdetails.Rows[intRowIndex].Cells["gvtSelchk"].ReadOnly = true;

                                /*######################################################################################################################################*/
                                /*######################################################################################################################################*/
                                #endregion

                                #region Mailing Address Fields

                                /*######################################################################################################################################*/
                                // MAILING ADDRESS
                                /*######################################################################################################################################*/
                                string isMailingAddress = _dsAllDSSXML.Tables["ApplicationSection"].Rows[0]["Mailing_Address_different_than_Home_Add__c"].ToString().Trim();
                                DataTable _RdtDIFF = new DataTable();
                                _RdtDIFF = _dsAllDSSXML.Tables["MAILINGDETAILS"];
                                if (_RdtDIFF.Rows.Count > 0)
                                {
                                    //if (_RdtDIFF.Columns.Contains("Address_Line_1__c"))
                                    //{
                                    //    string strValue1 = _RdtDIFF.Rows[0]["Address_Line_1__c"].ToString();
                                    //}
                                    string strXMLMailAddressLine1 = ""; string strXMLMailAddressLine2 = "";
                                    if (_RdtDIFF.Columns.Contains("Address_Line_1__c"))
                                        strXMLMailAddressLine1 = _RdtDIFF.Rows[0]["Address_Line_1__c"].ToString();
                                    if (_RdtDIFF.Columns.Contains("Address_Line_2__c"))
                                        strXMLMailAddressLine2 = _RdtDIFF.Rows[0]["Address_Line_2__c"].ToString();

                                    string strXMLMailAddress = strXMLMailAddressLine1 + " " + strXMLMailAddressLine2;
                                    if (strXMLMailAddress.ToString().Trim() == "")
                                        strXMLMailAddress = strXMLAddress;

                                    string strCAPMailHouse = CapCaseDiffMailDetails.Hn.ToString().Trim().ToUpper(); string strCAPMailStreet = CapCaseDiffMailDetails.Street.ToString().Trim().ToUpper();
                                    string strCAPMailSuffix = CapCaseDiffMailDetails.Suffix.ToString().Trim().ToUpper();
                                    string strCAPMailApt = CapCaseDiffMailDetails.Apt.ToString().Trim().ToUpper() == "" ? "" : ", Apt: " + CapCaseDiffMailDetails.Apt.ToString().Trim().ToUpper();
                                    string strCAPMailFloor = CapCaseDiffMailDetails.Flr.ToString().Trim().ToUpper() == "" ? "" : ", Floor: " + CapCaseDiffMailDetails.Flr.ToString().Trim().ToUpper();

                                    string strCAPMailAddress = "" + strCAPMailHouse + " " + strCAPMailStreet + " " + strCAPMailSuffix + strCAPMailApt + strCAPMailFloor;


                                    if (isMailingAddress.Trim().ToString().ToUpper() == "NO")
                                        strXMLMailAddress = "";

                                    intRowIndex = gvwSubdetails.Rows.Add("Mail: House#,Street,Suffix,Apt,Floor", strXMLMailAddress.ToString(), strCAPMailAddress.ToString(),
                               false, string.Empty);


                                    string xmlMailCity = ""; string xmlMailSate = "";
                                    if (_RdtDIFF.Columns.Contains("City__c"))
                                        xmlMailCity = _RdtDIFF.Rows[0]["City__c"].ToString().Trim() == "" ? "" : _RdtDIFF.Rows[0]["City__c"].ToString().Trim() + ", ";
                                    if (_RdtDIFF.Columns.Contains("State__c"))
                                        xmlMailSate = _RdtDIFF.Rows[0]["State__c"].ToString().Trim() == "" ? "" : _RdtDIFF.Rows[0]["State__c"].ToString().Trim() + " ";

                                    string strXMLMailZip = ""; string strXMLMailZipCode = "";
                                    if (_RdtDIFF.Columns.Contains("AddressZip_Code__c"))
                                    {
                                        string _LLRZIP = _RdtDIFF.Rows[0]["AddressZip_Code__c"].ToString();
                                        //decimal LLR_ZIP = 0;
                                        //decimal LLR_ZIP_PLUS = 0;
                                        if (_LLRZIP.ToString() != string.Empty)
                                        {
                                            string[] strArgs = null;

                                            if (_LLRZIP.Contains("-"))
                                                strArgs = _LLRZIP.Split('-');
                                            else if (_LLRZIP.Contains(" "))
                                                strArgs = _LLRZIP.Split(' ');
                                            else
                                                strArgs = _LLRZIP.Split('-');

                                            string Zipcode = "";
                                            string ZipPlus = "0";
                                            Zipcode = strArgs[0].ToString();
                                            if (strArgs.Length > 1)
                                                ZipPlus = strArgs[1].ToString();

                                            if (Zipcode.Length > 5)
                                            {
                                                Zipcode = Zipcode.Substring(0, 5);
                                                if (Zipcode.Length == 9)
                                                    ZipPlus = Zipcode.Substring(5, 4);
                                            }

                                            strXMLMailZip = ((Zipcode == "" || Zipcode == "0") ? "" : Zipcode.ToString().Trim().PadLeft(5, '0'));
                                            strXMLMailZipCode = ((ZipPlus == "" || ZipPlus == "0") ? "" : "-" + ZipPlus.ToString().Trim().PadLeft(4, '0'));
                                        }
                                    }


                                    string strCAPMailCity = CapCaseDiffMailDetails.City.ToString().Trim().ToUpper() == "" ? "" : CapCaseDiffMailDetails.City.ToString().Trim().ToUpper() + ", ";
                                    string strCAPMailState = CapCaseDiffMailDetails.State.ToString().Trim().ToUpper() == "" ? "" : CapCaseDiffMailDetails.State.ToString().Trim().ToUpper() + " ";
                                    string strCAPMailZip = (CapCaseDiffMailDetails.Zip.ToString().Trim() == "" || CapCaseDiffMailDetails.Zip.ToString().Trim() == "0") ? "" : CapCaseDiffMailDetails.Zip.ToString().PadLeft(5, '0');
                                    string strCAPMailZipCode = (CapCaseDiffMailDetails.ZipPlus.ToString().Trim() == "" || CapCaseDiffMailDetails.ZipPlus.ToString().Trim() == "0") ? "" : "-" + CapCaseDiffMailDetails.ZipPlus.ToString().PadLeft(4, '0');


                                    string strXMLMailCitystate = xmlMailCity + xmlMailSate + strXMLMailZip + strXMLMailZipCode;
                                    if (strXMLMailCitystate.ToString().Trim() == "")
                                        strXMLMailCitystate = strXMLCitystate;

                                    string strCAPMailCitystate = strCAPMailCity + strCAPMailState + strCAPMailZip + strCAPMailZipCode;
                                    if (isMailingAddress.Trim().ToString().ToUpper() == "NO")
                                        strXMLMailCitystate = "";


                                    intRowIndex = gvwSubdetails.Rows.Add("      City Name,State,Zip", strXMLMailCitystate.ToString(), strCAPMailCitystate.ToString(),
                                   false, string.Empty);

                                    gvwSubdetails.Rows[intRowIndex].Cells["gvtSelchk"].ReadOnly = true;
                                }

                                #endregion

                            }
                            #endregion



                        }

                        if ((gvwSubdetails.Rows.Count == 0 && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N") || gvwCustomer.SelectedRows[0].Cells["gvchkCrs"].Value.ToString().Trim() == "Y")
                        {
                            _hasVerifiedFlag = "Y";
                            rbtnDetailAddressTab.Checked = true;
                            rbtnSupplierTab.Checked = true;
                            rbtnPerfMesureTab.Checked = true;


                            btnLeanData.Visible = false;
                            gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].ReadOnly = true;
                            gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value = false;
                            chkSelectAll.Checked = false;

                            if (gvwCustomer.SelectedRows[0].Cells["gvadddetsDate"].Value != null)
                            {
                                if (gvwCustomer.SelectedRows[0].Cells["gvadddetsDate"].Value.ToString() != "" && gvwCustomer.SelectedRows[0].Cells["gvadddetsUser"].Value.ToString() != "")
                                {
                                    _AddressDetsTABFlag = true;
                                    string toolTipText = "Verified By     : " + gvwCustomer.SelectedRows[0].Cells["gvadddetsUser"].Value.ToString() + " on " + gvwCustomer.SelectedRows[0].Cells["gvadddetsDate"].Value.ToString();
                                    rbtnDetailAddressTab.ToolTipText = toolTipText;
                                }
                            }

                            if (gvwCustomer.SelectedRows[0].Cells["gvSupplierDate"].Value != null)
                            {
                                if (gvwCustomer.SelectedRows[0].Cells["gvSupplierDate"].Value.ToString() != "" && gvwCustomer.SelectedRows[0].Cells["gvSupplierUser"].Value.ToString() != "")
                                {
                                    _SupplierTABFlag = true;
                                    string toolTipText = "Verified By     : " + gvwCustomer.SelectedRows[0].Cells["gvSupplierUser"].Value.ToString() + " on " + gvwCustomer.SelectedRows[0].Cells["gvSupplierDate"].Value.ToString();
                                    rbtnSupplierTab.ToolTipText = toolTipText;
                                }
                            }

                            if (gvwCustomer.SelectedRows[0].Cells["gvPMDate"].Value != null)
                            {
                                if (gvwCustomer.SelectedRows[0].Cells["gvPMDate"].Value.ToString() != "" && gvwCustomer.SelectedRows[0].Cells["gvPMUser"].Value.ToString() != "")
                                {
                                    _PerfMeasureTABFlag = true;
                                    string toolTipText = "Verified By     : " + gvwCustomer.SelectedRows[0].Cells["gvPMUser"].Value.ToString() + " on " + gvwCustomer.SelectedRows[0].Cells["gvPMDate"].Value.ToString();
                                    rbtnPerfMesureTab.ToolTipText = toolTipText;
                                }
                            }

                            rbtnDetailAddressTab.Checked = _AddressDetsTABFlag;
                            rbtnPerfMesureTab.Checked = _PerfMeasureTABFlag;
                            rbtnSupplierTab.Checked = _SupplierTABFlag;

                            //rbtnDetailAddressTab.Enabled = true; rbtnDetailAddressTab.ReadOnly = true;
                            //rbtnPerfMesureTab.Checked = _PerfMeasureTABFlag;
                            //rbtnSupplierTab.Checked = _SupplierTABFlag;
                        }
                        else
                        {
                            if (_isNewApplicant != "R")
                            {
                                btnLeanData.Visible = false;
                                _hasVerifiedFlag = "Y";
                                rbtnDetailAddressTab.Checked = true;
                                rbtnSupplierTab.Checked = true;
                                rbtnPerfMesureTab.Checked = true;

                                gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].ReadOnly = true;
                                gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value = false;
                                chkSelectAll.Checked = false;
                            }
                            else
                            {
                                _hasVerifiedFlag = "";
                                rbtnDetailAddressTab.Checked = false;
                                rbtnSupplierTab.Checked = false;
                                rbtnPerfMesureTab.Checked = false;

                                btnLeanData.Visible = true;
                                gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].ReadOnly = false;

                                if (gvwCustomer.SelectedRows[0].Cells["gvadddetsDate"].Value != null)
                                {
                                    if (gvwCustomer.SelectedRows[0].Cells["gvadddetsDate"].Value.ToString() != "" && gvwCustomer.SelectedRows[0].Cells["gvadddetsUser"].Value.ToString() != "")
                                    {
                                        _AddressDetsTABFlag = true;
                                        string toolTipText = "Verified By     : " + gvwCustomer.SelectedRows[0].Cells["gvadddetsUser"].Value.ToString() + " on " + gvwCustomer.SelectedRows[0].Cells["gvadddetsDate"].Value.ToString();
                                        rbtnDetailAddressTab.ToolTipText = toolTipText;
                                    }
                                }

                                if (gvwCustomer.SelectedRows[0].Cells["gvSupplierDate"].Value != null)
                                {
                                    if (gvwCustomer.SelectedRows[0].Cells["gvSupplierDate"].Value.ToString() != "" && gvwCustomer.SelectedRows[0].Cells["gvSupplierUser"].Value.ToString() != "")
                                    {
                                        _SupplierTABFlag = true;
                                        string toolTipText = "Verified By     : " + gvwCustomer.SelectedRows[0].Cells["gvSupplierUser"].Value.ToString() + " on " + gvwCustomer.SelectedRows[0].Cells["gvSupplierDate"].Value.ToString();
                                        rbtnSupplierTab.ToolTipText = toolTipText;
                                    }
                                }

                                if (gvwCustomer.SelectedRows[0].Cells["gvPMDate"].Value != null)
                                {
                                    if (gvwCustomer.SelectedRows[0].Cells["gvPMDate"].Value.ToString() != "" && gvwCustomer.SelectedRows[0].Cells["gvPMUser"].Value.ToString() != "")
                                    {
                                        _PerfMeasureTABFlag = true;
                                        string toolTipText = "Verified By     : " + gvwCustomer.SelectedRows[0].Cells["gvPMUser"].Value.ToString() + " on " + gvwCustomer.SelectedRows[0].Cells["gvPMDate"].Value.ToString();
                                        rbtnPerfMesureTab.ToolTipText = toolTipText;
                                    }
                                }

                                rbtnDetailAddressTab.Checked = _AddressDetsTABFlag;
                                rbtnPerfMesureTab.Checked = _PerfMeasureTABFlag;
                                rbtnSupplierTab.Checked = _SupplierTABFlag;
                            }

                        }

                       
                    }
                    else
                    {
                        btnIncomeTypes.Visible = btnNoncashbenefits.Visible = btnViewAllHealthcodes.Visible = false;
                        if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString() == "I")
                        {

                            lblMessage2.Text = "This member is not in the DSS Intake household. CHECK the box to DELETE.";// from the [" + propRecentShortName + "] intake";
                            lblMessage2.ForeColor = Color.Red;
                            lblUpdateMessage.Visible = chkSelectAll.Visible = false;
                        }
                        else
                        {

                            lblMessage2.Text = "This member is not in the [" + propCurentShortName + "] household. CHECK the box to ADD it ";// from the [" + propRecentShortName + "] intake";
                            lblMessage2.ForeColor = Color.Red;
                            lblUpdateMessage.Visible = chkSelectAll.Visible = false;

                        }

                        if (gvwCustomer.SelectedRows[0].Cells["gvtAppType"].Value != null)
                        {
                            if (gvwCustomer.SelectedRows[0].Cells["gvtAppType"].Value.ToString() != string.Empty)
                            {
                                btnCombine.Visible = true;
                                lblCombineMsg.Visible = true;
                                btnCombine.Text = "combine";

                                CaseSnpEntity Basesnpdata = null;
                                if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString() == "I")
                                {
                                    Basesnpdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == _drDSSXMLitem["First_Name__c"].ToString().Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(_drDSSXMLitem["Date_of_Birth__c"].ToString()));
                                    if (Basesnpdata == null)
                                        Basesnpdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == _drDSSXMLitem["First_Name__c"].ToString().Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == _drDSSXMLitem["Last_Name__c"].ToString().Trim().ToUpper());
                                }
                                else
                                {
                                    Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == _drDSSXMLitem["First_Name__c"].ToString().Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(_drDSSXMLitem["Date_of_Birth__c"].ToString()));
                                    if (Basesnpdata == null)
                                        Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == _drDSSXMLitem["First_Name__c"].ToString().Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == _drDSSXMLitem["Last_Name__c"].ToString().Trim().ToUpper());

                                }

                                if (_drDSSXMLitem["First_Name__c"].ToString().Trim().ToUpper() != Basesnpdata.NameixFi.ToString().Trim().ToUpper())
                                {
                                    lblCombineMsg.Text = "There is a possible typeo of the first name. Press button to represent GREEN and RED row was 1 person.";
                                }
                                else if (LookupDataAccess.Getdate(_drDSSXMLitem["Date_of_Birth__c"].ToString().Trim()) != LookupDataAccess.Getdate(Basesnpdata.AltBdate.ToString().Trim()))
                                {
                                    lblCombineMsg.Text = "There is possible wrong date of birth entered both the first and last name are the same.";

                                }
                            }
                        }
                        else
                        {
                            btnCombine.Visible = false;
                            lblCombineMsg.Visible = false;
                            lblCombineMsg.Text = "";

                        }
                    }


                    if (gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value.ToString().Trim().ToUpper() == "TRUE")
                    {

                        chkSelectAll.Enabled = true;
                        List<DataGridViewRow> drRows = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
                        drRows.ForEach(xRow => xRow.Cells["gvtSelchk"].ReadOnly = false);
                    }
                    else
                    {

                        chkSelectAll.Enabled = false;
                        List<DataGridViewRow> drRows = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
                        drRows.ForEach(xRow => xRow.Cells["gvtSelchk"].ReadOnly = true);
                    }

                }
                else
                {
                    if (gvwCustomer.SelectedRows[0].Selected)
                    {
                        string strChkvalue = gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value.ToString().ToUpper() == null ? "FALSE" : gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value.ToString().ToUpper();
                        if (strChkvalue == "TRUE")
                        {
                            chkSelectAll.Checked = true;
                        }
                        else
                        {
                            chkSelectAll.Checked = false;
                        }
                    }
                    if (strFormType != string.Empty)
                    {
                        //CaseSnpEntity snpRecent = gvwCustomer.SelectedRows[0].Tag as CaseSnpEntity;
                        //string stroldvalue = string.Empty; string strNewvalue = string.Empty;

                        LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;
                        string stroldvalue = string.Empty; string strNewvalue = string.Empty; string strFieldType = string.Empty;
                        if (dritem != null)
                        {
                            int intValidmode = 0;
                            if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "I" && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N")
                            {

                                btnIncomeTypes.Visible = btnNoncashbenefits.Visible = btnViewAllHealthcodes.Visible = true;
                                //if (snpRecent != null)
                                //{
                                CaseSnpEntity Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                                if (Basesnpdata == null)
                                    Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                                List<CaseIncomeEntity> _propCurrentIncomelist = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, Basesnpdata.FamilySeq);

                                if (dritem.PIP_SSN.ToString().Trim() != Basesnpdata.Ssno.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("SSN", Basesnpdata.Ssno.ToString(), dritem.PIP_SSN.ToString(), dritem.PIP_SSN_MODE == "TRUE" ? true : false, string.Empty);
                                }

                                if (dritem.PIP_FNAME.ToString().Trim().ToUpper() != Basesnpdata.NameixFi.ToString().Trim().ToUpper())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("First Name", Basesnpdata.NameixFi.ToString(), dritem.PIP_FNAME.ToString(), dritem.PIP_FNAME_MODE == "TRUE" ? true : false, string.Empty);
                                }

                                if (dritem.PIP_MNAME.ToString().Trim().ToUpper() != Basesnpdata.NameixMi.ToString().Trim().ToUpper())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("MI", Basesnpdata.NameixMi.ToString(), dritem.PIP_MNAME.ToString(), dritem.PIP_MNAME_MODE == "TRUE" ? true : false, dritem.PIP_MNAME.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                }

                                if (dritem.PIP_LNAME.ToString().Trim().ToUpper() != Basesnpdata.NameixLast.ToString().Trim().ToUpper())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Last Name", Basesnpdata.NameixLast.ToString(), dritem.PIP_LNAME.ToString(), dritem.PIP_LNAME_MODE == "TRUE" ? true : false, string.Empty);
                                }

                                if (LookupDataAccess.Getdate(dritem.PIP_DOB.ToString().Trim().ToUpper()) != LookupDataAccess.Getdate(Basesnpdata.AltBdate.ToString().Trim().ToUpper()))
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Dob", LookupDataAccess.Getdate(Basesnpdata.AltBdate.ToString()), LookupDataAccess.Getdate(dritem.PIP_DOB.ToString()), dritem.PIP_DOB_MODE == "TRUE" ? true : false, string.Empty);
                                    //gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                }
                                if (dritem.PIP_STATUS.ToString().Trim().ToUpper() != Basesnpdata.Status.ToString().Trim().ToUpper())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Active", (Basesnpdata.Status == "A" ? "Active" : "Inactive"), (dritem.PIP_STATUS == "A" ? "Active" : "Inactive"), dritem.PIP_STATUS_MODE == "TRUE" ? true : false, string.Empty);
                                }


                                //if (dritem.PIP_GENDER.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = string.Empty;
                                List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                                CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Sex.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_GENDER.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_GENDER.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_GENDER.ToString().Trim() != Basesnpdata.Sex.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Gender", stroldvalue, strNewvalue, dritem.PIP_GENDER_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }

                                //if (dritem.PIP_PREGNANT.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.PREGNANT, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Pregnant.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_PREGNANT.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_PREGNANT.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }
                                if (dritem.PIP_PREGNANT.ToString().Trim() != Basesnpdata.Pregnant.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Are You Pregnant?", stroldvalue, strNewvalue, dritem.PIP_PREGNANT_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}

                                // }
                                //if (dritem.PIP_MARITAL_STATUS.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MARITALSTATUS, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.MaritalStatus.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_MARITAL_STATUS.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_MARITAL_STATUS.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_MARITAL_STATUS.ToString().Trim() != Basesnpdata.MaritalStatus.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Marital Status", stroldvalue, strNewvalue, dritem.PIP_MARITAL_STATUS_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                // }
                                //if (dritem.PIP_RELATIONSHIP.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.MemberCode.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RELATIONSHIP.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_RELATIONSHIP.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_RELATIONSHIP.ToString().Trim() != Basesnpdata.MemberCode.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Relation", stroldvalue, strNewvalue, dritem.PIP_RELATIONSHIP_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}
                                //if (dritem.PIP_ETHNIC.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Ethnic.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_ETHNIC.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_ETHNIC.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_ETHNIC.ToString().Trim() != Basesnpdata.Ethnic.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Ethnicity", stroldvalue, strNewvalue, dritem.PIP_ETHNIC_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}
                                //if (dritem.PIP_RACE.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Race.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RACE.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_RACE.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_RACE.ToString().Trim() != Basesnpdata.Race.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Race", stroldvalue, strNewvalue, dritem.PIP_RACE_MODE == "TRUE" ? true : false, strFieldType);

                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}
                                //if (dritem.PIP_EDUCATION.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Education.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_EDUCATION.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_EDUCATION.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_EDUCATION.ToString().Trim() != Basesnpdata.Education.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Education", stroldvalue, strNewvalue, dritem.PIP_EDUCATION_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }

                                }
                                //}
                                //if (dritem.PIP_SCHOOL.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SCHOOLDISTRICTS, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.SchoolDistrict.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_SCHOOL.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_SCHOOL.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_SCHOOL.ToString().Trim() != Basesnpdata.SchoolDistrict.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("School", stroldvalue, strNewvalue, dritem.PIP_SCHOOL_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }

                                }

                                // }
                                //if (dritem.PIP_RELITRAN.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELIABLETRANSPORTATION, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Relitran.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RELITRAN.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_RELITRAN.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_RELITRAN.ToString().Trim() != Basesnpdata.Relitran.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Reliable Transport", stroldvalue, strNewvalue, dritem.PIP_RELITRAN_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }

                                }
                                //}
                                //if (dritem.PIP_DRVLIC.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DRIVERLICENSE, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Drvlic.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_DRVLIC.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_DRVLIC.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_DRVLIC.ToString().Trim() != Basesnpdata.Drvlic.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Drivers License", stroldvalue, strNewvalue, dritem.PIP_DRVLIC_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }

                                }
                                //}

                                //if (dritem.PIP_HEALTH_INS.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEALTHINSURANCE, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.HealthIns.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_HEALTH_INS.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_HEALTH_INS.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_HEALTH_INS.ToString().Trim() != Basesnpdata.HealthIns.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Health Insurance", stroldvalue, strNewvalue, dritem.PIP_HEALTH_INS_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }

                                }
                                //}

                                //if (dritem.PIP_HEALTH_CODES.ToString().Trim() != string.Empty)
                                //{


                                if (dritem.PIP_HEALTH_CODES.ToString().Trim().ToUpper() != string.Empty)
                                {

                                    stroldvalue = strNewvalue = string.Empty;
                                    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HealthCodes, "**", "**", "**", string.Empty);

                                    string[] strPipHealthcodes = dritem.PIP_HEALTH_CODES.ToString().Trim().Split(',');
                                    string[] strHealthcode = Basesnpdata.Health_Codes.ToString().Trim().Split(',');
                                    bool booladdHealth = true;
                                    int inthealthcode = 0;
                                    string strNoncashmode = string.Empty;
                                    foreach (string strHealthtem in strPipHealthcodes)
                                    {

                                        if (strHealthtem.ToString() != string.Empty)
                                        {
                                            booladdHealth = true;
                                            foreach (string item in strHealthcode)
                                            {

                                                if (strHealthtem.Trim() == item.Trim())
                                                {
                                                    booladdHealth = false;
                                                    ListItem lstitem = dritem.Pip_Healthcodes_list_Mode.Find(u => u.Text.ToString().Trim() == item.ToString().Trim());
                                                    dritem.Pip_Healthcodes_list_Mode.Remove(lstitem);
                                                    break;
                                                }
                                            }
                                            if (booladdHealth)
                                            {


                                                foreach (CommonEntity item in listAgytabls)
                                                {

                                                    if (strHealthtem.Trim() == (item.Code.ToString().Trim()))
                                                    {

                                                        if (dritem.Pip_Healthcodes_list_Mode.Count > 0)
                                                        {
                                                            strNoncashmode = dritem.Pip_Healthcodes_list_Mode[inthealthcode].Value.ToString();
                                                        }
                                                        inthealthcode = inthealthcode + 1;
                                                        intRowIndex = gvwSubdetails.Rows.Add("Health Ins " + inthealthcode, string.Empty, item.Desc.ToString(), strNoncashmode == "TRUE" ? true : false, string.Empty);
                                                    }
                                                }
                                            }
                                        }


                                    }

                                    //stroldvalue = strNewvalue = strFieldType = string.Empty;
                                    //listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HealthCodes, "**", "**", "**", string.Empty);

                                    //string[] strNCashben = Basesnpdata.Health_Codes.ToString().Trim().Split(',');

                                    //foreach (var ncashbenitem in strNCashben)
                                    //{

                                    //    comOldvalue = listAgytabls.Find(u => u.Code == ncashbenitem);
                                    //    if (comOldvalue != null)
                                    //    {
                                    //        stroldvalue = stroldvalue + comOldvalue.Desc + ", ";
                                    //    }
                                    //}
                                    //string[] strLeanNCashben = dritem.PIP_HEALTH_CODES.ToString().Trim().Split(',');
                                    //if (dritem.PIP_HEALTH_CODES.ToString().Trim() == string.Empty)
                                    //    strFieldType = "0";
                                    //foreach (var ncashbenitem in strLeanNCashben)
                                    //{
                                    //    comNewvalue = listAgytabls.Find(u => u.Code == ncashbenitem.Trim());
                                    //    if (comNewvalue != null)
                                    //    {
                                    //        strNewvalue = strNewvalue + comNewvalue.Desc + ",";
                                    //    }
                                    //}
                                    //intValidmode = intValidmode + 1;
                                    //intRowIndex = gvwSubdetails.Rows.Add("Health codes", stroldvalue, strNewvalue, dritem.PIP_HEALTH_CODES_MODE == "TRUE" ? true : false, strFieldType);
                                }
                                //}

                                //if (dritem.PIP_MILITARY_STATUS.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MilitaryStatus, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.MilitaryStatus.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_MILITARY_STATUS.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_MILITARY_STATUS.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_MILITARY_STATUS.ToString().Trim() != Basesnpdata.MilitaryStatus.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Military Status", stroldvalue, strNewvalue, dritem.PIP_MILITARY_STATUS_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }

                                }
                                //}


                                //if (dritem.PIP_VETERAN.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.VETERAN, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Vet.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_VETERAN.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_VETERAN.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }
                                if (dritem.PIP_VETERAN.ToString().Trim() != Basesnpdata.Vet.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Veteran", stroldvalue, strNewvalue, dritem.PIP_VETERAN_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}
                                //if (dritem.PIP_FOOD_STAMP.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FOODSTAMPS, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.FootStamps.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_FOOD_STAMP.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_FOOD_STAMP.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_FOOD_STAMP.ToString().Trim() != Basesnpdata.FootStamps.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Food Stamp", stroldvalue, strNewvalue, dritem.PIP_FOOD_STAMP_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}

                                //if (dritem.PIP_DISABLE.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Disable.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_DISABLE.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_DISABLE.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_DISABLE.ToString().Trim() != Basesnpdata.Disable.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Disable", stroldvalue, strNewvalue, dritem.PIP_DISABLE_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}

                                //if (dritem.PIP_FARMER.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FARMER, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Farmer.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_FARMER.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_FARMER.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_FARMER.ToString().Trim() != Basesnpdata.Farmer.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Farmer", stroldvalue, strNewvalue, dritem.PIP_FARMER_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}
                                //if (dritem.PIP_WORK_STAT.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.WorkStatus.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_WORK_STAT.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_WORK_STAT.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_WORK_STAT.ToString().Trim() != Basesnpdata.WorkStatus.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("Work Status", stroldvalue, strNewvalue, dritem.PIP_WORK_STAT_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }
                                //}

                                //if (dritem.PIP_WIC.ToString().Trim() != string.Empty)
                                //{
                                stroldvalue = strNewvalue = strFieldType = string.Empty;
                                listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WIC, "**", "**", "**", string.Empty);

                                comOldvalue = listAgytabls.Find(u => u.Code == Basesnpdata.Wic.ToString().Trim());
                                if (comOldvalue != null)
                                {
                                    stroldvalue = comOldvalue.Desc;
                                }
                                comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_WIC.ToString().Trim());
                                if (comNewvalue != null)
                                {
                                    strNewvalue = comNewvalue.Desc;
                                    if (comNewvalue.Active.ToUpper() == "N")
                                        strFieldType = "2";
                                }
                                else
                                {
                                    if (dritem.PIP_WIC.ToString() != string.Empty)
                                        strFieldType = "1";
                                    else
                                        strFieldType = "0";
                                }

                                if (dritem.PIP_WIC.ToString().Trim() != Basesnpdata.Wic.ToString().Trim())
                                {
                                    intValidmode = intValidmode + 1;
                                    intRowIndex = gvwSubdetails.Rows.Add("WIC", stroldvalue, strNewvalue, dritem.PIP_WIC_MODE == "TRUE" ? true : false, strFieldType);
                                    if (strFieldType == "2" || strFieldType == "1")
                                    {
                                        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                        if (strFieldType == "1")
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                    }
                                }

                                //}

                                //if (dritem.PIP_NCASHBEN.ToString().Trim() != string.Empty)
                                //{








                                //if (dritem.PIP_NCASHBEN.ToString().Trim().ToUpper() != Basesnpdata.NonCashBenefits.ToString().Trim().ToUpper())
                                //{
                                //    stroldvalue = strNewvalue = strFieldType = string.Empty;
                                //    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, "**", "**", "**", string.Empty);

                                //    string[] strNCashben = Basesnpdata.NonCashBenefits.ToString().Trim().Split(',');

                                //    foreach (var ncashbenitem in strNCashben)
                                //    {

                                //        comOldvalue = listAgytabls.Find(u => u.Code == ncashbenitem);
                                //        if (comOldvalue != null)
                                //        {
                                //            stroldvalue = stroldvalue + comOldvalue.Desc + ", ";
                                //        }
                                //    }
                                //    string[] strLeanNCashben = dritem.PIP_NCASHBEN.ToString().Trim().Split(',');

                                //    if (dritem.PIP_NCASHBEN.ToString().Trim() == string.Empty)
                                //        strFieldType = "0";
                                //    foreach (var ncashbenitem in strLeanNCashben)
                                //    {
                                //        comNewvalue = listAgytabls.Find(u => u.Code == ncashbenitem.Trim());
                                //        if (comNewvalue != null)
                                //        {
                                //            strNewvalue = strNewvalue + comNewvalue.Desc + ",";
                                //        }
                                //    }
                                //    intValidmode = intValidmode + 1;
                                //    intRowIndex = gvwSubdetails.Rows.Add("Non Cash Benefit", stroldvalue.ToUpper(), strNewvalue.ToUpper(), dritem.PIP_NCASHBEN_MODE == "TRUE" ? true : false, strFieldType);
                                //    if (strFieldType == "2" || strFieldType == "1")
                                //    {
                                //        gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                //        if (strFieldType == "1")
                                //            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                //    }
                                //}


                                if (dritem.PIP_NCASHBEN.ToString().Trim() != string.Empty)
                                {

                                    stroldvalue = strNewvalue = string.Empty;
                                    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, "**", "**", "**", string.Empty);

                                    string[] strPipNoncashbenefit = dritem.PIP_NCASHBEN.ToString().Trim().Split(',');
                                    string[] strNCashben = Basesnpdata.NonCashBenefits.ToString().Trim().Split(',');
                                    bool booladdNoncash = true;
                                    int intNoncashbenefit = 0;
                                    string strNoncashmode = string.Empty;
                                    foreach (string strNonbitem in strPipNoncashbenefit)
                                    {

                                        if (strNonbitem.ToString() != string.Empty)
                                        {
                                            booladdNoncash = true;
                                            foreach (string item in strNCashben)
                                            {

                                                if (strNonbitem.Trim() == item.Trim())
                                                {
                                                    booladdNoncash = false;
                                                    ListItem lstitem = dritem.Pip_NonCashbenefit_list_Mode.Find(u => u.Text.ToString().Trim() == item.ToString().Trim());
                                                    dritem.Pip_NonCashbenefit_list_Mode.Remove(lstitem);
                                                    break;
                                                }
                                            }
                                            if (booladdNoncash)
                                            {
                                                //  comOldvalue = listAgytabls.Find(u => u.Code.Trim() == strincome.Trim());

                                                foreach (CommonEntity item in listAgytabls)
                                                {

                                                    if (strNonbitem.Trim() == (item.Code.ToString().Trim()))
                                                    {

                                                        if (dritem.Pip_NonCashbenefit_list_Mode.Count > 0)
                                                        {
                                                            strNoncashmode = dritem.Pip_NonCashbenefit_list_Mode[intNoncashbenefit].Value.ToString();
                                                        }
                                                        intNoncashbenefit = intNoncashbenefit + 1;
                                                        intRowIndex = gvwSubdetails.Rows.Add("Non Cash Benefit " + intNoncashbenefit, string.Empty, item.Desc.ToString(), strNoncashmode == "TRUE" ? true : false, string.Empty);
                                                    }
                                                }
                                            }
                                        }


                                    }
                                }

                                //}


                                if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() == "A")
                                {

                                    if (ValidAddress(dritem))
                                    {
                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("House No ", BaseForm.BaseCaseMstListEntity[0].Hn.ToString(), dritem.PIP_HOUSENO.ToString().Trim(), dritem.PIP_HOUSENO_MODE == "TRUE" ? true : false, dritem.PIP_HOUSENO.ToString().Trim() == string.Empty ? "0" : string.Empty);

                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Direction", BaseForm.BaseCaseMstListEntity[0].Direction.ToString(), dritem.PIP_DIRECTION.ToString().Trim(), dritem.PIP_DIRECTION_MODE == "TRUE" ? true : false, dritem.PIP_DIRECTION.ToString().Trim() == string.Empty ? "0" : string.Empty);

                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Street", BaseForm.BaseCaseMstListEntity[0].Street.ToString(), dritem.PIP_STREET.ToString().Trim(), dritem.PIP_STREET_MODE == "TRUE" ? true : false, dritem.PIP_STREET.ToString().Trim() == string.Empty ? "0" : string.Empty);

                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Suffix", BaseForm.BaseCaseMstListEntity[0].Suffix.ToString(), dritem.PIP_SUFFIX.ToString().Trim(), dritem.PIP_SUFFIX_MODE == "TRUE" ? true : false, dritem.PIP_SUFFIX.ToString().Trim() == string.Empty ? "0" : string.Empty);

                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Apartment", BaseForm.BaseCaseMstListEntity[0].Apt.ToString().Trim(), dritem.PIP_APT.ToString().Trim(), dritem.PIP_APT_MODE == "TRUE" ? true : false, dritem.PIP_APT.ToString().Trim() == string.Empty ? "0" : string.Empty);

                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Floor", BaseForm.BaseCaseMstListEntity[0].Flr.ToString(), dritem.PIP_FLR.ToString().Trim(), dritem.PIP_FLR_MODE == "TRUE" ? true : false, dritem.PIP_FLR.ToString().Trim() == string.Empty ? "0" : string.Empty);


                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("City", BaseForm.BaseCaseMstListEntity[0].City.ToString(), dritem.PIP_CITY.ToString().Trim(), dritem.PIP_CITY_MODE == "TRUE" ? true : false, dritem.PIP_CITY.ToString().Trim() == string.Empty ? "0" : string.Empty);



                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Zip", BaseForm.BaseCaseMstListEntity[0].Zip.ToString(), dritem.PIP_ZIP.ToString().Trim(), dritem.PIP_ZIP_MODE == "TRUE" ? true : false, dritem.PIP_ZIP.ToString().Trim() == string.Empty ? "0" : string.Empty);


                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("State", BaseForm.BaseCaseMstListEntity[0].State.ToString(), dritem.PIP_STATE.ToString().Trim(), dritem.PIP_STATE_MODE == "TRUE" ? true : false, dritem.PIP_STATE.ToString().Trim() == string.Empty ? "0" : string.Empty);

                                        stroldvalue = strNewvalue = strFieldType = string.Empty;
                                        listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, "**", "**", "**", string.Empty);

                                        comOldvalue = listAgytabls.Find(u => u.Code == BaseForm.BaseCaseMstListEntity[0].County.ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_COUNTY.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                            if (comNewvalue.Active.ToUpper() == "N")
                                                strFieldType = "2";
                                        }
                                        else
                                        {
                                            if (dritem.PIP_COUNTY.ToString() != string.Empty)
                                                strFieldType = "1";
                                            else
                                                strFieldType = "0";
                                        }


                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("County", stroldvalue, strNewvalue, dritem.PIP_COUNTY_MODE == "TRUE" ? true : false, strFieldType);
                                        if (strFieldType == "2" || strFieldType == "1")
                                        {
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                            if (strFieldType == "1")
                                                gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                        }


                                        stroldvalue = strNewvalue = strFieldType = string.Empty;
                                        listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, "**", "**", "**", string.Empty);

                                        comOldvalue = listAgytabls.Find(u => u.Code == BaseForm.BaseCaseMstListEntity[0].TownShip.ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_TOWNSHIP.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                            if (comNewvalue.Active.ToUpper() == "N")
                                                strFieldType = "2";
                                        }
                                        else
                                        {
                                            if (dritem.PIP_TOWNSHIP.ToString() != string.Empty)
                                                strFieldType = "1";
                                            else
                                                strFieldType = "0";
                                        }


                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Township", stroldvalue, strNewvalue, dritem.PIP_TOWNSHIP_MODE == "TRUE" ? true : false, strFieldType);
                                        if (strFieldType == "2" || strFieldType == "1")
                                        {
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                            if (strFieldType == "1")
                                                gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                        }


                                        intRowIndex = gvwSubdetails.Rows.Add("Precinct", BaseForm.BaseCaseMstListEntity[0].Precinct.ToString().Trim(), dritem.PIP_Precint.ToString().Trim(), dritem.PIP_PRECINT_MODE == "TRUE" ? true : false, dritem.PIP_Precint.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                    }


                                    //if (dritem.PIP_PRI_LANGUAGE.ToString().Trim() != string.Empty)
                                    //{
                                    stroldvalue = strNewvalue = strFieldType = string.Empty;
                                    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, "**", "**", "**", string.Empty);

                                    comOldvalue = listAgytabls.Find(u => u.Code == BaseForm.BaseCaseMstListEntity[0].Language.ToString().Trim());
                                    if (comOldvalue != null)
                                    {
                                        stroldvalue = comOldvalue.Desc;
                                    }
                                    comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_PRI_LANGUAGE.ToString().Trim());
                                    if (comNewvalue != null)
                                    {
                                        strNewvalue = comNewvalue.Desc;
                                        if (comNewvalue.Active.ToUpper() == "N")
                                            strFieldType = "2";
                                    }
                                    else
                                    {
                                        if (dritem.PIP_PRI_LANGUAGE.ToString() != string.Empty)
                                            strFieldType = "1";
                                        else
                                            strFieldType = "0";
                                    }

                                    if (dritem.PIP_PRI_LANGUAGE.ToString().Trim() != BaseForm.BaseCaseMstListEntity[0].Language.ToString().Trim())
                                    {
                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Language", stroldvalue, strNewvalue, dritem.PIP_PRI_LANGUAGE_MODE == "TRUE" ? true : false, strFieldType);
                                        if (strFieldType == "2" || strFieldType == "1")
                                        {
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                            if (strFieldType == "1")
                                                gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                        }
                                    }
                                    //}
                                    //if (dritem.PIP_FAMILY_TYPE.ToString().Trim() != string.Empty)
                                    //{
                                    stroldvalue = strNewvalue = strFieldType = string.Empty;
                                    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, "**", "**", "**", string.Empty);

                                    comOldvalue = listAgytabls.Find(u => u.Code == BaseForm.BaseCaseMstListEntity[0].FamilyType.ToString().Trim());
                                    if (comOldvalue != null)
                                    {
                                        stroldvalue = comOldvalue.Desc;
                                    }
                                    comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_FAMILY_TYPE.ToString().Trim());
                                    if (comNewvalue != null)
                                    {
                                        strNewvalue = comNewvalue.Desc;
                                        if (comNewvalue.Active.ToUpper() == "N")
                                            strFieldType = "2";
                                    }
                                    else
                                    {
                                        if (dritem.PIP_FAMILY_TYPE.ToString() != string.Empty)
                                            strFieldType = "1";
                                        else
                                            strFieldType = "0";
                                    }

                                    if (dritem.PIP_FAMILY_TYPE.ToString().Trim() != BaseForm.BaseCaseMstListEntity[0].FamilyType.ToString().Trim())
                                    {
                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Family Type", stroldvalue, strNewvalue, dritem.PIP_FAMILY_TYPE_MODE == "TRUE" ? true : false, strFieldType);
                                        if (strFieldType == "2" || strFieldType == "1")
                                        {
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                            if (strFieldType == "1")
                                                gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                        }
                                    }
                                    //}
                                    //if (dritem.PIP_HOME_PHONE.ToString().Trim() != string.Empty)
                                    //{

                                    if (dritem.PIP_AREA.ToString().Trim() + dritem.PIP_HOME_PHONE.ToString().Trim() != (BaseForm.BaseCaseMstListEntity[0].Area.ToString().Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.ToString().Trim()))
                                    {
                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Home Phone", (BaseForm.BaseCaseMstListEntity[0].Area.ToString().Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.ToString().Trim()), dritem.PIP_AREA.ToString().Trim() + dritem.PIP_HOME_PHONE.ToString().Trim(), dritem.PIP_HOME_PHONE_MODE == "TRUE" ? true : false, dritem.PIP_HOME_PHONE.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                    }
                                    //}
                                    //if (dritem.PIP_CELL_NUMBER.ToString().Trim() != string.Empty)
                                    //{
                                    stroldvalue = strNewvalue = strFieldType = string.Empty;

                                    if (dritem.PIP_CELL_NUMBER.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].CellPhone.ToString().Trim().ToUpper())
                                    {
                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Cell Number", BaseForm.BaseCaseMstListEntity[0].CellPhone.ToString().Trim(), dritem.PIP_CELL_NUMBER.ToString().Trim(), dritem.PIP_CELL_NUMBER_MODE == "TRUE" ? true : false, dritem.PIP_CELL_NUMBER.ToString().Trim() == string.Empty ? "0" : string.Empty);
                                    }
                                    //}

                                    //if (dritem.PIP_HOUSING.ToString().Trim() != string.Empty)
                                    //{
                                    stroldvalue = strNewvalue = strFieldType = string.Empty;
                                    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, "**", "**", "**", string.Empty);

                                    comOldvalue = listAgytabls.Find(u => u.Code == BaseForm.BaseCaseMstListEntity[0].Housing.ToString().Trim());
                                    if (comOldvalue != null)
                                    {
                                        stroldvalue = comOldvalue.Desc;
                                    }
                                    comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_HOUSING.ToString().Trim());
                                    if (comNewvalue != null)
                                    {
                                        strNewvalue = comNewvalue.Desc;
                                        if (comNewvalue.Active.ToUpper() == "N")
                                            strFieldType = "2";
                                    }
                                    else
                                    {
                                        if (dritem.PIP_HOUSING.ToString() != string.Empty)
                                            strFieldType = "1";
                                        else
                                            strFieldType = "0";
                                    }

                                    if (dritem.PIP_HOUSING.ToString().Trim() != BaseForm.BaseCaseMstListEntity[0].Housing.ToString().Trim())
                                    {
                                        intValidmode = intValidmode + 1;
                                        intRowIndex = gvwSubdetails.Rows.Add("Housing", stroldvalue, strNewvalue, dritem.PIP_HOUSENO_MODE == "TRUE" ? true : false, strFieldType);
                                        if (strFieldType == "2" || strFieldType == "1")
                                        {
                                            gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Style.ForeColor = Color.Red;
                                            if (strFieldType == "1")
                                                gvwSubdetails.Rows[intRowIndex].Cells["gvtNewValue"].Value = "Invalid Item can not be selected";
                                        }
                                    }
                                    //}                                

                                    if (dritem.PIP_SERVICES != string.Empty)
                                    {
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

                                        string strAgy = "00";
                                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                                            strAgy = BaseForm.BaseAgency;

                                        DataSet dsservice = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE(strType, strAgy, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseCaseMstListEntity[0].ApplNo);
                                        DataTable dtservice = dsservice.Tables[0];
                                        DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseCaseMstListEntity[0].ApplNo);
                                        DataTable serviceSaveDT = serviceSaveDS.Tables[0];


                                        //foreach (DataRow drservices in dtservice.Rows)
                                        //{
                                        //    dritem.Pip_service_list_Mode
                                        //}

                                        string[] strServices = dritem.PIP_SERVICES.Split(',');
                                        int intservices = 0;
                                        bool booladdservice = true;
                                        string strservicemode = string.Empty;
                                        foreach (string strserv in strServices)
                                        {
                                            if (strserv.ToString() != string.Empty)
                                            {
                                                booladdservice = true;
                                                foreach (DataRow drservicessave in serviceSaveDT.Rows)
                                                {
                                                    if (strserv.Trim() == (drservicessave["INQ_CODE"].ToString().Trim()))
                                                    {
                                                        booladdservice = false;
                                                        ListItem lstitem = dritem.Pip_service_list_Mode.Find(u => u.Text.ToString().Trim() == drservicessave["INQ_CODE"].ToString().Trim());
                                                        dritem.Pip_service_list_Mode.Remove(lstitem);
                                                        break;
                                                    }
                                                }
                                                if (booladdservice)
                                                {
                                                    foreach (DataRow drservices in dtservice.Rows)
                                                    {

                                                        if (strserv.Trim() == (drservices["INQ_CODE"].ToString().Trim()))
                                                        {
                                                            if (dritem.Pip_service_list_Mode.Count > 0)
                                                            {
                                                                strservicemode = dritem.Pip_service_list_Mode[intservices].Value.ToString();
                                                            }
                                                            intservices = intservices + 1;
                                                            intRowIndex = gvwSubdetails.Rows.Add("Service Inq" + intservices, string.Empty, drservices["INQ_DESC"].ToString(), strservicemode == "TRUE" ? true : false, string.Empty);
                                                        }
                                                    }
                                                }
                                            }
                                        }



                                    }

                                }
                                //if (dritem.PIP_RELATIONSHIP"].ToString().Trim() != string.Empty)
                                //{
                                //    stroldvalue = strNewvalue  = strFieldType = string.Empty;
                                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                                //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0][""].ToString().Trim());
                                //    if (comOldvalue != null)
                                //    {
                                //        stroldvalue = comOldvalue.Desc;
                                //    }
                                //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RELATIONSHIP"].ToString().Trim());
                                //    if (comNewvalue != null)
                                //    {
                                //        strNewvalue = comNewvalue.Desc;
                                //    }

                                //    if (dritem.PIP_RELATIONSHIP"].ToString().Trim() != dtview.Rows[0][""].ToString().Trim())
                                //    {
                                //        gvwSubdetails.Rows.Add("", stroldvalue, strNewvalue);
                                //    }
                                //}

                                //if (dritem.PIP_YOUTH.ToString().Trim() != string.Empty)
                                //{
                                //    stroldvalue = strNewvalue  = strFieldType = string.Empty;
                                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DisconnectedYouth, "**", "**", "**", string.Empty);

                                //    //CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_YOUTH"].ToString().Trim());
                                //    //if (comOldvalue != null)
                                //    //{
                                //    //    stroldvalue = comOldvalue.Desc;
                                //    //}
                                //    //CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_YOUTH.ToString().Trim());
                                //    //if (comNewvalue != null)
                                //    //{
                                //    //    strNewvalue = comNewvalue.Desc;
                                //    //}

                                //    //if (dritem.PIP_YOUTH.ToString().Trim() != dtview.Rows[0]["SNP_YOUTH"].ToString().Trim())
                                //    //{
                                //    //    gvwSubdetails.Rows.Add("Disconnected Youth", stroldvalue, strNewvalue, dritem.PIP_YOUTH_MODE == "TRUE" ? true : false);
                                //    //}
                                //}




                                if (dritem.PIP_INCOME_TYPES.ToString().Trim() != string.Empty)
                                {
                                    // string pipremovetypes = dritem.PIP_INCOME_TYPES.ToString().Replace(",", "");
                                    //string strcurrenIncometypes = string.Empty;
                                    //List<string> strincomecurrentlist = new List<string>();
                                    //foreach (CaseIncomeEntity item in _propCurrentIncomelist)
                                    //{
                                    //    if (!strcurrenIncometypes.Contains(item.Type))
                                    //    {
                                    //        strincomecurrentlist.Add(item.Type);
                                    //        strcurrenIncometypes = item.Type + "," + strcurrenIncometypes;
                                    //    }
                                    //}
                                    // string mstremovetypes = BaseForm.BaseCaseMstListEntity[0].IncomeTypes.ToString().Replace(" ", "");



                                    string[] strIncomeTypes = dritem.PIP_INCOME_TYPES.Split(',');
                                    int intincomeTypes = 0;
                                    bool booladdservice = true;
                                    string strincomemode = string.Empty;
                                    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.INCOMETYPES, "**", "**", "**", string.Empty);


                                    foreach (string strincome in strIncomeTypes)
                                    {

                                        if (strincome.ToString() != string.Empty)
                                        {
                                            booladdservice = true;
                                            foreach (CaseIncomeEntity item in _propCurrentIncomelist)
                                            {

                                                if (strincome.Trim() == item.Type.Trim())
                                                {
                                                    booladdservice = false;
                                                    ListItem lstitem = dritem.Pip_Income_list_Mode.Find(u => u.Text.ToString().Trim() == item.Type.ToString().Trim());
                                                    dritem.Pip_Income_list_Mode.Remove(lstitem);
                                                    break;
                                                }
                                            }
                                            if (booladdservice)
                                            {
                                                //  comOldvalue = listAgytabls.Find(u => u.Code.Trim() == strincome.Trim());

                                                foreach (CommonEntity item in listAgytabls)
                                                {

                                                    if (strincome.Trim() == (item.Code.ToString().Trim()))
                                                    {

                                                        if (dritem.Pip_Income_list_Mode.Count > 0)
                                                        {
                                                            strincomemode = dritem.Pip_Income_list_Mode[intincomeTypes].Value.ToString();
                                                        }
                                                        intincomeTypes = intincomeTypes + 1;
                                                        intRowIndex = gvwSubdetails.Rows.Add("Income Type" + intincomeTypes, string.Empty, item.Desc.ToString(), strincomemode == "TRUE" ? true : false, string.Empty);
                                                    }
                                                }
                                            }
                                        }


                                    }



                                    //if (pipremovetypes.ToUpper().Trim() != strcurrenIncometypes.ToUpper().Trim())
                                    //{
                                    //    stroldvalue = strNewvalue  = strFieldType = string.Empty;
                                    //    listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.INCOMETYPES, "**", "**", "**", string.Empty);






                                    //    foreach (var ncashbenitem in strincomecurrentlist)
                                    //    {

                                    //        comOldvalue = listAgytabls.Find(u => u.Code.Trim() == ncashbenitem.Trim());
                                    //        if (comOldvalue != null)
                                    //        {
                                    //            stroldvalue = stroldvalue + comOldvalue.Desc + ", ";
                                    //        }
                                    //    }
                                    //    string[] strLeanNCashben = dritem.PIP_INCOME_TYPES.ToString().Trim().Split(',');

                                    //    foreach (var ncashbenitem in strLeanNCashben)
                                    //    {
                                    //        comNewvalue = listAgytabls.Find(u => u.Code == ncashbenitem.Trim());
                                    //        if (comNewvalue != null)
                                    //        {
                                    //            strNewvalue = strNewvalue + comNewvalue.Desc + ",";
                                    //        }
                                    //    }


                                    //    if (strincomecurrentlist.Count != strLeanNCashben.Length)
                                    //    {
                                    //        intValidmode = intValidmode + 1;
                                    //        gvwSubdetails.Rows.Add("Income Types", stroldvalue.ToUpper(), strNewvalue.ToUpper(), dritem.PIP_INCOME_TYPES_MODE == "TRUE" ? true : false);

                                    //    }
                                    //    else
                                    //    {
                                    //        bool boolvalid = true;
                                    //        foreach (var ncashbenitem in strincomecurrentlist)
                                    //        {
                                    //            if (!pipremovetypes.Contains(ncashbenitem.Trim()))
                                    //            {
                                    //                boolvalid = false;
                                    //            }
                                    //        }
                                    //        if (boolvalid == false)
                                    //        {
                                    //            intValidmode = intValidmode + 1;
                                    //            gvwSubdetails.Rows.Add("Income Types", stroldvalue.ToUpper(), strNewvalue.ToUpper(), dritem.PIP_INCOME_TYPES_MODE == "TRUE" ? true : false);

                                    //        }
                                    //    }
                                    //}
                                }



                                if (intValidmode > 1)
                                {

                                    List<DataGridViewRow> SelectedgvRows = (from c in gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList()
                                                                            where (((DataGridViewCheckBoxCell)c.Cells["gvtSelchk"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                                            select c).ToList();
                                    dritem.PIP_Valid_MODE = SelectedgvRows.Count;
                                }
                                gvwCustomer.SelectedRows[0].Tag = dritem;

                                if (gvwSubdetails.Rows.Count == 0)
                                {
                                    lblMessage2.Text = "No field differences between [" + propCurentShortName + "] and [" + propRecentShortName + "]";
                                    lblMessage2.ForeColor = Color.Red;
                                    lblUpdateMessage.Visible = chkSelectAll.Visible = btnLeanData.Visible = false;
                                }

                                if (dritem.PIP_FNAME.ToString().Trim().ToUpper() != Basesnpdata.NameixFi.ToString().Trim().ToUpper())
                                {
                                    btnCombine.Visible = true;
                                    lblCombineMsg.Visible = true;
                                    lblCombineMsg.Text = "There is a possible typeo of the first name. Press button to represent separate these as 2 members";
                                    btnCombine.Text = "separate";
                                }
                                else if (LookupDataAccess.Getdate(dritem.PIP_DOB.ToString().Trim()) != LookupDataAccess.Getdate(Basesnpdata.AltBdate.ToString().Trim()))
                                {
                                    btnCombine.Visible = true;
                                    lblCombineMsg.Visible = true;
                                    lblCombineMsg.Text = "There is possible wrong date of birth entered both the first and last name are the same.";
                                    btnCombine.Text = "separate";
                                }
                                else
                                {
                                    btnCombine.Visible = false;
                                    lblCombineMsg.Visible = false;
                                    lblCombineMsg.Text = "";
                                }

                            }
                            else
                            {
                                btnIncomeTypes.Visible = btnNoncashbenefits.Visible = btnViewAllHealthcodes.Visible = false;
                                if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString() == "I")
                                {

                                    lblMessage2.Text = "This member is not in the [" + propCurentShortName + "] household. CHECK the box to INACTIVATE from the [" + propRecentShortName + "] intake";
                                    lblMessage2.ForeColor = Color.Red;
                                    lblUpdateMessage.Visible = chkSelectAll.Visible = false;
                                }
                                else
                                {

                                    lblMessage2.Text = "This member is not in the [" + propCurentShortName + "] household. CHECK the box to ADD it from the [" + propRecentShortName + "] intake";
                                    lblMessage2.ForeColor = Color.Red;
                                    lblUpdateMessage.Visible = chkSelectAll.Visible = false;

                                }
                                if (gvwCustomer.SelectedRows[0].Cells["gvtAppType"].Value.ToString() != string.Empty)
                                {
                                    btnCombine.Visible = true;
                                    lblCombineMsg.Visible = true;
                                    btnCombine.Text = "combine";

                                    CaseSnpEntity Basesnpdata = null;
                                    if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString() == "I")
                                    {
                                        Basesnpdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                                        if (Basesnpdata == null)
                                            Basesnpdata = propCasesnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());
                                    }
                                    else
                                    {
                                        Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                                        if (Basesnpdata == null)
                                            Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                                    }

                                    if (dritem.PIP_FNAME.ToString().Trim().ToUpper() != Basesnpdata.NameixFi.ToString().Trim().ToUpper())
                                    {
                                        lblCombineMsg.Text = "There is a possible typeo of the first name. Press button to represent GREEN and RED row was 1 person.";
                                    }
                                    else if (LookupDataAccess.Getdate(dritem.PIP_DOB.ToString().Trim()) != LookupDataAccess.Getdate(Basesnpdata.AltBdate.ToString().Trim()))
                                    {
                                        lblCombineMsg.Text = "There is possible wrong date of birth entered both the first and last name are the same.";

                                    }
                                }
                                else
                                {
                                    btnCombine.Visible = false;
                                    lblCombineMsg.Visible = false;
                                    lblCombineMsg.Text = "";

                                }
                            }

                        }


                    }
                    else
                    {
                        LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;
                        string stroldvalue = string.Empty; string strNewvalue = string.Empty;
                        if (dritem != null)
                        {
                            if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString() != "I")
                            {
                                btnNoncashbenefits.Visible = btnViewAllHealthcodes.Visible = true;
                                DataTable dtview = _propSnpDatatable;
                                DataView dv = dtview.DefaultView;
                                dv.RowFilter = "Fname LIKE '" + dritem.PIP_FNAME.FirstOrDefault() + "*' AND DOB = '" + dritem.PIP_DOB.ToString() + "'";

                                if (dv.ToTable().Rows.Count == 0)
                                {
                                    dv = dtview.DefaultView;
                                    dv.RowFilter = "Fname = '" + dritem.PIP_FNAME.ToString() + "' AND Lname = '" + dritem.PIP_LNAME.ToString() + "'";
                                }

                                dtview = dv.ToTable();
                                if (dtview.Rows.Count > 0)
                                {
                                    if (dritem.PIP_SSN.ToString().Trim() != string.Empty)
                                    {
                                        if (dritem.PIP_SSN.ToString().Trim() != dtview.Rows[0]["SSN"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("SSN", dtview.Rows[0]["SSN"].ToString(), dritem.PIP_SSN.ToString().Trim(), dritem.PIP_SSN_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_FNAME.ToString().Trim() != string.Empty)
                                    {
                                        if (dritem.PIP_FNAME.ToString().Trim().ToUpper() != dtview.Rows[0]["Fname"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("First Name", dtview.Rows[0]["Fname"].ToString(), dritem.PIP_FNAME.ToString().Trim(), dritem.PIP_FNAME_MODE == "TRUE" ? true : false);
                                        }
                                    }


                                    if (dritem.PIP_MNAME.ToString().Trim() != string.Empty)
                                    {
                                        if (dritem.PIP_MNAME.ToString().Trim().ToUpper() != dtview.Rows[0]["MName"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("MI", dtview.Rows[0]["MName"].ToString(), dritem.PIP_MNAME.ToString().Trim(), dritem.PIP_MNAME_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_LNAME.ToString().Trim() != string.Empty)
                                    {
                                        if (dritem.PIP_LNAME.ToString().Trim().ToUpper() != dtview.Rows[0]["LName"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Last Name", dtview.Rows[0]["LName"].ToString(), dritem.PIP_LNAME.ToString().Trim(), dritem.PIP_LNAME_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_DOB.ToString().Trim() != string.Empty)
                                    {
                                        if (LookupDataAccess.Getdate(dritem.PIP_DOB.ToString().Trim().ToUpper()) != LookupDataAccess.Getdate(dtview.Rows[0]["DOB"].ToString()))
                                        {

                                            intRowIndex = gvwSubdetails.Rows.Add("Dob", LookupDataAccess.Getdate(dtview.Rows[0]["DOB"].ToString()), LookupDataAccess.Getdate(dritem.PIP_DOB.ToString()), dritem.PIP_DOB_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    //if (dritem.PIP_MNAME.ToString().Trim() != string.Empty)
                                    //{
                                    //    if (dritem.PIP_MNAME.ToString().Trim() != dtview.Rows[0]["SNP_NAME_IX_MI"].ToString().Trim())
                                    //    {
                                    //        intRowIndex = gvwSubdetails.Rows.Add("MI", dtview.Rows[0]["SNP_NAME_IX_MI"].ToString(), dritem.PIP_MNAME.ToString().Trim(), dritem.PIP_MNAME_MODE == "TRUE" ? true : false);
                                    //    }
                                    //}
                                    //if (dritem.PIP_LNAME.ToString().Trim() != string.Empty)
                                    //{
                                    //    if (dritem.PIP_LNAME.ToString().Trim() != dtview.Rows[0]["SNP_NAME_IX_LAST"].ToString().Trim())
                                    //    {
                                    //        intRowIndex = gvwSubdetails.Rows.Add("MI", dtview.Rows[0]["SNP_NAME_IX_LAST"].ToString(), dritem.PIP_LNAME.ToString().Trim(), dritem.PIP_LNAME_MODE == "TRUE" ? true : false);
                                    //    }
                                    //}



                                    if (dritem.PIP_GENDER.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_SEX"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_GENDER.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_GENDER.ToString().Trim() != dtview.Rows[0]["SNP_SEX"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Gender", stroldvalue, strNewvalue, dritem.PIP_GENDER_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_PREGNANT.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.PREGNANT, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_PREGNANT"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_PREGNANT.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_PREGNANT.ToString().Trim() != dtview.Rows[0]["SNP_PREGNANT"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Are You Pregnant?", dtview.Rows[0]["SNP_PREGNANT"].ToString(), dritem.PIP_PREGNANT.ToString().Trim(), dritem.PIP_PREGNANT_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_MARITAL_STATUS.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MARITALSTATUS, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_MARITAL_STATUS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_MARITAL_STATUS.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_MARITAL_STATUS.ToString().Trim() != dtview.Rows[0]["SNP_MARITAL_STATUS"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Marital Status", stroldvalue, strNewvalue, dritem.PIP_MARITAL_STATUS_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_RELATIONSHIP.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RELATIONSHIP.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_RELATIONSHIP.ToString().Trim() != dtview.Rows[0]["SNP_MEMBER_CODE"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Relation", stroldvalue, strNewvalue, dritem.PIP_RELATIONSHIP_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_ETHNIC.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_ETHNIC"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_ETHNIC.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_ETHNIC.ToString().Trim() != dtview.Rows[0]["SNP_ETHNIC"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Ethnicity", stroldvalue, strNewvalue, dritem.PIP_ETHNIC_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_RACE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_RACE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RACE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_RACE.ToString().Trim() != dtview.Rows[0]["SNP_RACE"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Race", stroldvalue, strNewvalue, dritem.PIP_RACE_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_EDUCATION.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_EDUCATION"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_EDUCATION.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_EDUCATION.ToString().Trim() != dtview.Rows[0]["SNP_EDUCATION"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Education", stroldvalue, strNewvalue, dritem.PIP_EDUCATION_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_SCHOOL.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SCHOOLDISTRICTS, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_SCHOOL_DISTRICT"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_SCHOOL.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_SCHOOL.ToString().Trim() != dtview.Rows[0]["SNP_SCHOOL_DISTRICT"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("School", stroldvalue, strNewvalue, dritem.PIP_SCHOOL_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_RELITRAN.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELIABLETRANSPORTATION, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_RELITRAN"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RELITRAN.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_RELITRAN.ToString().Trim() != dtview.Rows[0]["SNP_RELITRAN"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Reliable Transport", stroldvalue, strNewvalue, dritem.PIP_RELITRAN_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_DRVLIC.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DRIVERLICENSE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_DRVLIC"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_DRVLIC.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_DRVLIC.ToString().Trim() != dtview.Rows[0]["SNP_DRVLIC"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Drivers License", stroldvalue, strNewvalue, dritem.PIP_DRVLIC_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_HEALTH_INS.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEALTHINSURANCE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_HEALTH_INS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_HEALTH_INS.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_HEALTH_INS.ToString().Trim() != dtview.Rows[0]["SNP_HEALTH_INS"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Health Insurance", stroldvalue, strNewvalue, dritem.PIP_HEALTH_INS_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_HEALTH_CODES.ToString().Trim() != string.Empty)
                                    {

                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HealthCodes, "**", "**", "**", string.Empty);

                                        string[] strPipHealthcodes = dritem.PIP_HEALTH_CODES.ToString().Trim().Split(',');
                                        string[] strHealthcodes = dtview.Rows[0]["SNP_HEALTH_CODES"].ToString().Trim().Split(',');
                                        bool booladdHealthcode = true;
                                        int inthealthcode = 0;
                                        string strNoncashmode = string.Empty;
                                        foreach (string strHealthitem in strPipHealthcodes)
                                        {

                                            if (strHealthitem.ToString() != string.Empty)
                                            {
                                                booladdHealthcode = true;
                                                foreach (string item in strHealthcodes)
                                                {

                                                    if (strHealthitem.Trim() == item.Trim())
                                                    {
                                                        booladdHealthcode = false;
                                                        ListItem lstitem = dritem.Pip_Healthcodes_list_Mode.Find(u => u.Text.ToString().Trim() == item.ToString().Trim());
                                                        dritem.Pip_Healthcodes_list_Mode.Remove(lstitem);
                                                        break;
                                                    }
                                                }
                                                if (booladdHealthcode)
                                                {
                                                    //  comOldvalue = listAgytabls.Find(u => u.Code.Trim() == strincome.Trim());

                                                    foreach (CommonEntity item in listAgytabls)
                                                    {

                                                        if (strHealthitem.Trim() == (item.Code.ToString().Trim()))
                                                        {

                                                            if (dritem.Pip_Healthcodes_list_Mode.Count > 0)
                                                            {
                                                                strNoncashmode = dritem.Pip_Healthcodes_list_Mode[inthealthcode].Value.ToString();
                                                            }
                                                            inthealthcode = inthealthcode + 1;
                                                            intRowIndex = gvwSubdetails.Rows.Add("Health Ins " + inthealthcode, string.Empty, item.Desc.ToString(), strNoncashmode == "TRUE" ? true : false, string.Empty);
                                                        }
                                                    }
                                                }
                                            }


                                        }

                                    }
                                    if (dritem.PIP_MILITARY_STATUS.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MilitaryStatus, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_MILITARY_STATUS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_MILITARY_STATUS.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_MILITARY_STATUS.ToString().Trim() != dtview.Rows[0]["SNP_MILITARY_STATUS"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Military Status", stroldvalue, strNewvalue, dritem.PIP_MILITARY_STATUS_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_VETERAN.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.VETERAN, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_VET"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_VETERAN.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_VETERAN.ToString().Trim() != dtview.Rows[0]["SNP_VET"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Veteran", stroldvalue, strNewvalue, dritem.PIP_VETERAN_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_FOOD_STAMP.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FOODSTAMPS, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_FOOD_STAMPS"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_FOOD_STAMP.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_FOOD_STAMP.ToString().Trim() != dtview.Rows[0]["SNP_FOOD_STAMPS"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Food Stamp", stroldvalue, strNewvalue, dritem.PIP_FOOD_STAMP_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_DISABLE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_DISABLE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_DISABLE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_DISABLE.ToString().Trim() != dtview.Rows[0]["SNP_DISABLE"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Disable", stroldvalue, strNewvalue, dritem.PIP_DISABLE_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_FARMER.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FARMER, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_FARMER"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_FARMER.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_FARMER.ToString().Trim() != dtview.Rows[0]["SNP_FARMER"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Farmer", stroldvalue, strNewvalue, dritem.PIP_FARMER_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_WORK_STAT.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_WORK_STAT"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_WORK_STAT.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_WORK_STAT.ToString().Trim() != dtview.Rows[0]["SNP_WORK_STAT"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Work Status", stroldvalue, strNewvalue, dritem.PIP_WORK_STAT_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_WIC.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WIC, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_WIC"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_WIC.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_WIC.ToString().Trim() != dtview.Rows[0]["SNP_WIC"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("WIC", stroldvalue, strNewvalue, dritem.PIP_WIC_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_NCASHBEN.ToString().Trim() != string.Empty)
                                    {

                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, "**", "**", "**", string.Empty);

                                        string[] strPipNoncashbenefit = dritem.PIP_NCASHBEN.ToString().Trim().Split(',');
                                        string[] strNCashben = dtview.Rows[0]["SNP_NCASHBEN"].ToString().Trim().Split(',');
                                        bool booladdNoncash = true;
                                        int intNoncashbenefit = 0;
                                        string strNoncashmode = string.Empty;
                                        foreach (string strNonbitem in strPipNoncashbenefit)
                                        {

                                            if (strNonbitem.ToString() != string.Empty)
                                            {
                                                booladdNoncash = true;
                                                foreach (string item in strNCashben)
                                                {

                                                    if (strNonbitem.Trim() == item.Trim())
                                                    {
                                                        booladdNoncash = false;
                                                        ListItem lstitem = dritem.Pip_NonCashbenefit_list_Mode.Find(u => u.Text.ToString().Trim() == item.ToString().Trim());
                                                        dritem.Pip_NonCashbenefit_list_Mode.Remove(lstitem);
                                                        break;
                                                    }
                                                }
                                                if (booladdNoncash)
                                                {
                                                    //  comOldvalue = listAgytabls.Find(u => u.Code.Trim() == strincome.Trim());

                                                    foreach (CommonEntity item in listAgytabls)
                                                    {

                                                        if (strNonbitem.Trim() == (item.Code.ToString().Trim()))
                                                        {

                                                            if (dritem.Pip_NonCashbenefit_list_Mode.Count > 0)
                                                            {
                                                                strNoncashmode = dritem.Pip_NonCashbenefit_list_Mode[intNoncashbenefit].Value.ToString();
                                                            }
                                                            intNoncashbenefit = intNoncashbenefit + 1;
                                                            intRowIndex = gvwSubdetails.Rows.Add("Non Cash Benefit " + intNoncashbenefit, string.Empty, item.Desc.ToString(), strNoncashmode == "TRUE" ? true : false, string.Empty);
                                                        }
                                                    }
                                                }
                                            }


                                        }
                                    }

                                    //    if (dritem.PIP_NCASHBEN.ToString().Trim().ToUpper() != dtview.Rows[0]["SNP_NCASHBEN"].ToString().Trim().ToUpper())
                                    //    {
                                    //        stroldvalue = strNewvalue = string.Empty;
                                    //        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, "**", "**", "**", string.Empty);

                                    //        string[] strNCashben = dtview.Rows[0]["SNP_NCASHBEN"].ToString().Trim().Split(',');

                                    //        foreach (var ncashbenitem in strNCashben)
                                    //        {

                                    //            CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == ncashbenitem);
                                    //            if (comOldvalue != null)
                                    //            {
                                    //                stroldvalue = stroldvalue + comOldvalue.Desc + ", ";
                                    //            }
                                    //        }
                                    //        string[] strLeanNCashben = dritem.PIP_NCASHBEN.ToString().Trim().Split(',');

                                    //        foreach (var ncashbenitem in strLeanNCashben)
                                    //        {
                                    //            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == ncashbenitem.Trim());
                                    //            if (comNewvalue != null)
                                    //            {
                                    //                strNewvalue = strNewvalue + comNewvalue.Desc + ",";
                                    //            }
                                    //        }

                                    //        intRowIndex = gvwSubdetails.Rows.Add("Non Cash Benefit", stroldvalue.ToUpper(), strNewvalue.ToUpper(), dritem.PIP_NCASHBEN_MODE == "TRUE" ? true : false);
                                    //    }
                                    //}



                                    //if (dritem.PIP_RELATIONSHIP"].ToString().Trim() != string.Empty)
                                    //{
                                    //    stroldvalue = strNewvalue = string.Empty;
                                    //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                                    //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0][""].ToString().Trim());
                                    //    if (comOldvalue != null)
                                    //    {
                                    //        stroldvalue = comOldvalue.Desc;
                                    //    }
                                    //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_RELATIONSHIP"].ToString().Trim());
                                    //    if (comNewvalue != null)
                                    //    {
                                    //        strNewvalue = comNewvalue.Desc;
                                    //    }

                                    //    if (dritem.PIP_RELATIONSHIP"].ToString().Trim() != dtview.Rows[0][""].ToString().Trim())
                                    //    {
                                    //        gvwSubdetails.Rows.Add("", stroldvalue, strNewvalue);
                                    //    }
                                    //}





                                    //if (dritem.PIP_YOUTH.ToString().Trim() != string.Empty)
                                    //{
                                    //    stroldvalue = strNewvalue = string.Empty;
                                    //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DisconnectedYouth, "**", "**", "**", string.Empty);

                                    //    //CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["SNP_YOUTH"].ToString().Trim());
                                    //    //if (comOldvalue != null)
                                    //    //{
                                    //    //    stroldvalue = comOldvalue.Desc;
                                    //    //}
                                    //    //CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_YOUTH.ToString().Trim());
                                    //    //if (comNewvalue != null)
                                    //    //{
                                    //    //    strNewvalue = comNewvalue.Desc;
                                    //    //}

                                    //    //if (dritem.PIP_YOUTH.ToString().Trim() != dtview.Rows[0]["SNP_YOUTH"].ToString().Trim())
                                    //    //{
                                    //    //    gvwSubdetails.Rows.Add("Disconnected Youth", stroldvalue, strNewvalue, dritem.PIP_YOUTH_MODE == "TRUE" ? true : false);
                                    //    //}
                                    //}


                                    if (dritem.PIP_HOUSENO.ToString().Trim() != string.Empty)
                                    {

                                        if (dritem.PIP_HOUSENO.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_HN"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("House No ", dtview.Rows[0]["MST_HN"].ToString(), dritem.PIP_HOUSENO.ToString().Trim(), dritem.PIP_HOUSENO_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_DIRECTION.ToString().Trim() != string.Empty)
                                    {

                                        if (dritem.PIP_DIRECTION.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_DIRECTION"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Direction", dtview.Rows[0]["MST_DIRECTION"].ToString(), dritem.PIP_DIRECTION.ToString().Trim(), dritem.PIP_DIRECTION_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_STREET.ToString().Trim() != string.Empty)
                                    {

                                        if (dritem.PIP_STREET.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_STREET"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Street", dtview.Rows[0]["MST_STREET"].ToString(), dritem.PIP_STREET.ToString().Trim(), dritem.PIP_STREET_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_SUFFIX.ToString().Trim() != string.Empty)
                                    {
                                        if (dritem.PIP_SUFFIX.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_SUFFIX"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Suffix", dtview.Rows[0]["MST_SUFFIX"].ToString(), dritem.PIP_SUFFIX.ToString().Trim(), dritem.PIP_SUFFIX_MODE == "TRUE" ? true : false);
                                        }

                                    }
                                    if (dritem.PIP_APT.ToString().Trim() != string.Empty)
                                    {
                                        if (dritem.PIP_APT.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_APT"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Apartment", dtview.Rows[0]["MST_APT"].ToString().Trim(), dritem.PIP_APT.ToString().Trim(), dritem.PIP_APT_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_FLR.ToString().Trim() != string.Empty)
                                    {

                                        if (dritem.PIP_FLR.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_FLR"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Floor", dtview.Rows[0]["MST_FLR"].ToString(), dritem.PIP_FLR.ToString().Trim(), dritem.PIP_FLR_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_CITY.ToString().Trim() != string.Empty)
                                    {

                                        if (dritem.PIP_CITY.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_CITY"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("City", dtview.Rows[0]["MST_CITY"].ToString(), dritem.PIP_CITY.ToString().Trim(), dritem.PIP_CITY_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_ZIP.ToString().Trim() != string.Empty)
                                    {

                                        if (dritem.PIP_ZIP.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_ZIP"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Zip", dtview.Rows[0]["MST_ZIP"].ToString(), dritem.PIP_ZIP.ToString().Trim(), dritem.PIP_ZIP_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_STATE.ToString().Trim() != string.Empty)
                                    {
                                        if (dritem.PIP_STATE.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_STATE"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("State", dtview.Rows[0]["MST_STATE"].ToString(), dritem.PIP_STATE.ToString().Trim(), dritem.PIP_STATE_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_COUNTY.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["MST_COUNTY"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_COUNTY.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_COUNTY.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_COUNTY"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("County", stroldvalue, strNewvalue, dritem.PIP_COUNTY_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_TOWNSHIP.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["MST_TOWNSHIP"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_TOWNSHIP.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_TOWNSHIP.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_TOWNSHIP"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Township", stroldvalue, strNewvalue, dritem.PIP_TOWNSHIP_MODE == "TRUE" ? true : false);
                                        }
                                    }


                                    if (dritem.PIP_PRI_LANGUAGE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["MST_LANGUAGE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_PRI_LANGUAGE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_PRI_LANGUAGE.ToString().Trim() != dtview.Rows[0]["MST_LANGUAGE"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Language", stroldvalue, strNewvalue, dritem.PIP_PRI_LANGUAGE_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_FAMILY_TYPE.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["MST_FAMILY_TYPE"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_FAMILY_TYPE.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_FAMILY_TYPE.ToString().Trim() != dtview.Rows[0]["MST_FAMILY_TYPE"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Family Type", stroldvalue, strNewvalue, dritem.PIP_FAMILY_TYPE_MODE == "TRUE" ? true : false);
                                        }
                                    }


                                    if (dritem.PIP_HOME_PHONE.ToString().Trim() != string.Empty)
                                    {

                                        if (dritem.PIP_AREA.ToString().Trim() + dritem.PIP_HOME_PHONE.ToString().Trim() != (dtview.Rows[0]["MST_AREA"].ToString().Trim() + dtview.Rows[0]["MST_PHONE"].ToString().Trim()))
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Home Phone", (dtview.Rows[0]["MST_AREA"].ToString().Trim() + dtview.Rows[0]["MST_PHONE"].ToString().Trim()), dritem.PIP_AREA.ToString().Trim() + dritem.PIP_HOME_PHONE.ToString().Trim(), dritem.PIP_HOME_PHONE_MODE == "TRUE" ? true : false);
                                        }
                                    }
                                    if (dritem.PIP_CELL_NUMBER.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;

                                        if (dritem.PIP_CELL_NUMBER.ToString().Trim().ToUpper() != dtview.Rows[0]["MST_CELL_PHONE"].ToString().Trim().ToUpper())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Cell Number", dtview.Rows[0]["MST_CELL_PHONE"].ToString().Trim(), dritem.PIP_CELL_NUMBER.ToString().Trim(), dritem.PIP_CELL_NUMBER_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_HOUSING.ToString().Trim() != string.Empty)
                                    {
                                        stroldvalue = strNewvalue = string.Empty;
                                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, "**", "**", "**", string.Empty);

                                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == dtview.Rows[0]["MST_HOUSING"].ToString().Trim());
                                        if (comOldvalue != null)
                                        {
                                            stroldvalue = comOldvalue.Desc;
                                        }
                                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == dritem.PIP_HOUSING.ToString().Trim());
                                        if (comNewvalue != null)
                                        {
                                            strNewvalue = comNewvalue.Desc;
                                        }

                                        if (dritem.PIP_HOUSING.ToString().Trim() != dtview.Rows[0]["MST_HOUSING"].ToString().Trim())
                                        {
                                            intRowIndex = gvwSubdetails.Rows.Add("Housing", stroldvalue, strNewvalue, dritem.PIP_HOUSENO_MODE == "TRUE" ? true : false);
                                        }
                                    }

                                    if (dritem.PIP_INCOME_TYPES.ToString().Trim() != string.Empty)
                                    {
                                        string pipremovetypes = dritem.PIP_INCOME_TYPES.ToString().Replace(",", "");
                                        string mstremovetypes = dtview.Rows[0]["MST_INCOME_TYPES"].ToString().Replace(" ", "");
                                        if (pipremovetypes.ToUpper().Trim() != mstremovetypes.ToUpper().Trim())
                                        {
                                            stroldvalue = strNewvalue = string.Empty;
                                            List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.INCOMETYPES, "**", "**", "**", string.Empty);


                                            string strvalue = dtview.Rows[0]["MST_INCOME_TYPES"].ToString();
                                            List<string> strlist = new List<string>();
                                            for (int i = 0; i < strvalue.Length; i += 2)
                                                if (i + 2 > strvalue.Length)
                                                    strlist.Add(strvalue.Substring(i));
                                                else
                                                    strlist.Add(strvalue.Substring(i, 2));



                                            //  var output = dtview.Rows[0]["MST_INCOME_TYPES"].ToString().ToLookup(c => Math.Floor(k++ /partsize )).Select(u => new String(u.ToArray()));
                                            // List<string> strlist = new List<string>();
                                            //// string[] strNCashben = //dtview.Rows[0]["MST_INCOME_TYPES"].ToString().Trim().Split(' ');
                                            // for (int i = 0; i < dtview.Rows[0]["MST_INCOME_TYPES"].ToString().Length; i++)
                                            // {
                                            //     if (i % 2 == 0)
                                            //         strlist.Add(i.ToString());

                                            // }



                                            foreach (var ncashbenitem in strlist)
                                            {

                                                CommonEntity comOldvalue = listAgytabls.Find(u => u.Code.Trim() == ncashbenitem.Trim());
                                                if (comOldvalue != null)
                                                {
                                                    stroldvalue = stroldvalue + comOldvalue.Desc + ", ";
                                                }
                                            }
                                            string[] strLeanNCashben = dritem.PIP_INCOME_TYPES.ToString().Trim().Split(',');

                                            foreach (var ncashbenitem in strLeanNCashben)
                                            {
                                                CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == ncashbenitem.Trim());
                                                if (comNewvalue != null)
                                                {
                                                    strNewvalue = strNewvalue + comNewvalue.Desc + ",";
                                                }
                                            }


                                            if (strlist.Count != strLeanNCashben.Length)
                                            {

                                                intRowIndex = gvwSubdetails.Rows.Add("Income Types", stroldvalue.ToUpper(), strNewvalue.ToUpper(), dritem.PIP_INCOME_TYPES_MODE == "TRUE" ? true : false);

                                            }
                                            else
                                            {
                                                bool boolvalid = true;
                                                foreach (var ncashbenitem in strlist)
                                                {
                                                    if (!pipremovetypes.Contains(ncashbenitem.Trim()))
                                                    {
                                                        boolvalid = false;
                                                    }
                                                }
                                                if (boolvalid == false)
                                                {
                                                    intRowIndex = gvwSubdetails.Rows.Add("Income Types", stroldvalue.ToUpper(), strNewvalue.ToUpper(), dritem.PIP_INCOME_TYPES_MODE == "TRUE" ? true : false);

                                                }
                                            }
                                        }
                                    }
                                    if (dritem.PIP_SERVICES != string.Empty)
                                    {
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

                                        string strAgy = "00";
                                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                                            strAgy = propAgency;

                                        DataTable dtservice = PIPDATA.GETPIPSERVICES(BaseForm.BaseLeanDataBaseConnectionString, strType, BaseForm.BaseAgencyControlDetails.AgyShortName, strAgy);

                                        DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", propAgency, propDept, propProgram, propYear, propAppl);
                                        DataTable serviceSaveDT = serviceSaveDS.Tables[0];


                                        string[] strServices = dritem.PIP_SERVICES.Split(',');
                                        int intservices = 0;
                                        bool booladdservice = true;
                                        string strservicemode = string.Empty;
                                        foreach (string strserv in strServices)
                                        {
                                            if (strserv.ToString() != string.Empty)
                                            {
                                                booladdservice = true;
                                                foreach (DataRow drservicessave in serviceSaveDT.Rows)
                                                {
                                                    if (strserv.Trim() == (drservicessave["INQ_CODE"].ToString().Trim()))
                                                    {
                                                        booladdservice = false;
                                                        ListItem lstitem = dritem.Pip_service_list_Mode.Find(u => u.Text.ToString().Trim() == drservicessave["INQ_CODE"].ToString().Trim());
                                                        dritem.Pip_service_list_Mode.Remove(lstitem);
                                                        break;
                                                    }
                                                }
                                                if (booladdservice)
                                                {
                                                    foreach (DataRow drservices in dtservice.Rows)
                                                    {

                                                        if (strserv.Trim() == (drservices["CODE"].ToString()))
                                                        {
                                                            if (dritem.Pip_service_list_Mode.Count > 0)
                                                            {
                                                                strservicemode = dritem.Pip_service_list_Mode[intservices].Value.ToString();
                                                            }
                                                            intservices = intservices + 1;
                                                            intRowIndex = gvwSubdetails.Rows.Add("Service Inq" + intservices, string.Empty, drservices["DESCRIP"].ToString(), strservicemode == "TRUE" ? true : false);
                                                        }
                                                    }
                                                }
                                            }
                                        }



                                    }


                                }
                                if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString() == "N")
                                {
                                    lblMessage2.Text = "This member is not in CAPTAIN. Select the row if you would like to copy New Member into CAPTAIN from the portal";
                                    lblMessage2.ForeColor = Color.Red;
                                    lblUpdateMessage.Visible = chkSelectAll.Visible = false;
                                }
                                else
                                {
                                    if (gvwSubdetails.Rows.Count == 0)
                                    {
                                        lblMessage2.Text = "No field differences between PIP and CAPTAIN";
                                        lblMessage2.ForeColor = Color.Red;
                                        lblUpdateMessage.Visible = chkSelectAll.Visible = false;
                                        //btnLeanData.Visible = false;
                                        CheckConnectbutton();
                                    }
                                }
                            }
                            else
                            {
                                if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString() == "I")
                                {
                                    lblMessage2.Text = "This member is not in Public Portal, Select the row if you would like to Inactive in CAPTAIN";
                                    lblMessage2.ForeColor = Color.Red;
                                    lblUpdateMessage.Visible = chkSelectAll.Visible = false;
                                }
                                else
                                {
                                    lblMessage2.Text = "This member is not in CAPTAIN. Select the row if you would like to copy New Member into CAPTAIN from the portal.";
                                    lblMessage2.ForeColor = Color.Red;
                                    lblUpdateMessage.Visible = chkSelectAll.Visible = false;
                                }

                            }
                        }
                    }
                }
            }
            // gvwSubdetails.CellValueChanged += gvwSubdetails_CellValueChanged;
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }


        private void btncustomQues_Click(object sender, EventArgs e)
        {
            PipCustomquestionsForm pipcustom = new PipCustomquestionsForm(BaseForm, "Questions", propAgency, propDept, propProgram, propYear, propAppl, propLeanUserId, string.Empty, strFormType);
            pipcustom.StartPosition = FormStartPosition.CenterScreen;
            pipcustom.ShowDialog();
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            PipCustomquestionsForm pipcustom = new PipCustomquestionsForm(BaseForm, string.Empty, propAgency, propDept, propProgram, propYear, propAppl, propLeanUserId, propLeanServices, strFormType);
            pipcustom.StartPosition = FormStartPosition.CenterScreen;
            pipcustom.ShowDialog();
        }


        void CheckItems(DataRow _drDSSXMLitem)
        {
            List<DataGridViewRow> lstCust = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
            //DataRow _drDSSXMLitem = gvwCustomer.SelectedRows[0].Tag as DataRow;
            _drDSSXMLitem["SSNmode"] = "FALSE";
            _drDSSXMLitem["FNAMEmode"] = "FALSE";
            _drDSSXMLitem["MNAMEmode"] = "FALSE";
            _drDSSXMLitem["LNAMEmode"] = "FALSE";
            _drDSSXMLitem["DOBmode"] = "FALSE";
            _drDSSXMLitem["GENDERmode"] = "FALSE";
            _drDSSXMLitem["ETHENICmode"] = "FALSE";
            _drDSSXMLitem["RACEmode"] = "FALSE";

            _drDSSXMLitem["EDUCATIONmode"] = "FALSE";
            _drDSSXMLitem["DISABLEDmode"] = "FALSE";
            _drDSSXMLitem["MILTARYmode"] = "FALSE";
            _drDSSXMLitem["WORKSTATUSmode"] = "FALSE";
            _drDSSXMLitem["HOUSENOmode"] = "FALSE";
            _drDSSXMLitem["STREETmode"] = "FALSE";
            _drDSSXMLitem["ZIPmode"] = "FALSE";
            _drDSSXMLitem["ZIPCODEmode"] = "FALSE";
            _drDSSXMLitem["STATEmode"] = "FALSE";
            _drDSSXMLitem["CITYmode"] = "FALSE";
            _drDSSXMLitem["PHONEmode"] = "FALSE";

            _drDSSXMLitem["CELLmode"] = "FALSE";
            _drDSSXMLitem["LANGUAGEmode"] = "FALSE";
            _drDSSXMLitem["HOUSINGmode"] = "FALSE";
            _drDSSXMLitem["Emailmode"] = "FALSE";

            /*Landlord info*/
            _drDSSXMLitem["LLRFnameMode"] = "FALSE";
            _drDSSXMLitem["LLRLnameMode"] = "FALSE";
            _drDSSXMLitem["LLRPhoneMode"] = "FALSE";
            _drDSSXMLitem["LLRCityMode"] = "FALSE";
            _drDSSXMLitem["LLRStateMode"] = "FALSE";
            _drDSSXMLitem["LLRStreetMode"] = "FALSE";
            _drDSSXMLitem["LLRHouseNoMode"] = "FALSE";
            _drDSSXMLitem["LLRZipMode"] = "FALSE";
            _drDSSXMLitem["LLRZipCodeMode"] = "FALSE";

            /*Case Diff*/
            _drDSSXMLitem["DiffSateMode"] = "FALSE";
            _drDSSXMLitem["DiffCityMode"] = "FALSE";
            _drDSSXMLitem["DiffHNMode"] = "FALSE";
            _drDSSXMLitem["DiffStreetMode"] = "FALSE";
            _drDSSXMLitem["DiffPhoneMode"] = "FALSE";
            _drDSSXMLitem["DiffZipMode"] = "FALSE";
            _drDSSXMLitem["DiffZipPlusMode"] = "FALSE";

            _drDSSXMLitem["MST_LPM_0001Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0002Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0003Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0004Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0005Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0006Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0007Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0008Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0009Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0010Mode"] = "FALSE";
            _drDSSXMLitem["MST_LPM_0011Mode"] = "FALSE";


            _drDSSXMLitem["RENTMORTMode"] = "FALSE";
            _drDSSXMLitem["RELATIONMode"] = "FALSE";
            _drDSSXMLitem["DISYOUTHMode"] = "FALSE";
            _drDSSXMLitem["DWELLINGMode"] = "FALSE";
            _drDSSXMLitem["PRIMPAYHEATMode"] = "FALSE";
            _drDSSXMLitem["PRIMSRCHEARMode"] = "FALSE";




            foreach (DataGridViewRow _dvRow in lstCust)
            {
                if (_dvRow.Cells[0].Value.ToString() == "SSN")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["SSNmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "First Name")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["FNAMEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Middle Name")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["MNAMEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Last Name")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["LNAMEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Dob")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["DOBmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Gender")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["GENDERmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Ethnicity")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["ETHENICmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Race")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["RACEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Education")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["EDUCATIONmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Disable")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["DISABLEDmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Military Status")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["MILTARYmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Work Status")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["WORKSTATUSmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "House No")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["HOUSENOmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Street")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["STREETmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Zip")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["ZIPmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Zip Plus")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["ZIPCODEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "State")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["STATEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "City")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["CITYmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Phone")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["PHONEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Email")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["Emailmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Cell Phone")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["CELLmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Language")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["LANGUAGEmode"] = "TRUE";
                }
                if (_dvRow.Cells[0].Value.ToString() == "Housing")
                {
                    if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE")
                        _drDSSXMLitem["HOUSINGmode"] = "TRUE";
                }

                /*Lanlord info*/
                if (_dvRow.Cells[0].Value.ToString() == "Landlord First Name") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRFnameMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord Last Name") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRLnameMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord Phone") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRPhoneMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord City") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRCityMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord State") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRStateMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord Street") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRStreetMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord HN") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRHouseNoMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord Zip") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRZipMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Landlord ZipPlus") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["LLRZipCodeMode"] = "TRUE"; } }

                /*CASE DiFF*/
                if (_dvRow.Cells[0].Value.ToString() == "Mailing Address State") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["DiffSateMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Mailing Address City") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["DiffCityMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Mailing Address HN") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["DiffHNMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Mailing Address Street") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["DiffStreetMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Mailing Address Phone") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["DiffPhoneMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Mailing Address Zip") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["DiffZipMode"] = "TRUE"; } }
                if (_dvRow.Cells[0].Value.ToString() == "Mailing Address ZipPlus") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["DiffZipPlusMode"] = "TRUE"; } }


                /*LIHEAP Questions*/
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0001") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0001Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0002") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0002Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0003") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0003Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0004") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0004Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0005") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0005Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0006") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0006Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0007") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0007Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0008") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0008Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0009") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0009Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0010") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0010Mode"] = "TRUE"; } } }
                if (_dvRow.Cells[0].Tag != null) { if (_dvRow.Cells[0].Tag.ToString() == "0011") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") { _drDSSXMLitem["MST_LPM_0011Mode"] = "TRUE"; } } }


                if (_dvRow.Cells[0].Value.ToString() == "Rent/Mortgage") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") _drDSSXMLitem["RENTMORTMode"] = "TRUE"; }
                if (_dvRow.Cells[0].Value.ToString() == "Relation") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") _drDSSXMLitem["RELATIONMode"] = "TRUE"; }
                if (_dvRow.Cells[0].Value.ToString() == "Disconnected Youth") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") _drDSSXMLitem["DISYOUTHMode"] = "TRUE"; }
                if (_dvRow.Cells[0].Value.ToString() == "Dwelling Type") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") _drDSSXMLitem["DWELLINGMode"] = "TRUE"; }
                if (_dvRow.Cells[0].Value.ToString() == "Primary Method of Paying for Heat") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") _drDSSXMLitem["PRIMPAYHEATMode"] = "TRUE"; }
                if (_dvRow.Cells[0].Value.ToString() == "Primary Source of Heat") { if (_dvRow.Cells["gvtSelChk"].Value.ToString().ToUpper() == "TRUE") _drDSSXMLitem["PRIMSRCHEARMode"] = "TRUE"; }


                _dvRow.Cells["gvtSelChk"].Value = _dvRow.Cells["gvtSelChk"].Value;
            }
        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            // gvwSubdetails.CellValueChanged -= gvwSubdetails_CellValueChanged;
            chkSelectAll.CheckedChanged -= new System.EventHandler(chkSelectAll_CheckedChanged);
            if (strFormType == "DSSXML")
            {
                List<DataGridViewRow> lstCust = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
                //lstCust.ForEach(x => x.Cells["gvtSelchk"].Value = false);

                DataRow _drDSSXMLitem = gvwCustomer.SelectedRows[0].Tag as DataRow;



                CheckItems(_drDSSXMLitem);
                if (chkDSSMode(_drDSSXMLitem))
                {
                    List<DataGridViewRow> checkedlst = gvwSubdetails.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtSelchk"].Value.ToString().ToUpper() == "TRUE").ToList();
                    if (lstCust.Count == checkedlst.Count)
                    {
                        lstCust.ForEach(x => x.Cells["gvtSelchk"].Value = false);
                        CheckItems(_drDSSXMLitem);
                    }

                }
                else
                {
                    if (!chkSelectAll.Checked)
                    {
                        lstCust.ForEach(x => x.Cells["gvtSelchk"].Value = false);
                    }
                }


                // else if (uncheckedlst.Count == 0)
                // chkSelectAll.Checked = true;


                //List<DataGridViewRow> uncheckedlst = gvwSubdetails.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtSelchk"].Value.ToString().ToUpper() == "FALSE").ToList();
                //if (uncheckedlst.Count > 0)
                //    chkSelectAll.Checked = false;
                if (chkSelectAll.Checked)
                {
                    lstCust.ForEach(x => x.Cells["gvtSelchk"].Value = true);
                    _drDSSXMLitem["SSNmode"] = "TRUE";
                    _drDSSXMLitem["FNAMEmode"] = "TRUE";
                    _drDSSXMLitem["MNAMEmode"] = "TRUE";
                    _drDSSXMLitem["LNAMEmode"] = "TRUE";
                    _drDSSXMLitem["DOBmode"] = "TRUE";
                    _drDSSXMLitem["GENDERmode"] = "TRUE";
                    _drDSSXMLitem["ETHENICmode"] = "TRUE";
                    _drDSSXMLitem["RACEmode"] = "TRUE";

                    _drDSSXMLitem["EDUCATIONmode"] = "TRUE";
                    _drDSSXMLitem["DISABLEDmode"] = "TRUE";
                    _drDSSXMLitem["MILTARYmode"] = "TRUE";
                    _drDSSXMLitem["WORKSTATUSmode"] = "TRUE";

                    _drDSSXMLitem["HOUSENOmode"] = "TRUE";
                    _drDSSXMLitem["STREETmode"] = "TRUE";
                    _drDSSXMLitem["ZIPmode"] = "TRUE";
                    _drDSSXMLitem["ZIPCODEmode"] = "TRUE";
                    _drDSSXMLitem["STATEmode"] = "TRUE";
                    _drDSSXMLitem["CITYmode"] = "TRUE";
                    _drDSSXMLitem["PHONEmode"] = "TRUE";
                    _drDSSXMLitem["CELLmode"] = "TRUE";
                    _drDSSXMLitem["LANGUAGEmode"] = "TRUE";
                    _drDSSXMLitem["HOUSINGmode"] = "TRUE";
                    _drDSSXMLitem["Emailmode"] = "TRUE";

                    /*Landlord Info*/
                    _drDSSXMLitem["LLRFnameMode"] = "TRUE";
                    _drDSSXMLitem["LLRLnameMode"] = "TRUE";
                    _drDSSXMLitem["LLRPhoneMode"] = "TRUE";
                    _drDSSXMLitem["LLRCityMode"] = "TRUE";
                    _drDSSXMLitem["LLRStateMode"] = "TRUE";
                    _drDSSXMLitem["LLRStreetMode"] = "TRUE";
                    _drDSSXMLitem["LLRHouseNoMode"] = "TRUE";
                    _drDSSXMLitem["LLRZipMode"] = "TRUE";
                    _drDSSXMLitem["LLRZipCodeMode"] = "TRUE";

                    /*CASE DIFF*/
                    _drDSSXMLitem["DiffSateMode"] = "TRUE";
                    _drDSSXMLitem["DiffCityMode"] = "TRUE";
                    _drDSSXMLitem["DiffHNMode"] = "TRUE";
                    _drDSSXMLitem["DiffStreetMode"] = "TRUE";
                    _drDSSXMLitem["DiffPhoneMode"] = "TRUE";
                    _drDSSXMLitem["DiffZipMode"] = "TRUE";
                    _drDSSXMLitem["DiffZipPlusMode"] = "TRUE";

                    _drDSSXMLitem["MST_LPM_0001Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0002Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0003Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0004Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0005Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0006Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0007Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0008Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0009Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0010Mode"] = "TRUE";
                    _drDSSXMLitem["MST_LPM_0011Mode"] = "TRUE";


                    _drDSSXMLitem["RENTMORTMode"] = "TRUE";
                    _drDSSXMLitem["RELATIONMode"] = "TRUE";
                    _drDSSXMLitem["DISYOUTHMode"] = "TRUE";
                    _drDSSXMLitem["DWELLINGMode"] = "TRUE";
                    _drDSSXMLitem["PRIMPAYHEATMode"] = "TRUE";
                    _drDSSXMLitem["PRIMSRCHEARMode"] = "TRUE";
                }


                List<DataGridViewRow> lstCustAddress = gvwSubdetails.Rows.Cast<DataGridViewRow>().Where(
                   x => x.Cells["gvtfield"].Value.ToString().Contains("Home: House#,Street,Suffix,Apt,Floor")
                   || x.Cells["gvtfield"].Value.ToString().Contains("      City Name,State,Zip") ||
                   x.Cells["gvtfield"].Value.ToString().Contains("Mail: House#,Street,Suffix,Apt,Floor")
                   ).ToList();

                lstCustAddress.ForEach(x => x.Cells["gvtSelchk"].Value = false);
            }
            else
            {
                string strlastvalue = string.Empty;
                int serviceindex = 0;
                if (chkSelectAll.Checked)
                {


                    LeanIntakeEntity leanentitydata = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;

                    if (leanentitydata != null)
                    {

                        foreach (DataGridViewRow gvitem in gvwSubdetails.Rows)
                        {

                            string strMType = gvitem.Cells["gvtActiveType"].Value == null ? string.Empty : gvitem.Cells["gvtActiveType"].Value.ToString();
                            if (strMType != string.Empty)
                            {
                                goto NewvalueEmpty;
                            }

                            string strFieldValue = gvitem.Cells["gvtfield"].Value.ToString();
                            if (strFieldValue.Contains("Service"))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Service";

                            }
                            if (strFieldValue.Contains("Health Ins "))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Health Ins";

                            }
                            if (strFieldValue.Contains("Non Cash Benefit"))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Non Cash Benefit";

                            }

                            if (strFormType != string.Empty)
                            {
                                if (strFieldValue.Contains("Income Type"))
                                {
                                    strlastvalue = strFieldValue.LastOrDefault().ToString();
                                    if (strlastvalue != string.Empty)
                                    {
                                        serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                    }
                                    strFieldValue = "Income Type";

                                }
                            }
                            gvitem.Cells["gvtSelchk"].Value = true; ;
                            switch (strFieldValue)
                            {
                                case "SSN":
                                    leanentitydata.PIP_SSN_MODE = "TRUE";
                                    if (gvitem.Cells["gvtNewValue"].Value.ToString().Substring(3, 2).ToString() == "00")
                                    {
                                        leanentitydata.PIP_SSN_MODE = "FALSE";
                                        CommonFunctions.MessageBoxDisplay("Most Recent ss# is a psuedo you should not replace a good ss# with a psuedo");
                                        gvitem.Cells["gvtSelchk"].Value = false;
                                    }
                                    break;
                                case "First Name":
                                    leanentitydata.PIP_FNAME_MODE = "TRUE";
                                    break;
                                case "MI":
                                    leanentitydata.PIP_MNAME_MODE = "TRUE";
                                    break;
                                case "Last Name":
                                    leanentitydata.PIP_LNAME_MODE = "TRUE";
                                    break;
                                case "Active":
                                    leanentitydata.PIP_STATUS_MODE = "TRUE";
                                    break;
                                case "Gender":
                                    leanentitydata.PIP_GENDER_MODE = "TRUE";
                                    break;
                                case "Dob":
                                    leanentitydata.PIP_DOB_MODE = "TRUE";
                                    break;

                                case "Marital Status":
                                    leanentitydata.PIP_MARITAL_STATUS_MODE = "TRUE";
                                    break;

                                case "Relation":

                                    leanentitydata.PIP_RELATIONSHIP_MODE = "TRUE";

                                    break;

                                case "Ethnicity":

                                    leanentitydata.PIP_ETHNIC_MODE = "TRUE";

                                    break;

                                case "Race":

                                    leanentitydata.PIP_RACE_MODE = "TRUE";

                                    break;

                                case "Education":

                                    leanentitydata.PIP_EDUCATION_MODE = "TRUE";

                                    break;

                                case "Disable":
                                    leanentitydata.PIP_DISABLE_MODE = "TRUE";

                                    break;

                                case "Work Status":
                                    leanentitydata.PIP_WORK_STAT_MODE = "TRUE";

                                    break;
                                case "Language":
                                    leanentitydata.PIP_PRI_LANGUAGE_MODE = "TRUE";
                                    break;

                                case "Family Type":

                                    leanentitydata.PIP_FAMILY_TYPE_MODE = "TRUE";

                                    break;

                                case "Housing":

                                    leanentitydata.PIP_HOUSING_MODE = "TRUE";

                                    break;

                                case "School":

                                    leanentitydata.PIP_SCHOOL_MODE = "TRUE";

                                    break;

                                case "Health Insurance":

                                    leanentitydata.PIP_HEALTH_INS_MODE = "TRUE";

                                    break;

                                case "Veteran":
                                    leanentitydata.PIP_VETERAN_MODE = "TRUE";

                                    break;

                                case "Food Stamp":

                                    leanentitydata.PIP_FOOD_STAMP_MODE = "TRUE";

                                    break;

                                case "Farmer":

                                    leanentitydata.PIP_FARMER_MODE = "TRUE";

                                    break;

                                case "WIC":

                                    leanentitydata.PIP_WIC_MODE = "TRUE";

                                    break;

                                case "Reliable Transport":

                                    leanentitydata.PIP_RELITRAN_MODE = "TRUE";

                                    break;

                                case "Drivers License":

                                    leanentitydata.PIP_DRVLIC_MODE = "TRUE";

                                    break;

                                case "Military Status":
                                    leanentitydata.PIP_MILITARY_STATUS_MODE = "TRUE";

                                    break;

                                case "Disconnected Youth":

                                    leanentitydata.PIP_YOUTH_MODE = "TRUE";

                                    break;

                                case "Are You Pregnant?":

                                    leanentitydata.PIP_PREGNANT_MODE = "TRUE";

                                    break;

                                case "Home Phone":
                                    leanentitydata.PIP_HOME_PHONE_MODE = "TRUE";

                                    break;

                                case "Cell Number":
                                    leanentitydata.PIP_CELL_NUMBER_MODE = "TRUE";

                                    break;

                                case "House No ":

                                    leanentitydata.PIP_HOUSENO_MODE = "TRUE";

                                    break;

                                case "Direction":

                                    leanentitydata.PIP_DIRECTION_MODE = "TRUE";

                                    break;

                                case "Street":

                                    leanentitydata.PIP_STREET_MODE = "TRUE";

                                    break;

                                case "Suffix":
                                    leanentitydata.PIP_SUFFIX_MODE = "TRUE";
                                    break;

                                case "Apartment":

                                    leanentitydata.PIP_APT_MODE = "TRUE";

                                    break;

                                case "Floor":
                                    leanentitydata.PIP_FLR_MODE = "TRUE";

                                    break;

                                case "City":

                                    leanentitydata.PIP_CITY_MODE = "TRUE";

                                    break;

                                case "State":

                                    leanentitydata.PIP_STATE_MODE = "TRUE";

                                    break;

                                case "Zip":
                                    leanentitydata.PIP_ZIP_MODE = "TRUE";

                                    break;
                                case "Township":

                                    leanentitydata.PIP_TOWNSHIP_MODE = "TRUE";

                                    break;
                                case "Precinct":

                                    leanentitydata.PIP_PRECINT_MODE = "TRUE";

                                    break;



                                case "County":

                                    leanentitydata.PIP_COUNTY_MODE = "TRUE";

                                    break;
                                case "Health Ins":

                                    leanentitydata.Pip_Healthcodes_list_Mode[serviceindex].Value = "TRUE";

                                    break;


                                case "Non Cash Benefit":
                                    leanentitydata.Pip_NonCashbenefit_list_Mode[serviceindex].Value = "TRUE";

                                    break;
                                case "Income Types":
                                    leanentitydata.PIP_INCOME_TYPES_MODE = "TRUE";

                                    break;
                                case "Income Type":
                                    leanentitydata.Pip_Income_list_Mode[serviceindex].Value = "TRUE";

                                    break;
                                case "Service":
                                    leanentitydata.Pip_service_list_Mode[serviceindex].Value = "TRUE";
                                    break;

                            }

                            leanentitydata.PIP_Valid_MODE = leanentitydata.PIP_Valid_MODE + 1;

                        NewvalueEmpty:
                            {
                                if (strMType != string.Empty)
                                {
                                    // gvwSubdetails.CellValueChanged -= gvwSubdetails_CellValueChanged;
                                    gvitem.Cells["gvtSelchk"].Value = false;
                                }
                            }

                        }

                        gvwCustomer.SelectedRows[0].Tag = leanentitydata;
                    }

                }
                else
                {
                    LeanIntakeEntity leanentitydata = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;

                    if (leanentitydata != null)
                    {
                        foreach (DataGridViewRow gvitem in gvwSubdetails.Rows)
                        {
                            string strFieldValue = gvitem.Cells["gvtfield"].Value.ToString();
                            if (strFieldValue.Contains("Service"))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Service";

                            }
                            if (strFieldValue.Contains("Non Cash Benefit"))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Non Cash Benefit";

                            }
                            if (strFieldValue.Contains("Health Ins "))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Health Ins";

                            }

                            if (strFormType != string.Empty)
                            {
                                if (strFieldValue.Contains("Income Type"))
                                {
                                    strlastvalue = strFieldValue.LastOrDefault().ToString();
                                    if (strlastvalue != string.Empty)
                                    {
                                        serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                    }
                                    strFieldValue = "Income Type";

                                }
                            }
                            gvitem.Cells["gvtSelchk"].Value = false;
                            switch (strFieldValue)
                            {
                                case "SSN":
                                    leanentitydata.PIP_SSN_MODE = "FALSE";
                                    break;
                                case "First Name":
                                    leanentitydata.PIP_FNAME_MODE = "FALSE";
                                    break;
                                case "MI":
                                    leanentitydata.PIP_MNAME_MODE = "FALSE";
                                    break;
                                case "Last Name":
                                    leanentitydata.PIP_LNAME_MODE = "FALSE";
                                    break;
                                case "Active":
                                    leanentitydata.PIP_STATUS_MODE = "TRUE";
                                    break;
                                case "Dob":
                                    leanentitydata.PIP_DOB_MODE = "FALSE";
                                    break;

                                case "Gender":
                                    leanentitydata.PIP_GENDER_MODE = "FALSE";
                                    break;

                                case "Marital Status":
                                    leanentitydata.PIP_MARITAL_STATUS_MODE = "FALSE";
                                    break;

                                case "Relation":
                                    leanentitydata.PIP_RELATIONSHIP_MODE = "FALSE";
                                    break;

                                case "Ethnicity":
                                    leanentitydata.PIP_ETHNIC_MODE = "FALSE";
                                    break;

                                case "Race":
                                    leanentitydata.PIP_RACE_MODE = "FALSE";
                                    break;

                                case "Education":
                                    leanentitydata.PIP_EDUCATION_MODE = "FALSE";
                                    break;

                                case "Disable":
                                    leanentitydata.PIP_DISABLE_MODE = "FALSE";
                                    break;

                                case "Work Status":
                                    leanentitydata.PIP_WORK_STAT_MODE = "FALSE";
                                    break;
                                case "Language":
                                    leanentitydata.PIP_PRI_LANGUAGE_MODE = "FALSE";
                                    break;

                                case "Family Type":
                                    leanentitydata.PIP_FAMILY_TYPE_MODE = "FALSE";
                                    break;

                                case "Housing":
                                    leanentitydata.PIP_HOUSING_MODE = "FALSE";
                                    break;

                                case "School":
                                    leanentitydata.PIP_SCHOOL_MODE = "FALSE";
                                    break;

                                case "Health Insurance":
                                    leanentitydata.PIP_HEALTH_INS_MODE = "FALSE";
                                    break;

                                case "Veteran":
                                    leanentitydata.PIP_VETERAN_MODE = "FALSE";
                                    break;

                                case "Food Stamp":
                                    leanentitydata.PIP_FOOD_STAMP_MODE = "FALSE";
                                    break;

                                case "Farmer":
                                    leanentitydata.PIP_FARMER_MODE = "FALSE";
                                    break;

                                case "WIC":
                                    leanentitydata.PIP_WIC_MODE = "FALSE";
                                    break;

                                case "Reliable Transport":
                                    leanentitydata.PIP_RELITRAN_MODE = "FALSE";
                                    break;

                                case "Drivers License":
                                    leanentitydata.PIP_DRVLIC_MODE = "FALSE";
                                    break;

                                case "Military Status":
                                    leanentitydata.PIP_MILITARY_STATUS_MODE = "FALSE";
                                    break;

                                case "Disconnected Youth":
                                    leanentitydata.PIP_YOUTH_MODE = "FALSE";
                                    break;

                                case "Are You Pregnant?":
                                    leanentitydata.PIP_PREGNANT_MODE = "FALSE";
                                    break;

                                case "Home Phone":
                                    leanentitydata.PIP_HOME_PHONE_MODE = "FALSE";
                                    break;

                                case "Cell Number":
                                    leanentitydata.PIP_CELL_NUMBER_MODE = "FALSE";
                                    break;

                                case "House No ":
                                    leanentitydata.PIP_HOUSENO_MODE = "FALSE";
                                    break;

                                case "Direction":
                                    leanentitydata.PIP_DIRECTION_MODE = "FALSE";
                                    break;

                                case "Street":
                                    leanentitydata.PIP_STREET_MODE = "FALSE";
                                    break;

                                case "Suffix":
                                    leanentitydata.PIP_SUFFIX_MODE = "FALSE";
                                    break;

                                case "Apartment":
                                    leanentitydata.PIP_APT_MODE = "FALSE";
                                    break;

                                case "Floor":
                                    leanentitydata.PIP_FLR_MODE = "FALSE";
                                    break;

                                case "City":
                                    leanentitydata.PIP_CITY_MODE = "FALSE";
                                    break;

                                case "State":
                                    leanentitydata.PIP_STATE_MODE = "FALSE";
                                    break;

                                case "Zip":
                                    leanentitydata.PIP_ZIP_MODE = "FALSE";
                                    break;
                                case "Township":
                                    leanentitydata.PIP_TOWNSHIP_MODE = "FALSE";
                                    break;
                                case "Precinct":
                                    leanentitydata.PIP_PRECINT_MODE = "FALSE";
                                    break;

                                case "County":
                                    leanentitydata.PIP_COUNTY_MODE = "FALSE";
                                    break;
                                case "Health Ins":
                                    leanentitydata.Pip_Healthcodes_list_Mode[serviceindex].Value = "FALSE";
                                    break;


                                case "Non Cash Benefit":
                                    leanentitydata.Pip_NonCashbenefit_list_Mode[serviceindex].Value = "FALSE";
                                    break;
                                case "Income Types":
                                    leanentitydata.PIP_INCOME_TYPES_MODE = "FALSE";
                                    break;
                                case "Income Type":
                                    leanentitydata.Pip_Income_list_Mode[serviceindex].Value = "FALSE";

                                    break;
                                case "Service":
                                    leanentitydata.Pip_service_list_Mode[serviceindex].Value = "FALSE";
                                    break;

                            }

                        }
                        if (leanentitydata.PIP_MEMBER_TYPE == string.Empty)
                            leanentitydata.PIP_Valid_MODE = 0;
                        gvwCustomer.SelectedRows[0].Tag = leanentitydata;
                    }

                }
            }
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
            // gvwSubdetails.CellValueChanged += gvwSubdetails_CellValueChanged;
        }

        private bool ValidAddress(LeanIntakeEntity dritem)
        {
            bool boolAddress = false;



            if (dritem.PIP_HOUSENO.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Hn.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }


            if (dritem.PIP_DIRECTION.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Direction.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }

            if (dritem.PIP_STREET.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Street.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }

            if (dritem.PIP_SUFFIX.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Suffix.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }


            if (dritem.PIP_APT.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Apt.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }


            if (dritem.PIP_FLR.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Flr.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }

            if (dritem.PIP_CITY.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].City.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }


            if (dritem.PIP_ZIP.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Zip.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }

            if (dritem.PIP_STATE.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].State.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }

            if (dritem.PIP_COUNTY.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].County.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }

            if (dritem.PIP_TOWNSHIP.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].TownShip.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }
            if (dritem.PIP_Precint.ToString().Trim().ToUpper() != BaseForm.BaseCaseMstListEntity[0].Precinct.ToString().Trim().ToUpper())
            {
                boolAddress = true;
            }

            return boolAddress;

        }

        private void CheckHistoryIntakeData(LeanIntakeEntity leanintakedata, CaseMstEntity CaseMST, CaseSnpEntity CaseSNP, bool IsApplicant)
        {
            string stroldvalue, strNewvalue = string.Empty;
            string strHistoryDetails = "<XmlHistory>";
            bool boolHistory = false;
            bool boolAddressHistory = false;
            //CaseMstEntity CaseMST = propCasemstEntity;// _model.CaseMstData.GetCaseMST(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

            //// List<CaseSnpEntity> _snpentitylist = _model.CaseMstData.GetCaseSnpadpyn(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

            //CaseSnpEntity CaseSNP;//= new CaseSnpEntity();
            //if (IsApplicant)
            //{
            //    CaseSNP = propCasesnpEntity.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);
            //}
            //else
            //{
            //    CaseSNP = propCasesnpEntity.Find(u => u.NameixFi.ToUpper() == strFirstName.ToUpper() && u.NameixLast.ToUpper() == strLastName.ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(strDob));

            //}



            if (CaseSNP != null)
            {


                if (CaseSNP.Ssno.Trim() != leanintakedata.PIP_SSN.Trim())
                {
                    if (leanintakedata.PIP_SSN_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SSN#</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseSNP.Ssno) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(leanintakedata.PIP_SSN) + "</NewValue></HistoryFields>";
                    }
                }



                if (CaseSNP.NameixFi.Trim().ToUpper() != leanintakedata.PIP_FNAME.Trim().ToUpper())
                {
                    if (leanintakedata.PIP_FNAME_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>First Name</FieldName><OldValue>" + CaseSNP.NameixFi + "</OldValue><NewValue>" + leanintakedata.PIP_FNAME + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseSNP.NameixMi.Trim().ToUpper() != leanintakedata.PIP_MNAME.Trim().ToUpper())
                {
                    if (leanintakedata.PIP_MNAME_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MI</FieldName><OldValue>" + CaseSNP.NameixMi + "</OldValue><NewValue>" + leanintakedata.PIP_MNAME + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseSNP.NameixLast.Trim().ToUpper() != leanintakedata.PIP_LNAME.Trim().ToUpper())
                {
                    if (leanintakedata.PIP_LNAME_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Last Name</FieldName><OldValue>" + CaseSNP.NameixLast + "</OldValue><NewValue>" + leanintakedata.PIP_LNAME + "</NewValue></HistoryFields>";
                    }
                }
                if (LookupDataAccess.Getdate(CaseSNP.AltBdate.Trim()) != LookupDataAccess.Getdate(leanintakedata.PIP_DOB.Trim()))
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>DOB</FieldName><OldValue>" + LookupDataAccess.Getdate(CaseSNP.AltBdate) + "</OldValue><NewValue>" + LookupDataAccess.Getdate(leanintakedata.PIP_DOB) + "</NewValue></HistoryFields>";
                }
                if (CaseSNP.Status.Trim().ToUpper() != leanintakedata.PIP_STATUS.Trim().ToUpper())
                {
                    if (leanintakedata.PIP_STATUS_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Active</FieldName><OldValue>" + (CaseSNP.Status == "A" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (leanintakedata.PIP_STATUS == "A" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseSNP.Sex.Trim() != leanintakedata.PIP_GENDER.Trim())
                {
                    if (leanintakedata.PIP_GENDER_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Sex.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_GENDER.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Gender</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }

                }

                if (leanintakedata.PIP_GENDER.Equals("F"))
                {
                    if (CaseSNP.Pregnant.Trim() != leanintakedata.PIP_PREGNANT.Trim())
                    {
                        if (leanintakedata.PIP_PREGNANT_MODE.ToUpper() == "TRUE")
                        {
                            boolHistory = true;
                            stroldvalue = strNewvalue = string.Empty;
                            List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.PREGNANT, "**", "**", "**", string.Empty);

                            CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Pregnant.ToString().Trim());
                            if (comOldvalue != null)
                            {
                                stroldvalue = comOldvalue.Desc;
                            }
                            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_PREGNANT.Trim());
                            if (comNewvalue != null)
                            {
                                strNewvalue = comNewvalue.Desc;
                            }
                            strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Are You Pregnant</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                        }
                    }
                }

                if (CaseSNP.MaritalStatus.Trim() != leanintakedata.PIP_MARITAL_STATUS.Trim())
                {
                    if (leanintakedata.PIP_MARITAL_STATUS_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MARITALSTATUS, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.MaritalStatus.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_MARITAL_STATUS.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Marital Status</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }


                if (CaseSNP.MemberCode.Trim() != leanintakedata.PIP_RELATIONSHIP.Trim())
                {
                    if (leanintakedata.PIP_RELATIONSHIP_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.MemberCode.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_RELATIONSHIP.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }

                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Relation</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseSNP.Ethnic.Trim() != leanintakedata.PIP_ETHNIC.Trim())
                {
                    if (leanintakedata.PIP_ETHNIC_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Ethnic.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_ETHNIC.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Ethnicity</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseSNP.Race.Trim() != leanintakedata.PIP_RACE.Trim())
                {
                    if (leanintakedata.PIP_RACE_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Race.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_RACE.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Race</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseSNP.Education.Trim() != leanintakedata.PIP_EDUCATION.Trim())
                {
                    if (leanintakedata.PIP_EDUCATION_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Education.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_EDUCATION.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Education</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseSNP.SchoolDistrict.Trim() != leanintakedata.PIP_SCHOOL.Trim())
                {
                    if (leanintakedata.PIP_SCHOOL_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SCHOOLDISTRICTS, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.SchoolDistrict.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_SCHOOL.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>School</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }


                if (CaseSNP.Relitran.Trim() != leanintakedata.PIP_RELITRAN.Trim())
                {
                    if (leanintakedata.PIP_RELITRAN_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELIABLETRANSPORTATION, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Relitran.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_RELITRAN.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Reliable Transport</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseSNP.Drvlic.Trim() != leanintakedata.PIP_DRVLIC.Trim())
                {
                    if (leanintakedata.PIP_DRVLIC_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DRIVERLICENSE, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Drvlic.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_DRVLIC.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Drivers License</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }

                if ((CaseSNP.HealthIns.Trim() != leanintakedata.PIP_HEALTH_INS.Trim()))
                {
                    if (leanintakedata.PIP_HEALTH_INS_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEALTHINSURANCE, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.HealthIns.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_HEALTH_INS.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Health Insurance</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (leanintakedata.PIP_HEALTH_CODES.Trim() != string.Empty)
                {
                    bool boolnoncash = false;

                    string strhealthcode = CaseSNP.Health_Codes;
                    foreach (ListItem itemNon in leanintakedata.Pip_Healthcodes_list_Mode)
                    {
                        if (itemNon.Value.ToString().ToUpper() == "TRUE")
                        {
                            boolnoncash = true;
                            if (strhealthcode.Trim() != string.Empty)
                                strhealthcode = strhealthcode + "," + itemNon.Text;
                            else
                                strhealthcode = itemNon.Text;
                        }
                    }

                    if (boolnoncash)
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Health Ins Code</FieldName><OldValue>" + CaseSNP.Health_Codes + "</OldValue><NewValue>" + strhealthcode + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseSNP.MilitaryStatus.Trim() != leanintakedata.PIP_MILITARY_STATUS.Trim())
                {
                    if (leanintakedata.PIP_MILITARY_STATUS_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MilitaryStatus, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.MilitaryStatus.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_MILITARY_STATUS.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Military Status</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseSNP.FootStamps.Trim() != leanintakedata.PIP_FOOD_STAMP.Trim())
                {
                    if (leanintakedata.PIP_FOOD_STAMP_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FOODSTAMPS, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.FootStamps.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_FOOD_STAMP.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Food Stamps</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseSNP.Disable.Trim() != leanintakedata.PIP_DISABLE.Trim())
                {
                    if (leanintakedata.PIP_DISABLE_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Disable.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_DISABLE.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Disabled</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }


                if (CaseSNP.Farmer.Trim() != leanintakedata.PIP_FARMER.Trim())
                {
                    if (leanintakedata.PIP_FARMER_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FARMER, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Farmer.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_FARMER.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Farmer</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseSNP.WorkStatus.Trim() != leanintakedata.PIP_WORK_STAT.Trim())
                {
                    if (leanintakedata.PIP_WORK_STAT_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.WorkStatus.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_WORK_STAT.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Work Status</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }


                if (CaseSNP.Wic.Trim() != leanintakedata.PIP_WIC.Trim())
                {
                    if (leanintakedata.PIP_WIC_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WIC, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Wic.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_WIC.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>WIC</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (leanintakedata.PIP_NCASHBEN.Trim() != string.Empty)
                {
                    bool boolnoncash = false;

                    string strNoncashbenefit = CaseSNP.NonCashBenefits;
                    foreach (ListItem itemNon in leanintakedata.Pip_NonCashbenefit_list_Mode)
                    {
                        if (itemNon.Value.ToString().ToUpper() == "TRUE")
                        {
                            boolnoncash = true;
                            if (strNoncashbenefit.Trim() != string.Empty)
                                strNoncashbenefit = strNoncashbenefit + "," + itemNon.Text;
                            else
                                strNoncashbenefit = itemNon.Text;
                        }
                    }

                    if (boolnoncash)
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Non-Cash Benefits</FieldName><OldValue>" + CaseSNP.NonCashBenefits + "</OldValue><NewValue>" + strNoncashbenefit + "</NewValue></HistoryFields>";
                    }
                }
                //if (CaseSNP.Student.Trim() != leanintakedata.PIP_Student.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_STUDENT</FieldName><OldValue>" + CaseSNP.Student + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.Resident.Trim() != leanintakedata.PIP_RE.Trim())
                //{
                //    boolHistory = true;
                //    stroldvalue = strNewvalue = string.Empty;
                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RESIDENTCODES, "**", "**", "**", string.Empty);

                //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Resident.ToString().Trim());
                //    if (comOldvalue != null)
                //    {
                //        stroldvalue = comOldvalue.Desc;
                //    }
                //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_Resident.Trim());
                //    if (comNewvalue != null)
                //    {
                //        strNewvalue = comNewvalue.Desc;
                //    }
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Resident</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                //}

                //if (CaseSNP.AlienRegNo.Trim() != leanintakedata.PIP_AlienRegNo.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Alien Reg No</FieldName><OldValue>" + CaseSNP.AlienRegNo + "</OldValue><NewValue>" + leanintakedata.PIP_AlienRegNo + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.LegalTowork.Trim() != leanintakedata.PIP_LegalTowork.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Legal to Work</FieldName><OldValue>" + CaseSNP.LegaltoworkDesc + "</OldValue><NewValue>" + leanintakedata.PIP_LegaltoworkDesc + "</NewValue></HistoryFields>";
                //}
                //if (LookupDataAccess.Getdate(CaseSNP.ExpireWorkDate.Trim()) != LookupDataAccess.Getdate(leanintakedata.PIP_ExpireWorkDate.Trim()))
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Expiration date</FieldName><OldValue>" + CaseSNP.ExpireWorkDate + "</OldValue><NewValue>" + leanintakedata.PIP_ExpireWorkDate + "</NewValue></HistoryFields>";
                //}





            }
            if (IsApplicant)
            {


                if (CaseMST.Hn.Trim() != leanintakedata.PIP_HOUSENO.Trim())
                {
                    if (leanintakedata.PIP_HOUSENO_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>HN</FieldName><OldValue>" + CaseMST.Hn + "</OldValue><NewValue>" + leanintakedata.PIP_HOUSENO + "</NewValue></HistoryFields>";
                    }
                }


                if (CaseMST.Street.Trim().ToUpper() != leanintakedata.PIP_STREET.Trim().ToUpper())
                {
                    if (leanintakedata.PIP_STREET_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Street</FieldName><OldValue>" + CaseMST.Street + "</OldValue><NewValue>" + leanintakedata.PIP_STREET + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseMST.Suffix.Trim().ToUpper() != leanintakedata.PIP_SUFFIX.Trim().ToUpper())
                {

                    if (leanintakedata.PIP_SUFFIX_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Suffix</FieldName><OldValue>" + CaseMST.Suffix + "</OldValue><NewValue>" + leanintakedata.PIP_SUFFIX + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseMST.Direction.Trim() != leanintakedata.PIP_DIRECTION.Trim())
                {
                    if (leanintakedata.PIP_DIRECTION_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Direction</FieldName><OldValue>" + CaseMST.Direction + "</OldValue><NewValue>" + leanintakedata.PIP_DIRECTION + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseMST.Apt.Trim().ToUpper() != leanintakedata.PIP_APT.Trim().ToUpper())
                {
                    if (leanintakedata.PIP_APT_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Apartment</FieldName><OldValue>" + CaseMST.Apt + "</OldValue><NewValue>" + leanintakedata.PIP_APT + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseMST.Flr.Trim() != leanintakedata.PIP_FLR.Trim())
                {
                    if (leanintakedata.PIP_FLR_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Floor</FieldName><OldValue>" + CaseMST.Flr + "</OldValue><NewValue>" + leanintakedata.PIP_FLR + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseMST.City.ToUpper().Trim() != leanintakedata.PIP_CITY.ToUpper().Trim())
                {
                    if (leanintakedata.PIP_CITY_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>City Name</FieldName><OldValue>" + CaseMST.City + "</OldValue><NewValue>" + leanintakedata.PIP_CITY + "</NewValue></HistoryFields>";
                    }
                }
                if ("00000".Substring(0, 5 - CaseMST.Zip.Length) + CaseMST.Zip != "00000".Substring(0, 5 - leanintakedata.PIP_ZIP.Length) + leanintakedata.PIP_ZIP.Trim())
                {
                    if (leanintakedata.PIP_ZIP_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Zip Code</FieldName><OldValue>" + CaseMST.Zip + "</OldValue><NewValue>" + leanintakedata.PIP_ZIP + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseMST.State.ToUpper().Trim() != leanintakedata.PIP_STATE.ToUpper().Trim())
                {
                    if (leanintakedata.PIP_STATE_MODE.ToUpper() == "TRUE")
                    {
                        boolAddressHistory = boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>State</FieldName><OldValue>" + CaseMST.State + "</OldValue><NewValue>" + leanintakedata.PIP_STATE + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseMST.County.Trim() != leanintakedata.PIP_COUNTY.Trim())
                {
                    if (leanintakedata.PIP_COUNTY_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.County.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_COUNTY.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }

                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>County</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseMST.TownShip.Trim() != leanintakedata.PIP_TOWNSHIP.Trim())
                {
                    if (leanintakedata.PIP_TOWNSHIP_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.TownShip.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_TOWNSHIP.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }

                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Township</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseMST.Precinct.Trim() != leanintakedata.PIP_Precint.Trim())
                {
                    if (leanintakedata.PIP_PRECINT_MODE.ToUpper() == "TRUE")
                    {

                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Precinct</FieldName><OldValue>" + CaseMST.Precinct.Trim() + "</OldValue><NewValue>" + leanintakedata.PIP_Precint.Trim() + "</NewValue></HistoryFields>";
                    }
                }


                if (CaseMST.Language.Trim() != leanintakedata.PIP_PRI_LANGUAGE.Trim())
                {
                    if (leanintakedata.PIP_PRI_LANGUAGE_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.Language.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_PRI_LANGUAGE.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }

                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Language</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if (CaseMST.FamilyType.Trim() != leanintakedata.PIP_FAMILY_TYPE.Trim())
                {
                    if (leanintakedata.PIP_FAMILY_TYPE_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.FamilyType.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_FAMILY_TYPE.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }

                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Family Type</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }
                if ((CaseMST.Area.Trim() + CaseMST.Phone.Trim()) != (leanintakedata.PIP_AREA.Trim() + leanintakedata.PIP_HOME_PHONE.Trim()))
                {
                    if (leanintakedata.PIP_AREA_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Home Phone</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.Area + CaseMST.Phone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(leanintakedata.PIP_AREA + leanintakedata.PIP_HOME_PHONE) + "</NewValue></HistoryFields>";
                    }
                }
                if ((CaseMST.CellPhone) != (leanintakedata.PIP_CELL_NUMBER.Trim()))
                {
                    if (leanintakedata.PIP_CELL_NUMBER_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Cell Phone</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.CellPhone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(leanintakedata.PIP_CELL_NUMBER) + "</NewValue></HistoryFields>";
                    }
                }


                if (CaseMST.Housing.Trim() != leanintakedata.PIP_HOUSING.Trim())
                {
                    if (leanintakedata.PIP_HOUSING_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.Housing.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == leanintakedata.PIP_HOUSING.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }

                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Housing Situation</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }




                //if (CaseMST.MessagePhone_NA.Trim() != leanintakedata.PIP_MessagePhone_NA.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Message NA</FieldName><OldValue>" + (CaseMST.MessagePhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (leanintakedata.PIP_MessagePhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.MessagePhone.Trim() != leanintakedata.PIP_MessagePhone.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Message</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.MessagePhone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(leanintakedata.PIP_MessagePhone) + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.CellPhone_NA.Trim() != leanintakedata.PIP_CellPhone_NA.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Cell NA</FieldName><OldValue>" + (CaseMST.CellPhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (leanintakedata.PIP_CellPhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.CellPhone.Trim() != leanintakedata.PIP_CellPhone.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Cell</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.CellPhone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(leanintakedata.PIP_CellPhone) + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.FaxNumber.Trim() != leanintakedata.PIP_FaxNumber.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Fax</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.FaxNumber) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(leanintakedata.PIP_FaxNumber) + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.TtyNumber.Trim() != leanintakedata.PIP_TtyNumber.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>TTY</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.TtyNumber) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(leanintakedata.PIP_TtyNumber) + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Email_NA.Trim() != leanintakedata.PIP_Email_NA.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Email NA</FieldName><OldValue>" + (CaseMST.Email_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (leanintakedata.PIP_Email_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.Email.Trim() != leanintakedata.PIP_EMAIL.Trim())
                {
                    if (leanintakedata.PIP_EMAIL_MODE.ToUpper() == "TRUE")
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Email</FieldName><OldValue>" + CaseMST.Email + "</OldValue><NewValue>" + leanintakedata.PIP_EMAIL + "</NewValue></HistoryFields>";
                    }
                }
                //if (CaseMST.BestContact.Trim() != leanintakedata.PIP_BestContact.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>What is the best way to contact you</FieldName><OldValue>" + CaseMST.ContactusDesc + "</OldValue><NewValue>" + leanintakedata.PIP_ContactusDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.AboutUs.Trim() != leanintakedata.PIP_AboutUs.Trim())
                //{
                //    boolHistory = true;
                //    stroldvalue = strNewvalue = string.Empty;
                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOWDIDYOUHEARABOUTTHEPROGRAM, "**", "**", "**", string.Empty);

                //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.Housing.ToString().Trim());
                //    if (comOldvalue != null)
                //    {
                //        stroldvalue = comOldvalue.Desc;
                //    }
                //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.Housing.Trim());
                //    if (comNewvalue != null)
                //    {
                //        strNewvalue = comNewvalue.Desc;
                //    }

                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>How did you find out about us?</FieldName><OldValue>" + CaseMST.AboutUsDesc + "</OldValue><NewValue>" + caseHistMst.AboutUsDesc + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.ExpRent.Trim() != caseHistMst.ExpRent.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Rent/Mortgage</FieldName><OldValue>" + CaseMST.ExpRent + "</OldValue><NewValue>" + caseHistMst.ExpRent + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ExpWater.Trim() != caseHistMst.ExpWater.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Water/Sewer</FieldName><OldValue>" + CaseMST.ExpWater + "</OldValue><NewValue>" + caseHistMst.ExpWater + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ExpElectric.Trim() != caseHistMst.ExpElectric.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Electric</FieldName><OldValue>" + CaseMST.ExpElectric + "</OldValue><NewValue>" + caseHistMst.ExpElectric + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ExpHeat.Trim() != caseHistMst.ExpHeat.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Heating</FieldName><OldValue>" + CaseMST.ExpHeat + "</OldValue><NewValue>" + caseHistMst.ExpHeat + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.ExpMisc.Trim() != caseHistMst.ExpMisc.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Misc </FieldName><OldValue>" + CaseMST.ExpMisc + "</OldValue><NewValue>" + caseHistMst.ExpMisc + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.Dwelling.Trim() != caseHistMst.Dwelling.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Dwelling Type</FieldName><OldValue>" + CaseMST.DwellingDesc + "</OldValue><NewValue>" + caseHistMst.DwellingDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.HeatIncRent.Trim() != caseHistMst.HeatIncRent.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Method of Paying for Heat</FieldName><OldValue>" + CaseMST.HeatIncRentDesc + "</OldValue><NewValue>" + caseHistMst.HeatIncRentDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Source.Trim() != caseHistMst.Source.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Source of Heat </FieldName><OldValue>" + CaseMST.SourceDesc + "</OldValue><NewValue>" + caseHistMst.SourceDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SubShouse.Trim() != caseHistMst.SubShouse.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Subsidized Housing</FieldName><OldValue>" + CaseMST.SubShouse + "</OldValue><NewValue>" + caseHistMst.SubShouse + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SubStype.Trim() != caseHistMst.SubStype.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Subsidized Housing Type</FieldName><OldValue>" + CaseMST.SubsTypeDesc + "</OldValue><NewValue>" + caseHistMst.SubsTypeDesc + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.ExpCaseWorker.Trim() != caseHistMst.ExpCaseWorker.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Verified that all Household Expenses entered</FieldName><OldValue>" + CaseMST.ExpCaseworkerDesc + "</OldValue><NewValue>" + caseHistMst.ExpCaseworkerDesc + "</NewValue></HistoryFields>";
                //}



            }
            strHistoryDetails = strHistoryDetails + "</XmlHistory>";
            if (boolHistory)
            {
                CaseHistEntity caseHistEntity = new CaseHistEntity();
                caseHistEntity.HistTblName = "CASEMST";
                caseHistEntity.HistScreen = "CASE2001";
                caseHistEntity.HistSubScr = "Intake UpdateForm";
                caseHistEntity.HistTblKey = CaseSNP.Agency + CaseSNP.Dept + CaseSNP.Program + (CaseSNP.Year == string.Empty ? "    " : CaseSNP.Year) + CaseSNP.App + CaseSNP.FamilySeq;
                caseHistEntity.LstcOperator = BaseForm.UserID;
                caseHistEntity.HistChanges = strHistoryDetails;
                _model.CaseMstData.InsertCaseHist(caseHistEntity);

                //if (boolAddressHistory)
                //{
                //    CaseMST.Mode = "Address";
                //    CaseMST.LstcOperator4 = BaseForm.UserID;
                //    _model.CaseMstData.InsertUpdateCaseMst(CaseMST, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg);
                //}


            }
        }

        private void CheckHistoryTableData(CaseMstEntity caseHistMst, CaseSnpEntity casesnpHist, CaseMstEntity CaseMST, CaseSnpEntity CaseSNP, bool IsApplicant)
        {
            string stroldvalue, strNewvalue = string.Empty;
            string strHistoryDetails = "<XmlHistory>";
            bool boolHistory = false;
            bool boolAddressHistory = false;
            //CaseMstEntity CaseMST = propCasemstEntity;// _model.CaseMstData.GetCaseMST(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

            //// List<CaseSnpEntity> _snpentitylist = _model.CaseMstData.GetCaseSnpadpyn(propCasemstEntity.ApplAgency, propCasemstEntity.ApplDept, propCasemstEntity.ApplProgram, propCasemstEntity.ApplYr, propCasemstEntity.ApplNo);

            //CaseSnpEntity CaseSNP;//= new CaseSnpEntity();
            //if (IsApplicant)
            //{
            //    CaseSNP = propCasesnpEntity.Find(u => u.FamilySeq == propCasemstEntity.FamilySeq);
            //}
            //else
            //{
            //    CaseSNP = propCasesnpEntity.Find(u => u.NameixFi.ToUpper() == strFirstName.ToUpper() && u.NameixLast.ToUpper() == strLastName.ToUpper() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(strDob));

            //}



            if (CaseSNP != null)
            {


                if (CaseSNP.Ssno.Trim() != casesnpHist.Ssno.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SSN#</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseSNP.Ssno) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(casesnpHist.Ssno) + "</NewValue></HistoryFields>";
                }

                //if (CaseSNP.SsBic.Trim() != casesnpHist.SsBic.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Bic</FieldName><OldValue>" + CaseSNP.SsBic + "</OldValue><NewValue>" + casesnpHist.SsBic + "</NewValue></HistoryFields>";
                //}

                if (CaseSNP.NameixFi.Trim().ToUpper() != casesnpHist.NameixFi.Trim().ToUpper())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>First Name</FieldName><OldValue>" + CaseSNP.NameixFi + "</OldValue><NewValue>" + casesnpHist.NameixFi + "</NewValue></HistoryFields>";
                }
                if (CaseSNP.NameixMi.Trim().ToUpper() != casesnpHist.NameixMi.Trim().ToUpper())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MI</FieldName><OldValue>" + CaseSNP.NameixMi + "</OldValue><NewValue>" + casesnpHist.NameixMi + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.NameixLast.Trim().ToUpper() != casesnpHist.NameixLast.Trim().ToUpper())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Last Name</FieldName><OldValue>" + CaseSNP.NameixLast + "</OldValue><NewValue>" + casesnpHist.NameixLast + "</NewValue></HistoryFields>";
                }
                if (LookupDataAccess.Getdate(CaseSNP.AltBdate.Trim()) != LookupDataAccess.Getdate(casesnpHist.AltBdate.Trim()))
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>DOB</FieldName><OldValue>" + LookupDataAccess.Getdate(CaseSNP.AltBdate) + "</OldValue><NewValue>" + LookupDataAccess.Getdate(casesnpHist.AltBdate) + "</NewValue></HistoryFields>";
                }
                //if (CaseSNP.Alias.Trim() != casesnpHist.Alias.Trim())
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Alias</FieldName><OldValue>" + CaseSNP.Alias + "</OldValue><NewValue>" + casesnpHist.Alias + "</NewValue></HistoryFields>";
                if (CaseSNP.Status.Trim() != casesnpHist.Status.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Active</FieldName><OldValue>" + (CaseSNP.Status == "A" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (casesnpHist.Status == "A" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                if (CaseSNP.Sex.Trim() != casesnpHist.Sex.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Sex.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Sex.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Gender</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (casesnpHist.Sex.Equals("F"))
                {
                    if (CaseSNP.Pregnant.Trim() != casesnpHist.Pregnant.Trim())
                    {
                        boolHistory = true;
                        stroldvalue = strNewvalue = string.Empty;
                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.PREGNANT, "**", "**", "**", string.Empty);

                        CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Pregnant.ToString().Trim());
                        if (comOldvalue != null)
                        {
                            stroldvalue = comOldvalue.Desc;
                        }
                        CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Pregnant.Trim());
                        if (comNewvalue != null)
                        {
                            strNewvalue = comNewvalue.Desc;
                        }
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Are You Pregnant</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                    }
                }

                if (CaseSNP.MaritalStatus.Trim() != casesnpHist.MaritalStatus.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MARITALSTATUS, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.MaritalStatus.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.MaritalStatus.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Marital Status</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }



                if (CaseSNP.MemberCode.Trim() != casesnpHist.MemberCode.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.MemberCode.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.MemberCode.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }

                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Relation</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }


                if (CaseSNP.Ethnic.Trim() != casesnpHist.Ethnic.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Ethnic.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Ethnic.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Ethnicity</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.Race.Trim() != casesnpHist.Race.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Race.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Race.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Race</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.Education.Trim() != casesnpHist.Education.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Education.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Education.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Education</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                if (CaseSNP.SchoolDistrict.Trim() != casesnpHist.SchoolDistrict.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SCHOOLDISTRICTS, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.SchoolDistrict.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.SchoolDistrict.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>School</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.Relitran.Trim() != casesnpHist.Relitran.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELIABLETRANSPORTATION, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Relitran.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Relitran.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Reliable Transport</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.Drvlic.Trim() != casesnpHist.Drvlic.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DRIVERLICENSE, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Drvlic.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Drvlic.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Drivers License</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }


                if ((CaseSNP.HealthIns.Trim() != casesnpHist.HealthIns.Trim()))
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEALTHINSURANCE, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.HealthIns.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.HealthIns.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Health Insurance</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                if ((CaseSNP.Health_Codes.Trim() != casesnpHist.Health_Codes.Trim()))
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Health Ins Code</FieldName><OldValue>" + CaseSNP.Health_Codes + "</OldValue><NewValue>" + casesnpHist.Health_Codes + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.MilitaryStatus.Trim() != casesnpHist.MilitaryStatus.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MilitaryStatus, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.MilitaryStatus.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.MilitaryStatus.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Military Status</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }



                if (CaseSNP.FootStamps.Trim() != casesnpHist.FootStamps.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FOODSTAMPS, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.FootStamps.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.FootStamps.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Food Stamps</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                if (CaseSNP.Disable.Trim() != casesnpHist.Disable.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Disable.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Disable.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Disabled</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                if (CaseSNP.Farmer.Trim() != casesnpHist.Farmer.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FARMER, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Farmer.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Farmer.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Farmer</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                if (CaseSNP.WorkStatus.Trim() != casesnpHist.WorkStatus.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.WorkStatus.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.WorkStatus.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Work Status</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.Wic.Trim() != casesnpHist.Wic.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WIC, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Wic.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Wic.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>WIC</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (CaseSNP.NonCashBenefits.Trim() != casesnpHist.NonCashBenefits.Trim())
                {

                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Non-Cash Benefits</FieldName><OldValue>" + CaseSNP.NonCashBenefits + "</OldValue><NewValue>" + casesnpHist.NonCashBenefits + "</NewValue></HistoryFields>";
                }
                //if (CaseSNP.Youth.Trim() != casesnpHist.Youth.Trim())
                //{
                //    boolHistory = true;
                //    stroldvalue = strNewvalue = string.Empty;
                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DisconnectedYouth, "**", "**", "**", string.Empty);

                //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Youth.ToString().Trim());
                //    if (comOldvalue != null)
                //    {
                //        stroldvalue = comOldvalue.Desc;
                //    }
                //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Youth.Trim());
                //    if (comNewvalue != null)
                //    {
                //        strNewvalue = comNewvalue.Desc;
                //    }
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Disconnected Youth</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                //}



                //if (CaseSNP.Student.Trim() != casesnpHist.Student.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_STUDENT</FieldName><OldValue>" + CaseSNP.Student + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.Resident.Trim() != casesnpHist.Resident.Trim())
                //{
                //    boolHistory = true;
                //    stroldvalue = strNewvalue = string.Empty;
                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RESIDENTCODES, "**", "**", "**", string.Empty);

                //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseSNP.Resident.ToString().Trim());
                //    if (comOldvalue != null)
                //    {
                //        stroldvalue = comOldvalue.Desc;
                //    }
                //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == casesnpHist.Resident.Trim());
                //    if (comNewvalue != null)
                //    {
                //        strNewvalue = comNewvalue.Desc;
                //    }
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Resident</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                //}




                //if (CaseSNP.AlienRegNo.Trim() != casesnpHist.AlienRegNo.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Alien Reg No</FieldName><OldValue>" + CaseSNP.AlienRegNo + "</OldValue><NewValue>" + casesnpHist.AlienRegNo + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.LegalTowork.Trim() != casesnpHist.LegalTowork.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Legal to Work</FieldName><OldValue>" + CaseSNP.LegaltoworkDesc + "</OldValue><NewValue>" + casesnpHist.LegaltoworkDesc + "</NewValue></HistoryFields>";
                //}
                //if (LookupDataAccess.Getdate(CaseSNP.ExpireWorkDate.Trim()) != LookupDataAccess.Getdate(casesnpHist.ExpireWorkDate.Trim()))
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Expiration date</FieldName><OldValue>" + CaseSNP.ExpireWorkDate + "</OldValue><NewValue>" + casesnpHist.ExpireWorkDate + "</NewValue></HistoryFields>";
                //}

                //if (CaseSNP.Employed.Trim() != casesnpHist.Employed.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Presently employed</FieldName><OldValue>" + CaseSNP.PresentEmployDesc + "</OldValue><NewValue>" + casesnpHist.PresentEmployDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.LastWorkDate.Trim() != casesnpHist.LastWorkDate.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Last Date Worked</FieldName><OldValue>" + CaseSNP.LastWorkDate + "</OldValue><NewValue>" + casesnpHist.LastWorkDate + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.WorkLimit.Trim() != casesnpHist.WorkLimit.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Any work limitations</FieldName><OldValue>" + CaseSNP.AnyworklimitationDesc + "</OldValue><NewValue>" + casesnpHist.AnyworklimitationDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.ExplainWorkLimit.Trim() != casesnpHist.ExplainWorkLimit.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>If yes, explain</FieldName><OldValue>" + CaseSNP.ExplainWorkLimit + "</OldValue><NewValue>" + casesnpHist.ExplainWorkLimit + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.NumberOfcjobs.Trim() != casesnpHist.NumberOfcjobs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName># of jobs that you currently have?</FieldName><OldValue>" + CaseSNP.NumberOfcjobs + "</OldValue><NewValue>" + casesnpHist.NumberOfcjobs + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.NumberofLvjobs.Trim() != casesnpHist.NumberofLvjobs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName># of jobs since your last visit?</FieldName><OldValue>" + CaseSNP.NumberofLvjobs + "</OldValue><NewValue>" + casesnpHist.NumberofLvjobs + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.FullTimeHours.Trim() != casesnpHist.FullTimeHours.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Full Time Hours(per week)</FieldName><OldValue>" + CaseSNP.FullTimeHours + "</OldValue><NewValue>" + casesnpHist.FullTimeHours + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.PartTimeHours.Trim() != casesnpHist.PartTimeHours.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Part Time Hours(per week)</FieldName><OldValue>" + CaseSNP.PartTimeHours + "</OldValue><NewValue>" + casesnpHist.PartTimeHours + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.SeasonalEmploy.Trim() != casesnpHist.SeasonalEmploy.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Seasonal Employment Only</FieldName><OldValue>" + CaseSNP.SeasonEmployedDesc + "</OldValue><NewValue>" + casesnpHist.SeasonEmployedDesc + "</NewValue></HistoryFields>";
                //}
                //if ((CaseSNP.IstShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.IstShift.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>1st</FieldName><OldValue>" + CaseSNP.IstShift + "</OldValue><NewValue>" + casesnpHist.IstShift + "</NewValue></HistoryFields>";
                //}
                //if ((CaseSNP.IIndShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.IIndShift.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>2nd</FieldName><OldValue>" + CaseSNP.IIndShift + "</OldValue><NewValue>" + casesnpHist.IIndShift + "</NewValue></HistoryFields>";
                //}

                //if ((CaseSNP.IIIrdShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.IIIrdShift.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>3rd</FieldName><OldValue>" + CaseSNP.IIIrdShift + "</OldValue><NewValue>" + casesnpHist.IIIrdShift + "</NewValue></HistoryFields>";
                //}
                //if ((CaseSNP.RShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.RShift.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Rotating</FieldName><OldValue>" + CaseSNP.RShift + "</OldValue><NewValue>" + casesnpHist.RShift + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.EmployerName.Trim() != casesnpHist.EmployerName.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Employer</FieldName><OldValue>" + CaseSNP.EmployerName + "</OldValue><NewValue>" + casesnpHist.EmployerName + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.EmployerStreet.Trim() != casesnpHist.EmployerStreet.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Street</FieldName><OldValue>" + CaseSNP.EmployerStreet + "</OldValue><NewValue>" + casesnpHist.EmployerStreet + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.EmployerCity.Trim() != casesnpHist.EmployerCity.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>City/State/Zip</FieldName><OldValue>" + CaseSNP.EmployerCity + "</OldValue><NewValue>" + casesnpHist.EmployerCity + "</NewValue></HistoryFields>";
                //}
                //if ((CaseSNP.JobTitle.Trim() != casesnpHist.JobTitle.Trim()) && (CaseSNP.JobTitle.Trim() != "0"))
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Job Title</FieldName><OldValue>" + CaseSNP.JobTitle + "</OldValue><NewValue>" + casesnpHist.JobTitle + "</NewValue></HistoryFields>";
                //}
                //if ((CaseSNP.JobCategory.Trim() != casesnpHist.JobCategory.Trim()) && (CaseSNP.JobCategory.Trim() != "0"))
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Job Category</FieldName><OldValue>" + CaseSNP.JobCategory + "</OldValue><NewValue>" + casesnpHist.JobCategory + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.HourlyWage.Trim() != casesnpHist.HourlyWage.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Hourly Wage $</FieldName><OldValue>" + CaseSNP.HourlyWage + "</OldValue><NewValue>" + casesnpHist.HourlyWage + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.PayFrequency.Trim() != casesnpHist.PayFrequency.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Pay Frequency</FieldName><OldValue>" + CaseSNP.PayFrequency + "</OldValue><NewValue>" + casesnpHist.PayFrequency + "</NewValue></HistoryFields>";
                //}
                //if (CaseSNP.HireDate.Trim() != casesnpHist.HireDate.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Hire Date</FieldName><OldValue>" + CaseSNP.HireDate + "</OldValue><NewValue>" + casesnpHist.HireDate + "</NewValue></HistoryFields>";
                //}



                //if (CaseSNP.EmplPhone.Trim() != casesnpHist.EmplPhone.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Phone</FieldName><OldValue>" + CaseSNP.EmplPhone + "</OldValue><NewValue>" + casesnpHist.EmplPhone + "</NewValue></HistoryFields>";
                //}

                //if (CaseSNP.EmplExt.Trim() != casesnpHist.EmplExt.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>EXT</FieldName><OldValue>" + CaseSNP.EmplExt + "</OldValue><NewValue>" + casesnpHist.EmplExt + "</NewValue></HistoryFields>";
                //}

                //if (CaseSNP.Exclude.Trim() != casesnpHist.Exclude.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Exclude Member</FieldName><OldValue>" + (CaseSNP.Exclude == "Y" ? "Yes" : "NO") + "</OldValue><NewValue>" + (casesnpHist.Exclude == "Y" ? "Yes" : "NO") + "</NewValue></HistoryFields>";
                //}
            }
            if (IsApplicant)
            {
                if (CaseMST.Hn.Trim() != caseHistMst.Hn.Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>HN</FieldName><OldValue>" + CaseMST.Hn + "</OldValue><NewValue>" + caseHistMst.Hn + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Direction.Trim() != caseHistMst.Direction.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Direction</FieldName><OldValue>" + CaseMST.Direction + "</OldValue><NewValue>" + caseHistMst.Direction + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Street.Trim().ToUpper() != caseHistMst.Street.Trim().ToUpper())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Street</FieldName><OldValue>" + CaseMST.Street + "</OldValue><NewValue>" + caseHistMst.Street + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Suffix.Trim().ToUpper() != caseHistMst.Suffix.Trim().ToUpper())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Suffix</FieldName><OldValue>" + CaseMST.Suffix + "</OldValue><NewValue>" + caseHistMst.Suffix + "</NewValue></HistoryFields>";
                }

                if (CaseMST.Apt.Trim().ToUpper() != caseHistMst.Apt.Trim().ToUpper())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Apartment</FieldName><OldValue>" + CaseMST.Apt + "</OldValue><NewValue>" + caseHistMst.Apt + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Flr.Trim() != caseHistMst.Flr.Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Floor</FieldName><OldValue>" + CaseMST.Flr + "</OldValue><NewValue>" + caseHistMst.Flr + "</NewValue></HistoryFields>";
                }
                if (CaseMST.City.ToUpper().Trim() != caseHistMst.City.ToUpper().Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>City Name</FieldName><OldValue>" + CaseMST.City + "</OldValue><NewValue>" + caseHistMst.City + "</NewValue></HistoryFields>";
                }
                if ("00000".Substring(0, 5 - CaseMST.Zip.Length) + CaseMST.Zip != "00000".Substring(0, 5 - caseHistMst.Zip.Length) + caseHistMst.Zip.Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Zip Code</FieldName><OldValue>" + CaseMST.Zip + "</OldValue><NewValue>" + caseHistMst.Zip + "</NewValue></HistoryFields>";
                }
                //if ("0000".Substring(0, 4 - CaseMST.Zipplus.Length) + CaseMST.Zipplus != "0000".Substring(0, 4 - caseHistMst.Zipplus.Length) + caseHistMst.Zipplus.Trim())
                //{
                //    boolAddressHistory = boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Zip Plus</FieldName><OldValue>" + CaseMST.Zipplus + "</OldValue><NewValue>" + caseHistMst.Zipplus + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.State.ToUpper().Trim() != caseHistMst.State.ToUpper().Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>State</FieldName><OldValue>" + CaseMST.State + "</OldValue><NewValue>" + caseHistMst.State + "</NewValue></HistoryFields>";
                }

                if (CaseMST.TownShip.Trim() != caseHistMst.TownShip.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.TownShip.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.TownShip.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }

                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Township</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                if (CaseMST.County.Trim() != caseHistMst.County.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.County.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.County.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }

                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>County</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                if (strFormType != string.Empty)
                    if (CaseMST.Precinct.Trim() != caseHistMst.Precinct.Trim())
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Precinct</FieldName><OldValue>" + CaseMST.Precinct + "</OldValue><NewValue>" + caseHistMst.Precinct + "</NewValue></HistoryFields>";
                    }

                if (CaseMST.Language.Trim() != caseHistMst.Language.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.Language.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.Language.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }

                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Language</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                if (CaseMST.FamilyType.Trim() != caseHistMst.FamilyType.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.FamilyType.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.FamilyType.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }

                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Family Type</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }

                //if (CaseMST.LanguageOt.Trim() != caseHistMst.LanguageOt.Trim())
                //{
                //    boolHistory = true;
                //    stroldvalue = strNewvalue = string.Empty;
                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, "**", "**", "**", string.Empty);

                //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.LanguageOt.ToString().Trim());
                //    if (comOldvalue != null)
                //    {
                //        stroldvalue = comOldvalue.Desc;
                //    }
                //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.LanguageOt.Trim());
                //    if (comNewvalue != null)
                //    {
                //        strNewvalue = comNewvalue.Desc;
                //    }

                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Second Language</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.IntakeWorker.Trim() != caseHistMst.IntakeWorker.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Assigned Worker</FieldName><OldValue>" + CaseMST.AssignWorkerDesc + "</OldValue><NewValue>" + caseHistMst.AssignWorkerDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.IntakeDate.Trim() != caseHistMst.IntakeDate.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Intake Date</FieldName><OldValue>" + CaseMST.IntakeDate + "</OldValue><NewValue>" + caseHistMst.IntakeDate + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.InitialDate.Trim() != caseHistMst.InitialDate.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Initial Date</FieldName><OldValue>" + CaseMST.InitialDate + "</OldValue><NewValue>" + caseHistMst.InitialDate + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.CaseType.Trim() != caseHistMst.CaseType.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Case Type</FieldName><OldValue>" + CaseMST.CaseTypeDesc + "</OldValue><NewValue>" + caseHistMst.CaseTypeDesc + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.Housing.Trim() != caseHistMst.Housing.Trim())
                {
                    boolHistory = true;
                    stroldvalue = strNewvalue = string.Empty;
                    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, "**", "**", "**", string.Empty);

                    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.Housing.ToString().Trim());
                    if (comOldvalue != null)
                    {
                        stroldvalue = comOldvalue.Desc;
                    }
                    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.Housing.Trim());
                    if (comNewvalue != null)
                    {
                        strNewvalue = comNewvalue.Desc;
                    }

                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Housing Situation</FieldName><OldValue>" + stroldvalue + "</OldValue><NewValue>" + strNewvalue + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.Site.Trim() != caseHistMst.Site.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Site</FieldName><OldValue>" + CaseMST.Site + "</OldValue><NewValue>" + caseHistMst.Site + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Juvenile.Trim() != caseHistMst.Juvenile.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Juvenile</FieldName><OldValue>" + CaseMST.Juvenile + "</OldValue><NewValue>" + caseHistMst.Juvenile + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Senior.Trim() != caseHistMst.Senior.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Senior</FieldName><OldValue>" + CaseMST.Senior + "</OldValue><NewValue>" + caseHistMst.Senior + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Secret.Trim() != caseHistMst.Secret.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Secret</FieldName><OldValue>" + CaseMST.Secret + "</OldValue><NewValue>" + caseHistMst.Secret + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.CaseReviewDate.Trim() != caseHistMst.CaseReviewDate.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Case Review</FieldName><OldValue>" + CaseMST.CaseReviewDate + "</OldValue><NewValue>" + caseHistMst.CaseReviewDate + "</NewValue></HistoryFields>";
                //}

                // Area Home phone no
                //if (CaseMST.Area.Trim() != caseHistMst.Area.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Area</FieldName><OldValue>" + CaseMST.Area + "</OldValue><NewValue>" + caseHistMst.Area + "</NewValue></HistoryFields>";
                //}
                if ((CaseMST.Area.Trim() + CaseMST.Phone.Trim()) != (caseHistMst.Area.Trim() + caseHistMst.Phone.Trim()))
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Home Phone</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.Area + CaseMST.Phone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.Area + caseHistMst.Phone) + "</NewValue></HistoryFields>";
                }
                if (CaseMST.HomePhone_NA.Trim() != caseHistMst.HomePhone_NA.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Home NA</FieldName><OldValue>" + (CaseMST.HomePhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.HomePhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.NextYear.Trim() != caseHistMst.NextYear.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Next Year</FieldName><OldValue>" + CaseMST.NextYear + "</OldValue><NewValue>" + caseHistMst.NextYear + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Classification.Trim() != caseHistMst.Classification.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_CLASSIFICATION</FieldName><OldValue>" + CaseMST.Classification + "</OldValue><NewValue>" + caseHistMst.Classification + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.AddressYears.Trim() != caseHistMst.AddressYears.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>No of years at address</FieldName><OldValue>" + CaseMST.AddressYears + "</OldValue><NewValue>" + caseHistMst.AddressYears + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.MessagePhone_NA.Trim() != caseHistMst.MessagePhone_NA.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Message NA</FieldName><OldValue>" + (CaseMST.MessagePhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.MessagePhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                if (CaseMST.MessagePhone.Trim() != caseHistMst.MessagePhone.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Message</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.MessagePhone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.MessagePhone) + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.CellPhone_NA.Trim() != caseHistMst.CellPhone_NA.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Cell NA</FieldName><OldValue>" + (CaseMST.CellPhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.CellPhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.CellPhone.Trim() != caseHistMst.CellPhone.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Cell</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.CellPhone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.CellPhone) + "</NewValue></HistoryFields>";
                }
                if (CaseMST.FaxNumber.Trim() != caseHistMst.FaxNumber.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Fax</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.FaxNumber) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.FaxNumber) + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.TtyNumber.Trim() != caseHistMst.TtyNumber.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>TTY</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.TtyNumber) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.TtyNumber) + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.Email_NA.Trim() != caseHistMst.Email_NA.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Email NA</FieldName><OldValue>" + (CaseMST.Email_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.Email_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Email.Trim() != caseHistMst.Email.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Email</FieldName><OldValue>" + CaseMST.Email + "</OldValue><NewValue>" + caseHistMst.Email + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.BestContact.Trim() != caseHistMst.BestContact.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>What is the best way to contact you</FieldName><OldValue>" + CaseMST.ContactusDesc + "</OldValue><NewValue>" + caseHistMst.ContactusDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.AboutUs.Trim() != caseHistMst.AboutUs.Trim())
                //{
                //    boolHistory = true;
                //    stroldvalue = strNewvalue = string.Empty;
                //    List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOWDIDYOUHEARABOUTTHEPROGRAM, "**", "**", "**", string.Empty);

                //    CommonEntity comOldvalue = listAgytabls.Find(u => u.Code == CaseMST.Housing.ToString().Trim());
                //    if (comOldvalue != null)
                //    {
                //        stroldvalue = comOldvalue.Desc;
                //    }
                //    CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == caseHistMst.Housing.Trim());
                //    if (comNewvalue != null)
                //    {
                //        strNewvalue = comNewvalue.Desc;
                //    }

                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>How did you find out about us?</FieldName><OldValue>" + CaseMST.AboutUsDesc + "</OldValue><NewValue>" + caseHistMst.AboutUsDesc + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.ExpRent.Trim() != caseHistMst.ExpRent.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Rent/Mortgage</FieldName><OldValue>" + CaseMST.ExpRent + "</OldValue><NewValue>" + caseHistMst.ExpRent + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ExpWater.Trim() != caseHistMst.ExpWater.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Water/Sewer</FieldName><OldValue>" + CaseMST.ExpWater + "</OldValue><NewValue>" + caseHistMst.ExpWater + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ExpElectric.Trim() != caseHistMst.ExpElectric.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Electric</FieldName><OldValue>" + CaseMST.ExpElectric + "</OldValue><NewValue>" + caseHistMst.ExpElectric + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ExpHeat.Trim() != caseHistMst.ExpHeat.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Heating</FieldName><OldValue>" + CaseMST.ExpHeat + "</OldValue><NewValue>" + caseHistMst.ExpHeat + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.ExpMisc.Trim() != caseHistMst.ExpMisc.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Misc </FieldName><OldValue>" + CaseMST.ExpMisc + "</OldValue><NewValue>" + caseHistMst.ExpMisc + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.Dwelling.Trim() != caseHistMst.Dwelling.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Dwelling Type</FieldName><OldValue>" + CaseMST.DwellingDesc + "</OldValue><NewValue>" + caseHistMst.DwellingDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.HeatIncRent.Trim() != caseHistMst.HeatIncRent.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Method of Paying for Heat</FieldName><OldValue>" + CaseMST.HeatIncRentDesc + "</OldValue><NewValue>" + caseHistMst.HeatIncRentDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Source.Trim() != caseHistMst.Source.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Source of Heat </FieldName><OldValue>" + CaseMST.SourceDesc + "</OldValue><NewValue>" + caseHistMst.SourceDesc + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SubShouse.Trim() != caseHistMst.SubShouse.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Subsidized Housing</FieldName><OldValue>" + CaseMST.SubShouse + "</OldValue><NewValue>" + caseHistMst.SubShouse + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SubStype.Trim() != caseHistMst.SubStype.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Subsidized Housing Type</FieldName><OldValue>" + CaseMST.SubsTypeDesc + "</OldValue><NewValue>" + caseHistMst.SubsTypeDesc + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.ExpCaseWorker.Trim() != caseHistMst.ExpCaseWorker.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Verified that all Household Expenses entered</FieldName><OldValue>" + CaseMST.ExpCaseworkerDesc + "</OldValue><NewValue>" + caseHistMst.ExpCaseworkerDesc + "</NewValue></HistoryFields>";
                //}



            }

            strHistoryDetails = strHistoryDetails + "</XmlHistory>";
            if (boolHistory)
            {
                CaseHistEntity caseHistEntity = new CaseHistEntity();
                caseHistEntity.HistTblName = "CASEMST";
                caseHistEntity.HistScreen = "CASE2001";
                caseHistEntity.HistSubScr = "PIP UpdateForm";
                caseHistEntity.HistTblKey = CaseSNP.Agency + CaseSNP.Dept + CaseSNP.Program + (CaseSNP.Year == string.Empty ? "    " : CaseSNP.Year) + CaseSNP.App + CaseSNP.FamilySeq;
                caseHistEntity.LstcOperator = BaseForm.UserID;
                caseHistEntity.HistChanges = strHistoryDetails;
                _model.CaseMstData.InsertCaseHist(caseHistEntity);

                //if (boolAddressHistory)
                //{
                //    CaseMST.Mode = "Address";
                //    CaseMST.LstcOperator4 = BaseForm.UserID;
                //    _model.CaseMstData.InsertUpdateCaseMst(CaseMST, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg);
                //}


            }

        }

        private void gvwCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (gvwCustomer.Rows[e.RowIndex].Cells["gvtType"].Value.ToString() != "I")
                {
                    if (gvwCustomer.Rows.Count > 0)
                    {
                        if (gvcTopSelect.Index == e.ColumnIndex)
                        {

                            // gvwSubdetails.CellValueChanged -= gvwSubdetails_CellValueChanged;
                            chkSelectAll.CheckedChanged -= new System.EventHandler(chkSelectAll_CheckedChanged);
                            string strChkvalue = gvwCustomer.SelectedRows[0].Cells[e.ColumnIndex].Value == null ? "FALSE" : gvwCustomer.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().ToUpper();
                            //string strChkvalue = gvwCustomer.SelectedCells[e.ColumnIndex].Value == null ? "FALSE" : gvwCustomer.SelectedCells[e.ColumnIndex].Value.ToString().ToUpper();
                            if (strChkvalue == "TRUE")
                            {
                                chkSelectAll.CheckedChanged -= new System.EventHandler(chkSelectAll_CheckedChanged);
                                chkSelectAll.Checked = true;
                                List<DataGridViewRow> lstCust = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
                                lstCust.ForEach(x => x.Cells["gvtSelchk"].Value = true);
                            }
                            else
                            {
                                chkSelectAll.Checked = false;
                                List<DataGridViewRow> lstCust = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
                                lstCust.ForEach(x => x.Cells["gvtSelchk"].Value = false);
                            }
                            chkSelectAll_CheckedChanged(sender, e);
                            // gvwSubdetails.CellValueChanged += gvwSubdetails_CellValueChanged;
                            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;



                            if (gvwCustomer.SelectedRows[0].Cells["gvcTopSelect"].Value.ToString().Trim().ToUpper() == "TRUE")
                            {

                                chkSelectAll.Enabled = true;
                                List<DataGridViewRow> drRows = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
                                drRows.ForEach(xRow => xRow.Cells["gvtSelchk"].ReadOnly = false);
                            }
                            else
                            {
                                chkSelectAll.Enabled = false;
                                List<DataGridViewRow> drRows = gvwSubdetails.Rows.Cast<DataGridViewRow>().ToList();
                                drRows.ForEach(xRow => xRow.Cells["gvtSelchk"].ReadOnly = true);
                            }
                        }
                    }
                }
            }
        }

        void UpdateLeanIntakeData(string strConforNum, string strPIPID, string strAgency, string strDept, string strProgram, string stryear, string strapp, string draggedFrm)
        {

            try
            {

                if (strapp != string.Empty)
                {
                    SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE PIPINTAKE SET PIP_DRAGGED='U',PIP_CAP_AGY='" + strAgency + "',PIP_CAP_DEPT ='" + strDept + "',PIP_CAP_PROG ='" + strProgram + "',PIP_CAP_YEAR ='" + stryear + "',PIP_CAP_APP_NO='" + strapp + "', PIP_DRAG_FRM='" + draggedFrm + "' WHERE PIP_REG_ID= '" + strConforNum + "'", con))// AND PIP_ID =" + strPIPID
                    {
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {


            }

        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            if (btnCombine.Text == "combine")
            {
                IntakeCompareGridLoad("Merged");
                btnCombine.Text = "separate";
            }
            else
            {
                IntakeCompareGridLoad("Merged");
                btnCombine.Text = "combine";
            }
        }

        private void btnIncomeTypes_Click(object sender, EventArgs e)
        {
            if (gvwCustomer.Rows.Count > 0)
            {
                LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;

                if (dritem != null)
                {

                    if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "I" && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N")
                    {

                        PipCustomquestionsForm pipcustom = new PipCustomquestionsForm(BaseForm, "INCOMETYPES", propAgency, propDept, propProgram, propYear, propAppl, propLeanUserId, propLeanServices, strFormType, dritem);
                        pipcustom.StartPosition = FormStartPosition.CenterScreen;
                        pipcustom.ShowDialog();
                    }
                }
            }
        }

        private void gvwSubdetails_Click(object sender, EventArgs e)
        {
            //if (gvwSubdetails.Rows.Count > 0)
            //{
            //    // gvwSubdetails.CellValueChanged -= gvwSubdetails_Click;
            //    if (gvwSubdetails.CurrentCell.ColumnIndex == gvtSelchk.Index)
            //    {
            //        string strlastvalue;
            //        int serviceindex = 0;
            //        LeanIntakeEntity leanentitydata = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;
            //        string strMType = gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value == null ? string.Empty : gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value.ToString();
            //        if (strMType != string.Empty)
            //        {
            //            goto NewvalueEmpty;
            //        }

            //        if (leanentitydata != null)
            //        {
            //            string strFieldValue = gvwSubdetails.CurrentRow.Cells["gvtfield"].Value.ToString();
            //            if (strFieldValue.Contains("Service"))
            //            {
            //                strlastvalue = strFieldValue.LastOrDefault().ToString();
            //                if (strlastvalue != string.Empty)
            //                {
            //                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
            //                }
            //                strFieldValue = "Service";

            //            }
            //            if (strFieldValue.Contains("Health Ins "))
            //            {
            //                strlastvalue = strFieldValue.LastOrDefault().ToString();
            //                if (strlastvalue != string.Empty)
            //                {
            //                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
            //                }
            //                strFieldValue = "Health Ins";

            //            }
            //            if (strFieldValue.Contains("Non Cash Benefit"))
            //            {
            //                strlastvalue = strFieldValue.LastOrDefault().ToString();
            //                if (strlastvalue != string.Empty)
            //                {
            //                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
            //                }
            //                strFieldValue = "Non Cash Benefit";

            //            }
            //            if (strFormType != string.Empty)
            //            {
            //                if (strFieldValue.Contains("Income Type"))
            //                {
            //                    strlastvalue = strFieldValue.LastOrDefault().ToString();
            //                    if (strlastvalue != string.Empty)
            //                    {
            //                        serviceindex = Convert.ToInt32(strlastvalue) - 1;
            //                    }
            //                    strFieldValue = "Income Type";

            //                }
            //            }
            //            string strstatusfield = gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value.ToString();
            //            switch (strFieldValue)
            //            {

            //                case "SSN":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_SSN_MODE = "TRUE";
            //                        if (gvwSubdetails.CurrentRow.Cells["gvtNewValue"].Value.ToString().Substring(3, 2).ToString() == "00")
            //                        {
            //                            leanentitydata.PIP_SSN_MODE = "FALSE";
            //                            CommonFunctions.MessageBoxDisplay("Most Recent ss# is a psuedo you should not replace a good ss# with a psuedo");
            //                            gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value = false;
            //                        }

            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_SSN_MODE = "FALSE";
            //                        if (gvwSubdetails.CurrentRow.Cells["gvtNewValue"].Value.ToString().Substring(3, 2).ToString() == "00")
            //                        {
            //                            leanentitydata.PIP_SSN_MODE = "FALSE";
            //                            //CommonFunctions.MessageBoxDisplay("Most Recent ss# is a psuedo you should not replace a good ss# with a psuedo");
            //                            gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value = false;
            //                        }
            //                    }
            //                    break;

            //                case "First Name":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_FNAME_MODE = "TRUE";

            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_FNAME_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "MI":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_MNAME_MODE = "TRUE";

            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_MNAME_MODE = "FALSE";
            //                    }
            //                    break;
            //                case "Last Name":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_LNAME_MODE = "TRUE";

            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_LNAME_MODE = "FALSE";
            //                    }
            //                    break;
            //                case "Dob":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_DOB_MODE = "TRUE";

            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_DOB_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Active":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_STATUS_MODE = "TRUE";

            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_STATUS_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Gender":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_GENDER_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_GENDER_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Marital Status":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_MARITAL_STATUS_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_MARITAL_STATUS_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Relation":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_RELATIONSHIP_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_RELATIONSHIP_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Ethnicity":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_ETHNIC_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_ETHNIC_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Race":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_RACE_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_RACE_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Education":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_EDUCATION_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_EDUCATION_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Disable":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_DISABLE_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_DISABLE_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Work Status":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_WORK_STAT_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_WORK_STAT_MODE = "FALSE";
            //                    }
            //                    break;
            //                case "Language":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_PRI_LANGUAGE_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_PRI_LANGUAGE_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Family Type":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_FAMILY_TYPE_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_FAMILY_TYPE_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Housing":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_HOUSING_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_HOUSING_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "School":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_SCHOOL_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_SCHOOL_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Health Insurance":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_HEALTH_INS_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_HEALTH_INS_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Veteran":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_VETERAN_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_VETERAN_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Food Stamp":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_FOOD_STAMP_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_FOOD_STAMP_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Farmer":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_FARMER_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_FARMER_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "WIC":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_WIC_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_WIC_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Reliable Transport":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_RELITRAN_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_RELITRAN_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Drivers License":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_DRVLIC_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_DRVLIC_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Military Status":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_MILITARY_STATUS_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_MILITARY_STATUS_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Disconnected Youth":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_YOUTH_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_YOUTH_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Are You Pregnant?":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_PREGNANT_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_PREGNANT_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Home Phone":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_HOME_PHONE_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_HOME_PHONE_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Cell Number":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_CELL_NUMBER_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_CELL_NUMBER_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "House No ":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_HOUSENO_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_HOUSENO_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Direction":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_DIRECTION_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_DIRECTION_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Street":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_STREET_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_STREET_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Suffix":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_SUFFIX_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_SUFFIX_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Apartment":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_APT_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_APT_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Floor":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_FLR_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_FLR_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "City":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_CITY_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_CITY_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "State":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_STATE_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_STATE_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "Zip":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_ZIP_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_ZIP_MODE = "FALSE";
            //                    }
            //                    break;
            //                case "Township":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_TOWNSHIP_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_TOWNSHIP_MODE = "FALSE";
            //                    }
            //                    break;
            //                case "Precinct":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_PRECINT_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_PRECINT_MODE = "FALSE";
            //                    }
            //                    break;

            //                case "County":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_COUNTY_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_COUNTY_MODE = "FALSE";
            //                    }
            //                    break;
            //                case "Health Ins":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.Pip_Healthcodes_list_Mode[serviceindex].Value = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.Pip_Healthcodes_list_Mode[serviceindex].Value = "FALSE";
            //                    }
            //                    break;


            //                case "Non Cash Benefit":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.Pip_NonCashbenefit_list_Mode[serviceindex].Value = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.Pip_NonCashbenefit_list_Mode[serviceindex].Value = "FALSE";
            //                    }
            //                    break;

            //                case "Income Types":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.PIP_INCOME_TYPES_MODE = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.PIP_INCOME_TYPES_MODE = "FALSE";
            //                    }
            //                    break;
            //                case "Service":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.Pip_service_list_Mode[serviceindex].Value = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.Pip_service_list_Mode[serviceindex].Value = "FALSE";
            //                    }

            //                    break;

            //                case "Income Type":
            //                    if (strstatusfield.ToUpper() == "TRUE")
            //                    {
            //                        leanentitydata.Pip_Income_list_Mode[serviceindex].Value = "TRUE";
            //                    }
            //                    else
            //                    {
            //                        leanentitydata.Pip_Income_list_Mode[serviceindex].Value = "FALSE";
            //                    }

            //                    break;



            //            }
            //            if (strstatusfield.ToUpper() == "TRUE")
            //            {
            //                leanentitydata.PIP_Valid_MODE = leanentitydata.PIP_Valid_MODE + 1;
            //            }
            //            else
            //            {
            //                leanentitydata.PIP_Valid_MODE = leanentitydata.PIP_Valid_MODE - 1;
            //            }
            //            gvwCustomer.SelectedRows[0].Tag = leanentitydata;
            //        }

            //    NewvalueEmpty:
            //        {
            //            if (strMType != string.Empty)
            //            {

            //                //string strMType = gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value == null ? string.Empty : gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value.ToString();
            //                gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value = false;


            //                if (strMType == "0")
            //                    CommonFunctions.MessageBoxDisplay("A good field can’t be replace with blank");
            //                if (strMType == "1")
            //                    CommonFunctions.MessageBoxDisplay("An Invalid Item can’t be selected");
            //                if (strMType == "2")
            //                    CommonFunctions.MessageBoxDisplay("An Inactive Item can’t be selected");

            //            }
            //        }
            //    }

            //    // gvwSubdetails.CellValueChanged += gvwSubdetails_CellValueChanged;
            //}

        }

        private void btnNoncashbenefits_Click(object sender, EventArgs e)
        {
            if (gvwCustomer.Rows.Count > 0)
            {
                LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;

                if (dritem != null)
                {

                    if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "I" && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N")
                    {

                        PipCustomquestionsForm pipcustom = new PipCustomquestionsForm(BaseForm, "NONCASHBEBEFITS", propAgency, propDept, propProgram, propYear, propAppl, propLeanUserId, propLeanServices, strFormType, dritem);
                        pipcustom.StartPosition = FormStartPosition.CenterScreen;
                        pipcustom.ShowDialog();
                    }
                }
            }
        }

        private void btnViewAllHealthcodes_Click(object sender, EventArgs e)
        {
            LeanIntakeEntity dritem = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;

            if (dritem != null)
            {

                if (gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "I" && gvwCustomer.SelectedRows[0].Cells["gvtType"].Value.ToString().Trim() != "N")
                {

                    PipCustomquestionsForm pipcustom = new PipCustomquestionsForm(BaseForm, "HEALTH INS CODE", propAgency, propDept, propProgram, propYear, propAppl, propLeanUserId, propLeanServices, strFormType, dritem);
                    pipcustom.StartPosition = FormStartPosition.CenterScreen;
                    pipcustom.ShowDialog();
                }
            }
        }

        private void gvwSubdetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (strFormType == "DSSXML")
            {
                if (gvwSubdetails.Rows.Count > 0)
                {
                    if (gvwSubdetails.CurrentCell.ColumnIndex == gvtSelchk.Index)
                    {
                        if (gvwSubdetails.Rows[e.RowIndex].Cells["gvtfield"].Value.ToString().Contains("Home: House#,Street,Suffix,Apt,Floor") ||
                            gvwSubdetails.Rows[e.RowIndex].Cells["gvtfield"].Value.ToString().Contains("      City Name,State,Zip") ||
                            gvwSubdetails.Rows[e.RowIndex].Cells["gvtfield"].Value.ToString().Contains("Mail: House#,Street,Suffix,Apt,Floor"))
                        {
                            gvwSubdetails.Rows[e.RowIndex].Cells["gvtSelchk"].Value = false;
                        }
                        else
                        {

                            List<DataGridViewRow> uncheckedlst = gvwSubdetails.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["gvtSelchk"].Value.ToString().ToUpper() == "FALSE").ToList();
                            if (uncheckedlst.Count > 0)
                            {
                                chkSelectAll.CheckedChanged -= new System.EventHandler(chkSelectAll_CheckedChanged);
                                chkSelectAll.Checked = false;
                                chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
                            }
                            DataRow _drDSSXMLitem = gvwCustomer.SelectedRows[0].Tag as DataRow;
                            CheckItems(_drDSSXMLitem);
                        }
                        // chkSelectAll_CheckedChanged(sender, e);
                    }
                }
            }
            else
            {
                if (gvwSubdetails.Rows.Count > 0)
                {
                    // gvwSubdetails.CellValueChanged -= gvwSubdetails_Click;
                    if (gvwSubdetails.CurrentCell.ColumnIndex == gvtSelchk.Index)
                    {
                        string strlastvalue;
                        int serviceindex = 0;
                        LeanIntakeEntity leanentitydata = gvwCustomer.SelectedRows[0].Tag as LeanIntakeEntity;
                        string strMType = gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value == null ? string.Empty : gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value.ToString();
                        if (strMType != string.Empty)
                        {
                            goto NewvalueEmpty;
                        }

                        if (leanentitydata != null)
                        {
                            string strFieldValue = gvwSubdetails.CurrentRow.Cells["gvtfield"].Value.ToString();
                            if (strFieldValue.Contains("Service"))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Service";

                            }
                            if (strFieldValue.Contains("Health Ins "))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Health Ins";

                            }
                            if (strFieldValue.Contains("Non Cash Benefit"))
                            {
                                strlastvalue = strFieldValue.LastOrDefault().ToString();
                                if (strlastvalue != string.Empty)
                                {
                                    serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                }
                                strFieldValue = "Non Cash Benefit";

                            }
                            if (strFormType != string.Empty)
                            {
                                if (strFieldValue.Contains("Income Type"))
                                {
                                    strlastvalue = strFieldValue.LastOrDefault().ToString();
                                    if (strlastvalue != string.Empty)
                                    {
                                        serviceindex = Convert.ToInt32(strlastvalue) - 1;
                                    }
                                    strFieldValue = "Income Type";

                                }
                            }
                            string strstatusfield = gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value.ToString();
                            switch (strFieldValue)
                            {

                                case "SSN":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_SSN_MODE = "TRUE";
                                        if (gvwSubdetails.CurrentRow.Cells["gvtNewValue"].Value.ToString().Substring(3, 2).ToString() == "00")
                                        {
                                            leanentitydata.PIP_SSN_MODE = "FALSE";
                                            CommonFunctions.MessageBoxDisplay("Most Recent ss# is a psuedo you should not replace a good ss# with a psuedo");
                                            gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value = false;
                                        }

                                    }
                                    else
                                    {
                                        leanentitydata.PIP_SSN_MODE = "FALSE";
                                        if (gvwSubdetails.CurrentRow.Cells["gvtNewValue"].Value.ToString().Substring(3, 2).ToString() == "00")
                                        {
                                            leanentitydata.PIP_SSN_MODE = "FALSE";
                                            //CommonFunctions.MessageBoxDisplay("Most Recent ss# is a psuedo you should not replace a good ss# with a psuedo");
                                            gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value = false;
                                        }
                                    }
                                    break;

                                case "First Name":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_FNAME_MODE = "TRUE";

                                    }
                                    else
                                    {
                                        leanentitydata.PIP_FNAME_MODE = "FALSE";
                                    }
                                    break;

                                case "MI":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_MNAME_MODE = "TRUE";

                                    }
                                    else
                                    {
                                        leanentitydata.PIP_MNAME_MODE = "FALSE";
                                    }
                                    break;
                                case "Last Name":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_LNAME_MODE = "TRUE";

                                    }
                                    else
                                    {
                                        leanentitydata.PIP_LNAME_MODE = "FALSE";
                                    }
                                    break;
                                case "Dob":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_DOB_MODE = "TRUE";

                                    }
                                    else
                                    {
                                        leanentitydata.PIP_DOB_MODE = "FALSE";
                                    }
                                    break;

                                case "Active":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_STATUS_MODE = "TRUE";

                                    }
                                    else
                                    {
                                        leanentitydata.PIP_STATUS_MODE = "FALSE";
                                    }
                                    break;

                                case "Gender":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_GENDER_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_GENDER_MODE = "FALSE";
                                    }
                                    break;

                                case "Marital Status":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_MARITAL_STATUS_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_MARITAL_STATUS_MODE = "FALSE";
                                    }
                                    break;

                                case "Relation":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_RELATIONSHIP_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_RELATIONSHIP_MODE = "FALSE";
                                    }
                                    break;

                                case "Ethnicity":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_ETHNIC_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_ETHNIC_MODE = "FALSE";
                                    }
                                    break;

                                case "Race":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_RACE_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_RACE_MODE = "FALSE";
                                    }
                                    break;

                                case "Education":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_EDUCATION_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_EDUCATION_MODE = "FALSE";
                                    }
                                    break;

                                case "Disable":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_DISABLE_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_DISABLE_MODE = "FALSE";
                                    }
                                    break;

                                case "Work Status":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_WORK_STAT_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_WORK_STAT_MODE = "FALSE";
                                    }
                                    break;
                                case "Language":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_PRI_LANGUAGE_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_PRI_LANGUAGE_MODE = "FALSE";
                                    }
                                    break;

                                case "Family Type":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_FAMILY_TYPE_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_FAMILY_TYPE_MODE = "FALSE";
                                    }
                                    break;

                                case "Housing":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_HOUSING_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_HOUSING_MODE = "FALSE";
                                    }
                                    break;

                                case "School":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_SCHOOL_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_SCHOOL_MODE = "FALSE";
                                    }
                                    break;

                                case "Health Insurance":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_HEALTH_INS_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_HEALTH_INS_MODE = "FALSE";
                                    }
                                    break;

                                case "Veteran":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_VETERAN_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_VETERAN_MODE = "FALSE";
                                    }
                                    break;

                                case "Food Stamp":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_FOOD_STAMP_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_FOOD_STAMP_MODE = "FALSE";
                                    }
                                    break;

                                case "Farmer":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_FARMER_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_FARMER_MODE = "FALSE";
                                    }
                                    break;

                                case "WIC":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_WIC_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_WIC_MODE = "FALSE";
                                    }
                                    break;

                                case "Reliable Transport":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_RELITRAN_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_RELITRAN_MODE = "FALSE";
                                    }
                                    break;

                                case "Drivers License":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_DRVLIC_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_DRVLIC_MODE = "FALSE";
                                    }
                                    break;

                                case "Military Status":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_MILITARY_STATUS_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_MILITARY_STATUS_MODE = "FALSE";
                                    }
                                    break;

                                case "Disconnected Youth":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_YOUTH_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_YOUTH_MODE = "FALSE";
                                    }
                                    break;

                                case "Are You Pregnant?":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_PREGNANT_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_PREGNANT_MODE = "FALSE";
                                    }
                                    break;

                                case "Home Phone":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_HOME_PHONE_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_HOME_PHONE_MODE = "FALSE";
                                    }
                                    break;

                                case "Cell Number":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_CELL_NUMBER_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_CELL_NUMBER_MODE = "FALSE";
                                    }
                                    break;

                                case "House No ":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_HOUSENO_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_HOUSENO_MODE = "FALSE";
                                    }
                                    break;

                                case "Direction":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_DIRECTION_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_DIRECTION_MODE = "FALSE";
                                    }
                                    break;

                                case "Street":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_STREET_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_STREET_MODE = "FALSE";
                                    }
                                    break;

                                case "Suffix":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_SUFFIX_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_SUFFIX_MODE = "FALSE";
                                    }
                                    break;

                                case "Apartment":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_APT_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_APT_MODE = "FALSE";
                                    }
                                    break;

                                case "Floor":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_FLR_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_FLR_MODE = "FALSE";
                                    }
                                    break;

                                case "City":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_CITY_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_CITY_MODE = "FALSE";
                                    }
                                    break;

                                case "State":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_STATE_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_STATE_MODE = "FALSE";
                                    }
                                    break;

                                case "Zip":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_ZIP_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_ZIP_MODE = "FALSE";
                                    }
                                    break;
                                case "Township":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_TOWNSHIP_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_TOWNSHIP_MODE = "FALSE";
                                    }
                                    break;
                                case "Precinct":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_PRECINT_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_PRECINT_MODE = "FALSE";
                                    }
                                    break;

                                case "County":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_COUNTY_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_COUNTY_MODE = "FALSE";
                                    }
                                    break;
                                case "Health Ins":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.Pip_Healthcodes_list_Mode[serviceindex].Value = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.Pip_Healthcodes_list_Mode[serviceindex].Value = "FALSE";
                                    }
                                    break;


                                case "Non Cash Benefit":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.Pip_NonCashbenefit_list_Mode[serviceindex].Value = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.Pip_NonCashbenefit_list_Mode[serviceindex].Value = "FALSE";
                                    }
                                    break;

                                case "Income Types":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.PIP_INCOME_TYPES_MODE = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.PIP_INCOME_TYPES_MODE = "FALSE";
                                    }
                                    break;
                                case "Service":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.Pip_service_list_Mode[serviceindex].Value = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.Pip_service_list_Mode[serviceindex].Value = "FALSE";
                                    }

                                    break;

                                case "Income Type":
                                    if (strstatusfield.ToUpper() == "TRUE")
                                    {
                                        leanentitydata.Pip_Income_list_Mode[serviceindex].Value = "TRUE";
                                    }
                                    else
                                    {
                                        leanentitydata.Pip_Income_list_Mode[serviceindex].Value = "FALSE";
                                    }

                                    break;



                            }
                            if (strstatusfield.ToUpper() == "TRUE")
                            {
                                leanentitydata.PIP_Valid_MODE = leanentitydata.PIP_Valid_MODE + 1;
                            }
                            else
                            {
                                leanentitydata.PIP_Valid_MODE = leanentitydata.PIP_Valid_MODE - 1;
                            }
                            gvwCustomer.SelectedRows[0].Tag = leanentitydata;
                        }

                    NewvalueEmpty:
                        {
                            if (strMType != string.Empty)
                            {

                                //string strMType = gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value == null ? string.Empty : gvwSubdetails.CurrentRow.Cells["gvtActiveType"].Value.ToString();
                                gvwSubdetails.CurrentRow.Cells["gvtSelchk"].Value = false;


                                if (strMType == "0")
                                    CommonFunctions.MessageBoxDisplay("A good field can’t be replace with blank");
                                if (strMType == "1")
                                    CommonFunctions.MessageBoxDisplay("An Invalid Item can’t be selected");
                                if (strMType == "2")
                                    CommonFunctions.MessageBoxDisplay("An Inactive Item can’t be selected");

                            }
                        }
                    }

                    // gvwSubdetails.CellValueChanged += gvwSubdetails_CellValueChanged;
                }
            }
        }

        DataTable dtDSSFinData = new DataTable();
        DataTable CreateMODEforDSSXML(DataTable dt)
        {

            //dt.Columns.Add("Appno", typeof(string));
            //dt.Columns.Add("Mode", typeof(string));
            //dt.Columns.Add("ISAPP", typeof(string));

            dt.Columns.Add("SSNmode", typeof(string));
            dt.Columns.Add("FNAMEmode", typeof(string));
            dt.Columns.Add("MNAMEmode", typeof(string));
            dt.Columns.Add("LNAMEmode", typeof(string));
            dt.Columns.Add("DOBmode", typeof(string));
            dt.Columns.Add("GENDERmode", typeof(string));
            dt.Columns.Add("ETHENICmode", typeof(string));
            dt.Columns.Add("RACEmode", typeof(string));

            dt.Columns.Add("EDUCATIONmode", typeof(string));
            dt.Columns.Add("DISABLEDmode", typeof(string));
            dt.Columns.Add("MILTARYmode", typeof(string));
            dt.Columns.Add("WORKSTATUSmode", typeof(string));

            dt.Columns.Add("HOUSENOmode", typeof(string));
            dt.Columns.Add("STREETmode", typeof(string));
            dt.Columns.Add("ZIPmode", typeof(string));
            dt.Columns.Add("ZIPCODEmode", typeof(string));
            dt.Columns.Add("STATEmode", typeof(string));
            dt.Columns.Add("CITYmode", typeof(string));
            dt.Columns.Add("PHONEmode", typeof(string));
            dt.Columns.Add("CELLmode", typeof(string));
            dt.Columns.Add("LANGUAGEmode", typeof(string));
            dt.Columns.Add("HOUSINGmode", typeof(string));
            dt.Columns.Add("Emailmode", typeof(string));

            dt.Columns.Add("LLRFnameMode", typeof(string));
            dt.Columns.Add("LLRLnameMode", typeof(string));
            dt.Columns.Add("LLRPhoneMode", typeof(string));
            dt.Columns.Add("LLRCityMode", typeof(string));
            dt.Columns.Add("LLRStateMode", typeof(string));
            dt.Columns.Add("LLRStreetMode", typeof(string));
            dt.Columns.Add("LLRHouseNoMode", typeof(string));
            dt.Columns.Add("LLRZipMode", typeof(string));
            dt.Columns.Add("LLRZipCodeMode", typeof(string));

            dt.Columns.Add("DiffSateMode", typeof(string));
            dt.Columns.Add("DiffCityMode", typeof(string));
            dt.Columns.Add("DiffHNMode", typeof(string));
            dt.Columns.Add("DiffStreetMode", typeof(string));
            dt.Columns.Add("DiffPhoneMode", typeof(string));
            dt.Columns.Add("DiffZipMode", typeof(string));
            dt.Columns.Add("DiffZipPlusMode", typeof(string));

            //Performance Questions
            dt.Columns.Add("MST_LPM_0001Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0002Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0003Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0004Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0005Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0006Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0007Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0008Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0009Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0010Mode", typeof(string));
            dt.Columns.Add("MST_LPM_0011Mode", typeof(string));

            dt.Columns.Add("RENTMORTMode", typeof(string));
            dt.Columns.Add("RELATIONMode", typeof(string));
            dt.Columns.Add("DISYOUTHMode", typeof(string));
            dt.Columns.Add("DWELLINGMode", typeof(string));
            dt.Columns.Add("PRIMPAYHEATMode", typeof(string));
            dt.Columns.Add("PRIMSRCHEARMode", typeof(string));

            return dt;
        }

        private void PIPUpdateApplicantForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (_dssCloseFlag == "Y") { 

            //}
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            DSSAddress dSSAddress = new DSSAddress(BaseForm, propCasemstEntity, CapCaseDiffMailDetails, _dsAllDSSXML, propAgency, propDept, propProgram, propYear, propAppl, 
                _privilegeEntity,_hasVerifiedFlag,this,_xmlAppID,xmlApplMembID,_isNewApplicant);
            dSSAddress.StartPosition = FormStartPosition.CenterScreen;
            dSSAddress.FormClosed += DSSAddress_FormClosed;
            dSSAddress.ShowDialog();
        }

        public static string isAddressSaved = "N";
        private void DSSAddress_FormClosed(object sender, FormClosedEventArgs e)
        {
            rbtnDetailAddressTab.Checked = _AddressDetsTABFlag;
            rbtnPerfMesureTab.Checked = _PerfMeasureTABFlag;
            rbtnSupplierTab.Checked = _SupplierTABFlag;
            if (_hasVerifiedFlag == "Y") {
                rbtnDetailAddressTab.Checked = true;
                rbtnPerfMesureTab.Checked = true;
                rbtnSupplierTab.Checked = true;
            }

            //if (isAddressSaved == "Y")
            //{
                _propCloseFormStatus = "Y";
                propCasemstEntity = _model.CaseMstData.GetCaseMST(propAgency, propDept, propProgram, propYear, propAppl);
                propMainCasemstEntity = _model.CaseMstData.GetCaseMST(propAgency, propDept, propProgram, propYear, propAppl);

                CapCaseDiffMailDetails = _model.CaseMstData.GetCaseDiffadpya(propAgency, propDept, propProgram, propYear, propAppl, "");
                if (CapCaseDiffMailDetails == null)
                    CapCaseDiffMailDetails = new CaseDiffEntity();

                ClearModeDSSXMLDate();
                LoadDSSXMLGrid();
                isAddressSaved = "N";
            //}
        }
    }
}

