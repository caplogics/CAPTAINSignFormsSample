using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using DevExpress.ExpressApp.Validation.AllContextsView;
using DevExpress.XtraRichEdit.Model;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Captain.Common.Utilities;
using Wisej.Web;
using DevExpress.DataAccess.Sql;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;
using DevExpress.XtraCharts;
using DevExpress.Office.Utils;
using NPOI.SS.Formula.Functions;

namespace Captain.Common.Views.Forms
{
    public partial class HUD00003_IndvSessDetails : Form
    {

        #region Private Variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public int selRowIndex = 0;
        public int mstIndex = 0;

        #endregion

        public HUD00003_IndvSessDetails(BaseForm baseform, PrivilegeEntity privilegeEntity, string Individ_Seq,string MST_Seq, string mode, int currentRowIndex, int mstRowIndex, string hudDate)
        {
            InitializeComponent();
            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 0;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.Icon = null;
            Indiv_Seq = Individ_Seq;
            _Mode = mode;
            _MST_Seq = MST_Seq;

            HUDDate = hudDate;
            selRowIndex = currentRowIndex;
            mstIndex = mstRowIndex;

            this.Text = "HUD Form: " + HUDDate + " - Individual Session Details";

            hudindivEntity = _model.HUDCNTLData.GetHUDINDIV(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, "","");
            hudimpactEntity = _model.HUDCNTLData.GetHUDIMPACT(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, "", "","");
            
            txtIndivSeq.Text = Indiv_Seq;
            txtMstSeq.Text = _MST_Seq;

            DataSet dsCounselor = DatabaseLayer.HUDCNTLDB.Get_HUDSTAFF(_baseForm.BaseAgency, "", "2");
            dtCounselor = dsCounselor.Tables[0];

            txtSessClntPaid.Validator = TextBoxValidation.CustomDecimalValidation4dot2;
            txtSessGrantUsed.Validator = TextBoxValidation.CustomDecimalValidation4dot2;

            txtCDHousMontIncome.Validator = TextBoxValidation.CustomDecimalValidation5dot2;

            txtCFHNoDepend.Validator = TextBoxValidation.IntegerValidator;
            txtCFHMontHHLiab.Validator = TextBoxValidation.CustomDecimalValidation5dot2;
            txtCFHCredScore.Validator = TextBoxValidation.IntegerValidator;
            txtCFHLengatJob.Validator = TextBoxValidation.IntegerValidator;

            txtLIClosingCost.Validator = TextBoxValidation.CustomDecimalValidation4dot2;
            txtLIIntRate.Validator = TextBoxValidation.CustomDecimalValidation;
            txtLIUnit.Validator = TextBoxValidation.IntegerValidator;
            txtLIZIP.Validator = TextBoxValidation.IntegerValidator;

            #region Combo box Fillings

            fill_Status_Combo();
            fill_Counselor_Combo();
            fill_SessType_Combo();
            fill_SessPurVisit_Combo();
            fill_SessActType_Combo();
            fill_SessAttGrant_Combo();
            fill_SessFund_Combo();

            fill_CDIncLevel_Combo();
            fill_CDAppEthnicity_Combo();
            fill_CDAppRace_Combo();
            fill_CDLang_Combo();
            fill_CDRefSource_Combo();
            fill_CDRurAreaStatus_Combo();
            fill_CDLimLangProf_Combo();

            fill_CFHSource_Combo();
            fill_CFHNoCredScore_Combo();
            fill_CFHAward_Combo();

            fill_LILoanType_Combo();
            fill_LILoanReported_Combo();
            fill_LIBefMortgage_Combo();
            fill_LIBefFinType_Combo();
            fill_LIBefHECMIssued_Combo();
            fill_LIAftSpouse_Combo();
            fill_LIAftMortgage_Combo();
            fill_LIAftFinType_Combo();
            fill_LIAftCity_Combo();
            fill_LIAftState_Combo();

            #endregion

            if (tbcIndvSessDetails.SelectedIndex == 0)
            {
                dtpSessDte.Focus();
                //pnlSessParams.Enabled = false;
                //btnSave.Visible = btnCancel.Visible = false;
            }

            //if (hudformEntity.Count > 0)
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = true;
            //    pbSessDel.Visible = true;

            //    fillControls();
            //}
            //else
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            //    pbSessDel.Visible = false;
            //}

            if (_Mode == "INSERT")
                ClearControls();
            if (_Mode == "UPDATE")
            {
                if (hudindivEntity.Count > 0)
                {
                    fillControls();
                }
            }

                fillImpacts_Grid();
        }

        private void fillControls()
        {

            HUDINDIVENTITY entity = hudindivEntity.Find(x => x.MST_Seq == txtMstSeq.Text && x.Seq.ToString() == txtIndivSeq.Text);

            if (entity != null)
            {
                //Session
                dtpSessDte.Value = entity.Sess_Date == "" ? DateTime.Now : Convert.ToDateTime(entity.Sess_Date);
                txtSessClntPaid.Text = entity.Sess_Client_Paid;
                SetComboBoxValue(cmbSessCouns, entity.Sess_Counselor);

                if (!string.IsNullOrEmpty(entity.Sess_Start_Time.Trim()))
                {
                    dtpSessStartTime.Text = entity.Sess_Start_Time.ToString();
                    dtpSessStartTime.Checked = true;
                }
                if (!string.IsNullOrEmpty(entity.Sess_End_Time.Trim()))
                {
                    dtpSessEndTime.Text = entity.Sess_End_Time.ToString();
                    dtpSessEndTime.Checked = true;
                }

                SetComboBoxValue(cmbSessStatus, entity.Sess_Status);
                SetComboBoxValue(cmbSessType, entity.Sess_Type);
                SetComboBoxValue(cmbSessPuVisit, entity.Sess_Pur_Visit);
                SetComboBoxValue(cmbSessActType, entity.Sess_Act_Type);
                SetComboBoxValue(cmbSessAttGrant, entity.Sess_Attr_Grant);
                SetComboBoxValue(cmbSessFund, entity.Sess_Fund);
                txtSessGrantUsed.Text = entity.Sess_Grant_Used;

                //Client Demographics
                txtCDHousMontIncome.Text = entity.CD_HH_Month_Inc;
                SetComboBoxValue(cmbCDIncLevel, entity.CD_Inc_Level);
                SetComboBoxValue(cmbCDAppEthnicity, entity.CD_App_Ethnicity);
                SetComboBoxValue(cmbCDAppRace, entity.CD_App_Race);
                SetComboBoxValue(cmbCDCounsLang, entity.CD_Lang);
                SetComboBoxValue(cmbCDRefSource, entity.CD_Ref_Source);
                SetComboBoxValue(cmbCDRurAreaStat, entity.CD_Ref_Area_Status);
                SetComboBoxValue(cmbCDLimEngProfStat, entity.CD_Eng_Prof_Status);

                string[] CD_Chck_Applies = entity.CD_If_Applies.Split(',');
                chkbCD_1.Checked = chkbCD_2.Checked = chkbCD_3.Checked = chkbCD_4.Checked = chkbCD_5.Checked = false;
                if (CD_Chck_Applies.Length > 0)
                {
                    foreach (string cd_check in CD_Chck_Applies)
                    {
                        if (cd_check == "1")
                            chkbCD_1.Checked = true;
                        if (cd_check == "2")
                            chkbCD_2.Checked = true;
                        if (cd_check == "3")
                            chkbCD_3.Checked = true;
                        if (cd_check == "4")
                            chkbCD_4.Checked = true;
                        if (cd_check == "5")
                            chkbCD_5.Checked = true;

                    }
                }

                //Client Financial Health
                txtCFHNoDepend.Text = entity.CFH_Depend;
                txtCFHMontHHLiab.Text = entity.CFH_HH_Liab;
                txtCFHCredScore.Text = entity.CFH_Cred_Score;
                SetComboBoxValue(cmbCFHSource, entity.CFH_Source);
                SetComboBoxValue(cmbCFHIfNoCredScore, entity.CFH_Cred_Score_Reason);
                txtCFHLengatJob.Text = entity.CFH_Job_Length;
                SetComboBoxValue(cmbCFHBesCounslAward, entity.CFH_OTR_Serv_Award);

                //Loan Information
                SetComboBoxValue(cmbLILoanType, entity.LI_Bef_Loan_Type);
                SetComboBoxValue(cmbLILoanBengReported, entity.LI_Bef_Loan_Reported);
                SetComboBoxValue(cmbLIMortType, entity.LI_Bef_Mortgage_Type);
                SetComboBoxValue(cmbLIFinType, entity.LI_Bef_Finance_Type);

                string[] CFH_Chck_Applies = entity.LI_Bef_If_Applies.Split(',');
                chkb_LI_1.Checked = chkb_LI_2.Checked = chkb_LI_3.Checked = chkb_LI_4.Checked = chkb_LI_5.Checked = chkb_LI_6.Checked = chkb_LI_7.Checked = chkb_LI_8.Checked = false;
                if (CFH_Chck_Applies.Length > 0)
                {
                    foreach (string cfh_check in CFH_Chck_Applies)
                    {
                        if (cfh_check == "1")
                            chkb_LI_1.Checked = true;
                        if (cfh_check == "2")
                            chkb_LI_2.Checked = true;
                        if (cfh_check == "3")
                            chkb_LI_3.Checked = true;
                        if (cfh_check == "4")
                            chkb_LI_4.Checked = true;
                        if (cfh_check == "5")
                            chkb_LI_5.Checked = true;
                        if (cfh_check == "6")
                            chkb_LI_6.Checked = true;
                        if (cfh_check == "7")
                            chkb_LI_7.Checked = true;
                        if (cfh_check == "8")
                            chkb_LI_8.Checked = true;
                    }
                }

                SetComboBoxValue(cmbLIHECMCert, entity.LI_Bef_Is_Cert_HUD);
                txtLIHECMID.Text = entity.LI_Bef_HECM_ID;
                dtpLIIssued.Value = entity.LI_Bef_Cert_Issued == "" ? DateTime.Now : Convert.ToDateTime(entity.LI_Bef_Cert_Issued);
                dtpLIExpires.Value = entity.LI_Bef_Cert_Expires == "" ? DateTime.Now : Convert.ToDateTime(entity.LI_Bef_Cert_Expires);

                SetComboBoxValue(cmbLISpouse, entity.LI_Aft_Loan_Spouse);
                SetComboBoxValue(cmbLIMortType2, entity.LI_Aft_Mortgage_Type);
                SetComboBoxValue(cmbLIFinType2, entity.LI_Aft_Finance_Type);
                txtLIIntRate.Text = entity.LI_Aft_Interest_Rate;
                txtLIClosingCost.Text = entity.LI_Aft_Close_Cost;
                txtLIStreet.Text = entity.LI_Aft_Street;
                txtLIUnit.Text = entity.LI_Aft_Unit;
                SetComboBoxValue(cmbLICity, entity.LI_Aft_City);
                SetComboBoxValue(cmbLIState, entity.LI_Aft_State);
                txtLIZIP.Text = entity.LI_Aft_ZIP;
            }

            //Impacts
            if (hudimpactEntity.Count > 0)
            {
                HUDIMPACTENTITY impEntity = hudimpactEntity.Find(x => x.Indv_Seq == txtIndivSeq.Text && x.Mst_Seq == txtMstSeq.Text);

                if (impEntity != null)
                {
                    dtpImpactDte.Value = impEntity.Impact_Date.ToString() == "" ? DateTime.Now : Convert.ToDateTime(impEntity.Impact_Date.ToString());
                }
                fillImpacts_Grid();
            }

        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        #region Properties

        public BaseForm _baseForm
        {
            get;set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get; set;
        }

        public string _Mode
        {
            get; set;
        }

        public string HUDDate
        {
        get; set; }

        public List<HUDINDIVENTITY> hudindivEntity
        {
            get; set;
        }

        public List<HUDIMPACTENTITY> hudimpactEntity
        {
            get; set;
        }

        public DataTable dtCounselor
        {
            get; set;
        }

        public string Indiv_Seq
        {
            get;
            set;
        }
        public string _MST_Seq
        {
            get;
            set;
        }
        #endregion

        //List<HUDIMPACTENTITY> fillimpactEntity = new List<HUDIMPACTENTITY>();
        private void fillImpacts_Grid()
        {

            dgvImpacts.Rows.Clear();

            dgvImpacts.Rows.Add(false, "Households that received one-on-one counseling that also received education services.","1",string.Empty);

            dgvImpacts.Rows.Add(false, "Households that received information fair housing, fair lending and/or accessibility rights.","2", string.Empty);

            dgvImpacts.Rows.Add(false, "Households for whom counselor developed a budget customized to a client's current situation.","3", string.Empty);

            dgvImpacts.Rows.Add(false, "Households that improved their financial capacity (e.g. increased discretionary income, decreased debt load, increased savings, increased credit score, etc.) after receiving Housing Counseling Services.","4", string.Empty);

            dgvImpacts.Rows.Add(false, "Households that gained access to resources to help improve their housing situation (e.g. down payment assistance, rental assistance, utility assistance, etc.) after receiving Housing Counseling Services.","5", string.Empty);

            dgvImpacts.Rows.Add(false, "Households that gained access to non-housing resources (e.g. social service programs, legal services, public benefits such as Social Security or Medicaid, etc.) after receiving Housing Counseling Services.","6", string.Empty);

            dgvImpacts.Rows.Add(false, "Homeless or potentially homeless households that obtained temporary or permanent housing after receiving Housing Counseling Services.","7", string.Empty);

            dgvImpacts.Rows.Add(false, "Households that gained access to disaster recovery non-housing resources after receiving Housing Counseling Services (e.g. Red Cross/FEMA relief items, legal services, assistance).","8", string.Empty);

            dgvImpacts.Rows.Add(false, "Households obtained disaster recovery housing resources after receiving Housing Counseling Services (e.g. temporary shelter, homeowner rehab, relocation, etc.).","9", string.Empty);

            dgvImpacts.Rows.Add(false, "Households for whom counselor developed or updated an emergency preparedness plan.","10", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that received rental counseling and avoided eviction after receiving Housing Counseling Services.","11", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that received rental counseling and improved living conditions after receiving Housing Counseling Services.","12", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that received pre-purchase/homebuying counseling and purchased housing after receiving Housing Counseling Services.","13", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that received reverse mortgage counseling and obtained a Home Enquiry Conversion Mortgage (HECM) after receiving Housing Counseling Services.","14", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that received non-delinquency post-purchase counseling that were able to improve home conditions or home affordability after receiving Housing Counseling Services.","15", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that prevented or resolved a forward mortgage default after receiving Housing Counseling Services.","16", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that prevented or resolved a reverse mortgage default after receiving Housing Counseling Services.","17", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that received a forward mortgage modification and remained current in their modified mortgage after receiving Housing Counseling Services.","18", string.Empty);

            dgvImpacts.Rows.Add(false, "Household that received a forward mortgage modification and improved their financial capacity after receiving Housing Counseling Services.","19", string.Empty);

            if (dgvImpacts.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in dgvImpacts.Rows)
                {
                    List<HUDIMPACTENTITY> fillimpactEntity = hudimpactEntity.FindAll(s => s.Impacts == dr.Cells["gvImpactID"].Value.ToString() && s.Indv_Seq == txtIndivSeq.Text && s.Mst_Seq == txtMstSeq.Text && LookupDataAccess.Getdate(s.Impact_Date) == dtpImpactDte.Text);

                    if (fillimpactEntity.Count > 0)
                    {
                        foreach (HUDIMPACTENTITY ent in fillimpactEntity)
                        {
                            dr.Cells["gvSelect"].Value = "True";
                            dr.Cells["gvImpactSeq"].Value = ent.Seq;
                        }
                    }
                }
            }
        }

        private void tbcIndvSessDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbcIndvSessDetails.SelectedIndex == 0)
            {
                dtpSessDte.Focus();

                //**fill_Status_Combo();
            }
            else if (tbcIndvSessDetails.SelectedIndex == 1)
            {
                txtCDHousMontIncome.Focus();
            }
            else if (tbcIndvSessDetails.SelectedIndex == 2)
            {
                txtCFHNoDepend.Focus();
            }
            else if (tbcIndvSessDetails.SelectedIndex == 3)
            {
                cmbLILoanType.Focus();
            }
            else if (tbcIndvSessDetails.SelectedIndex == 4)
            {
                dtpImpactDte.Focus();
            }

            if (hudindivEntity.Count > 0)
            {
                if (btnSave.Visible == true && btnCancel.Visible == true)
                {
                    pbSessAdd.Visible = false;
                    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
                    pbSessDel.Visible = false;

                    pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
                }
                else
                {
                    pbSessAdd.Visible = true;
                    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = true;
                    pbSessDel.Visible = true;

                    //pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = false;
                }
            }
            else
            {
                if (btnSave.Visible == true && btnCancel.Visible == true)
                {
                    pbSessAdd.Visible = false;
                    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
                    pbSessDel.Visible = false;

                    pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
                }
                else
                {
                    pbSessAdd.Visible = true;
                    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
                    pbSessDel.Visible = false;

                    //pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = false;
                }
            }
        }

        #region Combo Box Fillings

        List<CommonEntity> StatusList = new List<CommonEntity>();
        private void fill_Status_Combo()
        {
            cmbSessStatus.Items.Clear();

            //StatusList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0047", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbSessStatus.Items.Add(new ListItem("Select One", "0"));

            //foreach (CommonEntity entity in StatusList)
            //{
            //    cmbSessStatus.Items.Add(new ListItem(entity.Desc, entity.Code));
            //}

            cmbSessStatus.Items.Add(new ListItem("In Progress", "O"));
            cmbSessStatus.Items.Add((new ListItem("Completed", "C")));

            if (cmbSessStatus.Items.Count > 0)
                cmbSessStatus.SelectedIndex = 0;
        }

        List<STAFFMSTEntity> CounsleorList = new List<STAFFMSTEntity>();
        private void fill_Counselor_Combo()
        {
           cmbSessCouns.Items.Clear();

            dtCounselor = dtCounselor.AsEnumerable().GroupBy(row => row.Field<string>("HUDS_STF_CODE")).Select(group => group.First()).CopyToDataTable();

            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");

            cmbSessCouns.Items.Add(new ListItem("", "E"));

            foreach (DataRow dr in dtCounselor.Rows)
            {
                CounsleorList = STAFFMST_List.Where(x => x.Staff_Code == dr["HUDS_STF_CODE"].ToString()).ToList();

                cmbSessCouns.Items.Add(new ListItem((CounsleorList[0].First_Name + " " + CounsleorList[0].Last_Name), CounsleorList[0].Staff_Code));
            }

            if (cmbSessCouns.Items.Count > 0)
                cmbSessCouns.SelectedIndex = 0;
        }

        List<CommonEntity> SessTypeList = new List<CommonEntity>();
        private void fill_SessType_Combo()
        {
            cmbSessType.Items.Clear();

            SessTypeList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0300", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbSessType.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in SessTypeList)
            {
                cmbSessType.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbSessType.Items.Add(new ListItem("Face to Face", "F"));
            //cmbSessType.Items.Add((new ListItem("Phone", "P")));
            //cmbSessType.Items.Add(new ListItem("Group", "G"));
            //cmbSessType.Items.Add((new ListItem("Internet", "I")));
            //cmbSessType.Items.Add(new ListItem("Other", "O"));
            //cmbSessType.Items.Add((new ListItem("N/A", "N")));

            if (cmbSessType.Items.Count > 0)
                cmbSessType.SelectedIndex = 0;
        }

        List<CommonEntity> PurVisitList = new List<CommonEntity>();
        private void fill_SessPurVisit_Combo()
        {
            cmbSessPuVisit.Items.Clear();

            PurVisitList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0301", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbSessPuVisit.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in PurVisitList)
            {
                cmbSessPuVisit.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbSessPuVisit.Items.Add(new ListItem("Homeless Assistance", "HA"));
            //cmbSessPuVisit.Items.Add((new ListItem("Rental Topics", "RT")));
            //cmbSessPuVisit.Items.Add(new ListItem("Prepurchase/Homebuying", "PH"));
            //cmbSessPuVisit.Items.Add((new ListItem("Non-Delinquency Post-Purchase", "NP")));
            //cmbSessPuVisit.Items.Add(new ListItem("Reverse Mortgage", "RM"));
            //cmbSessPuVisit.Items.Add((new ListItem("Resolving or Preventing Forward Mortgage Delinquency or Default", "RF")));
            //cmbSessPuVisit.Items.Add(new ListItem("Resolving or Preventing Reverse Mortgage Delinquency or Default", "RR"));
            //cmbSessPuVisit.Items.Add((new ListItem("Disaster Preparedness Assistance", "DP")));
            //cmbSessPuVisit.Items.Add(new ListItem("Disaster Recovery Assistance", "DR"));

            if (cmbSessPuVisit.Items.Count > 0)
                cmbSessPuVisit.SelectedIndex = 0;
        }

        List<CommonEntity> ActTypeList = new List<CommonEntity>();
        private void fill_SessActType_Combo()
        {
            cmbSessActType.Items.Clear();

            ActTypeList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0302", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbSessActType.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in ActTypeList)
            {
                cmbSessActType.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbSessActType.Items.Add(new ListItem("HUD – All Activities (4)", "A"));

            if (cmbSessActType.Items.Count > 0)
                cmbSessActType.SelectedIndex = 0;
        }

        List<CommonEntity> AttGrantList = new List<CommonEntity>();
        private void fill_SessAttGrant_Combo()
        {
            cmbSessAttGrant.Items.Clear();

            AttGrantList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0304", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbSessAttGrant.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in AttGrantList)
            {
                cmbSessAttGrant.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbSessAttGrant.Items.Add(new ListItem("Ongoing - use if not known", "H"));
            //cmbSessAttGrant.Items.Add((new ListItem("Completed", "C")));
            //cmbSessAttGrant.Items.Add(new ListItem("Dropped out", "D"));
            //cmbSessAttGrant.Items.Add((new ListItem("Referred", "R")));
            //cmbSessAttGrant.Items.Add(new ListItem("No Further Contact", "F"));
            //cmbSessAttGrant.Items.Add((new ListItem("Other", "O")));
            //cmbSessAttGrant.Items.Add(new ListItem("N/A", "N"));

            if (cmbSessAttGrant.Items.Count > 0)
                cmbSessAttGrant.SelectedIndex = 0;
        }

        List<CommonEntity> FundList = new List<CommonEntity>();
        private void fill_SessFund_Combo()
        {
            cmbSessFund.Items.Clear();

            FundList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0303", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbSessFund.Items.Add(new ListItem("Not Assigned", "0"));

            foreach (CommonEntity entity in FundList)
            {
                cmbSessFund.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbSessFund.Items.Add(new ListItem("2024 - Office of the Attorney General (New York State)", "O"));

            if (cmbSessFund.Items.Count > 0)
                cmbSessFund.SelectedIndex = 0;
        }

        List<CommonEntity> IncLevelList = new List<CommonEntity>();
        private void fill_CDIncLevel_Combo()
        {
            cmbCDIncLevel.Items.Clear();

            IncLevelList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0305", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbCDIncLevel.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in IncLevelList)
            {
                cmbCDIncLevel.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCDIncLevel.Items.Add(new ListItem("<30% of AMI", "1"));
            //cmbCDIncLevel.Items.Add((new ListItem("30-49% of AMI", "2")));
            //cmbCDIncLevel.Items.Add(new ListItem("50-79% of AMI", "3"));
            //cmbCDIncLevel.Items.Add((new ListItem("80-100% AMI", "4")));
            //cmbCDIncLevel.Items.Add(new ListItem(">100% AMI", "5"));
            //cmbCDIncLevel.Items.Add((new ListItem("Chose not to respond", "6")));

            if (cmbCDIncLevel.Items.Count > 0)
                cmbCDIncLevel.SelectedIndex = 0;
        }

        List<CommonEntity> AppRaceList = new List<CommonEntity>();
        private void fill_CDAppRace_Combo()
        {
            cmbCDAppRace.Items.Clear();

            //AppRaceList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0306", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            AppRaceList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _Mode);

            cmbCDAppRace.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in AppRaceList)
            {
                cmbCDAppRace.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCDAppRace.Items.Add(new ListItem("American Indian/Alaska Native", "AA"));
            //cmbCDAppRace.Items.Add((new ListItem("Asian", "AS")));
            //cmbCDAppRace.Items.Add(new ListItem("Black or African American", "BA"));
            //cmbCDAppRace.Items.Add((new ListItem("Native Hawaiian or other Pacific Islander", "NH")));
            //cmbCDAppRace.Items.Add(new ListItem("White", "WH"));
            //cmbCDAppRace.Items.Add((new ListItem("More than one Race", "MR")));
            //cmbCDAppRace.Items.Add((new ListItem("Chose not to respond", "CR")));

            if (cmbCDAppRace.Items.Count > 0)
                cmbCDAppRace.SelectedIndex = 0;
        }

        List<CommonEntity> AppEthnicityList = new List<CommonEntity>();
        private void fill_CDAppEthnicity_Combo()
        {
            cmbCDAppEthnicity.Items.Clear();

            //AppRaceList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0306", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            AppEthnicityList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _Mode);

            cmbCDAppEthnicity.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in AppEthnicityList)
            {
                cmbCDAppEthnicity.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCDAppRace.Items.Add(new ListItem("American Indian/Alaska Native", "AA"));
            //cmbCDAppRace.Items.Add((new ListItem("Asian", "AS")));
            //cmbCDAppRace.Items.Add(new ListItem("Black or African American", "BA"));
            //cmbCDAppRace.Items.Add((new ListItem("Native Hawaiian or other Pacific Islander", "NH")));
            //cmbCDAppRace.Items.Add(new ListItem("White", "WH"));
            //cmbCDAppRace.Items.Add((new ListItem("More than one Race", "MR")));
            //cmbCDAppRace.Items.Add((new ListItem("Chose not to respond", "CR")));

            if (cmbCDAppEthnicity.Items.Count > 0)
                cmbCDAppEthnicity.SelectedIndex = 0;
        }

        List<CommonEntity> LanguageList = new List<CommonEntity>();
        private void fill_CDLang_Combo()
        {
            cmbCDCounsLang.Items.Clear();

            //LanguageList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0307", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            LanguageList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _Mode);

            cmbCDCounsLang.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in LanguageList)
            {
                cmbCDCounsLang.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCDCounsLang.Items.Add(new ListItem("English", "E"));
            //cmbCDCounsLang.Items.Add((new ListItem("Spanish", "S")));

            if (cmbCDCounsLang.Items.Count > 0)
                cmbCDCounsLang.SelectedIndex = 0;
        }

        List<CommonEntity> RefSourceList = new List<CommonEntity>();
        private void fill_CDRefSource_Combo()
        {
            cmbCDRefSource.Items.Clear();

            RefSourceList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0308", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbCDRefSource.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in RefSourceList)
            {
                cmbCDRefSource.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCDRefSource.Items.Add(new ListItem("Agency", "AG"));
            //cmbCDRefSource.Items.Add((new ListItem("Agency Outreach", "AO")));
            //cmbCDRefSource.Items.Add(new ListItem("HUD Website", "HW"));
            //cmbCDRefSource.Items.Add((new ListItem("Lender", "LE")));
            //cmbCDRefSource.Items.Add(new ListItem("Not Applicable", "NA"));

            if (cmbCDRefSource.Items.Count > 0)
                cmbCDRefSource.SelectedIndex = 0;
        }

        List<CommonEntity> RurAreaList = new List<CommonEntity>();
        private void fill_CDRurAreaStatus_Combo()
        {
            cmbCDRurAreaStat.Items.Clear();

            RurAreaList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0309", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbCDRurAreaStat.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in RurAreaList)
            {
                cmbCDRurAreaStat.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCDRurAreaStat.Items.Add(new ListItem("Lives in Rural area", "LR"));
            //cmbCDRurAreaStat.Items.Add((new ListItem("Does not live in rural area", "DR")));
            //cmbCDRurAreaStat.Items.Add(new ListItem("Chose not to respond", "CR"));

            if (cmbCDRurAreaStat.Items.Count > 0)
                cmbCDRurAreaStat.SelectedIndex = 0;
        }

        List<CommonEntity> LimLangList = new List<CommonEntity>();
        private void fill_CDLimLangProf_Combo()
        {
            cmbCDLimEngProfStat.Items.Clear();

            LimLangList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0310", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbCDLimEngProfStat.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in LimLangList)
            {
                cmbCDLimEngProfStat.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCDLimEngProfStat.Items.Add(new ListItem("Limited English Proficient", "LE"));
            //cmbCDLimEngProfStat.Items.Add((new ListItem("Not Limited English Proficient", "NE")));
            //cmbCDLimEngProfStat.Items.Add(new ListItem("Chose not to respond", "CR"));

            if (cmbCDLimEngProfStat.Items.Count > 0)
                cmbCDLimEngProfStat.SelectedIndex = 0;
        }

        List<CommonEntity> CFHSourceList = new List<CommonEntity>();
        private void fill_CFHSource_Combo()
        {
            cmbCFHSource.Items.Clear();

            CFHSourceList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0311", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbCFHSource.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in CFHSourceList)
            {
                cmbCFHSource.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCFHSource.Items.Add(new ListItem("Experian", "E"));
            //cmbCFHSource.Items.Add((new ListItem("Other", "O")));
            //cmbCFHSource.Items.Add(new ListItem("Transunion", "T"));
            //cmbCFHSource.Items.Add((new ListItem("Tri-Merge", "M")));
            //cmbCFHSource.Items.Add(new ListItem("(can be left blank if no credit score source)", "C"));

            if (cmbCFHSource.Items.Count > 0)
                cmbCFHSource.SelectedIndex = 0;
        }

        List<CommonEntity> NoCredScoreList = new List<CommonEntity>();
        private void fill_CFHNoCredScore_Combo()
        {
            cmbCFHIfNoCredScore.Items.Clear();

            NoCredScoreList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0312", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbCFHIfNoCredScore.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in NoCredScoreList)
            {
                cmbCFHIfNoCredScore.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCFHIfNoCredScore.Items.Add(new ListItem("Client refused to authorize credit report", "CR"));
            //cmbCFHIfNoCredScore.Items.Add((new ListItem("Foreclosure expected within 14 days", "FE")));
            //cmbCFHIfNoCredScore.Items.Add(new ListItem("NFMC Counseling Organization analyzed credit report that did not contain score", "NC"));
            //cmbCFHIfNoCredScore.Items.Add((new ListItem("Other", "OT")));
            //cmbCFHIfNoCredScore.Items.Add(new ListItem("(can be left blank)", "CB"));

            if (cmbCFHIfNoCredScore.Items.Count > 0)
                cmbCFHIfNoCredScore.SelectedIndex = 0;
        }

        List<CommonEntity> CFHAwardList = new List<CommonEntity>();
        private void fill_CFHAward_Combo()
        {
            cmbCFHBesCounslAward.Items.Clear();

            cmbCFHBesCounslAward.Items.Add(new ListItem("", "E"));

            if (cmbCFHBesCounslAward.Items.Count > 0)
                cmbCFHBesCounslAward.SelectedIndex = 0;
        }

        List<CommonEntity> LoanTypeList = new List<CommonEntity>();
        private void fill_LILoanType_Combo()
        {
            cmbLILoanType.Items.Clear();

            LoanTypeList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0313", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbLILoanType.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in LoanTypeList)
            {
                cmbLILoanType.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbLILoanType.Items.Add(new ListItem("Hybrid ARM loan", "HL"));
            //cmbLILoanType.Items.Add((new ListItem("Option ARM loan", "OL")));
            //cmbLILoanType.Items.Add(new ListItem("Interest Only", "IO"));
            //cmbLILoanType.Items.Add((new ListItem("FHA or VA Insured", "FV")));
            //cmbLILoanType.Items.Add(new ListItem("Privately held", "PH"));

            if (cmbLILoanType.Items.Count > 0)
                cmbLILoanType.SelectedIndex = 0;
        }

        List<CommonEntity> LoanReportedList = new List<CommonEntity>();
        private void fill_LILoanReported_Combo()
        {
            cmbLILoanBengReported.Items.Clear();

            LoanReportedList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0314", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbLILoanBengReported.Items.Add(new ListItem("", "0"));

            foreach (CommonEntity entity in LoanReportedList)
            {
                cmbLILoanBengReported.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbLILoanBengReported.Items.Add(new ListItem("First", "F"));

            if (cmbLILoanBengReported.Items.Count > 0)
                cmbLILoanBengReported.SelectedIndex = 0;
        }

        List<CommonEntity> BefMortgList = new List<CommonEntity>();
        private void fill_LIBefMortgage_Combo()
        {
            cmbLIMortType.Items.Clear();

            BefMortgList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0315", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbLIMortType.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in BefMortgList)
            {
                cmbLIMortType.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbLIMortType.Items.Add(new ListItem("30- year fixed", "30"));
            //cmbLIMortType.Items.Add((new ListItem("15 - year fixed", "15")));
            //cmbLIMortType.Items.Add(new ListItem("2 – year ARM", "02"));
            //cmbLIMortType.Items.Add((new ListItem("40 – year fixed", "40")));
            //cmbLIMortType.Items.Add(new ListItem("5 – year ARM", "05"));
            //cmbLIMortType.Items.Add((new ListItem("Interest Only", "IO")));
            //cmbLIMortType.Items.Add((new ListItem("N/A", "NA")));

            if (cmbLIMortType.Items.Count > 0)
                cmbLIMortType.SelectedIndex = 0;
        }

        List<CommonEntity> BefFinTypeList = new List<CommonEntity>();
        private void fill_LIBefFinType_Combo()
        {
            cmbLIFinType.Items.Clear();

            BefFinTypeList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0316", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbLIFinType.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in BefFinTypeList)
            {
                cmbLIFinType.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbLIFinType.Items.Add(new ListItem("Conventional", "C"));
            //cmbLIFinType.Items.Add((new ListItem("FHA", "F")));
            //cmbLIFinType.Items.Add(new ListItem("N/A", "N"));

            if (cmbLIFinType.Items.Count > 0)
                cmbLIFinType.SelectedIndex = 0;
        }

        List<CommonEntity> BefHECMIList = new List<CommonEntity>();
        private void fill_LIBefHECMIssued_Combo()
        {
            cmbLIHECMCert.Items.Clear();

            BefHECMIList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0001", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbLIHECMCert.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in BefHECMIList)
            {
                cmbLIHECMCert.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbLIHECMCert.Items.Add(new ListItem("Yes", "Y"));
            //cmbLIHECMCert.Items.Add((new ListItem("No", "N")));

            if (cmbLIHECMCert.Items.Count > 0)
                cmbLIHECMCert.SelectedIndex = 0;
        }

        List<CommonEntity> AftSpouseList = new List<CommonEntity>();
        private void fill_LIAftSpouse_Combo()
        {
            cmbLISpouse.Items.Clear();

            cmbLISpouse.Items.Add(new ListItem("", "E"));

            if (cmbLISpouse.Items.Count > 0)
                cmbLISpouse.SelectedIndex = 0;
        }

        List<CommonEntity> AftMortgList = new List<CommonEntity>();
        private void fill_LIAftMortgage_Combo()
        {
            cmbLIMortType2.Items.Clear();

            AftMortgList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0315", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbLIMortType2.Items.Add(new ListItem("Select One", "00"));

            foreach (CommonEntity entity in AftMortgList)
            {
                cmbLIMortType2.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbLIMortType2.Items.Add(new ListItem("30- year fixed", "30"));
            //cmbLIMortType2.Items.Add((new ListItem("15 - year fixed", "15")));
            //cmbLIMortType2.Items.Add(new ListItem("2 – year ARM", "02"));
            //cmbLIMortType2.Items.Add((new ListItem("40 – year fixed", "40")));
            //cmbLIMortType2.Items.Add(new ListItem("5 – year ARM", "05"));
            //cmbLIMortType2.Items.Add((new ListItem("Interest Only", "IO")));
            //cmbLIMortType2.Items.Add((new ListItem("N/A", "NA")));

            if (cmbLIMortType2.Items.Count > 0)
                cmbLIMortType2.SelectedIndex = 0;
        }

        List<CommonEntity> AftFinTypeList = new List<CommonEntity>();
        private void fill_LIAftFinType_Combo()
        {
            cmbLIFinType2.Items.Clear();

            AftFinTypeList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0316", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbLIFinType2.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in AftFinTypeList)
            {
                cmbLIFinType2.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbLIFinType2.Items.Add(new ListItem("Conventional", "C"));
            //cmbLIFinType2.Items.Add((new ListItem("FHA", "F")));
            //cmbLIFinType2.Items.Add(new ListItem("N/A", "N"));

            if (cmbLIFinType2.Items.Count > 0)
                cmbLIFinType2.SelectedIndex = 0;
        }

        List<CommonEntity> AftCityList = new List<CommonEntity>();
        private void fill_LIAftCity_Combo()
        {
            cmbLICity.Items.Clear();

            cmbLICity.Items.Add(new ListItem("", "E"));

            if (cmbLICity.Items.Count > 0)
                cmbLICity.SelectedIndex = 0;
        }

        List<CommonEntity> AftStateList = new List<CommonEntity>();
        private void fill_LIAftState_Combo()
        {
            cmbLIState.Items.Clear();

            cmbLIState.Items.Add(new ListItem("N/A", "E"));

            if (cmbLIState.Items.Count > 0)
                cmbLIState.SelectedIndex = 0;
        }

        #endregion

        private bool ValidateForm()
        {
            bool IsValid = true;

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);
            _errorProvider.SetError(cmbSessCouns, null);
            _errorProvider.SetError(dtpSessEndTime, null);

            #region Validate Dates

            if (string.IsNullOrEmpty(dtpSessDte.Text.Trim()))
            {
                _errorProvider.SetError(dtpSessDte, lblSessDte.Text.Trim() + " is required.");
                IsValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpSessDte, null);

                if (dtpSessDte.Value > DateTime.Now)
                {
                    _errorProvider.SetError(dtpSessDte, "Future Date is not allowed.");
                    IsValid = false;
                }
                else
                {

                    _errorProvider.SetError(dtpSessDte, null);

                    if (Convert.ToDateTime(dtpSessDte.Text) < Convert.ToDateTime(HUDDate))
                    {
                        _errorProvider.SetError(dtpSessDte, "Session Date should be equal or greater than HUD Form Date");
                        IsValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpSessDte, null);
                    }
                }
            }

            if (string.IsNullOrEmpty(dtpImpactDte.Text.Trim()))
            {
                _errorProvider.SetError(dtpImpactDte, lblImpDate.Text.Trim() + " is required.");
                IsValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpImpactDte, null);

                bool anyCheckboxChecked = dgvImpacts.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["gvSelect"].Value) == true);

                if (anyCheckboxChecked)
                {
                    if (dtpImpactDte.Value > DateTime.Now)
                    {
                        _errorProvider.SetError(dtpImpactDte, "Future Date is not allowed.");
                        IsValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpImpactDte, null);
                        if (dtpImpactDte.Value < dtpSessDte.Value)
                        {
                            _errorProvider.SetError(dtpImpactDte, "Impact Date should be equal or greater than Session Date");
                            IsValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtpImpactDte, null);
                        }

                    }
                }
            }

            #endregion

            #region Validate Other Fields

            if (dtpSessStartTime.Checked && dtpSessEndTime.Checked)
            {
                if (Convert.ToDateTime(LookupDataAccess.GetTime(dtpSessEndTime.Text)) < Convert.ToDateTime(LookupDataAccess.GetTime(dtpSessStartTime.Text)))
                {
                    _errorProvider.SetError(dtpSessEndTime, ("End Time cannot be less than Start Time"));
                    IsValid = false;
                }
                else
                    _errorProvider.SetError(dtpSessEndTime, null);
            }

            if (cmbSessCouns.Items.Count > 0)
            {
                if (cmbSessCouns.SelectedIndex == 0)
                {
                    _errorProvider.SetError(cmbSessCouns, (lblSessCouns.Text + " is required."));
                    IsValid = false;
                }
                else
                    _errorProvider.SetError(cmbSessCouns, null);
            }

            if (string.IsNullOrEmpty(dtpLIIssued.Text.Trim()))
            {
                _errorProvider.SetError(dtpLIIssued, lblLIIssued.Text.Trim() + " is required.");
                IsValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpLIIssued, null);
            }

            if (string.IsNullOrEmpty(dtpLIExpires.Text.Trim()))
            {
                _errorProvider.SetError(dtpLIExpires, lblLIExpires.Text.Trim() + " is required.");
                IsValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpLIExpires, null);
            }

            if (!string.IsNullOrEmpty(dtpLIExpires.Text.Trim()) && !string.IsNullOrEmpty(dtpLIIssued.Text.Trim()))
            {
                if (Convert.ToDateTime(dtpLIExpires.Text.Trim()) < Convert.ToDateTime(dtpLIIssued.Text.Trim()))
                {
                    _errorProvider.SetError(dtpLIExpires, "Expired Date cannot be greater than Issued Date");
                    IsValid = false;
                }
                else
                    _errorProvider.SetError(dtpLIExpires, null);
            }

            #endregion

            return IsValid;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);
            _errorProvider.SetError(cmbSessCouns, null);
            _errorProvider.SetError(dtpSessEndTime, null);

            HUD00003_ClientHUDForms HUD0003_Control = _baseForm.GetBaseUserControl() as HUD00003_ClientHUDForms;

            try
            {
                if (ValidateForm())
                {
                    HUDINDIVENTITY saveEntity = new HUDINDIVENTITY();

                    saveEntity.Agency = _baseForm.BaseAgency;
                    saveEntity.Dept = _baseForm.BaseDept;
                    saveEntity.Prog = _baseForm.BaseProg;
                    saveEntity.Year = _baseForm.BaseYear;

                    saveEntity.AppNo = _baseForm.BaseApplicationNo;

                    string Seq = "1";
                    if (_Mode == "INSERT")
                        Seq = "1";
                    else if (_Mode == "UPDATE")
                        Seq = txtIndivSeq.Text.Trim();

                    saveEntity.Seq = Seq;

                    saveEntity.MST_Seq = txtMstSeq.Text.Trim();

                    //Session Tab
                    saveEntity.Sess_Date = dtpSessDte.Text;
                    saveEntity.Sess_Client_Paid = txtSessClntPaid.Text;
                    saveEntity.Sess_Counselor = ((Captain.Common.Utilities.ListItem)cmbSessCouns.SelectedItem).Value.ToString().Trim();
                    if (dtpSessStartTime.Checked.Equals(true))
                        saveEntity.Sess_Start_Time = dtpSessStartTime.Value.ToString("HH:mm:ss");//txtSessStartTime.Text;
                    if (dtpSessEndTime.Checked.Equals(true))
                        saveEntity.Sess_End_Time = dtpSessEndTime.Value.ToString("HH:mm:ss");//txtSessEndTime.Text;
                    saveEntity.Sess_Status = ((Captain.Common.Utilities.ListItem)cmbSessStatus.SelectedItem).Value.ToString().Trim();
                    saveEntity.Sess_Type = ((Captain.Common.Utilities.ListItem)cmbSessType.SelectedItem).Value.ToString().Trim();
                    saveEntity.Sess_Pur_Visit = ((Captain.Common.Utilities.ListItem)cmbSessPuVisit.SelectedItem).Value.ToString().Trim();
                    saveEntity.Sess_Act_Type = ((Captain.Common.Utilities.ListItem)cmbSessActType.SelectedItem).Value.ToString().Trim();
                    saveEntity.Sess_Attr_Grant = ((Captain.Common.Utilities.ListItem)cmbSessAttGrant.SelectedItem).Value.ToString().Trim();
                    saveEntity.Sess_Grant_Used = txtSessGrantUsed.Text;
                    saveEntity.Sess_Fund = ((Captain.Common.Utilities.ListItem)cmbSessFund.SelectedItem).Value.ToString().Trim();


                    //Client Demographics
                    saveEntity.CD_HH_Month_Inc = txtCDHousMontIncome.Text;
                    saveEntity.CD_Inc_Level = ((Captain.Common.Utilities.ListItem)cmbCDIncLevel.SelectedItem).Value.ToString().Trim();
                    saveEntity.CD_App_Race = ((Captain.Common.Utilities.ListItem)cmbCDAppRace.SelectedItem).Value.ToString().Trim();
                    saveEntity.CD_App_Ethnicity = ((Captain.Common.Utilities.ListItem)cmbCDAppEthnicity.SelectedItem).Value.ToString().Trim();
                    saveEntity.CD_Lang = ((Captain.Common.Utilities.ListItem)cmbCDCounsLang.SelectedItem).Value.ToString().Trim();
                    saveEntity.CD_Ref_Source = ((Captain.Common.Utilities.ListItem)cmbCDRefSource.SelectedItem).Value.ToString().Trim();
                    saveEntity.CD_Ref_Area_Status = ((Captain.Common.Utilities.ListItem)cmbCDRurAreaStat.SelectedItem).Value.ToString().Trim();
                    saveEntity.CD_Eng_Prof_Status = ((Captain.Common.Utilities.ListItem)cmbCDLimEngProfStat.SelectedItem).Value.ToString().Trim();

                    string cd_if_applies = string.Empty;

                    if (chkbCD_1.Checked)
                        cd_if_applies = "1" + ",";
                    if (chkbCD_2.Checked)
                        cd_if_applies += "2" + ",";
                    if (chkbCD_3.Checked)
                        cd_if_applies += "3" + ",";
                    if (chkbCD_4.Checked)
                        cd_if_applies += "4" + ",";
                    if (chkbCD_5.Checked)
                        cd_if_applies += "5";

                    saveEntity.CD_If_Applies = cd_if_applies.Trim().TrimEnd(',');


                    //Client Financial Health
                    saveEntity.CFH_Depend = txtCFHNoDepend.Text;
                    saveEntity.CFH_HH_Liab = txtCFHMontHHLiab.Text;
                    saveEntity.CFH_Cred_Score = txtCFHCredScore.Text;
                    saveEntity.CFH_Source = ((Captain.Common.Utilities.ListItem)cmbCFHSource.SelectedItem).Value.ToString().Trim();
                    saveEntity.CFH_Cred_Score_Reason = ((Captain.Common.Utilities.ListItem)cmbCFHIfNoCredScore.SelectedItem).Value.ToString().Trim();
                    saveEntity.CFH_Job_Length = txtCFHLengatJob.Text;
                    saveEntity.CFH_OTR_Serv_Award = "";//((Captain.Common.Utilities.ListItem)cmbCFHBesCounslAward.SelectedItem).Value.ToString().Trim();


                    //Loan Information
                    saveEntity.LI_Bef_Loan_Type = ((Captain.Common.Utilities.ListItem)cmbLILoanType.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Bef_Loan_Reported = ((Captain.Common.Utilities.ListItem)cmbLILoanBengReported.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Bef_Mortgage_Type = ((Captain.Common.Utilities.ListItem)cmbLIMortType.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Bef_Finance_Type = ((Captain.Common.Utilities.ListItem)cmbLIFinType.SelectedItem).Value.ToString().Trim();

                    string cfh_if_applies = string.Empty;

                    if (chkb_LI_1.Checked)
                        cfh_if_applies = "1" + ",";
                    if (chkb_LI_2.Checked)
                        cfh_if_applies += "2" + ",";
                    if (chkb_LI_3.Checked)
                        cfh_if_applies += "3" + ",";
                    if (chkb_LI_4.Checked)
                        cfh_if_applies += "4" + ",";
                    if (chkb_LI_5.Checked)
                        cfh_if_applies += "5" + ",";
                    if (chkb_LI_6.Checked)
                        cfh_if_applies += "6" + ",";
                    if (chkb_LI_7.Checked)
                        cfh_if_applies += "7" + ",";
                    if (chkb_LI_8.Checked)
                        cfh_if_applies += "8";

                    saveEntity.LI_Bef_If_Applies = cfh_if_applies.Trim().TrimEnd(',');

                    saveEntity.LI_Bef_Is_Cert_HUD = ((Captain.Common.Utilities.ListItem)cmbLIHECMCert.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Bef_HECM_ID = txtLIHECMID.Text;
                    saveEntity.LI_Bef_Cert_Issued = dtpLIIssued.Text;
                    saveEntity.LI_Bef_Cert_Expires = dtpLIExpires.Text;

                    saveEntity.LI_Aft_Loan_Spouse = "";//((Captain.Common.Utilities.ListItem)cmbLISpouse.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Aft_Mortgage_Type = ((Captain.Common.Utilities.ListItem)cmbLIMortType2.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Aft_Finance_Type = ((Captain.Common.Utilities.ListItem)cmbLIFinType2.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Aft_Interest_Rate = txtLIIntRate.Text;
                    saveEntity.LI_Aft_Close_Cost = txtLIClosingCost.Text;
                    saveEntity.LI_Aft_Street = txtLIStreet.Text;
                    saveEntity.LI_Aft_Unit = txtLIUnit.Text;
                    saveEntity.LI_Aft_City = "";//((Captain.Common.Utilities.ListItem)cmbLICity.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Aft_State = "";//((Captain.Common.Utilities.ListItem)cmbLIState.SelectedItem).Value.ToString().Trim();
                    saveEntity.LI_Aft_ZIP = txtLIZIP.Text;

                    saveEntity.Add_Operator = _baseForm.UserID;
                    saveEntity.Lstc_Operator = _baseForm.UserID;

                    string strOutIndvseq = saveEntity.Seq;

                    if (_model.HUDCNTLData.InsertUpdateHUDIndiv(saveEntity, _Mode, out strOutIndvseq))
                    {
                        if (_Mode == "INSERT")
                            AlertBox.Show("Individual Session Inserted Successfully");
                        if (_Mode == "UPDATE")
                            AlertBox.Show("Individual Session Updated Successfully");

                        if (dgvImpacts.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow dr in dgvImpacts.Rows)
                            {
                                string  Imp_Mode = string.Empty;

                                if (dr.Cells["gvSelect"].Value.ToString() == "True" && dr.Cells["gvImpactSeq"].Value.ToString() == "")
                                    Imp_Mode = "INSERT";
                                else if (dr.Cells["gvSelect"].Value.ToString() == "True" && dr.Cells["gvImpactSeq"].Value.ToString() != "")
                                    Imp_Mode = "UPDATE";
                                else if (dr.Cells["gvSelect"].Value.ToString() == "False" && dr.Cells["gvImpactSeq"].Value.ToString() != "")
                                    Imp_Mode = "DELETE";


                                HUDIMPACTENTITY impact_saveEntity = new HUDIMPACTENTITY();

                                impact_saveEntity.Agency = _baseForm.BaseAgency;
                                impact_saveEntity.Dept = _baseForm.BaseDept;
                                impact_saveEntity.Prog = _baseForm.BaseProg;
                                impact_saveEntity.Year = _baseForm.BaseYear;

                                impact_saveEntity.AppNo = _baseForm.BaseApplicationNo;

                                string ImpactSeq = string.Empty;
                                if (_Mode == "INSERT")
                                {
                                    impact_saveEntity.Indv_Seq = strOutIndvseq;
                                    ImpactSeq = "1";
                                }
                                else if (_Mode == "UPDATE")
                                {
                                    if (Imp_Mode == "INSERT")
                                        ImpactSeq = "1";
                                    else
                                        ImpactSeq = dr.Cells["gvImpactSeq"].Value.ToString();
                                    impact_saveEntity.Indv_Seq = txtIndivSeq.Text;
                                }

                                impact_saveEntity.Mst_Seq = txtMstSeq.Text;

                                impact_saveEntity.Seq = ImpactSeq;

                                impact_saveEntity.Impact_Date = dtpImpactDte.Text;

                                impact_saveEntity.Impacts = dr.Cells["gvImpactID"].Value.ToString();

                                impact_saveEntity.Add_Operator = _baseForm.UserID;
                                impact_saveEntity.Lstc_Operator = _baseForm.UserID;

                                if (Imp_Mode != string.Empty)
                                {
                                    if (_model.HUDCNTLData.InsertUpdateHUDImpact(impact_saveEntity, Imp_Mode))
                                    {

                                    }
                                }
                            }

                            DialogResult = DialogResult.OK;
                            form_closed = "Close";
                            this.Close();

                            if (HUD0003_Control != null)
                            {
                                if (HUD0003_Control.rowSel == selRowIndex)
                                    HUD0003_Control.isSel = true;

                                HUD0003_Control.RefreshGrid(selRowIndex, mstIndex, false);
                            }
                        }

                        pbSessAdd.Visible = true;
                        pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = true;
                        pbSessDel.Visible = true;

                        //pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = false;
                        //btnSave.Visible = btnCancel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        string selImpacts = string.Empty;
        private string GetSelectedImpacts()
        {
            if (dgvImpacts.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in dgvImpacts.Rows)
                {
                    if (dr.Cells["gvSelect"].Value.ToString() == "True")
                    {
                        selImpacts += dr.Cells["gvSeq"].Value.ToString() + ",";
                    }
                }
            }
            return selImpacts.Trim().TrimEnd(',');
        }

        string form_closed = string.Empty;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //selRowIndex = 0;

            HUD00003_ClientHUDForms HUD0003_Control = _baseForm.GetBaseUserControl() as HUD00003_ClientHUDForms;
            if (HUD0003_Control != null)
            {
                HUD0003_Control.RefreshGrid(selRowIndex, mstIndex, true);
            }
            form_closed = "Close";
            this.Close();

            //_errorProvider.SetError(dtpSessDte, null);
            //_errorProvider.SetError(dtpImpactDte, null);
            //_errorProvider.SetError(cmbSessCouns, null);
            //_errorProvider.SetError(dtpSessEndTime, null);

            //txtIndivSeq.Text = Indiv_Seq;

            //pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = false;
            //btnSave.Visible= btnCancel.Visible = false;

            //if (_Mode == "INSERT")
            //{
            //if (hudformEntity.Count > 0)
            //{
            //fillControls();
            //if (btnSave.Visible == true && btnCancel.Visible == true)
            //{
            //    pbSessAdd.Visible = false;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            //    pbSessDel.Visible = false;
            //}
            //else
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = true;
            //    pbSessDel.Visible = true;
            //}
            //}
            //else
            //{
            //ClearControls();
            //if (btnSave.Visible == true && btnCancel.Visible == true)
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            //    pbSessDel.Visible = false;
            //}
            //else
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            //    pbSessDel.Visible = false;
            //}
            //}
            //}
            //else if (_Mode == "UPDATE")
            //{
            //fillControls();

            //if (hudformEntity.Count > 0)
            //{
            //if (btnSave.Visible == true && btnCancel.Visible == true)
            //{
            //    pbSessAdd.Visible = false;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            //    pbSessDel.Visible = false;
            //}
            //else
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = true;
            //    pbSessDel.Visible = true;
            //}
            //}
            //else
            //{
            //if (btnSave.Visible == true && btnCancel.Visible == true)
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            //    pbSessDel.Visible = false;
            //}
            //else
            //{
            //    pbSessAdd.Visible = true;
            //    pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            //    pbSessDel.Visible = false;
            //}
            //}
            //}
        }
        
        List<CaseSnpEntity> setValueEntity = new List<CaseSnpEntity>();
        private void ClearControls()
        {

            List<CaseSnpEntity> CaseSnpDetails = _model.CaseMstData.GetCaseSnpDetails(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo);
            setValueEntity = CaseSnpDetails.FindAll(x => x.Agency == _baseForm.BaseAgency && x.Dept == _baseForm.BaseDept && x.Program == _baseForm.BaseProg && x.Year.Trim() == _baseForm.BaseYear.Trim() && x.App == _baseForm.BaseApplicationNo && x.FamilySeq == _baseForm.BaseCaseMstListEntity[0].FamilySeq);

            CaseMstEntity caseMST = _model.CaseMstData.GetCaseMST(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo);

            txtIndivSeq.Text = string.Empty;

            //Session
            dtpSessDte.Value = DateTime.Now;
            txtSessClntPaid.Text = string.Empty;
            cmbSessCouns.SelectedIndex = 0;
            dtpSessStartTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            dtpSessEndTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            cmbSessStatus.SelectedIndex = 1;//0;
            cmbSessType.SelectedIndex = 0;
            cmbSessPuVisit.SelectedIndex = 0;
            cmbSessActType.SelectedIndex = 0;
            cmbSessAttGrant.SelectedIndex = 0;
            cmbSessFund.SelectedIndex = 0;
            txtSessGrantUsed.Text = string.Empty;


            //Client Demographics
            cmbCDIncLevel.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(setValueEntity[0].Race))
            {
                string RaceCode = AppRaceList.Find(x => x.Code == setValueEntity[0].Race).Code;
                SetComboBoxValue(cmbCDAppRace, RaceCode);//cmbCDAppRace.SelectedIndex = Convert.ToInt32(RaceCode);
            }
            else
                cmbCDAppRace.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(setValueEntity[0].Ethnic))
            {
                string EtnicityCode = AppEthnicityList.Find(x => x.Code == setValueEntity[0].Ethnic).Code;
                SetComboBoxValue(cmbCDAppEthnicity, EtnicityCode);
            }
            else
                cmbCDAppEthnicity.SelectedIndex = 0;

            if (caseMST != null)
            {
                if (!string.IsNullOrEmpty(caseMST.Language))
                {
                    string LangCode = LanguageList.Find(x => x.Code == caseMST.Language).Code;
                    SetComboBoxValue(cmbCDCounsLang, LangCode);//cmbCDCounsLang.SelectedIndex = Convert.ToInt32(LangCode);
                }
                else
                    cmbCDCounsLang.SelectedIndex = 0;

                if (!string.IsNullOrEmpty(caseMST.NoInProg))
                    txtCFHNoDepend.Text = caseMST.NoInProg;//string.Empty;
                else
                    txtCFHNoDepend.Text = string.Empty;

                if (!string.IsNullOrEmpty(caseMST.ProgIncome))
                {
                    Decimal MonthIncome = Convert.ToDecimal(caseMST.ProgIncome) / 12;

                    txtCDHousMontIncome.Text = (Math.Floor(MonthIncome * 100) / 100).ToString();
                }
            }
            

            cmbCDRefSource.SelectedIndex = 0;
            cmbCDRurAreaStat.SelectedIndex = 0;
            chkbCD_1.Checked = false;
            chkbCD_2.Checked = false;
            chkbCD_3.Checked = false;
            chkbCD_4.Checked = false;
            chkbCD_5.Checked = false;

            //Client Financial Health
            txtCFHMontHHLiab.Text = string.Empty;
            txtCFHCredScore.Text = string.Empty;
            cmbCFHSource.SelectedIndex = 0;
            cmbCFHIfNoCredScore.SelectedIndex = 0;
            txtCFHLengatJob.Text = string.Empty;
            cmbCFHBesCounslAward.SelectedIndex = 0;

            //Loan Information
            cmbLILoanType.SelectedIndex = 0;

            cmbLILoanBengReported.SelectedIndex = 0;
            cmbLIMortType.SelectedIndex = 0;
            cmbLILoanType.SelectedIndex = 0;

            chkb_LI_1.Checked = false;
            chkb_LI_2.Checked = false;
            chkb_LI_3.Checked = false;
            chkb_LI_4.Checked = false;
            chkb_LI_5.Checked = false;
            chkb_LI_6.Checked = false;
            chkb_LI_7.Checked = false;
            chkb_LI_8.Checked = false;

            cmbLIHECMCert.SelectedIndex = 0;
            txtLIHECMID.Text = string.Empty;
            dtpLIIssued.Value = DateTime.Now;
            dtpLIExpires.Value = DateTime.Now;

            cmbLISpouse.SelectedIndex = 0;
            cmbLIMortType2.SelectedIndex = 0;
            cmbLIFinType2.SelectedIndex = 0;
            txtLIIntRate.Text = string.Empty;
            txtLIClosingCost.Text = string.Empty;
            txtLIStreet.Text = string.Empty;
            txtLIUnit.Text = string.Empty;
            cmbLICity.SelectedIndex = 0;
            cmbLIState.SelectedIndex = 0;
            txtLIZIP.Text = string.Empty;

            //Impacts
            List<DataGridViewRow> lstuncked = dgvImpacts.Rows.Cast<DataGridViewRow>().ToList();
            lstuncked.ForEach(row => { row.Cells["gvSelect"].Value = false; });

            dtpImpactDte.Value = DateTime.Now;
        }

        #region Add/Edit/Del Click Events

        private void pbSessAdd_Click(object sender, EventArgs e)
        {
            _Mode = "INSERT";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;

            ClearControls();
        }

        private void pbSessEdit_Click(object sender, EventArgs e)
        {
            _Mode = "UPDATE";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
        }

        private void pbCDAdd_Click(object sender, EventArgs e)
        {
            _Mode = "INSERT";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;

            ClearControls();
        }

        private void pbCDEdit_Click(object sender, EventArgs e)
        {
            _Mode = "UPDATE";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
        }

        private void pbCFHAdd_Click(object sender, EventArgs e)
        {
            _Mode = "INSERT";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;

            ClearControls();
        }

        private void pbCFHEdit_Click(object sender, EventArgs e)
        {
            _Mode = "UPDATE";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
        }

        private void pbLIAdd_Click(object sender, EventArgs e)
        {
            _Mode = "INSERT";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;

            ClearControls();
        }

        private void pbLIEdit_Click(object sender, EventArgs e)
        {
            _Mode = "UPDATE";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
        }

        private void pbImpacAdd_Click(object sender, EventArgs e)
        {
            _Mode = "INSERT";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;

            ClearControls();
        }

        private void pbImpcEdit_Click(object sender, EventArgs e)
        {
            _Mode = "UPDATE";

            _errorProvider.SetError(dtpSessDte, null);
            _errorProvider.SetError(dtpImpactDte, null);

            pbSessAdd.Visible = false;
            pbSessEdit.Visible = pbCDEdit.Visible = pbCFHEdit.Visible = pbLIEdit.Visible = pbImpcEdit.Visible = false;
            pbSessDel.Visible = false;

            pnlSessParams.Enabled = pnlCDParams.Enabled = pnlCFHParams.Enabled = pnlLoanInfoParams.Enabled = pnlImpactParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
        }

        #endregion

        private void dgvImpacts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void pbSessDel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure want to delete?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
        }

        private void MessageBoxHandler(DialogResult result)
        {
            if (result == DialogResult.Yes)
            {
                HUDIMPACTENTITY delImpacts = new HUDIMPACTENTITY();

                delImpacts.Agency = _baseForm.BaseAgency;
                delImpacts.Dept = _baseForm.BaseDept;
                delImpacts.Prog = _baseForm.BaseProg;
                delImpacts.Year = _baseForm.BaseYear;

                delImpacts.AppNo = _baseForm.BaseApplicationNo;

                delImpacts.Indv_Seq = txtIndivSeq.Text;

                if (_model.HUDCNTLData.InsertUpdateHUDImpact(delImpacts, "DELETE"))
                {

                }
                string strMsg = string.Empty;
                HUDINDIVENTITY delEntity = new HUDINDIVENTITY();

                delEntity.Agency = _baseForm.BaseAgency;
                delEntity.Dept = _baseForm.BaseDept;
                delEntity.Prog = _baseForm.BaseProg;
                delEntity.Year = _baseForm.BaseYear;

                delEntity.AppNo = _baseForm.BaseApplicationNo;

                delEntity.Seq = txtIndivSeq.Text;

                if (_model.HUDCNTLData.InsertUpdateHUDIndiv(delEntity, "DELETE", out strMsg))
                {
                    //HUD00003_ClientHUDForms hierarchyControl = new HUD00003_ClientHUDForms(_baseForm, _privilegeEntity);

                    DialogResult = DialogResult.OK;
                    this.Close();

                }
            }
        }

        private void HUD00003_IndvSessDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(form_closed))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to close? Any changes made will not be saved.", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerFormClosed);

                if (result == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void MessageBoxHandlerFormClosed(DialogResult dialogResult)
        {
            //selRowIndex = 0;
            HUD00003_ClientHUDForms HUD0003_Control = _baseForm.GetBaseUserControl() as HUD00003_ClientHUDForms;

            if (dialogResult == DialogResult.Yes)
            {
                this.FormClosing -= HUD00003_IndvSessDetails_FormClosing;
                this.Close();

                HUD0003_Control.RefreshGrid(selRowIndex, mstIndex, true);
            }
        }

        private void dtpImpactDte_Leave(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpImpactDte, null);
            fillImpacts_Grid();
        }
    }
}
