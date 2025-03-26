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
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using Captain.Common.Views.UserControls;

#endregion
namespace Captain.Common.Views.Forms
{
    public partial class SSBGParam_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public SSBGParam_Form(BaseForm baseForm, string mode, string strType, string strID, string strParamSeq, string strTypeCode, string strTypeCodeSeq, string strHierchyCode, string strHierchydesc, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            strgroupcode = strParamSeq;
            TypeCode = strTypeCode;
            TypeSeq = strTypeCodeSeq;
            strSeqCode = strID;
            strHierachycode = strHierchyCode;
            HiearchyDesc = strHierchydesc;
            FormType = strType;
            //**lblHeader.Text = Privileges.PrivilegeName;
            this.Text = Privileges.PrivilegeName;
            this.Text = Privileges.Program + " - " + Mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            txtYear.Validator = TextBoxValidation.FloatValidator;
            txtBudget.Validator = TextBoxValidation.FloatValidator;
            txtAward.Validator = TextBoxValidation.FloatValidator;
            txtCntlOblig.Validator = TextBoxValidation.IntegerValidator;
            
            fillcombo();
            if (FormType == Consts.LiheAllDetails.strMainType)
            {
                //**lblHeader.Text = "Report Header Control";
                this.Text = "Report Header Control";
                pnlGroup.Visible = true;
                pnlCntltypes.Visible = false; pnlQuestions.Visible = false;pnlBottom.Visible = false;
                //**this.pnlGroup.Location = new System.Drawing.Point(0, 50);
                //this.Size = new System.Drawing.Size(543, 222);
                //this.Size = new System.Drawing.Size(549, 314); //modified on 05/30/2017
                //this.Size = new System.Drawing.Size(549, 364); //modified on 06/10/2017
                //this.Size = new System.Drawing.Size(761, 497); //modified on 06/22/2017
                this.Size = new System.Drawing.Size(880,645);//(761, 574); //modified on 06/23/2017
                FillCustomQues();
                if (Mode == "Add")
                {
                    if (strHierchyCode != string.Empty)
                    {
                        txtHierarchy.Text = strHierchyCode;
                        txtHierachydesc.Text = strHierchydesc;
                        string strCode = CaseSum.GETSSBGCode(strHierachycode.Substring(0, 2) + strHierachycode.Substring(3, 2) + strHierachycode.Substring(6, 2));
                        txtYear.Text = strCode;
                    }
                    
                }
                else
                {
                    
                    string strCode = strgroupcode;
                    txtYear.Text =  strCode;
                    List<SSBGPARAMEntity> caseEligGEntity = _model.CaseSumData.GetSSBGParams(strHierachycode.Substring(0, 2), strHierachycode.Substring(3, 2), strHierachycode.Substring(6, 2));
                    caseEligGEntity = caseEligGEntity.FindAll(u => u.SSBGSeq.Equals(strSeqCode) && u.SSBGYear.Equals(strCode));
                    foreach (var EligQDetails in caseEligGEntity)
                    {
                        txtGroupDesc.Text = EligQDetails.SSBGDesc;
                        txtGroupDesc.Text = EligQDetails.SSBGDesc.Trim();
                        txtYear.Text = EligQDetails.SSBGYear;
                        txtBudget.Text = EligQDetails.SSBGBudget;
                        txtAward.Text = EligQDetails.SSBGAward;
                        if (!string.IsNullOrEmpty(EligQDetails.SSBGGPFrom.Trim()))
                        {
                            dtpGPFrom.Checked = true;
                            dtpGPFrom.Text = EligQDetails.SSBGGPFrom;
                        }
                        if (!string.IsNullOrEmpty(EligQDetails.SSBGGPTo.Trim()))
                        {
                            dtpGPTo.Checked = true;
                            dtpGPTo.Text = EligQDetails.SSBGGPTo;
                        }
                        //dtpGPFrom.Text = EligQDetails.SSBGGPFrom;
                        //dtpGPTo.Text = EligQDetails.SSBGGPTo;
                        if (!string.IsNullOrEmpty(EligQDetails.SSBGRPDate.Trim()))
                        {
                            dtpRepDate.Checked = true;
                            dtpRepDate.Text = EligQDetails.SSBGRPDate;
                        }
                        //dtpRPFrom.Text = EligQDetails.SSBGRPFrom;
                        //dtpRPTo.Text = EligQDetails.SSBGRPTo;
                        txtProgDesc.Text = EligQDetails.SSBGProgDesc;
                        txtSeq.Text = EligQDetails.SSBGSeq;
                        txtCntArea1.Text = EligQDetails.SSBGCntArea1;
                        txtCntArea2.Text = EligQDetails.SSBGCntArea2;
                        txtCntArea3.Text = EligQDetails.SSBGCntArea3;
                        txtCntArea4.Text = EligQDetails.SSBGCntArea4;
                        txtCntArea5.Text = EligQDetails.SSBGCntArea5;

                        if (EligQDetails.SSBGCntArea1_Chk == "1") { chkGrp1.Checked = true; } else chkGrp1.Checked = false;
                        if (EligQDetails.SSBGCntArea2_Chk == "1") { chkGrp2.Checked = true; } else chkGrp2.Checked = false;
                        if (EligQDetails.SSBGCntArea3_Chk == "1") { chkGrp3.Checked = true; } else chkGrp3.Checked = false;
                        if (EligQDetails.SSBGCntArea4_Chk == "1") { chkGrp4.Checked = true; } else chkGrp4.Checked = false;
                        if (EligQDetails.SSBGCntArea5_Chk == "1") { chkGrp5.Checked = true; } else chkGrp5.Checked = false;

                        if (!string.IsNullOrEmpty(EligQDetails.SSBGCntArea1_Val.Trim()))
                        {
                            CommonFunctions.SetComboBoxValue(cmbGrp1, EligQDetails.SSBGCntArea1_Val);
                            if (EligQDetails.SSBGCntArea1_Val.Trim() == "Q")
                            {
                                this.cmbGrp1.SelectedIndexChanged -= new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                                CommonFunctions.SetComboBoxValue(cmbCust1, EligQDetails.SSBGCust1);
                                this.cmbGrp1.SelectedIndexChanged += new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                            }
                        }
                        if (!string.IsNullOrEmpty(EligQDetails.SSBGCntArea2_Val.Trim()))
                        {
                            CommonFunctions.SetComboBoxValue(cmbGrp2, EligQDetails.SSBGCntArea2_Val);
                            if (EligQDetails.SSBGCntArea2_Val.Trim() == "Q")
                            {
                                this.cmbGrp2.SelectedIndexChanged -= new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                                CommonFunctions.SetComboBoxValue(cmbCust2, EligQDetails.SSBGCust2);
                                this.cmbGrp2.SelectedIndexChanged += new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                            }
                        }
                        if (!string.IsNullOrEmpty(EligQDetails.SSBGCntArea3_Val.Trim()))
                        {
                            CommonFunctions.SetComboBoxValue(cmbGrp3, EligQDetails.SSBGCntArea3_Val);
                            if (EligQDetails.SSBGCntArea3_Val.Trim() == "Q")
                            {
                                CommonFunctions.SetComboBoxValue(cmbCust3, EligQDetails.SSBGCust3);
                            }
                        }
                        if (!string.IsNullOrEmpty(EligQDetails.SSBGCntArea4_Val.Trim()))
                        {
                            CommonFunctions.SetComboBoxValue(cmbGrp4, EligQDetails.SSBGCntArea4_Val);
                            if (EligQDetails.SSBGCntArea4_Val.Trim() == "Q")
                                CommonFunctions.SetComboBoxValue(cmbCust4, EligQDetails.SSBGCust4);
                        }
                        if (!string.IsNullOrEmpty(EligQDetails.SSBGCntArea5_Val.Trim()))
                        {
                            CommonFunctions.SetComboBoxValue(cmbGrp5, EligQDetails.SSBGCntArea5_Val);
                            if (EligQDetails.SSBGCntArea5_Val.Trim() == "Q")
                                CommonFunctions.SetComboBoxValue(cmbCust5, EligQDetails.SSBGCust5);
                        }


                    }
                    PbHierarchies.Visible = false;
                    txtHierarchy.Text = strHierchyCode;
                    txtHierachydesc.Text = strHierchydesc;
                }
            }
            else if (FormType == Consts.LiheAllDetails.strSubType)
            {
                //**lblHeader.Text = "Control Types";
                this.Text = "Control Types";
                pnlGroup.Visible = false; pnlQuestions.Visible = false; pnlTypes.Visible = false;
                pnlBottom.Visible = true;
                pnlLeft.Visible = true;pnlLeft.Dock = DockStyle.Fill;
                pnlCntltypes.Visible = true; pnlCntltypes.Dock = DockStyle.Fill;
                //**this.lblCntType.Location = new System.Drawing.Point(220, 36);
                //**this.cmbCntType.Location = new System.Drawing.Point(295, 33);
                lblCntType.Visible = true; cmbCntType.Visible = true;
                this.txtTDesc.Size = new System.Drawing.Size(424, 25);
                lblType2.Visible = false; cmbType2.Visible = false;
                txtTDesc.MaxLength = 50;
                txtTDesc.Multiline = false;
               //** this.pnlCntltypes.Location = new System.Drawing.Point(0, 50);
                //this.pnlTypes.Location = new System.Drawing.Point(0, 50);
                //this.Size = new System.Drawing.Size(549, 143);
                this.Size = new System.Drawing.Size(550, 170);//modified on 06/28/2017
                //this.Size = new System.Drawing.Size(549, 299); //modified on 06/10/2017
                //List<SSBGPARAMEntity> caseEligGEntity = _model.CaseSumData.GetSSBGParams(strHierachycode.Substring(0, 2), strHierachycode.Substring(3, 2), strHierachycode.Substring(6, 2));
                //caseEligGEntity = caseEligGEntity.FindAll(u => u.SSBGSeq.Equals(strSeqCode) && u.SSBGYear.Equals(strgroupcode));
                //if (caseEligGEntity.Count>0)
                //{
                //    if(!string.IsNullOrEmpty(caseEligGEntity[0].SSBGCntArea1.Trim()))
                //        chkC1.Text = caseEligGEntity[0].SSBGCntArea1.Trim();
                //    if (!string.IsNullOrEmpty(caseEligGEntity[0].SSBGCntArea2.Trim()))
                //        chkC2.Text = caseEligGEntity[0].SSBGCntArea2.Trim();
                //    if (!string.IsNullOrEmpty(caseEligGEntity[0].SSBGCntArea3.Trim()))
                //        chkC3.Text = caseEligGEntity[0].SSBGCntArea3.Trim();
                //    if (!string.IsNullOrEmpty(caseEligGEntity[0].SSBGCntArea4.Trim()))
                //        chkC4.Text = caseEligGEntity[0].SSBGCntArea4.Trim();
                //    if (!string.IsNullOrEmpty(caseEligGEntity[0].SSBGCntArea5.Trim()))
                //        chkC5.Text = caseEligGEntity[0].SSBGCntArea5.Trim();
                //}

                if (Mode == "Add")
                {
                    btnMonths.Visible = false;
                }
                else
                {
                    btnMonths.Visible = true;
                    List<SSBGTYPESEntity> SSBGTypesall = _model.CaseSumData.GetSSBGTypes();
                    List<SSBGTYPESEntity> caseEligGroups = SSBGTypesall.FindAll(u => u.SBGTID.Equals(strSeqCode) && u.SBGTCode.Equals(TypeCode) && u.SBGTCodeSeq.Equals("0"));

                    if (caseEligGroups.Count > 0)
                    {
                        foreach (var EligQDetails in caseEligGroups)
                        {
                            txtTDesc.Text = EligQDetails.SBGTDesc.Trim();
                            txtCode.Text = EligQDetails.SBGTCode;
                            txtCntlOblig.Text = EligQDetails.SBGTCntlOblig;
                            CommonFunctions.SetComboBoxValue(cmbCntType, EligQDetails.SBGTMemAccess);
                            //if (EligQDetails.SBGT_CNTAREA1 == "1") { chkC1.Checked = true; } else chkC1.Checked = false;
                            //if (EligQDetails.SBGT_CNTAREA2 == "1") { chkC2.Checked = true; } else chkC2.Checked = false;
                            //if (EligQDetails.SBGT_CNTAREA3 == "1") { chkC3.Checked = true; } else chkC3.Checked = false;
                            //if (EligQDetails.SBGT_CNTAREA4 == "1") { chkC4.Checked = true; } else chkC4.Checked = false;
                            //if (EligQDetails.SBGT_CNTAREA5 == "1") { chkC5.Checked = true; } else chkC5.Checked = false;
                            
                            //if(!string.IsNullOrEmpty(EligQDetails.SBGT_CNTAREA1_VAL.Trim()))
                            //    CommonFunctions.SetComboBoxValue(cmbC1, EligQDetails.SBGT_CNTAREA1_VAL);
                            //if (!string.IsNullOrEmpty(EligQDetails.SBGT_CNTAREA2_VAL.Trim()))
                            //    CommonFunctions.SetComboBoxValue(cmbC2, EligQDetails.SBGT_CNTAREA2_VAL);
                            //if (!string.IsNullOrEmpty(EligQDetails.SBGT_CNTAREA3_VAL.Trim()))
                            //    CommonFunctions.SetComboBoxValue(cmbC3, EligQDetails.SBGT_CNTAREA3_VAL);
                            //if (!string.IsNullOrEmpty(EligQDetails.SBGT_CNTAREA4_VAL.Trim()))
                            //    CommonFunctions.SetComboBoxValue(cmbC4, EligQDetails.SBGT_CNTAREA4_VAL);
                            //if (!string.IsNullOrEmpty(EligQDetails.SBGT_CNTAREA5_VAL.Trim()))
                            //    CommonFunctions.SetComboBoxValue(cmbC5, EligQDetails.SBGT_CNTAREA5_VAL);
                        }
                    }
                }
            }
            else if (FormType == "GOAL")
            {
                //**lblHeader.Text = "Goal Definition";
                this.Text = "Goal Definition";
                pnlGroup.Visible = false; pnlQuestions.Visible = false; pnlTypes.Visible = false;
                pnlBottom.Visible = true;
                pnlLeft.Visible = true; pnlLeft.Dock = DockStyle.Fill;
                pnlCntltypes.Visible = true; pnlCntltypes.Dock = DockStyle.Fill;
                this.lblCntType.Location = new System.Drawing.Point(15,77);//(12, 61);
                this.cmbCntType.Location = new System.Drawing.Point(90,73);//(107, 58);
                lblCntType.Visible = true; cmbCntType.Visible = true;
                lblType2.Visible = true; cmbType2.Visible = true;
                this.lblType2.Location = new System.Drawing.Point(295, 77);//(12, 61);
                this.cmbType2.Location = new System.Drawing.Point(340, 73);//(107, 58);
                //lblCntType.Visible = false; cmbCntType.Visible = false;
                lblCntlOblig.Visible = false; txtCntlOblig.Visible = false;
                this.txtTDesc.Size = new System.Drawing.Size(425, 56);
                txtTDesc.MaxLength = 500; txtTDesc.Multiline = true;
                //** this.pnlCntltypes.Location = new System.Drawing.Point(0, 50);
                //this.Size = new System.Drawing.Size(549, 143);
                this.Size = new System.Drawing.Size(550, 200);//(549, 168);//modified on 06/28/2017
                if (Mode == "Add") { }
                else
                {
                    List<SSBGGOALSEntity> SSBGGoalsall = _model.CaseSumData.GetSSBGGoals();
                    List<SSBGGOALSEntity> caseEligGroups = SSBGGoalsall.FindAll(u => u.SBGGID.Equals(strSeqCode) && u.SBGGCode.Equals(TypeCode) && u.SBGGSeq.Equals("0"));

                    if (caseEligGroups.Count > 0)
                    {
                        foreach (var EligQDetails in caseEligGroups)
                        {
                            txtTDesc.Text = EligQDetails.SBGGDesc.Trim();
                            txtCode.Text = EligQDetails.SBGGCode;
                            CommonFunctions.SetComboBoxValue(cmbCntType, EligQDetails.SBGGCountType);
                            CommonFunctions.SetComboBoxValue(cmbType2, EligQDetails.Type2);
                            //txtCntlOblig.Text = EligQDetails.SBGTCntlOblig;
                        }
                    }
                }

            }
            else if (FormType == "OBJECT")
            {
                //**lblHeader.Text = "Objective Definition";
                this.Text = "Objective Definition";
                pnlGroup.Visible = false; pnlQuestions.Visible = false; pnlTypes.Visible = false;
                pnlBottom.Visible = true;
                pnlLeft.Visible = true; pnlLeft.Dock = DockStyle.Fill;
                pnlCntltypes.Visible = true; pnlCntltypes.Dock = DockStyle.Fill;
                //this.lblCntType.Location = new System.Drawing.Point(12, 61);
                //this.cmbCntType.Location = new System.Drawing.Point(107, 58);
                //lblCntType.Visible = true; cmbCntType.Visible = true;
                //Added by Sudheer on 02/28/2022 as per Joe document
                lblType2.Visible = false; cmbType2.Visible = false;
                lblCntType.Visible = false; cmbCntType.Visible = false;

                lblCntlOblig.Visible = false; txtCntlOblig.Visible = false;
                this.txtTDesc.Size = new System.Drawing.Size(425, 56);
                txtTDesc.MaxLength = 500; txtTDesc.Multiline = true;
                //**this.pnlCntltypes.Location = new System.Drawing.Point(0, 50);
                //** lblType2.Visible = false; cmbType2.Visible = false;
                //this.Size = new System.Drawing.Size(549, 143);
                this.Size = new System.Drawing.Size(550, 170);//(549, 168);//modified on 06/28/2017
                if (Mode == "Add") { }
                else
                {
                    List<SSBGGOALSEntity> SSBGGoalsall = _model.CaseSumData.GetSSBGGoals();
                    List<SSBGGOALSEntity> caseEligGroups = SSBGGoalsall.FindAll(u => u.SBGGID.Equals(strSeqCode) && u.SBGGCode.Equals(TypeCode) && u.SBGGSeq.Equals(TypeSeq));

                    if (caseEligGroups.Count > 0)
                    {
                        foreach (var EligQDetails in caseEligGroups)
                        {
                            txtTDesc.Text = EligQDetails.SBGGDesc.Trim();
                            txtCode.Text = EligQDetails.SBGGCode;
                            //CommonFunctions.SetComboBoxValue(cmbCntType, EligQDetails.SBGGCountType);
                            //CommonFunctions.SetComboBoxValue(cmbType2, EligQDetails.Type2);
                            //txtCntlOblig.Text = EligQDetails.SBGTCntlOblig;
                        }
                    }
                }

            }
            //else if (FormType == "Questions")
            //{
            //    lblHeader.Text = "SSBG Conditions";
            //    pnlGroup.Visible = false; pnlCntltypes.Visible = false;
            //    pnlQuestions.Visible = true;
            //    this.pnlQuestions.Location = new System.Drawing.Point(0, 51);
            //    this.Size = new System.Drawing.Size(549, 170);
            //    if (Mode == "Add")
            //    {
            //    }
            //    else
            //    {
            //        List<SSBGTYPESEntity> SSBGTypesall = _model.CaseSumData.GetSSBGTypes();
            //        List<SSBGTYPESEntity> caseEligGroups = SSBGTypesall.FindAll(u => u.SBGTID.Equals(strSeqCode) && u.SBGTCode.Equals(TypeCode) && u.SBGTCodeSeq.Equals(TypeSeq));
            //        if (caseEligGroups.Count > 0)
            //        {
            //            foreach (var EligQDetails in caseEligGroups)
            //            {
            //                CommonFunctions.SetComboBoxValue(cmbQuestions, EligQDetails.SBGTQuesCode);
            //                CommonFunctions.SetComboBoxValue(cmbMemAccess, EligQDetails.SBGTMemAccess);
            //                CommonFunctions.SetComboBoxValue(cmbResponce, EligQDetails.SBGTRESP);
            //                if (EligQDetails.SBGTConj != string.Empty)
            //                    CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, EligQDetails.SBGTConj);
            //                else
            //                    CommonFunctions.SetComboBoxValue(cmbQuestionConjuction, "0");
            //                txtEqualTo.Text = EligQDetails.SBGTNumEqual;
            //                txtGreaterthan.Text = EligQDetails.SBGTNumGThan;
            //                txtLessthan.Text = EligQDetails.SBGTNumLThan;
            //            }
            //        }
            //    }
            //}
        }

        #region Properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string Mode { get; set; }
        public string strgroupcode { get; set; }
        public string TypeCode { get; set; }
        public string TypeSeq { get; set; }
        public string strSeqCode { get; set; }
        public string FormType { get; set; }
        public string strHierachycode { get; set; }
        public string HiearchyCode { get; set; }
        public string strQuestionCode { get; set; }
        public string HiearchyDesc { get; set; }
        

        #endregion

        private void fillcombo()
        {
            if (FormType == Consts.LiheAllDetails.strMainType)
            {
                //cmbCntType.Items.Clear();
                //List<ListItem> ampmlist = new List<ListItem>();
                //ampmlist.Add(new ListItem("Individual Count", "I"));
                //ampmlist.Add(new ListItem("Family Count", "F"));
                //cmbCntType.Items.AddRange(ampmlist.ToArray());
                //cmbCntType.SelectedIndex = 0;

                cmbGrp1.Items.Clear();
                List<ListItem> ampmlist1 = new List<ListItem>();
                ampmlist1.Add(new ListItem("", ""));
                ampmlist1.Add(new ListItem("Intake Date", "I"));
                ampmlist1.Add(new ListItem("Custom Question", "Q"));
                ampmlist1.Add(new ListItem("Contact Date", "C"));
                ampmlist1.Add(new ListItem("Contact Date Duplicates", "D"));
                ampmlist1.Add(new ListItem("Activity Date", "A"));
                ampmlist1.Add(new ListItem("Activity Date Duplicates", "B"));
                ampmlist1.Add(new ListItem("Service Plan Start Date", "S"));
                ampmlist1.Add(new ListItem("Service Plan Start Date Duplicates", "T"));
                cmbGrp1.Items.AddRange(ampmlist1.ToArray());
                this.cmbGrp1.SelectedIndexChanged -= new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                cmbGrp1.SelectedIndex = 0;
                this.cmbGrp1.SelectedIndexChanged += new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);

                cmbGrp2.Items.Clear();
                List<ListItem> ampmlist2 = new List<ListItem>();
                ampmlist2.Add(new ListItem("", ""));
                ampmlist2.Add(new ListItem("Intake Date", "I"));
                ampmlist2.Add(new ListItem("Custom Question", "Q"));
                ampmlist2.Add(new ListItem("Contact Date", "C"));
                ampmlist2.Add(new ListItem("Contact Date Duplicates", "D"));
                ampmlist2.Add(new ListItem("Activity Date", "A"));
                ampmlist2.Add(new ListItem("Activity Date Duplicates", "B"));
                ampmlist2.Add(new ListItem("Service Plan Start Date", "S"));
                ampmlist2.Add(new ListItem("Service Plan Start Date Duplicates", "T"));
                cmbGrp2.Items.AddRange(ampmlist2.ToArray());
                this.cmbGrp2.SelectedIndexChanged -= new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                cmbGrp2.SelectedIndex = 0;
                this.cmbGrp2.SelectedIndexChanged += new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);

                cmbGrp3.Items.Clear();
                List<ListItem> ampmlist3 = new List<ListItem>();
                ampmlist3.Add(new ListItem("", ""));
                ampmlist3.Add(new ListItem("Intake Date", "I"));
                ampmlist3.Add(new ListItem("Custom Question", "Q"));
                ampmlist3.Add(new ListItem("Contact Date", "C"));
                ampmlist3.Add(new ListItem("Contact Date Duplicates", "D"));
                ampmlist3.Add(new ListItem("Activity Date", "A"));
                ampmlist3.Add(new ListItem("Activity Date Duplicates", "B"));
                ampmlist3.Add(new ListItem("Service Plan Start Date", "S"));
                ampmlist3.Add(new ListItem("Service Plan Start Date Duplicates", "T"));
                cmbGrp3.Items.AddRange(ampmlist3.ToArray());
                this.cmbGrp3.SelectedIndexChanged -= new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                cmbGrp3.SelectedIndex = 0;
                this.cmbGrp3.SelectedIndexChanged += new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);

                cmbGrp4.Items.Clear();
                List<ListItem> ampmlist4 = new List<ListItem>();
                ampmlist4.Add(new ListItem("", ""));
                ampmlist4.Add(new ListItem("Intake Date", "I"));
                ampmlist4.Add(new ListItem("Custom Question", "Q"));
                ampmlist4.Add(new ListItem("Contact Date", "C"));
                ampmlist4.Add(new ListItem("Contact Date Duplicates", "D"));
                ampmlist4.Add(new ListItem("Activity Date", "A"));
                ampmlist4.Add(new ListItem("Activity Date Duplicates", "B"));
                ampmlist4.Add(new ListItem("Service Plan Start Date", "S"));
                ampmlist4.Add(new ListItem("Service Plan Start Date Duplicates", "T"));
                cmbGrp4.Items.AddRange(ampmlist4.ToArray());
                this.cmbGrp4.SelectedIndexChanged -= new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                cmbGrp4.SelectedIndex = 0;
                this.cmbGrp4.SelectedIndexChanged += new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);

                cmbGrp5.Items.Clear();
                List<ListItem> ampmlist5 = new List<ListItem>();
                ampmlist5.Add(new ListItem("", ""));
                ampmlist5.Add(new ListItem("Intake Date", "I"));
                ampmlist5.Add(new ListItem("Custom Question", "Q"));
                ampmlist5.Add(new ListItem("Contact Date", "C"));
                ampmlist5.Add(new ListItem("Contact Date Duplicates", "D"));
                ampmlist5.Add(new ListItem("Activity Date", "A"));
                ampmlist5.Add(new ListItem("Activity Date Duplicates", "B"));
                ampmlist5.Add(new ListItem("Service Plan Start Date", "S"));
                ampmlist5.Add(new ListItem("Service Plan Start Date Duplicates", "T"));
                cmbGrp5.Items.AddRange(ampmlist5.ToArray());
                this.cmbGrp5.SelectedIndexChanged -= new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
                cmbGrp5.SelectedIndex = 0;
                this.cmbGrp5.SelectedIndexChanged += new System.EventHandler(this.cmbGrp5_SelectedIndexChanged);
            }
            if (FormType == Consts.LiheAllDetails.strSubType)
            {
                cmbCntType.Items.Clear();
                List<ListItem> ampmlist = new List<ListItem>();
                ampmlist.Add(new ListItem("Individual Count", "I"));
                ampmlist.Add(new ListItem("Family Count", "F"));
                cmbCntType.Items.AddRange(ampmlist.ToArray());
                cmbCntType.SelectedIndex = 0;
            }
            if (FormType == "GOAL")//(FormType == "OBJECT")
            {
                cmbCntType.Items.Clear();
                List<ListItem> ampmlist = new List<ListItem>();
                ampmlist.Add(new ListItem(" ", "0"));
                ampmlist.Add(new ListItem("All Service Outcome completed", "A"));
                ampmlist.Add(new ListItem("Any one Service Outcome", "N"));
                ampmlist.Add(new ListItem("Greater than 1 Service Outcome", "1"));
                ampmlist.Add(new ListItem("Greater than 2", "2"));
                ampmlist.Add(new ListItem("Service Plan Start Date", "S"));
                ampmlist.Add(new ListItem("Actual Completion Date", "C"));
                cmbCntType.Items.AddRange(ampmlist.ToArray());
                cmbCntType.SelectedIndex = 0;

                cmbType2.Items.Clear();
                List<ListItem> ampmlist1 = new List<ListItem>();
                ampmlist1.Add(new ListItem(" ", "0"));
                ampmlist1.Add(new ListItem("All Service Outcome completed", "A"));
                ampmlist1.Add(new ListItem("Any one Service Outcome", "N"));
                ampmlist1.Add(new ListItem("Greater than 1 Service Outcome", "1"));
                ampmlist1.Add(new ListItem("Greater than 2", "2"));
                ampmlist1.Add(new ListItem("Service Plan Start Date", "S"));
                ampmlist1.Add(new ListItem("Actual Completion Date", "C"));
                cmbType2.Items.AddRange(ampmlist1.ToArray());
                cmbType2.SelectedIndex = 0;
            }

            if (FormType == "Questions")
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
                cmbQuestions.SelectedIndex = 0;
            }


            List<CommonEntity> commonconjunction = _model.lookupDataAccess.GetConjunctions();
            foreach (CommonEntity conjunction in commonconjunction)
            {
                cmbQuestionConjuction.Items.Add(new ListItem(conjunction.Desc, conjunction.Code));
                //cmbConjuction.Items.Add(new ListItem(conjunction.Desc, conjunction.Code));
            }
            cmbQuestionConjuction.Items.Insert(0, new ListItem("    ", "0"));
            cmbQuestionConjuction.SelectedIndex = 0;
            

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

        //private void FillControls()
        //{
        //    if (gvwHierachy.Rows.Count > 0)
        //    {
        //        string strHierchycode = gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString();
        //        List<SSBGPARAMEntity> caseEligGroups = SSBGParamall.FindAll(u => ((u.SSBGAgency + u.SSBGDept + u.SSBGProgram).Equals(strHierchycode)));
        //        if (caseEligGroups.Count > 0)
        //        {
        //            ClearControls();
        //            var EligYears = caseEligGroups.Select(u => u.SSBGYear).Distinct().ToList();
        //            cmbYear.Items.Clear();

        //            if (EligYears.Count > 0)
        //            {
        //                foreach (var Years in EligYears)
        //                {
        //                    cmbYear.Items.Add(new Captain.Common.Utilities.ListItem(Years, Years));
        //                }
        //                cmbYear.SelectedIndex = 0;
        //            }

        //            foreach (var EligQDetails in caseEligGroups)
        //            {
        //                txtHierarchy.Text = gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString();
        //                txtHierachydesc.Text = gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString();
        //                CommonFunctions.SetComboBoxValue(cmbYear, EligQDetails.SSBGYear);
        //                txtGroupDesc.Text = EligQDetails.SSBGDesc.Trim();
        //                txtYear.Text = EligQDetails.SSBGYear;
        //                txtBudget.Text = EligQDetails.SSBGBudget;
        //                txtAward.Text = EligQDetails.SSBGAward;
        //                dtpGPFrom.Text = EligQDetails.SSBGGPFrom;
        //                dtpGPTo.Text = EligQDetails.SSBGGPTo;
        //                dtpRepDate.Text = EligQDetails.SSBGRPDate;
        //                //dtpRPFrom.Text = EligQDetails.SSBGRPFrom;
        //                //dtpRPTo.Text = EligQDetails.SSBGRPTo;
        //                txtSeq.Text = EligQDetails.SSBGSeq;
        //            }
        //        }
        //    }

        //}

        //private void FillTypeControls()
        //{
            
        //    if (gvControl.Rows.Count > 0)
        //    {
        //        string strHierchycode = gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString();
        //        List<SSBGTYPESEntity> caseEligGroups = SSBGTypesall.FindAll(u => u.SBGTSeq.Equals(txtSeq.Text.Trim()) && u.SBGTCode.Equals(gvControl.SelectedRows[0].Cells["Code"].Value.ToString()));

        //        if (caseEligGroups.Count > 0)
        //        {
        //            foreach (var EligQDetails in caseEligGroups)
        //            {
        //                //txtHierarchy.Text = gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString();
        //                //txtHierachydesc.Text = gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString();
        //                //CommonFunctions.SetComboBoxValue(cmbYear, EligQDetails.SSBGYear);
        //                txtTDesc.Text = EligQDetails.SBGTDesc.Trim();
        //                txtCode.Text = EligQDetails.SBGTCode;
        //                txtCntlOblig.Text = EligQDetails.SBGTCntlOblig;
        //                //txtAward.Text = EligQDetails.SSBGAward;
        //                //dtpGPFrom.Text = EligQDetails.SSBGGPFrom;
        //                //dtpGPTo.Text = EligQDetails.SSBGGPTo;
        //                //dtpRepDate.Text = EligQDetails.SSBGRPDate;
        //                ////dtpRPFrom.Text = EligQDetails.SSBGRPFrom;
        //                ////dtpRPTo.Text = EligQDetails.SSBGRPTo;
        //                //txtSeq.Text = EligQDetails.SSBGSeq;
        //            }
        //        }
        //    }

        //}

        private bool ValidateForm()
        {
            bool isValid = true;

            if (FormType == Consts.LiheAllDetails.strMainType)
            {
                if (String.IsNullOrEmpty(txtHierarchy.Text.Trim()))
                {
                    _errorProvider.SetError(PbHierarchies, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHier.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtHierachydesc, null);
                }

                if (String.IsNullOrEmpty(txtYear.Text))
                {
                    _errorProvider.SetError(txtYear, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblYear.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtYear, null);
                }

                if (dtpGPFrom.Checked.Equals(true) && dtpGPTo.Checked.Equals(true))
                {
                    if (!string.IsNullOrEmpty(dtpGPFrom.Text) && (!string.IsNullOrEmpty(dtpGPTo.Text)))
                    {
                        if (Convert.ToDateTime(dtpGPFrom.Text) >= Convert.ToDateTime(dtpGPTo.Text))
                        {
                            _errorProvider.SetError(dtpGPTo, "It should be greater than Grant Period start date ".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtpGPTo, null);
                        }
                    }
                }

            }
            else if (FormType == Consts.LiheAllDetails.strSubType)
            {
                if (String.IsNullOrEmpty(txtTDesc.Text.Trim()))
                {
                    _errorProvider.SetError(txtTDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtTDesc, null);
                }
            }
            else if (FormType == "Questions")
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

            //if (String.IsNullOrEmpty(txtGroupDesc.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtGroupDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGroupDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}

            //else
            //{
            //    _errorProvider.SetError(txtGroupDesc, null);
            //}

            //}
            //else
            //{
            //    if (((ListItem)cmbQuestions.SelectedItem).Value.ToString() == "0")
            //    {
            //        _errorProvider.SetError(cmbQuestions, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQuestion.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(cmbQuestions, null);
            //    }
            //    if (cmbMemAccess.Items.Count > 0)
            //    {
            //        if (((ListItem)cmbMemAccess.SelectedItem).Value.ToString() == "0")
            //        {
            //            _errorProvider.SetError(cmbMemAccess, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMemAccess.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //        {
            //            _errorProvider.SetError(cmbMemAccess, null);
            //        }
            //    }
            //    if (lblResponse.Visible == true)
            //    {
            //        if (((ListItem)cmbResponce.SelectedItem).Value.ToString() == "0")
            //        {
            //            _errorProvider.SetError(cmbResponce, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResponse.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //        {
            //            _errorProvider.SetError(cmbResponce, null);
            //        }
            //    }
            //    if (labelNResp.Visible == true)
            //    {
            //        if (String.IsNullOrEmpty(txtLessthan.Text) && String.IsNullOrEmpty(txtGreaterthan.Text) && String.IsNullOrEmpty(txtEqualTo.Text))
            //        {
            //            _errorProvider.SetError(labelNResp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), labelNResp.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //        {
            //            string strLessthan = txtLessthan.Text == string.Empty ? "0" : txtLessthan.Text;
            //            string strGreaterthan = txtGreaterthan.Text == string.Empty ? "0" : txtGreaterthan.Text;
            //            string strEqual = txtEqualTo.Text == string.Empty ? "0" : txtEqualTo.Text;
            //            if ((Convert.ToDouble(strLessthan) < 1) && (Convert.ToDouble(strGreaterthan) < 1) && (Convert.ToDouble(strEqual) < 1))
            //            {
            //                _errorProvider.SetError(labelNResp, Consts.Messages.Greaterthanzzero);
            //                isValid = false;
            //            }
            //            else
            //            {
            //                _errorProvider.SetError(labelNResp, null);

            //            }
            //        }

            //    }

            //}

            return (isValid);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SSBGPARAMEntity Entity = new SSBGPARAMEntity();
                string strMsg = string.Empty;

                string strHierarchy = txtHierarchy.Text;
                Entity.SSBGAgency = strHierarchy.Substring(0, 2);
                Entity.SSBGDept = strHierarchy.Substring(3, 2);
                Entity.SSBGProgram = strHierarchy.Substring(6, 2);
                if (Mode == "Add") { Entity.SSBGYear = txtYear.Text; Entity.SSBGSeq = "1"; }
                else if (Mode == "Edit")
                {
                    Entity.SSBGYear = txtYear.Text;
                    Entity.SSBGSeq = txtSeq.Text;
                }

                //Entity.SSBGRPFrom = dtpRPFrom.Value.ToShortDateString();
                //Entity.SSBGRPTo = dtpRPTo.Value.ToShortDateString();
                if(dtpGPFrom.Checked)
                    Entity.SSBGGPFrom = dtpGPFrom.Value.ToShortDateString();
                if(dtpGPTo.Checked)
                    Entity.SSBGGPTo = dtpGPTo.Value.ToShortDateString();
                if(dtpRepDate.Checked)
                    Entity.SSBGRPDate = dtpRepDate.Value.ToShortDateString();
                Entity.SSBGBudget = txtBudget.Text.Trim();
                Entity.SSBGAward = txtAward.Text.Trim();
                Entity.SSBGDesc = txtGroupDesc.Text.Trim();
                Entity.SSBGProgDesc = txtProgDesc.Text.Trim();
                Entity.SSBGCntArea1 = txtCntArea1.Text.Trim();
                Entity.SSBGCntArea2 = txtCntArea2.Text.Trim();
                Entity.SSBGCntArea3 = txtCntArea3.Text.Trim();
                Entity.SSBGCntArea4 = txtCntArea4.Text.Trim();
                Entity.SSBGCntArea5 = txtCntArea5.Text.Trim();

                if (chkGrp1.Checked) Entity.SSBGCntArea1_Chk = "1"; else Entity.SSBGCntArea1_Chk = "0";
                if (chkGrp2.Checked) Entity.SSBGCntArea2_Chk = "1"; else Entity.SSBGCntArea2_Chk = "0";
                if (chkGrp3.Checked) Entity.SSBGCntArea3_Chk = "1"; else Entity.SSBGCntArea3_Chk = "0";
                if (chkGrp4.Checked) Entity.SSBGCntArea4_Chk = "1"; else Entity.SSBGCntArea4_Chk = "0";
                if (chkGrp5.Checked) Entity.SSBGCntArea5_Chk = "1"; else Entity.SSBGCntArea5_Chk = "0";

                if (chkGrp1.Checked) Entity.SSBGCntArea1_Val = ((ListItem)cmbGrp1.SelectedItem).Value.ToString();
                if (chkGrp2.Checked) Entity.SSBGCntArea2_Val = ((ListItem)cmbGrp2.SelectedItem).Value.ToString();
                if (chkGrp3.Checked) Entity.SSBGCntArea3_Val = ((ListItem)cmbGrp3.SelectedItem).Value.ToString();
                if (chkGrp4.Checked) Entity.SSBGCntArea4_Val = ((ListItem)cmbGrp4.SelectedItem).Value.ToString();
                if (chkGrp5.Checked) Entity.SSBGCntArea5_Val = ((ListItem)cmbGrp5.SelectedItem).Value.ToString();

                if (((ListItem)cmbGrp1.SelectedItem).Value.ToString() == "Q") Entity.SSBGCust1 = ((ListItem)cmbCust1.SelectedItem).Value.ToString();
                if (((ListItem)cmbGrp2.SelectedItem).Value.ToString() == "Q") Entity.SSBGCust2 = ((ListItem)cmbCust2.SelectedItem).Value.ToString();
                if (((ListItem)cmbGrp3.SelectedItem).Value.ToString() == "Q") Entity.SSBGCust3 = ((ListItem)cmbCust3.SelectedItem).Value.ToString();
                if (((ListItem)cmbGrp4.SelectedItem).Value.ToString() == "Q") Entity.SSBGCust4 = ((ListItem)cmbCust4.SelectedItem).Value.ToString();
                if (((ListItem)cmbGrp5.SelectedItem).Value.ToString() == "Q") Entity.SSBGCust5 = ((ListItem)cmbCust5.SelectedItem).Value.ToString();

                Entity.AddOperator = BaseForm.UserID;
                Entity.LstcOperator = BaseForm.UserID;
                Entity.Mode = Mode;

                if (_model.CaseSumData.InsertUpdateDelSSBGParams(Entity, out strMsg))
                {
                    //btnCancel.Visible = false; btnSave.Visible = false;
                    //pnlGroup.Enabled = false;
                    SSBGParams_Control SSBGControl = BaseForm.GetBaseUserControl() as SSBGParams_Control;
                    if (SSBGControl != null)
                    {
                        if(Mode == "Add")
                            strSeqCode = strMsg;
                        SSBGControl.RefreshGrid(strSeqCode);
                        strgroupcode = txtYear.Text;
                        
                        this.DialogResult = DialogResult.OK;
                    }
                    this.Close();

                    if (Mode == "Add") AlertBox.Show("Saved Successfully");
                    else AlertBox.Show("Updated Successfully");
                }


            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Master", string.Empty, "D", string.Empty);
            addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            addForm.StartPosition = FormStartPosition.CenterScreen;
            addForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

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
                        txtYear.Text =  strCode;
                        HiearchyCode = row.Agency + row.Dept + row.Prog;
                    }
                    else
                    {
                        AlertBox.Show("****** Hierachy Not allowed", MessageBoxIcon.Warning);
                    }
                }
            }

        }


        private void btnTCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool TValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtTDesc.Text.Trim()))
            {
                _errorProvider.SetError(txtTDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTDesc, null);
            }

            return (isValid);
        }


        private void btnTSave_Click(object sender, EventArgs e)
        {
            if (TValidateForm())
            {
                if (FormType == Consts.LiheAllDetails.strSubType)
                {
                    SSBGTYPESEntity Entity = new SSBGTYPESEntity();
                    string strMsg = string.Empty;

                    //string strHierarchy = txtHierarchy.Text;
                    //Entity.SSBGAgency = strHierarchy.Substring(0, 2);
                    //Entity.SSBGDept = strHierarchy.Substring(3, 2);
                    //Entity.SSBGProgram = strHierarchy.Substring(6, 2);
                    Entity.SBGTID = strSeqCode;
                    if (Mode == "Add") { Entity.SBGTCode = "1"; }
                    else if (Mode == "Edit")
                    {
                        //Entity.SSBGYear = ((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.Trim();
                        Entity.SBGTCode = txtCode.Text;
                    }
                    Entity.SBGTCodeSeq = "0";
                    //Entity.SSBGRPFrom = dtpRPFrom.Value.ToShortDateString();
                    //Entity.SSBGRPTo = dtpRPTo.Value.ToShortDateString();
                    Entity.SBGTDesc = txtTDesc.Text.Trim();
                    Entity.SBGTCntlOblig = txtCntlOblig.Text.Trim();
                    Entity.SBGTMemAccess = ((ListItem)cmbCntType.SelectedItem).Value.ToString();
                    //if (chkC1.Checked) Entity.SBGT_CNTAREA1 = "1"; else Entity.SBGT_CNTAREA1 = "0";
                    //if (chkC2.Checked) Entity.SBGT_CNTAREA2 = "1"; else Entity.SBGT_CNTAREA2 = "0";
                    //if (chkC3.Checked) Entity.SBGT_CNTAREA3 = "1"; else Entity.SBGT_CNTAREA3 = "0";
                    //if (chkC4.Checked) Entity.SBGT_CNTAREA4 = "1"; else Entity.SBGT_CNTAREA4 = "0";
                    //if (chkC5.Checked) Entity.SBGT_CNTAREA5 = "1"; else Entity.SBGT_CNTAREA5 = "0";

                    //if (chkC1.Checked) Entity.SBGT_CNTAREA1_VAL = ((ListItem)cmbC1.SelectedItem).Value.ToString();
                    //if (chkC2.Checked) Entity.SBGT_CNTAREA2_VAL = ((ListItem)cmbC2.SelectedItem).Value.ToString();
                    //if (chkC3.Checked) Entity.SBGT_CNTAREA3_VAL = ((ListItem)cmbC3.SelectedItem).Value.ToString();
                    //if (chkC4.Checked) Entity.SBGT_CNTAREA4_VAL = ((ListItem)cmbC4.SelectedItem).Value.ToString();
                    //if (chkC5.Checked) Entity.SBGT_CNTAREA5_VAL = ((ListItem)cmbC5.SelectedItem).Value.ToString();
                    
                    //Entity.SSBGRPDate = dtpRepDate.Value.ToShortDateString();
                    //Entity.SSBGBudget = txtBudget.Text.Trim();
                    //Entity.SSBGAward = txtAward.Text.Trim();
                    //Entity.SSBGDesc = txtGroupDesc.Text.Trim();
                    Entity.AddOperator = BaseForm.UserID;
                    Entity.LstcOperator = BaseForm.UserID;
                    Entity.Mode = Mode;
                    Entity.Type = "Types";

                    if (_model.CaseSumData.InsertUpdateDelSSBGTypes(Entity, out strMsg))
                    {
                        //btnTCancel.Visible = false; btnTSave.Visible = false;
                        //pnlCntltypes.Enabled = false;
                        SSBGParams_Control SSBGControl = BaseForm.GetBaseUserControl() as SSBGParams_Control;
                        if (SSBGControl != null)
                        {
                            SSBGControl.RefreshGrid(string.Empty);
                            TypeCode = strMsg;
                            strSeqCode = strSeqCode;
                            this.DialogResult = DialogResult.OK;
                        }
                        this.Close();

                        if (Mode == "Add") AlertBox.Show("Saved Successfully");
                        else AlertBox.Show("Updated Successfully");
                    }
                    int strCntlTypeIndex = 0;
                    strCntlTypeIndex = int.Parse(Entity.SBGTCode);
                }
                else if (FormType == "GOAL")
                {
                    SSBGGOALSEntity Entity = new SSBGGOALSEntity();
                    string strMsg = string.Empty;

                    //string strHierarchy = txtHierarchy.Text;
                    //Entity.SSBGAgency = strHierarchy.Substring(0, 2);
                    //Entity.SSBGDept = strHierarchy.Substring(3, 2);
                    //Entity.SSBGProgram = strHierarchy.Substring(6, 2);
                    Entity.SBGGID = strSeqCode;
                    if (Mode == "Add") { Entity.SBGGCode = "1"; }
                    else if (Mode == "Edit")
                    {
                        //Entity.SSBGYear = ((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.Trim();
                        Entity.SBGGCode = txtCode.Text;
                    }
                    Entity.SBGGSeq = "0";
                    //Entity.SSBGRPFrom = dtpRPFrom.Value.ToShortDateString();
                    //Entity.SSBGRPTo = dtpRPTo.Value.ToShortDateString();
                    Entity.SBGGDesc = txtTDesc.Text.Trim();
                    Entity.SBGGCountType = ((Captain.Common.Utilities.ListItem)cmbCntType.SelectedItem).Value.ToString().Trim();
                    Entity.Type2 = ((Captain.Common.Utilities.ListItem)cmbType2.SelectedItem).Value.ToString().Trim();
                    //Entity.SBGTCntlOblig = txtCntlOblig.Text.Trim();
                    //Entity.SSBGRPDate = dtpRepDate.Value.ToShortDateString();
                    //Entity.SSBGBudget = txtBudget.Text.Trim();
                    //Entity.SSBGAward = txtAward.Text.Trim();
                    //Entity.SSBGDesc = txtGroupDesc.Text.Trim();
                    Entity.AddOperator = BaseForm.UserID;
                    Entity.LstcOperator = BaseForm.UserID;
                    Entity.Mode = Mode;
                    Entity.Type = "GOAL";

                    if (_model.CaseSumData.InsertUpdateDelSSBGGoals(Entity, out strMsg))
                    {
                        //btnTCancel.Visible = false; btnTSave.Visible = false;
                        //pnlCntltypes.Enabled = false;
                        SSBGParams_Control SSBGControl = BaseForm.GetBaseUserControl() as SSBGParams_Control;
                        if (SSBGControl != null)
                        {
                            SSBGControl.RefreshGoals();
                            TypeCode = strMsg;
                            strSeqCode = strSeqCode;
                            this.DialogResult = DialogResult.OK;
                        }
                        this.Close();

                        if (Mode == "Add") AlertBox.Show("Saved Successfully");
                        else AlertBox.Show("Updated Successfully");
                    }
                }
                else if (FormType == "OBJECT")
                {
                    SSBGGOALSEntity Entity = new SSBGGOALSEntity();
                    string strMsg = string.Empty;

                    //string strHierarchy = txtHierarchy.Text;
                    //Entity.SSBGAgency = strHierarchy.Substring(0, 2);
                    //Entity.SSBGDept = strHierarchy.Substring(3, 2);
                    //Entity.SSBGProgram = strHierarchy.Substring(6, 2);
                    Entity.SBGGID = strSeqCode;
                    Entity.SBGGCode = TypeCode;
                    if (Mode == "Add") { Entity.SBGGSeq = "1"; }
                    else if (Mode == "Edit")
                    {
                        //Entity.SSBGYear = ((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.Trim();
                        Entity.SBGGSeq = TypeSeq;
                    }
                    //Entity.SBGGSeq = "0";
                    //Entity.SSBGRPFrom = dtpRPFrom.Value.ToShortDateString();
                    //Entity.SSBGRPTo = dtpRPTo.Value.ToShortDateString();
                    Entity.SBGGDesc = txtTDesc.Text.Trim();
                    //Entity.SBGGCountType = ((Captain.Common.Utilities.ListItem)cmbCntType.SelectedItem).Value.ToString().Trim();
                    //Entity.Type2 = ((Captain.Common.Utilities.ListItem)cmbType2.SelectedItem).Value.ToString().Trim();
                    ////Entity.SBGTCntlOblig = txtCntlOblig.Text.Trim();
                    Entity.AddOperator = BaseForm.UserID;
                    Entity.LstcOperator = BaseForm.UserID;
                    Entity.Mode = Mode;
                    Entity.Type = "OBJECT";

                    if (_model.CaseSumData.InsertUpdateDelSSBGGoals(Entity, out strMsg))
                    {
                        //btnTCancel.Visible = false; btnTSave.Visible = false;
                        //pnlCntltypes.Enabled = false;
                        SSBGParams_Control SSBGControl = BaseForm.GetBaseUserControl() as SSBGParams_Control;
                        if (SSBGControl != null)
                        {
                            SSBGControl.RefreshGoals();
                            TypeCode = strMsg;
                            strSeqCode = strSeqCode;
                            this.DialogResult = DialogResult.OK;
                        }
                        this.Close();

                        if (Mode == "Add") AlertBox.Show("Saved Successfully");
                        else AlertBox.Show("Updated Successfully");
                    }
                }

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

        private void btnQCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SSBGTYPESEntity Entity = new SSBGTYPESEntity();
                string strMsg = string.Empty;

                Entity.SBGTID = strSeqCode;
                Entity.SBGTCode = TypeCode;
                if (Mode == "Add") { Entity.SBGTCodeSeq = "1"; }
                else if (Mode == "Edit")
                {
                    //Entity.SSBGYear = ((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.Trim();
                    Entity.SBGTCodeSeq = TypeSeq;
                }
                Entity.SBGTDesc = txtTDesc.Text.Trim();
                Entity.SBGTCntlOblig = txtCntlOblig.Text.Trim();

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
                    SSBGParams_Control SSBGControl = BaseForm.GetBaseUserControl() as SSBGParams_Control;
                    if (SSBGControl != null)
                    {
                        //SSBGControl.RefreshQuestions();
                        TypeCode = strMsg;
                        strSeqCode = strSeqCode;
                        strQuestionCode = ((ListItem)cmbQuestions.SelectedItem).Value.ToString();
                        this.DialogResult = DialogResult.OK;
                    }
                    this.Close();

                    if (Mode == "Add") AlertBox.Show("Saved Successfully");
                    else AlertBox.Show("Updated Successfully");
                }
            }
        }

        private void btnMonths_Click(object sender, EventArgs e)
        {
            SSBGMonths_Form EditForm = new SSBGMonths_Form(BaseForm, "Edit", Consts.LiheAllDetails.strMainType, strSeqCode, strgroupcode, TypeCode, strHierachycode, HiearchyDesc, Privileges);
            //EditForm.FormClosed += new Form.FormClosedEventHandler(OnSSBGFromClosed);
            EditForm.StartPosition = FormStartPosition.CenterScreen;
            EditForm.ShowDialog();
        }

        private void chkC1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGrp1.Checked == true) cmbGrp1.Enabled = true; else cmbGrp1.Enabled = false;
            if (chkGrp2.Checked == true) cmbGrp2.Enabled = true; else cmbGrp2.Enabled = false;
            if (chkGrp3.Checked == true) cmbGrp3.Enabled = true; else cmbGrp3.Enabled = false;
            if (chkGrp4.Checked == true) cmbGrp4.Enabled = true; else cmbGrp4.Enabled = false;
            if (chkGrp5.Checked == true) cmbGrp5.Enabled = true; else cmbGrp5.Enabled = false;
                
        }

        private void cmbGrp5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)cmbGrp1.SelectedItem).Value.ToString().Trim() == "Q")
            { cmbCust1.Enabled = true; }
            else { cmbCust1.SelectedIndex = 0; cmbCust1.Enabled = false; } 
            if (((Captain.Common.Utilities.ListItem)cmbGrp2.SelectedItem).Value.ToString().Trim() == "Q")
            { cmbCust2.Enabled = true;  }
            else { cmbCust2.SelectedIndex = 0; cmbCust2.Enabled = false; } 
            if (((Captain.Common.Utilities.ListItem)cmbGrp3.SelectedItem).Value.ToString().Trim() == "Q")
            { cmbCust3.Enabled = true;  }
            else { cmbCust3.SelectedIndex = 0; cmbCust3.Enabled = false; } 
            if (((Captain.Common.Utilities.ListItem)cmbGrp4.SelectedItem).Value.ToString().Trim() == "Q")
            { cmbCust4.Enabled = true;  }
            else { cmbCust4.SelectedIndex = 0; cmbCust4.Enabled = false; } 
            if (((Captain.Common.Utilities.ListItem)cmbGrp5.SelectedItem).Value.ToString().Trim() == "Q")
            { cmbCust5.Enabled = true;  }
            else { cmbCust5.SelectedIndex = 0; cmbCust5.Enabled = false; } 
        }

        private void FillCustomQues()
        {
            cmbCust1.Items.Clear();

            List<CustfldsEntity> Cust = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);

            if (Cust.Count > 0)
            {
                cmbCust1.Items.Add(new ListItem(" ","00"));
                cmbCust2.Items.Add(new ListItem(" ", "00"));
                cmbCust3.Items.Add(new ListItem(" ", "00"));
                cmbCust4.Items.Add(new ListItem(" ", "00"));
                cmbCust5.Items.Add(new ListItem(" ", "00"));
                foreach (CustfldsEntity Entity in Cust)
                {
                    if (Entity.Active == "A")
                    {
                        Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(Entity.CustDesc, Entity.CustCode);
                        cmbCust1.Items.Add(li);
                        cmbCust2.Items.Add(li);
                        cmbCust3.Items.Add(li);
                        cmbCust4.Items.Add(li);
                        cmbCust5.Items.Add(li);
                    }
                }
            }
        }
        


    }
}