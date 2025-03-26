#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using Wisej.Web;
using System.Drawing;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using System.Text.RegularExpressions;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Matrix_Scales_DefinitionsForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public Matrix_Scales_DefinitionsForm(BaseForm baseform, string mode, string Matrix_Code, string Scale_Code, PrivilegeEntity priviliges, string strtype)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();

            Baseform = baseform;
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            Priviliges = priviliges;
            MatrixCode = Matrix_Code;
            ScaleCode = Scale_Code;
            Mode = mode;
            strType = strtype;
            //txtScale_Code.Validator = TextBoxValidation.IntegerValidator;
            txtScale_Seq.Validator = TextBoxValidation.IntegerValidator;
            txtInterval.Validator = TextBoxValidation.IntegerValidator;
            Txt_Bench_Low.Validator = TextBoxValidation.IntegerValidator;
            Txt_Bench_High.Validator = TextBoxValidation.IntegerValidator;
            txt_Over_Low.Validator = TextBoxValidation.IntegerValidator;
            txt_Over_High.Validator = TextBoxValidation.IntegerValidator;
            //if (Scale_Code == "No Scales")
            //{
            //    tabControl1.TabPages[1].Hide();
            //}
            //else
            //    tabControl1.TabPages[1].Show();
            //if (Tab_page == "Matrix")
            //{
            //    FillAssesment_dates();
            //    FillScore_Sheets();
            //    if (Mode == "Add")
            //    {
            //        this.Text = priviliges.Program + " - Add";
            //        this.pictureBox2.Location = new System.Drawing.Point(433, 17);
            //        string strCode = DatabaseLayer.MatrixDB.GetMATDEF_MATCODE("M",null);
            //        txtCode.Text = strCode;
            //        txtScale_Code.Text = "New";

            //    }
            //    else
            //    {
            //        this.Text = priviliges.Program + " - Edit";
            //        fillMatrix_Controls();
            //    }
            //}
            //else
            //{
            //    this.pictureBox2.Location = new System.Drawing.Point(565, 17);
            //    FillCOmboMatrix();
            //    Get_BenchMarks_List();

            //}
            this.Text = "Matrix Maintenance" + " - " + Mode;
            if (strType == "Matrix")
            {
                pnlMatrix.Visible = true;
                pnlScale.Visible = false;
                pnlBenchMarks.Visible = false;
               // this.pnlMatrix.Location = new System.Drawing.Point(0, 55);
                Size = new Size(443, 382);
                chkbActive.Location = new Point(131,11);
                //this.pictureBox2.Size = new System.Drawing.Size(20, 24);
                //this.pictureBox2.Location = new System.Drawing.Point(443, 15);
                FillAssesment_dates();
                FillScore_Sheets();
                //this.Text = "Matrix Maintenance"; 
                if (Mode == "Add")
                {
                    string strCode = DatabaseLayer.MatrixDB.GetMATDEF_MATCODE("M", null);
                    txtCode.Text = strCode;
                    if (((ListItem)cmbAssDates.SelectedItem).Value.ToString() == "M")
                        btnMatAssDates.Visible = true;
                    else
                        btnMatAssDates.Visible = false;
                }
                else
                {
                    btnScaleGrops.Visible = true;
                    if (((ListItem)cmbAssDates.SelectedItem).Value.ToString() == "M")
                        btnMatAssDates.Visible = true;
                    else
                        btnMatAssDates.Visible = false;
                    Get_Dates_ForMatrix();
                    fillMatrix_Controls();
                }

            }
            else if (strType == "Scale")
            {
                this.Text = "Scales Maintenance" + " - " + Mode;
                // this.Text = "Scales Maintenance";
                FillCOmboMatrix();
                FillAssessment_Types();
                CommonFunctions.SetComboBoxValue(cmbMatrix, MatrixCode);
                FillGroupCombo(MatrixCode);
                Get_BenchMarks_List();
                pnlMatrix.Visible = false;
                pnlScale.Visible = true;
                pnlBenchMarks.Visible = false;
                cmbAssessmentType.Visible = false;
                lblAssType.Visible = false;
               // this.pnlScale.Location = new System.Drawing.Point(0, 55);
                //this.pictureBox2.Location = new System.Drawing.Point(550, 15);
                //this.pictureBox2.Size = new System.Drawing.Size(20, 24);
                Size = new Size(793, 465);

                cmbMatrix.Enabled = false;
                if (propAgencyControlDetails != null)
                {
                    if (propAgencyControlDetails.MatAssesment.ToUpper() == "Y")
                    {
                        cmbAssessmentType.Visible = true;
                        lblAssType.Visible = true;
                    }
                }

                if (Mode == "Add")
                {
                    txtScale_Code.Text = "New";
                    CommonFunctions.SetComboBoxValue(cmbAssessmentType, "B");
                }
                else
                {
                    FillScale_Controls();
                }
            }
            else if (strType == "BenchMarks")
            {
                // this.Text = "Benchmark";
                this.Text = "BenchMarks" + " - " + Mode;
                FillCOmboMatrix();
                pnlMatrix.Visible = false;
                pnlScale.Visible = false;
                pnlBenchMarks.Visible = true;
               // this.pnlBenchMarks.Location = new System.Drawing.Point(0, 55);
                Size = new Size(425, 325); 
                //this.pictureBox2.Location = new System.Drawing.Point(329, 15);
                //this.pictureBox2.Size = new System.Drawing.Size(20, 24);
                CommonFunctions.SetComboBoxValue(cmbBenchMatrix, MatrixCode);
                cmbBenchMatrix.Enabled = false;
                FillScoreType(((ListItem)cmbBenchMatrix.SelectedItem).ID.ToString(), ((ListItem)cmbBenchMatrix.SelectedItem).ValueDisplayCode.ToString());
                if (((ListItem)cmbBenchMatrix.SelectedItem).ID.ToString() == "Y" || ((ListItem)cmbBenchMatrix.SelectedItem).ValueDisplayCode.ToString() == "Y")
                    cmbScoreType.Visible = true;
                else
                    cmbScoreType.Visible = false;

                MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);
                Benchmarks_List = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");
                if (Mode == "Add")
                {
                    Txt_Bench_Code.Text = "New";
                }
                else
                {
                    Get_BenchMarkFillData();
                }
            }

        }

        #region properties

        public BaseForm Baseform { get; set; }

        public PrivilegeEntity Priviliges { get; set; }

        public string MatrixCode { get; set; }

        public string RankCode { get; set; }

        public string ScaleCode { get; set; }

        public string BenchmarkCode { get; set; }

        public string Mode { get; set; }

        public string strType { get; set; }

        public AgencyControlEntity propAgencyControlDetails { get; set; }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }

        #endregion

        bool Hie_Changed_FLG = false;

        private void FillAssesment_dates()
        {
            cmbAssDates.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Applicant Defined Assessment Dates", "A"));
            listItem.Add(new ListItem("Matrix Defined Assessment Dates", "M"));
            cmbAssDates.Items.AddRange(listItem.ToArray());
            cmbAssDates.SelectedIndex = 0;

            cmbMethd.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("Up", "U"));
            listItem.Add(new ListItem("Down", "D"));
            cmbMethd.Items.AddRange(listItem.ToArray());
            cmbMethd.SelectedIndex = 0;
        }

        private void FillScore_Sheets()
        {
            cmbSSheets.Items.Clear();
            List<ListItem> ListSheets = new List<ListItem>();
            ListSheets.Add(new ListItem("Response Average Method", "R"));
            ListSheets.Add(new ListItem("Outcome Criteria Method", "O"));
            ListSheets.Add(new ListItem("Sum of All Points", "S"));
            cmbSSheets.Items.AddRange(ListSheets.ToArray());
            cmbSSheets.SelectedIndex = 0;
        }
        private void FillScoreType(string strTotal, string strPartial)
        {
            cmbScoreType.Items.Clear();
            List<ListItem> ListSheets = new List<ListItem>();
            ListSheets.Add(new ListItem("   ", "0"));
            if (strTotal == "Y")
                ListSheets.Add(new ListItem("Total Score", "T"));
            if (strPartial == "Y")
                ListSheets.Add(new ListItem("Partial Score", "P"));
            cmbScoreType.Items.AddRange(ListSheets.ToArray());
            cmbScoreType.SelectedIndex = 0;
        }
        List<MATDEFEntity> MATDEF_List = new List<MATDEFEntity>();
        private void FillCOmboMatrix()
        {
            cmbMatrix.Items.Clear();
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            int Tmp_Count = 0;
            foreach (MATDEFEntity entity in MATDEF_List)
            {
                if (entity.Scale_Code == "0")
                {
                    cmbMatrix.Items.Add(new ListItem(entity.Desc, entity.Mat_Code, entity.OverlScor, entity.SpecScor));
                    cmbBenchMatrix.Items.Add(new ListItem(entity.Desc, entity.Mat_Code, entity.OverlScor, entity.SpecScor));
                    Tmp_Count++;
                }
            }
            if (Tmp_Count > 0)
            {
                cmbMatrix.SelectedIndex = 0;
                cmbBenchMatrix.SelectedIndex = 0;
            }



        }

        private void FillComboScales()
        {
            cmbMatrix.Items.Clear();
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            int Tmp_Count = 0;
            foreach (MATDEFEntity entity in MATDEF_List)
            {
                if (entity.Mat_Code == MatrixCode && entity.Scale_Code != "0")
                {
                    cmbMatrix.Items.Add(new ListItem(entity.Desc, entity.Scale_Code));
                    Tmp_Count++;
                }
            }
            if (Tmp_Count > 0)
                cmbMatrix.SelectedIndex = int.Parse(ScaleCode) - 1;
        }

        private void FillAssessment_Types()
        {
            cmbAssessmentType.Items.Clear();
            List<ListItem> ListAssesmentType = new List<ListItem>();
            ListAssesmentType.Add(new ListItem("Both", "B"));
            ListAssesmentType.Add(new ListItem("Households", "H"));
            ListAssesmentType.Add(new ListItem("Individual", "I"));
            cmbAssessmentType.Items.AddRange(ListAssesmentType.ToArray());
            cmbAssessmentType.SelectedIndex = 0;
        }

        private void FillGroupCombo(string strMatCode)
        {
            gvwGroups.Rows.Clear();
            List<MATGroupEntity> Group_List = _model.MatrixScalesData.GetSCLGRPS(strMatCode, string.Empty);
            foreach (MATGroupEntity groupitem in Group_List)
            {
                gvwGroups.Rows.Add(false, groupitem.ShortName, groupitem.Desc, groupitem.Code);
            }

        }

        private void Get_Dates_ForMatrix()
        {
            if (Mode == "Edit")
            {
                MATDEFDTEntity Search_DEFDT_Entity = new MATDEFDTEntity(true);
                Search_DEFDT_Entity.MatCode = MatrixCode;
                sel_list = _model.MatrixScalesData.Browse_MATDEFDT(Search_DEFDT_Entity, "Browse");
            }
        }


        List<MATDEFBMEntity> Benchmarks_List = new List<MATDEFBMEntity>();
        private void Get_BenchMarks_List()
        {
            MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);
            Benchmarks_List = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");
            cmbMatBm.Items.Clear();
            List<ListItem> listMatDefBM = new List<ListItem>();
            int Tmp_Count = 0;
            foreach (MATDEFBMEntity drMatdefBM in Benchmarks_List)
            {
                if (Mode == "Add")
                {
                    if (drMatdefBM.MatCode.ToString().Trim() == ((ListItem)cmbMatrix.SelectedItem).Value.ToString().Trim())
                    {
                        cmbMatBm.Items.Add(new ListItem(drMatdefBM.Desc.ToString(), drMatdefBM.Code.ToString()));
                        Tmp_Count++;
                    }
                }
                else
                {
                    if (drMatdefBM.MatCode.ToString().Trim() == MatrixCode.ToString().Trim())
                    {
                        cmbMatBm.Items.Add(new ListItem(drMatdefBM.Desc.ToString(), drMatdefBM.Code.ToString()));
                        Tmp_Count++;
                    }
                }
            }
            cmbMatBm.Items.Insert(0, new ListItem("    ", "0"));
            cmbMatBm.SelectedIndex = 0;
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(Baseform, SelectedHierarchies, "Add", "I", "*", string.Empty,Priviliges);
            addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            addForm.StartPosition = FormStartPosition.CenterScreen;
            addForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            //TagClass selectedTabTagClass = Baseform.ContentTabs.SelectedItem.Tag as TagClass;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                // HierarchyGrid.Rows.Clear();
                Hie_Changed_FLG = true; HierarchyGrid.Rows.Clear();
                if (selectedHierarchies.Count > 0)
                {
                    string Agy = "**", Dept = "**", Prog = "**";
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        Agy = Dept = Prog = "**";
                        if (!string.IsNullOrEmpty(row.Agency.Trim()))
                            Agy = row.Agency.Trim();

                        if (!string.IsNullOrEmpty(row.Prog.Trim()))
                            Prog = row.Prog.Trim();

                        if (!string.IsNullOrEmpty(row.Dept.Trim()))
                            Dept = row.Dept.Trim();

                        int rowIndex = HierarchyGrid.Rows.Add(string.Empty, row.Code + "  " + row.HirarchyName.ToString(), Agy + Dept + Prog);
                        HierarchyGrid.Rows[rowIndex].Tag = row;

                        //hierarchy += row.Agency + row.Dept + row.Prog;
                        hierarchy += row.Code.Substring(0, 2) + row.Code.Substring(3, 2) + row.Code.Substring(6, 2);
                    }

                }
                if (HierarchyGrid.RowCount > 0)
                    _errorProvider.SetError(HierarchyGrid, null);
                //else
                //    hierarchy = string.Empty;
            }
        }

        private void cmbAssDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbAssDates.SelectedItem).Value.ToString() == "M")
            {
                lblInterval.Visible = false; 
                txtInterval.Clear(); _errorProvider.SetError(txtInterval, null);
                txtInterval.Visible = false;
                lblReqInterval.Visible = false;
                Cb_Bench_ShowAssment.Location = new Point(127,140);
                btnMatAssDates.Visible = true;
            }
            else
            {
                lblInterval.Visible = true;
                txtInterval.Visible = true;
                lblReqInterval.Visible = true;
                Cb_Bench_ShowAssment.Location = new Point(187, 140);
                btnMatAssDates.Visible = false;
            }
        }
        List<MATDEFDTEntity> sel_list = new List<MATDEFDTEntity>();
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    int NewCode = 0;
                    CaptainModel model = new CaptainModel();
                    MATDEFEntity MatEntity = new MATDEFEntity();
                    MATDEFDTEntity MatDateEntity = new MATDEFDTEntity();
                    if (Mode == "Add")
                        MatEntity.Rec_Type = "I";
                    else
                        MatEntity.Rec_Type = "U";
                    MatEntity.Rec_mode = "M";
                    MatEntity.Mat_Code = txtCode.Text;
                    MatEntity.Desc = txtDesc.Text;
                    MatEntity.Scale_Code = "0";
                    if (chkbActive.Checked == true)
                        MatEntity.Active = "A";
                    else
                        MatEntity.Active = "I";
                    MatEntity.Score = ((ListItem)cmbSSheets.SelectedItem).Value.ToString();
                    MatEntity.Date_option = (((ListItem)cmbAssDates.SelectedItem).Value.ToString());
                    if (MatEntity.Date_option == "A")
                        MatEntity.Interval = txtInterval.Text.Trim('-');
                    else
                        MatEntity.Interval = "";
                    if (Cb_Bench_ShowAssment.Checked == true)
                        MatEntity.Show_BA = "1";
                    else
                        MatEntity.Show_BA = "0";
                    //if(string.IsNullOrEmpty(MatEntity.Benchmark.ToString()))
                    //    MatEntity.Benchmark="0";
                    MatEntity.Lstc_Operator = Baseform.UserID;
                    MatEntity.Add_Operator = Baseform.UserID;

                    MatEntity.OverlScor = chkOverScor.Checked == true ? "Y" : "N";
                    MatEntity.SpecScor = chkSpecScor.Checked == true ? "Y" : "N";
                    MatEntity.Copy_Prassmnt = chkCopyPrior.Checked == true ? "Y" : "N";
                    MatEntity.Prog_Method = (((ListItem)cmbMethd.SelectedItem).Value.ToString());
                    if (_model.MatrixScalesData.InsertUpdateMatDef(MatEntity, "Update", out NewCode))
                    {
                        //this.Close();
                        if (MatEntity.Rec_Type == "I")
                        {
                            if (((ListItem)cmbAssDates.SelectedItem).Value.ToString() == "M")
                            {
                                foreach (MATDEFDTEntity Entity in sel_list)
                                {
                                    string strmsg = string.Empty;
                                    Entity.Rec_Type = "I";
                                    Entity.MatCode = txtCode.Text;
                                    _model.MatrixScalesData.UpdateMATDEFDT(Entity, "Update", out strmsg);
                                }
                                //MatDateEntity.MatDate=
                            }

                            AlertBox.Show("Matrix Inserted Successfully");
                            MatrixCode = txtCode.Text;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            AlertBox.Show("Matrix Updated Successfully");
                            if (((ListItem)cmbAssDates.SelectedItem).Value.ToString() == "M")
                            {
                                foreach (MATDEFDTEntity Entity in sel_list)
                                {
                                    List<MATDEFDTEntity> Def_Entitylist = new List<MATDEFDTEntity>();
                                    MATDEFDTEntity search_Entity = new MATDEFDTEntity(true);
                                    search_Entity.MatCode = txtCode.Text; search_Entity.MatDate = Entity.MatDate;
                                    Def_Entitylist = _model.MatrixScalesData.Browse_MATDEFDT(search_Entity, "Browse");
                                    if (Def_Entitylist.Count > 0)
                                    {
                                        Entity.Rec_Type = "U";
                                    }
                                    else
                                        Entity.Rec_Type = "I";
                                    string strmsg = string.Empty;
                                    Entity.MatCode = txtCode.Text;
                                    _model.MatrixScalesData.UpdateMATDEFDT(Entity, "Update", out strmsg);
                                }
                                //MatDateEntity.MatDate=
                            }

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        if (MatEntity.Rec_Type == "I")
                        {
                            //MessageBox.Show("Unsuccessful Matrix Insert...", "CAP Systems");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            //MessageBox.Show("Unsuccessful Matrix Update...", "CAP Systems");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    //MAT00001Control MatrixControl = Baseform.GetBaseUserControl() as MAT00001Control;
                    //if (MatrixControl != null)
                    //{
                    //    MatrixControl.RefreshMatrixGrid();
                    //}

                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool ValidateForm()
        {
            bool isValid = true;
            if (strType == "Matrix")
            {
                if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                {
                    _errorProvider.SetError(txtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtDesc, null);
                }
                if (((ListItem)cmbAssDates.SelectedItem).Value.ToString() == "A")
                {
                    if (string.IsNullOrEmpty(txtInterval.Text) || string.IsNullOrWhiteSpace(txtInterval.Text))
                    {
                        _errorProvider.SetError(txtInterval, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblInterval.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtInterval, null);
                    }
                }
                if (!string.IsNullOrEmpty(txtInterval.Text.Trim('-')))
                {
                    if (int.Parse(txtInterval.Text.Trim()) < 1)
                    {
                        _errorProvider.SetError(txtInterval, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtInterval, null);
                }


            }
            else
            {
                if (string.IsNullOrEmpty(txtScale_Desc.Text) || string.IsNullOrWhiteSpace(txtScale_Desc.Text))
                {
                    _errorProvider.SetError(txtScale_Desc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblS_Desc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtScale_Desc, null);
                }
                if (string.IsNullOrWhiteSpace(txtScale_Seq.Text) || string.IsNullOrEmpty(txtScale_Seq.Text))
                {
                    _errorProvider.SetError(txtScale_Seq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblS_Seq.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtScale_Seq, null);
                }
                if (string.IsNullOrEmpty(txtScale_Rationle.Text) || string.IsNullOrWhiteSpace(txtScale_Rationle.Text))
                {
                    _errorProvider.SetError(txtScale_Rationle, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblS_Rationle.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtScale_Rationle, null);
                }
                if (HierarchyGrid.Rows.Count < 1)
                {
                    _errorProvider.SetError(HierarchyGrid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Atleast One Hierarchy ".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(HierarchyGrid, null);
                }
                if (!string.IsNullOrEmpty(txtScale_Seq.Text))
                {
                    if (int.Parse(txtScale_Seq.Text.Trim()) < 1)
                    {
                        _errorProvider.SetError(txtScale_Seq, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtScale_Seq, null);
                }
            }
            return (isValid);
        }

        private bool isCodeExists(string Code)
        {
            bool isExists = false;
            if (Mode == "Add")
            {
                MATDEFEntity Search_Entity = new MATDEFEntity(true);
                Search_Entity.Mat_Code = ((ListItem)cmbMatrix.SelectedItem).Value.ToString();
                Search_Entity.Scale_Code = txtScale_Code.Text;
                MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
                if (MATDEF_List.Count > 0)
                {
                    isExists = true;
                }
            }

            return isExists;
        }

        private void fillMatrix_Controls()
        {

            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            //Search_Entity.Mat_Code = MatrixCode;
            //Search_Entity.Scale_Code = txtScale_Code.Text;
            MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            foreach (MATDEFEntity MatdefEntity in MATDEF_List)
            {
                if (MatdefEntity.Mat_Code.ToString().Trim() == MatrixCode.ToString().Trim() && MatdefEntity.Scale_Code == "0")
                {
                    txtCode.Text = MatdefEntity.Mat_Code;
                    txtDesc.Text = MatdefEntity.Desc;

                    if (cmbSSheets != null && cmbSSheets.Items.Count > 0)
                    {
                        foreach (ListItem listss in cmbSSheets.Items)
                        {
                            if (listss.Value.Equals(MatdefEntity.Score.ToString()) || listss.Text.Equals(MatdefEntity.Score.ToString()))
                            {
                                cmbSSheets.SelectedItem = listss;
                                break;
                            }
                        }
                    }
                    if (MatdefEntity.Date_option == "A")
                        SetComboBoxValue(cmbAssDates, "A");
                    else
                        SetComboBoxValue(cmbAssDates, "M");

                    if (MatdefEntity.Show_BA == "0")
                        Cb_Bench_ShowAssment.Checked = false;
                    else
                        Cb_Bench_ShowAssment.Checked = true;
                    //if (cmbAssDates != null && cmbAssDates.Items.Count > 0)
                    //{
                    //    foreach (ListItem list in cmbSSheets.Items)
                    //    {
                    //        if (list.Value.Equals(MatdefEntity.Date_option.ToString()) || list.Text.Equals(MatdefEntity.Date_option.ToString()))
                    //        {
                    //            cmbAssDates.SelectedItem = list;
                    //            break;
                    //        }
                    //    }
                    //}
                    if (((ListItem)cmbAssDates.SelectedItem).Value.ToString() == "A")
                        txtInterval.Text = MatdefEntity.Interval;
                    if (MatdefEntity.Active == "A")
                        chkbActive.Checked = true;
                    else
                        chkbActive.Checked = false;

                    chkOverScor.Checked = MatdefEntity.OverlScor == "Y" ? true : false;
                    chkSpecScor.Checked = MatdefEntity.SpecScor == "Y" ? true : false;
                    chkCopyPrior.Checked = MatdefEntity.Copy_Prassmnt == "Y" ? true : false;
                    SetComboBoxValue(cmbMethd, MatdefEntity.Prog_Method);
                }
            }

        }
        MATDEFEntity Search_Entity = new MATDEFEntity(true);
        private void FillScale_Controls()
        {
            // CommonFunctions.SetComboBoxValue(cmbMatrix, ScaleCode);
            MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            MATDEFEntity MatdefEntity = MATDEF_List.Find(u => u.Scale_Code.Equals(ScaleCode.ToString()) && u.Mat_Code.Equals(MatrixCode.ToString()));

            //   CommonFunctions.SetComboBoxValue(cmbMatrix, MatdefEntity.Scale_Code);
            txtScale_Code.Text = MatdefEntity.Scale_Code;
            txtScale_Seq.Text = MatdefEntity.Sequence;
            txtScale_Desc.Text = MatdefEntity.Desc;
            txtScale_Rationle.Text = MatdefEntity.Rationale;
            if (MatdefEntity.Override == "1")
            {
                chkbAssessOvr.Checked = true;
                CommonFunctions.SetComboBoxValue(cmbMatBm, MatdefEntity.Benchmark);
            }
            CommonFunctions.SetComboBoxValue(cmbAssessmentType, "H");
            if (propAgencyControlDetails != null)
            {
                if (propAgencyControlDetails.MatAssesment.ToUpper() == "Y")
                {
                    CommonFunctions.SetComboBoxValue(cmbAssessmentType, MatdefEntity.Assessment_Type);
                }
            }
            if (MatdefEntity.Score == "Y")
                chkHideScale.Checked = true;
            else
                chkHideScale.Checked = false;
            groupselection(MatdefEntity.GroupCode.Split(','));
            Fill_mat_Hierarchies_Grid();



        }

        public void groupselection(string[] strApplication)
        {
            foreach (DataGridViewRow row in gvwGroups.Rows)
            {
                if (row.Cells["gvtCode"].Value != null)
                {
                    for (int i = 0; i <= strApplication.Length - 1; i++)
                    {
                        if (Convert.ToString(row.Cells["gvtCode"].Value) == strApplication[i].ToString())
                        {
                            row.Cells["gvchkSelect"].Value = true;
                        }
                    }

                }
            }
        }

        List<MATSHIEEntity> MatDef_hierachies;
        private void Fill_mat_Hierarchies_Grid()
        {
            MatDef_hierachies = new List<MATSHIEEntity>();
            MatDef_hierachies = _model.MatrixScalesData.Browse_MatsHie(MatrixCode, ScaleCode);
            if (MatDef_hierachies.Count > 0)
            {
                string Hiename = null;
                HierarchyGrid.Rows.Clear();

                foreach (MATSHIEEntity Entity in MatDef_hierachies)
                {
                    int rowIndex = 0;
                    HierarchyEntity hierchyEntity = null;

                    if (Entity.Agency + Entity.Dept + Entity.Prog == "******")
                    {
                        Hiename = "All Hierarchies";
                        hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                    }
                    else
                        Hiename = "Not Defined in CASEHIE";

                    DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Entity.Agency, Entity.Dept, Entity.Prog);
                    if (ds_Hie_Name.Tables.Count > 0)
                    {
                        if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                            //hierchyEntity = new HierarchyEntity(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Hiename);

                            if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                            if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";

                            hierchyEntity = new HierarchyEntity(ds_Hie_Name.Tables[0].Rows[0], "CASEHIE");
                        }
                    }

                    rowIndex = HierarchyGrid.Rows.Add(string.Empty, Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog + "  " + Hiename, Entity.Agency + Entity.Dept + Entity.Prog);
                    HierarchyGrid.Rows[rowIndex].Tag = hierchyEntity;
                }
            }
        }


        int NewCode = 0; MATDEFEntity MatEntity = new MATDEFEntity();
        private void btnScale_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {

                    CaptainModel model = new CaptainModel();

                    if (Mode == "Add")
                        MatEntity.Mat_Code = ((ListItem)cmbMatrix.SelectedItem).Value.ToString();
                    else
                        MatEntity.Mat_Code = MatrixCode.ToString();
                    MatEntity.Rec_Type = "U";
                    MatEntity.Scale_Code = txtScale_Code.Text;
                    if (txtScale_Code.Text == "New")
                    {
                        MatEntity.Rec_Type = "I";
                        MatEntity.Scale_Code = "1";
                    }

                    //if (Mode == "Add")
                    //{
                    //    MatEntity.Rec_Type = "I";
                    //    MatEntity.Scale_Code = "0";
                    //}
                    if (chkHideScale.Checked == true)
                        MatEntity.Score = "Y";
                    MatEntity.Rec_mode = "S";
                    MatEntity.Desc = txtScale_Desc.Text;
                    MatEntity.Sequence = txtScale_Seq.Text;
                    MatEntity.Rationale = txtScale_Rationle.Text;
                    MatEntity.Benchmark = MatEntity.Override = "0";
                    if (propAgencyControlDetails != null)
                    {
                        if (propAgencyControlDetails.MatAssesment.ToUpper() == "Y")
                        {
                            MatEntity.Assessment_Type = ((ListItem)cmbAssessmentType.SelectedItem).Value.ToString();
                        }
                    }
                    if (chkbAssessOvr.Checked)
                    {
                        MatEntity.Override = "1";
                        if (cmbMatBm.Items.Count > 0)
                        {
                            if (((ListItem)cmbMatBm.SelectedItem).Value.ToString() != "0")
                                MatEntity.Benchmark = ((ListItem)cmbMatBm.SelectedItem).Value.ToString();
                        }
                    }
                    string strGroupCode = string.Empty;
                    foreach (DataGridViewRow row in gvwGroups.Rows)
                    {
                        if (row.Cells["gvchkSelect"].Value.ToString().ToUpper() == "TRUE")
                        {
                            strGroupCode = strGroupCode + row.Cells["gvtCode"].Value + ",";
                        }
                    }
                    //if (((ListItem)cmbGroupAssoc.SelectedItem).Value.ToString() != "0")
                    MatEntity.GroupCode = strGroupCode;
                    MatEntity.Lstc_Operator = Baseform.UserID;
                    MatEntity.Add_Operator = Baseform.UserID;

                    if (_model.MatrixScalesData.InsertUpdateMatDef(MatEntity, "Update", out NewCode))
                    {
                        if (Hie_Changed_FLG)
                            Update_Mats_Hierarchies();

                        //this.Close();
                        if (MatEntity.Rec_Type == "I")
                        {
                            AlertBox.Show("Scales Inserted Successfully");
                            ScaleCode = NewCode.ToString();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            AlertBox.Show("Scales Updated Successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        if (MatEntity.Rec_Type == "I")
                        {
                           // MessageBox.Show("Unsuccessful Scale Defintion Insert...", "CAP Systems");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                           // MessageBox.Show("Unsuccessful Scale Defintion Update...", "CAP Systems");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    //MAT00001Control MatrixControl = Baseform.GetBaseUserControl() as MAT00001Control;
                    //if (MatrixControl != null)
                    //{
                    //    MatrixControl.RefreshScaleGrid();
                    //}

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Update_Mats_Hierarchies()
        {
            MATSHIEEntity Entity = new MATSHIEEntity();
            string Hierarchy = null;
            bool Hierarchy_Saved = true, Old_Hierarchy = false, Delete_Hierarchy = true;

            if (Mode.Equals("Edit"))
            {
                foreach (MATSHIEEntity CompareENT in MatDef_hierachies)
                {
                    Delete_Hierarchy = true;
                    foreach (DataGridViewRow dr in HierarchyGrid.Rows)
                    {
                        if (dr.Cells["Hie_Code"].Value.ToString() == CompareENT.Agency + CompareENT.Dept + CompareENT.Prog)
                        { Delete_Hierarchy = false; break; }
                    }

                    if (Delete_Hierarchy)
                    {
                        Entity.Rec_Type = "D";
                        Entity.MatCode = MatrixCode;
                        Entity.SclCode = ScaleCode;
                        Entity.Agency = CompareENT.Agency;
                        Entity.Dept = CompareENT.Dept;
                        Entity.Prog = CompareENT.Prog;
                        //Entity.lstcOperator = BaseForm.UserID;

                        if (!_model.MatrixScalesData.UpdateMatsHie(Entity))
                            Hierarchy_Saved = false;
                    }
                }
            }


            foreach (DataGridViewRow dr in HierarchyGrid.Rows)
            {
                Hierarchy = null;
                Hierarchy = dr.Cells["Hie_Code"].Value.ToString();

                Old_Hierarchy = false;

                if (Mode.Equals("Edit"))
                {
                    foreach (MATSHIEEntity CompareENT in MatDef_hierachies)
                    {
                        if (Hierarchy == CompareENT.Agency + CompareENT.Dept + CompareENT.Prog)
                        { Old_Hierarchy = true; break; }
                    }
                }

                if (!Old_Hierarchy)
                {
                    Entity.Rec_Type = "I";
                    Entity.MatCode = MatrixCode;
                    if (Mode == "Edit")
                        Entity.SclCode = ScaleCode;
                    else
                        Entity.SclCode = Convert.ToString(NewCode);
                    Entity.Agency = Hierarchy.Substring(0, 2);
                    Entity.Dept = Hierarchy.Substring(2, 2);
                    Entity.Prog = Hierarchy.Substring(4, 2);
                    //Entity.lstcOperator = BaseForm.UserID;

                    if (!_model.MatrixScalesData.UpdateMatsHie(Entity))
                        Hierarchy_Saved = false;
                }
            }

            Hie_Changed_FLG = false;
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void chkbAssessOvr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAssessOvr.Checked)
            {
                cmbMatBm.Visible = true;

            }
            else
            {
                cmbMatBm.Visible = false;

            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDesc_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDesc.Text))
                _errorProvider.SetError(txtDesc, null);
        }

        private void txtInterval_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInterval.Text))
                _errorProvider.SetError(txtInterval, null);
        }

        private void btnScale_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbMatrix_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbMatrix.Items.Count > 0)
            //{
            //    FillGroupCombo(((ListItem)cmbMatrix.SelectedItem).Value.ToString());
            //}
            //if (Mode == "Edit")
            //{
            //    //ScaleCode = ((ListItem)cmbMatrix.SelectedItem).Value.ToString();
            //    FillScale_Controls();
            //}
            //else
            //    Get_BenchMarks_List();
        }

        private void txtScale_Seq_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtScale_Seq.Text))
            {
                if (int.Parse(txtScale_Seq.Text.Trim()) < 0)
                    _errorProvider.SetError(txtScale_Seq, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                else
                    _errorProvider.SetError(txtScale_Seq, null);
            }
        }

        private void txtInterval_LostFocus_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInterval.Text))
            {
                if (int.Parse(txtInterval.Text.Trim()) < 1)
                    _errorProvider.SetError(txtInterval, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                else
                    _errorProvider.SetError(txtInterval, null);
            }
        }

        private void Txt_Bench_High_LostFocus(object sender, EventArgs e)
        {

        }

        private void Txt_Bench_Low_LostFocus(object sender, EventArgs e)
        {

        }

        private void Btn_Cancel_Bench_Click(object sender, EventArgs e)
        {

        }



        private void Pb_Bench_MainButtons_Click(object sender, EventArgs e)
        {

        }

        private void Txt_Bench_Desc_LostFocus(object sender, EventArgs e)
        {

        }

        private bool CheckBenchmarkRange()
        {
            bool boolCheckBench = true;

            List<MATDEFBMEntity> MatDEfBenchMark = Benchmarks_List.FindAll(u => u.MatCode.Equals(MatrixCode));
            foreach (var item in MatDEfBenchMark)
            {
                if ((Mode == "Edit" && item.Code != Txt_Bench_Code.Text.Trim()) || Mode == "Add")
                {
                    if ((Convert.ToInt32(item.Low) <= Convert.ToInt32(Txt_Bench_Low.Text)) && (Convert.ToInt32(Txt_Bench_Low.Text) <= Convert.ToInt32(item.High)))
                    {
                        boolCheckBench = false;
                        break;
                    }
                    else if ((Convert.ToInt32(item.Low) <= Convert.ToInt32(Txt_Bench_High.Text)) && (Convert.ToInt32(Txt_Bench_High.Text) <= Convert.ToInt32(item.High)))
                    {
                        boolCheckBench = false;
                        break;
                    }

                }
            }
            return boolCheckBench;


        }

        string Added_Edited_BenchMark = string.Empty;
        private void Btn_Save_Bench_Click(object sender, EventArgs e)
        {
            if (BenchValidateForm())
            {

                if (CheckBenchmarkRange())
                {
                    MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);

                    Search_Entity.Rec_Type = "U";
                    Search_Entity.Code = Txt_Bench_Code.Text;
                    if (Txt_Bench_Code.Text == "New")
                    {
                        Search_Entity.Rec_Type = "I";
                        Search_Entity.Code = "1";
                    }

                    Search_Entity.MatCode = MatrixCode;

                    if (!string.IsNullOrEmpty(Txt_Bench_Desc.Text.Trim()))
                        Search_Entity.Desc = Txt_Bench_Desc.Text;

                    if (!string.IsNullOrEmpty(Txt_Bench_Low.Text.Trim()))
                        Search_Entity.Low = Txt_Bench_Low.Text;

                    if (!string.IsNullOrEmpty(Txt_Bench_High.Text.Trim()))
                        Search_Entity.High = Txt_Bench_High.Text;

                    if (!string.IsNullOrEmpty(txt_Over_Low.Text.Trim()))
                        Search_Entity.Overall_Low = txt_Over_Low.Text;

                    if (!string.IsNullOrEmpty(txt_Over_High.Text.Trim()))
                        Search_Entity.Overall_High = txt_Over_High.Text;

                    if ((((ListItem)cmbScoreType.SelectedItem).Value.ToString()) != "0")
                        Search_Entity.ScoreType = (((ListItem)cmbScoreType.SelectedItem).Value.ToString());
                    Search_Entity.Progress = chkProgress.Checked == true ? "Y" : "N";
                    Search_Entity.Continuity = chkContinuity.Checked == true ? "Y" : "N";
                    Search_Entity.Regressed = chkbRegress.Checked == true ? "Y" : "N";

                    string strmsg = string.Empty;

                    if (_model.MatrixScalesData.UpdateMATDEFBM(Search_Entity, "Update", out strmsg))
                    {
                        if (Search_Entity.Rec_Type == "I" /*&& strmsg == "Successful"*/)
                        {

                            BenchmarkCode = strmsg;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            AlertBox.Show("Benchmarks Inserted Successfully");
                        }
                        else
                        {
                            AlertBox.Show("Benchmarks Updated Successfully");
                            BenchmarkCode = Txt_Bench_Code.Text;
                            Added_Edited_BenchMark = Txt_Bench_Code.Text;
                        }
                        this.DialogResult = DialogResult.OK;
                        this.Close();


                    }
                    else
                    {
                        if (Search_Entity.Rec_Type == "I")
                        {
                            // MessageBox.Show("Unsuccessful Benchmark Insert...", "CAP Systems");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }

                        else
                        {
                            // MessageBox.Show("Unsuccessful Benchmark Update...", "CAP Systems");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                       
                    // ClearControls();
                    // Fill_Sel_Benchmark_Combo();
                    //if (Privileges.AddPriv.Equals("false"))
                    //    Pb_Add_Bench.Visible = false;
                    //else
                    //    Pb_Add_Bench.Visible = true;
                    //if (Privileges.ChangePriv.Equals("false"))
                    //    Pb_Edit_Bench.Visible = false;
                    //else
                    //    Pb_Edit_Bench.Visible = true;
                    //if (Privileges.DelPriv.Equals("false"))
                    //    Pb_Delete_Bench.Visible = false;
                    //else
                    //    Pb_Delete_Bench.Visible = true;
                }
                else
                {
                    AlertBox.Show("Benchmark range entered overlaps other benchmark low and high range", MessageBoxIcon.Warning);
                }
            }
        }

        public bool BenchValidateForm()
        {
            bool isValid = true;
            //Txt_Bench_Low.Text = Txt_Bench_Low.Text.Trim('-');
            //Txt_Bench_High.Text = Txt_Bench_High.Text.Trim('-');
            if (string.IsNullOrEmpty(Txt_Bench_Desc.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Bench_Desc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBenchDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Txt_Bench_Desc, null);
            if (string.IsNullOrEmpty(Txt_Bench_Low.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Bench_Low, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLow.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else

                _errorProvider.SetError(Txt_Bench_Low, null);
            if (string.IsNullOrEmpty(Txt_Bench_High.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Bench_High, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHigh.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Txt_Bench_High, null);
            //if (!string.IsNullOrEmpty(Txt_Bench_High.Text.Trim('-')) && !string.IsNullOrEmpty(Txt_Bench_Low.Text.Trim('-')))
            if (!string.IsNullOrEmpty(Txt_Bench_High.Text.Trim()) && !string.IsNullOrEmpty(Txt_Bench_Low.Text.Trim()))
            {
                if (int.Parse(Txt_Bench_High.Text) < int.Parse(Txt_Bench_Low.Text))
                {
                    _errorProvider.SetError(Txt_Bench_High, "low values is greater than the high value".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Txt_Bench_High, null);
            }


            if (string.IsNullOrEmpty(txt_Over_Low.Text.Trim()))
            {
                _errorProvider.SetError(txt_Over_Low, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLow.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else

                _errorProvider.SetError(txt_Over_Low, null);
            if (string.IsNullOrEmpty(txt_Over_High.Text.Trim()))
            {
                _errorProvider.SetError(txt_Over_High, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHigh.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txt_Over_High, null);
            //if (!string.IsNullOrEmpty(Txt_Bench_High.Text.Trim('-')) && !string.IsNullOrEmpty(Txt_Bench_Low.Text.Trim('-')))
            if (!string.IsNullOrEmpty(txt_Over_High.Text.Trim()) && !string.IsNullOrEmpty(txt_Over_Low.Text.Trim()))
            {
                if (int.Parse(txt_Over_High.Text) < int.Parse(txt_Over_Low.Text))
                {
                    _errorProvider.SetError(txt_Over_High, "low values is greater than the high value".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txt_Over_High, null);
            }

            return isValid;
        }

        private void Btn_Cancel_Bench_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Get_BenchMarkFillData()
        {

            MATDEFBMEntity MATDEFBMSingleEntity = Benchmarks_List.Find(u => u.MatCode.Equals(MatrixCode) && u.Code.Equals(ScaleCode));

            Txt_Bench_Code.Text = MATDEFBMSingleEntity.Code.ToString();
            Txt_Bench_Desc.Text = MATDEFBMSingleEntity.Desc.ToString();
            Txt_Bench_Low.Text = MATDEFBMSingleEntity.Low.ToString();
            Txt_Bench_High.Text = MATDEFBMSingleEntity.High.ToString();
            txt_Over_Low.Text = MATDEFBMSingleEntity.Overall_Low.ToString();
            txt_Over_High.Text = MATDEFBMSingleEntity.Overall_High.ToString();
            chkProgress.Checked = MATDEFBMSingleEntity.Progress == "Y" ? true : false;
            chkContinuity.Checked = MATDEFBMSingleEntity.Continuity == "Y" ? true : false;
            chkbRegress.Checked = MATDEFBMSingleEntity.Regressed == "Y" ? true : false;
            CommonFunctions.SetComboBoxValue(cmbScoreType, MATDEFBMSingleEntity.ScoreType.ToString());
        }

        private void Matrix_Scales_DefinitionsForm_Load(object sender, EventArgs e)
        {
            if (strType == "Matrix")
            {
                chkbActive.Focus();
            }
            else if (strType == "Scale")
            {
                txtScale_Seq.Focus();
            }
            else if (strType == "BenchMarks")
            {
                Txt_Bench_Desc.Focus();
            }


        }


        private void btnMatAssDates_Click(object sender, EventArgs e)
        {
            //List<MATDEFDTEntity> sel_list = new List<MATDEFDTEntity>();
            MAT00003AssessmentDate Mat_ass_date = new MAT00003AssessmentDate(Baseform, Priviliges, txtCode.Text, sel_list, "Matrix", Mode);
            Mat_ass_date.FormClosed += new FormClosedEventHandler(On_MAtAssDt_Select_Closed);
            Mat_ass_date.StartPosition = FormStartPosition.CenterScreen;
            Mat_ass_date.ShowDialog();
        }

        private void On_MAtAssDt_Select_Closed(object sender, FormClosedEventArgs e)
        {
            string SelRef_Name = null;
            string Sql_MSg = string.Empty;
            MAT00003AssessmentDate form = sender as MAT00003AssessmentDate;
            if (form.DialogResult == DialogResult.OK)
            {
                //Sel_REFS_List = form.GetSelected_Referral_Entity();
                sel_list = form.GetSelected_MatDates(); ;
                //sel_list = form.GetAdd_Del_Selected_Services();
                //if (Sel_REFS_List.Count > 30)
                //{
                //    MessageBox.Show("You may not select more than 30 Seevices", "CAP Systems");
                //}

            }
        }

        private void btnScaleGrops_Click(object sender, EventArgs e)
        {
            MatScaleGroup matScalegroup = new MatScaleGroup(Baseform, Priviliges, MatrixCode);
            matScalegroup.StartPosition = FormStartPosition.CenterScreen;
            matScalegroup.ShowDialog();
        }

        private void Matrix_Scales_DefinitionsForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (strType == "Matrix")
            {
                Application.Navigate(CommonFunctions.BuildHelpURLS(Priviliges.Program, 1, Baseform.BusinessModuleID.ToString()), target: "_blank");
            }
            else if (strType == "BenchMarks")
            {
                Application.Navigate(CommonFunctions.BuildHelpURLS(Priviliges.Program, 2, Baseform.BusinessModuleID.ToString()), target: "_blank");
            }
            else if (strType == "Scale")
            {
                Application.Navigate(CommonFunctions.BuildHelpURLS(Priviliges.Program, 3, Baseform.BusinessModuleID.ToString()), target: "_blank");
            }
        }
    }
}