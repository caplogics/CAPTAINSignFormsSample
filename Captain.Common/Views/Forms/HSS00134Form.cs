#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;

using Captain.DatabaseLayer;
using Captain.Common.Views.Controls.Compatibility;
using Spire.Pdf.Grid;
using DevExpress.XtraRichEdit.Model;
using NPOI.OpenXmlFormats;
using System.IO;
using Captain.Common.Interfaces;
using Microsoft.Ajax.Utilities;
using Twilio.TwiML.Voice;
using DevExpress.Web.Internal.XmlProcessor;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00134Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<CustomQuestionsandAnswers> _customQuestionsandAnswers = null;
        private List<FldcntlHieEntity> _custFLDCNTLEntity = new List<FldcntlHieEntity>();
        private List<CommonEntity> _custCASEELIGEntity = new List<CommonEntity>();
        private string strcheckMode = string.Empty;
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Blank = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("EditIcon.gif");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Cross = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("AddItem.gif");

        string Img_Blank = Consts.Icons.ico_Edit;   //"Resources/Icons/16X16/EditIcon.gif";
        string Img_Tick = Consts.Icons.ico_Tick;    //"Resources/Icons/16X16/tick.ico";
        string Img_Cross = "Resources/Icons/16X16/AddItem.gif";
        string TaskUpldID = "";
        string _prvDocFile = "";
        string _formType = string.Empty;
        int strGridIndex = 0;
        #endregion

        /// <summary>
        /// This Contractor calling Only Regular add and edit. 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="baseForm"></param>
        /// <param name="privilieges"></param>
        /// <param name="strComponents"></param>
        /// <param name="strTask"></param>
        /// <param name="chldmedientity"></param>
        /// <param name="strSeq"></param>
        /// <param name="strTaskDesc"></param>
        /// <param name="strComponetDesc"></param>
        public HSS00134Form(string mode, BaseForm baseForm, PrivilegeEntity privilieges, string strComponents, string strTask, ChldMediEntity chldmedientity, string strSeq, string strTaskDesc, string strComponetDesc, string formType, string a)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            propRegularForm = "Regular";
            Mode = mode;
            _formType = formType;

            txtWheree.Validator = TextBoxValidation.IntegerValidator;
            FillAgencyReferalDetails();
            BaseForm = baseForm;
            Privileges = privilieges;
            gvwSelectTask.Visible = false;
            gvwChildDetails.BringToFront();
            txtTaskData.BringToFront();
            cmbTask.Enabled = false;
            propPostTrack = false;
            cmbTask.Visible = false;
            gvwChildDetails.Visible = true;
            propPostForm = string.Empty;

            propTask = strTask;
            propSeq = strSeq;
            propComponents = strComponents;
            propChldmedi = chldmedientity;
            if (propChldmedi != null)
            {
                TaskUpldID = propChldmedi.TSKUPLD_ID;
                _prvDocFile = propChldmedi.TSKUPLD_ORIG_NAME;
                lbldocupldName.Text = propChldmedi.TSKUPLD_ORIG_NAME;
            }
            propchldTrckList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", strTask, string.Empty);

            CaseEnrlEntity EnrlList = new CaseEnrlEntity(true);
            EnrlList.Agy = BaseForm.BaseAgency;
            EnrlList.Dept = BaseForm.BaseDept;
            EnrlList.Prog = BaseForm.BaseProg;
            EnrlList.Year = BaseForm.BaseYear;
            EnrlList.App = BaseForm.BaseApplicationNo;
            EnrlList.Rec_Type = "H";
            propSaveForm = false;
            propCaseENrl = _model.EnrollData.Browse_CASEENRL(EnrlList, "Browse");

            //fillCombofund();
            fillCombofund_New();

            GetFund();

            //propCaseSnp = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);
            //propCaseMst = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);
            //if (BaseForm.UserProfile.LoadData.Equals("1") && (!string.IsNullOrEmpty(BaseForm.BaseYear)))
            //{
            //propChldmediList = _model.ChldTrckData.GetChldMediDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, propTask, string.Empty);
            //}
            //else
            //{
            propChldmediList = _model.ChldTrckData.GetChldMediDetails1(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, BaseForm.BaseApplicationNo, propTask, string.Empty);
            //}
            //FillCombo();
            cmbClass.Enabled = false;
            cmbClass.Visible = false;
            lblClass.Visible = false;
            FillRegularApplicantData();
            //txtComponentData.Text = strComponetDesc.ToString().Trim();

            this.Text = strComponetDesc.ToString().Trim() + " - " + mode; ;

            txtTaskData.Text = strTaskDesc.ToString().Trim();
            if (propchldTrckList.Count > 0)
            {
                propcustQuestions = _model.FieldControls.GetCustomQuestions("HSS00133", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                fillCustomQuestions(propchldTrckList[0].CustQCodes.Trim(), BaseForm.BaseApplicationNo, BaseForm.BaseYear, propChldmedi);
            }
            //this.Text = Privileges.PrivilegeName + " - " + mode;// +" - Client Tracking";
            gvwChildDetails.Columns["gvChildName"].Width = 120;//195;
            gvwChildDetails.Columns["Column0"].Width = 120;
            gvwChildDetails.Columns["gvSbcdDate"].Visible = false;
            gvwChildDetails.Columns["gvChkSelect"].Visible = false;
            gvwChildDetails.Columns["gvtCompletDate"].Visible = false;
            if (!Mode.Equals("Add"))
            {
                FillForm(string.Empty);
                gvwChildDetails.Columns["gvChkSelect"].Visible = false;

            }
            else
            {
                if (_formType == "ClientTracking")
                {
                    CommonFunctions.SetComboBoxValue(cmbFund, propFund);
                }
                else if (_formType == "Components")
                {
                    CommonFunctions.SetComboBoxValue(cmbFund, propFund);

                    if (propFund == "HS" || propCaseENrl.Count > 1)//|| propFund == "EHS")
                        CommonFunctions.SetComboBoxValue(cmbFund, "HS");
                }

                //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                //{
                List<ChldTrckREntity> chldtrckrlist = new List<ChldTrckREntity>();
                if (propchldTrckList.Count > 0)
                {
                    chldtrckrlist = _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", propTask, string.Empty);
                }
                CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));
                string strSBCBDate = FillSBCDDetailsNext(BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].IntakeDate, casesnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                if (strSBCBDate != string.Empty)
                {
                    dtSBCB.Value = Convert.ToDateTime(strSBCBDate);
                    dtSBCB.Checked = true;
                }
                else
                    dtSBCB.Checked = false;
                //}
                //else
                //{
                //    gvwChildDetails.Columns["gvChildName"].Width = 172;
                //    gvwChildDetails.Columns["gvSbcdDate"].Visible = false;
                //}
            }
            gvwChildDetails.Rows[0].Selected = true;
            gvwChildDetails.CurrentCell = gvwChildDetails.Rows[0].Cells[2];//[1];
            EnableDisableControls(string.Empty);
        }

        /// <summary>
        /// This Constractor calling Only Quick add and edit
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="baseForm"></param>
        /// <param name="privilieges"></param>
        /// <param name="strComponents"></param>
        /// <param name="chldmedientity"></param>
        /// <param name="strSeq"></param>
        /// <param name="strComponetDesc"></param>
        /// <param name="strTask"></param>
        public HSS00134Form(string mode, BaseForm baseForm, PrivilegeEntity privilieges, string strComponents, ChldMediEntity chldmedientity, string strSeq, string strComponetDesc, string strTask, string strFund, string formType)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            Mode = mode;
            _formType = formType;
            //txtComponentData.Visible = false;

            txtWheree.Validator = TextBoxValidation.IntegerValidator;
            FillAgencyReferalDetails();
            BaseForm = baseForm;
            Privileges = privilieges;
            //fillCombofund();
            propRegularForm = string.Empty;
            propSeq = strSeq;
            propPostForm = "QuickPost";
            propComponents = strComponents;
            propChldmedi = chldmedientity;
            btnTasks.Visible = false;
            lblTask.Visible = false;
            //textBox1.Visible = false;
            propPostTrack = false;
            lblClass.Visible = false;
            propchldTrckList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", strTask, string.Empty);
            propcustQuestions = _model.FieldControls.GetCustomQuestions("HSS00133", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            CaseEnrlEntity EnrlList = new CaseEnrlEntity(true);
            EnrlList.Agy = BaseForm.BaseAgency;
            EnrlList.Dept = BaseForm.BaseDept;
            EnrlList.Prog = BaseForm.BaseProg;
            EnrlList.Year = BaseForm.BaseYear;
            EnrlList.Rec_Type = "H";
            string strApplicant = string.Empty;
            //if (Mode == "Add")
            //{
            //    EnrlList.App = BaseForm.BaseApplicationNo;
            //    strApplicant = BaseForm.BaseApplicationNo;
            //}
            propCaseENrl = _model.EnrollData.Browse_CASEENRL(EnrlList, "Browse");
            propSaveForm = false;

            //GetFund();
            propFund = strFund;
            fillCombofund_New();
            //if (Mode == "Edit")
            //{
            propPostTrack = true;
            cmbTask.Visible = true;
            cmbTask.BringToFront();
            lblTask.Visible = true;
            lblClass.Visible = true;
            txtTaskData.Visible = false;
            gvwSelectTask.Visible = false;
            gvwChildDetails.BringToFront();
            gvwChildDetails.Visible = true;
            cmbClass.Enabled = true;
            cmbClass.Visible = true;
            this.btnTasks.Location = new System.Drawing.Point(230, 0);
            propCaseSnp = _model.CaseMstData.GetCaseSnpMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, "CASEMSTSNP");
            //   propCaseMst = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);
            FillComboTask();
            CommonFunctions.SetComboBoxValue(cmbTask, strTask);
            propTask = strTask;
            cmbTask_SelectedIndexChanged(cmbTask, new EventArgs());
            FillCombo();
            //chkClassRoom.CheckedChanged -= new EventHandler(chkClassRoom_CheckedChanged);
            //chkClassRoom.CheckedChanged += new EventHandler(chkClassRoom_CheckedChanged);

            if (_formType == "Components")
            {
                chkClassRoom.Visible = false;
                cmbClass.Enabled = true;
                cmbTask.Enabled = true;//false;

                this.Text = strComponetDesc + " - Classroom Post";

                List<CaseEnrlEntity> CaseEnrlDetails = propCaseENrl.FindAll(u => u.App.Equals(BaseForm.BaseApplicationNo));
                cmbClass_SelectedIndexChanged(cmbClass, new EventArgs());

                this.Size = new Size(1185, 558);
                this.pnlLeftGrids.Size = new Size(445, 428);
                this.gvwChildDetails.Size = new Size(420, 374);
                this.gvApplication.Visible = true;
                this.gvApplication.ShowInVisibilityMenu = true;
            }
            else if (_formType == "ClientTracking")
            {
                chkClassRoom.Visible = true;
                cmbClass.Enabled = false;
                cmbTask.Enabled = true;

                this.Text = strComponetDesc + " - Quick Post";

                this.Size = new Size(1103, 558);
                this.pnlLeftGrids.Size = new Size(353, 428);
                this.gvwChildDetails.Size = new Size(345, 374);
                this.gvApplication.Visible = false;
                this.gvApplication.ShowInVisibilityMenu = false;
            }
            //}

            if (Mode == "Add")
            {
                gvwChildDetails.SelectionChanged -= new EventHandler(gvwChildDetails_SelectionChanged);
            }
            //    gvwSelectTask.Visible = true;
            //    gvwSelectTask.BringToFront();
            //    gvwChildDetails.Visible = false;
            //    cmbClass.Enabled = false;
            //    cmbTask.Visible = false;
            //    cmbTask.Enabled = false;
            //    cmbClass.Visible = false;
            //    gvwSelectTask.SelectionChanged -= new EventHandler(gvwSelectTask_SelectionChanged);
            //    FillTaskGrid();
            //    gvwSelectTask.SelectionChanged += new EventHandler(gvwSelectTask_SelectionChanged);
            //    txtComponentData.Text = BaseForm.BaseApplicationNo + "    " + BaseForm.BaseApplicationName;
            //    this.txtComponentData.Location = new System.Drawing.Point(300, 2);
            //    this.txtComponentData.Size = new System.Drawing.Size(400, 20);
            //    gvwSelectTask_SelectionChanged(gvwSelectTask, new EventArgs());
            //}


            // FillCombo();


            // txtTaskData.Text = strTaskDesc.ToString().Trim();
            //if (propchldTrckList.Count > 0)
            //{
            //    fillCustomQuestions(propchldTrckList[0].CustQCodes.Trim(), BaseForm.BaseApplicationNo, BaseForm.BaseYear, propChldmedi);
            //}
            //this.Text = Privileges.PrivilegeName + " - Quick Post";// +" - Client Tracking";
            if (!Mode.Equals("Add"))
            {
                //FillForm();
                // gvwChildDetails.Columns["gvChkSelect"].Visible = false;
                gvwChildDetails.Columns["gvChkSelect"].ReadOnly = true;
                // gvwChildDetails.Columns["gvChildName"].Width = 195;
                gvwChildDetails.Columns["gvSbcdDate"].Visible = false;
            }
        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public ChldMediEntity propChldmedi { get; set; }
        public List<ChldMediEntity> propChldmediList { get; set; }
        public string propTask { get; set; }
        public string propComponents { get; set; }
        public string propSeq { get; set; }
        public string Mode { get; set; }
        public List<CaseNotesEntity> caseNotesEntity { get; set; }
        public List<CaseEnrlEntity> propCaseENrl { get; set; }
        public List<CaseSnpEntity> propCaseSnp { get; set; }
        // public List<CaseMstEntity> propCaseMst { get; set; }
        public List<CustomQuestionsEntity> propcustQuestions { get; set; }
        public List<ChldTrckEntity> propchldTrckList { get; set; }
        public List<ChldTrckEntity> propchldseltrck { get; set; }
        public List<ChldTrckREntity> propchldRtrckList { get; set; }
        public bool propSaveForm { get; set; }
        public bool propPostTrack { get; set; }
        public string propPostForm { get; set; }
        public string propRegularForm { get; set; }
        public string propFund { get; set; }
        #endregion

        private string FillSBCDDetails(string strApplicationNo, string strIntakeDate, string strDateOfBirth, string strAttnDate, string strcomponents, string strTask, List<ChldTrckREntity> _chldtrackRList)
        {
            string strDate = string.Empty;
            if (propCaseENrl.Count > 0)
            {
                List<CaseEnrlEntity> CaseEnrlDetails = propCaseENrl.FindAll(u => u.App.Equals(strApplicationNo) && u.Status.Trim().Equals("E"));
                if (CaseEnrlDetails.Count > 0)
                {
                    CaseEnrlEntity caseEnrlCheckRecord;
                    if (CaseEnrlDetails.Count == 1)
                        caseEnrlCheckRecord = CaseEnrlDetails[0];
                    else
                    {
                        caseEnrlCheckRecord = CaseEnrlDetails.Find(u => u.FundHie.Trim() == "HS");
                        if (caseEnrlCheckRecord == null)
                            caseEnrlCheckRecord = CaseEnrlDetails[CaseEnrlDetails.Count - 1];
                    }


                    if (propchldTrckList.Count > 0)
                    {
                        List<ChldTrckREntity> chldtrackRList = _chldtrackRList;
                        if (chldtrackRList.Count > 0)
                        {
                            foreach (ChldTrckREntity chldtrackitem in chldtrackRList)
                            {
                                if (chldtrackitem.FUND.Trim() == caseEnrlCheckRecord.FundHie.ToString())
                                {
                                    if (!string.IsNullOrEmpty(chldtrackitem.ENTERDAYS))
                                    {
                                        if (Convert.ToInt32(chldtrackitem.ENTERDAYS) >= 0)
                                        {
                                            if (chldtrackitem.INTAKEENTRY.Trim().Equals("A"))
                                            {
                                                if (!string.IsNullOrEmpty(strIntakeDate))
                                                    strDate = Convert.ToDateTime(strIntakeDate).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                                break;
                                            }
                                            else if (chldtrackitem.INTAKEENTRY.Trim().Equals("E"))
                                            {
                                                strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                                if (!string.IsNullOrEmpty(strAttnDate))
                                                    strDate = Convert.ToDateTime(strAttnDate).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                            }
                                            else if (chldtrackitem.INTAKEENTRY.Trim().Equals("D"))
                                            {
                                                if (!string.IsNullOrEmpty(strDateOfBirth))
                                                    strDate = Convert.ToDateTime(strDateOfBirth).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return strDate;
        }



        private string FillSBCDDetailsNext(string strApplicationNo, string strIntakeDate, string strDateOfBirth, string strAttnDate, string strcomponents, string strTask, List<ChldTrckREntity> _chldtrackRList)
        {
            string strDate = string.Empty;
            bool boolChecksbcb = true;
            if (propFund != string.Empty)
            {
                List<ChldTrckREntity> chldtrackRList = _chldtrackRList;
                if (chldtrackRList.Count > 0)
                {
                    if (propChldmediList.Count > 0)
                    {
                        List<ChldMediEntity> chldMediEntity = null;
                        if (propPostForm == "QuickPost")
                        {
                            chldMediEntity = _model.ChldTrckData.GetChldMediDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, strApplicationNo, propTask, string.Empty);//propChldmediList.FindAll(u => u.APP_NO.Equals(strApplicationNo));
                        }
                        else
                        {
                            chldMediEntity = propChldmediList.FindAll(u => u.TASK.Equals(propTask));
                        }
                        if (chldMediEntity.Count > 0)
                        {
                            if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                            {
                                chldMediEntity = chldMediEntity.OrderByDescending(u => u.SEQ).ToList();
                            }
                            else
                            {
                                chldMediEntity = chldMediEntity.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                            }
                            //chldMediEntity = chldMediEntity.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();

                            if (chldMediEntity[0].COMPLETED_DATE != string.Empty)
                            {
                                boolChecksbcb = false;
                                ChldTrckREntity chldtrackitemNext = _chldtrackRList.Find(u => u.TASK == strTask && u.FUND == propFund); // _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", propTask, string.Empty);
                                if (chldtrackitemNext != null)
                                {
                                    if (chldtrackitemNext.NXTACTION.Trim() == "Y")
                                    {
                                        if (!string.IsNullOrEmpty(chldtrackitemNext.NEXTDAYS))
                                        {
                                            if (Convert.ToInt32(chldtrackitemNext.NEXTDAYS) >= 0)
                                            {
                                                // MessageBox.Show(Convert.ToDateTime(chldMediEntity[0].COMPLETED_DATE).ToString());
                                                strDate = Convert.ToDateTime(chldMediEntity[0].COMPLETED_DATE).AddDays(Convert.ToInt32(chldtrackitemNext.NEXTDAYS)).ToString();
                                                boolChecksbcb = false;
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                goto next;
                                boolChecksbcb = true;
                            }
                        }
                    }

                next:
                    if (boolChecksbcb)
                    {
                        ChldTrckREntity chldtrackitem = chldtrackRList.Find(u => u.FUND.Trim() == propFund);
                        if (chldtrackitem != null)
                        {
                            if (!string.IsNullOrEmpty(chldtrackitem.ENTERDAYS))
                            {
                                if (Convert.ToInt32(chldtrackitem.ENTERDAYS) >= 0)
                                {
                                    if (chldtrackitem.INTAKEENTRY.Trim().Equals("A"))
                                    {
                                        if (!string.IsNullOrEmpty(strIntakeDate))
                                        {
                                            // MessageBox.Show(Convert.ToDateTime(strIntakeDate).ToString());
                                            strDate = Convert.ToDateTime(strIntakeDate).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                        }

                                    }
                                    else if (chldtrackitem.INTAKEENTRY.Trim().Equals("E"))
                                    {
                                        strAttnDate = string.Empty;
                                        List<CaseEnrlEntity> CaseEnrlDetails = propCaseENrl.FindAll(u => u.App.Equals(strApplicationNo) && u.Status.Trim().Equals("E"));
                                        if (CaseEnrlDetails.Count > 0)
                                        {
                                            CaseEnrlEntity caseEnrlCheckRecord;
                                            if (CaseEnrlDetails.Count == 1)
                                                caseEnrlCheckRecord = CaseEnrlDetails[0];
                                            else
                                                caseEnrlCheckRecord = CaseEnrlDetails[CaseEnrlDetails.Count - 1];
                                            strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                        }
                                        if (!string.IsNullOrEmpty(strAttnDate))
                                        {
                                            //MessageBox.Show(Convert.ToDateTime(strAttnDate).ToString());
                                            strDate = Convert.ToDateTime(strAttnDate).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                        }
                                    }
                                    else if (chldtrackitem.INTAKEENTRY.Trim().Equals("D"))
                                    {
                                        if (!string.IsNullOrEmpty(strDateOfBirth))
                                        {
                                            // MessageBox.Show(Convert.ToDateTime(strDateOfBirth).ToString());
                                            strDate = Convert.ToDateTime(strDateOfBirth).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                        }

                                    }
                                }

                            }

                        }
                    }
                }

            }
            return strDate;
        }

        bool boolCheckFund = true;
        private void GetFund()
        {
            propFund = string.Empty;          //= "HS";//
            //CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            //Search_Entity.Agy = BaseForm.BaseAgency;
            //Search_Entity.Dept = BaseForm.BaseDept;
            //Search_Entity.Prog = BaseForm.BaseProg;
            //Search_Entity.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
            //Search_Entity.App = BaseForm.BaseApplicationNo;
            //Search_Entity.Rec_Type = "H";
            //Search_Entity.Status = "E";
            //Search_Entity.Enrl_Status_Not_Equalto = "T";
            //App_Mult_Fund_List = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity, "Browse");
            if (propCaseENrl.Count > 0)
            {
                if (propCaseENrl.Count == 1)
                {
                    List<CaseEnrlEntity> CaseEnrlDetails1 = propCaseENrl.FindAll(u => u.Status.Trim().Equals("E"));
                    if (CaseEnrlDetails1.Count > 0)
                        propFund = CaseEnrlDetails1[0].FundHie.ToString().Trim();
                    else
                        propFund = propCaseENrl[0].FundHie.ToString().Trim();
                }
                else
                {
                    CaseEnrlEntity caseEnrlFundFilter = propCaseENrl.Find(u => u.FundHie.Trim() == "HS");
                    if (caseEnrlFundFilter != null)
                    {
                        propFund = caseEnrlFundFilter.FundHie;
                    }
                    else
                    {
                        List<CaseEnrlEntity> CaseEnrlDetails1 = propCaseENrl.FindAll(u => u.Status.Trim().Equals("E"));
                        if (CaseEnrlDetails1.Count > 0)
                            propFund = CaseEnrlDetails1[0].FundHie.ToString().Trim();
                        else
                            propFund = propCaseENrl[0].FundHie.ToString().Trim();
                    }
                }

                //List<CaseEnrlEntity> CaseEnrlDetails = propCaseENrl.FindAll(u => u.Status.Trim().Equals("E"));
                //if (CaseEnrlDetails.Count > 0)
                //    propFund = CaseEnrlDetails[0].FundHie.ToString().Trim();
                //else
                //    propFund = propCaseENrl[0].FundHie.ToString().Trim();
            }
            else
            {

                ChldMstEntity chldmstentity = _model.ChldMstData.GetChldMstDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    "), BaseForm.BaseApplicationNo, string.Empty);
                if (chldmstentity != null)
                {
                    propFund = chldmstentity.FundSource.ToString().Trim();
                    if (propFund == string.Empty)
                    {
                        CaseVerEntity caseverEntity = _model.CaseMstData.GetCaseveradpynd(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    "), BaseForm.BaseApplicationNo, string.Empty, string.Empty);
                        if (caseverEntity != null)
                        {
                            propFund = caseverEntity.FundSource.ToString().Trim();
                            if (propFund == string.Empty)
                            {
                                List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                                if (AgyFundList.Count > 0)
                                {
                                    CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                                    if (headstartEntity != null)
                                    {
                                        propFund = headstartEntity.Code.Trim();

                                    }
                                    else
                                    {
                                        boolCheckFund = false;

                                    }

                                }
                            }
                        }
                        else
                        {
                            List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                            if (AgyFundList.Count > 0)
                            {
                                CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                                if (headstartEntity != null)
                                {
                                    propFund = headstartEntity.Code.Trim();

                                }
                                else
                                {
                                    boolCheckFund = false;

                                }

                            }
                        }
                    }

                }
                else
                {

                    CaseVerEntity caseverEntity = _model.CaseMstData.GetCaseveradpynd(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    "), BaseForm.BaseApplicationNo, string.Empty, string.Empty);
                    if (caseverEntity != null)
                    {
                        propFund = caseverEntity.FundSource.ToString().Trim();
                        if (propFund == string.Empty)
                        {
                            List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                            if (AgyFundList.Count > 0)
                            {
                                CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                                if (headstartEntity != null)
                                {
                                    propFund = headstartEntity.Code.Trim();

                                }
                                else
                                {
                                    boolCheckFund = false;

                                }

                            }
                        }
                    }
                    else
                    {
                        List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                        if (AgyFundList.Count > 0)
                        {
                            CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                            if (headstartEntity != null)
                            {
                                propFund = headstartEntity.Code.Trim();

                            }
                            else
                            {
                                boolCheckFund = false;

                            }

                        }
                    }
                }
            }
        }

        private string GetFundChilds(string strEnrlAgency, string strEnrlDept, string strEnrlProg, string strEnrlYear, string strEnrlApp)
        {
            string strpropFund = string.Empty;          //= "HS";//
            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            Search_Entity.Agy = strEnrlAgency;
            Search_Entity.Dept = strEnrlDept;
            Search_Entity.Prog = strEnrlProg;
            Search_Entity.Year = (!string.IsNullOrEmpty(strEnrlYear) ? strEnrlYear : "    ");
            Search_Entity.App = strEnrlApp;
            Search_Entity.Rec_Type = "H";
            Search_Entity.Status = "E";
            // Search_Entity.Enrl_Status_Not_Equalto = "T";
            List<CaseEnrlEntity> App_Mult_Fund_List = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity, "Browse");
            if (App_Mult_Fund_List.Count > 0)
            {
                if (App_Mult_Fund_List.Count == 1)
                {
                    strpropFund = App_Mult_Fund_List[0].FundHie.ToString().Trim();
                }
                else
                {
                    CaseEnrlEntity caseEnrlFundFilter = App_Mult_Fund_List.Find(u => u.FundHie.Trim() == "HS");
                    if (caseEnrlFundFilter != null)
                    {
                        strpropFund = caseEnrlFundFilter.FundHie;
                    }
                    else
                    {
                        strpropFund = App_Mult_Fund_List[0].FundHie.ToString().Trim();
                    }
                }

                strpropFund = App_Mult_Fund_List[0].FundHie.ToString().Trim();
            }
            else
            {

                ChldMstEntity chldmstentity = _model.ChldMstData.GetChldMstDetails(strEnrlAgency, strEnrlDept, strEnrlProg, (!string.IsNullOrEmpty(strEnrlYear.Trim()) ? strEnrlYear : "    "), strEnrlApp, string.Empty);
                if (chldmstentity != null)
                {
                    strpropFund = chldmstentity.FundSource.ToString().Trim();
                    if (strpropFund == string.Empty)
                    {
                        CaseVerEntity caseverEntity = _model.CaseMstData.GetCaseveradpynd(strEnrlAgency, strEnrlDept, strEnrlProg, (!string.IsNullOrEmpty(strEnrlYear.Trim()) ? strEnrlYear : "    "), strEnrlApp, string.Empty, string.Empty);
                        if (caseverEntity != null)
                        {
                            strpropFund = caseverEntity.FundSource.ToString().Trim();
                            if (strpropFund == string.Empty)
                            {
                                List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                                if (AgyFundList.Count > 0)
                                {
                                    CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                                    if (headstartEntity != null)
                                    {
                                        strpropFund = headstartEntity.Code.Trim();

                                    }
                                    else
                                    {
                                        boolCheckFund = false;

                                    }

                                }
                            }
                        }
                        else
                        {
                            List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                            if (AgyFundList.Count > 0)
                            {
                                CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                                if (headstartEntity != null)
                                {
                                    strpropFund = headstartEntity.Code.Trim();

                                }
                                else
                                {
                                    boolCheckFund = false;

                                }

                            }
                        }
                    }

                }
                else
                {

                    CaseVerEntity caseverEntity = _model.CaseMstData.GetCaseveradpynd(strEnrlAgency, strEnrlDept, strEnrlProg, (!string.IsNullOrEmpty(strEnrlYear.Trim()) ? strEnrlYear : "    "), strEnrlApp, string.Empty, string.Empty);
                    if (caseverEntity != null)
                    {
                        strpropFund = caseverEntity.FundSource.ToString().Trim();
                        if (strpropFund == string.Empty)
                        {
                            List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                            if (AgyFundList.Count > 0)
                            {
                                CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                                if (headstartEntity != null)
                                {
                                    strpropFund = headstartEntity.Code.Trim();

                                }
                                else
                                {
                                    boolCheckFund = false;

                                }

                            }
                        }
                    }
                    else
                    {
                        List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                        if (AgyFundList.Count > 0)
                        {
                            CommonEntity headstartEntity = AgyFundList.Find(u => u.Default == "Y");
                            if (headstartEntity != null)
                            {
                                strpropFund = headstartEntity.Code.Trim();

                            }
                            else
                            {
                                boolCheckFund = false;

                            }

                        }
                    }
                }
            }

            return strpropFund;
        }


        private void FillTaskGrid()
        {
            gvwSelectTask.Rows.Clear();
            if (BaseForm.BaseTaskEntity.Count > 0)
            {
                foreach (ChldTrckEntity item in BaseForm.BaseTaskEntity)
                {
                    int rowindex = gvwSelectTask.Rows.Add(item.TASKDESCRIPTION, string.Empty, item.TASK);
                    gvwSelectTask.Rows[rowindex].Tag = item;
                }
            }
            else
            {

                foreach (ChldTrckEntity item in propchldTrckList)
                {
                    int rowindex = gvwSelectTask.Rows.Add(item.TASKDESCRIPTION, string.Empty, item.TASK);
                    gvwSelectTask.Rows[rowindex].Tag = item;
                }
            }
            if (gvwSelectTask.Rows.Count > 0)
                gvwSelectTask.Rows[0].Selected = true;
        }

        private void FillComboTask()
        {
            cmbTask.SelectedIndexChanged -= new EventHandler(cmbTask_SelectedIndexChanged);
            cmbTask.Items.Clear();
            if (BaseForm.BaseTaskEntity.Count > 0)
            {
                foreach (ChldTrckEntity item in BaseForm.BaseTaskEntity)
                {
                    cmbTask.Items.Add(new ListItem(item.TASKDESCRIPTION, item.TASK));
                }
                cmbTask.SelectedIndex = 0;
                propTask = ((ListItem)cmbTask.SelectedItem).Value.ToString();
            }
            else
            {

                foreach (ChldTrckEntity item in propchldTrckList)
                {
                    cmbTask.Items.Add(new ListItem(item.TASKDESCRIPTION, item.TASK));
                }
                cmbTask.SelectedIndex = 0;
                propTask = ((ListItem)cmbTask.SelectedItem).Value.ToString();
            }
            cmbTask.SelectedIndexChanged += new EventHandler(cmbTask_SelectedIndexChanged);
        }



        private void FillCombo()
        {
            CaseSiteEntity casesite = new CaseSiteEntity(true);
            casesite.SiteAGENCY = BaseForm.BaseAgency;
            casesite.SiteDEPT = BaseForm.BaseDept;
            casesite.SitePROG = BaseForm.BaseProg;
            casesite.SiteYEAR = BaseForm.BaseYear;
            //casesite.SiteROOM = 
            List<CaseSiteEntity> caseSiteList = _model.CaseMstData.Browse_CASESITE(casesite, "Browse");
            if (caseSiteList.Count > 0)
                caseSiteList = caseSiteList.FindAll(u => u.SiteROOM.ToString() != "0000");
            List<CaseEnrlEntity> CaseEnrlDetails = propCaseENrl.FindAll(u => u.App.Equals(BaseForm.BaseApplicationNo));
            cmbClass.Items.Clear();
            cmbClass.SelectedIndexChanged -= new EventHandler(cmbClass_SelectedIndexChanged);
            cmbClass.Items.Insert(0, (new ListItem("  ", "0")));
            cmbClass.SelectedIndex = 0;
            if (caseSiteList.Count > 0)
            {
                foreach (CaseSiteEntity item in caseSiteList)
                {
                    string strRoom = item.SiteROOM;
                    strRoom = "0000".Substring(0, 4 - strRoom.Length) + strRoom;
                    ListItem li = new ListItem(item.SiteNUMBER.Trim() + "  " + strRoom + "  " + item.SiteAM_PM.Trim(), item.SiteNUMBER.Trim() + item.SiteROOM.Trim() + item.SiteAM_PM.Trim());
                    cmbClass.Items.Add(li);
                    if (CaseEnrlDetails.Count > 0)
                    {
                        if ((CaseEnrlDetails[0].Site.Trim() + CaseEnrlDetails[0].Room.Trim() + CaseEnrlDetails[0].AMPM.Trim()).Equals(item.SiteNUMBER.Trim() + item.SiteROOM.Trim() + item.SiteAM_PM.Trim()))
                            cmbClass.SelectedItem = li;
                    }

                }


            }

            cmbClass.SelectedIndexChanged += new EventHandler(cmbClass_SelectedIndexChanged);
            cmbClass_SelectedIndexChanged(cmbClass, new EventArgs());
        }

        void cmbTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTask.Items.Count > 0)
            {
                propTask = ((ListItem)cmbTask.SelectedItem).Value.ToString();
                //if (BaseForm.UserProfile.LoadData.Equals("1") && (!string.IsNullOrEmpty(BaseForm.BaseYear)))
                //{
                propChldmediList = _model.ChldTrckData.GetChldMediDetails1(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, propTask, string.Empty);
                //}
                //else
                //{
                //    propChldmediList = _model.ChldTrckData.GetChldMediDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, propTask, string.Empty);
                //}
                EnableDisableControls(string.Empty);
                if (Mode != "Add")
                {
                    gvwChildDetails_SelectionChanged(sender, e);
                }
                fillCustomQuestions(propchldTrckList[0].CustQCodes.Trim(), BaseForm.BaseApplicationNo, BaseForm.BaseYear, propChldmedi);
                if (chkClassRoom.Checked == false)
                {
                    if (gvwChildDetails.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow item in gvwChildDetails.Rows)
                        {
                            if (item.Cells["gvApplication"].Value.ToString() == BaseForm.BaseApplicationNo)
                            {
                                item.Cells["gvChkSelect"].Value = Img_Tick;
                                item.Cells["gvtSelectValue"].Value = "T";
                                item.Selected = true;
                                btnOk.Visible = true;
                                break;
                            }
                        }
                    }
                }

                cmbClass_SelectedIndexChanged(sender, e);
            }
        }

        void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvwChildDetails.SelectionChanged -= new EventHandler(gvwChildDetails_SelectionChanged);
            gvwChildDetails.CellDoubleClick -= new DataGridViewCellEventHandler(gvwChildDetails_CellDoubleClick);

            gvwChildDetails.Rows.Clear();
            List<ChldTrckREntity> chldtrckrlist = new List<ChldTrckREntity>();
            if (propchldTrckList.Count > 0)
            {
                chldtrckrlist = _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", propTask, string.Empty);
                propchldRtrckList = chldtrckrlist;
            }
            int rowIndex = 0;
            //If statement1 Start
            if (cmbClass.Items.Count > 0)
            {
                //If statement2 Start
                if (((ListItem)cmbClass.SelectedItem).Value.ToString() != "0")
                {
                    List<CaseEnrlEntity> caseEnrlChildList = propCaseENrl.FindAll(u => (u.Site + u.Room + u.AMPM).Equals(((ListItem)cmbClass.SelectedItem).Value.ToString()) && u.Status.Trim().Equals("E"));
                    var caseEnrl = caseEnrlChildList.Select(u => u.App).Distinct().ToList();

                    //If statement3 Start
                    if (caseEnrl.Count > 0)
                    {
                        List<CaseSnpEntity> casesnpOrderList = new List<CaseSnpEntity>();
                        foreach (var caseEnrlitem in caseEnrl)
                        {
                            CaseSnpEntity caseSnp = propCaseSnp.Find(u => u.App.Equals(caseEnrlitem));
                            if (caseSnp != null)
                            {
                                casesnpOrderList.Add(new CaseSnpEntity(caseSnp.NameixFi, caseSnp.NameixLast, caseSnp.NameixMi, caseSnp.App));
                            }
                        }
                        if (casesnpOrderList.Count > 0)
                        {
                            if (BaseForm.BaseHierarchyCnFormat == "1")
                            {
                                casesnpOrderList = casesnpOrderList.OrderBy(u => u.NameixFi).ThenBy(u => u.NameixLast).ToList();
                            }
                            else
                            {
                                casesnpOrderList = casesnpOrderList.OrderBy(u => u.NameixLast).ThenBy(u => u.NameixFi).ToList();
                            }
                        }
                        foreach (CaseSnpEntity caseSnp in casesnpOrderList)
                        {
                            //If statement4 Start
                            if (Mode != "Add")
                            {
                                if (_formType == "Components")
                                {
                                    rowIndex = gvwChildDetails.Rows.Add(Img_Cross, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "N", "Add", string.Empty);
                                    //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                                    //{
                                    string strSBCBDate = FillSBCDDetails(caseSnp.App.ToString(), caseSnp.Mst_IntakeDate, caseSnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                                    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                                    gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = caseSnp.App.ToString();
                                    //}
                                    //else
                                    //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
                                    if (propChldmediList != null)
                                    {
                                        List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(caseSnp.App.ToString()));

                                        if (chldmediDetails.Count > 0)
                                        {
                                            if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                            {
                                                chldmediDetails = chldmediDetails.OrderByDescending(u => u.SEQ).ToList();
                                            }
                                            else
                                            {
                                                chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                            }

                                            //chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();
                                            gvwChildDetails.Rows[rowIndex].Cells["gvtCompletDate"].Value = LookupDataAccess.Getdate(chldmediDetails[0].COMPLETED_DATE);
                                            if (caseSnp.App.ToString().Equals(BaseForm.BaseApplicationNo))
                                            {
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Blank;
                                                gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "E";
                                                gvwChildDetails.Rows[rowIndex].Selected = true;
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                            }
                                            else
                                            {
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Blank;
                                                gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "E";
                                            }
                                            gvwChildDetails.Rows[rowIndex].Cells["gvtMode"].Value = "Edit";

                                            // Murali added by Class room wise medi record existed same applicant added on 02/02/2021

                                            rowIndex = gvwChildDetails.Rows.Add(Img_Cross, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "N", "Add", string.Empty);

                                            gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                                            gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = caseSnp.App.ToString();
                                        }
                                    }
                                }
                                else if (_formType == "ClientTracking")
                                {
                                    if (chkClassRoom.Checked)
                                    {
                                        rowIndex = gvwChildDetails.Rows.Add(Img_Cross, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "N", "Add", string.Empty);
                                        //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                                        //{
                                        string strSBCBDate = FillSBCDDetails(caseSnp.App.ToString(), caseSnp.Mst_IntakeDate, caseSnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                                        gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                                        gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = caseSnp.App.ToString();
                                        //}
                                        //else
                                        //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
                                        if (propChldmediList != null)
                                        {
                                            List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(caseSnp.App.ToString()));

                                            if (chldmediDetails.Count > 0)
                                            {
                                                if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                                {
                                                    chldmediDetails = chldmediDetails.OrderByDescending(u => u.SEQ).ToList();
                                                }
                                                else
                                                {
                                                    chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                                }

                                                //chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();
                                                gvwChildDetails.Rows[rowIndex].Cells["gvtCompletDate"].Value = LookupDataAccess.Getdate(chldmediDetails[0].COMPLETED_DATE);
                                                if (caseSnp.App.ToString().Equals(BaseForm.BaseApplicationNo))
                                                {
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Blank;
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "E";
                                                    gvwChildDetails.Rows[rowIndex].Selected = true;
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                                }
                                                else
                                                {
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Blank;
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "E";
                                                }
                                                gvwChildDetails.Rows[rowIndex].Cells["gvtMode"].Value = "Edit";

                                                // Murali added by Class room wise medi record existed same applicant added on 02/02/2021

                                                rowIndex = gvwChildDetails.Rows.Add(Img_Cross, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "N", "Add", string.Empty);

                                                gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = caseSnp.App.ToString();


                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (caseSnp.App.ToString().Equals(BaseForm.BaseApplicationNo))
                                        {
                                            rowIndex = gvwChildDetails.Rows.Add(Img_Cross, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "N", "Add", string.Empty);
                                            gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = BaseForm.BaseApplicationNo;
                                            //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                                            //{
                                            string strSBCBDate = FillSBCDDetails(caseSnp.App.ToString(), caseSnp.Mst_IntakeDate, caseSnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                                            gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                                            //}
                                            //else
                                            //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
                                            if (propChldmediList != null)
                                            {
                                                List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(caseSnp.App.ToString()) && u.YEAR.Trim().ToString() == BaseForm.BaseYear.Trim().ToString());
                                                if (chldmediDetails.Count > 0)
                                                {
                                                    if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                                    {
                                                        chldmediDetails = chldmediDetails.OrderByDescending(u => u.SEQ).ToList();
                                                    }
                                                    else
                                                    {
                                                        chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                                    }
                                                    //chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvtCompletDate"].Value = LookupDataAccess.Getdate(chldmediDetails[0].COMPLETED_DATE);

                                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Blank;
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "B";
                                                    gvwChildDetails.Rows[rowIndex].Selected = true;
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvtMode"].Value = "Edit";

                                                    // Murali added this logic on 02/02/2021

                                                    rowIndex = gvwChildDetails.Rows.Add(Img_Cross, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "N", "Add", string.Empty);
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = BaseForm.BaseApplicationNo;
                                                    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);


                                                    break;

                                                }
                                            }


                                        }
                                    }
                                }
                            } //If statement4 End
                            else
                            {
                                if (chkClassRoom.Checked)
                                {
                                    rowIndex = gvwChildDetails.Rows.Add(Img_Blank, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "E", string.Empty, string.Empty);
                                    if (caseSnp.App.ToString().Equals(BaseForm.BaseApplicationNo))
                                    {
                                        //gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Tick;
                                        //gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "T";
                                        gvwChildDetails.Rows[rowIndex].Selected = true;
                                    }
                                    //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                                    //{
                                    string strSBCBDate = FillSBCDDetails(caseSnp.App.ToString(), caseSnp.Mst_IntakeDate, caseSnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                                    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                                    //}
                                    //else
                                    //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;

                                    if (propChldmediList != null)
                                    {
                                        List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(caseSnp.App.ToString()) && u.YEAR.Trim().ToString() == BaseForm.BaseYear.Trim().ToString());
                                        if (chldmediDetails.Count > 0)
                                        {
                                            if (caseSnp.App.ToString().Equals(BaseForm.BaseApplicationNo))
                                            {
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Cross;
                                                gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "N";
                                                btnOk.Visible = false;
                                            }
                                            gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                            gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Cross;
                                            gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "N";
                                        }
                                    }
                                    gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = caseSnp.App.ToString();
                                }
                                else
                                {
                                    if (caseSnp.App.ToString().Equals(BaseForm.BaseApplicationNo))
                                    {
                                        rowIndex = gvwChildDetails.Rows.Add(Img_Blank, caseSnp.App.ToString(), caseSnp.NameixFi, caseSnp.NameixLast, string.Empty, "E", string.Empty, string.Empty);
                                        gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = BaseForm.BaseApplicationNo;
                                        //gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Tick;
                                        //gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "T";
                                        if (propChldmediList != null)
                                        {
                                            List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(caseSnp.App.ToString()) && u.YEAR.Trim().ToString() == BaseForm.BaseYear.Trim().ToString());
                                            if (chldmediDetails.Count > 0)
                                            {
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Cross;
                                                gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "N";
                                                btnOk.Visible = false;
                                            }
                                        }

                                        gvwChildDetails.Rows[rowIndex].Selected = true;




                                        //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                                        //{
                                        string strSBCBDate = FillSBCDDetails(caseSnp.App.ToString(), caseSnp.Mst_IntakeDate, caseSnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                                        gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                                        //}
                                        //else
                                        //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;

                                        break;
                                    }
                                }
                            }
                        }
                    } //If statement3 End
                    else
                    {
                        cmbClass.SelectedIndexChanged -= new EventHandler(cmbClass_SelectedIndexChanged);
                        cmbClass.SelectedIndex = 0;
                        gvwChildDetails.Rows.Clear();
                        if (Mode.Equals("Add"))
                        {
                            //int rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationName, BaseForm.BaseApplicationNo, string.Empty, "T");
                            CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));
                            //int rowIndex = 0;
                            if (casesnp != null)
                                rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, casesnp.NameixFi, casesnp.NameixLast, string.Empty, "T", string.Empty, string.Empty);
                            else
                                rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, BaseForm.BaseCaseSnpEntity[0].NameixFi, BaseForm.BaseCaseSnpEntity[0].NameixLast, string.Empty, "T", string.Empty, string.Empty);
                            if (propChldmediList != null)
                            {
                                List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(BaseForm.BaseApplicationNo.ToString()) && u.YEAR.Trim().ToString() == BaseForm.BaseYear.Trim().ToString());
                                if (chldmediDetails.Count > 0)
                                {
                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Cross;
                                    gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "N";
                                    btnOk.Visible = false;
                                }
                            }

                            //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                            //{
                            //CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));
                            string strSBCBDate = FillSBCDDetails(BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].IntakeDate, casesnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                            gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                            //}
                            //else
                            //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
                        }
                        else
                        {//int rowIndex = 0;
                            //int rowIndex = gvwChildDetails.Rows.Add(Img_Cross, BaseForm.BaseApplicationName, BaseForm.BaseApplicationNo, string.Empty, "N", "Add", string.Empty);
                            CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                            if (casesnp != null)
                                rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, casesnp.NameixFi, casesnp.NameixLast, string.Empty, "T", string.Empty, string.Empty);
                            else
                                rowIndex = gvwChildDetails.Rows.Add(Img_Cross, BaseForm.BaseApplicationNo, BaseForm.BaseCaseSnpEntity[0].NameixFi, BaseForm.BaseCaseSnpEntity[0].NameixLast, string.Empty, "N", "Add", string.Empty);
                            gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = BaseForm.BaseApplicationNo;
                            if (propChldmediList != null)
                            {
                                List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(BaseForm.BaseApplicationNo.ToString()) && u.YEAR.Trim().ToString() == BaseForm.BaseYear.Trim().ToString());
                                if (chldmediDetails.Count > 0)
                                {
                                    if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                    {
                                        chldmediDetails = chldmediDetails.OrderByDescending(u => u.SEQ).ToList();
                                    }
                                    else
                                    {
                                        chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                    }
                                    //chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                    gvwChildDetails.Rows[rowIndex].Cells["gvtCompletDate"].Value = LookupDataAccess.Getdate(chldmediDetails[0].COMPLETED_DATE);

                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                    gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Blank;
                                    gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "E";
                                    gvwChildDetails.Rows[rowIndex].Cells["gvtMode"].Value = "Edit";
                                    //btnOk.Visible = false;                                   
                                }
                            }
                            //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                            //{
                            //CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));
                            string strSBCBDate = FillSBCDDetails(BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].IntakeDate, casesnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                            gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                            //}
                            //else
                            //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
                        }
                        cmbClass.SelectedIndexChanged += new EventHandler(cmbClass_SelectedIndexChanged);
                    }
                } //If statement2 End

                else
                {
                    gvwChildDetails.Rows.Clear();
                    if (Mode.Equals("Add"))
                    {
                        //int rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationName, BaseForm.BaseApplicationNo, string.Empty, "T");
                        CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                        if (casesnp != null)
                            rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, casesnp.NameixFi, casesnp.NameixLast, string.Empty, "T", string.Empty, string.Empty);
                        else
                            rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, BaseForm.BaseCaseSnpEntity[0].NameixFi, BaseForm.BaseCaseSnpEntity[0].NameixLast, string.Empty, "T");
                        gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = BaseForm.BaseApplicationNo;
                        if (propChldmediList != null)
                        {
                            List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(BaseForm.BaseApplicationNo.ToString()) && u.YEAR.Trim().ToString() == BaseForm.BaseYear.Trim().ToString());
                            if (chldmediDetails.Count > 0)
                            {
                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Cross;
                                gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "N";
                                btnOk.Visible = false;
                            }
                        }
                        //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                        //{
                        //CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));
                        string strSBCBDate = FillSBCDDetails(BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].IntakeDate, casesnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                        gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                        //}
                        //else
                        //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
                    }
                    else
                    {
                        //int rowIndex = gvwChildDetails.Rows.Add(Img_Cross, BaseForm.BaseApplicationName, BaseForm.BaseApplicationNo, string.Empty, "N", "Add", string.Empty);
                        CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                        if (casesnp != null)
                            rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, casesnp.NameixFi, casesnp.NameixLast, string.Empty, "T", string.Empty, string.Empty);
                        else
                            rowIndex = gvwChildDetails.Rows.Add(Img_Cross, BaseForm.BaseApplicationNo, BaseForm.BaseCaseSnpEntity[0].NameixFi, BaseForm.BaseCaseSnpEntity[0].NameixLast, string.Empty, "N", "Add", string.Empty);
                        gvwChildDetails.Rows[rowIndex].Cells["gvChildName"].ToolTipText = BaseForm.BaseApplicationNo;
                        if (propChldmediList != null)
                        {
                            List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(BaseForm.BaseApplicationNo.ToString()) && u.YEAR.Trim().ToString() == BaseForm.BaseYear.Trim().ToString());
                            if (chldmediDetails.Count > 0)
                            {
                                if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                {
                                    chldmediDetails = chldmediDetails.OrderByDescending(u => u.SEQ).ToList();
                                }
                                else
                                {
                                    chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                }
                                gvwChildDetails.Rows[rowIndex].Cells["gvtCompletDate"].Value = LookupDataAccess.Getdate(chldmediDetails[0].COMPLETED_DATE);
                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].ReadOnly = true;
                                gvwChildDetails.Rows[rowIndex].Cells["gvChkSelect"].Value = Img_Blank;
                                gvwChildDetails.Rows[rowIndex].Cells["gvtSelectValue"].Value = "E";
                                gvwChildDetails.Rows[rowIndex].Cells["gvtMode"].Value = "Edit";
                                // btnOk.Visible = false;                               

                            }
                        }

                        //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                        //{
                        //CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));
                        string strSBCBDate = FillSBCDDetails(BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].IntakeDate, casesnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                        gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                        //}
                        //else
                        //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
                    }
                }
                gvwChildDetails.CellDoubleClick += new DataGridViewCellEventHandler(gvwChildDetails_CellDoubleClick);
                //if (!Mode.Equals("Add"))
                //{
                //    //if (strcheckMode != string.Empty)
                //    //{
                //    gvwChildDetails.SelectionChanged += new EventHandler(gvwChildDetails_SelectionChanged);
                //    gvwChildDetails_SelectionChanged(sender, e);
                //    //}
                //}
            } //If statement1 End
            if (gvwChildDetails.Rows.Count > 0)
            {
                gvwChildDetails.CurrentCell = gvwChildDetails.Rows[0].Cells[0];
                gvwChildDetails.Rows[0].Selected = true;
            }

            if (!Mode.Equals("Add"))
            {
                //if (strcheckMode != string.Empty)
                //{
                gvwChildDetails.SelectionChanged += new EventHandler(gvwChildDetails_SelectionChanged);
                gvwChildDetails_SelectionChanged(sender, e);
                //}
            }
        }


        void FillRegularApplicantData()
        {
            //gvwChildDetails.SelectionChanged -= new EventHandler(gvwChildDetails_SelectionChanged);
            //gvwChildDetails.CellDoubleClick -= new DataGridViewCellEventHandler(gvwChildDetails_CellDoubleClick);
            gvwChildDetails.Rows.Clear();
            CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));
            int rowIndex = 0;
            if (casesnp != null)
                rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, casesnp.NameixFi, casesnp.NameixLast, string.Empty, "T");
            else
                rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationNo, BaseForm.BaseCaseSnpEntity[0].NameixFi, BaseForm.BaseCaseSnpEntity[0].NameixLast, string.Empty, "T");
            //int rowIndex = gvwChildDetails.Rows.Add(Img_Tick, BaseForm.BaseApplicationName, BaseForm.BaseApplicationNo, string.Empty, "T");

            if (Mode.Equals("Add"))
            {
                //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                //{
                List<ChldTrckREntity> chldtrckrlist = new List<ChldTrckREntity>();
                if (propchldTrckList.Count > 0)
                {
                    chldtrckrlist = _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", propTask, string.Empty);
                }


                string strSBCBDate = FillSBCDDetails(BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].IntakeDate, casesnp.AltBdate, string.Empty, propComponents, propTask, chldtrckrlist);
                gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = LookupDataAccess.Getdate(strSBCBDate);
                //}
                //else
                //    gvwChildDetails.Rows[rowIndex].Cells["gvSbcdDate"].Value = string.Empty;
            }
            // gvwChildDetails.SelectionChanged += new EventHandler(gvwChildDetails_SelectionChanged);
            // gvwChildDetails.CellDoubleClick += new DataGridViewCellEventHandler(gvwChildDetails_CellDoubleClick);
            //gvwChildDetails_SelectionChanged(gvwChildDetails, new EventArgs());
        }

        void gvwChildDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow item in gvwChildDetails.Rows)
            {
                if (item.Selected)
                {
                    List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(item.Cells["gvApplication"].Value.ToString()));
                    if (chldmediDetails.Count > 0)
                    {
                        if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                        {
                            chldmediDetails = chldmediDetails.OrderByDescending(u => u.SEQ).ToList();
                        }
                        else
                        {
                            chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                        }
                        if (propRegularForm == string.Empty && Mode != "Add")
                        {
                            if (propPostForm != "QuickPost")
                            {
                                if (BaseForm.UserProfile.LoadData.Equals("2") && (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim())))
                                {

                                    List<ChldMediEntity> chldMediFilterList = new List<ChldMediEntity>();
                                    foreach (ChldMediEntity mediEntity in chldmediDetails)
                                    {
                                        if (!string.IsNullOrEmpty(mediEntity.YEAR.Trim()))
                                        {
                                            if (Convert.ToInt32(mediEntity.YEAR) < Convert.ToInt32(BaseForm.BaseYear))
                                            {
                                                chldMediFilterList.Add(mediEntity);
                                            }
                                        }
                                    }
                                    if (chldMediFilterList.Count > 0)
                                    {
                                        if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                        {
                                            chldMediFilterList = chldMediFilterList.OrderByDescending(u => u.SEQ).ToList();
                                        }
                                        else
                                        {
                                            chldMediFilterList = chldMediFilterList.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                        }
                                        // chldMediFilterList = chldMediFilterList.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                        if (chldMediFilterList.Count == 1)
                                        {
                                            ClearControls(string.Empty);
                                            propChldmedi = chldMediFilterList[0];
                                            FillForm(string.Empty);
                                            if (Privileges.ChangePriv.Equals("false"))
                                            {
                                                btnOk.Visible = false;
                                                pnlDates.Enabled = false;
                                                pnlGridNotes.Enabled = false;
                                            }
                                            else
                                            {
                                                btnOk.Visible = true;
                                                pnlDates.Enabled = true;
                                                pnlGridNotes.Enabled = true;
                                            }
                                        }
                                        else
                                        {

                                            HSS00134Popup hss00134popup = new HSS00134Popup(BaseForm, Privileges, chldMediFilterList);
                                            hss00134popup.FormClosed += new FormClosedEventHandler(hss00134popup_FormClosed);
                                            hss00134popup.StartPosition = FormStartPosition.CenterScreen;
                                            hss00134popup.ShowDialog();
                                        }
                                    }
                                    else
                                    {
                                        ClearControls(item.Cells["gvApplication"].Value.ToString());
                                        btnOk.Visible = false;
                                    }


                                }
                                else
                                {

                                    pnlDates.Enabled = true;
                                    pnlGridNotes.Enabled = true;
                                    btnOk.Visible = true;
                                    if (chldmediDetails.Count == 1)
                                    {
                                        ClearControls(string.Empty);
                                        pnlDates.Enabled = true;
                                        pnlGridNotes.Enabled = true;
                                        btnOk.Visible = true;
                                        propChldmedi = chldmediDetails[0];
                                        FillForm(string.Empty);
                                    }
                                    else
                                    {

                                        HSS00134Popup hss00134popup = new HSS00134Popup(BaseForm, Privileges, chldmediDetails);
                                        hss00134popup.FormClosed += new FormClosedEventHandler(hss00134popup_FormClosed);
                                        hss00134popup.StartPosition = FormStartPosition.CenterScreen;
                                        hss00134popup.ShowDialog();
                                    }
                                }
                            }
                            else
                            {
                                if (item.Cells["gvtMode"].Value.ToString().ToUpper() != "ADD")
                                {
                                    if (chldmediDetails.Count == 1)
                                    {
                                        ClearControls(string.Empty);
                                        //panel2.Enabled = true;
                                        //panel4.Enabled = true;
                                        //btnOk.Visible = true;
                                        propChldmedi = chldmediDetails[0];
                                        FillForm(string.Empty);
                                        if (Privileges.ChangePriv.Equals("false"))
                                        {
                                            btnOk.Visible = false;
                                            pnlDates.Enabled = false;
                                            pnlGridNotes.Enabled = false;
                                        }
                                        else
                                        {
                                            btnOk.Visible = true;
                                            pnlDates.Enabled = true;
                                            pnlGridNotes.Enabled = true;
                                        }
                                    }
                                    else
                                    {

                                        HSS00134Popup hss00134popup = new HSS00134Popup(BaseForm, Privileges, chldmediDetails);
                                        hss00134popup.FormClosed += new FormClosedEventHandler(hss00134popup_FormClosed);
                                        hss00134popup.StartPosition = FormStartPosition.CenterScreen;
                                        hss00134popup.ShowDialog();

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Mode.Equals("Add"))
                            {
                                HSS00134Popup hss00134popup = new HSS00134Popup(BaseForm, Privileges, chldmediDetails, string.Empty);
                                hss00134popup.FormClosed += new FormClosedEventHandler(hss00134popup_FormClosed);
                                hss00134popup.StartPosition = FormStartPosition.CenterScreen;
                                hss00134popup.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        //if (propRegularForm == string.Empty && Mode != "Add")
                        //{
                        //    ClearControls();
                        //    if (Privileges.ChangePriv.Equals("false"))
                        //    {
                        //        btnOk.Visible = false;
                        //        panel2.Enabled = false;
                        //        panel4.Enabled = false;
                        //    }
                        //    else
                        //    {
                        //        btnOk.Visible = true;
                        //        panel2.Enabled = true;
                        //        panel4.Enabled = true;
                        //    }
                        //   // btnOk.Visible = false;
                        //}

                    }

                }
            }

        }

        void gvwChildDetails_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwChildDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in gvwChildDetails.Rows)
                {
                    if (item.Selected)
                    {
                        // strGridIndex = item.Index;
                        if (propChldmediList != null)
                        {
                            if (propPostForm == "QuickPost" && item.Cells["gvtMode"].Value.ToString().ToUpper() == "ADD")
                            {
                                ClearControls(item.Cells["gvApplication"].Value.ToString());
                                if (Privileges.AddPriv.Equals("false"))
                                {
                                    btnOk.Visible = false;
                                    pnlDates.Enabled = false;
                                    pnlGridNotes.Enabled = false;
                                }
                                else
                                {
                                    btnOk.Visible = true;
                                    pnlDates.Enabled = true;
                                    pnlGridNotes.Enabled = true;
                                }

                            }
                            else
                            {
                                List<ChldMediEntity> chldmediDetails = propChldmediList.FindAll(u => u.APP_NO.Equals(item.Cells["gvApplication"].Value.ToString()));
                                if (propPostForm == "QuickPost")
                                    chldmediDetails = chldmediDetails.FindAll(u => u.TASK == propTask);
                                if (chldmediDetails.Count > 0)
                                {
                                    if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                    {
                                        chldmediDetails = chldmediDetails.OrderByDescending(u => u.SEQ).ToList();
                                    }
                                    else
                                    {
                                        chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                    }
                                    //chldmediDetails = chldmediDetails.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                    if (propPostForm != "QuickPost")
                                    {
                                        if (BaseForm.UserProfile.LoadData.Equals("2") && (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim())))
                                        {

                                            List<ChldMediEntity> chldMediFilterList = new List<ChldMediEntity>();
                                            foreach (ChldMediEntity mediEntity in chldmediDetails)
                                            {
                                                if (!string.IsNullOrEmpty(mediEntity.YEAR.Trim()))
                                                {
                                                    if (Convert.ToInt32(mediEntity.YEAR) < Convert.ToInt32(BaseForm.BaseYear))
                                                    {
                                                        chldMediFilterList.Add(mediEntity);
                                                    }
                                                }
                                            }
                                            if (chldMediFilterList.Count > 0)
                                            {
                                                ClearControls(string.Empty);
                                                if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                                                {
                                                    chldMediFilterList = chldMediFilterList.OrderByDescending(u => u.SEQ).ToList();
                                                }
                                                else
                                                {
                                                    chldMediFilterList = chldMediFilterList.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                                }
                                                //chldMediFilterList = chldMediFilterList.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => Convert.ToInt32(u.YEAR)).ToList();
                                                pnlDates.Enabled = true;
                                                pnlGridNotes.Enabled = true;
                                                btnOk.Visible = true;
                                                if (chldMediFilterList.Count == 1)
                                                {
                                                    propChldmedi = chldMediFilterList[0];
                                                    FillForm(string.Empty);
                                                }
                                                else
                                                {
                                                    //propChldmedi = chldMediFilterList[chldMediFilterList.Count - 1];
                                                    propChldmedi = chldMediFilterList[0];
                                                    FillForm(string.Empty);
                                                }
                                            }
                                            else
                                            {
                                                ClearControls(item.Cells["gvApplication"].Value.ToString());
                                                btnOk.Visible = false;
                                            }

                                        }
                                        else
                                        {
                                            ClearControls(string.Empty);
                                            pnlDates.Enabled = true;
                                            pnlGridNotes.Enabled = true;
                                            btnOk.Visible = true;
                                            propChldmedi = chldmediDetails[0];
                                            FillForm(string.Empty);

                                        }
                                    }
                                    else
                                    {
                                        ClearControls(item.Cells["gvApplication"].Value.ToString());
                                        if (Privileges.ChangePriv.Equals("false"))
                                        {
                                            btnOk.Visible = false;
                                            pnlDates.Enabled = false;
                                            pnlGridNotes.Enabled = false;
                                        }
                                        else
                                        {
                                            btnOk.Visible = true;
                                            pnlDates.Enabled = true;
                                            pnlGridNotes.Enabled = true;
                                        }
                                        propChldmedi = chldmediDetails[0];
                                        FillForm(string.Empty);

                                    }
                                }
                                else
                                {
                                    ClearControls(item.Cells["gvApplication"].Value.ToString());
                                    CaseSnpEntity caseSnp = propCaseSnp.Find(u => u.App.Equals(item.Cells["gvApplication"].Value.ToString()));

                                    string strSBCBDate = FillSBCDDetailsNext(item.Cells["gvApplication"].Value.ToString(), caseSnp.Mst_IntakeDate, caseSnp.AltBdate, string.Empty, propComponents, propTask, propchldRtrckList);
                                    if (strSBCBDate != string.Empty)
                                    {
                                        dtSBCB.Value = Convert.ToDateTime(strSBCBDate);
                                        dtSBCB.Checked = true;
                                    }
                                    else
                                        dtSBCB.Checked = false;


                                    if (Privileges.AddPriv.Equals("false"))
                                    {
                                        btnOk.Visible = false;
                                        pnlDates.Enabled = false;
                                        pnlGridNotes.Enabled = false;
                                    }
                                    else
                                    {
                                        btnOk.Visible = true;
                                        pnlDates.Enabled = true;
                                        pnlGridNotes.Enabled = true;
                                    }


                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ClearControls(string.Empty);
                btnOk.Visible = false;
            }

        }

        void hss00134popup_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSS00134Popup hss00134pop = sender as HSS00134Popup;
            if (hss00134pop.DialogResult == DialogResult.OK)
            {
                propChldmedi = hss00134pop.chldmediDetails;
                FillForm(string.Empty);

            }

        }

        private void FillForm(string strPost)
        {
            if (propChldmedi != null)
            {
                // btnCaseNotes.Enabled = true;
                dtCompleted.Checked = false;
                dtAddressed.Checked = false;
                dtDiagnosed.Checked = false;
                dtFollowup.Checked = false;
                dtSBCB.Checked = false;
                dtSSR.Checked = false;
                if (!string.IsNullOrEmpty(propChldmedi.COMPLETED_DATE))
                {
                    dtCompleted.Value = Convert.ToDateTime(propChldmedi.COMPLETED_DATE);
                    dtCompleted.Checked = true;
                }
                if (!string.IsNullOrEmpty(propChldmedi.ADDRESSED_DATE))
                {
                    dtAddressed.Value = Convert.ToDateTime(propChldmedi.ADDRESSED_DATE);
                    dtAddressed.Checked = true;
                }
                if (!string.IsNullOrEmpty(propChldmedi.DIAGNOSIS_DATE))
                {
                    dtDiagnosed.Value = Convert.ToDateTime(propChldmedi.DIAGNOSIS_DATE);
                    dtDiagnosed.Checked = true;
                }
                if (!string.IsNullOrEmpty(propChldmedi.FOLLOWUP_DATE))
                {
                    dtFollowup.Value = Convert.ToDateTime(propChldmedi.FOLLOWUP_DATE);
                    dtFollowup.Checked = true;
                }
                if (!string.IsNullOrEmpty(propChldmedi.FOLLOWUPC_DATE))
                {
                    dtFupCompleted.Value = Convert.ToDateTime(propChldmedi.FOLLOWUPC_DATE);
                    dtFupCompleted.Checked = true;
                }
                if (!string.IsNullOrEmpty(propChldmedi.SBCB_DATE))
                {
                    dtSBCB.Value = Convert.ToDateTime(propChldmedi.SBCB_DATE);
                    dtSBCB.Checked = true;
                }
                if (!string.IsNullOrEmpty(propChldmedi.SPECIAL_DATE))
                {
                    dtSSR.Value = Convert.ToDateTime(propChldmedi.SPECIAL_DATE);
                    dtSSR.Checked = true;
                }
                txtWheree.Text = propChldmedi.SPECIAL_WHERE;
                CommonFunctions.SetComboBoxValue(cmbFund, propChldmedi.MediFund);
                CASEREFEntity caseRefName = CaseRefList.Find(u => u.Code.Equals(txtWheree.Text));
                if (caseRefName != null)
                {
                    txtAgency.Text = caseRefName.Name1;
                }
                else
                    txtAgency.Text = string.Empty;


                lbldocupldName.Text = propChldmedi.TSKUPLD_ORIG_NAME;
                TaskUpldID = propChldmedi.TSKUPLD_ID;
                _prvDocFile = propChldmedi.TSKUPLD_ORIG_NAME;

                // pbCaseNotes.Visible = true;
                propSeq = propChldmedi.SEQ.Trim();
                if (strPost != "PostView")
                {
                    if (propchldTrckList.Count > 0)
                    {
                        fillCustomQuestions(propchldTrckList[0].CustQCodes.Trim(), propChldmedi.APP_NO, propChldmedi.YEAR, propChldmedi);
                    }
                    ShowCaseNotesImages(propChldmedi.SEQ.Trim(), propChldmedi.APP_NO, propChldmedi.YEAR);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        public string _CompFormSaveFlag = "";
        private void btnOk_Click(object sender, EventArgs e)
        {
            string strMediSeq = string.Empty;
            _CompFormSaveFlag = "";
            bool boolSaveMode = false;
            if (ValidateForm())
            {
                if (propPostForm == string.Empty)
                {
                    if (Mode.Equals("Add"))
                    {
                        List<DataGridViewRow> SelectedgvRows = (from c in gvwChildDetails.Rows.Cast<DataGridViewRow>().ToList()
                                                                where (c.Cells["gvtSelectValue"].Value.ToString().ToUpper().Equals("T"))
                                                                select c).ToList();
                        if (SelectedgvRows.Count > 0)
                        {

                            foreach (DataGridViewRow gvrow in SelectedgvRows)
                            {
                                ChldMediEntity mediEntity = new ChldMediEntity();
                                mediEntity.AGENCY = BaseForm.BaseAgency;
                                mediEntity.DEPT = BaseForm.BaseDept;
                                mediEntity.PROG = BaseForm.BaseProg;
                                if (Mode.Equals("Edit"))
                                    mediEntity.YEAR = propChldmedi.YEAR;
                                else
                                    mediEntity.YEAR = BaseForm.BaseYear;
                                mediEntity.APP_NO = gvrow.Cells["gvApplication"].Value.ToString();
                                mediEntity.TASK = propTask;
                                if (dtAddressed.Checked == true)
                                    mediEntity.ADDRESSED_DATE = dtAddressed.Value.ToString();
                                if (dtCompleted.Checked == true)
                                    mediEntity.COMPLETED_DATE = dtCompleted.Value.ToString();
                                if (dtDiagnosed.Checked == true)
                                    mediEntity.DIAGNOSIS_DATE = dtDiagnosed.Value.ToString();
                                if (dtFollowup.Checked == true)
                                    mediEntity.FOLLOWUP_DATE = dtFollowup.Value.ToString();
                                if (dtFupCompleted.Checked == true)
                                    mediEntity.FOLLOWUPC_DATE = dtFupCompleted.Value.ToString();
                                //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                                //{
                                //if (gvrow.Cells["gvSbcdDate"].Value.ToString() != string.Empty)
                                //{
                                //mediEntity.SBCB_DATE = gvrow.Cells["gvSbcdDate"].Value.ToString();
                                // }
                                if (dtSBCB.Checked == true)
                                    mediEntity.SBCB_DATE = dtSBCB.Value.ToString();
                                //}
                                //else
                                //{
                                //    if (dtSBCB.Checked == true)
                                //        mediEntity.SBCB_DATE = dtSBCB.Value.ToString();
                                //}
                                if (dtSSR.Checked == true)
                                    mediEntity.SPECIAL_DATE = dtSSR.Value.ToString();
                                mediEntity.SPECIAL_WHERE = txtWheree.Text;
                                mediEntity.LSTC_OPERATOR = BaseForm.UserID;
                                mediEntity.ADD_OPERATOR = BaseForm.UserID;
                                mediEntity.COMPONENT = propComponents;
                                //**if (((ListItem)cmbFund.SelectedItem).Value.ToString() != "0")
                                if (cmbFund.Items.Count > 0)
                                    mediEntity.MediFund = ((ListItem)cmbFund.SelectedItem).Value.ToString();

                                foreach (DataGridViewRow dataGridViewRow in gvwCustomQuestions.Rows)
                                {
                                    if (true)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                                    {
                                        string inputValue = string.Empty;
                                        string strResponse = string.Empty;
                                        inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                                        strResponse = dataGridViewRow.Cells["Response"].Tag != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                                        if (dataGridViewRow.Cells[1].Tag == null && (dataGridViewRow.Cells[1].Tag != null && !((string)dataGridViewRow.Cells[1].Tag).Equals("U")))
                                        {
                                            continue;
                                        }


                                        CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;

                                        if (questionEntity.CUSTRESPTYPE.Equals("D") || questionEntity.CUSTRESPTYPE.Equals("C"))
                                        {
                                            if (dataGridViewRow.Cells["Response"].Tag == null || strResponse == string.Empty) continue;
                                            mediEntity.ANSWER1 = dataGridViewRow.Cells["Response"].Tag.ToString();
                                        }
                                        else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                                        {
                                            if (inputValue == string.Empty) continue;
                                            mediEntity.ANSWER2 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                        }
                                        else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                                        {
                                            if (inputValue == string.Empty || inputValue == "  /  /") continue;
                                            mediEntity.ANSWER3 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                        }
                                        else
                                        {
                                            if (inputValue == string.Empty) continue;
                                            mediEntity.ANSWER1 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                            if (inputValue != string.Empty)
                                            {
                                                mediEntity.ANSWER2 = CustomQuesValidation(mediEntity.ANSWER1);
                                            }
                                        }

                                    }
                                }

                                mediEntity.SEQ = propSeq;
                                mediEntity.SN = string.Empty;
                                mediEntity.Mode = Mode;


                                if (_model.ChldTrckData.InsertUpdateDelChldmedi(mediEntity, out strMediSeq))
                                {
                                    _CompFormSaveFlag = "Y";
                                    boolSaveMode = true;
                                    propSaveForm = true;
                                    string strYear = "    ";
                                    if (!string.IsNullOrEmpty(mediEntity.YEAR.Trim()))
                                    {
                                        strYear = mediEntity.YEAR;
                                    }
                                    if (!string.IsNullOrEmpty(txtCaseNotes.Text))
                                    {
                                        InsertDelCaseNotes(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO + propTask + strMediSeq, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO, string.Empty);
                                    }
                                    //else
                                    //{
                                    //    InsertDelCaseNotes(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO + propTask + strMediSeq, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO, "Del");
                                    //}
                                    //saveCustQuestions(strMediSeq, mediEntity.APP_NO, strYear);


                                    SaveTaskDocUpload(mediEntity, Mode, strMediSeq);

                                    HSS00134Control hSS00134Control = BaseForm.GetBaseUserControl() as HSS00134Control;
                                    if (hSS00134Control != null)
                                    {

                                        if (BaseForm.UserProfile.FastLoad.Equals("Y"))
                                        {
                                            if (Mode.Equals("Add"))
                                            {
                                                hSS00134Control.RefreshFastLoadGrid();
                                                this.Close();
                                            }
                                        }
                                        else
                                        {
                                            hSS00134Control.RefreshGrid();
                                            this.Close();
                                        }
                                        AlertBox.Show("Saved Successfully");
                                    }
                                    else
                                    {
                                        if (_CompFormSaveFlag == "Y")
                                        {
                                            AlertBox.Show("Saved Successfully");
                                            this.Close();
                                        }
                                    }

                                }
                            }
                            //if (boolSaveMode)
                            //{
                            //    btnOk.Visible = false;
                            //    foreach (DataGridViewRow dataGridViewRow in gvwChildDetails.Rows)
                            //    {
                            //        dataGridViewRow.Cells["gvChkSelect"].Value = false;
                            //    }
                            //    ClearControls();
                            //    panel2.Enabled = true;
                            //    panel4.Enabled = true;
                            //}


                        }
                        else
                        {
                            AlertBox.Show("Please select atleast One Child", MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        propSaveForm = true;
                        ChldMediEntity mediEntity = new ChldMediEntity();
                        mediEntity.AGENCY = BaseForm.BaseAgency;
                        mediEntity.DEPT = BaseForm.BaseDept;
                        mediEntity.PROG = BaseForm.BaseProg;
                        if (Mode.Equals("Edit"))
                            mediEntity.YEAR = propChldmedi.YEAR;
                        else
                            mediEntity.YEAR = BaseForm.BaseYear;
                        mediEntity.APP_NO = gvwChildDetails.SelectedRows[0].Cells["gvApplication"].Value.ToString();
                        mediEntity.TASK = propTask;
                        if (dtAddressed.Checked == true)
                            mediEntity.ADDRESSED_DATE = dtAddressed.Value.ToString();
                        if (dtCompleted.Checked == true)
                            mediEntity.COMPLETED_DATE = dtCompleted.Value.ToString();
                        if (dtDiagnosed.Checked == true)
                            mediEntity.DIAGNOSIS_DATE = dtDiagnosed.Value.ToString();
                        if (dtFollowup.Checked == true)
                            mediEntity.FOLLOWUP_DATE = dtFollowup.Value.ToString();
                        if (dtFupCompleted.Checked == true)
                            mediEntity.FOLLOWUPC_DATE = dtFupCompleted.Value.ToString();
                        if (dtSBCB.Checked == true)
                            mediEntity.SBCB_DATE = dtSBCB.Value.ToString();
                        if (dtSSR.Checked == true)
                            mediEntity.SPECIAL_DATE = dtSSR.Value.ToString();
                        mediEntity.SPECIAL_WHERE = txtWheree.Text;
                        mediEntity.LSTC_OPERATOR = BaseForm.UserID;
                        mediEntity.ADD_OPERATOR = BaseForm.UserID;
                        mediEntity.COMPONENT = propComponents;
                        //**if (((ListItem)cmbFund.SelectedItem).Value.ToString() != "0")
                        if (cmbFund.Items.Count > 0)
                            mediEntity.MediFund = ((ListItem)cmbFund.SelectedItem).Value.ToString();

                        mediEntity.SEQ = propSeq;
                        mediEntity.SN = string.Empty;


                        foreach (DataGridViewRow dataGridViewRow in gvwCustomQuestions.Rows)
                        {
                            if (true)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                            {
                                string inputValue = string.Empty;
                                string strResponse = string.Empty;
                                inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                                strResponse = dataGridViewRow.Cells["Response"].Tag != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                                if (dataGridViewRow.Cells[1].Tag == null && (dataGridViewRow.Cells[1].Tag != null && !((string)dataGridViewRow.Cells[1].Tag).Equals("U")))
                                {
                                    continue;
                                }


                                CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;

                                if (questionEntity.CUSTRESPTYPE.Equals("D") || questionEntity.CUSTRESPTYPE.Equals("C"))
                                {
                                    if (dataGridViewRow.Cells["Response"].Tag == null || strResponse == string.Empty) continue;
                                    mediEntity.ANSWER1 = dataGridViewRow.Cells["Response"].Tag.ToString();
                                }
                                else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                                {
                                    if (inputValue == string.Empty) continue;
                                    mediEntity.ANSWER2 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                }
                                else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                                {
                                    if (inputValue == string.Empty) continue;
                                    mediEntity.ANSWER3 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                }
                                else
                                {
                                    if (inputValue == string.Empty) continue;
                                    mediEntity.ANSWER1 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                    if (inputValue != string.Empty)
                                    {
                                        mediEntity.ANSWER2 = CustomQuesValidation(mediEntity.ANSWER1);
                                    }
                                }

                            }
                        }

                        mediEntity.Mode = Mode;
                        if (_model.ChldTrckData.InsertUpdateDelChldmedi(mediEntity, out strMediSeq))
                        {
                            _CompFormSaveFlag = "Y";
                            string strYear = "    ";
                            if (!string.IsNullOrEmpty(mediEntity.YEAR.Trim()))
                            {
                                strYear = mediEntity.YEAR;
                            }
                            if (!string.IsNullOrEmpty(txtCaseNotes.Text))
                            {
                                InsertDelCaseNotes(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO + propTask + strMediSeq, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO, string.Empty);
                            }
                            else
                            {
                                InsertDelCaseNotes(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO + propTask + strMediSeq, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO, "Del");
                            }
                            //  saveCustQuestions(strMediSeq, mediEntity.APP_NO, strYear);

                            //if (BaseForm.UserProfile.LoadData.Equals("1") && (!string.IsNullOrEmpty(BaseForm.BaseYear)))
                            //{
                            //    propChldmediList = _model.ChldTrckData.GetChldMediDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, propTask, string.Empty);
                            //}
                            //else
                            //{
                            //    propChldmediList = _model.ChldTrckData.GetChldMediDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, propTask, string.Empty);
                            //}
                            // gvwChildDetails_SelectionChanged(sender, e);


                            SaveTaskDocUpload(mediEntity, Mode, strMediSeq);

                            HSS00134Control hSS00134Control = BaseForm.GetBaseUserControl() as HSS00134Control;
                            if (hSS00134Control != null)
                            {
                                hSS00134Control.RefreshGrid();
                                this.Close();
                                AlertBox.Show("Saved Successfully");
                            }
                            else
                            {
                                if (_CompFormSaveFlag == "Y")
                                {
                                    AlertBox.Show("Saved Successfully");
                                    this.Close();
                                }
                            }
                        }
                    }
                }
                else
                {
                    // if (Mode.Equals("Add"))
                    //{
                    //    List<DataGridViewRow> SelectedgvRows = (from c in gvwChildDetails.Rows.Cast<DataGridViewRow>().ToList()
                    //                                            where (c.Cells["gvtSelectValue"].Value.ToString().ToUpper().Equals("T"))
                    //                                            select c).ToList();
                    //    if (SelectedgvRows.Count > 0)
                    //    {
                    //        foreach (DataGridViewRow item in SelectedgvRows)
                    //        {


                    //            ChldMediEntity mediEntity = new ChldMediEntity();
                    //            mediEntity.AGENCY = BaseForm.BaseAgency;
                    //            mediEntity.DEPT = BaseForm.BaseDept;
                    //            mediEntity.PROG = BaseForm.BaseProg;
                    //            if (Mode.Equals("Edit"))
                    //                mediEntity.YEAR = propChldmedi.YEAR;
                    //            else
                    //                mediEntity.YEAR = BaseForm.BaseYear;
                    //            mediEntity.APP_NO = item.Cells["gvApplication"].Value.ToString();
                    //            mediEntity.TASK = propTask;
                    //            if (dtAddressed.Checked == true)
                    //                mediEntity.ADDRESSED_DATE = dtAddressed.Value.ToString();
                    //            if (dtCompleted.Checked == true)
                    //                mediEntity.COMPLETED_DATE = dtCompleted.Value.ToString();
                    //            if (dtDiagnosed.Checked == true)
                    //                mediEntity.DIAGNOSIS_DATE = dtDiagnosed.Value.ToString();
                    //            if (dtFollowup.Checked == true)
                    //                mediEntity.FOLLOWUP_DATE = dtFollowup.Value.ToString();
                    //            if (dtFupCompleted.Checked == true)
                    //                mediEntity.FOLLOWUPC_DATE = dtFupCompleted.Value.ToString();

                    //            if (dtSBCB.Checked == true)
                    //                mediEntity.SBCB_DATE = dtSBCB.Value.ToString();

                    //            if (dtSSR.Checked == true)
                    //                mediEntity.SPECIAL_DATE = dtSSR.Value.ToString();
                    //            mediEntity.SPECIAL_WHERE = txtWheree.Text;
                    //            mediEntity.LSTC_OPERATOR = BaseForm.UserID;
                    //            mediEntity.ADD_OPERATOR = BaseForm.UserID;
                    //            mediEntity.COMPONENT = propComponents;

                    //            foreach (DataGridViewRow dataGridViewRow in gvwCustomQuestions.Rows)
                    //            {
                    //                if (true)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                    //                {
                    //                    string inputValue = string.Empty;
                    //                    string strResponse = string.Empty;
                    //                    inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                    //                    strResponse = dataGridViewRow.Cells["Response"].Tag != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                    //                    if (dataGridViewRow.Cells[1].Tag == null && (dataGridViewRow.Cells[1].Tag != null && !((string)dataGridViewRow.Cells[1].Tag).Equals("U")))
                    //                    {
                    //                        continue;
                    //                    }


                    //                    CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;

                    //                    if (questionEntity.CUSTRESPTYPE.Equals("D") || questionEntity.CUSTRESPTYPE.Equals("C"))
                    //                    {
                    //                        if (dataGridViewRow.Cells["Response"].Tag == null || strResponse == string.Empty) continue;
                    //                        mediEntity.ANSWER1 = dataGridViewRow.Cells["Response"].Tag.ToString();
                    //                    }
                    //                    else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                    //                    {
                    //                        if (inputValue == string.Empty) continue;
                    //                        mediEntity.ANSWER2 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    //                    }
                    //                    else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                    //                    {
                    //                        if (inputValue == string.Empty) continue;
                    //                        mediEntity.ANSWER3 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    //                    }
                    //                    else
                    //                    {
                    //                        if (inputValue == string.Empty) continue;
                    //                        mediEntity.ANSWER1 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    //                    }

                    //                }
                    //            }

                    //            mediEntity.SEQ = propSeq;
                    //            mediEntity.SN = string.Empty;
                    //            mediEntity.Mode = Mode;


                    //            if (_model.ChldTrckData.InsertUpdateDelChldmedi(mediEntity, out strMediSeq))
                    //            {
                    //                mediEntity.SEQ = strMediSeq;
                    //                boolSaveMode = true;
                    //                propSaveForm = true;
                    //                //btnOk.Visible = false;
                    //                string strYear = "    ";
                    //                if (!string.IsNullOrEmpty(mediEntity.YEAR))
                    //                {
                    //                    strYear = mediEntity.YEAR;
                    //                }
                    //                if (!string.IsNullOrEmpty(txtCaseNotes.Text))
                    //                {
                    //                    InsertDelCaseNotes(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO + propTask + strMediSeq, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO, string.Empty);
                    //                }

                    //                //gvwSelectTask.SelectedRows[0].Cells["gvtAddressdt"].Tag = mediEntity;
                    //                //gvwSelectTask.SelectedRows[0].Cells["gvtAddressdt"].Value = dtAddressed.Value.ToShortDateString();
                    //                //gvwSelectTask.SelectedRows[0].Cells["gvtTaskDesc"].Tag = txtCaseNotes.Text;
                    //                //ClearControls();
                    //                //panel2.Enabled = true;
                    //                //panel4.Enabled = true;

                    //            }
                    //        }
                    //        if (boolSaveMode)
                    //        {
                    //            btnOk.Visible = false;
                    //            cmbTask_SelectedIndexChanged(sender, e);
                    //            ClearControls();
                    //            panel2.Enabled = true;
                    //            panel4.Enabled = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        CommonFunctions.MessageBoxDisplay("Please at least one children select");
                    //    }
                    //}
                    // else
                    //{

                    foreach (DataGridViewRow item in gvwChildDetails.Rows)
                    {
                        if (item.Selected)
                        {
                            strGridIndex = item.Index;
                            propSaveForm = true;
                            ChldMediEntity mediEntity = new ChldMediEntity();
                            mediEntity.AGENCY = BaseForm.BaseAgency;
                            mediEntity.DEPT = BaseForm.BaseDept;
                            mediEntity.PROG = BaseForm.BaseProg;
                            //if(Mode.Equals("Edit"))
                            if (item.Cells["gvtMode"].Value == "Edit")
                                mediEntity.YEAR = propChldmedi.YEAR;
                            else
                                mediEntity.YEAR = BaseForm.BaseYear;
                            mediEntity.APP_NO = gvwChildDetails.SelectedRows[0].Cells["gvApplication"].Value.ToString();
                            mediEntity.TASK = propTask;
                            if (dtAddressed.Checked == true)
                                mediEntity.ADDRESSED_DATE = dtAddressed.Value.ToString();
                            if (dtCompleted.Checked == true)
                                mediEntity.COMPLETED_DATE = dtCompleted.Value.ToString();
                            if (dtDiagnosed.Checked == true)
                                mediEntity.DIAGNOSIS_DATE = dtDiagnosed.Value.ToString();
                            if (dtFollowup.Checked == true)
                                mediEntity.FOLLOWUP_DATE = dtFollowup.Value.ToString();
                            if (dtFupCompleted.Checked == true)
                                mediEntity.FOLLOWUPC_DATE = dtFupCompleted.Value.ToString();
                            if (dtSBCB.Checked == true)
                                mediEntity.SBCB_DATE = dtSBCB.Value.ToString();
                            if (dtSSR.Checked == true)
                                mediEntity.SPECIAL_DATE = dtSSR.Value.ToString();
                            mediEntity.SPECIAL_WHERE = txtWheree.Text;
                            mediEntity.LSTC_OPERATOR = BaseForm.UserID;
                            mediEntity.ADD_OPERATOR = BaseForm.UserID;
                            mediEntity.COMPONENT = propComponents;
                            //**if (((ListItem)cmbFund.SelectedItem).Value.ToString() != "0")
                            if (cmbFund.Items.Count > 0)
                                mediEntity.MediFund = ((ListItem)cmbFund.SelectedItem).Value.ToString();

                            mediEntity.SEQ = propSeq;
                            mediEntity.SN = string.Empty;


                            foreach (DataGridViewRow dataGridViewRow in gvwCustomQuestions.Rows)
                            {
                                if (true)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                                {
                                    string inputValue = string.Empty;
                                    string strResponse = string.Empty;
                                    inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                                    strResponse = dataGridViewRow.Cells["Response"].Tag != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                                    if (dataGridViewRow.Cells[1].Tag == null && (dataGridViewRow.Cells[1].Tag != null && !((string)dataGridViewRow.Cells[1].Tag).Equals("U")))
                                    {
                                        continue;
                                    }


                                    CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;

                                    if (questionEntity.CUSTRESPTYPE.Equals("D") || questionEntity.CUSTRESPTYPE.Equals("C"))
                                    {
                                        if (dataGridViewRow.Cells["Response"].Tag == null || strResponse == string.Empty) continue;
                                        mediEntity.ANSWER1 = dataGridViewRow.Cells["Response"].Tag.ToString();
                                    }
                                    else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                                    {
                                        if (inputValue == string.Empty) continue;
                                        mediEntity.ANSWER2 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                    }
                                    else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                                    {
                                        if (inputValue == string.Empty) continue;
                                        mediEntity.ANSWER3 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                    }
                                    else
                                    {
                                        if (inputValue == string.Empty) continue;
                                        mediEntity.ANSWER1 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                                        if (inputValue != string.Empty)
                                        {
                                            mediEntity.ANSWER2 = CustomQuesValidation(mediEntity.ANSWER1);
                                        }
                                    }

                                }
                            }

                            mediEntity.Mode = item.Cells["gvtMode"].Value.ToString();
                            if (_model.ChldTrckData.InsertUpdateDelChldmedi(mediEntity, out strMediSeq))
                            {
                                string strYear = "    ";
                                if (!string.IsNullOrEmpty(mediEntity.YEAR.Trim()))
                                {
                                    strYear = mediEntity.YEAR;
                                }
                                if (!string.IsNullOrEmpty(txtCaseNotes.Text))
                                {
                                    InsertDelCaseNotes(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO + propTask + strMediSeq, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO, string.Empty);
                                }
                                else
                                {
                                    InsertDelCaseNotes(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO + propTask + strMediSeq, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + mediEntity.APP_NO, "Del");
                                }

                                SaveTaskDocUpload(mediEntity, Mode, strMediSeq);
                                //  saveCustQuestions(strMediSeq, mediEntity.APP_NO, strYear);

                                //if (BaseForm.UserProfile.LoadData.Equals("1") && (!string.IsNullOrEmpty(BaseForm.BaseYear)))
                                //{
                                propChldmediList = _model.ChldTrckData.GetChldMediDetails1(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, propTask, string.Empty);
                                //}
                                //else
                                //{
                                //    propChldmediList = _model.ChldTrckData.GetChldMediDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, propTask, string.Empty);
                                //}

                                cmbClass_SelectedIndexChanged(sender, e);
                                gvwChildDetails.Rows[strGridIndex].Selected = true;
                                gvwChildDetails_SelectionChanged(sender, e);

                                _CompFormSaveFlag = "Y";

                                //HSS00134Control hSS00134Control = BaseForm.GetBaseUserControl() as HSS00134Control;
                                //if (hSS00134Control != null)
                                //{
                                //    hSS00134Control.RefreshGrid();
                                //    this.Close();
                                //}
                            }
                            // }
                            break;
                        }
                    }
                    if (_CompFormSaveFlag == "Y")
                    {
                        AlertBox.Show("Saved Successfully");
                        //this.Close();
                    }
                }
            }

        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (dtAddressed.Checked == false)
            {
                _errorProvider.SetError(dtAddressed, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAddressed.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtAddressed, null);
            }

            if (lblSBCBReq.Visible && dtSBCB.Checked == false)
            {
                _errorProvider.SetError(dtSBCB, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSBCB.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtSBCB, null);
            }

            if (lblComplReq.Visible && dtCompleted.Checked == false)
            {
                _errorProvider.SetError(dtCompleted, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblComple.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtCompleted, null);
            }

            if (lblFollowupReq.Visible && dtFollowup.Checked == false)
            {
                _errorProvider.SetError(dtFollowup, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFollowup.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtFollowup, null);
            }

            if (lblFollUPCReq.Visible && dtFupCompleted.Checked == false)
            {
                _errorProvider.SetError(dtFupCompleted, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFollowupComple.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtFupCompleted, null);
            }

            if (lblDiagnoReq.Visible && dtDiagnosed.Checked == false)
            {
                _errorProvider.SetError(dtDiagnosed, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDiagnosed.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtDiagnosed, null);
            }

            if (lblSSRReq.Visible && dtSSR.Checked == false)
            {
                _errorProvider.SetError(dtSSR, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSSR.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtSSR, null);
            }

            if (lblwhereReq.Visible && String.IsNullOrEmpty(txtWheree.Text.Trim()))
            {
                _errorProvider.SetError(txtWheree, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblWhere.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtWheree, null);
            }
            //if (lblFundReq.Visible && ((ListItem)cmbFund.SelectedItem).Value.ToString() == "0")
            //{
            //    _errorProvider.SetError(cmbFund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFund.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{

            //    _errorProvider.SetError(cmbFund, null);
            //}

            if (lblCaseNotReq.Visible && string.IsNullOrEmpty(txtCaseNotes.Text.Trim()))
            {
                _errorProvider.SetError(txtCaseNotes, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseNotes.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtCaseNotes, null);
            }

            if (lblQuesReq.Visible)
            {
                _errorProvider.SetError(gvwCustomQuestions, null);
                foreach (DataGridViewRow item in gvwCustomQuestions.Rows)
                {
                    if (item.Cells["Response"].Value == string.Empty)
                    {
                        isValid = false;
                        _errorProvider.SetError(gvwCustomQuestions, "Response Required.");
                    }
                }

                //isValid = false;
            }
            else
            {

                _errorProvider.SetError(gvwCustomQuestions, null);
            }


            return (isValid);
        }

        private string CustomQuesValidation(string strMediAnswer1)
        {
            string strAnswer2 = string.Empty;

            ChldTrckEntity chldheightdata = propchldTrckList.Find(u => u.TASK == propTask);
            if (chldheightdata.GCHARTCODE == "HT" || chldheightdata.GCHARTCODE == "WT" || chldheightdata.GCHARTCODE == "HC")
            {

                string strMediAnswer2 = "0";
                if (IsNumeric(strMediAnswer1))
                {
                    strMediAnswer2 = strMediAnswer1;
                    //string[] str = strMediAnswer2.Split('.');
                    //if (str.Length > 0)
                    //{
                    //    if (str[1].Length > 3)
                    //    {
                    //        strMediAnswer2 = str[0] + "." + str[1].Substring(0, 3);
                    //    }
                    //}
                }
                else
                {
                    string strvalue = strMediAnswer1.Substring(0, 2);
                    if (strMediAnswer1.Length > 3)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 3)) && strMediAnswer1.Substring(3, 1) == ".")
                        {
                            if (strMediAnswer1.Length > 6)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(4, 3)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 7);
                                    goto kk;
                                }
                            }
                            if (strMediAnswer1.Length > 5)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(4, 2)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 6);
                                    goto kk;
                                }
                            }
                            if (strMediAnswer1.Length > 4)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(4, 1)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 5);
                                    goto kk;
                                }
                            }
                        }
                    }
                    if (strMediAnswer1.Length > 2)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 2)) && strMediAnswer1.Substring(2, 1) == ".")
                        {
                            if (strMediAnswer1.Length > 5)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(3, 3)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 6);
                                    goto kk;
                                }
                            }
                            if (strMediAnswer1.Length > 4)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(3, 2)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 5);
                                    goto kk;
                                }
                            }
                            if (strMediAnswer1.Length > 3)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(3, 1)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 4);
                                    goto kk;
                                }
                            }
                        }
                    }
                    if (strMediAnswer1.Length > 1)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 1)) && strMediAnswer1.Substring(1, 1) == ".")
                        {
                            if (strMediAnswer1.Length > 4)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(2, 3)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 5);
                                    goto kk;
                                }
                            }
                            if (strMediAnswer1.Length > 3)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(2, 2)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 4);
                                    goto kk;
                                }
                            }
                            if (strMediAnswer1.Length > 2)
                            {
                                if (IsNumeric(strMediAnswer1.Substring(2, 1)))
                                {
                                    strMediAnswer2 = strMediAnswer1.Substring(0, 3);
                                    goto kk;
                                }
                            }
                        }
                    }

                    ///check Fractions
                    ///

                    if (strMediAnswer1.Length > 4)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 3)) && IsNumeric(strMediAnswer1.Substring(4, 1)))
                        {
                            if (strMediAnswer1.Length > 6)
                            {
                                switch (strMediAnswer1.Substring(4, 3))
                                {
                                    case "1/4":
                                    case "2/8":
                                    case @"1\4":
                                    case @"2\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".250";
                                        break;
                                    case "1/2":
                                    case "4/8":
                                    case @"1\2":
                                    case @"4\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".500";
                                        break;
                                    case "3/4":
                                    case "6/8":
                                    case @"3\4":
                                    case @"6\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".750";
                                        break;
                                    case "1/8":
                                    case @"1\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".125";
                                        break;
                                    case "3/8":
                                    case @"3\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".375";
                                        break;
                                    case "5/8":
                                    case @"5\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".625";
                                        break;
                                    case "7/8":
                                    case @"7\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".875";
                                        break;
                                    case "1/3":
                                    case @"1\3":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".333";
                                        break;
                                    case "2/3":
                                    case @"2\3":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 3) + ".666";
                                        break;

                                }
                                goto kk;
                            }
                        }
                    }
                    if (strMediAnswer1.Length > 3)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 2)) && IsNumeric(strMediAnswer1.Substring(3, 1)))
                        {
                            if (strMediAnswer1.Length > 5)
                            {
                                switch (strMediAnswer1.Substring(3, 3))
                                {
                                    case "1/4":
                                    case "2/8":
                                    case @"1\4":
                                    case @"2\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".250";
                                        break;
                                    case "1/2":
                                    case "4/8":
                                    case @"1\2":
                                    case @"4\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".500";
                                        break;
                                    case "3/4":
                                    case "6/8":
                                    case @"3\4":
                                    case @"6\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".750";
                                        break;
                                    case "1/8":
                                    case @"1\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".125";
                                        break;
                                    case "3/8":
                                    case @"3\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".375";
                                        break;
                                    case "5/8":
                                    case @"5\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".625";
                                        break;
                                    case "7/8":
                                    case @"7\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".875";
                                        break;
                                    case "1/3":
                                    case @"1\3":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".333";
                                        break;
                                    case "2/3":
                                    case @"2\3":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 2) + ".666";
                                        break;

                                }
                                goto kk;
                            }
                        }
                    }

                    if (strMediAnswer1.Length > 2)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 1)) && IsNumeric(strMediAnswer1.Substring(2, 1)))
                        {
                            if (strMediAnswer1.Length > 4)
                            {
                                switch (strMediAnswer1.Substring(2, 3))
                                {
                                    case "1/4":
                                    case "2/8":
                                    case @"1\4":
                                    case @"2\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".250";
                                        break;
                                    case "1/2":
                                    case "4/8":
                                    case @"1\2":
                                    case @"4\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".500";
                                        break;
                                    case "3/4":
                                    case "6/8":
                                    case @"3\4":
                                    case @"6\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".750";
                                        break;
                                    case "1/8":
                                    case @"1\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".125";
                                        break;
                                    case "3/8":
                                    case @"3\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".375";
                                        break;
                                    case "5/8":
                                    case @"5\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".625";
                                        break;
                                    case "7/8":
                                    case @"7\8":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".875";
                                        break;
                                    case "1/3":
                                    case @"1\3":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".333";
                                        break;
                                    case "2/3":
                                    case @"2\3":
                                        strMediAnswer2 = strMediAnswer1.Substring(0, 1) + ".666";
                                        break;

                                }
                                goto kk;
                            }
                        }
                    }


                    if (strMediAnswer1.Length > 2)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 3)))
                        {
                            strMediAnswer2 = strMediAnswer1.Substring(0, 3);
                            goto kk;
                        }
                    }
                    if (strMediAnswer1.Length > 1)
                    {
                        if (IsNumeric(strMediAnswer1.Substring(0, 2)))
                        {
                            strMediAnswer2 = strMediAnswer1.Substring(0, 2);
                            goto kk;
                        }
                    }

                }
            kk:
                strAnswer2 = strMediAnswer2;

            }
            return strAnswer2;
        }

        public bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //if (propRegularForm != "Regular")
            //{
            //    HSS00134Control hSS00134Control = BaseForm.GetBaseUserControl() as HSS00134Control;
            //    if (hSS00134Control != null)
            //    {

            //        if (BaseForm.UserProfile.FastLoad.Equals("Y"))
            //        {
            //            if (Mode.Equals("Add"))
            //            {
            //                hSS00134Control.RefreshFastLoadGrid();
            //                this.Close();
            //            }
            //        }
            //        else
            //        {
            //            hSS00134Control.RefreshGrid();
            //            this.Close();
            //        }
            //    }
            //    this.Close();
            //}
            //else
            this.Close();
        }

        private void btnAgency_Click(object sender, EventArgs e)
        {
            //List<CASEREFSEntity> sel_REFS_entity = new List<CASEREFSEntity>();
            List<ACTREFSEntity> sel_REFS_entity = new List<ACTREFSEntity>();
            AgencyReferral_SubForm Ref_Form = new AgencyReferral_SubForm("Browse", string.Empty);
            Ref_Form.FormClosed += new FormClosedEventHandler(On_Referral_Select_Closed);
            Ref_Form.StartPosition = FormStartPosition.CenterScreen;
            Ref_Form.ShowDialog();
        }


        private void On_Referral_Select_Closed(object sender, FormClosedEventArgs e)
        {
            string[] SelRef_Name = new string[2];

            AgencyReferral_SubForm form = sender as AgencyReferral_SubForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelRef_Name = form.GetSelected_Referral();

                txtAgency.Text = SelRef_Name[0];
                txtWheree.Text = SelRef_Name[1];

            }
        }

        List<CASEREFEntity> CaseRefList;
        private void FillAgencyReferalDetails()
        {

            CASEREFEntity Search_Entity = new CASEREFEntity(true);
            CaseRefList = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");

        }

        void fillCombofund()
        {
            List<CommonEntity> lookUpfundingResource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            foreach (CommonEntity agyEntity in lookUpfundingResource)
            {
                cmbFund.Items.Add(new Captain.Common.Utilities.ListItem(agyEntity.Desc, agyEntity.Code));
                //if (agyEntity.Default.Equals("Y"))
                //    strFundDefaultCode = agyEntity.Code;
            }
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SHI")
            {
                cmbFund.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "0"));
            }
            else
            {
                cmbFund.Items.Insert(0, new Captain.Common.Utilities.ListItem("None", "0"));
            }
            cmbFund.SelectedIndex = 0;
        }

        List<CaseEnrlEntity> Fund_List_Filtered = new List<CaseEnrlEntity>();
        void fillCombofund_New()
        {
            cmbFund.Items.Clear();

            List<CommonEntity> lookUpfundingResource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");

            cmbFund.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "0"));

            foreach (CommonEntity entity in lookUpfundingResource)
            {
                cmbFund.Items.Add(new Captain.Common.Utilities.ListItem(entity.Desc, entity.Code));
            }
            //var Fund_List = propCaseENrl.Select(x => x.FundHie).Distinct().ToList();

            //foreach (var entity in Fund_List)
            //{
            //    string fundDesc = lookUpfundingResource.Find(x => x.Code == entity).Desc;

            //    cmbFund.Items.Add(new Captain.Common.Utilities.ListItem(fundDesc, entity));

            //}
            //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SHI")
            //{
            //    cmbFund.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "0"));
            //}
            //else
            //{
            //    cmbFund.Items.Insert(0, new Captain.Common.Utilities.ListItem("None", "0"));
            //}
            if (cmbFund.Items.Count > 0)
                cmbFund.SelectedIndex = 0;
        }

        private void txtWheree_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWheree.Text))
            {
                string AgencyCode = txtWheree.Text;
                txtWheree.Text = "00000".Substring(0, 5 - AgencyCode.Length) + AgencyCode;
                CASEREFEntity caseRefName = CaseRefList.Find(u => u.Code.Equals(txtWheree.Text));
                if (caseRefName != null)
                {
                    txtAgency.Text = caseRefName.Name1;
                }
                else
                {
                    txtAgency.Text = string.Empty;
                    txtWheree.Text = string.Empty;
                    AlertBox.Show("Invalid Referral Code", MessageBoxIcon.Warning);
                }
            }
        }

        private void pbCaseNotes_Click(object sender, EventArgs e)
        {
            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo + propTask + propSeq);
            CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo + propTask.Trim() + propSeq.Trim());
            caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
            caseNotes.StartPosition = FormStartPosition.CenterScreen;
            caseNotes.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCaseNotesFormClosed(object sender, FormClosedEventArgs e)
        {
            CaseNotes form = sender as CaseNotes;

            //if (form.DialogResult == DialogResult.OK)
            //{
            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo + propTask + propSeq);
            if (caseNotesEntity.Count > 0)
            {
                this.Tools["tlCaseNotes"].ImageSource = Consts.Icons.ico_CaseNotes_View;
            }
            else
            {
                this.Tools["tlCaseNotes"].ImageSource = Consts.Icons.ico_CaseNotes_New;
            }
            caseNotesEntity = caseNotesEntity;

            //}
        }

        private void ShowCaseNotesImages(string strseq, string strApplication, string strYear1)
        {
            string strYear = "    ";
            if (!string.IsNullOrEmpty(strYear1))
            {
                strYear = strYear1;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + strApplication + propTask + strseq);
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

        private void HSS00134Form_Load(object sender, EventArgs e)
        {

            if (boolCheckFund == false)
                AlertBox.Show("No Fund is set as default in AGYTAB 03320", MessageBoxIcon.Warning);

            dtSBCB.Focus();
            if (Mode.Equals("Edit"))
            {
                strcheckMode = "Edit";
                // gvwChildDetails_SelectionChanged(sender, e);
            }
        }

        private void fillCustomQuestions(string custQues, string strApplication, string strYear, ChldMediEntity chldmediQueslist)
        {

            //List<ChldMedRespEntity> chldMedResponses = _model.ChldTrckData.GetChldMedRespDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear, strApplication, propTask, propSeq,  string.Empty);
            bool isResponse = false;

            //List<string> QuesList = new List<string>();
            //if (custQues != null)
            //{
            //    string[] relationTypes = custQues.Split(' ');
            //    for (int i = 0; i < relationTypes.Length; i++)
            //    {
            //        QuesList.Add(relationTypes.GetValue(i).ToString());
            //    }
            //}


            gvwCustomQuestions.Rows.Clear();
            if (propcustQuestions.Count > 0)
            {
                gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);

                _customQuestionsandAnswers = new List<CustomQuestionsandAnswers>();
                if (propcustQuestions.Count > 0)
                {
                    CustomQuestionsEntity dr = propcustQuestions.Find(u => u.CUSTCODE.Trim().ToString() == custQues.Trim().ToString());
                    if (dr != null)
                    {

                        //foreach (CustomQuestionsEntity dr in propcustQuestions)
                        //{
                        //    //if (custQues != null && custQues.Contains(dr.CUSTCODE))
                        //    //{
                        //    if (custQues != null && custQues.Trim().ToString() == dr.CUSTCODE.Trim().ToString())
                        //    {
                        string custCode = dr.CUSTCODE.ToString();

                        //Gizmox.WebGUI.Common.Resources.ResourceHandle saveImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("save.gif");
                        //Gizmox.WebGUI.Common.Resources.ResourceHandle DeleteImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("DeleteItem.gif");
                        //Gizmox.WebGUI.Common.Resources.ResourceHandle BlanckImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");

                        string saveImage = "Resources/Icons/16X16/save.gif";
                        string DeleteImage = Consts.Icons.ico_Delete;   // "Resources/Icons/16X16/DeleteItem.gif";
                        string BlanckImage = Consts.Icons.ico_Blank;    // "Resources/Icons/16X16/Blank.JPG";

                        int rowIndex = gvwCustomQuestions.Rows.Add(BlanckImage, dr.CUSTDESC, string.Empty, string.Empty, string.Empty, string.Empty, BlanckImage);

                        gvwCustomQuestions.Rows[rowIndex].Tag = dr;
                        //if (!Mode.Equals("Add"))
                        //{
                        //List<ChldMedRespEntity> response = chldMedResponses.FindAll(u => u.QUE.Equals(custCode)).ToList();
                        //if (response.Count > 0)
                        //{

                        //    gvwCustomQuestions.Rows[rowIndex].Cells[0].Value = saveImage;
                        //    gvwCustomQuestions.Rows[rowIndex].Cells["ResponceDelete"].Value = DeleteImage;
                        //}

                        string fieldType = dr.CUSTRESPTYPE.ToString();
                        if (fieldType.Equals("D") || fieldType.Equals("C"))
                        {
                            gvwCustomQuestions.Rows[rowIndex].ReadOnly = true;
                        }
                        else
                        {
                            gvwCustomQuestions.Rows[rowIndex].Cells[0].ReadOnly = true;
                            gvwCustomQuestions.Rows[rowIndex].Cells[1].ReadOnly = true;
                        }
                        if (fieldType.Equals("T"))
                        {
                            //this.Response.Mask = "00/00/0000";
                            // gvwCustomQuestions.Rows[rowIndex].Cells[2].Value = "  /  /";
                        }
                        else
                        {
                            //this.Response.Mask = "";
                            //this.Response.TextMaskFormat = Gizmox.WebGUI.Forms.MaskFormat.ExcludePromptAndLiterals;
                        }
                        string custQuestionResp = string.Empty;
                        string custQuestionCode = string.Empty;
                        if (!Mode.Equals(Consts.Common.Add))
                        {

                            isResponse = true;
                            if (fieldType.Equals("D") || fieldType.Equals("C"))
                            {
                                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("HSS00133", custCode);


                                string code = chldmediQueslist.ANSWER1.Trim();
                                CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
                                if (custRespEntity != null)
                                {
                                    custQuestionResp = custRespEntity.RespDesc;
                                    custQuestionCode = chldmediQueslist.ANSWER1.Trim();
                                    gvwCustomQuestions.Rows[rowIndex].Cells[0].Value = saveImage;
                                    gvwCustomQuestions.Rows[rowIndex].Cells["ResponceDelete"].Value = DeleteImage;
                                }

                                gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Tag = custQuestionCode;
                                gvwCustomQuestions.Rows[rowIndex].Cells[2].Value = custQuestionResp;
                                gvwCustomQuestions.Rows[rowIndex].Cells[1].Tag = "U";
                            }
                            else if (fieldType.Equals("N"))
                            {
                                custQuestionResp = chldmediQueslist.ANSWER2.Trim().ToString();
                            }
                            else if (fieldType.Equals("T"))
                            {
                                custQuestionResp = LookupDataAccess.Getdate(chldmediQueslist.ANSWER3.Trim().ToString());

                            }
                            else
                            {
                                custQuestionResp = chldmediQueslist.ANSWER1.Trim().ToString();

                            }

                            if (custQuestionResp != string.Empty)
                            {
                                gvwCustomQuestions.Rows[rowIndex].Cells[2].Value = custQuestionResp;
                                gvwCustomQuestions.Rows[rowIndex].Cells[1].Tag = "U";
                                // gvwCustomQuestions.Rows[rowIndex].Cells["FamilySeq"].Value = response[0].ACTSNPFAMILYSEQ;
                                // gvwCustomQuestions.Rows[rowIndex].Cells["ResponceSeq"].Value = response[0].ACTRESPSEQ;
                                gvwCustomQuestions.Rows[rowIndex].Cells["Code"].Value = custCode;
                                gvwCustomQuestions.Rows[rowIndex].Cells[0].Value = saveImage;
                                gvwCustomQuestions.Rows[rowIndex].Cells["ResponceDelete"].Value = DeleteImage;
                            }
                        }

                        //}


                    }
                }
                gvwCustomQuestions.Update();
                if (gvwCustomQuestions.Rows.Count > 0)
                    gvwCustomQuestions.Rows[0].Selected = true;
                gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
            }
        }


        //private void fillCustomQuestions(string custQues, string strApplication, string strYear)
        //{

        //    //List<ChldMedRespEntity> chldMedResponses = _model.ChldTrckData.GetChldMedRespDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear, strApplication, propTask, propSeq,  string.Empty);
        //    bool isResponse = false;

        //    //List<string> QuesList = new List<string>();
        //    //if (custQues != null)
        //    //{
        //    //    string[] relationTypes = custQues.Split(' ');
        //    //    for (int i = 0; i < relationTypes.Length; i++)
        //    //    {
        //    //        QuesList.Add(relationTypes.GetValue(i).ToString());
        //    //    }
        //    //}


        //    gvwCustomQuestions.Rows.Clear();
        //    if (propcustQuestions.Count > 0)
        //    {
        //        gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);

        //        _customQuestionsandAnswers = new List<CustomQuestionsandAnswers>();
        //        foreach (CustomQuestionsEntity dr in propcustQuestions)
        //        {
        //            //if (custQues != null && custQues.Contains(dr.CUSTCODE))
        //            //{
        //            if (custQues != null && custQues.Trim().ToString() == dr.CUSTCODE.Trim().ToString())
        //            {
        //                string custCode = dr.CUSTCODE.ToString();

        //                Gizmox.WebGUI.Common.Resources.ResourceHandle saveImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("save.gif");
        //                Gizmox.WebGUI.Common.Resources.ResourceHandle DeleteImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("DeleteItem.gif");
        //                Gizmox.WebGUI.Common.Resources.ResourceHandle BlanckImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");

        //                int rowIndex = gvwCustomQuestions.Rows.Add(BlanckImage, dr.CUSTDESC, string.Empty, string.Empty, string.Empty, string.Empty, BlanckImage);

        //                gvwCustomQuestions.Rows[rowIndex].Tag = dr;
        //                if (!Mode.Equals("Add"))
        //                {
        //                    //List<ChldMedRespEntity> response = chldMedResponses.FindAll(u => u.QUE.Equals(custCode)).ToList();
        //                    //if (response.Count > 0)
        //                    //{

        //                    //    gvwCustomQuestions.Rows[rowIndex].Cells[0].Value = saveImage;
        //                    //    gvwCustomQuestions.Rows[rowIndex].Cells["ResponceDelete"].Value = DeleteImage;
        //                    //}

        //                    string fieldType = dr.CUSTRESPTYPE.ToString();
        //                    if (fieldType.Equals("D") || fieldType.Equals("C"))
        //                    {
        //                        gvwCustomQuestions.Rows[rowIndex].ReadOnly = true;
        //                    }
        //                    else
        //                    {
        //                        gvwCustomQuestions.Rows[rowIndex].Cells[0].ReadOnly = true;
        //                        gvwCustomQuestions.Rows[rowIndex].Cells[1].ReadOnly = true;
        //                    }

        //                    string custQuestionResp = string.Empty;
        //                    string custQuestionCode = string.Empty;
        //                    if (true)   //!Mode.Equals(Consts.Common.Add))
        //                    {
        //                        List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
        //                        if (response != null && response.Count > 0)
        //                        {
        //                            isResponse = true;
        //                            if (fieldType.Equals("D") || fieldType.Equals("C"))
        //                            {
        //                                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("HSS00133", response[0].QUE);

        //                                foreach (ChldMedRespEntity custResp in response)
        //                                {
        //                                    string code = custResp.ALPHA_RESP.Trim();
        //                                    CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
        //                                    if (custRespEntity != null)
        //                                    {
        //                                        custQuestionResp += custRespEntity.RespDesc;
        //                                        custQuestionCode += custResp.ALPHA_RESP.ToString() + " ";
        //                                    }
        //                                }
        //                                gvwCustomQuestions.Rows[rowIndex].Cells[2].Tag = custQuestionCode;
        //                                gvwCustomQuestions.Rows[rowIndex].Cells[2].Value = custQuestionResp;
        //                                gvwCustomQuestions.Rows[rowIndex].Cells[1].Tag = "U";
        //                            }
        //                            else if (fieldType.Equals("N"))
        //                            {
        //                                custQuestionResp = response[0].NUM_RESP.ToString();
        //                            }
        //                            else if (fieldType.Equals("T"))
        //                            {
        //                                custQuestionResp = LookupDataAccess.Getdate(response[0].DATE_RESP.ToString());
        //                            }
        //                            else
        //                            {
        //                                custQuestionResp = response[0].ALPHA_RESP.ToString();
        //                            }
        //                            gvwCustomQuestions.Rows[rowIndex].Cells[2].Value = custQuestionResp;
        //                            gvwCustomQuestions.Rows[rowIndex].Cells[1].Tag = "U";
        //                            // gvwCustomQuestions.Rows[rowIndex].Cells["FamilySeq"].Value = response[0].ACTSNPFAMILYSEQ;
        //                            // gvwCustomQuestions.Rows[rowIndex].Cells["ResponceSeq"].Value = response[0].ACTRESPSEQ;
        //                            gvwCustomQuestions.Rows[rowIndex].Cells["Code"].Value = response[0].QUE;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        gvwCustomQuestions.Update();
        //        gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
        //    }
        //}


        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu2.MenuItems.Clear();
            if (gvwCustomQuestions.Rows.Count > 0)
            {
                if (gvwCustomQuestions.Rows[0].Tag is CustomQuestionsEntity)
                {
                    CustomQuestionsEntity drow = gvwCustomQuestions.SelectedRows[0].Tag as CustomQuestionsEntity;
                    string fieldCode = drow.CUSTCODE.ToString();
                    string fieldType = drow.CUSTRESPTYPE.ToString();

                    if (fieldType.Equals("D") || fieldType.Equals("C"))
                    {
                        List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("HSS00133", fieldCode);
                        if (custReponseEntity.Count > 0)
                        {
                            string response = gvwCustomQuestions.SelectedRows[0].Cells[2].Value != null ? gvwCustomQuestions.SelectedRows[0].Cells[2].Value.ToString() : string.Empty;
                            string[] arrResponse = null;
                            if (response.IndexOf(',') > 0)
                            {
                                arrResponse = response.Split(',');
                            }
                            else if (!response.Equals(string.Empty))
                            {
                                arrResponse = new string[] { response };
                            }

                            foreach (CustRespEntity dr in custReponseEntity)
                            {
                                string resDesc = dr.RespDesc.ToString().Trim();

                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = resDesc;
                                menuItem.Tag = dr;
                                if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                                {
                                    menuItem.Checked = true;
                                }
                                contextMenu2.MenuItems.Add(menuItem);
                            }
                        }
                    }
                    else if (fieldType.Equals("X"))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Please enter text here";
                        menuItem.Tag = "X"; // menuItem.Tag = "A";
                        contextMenu2.MenuItems.Add(menuItem);
                        gvwCustomQuestions.Rows[0].Cells[2].ReadOnly = false;
                    }
                    else if (fieldType.Equals("T"))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Please enter Date here";
                        menuItem.Tag = "T";//menuItem.Tag = "X";
                        contextMenu2.MenuItems.Add(menuItem);
                        gvwCustomQuestions.Rows[0].Cells[2].ReadOnly = false;
                    }
                    else if (fieldType.Equals("N"))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Please enter number here";
                        menuItem.Tag = "N";
                        contextMenu2.MenuItems.Add(menuItem);
                        gvwCustomQuestions.Rows[0].Cells[2].ReadOnly = false;
                        this.Response.MaxInputLength = 5;
                    }
                }
            }
        }

        private void gvwCustomQuestions_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is CustRespEntity)
            {
                string responseValue = gvwCustomQuestions.SelectedRows[0].Cells[2].Value.ToString();
                string responseCode = gvwCustomQuestions.SelectedRows[0].Cells[2].Tag == null ? string.Empty : gvwCustomQuestions.SelectedRows[0].Cells[2].Tag.ToString();
                CustRespEntity dr = (CustRespEntity)objArgs.MenuItem.Tag as CustRespEntity;

                string selectedValue = objArgs.MenuItem.Text;
                string selectedCode = dr.DescCode.ToString();
                if (objArgs.MenuItem.Checked)
                {
                    //responseValue = responseValue.Replace(selectedValue + Consts.Common.Comma, string.Empty);
                    //responseValue = responseValue.Replace(selectedValue, string.Empty);
                    //responseCode = responseCode.Replace(selectedCode + Consts.Common.Comma, string.Empty);
                    //responseCode = responseCode.Replace(selectedCode, string.Empty);
                    //gvwCustomQuestions.SelectedRows[0].Cells[2].Value = responseValue;
                    //gvwCustomQuestions.SelectedRows[0].Cells[2].Tag = responseCode;
                    responseValue = selectedValue;
                    responseCode = selectedCode;
                }
                else
                {
                    //if (!responseValue.Equals(string.Empty)) responseValue += ",";
                    //if (!responseCode.Equals(string.Empty)) responseCode += ",";
                    responseValue = selectedValue;
                    responseCode = selectedCode;
                    //gvwCustomQuestions.SelectedRows[0].Cells[2].Value = responseValue;
                    //gvwCustomQuestions.SelectedRows[0].Cells[2].Tag = responseCode;
                }
                string custCode = ((CustomQuestionsEntity)gvwCustomQuestions.SelectedRows[0].Tag).CUSTCODE;
                _customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseValue = responseValue);
                _customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseCode = responseCode);
                gvwCustomQuestions.SelectedRows[0].Cells[2].Tag = responseCode;
                gvwCustomQuestions.SelectedRows[0].Cells[2].Value = responseValue;
            }
            else
            {
                gvwCustomQuestions.Rows[0].Cells[2].ReadOnly = false;
                gvwCustomQuestions.Rows[0].Cells[2].Selected = true;
            }
        }

        private void gvwCustomQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int intcolindex = gvwCustomQuestions.CurrentCell.ColumnIndex;
            int introwindex = gvwCustomQuestions.CurrentCell.RowIndex;
            string strCurrectValue = Convert.ToString(gvwCustomQuestions.Rows[introwindex].Cells[intcolindex].Value);
            CustomQuestionsEntity questionEntity = gvwCustomQuestions.Rows[e.RowIndex].Tag as CustomQuestionsEntity;

            if (gvwCustomQuestions.Columns[e.ColumnIndex].Name.Equals("Response") && questionEntity != null && questionEntity.CUSTRESPTYPE.Equals("N"))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                {
                    gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                    gvwCustomQuestions.Rows[introwindex].Cells["Response"].Value = string.Empty;
                    gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                    AlertBox.Show(Consts.Messages.PleaseEnterNumbers, MessageBoxIcon.Warning);
                }
            }
            else if (gvwCustomQuestions.Columns[e.ColumnIndex].Name.Equals("Response") && questionEntity != null && questionEntity.CUSTRESPTYPE.Equals("T"))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                {
                    gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                    gvwCustomQuestions.Rows[introwindex].Cells["Response"].Value = string.Empty;
                    gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                    AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                }
            }
        }

        private void gvwCustomQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex != -1)
            {
                if (SelectedRow())
                {
                    if (gvwCustomQuestions.SelectedRows[0].Cells["Code"].Value != string.Empty)
                        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerQuestions);
                }
            }
        }
        //Gizmox.WebGUI.Common.Resources.ResourceHandle BlanckImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        string BlanckImage = "Resources/Icons/16X16/Blank.JPG";
        private void MessageBoxHandlerQuestions(DialogResult dialogresult)
        {
            // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
            //Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (DialogResult.Yes == dialogresult)
            {
                //ChldMedRespEntity chldMedrespEntity = new ChldMedRespEntity();

                //chldMedrespEntity.AGENCY = BaseForm.BaseAgency;
                //chldMedrespEntity.DEPT = BaseForm.BaseDept;
                //chldMedrespEntity.PROG = BaseForm.BaseProg;
                //chldMedrespEntity.YEAR = propChldmedi.YEAR;
                //chldMedrespEntity.APP_NO = propChldmedi.APP_NO;
                ////chldMedrespEntity.TASK = propTask;                    
                ////chldMedrespEntity.SEQ = propSeq;
                //chldMedrespEntity.Mode = "Delete";
                //chldMedrespEntity.QUE = gvwCustomQuestions.SelectedRows[0].Cells["Code"].Value.ToString();
                //if (_model.ChldTrckData.InsertUpdateDelChldmedResp(chldMedrespEntity))
                //{
                gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                gvwCustomQuestions.SelectedRows[0].Cells[0].Value = BlanckImage;
                gvwCustomQuestions.SelectedRows[0].Cells["ResponceDelete"].Value = BlanckImage;
                gvwCustomQuestions.SelectedRows[0].Cells[2].Value = string.Empty;
                gvwCustomQuestions.SelectedRows[0].Cells["Code"].Value = string.Empty;
                gvwCustomQuestions.SelectedRows[0].Cells[1].Tag = string.Empty;
                gvwCustomQuestions.SelectedRows[0].Cells["Response"].Tag = string.Empty;
                gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);

                //}

            }
            //}
        }

        private bool SelectedRow()
        {

            bool boolrowselet = false;
            if (gvwCustomQuestions.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwCustomQuestions.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        boolrowselet = true;
                        break;
                    }
                }
            }
            return boolrowselet;
        }

        private void fillCustQuestions(string custQues)
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
            //List<CustomQuestionsEntity> custQuestionsList = custQuestions;
            //gvwQuestion.Rows.Clear();
            //foreach (CustomQuestionsEntity entityQuestion in custQuestionsList)
            //{
            //    string flag = "false";
            //    if (custQues != null && QuesList.Contains(entityQuestion.CUSTCODE))
            //    {
            //        flag = "true";
            //    }
            //    int rowIndex = gvwQuestion.Rows.Add(flag, entityQuestion.CUSTDESC.Trim(), entityQuestion.CUSTCODE);
            //    gvwQuestion.Rows[rowIndex].Tag = entityQuestion;
            //    CommonFunctions.setTooltip(rowIndex, entityQuestion.addoperator, entityQuestion.adddate, entityQuestion.lstcoperator, entityQuestion.lstcdate, gvwQuestion);

            //}

        }

        private void saveCustQuestions(string strSeq, string strApplicationNo, string strYear)
        {
            if (Mode.Equals("Edit"))
            {
                ChldMedRespEntity chldMedrespEntity = new ChldMedRespEntity();

                chldMedrespEntity.AGENCY = BaseForm.BaseAgency;
                chldMedrespEntity.DEPT = BaseForm.BaseDept;
                chldMedrespEntity.PROG = BaseForm.BaseProg;
                chldMedrespEntity.YEAR = strYear;
                chldMedrespEntity.APP_NO = strApplicationNo;
                //chldMedrespEntity.TASK = propTask;                
                //chldMedrespEntity.SEQ = strSeq;
                chldMedrespEntity.Mode = "DeleteAll";
                _model.ChldTrckData.InsertUpdateDelChldmedResp(chldMedrespEntity);

            }

            foreach (DataGridViewRow dataGridViewRow in gvwCustomQuestions.Rows)
            {
                if (true)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                {
                    string inputValue = string.Empty;
                    string strResponse = string.Empty;
                    inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                    strResponse = dataGridViewRow.Cells["Response"].Tag != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                    if (dataGridViewRow.Cells[1].Tag == null && (dataGridViewRow.Cells[1].Tag != null && !((string)dataGridViewRow.Cells[1].Tag).Equals("U")))
                    {
                        continue;
                    }
                    ChldMedRespEntity chldMedrespEntity = new ChldMedRespEntity();

                    CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;
                    chldMedrespEntity.AGENCY = BaseForm.BaseAgency;
                    chldMedrespEntity.DEPT = BaseForm.BaseDept;
                    chldMedrespEntity.PROG = BaseForm.BaseProg;

                    chldMedrespEntity.YEAR = strYear;
                    chldMedrespEntity.APP_NO = strApplicationNo;
                    //chldMedrespEntity.TASK = propTask;                    
                    //chldMedrespEntity.SEQ = strSeq;

                    chldMedrespEntity.QUE = questionEntity.CUSTCODE;
                    if (questionEntity.CUSTRESPTYPE.Equals("D") || questionEntity.CUSTRESPTYPE.Equals("C"))
                    {
                        if (dataGridViewRow.Cells["Response"].Tag == null || strResponse == string.Empty) continue;
                        chldMedrespEntity.ALPHA_RESP = dataGridViewRow.Cells["Response"].Tag.ToString();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                    {
                        if (inputValue == string.Empty) continue;
                        chldMedrespEntity.NUM_RESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                    {
                        if (inputValue == string.Empty) continue;
                        chldMedrespEntity.DATE_RESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    else
                    {
                        if (inputValue == string.Empty) continue;
                        chldMedrespEntity.ALPHA_RESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }

                    chldMedrespEntity.Mode = "Add";

                    chldMedrespEntity.ADD_OPERATOR = BaseForm.UserID;
                    chldMedrespEntity.LSTC_OPERATOR = BaseForm.UserID;
                    _model.ChldTrckData.InsertUpdateDelChldmedResp(chldMedrespEntity);
                }
            }
        }

        private void InsertDelCaseNotes(string FiledName, string ApplicationNo, string strMode)
        {

            CaseNotesEntity caseNotesDetails = new CaseNotesEntity();
            caseNotesDetails.ScreenName = Privileges.Program;
            caseNotesDetails.FieldName = FiledName;
            caseNotesDetails.AppliCationNo = ApplicationNo;
            caseNotesDetails.Data = txtCaseNotes.Text.Trim();
            caseNotesDetails.AddOperator = BaseForm.UserID;
            caseNotesDetails.LstcOperation = BaseForm.UserID;
            if (strMode == "Del")
            {
                caseNotesDetails.Mode = "Del";
            }
            if (_model.TmsApcndata.InsertUpdateCaseNotes(caseNotesDetails))
            {
                //strYear = "    ";
                //if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                //{
                //    strYear = BaseForm.BaseYear;
                //}
                //caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privilege.Program, propKeyFiedld);

                //fillCaseNotes();
            }


        }

        private void btnCaseNotes_Click(object sender, EventArgs e)
        {

        }


        private string SaveClientTrackingXml()
        {
            StringBuilder str = new StringBuilder();
            if (Mode != Consts.Common.Delete)
            {
                str.Append("<CaseApplication>");
                List<DataGridViewRow> SelectedgvRows = (from c in gvwChildDetails.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (c.Cells["gvtSelectValue"].Value.ToString().ToUpper().Equals("T"))
                                                        select c).ToList();

                foreach (DataGridViewRow gvrow in SelectedgvRows)
                {
                    str.Append("<Details Medi_Appl_No = \"" + gvrow.Cells["gvApplication"].Value.ToString().Substring(0, 2) + "\" />");
                }
                str.Append("</CaseApplication>");
            }
            else
                str = null;

            return str.ToString();
        }

        private void ClearControls(string strEnrlApp)
        {

            _errorProvider.SetError(dtAddressed, null);
            _errorProvider.SetError(dtSBCB, null);
            _errorProvider.SetError(dtCompleted, null);
            _errorProvider.SetError(dtFollowup, null);
            _errorProvider.SetError(dtFupCompleted, null);
            _errorProvider.SetError(dtDiagnosed, null);
            _errorProvider.SetError(dtSSR, null);
            _errorProvider.SetError(txtWheree, null);
            _errorProvider.SetError(txtCaseNotes, null);
            _errorProvider.SetError(gvwCustomQuestions, null);
            _errorProvider.SetError(cmbFund, null);
            pnlDates.Enabled = false;
            pnlGridNotes.Enabled = false;
            dtAddressed.Value = DateTime.Now.Date;
            dtSBCB.Value = DateTime.Now.Date;
            dtCompleted.Value = DateTime.Now.Date;
            dtDiagnosed.Value = DateTime.Now.Date;
            dtFollowup.Value = DateTime.Now.Date;
            dtFupCompleted.Value = DateTime.Now.Date;
            dtSSR.Value = DateTime.Now.Date;
            dtAddressed.Checked = false;
            dtCompleted.Checked = false;
            dtDiagnosed.Checked = false;
            dtFollowup.Checked = false;
            dtFupCompleted.Checked = false;
            dtSBCB.Checked = false;
            dtSSR.Checked = false;
            txtAgency.Text = string.Empty;
            txtCaseNotes.Text = string.Empty;
            txtWheree.Text = string.Empty;

            lbldocupldName.Text = string.Empty;
            TaskUpldID = "";
            _prvDocFile = "";

            if (strEnrlApp != string.Empty)
            {
                string strDefaultFund = GetFundChilds(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, (BaseForm.BaseYear == string.Empty ? "    " : BaseForm.BaseYear), strEnrlApp);
                CommonFunctions.SetComboBoxValue(cmbFund, strDefaultFund);
            }
            else
            {
                CommonFunctions.SetComboBoxValue(cmbFund, "0");
            }
            gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
            foreach (DataGridViewRow item in gvwCustomQuestions.Rows)
            {

                item.Cells[0].Value = BlanckImage;
                item.Cells["ResponceDelete"].Value = BlanckImage;
                item.Cells[2].Value = string.Empty;
                item.Cells["Code"].Value = string.Empty;
                item.Cells[1].Tag = string.Empty;
                item.Cells["Response"].Tag = string.Empty;

            }
            gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
        }



        private void Hepl_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSS00134");
        }

        private void EnableDisableControls(string strPostTask)
        {
            //string Agency = BaseForm.BaseAgency;
            //string Dept = BaseForm.BaseDept;
            //string Program = BaseForm.BaseProg;
            //if (propComponents == "0000")
            //{
            //    Agency = string.Empty;
            //    Dept = string.Empty;
            //    Program = string.Empty;
            //}
            List<ChldTrckEntity> trackEntitylist = new List<ChldTrckEntity>();
            if (strPostTask == string.Empty)
            {
                trackEntitylist = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", propTask, string.Empty);
            }
            else
            {

                foreach (DataGridViewRow item in gvwSelectTask.Rows)
                {
                    if (item.Selected)
                    {
                        trackEntitylist.Clear();
                        ChldTrckEntity trackEntity = item.Tag as ChldTrckEntity;
                        trackEntitylist.Add(trackEntity);
                        break;
                    }

                }

            }
            if (trackEntitylist.Count > 0)
            {
                propchldTrckList = trackEntitylist;
                ChldTrckEntity trackEntity = trackEntitylist[0];
                if (trackEntity.QUESTIONE == "Y")
                    gvwCustomQuestions.Enabled = true;
                else
                    gvwCustomQuestions.Enabled = false;

                lblQuesReq.Visible = trackEntity.QUESTIONR == "Y" ? true : false;
                txtCaseNotes.ReadOnly = trackEntity.CASENOTESE == "Y" ? false : true;
                lblCaseNotReq.Visible = trackEntity.CASENOTESR == "Y" ? true : false;
                dtSBCB.Enabled = trackEntity.SBCBE == "Y" ? true : false;
                lblSBCBReq.Visible = trackEntity.SBCBR == "Y" ? true : false;
                //= trackEntity.ADDRESSE == "Y" ? true : false;
                //= trackEntity.ADDRESSR == "Y" ? true : false;
                dtCompleted.Enabled = trackEntity.COMPLETEE == "Y" ? true : false;
                lblComplReq.Visible = trackEntity.COMPLETER == "Y" ? true : false;
                dtFollowup.Enabled = trackEntity.FOLLOWUPE == "Y" ? true : false;
                lblFollowupReq.Visible = trackEntity.FOLLOWUPR == "Y" ? true : false;
                dtFupCompleted.Enabled = trackEntity.FOLLOWUPCE == "Y" ? true : false;
                lblFollUPCReq.Visible = trackEntity.FOLLOWUPCR == "Y" ? true : false;
                dtDiagnosed.Enabled = trackEntity.DIAGNOSEE == "Y" ? true : false;
                lblDiagnoReq.Visible = trackEntity.DIAGNOSER == "Y" ? true : false;
                dtSSR.Enabled = trackEntity.SSRE == "Y" ? true : false;
                lblSSRReq.Visible = trackEntity.SSRR == "Y" ? true : false;
                txtWheree.Enabled = trackEntity.WHEREE == "Y" ? true : false;
                lblwhereReq.Visible = trackEntity.WHERER == "Y" ? true : false;
                cmbFund.Enabled = trackEntity.FUNDE == "Y" ? true : false;
                lblFundReq.Visible = trackEntity.FUNDR == "Y" ? true : false;

                btnAgency.Enabled = trackEntity.WHEREE == "Y" ? true : false;
                if (strPostTask == "Post")
                    fillCustomQuestions(trackEntity.CustQCodes.Trim(), BaseForm.BaseApplicationNo, BaseForm.BaseYear, propChldmedi);
            }

        }

        private void gvwChildDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Mode.Equals("Add"))
            {
                if (gvwChildDetails.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        if (e.ColumnIndex == gvChkSelect.Index)
                        {
                            if (gvwChildDetails.CurrentRow.Cells["gvtSelectValue"].Value.ToString() == "T")
                            {
                                gvwChildDetails.CurrentRow.Cells["gvChkSelect"].Value = Img_Blank;
                                gvwChildDetails.CurrentRow.Cells["gvtSelectValue"].Value = "E";
                            }
                            else if (gvwChildDetails.CurrentRow.Cells["gvtSelectValue"].Value.ToString() == "E")
                            {
                                gvwChildDetails.CurrentRow.Cells["gvChkSelect"].Value = Img_Tick;
                                gvwChildDetails.CurrentRow.Cells["gvtSelectValue"].Value = "T";
                            }
                        }
                    }


                    List<DataGridViewRow> SelectedgvRows = (from c in gvwChildDetails.Rows.Cast<DataGridViewRow>().ToList()
                                                            where (c.Cells["gvtSelectValue"].Value.ToString().ToUpper().Equals("T"))
                                                            select c).ToList();

                    if (SelectedgvRows.Count > 0)
                        btnOk.Visible = true;
                    else
                        btnOk.Visible = false;
                }
            }
        }


        private void gvwSelectTask_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwSelectTask.Rows.Count > 0)
            {
                if (gvwSelectTask.SelectedRows[0].Selected)
                {
                    ClearControls(string.Empty);
                    pnlDates.Enabled = true;
                    pnlGridNotes.Enabled = true;
                    if (gvwSelectTask.SelectedRows[0].Cells["gvtAddressdt"].Value != string.Empty)
                    {
                        ChldMediEntity chldmedientity = gvwSelectTask.SelectedRows[0].Cells["gvtAddressdt"].Tag as ChldMediEntity;
                        propChldmedi = chldmedientity;
                        EnableDisableControls("PostView");
                        FillForm("PostView");
                        txtCaseNotes.Text = gvwSelectTask.SelectedRows[0].Cells["gvtTaskDesc"].Tag as string;
                        if (gvwCustomQuestions.Rows.Count > 0)
                        {
                            CustomQuestionsEntity dr = gvwCustomQuestions.SelectedRows[0].Tag as CustomQuestionsEntity;
                            string fieldType = dr.CUSTRESPTYPE.ToString();
                            if (fieldType.Equals("D") || fieldType.Equals("C"))
                            {
                                gvwCustomQuestions.SelectedRows[0].ReadOnly = true;
                            }
                            else
                            {
                                gvwCustomQuestions.SelectedRows[0].Cells[0].ReadOnly = true;
                                gvwCustomQuestions.SelectedRows[0].Cells[1].ReadOnly = true;
                            }

                            string custQuestionResp = string.Empty;
                            string custQuestionCode = string.Empty;

                            if (fieldType.Equals("D") || fieldType.Equals("C"))
                            {
                                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("HSS00133", dr.CUSTCODE);


                                string code = propChldmedi.ANSWER1;
                                CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
                                if (custRespEntity != null)
                                {
                                    custQuestionResp = custRespEntity.RespDesc;
                                    custQuestionCode = propChldmedi.ANSWER1;
                                }

                                gvwCustomQuestions.SelectedRows[0].Cells["Response"].Tag = custQuestionCode;
                                gvwCustomQuestions.SelectedRows[0].Cells[2].Value = custQuestionResp;
                                gvwCustomQuestions.SelectedRows[0].Cells[1].Tag = "U";
                            }
                            else if (fieldType.Equals("N"))
                            {
                                custQuestionResp = propChldmedi.ANSWER2.ToString();
                            }
                            else if (fieldType.Equals("T"))
                            {
                                custQuestionResp = LookupDataAccess.Getdate(propChldmedi.ANSWER3.ToString());

                            }
                            else
                            {
                                custQuestionResp = propChldmedi.ANSWER1.ToString();

                            }

                            if (custQuestionResp != string.Empty)
                            {
                                gvwCustomQuestions.SelectedRows[0].Cells[2].Value = custQuestionResp;
                                gvwCustomQuestions.SelectedRows[0].Cells[1].Tag = "U";
                                // gvwCustomQuestions.Rows[rowIndex].Cells["FamilySeq"].Value = response[0].ACTSNPFAMILYSEQ;
                                // gvwCustomQuestions.Rows[rowIndex].Cells["ResponceSeq"].Value = response[0].ACTRESPSEQ;
                                gvwCustomQuestions.SelectedRows[0].Cells["Code"].Value = dr.CUSTCODE;
                                //gvwCustomQuestions.SelectedRows[0].Cells[0].Value = saveImage;
                                //gvwCustomQuestions.SelectedRows[0].Cells["ResponceDelete"].Value = DeleteImage;
                            }
                        }
                        btnOk.Visible = false;
                    }
                    else
                    {
                        EnableDisableControls("Post");
                        btnOk.Visible = true;
                        //if ((!BaseForm.UserProfile.FastLoad.Equals("Y")) && (BaseForm.UserProfile.CalcSBCB.Equals("Y")))
                        //{
                        CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                        List<ChldTrckREntity> chldtrckrlist = new List<ChldTrckREntity>();
                        if (propchldTrckList.Count > 0)
                        {
                            chldtrckrlist = _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", gvwSelectTask.SelectedRows[0].Cells["gvtTask"].Value.ToString(), string.Empty);
                        }

                        string strSBCBDate = FillSBCDDetails(BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].IntakeDate, casesnp.AltBdate, string.Empty, "0000", gvwSelectTask.SelectedRows[0].Cells["gvtTask"].Value.ToString(), chldtrckrlist);
                        if (strSBCBDate != string.Empty)
                        {
                            dtSBCB.Value = Convert.ToDateTime(strSBCBDate);
                            dtSBCB.Checked = true;
                        }
                        else
                            dtSBCB.Checked = false;
                        //}
                    }
                }
            }
        }

        private void btnTasks_Click(object sender, EventArgs e)
        {
            HSSB2111FundForm TaskForm = new HSSB2111FundForm(BaseForm, BaseForm.BaseTaskEntity, Privileges, "0000", string.Empty, string.Empty);
            TaskForm.FormClosed += new FormClosedEventHandler(TaskForm_FormClosed);
            TaskForm.StartPosition = FormStartPosition.CenterScreen;
            TaskForm.ShowDialog();
        }

        void TaskForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                BaseForm.BaseTaskEntity.Clear();
                BaseForm.BaseTaskEntity = form.GetSelectedTracks();
                if (Mode == "Add")
                {
                    gvwSelectTask.SelectionChanged -= new EventHandler(gvwSelectTask_SelectionChanged);
                    FillTaskGrid();
                    gvwSelectTask.SelectionChanged += new EventHandler(gvwSelectTask_SelectionChanged);
                    gvwSelectTask_SelectionChanged(sender, e);
                }
                else
                {

                    FillComboTask();
                    cmbTask_SelectedIndexChanged(sender, e);
                }
            }
        }

        private void chkClassRoom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClassRoom.Checked == true)
            {
                cmbClass.Enabled = true;
            }
            else
            {
                cmbClass.Enabled = false;
            }

            List<CaseEnrlEntity> CaseEnrlDetails = propCaseENrl.FindAll(u => u.App.Equals(BaseForm.BaseApplicationNo));
            if (CaseEnrlDetails.Count > 0)
            {
                CommonFunctions.SetComboBoxValue(cmbClass, CaseEnrlDetails[0].Site.Trim() + CaseEnrlDetails[0].Room.Trim() + CaseEnrlDetails[0].AMPM.Trim());
            }
            else
            {
                cmbClass.SelectedIndex = 0;
            }
            cmbClass_SelectedIndexChanged(sender, e);
        }

        private void HSS00134Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tlCaseNotes")
            {
                string strYear = "    ";
                if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                {
                    strYear = BaseForm.BaseYear;
                }
                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo + propTask + propSeq);
                CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo + propTask.Trim() + propSeq.Trim());
                caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                caseNotes.StartPosition = FormStartPosition.CenterScreen;
                caseNotes.ShowDialog();
            }
            if (e.Tool.Name == "tlHelp")
            {
            }
        }

        string _isUploaded = "N"; string _tempFilePath = ""; string strOrgFileName = ""; string strfiletype = "";
        private void uploadBrowser_Uploaded(object sender, UploadedEventArgs e)
        {
            string strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
            string strTempFolderName = _tempFilePath = _model.lookupDataAccess.GetReportPath() + "\\Temp\\ScannedImages\\" + strFolderName;
            DirectoryInfo MyDir = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    MyFile.Delete();
                }
            }


            var mobjResult = e.Files[0];

            string strName = strOrgFileName = mobjResult.FileName;
            string strType = strfiletype = Path.GetExtension(strName.ToString().ToLower()); //getFileFormat(mobjResult.ContentType);
            long strr = mobjResult.ContentLength;

            string strModifyfilename = mobjResult.FileName;
            strModifyfilename = strModifyfilename.Replace(',', '_').Replace('&', '_').Replace('$', '_').Replace('#', '_').Replace('/', '_').Replace("'", "_").Replace('{', '_').Replace('}', '_').Replace('@', '_').Replace('%', '_').Replace('/', '_').Replace('?', '_');

            lbldocupldName.Text = strName;
            //_tempFilePath = strTempFolderName;
            mobjResult.SaveAs(strTempFolderName + "\\" + strName);
            _isUploaded = "Y";
            // this.Close();
        }


        public string SaveTaskDocUpload(ChldMediEntity mediEntity, string _Mode, string strMediSeq)
        {
            string _saveFlag = "N";
            if (_isUploaded.ToString() == "Y")
            {

                string TSKUPLD_ID = TaskUpldID;// row.Cells["coltskupldid"].Value.ToString();
                //string TSKUPLD_AGENCY, string TSKUPLD_DEPT, string TSKUPLD_PROGRAM, string TSKUPLD_YEAR, string TSKUPLD_APPNO, string TSKUPLD_TASK, string MEDI_COMPONENT,
                string TSKUPLD_ORIG_NAME = strOrgFileName; // row.Cells["ColoDocName"].Value.ToString();
                string TSKUPLD_UPLOAD_BY = BaseForm.UserID;
                string TSKUPLD_LSTC_OPERATOR = BaseForm.UserID;
                string MEDI_SEQNO = strMediSeq; // mediEntity.SEQ; //strMediSeq;
                string TaskupldID = string.Empty;


                if (TSKUPLD_ID != null && TSKUPLD_ID != "")
                {
                    _Mode = "Edit";
                }
                else
                    _Mode = "Add";

                string filetype = strfiletype; // row.Cells["colfiletype"].Value.ToString();

                if (_model.ChldTrckData.INSUPDEL_TASKUPLDS(TSKUPLD_ID, mediEntity.AGENCY, mediEntity.DEPT, mediEntity.PROG, mediEntity.YEAR, mediEntity.APP_NO, mediEntity.TASK,
                    TSKUPLD_ORIG_NAME, "", TSKUPLD_UPLOAD_BY, "", TSKUPLD_LSTC_OPERATOR, MEDI_SEQNO, mediEntity.COMPONENT, filetype, _Mode, out TaskupldID))
                {
                    // move uploaded document to tasks folder
                    string strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                    string _filename = TaskupldID + filetype;
                    string strTaskUpldFolder = _model.lookupDataAccess.GetReportPath() + "\\ScannedImages\\" + strFolderName + "\\TaskUploads";
                    DirectoryInfo info = new DirectoryInfo(strTaskUpldFolder);
                    if (!info.Exists)
                    {
                        info.Create();
                    }

                    if (_prvDocFile != null && _prvDocFile != "")
                    {
                        try
                        {
                            File.Delete(strTaskUpldFolder + "\\" + _prvDocFile);
                        }
                        catch
                        {

                        }
                    }

                    File.Copy(_tempFilePath + "\\" + TSKUPLD_ORIG_NAME, strTaskUpldFolder + "\\" + _filename, true);
                }

                _isUploaded = "N";
                _saveFlag = "Y";
            }

            return _saveFlag;
        }

        //Added by Vikash on 01/02/2025 as per Client Tracking enhancement document.
        private void cmbFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fund_List_Filtered = propCaseENrl.FindAll(x => x.FundHie.Trim() == ((Captain.Common.Utilities.ListItem)cmbFund.SelectedItem).Value.ToString().Trim()).ToList();

            if (Fund_List_Filtered.Count > 0 || ((Captain.Common.Utilities.ListItem)cmbFund.SelectedItem).Value.ToString().Trim() == "0")
            {
                lblFundWarn.Visible = false;
            }
            else
            {
                lblFundWarn.Visible = true;
            }
        }
    }
}