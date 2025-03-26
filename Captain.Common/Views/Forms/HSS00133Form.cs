#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls;
//using Gizmox.WebGUI.Common.Interfaces;
using Captain.DatabaseLayer;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00133Form : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public HSS00133Form(BaseForm baseForm, string mode, string strComponent, PrivilegeEntity privilegeEntity, ChldTrckEntity trackEntity, string strFormType)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            propComponent = strComponent;
           // lblHeader.Text = "Task Master Maintenance";// privilegeEntity.PrivilegeName;
            propTrackEntity = trackEntity;
            this.Text = "Task Master Maintenance" + " - " + Consts.Common.Add;
            _model = new CaptainModel();
            // RemoveEventHandler();
            FillDroupdowns();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            //txtTrack.Validator = TextBoxValidation.IntegerValidator;

            if (Mode.Equals("Add"))
            {
                propNewcustomQues = string.Empty;
                propQuestionE = "Y";
                propQuestionR = "N";
                propCaseNotesE = "Y";
                propCaseNotesR = "N";
                PropSBCBE = "Y";
                PropSBCBR = "N";
                PropAddressE = "Y";
                PropAddressR = "Y";
                PropCompletE = "Y";
                PropCompletR = "N";
                PropFollowupE = "Y";
                PropFollowupR = "N";
                PropFollowupCE = "Y";
                PropFollowupCR = "N";
                PropDiagnosedE = "Y";
                PropDiagnosedR = "N";
                PropSSRE = "Y";
                PropSSRR = "N";
                PropAgencyRefE = "Y";
                PropAgencyRefR = "N";
                PropFundE = "Y";
                PropFundR = "N";
                gvwQuestion.AllowUserToAddRows = true;

            }
            else
            {
                propQuestionE = trackEntity.QUESTIONE;
                propQuestionR = trackEntity.QUESTIONR;
                propCaseNotesE = trackEntity.CASENOTESE;
                propCaseNotesR = trackEntity.CASENOTESR;
                PropSBCBE = trackEntity.SBCBE;
                PropSBCBR = trackEntity.SBCBR;
                PropAddressE = trackEntity.ADDRESSE;
                PropAddressR = trackEntity.ADDRESSR;
                PropCompletE = trackEntity.COMPLETEE;
                PropCompletR = trackEntity.COMPLETER;
                PropFollowupE = trackEntity.FOLLOWUPE;
                PropFollowupR = trackEntity.FOLLOWUPR;
                PropFollowupCE = trackEntity.FOLLOWUPCE;
                PropFollowupCR = trackEntity.FOLLOWUPCR;
                PropDiagnosedE = trackEntity.DIAGNOSEE;
                PropDiagnosedR = trackEntity.DIAGNOSER;
                PropSSRE = trackEntity.SSRE;
                PropSSRR = trackEntity.SSRR;
                PropAgencyRefE = trackEntity.WHEREE;
                PropAgencyRefR = trackEntity.WHERER;
                PropFundE = trackEntity.FUNDE;
                PropFundR = trackEntity.FUNDR;
            }

            if (Mode.Equals("Edit"))
            {
                // txtEmployNumber.Enabled = false;
                this.Text = "Task Master Maintenance"+ " - " + Consts.Common.Edit;
                txtTrack.Enabled = false;
                FillAllControls();
            }
            else if (Mode.Equals("View"))
            {
                // DisableControls();

                this.Text = "Task Master Maintenance" + " - " + Consts.Common.View;
                txtTrack.Enabled = false;
                txtTrackDesc.Enabled = false;
                cmbOwn1.Enabled = false;
                cmbGrowthChart.Enabled = false;
                cmbOwn3.Enabled = false;
                // gvwQuestion.Enabled = false;
                gvwQuestion.ReadOnly = true;
                FillAllControls();
                btnSave.Visible = false;
                btnCancel.Visible = false;
                gvwTrakrdetails.ReadOnly = true;
                //  gvwTrakrdetails.Enabled = false;
                // this.Size = new System.Drawing.Size(685, 424);


            }


        }

        private void FillDroupdowns()
        {
            gvwTrakrdetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
            DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.gvwTrakrdetails.Columns["gvNextTask"];
            DataGridViewComboBoxColumn cbIntake = (DataGridViewComboBoxColumn)this.gvwTrakrdetails.Columns["gvIntakeEntry1"];


            List<CommonEntity> NewTaskList = new List<CommonEntity>();
            NewTaskList.Add(new CommonEntity("0", "  "));
            List<ChldTrckEntity> chldtrackList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
            foreach (ChldTrckEntity item in chldtrackList)
            {
                NewTaskList.Add(new CommonEntity(item.TASK, item.TASK.Trim() + "-" + item.TASKDESCRIPTION.Trim()));
            }

            // List<CommonEntity> commonInterval = _model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.COMPONENTS);
            cb.DataSource = NewTaskList;
            cb.DisplayMember = "DESC";
            cb.ValueMember = "CODE";
            cb = (DataGridViewComboBoxColumn)this.gvwTrakrdetails.Columns["gvNextTask"];


            List<CommonEntity> incomeInterval = new List<CommonEntity>();
            incomeInterval.Add(new CommonEntity("0", "    "));
            incomeInterval.Add(new CommonEntity("E", "From Date of Entry"));
            incomeInterval.Add(new CommonEntity("A", "From Date of Intake"));
            incomeInterval.Add(new CommonEntity("D", "From Date of Birth"));
            incomeInterval.Add(new CommonEntity("N", "No Next Action"));
            cbIntake.DataSource = incomeInterval;
            cbIntake.DisplayMember = "DESC";
            cbIntake.ValueMember = "CODE";
            cbIntake = (DataGridViewComboBoxColumn)this.gvwTrakrdetails.Columns["gvIntakeEntry1"];


            //List<CommonEntity> AgyCommon_List = _model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.ChldTrck.GrowChartType);
            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.ChldTrck.GrowChartType, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            commonEntity = CommonFunctions.AgyTabsDecisionCodeFilters(commonEntity, Consts.ChldTrck.GrowChartType, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);

            cmbGrowthChart.ColorMember = "FavoriteColor";
            foreach (CommonEntity Entity in commonEntity)
            {
                cmbGrowthChart.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y")) ? Color.Black : Color.Red));
            }
            if (cmbGrowthChart.Items.Count > 0)
                cmbGrowthChart.SelectedIndex = 0;

            // List<CommonEntity> AgyCommon_List = _model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.COMPONENTS);
            // foreach (CommonEntity Entity in AgyCommon_List)
            // {

            //     cmbOwn1.Items.Add(new ListItem(Entity.Code + " - " + Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y")) ? Color.Green : Color.Red));
            //    // cmbGrowthChart.Items.Add(new ListItem(Entity.Code + " - " + Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y")) ? Color.Green : Color.Red));
            //     cmbOwn3.Items.Add(new ListItem(Entity.Code + " - " + Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y")) ? Color.Green : Color.Red));

            // }
            // cmbOwn1.Items.Insert(0, new ListItem("All", "****", "Y", Color.Green));
            // cmbOwn1.Items.Insert(0, new ListItem("None", "0", " ", Color.White));
            //// cmbGrowthChart.Items.Insert(0, new ListItem("All", "****", "Y", Color.Green));
            //// cmbGrowthChart.Items.Insert(0, new ListItem("None", "0", " ", Color.White));
            // cmbOwn3.Items.Insert(0, new ListItem("All", "****", "Y", Color.Green));
            // cmbOwn3.Items.Insert(0, new ListItem("None", "0", " ", Color.White));
            // //CmbComponents.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
            // cmbOwn1.SelectedIndex =  cmbOwn3.SelectedIndex = 0;

            List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            AgyFundList = filterByHIE(AgyFundList);
            foreach (CommonEntity Entity in AgyFundList)
            {
                int rowindex = gvwTrakrdetails.Rows.Add(Entity.Code, Entity.Desc, "0", string.Empty, false, false, "0", string.Empty);
                if (Entity.Active != "Y")
                {
                    gvwTrakrdetails.Rows[rowindex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            custQuestions = _model.FieldControls.GetCustomQuestions(Privileges.Program, "*", string.Empty, string.Empty, string.Empty, string.Empty);

            //if (!Mode.Equals("Edit"))
            //{
            //    foreach (CustomQuestionsEntity entityQuestion in custQuestions)
            //    {
            //        int rowIndex = gvwQuestion.Rows.Add(false, entityQuestion.CUSTDESC.Trim(), entityQuestion.CUSTCODE);
            //        gvwQuestion.Rows[rowIndex].Tag = entityQuestion;
            //        //CommonFunctions.setTooltip(rowIndex, entityQuestion.addoperator, entityQuestion.adddate, entityQuestion.lstcoperator, entityQuestion.lstcdate, gvwQuestion);

            //    }
            //}

            //CmbResponse.Items.Clear();
            //List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            //listItem.Add(new ListItem("   ", "NN"));
            //listItem.Add(new ListItem("Drop Down", "DD"));
            //listItem.Add(new ListItem("Other", "OT"));
            //CmbResponse.Items.AddRange(listItem.ToArray());
            //CmbResponse.SelectedIndex = 0;


            //Config_List.Add(new Captain.Common.Utilities.ListItem("Question", "1"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Casenotes", "2"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("SBCB", "3"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Addressed", "4"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Completed", "5"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Followup", "6"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Followup Completed", "7"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Diagnosed", "8"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Special Service Referral", "9"));
            //Config_List.Add(new Captain.Common.Utilities.ListItem("Agency Referral", "10"));

            //foreach (ListItem List in Config_List)
            //{
            //    Gd_Config.Rows.Add(List.Text, false, false);
            //}

            if (gvwTrakrdetails.Rows.Count > 0) {
                gvwTrakrdetails.Rows[0].Selected = true;
            }

            gvwTrakrdetails.CellValueChanged += new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            //if (LookupValues.Exists(u => u.Hierarchy.Equals(HIE)))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(HIE)).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")).ToList();
            //else
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();

            return LookupValues;
        }

        private void FillAllControls()
        {
            if (propTrackEntity != null)
            {
                gvwTrakrdetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
                List<ChldTrckREntity> chldtrackRList = _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", propTrackEntity.TASK, string.Empty);
                foreach (ChldTrckREntity chldtrackitem in chldtrackRList)
                {
                    foreach (DataGridViewRow gvitem in gvwTrakrdetails.Rows)
                    {
                        if (gvitem.Cells["gvFundCode"].Value.ToString().Trim().ToUpper() == chldtrackitem.FUND.ToString().Trim().ToUpper())
                        {
                            if (chldtrackitem.INTAKEENTRY == "N" || string.IsNullOrEmpty(chldtrackitem.INTAKEENTRY))
                            {

                                gvitem.Cells["gvEntryDays"].ReadOnly = true;
                                gvitem.Cells["gvYear"].ReadOnly = true;
                                gvitem.Cells["gvEntryDays"].Value = string.Empty;
                                gvitem.Cells["gvYear"].Value = false;

                            }
                            else
                            {
                                gvitem.Cells["gvEntryDays"].ReadOnly = false;
                                gvitem.Cells["gvYear"].ReadOnly = false;
                            }
                            gvitem.Cells["gvIntakeEntry1"].Value = chldtrackitem.INTAKEENTRY;
                            // gvitem.Cells["gvNAcation"].Value = chldtrackitem.NXTACTION=="Y"? true : false;
                            if (chldtrackitem.NXTACTION.Equals("Y"))
                            {
                                gvitem.Cells["gvNextTask"].ReadOnly = false;
                                gvitem.Cells["gvNextDays"].ReadOnly = false;
                                gvitem.Cells["gvNAcation"].Value = true;

                            }
                            else
                            {
                                gvitem.Cells["gvNAcation"].Value = false;
                                //gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNAcation"].Value = true;
                                gvitem.Cells["gvNextTask"].ReadOnly = true;
                                gvitem.Cells["gvNextDays"].ReadOnly = true;
                                //  gvitem.Cells["gvNextTask"].Value = "0";
                                //   gvitem.Cells["gvNextDays"].Value = string.Empty;
                            }
                            gvitem.Cells["gvYear"].Value = chldtrackitem.REQUIREYEAR.Equals("Y") ? true : false;
                            gvitem.Cells["gvNextDays"].Value = chldtrackitem.NEXTDAYS;
                            gvitem.Cells["gvEntryDays"].Value = chldtrackitem.ENTERDAYS;
                            gvitem.Cells["gvNextTask"].Value = chldtrackitem.NEXTTASK;
                            CommonFunctions.setTooltip(gvitem.Index, chldtrackitem.ADDOPERATOR, chldtrackitem.DATEADD, chldtrackitem.LSTCOPERATOR, chldtrackitem.DATELSTC, gvwTrakrdetails);
                            gvwTrakrdetails.Rows[gvitem.Index].Tag = chldtrackitem;
                            break;
                        }
                    }
                }

                gvwTrakrdetails.CellValueChanged += new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);

                txtTrack.Text = propTrackEntity.TASK;
                txtTrackDesc.Text = propTrackEntity.TASKDESCRIPTION;
                if (cmbGrowthChart.Items.Count > 0)
                {
                    CommonFunctions.SetComboBoxValue(cmbGrowthChart, propTrackEntity.GCHARTCODE);
                }
                //CommonFunctions.SetComboBoxValue(cmbOwn2, propTrackEntity.COMPONENTOWNER2);
                //CommonFunctions.SetComboBoxValue(cmbOwn3, propTrackEntity.COMPONENTOWNER3);

                fillCustQuestions(propTrackEntity.CustQCodes);
                propNewcustomQues = propTrackEntity.CustQCodes;

            }
        }

        void gvwTrakrdetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvIntakeEntry1.Index)
            {
                disableenableIntakefields(e.RowIndex);
            }
            if (e.ColumnIndex == gvNextDays.Index)
            {

                int introwindex = gvwTrakrdetails.CurrentCell.RowIndex;
                int intcolumnindex = gvwTrakrdetails.CurrentCell.ColumnIndex;
                string strNextValue = Convert.ToString(gvwTrakrdetails.Rows[introwindex].Cells["gvNextDays"].Value);
                if (!System.Text.RegularExpressions.Regex.IsMatch(strNextValue, Consts.StaticVars.NumericString))
                {
                    gvwTrakrdetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
                    gvwTrakrdetails.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                    gvwTrakrdetails.CellValueChanged += new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);

                }
            }
            if (e.ColumnIndex == gvEntryDays.Index)
            {
                int introwindex = gvwTrakrdetails.CurrentCell.RowIndex;
                int intcolumnindex = gvwTrakrdetails.CurrentCell.ColumnIndex;
                string strEntryDaysValue = Convert.ToString(gvwTrakrdetails.Rows[introwindex].Cells["gvEntryDays"].Value);
                if (!System.Text.RegularExpressions.Regex.IsMatch(strEntryDaysValue, Consts.StaticVars.NumericString))
                {
                    gvwTrakrdetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
                    gvwTrakrdetails.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                    gvwTrakrdetails.CellValueChanged += new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);

                }

            }
        }

        private void disableenableIntakefields(int rowindex)
        {
            gvwTrakrdetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
            if (gvwTrakrdetails.Rows[rowindex].Cells["gvIntakeEntry1"].Value == "N" || gvwTrakrdetails.Rows[rowindex].Cells["gvIntakeEntry1"].Value == null)
            {
                gvwTrakrdetails.Rows[rowindex].Cells["gvEntryDays"].ReadOnly = true;
                gvwTrakrdetails.Rows[rowindex].Cells["gvYear"].ReadOnly = true;
                gvwTrakrdetails.Rows[rowindex].Cells["gvEntryDays"].Value = string.Empty;
                gvwTrakrdetails.Rows[rowindex].Cells["gvYear"].Value = false;

            }
            else
            {
                gvwTrakrdetails.Rows[rowindex].Cells["gvEntryDays"].ReadOnly = false;
                gvwTrakrdetails.Rows[rowindex].Cells["gvYear"].ReadOnly = false;
            }
            gvwTrakrdetails.Update();
            //gvwTrakrdetails.RefreshEdit();
            gvwTrakrdetails.CellValueChanged += new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        public string propComponent { get; set; }

        public string propQuestionE { get; set; }
        public string propQuestionR { get; set; }
        public string propCaseNotesE { get; set; }
        public string propCaseNotesR { get; set; }
        public string PropSBCBE { get; set; }
        public string PropSBCBR { get; set; }
        public string PropAddressE { get; set; }
        public string PropAddressR { get; set; }
        public string PropCompletE { get; set; }
        public string PropCompletR { get; set; }
        public string PropFollowupE { get; set; }
        public string PropFollowupR { get; set; }
        public string PropFollowupCE { get; set; }
        public string PropFollowupCR { get; set; }
        public string PropDiagnosedE { get; set; }
        public string PropDiagnosedR { get; set; }
        public string PropSSRE { get; set; }
        public string PropSSRR { get; set; }
        public string PropAgencyRefE { get; set; }
        public string PropAgencyRefR { get; set; }
        public string PropFundE { get; set; }
        public string PropFundR { get; set; }

        public List<CustomQuestionsEntity> custQuestions { get; set; }
        public bool IsSaveValid { get; set; }

        public string propQuestion1 { get; set; }
        public string propQuestion2 { get; set; }
        public string propQuestion3 { get; set; }
        public string propQuestion4 { get; set; }
        public string propQuestion5 { get; set; }
        public string propNewcustomQues { get; set; }

        public ChldTrckEntity propTrackEntity { get; set; }

        private void PbPdf_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSS00133");
        }


        private bool isValidate()
        {
            bool isValid = true;


            if (String.IsNullOrEmpty(txtTrack.Text))
            {
                _errorProvider.SetError(txtTrack, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTrack.Text));
                isValid = false;
            }
            else
            {
                //if (Convert.ToDouble(txtTrack.Text) <= 0)
                //{
                //    _errorProvider.SetError(txtTrack, Consts.Messages.Greaterthanzzero);
                //    isValid = false;
                //}
                //else
                {

                    if (isTrackCodeExists(txtTrack.Text))
                    {
                        _errorProvider.SetError(txtTrack, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblTrack.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtTrack, null);
                    }
                }

            }

            if (String.IsNullOrEmpty(txtTrackDesc.Text.Trim()))
            {
                _errorProvider.SetError(txtTrackDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTrackDesc.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTrackDesc, null);
            }


            return isValid;
        }

        private bool isTrackCodeExists(string trackCode)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                ChldTrckEntity Search_Track = new ChldTrckEntity();
                Search_Track.Agency = string.Empty;
                Search_Track.Dept = string.Empty;
                Search_Track.Prog = string.Empty;//BaseForm.BaseAgency;
                Search_Track.COMPONENT = "0000";
                Search_Track.TASK = trackCode;
                List<ChldTrckEntity> chldTrack_List = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", trackCode, string.Empty);
                if (chldTrack_List.Count > 0)
                {
                    isExists = true;
                }
            }
            return isExists;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidate()) return;

            List<CustomQuestionsEntity> selectedQuestions = (from c in gvwQuestion.Rows.Cast<DataGridViewRow>().ToList()
                                                             where (((DataGridViewCheckBoxCell)c.Cells["gvcheckdetails"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                             select ((DataGridViewRow)c).Tag as CustomQuestionsEntity).ToList();

            if (selectedQuestions.Count == 1)
            {
                SaveTrackrDetails();
                ChldTrckEntity chldtrackEntity = new ChldTrckEntity();
                chldtrackEntity.Agency = string.Empty;
                chldtrackEntity.Dept = string.Empty;
                chldtrackEntity.Prog = string.Empty;
                chldtrackEntity.COMPONENT = propComponent;
                chldtrackEntity.TASK = txtTrack.Text;
                chldtrackEntity.TASKDESCRIPTION = txtTrackDesc.Text;
                chldtrackEntity.COMPONENTOWNER1 = string.Empty;
                chldtrackEntity.COMPONENTOWNER2 = string.Empty;
                chldtrackEntity.COMPONENTOWNER3 = string.Empty;
                if (cmbGrowthChart.Items.Count > 0)
                    chldtrackEntity.GCHARTCODE = ((ListItem)cmbGrowthChart.SelectedItem).Value.ToString();
                //if (!((ListItem)cmbOwn1.SelectedItem).Value.ToString().Equals("0"))
                //{
                //    chldtrackEntity.COMPONENTOWNER1 = ((ListItem)cmbOwn1.SelectedItem).Value.ToString();
                //}
                //if (!((ListItem)cmbOwn2.SelectedItem).Value.ToString().Equals("0"))
                //{
                //    chldtrackEntity.COMPONENTOWNER2 = ((ListItem)cmbOwn2.SelectedItem).Value.ToString();
                //}
                //if (!((ListItem)cmbOwn3.SelectedItem).Value.ToString().Equals("0"))
                //{
                //    chldtrackEntity.COMPONENTOWNER3 = ((ListItem)cmbOwn3.SelectedItem).Value.ToString();
                //}

                string strCustQues = string.Empty;

                foreach (CustomQuestionsEntity custQuestion in selectedQuestions)
                {
                    if (!strCustQues.Equals(string.Empty)) strCustQues += " ";
                    strCustQues += custQuestion.CUSTCODE.ToString();
                }
                chldtrackEntity.CustQCodes = strCustQues;

                chldtrackEntity.ADDOPERATOR = BaseForm.UserID;
                chldtrackEntity.LSTCOPERATOR = BaseForm.UserID;
                chldtrackEntity.QUESTIONE = propQuestionE;
                chldtrackEntity.QUESTIONR = propQuestionR;
                chldtrackEntity.CASENOTESE = propCaseNotesE;
                chldtrackEntity.CASENOTESR = propCaseNotesR;
                chldtrackEntity.SBCBE = PropSBCBE;
                chldtrackEntity.SBCBR = PropSBCBR;
                chldtrackEntity.ADDRESSE = PropAddressE;
                chldtrackEntity.ADDRESSR = PropAddressR;
                chldtrackEntity.COMPLETEE = PropCompletE;
                chldtrackEntity.COMPLETER = PropCompletR;
                chldtrackEntity.FOLLOWUPE = PropFollowupE;
                chldtrackEntity.FOLLOWUPR = PropFollowupR;
                chldtrackEntity.FOLLOWUPCE = PropFollowupCE;
                chldtrackEntity.FOLLOWUPCR = PropFollowupCR;
                chldtrackEntity.DIAGNOSEE = PropDiagnosedE;
                chldtrackEntity.DIAGNOSER = PropDiagnosedR;
                chldtrackEntity.SSRE = PropSSRE;
                chldtrackEntity.SSRR = PropSSRR;
                chldtrackEntity.WHEREE = PropAgencyRefE;
                chldtrackEntity.WHERER = PropAgencyRefR;
                chldtrackEntity.FUNDE = PropFundE;
                chldtrackEntity.FUNDR = PropFundR;
                chldtrackEntity.Mode = Mode;
                if (_model.ChldTrckData.InsertUpdateDelChldTrck(chldtrackEntity))
                {
                    HSS00133_Control hSS00133Control = BaseForm.GetBaseUserControl() as HSS00133_Control;
                    if (hSS00133Control != null)
                    {
                        hSS00133Control.NewTaskId = txtTrack.Text;
                        hSS00133Control.RefreshGrid();
                    }
                    this.Close();
                    AlertBox.Show("Saved Successfully");
                }
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please select at least one question");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void SaveTrackrDetails()
        {
            ChldTrckREntity chldtrackr = new ChldTrckREntity();
            chldtrackr.Agency = string.Empty;
            chldtrackr.Dept = string.Empty;
            chldtrackr.Prog = string.Empty;//BaseForm.BaseAgency;
            chldtrackr.COMPONENT = "0000";
            chldtrackr.TASK = txtTrack.Text;
            chldtrackr.COMPONENT = propComponent;
            chldtrackr.Mode = "DeleteAll";
            _model.ChldTrckData.InsertUpdateDelChldTrckR(chldtrackr);

            string strIntakeEntry = string.Empty;
            string strentryDays = string.Empty;
            string strNextask = string.Empty;
            string strNextdays = string.Empty;
            bool boolEveryear = false;
            bool boolNextAction = false;
            foreach (DataGridViewRow gvtractrow in gvwTrakrdetails.Rows)
            {
                strIntakeEntry = gvtractrow.Cells["gvIntakeEntry1"].Value == null ? string.Empty : gvtractrow.Cells["gvIntakeEntry1"].Value.ToString();
                strentryDays = gvtractrow.Cells["gvEntryDays"].Value == null ? string.Empty : gvtractrow.Cells["gvEntryDays"].Value.ToString();
                strNextask = gvtractrow.Cells["gvNextTask"].Value == null ? string.Empty : gvtractrow.Cells["gvNextTask"].Value.ToString();
                strNextdays = gvtractrow.Cells["gvNextDays"].Value == null ? string.Empty : gvtractrow.Cells["gvNextDays"].Value.ToString();
                boolEveryear = gvtractrow.Cells["gvYear"].Value == null ? false : Convert.ToBoolean(gvtractrow.Cells["gvYear"].Value);
                boolNextAction = gvtractrow.Cells["gvNAcation"].Value == null ? false : Convert.ToBoolean(gvtractrow.Cells["gvNAcation"].Value);
                strNextask = strNextask == "0" ? string.Empty : strNextask;
                strIntakeEntry = strIntakeEntry == "0" ? string.Empty : strIntakeEntry;
                if (strIntakeEntry.Trim() != string.Empty || strentryDays.Trim() != string.Empty || strNextask.Trim() != string.Empty || strNextdays.Trim() != string.Empty || boolEveryear != false || boolNextAction != false)
                {

                    chldtrackr.TASK = txtTrack.Text;
                    chldtrackr.FUND = gvtractrow.Cells["gvFundCode"].Value.ToString();
                    chldtrackr.NEXTDAYS = strNextdays;
                    chldtrackr.NEXTTASK = strNextask;
                    chldtrackr.NXTACTION = boolNextAction == true ? "Y" : "N";
                    chldtrackr.REQUIREYEAR = boolEveryear == true ? "Y" : "N";
                    chldtrackr.INTAKEENTRY = strIntakeEntry;
                    chldtrackr.ENTERDAYS = strentryDays;

                    chldtrackr.ADDOPERATOR = BaseForm.UserID;
                    chldtrackr.LSTCOPERATOR = BaseForm.UserID;
                    chldtrackr.Mode = Mode;
                    _model.ChldTrckData.InsertUpdateDelChldTrckR(chldtrackr);

                }

            }
        }

        private void txtTrack_Leave(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtTrack, null);
            if (txtTrack.Text != string.Empty)
            {
                txtTrack.Text = "0000".Substring(0, 4 - txtTrack.Text.Length) + txtTrack.Text;
                if (isTrackCodeExists(txtTrack.Text))
                {
                    _errorProvider.SetError(txtTrack, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblTrack.Text.Replace(Consts.Common.Colon, string.Empty)));
                    InvokeFocusCommand(txtTrack);
                }
                else
                {
                    _errorProvider.SetError(txtTrack, null);
                }
            }
        }

        private void InvokeFocusCommand(Control objControl)
        {
            //IApplicationContext objApplicationContext = this.Context as IApplicationContext;
            //if (objApplicationContext != null)
            //{
            //    objApplicationContext.SetFocused(objControl, true);
            //}
        }

        private void fillCustQuestions(string custQues)
        {
            if (string.IsNullOrEmpty(custQues))
            {
                int rowIndex = gvwQuestion.Rows.Add("false", string.Empty, string.Empty, "...");
                CustomQuestionsEntity customquesempty = new CustomQuestionsEntity();
                gvwQuestion.Rows[rowIndex].Tag = customquesempty;

            }
            else
            {
                List<string> QuesList = new List<string>();
                if (custQues != null)
                {
                    string[] relationTypes = custQues.Split(' ');
                    for (int i = 0; i < relationTypes.Length; i++)
                    {
                        QuesList.Add(relationTypes.GetValue(i).ToString());
                    }
                }
                List<CustomQuestionsEntity> custQuestionsList = custQuestions;
                gvwQuestion.Rows.Clear();
                foreach (CustomQuestionsEntity entityQuestion in custQuestionsList)
                {
                    //if (Mode.ToUpper() != "VIEW")
                    //{
                    //    string flag = "false";
                    //    if (custQues != null && QuesList.Contains(entityQuestion.CUSTCODE))
                    //    {
                    //        flag = "true";
                    //    }
                    //    int rowIndex = gvwQuestion.Rows.Add(flag, entityQuestion.CUSTDESC.Trim(), entityQuestion.CUSTCODE);
                    //    if (flag == "true")
                    //    {
                    //        CommonFunctions.setTooltip(rowIndex, propTrackEntity.ADDOPERATOR, propTrackEntity.DATEADD, propTrackEntity.LSTCOPERATOR, propTrackEntity.DATELSTC, gvwQuestion);
                    //    }
                    //    gvwQuestion.Rows[rowIndex].Tag = entityQuestion;
                    //    //  CommonFunctions.setTooltip(rowIndex, entityQuestion.addoperator, entityQuestion.adddate, entityQuestion.lstcoperator, entityQuestion.lstcdate, gvwQuestion);
                    //}
                    //else
                    //{
                    string flag = "false";
                    if (custQues != null && QuesList.Contains(entityQuestion.CUSTCODE))
                    {
                        flag = "true";
                        int rowIndex = gvwQuestion.Rows.Add(flag, entityQuestion.CUSTDESC.Trim(), entityQuestion.CUSTCODE, "...");
                        gvwQuestion.Rows[rowIndex].Tag = entityQuestion;
                        CommonFunctions.setTooltip(rowIndex, propTrackEntity.ADDOPERATOR, propTrackEntity.DATEADD, propTrackEntity.LSTCOPERATOR, propTrackEntity.DATELSTC, gvwQuestion);
                    }

                    // }
                }
                if (Mode.ToUpper() == "EDIT")
                {
                    gvwQuestion.Sort(gvwQuestion.Columns["gvcheckdetails"], ListSortDirection.Descending);
                }
            }

            if (gvwQuestion.Rows.Count > 0) {
                gvwQuestion.Rows[0].Selected = true;
            }
        }

        private void gvwTrakrdetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == gvNAcation.Index && e.RowIndex != -1)
            {
                gvwTrakrdetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
                bool boolNextAction = gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNAcation"].Value == null ? false : Convert.ToBoolean(gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNAcation"].Value);
                if (boolNextAction == true)
                {
                    //  gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNAcation"].Value = true;
                    gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNextTask"].ReadOnly = false;
                    gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNextDays"].ReadOnly = false;


                }
                else
                {
                    //  gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNAcation"].Value = false;
                    gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNextTask"].ReadOnly = true;
                    gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNextDays"].ReadOnly = true;
                    gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNextTask"].Value = "0";
                    gvwTrakrdetails.Rows[e.RowIndex].Cells["gvNextDays"].Value = string.Empty;
                }

                gvwTrakrdetails.Update();
                //gvwTrakrdetails.RefreshEdit();
                gvwTrakrdetails.CellValueChanged += new DataGridViewCellEventHandler(gvwTrakrdetails_CellValueChanged);
            }
        }

        private void gvwQuestion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwQuestion.Rows.Count > 0)
            {
                if (!(Mode.Equals("View")))
                {
                    if (e.RowIndex > -1)
                    {
                        if (e.ColumnIndex == gvbQuestion.Index)
                        {
                            custQuestions = _model.FieldControls.GetCustomQuestions(Privileges.Program, "*", string.Empty, string.Empty, string.Empty, string.Empty);
                            HSS00133QuestionView questionview = new HSS00133QuestionView(BaseForm, Mode, Privileges, string.Empty, propNewcustomQues, custQuestions);
                            questionview.FormClosed += new FormClosedEventHandler(questionview_FormClosed);
                            questionview.StartPosition = FormStartPosition.CenterScreen;
                            questionview.ShowDialog();
                        }
                    }
                }


                //propQuestion1 = string.Empty;
                //propQuestion2 = string.Empty;
                //propQuestion3 = string.Empty;
                //propQuestion4 = string.Empty;
                //propQuestion5 = string.Empty;
                //int i = 0;
                //foreach (DataGridViewRow gvRows in gvwQuestion.Rows)
                //{
                //    if (gvRows.Cells["gvcheckdetails"].Value != null && Convert.ToBoolean(gvRows.Cells["gvcheckdetails"].Value) == true)
                //    {
                //        i++;
                //    }
                //}

                //if (i > 1)
                //{
                //    gvwQuestion.CurrentRow.Cells["gvcheckdetails"].Value = false;
                //    CommonFunctions.MessageBoxDisplay("You Can't Add More Than 1 Questions");
                //}

            }
        }

        private void btnTaskConfig_Click(object sender, EventArgs e)
        {
            HSS00133TaskConfiguration hsstaskconfig = new HSS00133TaskConfiguration(BaseForm, Mode, Privileges, propTrackEntity);
            hsstaskconfig.FormClosed += new FormClosedEventHandler(hsstaskconfig_FormClosed);
            hsstaskconfig.StartPosition = FormStartPosition.CenterScreen;
            hsstaskconfig.ShowDialog();
        }

        void hsstaskconfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSS00133TaskConfiguration hss00133Taslk = sender as HSS00133TaskConfiguration;
            if (hss00133Taslk.DialogResult == DialogResult.OK)
            {
                hss00133Taslk.FillTaskConfiguration();
                propQuestionE = hss00133Taslk.propQuestionE;
                propQuestionR = hss00133Taslk.propQuestionR;
                propCaseNotesE = hss00133Taslk.propCaseNotesE;
                propCaseNotesR = hss00133Taslk.propCaseNotesR;
                PropSBCBE = hss00133Taslk.PropSBCBE;
                PropSBCBR = hss00133Taslk.PropSBCBR;
                PropAddressE = hss00133Taslk.PropAddressE;
                PropAddressR = hss00133Taslk.PropAddressR;
                PropCompletE = hss00133Taslk.PropCompletE;
                PropCompletR = hss00133Taslk.PropCompletR;
                PropFollowupE = hss00133Taslk.PropFollowupE;
                PropFollowupR = hss00133Taslk.PropFollowupR;
                PropFollowupCE = hss00133Taslk.PropFollowupCE;
                PropFollowupCR = hss00133Taslk.PropFollowupCR;
                PropDiagnosedE = hss00133Taslk.PropDiagnosedE;
                PropDiagnosedR = hss00133Taslk.PropDiagnosedR;
                PropSSRE = hss00133Taslk.PropSSRE;
                PropSSRR = hss00133Taslk.PropSSRR;
                PropAgencyRefE = hss00133Taslk.PropAgencyRefE;
                PropAgencyRefR = hss00133Taslk.PropAgencyRefR;
                PropFundE = hss00133Taslk.PropFundE;
                PropFundR = hss00133Taslk.PropFundR;

            }

        }

        private void btnQuesDefine_Click(object sender, EventArgs e)
        {
            HSS00133CustomQuestions customQuestionForm = new HSS00133CustomQuestions(BaseForm, Mode, Privileges);
            customQuestionForm.ShowDialog();
        }

        private void btnSelectQuestion_Click(object sender, EventArgs e)
        {
            HSS00133QuestionView questionview = new HSS00133QuestionView(BaseForm, Mode, Privileges, string.Empty, propNewcustomQues, custQuestions);
            questionview.FormClosed += new FormClosedEventHandler(questionview_FormClosed);
            questionview.ShowDialog();
        }

        void questionview_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSS00133QuestionView hss00133question = sender as HSS00133QuestionView;
            if (hss00133question.DialogResult == DialogResult.OK)
            {
                propNewcustomQues = hss00133question.propCustomQuestios;
                custQuestions = hss00133question.custQuestions;
                fillCustQuestions(hss00133question.propCustomQuestios);
            }
        }

        private void gvwQuestion_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (gvwQuestion.Rows[e.RowIndex].Cells["gvQuestionDesc"].Value == null)
            {
                gvwQuestion.Rows[e.RowIndex].Cells["gvQuestionDesc"].Value = string.Empty;
                gvwQuestion.Rows[e.RowIndex].Cells["gvcheckdetails"].Value = false;
                gvwQuestion.Rows[e.RowIndex].Cells["gvQuestioncode"].Value = false;
                gvwQuestion.Rows[e.RowIndex].Cells["gvbQuestion"].Value = "...";
            }
        }

        private void HSS00133Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tlHelp") { 
            
            }
        }

        private void gvwTrakrdetails_Click(object sender, EventArgs e)
        {

        }
    }
}