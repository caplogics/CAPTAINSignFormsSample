#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;
using System.Drawing.Text;
using DevExpress.CodeParser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Case0009Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public Case0009Form(BaseForm baseForm, string mode, string strType, string strGroupId, string strQuestionId, string strSeq, string strHierchyCode, string strHierchydesc, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Mode = mode;
            strgroupcode = strGroupId;
            strQuestionCode = strQuestionId;
            strSeqCode = strSeq;
            strHierachycode = strHierchyCode;
            FormType = strType;
            //lblHeader.Text = privilegeEntity.PrivilegeName;
           // this.Text = "Group Definition" /*privilegeEntity*/ + " - " + Mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtEqualTo.Validator = TextBoxValidation.FloatValidator;
            txtLessthan.Validator = TextBoxValidation.FloatValidator;
            txtGreaterthan.Validator = TextBoxValidation.FloatValidator;
            txtPoints.Validator = TextBoxValidation.FloatValidator;
            fillShowQuesCombo();
            fillQuesGrid();
            fillcombo();
            if (FormType == Consts.LiheAllDetails.strMainType)
            {
                //lblHeader.Text = "Group Definition";
                Tools["TL_GroupHELP"].Visible = true;
                Tools["TL_QuesHELP"].Visible = false;

                this.Text = "Group Definition" /*privilegeEntity*/ + " - " + Mode;
                pnlGroup.Visible = true;
                pnlQuestions.Visible = false;
                btnSave.Visible = btnCancel.Visible = true;
                this.Size = new Size(578, 200);
                this.pnlCompleteForm.Size = new Size(578, 200);

                if (Mode == "Add")
                {
                    if (strHierchyCode != string.Empty)
                    {
                        txtHierarchy.Text = strHierchyCode;
                        txtHierachydesc.Text = strHierchydesc;
                        string strCode = CaseSum.GETCASEELIGGCode(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2));
                        txtGroupCode.Text = "0000".Substring(0, 4 - strCode.Length) + strCode;
                    }
                }
                else
                {
                    string strCode = strgroupcode;
                    txtGroupCode.Text = "0000".Substring(0, 4 - strCode.Length) + strCode;
                    List<CaseELIGEntity> caseEligGEntity = _model.CaseSumData.GetCASEELIGadpgsGroup(strHierachycode.Substring(0, 2), strHierachycode.Substring(3, 2), strHierachycode.Substring(6, 2), strgroupcode, strSeqCode, "Group");
                    foreach (var EligGDetails in caseEligGEntity)
                    {
                        txtGroupDesc.Text = EligGDetails.EligGroupDesc;
                        if(EligGDetails.EligConjunction!=string.Empty)
                        CommonFunctions.SetComboBoxValue(cmbConjunction, EligGDetails.EligConjunction);
                        else
                            CommonFunctions.SetComboBoxValue(cmbConjunction, "0");
                        txtPoints.Text = EligGDetails.EligPoints;
                    }
                    PbHierarchies.Visible = false;
                    txtHierarchy.Text = strHierchyCode;
                    txtHierachydesc.Text = strHierchydesc;

                }

            }
            else if (FormType == Consts.LiheAllDetails.strSubType)
            {
                Tools["TL_GroupHELP"].Visible = false;
                Tools["TL_QuesHELP"].Visible = true;

                // lblHeader.Text = "Question Definition";
                this.Text = "Question Definition" /*privilegeEntity*/ + " - " + Mode;
                pnlGroup.Visible = false;
                pnlQuestions.Visible = true;
                //pnlQuescmb.Visible = false;
                //pnlShowQues.Visible = true;
                //pnlQuesNType.Size = new Size(550,39);
                //lblQuesConjunction.Location = new Point(393, 47);
                //cmbQuestionConjuction.Location = new Point(471, 43);
                //this.Size = new Size(550, 163);

                if (Mode == "Add")
                {
                    //pnlQuescmb.Visible = false;
                    //pnlShowQues.Visible = true;
                    //lblResponse.Visible = lblcmbReqResponse.Visible = cmbResponce.Visible= true;
                    //pnlQuesNType.Size = new Size(760, 39);
                    //lblMemAccess.Location = new Point(175, 9); lblReqMemaccess.Location = new Point(245,7); cmbMemAccess.Location = new Point(260,5);
                    //lblQuesConjunction.Location = new Point(15,9);cmbQuestionConjuction.Location = new Point(100,5);
                    //lblResponse.Location = new Point(415,9);lblcmbReqResponse.Location = new Point(468,7); cmbResponce.Location = new Point(488,5);
                    //this.Size = new Size(760, 350);//(760, 385);
                    //cmbMemAccess.Enabled = false;cmbQuestionConjuction.Enabled = false;cmbResponce.Enabled = false; labelNResp.Visible = lblReqResponse.Visible = txtCity.Visible = false;
                }
                else
                {
                    btnSave.Visible = btnCancel.Visible = true; 
                    //cmbQuestions.Enabled = false;
                    btnAdd.Visible = false;
                    pnlQuescmb.Visible = true;
                    pnlShowQues.Visible = false;
                    pnlQuesNType.Size = new Size(550,33);
                    pnldgvQuestions.Visible = false;
                    lblQuesConjunction.Location = new Point(393, 76);
                    cmbQuestionConjuction.Location = new Point(471, 71);
                    this.Size = new Size(550, 163);

                    List<CaseELIGEntity> caseEligGEntity = _model.CaseSumData.GetCASEELIGadpgsGroup(strHierachycode.Substring(0, 2), strHierachycode.Substring(3, 2), strHierachycode.Substring(6, 2), strgroupcode, strSeqCode, "Question");
                    foreach (var EligQDetails in caseEligGEntity)
                    {
                        CommonFunctions.SetComboBoxValue(cmbQuestions, EligQDetails.EligQuesCode);
                        CommonFunctions.SetComboBoxValue(cmbMemAccess, EligQDetails.EligMemberAccess);
                        CommonFunctions.SetComboBoxValue(cmbResponce, EligQDetails.EligDDTextResp);
                        if(EligQDetails.EligConjunction!=string.Empty)
                        CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, EligQDetails.EligConjunction);
                        else
                            CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, "0");
                        txtEqualTo.Text = EligQDetails.EligNumEqual;
                        txtGreaterthan.Text = EligQDetails.EligNumGthan;
                        txtLessthan.Text = EligQDetails.EligNumLthan;

                        txtCity.Text = EligQDetails.EligDDTextResp.Trim();
                    }
                }
            }
        }

        public BaseForm BaseForm { get; set; }
        public string Mode { get; set; }
        public string strgroupcode { get; set; }
        public string strQuestionCode { get; set; }
        public string strSeqCode { get; set; }
        public string FormType { get; set; }
        public string strHierachycode { get; set; }
        public string HiearchyCode { get; set; }

        //bool Hie_Changed_FLG = false;



        private bool ValidateForm()
        {
            bool isValid = true;

            if (FormType == Consts.LiheAllDetails.strMainType)
            {
                if (String.IsNullOrEmpty(txtHierarchy.Text.Trim()))
                {
                    _errorProvider.SetError(PbHierarchies, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHierarchy.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtHierachydesc, null);
                }

                if (String.IsNullOrEmpty(txtGroupCode.Text))
                {
                    _errorProvider.SetError(txtGroupCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGroupCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtGroupCode, null);
                }

                if (String.IsNullOrEmpty(txtGroupDesc.Text.Trim()))
                {
                    _errorProvider.SetError(txtGroupDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGroupDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }

                else
                {
                    _errorProvider.SetError(txtGroupDesc, null);
                }

            }
            else
            {
                if (Mode == "Add")
                {
                    if (cmbShowques.SelectedIndex.Equals("0"))
                    {
                        _errorProvider.SetError(cmbShowques, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblShowQues.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbShowques, null);
                    }

                    if (dgvQuestions.Rows.Count.Equals("0"))
                    {
                        _errorProvider.SetError(dgvQuestions, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQuestion.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dgvQuestions, null);
                    }
                }
                else
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
                    if (Mode == "Add")
                    {
                        if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "N")
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
                        else
                        {
                            if (string.IsNullOrEmpty(txtCity.Text.Trim()))
                            {
                                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResponse.Text.Replace(Consts.Common.Colon, string.Empty)));
                                isValid = false;
                            }
                            else
                            {
                                _errorProvider.SetError(txtCity, null);
                            }
                        }
                    }
                    else 
                    {
                        if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "N")
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
                        else
                        {
                            if (string.IsNullOrEmpty(txtCity.Text.Trim()))
                            {
                                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResponse.Text.Replace(Consts.Common.Colon, string.Empty)));
                                isValid = false;
                            }
                            else
                            {
                                _errorProvider.SetError(txtCity, null);
                            }
                        }
                    }
                    //else
                    //{
                    //    if (string.IsNullOrEmpty(txtCity.Text.Trim()))
                    //    {
                    //        _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResponse.Text.Replace(Consts.Common.Colon, string.Empty)));
                    //        isValid = false;
                    //    }
                    //    else
                    //    {
                    //        _errorProvider.SetError(txtCity, null);
                    //    }
                    //}

                }

            }
            return (isValid);
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                CaseELIGEntity caseEliggentity = new CaseELIGEntity();
                string strMsg = string.Empty;
                if (FormType == Consts.LiheAllDetails.strMainType)
                {
                    string strHierarchy = txtHierarchy.Text;
                    caseEliggentity.EligGroupCode = txtGroupCode.Text;
                    caseEliggentity.EligGroupDesc = txtGroupDesc.Text;
                    caseEliggentity.EligPoints = txtPoints.Text;
                    if (((ListItem)cmbConjunction.SelectedItem).Value.ToString() != "0")
                        caseEliggentity.EligConjunction = ((ListItem)cmbConjunction.SelectedItem).Value.ToString();
                    caseEliggentity.EligAgency = strHierarchy.Substring(0, 2);
                    caseEliggentity.EligDept = strHierarchy.Substring(3, 2);
                    caseEliggentity.EligProgram = strHierarchy.Substring(6, 2);
                    caseEliggentity.EligGroupSeq = "0";
                    caseEliggentity.AddOperator = BaseForm.UserID;
                    caseEliggentity.LstcOperator = BaseForm.UserID;
                    caseEliggentity.Mode = Mode;
                    caseEliggentity.Type = "Group";
                    if (_model.CaseSumData.InsertUpdateDelCaseElig(caseEliggentity, out strMsg))
                    {
                        Case0009Control Case0009Control = BaseForm.GetBaseUserControl() as Case0009Control;
                        if (Mode == "Add")
                        {
                            AlertBox.Show("Saved Successfully");
                        }
                        else
                        {
                            AlertBox.Show("Updated Successfully");
                        }
                        if (Case0009Control != null)
                        {
                            Case0009Control.RefreshGrid();
                            strgroupcode = txtGroupCode.Text;
                            this.DialogResult = DialogResult.OK;
                        }
                        this.Close();
                    }
                }
                else if (FormType == Consts.LiheAllDetails.strSubType)
                {
                    if(Mode == "Add")
                    {
                        if (((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() != "0")
                            caseEliggentity.EligConjunction = ((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() != null ? ((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() : string.Empty;
                        if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString()=="N")
                            caseEliggentity.EligDDTextResp = string.Empty;
                        else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "T")
                        {
                            caseEliggentity.EligDDTextResp = txtCity.Text.Trim();
                        }
                        else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "D" || dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "L")
                            caseEliggentity.EligDDTextResp = ((ListItem)cmbResponce.SelectedItem).Value.ToString() != null ? ((ListItem)cmbResponce.SelectedItem).Value.ToString() : string.Empty;
                    }
                    else
                    {
                        if (((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() != "0")
                            caseEliggentity.EligConjunction = ((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() != null ? ((ListItem)cmbQuestionConjuction.SelectedItem).Value.ToString() : string.Empty;
                        if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "N")
                        {
                            caseEliggentity.EligDDTextResp = string.Empty;
                        }
                        else if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "T")
                        {
                            caseEliggentity.EligDDTextResp = txtCity.Text.Trim();
                        }
                        else if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "D" || ((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "L")
                            caseEliggentity.EligDDTextResp = ((ListItem)cmbResponce.SelectedItem).Value.ToString() != null ? ((ListItem)cmbResponce.SelectedItem).Value.ToString() : string.Empty;
                    }
                    

                    caseEliggentity.EligAgency = strHierachycode.Substring(0, 2);
                    caseEliggentity.EligDept = strHierachycode.Substring(3, 2);
                    caseEliggentity.EligProgram = strHierachycode.Substring(6, 2);
                    caseEliggentity.EligGroupCode = strgroupcode;
                    caseEliggentity.EligMemberAccess = ((ListItem)cmbMemAccess.SelectedItem).Value.ToString();
                    if(Mode == "Add")
                        caseEliggentity.EligQuesCode = dgvQuestions.CurrentRow.Cells["dgvQuesCode"].Value.ToString();
                    else
                        caseEliggentity.EligQuesCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                    //if (((ListItem)cmbQuestions.SelectedItem).ScreenType.ToString() == Consts.Common.Standard)
                    //{
                    //    caseEliggentity.EligQuesScreen = "";
                    //    caseEliggentity.EligQuesType = "I";
                    //}
                    //else
                    //{
                    //    caseEliggentity.EligQuesScreen = ((ListItem)cmbQuestions.SelectedItem).ScreenCode.ToString(); 
                    //    caseEliggentity.EligQuesType = "C";
                    //}
                    caseEliggentity.EligNumEqual = txtEqualTo.Text;
                    caseEliggentity.EligNumGthan = txtGreaterthan.Text;
                    caseEliggentity.EligNumLthan = txtLessthan.Text;
                    if(Mode == "Add")
                        caseEliggentity.EligResponseType = dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString();
                    else
                        caseEliggentity.EligResponseType = ((ListItem)cmbQuestions.SelectedItem).ID.ToString();
                    caseEliggentity.EligGroupSeq = strSeqCode;
                    caseEliggentity.AddOperator = BaseForm.UserID;
                    caseEliggentity.LstcOperator = BaseForm.UserID;
                    caseEliggentity.Mode = Mode;
                    caseEliggentity.Type = "Question";
                    if (_model.CaseSumData.InsertUpdateDelCaseElig(caseEliggentity, out strMsg))
                    {
                        Case0009Control Case0009Control = BaseForm.GetBaseUserControl() as Case0009Control;
                        if (Case0009Control != null)
                        {
                            if (Mode == "Add")
                            {
                                strSeqCode = strMsg;
                                AlertBox.Show("Saved Successfully");
                            }
                            else
                            {
                                AlertBox.Show("Updated Successfully");
                            }
                            Case0009Control.RefreshQuestions(strSeqCode);
                            if (Mode == "Add")
                                strQuestionCode = dgvQuestions.CurrentRow.Cells["dgvQuesCode"].Value.ToString();
                            else
                                strQuestionCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                            this.DialogResult = DialogResult.OK;
                        }
                        this.Close();
                    }
                }
            }
        }

        private void fillcombo()
        {
            if (FormType == Consts.LiheAllDetails.strSubType)
            {
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
                List<CaseELIGQUESEntity> EligPreassData = _model.CaseSumData.GetELIGCUSTOMQUES(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2), "PREASSES");
                foreach (CaseELIGQUESEntity EligQues in EligPreassData)
                {
                    ListItem li = new ListItem(EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFliedType, Consts.Common.Custom);
                    cmbQuestions.Items.Add(li);
                }

                cmbQuestions.SelectedIndex = 0;
            }


            List<CommonEntity> commonconjunction = _model.lookupDataAccess.GetConjunctions();
            foreach (CommonEntity conjunction in commonconjunction)
            {
                cmbConjunction.Items.Add(new ListItem(conjunction.Desc, conjunction.Code));
                cmbQuestionConjuction.Items.Add(new ListItem(conjunction.Desc, conjunction.Code));
            }
            cmbConjunction.Items.Insert(0, new ListItem("    ", "0"));
            cmbConjunction.SelectedIndex = 0;
            cmbQuestionConjuction.Items.Insert(0, new ListItem("    ", "0"));
            cmbQuestionConjuction.SelectedIndex = 0;

        }
        #region Vikash added
        private void fillShowQuesCombo()
        {
            _errorProvider.SetError(cmbShowques, null);
            cmbShowques.Items.Clear();
            cmbShowques.Items.Insert(0, new ListItem("    ", "0"));
            cmbShowques.Items.Add("Standard");
            cmbShowques.Items.Add("Custom");
            cmbShowques.Items.Add("Both");
            cmbShowques.SelectedIndex = 1;
        }

        int row_Index = 0;
        private void fillQuesGrid()
        {           
            if (FormType == Consts.LiheAllDetails.strSubType)
            {
                dgvQuestions.Rows.Clear();
                
                if (cmbShowques.SelectedIndex == 1)
                {
                    List<CaseELIGQUESEntity> EligData = _model.CaseSumData.GetELIGQUES();
                    foreach (CaseELIGQUESEntity EligQues in EligData)
                    {
                        row_Index = dgvQuestions.Rows.Add(Consts.Common.Standard, EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFileName);
                        row_Index++;
                    }
                }
                else if(cmbShowques.SelectedIndex == 2)
                {
                    List<CaseELIGQUESEntity> EligCustomData = _model.CaseSumData.GetELIGCUSTOMQUES(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2));
                    foreach (CaseELIGQUESEntity EligQues in EligCustomData)
                    {
                        row_Index = dgvQuestions.Rows.Add(Consts.Common.Custom, EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFliedType);
                        row_Index++;
                    }

                    List<CaseELIGQUESEntity> EligPreassData = _model.CaseSumData.GetELIGCUSTOMQUES(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2), "PREASSES");
                    foreach (CaseELIGQUESEntity EligQues in EligPreassData)
                    {
                        row_Index = dgvQuestions.Rows.Add(Consts.Common.Custom, EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFliedType);
                        row_Index++;
                    }
                }
                else if(cmbShowques.SelectedIndex == 3)
                {
                    List<CaseELIGQUESEntity> EligData = _model.CaseSumData.GetELIGQUES();
                    foreach (CaseELIGQUESEntity EligQues in EligData)
                    {
                        row_Index = dgvQuestions.Rows.Add(Consts.Common.Standard, EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFileName);
                        row_Index++;
                    }

                    List<CaseELIGQUESEntity> EligCustomData = _model.CaseSumData.GetELIGCUSTOMQUES(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2));
                    foreach (CaseELIGQUESEntity EligQues in EligCustomData)
                    {
                        row_Index = dgvQuestions.Rows.Add(Consts.Common.Custom, EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFliedType);
                        row_Index++;
                    }

                    List<CaseELIGQUESEntity> EligPreassData = _model.CaseSumData.GetELIGCUSTOMQUES(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2), "PREASSES");
                    foreach (CaseELIGQUESEntity EligQues in EligPreassData)
                    {
                        row_Index = dgvQuestions.Rows.Add(Consts.Common.Custom, EligQues.EligQuesDesc, EligQues.EligQuesCode, EligQues.EligRespType, EligQues.EligAgyCode, EligQues.EligFliedType);
                        row_Index++;
                    }
                }
                //if(dgvQuestions.Rows.Count>0)
                //    dgvQuestions.Rows[Selection_Index].Selected = true;
                //else
                //    dgvQuestions.Rows[0].Selected = true;
            }
        }

        private void cmbShowques_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillQuesGrid();

            if (Mode == "Add")
            {
                if (dgvQuestions.Rows.Count > 0)
                {
                    btnAdd.Visible = true;
                    pnlAccessConj.Visible = true;
                }
                else
                {
                    btnAdd.Visible = false;
                    pnlAccessConj.Visible = false;
                }
            }

           /* if (Mode == "Add")
            {
                _errorProvider.SetError(cmbResponce, null);
                _errorProvider.SetError(cmbMemAccess, null);
                _errorProvider.SetError(cmbShowques, null);
                pnlQuescmb.Visible = false;//btnAdd.Visible = false;
                pnlShowQues.Visible = true;
                labelNResp.Visible = lblReqResponse.Visible = txtCity.Visible = false;
                lblResponse.Visible = lblcmbReqResponse.Visible = cmbResponce.Visible = true;
                pnlQuesNType.Size = new Size(760, 39);
                lblMemAccess.Location = new Point(175, 9);
                lblReqMemaccess.Location = new Point(245, 7);
                cmbMemAccess.Location = new Point(260, 5);
                lblQuesConjunction.Location = new Point(15, 9);
                cmbQuestionConjuction.Location = new Point(100, 5);
                lblResponse.Location = new Point(415, 9);
                lblcmbReqResponse.Location = new Point(468, 7);
                cmbResponce.Location = new Point(488, 5);
                this.Size = new Size(760, 350);//(760, 385);
                cmbMemAccess.Enabled = false;
                cmbQuestionConjuction.Enabled = false;
                cmbResponce.Enabled = false;

                if (dgvQuestions.Rows.Count > 0)
                    btnAdd.Visible = true;
                else
                    btnAdd.Visible = false;
            }
            else
            {
                pnlQuesNType.Size = new Size(550, 33);
                pnldgvQuestions.Visible = false;
                lblQuesConjunction.Location = new Point(393, 41);
                cmbQuestionConjuction.Location = new Point(471, 37);
                this.Size = new Size(550, 195);
            }*/
        }
        private void dgvQuestions_SelectionChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(cmbResponce, null);
            _errorProvider.SetError(cmbMemAccess, null);
            _errorProvider.SetError(cmbShowques, null);
            _errorProvider.SetError(txtCity, null);
            btnAdd.Visible = true;btnSave.Visible = btnCancel.Visible = false;
            this.Size = new Size(760, 385/*385*/);
            lblMemAccess.Location = new Point(175, 9); lblReqMemaccess.Location = new Point(245, 7); cmbMemAccess.Location = new Point(260, 5);
            lblQuesConjunction.Location = new Point(15, 9); cmbQuestionConjuction.Location = new Point(100, 5);

            pnlQuescmb.Visible = false;
            pnlShowQues.Visible = true;
            pnlQuesNType.Size = new Size(760, 39);

            if (cmbShowques.SelectedIndex != 0)
            {
                if (dgvQuestions.Rows.Count > 0)
                {
                    _errorProvider.SetError(labelNResp, null);
                    _errorProvider.SetError(txtEqualTo, null);
                    _errorProvider.SetError(txtGreaterthan, null);
                    _errorProvider.SetError(txtLessthan, null);
                    fillMemberAccess(dgvQuestions.CurrentRow.Cells["dgvQuesFieldType"].Value.ToString());
                    if (((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "H" && ((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "A")
                        cmbMemAccess.Enabled = false;
                    if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "N")
                    {
                        this.Size = new Size(760, 385);
                        labelNResp.Visible = lblReqResponse.Visible = lblEqualTo.Visible = txtEqualTo.Visible = lblGreaterthan.Visible = txtGreaterthan.Visible = lblLessthan.Visible = txtLessthan.Visible = true;
                        txtEqualTo.Enabled = txtGreaterthan.Enabled = txtLessthan.Enabled = false;
                        txtCity.Visible = false;
                        lblResponse.Visible = lblcmbReqResponse.Visible = cmbResponce.Visible = false;
                        cmbMemAccess.Enabled = false;
                        cmbQuestionConjuction.Enabled = false;
                        txtEqualTo.Text = string.Empty;
                        txtGreaterthan.Text = string.Empty;
                        txtLessthan.Text = string.Empty;
                        lblMemAccess.Location = new Point(15, 9);
                        lblReqMemaccess.Location = new Point(83, 7);
                        cmbMemAccess.Location = new Point(100, 5);
                        lblQuesConjunction.Location = new Point(252, 9);
                        cmbQuestionConjuction.Location = new Point(329, 5);
                        labelNResp.Location = new Point(15, 41); lblReqResponse.Location = new Point(68, 38);
                    }
                    else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "D" || dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "L")
                    {
                        this.Size = new Size(760, 385);
                        labelNResp.Visible = lblReqResponse.Visible = lblEqualTo.Visible = txtEqualTo.Visible = lblGreaterthan.Visible = txtGreaterthan.Visible = lblLessthan.Visible = txtLessthan.Visible = false;
                        txtEqualTo.Text = string.Empty;
                        txtGreaterthan.Text = string.Empty;
                        txtLessthan.Text = string.Empty;
                        txtCity.Visible = false;
                        lblResponse.Visible = lblcmbReqResponse.Visible = cmbResponce.Visible = true;
                        cmbResponce.Enabled = false;
                        cmbMemAccess.Enabled = cmbResponce.Enabled = false;
                            cmbQuestionConjuction.Enabled = false;
                        //**lblMemAccess.Location = new Point(15, 9); lblReqMemaccess.Location = new Point(83, 7); cmbMemAccess.Location = new Point(100, 5);
                        //lblResponse.Location = new Point(415, 9); lblcmbReqResponse.Location = new Point(468, 7); cmbResponce.Location = new Point(488, 5);
                        lblResponse.Location = new Point(15, 41); lblcmbReqResponse.Location = new Point(68, 38); cmbResponce.Location = new Point(100, 37);
                        //**lblQuesConjunction.Location = new Point(252, 9); cmbQuestionConjuction.Location = new Point(329, 5);

                        if (dgvQuestions.CurrentRow.Cells["dgvQuesType"].Value.ToString() == Consts.Common.Standard)
                        {
                            cmbResponce.Items.Clear();
                            cmbResponce.Items.Insert(0, new ListItem("    ", "0"));
                            cmbResponce.SelectedIndex = 0;

                            if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "L")
                            {
                                AGYTABSEntity Search_Agytabs = new AGYTABSEntity(true);
                                Search_Agytabs.Tabs_Type = dgvQuestions.CurrentRow.Cells["dgvQuesAgyCode"].Value.ToString();
                                List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(Search_Agytabs);
                                foreach (AGYTABSEntity standardQues in AgyTabs_List)
                                {
                                    ListItem listItem = new ListItem(standardQues.Code_Desc, standardQues.Table_Code);
                                    cmbResponce.Items.Add(listItem);
                                }
                            }
                            else
                            {
                                List<CommonEntity> Response = _model.lookupDataAccess.GetLookkupFronAGYTAB(dgvQuestions.CurrentRow.Cells["dgvQuesAgyCode"].Value.ToString());
                                foreach (CommonEntity standQuestion in Response)
                                {
                                    ListItem li = new ListItem(standQuestion.Desc, standQuestion.Code);
                                    cmbResponce.Items.Add(li);
                                    if (Mode.Equals(Consts.Common.Add) && standQuestion.Default.Equals("Y"))
                                        cmbResponce.SelectedItem = li;
                                }
                            }
                        }
                        else
                        {
                            cmbResponce.Items.Clear();
                            cmbResponce.Items.Insert(0, new ListItem("    ", "0"));
                            cmbResponce.SelectedIndex = 0;
                            List<CustRespEntity> custRepEntity = _model.FieldControls.GetCustRespByCustCode(dgvQuestions.CurrentRow.Cells["dgvQuesAgyCode"].Value.ToString());
                            foreach (CustRespEntity customques in custRepEntity)
                            {
                                ListItem li = new ListItem(customques.RespDesc, customques.DescCode);
                                cmbResponce.Items.Add(li);
                            }
                        }
                    }

                    else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "T")
                    {
                        this.Size = new Size(760, 385);
                        lblEqualTo.Visible = txtEqualTo.Visible = lblGreaterthan.Visible = txtGreaterthan.Visible = lblLessthan.Visible = txtLessthan.Visible = false;
                        lblResponse.Visible = lblcmbReqResponse.Visible = cmbResponce.Visible = false;
                        cmbMemAccess.Enabled = false; 
                        cmbQuestionConjuction.Enabled = false;
                        //**lblMemAccess.Location = new Point(15, 9); lblReqMemaccess.Location = new Point(83, 7); cmbMemAccess.Location = new Point(100, 5);
                        labelNResp.Visible = true; lblReqResponse.Visible = true;
                        txtEqualTo.Text = string.Empty;
                        txtGreaterthan.Text = string.Empty;
                        txtLessthan.Text = string.Empty;
                        txtCity.Visible = true;
                        //labelNResp.Location = new Point(415, 9); lblReqResponse.Location = new Point(468, 7);
                        txtCity.Location = new Point(100, 37);
                        lblResponse.Location = new Point(15, 41); lblcmbReqResponse.Location = new Point(68, 38); cmbResponce.Location = new Point(100, 37);
                        txtCity.Enabled = false;
                        //**lblQuesConjunction.Location = new Point(252, 9); cmbQuestionConjuction.Location = new Point(329, 5);
                    }
                }
                else
                {
                    lblMemAccess.Visible = lblReqMemaccess.Visible = lblQuesConjunction.Visible = true; cmbQuestionConjuction.Visible = cmbQuestions.Visible = true;
                    labelNResp.Visible = lblReqResponse.Visible = lblEqualTo.Visible = lblGreaterthan.Visible = lblLessthan.Visible = false; txtEqualTo.Visible = txtGreaterthan.Visible = txtLessthan.Visible = false;
                    lblResponse.Visible = lblcmbReqResponse.Visible = cmbResponce.Visible = false;
                    txtCity.Visible = false;
                    //**lblMemAccess.Location = new Point(15, 9); lblReqMemaccess.Location = new Point(83, 7); cmbMemAccess.Location = new Point(100, 5);
                    //**lblQuesConjunction.Location = new Point(252, 9);
                    //**cmbQuestionConjuction.Location = new Point(329, 5);
                    this.Size = new Size(760, 385);
                }

                if (Mode == "Edit")
                {
                    cmbQuestions.Enabled = true;
                    if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "N")
                    {
                        txtEqualTo.Enabled = txtGreaterthan.Enabled = txtLessthan.Enabled = true;
                    }
                    else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "D" || dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "L")
                    {
                    }
                    else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "T")
                    {
                    }
                    Case0009Control _cntl = BaseForm.GetBaseUserControl() as Case0009Control;
                    if (_cntl._gvwQuesRowCnt > 1)
                        cmbQuestionConjuction.Enabled = true;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(cmbResponce, null);
            _errorProvider.SetError(cmbMemAccess, null);
            _errorProvider.SetError(cmbShowques, null);
            _errorProvider.SetError(labelNResp, null);
            _errorProvider.SetError(txtEqualTo, null);
            _errorProvider.SetError(txtGreaterthan, null);
            _errorProvider.SetError(txtLessthan, null);
            _errorProvider.SetError(txtCity, null);
            this.Size = new Size(760, 385);
            pnlAccessConj.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnAdd.Visible = false;
            dgvQuestions.Enabled = false; cmbShowques.Enabled = false;
            fillMemberAccess(dgvQuestions.CurrentRow.Cells["dgvQuesFieldType"].Value.ToString());
            if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "N")
            {
                this.Size = new Size(760, 385);
                txtEqualTo.Enabled = txtGreaterthan.Enabled = txtLessthan.Enabled = true;
                txtEqualTo.Text = txtGreaterthan.Text = txtLessthan.Text = string.Empty;
                cmbMemAccess.Enabled = cmbQuestionConjuction.Enabled = true;
                if (((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "H" || ((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "A")
                    cmbMemAccess.Enabled = false;
            }
            else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "D" || dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "L")
            {
                this.Size = new Size(760, 385);
                cmbResponce.Enabled = true;
                cmbMemAccess.Enabled = cmbResponce.Enabled = cmbQuestionConjuction.Enabled = true;
                if (((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "H" || ((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "A")
                    cmbMemAccess.Enabled = false;
            }
            else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "T")
            {
                this.Size = new Size(760, 385);
                txtCity.Text = string.Empty;
                txtCity.Enabled = true;
                cmbQuestionConjuction.Enabled = true;
                if (((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "H" || ((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "A")
                    cmbMemAccess.Enabled = false;
            }
            else
            {
                this.Size = new Size(760, 385);
                cmbMemAccess.Enabled = cmbResponce.Enabled = cmbQuestionConjuction.Enabled = true;
                if (((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "H" || ((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "A")
                    cmbMemAccess.Enabled = false;
            }
        }
        #endregion
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
            lblQuesConjunction.Location = new Point(393, 41/*76*/);
            cmbQuestionConjuction.Location = new Point(471, 37/*71*/);
            this.Size = new Size(550, 195);

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
                    lblReqResponse.Visible = false;
                    lblcmbReqResponse.Visible = true;
                    lblResponse.Visible = true;
                    cmbResponce.Visible = true;
                    txtCity.Visible = false;
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
                                if (Mode.Equals(Consts.Common.Add) && standQuestion.Default.Equals("Y"))
                                    cmbResponce.SelectedItem = li;
                            }
                        }

                    }
                    else
                    {
                        cmbResponce.Items.Clear();
                        cmbResponce.Items.Insert(0, new ListItem("    ", "0"));
                        cmbResponce.SelectedIndex = 0;
                        List<CustRespEntity> custRepEntity = _model.FieldControls.GetCustRespByCustCode(((ListItem)cmbQuestions.SelectedItem).ValueDisplayCode.ToString());
                        foreach (CustRespEntity customques in custRepEntity)
                        {
                            cmbResponce.Items.Add(new ListItem(customques.RespDesc, customques.DescCode));
                        }
                    }
                } 

            else if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "N")
            {
                lblcmbReqResponse.Visible = false;
                cmbResponce.Visible = false;
                lblResponse.Visible = false;
                lblReqResponse.Visible = true;
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

                txtCity.Visible = false;

                cmbQuestionConjuction.SelectedIndex = 0;

            }
            else if (((ListItem)cmbQuestions.SelectedItem).ID.ToString() == "T")
            {
                lblcmbReqResponse.Visible = false;
                cmbResponce.Visible = false;
                lblResponse.Visible = false;
                lblReqResponse.Visible = true;
                labelNResp.Visible = true;
                txtEqualTo.Visible = false;
                txtGreaterthan.Visible = false;
                txtLessthan.Visible = false;
                txtEqualTo.Text = string.Empty;
                txtGreaterthan.Text = string.Empty;
                txtLessthan.Text = string.Empty;
                lblEqualTo.Visible = false;
                lblGreaterthan.Visible = false;
                lblLessthan.Visible = false;
                cmbQuestionConjuction.SelectedIndex = 0;
                this.Size = new Size(550, 195);//(550, 195);
                this.txtCity.Location = new Point(100, 37);//(100, 71);
                txtCity.Visible = true;
            }
            }
            else
            {
                lblQuestion.Visible = lblReqQuestions.Visible = lblMemAccess.Visible = lblReqMemaccess.Visible = lblQuesConjunction.Visible = true;
                cmbQuestionConjuction.Visible = cmbQuestions.Visible = true;
                labelNResp.Visible = lblReqResponse.Visible = lblEqualTo.Visible = lblGreaterthan.Visible = lblLessthan.Visible = false;
                txtEqualTo.Visible = txtGreaterthan.Visible = txtLessthan.Visible = false;
                lblResponse.Visible = lblcmbReqResponse.Visible = cmbResponce.Visible = false;
                txtCity.Visible = false;
                lblQuesConjunction.Location = new Point(393,41);
                cmbQuestionConjuction.Location = new Point(471,37);
                this.Size = new Size(550, 163);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(cmbResponce, null);
            _errorProvider.SetError(cmbMemAccess, null);
            _errorProvider.SetError(cmbShowques, null);
            _errorProvider.SetError(labelNResp, null);
            _errorProvider.SetError(txtEqualTo, null);
            _errorProvider.SetError(txtGreaterthan, null);
            _errorProvider.SetError(txtLessthan, null);
            _errorProvider.SetError(txtCity, null);
            if (FormType == Consts.LiheAllDetails.strMainType)
            {
                 this.Close();
            }
            else if (FormType == Consts.LiheAllDetails.strSubType)
            {
                if (Mode == "Edit")
                    this.Close();
                else
                {
                    btnSave.Visible = btnCancel.Visible = false; btnAdd.Visible = true;
                    dgvQuestions.Enabled = true; cmbShowques.Enabled = true;
                    fillMemberAccess(dgvQuestions.CurrentRow.Cells["dgvQuesFieldType"].Value.ToString());
                    if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "N")
                    {
                        this.Size = new Size(760, 385);
                        txtEqualTo.Text = txtGreaterthan.Text = txtLessthan.Text = string.Empty;
                        txtEqualTo.Enabled = txtGreaterthan.Enabled = txtLessthan.Enabled = false;
                        cmbMemAccess.Enabled = cmbQuestionConjuction.Enabled = false;
                        cmbQuestionConjuction.SelectedIndex = 0;                      
                    }
                    else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "D" || dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "L")
                    {
                        this.Size = new Size(760, 385);
                        cmbResponce.Enabled = false;
                        cmbMemAccess.Enabled = cmbResponce.Enabled = cmbQuestionConjuction.Enabled = false;
                        cmbQuestionConjuction.SelectedIndex = 0; cmbResponce.SelectedIndex = 0;
                    }
                    else if (dgvQuestions.CurrentRow.Cells["dgvQuesRespType"].Value.ToString() == "T")
                    {
                        this.Size = new Size(760, 385);
                        txtCity.Enabled = false;
                        cmbQuestionConjuction.Enabled = false;
                        txtCity.Text = string.Empty;cmbQuestionConjuction.SelectedIndex = 0;
                    }
                }
            }
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, "Master", string.Empty);
            //hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.ShowDialog();
            HierarchieSelection addForm = new HierarchieSelection(BaseForm, string.Empty, "Master", string.Empty, "D", string.Empty, BaseForm.UserID);
            addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            addForm.StartPosition = FormStartPosition.CenterScreen;
            addForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    hierarchy += row.Agency + row.Dept + row.Prog;
                    if (hierarchy != "******")
                    {
                        txtHierarchy.Text = row.Code;
                        txtHierachydesc.Text = row.HirarchyName;
                        string strCode = CaseSum.GETCASEELIGGCode(row.Agency + row.Dept + row.Prog);
                        txtGroupCode.Text = "0000".Substring(0, 4 - strCode.Length) + strCode;
                        HiearchyCode = row.Agency + row.Dept + row.Prog;
                    }
                    else
                    {
                        AlertBox.Show("****** Hierachy Not Allowed", MessageBoxIcon.Warning);
                    }
                }
            }

        }

        List<CASESP1Entity> SP_Hierarchies;

        private void cmbMemAccess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Click(object sender, EventArgs e)
        {

        }

        private void pnlGroup_Click(object sender, EventArgs e)
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (FormType == Consts.LiheAllDetails.strMainType)
                Application.Navigate("https://vikash-chatla.gitbook.io/captain-1/system-administration-module/screens/eligibility-criteria/add-a-group-definition", target: "_blank");
            // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Case2009_Group");
            else if (FormType == Consts.LiheAllDetails.strSubType)
                Application.Navigate("https://vikash-chatla.gitbook.io/captain-1/system-administration-module/screens/eligibility-criteria/add-a-question-to-a-group", target: "_blank");
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Case2009_Question"); 
        }

        private void Case0009Form_Load(object sender, EventArgs e)
        {

        }

        private void Case0009Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "TL_QuesHELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps("CASE0009", 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
            if (e.Tool.Name == "TL_GroupHELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps("CASE0009", 2, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }

        //private void Fill_SP_Hierarchies_Grid()
        //{
        //    SP_Hierarchies = new List<CASESP1Entity>();
        //    SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(SP_Code, null, null, null);
        //    if (SP_Hierarchies.Count > 0)
        //    {
        //        string Hiename = null;
        //        HierarchyGrid.Rows.Clear();

        //        foreach (CASESP1Entity Entity in SP_Hierarchies)
        //        {
        //            int rowIndex = 0;
        //            HierarchyEntity hierchyEntity = null;

        //            if (Entity.Agency + Entity.Dept + Entity.Prog == "******")
        //            {
        //                Hiename = "All Hierarchies";
        //                hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
        //            }
        //            else
        //                Hiename = "Not Defined in CASEHIE";

        //            DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Entity.Agency, Entity.Dept, Entity.Prog);
        //            if (ds_Hie_Name.Tables.Count > 0)
        //            {
        //                if (ds_Hie_Name.Tables[0].Rows.Count > 0)
        //                {
        //                    if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
        //                        Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

        //                    //hierchyEntity = new HierarchyEntity(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Hiename);

        //                    if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
        //                        ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
        //                    if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
        //                        ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";

        //                    hierchyEntity = new HierarchyEntity(ds_Hie_Name.Tables[0].Rows[0], "CASEHIE");
        //                }
        //            }

        //            rowIndex = HierarchyGrid.Rows.Add(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog + "  " + Hiename, Entity.Agency + Entity.Dept + Entity.Prog);
        //            HierarchyGrid.Rows[rowIndex].Tag = hierchyEntity;
        //        }
        //    }
        //}
    }
}