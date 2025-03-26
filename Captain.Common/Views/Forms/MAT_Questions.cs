#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Linq;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
//using Wisej.Web.Design;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.Text.RegularExpressions;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Captain.Common.Views.UserControls;
using Spire.Pdf.Grid;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MAT_Questions : Form
    {

        #region private variables
        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private PrivilegesControl _screenPrivileges = null;

        #endregion

        public MAT_Questions(BaseForm baseform, string mode, string Matrix_cd, string Scale_cd, string Grp_Cd, string Ques_Cd, string Resp_Cd, string scoresheet, PrivilegeEntity priviliges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            _model = new CaptainModel();
            QuestionType = string.Empty;
            QuestionNType = string.Empty;
            Baseform = baseform;
            Privileges = priviliges;
            MatrixCode = Matrix_cd;
            ScaleCode = Scale_cd;
            Group_code = Grp_Cd;
            Quest_Code = Ques_Cd;
            Responce_Code = Resp_Cd;
            ScoreStreet = scoresheet;
            Mode = mode;
            Get_Questions_Related_Data();

            if (Tab_Page == "Groups")
            {

                this.Text = "Question Maintenance";//priviliges.Program;
                // FillGroups_Grid();
                //    Group_Controls_Fill();
                txtGrpCode.Enabled = false;

            }
            txtGrpCode.Validator = TextBoxValidation.IntegerValidator;
            txtSeq.Validator = TextBoxValidation.IntegerValidator;
            txtQues_Code.Validator = TextBoxValidation.IntegerValidator;
            txtQues_Seq.Validator = TextBoxValidation.IntegerValidator;
            //  txtResp_Code.Validator = TextBoxValidation.IntegerValidator;
            txtPoints.Validator = TextBoxValidation.IntegerValidator;

            //if (baseform.UserID == "JAKE")
            //{
            //    PicAddResponce.Visible = true;
            //    picAddQuestions.Visible = true;
            //    picGroupAdd.Visible = true;

            //    gvResponses.Columns["EditResponce"].Visible = true;
            //    gvQuestions.Columns["EditQuestions"].Visible = true;
            //    gvMat_Groups.Columns["EditGroup"].Visible = true;

            //    gvResponses.Columns[gvResponses.ColumnCount - 1].Visible = true;
            //    gvQuestions.Columns[gvQuestions.ColumnCount - 1].Visible = true;
            //    gvMat_Groups.Columns[gvMat_Groups.ColumnCount - 1].Visible = true;

            //}
            if (baseform.UserID != "JAKE")
            {
                PicAddResponce.Visible = false;
                picAddQuestions.Visible = false;
                picGroupAdd.Visible = false;

                gvResponses.Columns["EditResponce"].Visible = false;
                gvQuestions.Columns["EditQuestions"].Visible = false;
                gvMat_Groups.Columns["EditGroup"].Visible = false;

                gvResponses.Columns[gvResponses.ColumnCount - 1].Visible = false;
                gvQuestions.Columns[gvQuestions.ColumnCount - 1].Visible = false;
                gvMat_Groups.Columns[gvMat_Groups.ColumnCount - 1].Visible = false;
            }


            if (Privileges.AddPriv.Equals("false"))
            {
                PicAddResponce.Visible = false;
                picAddQuestions.Visible = false;
                picGroupAdd.Visible = false;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                gvResponses.Columns["EditResponce"].Visible = false;
                gvQuestions.Columns["EditQuestions"].Visible = false;
                gvMat_Groups.Columns["EditGroup"].Visible = false;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvResponses.Columns[gvResponses.ColumnCount - 1].Visible = false;
                gvQuestions.Columns[gvQuestions.ColumnCount - 1].Visible = false;
                gvMat_Groups.Columns[gvMat_Groups.ColumnCount - 1].Visible = false;
                // dataGridCaseIncome.Columns[dataGridCaseIncome.ColumnCount - 2].Width = 200;
            }
            if (ScoreStreet == "O")
            {
                gvResponses.Columns["Points"].Visible = false;
                gvResponses.Columns["Resp_Desc"].Width = 460;
            }
            else
            {
                gvResponses.Columns["Resp_Desc"].Width = 410;
            }

            //Get_BenchMarks_List();
        }

        #region properties

        public BaseForm Baseform { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string MatrixCode { get; set; }

        public string Group_code { get; set; }

        public string Quest_Code { get; set; }

        public string Responce_Code { get; set; }

        public string ScaleCode { get; set; }

        public string ScoreStreet { get; set; }

        public string Mode { get; set; }

        private string QuestionType { get; set; }
        private string QuestionNType { get; set; }

        #endregion

        private void fillCmbGrpType()
        {
            cmbGrp_Type.Items.Clear();
            List<Captain.Common.Utilities.ListItem> ListGrpType = new List<Captain.Common.Utilities.ListItem>();
            ListGrpType.Add(new Captain.Common.Utilities.ListItem("All", "1"));
            ListGrpType.Add(new Captain.Common.Utilities.ListItem("One", "2"));
            ListGrpType.Add(new Captain.Common.Utilities.ListItem("Any", "3"));
            ListGrpType.Add(new Captain.Common.Utilities.ListItem("Optional", "4"));
            cmbGrp_Type.Items.AddRange(ListGrpType.ToArray());
            cmbGrp_Type.SelectedIndex = 0;
        }

        private void fillcmbQuesType()
        {
            cmbQues_Type.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listQues_Type = new List<Captain.Common.Utilities.ListItem>();
            listQues_Type.Add(new Captain.Common.Utilities.ListItem("Drop Down", "1"));
            listQues_Type.Add(new Captain.Common.Utilities.ListItem("Check Box", "2"));
            listQues_Type.Add(new Captain.Common.Utilities.ListItem("Radio Button", "3"));
            listQues_Type.Add(new Captain.Common.Utilities.ListItem("Date", "4"));
            listQues_Type.Add(new Captain.Common.Utilities.ListItem("Numeric(W)", "5"));
            listQues_Type.Add(new Captain.Common.Utilities.ListItem("Numeric(D)", "6"));
            listQues_Type.Add(new Captain.Common.Utilities.ListItem("Alphanumeric", "7"));
            cmbQues_Type.Items.AddRange(listQues_Type.ToArray());
            cmbQues_Type.SelectedIndex = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "MAT00001_Questions");
        }

        List<MATSGRPEntity> QuesGroups_List = new List<MATSGRPEntity>();
        List<MATQUESTEntity> Questions_List = new List<MATQUESTEntity>();
        List<MATQUESREntity> Responses_List = new List<MATQUESREntity>();
        private void Get_Questions_Related_Data()
        {
            MATSGRPEntity Search_Entity = new MATSGRPEntity(true);
            MATQUESTEntity Search_Entity1 = new MATQUESTEntity(true);
            MATQUESREntity Search_Entity3 = new MATQUESREntity(true);

            QuesGroups_List = _model.MatrixScalesData.Browse_MATSGRP(Search_Entity, "Browse");
            Questions_List = _model.MatrixScalesData.Browse_MATQUEST(Search_Entity1, "Browse");
            Responses_List = _model.MatrixScalesData.Browse_MATQUESR(Search_Entity3, "Browse");
        }

        private void FillGroups_Grid()       // Groups Grid
        {
            this.gvMat_Groups.SelectionChanged -= new System.EventHandler(this.gvMat_Groups_SelectionChanged);
            gvMat_Groups.Rows.Clear();
            int RowIndex, TmpCount = 0;
            MATSGRPEntity Search_Entity = new MATSGRPEntity(true);
            QuesGroups_List = _model.MatrixScalesData.Browse_MATSGRP(Search_Entity, "Browse");
            if (QuesGroups_List.Count > 0)
            {
                List<MATSGRPEntity> matsgrpEntityList = QuesGroups_List.FindAll(u => u.MatCode.Trim().Equals(MatrixCode.ToString().Trim()) && u.SclCode.Trim().Equals(ScaleCode.Trim()));
                foreach (MATSGRPEntity EntityGrp in matsgrpEntityList)
                {
                    string Grptype = null;
                    if (EntityGrp.GrpType.ToString().Trim() == "1")
                        Grptype = "All";
                    else if (EntityGrp.GrpType.ToString().Trim() == "2")
                        Grptype = "One";
                    else if (EntityGrp.GrpType.ToString().Trim() == "3")
                        Grptype = "Any";
                    else if (EntityGrp.GrpType.ToString().Trim() == "4")
                        Grptype = "Optional";

                    RowIndex = gvMat_Groups.Rows.Add(EntityGrp.Group, EntityGrp.Seq, EntityGrp.Desc, Grptype);
                    //     CommonFunctions.setTooltip(RowIndex, EntityGrp.Add_Operator, Entity.AddDate, Entity.LstcOperator, Entity.DateLstc, gvQuestions);
                    gvMat_Groups.Rows[RowIndex].Tag = EntityGrp;
                   // if (Sel_Ques_Index.Equals(EntityGrp.Group))
                        //Sel_Ques_Index = TmpCount;
                    TmpCount++;
                }
            }
            if (TmpCount > 0)
            {
                //gvMat_Groups.Rows[0].Tag = 0;
                //gvMat_Groups.CurrentCell = gvMat_Groups.Rows[Sel_Ques_Index].Cells[1];
                //Txt_Ques_Desc.Text = Questions_Grid.CurrentRow.Cells["Ques_Desc"].Value.ToString();
            }
            else
            {
                txtGrpCode.Enabled = false;
                txtGrpCode.Text = txtGrp_Desc.Text = txtSeq.Text = string.Empty;
                cmbGrp_Type.SelectedIndex = 0;
                txtSeq.Enabled = false;
                cmbGrp_Type.Enabled = false;
                txtGrp_Desc.Enabled = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                gvMat_Groups.Enabled = true;
            }

            this.gvMat_Groups.SelectionChanged += new System.EventHandler(this.gvMat_Groups_SelectionChanged);

            if (gvMat_Groups.Rows.Count > 0)
            {
                if (deleteSelRow == "D")
                {
                    Sel_Ques_Index = gvMat_Groups.Rows.Count - 1;
                }

                gvMat_Groups.Rows[Sel_Ques_Index].Selected = true;
                gvMat_Groups.CurrentCell = gvMat_Groups.Rows[Sel_Ques_Index].Cells[1];
                gvMat_Groups_SelectionChanged(gvMat_Groups, EventArgs.Empty);
            }
            //this.gvMat_Groups.SelectionChanged += new System.EventHandler(this.gvMat_Groups_SelectionChanged);
           

        }

        private void Fill_Questions_Group_Combo()      //Questions   GroupsCombo
        {
            cmbQues_Group.Items.Clear();
            if (QuesGroups_List.Count > 0)
            {

                int Tmp_Count = 0;
                foreach (MATSGRPEntity Entity in QuesGroups_List)
                {
                    if (Entity.MatCode.ToString() == MatrixCode.ToString() && Entity.SclCode.ToString() == ScaleCode.ToString())
                    {
                        cmbQues_Group.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Group));

                        Tmp_Count++;
                    }
                }

                if (Tmp_Count > 0)
                {
                    cmbQues_Group.SelectedIndex = 0;



                    if (Privileges.AddPriv.Equals("false"))
                    {
                        picAddQuestions.Visible = false;
                    }
                    else
                        picAddQuestions.Visible = true;
                }
                else
                {
                    picAddQuestions.Visible = false;
                    if (Mode == "Edit")
                        pnlQues.Visible = false;
                }

                if (Baseform.UserID != "JAKE")
                {
                    picAddQuestions.Visible = false;
                    gvQuestions.Columns["EditQuestions"].Visible = false;
                    gvQuestions.Columns[gvQuestions.ColumnCount - 1].Visible = false;
                }

            }
        }

        private void Fill_RespQuestions_Group_Combo()     //Responses GroupsCombo
        {
            cmbResp_Grp.Items.Clear();

            if (QuesGroups_List.Count > 0)
            {
                int Tmp_Count = 0;
                foreach (MATSGRPEntity Entity in QuesGroups_List)
                {
                    if (Entity.MatCode.ToString() == MatrixCode.ToString() && Entity.SclCode.ToString() == ScaleCode.ToString())
                    {
                        cmbResp_Grp.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Group));

                        Tmp_Count++;
                    }
                }

                if (Tmp_Count > 0)
                {
                    cmbResp_Grp.SelectedIndex = 0;
                    if (Privileges.AddPriv.Equals("false"))
                    {
                        PicAddResponce.Visible = false;
                    }
                    else
                        PicAddResponce.Visible = true;
                }
                else
                {
                    //this.Close();
                    PicAddResponce.Visible = false;
                }

                if (Baseform.UserID != "JAKE")
                {
                    PicAddResponce.Visible = false;
                    gvResponses.Columns["EditResponce"].Visible = false;
                    gvResponses.Columns[gvResponses.ColumnCount - 1].Visible = false;
                }

            }
        }

        int Sel_Ques_Index = 0;
        private void FillQuesResp_Grid()                      //ResponsesGrid
        {
            this.gvResponses.SelectionChanged -= new System.EventHandler(this.gvResponses_SelectionChanged);
            gvResponses.Rows.Clear();
            int RowResponceIndex, TmpCount = 0;
            MATQUESREntity Search_Entity = new MATQUESREntity(true);
            Responses_List = _model.MatrixScalesData.Browse_MATQUESR(Search_Entity, "Browse");
            if (Responses_List.Count > 0)
            {
                if ((cmbResp_Grp.Items.Count > 0) && (cmbResp_Ques.Items.Count > 0))
                {
                    QuestionType = ((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).ID.ToString();
                    QuestionNType = ((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).ValueDisplayCode.ToString();
                    List<MATQUESREntity> matQuesResponEntityList = Responses_List.FindAll(u => u.MatCode.Trim().Equals(MatrixCode.Trim()) && u.SclCode.Trim().Equals(ScaleCode.Trim()) && u.Group.Trim().Equals(((Captain.Common.Utilities.ListItem)cmbResp_Grp.SelectedItem).Value.ToString()) && u.Code.Trim().Equals(((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).Value.ToString()));
                    matQuesResponEntityList= matQuesResponEntityList.OrderBy(u=>u.RespCode).ToList();
                    foreach (MATQUESREntity Entity in matQuesResponEntityList)
                    {
                        RowResponceIndex = gvResponses.Rows.Add(Entity.RespCode, Entity.RespDesc, Entity.Points);
                        gvResponses.Rows[RowResponceIndex].Tag = Entity;
                        // CommonFunctions.setTooltip(RowResponceIndex, Entity.Add_Operator, Entity.AddDate, Entity.LstcOperator, Entity.DateLstc, gvQuestions);
                        //set_Contact_Tooltip(TmpCount, Entity);
                        //if (Sel_Ques_Index.Equals(Entity.RespCode))
                        //    Sel_Ques_Index = TmpCount;
                        TmpCount++;
                    }
                }
                if (TmpCount > 0)
                {
                    gvResponses.Visible = true;
                    pnlRes.Visible = true;
                    ErrMsg.Visible = false;
                    //gvResponses.Rows[0].Tag = 0;
                   // gvResponses.CurrentCell = gvResponses.Rows[Sel_Ques_Index].Cells[1];
                    //Txt_Ques_Desc.Text = Questions_Grid.CurrentRow.Cells["Ques_Desc"].Value.ToString();
                }
                else
                {
                    ResponceControlEnable();
                    Mode = string.Empty;

                }
            }
            if (gvResponses.Rows.Count > 0)
            { 
                gvResponses.Rows[selRespIndex].Selected = true;
                gvResponses.CurrentCell = gvResponses.Rows[selRespIndex].Cells[1];
            }
            this.gvResponses.SelectionChanged += new System.EventHandler(this.gvResponses_SelectionChanged);
           
        }

        private void fillcmbRespQuestions()           //Responses QuestionsCombo
        {
            cmbResp_Ques.Items.Clear();
            if (Questions_List.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)cmbResp_Grp.SelectedItem) != null)
                {
                    int Tmp_Count = 0;
                foreach (MATQUESTEntity QuesList in Questions_List)
                {
                        if (QuesList.MatCode.ToString().Trim() == MatrixCode.ToString().Trim() &&
                            QuesList.SclCode.ToString().Trim() == ScaleCode.ToString().Trim() &&
                            QuesList.Group.ToString() == ((Captain.Common.Utilities.ListItem)cmbResp_Grp.SelectedItem).Value.ToString())
                        {
                            cmbResp_Ques.Items.Add(new Captain.Common.Utilities.ListItem(QuesList.Desc, QuesList.Code, QuesList.Type, QuesList.QuesNumericType));
                            Tmp_Count++;
                        }
                    }

                    if (Tmp_Count > 0)
                    {
                        cmbResp_Ques.SelectedIndex = 0;
                        gvResponses.Visible = true;
                        pnlRes.Visible = true;
                        ErrMsg.Visible = false;
                        PicAddResponce.Visible = true;
                    }
                    else
                    {
                        PicAddResponce.Visible = false;
                        //gvResponses.Visible = false;
                        //panel3.Visible = false;
                        //ErrMsg.Visible = true;
                        //ErrMsg.Text = "This Group not having Questions...";
                        //this.ErrMsg.Location = new System.Drawing.Point(132, 126);
                        //this.ErrMsg.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    }
                }
            }
        }

        private void FillQuestions_grid()                     //Questions Grid
        {
            this.gvQuestions.SelectionChanged -= new System.EventHandler(this.gvQuestions_SelectionChanged);
            gvQuestions.Rows.Clear();
            int RowQuestionIndex, TmpCount = 0;
            MATQUESTEntity Search_Entity = new MATQUESTEntity(true);
            Questions_List = _model.MatrixScalesData.Browse_MATQUEST(Search_Entity, "Browse");
            if (Questions_List.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)cmbQues_Group.SelectedItem) != null)
                {
                    List<MATQUESTEntity> matQuesEntityList = Questions_List.FindAll(u => u.MatCode.Trim().Equals(MatrixCode.Trim()) && u.SclCode.Trim().Equals(ScaleCode.Trim()) && u.Group.Trim().Equals(((Captain.Common.Utilities.ListItem)cmbQues_Group.SelectedItem).Value.ToString().Trim()));
                    foreach (MATQUESTEntity Entity in matQuesEntityList)
                    {

                        string QuesType = null;
                        if (Entity.Type.ToString().Trim() == "1")
                            QuesType = "Drop Down";
                        else if (Entity.Type.ToString().Trim() == "2")
                            QuesType = "Check Box";
                        else if (Entity.Type.ToString().Trim() == "3")
                            QuesType = "Radio Button";
                        else if (Entity.Type.ToString().Trim() == "4")
                            QuesType = "Date";
                        else if (Entity.Type.ToString().Trim() == "5")
                            QuesType = "Numeric(W)";
                        else if (Entity.Type.ToString().Trim() == "6")
                            QuesType = "Numeric(D)";
                        else
                            QuesType = "AlphaNumeric";
                        RowQuestionIndex = gvQuestions.Rows.Add(Entity.Code, Entity.Seq, Entity.Desc, QuesType);
                        CommonFunctions.setTooltip(RowQuestionIndex, Entity.AddOperator, Entity.AddDate, Entity.LstcOperator, Entity.DateLstc, gvQuestions);
                        gvQuestions.Rows[RowQuestionIndex].Tag = Entity;
                        //set_Contact_Tooltip(TmpCount, Entity);
                        //if(Sel_Ques_Index.Equals(Entity.Code))
                            //Sel_Ques_Index = TmpCount;
                        TmpCount++;

                    }
                    if (TmpCount > 0)
                    {
                        // gvQuestions.Rows[0].Tag = 0;
                        //gvQuestions.CurrentCell = gvQuestions.Rows[Sel_Ques_Index].Cells[1];
                        gvQuestions.Visible = true;
                        pnlQues.Visible = true;
                        ErrmsgQues.Visible = false;
                        //Txt_Ques_Desc.Text = Questions_Grid.CurrentRow.Cells["Ques_Desc"].Value.ToString();
                    }
                    else
                    {
                        Mode = string.Empty;
                        txtQues_Code.Enabled = false;
                        txtQues_Code.Text = txtQues_Seq.Text = txtQues_Desc.Text = string.Empty;
                        chkbAssessment.Enabled = false;
                        txtQues_Seq.Enabled = false;
                        cmbQues_Type.Enabled = false;
                        chkNumericType.Enabled = false;
                        txtQues_Desc.Enabled = false;
                        btnQues_Cancel.Visible = false;
                        btnQues_Save.Visible = false;
                        gvQuestions.Enabled = true;
                    }
                }
            }
            if (gvQuestions.Rows.Count > 0)
            { 
                gvQuestions.Rows[selQuesindex].Selected = true;
                gvQuestions.CurrentCell = gvQuestions.Rows[selQuesindex].Cells[1];
            }

            this.gvQuestions.SelectionChanged += new System.EventHandler(this.gvQuestions_SelectionChanged);
          
        }

        string Tab_Page = "Groups";
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Sel_Ques_Index = 0;
                Tab_Page = "Groups";
                fillCmbGrpType();
                FillGroups_Grid();
                txtGrpCode.Enabled = false;
                Group_Controls_Fill();
                Mode = string.Empty;
                this.Size = new Size(606, 494);

            }
            else if (tabControl1.SelectedIndex == 1)
            {
                selQuesindex = 0;
                Tab_Page = "Questions";
                Get_Questions_Related_Data();
                Fill_Questions_Group_Combo();
                fillcmbQuesType();
                // FillQuestions_grid();
                Mode = string.Empty;
                picAddQuestions.Enabled = true;
                txtQues_Code.Enabled = false;
                Questions_Controls_Fill();
                this.Size = new Size(687, 500);


            }
            else
            {
                selRespIndex = 0;
                Tab_Page = "Responses";
                if (ScoreStreet == "O")
                {
                    lblPoints.Visible = false;
                    lblPointsReq.Visible = false;
                    txtPoints.Visible = false;
                    txtPoints.Text = "0";
                    chkbExclude.Visible = false;
                    chkbExclude.Checked = false;
                    this.Size = new Size(660, 462);
                }
                else
                {
                    lblPoints.Visible = true;
                    lblPointsReq.Visible = true;
                    txtPoints.Visible = true;
                    txtPoints.Text = string.Empty;
                    chkbExclude.Visible = true;
                    chkbExclude.Checked = false;

                }
                Get_Questions_Related_Data();
                Fill_RespQuestions_Group_Combo();
                Mode = string.Empty;
                PicAddResponce.Enabled = true;
                txtResp_Code.Enabled = false;
                Responses_Control_Fill();
            }
        }

        private void cmbQues_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillQuestions_grid();
            Mode = string.Empty;
            txtQues_Code.Enabled = false;
            txtQues_Code.Text = txtQues_Seq.Text = txtQues_Desc.Text = string.Empty;
            chkbAssessment.Enabled = false;
            txtQues_Seq.Enabled = false;
            cmbQues_Type.Enabled = false;
            chkNumericType.Enabled = false;
            txtQues_Desc.Enabled = false;
            btnQues_Cancel.Visible = false;
            btnQues_Save.Visible = false;
            gvQuestions.Enabled = true;
            Questions_Controls_Fill();
        }

        private void cmbResp_Grp_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillcmbRespQuestions();

        }

        private void cmbResp_Ques_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillQuesResp_Grid();
            ResponceControlEnable();
            Responses_Control_Fill();

        }
        string strmsgGrp = string.Empty; MATSGRPEntity GrpEntity = new MATSGRPEntity();
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CaptainModel model = new CaptainModel();

                    if (Mode == "Edit")
                        GrpEntity.Rec_Type = "U";
                    GrpEntity.Group = txtGrpCode.Text; 
                    if (txtGrpCode.Text == "New")
                    {
                        GrpEntity.Rec_Type = "I";
                        GrpEntity.Group = "1";
                    }

                    GrpEntity.MatCode = MatrixCode;
                    GrpEntity.SclCode = ScaleCode;
                    GrpEntity.Desc = txtGrp_Desc.Text;
                    GrpEntity.Seq = txtSeq.Text;
                    GrpEntity.GrpType = ((Captain.Common.Utilities.ListItem)cmbGrp_Type.SelectedItem).Value.ToString();

                    if (_model.MatrixScalesData.UpdateMATSGRP(GrpEntity, "Update", out strmsgGrp))
                    {
                        //this.Close();
                        //if (GrpEntity.Rec_Type == "I")
                        //    MessageBox.Show("Group Inserted Successfully...", "CAP Systems");
                        //else
                        //    MessageBox.Show("Group Updated Successfully...", "CAP Systems");

                        // this.DialogResult = DialogResult.OK;
                    }
                    //else
                    //{
                    //    if (GrpEntity.Rec_Type == "I")
                    //        MessageBox.Show("Unsuccessful Group Insert...", "CAP Systems");
                    //    else
                    //        MessageBox.Show("Unsuccessful Group Update...", "CAP Systems");
                    //    //this.DialogResult = DialogResult.OK;
                    //}
                    FillGroups_Grid();
                    txtGrpCode.Enabled = false;
                    picGroupAdd.Enabled = true;
                    txtGrpCode.Text = txtGrp_Desc.Text = txtSeq.Text = string.Empty;
                    cmbGrp_Type.SelectedIndex = 0;
                    txtSeq.Enabled = false;
                    cmbGrp_Type.Enabled = false;
                    txtGrp_Desc.Enabled = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    gvMat_Groups.Enabled = true;
                    Group_Controls_Fill();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool ValidateForm()
        {
            bool isValid = true;
            if (Tab_Page == "Groups")
            {
                if (string.IsNullOrEmpty(txtGrpCode.Text) || string.IsNullOrWhiteSpace(txtGrpCode.Text))
                {
                    _errorProvider.SetError(txtGrpCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGrp_code.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else if (isCodeExists(txtGrpCode.Text.Trim()))
                {
                    _errorProvider.SetError(txtGrpCode, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblGrp_code.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtGrpCode, null);
                }
                if (string.IsNullOrEmpty(txtSeq.Text) || string.IsNullOrWhiteSpace(txtSeq.Text))
                {
                    _errorProvider.SetError(txtSeq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSeq.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtSeq, null);
                }
                if (string.IsNullOrEmpty(txtGrp_Desc.Text) || string.IsNullOrWhiteSpace(txtGrp_Desc.Text))
                {
                    _errorProvider.SetError(txtGrp_Desc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGrp_Desc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtGrp_Desc, null);
                }

            }
            else if (Tab_Page == "Questions")
            {
                if (string.IsNullOrEmpty(txtQues_Code.Text) || string.IsNullOrWhiteSpace(txtQues_Code.Text))
                {
                    _errorProvider.SetError(txtQues_Code, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQues_code.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else if (isCodeExists(txtQues_Code.Text))
                {
                    _errorProvider.SetError(txtQues_Code, "Response code already used for this Question");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtQues_Code, null);
                }
                if (string.IsNullOrEmpty(txtQues_Seq.Text) || string.IsNullOrWhiteSpace(txtQues_Seq.Text))
                {
                    _errorProvider.SetError(txtQues_Seq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQues_seq.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtQues_Seq, null);
                }
                if (string.IsNullOrEmpty(txtQues_Desc.Text) || string.IsNullOrWhiteSpace(txtQues_Desc.Text))
                {
                    _errorProvider.SetError(txtQues_Desc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQues_Desc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtQues_Desc, null);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtResp_Code.Text) || string.IsNullOrWhiteSpace(txtResp_Code.Text))
                {
                    _errorProvider.SetError(txtResp_Code, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResp_Code.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else if (isCodeExists(txtResp_Code.Text))
                {
                    _errorProvider.SetError(txtResp_Code, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblResp_Code.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtResp_Code, null);
                }
                if (!(ScoreStreet == "O"))
                {
                    if (string.IsNullOrEmpty(txtPoints.Text) || string.IsNullOrWhiteSpace(txtPoints.Text))
                    {
                        _errorProvider.SetError(txtPoints, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPoints.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtPoints, null);
                    }
                }
                if (string.IsNullOrEmpty(txtResp_Desc.Text) || string.IsNullOrWhiteSpace(txtResp_Desc.Text))
                {
                    _errorProvider.SetError(txtResp_Desc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResp_Desc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtResp_Desc, null);
                }
            }

            return (isValid);
        }

        private bool isCodeExists(string Code)
        {
            bool isExists = false;
            if (Mode == string.Empty)
            {
                if (Tab_Page == "Groups")
                {
                    MATSGRPEntity Search_Entity = new MATSGRPEntity(true);
                    Search_Entity.MatCode = MatrixCode;
                    Search_Entity.SclCode = ScaleCode;
                    Search_Entity.Group = txtGrpCode.Text;
                    QuesGroups_List = _model.MatrixScalesData.Browse_MATSGRP(Search_Entity, "Browse");
                    if (QuesGroups_List.Count > 0)
                    {
                        isExists = true;
                    }
                }
                else if (Tab_Page == "Questions")
                {
                    MATQUESTEntity Search_Entity = new MATQUESTEntity(true);
                    Search_Entity.MatCode = MatrixCode;
                    Search_Entity.SclCode = ScaleCode;
                    Search_Entity.Group = ((Captain.Common.Utilities.ListItem)cmbQues_Group.SelectedItem).Value.ToString();
                    Search_Entity.Code = txtQues_Code.Text;
                    Questions_List = _model.MatrixScalesData.Browse_MATQUEST(Search_Entity, "Browse");
                    if (Questions_List.Count > 0)
                    {
                        isExists = true;
                    }
                }
                else
                {
                    MATQUESREntity Search_Entity = new MATQUESREntity(true);
                    Search_Entity.MatCode = MatrixCode;
                    Search_Entity.SclCode = ScaleCode;
                    Search_Entity.Group = ((Captain.Common.Utilities.ListItem)cmbResp_Grp.SelectedItem).Value.ToString();
                    Search_Entity.Code = ((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).Value.ToString();
                    Search_Entity.RespCode = txtResp_Code.Text;
                    Responses_List = _model.MatrixScalesData.Browse_MATQUESR(Search_Entity, "Browse");
                    if (Responses_List.Count > 0)
                    {
                        isExists = true;
                    }
                }
            }

            return isExists;
        }
        string strmsgQues = string.Empty; MATQUESTEntity QuesEntity = new MATQUESTEntity();
        private void btnQues_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CaptainModel model = new CaptainModel();

                    if (Mode == "Edit")
                        QuesEntity.Rec_Type = "U";
                    QuesEntity.Code = txtQues_Code.Text;
                    if (txtQues_Code.Text == "New")
                    {
                        QuesEntity.Rec_Type = "I";
                        QuesEntity.Code = "1";
                    }

                    QuesEntity.MatCode = MatrixCode;
                    QuesEntity.SclCode = ScaleCode;
                    QuesEntity.Group = ((Captain.Common.Utilities.ListItem)cmbQues_Group.SelectedItem).Value.ToString();

                    QuesEntity.Desc = txtQues_Desc.Text;
                    QuesEntity.Seq = txtQues_Seq.Text;
                    if (chkbAssessment.Checked)
                        QuesEntity.SsOverride = "1";
                    else
                        QuesEntity.SsOverride = "0";
                    QuesEntity.Type = ((Captain.Common.Utilities.ListItem)cmbQues_Type.SelectedItem).Value.ToString();
                    QuesEntity.AddOperator = Baseform.UserID;
                    QuesEntity.LstcOperator = Baseform.UserID;
                    QuesEntity.QuesNumericType = chkNumericType.Checked == true ? "Y" : string.Empty;

                    if (_model.MatrixScalesData.UpdateMATQUEST(QuesEntity, "Update", out strmsgQues))
                    {
                        //this.Close();
                        //if (QuesEntity.Rec_Type == "I")
                        //    MessageBox.Show("Question Inserted Successfully...", "CAP Systems");
                        //else
                        //    MessageBox.Show("Question Updated Successfully...", "CAP Systems");
                        //MAT00001Control MatrixControl = Baseform.GetBaseUserControl() as MAT00001Control;
                        //if (MatrixControl != null)
                        //{
                        //    MatrixControl.RefreshQues_grid();
                        //}
                        //this.DialogResult = DialogResult.OK;
                    }
                    //else
                    //{
                    //    if (QuesEntity.Rec_Type == "I")
                    //        MessageBox.Show("Unsuccessful Question Insert...", "CAP Systems");
                    //    else
                    //        MessageBox.Show("Unsuccessful Question Update...", "CAP Systems");
                    //}
                    FillQuestions_grid();
                    //  fillcmbQuesType();
                    Mode = string.Empty;
                    txtQues_Code.Enabled = false;
                    txtQues_Code.Text = txtQues_Seq.Text = txtQues_Desc.Text = string.Empty;
                    chkbAssessment.Enabled = false;
                    txtQues_Seq.Enabled = false;
                    cmbQues_Type.Enabled = false;
                    chkNumericType.Enabled = false;
                    txtQues_Desc.Enabled = false;
                    btnQues_Cancel.Visible = false;
                    btnQues_Save.Visible = false;
                    gvQuestions.Enabled = true;
                    picAddQuestions.Enabled = true;
                    Questions_Controls_Fill();
                }
            }
            catch (Exception ex)
            {
            }
        }
        string strmsgResp = string.Empty; MATQUESREntity RespEntity = new MATQUESREntity();
        private void btnResp_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    if (QuestionType == "4" || QuestionType == "5" || QuestionType == "6" || QuestionType == "7")
                    {
                        CommonFunctions.MessageBoxDisplay("You cannot enter responses for date, numeric or alphanumeric questions.");
                    }
                    else
                    {
                        CaptainModel model = new CaptainModel();


                        if (Mode == "Edit")
                        {
                            RespEntity.Rec_Type = "U";
                        }
                        else
                        {
                            RespEntity.Rec_Type = "I";
                        }
                        RespEntity.RespCode = txtResp_Code.Text;
                        RespEntity.MatCode = MatrixCode;
                        RespEntity.SclCode = ScaleCode;
                        RespEntity.Group = ((Captain.Common.Utilities.ListItem)cmbResp_Grp.SelectedItem).Value.ToString();
                        RespEntity.Code = ((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).Value.ToString();
                        RespEntity.RespCode = txtResp_Code.Text;
                        RespEntity.RespDesc = txtResp_Desc.Text;
                        if (chkbExclude.Checked)
                            RespEntity.Exclude = "Y";
                        else
                            RespEntity.Exclude = "N";
                        if (ScoreStreet == "O")
                        {
                            RespEntity.Exclude = "N";
                            RespEntity.Points = "0";
                        }
                        else
                            RespEntity.Points = txtPoints.Text;
                        //RespEntity.Rec_Type = ((ListItem)cmbQues_Type.SelectedItem).Value.ToString();

                        if (_model.MatrixScalesData.UpdateMATQUESR(RespEntity, "Update", out strmsgResp))
                        {
                            //this.Close();
                            //if (RespEntity.Rec_Type == "I")
                            //    MessageBox.Show("Question Response Inserted Successfully...", "CAP Systems");
                            //else
                            //    MessageBox.Show("Question Response Updated Successfully...", "CAP Systems");
                            //MAT00001Control MatrixControl = Baseform.GetBaseUserControl() as MAT00001Control;
                            //if (MatrixControl != null)
                            //{
                            //    MatrixControl.RefreshQues_resp_Grid();
                            //}
                            //  this.DialogResult = DialogResult.OK;
                        }
                        //else
                        //{
                        //    if (RespEntity.Rec_Type == "I")
                        //        MessageBox.Show("Unsuccessful Question Response Insert...", "CAP Systems");
                        //    else
                        //        MessageBox.Show("Unsuccessful Question Response Update...", "CAP Systems");
                        // }

                        FillQuesResp_Grid();
                        ResponceControlEnable();
                        Responses_Control_Fill();
                        PicAddResponce.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string[] GetSelected_Group_Code()
        {
            string[] Added_Edited_GroupCode = new string[5];
            if (Tab_Page == "Groups")
            {
                if (GrpEntity.Rec_Type == "I")
                    Added_Edited_GroupCode[0] = strmsgGrp.Trim();
                else
                    Added_Edited_GroupCode[0] = txtGrpCode.Text;
                Added_Edited_GroupCode[1] = Mode;//MatEntity.Rec_Type;
                Added_Edited_GroupCode[2] = Mode;//MatEntity.Rec_Type;
                Added_Edited_GroupCode[3] = GrpEntity.Rec_Type;
                Added_Edited_GroupCode[4] = Tab_Page;

            }
            else if (Tab_Page == "Questions")
            {
                if (QuesEntity.Rec_Type == "I")
                    Added_Edited_GroupCode[1] = strmsgQues.Trim();
                else
                    Added_Edited_GroupCode[1] = txtQues_Code.Text;
                Added_Edited_GroupCode[0] = ((Captain.Common.Utilities.ListItem)cmbQues_Group.SelectedItem).Value.ToString();//MatEntity.Rec_Type;
                Added_Edited_GroupCode[2] = Mode;//((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).Value.ToString();//MatEntity.Rec_Type;
                Added_Edited_GroupCode[3] = QuesEntity.Rec_Type;
                Added_Edited_GroupCode[4] = Tab_Page;
            }
            else
            {
                if (RespEntity.Rec_Type == "I")
                    Added_Edited_GroupCode[2] = strmsgResp.Trim();
                else
                    Added_Edited_GroupCode[2] = txtResp_Code.Text;
                Added_Edited_GroupCode[0] = ((Captain.Common.Utilities.ListItem)cmbResp_Grp.SelectedItem).Value.ToString();//MatEntity.Rec_Type;
                Added_Edited_GroupCode[1] = ((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).Value.ToString(); ;//MatEntity.Rec_Type;
                Added_Edited_GroupCode[3] = RespEntity.Rec_Type;
                Added_Edited_GroupCode[4] = Tab_Page;
            }

            return Added_Edited_GroupCode;
        }


        private void Group_Controls_Fill()
        {
            if (gvMat_Groups.Rows.Count > 0)
            {
                MATSGRPEntity row = gvMat_Groups.SelectedRows[0].Tag as MATSGRPEntity;
                if (row != null)
                {
                    txtGrp_Desc.Enabled = false;
                    txtSeq.Enabled = false;
                    cmbGrp_Type.Enabled = false;
                    txtGrpCode.Text = gvMat_Groups.CurrentRow.Cells["Group_Code"].Value.ToString();
                    txtGrp_Desc.Text = row.Desc;
                    txtSeq.Text = row.Seq;
                    SetComboBoxValue(cmbGrp_Type, row.GrpType);
                }
            }
            else
            {
                txtGrpCode.Enabled = false;
                txtGrpCode.Text = txtGrp_Desc.Text = txtSeq.Text = string.Empty;
                cmbGrp_Type.SelectedIndex = 0;
                txtSeq.Enabled = false;
                cmbGrp_Type.Enabled = false;
                txtGrp_Desc.Enabled = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                gvMat_Groups.Enabled = true;
            }
        }

        private void Questions_Controls_Fill()
        {
            if (gvQuestions.Rows.Count > 0)
            {
                MATQUESTEntity QuesEntity = gvQuestions.SelectedRows[0].Tag as MATQUESTEntity;
                if (QuesEntity != null)
                {
                    SetComboBoxValue(cmbQues_Group, QuesEntity.Group);
                    txtQues_Code.Text = gvQuestions.CurrentRow.Cells["Ques_Code"].Value.ToString();
                    txtQues_Desc.Text = QuesEntity.Desc;
                    txtQues_Seq.Text = QuesEntity.Seq;
                    if (QuesEntity.SsOverride == "1")
                        chkbAssessment.Checked = true;
                    else
                        chkbAssessment.Checked = false;


                    SetComboBoxValue(cmbQues_Type, QuesEntity.Type);
                    chkNumericType.Checked = QuesEntity.QuesNumericType == "Y" ? true : false;
                    if (QuesEntity.Type == "1")
                    {
                        chkNumericType.Visible = true;
                    }
                    else
                        chkNumericType.Visible = false;
                }
            }
            else
            {
                Mode = string.Empty;
                txtQues_Code.Enabled = false;
                txtQues_Code.Text = txtQues_Seq.Text = txtQues_Desc.Text = string.Empty;
                chkbAssessment.Enabled = false;
                txtQues_Seq.Enabled = false;
                cmbQues_Type.Enabled = false;
                chkNumericType.Enabled = false;
                txtQues_Desc.Enabled = false;
                btnQues_Cancel.Visible = false;
                btnQues_Save.Visible = false;
                gvQuestions.Enabled = true;
            }
        }

        private void Responses_Control_Fill()
        {
            if (gvResponses.Rows.Count > 0)
            {
                MATQUESREntity RespEntity = gvResponses.SelectedRows[0].Tag as MATQUESREntity;
                if (RespEntity != null)
                {
                    SetComboBoxValue(cmbResp_Grp, RespEntity.Group);
                    // SetComboBoxValue(cmbResp_Ques, RespEntity.RespCode);
                    txtResp_Code.Text = gvResponses.CurrentRow.Cells["Resp_Code"].Value.ToString();
                    txtResp_Desc.Text = RespEntity.RespDesc;
                    txtPoints.Text = RespEntity.Points;
                    if (RespEntity.Exclude == "Y")
                        chkbExclude.Checked = true;
                    else
                        chkbExclude.Checked = false;
                }
            }
            else
            {
                Mode = string.Empty;
                txtResp_Code.Enabled = false;
                // cmbResp_Ques.Enabled = false;
                txtResp_Code.Text = txtResp_Desc.Text = txtPoints.Text = string.Empty;
                // cmbResp_Grp.Enabled = false;
                txtResp_Desc.Enabled = false;
                txtPoints.Enabled = false;
                chkbExclude.Enabled = false;
                btnResp_Cancel.Visible = false;
                btnResp_Save.Visible = false;
                gvResponses.Enabled = true;
            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (Captain.Common.Utilities.ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void gvMat_Groups_SelectionChanged(object sender, EventArgs e)
        {
            //Sel_Ques_Index = gvMat_Groups.CurrentRow.Index;
            Group_Controls_Fill();
        }

        int selQuesindex = 0;
        private void gvQuestions_SelectionChanged(object sender, EventArgs e)
        {
            selQuesindex= gvQuestions.CurrentRow.Index;
            Questions_Controls_Fill();
        }

        int selRespIndex = 0;
        private void gvResponses_SelectionChanged(object sender, EventArgs e)
        {
            selRespIndex= gvResponses.CurrentRow.Index;
            Responses_Control_Fill();
        }

        private void btnResp_Cancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtResp_Desc, null);
            _errorProvider.SetError(txtResp_Code, null);
            _errorProvider.SetError(txtPoints, null);
            ResponceControlEnable();
            if (gvResponses.Rows.Count > 0)
            {
                MATQUESREntity RespEntity = gvResponses.SelectedRows[0].Tag as MATQUESREntity;
                if (RespEntity != null)
                {
                    SetComboBoxValue(cmbResp_Grp, RespEntity.Group);
                    // SetComboBoxValue(cmbResp_Ques, RespEntity.RespCode);
                    txtResp_Code.Text = gvResponses.CurrentRow.Cells["Resp_Code"].Value.ToString();
                    txtResp_Desc.Text = RespEntity.RespDesc;
                    txtPoints.Text = RespEntity.Points;
                    if (RespEntity.Exclude == "Y")
                        chkbExclude.Checked = true;
                    else
                        chkbExclude.Checked = false;
                }
            }
            PicAddResponce.Enabled = true;

        }

        private void btnQues_Cancel_Click(object sender, EventArgs e)
        {
            Mode = string.Empty;
            txtQues_Code.Enabled = false;
            txtQues_Code.Text = txtQues_Seq.Text = txtQues_Desc.Text = string.Empty;
            _errorProvider.SetError(txtQues_Seq, null);
            _errorProvider.SetError(txtQues_Desc, null);
            chkbAssessment.Enabled = false;
            txtQues_Seq.Enabled = false;
            cmbQues_Type.Enabled = false;
            chkNumericType.Enabled = false;
            txtQues_Desc.Enabled = false;
            btnQues_Cancel.Visible = false;
            btnQues_Save.Visible = false;
            picAddQuestions.Enabled = true;
            gvQuestions.Enabled = true;
            Questions_Controls_Fill();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtSeq, null);
            _errorProvider.SetError(txtGrp_Desc, null);
            txtGrpCode.Enabled = false;
            txtGrpCode.Text = txtGrp_Desc.Text = txtSeq.Text = string.Empty;
            cmbGrp_Type.SelectedIndex = 0;
            txtSeq.Enabled = false;
            cmbGrp_Type.Enabled = false;
            txtGrp_Desc.Enabled = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
            picGroupAdd.Enabled = true;
            gvMat_Groups.Enabled = true;
            Group_Controls_Fill();

        }

        private void Clear_Controls()
        {
            txtGrp_Desc.Clear();
            //txtGrpCode.Clear();
            txtSeq.Clear();
            //txtQues_Code.Clear();
            txtQues_Desc.Clear();
            txtQues_Seq.Clear();
            //txtResp_Code.Clear();
            txtResp_Desc.Clear();
            txtPoints.Clear();
            chkbAssessment.Checked = false;
            chkbExclude.Checked = false;
        }

        private void ResponceControlEnable()
        {
            Mode = string.Empty;
            txtResp_Code.Enabled = false;
            // cmbResp_Ques.Enabled = false;
            txtResp_Code.Text = txtResp_Desc.Text = txtPoints.Text = string.Empty;
            // cmbResp_Grp.Enabled = false;
            txtResp_Desc.Enabled = false;
            txtPoints.Enabled = false;
            chkbExclude.Enabled = false;
            btnResp_Cancel.Visible = false;
            btnResp_Save.Visible = false;
            gvResponses.Enabled = true;
            // Responses_Control_Fill();
        }



        //PdfContentByte cb;
        //int X_Pos, Y_Pos;
        //string strFolderPath = string.Empty;
        //string Random_Filename = null;
        //int pageNumber = 1;
        //string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        //string PrintText = null;
        //private void On_SaveForm_Closed()
        //{
        //    Random_Filename = null;
        //    string PdfName = "Pdf File";
        //    PdfName = PdfName + "Report";//form.GetFileName();
        //    PdfName = strFolderPath + PdfName;

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    FileStream fs = new FileStream(PdfName, FileMode.Create);

        //    Document document = new Document();
        //    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();
        //    cb = writer.DirectContent;
        //}

        private void txtGrpCode_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGrpCode.Text))
                _errorProvider.SetError(txtGrpCode, null);
        }

        private void txtSeq_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSeq.Text))
            {
                if (int.Parse(txtSeq.Text.Trim()) < 0)
                    _errorProvider.SetError(txtSeq, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                else
                    _errorProvider.SetError(txtSeq, null);
            }
        }

        private void txtGrp_Desc_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGrp_Desc.Text))
                _errorProvider.SetError(txtGrp_Desc, null);
        }

        private void txtQues_Code_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQues_Code.Text))
                _errorProvider.SetError(txtQues_Code, null);
        }

        private void txtQues_Seq_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQues_Seq.Text))
            {
                if (int.Parse(txtQues_Seq.Text.Trim()) < 0)
                    _errorProvider.SetError(txtQues_Seq, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                else
                    _errorProvider.SetError(txtQues_Seq, null);
            }
        }

        private void txtQues_Desc_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQues_Desc.Text))
                _errorProvider.SetError(txtQues_Desc, null);
        }

        private void txtResp_Code_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResp_Code.Text))
                _errorProvider.SetError(txtResp_Code, null);
        }

        private void txtPoints_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPoints.Text))
            {
                if (int.Parse(txtPoints.Text.Trim()) < 0)
                    _errorProvider.SetError(txtPoints, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                else
                    _errorProvider.SetError(txtPoints, null);
            }
        }

        private void txtResp_Desc_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResp_Desc.Text))
                _errorProvider.SetError(txtResp_Desc, null);
        }
        string deleteSelRow = string.Empty;
        private void gvMat_Groups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == EditGroup.Index && e.RowIndex != -1)
            {
                txtGrpCode.Enabled = false;
                txtSeq.Enabled = true;
                cmbGrp_Type.Enabled = true;
                txtGrp_Desc.Enabled = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                gvMat_Groups.Enabled = false;
                picGroupAdd.Enabled = false;
                txtSeq.Focus();
                Mode = "Edit";

                deleteSelRow = string.Empty;
                Sel_Ques_Index = gvMat_Groups.CurrentRow.Index;
            }
            else if (e.ColumnIndex == Del_Group.Index && e.RowIndex != -1)
            {
                deleteSelRow = "D";
                strIndex = gvMat_Groups.SelectedRows[0].Index;
                //strPageIndex = gvMat_Groups.CurrentPage;
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Groups);
            }
        }

        private void Delete_Groups(DialogResult dialogResult)
        {
            // Wisej.Web.Form senderform = (Wisej.Web.Form)sender;
            string Strmsg = string.Empty;
            //if (senderform != null)
            //{
            if (dialogResult == DialogResult.Yes)
            {
                MATSGRPEntity Search_Entity = new MATSGRPEntity();
                Search_Entity.Rec_Type = "D";
                string strmsg = string.Empty;
                Search_Entity.MatCode = MatrixCode;
                Search_Entity.SclCode = ScaleCode;
                Search_Entity.Group = gvMat_Groups.CurrentRow.Cells["Group_Code"].Value.ToString();
                Search_Entity.Desc = "Delete Benc Desc";

                //string GroupCd = gvMat_Groups.CurrentRow.Cells["Group_Code"].Value.ToString();
                if (_model.MatrixScalesData.UpdateMATSGRP(Search_Entity, "Delete", out strmsg))
                {
                    //MessageBox.Show("Group Deleted Successfully...", "CAP Systems");
                    Get_Questions_Related_Data();
                    FillGroups_Grid();//Get_BenchMarks_List();
                                      //Fill_Sel_Matrix_Benchmarks();
                    MAT00001Control MatrixControl = Baseform.GetBaseUserControl() as MAT00001Control;
                    if (MatrixControl != null)
                    {
                        // MatrixControl.RefreshcmbGroup();
                    }

                }
                else
                    if (strmsg == "Already Exist")
                    AlertBox.Show("Group cannot be deleted because questions have been defined for this group.", MessageBoxIcon.Warning); //MessageBox.Show("Group cannot be deleted because questions have been defined for this group.", "CAP Systems", MessageBoxButtons.OK);
                // else
                // MessageBox.Show("Group Deleted Unsuccessful...", "CAP Systems");
                if (Mode == "Edit")
                {
                    if (gvMat_Groups.Rows.Count < 1)
                        this.Close();
                }
            }
            // }
        }

        private void gvQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == EditQuestions.Index && e.RowIndex != -1)
            {

                Mode = "Edit";
                txtQues_Code.Enabled = false;
                btnQues_Cancel.Visible = true;
                btnQues_Save.Visible = true;
                gvQuestions.Enabled = false;
                chkbAssessment.Enabled = true;
                txtQues_Seq.Enabled = true;
                cmbQues_Type.Enabled = true;
                chkNumericType.Enabled = true;
                txtQues_Desc.Enabled = true;
                txtQues_Seq.Focus();
                picAddQuestions.Enabled = false;
                Questions_Controls_Fill();
            }
            if (e.ColumnIndex == Delete_Ques.Index && e.RowIndex != -1)
            {
                //strIndex = gvQuestions.SelectedRows[0].Index;
                //strPageIndex = gvQuestions.CurrentPage;
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Questions);
            }
        }

        private void Delete_Questions(DialogResult dialogResult)
        {
            // Wisej.Web.Form senderform = (Wisej.Web.Form)sender;
            string Strmsg = string.Empty;
            //if (senderform != null)
            //{
            if (dialogResult == DialogResult.Yes)
            {
                MATQUESTEntity Search_Entity = new MATQUESTEntity();
                Search_Entity.Rec_Type = "D";
                string strmsg = string.Empty;
                Search_Entity.MatCode = MatrixCode;
                Search_Entity.SclCode = ScaleCode;
                Search_Entity.Group = ((Captain.Common.Utilities.ListItem)cmbQues_Group.SelectedItem).Value.ToString();
                Search_Entity.Code = gvQuestions.CurrentRow.Cells["Ques_Code"].Value.ToString();
                Search_Entity.Desc = "Delete Benc Desc";

                if (_model.MatrixScalesData.UpdateMATQUEST(Search_Entity, "Delete", out strmsg))
                {
                    // MessageBox.Show("Question Deleted Successfully’", "CAP Systems");                       
                    FillQuestions_grid();
                    Questions_Controls_Fill();
                    //MAT00001Control MatrixControl = Baseform.GetBaseUserControl() as MAT00001Control;
                    //if (MatrixControl != null)
                    //{
                    //  //  MatrixControl.RefreshQues_grid();
                    //}
                }
                else
                    if (strmsg == "Already Exist")
                    AlertBox.Show("Question cannot be deleted because responses have been defined", MessageBoxIcon.Warning);//MessageBox.Show("Question cannot be deleted because responses have been defined", "CAP Systems", MessageBoxButtons.OK);
                // else
                //   MessageBox.Show("Question Deleted UnSuccessfully’", "CAP Systems");
            }
            // }
        }

        private void gvResponses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == EditResponce.Index && e.RowIndex != -1)
            {
                Mode = "Edit";
                txtResp_Code.Enabled = false;
                txtResp_Desc.Enabled = true;
                txtPoints.Enabled = true;
                chkbExclude.Enabled = true;
                btnResp_Cancel.Visible = true;
                btnResp_Save.Visible = true;
                gvResponses.Enabled = false;
                PicAddResponce.Enabled = false;
                txtResp_Desc.Focus();
                if (QuestionNType == "Y")
                {
                    txtResp_Desc.Validator = TextBoxValidation.IntegerValidator;
                }
                else
                {
                    txtResp_Desc.Validator = null;
                }

            }

            if (e.ColumnIndex == Delete_Resp.Index && e.RowIndex != -1)
            {
                //strIndex = gvResponses.SelectedRows[0].Index;
                //strPageIndex = gvResponses.CurrentPage;
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_QuesResponses);
            }
        }

        private void Delete_QuesResponses(DialogResult dialogResult)
        {
            // Wisej.Web.Form senderform = (Wisej.Web.Form)sender;
            string Strmsg = string.Empty;
            //if (senderform != null)
            //{
            if (dialogResult == DialogResult.Yes)
            {
                MATQUESREntity Search_Entity = new MATQUESREntity();
                Search_Entity.Rec_Type = "D";
                string strmsg = string.Empty;
                Search_Entity.MatCode = MatrixCode;
                Search_Entity.SclCode = ScaleCode;
                Search_Entity.Group = ((Captain.Common.Utilities.ListItem)cmbResp_Grp.SelectedItem).Value.ToString();
                Search_Entity.Code = ((Captain.Common.Utilities.ListItem)cmbResp_Ques.SelectedItem).Value.ToString();
                Search_Entity.RespCode = gvResponses.CurrentRow.Cells["Resp_Code"].Value.ToString();
                Search_Entity.RespDesc = "Delete Benc Desc";

                if (_model.MatrixScalesData.UpdateMATQUESR(Search_Entity, "Delete", out strmsg))
                {
                    //  MessageBox.Show("Response Deleted Successfully", "CAP Systems");
                    FillQuesResp_Grid();
                    ResponceControlEnable();
                    Responses_Control_Fill();
                    //MAT00001Control MatrixControl = Baseform.GetBaseUserControl() as MAT00001Control;
                    //if (MatrixControl != null)
                    //{
                    //    //MatrixControl.RefreshQues_resp_Grid();
                    //}
                }
                else
                    if (strmsg == "Already Exist")
                    AlertBox.Show("This response is being used and cannot be deleted", MessageBoxIcon.Warning);//MessageBox.Show("This response is being used and cannot be deleted", "CAP Systems", MessageBoxButtons.OK);
                // else
                // MessageBox.Show("Response Deleted UnSuccessfully", "CAP Systems");
            }
            // }
        }

        private void picGroupAdd_Click(object sender, EventArgs e)
        {
            Mode = string.Empty;
            txtGrpCode.Enabled = false;
            txtGrpCode.Text = "New";
            txtGrp_Desc.Text = txtSeq.Text = string.Empty;
            cmbGrp_Type.SelectedIndex = 0;
            txtSeq.Enabled = true;
            cmbGrp_Type.Enabled = true;
            txtGrp_Desc.Enabled = true;
            btnCancel.Visible = true;
            btnSave.Visible = true;
            gvMat_Groups.Enabled = false;
            picGroupAdd.Enabled = false;
            txtSeq.Focus();

            deleteSelRow = string.Empty;
        }

        private void PicAddResponce_Click(object sender, EventArgs e)
        {
            if (QuestionType == "4" || QuestionType == "5" || QuestionType == "6" || QuestionType == "7")
            {
                CommonFunctions.MessageBoxDisplay("You cannot enter responses for date, numeric or alphanumeric questions.");
            }
            else
            {
                if (QuestionNType == "Y")
                {
                    txtResp_Desc.Validator = TextBoxValidation.IntegerValidator;
                }
                else
                {
                    txtResp_Desc.Validator = null;
                }
                Mode = string.Empty;
                txtResp_Code.Enabled = true;
                txtResp_Code.Text = txtResp_Desc.Text = txtPoints.Text = string.Empty;
                // cmbResp_Ques.Enabled = true;           
                // cmbResp_Grp.Enabled = true;
                txtResp_Desc.Enabled = true;
                txtPoints.Enabled = true;
                chkbExclude.Checked = false;
                chkbExclude.Enabled = true;
                btnResp_Cancel.Visible = true;
                btnResp_Save.Visible = true;
                gvResponses.Enabled = false;
                PicAddResponce.Enabled = false;
                txtResp_Code.Focus();
            }
        }

        private void picAddQuestions_Click(object sender, EventArgs e)
        {
            Mode = string.Empty;
            txtQues_Code.Enabled = false;
            txtQues_Code.Text = "New";
            txtQues_Desc.Text = txtQues_Seq.Text = string.Empty;
            cmbQues_Type.SelectedIndex = 0;
            cmbQues_Type_SelectedIndexChanged(sender, e);
            chkbAssessment.Checked = false;
            chkbAssessment.Enabled = true;
            txtQues_Seq.Enabled = true;
            cmbQues_Type.Enabled = true;
            chkNumericType.Enabled = true;
            chkNumericType.Checked = false;
            txtQues_Desc.Enabled = true;
            btnQues_Cancel.Visible = true;
            btnQues_Save.Visible = true;
            gvQuestions.Enabled = false;
            picAddQuestions.Enabled = false;
            txtQues_Seq.Focus();
        }

        private void MAT_Questions_Load(object sender, EventArgs e)
        {
            tabControl1_SelectedIndexChanged(sender, e);
        }

        private void cmbQues_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbQues_Type.SelectedItem) != null)
            {
                if (((Utilities.ListItem)cmbQues_Type.SelectedItem).Value.ToString() == "1")
                {
                    chkNumericType.Visible = true;
                    chkbAssessment.Location = new Point(423, 12);
                }
                else
                {
                    chkNumericType.Visible = false;
                    chkbAssessment.Location = new Point(314, 12);
                }
            }
        }

        private void MAT_Questions_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 5, Baseform.BusinessModuleID.ToString()), target: "_blank");
        }
    }
}