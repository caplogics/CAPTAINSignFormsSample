#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using DevExpress.CodeParser;

#endregion


namespace Captain.Common.Views.Forms
{
    public partial class SSBGQuestions_Form : Form
    {
        #region private variables

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public SSBGQuestions_Form(BaseForm baseForm, PrivilegeEntity privilieges, string strtype, string strID, string strSeq, string strTypeCode, string strHierchyCode)
        {
            InitializeComponent();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            this.Text = Privileges.PrivilegeName;//Privileges.Program;
            strType = strtype;
            ID = strID;
            CodeSeq = strSeq;
            GroupCode = "0";
            TypeCode = strTypeCode;
            strHierachycode = strHierchyCode;
            txtEqualTo.Validator = TextBoxValidation.FloatValidator;
            txtLessthan.Validator = TextBoxValidation.FloatValidator;
            txtGreaterthan.Validator = TextBoxValidation.FloatValidator;

            fillcombo();
            if (strtype == "Groups")
            {
                FillComboGroupType();
                CommonFunctions.SetComboBoxValue(cmbGrpType, TypeCode);
                
            }
            else if (strType == "Questions")
            {
                //lblHeader.Text = "SSBG Conditions";
                //pnlGroup.Visible = false; pnlCntltypes.Visible = false;
                pnlQuestions.Visible = true;
                //this.pnlQuestions.Location = new System.Drawing.Point(0, 51);
                //this.Size = new System.Drawing.Size(549, 170);
                fillcombo();
                FillComboType();
                //fillComboGroups();
                CommonFunctions.SetComboBoxValue(cmbType, TypeCode);
            }
        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string strType { get; set; }

        public string ID { get; set; }
        public string CodeSeq { get; set; }
        public string TypeCode { get; set; }
        public string QuesCode { get; set; }
        public string strHierachycode { get; set; }
        public string Mode { get; set; }
        public string GroupCode { get; set; }


        #endregion

        private void fillcombo()
        {
            this.cmbQuestions.SelectedIndexChanged -= new System.EventHandler(this.cmbQuestions_SelectedIndexChanged);
            if (strType == "Questions")
            {
                cmbQuestions.Items.Clear();
                List<CaseELIGQUESEntity> EligData = _model.CaseSumData.GetELIGQUES();
                cmbQuestions.Items.Insert(0, new ListItem("    ", "0"));
                foreach (CaseELIGQUESEntity EligQues in EligData)
                {
                    ListItem li = new ListItem(EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFileName, Consts.Common.Standard);
                    cmbQuestions.Items.Add(li);
                    // if (Mode.Equals(Consts.Common.Add)) cmbQuestions.SelectedItem = li;
                }
                List<CaseELIGQUESEntity> EligCustomData = _model.CaseSumData.GetELIGCUSTOMQUES(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2));
                foreach (CaseELIGQUESEntity EligQues in EligCustomData)
                {
                    ListItem li = new ListItem(EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFliedType, Consts.Common.Custom);
                    cmbQuestions.Items.Add(li);
                }
                cmbQuestions.SelectedIndex = 0;
            }
            this.cmbQuestions.SelectedIndexChanged += new System.EventHandler(this.cmbQuestions_SelectedIndexChanged);
            cmbConjunction.Items.Clear();
            cmbQuestionConjuction.Items.Clear();
            List<CommonEntity> commonconjunction = _model.lookupDataAccess.GetConjunctions();
            foreach (CommonEntity conjunction in commonconjunction)
            {
                cmbQuestionConjuction.Items.Add(new ListItem(conjunction.Desc, conjunction.Code));
                cmbConjunction.Items.Add(new ListItem(conjunction.Desc, conjunction.Code));
            }
            cmbQuestionConjuction.Items.Insert(0, new ListItem("    ", "0"));
            cmbQuestionConjuction.SelectedIndex = 0;
            cmbConjunction.Items.Insert(0, new ListItem("    ", "0"));
            cmbConjunction.SelectedIndex = 0;

        }

        private void FillComboType()
        {
            List<SSBGTYPESEntity> SSBGTypesall = _model.CaseSumData.GetSSBGTypes();
            List<SSBGTYPESEntity> caseEligGroups = SSBGTypesall.FindAll(u => u.SBGTID.Equals(ID) && u.SBGTCodeSeq.Equals("0"));
            int Tmp_Count = 0;
            this.cmbType.SelectedIndexChanged -= new System.EventHandler(this.cmbType_SelectedIndexChanged);
            cmbType.Items.Clear();
            if (caseEligGroups.Count > 0)
            {
                foreach (SSBGTYPESEntity Entity in caseEligGroups)
                {
                    cmbGrpType.Items.Add(new ListItem(Entity.SBGTDesc, Entity.SBGTCode));
                    cmbType.Items.Add(new ListItem(Entity.SBGTDesc, Entity.SBGTCode));
                    Tmp_Count++;
                }
            }
            if (Tmp_Count > 0)
                cmbType.SelectedIndex = int.Parse(TypeCode) - 1;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
        }

        private void fillComboGroups()
        {
            this.cmbQues_Group.SelectedIndexChanged -= new System.EventHandler(this.cmbQues_Group_SelectedIndexChanged_1);
            List<SSBGTYPESEntity> SSBGTypesGroups = _model.CaseSumData.GetSSBGTypesadpgs(ID, ((ListItem)cmbType.SelectedItem).Value.ToString(),string.Empty,string.Empty, string.Empty, "Group");
            int Tmp_Count = 0; cmbQues_Group.Items.Clear();
            if (SSBGTypesGroups.Count > 0)
            {
                foreach (SSBGTYPESEntity Entity in SSBGTypesGroups)
                {
                    cmbQues_Group.Items.Add(new ListItem(Entity.SBGTDesc, Entity.SBGTGroup,Entity.SBGTCodeSeq,string.Empty));
                    Tmp_Count++;
                }
            }
            this.cmbQues_Group.SelectedIndexChanged += new System.EventHandler(this.cmbQues_Group_SelectedIndexChanged_1);
            if (Tmp_Count > 0)
            {
                cmbQues_Group.SelectedIndex = 0;
                //picAddQues.Enabled = true;
            }
            else
            {
                picAddQues.Enabled = false;
                gvwQuestions.Rows.Clear();
            }
           
        }

        private void FillComboGroupType()
        {
            List<SSBGTYPESEntity> SSBGTypesall = _model.CaseSumData.GetSSBGTypes();
            List<SSBGTYPESEntity> caseEligGroups = SSBGTypesall.FindAll(u => u.SBGTID.Equals(ID) && u.SBGTCodeSeq.Equals("0"));

            int Tmp_Count = 0; cmbGrpType.Items.Clear();
            if (caseEligGroups.Count > 0)
            {
                foreach (SSBGTYPESEntity Entity in caseEligGroups)
                {
                    cmbGrpType.Items.Add(new ListItem(Entity.SBGTDesc, Entity.SBGTCode));
                    Tmp_Count++;
                }
            }
            if (Tmp_Count > 0)
                cmbGrpType.SelectedIndex = int.Parse(TypeCode) - 1;

        }

        public void fillGroupsgrid()
        {
            string strConjunction = string.Empty;
            if (!string.IsNullOrEmpty(((ListItem)cmbGrpType.SelectedItem).Value.ToString()))
            {
                gvMat_Groups.Rows.Clear();
                List<SSBGTYPESEntity> SSBGTypesGroups = _model.CaseSumData.GetSSBGTypesadpgs(ID, ((ListItem)cmbGrpType.SelectedItem).Value.ToString(),string.Empty,string.Empty, string.Empty, "Group");
                //List<SSBGTYPESEntity> caseEligQuestions = SSBGTypesQues.FindAll(u => u.SBGTID.Equals(ID) && !u.SBGTCodeSeq.Equals("0") && (u.SBGTCode.Equals(((ListItem)cmbType.SelectedItem).Value.ToString())));
               
                if (SSBGTypesGroups.Count > 0)
                {
                    foreach (var caseEligquestion in SSBGTypesGroups)
                    {
                        strConjunction = string.Empty;
                        if (caseEligquestion.SBGTConj.Trim() != string.Empty)
                            strConjunction = caseEligquestion.SBGTConj == "A" ? "And" : "Or";

                        int rowIndex = gvMat_Groups.Rows.Add(caseEligquestion.SBGTGroup, caseEligquestion.SBGTDesc, strConjunction,caseEligquestion.SBGTCodeSeq);

                        CommonFunctions.setTooltip(rowIndex, caseEligquestion.AddOperator, caseEligquestion.DateAdd, caseEligquestion.LstcOperator, caseEligquestion.DateLstc, gvMat_Groups);
                    }

                }
            }
            if (gvMat_Groups.Rows.Count > 0)
            {
                //if (Privileges.ChangePriv.Equals("false"))
                //{
                //    gvMat_Groups.Columns["Edit"].Visible = false;

                //}
                //else
                //{
                //    gvMat_Groups.Columns["Edit"].Visible = true;
                //}
                //if (Privileges.DelPriv.Equals("false"))
                //{
                //    gvMat_Groups.Columns["Delete"].Visible = false;

                //}
                //else
                //{
                //    gvMat_Groups.Columns["Delete"].Visible = true;
                //}
                gvMat_Groups.Rows[0].Selected = true;
            }

        }

        public void fillGridQuestions()
        {
            string strResponceValue = string.Empty;
            string strConjunction = string.Empty;
            if(!string.IsNullOrEmpty(((ListItem)cmbType.SelectedItem).Value.ToString()))
            {
                gvwQuestions.Rows.Clear();
                //if (dr.Selected)
                //{
                    //string strHierchycode = gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString();
                List<SSBGTYPESEntity> SSBGTypesQues = _model.CaseSumData.GetSSBGTypesadpgs(string.Empty, string.Empty,string.Empty,string.Empty, string.Empty, string.Empty);
                List<SSBGTYPESEntity> caseEligQuestions = SSBGTypesQues.FindAll(u => u.SBGTID.Equals(ID) && !u.SBGTCodeSeq.Equals("0") && (u.SBGTCode.Equals(((ListItem)cmbType.SelectedItem).Value.ToString())) && (u.SBGTGroup.Equals(((ListItem)cmbQues_Group.SelectedItem).Value.ToString())) && !u.SBGTGroupSeq.Equals("0"));


                    foreach (var caseEligquestion in caseEligQuestions)
                    {
                        strResponceValue = string.Empty;
                        if (caseEligquestion.SBGTRespType.ToString() == "N")
                        {
                            if (caseEligquestion.SBGTNumEqual != string.Empty)
                            {
                                if (Convert.ToDecimal(caseEligquestion.SBGTNumEqual) > 0)
                                    strResponceValue = "= " + caseEligquestion.SBGTNumEqual + ",";
                            }
                            if (caseEligquestion.SBGTNumLThan != string.Empty)
                            {
                                if (Convert.ToDecimal(caseEligquestion.SBGTNumLThan) > 0)
                                    strResponceValue = strResponceValue + "< " + caseEligquestion.SBGTNumLThan + ",";
                            }
                            if (caseEligquestion.SBGTNumGThan != string.Empty)
                            {
                                if (Convert.ToDecimal(caseEligquestion.SBGTNumGThan) > 0)
                                    strResponceValue = strResponceValue + "> " + caseEligquestion.SBGTNumGThan + ",";
                            }
                            if (strResponceValue != string.Empty)
                                strResponceValue = strResponceValue.Remove(strResponceValue.Length - 1);
                        }
                        else
                        {
                            if (caseEligquestion.SBGTQuesCode == "I00025" || caseEligquestion.SBGTQuesCode == "I00026" || caseEligquestion.SBGTQuesCode == "I00036")
                            {
                                if (caseEligquestion.SBGTRESP.Trim() == "Y")
                                {
                                    strResponceValue = "Yes";
                                }
                                else if (caseEligquestion.SBGTRESP.Trim() == "N")
                                {
                                    strResponceValue = "No";
                                }
                                else if (caseEligquestion.SBGTRESP.Trim() == "U")
                                {
                                    strResponceValue = "Unavailable";
                                }
                            }
                            else
                            {
                                if (caseEligquestion.EligAgyCode != string.Empty)
                                    if (caseEligquestion.EligAgyCode.StartsWith("C"))
                                        strResponceValue = CaseSumData.GetCustRespCode(caseEligquestion.EligAgyCode, caseEligquestion.SBGTRESP.Trim());
                                    else
                                        strResponceValue = LookupDataAccess.GetLookUpCode(caseEligquestion.EligAgyCode, caseEligquestion.SBGTRESP.Trim());
                            }

                        }
                        strConjunction = string.Empty;
                        if (caseEligquestion.SBGTConj.Trim() != string.Empty)
                            strConjunction = caseEligquestion.SBGTConj == "A" ? "And" : "Or";
                        int rowIndex = gvwQuestions.Rows.Add(caseEligquestion.EligQuestionDesc, strResponceValue, strConjunction, caseEligquestion.SBGTGroup, caseEligquestion.SBGTQuesCode, caseEligquestion.SBGTGroupSeq);

                        CommonFunctions.setTooltip(rowIndex, caseEligquestion.AddOperator, caseEligquestion.DateAdd, caseEligquestion.LstcOperator, caseEligquestion.DateLstc, gvwQuestions);
                    }

                }

            //}


            if (Privileges.ChangePriv.Equals("false"))
            {
                gvwQuestions.Columns["Edit"].Visible = false;

            }
            else
            {
                gvwQuestions.Columns["Edit"].Visible = true;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvwQuestions.Columns["Delete"].Visible = false;

            }
            else
            {
                gvwQuestions.Columns["Delete"].Visible = true;
            }

            //      gvwQuestions.Sort(gvwQuestions.Columns["QSeq"], ListSortDirection.Ascending);
            if (gvwQuestions.Rows.Count > 0)
                gvwQuestions.Rows[0].Selected = true;
        }

        private void picAddScale_Click(object sender, EventArgs e)
        {

        }

        private bool ValidateForm()
        {
            bool isValid = true;
            if (strType == "Groups")
            {
                if (string.IsNullOrEmpty(txtGroupDesc.Text.Trim()))
                {
                    _errorProvider.SetError(txtGroupDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGroupDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtGroupDesc, null);
                }
            }
            if (strType == "Questions")
            {
                if (((ListItem)cmbQuestions.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(cmbQuestions, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQuestion.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbQuestions, null);
                }
                if (cmbMemAccess.Items.Count > 0)
                {
                    if (((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "0")
                    {
                        _errorProvider.SetError(cmbMemAccess, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMemAccess.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbMemAccess, null);
                    }
                }
                if (lblResponse.Visible == true)
                {
                    if (((ListItem)cmbResponce.SelectedItem).Value.ToString() == "0")
                    {
                        _errorProvider.SetError(cmbResponce, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResponse.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbResponce, null);
                    }
                }
                if (labelNResp.Visible == true)
                {
                    if (String.IsNullOrEmpty(txtLessthan.Text) && String.IsNullOrEmpty(txtGreaterthan.Text) && String.IsNullOrEmpty(txtEqualTo.Text))
                    {
                        _errorProvider.SetError(labelNResp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), labelNResp.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        string strLessthan = txtLessthan.Text == string.Empty ? "0" : txtLessthan.Text;
                        string strGreaterthan = txtGreaterthan.Text == string.Empty ? "0" : txtGreaterthan.Text;
                        string strEqual = txtEqualTo.Text == string.Empty ? "0" : txtEqualTo.Text;
                        if ((Convert.ToDouble(strLessthan) < 1) && (Convert.ToDouble(strGreaterthan) < 1) && (Convert.ToDouble(strEqual) < 1))
                        {
                            _errorProvider.SetError(labelNResp, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(labelNResp, null);

                        }
                    }

                }
            }
            return (isValid);
        }


        private void btnQSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SSBGTYPESEntity Entity = new SSBGTYPESEntity();
                string strMsg = string.Empty;

                Entity.SBGTID = ID;
                Entity.SBGTCode = ((ListItem)cmbType.SelectedItem).Value.ToString();
                Entity.SBGTCodeSeq = ((ListItem)cmbQues_Group.SelectedItem).ID.ToString();
                Entity.SBGTGroup = ((ListItem)cmbQues_Group.SelectedItem).Value.ToString();
                if (Mode == "Add") { Entity.SBGTGroupSeq = "1"; }
                else if (Mode == "Edit")
                {
                    //Entity.SSBGYear = ((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.Trim();
                    Entity.SBGTGroupSeq = gvwQuestions.SelectedRows[0].Cells["QSeq"].Value.ToString();
                }
                //Entity.SBGTDesc = txtGroupDesc.Text.Trim();
                //Entity.SBGTCntlOblig = txtCntlOblig.Text.Trim();

                Entity.SBGTMemAccess = ((ListItem)cmbMemAccess.SelectedItem).Value.ToString();
                Entity.SBGTQuesCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                if (((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() != "0")
                    Entity.SBGTConj = ((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() != null ? ((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() : string.Empty;
                if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "N")
                {
                    Entity.SBGTRESP = string.Empty;
                }
                else if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "D" || ((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "L")
                    Entity.SBGTRESP = ((ListItem)cmbResponce.SelectedItem).Value.ToString() != null ? ((ListItem)cmbResponce.SelectedItem).Value.ToString() : string.Empty;

                Entity.SBGTNumEqual = txtEqualTo.Text;
                Entity.SBGTNumGThan = txtGreaterthan.Text;
                Entity.SBGTNumLThan = txtLessthan.Text;
                Entity.SBGTRespType = ((ListItem)cmbQuestions.SelectedItem).ID.ToString();
                Entity.SBGT_Name = txtName.Text.Trim();
                //Entity.SSBGRPDate = dtpRepDate.Value.ToShortDateString();
                //Entity.SSBGBudget = txtBudget.Text.Trim();
                //Entity.SSBGAward = txtAward.Text.Trim();
                //Entity.SSBGDesc = txtGroupDesc.Text.Trim();
                Entity.AddOperator = BaseForm.UserID;
                Entity.LstcOperator = BaseForm.UserID;
                Entity.Mode = Mode;
                Entity.Type = "Question";

                if (_model.CaseSumData.InsertUpdateDelSSBGTypes(Entity, out strMsg))
                {
                    //btnTCancel.Visible = false; btnTSave.Visible = false;
                    //pnlCntltypes.Enabled = false;
                    //SSBGParams_Control SSBGControl = BaseForm.GetBaseUserControl() as SSBGParams_Control;
                    //if (SSBGControl != null)
                    //{
                    //    SSBGControl.RefreshQuestions();
                    //TypeCode = ((ListItem)cmbType.SelectedItem).ToString(); 
                    //    strSeqCode = strSeqCode;
                    //    strQuestionCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                    //    this.DialogResult = DialogResult.OK;
                    //}
                    gvwQuestions.Enabled=true;picAddQues.Enabled=true;
                    fillGridQuestions();
                }
                if (Mode == "Add") AlertBox.Show("Saved Successfully");
                else AlertBox.Show("Updated Successfully");
            }
        }

        private void btnQCancel_Click(object sender, EventArgs e)
        {
            //this.Close();
            if (gvwQuestions.Rows.Count > 0)
            {
                clearcontrols();
                gvwQuestions.Enabled=true;picAddQues.Enabled=true;
                fillControls();
            }
        }

        private void fillMemberAccess(string strHouseType)
        {
            cmbMemAccess.Items.Clear();
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetMemAccess(strHouseType);
            foreach (CommonEntity memAccess in commonEntity)
            {
                cmbMemAccess.Items.Add(new ListItem(memAccess.Desc, memAccess.Code));
            }
            cmbMemAccess.Items.Insert(0, new ListItem("    ", "0"));
            cmbMemAccess.SelectedIndex = 0;
            if (strHouseType.Equals("CASEMST") || strHouseType.Equals("H"))
            {
                CommonFunctions.SetComboBoxValue(cmbMemAccess, "H");
                cmbMemAccess.Enabled = false;
            }
            else if (strHouseType == "A")
            {
                CommonFunctions.SetComboBoxValue(cmbMemAccess, "A");
                cmbMemAccess.Enabled = false;
            }
            else
            {
                cmbMemAccess.Enabled = true;
                CommonFunctions.SetComboBoxValue(cmbMemAccess, "*");
            }
        }


        private void cmbQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbQuestions.SelectedIndex != 0)
            {
                _errorProvider.SetError(labelNResp, null);
                fillMemberAccess(((ListItem)cmbQuestions.SelectedItem).ScreenCode.ToString().Trim().ToUpper());
                if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "D" || ((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "L")
                {
                    txtEqualTo.Visible = false;
                    lblEqualTo.Visible = false;
                    lblGreaterthan.Visible = false;
                    lblLessthan.Visible = false;
                    labelNResp.Visible = false;
                    txtGreaterthan.Visible = false;
                    txtLessthan.Visible = false;
                    label9.Visible = false;
                    label7.Visible = true;
                    lblResponse.Visible = true;
                    cmbResponce.Visible = true;

                    lblName.Location = new Point(15,77);txtName.Location = new Point(105, 73);

                    cmbQuestionConjuction.SelectedIndex = 0;
                    if (((ListItem)cmbQuestions.SelectedItem).ScreenType.ToString() == Consts.Common.Standard)
                    {
                        cmbResponce.Items.Clear();
                        cmbResponce.Items.Insert(0, new ListItem("    ", "0"));
                        cmbResponce.SelectedIndex = 0;
                        //string strValue = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                        //if (strValue == "I00025" || strValue == "I00026" || strValue == "I00036")
                        //{
                        //    cmbResponce.Items.Clear();
                        //    List<ListItem> listItem = new List<ListItem>();
                        //    listItem.Add(new ListItem("None", "0"));
                        //    listItem.Add(new ListItem("Yes", "Y"));
                        //    listItem.Add(new ListItem("No", "N"));
                        //    listItem.Add(new ListItem("Unavailable", "U"));
                        //    cmbResponce.Items.AddRange(listItem.ToArray());
                        //    cmbResponce.SelectedIndex = 0;

                        //}                        
                        if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "L")
                        {
                            AGYTABSEntity Search_Agytabs = new AGYTABSEntity(true);
                            Search_Agytabs.Tabs_Type = ((ListItem)cmbQuestions.SelectedItem).ValueDisplayCode.ToString();
                            List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(Search_Agytabs);            // Run at Server
                            foreach (AGYTABSEntity standQuestion in AgyTabs_List)
                            {
                                ListItem li = new ListItem(standQuestion.Code_Desc, standQuestion.Table_Code);
                                cmbResponce.Items.Add(li);
                                // if (Mode.Equals(Consts.Common.Add) && standQuestion.Default.Equals("Y")) cmbResponce.SelectedItem = li;
                            }

                        }
                        else
                        {
                            List<CommonEntity> Responce = _model.lookupDataAccess.GetLookkupFronAGYTAB(((ListItem)cmbQuestions.SelectedItem).ValueDisplayCode.ToString());
                            foreach (CommonEntity standQuestion in Responce)
                            {
                                ListItem li = new ListItem(standQuestion.Desc, standQuestion.Code);
                                cmbResponce.Items.Add(li);
                                if (Mode.Equals(Consts.Common.Add) && standQuestion.Default.Equals("Y")) cmbResponce.SelectedItem = li;
                            }
                        }

                    }
                    else
                    {
                        List<CustRespEntity> custRepEntity = _model.FieldControls.GetCustRespByCustCode(((ListItem)cmbQuestions.SelectedItem).ValueDisplayCode.ToString());
                        foreach (CustRespEntity customques in custRepEntity)
                        {
                            cmbResponce.Items.Add(new ListItem(customques.RespDesc, customques.DescCode));
                        }
                        cmbResponce.Items.Insert(0, new ListItem("    ", "0"));
                    }

                }
                else if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "N")
                {
                    label7.Visible = false;
                    cmbResponce.Visible = false;
                    lblResponse.Visible = false;
                    label9.Visible = true;
                    labelNResp.Visible = true;
                    txtEqualTo.Visible = true;
                    txtGreaterthan.Visible = true;
                    txtLessthan.Visible = true;
                    txtEqualTo.Text = string.Empty;
                    txtGreaterthan.Text = string.Empty;
                    txtLessthan.Text = string.Empty;
                    lblEqualTo.Visible = true;
                    lblGreaterthan.Visible = true;
                    lblLessthan.Visible = true;
                    lblName.Location = new Point(15, 108); txtName.Location = new Point(105, 104);
                    cmbQuestionConjuction.SelectedIndex = 0;
                }
            }
        }

        private void cmbMemAccess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtEqualTo_Leave(object sender, EventArgs e)
        {
            if (txtEqualTo.Text.Length > 7)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEqualTo.Text, Consts.StaticVars.TwoDecimalRange7String))
                {

                    AlertBox.Show(Consts.Messages.PleaseEnterDecimalsRange, MessageBoxIcon.Warning);
                    txtEqualTo.Text = string.Empty;
                }
            }
        }

        private void txtGreaterthan_Leave(object sender, EventArgs e)
        {
            if (txtGreaterthan.Text.Length > 7)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtGreaterthan.Text, Consts.StaticVars.TwoDecimalRange7String))
                {

                    AlertBox.Show(Consts.Messages.PleaseEnterDecimalsRange, MessageBoxIcon.Warning);
                    txtGreaterthan.Text = string.Empty;
                }
            }
        }

        private void txtLessthan_Leave(object sender, EventArgs e)
        {
            if (txtLessthan.Text.Length > 7)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtLessthan.Text, Consts.StaticVars.TwoDecimalRange7String))
                {

                    AlertBox.Show(Consts.Messages.PleaseEnterDecimalsRange, MessageBoxIcon.Warning);
                    txtLessthan.Text = string.Empty;
                }
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillComboGroups();
        }

        private void gvwQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex != -1)
            {
                pnlQuestions.Enabled = true;
                btnQSave.Visible = btnQCancel.Visible = true;
                gvwQuestions.Enabled = false; picAddQues.Enabled = false;
                Mode = "Edit";
            }
            else if (e.ColumnIndex == 7 && e.RowIndex != -1)
            {
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandlerQuestions);
            }
        }

        private void MessageBoxHandlerQuestions(DialogResult dialogResult)
        {
            // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
           // Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;
            string strMsg = string.Empty;
            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    //GetSelectedRow();
                    SSBGTYPESEntity EligQEntity = new SSBGTYPESEntity();
                    EligQEntity.SBGTID = ID;
                    EligQEntity.SBGTCode = ((ListItem)cmbType.SelectedItem).Value.ToString();
                    EligQEntity.SBGTCodeSeq = ((ListItem)cmbQues_Group.SelectedItem).ID.ToString();
                    EligQEntity.SBGTGroup = ((ListItem)cmbQues_Group.SelectedItem).Value.ToString();
                    EligQEntity.SBGTQuesCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                    EligQEntity.SBGTGroupSeq = gvwQuestions.SelectedRows[0].Cells["QSeq"].Value.ToString();
                    EligQEntity.Mode = "Delete";
                    EligQEntity.Type = "Question";
                    if (_model.CaseSumData.InsertUpdateDelSSBGTypes(EligQEntity, out strMsg))
                    {
                        if (strMsg == "Success")
                        {
                            //if (strIndex != 0)
                            //    strIndex = strIndex - 1;
                            //RefreshQuestions();
                            fillGridQuestions();
                        }
                        else
                            AlertBox.Show("You can’t delete this Question, Exception error ", MessageBoxIcon.Warning);

                    }
                    else
                    {
                        AlertBox.Show("You can’t delete this Question, already used in Service Integration Matrix", MessageBoxIcon.Warning);
                    }

                }
            //}
        }

        private void gvwQuestions_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwQuestions.Rows.Count > 0)
            {
                clearcontrols();
                fillControls();
            }
        }

        private void fillControls()
        {
            foreach (DataGridViewRow dr in gvwQuestions.Rows)
            {
                if (dr.Selected)
                {
                    List<SSBGTYPESEntity> SSBGTypesall = _model.CaseSumData.GetSSBGTypes();
                    List<SSBGTYPESEntity> caseEligGroups = SSBGTypesall.FindAll(u => u.SBGTID.Equals(ID) && u.SBGTCode.Equals(((ListItem)cmbType.SelectedItem).Value.ToString()) && u.SBGTCodeSeq.Equals(((ListItem)cmbQues_Group.SelectedItem).ID.ToString()) && u.SBGTGroup.Equals(((ListItem)cmbQues_Group.SelectedItem).Value.ToString()) && u.SBGTQuesCode.Equals(gvwQuestions.SelectedRows[0].Cells["QuestionId"].Value.ToString()) && u.SBGTGroupSeq.Equals(gvwQuestions.SelectedRows[0].Cells["QSeq"].Value.ToString()));
                    if (caseEligGroups.Count > 0)
                    {
                        
                        foreach (var EligQDetails in caseEligGroups)
                        {
                            CommonFunctions.SetComboBoxValue(cmbQuestions, EligQDetails.SBGTQuesCode);
                            CommonFunctions.SetComboBoxValue(cmbMemAccess, EligQDetails.SBGTMemAccess);
                            CommonFunctions.SetComboBoxValue(cmbResponce, EligQDetails.SBGTRESP);
                            if (EligQDetails.SBGTConj != string.Empty)
                                CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, EligQDetails.SBGTConj);
                            else
                                CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, "0");
                            txtEqualTo.Text = EligQDetails.SBGTNumEqual;
                            txtGreaterthan.Text = EligQDetails.SBGTNumGThan;
                            txtLessthan.Text = EligQDetails.SBGTNumLThan;
                            txtName.Text = EligQDetails.SBGT_Name.Trim();
                        }
                    }
                }
            }
        }

        private void picAddQues_Click(object sender, EventArgs e)
        {
            pnlQuestions.Enabled = true;
            btnQSave.Visible = btnQCancel.Visible = true;
            gvwQuestions.Enabled=false;picAddQues.Enabled=false;
            txtEqualTo.Text = string.Empty;
            txtGreaterthan.Text = string.Empty;
            txtLessthan.Text = string.Empty;
            txtName.Text = string.Empty;
            CommonFunctions.SetComboBoxValue(cmbQuestions, "0");
            CommonFunctions.SetComboBoxValue(cmbMemAccess, "0");
            CommonFunctions.SetComboBoxValue(cmbResponce, "0");
            CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, "0");
            Mode = "Add";
        }

        private void clearcontrols()
        {
            pnlQuestions.Enabled = false;
            btnQSave.Visible = btnQCancel.Visible = false;
            txtEqualTo.Text = string.Empty;
            txtGreaterthan.Text = string.Empty;
            txtLessthan.Text = string.Empty;
            txtName.Text = string.Empty;
            CommonFunctions.SetComboBoxValue(cmbQuestions, "0");
            CommonFunctions.SetComboBoxValue(cmbMemAccess, "0");
            CommonFunctions.SetComboBoxValue(cmbResponce, "0");
            CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, "0");
            
        }

        private void ClearGroupControls()
        {
            txtGrpCode.Enabled = false;
            picGroupAdd.Enabled = true;
            txtGrpCode.Text = txtGroupDesc.Text = txtSeq.Text = string.Empty;
            cmbConjunction.SelectedIndex = 0;
            txtSeq.Enabled = false;
            cmbConjunction.Enabled = false;
            txtGroupDesc.Enabled = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
            gvMat_Groups.Enabled = true;
        }

        private void picGroupAdd_Click(object sender, EventArgs e)
        {
            Mode = "Add";
            txtGrpCode.Enabled = false;
            string strCode = CaseSum.GETSSBGGroupCode(ID,((ListItem)cmbGrpType.SelectedItem).Value.ToString());
            txtGrpCode.Text = strCode;
            txtGroupDesc.Text = txtSeq.Text = string.Empty;
            cmbConjunction.SelectedIndex = 0;
            txtSeq.Enabled = true;
            cmbConjunction.Enabled = true;
            txtGroupDesc.Enabled = true;
            btnCancel.Visible = true;
            btnSave.Visible = true;
            gvMat_Groups.Enabled = false;
            picGroupAdd.Enabled = false;
            txtGroupDesc.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //_errorProvider.SetError(txtSeq, null);
            _errorProvider.SetError(txtGroupDesc, null);
            txtGrpCode.Enabled = false;
            txtGrpCode.Text = txtGroupDesc.Text = txtSeq.Text = string.Empty;
            cmbConjunction.SelectedIndex = 0;
            txtSeq.Enabled = false;
            cmbConjunction.Enabled = false;
            txtGroupDesc.Enabled = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
            picGroupAdd.Enabled = true;
            gvMat_Groups.Enabled = true;
            Group_Controls_Fill();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SSBGTYPESEntity Entity = new SSBGTYPESEntity();
                string strMsg = string.Empty;

                Entity.SBGTID = ID;
                Entity.SBGTCode = ((ListItem)cmbGrpType.SelectedItem).Value.ToString();
                
                if (Mode == "Add") { Entity.SBGTCodeSeq = "1"; }
                else if (Mode == "Edit")
                {
                    //Entity.SSBGYear = ((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.Trim();
                    Entity.SBGTCodeSeq = gvMat_Groups.SelectedRows[0].Cells["GSeq"].Value.ToString();
                }
                Entity.SBGTGroup=txtGrpCode.Text;
                Entity.SBGTGroupSeq = "0";

                Entity.SBGTDesc = txtGroupDesc.Text.Trim();
                //Entity.SBGTCntlOblig = txtCntlOblig.Text.Trim();

                //Entity.SBGTMemAccess = ((ListItem)cmbMemAccess.SelectedItem).Value.ToString();
                //Entity.SBGTQuesCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                if (((ListItem)cmbConjunction.SelectedItem).Value.ToString() != "0")
                    Entity.SBGTConj = ((ListItem)cmbConjunction.SelectedItem).Value.ToString() != null ? ((ListItem)cmbConjunction.SelectedItem).Value.ToString() : string.Empty;
                //if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "N")
                //{
                //    Entity.SBGTRESP = string.Empty;
                //}
                //else if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "D" || ((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "L")
                //    Entity.SBGTRESP = ((ListItem)cmbResponce.SelectedItem).Value.ToString() != null ? ((ListItem)cmbResponce.SelectedItem).Value.ToString() : string.Empty;

                //Entity.SBGTNumEqual = txtEqualTo.Text;
                //Entity.SBGTNumGThan = txtGreaterthan.Text;
                //Entity.SBGTNumLThan = txtLessthan.Text;
                //Entity.SBGTRespType = ((ListItem)cmbQuestions.SelectedItem).ID.ToString();
                ////Entity.SSBGRPDate = dtpRepDate.Value.ToShortDateString();
                //Entity.SSBGBudget = txtBudget.Text.Trim();
                //Entity.SSBGAward = txtAward.Text.Trim();
                //Entity.SSBGDesc = txtGroupDesc.Text.Trim();
                Entity.AddOperator = BaseForm.UserID;
                Entity.LstcOperator = BaseForm.UserID;
                Entity.Mode = Mode;
                Entity.Type = "Groups";

                if (_model.CaseSumData.InsertUpdateDelSSBGTypes(Entity, out strMsg))
                {
                    //btnTCancel.Visible = false; btnTSave.Visible = false;
                    //pnlCntltypes.Enabled = false;
                    //SSBGParams_Control SSBGControl = BaseForm.GetBaseUserControl() as SSBGParams_Control;
                    //if (SSBGControl != null)
                    //{
                    //    SSBGControl.RefreshQuestions();
                    GroupCode = txtGrpCode.Text;
                    //    strSeqCode = strSeqCode;
                    //    strQuestionCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                    //    this.DialogResult = DialogResult.OK;
                    //}
                    fillGroupsgrid();
                    
                    //Group_Controls_Fill();
                }

                if (Mode == "Add") AlertBox.Show("Saved Successfully");
                else AlertBox.Show("Updated Successfully");
            }
        }

        private void txtGrp_Desc_LostFocus(object sender, EventArgs e)
        {

        }

        private void txtSeq_LostFocus(object sender, EventArgs e)
        {

        }

        private void txtGrpCode_LostFocus(object sender, EventArgs e)
        {

        }

        private void gvMat_Groups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex != -1)
            {
                txtGrpCode.Enabled = false;
                //txtSeq.Enabled = true;
                cmbConjunction.Enabled = true;
                txtGroupDesc.Enabled = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                gvMat_Groups.Enabled = false;
                picGroupAdd.Enabled = false;
                txtSeq.Focus();
                Mode = "Edit";


            }
            else if (e.ColumnIndex == 5 && e.RowIndex != -1)
            {
                strIndex = gvMat_Groups.SelectedRows[0].Index;
                //**strPageIndex = gvMat_Groups.CurrentPage;
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: Delete_Groups);
            }
        }

        private void Delete_Groups(DialogResult dialogResult)
        {
            //Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;
            string strMsg = string.Empty;
            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    //GetSelectedRow();
                    SSBGTYPESEntity EligQEntity = new SSBGTYPESEntity();
                    EligQEntity.SBGTID = ID;
                    EligQEntity.SBGTCode = ((ListItem)cmbGrpType.SelectedItem).Value.ToString();
                    //EligQEntity.SBGTQuesCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                    EligQEntity.SBGTCodeSeq = gvMat_Groups.SelectedRows[0].Cells["GSeq"].Value.ToString();
                    EligQEntity.SBGTGroup = gvMat_Groups.SelectedRows[0].Cells["GrpCd"].Value.ToString();
                    EligQEntity.SBGTGroupSeq = "0";
                    EligQEntity.Mode = "Delete";
                    EligQEntity.Type = "Groups";
                    if (_model.CaseSumData.InsertUpdateDelSSBGTypes(EligQEntity, out strMsg))
                    {
                        if (strMsg == "Success")
                        {
                            //if (strIndex != 0)
                            //    strIndex = strIndex - 1;
                            //RefreshQuestions();
                            fillGroupsgrid();
                            //Group_Controls_Fill();
                        }
                        else
                            AlertBox.Show("You can’t delete this Group, Exception error", MessageBoxIcon.Warning);

                    }
                    else
                    {
                        AlertBox.Show("You can’t delete this Group, already used in Service Integration Matrix", MessageBoxIcon.Warning);
                    }

                }
            //}
        }

        private void gvMat_Groups_SelectionChanged(object sender, EventArgs e)
        {
            if (gvMat_Groups.Rows.Count > 0)
            {
                ClearGroupControls();
                Group_Controls_Fill();
            }
        }

        private void picAddQuestions_Click(object sender, EventArgs e)
        {

        }

        private void cmbQues_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillGridQuestions();
        }

        private void PicAddResponce_Click(object sender, EventArgs e)
        {

        }

        private void txtPoints_LostFocus(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void cmbGrpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillGroupsgrid();
            //Group_Controls_Fill();
        }

        private void cmbQues_Group_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            fillGridQuestions();
        }

        private void Group_Controls_Fill()
        {
            if (gvMat_Groups.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvMat_Groups.Rows)
                {
                    if (dr.Selected)
                    {
                        List<SSBGTYPESEntity> SSBGTypesGroups = _model.CaseSumData.GetSSBGTypesadpgs(ID, ((ListItem)cmbGrpType.SelectedItem).Value.ToString(), gvMat_Groups.SelectedRows[0].Cells["GSeq"].Value.ToString(), gvMat_Groups.SelectedRows[0].Cells["GrpCd"].Value.ToString(), string.Empty, "Group");
                        //SSBGTYPESEntity row = gvMat_Groups.SelectedRows[0].Tag as SSBGTYPESEntity;
                        if (SSBGTypesGroups.Count>0)
                        {
                            foreach (var row in SSBGTypesGroups)
                            {
                                txtGroupDesc.Enabled = false;
                                //txtSeq.Enabled = false;
                                cmbConjunction.Enabled = false;
                                txtGrpCode.Text = gvMat_Groups.SelectedRows[0].Cells["GrpCd"].Value.ToString();
                                txtGroupDesc.Text = row.SBGTDesc;
                                //txtSeq.Text = row.Seq;
                                CommonFunctions.SetComboBoxValue(cmbConjunction, row.SBGTConj);
                            }
                        }
                        else
                        {
                            txtGrpCode.Enabled = false;
                            txtGrpCode.Text = txtGroupDesc.Text = txtSeq.Text = string.Empty;
                            cmbConjunction.SelectedIndex = 0;
                            txtSeq.Enabled = false;
                            cmbConjunction.Enabled = false;
                            txtGroupDesc.Enabled = false;
                            btnCancel.Visible = false;
                            btnSave.Visible = false;
                            gvMat_Groups.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                txtGrpCode.Enabled = false;
                txtGrpCode.Text = txtGroupDesc.Text = txtSeq.Text = string.Empty;
                cmbConjunction.SelectedIndex = 0;
                txtSeq.Enabled = false;
                cmbConjunction.Enabled = false;
                txtGroupDesc.Enabled = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                gvMat_Groups.Enabled = true;
            }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlGQ.SelectedIndex == 0)
            {
                strType = "Groups";
                this.cmbGrpType.SelectedIndexChanged -= new System.EventHandler(this.cmbGrpType_SelectedIndexChanged);
                FillComboGroupType();
                this.cmbGrpType.SelectedIndexChanged += new System.EventHandler(this.cmbGrpType_SelectedIndexChanged);
                fillGroupsgrid();
                    //Group_Controls_Fill();
                    Mode = string.Empty;

            }
            else if (tabControlGQ.SelectedIndex == 1)
            {
                strType = "Questions";
                pnlQuestions.Visible = true;
                picAddQues.Enabled = true;
                this.cmbType.SelectedIndexChanged -= new System.EventHandler(this.cmbType_SelectedIndexChanged);
                fillcombo();
                this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
                FillComboType();
                fillComboGroups();
                Mode = string.Empty;
                //txtQues_Code.Enabled = false;
                //Questions_Controls_Fill();
            }
        }


    }
}