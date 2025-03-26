using Aspose.Cells;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using DevExpress.DataAccess.DataFederation;
using DevExpress.DataAccess.Native.Data;
using DevExpress.ExpressApp.Utils;
using DevExpress.Office.Utils;
using DevExpress.Utils.Extensions;
using DevExpress.Xpo.DB;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraRichEdit.Model.History;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using Wisej.Web;
using System.Data;

namespace Captain.Common.Views.Forms
{
    public partial class CASE1016_Usage : Form
    {

        #region Private Variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public CASE1016_Usage(BaseForm baseForm, string mode, string sp_code, string sp_sequence, string spm_year, string SPName, string strPrimSource, string strType, string strOthersource, string strelec,string strother, CASESPMEntity spmentity, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _baseForm = baseForm;
            _privilegeEntity = privilegeEntity;

            _Mode = mode;
            _spCode = sp_code;
            _spSequence = sp_sequence;
            _spmYear = spm_year;
            _spmName = SPName;
            _strPrimSource = strPrimSource;
            _strType = strType;
            _strOtherSource = strOthersource;
            _strElec = strelec;
            _strOther = strother;
            _spmentity = spmentity;

            propReportPath = _model.lookupDataAccess.GetReportPath();

            this.Size = new Size(925, 737);

            txtAward.Validator = TextBoxValidation.FloatValidator;
            txtBalance.Validator = TextBoxValidation.FloatValidator;
            txtTotPaid.Validator = TextBoxValidation.FloatValidator;
            txtPArrInv.Validator = TextBoxValidation.FloatValidator;
            txtSArrInv.Validator = TextBoxValidation.FloatValidator;

            PostUsagePriv = new PrivilegeEntity();
            List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(_privilegeEntity.ModuleCode.Trim(), _baseForm.UserID, _baseForm.BaseAgency + _baseForm.BaseDept + _baseForm.BaseProg);
            if (userPrivilege.Count > 0)
            {
                PostUsagePriv = userPrivilege.Find(u => u.Program == "CASE0016");
                if (PostUsagePriv != null)
                {
                   
                }
            }

            if (!string.IsNullOrEmpty(_baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (_baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }

            propSearch_Entity = _model.SPAdminData.Browse_CASESP0List(null, null, null, null, null, null, null, null, null);
            propfundingsource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, string.Empty);
            ALLEmsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, string.Empty);

            if (!string.IsNullOrEmpty(_baseForm.BaseYear.Trim()))
                CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, _baseForm.BaseYear, string.Empty, string.Empty);

            SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(_spCode, null, null, null, null, null, null, null, null);

            Get_Vendor_List();

            Vulner_Flag = Get_SNP_Vulnerable_Status();

            if (!string.IsNullOrEmpty(_baseForm.BaseCaseMstListEntity[0].Rank1.Trim()))
                txtScore.Text = _baseForm.BaseCaseMstListEntity[0].Rank1.Trim();
            else
                txtScore.Text = "0";

            if (CEAPCNTL_List.Count > 0)
            {
                if (CEAPCNTL_List[0].CPCT_INV_METHOD == "2")
                {
                    if (Vulner_Flag)
                    {
                        ServiceCode = CEAPCNTL_List[0].CPCT_VUL_PRIM_CA.Trim();
                    }
                    else if (!Vulner_Flag)
                    {
                        ServiceCode = CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA.Trim();
                    }

                    List<CEAPINVEntity> CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");
                    if (CEAPINVs.Count > 0)
                    {
                        CEAPINVEntity CEntity = CEAPINVs.Find(u => int.Parse(u.CPINV_LOW) <= int.Parse(txtScore.Text.Trim()) && int.Parse(u.CPINV_HIGH) >= int.Parse(txtScore.Text.Trim()));
                        if (CEntity != null)
                            txtMaxInv.Text = CEntity.CPINV_MAX_INV.ToString();
                        else
                            txtMaxInv.Text = "0";
                    }
                }
                else
                {
                    if (Vulner_Flag)
                    {
                        txtMaxInv.Text = CEAPCNTL_List[0].CPCT_VUL_MAX_INV;
                        ServiceCode = CEAPCNTL_List[0].CPCT_VUL_PRIM_CA.Trim();
                    }
                    else if (!Vulner_Flag)
                    {
                        txtMaxInv.Text = CEAPCNTL_List[0].CPCT_NONVUL_MAX_INV;
                        ServiceCode = CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA.Trim();
                    }
                }

                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM=="Y")
                //{
                //    if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL=="A") txtMaxInv.Text = "2";
                //    if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "S") txtMaxInv.Text = "4";
                //    if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "Q") txtMaxInv.Text = "8";
                //}


            }

            txtServPlan.Text = SPName.Trim();
            

            if (!string.IsNullOrEmpty(_spmentity.SPM_Vendor))
                txtPArrVName.Text = Get_Vendor_Name(_spmentity.SPM_Vendor);
            if (!string.IsNullOrEmpty(_spmentity.SPM_Gas_Vendor))
                txtSArrVName.Text = Get_Vendor_Name(_spmentity.SPM_Gas_Vendor);

            Fill_Applicant_SPs();

            if (SPM_Entity != null)
            {
                txtAward.Text = SPM_Entity.SPM_Amount;
                txtBalance.Text = SPM_Entity.SPM_Balance;

                decimal PaidAmt = 0;
                PaidAmt = Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Amount.Trim()) ? SPM_Entity.SPM_Amount.Trim() : "0") - Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Balance.Trim()) ? SPM_Entity.SPM_Balance.Trim() : "0");

                txtTotPaid.Text = PaidAmt.ToString();

                dtpPayProcDte.Value = Convert.ToDateTime(_spmentity.startdate);
            }

            FillLumpSumCombo();

            if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y")
            {
                if (CEAPCNTL_List[0].CPCT_USER_CNTL == "Y")
                    cmbLumpsum.Enabled = true;
                else
                    cmbLumpsum.Enabled = false;

                pnlVendCounts.Visible = false;

                this.Size = new Size(925, 743);
            }

            if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
            {
                this.dgvMonthsGrid.Columns["gvAmtPaid"].ReadOnly = true;
                lblLump.Visible = false; cmbLumpsum.Visible = false;//txtLumpInterval.Visible = false;
            }

            FillServiceTextbox();

            FillMonthsGrid();

            FillInvPostingGrid();
        }
        List<CASEACTEntity> SelListEntity = new List<CASEACTEntity>();
        private void FillLumpSumCombo()
        {
            cmbLumpsum.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();

            if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "A")
                listItem.Add(new ListItem("Annual", "A"));
            if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "S")
                listItem.Add(new ListItem("Semi-Annual", "S"));
            if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "Q")
                listItem.Add(new ListItem("Quarterly", "Q"));

            if (CEAPCNTL_List[0].CPCT_USER_CNTL == "Y")
                listItem.Add(new ListItem("Monthly", "M"));

            cmbLumpsum.Items.AddRange(listItem.ToArray());

            
        }

        string BlankImg = Consts.Icons.ico_Blank;
        string DeleteImg = Consts.Icons.ico_Delete;
        string Img_Edit = Consts.Icons.ico_Edit;

        int RecCount = 0;
        List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
        List<CustomQuestionsEntity> custResponses2 = new List<CustomQuestionsEntity>();

        decimal ResAmount = 0; string ChkNo = string.Empty; string ChkDate = string.Empty; string ServsDate = string.Empty; string SeekDate = string.Empty;
        CASEACTEntity CaseactRec = new CASEACTEntity();
        private void FillMonthsGrid1()
        {
            dgvMonthsGrid.Rows.Clear();
            List<CustomQuestionsEntity> custmonthQuestions = new List<CustomQuestionsEntity>();

            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, string.Empty);
            List <CustomQuestionsEntity> SelQues =custResponses.ToList();
            if (custResponses.Count > 0)
            {
                custResponses = custResponses.FindAll(u => u.USAGE_MONTH == "CUR");
                if (custResponses.Count == 0 && CEAPCNTL_List[0].CPCT_LUMP_SUM=="Y")
                {
                    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                    custEntity.ACTAGENCY = _baseForm.BaseAgency;
                    custEntity.ACTDEPT = _baseForm.BaseDept;
                    custEntity.ACTPROGRAM = _baseForm.BaseProg;
                    if (_baseForm.BaseYear == string.Empty)
                        custEntity.ACTYEAR = "    ";
                    else
                        custEntity.ACTYEAR = _baseForm.BaseYear;
                    custEntity.ACTAPPNO = _baseForm.BaseApplicationNo;

                    custEntity.USAGE_MONTH = "CUR";
                    custEntity.USAGE_PMON_SW = "N";
                    custEntity.USAGE_SMON_SW = "N";
                    if (SelQues[0].USAGE_PMON_SW == "Y")
                    {
                        custEntity.USAGE_PMON_SW = "Y";
                    }
                    if (SelQues[0].USAGE_SMON_SW == "Y")
                    {
                        custEntity.USAGE_SMON_SW = "Y";
                    }
                    custEntity.USAGE_PRIM_12HIST = "N"; custEntity.USAGE_SEC_12HIST = "N";
                    if (SelQues[0].USAGE_PRIM_12HIST == "Y") custEntity.USAGE_PRIM_12HIST = "Y";
                    if (SelQues[0].USAGE_SEC_12HIST == "Y") custEntity.USAGE_SEC_12HIST = "Y";

                    custEntity.USAGE_PRIM_PAYMENT = "0.00";
                    custEntity.USAGE_PRIM_USAGE = "0.00";
                    custEntity.USAGE_SEC_PAYMENT = "0.00";
                    custEntity.USAGE_SEC_USAGE = "0.00";
                    custEntity.USAGE_TOTAL = "0.00";
                    custEntity.USAGE_SSOURCE = string.Empty;
                    custEntity.USAGE_PRIM_VEND = string.Empty;
                    custEntity.USAGE_SEC_VEND = string.Empty;
                    custEntity.addoperator = _baseForm.UserID;
                    custEntity.lstcoperator = _baseForm.UserID;
                    custEntity.Mode = "";

                    bool chk = _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);


                }
            }

            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, string.Empty);
            custResponses2 = _model.CaseMstData.CAPS_CASEUSAGE_GET(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, string.Empty);

            string custCode = string.Empty;

            RecCount = 0;
            int rowIndex;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            
            List<CustomQuestionsEntity> CustRespElec = new List<CustomQuestionsEntity>();
            List<CustomQuestionsEntity> CustRespOther = new List<CustomQuestionsEntity>();

            foreach (CustomQuestionsEntity Entity in custResponses)
            {
                if (string.IsNullOrEmpty(Entity.USAGE_PRIM_PAYMENT.Trim()))
                    Entity.USAGE_PRIM_PAYMENT = "0.00";
                //else
                //    CustRespElec.Add(Entity);
                if (string.IsNullOrEmpty(Entity.USAGE_SEC_PAYMENT.Trim()))
                    Entity.USAGE_SEC_PAYMENT = "0.00";
                else
                {
                    //string PrimAmt= Entity.USAGE_SEC_PAYMENT.Trim();
                    //CustomQuestionsEntity CstEnt = Entity;
                    //CstEnt.USAGE_PRIM_PAYMENT = PrimAmt;
                    //CustRespOther.Add(Entity);
                }
            }

            CustRespElec = custResponses2.FindAll(u => Convert.ToDecimal(u.USAGE_PRIM_PAYMENT.Trim()) != 0 || u.USAGE_MONTH=="CUR");

            if (!string.IsNullOrEmpty(_spmentity.SPM_Gas_Vendor.Trim()) && !string.IsNullOrEmpty(_spmentity.SPM_Gas_Account.Trim()) && !string.IsNullOrEmpty(_spmentity.SPM_Gas_BillName_Type.Trim()))
                    CustRespOther = custResponses.FindAll(u => Convert.ToDecimal(u.USAGE_SEC_PAYMENT.Trim()) != 0 || u.USAGE_MONTH == "CUR");

            foreach (CustomQuestionsEntity REntity in CustRespOther)
            {
                if (string.IsNullOrEmpty(REntity.USAGE_SEC_PAYMENT.Trim()))
                {
                    REntity.USAGE_SEC_PAYMENT = "0.00";
                    REntity.USAGE_PRIM_PAYMENT = "0.00";
                }
                else
                    REntity.USAGE_PRIM_PAYMENT = REntity.USAGE_SEC_PAYMENT.Trim();

                REntity.CUSTOTHER = "O";

            }

            List<CustomQuestionsEntity> customQuestions= new List<CustomQuestionsEntity>();
            if(CustRespElec.Count>0)
                customQuestions.AddRange(CustRespElec);
            if (CustRespOther.Count > 0)
                customQuestions.AddRange(CustRespOther);


            if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
                customQuestions = customQuestions.FindAll(u => u.USAGE_MONTH != "CUR");

            customQuestions = customQuestions.OrderByDescending(u => Convert.ToDecimal(u.USAGE_PRIM_PAYMENT.Trim())).ToList();

            int k = 0;
            if (customQuestions.Count > 0)
            {
                foreach (CustomQuestionsEntity Entity in customQuestions)
                {
                    string MonthName = string.Empty;
                    MonthName = GetMonthName(Entity.USAGE_MONTH.Trim());
                   

                    string UsageAmount = string.Empty;

                    if (Entity.USAGE_MONTH != "TOT")
                    {
                        k++;

                        string Iscaseact = "N";
                        string BundleNo = string.Empty;
                        string PDOut = "N";
                        string VendName = string.Empty;
                        string VendNumber = string.Empty;
                        string VendType = string.Empty;

                        if (Entity.CUSTOTHER=="O")//(Entity.USAGE_PRIM_PAYMENT == Entity.USAGE_SEC_PAYMENT)
                        {
                            VendName = Get_Vendor_Name(SPM_Entity.SPM_Gas_Vendor);
                            VendNumber = SPM_Entity.SPM_Gas_Vendor;
                            VendType = "S";
                            _strElec = "O";
                        }
                        else
                        {
                            VendName = Get_Vendor_Name(SPM_Entity.SPM_Vendor);
                            VendNumber = SPM_Entity.SPM_Vendor;
                            VendType = "P";
                            _strElec = "E";
                        }

                        if (_strElec == "E")
                            UsageAmount = Entity.USAGE_PRIM_PAYMENT.Trim();
                        else
                            UsageAmount = Entity.USAGE_SEC_PAYMENT.Trim();

                        if (UsageAmount == "0.00")
                        {
                            CustomQuestionsEntity REnt = custResponses.Find(u => u.USAGE_MONTH == Entity.USAGE_MONTH);
                            if (REnt != null)
                            {
                                if (_strElec == "E")
                                    UsageAmount = REnt.USAGE_PRIM_PAYMENT.Trim();
                                else
                                    UsageAmount = REnt.USAGE_SEC_PAYMENT.Trim();
                            }
                        }

                        if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y")
                        {
                            if (_strElec == "E")
                                ResAmount = Entity.USAGE_LUMP_PRIM.Trim() == "" ? 0 : Convert.ToDecimal(Entity.USAGE_LUMP_PRIM.Trim());
                            else
                                ResAmount = Entity.USAGE_LUMP_SEC.Trim() == "" ? 0 : Convert.ToDecimal(Entity.USAGE_LUMP_SEC.Trim());

                            //ServsDate = DateTime.Now.ToShortDateString();

                            if(ResAmount>0)
                                Iscaseact = "Y";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(txtService.Text))
                                InvoiceDetails(Entity.USAGE_MONTH.Trim());
                        }

                        if (CaseactRec != null)
                        {
                            if (!string.IsNullOrEmpty(CaseactRec.App_no.Trim()))
                            {
                                Iscaseact = "Y";
                                BundleNo = CaseactRec.BundleNo.Trim();
                                PDOut = CaseactRec.PDOUT.Trim();
                            }
                        }


                        rowIndex = dgvMonthsGrid.Rows.Add(Entity.USAGE_MONTH, MonthName, k, VendName, VendType, UsageAmount, SeekDate.Trim() == "" ? false : true, ResAmount == 0 ? "" : ResAmount.ToString(), BlankImg, VendNumber, LookupDataAccess.Getdate(SeekDate.Trim()), LookupDataAccess.Getdate(ServsDate.Trim()), BundleNo, ChkNo, LookupDataAccess.Getdate(ChkDate.Trim()), Iscaseact, PDOut, BlankImg, BlankImg, _strElec);

                        if (!string.IsNullOrEmpty(ServsDate.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.ChangePriv.ToUpper() == "TRUE")
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].ReadOnly = false;
                                    if (!string.IsNullOrEmpty(BundleNo.Trim()))
                                        dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].Value = BlankImg;
                                    else
                                        dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                }
                                else
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].ReadOnly = true;
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].Value = BlankImg;
                                }
                            }
                            dgvMonthsGrid.Rows[rowIndex].Cells["gvSelect"].ReadOnly = true;
                        }
                        dgvMonthsGrid.Rows[rowIndex].Cells["gvtServicedate"].ReadOnly = true;

                        
                        if (!string.IsNullOrEmpty(ServsDate.Trim()) && string.IsNullOrEmpty(ChkDate.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].ReadOnly = false;
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].Value = DeleteImg;
                                }
                                else
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].ReadOnly = true;
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].Value = BlankImg;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(BundleNo.Trim()))
                        {
                            dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].ReadOnly = true;
                            dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].Value = BlankImg;
                        }

                        if (Iscaseact == "Y")
                        {
                             dgvMonthsGrid.Rows[rowIndex].Tag = CaseactRec;
                            dgvMonthsGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                            if (CEAPCNTL_List[0].CPCT_LUMP_SUM!="Y")
                                set_CAMS_Tooltip(rowIndex, CaseactRec.Add_Date, CaseactRec.Add_Operator, CaseactRec.Lstc_Date, CaseactRec.Lsct_Operator);
                        }
                    }
                }
            }

            if (dgvMonthsGrid.Rows.Count > 0)
            {
                foreach (DataGridViewRow _dRow in dgvMonthsGrid.Rows)
                {
                    if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y" && _dRow.Cells["gvMonthsCode"].Value.ToString().Trim() == "CUR") { }
                    else
                    {
                        string IntakeDate = _spmentity.startdate.Trim(); //_baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                        string Month = GetMonth(_dRow.Cells["gvMonthsCode"].Value.ToString().Trim());
                        string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                        int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                        if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                        {
                            this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                            //string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            _dRow.Cells["gvtUsageReq"].Value = StrUsageDate;
                            if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                                _dRow.Cells["gvtServicedate"].Value = StrDate;

                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                        }
                        else if (Convert.ToDateTime(IntakeDate.Trim()).Month == Convert.ToDateTime(StrDate.Trim()).Month)
                        {
                            this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                            //string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            _dRow.Cells["gvtUsageReq"].Value = StrUsageDate;
                            if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                                _dRow.Cells["gvtServicedate"].Value = IntakeDate;

                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                        }
                        else
                        {
                            this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();

                            _dRow.Cells["gvtUsageReq"].Value = StrDate;

                            if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                                _dRow.Cells["gvtServicedate"].Value = StrUsageDate;

                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                        }
                        if (txtServCode.Text.Trim() == ServiceCode)
                        {
                                if (Convert.ToDateTime(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim())
                                && Convert.ToDateTime(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                                {
                                    this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                                    if (_dRow.Cells["gvSelect"].ReadOnly != true)
                                    {
                                        _dRow.Cells["gvtUsageReq"].Value = "";
                                        _dRow.Cells["gvtServicedate"].Value = "";
                                    }
                                    this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                                }
                                else
                                {
                                    _dRow.Cells.ForEach(y => y.ReadOnly = true);
                                    _dRow.Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                                    _dRow.Cells[1].Style.ForeColor = Color.OrangeRed;

                                    string strMsg = "The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim());
                                    _dRow.Cells.ForEach(y => y.ToolTipText = strMsg);
                                }
                            }
                        }
                    if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y")
                    {
                        if (_dRow.Cells["gvAmtPaid"].Value.ToString() != string.Empty)
                        {
                            
                            _dRow.Cells["gvSelect"].Value = true;

                            //if (_dRow.Cells["gvMonthsCode"].Value.ToString().Trim() != "CUR")
                            //{
                                _dRow.Cells["gvSelect"].ReadOnly = true;
                                _dRow.Cells["gvAmtPaid"].ReadOnly = true;
                            //}
                            //_dRow.Cells["gvDelete"].Value = DeleteImg;
                            //_dRow.Cells["gvDelete"].ReadOnly = false;
                        }
                    }
                }

            }
            dgvMonthsGrid.Update();
        }
        private void FillInvPostingGrid()
        {
            dgvInvoicePosting.Rows.Clear();

            if (SP_Activity_Details.Count > 0)
            {
                List<CASEACTEntity> SelListEntity1 = SP_Activity_Details.FindAll(u => u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other!=string.Empty);
                int rowIndex = 0;
                if (SelListEntity1.Count > 0)
                {
                    foreach(CASEACTEntity entity in SelListEntity1)
                    {
                        string Adj = string.Empty;
                        if (custResponses2.Count > 0)
                        {
                            if (entity.Elec_Other == "E")
                            {
                                decimal TotFundAmt = 0;
                                List<CustomQuestionsEntity> SelQues = custResponses2.FindAll(u => u.USAGE_PRIM_ACTID == entity.ACT_ID);
                                if (SelQues.Count > 0)
                                {
                                    List<CustomQuestionsEntity> SelCaseact = SelQues.FindAll(u => u.USAGE_LUMP_PRIM.Trim() != "");
                                    if (SelCaseact.Count > 0)
                                    {
                                        TotFundAmt = SelCaseact.Sum(x => Convert.ToDecimal(x.USAGE_LUMP_PRIM.Trim()));
                                        if (TotFundAmt != Convert.ToDecimal(entity.Cost.Trim())) Adj = "Adj";

                                    }
                                }
                            }
                            if (entity.Elec_Other == "O")
                            {
                                decimal TotFundAmt = 0;
                                List<CustomQuestionsEntity> SelQues = custResponses2.FindAll(u => u.USAGE_SEC_ACTID == entity.ACT_ID);
                                if (SelQues.Count > 0)
                                {
                                    List<CustomQuestionsEntity> SelCaseact = SelQues.FindAll(u => u.USAGE_LUMP_SEC.Trim() != "");
                                    if (SelCaseact.Count > 0)
                                    {
                                        TotFundAmt = SelCaseact.Sum(x => Convert.ToDecimal(x.USAGE_LUMP_SEC.Trim()));
                                        if (TotFundAmt != Convert.ToDecimal(entity.Cost.Trim())) Adj = "Adj";

                                    }
                                }
                            }
                        }

                        rowIndex =dgvInvoicePosting.Rows.Add("Regular UA", LookupDataAccess.Getdate(entity.ACT_Date), entity.Elec_Other == "E" ? txtPArrVName.Text.Trim() : txtSArrVName.Text.Trim(), entity.Elec_Other == "E" ? "P" : "S", entity.Cost,Adj, Img_Edit, DeleteImg,entity.BundleNo,entity.Check_No,LookupDataAccess.Getdate(entity.Check_Date));

                        dgvInvoicePosting.Rows[rowIndex].Tag = entity;

                        if (!string.IsNullOrEmpty(entity.BundleNo.Trim()))
                        {
                            dgvInvoicePosting.Rows[rowIndex].Cells["gvInvDel"].ReadOnly = true; 
                            dgvInvoicePosting.Rows[rowIndex].Cells["gvInvDel"].Value = BlankImg;
                            dgvInvoicePosting.Rows[rowIndex].Cells["gvInvEdit"].ReadOnly = true;
                            dgvInvoicePosting.Rows[rowIndex].Cells["gvInvEdit"].Value = BlankImg; 
                        }

                    }
                    if (dgvInvoicePosting.Rows.Count > 0)
                    {
                        //txtRemTrans.Text = dgvInvoicePosting.Rows.Count.ToString();//CeapCnt.ToString();
                        //txtTotInv.Text = dgvInvoicePosting.Rows.Count.ToString();//CeapCnt.ToString();
                        //txtInvBal.Text = (Convert.ToDecimal(txtMaxInv.Text.Trim() == "" ? "0" : txtMaxInv.Text.Trim()) - Convert.ToDecimal(txtTotInv.Text.Trim())).ToString();

                        dgvInvoicePosting.Rows[0].Selected = true;
                        dgvInvoicePosting.CurrentCell = dgvInvoicePosting.SelectedRows[0].Cells[1];
                    }
                }
            }
        }

        bool IsPrimary = false;bool IsSecondary = false;
        private void FillMonthsGrid()
        {
            dgvMonthsGrid.Rows.Clear();
            List<CustomQuestionsEntity> custmonthQuestions = new List<CustomQuestionsEntity>();

            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, string.Empty);
            List<CustomQuestionsEntity> SelQues = custResponses.ToList();
            if (custResponses.Count > 0)
            {
                custResponses = custResponses.FindAll(u => u.USAGE_MONTH == "CUR");
                if (custResponses.Count == 0)  //&& CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y"
                {
                    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                    custEntity.ACTAGENCY = _baseForm.BaseAgency;
                    custEntity.ACTDEPT = _baseForm.BaseDept;
                    custEntity.ACTPROGRAM = _baseForm.BaseProg;
                    if (_baseForm.BaseYear == string.Empty)
                        custEntity.ACTYEAR = "    ";
                    else
                        custEntity.ACTYEAR = _baseForm.BaseYear;
                    custEntity.ACTAPPNO = _baseForm.BaseApplicationNo;

                    custEntity.USAGE_MONTH = "CUR";
                    custEntity.USAGE_PMON_SW = "N";
                    custEntity.USAGE_SMON_SW = "N";
                    if (SelQues[0].USAGE_PMON_SW == "Y")
                    {
                        custEntity.USAGE_PMON_SW = "Y";
                    }
                    if (SelQues[0].USAGE_SMON_SW == "Y")
                    {
                        custEntity.USAGE_SMON_SW = "Y";
                    }
                    custEntity.USAGE_PRIM_12HIST = "N"; custEntity.USAGE_SEC_12HIST = "N";
                    if (SelQues[0].USAGE_PRIM_12HIST == "Y") custEntity.USAGE_PRIM_12HIST = "Y";
                    if (SelQues[0].USAGE_SEC_12HIST == "Y") custEntity.USAGE_SEC_12HIST = "Y";

                    custEntity.USAGE_PRIM_PAYMENT = "0.00";
                    custEntity.USAGE_PRIM_USAGE = "0.00";
                    custEntity.USAGE_SEC_PAYMENT = "0.00";
                    custEntity.USAGE_SEC_USAGE = "0.00";
                    custEntity.USAGE_TOTAL = "0.00";
                    custEntity.USAGE_SSOURCE = string.Empty;
                    custEntity.USAGE_PRIM_VEND = string.Empty;
                    custEntity.USAGE_SEC_VEND = string.Empty;
                    custEntity.addoperator = _baseForm.UserID;
                    custEntity.lstcoperator = _baseForm.UserID;
                    custEntity.Mode = "";

                    bool chk = _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);


                }
            }

            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, string.Empty);
            custResponses2 = _model.CaseMstData.CAPS_CASEUSAGE_GET(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, string.Empty);

            string custCode = string.Empty;

            RecCount = 0;
            int rowIndex;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));

            List<CustomQuestionsEntity> CustRespElec = new List<CustomQuestionsEntity>();
            List<CustomQuestionsEntity> CustRespOther = new List<CustomQuestionsEntity>();

            foreach (CustomQuestionsEntity Entity in custResponses)
            {
                if (string.IsNullOrEmpty(Entity.USAGE_PRIM_PAYMENT.Trim()))
                    Entity.USAGE_PRIM_PAYMENT = "0.00";
                //else
                //    CustRespElec.Add(Entity);
                if (string.IsNullOrEmpty(Entity.USAGE_SEC_PAYMENT.Trim()))
                    Entity.USAGE_SEC_PAYMENT = "0.00";
                else
                {
                    //string PrimAmt= Entity.USAGE_SEC_PAYMENT.Trim();
                    //CustomQuestionsEntity CstEnt = Entity;
                    //CstEnt.USAGE_PRIM_PAYMENT = PrimAmt;
                    //CustRespOther.Add(Entity);
                }
            }
            CustRespElec = custResponses2.FindAll(u => u.USAGE_PRIM_PAYMENT != string.Empty);
            CustRespElec = CustRespElec.FindAll(u => Convert.ToDecimal(u.USAGE_PRIM_PAYMENT.Trim()) != 0 || u.USAGE_MONTH == "CUR");
            //CustRespElec = custResponses2.FindAll(u => Convert.ToDecimal(u.USAGE_PRIM_PAYMENT.Trim()) != 0 || u.USAGE_MONTH == "CUR");

            if (!string.IsNullOrEmpty(_spmentity.SPM_Gas_Vendor.Trim()) && !string.IsNullOrEmpty(_spmentity.SPM_Gas_Account.Trim()) && !string.IsNullOrEmpty(_spmentity.SPM_Gas_BillName_Type.Trim()) && !string.IsNullOrEmpty(_spmentity.SPM_Gas_Bill_FName.Trim()) &&!string.IsNullOrEmpty(_spmentity.SPM_Gas_Bill_LName.Trim()))
                CustRespOther = custResponses.FindAll(u => Convert.ToDecimal(u.USAGE_SEC_PAYMENT.Trim()) != 0 || u.USAGE_MONTH == "CUR");

            foreach (CustomQuestionsEntity REntity in CustRespOther)
            {
                if (string.IsNullOrEmpty(REntity.USAGE_SEC_PAYMENT.Trim()))
                {
                    REntity.USAGE_SEC_PAYMENT = "0.00";
                    REntity.USAGE_PRIM_PAYMENT = "0.00";
                }
                else
                    REntity.USAGE_PRIM_PAYMENT = REntity.USAGE_SEC_PAYMENT.Trim();

                REntity.CUSTOTHER = "O";

            }

            List<CustomQuestionsEntity> customQuestions = new List<CustomQuestionsEntity>();
            if (CustRespElec.Count > 0)
                customQuestions.AddRange(CustRespElec);
            if (CustRespOther.Count > 0)
                customQuestions.AddRange(CustRespOther);

            if(customQuestions.Count > 0)
            {
                foreach (CustomQuestionsEntity Entity in customQuestions)
                {
                    if(Entity.USAGE_MONTH != "TOT")
                        Entity.CUSTSCRCODE= GetMonth(Entity.USAGE_MONTH.Trim());
                    Entity.CUSTCALLOWFDATE = string.Empty;
                    if ( Entity.USAGE_MONTH!="CUR" && Entity.USAGE_MONTH != "TOT") //CEAPCNTL_List[0].CPCT_LUMP_SUM=="Y" &&
                    {
                        string Month = string.Empty;
                        if(Entity.USAGE_MONTH=="CURR")
                            Month = DateTime.Now.Month.ToString();
                        else
                            Month = Entity.USAGE_MONTH.Trim();
                         string IntakeDate = _spmentity.startdate.Trim();//_baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                        //string Month = GetMonth(Entity.USAGE_MONTH.Trim());
                        string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                        int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());


                        if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                        {
                            string StrUsageDate = string.Empty;

                            if (Entity.USAGE_MONTH == "CURR")
                                StrUsageDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + "1/" + (IntakeYear)).ToShortDateString();
                            //StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            else
                            {
                                StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                                //StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            }
                            //_dRow.Cells["gvtUsageReq"].Value = StrUsageDate;
                            //if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                            //    _dRow.Cells["gvtServicedate"].Value = StrDate;

                            Entity.CUSTREQUIRED = StrUsageDate;
                            //Entity.CUSTCALLOWFDATE= StrUsageDate;
                            if(string.IsNullOrEmpty(Entity.CUSTCALLOWFDATE.Trim()))
                                Entity.CUSTCALLOWFDATE = StrDate;

                        }
                        else if (Convert.ToDateTime(IntakeDate.Trim()).Month == Convert.ToDateTime(StrDate.Trim()).Month)
                        {
                            string StrUsageDate = string.Empty;

                            if (Entity.USAGE_MONTH == "CURR")
                                StrUsageDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + "1/" + (IntakeYear)).ToShortDateString();
                            //StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            else
                            {
                                StrUsageDate = IntakeDate;//Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                                //StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            }
                            //_dRow.Cells["gvtUsageReq"].Value = StrUsageDate;
                            //if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                            //    _dRow.Cells["gvtServicedate"].Value = StrDate;

                            Entity.CUSTREQUIRED = StrUsageDate;
                            //Entity.CUSTCALLOWFDATE= StrUsageDate;
                            if (string.IsNullOrEmpty(Entity.CUSTCALLOWFDATE.Trim()))
                                Entity.CUSTCALLOWFDATE = StrDate;


                        }
                        else
                        {
                            //this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();

                            Entity.CUSTREQUIRED = StrDate;
                            if (string.IsNullOrEmpty(Entity.CUSTCALLOWFDATE.Trim()))
                                Entity.CUSTCALLOWFDATE = StrUsageDate;

                            //_dRow.Cells["gvtUsageReq"].Value = StrDate;

                            //if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                            //    _dRow.Cells["gvtServicedate"].Value = StrUsageDate;

                            //this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                        }
                        Entity.CUSTALPHA = "B";
                        if (txtServCode.Text.Trim() == ServiceCode)
                        {
                            if (Convert.ToDateTime(Entity.CUSTCALLOWFDATE) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim())
                            && Convert.ToDateTime(Entity.CUSTCALLOWFDATE) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                            {
                                Entity.CUSTALPHA = "B";
                                //this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                                //if (_dRow.Cells["gvSelect"].ReadOnly != true)
                                //{
                                //    _dRow.Cells["gvtUsageReq"].Value = "";
                                //    _dRow.Cells["gvtServicedate"].Value = "";
                                //}
                                //this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            }
                            else
                            {
                                Entity.CUSTALPHA= "R";
                                //_dRow.Cells.ForEach(y => y.ReadOnly = true);
                                //_dRow.Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                                //_dRow.Cells[1].Style.ForeColor = Color.OrangeRed;

                                //string strMsg = "The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim());
                                //_dRow.Cells.ForEach(y => y.ToolTipText = strMsg);
                            }
                        }


                    }
                }
            }

            customQuestions = customQuestions.OrderByDescending(u => Convert.ToDecimal(u.USAGE_PRIM_PAYMENT.Trim())).ToList();

            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
            //    customQuestions = customQuestions.FindAll(u => u.USAGE_MONTH != "CUR");

            //    //customQuestions = customQuestions.OrderBy(u=>u.CUSTALPHA).ThenBy(u => Convert.ToInt32(u.CUSTSCRCODE)).ToList();

            customQuestions = customQuestions.OrderBy(u => u.CUSTALPHA).ThenByDescending(u => Convert.ToDecimal(u.USAGE_PRIM_PAYMENT.Trim())).ToList();

            int k = 0; int CeapCnt = 0;
            if (customQuestions.Count > 0)
            {   
                foreach (CustomQuestionsEntity Entity in customQuestions)
                {
                    string MonthName = string.Empty;
                    MonthName = GetMonthName(Entity.USAGE_MONTH.Trim());


                    string UsageAmount = string.Empty;

                    if (Entity.USAGE_MONTH != "TOT")
                    {
                        if(Entity.USAGE_MONTH!="CUR")
                            k++;

                        string Iscaseact = "N";
                        string BundleNo = string.Empty;
                        string PDOut = "N";
                        string VendName = string.Empty;
                        string VendNumber = string.Empty;
                        string VendType = string.Empty;

                        if (Entity.CUSTOTHER == "O")//(Entity.USAGE_PRIM_PAYMENT == Entity.USAGE_SEC_PAYMENT)
                        {
                            VendName = Get_Vendor_Name(SPM_Entity.SPM_Gas_Vendor);
                            VendNumber = SPM_Entity.SPM_Gas_Vendor;
                            VendType = "S";
                            _strElec = "O";
                            IsSecondary = true;
                        }
                        else
                        {
                            VendName = Get_Vendor_Name(SPM_Entity.SPM_Vendor);
                            VendNumber = SPM_Entity.SPM_Vendor;
                            VendType = "P";
                            _strElec = "E";
                            IsPrimary = true;
                        }

                        if (_strElec == "E")
                            UsageAmount = Entity.USAGE_PRIM_PAYMENT.Trim();
                        else
                            UsageAmount = Entity.USAGE_SEC_PAYMENT.Trim();

                        if (UsageAmount == "0.00")
                        {
                            CustomQuestionsEntity REnt = custResponses.Find(u => u.USAGE_MONTH == Entity.USAGE_MONTH);
                            if (REnt != null)
                            {
                                if (_strElec == "E")
                                    UsageAmount = REnt.USAGE_PRIM_PAYMENT.Trim();
                                else
                                    UsageAmount = REnt.USAGE_SEC_PAYMENT.Trim();
                            }
                        }

                        string InvoiceDt = string.Empty;
                        if ((CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y" && ((ListItem)cmbLumpsum.SelectedItem).Value.ToString() != "M") || (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y" && CEAPCNTL_List[0].CPCT_USER_CNTL == "Y" && ((ListItem)cmbLumpsum.SelectedItem).Value.ToString()!="M"))
                        {
                            if (_strElec == "E")
                                ResAmount = Entity.USAGE_LUMP_PRIM.Trim() == "" ? 0 : Convert.ToDecimal(Entity.USAGE_LUMP_PRIM.Trim());
                            else
                                ResAmount = Entity.USAGE_LUMP_SEC.Trim() == "" ? 0 : Convert.ToDecimal(Entity.USAGE_LUMP_SEC.Trim());

                            //ServsDate = DateTime.Now.ToShortDateString();

                            if (ResAmount > 0)
                            {
                                Iscaseact = "Y";
                                if(SP_Activity_Details.Count>0)
                                {
                                    CASEACTEntity ActEnt = new CASEACTEntity();
                                    if(_strElec== "E")
                                        ActEnt = SP_Activity_Details.Find(u => u.ACT_ID == Entity.USAGE_PRIM_ACTID);
                                    else
                                        ActEnt = SP_Activity_Details.Find(u => u.ACT_ID == Entity.USAGE_SEC_ACTID);

                                    if (ActEnt != null) InvoiceDt = LookupDataAccess.Getdate(ActEnt.ACT_Date.ToString());
                                }
                                
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(txtService.Text))
                                InvoiceDetails(Entity.USAGE_MONTH.Trim());

                            if (!string.IsNullOrEmpty(ServsDate.Trim()))
                                InvoiceDt = LookupDataAccess.Getdate(ServsDate.Trim());

                        }

                        if (CaseactRec != null)
                        {
                            if (!string.IsNullOrEmpty(CaseactRec.App_no.Trim()))
                            {
                                Iscaseact = "Y";
                                BundleNo = CaseactRec.BundleNo.Trim();
                                PDOut = CaseactRec.PDOUT.Trim();
                            }
                        }


                        rowIndex = dgvMonthsGrid.Rows.Add(Entity.USAGE_MONTH, MonthName, k, VendName, VendType, UsageAmount, SeekDate.Trim() == "" ? false : true, ResAmount == 0 ? "" : ResAmount.ToString(), InvoiceDt, BlankImg, VendNumber, LookupDataAccess.Getdate(SeekDate.Trim()), LookupDataAccess.Getdate(ServsDate.Trim()), BundleNo, ChkNo, LookupDataAccess.Getdate(ChkDate.Trim()), Iscaseact, PDOut, BlankImg, BlankImg, _strElec);

                        if (!string.IsNullOrEmpty(ServsDate.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.ChangePriv.ToUpper() == "TRUE")
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].ReadOnly = false;
                                    if (!string.IsNullOrEmpty(BundleNo.Trim()))
                                        dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].Value = BlankImg;
                                    else
                                        dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                }
                                else
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].ReadOnly = true;
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvtOverride"].Value = BlankImg;
                                }
                            }
                            dgvMonthsGrid.Rows[rowIndex].Cells["gvSelect"].ReadOnly = true;
                        }
                        dgvMonthsGrid.Rows[rowIndex].Cells["gvtServicedate"].ReadOnly = true;


                        if (!string.IsNullOrEmpty(ServsDate.Trim()) && string.IsNullOrEmpty(ChkDate.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].ReadOnly = false;
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].Value = DeleteImg;
                                }
                                else
                                {
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].ReadOnly = true;
                                    dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].Value = BlankImg;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(BundleNo.Trim()))
                        {
                            dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].ReadOnly = true;
                            dgvMonthsGrid.Rows[rowIndex].Cells["gvDelete"].Value = BlankImg;
                        }

                        if (Entity.CUSTALPHA == "R")
                        {
                            dgvMonthsGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Silver;
                            dgvMonthsGrid.Rows[rowIndex].Cells["gvMonth"].Style.ForeColor = Color.OrangeRed;
                        }

                        if (Iscaseact == "Y")
                        {
                            dgvMonthsGrid.Rows[rowIndex].Tag = CaseactRec;
                            if(Entity.CUSTALPHA != "R") //|| dgvMonthsGrid.Rows[rowIndex].Cells["gvMonth"].Value.ToString()!="Curr"
                                dgvMonthsGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                            if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
                                set_CAMS_Tooltip(rowIndex, CaseactRec.Add_Date, CaseactRec.Add_Operator, CaseactRec.Lstc_Date, CaseactRec.Lsct_Operator);
                        }

                        if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y")
                        {
                            if (dgvMonthsGrid.Rows[rowIndex].Cells["gvAmtPaid"].Value.ToString() != string.Empty)
                            {
                                dgvMonthsGrid.Rows[rowIndex].Cells["gvSelect"].Value = true;
                                //if (dgvMonthsGrid.Rows[rowIndex].Cells["gvMonthsCode"].Value.ToString() != "CUR")
                                //{
                                dgvMonthsGrid.Rows[rowIndex].Cells["gvAmtPaid"].ReadOnly = true;
                                dgvMonthsGrid.Rows[rowIndex].Cells["gvSelect"].ReadOnly = true;
                                //}
                                CeapCnt++;
                            }

                        }
                        else
                        {
                            if (dgvMonthsGrid.Rows[rowIndex].Cells["gvMonthsCode"].Value.ToString() == "CUR")
                            {
                                dgvMonthsGrid.Rows[rowIndex].Cells["gvAmtPaid"].ReadOnly = false;
                            }
                        }

                    }
                }

                if(dgvMonthsGrid.Rows.Count>0)
                {
                    txtRemTrans.Text = CeapCnt.ToString();//dgvInvoicePosting.Rows.Count.ToString();//CeapCnt.ToString();
                    txtTotInv.Text = CeapCnt.ToString();//dgvMonthsGrid.Rows.Count.ToString();//CeapCnt.ToString();
                    txtInvBal.Text = (Convert.ToDecimal(txtMaxInv.Text.Trim() == "" ? "0" : txtMaxInv.Text.Trim()) - Convert.ToDecimal(txtTotInv.Text.Trim())).ToString();

                    List<DataGridViewRow> Selrows = dgvMonthsGrid.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["gvprimorSeco"].Value.ToString() == "P").ToList();
                    if (Selrows.Count > 0) pnlPCurrInv.Visible = true; else pnlPCurrInv.Visible = false;
                    Selrows = dgvMonthsGrid.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["gvprimorSeco"].Value.ToString() == "S").ToList();
                    if (Selrows.Count > 0) pnlSCurrInv.Visible = true; else pnlSCurrInv.Visible = false;


                }


            }

            if (dgvMonthsGrid.Rows.Count > 0)
            {
                PCnt = 0;SCnt= 0;
                txtPVendCounts.Text = PCnt.ToString(); txtSVendCounts.Text = SCnt.ToString();

                this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                foreach (DataGridViewRow _dRow in dgvMonthsGrid.Rows)
                {
                    if (_dRow.Cells["gvMonthsCode"].Value.ToString().Trim() == "CUR")  
                    {
                        
                        string IntakeDate = _spmentity.startdate.Trim();
                        string StrDate = DateTime.Now.ToShortDateString();
                        if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                        {
                            _dRow.Cells["gvtUsageReq"].Value = StrDate; _dRow.Cells["gvtServicedate"].Value = StrDate;
                        }
                        else
                        { _dRow.Cells["gvtUsageReq"].Value = IntakeDate; _dRow.Cells["gvtServicedate"].Value = IntakeDate; }

                        //this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                    }  //(CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y" && _dRow.Cells["gvMonthsCode"].Value.ToString().Trim() == "CUR") || 
                    else
                    {
                        string IntakeDate = _spmentity.startdate.Trim();//_baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                        string Month = GetMonth(_dRow.Cells["gvMonthsCode"].Value.ToString().Trim());
                        string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                        int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                        if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                        {
                            this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            //string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                            _dRow.Cells["gvtUsageReq"].Value = StrUsageDate;
                            if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                                _dRow.Cells["gvtServicedate"].Value = StrDate;

                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                        }
                        else if(Convert.ToDateTime(IntakeDate.Trim()).Month==Convert.ToDateTime(StrDate.Trim()).Month)
                        {
                            this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            //string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                            _dRow.Cells["gvtUsageReq"].Value = StrUsageDate;
                            if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                                _dRow.Cells["gvtServicedate"].Value = IntakeDate;

                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                        }
                        else
                        {
                            this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();

                            _dRow.Cells["gvtUsageReq"].Value = StrDate;

                            if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                                _dRow.Cells["gvtServicedate"].Value = StrUsageDate;

                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                        }
                        if (txtServCode.Text.Trim() == ServiceCode)
                        {
                            if (Convert.ToDateTime(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim())
                            && Convert.ToDateTime(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                            {
                                this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                                if (_dRow.Cells["gvSelect"].ReadOnly != true)
                                {
                                    _dRow.Cells["gvtUsageReq"].Value = "";
                                    _dRow.Cells["gvtServicedate"].Value = "";
                                }
                                this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            }
                            else
                            {
                                _dRow.Cells.ForEach(y => y.ReadOnly = true);
                                _dRow.Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                                _dRow.Cells[1].Style.ForeColor = Color.OrangeRed;

                                string strMsg = "The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim());
                                _dRow.Cells.ForEach(y => y.ToolTipText = strMsg);
                            }
                        }
                    }
                    if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y")
                    {
                        if (_dRow.Cells["gvAmtPaid"].Value.ToString() != string.Empty)
                        {

                            _dRow.Cells["gvSelect"].Value = true;

                            if (_dRow.Cells["gvMonthsCode"].Value.ToString().Trim() != "CUR")
                            {
                                _dRow.Cells["gvSelect"].ReadOnly = true;
                                _dRow.Cells["gvAmtPaid"].ReadOnly = true;
                            }
                            //_dRow.Cells["gvDelete"].Value = DeleteImg;
                            //_dRow.Cells["gvDelete"].ReadOnly = false;
                        }
                    }
                }

                this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
            }
            dgvMonthsGrid.Update();
        }


        private void set_CAMS_Tooltip(int rowIndex, string Add_Date, string Add_Opr, string Lstc_Date, string Lstc_Opr)
        {
            string toolTipText = "Added By     : " + Add_Opr + " on " + Add_Date + "\n" +
                                 "Modified By  : " + Lstc_Opr + " on " + Lstc_Date;

            foreach (DataGridViewCell cell in dgvMonthsGrid.Rows[rowIndex].Cells)
                cell.ToolTipText = toolTipText;
        }
        private string GetMonth(string Month)
        {
            string MonName = string.Empty;

            switch (Month)
            {
                case "JAN":
                    MonName = "1";
                    break;
                case "FEB":
                    MonName = "2";
                    break;
                case "MAR":
                    MonName = "3";
                    break;
                case "APR":
                    MonName = "4";
                    break;
                case "MAY":
                    MonName = "5";
                    break;
                case "JUN":
                    MonName = "6";
                    break;
                case "JUL":
                    MonName = "7";
                    break;
                case "AUG":
                    MonName = "8";
                    break;
                case "SEP":
                    MonName = "9";
                    break;
                case "OCT":
                    MonName = "10";
                    break;
                case "NOV":
                    MonName = "11";
                    break;
                case "DEC":
                    MonName = "12";
                    break;
                case "CUR":
                    MonName = "00";
                    break;
            }

            return MonName;
        }
        private string GetMonthName(string Month)
        {
            string MonName = string.Empty;

            switch (Month)
            {
                case "JAN":
                    MonName = "01-January";
                    break;
                case "FEB":
                    MonName = "02-February";
                    break;
                case "MAR":
                    MonName = "03-March";
                    break;
                case "APR":
                    MonName = "04-April";
                    break;
                case "MAY":
                    MonName = "05-May";
                    break;
                case "JUN":
                    MonName = "06-June";
                    break;
                case "JUL":
                    MonName = "07-July";
                    break;
                case "AUG":
                    MonName = "08-August";
                    break;
                case "SEP":
                    MonName = "09-September";
                    break;
                case "OCT":
                    MonName = "10-October";
                    break;
                case "NOV":
                    MonName = "11-November";
                    break;
                case "DEC":
                    MonName = "12-December";
                    break;
                case "CUR":
                    MonName = "00-Current Invoice";
                    break;
            }

            return MonName;
        }
        private string Get_Vendor_Name(string VendorNo)
        {
            string Vend_Name = string.Empty;
            foreach (CASEVDDEntity Entity in CaseVddlist)
            {
                if (Entity.Code == VendorNo)
                {
                    Vend_Name = Entity.Name.Trim();
                    break;
                }
            }

            return Vend_Name;
        }
        private void InvoiceDetails(string Month)
        {
            ResAmount = 0;
            ChkNo = string.Empty;
            ChkDate = string.Empty;
            ServsDate = string.Empty;
            SeekDate = string.Empty;
            CaseactRec = new CASEACTEntity();
            List<CASEACTEntity> CaseactList = new List<CASEACTEntity>();

            if (txtServCode.Text.Trim() == ServiceCode)
            {
                if (SP_Activity_Details.Count > 0)
                {
                    List<CASEACTEntity> SelCaseactlist = SP_Activity_Details.FindAll(u => u.ACT_Code.Trim() == txtServCode.Text.Trim());

                    switch (Month)
                    {
                        case "CUR":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk=="C" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JAN":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "1" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim(); 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "FEB":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "2" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAR":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "3" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "APR":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "4" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAY":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "5" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUN":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "6" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUL":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "7" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "AUG":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "8" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());// CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "SEP":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "9" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "OCT":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "10" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "NOV":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk!="C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "11" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "DEC":
                            CaseactList = SelCaseactlist.FindAll(u => u.Cost.Trim() != "" && u.Bulk != "C" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "12" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                    }
                }
            }
            else
            {
                if (SP_Activity_Details.Count > 0)
                {
                    switch (Month)
                    {
                        case "JAN":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "1" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "FEB":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "2" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAR":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "3" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "APR":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "4" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAY":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "5" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUN":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "6" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUL":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "7" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "AUG":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "8" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "SEP":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "9" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "OCT":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "10" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "NOV":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "11" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "DEC":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "12" && u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == _strElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No;
                                ChkDate = CaseactList[0].Check_Date.Trim();
                                ServsDate = CaseactList[0].ACT_Date.Trim();
                                SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                    }
                }
            }
        }

        #region Properties

        public BaseForm _baseForm
        {
            get;
            set;
        }
        public PrivilegeEntity _privilegeEntity
        {
            get;
            set;
        }
        public string _Mode
        {
        get; set;
        }
        public string propReportPath
        {
            get; set;
        }
        public string _spCode
        {
            get;
            set;
        }
        public string _spSequence
        {
            get;
            set;
        }
        public string _spmYear
        {
            get;
            set;
        }
        public string _spmName
        {
            get;
            set;
        }
        public string _strPrimSource
        {
            get;
            set;
        }
        public string _strType
        {
            get;
            set;
        }
        public string _strOtherSource
        {
            get;
            set;
        }
        public string _strElec
        {
            get;
            set;
        }
        public string _strOther
        {
            get;
            set;
        }
        public CASESPMEntity _spmentity
        {
            get;
            set;
        }
        public CASESPMEntity SPM_Entity
        {
            get; set;
        }
        List<CASESP0Entity> propSearch_Entity
        {
            get; set;
        }
        List<CommonEntity> propfundingsource
        {
            get; set;
        }
        List<CMBDCEntity> Emsbdc_List
        {
            get; set;
        }
        List<CMBDCEntity> ALLEmsbdc_List
        {
            get; set;
        }
        List<CEAPCNTLEntity> CEAPCNTL_List
        {
            get; set;
        }
        public PrivilegeEntity PostUsagePriv
        {
            get; set;
        }
        CASESP0Entity SP_Header_Rec;

        string ACR_SERV_Hies = string.Empty;
        public string ServiceCode
        {
            get; set;
        }

        #endregion

        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (_baseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == _baseForm.BaseAgency);
        }

        List<CASESPMEntity> CASESPM_SP_List;
        CASESPMEntity Search_Entity = new CASESPMEntity();
        private void Fill_Applicant_SPs()
        {
            Search_Entity.agency = _baseForm.BaseAgency;
            Search_Entity.dept = _baseForm.BaseDept;
            Search_Entity.program = _baseForm.BaseProg;
            //Search_Entity.year = BaseYear;
            Search_Entity.year = null;                          // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = _baseForm.BaseApplicationNo;

            Search_Entity.service_plan = _spCode;
            Search_Entity.Seq = _spSequence;
            Search_Entity.caseworker = Search_Entity.site = null;
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = Search_Entity.Def_Program =
            Search_Entity.SPM_MassClose = Search_Entity.Seq = Search_Entity.Bulk_Post = null;

            CASESPM_SP_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");

            if (CASESPM_SP_List.Count > 0)
            {
                SPM_Entity = CASESPM_SP_List[0];

                Get_App_CASEACT_List(SPM_Entity);

                txtAward.Text = SPM_Entity.SPM_Amount;
                txtBalance.Text = SPM_Entity.SPM_Balance;
                decimal PaidAmt = 0;

                PaidAmt = Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Amount.Trim()) ? SPM_Entity.SPM_Amount.Trim() : "0") - Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Balance.Trim()) ? SPM_Entity.SPM_Balance.Trim() : "0");
                txtTotPaid.Text = PaidAmt.ToString();
            }
        }

        List<CASESP2Entity> SP_CAMS_Details = new List<CASESP2Entity>();
        List<CASEACTEntity> arrearEntity = new List<CASEACTEntity>();
        private void FillServiceTextbox()
        {
            SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(SPM_Entity.service_plan, null, null, null, "CASE4006");

            if (SP_CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_CAMS_Details.FindAll(u => u.ServPlan == SPM_Entity.service_plan && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    bool IsService = true; bool IsArrears=true;
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        if (CEAPCNTL_List[0].CPCT_VUL_SP.Trim() == SPM_Entity.service_plan)
                        {
                            if (CEAPCNTL_List[0].CPCT_VUL_PRIM_CA.Trim() == Entity.CamCd.Trim())
                                IsService = true;
                            else
                                IsService = false;

                            if (CEAPCNTL_List[0].CPCT_VUL_ARR_CA == Entity.CamCd.Trim())
                                IsArrears = true;
                            else
                                IsArrears = false;

                        }
                        else if (CEAPCNTL_List[0].CPCT_NONVUL_SP.Trim() == SPM_Entity.service_plan)
                        {
                            if (CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA.Trim() == Entity.CamCd.Trim())
                                IsService = true;
                            else
                                IsService = false;

                            if (CEAPCNTL_List[0].CPCT_NONVUL_ARR_CA == Entity.CamCd.Trim())
                                IsArrears = true;
                            else
                                IsArrears = false;
                        }

                        if (IsService)
                        {
                            txtService.Text = Entity.CAMS_Desc.Trim();
                            txtServCode.Text = Entity.CamCd.Trim();
                            txtBranch.Text = Entity.Branch.Trim();
                            txtGroup.Text = Entity.Curr_Grp.ToString();

                            //if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "A")
                            //    txtLumpInterval.Text = "Annual";
                            //if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "S")
                            //    txtLumpInterval.Text = "Semi-Annual";
                            //if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == "Q")
                            //    txtLumpInterval.Text = "Quaterly";

                            SelListEntity = SP_Activity_Details.FindAll(u => u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other != string.Empty);

                            if (SelListEntity.Count != 0)
                            {
                                if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL == CASESPM_SP_List[0].SPM_LUMP_INTRVAL)
                                {
                                    if (!string.IsNullOrEmpty(SPM_Entity.SPM_LUMP_INTRVAL.Trim()))
                                    {
                                        CommonFunctions.SetComboBoxValue(cmbLumpsum, SPM_Entity.SPM_LUMP_INTRVAL);
                                        cmbLumpsum.Enabled = false;
                                    }
                                    else
                                    {
                                        CommonFunctions.SetComboBoxValue(cmbLumpsum, CEAPCNTL_List[0].CPCT_LUMP_INTRVAL);
                                        cmbLumpsum.Enabled = true;
                                    }
                                }
                                else
                                {
                                    List<ListItem> listItem = new List<ListItem>();
                                    listItem.Add(new ListItem("", "0"));

                                    cmbLumpsum.Items.AddRange(listItem.ToArray());

                                    CommonFunctions.SetComboBoxValue(cmbLumpsum, "0");
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(SPM_Entity.SPM_LUMP_INTRVAL.Trim()))
                                {
                                    CommonFunctions.SetComboBoxValue(cmbLumpsum, SPM_Entity.SPM_LUMP_INTRVAL);
                                    cmbLumpsum.Enabled = false;
                                }
                                else
                                {
                                    CommonFunctions.SetComboBoxValue(cmbLumpsum, CEAPCNTL_List[0].CPCT_LUMP_INTRVAL);
                                    cmbLumpsum.Enabled = true;
                                }
                            }
                        }
                        if (IsArrears)
                        {
                            lblArrear.Text=Entity.CAMS_Desc.Trim();
                            txtArrearCode.Text = Entity.CamCd.Trim();

                            SP_Activity_Details = SP_Activity_Details.OrderBy(x => x.Lstc_Date).ToList();

                            arrearEntity = SP_Activity_Details.FindAll(x => x.ACT_Code.ToString() == txtArrearCode.Text && x.Vendor_No ==SPM_Entity.SPM_Vendor && x.Bulk=="A");

                            if (arrearEntity.Count > 0)
                            {
                                dtpPArrDte.Value = arrearEntity[0].ACT_Date.ToString() == "" ? DateTime.Now : Convert.ToDateTime(arrearEntity[0].ACT_Date.ToString()).Date;

                                txtPArrInv.Text = arrearEntity[0].Cost.ToString();
                                txtPArrBundle.Text= arrearEntity[0].BundleNo.ToString();
                                txtPArrCheck.Text = arrearEntity[0].Check_No.ToString();
                                txtPArrChkDt.Text = arrearEntity[0].Check_Date.ToString();

                                pbPArrCncl.Visible = false;

                                if (!string.IsNullOrEmpty(arrearEntity[0].BundleNo.Trim()))
                                {
                                    pbPArrDel.Visible = false;
                                    pbPArrEdit.Visible = false;
                                }
                                else
                                {
                                    pbPArrDel.Visible = true;
                                    pbPArrEdit.Visible = true;
                                }

                                pbPArrAdd.Visible = false;
                            }
                            else
                            {
                                pbPArrCncl.Visible = false;

                                pbPArrDel.Visible = false;
                                pbPArrEdit.Visible = false;

                                pbPArrAdd.Visible = true;

                                txtPArrInv.Text = string.Empty;
                                dtpPArrDte.Text = SPM_Entity.startdate.ToString(); //DateTime.Now.ToShortDateString();
                                //txtPArrInv.Enabled = true; dtpPArrDte.Enabled = true;
                            }

                            arrearEntity = SP_Activity_Details.FindAll(x => x.ACT_Code.ToString() == txtArrearCode.Text && x.Vendor_No == SPM_Entity.SPM_Gas_Vendor && x.Bulk == "A");

                            if (arrearEntity.Count > 0)
                            {
                                dtpSArrDte.Value = arrearEntity[0].ACT_Date.ToString() == "" ? DateTime.Now : Convert.ToDateTime(arrearEntity[0].ACT_Date.ToString()).Date;

                                txtSArrInv.Text = arrearEntity[0].Cost.ToString();
                                txtSArrBundle.Text = arrearEntity[0].BundleNo.ToString();
                                txtSArrChkNo.Text = arrearEntity[0].Check_No.ToString();
                                txtSArrChkDt.Text = LookupDataAccess.Getdate(arrearEntity[0].Check_Date.ToString().Trim());

                                pbSArrCncl.Visible = false;

                                if (!string.IsNullOrEmpty(arrearEntity[0].BundleNo.Trim()))
                                {
                                    pbSArrDel.Visible = false;
                                    pbSArrEdit.Visible = false;
                                }
                                else
                                {
                                    pbSArrEdit.Visible = true;
                                    pbSArrDel.Visible = true;
                                }

                                pbSArrAdd.Visible = false;
                            }
                            else
                            {
                                pbSArrCncl.Visible = false;

                                pbSArrEdit.Visible = false;
                                pbSArrDel.Visible = false;

                                pbSArrAdd.Visible = true;

                                txtSArrInv.Text = string.Empty;
                                dtpSArrDte.Text = SPM_Entity.startdate.ToString();//DateTime.Now.ToShortDateString();
                                //txtSArrInv.Enabled = true; dtpSArrDte.Enabled = true;
                            }
                        }


                    }
                }
            }
        }

        List<CASEACTEntity> PropCaseactList = new List<CASEACTEntity>();
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


            SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse", "PAYMENT");

            if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
            {
                if (SP_Activity_Details.Count > 0)
                    SP_Activity_Details = SP_Activity_Details.FindAll(u => u.ActSeek_Date.Trim() != string.Empty);
            }

            SP_Activity_Details = SP_Activity_Details.OrderByDescending(u => Convert.ToDateTime(u.ACT_Date.Trim())).ToList();

            Search_Entity.app_no = string.Empty;
            PropCaseactList = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse", "PAYMENT");
            if (PropCaseactList.Count > 0)
                PropCaseactList = PropCaseactList.FindAll(u => u.Elec_Other == "E" || u.Elec_Other == "O");

            if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
            {
                if (PropCaseactList.Count > 0)
                {
                    txtRemTrans.Text = PropCaseactList.Count.ToString();
                    txtTotInv.Text = PropCaseactList.Count.ToString();
                    txtInvBal.Text = (Convert.ToDecimal(txtMaxInv.Text.Trim() == "" ? "0" : txtMaxInv.Text.Trim()) - Convert.ToDecimal(txtTotInv.Text.Trim())).ToString();
                }
                else
                {
                    txtTotInv.Text = "0";
                    txtInvBal.Text = txtMaxInv.Text.Trim();
                }
            }
        }

        bool Vulner_Flag = false;
        bool Age_Grt_60 = false, Age_Les_6 = false, Disable_Flag = false, FoodStamps_Flag = false;

        bool IsCal = false; bool isAlert = false;
        int PCnt = 0;int SCnt = 0;
        private void dgvMonthsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMonthsGrid.Rows.Count > 0)
            {
                dgvMonthsGrid.CellValueChanged -= new DataGridViewCellEventHandler(dgvMonthsGrid_CellValueChanged);
                if (e.ColumnIndex == gvSelect.Index && e.RowIndex != -1)
                {
                    #region LumpSum
                    //if (CEAPCNTL_List[0].CPCT_LUMP_INTRVAL != "")
                    //{
                    //    string strType = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvPrimorSeco"].Value.ToString();
                    //    if (((ListItem)cmbLump.SelectedItem).Value.ToString() == "A")
                    //    {
                    //        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].ReadOnly == false)
                    //        {
                    //            bool IsFalse = true;
                    //            IsCal = false;
                    //            string strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                    //            int introwindex = dgvMonthsGrid.CurrentCell.RowIndex;
                    //            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].ReadOnly = true;

                    //            var PCount = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                    //                   .Count(row => (row.Cells["gvPrimorSeco"].Value.ToString() == strType));

                    //            //if (PCount > 1)
                    //            //{
                    //            //    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //            //    strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                    //            //}
                    //            //else
                    //            {
                    //                if (strdata.ToUpper() == "TRUE")
                    //                {
                    //                    RecCount++;
                    //                    string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString();
                    //                    decimal OrigBal = 0;
                    //                    if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                    {
                    //                        this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                    //                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = UsageAmount;
                    //                        this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                    //                        decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                        {
                    //                            if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                    //                                OrigBal = Convert.ToDecimal(txtBalance.Text.Trim());
                    //                        }

                    //                        isAlert = false;
                    //                        if (IsCal == false)
                    //                            CalculateAmounts(introwindex);

                    //                        decimal Balance = 0;
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                            Balance = Convert.ToDecimal(txtBalance.Text.Trim());// - Amount;

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Amount > 0)
                    //                            {
                    //                                int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                    //                                int PaidTrans = 0;
                    //                                if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                    //                                {
                    //                                    if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                    //                                        PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                    //                                    PaidTrans++;

                    //                                    if (PaidTrans > MaxTrans)
                    //                                    {
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //                                        AlertBox.Show("We can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                    //                                        IsFalse = false;
                    //                                    }
                    //                                    else
                    //                                    {

                    //                                    }

                    //                                }

                    //                                if (Balance <= 0)
                    //                                {
                    //                                    if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                    //                                    {
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = txtBalance.Text;
                    //                                        txtBalance.Text = "0";
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString() != OrigBal.ToString())
                    //                                        {
                    //                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                    //                                            if (isAlert == false)
                    //                                                AlertBox.Show("We can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                    //                                            IsFalse = false;
                    //                                        }
                    //                                    }
                    //                                }
                    //                                else
                    //                                {

                    //                                }

                    //                                if (IsFalse)
                    //                                {
                    //                                    txtRemTrans.Text = PaidTrans.ToString();
                    //                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                    //                                    if (Balance >= 0)
                    //                                        txtBalance.Text = Balance.ToString();
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                    //                                AlertBox.Show("You cannot post invoice with Amount Paid of zero", MessageBoxIcon.Warning);
                    //                                IsFalse = false;
                    //                            }
                    //                        }

                    //                    }

                    //                    if (IsFalse)
                    //                    {
                    //                        string IntakeDate = _baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                    //                        string Month = GetMonth(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvMonthsCode"].Value.ToString().Trim());
                    //                        string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                    //                        int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                    //                        if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                    //                        {
                    //                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                    //                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrUsageDate;
                    //                            if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrDate;

                    //                            if (PostUsagePriv != null)
                    //                            {
                    //                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                    //                                }
                    //                                else
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                                }
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                    //                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();
                    //                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrDate;

                    //                            if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrUsageDate;

                    //                            if (PostUsagePriv != null)
                    //                            {
                    //                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                    //                                }
                    //                                else
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                                }
                    //                            }
                    //                        }

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                    //                            {
                    //                                dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                    //                       //         if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                       //         {
                    //                       //             decimal uAmount = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                    //                       //.Where(row => !row.IsNewRow && (row.Cells[1].Style.ForeColor != Color.OrangeRed) && (row.Cells["gvPrimorSeco"].Value.ToString() == strType)) // && !(row.Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE" && row.Cells["gvkChk"].ReadOnly == true)
                    //                       //                .Sum(row =>
                    //                       //                {
                    //                       //                    var cellValue = row.Cells["gvUsage"].Value;
                    //                       //                    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                    //                       //                    {
                    //                       //                        return 0m;
                    //                       //                    }

                    //                       //                    if (decimal.TryParse(cellValue.ToString(), out decimal amount))
                    //                       //                    {
                    //                       //                        return amount;
                    //                       //                    }

                    //                       //                    return 0m;
                    //                       //                });

                    //                       //             if (uAmount > Convert.ToDecimal(SPM_Entity.SPM_Balance))
                    //                       //             {
                    //                       //                 uAmount = Convert.ToDecimal(SPM_Entity.SPM_Balance);
                    //                       //             }
                    //                       //             decimal balat = Convert.ToDecimal(txtBalance.Text) + Convert.ToDecimal(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString());
                    //                       //             txtBalance.Text = balat.ToString();

                    //                       //             decimal _balAmt = Convert.ToDecimal(txtBalance.Text) - Convert.ToDecimal(uAmount);
                    //                       //             txtBalance.Text = _balAmt.ToString();
                    //                       //             this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                    //                       //             dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = uAmount.ToString();
                    //                       //             this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);


                    //                       //             if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                       //             {
                    //                       //                 for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                    //                       //                 {
                    //                       //                     if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                    //                       //                     {
                    //                       //                         if (dgvMonthsGrid.Rows[x].Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE")
                    //                       //                         {
                    //                       //                             //dgvMonthsGrid.Rows[x].Cells["gvtOverride"].Value = BlankImg;
                    //                       //                         }
                    //                       //                         else
                    //                       //                         {
                    //                       //                             dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = true);
                    //                       //                             dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                    //                       //                         }
                    //                       //                     }
                    //                       //                 }
                    //                       //             }

                    //                       //         }

                    //                            }
                    //                            else
                    //                            {
                    //                                decimal cellAmount = 0;
                    //                                if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                                    cellAmount = Convert.ToDecimal(UsageAmount.Trim());

                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells.ForEach(x => x.Style.ForeColor = Color.Gray);
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.OrangeRed;


                    //                                txtBalance.Text = OrigBal.ToString();
                    //                                AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()), MessageBoxIcon.Warning);
                    //                            }
                    //                        }
                    //                    }
                    //                    if (RecCount > 0)
                    //                    {
                    //                        if (PostUsagePriv != null)
                    //                        {
                    //                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                btnSave.Enabled = true;
                    //                            else
                    //                                btnSave.Enabled = false;

                    //                        }
                    //                    }
                    //                    else
                    //                        btnSave.Enabled = false;
                    //                }
                    //                else
                    //                {
                    //                    RecCount--;

                    //                    string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString();
                    //                    if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                    {
                    //                        decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                    //                        decimal Balance = 0;
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                            Balance = Convert.ToDecimal(txtBalance.Text.Trim());
                    //                        if (dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Red)
                    //                        {
                    //                            if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                                Balance = Convert.ToDecimal(txtBalance.Text.Trim()) + Amount;
                    //                        }

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Balance <= 0)
                    //                            {
                    //                                AlertBox.Show("We can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                    //                                IsFalse = false;
                    //                            }
                    //                            else
                    //                            {
                    //                                txtBalance.Text = Balance.ToString();
                    //                                if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                                {
                    //                                    for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                    //                                    {
                    //                                        if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                    //                                        {
                    //                                            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = false);
                    //                                            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Black);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }


                    //                            if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                    //                            {
                    //                                int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                    //                                int PaidTrans = 0;
                    //                                if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                    //                                    PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                    //                                PaidTrans--;

                    //                                if (PaidTrans > MaxTrans)
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //                                    AlertBox.Show("We can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                    //                                    IsFalse = false;
                    //                                }
                    //                                else
                    //                                {
                    //                                    txtRemTrans.Text = PaidTrans.ToString();
                    //                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                    //                                }

                    //                            }
                    //                        }

                    //                    }

                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                    //                    if (RecCount > 0)
                    //                    {
                    //                        if (PostUsagePriv != null)
                    //                        {
                    //                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                btnSave.Enabled = true;
                    //                            else
                    //                                btnSave.Enabled = false;
                    //                        }
                    //                        btnSave.Enabled = true;
                    //                    }
                    //                    else
                    //                        btnSave.Enabled = false;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else if (((ListItem)cmbLump.SelectedItem).Value.ToString() == "S")
                    //    {
                    //        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].ReadOnly == false)
                    //        {
                    //            bool IsFalse = true;
                    //            IsCal = false;
                    //            string strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                    //            int introwindex = dgvMonthsGrid.CurrentCell.RowIndex;
                    //            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].ReadOnly = true;

                    //            var PCount = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                    //                  .Count(row => (row.Cells["gvPrimorSeco"].Value.ToString() == strType));

                    //            //if (PCount > 2)
                    //            //{
                    //            //    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //            //    strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                    //            //}
                    //            //else
                    //            {
                    //                if (strdata.ToUpper() == "TRUE")
                    //                {
                    //                    RecCount++;
                    //                    string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString();
                    //                    decimal OrigBal = 0;
                    //                    if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                    {
                    //                        this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                    //                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = UsageAmount;
                    //                        this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                    //                        decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                        {
                    //                            if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                    //                                OrigBal = Convert.ToDecimal(txtBalance.Text.Trim());
                    //                        }

                    //                        isAlert = false;
                    //                        if (IsCal == false)
                    //                            CalculateAmounts(introwindex);

                    //                        decimal Balance = 0;
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                            Balance = Convert.ToDecimal(txtBalance.Text.Trim());// - Amount;

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Amount > 0)
                    //                            {
                    //                                int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                    //                                int PaidTrans = 0;
                    //                                if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                    //                                {
                    //                                    if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                    //                                        PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                    //                                    PaidTrans++;

                    //                                    if (PaidTrans > MaxTrans)
                    //                                    {
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //                                        AlertBox.Show("We can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                    //                                        IsFalse = false;
                    //                                    }
                    //                                    else
                    //                                    {

                    //                                    }

                    //                                }

                    //                                if (Balance <= 0)
                    //                                {
                    //                                    if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                    //                                    {
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = txtBalance.Text;
                    //                                        txtBalance.Text = "0";
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString() != OrigBal.ToString())
                    //                                        {
                    //                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                    //                                            if (isAlert == false)
                    //                                                AlertBox.Show("we can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                    //                                            IsFalse = false;
                    //                                        }
                    //                                    }
                    //                                }
                    //                                else
                    //                                {

                    //                                }

                    //                                if (IsFalse)
                    //                                {
                    //                                    txtRemTrans.Text = PaidTrans.ToString();
                    //                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                    //                                    if (Balance >= 0)
                    //                                        txtBalance.Text = Balance.ToString();
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                    //                                AlertBox.Show("You cannot post invoice with Amount Paid of zero", MessageBoxIcon.Warning);
                    //                                IsFalse = false;
                    //                            }
                    //                        }

                    //                    }

                    //                    if (IsFalse)
                    //                    {
                    //                        string IntakeDate = _baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                    //                        string Month = GetMonth(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvMonthsCode"].Value.ToString().Trim());
                    //                        string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                    //                        int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                    //                        if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                    //                        {
                    //                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                    //                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrUsageDate;
                    //                            if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrDate;

                    //                            if (PostUsagePriv != null)
                    //                            {
                    //                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                    //                                }
                    //                                else
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                                }
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                    //                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();
                    //                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrDate;

                    //                            if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrUsageDate;

                    //                            if (PostUsagePriv != null)
                    //                            {
                    //                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                    //                                }
                    //                                else
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                                }
                    //                            }
                    //                        }

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                    //                            {
                    //                                dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                    //                       //         if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                       //         {
                    //                       //             decimal uAmount = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                    //                       //.Where(row => !row.IsNewRow && (row.Cells[1].Style.ForeColor != Color.OrangeRed) && (row.Cells["gvPrimorSeco"].Value.ToString() == strType)) // && !(row.Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE" && row.Cells["gvkChk"].ReadOnly == true)
                    //                       //                .Sum(row =>
                    //                       //                {
                    //                       //                    var cellValue = row.Cells["gvUsage"].Value;
                    //                       //                    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                    //                       //                    {
                    //                       //                        return 0m;
                    //                       //                    }

                    //                       //                    if (decimal.TryParse(cellValue.ToString(), out decimal amount))
                    //                       //                    {
                    //                       //                        return amount;
                    //                       //                    }

                    //                       //                    return 0m;
                    //                       //                });

                    //                       //             if (uAmount > Convert.ToDecimal(SPM_Entity.SPM_Balance))
                    //                       //             {
                    //                       //                 uAmount = Convert.ToDecimal(SPM_Entity.SPM_Balance);
                    //                       //             }
                    //                       //             decimal balat = Convert.ToDecimal(txtBalance.Text) + Convert.ToDecimal(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString());
                    //                       //             txtBalance.Text = balat.ToString();

                    //                       //             decimal _balAmt = Convert.ToDecimal(txtBalance.Text) - Convert.ToDecimal(uAmount);
                    //                       //             txtBalance.Text = _balAmt.ToString();
                    //                       //             this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                    //                       //             dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = uAmount.ToString();
                    //                       //             this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);


                    //                       //             if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                       //             {
                    //                       //                 for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                    //                       //                 {
                    //                       //                     if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                    //                       //                     {
                    //                       //                         if (dgvMonthsGrid.Rows[x].Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE")
                    //                       //                         {
                    //                       //                             //dgvMonthsGrid.Rows[x].Cells["gvtOverride"].Value = BlankImg;
                    //                       //                         }
                    //                       //                         else
                    //                       //                         {
                    //                       //                             dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = true);
                    //                       //                             dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                    //                       //                         }
                    //                       //                     }
                    //                       //                 }
                    //                       //             }

                    //                       //         }

                    //                            }
                    //                            else
                    //                            {
                    //                                decimal cellAmount = 0;
                    //                                if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                                    cellAmount = Convert.ToDecimal(UsageAmount.Trim());

                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells.ForEach(x => x.Style.ForeColor = Color.Gray);
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.OrangeRed;


                    //                                txtBalance.Text = OrigBal.ToString();
                    //                                AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()), MessageBoxIcon.Warning);
                    //                            }
                    //                        }
                    //                    }
                    //                    if (RecCount > 0)
                    //                    {
                    //                        if (PostUsagePriv != null)
                    //                        {
                    //                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                btnSave.Enabled = true;
                    //                            else
                    //                                btnSave.Enabled = false;

                    //                        }
                    //                    }
                    //                    else
                    //                        btnSave.Enabled = false;
                    //                }
                    //                else
                    //                {
                    //                    RecCount--;

                    //                    string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString();
                    //                    if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                    {
                    //                        decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                    //                        decimal Balance = 0;
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                            Balance = Convert.ToDecimal(txtBalance.Text.Trim());
                    //                        if (dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Red)
                    //                        {
                    //                            if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                                Balance = Convert.ToDecimal(txtBalance.Text.Trim()) + Amount;
                    //                        }

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Balance <= 0)
                    //                            {
                    //                                AlertBox.Show("We can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                    //                                IsFalse = false;
                    //                            }
                    //                            else
                    //                            {
                    //                                txtBalance.Text = Balance.ToString();
                    //                                if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                                {
                    //                                    for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                    //                                    {
                    //                                        if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                    //                                        {
                    //                                            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = false);
                    //                                            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Black);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }


                    //                            if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                    //                            {
                    //                                int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                    //                                int PaidTrans = 0;
                    //                                if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                    //                                    PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                    //                                PaidTrans--;

                    //                                if (PaidTrans > MaxTrans)
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //                                    AlertBox.Show("we can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                    //                                    IsFalse = false;
                    //                                }
                    //                                else
                    //                                {
                    //                                    txtRemTrans.Text = PaidTrans.ToString();
                    //                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                    //                                }

                    //                            }
                    //                        }

                    //                    }

                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                    //                    if (RecCount > 0)
                    //                    {
                    //                        if (PostUsagePriv != null)
                    //                        {
                    //                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                btnSave.Enabled = true;
                    //                            else
                    //                                btnSave.Enabled = false;
                    //                        }
                    //                        btnSave.Enabled = true;
                    //                    }
                    //                    else
                    //                        btnSave.Enabled = false;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else if (((ListItem)cmbLump.SelectedItem).Value.ToString() == "Q")
                    //    {
                    //        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].ReadOnly == false)
                    //        {
                    //            bool IsFalse = true;
                    //            IsCal = false;
                    //            string strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                    //            //string strType = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvPrimorSeco"].Value.ToString();
                    //            int introwindex = dgvMonthsGrid.CurrentCell.RowIndex;
                    //            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].ReadOnly = true;

                    //            var PCount = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                    //                   .Count(row => (row.Cells["gvPrimorSeco"].Value.ToString() == strType));

                    //            //if (PCount > 4)
                    //            //{
                    //            //    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //            //    strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                    //            //}
                    //            //else
                    //            {
                    //                if (strdata.ToUpper() == "TRUE")
                    //                {
                    //                    RecCount++;
                    //                    string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString();
                    //                    decimal OrigBal = 0;
                    //                    if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                    {
                    //                        this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                    //                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = UsageAmount;
                    //                        this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                    //                        decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                        {
                    //                            if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                    //                                OrigBal = Convert.ToDecimal(txtBalance.Text.Trim());
                    //                        }

                    //                        isAlert = false;
                    //                        if (IsCal == false)
                    //                            CalculateAmounts(introwindex);

                    //                        decimal Balance = 0;
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                            Balance = Convert.ToDecimal(txtBalance.Text.Trim());// - Amount;

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Amount > 0)
                    //                            {
                    //                                int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                    //                                int PaidTrans = 0;
                    //                                if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                    //                                {
                    //                                    if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                    //                                        PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                    //                                    PaidTrans++;

                    //                                    if (PaidTrans > MaxTrans)
                    //                                    {
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //                                        AlertBox.Show("We can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                    //                                        IsFalse = false;
                    //                                    }
                    //                                    else
                    //                                    {

                    //                                    }

                    //                                }

                    //                                if (Balance <= 0)
                    //                                {
                    //                                    if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                    //                                    {
                    //                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = txtBalance.Text;
                    //                                        txtBalance.Text = "0";
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString() != OrigBal.ToString())
                    //                                        {
                    //                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                    //                                            if (isAlert == false)
                    //                                                AlertBox.Show("we can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                    //                                            IsFalse = false;
                    //                                        }
                    //                                    }
                    //                                }
                    //                                else
                    //                                {

                    //                                }

                    //                                if (IsFalse)
                    //                                {
                    //                                    txtRemTrans.Text = PaidTrans.ToString();
                    //                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                    //                                    if (Balance >= 0)
                    //                                        txtBalance.Text = Balance.ToString();
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                    //                                AlertBox.Show("You cannot post invoice with Amount Paid of zero", MessageBoxIcon.Warning);
                    //                                IsFalse = false;
                    //                            }
                    //                        }

                    //                    }

                    //                    if (IsFalse)
                    //                    {
                    //                        string IntakeDate = _baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                    //                        string Month = GetMonth(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvMonthsCode"].Value.ToString().Trim());
                    //                        string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                    //                        int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                    //                        if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                    //                        {
                    //                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                    //                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrUsageDate;
                    //                            if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrDate;

                    //                            if (PostUsagePriv != null)
                    //                            {
                    //                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                    //                                }
                    //                                else
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                                }
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                    //                            string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();
                    //                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrDate;

                    //                            if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrUsageDate;

                    //                            if (PostUsagePriv != null)
                    //                            {
                    //                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                    //                                }
                    //                                else
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                                }
                    //                            }
                    //                        }

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                    //                            {
                    //                                dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                    //                                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                                //{
                    //                                //    decimal uAmount = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                    //                                //    .Where(row => !row.IsNewRow && (row.Cells[1].Style.ForeColor != Color.OrangeRed)
                    //                                //                && (row.Cells["gvPrimorSeco"].Value.ToString() == strType)) // && !(row.Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE" && row.Cells["gvkChk"].ReadOnly == true)
                    //                                //       .Sum(row =>
                    //                                //       {
                    //                                //           var cellValue = row.Cells["gvUsage"].Value;
                    //                                //           if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                    //                                //           {
                    //                                //               return 0m;
                    //                                //           }

                    //                                //           if (decimal.TryParse(cellValue.ToString(), out decimal amount))
                    //                                //           {
                    //                                //               return amount;
                    //                                //           }

                    //                                //           return 0m;
                    //                                //       });

                    //                                //    if (uAmount > Convert.ToDecimal(SPM_Entity.SPM_Balance))
                    //                                //    {
                    //                                //        uAmount = Convert.ToDecimal(SPM_Entity.SPM_Balance);
                    //                                //    }

                    //                                //    decimal balat = Convert.ToDecimal(txtBalance.Text) + Convert.ToDecimal(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString());
                    //                                //    txtBalance.Text = balat.ToString();
                    //                                //    //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString()


                    //                                //    decimal _balAmt = Convert.ToDecimal(txtBalance.Text) - Convert.ToDecimal(uAmount);
                    //                                //    txtBalance.Text = _balAmt.ToString();
                    //                                //    this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                    //                                //    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = uAmount.ToString();
                    //                                //    this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);


                    //                                //    if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                                //    {
                    //                                //        for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                    //                                //        {
                    //                                //            if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                    //                                //            {
                    //                                //                if (dgvMonthsGrid.Rows[x].Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE")
                    //                                //                {
                    //                                //                    //dgvMonthsGrid.Rows[x].Cells["gvtOverride"].Value = BlankImg;
                    //                                //                }
                    //                                //                else
                    //                                //                {
                    //                                //                    if (dgvMonthsGrid.Rows[x].Cells["gvPrimorSeco"].Value.ToString() == strType)
                    //                                //                    {
                    //                                //                        dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = true);
                    //                                //                        dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                    //                                //                    }
                    //                                //                }
                    //                                //            }
                    //                                //        }
                    //                                //    }

                    //                                //}

                    //                            }
                    //                            else
                    //                            {
                    //                                decimal cellAmount = 0;
                    //                                if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                                    cellAmount = Convert.ToDecimal(UsageAmount.Trim());

                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells.ForEach(x => x.Style.ForeColor = Color.Gray);
                    //                                dgvMonthsGrid.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.OrangeRed;


                    //                                txtBalance.Text = OrigBal.ToString();
                    //                                AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()), MessageBoxIcon.Warning);
                    //                            }
                    //                        }
                    //                    }
                    //                    if (RecCount > 0)
                    //                    {
                    //                        if (PostUsagePriv != null)
                    //                        {
                    //                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                btnSave.Enabled = true;
                    //                            else
                    //                                btnSave.Enabled = false;

                    //                        }
                    //                    }
                    //                    else
                    //                        btnSave.Enabled = false;
                    //                }
                    //                else
                    //                {
                    //                    RecCount--;

                    //                    string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString();
                    //                    if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                    //                    {
                    //                        decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                    //                        decimal Balance = 0;
                    //                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                            Balance = Convert.ToDecimal(txtBalance.Text.Trim());
                    //                        if (dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Red)
                    //                        {
                    //                            if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                    //                                Balance = Convert.ToDecimal(txtBalance.Text.Trim()) + Amount;
                    //                        }

                    //                        if (txtServCode.Text == ServiceCode)
                    //                        {
                    //                            if (Balance <= 0)
                    //                            {
                    //                                AlertBox.Show("We can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                    //                                IsFalse = false;
                    //                            }
                    //                            else
                    //                            {
                    //                                txtBalance.Text = Balance.ToString();
                    //                                if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                    //                                {
                    //                                    for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                    //                                    {
                    //                                        if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                    //                                        {
                    //                                            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = false);
                    //                                            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Black);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }


                    //                            if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                    //                            {
                    //                                int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                    //                                int PaidTrans = 0;
                    //                                if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                    //                                    PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                    //                                PaidTrans--;

                    //                                if (PaidTrans > MaxTrans)
                    //                                {
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                    //                                    AlertBox.Show("we can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                    //                                    IsFalse = false;
                    //                                }
                    //                                else
                    //                                {
                    //                                    txtRemTrans.Text = PaidTrans.ToString();
                    //                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                    //                                }

                    //                            }
                    //                        }

                    //                    }

                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                    //                    dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                    //                    if (RecCount > 0)
                    //                    {
                    //                        if (PostUsagePriv != null)
                    //                        {
                    //                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //                                btnSave.Enabled = true;
                    //                            else
                    //                                btnSave.Enabled = false;
                    //                        }
                    //                        btnSave.Enabled = true;
                    //                    }
                    //                    else
                    //                        btnSave.Enabled = false;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    #endregion
                    {
                        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].ReadOnly == false)
                        {
                            bool IsFalse = true;
                            IsCal = false;
                            string strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                            string strMonth = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvMonthsCode"].Value.ToString();
                            int introwindex = dgvMonthsGrid.CurrentCell.RowIndex;
                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].ReadOnly = true;
                            string strPrimElec = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvPrimorSeco"].Value.ToString();
                            string strVendType = string.Empty;
                            if (strPrimElec == "P") strVendType = "E"; else strVendType = "O";
                            if (strPrimElec == "P" && strdata == "True") PCnt++; else if (strPrimElec == "P" && strdata == "False") PCnt--;
                            if (strPrimElec == "S" && strdata == "True") SCnt++; else if (strPrimElec == "S" && strdata == "False") SCnt--;

                            if (strMonth == "CUR") dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].ReadOnly = false;


                            pnlPVendCounts.Visible = false; pnlSVendCounts.Visible = false;

                            if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y") //&& strMonth!="CUR"
                            {
                                // if (CEAPCNTL_List[0].CPCT_USER_CNTL=="Y" && (strMonth == "CUR")) dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].ReadOnly = false;
                                pnlVendCounts.Visible = true;

                                if (IsPrimary) pnlPVendCounts.Visible = true;
                                if (IsSecondary) pnlSVendCounts.Visible = true;


                                string TotInv = string.Empty;

                                List<CASEACTEntity> SelListEntity = SP_Activity_Details.FindAll(u => u.ACT_Code.Trim() == txtServCode.Text.Trim() && u.Elec_Other == strVendType);
                                if (((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "A")
                                {
                                    if (SelListEntity.Count >= 1)
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString(); if (txtInvBal.Text == "0" || txtMaxInv.Text == txtTotInv.Text)
                                        if (strdata == "True")  //&& txtInvBal.Text!="0"
                                            AlertBox.Show("We can't allow the “Invoice” to exceed the Lump Sum Interval!!", MessageBoxIcon.Warning);
                                    }

                                    if (PCnt > 12 && strPrimElec == "P")
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True")  //&& txtInvBal.Text != "0"
                                            AlertBox.Show("Exceeding the Annual Limit 12 for the Vendor " + dgvMonthsGrid.Rows[e.RowIndex].Cells["gvVendor"].Value.ToString().Trim() + "!!", MessageBoxIcon.Warning);
                                    }
                                    if (SCnt > 12 && strPrimElec == "S")
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True")  //&& txtInvBal.Text != "0"
                                            AlertBox.Show("Exceeding the Annual Limit 12 for the Vendor " + dgvMonthsGrid.Rows[e.RowIndex].Cells["gvVendor"].Value.ToString().Trim() + "!!", MessageBoxIcon.Warning);
                                    }
                                    TotInv = "/12";

                                }
                                else if (((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "S")
                                {
                                    if (SelListEntity.Count >= 2)
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True") //&& txtInvBal.Text != "0"
                                            AlertBox.Show("We can't allow the “Invoice” to exceed the Lump Sum Interval!!", MessageBoxIcon.Warning);
                                    }
                                    if (PCnt > 6 && strPrimElec == "P")
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True")  //&& txtInvBal.Text != "0"
                                            AlertBox.Show("Exceeding the Semi-Annual Limit 6 for the Vendor " + dgvMonthsGrid.Rows[e.RowIndex].Cells["gvVendor"].Value.ToString().Trim() + "!!", MessageBoxIcon.Warning);
                                    }
                                    if (SCnt > 6 && strPrimElec == "S")
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True") //&& txtInvBal.Text != "0"
                                            AlertBox.Show("Exceeding the Annual Limit 6 for the Vendor " + dgvMonthsGrid.Rows[e.RowIndex].Cells["gvVendor"].Value.ToString().Trim() + "!!", MessageBoxIcon.Warning);
                                    }
                                    TotInv = "/6";
                                }
                                else if (((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "Q")
                                {
                                    if (SelListEntity.Count >= 4)
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True") //&& txtInvBal.Text != "0"
                                            AlertBox.Show("We can't allow the “Invoice” to exceed the Lump Sum Interval!!", MessageBoxIcon.Warning);
                                    }

                                    if (PCnt > 3 && strPrimElec == "P")
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True") //&& txtInvBal.Text != "0"
                                            AlertBox.Show("Exceeding the Quarterly Limit 3 for the Vendor " + dgvMonthsGrid.Rows[e.RowIndex].Cells["gvVendor"].Value.ToString().Trim() + "!!", MessageBoxIcon.Warning);
                                    }
                                    if (SCnt > 3 && strPrimElec == "S")
                                    {
                                        //dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                        //strdata = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value.ToString();
                                        if (strdata == "True")  //&& txtInvBal.Text != "0"
                                            AlertBox.Show("Exceeding the Quarterly Limit 3 for the Vendor " + dgvMonthsGrid.Rows[e.RowIndex].Cells["gvVendor"].Value.ToString().Trim() + "!!", MessageBoxIcon.Warning);
                                    }
                                    TotInv = "/3";
                                }

                                

                                txtPVendCounts.Text = PCnt.ToString() + TotInv; txtSVendCounts.Text = SCnt.ToString() + TotInv;

                            }

                            //if (strdata.ToUpper() == "TRUE" && strMonth == "CUR")
                            //    RecCount++;

                            if (strdata.ToUpper() == "TRUE" )
                            {
                                RecCount++;
                                                               

                                if (strMonth == "CUR")
                                {
                                    if (RecCount > 0)
                                    {
                                        if (PostUsagePriv != null)
                                        {
                                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                                btnSave.Enabled = true;
                                            //else
                                            //    btnSave.Enabled = false;

                                        }
                                    }
                                    //else
                                    //    btnSave.Enabled = false;

                                    return;
                                }
                                    

                                string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvUsage"].Value.ToString();
                                decimal OrigBal = 0;
                                if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                                {
                                    this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = UsageAmount;
                                    this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                                    decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                                    if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                    {
                                        if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                                            OrigBal = Convert.ToDecimal(txtBalance.Text.Trim());
                                    }

                                    isAlert = false;
                                    if (IsCal == false)
                                        CalculateAmounts(introwindex);

                                    decimal Balance = 0;
                                    if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                        Balance = Convert.ToDecimal(txtBalance.Text.Trim());// - Amount;

                                    if (txtServCode.Text == ServiceCode)
                                    {
                                        if (Amount > 0)
                                        {
                                            int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                                            int PaidTrans = 0;
                                            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y" || (CEAPCNTL_List[0].CPCT_LUMP_SUM=="Y" && CEAPCNTL_List[0].CPCT_USER_CNTL=="Y" && ((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "M"))
                                            //{
                                                if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                                                {
                                                    if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                                                        PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                                                    PaidTrans++;

                                                    if (PaidTrans > MaxTrans)
                                                    {
                                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                                        AlertBox.Show("We can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                                                        IsFalse = false;
                                                    }
                                                    else
                                                    {

                                                    }

                                                }
                                            //}
                                            //Commented the code on 01/23/25
                                            //if (((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "A" || ((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "S" || ((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "Q")
                                            //{
                                            //    if (txtInvBal.Text == "0" || txtMaxInv.Text == txtTotInv.Text)
                                            //    {
                                            //        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                                            //        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                            //        AlertBox.Show("We can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                                            //        IsFalse = false;
                                            //    }
                                            //}

                                            if (Balance <= 0)
                                            {
                                                if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                                                {
                                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = txtBalance.Text;
                                                    txtBalance.Text = "0";
                                                }
                                                else
                                                {
                                                    if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString() != OrigBal.ToString())
                                                    {
                                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                                                        if (isAlert == false)
                                                            AlertBox.Show("we can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                                                        IsFalse = false;
                                                    }
                                                }
                                            }
                                            else
                                            {

                                            }

                                            if (IsFalse)
                                            {
                                                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
                                                {
                                                    txtRemTrans.Text = PaidTrans.ToString();
                                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                                                }
                                                if (Balance >= 0)
                                                    txtBalance.Text = Balance.ToString();
                                            }
                                        }
                                        else
                                        {
                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;

                                            AlertBox.Show("You cannot post invoice with Amount Paid of zero", MessageBoxIcon.Warning);
                                            IsFalse = false;
                                        }
                                    }

                                }

                                if (IsFalse)
                                {

                                    string IntakeDate = _spmentity.startdate.Trim();//_baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                                    string Month = GetMonth(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvMonthsCode"].Value.ToString().Trim());

                                    //if (Month != "CUR")
                                    //{
                                        string StrDate = string.Empty;

                                        if(Month=="CUR")
                                            StrDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                                        else
                                            StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                                        int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                                    if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                                    {
                                        string StrUsageDate = string.Empty;

                                        if (Month == "CUR")
                                            StrUsageDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + "1/" + (IntakeYear)).ToShortDateString();
                                        //StrUsageDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                                        else
                                            StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                                        //StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();

                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrUsageDate;
                                        if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrDate;

                                        if (PostUsagePriv != null)
                                        {
                                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                            {
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                            }
                                            else
                                            {
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                                            }
                                        }
                                    }
                                    else if (Convert.ToDateTime(IntakeDate.Trim()).Month == Convert.ToDateTime(StrDate.Trim()).Month)   
                                    {
                                        string StrUsageDate = string.Empty;

                                        if (Month == "CUR")
                                            StrUsageDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + "1/" + (IntakeYear)).ToShortDateString();
                                        //StrUsageDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                                        else
                                            StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear)).ToShortDateString();
                                        //StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();

                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrUsageDate;
                                        if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = IntakeDate;

                                        if (PostUsagePriv != null)
                                        {
                                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                            {
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                            }
                                            else
                                            {
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                                        string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();
                                        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrDate;

                                        if (string.IsNullOrEmpty(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                                            dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrUsageDate;

                                        if (PostUsagePriv != null)
                                        {
                                            if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                            {
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                            }
                                            else
                                            {
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                                            }
                                        }
                                    }

                                        if (txtServCode.Text == ServiceCode)
                                        {
                                            if (Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                                            {
                                                dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                                                //         if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                                                //         {
                                                //             decimal uAmount = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                                                //.Where(row => !row.IsNewRow && (row.Cells[1].Style.ForeColor != Color.OrangeRed)) // && !(row.Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE" && row.Cells["gvkChk"].ReadOnly == true)
                                                //                .Sum(row =>
                                                //                {
                                                //                    var cellValue = row.Cells["gvUsage"].Value;
                                                //                    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                                                //                    {
                                                //                        return 0m;
                                                //                    }

                                                //                    if (decimal.TryParse(cellValue.ToString(), out decimal amount))
                                                //                    {
                                                //                        return amount;
                                                //                    }

                                                //                    return 0m;
                                                //                });

                                                //             if (uAmount > Convert.ToDecimal(SPM_Entity.SPM_Balance))
                                                //             {
                                                //                 uAmount = Convert.ToDecimal(SPM_Entity.SPM_Balance);
                                                //             }
                                                //             decimal _balAmt = Convert.ToDecimal(SPM_Entity.SPM_Balance) - Convert.ToDecimal(uAmount);
                                                //             txtBalance.Text = _balAmt.ToString();
                                                //             this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                                                //             dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = uAmount.ToString();
                                                //             this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);


                                                //             if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                                                //             {
                                                //                 for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                                                //                 {
                                                //                     if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                                                //                     {
                                                //                         if (dgvMonthsGrid.Rows[x].Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE")
                                                //                         {
                                                //                             //dgvMonthsGrid.Rows[x].Cells["gvtOverride"].Value = BlankImg;
                                                //                         }
                                                //                         else
                                                //                         {
                                                //                             dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = true);
                                                //                             dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                                                //                         }
                                                //                     }
                                                //                 }
                                                //             }

                                                //         }

                                            }
                                            else
                                            {
                                                decimal cellAmount = 0;
                                                if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                                                    cellAmount = Convert.ToDecimal(UsageAmount.Trim());

                                                dgvMonthsGrid.Rows[e.RowIndex].Cells.ForEach(x => x.Style.ForeColor = Color.Gray);
                                                dgvMonthsGrid.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.OrangeRed;


                                                txtBalance.Text = OrigBal.ToString();
                                                AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()), MessageBoxIcon.Warning);
                                            }
                                        }
                                    //}
                                }
                                if (RecCount > 0)
                                {
                                    if (PostUsagePriv != null)
                                    {
                                        if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                            btnSave.Enabled = true;
                                        //else
                                        //    btnSave.Enabled = false;

                                    }
                                }
                                //else
                                //    btnSave.Enabled = false;
                            }
                            else
                            {
                                if(RecCount>0)
                                    RecCount--;

                                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y")
                                //{
                                //    if (strPrimElec == "P") PCnt--;
                                //    if (strPrimElec == "S") SCnt--;
                                //}

                                string UsageAmount = dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value.ToString();
                                if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                                {
                                    decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                                    decimal Balance = 0;
                                    if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                        Balance = Convert.ToDecimal(txtBalance.Text.Trim());
                                    if (dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Red)
                                    {
                                        if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                            Balance = Convert.ToDecimal(txtBalance.Text.Trim()) + Amount;
                                    }

                                    if (txtServCode.Text == ServiceCode)
                                    {
                                        //int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                                        //int PaidTrans = 0;
                                        //if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                                        //{
                                        //    if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim())) PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                                        //    PaidTrans++;

                                        //    if (PaidTrans > MaxTrans)
                                        //    {
                                        //        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = string.Empty;
                                        //        dgvMonthsGrid.Rows[e.RowIndex].Cells["gvkChk"].Value = false;
                                        //        AlertBox.Show("we can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                                        //        IsFalse = false;
                                        //    }
                                        //    else
                                        //    {

                                        //    }

                                        //}

                                        if (Balance <= 0)
                                        {
                                            AlertBox.Show("We can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                                            IsFalse = false;
                                        }
                                        else
                                        {
                                            txtBalance.Text = Balance.ToString();
                                            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                                            //{
                                            //    for (int x = 0; x < dgvMonthsGrid.Rows.Count; x++)
                                            //    {
                                            //        if (dgvMonthsGrid.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                                            //        {
                                            //            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.ReadOnly = false);
                                            //            dgvMonthsGrid.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Black);
                                            //        }
                                            //    }
                                            //}
                                        }


                                        if (!string.IsNullOrEmpty(txtMaxInv.Text.Trim()))
                                        {
                                            //Commented by Sudheer on 01/23/25
                                            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y" || CEAPCNTL_List[0].CPCT_USER_CNTL.ToString() == "Y" && ((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "M")
                                            //{
                                                int MaxTrans = int.Parse(txtMaxInv.Text.ToString());
                                                int PaidTrans = 0;
                                                if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim()))
                                                    PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                                                PaidTrans--;

                                                if (PaidTrans > MaxTrans)
                                                {
                                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                                                    dgvMonthsGrid.Rows[e.RowIndex].Cells["gvSelect"].Value = false;
                                                    AlertBox.Show("We can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                                                    IsFalse = false;
                                                }
                                                else
                                                {
                                                    txtRemTrans.Text = PaidTrans.ToString();
                                                    txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                                                }
                                            //}
                                        }
                                    }

                                }

                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = string.Empty;
                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtServicedate"].Value = string.Empty;
                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                                dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].Value = string.Empty;
                                //if(strMonth=="CUR") dgvMonthsGrid.Rows[e.RowIndex].Cells["gvAmtPaid"].ReadOnly=true;
                                dgvMonthsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                                if (RecCount > 0)
                                {
                                    if (PostUsagePriv != null)
                                    {
                                        if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                            btnSave.Enabled = true;
                                        //else
                                        //    btnSave.Enabled = false;
                                    }
                                    btnSave.Enabled = true;
                                }
                                //else
                                //    btnSave.Enabled = false;
                            }

                           
                        }
                    }
                }

                /*if (e.ColumnIndex == gvtOverride.Index && e.RowIndex != -1)
                {
                    if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvtOverride"].Value.ToString() == Img_Edit)
                    {
                        if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvCaseact"].Value.ToString() == "Y")
                        {
                            CASEACTEntity Entity = dgvMonthsGrid.Rows[e.RowIndex].Tag as CASEACTEntity;

                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.ChangePriv.ToUpper() == "TRUE")
                                {
                                    if (string.IsNullOrEmpty(Entity.BundleNo.Trim()))
                                    {
                                        CASE0016_UsageEditForm EditForm = new CASE0016_UsageEditForm(_baseForm, "Edit", SPM_Entity, Entity, CEAPCNTL_List[0], txtBalance.Text.Trim(), _privilegeEntity);
                                        EditForm.FormClosed += new FormClosedEventHandler(On_Edit_Form_Closed);
                                        EditForm.StartPosition = FormStartPosition.CenterScreen;
                                        EditForm.ShowDialog();
                                    }

                                }
                            }

                        }
                        else
                        {
                            CASEACTEntity Entity = new CASEACTEntity();
                            Entity.Service_plan = SP_Code;
                            Entity.ACT_Code = txtServCode.Text;
                            Entity.ACT_Date = dgvMonthsGrid.CurrentRow.Cells["gvtServicedate"].Value.ToString();
                            Entity.Cost = dgvMonthsGrid.CurrentRow.Cells["gvtAmtPaid"].Value.ToString();
                            Entity.PDOUT = dgvMonthsGrid.CurrentRow.Cells["gvPDOut"].Value.ToString();
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                {
                                    CASE0016_UsageEditForm EditForm = new CASE0016_UsageEditForm(_baseForm, "Add", SPM_Entity, Entity, CEAPCNTL_List[0], txtBalance.Text.Trim(), _privilegeEntity);
                                    EditForm.FormClosed += new FormClosedEventHandler(On_Edit_Form_Closed);
                                    EditForm.StartPosition = FormStartPosition.CenterScreen;
                                    EditForm.ShowDialog();
                                }
                            }
                        }
                    }
                }*/

                if (e.ColumnIndex == gvDelete.Index && e.RowIndex != -1)
                {
                    if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvCaseact"].Value.ToString() == "Y")
                    {
                        CASEACTEntity Entity = dgvMonthsGrid.Rows[e.RowIndex].Tag as CASEACTEntity;

                        if (string.IsNullOrEmpty(Entity.BundleNo.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                                {
                                    MessageBox.Show("Are you sure you want to delete " + dgvMonthsGrid.Rows[e.RowIndex].Cells["gvMonth"].Value.ToString() + " Invoice?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Selected_CAMS);
                                }
                            }
                        }

                    }
                }

                dgvMonthsGrid.CellValueChanged += new DataGridViewCellEventHandler(dgvMonthsGrid_CellValueChanged);
            }

        }

        private void Delete_Selected_CAMS(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                bool Delete_Status = true;
                int New_MS_ID = 0, Tmp_CA_Seq = 0;
                bool VouchMSG = false;
                int CountCA = 0, CountMS = 0;

                CASEACTEntity Entity = dgvInvoicePosting.CurrentRow.Tag as CASEACTEntity;
                //**Entity.Elec_Other = dgvMonthsGrid.CurrentRow.Cells["gvElecorOther"].Value.ToString();
                Entity.Rec_Type = "D";

                if (!_model.SPAdminData.UpdateCASEACT2(Entity, "Delete", out New_MS_ID, out Tmp_CA_Seq, out Sql_SP_Result_Message))
                    Delete_Status = false;

                if (Delete_Status)
                {
                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Entity.Fund1);

                    if (Emsbdc_List.Count > 0)
                        Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Entity.BDC_ID);
                    CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                    if (emsbdcentity != null)
                    {
                        CMBDCEntity emsbdcdata = new CMBDCEntity();
                        emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                        emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                        emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                        emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                        emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                        emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                        emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                        emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                        emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                        emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                        emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                        emsbdcdata.Mode = "BdcAmount";
                        string strstatus = string.Empty;
                        if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                        {
                        }
                    }

                    AlertBox.Show("Invoice Deleted Successfully");
                    Fill_Applicant_SPs();
                    Get_App_CASEACT_List(SPM_Entity);
                    FillMonthsGrid();
                    FillInvPostingGrid();
                    FillServiceTextbox();
                }


            }
        }

        private void dgvMonthsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvMonthsGrid.Rows.Count > 0)
                {
                    if (e.ColumnIndex == gvAmtPaid.Index)
                    {
                        int introwindex = dgvMonthsGrid.CurrentCell.RowIndex;
                        int intcolumnindex = dgvMonthsGrid.CurrentCell.ColumnIndex;
                        string strAmtValue = Convert.ToString(dgvMonthsGrid.Rows[introwindex].Cells["gvAmtPaid"].Value);
                        string strmonth = dgvMonthsGrid.Rows[introwindex].Cells["gvMonthsCode"].Value.ToString(); 
                        dgvMonthsGrid.Rows[introwindex].Cells["gvAmtPaid"].Selected = true;

                        if (!string.IsNullOrEmpty(strAmtValue))
                        {
                            if (CommonFunctions.IsNumeric(strAmtValue.Trim()))
                            {
                                if (Convert.ToDecimal(strAmtValue) < 1 && Convert.ToDecimal(strAmtValue) > 0)
                                {
                                }
                                else
                                {
                                    if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                    {
                                        dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    }
                                    else
                                    {
                                        if (strAmtValue.Length > 6)
                                        {
                                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalRange6String))
                                            {
                                                dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = "999999.99";
                                            }
                                        }
                                        else
                                        {
                                            if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                            {
                                                dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                {
                                    dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                }
                            }
                            strAmtValue = Convert.ToString(dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value);
                            //if (!string.IsNullOrEmpty(strAmtValue.Trim()) && strmonth != "CUR")
                            //{
                                if (txtServCode.Text == ServiceCode)
                                    CalculateAmounts(introwindex);
                            //}
                        }
                        

                    }
                    if (e.ColumnIndex == gvtServicedate.Index)
                    {
                        int introwindex = dgvMonthsGrid.CurrentCell.RowIndex;
                        int intcolumnindex = dgvMonthsGrid.CurrentCell.ColumnIndex;
                        string strIntervalValue = Convert.ToString(dgvMonthsGrid.Rows[introwindex].Cells["gvtServicedate"].Value);
                        string strCurrectValue = Convert.ToString(dgvMonthsGrid.Rows[introwindex].Cells["gvtServicedate"].Value);
                        if (strCurrectValue.Length > 10)
                            strCurrectValue = Convert.ToDateTime(strCurrectValue).ToShortDateString();
                                // strCurrectValue.Substring(0, 10);

                        strCurrectValue = strCurrectValue.Replace("_", "").Trim();
                        strCurrectValue = strCurrectValue.Replace(" ", "").Trim();

                        if ((!string.IsNullOrEmpty(strCurrectValue)) && strCurrectValue.Trim() != "/  /")
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                            {
                                try
                                {
                                    if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                                    {
                                        AlertBox.Show("01/01/1800 below date not except", MessageBoxIcon.Warning);
                                        dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;

                                    }
                                    else
                                    {
                                        dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                        AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);

                                    }
                                }
                                catch (Exception)
                                {
                                    dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);

                                }
                            }
                            else
                            {
                                bool booldatevalid = true;
                                string IsLeap = "N";
                                if (DateTime.IsLeapYear(int.Parse(strCurrectValue.Substring(6, 4))))
                                    IsLeap = "Y";

                                if ((strCurrectValue.ToString().Substring(0, 2) == "02") && ((IsLeap == "N" && strCurrectValue.ToString().Substring(3, 2) == "29") || strCurrectValue.ToString().Substring(3, 2) == "30" || strCurrectValue.ToString().Substring(3, 2) == "31"))
                                {
                                    dgvMonthsGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);
                                    booldatevalid = false;
                                }

                                if (booldatevalid)
                                {
                                    if (txtServCode.Text == ServiceCode)
                                    {
                                        if (Convert.ToDateTime(strCurrectValue.Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(strCurrectValue.Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                                        {
                                            if (dgvMonthsGrid.Rows[introwindex].Cells["gvCaseact"].Value.ToString() == "Y")
                                                dgvMonthsGrid.Rows[introwindex].DefaultCellStyle.BackColor = Color.White;
                                            else
                                                dgvMonthsGrid.Rows[introwindex].DefaultCellStyle.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            dgvMonthsGrid.Rows[introwindex].DefaultCellStyle.BackColor = Color.Red;
                                            AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()));
                                            booldatevalid = false;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { }
        }

        private void dgvMonthsGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvMonthsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMonthsGrid.Rows.Count > 0)
            {
                string strcmbusage = dgvMonthsGrid.CurrentRow.Cells["gvUsage"].Value == null ? string.Empty : dgvMonthsGrid.CurrentRow.Cells["gvUsage"].Value.ToString();
                string strcmbAmount = dgvMonthsGrid.CurrentRow.Cells["gvAmtPaid"].Value == null ? string.Empty : dgvMonthsGrid.CurrentRow.Cells["gvAmtPaid"].Value.ToString();

                dgvMonthsGrid.CurrentRow.Cells["gvtServicedate"].ReadOnly = true;
                //dgvMonthsGrid.CurrentRow.Cells["gvAmtPaid"].ReadOnly = true;
            }
        }

        CASEACTEntity Pass_CA_Entity = new CASEACTEntity();
        private void Get_Latest_Activity_data(DataGridViewRow dr)
        {
            if (Pass_CA_Entity.Rec_Type == "I")
            {
                Pass_CA_Entity.Agency = _baseForm.BaseAgency;
                Pass_CA_Entity.Dept = _baseForm.BaseDept;
                Pass_CA_Entity.Program = _baseForm.BaseProg;
                Pass_CA_Entity.Year = _baseForm.BaseYear;
                Pass_CA_Entity.App_no = _baseForm.BaseApplicationNo;
                Pass_CA_Entity.Service_plan = _spCode;
                Pass_CA_Entity.SPM_Seq = _spSequence;

                Pass_CA_Entity.Check_Date = Pass_CA_Entity.Followup_Comp = Pass_CA_Entity.Followup_On =
                Pass_CA_Entity.Fund2 =
                Pass_CA_Entity.Fund3 = Pass_CA_Entity.Check_No = Pass_CA_Entity.Refer_Data = Pass_CA_Entity.Rate =Pass_CA_Entity.Bulk=
                Pass_CA_Entity.Cust_Code1 = Pass_CA_Entity.Cust_Value1 = Pass_CA_Entity.Account = Pass_CA_Entity.Amount = Pass_CA_Entity.Amount2 = Pass_CA_Entity.Amount3 =
                Pass_CA_Entity.Cust_Code2 = Pass_CA_Entity.Cust_Value2 = Pass_CA_Entity.ArrearsAmt = Pass_CA_Entity.BillingPeriod = Pass_CA_Entity.BundleNo = Pass_CA_Entity.CA_VEND_PAY_CAT =
                Pass_CA_Entity.Cust_Code3 = Pass_CA_Entity.Cust_Value3 = Pass_CA_Entity.LVL1Apprval = Pass_CA_Entity.LVL1AprrvalDate = Pass_CA_Entity.LVL2Apprval = Pass_CA_Entity.LVL2ApprvalDate =
                Pass_CA_Entity.Cust_Code4 = Pass_CA_Entity.Cust_Value4 = Pass_CA_Entity.SentPmtDate = Pass_CA_Entity.SentPmtUser = Pass_CA_Entity.UOM2 = Pass_CA_Entity.UOM3 =
                Pass_CA_Entity.Cust_Code5 = Pass_CA_Entity.Cust_Value5 =
                Pass_CA_Entity.UOM = Pass_CA_Entity.Units = null;

                Pass_CA_Entity.Add_Operator = _baseForm.UserID;
            }
            else
            {
                CASEACTEntity Entity = dr.Tag as CASEACTEntity;
                if (Entity != null)
                    Pass_CA_Entity = Entity;
            }

            if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
            //    Pass_CA_Entity.ACT_Date = DateTime.Now.ToShortDateString();
            //else
            //    Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString();

            Pass_CA_Entity.Caseworker = SPM_Entity.caseworker;
            Pass_CA_Entity.Site = SPM_Entity.site;
            Pass_CA_Entity.Act_PROG = SPM_Entity.Def_Program;
            Pass_CA_Entity.Branch = txtBranch.Text;
            Pass_CA_Entity.Group = txtGroup.Text;
            Pass_CA_Entity.ACT_Code = txtServCode.Text;
            Pass_CA_Entity.BenefitReason = SPM_Entity.Spm_Benefit_Reasn;

            Pass_CA_Entity.CA_OBF = "2";

            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y" )
            //{
            //    Pass_CA_Entity.ACT_Date = DateTime.Now.ToShortDateString();
            //}
            //else
            //{
                if (!string.IsNullOrEmpty(dr.Cells["gvtServicedate"].Value.ToString()))
                    Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString();

                if (!string.IsNullOrEmpty(dr.Cells["gvtUsageReq"].Value.ToString()))
                    Pass_CA_Entity.ActSeek_Date = dr.Cells["gvtUsageReq"].Value.ToString();

                if (!string.IsNullOrEmpty(dr.Cells["gvPDOut"].Value.ToString()))
                    Pass_CA_Entity.PDOUT = dr.Cells["gvPDOut"].Value.ToString();
            //}

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_Fund.Trim()))
                Pass_CA_Entity.Fund1 = SPM_Entity.SPM_Fund;

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_BDC_ID.Trim()))
                Pass_CA_Entity.BDC_ID = SPM_Entity.SPM_BDC_ID;

            if (dr.Cells["gvMonthsCode"].Value.ToString().Trim() == "CUR")
            {
                Pass_CA_Entity.Bulk = "C";
            }

            if (dr.Cells["gvElecorOther"].Value.ToString() == "E")
            {
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Vendor;
                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() != "Y")
                Pass_CA_Entity.Cost = dr.Cells["gvAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_BillName_Type;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Bill_LName;
                Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Account;
                Pass_CA_Entity.CA_Source = _strPrimSource;
                //if (txtServCode.Text == ServiceCode)
                Pass_CA_Entity.Elec_Other = "E";
            }
            else
            {
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Gas_Vendor;
                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() != "Y")
                Pass_CA_Entity.Cost = dr.Cells["gvAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_Gas_BillName_Type;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Gas_Bill_LName;
                Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Gas_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Gas_Account;
                Pass_CA_Entity.CA_Source = _strOtherSource;
                //if (txtServCode.Text == ServiceCode)
                Pass_CA_Entity.Elec_Other = "O";
            }

            Pass_CA_Entity.Lsct_Operator = _baseForm.UserID;
        }

        private void Get_Latest_Activity_data(string VentodType)
        {
            if (Pass_CA_Entity.Rec_Type == "I")
            {
                Pass_CA_Entity.Agency = _baseForm.BaseAgency;
                Pass_CA_Entity.Dept = _baseForm.BaseDept;
                Pass_CA_Entity.Program = _baseForm.BaseProg;
                Pass_CA_Entity.Year = _baseForm.BaseYear;
                Pass_CA_Entity.App_no = _baseForm.BaseApplicationNo;
                Pass_CA_Entity.Service_plan = _spCode;
                Pass_CA_Entity.SPM_Seq = _spSequence;

                Pass_CA_Entity.Check_Date = Pass_CA_Entity.Followup_Comp = Pass_CA_Entity.Followup_On =
                Pass_CA_Entity.Fund2 =
                Pass_CA_Entity.Fund3 = Pass_CA_Entity.Check_No = Pass_CA_Entity.Refer_Data = Pass_CA_Entity.Rate =
                Pass_CA_Entity.Cust_Code1 = Pass_CA_Entity.Cust_Value1 = Pass_CA_Entity.Account = Pass_CA_Entity.Amount = Pass_CA_Entity.Amount2 = Pass_CA_Entity.Amount3 =
                Pass_CA_Entity.Cust_Code2 = Pass_CA_Entity.Cust_Value2 = Pass_CA_Entity.ArrearsAmt = Pass_CA_Entity.BillingPeriod = Pass_CA_Entity.BundleNo = Pass_CA_Entity.CA_VEND_PAY_CAT =
                Pass_CA_Entity.Cust_Code3 = Pass_CA_Entity.Cust_Value3 = Pass_CA_Entity.LVL1Apprval = Pass_CA_Entity.LVL1AprrvalDate = Pass_CA_Entity.LVL2Apprval = Pass_CA_Entity.LVL2ApprvalDate =
                Pass_CA_Entity.Cust_Code4 = Pass_CA_Entity.Cust_Value4 = Pass_CA_Entity.SentPmtDate = Pass_CA_Entity.SentPmtUser = Pass_CA_Entity.UOM2 = Pass_CA_Entity.UOM3 =
                Pass_CA_Entity.Cust_Code5 = Pass_CA_Entity.Cust_Value5 =
                Pass_CA_Entity.UOM = Pass_CA_Entity.Units = null;

                Pass_CA_Entity.Add_Operator = _baseForm.UserID;
            }
            //else
            //{
            //    CASEACTEntity Entity = dr.Tag as CASEACTEntity;
            //    if (Entity != null)
            //        Pass_CA_Entity = Entity;
            //}

            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
            //    //    Pass_CA_Entity.ACT_Date = DateTime.Now.ToShortDateString();
            //    //else
            //    //    Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString();

                Pass_CA_Entity.Caseworker = SPM_Entity.caseworker;
            Pass_CA_Entity.Site = SPM_Entity.site;
            Pass_CA_Entity.Act_PROG = SPM_Entity.Def_Program;
            Pass_CA_Entity.Branch = txtBranch.Text;
            Pass_CA_Entity.Group = txtGroup.Text;
            Pass_CA_Entity.ACT_Code = txtServCode.Text;
            Pass_CA_Entity.BenefitReason = SPM_Entity.Spm_Benefit_Reasn;

            Pass_CA_Entity.CA_OBF = "2";

            if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
            {
                Pass_CA_Entity.ACT_Date = DateTime.Now.ToShortDateString();
                Pass_CA_Entity.ActSeek_Date = DateTime.Now.ToShortDateString();
            }
            //else
            //{
            //    if (!string.IsNullOrEmpty(dr.Cells["gvtServicedate"].Value.ToString()))
            //        Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString();

            //    if (!string.IsNullOrEmpty(dr.Cells["gvtUsageReq"].Value.ToString()))
            //        Pass_CA_Entity.ActSeek_Date = dr.Cells["gvtUsageReq"].Value.ToString();

            //    if (!string.IsNullOrEmpty(dr.Cells["gvPDOut"].Value.ToString()))
            //        Pass_CA_Entity.PDOUT = dr.Cells["gvPDOut"].Value.ToString();
            //}

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_Fund.Trim()))
                Pass_CA_Entity.Fund1 = SPM_Entity.SPM_Fund;

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_BDC_ID.Trim()))
                Pass_CA_Entity.BDC_ID = SPM_Entity.SPM_BDC_ID;



            if (VentodType == "E")
            {
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Vendor;
                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() != "Y")
                //    Pass_CA_Entity.Cost = dr.Cells["gvAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_BillName_Type;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Bill_LName;
                Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Account;
                Pass_CA_Entity.CA_Source = _strPrimSource;
                //if (txtServCode.Text == ServiceCode)
                Pass_CA_Entity.Elec_Other = "E";
            }
            else
            {
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Gas_Vendor;
                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() != "Y")
                //    Pass_CA_Entity.Cost = dr.Cells["gvAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_Gas_BillName_Type;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Gas_Bill_LName;
                Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Gas_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Gas_Account;
                Pass_CA_Entity.CA_Source = _strOtherSource;
                //if (txtServCode.Text == ServiceCode)
                Pass_CA_Entity.Elec_Other = "O";
            }

            Pass_CA_Entity.Lsct_Operator = _baseForm.UserID;
        }

        private void Get_Arrears_Activity_data(string VentodType)
        {
            if (Pass_CA_Entity.Rec_Type == "I")
            {
                Pass_CA_Entity.Agency = _baseForm.BaseAgency;
                Pass_CA_Entity.Dept = _baseForm.BaseDept;
                Pass_CA_Entity.Program = _baseForm.BaseProg;
                Pass_CA_Entity.Year = _baseForm.BaseYear;
                Pass_CA_Entity.App_no = _baseForm.BaseApplicationNo;
                Pass_CA_Entity.Service_plan = _spCode;
                Pass_CA_Entity.SPM_Seq = _spSequence;

                Pass_CA_Entity.Check_Date = Pass_CA_Entity.Followup_Comp = Pass_CA_Entity.Followup_On =
                Pass_CA_Entity.Fund2 =
                Pass_CA_Entity.Fund3 = Pass_CA_Entity.Check_No = Pass_CA_Entity.Refer_Data = Pass_CA_Entity.Rate =
                Pass_CA_Entity.Cust_Code1 = Pass_CA_Entity.Cust_Value1 = Pass_CA_Entity.Account = Pass_CA_Entity.Amount = Pass_CA_Entity.Amount2 = Pass_CA_Entity.Amount3 =
                Pass_CA_Entity.Cust_Code2 = Pass_CA_Entity.Cust_Value2 = Pass_CA_Entity.ArrearsAmt = Pass_CA_Entity.BillingPeriod = Pass_CA_Entity.BundleNo = Pass_CA_Entity.CA_VEND_PAY_CAT =
                Pass_CA_Entity.Cust_Code3 = Pass_CA_Entity.Cust_Value3 = Pass_CA_Entity.LVL1Apprval = Pass_CA_Entity.LVL1AprrvalDate = Pass_CA_Entity.LVL2Apprval = Pass_CA_Entity.LVL2ApprvalDate =
                Pass_CA_Entity.Cust_Code4 = Pass_CA_Entity.Cust_Value4 = Pass_CA_Entity.SentPmtDate = Pass_CA_Entity.SentPmtUser = Pass_CA_Entity.UOM2 = Pass_CA_Entity.UOM3 =
                Pass_CA_Entity.Cust_Code5 = Pass_CA_Entity.Cust_Value5 = Pass_CA_Entity.Elec_Other=
                Pass_CA_Entity.UOM = Pass_CA_Entity.Units = null;

                Pass_CA_Entity.Add_Operator = _baseForm.UserID;
            }
            //else
            //{
            //    CASEACTEntity Entity = dr.Tag as CASEACTEntity;
            //    if (Entity != null)
            //        Pass_CA_Entity = Entity;
            //}

            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
            //    //    Pass_CA_Entity.ACT_Date = DateTime.Now.ToShortDateString();
            //    //else
            //    //    Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString();

            Pass_CA_Entity.Caseworker = SPM_Entity.caseworker;
            Pass_CA_Entity.Site = SPM_Entity.site;
            Pass_CA_Entity.Act_PROG = SPM_Entity.Def_Program;
            Pass_CA_Entity.Branch = txtBranch.Text;
            Pass_CA_Entity.Group = txtGroup.Text;
            Pass_CA_Entity.ACT_Code = txtArrearCode.Text;
            //Pass_CA_Entity.BenefitReason = SPM_Entity.Spm_Benefit_Reasn;

            Pass_CA_Entity.CA_OBF = "2";

            //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
            //{
           
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(dr.Cells["gvtServicedate"].Value.ToString()))
            //        Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString();

            //    if (!string.IsNullOrEmpty(dr.Cells["gvtUsageReq"].Value.ToString()))
            //        Pass_CA_Entity.ActSeek_Date = dr.Cells["gvtUsageReq"].Value.ToString();

            //    if (!string.IsNullOrEmpty(dr.Cells["gvPDOut"].Value.ToString()))
            //        Pass_CA_Entity.PDOUT = dr.Cells["gvPDOut"].Value.ToString();
            //}

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_Fund.Trim()))
                Pass_CA_Entity.Fund1 = SPM_Entity.SPM_Fund;

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_BDC_ID.Trim()))
                Pass_CA_Entity.BDC_ID = SPM_Entity.SPM_BDC_ID;



            if (VentodType == "E")
            {
                Pass_CA_Entity.ACT_Date = dtpPArrDte.Text.ToString();
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Vendor;
                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() != "Y")
                //    Pass_CA_Entity.Cost = dr.Cells["gvAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_BillName_Type;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Bill_LName;
                Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Account;
                Pass_CA_Entity.CA_Source = _strPrimSource;
                //if (txtServCode.Text == ServiceCode)
                //Pass_CA_Entity.Elec_Other = "E";
            }
            else
            {
                Pass_CA_Entity.ACT_Date = dtpSArrDte.Text.ToString();
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Gas_Vendor;
                //if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() != "Y")
                //    Pass_CA_Entity.Cost = dr.Cells["gvAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_Gas_BillName_Type;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Gas_Bill_LName;
                Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Gas_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Gas_Account;
                Pass_CA_Entity.CA_Source = _strOtherSource;
                //if (txtServCode.Text == ServiceCode)
                //Pass_CA_Entity.Elec_Other = "O";
            }

            Pass_CA_Entity.Lsct_Operator = _baseForm.UserID;
        }

        private bool isValidateForm()
        {
            bool isValid = true;

            if (dgvMonthsGrid.Rows.Count > 0)
            {
                if (CEAPCNTL_List[0].CPCT_LUMP_SUM != "Y")
                {


                    if (txtServCode.Text == ServiceCode)
                    {
                        foreach (DataGridViewRow dr in dgvMonthsGrid.Rows)
                        {
                            if (dr.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE")
                            {
                                if (Convert.ToDateTime(dr.Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(dr.Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                                {

                                }
                                else
                                {
                                    AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()) + " in " + dr.Cells["gvtMonthQues"].Value.ToString(), MessageBoxIcon.Warning);
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return isValid;
        }

        string Sql_SP_Result_Message = string.Empty;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtServCode.Text))
            {
                if (isValidateForm())
                {
                    if (dgvMonthsGrid.Rows.Count > 0)
                    {
                        bool IsPost = false;
                        bool IsBilling = false;
                        if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y" )
                        {
                            if (CEAPCNTL_List[0].CPCT_USER_CNTL.ToString() == "Y" && ((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "M")
                            {
                                //if(!string.IsNullOrEmpty(row.Cells["gvAmtPaid"].Value.ToString().Trim())
                                IsPost = SaveBillingInvoices(IsPost);

                                if(IsPost) IsBilling = true;
                            }
                            else
                            {
                                #region Primary Vendor Record  
                                decimal PrimAmt = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                                    .Where(row => row.Cells["gvCaseact"].Value.ToString() == "N" && row.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" //&& row.Cells["gvMonthsCode"].Value.ToString()!="CUR"
                                    && row.Cells["gvprimorSeco"].Value.ToString() == "P" && row.Cells["gvAmtPaid"].Value.ToString().Trim()!="").Sum(row =>
                                    {
                                        var cellValue = row.Cells["gvAmtPaid"].Value;
                                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                                        {
                                            return 0m;
                                        }
                                        if (decimal.TryParse(cellValue.ToString(), out decimal amount))
                                        {
                                            return amount;
                                        }
                                        return 0m;
                                    });
                                if (PrimAmt > 0)
                                {
                                    //System.Data.DataTable dt = new System.Data.DataTable();
                                    //dt.Columns.Add("ActDate", typeof(string));
                                    IsBilling = true;

                                    List<DataGridViewRow> Selrows = dgvMonthsGrid.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["gvCaseact"].Value.ToString() == "N" && row.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" //&& row.Cells["gvMonthsCode"].Value.ToString()!="CUR"
                                                                    && row.Cells["gvprimorSeco"].Value.ToString() == "P" && row.Cells["gvAmtPaid"].Value.ToString().Trim() != "")
                                                                    .OrderBy(row => (row.Cells["gvtServicedate"].Value)).ToList();

                                   
                                    DateTime MainDate = new DateTime();
                                    if (Selrows.Count > 0) 
                                    {
                                        DateTime strDate = new DateTime(); //string strActualDate = string.Empty;
                                        bool First = true;
                                        foreach (DataGridViewRow item in Selrows)
                                        {
                                            if (First) { MainDate = Convert.ToDateTime(item["gvtServicedate"].Value.ToString());First = false; }
                                            strDate = Convert.ToDateTime(item["gvtServicedate"].Value.ToString());

                                            if (MainDate >= strDate)
                                                MainDate = strDate;

                                            //System.Data.DataRow dr = dt.NewRow();
                                            //dr["ActDate"] = Convert.ToDateTime(item["gvtServicedate"].Value.ToString()).ToShortDateString();
                                            //dt.Rows.Add(dr);
                                        }
                                    }

                                    //if(dt.Rows.Count>0)
                                    //{

                                    //    IEnumerable<DataRow> orderedRows = dt.AsEnumerable()
                                    //                            .OrderBy(r => r.Field<DateTime>("ActDate"));

                                    //    System.Data.DataView dv = new System.Data.DataView(dt);
                                    //    dv.Sort= "ActDate ASC";
                                    //    dt = dv.ToTable();
                                    //   // dt.DefaultView.Sort = "ActDate ASC";
                                    //}

                                    Pass_CA_Entity.Rec_Type = "I";
                                    Get_Latest_Activity_data("E");
                                    Pass_CA_Entity.Cost = PrimAmt.ToString();
                                    Pass_CA_Entity.Elec_Other = "E";

                                    int New_CAID = 1, New_CA_Seq = 1;
                                    string Operatipn_Mode = "Insert";
                                    if (Pass_CA_Entity.Rec_Type == "I")
                                    {
                                        Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                                        Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString();
                                    }
                                    if (dgvInvoicePosting.Rows.Count > 0)
                                    {
                                        List<DataGridViewRow> Selrow = dgvInvoicePosting.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["gvPrimorSec"].Value.ToString() == "P").ToList();
                                        if(Selrow.Count>0)
                                        {
                                            Pass_CA_Entity.ACT_Date = MainDate.ToShortDateString();
                                            Pass_CA_Entity.ActSeek_Date = MainDate.ToShortDateString();
                                        }
                                        else
                                        {
                                            Pass_CA_Entity.ACT_Date = SPM_Entity.startdate.Trim();
                                            Pass_CA_Entity.ActSeek_Date = SPM_Entity.startdate.Trim();
                                        }
                                        
                                    }
                                    else
                                    {
                                        Pass_CA_Entity.ACT_Date = SPM_Entity.startdate.Trim();
                                        Pass_CA_Entity.ActSeek_Date = SPM_Entity.startdate.Trim();
                                    }


                                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);
                                    if (Emsbdc_List.Count > 0)
                                        Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);

                                    decimal decamount = 0;
                                    bool boolsucess = true;
                                    decimal decbal = 0;
                                    if (Emsbdc_List[0] != null)
                                    {
                                        decbal = Convert.ToDecimal(Emsbdc_List[0].BDC_BALANCE.Trim());
                                        decamount = Convert.ToDecimal(Pass_CA_Entity.Cost.Trim());
                                    }
                                    if (decamount > decbal)
                                    {
                                        CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this Benefit \n Amount may not be more than " + decbal);
                                        boolsucess = false;
                                        btnSave.Enabled = true;
                                    }

                                    if (boolsucess)
                                    {
                                        if (_model.SPAdminData.UpdateCASEACT3(Pass_CA_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                                        {
                                            IsPost = true;

                                            Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                                            Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString();

                                            CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                            Search_CAOBO_Entity.ID = New_CAID.ToString();
                                            Search_CAOBO_Entity.Rec_Type = "S";
                                            Search_CAOBO_Entity.Seq = "1";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                            foreach (CaseSnpEntity Entity in _baseForm.BaseCaseSnpEntity)
                                            {
                                                if (Entity.Status == "A" && Entity.Exclude == "N")
                                                {
                                                    Search_CAOBO_Entity.CLID = Entity.ClientId.ToString();
                                                    Search_CAOBO_Entity.Fam_Seq = Entity.FamilySeq.ToString();
                                                    Search_CAOBO_Entity.Seq = "1";
                                                    Search_CAOBO_Entity.Rec_Type = "I";

                                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                                }
                                            }

                                            //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                                            if (Pass_CA_Entity.Fund1.Trim() != SPM_Entity.SPM_Fund.Trim() || Pass_CA_Entity.BDC_ID != SPM_Entity.SPM_BDC_ID)
                                            {

                                                Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);

                                                if (Emsbdc_List.Count > 0)
                                                    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);
                                                CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                                if (emsbdcentity != null)
                                                {
                                                    CMBDCEntity emsbdcdata = new CMBDCEntity();
                                                    emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                                                    emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                                                    emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                                                    emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                                                    emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                                                    emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                                                    emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                                                    emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                                                    emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                                                    emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                                                    emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                                    emsbdcdata.Mode = "BdcAmount";
                                                    string strstatus = string.Empty;
                                                    if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                                    {
                                                    }
                                                }
                                            }

                                            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);

                                            if (Emsbdc_List.Count > 0)
                                                Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                                            CMBDCEntity bdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                            if (bdcentity != null)
                                            {
                                                CMBDCEntity emsbdcdata = new CMBDCEntity();
                                                emsbdcdata.BDC_AGENCY = bdcentity.BDC_AGENCY;
                                                emsbdcdata.BDC_DEPT = bdcentity.BDC_DEPT;
                                                emsbdcdata.BDC_PROGRAM = bdcentity.BDC_PROGRAM;
                                                emsbdcdata.BDC_YEAR = bdcentity.BDC_YEAR;


                                                emsbdcdata.BDC_DESCRIPTION = bdcentity.BDC_DESCRIPTION;
                                                emsbdcdata.BDC_FUND = bdcentity.BDC_FUND;
                                                emsbdcdata.BDC_ID = bdcentity.BDC_ID;
                                                emsbdcdata.BDC_START = bdcentity.BDC_START;
                                                emsbdcdata.BDC_END = bdcentity.BDC_END;
                                                emsbdcdata.BDC_BUDGET = bdcentity.BDC_BUDGET;
                                                emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                                emsbdcdata.Mode = "BdcAmount";
                                                string strstatus = string.Empty;
                                                if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                                {
                                                }
                                            }

                                            foreach (DataGridViewRow dr in dgvMonthsGrid.Rows)
                                            {
                                                if (dr.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" && dr.Cells["gvCaseact"].Value.ToString() == "N" && dr.Cells["gvprimorSeco"].Value.ToString() == "P")
                                                {
                                                    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                                                    custEntity.ACTAGENCY = Pass_CA_Entity.Agency;
                                                    custEntity.ACTDEPT = Pass_CA_Entity.Dept;
                                                    custEntity.ACTPROGRAM = Pass_CA_Entity.Program;
                                                    if (Pass_CA_Entity.Year.Trim() == string.Empty)
                                                        custEntity.ACTYEAR = "    ";
                                                    else
                                                        custEntity.ACTYEAR = Pass_CA_Entity.Year;
                                                    custEntity.ACTAPPNO = Pass_CA_Entity.App_no;


                                                    custEntity.USAGE_MONTH = dr.Cells["gvMonthsCode"].Value.ToString();
                                                    custEntity.USAGE_LUMP_PRIM = dr.Cells["gvAmtPaid"].Value.ToString();
                                                    custEntity.USAGE_PRIM_ACTID = Pass_CA_Entity.ACT_ID;
                                                    custEntity.addoperator = _baseForm.UserID;
                                                    custEntity.lstcoperator = _baseForm.UserID;
                                                    custEntity.USAGE_PSOURCE = "P";
                                                    custEntity.Mode = "USAGE";

                                                    _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);
                                                }
                                            }
                                        }
                                    }
                                }
                              

                                #endregion

                                #region secondary Vendor Record

                                decimal SecAmt = dgvMonthsGrid.Rows.Cast<DataGridViewRow>()
                                    .Where(row => row.Cells["gvCaseact"].Value.ToString() == "N" && row.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" //&& row.Cells["gvMonthsCode"].Value.ToString() != "CUR"
                                    && row.Cells["gvprimorSeco"].Value.ToString() == "S" && row.Cells["gvAmtPaid"].Value.ToString().Trim() != "").Sum(row =>
                                    {
                                        var cellValue = row.Cells["gvAmtPaid"].Value;
                                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                                        {
                                            return 0m;
                                        }
                                        if (decimal.TryParse(cellValue.ToString(), out decimal amount))
                                        {
                                            return amount;
                                        }
                                        return 0m;
                                    });

                                if (SecAmt > 0)
                                {
                                    IsBilling = true;
                                    //System.Data.DataTable dt = new System.Data.DataTable();
                                    //dt.Columns.Add("ActDate", typeof(string));

                                    List<DataGridViewRow> Selrows = dgvMonthsGrid.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["gvCaseact"].Value.ToString() == "N" && row.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" //&& row.Cells["gvMonthsCode"].Value.ToString()!="CUR"
                                                                     && row.Cells["gvprimorSeco"].Value.ToString() == "S" && row.Cells["gvAmtPaid"].Value.ToString().Trim() != "")
                                                                     .OrderBy(row => (row.Cells["gvtServicedate"].Value)).ToList();

                                    DateTime MainDate = new DateTime();
                                    if (Selrows.Count > 0)
                                    {
                                        DateTime strDate = new DateTime(); //string strActualDate = string.Empty;
                                        bool First = true;

                                        foreach (DataGridViewRow item in Selrows)
                                        {
                                            if (First) { MainDate = Convert.ToDateTime(item["gvtServicedate"].Value.ToString()); First = false; }
                                            strDate = Convert.ToDateTime(item["gvtServicedate"].Value.ToString());

                                            if (MainDate >= strDate)
                                                MainDate = strDate;
                                        }
                                    }


                                    Pass_CA_Entity.Rec_Type = "I";
                                    Get_Latest_Activity_data("O");
                                    Pass_CA_Entity.Cost = SecAmt.ToString();
                                    Pass_CA_Entity.Elec_Other = "O";

                                    int New_CAIDS = 1, New_CA_SeqS = 1;
                                    string Operatipn_ModeS = "Insert";
                                    if (Pass_CA_Entity.Rec_Type == "I")
                                    {
                                        Pass_CA_Entity.ACT_ID = New_CAIDS.ToString();
                                        Pass_CA_Entity.ACT_Seq = New_CA_SeqS.ToString();
                                    }

                                    if(dgvInvoicePosting.Rows.Count>0)
                                    {
                                        List<DataGridViewRow> Selrow = dgvInvoicePosting.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["gvPrimorSec"].Value.ToString() == "S").ToList();
                                        if (Selrow.Count > 0)
                                        {
                                            Pass_CA_Entity.ACT_Date = MainDate.ToShortDateString();
                                            Pass_CA_Entity.ActSeek_Date = MainDate.ToShortDateString();
                                        }
                                        else
                                        {
                                            Pass_CA_Entity.ACT_Date = SPM_Entity.startdate.Trim();
                                            Pass_CA_Entity.ActSeek_Date = SPM_Entity.startdate.Trim();
                                        }
                                    }
                                    else
                                    {
                                        Pass_CA_Entity.ACT_Date = SPM_Entity.startdate.Trim();
                                        Pass_CA_Entity.ActSeek_Date = SPM_Entity.startdate.Trim();
                                    }



                                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);
                                    if (Emsbdc_List.Count > 0)
                                        Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);

                                    decimal decamount = 0;
                                    bool boolsucess = true;
                                    decimal decbal = 0;
                                    if (Emsbdc_List[0] != null)
                                    {
                                        decbal = Convert.ToDecimal(Emsbdc_List[0].BDC_BALANCE.Trim());
                                        decamount = Convert.ToDecimal(Pass_CA_Entity.Cost.Trim());
                                    }
                                    if (decamount > decbal)
                                    {
                                        CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this Benefit \n Amount may not be more than " + decbal);
                                        boolsucess = false;
                                        btnSave.Enabled = true;
                                    }

                                    if (boolsucess)
                                    {
                                        if (_model.SPAdminData.UpdateCASEACT3(Pass_CA_Entity, Operatipn_ModeS, out New_CAIDS, out New_CA_SeqS, out Sql_SP_Result_Message))
                                        {
                                            IsPost = true;

                                            Pass_CA_Entity.ACT_ID = New_CAIDS.ToString();
                                            Pass_CA_Entity.ACT_Seq = New_CA_SeqS.ToString();

                                            CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                            Search_CAOBO_Entity.ID = New_CAIDS.ToString();
                                            Search_CAOBO_Entity.Rec_Type = "S";
                                            Search_CAOBO_Entity.Seq = "1";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                            foreach (CaseSnpEntity Entity in _baseForm.BaseCaseSnpEntity)
                                            {
                                                if (Entity.Status == "A" && Entity.Exclude == "N")
                                                {
                                                    Search_CAOBO_Entity.CLID = Entity.ClientId.ToString();
                                                    Search_CAOBO_Entity.Fam_Seq = Entity.FamilySeq.ToString();
                                                    Search_CAOBO_Entity.Seq = "1";
                                                    Search_CAOBO_Entity.Rec_Type = "I";

                                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                                }
                                            }

                                            //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                                            if (Pass_CA_Entity.Fund1.Trim() != SPM_Entity.SPM_Fund.Trim() || Pass_CA_Entity.BDC_ID != SPM_Entity.SPM_BDC_ID)
                                            {

                                                Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);

                                                if (Emsbdc_List.Count > 0)
                                                    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);
                                                CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                                if (emsbdcentity != null)
                                                {
                                                    CMBDCEntity emsbdcdata = new CMBDCEntity();
                                                    emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                                                    emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                                                    emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                                                    emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                                                    emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                                                    emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                                                    emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                                                    emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                                                    emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                                                    emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                                                    emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                                    emsbdcdata.Mode = "BdcAmount";
                                                    string strstatus = string.Empty;
                                                    if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                                    {
                                                    }
                                                }
                                            }

                                            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);

                                            if (Emsbdc_List.Count > 0)
                                                Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                                            CMBDCEntity bdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                            if (bdcentity != null)
                                            {
                                                CMBDCEntity emsbdcdata = new CMBDCEntity();
                                                emsbdcdata.BDC_AGENCY = bdcentity.BDC_AGENCY;
                                                emsbdcdata.BDC_DEPT = bdcentity.BDC_DEPT;
                                                emsbdcdata.BDC_PROGRAM = bdcentity.BDC_PROGRAM;
                                                emsbdcdata.BDC_YEAR = bdcentity.BDC_YEAR;


                                                emsbdcdata.BDC_DESCRIPTION = bdcentity.BDC_DESCRIPTION;
                                                emsbdcdata.BDC_FUND = bdcentity.BDC_FUND;
                                                emsbdcdata.BDC_ID = bdcentity.BDC_ID;
                                                emsbdcdata.BDC_START = bdcentity.BDC_START;
                                                emsbdcdata.BDC_END = bdcentity.BDC_END;
                                                emsbdcdata.BDC_BUDGET = bdcentity.BDC_BUDGET;
                                                emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                                emsbdcdata.Mode = "BdcAmount";
                                                string strstatus = string.Empty;
                                                if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                                {
                                                }
                                            }

                                            foreach (DataGridViewRow dr in dgvMonthsGrid.Rows)
                                            {
                                                if (dr.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" && dr.Cells["gvCaseact"].Value.ToString() == "N" && dr.Cells["gvprimorSeco"].Value.ToString() == "S")
                                                {
                                                    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                                                    custEntity.ACTAGENCY = _baseForm.BaseAgency;
                                                    custEntity.ACTDEPT = _baseForm.BaseDept;
                                                    custEntity.ACTPROGRAM = _baseForm.BaseProg;
                                                    if (_baseForm.BaseYear.Trim() == string.Empty)
                                                        custEntity.ACTYEAR = "    ";
                                                    else
                                                        custEntity.ACTYEAR = _baseForm.BaseYear;
                                                    custEntity.ACTAPPNO = _baseForm.BaseApplicationNo;


                                                    custEntity.USAGE_MONTH = dr.Cells["gvMonthsCode"].Value.ToString();
                                                    custEntity.USAGE_LUMP_SEC = dr.Cells["gvAmtPaid"].Value.ToString();
                                                    custEntity.USAGE_SEC_ACTID = Pass_CA_Entity.ACT_ID;
                                                    //custEntity.USAGE_SEC_VEND=Pass_CA_Entity.se
                                                    custEntity.addoperator = _baseForm.UserID;
                                                    custEntity.lstcoperator = _baseForm.UserID;
                                                    custEntity.USAGE_PSOURCE = "S";
                                                    custEntity.Mode = "USAGE";

                                                    _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);
                                                }
                                            }

                                        }
                                    }
                                }
                  


                                #endregion
                            }

                            if(dgvInvoicePosting.Rows.Count==0)
                            {
                                if(SPM_Entity!=null && IsBilling)
                                {
                                    LINterval= ((ListItem)cmbLumpsum.SelectedItem).Value.ToString();
                                    string Tmp_SPM_Sequence = "1";
                                    SPM_Entity.Rec_Type = "S";
                                    SPM_Entity.SPM_LUMP_INTRVAL = ((ListItem)cmbLumpsum.SelectedItem).Value.ToString();
                                    if (_model.SPAdminData.UpdateCASESPM(SPM_Entity, "Insert", out Sql_SP_Result_Message, out Tmp_SPM_Sequence))
                                    {

                                    }
                                }
                            }

                            #region Arrears Service 

                            string Operatipn_Mode3 = "Insert";
                            
                            #region Primary Arrears

                            if (IsPAdd || IsPEdit)
                            {
                                CASEACTEntity ActEntity = SP_Activity_Details.Find(u => u.Vendor_No == SPM_Entity.SPM_Vendor && u.ACT_Code.Trim() == txtArrearCode.Text.Trim() && u.Bulk == "A");
                                if (ActEntity != null)
                                {
                                    Pass_CA_Entity = ActEntity;
                                    Pass_CA_Entity.Rec_Type = "U";
                                }
                                else
                                    Pass_CA_Entity.Rec_Type = "I";

                                Get_Arrears_Activity_data("E");
                                Pass_CA_Entity.Cost = txtPArrInv.Text;
                                //Pass_CA_Entity.Elec_Other = "E";

                                //Pass_CA_Entity.ACT_Date = SPM_Entity.startdate.Trim();

                                int New_CAID3 = 1, New_CA_Seq3 = 1;
                                
                                if (Pass_CA_Entity.Rec_Type == "I")
                                {
                                    Pass_CA_Entity.ACT_ID = New_CAID3.ToString();
                                    Pass_CA_Entity.ACT_Seq = New_CA_Seq3.ToString();
                                }
                                else
                                {
                                    Pass_CA_Entity.ACT_ID = ActEntity.ACT_ID;
                                    Pass_CA_Entity.ACT_Seq = ActEntity.ACT_Seq;
                                }
                                Pass_CA_Entity.Bulk = "A";
                                if (_model.SPAdminData.UpdateCASEACT3(Pass_CA_Entity, Operatipn_Mode3, out New_CAID3, out New_CA_Seq3, out Sql_SP_Result_Message))
                                {
                                    IsPost = true;

                                    Pass_CA_Entity.ACT_ID = New_CAID3.ToString();
                                    Pass_CA_Entity.ACT_Seq = New_CA_Seq3.ToString();

                                    CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                    Search_CAOBO_Entity.ID = New_CAID3.ToString();
                                    Search_CAOBO_Entity.Rec_Type = "S";
                                    Search_CAOBO_Entity.Seq = "1";

                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                    foreach (CaseSnpEntity Entity in _baseForm.BaseCaseSnpEntity)
                                    {
                                        if (Entity.Status == "A" && Entity.Exclude == "N")
                                        {
                                            Search_CAOBO_Entity.CLID = Entity.ClientId.ToString();
                                            Search_CAOBO_Entity.Fam_Seq = Entity.FamilySeq.ToString();
                                            Search_CAOBO_Entity.Seq = "1";
                                            Search_CAOBO_Entity.Rec_Type = "I";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Secondary Arrears
                            
                            if (IsSAdd || IsSEdit)
                            {
                                CASEACTEntity ActEntity1 = SP_Activity_Details.Find(u => u.Vendor_No == SPM_Entity.SPM_Gas_Vendor && u.ACT_Code.Trim() == txtArrearCode.Text.Trim() && u.Bulk == "A");
                                if (ActEntity1 != null)
                                {
                                    Pass_CA_Entity = ActEntity1;
                                    Pass_CA_Entity.Rec_Type = "U";
                                }
                                else
                                    Pass_CA_Entity.Rec_Type = "I";

                                Get_Arrears_Activity_data("O");
                                Pass_CA_Entity.Cost = txtSArrInv.Text;
                                //Pass_CA_Entity.Elec_Other = "O";

                                //Pass_CA_Entity.ACT_Date = SPM_Entity.startdate.Trim();

                                int New_CAID1 = 1, New_CA_Seq1 = 1;
                                Operatipn_Mode3 = "Insert";
                                if (Pass_CA_Entity.Rec_Type == "I")
                                {
                                    Pass_CA_Entity.ACT_ID = New_CAID1.ToString();
                                    Pass_CA_Entity.ACT_Seq = New_CA_Seq1.ToString();
                                }
                                else
                                {
                                    Pass_CA_Entity.ACT_ID = ActEntity1.ACT_ID;
                                    Pass_CA_Entity.ACT_Seq = ActEntity1.ACT_Seq;
                                }
                                Pass_CA_Entity.Bulk = "A";
                                if (_model.SPAdminData.UpdateCASEACT3(Pass_CA_Entity, Operatipn_Mode3, out New_CAID1, out New_CA_Seq1, out Sql_SP_Result_Message))
                                {
                                    IsPost = true;

                                    Pass_CA_Entity.ACT_ID = New_CAID1.ToString();
                                    Pass_CA_Entity.ACT_Seq = New_CA_Seq1.ToString();

                                    CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                    Search_CAOBO_Entity.ID = New_CAID1.ToString();
                                    Search_CAOBO_Entity.Rec_Type = "S";
                                    Search_CAOBO_Entity.Seq = "1";

                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                    foreach (CaseSnpEntity Entity in _baseForm.BaseCaseSnpEntity)
                                    {
                                        if (Entity.Status == "A" && Entity.Exclude == "N")
                                        {
                                            Search_CAOBO_Entity.CLID = Entity.ClientId.ToString();
                                            Search_CAOBO_Entity.Fam_Seq = Entity.FamilySeq.ToString();
                                            Search_CAOBO_Entity.Seq = "1";
                                            Search_CAOBO_Entity.Rec_Type = "I";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                        }
                                    }
                                }
                            }
                                
                            #endregion

                            
                            #endregion


                        }
                        else
                        {
                            foreach (DataGridViewRow dr in dgvMonthsGrid.Rows)
                            {
                                if (dr.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" && dr.Cells["gvCaseact"].Value.ToString() == "N")
                                {
                                    Pass_CA_Entity.Rec_Type = "I";
                                    if (dr.Cells["gvCaseact"].Value.ToString() == "Y")
                                        Pass_CA_Entity.Rec_Type = "U";

                                    Get_Latest_Activity_data(dr);

                                    if (dr.Tag != null)
                                    {
                                        Pass_CA_Entity.BDC_ID = dr.Tag.ToString();
                                        CMBDCEntity BDCEntity = ALLEmsbdc_List.Find(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                                        if (BDCEntity != null)
                                            Pass_CA_Entity.Fund1 = BDCEntity.BDC_FUND;
                                    }

                                    Pass_CA_Entity.Elec_Other = dr.Cells["gvElecorOther"].Value.ToString();

                                    int New_CAID = 1, New_CA_Seq = 1;
                                    string Operatipn_Mode = "Insert";
                                    if (Pass_CA_Entity.Rec_Type == "U")
                                        Operatipn_Mode = "Update";
                                    //if(!string.IsNullOrEmpty(dr.Cells[""].Value.ToString()))
                                    if (Pass_CA_Entity.Rec_Type == "I")
                                    {
                                        Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                                        Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString();
                                    }

                                    //Addded by Sudheer on 05/25/23
                                    //Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);
                                    //if (Emsbdc_List.Count > 0)
                                    //    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);

                                    //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);
                                    if (Emsbdc_List.Count > 0)
                                        Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);


                                    decimal decamount = 0;
                                    bool boolsucess = true;
                                    decimal decbal = 0;
                                    if (Emsbdc_List[0] != null)
                                    {
                                        decbal = Convert.ToDecimal(Emsbdc_List[0].BDC_BALANCE.Trim());
                                        decamount = Convert.ToDecimal(Pass_CA_Entity.Cost.Trim());
                                    }

                                    if (decamount > decbal)
                                    {
                                        CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this Benefit \n Amount may not be more than " + decbal);
                                        boolsucess = false;
                                        btnSave.Enabled = true;
                                        break;
                                    }

                                    if (boolsucess)
                                    {
                                        if (_model.SPAdminData.UpdateCASEACT3(Pass_CA_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                                        {
                                            IsPost = true;

                                            Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                                            Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString();

                                            CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                            Search_CAOBO_Entity.ID = New_CAID.ToString();
                                            Search_CAOBO_Entity.Rec_Type = "S";
                                            Search_CAOBO_Entity.Seq = "1";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                            foreach (CaseSnpEntity Entity in _baseForm.BaseCaseSnpEntity)
                                            {
                                                if (Entity.Status == "A" && Entity.Exclude == "N")
                                                {
                                                    Search_CAOBO_Entity.CLID = Entity.ClientId.ToString();
                                                    Search_CAOBO_Entity.Fam_Seq = Entity.FamilySeq.ToString();
                                                    Search_CAOBO_Entity.Seq = "1";
                                                    Search_CAOBO_Entity.Rec_Type = "I";

                                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                                }
                                            }

                                            //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                                            if (Pass_CA_Entity.Fund1.Trim() != SPM_Entity.SPM_Fund.Trim() || Pass_CA_Entity.BDC_ID != SPM_Entity.SPM_BDC_ID)
                                            {

                                                Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);

                                                if (Emsbdc_List.Count > 0)
                                                    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);
                                                CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                                if (emsbdcentity != null)
                                                {
                                                    CMBDCEntity emsbdcdata = new CMBDCEntity();
                                                    emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                                                    emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                                                    emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                                                    emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                                                    emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                                                    emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                                                    emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                                                    emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                                                    emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                                                    emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                                                    emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                                    emsbdcdata.Mode = "BdcAmount";
                                                    string strstatus = string.Empty;
                                                    if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                                    {
                                                    }
                                                }
                                            }

                                            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);

                                            if (Emsbdc_List.Count > 0)
                                                Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                                            CMBDCEntity bdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                            if (bdcentity != null)
                                            {
                                                CMBDCEntity emsbdcdata = new CMBDCEntity();
                                                emsbdcdata.BDC_AGENCY = bdcentity.BDC_AGENCY;
                                                emsbdcdata.BDC_DEPT = bdcentity.BDC_DEPT;
                                                emsbdcdata.BDC_PROGRAM = bdcentity.BDC_PROGRAM;
                                                emsbdcdata.BDC_YEAR = bdcentity.BDC_YEAR;


                                                emsbdcdata.BDC_DESCRIPTION = bdcentity.BDC_DESCRIPTION;
                                                emsbdcdata.BDC_FUND = bdcentity.BDC_FUND;
                                                emsbdcdata.BDC_ID = bdcentity.BDC_ID;
                                                emsbdcdata.BDC_START = bdcentity.BDC_START;
                                                emsbdcdata.BDC_END = bdcentity.BDC_END;
                                                emsbdcdata.BDC_BUDGET = bdcentity.BDC_BUDGET;
                                                emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                                emsbdcdata.Mode = "BdcAmount";
                                                string strstatus = string.Empty;
                                                if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (IsPost)
                            AlertBox.Show("Invoices are posted Successfully");

                       // btnSave.Enabled = false;
                        Fill_Applicant_SPs();
                        Get_App_CASEACT_List(SPM_Entity);
                        if (LINterval == "M") CommonFunctions.SetComboBoxValue(cmbLumpsum, "M");
                        FillMonthsGrid();
                        FillInvPostingGrid();
                        FillServiceTextbox();

                        dtpPArrDte.Enabled = dtpSArrDte.Enabled = txtPArrInv.Enabled = txtSArrInv.Enabled = false;
                    }
                }
            }
            else
            {
                AlertBox.Show("There is no services to post", MessageBoxIcon.Warning);
            }
        }

        string LINterval = string.Empty;
        private bool SaveBillingInvoices(bool IsPost)
        {

            foreach (DataGridViewRow dr in dgvMonthsGrid.Rows)
            {
                if (dr.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" && dr.Cells["gvCaseact"].Value.ToString() == "N")
                {
                    if (dr.Cells["gvAmtPaid"].Value.ToString()!="")
                    {
                        Pass_CA_Entity.Rec_Type = "I";
                        if (dr.Cells["gvCaseact"].Value.ToString() == "Y")
                            Pass_CA_Entity.Rec_Type = "U";

                        Get_Latest_Activity_data(dr);

                        if (dr.Tag != null)
                        {
                            Pass_CA_Entity.BDC_ID = dr.Tag.ToString();
                            CMBDCEntity BDCEntity = ALLEmsbdc_List.Find(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                            if (BDCEntity != null)
                                Pass_CA_Entity.Fund1 = BDCEntity.BDC_FUND;
                        }

                        Pass_CA_Entity.Elec_Other = dr.Cells["gvElecorOther"].Value.ToString();

                        if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y" && CEAPCNTL_List[0].CPCT_USER_CNTL == "Y" && ((ListItem)cmbLumpsum.SelectedItem).Value.ToString() == "M" && dr.Cells["gvMonthsCode"].Value.ToString().ToUpper() == "CUR")
                        {
                            Pass_CA_Entity.ActSeek_Date = DateTime.Now.ToShortDateString();
                            Pass_CA_Entity.ACT_Date = DateTime.Now.ToShortDateString();
                        }


                        int New_CAID = 1, New_CA_Seq = 1;
                        string Operatipn_Mode = "Insert";
                        if (Pass_CA_Entity.Rec_Type == "U")
                            Operatipn_Mode = "Update";
                        //if(!string.IsNullOrEmpty(dr.Cells[""].Value.ToString()))
                        if (Pass_CA_Entity.Rec_Type == "I")
                        {
                            Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                            Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString();
                        }

                        //Addded by Sudheer on 05/25/23
                        //Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);
                        //if (Emsbdc_List.Count > 0)
                        //    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);

                        //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                        Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);
                        if (Emsbdc_List.Count > 0)
                            Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);


                        decimal decamount = 0;
                        bool boolsucess = true;
                        decimal decbal = 0;
                        if (Emsbdc_List[0] != null)
                        {
                            decbal = Convert.ToDecimal(Emsbdc_List[0].BDC_BALANCE.Trim());
                            decamount = Convert.ToDecimal(Pass_CA_Entity.Cost.Trim());
                        }

                        if (decamount > decbal)
                        {
                            CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this Benefit \n Amount may not be more than " + decbal);
                            boolsucess = false;
                            btnSave.Enabled = true;
                            break;
                        }

                        if (boolsucess)
                        {
                            if (_model.SPAdminData.UpdateCASEACT3(Pass_CA_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                            {
                                IsPost = true;

                                Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                                Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString();

                                CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                Search_CAOBO_Entity.ID = New_CAID.ToString();
                                Search_CAOBO_Entity.Rec_Type = "S";
                                Search_CAOBO_Entity.Seq = "1";

                                _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                foreach (CaseSnpEntity Entity in _baseForm.BaseCaseSnpEntity)
                                {
                                    if (Entity.Status == "A" && Entity.Exclude == "N")
                                    {
                                        Search_CAOBO_Entity.CLID = Entity.ClientId.ToString();
                                        Search_CAOBO_Entity.Fam_Seq = Entity.FamilySeq.ToString();
                                        Search_CAOBO_Entity.Seq = "1";
                                        Search_CAOBO_Entity.Rec_Type = "I";

                                        _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                    }
                                }

                                //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                                if (Pass_CA_Entity.Fund1.Trim() != SPM_Entity.SPM_Fund.Trim() || Pass_CA_Entity.BDC_ID != SPM_Entity.SPM_BDC_ID)
                                {

                                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);

                                    if (Emsbdc_List.Count > 0)
                                        Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);
                                    CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                    if (emsbdcentity != null)
                                    {
                                        CMBDCEntity emsbdcdata = new CMBDCEntity();
                                        emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                                        emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                                        emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                                        emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                                        emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                                        emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                                        emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                                        emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                                        emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                                        emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                                        emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                        emsbdcdata.Mode = "BdcAmount";
                                        string strstatus = string.Empty;
                                        if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                        {
                                        }
                                    }
                                }

                                Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);

                                if (Emsbdc_List.Count > 0)
                                    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                                CMBDCEntity bdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                if (bdcentity != null)
                                {
                                    CMBDCEntity emsbdcdata = new CMBDCEntity();
                                    emsbdcdata.BDC_AGENCY = bdcentity.BDC_AGENCY;
                                    emsbdcdata.BDC_DEPT = bdcentity.BDC_DEPT;
                                    emsbdcdata.BDC_PROGRAM = bdcentity.BDC_PROGRAM;
                                    emsbdcdata.BDC_YEAR = bdcentity.BDC_YEAR;


                                    emsbdcdata.BDC_DESCRIPTION = bdcentity.BDC_DESCRIPTION;
                                    emsbdcdata.BDC_FUND = bdcentity.BDC_FUND;
                                    emsbdcdata.BDC_ID = bdcentity.BDC_ID;
                                    emsbdcdata.BDC_START = bdcentity.BDC_START;
                                    emsbdcdata.BDC_END = bdcentity.BDC_END;
                                    emsbdcdata.BDC_BUDGET = bdcentity.BDC_BUDGET;
                                    emsbdcdata.BDC_LSTC_OPERATOR = _baseForm.UserID;
                                    emsbdcdata.Mode = "BdcAmount";
                                    string strstatus = string.Empty;
                                    if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                    {
                                    }
                                }

                                //foreach (DataGridViewRow dr in dgvMonthsGrid.Rows)
                                //{
                                    if (dr.Cells["gvSelect"].Value.ToString().ToUpper() == "TRUE" && dr.Cells["gvCaseact"].Value.ToString() == "N")
                                    {
                                        CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                                        custEntity.ACTAGENCY = Pass_CA_Entity.Agency;
                                        custEntity.ACTDEPT = Pass_CA_Entity.Dept;
                                        custEntity.ACTPROGRAM = Pass_CA_Entity.Program;
                                        if (Pass_CA_Entity.Year.Trim() == string.Empty)
                                            custEntity.ACTYEAR = "    ";
                                        else
                                            custEntity.ACTYEAR = Pass_CA_Entity.Year;
                                        custEntity.ACTAPPNO = Pass_CA_Entity.App_no;
                                    custEntity.USAGE_MONTH = dr.Cells["gvMonthsCode"].Value.ToString();
                                    if (dr.Cells["gvprimorSeco"].Value.ToString() == "P")
                                    {
                                        
                                        custEntity.USAGE_LUMP_PRIM = dr.Cells["gvAmtPaid"].Value.ToString();
                                        custEntity.USAGE_PRIM_ACTID = Pass_CA_Entity.ACT_ID;
                                        
                                    }
                                    else
                                    {
                                        
                                        custEntity.USAGE_LUMP_SEC = dr.Cells["gvAmtPaid"].Value.ToString();
                                        custEntity.USAGE_SEC_ACTID = Pass_CA_Entity.ACT_ID;
                                    }
                                    custEntity.addoperator = _baseForm.UserID;
                                    custEntity.lstcoperator = _baseForm.UserID;
                                    custEntity.USAGE_PSOURCE = dr.Cells["gvprimorSeco"].Value.ToString();
                                        custEntity.Mode = "USAGE";

                                        _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);
                                    }
                                //}




                            }
                        }
                    }

                    
                }
            }
            LINterval = "M";
            return IsPost;
        }

        private void dgvInvoicePosting_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInvoicePosting.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 6)
                {
                    //if (dgvMonthsGrid.Rows[e.RowIndex].Cells["gvCaseact"].Value.ToString() == "Y")
                    //{
                        CASEACTEntity Entity = dgvInvoicePosting.Rows[e.RowIndex].Tag as CASEACTEntity;

                        List<CASEACTEntity> SelListEntity = SP_Activity_Details.FindAll(u => u.ACT_Code.Trim() == txtServCode.Text.Trim());

                        if (PostUsagePriv != null)
                        {
                            if (PostUsagePriv.ChangePriv.ToUpper() == "TRUE")
                            {
                                if (string.IsNullOrEmpty(Entity.BundleNo.Trim()))
                                {
                                    CASEACTEntity ActEntity = dgvInvoicePosting.Rows[e.RowIndex].Tag as CASEACTEntity;

                                    CASE0016_UsageEditForm EditForm = new CASE0016_UsageEditForm(_baseForm, "Edit", SPM_Entity, ActEntity, CEAPCNTL_List[0], txtBalance.Text.Trim(), _privilegeEntity);
                                    EditForm.FormClosed += new FormClosedEventHandler(On_Edit_Form_Closed);
                                    EditForm.StartPosition = FormStartPosition.CenterScreen;
                                    EditForm.ShowDialog();
                                }
                            }
                        }
                    //}
                    //else
                    //{
                    //    CASEACTEntity Entity = new CASEACTEntity();
                    //    Entity.Service_plan = _spCode;
                    //    Entity.ACT_Code = txtServCode.Text;
                    //    Entity.ACT_Date = dgvMonthsGrid.CurrentRow.Cells["gvtServicedate"].Value.ToString();
                    //    Entity.Cost = dgvMonthsGrid.CurrentRow.Cells["gvAmtPaid"].Value.ToString();
                    //    Entity.PDOUT = dgvMonthsGrid.CurrentRow.Cells["gvPDOut"].Value.ToString();
                    //    if (PostUsagePriv != null)
                    //    {
                    //        if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //        {
                    //            CASE0016_UsageEditForm EditForm = new CASE0016_UsageEditForm(_baseForm, "Add", SPM_Entity, Entity, CEAPCNTL_List[0], txtBalance.Text.Trim(), _privilegeEntity);
                    //            EditForm.FormClosed += new FormClosedEventHandler(On_Edit_Form_Closed);
                    //            EditForm.StartPosition = FormStartPosition.CenterScreen;
                    //            EditForm.ShowDialog();
                    //        }
                    //    }
                    //}
                }
                if (e.RowIndex != -1 && e.ColumnIndex == 7)
                {
                    CASEACTEntity Entity = dgvInvoicePosting.Rows[e.RowIndex].Tag as CASEACTEntity;

                    if (string.IsNullOrEmpty(Entity.BundleNo.Trim()))
                    {
                        if (PostUsagePriv != null)
                        {
                            if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                            {
                                MessageBox.Show("Are you sure you want to delete selected Invoice?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Selected_CAMS);
                            }
                        }
                    }
                }
            }
        }
        private void On_Edit_Form_Closed(object sender, FormClosedEventArgs e)
        {
            CASE0016_UsageEditForm form = sender as CASE0016_UsageEditForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Selected_Details = new string[4];
                Selected_Details = form.Get_Selected_Row();

                if (Selected_Details[0].ToString() == "Add")
                {
                    dgvMonthsGrid.CellValueChanged -= new DataGridViewCellEventHandler(dgvMonthsGrid_CellValueChanged);
                    dgvMonthsGrid.CurrentRow.Cells["gvtServicedate"].Value = Selected_Details[1].ToString();
                    dgvMonthsGrid.CurrentRow.Cells["gvAmtPaid"].Value = Selected_Details[2].ToString();
                    dgvMonthsGrid.CurrentRow.Cells["gvPDOut"].Value = Selected_Details[3].ToString();
                    dgvMonthsGrid.CurrentRow.Tag = Selected_Details[4].ToString();

                    int introwindex = dgvMonthsGrid.CurrentCell.RowIndex;

                    if (txtServCode.Text == ServiceCode)
                        CalculateAmounts(introwindex);

                    dgvMonthsGrid.SelectedRows[0].DefaultCellStyle.BackColor = Color.White;

                    dgvMonthsGrid.CellValueChanged += new DataGridViewCellEventHandler(dgvMonthsGrid_CellValueChanged);
                }
                else
                {
                    Fill_Applicant_SPs();
                    Get_App_CASEACT_List(SPM_Entity);
                    FillMonthsGrid();
                    FillInvPostingGrid();
                }
            }
        }

        private void pbPArrEdit_Click(object sender, EventArgs e)
        {
            pbPArrEdit.Visible = false;
            pbPArrDel.Visible = false;
            pbPArrCncl.Visible = true;

            dtpPArrDte.Enabled = true;
            txtPArrInv.Enabled = true;

            dtpSArrDte.Enabled = false;
            txtSArrInv.Enabled = false;

            IsPEdit = true;

            dtpPArrDte.Focus();
        }

        private void pbSArrEdit_Click(object sender, EventArgs e)
        {
            pbSArrEdit.Visible = false;
            pbSArrDel.Visible = false;
            pbSArrCncl.Visible = true;

            dtpSArrDte.Enabled = true;
            txtSArrInv.Enabled = true;

            dtpPArrDte.Enabled = false;
            txtPArrInv.Enabled = false;

            IsSEdit = true;

            dtpSArrDte.Focus();
        }

        string ArrVendor = string.Empty;
        private void pbPArrDel_Click(object sender, EventArgs e)
        {
            if(SP_Activity_Details.Count>0)
            {
                CASEACTEntity ActEntity = SP_Activity_Details.Find(u => u.Vendor_No == SPM_Entity.SPM_Vendor && u.ACT_Code.Trim() == txtArrearCode.Text.Trim() && u.Bulk == "A");
                if(ActEntity!=null)
                {
                    if (string.IsNullOrEmpty(ActEntity.BundleNo.Trim()))
                    {
                        if (PostUsagePriv != null)
                        {
                            if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                            {
                                ArrVendor = ActEntity.Vendor_No;
                                MessageBox.Show("Are you sure you want to delete Primary Arrears Invoice?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Arrears_CAMS);
                            }
                        }
                    }
                }
            }
        }

        private void Delete_Arrears_CAMS(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                bool Delete_Status = true;
                int New_MS_ID = 0, Tmp_CA_Seq = 0;
                bool VouchMSG = false;
                int CountCA = 0, CountMS = 0;

                CASEACTEntity ActEntity = SP_Activity_Details.Find(u => u.Vendor_No == ArrVendor && u.ACT_Code.Trim() == txtArrearCode.Text.Trim() && u.Bulk == "A");
                if(ActEntity!=null)
                {
                    ActEntity.Rec_Type = "D";
                    if (!_model.SPAdminData.UpdateCASEACT2(ActEntity, "Delete", out New_MS_ID, out Tmp_CA_Seq, out Sql_SP_Result_Message))
                        Delete_Status = false;

                    ArrVendor = string.Empty;
                    AlertBox.Show("Arrears Invoice Deleted Successfully");
                    Fill_Applicant_SPs();
                    Get_App_CASEACT_List(SPM_Entity);
                    FillMonthsGrid();
                    FillInvPostingGrid();
                    FillServiceTextbox();
                }
            }
        }


        private void pbSArrDel_Click(object sender, EventArgs e)
        {
            if (SP_Activity_Details.Count > 0)
            {
                CASEACTEntity ActEntity = SP_Activity_Details.Find(u => u.Vendor_No == SPM_Entity.SPM_Gas_Vendor && u.ACT_Code.Trim() == txtArrearCode.Text.Trim() && u.Bulk == "A");
                if (ActEntity != null)
                {
                    if (string.IsNullOrEmpty(ActEntity.BundleNo.Trim()))
                    {
                        if (PostUsagePriv != null)
                        {
                            if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                            {
                                ArrVendor = ActEntity.Vendor_No;
                                MessageBox.Show("Are you sure you want to delete Secondary Arrears Invoice?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Arrears_CAMS);
                            }
                        }
                    }
                }
            }
        }

        bool IsPAdd = false; bool IsSAdd=false;bool IsPEdit = false; bool IsSEdit = false;
        private void pbPArrAdd_Click(object sender, EventArgs e)
        {
            pbPArrAdd.Visible = false;
            pbPArrCncl.Visible = true;

            dtpPArrDte.Enabled = true;
            txtPArrInv.Enabled = true;

            dtpSArrDte.Enabled = false;
            txtSArrInv.Enabled = false;

            IsPAdd = true;

            dtpPArrDte.Focus();
        }

        private void pbSArrAdd_Click(object sender, EventArgs e)
        {
            pbSArrAdd.Visible = false;
            pbSArrCncl.Visible = true;

            dtpSArrDte.Enabled = true;
            txtSArrInv.Enabled = true;

            dtpPArrDte.Enabled = false;
            txtPArrInv.Enabled = false;

            IsSAdd = true;

            dtpSArrDte.Focus();
        }

        private void pbPArrCncl_Click(object sender, EventArgs e)
        {
            arrearEntity = SP_Activity_Details.FindAll(x => x.ACT_Code.ToString() == txtArrearCode.Text && x.Vendor_No == SPM_Entity.SPM_Vendor && x.Bulk == "A");

            if (arrearEntity.Count > 0)
            {
                dtpPArrDte.Text = arrearEntity[0].ACT_Date.ToString() == "" ? DateTime.Now.ToString("MM/dd/yyyy") : Convert.ToDateTime(arrearEntity[0].ACT_Date.ToString()).ToString("MM/dd/yyyy");

                txtPArrInv.Text = arrearEntity[0].Cost.ToString();
                txtPArrBundle.Text = arrearEntity[0].BundleNo.ToString();
                txtPArrCheck.Text = arrearEntity[0].Check_No.ToString();
                txtPArrChkDt.Text = arrearEntity[0].Check_Date.ToString();

                pbPArrCncl.Visible = false;

                pbPArrDel.Visible = true;
                pbPArrEdit.Visible = true;

                pbPArrAdd.Visible = false;

                dtpPArrDte.Enabled = false;
                txtPArrInv.Enabled = false;
            }
            else
            {
                pbPArrCncl.Visible = false;

                pbPArrDel.Visible = false;
                pbPArrEdit.Visible = false;

                pbPArrAdd.Visible = true;

                txtPArrInv.Text = string.Empty;
                dtpPArrDte.Text = SPM_Entity.startdate == "" ? DateTime.Now.ToString("MM/dd/yyyy") : Convert.ToDateTime(SPM_Entity.startdate.ToString()).ToString("MM/dd/yyyy");//DateTime.Now.ToShortDateString();

                dtpPArrDte.Enabled = false;
                txtPArrInv.Enabled = false;

                //txtPArrInv.Enabled = true; dtpPArrDte.Enabled = true;
            }
        }

        private void pbSArrCncl_Click(object sender, EventArgs e)
        {
            arrearEntity = SP_Activity_Details.FindAll(x => x.ACT_Code.ToString() == txtArrearCode.Text && x.Vendor_No == SPM_Entity.SPM_Gas_Vendor && x.Bulk == "A");

            if (arrearEntity.Count > 0)
            {
                dtpSArrDte.Text = arrearEntity[0].ACT_Date.ToString() == "" ? DateTime.Now.ToString("MM/dd/yyyy") : Convert.ToDateTime(arrearEntity[0].ACT_Date.ToString()).ToString("MM/dd/yyyy");

                txtSArrInv.Text = arrearEntity[0].Cost.ToString();
                txtSArrBundle.Text = arrearEntity[0].BundleNo.ToString();
                txtSArrChkNo.Text = arrearEntity[0].Check_No.ToString();
                txtSArrChkDt.Text = LookupDataAccess.Getdate(arrearEntity[0].Check_Date.ToString().Trim());

                pbSArrCncl.Visible = false;

                pbSArrEdit.Visible = true;
                pbSArrDel.Visible = true;

                pbSArrAdd.Visible = false;

                dtpSArrDte.Enabled = false;
                txtSArrInv.Enabled = false;
            }
            else
            {
                pbSArrCncl.Visible = false;

                pbSArrEdit.Visible = false;
                pbSArrDel.Visible = false;

                pbSArrAdd.Visible = true;

                txtSArrInv.Text = string.Empty;
                dtpSArrDte.Text = SPM_Entity.startdate == "" ? DateTime.Now.ToString("MM/dd/yyyy") : Convert.ToDateTime(SPM_Entity.startdate.ToString()).ToString("MM/dd/yyyy");//DateTime.Now.ToShortDateString();

                dtpSArrDte.Enabled = false;
                txtSArrInv.Enabled = false;
                //txtSArrInv.Enabled = true; dtpSArrDte.Enabled = true;
            }
        }

        private void CalculateAmounts(int rowcalindex)
        {
            if (dgvMonthsGrid.Rows.Count > 0)
            {
                decimal ResAmount = 0;
                decimal Total = 0;
                decimal Bal = 0;
                if (!string.IsNullOrEmpty(txtAward.Text.Trim()))
                    Total = Convert.ToDecimal(txtAward.Text.Trim());
                if (PropCaseactList.Count > 0)
                {
                    List<CASEACTEntity> caseactList = PropCaseactList.FindAll(u => u.Elec_Other != "E" && u.Elec_Other != "O" && u.Cost != "");
                    if (caseactList.Count > 0)
                        ResAmount = caseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                }
                foreach (DataGridViewRow dr in dgvMonthsGrid.Rows)
                {
                    if (!string.IsNullOrEmpty(dr.Cells["gvAmtPaid"].Value.ToString().Trim()))
                    {
                        ResAmount = ResAmount + Convert.ToDecimal(dr.Cells["gvAmtPaid"].Value.ToString().Trim());
                    }
                }

                if (ResAmount > 0 && Total > 0)
                {
                    Bal = Total - ResAmount;
                    if (Bal < 0)
                    {
                        if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                        {
                            this.dgvMonthsGrid.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            dgvMonthsGrid.Rows[rowcalindex].Cells["gvAmtPaid"].Value = txtBalance.Text;
                            txtBalance.Text = "0";
                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                        }
                        else
                        {
                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);
                            dgvMonthsGrid.Rows[rowcalindex].Cells["gvAmtPaid"].Value = string.Empty;
                            dgvMonthsGrid.Rows[rowcalindex].Cells["gvSelect"].Value = false;
                            this.dgvMonthsGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.dgvMonthsGrid_CellValueChanged);

                            AlertBox.Show("we can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);

                            isAlert = true;
                        }
                    }
                    else
                    {
                        txtBalance.Text = Bal.ToString();
                        IsCal = true;
                    }
                }

            }
        }

        private bool Get_SNP_Vulnerable_Status()
        {
            Vulner_Flag = false;
            DateTime MST_Intake_Date = DateTime.Today, SNP_DOB = DateTime.Today;
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan Time_Span;
            int Age_In_years = 0;

            if (!string.IsNullOrEmpty(_baseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()))
                MST_Intake_Date = Convert.ToDateTime(_baseForm.BaseCaseMstListEntity[0].IntakeDate);
            string Non_Qual_Alien_SW = "N";
            foreach (CaseSnpEntity Entity in _baseForm.BaseCaseSnpEntity)
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

                if (Age_In_years > 59)
                    Age_Grt_60 = true;

                if (Age_In_years < 6)
                    Age_Les_6 = true;

                if (Entity.Disable == "Y")
                    Disable_Flag = true;

                if (Entity.FootStamps == "Y")
                    FoodStamps_Flag = true;

                if (Entity.SsnReason == "Q" && _baseForm.BaseAgencyControlDetails.State == "TX") Non_Qual_Alien_SW = "Y";
            }

            string Tmp_Age_Dis = string.Empty;

            if ((Age_Grt_60 || Age_Les_6 || Disable_Flag) && Non_Qual_Alien_SW == "N")
            {
                //if (Tmp_Age_Dis == "1" || Tmp_Age_Dis == "2" || Tmp_Age_Dis == "3")
                Vulner_Flag = true;

                if (Age_Les_6)
                    Vulner_Flag = true;
            }
            return Vulner_Flag;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
