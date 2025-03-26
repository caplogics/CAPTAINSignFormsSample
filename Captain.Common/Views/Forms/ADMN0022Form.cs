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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMN0022Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion

        public ADMN0022Form(BaseForm baseForm, string mode, string strServicecode, PrivilegeEntity privilegeEntity, string strScreenType, string strGroupcode, SalquesEntity _salquesdata, string strGroupSeq)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Mode = mode;
            // lblHeader.Text = privilegeEntity.PrivilegeName;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propScreenType = strScreenType;
            propsalquesdata = _salquesdata;
            propgroupcode = strGroupcode;
            Mode = mode;
            propSalId = strServicecode;
            txtQseq.Validator = TextBoxValidation.IntegerValidator;
            txtGSeq.Validator = TextBoxValidation.IntegerValidator;
            SelectedHierarchies = new List<Model.Objects.HierarchyEntity>();
            txtName.Focus();

            if (propScreenType == "S")
            {
                this.Size = new Size(730, 330);
               // lblHeader.Text = "SAL/CAL Definition";
                List<ListItem> listItem = new List<ListItem>();
                cmbType.Items.Clear();
                listItem = new List<ListItem>();
                listItem.Add(new ListItem("Select One", "0"));
                listItem.Add(new ListItem("Contact", "C"));
                listItem.Add(new ListItem("Service", "S"));
                cmbType.Items.AddRange(listItem.ToArray());
                cmbType.SelectedIndex = 0;
                this.Text = "Contact/Service Activity Definition" + " - " + mode; //"SAL/CAL Definition" + " - " + mode;
                this.pnlService.Size = new Size(730, 330);
                this.pnlQuestions.Visible = false;
                this.pnlGroup.Visible = false;
                this.pnlService.Visible = true;
                chkActive.Checked = true;
                if (Mode == "Edit")
                {
                    fillServicesData();
                }
                else if (Mode == "Copy")
                {
                    fillServicesData();
                    txt_Hierachies.Text = string.Empty;
                    Current_Hierarchy_DB = string.Empty;
                    cmbType.Enabled = false;

                }

            }
            else if (propScreenType == "G")
            {
                this.Size = new Size(647, 165);
                // lblHeader.Text = "Custom Questions Group";
                this.Text = "Custom Questions Group" + " - " + mode;
               // this.pnlGroup.Location = new Point(0, 0);
               // this.pnlGroup.Size = new Size(646, 138);
               // this.pnlsave.Location = new Point(0, 137);
                this.pnlQuestions.Visible = false;
                this.pnlService.Visible = false;
                this.pnlGroup.Visible = true;
                if (Mode == "Edit")
                {
                    txtGSeq.Text = propsalquesdata.SALQ_GRP_SEQ;
                    txtGDesc.Text = propsalquesdata.SALQ_DESC;
                }
                else
                    txtGSeq.Text = strGroupSeq;
            }
            else if (propScreenType == "Q")
            {
                this.Text = "Contact and Service Activity Custom Questions" + " - " + mode;
              //  this.pnlQuestions.Location = new Point(0, 0);
              //  this.pnlQuestions.Size = new Size(695, 410);
              //  this.pnlsave.Location = new Point(0, 214);
                this.Size = new Size(695, 410);
                this.pnlGroup.Visible = false;
                this.pnlService.Visible = false;
                this.pnlQuestions.Visible = true;
                SaldefEntity _sqldefentity = new SaldefEntity(true);
                _sqldefentity.SALD_ID = propSalId;
                List<SaldefEntity> sqldeflist = _model.SALDEFData.Browse_SALDEF(_sqldefentity, "Browse",BaseForm.UserID,BaseForm.BaseAdminAgency);
                if (sqldeflist.Count > 0)
                    Cust5Quests = sqldeflist[0].SALD_5QUEST;

                filldropdowns();
                if (Mode == "Edit")
                {
                    txtQseq.Text = propsalquesdata.SALQ_SEQ;
                    TxtQuesDesc.Text = propsalquesdata.SALQ_DESC;
                    CommonFunctions.SetComboBoxValue(cmbQuestionType, propsalquesdata.SALQ_TYPE);
                   
                    if (propsalquesdata.SALQ_REQ.Trim() == "Y") chkbQuesReq.Checked = true;else chkbQuesReq.Checked = false;
                    
                    if (propsalquesdata.SALQ_TYPE == "C" || propsalquesdata.SALQ_TYPE == "D")
                    {
                        FillCustRespGrid(propsalquesdata.SALQ_ID);
                        CustRespPanel.Visible = true;
                        this.pnlQuestions.Size = new Size(695, 330);
                        this.Size = new Size(695, 422);
                    }
                    else
                    {
                        CustRespPanel.Visible = false;
                        this.pnlQuestions.Size = new Size(695, 118);
                        this.Size = new Size(695, 219);
                    }
                }
                else
                    txtQseq.Text = strGroupSeq;
            }
        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity privileges { get; set; }
        public string propgroupcode { get; set; }
        public SalquesEntity propsalquesdata { get; set; }
        public string Cust5Quests{get;set;}

        string PropAgency = string.Empty, propDept = string.Empty, propProgram = string.Empty, propYear = string.Empty;

        public string Mode { get; set; }
        public string propScreenType { get; set; }
        public string propSalId { get; set; }

        private void fillServicesData()
        {
            SaldefEntity _sqldefentity = new SaldefEntity(true);
            _sqldefentity.SALD_ID = propSalId;
            List<SaldefEntity> sqldeflist = _model.SALDEFData.Browse_SALDEF(_sqldefentity, "Browse", BaseForm.UserID, BaseForm.BaseAdminAgency);
            if (sqldeflist.Count > 0)
            {


                txt_Hierachies.Text = sqldeflist[0].SALD_HIE.Substring(0, 2) + "-" + sqldeflist[0].SALD_HIE.Substring(2, 2) + "-" + sqldeflist[0].SALD_HIE.Substring(4, 2);
                PropAgency = sqldeflist[0].SALD_HIE.Substring(0, 2);
                propDept = sqldeflist[0].SALD_HIE.Substring(2, 2);
                propProgram = sqldeflist[0].SALD_HIE.Substring(4, 2);
                Current_Hierarchy_DB = txt_Hierachies.Text;
                txtServiceplan.Text = sqldeflist[0].SALD_SPS;
                txtServices.Text = sqldeflist[0].SALD_SERVICES;
                txtName.Text = sqldeflist[0].SALD_NAME;
                chkActive.Checked = sqldeflist[0].SALD_ACTIVE == "A" ? true : false;
                CommonFunctions.SetComboBoxValue(cmbType, sqldeflist[0].SALD_TYPE);
                txtBoilerplate.Text = sqldeflist[0].SALD_BOILERPLATE;
                chkSignature.Checked = sqldeflist[0].SALD_SIGN_REQURED == "Y" ? true : false;
                if (sqldeflist[0].SALD_TYPE == "C")
                    this.Text = "Contact Definition" + " - " + Mode;   //"CAL Definition";//lblHeader.Text = "CAL Definition";
                else
                    this.Text = "Service Definition" + " - " + Mode;   //"SAL Definition";//lblHeader.Text = "SAL Definition";

                if (!string.IsNullOrEmpty(sqldeflist[0].SALD_SPS.Trim()) && string.IsNullOrEmpty(sqldeflist[0].SALD_SERVICES.Trim()))
                {
                    chkbQuestion.Visible = true;
                    if (sqldeflist[0].SALD_5QUEST == "Y") { chkbQuestion.Checked = true; btnservices.Enabled = false; } else { chkbQuestion.Checked = false; btnservices.Enabled = true; }
                }
                else chkbQuestion.Visible = false;

            }
        }

        private void OnHelpClick(object sender, EventArgs e)
        {

        }

        string strmsgGrp = string.Empty; string SqlMsg = string.Empty;
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (boolValidation())
            {
                switch (propScreenType)
                {
                    case "S":
                        SaldefEntity _saldefentity = new SaldefEntity();
                        _saldefentity.SALD_ID = propSalId;
                        _saldefentity.SALD_TYPE = ((ListItem)cmbType.SelectedItem).Value.ToString();
                        _saldefentity.SALD_HIE = PropAgency + propDept + propProgram;
                        _saldefentity.SALD_NAME = txtName.Text;
                        _saldefentity.SALD_SPS = txtServiceplan.Text;
                        _saldefentity.SALD_SERVICES = txtServices.Text;
                        _saldefentity.SALD_ACTIVE = chkActive.Checked == true ? "A" : "I";

                        _saldefentity.SALD_ADD_OPERATOR = BaseForm.UserID;
                        _saldefentity.SALD_LSTC_OPERATOR = BaseForm.UserID;
                        _saldefentity.SALD_BOILERPLATE = txtBoilerplate.Text;
                        _saldefentity.SALD_SIGN_REQURED = chkSignature.Checked == true ? "Y" : "N";

                        _saldefentity.SALD_5QUEST = chkbQuestion.Checked == true ? "Y" : "N";

                        _saldefentity.Mode = Mode;

                        if (_model.SALDEFData.CAP_SALDEF_INSUPDEL(_saldefentity, out strmsgGrp, out SqlMsg))
                        {
                            this.Close();
                            ADMN0022Control ADMN0022Control = BaseForm.GetBaseUserControl() as ADMN0022Control;
                            if (Mode == "Add")
                            {
                                AlertBox.Show("Saved Successfully");
                            }
                            else
                            {
                                AlertBox.Show("Updated Successfully");
                            }
                            if (ADMN0022Control != null)
                            {
                                ADMN0022Control.Refresh("S",strmsgGrp);
                            }
                        }
                        break;
                    case "G":
                        SalquesEntity _salquesentity = new SalquesEntity();
                        _salquesentity.SALQ_ID = string.Empty;
                        if (propsalquesdata != null)
                            _salquesentity.SALQ_ID = propsalquesdata.SALQ_ID;
                        _salquesentity.SALQ_SALD_ID = propSalId;
                        _salquesentity.SALQ_GRP_CODE = string.Empty;
                        _salquesentity.SALQ_GRP_SEQ = txtGSeq.Text;
                        _salquesentity.SALQ_SEQ = "0";
                        _salquesentity.SALQ_TYPE = string.Empty;
                        _salquesentity.SALQ_DESC = txtGDesc.Text;
                        _salquesentity.SALQ_ADD_OPERATOR = BaseForm.UserID;
                        _salquesentity.SALQ_LSTC_OPERATOR = BaseForm.UserID;
                        _salquesentity.Mode = Mode;
                        string stroutquesid = string.Empty;
                        if (_model.SALDEFData.CAP_SALQUES_INSUPDEL(_salquesentity, "GROUP", out stroutquesid))
                        {
                            this.Close();
                            ADMN0022Control ADMN0022Control = BaseForm.GetBaseUserControl() as ADMN0022Control;
                            if (Mode == "Add")
                            {
                                AlertBox.Show("Saved Successfully");
                            }
                            else
                            {
                                AlertBox.Show("Updated Successfully");
                            }
                            if (ADMN0022Control != null)
                            {
                                ADMN0022Control.Refresh("G", stroutquesid);

                            }
                        }
                        break;
                    case "Q":
                        _salquesentity = new SalquesEntity();
                        if (Mode == "Edit")
                        {
                            if (propsalquesdata != null)
                                _salquesentity.SALQ_ID = propsalquesdata.SALQ_ID;
                        }
                        
                        else
                            _salquesentity.SALQ_ID = string.Empty;

                        _salquesentity.SALQ_SALD_ID = propSalId;
                        _salquesentity.SALQ_GRP_CODE = propsalquesdata.SALQ_GRP_CODE;
                        _salquesentity.SALQ_GRP_SEQ = propsalquesdata.SALQ_GRP_SEQ;
                        _salquesentity.SALQ_SEQ = txtQseq.Text;
                        _salquesentity.SALQ_TYPE = ((ListItem)cmbQuestionType.SelectedItem).Value.ToString(); ;
                        _salquesentity.SALQ_DESC = TxtQuesDesc.Text;
                        _salquesentity.SALQ_REQ = chkbQuesReq.Checked == true ? "Y" : "N";
                        _salquesentity.SALQ_ADD_OPERATOR = BaseForm.UserID;
                        _salquesentity.SALQ_LSTC_OPERATOR = BaseForm.UserID;
                        _salquesentity.Mode = Mode;
                        stroutquesid = string.Empty;
                        if (_model.SALDEFData.CAP_SALQUES_INSUPDEL(_salquesentity, "QUES", out stroutquesid))
                        {
                            if (Mode == "Add")
                                _salquesentity.SALQ_ID = stroutquesid;

                            if (_salquesentity.SALQ_TYPE == "C" || _salquesentity.SALQ_TYPE == "D")
                            {
                                int intseq = 0;
                                SalqrespEntity _salqrespentity = new SalqrespEntity();
                                _salqrespentity.SALQR_Q_ID = _salquesentity.SALQ_ID;
                                _salqrespentity.Mode = "DELETEALL";
                                _model.SALDEFData.CAP_SALQRESP_INSUPDEL(_salqrespentity);

                                foreach (DataGridViewRow gridrow in CustRespGrid.Rows)
                                {
                                    if (gridrow != null)
                                    {
                                        if (gridrow.Cells["RespDesc"].Value != null && gridrow.Cells["RespCode"].Value != null)
                                        {
                                            intseq = intseq + 1;
                                            _salqrespentity = new SalqrespEntity();
                                            _salqrespentity.SALQR_Q_ID = _salquesentity.SALQ_ID;
                                            _salqrespentity.SALQR_CODE = gridrow.Cells["RespCode"].Value.ToString();
                                            _salqrespentity.SALQR_DESC = gridrow.Cells["RespDesc"].Value.ToString();
                                            _salqrespentity.SALQR_SEQ = intseq.ToString();
                                            _salqrespentity.SALQR_ADD_OPERATOR = BaseForm.UserID;
                                            _salqrespentity.SALQR_LSTC_OPERATOR = BaseForm.UserID;
                                            _salqrespentity.Mode = "Add";
                                            string strchange = gridrow.Cells["Changed"].Value != null ? gridrow.Cells["Changed"].Value.ToString() : string.Empty;
                                            string strType = gridrow.Cells["Type"].Value != null ? gridrow.Cells["Type"].Value.ToString() : string.Empty;
                                            _model.SALDEFData.CAP_SALQRESP_INSUPDEL(_salqrespentity);
                                        }
                                    }

                                }

                            }
                            this.Close();
                            ADMN0022Control ADMN0022Control = BaseForm.GetBaseUserControl() as ADMN0022Control;
                            if (Mode == "Add")
                            {
                                AlertBox.Show("Saved Successfully");
                            }
                            else
                            {
                                AlertBox.Show("Updated Successfully");
                            }

                            if (ADMN0022Control != null)
                            {
                                ADMN0022Control.Refresh("Q", stroutquesid);
                            }
                        }
                        break;

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public List<HierarchyEntity> SelectedHierarchies { get; set; }
        private void Pb_MS_Prog_Click(object sender, EventArgs e)
        {
            if (txt_Hierachies.Text != string.Empty)
                Current_Hierarchy_DB = txt_Hierachies.Text;
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "E", "A", "Reports",BaseForm.UserID); 
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "", Current_Hierarchy_DB = "";

        private void btnservices_Click(object sender, EventArgs e)
        {
            if (txtServiceplan.Text != string.Empty)
            {
                ServicesSelectionsForm serviceform = new ServicesSelectionsForm(BaseForm, privileges, string.Empty, "SP2", PropAgency, propDept, propProgram, txtServiceplan.Text, txtServices.Text);
                serviceform.FormClosed += new FormClosedEventHandler(On_servicesp2_Closed);
                serviceform.StartPosition = FormStartPosition.CenterScreen;
                serviceform.ShowDialog();
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please select at least one Service Plan");
            }
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    txt_Hierachies.Text = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);
                    PropAgency = hierarchy.Substring(0, 2);
                    propDept = hierarchy.Substring(2, 2);
                    propProgram = hierarchy.Substring(4, 2);
                    txtServices.Text = string.Empty;
                    txtServiceplan.Text = string.Empty;

                }
            }
        }

        private void filldropdowns()
        {



            List<ListItem> listqType = new List<ListItem>();
            cmbQuestionType.Items.Clear();
            listqType = new List<ListItem>();
            listqType.Add(new ListItem("Select One", "0"));
            listqType.Add(new ListItem("Checkbox", "C"));
            listqType.Add(new ListItem("Date", "T"));
            listqType.Add(new ListItem("Dropdown", "D"));
            listqType.Add(new ListItem("Numeric", "N"));
            listqType.Add(new ListItem("Text", "X"));
            if (Cust5Quests == "N")
            {
                listqType.Add(new ListItem("Text- 1 line 80 characters", "1"));
                listqType.Add(new ListItem("Text- 2 line 80 characters per line", "2"));
                listqType.Add(new ListItem("Text- 3 lines 80 character per line", "3"));
            }
            cmbQuestionType.Items.AddRange(listqType.ToArray());
            cmbQuestionType.SelectedIndex = 0;


        }


        private void btnserviceplan_Click(object sender, EventArgs e)
        {
            if (txt_Hierachies.Text != string.Empty)
            {
                ServicesSelectionsForm serviceform = new ServicesSelectionsForm(BaseForm, privileges, string.Empty, "SERVICEPLAN", PropAgency, propDept, propProgram, txtServiceplan.Text, txtServices.Text);
                serviceform.FormClosed += new FormClosedEventHandler(On_serviceplan_Closed);
                serviceform.StartPosition = FormStartPosition.CenterScreen;
                serviceform.ShowDialog();
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please select Hierarchy");
            }
        }
        List<CASESP1Entity> Select_sps_List = new List<CASESP1Entity>();

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string RespType = ((ListItem)cmbQuestionType.SelectedItem).Value.ToString();
            _errorProvider.SetError(lblquestype, null);
            //   this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
            // OrgCustResp.Clear();
            //  this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            if (!((ListItem)cmbQuestionType.SelectedItem).Value.ToString().Equals("0"))
                _errorProvider.SetError(cmbQuestionType, null);


            if (RespType == "D" || RespType == "C")
            {
                CustRespPanel.Visible = true;
                this.pnlQuestions.Size = new Size(695, 330);
                this.Size = new Size(695,422);
                CustRespGrid.AllowUserToAddRows = true;
                CustRespGrid.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            else
            {
                CustRespPanel.Visible = false;
                this.pnlQuestions.Size = new Size(695,118);
                this.Size = new Size(695, 219);
            }

        }

        private void CustRespGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        string Cust_SCR_Mode = "View";
        private void CustRespGrid_SelectionChanged(object sender, EventArgs e)
        {

            bool CanSave = true, Delete_SW = false;

            //if (PrivRow == null)
            //    PrivRow.Tag = 0; 
            if (Cust_SCR_Mode != "View")
            {
                if (CustResp_lod_complete && PrivRow != null && PrivRow.Index >= 0)
                {

                    if (CanSave) // CheckDupCustResps(Delete_SW))
                    {
                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Type"].EditedFormattedValue.ToString())))
                        {
                            foreach (SalqrespEntity Entity in OrgCustResp)
                            {
                                if (PrivRow.Cells["CustSeq"].Value.ToString() == Entity.SALQR_SEQ)
                                {
                                    if (!Delete_SW)
                                    {
                                        Entity.SALQR_SEQ = PrivRow.Cells["RespCode"].Value.ToString();
                                        Entity.SALQR_DESC = PrivRow.Cells["RespDesc"].Value.ToString();

                                    }
                                    else
                                    {
                                        this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);

                                        //if (!CustRespGrid.Rows[this.CustRespGrid.SelectedRows[0].Index].IsNewRow)
                                        CustRespGrid.Rows.RemoveAt(PrivRow.Index);
                                        //CustRespGrid.Rows.RemoveAt(this.CustRespGrid.SelectedRows[0].Index);

                                        this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);

                                        Entity.RecType = "D";
                                    }

                                    Entity.Changed = "Y";
                                    // PrivRow.Cells["Changed"].Value = "Y";
                                    break;
                                }
                            }
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
                                !string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()))
                            {
                                PrivRow.Cells["Type"].Value = "I";
                                SalqrespEntity New_Entity = new SalqrespEntity();
                                New_Entity.RecType = "I";
                                //if (Cust_SCR_Mode.Equals("Edit"))
                                //    New_Entity.SALQR_CODE = gvwQuestion.CurrentRow.Cells["CustomKey"].Value.ToString();
                                //else
                                //{
                                //    if (!string.IsNullOrEmpty(TxtCode.Text.Trim()))
                                //        New_Entity.SALQR_CODE = "C" + TxtCode.Text.Trim();
                                //}
                                New_Entity.SALQR_SEQ = (OrgCustResp.Count + 1).ToString();
                                New_Entity.SALQR_DESC = PrivRow.Cells["RespDesc"].Value.ToString();
                                New_Entity.SALQR_ADD_OPERATOR = BaseForm.UserID;
                                New_Entity.SALQR_LSTC_OPERATOR = BaseForm.UserID;
                                New_Entity.Changed = "Y";

                                OrgCustResp.Add(new SalqrespEntity(New_Entity));
                            }
                        }
                    }
                }

                if (CustRespGrid.Rows.Count >= 0)
                {
                    try
                    {
                        DataGridViewRow row = CustRespGrid.SelectedRows[0];
                        PrivRow = row;
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }

        }

        private void Clear_CustRespGrid()
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
        }

        DataGridViewRow PrivRow;
        bool CustResp_lod_complete = false;
        List<SalqrespEntity> OrgCustResp = new List<SalqrespEntity>();
        private void FillCustRespGrid(string CustCode)
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            Clear_CustRespGrid();
            OrgCustResp.Clear();
            SalqrespEntity _sqlqrespentity = new SalqrespEntity(true);
            _sqlqrespentity.SALQR_Q_ID = CustCode;
            List<SalqrespEntity> CustResp = _model.SALDEFData.Browse_SALQRESP(_sqlqrespentity, "Browse");

            if (CustResp.Count > 0)
            {
                CustRespPanel.Visible = true;
                int rowIndex = 0;
                int RowCount = 0;
                foreach (SalqrespEntity Entity in CustResp)
                {
                    //rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc);  
                    rowIndex = CustRespGrid.Rows.Add(Entity.SALQR_CODE, Entity.SALQR_DESC.Trim(), Entity.RecType, Entity.Changed);
                    RowCount++;
                }
                if (RowCount > 0)
                {

                    OrgCustResp = CustResp;
                    CustResp_lod_complete = true;
                    CustRespGrid.Rows[rowIndex].Tag = 0;

                }
            }
            CustResp_lod_complete = true;
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            if (CustRespGrid.Rows.Count > 0)
            {
                CustRespGrid.Rows[0].Selected = true;
            }
        }

        //private bool CheckDupCustResps(bool Delete_SW)
        //{
        //    bool ReturnVal = true;
        //    if (!Delete_SW)
        //    {
        //        string TestCode, TestDesc, TestNCode, TmpSelCode = null, TmpSelDesc = null, TmpSelNCode = null;

        //        TmpSelCode = PrivRow.Cells["RespCode"].Value == null ? string.Empty : PrivRow.Cells["RespCode"].Value.ToString();
        //        TmpSelDesc = PrivRow.Cells["RespDesc"].Value == null ? string.Empty : PrivRow.Cells["RespDesc"].Value.ToString();

        //        foreach (DataGridViewRow dr in CustRespGrid.Rows)
        //        {
        //            TestCode = TestDesc = TestNCode = null;
        //            if (!(string.IsNullOrEmpty(dr.Cells["RespCode"].FormattedValue.ToString())))
        //                TestCode = dr.Cells["RespCode"].Value.ToString().Trim();
        //            if (!(string.IsNullOrEmpty(dr.Cells["RespDesc"].FormattedValue.ToString())))
        //                TestDesc = dr.Cells["RespDesc"].Value.ToString().Trim();



        //            if (TmpSelCode == TestCode && PrivRow != dr)
        //            {
        //                ReturnVal = false;
        //                MessageBox.Show("Response Code '" + "'" + TmpSelCode + "' Already Exists", "CAP Systems", MessageBoxButtons.OK);
        //                break;
        //            }
        //            if (TmpSelDesc == TestDesc && PrivRow != dr)
        //            {
        //                ReturnVal = false;
        //                MessageBox.Show("Code Description '" + TmpSelDesc + "' Already Exists", "CAP Systems", MessageBoxButtons.OK);
        //                break;
        //            }
        //            if (TmpSelNCode == TestNCode && PrivRow != dr)
        //            {
        //                ReturnVal = false;
        //                MessageBox.Show("Xml Code '" + "'" + TmpSelNCode + "' Already Exists", "CAP Systems", MessageBoxButtons.OK);
        //                break;
        //            }

        //        }
        //    }
        //    return ReturnVal;
        //}

        private void cmbType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbType.Items.Count > 0)
            {
                _errorProvider.SetError(txtServiceplan, null);
                _errorProvider.SetError(txtServices, null);
                if (((ListItem)cmbType.SelectedItem).Value.ToString() == "C")
                {
                    pnlserviceplan.Visible = false;
                    txtServiceplan.Text = string.Empty;
                    txtServices.Text = string.Empty;
                    this.Size = new Size(730, 270);
                }
                else
                {
                    pnlserviceplan.Visible = true;
                    this.Size = new Size(730, 330);
                }
            }
        }

        private void chkbQuestion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbQuestion.Checked)
            {
                btnservices.Enabled = false;
            }
            else btnservices.Enabled = true;
        }

        private void txtQseq_Leave(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtQseq.Text.Trim()))
            {
                if (int.Parse(txtQseq.Text.Trim()) <= 0)
                {
                    txtQseq.Text = string.Empty;
                    _errorProvider.SetError(txtQseq, string.Format("Sequence should be greater than 0", lblQseq.Text));
                }
                else
                    _errorProvider.SetError(txtQseq, null);
            }
        }

        private void On_serviceplan_Closed(object sender, FormClosedEventArgs e)
        {
            string Selspcodes_Name = string.Empty;

            ServicesSelectionsForm form = sender as ServicesSelectionsForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Select_sps_List = form.GetSelected_spslist_Entity();
                foreach (CASESP1Entity spitem in Select_sps_List)
                {
                    if (Selspcodes_Name != string.Empty)
                        Selspcodes_Name = Selspcodes_Name + "," + spitem.Code.Trim();
                    else
                        Selspcodes_Name = spitem.Code.Trim();
                }
                txtServiceplan.Text = Selspcodes_Name;
                if (!string.IsNullOrEmpty(txtServiceplan.Text.Trim())) chkbQuestion.Visible = true; else chkbQuestion.Visible = false;
            }
        }

        List<CASESP2Entity> Select_sp2_List = new List<CASESP2Entity>();
        private void On_servicesp2_Closed(object sender, FormClosedEventArgs e)
        {
            string Selspcodes_Name = string.Empty;

            ServicesSelectionsForm form = sender as ServicesSelectionsForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Select_sp2_List = form.GetSelected_sp2list_Entity();
                foreach (CASESP2Entity spitem in Select_sp2_List)
                {
                    if (Selspcodes_Name != string.Empty)
                        Selspcodes_Name = Selspcodes_Name + "," + spitem.CamCd.Trim();
                    else
                        Selspcodes_Name = spitem.CamCd.Trim();
                }
                txtServices.Text = Selspcodes_Name;
                if (!string.IsNullOrEmpty(txtServices.Text.Trim())) { chkbQuestion.Visible = false; chkbQuestion.Checked = false; }
                else { chkbQuestion.Visible = true; }
            }
        }

        private bool boolValidation()
        {
            bool isValid = true;
            switch (propScreenType)
            {
                case "S":
                    if ((((ListItem)cmbType.SelectedItem).Value.ToString().Equals("0")))
                    {
                        _errorProvider.SetError(cmbType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblType.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbType, null);
                    }
                    if (string.IsNullOrEmpty(txtName.Text.Trim()))
                    {
                        _errorProvider.SetError(txtName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblName.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtName, null);
                    }
                    if (String.IsNullOrEmpty(txt_Hierachies.Text.Trim()))
                    {
                        _errorProvider.SetError(txt_Hierachies, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHierchy.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txt_Hierachies, null);
                    }

                    if(chkbQuestion.Checked==true)
                    {
                        SaldefEntity _sqldefentity = new SaldefEntity(true);
                        _sqldefentity.SALD_TYPE = "S";
                        
                        //_sqldefentity.SALD_HIE=txt_Hierachies
                        List<SaldefEntity> sqldeflist = _model.SALDEFData.Browse_SALDEF(_sqldefentity, "Browse", BaseForm.UserID, BaseForm.BaseAdminAgency);
                        if (Mode == "Edit") sqldeflist = sqldeflist.FindAll(u => u.SALD_ID != propSalId);
                        if (sqldeflist.Count>0)
                        {
                            List<SaldefEntity> ListSaldefEntity = sqldeflist.FindAll(u => u.SALD_HIE.Equals(txt_Hierachies.Text.Replace("-", "").ToString())  && u.SALD_5QUEST == "Y" && u.SALD_SERVICES==string.Empty);
                            if(ListSaldefEntity.Count>0)
                            {
                                SaldefEntity Entity = new SaldefEntity();
                                string[] Serplan = txtServiceplan.Text.Trim().Split(',');
                                if(Serplan.Length>0)
                                {
                                    for(int i=0;i<Serplan.Length;i++)
                                    {
                                        Entity = ListSaldefEntity.Find(u => u.SALD_SPS.Contains(Serplan[i].ToString()));
                                        if (Entity != null)
                                            break;
                                    }
                                }

                                if (Entity != null)
                                {
                                    _errorProvider.SetError(chkbQuestion, string.Format("Already defined for the selected Hierarchy and Service Plan", chkbQuestion.Text));
                                    isValid = false;
                                }
                                else _errorProvider.SetError(chkbQuestion, null);
                            }
                            else _errorProvider.SetError(chkbQuestion, null);

                        }
                    }

                    //if ((((ListItem)cmbType.SelectedItem).Value.ToString().Equals("S")))
                    //{

                    //    if (String.IsNullOrEmpty(txtServiceplan.Text.Trim()))
                    //    {
                    //        _errorProvider.SetError(txtServiceplan, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblServiceplan.Text));
                    //        isValid = false;
                    //    }
                    //    else
                    //    {
                    //        _errorProvider.SetError(txtServiceplan, null);
                    //    }
                    //    if (string.IsNullOrEmpty(txtServices.Text.Trim()))
                    //    {
                    //        _errorProvider.SetError(txtServices, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblServices.Text));
                    //        isValid = false;
                    //    }
                    //    else
                    //    {
                    //        _errorProvider.SetError(txtServices, null);
                    //    }
                    //}

                    break;
                case "G":
                    if (String.IsNullOrEmpty(txtGSeq.Text.Trim()))
                    {
                        _errorProvider.SetError(txtGSeq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGSeq.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtGSeq, null);
                    }
                    if (String.IsNullOrEmpty(txtGDesc.Text.Trim()))
                    {
                        _errorProvider.SetError(txtGDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGDesc.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtGDesc, null);
                    }
                    break;
                case "Q":
                    if (String.IsNullOrEmpty(txtQseq.Text.Trim()))
                    {
                        _errorProvider.SetError(txtQseq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQseq.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtQseq, null);
                    }
                    if (String.IsNullOrEmpty(TxtQuesDesc.Text.Trim()))
                    {
                        _errorProvider.SetError(TxtQuesDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQDesc.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(TxtQuesDesc, null);
                    }
                    if ((((ListItem)cmbQuestionType.SelectedItem).Value.ToString().Equals("0")))
                    {
                        _errorProvider.SetError(cmbQuestionType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblquestype.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbQuestionType, null);
                    }
                    break;
            }



            return isValid;
        }






    }
}