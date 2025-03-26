/************************************************************************
 * Conversion On    :   01/05/2023      * Converted By     :   Kranthi
 * Modified On      :   01/05/2023      * Modified By      :   Kranthi
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
using System.Globalization;
using Captain.Common.Views.UserControls;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using static Amazon.S3.Util.S3EventNotification;
//using Spire.Doc.Documents;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0021Form : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public CASE0021Form(BaseForm baseForm, PrivilegeEntity privilegeEntity, string mode, DataRow dr, ProgramDefinitionEntity programdefentity)
        {

            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            propProgramdefinationdata = programdefentity;
            Mode = mode;
            propdr = dr;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtAmountPaid.Validator = TextBoxValidation.FloatValidator;
            txtSerAmt.Validator = TextBoxValidation.FloatValidator;
            txtErapAmtpaid.Validator = TextBoxValidation.FloatValidator;
            txtErapArrea.Validator = TextBoxValidation.FloatValidator;
            //this.Text = privilegeEntity.PrivilegeName.Trim() + " - " + Mode;
            this.Text = dr["CA_DESC"].ToString().Trim() + " - " + Mode;
            txtSerAmt.Enabled = false;
            propCEAPCNTLSwitch = "N";
            CMbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);

            CASESPMEntity Search_Entity = new CASESPMEntity(true);

            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;

            //Search_Entity.year = BaseForm.BaseYear;        
            Search_Entity.year = null;                // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = BaseForm.BaseApplicationNo;
            Search_Entity.service_plan = dr["CASEACT_SERVICE_PLAN"].ToString().Trim();
            Search_Entity.Seq = dr["CASEACT_SPM_SEQ"].ToString().ToString().Trim();

            //Search_Entity.service_plan = Search_Entity.caseworker = Search_Entity.site = null;
            //Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            //Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            //Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            //Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = null;
            List<CASESPMEntity> CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");


            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, BaseForm.BaseYear, string.Empty, string.Empty);

            string HieAgency = BaseForm.BaseAgency;
            string HieDept = BaseForm.BaseDept;
            string HieProg = BaseForm.BaseProg;

            if (CASESPM_List.Count>0)
            {
                if (!string.IsNullOrEmpty(CASESPM_List[0].Def_Program.Trim()))
                {
                    HieAgency = CASESPM_List[0].Def_Program.Substring(0, 2);
                    HieDept = CASESPM_List[0].Def_Program.Substring(2, 2);
                    HieProg = CASESPM_List[0].Def_Program.Substring(4, 2);

                    //programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(HieAgency, HieDept, HieProg);
                }
            }

            if (propProgramdefinationdata != null)
            {

                string CategoryCode = string.Empty;



                CategoryCode = propProgramdefinationdata.DepSerpostPAYCAT.Trim();
                propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", CategoryCode, HieAgency + HieDept + HieProg, dr["CASEACT_SERVICE_PLAN"].ToString().Trim(), propdr["CASEACT_BRANCH"].ToString(), propdr["CASEACT_GROUP"].ToString(), dr["CASEACT_ACTIVITY_CODE"].ToString().ToString().Trim(), "SP");
                propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == CategoryCode);

                if (propPMTFLDCNTLHEntity.Count == 0)
                {
                    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", CategoryCode, HieAgency + HieDept + HieProg, "0", " ", "0", "          ", "hie");
                    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == CategoryCode);
                }

                if (programdefentity.DepSerpostPAYCAT == "04")
                {
                    if (CEAPCNTL_List.Count > 0)
                    {
                        if (CEAPCNTL_List.FindAll(u => u.CPCT_VUL_SP.ToString().Trim() == dr["CASEACT_SERVICE_PLAN"].ToString().Trim() && u.CPCT_VUL_PRIM_CA.ToString().Trim() == dr["CASEACT_ACTIVITY_CODE"].ToString().ToString().Trim()).Count > 0)
                        {
                            txtSerAmt.Enabled = true;
                            propCEAPCNTLSwitch = "Y";
                        }

                        if (CEAPCNTL_List.FindAll(u => u.CPCT_NONVUL_SP.ToString().Trim() == dr["CASEACT_SERVICE_PLAN"].ToString().Trim() && u.CPCT_NONVUL_PRIM_CA.ToString().Trim() == dr["CASEACT_ACTIVITY_CODE"].ToString().ToString().Trim()).Count > 0)
                        {
                            txtSerAmt.Enabled = true;
                            propCEAPCNTLSwitch = "Y";
                        }
                    }
                }

                //this.Size = new Size(841, 498);
                pnlERAP.Visible = false;
                
                fillComboboxes();
                Fill_Sources();
                Fill_Sites();
                SP_Header_Rec = dr["SP0_FUNDS"].ToString();
                string ACR_SERV_Hies = string.Empty;
                if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.ToString().Trim()))
                    ACR_SERV_Hies = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.ToString();
                SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, dr["CASEACT_SERVICE_PLAN"].ToString(), ACR_SERV_Hies);
                Fill_Funding();

                EnableDisableControls();
                if (programdefentity.DepSerpostPAYCAT == "02")
                {
                    dtActSeek_Date.Visible = false;
                    lblActSeekDate.Visible = false;
                    lblActSeekDateReq.Visible = false;
                    pnlCEAP.Visible = false;
                    pnlERAP.Location = new Point(-1, 97);

                    pnlERAP.Visible = true;
                    this.Size = new Size(this.Width, this.Height - pnlCEAP.Height);


                    //grpBudget.Visible = false;
                    //grpService.Visible = false;
                    //pnlServicesMaster.Location = new Point(-2, 300);

                    //pnlBundledetails.Visible = false;
                    pnlBudget.Visible = false;
                    //pnlsave.Location = new Point(-3, 353);

                    lblResouceAmount.Visible = false;
                    lblResourceReq.Visible = false;
                    txtSerAmt.Visible = false;
                    lblServiceBal.Visible = false;
                    lblServicePaid.Visible = false;
                    lblPaid.Visible = false;
                    lblBal.Visible = false;

                }
                else {
                    this.Size = new Size(this.Width, this.Height - pnlERAP.Height);
                }

                FillCase0021Form(dr);


                if (Mode == "View")
                {
                    btnSave.Visible = false;
                    pnlAdjustment.Enabled = false;
                    pnlCA.Enabled = false;
                    pnlCEAP.Enabled = false;
                    pnlERAP.Enabled = false;
                    pnlsave.Enabled = false;

                }
            }
        }

        #region Properties
        public BaseForm BaseForm { get; set; }

        public ProgramDefinitionEntity propProgramdefinationdata { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        DataRow propdr { get; set; }

        public string propCEAPCNTLSwitch { get; set; }


        #endregion

        List<CMBDCEntity> CMbdc_List = new List<CMBDCEntity>();
        List<CEAPCNTLEntity> CEAPCNTL_List = new List<CEAPCNTLEntity>();
        string strCaseWorkerDefaultCode = "0";
        string strCwdefaultStartCode = "0";
        private void fillComboboxes()
        {
            cmbReason.Items.Clear();

            List<CommonEntity> commonReasonlist = CommonFunctions.AgyTabsFilterOrderbyCode(BaseForm.BaseAgyTabsEntity, "S0133", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);

            cmbBenfitReason4.Items.Clear();
            cmbBenfitReason4.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "0"));
            cmbBenfitReason4.SelectedIndex = 0;
            foreach (CommonEntity reasonlist in commonReasonlist)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(reasonlist.Desc, reasonlist.Code);
                cmbBenfitReason4.Items.Add(li);
            }

            List<CommonEntity> Reasoncodes = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.Reasoncodes, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);

            cmbReason.Items.Insert(0, new ListItem("Select One", "0"));
            cmbReason.SelectedIndex = 0;
            foreach (CommonEntity reasoncode in Reasoncodes)
            {
                ListItem li = new ListItem(reasoncode.Desc, reasoncode.Code);
                cmbReason.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && Dwellingitems.Default.Equals("Y")) cmbReason.SelectedItem = li;
            }

            CmbWorker.Items.Clear();
            CmbWorker.ColorMember = "FavoriteColor";
            cmbservicewkr.Items.Clear();
            cmbservicewkr.ColorMember = "FavoriteColor";

            List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(BaseForm.BaseHierarchyCwFormat.ToString(), BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
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
                    CmbWorker.Items.Add(new ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Black : Color.Red));
                    cmbservicewkr.Items.Add(new ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Black : Color.Red));
                }
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCwdefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                }

            }
            CmbWorker.Items.Insert(0, new ListItem(" ", "0"));
            CmbWorker.SelectedIndex = 0;
            cmbservicewkr.Items.Insert(0, new ListItem(" ", "0"));
            cmbservicewkr.SelectedIndex = 0;
            if (Mode.Equals(Consts.Common.Add))
            {
                CommonFunctions.SetComboBoxValue(CmbWorker, strCaseWorkerDefaultCode);

            }

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

            BillPeriodEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00202", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "Add");


        }

        private void fillBudgets(List<CMBDCEntity> BudgetList, string budgetID)
        {
            cmbBudget.SelectedIndexChanged -= new EventHandler(cmbBudget_SelectedIndexChanged);
            cmbBudget.Items.Clear();
            if (BudgetList.Count > 0)
            {
                

                /***************************** Kranthi 01/20/2023 :: Reloading the Budget combo causes to zero selected value so we are not getting description text :: As of now i am commenting this reloading ************************************************************/
                foreach (CMBDCEntity entity in BudgetList)
                {
                    cmbBudget.Items.Add(new Utilities.ListItem(entity.BDC_DESCRIPTION.ToString(), entity.BDC_ID.ToString(),entity.BDC_ALLOW_POSTING.Trim(),string.Empty));
                }

                if (BudgetList.Count > 1)
                {
                    cmbBudget.Items.Insert(0, new Utilities.ListItem("  ", "0"));
                }

                if (budgetID != "")
                    CommonFunctions.SetComboBoxValue(cmbBudget, budgetID);
                else
                    cmbBudget.SelectedIndex = 0;
                /*********************************************************************************************************************************/
                string strBudgetId = ((Utilities.ListItem)cmbBudget.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();

                CMBDCEntity cmbdcdata = CMbdc_List.Find(u => u.BDC_ID.ToString().Trim() == strBudgetId.ToString().Trim() && u.BDC_ALLOW_POSTING == "Y");
                if (cmbdcdata != null)
                {
                    lblBudgetdesc2.Text = cmbdcdata.BDC_DESCRIPTION;
                    decimal decbudget = cmbdcdata.BDC_BUDGET.ToString() == string.Empty ? 0 : Convert.ToDecimal(cmbdcdata.BDC_BUDGET);
                    lblBudget1.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decbudget);
                    lblStart1.Text = LookupDataAccess.Getdate(cmbdcdata.BDC_START.ToString());
                    lblEnd1.Text = LookupDataAccess.Getdate(cmbdcdata.BDC_END.ToString());
                    decimal decbudbalance = cmbdcdata.BDC_BALANCE.ToString() == string.Empty ? 0 : Convert.ToDecimal(cmbdcdata.BDC_BALANCE);
                    lblBudgetBalance.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decbudbalance); ;
                }
                else
                {
                    lblBudgetdesc2.Text = string.Empty;
                    lblBudget1.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", 0);
                    lblStart1.Text = string.Empty;
                    lblEnd1.Text = string.Empty;
                    lblBudgetBalance.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", 0); ;
                }

            }
            else
            {
                txtSerAmt.Enabled = false;
                lblBudgetdesc2.Text = string.Empty;
                lblBudget1.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", 0);
                lblStart1.Text = string.Empty;
                lblEnd1.Text = string.Empty;
                lblBudgetBalance.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", 0); 
            }
            cmbBudget.SelectedIndexChanged += new EventHandler(cmbBudget_SelectedIndexChanged);
        }



        private void Fill_Sites()
        {
            CmbSite.Items.Clear();


            CmbSite.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));


            DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable Sites_Table = ds.Tables[0];
                if (Sites_Table.Rows.Count > 0)
                {
                    if (Mode.Equals("Add"))
                    {
                        DataView dv = new DataView(Sites_Table);
                        dv.RowFilter = "SITE_ACTIVE='Y'";
                        Sites_Table = dv.ToTable();
                    }

                    foreach (DataRow dr in Sites_Table.Rows)
                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString().Trim(), dr["SITE_ACTIVE"].ToString().Trim(), (dr["SITE_ACTIVE"].ToString().Trim().Equals("Y") ? Color.Black : Color.Red)));

                }
                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity(); List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                    foreach (HierarchyEntity Entity in userHierarchy)
                    {
                        if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        { hierarchyEntity = null; }
                    }

                    if (hierarchyEntity != null)
                    {
                        List<Captain.Common.Utilities.ListItem> listItemSite = new List<Captain.Common.Utilities.ListItem>();
                        listItemSite.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));
                        if (hierarchyEntity.Sites.Length > 0)
                        {
                            string[] Sites = hierarchyEntity.Sites.Split(',');

                            for (int i = 0; i < Sites.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                {
                                    foreach (Captain.Common.Utilities.ListItem casesite in listItem) //Site_List)//ListcaseSiteEntity)
                                    {
                                        if (Sites[i].ToString() == casesite.Value.ToString())
                                        {
                                            listItemSite.Add(casesite);
                                            //break;
                                        }

                                    }
                                }
                            }

                            listItem = listItemSite;
                        }

                    }
                }
            }

            CmbSite.Items.AddRange(listItem.ToArray());
            CmbSite.SelectedIndex = 0;

        }

        public string SP_Header_Rec { get; set; }
        private void Fill_Funding()
        {
            cmbFUnd.Items.Clear(); cmbFUnd.ColorMember = "FavoriteColor";
            cmbErapFund.Items.Clear(); cmbErapFund.ColorMember = "FavoriteColor";


            if (!string.IsNullOrEmpty(SP_Header_Rec))
            {
                bool Fund_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0;
                List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
                FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
                string Funds_Str = SP_Header_Rec;
                int Tmp_Curr_Fund_Len = 0;

                foreach (SPCommonEntity Entity in FundingList)
                {

                    Fund_Exists = false; Pos = 0;
                    for (int i = 0; Pos < SP_Header_Rec.Length; i++)
                    {

                        Tmp_Curr_Fund_Len = (Funds_Str.Substring(Pos, Funds_Str.Substring(Pos, (Funds_Str.Length - Pos)).Length)).Length;

                        if (Entity.Code == SP_Header_Rec.Substring(Pos, (Tmp_Curr_Fund_Len >= 10 ? 10 : Tmp_Curr_Fund_Len)).Trim())
                        {
                            Fund_Exists = true; break;
                        }
                        Pos += 10;

                    }

                    if (Fund_Exists)
                    {
                        if (Mode == "View" || Mode == "Edit" || (Mode == "Add" && Entity.Active.Equals("Y")))
                        {
                            cmbFUnd.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                            cmbErapFund.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));

                            Tmp_Loop_Cnt++;
                        }
                    }
                }

            }
            cmbFUnd.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            cmbFUnd.SelectedIndexChanged -= cmbFUnd_SelectedIndexChanged;
            cmbFUnd.SelectedIndex = 0;
            cmbFUnd.SelectedIndexChanged += cmbFUnd_SelectedIndexChanged;
            cmbErapFund.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));

            cmbErapFund.SelectedIndex = 0;

        }


        private void CAPanel_Click(object sender, EventArgs e)
        {

        }

        private void label78_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

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



        private void btnBenefit_Click(object sender, EventArgs e)
        {

            MembersGridForm PostCA_Form;
            //PostCA_Form = new MembersGridForm(BaseForm, Hierarchy, Year, CAMS_Desc, Pass_CA_Entity, Privileges, CA_Template_List, ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString(), CAOBO_List);   // 08022012
            //PostCA_Form.FormClosed += new Form.FormClosedEventHandler(Add_Edit_MembersForm_Closed);

            //PostCA_Form.ShowDialog();
        }

        private void Add_Edit_MembersForm_Closed(object sender, FormClosedEventArgs e)
        {
            MembersGridForm form = sender as MembersGridForm;
            if (form.DialogResult == DialogResult.OK)
            {
                //CAOBO_List = new List<CAOBOEntity>();
                //CAOBO_List = form.GetMemberRecords();

            }
        }

        private void PbVendor_Click(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbCat4Source.SelectedItem).Value.ToString() != "0")
            {
                VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, ((Utilities.ListItem)cmbCat4Source.SelectedItem).Value.ToString(), string.Empty,null);
                Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Vendor_Browse_Closed);
                Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
                Vendor_Browse.ShowDialog();
            }
            else
            {
                VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, "**");
                Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Vendor_Browse_Closed);
                Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
                Vendor_Browse.ShowDialog();
            }
        }

      
        private void On_Vendor_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();

                txtVendorNo.Text = Vendor_Details[0].Trim();
                lblCEAPVendorname.Text = Vendor_Details[1].Trim();
            }
        }

        string PayCost = string.Empty;
        string BundleDate=string.Empty;
        string SpmStartDate = string.Empty;
        string SPMDate = string.Empty;
        private void FillCase0021Form(DataRow dr)
        {
            if (dr != null)
            {

                decimal decspmbal = 0;
                decimal decspmpaid = 0;
                decimal decbudget = 0;
                decimal decbudbalance = 0;
                dtAct_Date.Value = Convert.ToDateTime(dr["CASEACT_ACTY_DATE"].ToString());
                SPMDate = dr["CASEACT_ACTY_DATE"].ToString();
                dtAct_Date.Checked = true;
                if (!string.IsNullOrEmpty(dr["CASEACT_SEEK_DATE"].ToString()))
                {
                    dtActSeek_Date.Value = Convert.ToDateTime(dr["CASEACT_SEEK_DATE"].ToString());
                    dtActSeek_Date.Checked = true;
                }
                txtSerFund.Text = dr["SPM_FUND"].ToString();
                dtSerDate1.Value = Convert.ToDateTime(LookupDataAccess.Getdate(dr["SPM_STARTDATE"].ToString()));
                if (!string.IsNullOrEmpty(dr["SPM_STARTDATE"].ToString().Trim()))
                    dtSerDate1.Checked = true;

                CommonFunctions.SetComboBoxValue(cmbservicewkr, dr["SPM_CASEWORKER"].ToString());

                CommonFunctions.SetComboBoxValue(CmbSite, dr["CASEACT_SITE"].ToString().Trim());
                CommonFunctions.SetComboBoxValue(CmbWorker, dr["CASEACT_CASEWRKR"].ToString());

                if(!string.IsNullOrEmpty(dr["SPM_STARTDATE"].ToString().Trim()))
                    SpmStartDate = dr["SPM_STARTDATE"].ToString();

                if (propProgramdefinationdata.DepSerpostPAYCAT == "02")
                {
                    CommonFunctions.SetComboBoxValue(cmbErapFund, dr["CASEACT_FUND1"].ToString());
                    //  txtCat2BillPCode.Text = Pass_CA_Entity.BillingPeriod;
                    if (!string.IsNullOrEmpty(dr["CASEACT_BILL_PERIOD"].ToString()))
                    {
                        txtBillperiodcode.Text = dr["CASEACT_BILL_PERIOD"].ToString();
                        ListcommonEntity = new List<CommonEntity>();
                        string[] CountyList = dr["CASEACT_BILL_PERIOD"].ToString().Split(',');
                        if (CountyList.Length > 0)
                        {
                            string BillPeriodDesc = string.Empty;
                            foreach (string Cont in CountyList)
                            {
                                ListcommonEntity.Add(new CommonEntity(Cont, string.Empty));

                                if (BillPeriodEntity.Count > 0)
                                {
                                    foreach (CommonEntity Entity in BillPeriodEntity)
                                    {
                                        if (Entity.Code.Trim() == Cont.Trim())
                                        {
                                            BillPeriodDesc += Entity.Desc.Trim() + ", ";
                                            break;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(BillPeriodDesc.Trim()))
                            {
                                txtBillPeriod.Text = BillPeriodDesc.Substring(0, BillPeriodDesc.Length - 2);
                            }
                        }

                    }


                    txtErapAmtpaid.Text = dr["CASEACT_COST"].ToString();
                    txtErapVendor.Text = dr["CASEACT_VENDOR_NO"].ToString();
                    lblErapVendorName.Text = dr["CASEVDD_NAME"].ToString();
                    txtErapAccount.Text = dr["CASEACT_VEND_ACCT"].ToString();
                    string strBundleNO = "0000000000".Substring(0, (10 - dr["CASEACT_BUNDLE_NO"].ToString().Length)) + dr["CASEACT_BUNDLE_NO"].ToString();
                    txtErapBundle.Text = strBundleNO.ToString();
                    txtErapArrea.Text = dr["CASEACT_ARREARS"].ToString();

                    txtErapsentpayment.Text = dr["CASEACT_SENT_PMT_USER"].ToString();
                    txtErapSuper.Text = dr["CASEACT_LVL1_APRVL"].ToString();
                    txtErapPrepayment.Text = dr["CASEACT_LVL2_APRVL"].ToString();

                    if (dr["CASEACT_LVL1_APRVL_DATE"].ToString() != string.Empty)
                    {
                        dtErapDate1.Value = Convert.ToDateTime(dr["CASEACT_LVL1_APRVL_DATE"].ToString());
                        dtErapDate1.Checked = true;
                    }
                    if (dr["CASEACT_LVL2_APRVL_DATE"].ToString() != string.Empty)
                    {
                        dtErapDate2.Value = Convert.ToDateTime(dr["CASEACT_LVL2_APRVL_DATE"].ToString());
                        dtErapDate2.Checked = true;
                    }
                    if (dr["CASEACT_SENT_PMT_DATE"].ToString() != string.Empty)
                    {
                        dtErapPaymentOn.Value = Convert.ToDateTime(dr["CASEACT_SENT_PMT_DATE"].ToString());
                        dtErapPaymentOn.Checked = true;
                    }

                    if (dr["CASEACT_FOLLUP_ON"].ToString() != string.Empty)
                    {
                        dtErapFon.Value = Convert.ToDateTime(dr["CASEACT_FOLLUP_ON"].ToString());
                        dtErapFon.Checked = true;
                    }
                    if (dr["CASEACT_FOLLUP_COMP"].ToString() != string.Empty)
                    {
                        dtErapComplete.Value = Convert.ToDateTime(dr["CASEACT_FOLLUP_COMP"].ToString());
                        dtErapComplete.Checked = true;
                    }
                    txtErapby.Text = dr["CASEACT_FUPBY"].ToString();

                    if (!string.IsNullOrEmpty(dr["CASEACT_ACTY_PROG"].ToString()) && !dr["CASEACT_ACTY_PROG"].ToString().Contains("**"))
                        txtErapprogram.Text = Set_SP_Program_Text(dr["CASEACT_ACTY_PROG"].ToString());

                }
                else
                {
                    cmbFUnd.SelectedIndexChanged -= cmbFUnd_SelectedIndexChanged;
                    CommonFunctions.SetComboBoxValue(cmbFUnd, dr["CASEACT_FUND1"].ToString());
                    cmbFUnd.SelectedIndexChanged += cmbFUnd_SelectedIndexChanged;
                    if (!string.IsNullOrEmpty(dr["CASEACT_SOURCE"].ToString()))
                        CommonFunctions.SetComboBoxValue(cmbCat4Source, dr["CASEACT_SOURCE"].ToString().Trim());
                    txtAmountPaid.Text = dr["CASEACT_COST"].ToString();
                    PayCost = dr["CASEACT_COST"].ToString();


                    txtVendorNo.Text = dr["CASEACT_VENDOR_NO"].ToString();
                    lblCEAPVendorname.Text = dr["CASEVDD_NAME"].ToString();

                    /******************* Kranthi 01/23/2023:: To reselect the grid value in edit mode ******************************
                    Vendor_Details = new string[2];
                    Vendor_Details[0] = dr["CASEACT_VENDOR_NO"].ToString();
                    Vendor_Details[1] = dr["CASEVDD_NAME"].ToString();
                    /***************************************************/
                    txtAccountNo.Text = dr["CASEACT_VEND_ACCT"].ToString();
                    string strBundleNO = "0000000000".Substring(0, (10 - dr["CASEACT_BUNDLE_NO"].ToString().Length)) + dr["CASEACT_BUNDLE_NO"].ToString();
                    lblCeapBundle2.Text = strBundleNO.ToString();
                    lblCeapPay2.Text = dr["CASEACT_PAYMENT_NO"].ToString();
                    lblCeapCheck2.Text = dr["CASEACT_CHECK_NO"].ToString();
                    lblCeapCheckdate2.Text = LookupDataAccess.Getdate(dr["CASEACT_CHECK_DT"].ToString());
                    lblBudgetdesc2.Text = dr["BDC_DESCRIPTION"].ToString();
                    decbudget = dr["BDC_BUDGET"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["BDC_BUDGET"]);
                    lblBudget1.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decbudget);
                    lblStart1.Text = LookupDataAccess.Getdate(dr["BDC_START"].ToString());
                    lblEnd1.Text = LookupDataAccess.Getdate(dr["BDC_END"].ToString());
                    decbudbalance = dr["BDC_BALANCE"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["BDC_BALANCE"]);
                    CommonFunctions.SetComboBoxValue(cmbBenfitReason4, dr["CASEACT_BENEFIT_REASN"].ToString().Trim());


                    if (!string.IsNullOrEmpty(dr["CAB_DATE_OPENED"].ToString().Trim()))
                        BundleDate = dr["CAB_DATE_OPENED"].ToString();


                    List<CMBDCEntity> cmbdcdata = CMbdc_List.FindAll(u => u.BDC_FUND.ToString().Trim() == dr["CASEACT_FUND1"].ToString().Trim());
                    if(CMbdc_List.Count>0)
                    {
                        cmbdcdata = cmbdcdata.FindAll(u => Convert.ToDateTime(u.BDC_START.Trim()) <= Convert.ToDateTime(dtAct_Date.Text.Trim()) && Convert.ToDateTime(u.BDC_END.Trim()) >= Convert.ToDateTime(dtAct_Date.Text.Trim()));
                    }
                    fillBudgets(cmbdcdata, dr["CASEACT_BDC_ID"].ToString());

                    cmbBudget.SelectedIndexChanged -= new EventHandler(cmbBudget_SelectedIndexChanged);
                    CommonFunctions.SetComboBoxValue(cmbBudget, dr["CASEACT_BDC_ID"].ToString());
                    cmbBudget.SelectedIndexChanged += new EventHandler(cmbBudget_SelectedIndexChanged);

                    decspmbal = dr["SPM_BAL_AMT"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["SPM_BAL_AMT"]);
                    lblServiceBal.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decspmbal);

                    decspmpaid = (dr["SPM_AMOUNT"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["SPM_AMOUNT"])) - decspmbal;
                    lblServicePaid.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decspmpaid);

                    if (dr["CASEACT_FOLLUP_ON"].ToString() != string.Empty)
                    {
                        dtupOn.Value = Convert.ToDateTime(dr["CASEACT_FOLLUP_ON"].ToString());
                        dtupOn.Checked = true;
                    }
                    if (dr["CASEACT_FOLLUP_COMP"].ToString() != string.Empty)
                    {
                        dtComplet.Value = Convert.ToDateTime(dr["CASEACT_FOLLUP_COMP"].ToString());
                        dtComplet.Checked = true;
                    }
                    txttobeFollowUp.Text = dr["CASEACT_FUPBY"].ToString();

                    if (!string.IsNullOrEmpty(dr["CASEACT_ACTY_PROG"].ToString()) && !dr["CASEACT_ACTY_PROG"].ToString().Contains("**"))
                        txtCeapProgram.Text = Set_SP_Program_Text(dr["CASEACT_ACTY_PROG"].ToString());


                    lblBudgetBalance.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decbudbalance); ;
                    txtSerFund.Text = dr["SPM_FUND"].ToString();
                    dtSerDate1.Value = Convert.ToDateTime(LookupDataAccess.Getdate(dr["SPM_STARTDATE"].ToString()));
                    txtSerAmt.Text = dr["SPM_AMOUNT"].ToString();
                    CommonFunctions.SetComboBoxValue(cmbservicewkr, dr["SPM_CASEWORKER"].ToString());

                    if (dr["CASEACT_BILL_TYPE"].ToString() == "T")
                    {
                        CommonFunctions.SetComboBoxValue(cmbBilling, dr["CASEACT_BILL_TYPE"].ToString());
                    }
                    else if (!string.IsNullOrEmpty(dr["CASEACT_BILL_TYPE"].ToString()))
                    {
                        CommonFunctions.SetComboBoxValue(cmbBilling, dr["CASEACT_BILL_FNAME"].ToString().Trim() + dr["CASEACT_BILL_LNAME"].ToString());

                    }
                    if (dr["CASEACT_FUND1"].ToString().Trim() != dr["SPM_FUND"].ToString().Trim())
                    {
                        txtSerAmt.Enabled = false;
                    }

                    txtFirst.Text = dr["CASEACT_BILL_FNAME"].ToString();
                    txtLast.Text = dr["CASEACT_BILL_LNAME"].ToString();
                }

            }

        }

        List<HierarchyEntity> SP_Programs_List = new List<HierarchyEntity>();
        string Sel_CAMS_Program = string.Empty;
        private string Set_SP_Program_Text(string Prog_Code)
        {
            string Tmp_Hierarchy = "";
            Sel_CAMS_Program = "";

            foreach (HierarchyEntity Ent in SP_Programs_List)
            {
                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                if (Prog_Code == Tmp_Hierarchy)
                {
                    Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                    break;
                }
            }

            return Sel_CAMS_Program;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            string strSId = string.Empty;
            btnSave.Enabled = false;
            CaseActAdjEntity actAdjEntity = new CaseActAdjEntity();
            CASEACTEntity actentitydata = new CASEACTEntity();
            actAdjEntity.ADJ_AGENCY = BaseForm.BaseAgency;
            actAdjEntity.ADJ_DEPT = BaseForm.BaseDept;
            actAdjEntity.ADJ_PROGRAM = BaseForm.BaseProg;
            actAdjEntity.ADJ_YEAR = BaseForm.BaseYear;
            actAdjEntity.ADJ_APP_NO = BaseForm.BaseApplicationNo;
            actAdjEntity.ADJ_SERVICE_PLAN = propdr["CASEACT_SERVICE_PLAN"].ToString();
            actAdjEntity.ADJ_SPM_SEQ = propdr["CASEACT_SPM_SEQ"].ToString();
            actAdjEntity.ADJ_BRANCH = propdr["CASEACT_BRANCH"].ToString();
            actAdjEntity.ADJ_GROUP = propdr["CASEACT_GROUP"].ToString();
            actAdjEntity.ADJ_ACTIVITY_CODE = propdr["CASEACT_ACTIVITY_CODE"].ToString();
            actAdjEntity.ADJ_ACT_SEQ = propdr["CASEACT_SEQ"].ToString();

            actAdjEntity.ADJ_REASON = ((ListItem)cmbReason.SelectedItem).Value.ToString();
            actAdjEntity.ADJ_Date = dtAdjustment.Value.ToString();
            // actAdjEntity.ADJ_SEQ = propdr[""].ToString();
            actAdjEntity.ADJ_O_ACTYR_DATE = propdr["CASEACT_SEEK_DATE"].ToString();
            actAdjEntity.ADJ_O_ACTY_DATE = propdr["CASEACT_ACTY_DATE"].ToString();
            actAdjEntity.ADJ_O_CASEWRKR = propdr["CASEACT_CASEWRKR"].ToString();
            actAdjEntity.ADJ_O_SITE = propdr["CASEACT_SITE"].ToString();
            actAdjEntity.ADJ_O_VENDOR_NO = propdr["CASEACT_VENDOR_NO"].ToString();
            actAdjEntity.ADJ_O_VEND_ACCT = propdr["CASEACT_VEND_ACCT"].ToString();
            actAdjEntity.ADJ_O_BILLNAME_TYPE = propdr["CASEACT_BILL_TYPE"].ToString();
            actAdjEntity.ADJ_O_BILL_LNAME = propdr["CASEACT_BILL_LNAME"].ToString();
            actAdjEntity.ADJ_O_BILL_FNAME = propdr["CASEACT_BILL_FNAME"].ToString();
            actAdjEntity.ADJ_O_FUND1 = propdr["CASEACT_FUND1"].ToString();
            actAdjEntity.ADJ_O_BUDGET_ID = propdr["CASEACT_BDC_ID"].ToString();
            actAdjEntity.ADJ_O_COST = propdr["CASEACT_COST"].ToString();
            actAdjEntity.ADJ_O_FOLLUP_ON = propdr["CASEACT_FOLLUP_ON"].ToString();
            actAdjEntity.ADJ_O_FOLLUP_COMP = propdr["CASEACT_FOLLUP_COMP"].ToString();
            actAdjEntity.ADJ_O_FUPBY = propdr["CASEACT_FUPBY"].ToString();
            actAdjEntity.ADJ_O_ACTY_PROG = propdr["CASEACT_ACTY_PROG"].ToString();
            if (dtActSeek_Date.Checked)
                actAdjEntity.ADJ_N_ACTYR_DATE = dtActSeek_Date.Value.ToString();
            actAdjEntity.ADJ_N_ACTY_DATE = dtAct_Date.Value.ToString();
            if (((ListItem)CmbWorker.SelectedItem).Value.ToString() != "0")
                actAdjEntity.ADJ_N_CASEWRKR = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
            if (((ListItem)CmbSite.SelectedItem).Value.ToString() != "0")
                actAdjEntity.ADJ_N_SITE = ((ListItem)CmbSite.SelectedItem).Value.ToString();

            if (propProgramdefinationdata.DepSerpostPAYCAT == "02")
            {
                // actAdjEntity.ADJ_N_ACTYR_DATE = string.Empty;
                actAdjEntity.ADJ_N_ACTYR_DATE = string.Empty;
                actAdjEntity.ADJ_O_BILL_PERIOD = propdr["CASEACT_BILL_PERIOD"].ToString();
                actAdjEntity.ADJ_N_BILL_PERIOD = txtBillperiodcode.Text;

                actAdjEntity.ADJ_O_ARREARS = propdr["CASEACT_ARREARS"].ToString();
                actAdjEntity.ADJ_N_ARREARS = txtErapArrea.Text;


                actAdjEntity.ADJ_N_VENDOR_NO = txtErapVendor.Text;
                actAdjEntity.ADJ_N_VEND_ACCT = txtErapAccount.Text;

                //actAdjEntity.ADJ_N_BILLNAME_TYPE = ((ListItem)cmbBilling.SelectedItem).Value.ToString();
                //actAdjEntity.ADJ_N_BILL_LNAME = txtLast.Text;
                //actAdjEntity.ADJ_N_BILL_FNAME = txtFirst.Text;
                actAdjEntity.ADJ_N_FUND1 = ((ListItem)cmbErapFund.SelectedItem).Value.ToString();
                //actAdjEntity.ADJ_N_BUDGET_ID = ((ListItem)cmbBudget.SelectedItem).Value.ToString();
                actAdjEntity.ADJ_N_COST = txtErapAmtpaid.Text;

                if (dtErapFon.Checked)
                    actAdjEntity.ADJ_N_FOLLUP_ON = dtErapFon.Value.ToString();
                if (dtErapComplete.Checked)
                    actAdjEntity.ADJ_N_FOLLUP_COMP = dtErapComplete.Value.ToString();
                actAdjEntity.ADJ_N_FUPBY = txtErapby.Text;
                actAdjEntity.ADJ_N_ACTY_PROG = txtErapprogram.Text;

                actAdjEntity.SPM_O_CASEWRKR = propdr["SPM_CASEWORKER"].ToString();
                if (((ListItem)cmbservicewkr.SelectedItem).Value.ToString() != "0")
                    actAdjEntity.SPM_N_CASEWRKR = ((ListItem)cmbservicewkr.SelectedItem).Value.ToString();


                actAdjEntity.SPM_O_AMOUNT = propdr["SPM_AMOUNT"].ToString();
                actAdjEntity.SPM_N_AMOUNT = txtSerAmt.Text;

                actAdjEntity.ADJ_ADD_OPERATOR = BaseForm.UserID;
            }
            else
            {


                actAdjEntity.ADJ_O_SOURCE = propdr["CASEACT_SOURCE"].ToString();
                if (((Utilities.ListItem)cmbCat4Source.SelectedItem).Value.ToString() != "0")
                    actAdjEntity.ADJ_N_SOURCE = ((Utilities.ListItem)cmbCat4Source.SelectedItem).Value.ToString();

                actAdjEntity.ADJ_O_BENEFIT_REASN = propdr["CASEACT_BENEFIT_REASN"].ToString();
                if (((ListItem)cmbBenfitReason4.SelectedItem).Value.ToString() != "0")
                    actAdjEntity.ADJ_N_BENEFIT_REASN = ((ListItem)cmbBenfitReason4.SelectedItem).Value.ToString();

                actAdjEntity.ADJ_N_VENDOR_NO = txtVendorNo.Text;
                actAdjEntity.ADJ_N_VEND_ACCT = txtAccountNo.Text;
                if (((ListItem)cmbBilling.SelectedItem).Value.ToString() != "0")
                    actAdjEntity.ADJ_N_BILLNAME_TYPE = ((ListItem)cmbBilling.SelectedItem).Value.ToString();
                actAdjEntity.ADJ_N_BILL_LNAME = txtLast.Text;
                actAdjEntity.ADJ_N_BILL_FNAME = txtFirst.Text;
                actAdjEntity.ADJ_N_FUND1 = ((ListItem)cmbFUnd.SelectedItem).Value.ToString();
                if (cmbBudget.Items.Count > 0)
                {
                    if (((ListItem)cmbBudget.SelectedItem).Value.ToString() != "0")
                        actAdjEntity.ADJ_N_BUDGET_ID = ((ListItem)cmbBudget.SelectedItem).Value.ToString();
                }
                actAdjEntity.ADJ_N_COST = txtAmountPaid.Text;
                if (dtupOn.Checked)
                    actAdjEntity.ADJ_N_FOLLUP_ON = dtupOn.Value.ToString();
                if (dtComplet.Checked)
                    actAdjEntity.ADJ_N_FOLLUP_COMP = dtComplet.Value.ToString();
                actAdjEntity.ADJ_N_FUPBY = txttobeFollowUp.Text;
                actAdjEntity.ADJ_N_ACTY_PROG = txtCeapProgram.Text;

                actAdjEntity.SPM_O_CASEWRKR = propdr["SPM_CASEWORKER"].ToString();
                if (((ListItem)cmbservicewkr.SelectedItem).Value.ToString() != "0")
                    actAdjEntity.SPM_N_CASEWRKR = ((ListItem)cmbservicewkr.SelectedItem).Value.ToString();


                actAdjEntity.SPM_O_AMOUNT = propdr["SPM_AMOUNT"].ToString();
                actAdjEntity.SPM_N_AMOUNT = txtSerAmt.Text;

                actAdjEntity.ADJ_ADD_OPERATOR = BaseForm.UserID;
                actAdjEntity.Mode = "Add";



                if (propCEAPCNTLSwitch == "Y")
                {
                    if (txtAmountPaid.Text != string.Empty)
                    {
                        if (Convert.ToDecimal(txtAmountPaid.Text) > 0)
                        {

                            if (Convert.ToDecimal(txtAmountPaid.Text) > (Convert.ToDecimal(lblServiceBal.Text) + (!string.IsNullOrEmpty(propdr["CASEACT_COST"].ToString()) ? Convert.ToDecimal(propdr["CASEACT_COST"]) : 0)))
                            //if (Convert.ToDecimal(txtInvAmt.Text) > (Convert.ToDecimal(Oper_EMSBDCEntity.BDC_BALANCE) + (!string.IsNullOrEmpty(propEmsclcpmcentity.PMC_AMOUNT.Trim()) ? Convert.ToDecimal(propEmsclcpmcentity.PMC_AMOUNT) : 0)))
                            {
                                btnSave.Enabled = true;
                                AlertBox.Show("Amount paid may not exceed " + (Convert.ToDecimal(lblServiceBal.Text) + (!string.IsNullOrEmpty(propdr["CASEACT_COST"].ToString().Trim()) ? Convert.ToDecimal(propdr["CASEACT_COST"]) : 0)).ToString(), MessageBoxIcon.Warning);
                                //CommonFunctions.MessageBoxDisplay("Amount may not exceed " + (Convert.ToDecimal(Oper_EMSBDCEntity.BDC_BALANCE) + (!string.IsNullOrEmpty(propEmsclcpmcentity.PMC_AMOUNT.Trim()) ? Convert.ToDecimal(propEmsclcpmcentity.PMC_AMOUNT) : 0)).ToString());
                                txtAmountPaid.Text = string.Empty;
                                txtAmountPaid.Focus();
                                return;
                            }


                        }
                    }
                }
            }
            actentitydata.Agency = BaseForm.BaseAgency;
            actentitydata.Dept = BaseForm.BaseDept;
            actentitydata.Program = BaseForm.BaseProg;
            actentitydata.Year = BaseForm.BaseYear;
            actentitydata.App_no = BaseForm.BaseApplicationNo;
            actentitydata.Service_plan = propdr["CASEACT_SERVICE_PLAN"].ToString();
            actentitydata.SPM_Seq = propdr["CASEACT_SPM_SEQ"].ToString();
            actentitydata.Branch = propdr["CASEACT_BRANCH"].ToString();
            actentitydata.Group = propdr["CASEACT_GROUP"].ToString();
            actentitydata.ACT_Code = propdr["CASEACT_ACTIVITY_CODE"].ToString();
            actentitydata.ACT_Seq = propdr["CASEACT_SEQ"].ToString();

            if (dtActSeek_Date.Visible == true && dtActSeek_Date.Checked)
                actentitydata.ActSeek_Date = dtActSeek_Date.Value.ToString();
            else if(dtActSeek_Date.Checked)
                actentitydata.ActSeek_Date = dtActSeek_Date.Value.ToString();

            actentitydata.ACT_Date = dtAct_Date.Value.ToString();
            if (((ListItem)CmbWorker.SelectedItem).Value.ToString() != "0")
                actentitydata.Caseworker = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
            if (((ListItem)CmbSite.SelectedItem).Value.ToString() != "0")
                actentitydata.Site = ((ListItem)CmbSite.SelectedItem).Value.ToString().Trim();

            if (propProgramdefinationdata.DepSerpostPAYCAT == "02")
            {

                actentitydata.Vendor_No = txtErapVendor.Text;
                actentitydata.Account = txtErapAccount.Text;

                actentitydata.ArrearsAmt = txtErapArrea.Text;
                actentitydata.BillingPeriod = txtBillperiodcode.Text;
                actentitydata.Fund1 = ((ListItem)cmbErapFund.SelectedItem).Value.ToString();
                actentitydata.Cost = txtErapAmtpaid.Text;
                if (dtErapFon.Checked)
                    actentitydata.Followup_On = dtErapFon.Value.ToString();
                if (dtErapComplete.Checked)
                    actentitydata.Followup_Comp = dtErapComplete.Value.ToString();
                actentitydata.Followup_By = txtErapby.Text;
                actentitydata.Act_PROG = txtErapprogram.Text;
                //actentitydata.Amount = txtSerAmt.Text;
                if (((ListItem)cmbservicewkr.SelectedItem).Value.ToString() != "0")
                    actentitydata.VendorName = ((ListItem)cmbservicewkr.SelectedItem).Value.ToString();
            }
            else
            {
                actentitydata.Vendor_No = txtVendorNo.Text;
                actentitydata.Account = txtAccountNo.Text;
                if (((ListItem)cmbBilling.SelectedItem).Value.ToString() != "0")
                    actentitydata.BillngType = ((ListItem)cmbBilling.SelectedItem).Value.ToString();
                actentitydata.BillngLname = txtLast.Text;
                actentitydata.BillngFname = txtFirst.Text;
                if (((ListItem)cmbFUnd.SelectedItem).Value.ToString() != "0")
                    actentitydata.Fund1 = ((ListItem)cmbFUnd.SelectedItem).Value.ToString();
                if (cmbBudget.Items.Count > 0)
                {
                    if (((ListItem)cmbBudget.SelectedItem).Value.ToString() != "0")
                        actentitydata.BDC_ID = ((ListItem)cmbBudget.SelectedItem).Value.ToString();
                }
                actentitydata.Cost = txtAmountPaid.Text;
                if (dtupOn.Checked)
                    actentitydata.Followup_On = dtupOn.Value.ToString();
                if (dtComplet.Checked)
                    actentitydata.Followup_Comp = dtComplet.Value.ToString();
                actentitydata.Followup_By = txttobeFollowUp.Text;
                actentitydata.Act_PROG = txtCeapProgram.Text;
                actentitydata.Amount = txtSerAmt.Text;
                if (cmbCat4Source.Items.Count > 0)
                {
                    if (((ListItem)cmbCat4Source.SelectedItem).Value.ToString() != "0")
                        actentitydata.CA_Source = ((ListItem)cmbCat4Source.SelectedItem).Value.ToString();
                }
                if (((ListItem)cmbservicewkr.SelectedItem).Value.ToString() != "0")
                    actentitydata.VendorName = ((ListItem)cmbservicewkr.SelectedItem).Value.ToString();
                if (((ListItem)cmbBenfitReason4.SelectedItem).Value.ToString() != "0")
                    actentitydata.BenefitReason = ((ListItem)cmbBenfitReason4.SelectedItem).Value.ToString();
            }
            actentitydata.Lsct_Operator = BaseForm.UserID;

            if (_model.SPAdminData.CAPS_SPMACT_UPDATE(actentitydata, propCEAPCNTLSwitch, propProgramdefinationdata.DepSerpostPAYCAT))
            {
                if (!CheckAdjmentdata(actAdjEntity))
                {
                    if (propProgramdefinationdata.DepSerpostPAYCAT == "02")
                    {
                        actAdjEntity.ADJ_O_ACTYR_DATE = actAdjEntity.ADJ_N_ACTYR_DATE = string.Empty;
                    }
                    actAdjEntity.Mode = "Add";
                    _model.SPAdminData.CAPS_CASEACTADJ_INSUPDEL(actAdjEntity);
                }

                if (propProgramdefinationdata.DepSerpostPAYCAT == "04")
                {
                    string strBudgetId = string.Empty;
                    if (cmbBudget.Items.Count > 0)
                    {
                        if (((ListItem)cmbBudget.SelectedItem).Value.ToString() != "0")
                            strBudgetId = ((ListItem)cmbBudget.SelectedItem).Value.ToString();
                    }

                    CMBDCEntity cmbdcentity = new CMBDCEntity();
                    if (strBudgetId != string.Empty)
                    {
                        if (CMbdc_List.Count > 0)
                            cmbdcentity = CMbdc_List.Find(u => u.BDC_ID == strBudgetId);

                        if (cmbdcentity != null)
                        {
                            CMBDCEntity cmbdcdata = new CMBDCEntity();
                            cmbdcdata.BDC_AGENCY = cmbdcentity.BDC_AGENCY;
                            cmbdcdata.BDC_DEPT = cmbdcentity.BDC_DEPT;
                            cmbdcdata.BDC_PROGRAM = cmbdcentity.BDC_PROGRAM;
                            cmbdcdata.BDC_YEAR = cmbdcentity.BDC_YEAR;


                            cmbdcdata.BDC_DESCRIPTION = cmbdcentity.BDC_DESCRIPTION;
                            cmbdcdata.BDC_FUND = cmbdcentity.BDC_FUND;
                            cmbdcdata.BDC_ID = cmbdcentity.BDC_ID;
                            cmbdcdata.BDC_START = cmbdcentity.BDC_START;
                            cmbdcdata.BDC_END = cmbdcentity.BDC_END;
                            cmbdcdata.BDC_BUDGET = cmbdcentity.BDC_BUDGET;
                            cmbdcdata.BDC_LSTC_OPERATOR = BaseForm.UserID;
                            cmbdcdata.Mode = "BdcAmount";
                            string strstatus = string.Empty;
                            if (_model.SPAdminData.InsertUpdateDelCMBDC(cmbdcdata, out strstatus))
                            { }
                        }

                    }
                    if (propdr["CASEACT_BDC_ID"].ToString().Trim() != strBudgetId)
                    {
                        cmbdcentity = new CMBDCEntity();
                        if (CMbdc_List.Count > 0)
                            cmbdcentity = CMbdc_List.Find(u => u.BDC_ID == propdr["CASEACT_BDC_ID"].ToString());

                        if (cmbdcentity != null)
                        {
                            CMBDCEntity cmbdcdata = new CMBDCEntity();
                            cmbdcdata.BDC_AGENCY = cmbdcentity.BDC_AGENCY;
                            cmbdcdata.BDC_DEPT = cmbdcentity.BDC_DEPT;
                            cmbdcdata.BDC_PROGRAM = cmbdcentity.BDC_PROGRAM;
                            cmbdcdata.BDC_YEAR = cmbdcentity.BDC_YEAR;


                            cmbdcdata.BDC_DESCRIPTION = cmbdcentity.BDC_DESCRIPTION;
                            cmbdcdata.BDC_FUND = cmbdcentity.BDC_FUND;
                            cmbdcdata.BDC_ID = cmbdcentity.BDC_ID;
                            cmbdcdata.BDC_START = cmbdcentity.BDC_START;
                            cmbdcdata.BDC_END = cmbdcentity.BDC_END;
                            cmbdcdata.BDC_BUDGET = cmbdcentity.BDC_BUDGET;
                            cmbdcdata.BDC_LSTC_OPERATOR = BaseForm.UserID;
                            cmbdcdata.Mode = "BdcAmount";
                            string strstatus = string.Empty;
                            if (_model.SPAdminData.InsertUpdateDelCMBDC(cmbdcdata, out strstatus))
                            { }
                        }
                    }
                }
                AlertBox.Show("Updated Successfully.");
                CASE0021Control case0021Control = BaseForm.GetBaseUserControl() as CASE0021Control;
                if (case0021Control != null)
                {
                    case0021Control.Refresh();
                    this.Close();
                }
            }

        }

        private bool CheckAdjmentdata(CaseActAdjEntity actAdjEntity)
        {
            bool boolcheckstatus = true;

            if (LookupDataAccess.Getdate(actAdjEntity.ADJ_O_ACTYR_DATE) != LookupDataAccess.Getdate(actAdjEntity.ADJ_N_ACTYR_DATE))
                boolcheckstatus = false;
            if (LookupDataAccess.Getdate(actAdjEntity.ADJ_O_ACTY_DATE) != LookupDataAccess.Getdate(actAdjEntity.ADJ_N_ACTY_DATE))
                boolcheckstatus = false;
            if (actAdjEntity.ADJ_O_CASEWRKR != actAdjEntity.ADJ_N_CASEWRKR)
                boolcheckstatus = false;

            if (actAdjEntity.ADJ_O_SITE.Trim() != actAdjEntity.ADJ_N_SITE.Trim())
                boolcheckstatus = false;

            if (actAdjEntity.ADJ_O_VENDOR_NO != actAdjEntity.ADJ_N_VENDOR_NO)
                boolcheckstatus = false;

            if (actAdjEntity.ADJ_O_VEND_ACCT != actAdjEntity.ADJ_N_VEND_ACCT)
                boolcheckstatus = false;

            if (actAdjEntity.ADJ_O_COST != actAdjEntity.ADJ_N_COST)
                boolcheckstatus = false;

            if (LookupDataAccess.Getdate(actAdjEntity.ADJ_O_FOLLUP_ON) != LookupDataAccess.Getdate(actAdjEntity.ADJ_N_FOLLUP_ON))
                boolcheckstatus = false;

            if (LookupDataAccess.Getdate(actAdjEntity.ADJ_O_FOLLUP_COMP) != LookupDataAccess.Getdate(actAdjEntity.ADJ_N_FOLLUP_COMP))
                boolcheckstatus = false;

            if (actAdjEntity.ADJ_O_FUPBY != actAdjEntity.ADJ_N_FUPBY)
                boolcheckstatus = false;

            if (!string.IsNullOrEmpty(actAdjEntity.ADJ_N_ACTY_PROG.Trim()))
            {
                if (actAdjEntity.ADJ_O_ACTY_PROG != actAdjEntity.ADJ_N_ACTY_PROG.Substring(0, 6))
                    boolcheckstatus = false;
            }

            if (actAdjEntity.ADJ_O_FUND1.Trim() != actAdjEntity.ADJ_N_FUND1.Trim())
                boolcheckstatus = false;

            //if (actAdjEntity.ADJ_O_BILLNAME_TYPE != actAdjEntity.ADJ_N_BILLNAME_TYPE)
            //    boolcheckstatus = false;

            if (actAdjEntity.SPM_O_CASEWRKR!= actAdjEntity.SPM_N_CASEWRKR)
                boolcheckstatus = false;

            if (propProgramdefinationdata.DepSerpostPAYCAT == "02")
            {
                if (actAdjEntity.ADJ_O_ARREARS != actAdjEntity.ADJ_N_ARREARS)
                    boolcheckstatus = false;

                if (actAdjEntity.ADJ_O_BILL_PERIOD != actAdjEntity.ADJ_N_BILL_PERIOD)
                    boolcheckstatus = false;

            }
            else
            {
                if (actAdjEntity.ADJ_O_BILL_LNAME != actAdjEntity.ADJ_N_BILL_LNAME)
                    boolcheckstatus = false;

                if (actAdjEntity.ADJ_O_BILL_FNAME != actAdjEntity.ADJ_N_BILL_FNAME)
                    boolcheckstatus = false;


                if (actAdjEntity.ADJ_O_BUDGET_ID != actAdjEntity.ADJ_N_BUDGET_ID)
                    boolcheckstatus = false;

                if (actAdjEntity.ADJ_O_SOURCE != actAdjEntity.ADJ_N_SOURCE)
                    boolcheckstatus = false;

                if (actAdjEntity.ADJ_O_BENEFIT_REASN != actAdjEntity.ADJ_N_BENEFIT_REASN)
                    boolcheckstatus = false;

                if (actAdjEntity.SPM_O_AMOUNT != actAdjEntity.SPM_N_AMOUNT)
                    boolcheckstatus = false;

            }

            return boolcheckstatus;
        }


        private bool ValidateForm()
        {
            bool isValid = true;

            _errorProvider.SetError(panel_Referral2, null);
            _errorProvider.SetError(txtAmountPaid, null);
            _errorProvider.SetError(txtSerAmt, null);
            _errorProvider.SetError(CmbWorker, null);
            _errorProvider.SetError(CmbSite, null);
            _errorProvider.SetError(cmbservicewkr, null);
            _errorProvider.SetError(dtActSeek_Date, null);
            _errorProvider.SetError(dtAct_Date, null);
            _errorProvider.SetError(cmbBilling, null);
            _errorProvider.SetError(cmbFUnd, null);
            _errorProvider.SetError(cmbBudget, null);
            _errorProvider.SetError(cmbBenfitReason4, null);

            if ((((ListItem)cmbReason.SelectedItem).Value.ToString().Equals("0")))
            {
                _errorProvider.SetError(cmbReason, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReason.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbReason, null);
            }
            if (dtAdjustment.Checked == false)
            {
                _errorProvider.SetError(dtAdjustment, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lbladjDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtAdjustment, null);
            }

            if (dtAct_Date.Checked == false)
            {
                _errorProvider.SetError(dtAct_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActivityDate.Text));
                isValid = false;
            }
            else
            {
                //Commented by Sudheer on 05/24/23 based on the jen's request by document 'STDC.docx'
                //if (!string.IsNullOrEmpty(SpmStartDate.Trim()))
                //{
                //    if (string.IsNullOrEmpty(BundleDate.Trim()))
                //    {
                //        if (dtAct_Date.Value < Convert.ToDateTime(SpmStartDate))
                //        {
                //            _errorProvider.SetError(dtAct_Date, string.Format("' " + lblActivityDate.Text + "' Should not be Prior to 'Service Plan Master Date'".Replace(Consts.Common.Colon, string.Empty)));
                //            isValid = false;
                //        }
                //        else
                //            _errorProvider.SetError(dtAct_Date, null);
                //    }
                //    else
                //    {

                //        if (dtAct_Date.Value < Convert.ToDateTime(SpmStartDate) || dtAct_Date.Value > Convert.ToDateTime(BundleDate))
                //        {
                //            _errorProvider.SetError(dtAct_Date, string.Format("' " + lblActivityDate.Text + "' Should not be Prior to 'Service Plan Master Date' OR 'Actual Completion Date'".Replace(Consts.Common.Colon, string.Empty)));
                //            isValid = false;
                //        }
                //        else
                //            _errorProvider.SetError(dtAct_Date, null);
                //    }

                //}

                //Commented on 06/03/23 by sudheer based on the NCCAA document
                //if(!string.IsNullOrEmpty(BundleDate.Trim()))
                //{
                //    if(dtAct_Date.Value > Convert.ToDateTime(BundleDate))
                //    {
                //        _errorProvider.SetError(dtAct_Date, string.Format("' " + lblActivityDate.Text + "' Should not be greater to 'Bundle Date'".Replace(Consts.Common.Colon, string.Empty)));
                //        isValid = false;
                //    }
                //    else _errorProvider.SetError(dtAct_Date, null);
                //}
                //else
                //{
                //    _errorProvider.SetError(dtAct_Date, null);
                //}
            }

            if (dtActSeek_Date.Checked == false && dtActSeek_Date.Visible == true && lblActSeekDateReq.Visible == true)
            {
                _errorProvider.SetError(dtActSeek_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActSeekDate.Text));
                isValid = false;
            }


            if ((((ListItem)CmbWorker.SelectedItem).Value.ToString().Equals("0")) && lblCaseworcaReq.Visible == true)
            {
                _errorProvider.SetError(CmbWorker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseworca.Text));
                isValid = false;
            }

            if ((((ListItem)CmbSite.SelectedItem).Value.ToString().Equals("0")) && lblSitecaReq.Visible == true)
            {
                _errorProvider.SetError(CmbSite, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSitecaReq.Text));
                isValid = false;
            }
            if ((((ListItem)cmbservicewkr.SelectedItem).Value.ToString().Equals("0")))
            {
                _errorProvider.SetError(cmbservicewkr, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResourewkr.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbservicewkr, null);
            }

            if (propProgramdefinationdata.DepSerpostPAYCAT == "02")
            {


                if (string.IsNullOrEmpty(txtErapVendor.Text) && lblErapVendorReq.Visible == true)
                {
                    _errorProvider.SetError(pnlErapVendor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapVendor.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(pnlErapVendor, null);
                }

                if (string.IsNullOrEmpty(txtBillPeriod.Text) && lblErapBillpReq.Visible == true)
                {
                    _errorProvider.SetError(btnBillperiod, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapBillPeriod.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(btnBillperiod, null);
                }
                if (string.IsNullOrEmpty(txtErapAccount.Text) && lblErapAccReq.Visible == true)
                {
                    _errorProvider.SetError(txtErapAccount, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapAcct.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtErapAccount, null);
                }

                if (string.IsNullOrEmpty(txtErapAmtpaid.Text) && lblErapAmtReq.Visible == true)
                {
                    _errorProvider.SetError(txtErapAmtpaid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapAmtpaid.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtErapAmtpaid, null);
                }

                if (string.IsNullOrEmpty(txtErapArrea.Text) && lblErapArreaAmtReq.Visible == true)
                {
                    _errorProvider.SetError(txtErapArrea, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapArreas.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtErapArrea, null);
                }

                if ((((ListItem)cmbErapFund.SelectedItem).Value.ToString().Equals("0")) && lblErapFundReq.Visible == true)
                {
                    _errorProvider.SetError(cmbErapFund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCEAPFund.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbErapFund, null);
                }

                if (dtErapFon.Checked == false && lblErapOnReq.Visible == true)
                {
                    _errorProvider.SetError(dtErapFon, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapOn.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtErapFon, null);
                }

                if (dtErapComplete.Checked == false && lblErapCompReq.Visible == true)
                {
                    _errorProvider.SetError(dtErapComplete, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapComplete.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtErapComplete, null);
                }

                if (string.IsNullOrEmpty(txtErapby.Text) && lblErapByReq.Visible == true)
                {
                    _errorProvider.SetError(txtErapby, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapby.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtErapby, null);
                }

                if (string.IsNullOrEmpty(txtErapprogram.Text) && lblErapByReq.Visible == true)
                {
                    _errorProvider.SetError(pnlErapProgram, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapProgram.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(pnlErapProgram, null);
                }


            }
            else
            {
                _errorProvider.SetError(cmbCat4Source, null);
                if (lblCat4SourceReq.Visible && (cmbCat4Source.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbCat4Source.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(cmbCat4Source, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCat4Source.Text));
                    isValid = false;
                }


                if (string.IsNullOrEmpty(txtVendorNo.Text) && lblVendorReq.Visible == true)
                {
                    _errorProvider.SetError(panel_Referral2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblErapVendor.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(panel_Referral2, null);
                }

                if ((((ListItem)cmbFUnd.SelectedItem).Value.ToString().Equals("0")) && lblFundReq.Visible == true)
                {
                    _errorProvider.SetError(cmbFUnd, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCEAPFund.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbFUnd, null);
                }


                if (cmbBudget.Items.Count > 0)
                {
                    if ((((ListItem)cmbBudget.SelectedItem).Value.ToString().Equals("0")) && lblBudgetReq.Visible == true)
                    {
                        _errorProvider.SetError(cmbBudget, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCEAPBudget.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbBudget, null);
                    }

                }

                if (string.IsNullOrEmpty(txtAmountPaid.Text) && lblCeapAmtReq.Visible == true)
                {
                    _errorProvider.SetError(txtAmountPaid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCeapAmtpaid.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAmountPaid, null);
                }

                decimal decamountpaid = 0;
                if (txtAmountPaid.Text != string.Empty)
                    decamountpaid = Convert.ToDecimal(txtAmountPaid.Text);
                if (decamountpaid > 0)
                {

                }


                if (string.IsNullOrEmpty(txtAccountNo.Text) && lblAcctReq.Visible == true)
                {
                    _errorProvider.SetError(txtAccountNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCeapAccount.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAccountNo, null);
                }

                if (lblReqBenfitReason4.Visible && (cmbBenfitReason4.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbBenfitReason4.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(cmbBenfitReason4, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReqBenfitReason4.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbBenfitReason4, null);

                if (dtupOn.Checked == false && lblFollowupReq.Visible == true)
                {
                    _errorProvider.SetError(dtupOn, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFollowup.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtupOn, null);
                }

                if (dtComplet.Checked == false && lblFollowupComplReq.Visible == true)
                {
                    _errorProvider.SetError(dtComplet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFollowupCompl.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtComplet, null);
                }

                if (string.IsNullOrEmpty(txttobeFollowUp.Text) && lblTobeFolledReq.Visible == true)
                {
                    _errorProvider.SetError(txttobeFollowUp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTobeFolled.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txttobeFollowUp, null);
                }

                if (string.IsNullOrEmpty(txtCeapProgram.Text) && lblProgramReq.Visible == true)
                {
                    _errorProvider.SetError(pnlCeapprogram, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblProgram.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(pnlCeapprogram, null);
                }

                if ((((ListItem)cmbBilling.SelectedItem).Value.ToString().Equals("0")) && lblBillNameReq.Visible == true)
                {
                    _errorProvider.SetError(cmbBilling, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBillingName.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbBilling, null);
                }

                if (isValid)
                {
                    if (txtVendorNo.Text != string.Empty)
                    {

                        CaseVDD1Entity Vdd1_Entity = new CaseVDD1Entity(true);
                        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();

                        string strSourceType = string.Empty;
                        if (((Utilities.ListItem)cmbCat4Source.SelectedItem).Value.ToString() != "0")
                        {
                            isValid = false;
                            strSourceType = ((ListItem)cmbCat4Source.SelectedItem).Value.ToString().Trim();
                            Vdd1_Entity.Code = txtVendorNo.Text;
                            List<CaseVDD1Entity> Vdd1list = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");
                            foreach (CaseVDD1Entity Entity in Vdd1list)
                            {

                                if ((Entity.FUEL_TYPE1.Trim() == strSourceType) || (Entity.FUEL_TYPE2.Trim() == strSourceType) ||
                                    (Entity.FUEL_TYPE3.Trim() == strSourceType) || (Entity.FUEL_TYPE4.Trim() == strSourceType) ||
                                    (Entity.FUEL_TYPE5.Trim() == strSourceType) || (Entity.FUEL_TYPE6.Trim() == strSourceType) ||
                                    (Entity.FUEL_TYPE7.Trim() == strSourceType) || (Entity.FUEL_TYPE8.Trim() == strSourceType) ||
                                    (Entity.FUEL_TYPE9.Trim() == strSourceType) || (Entity.FUEL_TYPE10.Trim() == strSourceType))
                                {

                                    if (txtVendorNo.Text.Trim() == Entity.Code.Trim())
                                    {
                                        isValid = true;
                                        break;
                                    }
                                }
                            }
                            if (!isValid)
                            {
                                AlertBox.Show("Please select a Vendor for the new Source", MessageBoxIcon.Warning);
                            }
                        }

                    }
                }
                if (propCEAPCNTLSwitch == "Y")
                {
                    if (string.IsNullOrEmpty(txtSerAmt.Text))
                    {
                        _errorProvider.SetError(txtSerAmt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResouceAmount.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtSerAmt, null);
                        if (Convert.ToDecimal(txtSerAmt.Text) <= 0)
                        {
                            _errorProvider.SetError(txtSerAmt, "Amount should't be zero");
                            isValid = false;
                        }
                        else
                        {

                            decimal decamount = 0;
                            decimal decbal = 0;
                            decimal decbudgetbal = 0;
                            decimal decSeramount = 0;
                            decimal deccheckSeramount = 0;

                            if (propdr["SPM_AMOUNT"].ToString() != string.Empty)
                                decSeramount = Convert.ToDecimal(propdr["SPM_AMOUNT"]);

                            if (txtSerAmt.Text.Trim() != string.Empty)
                                decamount = Convert.ToDecimal(txtSerAmt.Text);
                            if (lblServiceBal.Text.Trim() != string.Empty)
                                decbal = Convert.ToDecimal(lblServiceBal.Text);
                            if (lblBudgetBalance.Text != string.Empty)
                                decbudgetbal = Convert.ToDecimal(lblBudgetBalance.Text);

                            deccheckSeramount = decSeramount - decbal;
                            decbal = decSeramount + decbudgetbal;

                            if (decamount > decbal)
                            {
                                AlertBox.Show("Insufficent funds in budget to post this SPM Amount should not be less than $ " + decbal, MessageBoxIcon.Warning);
                                isValid = false;
                            }
                            if (decSeramount < deccheckSeramount)
                            {
                                if (deccheckSeramount > decamount)
                                {
                                    // _errorProvider.SetError(txtResouAmt, "Resource amount greater than" + deccheckResamount);
                                    AlertBox.Show("The SPM Amount should not be less than $ " + deccheckSeramount, MessageBoxIcon.Warning);
                                    isValid = false;
                                }
                            }
                            else
                            {
                                if (deccheckSeramount > decamount)
                                {
                                    AlertBox.Show("The SPM Amount should not be less than $ " + deccheckSeramount, MessageBoxIcon.Warning);
                                    isValid = false;
                                }

                            }
                            //if (isValid)
                            //{
                            //    decimal decBdcBalance = 0;
                            //    if (lblBudgetBalance.Text.Trim() != string.Empty)
                            //        decBdcBalance = Convert.ToDecimal(lblBudgetBalance.Text);
                            //    List<EMSBDCEntity> propEmsbdc_List = _model.EMSBDCData.GetEmsBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            //    if (Oper_EMSBDCEntity != null)
                            //    {
                            //        EMSBDCEntity emsbdcentity = propEmsbdc_List.Find(u => u.BDC_FUND == Oper_EMSBDCEntity.BDC_FUND.ToString() && (Convert.ToDateTime(u.BDC_START) <= dtResourDate1.Value && Convert.ToDateTime(u.BDC_END) >= dtResourDate1.Value));
                            //        if (emsbdcentity != null)
                            //        {
                            //            //if ((emsbdcentity.BDC_LOCK_BY == string.Empty && emsbdcentity.BDC_LOCK_ON == string.Empty) || (emsbdcentity.BDC_LOCK_BY == BaseForm.UserID && emsbdcentity.BDC_LOCK_SCREEN == "RESOURCE"))
                            //            //{
                            //            decBdcBalance = Convert.ToDecimal(emsbdcentity.BDC_BALANCE);
                            //            //}
                            //        }
                            //        if (txt8.Text != string.Empty)
                            //        {
                            //            if (Convert.ToDecimal(txt8.Text) != decBdcBalance)
                            //            {
                            //                txt8.Text = decBdcBalance.ToString();
                            //                if (txtResouAmt.Text.Trim() != string.Empty)
                            //                    decamount = Convert.ToDecimal(txtResouAmt.Text);
                            //                if (txtResourBal.Text.Trim() != string.Empty)
                            //                    decbal = Convert.ToDecimal(txtResourBal.Text);
                            //                if (txt8.Text != string.Empty)
                            //                    decbudgetbal = Convert.ToDecimal(txt8.Text);

                            //                deccheckSeramount = decSeramount - decbal;
                            //                decbal = decSeramount + decbudgetbal;

                            //                if (decamount > decbal)
                            //                {
                            //                    CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this resource account \n Amount may not be more than " + decbal);
                            //                    isValid = false;
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            //if (decResamount < deccheckResamount)
                            //{
                            //    if (deccheckResamount > decamount)
                            //    {
                            //        // _errorProvider.SetError(txtResouAmt, "Resource amount greater than" + deccheckResamount);
                            //        CommonFunctions.MessageBoxDisplay("this resource account  Amount greater than " + deccheckResamount);
                            //        isValid = false;
                            //    }
                            //}

                        }
                    }
                }


                if (lblBudgetBalance.Text != string.Empty)
                {
                    if (isValid)
                    {
                        decimal decBdcBalance = 0;
                        decimal decAmount = 0;
                        decimal ActualCost = 0;

                        if (lblBudgetBalance.Text.Trim() != string.Empty)
                            decBdcBalance = Convert.ToDecimal(lblBudgetBalance.Text);

                        if (txtAmountPaid.Text.Trim() != string.Empty)
                            decAmount = Convert.ToDecimal(txtAmountPaid.Text);

                        if (PayCost.Trim() != string.Empty)
                            ActualCost = Convert.ToDecimal(PayCost);

                        decAmount = decAmount - ActualCost;
                        //if (decAmount>ActualCost && decAmount > decBdcBalance)
                        //{
                        //    AlertBox.Show("Insufficent funds in budget to post this SPM Amount should not be less than $ " + decBdcBalance, MessageBoxIcon.Warning);
                        //    isValid = false;
                        //}

                        if (decAmount > decBdcBalance)
                        {
                            AlertBox.Show("Insufficent funds in budget to post this SPM Amount should not be less than $ " + decBdcBalance, MessageBoxIcon.Warning);
                            isValid = false;
                        }

                        //    List<EMSBDCEntity> propEmsbdc_List = _model.EMSBDCData.GetEmsBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        //    if (Oper_EMSBDCEntity != null)
                        //    {

                        //        if (txt8.Text != string.Empty)
                        //        {
                        //            if (Convert.ToDecimal(txt8.Text) != decBdcBalance)
                        //            {
                        //                txt8.Text = decBdcBalance.ToString();
                        //                if (txtResouAmt.Text.Trim() != string.Empty)
                        //                    decamount = Convert.ToDecimal(txtResouAmt.Text);
                        //                if (txtResourBal.Text.Trim() != string.Empty)
                        //                    decbal = Convert.ToDecimal(txtResourBal.Text);
                        //                if (txt8.Text != string.Empty)
                        //                    decbudgetbal = Convert.ToDecimal(txt8.Text);

                        //                deccheckSeramount = decSeramount - decbal;
                        //                decbal = decSeramount + decbudgetbal;

                        //                if (decamount > decbal)
                        //                {
                        //                    CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this resource account \n Amount may not be more than " + decbal);
                        //                    isValid = false;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //if (decResamount < deccheckResamount)
                        //{
                        //    if (deccheckResamount > decamount)
                        //    {
                        //        // _errorProvider.SetError(txtResouAmt, "Resource amount greater than" + deccheckResamount);
                        //        CommonFunctions.MessageBoxDisplay("this resource account  Amount greater than " + deccheckResamount);
                        //        isValid = false;
                        //    }
                        //}
                    }
                }
            }
            return (isValid);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Pb_Ceap_Prog_Click(object sender, EventArgs e)
        {
            string ACR_SerPlan_HIE = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                ACR_SerPlan_HIE = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.ToString();
            }

            string ProgCA = string.Empty;
            if (!string.IsNullOrEmpty(txtCeapProgram.Text.Trim())) ProgCA = txtCeapProgram.Text.Substring(0, 6);
            string Sel_Prog = ProgCA;
            string Sel_SerPlan = propdr["CASEACT_SERVICE_PLAN"].ToString();
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, Sel_SerPlan, ACR_SerPlan_HIE);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                Sel_CAMS_Program = form.Selected_SerPlan_Prog();
                txtCeapProgram.Text = Sel_CAMS_Program;

            }
        }

        private void pnlCA_Click(object sender, EventArgs e)
        {

        }

        public List<CommonEntity> ListcommonEntity { get; set; }
        List<CommonEntity> BillPeriodEntity = new List<CommonEntity>();
        private void btnBillperiod_Click(object sender, EventArgs e)
        {

            string BundleEnable = string.Empty;
            BundleEnable = "N";
            SelectZipSiteCountyForm countyform = new SelectZipSiteCountyForm(BaseForm, ListcommonEntity, "BillingPeriod", BundleEnable, string.Empty, string.Empty);
            countyform.FormClosed += new FormClosedEventHandler(SelectBillFormClosed);
            countyform.StartPosition = FormStartPosition.CenterScreen;
            countyform.ShowDialog();
        }

        private void SelectBillFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.FormType == "Billing")
                {
                    ListcommonEntity = form.SelectedCountyEntity;
                    if (ListcommonEntity.Count > 0)
                    {
                        string County = string.Empty; string BillPeriodDesc = string.Empty;
                        foreach (CommonEntity Entity in ListcommonEntity)
                        {
                            County += Entity.Code.Trim() + ",";
                            BillPeriodDesc += Entity.Desc.Trim() + ", ";
                        }
                        txtBillperiodcode.Text = County.Substring(0, County.Length - 1);
                        txtBillPeriod.Text = BillPeriodDesc.Substring(0, BillPeriodDesc.Length - 2);
                    }
                }


            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, "**");
            Vendor_Browse.FormClosed += new FormClosedEventHandler(On_VendorErap_Browse_Closed);
            Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
            Vendor_Browse.ShowDialog();
        }

        private void On_VendorErap_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();

                txtErapVendor.Text = Vendor_Details[0].Trim();
                lblErapVendorName.Text = Vendor_Details[1].Trim();
            }
        }

        private void picErapProgram_Click(object sender, EventArgs e)
        {
            string ACR_SerPlan_HIE = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                ACR_SerPlan_HIE = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.ToString();
            }

            string ProgCA = string.Empty;
            if (!string.IsNullOrEmpty(txtErapprogram.Text.Trim())) ProgCA = txtErapprogram.Text.Substring(0, 6);
            string Sel_Prog = ProgCA;
            string Sel_SerPlan = propdr["CASEACT_SERVICE_PLAN"].ToString();
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, Sel_SerPlan, ACR_SerPlan_HIE);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieErapFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }


        private void OnHierarchieErapFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                Sel_CAMS_Program = form.Selected_SerPlan_Prog();
                txtErapprogram.Text = Sel_CAMS_Program;

            }
        }



        List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = new List<PMTFLDCNTLHEntity>();
        private void EnableDisableControls()
        {


            if (propPMTFLDCNTLHEntity.Count > 0)
            {
                foreach (PMTFLDCNTLHEntity Entity in propPMTFLDCNTLHEntity)
                {
                    bool required = Entity.PMFLDH_REQUIRED.Equals("Y") ? true : false;
                    bool enabled = Entity.PMFLDH_ENABLED.Equals("Y") ? true : false;

                    if (propProgramdefinationdata.DepSerpostPAYCAT.Trim() == "02")
                    {
                        switch (Entity.PMFLDH_CODE)
                        {
                            case Consts.CASE0063.VendorNo:
                                if (enabled) { pbErapVendor.Enabled = lblErapVendor.Enabled = pnlErapVendor.Visible = true; if (required) lblErapVendorReq.Visible = true; } else { lblVendor.Enabled = pnlErapVendor.Visible = false; lblErapVendorReq.Visible = false; }
                                //if (enabled)
                                //{
                                //    if (SerVendPrivileges != null)
                                //    {
                                //        if (SerVendPrivileges.ViewPriv.ToUpper() == "TRUE")
                                //        {
                                //            pnlCat2_Vendor.Visible = true;
                                //        }
                                //        else
                                //        {
                                //            pnlCat2_Vendor.Visible = false;
                                //        }
                                //    }
                                //    else { pnlCat2_Vendor.Visible = false; }
                                //}
                                break;

                            case Consts.CASE0063.ActCaseWorker:
                                if (enabled) { CmbWorker.Enabled = lblCaseworca.Enabled = true; if (required) lblCaseworcaReq.Visible = true; } else { CmbWorker.Enabled = false; lblCaseworcaReq.Visible = false; }
                                break;
                            case Consts.CASE0063.Site:
                                if (enabled) { CmbSite.Enabled = lblSiteca.Enabled = true; if (required) lblSitecaReq.Visible = true; } else { CmbSite.Enabled = false; lblSitecaReq.Visible = false; }
                                break;


                            //case Consts.CASE0063.Act_Seek_Date:
                            //    if (enabled) { dtActSeek_Date.Visible = lblActSeekDate.Visible = true; if (required) lblActSeekDateReq.Visible = true; } else { dtActSeek_Date.Visible = lblActSeekDate.Visible = false; lblActSeekDateReq.Visible = false; }
                            //    break;


                            case Consts.CASE0063.TobeFollowUpBy:
                                if (enabled) { txtErapby.Enabled = lblErapby.Enabled = true; if (required) lblErapByReq.Visible = true; } else { txtErapby.Enabled = false; lblErapByReq.Visible = false; }
                                break;
                            case Consts.CASE0063.FollowUPOn:
                                if (enabled) { dtErapFon.Enabled = lblErapOn.Enabled = true; if (required) lblErapOnReq.Visible = true; } else { dtErapFon.Enabled = false; lblErapOnReq.Visible = false; }
                                break;
                            case Consts.CASE0063.FollowUpComplete:
                                if (enabled) { dtErapComplete.Enabled = lblErapComplete.Enabled = true; if (required) lblErapCompReq.Visible = true; } else { dtErapComplete.Enabled = false; lblErapCompReq.Visible = false; }
                                break;

                            case Consts.CASE0063.Act_Acty_Program:
                                if (enabled) { pnlErapProgram.Visible = lblErapProgram.Enabled = true; if (required) lblErapProgramReq.Visible = true; } else { pnlErapProgram.Visible = lblErapProgramReq.Visible = false; }
                                break;


                            case Consts.CASE0063.BillingPeriod:
                                if (enabled) { lblErapBillPeriod.Enabled = btnBillperiod.Enabled = btnBillperiod.Visible = true; if (required) lblErapBillpReq.Visible = true; } else { btnBillperiod.Visible = false; lblErapBillpReq.Visible = false; }
                                break;
                            case Consts.CASE0063.Account:
                                if (enabled) { lblErapAcct.Enabled = txtErapAccount.Enabled = true; if (required) lblErapAccReq.Visible = true; } else { txtErapAccount.Enabled = false; lblErapAccReq.Visible = false; }
                                break;
                            case Consts.CASE0063.ArrearsAmount:
                                if (enabled) { txtErapArrea.Enabled = lblErapArreas.Enabled = true; if (required) lblErapArreaAmtReq.Visible = true; } else { txtErapArrea.Enabled = false; lblErapArreaAmtReq.Visible = false; }
                                break;
                            case Consts.CASE0063.AmountPaid:
                                if (enabled) { txtErapAmtpaid.Enabled = lblErapAmtpaid.Enabled = true; if (required) lblErapAmtReq.Visible = true; } else { txtErapAmtpaid.Enabled = false; lblErapAmtReq.Visible = false; }
                                break;
                            case Consts.CASE0063.FundingSource1:
                                if (enabled) { lblErapFund.Enabled = cmbErapFund.Enabled = true; if (required) lblErapFundReq.Visible = true; } else { cmbErapFund.Enabled = false; lblErapFundReq.Visible = false; }
                                break;
                                //case Consts.CASE0063.CWApproval:
                                //    if (enabled) { txtCat2LVL1Apprv.Enabled = lblcat2LVL1Apprv.Enabled = true; if (required) lblCWApprvReq.Visible = true; } else { txtCat2LVL1Apprv.Enabled = lblcat2LVL1Apprv.Enabled = false; lblCWApprvReq.Visible = false; }
                                //    break;
                                //case Consts.CASE0063.CWApprovalDate:
                                //    if (enabled) { dtpCat2CWApprvDate.Enabled = lblCat2CWAprDate.Enabled = true; if (required) lblCat2CWApprvDateReq.Visible = true; } else { dtpCat2CWApprvDate.Enabled = lblCat2CWAprDate.Enabled = false; lblCat2CWApprvDateReq.Visible = false; }
                                //    break;
                                //case Consts.CASE0063.SupervisorApproval:
                                //    if (enabled) { txtCat2LVL2Apprv.Enabled = lblCat2LVL2Apprv.Enabled= lblCat2SupvApprReq.Enabled = true; if (required) lblCat2SuprApprvDateReq.Visible = true; } else { txtCat2LVL2Apprv.Enabled = lblCat2LVL2Apprv.Enabled = false; lblCat2SupvApprReq.Visible = false; }
                                //    break;
                                //case Consts.CASE0063.SupervisorApprovalDate:
                                //    if (enabled) { dtpCat2SupvaprvDate.Enabled = lblCat2SupvaprvDate.Enabled = true; if (required) lblCat2SupvApprReq.Visible = true; } else { dtpCat2SupvaprvDate.Enabled = lblCat2SupvaprvDate.Enabled = false; lblCat2SupvApprReq.Visible = false; }
                                //    break;
                                //case Consts.CASE0063.SentforPaymentbyUser:
                                //    if (enabled) { txtCat2SentUser.Enabled = lblCat2SentUser.Enabled = true; if (required) lblSentUserReq.Visible = true; } else { txtCat2SentUser.Enabled = lblCat2SentUser.Enabled = false; lblSentUserReq.Visible = false; }
                                //    break;
                                //case Consts.CASE0063.SentOn:
                                //    if (enabled) { dtpCat2Sent.Enabled = lblCat2SentOn.Enabled = true; if (required) lblCat2SentdateReq.Visible = true; } else { dtpCat2Sent.Enabled = lblCat2SentOn.Enabled = false; lblCat2SentdateReq.Visible = false; }
                                //    break;
                                //case Consts.CASE0063.Bundle:
                                //    if (enabled) { txtcat2Bundle.Enabled = lblCat2Bundle.Enabled = true; if (required) lblCat2BundleReq.Visible = true; } else { txtcat2Bundle.Enabled = lblCat2Bundle.Enabled = false; lblCat2BundleReq.Visible = false; }
                                //    break;

                        }
                    }
                    else if (propProgramdefinationdata.DepSerpostPAYCAT == "04")
                    {
                        switch (Entity.PMFLDH_CODE)
                        {
                            case Consts.CASE0063.VendorNo:
                                if (enabled) { PbVendor.Enabled = lblVendor.Enabled = panel_Referral2.Visible = true; if (required) lblVendorReq.Visible = true; } else { lblVendor.Enabled = PbVendor.Enabled = panel_Referral2.Visible = false; lblVendorReq.Visible = false; }
                                //ADDED BY SUDHEER ON BRian's document on 05/20/23 (NCCAA.docx)
                               //Coomented by Sudheer on 02/05/24 based on the CEAP Enhancement document
                                //lblVendor.Enabled = PbVendor.Enabled = panel_Referral2.Visible = false; lblVendorReq.Visible = false;
                                
                                //if (enabled)
                                //{
                                //    if (SerVendPrivileges != null)
                                //    {
                                //        if (SerVendPrivileges.ViewPriv.ToUpper() == "TRUE")
                                //        {
                                //            pnlCat4_Vendor.Visible = true;
                                //        }
                                //        else
                                //        {
                                //            pnlCat4_Vendor.Visible = false;
                                //        }
                                //    }
                                //    else { pnlCat4_Vendor.Visible = false; }
                                //}
                                break;
                            case Consts.CASE0063.Source:
                                if (enabled) { cmbCat4Source.Enabled = lblCat4Source.Enabled = true; if (required) lblCat4SourceReq.Visible = true; } else { cmbCat4Source.Enabled = false; lblCat4SourceReq.Visible = false; }
                                break;
                            case Consts.CASE0063.ActCaseWorker:
                                if (enabled) { CmbWorker.Enabled = lblCaseworca.Enabled = true; if (required) lblCaseworcaReq.Visible = true; } else { CmbWorker.Enabled = false; lblCaseworcaReq.Visible = false; }
                                break;
                            case Consts.CASE0063.Site:
                                if (enabled) { CmbSite.Enabled = lblSiteca.Enabled = true; if (required) lblSitecaReq.Visible = true; } else { CmbSite.Enabled = false; lblSitecaReq.Visible = false; }
                                break;

                            case Consts.CASE0063.TobeFollowUpBy:
                                if (enabled) { txttobeFollowUp.Enabled = lblTobeFolled.Enabled = true; if (required) lblTobeFolledReq.Visible = true; } else { txttobeFollowUp.Enabled = false; lblTobeFolledReq.Visible = false; }
                                break;
                            case Consts.CASE0063.FollowUPOn:
                                if (enabled) { dtupOn.Enabled = lblFollowup.Enabled = true; if (required) lblFollowupReq.Visible = true; } else { dtupOn.Enabled = lblFollowupReq.Visible = false; }
                                break;
                            case Consts.CASE0063.FollowUpComplete:
                                if (enabled) { dtComplet.Enabled = lblFollowupCompl.Enabled = true; if (required) lblFollowupComplReq.Visible = true; } else { dtComplet.Enabled = false; lblFollowupComplReq.Visible = false; }
                                break;

                            case Consts.CASE0063.Act_Acty_Program:
                                if (enabled) { Pb_Ceap_Prog.Visible = LblProgram.Enabled = true; if (required) lblProgramReq.Visible = true; } else { Pb_Ceap_Prog.Visible = Pb_Ceap_Prog.Visible = false; lblProgramReq.Visible = false; }
                                break;

                            case Consts.CASE0063.Act_Seek_Date:
                                if (enabled) { dtActSeek_Date.Enabled = dtActSeek_Date.Visible = lblActSeekDate.Enabled = lblActSeekDate.Visible = true; if (required) lblActSeekDateReq.Visible = true; } else { dtActSeek_Date.Visible = lblActSeekDate.Visible = false; lblActSeekDateReq.Visible = false; }
                                break;

                            //case Consts.CASE0063.ActivityDate:
                            //    if (enabled) { dtAct_Date.Visible =  lblActivityDate.Visible = true; if (required) lblActivityDateReq.Visible = true; } else { dtAct_Date.Visible = lblActivityDate.Enabled = false; lblActivityDateReq.Visible = false; }
                            //    break;


                            case Consts.CASE0063.BillingType:
                                if (enabled) { cmbBilling.Enabled = true; if (required) lblBillNameReq.Visible = true; } else { cmbBilling.Enabled = false; lblBillNameReq.Visible = false; }
                                break;
                            case Consts.CASE0063.Account:
                                if (enabled) { lblCeapAccount.Enabled = txtAccountNo.Enabled = true; if (required) lblAcctReq.Visible = true; } else { txtAccountNo.Enabled = false; lblAcctReq.Visible = false; }
                                break;

                            case Consts.CASE0063.AmountPaid:
                                if (enabled) { txtAmountPaid.Enabled = lblCeapAmtpaid.Enabled = true; if (required) lblCeapAmtReq.Visible = true; } else { txtAmountPaid.Enabled = false; lblCeapAmtReq.Visible = false; }
                                break;
                            case Consts.CASE0063.FundingSource1:
                                if (enabled) { lblCEAPFund.Enabled = lblBudget1.Enabled = cmbBudget.Enabled = cmbFUnd.Enabled = true; if (required) lblFundReq.Visible = true; lblBudgetReq.Visible = true; } else { cmbFUnd.Enabled = cmbBudget.Enabled = false; lblFundReq.Visible = false; lblBudgetReq.Visible = false; }
                                break;
                            case Consts.CASE0063.BenefitReason:
                                if (enabled) { cmbBenfitReason4.Enabled = lblBenefitReason4.Enabled = true; if (required) lblReqBenfitReason4.Visible = true; } else { cmbBenfitReason4.Enabled = false; lblReqBenfitReason4.Visible = false; }
                                break;
                                //case Consts.CASE0063.FundingSource1:
                                //    if (enabled) { lblCEAPFund.Enabled = lblBudget1.Enabled = cmbBudget.Enabled = cmbFUnd.Enabled = true; if (required) lblFundReq.Visible = true; lblBudgetReq.Visible = true; } else { lblBudget1.Enabled = cmbBudget.Enabled = cmbFUnd.Enabled = false; lblFundReq.Visible = false; lblBudgetReq.Visible = false; }
                                //    break;


                        }
                    }

                }


            }
        }

        private void Fill_Sources()
        {
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
            cmbCat4Source.Items.AddRange(listItem.ToArray());
            cmbCat4Source.SelectedIndex = 0;
        }

        private void cmbFUnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFUnd.Items.Count > 0)
            {

                string strcmbFundsource = ((Utilities.ListItem)cmbFUnd.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbFUnd.SelectedItem).Value.ToString();
                if (!string.IsNullOrEmpty(strcmbFundsource))
                {
                    List<CMBDCEntity> cmbdcdata = CMbdc_List.FindAll(u => u.BDC_FUND.ToString().Trim() == strcmbFundsource.Trim() && u.BDC_ALLOW_POSTING=="Y");
                    if (cmbdcdata.Count > 0)
                    {
                        cmbdcdata = cmbdcdata.FindAll(u => (Convert.ToDateTime(u.BDC_START)) <= Convert.ToDateTime(dtAct_Date.Text.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(dtAct_Date.Text.Trim()));
                    }
                    fillBudgets(cmbdcdata,"");
                    if (cmbdcdata.Count > 0)
                    {
                        if (txtSerFund.Text.Trim() != strcmbFundsource.Trim() && (strcmbFundsource != "0"))
                        {
                            txtSerAmt.Enabled = false;
                        }
                        else
                        {
                            if (propCEAPCNTLSwitch == "Y")
                            {
                                txtSerAmt.Enabled = true;
                            }
                        }
                    }
                }

            }
        }


        private void cmbBudget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBudget.Items.Count > 0)
            {
                string strBudgetId = ((Utilities.ListItem)cmbBudget.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();

                if (((Utilities.ListItem)cmbBudget.SelectedItem).ID.ToString() == "N")
                    AlertBox.Show("Posting are not allowed to this Budget "+ ((Utilities.ListItem)cmbBudget.SelectedItem).Text.ToString());

                CMBDCEntity cmbdcdata = CMbdc_List.Find(u => u.BDC_ID.ToString().Trim() == strBudgetId.ToString().Trim());
                if (cmbdcdata != null)
                {
                    lblBudgetdesc2.Text = cmbdcdata.BDC_DESCRIPTION;
                    decimal decbudget = cmbdcdata.BDC_BUDGET.ToString() == string.Empty ? 0 : Convert.ToDecimal(cmbdcdata.BDC_BUDGET);
                    lblBudget1.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decbudget);
                    lblStart1.Text = LookupDataAccess.Getdate(cmbdcdata.BDC_START.ToString());
                    lblEnd1.Text = LookupDataAccess.Getdate(cmbdcdata.BDC_END.ToString());
                    decimal decbudbalance = cmbdcdata.BDC_BALANCE.ToString() == string.Empty ? 0 : Convert.ToDecimal(cmbdcdata.BDC_BALANCE);
                    lblBudgetBalance.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", decbudbalance); ;
                }
                else
                {
                    lblBudgetdesc2.Text = string.Empty;
                    lblBudget1.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", 0);
                    lblStart1.Text = string.Empty;
                    lblEnd1.Text = string.Empty;
                    lblBudgetBalance.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", 0); ;
                }
            }
        }

        private void dtAct_Date_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}