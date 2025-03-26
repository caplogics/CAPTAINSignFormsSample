using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class INKIND20_Form_v2 : Form
    {
        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;
        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        public BaseForm _baseform { get; set; }

        public PrivilegeEntity _privilages { get; set; }

        public string Mode { get; set; }

        public string FormType { get; set; }

        public string IKM_Code { get; set; }
        public string IKD_Seq { get; set; }
        public bool IsSaveValid { get; set; }
        #endregion
        public INKIND20_Form_v2(BaseForm baseform, string mode, string FormName, string Code, PrivilegeEntity privilegeEntity, string Seq)
        {
            InitializeComponent();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            _baseform = baseform;
            _privilages = privilegeEntity;
            Mode = mode;
            IKM_Code = Code;
            FormType = FormName;
            IKD_Seq = Seq;
            txtQty.Validator = TextBoxValidation.CustomDecimalValidation9dot2;
            txtRate.Validator = TextBoxValidation.CustomDecimalValidation9dot2;
            FillCombos();

            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");


            if (Mode == "Add")
                this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
            else if (Mode == "UPDATE")
            {
                this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                FillControls();
            }


        }

        List<CommonEntity> lstServArea = new List<CommonEntity>();
        void FillCombos()
        {
            //*********** SERVICES COMBO ***************//
            List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "05558", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, Mode);
            cmbService.Items.Insert(0, new ListItem("Select One", "0"));
            cmbService.ColorMember = "FavoriteColor";
            cmbService.SelectedIndex = 0;
            foreach (CommonEntity Etncity in Ethnicity)
            {
                ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbService.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbService.SelectedItem = li;
            }


            //*********** SERVICES AREA COMBO ***************//
            lstServArea = new List<CommonEntity>();
            lstServArea = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "05560", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, Mode);
            cmbServArea.Items.Insert(0, new ListItem("Select One", "0"));
            cmbServArea.ColorMember = "FavoriteColor";
            cmbServArea.SelectedIndex = 0;
            foreach (CommonEntity itmServArea in lstServArea)
            {
                ListItem li = new ListItem(itmServArea.Desc, itmServArea.Code, itmServArea.Active, itmServArea.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbServArea.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && itmServArea.Default.Equals("Y")) cmbServArea.SelectedItem = li;
            }


            //*****************  UOM Filling COMBO *************************//
            List<CommonEntity> DonorUOM = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "05562", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, Mode);
            cmbUOM.Items.Insert(0, new ListItem(" ", "0"));
            cmbUOM.ColorMember = "FavoriteColor";

            foreach (CommonEntity entyDonor in DonorUOM)
            {
                ListItem li = new ListItem(entyDonor.Desc, entyDonor.Code, entyDonor.Active, entyDonor.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbUOM.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && entyDonor.Default.Equals("Y")) cmbUOM.SelectedItem = li;

                cmbUOM.SelectedIndex = 0;
            }
        }
        public List<CommonEntity> propfundingSource { get; set; }
        void FillControls()
        {
            INKINDDTLSEntity Search_MST = new INKINDDTLSEntity(true);
            Search_MST.IKDL_AGENCY = _baseform.BaseAgency;
            Search_MST.IKDL_CODE = IKM_Code;
            Search_MST.IKDL_SEQ = IKD_Seq;
            List<INKINDDTLSEntity> MST_List = _model.INKINDData.Get_INKINDTLS(Search_MST, "ALL");
            if (MST_List.Count > 0)
            {
                SetComboBoxValue(cmbService, MST_List[0].IKDL_SERVICE_TYPE.Trim());
                SetComboBoxValue(cmbServArea, MST_List[0].IKDL_SERVICE_AREA.Trim());

                if (!string.IsNullOrEmpty(MST_List[0].IKDL_SERVICE_DATE.Trim()))
                {
                    dtpDate.Checked = true;
                    dtpDate.Text = MST_List[0].IKDL_SERVICE_DATE.Trim();
                }

                SelFundingList.Clear();

                if (MST_List[0].IKDL_PROG.ToString() == string.Empty)
                {
                    rbtnPrgAll.Checked = true;
                    rbtnPrgSel.Checked = false;
                }
                else
                {
                    rbtnPrgAll.Checked = false;
                    rbtnPrgSel.Checked = true;

                    string strFunds = MST_List[0].IKDL_PROG.ToString();
                    if (strFunds != null)
                    {
                        string[] FundCodes = strFunds.Split(',');
                        for (int i = 0; i < FundCodes.Length; i++)
                        {
                            //CommonEntity fundDetails = commonfundingsource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                            CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                            SelFundingList.Add(fundDetails);
                        }
                    }
                    SelFundingList = SelFundingList;
                }

                Txt_DrawEnroll_Site.Text = MST_List[0].IKDL_SITE;
                Txt_DrawEnroll_AMPM.Text = MST_List[0].IKDL_AMPM;
                Txt_DrawEnroll_Room.Text = MST_List[0].IKDL_ROOM;

                txtQty.Text = MST_List[0].IKDL_QUANTITY;
                SetComboBoxValue(cmbUOM, MST_List[0].IKDL_UOM.Trim());
                txtRate.Text = MST_List[0].IKDL_RATE;//lblRate.Text = MST_List[0].IKDL_RATE;
                if (MST_List[0].IKDL_UOM.Trim() == "D")
                    lblAmt.Text = "$" + MST_List[0].IKDL_AMOUNT;
                else
                    lblAmt.Text = MST_List[0].IKDL_AMOUNT;

                chkFatherAct.Checked = false;
                if (MST_List[0].IKDL_FATHER == "Y")
                    chkFatherAct.Checked = true;

                chkInkdClassrom.Checked = false;
                if (MST_List[0].IKDL_CLASS_ROOM == "Y")
                    chkInkdClassrom.Checked = true;

                //txtInkind.Text = MST_List[0].TOTAL_INKIND.Trim();
                //txtMileage.Text = MST_List[0].TOTAL_MILEAGE.Trim();
                //txtPSTime.Text = MST_List[0].SERVICE_TIME.Trim();
                //txtPTime.Text = MST_List[0].TRANSPORT_TIME.Trim();
                //txtRTMDriven.Text = MST_List[0].MILES_DRIVEN.Trim();
                //txtTotServices.Text = MST_List[0].TOTAL_SERVICE.Trim();

                //if (!string.IsNullOrEmpty(MST_List[0].Site.Trim()))
                //    txtSite.Text = MST_List[0].Site.Trim();

                ShowCaseNotesImages(Search_MST);
            }
        }
        public string[] GetSelected_Service_Code()
        {
            string[] Added_Edited_InkindSCode = new string[3];

            if (Mode == "UPDATE")
                Added_Edited_InkindSCode[2] = IKD_Seq;
            //else
            //    Added_Edited_InkindSCode[2] = strmsgGrp;
            Added_Edited_InkindSCode[1] = Mode;
            Added_Edited_InkindSCode[0] = IKM_Code;

            return Added_Edited_InkindSCode;
        }
        public List<CaseNotesEntity> caseNotesEntity { get; set; }
        private void ShowCaseNotesImages(INKINDDTLSEntity Entity)
        {

            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(_privilages.Program, Entity.IKDL_AGENCY + "  " + "  " + Entity.IKDL_CODE + Entity.IKDL_SEQ);
            if (caseNotesEntity.Count > 0)
            {
                // pbCaseNotes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesView);
                txtCaseNotes.Text = caseNotesEntity[0].Data.ToString().Trim();
            }
            else
            {
                txtCaseNotes.Text = string.Empty;
                // pbCaseNotes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesNew);
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
        private void cmbServArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRate.Text = "";
            txtRate.Enabled = true;//**lblRate.Text = ""; 
            lblAmt.Text = "";
            if (cmbServArea.SelectedIndex > 0)
            {
                string _code = ((ListItem)cmbServArea.SelectedItem).Value.ToString().Trim();

                List<Agy_Ext_Entity> PAYMENTService = _model.SPAdminData.Get_AgyRecs_With_Ext("05560", "", "", "1", "");
                //List<SPCommonEntity> lstExtenty = _model.SPAdminData.GetLookUpFromAGYTAB_EXT("05560", "A1");
                List<Agy_Ext_Entity> lstItems = PAYMENTService.Where(x => x.Code == _code).ToList();
                if (lstItems.Count > 0)
                {
                    //** lblRate.Text = Convert.ToDecimal(lstItems[0].Ext1_A == "" ? "0" : lstItems[0].Ext1_A).ToString("#0.00");
                    txtRate.Text = Convert.ToDecimal(lstItems[0].Ext1_A == "" ? "0" : lstItems[0].Ext1_A).ToString("#0.00");
                }

                if (txtRate.Text == "0.00")
                    txtRate.Enabled = true;
                else
                    txtRate.Enabled = true;

                if (Mode == "UPDATE")
                    txtRate.Enabled = true;

                CalcAmount();
            }
        }
        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        private void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelFundingList = form.GetSelectedFundings();
            }
        }

        private void rbtnPrgSel_Click(object sender, EventArgs e)
        {
            if (rbtnPrgSel.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(_baseform, SelFundingList, _privilages, 1);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        private void Pb_Withdraw_Enroll_Click(object sender, EventArgs e)
        {
            Site_SelectionForm SiteSelection = new Site_SelectionForm(_baseform, "Room", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, _baseform.BaseYear, _privilages, "****");
            SiteSelection.FormClosed += new FormClosedEventHandler(WithdrawEnroll_Site_AddForm_Closed);
            SiteSelection.StartPosition = FormStartPosition.CenterScreen;
            SiteSelection.ShowDialog();
        }
        private void WithdrawEnroll_Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Row();
                Txt_DrawEnroll_Site.Text = From_Results[0];
                Txt_DrawEnroll_Room.Text = From_Results[1];
                Txt_DrawEnroll_AMPM.Text = From_Results[2];

                //if (Txt_DrawEnroll_Room.Text.Trim() == "****")
                //    Txt_DrawEnroll_Site.Text = Pass_Enroll_List[0].Mst_Site.Trim();
            }
        }

        private void btnSCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbtnPrgAll_Click(object sender, EventArgs e)
        {
            SelFundingList.Clear();
        }

        decimal CalcAmount()
        {
            decimal amount = 0;
            if (cmbUOM.Items.Count > 1)
            {
                try
                {
                    decimal _Rate = 0;
                    decimal _Qty = 0;
                    string _UOMtype = ((ListItem)cmbUOM.SelectedItem).Value.ToString().Trim();
                    if (_UOMtype != "0")
                    {
                        //**if (lblRate.Text != "")
                        //**    _Rate = Convert.ToDecimal(lblRate.Text);
                        if (txtRate.Text != "")
                            _Rate = Convert.ToDecimal(txtRate.Text);
                        if (txtQty.Text != "")
                            _Qty = Convert.ToDecimal(txtQty.Text);

                        amount = _Rate * _Qty;
                        if (_UOMtype == "D")
                        {
                            lblAmt.Text = "$" + Convert.ToDecimal(amount).ToString("#0.00");
                        }
                        else if (_UOMtype == "H")
                        {
                            lblAmt.Text = Convert.ToDecimal(amount).ToString("#0.00");
                        }
                    }
                    else
                        lblAmt.Text = "";
                }
                catch
                {

                }
            }

            return amount;
        }

        private void cmbUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcAmount();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {

        }

        string SqlMsg = string.Empty;
        private void btnSSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    INKINDDTLSEntity Entity = new INKINDDTLSEntity();
                    Entity.IKDL_AGENCY = _baseform.BaseAgency;
                    Entity.IKDL_DEPT = "  ";
                    Entity.IKDL_PROGRAM = "  ";
                    Entity.IKDL_CODE = IKM_Code;
                    if (Mode == "UPDATE")
                        Entity.IKDL_SEQ = IKD_Seq;
                    else
                    {
                        Entity.IKDL_SEQ = "";
                    }

                    Entity.IKDL_SERVICE_TYPE = ((ListItem)cmbService.SelectedItem).Value.ToString().Trim();
                    Entity.IKDL_SERVICE_AREA = ((ListItem)cmbServArea.SelectedItem).Value.ToString().Trim();
                    if (dtpDate.Checked == true)
                        Entity.IKDL_SERVICE_DATE = dtpDate.Text.ToString().Trim();
                    Entity.IKDL_PROG = getPrograms();
                    Entity.IKDL_SITE = Txt_DrawEnroll_Site.Text;
                    Entity.IKDL_ROOM = Txt_DrawEnroll_Room.Text;
                    Entity.IKDL_AMPM = Txt_DrawEnroll_AMPM.Text;
                    Entity.IKDL_FATHER = (chkFatherAct.Checked ? "Y" : "N");
                    Entity.IKDL_CLASS_ROOM = (chkInkdClassrom.Checked ? "Y" : "N");
                    Entity.IKDL_QUANTITY = txtQty.Text == "" ? "0.00" : txtQty.Text;
                    Entity.IKDL_RATE = txtRate.Text == "" ? "0.00" : txtRate.Text;//lblRate.Text;
                                                                                  //if(((ListItem)cmbUOM.SelectedItem).Value.ToString().Trim()!="0")
                    Entity.IKDL_UOM = ((ListItem)cmbUOM.SelectedItem).Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(lblAmt.Text.Trim()))
                        Entity.IKDL_AMOUNT = lblAmt.Text.Replace("$", "");
                    Entity.IKDL_LSTC_OPERATOR = _baseform.UserID;

                    if (_model.INKINDData.Insupdel_INKINDDTLS(Entity, Mode.ToUpper(), out SqlMsg))
                    {
                        string strSeq = SqlMsg;// string.Empty;
                        if (Mode == "UPDATE")
                        {
                            strSeq = Entity.IKDL_SEQ;
                        }
                        else
                        {
                            // strSeq = strmsgGrp;
                        }

                        if (!string.IsNullOrEmpty(txtCaseNotes.Text))
                        {


                            InsertDelCaseNotes(_baseform.BaseAgency + "  " + "  " + IKM_Code + strSeq, _baseform.BaseAgency + "  " + "  " + IKM_Code, string.Empty);
                        }
                        else
                        {
                            InsertDelCaseNotes(_baseform.BaseAgency + "  " + "  " + IKM_Code + strSeq, _baseform.BaseAgency + "  " + "  " + IKM_Code, "Del");
                        }
                        AlertBox.Show("Saved Successfully");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }

                }
            }
            catch
            {

            }
        }
        private void InsertDelCaseNotes(string FiledName, string ApplicationNo, string strMode)
        {

            CaseNotesEntity caseNotesDetails = new CaseNotesEntity();
            caseNotesDetails.ScreenName = _privilages.Program;
            caseNotesDetails.FieldName = FiledName;
            caseNotesDetails.AppliCationNo = ApplicationNo;
            caseNotesDetails.Data = txtCaseNotes.Text.Trim();
            caseNotesDetails.AddOperator = _baseform.UserID;
            caseNotesDetails.LstcOperation = _baseform.UserID;
            if (strMode == "Del")
            {
                caseNotesDetails.Mode = "Del";
            }
            if (_model.TmsApcndata.InsertUpdateCaseNotes(caseNotesDetails))
            {
            }


        }
        string getPrograms()
        {
            string strFundingCodes = string.Empty;
            if (rbtnPrgSel.Checked == true)
            {
                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }
            }
            else
            {
                strFundingCodes = string.Empty;
            }
            return strFundingCodes.TrimEnd(',');
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (((ListItem)cmbService.SelectedItem).Value.ToString().Trim() == "0")
            {
                _errorProvider.SetError(cmbService, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblService.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbService, null);

            if (((ListItem)cmbServArea.SelectedItem).Value.ToString().Trim() == "0")
            {
                _errorProvider.SetError(cmbServArea, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblServiceArea.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbServArea, null);

            if (dtpDate.Checked == false)
            {
                _errorProvider.SetError(dtpDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else if (dtpDate.Checked == true)
            {
                if (dtpDate.Text == String.Empty)
                {
                    _errorProvider.SetError(dtpDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(dtpDate, null);
            }
            else
                _errorProvider.SetError(dtpDate, null);


            if (string.IsNullOrEmpty(Txt_DrawEnroll_Site.Text) || string.IsNullOrWhiteSpace(Txt_DrawEnroll_Site.Text))
            {
                _errorProvider.SetError(WithdrawEnroll_Site_Panel, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSite.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(WithdrawEnroll_Site_Panel, null);



            if (rbtnPrgSel.Checked)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(panel3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label5.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(panel3, null);
            }


            if (txtQty.Text != "")
            {
                if (Convert.ToDecimal(txtQty.Text) > 0)
                {
                    if (((ListItem)cmbUOM.SelectedItem).Value.ToString().Trim() == "0")
                    {
                        _errorProvider.SetError(cmbUOM, "UOM is required");
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(cmbUOM, null);
                }
                else
                    _errorProvider.SetError(cmbUOM, null);
            }
            else
                _errorProvider.SetError(cmbUOM, null);



            IsSaveValid = isValid;
            return (isValid);
        }

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtRate_Leave(object sender, EventArgs e)
        {
            if (txtRate.Text.StartsWith("."))
            {
                txtRate.Text = "0" + txtRate.Text;
            }
            if (txtRate.Text.EndsWith("."))
            {
                txtRate.Text = txtRate.Text + "0";
            }
            if (txtRate.Text == ".")
            {
                txtRate.Text = "0";
            }
            CalcAmount();
        }

        private void txtQty_Leave(object sender, EventArgs e)
        {
            if (txtQty.Text.StartsWith("."))
            {
                txtQty.Text = "0" + txtQty.Text;
            }
            if (txtQty.Text.EndsWith("."))
            {
                txtQty.Text = txtQty.Text + "0";
            }
            if (txtQty.Text == ".")
            {
                txtQty.Text = "0";
            }
            CalcAmount();
        }
    }
}
