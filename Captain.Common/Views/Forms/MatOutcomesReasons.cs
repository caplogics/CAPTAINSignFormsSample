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
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Spire.Pdf.Grid;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MatOutcomesReasons : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion

        public MatOutcomesReasons(BaseForm baseForm, PrivilegeEntity privilieges, string strtype, string strMatCode, string strScaleCode, string strBenchcode, string strLow, string strHigh)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            this.Text = Privileges.Program;
            strType = strtype;
            Matcode = strMatCode;
            ScaleCode = strScaleCode;
            Benchmarkcode = strBenchcode;
            strLowValue = strLow;
            strHighValue = strHigh;
            Txt_Out_Points.Validator = TextBoxValidation.IntegerValidator;

            if (strType == "Outcomes")
            {
                this.Size = new System.Drawing.Size(583, 404);
                this.Text = "Matrix"/*Privileges.Program*/ +" - Outcomes";
               // LblHeader.Text = "Outcomes";
                this.cmbScales.SelectedIndexChanged -= new System.EventHandler(this.cmbScope_SelectedIndexChanged);
                this.Cmb_Benchmarks.SelectedIndexChanged -= new System.EventHandler(this.Cmb_Benchmarks_SelectedIndexChanged);
                Fill_Sel_Benchmark_Combo();
                FillComboScales();
                CommonFunctions.SetComboBoxValue(cmbScales, ScaleCode);
                CommonFunctions.SetComboBoxValue(Cmb_Benchmarks, strBenchcode);
                Cmb_Benchmarks.Enabled = true;
                pnlOutcomes.Visible = true;
                pnlReasons.Visible = false;
               // this.pnlOutcomes.Location = new System.Drawing.Point(1, 55);
                Get_Outcomes_List();
                Fill_Sel_BenchMark_Outcomes(string.Empty, ScaleCode, Benchmarkcode);
                OutComesControlsfill();
                this.cmbScales.SelectedIndexChanged += new System.EventHandler(this.cmbScope_SelectedIndexChanged);
                this.Cmb_Benchmarks.SelectedIndexChanged += new System.EventHandler(this.Cmb_Benchmarks_SelectedIndexChanged);

            }

            else if (strType == "Reasons")
            {
                this.Size = new Size(575, 368);
                this.Text = "Matrix"/*Privileges.Program*/ +" - Reasons";
              //  LblHeader.Text = "Reasons";
                pnlOutcomes.Visible = false;
                pnlReasons.Visible = true;
               // this.pnlReasons.Location = new System.Drawing.Point(1, 55);
                Fill_ReasonCombo();
                Get_Reasons_List();
                Fill_Sel_Matrix_Reasons(String.Empty);
            }

            if (baseForm.UserID != "JAKE")
            {
                picAddreason.Visible = false;
                picAddScale.Visible = false;
                Outcomes_Grid.Columns["EditOutComes"].Visible = false;
                Reasons_Grid.Columns["EditReason"].Visible = false;
                Outcomes_Grid.Columns[Outcomes_Grid.ColumnCount - 1].Visible = false;
                Reasons_Grid.Columns[Reasons_Grid.ColumnCount - 1].Visible = false;
            }

            if (Privileges.AddPriv.Equals("false"))
            {
                picAddreason.Visible = false;
                picAddScale.Visible = false;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                Outcomes_Grid.Columns["EditOutComes"].Visible = false;
                Reasons_Grid.Columns["EditReason"].Visible = false;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                Outcomes_Grid.Columns[Outcomes_Grid.ColumnCount - 1].Visible = false;
                Reasons_Grid.Columns[Reasons_Grid.ColumnCount - 1].Visible = false;
                // dataGridCaseIncome.Columns[dataGridCaseIncome.ColumnCount - 2].Width = 200;
            }


        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string strType { get; set; }

        public string Matcode { get; set; }
        public string ScaleCode { get; set; }
        public string Benchmarkcode { get; set; }
        public string strLowValue { get; set; }
        public string strHighValue { get; set; }


        #endregion



        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //if (strType == "Outcomes")
            //   Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "MAT00001_Outcomes");
            //else if (strType == "Reasons")
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "MAT00001_Reasons");
        }

        private void FillComboScales()
        {
            cmbScales.Items.Clear();
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            List<MATDEFEntity> MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            int Tmp_Count = 0;
            foreach (MATDEFEntity entity in MATDEF_List)
            {
                if (entity.Mat_Code == Matcode && entity.Scale_Code != "0")
                {
                    cmbScales.Items.Add(new ListItem(entity.Desc, entity.Scale_Code));
                    Tmp_Count++;
                }
            }
            if (Tmp_Count > 0)
            {
                if (ScaleCode != string.Empty)
                {
                    CommonFunctions.SetComboBoxValue(cmbScales,ScaleCode);
                }
                else
                {

                }
            }
        }

        string Added_Edited_OutComeCode = string.Empty;
        private void Pb_Save_Out_Click(object sender, EventArgs e)
        {
            if (OutCmesValidateForm())
            {
                if ((Convert.ToInt32(strLowValue) <= Convert.ToInt32(Txt_Out_Points.Text)) && (Convert.ToInt32(Txt_Out_Points.Text) <= (Convert.ToInt32(strHighValue))))
                {
                    Outcomes_Grid.Enabled = true;
                    picAddScale.Enabled = true;
                    MATOUTCEntity Search_Entity = new MATOUTCEntity(true);
                    if (Cmb_Benchmarks.Items.Count > 0)
                    {
                        Search_Entity.Rec_Type = "U";
                        Search_Entity.Code = Txt_Out_Code.Text;
                        if (Txt_Out_Code.Text == "New")
                        {
                            Search_Entity.Rec_Type = "I";
                            Search_Entity.Code = "1";
                        }

                        Search_Entity.MatCode = Matcode;
                        Search_Entity.SclCode = ((ListItem)cmbScales.SelectedItem).Value.ToString();
                        Search_Entity.BmCode = ((ListItem)Cmb_Benchmarks.SelectedItem).Value.ToString();//BenchMarks_Grid.CurrentRow.Cells["Bench_Code"].Value.ToString();
                        if (!string.IsNullOrEmpty(Txt_Out_Desc.Text.Trim()))
                            Search_Entity.Desc = Txt_Out_Desc.Text;

                        Search_Entity.Points = Txt_Out_Points.Text;

                        Search_Entity.AddOperator = BaseForm.UserID;
                        Search_Entity.LstcOperator = BaseForm.UserID;
                        string strmsg = string.Empty;
                        if (_model.MatrixScalesData.UpdateMATOUTC(Search_Entity, "Update", out strmsg))
                        {
                            if (Search_Entity.Rec_Type == "I")
                            {
                                Added_Edited_OutComeCode = strmsg;
                                AlertBox.Show("Outcomes Inserted Successfully");
                                // MessageBox.Show("Outcomes Inserted Successfully...", "CAP Systems");
                            }
                            else
                            {
                                Added_Edited_OutComeCode = Txt_Out_Code.Text;
                                AlertBox.Show("OutComes Updated Successfully");
                                // MessageBox.Show("OutComes Updated Successfully...", "CAP Systems");
                            }
                            Get_Outcomes_List();
                            Fill_Sel_BenchMark_Outcomes(Added_Edited_OutComeCode, ((ListItem)cmbScales.SelectedItem).Value.ToString(), ((ListItem)Cmb_Benchmarks.SelectedItem).Value.ToString());
                        }
                        else
                        {
                            //if (Search_Entity.Rec_Type == "I")
                            //    MessageBox.Show("Unsuccessful Outcomes Insert...", "CAP Systems");
                            //else
                            //    MessageBox.Show("Unsuccessful Outcomes Update...", "CAP Systems");
                        }
                    }
                    else
                        MessageBox.Show("The Outcomes Having No Benchmarks...", "CAP Systems");
                    ClearControls();
                    //if (Privileges.AddPriv.Equals("false"))
                    //   // Pb_Add_Out.Visible = false;
                    //else
                    //   // Pb_Add_Out.Visible = true;
                    //if (Privileges.ChangePriv.Equals("false"))
                    //   // Pb_Edit_Out.Visible = false;
                    //else
                    //  //  Pb_Edit_Out.Visible = true;
                    //if (Privileges.DelPriv.Equals("false"))
                    // //   Pb_Delete_Out.Visible = false;
                    //else
                    // //   Pb_Delete_Out.Visible = true;
                    ////Pb_Add_Out.Visible = true; Pb_Edit_Out.Visible = true; Pb_Delete_Out.Visible = true;
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("Outcome points must be within benchmark range. Low: " + strLowValue + " High: " + strHighValue);
                }
            }
        }

        public bool OutCmesValidateForm()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(Txt_Out_Desc.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Out_Desc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOutcmeDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Txt_Out_Desc, null);
            if (string.IsNullOrEmpty(Txt_Out_Points.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Out_Points, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOutcmePoints.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Txt_Out_Points, null);

            return isValid;
        }

        public void ClearControls()
        {

            // Txt_Bench_Code.Enabled = Txt_Bench_Desc.Enabled = Txt_Bench_High.Enabled = Txt_Bench_Low.Enabled = false;
            //Txt_Bench_Code.Clear(); Txt_Bench_Desc.Clear(); Txt_Bench_High.Clear(); Txt_Bench_Low.Clear();
            //Txt_Out_Code.Clear(); Txt_Out_Desc.Clear(); Txt_Out_Points.Clear();
            Txt_Out_Code.Enabled = Txt_Out_Desc.Enabled = Txt_Out_Points.Enabled = false;
            //txtReason.Clear(); txtReasonDesc.Clear();
            txtReason.Enabled = txtReasonDesc.Enabled = false;
            //  BenchMarks_Grid.Enabled = true; Outcomes_Grid.Enabled = true; Reasons_Grid.Enabled = true;
            //  Btn_Cancel_Bench.Visible = false; Btn_Save_Bench.Visible = false;
            Pb_Save_Reason.Visible = false; Pb_Cancel_Reason.Visible = false;
            Pb_Save_Out.Visible = false; Pb_Cancel_Out.Visible = false;
            
          
        }


        private void Pb_Cancel_Out_Click(object sender, EventArgs e)
        {
            Txt_Out_Code.Enabled = Txt_Out_Desc.Enabled = Txt_Out_Points.Enabled = false;
            picAddScale.Enabled = true;
            Pb_Cancel_Out.Visible = Pb_Save_Out.Visible = false;
            _errorProvider.SetError(Txt_Out_Desc, null);
            _errorProvider.SetError(Txt_Out_Points, null);
            Outcomes_Grid.Enabled = true;

            if (Outcomes_Grid.Rows.Count > 0)
            {
                Txt_Out_Code.Text = Outcomes_Grid.CurrentRow.Cells["Out_Code"].Value.ToString();
                Txt_Out_Desc.Text = Outcomes_Grid.CurrentRow.Cells["Out_Desc"].Value.ToString();
                Txt_Out_Points.Text = Outcomes_Grid.CurrentRow.Cells["Out_Points"].Value.ToString();
            }

        }

        string Added_Edited_ReasonCode = string.Empty;
        private void Pb_Save_Reason_Click(object sender, EventArgs e)
        {
            if (ReasonValidateForm())
            {
                Reasons_Grid.Enabled = true;
                picAddreason.Enabled = true;
                MATREASNEntity Search_Entity = new MATREASNEntity(true);

                Search_Entity.Rec_Type = "U";
                Search_Entity.Code = txtReason.Text;
                if (txtReason.Text == "New")
                {
                    Search_Entity.Rec_Type = "I";
                    Search_Entity.Code = "1";
                }

                Search_Entity.MatCode = Matcode;
                Search_Entity.Scl_Code = ScaleCode;
                if (!string.IsNullOrEmpty(txtReasonDesc.Text.Trim()))
                    Search_Entity.Desc = txtReasonDesc.Text;

                Search_Entity.PN = ((ListItem)cmbReason.SelectedItem).Value.ToString();
                Search_Entity.Add_Operator = BaseForm.UserID;
                Search_Entity.Lstc_Operator = BaseForm.UserID;
                string strmsg = string.Empty;
                if (_model.MatrixScalesData.UpdateMATREASN(Search_Entity, "Update", out strmsg))
                {
                    if (Search_Entity.Rec_Type == "I")
                    {
                        Added_Edited_ReasonCode = strmsg;
                        AlertBox.Show("Reasons Inserted Successfully");
                        // MessageBox.Show("Reason Inserted Successfully...", "CAP Systems");
                    }
                    else
                    {
                        Added_Edited_ReasonCode = txtReason.Text;
                        AlertBox.Show("Reasons Updated Successfully");
                        //  MessageBox.Show("Reason Updated Successfully...", "CAP Systems");
                    }
                    Get_Reasons_List();
                    Fill_Sel_Matrix_Reasons(Added_Edited_ReasonCode);
                }
                else
                {
                    //if (Search_Entity.Rec_Type == "I")
                    //    MessageBox.Show("Unsuccessful Reason Insert...", "CAP Systems");
                    //else
                    //    MessageBox.Show("Unsuccessful Reason Update...", "CAP Systems");
                }
                ClearControls();
                //Fill_ReasonCombo();
                //if (Privileges.AddPriv.Equals("false"))
                //    Pb_Add_Reason.Visible = false;
                //else
                //    Pb_Add_Reason.Visible = true;
                //if (Privileges.ChangePriv.Equals("false"))
                //    Pb_Edit_Reason.Visible = false;
                //else
                //    Pb_Edit_Reason.Visible = true;
                //if (Privileges.DelPriv.Equals("false"))
                //    Pb_Delete_Reason.Visible = false;
                //else
                //    Pb_Delete_Bench.Visible = true;
                //Pb_Delete_Reason.Visible = true; Pb_Edit_Reason.Visible = true; Pb_Delete_Reason.Visible = true;
            }
        }

        public bool ReasonValidateForm()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtReasonDesc.Text.Trim()))
            {
                _errorProvider.SetError(txtReasonDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResnDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtReasonDesc, null);
            return isValid;
        }

        private void Pb_Cancel_Reason_Click(object sender, EventArgs e)
        {
            picAddreason.Enabled = true;
            Reasons_Grid.Enabled = true;
            txtReason.Enabled = false; txtReasonDesc.Enabled = false; cmbReason.Enabled = false;
            Pb_Save_Reason.Visible = Pb_Cancel_Reason.Visible = false;
            _errorProvider.SetError(txtReasonDesc, null);
            if (Reasons_Grid.Rows.Count > 0)
            {
                txtReason.Text = Reasons_Grid.CurrentRow.Cells["Reason_Code"].Value.ToString();
                txtReasonDesc.Text = Reasons_Grid.CurrentRow.Cells["Reason_Desc"].Value.ToString();
                CommonFunctions.SetComboBoxValue(cmbReason, Reasons_Grid.CurrentRow.Cells["Reason_PN"].Value.ToString());
                //if(string.IsNullOrEmpty(txtReasonDesc.Text))
                _errorProvider.SetError(txtReasonDesc, null);
            }


        }

        private void Fill_Sel_BenchMark_Outcomes(string Sel_OutcomesCode, string strScaleCode, string strBenchMarkCode)
        {
            strHighValue = ((ListItem)Cmb_Benchmarks.SelectedItem).ValueDisplayCode.ToString();
            strLowValue = ((ListItem)Cmb_Benchmarks.SelectedItem).ID.ToString();
            this.Outcomes_Grid.SelectionChanged -= new System.EventHandler(this.Outcomes_Grid_SelectionChanged);
            Get_Outcomes_List();
            Outcomes_Grid.Rows.Clear();
            if (OutComes_List.Count > 0)
            {
                List<MATOUTCEntity> matoutcfilterList = OutComes_List.FindAll(u => u.MatCode.Equals(Matcode) && u.SclCode.Equals(strScaleCode) && u.BmCode.Equals(strBenchMarkCode));
                Txt_Out_Desc.Visible = true; Txt_Out_Points.Visible = true;
                int TmpCount = 0, RowIndex = 0, Sel_Outcome_Index = 0;

                foreach (MATOUTCEntity Entity in matoutcfilterList)
                {
                    RowIndex = Outcomes_Grid.Rows.Add(Entity.Code, Entity.Desc, Entity.Points);
                    CommonFunctions.setTooltip(RowIndex, Entity.AddOperator, Entity.AddDate, Entity.LstcOperator, Entity.DateLstc, Outcomes_Grid);
                    if (Sel_OutcomesCode == Entity.Code)
                        Sel_Outcome_Index = TmpCount;
                    TmpCount++;
                }

                if (TmpCount > 0)
                {
                    if (string.IsNullOrEmpty(Sel_OutcomesCode.ToString()))
                        Outcomes_Grid.Rows[0].Tag = 0;
                    else
                    {
                        Outcomes_Grid.CurrentCell = Outcomes_Grid.Rows[Sel_Outcome_Index].Cells[1];

                        int scrollPosition = 0;
                        scrollPosition = Outcomes_Grid.CurrentCell.RowIndex;
                        //int CurrentPage = (scrollPosition / Outcomes_Grid.ItemsPerPage);
                        //CurrentPage++;
                        //Outcomes_Grid.CurrentPage = CurrentPage;
                        //Outcomes_Grid.FirstDisplayedScrollingRowIndex = scrollPosition;
                    }
                    if (Outcomes_Grid.Rows.Count > 0)
                        Outcomes_Grid.Rows[Sel_Outcome_Index].Selected = true;

                    //Outcomes_Grid.Rows[0].Tag = 0;
                    //Outcomes_Grid.CurrentCell = Outcomes_Grid.Rows[Sel_Outcome_Index].Cells[1];

                    Txt_Out_Code.Text = Outcomes_Grid.CurrentRow.Cells["Out_Code"].Value.ToString();
                    Txt_Out_Desc.Text = Outcomes_Grid.CurrentRow.Cells["Out_Desc"].Value.ToString();
                    Txt_Out_Points.Text = Outcomes_Grid.CurrentRow.Cells["Out_Points"].Value.ToString();
                }
                else
                {

                    Txt_Out_Code.Clear(); Txt_Out_Desc.Clear(); Txt_Out_Points.Clear();
                }
            }
            
            this.Outcomes_Grid.SelectionChanged += new System.EventHandler(this.Outcomes_Grid_SelectionChanged);
        }

        List<MATOUTCEntity> OutComes_List = new List<MATOUTCEntity>();
        private void Get_Outcomes_List()
        {
            MATOUTCEntity Search_Entity = new MATOUTCEntity(true);
            OutComes_List = _model.MatrixScalesData.Browse_MATOUTC(Search_Entity, "Browse");
        }

        private void Fill_ReasonCombo()
        {
            cmbReason.Items.Clear();
            List<ListItem> listReason = new List<ListItem>();
            listReason.Add(new ListItem("Positive", "P"));
            listReason.Add(new ListItem("Negative", "N"));
            cmbReason.Items.AddRange(listReason.ToArray());
            cmbReason.SelectedIndex = 0;
        }

        private void Fill_Sel_Matrix_Reasons(string Sel_ReasonCode)
        {
            Reasons_Grid.Rows.Clear();
            Get_Reasons_List();
            if (Reasons_List.Count > 0)//&& Scales_Grid.Rows.Count > 0)
            {
                txtReasonDesc.Visible = true;

                List<MATREASNEntity> matreasnfilterList = Reasons_List.FindAll(u => u.MatCode.Equals(Matcode) && u.Scl_Code.Equals(ScaleCode));

                int Tmp_Count = 0, RowIndex = 0, Sel_Reason_Index = 0;
                foreach (MATREASNEntity Entity in matreasnfilterList)
                {

                    string PN = null;
                    if (Entity.PN == "P")
                        PN = "Positive";
                    else
                        PN = "Negative";
                    RowIndex = Reasons_Grid.Rows.Add(Entity.Code, PN, Entity.Desc);
                    CommonFunctions.setTooltip(RowIndex, Entity.Add_Operator, Entity.Add_Date, Entity.Lstc_Operator, Entity.Lstc_Date, Reasons_Grid);
                    set_Reasons_Tooltip(RowIndex, Entity);
                    if (Sel_ReasonCode == Entity.Code)
                        Sel_Reason_Index = Tmp_Count;
                    Tmp_Count++;

                }
                if (Tmp_Count > 0)
                {
                    if (string.IsNullOrEmpty(Sel_ReasonCode))
                        Reasons_Grid.Rows[0].Tag = 0;
                    else
                    {
                        Reasons_Grid.CurrentCell = Reasons_Grid.Rows[Sel_Reason_Index].Cells[1];

                        int scrollPosition = 0;
                        scrollPosition = Reasons_Grid.CurrentCell.RowIndex;
                        //int CurrentPage = (scrollPosition / Reasons_Grid.ItemsPerPage);
                        //CurrentPage++;
                        //Reasons_Grid.CurrentPage = CurrentPage;
                        //Reasons_Grid.FirstDisplayedScrollingRowIndex = scrollPosition;
                    }
                    if (Reasons_Grid.Rows.Count > 0)
                    { 
                        Reasons_Grid.Rows[Sel_Reason_Index].Selected = true;
                        Reasons_Grid.CurrentCell = Reasons_Grid.Rows[Sel_Reason_Index].Cells[1];
                    }

                    //Reasons_Grid.Rows[0].Tag = 0;
                    //Reasons_Grid.CurrentCell = Reasons_Grid.Rows[Sel_Reason_Index].Cells[1];
                    txtReason.Text = Reasons_Grid.CurrentRow.Cells["Reason_Code"].Value.ToString();
                    txtReasonDesc.Text = Reasons_Grid.CurrentRow.Cells["Reason_Desc"].Value.ToString();
                    CommonFunctions.SetComboBoxValue(cmbReason, Reasons_Grid.CurrentRow.Cells["Reason_PN"].Value.ToString());
                }
                else
                {
                    // Pb_Delete_Reason.Visible = false; Pb_Edit_Reason.Visible = false; Reasons_Grid.Rows.Clear();
                    txtReason.Clear(); txtReasonDesc.Clear(); cmbReason.SelectedIndex = 0;
                }

            }

        }

        private void set_Reasons_Tooltip(int rowIndex, MATREASNEntity Entity)
        {
            string toolTipText = "Added By     : " + Entity.Add_Operator.Trim() + " on " + Entity.Add_Date.ToString() + "\n" +
                                 "Modified By  : " + Entity.Lstc_Operator.Trim() + " on " + Entity.Lstc_Date.ToString();

            foreach (DataGridViewCell cell in Reasons_Grid.Rows[rowIndex].Cells)
                cell.ToolTipText = toolTipText;
        }

        List<MATREASNEntity> Reasons_List = new List<MATREASNEntity>();
        private void Get_Reasons_List()
        {
            MATREASNEntity Search_Entity = new MATREASNEntity(true);
            Reasons_List = _model.MatrixScalesData.Browse_MATREASN(Search_Entity, "Browse");
        }


        private void Fill_Sel_Benchmark_Combo()
        {
            Get_BenchMarks_List();
            Cmb_Benchmarks.Items.Clear();

            if (Benchmarks_List.Count > 0)
            {
                int Tmp_Count = 0, RowIndex = 0, Sel_Benchmark_Index = 0;
                foreach (MATDEFBMEntity Entity in Benchmarks_List)
                {
                    if (Entity.MatCode == Matcode)   //9999999
                    {
                        Cmb_Benchmarks.Items.Add(new ListItem(Entity.Desc, Entity.Code,Entity.Low,Entity.High));
                        Tmp_Count++;
                    }
                }

                if (Tmp_Count > 0)
                {
                    Cmb_Benchmarks.SelectedIndex = 0;
                }
                else
                {
                    Cmb_Benchmarks.Items.Clear();
                }
            }
        }

        List<MATDEFBMEntity> Benchmarks_List = new List<MATDEFBMEntity>();
        private void Get_BenchMarks_List()
        {
            MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);
            Benchmarks_List = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");
        }

        private void Outcomes_Grid_SelectionChanged(object sender, EventArgs e)
        {
            OutComesControlsfill();
        }

        void OutComesControlsfill()
        {
            if (Outcomes_Grid.Rows.Count > 0)
            {
                Txt_Out_Code.Text = Outcomes_Grid.CurrentRow.Cells["Out_Code"].Value.ToString();
                Txt_Out_Desc.Text = Outcomes_Grid.CurrentRow.Cells["Out_Desc"].Value.ToString();
                Txt_Out_Points.Text = Outcomes_Grid.CurrentRow.Cells["Out_Points"].Value.ToString();
            }
        }

        private void Reasons_Grid_SelectionChanged(object sender, EventArgs e)
        {
            ReasonsControlsfill();
        }

        void ReasonsControlsfill()
        {
            if (Reasons_Grid.Rows.Count > 0)
            {
                txtReason.Text = Reasons_Grid.CurrentRow.Cells["Reason_Code"].Value.ToString();
                txtReasonDesc.Text = Reasons_Grid.CurrentRow.Cells["Reason_Desc"].Value.ToString();
                CommonFunctions.SetComboBoxValue(cmbReason, Reasons_Grid.CurrentRow.Cells["Reason_PN"].Value.ToString());
            }
        }

        private void Outcomes_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {
                Txt_Out_Desc.Enabled = true; Txt_Out_Points.Enabled = true; Txt_Out_Code.Enabled = false;
                Pb_Save_Out.Visible = Pb_Cancel_Out.Visible = true;
                picAddScale.Enabled =  Outcomes_Grid.Enabled = false;
                Txt_Out_Desc.Focus();


            }
            else if (e.ColumnIndex == 4 && e.RowIndex != -1)
            {
                Added_Edited_OutComeCode = string.Empty;
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: Delete_Selected_Outcomes);

            }

        }

        private void Reasons_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {
                txtReasonDesc.Enabled = true; txtReason.Enabled = false;
                Pb_Save_Reason.Visible = Pb_Cancel_Reason.Visible = true;
                Reasons_Grid.Enabled = false;
                cmbReason.Enabled = true;
                txtReasonDesc.Focus();
                picAddreason.Enabled = false;
            }
            else if (e.ColumnIndex == 4 && e.RowIndex != -1)
            {
                Added_Edited_ReasonCode = string.Empty;
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: Delete_Selected_Reason);

            }
        }

        private void Delete_Selected_Reason(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                if (dialogResult == DialogResult.Yes)
                {
                    txtReasonDesc.Text = string.Empty;
                    Txt_Out_Points.Text = string.Empty;
                    MATREASNEntity Search_Entity = new MATREASNEntity();
                    Search_Entity.Rec_Type = "D";

                    Search_Entity.MatCode = Matcode.ToString();
                    Search_Entity.Scl_Code = ScaleCode.ToString();
                    Search_Entity.Code = Reasons_Grid.CurrentRow.Cells["Reason_Code"].Value.ToString();
                    Search_Entity.Desc = "Delete Reason Desc";
                    string strmsg = string.Empty;
                    if (_model.MatrixScalesData.UpdateMATREASN(Search_Entity, "Delete", out strmsg))
                    {
                    AlertBox.Show("Reason Deleted Successfully", MessageBoxIcon.Information, null, System.Drawing.ContentAlignment.BottomRight);
                        // MessageBox.Show("Reason Deleted Successfully...", "CAP Systems");
                        Get_Reasons_List();
                        Fill_Sel_Matrix_Reasons(strmsg);
                    }
                   // else
                      //  MessageBox.Show("Reason Deleted Unsuccessful...", "CAP Systems");
                    ClearControls();
                }
           // }
        }

        private void Delete_Selected_Outcomes(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                if (dialogResult == DialogResult.Yes)
                {
                    Txt_Out_Desc.Text = string.Empty;
                    Txt_Out_Points.Text = string.Empty;
                    MATOUTCEntity Search_Entity = new MATOUTCEntity();
                    Search_Entity.Rec_Type = "D";

                    Search_Entity.MatCode = Matcode;
                    Search_Entity.SclCode = ((ListItem)cmbScales.SelectedItem).Value.ToString();
                    Search_Entity.BmCode = ((ListItem)Cmb_Benchmarks.SelectedItem).Value.ToString();//BenchMarks_Grid.CurrentRow.Cells["Bench_Code"].Value.ToString();
                    Search_Entity.Code = Outcomes_Grid.CurrentRow.Cells["Out_Code"].Value.ToString();
                    Search_Entity.Desc = "Delete Reason Desc";
                    Search_Entity.Points = "0";
                    string strmsg = string.Empty;
                    Added_Edited_OutComeCode = string.Empty;
                    bool boolSuccess = _model.MatrixScalesData.UpdateMATOUTC(Search_Entity, "Delete", out strmsg);
                    if (strmsg == "Exists")
                    {
                        MessageBox.Show("Outcome cannot be deleted because Score Sheet have been defined", "CAP Systems");
                    }
                    else
                    {
                        if (boolSuccess)
                        {
                            AlertBox.Show("Outcome Deleted Successfully", MessageBoxIcon.Information, null, System.Drawing.ContentAlignment.BottomRight);
                        //MessageBox.Show("Outcome Deleted Successfully...", "CAP Systems");
                            Get_Outcomes_List();
                            Fill_Sel_BenchMark_Outcomes(Added_Edited_OutComeCode, ((ListItem)cmbScales.SelectedItem).Value.ToString(), ((ListItem)Cmb_Benchmarks.SelectedItem).Value.ToString());

                        }
                        else
                        AlertBox.Show("Reason Deleted Successfully", MessageBoxIcon.Information, null, System.Drawing.ContentAlignment.BottomRight);
                    // MessageBox.Show("Reason Deleted Unsuccessful...", "CAP Systems");
                }

                    ClearControls();
                }
            //}
        }

        private void picAddScale_Click(object sender, EventArgs e)
        {
            
           picAddScale.Enabled = Txt_Out_Desc.Enabled = Txt_Out_Points.Enabled = true; Txt_Out_Code.Enabled = false;
            Txt_Out_Code.Clear(); Txt_Out_Desc.Clear(); Txt_Out_Points.Clear(); //Txt_Bench_High.Clear();
            Pb_Save_Out.Visible = Pb_Cancel_Out.Visible = true;
            Outcomes_Grid.Enabled = false;
            Txt_Out_Code.Text = "New";
            Txt_Out_Desc.Focus();
        }

        private void picAddreason_Click(object sender, EventArgs e)
        {
            picAddreason.Enabled = txtReasonDesc.Enabled = cmbReason.Enabled = true; txtReason.Enabled = false;
            txtReason.Clear(); txtReasonDesc.Clear(); //Txt_Bench_Low.Clear(); Txt_Bench_High.Clear();               
            Reasons_Grid.Enabled = false;
            txtReason.Text = "New";
            Pb_Save_Reason.Visible = Pb_Cancel_Reason.Visible = true;
            txtReasonDesc.Focus();
        }

        private void cmbScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Sel_BenchMark_Outcomes(Added_Edited_OutComeCode, ((ListItem)cmbScales.SelectedItem).Value.ToString(), ((ListItem)Cmb_Benchmarks.SelectedItem).Value.ToString());
        }

        private void Cmb_Benchmarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Sel_BenchMark_Outcomes(Added_Edited_OutComeCode, ((ListItem)cmbScales.SelectedItem).Value.ToString(), ((ListItem)Cmb_Benchmarks.SelectedItem).Value.ToString());
        }

        private void MatOutcomesReasons_Load(object sender, EventArgs e)
        {
            //if (strType == "Outcomes")
            //{
            //    Fill_Sel_BenchMark_Outcomes(string.Empty, ScaleCode, Benchmarkcode);
            //    OutComesControlsfill();
            //}
        }

        private void MatOutcomesReasons_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (strType == "Outcomes")
                Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 4, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            else if (strType == "Reasons")
                Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 6, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }
    }
}