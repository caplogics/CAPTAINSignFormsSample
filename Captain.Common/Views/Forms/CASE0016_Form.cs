/************************************************************************
 * Conversion On    :   01/02/2023      * Converted By     :   Kranthi
 * Modified On      :   01/02/2023      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Captain.DatabaseLayer;
using System.IO;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Painters;
using Captain.Common.Interfaces;
using DevExpress.Utils;
using System.Reflection.Emit;
using DevExpress.DataAccess.Native.ExpressionEditor;
using static Amazon.S3.Util.S3EventNotification;
#endregion


namespace Captain.Common.Views.Forms
{
    public partial class CASE0016_Form : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        string strCaseWorkerDefaultCode = string.Empty;
        string strCaseWorkerDefaultStartCode = string.Empty;
        private string strTempFolderName = string.Empty;
        private string strImageFolderName = string.Empty;
        private string strFolderName = string.Empty;
        private string strSubFolderName = string.Empty;
        private string strFullFolderName = string.Empty;
        private string strMainFolderName = string.Empty;
        private string strExtensionName = string.Empty;
        private string strDeleteEnable = string.Empty;
        private string strCheckUploadMode = string.Empty;

        public CASE0016_Form(BaseForm baseForm, string mode, string sp_code, string sp_sequence, string spm_year, string strsource, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            SP_Code = sp_code;
            Sp_Sequence = sp_sequence;
            Spm_Year = spm_year;
            Source = strsource;

            this.Text = "Benefit Service Plan Selection" + " - " + Mode;

            //lblGas.Text = Source+ " Supplier Details";


            //this.Text = privilegeEntity.Program + " - " + Mode;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            propReportPath = _model.lookupDataAccess.GetReportPath();

            txtAmount.Validator = TextBoxValidation.FloatValidator;
           // txtBalance.Validator = TextBoxValidation.FloatValidator;

            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }
            propSearch_Entity = _model.SPAdminData.Browse_CASESP0List(null, null, null, null, null, null, null, null, null);
            propfundingsource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);
            commonReasonlist = CommonFunctions.AgyTabsFilterOrderbyCode(BaseForm.BaseAgyTabsEntity, "S0133", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, BaseForm.BaseYear, string.Empty, string.Empty);

            Vulner_Flag = Get_SNP_Vulnerable_Status();

            Poverty = BaseForm.BaseCaseMstListEntity[0].Poverty.Trim();

            if (Vulner_Flag) lblVulNonvul.Text = "Vulnerable Household"; else lblVulNonvul.Text = "Non-Vulnerable Household";

            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC")
            {
                //lblFrstApp.Visible = true;
                //cmbFrstApp.Visible = true;
                //label9.Visible = true;

                //Fill_FirstApp_Comb();
            }
            else
            {
                lblFrstApp.Visible = false;
                cmbFrstApp.Visible = false;
                label9.Visible = false;
            }
            

            Fill_Applicant_SPs();
            FillCombo();
            Fill_SP_DropDowns();
            fillFundCombo();
            Get_Vendor_List();

            //Txt_Income.Text = BaseForm.BaseCaseMstListEntity[0].FamIncome.Trim();
            Txt_Income.Text = BaseForm.BaseCaseMstListEntity[0].ProgIncome.Trim();
            Txt_HSize.Text = BaseForm.BaseCaseMstListEntity[0].NoInhh.Trim();
            if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim()))
            {
                if(BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC=="Y")
                    Txt_OMB.Text = BaseForm.BaseCaseMstListEntity[0].Poverty.Trim();
                else
                    Txt_OMB.Text = Math.Abs((int) decimal.Parse(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim())).ToString();
            }
            //Txt_OMB.Text = BaseForm.BaseCaseMstListEntity[0].Poverty.Trim();
            txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1.Trim();

            strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
            strTempFolderName = _model.lookupDataAccess.GetReportPath() + "\\Temp\\Invoices\\" + strFolderName; ; //"\\\\cap-dev\\C-Drive\\CapSystemsimages\\Temp"; //"C:\\CapsystemsImages\\Temp";//
            string strYear = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim())) strYear = BaseForm.BaseYear; else strYear = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            strImageFolderName = _model.lookupDataAccess.GetReportPath() + "\\INVDOCS\\" +strYear+"\\"+ strFolderName;//

            if (Mode == "Add")
            {
                dtStardt.Text= DateTime.Now.ToShortDateString();
            }

            if (Mode == "Edit")
            {
                FillBenefit_Controls();
                btnRecal.Visible = true;
                if (custResponses.Count > 0)
                {
                    CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
                    if (responsetot != null)
                    {
                        if (!string.IsNullOrEmpty(responsetot.USAGE_PRIM_VEND.Trim()))
                        {
                            //Txt_VendNo.Text = responsetot.USAGE_PRIM_VEND;
                            //Text_VendName.Text = Get_Vendor_Name(responsetot.USAGE_PRIM_VEND);
                            //CommonFunctions.SetComboBoxValue(cmbPrimSource, responsetot.USAGE_PSOURCE);
                        }
                        else
                        {
                            txtAccountNo.Enabled = false; cmbBilling.Enabled = false; panel_Referral2.Enabled = false; cmbSecSource.Enabled = false;
                        }

                        if (!string.IsNullOrEmpty(responsetot.USAGE_SEC_VEND.Trim()))
                        {
                            //Txt_GasVendNo.Text = responsetot.USAGE_SEC_VEND;
                            //Text_GasVendName.Text = Get_Vendor_Name(responsetot.USAGE_SEC_VEND);
                            //CommonFunctions.SetComboBoxValue(cmbSecSource, responsetot.USAGE_SSOURCE);
                        }
                        else
                        {
                            txtGasAccountNo.Enabled = false; cmbGasBilling.Enabled = false; panel_ReferralGas.Enabled = false; cmbSecSource.Enabled = false;
                        }
                    }
                }

            }
            else
            {
                if (custResponses.Count > 0)
                {
                    CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
                    if (responsetot != null)
                    {
                        if (!string.IsNullOrEmpty(responsetot.USAGE_PRIM_VEND.Trim()))
                        {
                            Txt_VendNo.Text = responsetot.USAGE_PRIM_VEND;
                            Text_VendName.Text = Get_Vendor_Name(responsetot.USAGE_PRIM_VEND);
                            CommonFunctions.SetComboBoxValue(cmbPrimSource, responsetot.USAGE_PSOURCE);
                        }
                        else
                        {
                            txtAccountNo.Enabled = false; cmbBilling.Enabled = false; panel_Referral2.Enabled = false; cmbPrimSource.Enabled = false;
                        }

                        if (!string.IsNullOrEmpty(responsetot.USAGE_SEC_VEND.Trim()))
                        {
                            Txt_GasVendNo.Text = responsetot.USAGE_SEC_VEND;
                            Text_GasVendName.Text = Get_Vendor_Name(responsetot.USAGE_SEC_VEND);
                            CommonFunctions.SetComboBoxValue(cmbSecSource, responsetot.USAGE_SSOURCE);
                        }
                        else
                        {
                            txtGasAccountNo.Enabled = false; cmbGasBilling.Enabled = false; panel_ReferralGas.Enabled = false; cmbSecSource.Enabled = false;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim()))
            {
                if (decimal.Parse(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim()) >= 150)
                {
                    if (Mode == "Add")
                    {
                        if (((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() != "E" && ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() != "S" && ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() != "M" && ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() != "N")
                            CommonFunctions.SetComboBoxValue(cmbEligStatus, "D");
                        lblOverInc.Visible = true;
                    }
                }
                else lblOverInc.Visible = false;
            }


        }

        List<CommonEntity> firstApp = new List<CommonEntity>();
        private void Fill_FirstApp_Comb()
        {
            cmbFrstApp.Items.Clear();
            firstApp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0001", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            cmbFrstApp.Items.Add(new Captain.Common.Utilities.ListItem("", "0"));
            foreach (CommonEntity entity in firstApp)
            {
                cmbFrstApp.Items.Add(new Captain.Common.Utilities.ListItem(entity.Desc, entity.Code));
            }

            if (cmbFrstApp.Items.Count > 0)
            {
                //cmbFrstApp.SelectedIndex = 0;
                CommonFunctions.SetComboBoxValue(cmbFrstApp, "Y");
            }
        }

        List<CASESP0Entity> propSearch_Entity { get; set; }
        List<CommonEntity> propfundingsource { get; set; }
        List<CMBDCEntity> Emsbdc_List { get; set; }
        List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
        List<CEAPCNTLEntity> CEAPCNTL_List = new List<CEAPCNTLEntity>();
        List<CommonEntity> commonReasonlist = new List<CommonEntity>();
        string ACR_SERV_Hies = string.Empty;
        #region Properties
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }
        public string Source { get; set; }

        public string Poverty { get; set; }

        public bool IsSaveValid { get; set; }
        public string SP_Code { get; set; }
        public string Sp_Sequence { get; set; }
        public string propReportPath { get; set; }

        public string Spm_Year { get; set; }

        public CASESPMEntity SPM_Entity { get; set; }

        #endregion
        List<HierarchyEntity> hierarchyEntity = new List<HierarchyEntity>();
        List<CASESP2Entity> SP_CAMS_Details = new List<CASESP2Entity>();
        private void FillCombo()
        {

            cmbcaseworker.Items.Clear();
            cmbcaseworker.ColorMember = "FavoriteColor";
            hierarchyEntity = _model.CaseMstData.GetCaseWorker(BaseForm.BaseHierarchyCwFormat.ToString(), BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (Mode.ToUpper() == "ADD")
            {
                hierarchyEntity = hierarchyEntity.FindAll(u => u.InActiveFlag == "N").ToList();
            }
            string strCaseworker = string.Empty;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {
                if (strCaseworker != caseworker.CaseWorker.ToString())
                {
                    strCaseworker = caseworker.CaseWorker.ToString();
                    cmbcaseworker.Items.Add(new Utilities.ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Black : Color.Red));
                }
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseWorkerDefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                }

            }
            cmbcaseworker.Items.Insert(0, new Utilities.ListItem(" ", "0"));
            cmbcaseworker.SelectedIndex = 0;
            if (Mode.Equals(Consts.Common.Add))
                CommonFunctions.SetComboBoxValue(cmbcaseworker, strCaseWorkerDefaultCode);
            else
                CommonFunctions.SetComboBoxValue(cmbcaseworker, "0");

            cmbBenfitReason.Items.Clear();
            cmbBenfitReason.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "0"));
            cmbBenfitReason.SelectedIndex = 0;
            foreach (CommonEntity reasonlist in commonReasonlist)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(reasonlist.Desc, reasonlist.Code);
                cmbBenfitReason.Items.Add(li);
            }

            CmbSite.Items.Clear();
            CmbSite.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem1 = new List<Captain.Common.Utilities.ListItem>();
            listItem1.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));

            //DataSet ds = Captain.DatabaseLayer.Lookups.GetCaseSite();
            DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, string.Empty, string.Empty);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                if (Mode.Equals("Add"))
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "SITE_ACTIVE='Y'";
                    dt = dv.ToTable();
                }
            }

            foreach (DataRow dr in dt.Rows)
                listItem1.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString().Trim(), dr["SITE_ACTIVE"].ToString().Trim(), (dr["SITE_ACTIVE"].ToString().Trim().Equals("Y") ? Color.Black : Color.Red)));

            //CmbSite.Items.Add(new Captain.Common.Utilities.ListItem(" ", "0"," ", Color.White));

            if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
            {
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                HierarchyEntity hierEntity = new HierarchyEntity(); List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                foreach (HierarchyEntity Entity in userHierarchy)
                {
                    if (Entity.InActiveFlag == "N")
                    {
                        if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg)
                            hierEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                            hierEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        { hierEntity = null; }
                    }
                }

                if (hierEntity != null)
                {
                    if (hierEntity.Sites.Length > 0)
                    {
                        string[] Sites = hierEntity.Sites.Split(',');
                        List<Captain.Common.Utilities.ListItem> listItemSite = new List<Captain.Common.Utilities.ListItem>();
                        listItemSite.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));
                        for (int i = 0; i < Sites.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                            {
                                foreach (Captain.Common.Utilities.ListItem casesite in listItem1) //Site_List)//ListcaseSiteEntity)
                                {
                                    if (Sites[i].ToString() == casesite.Value.ToString())
                                    {
                                        listItemSite.Add(casesite);
                                        //break;
                                    }

                                }
                            }
                        }

                        listItem1 = listItemSite;
                    }
                }
            }


            CmbSite.Items.AddRange(listItem1.ToArray());
            CmbSite.SelectedIndex = 0;


            List<Utilities.ListItem> EligStatusList = new List<Utilities.ListItem>();
            EligStatusList.Clear();
            EligStatusList.Add(new Utilities.ListItem("    ", "0"));
            EligStatusList.Add(new Utilities.ListItem("Denied", "D"));
            if (custResponses.Count > 0)
            {
                EligStatusList.Add(new Utilities.ListItem("Eligible", "E"));
                EligStatusList.Add(new Utilities.ListItem("Pending", "P"));
                

                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim()))
                {
                    if (decimal.Parse(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim()) > 150)
                    {
                        EligStatusList.Add(new Utilities.ListItem("SSI Categorical", "S"));
                        EligStatusList.Add(new Utilities.ListItem("Means Tested Veterans Categorical", "M"));
                        EligStatusList.Add(new Utilities.ListItem("SNAP Categorical", "N"));
                    }
                }
            }

            cmbEligStatus.Items.Clear();
            foreach (Utilities.ListItem List in EligStatusList)
                cmbEligStatus.Items.Add(new Utilities.ListItem(List.Text, List.Value));

            if (Mode == "Add") cmbEligStatus.SelectedIndex = 0;


            cmbBilling.SelectedIndexChanged -= new EventHandler(cmbBilling_SelectedIndexChanged);
            cmbBilling.Items.Add(new Utilities.ListItem("   ", "0"));
            cmbBilling.SelectedIndex = 0;
            int rowIndex = 0;
            foreach (CaseSnpEntity item in BaseForm.BaseCaseSnpEntity)
            {
                rowIndex++;
                if (item.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq)
                    cmbBilling.Items.Add(new Utilities.ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "A"));
                else
                    cmbBilling.Items.Add(new Utilities.ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "M"));
            }
            cmbBilling.Items.Add(new Utilities.ListItem("3rd Party Billing", "T", "T", "T"));
            cmbBilling.SelectedIndexChanged += new EventHandler(cmbBilling_SelectedIndexChanged);


            cmbGasBilling.SelectedIndexChanged -= new EventHandler(cmbGasBilling_SelectedIndexChanged);
            cmbGasBilling.Items.Add(new Utilities.ListItem("   ", "0"));
            cmbGasBilling.SelectedIndex = 0;
            int rowIndex1 = 0;
            foreach (CaseSnpEntity item in BaseForm.BaseCaseSnpEntity)
            {
                rowIndex1++;
                if (item.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq)
                    cmbGasBilling.Items.Add(new Utilities.ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "A"));
                else
                    cmbGasBilling.Items.Add(new Utilities.ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "M"));
            }
            cmbGasBilling.Items.Add(new Utilities.ListItem("3rd Party Billing", "T", "T", "T"));
            cmbGasBilling.SelectedIndexChanged += new EventHandler(cmbGasBilling_SelectedIndexChanged);


            DataSet dsSource = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB("08004");
            DataTable dtSource = new DataTable();
            if (dsSource.Tables.Count > 0)
                dtSource = dsSource.Tables[0];

            List<Utilities.ListItem> listItem = new List<Utilities.ListItem>();
            listItem.Add(new Utilities.ListItem("   ", "0"));
            foreach (DataRow dr in dtSource.Rows)
            {
                listItem.Add(new Utilities.ListItem(dr["LookUpDesc"].ToString().Trim(), dr["Code"].ToString().Trim()));
            }
            cmbPrimSource.Items.AddRange(listItem.ToArray());
            cmbPrimSource.SelectedIndex = 0;

            cmbSecSource.Items.AddRange(listItem.ToArray());
            cmbSecSource.SelectedIndex = 0;

        }

        private void fillFundCombo()
        {
            cmbFundsource.SelectedIndexChanged -= new EventHandler(cmbFundsource_SelectedIndexChanged);
            List<CommonEntity> commonfundingsource = propfundingsource;

            commonfundingsource = filterByHIE(commonfundingsource, Mode);
            cmbFundsource.Items.Clear();
            cmbFundsource.ColorMember = "FavoriteColor";

            CASESP0Entity casesp0data = new CASESP0Entity();

            if (CmbSP.Items.Count > 0)
                casesp0data = propSearch_Entity.Find(u => u.Code == ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString() && u.Funds != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code.Trim() == string.Empty);

            bool Istrue = false;
            foreach (CommonEntity item in commonfundingsource)
            {
                Istrue = false;
                if (casesp0data != null)
                {
                    if (casesp0data.Funds.Contains(item.Code)) Istrue = true;
                }
                if (Istrue)
                    cmbFundsource.Items.Add(new Utilities.ListItem(item.Desc.ToString(), item.Code.ToString(), item.Active.ToString(), (item.Active.Equals("Y") ? Color.Black : Color.Red), item.Default.ToString(), string.Empty));
            }
            cmbFundsource.Items.Insert(0, new Utilities.ListItem("  ", "0"));
            cmbFundsource.SelectedIndex = 0;
            cmbFundsource.SelectedIndexChanged += new EventHandler(cmbFundsource_SelectedIndexChanged);

        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues, string Mode)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = LookupValues;
            if (LookupValues.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => (u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                }
                else
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            return _AgytabsFilter;
        }

        private void Fill_SP_DropDowns()
        {
            this.CmbSP.SelectedIndexChanged -= new System.EventHandler(this.CmbSP_SelectedIndexChanged);
            CmbSP.Items.Clear();
            List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();
            ACR_SERV_Hies = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }

            if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            {
                if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, BaseForm.BaseDept, null, BaseForm.UserID);
                else
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, null, null, BaseForm.UserID);
            }
            else
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);

            CmbSP.ColorMember = "FavoriteColor"; int SelIndex = 0;

            //string strcmbFundsource = ((ListItem)cmbFundsource.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbFundsource.SelectedItem).Value.ToString();
            if (SP_Hierarchies.Count > 0)
            {
                bool SP_Exists = false, Allow_Dups = false;
                string Tmp_SP_Desc = null, Tmp_SP_Code = null, SP_Valid = null, SPM_Start_Date = " ", SP_DESC = " ", spm_posting_year = "";
                int Tmp_Sel_Index = 0, Itr_Index = 0, lst_index = 0;

                if (Mode.Equals("Edit"))
                {
                    foreach (CASESPMEntity Entity1 in CASESPM_SP_List)
                    {
                        SP_DESC = " "; SP_Exists = false;

                        CASESP1Entity casesp1data = SP_Hierarchies.Find(u => u.Code == Entity1.service_plan);
                        if (casesp1data != null)
                        {
                            SP_Exists = true;
                            if (Entity1.Sp0_Validatetd.ToUpper() == "Y" && Entity1.Sp0_Active.ToUpper() == "Y")
                            {
                                SP_Valid = "Y";
                            }
                            else
                            {
                                SP_Valid = "N";
                            }
                            SPM_Start_Date = Entity1.startdate;
                            SP_DESC = casesp1data.SP_Desc;

                            if (casesp1data.SP_Allow_Dups == "Y")
                                Allow_Dups = true;
                        }

                        if ((Mode.Equals("Edit") && SP_Exists))
                        {
                            Tmp_SP_Code = "000000".Substring(0, (6 - Entity1.service_plan.Length)) + Entity1.service_plan;

                            if (SP_Code == Tmp_SP_Code && Entity1.Seq == Sp_Sequence && Spm_Year == Entity1.year)
                                Tmp_Sel_Index = Itr_Index;

                            if (!string.IsNullOrEmpty(SPM_Start_Date.Trim()))
                                SPM_Start_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(SPM_Start_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                            else
                                SPM_Start_Date = " ";

                            spm_posting_year = "";
                            spm_posting_year = (string.IsNullOrEmpty(Entity1.year.ToString().Trim()) ? "" : " - PY" + Entity1.year.ToString().Trim());

                            //if(Mode.Equals("Add"))
                            //    CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Green : Color.Red)));
                            //else
                            CASESP0Entity casesp0data = propSearch_Entity.Find(u => u.Code == Entity1.service_plan && u.Funds != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code.Trim() == string.Empty);
                            if (casesp0data != null)
                            {
                                CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + SPM_Start_Date + " - " + SP_DESC.Trim() + spm_posting_year, Entity1.service_plan.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red), Entity1.Seq, Entity1.year));
                                Itr_Index++;
                            }

                        }
                    }

                    if (CmbSP.Items.Count > 0)
                    {


                        CmbSP.SelectedIndex = Tmp_Sel_Index;

                        SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString(), ACR_SERV_Hies);
                    }
                }



                //string Fund = ((ListItem)cmbFundsource.SelectedItem).Value.ToString();

                if (Mode.Equals("Add"))
                {
                    CmbSP.Items.Add(new Captain.Common.Utilities.ListItem("", "00"));

                    foreach (CASESP1Entity Entity in SP_Hierarchies)  // 08122012
                    {
                        SP_Exists = Allow_Dups = false;
                        Tmp_SP_Desc = null;
                        // SP_Valid = Entity.SP_validated;
                        SPM_Start_Date = " ";
                        if (Entity.SP_validated.ToUpper() == "Y" && Entity.Sp0_Active.ToUpper() == "Y")
                        {
                            SP_Valid = "Y";
                        }
                        else
                        {
                            SP_Valid = "N";
                        }
                        foreach (CASESPMEntity Entity1 in CASESPM_SP_List)
                        {
                            if (Entity1.service_plan == Entity.Code)
                            {
                                SP_Exists = true;
                                if (Entity1.Sp0_Validatetd.ToUpper() == "Y" && Entity1.Sp0_Active.ToUpper() == "Y")
                                {
                                    SP_Valid = "Y";
                                }
                                else
                                {
                                    SP_Valid = "N";
                                }
                                SPM_Start_Date = Entity1.startdate;

                                if (Entity.SP_Allow_Dups == "Y")
                                    Allow_Dups = true;

                                break;
                            }
                        }

                        if (SP_Valid.ToUpper() == "Y")
                        {
                            if ((Mode.Equals("Add") && !SP_Exists) || (Mode.Equals("Add") && Allow_Dups))// || (Mode.Equals("Edit") && SP_Exists))
                            {
                                Tmp_SP_Code = "000000".Substring(0, (6 - Entity.Code.Length)) + Entity.Code;

                                if (propSearch_Entity.Count > 0)
                                {
                                    CASESP0Entity casesp0data = propSearch_Entity.Find(u => u.Code == Entity.Code && u.Funds.Trim() != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code.Trim() == string.Empty);
                                    if (casesp0data != null)
                                    {
                                        if (casesp0data.Sp0ReadOnly != "Y" && casesp0data.NoSPM == "Y")
                                        {
                                            CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red)));
                                            lst_index++;

                                            if (Vulner_Flag)
                                            {
                                                if (CEAPCNTL_List[0].CPCT_VUL_SP.Trim() == Entity.Code.Trim())
                                                    SelIndex = lst_index;
                                            }
                                            else if (!Vulner_Flag)
                                            {
                                                if (CEAPCNTL_List[0].CPCT_NONVUL_SP.Trim() == Entity.Code.Trim())
                                                    SelIndex = lst_index;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.CmbSP.SelectedIndexChanged += new System.EventHandler(this.CmbSP_SelectedIndexChanged);
            //if (Mode.Equals("Add") && CmbSP.Items.Count == 1)
            if (CmbSP.Items.Count > 0)
            {
                if (Mode == "Add")
                {
                    if (CEAPCNTL_List.Count > 0)
                    {
                        if (Vulner_Flag)
                        {
                            if (SelIndex > 0) { CmbSP.SelectedIndex = SelIndex; CmbSP.Enabled = false; }
                            else CmbSP.SelectedIndex = 0;

                            //CommonFunctions.SetComboBoxValue(CmbSP, CEAPCNTL_List[0].CPCT_VUL_SP);
                        }
                        else if (!Vulner_Flag)
                        {
                            if (SelIndex > 0) { CmbSP.SelectedIndex = SelIndex; CmbSP.Enabled = false; }
                            else CmbSP.SelectedIndex = 0;
                            //CommonFunctions.SetComboBoxValue(CmbSP, CEAPCNTL_List[0].CPCT_NONVUL_SP);
                        }
                        else CmbSP.SelectedIndex = 0;
                    }
                    else CmbSP.SelectedIndex = 0;

                    GetMaxBenfit();
                }
                else
                    CmbSP.SelectedIndex = 0;

                //GetMaxBenfit();
            }

        }

        CASESP0Entity SP_Header_Rec;
        List<CASESPMEntity> CASESPM_SP_List;
        CASESPMEntity Search_Entity = new CASESPMEntity();
        private void Fill_Applicant_SPs()
        {
            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;
            //Search_Entity.year = BaseYear;
            Search_Entity.year = null;                          // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = BaseForm.BaseApplicationNo;

            Search_Entity.service_plan = Search_Entity.caseworker = Search_Entity.site = null;
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = Search_Entity.Def_Program = //Search_Entity.SPM_MassClose =
            Search_Entity.SPM_MassClose = Search_Entity.Seq = Search_Entity.Bulk_Post = null;

            CASESPM_SP_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Search_Entity.Def_Program = Search_Entity.caseworker = Search_Entity.site = Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;

            //if (Mode.Equals("Add"))
            //{
            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;
            //Search_Entity.year = Year;                             
            //Search_Entity.year = "    ";                             // Year will be always Four-Spaces in CASESPM 
            Search_Entity.year = BaseForm.BaseYear;
            Search_Entity.app_no = BaseForm.BaseApplicationNo;
            Search_Entity.Bulk_Post = "N";

            if (!string.IsNullOrEmpty(CmbSP.Text))
                Search_Entity.service_plan = ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(txtAmount.Text.Trim()))
                Search_Entity.SPM_Balance = txtAmount.Text.Trim();
            //}

            if (Mode.Equals("Edit"))
                Search_Entity.Seq = ((((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).DefaultValue.ToString()));

            if (!string.IsNullOrEmpty(cmbcaseworker.Text))
                Search_Entity.caseworker = ((Captain.Common.Utilities.ListItem)cmbcaseworker.SelectedItem).Value.ToString();
            if (!string.IsNullOrEmpty(CmbSite.Text))
                Search_Entity.site = ((Captain.Common.Utilities.ListItem)CmbSite.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(cmbFundsource.Text))
                Search_Entity.SPM_Fund = ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(cmbEligStatus.Text))
                Search_Entity.SPM_EligStatus = ((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(cmbBudget.Text))
                Search_Entity.SPM_BDC_ID = ((Captain.Common.Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(txtAmount.Text.Trim()))
                Search_Entity.SPM_Amount = txtAmount.Text.Trim();

            if (!string.IsNullOrEmpty(Txt_VendNo.Text.Trim()))
                Search_Entity.SPM_Vendor = Txt_VendNo.Text.Trim();
            if (!string.IsNullOrEmpty(txtAccountNo.Text.Trim()))
                Search_Entity.SPM_Account = txtAccountNo.Text.Trim();
            if (!string.IsNullOrEmpty(cmbBilling.Text))
                Search_Entity.SPM_BillName_Type = ((Captain.Common.Utilities.ListItem)cmbBilling.SelectedItem).ValueDisplayCode.ToString();
            if (!string.IsNullOrEmpty(txtFirst.Text.Trim()))
                Search_Entity.SPM_Bill_FName = txtFirst.Text.Trim();
            if (!string.IsNullOrEmpty(txtLast.Text.Trim()))
                Search_Entity.SPM_Bill_LName = txtLast.Text.Trim();

            if (!string.IsNullOrEmpty(Txt_GasVendNo.Text.Trim()))
                Search_Entity.SPM_Gas_Vendor = Txt_GasVendNo.Text.Trim();
            if (!string.IsNullOrEmpty(txtGasAccountNo.Text.Trim()))
                Search_Entity.SPM_Gas_Account = txtGasAccountNo.Text.Trim();
            if (!string.IsNullOrEmpty(cmbGasBilling.Text))
                Search_Entity.SPM_Gas_BillName_Type = ((Captain.Common.Utilities.ListItem)cmbGasBilling.SelectedItem).ValueDisplayCode.ToString();
            if (!string.IsNullOrEmpty(txtGasFirst.Text.Trim()))
                Search_Entity.SPM_Gas_Bill_FName = txtGasFirst.Text.Trim();
            if (!string.IsNullOrEmpty(txtGasLast.Text.Trim()))
                Search_Entity.SPM_Gas_Bill_LName = txtGasLast.Text.Trim();

            //if (!string.IsNullOrEmpty(cmbFrstApp.Text))
            //    Search_Entity.SPM_FIRST_APP = ((Captain.Common.Utilities.ListItem)cmbFrstApp.SelectedItem).Value.ToString();


            if (!string.IsNullOrEmpty(cmbPrimSource.Text))
                Search_Entity.SPM_Prim_Source = ((Captain.Common.Utilities.ListItem)cmbPrimSource.SelectedItem).Value.ToString();
            if (!string.IsNullOrEmpty(cmbBilling.Text))
                Search_Entity.SPM_Sec_Source = ((Captain.Common.Utilities.ListItem)cmbSecSource.SelectedItem).Value.ToString();


            if (!string.IsNullOrEmpty(Txt_SPM_Program.Text))
                Search_Entity.Def_Program = Txt_SPM_Program.Text.Trim().Substring(0, 6);

            if (dtStardt.Checked)
                Search_Entity.startdate = dtStardt.Value.ToShortDateString();
            //if (Est_Date.Checked)
            //    Search_Entity.estdate = Est_Date.Value.ToShortDateString();
            //if (Act_Date.Checked)
            //{
            //    Search_Entity.compdate = Act_Date.Value.ToShortDateString();
            //    Search_Entity.SPM_MassClose = Search_Entity.SPM_MassClose;
            //}
            //else
            Search_Entity.SPM_MassClose = "N";

            Search_Entity.sel_branches = "P";


            Search_Entity.lstc_operator = BaseForm.UserID;

            //if (Mode.Equals("Add"))
            Insert_Sel_SP_Details();
            //else
            //    Update_Sel_SP_Details();

        }

        string Sql_SP_Result_Message = string.Empty; string Tmp_SPM_Sequence = "1"; bool Refresh_Control = false;
        private void Insert_Sel_SP_Details()
        {
            if (Validate_ADD_Mode())
            {
                bool boolsucess = true;
                decimal decamount = 0;
                decimal decbal = 0;
                if (txtAmount.Text.Trim() != string.Empty)
                    decamount = Convert.ToDecimal(txtAmount.Text);
                if (txtBalance1.Text.Trim() != string.Empty)
                    decbal = Convert.ToDecimal(txtBalance1.Text);



                if (Mode.Equals("Edit"))
                {
                    CASESPMEntity SPMRec = new CASESPMEntity();
                    if (CASESPM_SP_List.Count > 0)
                        SPMRec = CASESPM_SP_List.Find(u => u.service_plan == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim());
                    if (SPMRec != null)
                        decbal = decbal + Convert.ToDecimal(SPMRec.SPM_Amount == string.Empty ? "0" : SPMRec.SPM_Amount);
                    //decbal = decbal + Convert.ToDecimal(CASESPM_SP_List[0].SPM_Amount);
                }
                //Commented by Sudheer on 09/11/24 based on the STDC.Docx
                //if (decamount > decbal)
                //{
                //    CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this Benefit \n Amount may not be more than " + decbal);
                //    boolsucess = false;
                //    btnSave.Enabled = true;
                //}
                //else
                //{

                //}
                if (Mode.Equals("Edit"))
                {
                    decimal decResAmount = 0;
                    List<CASESPMEntity> CASESPM_List = new List<CASESPMEntity>();

                    if (CASESPM_SP_List.Count > 0)
                    {
                        CASESPM_List = CASESPM_SP_List.FindAll(u => u.service_plan == SP_Code && u.Seq == Sp_Sequence);
                    }

                    if (SP_Activity_Details.Count > 0)
                    {
                        List<CASEACTEntity> CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && u.Elec_Other != string.Empty);
                        if (CaseactList.Count > 0)
                            decResAmount = CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                    }

                    //List<CASESPMEntity> EmsRes_List = _model.EMSBDCData.GetEmsresAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), BaseForm.BaseApplicationNo, propEmsresEntity.EMSRES_FUND, propEmsresEntity.EMSRES_SEQ, propEmsresEntity.EMSRES_DATE, "RESAMOUNT");
                    //if (CASESPM_List.Count > 0)
                    //{
                    //    decResAmount = Convert.ToDecimal(CASESPM_List[0].SPM_Amount);
                    //}
                    if (decamount < decResAmount)
                    {
                        CommonFunctions.MessageBoxDisplay("Amount Should be more than " + decResAmount);
                        boolsucess = false;
                        btnSave.Enabled = true;
                        return;
                    }

                }

                if (boolsucess)
                {
                    decimal decBdcBalance = 0;
                    if (((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "E" || ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "S" || ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "M" || ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "N")
                    {
                        string strcmbBudget = ((Utilities.ListItem)cmbBudget.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();
                        List<CMBDCEntity> PropBDCList = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);
                        CMBDCEntity CMbdcentity = PropBDCList.Find(u => u.BDC_FUND == ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() && (Convert.ToDateTime(u.BDC_START) <= dtStardt.Value && Convert.ToDateTime(u.BDC_END) >= dtStardt.Value) && u.BDC_ID == strcmbBudget);
                        if (CMbdcentity != null)
                        {
                            //if ((emsbdcentity.BDC_LOCK_BY == string.Empty && emsbdcentity.BDC_LOCK_ON == string.Empty) || (emsbdcentity.BDC_LOCK_BY == BaseForm.UserID && emsbdcentity.BDC_LOCK_SCREEN == "RESOURCE"))
                            //{
                            decBdcBalance = Convert.ToDecimal(CMbdcentity.BDC_BALANCE);
                            //}
                        }
                    }


                    if (txtBalance1.Text.Trim() != string.Empty)
                        decBdcBalance = Convert.ToDecimal(txtBalance1.Text);


                    if (!string.IsNullOrEmpty(txtBalance1.Text))
                    {
                        if (Convert.ToDecimal(txtBalance1.Text) != decBdcBalance)
                        {
                            txtBalance1.Text = decBdcBalance.ToString();
                            if (txtAmount.Text.Trim() != string.Empty)
                                decamount = Convert.ToDecimal(txtAmount.Text);
                            if (txtBalance1.Text.Trim() != string.Empty)
                                decbal = Convert.ToDecimal(txtBalance1.Text);
                            if (Mode.Equals("Edit"))
                            {
                                decbal = decbal + Convert.ToDecimal(SPM_Entity.SPM_Amount);
                            }
                            if (decamount > decbal)
                            {
                                CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this resource account \n Amount may not be more than " + decbal);
                                boolsucess = false;
                            }
                        }
                    }
                    if (Mode == "Edit")
                    {
                        if (SP_Activity_Details.Count > 0)
                        {
                            List<CASEACTEntity> CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && u.Elec_Other.Trim() != string.Empty);
                            if (CaseactList.Count > 0)
                            {
                                decimal decpmcresamount = CaseactList.Sum(u => Convert.ToDecimal(u.Cost.Trim()));
                                decpmcresamount = Convert.ToDecimal(txtAmount.Text) - decpmcresamount;
                                Search_Entity.SPM_Balance = decpmcresamount.ToString();
                            }
                        }

                    }

                    if ((((Utilities.ListItem)cmbBenfitReason.SelectedItem).Value.ToString().Trim()) != "0")
                    {
                        Search_Entity.Spm_Benefit_Reasn = ((Utilities.ListItem)cmbBenfitReason.SelectedItem).Value.ToString().Trim();
                    }
                    else
                    {
                        Search_Entity.Spm_Benefit_Reasn = string.Empty;
                    }

                    if (Mode.Equals("Add")) Search_Entity.Rec_Type = "I"; else Search_Entity.Rec_Type = "U";


                    if (_model.SPAdminData.UpdateCASESPM(Search_Entity, "Insert", out Sql_SP_Result_Message, out Tmp_SPM_Sequence))
                    {
                        //Update_AutoPost_CAMS(Tmp_SPM_Sequence);
                        ////MessageBox.Show("Service Plan Posting Successful", "CAP Systems");
                        SP_Code = "000000".Substring(0, (6 - Search_Entity.service_plan.Length)) + Search_Entity.service_plan;
                        Spm_Year = Search_Entity.year;
                        Sp_Sequence = Tmp_SPM_Sequence;
                        //Switch_To_Edit_Mode();
                        Refresh_Control = true;
                        //this.DialogResult = DialogResult.OK;
                        //this.Close();

                        

                        //Added by Sudheer on 02/07/25 as per the CCSCT.docx
                        if (Mode == "Add")
                        {

                            SPM_Entity = Search_Entity;
                            SPM_Entity.Seq = Sp_Sequence;


                            //Added by sudheer on 12/04/24 to save the uploaded Invoices
                            if (!string.IsNullOrEmpty(lblPInvFileName.Text.Trim()))
                            {
                                UploadLogPdf("ADD", string.Empty, "P");
                            }

                            if (!string.IsNullOrEmpty(lblSInvFileName.Text.Trim()))
                            {
                                UploadLogPdf("ADD", string.Empty, "S");
                            }
                        }

                        Get_App_CASEACT_List(Search_Entity);

                        //added by Sudheer on 1/19/22 for auto add Activity record with SPM.
                        SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(Search_Entity.service_plan, null, null, null, "CASE4006");

                       // Get_App_CASEACT_List(Search_Entity);

                        if (SP_CAMS_Details.Count > 0)
                        {
                            List<CASESP2Entity> Sel_Services = SP_CAMS_Details.FindAll(u => u.ServPlan == Search_Entity.service_plan && u.SP2_Spl_Ins.Trim() == "P" && u.Type1 == "CA");
                            string Operatipn_Mode = "Insert"; int New_CAID = 1, New_CA_Seq = 1;
                            if (Sel_Services.Count > 0)
                            {
                                foreach (CASESP2Entity Entity in Sel_Services)
                                {
                                    CASEACTEntity CA_Pass_Entity = new CASEACTEntity();

                                    CA_Pass_Entity.Agency = Search_Entity.agency;
                                    CA_Pass_Entity.Dept = Search_Entity.dept;
                                    CA_Pass_Entity.Program = Search_Entity.program;
                                    CA_Pass_Entity.Year = Search_Entity.year;
                                    CA_Pass_Entity.App_no = Search_Entity.app_no;

                                    CA_Pass_Entity.Service_plan = Search_Entity.service_plan;
                                    CA_Pass_Entity.SPM_Seq = Sp_Sequence;
                                    CA_Pass_Entity.ACT_Code = Entity.CamCd; CA_Pass_Entity.Branch = Entity.Branch; CA_Pass_Entity.Group = Entity.Orig_Grp.ToString();

                                    CA_Pass_Entity.CA_OBF = Entity.SP2_OBF;

                                    List<CASEACTEntity> Sel_ActList = SP_Activity_Details.FindAll(u => u.ACT_Code.Trim() == Entity.CamCd.Trim() && u.Branch == Entity.Branch && u.Group == Entity.Curr_Grp.ToString());

                                    if (Sel_ActList.Count > 0)
                                    {
                                        foreach (CASEACTEntity ActEnt in Sel_ActList)
                                        {
                                            CA_Pass_Entity = ActEnt;

                                            CA_Pass_Entity.ACT_Date = Search_Entity.startdate; CA_Pass_Entity.Site = Search_Entity.site; CA_Pass_Entity.Fund1 = Search_Entity.SPM_Fund;
                                            CA_Pass_Entity.Caseworker = Search_Entity.caseworker; CA_Pass_Entity.Vendor_No = Search_Entity.SPM_Vendor; //CA_Pass_Entity.Cost = Search_Entity.SPM_Amount;
                                            CA_Pass_Entity.BillngType = Search_Entity.SPM_BillName_Type; CA_Pass_Entity.BillngFname = Search_Entity.SPM_Bill_FName; CA_Pass_Entity.BillngLname = Search_Entity.SPM_Bill_LName;
                                            CA_Pass_Entity.Account = Search_Entity.SPM_Account; CA_Pass_Entity.Act_PROG = Search_Entity.Def_Program;

                                            string HieAgency = BaseForm.BaseAgency;
                                            string HieDept = BaseForm.BaseDept;
                                            string HieProg = BaseForm.BaseProg;
                                            ProgramDefinitionEntity programEntity = new ProgramDefinitionEntity();
                                            if (!string.IsNullOrEmpty(Search_Entity.Def_Program.Trim()))
                                            {
                                                HieAgency = Search_Entity.Def_Program.Substring(0, 2);
                                                HieDept = Search_Entity.Def_Program.Substring(2, 2);
                                                HieProg = Search_Entity.Def_Program.Substring(4, 2);

                                                programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(HieAgency, HieDept, HieProg);
                                            }


                                            List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = new List<PMTFLDCNTLHEntity>();
                                            string CategoryCode = string.Empty;
                                            if (programEntity != null)
                                            {
                                                CategoryCode = programEntity.DepSerpostPAYCAT.Trim();
                                                propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", CategoryCode, HieAgency + HieDept + HieProg, CA_Pass_Entity.Service_plan, CA_Pass_Entity.Branch, CA_Pass_Entity.Curr_Grp, CA_Pass_Entity.ACT_Code.Trim(), "SP");
                                                propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == CategoryCode);

                                                if (propPMTFLDCNTLHEntity.Count == 0)
                                                {
                                                    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", CategoryCode, HieAgency + HieDept + HieProg, "0", " ", "0", "          ", "hie");
                                                    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == CategoryCode);
                                                }

                                                if(propPMTFLDCNTLHEntity.Count>0)
                                                {
                                                    PMTFLDCNTLHEntity FldEntity = propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == "S00016");
                                                    if (FldEntity != null) { if (FldEntity.PMFLDH_ENABLED == "N") CA_Pass_Entity.Fund1 = string.Empty; }
                                                }

                                            }

                                                


                                            //if (!string.IsNullOrEmpty(cmbBudget.Text))
                                            //    CA_Pass_Entity.BDC_ID = Search_Entity.SPM_BDC_ID;

                                            CA_Pass_Entity.Lsct_Operator = BaseForm.UserID;


                                            if (CA_Pass_Entity.Rec_Type == "U")
                                                Operatipn_Mode = "Update";

                                            if (!string.IsNullOrEmpty(CA_Pass_Entity.ACT_ID) && CA_Pass_Entity.Rec_Type == "U")
                                                New_CAID = int.Parse(CA_Pass_Entity.ACT_ID);
                                            else
                                                CA_Pass_Entity.ACT_ID = "1";

                                            if (_model.SPAdminData.UpdateCASEACT3(CA_Pass_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                                            {
                                                CA_Pass_Entity.ACT_ID = New_CAID.ToString();
                                                CA_Pass_Entity.ACT_Seq = New_CA_Seq.ToString();

                                                CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();

                                                Search_CAOBO_Entity.ID = New_CAID.ToString();
                                                Search_CAOBO_Entity.Rec_Type = "S";
                                                Search_CAOBO_Entity.Seq = "1";

                                                _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);

                                                if(CA_Pass_Entity.CA_OBF=="1")
                                                {
                                                    string ClientId = "1";
                                                    CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);

                                                    if (SnpEntity != null) ClientId = SnpEntity.ClientId;

                                                    Search_CAOBO_Entity.CLID = ClientId;
                                                    Search_CAOBO_Entity.Fam_Seq = BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                                                    Search_CAOBO_Entity.Seq = "1";
                                                    Search_CAOBO_Entity.Rec_Type = "I";

                                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                                }
                                                else
                                                {
                                                    foreach (CaseSnpEntity dr1 in BaseForm.BaseCaseSnpEntity)
                                                    {
                                                        if (dr1.SnpAcitveStatus == "A" && dr1.Exclude == "N")
                                                        {
                                                            Search_CAOBO_Entity.CLID = dr1.ClientId;
                                                            Search_CAOBO_Entity.Fam_Seq = dr1.FamilySeq.ToString();
                                                            Search_CAOBO_Entity.Seq = "1";
                                                            Search_CAOBO_Entity.Rec_Type = "I";

                                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);

                                                        }
                                                    }
                                                }

                                                //string ClientId = "1";
                                                //CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);

                                                //if (SnpEntity != null) ClientId = SnpEntity.ClientId;

                                                //Search_CAOBO_Entity.CLID = ClientId;
                                                //Search_CAOBO_Entity.Fam_Seq = BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                                                //Search_CAOBO_Entity.Seq = "1";
                                                //Search_CAOBO_Entity.Rec_Type = "I";

                                                //_model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);

                                            }

                                        }
                                    }
                                    else
                                    {
                                        CA_Pass_Entity.Rec_Type = "I";
                                        CA_Pass_Entity.ACT_Date = Search_Entity.startdate; CA_Pass_Entity.Site = Search_Entity.site; CA_Pass_Entity.Fund1 = Search_Entity.SPM_Fund;
                                        CA_Pass_Entity.Caseworker = Search_Entity.caseworker; CA_Pass_Entity.Vendor_No = Search_Entity.SPM_Vendor; //CA_Pass_Entity.Cost = Search_Entity.SPM_Amount;
                                        CA_Pass_Entity.BillngType = Search_Entity.SPM_BillName_Type; CA_Pass_Entity.BillngFname = Search_Entity.SPM_Bill_FName; CA_Pass_Entity.BillngLname = Search_Entity.SPM_Bill_LName;
                                        CA_Pass_Entity.Account = Search_Entity.SPM_Account; CA_Pass_Entity.Act_PROG = Search_Entity.Def_Program;
                                        CA_Pass_Entity.Fund2 = CA_Pass_Entity.Fund3 = null;
                                        CA_Pass_Entity.Check_Date = CA_Pass_Entity.Check_No = CA_Pass_Entity.Followup_On = null;
                                        CA_Pass_Entity.Followup_Comp = CA_Pass_Entity.Followup_By = CA_Pass_Entity.Refer_Data = CA_Pass_Entity.Cust_Code1 = null;
                                        CA_Pass_Entity.Cust_Value1 = CA_Pass_Entity.Cust_Code2 = CA_Pass_Entity.Cust_Value2 = CA_Pass_Entity.Cust_Code3 = null;
                                        CA_Pass_Entity.Cust_Value3 = CA_Pass_Entity.Lstc_Date = CA_Pass_Entity.Lsct_Operator = CA_Pass_Entity.Add_Date = null;
                                        CA_Pass_Entity.Add_Operator = CA_Pass_Entity.UOM = CA_Pass_Entity.Units =
                                        CA_Pass_Entity.Cust_Code4 = CA_Pass_Entity.Cust_Value4 = CA_Pass_Entity.Cust_Code5 = CA_Pass_Entity.Cust_Value5 = null;
                                        CA_Pass_Entity.Curr_Grp = Entity.Curr_Grp.ToString();

                                        //if (!string.IsNullOrEmpty(cmbBudget.Text))
                                        //    CA_Pass_Entity.BDC_ID = Search_Entity.SPM_BDC_ID;

                                        CA_Pass_Entity.ACT_Seq = "1";

                                        CA_Pass_Entity.Lsct_Operator = BaseForm.UserID;


                                        if (!string.IsNullOrEmpty(CA_Pass_Entity.ACT_ID) && CA_Pass_Entity.Rec_Type == "U")
                                            New_CAID = int.Parse(CA_Pass_Entity.ACT_ID);
                                        else
                                            CA_Pass_Entity.ACT_ID = "1";

                                        if (_model.SPAdminData.UpdateCASEACT3(CA_Pass_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                                        {
                                            CA_Pass_Entity.ACT_ID = New_CAID.ToString();
                                            CA_Pass_Entity.ACT_Seq = New_CA_Seq.ToString();

                                            CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();

                                            Search_CAOBO_Entity.ID = New_CAID.ToString();
                                            Search_CAOBO_Entity.Rec_Type = "S";
                                            Search_CAOBO_Entity.Seq = "1";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);

                                            string ClientId = "1";
                                            CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);

                                            if (SnpEntity != null) ClientId = SnpEntity.ClientId;

                                            Search_CAOBO_Entity.CLID = ClientId;
                                            Search_CAOBO_Entity.Fam_Seq = BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                                            Search_CAOBO_Entity.Seq = "1";
                                            Search_CAOBO_Entity.Rec_Type = "I";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);

                                        }
                                    }


                                }
                            }

                        }

                        //if (((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "E" || ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "S" || ((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "M")
                        //{

                        //    if (Mode.Equals("Edit"))
                        //    {
                        //        string Fund = string.Empty; string BDC_ID = string.Empty;
                        //        if (Search_Entity.Rec_Type == "U" && ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() != "E" && ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() != "S" && ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() != "M")
                        //        { Fund = CASESPM_List[0].SPM_Fund; BDC_ID = CASESPM_List[0].SPM_BDC_ID; }
                        //        else
                        //        {
                        //            Fund = ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString(); BDC_ID = ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();
                        //        }

                        //        Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, Fund);

                        //        if (Emsbdc_List.Count > 0)
                        //            Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString());

                        //    }

                        //    else
                        //    {
                        //        Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString());
                        //        if (Emsbdc_List.Count > 0)
                        //            Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString());
                        //    }
                        //    CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= dtStardt.Value && Convert.ToDateTime(u.BDC_END) >= dtStardt.Value));
                        //    if (emsbdcentity != null)
                        //    {
                        //        CMBDCEntity emsbdcdata = new CMBDCEntity();
                        //        emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                        //        emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                        //        emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                        //        emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                        //        emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                        //        emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                        //        emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                        //        emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                        //        emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                        //        emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                        //        emsbdcdata.BDC_LSTC_OPERATOR = BaseForm.UserID;
                        //        emsbdcdata.Mode = "BdcAmount";
                        //        string strstatus = string.Empty;
                        //        if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                        //        { }
                        //    }
                        //}

                        CASE0016_Control case0016Control = BaseForm.GetBaseUserControl() as CASE0016_Control;
                        if (case0016Control != null)
                        {
                            case0016Control.RefreshGrid(Search_Entity.service_plan, Sp_Sequence);
                        }
                        this.Close();

                    }
                    else
                        MessageBox.Show("Exception : " + Sql_SP_Result_Message, "CAP Systems");
                }
                if (boolsucess)
                {
                    if (Mode == "Add")
                        AlertBox.Show("Saved Successfully");
                    else
                        AlertBox.Show("Updated Successfully");
                }
            }

            
        }

        List<CASEACTEntity> SP_Activity_Details = new List<CASEACTEntity>();
        private void Get_App_CASEACT_List(CASESPMEntity SPM_Entity)
        {

            CASEACTEntity Search_Enty = new CASEACTEntity(true);
            Search_Enty.Agency = SPM_Entity.agency;
            Search_Enty.Dept = SPM_Entity.dept;
            Search_Enty.Program = SPM_Entity.program;
            Search_Enty.Year = SPM_Entity.year;
            Search_Enty.App_no = SPM_Entity.app_no;
            Search_Enty.SPM_Seq = SPM_Entity.Seq;
            Search_Enty.Service_plan = SPM_Entity.service_plan;


            SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");

            SP_Activity_Details = SP_Activity_Details.OrderByDescending(u => Convert.ToDateTime(u.ACT_Date.Trim())).ToList();


            if (BaseForm.BaseAgencyControlDetails.CEAPPostUsage == "Y")
            {
                //List<CASEACTEntity> caseactlist = SP_Activity_Details.FindAll(u => u.Elec_Other.ToString().Trim() == "O");
                //if (caseactlist.Count > 0) { upload2.Visible = true; }
                //caseactlist = SP_Activity_Details.FindAll(u => u.Elec_Other.ToString().Trim() == "E");
                //if (caseactlist.Count > 0) { upload1.Visible = true; }

                if ((SPM_Entity.SPM_EligStatus == "E" || SPM_Entity.SPM_EligStatus == "S" || SPM_Entity.SPM_EligStatus == "M" || SPM_Entity.SPM_EligStatus == "N")) { if(SPM_Entity.SPM_Gas_Vendor!=string.Empty) upload2.Visible = true; if (SPM_Entity.SPM_Vendor != string.Empty) upload1.Visible = true; }

                List<INVDOCLOGEntity> InvDoclogEntitylist = _model.ChldMstData.GetInvDocsLogList(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, SPM_Entity.service_plan, SPM_Entity.Seq, string.Empty, string.Empty);
                InvDoclogEntitylist = InvDoclogEntitylist.FindAll(u => u.INVDOC_DELETED_BY == string.Empty && u.INVDOC_DATE_DELETED == string.Empty && u.INVDOC_SERVICE == string.Empty);
                if (InvDoclogEntitylist.Count > 0)
                {
                    List<INVDOCLOGEntity> PINVDocs = InvDoclogEntitylist.FindAll(u => u.INVDOC_VEND_TYPE == "P");
                    if (PINVDocs.Count > 0) btnPInvDocs.Visible = true; else btnPInvDocs.Visible = false;
                    List<INVDOCLOGEntity> SINVDocs = InvDoclogEntitylist.FindAll(u => u.INVDOC_VEND_TYPE == "S");
                    if (SINVDocs.Count > 0) btnSInvDocs.Visible = true; else btnSInvDocs.Visible = false;
                }
                else
                {
                    btnPInvDocs.Visible = false;
                    btnSInvDocs.Visible = false;
                }
            }
        }


        private void cmbFundsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFundsource.Items.Count > 0)
            {
                string strcmbFundsource = ((Utilities.ListItem)cmbFundsource.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString();
                if (!string.IsNullOrEmpty(strcmbFundsource))
                {
                    if (((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() != "0")
                    {
                        if (Emsbdc_List.Count > 0)
                        {
                            List<CMBDCEntity> Entity = Emsbdc_List.FindAll(u => u.BDC_FUND == strcmbFundsource);// && u.BDC_ALLOW_POSTING=="Y");
                            if (Entity.Count > 0)
                            {
                                Entity = Entity.FindAll(u => Convert.ToDateTime(u.BDC_START.Trim()) <= Convert.ToDateTime(dtStardt.Text.Trim()) && Convert.ToDateTime(u.BDC_END.Trim()) >= Convert.ToDateTime(dtStardt.Text.Trim()));
                                if (Entity.Count > 0)
                                {
                                    fillBudgets(Entity);
                                    if (Entity.Count == 1)
                                    {
                                        cmbBudget.Enabled = false; lblBudgetReq.Visible = false;
                                        CommonFunctions.SetComboBoxValue(cmbBudget, Entity[0].BDC_ID);
                                        txtstart1.Text = LookupDataAccess.Getdate(Entity[0].BDC_START);
                                        txtEnd.Text = LookupDataAccess.Getdate(Entity[0].BDC_END);
                                        txtBudget.Text = Entity[0].BDC_BUDGET;
                                        txtBalance1.Text = Entity[0].BDC_BALANCE;
                                        if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
                                            GetMaxBenfit();
                                    }
                                    else
                                    {
                                        cmbBudget.Enabled = true;
                                        lblBudgetReq.Visible = true;
                                    }
                                }
                                else
                                {
                                    CommonFunctions.MessageBoxDisplay("Budget not set up for this fund given Benefit Date");
                                    txtAmount.Text = string.Empty;
                                    txtBalance1.Text = string.Empty;
                                    txtstart1.Text = string.Empty;
                                    txtEnd.Text = string.Empty;
                                    txtBudget.Text = string.Empty;
                                    CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                                }

                            }
                            else
                            {
                                CommonFunctions.MessageBoxDisplay("Budget not set up for this fund...");
                                txtAmount.Text = string.Empty;
                                txtBalance1.Text = string.Empty;
                                txtstart1.Text = string.Empty;
                                txtEnd.Text = string.Empty;
                                txtBudget.Text = string.Empty;
                                CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                            }
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Budget not set up for this fund...");
                            txtAmount.Text = string.Empty;
                            txtBalance1.Text = string.Empty;
                            txtstart1.Text = string.Empty;
                            txtEnd.Text = string.Empty;
                            txtBudget.Text = string.Empty;
                            CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                        }
                        //Fill_SP_DropDowns();
                    }
                }
            }
        }

        private void fillBudgets(List<CMBDCEntity> BudgetList)
        {
            cmbBudget.SelectedIndexChanged -= new EventHandler(cmbBudget_SelectedIndexChanged);
            cmbBudget.Items.Clear();
            if (BudgetList.Count > 0)
            {
                if (Mode == "Add")
                    BudgetList = BudgetList.FindAll(u => u.BDC_ALLOW_POSTING == "Y");

                foreach (CMBDCEntity entity in BudgetList)
                {
                    cmbBudget.Items.Add(new Utilities.ListItem(entity.BDC_DESCRIPTION.ToString(), entity.BDC_ID.ToString(),entity.BDC_ALLOW_POSTING.Trim(),string.Empty));
                }

                if (BudgetList.Count > 1)
                {
                    cmbBudget.Items.Insert(0, new Utilities.ListItem("  ", "0"));


                }
                cmbBudget.SelectedIndex = 0;

            }
            cmbBudget.SelectedIndexChanged += new EventHandler(cmbBudget_SelectedIndexChanged);
        }

        private void dtStardt_ValueChanged(object sender, EventArgs e)
        {
            if (dtStardt.Checked)
            {
                if (dtStardt.Value > DateTime.Now.Date)
                {
                    CommonFunctions.MessageBoxDisplay("Benefit Determination date may not be a future date");
                }
                else
                {
                    bool boolintakevalid = true;
                    //if (BaseForm.BaseCaseMstListEntity[0].IntakeDate != string.Empty)
                    //{
                    //    if (Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].IntakeDate) <= dtStardt.Value)
                    //    {
                    //        boolintakevalid = true;
                    //    }
                    //    else
                    //    {
                    //        boolintakevalid = false;
                    //        CommonFunctions.MessageBoxDisplay("Benefit Determination date may not precede Intake date...");
                    //    }
                    //}
                    if (boolintakevalid)
                    {
                        if (Emsbdc_List.Count > 0)
                        {
                            if (((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() != "0")
                            {
                                //if (((ListItem)cmbFundsource.SelectedItem).ValueDisplayCode.ToString() != "N")
                                //{
                                //EMSBDCEntity emsbdcentity = propEmsbdc_List.Find(u => u.BDC_FUND == ((ListItem)cmbFundsource.SelectedItem).Value.ToString() && (Convert.ToDateTime(u.BDC_START) <= dtStardt.Value && Convert.ToDateTime(u.BDC_END) >= dtStardt.Value));
                                //if (emsbdcentity != null)
                                //{
                                //    txtstart1.Text = LookupDataAccess.Getdate(emsbdcentity.BDC_START);
                                //    txtEnd.Text = LookupDataAccess.Getdate(emsbdcentity.BDC_END);
                                //    txtBudget.Text = emsbdcentity.BDC_BUDGET;
                                //    txtBalance1.Text = emsbdcentity.BDC_BALANCE;

                                //}
                                //propEmsbdc_List = _model.EMSBDCData.GetEmsBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => u.BDC_FUND == ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() && (Convert.ToDateTime(u.BDC_START) <= dtStardt.Value && Convert.ToDateTime(u.BDC_END) >= dtStardt.Value));
                                if (emsbdcentity != null)
                                {
                                    //bool boolLocktype = true;
                                    //if (Mode.Equals("Edit"))
                                    //{
                                    if ((emsbdcentity.BDC_FUND.Trim() == emsbdcentity.BDC_FUND.Trim()))
                                    {
                                        //boolLocktype = false;
                                        txtstart1.Text = LookupDataAccess.Getdate(emsbdcentity.BDC_START);
                                        txtEnd.Text = LookupDataAccess.Getdate(emsbdcentity.BDC_END);
                                        txtBudget.Text = emsbdcentity.BDC_BUDGET;
                                        txtBalance1.Text = emsbdcentity.BDC_BALANCE;

                                    }
                                    else
                                    {
                                        txtBalance1.Text = string.Empty;
                                        txtstart1.Text = string.Empty;
                                        txtEnd.Text = string.Empty;
                                        txtBudget.Text = string.Empty;
                                    }
                                    //}
                                    //else
                                    //{
                                    //    txtAmount.Text = string.Empty;
                                    //    txtBalance1.Text = string.Empty;
                                    //    txtstart1.Text = string.Empty;
                                    //    txtEnd.Text = string.Empty;
                                    //    txtBudget.Text = string.Empty;
                                    //    //txtAmount.Enabled = false;
                                    //}

                                    //if (boolLocktype)
                                    //{
                                    //    CommonFunctions.MessageBoxDisplay("EMSBDC record is Locked \n In :" + emsbdcentity.BDC_LOCK_SCREEN + "\n By :" + emsbdcentity.BDC_LOCK_BY + "\n On :" + emsbdcentity.BDC_LOCK_ON);
                                    //    txtAmount.Text = string.Empty;
                                    //    txtBalance1.Text = string.Empty;
                                    //    txtstart1.Text = string.Empty;
                                    //    txtEnd.Text = string.Empty;
                                    //    txtBudget.Text = string.Empty;
                                    //    txtAmount.Enabled = false;
                                    //}

                                }
                                else
                                {
                                    if (!Mode.Equals("View"))
                                    {
                                        CommonFunctions.MessageBoxDisplay("Budget not set up for this fundcode...");
                                        txtBalance1.Text = string.Empty;
                                        txtstart1.Text = string.Empty;
                                        txtEnd.Text = string.Empty;
                                        txtBudget.Text = string.Empty;
                                        CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                                    }
                                }
                                //}
                            }
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Budget not set up for this fundcode...");
                            txtBalance1.Text = string.Empty;
                            txtstart1.Text = string.Empty;
                            txtEnd.Text = string.Empty;
                            txtBudget.Text = string.Empty;
                            CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                        }
                    }

                }
            }
        }

        bool Future_Date_Soft_Edit = false;
        private bool Validate_ADD_Mode()
        {
            bool isValid = true;

            if ((CmbSP.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Text.Trim()))))
            {
                _errorProvider.SetError(CmbSP, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Service Plan".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbSP, null);


            if ((!dtStardt.Checked))
            {
                _errorProvider.SetError(dtStardt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else if(dtStardt.Value< Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()))
            {
                _errorProvider.SetError(dtStardt, "The Service Plan Start date must be after the Intake date " + LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()));
                isValid = false;
                //AlertBox.Show("The Service Plan Start date must be after the Intake date "+ LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()));
            }
            else
            {
                bool Future_Date_Flg = false;
                if (dtStardt.Value > DateTime.Today && !Future_Date_Soft_Edit)
                {
                    //MessageBox.Show("' " + lblActivityDate.Text + "' Should not be Future Date", "CAPSYSTEMS");

                    MessageBox.Show("You are about to post a future date for 'Service Plan Start Date'. \n Do you want to proceed?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Allow_Post_Future_Date);

                    //_errorProvider.SetError(Act_Date, string.Format("' " + lblActivityDate.Text + "' Should not be Future Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                    Future_Date_Flg = true;
                }
                else
                {
                    Future_Date_Soft_Edit = false;
                    _errorProvider.SetError(dtStardt, null);
                }
            }

            if (!string.IsNullOrEmpty(Txt_VendNo.Text.Trim()))
            {
                bool IsPrim = false;
                if (CaseVdd1list.Count > 0)
                {
                    string cmbSource = ((Utilities.ListItem)cmbPrimSource.SelectedItem).Value.ToString();
                    CaseVDD1Entity VDDEntity = CaseVdd1list.Find(u => u.Code.Trim() == Txt_VendNo.Text.Trim());
                    if (VDDEntity != null)
                    {
                        if (VDDEntity.FUEL_TYPE1.Trim() == cmbSource || VDDEntity.FUEL_TYPE2.Trim() == cmbSource || VDDEntity.FUEL_TYPE3.Trim() == cmbSource || VDDEntity.FUEL_TYPE4.Trim() == cmbSource || VDDEntity.FUEL_TYPE5.Trim() == cmbSource
                             || VDDEntity.FUEL_TYPE6.Trim() == cmbSource || VDDEntity.FUEL_TYPE7.Trim() == cmbSource || VDDEntity.FUEL_TYPE8.Trim() == cmbSource || VDDEntity.FUEL_TYPE9.Trim() == cmbSource || VDDEntity.FUEL_TYPE10.Trim() == cmbSource)
                            IsPrim = true;
                    }
                }

                if (!IsPrim)
                {
                    //_errorProvider.SetError(Text_VendName, "Please select a Vendor for the new Source");
                    CommonFunctions.MessageBoxDisplay("Please select a Vendor for the new Source");
                    isValid = false;
                }
                //else
                //    _errorProvider.SetError(Text_VendName, null);

            }

            //if ((((Utilities.ListItem)cmbPrimSource.SelectedItem).Value.ToString().Equals("0")))
            //{
            //    _errorProvider.SetError(cmbPrimSource, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPrimSource.Text));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(cmbPrimSource, null);
            //}

            if ((((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString().Equals("0")))
            {
                _errorProvider.SetError(cmbEligStatus, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEligStatus.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEligStatus, null);
            }


            //if (string.IsNullOrEmpty(Txt_VendNo.Text.Trim()))
            //{
            //    _errorProvider.SetError(Text_VendName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVendor.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{

            //    _errorProvider.SetError(Text_VendName, null);
            //}



            ////if (((Utilities.ListItem)cmbBilling.SelectedItem).Value.ToString() == "0")
            ////{
            ////    _errorProvider.SetError(cmbBilling, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBillingName.Text.Replace(Consts.Common.Colon, string.Empty)));
            ////    isValid = false;
            ////}
            ////else
            ////{
            ////    _errorProvider.SetError(cmbBilling, null);
            ////}

            //if (lblAccountReq.Visible && String.IsNullOrEmpty(txtAccountNo.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtAccountNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccount.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(txtAccountNo, null);
            //}

            //if (string.IsNullOrEmpty(Txt_GasVendNo.Text.Trim()))
            //{
            //    _errorProvider.SetError(Text_GasVendName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGasVendor.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{

            //    _errorProvider.SetError(Text_GasVendName, null);
            //}


            //if (((Utilities.ListItem)cmbGasBilling.SelectedItem).Value.ToString() == "0")
            //{
            //    _errorProvider.SetError(cmbGasBilling, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGasBillingName.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(cmbGasBilling, null);
            //}


            //if (lblGasAccountReq.Visible && String.IsNullOrEmpty(txtGasAccountNo.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtGasAccountNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGasAccount.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(txtGasAccountNo, null);
            //}


            if (((Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString().Trim() == "E" || ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() == "S" || ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() == "M" || ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() == "N")
            {
                if ((((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString().Equals("0")))
                {
                    _errorProvider.SetError(cmbFundsource, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFundSource.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbFundsource, null);
                }
            }


            if (cmbBudget.Enabled == true)
            {
                if ((((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString().Equals("0")))
                {
                    _errorProvider.SetError(cmbBudget, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBudget.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbBudget, null);
                }
            }

            if (cmbBenfitReason.Enabled)
            {

                if ((((Utilities.ListItem)cmbBenfitReason.SelectedItem).Value.ToString().Equals("0")))
                {
                    _errorProvider.SetError(cmbBenfitReason, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReason.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbBenfitReason, null);
                }
            }
            //if ((CmbSite.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSite.SelectedItem).Text.Trim()))))
            //{
            //    _errorProvider.SetError(CmbSite, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Site".Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(CmbSite, null);

            if ((cmbcaseworker.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbcaseworker.SelectedItem).Text.Trim()))))
            {
                _errorProvider.SetError(cmbcaseworker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Case Worker".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbcaseworker, null);

            decimal decauthamount = 0;
            if (((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString().Trim() == "E" || ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() == "S" || ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() == "M" || ((Captain.Common.Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() == "N")
            {
                if (string.IsNullOrEmpty(txtAmount.Text))
                {
                    _errorProvider.SetError(txtAmount, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmount.Text));
                    isValid = false;
                }
                else
                {
                    decauthamount = Convert.ToDecimal(txtAmount.Text);
                    if (decauthamount <= 0)
                    {
                        _errorProvider.SetError(txtAmount, "Please Enter above 0");
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtAmount, null);
                }
            }
            //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC")
            //{
            //    if ((((Utilities.ListItem)cmbFrstApp.SelectedItem).Value.ToString().Equals("0")))
            //    {
            //        _errorProvider.SetError(cmbFrstApp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFrstApp.Text));
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(cmbFrstApp, null);
            //    }
            //}

            if (Mode.Equals("Edit"))
            {
                decimal decResAmount = 0;
                decimal decamount = 0;

                if (txtAmount.Text.Trim() != string.Empty)
                    decamount = Convert.ToDecimal(txtAmount.Text);

                if (SPM_Entity != null)
                    Get_App_CASEACT_List(SPM_Entity);

                if (SP_Activity_Details.Count > 0)
                {
                    List<CASEACTEntity> CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && u.Elec_Other.Trim() != "");
                    if (CaseactList.Count > 0)
                        decResAmount = CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                }

                if (decamount < decResAmount)
                {
                    CommonFunctions.MessageBoxDisplay("Amount Should be more than " + decResAmount);
                    isValid = false;
                }

            }

            //if (this.SP_Branches.HeaderText.Contains("*"))
            //{
            //    bool Branch_Selected = false;
            //    foreach (DataGridViewRow dr in Branches_Grid.Rows)
            //    {
            //        if (dr.Cells["Branch_Sel"].Value.ToString() == true.ToString())
            //        {
            //            Branch_Selected = true; break;
            //        }
            //    }

            //    if (!Branch_Selected)
            //    {
            //        _errorProvider.SetError(Branches_Grid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Branches".Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //        _errorProvider.SetError(Branches_Grid, null);
            //}
            //else
            //    _errorProvider.SetError(Branches_Grid, null);


            //if (Est_Date.Checked && Est_Date.Value < Start_Date.Value)
            //{
            //    _errorProvider.SetError(Est_Date, string.Format("' " + Lbl_Est_CompleteDate.Text + "' Should not be Prior to 'Start Date'".Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(Est_Date, null);

            //if (Act_Date.Checked && Act_Date.Value < Start_Date.Value)
            //{
            //    _errorProvider.SetError(Act_Date, string.Format("' " + Lbl_Actual_CompleteDate.Text + "' Should not be Prior to 'Start Date'".Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(Act_Date, null);



            return isValid;
        }

        private void Allow_Post_Future_Date(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                Future_Date_Soft_Edit = true;
                //if (CAMS_FLG == "CA")
                btnSave_Click(btnSave, EventArgs.Empty);
                //else
                //    Btn_MS_Save_Click(Btn_MS_Save, EventArgs.Empty);
            }
        }

        public string[] GetSelected_SP_Code()
        {
            string[] Added_Edited_SPCode = new string[2];

            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString())))
            {
                Added_Edited_SPCode[0] = ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString();
                Added_Edited_SPCode[1] = Mode;
            }

            return Added_Edited_SPCode;
        }

        List<CASESPMEntity> CASESPM_List = new List<CASESPMEntity>();
        private void FillBenefit_Controls()
        {

            if (CASESPM_SP_List.Count > 0)
            {
                CASESPM_List = CASESPM_SP_List.FindAll(u => u.service_plan == SP_Code && u.Seq == Sp_Sequence);
            }


            if (CASESPM_List.Count > 0)
            {
                SPM_Entity = CASESPM_List[0];

                Get_App_CASEACT_List(SPM_Entity);

                if (!string.IsNullOrEmpty(SPM_Entity.startdate))
                {
                    dtStardt.Value = Convert.ToDateTime(SPM_Entity.startdate);
                    dtStardt.Checked = true;
                    dtStardt.ShowCalendar = true;

                    if (SP_Activity_Details.Count > 0)
                    {
                        CASEACTEntity Electric = SP_Activity_Details.Find(u => u.Elec_Other == "E");
                        if (Electric != null) 
                        {
                            if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                            {
                                panel_Referral2.Enabled = true; cmbPrimSource.Enabled = false;
                            }
                            else
                            { cmbPrimSource.Enabled = false; panel_Referral2.Enabled = false; }
                        }

                        CASEACTEntity Gas = SP_Activity_Details.Find(u => u.Elec_Other == "O");
                        if (Gas != null) 
                        {
                            if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                            {
                                cmbSecSource.Enabled = false; panel_ReferralGas.Enabled = true;
                            }
                            else
                            { cmbSecSource.Enabled = false; panel_ReferralGas.Enabled = false; }
                        }

                        if(Electric!=null || Gas!=null)
                        {
                            dtStardt.Enabled = false;
                            dtStardt.ShowCalendar = false;
                        }
                    }


                    if (SP_Activity_Details.Count > 0)
                    {
                        decimal decResAmount = 0;

                        List<CASEACTEntity> CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && u.Elec_Other.Trim() != "");
                        if (CaseactList.Count > 0)
                            decResAmount = CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                        if (decResAmount > 0)
                        {
                            cmbBenfitReason.Enabled = false;
                        }
                    }

                }

                //dtStardt.Value = DateTime.Today;
                //dtStardt.Checked = false;
                CommonFunctions.SetComboBoxValue(cmbEligStatus, SPM_Entity.SPM_EligStatus.Trim());
                CommonFunctions.SetComboBoxValue(CmbSP, SPM_Entity.service_plan);
                CommonFunctions.SetComboBoxValue(cmbFundsource, SPM_Entity.SPM_Fund.Trim());
                CommonFunctions.SetComboBoxValue(CmbSite, SPM_Entity.site.Trim());
                CommonFunctions.SetComboBoxValue(cmbcaseworker, SPM_Entity.caseworker.Trim());
                

                CommonFunctions.SetComboBoxValue(cmbBudget, SPM_Entity.SPM_BDC_ID.Trim());
                CommonFunctions.SetComboBoxValue(cmbBenfitReason, SPM_Entity.Spm_Benefit_Reasn.Trim());
                CmbSP.Enabled = false;

                //CommonFunctions.SetComboBoxValue(cmbFrstApp, SPM_Entity.SPM_FIRST_APP.Trim());
                //SetComboBoxValue(Cmb_Def_Prog, Search_Entity.Def_Program);
                Txt_SPM_Program.Text = Set_SP_Program_Text(SPM_Entity.Def_Program);

                //cmbFundsource.Enabled = false; cmbBudget.Enabled = false;
                if (SPM_Entity.CA_Postings_Cnt != "0" || SPM_Entity.MS_Postings_Cnt != "0")
                {
                    CmbSP.Enabled = false;
                    cmbFundsource.Enabled = false; cmbBudget.Enabled = false;
                }

                txtFirst.Text = SPM_Entity.SPM_Bill_FName;
                txtLast.Text = SPM_Entity.SPM_Bill_LName;
                txtAccountNo.Text = SPM_Entity.SPM_Account.Trim();

                Txt_VendNo.Text = SPM_Entity.SPM_Vendor;
                Text_VendName.Text = Get_Vendor_Name(SPM_Entity.SPM_Vendor);
                CommonFunctions.SetComboBoxValue(cmbPrimSource, SPM_Entity.SPM_Prim_Source.Trim());

                Txt_GasVendNo.Text = SPM_Entity.SPM_Gas_Vendor;
                Text_GasVendName.Text = Get_Vendor_Name(SPM_Entity.SPM_Gas_Vendor);
                CommonFunctions.SetComboBoxValue(cmbSecSource, SPM_Entity.SPM_Sec_Source.Trim());


                CommonFunctions.SetComboBoxValue(cmbBilling, "0");
                if (SPM_Entity.SPM_BillName_Type == "T")
                {
                    CommonFunctions.SetComboBoxValue(cmbBilling, "T");
                    if (Mode != "Add" && Mode != "Edit")
                    {
                        txtFirst.Enabled = false;
                        txtLast.Enabled = false;
                    }
                }
                else
                {
                    CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim() == SPM_Entity.SPM_Bill_FName.Trim() && u.NameixLast.Trim() == SPM_Entity.SPM_Bill_LName.Trim());
                    if (casesnp != null)
                        CommonFunctions.SetComboBoxValue(cmbBilling, SPM_Entity.SPM_Bill_FName.Trim() + SPM_Entity.SPM_Bill_LName.Trim());
                }

                txtGasFirst.Text = SPM_Entity.SPM_Gas_Bill_FName;
                txtGasLast.Text = SPM_Entity.SPM_Gas_Bill_LName;
                txtGasAccountNo.Text = SPM_Entity.SPM_Gas_Account.Trim();



                CommonFunctions.SetComboBoxValue(cmbGasBilling, "0");
                if (SPM_Entity.SPM_Gas_BillName_Type == "T")
                {
                    CommonFunctions.SetComboBoxValue(cmbGasBilling, "T");
                    if (Mode != "Add" && Mode != "Edit")
                    {
                        txtGasFirst.Enabled = false;
                        txtGasLast.Enabled = false;
                    }
                }
                else
                {
                    CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim() == SPM_Entity.SPM_Gas_Bill_FName.Trim() && u.NameixLast.Trim() == SPM_Entity.SPM_Gas_Bill_LName.Trim());
                    if (casesnp != null)
                        CommonFunctions.SetComboBoxValue(cmbGasBilling, SPM_Entity.SPM_Gas_Bill_FName.Trim() + SPM_Entity.SPM_Gas_Bill_LName.Trim());
                }


                if (!string.IsNullOrEmpty(SPM_Entity.SPM_Amount))
                {
                    if(SPM_Entity.SPM_Amount!= SPM_Entity.SPM_Balance)
                        txtAmount.Text = SPM_Entity.SPM_Amount;
                }
                if (!string.IsNullOrEmpty(SPM_Entity.SPM_Balance))
                {
                    if (SPM_Entity.SPM_Amount != SPM_Entity.SPM_Balance)
                        txtBalance.Text = SPM_Entity.SPM_Balance;
                }

                if (Mode == "Edit")
                {
                    if (SPM_Entity.SPM_EligStatus == "E" || SPM_Entity.SPM_EligStatus == "S" || SPM_Entity.SPM_EligStatus == "M" || SPM_Entity.SPM_EligStatus == "N")
                    {
                        if (Convert.ToDecimal(SPM_Entity.SPM_Amount.Trim()) != Convert.ToDecimal(SPM_Entity.SPM_Balance.Trim()))
                        {
                            cmbEligStatus.Enabled = false;
                            cmbBenfitReason.Enabled = false;
                        }
                    }
                }





                //SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(Code, null, null, null, "CASE4006");

                //Get_App_CASEACT_List();
                //Get_App_CASEMS_List();

                //if (SP_Activity_Details.Count == 0 && SP_MS_Details.Count == 0)
                //    Start_Date.Enabled = Start_Date_Enable_SW;
                //else
                //    Start_Date.Enabled = false;



                //Fill_Branch_Grid(Search_Entity.service_plan);
                //Fill_SP_CAMS_Details(Search_Entity.service_plan, "P", null);


                //CASESP0Entity casesp0data = propSearch_Entity.Find(u => u.Code == ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString());
                //if (casesp0data != null)
                //{
                //    //if (Mode.Equals("Edit"))
                //    //{
                //    //    btnServiceAdmin.Visible = false;
                //    //    if (BaseForm.BusinessModuleID == "03")
                //    //    {
                //    //        if (casesp0data.Usage_SW == "Y")
                //    //        {
                //    //            btnServiceAdmin.Visible = true;
                //    //        }
                //    //    }
                //    //}
                //    if (casesp0data.Sp0Notes == "Y" && Mode.Equals("Edit")) { ShowCaseNotesImages(); picSPMNotes.Visible = true; } else picSPMNotes.Visible = false;
                //}
            }
        }

        bool IsVulnerable = false;
        private void CmbSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim()))
            {
                SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString(), ACR_SERV_Hies);
                fillFundCombo();
                List<HierarchyEntity> SPHie_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString(), "I");

                GetMaxBenfit();

                if (Mode == "Add")
                {
                    if (SPHie_Programs_List.Count > 0 && SPHie_Programs_List.Count == 1) Txt_SPM_Program.Text = Set_SP_Program_Text(SPHie_Programs_List[0].Code.ToString().Replace("-", "").ToString());

                    if (CmbSite.Items.Count > 0 && (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Site.Trim())))
                        CommonFunctions.SetComboBoxValue(CmbSite, BaseForm.BaseCaseMstListEntity[0].Site.Trim());
                    else
                        CmbSite.SelectedIndex = 0;

                    CASESP0Entity casesp0data = new CASESP0Entity();
                    string IsVul = string.Empty;
                    if (CmbSP.Items.Count > 0)
                    {
                        casesp0data = propSearch_Entity.Find(u => u.Code == ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString());


                        //if(casesp0data!=null)
                        //{
                        //if (casesp0data.Household_Type == "01") IsVulnerable = true; else IsVulnerable = false;
                        if (CEAPCNTL_List.Count > 0)
                        {
                            if (((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == CEAPCNTL_List[0].CPCT_VUL_SP.Trim())
                                IsVul = "01";
                            else if (((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == CEAPCNTL_List[0].CPCT_NONVUL_SP.Trim())
                                IsVul = "02";
                        }
                    }

                    if (Vulner_Flag && IsVul == "01")
                    {
                        btnSave.Enabled = true;
                    }
                    else if (!Vulner_Flag && IsVul == "02")
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = false;
                    }
                }
            }
        }

        private void cmbEligStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEligStatus.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "E")
                {
                    bool IsElig = true;
                    if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim()))
                    {
                        if (decimal.Parse(BaseForm.BaseCaseMstListEntity[0].Poverty.Trim()) > 150)
                        {
                            if (Mode == "Add")
                            {
                                CommonFunctions.SetComboBoxValue(cmbEligStatus, "D");
                                IsElig = false;
                            }
                        }

                    }

                    if (Mode == "Add")
                    {
                        //if (custResponses.Count > 0)
                        //    CustomQuestionsEntity _entity = custResponses.Find(u => u.SER_ELEC.Trim() == "");

                        if (((Captain.Common.Utilities.ListItem)cmbPrimSource.SelectedItem).Value.ToString() == "0" || Txt_VendNo.Text.Trim() == "" || txtAccountNo.Text.Trim() == "" || ((Captain.Common.Utilities.ListItem)cmbBilling.SelectedItem).Value.ToString() == "0")
                        {
                            MessageBox.Show("You must enter 12 months of Payment History and Source, Vendor, Account Number, Billing Name of Primary Supplier before you can select Eligible and Calculate Award.");
                            CommonFunctions.SetComboBoxValue(cmbEligStatus, "P");
                            IsElig = false;
                        }
                        else if (custResponses.Count > 0)
                        {
                            CustomQuestionsEntity _entity = custResponses.Find(u => u.USAGE_PRIM_PAYMENT.Trim() == "");
                            //CustomQuestionsEntity _entity = custResponses.Find(u => u.USAGE_MONTH.Trim() == "TOT" && u.USAGE_TOTAL=="0.00" && u.USAGE_TOTAL.Trim()==string.Empty);
                            if (_entity != null)
                            {
                                MessageBox.Show("You must enter 12 months of Payment History and Source, Vendor, Account Number, Billing Name of Primary Supplier before you can select Eligible and Calculate Award.");
                                CommonFunctions.SetComboBoxValue(cmbEligStatus, "P");
                                IsElig = false;
                            }
                        }
                    }

                    if (IsElig)
                    {
                        txtAmount.ReadOnly = false; lblAmountReq.Visible = true;
                        cmbFundsource.Enabled = true; lblFundReq.Visible = true;
                        cmbBenfitReason.Enabled = true; lblBenefitReasonreq.Visible = true;
                        GetMaxBenfit();

                        if (!string.IsNullOrEmpty(Txt_VendNo.Text.Trim())) upload1.Visible = true;
                        if (!string.IsNullOrEmpty(Txt_GasVendNo.Text.Trim())) upload2.Visible = true;
                    }
                    else
                    {
                        lblAmountReq.Visible = false;lblBudgetReq.Visible = false; lblBenefitReasonreq.Visible = false;
                    }
                }
                else if (((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "S" || ((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "M" || ((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "N")
                {
                    bool IsElig = true;
                    if (Mode == "Add")
                    {
                        if (((Captain.Common.Utilities.ListItem)cmbPrimSource.SelectedItem).Value.ToString() == "0" || Txt_VendNo.Text.Trim() == "" || txtAccountNo.Text.Trim() == "" || ((Captain.Common.Utilities.ListItem)cmbBilling.SelectedItem).Value.ToString() == "0")
                        {
                            MessageBox.Show("You must enter 12 months of Payment History and Source, Vendor, Account Number, Billing Name of Primary Supplier before you can select Eligible and Calculate Award.");
                            CommonFunctions.SetComboBoxValue(cmbEligStatus, "D");
                            IsElig = false;
                        }
                        else if (custResponses.Count > 0)
                        {
                            CustomQuestionsEntity _entity = custResponses.Find(u => u.USAGE_PRIM_PAYMENT.Trim() == "");
                            if (_entity != null)
                            {
                                MessageBox.Show("You must enter 12 months of Payment History and Source, Vendor, Account Number, Billing Name of Primary Supplier before you can select Eligible and Calculate Award.");
                                CommonFunctions.SetComboBoxValue(cmbEligStatus, "P");
                                IsElig = false;
                            }
                        }
                    }
                    if (IsElig)
                    {
                        txtAmount.ReadOnly = false; lblAmountReq.Visible = true;
                        cmbFundsource.Enabled = true; lblFundReq.Visible = true;
                        cmbBenfitReason.Enabled = true; lblBenefitReasonreq.Visible = true;
                        GetMaxBenfit();

                        if (!string.IsNullOrEmpty(Txt_VendNo.Text.Trim())) upload1.Visible = true;
                        if (!string.IsNullOrEmpty(Txt_GasVendNo.Text.Trim())) upload2.Visible = true;

                    }
                    else
                    {
                        lblAmountReq.Visible = false; lblBudgetReq.Visible = false; lblBenefitReasonreq.Visible = false;
                    }
                }
                else
                {
                    txtAmount.ReadOnly = true; lblAmountReq.Visible = false; lblFundReq.Visible = false; lblBudgetReq.Visible = false;
                    cmbFundsource.Enabled = false; cmbBudget.Enabled = false; cmbBenfitReason.Enabled = false; lblBenefitReasonreq.Visible = false;
                    //if (Mode == "Add")
                    //{
                    CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                    CommonFunctions.SetComboBoxValue(cmbBudget, "0");
                    CommonFunctions.SetComboBoxValue(cmbBenfitReason, "0");
                    txtAmount.Text = string.Empty;
                    txtBalance1.Text = string.Empty;
                    txtstart1.Text = string.Empty;
                    txtEnd.Text = string.Empty;
                    txtBudget.Text = string.Empty;
                    //}
                    upload1.Visible = false;upload2.Visible = false;
                }
            }
        }

        private void Pb_SPM_Prog_Click(object sender, EventArgs e)
        {
            try
            {
                if (CmbSP.SelectedItem != null)
                {
                    string Sel_Prog = (!string.IsNullOrEmpty(Txt_SPM_Program.Text.Trim()) ? Txt_SPM_Program.Text.Trim().Substring(0, 6) : "");
                    HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString());
                    //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString(), ACR_SERV_Hies);
                    hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                    hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                    hierarchieSelectionForm.ShowDialog();
                }
            }
            catch (Exception ex) { }
        }

        string Sel_SPM_Program = "";
        List<HierarchyEntity> SP_Programs_List = new List<HierarchyEntity>();
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
                Txt_SPM_Program.Text = Sel_SPM_Program = form.Selected_SerPlan_Prog();
        }

        private string Set_SP_Program_Text(string Prog_Code)
        {
            string Tmp_Hierarchy = "";
            Sel_SPM_Program = "";

            foreach (HierarchyEntity Ent in SP_Programs_List)
            {
                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                if (Prog_Code == Tmp_Hierarchy)
                {
                    Sel_SPM_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                    break;
                }
            }

            return Sel_SPM_Program;
        }


        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        private void On_CEAP_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "CEAP_Priority_Rating.pdf";



            PdfName = "CEAP_Priority_Rating";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 150; Y_Pos -= 90;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 500;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseCaseMstListEntity[0].ApplNo, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                int inteldercount = 0;
                if (casesnpElder.Count > 0)
                {
                    inteldercount = 4;
                }
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0)
                {
                    intyoungercount = 0;
                }
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0)
                {
                    intdisablecount = 4;
                }
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0; int intsenvtyfive = 0; int inttwentyfive = 0; int inttwentytofifty = 0; int intfiftyone = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 10;
                    intfity = 10;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 7;
                    intsenvtyfive = 7;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <126)
                {
                    inttotalcount = inttotalcount + 3;
                    inttwentyfive = 3;
                }
                else if (intmstpoverty >= 126 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 1;
                    inttwentytofifty = 1;
                }
                else if (intmstpoverty >= 151)
                {

                    intfiftyone = 0;
                }

                int intExceedYes = 0; int intExceedNo = 0;
                if (doublesertotal > 1000)
                {
                    inttotalcount = inttotalcount + 4;
                    intExceedYes = 4;
                }
                else
                {
                    inttotalcount = inttotalcount + 1;
                    intExceedNo = 1;
                }


                int intthirty = 0; int inttwenty = 0; int inteleven = 0; int intsix = 0; int intfive = 0;
                if (doubleTotalAmount == 0 || doublesertotal == 0)
                {
                    if (doubleTotalAmount == 0)
                    {
                        inttotalcount = inttotalcount + 12;
                        intthirty = 12;
                    }
                    else
                        intfive = 0;
                }
                else
                {

                    if (totaldive >= 30)
                    {
                        inttotalcount = inttotalcount + 12;
                        intthirty = 12;
                    }
                    else if (totaldive >= 20 && totaldive <= 29.99)
                    {
                        inttotalcount = inttotalcount + 9;
                        inttwenty = 9;
                    }
                    else if (totaldive >= 11 && totaldive <= 19.99)
                    {
                        inttotalcount = inttotalcount + 6;
                        inteleven = 6;
                    }
                    else if (totaldive >= 6 && totaldive <= 10.99)
                    {
                        inttotalcount = inttotalcount + 3;
                        intsix = 3;
                    }
                    else if (totaldive <= 5.99)
                    {
                        if (doubleTotalAmount == 0 || doublesertotal == 0)
                        {
                            intfive = 0;
                        }
                        else
                        {
                            inttotalcount = inttotalcount + 1;
                            intfive = 1;
                        }
                    }
                }

                X_Pos = 535;
                Y_Pos -= 45;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 42;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 65; Y_Pos -= 45;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doublesertotal.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 200;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 400;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(totaldive.ToString().ToUpper() == "NAN" ? string.Empty : totaldive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 535;
                Y_Pos -= 37;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwenty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteleven.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsix.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 40;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intExceedYes.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intExceedNo.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 30;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                if (inttotalcount >= 17)
                {
                    X_Pos = 40;
                    Y_Pos -= 30;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 11 && inttotalcount <= 16)
                {
                    X_Pos = 40;
                    Y_Pos -= 66;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 10)
                {
                    X_Pos = 40;
                    Y_Pos -= 93;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }


                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }

        private void PbPdf_Click(object sender, EventArgs e)
        {
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "PCS")
            {
                if (BaseForm.BaseYear == "2022")
                    On_PCS_PriorityRankingForm2022();
                else if (BaseForm.BaseYear == "2023")
                    On_PCS_PriorityRankingForm2023();
                else
                    On_PCS_PriorityRankingForm();

                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "RMPC")
            {
                On_RPMC_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "NCCAA")
            {
                On_NCCAA_PriorityRankingForm();
                //On_Fortworth_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "FTW")
            {
                //On_NCCAA_PriorityRankingForm();
                On_Fortworth_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "BVCOG")
            {
                //On_NCCAA_PriorityRankingForm();
                On_CSNT_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "CVCAA")
            {
                On_CVCAA_PriorityRankingForm();

                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "HCCAA")
            {
                On_HCCAA_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "CACOST")
            {
                On_CACOST_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "WCCAA")
            {
                On_WCCAA_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else if(BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "CCSCT")
            {
                On_CCSCT_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
            }
            else
            {
                On_CEAP_PriorityRankingForm();
                txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;
                SavePrintRecord();
            }
        }


        #region PCS Priority Sheet Form

        private void On_PCS_PriorityRankingForm2022()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "PCS_Priority_Ranking_2022.pdf";



            PdfName = "PCS_Priority_Sheet";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 100; Y_Pos -= 140;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 280;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                int inteldercount = 0;//casesnpElder.Count * 3;
                if (casesnpElder.Count > 0) inteldercount = 3;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0; //casesnpyounger.Count * 3;
                if (casesnpyounger.Count > 0) intyoungercount = 3;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0; //casesnpdisable.Count * 3;
                if(casesnpdisable.Count>0) intdisablecount = 3;
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 5;
                    intfity = 5;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 4;
                    intsenvtyfive = 4;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 3;
                    intonefiftyfive = 3;
                }
                //else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    inttwentytofifty = 1;
                //}
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal > 1000)
                {
                    inttotalcount = inttotalcount + 5;
                    int1000plus = 5;
                }
                else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                {
                    inttotalcount = inttotalcount + 4;
                    int500above = 4;
                }
                else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                {
                    inttotalcount = inttotalcount + 3;
                    int250above = 3;
                }
                else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                {
                    inttotalcount = inttotalcount + 2;
                    int250below = 2;
                }



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                if (doubleTotalAmount == 0 || doublesertotal == 0)
                {
                    if (doubleTotalAmount == 0)
                    {
                        inttotalcount = inttotalcount + 8;
                        intthirty = 8;
                    }
                    else
                        intfive = 0;
                }
                else
                {

                    if (totaldive >= 30)
                    {
                        inttotalcount = inttotalcount + 8;
                        intthirty = 8;
                    }
                    else if (totaldive >= 17 && totaldive <= 29.99)
                    {
                        inttotalcount = inttotalcount + 7;
                        intseventto29 = 7;
                    }
                    else if (totaldive >= 11 && totaldive <= 16.99)
                    {
                        inttotalcount = inttotalcount + 6;
                        intelevento16 = 6;
                    }
                    else if (totaldive >= 6 && totaldive <= 10.99)
                    {
                        inttotalcount = inttotalcount + 2;
                        intsixtoten = 2;
                    }
                    else if (totaldive <= 5.99)
                    {
                        if (doubleTotalAmount == 0 || doublesertotal == 0)
                        {
                            intfive = 0;

                        }
                        else
                        {
                            inttotalcount = inttotalcount + 1;
                            intfive = 1;
                        }
                    }
                }

                X_Pos = 510;
                Y_Pos -= 58;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 510;
                Y_Pos -= 45;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Consumption Rate

                X_Pos = 510;
                Y_Pos -= 44;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Poverty 

                X_Pos = 510;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                Y_Pos -= 40;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0".ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 36;
                X_Pos = 510;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 60;
                if (inttotalcount >= 20)
                {
                    X_Pos = 40;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 11 && inttotalcount <= 19)
                {
                    X_Pos = 205;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 10)
                {
                    X_Pos = 370;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }

        private void On_PCS_PriorityRankingForm2023()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "PCS_Priority_Ranking_2023.pdf";



            PdfName = "PCS_Priority_Sheet";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 100; Y_Pos -= 140;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 280;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                //List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));
                int inteldercount = 0; 
                if(casesnpElder.Count>0) inteldercount = 4;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if(casesnpyounger.Count>0) intyoungercount = 4;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0; 
                if(casesnpdisable.Count>0) intdisablecount = 4;
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 4;
                    intfity = 4;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 3;
                    intsenvtyfive = 3;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 2;
                    intonefiftyfive = 2;
                }
                //else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    inttwentytofifty = 1;
                //}
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal >= 1000)
                {
                    inttotalcount = inttotalcount + 4;
                    int1000plus = 4;
                }
                //else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                //{
                //    inttotalcount = inttotalcount + 4;
                //    int500above = 4;
                //}
                //else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    int250above = 3;
                //}
                //else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    int250below = 2;
                //}



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                //if (doubleTotalAmount == 0 || doublesertotal == 0)
                //{
                //    if (doubleTotalAmount == 0)
                //    {
                //        inttotalcount = inttotalcount + 8;
                //        intthirty = 8;
                //    }
                //    else
                //        intfive = 0;
                //}
                //else
                {

                    if (totaldive >= 11)
                    {
                        inttotalcount = inttotalcount + 5;
                        intthirty = 5;
                    }
                    //else if (totaldive >= 17 && totaldive <= 29.99)
                    //{
                    //    inttotalcount = inttotalcount + 7;
                    //    intseventto29 = 7;
                    //}
                    //else if (totaldive >= 11 && totaldive <= 16.99)
                    //{
                    //    inttotalcount = inttotalcount + 6;
                    //    intelevento16 = 6;
                    //}
                    //else if (totaldive >= 6 && totaldive <= 10.99)
                    //{
                    //    inttotalcount = inttotalcount + 2;
                    //    intsixtoten = 2;
                    //}
                    //else if (totaldive <= 5.99)
                    //{
                    //    if (doubleTotalAmount == 0 || doublesertotal == 0)
                    //    {
                    //        intfive = 0;

                    //    }
                    //    else
                    //    {
                    //        inttotalcount = inttotalcount + 1;
                    //        intfive = 1;
                    //    }
                    //}
                }

                X_Pos = 510;
                Y_Pos -= 58;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 510;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Consumption Rate

                X_Pos = 510;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Poverty 

                X_Pos = 510;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                string Fname = string.Empty; string dob = string.Empty;
                if (SnpEntity != null)
                {
                    Fname = SnpEntity.NameixFi; dob = SnpEntity.AltBdate.Trim();
                }


                //DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("ALS", "ALL", string.Empty, string.Empty, string.Empty, string.Empty, Fname, string.Empty,
                //              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dob, BaseForm.UserID, string.Empty, string.Empty);
                //int FirstTime = 0;
                //if(ds.Tables[0].Rows.Count>0)
                //{
                //    if(ds.Tables[0].Rows.Count==1) FirstTime = 3;

                //    //foreach(DataRow dr in ds.Tables[0].Rows)
                //    //{
                //    //    if(dr["Agency"].ToString()!=BaseForm.BaseAgency && dr["Dept"].ToString() != BaseForm.BaseDept && dr["Prog"].ToString() != BaseForm.BaseProg && dr["SnpYear"].ToString() != BaseForm.BaseYear)
                //    //    {
                //    //        FirstTime = 3;
                //    //        break;
                //    //    }
                //    //}
                //}


                Y_Pos -= 42;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 36;
                X_Pos = 510;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 60;
                if (inttotalcount >= 20)
                {
                    X_Pos = 40;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 11 && inttotalcount <= 19)
                {
                    X_Pos = 205;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 10)
                {
                    X_Pos = 370;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }

        private void On_PCS_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "PCS_Priority_Ranking_" + BaseForm.BaseYear + ".pdf";



            PdfName = "PCS_Priority_Sheet";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            List<AddCustEntity> propADDCUST = new List<AddCustEntity>();
            AddCustEntity Search_AddCust = new AddCustEntity(true);
            Search_AddCust.ACTAGENCY = BaseForm.BaseAgency; Search_AddCust.ACTDEPT = BaseForm.BaseDept; Search_AddCust.ACTPROGRAM = BaseForm.BaseProg;
            Search_AddCust.ACTYEAR = BaseForm.BaseYear; Search_AddCust.ACTAPPNO = BaseForm.BaseApplicationNo;
            propADDCUST = _model.CaseMstData.Browse_ADDCUST(Search_AddCust, "Browse");

            //if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            //    CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, BaseForm.BaseYear, string.Empty, string.Empty);

            List<CEAPINVEntity> CEAPINVs = new List<CEAPINVEntity>();
            if (CEAPCNTL_List.Count > 0)
                CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 100; Y_Pos -= 140;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 280;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                //List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));
                int inteldercount = 0;
                if (casesnpElder.Count > 0) inteldercount = 4;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0) intyoungercount = 4;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0) intdisablecount = 4;
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 4;
                    intfity = 4;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 3;
                    intsenvtyfive = 3;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 2;
                    intonefiftyfive = 2;
                }
                //else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    inttwentytofifty = 1;
                //}
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal >= 1000)
                {
                    inttotalcount = inttotalcount + 4;
                    int1000plus = 4;
                }
                //else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                //{
                //    inttotalcount = inttotalcount + 4;
                //    int500above = 4;
                //}
                //else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    int250above = 3;
                //}
                //else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    int250below = 2;
                //}



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                //if (doubleTotalAmount == 0 || doublesertotal == 0)
                //{
                //    if (doubleTotalAmount == 0)
                //    {
                //        inttotalcount = inttotalcount + 8;
                //        intthirty = 8;
                //    }
                //    else
                //        intfive = 0;
                //}
                //else
                {

                    if (totaldive >= 11)
                    {
                        inttotalcount = inttotalcount + 5;
                        intthirty = 5;
                    }
                    //else if (totaldive >= 17 && totaldive <= 29.99)
                    //{
                    //    inttotalcount = inttotalcount + 7;
                    //    intseventto29 = 7;
                    //}
                    //else if (totaldive >= 11 && totaldive <= 16.99)
                    //{
                    //    inttotalcount = inttotalcount + 6;
                    //    intelevento16 = 6;
                    //}
                    //else if (totaldive >= 6 && totaldive <= 10.99)
                    //{
                    //    inttotalcount = inttotalcount + 2;
                    //    intsixtoten = 2;
                    //}
                    //else if (totaldive <= 5.99)
                    //{
                    //    if (doubleTotalAmount == 0 || doublesertotal == 0)
                    //    {
                    //        intfive = 0;

                    //    }
                    //    else
                    //    {
                    //        inttotalcount = inttotalcount + 1;
                    //        intfive = 1;
                    //    }
                    //}
                }

                X_Pos = 525;
                Y_Pos -= 48;//58;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 525;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Consumption Rate

                X_Pos = 525;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Poverty 

                X_Pos = 525;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                string Fname = string.Empty; string dob = string.Empty;
                if (SnpEntity != null)
                {
                    Fname = SnpEntity.NameixFi; dob = SnpEntity.AltBdate.Trim();
                }


                //DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("ALS", "ALL", string.Empty, string.Empty, string.Empty, string.Empty, Fname, string.Empty,
                //              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dob, BaseForm.UserID, string.Empty, string.Empty);
                //int FirstTime = 0;
                //if(ds.Tables[0].Rows.Count>0)
                //{
                //    if(ds.Tables[0].Rows.Count==1) FirstTime = 3;

                //    //foreach(DataRow dr in ds.Tables[0].Rows)
                //    //{
                //    //    if(dr["Agency"].ToString()!=BaseForm.BaseAgency && dr["Dept"].ToString() != BaseForm.BaseDept && dr["Prog"].ToString() != BaseForm.BaseProg && dr["SnpYear"].ToString() != BaseForm.BaseYear)
                //    //    {
                //    //        FirstTime = 3;
                //    //        break;
                //    //    }
                //    //}
                //}

                int FirstApp = 0; int ECMS = 0;
                if (propADDCUST.Count > 0)
                {
                    AddCustEntity CusEnt = propADDCUST.Find(u => u.ACTCODE == "C01667" && u.ACTSNPFAMILYSEQ == "9999999");
                    if (CusEnt != null)
                    {
                        if (CusEnt.ACTMULTRESP.Trim() == "Y") { FirstApp = 3; inttotalcount = inttotalcount + FirstApp; }
                    }

                    AddCustEntity CusEnt1 = propADDCUST.Find(u => u.ACTCODE == "C01668" && u.ACTSNPFAMILYSEQ == "9999999");
                    if (CusEnt1 != null)
                    {
                        if (CusEnt1.ACTMULTRESP.Trim() == "Y") { ECMS = 3; inttotalcount = inttotalcount + ECMS; }
                    }

                }


                Y_Pos -= 42;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(FirstApp.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(ECMS.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 36;
                X_Pos = 525;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 60;

                if (CEAPINVs.Count > 0)
                {
                    CEAPINVEntity entity = CEAPINVs.Find(u => u.CPINV_PRIORTY == "1");
                    if (entity != null)
                    {
                        X_Pos = 200;
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_LOW.ToString() + "-" + entity.CPINV_HIGH.ToString() + " Points", TableFont), X_Pos, Y_Pos - 7, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_MAX_INV.ToString(), Times), 191, Y_Pos - 20, 0);

                        if (int.Parse(entity.CPINV_LOW) <= inttotalcount && int.Parse(entity.CPINV_HIGH) >= inttotalcount)
                        {
                            _image_Tick.SetAbsolutePosition(40, Y_Pos - 15);
                            cb.AddImage(_image_Tick);
                        }

                    }
                    entity = CEAPINVs.Find(u => u.CPINV_PRIORTY == "2");
                    if (entity != null)
                    {
                        Y_Pos -= 33;
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_LOW.ToString() + "-" + entity.CPINV_HIGH.ToString() + " Points", TableFont), X_Pos, Y_Pos - 7, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_MAX_INV.ToString(), Times), 192, Y_Pos - 18, 0);

                        if (int.Parse(entity.CPINV_LOW) <= inttotalcount && int.Parse(entity.CPINV_HIGH) >= inttotalcount)
                        {
                            _image_Tick.SetAbsolutePosition(40, Y_Pos - 15);
                            cb.AddImage(_image_Tick);
                        }

                    }
                    entity = CEAPINVs.Find(u => u.CPINV_PRIORTY == "3");
                    if (entity != null)
                    {
                        Y_Pos -= 33;
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_LOW.ToString() + "-" + entity.CPINV_HIGH.ToString() + " Points", TableFont), X_Pos, Y_Pos - 7, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_MAX_INV.ToString(), Times), 192, Y_Pos - 18, 0);

                        if (int.Parse(entity.CPINV_LOW) <= inttotalcount && int.Parse(entity.CPINV_HIGH) >= inttotalcount)
                        {
                            _image_Tick.SetAbsolutePosition(40, Y_Pos - 15);
                            cb.AddImage(_image_Tick);
                        }
                    }
                }
                else
                {
                    X_Pos = 200;
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("20-31 Points", TableFont), X_Pos, Y_Pos - 7, 0);
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("12", Times), 191, Y_Pos - 20, 0);

                    if (inttotalcount >= 20)
                    {
                        _image_Tick.SetAbsolutePosition(40, Y_Pos - 15);
                        cb.AddImage(_image_Tick);
                    }

                    Y_Pos -= 33;
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("11-19 Points", TableFont), X_Pos, Y_Pos - 7, 0);
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("8", Times), 192, Y_Pos - 20, 0);

                    if (inttotalcount >= 11 && inttotalcount <= 19)
                    {
                        _image_Tick.SetAbsolutePosition(40, Y_Pos - 15);
                        cb.AddImage(_image_Tick);
                    }

                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0-10 Points", TableFont), X_Pos, Y_Pos - 7, 0);
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("6", Times), 192, Y_Pos - 20, 0);

                    if (inttotalcount <= 10)
                    {
                        _image_Tick.SetAbsolutePosition(40, Y_Pos - 15);
                        cb.AddImage(_image_Tick);
                    }

                }


                //if (inttotalcount >= 20)
                //{
                //    X_Pos = 40;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 15);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount >= 11 && inttotalcount <= 19)
                //{
                //    X_Pos = 40;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 15);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount <= 10)
                //{
                //    X_Pos = 370;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                //    cb.AddImage(_image_Tick);
                //}

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }


        #endregion

        #region RPMC Priority Sheet Form

        private void On_RPMC_PriorityRankingForm()
        {

            #region  PDF Name

            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "RPMC_Priority_Rating.pdf";

            PdfName = "RPMC_Priority_Sheet";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);

            #endregion

            #region Font Styles

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 13);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));

            _image_Tick.ScalePercent(60f);

            #endregion

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            List<AddCustEntity> propADDCUST = new List<AddCustEntity>();
            AddCustEntity Search_AddCust = new AddCustEntity(true);
            Search_AddCust.ACTAGENCY = BaseForm.BaseAgency; Search_AddCust.ACTDEPT = BaseForm.BaseDept; Search_AddCust.ACTPROGRAM = BaseForm.BaseProg;
            Search_AddCust.ACTYEAR = BaseForm.BaseYear; Search_AddCust.ACTAPPNO = BaseForm.BaseApplicationNo;
            propADDCUST = _model.CaseMstData.Browse_ADDCUST(Search_AddCust, "Browse");

            List<CEAPINVEntity> CEAPINVs = new List<CEAPINVEntity>();
            if (CEAPCNTL_List.Count > 0)
                CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0;
            int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30;
                Y_Pos = 760;

                X_Pos = 116;
                Y_Pos -= 118;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 276;

                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");

                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));

                int inteldercount = 0;
                if (casesnpElder.Count > 0)
                    inteldercount = 4;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0)
                    intyoungercount = 4;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0)
                    intdisablecount = 4;
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0;
                int intsenvtyfive = 0;
                int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 4;
                    intfity = 4;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 3;
                    intsenvtyfive = 3;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 2;
                    intonefiftyfive = 2;
                }

                int int1000plus = 0;

                if (doublesertotal >= 1000)
                {
                    inttotalcount = inttotalcount + 4;
                    int1000plus = 4;
                }

                int intthirty = 0;

                if (totaldive >= 11)
                {
                    inttotalcount = inttotalcount + 5;
                    intthirty = 5;
                }

                X_Pos = 525;
                Y_Pos -= 51;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Family Characterisitics 

                //List<CaseSnpEntity> casesnpEnrlSecEduc = casesnpEligbulity.FindAll(u => u.Education == "C");

                //int intEnrlSecEduc = 0;
                //if (casesnpEnrlSecEduc.Count > 0)
                //    intEnrlSecEduc = 3;
                //inttotalcount = inttotalcount + intEnrlSecEduc;

                List<CaseSnpEntity> casesnpVetStatus = casesnpEligbulity.FindAll(u => u.MilitaryStatus == "V");

                int intVetStatus = 0;
                if (casesnpVetStatus.Count > 0)
                    intVetStatus = 3;
                inttotalcount = inttotalcount + intVetStatus;

                //List<CaseSnpEntity> casesnpEnrlSecEduc = casesnpEligbulity.FindAll(u => u.Education == "C");

                //int intEnrlSecEduc = 0;
                //if (casesnpEnrlSecEduc.Count > 0)
                //    intEnrlSecEduc = 3;
                //inttotalcount = inttotalcount + intEnrlSecEduc;

                int RegTexas = 0; int EnrlCM = 0;
                if (propADDCUST.Count > 0)
                {
                    AddCustEntity CusEnt = propADDCUST.Find(u => u.ACTCODE == "C01617" && u.ACTSNPFAMILYSEQ == "9999999");
                    if (CusEnt != null)
                    {
                        if (CusEnt.ACTMULTRESP.Trim() == "Y") { RegTexas = 3; inttotalcount = inttotalcount + RegTexas; }
                    }
                    
                    CusEnt = propADDCUST.Find(u => u.ACTCODE == "C01619" && u.ACTSNPFAMILYSEQ == "9999999");
                    if (CusEnt != null)
                    {
                        if (CusEnt.ACTMULTRESP.Trim() == "Y") { EnrlCM = 3; inttotalcount = inttotalcount + EnrlCM; }
                    }
                }


                X_Pos = 525;
                Y_Pos -= 54;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(EnrlCM.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intVetStatus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(RegTexas.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Poverty 

                X_Pos = 525;
                Y_Pos -= 54;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Energy Burden

                X_Pos = 525;
                Y_Pos -= 54;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Consumption Rate

                X_Pos = 525;
                Y_Pos -= 54;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // First Time Participant

                int FirstApp = 0;
                if (propADDCUST.Count > 0)
                {
                    AddCustEntity CusEnt = propADDCUST.Find(u => u.ACTCODE == "C01618" && u.ACTSNPFAMILYSEQ == "9999999");
                    if (CusEnt != null)
                    {
                        if (CusEnt.ACTMULTRESP.Trim() == "Y") { FirstApp = 3; inttotalcount = inttotalcount + FirstApp; }
                    }
                }
                //if(SPM_Entity!=null)
                //{
                //    if (SPM_Entity.SPM_FIRST_APP == "Y") { FirstApp = 3; inttotalcount = inttotalcount + FirstApp; }
                //}


                X_Pos = 525;
                Y_Pos -= 32;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(FirstApp.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                string Fname = string.Empty;
                string dob = string.Empty;
                if (SnpEntity != null)
                {
                    Fname = SnpEntity.NameixFi;
                    dob = SnpEntity.AltBdate.Trim();
                }

                Y_Pos -= 42;
                X_Pos = 525;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 60;

                if (inttotalcount <= 10)
                {
                    cb.Rectangle(384, 163, 172, 22);
                }
                if (inttotalcount >= 11 && inttotalcount <= 16)
                {
                    cb.Rectangle(212, 163, 172, 22);

                }
                if (inttotalcount >= 17 && inttotalcount <= 50)
                {
                    cb.Rectangle(39, 163, 172, 22);
                }

                cb.SetColorFill(new BaseColor(179, 216, 167));

                PdfGState gs = new PdfGState();
                gs.FillOpacity = 0.5f;
                cb.SetGState(gs);

                cb.Fill();


                //if (CEAPINVs.Count > 0)
                //{
                //    CEAPINVEntity entity = CEAPINVs.Find(u => u.CPINV_PRIORTY == "1");
                //    if (entity != null)
                //    {
                //        X_Pos = 200;
                //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_LOW.ToString() + "-" + entity.CPINV_HIGH.ToString() + " Points", TableFont), X_Pos, Y_Pos - 7, 0);
                //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_MAX_INV.ToString(), Times), 191, Y_Pos - 20, 0);

                //    }
                //    entity = CEAPINVs.Find(u => u.CPINV_PRIORTY == "2");
                //    if (entity != null)
                //    {
                //        Y_Pos -= 33;
                //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_LOW.ToString() + "-" + entity.CPINV_HIGH.ToString() + " Points", TableFont), X_Pos, Y_Pos - 7, 0);
                //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_MAX_INV.ToString(), Times), 192, Y_Pos - 18, 0);

                //    }
                //    entity = CEAPINVs.Find(u => u.CPINV_PRIORTY == "3");
                //    if (entity != null)
                //    {
                //        Y_Pos -= 33;
                //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_LOW.ToString() + "-" + entity.CPINV_HIGH.ToString() + " Points", TableFont), X_Pos, Y_Pos - 7, 0);
                //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(entity.CPINV_MAX_INV.ToString(), Times), 192, Y_Pos - 18, 0);
                //    }
                //}
                //else
                //{
                //    //X_Pos = 200;
                //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("20-31 Points", TableFont), X_Pos, Y_Pos - 7, 0);
                //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("12", Times), 191, Y_Pos - 20, 0);

                //    //Y_Pos -= 33;
                //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("11-19 Points", TableFont), X_Pos, Y_Pos - 7, 0);
                //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("8", Times), 192, Y_Pos - 20, 0);

                //    //if (inttotalcount >= 11 && inttotalcount <= 19)
                //    //{
                //    //    _image_Tick.SetAbsolutePosition(40, Y_Pos - 15);
                //    //    cb.AddImage(_image_Tick);
                //    //}

                //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0-10 Points", TableFont), X_Pos, Y_Pos - 7, 0);
                //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("6", Times), 192, Y_Pos - 20, 0);

                //}

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }
            }
            catch (Exception ex) { }

            Hstamper.Close();

            /** SEND EMAIL **/
            //SendEmail();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }

        #endregion


        #region CVCAA Priority Sheet Form

        private void On_CVCAA_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "CVCAA_Priority_Ranking_" + BaseForm.BaseYear + ".pdf";

            //ReaderName = propReportPath + "\\" + "PCS_Priority_Ranking_" + BaseForm.BaseYear + ".pdf";

            PdfName = "CVCAA_Priority_Sheet";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            List<AddCustEntity> propADDCUST = new List<AddCustEntity>();
            AddCustEntity Search_AddCust = new AddCustEntity(true);
            Search_AddCust.ACTAGENCY = BaseForm.BaseAgency; Search_AddCust.ACTDEPT = BaseForm.BaseDept; Search_AddCust.ACTPROGRAM = BaseForm.BaseProg;
            Search_AddCust.ACTYEAR = BaseForm.BaseYear; Search_AddCust.ACTAPPNO = BaseForm.BaseApplicationNo;
            propADDCUST = _model.CaseMstData.Browse_ADDCUST(Search_AddCust, "Browse");

            //if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            //    CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, BaseForm.BaseYear, string.Empty, string.Empty);

            List<CEAPINVEntity> CEAPINVs = new List<CEAPINVEntity>();
            if (CEAPCNTL_List.Count > 0)
                CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 50; Y_Pos = 760;

                X_Pos = 105; Y_Pos -= 145;//91;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 280;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                //List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));
                int inteldercount = 0;
                if (casesnpElder.Count > 0) inteldercount = 5;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0) intyoungercount = 5;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0) intdisablecount = 5;
                inttotalcount = inttotalcount + intdisablecount;

                int intVeterancount = 0;
                //if (BaseForm.BaseYear == "2024")
                //{
                //    List<CaseSnpEntity> casesnpVetran = casesnpEligbulity.FindAll(u => (u.MilitaryStatus.ToString().ToUpper() == "V") && u.Status == "A");
                //    if (casesnpVetran.Count > 0) intVeterancount = 4;
                //}
                //else
                {
                    List<CaseSnpEntity> casesnpVetran = casesnpEligbulity.FindAll(u => (u.MilitaryStatus.ToString().ToUpper() == "V" || u.MilitaryStatus.ToString().ToUpper() == "A") && u.Status == "A");
                    if (casesnpVetran.Count > 0) intVeterancount = 4;
                }
                inttotalcount = inttotalcount + intVeterancount;

                int intage6to8 = 0;
                //if (BaseForm.BaseYear == "2024")
                //{
                //    List<CaseSnpEntity> casesnp6to8 = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(6))) && ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(8))));
                //    if (casesnp6to8.Count > 0) intage6to8 = 3;
                //}
                //else
                {
                    List<CaseSnpEntity> casesnp6to18 = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(6))) && ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(18))));
                    if (casesnp6to18.Count > 0) intage6to8 = 3;
                }
                inttotalcount = inttotalcount + intage6to8;

                int RuralClient = 0; int CMC = 0;
                if (propADDCUST.Count > 0)
                {
                    AddCustEntity CusEnt = propADDCUST.Find(u => u.ACTCODE == "C01622" && u.ACTSNPFAMILYSEQ == "9999999");
                    if (CusEnt != null)
                    {
                        if (CusEnt.ACTMULTRESP.Trim() == "Y") { RuralClient = 4; inttotalcount = inttotalcount + RuralClient; }
                    }

                    AddCustEntity CusEnt1 = propADDCUST.Find(u => u.ACTCODE == "C01623" && u.ACTSNPFAMILYSEQ == "9999999");
                    if (CusEnt1 != null)
                    {
                        if (CusEnt1.ACTMULTRESP.Trim() == "Y") { CMC = 3; inttotalcount = inttotalcount + CMC; }
                    }

                }


                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 5;
                    intfity = 5;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 4;
                    intsenvtyfive = 4;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 3;
                    intonefiftyfive = 3;
                }
                //else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    inttwentytofifty = 1;
                //}
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal >= 1000)
                {
                    inttotalcount = inttotalcount + 7;
                    int1000plus = 7;
                }
                //else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                //{
                //    inttotalcount = inttotalcount + 4;
                //    int500above = 4;
                //}
                //else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    int250above = 3;
                //}
                //else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    int250below = 2;
                //}



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                //if (doubleTotalAmount == 0 || doublesertotal == 0)
                //{
                //    if (doubleTotalAmount == 0)
                //    {
                //        inttotalcount = inttotalcount + 8;
                //        intthirty = 8;
                //    }
                //    else
                //        intfive = 0;
                //}
                //else
                {

                    if (totaldive >= 11)
                    {
                        inttotalcount = inttotalcount + 9;
                        intthirty = 9;
                    }
                    //else if (totaldive >= 17 && totaldive <= 29.99)
                    //{
                    //    inttotalcount = inttotalcount + 7;
                    //    intseventto29 = 7;
                    //}
                    //else if (totaldive >= 11 && totaldive <= 16.99)
                    //{
                    //    inttotalcount = inttotalcount + 6;
                    //    intelevento16 = 6;
                    //}
                    //else if (totaldive >= 6 && totaldive <= 10.99)
                    //{
                    //    inttotalcount = inttotalcount + 2;
                    //    intsixtoten = 2;
                    //}
                    //else if (totaldive <= 5.99)
                    //{
                    //    if (doubleTotalAmount == 0 || doublesertotal == 0)
                    //    {
                    //        intfive = 0;

                    //    }
                    //    else
                    //    {
                    //        inttotalcount = inttotalcount + 1;
                    //        intfive = 1;
                    //    }
                    //}
                }

                X_Pos = 525;
                Y_Pos -= 55;//48;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intVeterancount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;//14
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(RuralClient.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intage6to8.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;//15
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(CMC.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 525;
                Y_Pos -= 58; //43
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Consumption Rate

                X_Pos = 525;
                Y_Pos -= 43;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Poverty 

                X_Pos = 525;
                Y_Pos -= 44;//42
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 22;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 22;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                string Fname = string.Empty; string dob = string.Empty;
                if (SnpEntity != null)
                {
                    Fname = SnpEntity.NameixFi; dob = SnpEntity.AltBdate.Trim();
                }


                //DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("ALS", "ALL", string.Empty, string.Empty, string.Empty, string.Empty, Fname, string.Empty,
                //              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dob, BaseForm.UserID, string.Empty, string.Empty);
                //int FirstTime = 0;
                //if(ds.Tables[0].Rows.Count>0)
                //{
                //    if(ds.Tables[0].Rows.Count==1) FirstTime = 3;

                //    //foreach(DataRow dr in ds.Tables[0].Rows)
                //    //{
                //    //    if(dr["Agency"].ToString()!=BaseForm.BaseAgency && dr["Dept"].ToString() != BaseForm.BaseDept && dr["Prog"].ToString() != BaseForm.BaseProg && dr["SnpYear"].ToString() != BaseForm.BaseYear)
                //    //    {
                //    //        FirstTime = 3;
                //    //        break;
                //    //    }
                //    //}
                //}


                //Y_Pos -= 35; //42
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 26;//20 //36
                X_Pos = 525;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 40;//60

                if (inttotalcount >= 30)
                {
                    _image_Tick.SetAbsolutePosition(125, Y_Pos - 15);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 16 && inttotalcount <= 29) //inttotalcount >= 10
                {
                    _image_Tick.SetAbsolutePosition(320, Y_Pos - 15);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 15)
                {
                    _image_Tick.SetAbsolutePosition(500, Y_Pos - 15);
                    cb.AddImage(_image_Tick);
                }


                //if (inttotalcount >= 20)
                //{
                //    X_Pos = 40;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 15);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount >= 11 && inttotalcount <= 19)
                //{
                //    X_Pos = 40;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 15);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount <= 10)
                //{
                //    X_Pos = 370;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                //    cb.AddImage(_image_Tick);
                //}

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }

        #endregion

        #region NCCAA Priority Sheet Form

        private void On_NCCAA_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "NCCAA_Priority_Rating.pdf";



            PdfName = "NCCAA_Priority_Rating";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 100; Y_Pos -= 138;//105;

                int inttotalcount = 0;

                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("App# ", TblFontBold), X_Pos-50, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 550;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("Name: "+BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60)))); //&& (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                int inteldercount = 0;
                if (casesnpElder.Count > 0) inteldercount = 5;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0) intyoungercount = 5;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0) intdisablecount = 5;
                inttotalcount = inttotalcount + intdisablecount;

                List<CaseSnpEntity> casesnpvetran = casesnpEligbulity.FindAll(u => u.MilitaryStatus.ToString().ToUpper() == "V" && u.Status == "A");
                int intVetCount = 0;
                if (casesnpvetran.Count > 0) intVetCount = 5;
                inttotalcount = inttotalcount + intVetCount;

                //int intNoneabove = 0;
                //if (inttotalcount == 0)
                //{
                //    inttotalcount = inttotalcount + intNoneabove;
                //    intNoneabove = 1;
                //}
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 5;
                    intfity = 5;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 5;
                    intsenvtyfive = 5;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 5;
                    intonefiftyfive = 5;
                }
                //else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    inttwentytofifty = 1;
                //}
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal > 1000)
                {
                    inttotalcount = inttotalcount + 5;
                    int1000plus = 5;
                }
                //else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                //{
                //    inttotalcount = inttotalcount + 4;
                //    int500above = 4;
                //}
                //else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    int250above = 3;
                //}
                //else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    int250below = 2;
                //}



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                if (doubleTotalAmount == 0 || doublesertotal == 0)
                {
                    //if (doubleTotalAmount == 0)
                    //{
                    //    inttotalcount = inttotalcount + 8;
                    //    intthirty = 8;
                    //}
                    //else
                    //    intfive = 0;
                }
                else
                {

                    ////if (totaldive >= 30)
                    ////{
                    ////    inttotalcount = inttotalcount + 8;
                    ////    intthirty = 8;
                    ////}
                    ////else if (totaldive >= 17 && totaldive <= 29.99)
                    ////{
                    ////    inttotalcount = inttotalcount + 7;
                    ////    intseventto29 = 7;
                    ////}
                    //if (totaldive >= 11) //&& totaldive <= 16.99
                    //{
                    //    inttotalcount = inttotalcount + 6;
                    //    intelevento16 = 6;
                    //}
                    ////else if (totaldive >= 6 && totaldive <= 10.99)
                    ////{
                    ////    inttotalcount = inttotalcount + 2;
                    ////    intsixtoten = 2;
                    ////}
                    ////else if (totaldive <= 5.99)
                    ////{
                    ////    if (doubleTotalAmount == 0 || doublesertotal == 0)
                    ////    {
                    ////        intfive = 0;

                    ////    }
                    ////    else
                    ////    {
                    ////        inttotalcount = inttotalcount + 1;
                    ////        intfive = 1;
                    ////    }
                    ////}
                }

                X_Pos = 510;
                Y_Pos -= 74;//58;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 24;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 25;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intVetCount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

              //  X_Pos = 510;
              //  Y_Pos -= 36;
              //  //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
              //  //Y_Pos -= 14;
              //  //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
              //  //Y_Pos -= 14;
              //////  ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
              //  //Y_Pos -= 14;
              //  //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
              //  //Y_Pos -= 14;
              //  //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

              //  //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doublesertotal.ToString(), TblFontBold), 210, 476, 0);
              //  //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), 210, 461, 0);

              //  ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(doublesertotal.ToString("0.00"), TblFontBold), 240, Y_Pos-14, 0);
              //  ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(doubleTotalAmount.ToString("0.00"), TblFontBold), 240, Y_Pos - 28, 0);


                // Consumption Rate

                X_Pos = 510;
                Y_Pos -= 29;//63;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Poverty 

                X_Pos = 510;
                Y_Pos -= 50;//52;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 23;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 23;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                //Y_Pos -= 40;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0".ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 34;//32;
                X_Pos = 510;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 24;
                if (inttotalcount >= 29)
                {
                    X_Pos = 140;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 19 && inttotalcount <= 28)
                {
                    X_Pos = 330;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 18)
                {
                    X_Pos = 510;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }


        #endregion

        #region FORTWORTH Priority Sheet Form

        private void On_Fortworth_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "FORTWORTH_Priority_Rating.pdf";



            PdfName = "FORTWORTH_Priority_Rating";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 150; Y_Pos -= 135;

                int inttotalcount = 0;

                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doublesertotal.ToString(),TblFontBold), X_Pos - 70, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), X_Pos-200, Y_Pos, 0);


                X_Pos = 240;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 390;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(totaldive.ToString()+ "%", TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60)))); //&& (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                int inteldercount = 0;
                if (casesnpElder.Count > 0) inteldercount = 4;
                //inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0) intyoungercount = 4;
                //inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0) intdisablecount = 4;
                //inttotalcount = inttotalcount + intdisablecount;

                List<CaseSnpEntity> casesnpvetran = casesnpEligbulity.FindAll(u => u.MilitaryStatus.ToString().ToUpper() == "V" && u.Status == "A");
                int intVetCount = 0;
                if (casesnpvetran.Count > 0) intVetCount = 4;
                //inttotalcount = inttotalcount + intVetCount;

                //int intNoneabove = 0;
                //if (inttotalcount == 0)
                //{
                //    inttotalcount = inttotalcount + intNoneabove;
                //    intNoneabove = 1;
                //}
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 4;
                    intfity = 4;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 3;
                    intsenvtyfive = 3;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 2;
                    intonefiftyfive = 2;
                }
                //else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    inttwentytofifty = 1;
                //}
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal > 1000)
                {
                    inttotalcount = inttotalcount + 4;
                    int1000plus = 4;
                }
                //else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                //{
                //    inttotalcount = inttotalcount + 4;
                //    int500above = 4;
                //}
                //else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    int250above = 3;
                //}
                //else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    int250below = 2;
                //}


                int Householcnt = 0;
                if (intdisablecount > 0) Householcnt = 4;
                else if(inteldercount> 0) Householcnt = 4;
                else if(intyoungercount> 0) Householcnt=4;
                else if(intVetCount> 0)Householcnt=4;

                if(Householcnt> 0) { inttotalcount = inttotalcount + Householcnt; }

                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                if (doubleTotalAmount == 0 || doublesertotal == 0)
                {
                    //if (doubleTotalAmount == 0)
                    //{
                    //    inttotalcount = inttotalcount + 8;
                    //    intthirty = 8;
                    //}
                    //else
                    //    intfive = 0;
                }
                else
                {
                    if (totaldive >= 5 && totaldive <= 7.99)
                    {
                        inttotalcount = inttotalcount + 2;
                        intelevento16 = 2;
                    }
                    else if (totaldive >= 8 && totaldive <= 10.99)
                    {
                        inttotalcount = inttotalcount + 3;
                        intelevento16 = 3;
                    }
                    else if(totaldive>=11)
                    {
                        inttotalcount = inttotalcount + 5;
                        intelevento16 = 5;
                    }
                    
                }

                int Poverty = 0;
                if(intfity> 0) { Poverty = intfity; }
                else if(intsenvtyfive> 0) { Poverty = intsenvtyfive;}
                else if(intonefiftyfive> 0) { Poverty = intonefiftyfive; }

                X_Pos = 470;
                Y_Pos -= 58;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 25;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intVetCount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 470;
                Y_Pos -= 50;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doublesertotal.ToString(), TblFontBold), 210, 476, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), 210, 461, 0);


                Y_Pos -= 103;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Householcnt.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Consumption Rate

                //X_Pos = 510;
                Y_Pos -= 72;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Poverty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Poverty 

                X_Pos = 470;
                Y_Pos -= 28;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 23;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 23;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                //Y_Pos -= 40;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0".ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                //Y_Pos -= 32;
                //X_Pos = 510;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                if(inteldercount>0 || intyoungercount>0 || intdisablecount>0)
                {
                    X_Pos = 60; Y_Pos -= 27;
                    
                    if (inttotalcount>=14)
                    {
                        _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                        cb.AddImage(_image_Tick);
                    }
                    else if (inttotalcount <= 13)
                    {
                        Y_Pos -= 24;
                        //X_Pos = 305;
                        _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 34);
                        cb.AddImage(_image_Tick);
                    }
                }
                else
                {
                    X_Pos = 60; Y_Pos -= 27;
                    Y_Pos -= 24; X_Pos = 305;
                    if (inttotalcount >= 14)
                    {
                        _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                        cb.AddImage(_image_Tick);
                    }
                    else if (inttotalcount <= 13)
                    {
                        //Y_Pos -= 24;
                        _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 34);
                        cb.AddImage(_image_Tick);
                    }
                }

                Y_Pos -= 85; X_Pos = 185;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                 X_Pos = 455;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                //Y_Pos -= 24;
                //if (inttotalcount >= 29)
                //{
                //    X_Pos = 140;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount >= 19 && inttotalcount <= 28)
                //{
                //    X_Pos = 330;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount <= 18)
                //{
                //    X_Pos = 305;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                //    cb.AddImage(_image_Tick);
                //}



                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }


        #endregion

        #region CSNT Priority Sheet Form
        private void On_CSNT_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG")
                ReaderName = propReportPath + "\\" + "BVCOG_Priority.pdf";
            else
                ReaderName = propReportPath + "\\" + "CSNT_Priority.pdf";



            PdfName = "CSNT_Priority";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 135; Y_Pos -= 115;

                int inttotalcount = 0;

                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("App# ", TblFontBold), X_Pos - 50, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 300;//280;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                //List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));
                int inteldercount = 0;
                if (casesnpElder.Count > 0) inteldercount = 10;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0) intyoungercount = 10;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0) intdisablecount = 10;
                inttotalcount = inttotalcount + intdisablecount;

                //List<CaseSnpEntity> casesnpvetran = casesnpEligbulity.FindAll(u => u.MilitaryStatus.ToString().ToUpper() == "V" && u.Status == "A");
                //int intVetCount = 0;
                //if (casesnpvetran.Count > 0) intVetCount = 5;
                //inttotalcount = inttotalcount + intVetCount;

                ////int intNoneabove = 0;
                ////if (inttotalcount == 0)
                ////{
                ////    inttotalcount = inttotalcount + intNoneabove;
                ////    intNoneabove = 1;
                ////}
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 5;
                    intfity = 5;
                }
                if (intfity == 0)
                {
                    List<CaseVerEntity> caseVerList = _model.CaseMstData.GetCASEVeradpyalst(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty);
                    if (caseVerList.Count > 0)
                    {
                        if (caseVerList[0].CatElig == "A" || caseVerList[0].CatElig == "B" || caseVerList[0].CatElig == "O")
                        {
                            inttotalcount = inttotalcount + 5;
                            intfity = 5;
                        }
                    }
                }

                //else if (intmstpoverty >= 51 && intmstpoverty <= 75)
                //{
                //    inttotalcount = inttotalcount + 5;
                //    intsenvtyfive = 5;
                //}
                //else if (intmstpoverty >= 76 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 5;
                //    intonefiftyfive = 5;
                //}
                ////else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                ////{
                ////    inttotalcount = inttotalcount + 1;
                ////    inttwentytofifty = 1;
                ////}
                ////else if (intmstpoverty <= 151)
                ////{

                ////    intfiftyone = 0;
                ////}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal > 1000)
                {
                    inttotalcount = inttotalcount + 5;
                    int1000plus = 5;
                }
                //else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                //{
                //    inttotalcount = inttotalcount + 4;
                //    int500above = 4;
                //}
                //else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    int250above = 3;
                //}
                //else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    int250below = 2;
                //}



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                if (doubleTotalAmount == 0 || doublesertotal == 0)
                {
                    //if (doubleTotalAmount == 0)
                    //{
                    //    inttotalcount = inttotalcount + 8;
                    //    intthirty = 8;
                    //}
                    //else
                    //    intfive = 0;
                }
                else
                {

                    //if (totaldive >= 30)
                    //{
                    //    inttotalcount = inttotalcount + 8;
                    //    intthirty = 8;
                    //}
                    //else if (totaldive >= 17 && totaldive <= 29.99)
                    //{
                    //    inttotalcount = inttotalcount + 7;
                    //    intseventto29 = 7;
                    //}
                    if (totaldive >= 11) //&& totaldive <= 16.99
                    {
                        if (BaseForm.BaseAgencyControlDetails.AgyShortName.Trim() == "CSNT")
                        {
                            inttotalcount = inttotalcount + 15;
                            intelevento16 = 15;
                        }
                        else
                        {
                            inttotalcount = inttotalcount + 5;
                            intelevento16 = 5;
                        }
                    }
                    //else if (totaldive >= 6 && totaldive <= 10.99)
                    //{
                    //    inttotalcount = inttotalcount + 2;
                    //    intsixtoten = 2;
                    //}
                    //else if (totaldive <= 5.99)
                    //{
                    //    if (doubleTotalAmount == 0 || doublesertotal == 0)
                    //    {
                    //        intfive = 0;

                    //    }
                    //    else
                    //    {
                    //        inttotalcount = inttotalcount + 1;
                    //        intfive = 1;
                    //    }
                    //}
                }

                X_Pos = 510;
                Y_Pos -= 73;//58;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 24;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 24;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intVetCount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden 

                X_Pos = 510;
                Y_Pos -= 30;//36;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 14;
                ////ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 14;
                ////ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                ////ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doublesertotal.ToString(), TblFontBold), 210, 476, 0);
                ////ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), 210, 461, 0);

                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(doublesertotal.ToString("0.00"), TblFontBold), 250, Y_Pos - 19, 0);    //14
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(doubleTotalAmount.ToString("0.00"), TblFontBold), 250, Y_Pos - 33, 0);  //28

                // Consumption Rate

                X_Pos = 510;
                Y_Pos -= 22;//75;//63;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Poverty 

                X_Pos = 510;
                Y_Pos -= 50;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 23;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 23;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                //Y_Pos -= 40;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0".ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                //Y_Pos -= 10;//32;
                X_Pos = 510;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 42;
                if (inttotalcount >= 20)
                {
                    X_Pos = 150;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 10 && inttotalcount <= 15)
                {
                    X_Pos = 345;

                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount == 5)
                {
                    X_Pos = 480;//510;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }

        #endregion

        #region HCCAA Priority Sheet Form

        private void On_HCCAA_PriorityRankingForm()
        {

            #region  PDF Name

            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "HCCAA_Priority.pdf";

            PdfName = "HCCAA_Priority_Sheet";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);

            #endregion

            #region Font Styles

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 13);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\Green_rect_tick.png"));

            _image_Tick.ScalePercent(50f);

            #endregion

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            List<CEAPINVEntity> CEAPINVs = new List<CEAPINVEntity>();
            if (CEAPCNTL_List.Count > 0)
                CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0;
            int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30;
                Y_Pos = 760;

                X_Pos = 116;
                Y_Pos -= 95;//118;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 480;

                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");

                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));

                int intSSI = 0;int intVABen = 0;
                if (BaseForm.BaseCaseMstListEntity[0].IncomeTypes.Contains("D "))
                {
                    intSSI = 2;
                    inttotalcount = inttotalcount + intSSI;
                }
                if (BaseForm.BaseCaseMstListEntity[0].IncomeTypes.Contains("VN") || BaseForm.BaseCaseMstListEntity[0].IncomeTypes.Contains("VS"))
                {
                    intVABen = 0;
                    inttotalcount = inttotalcount + intVABen;
                }

                int inteldercount = 0;
                if (casesnpElder.Count > 0)
                    inteldercount = 4;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0)
                    intyoungercount = 4;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0)
                    intdisablecount = 4;
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0;
                int intsenvtyfive = 0;
                int intonefiftyfive = 0; int intmore150 = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 3;
                    intfity = 3;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 2;
                    intsenvtyfive = 2;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 1;
                    intonefiftyfive = 1;
                }
                else if (intmstpoverty >= 151)
                {
                    inttotalcount = inttotalcount + 0;
                    intmore150 = 0;
                }

                int int1000plus = 0;

                if (doublesertotal >= 1000)
                {
                    inttotalcount = inttotalcount + 4;
                    int1000plus = 4;
                }

                int intthirty = 0; int intbelowThirty = 0;

                if (totaldive >= 11)
                {
                    inttotalcount = inttotalcount + 5;
                    intthirty = 5;
                }
                else
                {
                    inttotalcount = inttotalcount + 2;
                    intbelowThirty = 2;
                }

                X_Pos = 510;
                Y_Pos -= 42;//51;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 22;//18
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 22; //18
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                int intnonabove = 0;
                if(intyoungercount==0 && intdisablecount==0 && inteldercount==0)
                {
                    inttotalcount = inttotalcount + 1;
                    intnonabove = 1;
                }
                Y_Pos -= 22; //18
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intnonabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 60;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intSSI.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 19;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intVABen.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Family Characterisitics 

                //List<CaseSnpEntity> casesnpEnrlSecEduc = casesnpEligbulity.FindAll(u => u.Education == "C");

                //int intEnrlSecEduc = 0;
                //if (casesnpEnrlSecEduc.Count > 0)
                //    intEnrlSecEduc = 3;
                //inttotalcount = inttotalcount + intEnrlSecEduc;

                List<CaseSnpEntity> casesnpVetStatus = casesnpEligbulity.FindAll(u => u.MilitaryStatus == "V");

                int intVetStatus = 0;
                if (casesnpVetStatus.Count > 0)
                    intVetStatus = 3;
                inttotalcount = inttotalcount + intVetStatus;

                //List<CaseSnpEntity> casesnpEnrlSecEduc = casesnpEligbulity.FindAll(u => u.Education == "C");

                //int intEnrlSecEduc = 0;
                //if (casesnpEnrlSecEduc.Count > 0)
                //    intEnrlSecEduc = 3;
                //inttotalcount = inttotalcount + intEnrlSecEduc;

                // Poverty 
                X_Pos = 510;
                Y_Pos -= 62;//54;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 22;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 22;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intmore150.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intVetStatus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);


                //// Poverty 

                //X_Pos = 525;
                //Y_Pos -= 54;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Consumption Rate

                X_Pos = 510;
                Y_Pos -= 54;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 510;
                Y_Pos -= 60;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 22;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intbelowThirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                //// First Time Participant

                //X_Pos = 525;
                //Y_Pos -= 32;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);

                CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                string Fname = string.Empty;
                string dob = string.Empty;
                if (SnpEntity != null)
                {
                    Fname = SnpEntity.NameixFi;
                    dob = SnpEntity.AltBdate.Trim();
                }

                Y_Pos -= 39;
                X_Pos = 510;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                
                X_Pos = 50;
                if (inttotalcount <= 7)
                {
                    Y_Pos -= 115;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 8);
                    cb.AddImage(_image_Tick);
                }
                if (inttotalcount >= 8 && inttotalcount <= 15)
                {
                    Y_Pos -= 96;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos-8 );
                    cb.AddImage(_image_Tick);

                }
                if (inttotalcount >= 16 && inttotalcount <= 23)
                {
                    Y_Pos -= 78;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 8);
                    cb.AddImage(_image_Tick);
                }
                if (inttotalcount >= 24 && inttotalcount <= 50)
                {
                    Y_Pos -= 60;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 8);
                    cb.AddImage(_image_Tick);
                }

                //cb.SetColorFill(new BaseColor(179, 216, 167));

                //PdfGState gs = new PdfGState();
                //gs.FillOpacity = 0.5f;
                //cb.SetGState(gs);

                //cb.Fill();

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }
            }
            catch (Exception ex) { }

            Hstamper.Close();

            /** SEND EMAIL **/
            //SendEmail();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }

        #endregion

        #region CACOST Priority Sheet Form
        private void On_CACOST_PriorityRankingForm()
        {

            #region  PDF Name

            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "CACOST_PRIORITY.pdf";

            PdfName = "CACOST_PRIORITY_Sheet";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);

            #endregion

            #region Font Styles

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 13);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\Green_rect_tick.png"));

            //_image_Tick.ScalePercent(50f);

            #endregion

            iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
            iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));
            _image_UnChecked.ScalePercent(80f);
            _image_Checked.ScalePercent(80f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            List<CEAPINVEntity> CEAPINVs = new List<CEAPINVEntity>();
            if (CEAPCNTL_List.Count > 0)
                CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0;
            int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = 0;
            if (doubleTotalAmount > 0)
                totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30;
                Y_Pos = 760;

                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), 508, 767, 0);

                X_Pos = 215;//192
                Y_Pos -= 32;//20;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 465; //442

                List<CaseSnpEntity> casesnplist = BaseForm.BaseCaseSnpEntity.FindAll(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);

                if (casesnplist.Count > 0)
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(casesnplist[0].AltBdate.Trim()), TblFontBold), X_Pos, Y_Pos, 0);

                string HN = string.Empty;
                string Apt = string.Empty;
                string Floor = string.Empty;
                string Suffix = string.Empty;
                string Street = string.Empty;
                string Direction = string.Empty;

                //string MailAddress = string.Empty;
                //string MailAddress1 = string.Empty;

                string AppAddress = string.Empty;
                string AppAddress1 = string.Empty;

                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Hn.Trim()))
                    HN = BaseForm.BaseCaseMstListEntity[0].Hn.Trim() + "  ";
                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Direction.Trim()))
                    Direction = BaseForm.BaseCaseMstListEntity[0].Direction.Trim() + "  ";

                Street = BaseForm.BaseCaseMstListEntity[0].Street.Trim() + "  ";
                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Suffix.Trim()))
                    Suffix = BaseForm.BaseCaseMstListEntity[0].Suffix.Trim() + "  ";
                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Apt.Trim()))
                    Apt = "Apt: " + BaseForm.BaseCaseMstListEntity[0].Apt.Trim() + "  ";
                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Flr.Trim()))
                    Floor = "Flr: " + BaseForm.BaseCaseMstListEntity[0].Flr.Trim();

                string zipplus = string.Empty;
                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Zipplus.Trim()))
                {
                    if (int.Parse(BaseForm.BaseCaseMstListEntity[0].Zipplus.ToString()) > 0)
                        zipplus = "-" + "0000".Substring(0, 4 - BaseForm.BaseCaseMstListEntity[0].Zipplus.Trim().Length) + BaseForm.BaseCaseMstListEntity[0].Zipplus.Trim();
                }
                AppAddress = HN + Direction + Street + Suffix + Apt + Floor;
                AppAddress1 = BaseForm.BaseCaseMstListEntity[0].City.Trim() + "  " + BaseForm.BaseCaseMstListEntity[0].State.Trim() + "  " + "00000".Substring(0, 5 - BaseForm.BaseCaseMstListEntity[0].Zip.Trim().Length) + BaseForm.BaseCaseMstListEntity[0].Zip.Trim() + zipplus;

                X_Pos = 215;//192
                Y_Pos -= 24; //41

                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(AppAddress + AppAddress1, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");

                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));

                int intSSI = 0; int intVABen = 0;
                if (BaseForm.BaseCaseMstListEntity[0].IncomeTypes.Contains("D "))
                {
                    intSSI = 2;
                    inttotalcount = inttotalcount + intSSI;
                }
                if (BaseForm.BaseCaseMstListEntity[0].IncomeTypes.Contains("VN") || BaseForm.BaseCaseMstListEntity[0].IncomeTypes.Contains("VS"))
                {
                    intVABen = 0;
                    inttotalcount = inttotalcount + intVABen;
                }

                int inteldercount = 0;
                //if (casesnpElder.Count > 0)
                //    inteldercount = 4;
                //inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                //if (casesnpyounger.Count > 0)
                //    intyoungercount = 4;
                //inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                //if (casesnpdisable.Count > 0)
                //    intdisablecount = 4;
                //inttotalcount = inttotalcount + intdisablecount;

                if (casesnpElder.Count > 0 || casesnpdisable.Count > 0 || casesnpyounger.Count > 0)
                    inteldercount = 4;
                inttotalcount = inttotalcount + inteldercount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0;
                int intsenvtyfive = 0;
                int intonefiftyfive = 0; int intmore150 = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                //if (intmstpoverty <= 50)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    intfity = 3;
                //}
                //else if (intmstpoverty >= 51 && intmstpoverty <= 75)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    intsenvtyfive = 2;
                //}
                //else if (intmstpoverty >= 76 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    intonefiftyfive = 1;
                //}
                //else if (intmstpoverty >= 151)
                //{
                //    inttotalcount = inttotalcount + 0;
                //    intmore150 = 0;
                //}

                int int1000plus = 0;

                if (doublesertotal >= 1000)
                {
                    inttotalcount = inttotalcount + 1;
                    int1000plus = 1;
                }

                int intthirty = 0; int intbelowThirty = 0;

                if (totaldive >= 11)
                {
                    inttotalcount = inttotalcount + 5;
                    intthirty = 5;
                }
                //else
                //{
                //    inttotalcount = inttotalcount + 2;
                //    intbelowThirty = 2;
                //}

                X_Pos = 543;
                Y_Pos -= 117;//135;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22;//18
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22; //18
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                int intnonabove = 0;
                if (intyoungercount == 0 && intdisablecount == 0 && inteldercount == 0)
                {
                    inttotalcount = inttotalcount + 1;
                    intnonabove = 1;
                }
                Y_Pos -= 35; //24
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(BaseForm.BaseCaseMstListEntity[0].FamIncome.Trim(), TblFontBold), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intnonabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 60;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intSSI.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 19;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intVABen.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Family Characterisitics 

                //List<CaseSnpEntity> casesnpEnrlSecEduc = casesnpEligbulity.FindAll(u => u.Education == "C");

                //int intEnrlSecEduc = 0;
                //if (casesnpEnrlSecEduc.Count > 0)
                //    intEnrlSecEduc = 3;
                //inttotalcount = inttotalcount + intEnrlSecEduc;

                //List<CaseSnpEntity> casesnpVetStatus = casesnpEligbulity.FindAll(u => u.MilitaryStatus == "V");

                //int intVetStatus = 0;
                //if (casesnpVetStatus.Count > 0)
                //    intVetStatus = 3;
                //inttotalcount = inttotalcount + intVetStatus;

                //List<CaseSnpEntity> casesnpEnrlSecEduc = casesnpEligbulity.FindAll(u => u.Education == "C");

                //int intEnrlSecEduc = 0;
                //if (casesnpEnrlSecEduc.Count > 0)
                //    intEnrlSecEduc = 3;
                //inttotalcount = inttotalcount + intEnrlSecEduc;

                //// Poverty 
                //X_Pos = 510;
                //Y_Pos -= 62;//54;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intmore150.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intVetStatus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 22;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);


                //// Poverty 

                //X_Pos = 525;
                //Y_Pos -= 54;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Consumption Rate

                X_Pos = 543;
                Y_Pos -= 35;//30
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(doublesertotal.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 48; //43
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Energy Burden

                X_Pos = 543;
                Y_Pos -= 37; //37
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(totaldive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 63; //45
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                //// First Time Participant

                //X_Pos = 525;
                //Y_Pos -= 32;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);

                CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                string Fname = string.Empty;
                string dob = string.Empty;
                if (SnpEntity != null)
                {
                    Fname = SnpEntity.NameixFi;
                    dob = SnpEntity.AltBdate.Trim();
                }

                Y_Pos -= 39;
                X_Pos = 543;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                if (CEAPCNTL_List.Count > 0)
                {
                    Vulner_Flag = Get_SNP_Vulnerable_Status();
                    Y_Pos -= 100; X_Pos = 80;
                    if (Vulner_Flag)
                    {
                        if (intmstpoverty < 51)
                        {
                            _image_Checked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_Checked);
                        }
                        else
                        {
                            _image_UnChecked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_UnChecked);
                        }
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0% TO 50.999%", TblFontBold), X_Pos, Y_Pos, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_VUL_50).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                        Y_Pos -= 20;
                        if (intmstpoverty >= 51 && intmstpoverty <= 75)
                        {
                            _image_Checked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_Checked);
                        }
                        else
                        {
                            _image_UnChecked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_UnChecked);
                        }
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("51% TO 75.999%", TblFontBold), X_Pos, Y_Pos, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_VUL_75).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                        Y_Pos -= 20;
                        if (intmstpoverty >= 76 && intmstpoverty <= 150)
                        {
                            _image_Checked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_Checked);
                        }
                        else
                        {
                            _image_UnChecked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_UnChecked);
                        }
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("76% TO 150%", TblFontBold), X_Pos, Y_Pos, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_VUL_150).ToString("N0"), TblFontBold), 545, Y_Pos, 0);

                    }
                    else
                    {
                        if (intmstpoverty < 51)
                        {
                            _image_Checked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_Checked);
                        }
                        else
                        {
                            _image_UnChecked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_UnChecked);
                        }
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0% TO 50.999%", TblFontBold), X_Pos, Y_Pos, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_NONVUL_50).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                        Y_Pos -= 20;
                        if (intmstpoverty >= 51 && intmstpoverty <= 75)
                        {
                            _image_Checked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_Checked);
                        }
                        else
                        {
                            _image_UnChecked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_UnChecked);
                        }
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("51% TO 75.999%", TblFontBold), X_Pos, Y_Pos, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_NONVUL_75).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                        Y_Pos -= 20;
                        if (intmstpoverty >= 76 && intmstpoverty <= 150)
                        {
                            _image_Checked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_Checked);
                        }
                        else
                        {
                            _image_UnChecked.SetAbsolutePosition(60, Y_Pos - 2);
                            cb.AddImage(_image_UnChecked);
                        }
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("76% TO 150%", TblFontBold), X_Pos, Y_Pos, 0);
                        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_NONVUL_150).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                    }

                    //if (Vulner_Flag)
                    //{
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0% TO 50.999%", TblFontBold), X_Pos, Y_Pos, 0);
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_VUL_50).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                    //    Y_Pos -= 20;
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("51% TO 75.999%", TblFontBold), X_Pos, Y_Pos, 0);
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_VUL_75).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                    //    Y_Pos -= 20;
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("76% TO 150%", TblFontBold), X_Pos, Y_Pos, 0);
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_VUL_150).ToString("N0"), TblFontBold), 545, Y_Pos, 0);

                    //}
                    //else
                    //{
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0% TO 50.999%", TblFontBold), X_Pos, Y_Pos, 0);
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_NONVUL_50).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                    //    Y_Pos -= 20;
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("51% TO 75.999%", TblFontBold), X_Pos, Y_Pos, 0);
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_NONVUL_75).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                    //    Y_Pos -= 20;
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("76% TO 150%", TblFontBold), X_Pos, Y_Pos, 0);
                    //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("$" + decimal.Parse(CEAPCNTL_List[0].CPCT_NONVUL_150).ToString("N0"), TblFontBold), 545, Y_Pos, 0);
                    //}



                }


                //X_Pos = 50;
                //if (inttotalcount <= 7)
                //{
                //    Y_Pos -= 115;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 8);
                //    cb.AddImage(_image_Tick);
                //}
                //if (inttotalcount >= 8 && inttotalcount <= 15)
                //{
                //    Y_Pos -= 96;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 8);
                //    cb.AddImage(_image_Tick);

                //}
                //if (inttotalcount >= 16 && inttotalcount <= 23)
                //{
                //    Y_Pos -= 78;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 8);
                //    cb.AddImage(_image_Tick);
                //}
                //if (inttotalcount >= 24 && inttotalcount <= 50)
                //{
                //    Y_Pos -= 60;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 8);
                //    cb.AddImage(_image_Tick);
                //}

                //cb.SetColorFill(new BaseColor(179, 216, 167));

                //PdfGState gs = new PdfGState();
                //gs.FillOpacity = 0.5f;
                //cb.SetGState(gs);

                //cb.Fill();

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }
            }
            catch (Exception ex) { }

            Hstamper.Close();

            /** SEND EMAIL **/
            //SendEmail();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }
        #endregion

        #region WCCAA Priority Sheet Form

        private void On_WCCAA_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "WCCAA_Priority.pdf";



            PdfName = "WCCAA_Priority_Sheet";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                Y_Pos -= 116; X_Pos = 110;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 110; Y_Pos -= 22;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 460;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseCaseMstListEntity[0].FamilyId, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                int inteldercount = 0;//casesnpElder.Count * 3;
                if (casesnpElder.Count > 0) inteldercount = 6;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0; //casesnpyounger.Count * 3;
                if (casesnpyounger.Count > 0) intyoungercount = 6;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0; //casesnpdisable.Count * 3;
                if (casesnpdisable.Count > 0) intdisablecount = 6;
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 0;
                }
                int intfity = 0; int intsenvtyfive = 0; int intonefiftyfive = 0; int intmore150 = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 4;
                    intfity = 4;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 3;
                    intsenvtyfive = 3;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 2;
                    intonefiftyfive = 2;
                }
                else if (intmstpoverty >= 151)
                {
                    inttotalcount = inttotalcount + 0;
                    intmore150 = 0;
                }
                //else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                //{
                //    inttotalcount = inttotalcount + 1;
                //    inttwentytofifty = 1;
                //}
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal > 1000)
                {
                    inttotalcount = inttotalcount + 6;
                    int1000plus = 6;
                }
                else if (doublesertotal >= 600 && doublesertotal <= 999.99)
                {
                    inttotalcount = inttotalcount + 5;
                    int500above = 5;
                }
                else if (doublesertotal >= 200 && doublesertotal <= 599.99)
                {
                    inttotalcount = inttotalcount + 4;
                    int250above = 4;
                }
                else if (doublesertotal >= 0 && doublesertotal <= 199.99)
                {
                    inttotalcount = inttotalcount + 3;
                    int250below = 3;
                }



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                if (doubleTotalAmount == 0 || doublesertotal == 0)
                {
                    if (doubleTotalAmount == 0)
                    {
                        inttotalcount = inttotalcount + 8;
                        intthirty = 8;
                    }
                    else
                        intfive = 0;
                }
                else
                {

                    if (totaldive >= 30)
                    {
                        inttotalcount = inttotalcount + 9;
                        intthirty = 9;
                    }
                    else if (totaldive >= 20 && totaldive <= 29.99)
                    {
                        inttotalcount = inttotalcount + 8;
                        intseventto29 = 8;
                    }
                    else if (totaldive >= 11 && totaldive <= 19.99)
                    {
                        inttotalcount = inttotalcount + 7;
                        intelevento16 = 7;
                    }
                    else if (totaldive >= 1 && totaldive <= 10.99)
                    {
                        inttotalcount = inttotalcount + 3;
                        intsixtoten = 3;
                    }
                    //else if (totaldive <= 5.99)
                    //{
                    //    //if (doubleTotalAmount == 0 || doublesertotal == 0)
                    //    if (doublesertotal == 0)
                    //    {
                    //        intfive = 0;

                    //    }
                    //    else
                    //    {
                    //        inttotalcount = inttotalcount + 1;
                    //        intfive = 1;
                    //    }
                    //}
                }

                X_Pos = 540;
                Y_Pos -= 42;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 16;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 16;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 16;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                int HOHparttimeemployment = 0; int HOHUnemp = 0; int RecSS = 0; int zeroinc = 0;
                CaseSnpEntity SnpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                if (SnpApplicant != null)
                {
                    if (SnpApplicant.WorkStatus.Trim() == "B")
                    { HOHparttimeemployment = 6; inttotalcount = inttotalcount + 6; }
                    if (SnpApplicant.WorkStatus.Trim() == "D" || SnpApplicant.WorkStatus.Trim() == "E" || SnpApplicant.WorkStatus.Trim() == "F")
                    { HOHUnemp = 6; inttotalcount = inttotalcount + 6; }
                    List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                    if (caseIncomeList.Count > 0)
                    {
                        CaseIncomeEntity SnpInc = caseIncomeList.Find(u => u.Type == "C" || u.Type == "D");
                        if (SnpInc != null) { RecSS = 6; inttotalcount = inttotalcount + 6; }
                    }

                }
                if (BaseForm.BaseCaseMstListEntity[0].ProgIncome.Trim() == "0.00")
                { zeroinc = 6; inttotalcount = inttotalcount + 6; }


                X_Pos = 540;
                Y_Pos -= 45;
                //HOH Part-Time Employment 
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(HOHparttimeemployment.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //HOH Receives SS/SSI Benefits 
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(RecSS.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //HOH Unemployed 
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(HOHUnemp.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Household Member Receives Means Tested Veterans Program Payments 
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(0.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Zero Income for Household 
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(zeroinc.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Poverty 

                X_Pos = 540;
                Y_Pos -= 48;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(0.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                // Consumption Rate

                X_Pos = 540;
                Y_Pos -= 44;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 540;
                Y_Pos -= 50;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 15;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                //Y_Pos -= 40;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("0".ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 38;
                X_Pos = 540;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 21; X_Pos = 540;
                if (inttotalcount >= 15)
                {
                    X_Pos = 540;
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("1".ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                    //_image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    //cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 10 && inttotalcount <= 14)
                {
                    //X_Pos = 205;
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("2".ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                    //_image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    //cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 9)
                {
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("3".ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                    //X_Pos = 370;
                    //_image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    //cb.AddImage(_image_Tick);
                }

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            /** SEND EMAIL **/
            //SendEmail();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }

        #endregion

        #region CCSCT Priority Sheet Form

        private void On_CCSCT_PriorityRankingForm()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "CCSCT_Priority_2025.pdf";



            PdfName = "CCSCT_Priority_Sheet";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            //if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            //    CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, BaseForm.BaseYear, string.Empty, string.Empty);

            List<CEAPINVEntity> CEAPINVs = new List<CEAPINVEntity>();
            if (CEAPCNTL_List.Count > 0)
                CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].ProgIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].ProgIncome);
            double totaldive = 0;
            if(doubleTotalAmount>0)
                totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 50; Y_Pos = 760;

                X_Pos = 110; Y_Pos -= 125;//91;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 310;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                //List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(60))));
                int inteldercount = 0;
                if (casesnpElder.Count > 0) inteldercount = 9;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = 0;
                if (casesnpyounger.Count > 0) intyoungercount = 9;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = 0;
                if (casesnpdisable.Count > 0) intdisablecount = 9;
                inttotalcount = inttotalcount + intdisablecount;


                //List<CaseSnpEntity> casesnpVetran = casesnpEligbulity.FindAll(u => u.MilitaryStatus.ToString().ToUpper() == "V" && u.Status == "A");
                //int intVeterancount = 0;
                //if (casesnpVetran.Count > 0) intVeterancount = 4;
                //inttotalcount = inttotalcount + intVeterancount;

                //List<CaseSnpEntity> casesnp6to8 = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(6))) && ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(8))));
                //int intage6to8 = 0;
                //if (casesnp6to8.Count > 0) intage6to8 = 3;
                //inttotalcount = inttotalcount + intage6to8;


                //int intNoneabove = 0;
                //if (inttotalcount == 0)
                //{
                //    inttotalcount = inttotalcount + intNoneabove;
                //    intNoneabove = 1;
                //}
                int intfity = 0; int intsenvtyfive = 0;int inthundred = 0; int intonefiftyfive = 0;int inttwentytofifty = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <51)
                {
                    inttotalcount = inttotalcount + 5;
                    intfity = 5;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <76)
                {
                    inttotalcount = inttotalcount + 4;
                    intsenvtyfive = 4;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <101)
                {
                    inttotalcount = inttotalcount + 3;
                    inthundred = 3;
                }
                else if (intmstpoverty >= 101 && intmstpoverty <126)
                {
                    inttotalcount = inttotalcount + 2;
                    inttwentytofifty = 2;
                }
                else if (intmstpoverty >= 126 && intmstpoverty <151)
                {
                    inttotalcount = inttotalcount + 1;
                    intonefiftyfive = 1;
                }
                //else if (intmstpoverty <= 151)
                //{

                //    intfiftyone = 0;
                //}

                int int1000plus = 0; int int500above = 0; int int250above = 0; int int250below = 0;
                if (doublesertotal >= 1000)
                {
                    inttotalcount = inttotalcount + 5;
                    int1000plus = 5;
                }
                //else if (doublesertotal >= 500 && doublesertotal <= 999.99)
                //{
                //    inttotalcount = inttotalcount + 4;
                //    int500above = 4;
                //}
                //else if (doublesertotal >= 250 && doublesertotal <= 499.99)
                //{
                //    inttotalcount = inttotalcount + 3;
                //    int250above = 3;
                //}
                //else if (doublesertotal >= 1 && doublesertotal <= 249.99)
                //{
                //    inttotalcount = inttotalcount + 2;
                //    int250below = 2;
                //}



                int intthirty = 0; int intseventto29 = 0; int intelevento16 = 0; int intsixtoten = 0; int intfive = 0;
                //if (doubleTotalAmount == 0 || doublesertotal == 0)
                //{
                //    if (doubleTotalAmount == 0)
                //    {
                //        inttotalcount = inttotalcount + 8;
                //        intthirty = 8;
                //    }
                //    else
                //        intfive = 0;
                //}
                //else
                {

                    if (totaldive >= 11 || doubleTotalAmount==0)
                    {
                        inttotalcount = inttotalcount + 10;
                        intthirty = 10;
                    }
                    //else if (totaldive >= 17 && totaldive <= 29.99)
                    //{
                    //    inttotalcount = inttotalcount + 7;
                    //    intseventto29 = 7;
                    //}
                    //else if (totaldive >= 11 && totaldive <= 16.99)
                    //{
                    //    inttotalcount = inttotalcount + 6;
                    //    intelevento16 = 6;
                    //}
                    //else if (totaldive >= 6 && totaldive <= 10.99)
                    //{
                    //    inttotalcount = inttotalcount + 2;
                    //    intsixtoten = 2;
                    //}
                    //else if (totaldive <= 5.99)
                    //{
                    //    if (doubleTotalAmount == 0 || doublesertotal == 0)
                    //    {
                    //        intfive = 0;

                    //    }
                    //    else
                    //    {
                    //        inttotalcount = inttotalcount + 1;
                    //        intfive = 1;
                    //    }
                    //}
                }

                X_Pos = 550;
                Y_Pos -= 87;//48;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase((intyoungercount+ inteldercount+ intdisablecount).ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intVeterancount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;//14
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intage6to8.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 18;//15
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);


                // Energy Burden

                X_Pos = 110; Y_Pos -= 70;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase(doublesertotal.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase(doubleTotalAmount.ToString(), TblFontBold), 320, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase(totaldive.ToString(), TblFontBold), 500, Y_Pos, 0);

                X_Pos = 550;
                Y_Pos -= 77; //43
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intseventto29.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intelevento16.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsixtoten.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);



                // Consumption Rate

                X_Pos = 550;
                Y_Pos -= 19;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(int1000plus.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int500above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250above.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(int250below.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 30;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase((int1000plus+ intthirty).ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Poverty 

                X_Pos = 550;
                Y_Pos -= 65;//42
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 18;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 15;
                ColumnText. ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inthundred.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 16;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 16;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(intonefiftyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 21;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase((intfity+intsenvtyfive+inthundred+inttwentytofifty+ intonefiftyfive).ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                CaseSnpEntity SnpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                string Fname = string.Empty; string dob = string.Empty;
                if (SnpEntity != null)
                {
                    Fname = SnpEntity.NameixFi; dob = SnpEntity.AltBdate.Trim();
                }


                //DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("ALS", "ALL", string.Empty, string.Empty, string.Empty, string.Empty, Fname, string.Empty,
                //              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dob, BaseForm.UserID, string.Empty, string.Empty);
                //int FirstTime = 0;
                //if(ds.Tables[0].Rows.Count>0)
                //{
                //    if(ds.Tables[0].Rows.Count==1) FirstTime = 3;

                //    //foreach(DataRow dr in ds.Tables[0].Rows)
                //    //{
                //    //    if(dr["Agency"].ToString()!=BaseForm.BaseAgency && dr["Dept"].ToString() != BaseForm.BaseDept && dr["Prog"].ToString() != BaseForm.BaseProg && dr["SnpYear"].ToString() != BaseForm.BaseYear)
                //    //    {
                //    //        FirstTime = 3;
                //    //        break;
                //    //    }
                //    //}
                //}


                //Y_Pos -= 35; //42
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);
                //Y_Pos -= 14;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("0", TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 47;//20 //36
                X_Pos = 550;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                // Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 42;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 17;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 45;//60

                if (inttotalcount >= 21)
                {
                    //_image_Tick.SetAbsolutePosition(125, Y_Pos - 15);
                    //cb.AddImage(_image_Tick);
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(1.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                }
                else if (inttotalcount >= 11 && inttotalcount <= 20) //inttotalcount >= 10
                {
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(2.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                    //_image_Tick.SetAbsolutePosition(320, Y_Pos - 15);
                    //cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 10)
                {
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(3.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                    //_image_Tick.SetAbsolutePosition(500, Y_Pos - 15);
                    //cb.AddImage(_image_Tick);
                }


                //if (inttotalcount >= 20)
                //{
                //    X_Pos = 40;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 15);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount >= 11 && inttotalcount <= 19)
                //{
                //    X_Pos = 40;

                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 15);
                //    cb.AddImage(_image_Tick);
                //}
                //else if (inttotalcount <= 10)
                //{
                //    X_Pos = 370;
                //    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                //    cb.AddImage(_image_Tick);
                //}

                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {
                    BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
                }



            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }

        #endregion


        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void cmbBudget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBudget.Items.Count > 0)
            {
                string strcmbBudget = ((Utilities.ListItem)cmbBudget.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();
                string strcmbFundsource = ((Utilities.ListItem)cmbFundsource.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString();

                

                if (!string.IsNullOrEmpty(strcmbBudget))
                {
                    if (((((Utilities.ListItem)cmbFundsource.SelectedItem).ID.ToString() == "N")))
                        AlertBox.Show("Postings are not allowed to this Budget " + (((Utilities.ListItem)cmbFundsource.SelectedItem).Text.ToString()));

                    if (((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString() != "0")
                    {
                        if (Emsbdc_List.Count > 0)
                        {
                            CMBDCEntity Entity = Emsbdc_List.Find(u => u.BDC_FUND == strcmbFundsource && u.BDC_ID.Trim() == strcmbBudget.Trim()); //&& u.BDC_ALLOW_POSTING=="Y");
                            if (Entity != null)
                            {
                                txtstart1.Text = LookupDataAccess.Getdate(Entity.BDC_START);
                                txtEnd.Text = LookupDataAccess.Getdate(Entity.BDC_END);
                                txtBudget.Text = Entity.BDC_BUDGET;
                                txtBalance1.Text = Entity.BDC_BALANCE;
                            }
                        }
                    }
                }
            }
        }

        private void PbVendor_Click(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbPrimSource.SelectedItem).Value.ToString() != "0")
            {
                VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, ((Utilities.ListItem)cmbPrimSource.SelectedItem).Value.ToString(), string.Empty, null);
                Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Vendor_Browse_Closed);
                Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
                Vendor_Browse.ShowDialog();
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please Select Primary Source");
            }

            ////if (string.IsNullOrEmpty(CategoryCode.Trim()))
            ////{
            //    VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, "**");
            //    Vendor_Browse.FormClosed += new Form.FormClosedEventHandler(On_Vendor_Browse_Closed);
            //    Vendor_Browse.ShowDialog();
            ////}
            ////else
            ////{
            ////    DataTable dt = new DataTable(); string LLR = "N";
            ////    if (dtSource.Rows.Count > 0)
            ////    {
            ////        DataView dv = new DataView(dtSource);
            ////        dv.RowFilter = "AGY_2= '" + CASource + "'";
            ////        dt = dv.ToTable();

            ////        if (dt.Rows.Count > 0) LLR = "Y";

            ////    }

            ////    string LLRName = string.Empty;
            ////    if (LLR == "Y")
            ////    {
            ////        if (caseDiffDetails != null)
            ////        {
            ////            LLRName = caseDiffDetails.IncareFirst.Trim() + " " + caseDiffDetails.IncareLast.Trim();
            ////        }
            ////    }

            ////    //Test1 Vendor_Browse = new Test1(BaseForm, Privileges, CASource, LLRName);
            ////    //Vendor_Browse.FormClosed += new Form.FormClosedEventHandler(On_Vendor_Browse_Closed1);
            ////    //Vendor_Browse.ShowDialog();

            ////    VendorBrowser_From Vendor_Browse = new VendorBrowser_From(BaseForm, Privileges, CASource, LLRName);
            ////    Vendor_Browse.FormClosed += new Form.FormClosedEventHandler(On_Vendor_Browse_Closed1);
            ////    Vendor_Browse.ShowDialog();
            ////}
        }

        private void On_Vendor_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();

                Txt_VendNo.Text = Vendor_Details[0].Trim();
                Text_VendName.Text = Vendor_Details[1].Trim();

            }
        }

        private void cmbBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbBilling.SelectedItem).Value.ToString() != "0")
            {
                if (((Utilities.ListItem)cmbBilling.SelectedItem).Value.ToString() == "T")
                {

                    if (Mode == "Add")
                    {
                        txtFirst.Text = string.Empty;
                        txtLast.Text = string.Empty;

                        txtFirst.Enabled = true;
                        txtLast.Enabled = true;
                    }
                    if (Mode == "Edit")
                    {
                        txtFirst.Enabled = true;
                        txtLast.Enabled = true;
                    }
                }
                else
                {
                    CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == ((Utilities.ListItem)cmbBilling.SelectedItem).ID.ToString());
                    if (casesnp != null)
                    {
                        txtFirst.Enabled = false;
                        txtLast.Enabled = false;
                        txtFirst.Text = casesnp.NameixFi;
                        txtLast.Text = casesnp.NameixLast;
                    }
                }
            }
        }

        private void SavePrintRecord()
        {
            LETRHISTCEntity Entity = new LETRHISTCEntity();
            string Msg = string.Empty;
            Entity.AGENCY = BaseForm.BaseAgency;
            Entity.DEPT = BaseForm.BaseDept;
            Entity.PROGRAM = BaseForm.BaseProg;
            Entity.YEAR = BaseForm.BaseYear;
            Entity.APPNO = BaseForm.BaseApplicationNo;
            Entity.LETR_CODE = "8";
            Entity.DATE = DateTime.Now.ToShortDateString();
            Entity.SEQ = "1";

            HierarchyEntity CaseWorker = new HierarchyEntity();
            if (hierarchyEntity.Count > 0)
            {
                CaseWorker = hierarchyEntity.Find(u => u.UserID == BaseForm.UserID);
            }

            if (CaseWorker != null)
                Entity.WORKER = CaseWorker.CaseWorker.Trim();
            Entity.ADD_OPERATOR = BaseForm.UserID;

            _model.SPAdminData.InsertLETRHIST(Entity, out Msg);
        }

        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        List<CaseVDD1Entity> CaseVdd1list = new List<CaseVDD1Entity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);

            CaseVDD1Entity Search_Entity1 = new CaseVDD1Entity(true);
            CaseVdd1list = _model.SPAdminData.Browse_CASEVDD1(Search_Entity1, "Browse");
        }

        private void PbGasVendor_Click(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbSecSource.SelectedItem).Value.ToString() != "0")
            {
                VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, ((Utilities.ListItem)cmbSecSource.SelectedItem).Value.ToString(), string.Empty, null);
                Vendor_Browse.FormClosed += new FormClosedEventHandler(On_GasVendor_Browse_Closed);
                Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
                Vendor_Browse.ShowDialog();
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please Select Primary Source");
            }

            //VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, "**");
            //Vendor_Browse.FormClosed += new Form.FormClosedEventHandler(On_GasVendor_Browse_Closed);
            //Vendor_Browse.ShowDialog();
        }

        private void On_GasVendor_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();

                Txt_GasVendNo.Text = Vendor_Details[0].Trim();
                Text_GasVendName.Text = Vendor_Details[1].Trim();

            }
        }

        private void cmbGasBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbGasBilling.SelectedItem).Value.ToString() != "0")
            {
                if (((Utilities.ListItem)cmbGasBilling.SelectedItem).Value.ToString() == "T")
                {

                    if (Mode == "Add")
                    {
                        txtGasFirst.Text = string.Empty;
                        txtGasLast.Text = string.Empty;

                        txtGasFirst.Enabled = true;
                        txtGasLast.Enabled = true;
                    }
                    if (Mode == "Edit")
                    {
                        txtGasFirst.Enabled = true;
                        txtGasLast.Enabled = true;
                    }
                }
                else
                {
                    CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == ((Utilities.ListItem)cmbGasBilling.SelectedItem).ID.ToString());
                    if (casesnp != null)
                    {
                        txtGasFirst.Enabled = false;
                        txtGasLast.Enabled = false;
                        txtGasFirst.Text = casesnp.NameixFi;
                        txtGasLast.Text = casesnp.NameixLast;
                    }
                }
            }
        }

        private string Get_Vendor_Name(string VendorNo)
        {
            string Vend_Name = string.Empty;
            foreach (CASEVDDEntity Entity in CaseVddlist)
            {
                if (Entity.Code == VendorNo)
                {
                    Vend_Name = Entity.Name.Trim(); break;
                }
            }

            return Vend_Name;
        }

        private void GetMaxBenfit()
        {
            if (CEAPCNTL_List.Count > 0)
            {
                txtAmount.Enabled = false;
                //string Poverty = BaseForm.BaseCaseMstListEntity[0].Poverty.Trim();
                if (string.IsNullOrEmpty(Poverty.ToString().Trim())) Poverty = "0";

                if (decimal.Parse(Poverty.Trim()) >= 0 && decimal.Parse(Poverty.Trim()) <51)
                {
                    if (Vulner_Flag) txtAmount.Text = CEAPCNTL_List[0].CPCT_VUL_50;
                    else txtAmount.Text = CEAPCNTL_List[0].CPCT_NONVUL_50;
                }
                else if (decimal.Parse(Poverty.Trim()) > 50 && decimal.Parse(Poverty.Trim()) <76)
                {
                    if (Vulner_Flag) txtAmount.Text = CEAPCNTL_List[0].CPCT_VUL_75;
                    else txtAmount.Text = CEAPCNTL_List[0].CPCT_NONVUL_75;
                }
                else if (decimal.Parse(Poverty.Trim()) > 75 && decimal.Parse(Poverty.Trim()) <151)
                {
                    if (Vulner_Flag) txtAmount.Text = CEAPCNTL_List[0].CPCT_VUL_150;
                    else txtAmount.Text = CEAPCNTL_List[0].CPCT_NONVUL_150;

                }
                else if (((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "S")
                {
                    if (Vulner_Flag) txtAmount.Text = CEAPCNTL_List[0].CPCT_VUL_SSI;
                    else txtAmount.Text = CEAPCNTL_List[0].CPCT_NONVUL_SSI;
                }
                else if (((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "M")
                {
                    if (Vulner_Flag) txtAmount.Text = CEAPCNTL_List[0].CPCT_VUL_MTVC;
                    else txtAmount.Text = CEAPCNTL_List[0].CPCT_NONVUL_MTVC;
                }
                else if (((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "N")
                {
                    if (Vulner_Flag) txtAmount.Text = CEAPCNTL_List[0].CPCT_VUL_SNAP;
                    else txtAmount.Text = CEAPCNTL_List[0].CPCT_NONVUL_SNAP;
                }
                else if (decimal.Parse(Poverty.Trim()) > 150)
                {
                    if (Mode == "Add")
                        CommonFunctions.SetComboBoxValue(cmbEligStatus, "P");
                }

            }

            //CASESP0Entity SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString(), null, null, null, null, null, null, null, null);
            //if(SP_Header_Rec!=null)
            //{
            //    if (!string.IsNullOrEmpty(SP_Header_Rec.Maxbenefit.Trim()))
            //    {
            //        txtAmount.Text = SP_Header_Rec.Maxbenefit.ToString();
            //        txtAmount.Enabled = false;
            //    }
            //    else
            //    {
            //        txtAmount.ReadOnly = false;
            //        //if(Mode=="Edit")
            //        //{
            //        //    if (((Captain.Common.Utilities.ListItem)cmbEligStatus.SelectedItem).Value.ToString() == "E")
            //        //        txtAmount.ReadOnly = false;
            //        //    else
            //        //        txtAmount.ReadOnly = true;
            //        //}
            //    }

            //}



        }

        bool Vulner_Flag = false;
        bool Age_Grt_60 = false, Age_Les_6 = false, Disable_Flag = false, FoodStamps_Flag = false;

        private bool Get_SNP_Vulnerable_Status()
        {
            //bool Vulner_Flag = false;
            Vulner_Flag = false;
            DateTime MST_Intake_Date = DateTime.Today, SNP_DOB = DateTime.Today;
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan Time_Span;
            int Age_In_years = 0;

            if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()))
                MST_Intake_Date = Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].IntakeDate);
            string Non_Qual_Alien_SW = "N";
            foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
            {
                SNP_DOB = MST_Intake_Date;
                if (!string.IsNullOrEmpty(Entity.AltBdate.Trim()))
                    SNP_DOB = Convert.ToDateTime(Entity.AltBdate);

                Age_In_years = 0;

                if (MST_Intake_Date > SNP_DOB)
                {
                    Time_Span = (MST_Intake_Date - SNP_DOB);
                    Age_In_years = (zeroTime + Time_Span).Year - 1;
                }
                //Age_In_years = (Time_Span). - 1;

                if (Age_In_years > 59)
                    Age_Grt_60 = true;

                if (Age_In_years < 6)
                    Age_Les_6 = true;

                if (Entity.Disable == "Y")
                    Disable_Flag = true;

                if (Entity.FootStamps == "Y")
                    FoodStamps_Flag = true;

                if (Entity.SsnReason == "Q" && BaseForm.BaseAgencyControlDetails.State=="TX") Non_Qual_Alien_SW = "Y";

                if ((Age_Grt_60 || Age_Les_6 || Disable_Flag) && Non_Qual_Alien_SW != "Y")
                    break;
            }

            string Tmp_Age_Dis = string.Empty;
            //if (Sel_Activity == "B")
            //{
            //    if (((ListItem)Cmb_Age_Dis.SelectedItem).Value.ToString() != null)
            //        Tmp_Age_Dis = ((ListItem)Cmb_Age_Dis.SelectedItem).Value.ToString();
            //}
            //else
            //    Tmp_Age_Dis = PassLIHEAPB_List[0].Age_dis;


            if ((Age_Grt_60 || Age_Les_6 || Disable_Flag) && Non_Qual_Alien_SW=="N")
            {
                //if (Tmp_Age_Dis == "1" || Tmp_Age_Dis == "2" || Tmp_Age_Dis == "3")
                Vulner_Flag = true;

                if (Age_Les_6)
                    Vulner_Flag = true;
            }



            return Vulner_Flag;
        }



        private void upload2_Uploaded(object sender, UploadedEventArgs e)
        {
            DirectoryInfo MyDir = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            //FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            //foreach (FileInfo MyFile in MyFiles)
            //{
            //    if (MyFile.Exists)
            //    {
            //        MyFile.Delete();
            //    }
            //}

            var mobjResult = e.Files[0];
            string strName = mobjResult.FileName;
            string strType = mobjResult.ContentType;
            long strr = mobjResult.ContentLength;
            lblSInvFileName.Text = mobjResult.FileName;
            string strModifyfilename = mobjResult.FileName;
            strModifyfilename = strModifyfilename.Replace(',', '_').Replace('&', '_').Replace('$', '_').Replace('#', '_').Replace('/', '_').Replace("'", "_").Replace('{', '_').Replace('}', '_').Replace('@', '_').Replace('%', '_').Replace('/', '_').Replace('?', '_');
            lblSInvFileName.Text = strModifyfilename;
            //File.Move(mobjResult.FileName, strTempFolderName + "\\" + mobjResult.FileName); //Alaa: This one here always throws an exception, delete this line if it's the right way
            mobjResult.SaveAs(strTempFolderName + "\\" + strName);

            pb_Delete_SInv.Visible = true;
            upload2.Visible = false;


            if (Mode == "Edit")
            {
                if (!string.IsNullOrEmpty(lblSInvFileName.Text.Trim()))
                {
                    UploadLogPdf("ADD", string.Empty, "S");
                    lblSInvFileName.Text = string.Empty;
                    upload2.Visible = true; pb_Delete_SInv.Visible = false;
                }
                Get_App_CASEACT_List(SPM_Entity);
            }

        }

        private void lblPInvFileName_Click(object sender, EventArgs e)
        {
            if (lblPInvFileName.Text != string.Empty)
            {
                string strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                //string strFolderPath = Consts.Common.ServerLocation + BaseForm.BaseAgencyControlDetails.Path + "\\TEMP\\Invoices\\" + strFolderName;

                PdfViewerNewForm objfrm = new PdfViewerNewForm(strTempFolderName + "\\" + lblPInvFileName.Text.ToString());
                objfrm.ShowDialog();
            }
        }

        private void lblSInvFileName_Click(object sender, EventArgs e)
        {
            if (lblSInvFileName.Text != string.Empty)
            {
                string strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                //string strFolderPath = Consts.Common.ServerLocation + BaseForm.BaseAgencyControlDetails.Path + "\\TEMP\\Invoices\\" + strFolderName;

                PdfViewerNewForm objfrm = new PdfViewerNewForm(strTempFolderName + "\\" + lblSInvFileName.Text.ToString());
                objfrm.ShowDialog();
            }
        }

        private void Pb_Delete_Pinv_Click(object sender, EventArgs e)
        {
            if(lblPInvFileName.Text!=string.Empty)
            {
                MessageBox.Show("Are you Sure You want to delete uploaded invoice file?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerPDelete);
            }
        }

        private void MessageBoxHandlerPDelete(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                string strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                //string strFolderPath = Consts.Common.ServerLocation + BaseForm.BaseAgencyControlDetails.Path + "\\TEMP\\Invoices\\" + strFolderName;
                if (File.Exists(strTempFolderName + "\\" + lblPInvFileName.Text.ToString()))
                {
                    File.Delete(strTempFolderName + "\\" + lblPInvFileName.Text.ToString());
                }

                lblPInvFileName.Text = string.Empty;
                upload1.Visible = true;Pb_Delete_Pinv.Visible = false;
            }
        }

        private void pb_Delete_SInv_Click(object sender, EventArgs e)
        {
            if (lblSInvFileName.Text != string.Empty)
            {
                MessageBox.Show("Are you Sure You want to delete uploaded invoice file?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerSDelete);
            }
        }

        private void MessageBoxHandlerSDelete(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                string strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                //string strFolderPath = Consts.Common.ServerLocation + BaseForm.BaseAgencyControlDetails.Path + "\\TEMP\\Invoices\\" + strFolderName;
                if (File.Exists(strTempFolderName + "\\" + lblSInvFileName.Text.ToString()))
                {
                    File.Delete(strTempFolderName + "\\" + lblSInvFileName.Text.ToString());
                }

                lblSInvFileName.Text = string.Empty;
                upload2.Visible = true; pb_Delete_SInv.Visible = false;
            }
        }

        private void btnPInvDocs_Click(object sender, EventArgs e)
        {
            //Invoice_Documents invDoc = new Invoice_Documents(BaseForm, Privileges, SPM_Entity, "P","");
            //invDoc.StartPosition = FormStartPosition.CenterScreen;
            //invDoc.FormClosed += new FormClosedEventHandler(SP_AddForm_Closed);
            //invDoc.ShowDialog();
        }

        private void SP_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            if (BaseForm.BaseAgencyControlDetails.CEAPPostUsage == "Y")
            {
                //List<CASEACTEntity> caseactlist = SP_Activity_Details.FindAll(u => u.Elec_Other.ToString().Trim() == "O");
                //if (caseactlist.Count > 0) { upload2.Visible = true; }
                //caseactlist = SP_Activity_Details.FindAll(u => u.Elec_Other.ToString().Trim() == "E");
                //if (caseactlist.Count > 0) { upload1.Visible = true; }

                if (SPM_Entity.SPM_EligStatus == "E" || SPM_Entity.SPM_EligStatus == "S" || SPM_Entity.SPM_EligStatus == "M" || SPM_Entity.SPM_EligStatus == "N") { upload2.Visible = true; upload1.Visible = true; }

                List<INVDOCLOGEntity> InvDoclogEntitylist = _model.ChldMstData.GetInvDocsLogList(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, SPM_Entity.service_plan, SPM_Entity.Seq, string.Empty, string.Empty);
                InvDoclogEntitylist = InvDoclogEntitylist.FindAll(u => u.INVDOC_DELETED_BY == string.Empty && u.INVDOC_DATE_DELETED == string.Empty && u.INVDOC_SERVICE == string.Empty);
                if (InvDoclogEntitylist.Count > 0)
                {
                    List<INVDOCLOGEntity> PINVDocs = InvDoclogEntitylist.FindAll(u => u.INVDOC_VEND_TYPE == "P");
                    if (PINVDocs.Count > 0) btnPInvDocs.Visible = true; else btnPInvDocs.Visible = false;
                    List<INVDOCLOGEntity> SINVDocs = InvDoclogEntitylist.FindAll(u => u.INVDOC_VEND_TYPE == "S");
                    if (SINVDocs.Count > 0) btnSInvDocs.Visible = true; else btnSInvDocs.Visible = false;
                }
                else
                {
                    btnPInvDocs.Visible = false;
                    btnSInvDocs.Visible = false;
                }
            }
        }
        private void btnSInvDocs_Click(object sender, EventArgs e)
        {
            Invoice_Documents invDoc = new Invoice_Documents(BaseForm, Privileges, SPM_Entity, "S","");
            invDoc.StartPosition = FormStartPosition.CenterScreen;
            invDoc.FormClosed += new FormClosedEventHandler(SP_AddForm_Closed);
            invDoc.ShowDialog();
        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            if(SPM_Entity!=null)
            {
                Search_Entity = SPM_Entity;
                Search_Entity.Rec_Type = "C";
                if (_model.SPAdminData.UpdateCASESPM(Search_Entity, "Insert", out Sql_SP_Result_Message, out Tmp_SPM_Sequence))
                {
                    txtBalance.Text = Sql_SP_Result_Message;
                }
                else
                    txtBalance.Text = Sql_SP_Result_Message;
            }
        }

        private void upload1_Uploaded(object sender, UploadedEventArgs e)
        {
            DirectoryInfo MyDir = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    MyFile.Delete();
                }
            }

            var mobjResult = e.Files[0];
            string strName = mobjResult.FileName;
            string strType = mobjResult.ContentType;
            long strr = mobjResult.ContentLength;
            lblPInvFileName.Text = mobjResult.FileName;
            string strModifyfilename = mobjResult.FileName;
            strModifyfilename = strModifyfilename.Replace(',', '_').Replace('&', '_').Replace('$', '_').Replace('#', '_').Replace('/', '_').Replace("'", "_").Replace('{', '_').Replace('}', '_').Replace('@', '_').Replace('%', '_').Replace('/', '_').Replace('?', '_');
            lblPInvFileName.Text = strModifyfilename;
            //File.Move(mobjResult.FileName, strTempFolderName + "\\" + mobjResult.FileName); //Alaa: This one here always throws an exception, delete this line if it's the right way
            mobjResult.SaveAs(strTempFolderName + "\\" + strName);

            Pb_Delete_Pinv.Visible = true;
            upload1.Visible= false;

            if (Mode == "Edit")
            {
                if (!string.IsNullOrEmpty(lblPInvFileName.Text.Trim()))
                {
                    UploadLogPdf("ADD", string.Empty, "P");
                    lblPInvFileName.Text = string.Empty;
                    upload1.Visible = true; Pb_Delete_Pinv.Visible = false;
                }
                Get_App_CASEACT_List(SPM_Entity);
            }

        }

        private void UploadLogPdf(string strMode, string strLogId,string strType)
        {

            string stroutLogid = strLogId;
            bool boolImageUpload = false;
            //string strFamilySeq = string.Empty;
            string strFileName = string.Empty;
            if (strType == "P") strFileName = lblPInvFileName.Text;else if(strType=="S") strFileName= lblSInvFileName.Text;

            //var Extension = lblPInvFileName.Text.Substring(lblPInvFileName.Text.LastIndexOf('.') + 1).ToLower();
            var Extension = strFileName.Substring(strFileName.LastIndexOf('.') + 1).ToLower();
            strExtensionName = Extension;

            string strUploadFolderName = string.Empty;//Consts.Common.ServerLocation + BaseForm.BaseAgencyControlDetails.Path + @"\\Invoices\" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;

            string strOnlyFileName = string.Empty;

            strUploadFolderName = strImageFolderName;// + "\\" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
            strOnlyFileName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;

            if (Extension == "pdf")
            {
                boolImageUpload = true; // Valid file type
            }
            if (boolImageUpload)
            {

                try
                {
                    // Determine whether the directory exists.

                    if (Directory.Exists(strUploadFolderName))
                    {
                        //stroutLogid = createLogos(strUploadFolderName, strMode, strLogId);
                        createLogos(strUploadFolderName, strMode, strLogId, strType);

                    }
                    else
                    {
                        // Try to create the directory.
                        DirectoryInfo di = Directory.CreateDirectory(strUploadFolderName);
                        //stroutLogid = createLogos(strUploadFolderName, strMode, strLogId);
                        createLogos(strUploadFolderName, strMode, strLogId, strType);

                    }

                }
                catch (Exception ex)
                {
                    // Console.WriteLine("The process failed: {0}", ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("You should upload the invoice only in PDF format");
            }


           // return stroutLogid;
        }

        private void createLogos(string strFPath, string strMode, string strLogId,string strType)
        {
            string strInvoiceId = string.Empty;
            DirectoryInfo dir1 = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            DirectoryInfo dir2 = new DirectoryInfo(strFPath);

            string FileName = string.Empty;
            if (strType == "P") FileName = lblPInvFileName.Text; else if (strType == "S") FileName = lblSInvFileName.Text;


            FileInfo[] Folder1Files = dir1.GetFiles();
            FileInfo[] Folder2Files = dir2.GetFiles();
            string[] strorgsplitFile = FileName.Split('.');//lblPInvFileName.Text.Split('.');
            string orginialFileName = string.Empty;
            if (strorgsplitFile.Length > 0)
                orginialFileName = strorgsplitFile[0];

            if (FileName != string.Empty)
            {
                strorgsplitFile = FileName.Split('.');
                if (strorgsplitFile.Length > 0)
                    orginialFileName = strorgsplitFile[0];
            }

            
                //if (lblPInvFileName.Text != string.Empty)
                //{
                //    strorgsplitFile = lblPInvFileName.Text.Split('.');
                //    if (strorgsplitFile.Length > 0)
                //        orginialFileName = strorgsplitFile[0];
                //}
           

            if (Folder1Files.Length > 0)
            {
                foreach (FileInfo aFile in Folder1Files)
                {
                    string newFileName = orginialFileName + "." + strExtensionName;
                    string strpathToCheck = strFPath + "\\" + newFileName;

                    string tempfileName = "";
                    // Check to see if a file already exists with the
                    // same name as the file to upload.        
                    if (System.IO.File.Exists(strpathToCheck))
                    {
                        int counter = 1;
                        while (System.IO.File.Exists(strpathToCheck))
                        {
                            // if a file with this name already exists,
                            // prefix the filename with a number.
                            tempfileName = orginialFileName + counter.ToString() + "." + strExtensionName;
                            strpathToCheck = strFPath + "\\" + tempfileName;
                            counter++;
                        }

                        newFileName = tempfileName;
                    }

                    if (aFile.Name == FileName)
                    {
                        File.Move(strTempFolderName + "\\" + aFile.Name, strFPath + "\\" + newFileName);
                        //strInvoiceId = 
                        InsertDeleteInvDocLog(strLogId, strMode, FileName, newFileName,strType);

                    }

                }
            }

            else
            {
                MessageBox.Show("Upload new image");

            }
            //return strInvoiceId;

        }

        private void InsertDeleteInvDocLog(string strIMGId, string strOpertype, string strimgFileName, string strimgloadas,string strType)
        {
            string strInvoiceId = strIMGId;
            INVDOCLOGEntity imglogentity = new INVDOCLOGEntity();
            imglogentity.INVDOC_ID = strIMGId;
            imglogentity.INVDOC_AGENCY = BaseForm.BaseAgency;
            imglogentity.INVDOC_DEPT = BaseForm.BaseDept;
            imglogentity.INVDOC_PROGRAM = BaseForm.BaseProg;
            imglogentity.INVDOC_YEAR = BaseForm.BaseYear;
            imglogentity.INVDOC_APP = BaseForm.BaseApplicationNo;
            //CaseSnpEntity casesnpimag = dataGridHierchys.SelectedRows[0].Tag as CaseSnpEntity;
            //if (chkHouseHold.Checked == true)
            //{
            //    imglogentity.IMGLOG_FAMILY_SEQ = casesnpimag.FamilySeq;
            //}
            //else
            imglogentity.INVDOC_SERVICEPLAN = SPM_Entity.service_plan;
            imglogentity.INVDOC_SPM_SEQ = SPM_Entity.Seq;

            
                imglogentity.INVDOC_VEND_TYPE = strType;
                imglogentity.INVDOC_SEQ = "1";

            if (strType == "P") imglogentity.INVDOC_VENDOR = SPM_Entity.SPM_Vendor;
            else if (strType == "S") imglogentity.INVDOC_VENDOR = SPM_Entity.SPM_Gas_Vendor;

            imglogentity.INVDOC_UPLOAD_AS = strimgloadas;
            imglogentity.INVDOC_UPLOAD_BY = BaseForm.UserID;
            imglogentity.INVDOC_ORIG_NAME = strimgFileName;
            imglogentity.MODE = strOpertype;

            _model.ChldMstData.InsertINVDOCLOG(imglogentity);

            if(Mode=="Edit")
            {
                if (!string.IsNullOrEmpty(lblPInvFileName.Text.Trim()))
                    AlertBox.Show(lblPInvFileName.Text.Trim() + " Uploaded Successfully");

                if (!string.IsNullOrEmpty(lblSInvFileName.Text.Trim()))
                    AlertBox.Show(lblSInvFileName.Text.Trim() + " Uploaded Successfully");

                lblPInvFileName.Text = string.Empty;
                upload1.Visible = true; Pb_Delete_Pinv.Visible = false;

                lblSInvFileName.Text = string.Empty;
                upload2.Visible = true; pb_Delete_SInv.Visible = false;

                if (SPM_Entity.SPM_Gas_Vendor != string.Empty) upload2.Visible = true; if (SPM_Entity.SPM_Vendor != string.Empty) upload1.Visible = true;

               
            }

            //return strInvoiceId;
        }



    }
}