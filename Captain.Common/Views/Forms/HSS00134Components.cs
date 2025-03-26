using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Utilities.MixedDGVUtilities;
using Captain.Common.Views.Forms.Base;
using DevExpress.DataAccess.Native.Data;
using DevExpress.ExpressApp.Validation.AllContextsView;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraSpreadsheet.Model;
using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wisej.Web;
using static Captain.Common.Utilities.Consts;

namespace Captain.Common.Views.Forms
{
    public partial class HSS00134Components : Form
    {
        #region Parameters
        string _componentCode = "";
        string _componentName = "";
        PrivilegeEntity _privilegeEntity = null;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        BaseForm _baseForm = null;
        string strTaskCount = "";
        string GchartCode = "";
        public List<CustomQuestionsEntity> propcustQuestions { get; set; }
        public List<CustRespEntity> propReponseEntity { get; set; }
        public List<ChldMediEntity> propChldMediEntity { get; set; }
        #endregion
        public HSS00134Components(BaseForm baseForm, string COMPNTcode, string COMPNTname, PrivilegeEntity privilege)
        {
            InitializeComponent();
            _baseForm = baseForm;
            _componentCode = COMPNTcode;
            _componentName = COMPNTname;
            _privilegeEntity = privilege;
            _model = new CaptainModel();
            propcustQuestions = _model.FieldControls.GetCustomQuestions("HSS00133", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            //propReponseEntity = _model.FieldControls.GetCustomResponses("HSS00133", string.Empty);
            //propChldMediEntity = _model.ChldTrckData.GetChldMediDetails(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, _baseForm.BaseApplicationNo, string.Empty, string.Empty);
            this.Text = _componentName;
            chkBulkPost.Checked = true;
            fillGrid();

        }

        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Add = Consts.Icons.ico_Add;
        string Img_History = Consts.Icons.ico_History;
        string Img_Delete = Consts.Icons.ico_Delete;

        void fillGrid()
        {
            dgvTasks.Rows.Clear();
            dgvTasks.CellValueChanged -= new DataGridViewCellEventHandler(dgvTasks_CellValueChanged);
            propReponseEntity = _model.FieldControls.GetCustomResponses("HSS00133", string.Empty);
            propChldMediEntity = _model.ChldTrckData.GetChldMediDetails(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty, _baseForm.BaseApplicationNo, string.Empty, string.Empty);
            List<ChldTrckEntity> Track_Det = new List<ChldTrckEntity>();
            List<ChldTrckEntity> TrackList = _model.ChldTrckData.Browse_CasetrckDetails("01");
            if (strTaskCount != string.Empty)
            {
                Track_Det = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
            }
            else
            {
                if (_componentCode == "0000")
                {
                    if (!string.IsNullOrEmpty(GchartCode.Trim()))
                        Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(_componentCode) && u.GCHARTCODE.Trim().Equals(GchartCode.Trim()));
                    else
                        Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(_componentCode));
                }
                else
                    Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(_componentCode) && u.Agency.Trim().Equals(_baseForm.BaseAgency.Trim()) && u.Dept.Equals(_baseForm.BaseDept.Trim()) && u.Prog.Equals(_baseForm.BaseProg.Trim()));
            }

            int i = 0; int IsEdit = 0;
            foreach (ChldTrckEntity Entity in Track_Det)
            {
                System.Data.DataTable _dtVals = new System.Data.DataTable();

                bool Sel_Ref = false;
                int rowIndex = 0;

                string _QuesType = "";
                string _QuesDesc = "";
                CustomQuestionsEntity dr = propcustQuestions.Find(u => u.CUSTCODE.Trim().ToString() == Entity.CustQCodes.Trim().ToString());
                if (dr != null)
                {
                    //lblQuestion.Text = "Question: " + dr.CUSTDESC;
                    _QuesType = dr.CUSTRESPTYPE;
                    _QuesDesc = dr.CUSTDESC;
                }


                List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => u.TASK.Equals(Entity.TASK.Trim()));
                if (chldMediEntity.Count > 0)
                {
                    foreach (ChldMediEntity mediEntity in chldMediEntity)
                    {
                        rowIndex = dgvTasks.Rows.Add(Entity.TASK.Trim(), Entity.CustQCodes, Entity.TASKDESCRIPTION.Trim(), "", LookupDataAccess.Getdate(mediEntity.ADDRESSED_DATE), LookupDataAccess.Getdate(mediEntity.COMPLETED_DATE), Img_Add, Img_Delete, Img_History, _QuesType, mediEntity.YEAR, mediEntity.SEQ, mediEntity.MediFund, ""); //
                        dgvTasks.Rows[rowIndex].Tag = Entity;
                        BuildCustQuesColumn(rowIndex, 3, _QuesType, _dtVals, Entity.CustQCodes);
                        if (chldMediEntity.Count > 0)
                            this.dgvTasks[3, rowIndex].Value = GetCustomQuesResp(Entity.CustQCodes.Trim(), mediEntity);

                        IsEdit++;
                    }
                }
                else
                {
                    rowIndex = dgvTasks.Rows.Add(Entity.TASK.Trim(), Entity.CustQCodes, Entity.TASKDESCRIPTION.Trim(), "", "", "", Img_Add, Img_Blank, Img_History, _QuesType, "", "", "", "I");
                    dgvTasks.Rows[rowIndex].Tag = Entity;
                    BuildCustQuesColumn(rowIndex, 3, _QuesType, _dtVals, Entity.CustQCodes);
                }


            }

            if (dgvTasks.Rows.Count > 0)
            {
                btnSave.Visible = false;
                btnCancle.Visible = false;
                foreach (DataGridViewColumn dgvCol in dgvTasks.Columns)
                {
                    dgvCol.ReadOnly = true;
                    //if(dgvCol.Name== "ColAdd")
                    //{

                    //}
                }
                //this.ColAdd.ReadOnly = true;

                if (_privilegeEntity.DelPriv == "false") { this.ColDel.Visible = false; } else { this.ColDel.Visible = true; }

                if (IsEdit > 0) { this.Tools["tl_edit"].Visible = true; this.Tools["tl_add"].Visible = false; } else { this.Tools["tl_edit"].Visible = false; this.Tools["tl_add"].Visible = true; }

                if (_privilegeEntity.AddPriv == "false") { this.ColAdd.Visible = false; this.Tools["tl_add"].Visible = false; } //else { this.ColDel.Visible = true; }
                if (_privilegeEntity.ChangePriv == "false") { this.Tools["tl_edit"].Visible = false; }

                dgvTasks.CellValueChanged += new DataGridViewCellEventHandler(dgvTasks_CellValueChanged);
            }
            else
            {
                btnSave.Visible = false;
                btnCancle.Visible = false;
                this.Tools["tl_add"].Visible = false;
                this.Tools["tl_edit"].Visible = false;
            }

        }
        private void BuildCustQuesColumn(int _rowindex, int _cellIndex, string _CellType, System.Data.DataTable _dtValues, string _QesCode)
        {
            var row = dgvTasks.Rows[_rowindex]; // dataGridView1.Rows[e.RowIndex];
            var col = dgvTasks.Columns[_cellIndex]; // dataGridView1.Columns[e.ColumnIndex];

            var dataType = _CellType; // (string)dataGridView1.GetValue(0, e.RowIndex);
            switch (dataType)
            {
                // TextBox 
                case "X":
                    {
                        DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell();
                        textBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px; ";
                        //row[col] = textBoxCell;
                        this.dgvTasks[3, _rowindex] = textBoxCell;
                    }
                    break;

                // ComboBox with DataSource, Value and Display binding.
                case "D":
                    {
                        List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", _QesCode);

                        DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                        if (custReponseEntity.Count > 0)
                        {
                            comboCell.DataSource = custReponseEntity; // DummyData.CreateComboBoxDataTable(dataType);
                            comboCell.ValueMember = "DescCode";
                            comboCell.DisplayMember = "RespDesc";
                        }
                        comboCell.Style.BackgroundImageSource = "combo-arrow";
                        comboCell.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                        comboCell.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px; ";

                        this.dgvTasks[3, _rowindex] = comboCell;


                        //row[col] = comboCell;
                    }
                    break;

                // CheckBox with true/false values.
                case "R":
                    {
                        var checkBoxCell = new DataGridViewCheckBoxCell();
                        checkBoxCell.TrueValue = "Y";
                        checkBoxCell.FalseValue = "N";
                        row[col] = checkBoxCell;
                    }
                    break;

                case "T":
                    {
                        DataGridViewDateTimePickerCell dateCell = new DataGridViewDateTimePickerCell();
                        dateCell.Format = DateTimePickerFormat.Short;
                        dateCell.Style.BackgroundImageSource = "icon-calendar";
                        dateCell.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                        row[col].ToolTipText = "Question Type: Date";
                        dateCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";


                        row[col] = dateCell;
                    }
                    break;
                case "N":
                    DataGridViewTextBoxCell txtNumeric = new DataGridViewTextBoxCell();
                    row[col].ToolTipText = "Question Type: Numeric";
                    txtNumeric.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";
                    row[col] = txtNumeric;
                    break;

                case "C":
                    {
                        var multiComboCell = new Captain.Common.Utilities.MixedDGVUtilities.DataGridViewMultiComboBoxCell();
                        //multiComboCell.DataSource = CreateComboBoxDataTable(dataType);
                        //multiComboCell.ValueMember = "ValueColumn";
                        //multiComboCell.DisplayMember = "DisplayColumn";

                        List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", _QesCode);


                        if (custReponseEntity.Count > 0)
                        {
                            multiComboCell.DataSource = GenrateMultiCombo(custReponseEntity); // custReponseEntity; // DummyData.CreateComboBoxDataTable(dataType);
                            multiComboCell.ValueMember = "Value";
                            multiComboCell.DisplayMember = "Desc";

                        }
                        multiComboCell.Style.BackgroundImageSource = "combo-arrow";
                        multiComboCell.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                        //multiComboCell.DropDownStyle = ComboBoxStyle.DropDownList;
                        multiComboCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px; ";

                        row[col] = multiComboCell;
                    }
                    break;

                default:

                    break;

            }
        }


        System.Data.DataTable GenrateMultiCombo(List<CustRespEntity> custReponseEntity)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("CheckedStateColumn", typeof(bool));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Desc", typeof(string));


            foreach (CustRespEntity _enty in custReponseEntity)
            {
                dt.Rows.Add(false, _enty.DescCode, _enty.RespDesc);
            }

            return dt;
        }


        private void dgvTasks_SelectionChanged(object sender, EventArgs e)
        {
            lblQuestion.Text = "";
            string custQues = dgvTasks.CurrentRow.Cells["ColQuesCode"].Value.ToString();
            CustomQuestionsEntity dr = propcustQuestions.Find(u => u.CUSTCODE.Trim().ToString() == custQues.Trim().ToString());
            if (dr != null)
            {
                lblQuestion.Text = "Question: " + dr.CUSTDESC;
            }
        }

        private string GetCustomQuesResp(string custQues, ChldMediEntity chldmediQueslist)
        {
            string strCustResponse = string.Empty;
            if (propcustQuestions.Count > 0)
            {
                if (propcustQuestions.Count > 0)
                {
                    CustomQuestionsEntity dr = propcustQuestions.Find(u => u.CUSTCODE.Trim().ToString() == custQues.Trim().ToString());
                    if (dr != null)
                    {

                        string fieldType = dr.CUSTRESPTYPE.ToString();

                        if (fieldType.Equals("D") || fieldType.Equals("C"))
                        {
                            string code = chldmediQueslist.ANSWER1.Trim();
                            strCustResponse = code;
                            //CustRespEntity custRespEntity = propReponseEntity.Find(u => u.DescCode.Trim().Equals(code) && u.ResoCode.Trim().Equals(custQues.Trim()));
                            //if (custRespEntity != null)
                            //{
                            //    strCustResponse = custRespEntity.RespDesc;
                            //}
                        }
                        else if (fieldType.Equals("N"))
                        {
                            strCustResponse = chldmediQueslist.ANSWER2.Trim().ToString();
                        }
                        else if (fieldType.Equals("T"))
                        {
                            strCustResponse = LookupDataAccess.Getdate(chldmediQueslist.ANSWER3.Trim().ToString());
                        }
                        else
                        {
                            strCustResponse = chldmediQueslist.ANSWER1.Trim().ToString();
                        }

                    }
                }

            }
            return strCustResponse;
        }

        private void dgvTasks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            string strMediSeq = string.Empty;

            //foreach (DataGridViewRow row in dgvTasks.Rows)
            //{
            //    string inputValue = string.Empty;
            //    inputValue = row.Cells["ColResponse"].Value != null ? row.Cells["ColResponse"].Value.ToString() : string.Empty;
            //    if (inputValue != "")
            //        sb.Append($" ({inputValue})");
            //    //sb.Append($"Row #{row.Index}: ");

            //    //foreach (DataGridViewCell cell in row.Cells)
            //    //{
            //    //    sb.Append(cell.FormattedValue);
            //    //    sb.Append(" ");

            //    //    if (cell.OwningColumn is MultiTypeDataGridViewColumn)
            //    //        sb.Append($" ({cell.Value})");
            //    //}

            //    //sb.AppendLine();
            //}

            //if (propPostForm == string.Empty)
            //{


            //List<DataGridViewRow> lstRespRows = dgvTasks.Rows.Where(x => (x.Cells["ColResponse"].Value != null)).ToList();
            List<DataGridViewRow> lstRespRows = dgvTasks.Rows.Where(x => (x.Cells["colAddressDate"].Value != null)).ToList();
            lstRespRows = lstRespRows.Where(x => (x.Cells["colAddressDate"].Value.ToString() != "") && x.Cells["colSavMode"].Value.ToString() == "I").ToList();
            //if (lstRespRows.Count > 0)
            if (isValid())
            {
                if (lstRespRows.Count > 0)
                {
                    int _saveFlag = 0;
                    foreach (DataGridViewRow row in lstRespRows)
                    {
                        string _Seq = row.Cells["colMediSeq"].Value.ToString();
                        string _Mode = "Add";
                        if (_Seq != "")
                            _Mode = "Edit";

                        string _appNo = _baseForm.BaseApplicationNo;
                        string _taskCode = row.Cells["coltskcode"].Value.ToString();
                        string _addressDate = row.Cells["colAddressDate"].Value != null ? row.Cells["colAddressDate"].Value.ToString() : "";
                        string _CompleteDate = row.Cells["colCompDate"].Value != null ? row.Cells["colCompDate"].Value.ToString() : "";
                        string _MidYear = row.Cells["ColYear"].Value.ToString();
                        string _MidFund = row.Cells["ColMediFund"].Value.ToString();

                        ChldMediEntity mediEntity = new ChldMediEntity();
                        mediEntity.AGENCY = _baseForm.BaseAgency;
                        mediEntity.DEPT = _baseForm.BaseDept;
                        mediEntity.PROG = _baseForm.BaseProg;
                        if (_Mode == "Edit")
                            mediEntity.YEAR = _MidYear; // have to add year // propChldMediEntity.YEAR;
                        else
                            mediEntity.YEAR = _baseForm.BaseYear;

                        mediEntity.APP_NO = _appNo;
                        mediEntity.TASK = _taskCode;

                        mediEntity.ADDRESSED_DATE = _addressDate;
                        mediEntity.COMPLETED_DATE = _CompleteDate;
                        //mediEntity.DIAGNOSIS_DATE = dtDiagnosed.Value.ToString();
                        //mediEntity.FOLLOWUP_DATE = dtFollowup.Value.ToString();
                        //mediEntity.FOLLOWUPC_DATE = dtFupCompleted.Value.ToString();
                        //mediEntity.SBCB_DATE = dtSBCB.Value.ToString();
                        //mediEntity.SPECIAL_DATE = dtSSR.Value.ToString();
                        //mediEntity.SPECIAL_WHERE = txtWheree.Text;

                        mediEntity.LSTC_OPERATOR = _baseForm.UserID;
                        mediEntity.ADD_OPERATOR = _baseForm.UserID;
                        mediEntity.COMPONENT = _componentCode;

                        //Need to check this FUND
                        //if (((ListItem)cmbFund.SelectedItem).Value.ToString() != "0")
                        //    mediEntity.MediFund = ((ListItem)cmbFund.SelectedItem).Value.ToString();

                        if (_Mode == "Edit")
                            mediEntity.MediFund = _MidFund;
                        else
                            mediEntity.MediFund = "";

                        mediEntity.SEQ = _Seq;
                        mediEntity.SN = string.Empty;
                        mediEntity.Mode = _Mode;


                        /***********************************************************************************************/
                        // RESPONSE FOR QUESTIONS
                        /***********************************************************************************************/
                        //string inputValue = string.Empty;
                        string strResponse = string.Empty;
                        strResponse = row.Cells["ColResponse"].Value != null ? row.Cells["ColResponse"].Value.ToString() : string.Empty;
                        if (strResponse != "")
                        {
                            //strResponse = row.Cells["ColResponse"].Tag != null ? row.Cells["ColResponse"].Value.ToString() : string.Empty;
                            //if (row.Cells[1].Tag == null && (row.Cells[1].Tag != null && !((string)row.Cells[1].Tag).Equals("U")))
                            //{
                            //    continue;
                            //}

                            string _QuesType = row.Cells["ColQtype"].Value.ToString();

                            if (_QuesType == "D")
                            {
                                mediEntity.ANSWER1 = strResponse;
                            }
                            else if (_QuesType == "C")
                            {
                                string multichkval = string.Empty;
                                //multichkval = row.Cells["ColResponse"].Value != null ? row.Cells["ColResponse"].Value.ToString() : string.Empty;
                                //if (multichkval != "")
                                //    sb.Append($" ({multichkval})");

                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    //sb.Append(cell.FormattedValue);
                                    //sb.Append(" ");

                                    if (cell.OwningColumn is MultiTypeDataGridViewColumn)
                                    {
                                        //sb.Append($" ({cell.Value})");
                                        multichkval = cell.Value.ToString();
                                    }
                                }


                                mediEntity.ANSWER1 = multichkval;
                            }
                            else if (_QuesType == "N")
                            {
                                mediEntity.ANSWER2 = strResponse;
                            }
                            else if (_QuesType == "T")
                            {
                                mediEntity.ANSWER3 = strResponse;
                            }
                            else
                            {
                                if (strResponse == string.Empty) continue;
                                mediEntity.ANSWER1 = strResponse; // dataGridViewRow.Cells["Response"].Value.ToString();
                                if (strResponse != string.Empty)
                                {
                                    mediEntity.ANSWER2 = CustomQuesValidation(mediEntity.ANSWER1, _taskCode);
                                }
                            }

                            // CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;

                            //if (questionEntity.CUSTRESPTYPE.Equals("D") || questionEntity.CUSTRESPTYPE.Equals("C"))
                            //{
                            //    if (dataGridViewRow.Cells["Response"].Tag == null || strResponse == string.Empty) continue;
                            //    mediEntity.ANSWER1 = dataGridViewRow.Cells["Response"].Tag.ToString();
                            //}
                            //else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                            //{
                            //    if (inputValue == string.Empty) continue;
                            //    mediEntity.ANSWER2 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                            //}
                            //else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                            //{
                            //    if (inputValue == string.Empty || inputValue == "  /  /") continue;
                            //    mediEntity.ANSWER3 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                            //}
                            //else
                            //{
                            //    if (inputValue == string.Empty) continue;
                            //    mediEntity.ANSWER1 = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                            //    if (inputValue != string.Empty)
                            //    {
                            //        mediEntity.ANSWER2 = CustomQuesValidation(mediEntity.ANSWER1);
                            //    }
                            //}
                        }
                        /***********************************************************************************************/

                        if (_model.ChldTrckData.InsertUpdateDelChldmedi(mediEntity, out strMediSeq))
                        {
                            _saveFlag++;
                        }



                    }

                    if (_saveFlag > 0)
                    {
                        chkBulkPost.Enabled = true;
                        chkBulkPost.ReadOnly = false;
                        AlertBox.Show("Saved Successfully");
                        fillGrid();
                    }
                }
                else
                    AlertBox.Show("Please give Response for any one of the question", MessageBoxIcon.Warning);
            }
            else
                AlertBox.Show("Please give Addressed date which we have response or Completed date", MessageBoxIcon.Warning);
            //AlertBox.Show("Please give Response for any one of the question", MessageBoxIcon.Warning);
        }

        public bool isValid()
        {
            bool IsValid = true;

            if (dgvTasks.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvTasks.Rows)
                {
                    string AddressDate = row.Cells["colAddressDate"].Value != null ? row.Cells["colAddressDate"].Value.ToString() : "";
                    string Response = row.Cells["ColResponse"].Value != null ? row.Cells["ColResponse"].Value.ToString() : "";
                    string CompDate = row.Cells["colCompDate"].Value != null ? row.Cells["colCompDate"].Value.ToString() : "";

                    if ((Response != string.Empty || CompDate != string.Empty) && (AddressDate == string.Empty))
                    {
                        //if ((row.Cells["ColResponse"].Value.ToString() != "" || row.Cells["colCompDate"].Value.ToString() != "") && (row.Cells["colAddressDate"].Value.ToString() == ""))
                        //{
                        IsValid = false;
                        break;
                        //}
                    }
                }
            }


            return IsValid;
        }

        private string CustomQuesValidation(string strMediAnswer1, string strTask)
        {
            string strAnswer2 = string.Empty;
            List<ChldTrckEntity> propchldTrckList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", strTask, string.Empty);
            ChldTrckEntity chldheightdata = propchldTrckList.Find(u => u.TASK == strTask);
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

        private void dgvTasks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvTasks.CellValueChanged -= new DataGridViewCellEventHandler(dgvTasks_CellValueChanged);
            if (dgvTasks.Rows.Count > 0)
            {
                if (e.ColumnIndex == 6) // Add New row
                {
                    if (dgvTasks.Columns["ColAdd"].ReadOnly == false)
                    {
                        ChldTrckEntity Entity = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Tag as ChldTrckEntity;

                        System.Data.DataTable _dtVals = new System.Data.DataTable();

                        string _QuesType = "";
                        string _QuesDesc = "";
                        CustomQuestionsEntity dr = propcustQuestions.Find(u => u.CUSTCODE.Trim().ToString() == Entity.CustQCodes.Trim().ToString());
                        if (dr != null)
                        {
                            //lblQuestion.Text = "Question: " + dr.CUSTDESC;
                            _QuesType = dr.CUSTRESPTYPE;
                            _QuesDesc = dr.CUSTDESC;
                        }

                        DataGridViewRow row = (DataGridViewRow)dgvTasks.Rows[dgvTasks.CurrentRow.Index].Clone();
                        dgvTasks.Rows.Insert(dgvTasks.CurrentRow.Index + 1, row);
                        row.Cells["coltskcode"].Value = Entity.TASK.Trim();
                        row.Cells["ColQuesCode"].Value = Entity.CustQCodes;
                        row.Cells["ColTask"].Value = Entity.TASKDESCRIPTION.Trim();
                        row.Cells["ColQtype"].Value = _QuesType;

                        row.Cells["ColAdd"].Value = Img_Add;
                        row.Cells["ColDel"].Value = Img_Blank;
                        row.Cells["ColHist"].Value = Img_History;

                        row.Cells["ColYear"].Value = "";
                        row.Cells["colMediSeq"].Value = "";
                        row.Cells["ColMediFund"].Value = "";
                        row.Cells["colSavMode"].Value = "I";
                        BuildCustQuesColumn(row.Index, 3, _QuesType, _dtVals, Entity.CustQCodes);
                    }

                }
                if (e.ColumnIndex == 7) // Delete Row
                {
                    if (dgvTasks.CurrentRow.Cells["ColDel"].Value != Img_Blank)
                        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                }
            }

            dgvTasks.CellValueChanged += new DataGridViewCellEventHandler(dgvTasks_CellValueChanged);
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
            //Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (DialogResult.Yes == dialogResult)
            {
                string strseq = string.Empty;
                if (dgvTasks.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow dr in dgvTasks.SelectedRows)
                    {
                        if (dr.Selected)
                        {
                            //GetSelectedRow();
                            ChldMediEntity mediEntity = new ChldMediEntity();
                            mediEntity.AGENCY = _baseForm.BaseAgency;
                            mediEntity.DEPT = _baseForm.BaseDept;
                            mediEntity.PROG = _baseForm.BaseProg;
                            mediEntity.YEAR = dgvTasks.CurrentRow.Cells["ColYear"].Value.ToString();
                            mediEntity.APP_NO = _baseForm.BaseApplicationNo;
                            mediEntity.SEQ = dgvTasks.CurrentRow.Cells["colMediSeq"].Value.ToString();
                            mediEntity.TASK = dgvTasks.CurrentRow.Cells["coltskcode"].Value.ToString();
                            mediEntity.COMPONENT = _componentCode;
                            mediEntity.Mode = "Delete";
                            if (_model.ChldTrckData.InsertUpdateDelChldmedi(mediEntity, out strseq))
                            {
                                CaseNotesEntity caseNotesDetails = new CaseNotesEntity();
                                caseNotesDetails.ScreenName = _privilegeEntity.Program;
                                caseNotesDetails.FieldName = _baseForm.BaseAgency + _baseForm.BaseDept + _baseForm.BaseProg + dgvTasks.CurrentRow.Cells["ColYear"].Value.ToString() + _baseForm.BaseApplicationNo + dgvTasks.CurrentRow.Cells["coltskcode"].Value.ToString() + dgvTasks.CurrentRow.Cells["colMediSeq"].Value.ToString();
                                caseNotesDetails.AppliCationNo = _baseForm.BaseAgency + _baseForm.BaseDept + _baseForm.BaseProg + dgvTasks.CurrentRow.Cells["ColYear"].Value.ToString() + _baseForm.BaseApplicationNo;
                                caseNotesDetails.Mode = "Del";
                                if (_model.TmsApcndata.InsertUpdateCaseNotes(caseNotesDetails))
                                { }
                                //if (strindex > 0)
                                //    strindex = strindex - 1;
                                //RefreshGrid();
                                fillGrid();

                                AlertBox.Show("Deleted Successfully");
                            }
                            else
                            {
                                MessageBox.Show("You can’t delete this record, as there are Dependents", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            //}
        }

        private void HSS00134Components_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tl_add")
            {
                chkBulkPost.Enabled = false;
                chkBulkPost.ReadOnly = true;
                if (chkBulkPost.Checked)
                {
                    btnSave.Visible = true;
                    btnCancle.Visible = true;
                    foreach (DataGridViewColumn dgvCol in dgvTasks.Columns)
                    {
                        dgvCol.ReadOnly = false;
                    }
                }
                else
                {
                    ChldTrckEntity Entity = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Tag as ChldTrckEntity;
                    string propTask = Entity.TASK.Trim();
                    string propSeq = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colMediSeq"].Value.ToString().Trim();
                    string propTaskDesc = Entity.TASKDESCRIPTION.Trim();
                    ChldMediEntity chldmediNew = new ChldMediEntity();
                    HSS00134Form hss00134form = new HSS00134Form("Add", _baseForm, _privilegeEntity,_componentCode, propTask, chldmediNew, string.Empty, propTaskDesc, _componentName.ToString(), "Components", "");
                    hss00134form.FormClosed += new FormClosedEventHandler(Hss00134formEdit_FormClosed);
                    hss00134form.StartPosition = FormStartPosition.CenterScreen;
                    hss00134form.ShowDialog();
                }
            }
            else if (e.Tool.Name == "tl_edit")
            {
                chkBulkPost.Enabled = false;
                chkBulkPost.ReadOnly = true;
                if (chkBulkPost.Checked)
                {
                    btnSave.Visible = true;
                    btnCancle.Visible = true;
                    foreach (DataGridViewColumn dgvCol in dgvTasks.Columns)
                    {
                        dgvCol.ReadOnly = false;
                    }
                }
                else
                {
                    ChldTrckEntity Entity = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Tag as ChldTrckEntity;
                    string propTask = Entity.TASK.Trim();
                    string propSeq = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colMediSeq"].Value.ToString().Trim();
                    string propTaskDesc = Entity.TASKDESCRIPTION.Trim();
                    string strMode = "Add";
                    ChldMediEntity _RowChldMediEntity = new ChldMediEntity();
                    List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => u.TASK.Equals(Entity.TASK.Trim()) && u.SEQ.Equals(propSeq));
                    if (chldMediEntity.Count > 0)
                    {
                        _RowChldMediEntity = chldMediEntity[0];
                        strMode = "Edit";
                    }
                    //GetSelectedRow();
                    HSS00134Form hss00134formEdit = new HSS00134Form(strMode, _baseForm, _privilegeEntity,_componentCode, propTask, _RowChldMediEntity, propSeq, propTaskDesc, _componentName.ToString(), "Components", "");
                    hss00134formEdit.FormClosed += new FormClosedEventHandler(Hss00134formEdit_FormClosed);
                    hss00134formEdit.StartPosition = FormStartPosition.CenterScreen;
                    hss00134formEdit.ShowDialog();
                }
            }
        }

        private void Hss00134formEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            chkBulkPost.Enabled = true;
            chkBulkPost.ReadOnly = false;
            HSS00134Form form = sender as HSS00134Form;
            if (form._CompFormSaveFlag == "Y")
            {
                fillGrid();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            if (dgvTasks.Rows.Count > 0)
            {
                chkBulkPost.Enabled = true;
                chkBulkPost.ReadOnly = false;
                btnSave.Visible = false;
                btnCancle.Visible = false;
                foreach (DataGridViewColumn dgvCol in dgvTasks.Columns)
                {
                    dgvCol.ReadOnly = true;
                }
                //this.ColAdd.ReadOnly = true;
                // if (IsEdit > 0) { this.Tools["tl_edit"].Visible = true; this.Tools["tl_add"].Visible = false; } else { this.Tools["tl_edit"].Visible = false; this.Tools["tl_add"].Visible = true; }
            }
            else
            {
                btnSave.Visible = false;
                btnCancle.Visible = false;
                this.Tools["tl_add"].Visible = false;
                this.Tools["tl_edit"].Visible = false;
                chkBulkPost.Enabled = true;
                chkBulkPost.ReadOnly = false;
            }
        }

        private void dgvTasks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTasks.Rows.Count > 0)
            {
                if (e.ColumnIndex == ColResponse.Index || e.ColumnIndex == colAddressDate.Index || e.ColumnIndex == colCompDate.Index)
                {
                    string propTask = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["coltskcode"].Value.ToString().Trim();
                    string propSeq = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colMediSeq"].Value.ToString().Trim();
                    string Qtype = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["ColQtype"].Value.ToString().Trim();
                    string _currResponse = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["ColResponse"].Value != null ?
                        dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["ColResponse"].Value.ToString().Trim() : "";
                    string _currAddressDate = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colAddressDate"].Value != null ?
                        dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colAddressDate"].Value.ToString().Trim() : "";
                    string _currCompleteDate = dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colCompDate"].Value != null ?
                        dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colCompDate"].Value.ToString().Trim() : "";

                    List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => u.TASK.Equals(propTask.Trim()) && u.SEQ.Equals(propSeq));
                    if (chldMediEntity.Count > 0)
                    {
                        dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colSavMode"].Value = "";
                        string PrvAns1 = chldMediEntity[0].ANSWER1;
                        string PrvAns2 = chldMediEntity[0].ANSWER2;
                        string PrvAns3 = chldMediEntity[0].ANSWER3;

                        if (Qtype == "D" || Qtype == "C") //Answer 1
                        {
                            if (_currResponse != PrvAns1)
                                dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colSavMode"].Value = "I";
                        }
                        else if (Qtype == "N") //Answer 2
                        {
                            if (_currResponse != PrvAns2)
                                dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colSavMode"].Value = "I";
                        }
                        else if (Qtype == "T") //Answer 3
                        {
                            if (_currResponse != PrvAns3)
                                dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colSavMode"].Value = "I";
                        }
                        else //Answer 1
                        {
                            if (_currResponse != PrvAns1)
                                dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colSavMode"].Value = "I";
                        }

                        string PrvAddressDate = chldMediEntity[0].ADDRESSED_DATE;
                        string PrvCompleteDate = chldMediEntity[0].COMPLETED_DATE;


                        if (_currAddressDate != PrvAddressDate)
                            dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colSavMode"].Value = "I";
                        if (_currCompleteDate != PrvCompleteDate)
                            dgvTasks.Rows[dgvTasks.CurrentRow.Index].Cells["colSavMode"].Value = "I";

                    }
                }
            }
        }
    }
}
