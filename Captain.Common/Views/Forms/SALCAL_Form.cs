/************************************************************************
 * Conversion On        :   11/28/2022
 * Converted By         :   Kranthi
 * Last Modification On :   11/28/2022
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SALCAL_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public SALCAL_Form(BaseForm baseform, PrivilegeEntity privileges, CASEACTEntity pass_entity, string actdate, string worker, string Fund, CASESP0Entity sp_header_rec, string mode, List<SALACTEntity> selSalactlist)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            BaseForm = baseform;
            Privileges = privileges;
            Pass_CA_Entity = pass_entity;
            ServiceActDate = actdate;
            CaseWorker = worker; FundingSource = Fund;
            SP_Header_Rec = sp_header_rec;
            Mode = mode;
            SALACTList = selSalactlist;

            propReportPath = _model.lookupDataAccess.GetReportPath();

            FormType = "SAL";

            btnSign.Visible = false;

            this.Size = new Size(this.Width, this.Height - pnlResp.Height);

            //if (SP_Header_Rec.Sp0Notes == "Y")
            //{
            //    pnlSPNotes.Visible = true;
            //    //lblSPName.Text = SP_Header_Rec.Desc.Trim();
            //    List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Pass_CA_Entity.Year + BaseForm.BaseApplicationNo + Pass_CA_Entity.Service_plan + Pass_CA_Entity.SPM_Seq);
            //    if (caseNotesEntity.Count > 0)
            //    {
            //        txtNotes.Text = caseNotesEntity[0].Data.Trim();
            //    }
            //}
            //else pnlSPNotes.Visible = false;

            if (sp_header_rec.Sp0Notes == "Y" && Mode.Equals("Edit")) { ShowCaseNotesImages(); this.Tools["picSPMNotes"].Visible = true; } else this.Tools["picSPMNotes"].Visible = false;


            getdata();
            Fill_CaseWorker();
            Fill_Funding();
            Fill_Combos();
            FillControls();
            FillgvNames();

        }

        public SALCAL_Form(BaseForm baseform, PrivilegeEntity privileges, CASECONTEntity pass_entity, string mode)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            BaseForm = baseform;
            Privileges = privileges;
            PASS_Cont_Entity = pass_entity;

            Mode = mode;

            panel1.Visible = false; pnlSPNotes.Visible = false; this.Tools["picSPMNotes"].Visible = false;

            this.Size = new Size(this.Width, this.Height - (pnlResp.Height + panel1.Height));

            //this.Size = new Size(1018, 800);

            btnSign.Visible = false;

            //this.panel3.Location = new System.Drawing.Point(0, 153);
            //this.panel4.Location = new System.Drawing.Point(0, 409);
            //this.Form.Size = new System.Drawing.Size(1173, 440);
            // this.panel4.Location = new System.Drawing.Point(0, 445);
            // this.Size = new System.Drawing.Size(897, 477);
            FormType = "CAL";
            txtRecName.Text = PASS_Cont_Entity.Contact_Name.Trim();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Tools["PbPdf"].Visible = true;

            getdata();


            FillgvNames();

        }

        #region properties

        public BaseForm BaseForm { get; set; }
        public CASESP0Entity SP_Header_Rec { get; set; }

        public string FormType { get; set; }

        public string propReportPath { get; set; }


        public string Hierarchy { get; set; }

        public string Year { get; set; }


        public string Mode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public CASEACTEntity Pass_CA_Entity { get; set; }

        public List<SaldefEntity> SALDEFEntity { get; set; }

        public List<SalquesEntity> SALQUESEntity { get; set; }
        public List<SalqrespEntity> SALQUESRespEntity { get; set; }

        public List<SALACTEntity> SALACTList { get; set; }

        public List<CALCONTEntity> CALCONTList { get; set; }


        public List<SALQLNKEntity> salQlinkEntitylist { get; set; }

        public CASECONTEntity PASS_Cont_Entity { get; set; }

        public string GroupCode { get; set; }
        public string ServiceActDate { get; set; }
        public string CaseWorker { get; set; }
        public string FundingSource { get; set; }

        public string MenuSelectValue { get; set; }
        public string MenuSelectCode { get; set; }



        #endregion

        private void getdata()
        {
            SaldefEntity Search_saldef_Entity = new SaldefEntity(true);

            List<SaldefEntity> SALDEF = _model.SALDEFData.Browse_SALDEF(Search_saldef_Entity, "Browse", BaseForm.UserID, BaseForm.BaseAgency);
            if (SALDEF.Count > 0)
            {
                if (FormType == "SAL")
                {
                    ////List<SaldefEntity> SALDEFEntity = new List<SaldefEntity>();
                    //if (SALDEF.Count > 0)
                    //{

                    //    if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    //    {
                    //        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG) || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 4) + "**") || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 2) + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                    //        if (SALDEFEntity.Count == 0)
                    //            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG) || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 4) + "**") || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 2) + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S"));
                    //        if (SALDEFEntity.Count == 0)
                    //            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG) || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 4) + "**") || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 2) + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(string.Empty) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S"));
                    //    }
                    //    else
                    //    {
                    //        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Contains(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S"));
                    //        if (SALDEFEntity.Count == 0)
                    //            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S"));
                    //        if (SALDEFEntity.Count == 0)
                    //            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(string.Empty) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S"));
                    //        //SALDEF = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_TYPE.Equals("S"));
                    //    }


                    //}

                    SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Contains(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S"));
                    if (SALDEFEntity.Count == 0)
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S"));
                    if (SALDEFEntity.Count == 0)
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(string.Empty) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S"));
                }
                else if (FormType == "CAL")
                {
                    SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_TYPE.Equals("C"));
                }

                //SALDEFEntity = SALDEF.FindAll(u => u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Contains(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S"));
            }
            //SALDEFEntity = SALDEF.FindAll(u => u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Contains(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S"));

            SalquesEntity Search_Salques_Entity = new SalquesEntity(true);
            SALQUESEntity = _model.SALDEFData.Browse_SALQUES(Search_Salques_Entity, "Browse");

            SalqrespEntity Search_Salqresp_Entity = new SalqrespEntity(true);
            SALQUESRespEntity = _model.SALDEFData.Browse_SALQRESP(Search_Salqresp_Entity, "Browse");

            SALACTList = new List<SALACTEntity>(); CALCONTList = new List<CALCONTEntity>();
            if (FormType == "SAL")
            {
                if (SALACTList.Count == 0 && Mode == "Edit")
                {
                    SALACTEntity Search_SALACT = new SALACTEntity(true);
                    Search_SALACT.SALACT_TYPE = "S";
                    Search_SALACT.SALACT_ID = Pass_CA_Entity.ACT_ID.ToString();
                    SALACTList = _model.SALDEFData.Browse_SALACT(Search_SALACT, "Browse");
                }
            }
            else if (FormType == "CAL")
            {
                CALCONTEntity Search_SALACT = new CALCONTEntity(true);
                Search_SALACT.CALCONT_ID = PASS_Cont_Entity.Contact_ID;
                CALCONTList = _model.SALDEFData.Browse_CALCONT(Search_SALACT, "Browse");
            }

            salQlinkEntitylist = _model.SALDEFData.Browse_SALQLNK(string.Empty, string.Empty, string.Empty);

        }

        private void Fill_Combos()
        {
            this.cmbStatus.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            List<CommonEntity> status = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALStatus, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetGender();
            // Gender = filterByHIE(Gender);
            cmbStatus.Items.Insert(0, new Captain.Common.Utilities.ListItem(" ", "0"));
            cmbStatus.ColorMember = "FavoriteColor";
            cmbStatus.SelectedIndex = 0;
            foreach (CommonEntity gender in status)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(gender.Desc, gender.Code, gender.Active, gender.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbStatus.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && gender.Default.Equals("Y")) Cmb_UOM.SelectedItem = li;
            }
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);

            this.cmbLocation.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            List<CommonEntity> location = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALLocation, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetGender();
            // Gender = filterByHIE(Gender);
            cmbLocation.Items.Insert(0, new Captain.Common.Utilities.ListItem(" ", "0"));
            cmbLocation.ColorMember = "FavoriteColor";
            cmbLocation.SelectedIndex = 0;
            foreach (CommonEntity gender in location)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(gender.Desc, gender.Code, gender.Active, gender.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbLocation.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && gender.Default.Equals("Y")) Cmb_UOM.SelectedItem = li;
            }
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);

            this.cmbReceipent.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            List<CommonEntity> Recipient = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALRecipient, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetGender();
            // Gender = filterByHIE(Gender);
            cmbReceipent.Items.Insert(0, new Captain.Common.Utilities.ListItem(" ", "0"));
            cmbReceipent.ColorMember = "FavoriteColor";
            cmbReceipent.SelectedIndex = 0;
            foreach (CommonEntity gender in Recipient)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(gender.Desc, gender.Code, gender.Active, gender.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbReceipent.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && gender.Default.Equals("Y")) Cmb_UOM.SelectedItem = li;
            }
            this.cmbReceipent.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);

            this.cmbAttn.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            List<CommonEntity> attn = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALAttendence, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetGender();
            // Gender = filterByHIE(Gender);
            cmbAttn.Items.Insert(0, new Captain.Common.Utilities.ListItem(" ", "0"));
            cmbAttn.ColorMember = "FavoriteColor";
            cmbAttn.SelectedIndex = 0;
            foreach (CommonEntity gender in attn)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(gender.Desc, gender.Code, gender.Active, gender.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbAttn.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && gender.Default.Equals("Y")) Cmb_UOM.SelectedItem = li;
            }
            this.cmbAttn.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);

        }

        private void Fill_CaseWorker()
        {

            DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(BaseForm.BaseAgency, "**", "**");
            string strNameFormat = null, strCwFormat = null;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            }

            CmbWorker.Items.Clear(); CmbWorker.ColorMember = "FavoriteColor";
            //Cmb_MS_Worker.Items.Clear(); Cmb_MS_Worker.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();

            listItem.Add(new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Black));
                    }

                    CmbWorker.Items.AddRange(listItem.ToArray());

                }
            }
            switch ("CA")
            {
                case "CA":
                    if (Pass_CA_Entity.Rec_Type == "I") // !string.IsNullOrEmpty(MST_Intakeworker))
                    {

                        if (!string.IsNullOrEmpty(BaseForm.UserProfile.CaseWorker))
                            CommonFunctions.SetComboBoxValue(CmbWorker, BaseForm.UserProfile.CaseWorker);
                        //else
                        //    !string.IsNullOrEmpty(MST_Intakeworker)
                        //        SetComboBoxValue(CmbWorker, MST_Intakeworker);
                    }
                    //SetComboBoxValue(CmbWorker, MST_Intakeworker);
                    else
                        CmbWorker.SelectedIndex = 0;
                    break;

            }
        }

        private void Fill_Funding()
        {
            CmbFunding.Items.Clear(); CmbFunding.ColorMember = "FavoriteColor";


            if (!string.IsNullOrEmpty(SP_Header_Rec.Funds))
            {
                bool Fund_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0;
                List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
                FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
                string Funds_Str = SP_Header_Rec.Funds;
                int Tmp_Curr_Fund_Len = 0;

                foreach (SPCommonEntity Entity in FundingList)
                {

                    Fund_Exists = false; Pos = 0;
                    for (int i = 0; Pos < SP_Header_Rec.Funds.Length; i++)
                    {

                        Tmp_Curr_Fund_Len = (Funds_Str.Substring(Pos, Funds_Str.Substring(Pos, (Funds_Str.Length - Pos)).Length)).Length;

                        if (Entity.Code == SP_Header_Rec.Funds.Substring(Pos, (Tmp_Curr_Fund_Len >= 10 ? 10 : Tmp_Curr_Fund_Len)).Trim())
                        {
                            Fund_Exists = true; break;
                        }
                        Pos += 10;

                    }

                    if (Fund_Exists)
                    {
                        CmbFunding.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                        Tmp_Loop_Cnt++;

                    }
                }

            }
            CmbFunding.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            CmbFunding.SelectedIndex = 0;
        }

        private void FillControls()
        {
            txtRecName.Text = BaseForm.BaseApplicationName.Trim();
            Act_Date.Text = Pass_CA_Entity.ACT_Date.Trim();

            CommonFunctions.SetComboBoxValue(CmbWorker, Pass_CA_Entity.Caseworker);
            CommonFunctions.SetComboBoxValue(CmbFunding, Pass_CA_Entity.Fund1);
        }

        private void FillgvNames()
        {
            if (SALDEFEntity.Count > 0)
            {
                int rowIndex = 0;
                foreach (SaldefEntity Entity in SALDEFEntity)
                {
                    rowIndex = gvSALNames.Rows.Add(Entity.SALD_NAME, Entity.SALD_ID, Entity.SALD_SIGN_REQURED);
                }

                if (gvSALNames.Rows.Count > 0)
                    gvSALNames.Rows[0].Selected = true;
            }
        }

        string DeleteImage = "delete-item";
        private void FillgvQuestions(string ID)
        {
            gvQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
            gvQuestions.Rows.Clear();

            if (SALQUESEntity.Count > 0)
            {
                List<SalquesEntity> SelQues = SALQUESEntity.FindAll(u => u.SALQ_SALD_ID.Equals(ID));

                if (SelQues.Count > 0)
                {
                    SelQues = SelQues.OrderBy(u => Convert.ToInt32(u.SALQ_GRP_SEQ)).ThenBy(u => Convert.ToInt32(u.SALQ_SEQ)).ThenBy(u => Convert.ToInt32(u.SALQ_GRP_CODE)).ToList();

                    int rowIndex = 0;
                    foreach (SalquesEntity Entity in SelQues)
                    {


                        //rowIndex = gvQuestions.Rows.Add(Entity.SALQ_DESC, "", "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, Entity.SALQ_GRP_SEQ, Entity.SALQ_CODE, Entity.SALQ_TYPE);

                        if (Entity.SALQ_SEQ == "0")
                        {
                            rowIndex = gvQuestions.Rows.Add(Entity.SALQ_DESC, "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, Entity.SALQ_GRP_SEQ, string.Empty, Entity.SALQ_TYPE, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            gvQuestions.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Desc"].Style.ForeColor = Color.Blue;
                            gvQuestions.Rows[rowIndex].Tag = Entity;
                        }
                        else
                        {
                            SALQLNKEntity SelLinkQuesList = salQlinkEntitylist.Find(u => u.SALQL_Q_ID.Equals(Entity.SALQ_ID));
                            if (SelLinkQuesList != null)
                            {
                                rowIndex = gvQuestions.Rows.Add(Entity.SALQ_DESC, "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, Entity.SALQ_GRP_SEQ, string.Empty, Entity.SALQ_TYPE, string.Empty, SelLinkQuesList.SALQL_LINKQ, SelLinkQuesList.SALQL_REQ, SelLinkQuesList.SALQL_ENABLE, SelLinkQuesList.SALQL_DISABLE, string.Empty);
                            }
                            else
                                rowIndex = gvQuestions.Rows.Add(Entity.SALQ_DESC, "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, Entity.SALQ_GRP_SEQ, string.Empty, Entity.SALQ_TYPE, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            gvQuestions.Rows[rowIndex].Tag = Entity;
                        }


                        if (FormType == "SAL")
                        {
                            if (SALACTList.Count > 0)
                            {
                                foreach (DataGridViewRow gvrows in gvQuestions.Rows)
                                {
                                    if (Entity.SALQ_SEQ != "0" && Entity.SALQ_ID == gvrows.Cells["Ques_ID"].Value.ToString())
                                    {
                                        SALACTEntity MatResponce = SALACTList.Find(u => u.SALACT_Q_ID.Equals(Entity.SALQ_ID) && u.SALACT_SALID.Equals(Entity.SALQ_SALD_ID));
                                        if (MatResponce != null)
                                        {
                                            //gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                            if (gvrows.Cells["Ques_Type"].Value.ToString() == "T")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = LookupDataAccess.Getdate(MatResponce.SALACT_DATE_RESP.Trim());
                                                gvrows.Cells["Responce"].Value = LookupDataAccess.Getdate(MatResponce.SALACT_DATE_RESP.Trim());
                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_DATE_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "N")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_NUM_RESP;
                                                gvrows.Cells["Responce"].Value = MatResponce.SALACT_NUM_RESP;
                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_NUM_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "D")
                                            {
                                                SalqrespEntity selRespEntity = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID) && u.SALQR_CODE.Trim().Equals(MatResponce.SALACT_MULTI_RESP.Trim()));
                                                if (selRespEntity != null)
                                                {
                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = selRespEntity.SALQR_DESC;
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                                }
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "C")
                                            {
                                                string custQuestionResp = string.Empty;
                                                List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID));

                                                if (selRespEntity.Count > 0)
                                                {
                                                    string response1 = MatResponce.SALACT_MULTI_RESP;
                                                    if (!string.IsNullOrEmpty(response1))
                                                    {
                                                        string[] arrResponse = null;
                                                        if (response1.IndexOf(',') > 0)
                                                        {
                                                            arrResponse = response1.Split(',');
                                                        }
                                                        else if (!response1.Equals(string.Empty))
                                                        {
                                                            arrResponse = new string[] { response1 };
                                                        }


                                                        foreach (string stringitem in arrResponse)
                                                        {
                                                            SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                                            if (custRespEntity != null)
                                                            {
                                                                custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                                                //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                            }

                                                        }
                                                    }


                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = custQuestionResp;
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                }

                                            }
                                            else
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP; gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_MULTI_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }

                                        }
                                        else
                                        {
                                            //string strresp = gvrows.Cells["Responce"].Value == null ? string.Empty : gvrows.Cells["Responce"].Value.ToString();
                                            //if (strresp != string.Empty)
                                            //{
                                            //    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            ////}
                                            //gvrows.Cells["Responce"].Value = string.Empty;
                                            //gvrows.Cells["ResponceCode"].Value = string.Empty;
                                        }
                                    }


                                }
                            }
                        }
                        else if (FormType == "CAL")
                        {
                            if (CALCONTList.Count > 0)
                            {
                                foreach (DataGridViewRow gvrows in gvQuestions.Rows)
                                {
                                    if (Entity.SALQ_SEQ != "0" && Entity.SALQ_ID == gvrows.Cells["Ques_ID"].Value.ToString())
                                    {
                                        CALCONTEntity MatResponce = CALCONTList.Find(u => u.CALCONT_Q_ID.Equals(Entity.SALQ_ID) && u.CALCONT_SALID.Equals(Entity.SALQ_SALD_ID));
                                        if (MatResponce != null)
                                        {
                                            //gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                            if (gvrows.Cells["Ques_Type"].Value.ToString() == "T")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = LookupDataAccess.Getdate(MatResponce.CALCONT_DATE_RESP.Trim());
                                                gvrows.Cells["Responce"].Value = LookupDataAccess.Getdate(MatResponce.CALCONT_DATE_RESP.Trim());

                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_DATE_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "N")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_NUM_RESP;
                                                gvrows.Cells["Responce"].Value = MatResponce.CALCONT_NUM_RESP;

                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_NUM_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "D")
                                            {
                                                SalqrespEntity selRespEntity = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID) && u.SALQR_CODE.Trim().Equals(MatResponce.CALCONT_MULTI_RESP.Trim()));
                                                if (selRespEntity != null)
                                                {
                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = selRespEntity.SALQR_DESC;
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                                }
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "C")
                                            {
                                                string custQuestionResp = string.Empty;
                                                List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID));

                                                if (selRespEntity.Count > 0)
                                                {
                                                    string response1 = MatResponce.CALCONT_MULTI_RESP;
                                                    if (!string.IsNullOrEmpty(response1))
                                                    {
                                                        string[] arrResponse = null;
                                                        if (response1.IndexOf(',') > 0)
                                                        {
                                                            arrResponse = response1.Split(',');
                                                        }
                                                        else if (!response1.Equals(string.Empty))
                                                        {
                                                            arrResponse = new string[] { response1 };
                                                        }


                                                        foreach (string stringitem in arrResponse)
                                                        {
                                                            SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                                            if (custRespEntity != null)
                                                            {
                                                                custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                                                //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                            }

                                                        }
                                                    }


                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = custQuestionResp;
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                }

                                            }
                                            else
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_MULTI_RESP; gvrows.Cells["Responce"].Value = MatResponce.CALCONT_MULTI_RESP;
                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_MULTI_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }

                                        }
                                        else
                                        {
                                            //string strresp = gvrows.Cells["Responce"].Value == null ? string.Empty : gvrows.Cells["Responce"].Value.ToString();
                                            //if (strresp != string.Empty)
                                            //{
                                            //    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            ////}
                                            //gvrows.Cells["Responce"].Value = string.Empty;
                                            //gvrows.Cells["ResponceCode"].Value = string.Empty;
                                        }
                                    }


                                }
                            }
                        }



                    }

                    if (gvQuestions.Rows.Count > 0)
                    {
                        EnabldisableQuestion();
                        gvQuestions.CurrentCell = gvQuestions.Rows[0].Cells[1];
                        //int CurrentPage = 1;
                        // gvQuestions.CurrentPage = CurrentPage;
                        // gvQuestions.FirstDisplayedScrollingRowIndex = 0;

                        gvQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
                        //gvQuestions.SelectedRows[0].Selected = true;
                    }
                }
            }
        }

        private void FillgvQuestions1(string ID)
        {
            gvQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
            gvQuestions.Rows.Clear();
            /**********************************************/
            if (gvQuestions.Columns.Count == 0)
            {
                gvQuestions.Columns.Add("Ques_Desc");
                gvQuestions.Columns.Add("Responce");
                gvQuestions.Columns.Add("Ques_ID");
                gvQuestions.Columns.Add("Ques_SAL_ID");
                gvQuestions.Columns.Add("Grp_Code");
                gvQuestions.Columns.Add("Grp_Seq");
                gvQuestions.Columns.Add("Ques_Code");
                gvQuestions.Columns.Add("Ques_Type");
                gvQuestions.Columns.Add("ResponceCode");
                gvQuestions.Columns.Add("Link_Ques");
                gvQuestions.Columns.Add("Ques_Req");
                gvQuestions.Columns.Add("Ques_Enable");
                gvQuestions.Columns.Add("Ques_Disable");
                gvQuestions.Columns.Add("Link_Ques_Resp");
                gvQuestions.Columns.Add("Ques_Delete");
            }
            /**********************************************/





            if (SALQUESEntity.Count > 0)
            {
                List<SalquesEntity> SelQues = SALQUESEntity.FindAll(u => u.SALQ_SALD_ID.Equals(ID));

                if (SelQues.Count > 0)
                {
                    SelQues = SelQues.OrderBy(u => Convert.ToInt32(u.SALQ_GRP_SEQ)).ThenBy(u => Convert.ToInt32(u.SALQ_SEQ)).ThenBy(u => Convert.ToInt32(u.SALQ_GRP_CODE)).ToList();

                    int rowIndex = 0;
                    foreach (SalquesEntity Entity in SelQues)
                    {
                        rowIndex = gvQuestions.Rows.Add();

                        #region ColumnHeaders
                        for (int x = 0; x < 15; x++)
                        {
                            int HeadcolIndex = x;

                            if (x == 14)
                            {
                                DataGridViewImageCell gvCellDel = new DataGridViewImageCell();
                                this.gvQuestions[HeadcolIndex, rowIndex] = gvCellDel;
                                this.gvQuestions[HeadcolIndex, rowIndex].Value = string.Empty;
                                this.gvQuestions.Columns[HeadcolIndex].Visible = true;
                                this.gvQuestions.Columns[HeadcolIndex].HeaderText = "Delete";
                                this.gvQuestions.Columns[HeadcolIndex].Width = 70;
                            }
                            else
                            {
                                DataGridViewCell gvCell = new DataGridViewCell();
                                //gvCell.Style.ForeColor = System.Drawing.Color.Red;
                                this.gvQuestions[HeadcolIndex, rowIndex] = gvCell;
                                this.gvQuestions[HeadcolIndex, rowIndex].Value = string.Empty;
                                gvCell.Style.Padding = new System.Windows.Forms.Padding(2);
                                this.gvQuestions.Columns[HeadcolIndex].Visible = false;
                                this.gvQuestions.Columns[HeadcolIndex].HeaderText = "";

                                if (x == 0)
                                {
                                    this.gvQuestions.Columns[HeadcolIndex].Visible = true;
                                    this.gvQuestions.Columns[HeadcolIndex].HeaderText = "Question";
                                    this.gvQuestions.Columns[HeadcolIndex].Width = 450;
                                    this.gvQuestions.Columns[HeadcolIndex].HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                }
                                if (x == 1)
                                {
                                    this.gvQuestions.Columns[HeadcolIndex].Visible = true;
                                    this.gvQuestions.Columns[HeadcolIndex].HeaderText = "Response";
                                    this.gvQuestions.Columns[HeadcolIndex].Width = 435;
                                }

                            }


                        }
                        #endregion
                        //  rowIndex = gvQuestions1.Rows.Add();

                        if (Entity.SALQ_SEQ == "0")
                        {
                            gvQuestions.Rows[rowIndex].Cells["Ques_Desc"].Value = Entity.SALQ_DESC;
                            gvQuestions.Rows[rowIndex].Cells["Responce"].Value = "";
                            gvQuestions.Rows[rowIndex].Cells["Ques_ID"].Value = Entity.SALQ_ID;
                            gvQuestions.Rows[rowIndex].Cells["Ques_SAL_ID"].Value = Entity.SALQ_SALD_ID;
                            gvQuestions.Rows[rowIndex].Cells["Grp_Code"].Value = Entity.SALQ_GRP_CODE;
                            gvQuestions.Rows[rowIndex].Cells["Grp_Seq"].Value = Entity.SALQ_GRP_SEQ;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Code"].Value = string.Empty;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Type"].Value = Entity.SALQ_TYPE;

                            gvQuestions.Rows[rowIndex].Cells["ResponceCode"].Value = string.Empty;
                            gvQuestions.Rows[rowIndex].Cells["Link_Ques"].Value = string.Empty;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Req"].Value = string.Empty;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Enable"].Value = string.Empty;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Disable"].Value = string.Empty;
                            gvQuestions.Rows[rowIndex].Cells["Link_Ques_Resp"].Value = string.Empty;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Delete"].Value = string.Empty;

                            // rowIndex = gvQuestions1.Rows.Add(Entity.SALQ_DESC, "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, Entity.SALQ_GRP_SEQ, string.Empty, Entity.SALQ_TYPE, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            gvQuestions.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                            gvQuestions.Rows[rowIndex].Cells["Ques_Desc"].Style.ForeColor = Color.Blue;
                            gvQuestions.Rows[rowIndex].Tag = Entity;
                        }
                        else
                        {
                            SALQLNKEntity SelLinkQuesList = salQlinkEntitylist.Find(u => u.SALQL_Q_ID.Equals(Entity.SALQ_ID));

                            if (SelLinkQuesList != null)
                            {
                                gvQuestions.Rows[rowIndex].Cells["Ques_Desc"].Value = Entity.SALQ_DESC;
                                gvQuestions.Rows[rowIndex].Cells["Responce"].Value = "";
                                gvQuestions.Rows[rowIndex].Cells["Ques_ID"].Value = Entity.SALQ_ID;
                                gvQuestions.Rows[rowIndex].Cells["Ques_SAL_ID"].Value = Entity.SALQ_SALD_ID;
                                gvQuestions.Rows[rowIndex].Cells["Grp_Code"].Value = Entity.SALQ_GRP_CODE;
                                gvQuestions.Rows[rowIndex].Cells["Grp_Seq"].Value = Entity.SALQ_GRP_SEQ;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Code"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Type"].Value = Entity.SALQ_TYPE;

                                gvQuestions.Rows[rowIndex].Cells["ResponceCode"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Link_Ques"].Value = SelLinkQuesList.SALQL_LINKQ;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Req"].Value = SelLinkQuesList.SALQL_REQ;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Enable"].Value = SelLinkQuesList.SALQL_ENABLE;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Disable"].Value = SelLinkQuesList.SALQL_DISABLE;
                                gvQuestions.Rows[rowIndex].Cells["Link_Ques_Resp"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Delete"].Value = string.Empty;

                                // rowIndex = gvQuestions1.Rows.Add(Entity.SALQ_DESC, "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, 
                                //    Entity.SALQ_GRP_SEQ, string.Empty, Entity.SALQ_TYPE, string.Empty, SelLinkQuesList.SALQL_LINKQ, SelLinkQuesList.SALQL_REQ,
                                //   SelLinkQuesList.SALQL_ENABLE, SelLinkQuesList.SALQL_DISABLE, string.Empty);
                            }
                            else
                            {
                                gvQuestions.Rows[rowIndex].Cells["Ques_Desc"].Value = Entity.SALQ_DESC;
                                gvQuestions.Rows[rowIndex].Cells["Responce"].Value = "";
                                gvQuestions.Rows[rowIndex].Cells["Ques_ID"].Value = Entity.SALQ_ID;
                                gvQuestions.Rows[rowIndex].Cells["Ques_SAL_ID"].Value = Entity.SALQ_SALD_ID;
                                gvQuestions.Rows[rowIndex].Cells["Grp_Code"].Value = Entity.SALQ_GRP_CODE;
                                gvQuestions.Rows[rowIndex].Cells["Grp_Seq"].Value = Entity.SALQ_GRP_SEQ;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Code"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Type"].Value = Entity.SALQ_TYPE;

                                gvQuestions.Rows[rowIndex].Cells["ResponceCode"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Link_Ques"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Req"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Enable"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Disable"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Link_Ques_Resp"].Value = string.Empty;
                                gvQuestions.Rows[rowIndex].Cells["Ques_Delete"].Value = string.Empty;

                                //  rowIndex = gvQuestions1.Rows.Add(Entity.SALQ_DESC, "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, Entity.SALQ_GRP_SEQ,
                                //   string.Empty, Entity.SALQ_TYPE, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            }

                            if (Entity.SALQ_TYPE == "X" || Entity.SALQ_TYPE == "1" || Entity.SALQ_TYPE == "2" || Entity.SALQ_TYPE == "3")
                            {
                                DataGridViewTextBoxCell TextBoxCell = new DataGridViewTextBoxCell();
                                this.gvQuestions["Responce", rowIndex] = TextBoxCell;
                                this.gvQuestions["Responce", rowIndex].Value = "";
                                this.gvQuestions["Responce", rowIndex].ToolTipText = "Question Type: Text";
                                TextBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";
                            }

                            if (Entity.SALQ_TYPE == "T")
                            {
                                DataGridViewDateTimePickerCell Response = new DataGridViewDateTimePickerCell();
                                Response.Format = DateTimePickerFormat.Short;
                                Response.Style.BackgroundImageSource = "icon-calendar";
                                Response.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                                this.gvQuestions["Responce", rowIndex] = Response;
                                this.gvQuestions["Responce", rowIndex].Value = string.Empty;
                                this.gvQuestions["Responce", rowIndex].ToolTipText = "Question Type: Date";
                                Response.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";

                            }
                            if (Entity.SALQ_TYPE == "N")
                            {
                                DataGridViewTextBoxCell Response = new DataGridViewTextBoxCell();
                                //Response.HideUpDownButtons = true;
                                this.gvQuestions["Responce", rowIndex] = Response;
                                this.gvQuestions["Responce", rowIndex].Value = string.Empty;
                                this.gvQuestions["Responce", rowIndex].ToolTipText = "Question Type: Numeric";
                                Response.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";
                            }
                            if (Entity.SALQ_TYPE == "D")
                            {
                                DataGridViewComboBoxCell ComboBoxCell = new DataGridViewComboBoxCell();
                                //if (dr.CUSTACTIVECUST.ToUpper() == "A")
                                //    fillgridComboResp(ComboBoxCell, fieldType, custCode, "");

                                ComboBoxCell.Style.BackgroundImageSource = "combo-arrow";
                                ComboBoxCell.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                                ComboBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px; ";

                                this.gvQuestions["Responce", rowIndex] = ComboBoxCell;
                                this.gvQuestions["Responce", rowIndex].ToolTipText = "Question Type: Drop down";

                                List<SalqrespEntity> SelQuesResp = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID.ToString()));
                                if (SelQuesResp.Count > 0)
                                {
                                    ComboBoxCell.DataSource = SelQuesResp;
                                    ComboBoxCell.DisplayMember = "SALQR_DESC";
                                    ComboBoxCell.ValueMember = "SALQR_CODE";
                                }

                            }
                            if (Entity.SALQ_TYPE == "C")
                            {
                                DataGridViewButtonCell Response = new DataGridViewButtonCell();
                                Response.Style.ForeColor = System.Drawing.Color.White;
                                // contextMenu1_Popup(null, Response, fieldType, custCode);
                                this.gvQuestions["Responce", rowIndex] = Response;
                                this.gvQuestions["Responce", rowIndex].Value = string.Empty;
                                this.gvQuestions["Responce", rowIndex].ToolTipText = "Question Type: Check Box";

                            }


                            gvQuestions.Rows[rowIndex].Tag = Entity;
                        }


                        #region SAL
                        if (FormType == "SAL")
                        {
                            if (SALACTList.Count > 0)
                            {
                                foreach (DataGridViewRow gvrows in gvQuestions.Rows)
                                {
                                    if (Entity.SALQ_SEQ != "0" && Entity.SALQ_ID == gvrows.Cells["Ques_ID"].Value.ToString())
                                    {
                                        SALACTEntity MatResponce = SALACTList.Find(u => u.SALACT_Q_ID.Equals(Entity.SALQ_ID) && u.SALACT_SALID.Equals(Entity.SALQ_SALD_ID));
                                        if (MatResponce != null)
                                        {
                                            //gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                            if (gvrows.Cells["Ques_Type"].Value.ToString() == "T")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = LookupDataAccess.Getdate(MatResponce.SALACT_DATE_RESP.Trim());
                                                gvrows.Cells["Responce"].Value = LookupDataAccess.Getdate(MatResponce.SALACT_DATE_RESP.Trim());
                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_DATE_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "N")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_NUM_RESP;
                                                gvrows.Cells["Responce"].Value = MatResponce.SALACT_NUM_RESP;
                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_NUM_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "D")
                                            {
                                                SalqrespEntity selRespEntity = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID) && u.SALQR_CODE.Trim().Equals(MatResponce.SALACT_MULTI_RESP.Trim()));
                                                if (selRespEntity != null)
                                                {
                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = selRespEntity.SALQR_DESC;
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                                }
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "C")
                                            {
                                                string custQuestionResp = string.Empty;
                                                List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID));

                                                if (selRespEntity.Count > 0)
                                                {
                                                    string response1 = MatResponce.SALACT_MULTI_RESP;
                                                    if (!string.IsNullOrEmpty(response1))
                                                    {
                                                        string[] arrResponse = null;
                                                        if (response1.IndexOf(',') > 0)
                                                        {
                                                            arrResponse = response1.Split(',');
                                                        }
                                                        else if (!response1.Equals(string.Empty))
                                                        {
                                                            arrResponse = new string[] { response1 };
                                                        }


                                                        foreach (string stringitem in arrResponse)
                                                        {
                                                            SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                                            if (custRespEntity != null)
                                                            {
                                                                custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                                                //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                            }

                                                        }
                                                    }


                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = custQuestionResp.Trim().TrimEnd(',');
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                }

                                            }
                                            else
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP; gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_MULTI_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }

                                        }
                                        else
                                        {
                                            //string strresp = gvrows.Cells["Responce"].Value == null ? string.Empty : gvrows.Cells["Responce"].Value.ToString();
                                            //if (strresp != string.Empty)
                                            //{
                                            //    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            ////}
                                            //gvrows.Cells["Responce"].Value = string.Empty;
                                            //gvrows.Cells["ResponceCode"].Value = string.Empty;
                                        }
                                    }


                                }
                            }
                        }
                        #endregion

                        #region CAL
                        else if (FormType == "CAL")
                        {
                            if (CALCONTList.Count > 0)
                            {
                                foreach (DataGridViewRow gvrows in gvQuestions.Rows)
                                {
                                    if (Entity.SALQ_SEQ != "0" && Entity.SALQ_ID == gvrows.Cells["Ques_ID"].Value.ToString())
                                    {
                                        CALCONTEntity MatResponce = CALCONTList.Find(u => u.CALCONT_Q_ID.Equals(Entity.SALQ_ID) && u.CALCONT_SALID.Equals(Entity.SALQ_SALD_ID));
                                        if (MatResponce != null)
                                        {
                                            //gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                            if (gvrows.Cells["Ques_Type"].Value.ToString() == "T")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = LookupDataAccess.Getdate(MatResponce.CALCONT_DATE_RESP.Trim());
                                                gvrows.Cells["Responce"].Value = LookupDataAccess.Getdate(MatResponce.CALCONT_DATE_RESP.Trim());

                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_DATE_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "N")
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_NUM_RESP;
                                                gvrows.Cells["Responce"].Value = MatResponce.CALCONT_NUM_RESP;

                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_NUM_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "D")
                                            {
                                                SalqrespEntity selRespEntity = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID) && u.SALQR_CODE.Trim().Equals(MatResponce.CALCONT_MULTI_RESP.Trim()));
                                                if (selRespEntity != null)
                                                {
                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = selRespEntity.SALQR_DESC;
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                                }
                                            }
                                            else if (gvrows.Cells["Ques_Type"].Value.ToString() == "C")
                                            {
                                                string custQuestionResp = string.Empty;
                                                List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID));

                                                if (selRespEntity.Count > 0)
                                                {
                                                    string response1 = MatResponce.CALCONT_MULTI_RESP;
                                                    if (!string.IsNullOrEmpty(response1))
                                                    {
                                                        string[] arrResponse = null;
                                                        if (response1.IndexOf(',') > 0)
                                                        {
                                                            arrResponse = response1.Split(',');
                                                        }
                                                        else if (!response1.Equals(string.Empty))
                                                        {
                                                            arrResponse = new string[] { response1 };
                                                        }


                                                        foreach (string stringitem in arrResponse)
                                                        {
                                                            SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                                            if (custRespEntity != null)
                                                            {
                                                                custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                                                //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                            }

                                                        }
                                                    }


                                                    gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_MULTI_RESP;
                                                    gvrows.Cells["Responce"].Value = custQuestionResp.Trim().TrimEnd(',');
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                }

                                            }
                                            else
                                            {
                                                gvrows.Cells["ResponceCode"].Value = MatResponce.CALCONT_MULTI_RESP; gvrows.Cells["Responce"].Value = MatResponce.CALCONT_MULTI_RESP;
                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_MULTI_RESP.Trim()))
                                                    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            }

                                        }
                                        else
                                        {
                                            //string strresp = gvrows.Cells["Responce"].Value == null ? string.Empty : gvrows.Cells["Responce"].Value.ToString();
                                            //if (strresp != string.Empty)
                                            //{
                                            //    gvrows.Cells["Ques_Delete"].Value = DeleteImage;
                                            ////}
                                            //gvrows.Cells["Responce"].Value = string.Empty;
                                            //gvrows.Cells["ResponceCode"].Value = string.Empty;
                                        }
                                    }


                                }
                            }
                        }
                        #endregion



                    }

                    if (gvQuestions.Rows.Count > 0)
                    {
                        EnabldisableQuestion();
                        gvQuestions.CurrentCell = gvQuestions.Rows[0].Cells[1];

                        gvQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
                    }
                }
            }
        }

        private void EnabldisableQuestion()
        {
            string BlankImage = Consts.Icons.ico_Blank;
            if (gvQuestions.Rows.Count > 0)
            {
                if (FormType == "SAL")
                {
                    if (SALACTList.Count > 0)
                    {
                        string strEnabledata = string.Empty;
                        string[] arrEnabledata = null;
                        string strDisabledata = string.Empty;
                        string[] arrDisabledata = null;
                        string strRequiredata = string.Empty;
                        string[] arrRequiredata = null;
                        string strDimentionQid = string.Empty; bool Istrue = false;
                        foreach (DataGridViewRow gvrows in gvQuestions.Rows)
                        {
                            if (!Istrue)
                                strDimentionQid = gvrows.Cells["Link_Ques"].Value.ToString();
                            if (strDimentionQid != string.Empty)
                                Istrue = true;

                            if (Istrue)
                            {
                                SALQLNKEntity SALLinkEntity = salQlinkEntitylist.Find(u => u.SALQL_LINKQ.Equals(strDimentionQid));
                                SALACTEntity MatResponce = SALACTList.Find(u => u.SALACT_Q_ID.Equals(gvrows.Cells["Ques_ID"].Value.ToString()) && u.SALACT_SALID.Equals(gvrows.Cells["Ques_SAL_ID"].Value.ToString()));

                                if (SALLinkEntity != null && gvrows.Cells["Ques_ID"].Value.ToString() == strDimentionQid)
                                {
                                    strDisabledata = SALLinkEntity.SALQL_DISABLE;
                                    strEnabledata = SALLinkEntity.SALQL_ENABLE;
                                    arrDisabledata = null;
                                    arrEnabledata = null;
                                    if (strDisabledata.IndexOf(',') > 0)
                                    {
                                        arrDisabledata = strDisabledata.Split(',');
                                    }
                                    else if (!strDisabledata.Equals(string.Empty))
                                    {
                                        arrDisabledata = new string[] { strDisabledata };
                                    }
                                    if (strEnabledata.IndexOf(',') > 0)
                                    {
                                        arrEnabledata = strEnabledata.Split(',');
                                    }
                                    else if (!strEnabledata.Equals(string.Empty))
                                    {
                                        arrEnabledata = new string[] { strEnabledata };
                                    }
                                    strRequiredata = SALLinkEntity.SALQL_REQ;
                                    arrRequiredata = null;
                                    if (strRequiredata.IndexOf(',') > 0)
                                    {
                                        arrRequiredata = strRequiredata.Split(',');
                                    }
                                    else if (!strRequiredata.Equals(string.Empty))
                                    {
                                        arrRequiredata = new string[] { strRequiredata };
                                    }


                                    if (MatResponce != null)
                                    {
                                        //gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                        if (gvrows.Cells["Ques_Type"].Value.ToString() == "D")
                                        {
                                            SalqrespEntity selRespEntity = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(gvrows.Cells["Ques_ID"].Value.ToString()) && u.SALQR_CODE.Equals(MatResponce.SALACT_MULTI_RESP));
                                            if (selRespEntity != null)
                                            {
                                                //gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                //gvrows.Cells["Responce"].Value = selRespEntity.SALQR_DESC;
                                                //gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                if (arrDisabledata != null)
                                                {
                                                    if (arrDisabledata.ToList().Exists(u => u.Equals(MatResponce.SALACT_MULTI_RESP)))
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Responce"].Tag = null;
                                                        gvrows.Cells["Responce"].Value = string.Empty;
                                                        gvrows.Cells["ResponceCode"].Tag = null;
                                                        gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                        gvrows.Cells["Ques_Enable"].Tag = "N";
                                                        if (!string.IsNullOrEmpty(MatResponce.SALACT_MULTI_RESP))
                                                            gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                    }

                                                }
                                                if (arrEnabledata != null)
                                                {
                                                    if (arrEnabledata.ToList().Exists(u => u.Equals(MatResponce.SALACT_MULTI_RESP)))
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Ques_Enable"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        if (arrDisabledata != null)
                                                        {
                                                            if (arrDisabledata.ToList().Exists(u => u.Equals(MatResponce.SALACT_MULTI_RESP)))
                                                            {
                                                                gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvrows.Cells["Responce"].Tag = null;
                                                                gvrows.Cells["Responce"].Value = string.Empty;
                                                                gvrows.Cells["ResponceCode"].Tag = null;
                                                                gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                                gvrows.Cells["Ques_Enable"].Tag = "N";
                                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_MULTI_RESP))
                                                                    gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                            }

                                                        }
                                                    }
                                                }
                                                if (arrRequiredata != null)
                                                {
                                                    if (arrRequiredata.ToList().Exists(u => u.Equals(MatResponce.SALACT_MULTI_RESP)))
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "N";
                                                    }
                                                }

                                            }
                                        }
                                        else if (gvrows.Cells["Ques_Type"].Value.ToString() == "C")
                                        {
                                            string custQuestionResp = string.Empty;
                                            List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(gvrows.Cells["Ques_ID"].Value.ToString()));
                                            bool QUESEnable = false, QuesDisable = false, QuesReq = false;
                                            if (selRespEntity.Count > 0)
                                            {
                                                string response1 = MatResponce.SALACT_MULTI_RESP;
                                                if (!string.IsNullOrEmpty(response1))
                                                {
                                                    string[] arrResponse = null;
                                                    if (response1.IndexOf(',') > 0)
                                                    {
                                                        arrResponse = response1.Split(',');
                                                    }
                                                    else if (!response1.Equals(string.Empty))
                                                    {
                                                        arrResponse = new string[] { response1 };
                                                    }


                                                    foreach (string stringitem in arrResponse)
                                                    {
                                                        //SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                                        //if (custRespEntity != null)
                                                        //{
                                                        //    custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                                        //    //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                        //}

                                                        if (arrDisabledata != null)
                                                        {
                                                            if (arrDisabledata.ToList().Exists(u => u.Equals(stringitem.Trim())))
                                                                QuesDisable = true;
                                                        }
                                                        if (arrEnabledata != null)
                                                        {
                                                            if (arrEnabledata.ToList().Exists(u => u.Equals(stringitem.Trim())))
                                                                QUESEnable = true;
                                                        }
                                                        if (arrRequiredata != null)
                                                        {
                                                            if (arrRequiredata.ToList().Exists(u => u.Equals(stringitem.Trim())))
                                                                QuesReq = true;
                                                        }

                                                    }
                                                }


                                                //gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                //gvrows.Cells["Responce"].Value = custQuestionResp;
                                                //gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                if (arrDisabledata != null)
                                                {
                                                    if (QuesDisable)
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Responce"].Tag = null;
                                                        gvrows.Cells["Responce"].Value = string.Empty;
                                                        gvrows.Cells["ResponceCode"].Tag = null;
                                                        gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                        gvrows.Cells["Ques_Enable"].Tag = "N";
                                                        if (!string.IsNullOrEmpty(MatResponce.SALACT_MULTI_RESP))
                                                            gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                    }

                                                }
                                                if (arrEnabledata != null)
                                                {
                                                    if (QUESEnable)
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Ques_Enable"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        if (arrDisabledata != null)
                                                        {
                                                            if (QuesDisable)
                                                            {
                                                                gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvrows.Cells["Responce"].Tag = null;
                                                                gvrows.Cells["Responce"].Value = string.Empty;
                                                                gvrows.Cells["ResponceCode"].Tag = null;
                                                                gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                                gvrows.Cells["Ques_Enable"].Tag = "N";
                                                                if (!string.IsNullOrEmpty(MatResponce.SALACT_MULTI_RESP))
                                                                    gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                            }

                                                        }
                                                    }
                                                }
                                                if (arrRequiredata != null)
                                                {
                                                    if (QuesReq)
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "N";
                                                    }
                                                }
                                            }

                                        }


                                    }

                                    Istrue = false;
                                }
                            }

                        }
                    }
                }
                else if (FormType == "CAL")
                {
                    if (CALCONTList.Count > 0)
                    {
                        string strEnabledata = string.Empty;
                        string[] arrEnabledata = null;
                        string strDisabledata = string.Empty;
                        string[] arrDisabledata = null;
                        string strRequiredata = string.Empty;
                        string[] arrRequiredata = null;
                        string strDimentionQid = string.Empty; bool Istrue = false;
                        foreach (DataGridViewRow gvrows in gvQuestions.Rows)
                        {
                            if (!Istrue)
                                strDimentionQid = gvrows.Cells["Link_Ques"].Value.ToString();
                            if (strDimentionQid != string.Empty)
                                Istrue = true;

                            if (Istrue)
                            {
                                SALQLNKEntity SALLinkEntity = salQlinkEntitylist.Find(u => u.SALQL_LINKQ.Equals(strDimentionQid));
                                CALCONTEntity MatResponce = CALCONTList.Find(u => u.CALCONT_Q_ID.Equals(gvrows.Cells["Ques_ID"].Value.ToString()) && u.CALCONT_SALID.Equals(gvrows.Cells["Ques_SAL_ID"].Value.ToString()));

                                if (SALLinkEntity != null && gvrows.Cells["Ques_ID"].Value.ToString() == strDimentionQid)
                                {
                                    strDisabledata = SALLinkEntity.SALQL_DISABLE;
                                    strEnabledata = SALLinkEntity.SALQL_ENABLE;
                                    arrDisabledata = null;
                                    arrEnabledata = null;
                                    if (strDisabledata.IndexOf(',') > 0)
                                    {
                                        arrDisabledata = strDisabledata.Split(',');
                                    }
                                    else if (!strDisabledata.Equals(string.Empty))
                                    {
                                        arrDisabledata = new string[] { strDisabledata };
                                    }
                                    if (strEnabledata.IndexOf(',') > 0)
                                    {
                                        arrEnabledata = strEnabledata.Split(',');
                                    }
                                    else if (!strEnabledata.Equals(string.Empty))
                                    {
                                        arrEnabledata = new string[] { strEnabledata };
                                    }
                                    strRequiredata = SALLinkEntity.SALQL_REQ;
                                    arrRequiredata = null;
                                    if (strRequiredata.IndexOf(',') > 0)
                                    {
                                        arrRequiredata = strRequiredata.Split(',');
                                    }
                                    else if (!strRequiredata.Equals(string.Empty))
                                    {
                                        arrRequiredata = new string[] { strRequiredata };
                                    }


                                    if (MatResponce != null)
                                    {
                                        //gvrows.Cells["Responce"].Value = MatResponce.SALACT_MULTI_RESP;
                                        if (gvrows.Cells["Ques_Type"].Value.ToString() == "D")
                                        {
                                            SalqrespEntity selRespEntity = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(gvrows.Cells["Ques_ID"].Value.ToString()) && u.SALQR_CODE.Equals(MatResponce.CALCONT_MULTI_RESP));
                                            if (selRespEntity != null)
                                            {
                                                //gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                //gvrows.Cells["Responce"].Value = selRespEntity.SALQR_DESC;
                                                //gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                if (arrDisabledata != null)
                                                {
                                                    if (arrDisabledata.ToList().Exists(u => u.Equals(MatResponce.CALCONT_MULTI_RESP)))
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Responce"].Tag = null;
                                                        gvrows.Cells["Responce"].Value = string.Empty;
                                                        gvrows.Cells["ResponceCode"].Tag = null;
                                                        gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                        gvrows.Cells["Ques_Enable"].Tag = "N";
                                                        if (!string.IsNullOrEmpty(MatResponce.CALCONT_MULTI_RESP))
                                                            gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                    }

                                                }
                                                if (arrEnabledata != null)
                                                {
                                                    if (arrEnabledata.ToList().Exists(u => u.Equals(MatResponce.CALCONT_MULTI_RESP)))
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Ques_Enable"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        if (arrDisabledata != null)
                                                        {
                                                            if (arrDisabledata.ToList().Exists(u => u.Equals(MatResponce.CALCONT_MULTI_RESP)))
                                                            {
                                                                gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvrows.Cells["Responce"].Tag = null;
                                                                gvrows.Cells["Responce"].Value = string.Empty;
                                                                gvrows.Cells["ResponceCode"].Tag = null;
                                                                gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                                gvrows.Cells["Ques_Enable"].Tag = "N";
                                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_MULTI_RESP))
                                                                    gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                            }

                                                        }
                                                    }
                                                }
                                                if (arrRequiredata != null)
                                                {
                                                    if (arrRequiredata.ToList().Exists(u => u.Equals(MatResponce.CALCONT_MULTI_RESP)))
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "N";
                                                    }
                                                }

                                            }
                                        }
                                        else if (gvrows.Cells["Ques_Type"].Value.ToString() == "C")
                                        {
                                            string custQuestionResp = string.Empty;
                                            List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(gvrows.Cells["Ques_ID"].Value.ToString()));
                                            bool QUESEnable = false, QuesDisable = false, QuesReq = false;
                                            if (selRespEntity.Count > 0)
                                            {
                                                string response1 = MatResponce.CALCONT_MULTI_RESP;
                                                if (!string.IsNullOrEmpty(response1))
                                                {
                                                    string[] arrResponse = null;
                                                    if (response1.IndexOf(',') > 0)
                                                    {
                                                        arrResponse = response1.Split(',');
                                                    }
                                                    else if (!response1.Equals(string.Empty))
                                                    {
                                                        arrResponse = new string[] { response1 };
                                                    }


                                                    foreach (string stringitem in arrResponse)
                                                    {
                                                        //SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                                        //if (custRespEntity != null)
                                                        //{
                                                        //    custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                                        //    //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                        //}

                                                        if (arrDisabledata != null)
                                                        {
                                                            if (arrDisabledata.ToList().Exists(u => u.Equals(stringitem.Trim())))
                                                                QuesDisable = true;
                                                        }
                                                        if (arrEnabledata != null)
                                                        {
                                                            if (arrEnabledata.ToList().Exists(u => u.Equals(stringitem.Trim())))
                                                                QUESEnable = true;
                                                        }
                                                        if (arrRequiredata != null)
                                                        {
                                                            if (arrRequiredata.ToList().Exists(u => u.Equals(stringitem.Trim())))
                                                                QuesReq = true;
                                                        }

                                                    }
                                                }


                                                //gvrows.Cells["ResponceCode"].Value = MatResponce.SALACT_MULTI_RESP;
                                                //gvrows.Cells["Responce"].Value = custQuestionResp;
                                                //gvrows.Cells["Ques_Delete"].Value = DeleteImage;

                                                if (arrDisabledata != null)
                                                {
                                                    if (QuesDisable)
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Responce"].Tag = null;
                                                        gvrows.Cells["Responce"].Value = string.Empty;
                                                        gvrows.Cells["ResponceCode"].Tag = null;
                                                        gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                        gvrows.Cells["Ques_Enable"].Tag = "N";
                                                        if (!string.IsNullOrEmpty(MatResponce.CALCONT_MULTI_RESP))
                                                            gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                    }

                                                }
                                                if (arrEnabledata != null)
                                                {
                                                    if (QUESEnable)
                                                    {
                                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                        gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                        gvrows.Cells["Ques_Enable"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        if (arrDisabledata != null)
                                                        {
                                                            if (QuesDisable)
                                                            {
                                                                gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvrows.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvrows.Cells["Responce"].Tag = null;
                                                                gvrows.Cells["Responce"].Value = string.Empty;
                                                                gvrows.Cells["ResponceCode"].Tag = null;
                                                                gvrows.Cells["ResponceCode"].Value = string.Empty;
                                                                gvrows.Cells["Ques_Enable"].Tag = "N";
                                                                if (!string.IsNullOrEmpty(MatResponce.CALCONT_MULTI_RESP))
                                                                    gvrows.Cells["Ques_Delete"].Value = BlankImage;
                                                            }

                                                        }
                                                    }
                                                }
                                                if (arrRequiredata != null)
                                                {
                                                    if (QuesReq)
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "Y";
                                                    }
                                                    else
                                                    {
                                                        gvrows.Cells["Ques_Req"].Tag = "N";
                                                    }
                                                }
                                            }

                                        }


                                    }

                                    Istrue = false;
                                }
                            }

                        }
                    }

                }

            }
        }

        bool Isdelete = false;
        private void gvQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex == 14 && e.RowIndex != -1)
            {
                int introwindex = e.RowIndex;
                if (GetSelectedRow() != null)
                {
                    if (gvQuestions.Rows[introwindex].Cells["Responce"].Value != string.Empty)
                    {
                        string BlankImage = Consts.Icons.ico_Blank;
                        gvQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
                        gvQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
                        gvQuestions.Rows[introwindex].Cells["Responcecode"].Value = string.Empty;
                        gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;

                        if (FormType == "SAL")
                        {
                            SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                            if (ActEntity != null)
                                SALACTList.Remove(ActEntity);

                            if (SALACTList.Count == 0) Isdelete = true;
                        }
                        else if (FormType == "CAL")
                        {
                            CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                            if (ActEntity != null)
                                CALCONTList.Remove(ActEntity);

                            if (CALCONTList.Count == 0) Isdelete = true;
                        }

                        gvQuestions.Rows[introwindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

                        gvQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
                    }
                    // MessageBox.Show("Are you sure want clear Responce", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandler, true);
                }
            }
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].CellRenderer == "ButtonCell" && e.RowIndex != -1)
                {
                    string response = gvQuestions.Rows[e.RowIndex].Cells[8].Value != null ? gvQuestions.Rows[e.RowIndex].Cells[8].Value.ToString() : string.Empty;

                    string QID = gvQuestions.Rows[e.RowIndex].Cells["Ques_ID"].Value.ToString();
                    string QDesc = gvQuestions.Rows[e.RowIndex].Cells["Ques_Desc"].Value.ToString();

                    // string response = gvQuestions.Rows[introwindex].Cells[8].Value != null ? gvQuestions.Rows[introwindex].Cells[8].Value.ToString() : string.Empty;
                    PrivilegeEntity privileges = new PrivilegeEntity();
                    privileges.AddPriv = "true";
                    //AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, dr.Cells["Ques_ID"].Value.ToString(), dr.Cells["Ques_Desc"].Value.ToString(), string.Empty);
                    AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, QID, QDesc, string.Empty);
                    objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                    objform.StartPosition = FormStartPosition.CenterScreen;
                    objform.ShowDialog();
                }

            }
        }

        int strIndex = 0;

        public string GetSelectedRow()
        {
            string QuestionCode = null;
            if (gvQuestions != null)
            {
                foreach (DataGridViewRow dr in gvQuestions.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        strIndex = gvQuestions.SelectedRows[0].Index;
                        GroupCode = gvQuestions.SelectedRows[0].Cells["Grp_Code"].Value.ToString();
                        QuestionCode = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();

                    }
                }
            }
            return QuestionCode;
        }


        private void gvQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gvQuestions.Rows.Count > 0)
            {
                gvQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
                string strQuesttype = gvQuestions.Rows[e.RowIndex].Cells["Ques_Type"].Value.ToString();
                //if (gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && (strQuesttype.Equals("5") || strQuesttype.Equals("6")))
                //{
                //    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                //    {
                //        gvQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
                //        gvQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
                //        gvQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
                //        MessageBox.Show(Consts.Messages.PleaseEnterNumbers);
                //    }
                //}
                if (gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("T"))
                {
                    int intcolindex = gvQuestions.CurrentCell.ColumnIndex;
                    int introwindex = gvQuestions.CurrentCell.RowIndex;
                    string strCurrectValue = Convert.ToString(gvQuestions.Rows[introwindex].Cells[intcolindex].Value);

                    if (strCurrectValue != "")
                        strCurrectValue = Convert.ToDateTime(strCurrectValue).ToString("MM/dd/yyyy");


                    //if (Convert.ToDateTime(strCurrectValue) > DateTime.Now)
                    //{
                    //    AlertBox.Show("Future date not allowed", MessageBoxIcon.Warning);

                    //}
                    //if (Convert.ToDateTime(strCurrectValue).ToShortDateString() == DateTime.Now.ToShortDateString())
                    //{
                    //    AlertBox.Show("DOB should not be current date", MessageBoxIcon.Warning);

                    //}

                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                    {
                        gvQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
                        AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (FormType == "SAL")
                        {
                            SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                            if (ActEntity != null)
                            {
                                ActEntity.SALACT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                ActEntity.SALACT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                ActEntity.SALACT_TYPE = "S";
                                ActEntity.SALACT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                ActEntity.SALACT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    ActEntity.SALACT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    ActEntity.SALACT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else
                                    ActEntity.SALACT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                                ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                                //SALACTList.Add(new SALACTEntity(ActEntity));
                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                            }
                            else
                            {

                                SALACTEntity Search_Act_entity = new SALACTEntity();
                                Search_Act_entity.SALACT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                Search_Act_entity.SALACT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                Search_Act_entity.SALACT_TYPE = "S";
                                Search_Act_entity.SALACT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                Search_Act_entity.SALACT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    Search_Act_entity.SALACT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    Search_Act_entity.SALACT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else
                                    Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                                Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                                SALACTList.Add(new SALACTEntity(Search_Act_entity));

                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;

                            }
                        }
                        else if (FormType == "CAL")
                        {
                            CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                            if (ActEntity != null)
                            {

                                ActEntity.CALCONT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                ActEntity.CALCONT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                ActEntity.CALCONT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                ActEntity.CALCONT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    ActEntity.CALCONT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    ActEntity.CALCONT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else
                                    ActEntity.CALCONT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                ActEntity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                                ActEntity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                                //SALACTList.Add(new SALACTEntity(ActEntity));

                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                            }
                            else
                            {
                                CALCONTEntity Search_Act_entity = new CALCONTEntity();
                                Search_Act_entity.CALCONT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                Search_Act_entity.CALCONT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                Search_Act_entity.CALCONT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                Search_Act_entity.CALCONT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    Search_Act_entity.CALCONT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    Search_Act_entity.CALCONT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else
                                    Search_Act_entity.CALCONT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                Search_Act_entity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                                Search_Act_entity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                                CALCONTList.Add(new CALCONTEntity(Search_Act_entity));

                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                            }
                        }
                    }
                }
                if (gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("N"))
                {
                    int intcolindex = gvQuestions.CurrentCell.ColumnIndex;
                    int introwindex = gvQuestions.CurrentCell.RowIndex;
                    string strCurrectValue = Convert.ToString(gvQuestions.Rows[introwindex].Cells[intcolindex].Value);

                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                    {
                        gvQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
                        AlertBox.Show(Consts.Messages.PleaseEnterNumbers, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (FormType == "SAL")
                        {
                            SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                            if (ActEntity != null)
                            {
                                ActEntity.SALACT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                ActEntity.SALACT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                ActEntity.SALACT_TYPE = "S";
                                ActEntity.SALACT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                ActEntity.SALACT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    ActEntity.SALACT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    ActEntity.SALACT_NUM_RESP = (gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString());
                                else
                                    ActEntity.SALACT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                                ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                                //SALACTList.Add(new SALACTEntity(ActEntity));
                            }
                            else
                            {

                                SALACTEntity Search_Act_entity = new SALACTEntity();
                                Search_Act_entity.SALACT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                Search_Act_entity.SALACT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                Search_Act_entity.SALACT_TYPE = "S";
                                Search_Act_entity.SALACT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                Search_Act_entity.SALACT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    Search_Act_entity.SALACT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    Search_Act_entity.SALACT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else
                                    Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                                Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                                SALACTList.Add(new SALACTEntity(Search_Act_entity));

                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;

                            }
                        }
                        else if (FormType == "CAL")
                        {
                            CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                            if (ActEntity != null)
                            {

                                ActEntity.CALCONT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                ActEntity.CALCONT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                ActEntity.CALCONT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                ActEntity.CALCONT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    ActEntity.CALCONT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    ActEntity.CALCONT_NUM_RESP = (gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString());
                                else
                                    ActEntity.CALCONT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                ActEntity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                                ActEntity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                                //SALACTList.Add(new SALACTEntity(ActEntity));

                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                            }
                            else
                            {
                                CALCONTEntity Search_Act_entity = new CALCONTEntity();
                                Search_Act_entity.CALCONT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                                Search_Act_entity.CALCONT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                                Search_Act_entity.CALCONT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                                Search_Act_entity.CALCONT_SEQ = "1";
                                if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                    Search_Act_entity.CALCONT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                    Search_Act_entity.CALCONT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                                else
                                    Search_Act_entity.CALCONT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                                Search_Act_entity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                                Search_Act_entity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                                CALCONTList.Add(new CALCONTEntity(Search_Act_entity));

                                if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                                else
                                    gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                            }
                        }
                    }
                }
                if (gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("X") || gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("D")
                    || gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("1") || gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("2")
                    || gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("3")
                    )
                {
                    int intcolindex = gvQuestions.CurrentCell.ColumnIndex;
                    int introwindex = gvQuestions.CurrentCell.RowIndex;
                    string strCurrectValue = Convert.ToString(gvQuestions.Rows[introwindex].Cells[intcolindex].Value);

                    if (FormType == "SAL")
                    {
                        SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                        if (ActEntity != null)
                        {
                            ActEntity.SALACT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                            ActEntity.SALACT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                            ActEntity.SALACT_TYPE = "S";
                            ActEntity.SALACT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                            ActEntity.SALACT_SEQ = "1";
                            if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                ActEntity.SALACT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                ActEntity.SALACT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else
                                ActEntity.SALACT_MULTI_RESP = (gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString());

                            ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                            ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                            //SALACTList.Add(new SALACTEntity(ActEntity));
                            if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                            else
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                        }
                        else
                        {

                            SALACTEntity Search_Act_entity = new SALACTEntity();
                            Search_Act_entity.SALACT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                            Search_Act_entity.SALACT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                            Search_Act_entity.SALACT_TYPE = "S";
                            Search_Act_entity.SALACT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                            Search_Act_entity.SALACT_SEQ = "1";
                            if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                Search_Act_entity.SALACT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                Search_Act_entity.SALACT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else
                                Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                            Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                            Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                            SALACTList.Add(new SALACTEntity(Search_Act_entity));

                            if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                            else
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;

                        }
                    }
                    else if (FormType == "CAL")
                    {
                        CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString()));
                        if (ActEntity != null)
                        {

                            ActEntity.CALCONT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                            ActEntity.CALCONT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                            ActEntity.CALCONT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                            ActEntity.CALCONT_SEQ = "1";
                            if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                ActEntity.CALCONT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                ActEntity.CALCONT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else
                                ActEntity.CALCONT_MULTI_RESP = (gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim());

                            ActEntity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                            ActEntity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                            //SALACTList.Add(new SALACTEntity(ActEntity));
                            if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value == null ? "" : gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                            else
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                        }
                        else
                        {
                            CALCONTEntity Search_Act_entity = new CALCONTEntity();
                            Search_Act_entity.CALCONT_SALID = gvQuestions.Rows[introwindex].Cells["Ques_SAL_ID"].Value.ToString();
                            Search_Act_entity.CALCONT_Q_ID = gvQuestions.Rows[introwindex].Cells["Ques_ID"].Value.ToString();
                            Search_Act_entity.CALCONT_Q_TYPE = gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString();
                            Search_Act_entity.CALCONT_SEQ = "1";
                            if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "T")
                                Search_Act_entity.CALCONT_DATE_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else if (gvQuestions.Rows[introwindex].Cells["Ques_Type"].Value.ToString() == "N")
                                Search_Act_entity.CALCONT_NUM_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();
                            else
                                Search_Act_entity.CALCONT_MULTI_RESP = gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString();

                            Search_Act_entity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                            Search_Act_entity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                            CALCONTList.Add(new CALCONTEntity(Search_Act_entity));

                            if (!string.IsNullOrEmpty(gvQuestions.Rows[introwindex].Cells["Responce"].Value.ToString().Trim()))
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = DeleteImage;
                            else
                                gvQuestions.Rows[introwindex].Cells["Ques_Delete"].Value = BlankImage;
                        }
                    }

                }

                #region Enable/Disable dependency Questions
                /*
                int _intcolindex = gvQuestions.CurrentCell.ColumnIndex;
                int _introwindex = gvQuestions.CurrentCell.RowIndex;
                string strDimentionQid = gvQuestions.Rows[_introwindex].Cells["Link_Ques"].Value.ToString();
                string selectedCode = gvQuestions.Rows[_introwindex].Cells["Responce"].Value.ToString(); //dr.SALQR_CODE.ToString();
                if (strDimentionQid != string.Empty)
                {
                    string strEnabledata = string.Empty;
                    string[] arrEnabledata = null;
                    string strDisabledata = string.Empty;
                    string[] arrDisabledata = null;
                    string strRequiredata = string.Empty;
                    string[] arrRequiredata = null;

                    foreach (DataGridViewRow item in gvQuestions.Rows)
                    {
                        if (item.Cells["Ques_ID"].Value.ToString().Trim() == strDimentionQid)
                        {
                            SALQLNKEntity preassesdimentdata = salQlinkEntitylist.Find(u => u.SALQL_LINKQ == strDimentionQid);
                            if (preassesdimentdata != null)
                            {
                                strDisabledata = preassesdimentdata.SALQL_DISABLE;
                                strEnabledata = preassesdimentdata.SALQL_ENABLE;
                                arrDisabledata = null;
                                arrEnabledata = null;
                                if (strDisabledata.IndexOf(',') > 0)
                                {
                                    arrDisabledata = strDisabledata.Split(',');
                                }
                                else if (!strDisabledata.Equals(string.Empty))
                                {
                                    arrDisabledata = new string[] { strDisabledata };
                                }
                                if (strEnabledata.IndexOf(',') > 0)
                                {
                                    arrEnabledata = strEnabledata.Split(',');
                                }
                                else if (!strEnabledata.Equals(string.Empty))
                                {
                                    arrEnabledata = new string[] { strEnabledata };
                                }
                                strRequiredata = preassesdimentdata.SALQL_REQ;
                                arrRequiredata = null;
                                if (strRequiredata.IndexOf(',') > 0)
                                {
                                    arrRequiredata = strRequiredata.Split(',');
                                }
                                else if (!strRequiredata.Equals(string.Empty))
                                {
                                    arrRequiredata = new string[] { strRequiredata };
                                }

                                //item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                if (arrDisabledata != null)
                                {
                                    if (arrDisabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                        item.Cells["Responce"].Tag = null;
                                        item.Cells["Responce"].Value = string.Empty;
                                        item.Cells["ResponceCode"].Tag = null;
                                        item.Cells["ResponceCode"].Value = string.Empty;
                                        item.Cells["Ques_Enable"].Tag = "N";
                                        if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                            item.Cells["Ques_Delete"].Value = BlankImage;
                                    }

                                }
                                if (arrEnabledata != null)
                                {
                                    if (arrEnabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                        item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                        item.Cells["Ques_Enable"].Tag = "Y";
                                    }
                                    else
                                    {
                                        if (arrDisabledata != null)
                                        {
                                            if (arrDisabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                            {
                                                item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                item.Cells["Responce"].Tag = null;
                                                item.Cells["Responce"].Value = string.Empty;
                                                item.Cells["ResponceCode"].Tag = null;
                                                item.Cells["ResponceCode"].Value = string.Empty;
                                                item.Cells["Ques_Enable"].Tag = "N";
                                                if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                                    item.Cells["Ques_Delete"].Value = BlankImage;
                                            }

                                        }
                                    }
                                }
                                if (arrRequiredata != null)
                                {
                                    if (arrRequiredata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                    {
                                        item.Cells["Ques_Req"].Tag = "Y";
                                    }
                                    else
                                    {
                                        item.Cells["Ques_Req"].Tag = "N";
                                    }
                                }

                            }
                        }
                    }

                }
                */
                #endregion

                gvQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
            }
        }

        //private void cellchanged()
        //{
        //    if (gvQuestions.Rows.Count > 0)
        //    {
        //        // gvQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);

        //        string strQuesttype = gvQuestions.Rows[e.RowIndex].Cells["Ques_Type"].Value.ToString();
        //        //if (gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && (strQuesttype.Equals("5") || strQuesttype.Equals("6")))
        //        //{
        //        //    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
        //        //    {
        //        //        gvQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
        //        //        gvQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
        //        //        gvQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
        //        //        MessageBox.Show(Consts.Messages.PleaseEnterNumbers);
        //        //    }
        //        //}
        //        if (gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("T"))
        //        {
        //            int intcolindex = gvQuestions.CurrentCell.ColumnIndex;
        //            int introwindex = gvQuestions.CurrentCell.RowIndex;
        //            string strCurrectValue = Convert.ToString(gvQuestions.Rows[introwindex].Cells[intcolindex].Value);

        //            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
        //            {
        //                gvQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;

        //                MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat);
        //            }
        //            else
        //            {
        //                SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
        //                if (ActEntity != null)
        //                {
        //                    ActEntity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
        //                    ActEntity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
        //                    ActEntity.SALACT_TYPE = "S";
        //                    ActEntity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
        //                    ActEntity.SALACT_SEQ = "1";
        //                    if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
        //                        ActEntity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
        //                        ActEntity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else
        //                        ActEntity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();

        //                    ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
        //                    ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

        //                    //SALACTList.Add(new SALACTEntity(ActEntity));
        //                }
        //                else
        //                {

        //                    SALACTEntity Search_Act_entity = new SALACTEntity();
        //                    Search_Act_entity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
        //                    Search_Act_entity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
        //                    Search_Act_entity.SALACT_TYPE = "S";
        //                    Search_Act_entity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
        //                    Search_Act_entity.SALACT_SEQ = "1";
        //                    if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
        //                        Search_Act_entity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
        //                        Search_Act_entity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else
        //                        Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();

        //                    Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
        //                    Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

        //                    SALACTList.Add(new SALACTEntity(Search_Act_entity));

        //                }
        //            }
        //        }
        //        if (gvQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("N"))
        //        {
        //            int intcolindex = gvQuestions.CurrentCell.ColumnIndex;
        //            int introwindex = gvQuestions.CurrentCell.RowIndex;
        //            string strCurrectValue = Convert.ToString(gvQuestions.Rows[introwindex].Cells[intcolindex].Value);

        //            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
        //            {
        //                gvQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
        //                MessageBox.Show(Consts.Messages.PleaseEnterNumbers);
        //            }
        //            else
        //            {
        //                SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
        //                if (ActEntity != null)
        //                {
        //                    ActEntity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
        //                    ActEntity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
        //                    ActEntity.SALACT_TYPE = "S";
        //                    ActEntity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
        //                    ActEntity.SALACT_SEQ = "1";
        //                    if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
        //                        ActEntity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
        //                        ActEntity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else
        //                        ActEntity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();

        //                    ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
        //                    ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

        //                    //SALACTList.Add(new SALACTEntity(ActEntity));
        //                }
        //                else
        //                {

        //                    SALACTEntity Search_Act_entity = new SALACTEntity();
        //                    Search_Act_entity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
        //                    Search_Act_entity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
        //                    Search_Act_entity.SALACT_TYPE = "S";
        //                    Search_Act_entity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
        //                    Search_Act_entity.SALACT_SEQ = "1";
        //                    if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
        //                        Search_Act_entity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
        //                        Search_Act_entity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();
        //                    else
        //                        Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString();

        //                    Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
        //                    Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

        //                    SALACTList.Add(new SALACTEntity(Search_Act_entity));

        //                }
        //            }
        //        }
        //        //gvQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvQuestions_CellValueChanged);
        //    }
        //}


        private void gvSALNames_SelectionChanged(object sender, EventArgs e)
        {
            if (gvSALNames.Rows.Count > 0)
            {
                if (gvSALNames.SelectedRows[0].Cells["gvSign"].Value.ToString() == "Y") btnSign.Visible = true; else btnSign.Visible = false;

                //FillgvQuestions(gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString());
                FillgvQuestions1(gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString());
                if (FormType == "SAL")
                    fillControl(gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString());
            }
        }

        private void fillControl(string SALID)
        {
            if (SALACTList.Count > 0)
            {
                SALACTEntity selAct = SALACTList.Find(u => u.SALACT_ID.Equals(Pass_CA_Entity.ACT_ID) && u.SALACT_SALID.Equals(SALID) && u.SALACT_Q_ID.Equals("0"));
                if (selAct != null)
                {
                    this.cmbStatus.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
                    this.cmbLocation.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
                    this.cmbReceipent.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
                    this.cmbAttn.SelectedIndexChanged -= new System.EventHandler(this.cmbAttn_SelectedIndexChanged);

                    CommonFunctions.SetComboBoxValue(cmbStatus, selAct.SALACT_STATUS);
                    CommonFunctions.SetComboBoxValue(cmbLocation, selAct.SALACT_LOCATION);
                    CommonFunctions.SetComboBoxValue(cmbReceipent, selAct.SALACT_RECIPIENT);
                    CommonFunctions.SetComboBoxValue(cmbAttn, selAct.SALACT_ATTN);

                    this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
                    this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
                    this.cmbReceipent.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
                    this.cmbAttn.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);

                    if (!string.IsNullOrEmpty(selAct.SALACT_TIME_SPENT))
                    {
                        DT_Duration.Text = selAct.SALACT_TIME_SPENT;
                        DT_Duration.Checked = true;
                    }

                    if (!string.IsNullOrEmpty(selAct.SALACT_TIME_IN))
                    {
                        DT_Dur_From.Text = selAct.SALACT_TIME_IN;
                        DT_Dur_From.Checked = true;
                    }

                    if (!string.IsNullOrEmpty(selAct.SALACT_TIME_OUT))
                    {
                        DT_Dur_To.Text = selAct.SALACT_TIME_OUT;
                        DT_Dur_To.Checked = true;
                    }
                }
                else
                {
                    CommonFunctions.SetComboBoxValue(cmbStatus, "0");
                    CommonFunctions.SetComboBoxValue(cmbLocation, "0");
                    CommonFunctions.SetComboBoxValue(cmbReceipent, "0");
                    CommonFunctions.SetComboBoxValue(cmbAttn, "0");
                    DT_Dur_To.Text = "12:00:00 AM";
                    DT_Dur_To.Checked = false;
                    DT_Dur_From.Text = "12:00:00 AM";
                    DT_Dur_From.Checked = false;
                    DT_Duration.Text = "12:00:00 AM";
                    DT_Duration.Checked = false;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if (gvQuestions.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvQuestions.SelectedRows)
                {
                    //Ques_Type
                    if (dr.Selected)
                    {
                        string strFieldEnable = dr.Cells["Ques_Enable"].Tag != null ? dr.Cells["Ques_Enable"].Tag.ToString() : string.Empty;
                        if (strFieldEnable != "N")
                        {
                            if (dr.Cells["Ques_Type"].Value.ToString() == "T")
                            {
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = "Please enter Date here";
                                menuItem.Tag = "T";
                                contextMenu1.MenuItems.Add(menuItem);
                                dr.Cells["Responce"].ReadOnly = false;
                            }
                            if (dr.Cells["Ques_Type"].Value.ToString() == "N")
                            {
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = "Please enter Numbers";
                                menuItem.Tag = "N";
                                contextMenu1.MenuItems.Add(menuItem);
                                dr.Cells["Responce"].ReadOnly = false;
                            }
                            else if (dr.Cells["Ques_Type"].Value.ToString() == "X")
                            {
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = "Please enter text here";
                                menuItem.Tag = "X";
                                contextMenu1.MenuItems.Add(menuItem);
                                dr.Cells["Responce"].ReadOnly = false;
                            }
                            else if (dr.Cells["Ques_Type"].Value.ToString() == "1" || dr.Cells["Ques_Type"].Value.ToString() == "2" || dr.Cells["Ques_Type"].Value.ToString() == "3")
                            {
                                MenuItem menuItem = new MenuItem();
                                if (string.IsNullOrEmpty(dr.Cells["ResponceCode"].Value.ToString().Trim()))
                                    menuItem.Text = "Add";
                                else menuItem.Text = "Edit";
                                menuItem.Tag = "1";
                                contextMenu1.MenuItems.Add(menuItem);
                                //dr.Cells["Responce"].ReadOnly = false;

                            }
                            else if (dr.Cells["Ques_Type"].Value.ToString() == "D")
                            {
                                if (SALQUESRespEntity.Count > 0)
                                {
                                    List<SalqrespEntity> SelQuesResp = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(dr.Cells["Ques_ID"].Value.ToString()));
                                    if (SelQuesResp.Count > 0)
                                    {
                                        foreach (SalqrespEntity Entity in SelQuesResp)
                                        {
                                            MenuItem menuItem = new MenuItem();
                                            menuItem.Text = Entity.SALQR_DESC.ToString().Trim();
                                            menuItem.Tag = Entity;
                                            contextMenu1.MenuItems.Add(menuItem);
                                        }
                                    }
                                }
                            }
                            else if (dr.Cells["Ques_Type"].Value.ToString() == "C")
                            {
                                string response = gvQuestions.SelectedRows[0].Cells[8].Value != null ? gvQuestions.SelectedRows[0].Cells[8].Value.ToString() : string.Empty;
                                //gvQuestions.SelectedRows[0].Cells[8].Tag != null ? gvQuestions.SelectedRows[0].Cells[8].Tag.ToString() : string.Empty;
                                PrivilegeEntity privileges = new PrivilegeEntity();
                                privileges.AddPriv = "true";
                                AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, dr.Cells["Ques_ID"].Value.ToString(), dr.Cells["Ques_Desc"].Value.ToString(), string.Empty);
                                objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                                objform.StartPosition = FormStartPosition.CenterScreen;
                                objform.ShowDialog();
                            }
                        }
                    }
                }
                contextMenu1.Update();
            }
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            //AlertCodeForm form = sender as AlertCodeForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    gvwCustomQuestions.SelectedRows[0].Cells[3].Tag = form.propAlertCode;
            //    gvwCustomQuestions.SelectedRows[0].Cells[3].Value = form.propAlertCode;
            //}
            // //  txtAlertCodes.Text = form.propAlertCode;

            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string BlankImage = Consts.Icons.ico_Blank;
                gvQuestions.SelectedRows[0].Cells[1].Tag = form.propAlertCode;
                gvQuestions.SelectedRows[0].Cells[8].Tag = form.propAlertCode;

                string custQuestionResp = string.Empty;
                SalqrespEntity Search_entity = new SalqrespEntity(true);
                Search_entity.SALQR_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();

                List<SalqrespEntity> SALResponseEntity = _model.SALDEFData.Browse_SALQRESP(Search_entity, "Browse");
                if (SALResponseEntity.Count > 0)
                {
                    string response1 = form.propAlertCode;
                    if (!string.IsNullOrEmpty(response1))
                    {
                        string[] arrResponse = null;
                        if (response1.IndexOf(',') > 0)
                        {
                            arrResponse = response1.Split(',');
                        }
                        else if (!response1.Equals(string.Empty))
                        {
                            arrResponse = new string[] { response1 };
                        }
                        foreach (string stringitem in arrResponse)
                        {

                            SalqrespEntity custRespEntity = SALResponseEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                            if (custRespEntity != null)
                            {
                                custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                            }
                        }
                    }
                }

                if (custQuestionResp.Length > 1)
                {
                    custQuestionResp = custQuestionResp.Trim();
                    if ((custQuestionResp.Substring(custQuestionResp.Length - 1)) == ",")
                    {
                        custQuestionResp = custQuestionResp.Remove(custQuestionResp.Length - 1, 1);
                    }
                    gvQuestions.SelectedRows[0].Cells["Ques_Delete"].Value = DeleteImage;
                }
                gvQuestions.SelectedRows[0].Cells[1].Value = custQuestionResp;
                gvQuestions.SelectedRows[0].Cells[8].Value = form.propAlertCode;

                string selectedCode = form.propAlertCode;
                if (FormType == "SAL")
                {
                    SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                    if (ActEntity != null)
                    {

                        ActEntity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        ActEntity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        ActEntity.SALACT_TYPE = "S";
                        ActEntity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        ActEntity.SALACT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            ActEntity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            ActEntity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            ActEntity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                        ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                        //SALACTList.Add(new SALACTEntity(ActEntity));
                    }
                    else
                    {
                        SALACTEntity Search_Act_entity = new SALACTEntity();
                        Search_Act_entity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        Search_Act_entity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        Search_Act_entity.SALACT_TYPE = "S";
                        Search_Act_entity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        Search_Act_entity.SALACT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            Search_Act_entity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            Search_Act_entity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                        Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                        SALACTList.Add(new SALACTEntity(Search_Act_entity));
                    }
                }
                else if (FormType == "CAL")
                {
                    CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                    if (ActEntity != null)
                    {

                        ActEntity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        ActEntity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        ActEntity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        ActEntity.CALCONT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            ActEntity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            ActEntity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            ActEntity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        ActEntity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                        ActEntity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                        //SALACTList.Add(new SALACTEntity(ActEntity));
                    }
                    else
                    {
                        CALCONTEntity Search_Act_entity = new CALCONTEntity();
                        Search_Act_entity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        Search_Act_entity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        Search_Act_entity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        Search_Act_entity.CALCONT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            Search_Act_entity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            Search_Act_entity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            Search_Act_entity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        Search_Act_entity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                        Search_Act_entity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                        CALCONTList.Add(new CALCONTEntity(Search_Act_entity));
                    }
                }

                string strDimentionQid = gvQuestions.SelectedRows[0].Cells["Link_Ques"].Value.ToString();
                if (strDimentionQid != string.Empty)
                {
                    string strEnabledata = string.Empty;
                    string[] arrEnabledata = null;
                    string strDisabledata = string.Empty;
                    string[] arrDisabledata = null;
                    string strRequiredata = string.Empty;
                    string[] arrRequiredata = null;
                    foreach (DataGridViewRow item in gvQuestions.Rows)
                    {
                        if (item.Cells["Ques_ID"].Value.ToString().Trim() == strDimentionQid)
                        {
                            SALQLNKEntity preassesdimentdata = salQlinkEntitylist.Find(u => u.SALQL_LINKQ == strDimentionQid);
                            if (preassesdimentdata != null)
                            {
                                strDisabledata = preassesdimentdata.SALQL_DISABLE.Trim();
                                strEnabledata = preassesdimentdata.SALQL_ENABLE.Trim();
                                arrDisabledata = null;
                                arrEnabledata = null;
                                if (strDisabledata.IndexOf(',') > 0)
                                {
                                    arrDisabledata = strDisabledata.Split(',');
                                }
                                else if (!strDisabledata.Equals(string.Empty))
                                {
                                    arrDisabledata = new string[] { strDisabledata };
                                }
                                if (strEnabledata.IndexOf(',') > 0)
                                {
                                    arrEnabledata = strEnabledata.Split(',');
                                }
                                else if (!strEnabledata.Equals(string.Empty))
                                {
                                    arrEnabledata = new string[] { strEnabledata };
                                }
                                strRequiredata = preassesdimentdata.SALQL_REQ;
                                arrRequiredata = null;
                                if (strRequiredata.IndexOf(',') > 0)
                                {
                                    arrRequiredata = strRequiredata.Split(',');
                                }
                                else if (!strRequiredata.Equals(string.Empty))
                                {
                                    arrRequiredata = new string[] { strRequiredata };
                                }

                                //item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;

                                if (!string.IsNullOrEmpty(selectedCode))
                                {
                                    string[] arrResponse = null;
                                    if (selectedCode.IndexOf(',') > 0)
                                    {
                                        arrResponse = selectedCode.Split(',');
                                    }
                                    else if (!selectedCode.Equals(string.Empty))
                                    {
                                        arrResponse = new string[] { selectedCode };
                                    }
                                    bool QuesEnable = false, QuesDisable = false, QuesReq = false;
                                    foreach (string stringitem in arrResponse)
                                    {
                                        if (arrDisabledata != null)
                                        {
                                            if (arrDisabledata.ToList().Exists(u => u.Equals(stringitem)))
                                                QuesDisable = true;
                                        }

                                        if (arrEnabledata != null)
                                        {
                                            if (arrEnabledata.ToList().Exists(u => u.Equals(stringitem)))
                                                QuesEnable = true;
                                        }
                                        if (arrRequiredata != null)
                                        {
                                            if (arrRequiredata.ToList().Exists(u => u.Equals(stringitem)))
                                                QuesReq = true;
                                        }
                                    }

                                    if (arrDisabledata != null)
                                    {
                                        if (QuesDisable)
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                            item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                            item.Cells["Responce"].Tag = null;
                                            item.Cells["Responce"].Value = string.Empty;
                                            item.Cells["Ques_Enable"].Tag = "N";
                                            if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                                item.Cells["Ques_Delete"].Value = BlankImage;
                                        }

                                    }
                                    if (arrEnabledata != null)
                                    {
                                        if (QuesEnable)
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                            item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                            item.Cells["Ques_Enable"].Tag = "Y";
                                        }
                                        else
                                        {
                                            if (arrDisabledata != null)
                                            {
                                                if (QuesDisable)
                                                {
                                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                    item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                    item.Cells["Responce"].Tag = null;
                                                    item.Cells["Responce"].Value = string.Empty;
                                                    item.Cells["Ques_Enable"].Tag = "N";
                                                    if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                                        item.Cells["Ques_Delete"].Value = BlankImage;
                                                }

                                            }
                                        }
                                    }
                                    if (arrRequiredata != null)
                                    {
                                        if (QuesReq)
                                        {
                                            item.Cells["Ques_Req"].Tag = "Y";
                                        }
                                        else
                                        {
                                            item.Cells["Ques_Req"].Tag = "N";
                                        }
                                    }
                                }



                            }
                        }
                    }
                }

            }

        }

        string BlankImage = Consts.Icons.ico_Blank;
        private void gvQuestions_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string DeleteImage = Consts.Icons.ico_Delete;
            string BlankImage = Consts.Icons.ico_Blank;
            if (objArgs.MenuItem.Tag is SalqrespEntity)
            {
                SalqrespEntity dr = (SalqrespEntity)objArgs.MenuItem.Tag as SalqrespEntity;
                string selectedValue = objArgs.MenuItem.Text;
                string selectedCode = dr.SALQR_CODE.ToString();
                //menuQueserEntity = dr;
                MenuSelectCode = selectedCode;
                MenuSelectValue = selectedValue;

                if (objArgs.MenuItem.Checked)
                {
                    //responseValue = selectedValue;
                    //responseCode = selectedCode;
                    gvQuestions.SelectedRows[0].Cells[1].Value = selectedValue;
                    gvQuestions.SelectedRows[0].Cells[1].Tag = selectedCode;
                    gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value = selectedCode;
                    gvQuestions.SelectedRows[0].Cells["Responce"].Value = selectedValue;
                    gvQuestions.SelectedRows[0].Cells["Ques_Delete"].Value = DeleteImage;

                    if (FormType == "SAL")
                    {
                        SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                        if (ActEntity != null)
                        {
                            ActEntity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            ActEntity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            ActEntity.SALACT_TYPE = "S";
                            ActEntity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            ActEntity.SALACT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                ActEntity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                ActEntity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                ActEntity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                            ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                            //SALACTList.Add(new SALACTEntity(ActEntity));
                        }
                        else
                        {

                            SALACTEntity Search_Act_entity = new SALACTEntity();
                            Search_Act_entity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            Search_Act_entity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            Search_Act_entity.SALACT_TYPE = "S";
                            Search_Act_entity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            Search_Act_entity.SALACT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                Search_Act_entity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                Search_Act_entity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                            Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                            SALACTList.Add(new SALACTEntity(Search_Act_entity));

                        }
                    }
                    else if (FormType == "CAL")
                    {
                        CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                        if (ActEntity != null)
                        {

                            ActEntity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            ActEntity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            ActEntity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            ActEntity.CALCONT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                ActEntity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                ActEntity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                ActEntity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            ActEntity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                            ActEntity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                            //SALACTList.Add(new SALACTEntity(ActEntity));
                        }
                        else
                        {
                            CALCONTEntity Search_Act_entity = new CALCONTEntity();
                            Search_Act_entity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            Search_Act_entity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            Search_Act_entity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            Search_Act_entity.CALCONT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                Search_Act_entity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                Search_Act_entity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                Search_Act_entity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            Search_Act_entity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                            Search_Act_entity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                            CALCONTList.Add(new CALCONTEntity(Search_Act_entity));
                        }
                    }

                    string strDimentionQid = gvQuestions.SelectedRows[0].Cells["Link_Ques"].Value.ToString();
                    if (strDimentionQid != string.Empty)
                    {
                        string strEnabledata = string.Empty;
                        string[] arrEnabledata = null;
                        string strDisabledata = string.Empty;
                        string[] arrDisabledata = null;
                        string strRequiredata = string.Empty;
                        string[] arrRequiredata = null;
                        foreach (DataGridViewRow item in gvQuestions.Rows)
                        {
                            if (item.Cells["Ques_ID"].Value.ToString().Trim() == strDimentionQid)
                            {
                                SALQLNKEntity preassesdimentdata = salQlinkEntitylist.Find(u => u.SALQL_LINKQ == strDimentionQid);
                                if (preassesdimentdata != null)
                                {
                                    strDisabledata = preassesdimentdata.SALQL_DISABLE;
                                    strEnabledata = preassesdimentdata.SALQL_ENABLE;
                                    arrDisabledata = null;
                                    arrEnabledata = null;
                                    if (strDisabledata.IndexOf(',') > 0)
                                    {
                                        arrDisabledata = strDisabledata.Split(',');
                                    }
                                    else if (!strDisabledata.Equals(string.Empty))
                                    {
                                        arrDisabledata = new string[] { strDisabledata };
                                    }
                                    if (strEnabledata.IndexOf(',') > 0)
                                    {
                                        arrEnabledata = strEnabledata.Split(',');
                                    }
                                    else if (!strEnabledata.Equals(string.Empty))
                                    {
                                        arrEnabledata = new string[] { strEnabledata };
                                    }
                                    strRequiredata = preassesdimentdata.SALQL_REQ;
                                    arrRequiredata = null;
                                    if (strRequiredata.IndexOf(',') > 0)
                                    {
                                        arrRequiredata = strRequiredata.Split(',');
                                    }
                                    else if (!strRequiredata.Equals(string.Empty))
                                    {
                                        arrRequiredata = new string[] { strRequiredata };
                                    }

                                    //item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                    if (arrDisabledata != null)
                                    {
                                        if (arrDisabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                            item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                            item.Cells["Responce"].Tag = null;
                                            item.Cells["Responce"].Value = string.Empty;
                                            item.Cells["ResponceCode"].Tag = null;
                                            item.Cells["ResponceCode"].Value = string.Empty;
                                            item.Cells["Ques_Enable"].Tag = "N";
                                            if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                                item.Cells["Ques_Delete"].Value = BlankImage;
                                        }

                                    }
                                    if (arrEnabledata != null)
                                    {
                                        if (arrEnabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                            item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                            item.Cells["Ques_Enable"].Tag = "Y";
                                        }
                                        else
                                        {
                                            if (arrDisabledata != null)
                                            {
                                                if (arrDisabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                                {
                                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                    item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                    item.Cells["Responce"].Tag = null;
                                                    item.Cells["Responce"].Value = string.Empty;
                                                    item.Cells["ResponceCode"].Tag = null;
                                                    item.Cells["ResponceCode"].Value = string.Empty;
                                                    item.Cells["Ques_Enable"].Tag = "N";
                                                    if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                                        item.Cells["Ques_Delete"].Value = BlankImage;
                                                }

                                            }
                                        }
                                    }
                                    if (arrRequiredata != null)
                                    {
                                        if (arrRequiredata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                        {
                                            item.Cells["Ques_Req"].Tag = "Y";
                                        }
                                        else
                                        {
                                            item.Cells["Ques_Req"].Tag = "N";
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
                else
                {

                    gvQuestions.SelectedRows[0].Cells[1].Value = selectedValue;
                    gvQuestions.SelectedRows[0].Cells[1].Tag = selectedCode;
                    gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value = selectedCode;
                    gvQuestions.SelectedRows[0].Cells["Responce"].Value = selectedValue;
                    gvQuestions.SelectedRows[0].Cells["Ques_Delete"].Value = DeleteImage;

                    if (FormType == "SAL")
                    {
                        SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                        if (ActEntity != null)
                        {

                            ActEntity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            ActEntity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            ActEntity.SALACT_TYPE = "S";
                            ActEntity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            ActEntity.SALACT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                ActEntity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                ActEntity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                ActEntity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                            ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                            //SALACTList.Add(new SALACTEntity(ActEntity));
                        }
                        else
                        {
                            SALACTEntity Search_Act_entity = new SALACTEntity();
                            Search_Act_entity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            Search_Act_entity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            Search_Act_entity.SALACT_TYPE = "S";
                            Search_Act_entity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            Search_Act_entity.SALACT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                Search_Act_entity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                Search_Act_entity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                            Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                            SALACTList.Add(new SALACTEntity(Search_Act_entity));
                        }
                    }
                    else if (FormType == "CAL")
                    {
                        CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                        if (ActEntity != null)
                        {

                            ActEntity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            ActEntity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            ActEntity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            ActEntity.CALCONT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                ActEntity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                ActEntity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                ActEntity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            ActEntity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                            ActEntity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                            //SALACTList.Add(new SALACTEntity(ActEntity));
                        }
                        else
                        {
                            CALCONTEntity Search_Act_entity = new CALCONTEntity();
                            Search_Act_entity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                            Search_Act_entity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                            Search_Act_entity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                            Search_Act_entity.CALCONT_SEQ = "1";
                            if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                                Search_Act_entity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                                Search_Act_entity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                            else
                                Search_Act_entity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                            Search_Act_entity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                            Search_Act_entity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                            CALCONTList.Add(new CALCONTEntity(Search_Act_entity));
                        }
                    }

                    string strDimentionQid = gvQuestions.SelectedRows[0].Cells["Link_Ques"].Value.ToString();
                    if (strDimentionQid != string.Empty)
                    {
                        string strEnabledata = string.Empty;
                        string[] arrEnabledata = null;
                        string strDisabledata = string.Empty;
                        string[] arrDisabledata = null;
                        string strRequiredata = string.Empty;
                        string[] arrRequiredata = null;
                        foreach (DataGridViewRow item in gvQuestions.Rows)
                        {
                            if (item.Cells["Ques_ID"].Value.ToString().Trim() == strDimentionQid)
                            {
                                SALQLNKEntity preassesdimentdata = salQlinkEntitylist.Find(u => u.SALQL_LINKQ == strDimentionQid);
                                if (preassesdimentdata != null)
                                {
                                    strDisabledata = preassesdimentdata.SALQL_DISABLE;
                                    strEnabledata = preassesdimentdata.SALQL_ENABLE;
                                    arrDisabledata = null;
                                    arrEnabledata = null;
                                    if (strDisabledata.IndexOf(',') > 0)
                                    {
                                        arrDisabledata = strDisabledata.Split(',');
                                    }
                                    else if (!strDisabledata.Equals(string.Empty))
                                    {
                                        arrDisabledata = new string[] { strDisabledata };
                                    }
                                    if (strEnabledata.IndexOf(',') > 0)
                                    {
                                        arrEnabledata = strEnabledata.Split(',');
                                    }
                                    else if (!strEnabledata.Equals(string.Empty))
                                    {
                                        arrEnabledata = new string[] { strEnabledata };
                                    }
                                    strRequiredata = preassesdimentdata.SALQL_REQ;
                                    arrRequiredata = null;
                                    if (strRequiredata.IndexOf(',') > 0)
                                    {
                                        arrRequiredata = strRequiredata.Split(',');
                                    }
                                    else if (!strRequiredata.Equals(string.Empty))
                                    {
                                        arrRequiredata = new string[] { strRequiredata };
                                    }

                                    //item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                    if (arrDisabledata != null)
                                    {
                                        if (arrDisabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                            item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                            item.Cells["Responce"].Tag = null;
                                            item.Cells["Responce"].Value = string.Empty;
                                            item.Cells["ResponceCode"].Tag = null;
                                            item.Cells["ResponceCode"].Value = string.Empty;
                                            item.Cells["Ques_Enable"].Tag = "N";
                                            if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                                item.Cells["Ques_Delete"].Value = BlankImage;
                                        }

                                    }
                                    if (arrEnabledata != null)
                                    {
                                        if (arrEnabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                            item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                            item.Cells["Ques_Enable"].Tag = "Y";
                                        }
                                        else
                                        {
                                            if (arrDisabledata != null)
                                            {
                                                if (arrDisabledata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                                {
                                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                    item.Cells["Ques_Req"].Style.ForeColor = System.Drawing.Color.Red;
                                                    item.Cells["Responce"].Tag = null;
                                                    item.Cells["Responce"].Value = string.Empty;
                                                    item.Cells["ResponceCode"].Tag = null;
                                                    item.Cells["ResponceCode"].Value = string.Empty;
                                                    item.Cells["Ques_Enable"].Tag = "N";
                                                    if (!string.IsNullOrEmpty(selectedCode.Trim()))
                                                        item.Cells["Ques_Delete"].Value = BlankImage;
                                                }

                                            }
                                        }
                                    }
                                    if (arrRequiredata != null)
                                    {
                                        if (arrRequiredata.ToList().Exists(u => u.Equals(selectedCode.Trim())))
                                        {
                                            item.Cells["Ques_Req"].Tag = "Y";
                                        }
                                        else
                                        {
                                            item.Cells["Ques_Req"].Tag = "N";
                                        }
                                    }

                                }
                            }
                        }

                    }

                }
            }
            else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "1" || gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "2" || gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "3")
            {

                pnlQues.Visible = true; this.Size = new Size(this.Width, 674);
                gvQuestions.Enabled = false;
                gvSALNames.Enabled = false;

                if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "1")
                { txtQues1.Visible = true; txtQues2.Visible = false; txtQues3.Visible = false; }
                if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "2")
                { txtQues1.Visible = true; txtQues2.Visible = true; txtQues3.Visible = false; }
                if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "3")
                { txtQues1.Visible = true; txtQues2.Visible = true; txtQues3.Visible = true; }


                if (objArgs.MenuItem.Text == "Edit")
                {
                    if (!string.IsNullOrEmpty(gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString().Trim()))
                    {

                        string[] locResult = Regex.Split(gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString(), "[\r\n]+");
                        if (locResult.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(locResult[0])) txtQues1.Text = locResult[0].Trim();
                            if (!string.IsNullOrEmpty(locResult[1])) txtQues2.Text = locResult[1].Trim();
                            if (!string.IsNullOrEmpty(locResult[2])) txtQues3.Text = locResult[2].Trim();
                        }

                    }
                }
                else { txtQues1.Text = txtQues2.Text = txtQues3.Text = string.Empty; }

            }



        }

        private void btnQuesCanel_Click(object sender, EventArgs e)
        {
            pnlQues.Visible = false; this.Size = new Size(this.Width, 674 - pnlResp.Height);
            gvQuestions.Enabled = true;
            gvSALNames.Enabled = true;
        }

        private void btnQuesOk_Click(object sender, EventArgs e)
        {
            pnlQues.Visible = false; this.Size = new Size(this.Width, 674 - pnlResp.Height);
            if (!string.IsNullOrEmpty(txtQues1.Text.Trim()) || !string.IsNullOrEmpty(txtQues2.Text.Trim()) || !string.IsNullOrEmpty(txtQues3.Text.Trim()))
            {
                if (!string.IsNullOrEmpty(txtQues1.Text.Trim())) txtQues1.Text = txtQues1.Text.Trim(); else txtQues1.Text = " ";
                if (!string.IsNullOrEmpty(txtQues2.Text.Trim())) txtQues2.Text = txtQues2.Text.Trim(); else txtQues2.Text = " ";
                if (!string.IsNullOrEmpty(txtQues3.Text.Trim())) txtQues3.Text = txtQues3.Text.Trim(); else txtQues3.Text = " ";

                if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "1")
                {
                    gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value = txtQues1.Text;
                    gvQuestions.SelectedRows[0].Cells["Responce"].Value = txtQues1.Text;
                }
                else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "2")
                {
                    gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value = txtQues1.Text + "\n" + txtQues2.Text;
                    gvQuestions.SelectedRows[0].Cells["Responce"].Value = txtQues1.Text + "\n" + txtQues2.Text;
                }
                else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "3")
                {
                    gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value = txtQues1.Text + "\n" + txtQues2.Text + "\n" + txtQues3.Text;
                    gvQuestions.SelectedRows[0].Cells["Responce"].Value = txtQues1.Text + "\n" + txtQues2.Text + "\n" + txtQues3.Text;
                }

                if (FormType == "SAL")
                {

                    SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                    if (ActEntity != null)
                    {

                        ActEntity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        ActEntity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        ActEntity.SALACT_TYPE = "S";
                        ActEntity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        ActEntity.SALACT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            ActEntity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            ActEntity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            ActEntity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                        ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                        //SALACTList.Add(new SALACTEntity(ActEntity));
                    }
                    else
                    {
                        SALACTEntity Search_Act_entity = new SALACTEntity();
                        Search_Act_entity.SALACT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        Search_Act_entity.SALACT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        Search_Act_entity.SALACT_TYPE = "S";
                        Search_Act_entity.SALACT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        Search_Act_entity.SALACT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            Search_Act_entity.SALACT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            Search_Act_entity.SALACT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            Search_Act_entity.SALACT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        Search_Act_entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                        Search_Act_entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                        SALACTList.Add(new SALACTEntity(Search_Act_entity));
                    }
                }
                else if (FormType == "CAL")
                {
                    CALCONTEntity ActEntity = CALCONTList.Find(u => u.CALCONT_SALID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString()) && u.CALCONT_Q_ID.Equals(gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString()));
                    if (ActEntity != null)
                    {

                        ActEntity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        ActEntity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        ActEntity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        ActEntity.CALCONT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            ActEntity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            ActEntity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            ActEntity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        ActEntity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                        ActEntity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                        //SALACTList.Add(new SALACTEntity(ActEntity));
                    }
                    else
                    {
                        CALCONTEntity Search_Act_entity = new CALCONTEntity();
                        Search_Act_entity.CALCONT_SALID = gvQuestions.SelectedRows[0].Cells["Ques_SAL_ID"].Value.ToString();
                        Search_Act_entity.CALCONT_Q_ID = gvQuestions.SelectedRows[0].Cells["Ques_ID"].Value.ToString();
                        Search_Act_entity.CALCONT_Q_TYPE = gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString();
                        Search_Act_entity.CALCONT_SEQ = "1";
                        if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "T")
                            Search_Act_entity.CALCONT_DATE_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else if (gvQuestions.SelectedRows[0].Cells["Ques_Type"].Value.ToString() == "N")
                            Search_Act_entity.CALCONT_NUM_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();
                        else
                            Search_Act_entity.CALCONT_MULTI_RESP = gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value.ToString();

                        Search_Act_entity.CALCONT_ADD_OPERATOR = BaseForm.UserID;
                        Search_Act_entity.CALCONT_LSTC_OPERATOR = BaseForm.UserID;

                        CALCONTList.Add(new CALCONTEntity(Search_Act_entity));
                    }
                }

                if (!string.IsNullOrEmpty(gvQuestions.SelectedRows[0].Cells["Responce"].Value.ToString().Trim()))
                    gvQuestions.SelectedRows[0].Cells["Ques_Delete"].Value = DeleteImage;
                else
                    gvQuestions.SelectedRows[0].Cells["Ques_Delete"].Value = BlankImage;

            }
            else
            {
                gvQuestions.SelectedRows[0].Cells["ResponceCode"].Value = string.Empty;
                gvQuestions.SelectedRows[0].Cells["Responce"].Value = string.Empty;
                gvQuestions.SelectedRows[0].Cells["Ques_Delete"].Value = BlankImage;
            }
            gvQuestions.Enabled = true;
            gvSALNames.Enabled = true;
        }

        public List<SALACTEntity> SALACTData()
        {
            List<SALACTEntity> SALdata = new List<SALACTEntity>();

            //if (gvSALNames.Rows.Count > 0)
            //{
            //    foreach (DataGridViewRow dr in gvSALNames.Rows)
            //    {

            //        SALACTEntity sea_entity = SALACTList.Find(u => u.SALACT_ID.Equals(Pass_CA_Entity.ACT_ID) && u.SALACT_SALID.Equals(dr.Cells["SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals("0"));
            //        if (sea_entity != null)
            //            SALACTList.Remove(sea_entity);

            //        SALACTEntity Entity = new SALACTEntity();
            //        Entity.SALACT_ID = Pass_CA_Entity.ACT_ID;
            //        Entity.SALACT_SALID = dr.Cells["SAL_ID"].Value.ToString();
            //        Entity.SALACT_Q_ID = "0";
            //        Entity.SALACT_TYPE = "S";
            //        Entity.SALACT_STATUS = ((Captain.Common.Utilities.ListItem)cmbStatus.SelectedItem).Value.ToString();
            //        Entity.SALACT_LOCATION = ((Captain.Common.Utilities.ListItem)cmbLocation.SelectedItem).Value.ToString();
            //        Entity.SALACT_RECIPIENT = ((Captain.Common.Utilities.ListItem)cmbReceipent.SelectedItem).Value.ToString();
            //        Entity.SALACT_ATTN = ((Captain.Common.Utilities.ListItem)cmbAttn.SelectedItem).Value.ToString();

            //        Entity.SALACT_TIME_IN = DT_Dur_From.Value.ToString("HH:mm:ss");
            //        Entity.SALACT_TIME_OUT = DT_Dur_To.Value.ToString("HH:mm:ss");
            //        Entity.SALACT_TIME_SPENT = DT_Duration.Value.ToString("HH:mm:ss");

            //        Entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
            //        Entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

            //        SALACTList.Add(new SALACTEntity(Entity));

            //        //if (gvQuestions.Rows.Count > 0)
            //        //{
            //        //    int i = 1;
            //        //    foreach (DataGridViewRow drques in gvQuestions.Rows)
            //        //    {
            //        //        if (!string.IsNullOrEmpty(drques.Cells["ResponceCode"].Value.ToString().Trim()) && drques.Cells["Ques_SAL_ID"].Value.ToString() == Entity.SALACT_SALID)
            //        //        {
            //        //            SALACTEntity QuesEntity = new SALACTEntity(true);
            //        //            QuesEntity.SALACT_ID = Pass_CA_Entity.ACT_ID;
            //        //            QuesEntity.SALACT_SALID = drques.Cells["Ques_SAL_ID"].Value.ToString();
            //        //            QuesEntity.SALACT_Q_ID = drques.Cells["Ques_ID"].Value.ToString();
            //        //            QuesEntity.SALACT_TYPE = "S";
            //        //            QuesEntity.SALACT_Q_TYPE = drques.Cells["Ques_Type"].Value.ToString();
            //        //            QuesEntity.SALACT_SEQ = i.ToString();
            //        //            if (drques.Cells["Ques_Type"].Value.ToString() == "T")
            //        //                QuesEntity.SALACT_DATE_RESP = drques.Cells["ResponceCode"].Value.ToString();
            //        //            else
            //        //                QuesEntity.SALACT_MULTI_RESP = drques.Cells["ResponceCode"].Value.ToString();

            //        //            QuesEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
            //        //            QuesEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

            //        //            SALdata.Add(new SALACTEntity(QuesEntity));
            //        //            i++;
            //        //        }
            //        //    }
            //        //}

            //    }

            //}

            if (SALACTList.Count > 0)
            {
                SALACTList = SALACTList.OrderBy(u => u.SALACT_ID).ThenBy(u => u.SALACT_SALID).ThenBy(u => u.SALACT_Q_ID).ToList();
                SALdata.AddRange(SALACTList);
            }

            return SALdata;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            //string strSignYear = "YYYY";
            //if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            //{
            //    strSignYear = BaseForm.BaseYear;
            //}
            //PdfViewerNewForm Signature = new PdfViewerNewForm(string.Empty, BaseForm.BaseAgency.ToString() + BaseForm.BaseDept.ToString() + BaseForm.BaseProg.ToString() + strSignYear + BaseForm.BaseApplicationNo.ToString());
            if (FormType == "SAL")
            {
                PdfViewerNewForm Signature = new PdfViewerNewForm(gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString(), Pass_CA_Entity.ACT_ID, BaseForm.UserID, "SAL");
                Signature.StartPosition = FormStartPosition.CenterScreen;

                Signature.ShowDialog();
            }
            else if (FormType == "CAL")
            {
                PdfViewerNewForm Signature = new PdfViewerNewForm(gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString(), PASS_Cont_Entity.Contact_ID, BaseForm.UserID, "CAL");
                Signature.StartPosition = FormStartPosition.CenterScreen;
                Signature.ShowDialog();
            }
        }

        private void cmbAttn_SelectedIndexChanged(object sender, EventArgs e)
        {
            SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals("0"));
            if (ActEntity != null)
            {

                ActEntity.SALACT_ID = Pass_CA_Entity.ACT_ID;
                ActEntity.SALACT_SALID = gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString();
                ActEntity.SALACT_Q_ID = "0";
                ActEntity.SALACT_TYPE = "S";
                ActEntity.SALACT_STATUS = ((Captain.Common.Utilities.ListItem)cmbStatus.SelectedItem).Value.ToString();
                ActEntity.SALACT_LOCATION = ((Captain.Common.Utilities.ListItem)cmbLocation.SelectedItem).Value.ToString();
                ActEntity.SALACT_RECIPIENT = ((Captain.Common.Utilities.ListItem)cmbReceipent.SelectedItem).Value.ToString();
                ActEntity.SALACT_ATTN = ((Captain.Common.Utilities.ListItem)cmbAttn.SelectedItem).Value.ToString();

                ActEntity.SALACT_TIME_IN = DT_Dur_From.Value.ToString("HH:mm:ss");
                ActEntity.SALACT_TIME_OUT = DT_Dur_To.Value.ToString("HH:mm:ss");
                ActEntity.SALACT_TIME_SPENT = DT_Duration.Value.ToString("HH:mm:ss");

                ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                //SALACTList.Add(new SALACTEntity(ActEntity));
            }
            else
            {
                SALACTEntity Entity = new SALACTEntity();
                Entity.SALACT_ID = Pass_CA_Entity.ACT_ID;
                Entity.SALACT_SALID = gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString();
                Entity.SALACT_Q_ID = "0";
                Entity.SALACT_TYPE = "S";
                Entity.SALACT_STATUS = ((Captain.Common.Utilities.ListItem)cmbStatus.SelectedItem).Value.ToString();
                Entity.SALACT_LOCATION = ((Captain.Common.Utilities.ListItem)cmbLocation.SelectedItem).Value.ToString();
                Entity.SALACT_RECIPIENT = ((Captain.Common.Utilities.ListItem)cmbReceipent.SelectedItem).Value.ToString();
                Entity.SALACT_ATTN = ((Captain.Common.Utilities.ListItem)cmbAttn.SelectedItem).Value.ToString();
                Entity.SALACT_TIME_IN = DT_Dur_From.Value.ToString("HH:mm:ss");
                Entity.SALACT_TIME_OUT = DT_Dur_To.Value.ToString("HH:mm:ss");
                Entity.SALACT_TIME_SPENT = DT_Duration.Value.ToString("HH:mm:ss");

                Entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                Entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                SALACTList.Add(new SALACTEntity(Entity));
            }
        }

        private void DT_Duration_Leave(object sender, EventArgs e)
        {
            SALACTEntity ActEntity = SALACTList.Find(u => u.SALACT_SALID.Equals(gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString()) && u.SALACT_Q_ID.Equals("0"));
            if (ActEntity != null)
            {
                ActEntity.SALACT_ID = Pass_CA_Entity.ACT_ID;
                ActEntity.SALACT_SALID = gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString();
                ActEntity.SALACT_Q_ID = "0";
                ActEntity.SALACT_TYPE = "S";
                ActEntity.SALACT_STATUS = ((Captain.Common.Utilities.ListItem)cmbStatus.SelectedItem).Value.ToString();
                ActEntity.SALACT_LOCATION = ((Captain.Common.Utilities.ListItem)cmbLocation.SelectedItem).Value.ToString();
                ActEntity.SALACT_RECIPIENT = ((Captain.Common.Utilities.ListItem)cmbReceipent.SelectedItem).Value.ToString();
                ActEntity.SALACT_ATTN = ((Captain.Common.Utilities.ListItem)cmbAttn.SelectedItem).Value.ToString();

                ActEntity.SALACT_TIME_IN = DT_Dur_From.Value.ToString("HH:mm:ss");
                ActEntity.SALACT_TIME_OUT = DT_Dur_To.Value.ToString("HH:mm:ss");
                ActEntity.SALACT_TIME_SPENT = DT_Duration.Value.ToString("HH:mm:ss");

                ActEntity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                ActEntity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                //SALACTList.Add(new SALACTEntity(ActEntity));
            }
            else
            {
                SALACTEntity Entity = new SALACTEntity();
                Entity.SALACT_ID = Pass_CA_Entity.ACT_ID;
                Entity.SALACT_SALID = gvSALNames.SelectedRows[0].Cells["SAL_ID"].Value.ToString();
                Entity.SALACT_Q_ID = "0";
                Entity.SALACT_TYPE = "S";
                Entity.SALACT_STATUS = ((Captain.Common.Utilities.ListItem)cmbStatus.SelectedItem).Value.ToString();
                Entity.SALACT_LOCATION = ((Captain.Common.Utilities.ListItem)cmbLocation.SelectedItem).Value.ToString();
                Entity.SALACT_RECIPIENT = ((Captain.Common.Utilities.ListItem)cmbReceipent.SelectedItem).Value.ToString();
                Entity.SALACT_ATTN = ((Captain.Common.Utilities.ListItem)cmbAttn.SelectedItem).Value.ToString();

                Entity.SALACT_TIME_IN = DT_Dur_From.Value.ToString("HH:mm:ss");
                Entity.SALACT_TIME_OUT = DT_Dur_To.Value.ToString("HH:mm:ss");
                Entity.SALACT_TIME_SPENT = DT_Duration.Value.ToString("HH:mm:ss");

                Entity.SALACT_ADD_OPERATOR = BaseForm.UserID;
                Entity.SALACT_LSTC_OPERATOR = BaseForm.UserID;

                SALACTList.Add(new SALACTEntity(Entity));
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isValidate())
            {
                if (FormType == "SAL")
                {
                    if (SALACTList.Count > 0)
                    {
                        SALACTEntity Search_Sal = new SALACTEntity();
                        Search_Sal.SALACT_ID = Pass_CA_Entity.ACT_ID;
                        Search_Sal.Mode = "DELETEALL";
                        _model.SALDEFData.CAP_SALACT_INSUPDEL(Search_Sal);

                        int i = 1;
                        foreach (SALACTEntity SalEntity in SALACTList)
                        {
                            SalEntity.SALACT_ID = Pass_CA_Entity.ACT_ID;
                            SalEntity.SALACT_SEQ = i.ToString();
                            SalEntity.Mode = "ADD";

                            _model.SALDEFData.CAP_SALACT_INSUPDEL(SalEntity);
                            i++;
                        }

                        AlertBox.Show("Service Activity Log Posting Successful");


                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else if (!string.IsNullOrEmpty(Pass_CA_Entity.ACT_ID.Trim()) && Isdelete)
                    {
                        SALACTEntity Search_Sal = new SALACTEntity();
                        Search_Sal.SALACT_ID = Pass_CA_Entity.ACT_ID;
                        Search_Sal.Mode = "DELETEALL";
                        _model.SALDEFData.CAP_SALACT_INSUPDEL(Search_Sal);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else if (FormType == "CAL")
                {
                    if (CALCONTList.Count > 0)
                    {
                        CALCONTEntity Search_Sal = new CALCONTEntity();
                        Search_Sal.CALCONT_ID = PASS_Cont_Entity.Contact_ID;
                        Search_Sal.Mode = "DELETEALL";
                        _model.SALDEFData.CAP_CALCONT_INSUPDEL(Search_Sal);

                        int i = 1;
                        foreach (CALCONTEntity calEntity in CALCONTList)
                        {
                            calEntity.CALCONT_ID = PASS_Cont_Entity.Contact_ID;
                            calEntity.CALCONT_SEQ = i.ToString();
                            calEntity.Mode = "ADD";

                            _model.SALDEFData.CAP_CALCONT_INSUPDEL(calEntity);
                            i++;
                        }

                        AlertBox.Show("Contact Activity Log Posting Successful");


                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else if (!string.IsNullOrEmpty(PASS_Cont_Entity.Contact_ID.Trim()) && Isdelete)
                    {
                        CALCONTEntity Search_Sal = new CALCONTEntity();
                        Search_Sal.CALCONT_ID = PASS_Cont_Entity.Contact_ID;
                        Search_Sal.Mode = "DELETEALL";
                        _model.SALDEFData.CAP_CALCONT_INSUPDEL(Search_Sal);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }

        private bool isValidate()
        {
            bool isValid = true;
            if (gvQuestions.Rows.Count > 0)
            {
                foreach (DataGridViewRow dataGridViewRow in gvQuestions.Rows)
                {
                    if (dataGridViewRow.Tag is SalquesEntity)
                    {
                        SalquesEntity custques = dataGridViewRow.Tag as SalquesEntity;
                        if (custques != null)
                        {
                            string strQuestiondi = dataGridViewRow.Cells["Link_Ques"].Value.ToString();
                            string strFieldEnable = dataGridViewRow.Cells["Ques_Enable"].Tag != null ? dataGridViewRow.Cells["Ques_Enable"].Tag.ToString() : string.Empty;
                            string strRequire = dataGridViewRow.Cells["Ques_Req"].Tag != null ? dataGridViewRow.Cells["Ques_Req"].Tag.ToString() : string.Empty;
                            if (dataGridViewRow.Cells["Ques_Req"].Value.ToString() == "*")
                            {
                                SALQLNKEntity preassesdimentdata = salQlinkEntitylist.Find(u => u.SALQL_LINKQ == strQuestiondi);
                                if (preassesdimentdata != null)
                                {
                                    if (strFieldEnable != "N" && strRequire == "Y")
                                    {
                                        string inputValue = string.Empty;
                                        inputValue = dataGridViewRow.Cells["Responce"].Value != null ? dataGridViewRow.Cells["Responce"].Value.ToString() : string.Empty;
                                        if (inputValue == string.Empty)
                                        {
                                            AlertBox.Show("Please enter require questions answers", MessageBoxIcon.Warning);
                                            isValid = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (strFieldEnable != "N")
                                    {
                                        string inputValue = string.Empty;
                                        inputValue = dataGridViewRow.Cells["Responce"].Value != null ? dataGridViewRow.Cells["Responce"].Value.ToString() : string.Empty;
                                        if (inputValue == string.Empty)
                                        {
                                            AlertBox.Show("Please enter require preasses questions answers", MessageBoxIcon.Warning);
                                            isValid = false;
                                            break;
                                        }
                                    }

                                }


                            }
                            else
                            {
                                if (strFieldEnable != "N" && strRequire == "Y")
                                {
                                    string inputValue = string.Empty;
                                    inputValue = dataGridViewRow.Cells["Responce"].Value != null ? dataGridViewRow.Cells["Responce"].Value.ToString() : string.Empty;
                                    string strQuestion = dataGridViewRow.Cells["Ques_Desc"].Value != null ? dataGridViewRow.Cells["Ques_Desc"].Value.ToString() : string.Empty;
                                    if (inputValue == string.Empty)
                                    {
                                        AlertBox.Show("Please enter " + strQuestion + " question answer", MessageBoxIcon.Warning);
                                        isValid = false;
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return isValid;
        }

        private void PbPdf_Click(object sender, EventArgs e)
        {
            On_SaveForm_Closed(PdfName, EventArgs.Empty);
            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);

        }


        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        string PdfName = "Pdf File";

        List<CaseNotesEntity> caseNotesEntity = new List<CaseNotesEntity>();
        private void picSPMNotes_Click(object sender, EventArgs e)
        {
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Pass_CA_Entity.Year + BaseForm.BaseApplicationNo + Pass_CA_Entity.Service_plan + Pass_CA_Entity.SPM_Seq);
            CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Pass_CA_Entity.Year + BaseForm.BaseApplicationNo + Pass_CA_Entity.Service_plan + Pass_CA_Entity.SPM_Seq, "SAL");
            caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
            caseNotes.ShowDialog();
        }

        private void OnCaseNotesFormClosed(object sender, FormClosedEventArgs e)
        {
            CaseNotes form = sender as CaseNotes;

            //if (form.DialogResult == DialogResult.OK)
            //{
            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Pass_CA_Entity.Year + BaseForm.BaseApplicationNo + Pass_CA_Entity.Service_plan + Pass_CA_Entity.SPM_Seq);
            if (caseNotesEntity.Count > 0)
            {
                this.Tools["picSPMNotes"].ImageSource = Consts.Icons.ico_CaseNotes_View;
            }
            else
            {
                this.Tools["picSPMNotes"].ImageSource = Consts.Icons.ico_CaseNotes_New;
            }
            caseNotesEntity = caseNotesEntity;

            //}
        }

        private void ShowCaseNotesImages()
        {
            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Pass_CA_Entity.Year + BaseForm.BaseApplicationNo + Pass_CA_Entity.Service_plan + Pass_CA_Entity.SPM_Seq);
            if (caseNotesEntity.Count > 0)
            {
                this.Tools["picSPMNotes"].ImageSource = Consts.Icons.ico_CaseNotes_View;
            }
            else
            {
                this.Tools["picSPMNotes"].ImageSource = Consts.Icons.ico_CaseNotes_New;
            }
            //if (!(CmbSP.Items.Count > 0)) picSPMNotes.Enabled = false; else picSPMNotes.Enabled = true;


        }

        private void SALCAL_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (FormType == "SAL")
            //{
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}
        }

        private void SALCAL_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "picSPMNotes")
            {
                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Pass_CA_Entity.Year + BaseForm.BaseApplicationNo + Pass_CA_Entity.Service_plan + Pass_CA_Entity.SPM_Seq);
                CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Pass_CA_Entity.Year + BaseForm.BaseApplicationNo + Pass_CA_Entity.Service_plan + Pass_CA_Entity.SPM_Seq, "SAL");
                caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                caseNotes.StartPosition = FormStartPosition.CenterScreen;
                caseNotes.ShowDialog();
            }
            else if (e.Tool.Name == "PbPdf")
            {
                On_SaveForm_Closed(PdfName, EventArgs.Empty);
                if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                {
                    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
                else
                {
                    FrmViewer objfrm = new FrmViewer(PdfName);
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
            }
        }

        private void On_SaveForm_Closed(object sender, EventArgs e)
        {
            Random_Filename = null;

            PdfName = "CAL Form";
            //PdfName = strFolderPath + PdfName;

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bftimes = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bftimes, 16, 2, new BaseColor(0, 0, 102));
            //BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            //iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
            //BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bftimes, 10);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;

            PdfPTable APP_details = new PdfPTable(3);
            APP_details.TotalWidth = 750f;
            APP_details.WidthPercentage = 100;
            APP_details.LockedWidth = true;
            float[] APP_details_Widths = new float[] { 30f, 40f, 30f };
            APP_details.SetWidths(APP_details_Widths);
            APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
            APP_details.SpacingBefore = 9f;


            PdfPCell Appl_No = new PdfPCell(new Phrase("App# :" + BaseForm.BaseApplicationNo.Trim(), TblFontBold));
            Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(Appl_No);

            PdfPCell App_Name = new PdfPCell(new Phrase("App Name :" + BaseForm.BaseApplicationName.Trim(), TblFontBold));
            App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(App_Name);

            PdfPCell Date = new PdfPCell(new Phrase("", TblFontBold));
            Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(Date);

            PdfPTable TableData = new PdfPTable(3);
            TableData.TotalWidth = 750f;
            TableData.WidthPercentage = 100;
            TableData.LockedWidth = true;
            float[] Widths = new float[] { 8f, 80f, 22f };
            TableData.SetWidths(Widths);
            TableData.HorizontalAlignment = Element.ALIGN_CENTER;
            TableData.SpacingBefore = 30f;

            document.Add(APP_details);

            bool Isfirst = true;
            foreach (SaldefEntity Entity in SALDEFEntity)
            {
                if (!Isfirst)
                    document.NewPage();

                PdfPCell DataHeading = new PdfPCell(new Phrase(Entity.SALD_NAME.Trim(), helvetica));
                DataHeading.Colspan = 3;
                DataHeading.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                DataHeading.Border = iTextSharp.text.Rectangle.NO_BORDER;
                TableData.AddCell(DataHeading);

                PdfPCell Space1 = new PdfPCell(new Phrase("", TblFontBold));
                Space1.Colspan = 3;
                Space1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                TableData.AddCell(Space1);

                Isfirst = false;
                if (SALQUESEntity.Count > 0)
                {
                    PdfPCell Q1 = new PdfPCell(new Phrase("Question", TblFontBold));
                    Q1.Colspan = 2;
                    Q1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Q1.Border = iTextSharp.text.Rectangle.BOX;
                    TableData.AddCell(Q1);

                    PdfPCell Q2 = new PdfPCell(new Phrase("Response", TblFontBold));
                    Q2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Q2.Border = iTextSharp.text.Rectangle.BOX;
                    TableData.AddCell(Q2);

                    List<SalquesEntity> SelQues = SALQUESEntity.FindAll(u => u.SALQ_SALD_ID.Equals(Entity.SALD_ID));

                    if (SelQues.Count > 0)
                    {
                        SelQues = SelQues.OrderBy(u => u.SALQ_GRP_CODE).ThenBy(u => u.SALQ_GRP_CODE).ThenBy(u => Convert.ToInt32(u.SALQ_SEQ)).ToList();

                        int rowIndex = 0;
                        foreach (SalquesEntity QEntity in SelQues)
                        {
                            //rowIndex = gvQuestions.Rows.Add(Entity.SALQ_DESC, "", "", Entity.SALQ_ID, Entity.SALQ_SALD_ID, Entity.SALQ_GRP_CODE, Entity.SALQ_GRP_SEQ, Entity.SALQ_CODE, Entity.SALQ_TYPE);

                            if (QEntity.SALQ_SEQ == "0")
                            {
                                PdfPCell A1 = new PdfPCell(new Phrase(QEntity.SALQ_DESC.Trim(), TblFontBold));
                                A1.Colspan = 2;
                                A1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                A1.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(A1);

                                PdfPCell A5 = new PdfPCell(new Phrase("", TblFontBold));
                                A5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                A5.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(A5);
                            }
                            else
                            {
                                PdfPCell A2 = new PdfPCell(new Phrase(QEntity.SALQ_DESC.Trim(), TableFont));
                                A2.Colspan = 2;
                                A2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                A2.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(A2);

                                CALCONTEntity MatResponce = CALCONTList.Find(u => u.CALCONT_Q_ID.Equals(QEntity.SALQ_ID) && u.CALCONT_SALID.Equals(QEntity.SALQ_SALD_ID));
                                if (MatResponce != null)
                                {
                                    if (QEntity.SALQ_TYPE == "T")
                                    {
                                        PdfPCell A3 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(MatResponce.CALCONT_DATE_RESP.Trim()), TableFont));
                                        A3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                        A3.Border = iTextSharp.text.Rectangle.BOX;
                                        TableData.AddCell(A3);
                                    }
                                    else if (QEntity.SALQ_TYPE == "N")
                                    {
                                        PdfPCell A3 = new PdfPCell(new Phrase(MatResponce.CALCONT_NUM_RESP.Trim(), TableFont));
                                        A3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                        A3.Border = iTextSharp.text.Rectangle.BOX;
                                        TableData.AddCell(A3);
                                    }
                                    else if (QEntity.SALQ_TYPE == "D")
                                    {
                                        SalqrespEntity selRespEntity = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(QEntity.SALQ_ID) && u.SALQR_CODE.Trim().Equals(MatResponce.CALCONT_MULTI_RESP.Trim()));
                                        if (selRespEntity != null)
                                        {
                                            PdfPCell A3 = new PdfPCell(new Phrase(selRespEntity.SALQR_DESC, TableFont));
                                            A3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                            A3.Border = iTextSharp.text.Rectangle.BOX;
                                            TableData.AddCell(A3);
                                        }
                                        else
                                        {
                                            PdfPCell A3 = new PdfPCell(new Phrase(MatResponce.CALCONT_MULTI_RESP.Trim(), TableFont));
                                            A3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                            A3.Border = iTextSharp.text.Rectangle.BOX;
                                            TableData.AddCell(A3);
                                        }

                                    }
                                    else if (QEntity.SALQ_TYPE == "C")
                                    {
                                        string custQuestionResp = string.Empty;
                                        List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(QEntity.SALQ_ID));
                                        if (selRespEntity.Count > 0)
                                        {
                                            string response1 = MatResponce.CALCONT_MULTI_RESP;
                                            if (!string.IsNullOrEmpty(response1))
                                            {
                                                string[] arrResponse = null;
                                                if (response1.IndexOf(',') > 0)
                                                {
                                                    arrResponse = response1.Split(',');
                                                }
                                                else if (!response1.Equals(string.Empty))
                                                {
                                                    arrResponse = new string[] { response1 };
                                                }


                                                foreach (string stringitem in arrResponse)
                                                {
                                                    SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                                    if (custRespEntity != null)
                                                    {
                                                        custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                                        //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                    }

                                                }
                                            }
                                        }

                                        PdfPCell A3 = new PdfPCell(new Phrase(custQuestionResp, TableFont));
                                        A3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                        A3.Border = iTextSharp.text.Rectangle.BOX;
                                        TableData.AddCell(A3);




                                    }
                                }
                                else
                                {
                                    PdfPCell A3 = new PdfPCell(new Phrase("", TableFont));
                                    A3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    A3.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(A3);
                                }

                            }

                        }

                    }

                }
                if (TableData.Rows.Count > 0)
                {
                    document.Add(TableData);
                    TableData.DeleteBodyRows();
                }
            }



            document.Close();
            fs.Close();
            fs.Dispose();

        }



    }
}