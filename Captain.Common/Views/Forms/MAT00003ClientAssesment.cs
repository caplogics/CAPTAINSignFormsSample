/************************************************************************
 * Conversion On    :   12/14/2022      * Converted By     :   Kranthi
 * Modified On      :   12/14/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MAT00003ClientAssesment : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();

        private string strMatCode = string.Empty;
        private string strMode = string.Empty;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        private string strCaseWorkerDefaultCode = "0";
        private string strCaseWorkerDefaultStartCode = "0";
        private int intPreviouspoint = 0;
        bool boolpreviousvalue = true;


        int strIndex = 0;

        public MAT00003ClientAssesment(BaseForm baseForm, PrivilegeEntity privilieges, string matCode, string sclCode, string ssDate, string strBenchMarkDesc, string strBenchmarkCode, string strScore, List<MATDEFBMEntity> matadefBMentity, string strBenchMark, string strDateType, string strFamilyseq, string strStatusMode, string strSelYear)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            propSelectYear = strSelYear;
            Privileges = privilieges;
            strMatCode = MatCode;
            strMode = string.Empty;
            ScoreSheet = string.Empty;
            propCopy_PreAssement = string.Empty;
            propssOverideQue = string.Empty;
            SSDate = ssDate;
            ScrCode = sclCode;
            MatCode = matCode;
            propFamseq = strFamilyseq;
            MATADEFBMentity = matadefBMentity;
            propStatusMode = strStatusMode;
            propBenchMark = strBenchMark;
            propDateType = strDateType;
            //LblHeader.Text = "Client Assessment Entry";
            this.Text = "Client Assessment Entry" + " - Matrix/Scale Assessments";
            this.Size = new Size(746, 474);
            this.pnlTop.Size = new Size(744, 150);

            //HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(strAgency, strDept, strProgram);
            //if (HierarchyEntity != null)
            //{
            //    strNameFormat = HierarchyEntity.CNFormat.ToString();
            //    strVerfierFormat = HierarchyEntity.CWFormat.ToString();
            //}
            if (Privileges.AddPriv.Equals("false"))
                btnSave.Visible = false;
            else
                btnSave.Visible = true;

            if (Privileges.ChangePriv.Equals("false"))
                btnSave.Visible = false;
            else
                btnSave.Visible = true;

            //Kranthi 05/18/2023 : Need to look into this later Delete button is visible and disable based on the privliges delete option
            /*
            if (Privileges.DelPriv.Equals("false"))
                gvwQuestions.Columns["QuestionDelete"].Visible = false;
            else
                gvwQuestions.Columns["QuestionDelete"].Visible = true;
            */

            fillcombo();
            GetOutcomeList();

            dtAssessmentDate.Text = SSDate;
            dtAssessmentDate.Enabled = false;
            cmbMatrix.Enabled = false;
            cmbScale.SelectedIndexChanged -= new EventHandler(cmbScale_SelectedIndexChanged);
            CommonFunctions.SetComboBoxValue(cmbMatrix, MatCode);
            cmbMatrix_SelectedIndexChanged(cmbMatrix, new EventArgs());
            CommonFunctions.SetComboBoxValue(cmbScale, ScrCode);

            if (strBenchmarkCode == "0")
            {
                lblViewBenchmark.Text = "NOT APPLICABLE";
                txtBenchMarkDesc.Visible = false;
                this.pnlTop.Size = new Size(744, 98);
            }
            else
            {
                lblViewBenchmark.Text = strBenchMarkDesc;
                MATOUTCEntity matOutEntityDesc = matOutEntityList.Find(u => u.SclCode.Equals(ScrCode) && u.BmCode.Equals(strBenchmarkCode) && u.Points.Equals(strScore));
                if (matOutEntityDesc != null)
                {
                    txtBenchMarkDesc.Text = matOutEntityDesc.Desc.ToString();
                    txtBenchMarkDesc.Visible = true;
                    this.pnlTop.Size = new Size(744, 150);
                }
                else
                {
                    txtBenchMarkDesc.Visible = false;
                    this.pnlTop.Size = new Size(744, 98);
                }
            }
            //FillGridData();

            if (txtBenchMarkDesc.Visible == false && cmbMembers.Visible == false)
                pnlTop.Size = new System.Drawing.Size(this.pnlTop.Width, 65);
            else if (txtBenchMarkDesc.Visible == false && cmbMembers.Visible == true)
                pnlTop.Size = new System.Drawing.Size(this.pnlTop.Width, 98);
        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        public List<MATASMTEntity> matasmtRespcode { get; set; }
        public List<MATASMTEntity> matasmtScaleDetails { get; set; }
        public List<MATASMTEntity> matasmtScaleTypeSDetails { get; set; }
        public MATASMTEntity mataSmtPreviousDate { get; set; }
        public List<MATDEFBMEntity> MATADEFBMentity { get; set; }
        public string MatCode { get; set; }
        public string ScrCode { get; set; }
        public string ScoreSheet { get; set; }
        public string SSDate { get; set; }
        public string QuestionType { get; set; }
        public string GroupCode { get; set; }
        public string propBenchMark { get; set; }
        public string propDateType { get; set; }
        public string propssOverideQue { get; set; }
        public string MenuSelectValue { get; set; }
        public string MenuSelectCode { get; set; }
        public string propFamseq { get; set; }
        public string propCopy_PreAssement { get; set; }
        public MATQUESREntity menuQueserEntity { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }
        public string propStatusMode { get; set; }
        public string propSelectYear { get; set; }

        //  public string ScrCode { get; set; }
        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //   Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "MAT00003");
        }

        List<MATOUTCEntity> matoutcEntity = new List<MATOUTCEntity>();
        List<MATOUTCEntity> matOutEntityList = new List<MATOUTCEntity>();
        private void GetOutcomeList()
        {
            MATOUTCEntity matOutEntity = new MATOUTCEntity(true);
            matOutEntity.MatCode = MatCode;
            // matOutEntity.SclCode = ScrCode;
            matOutEntityList = _model.MatrixScalesData.Browse_MATOUTC(matOutEntity, "Browse");
        }

        List<MATQUESREntity> matQuesrEntityList = new List<MATQUESREntity>();
        private void GetMatQuesr()
        {
            MATQUESREntity matQuesrEntiry = new MATQUESREntity(true);
            matQuesrEntiry.MatCode = MatCode;
            matQuesrEntiry.SclCode = ScrCode;
            matQuesrEntityList = _model.MatrixScalesData.Browse_MATQUESR(matQuesrEntiry, "Browse");

            matQuesrEntityList= matQuesrEntityList.OrderBy(u=>u.RespCode).ToList();

        }

        public void fillcombo()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Scale_Code = "0";
            List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");

            foreach (MATDEFEntity matdef in matdefEntity)
            {
                cmbMatrix.Items.Add(new ListItem(matdef.Desc, matdef.Mat_Code, matdef.Score, matdef.Copy_Prassmnt));
            }
            cmbMatrix.Items.Insert(0, new ListItem("    ", "0"));
            cmbMatrix.SelectedIndex = 0;
            cmbMembers.SelectedIndexChanged -= new EventHandler(cmbMembers_SelectedIndexChanged);
            foreach (CaseSnpEntity snpitem in BaseForm.BaseCaseSnpEntity)
            {
                string ApplicantName = LookupDataAccess.GetMemberName(snpitem.NameixFi, snpitem.NameixMi, snpitem.NameixLast, BaseForm.BaseHierarchyCnFormat);
                cmbMembers.Items.Add(new ListItem(ApplicantName, snpitem.FamilySeq));
            }
            CommonFunctions.SetComboBoxValue(cmbMembers, propFamseq);
            cmbMembers.SelectedIndexChanged += new EventHandler(cmbMembers_SelectedIndexChanged);

        }

        private void cmbMatrix_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMatrix.SelectedIndex != 0)
            {
                cmbScale.Items.Clear();
                MatCode = ((ListItem)cmbMatrix.SelectedItem).Value.ToString();
                ScoreSheet = ((ListItem)cmbMatrix.SelectedItem).ID.ToString();
                propCopy_PreAssement = ((ListItem)cmbMatrix.SelectedItem).ValueDisplayCode.ToString();
                List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Get_Matdef_MatCode(((ListItem)cmbMatrix.SelectedItem).Value.ToString());
                matdefEntity = matdefEntity.FindAll(u => u.Sequence != string.Empty);
                matdefEntity = matdefEntity.OrderBy(u => Convert.ToInt32(u.Sequence)).ToList();
                foreach (MATDEFEntity matdef in matdefEntity)
                {
                    cmbScale.Items.Add(new ListItem(matdef.Desc, matdef.Scale_Code, matdef.Assessment_Type, matdef.Score));
                }
                cmbScale.Items.Insert(0, new ListItem("    ", "0"));
                cmbScale.SelectedIndex = 0;
            }
            else
            {
                cmbScale.Items.Clear();
                cmbScale.Items.Insert(0, new ListItem("    ", "0"));
                cmbScale.SelectedIndex = 0;
            }
        }

        string DeleteImage = Consts.Icons.ico_Delete;

        string strScaleAssType = string.Empty;
        string strFamSeq = string.Empty;
        private void cmbScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbScale.SelectedIndex != 0)
            {
                GetMatasmtData(string.Empty);
            }
            else
            {
                //gvwOutComes.Rows.Clear();
                chkBypassScale.Checked = false;
                lblViewBenchmark.Text = string.Empty;
                txtBenchMarkDesc.Text = string.Empty;
                txtBenchMarkDesc.Visible = false;
                this.pnlTop.Size = new Size(744, 98);
                lblScore.Visible = false;
                txtScore.Visible = false;
                gvwQuestions.Rows.Clear();
                btnSave.Enabled = false;
                //pnllistQuestions.Enabled = false;
                //gvwOutComes.Enabled = false;
                //gvwQuestions.Enabled = false;
            }

            if (gvwQuestions.Rows.Count > 0)
            {
                btnSave.Enabled = true;
            }
            else
                btnSave.Enabled = false;

        }

        public void GetMatasmtData(string strfamilyType)
        {
            propssOverideQue = string.Empty;
            lblChangeDimesion.Visible = false;
            gvwQuestions.Enabled = true;
            chkBypassScale.Checked = false;
            btnReasonCodes.Visible = false;
            gvwQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
            cmbMembers.SelectedIndexChanged -= new EventHandler(cmbMembers_SelectedIndexChanged);
            gvwQuestions.Rows.Clear();

            /**********************************************/
            if (gvwQuestions.Columns.Count == 0)
            {
                gvwQuestions.Columns.Add("Code");
                gvwQuestions.Columns.Add("Question");
                gvwQuestions.Columns.Add("Responce");
                gvwQuestions.Columns.Add("GroupQues");
                gvwQuestions.Columns.Add("QuestionCode");
                gvwQuestions.Columns.Add("GrpType");
                gvwQuestions.Columns.Add("QuestType");
                gvwQuestions.Columns.Add("ResponceCode");
                gvwQuestions.Columns.Add("Points");
                gvwQuestions.Columns.Add("QuestionDelete");
                gvwQuestions.Columns.Add("QuesrExclude");
                gvwQuestions.Columns.Add("SSOverride");
                gvwQuestions.Columns.Add("gvtQuesNType");
            }
            /**********************************************/


            // lblViewBenchmark.Text = string.Empty;
            //txtBenchMarkDesc.Visible = false;
            GetMatQuesr();
            btnSave.Enabled = true;
            ScrCode = ((ListItem)cmbScale.SelectedItem).Value.ToString();
            strScaleAssType = ((ListItem)cmbScale.SelectedItem).ID.ToString();
            mataSmtPreviousDate = null;
            cmbMembers.Visible = false;
            lblContactName.Visible = false;
            pnlScaleType.Visible = false;
            if (((ListItem)cmbScale.SelectedItem).ValueDisplayCode.ToString() == "Y")
            {
                chkBypassScale.Visible = false;
            }
            else
            {
                chkBypassScale.Visible = true;
            }

            if (BaseForm.BaseAgencyControlDetails.MatAssesment == "Y")
            {
                strFamSeq = propFamseq;
                if (strScaleAssType == "B")
                {
                    pnlScaleType.Visible = true;
                    if (strFamSeq == "0")
                    {
                        rdoHouseHold.Checked = true;
                    }
                    else
                    {
                        rdoIndividual.Checked = true;
                        if (rdoIndividual.Checked)
                        {
                            cmbMembers.Visible = true;
                            lblContactName.Visible = true;
                            if (strfamilyType == "MEMBER")
                            {
                                strFamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                            }
                        }
                        else
                        {
                            strFamSeq = "0";
                        }
                    }
                }
                else if (strScaleAssType == "I")
                {
                    cmbMembers.Visible = true;
                    lblContactName.Visible = true;
                    if (strfamilyType == "MEMBER")
                    {
                        strFamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                    }

                }
                else
                {
                    strFamSeq = "0";//BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                }
            }
            else
            {
                strFamSeq = "0";
            }
            // matasmtScaleDetails = _model.MatrixScalesData.GETMatasmt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, propSelectYear, BaseForm.BaseApplicationNo, MatCode, ScrCode, string.Empty, string.Empty, string.Empty, strFamSeq); //(((ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((ListItem)cmbScale.SelectedItem).Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Benchmarkcode"].Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Outcomecode"].Value.ToString(), string.Empty, "MATOUTR");
            matasmtScaleDetails = _model.MatrixScalesData.GETMatasmt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, BaseForm.BaseApplicationNo, MatCode, ScrCode, string.Empty, string.Empty, string.Empty, strFamSeq); //(((ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((ListItem)cmbScale.SelectedItem).Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Benchmarkcode"].Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Outcomecode"].Value.ToString(), string.Empty, "MATOUTR");

            matasmtScaleTypeSDetails = matasmtScaleDetails.FindAll(u => u.Type.Equals("S"));
            matasmtRespcode = matasmtScaleDetails.FindAll(u => LookupDataAccess.Getdate(u.SSDate).Equals(SSDate));
            //int intssdateIndex = matasmtScaleTypeSDetails.FindIndex(u => LookupDataAccess.Getdate(u.SSDate).Equals(SSDate));
            //if (intssdateIndex > 0)
            //{
            //    mataSmtPreviousDate = matasmtScaleTypeSDetails[intssdateIndex - 1];
            //}
            //else
            //{
            List<MATASMTEntity> matasmtstypeDateEntity = matasmtScaleTypeSDetails.FindAll(u => Convert.ToDateTime(u.SSDate).Date < Convert.ToDateTime(SSDate).Date);      //FindIndex(u => LookupDataAccess.Getdate(u.SSDate).Equals(SSDate));
            if (matasmtstypeDateEntity.Count > 0)
                mataSmtPreviousDate = matasmtstypeDateEntity[matasmtstypeDateEntity.Count - 1];

            // }

            // List<MATASMTEntity> matasmtRespcode = _model.MatrixScalesData.GETMatasmt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, MatCode, ScrCode, string.Empty, SSDate, string.Empty); //(((ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((ListItem)cmbScale.SelectedItem).Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Benchmarkcode"].Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Outcomecode"].Value.ToString(), string.Empty, "MATOUTR");
            //List<MATASMTEntity> matasmtRespDetails = matasmtRespcode.FindAll(u => u.Type.Equals("D"));
            matoutcEntity = _model.MatrixScalesData.GetMATOUTCMatCode(MatCode, ScrCode, "MATDEFBM");

            MATSGRPEntity Search_Entity = new MATSGRPEntity(true);
            Search_Entity.MatCode = ((ListItem)cmbMatrix.SelectedItem).Value.ToString();
            Search_Entity.SclCode = ((ListItem)cmbScale.SelectedItem).Value.ToString();
            QuesGroups_List = _model.MatrixScalesData.Browse_MATSGRP(Search_Entity, "Browse");
            int rowIndex = 0;
            foreach (MATSGRPEntity MATSGRPEntityitem in QuesGroups_List)
            {


                rowIndex = gvwQuestions.Rows.Add();

                #region ColumnHeaders
                for (int x = 0; x < 13; x++)
                {
                    int HeadcolIndex = x;

                    if (x == 9)
                    {
                        DataGridViewImageCell gvCellDel = new DataGridViewImageCell();
                        this.gvwQuestions[HeadcolIndex, rowIndex] = gvCellDel;
                        this.gvwQuestions[HeadcolIndex, rowIndex].Value = string.Empty;
                        this.gvwQuestions.Columns[HeadcolIndex].Visible = true;
                        this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "Delete";
                        this.gvwQuestions.Columns[HeadcolIndex].Width = 70;
                    }
                    else
                    {
                        DataGridViewCell gvCell = new DataGridViewCell();
                        //gvCell.Style.ForeColor = System.Drawing.Color.Red;
                        this.gvwQuestions[HeadcolIndex, rowIndex] = gvCell;
                        this.gvwQuestions[HeadcolIndex, rowIndex].Value = string.Empty;
                        gvCell.Style.Padding = new System.Windows.Forms.Padding(2);
                        this.gvwQuestions.Columns[HeadcolIndex].Visible = false;
                        this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "";

                        if (x == 1)
                        {
                            this.gvwQuestions.Columns[HeadcolIndex].Visible = true;
                            this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "Question";
                            this.gvwQuestions.Columns[HeadcolIndex].Width = 320;
                            this.gvwQuestions.Columns[HeadcolIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            this.gvwQuestions.Columns[HeadcolIndex].HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            this.gvwQuestions.Columns[HeadcolIndex].ReadOnly = false;
                        }
                        if (x == 2)
                        {
                            this.gvwQuestions.Columns[HeadcolIndex].Visible = true;
                            this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "Response";
                            this.gvwQuestions.Columns[HeadcolIndex].Width = 300;
                        }

                    }


                }
                #endregion

                gvwQuestions.Rows[rowIndex].Cells["Code"].Value = "";
                gvwQuestions.Rows[rowIndex].Cells["Question"].Value = MATSGRPEntityitem.Desc; this.gvwQuestions["Question", rowIndex].ToolTipText = MATSGRPEntityitem.Desc;
                gvwQuestions.Rows[rowIndex].Cells["Responce"].Value = "";
                gvwQuestions.Rows[rowIndex].Cells["GroupQues"].Value = MATSGRPEntityitem.Group;
                gvwQuestions.Rows[rowIndex].Cells["QuestionCode"].Value = "";
                gvwQuestions.Rows[rowIndex].Cells["GrpType"].Value = MATSGRPEntityitem.GrpType;
                gvwQuestions.Rows[rowIndex].Cells["QuestType"].Value = "";
                gvwQuestions.Rows[rowIndex].Cells["ResponceCode"].Value = "Responce";
                gvwQuestions.Rows[rowIndex].Cells["Points"].Value = "";

                gvwQuestions.Rows[rowIndex].Cells["QuesrExclude"].Value = "";
                gvwQuestions.Rows[rowIndex].Cells["SSOverride"].Value = "";
                gvwQuestions.Rows[rowIndex].Cells["gvtQuesNType"].Value = "";

                //rowIndex = gvwQuestions.Rows.Add(string.Empty, MATSGRPEntityitem.Desc, string.Empty, MATSGRPEntityitem.Group, string.Empty, MATSGRPEntityitem.GrpType, string.Empty, "Responce", string.Empty);

                gvwQuestions.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                gvwQuestions.Rows[rowIndex].Cells["Question"].Style.ForeColor = Color.Blue;

                gvwQuestions.Rows[rowIndex].Cells["SSOverride"].Value = string.Empty;
                gvwQuestions.Rows[rowIndex].Cells["QuesrExclude"].Value = string.Empty;
                gvwQuestions.Rows[rowIndex].Cells["gvtQuesNType"].Value = string.Empty;

                List<MATQUESTEntity> matquestEntity = _model.MatrixScalesData.GETMATQUESTQuestions(((ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((ListItem)cmbScale.SelectedItem).Value.ToString(), MATSGRPEntityitem.Group, string.Empty, string.Empty, "ALL", string.Empty);
                bool boolQuesdisplay = true;
                foreach (MATQUESTEntity matques in matquestEntity)
                {
                    if (matques.QuesActive == "N")
                    {
                        MATASMTEntity MatResponce = matasmtRespcode.Find(u => u.QuesCode.Equals(matques.Code.Trim()) && u.Type.Equals("D"));
                        if (MatResponce != null)
                        {
                            boolQuesdisplay = true;
                        }
                        else
                            boolQuesdisplay = false;
                    }
                    else
                    {
                        boolQuesdisplay = true;
                    }

                    if (boolQuesdisplay)
                    {

                        rowIndex = gvwQuestions.Rows.Add();

                        #region ColumnHeaders
                        for (int x = 0; x < 13; x++)
                        {
                            int HeadcolIndex = x;

                            if (x == 9)
                            {
                                DataGridViewImageCell gvCellDel = new DataGridViewImageCell();
                                this.gvwQuestions[HeadcolIndex, rowIndex] = gvCellDel;
                                this.gvwQuestions[HeadcolIndex, rowIndex].Value = string.Empty;
                                this.gvwQuestions.Columns[HeadcolIndex].Visible = true;
                                this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "Delete";
                                this.gvwQuestions.Columns[HeadcolIndex].Width = 70;
                            }
                            else
                            {
                                DataGridViewCell gvCell = new DataGridViewCell();
                                //gvCell.Style.ForeColor = System.Drawing.Color.Red;
                                this.gvwQuestions[HeadcolIndex, rowIndex] = gvCell;
                                this.gvwQuestions[HeadcolIndex, rowIndex].Value = string.Empty;
                                gvCell.Style.Padding = new System.Windows.Forms.Padding(2);
                                this.gvwQuestions.Columns[HeadcolIndex].Visible = false;
                                this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "";

                                if (x == 1)
                                {
                                    this.gvwQuestions.Columns[HeadcolIndex].Visible = true;
                                    this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "Question";
                                    this.gvwQuestions.Columns[HeadcolIndex].Width = 320;
                                    this.gvwQuestions.Columns[HeadcolIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                                    this.gvwQuestions.Columns[HeadcolIndex].HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                }
                                if (x == 2)
                                {
                                    this.gvwQuestions.Columns[HeadcolIndex].Visible = true;
                                    this.gvwQuestions.Columns[HeadcolIndex].HeaderText = "Response";
                                    this.gvwQuestions.Columns[HeadcolIndex].Width = 300;
                                }

                            }


                        }
                        #endregion

                        gvwQuestions.Rows[rowIndex].Cells["Code"].Value = matques.Code;
                        gvwQuestions.Rows[rowIndex].Cells["Question"].Value = matques.Desc; this.gvwQuestions["Question", rowIndex].ToolTipText = matques.Desc;
                        gvwQuestions.Rows[rowIndex].Cells["Responce"].Value = "";
                        gvwQuestions.Rows[rowIndex].Cells["GroupQues"].Value = matques.Group;
                        gvwQuestions.Rows[rowIndex].Cells["QuestionCode"].Value = matques.Code;
                        gvwQuestions.Rows[rowIndex].Cells["GrpType"].Value = MATSGRPEntityitem.GrpType;
                        gvwQuestions.Rows[rowIndex].Cells["QuestType"].Value = matques.Type;
                        gvwQuestions.Rows[rowIndex].Cells["ResponceCode"].Value = "";
                        gvwQuestions.Rows[rowIndex].Cells["Points"].Value = "";

                        //rowIndex = gvwQuestions.Rows.Add(matques.Code, matques.Desc, string.Empty, matques.Group, matques.Code, MATSGRPEntityitem.GrpType, matques.Type, string.Empty, string.Empty);

                        gvwQuestions.Rows[rowIndex].Cells["QuesrExclude"].Value = string.Empty;
                        gvwQuestions.Rows[rowIndex].Cells["SSOverride"].Value = matques.SsOverride.ToString();
                        gvwQuestions.Rows[rowIndex].Cells["gvtQuesNType"].Value = matques.QuesNumericType.ToString();
                        // if (matques.Type.ToString() == "1" || matques.Type.ToString() == "2" || matques.Type.ToString() == "3" || matques.Type.ToString() == "4")
                        //  gvwQuestions.Rows[rowIndex].Cells["Responce"].ReadOnly = true;

                        if (matques.QuesActive == "N")
                        {
                            gvwQuestions.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }

                    #region MIXED Controls

                    if (matques.Type == "5" || matques.Type == "6" || matques.Type == "7")
                    {
                        DataGridViewTextBoxCell TextBoxCell = new DataGridViewTextBoxCell();
                        this.gvwQuestions["Responce", rowIndex] = TextBoxCell;
                        this.gvwQuestions["Responce", rowIndex].Value = "";
                        if (matques.Type == "5") // Numeric(W)
                            this.gvwQuestions["Responce", rowIndex].ToolTipText = "Question Type: Numeric(W)";
                        if (matques.Type == "6") // Numeric(D)
                            this.gvwQuestions["Responce", rowIndex].ToolTipText = "Question Type: Numeric(D)";
                        if (matques.Type == "7") // Alphanumeric
                            this.gvwQuestions["Responce", rowIndex].ToolTipText = "Question Type: Alphanumeric";

                        TextBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";
                    }


                    if (matques.Type == "4") // Date
                    {
                        DataGridViewDateTimePickerCell Response = new DataGridViewDateTimePickerCell();
                        Response.Format = DateTimePickerFormat.Short;
                        Response.Style.BackgroundImageSource = "icon-calendar";
                        Response.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                        this.gvwQuestions["Responce", rowIndex] = Response;
                        this.gvwQuestions["Responce", rowIndex].Value = string.Empty;
                        this.gvwQuestions["Responce", rowIndex].ToolTipText = "Question Type: Date";
                        Response.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";

                    }

                    if (matques.Type == "1") // Dropdown
                    {
                        DataGridViewComboBoxCell ComboBoxCell = new DataGridViewComboBoxCell();
                        //if (dr.CUSTACTIVECUST.ToUpper() == "A")
                        //    fillgridComboResp(ComboBoxCell, fieldType, custCode, "");

                        ComboBoxCell.Style.BackgroundImageSource = "combo-arrow";
                        ComboBoxCell.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                        ComboBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px; ";

                        this.gvwQuestions["Responce", rowIndex] = ComboBoxCell;
                        this.gvwQuestions["Responce", rowIndex].ToolTipText = "Question Type: Drop down";

                        List<MATQUESREntity> matresponceEntity = _model.MatrixScalesData.GETMATQUESR(MatCode, ScrCode, matques.Group, matques.Code, string.Empty);
                        //if (matques.QuesNumericType.ToString() == "Y") {
                        //    try
                        //    {
                        //        matresponceEntity = matresponceEntity.OrderBy(u => Convert.ToInt32(u.RespDesc)).ToList();
                        //    }
                        //    catch (Exception ex)
                        //    {


                        //    }
                        //}

                        if (matresponceEntity.Count > 0)
                        {
                            matresponceEntity= matresponceEntity.OrderBy(u=>u.RespCode).ToList();

                            gvwQuestions.Rows[rowIndex].Cells["Responce"].Tag = matresponceEntity;

                            ComboBoxCell.DataSource = matresponceEntity;
                            ComboBoxCell.DisplayMember = "RespDesc";
                            ComboBoxCell.ValueMember = "RespCode";
                            //foreach (MATQUESREntity matresp in matresponceEntity)
                            //{
                            //    string strValue = ""; string strText = "";
                            //    if (matresp.RespDesc.ToString().Length > 140)
                            //    {
                            //        strText = matresp.RespDesc.ToString().Trim().Substring(0, 139);

                            //    }
                            //    else
                            //    {
                            //        strText = matresp.RespDesc.ToString().Trim();
                            //    }
                            //}


                        }
                    }
                    if (matques.Type == "2" || matques.Type == "3")
                    {
                        DataGridViewButtonCell Response = new DataGridViewButtonCell();
                        Response.Style.ForeColor = System.Drawing.Color.White;
                        // contextMenu1_Popup(null, Response, fieldType, custCode);
                        this.gvwQuestions["Responce", rowIndex] = Response;
                        this.gvwQuestions["Responce", rowIndex].Value = string.Empty;

                        if (matques.Type == "2")    // Checkbox
                            this.gvwQuestions["Responce", rowIndex].ToolTipText = "Question Type: Check Box";
                        if (matques.Type == "3")    // Radiobutton
                            this.gvwQuestions["Responce", rowIndex].ToolTipText = "Question Type: Radiobutton";

                    }

                    #endregion


                }
                // List<MATASMTEntity> matasmtRespcode = _model.MatrixScalesData.GETMatasmt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, MatCode, ScrCode, string.Empty, SSDate, string.Empty); //(((ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((ListItem)cmbScale.SelectedItem).Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Benchmarkcode"].Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Outcomecode"].Value.ToString(), string.Empty, "MATOUTR");
                if (matasmtRespcode.Count > 0)
                {
                    foreach (DataGridViewRow gvrows in gvwQuestions.Rows)
                    {
                        MATASMTEntity MatResponce = matasmtRespcode.Find(u => u.QuesCode.Equals(gvrows.Cells["Code"].Value.ToString().Trim()) && u.Type.Equals("D"));

                        if (MatResponce != null)
                        {
                            if (gvrows.Cells["SSOverride"].Value.ToString() == "1")
                            {
                                gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                propssOverideQue = "1";
                            }

                            if (gvrows.Cells["QuestType"].Value.ToString() == "4")
                            {
                                string strDtValue = MatResponce.RespDesc;
                                if (strDtValue != "")
                                    strDtValue = Convert.ToDateTime(MatResponce.RespDesc).ToString("MM/dd/yyyy");

                                gvrows.Cells["Responce"].Value = strDtValue;
                            }
                            else
                                gvrows.Cells["Responce"].Value = MatResponce.RespDesc;


                            gvrows.Cells["Points"].Value = MatResponce.Points;
                            gvrows.Cells["ResponceCode"].Value = MatResponce.RespCode;
                            gvrows.Cells["QuestionDelete"].Value = DeleteImage;
                            MATQUESREntity matQuesrEntity = matQuesrEntityList.Find(u => u.Code.Equals(gvrows.Cells["Code"].Value.ToString().Trim()) && u.RespCode.Equals(MatResponce.RespCode));
                            if (matQuesrEntity != null)
                                gvrows.Cells["QuesrExclude"].Value = matQuesrEntity.Exclude;
                        }
                        else
                        {
                            string strresp = gvrows.Cells["Responce"].Value == null ? string.Empty : gvrows.Cells["Responce"].Value.ToString();
                            if (strresp != string.Empty)
                            {
                                gvrows.Cells["QuestionDelete"].Value = DeleteImage;
                            }
                        }
                    }
                }
                else
                {
                    txtScore.Text = string.Empty;
                    txtScore.Visible = false;
                    lblScore.Visible = false;
                }

            }
            if (matasmtRespcode.Count > 0)
            {
                MATASMTEntity MatResponcebypass = matasmtRespcode.Find(u => u.QuesCode.Equals("0") && u.Type.Equals("S"));
                if (MatResponcebypass != null)
                {
                    txtScore.Visible = true;
                    txtScore.Text = MatResponcebypass.Points;
                    intPreviouspoint = Convert.ToInt32(MatResponcebypass.Points);
                    lblScore.Visible = true;
                    boolpreviousvalue = false;
                    if (MatResponcebypass.ByPass.Equals("Y"))
                        chkBypassScale.Checked = true;
                    else
                        chkBypassScale.Checked = false;
                    if (propBenchMark == "Y")
                    {
                        int intchange = 0;
                        if (MatResponcebypass.Change != string.Empty)
                            intchange = Convert.ToInt32(MatResponcebypass.Change);
                        if ((intchange == 0 && MatResponcebypass.Pn != string.Empty) || (intchange > 0 && MatResponcebypass.Pn != "P" && MatResponcebypass.Pn != string.Empty) || (intchange < 0 && MatResponcebypass.Pn != "N" && MatResponcebypass.Pn != string.Empty))
                        {
                            if (BaseForm.BaseAgencyControlDetails.AgyShortName.Trim().ToUpper() != "TIOGA")
                            {
                                CommonFunctions.MessageBoxDisplay("A change in dimension from (+ to -) or (- to +) has occurred for  this assessment \n and reasons were previously selected.  Please review and select applicable reasons.");
                                lblChangeDimesion.Visible = true;
                                btnReasonCodes.Visible = true;
                                if (intchange > 0)
                                    lblChangeDimesion.Text = "Change in Dimension +" + intchange;
                                else
                                    lblChangeDimesion.Text = "Change in Dimension " + intchange;
                            }
                        }
                        else
                        {
                            lblChangeDimesion.Visible = false;
                            btnReasonCodes.Visible = false;
                        }
                    }

                    ShowBenchmarkDescription(MatResponcebypass.Points, string.Empty, string.Empty, "Change", MatResponcebypass.RespExcel, MatResponcebypass.PointsSwitch);

                }
            }
            else
            {
                txtScore.Text = string.Empty;
                txtBenchMarkDesc.Text = string.Empty;
                lblViewBenchmark.Text = string.Empty;
                txtScore.Visible = false;
                lblScore.Visible = false;
                txtBenchMarkDesc.Visible = false;
                this.pnlTop.Size = new Size(744, 98);

                if (((ListItem)cmbMatrix.SelectedItem).ValueDisplayCode.ToString() == "Y")
                {

                    if (mataSmtPreviousDate != null)
                    {
                        List<MATASMTEntity> matasmtpreviousRespcode = matasmtScaleDetails.FindAll(u => LookupDataAccess.Getdate(u.SSDate).Equals(LookupDataAccess.Getdate(mataSmtPreviousDate.SSDate)));
                        if (matasmtpreviousRespcode.Count > 0)
                        {
                            CommonFunctions.MessageBoxDisplay("Copying Assessment Responses from " + LookupDataAccess.Getdate(mataSmtPreviousDate.SSDate));
                            foreach (DataGridViewRow gvrows in gvwQuestions.Rows)
                            {
                                MATASMTEntity MatResponce = matasmtpreviousRespcode.Find(u => u.QuesCode.Equals(gvrows.Cells["Code"].Value.ToString().Trim()) && u.Type.Equals("D"));

                                if (MatResponce != null)
                                {
                                    if (gvrows.Cells["SSOverride"].Value.ToString() == "1")
                                    {
                                        gvrows.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                        propssOverideQue = "1";
                                    }
                                    gvrows.Cells["Responce"].Value = MatResponce.RespDesc;
                                    gvrows.Cells["Points"].Value = MatResponce.Points;
                                    gvrows.Cells["ResponceCode"].Value = MatResponce.RespCode;
                                    gvrows.Cells["QuestionDelete"].Value = DeleteImage;
                                    MATQUESREntity matQuesrEntity = matQuesrEntityList.Find(u => u.Code.Equals(gvrows.Cells["Code"].Value.ToString().Trim()) && u.RespCode.Equals(MatResponce.RespCode));
                                    if (matQuesrEntity != null)
                                        gvrows.Cells["QuesrExclude"].Value = matQuesrEntity.Exclude;
                                }
                                else
                                {
                                    string strresp = gvrows.Cells["Responce"].Value == null ? string.Empty : gvrows.Cells["Responce"].Value.ToString();
                                    if (strresp != string.Empty)
                                    {
                                        gvrows.Cells["QuestionDelete"].Value = DeleteImage;
                                    }
                                }
                            }
                        }
                    }

                }

            }
            if (cmbMembers.Visible)
            {
                if (cmbMembers.Items.Count > 0)
                {
                    if (cmbMembers.Text == null || cmbMembers.Text == "")
                    {
                        gvwQuestions.Enabled = false;
                    }

                }
            }
            gvwQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
            cmbMembers.SelectedIndexChanged += new EventHandler(cmbMembers_SelectedIndexChanged);

            if (gvwQuestions.Rows.Count > 0)
                gvwQuestions.Rows[0].Selected = true;
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {

            contextMenu1.MenuItems.Clear();
            if (gvwQuestions.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwQuestions.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        if (dr.Cells["QuestType"].Value.ToString() == "4")
                        {
                            MenuItem menuItem = new MenuItem();
                            menuItem.Text = "Please enter Date here";
                            menuItem.Tag = "4"; // menuItem.Tag = "A";
                            contextMenu1.MenuItems.Add(menuItem);
                            dr.Cells["Responce"].ReadOnly = false;
                            if (chkBypassScale.Checked == true)
                                menuItem.Enabled = false;

                        }
                        else if (dr.Cells["QuestType"].Value.ToString() == "5" || dr.Cells["QuestType"].Value.ToString() == "6")
                        {
                            MenuItem menuItem = new MenuItem();
                            menuItem.Text = "Please enter number here";
                            menuItem.Tag = "5"; // menuItem.Tag = "A";
                            contextMenu1.MenuItems.Add(menuItem);
                            dr.Cells["Responce"].ReadOnly = false;
                            if (chkBypassScale.Checked == true)
                                menuItem.Enabled = false;
                        }
                        else if (dr.Cells["QuestType"].Value.ToString() == "7")
                        {
                            MenuItem menuItem = new MenuItem();
                            menuItem.Text = "Please enter text here";
                            menuItem.Tag = "7"; // menuItem.Tag = "A";
                            contextMenu1.MenuItems.Add(menuItem);
                            dr.Cells["Responce"].ReadOnly = false;
                            if (chkBypassScale.Checked == true)
                                menuItem.Enabled = false;

                        }
                        else
                        {
                            dr.Cells["Responce"].ReadOnly = true;
                            List<MATQUESREntity> matresponceEntity = _model.MatrixScalesData.GETMATQUESR(MatCode, ScrCode, GroupCode, GetSelectedRow(), string.Empty);
                            if (dr.Cells["gvtQuesNType"].Value.ToString() == "Y")
                            {
                                try
                                {
                                    matresponceEntity = matresponceEntity.OrderBy(u => Convert.ToInt32(u.RespDesc)).ToList();
                                }
                                catch (Exception ex)
                                {


                                }
                            }
                            foreach (MATQUESREntity matresp in matresponceEntity)
                            {
                                MenuItem menuItem = new MenuItem();
                                if (matresp.RespDesc.ToString().Length > 140)
                                {
                                    menuItem.Text = matresp.RespDesc.ToString().Trim().Substring(0, 139);

                                }
                                else
                                {
                                    menuItem.Text = matresp.RespDesc.ToString().Trim();
                                }
                                menuItem.Tag = matresp;
                                contextMenu1.MenuItems.Add(menuItem);
                                if (chkBypassScale.Checked == true)
                                    menuItem.Enabled = false;

                            }
                            //contextMenu1.Popup -=new EventHandler(contextMenu1_Popup);
                            //contextMenu1.Show(gvwQuestions, new System.Drawing.Point(0, 0), DialogAlignment.Right);
                            //contextMenu1.Popup += new EventHandler(contextMenu1_Popup);
                        }

                    }
                }
                contextMenu1.Update();
                // this.ShowPopup(gvwQuestions, DialogAlignment.Right);
            }
        }

        //private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    Point objPoint = getLocation(this.gvwQuestions.Rows[e.RowIndex].Cells[e.ColumnIndex]);

        //    contextMenu1.Show(this.gvwQuestions, objPoint);
        //}

        //public Point getLocation(object obj)
        //{
        //    System.Reflection.PropertyInfo objProperty = obj.GetType().GetProperty("Location", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //    if (objProperty == null)
        //        return new Point();
        //    else
        //        return (Point)objProperty.GetValue(obj, null);
        //}

        private void gvwQuestions_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string DeleteImage = Consts.Icons.ico_Delete;
            if (objArgs.MenuItem.Tag is MATQUESREntity)
            {
                MATQUESREntity dr = (MATQUESREntity)objArgs.MenuItem.Tag as MATQUESREntity;
                string selectedValue = objArgs.MenuItem.Text;
                string selectedCode = dr.RespCode.ToString();
                menuQueserEntity = dr;
                MenuSelectCode = selectedCode;
                MenuSelectValue = selectedValue;
                if (propssOverideQue == "1")
                {
                    if (gvwQuestions.SelectedRows[0].Cells["SSOverride"].Value.ToString() != "1")
                    {
                        MessageBox.Show("Response to Question does not require the rest of questions in the scale to be answered \n Still You want to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerMenu);
                    }
                }
                else
                {
                    if (objArgs.MenuItem.Checked)
                    {
                        //responseValue = selectedValue;
                        //responseCode = selectedCode;
                        gvwQuestions.SelectedRows[0].Cells[2].Value = selectedValue;
                        gvwQuestions.SelectedRows[0].Cells[2].Tag = selectedCode;
                        gvwQuestions.SelectedRows[0].Cells["ResponceCode"].Value = selectedCode;
                        gvwQuestions.SelectedRows[0].Cells["QuestionDelete"].Value = DeleteImage;
                        gvwQuestions.SelectedRows[0].Cells["QuesrExclude"].Value = dr.Exclude;
                        gvwQuestions.SelectedRows[0].Cells["Points"].Value = dr.Points;
                    }
                    else
                    {

                        gvwQuestions.SelectedRows[0].Cells[2].Value = selectedValue;
                        gvwQuestions.SelectedRows[0].Cells[2].Tag = selectedCode;
                        gvwQuestions.SelectedRows[0].Cells["ResponceCode"].Value = selectedCode;
                        gvwQuestions.SelectedRows[0].Cells["Points"].Value = dr.Points;
                        gvwQuestions.SelectedRows[0].Cells["QuestionDelete"].Value = DeleteImage;
                        gvwQuestions.SelectedRows[0].Cells["QuesrExclude"].Value = dr.Exclude;

                    }
                    if (gvwQuestions.SelectedRows[0].Cells["SSOverride"].Value.ToString() == "1")
                    {
                        gvwQuestions.SelectedRows[0].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        propssOverideQue = "1";
                    }
                }

                //MATASMTEntity matasmtentity = new MATASMTEntity();

                //matasmtentity.Agency = BaseForm.BaseAgency;
                //matasmtentity.Dept = BaseForm.BaseDept;
                //matasmtentity.Program = BaseForm.BaseProg;
                //matasmtentity.Year = BaseForm.BaseYear;
                //matasmtentity.App = BaseForm.BaseApplicationNo;
                //matasmtentity.MatCode = MatCode;
                //matasmtentity.SclCode = ScrCode;
                //matasmtentity.Type = "D";
                //matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                ////matoutResponce.BmCode = BMcode;
                ////matoutResponce.Code = OutrCode;
                //matasmtentity.QuesCode = gvwQuestions.SelectedRows[0].Cells[0].Value.ToString();
                //matasmtentity.RespCode = selectedCode;
                //matasmtentity.RespDesc = dr.RespDesc;
                //matasmtentity.Points = dr.Points;
                //if (chkBypassScale.Checked == true)
                //{
                //    matasmtentity.ByPass = "Y";
                //}
                //else
                //{
                //    matasmtentity.ByPass = "N";
                //}
                //if (Convert.ToInt32(dr.Points) > 0)
                //{
                //    matasmtentity.Pn = "P";
                //}
                //else if (Convert.ToInt32(dr.Points) < 0)
                //{
                //    matasmtentity.Pn = "N";
                //}
                //else
                //{
                //    matasmtentity.Pn = " ";
                //}
                //matasmtentity.AddOperator = BaseForm.UserID;
                //matasmtentity.LstcOperator = BaseForm.UserID;
                //matasmtentity.Mode = string.Empty;

                //if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                //{
                //    gvwQuestions.SelectedRows[0].Cells["QuestionDelete"].Value = DeleteImage;
                //}


            }

        }
        string strMenustatus = string.Empty;
        private void MessageBoxHandlerMenu(DialogResult dialogResult)
        {

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                gvwQuestions.SelectedRows[0].Cells[2].Value = MenuSelectValue;
                gvwQuestions.SelectedRows[0].Cells[2].Tag = MenuSelectCode;
                gvwQuestions.SelectedRows[0].Cells["ResponceCode"].Value = MenuSelectCode;
                gvwQuestions.SelectedRows[0].Cells["QuestionDelete"].Value = DeleteImage;
                gvwQuestions.SelectedRows[0].Cells["QuesrExclude"].Value = menuQueserEntity.Exclude;
                gvwQuestions.SelectedRows[0].Cells["Points"].Value = menuQueserEntity.Points;
                if (gvwQuestions.SelectedRows[0].Cells["SSOverride"].Value.ToString() == "1")
                    gvwQuestions.SelectedRows[0].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }


        }

        string MatRCQuesID = "";
        private void gvwQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex == 9 && e.RowIndex != -1)
            {
                if (GetSelectedRow() != null)
                {
                    if (gvwQuestions.SelectedRows[0].Cells["Responce"].Value != string.Empty &&
                        senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].CellRenderer == "ImageCell" && e.RowIndex != -1)
                    {
                        string BlankImage = Consts.Icons.ico_Blank;
                        gvwQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                        gvwQuestions.SelectedRows[0].Cells["Responce"].Value = string.Empty;
                        gvwQuestions.SelectedRows[0].Cells["Responcecode"].Value = string.Empty;
                        gvwQuestions.SelectedRows[0].Cells["Points"].Value = string.Empty;
                        gvwQuestions.SelectedRows[0].Cells["QuestionDelete"].Value = BlankImage;
                        gvwQuestions.SelectedRows[0].Cells["QuesrExclude"].Value = string.Empty; ;
                        gvwQuestions.SelectedRows[0].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        propssOverideQue = string.Empty;
                        gvwQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                    }
                    // MessageBox.Show("Are you sure want clear Responce", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandler, true);
                }
            }
            if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].CellRenderer == "ButtonCell" && e.RowIndex != -1)
                {
                    string response = gvwQuestions.Rows[e.RowIndex].Cells["ResponceCode"].Value != null ? gvwQuestions.Rows[e.RowIndex].Cells["ResponceCode"].Value.ToString() : string.Empty;

                    MatRCQuesID = gvwQuestions.Rows[e.RowIndex].Cells["QuestionCode"].Value.ToString();
                    string QuesDesc = gvwQuestions.Rows[e.RowIndex].Cells["Question"].Value.ToString();
                    string QuesType = gvwQuestions.Rows[e.RowIndex].Cells["QuestType"].Value.ToString();
                    GroupCode = gvwQuestions.Rows[e.RowIndex].Cells["GroupQues"].Value.ToString();


                    List<MATQUESREntity> matresponceEntity = _model.MatrixScalesData.GETMATQUESR(MatCode, ScrCode, GroupCode, MatRCQuesID, string.Empty);

                    if (matresponceEntity.Count > 0)
                    {
                        PrivilegeEntity privileges = new PrivilegeEntity();
                        privileges.AddPriv = "true";
                        AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, QuesDesc, QuesType, response, matresponceEntity);
                        objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                        objform.StartPosition = FormStartPosition.CenterScreen;
                        objform.ShowDialog();
                    }
                }

            }
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string BlankImage = Consts.Icons.ico_Blank;
                String strresp = form.propAlertCode;

                List<MATQUESREntity> matresponceEntity = _model.MatrixScalesData.GETMATQUESR(MatCode, ScrCode, GroupCode, MatRCQuesID, string.Empty);
                if (matresponceEntity.Count > 0)
                {
                    if (strresp.Trim() != "")
                    {
                        string[] strArry = strresp.Split(',');
                        string stroutResp = "";
                        for (int x = 0; x < strArry.Length; x++)
                        {
                            List<MATQUESREntity> selEnty = matresponceEntity.Where(r => r.RespCode.Trim() == strArry[x].ToString()).ToList();
                            if (selEnty != null)
                            {
                                foreach (var ent in selEnty)
                                {
                                    stroutResp += ent.RespDesc.ToString() + ", ";
                                }
                            }

                        }

                        gvwQuestions.SelectedRows[0].Cells["Responce"].Value = stroutResp.Trim().TrimEnd(',');
                        gvwQuestions.SelectedRows[0].Cells["ResponceCode"].Value = strresp.Trim().TrimEnd(',');

                    }

                }
            }
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRow()
        {
            string QuestionCode = null;
            if (gvwQuestions != null)
            {
                foreach (DataGridViewRow dr in gvwQuestions.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        strIndex = gvwQuestions.SelectedRows[0].Index;
                        GroupCode = gvwQuestions.SelectedRows[0].Cells["GroupQues"].Value.ToString();
                        QuestionCode = gvwQuestions.SelectedRows[0].Cells["Code"].Value.ToString();

                    }
                }
            }
            return QuestionCode;
        }

        //private void MessageBoxHandler(object sender, EventArgs e)
        //{
        //    // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
        //    Gizmox.WebGUI.Common.Resources.ResourceHandle BlankImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        //    Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;

        //    if (senderForm != null)
        //    {
        //        // Set DialogResult value of the Form as a text for label
        //        if (senderForm.DialogResult.ToString() == "Yes")
        //        {
        //            //MATASMTEntity matasmtentity = new MATASMTEntity();

        //            //matasmtentity.Agency = BaseForm.BaseAgency;
        //            //matasmtentity.Dept = BaseForm.BaseDept;
        //            //matasmtentity.Program = BaseForm.BaseProg;
        //            //matasmtentity.Year = BaseForm.BaseYear;
        //            //matasmtentity.App = BaseForm.BaseApplicationNo;
        //            //matasmtentity.MatCode = MatCode;
        //            //matasmtentity.SclCode = ScrCode;
        //            //matasmtentity.Type = "D";
        //            //matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
        //            //matasmtentity.QuesCode = gvwQuestions.SelectedRows[0].Cells[0].Value.ToString();
        //            //matasmtentity.AddOperator = BaseForm.UserID;
        //            //matasmtentity.LstcOperator = BaseForm.UserID;
        //            //matasmtentity.Mode = "DELETE";

        //            //if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
        //            //{
        //            // Gizmox.WebGUI.Common.Resources.ResourceHandle BlankImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        //            gvwQuestions.SelectedRows[0].Cells["Responce"].Value = string.Empty;
        //            gvwQuestions.SelectedRows[0].Cells["Responcecode"].Value = string.Empty;
        //            gvwQuestions.SelectedRows[0].Cells["Points"].Value = string.Empty;
        //            gvwQuestions.SelectedRows[0].Cells["QuestionDelete"].Value = BlankImage;
        //            gvwQuestions.SelectedRows[0].Cells["QuesrExclude"].Value = string.Empty;
        //            //    List<MATASMTEntity> matasmtRespDetails = matasmtRespcode.FindAll(u => u.Type.Equals("D"));
        //            //    string strBMDesc = string.Empty;
        //            //    string strPointL = "0";
        //            //    string strBmCodeL = string.Empty;
        //            //    foreach (MATOUTCEntity item in matoutcEntity)
        //            //    {
        //            //        List<MATQUESTEntity> matquestResponceEntity = _model.MatrixScalesData.GETMATQUESTQuestions(MatCode, ScrCode, string.Empty, item.BmCode, item.Code, string.Empty, "MATOUTR");

        //            //        int Responcecount = matquestResponceEntity.Count;
        //            //        int intSmtResp = 0;
        //            //        foreach (MATQUESTEntity matQuesRespEntity in matquestResponceEntity)
        //            //        {
        //            //            foreach (MATASMTEntity mataSmtrespEntity in matasmtRespDetails)
        //            //            {
        //            //                if ((mataSmtrespEntity.QuesCode.Equals(matQuesRespEntity.Code)) && (mataSmtrespEntity.RespCode.Equals(matQuesRespEntity.ResponceCode)))
        //            //                {
        //            //                    intSmtResp++;
        //            //                }

        //            //            }
        //            //        }
        //            //        if (Responcecount > 0)
        //            //        {
        //            //            if (Responcecount == intSmtResp)
        //            //            {
        //            //                strBMDesc = item.BMDesc;
        //            //                strPointL = item.Points;
        //            //                strBmCodeL = item.BmCode;
        //            //                //  MessageBox.Show("Matched record" + item.BMDesc + "   " + );
        //            //                break;
        //            //            }
        //            //        }


        //            //    }


        //            //    matasmtentity.Agency = BaseForm.BaseAgency;
        //            //    matasmtentity.Dept = BaseForm.BaseDept;
        //            //    matasmtentity.Program = BaseForm.BaseProg;
        //            //    matasmtentity.Year = BaseForm.BaseYear;
        //            //    matasmtentity.App = BaseForm.BaseApplicationNo;
        //            //    matasmtentity.MatCode = MatCode;
        //            //    matasmtentity.SclCode = ScrCode;
        //            //    matasmtentity.Type = "S";
        //            //    matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
        //            //    matasmtentity.QuesCode = "0";
        //            //    matasmtentity.RespCode = "";
        //            //    matasmtentity.RespDesc = "";
        //            //    matasmtentity.Points = strPointL;
        //            //    matasmtentity.ByPass = "N";
        //            //    matasmtentity.AddOperator = BaseForm.UserID;
        //            //    matasmtentity.LstcOperator = BaseForm.UserID;
        //            //    matasmtentity.Mode = string.Empty;

        //            //    if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
        //            //    {
        //            //        txtScore.Text = strPointL;
        //            //        txtScore.Visible = true;
        //            //        lblScore.Visible = true;
        //            //        lblViewBenchmark.Text = strBMDesc;
        //            //        GetOutcomeList();
        //            //        MATOUTCEntity matOutEntityDesc = matOutEntityList.Find(u => u.BmCode.Equals(strBmCodeL) && u.Points.Equals(strPointL));
        //            //        if (matOutEntityDesc != null)
        //            //        {
        //            //            txtBenchMarkDesc.Text = matOutEntityDesc.Desc.ToString();
        //            //            txtBenchMarkDesc.Visible = true;
        //            //        }
        //            //        else
        //            //        {
        //            //            txtBenchMarkDesc.Visible = false;
        //            //        }
        //            //    }

        //            //}
        //            //else
        //            //{
        //            //    gvwQuestions.SelectedRows[0].Cells["Responce"].Value = string.Empty;
        //            //    gvwQuestions.SelectedRows[0].Cells["Responcecode"].Value = string.Empty;
        //            //    gvwQuestions.SelectedRows[0].Cells["Points"].Value = string.Empty;
        //            //    gvwQuestions.SelectedRows[0].Cells["QuestionDelete"].Value = BlankImage;
        //            //    //  MessageBox.Show("You can’t delete this Mat00003, as there are Dependices", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            //}

        //        }
        //    }
        //}


        List<MATSGRPEntity> QuesGroups_List = new List<MATSGRPEntity>();

        bool Validation()
        {
            bool boolvalid = true;
            if (cmbMembers.Visible == true)
            {
                if (cmbMembers.SelectedIndex < 0)
                {
                    _errorProvider.SetError(cmbMembers, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblContactName.Text));
                    boolvalid = false;
                }
                else
                    _errorProvider.SetError(cmbMembers, null);
            }
            return boolvalid;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                int intResponceQuestions = 0;
                int intResponceQuestionsCount = 0;
                MATASMTEntity matasmtentity = new MATASMTEntity();
                if (chkBypassScale.Checked == true)
                {
                    matasmtentity.Agency = BaseForm.BaseAgency;
                    matasmtentity.Dept = BaseForm.BaseDept;
                    matasmtentity.Program = BaseForm.BaseProg;
                    matasmtentity.Year = propSelectYear;
                    matasmtentity.App = BaseForm.BaseApplicationNo;
                    matasmtentity.MatCode = MatCode;
                    matasmtentity.SclCode = ScrCode;
                    if (BaseForm.BaseAgencyControlDetails.MatAssesment == "Y")
                    {
                        if (strScaleAssType == "I")
                        {
                            matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                        }
                        else if (strScaleAssType == "B")
                        {
                            if (rdoIndividual.Checked)
                            {
                                matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                            }
                            else
                            {
                                matasmtentity.FamSeq = "0";
                            }
                        }
                        else
                        {
                            matasmtentity.FamSeq = "0";//BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                        }
                    }
                    else
                    {
                        matasmtentity.FamSeq = "0";
                    }
                    //matasmtentity.Type = "D";
                    //matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                    //// matasmtentity.QuesCode = gvwQuestions.SelectedRows[0].Cells[0].Value.ToString();
                    //matasmtentity.AddOperator = BaseForm.UserID;
                    //matasmtentity.LstcOperator = BaseForm.UserID;
                    //matasmtentity.Mode = "DELETE";

                    //if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                    //{
                    //}

                    matasmtentity.Type = "S";
                    matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                    //matoutResponce.BmCode = BMcode;
                    //matoutResponce.Code = OutrCode;
                    matasmtentity.QuesCode = "0";
                    matasmtentity.RespCode = "";
                    matasmtentity.RespDesc = "";
                    matasmtentity.Points = "0";
                    matasmtentity.Change = "0";
                    matasmtentity.Pn = string.Empty;

                    if (chkBypassScale.Checked == true)
                    {
                        matasmtentity.ByPass = "Y";
                    }

                    matasmtentity.AddOperator = BaseForm.UserID;
                    matasmtentity.LstcOperator = BaseForm.UserID;
                    matasmtentity.Mode = string.Empty;

                    if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                    {

                        this.DialogResult = DialogResult.OK;
                        AlertBox.Show("Saved Successfully");
                        this.Close();
                    }

                }
                else
                {


                    bool boolResponce = false;
                    bool boolSucess = true;
                    string strGroupCheck = string.Empty;
                    string strResponceValue = string.Empty;
                    string strQuesExcludeswitch = string.Empty;
                    foreach (MATSGRPEntity MATSGRPEntityitem in QuesGroups_List)
                    {
                        int intResponce = 0;
                        boolSucess = true;
                        string strGroup = string.Empty;
                        strGroupCheck = MATSGRPEntityitem.Desc;

                        List<DataGridViewRow> SelectedgvRows = (from c in gvwQuestions.Rows.Cast<DataGridViewRow>().ToList()
                                                                where (c.Cells["GroupQues"].Value.ToString().Equals(MATSGRPEntityitem.Group.ToString()))
                                                                select c).ToList();





                        foreach (DataGridViewRow gvquesRows in SelectedgvRows)
                        {
                            strGroup = gvquesRows.Cells["GroupQues"].Value.ToString();
                            strResponceValue = gvquesRows.Cells["Responce"].Value == null ? string.Empty : gvquesRows.Cells["Responce"].Value.ToString().Trim();
                            if (MATSGRPEntityitem.GrpType == "1" && strGroup == MATSGRPEntityitem.Group.ToString())
                            {
                                if (gvquesRows.Cells["ResponceCode"].Value.ToString() != "Responce")
                                {
                                    if (strResponceValue == string.Empty)
                                    {
                                        boolResponce = true;
                                        boolSucess = false;
                                        goto extresponce;
                                    }
                                }
                            }
                            else if (MATSGRPEntityitem.GrpType == "2" && strGroup == MATSGRPEntityitem.Group.ToString())
                            {
                                if (gvquesRows.Cells["ResponceCode"].Value.ToString() != "Responce")
                                {
                                    if (strResponceValue != string.Empty)
                                    {
                                        intResponce++;
                                    }
                                }
                            }
                            else if (MATSGRPEntityitem.GrpType == "3" && strGroup == MATSGRPEntityitem.Group.ToString())
                            {
                                if (gvquesRows.Cells["ResponceCode"].Value.ToString() != "Responce")
                                {

                                    //if (!(string.IsNullOrEmpty(gvquesRows.Cells["Responce"].Value.ToString().Trim())))
                                    //{
                                    if (strResponceValue != string.Empty)
                                    {
                                        intResponce++;
                                    }
                                }
                            }
                        }


                        //foreach (DataGridViewRow gvquesRows in gvwQuestions.Rows)
                        //{
                        //    strGroup = gvquesRows.Cells["GroupQues"].Value.ToString();
                        //    if (MATSGRPEntityitem.GrpType == "1" && strGroup == MATSGRPEntityitem.Group.ToString())
                        //    {
                        //        if (gvquesRows.Cells["ResponceCode"].Value == string.Empty)
                        //        {
                        //            boolResponce = true;
                        //            boolSucess = false;
                        //            goto extresponce;


                        //        }
                        //    }
                        //    else if (MATSGRPEntityitem.GrpType == "2" && strGroup == MATSGRPEntityitem.Group.ToString())
                        //    {
                        //        if (gvquesRows.Cells["ResponceCode"].Value.ToString() != "Responce")
                        //        {
                        //            if (gvquesRows.Cells["ResponceCode"].Value.ToString() != string.Empty)
                        //            {
                        //                intResponce++;
                        //            }
                        //        }
                        //    }
                        //}
                        if (MATSGRPEntityitem.GrpType == "2" && strGroup == MATSGRPEntityitem.Group.ToString())
                        {
                            if (intResponce == 0)
                            {
                                boolSucess = false;
                                CommonFunctions.MessageBoxDisplay("At least 1 question must be answered in the group \n " + strGroupCheck);
                                break;
                            }
                            if (intResponce > 1)
                            {
                                boolSucess = false;
                                CommonFunctions.MessageBoxDisplay("Only 1 question must be answered in the group \n " + strGroupCheck);
                                break;
                            }
                        }
                        if (MATSGRPEntityitem.GrpType == "3" && strGroup == MATSGRPEntityitem.Group.ToString())
                        {
                            if (intResponce == 0)
                            {
                                boolSucess = false;
                                CommonFunctions.MessageBoxDisplay("At least 1 question must be answered in the group \n " + strGroupCheck);
                                break;
                            }

                        }
                    }
                extresponce:
                    if (boolResponce)
                    {
                        CommonFunctions.MessageBoxDisplay("All questions must be answered in the group \n " + strGroupCheck);
                    }

                    if (boolSucess)
                    {
                        matasmtentity.Agency = BaseForm.BaseAgency;
                        matasmtentity.Dept = BaseForm.BaseDept;
                        matasmtentity.Program = BaseForm.BaseProg;
                        matasmtentity.Year = propSelectYear;
                        matasmtentity.App = BaseForm.BaseApplicationNo;
                        matasmtentity.MatCode = MatCode;
                        matasmtentity.SclCode = ScrCode;
                        if (BaseForm.BaseAgencyControlDetails.MatAssesment == "Y")
                        {
                            if (strScaleAssType == "I")
                            {
                                if (((ListItem)cmbMembers.SelectedItem).Value != null)
                                    matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                            }
                            else if (strScaleAssType == "B")
                            {
                                if (rdoIndividual.Checked)
                                {
                                    matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                                }
                                else
                                {
                                    matasmtentity.FamSeq = "0";
                                }
                            }
                            else
                            {
                                matasmtentity.FamSeq = "0";//BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                            }
                        }
                        else
                        {
                            matasmtentity.FamSeq = "0";
                        }
                        matasmtentity.Type = "D";
                        matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                        matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                        // matasmtentity.QuesCode = gvwQuestions.SelectedRows[0].Cells[0].Value.ToString();
                        matasmtentity.AddOperator = BaseForm.UserID;
                        matasmtentity.LstcOperator = BaseForm.UserID;
                        matasmtentity.Mode = "DELETE";

                        if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                        {
                        }
                        string strResponceInsert = string.Empty;
                        string strQuestionType = string.Empty;
                        foreach (DataGridViewRow gvquesRows in gvwQuestions.Rows)
                        {
                            if (gvquesRows.Cells["QuestType"].Value.ToString() == "1")
                                strResponceInsert = gvquesRows.Cells["Responce"].FormattedValue == null ? string.Empty : gvquesRows.Cells["Responce"].FormattedValue.ToString().Trim();
                            else
                                strResponceInsert = gvquesRows.Cells["Responce"].Value == null ? string.Empty : gvquesRows.Cells["Responce"].Value.ToString().Trim();

                            if (strResponceInsert != "Responce")
                            {
                                matasmtentity.Agency = BaseForm.BaseAgency;
                                matasmtentity.Dept = BaseForm.BaseDept;
                                matasmtentity.Program = BaseForm.BaseProg;
                                matasmtentity.Year = propSelectYear;
                                matasmtentity.App = BaseForm.BaseApplicationNo;
                                matasmtentity.MatCode = MatCode;
                                matasmtentity.SclCode = ScrCode;
                                if (BaseForm.BaseAgencyControlDetails.MatAssesment == "Y")
                                {
                                    if (strScaleAssType == "I")
                                    {
                                        matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                                    }
                                    else if (strScaleAssType == "B")
                                    {
                                        if (rdoIndividual.Checked)
                                        {
                                            matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                                        }
                                        else
                                        {
                                            matasmtentity.FamSeq = "0";
                                        }
                                    }
                                    else
                                    {
                                        matasmtentity.FamSeq = "0";//BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                                    }
                                }
                                else
                                {
                                    matasmtentity.FamSeq = "0";
                                }
                                matasmtentity.Type = "D";
                                matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                                matasmtentity.Change = "0";
                                //matoutResponce.BmCode = BMcode;
                                //matoutResponce.Code = OutrCode;
                                matasmtentity.QuesCode = gvquesRows.Cells[0].Value.ToString();
                                matasmtentity.RespCode = gvquesRows.Cells["ResponceCode"].Value.ToString();
                                matasmtentity.RespDesc = strResponceInsert;//gvquesRows.Cells["Responce"].Value.ToString();
                                matasmtentity.Points = gvquesRows.Cells["Points"].Value.ToString();
                                if (strResponceInsert.Trim() != string.Empty)
                                {
                                    if (chkBypassScale.Checked == true)
                                    {
                                        matasmtentity.ByPass = "Y";
                                    }
                                    else
                                    {
                                        matasmtentity.ByPass = "N";
                                    }
                                    //if (gvquesRows.Cells["Points"].Value != string.Empty)
                                    //{
                                    //    if (Convert.ToInt32(gvquesRows.Cells["Points"].Value) > 0)
                                    //    {
                                    //        matasmtentity.Pn = "P";
                                    //    }
                                    //    else if (Convert.ToInt32(gvquesRows.Cells["Points"].Value) < 0)
                                    //    {
                                    //        matasmtentity.Pn = "N";
                                    //    }
                                    //    else
                                    //    {
                                    //        matasmtentity.Pn = " ";
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    matasmtentity.Pn = " ";
                                    //}
                                    matasmtentity.AddOperator = BaseForm.UserID;
                                    matasmtentity.LstcOperator = BaseForm.UserID;
                                    matasmtentity.Mode = string.Empty;

                                    if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                                    {
                                        gvquesRows.Cells["QuestionDelete"].Value = DeleteImage;
                                        if (ScoreSheet == "R")
                                        {
                                            strQuestionType = gvquesRows.Cells["QuestType"].Value.ToString();
                                            if ((gvquesRows.Cells["QuesrExclude"].Value.Equals("N")) && (strQuestionType == "1" || strQuestionType == "2" || strQuestionType == "3" || strQuestionType == "4"))
                                            {
                                                intResponceQuestions++;
                                                intResponceQuestionsCount = Convert.ToInt32(gvquesRows.Cells["Points"].Value) + intResponceQuestionsCount;
                                                strQuesExcludeswitch = "N";
                                            }
                                            else
                                            {
                                                if (!(gvquesRows.Cells["QuesrExclude"].Value.Equals("N")))
                                                {
                                                    if (strQuesExcludeswitch == string.Empty)
                                                    {
                                                        strQuesExcludeswitch = "Y";
                                                    }
                                                }
                                            }
                                        }
                                        if (ScoreSheet == "S")
                                        {
                                            strQuestionType = gvquesRows.Cells["QuestType"].Value.ToString();
                                            if ((gvquesRows.Cells["QuesrExclude"].Value.Equals("N")) && (strQuestionType == "1" || strQuestionType == "2" || strQuestionType == "3" || strQuestionType == "4"))
                                            {
                                                intResponceQuestions++;
                                                intResponceQuestionsCount = Convert.ToInt32(gvquesRows.Cells["Points"].Value) + intResponceQuestionsCount;
                                                strQuesExcludeswitch = "N";
                                            }
                                            //else
                                            //{
                                            //    if (!(gvquesRows.Cells["QuesrExclude"].Value.Equals("N")))
                                            //    {
                                            //        if (strQuesExcludeswitch == string.Empty)
                                            //        {
                                            //            strQuesExcludeswitch = "Y";
                                            //        }
                                            //    }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                        //if (chkBypassScale.Checked == true)
                        //{
                        string strMatPointSwitch = string.Empty;
                        string strBMDesc = string.Empty;
                        string strPointL = "0";
                        string strBmCodeL = string.Empty;
                        if (ScoreSheet == "O")
                        {
                            matasmtRespcode = _model.MatrixScalesData.GETMatasmt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, propSelectYear, BaseForm.BaseApplicationNo, MatCode, ScrCode, string.Empty, SSDate, string.Empty, string.Empty); //(((ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((ListItem)cmbScale.SelectedItem).Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Benchmarkcode"].Value.ToString(), gvwOutComes.SelectedRows[0].Cells["Outcomecode"].Value.ToString(), string.Empty, "MATOUTR");
                            List<MATASMTEntity> matasmtRespDetails = matasmtRespcode.FindAll(u => u.Type.Equals("D"));

                            foreach (MATOUTCEntity item in matoutcEntity)
                            {
                                strMatPointSwitch = "N";
                                List<MATQUESTEntity> matquestResponceEntity = _model.MatrixScalesData.GETMATQUESTQuestions(MatCode, ScrCode, string.Empty, item.BmCode, item.Code, string.Empty, "MATOUTR");

                                int Responcecount = matquestResponceEntity.Count;
                                int intSmtResp = 0;
                                foreach (MATQUESTEntity matQuesRespEntity in matquestResponceEntity)
                                {
                                    foreach (MATASMTEntity mataSmtrespEntity in matasmtRespDetails)
                                    {
                                        if ((mataSmtrespEntity.QuesCode.Equals(matQuesRespEntity.Code)) && (mataSmtrespEntity.RespCode.Equals(matQuesRespEntity.ResponceCode)))
                                        {
                                            //matQuesRespEntity.ResponceExclude;
                                            intSmtResp++;
                                        }

                                    }
                                }
                                if (Responcecount > 0)
                                {
                                    if (Responcecount == intSmtResp)
                                    {
                                        strBMDesc = item.BMDesc;
                                        strPointL = item.Points;
                                        strBmCodeL = item.BmCode;
                                        strMatPointSwitch = "Y";
                                        //  MessageBox.Show("Matched record" + item.BMDesc + "   " + );
                                        break;
                                    }
                                }


                            }
                        }

                        matasmtentity.Agency = BaseForm.BaseAgency;
                        matasmtentity.Dept = BaseForm.BaseDept;
                        matasmtentity.Program = BaseForm.BaseProg;
                        matasmtentity.Year = propSelectYear;
                        matasmtentity.App = BaseForm.BaseApplicationNo;
                        matasmtentity.MatCode = MatCode;
                        matasmtentity.SclCode = ScrCode;
                        if (BaseForm.BaseAgencyControlDetails.MatAssesment == "Y")
                        {
                            if (strScaleAssType == "I")
                            {
                                matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                            }
                            else if (strScaleAssType == "B")
                            {
                                if (rdoIndividual.Checked)
                                {
                                    matasmtentity.FamSeq = ((ListItem)cmbMembers.SelectedItem).Value.ToString();
                                }
                                else
                                {
                                    matasmtentity.FamSeq = "0";
                                }
                            }
                            else
                            {
                                matasmtentity.FamSeq = "0";//BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                            }
                        }
                        else
                        {
                            matasmtentity.FamSeq = "0";
                        }
                        matasmtentity.Type = "S";
                        matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                        //matasmtentity.QuesCode = strMatQuesCode;
                        matasmtentity.QuesCode = "0";
                        matasmtentity.PointsSwitch = strMatPointSwitch;
                        matasmtentity.RespCode = "";
                        matasmtentity.RespDesc = "";
                        if (ScoreSheet == "O")
                        {
                            matasmtentity.Points = strPointL;
                        }
                        else if (ScoreSheet == "S")
                        {
                            decimal intpoints;
                            if (intResponceQuestions > 0)
                            {
                                intpoints = Convert.ToDecimal(intResponceQuestionsCount);
                                strQuesExcludeswitch = "N";
                            }
                            else
                            {
                                intpoints = 0;
                            }

                            matasmtentity.Points = Math.Round(intpoints).ToString();
                            strPointL = Math.Round(intpoints).ToString();
                        }
                        else
                        {
                            decimal intpoints;
                            if (intResponceQuestions > 0)
                            {
                                intpoints = Convert.ToDecimal(intResponceQuestionsCount) / Convert.ToDecimal(intResponceQuestions);
                                strQuesExcludeswitch = "N";
                            }
                            else
                            {
                                intpoints = 0;
                            }

                            matasmtentity.Points = Math.Round(intpoints).ToString();
                            strPointL = Math.Round(intpoints).ToString();
                        }
                        if (mataSmtPreviousDate == null)
                        {
                            matasmtentity.Change = "0";
                            matasmtentity.Pn = string.Empty;
                        }
                        else
                            matasmtentity.Change = (Convert.ToInt32(matasmtentity.Points) - Convert.ToInt32(mataSmtPreviousDate.Points)).ToString();

                        matasmtentity.ByPass = "N";
                        matasmtentity.RespExcel = strQuesExcludeswitch;
                        matasmtentity.AddOperator = BaseForm.UserID;
                        matasmtentity.LstcOperator = BaseForm.UserID;
                        matasmtentity.Mode = string.Empty;

                        if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                        {
                            ShowBenchmarkDescription(strPointL, strBMDesc, strBmCodeL, string.Empty, strQuesExcludeswitch, strMatPointSwitch);
                            if (strQuesExcludeswitch != "Y")
                            {
                                MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(strPointL) && Convert.ToInt32(strPointL) <= Convert.ToInt32(u.High));
                                if (MatadefrowEntity != null)
                                {
                                    strBmCodeL = MatadefrowEntity.Code;
                                    string strmatcode = string.Empty;
                                    if (matasmtentity.MatCode.Length == 1)
                                        strmatcode = matasmtentity.MatCode + "  ";
                                    if (matasmtentity.MatCode.Length == 2)
                                        strmatcode = matasmtentity.MatCode + " ";
                                    if (matasmtentity.MatCode.Length == 3)
                                        strmatcode = matasmtentity.MatCode;
                                    matasmtentity.Mat_BM_Code = strmatcode + strBmCodeL;
                                    matasmtentity.Mode = "BMCODE";
                                    if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                                    { }
                                }

                            }
                            else
                            {
                                if (strQuesExcludeswitch == "Y")
                                {
                                    lblViewBenchmark.Text = "No Score";
                                    matasmtentity.Mat_BM_Code = "Score0";
                                    matasmtentity.Mode = "BMCODE";
                                    if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                                    { }
                                }
                                txtBenchMarkDesc.Text = string.Empty;
                                txtScore.Visible = false;
                                lblScore.Visible = false;
                                txtBenchMarkDesc.Visible = false; this.pnlTop.Size = new Size(744, 98);
                            }

                        }
                        AlertBox.Show("Saved Successfully");

                        //  }

                        // Delete Record functionality.

                        //else
                        //{
                        //    matasmtentity.Agency = BaseForm.BaseAgency;
                        //    matasmtentity.Dept = BaseForm.BaseDept;
                        //    matasmtentity.Program = BaseForm.BaseProg;
                        //    matasmtentity.Year = propSelectYear;
                        //    matasmtentity.App = BaseForm.BaseApplicationNo;
                        //    matasmtentity.MatCode = MatCode;
                        //    matasmtentity.SclCode = ScrCode;
                        //    matasmtentity.Type = "S";
                        //    matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                        //    matasmtentity.QuesCode = "0";
                        //    matasmtentity.AddOperator = BaseForm.UserID;
                        //    matasmtentity.LstcOperator = BaseForm.UserID;
                        //    matasmtentity.Mode = "DELETE";
                        //    if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                        //    {
                        //        //chkBypassScale.Checked = false;
                        //    }
                        //}
                    }

                }
                TotalScorepartialScoreUpdate();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        ScoreOCMForm OCM_Form; ScoreRAMForm RAM_Form;
        string Score_Sheet = null;
        private void PbPdf_Click(object sender, EventArgs e)
        {

        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            if (Score_Sheet == "O")
                System.IO.File.Delete(OCM_Form.Get_Pdf_Path());
            else
                System.IO.File.Delete(RAM_Form.Get_Pdf_Path());
        }

        //private void gvwQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{

        //}

        private void gvwQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int intcolindex = gvwQuestions.CurrentCell.ColumnIndex;
            int introwindex = gvwQuestions.CurrentCell.RowIndex;
            string strCurrectValue = Convert.ToString(gvwQuestions.Rows[introwindex].Cells[intcolindex].Value);
            string strQuesttype = gvwQuestions.Rows[e.RowIndex].Cells["QuestType"].Value.ToString();

            if (gvwQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && (strQuesttype.Equals("5") || strQuesttype.Equals("6")))
            {

                if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                {
                    gvwQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                    gvwQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
                    gvwQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                    MessageBox.Show(Consts.Messages.PleaseEnterNumbers);
                }

            }
            else if (gvwQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("4"))
            {
                if (strCurrectValue != "")
                    strCurrectValue = Convert.ToDateTime(strCurrectValue).ToString("MM/dd/yyyy");

                if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                {
                    gvwQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                    gvwQuestions.Rows[introwindex].Cells["Responce"].Value = string.Empty;
                    gvwQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                    MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat);
                }
            }
            else if (gvwQuestions.Columns[e.ColumnIndex].Name.Equals("Responce") && strQuesttype.Equals("1"))
            {
                if (propssOverideQue == "1")
                {
                    if (gvwQuestions.Rows[e.RowIndex].Cells["SSOverride"].Value.ToString() != "1")
                    {
                        MessageBox.Show("Response to Question does not require the rest of questions in the scale to be answered \n Still You want to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerMenu);
                    }
                }
                else
                {
                    List<MATQUESREntity> lstResps = (List<MATQUESREntity>)gvwQuestions.Rows[e.RowIndex].Cells["Responce"].Tag as List<MATQUESREntity>;

                    MATQUESREntity dr = lstResps.Find(x => x.RespCode == strCurrectValue);

                    gvwQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                    //responseValue = selectedValue;
                    //responseCode = selectedCode;
                    // gvwQuestions.SelectedRows[0].Cells[2].Value = strCurrectValue;
                    // gvwQuestions.SelectedRows[0].Cells[2].Tag = dr.RespCode.ToString();
                    gvwQuestions.Rows[e.RowIndex].Cells["ResponceCode"].Value = strCurrectValue;
                    gvwQuestions.Rows[e.RowIndex].Cells["QuestionDelete"].Value = DeleteImage;
                    gvwQuestions.Rows[e.RowIndex].Cells["QuesrExclude"].Value = dr.Exclude;
                    gvwQuestions.Rows[e.RowIndex].Cells["Points"].Value = dr.Points;

                    gvwQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);

                    if (gvwQuestions.Rows[e.RowIndex].Cells["SSOverride"].Value.ToString() == "1")
                    {
                        gvwQuestions.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        propssOverideQue = "1";
                    }
                }
            }

            if (gvwQuestions.Rows[e.RowIndex].Cells["Responce"].Value != string.Empty)
            {
                gvwQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
                gvwQuestions.Rows[e.RowIndex].Cells["QuestionDelete"].Value = DeleteImage;
                gvwQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestions_CellValueChanged);
            }
        }

        private void MAT00003ClientAssesment_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            // this.Close();
        }

        private void MAT00003ClientAssesment_Load(object sender, EventArgs e)
        {
            cmbScale.SelectedIndexChanged += new EventHandler(cmbScale_SelectedIndexChanged);
            cmbScale_SelectedIndexChanged(cmbScale, new EventArgs());
            cmbScale.Focus();
        }


        private void ShowBenchmarkDescription(string strPointL, string strBMDesc, string strBmCodeL, string strtype, string strRespSwich, string strMatPointSwitch)
        {
            txtScore.Text = strPointL;
            intPreviouspoint = Convert.ToInt32(strPointL);
            txtScore.Visible = true;
            lblScore.Visible = true;
            lblViewBenchmark.Text = string.Empty;
            txtBenchMarkDesc.Text = string.Empty;
            if (propBenchMark == "Y")
            {
                //if (strtype != "Change")
                //{
                //    if (ScoreSheet == "O")
                //    {
                //        lblViewBenchmark.Text = strBMDesc;
                //    }
                //    else
                //    {

                //        MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(strPointL) && Convert.ToInt32(strPointL) <= Convert.ToInt32(u.High));
                //        if (MatadefrowEntity != null)
                //        {
                //            lblViewBenchmark.Text = MatadefrowEntity.Desc;
                //            strBmCodeL = MatadefrowEntity.Code;
                //        }
                //        else
                //            lblViewBenchmark.Text = string.Empty;
                //    }
                //}
                //else
                //{
                if (strRespSwich != "Y")
                {
                    MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(strPointL) && Convert.ToInt32(strPointL) <= Convert.ToInt32(u.High));
                    if (MatadefrowEntity != null)
                    {
                        if (strMatPointSwitch == "N")
                        {
                            lblViewBenchmark.Text = "Combined responses do not meet standardized scale."; //"No responses met for proper benchmark association";
                            strBmCodeL = "999";
                        }
                        else
                        {
                            lblViewBenchmark.Text = MatadefrowEntity.Desc;
                            strBmCodeL = MatadefrowEntity.Code;
                        }
                    }
                    else
                        lblViewBenchmark.Text = string.Empty;
                    // }

                    MATOUTCEntity matOutEntityDesc = matOutEntityList.Find(u => u.SclCode.Equals(ScrCode) && u.BmCode.Equals(strBmCodeL) && u.Points.Equals(strPointL));
                    if (matOutEntityDesc != null)
                    {
                        txtBenchMarkDesc.Text = matOutEntityDesc.Desc.ToString();
                        txtBenchMarkDesc.Visible = true;
                        this.pnlTop.Size = new Size(744, 150);
                    }
                    else
                    {
                        txtBenchMarkDesc.Visible = false;
                        this.pnlTop.Size = new Size(744, 98);
                    }
                    if (chkBypassScale.Checked == true)
                    {
                        lblViewBenchmark.Text = "NOT APPLICABLE";
                        txtBenchMarkDesc.Visible = false;
                        txtScore.Visible = false;
                        lblScore.Visible = false;
                        this.pnlTop.Size = new Size(744, 98);
                    }
                }
                else
                {
                    if (strRespSwich == "Y")
                    {
                        lblViewBenchmark.Text = "No Score";
                    }
                    txtBenchMarkDesc.Text = string.Empty;
                    txtScore.Visible = false;
                    lblScore.Visible = false;
                    txtBenchMarkDesc.Visible = false; this.pnlTop.Size = new Size(744, 98);
                }
            }
            else
            {
                txtScore.Visible = false;
                lblScore.Visible = false;
                txtBenchMarkDesc.Visible = false; this.pnlTop.Size = new Size(744, 98);
                lblViewBenchmark.Text = string.Empty;
                txtBenchMarkDesc.Text = string.Empty;
            }
        }

        private void btnReasonCodes_Click(object sender, EventArgs e)
        {
            MAT00003ReasonCodes matrescode = new MAT00003ReasonCodes(BaseForm, Privileges, MatCode, ScrCode, SSDate, matasmtRespcode, mataSmtPreviousDate, propDateType);
            matrescode.StartPosition = FormStartPosition.CenterScreen;
            matrescode.ShowDialog();
        }

        private void ButtonPrivieges()
        {
            if (gvwQuestions.Rows.Count > 0)
            {

            }
            else
                btnSave.Visible = false;
        }

        private void cmbMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMembers.Items.Count > 0)
            {
                GetMatasmtData("MEMBER");
            }
        }

        private void rdoIndividual_Click(object sender, EventArgs e)
        {
            if (rdoIndividual.Checked)
            {
                cmbMembers.Visible = true;
                if (cmbMembers.Items.Count > 0)
                {
                    CommonFunctions.SetComboBoxValue(cmbMembers, BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                    propFamseq = BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                }
            }
            else
            {
                cmbMembers.Visible = false;
                propFamseq = "0";
            }
            if (cmbMembers.Items.Count > 0)
            {
                GetMatasmtData("MEMBER");
            }
        }

        void TotalScorepartialScoreUpdate()
        {
            List<MATASMTEntity> MATASMTEntity = _model.MatrixScalesData.GETMatasmt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, propSelectYear, BaseForm.BaseApplicationNo, ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString(), string.Empty, string.Empty, dtAssessmentDate.Value.ToShortDateString(), string.Empty, string.Empty);
            List<MATASMTEntity> matasmtdata = MATASMTEntity.FindAll(u => (LookupDataAccess.Getdate(u.SSDate).Equals(LookupDataAccess.Getdate(dtAssessmentDate.Value.ToShortDateString()))) && u.Type.Equals("S"));
            int intPartialScore = 0;
            int intTotalScore = 0;
            foreach (MATASMTEntity matitem in matasmtdata)
            {
                MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(matitem.Points) && Convert.ToInt32(matitem.Points) <= Convert.ToInt32(u.High));
                if (MatadefrowEntity != null)
                {
                    if (!string.IsNullOrEmpty(matitem.Points))
                    {
                        if (MatadefrowEntity.ScoreType == "P")
                            intPartialScore = intPartialScore + Convert.ToInt32(matitem.Points);
                        if (MatadefrowEntity.ScoreType == "T")
                            intTotalScore = intTotalScore + Convert.ToInt32(matitem.Points);
                    }
                }
            }

            string strMsg = string.Empty;
            MATAPDTSEntity matapdtsEntity = new MATAPDTSEntity();
            matapdtsEntity.Agency = BaseForm.BaseAgency;
            matapdtsEntity.Dept = BaseForm.BaseDept;
            matapdtsEntity.Program = BaseForm.BaseProg;
            matapdtsEntity.Year = propSelectYear;
            matapdtsEntity.App = BaseForm.BaseApplicationNo;
            matapdtsEntity.MatCode = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString();
            matapdtsEntity.SSDate = dtAssessmentDate.Value.ToString();
            matapdtsEntity.AddOperator = BaseForm.UserID;
            matapdtsEntity.LstcOperator = BaseForm.UserID;
            matapdtsEntity.TotalScore = intTotalScore.ToString();
            matapdtsEntity.PartialScore = intPartialScore.ToString();
            matapdtsEntity.Mode = "POINTS";
            //txtSub.Text;                
            bool boolSucess = _model.MatrixScalesData.InsertUpdateDelMatapdts(matapdtsEntity, out strMsg);

        }

        private void MAT00003ClientAssesment_ToolClick(object sender, ToolClickEventArgs e)
        {

            if (e.Tool.Name == "tlPDF")
            {
                MATDEFEntity MatDef_Entity = new MATDEFEntity(true);
                MatDef_Entity.Mat_Code = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString(); MatDef_Entity.Scale_Code = "0";
                List<MATDEFEntity> Matdef_list = _model.MatrixScalesData.Browse_MATDEF(MatDef_Entity, "Browse");

                if (Matdef_list.Count > 0)
                    Score_Sheet = Matdef_list[0].Score.Trim();
                if (Score_Sheet == "O")
                {
                    MatrixScales_SelectionForm SelForm = new MatrixScales_SelectionForm(BaseForm, ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString(), "MAT00003", dtAssessmentDate.Text, ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Value.ToString());
                    SelForm.StartPosition = FormStartPosition.CenterScreen;
                    SelForm.ShowDialog();
                    //OCM_Form = new ScoreOCMForm(BaseForm, "MAT00003", ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Value.ToString(), dtAssessmentDate.Text, ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Text.ToString());
                    //if (OCM_Form.DialogResult == DialogResult.OK)
                    //{
                    //    FrmViewer objfrm = new FrmViewer(OCM_Form.Get_Pdf_Path());
                    //    objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
                    //    objfrm.ShowDialog();
                    //}

                }
                else
                {
                    MatrixScales_SelectionForm SelForm = new MatrixScales_SelectionForm(BaseForm, ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString(), "MAT00003", dtAssessmentDate.Text, ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Value.ToString());
                    SelForm.StartPosition = FormStartPosition.CenterScreen;
                    SelForm.ShowDialog();
                    //RAM_Form = new ScoreRAMForm(BaseForm, "MAT00003", ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Value.ToString(), dtAssessmentDate.Text, ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Text.ToString());
                    //if (RAM_Form.DialogResult == DialogResult.OK)
                    //{
                    //    FrmViewer objfrm = new FrmViewer(RAM_Form.Get_Pdf_Path());
                    //    objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
                    //    objfrm.ShowDialog();
                    //}
                }
            }
            else if (e.Tool.Name == "tlHelp")
            {
            }

        }

        //void GetGroups()
        //{ //((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase)

        //    List<DataGridViewRow> SelectedHierarchies        = (from c in gvwQuestions.Rows.Cast<DataGridViewRow>().ToList()
        //                                       where (c.Cells["GroupQues"].Value.ToString().Equals("ss"))
        //                                       select ((DataGridViewRow)c).Tag as DataGridViewRow).ToList();


        //}

    }
}
