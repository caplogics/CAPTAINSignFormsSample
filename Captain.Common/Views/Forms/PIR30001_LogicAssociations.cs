using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class PIR30001_LogicAssociations : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public PIR30001_LogicAssociations(BaseForm baseForm, PrivilegeEntity privileges, string Agency, string Dept, string Prog, string Year, string QFund, string QId,string colid, string Seq, string QType, Pirassn2Entity Pirassnentity, string strMode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            FillEntities();
            FillAllCombo();

            propAgency = Agency;
            propDept = Dept;
            propProg = Prog;
            propYear = Year;
            propQFund = QFund;
            propQId = QId;
           
            propSeq = Seq;
            propQType = QType;
            PropColId = colid;
             PirassnEntity = Pirassnentity;
            propMode = strMode;
            if (propMode == "Edit")
            {
                FillFormDetails();
                cmbGroup.Enabled = false;
            }

            CommonFunctions.SetComboBoxValue(cmbType, propQType);


        }

        #region Properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<ChldTrckEntity> propChldTrckEntity { get; set; }
        public List<CustRespEntity> propCustrespEntity { get; set; }
        public List<CaseHierarchyEntity> propCaseHieEntity { get; set; }
        public Pirassn2Entity PirassnEntity { get; set; }
        public string propAgency { get; set; }
        public string propDept { get; set; }
        public string propProg { get; set; }
        public string propYear { get; set; }
        public string propQFund { get; set; }
        public string propQId { get; set; }
        public string propQSec { get; set; }
        public string propGrp { get; set; }
        public string propSeq { get; set; }
        public string propQType { get; set; }
        public string propQCode { get; set; }
        public string propQScode { get; set; }
        public string propMode { get; set; }
        public string PropColId { get; set; }


        #endregion

        public void FillEntities()
        {
            propChldTrckEntity = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
            propCaseHieEntity = _model.AdhocData.Browse_CASEHIE("**", "**", "**", BaseForm.UserID, BaseForm.BaseAdminAgency);   //Vikash added UserID and AdminAgency 03/13/2023);
            CustRespEntity searchEntity = new CustRespEntity(true);
            searchEntity.ScrCode = "HSS00133";
            propCustrespEntity = _model.FieldControls.Browse_CUSTRESP(searchEntity, "Browse");
        }

        public void FillAllCombo()
        {
            cmbGroup.Items.Insert(0, new ListItem("", "0"));
            cmbGroup.Items.Insert(1, new ListItem("001", "001"));
            cmbGroup.Items.Insert(2, new ListItem("002", "002"));
            cmbGroup.Items.Insert(3, new ListItem("003", "003"));
            cmbGroup.Items.Insert(4, new ListItem("004", "004"));
            cmbGroup.Items.Insert(5, new ListItem("005", "005"));
            cmbGroup.SelectedIndex = 0;

            cmbConjunction.Items.Insert(0, new ListItem("", ""));
            //cmbConjunction.Items.Insert(1, new ListItem("None", ""));
            cmbConjunction.Items.Insert(1, new ListItem("Or", "O"));
            cmbConjunction.Items.Insert(2, new ListItem("And", "A"));
            cmbConjunction.SelectedIndex = 0;

            cmbYear2.Items.Insert(0, new ListItem("", "0"));
            cmbYear2.Items.Insert(1, new ListItem("Current", "C"));
            cmbYear2.Items.Insert(2, new ListItem("Previous", "P"));
            cmbYear2.Items.Insert(3, new ListItem("All Years", "A"));
            cmbYear2.SelectedIndex = 0;

            cmbDateKey.Items.Insert(0, new ListItem("", "N"));
            cmbDateKey.Items.Insert(1, new ListItem("Addressed", "A"));
            cmbDateKey.Items.Insert(2, new ListItem("Followup", "F"));
            cmbDateKey.Items.Insert(3, new ListItem("Completed", "C"));
            cmbDateKey.Items.Insert(4, new ListItem("Followup Complete", "3"));
            cmbDateKey.SelectedIndex = 0;

            cmbType.Items.Insert(0, new ListItem("", "N"));
            cmbType.Items.Insert(1, new ListItem("Child Tracking", "T"));
            cmbType.Items.Insert(2, new ListItem("Client Intake", "I"));
            cmbType.Items.Insert(3, new ListItem("Service/Activity", "S"));
            //cmbType.Items.Insert(4, new ListItem("HardCoded", "H"));
            cmbType.Items.Insert(4, new ListItem("Staff Master", "F"));
            //cmbType.SelectedIndex = 0;

            //cmbResponse.Items.Insert(0, new ListItem("", "0"));
            //cmbResponse.Items.Insert(1, new ListItem("No", "N"));
            //cmbResponse.Items.Insert(2, new ListItem("Yes", "Y"));
            //cmbResponse.SelectedIndex = 0;


            //FillIntakeCombo();
            //foreach (ChldTrckEntity item in propChldTrckEntity)
            //{
            //    cmbCondition.Items.Add(new ListItem(item.TASK +" - "+item.TASKDESCRIPTION, item.TASK));
            //}
            cmbCondition.Items.Insert(0, new ListItem("", "0"));
            cmbCondition.SelectedIndex = 0;


        }

        public void FillIntakeCombo()
        {
            cmbIntakeField.Items.Clear();

            cmbIntakeField.Items.Insert(0, new ListItem("", "0"));
            if (((ListItem)cmbType.SelectedItem).Value.ToString().Trim() == "F")
            {
                //cmbIntakeField.Items.Insert(1, new ListItem("Position", "00550"));
                //cmbIntakeField.Items.Insert(1, new ListItem("Ethnicity", "S0201"));
                //cmbIntakeField.Items.Insert(2, new ListItem("Race", "S0202"));
                //cmbIntakeField.Items.Insert(3, new ListItem("Education Completed", "S0034"));
                //cmbIntakeField.Items.Insert(4, new ListItem("Education Progress", "S0033"));
                //cmbIntakeField.Items.Insert(5, new ListItem("Reason for Termination", "S0204"));
                //cmbIntakeField.Items.Insert(6, new ListItem("Primary Language", "S0203"));
                //cmbIntakeField.Items.Insert(7, new ListItem("Language 1", "S0203"));
                //cmbIntakeField.Items.Insert(8, new ListItem("Language 2", "S0203"));
                //cmbIntakeField.Items.Insert(9, new ListItem("Language 3", "S0203"));
                cmbIntakeField.Items.Insert(10, new ListItem("Postional Category", "S0031"));
            }
            else
            {

                cmbIntakeField.Items.Insert(1, new ListItem("Education", "00007"));
                cmbIntakeField.Items.Insert(2, new ListItem("Ethnicity", "00352"));
                cmbIntakeField.Items.Insert(3, new ListItem("Family Types", "00008"));
                cmbIntakeField.Items.Insert(4, new ListItem("Food Stamps", "00020"));
                cmbIntakeField.Items.Insert(5, new ListItem("Housing Type", "00009"));
                cmbIntakeField.Items.Insert(6, new ListItem("Income Types", "00004"));
                cmbIntakeField.Items.Insert(7, new ListItem("Language", "00353"));
                cmbIntakeField.Items.Insert(8, new ListItem("Race", "00003"));
                cmbIntakeField.Items.Insert(9, new ListItem("Relationship", "03259"));
                if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                    cmbIntakeField.Items.Insert(10, new ListItem("Miltary Status", "00035"));
                else
                    cmbIntakeField.Items.Insert(10, new ListItem("Veteran", "00021"));
                cmbIntakeField.Items.Insert(11, new ListItem("WIC", "00030"));

                if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                    cmbIntakeField.Items.Insert(11, new ListItem("Non-Cash Benefits", "00037"));
            }


            cmbIntakeField.SelectedIndex = 0;
        }

        public void FillFormDetails()
        {
            if (PirassnEntity != null)
            {
                CommonFunctions.SetComboBoxValue(cmbType, PirassnEntity.PIRASSN_Q_TYPE);
                CommonFunctions.SetComboBoxValue(cmbIntakeField, PirassnEntity.PIRASSN_INTAKE_AGYTAB.Trim());
                if (PirassnEntity.PIRASSN_CHK_RESP == "1")
                    chkResponse.Checked = true;
                else chkResponse.Checked = false;
                if (PirassnEntity.PIRASSN_Q_TYPE == "I" || PirassnEntity.PIRASSN_Q_TYPE == "F")
                    CommonFunctions.SetComboBoxValue(cmbCondition, PirassnEntity.PIRASSN_INTAKE_CODE.Trim());
                else if (PirassnEntity.PIRASSN_Q_TYPE == "T")
                    CommonFunctions.SetComboBoxValue(cmbCondition, PirassnEntity.PIRASSN_TASK.Trim());
                else if (PirassnEntity.PIRASSN_Q_TYPE == "S")
                    CommonFunctions.SetComboBoxValue(cmbCondition, PirassnEntity.PIRASSN_SERVICE.Trim());
                CommonFunctions.SetComboBoxValue(cmbConjunction, PirassnEntity.PIRASSN_CONJ);
                CommonFunctions.SetComboBoxValue(cmbDateKey, PirassnEntity.PIRASSN_DATE_TYPE);
                CommonFunctions.SetComboBoxValue(cmbGroup, "000".Substring(0, 3 - PirassnEntity.PIRASSN_GRP.Length) + PirassnEntity.PIRASSN_GRP);
                if (PirassnEntity.PIRASSN_CHK_DATE == "1") { chkValidDate.Checked = true; chkDateRange.Checked = false; }
                else if (PirassnEntity.PIRASSN_CHK_DATE == "2") { chkValidDate.Checked = false; chkDateRange.Checked = true; }
                else { chkValidDate.Checked = false; chkDateRange.Checked = false; }

                CommonFunctions.SetComboBoxValue(cmbYear2, PirassnEntity.PIRASSN_YEAR_TYPE);
                if (PirassnEntity.PIRASSN_CHK_RESP == "1")
                {
                    //chkResponse.Checked = true;
                    CommonFunctions.SetComboBoxValue(cmbResponse, PirassnEntity.PIRASSN_RESPONSE);
                }
                //else
                //    chkResponse.Checked = false;

                //chkDateRange.Checked = ;
                //chkResponse.Checked=;
                //chkValidDate.Checked=;
                if (PirassnEntity.PIRASSN_FDATE != string.Empty)
                {
                    dtFromDate.Value = Convert.ToDateTime(PirassnEntity.PIRASSN_FDATE);
                    dtFromDate.Checked = true;
                }
                if (PirassnEntity.PIRASSN_TDATE != string.Empty)
                {
                    dtToDate.Value = Convert.ToDateTime(PirassnEntity.PIRASSN_TDATE);
                    dtToDate.Checked = true;
                }

            }
        }

        private void chkDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDateRange.Checked == false)
            {
                dtFromDate.Enabled = false; dtToDate.Enabled = false;
                dtFromDate.Checked = false; dtToDate.Checked = false;
                //chkValidDate.Checked = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbDateKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbDateKey.SelectedItem).Value.ToString() != "N")
            {
                chkDateRange.Enabled = true;
                chkValidDate.Enabled = true;
            }
            else
            {
                chkDateRange.Enabled = false;
                chkValidDate.Enabled = false;
                chkDateRange.Checked = false;
                chkValidDate.Checked = false;
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (((ListItem)cmbType.SelectedItem).Value.ToString() == "N")
            {
                _errorProvider.SetError(cmbType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblType.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbType, null);
            }

            if (((ListItem)cmbGroup.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbGroup, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGroup.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbGroup, null);
            }

            if (((ListItem)cmbCondition.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbCondition, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCondition.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbCondition, null);
            }
            if (((ListItem)cmbType.SelectedItem).Value.ToString() == "I")
            {
                if (((ListItem)cmbIntakeField.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(cmbIntakeField, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIntakeField.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbIntakeField, null);
                }
            }

            if (chkResponse.Checked == true)
            {
                if (cmbResponse.Visible == true && ((ListItem)cmbResponse.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(cmbResponse, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), chkResponse.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbResponse, null);
                }
            }

            if (((ListItem)cmbDateKey.SelectedItem).Value.ToString() != "N")
            {
                if (chkDateRange.Checked == false && chkValidDate.Checked == false)
                {
                    _errorProvider.SetError(chkDateRange, "Valid Date/Date Range".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                else if (chkDateRange.Checked == true)
                {
                    _errorProvider.SetError(chkDateRange, null);
                    if (dtFromDate.Checked.Equals(false))
                    {
                        _errorProvider.SetError(dtFromDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFromDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtFromDate, null);
                    }

                    if (dtToDate.Checked.Equals(false))
                    {
                        _errorProvider.SetError(dtToDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblToDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtToDate, null);
                    }

                    if (dtFromDate.Checked.Equals(true) && dtToDate.Checked.Equals(true))
                    {
                        if (!string.IsNullOrEmpty(dtFromDate.Text) && (!string.IsNullOrEmpty(dtToDate.Text)))
                        {
                            if (Convert.ToDateTime(dtFromDate.Text) > Convert.ToDateTime(dtToDate.Text))
                            {
                                _errorProvider.SetError(dtToDate, "To Date should be greater than From date ".Replace(Consts.Common.Colon, string.Empty));
                                isValid = false;
                            }
                            else
                            {
                                _errorProvider.SetError(dtToDate, null);
                            }
                        }
                    }
                }
                else _errorProvider.SetError(chkDateRange, null);
            }


            return (isValid);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbType.SelectedItem).Value.ToString() == "I")
            {
                lblIntakReq.Visible = true; lblIntakeField.Visible = true; FillIntakeCombo();
                cmbIntakeField.Enabled = true; cmbIntakeField.Visible = true;
                cmbDateKey.Enabled = false; cmbYear2.Enabled = false; //label10.Visible = false;
                chkResponse.Enabled = false; cmbResponse.Enabled = false;
            }
            else if (((ListItem)cmbType.SelectedItem).Value.ToString() == "T")
            {
                cmbCondition.Items.Clear();
                foreach (ChldTrckEntity item in propChldTrckEntity)
                {
                    string Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-35}", item.TASKDESCRIPTION.ToString().Trim()) + String.Format("{0,5}", " - " + item.TASK.ToString().Trim());
                    //cmbCondition.Items.Add(new ListItem(item.TASK + " - " + item.TASKDESCRIPTION, item.TASK));
                    cmbCondition.Items.Add(new ListItem(Tmp_Desc, item.TASK));
                }
                cmbCondition.Items.Insert(0, new ListItem("", "0"));
                cmbCondition.SelectedIndex = 0;
                lblIntakReq.Visible = false; lblIntakeField.Visible = false; cmbIntakeField.Visible = false;
                label7.Visible = true; cmbDateKey.Enabled = true; cmbYear2.Enabled = true; label10.Visible = true;
                chkResponse.Enabled = true; cmbResponse.Enabled = false; cmbIntakeField.Enabled = false;
                cmbYear2.Enabled = true;
            }
            else if (((ListItem)cmbType.SelectedItem).Value.ToString() == "S")
            {
                cmbCondition.Items.Clear();
                foreach (CaseHierarchyEntity item in propCaseHieEntity)
                {
                    if (string.IsNullOrEmpty(item.Dept.ToString().Trim()) && string.IsNullOrEmpty(item.Prog.ToString().Trim()))
                    {
                        string Tmp_Desc = string.Empty;
                        Tmp_Desc = String.Format("{0,-35}", item.HierarchyName.ToString().Trim()) + String.Format("{0,5}", " - " + item.Code.ToString().Trim());
                        cmbCondition.Items.Add(new ListItem(Tmp_Desc, item.Code));
                    }
                }
                cmbCondition.Items.Insert(0, new ListItem("", "0"));
                cmbCondition.SelectedIndex = 0; cmbIntakeField.Enabled = false;
                lblIntakReq.Visible = false; lblIntakeField.Visible = false; cmbIntakeField.Visible = false;
                label7.Visible = true; cmbDateKey.Enabled = false; //cmbYear2.Visible = false; label10.Visible = false;
                chkResponse.Enabled = false; cmbResponse.Enabled = false; cmbYear2.Enabled = false;
            }
            else if (((ListItem)cmbType.SelectedItem).Value.ToString() == "F")
            {
                FillIntakeCombo();
                lblIntakReq.Visible = true; lblIntakeField.Visible = true; cmbIntakeField.Visible = true; cmbIntakeField.Enabled = true;
                label7.Visible = true; cmbDateKey.Enabled = false; //cmbYear2.Visible = false; label10.Visible = false;
                chkResponse.Enabled = false; cmbResponse.Enabled = false; cmbYear2.Enabled = false;
            }
        }

        private void cmbIntakeField_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCondition.Items.Clear();
            if (((ListItem)cmbIntakeField.SelectedItem).Value.ToString().Trim() != "0")
            {
                List<CommonEntity> AgyTabs = CommonFunctions.AgyTabsFilterCodeStatus(BaseForm.BaseAgyTabsEntity, ((ListItem)cmbIntakeField.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty, string.Empty, "Add");

                if(AgyTabs.Count>0)
                {
                    foreach(CommonEntity Entity in AgyTabs)
                    {
                        ListItem li = new ListItem(Entity.Desc, Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red);
                        cmbCondition.Items.Add(li);
                    }
                    cmbCondition.Items.Insert(0, new ListItem("", "0"));
                    cmbCondition.SelectedIndex = 0;
                }

                //DataSet ds = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB(((ListItem)cmbIntakeField.SelectedItem).Value.ToString().Trim());
                //DataTable dt = ds.Tables[0];
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        string Tmp_Desc = string.Empty;
                //        Tmp_Desc = String.Format("{0,-35}", dr["LookUpDesc"].ToString().Trim()) + String.Format("{0,5}", " - " + dr["Code"].ToString().Trim());
                //        cmbCondition.Items.Add(new ListItem(Tmp_Desc, dr["Code"].ToString().Trim()));
                //    }
                //    cmbCondition.Items.Insert(0, new ListItem("", "0"));
                //    //if (((ListItem)cmbType.SelectedItem).Value.ToString() == "I" || ((ListItem)cmbType.SelectedItem).Value.ToString() == "F")
                //    //    cmbCondition.SelectedIndex = 1;
                //    //else
                //    cmbCondition.SelectedIndex = 0;
                //}
            }
        }

        private void chkResponse_CheckedChanged(object sender, EventArgs e)
        {
            FillResponse();
            if (chkResponse.Checked == true)
                label1.Visible = true;
            else label1.Visible = false;
        }

        public void FillResponse()
        {
            cmbResponse.Items.Clear();
            cmbResponse.Items.Add(new ListItem("", "0"));
            if (chkResponse.Checked == true && ((ListItem)cmbType.SelectedItem).Value.ToString() == "T")
            {
                string CustCode = string.Empty;
                foreach (ChldTrckEntity item in propChldTrckEntity)
                {
                    if (item.TASK.Trim() == ((ListItem)cmbCondition.SelectedItem).Value.ToString().Trim())
                    {
                        CustCode = item.CustQCodes.Trim(); break;
                    }
                }
                List<CustRespEntity> RespEntity = propCustrespEntity.FindAll(u => u.ResoCode.Trim().Equals(CustCode));
                if (RespEntity.Count > 0)
                {
                    cmbResponse.Visible = true; cmbResponse.Enabled = true; txtResp.Visible = false;
                    foreach (CustRespEntity resp in RespEntity)
                    {
                        cmbResponse.Items.Add(new ListItem(resp.RespDesc.Trim(), resp.DescCode.Trim()));
                    }
                }
                else
                {
                    txtResp.Visible = true; cmbResponse.Visible = false;
                    this.txtResp.Location = new System.Drawing.Point(350, 107);
                }
            }
            else
                cmbResponse.Enabled = false;
            cmbResponse.SelectedIndex = 0;
        }

        private void cmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillResponse();
        }

        private void chkDateRange_Click(object sender, EventArgs e)
        {
            if (chkDateRange.Checked == true)
            {
                dtFromDate.Enabled = true;
                dtToDate.Enabled = true;
                if (chkValidDate.Checked == true) chkValidDate.Checked = false;
            }
        }

        private void chkValidDate_Click(object sender, EventArgs e)
        {
            if (chkValidDate.Checked == true)
            {
                dtFromDate.Enabled = false;
                dtToDate.Enabled = false;
                if (chkDateRange.Checked == true) chkDateRange.Checked = false;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                string Chkdate = string.Empty;
                Pirassn2Entity Pirassn2 = new Pirassn2Entity();
                Pirassn2.PIRASSN_AGENCY = propAgency;
                Pirassn2.PIRASSN_DEPT = propDept;
                Pirassn2.PIRASSN_PROG = propProg;
                Pirassn2.PIRASSN_YEAR = propYear;
                Pirassn2.PIRASSN_Q_FUND = propQFund;
                Pirassn2.PIRASSN_Q_ID = propQId;
                Pirassn2.PIRASSN_COL_ID = PropColId;

                if (((ListItem)cmbGroup.SelectedItem).Value.ToString().Trim() != "0")
                    Pirassn2.PIRASSN_GRP = ((ListItem)cmbGroup.SelectedItem).Value.ToString();

                if (propMode == "Edit")
                    Pirassn2.PIRASSN_SEQ = propSeq;

                Pirassn2.PIRASSN_Q_TYPE = propQType;

                Pirassn2.PIRASSN_CONJ = ((ListItem)cmbConjunction.SelectedItem).Value.ToString();
                if (string.IsNullOrEmpty(Pirassn2.PIRASSN_CONJ.Trim()))
                    Pirassn2.PIRASSN_CONJ = "O";

                if (propQType == "T")
                    Pirassn2.PIRASSN_TASK = ((ListItem)cmbCondition.SelectedItem).Value.ToString();
                else if (propQType == "I" || propQType == "F")
                {
                    Pirassn2.PIRASSN_INTAKE_AGYTAB = ((ListItem)cmbIntakeField.SelectedItem).Value.ToString().Trim();

                    if (((ListItem)cmbCondition.SelectedItem).Value.ToString().Trim() != "0")
                        Pirassn2.PIRASSN_INTAKE_CODE = ((ListItem)cmbCondition.SelectedItem).Value.ToString().Trim();
                }
                else if (propQType == "S")
                {
                    if (((ListItem)cmbCondition.SelectedItem).Value.ToString().Trim() != "0")
                        Pirassn2.PIRASSN_SERVICE = ((ListItem)cmbCondition.SelectedItem).Value.ToString().Trim();
                }
                if (((ListItem)cmbYear2.SelectedItem).Value.ToString().Trim() != "0")
                    Pirassn2.PIRASSN_YEAR_TYPE = ((ListItem)cmbYear2.SelectedItem).Value.ToString();

                Pirassn2.PIRASSN_CHK_RESP = chkResponse.Checked == true ? "1" : "0";
                if (chkResponse.Checked == true)
                {
                    if (((ListItem)cmbResponse.SelectedItem).Value.ToString().Trim() != "0")
                        Pirassn2.PIRASSN_RESPONSE = ((ListItem)cmbResponse.SelectedItem).Value.ToString();
                }

                if (((ListItem)cmbDateKey.SelectedItem).Value.ToString().Trim() != "N")
                    Pirassn2.PIRASSN_DATE_TYPE = ((ListItem)cmbDateKey.SelectedItem).Value.ToString();

                if (chkValidDate.Checked == true)
                    Chkdate = "1";
                else if (chkDateRange.Checked == true)
                    Chkdate = "2";
                else
                    Chkdate = "0";
                Pirassn2.PIRASSN_CHK_DATE = Chkdate.Trim();

                if (dtFromDate.Checked)
                    Pirassn2.PIRASSN_FDATE = dtFromDate.Value.ToShortDateString();
                if (dtToDate.Checked)
                    Pirassn2.PIRASSN_TDATE = dtToDate.Value.ToShortDateString();

                Pirassn2.PIRASSN_ADD_OPERATOR = BaseForm.UserID;

                Pirassn2.PIRASSN_LSTC_OPERATOR = BaseForm.UserID;

                Pirassn2.Mode = propMode;

                if (_model.EnrollData.InsertUpdatePIRASSN_2(Pirassn2))
                {
                    PIR30001Control pirControl = BaseForm.GetBaseUserControl() as PIR30001Control;
                    if (pirControl != null)
                    {
                        pirControl.RefreshGroupsGrid(propQSec + ((ListItem)cmbGroup.SelectedItem).Value.ToString() + propSeq + propQCode);
                    }
                    this.Close();
                    AlertBox.Show("Saved Successfully");
                }
            }
        }
    }
}
