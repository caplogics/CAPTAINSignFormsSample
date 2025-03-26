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
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.Controls.Compatibility;
using DevExpress.XtraCharts.Native;
using DevExpress.CodeParser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MAT00003AssessmentDate : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();
        private string strAgency = string.Empty;
        private string strDept = string.Empty;
        private string strProgram = string.Empty;
        private string strYear = string.Empty;
        private string strApplNo = string.Empty;
        private string strMatCode = string.Empty;
        private string strMode = string.Empty;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        private string strCaseWorkerDefaultCode = "0";
        private string strCaseWorkerDefaultStartCode = "0";
        private string strMaxDate = string.Empty;

        int strIndex = 0;

        public MAT00003AssessmentDate(BaseForm baseForm, PrivilegeEntity privilieges, string MatCode, string strInterval)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            strAgency = BaseForm.BaseAgency;
            strDept = BaseForm.BaseDept;
            strProgram = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            strApplNo = BaseForm.BaseApplicationNo;
            strMatCode = MatCode;
            Matcode = MatCode;
            strMode = string.Empty;
            txtIntervalIndays.Validator = TextBoxValidation.IntegerValidator;
            txtIntervalIndays.Text = strInterval;

            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Mat_Code = Matcode;
            Search_Entity.Scale_Code = "0";
            propmatdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");


            MATDEFBMEntity Search_BMEntity = new MATDEFBMEntity(true);
            Search_BMEntity.MatCode = MatCode;
            MATADEFBMentity = _model.MatrixScalesData.Browse_MATDEFBM(Search_BMEntity, "Browse");
            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().ToUpper() == "Y")
            {
                pnlDate.Visible = true;
                lblFon.Visible = true;
                lblCompdate.Visible = true;
                dtCompleted.Visible = true;
                dtFollowup.Visible = true;
            }
            if (propmatdefEntity.Count > 0)
            {
                if (propmatdefEntity[0].OverlScor == "Y")
                {
                    lblTotalScoreDesc.Visible = true;
                    txtTotalScore.Visible = true;
                    lblTot.Visible = true;
                }
                if (propmatdefEntity[0].SpecScor == "Y")
                {
                    lblParitalScoreDesc.Visible = true;
                    txtPatialScore.Visible = true;
                    lblPartial.Visible = true;
                }


            }
            //LblHeader.Text = Privileges.PrivilegeName;
            this.Text = "Matrix Maintenance" + " - " + "Assessment Dates";// Privileges.Program + " - " + privilieges.PrivilegeName;
            HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(strAgency, "**", "**");
            if (HierarchyEntity != null)
            {
                strNameFormat = HierarchyEntity.CNFormat.ToString();
                strVerfierFormat = HierarchyEntity.CWFormat.ToString();
            }
            fillCombo();
            FillGridData();
            if (Privileges.AddPriv.Equals("false"))
            {
                gvwAssessmentDetails.AllowUserToAddRows = false;
            }
            else
            {
                gvwAssessmentDetails.AllowUserToAddRows = true;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvwAssessmentDetails.Columns["Delete"].Visible = false;
                // gvwAssessmentDetails.Columns[gvwAssessmentDetails.ColumnCount - 2].Width = 200;
            }
            else
            {
                gvwAssessmentDetails.Columns["Delete"].Visible = true;

            }
            btnSave.Text = "&Save";

            FieldcontrolCheck();
        }


        public MAT00003AssessmentDate(BaseForm baseForm, PrivilegeEntity privilieges, string MatCode, List<MATDEFDTEntity> sel_MAtDates_List, string type, string Mode)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            strMatCode = MatCode;
            Matcode = MatCode;
            Sel_Date_List = sel_MAtDates_List;
            strMode = Mode;
            Type = type;
            //LblHeader.Text = Privileges.PrivilegeName;
            this.Text = "Matrix Maintenance" + " - " + "Assessment Dates"; //Privileges.Program + " - Assessment Dates";
            //fillCombo();
            lblCaseWorker.Visible = false;cmbCaseWorker.Visible = false;
            pnlDate.Visible = false;
            this.pnlDaysInterval.Visible = false;
            this.CaseWorker.Visible = false;
            lblAssessmentDate.Text = "Date";
            pnlCaseandFollup.Visible = false;
            pnlScores.Visible = false;
            pnlassDate.Visible = true;
            pnlCaseworkers.Visible = false;

            this.Size = new Size(300, 450);
            this.pnlAssDateScores.Size = new Size(577,100);
            //this.pnlAssDateScores.Size = new Size(350, 39);//(296, 267)
           // this.gvwAssessmentDetails.Size = new System.Drawing.Size(190, 160);//(274,150);
            //Size = new Size(450, 400);//(299, 339)//255
            picAddAssDate.Visible = true;
            ToolTip Tt = new ToolTip();
            Tt.SetToolTip(picAddAssDate, "Add Date to the Grid");
            GetSelected_MatDates();
            FillMatGridData();
            this.gvwAssessmentDetails.AllowUserToAddRows = false;
            if (Privileges.AddPriv.Equals("false"))
            {
                gvwAssessmentDetails.AllowUserToAddRows = false;
            }
            else
            {
                gvwAssessmentDetails.AllowUserToAddRows = true;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvwAssessmentDetails.Columns["Delete"].Visible = false;
                // gvwAssessmentDetails.Columns[gvwAssessmentDetails.ColumnCount - 2].Width = 200;
            }
            else
            {
                gvwAssessmentDetails.Columns["Delete"].Visible = true;

            }
            btnSave.Text = "&OK";
            //btnSave.Size = new Size(60,25);
            dtAssessmentDate.Enabled = true;


        }

        /* Change Dates for MATASMT Records*/
        public MAT00003AssessmentDate(BaseForm baseForm, PrivilegeEntity privilieges, string MatCode, string Scales_List, string type, string Mode, string Agency, string Depart, string Program, string Year)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            strMatCode = MatCode;
            Matcode = MatCode;
            MatAgy = Agency; MatDept = Depart; MatProg = Program; MatYear = Year;
            Mat_scales = Scales_List;
            strMode = Mode;
            Type = type;
            //LblHeader.Text = Privileges.PrivilegeName;
            this.Text = Privileges.Program + " - Assessment Dates";
            //fillCombo();
            lblCaseWorker.Visible = false;
            cmbCaseWorker.Visible = false;
            this.pnlDaysInterval.Visible = false;
            this.CaseWorker.Visible = false;
            lblAssessmentDate.Text = "Date";
            #region Vikash commented
            //this.lblAssessmentDate.Location = new System.Drawing.Point(20, 186);
            //this.dtAssessmentDate.Size = new System.Drawing.Size(100, 21);
            //this.dtAssessmentDate.Location = new System.Drawing.Point(55, 183);//(55,186)
            //this.pnlAssDateScores.Location = new System.Drawing.Point(1, 4);
            //this.pnlAssDateScores.Size = new System.Drawing.Size(210, 217);//(296, 267)
            //this.gvwAssessmentDetails.Size = new System.Drawing.Size(190, 160);//(274,150);
            //this.gvwAssessmentDetails.Location = new System.Drawing.Point(6, 10);
            //this.Size = new System.Drawing.Size(271, 253);//(299, 339)//255
            //this.Date.Width = 165;
            //this.btnSave.Location = new System.Drawing.Point(20, 225);//(150, 311)
            //this.btnCancel.Location = new System.Drawing.Point(100, 225);
            #endregion
            picAddAssDate.Visible = false;
            //this.pictureBox1.Location = new System.Drawing.Point(163, 183); //(165,186)
            //ToolTip Tt = new ToolTip();
            //Tt.SetToolTip(pictureBox1, "Add date to the Grid");
            FillDatestoGrid();

            this.gvwAssessmentDetails.AllowUserToAddRows = false;
            gvwAssessmentDetails.Columns["Delete"].Visible = false;
            //btnSave.Text = "OK";
            dtAssessmentDate.Enabled = true;


        }



        #region properties
        public BaseForm BaseForm { get; set; }
        public string Matcode { get; set; }
        public string MatAgy { get; set; }
        public string MatDept { get; set; }
        public string MatProg { get; set; }
        public string MatYear { get; set; }
        public string Mat_scales { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        public List<MATDEFDTEntity> Sel_Date_List { get; set; }
        public List<MATDEFEntity> propmatdefEntity { get; set; }
        public List<MATDEFBMEntity> MATADEFBMentity { get; set; }
        public string Type { get; set; }
        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        
        private void fillCombo()
        {
            cmbCaseWorker.Items.Clear();
            cmbCaseWorker.ColorMember = "FavoriteColor";
            List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(strVerfierFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            hierarchyEntity = hierarchyEntity.FindAll(u => u.InActiveFlag == "N");
            string strCaseworker = string.Empty;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {
                if (strCaseworker != caseworker.CaseWorker.ToString())
                {
                    strCaseworker = caseworker.CaseWorker.ToString();
                    cmbCaseWorker.Items.Add(new ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Black : Color.Red));
                }
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseWorkerDefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                }
            }
            cmbCaseWorker.Items.Insert(0, new ListItem("  ", "0"));
            CommonFunctions.SetComboBoxValue(cmbCaseWorker, strCaseWorkerDefaultCode);

        }

        private void SetComboBoxCaseWorkerValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    string strValue = li.Value.ToString();
                    if (strValue.Length > 1)
                    {
                        strValue = strValue.Replace(strValue.Substring(strValue.Length - 1, 1), "");
                    }
                    if (strValue.Trim().Equals(value.Trim()) || Convert.ToString(li.Text).Trim().Equals(value.Trim()))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "&OK")
            {
                this.DialogResult = DialogResult.OK;
                AlertBox.Show("Saved Successfully");
                this.Close();
            }
            else
            {
                if (strMode == "Report")
                {
                    if (Captain.DatabaseLayer.MatrixDB.Updatechangematdates(MatAgy, MatDept, MatProg, MatYear, string.Empty, string.Empty, gvwAssessmentDetails.CurrentRow.Cells["Date"].Value.ToString(), Mat_scales, dtAssessmentDate.Text.ToString(), BaseForm.UserID))
                        this.Close();
                }
                else
                {
                    if (ValidateForm())
                    {
                        string strMsg = string.Empty;
                        MATAPDTSEntity matapdtsEntity = new MATAPDTSEntity();
                        matapdtsEntity.Agency = BaseForm.BaseAgency;
                        matapdtsEntity.Dept = BaseForm.BaseDept;
                        matapdtsEntity.Program = BaseForm.BaseProg;
                        matapdtsEntity.Year = BaseForm.BaseYear;
                        matapdtsEntity.App = BaseForm.BaseApplicationNo;
                        matapdtsEntity.MatCode = strMatCode;

                        matapdtsEntity.SSDate = dtAssessmentDate.Value.ToString();

                        if (!((ListItem)cmbCaseWorker.SelectedItem).Value.ToString().Equals("0"))
                        {
                            matapdtsEntity.SSworker = ((ListItem)cmbCaseWorker.SelectedItem).Value.ToString();
                        }
                        matapdtsEntity.AddOperator = BaseForm.UserID;
                        matapdtsEntity.LstcOperator = BaseForm.UserID;
                        matapdtsEntity.Mode = strMode;

                        matapdtsEntity.FOllowON = string.Empty;
                        matapdtsEntity.FollowCDate = string.Empty;
                        if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().ToUpper() == "Y")
                        {
                            if (dtFollowup.Checked)
                                matapdtsEntity.FOllowON = dtFollowup.Value.ToShortDateString();
                            if (dtCompleted.Checked)
                                matapdtsEntity.FollowCDate = dtCompleted.Value.ToShortDateString();
                        }

                        //txtSub.Text;                
                        bool boolSucess = _model.MatrixScalesData.InsertUpdateDelMatapdts(matapdtsEntity, out strMsg);
                        if (strMsg == "Exists")
                        {
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.Datealreadyexists);
                        }
                        if (boolSucess == true)
                        {
                            FillGridData();
                            if (gvwAssessmentDetails.Rows.Count != 0)
                            {
                                gvwAssessmentDetails.Rows[strIndex].Selected = true;
                                gvwAssessmentDetails.CurrentCell = gvwAssessmentDetails.Rows[strIndex].Cells[1];

                            }
                            gvwAssessmentDetails_SelectionChanged(sender, e);
                        }
                        AlertBox.Show("Saved Successfully");
                    }
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (dtAssessmentDate.Checked == false)
            {
                _errorProvider.SetError(dtAssessmentDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAssessmentDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtAssessmentDate, null);
            }

            if (dtAssessmentDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtAssessmentDate.Text))
                {
                    _errorProvider.SetError(dtAssessmentDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtAssessmentDate, null);
                }
            }

            if (lblReqWorker.Visible == true)
            {
                if ((cmbCaseWorker.SelectedIndex == 0 || ((ListItem)cmbCaseWorker.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbCaseWorker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseWorker.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbCaseWorker, null);
                }
            }

            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().ToUpper() == "Y")
            {
                _errorProvider.SetError(dtCompleted, null);
                _errorProvider.SetError(dtFollowup, null);

                if (lblReqFolUpDte.Visible == true)
                {
                    if (dtFollowup.Checked == false)
                    {
                        _errorProvider.SetError(dtFollowup, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Follow up On Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtFollowup, null);
                    }

                    if (dtFollowup.Checked.Equals(true))
                    {
                        if (string.IsNullOrWhiteSpace(dtFollowup.Text))
                        {
                            _errorProvider.SetError(dtFollowup, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Follow up On Date".Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtFollowup, null);
                        }
                    }
                }

                if (lblReqCompDte.Visible == true)
                {
                    if (dtCompleted.Checked == false)
                    {
                        _errorProvider.SetError(dtCompleted, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Completed Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtCompleted, null);
                    }

                    if (dtCompleted.Checked.Equals(true))
                    {
                        if (string.IsNullOrWhiteSpace(dtCompleted.Text))
                        {
                            _errorProvider.SetError(dtCompleted, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Completed Date".Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtCompleted, null);
                        }
                    }

                    if (dtCompleted.Checked)
                    {
                        if (Convert.ToDateTime(dtCompleted.Value) > DateTime.Now.Date)
                        {
                            _errorProvider.SetError(dtCompleted, "Future Date is not allowed");
                            isValid = false;
                        }
                    }
                }

                //if (dtFollowup.Checked)
                //{
                //    if (Convert.ToDateTime(dtFollowup.Value) > DateTime.Now.Date)
                //    {
                //        _errorProvider.SetError(dtFollowup, "Future Date is not allowed");
                //        isValid = false;
                //    }
                //}

                //if ((dtFollowup.Checked) && (dtCompleted.Checked))
                //{
                //    if (Convert.ToDateTime(dtFollowup.Value) > Convert.ToDateTime(dtCompleted.Value))
                //    {
                //        _errorProvider.SetError(dtCompleted, "Complete Date can’t be less than Follow up On Date");
                //        isValid = false;
                //    }
                //}
                //else
                //{
                    if (dtCompleted.Checked == true && dtFollowup.Checked == false)
                    {
                        _errorProvider.SetError(dtFollowup, "Followup Date is Required");
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(dtFollowup, null);
               // }
            }
            return (isValid);
        }

        private void FillGridData()
        {
            List<MATAPDTSEntity> matapdtsList = _model.MatrixScalesData.GETMatapdts(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, strMatCode, string.Empty, strVerfierFormat);

            gvwAssessmentDetails.Rows.Clear();
            int i = 0;
            foreach (MATAPDTSEntity matapdts in matapdtsList)
            {
                string strAltDate = LookupDataAccess.Getdate(matapdts.SSDate);
                int rowIndex = gvwAssessmentDetails.Rows.Add(strAltDate, matapdts.Name);
                if (i == 0)
                {
                    strMaxDate = strAltDate;
                    i++;
                }
                gvwAssessmentDetails.Rows[rowIndex].Tag = matapdts;
                //gvwAssessmentDetails.ItemsPerPage = 100;
                CommonFunctions.setTooltip(rowIndex, matapdts.AddOperator, matapdts.AddDate, matapdts.LstcOperator, matapdts.DateLstc, gvwAssessmentDetails);
            }
            if(gvwAssessmentDetails.Rows.Count>0)
            {
                gvwAssessmentDetails.Rows[0].Selected = true;
            }
        }

        private void FillMatGridData()
        {
            List<MATDEFDTEntity> MatDefdtList = new List<MATDEFDTEntity>();
            MATDEFDTEntity Search_Entity = new MATDEFDTEntity(true);
            Search_Entity.MatCode = Matcode;
            MatDefdtList = _model.MatrixScalesData.Browse_MATDEFDT(Search_Entity, "Browse");
            gvwAssessmentDetails.Rows.Clear();
            if (strMode == "Edit")
            {
                if (Sel_Date_List.Count > 0)
                {
                    foreach (MATDEFDTEntity dr in Sel_Date_List)
                    {
                        string strDate = LookupDataAccess.Getdate(dr.MatDate);
                        int rowIndex = gvwAssessmentDetails.Rows.Add(strDate, Matcode);
                        gvwAssessmentDetails.Rows[rowIndex].Tag = MatDefdtList;
                       // gvwAssessmentDetails.ItemsPerPage = 100;
                    }
                }
                else
                {
                    foreach (MATDEFDTEntity Entity in MatDefdtList)
                    {
                        foreach (MATDEFDTEntity dr in Sel_Date_List)
                        {
                            if (Entity.MatDate == dr.MatDate)
                            {
                                string strDate = LookupDataAccess.Getdate(Entity.MatDate);
                                int rowIndex = gvwAssessmentDetails.Rows.Add(strDate, Matcode);
                                gvwAssessmentDetails.Rows[rowIndex].Tag = MatDefdtList;
                               // gvwAssessmentDetails.ItemsPerPage = 100;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (MATDEFDTEntity dr in Sel_Date_List)
                {
                    string strDate = LookupDataAccess.Getdate(dr.MatDate);
                    int rowIndex = gvwAssessmentDetails.Rows.Add(strDate, Matcode);
                    gvwAssessmentDetails.Rows[rowIndex].Tag = MatDefdtList;
                   // gvwAssessmentDetails.ItemsPerPage = 100;
                }
            }


        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (strMode == string.Empty)
            {
                if (txtIntervalIndays.Text != "")
                {
                    if (Convert.ToInt32(txtIntervalIndays.Text) > 0)
                    {
                        if (strMaxDate != string.Empty)
                            dtAssessmentDate.Value = Convert.ToDateTime(strMaxDate).AddDays(Convert.ToInt32(txtIntervalIndays.Text));
                        else
                            dtAssessmentDate.Value = DateTime.Now.AddDays(Convert.ToInt32(txtIntervalIndays.Text));

                        //foreach (DataGridViewRow item in gvwAssessmentDetails.Rows)
                        //{
                        //    if (item.Selected)
                        //    {
                        //        if (!string.IsNullOrEmpty(Convert.ToString(gvwAssessmentDetails.SelectedRows[0].Cells["Date"].Value)))
                        //            dtAssessmentDate.Value = Convert.ToDateTime(gvwAssessmentDetails.SelectedRows[0].Cells["Date"].Value).AddDays(Convert.ToInt32(txtIntervalIndays.Text));
                        //    }
                        //}
                    }
                    else
                        CommonFunctions.MessageBoxDisplay("Interval must be greater than zero");
                }
            }

        }

        private void gvwAssessmentDetails_SelectionChanged(object sender, EventArgs e)
        {
            if (Type == "Matrix")
            {
                if (gvwAssessmentDetails.Rows.Count > 0)
                {

                }
                else
                    dtAssessmentDate.Checked = false;
            }
            else
            {
                cmbCaseWorker.SelectedIndexChanged -= new EventHandler(cmbCaseWorker_SelectedIndexChanged);
                if (gvwAssessmentDetails.Rows.Count > 0)
                {
                    strMode = string.Empty;
                    txtTotalScore.Text = string.Empty;
                    txtPatialScore.Text = string.Empty;
                    lblTotalScoreDesc.Text = string.Empty;
                    lblParitalScoreDesc.Text = string.Empty;
                    if (gvwAssessmentDetails.SelectedRows.Count > 0)
                    {
                        if (gvwAssessmentDetails.SelectedRows[0].Tag is MATAPDTSEntity)
                        {

                            MATAPDTSEntity row = gvwAssessmentDetails.SelectedRows[0].Tag as MATAPDTSEntity;
                            strIndex = gvwAssessmentDetails.SelectedRows[0].Index;
                            if (row != null)
                            {
                                dtAssessmentDate.Checked = true;
                                dtAssessmentDate.Text = LookupDataAccess.Getdate(row.SSDate.ToString());
                                strCaseWorkerDefaultCode = row.SSworker;
                                if (strCaseWorkerDefaultCode != string.Empty || strCaseWorkerDefaultCode != "0")
                                    CommonFunctions.SetComboBoxValue(cmbCaseWorker, strCaseWorkerDefaultCode);
                                else
                                    CommonFunctions.SetComboBoxValue(cmbCaseWorker, "0");
                                // SetComboBoxCaseWorkerValue(cmbCaseWorker, row.SSworker);
                                // txtIncSource.Text = caseIncomeList[0].Source;


                               

                                strMode = "Edit";

                                //strIncomeIndex = dataGridCaseIncome.SelectedRows[0].Index;
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    EnableAllcontrols();
                                }
                                else
                                {
                                    EnableAllcontrols(true);
                                    dtAssessmentDate.Enabled = false;
                                }
                                if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().Trim().ToUpper() == "Y")
                                {
                                    if (row.FOllowON != string.Empty)
                                    {
                                        dtFollowup.Value = Convert.ToDateTime(row.FOllowON);
                                        dtFollowup.Checked = true;
                                    }
                                    if (row.FollowCDate != string.Empty)
                                    {
                                        dtCompleted.Value = Convert.ToDateTime(row.FollowCDate);
                                        dtCompleted.Checked = true;
                                    }
                                }
                                if (row.TotalScore != string.Empty)
                                {
                                    txtTotalScore.Text = row.TotalScore.ToString();
                                    if (Convert.ToInt32(row.TotalScore) > 0)
                                    {
                                        List<MATDEFBMEntity> matfilterentity = MATADEFBMentity.FindAll(u => u.Overall_Low != string.Empty && u.Overall_High != string.Empty);
                                        MATDEFBMEntity MatadefrowEntity = matfilterentity.Find(u => Convert.ToInt32(u.Overall_Low) <= Convert.ToInt32(row.TotalScore) && Convert.ToInt32(row.TotalScore) <= Convert.ToInt32(u.Overall_High));
                                        if (MatadefrowEntity != null)
                                        {
                                            lblTotalScoreDesc.Text = MatadefrowEntity.Desc;
                                        }
                                    }
                                }
                                if (row.PartialScore != string.Empty)
                                {
                                    txtPatialScore.Text = row.PartialScore.ToString();
                                    if (Convert.ToInt32(row.PartialScore) > 0)
                                    {
                                        List<MATDEFBMEntity> matfilterentity = MATADEFBMentity.FindAll(u => u.Overall_Low != string.Empty && u.Overall_High != string.Empty);
                                        MATDEFBMEntity MatadefrowEntity = matfilterentity.Find(u => Convert.ToInt32(u.Overall_Low) <= Convert.ToInt32(row.PartialScore) && Convert.ToInt32(row.PartialScore) <= Convert.ToInt32(u.Overall_High));
                                        if (MatadefrowEntity != null)
                                        {
                                            lblParitalScoreDesc.Text = MatadefrowEntity.Desc;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            strMode = string.Empty;
                            dtAssessmentDate.Checked = false;
                            CommonFunctions.SetComboBoxValue(cmbCaseWorker, strCaseWorkerDefaultStartCode);
                            if (Privileges.AddPriv.Equals("false"))
                            {
                                EnableAllcontrols();
                            }
                            else
                            {
                                EnableAllcontrols(true);
                            }
                        }
                    }
                    else
                    {
                        strMode = string.Empty;
                        dtAssessmentDate.Checked = false;
                        CommonFunctions.SetComboBoxValue(cmbCaseWorker, strCaseWorkerDefaultStartCode);
                        if (Privileges.AddPriv.Equals("false"))
                        {
                            EnableAllcontrols();
                        }
                        else
                        {
                            EnableAllcontrols(true);
                        }
                    }
                }
                else
                {
                    strMode = string.Empty;
                    dtAssessmentDate.Checked = false;
                    CommonFunctions.SetComboBoxValue(cmbCaseWorker, strCaseWorkerDefaultStartCode);
                    if (Privileges.AddPriv.Equals("false"))
                    {
                        EnableAllcontrols();
                    }
                    else
                    {
                        EnableAllcontrols(true);
                    }
                    strMode = string.Empty;
                }

                if (Privileges.ChangePriv.Equals("false"))
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
                cmbCaseWorker.SelectedIndexChanged += new EventHandler(cmbCaseWorker_SelectedIndexChanged);
                dtAssessmentDate.Focus();
            }
        }


        private void cmbCaseWorker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbCaseWorker.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbCaseWorker.SelectedItem).ID.ToString() != "N")
                    MessageBox.Show("Inactive CaseWorker", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void EnableAllcontrols()
        {

            cmbCaseWorker.Enabled = false;
            dtAssessmentDate.Enabled = false;
            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().Trim().ToUpper() == "Y")
            {            
                    dtFollowup.Enabled = false;     
                    dtCompleted.Enabled = false;
            }
            btnSave.Visible = false;
            btnCancel.Text = "&Close";
        }

        private void EnableAllcontrols(bool booltrue)
        {
            cmbCaseWorker.Enabled = booltrue;
            dtAssessmentDate.Enabled = booltrue;
            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().Trim().ToUpper() == "Y")
            {
                dtFollowup.Value = DateTime.Now.Date;
                dtCompleted.Value = DateTime.Now.Date;
                dtFollowup.Checked = false;
                dtCompleted.Checked = false;
               dtFollowup.Enabled = booltrue;
                dtCompleted.Enabled = booltrue;
            }
            btnSave.Visible = booltrue;
            btnCancel.Text = "&Close";
        }

        private void gvwAssessmentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvwAssessmentDetails.ColumnCount - 2 && e.RowIndex != -1)
            {
                if (gvwAssessmentDetails.SelectedRows.Count > 0)
                {
                    if (Type == "Matrix")
                    {
                        if (gvwAssessmentDetails.Rows.Count > 1)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: OnDeleteMessageBoxClicked);
                        }

                    }
                    else
                    {
                        if (gvwAssessmentDetails.SelectedRows[0].Tag is MATAPDTSEntity)
                        {
                            MATAPDTSEntity row = gvwAssessmentDetails.SelectedRows[0].Tag as MATAPDTSEntity;
                            if (row != null)
                            {
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: OnDeleteMessageBoxClicked);

                            }
                        }
                    }
                }
            }
        }


        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
           // MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
                if (Type == "Matrix")
                {
                    string strmsg = string.Empty;
                    if (strMode == "Add")
                    {
                        int rowIndex = gvwAssessmentDetails.CurrentRow.Cells["Date"].RowIndex;
                        gvwAssessmentDetails.Rows.RemoveAt(rowIndex);
                    }
                    else
                    {
                        List<MATDEFDTEntity> Matdeflist = new List<MATDEFDTEntity>();
                        MATDEFDTEntity defEntity = new MATDEFDTEntity(true);
                        defEntity.MatCode = strMatCode;
                        defEntity.MatDate = gvwAssessmentDetails.CurrentRow.Cells["Date"].Value.ToString();
                        Matdeflist = _model.MatrixScalesData.Browse_MATDEFDT(defEntity, "Browse");
                        if (Matdeflist.Count > 0)
                        {
                            defEntity.Rec_Type = "D";
                            defEntity.MatCode = strMatCode;
                            int rowIndex = gvwAssessmentDetails.CurrentRow.Cells["Date"].RowIndex;
                            defEntity.MatDate = gvwAssessmentDetails.CurrentRow.Cells["Date"].Value.ToString();
                            //defEntity.MatDate = gvwAssessmentDetails.SelectedRows[0].Cells["Date"].Value.ToString();
                            //gvwAssessmentDetails.Rows.RemoveAt(rowIndex);
                            bool boolsuccess = _model.MatrixScalesData.UpdateMATDEFDT(defEntity, "Update", out strmsg);
                            if (boolsuccess == true)
                            {
                                gvwAssessmentDetails.Rows.RemoveAt(rowIndex);
                            }
                            //    if (strIndex != 0)
                            //        strIndex = strIndex - 1;
                            //    GetSelected_MatDates();
                            //    FillMatGridData();
                            //}
                            //else
                            //{

                            //}
                        }
                        else
                        {
                            int rowIndex = gvwAssessmentDetails.CurrentRow.Cells["Date"].RowIndex;
                            gvwAssessmentDetails.Rows.RemoveAt(rowIndex);
                        }
                    }
                }
                else
                {
                    CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                    string strMsg = string.Empty;
                    MATAPDTSEntity matapdtsEntity = new MATAPDTSEntity();
                    matapdtsEntity.Agency = BaseForm.BaseAgency;
                    matapdtsEntity.Dept = BaseForm.BaseDept;
                    matapdtsEntity.Program = BaseForm.BaseProg;
                    matapdtsEntity.Year = BaseForm.BaseYear;
                    matapdtsEntity.App = BaseForm.BaseApplicationNo;
                    matapdtsEntity.MatCode = strMatCode;
                    matapdtsEntity.SSDate = gvwAssessmentDetails.SelectedRows[0].Cells["Date"].Value.ToString();
                    matapdtsEntity.AddOperator = BaseForm.UserID;
                    matapdtsEntity.LstcOperator = BaseForm.UserID;
                    matapdtsEntity.Mode = "Delete";

                    bool boolSucess = _model.MatrixScalesData.InsertUpdateDelMatapdts(matapdtsEntity, out strMsg);
                    if (strMsg == "Exists")
                    {
                        CommonFunctions.MessageBoxDisplay("Applicant Assessment date can't be deleted");
                    }
                    if (boolSucess == true)
                    {

                        if (strIndex != 0)
                            strIndex = strIndex - 1;
                        FillGridData();
                        if (gvwAssessmentDetails.Rows.Count != 0)
                        {
                            gvwAssessmentDetails.Rows[strIndex].Selected = true;
                            gvwAssessmentDetails.CurrentCell = gvwAssessmentDetails.Rows[strIndex].Cells[1];

                        }
                        gvwAssessmentDetails_SelectionChanged(gvwAssessmentDetails, EventArgs.Empty);

                    }
                }
            }
        }

        private void MAT00003AssessmentDate_Load(object sender, EventArgs e)
        {
            if (Type == "Matrix")
            {
            }
            else
            {
                gvwAssessmentDetails_SelectionChanged(sender, e);
                txtIntervalIndays.Focus();
            }
        }

        List<MATDEFEntity> MATDEF_List = new List<MATDEFEntity>();
        private void FillDatestoGrid()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Mat_Code = strMatCode; Search_Entity.Scale_Code = "0";
            MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            string Assdate = string.Empty;
            if (MATDEF_List.Count > 0)
                Assdate = MATDEF_List[0].Date_option.Trim();

            if (Assdate == "A")
            {
                List<MATAPDTSEntity> matapdtsList = _model.MatrixScalesData.GETMatapdts(MatAgy, MatDept, MatProg, MatYear, string.Empty, strMatCode, string.Empty, string.Empty);
                if (matapdtsList.Count > 0)
                {
                    var matdatesList = matapdtsList.Select(u => u.SSDate).Distinct().ToList();
                    gvwAssessmentDetails.Rows.Clear();
                    int i = 0;
                    foreach (var matapdts in matdatesList)
                    {
                        string strDate = LookupDataAccess.Getdate(matapdts);
                        int rowIndex = gvwAssessmentDetails.Rows.Add(strDate, strMatCode);
                        gvwAssessmentDetails.Rows[rowIndex].Tag = matdatesList;
                        //gvwAssessmentDetails.ItemsPerPage = 100;
                        //    string strAltDate = LookupDataAccess.Getdate(matapdts.SSDate);
                        //    int rowIndex = gvwAssessmentDetails.Rows.Add(strAltDate, matapdts.Name);
                        //    if (i == 0)
                        //    {
                        //        strMaxDate = strAltDate;
                        //        i++;
                        //    }
                        //    gvwAssessmentDetails.Rows[rowIndex].Tag = matapdts;
                        //    gvwAssessmentDetails.ItemsPerPage = 100;
                        //    CommonFunctions.setTooltip(rowIndex, matapdts.AddOperator, matapdts.AddDate, matapdts.LstcOperator, matapdts.DateLstc, gvwAssessmentDetails);
                    }
                }
            }
            else if (Assdate == "M")
            {
                List<MATDEFDTEntity> MatDefdtList = new List<MATDEFDTEntity>();
                MATDEFDTEntity Search1_Entity = new MATDEFDTEntity(true);
                Search1_Entity.MatCode = strMatCode;
                MatDefdtList = _model.MatrixScalesData.Browse_MATDEFDT(Search1_Entity, "Browse");

                if (MatDefdtList.Count > 0)
                {
                    var Matdefdate = MatDefdtList.Select(u => u.MatDate).Distinct().ToList();
                    gvwAssessmentDetails.Rows.Clear();
                    foreach (var Entity in Matdefdate)
                    {
                        string strDate = LookupDataAccess.Getdate(Entity);
                        int rowIndex = gvwAssessmentDetails.Rows.Add(strDate, strMatCode);
                        gvwAssessmentDetails.Rows[rowIndex].Tag = Matdefdate;
                        //gvwAssessmentDetails.ItemsPerPage = 100;
                    }
                }


            }

        }

        public List<MATDEFDTEntity> GetSelected_MatDates()
        {
            List<MATDEFDTEntity> Sel_Date_List = new List<MATDEFDTEntity>();
            MATDEFDTEntity Add_Entity = new MATDEFDTEntity();
            string Datelist = string.Empty;
            if (gvwAssessmentDetails.Rows.Count > 1)
            {
                foreach (DataGridViewRow dr in gvwAssessmentDetails.Rows)
                {
                    string strvalue = dr.Cells["date"].Value == null ? string.Empty : dr.Cells["date"].Value.ToString().Trim();
                    //Add_Entity.Rec_Type = "I";

                    if (!string.IsNullOrEmpty(strvalue))
                    {
                        Add_Entity.MatCode = dr.Cells["CaseWorker"].Value.ToString();
                        Add_Entity.MatDate = dr.Cells["Date"].Value.ToString();
                        Sel_Date_List.Add(new MATDEFDTEntity(Add_Entity));
                        Datelist += dr.Cells["Date"].Value.ToString();
                    }
                }
            }
            return Sel_Date_List;
        }
        bool First = true;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (dtAssessmentDate.Checked == true)
            {
                string strdate = LookupDataAccess.Getdate(dtAssessmentDate.Text);
                bool boolValue = true;
                foreach (DataGridViewRow dr in gvwAssessmentDetails.Rows)
                {
                    string strvalue = dr.Cells["date"].Value == null ? string.Empty : dr.Cells["date"].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(strvalue))
                    {
                        if (dr.Cells["Date"].Value.ToString() == strdate)
                        {
                            MessageBox.Show("Already added", "CAP Systems");
                            boolValue = false;
                        }
                    }
                }
                if (boolValue)
                {
                    gvwAssessmentDetails.Rows.Add(strdate, strMatCode);
                    First = false;
                }

            }
            dtAssessmentDate.Checked = true;
        }

        public List<FldcntlHieEntity> FLDCNTLHieEntity
        {
            get
            {
                return _fldCntlHieEntity;
            }
            set
            {
                _fldCntlHieEntity = value;
            }
        }
        public List<FldcntlHieEntity> CntlEntity
        {
            get; set;
        }

        private void FieldcontrolCheck()
        {
            string HIE = strAgency + strDept + strProgram;
            CntlEntity = _model.FieldControls.GetFLDCNTLHIE("MAT00003", HIE, "FLDCNTL");
            FLDCNTLHieEntity = CntlEntity;

            foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
            {
                bool required = entity.Req.Equals("Y") ? true : false;
                bool enabled = entity.Enab.Equals("Y") ? true : false;

                switch (entity.FldCode)
                {
                    case Consts.MatrixAssDates.AssementDte:
                        if (enabled)
                        {
                            dtAssessmentDate.Enabled = true;
                            if (required)
                            {
                                lblEjobTitleReq.Visible = true;
                            }
                        }
                        else
                        {
                            dtAssessmentDate.Enabled = false;
                            lblEjobTitleReq.Visible = false;
                            _errorProvider.SetError(dtAssessmentDate, null);
                        }
                        break;
                    case Consts.MatrixAssDates.CaseWorker:
                        if (enabled)
                        {
                            cmbCaseWorker.Enabled = true;
                            if (required)
                            {
                                lblReqWorker.Visible = true;
                            }
                        }
                        else
                        {
                            cmbCaseWorker.Enabled = false;
                            lblReqWorker.Visible = false;
                            _errorProvider.SetError(cmbCaseWorker, null);
                        }
                        break;
                    case Consts.MatrixAssDates.FollowUpOnDte:
                        if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().ToUpper() == "Y")
                        {
                            if (enabled)
                            {
                                dtFollowup.Enabled = true;
                                if (required)
                                {
                                    lblReqFolUpDte.Visible = true;
                                }

                            }
                            else
                            {
                                dtFollowup.Enabled = false;
                                lblReqFolUpDte.Visible = false;
                                _errorProvider.SetError(dtFollowup, null);
                            }
                        }
                        break;

                    case Consts.MatrixAssDates.CompleteDte:
                        if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().ToUpper() == "Y")
                        {
                            if (enabled)
                            {
                                dtCompleted.Enabled = true;
                                if (required)
                                {
                                    lblReqCompDte.Visible = true;
                                }
                            }
                            else
                            {
                                dtCompleted.Enabled = false;
                                lblReqCompDte.Visible = false;
                                _errorProvider.SetError(dtCompleted, null);
                            }
                        }
                        break;
                }
            }
        }
    }
}