#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using System.Data.SqlClient;
using Wisej.Web;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SpanishCustomQuestions : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public SpanishCustomQuestions(BaseForm baseForm, PrivilegeEntity privilegeEntity, string strQuestionID, string strType)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();
            propCustomCode = strQuestionID;
            propType = strType;
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            if (Privileges.AddPriv.ToUpper() == "TRUE" || Privileges.ChangePriv.ToUpper() == "TRUE")
            {
                Pb_Edit_Cust.Enabled = true;
            }
            else
            {
                Pb_Edit_Cust.Enabled = false;
            }
            if (propType == "CUSTOM")
            {
                this.Text = "Spanish Entry for Custom Questions";
                chkActive.Visible = true;
                chkSentPip.Visible = true;
                Pb_Edit_Cust.Location = new Point(502, 6);
                FillCustomQuestions();
            }
            else if (propType == "CASEHIE")
            {
                this.Text = "CASEHIE - Spanish Entry for Programs";
                chkPipselectall.Visible = true;
                Cmbquestions.Visible = false;
                //label17.Visible = false;
                lblResponse.Location = new Point(5, 16);
                lblResponse.Text = "Programs";
                gvwResponses.Location = new Point(5, 32);
                gvwResponses.Size = new Size(525, 240);
                Cmbquestions.Size = new Size(416, 21);
                lblDesc.Visible = false;
                lblSpDesc.Visible = false;
                //label6.Visible = false;
                txtQDescrption.Visible = false;
                TxtQuesDesc.Visible = false;
                lblQes.Visible = false;
                chkActive.Visible = false;
                chkSentPip.Visible = false;
                FillCASEHIE();
            }
            else if (propType == "CAMAST")
            {
                this.Text = "CAMAST - Spanish entry for Services Inquired";
                chkPipselectall.Visible = true;
                Cmbquestions.Visible = false;
                //label17.Visible = false;
                lblResponse.Location = new Point(5, 16);
                lblResponse.Text = "Services";
                gvwResponses.Location = new Point(5, 32);
                gvwResponses.Size = new Size(525, 240);
                Cmbquestions.Size = new Size(416, 21);
                lblDesc.Visible = false;
                lblSpDesc.Visible = false;
                // label6.Visible = false;
                txtQDescrption.Visible = false;
                TxtQuesDesc.Visible = false;
                lblQes.Visible = false;
                chkActive.Visible = false;
                chkSentPip.Visible = false;
                FillCAMAST();
            }
            else
            {
                this.Text = "Spanish Entry for Agency Tables";
                lblResponse.Location = new Point(5, 60);
                gvwResponses.Location = new Point(5, 85);
                gvwResponses.Size = new Size(525, 190);
                Cmbquestions.Size = new Size(416, 21);
                lblDesc.Visible = false;
                lblSpDesc.Visible = false;
                lblResponse.Text = "Options";
                // label6.Visible = false;
                txtQDescrption.Visible = false;
                TxtQuesDesc.Visible = false;
                lblQes.Text = "Agency Table";
                chkActive.Visible = false;
                chkSentPip.Visible = false;
                gvtCheckPIP.Visible = false;
                gvtCheckStatus.Visible = false;
                gvtDesc.Width = 190;
                gvtSDesc.Width = 275;
                Fill_AgyType_Combo();
            }

            CommonFunctions.SetComboBoxValue(Cmbquestions, strQuestionID);
            // Cmbquestions_SelectedIndexChanged(Cmbquestions, new EventArgs());

        }

        #region Properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string propType { get; set; }

        public string propCustomCode { get; set; }

        public PrivilegeEntity Privileges { get; set; }


        #endregion


        private void FillCustomQuestions()
        {
            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            List<CustfldsEntity> Cust = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);
            if (Cust.Count > 0)
            {
                Cust = Cust.FindAll(u => u.MemAccess == "A" || u.MemAccess == "H");
                Cmbquestions.Items.Clear();
                gvwResponses.Rows.Clear();
                // Cmbquestions.Items.Insert(0, new ListItem("Select One", "0"));
                foreach (CustfldsEntity Entity in Cust)
                {
                    ListItem li = new ListItem(Entity.CustDesc, Entity.CustCode, Entity.custSpanishDesc, Entity.RespType, Entity.custPIPActive, Entity.custSendtoPip);
                    Cmbquestions.Items.Add(li);
                }

            }
            gvtCheckPIP.Visible = false;
            gvtCheckStatus.Visible = false;
            gvtDesc.Width = 190;
            gvtSDesc.Width = 275;
            List<CustRespEntity> CustResp = _model.FieldControls.GetCustRespByCustCode(propCustomCode, string.Empty);
            if (CustResp.Count > 0)
            {
                int intIndex;
                foreach (CustRespEntity RespEntity in CustResp)
                {
                    intIndex = gvwResponses.Rows.Add(RespEntity.DescCode, RespEntity.RespDesc, RespEntity.RespSpanishDesc, RespEntity.RespSeq, RespEntity.ResoCode);
                    gvwResponses.Rows[intIndex].Tag = RespEntity;
                }
            }
            if (Cmbquestions.Items.Count > 0)
                Cmbquestions.SelectedIndex = 0;
            Cmbquestions_SelectedIndexChanged(Cmbquestions, new EventArgs());
            Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
        }


        private void FillCASEHIE()
        {
            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            Cmbquestions.Items.Clear();
            gvwResponses.Rows.Clear();

            List<HierarchyEntity> hierachyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyDepartment(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, "PROGRAM");

            if (hierachyEntity.Count > 0)
            {
                int intIndex;
                foreach (HierarchyEntity HieEntity in hierachyEntity)
                {
                    intIndex = gvwResponses.Rows.Add(HieEntity.Code, HieEntity.HirarchyName, HieEntity.SpanishName, HieEntity.Code, HieEntity.Code, (HieEntity.SendtoPIP == "Y" ? true : false), (HieEntity.PIPActive == "Y" ? true : false));
                    gvwResponses.Rows[intIndex].Tag = HieEntity;
                }
            }

            Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
        }

        private void FillCAMAST()
        {
            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            Cmbquestions.Items.Clear();
            gvwResponses.Rows.Clear();

            List<CAMASTEntity> CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            if (CAList.Count > 0)
            {
                int intIndex;
                foreach (CAMASTEntity CAMASTEntity in CAList)
                {
                    intIndex = gvwResponses.Rows.Add(CAMASTEntity.Code, CAMASTEntity.Desc, CAMASTEntity.SpanishDesc, CAMASTEntity.Code, CAMASTEntity.Code, (CAMASTEntity.SendtoPIP == "Y" ? true : false), (CAMASTEntity.PIPActive == "Y" ? true : false));
                    gvwResponses.Rows[intIndex].Tag = CAMASTEntity;
                    if (CAMASTEntity.Active.ToUpper() != "TRUE")
                    {
                        //gvwResponses.Rows[intIndex].DefaultCellStyle.ForeColor  = Color.Red;
                    }
                }
            }
            Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
        }



        private void Fill_AgyType_Combo()
        {

            DataSet ds = Captain.DatabaseLayer.AgyTab.GetAgencyTableByApp("**");
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow[] foundRows;


                foundRows = dt.Select(null, "AGY_TYPE ASC");
                //else
                //    foundRows = dt.Select(null, "AGY_DESC ASC");

                Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
                Cmbquestions.Items.Clear();
                gvwResponses.Rows.Clear();
                //Cmbquestions.Items.Insert(0, new ListItem("Select One", "0"));
                string Tmp_Desc = string.Empty;

                foreach (DataRow dr in foundRows)
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-50}", dr["AGY_DESC"].ToString().Trim()) + String.Format("{0,8}", " - " + dr["AGY_TYPE"].ToString());

                    Cmbquestions.Items.Add(new ListItem(Tmp_Desc, dr["AGY_TYPE"].ToString()));
                }

                if (propCustomCode != string.Empty)
                {
                    List<CommonEntity> lookupagytabs = _model.lookupDataAccess.GetAgyTabs("LEAGY", propCustomCode, string.Empty);
                    if (lookupagytabs.Count > 0)
                    {
                        int intIndex;
                        foreach (CommonEntity agytabsEntity in lookupagytabs)
                        {
                            intIndex = gvwResponses.Rows.Add(agytabsEntity.Code, agytabsEntity.Desc, agytabsEntity.SDesc, agytabsEntity.Code, agytabsEntity.Code, agytabsEntity.Code);
                            gvwResponses.Rows[intIndex].Tag = agytabsEntity;
                        }
                    }
                }

                if (Cmbquestions.Items.Count > 0)
                    Cmbquestions.SelectedIndex = 0;
                Cmbquestions_SelectedIndexChanged(Cmbquestions, new EventArgs());

                Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
            }
        }


        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(Cmbquestions, null);
            _errorProvider.SetError(TxtQuesDesc, null);

            if (propType == "CUSTOM")
            {
                if (Btn_Cancel.Text == "Cancel")
                {
                    Btn_Save.Enabled = TxtQuesDesc.Enabled = chkPipselectall.Enabled = gvwResponses.Enabled = false; chkSentPip.Enabled = false; chkActive.Enabled = false; Pb_Edit_Cust.Visible = true;
                    Btn_Cancel.Text = "Close"; btnMoveData.Enabled = true; Cmbquestions.Enabled = true;
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                if (Btn_Cancel.Text == "Cancel")
                {
                    Btn_Save.Enabled = TxtQuesDesc.Enabled = gvwResponses.Enabled = false; Pb_Edit_Cust.Visible = true;
                    Btn_Cancel.Text = "Close"; btnMoveData.Enabled = true; Cmbquestions.Enabled = true;
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (ValidateCustControls())
            {
                if (propType == "CUSTOM")
                {
                    CustfldsEntity CustDetails = new CustfldsEntity();

                    CustDetails.UpdateType = "S";
                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.CustCode = ((ListItem)Cmbquestions.SelectedItem).Value.ToString();
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Equalto = "0";
                    CustDetails.Greater = "0";
                    CustDetails.Less = "0";
                    propCustomCode = ((ListItem)Cmbquestions.SelectedItem).Value.ToString();
                    CustDetails.Active = chkSentPip.Checked == true ? "Y" : "N";
                    CustDetails.FutureDate = chkActive.Checked == true ? "A" : "I";

                    string New_CUST_Code_Code = propCustomCode;

                    if (_model.FieldControls.UpdateCUSTFLDS(CustDetails, out New_CUST_Code_Code))
                    {
                        if (((ListItem)Cmbquestions.SelectedItem).ValueDisplayCode.ToString() == "D" || ((ListItem)Cmbquestions.SelectedItem).ValueDisplayCode.ToString() == "C")
                        {
                            foreach (DataGridViewRow item in gvwResponses.Rows)
                            {
                                CustRespEntity custrespentity = item.Tag as CustRespEntity;
                                if (custrespentity != null)
                                {
                                    string strDesc = item.Cells["gvtSDesc"].Value == null ? string.Empty : item.Cells["gvtSDesc"].Value.ToString();
                                    _model.FieldControls.UpdateCUSTRESPSingle("S", custrespentity.ScrCode, custrespentity.ResoCode, custrespentity.RespSeq, strDesc, custrespentity.DescCode, BaseForm.UserID, BaseForm.UserID, string.Empty);
                                }
                            }


                        }

                        propCustomCode = "";
                        txtQDescrption.Text = string.Empty;
                        TxtQuesDesc.Text = string.Empty;
                        chkActive.Checked = false;
                        chkSentPip.Checked = false;
                        FillCustomQuestions();
                        //if (Cmbquestions.Items.Count > 0)
                        //    Cmbquestions.SelectedIndex = 0;
                        //Cmbquestions_SelectedIndexChanged(Cmbquestions, new EventArgs());
                        Btn_Save.Enabled = TxtQuesDesc.Enabled = gvwResponses.Enabled = chkPipselectall.Enabled = false; Pb_Edit_Cust.Visible = true; Cmbquestions.Enabled = true; btnMoveData.Enabled = true; chkSentPip.Enabled = false; chkActive.Enabled = false; Btn_Cancel.Text = "Close";
                    }
                }
                else if (propType == "CASEHIE")
                {
                    foreach (DataGridViewRow item in gvwResponses.Rows)
                    {

                        HierarchyEntity hierchyEntity = item.Tag as HierarchyEntity;

                        string strDesc = item.Cells["gvtSDesc"].Value == null ? string.Empty : item.Cells["gvtSDesc"].Value.ToString();
                        string strSendPIP = item.Cells["gvtCheckPIP"].Value == null ? string.Empty : item.Cells["gvtCheckPIP"].Value.ToString();
                        string strPIPActive = item.Cells["gvtCheckStatus"].Value == null ? string.Empty : item.Cells["gvtCheckStatus"].Value.ToString();
                        // if (!string.IsNullOrEmpty(strDesc))
                        // {
                        hierchyEntity.Mode = "PIP";
                        hierchyEntity.HirarchyName = strDesc;
                        hierchyEntity.Intake = strSendPIP.ToUpper() == "TRUE" ? "Y" : "N";
                        hierchyEntity.HIERepresentation = strPIPActive.ToUpper() == "TRUE" ? "Y" : "N";
                        _model.HierarchyAndPrograms.InsertUpdateHierarchy(hierchyEntity);
                        // }

                    }
                    propCustomCode = string.Empty;
                    propCustomCode = "";
                    txtQDescrption.Text = string.Empty;
                    TxtQuesDesc.Text = string.Empty;
                    FillCASEHIE();

                    Btn_Save.Enabled = gvwResponses.Enabled = false; Pb_Edit_Cust.Visible = true; Cmbquestions.Enabled = false; btnMoveData.Enabled = true; Btn_Cancel.Text = "Close";

                }
                else if (propType == "CAMAST")
                {
                    foreach (DataGridViewRow item in gvwResponses.Rows)
                    {

                        CAMASTEntity camastEntity = item.Tag as CAMASTEntity;

                        string strDesc = item.Cells["gvtSDesc"].Value == null ? string.Empty : item.Cells["gvtSDesc"].Value.ToString();
                        string strSendPIP = item.Cells["gvtCheckPIP"].Value == null ? string.Empty : item.Cells["gvtCheckPIP"].Value.ToString();
                        string strPIPActive = item.Cells["gvtCheckStatus"].Value == null ? string.Empty : item.Cells["gvtCheckStatus"].Value.ToString();
                        //  if (!string.IsNullOrEmpty(strDesc))
                        // {
                        camastEntity.Mode = "PIP";
                        camastEntity.Desc = strDesc;
                        camastEntity.AutoPost = strSendPIP.ToUpper() == "TRUE" ? "Y" : "N";
                        camastEntity.Active = strPIPActive.ToUpper() == "TRUE" ? "Y" : "N";
                        _model.SPAdminData.InsertCaMAST(camastEntity);

                        // }
                    }
                    propCustomCode = string.Empty;
                    propCustomCode = "";
                    txtQDescrption.Text = string.Empty;
                    TxtQuesDesc.Text = string.Empty;
                    FillCAMAST();

                    Btn_Save.Enabled = gvwResponses.Enabled = false; Pb_Edit_Cust.Visible = true; Cmbquestions.Enabled = false; btnMoveData.Enabled = true; Btn_Cancel.Text = "Close";
                }
                else
                {
                    foreach (DataGridViewRow item in gvwResponses.Rows)
                    {

                        CommonEntity agytabsEntity = item.Tag as CommonEntity;

                        string strDesc = item.Cells["gvtSDesc"].Value == null ? string.Empty : item.Cells["gvtSDesc"].Value.ToString();
                        _model.Agytab.InsertUpdateAGYTABS("SPANISH", agytabsEntity.AgyCode, agytabsEntity.Code, string.Empty, string.Empty, string.Empty, string.Empty, strDesc);

                    }
                    propCustomCode = string.Empty;
                    propCustomCode = "";
                    txtQDescrption.Text = string.Empty;
                    TxtQuesDesc.Text = string.Empty;
                    Fill_AgyType_Combo();
                    //if (Cmbquestions.Items.Count > 0)
                    //    Cmbquestions.SelectedIndex = 0;
                    Btn_Save.Enabled = gvwResponses.Enabled = false; Pb_Edit_Cust.Visible = true; Cmbquestions.Enabled = true; btnMoveData.Enabled = true; Btn_Cancel.Text = "Close";

                }
            }


        }

        private void Pb_Edit_Cust_Click(object sender, EventArgs e)
        {
            btnMoveData.Enabled = false; Cmbquestions.Enabled = false; chkSentPip.Enabled = chkActive.Enabled = TxtQuesDesc.Enabled = Btn_Save.Enabled = chkPipselectall.Enabled = true; Pb_Edit_Cust.Visible = false; gvwResponses.Enabled = true; Btn_Cancel.Text = "Cancel";
        }

        private void Cmbquestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmbquestions.Items.Count > 0)
            {
                _errorProvider.SetError(Cmbquestions, null);
                _errorProvider.SetError(TxtQuesDesc, null);

                if (propType == "CUSTOM")
                {
                    if (((ListItem)Cmbquestions.SelectedItem).Value.ToString() != string.Empty)
                    {
                        if (((ListItem)Cmbquestions.SelectedItem).Value.ToString() != "0")
                        {
                            if (Pb_Edit_Cust.Visible == false)
                            {
                                chkSentPip.Enabled = true;
                                chkActive.Enabled = true;
                            }
                            txtQDescrption.Text = ((ListItem)Cmbquestions.SelectedItem).Text.ToString();
                            TxtQuesDesc.Text = ((ListItem)Cmbquestions.SelectedItem).ID.ToString();

                            chkActive.Checked = ((ListItem)Cmbquestions.SelectedItem).ScreenCode.ToString() == "A" ? true : false;
                            chkSentPip.Checked = ((ListItem)Cmbquestions.SelectedItem).ScreenType.ToString() == "Y" ? true : false;
                            if (CheckCustomQuestions(((ListItem)Cmbquestions.SelectedItem).Value.ToString(), propType) > 0)
                            {
                                if (((ListItem)Cmbquestions.SelectedItem).ScreenType.ToString() == "Y")
                                {
                                    chkSentPip.Enabled = false;
                                }
                                else
                                {
                                    if (Pb_Edit_Cust.Visible == false)
                                        chkSentPip.Enabled = true;
                                }
                            }

                            lblResponse.Visible = false;
                            gvwResponses.Visible = false;
                            if (Pb_Edit_Cust.Visible == false)
                                gvwResponses.Enabled = true;
                            gvwResponses.Rows.Clear();
                            if (((ListItem)Cmbquestions.SelectedItem).ValueDisplayCode.ToString() == "D" || ((ListItem)Cmbquestions.SelectedItem).ValueDisplayCode.ToString() == "C")
                            {
                                List<CustRespEntity> CustResp = _model.FieldControls.GetCustRespByCustCode(((ListItem)Cmbquestions.SelectedItem).Value.ToString(), string.Empty);
                                if (CustResp.Count > 0)
                                {


                                    lblResponse.Visible = true;
                                    gvwResponses.Visible = true;
                                    int intIndex;
                                    foreach (CustRespEntity RespEntity in CustResp)
                                    {
                                        intIndex = gvwResponses.Rows.Add(RespEntity.DescCode, RespEntity.RespDesc, RespEntity.RespSpanishDesc, RespEntity.RespSeq, RespEntity.ResoCode, RespEntity.RespSeq);
                                        gvwResponses.Rows[intIndex].Tag = RespEntity;
                                    }
                                }
                            }
                        }
                        else
                        {
                            txtQDescrption.Text = string.Empty;
                            TxtQuesDesc.Text = string.Empty;
                            gvwResponses.Rows.Clear();
                            gvwResponses.Enabled = false;
                            chkActive.Checked = false;
                            chkSentPip.Checked = false;

                        }
                    }
                }
                else
                {
                    if (((ListItem)Cmbquestions.SelectedItem).Value.ToString() != "0")
                    {
                        gvwResponses.Visible = true;
                        gvwResponses.Rows.Clear();
                        lblAgytabused.Visible = false;
                        List<CommonEntity> lookupagytabs = _model.lookupDataAccess.GetAgyTabspanish("LEAGY", ((ListItem)Cmbquestions.SelectedItem).Value.ToString(), string.Empty);
                        if (lookupagytabs.Count > 0)
                        {
                            LeanAgytabStatusReq(((ListItem)Cmbquestions.SelectedItem).Value.ToString());
                            int intIndex;
                            foreach (CommonEntity agytabsEntity in lookupagytabs)
                            {
                                intIndex = gvwResponses.Rows.Add(agytabsEntity.Code, agytabsEntity.Desc, agytabsEntity.SDesc, agytabsEntity.Code, agytabsEntity.Code, agytabsEntity.Code);
                                gvwResponses.Rows[intIndex].Tag = agytabsEntity;
                            }
                        }

                    }
                    else
                    {
                        gvwResponses.Rows.Clear();
                        gvwResponses.Enabled = false;
                    }

                }
            }
        }


        public void LeanAgytabStatusReq(string strCode)
        {
            switch (strCode)
            {

                case "00002":
                    lblAgytabused.Visible = true;
                    break;
                case "00003":
                    lblAgytabused.Visible = true;
                    break;
                case "00004":
                    lblAgytabused.Visible = true;
                    break;
                case "00007":
                    lblAgytabused.Visible = true;
                    break;
                case "00008":
                    lblAgytabused.Visible = true;
                    break;
                case "00009":
                    lblAgytabused.Visible = true;
                    break;
                case "00015":
                    lblAgytabused.Visible = true;
                    break;
                case "00016":
                    lblAgytabused.Visible = true;
                    break;
                case "00018":
                    lblAgytabused.Visible = true;
                    break;
                case "00019":
                    lblAgytabused.Visible = true;
                    break;
                case "00020":
                    lblAgytabused.Visible = true;
                    break;
                case "00022":
                    lblAgytabused.Visible = true;
                    break;
                case "00025":
                    lblAgytabused.Visible = true;
                    break;
                case "00030":
                    lblAgytabused.Visible = true;
                    break;
                case "00035":
                    lblAgytabused.Visible = true;
                    break;
                case "00036":
                    lblAgytabused.Visible = true;
                    break;
                case "00037":
                    lblAgytabused.Visible = true;
                    break;
                case "00039":
                    lblAgytabused.Visible = true;
                    break;
                case "00352":
                    lblAgytabused.Visible = true;
                    break;
                case "00353":
                    lblAgytabused.Visible = true;
                    break;
                case "00525":
                    lblAgytabused.Visible = true;
                    break;
                case "03003":
                    lblAgytabused.Visible = true;
                    break;
                case "03006":
                    lblAgytabused.Visible = true;
                    break;
                case "03259":
                    lblAgytabused.Visible = true;
                    break;

                case "08001":
                    lblAgytabused.Visible = true;
                    break;





            }
        }



        private bool ValidateCustControls()
        {
            bool isValid = true;

            if (propType == "CUSTOM")
            {
                if ((Cmbquestions.SelectedItem == null || ((ListItem)Cmbquestions.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(Cmbquestions, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQes.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(Cmbquestions, null);
                }


                //if ((String.IsNullOrEmpty(TxtQuesDesc.Text)))
                //{
                //    _errorProvider.SetError(TxtQuesDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label9.Text.Replace(Consts.Common.Colon, string.Empty)));
                //    isValid = false;
                //}
                //else
                //    _errorProvider.SetError(TxtQuesDesc, null);
            }
            else if (propType == "CAMAST")
            { }
            else if (propType == "CASEHIE")
            { }
            else
            {
                if ((Cmbquestions.SelectedItem == null || ((ListItem)Cmbquestions.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(Cmbquestions, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQes.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(Cmbquestions, null);
                }
            }

            return isValid;
        }

        private void btnMoveData_Click(object sender, EventArgs e)
        {
            if (propType == "CUSTOM")
            {
                List<CustfldsEntity> Cust = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);
                if (Cust.Count > 0)
                {
                    Cust = Cust.FindAll(u => (u.MemAccess == "A" || u.MemAccess == "H") && u.custSendtoPip == "Y");

                    DeleteLeanCustomeQuestions(BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper(), string.Empty);
                    DeleteLeanCustResponses(BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper(), string.Empty);

                    foreach (CustfldsEntity Entity in Cust)
                    {
                        if (InsertLeanCustomeQuestions(Entity.CustCode, Entity.CustSeq, Entity.Active, Entity.CustDesc, Entity.RespType, Entity.MemAccess, Entity.custSpanishDesc, Entity.custPIPActive) > 0)
                        {
                            //ListItem li = new ListItem(Entity.CustDesc, Entity.CustCode, Entity.custSpanishDesc, Entity.RespType, Entity.ScrCode, string.Empty);
                            if ((Entity.RespType.ToString() == "D") || (Entity.RespType.ToString() == "C"))
                            {
                                List<CustRespEntity> CustResp = _model.FieldControls.GetCustRespByCustCode(Entity.CustCode, string.Empty);
                                if (CustResp.Count > 0)
                                {
                                    foreach (CustRespEntity RespEntity in CustResp)
                                    {
                                        InsertLeanCustResponses(Entity.CustCode, RespEntity.RespSeq, RespEntity.RespDesc, RespEntity.DescCode, RespEntity.RespSpanishDesc);

                                    }
                                }
                            }
                        }

                    }
                    AlertBox.Show("Successfully Data Moved in PIP");
                    //CommonFunctions.MessageBoxDisplay("Successfully Data Moved in PIP");
                }


            }
            else if (propType == "CAMAST")
            {
                List<CAMASTEntity> CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                if (CAList.Count > 0)
                {
                    DeleteLeanServices(propType);
                    CAList = CAList.FindAll(u => u.SendtoPIP == "Y");
                    foreach (CAMASTEntity CAMASTEntity in CAList)
                    {
                        InsertLeanServices(CAMASTEntity.Code, CAMASTEntity.Desc, CAMASTEntity.SpanishDesc, propType, CAMASTEntity.PIPActive);
                    }
                    AlertBox.Show("Successfully Data Moved in PIP");
                    //CommonFunctions.MessageBoxDisplay("Successfully Data Moved in PIP");
                }
            }
            else if (propType == "CASEHIE")
            {

                List<HierarchyEntity> hierachyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyDepartment(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, "PROGRAM");

                if (hierachyEntity.Count > 0)
                {
                    hierachyEntity = hierachyEntity.FindAll(u => u.SendtoPIP == "Y");
                    DeleteLeanServices(propType);
                    foreach (HierarchyEntity HieEntity in hierachyEntity)
                    {

                        InsertLeanServices(HieEntity.Code, HieEntity.HirarchyName, HieEntity.SpanishName, propType, HieEntity.PIPActive);
                    }
                    AlertBox.Show("Successfully Data Moved in PIP");
                    //CommonFunctions.MessageBoxDisplay("Successfully Data Moved in PIP");
                }
            }
            else
            {
                List<CommonEntity> lookupagytabs = _model.lookupDataAccess.GetAgyTabspanish("LEAGY", string.Empty, string.Empty);
                if (lookupagytabs.Count > 0)
                {
                    DeleteLeanAgyTabs();
                    foreach (CommonEntity agytabsEntity in lookupagytabs)
                    {
                        InsertLeanAgyTabs(agytabsEntity.AgyCode, agytabsEntity.Code, agytabsEntity.Active, agytabsEntity.Desc, agytabsEntity.Default, agytabsEntity.SDesc);
                    }
                    AlertBox.Show("Successfully Data Moved in PIP");
                    //CommonFunctions.MessageBoxDisplay("Successfully Data Moved in PIP");
                }
            }
        }

        int InsertLeanCustomeQuestions(string strLcode, string strSeq, string strActive, string strDesc, string strRespType, string strMemAccess, string strSDesc, string strPIPACTIVE)
        {
            int inti = 0;

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand(" INSERT INTO dbo.LEANCUSTFLDS (LCUST_CODE, LCUST_SEQ, LCUST_ACTIVE, LCUST_DESC, LCUST_RESP_TYPE, LCUST_MEM_ACCESS, LCUST_DATE_ADD, LCUST_ADD_OPERATOR, LCUST_DATE_LSTC, LCUST_LSTC_OPERATOR, LCUST_SDESC, LCUST_DBName,LCUST_PIP_ACTIVE)VALUES('" + strLcode + "','" + strSeq + "','" + strActive + "','" + strDesc + "','" + strRespType + "','" + strMemAccess + "','" + DateTime.Now.Date + "','" + BaseForm.UserID + "','" + DateTime.Now.Date + "','" + BaseForm.UserID + "','" + strSDesc + "','" + BaseForm.BaseAgencyControlDetails.AgyShortName + "','" + strPIPACTIVE + "')", con))
                {
                    inti = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }
            return inti;

        }
        void DeleteLeanCustomeQuestions(string strLcode, string strapp)
        {

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM LEANCUSTFLDS WHERE LCUST_DBName= '" + strLcode + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }

        }
        void DeleteLeanCustResponses(string strLcode, string strapp)
        {

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM LEANCUSTRESP WHERE LRSP_DBNAME= '" + strLcode + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }

        }
        int InsertLeanCustResponses(string strLcode, string strSeq, string strDesc, string strRespCode, string strSDesc)
        {

            int inti = 0;

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand(" INSERT INTO [dbo].[LEANCUSTRESP]([LRSP_CUST_CODE],[LRSP_SEQ],[LRSP_DESC],[LRSP_RESP_CODE],[LRSP_SDESC],[LRSP_DBNAME])VALUES('" + strLcode + "','" + strSeq + "','" + strDesc + "','" + strRespCode + "','" + strSDesc + "','" + BaseForm.BaseAgencyControlDetails.AgyShortName + "')", con))
                {
                    inti = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }
            return inti;

        }

        int InsertLeanAgyTabs(string strType, string strcode, string strActive, string strDesc, string strDefault, string strSDesc)
        {
            int inti = 0;

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand(" INSERT INTO [dbo].[LEAN_AGYTABS]([AGYS_TYPE],[AGYS_CODE],[AGYS_DESC],[AGYS_ACTIVE],[AGYS_DEFAULT],[AGYS_DB],[AGYS_SDESC])VALUES('" + strType + "','" + strcode + "','" + strDesc + "','" + strActive + "','" + strDefault + "','" + BaseForm.BaseAgencyControlDetails.AgyShortName + "','" + strSDesc + "')", con))
                {
                    inti = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }
            return inti;

        }
        void DeleteLeanAgyTabs()
        {

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM LEAN_AGYTABS  WHERE AGYS_DB = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }

        }

        int InsertLeanServices(string strcode, string strDesc, string strSDesc, string strType, string strPIPACTIVE)
        {
            int inti = 0;

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand(" INSERT INTO [dbo].[LEANCASEHIESER]([LCASE_CODE],[LCASE_DESC],[LCASE_SDESC],[LCASE_TYPE],[LCASE_DBNAME],[LCASE_PIP_ACTIVE])VALUES('" + strcode + "','" + strDesc + "','" + strSDesc + "','" + strType + "','" + BaseForm.BaseAgencyControlDetails.AgyShortName + "','" + strPIPACTIVE + "')", con))
                {
                    inti = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }
            return inti;

        }
        void DeleteLeanServices(string strType)
        {

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM LEANCASEHIESER  WHERE LCASE_DBNAME = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND LCASE_TYPE = '" + strType + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }

        }

        int CheckCustomQuestions(string strcode, string strType)
        {
            int inti = 0;

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                if (strType == "CUSTOM")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Count(*) as count1  FROM LEANADDCUST WHERE ADDCUST_ACT_CODE ='" + strcode + "' AND ADDCUST_DBName = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "'", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string strvalue1 = ds.Tables[0].Rows[0]["count1"].ToString();
                            if (strvalue1 != string.Empty)
                            {
                                inti = Convert.ToInt32(strvalue1);
                            }
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                inti = 0;

            }
            return inti;

        }

        private void chkPipselectall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPipselectall.Checked)
            {
                foreach (DataGridViewRow gvitemrows in gvwResponses.Rows)
                {
                    gvitemrows.Cells["gvtCheckPIP"].Value = "True";
                }
            }
            else
            {
                foreach (DataGridViewRow gvitemrows in gvwResponses.Rows)
                {
                    gvitemrows.Cells["gvtCheckPIP"].Value = "False";
                }
            }
        }
    }
}