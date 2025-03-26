#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.Controls.Compatibility;
using Spire.Pdf.Grid;
using Captain.Common.Interfaces;
using System.IO;
using DevExpress.PivotGrid.OLAP.SchemaEntities;
using DevExpress.XtraSpreadsheet.Model;
using Syncfusion.Calculate;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00133QuestionView : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        int strIndex = 0;
        int strPageIndex = 1;
        public HSS00133QuestionView(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity, string strFormType, string CustomQues, List<CustomQuestionsEntity> custQuestionslist)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            this.Text = privilegeEntity.PrivilegeName + " - Questions";
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            propReportPath = _model.lookupDataAccess.GetReportPath();

            if (Privileges.AddPriv.Equals("false"))
                btnAdd.Visible = false;
            if (Privileges.ChangePriv.Equals("false"))
                gvwQuestion.Columns["gviEdit"].Visible = false;
            if (Privileges.DelPriv.Equals("false"))
                gvwQuestion.Columns["gviDel"].Visible = false;
            FillAllCombos();
            CmbType.SelectedIndex = 0;
            TxtCode.Validator = TextBoxValidation.IntegerValidator;

            custQuestions = custQuestionslist;// _model.FieldControls.GetCustomQuestions(Privileges.Program, "*", string.Empty, string.Empty, string.Empty, string.Empty);
            fillCustQuestions(CustomQues);
            
           
            Btn_Cancel_Click(btnCancel, new EventArgs());
            if (gvwQuestion.Rows.Count > 0)
            {
                gvwQuestion.Rows[0].Selected = true;
                gvwQuestion.CurrentCell = gvwQuestion.Rows[0].Cells[1];
                gvwQuestion_SelectionChanged(gvwQuestion, new EventArgs());
            }

            if(BaseForm.UserID.ToUpper() == "JAKE")
                this.Tools["tlExcel"].Visible = true;
            else
                this.Tools["tlExcel"].Visible = false;
        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string Mode { get; set; }
        public List<CustomQuestionsEntity> custQuestions { get; set; }
        public string propCustomQuestios { get; set; }
        public string propReportPath
        {
            get; set;
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
            List<CustomQuestionsEntity> custQuestionsList = custQuestions;
            gvwQuestion.Rows.Clear();
            foreach (CustomQuestionsEntity entityQuestion in custQuestionsList)
            {
                string flag = "false";
                if (custQues != null && QuesList.Contains(entityQuestion.CUSTCODE))
                {
                    flag = "true";
                }
                //string strResponcetype = string.Empty;
                //switch (entityQuestion.CUSTRESPTYPE)
                //{
                //    case "D":
                //        strResponcetype = "Dropdown"; break;
                //    case "T":
                //        strResponcetype = "Date"; break;
                //    case "N":
                //        strResponcetype = "Numeric"; break;
                //    case "X":
                //        strResponcetype = "Text"; break;

                //}
                int rowIndex = gvwQuestion.Rows.Add(flag, entityQuestion.CUSTDESC.Trim(), entityQuestion.CUSTCODE,  entityQuestion.CUSTRESPTYPE, entityQuestion.CUSTACTIVECUST, entityQuestion.CUSTCODE);

                gvwQuestion.Rows[rowIndex].Tag = entityQuestion;
                if (entityQuestion.CUSTACTIVECUST != "A")
                    gvwQuestion.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                CommonFunctions.setTooltip(rowIndex, entityQuestion.addoperator, entityQuestion.adddate, entityQuestion.lstcoperator, entityQuestion.lstcdate, gvwQuestion);

            }
            if (Mode.ToUpper() == "EDIT")
            {
                gvwQuestion.Sort(gvwQuestion.Columns["gvcheckdetails"], ListSortDirection.Descending);
            }

            if (gvwQuestion.Rows.Count > 0) 
            {
                gvwQuestion.Rows[selIndex].Selected = true;
                gvwQuestion.CurrentCell = gvwQuestion.Rows[selIndex].Cells[1];
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<CustomQuestionsEntity> selectedQuestions = (from c in gvwQuestion.Rows.Cast<DataGridViewRow>().ToList()
                                                             where (((DataGridViewCheckBoxCell)c.Cells["gvcheckdetails"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                             select ((DataGridViewRow)c).Tag as CustomQuestionsEntity).ToList();

            if (selectedQuestions.Count == 1)
            {
                string strCustQues = string.Empty;
                foreach (CustomQuestionsEntity custQuestion in selectedQuestions)
                {
                    if (!strCustQues.Equals(string.Empty)) strCustQues += " ";
                    strCustQues += custQuestion.CUSTCODE.ToString();
                }
                propCustomQuestios = strCustQues;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (selectedQuestions.Count == 0)
            {
                CommonFunctions.MessageBoxDisplay("Select atleast one question");
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please select only one question");
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CustRespGrid_SelectionChanged(object sender, EventArgs e)
        {
            bool CanSave = true, Delete_SW = false;

            //if (PrivRow == null)
            //    PrivRow.Tag = 0; 
            if (Cust_SCR_Mode != "View")
            {
                if (CustResp_lod_complete && PrivRow != null && PrivRow.Index >= 0)
                {
                    if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) ||
                        string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()))
                    {

                        if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) &&
                            string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
                            !string.IsNullOrEmpty(PrivRow.Cells["Empty_Row"].EditedFormattedValue.ToString()))
                        {
                            MessageBox.Show("This Response Will be Deleted", "CapSystems", MessageBoxButtons.OK);
                            Delete_SW = true;
                        }
                        //else
                        //{
                        //    MessageBox.Show("Please Fill 'Code' and 'Description'", "CAP Systems", MessageBoxButtons.OK); CanSave = false;
                        //}
                    }


                    if (CanSave && CheckDupCustResps(Delete_SW))
                    {
                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Type"].EditedFormattedValue.ToString())))
                        {
                            foreach (CustRespEntity Entity in OrgCustResp)
                            {
                                if (PrivRow.Cells["CustSeq"].Value.ToString() == Entity.RespSeq)
                                {
                                    if (!Delete_SW)
                                    {
                                        Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                                        Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
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
                                PrivRow.Cells["Empty_Row"].Value = "N";
                                PrivRow.Cells["CustSeq"].Value = (OrgCustResp.Count + 1).ToString();

                                CustRespEntity New_Entity = new CustRespEntity();
                                New_Entity.RecType = "I";
                                New_Entity.ScrCode = Privileges.Program;
                                if (Cust_SCR_Mode.Equals("Edit"))
                                    New_Entity.ResoCode = gvwQuestion.CurrentRow.Cells["CustomKey"].Value.ToString();
                                else
                                {
                                    if (!string.IsNullOrEmpty(TxtCode.Text.Trim()))
                                        New_Entity.ResoCode = "C" + TxtCode.Text.Trim();
                                }
                                New_Entity.RespSeq = (OrgCustResp.Count + 1).ToString();
                                New_Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                New_Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                                New_Entity.AddOpr = BaseForm.UserID;
                                New_Entity.ChgOpr = BaseForm.UserID;
                                New_Entity.Changed = "Y";

                                OrgCustResp.Add(new CustRespEntity(New_Entity));
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

        private void gvwQuestion_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwQuestion.Rows.Count > 0)
            {
                if (gvwQuestion.SelectedRows[0].Selected)
                {
                    strIndex = gvwQuestion.SelectedRows[0].Index;
                    //strPageIndex = gvwQuestion.CurrentPage;
                    CustRespGrid.Rows.Clear();
                    OrgCustResp.Clear();
                    FillCustomControls(GetSelectedCustomKey());
                    if ((gvwQuestion.SelectedRows[0].Cells["gvtQueType1"].Value.ToString() == "D"))
                    {
                        CustRespPanel.Visible = true;


                    }
                    else
                    {
                        CustRespPanel.Visible = false;
                    }

                }
            }
        }

        private void FillCustomControls(string RecKey)
        {
            List<CustfldsEntity> Entity = _model.FieldControls.GetSelCustDetails(RecKey);
            if (Entity.Count > 0)
            {
                CustfldsEntity Cust = Entity[0];
                TxtCode.Text = Cust.CustCode;
                MtxtSeq.Text = SetLeadingZeros(Cust.CustSeq);
                TxtQuesDesc.Text = Cust.CustDesc;
                TxtAbbr.Text = Cust.Question;
                TxtType.Text = Cust.RespType;
                TxtAccess.Text = Cust.MemAccess;
                CommonFunctions.SetComboBoxValue(CmbType, Cust.RespType);
                CustRespPanel.Visible = FurtreDtPanel.Visible = false;
                if (Cust.FutureDate == "Y")
                    CbFDate.Checked = true;
                else
                    CbFDate.Checked = false;

                if (Cust.Active == "A")
                    CbActive.Checked = true;
                else
                    CbActive.Checked = false;

                switch (Cust.RespType)
                {
                    //case "D":
                    //case "C": FillCustRespGrid(RecKey); break;                  
                    case "T": FurtreDtPanel.Visible = true; break;
                }

                //Check_Answers_for_CustCode(RecKey);
                if (Cust.RespType == "D" || Cust.RespType == "C")
                    FillCustRespGrid(RecKey);
                else
                    CustRespPanel.Visible = false;

            }
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }
            return (TmpCode);
        }

        private bool CheckDupCustResps(bool Delete_SW)
        {
            bool ReturnVal = true;
            if (!Delete_SW)
            {
                string TestCode, TestDesc, TmpSelCode = null, TmpSelDesc = null;
                TmpSelCode = PrivRow.Cells["RespCode"].Value == null ? null : PrivRow.Cells["RespCode"].Value.ToString();
                TmpSelDesc = PrivRow.Cells["RespDesc"].Value == null ? null : PrivRow.Cells["RespDesc"].Value.ToString();
                foreach (DataGridViewRow dr in CustRespGrid.Rows)
                {
                    TestCode = TestDesc = null;
                    if (!(string.IsNullOrEmpty(dr.Cells["RespCode"].FormattedValue.ToString())))
                        TestCode = dr.Cells["RespCode"].Value.ToString().Trim();
                    if (!(string.IsNullOrEmpty(dr.Cells["RespDesc"].FormattedValue.ToString())))
                        TestDesc = dr.Cells["RespDesc"].Value.ToString().Trim();

                    if (TmpSelCode == TestCode && PrivRow != dr)
                    {
                        ReturnVal = false;
                        MessageBox.Show("Response Code '" + "'" + TmpSelCode + "' Already Exists", "CAP Systems", MessageBoxButtons.OK);
                        break;
                    }
                    if (TmpSelDesc == TestDesc && PrivRow != dr)
                    {
                        ReturnVal = false;
                        MessageBox.Show("Code Description '" + TmpSelDesc + "' Already Exists", "CAP Systems", MessageBoxButtons.OK);
                        break;
                    }
                }
            }
            return ReturnVal;
        }

        DataGridViewRow PrivRow;
        bool CustResp_lod_complete = false;
        private void FillCustRespGrid(string CustCode)
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            Clear_CustRespGrid();
            OrgCustResp.Clear();
            CaptainModel model = new CaptainModel();
            List<CustRespEntity> CustResp = model.FieldControls.GetCustRespByCustCode(CustCode);
            if (CustResp.Count > 0)
            {
                CustRespPanel.Visible = true;
                int rowIndex = 0;
                int RowCount = 0;
                foreach (CustRespEntity Entity in CustResp)
                {
                    //rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc);  
                    rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc.Trim(), Entity.RecType, Entity.Changed, Entity.RespSeq, "N");
                    RowCount++;
                }
                if (RowCount > 0)
                {

                    OrgCustResp = CustResp;
                    CustResp_lod_complete = true;
                    CustRespGrid.Rows[rowIndex].Tag = 0;
                    //CustRespGrid.SelectedRows[rowIndex - 1].Selected = true;
                    //if (CustRespGrid.SelectedRows[0].Selected)
                    //    PrivRow = CustRespGrid.SelectedRows[0];
                }

                //if (RowCount > 3)
                //    this.CustRespGrid.Size = new System.Drawing.Size(284, 131);
                //else
                //    this.CustRespGrid.Size = new System.Drawing.Size(266, 131);

            }
            CustResp_lod_complete = true;
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);

        }

        private void CustRespGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (gvwQuestion.Rows.Count == 0)
                pnlAllControls.Visible = true;

            Btn_Save.Visible = Btn_Cancel.Visible = true;            
                btnAdd.Visible =  btnCancel.Visible = btnSave.Visible = false;
            Enable_Disable_Cust_Controls(true);
            Clear_Cust_Controls();
            Cust_SCR_Mode = "Add";

        }

        private void Clear_Cust_Controls()
        {
            MtxtSeq.Clear();
            TxtCode.Clear();
            TxtQuesDesc.Clear();
            TxtAbbr.Clear();
            CbActive.Checked = true;
            Clear_CustRespGrid();
            CommonFunctions.SetComboBoxValue(CmbType, "0");
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
        }

        private void FillAllCombos()
        {
            CmbType.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            //listItem.Add(new ListItem("Check Box", "C"));
            listItem.Add(new ListItem("Date", "T"));
            listItem.Add(new ListItem("Drop Down", "D"));
            listItem.Add(new ListItem("Numeric", "N"));
            listItem.Add(new ListItem("Text", "X"));
            CmbType.Items.AddRange(listItem.ToArray());

        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Cust_SCR_Mode = "View";


            if (Privileges.AddPriv.Equals("true"))
                btnAdd.Visible = true;
            else
                btnAdd.Visible = false;

           
            btnCancel.Visible = btnSave.Visible =   true;
            Enable_Disable_Cust_Controls(false);

            if (gvwQuestion.Rows.Count > 0)
                gvwQuestion_SelectionChanged(gvwQuestion, EventArgs.Empty);
            else
            {
                pnlAllControls.Visible = false;
                CustRespPanel.Visible = false;
            }
        }

        int Cust_PrivRow_Index = 0;
        string Edited_Cust_Code = string.Empty;
        List<CustRespEntity> OrgCustResp = new List<CustRespEntity>();
        string Cust_SCR_Mode = "View";
        private void Btn_Save_Click(object sender, EventArgs e)
        {

            if (ValidateCustControls())
            {
                this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
                CaptainModel model = new CaptainModel();
                CustfldsEntity CustDetails = new CustfldsEntity();
                if (Cust_SCR_Mode == "Edit")
                {
                    List<CustfldsEntity> Entity = model.FieldControls.GetSelCustDetails(GetSelectedCustomKey());
                    CustDetails = Entity[0];
                    CustDetails.UpdateType = "U";
                    // CustDetails.CustSeq = SetLeadingZeros(MtxtSeq.Text);
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Question = TxtAbbr.Text;
                    CustDetails.RespType = ((ListItem)CmbType.SelectedItem).Value.ToString();
                    // CustDetails.MemAccess = ((ListItem)CmbAccess.SelectedItem).Value.ToString();
                    CustDetails.CustSeq = MtxtSeq.Text;
                    CustDetails.Equalto = CustDetails.Greater = CustDetails.Less = "0";

                    CustDetails.Active = "I";
                    if (CbActive.Checked)
                        CustDetails.Active = "A";

                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.ChdOpr = BaseForm.UserID;
                }
                else
                {
                    CustDetails.UpdateType = "I";
                    CustDetails.ScrCode = Privileges.Program;//"HSS00133";
                    CustDetails.CustCode = "C" + TxtCode.Text;
                    CustDetails.CustSeq = string.Empty;
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Question = TxtAbbr.Text;
                    CustDetails.RespType = ((ListItem)CmbType.SelectedItem).Value.ToString(); ;
                    CustDetails.MemAccess = string.Empty;
                    CustDetails.CustSeq = string.Empty;
                    CustDetails.Equalto = CustDetails.Greater = CustDetails.Less = "0";

                    CustDetails.FutureDate = "N";
                    CustDetails.Active = "I";
                    if (CbActive.Checked)
                        CustDetails.Active = "A";


                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.AddOpr = BaseForm.UserID;
                    CustDetails.ChdOpr = BaseForm.UserID;
                }
                switch (((ListItem)CmbType.SelectedItem).Value.ToString())
                {
                    case "C":
                    case "D":
                        CustRespPanel.Visible = true; break;
                    case "N": CustRespPanel.Visible = false;
                        break;
                    case "T": if (CbFDate.Checked)
                            CustDetails.FutureDate = "Y"; break;
                }


                string New_CUST_Code_Code = "NewCustCode";

                if (model.FieldControls.UpdateCUSTFLDS(CustDetails, out New_CUST_Code_Code))
                {
                    Edited_Cust_Code = New_CUST_Code_Code;

                    //CustResp_lod_complete = Tab2_Loading_Complete = false;

                    if (CustDetails.RespType == "C" || CustDetails.RespType == "D")
                        model.FieldControls.UpdateCUSTRESP(OrgCustResp, New_CUST_Code_Code);
                    FillCustomGrid();

                    Btn_Save.Visible = Btn_Cancel.Visible = false;
                    btnCancel.Visible = btnSave.Visible =   true;
                   
                    if (Privileges.AddPriv.Equals("true"))
                        btnAdd.Visible = true;

                    //if (Privileges.DelPriv.Equals("true"))
                    //    PbDelete.Visible = true;

                    //if (Privileges.AddPriv.Equals("true"))
                    //    Pb_Add_Cust.Visible = true;

                    //if (Privileges.ChangePriv.Equals("true"))
                    //    Pb_Edit_Cust.Visible = true;

                    Enable_Disable_Cust_Controls(false);
                    if (gvwQuestion.Rows.Count != 0)
                    {
                        gvwQuestion.Rows[strIndex].Selected = true;
                        gvwQuestion.CurrentCell = gvwQuestion.Rows[strIndex].Cells[1];
                        //gvwQuestion.CurrentPage = strPageIndex;
                        gvwQuestion_SelectionChanged(sender, e);
                    }

                    // this.Close();
                }
                else
                {
                    ////if (CustDetails.UpdateType == "U")
                    ////    MessageBox.Show("UnSuccessful Record Updated ", "CAP Systems", MessageBoxButtons.OK);
                    ////else
                    ////    MessageBox.Show("UnSuccessful Record Insert ", "CAP Systems", MessageBoxButtons.OK);
                }
                this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            }
        }

        private void FillCustomGrid()
        {
            gvwQuestion.SelectionChanged -= new EventHandler(gvwQuestion_SelectionChanged);
            gvwQuestion.CellValueChanged -=new DataGridViewCellEventHandler(gvwQuestion_CellValueChanged);    
            gvwQuestion.Rows.Clear();
            //BtnDeleteCust.Visible = false;
            // PbDelete.Visible = Pb_Edit_Cust.Visible = false;

            //if (Privileges.AddPriv.Equals("true"))
            //    Pb_Add_Cust.Visible = true;
            custQuestions = _model.FieldControls.GetCustomQuestions(Privileges.Program, "*", string.Empty, string.Empty, string.Empty, string.Empty);
            fillCustQuestions(string.Empty);
            FillCustomControls(GetSelectedCustomKey());
            gvwQuestion.SelectionChanged += new EventHandler(gvwQuestion_SelectionChanged);
            gvwQuestion.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestion_CellValueChanged);    
            //}
            //else
            //    CustCntlPanel.Visible = false;
        }



        private bool ValidateCustControls()
        {
            bool isValid = true;

            //if ((String.IsNullOrEmpty(MtxtSeq.Text)))
            //{
            //    _errorProvider.SetError(MtxtSeq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label3.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(MtxtSeq, null);

            if (((ListItem)CmbType.SelectedItem).Value.ToString().Equals("0"))
            {
                _errorProvider.SetError(CmbType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label4.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbType, null);


            if (string.IsNullOrEmpty(TxtQuesDesc.Text.Trim()))
            {
                _errorProvider.SetError(TxtQuesDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label9.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtQuesDesc, null);


            if (!(CustRespGrid.Rows.Count > 1) && (((ListItem)CmbType.SelectedItem).Value.ToString().Equals("C") || ((ListItem)CmbType.SelectedItem).Value.ToString().Equals("D")))
            {
                label13.Text = " "; label13.Visible = true;
                _errorProvider.SetError(label13, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Response Grid".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                label13.Visible = false;
                _errorProvider.SetError(label13, null);
            }

            // isValid = CheckDupCustResps(false);

            return isValid;
        }


        private void Clear_CustRespGrid()
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
        }

        private string GetSelectedCustomKey()
        {
            string RecordKey = null;
            if (gvwQuestion != null)
            {
                try
                {
                    RecordKey = gvwQuestion.SelectedRows[0].Cells["CustomKey"].Value.ToString();
                }
                catch (Exception ex) { }
            }
            return RecordKey;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (gvwQuestion.Rows.Count > 0)
            {
                if (Cust_SCR_Mode == "Add")
                    gvwQuestion_SelectionChanged(gvwQuestion, EventArgs.Empty);

                Btn_Save.Visible = Btn_Cancel.Visible = true;
                btnCancel.Visible = btnSave.Visible =  false;
                Enable_Disable_Cust_Controls(true);
                Check_Answers_for_CustCode(gvwQuestion.CurrentRow.Cells["CustomKey"].Value.ToString());
                Cust_SCR_Mode = "Edit";
            }
        }

        private void Enable_Disable_Cust_Controls(bool Enable_SW)
        {
            if (!Enable_SW)
            {
                Cust_SCR_Mode = "View";
                label13.Visible = false;
                _errorProvider.SetError(label13, null);
                _errorProvider.SetError(TxtQuesDesc, null);
                _errorProvider.SetError(CmbType, null);
                _errorProvider.SetError(MtxtSeq, null);
            }

            Btn_Cancel.Visible = Btn_Save.Visible = Enable_SW;
            MtxtSeq.Enabled = TxtCode.Enabled = TxtQuesDesc.Enabled = TxtAbbr.Enabled = CbActive.Enabled =
             CmbType.Enabled = CbFDate.Enabled = Enable_SW;

            this.CustRespGrid.AllowUserToAddRows = Enable_SW;
            this.RespCode.ReadOnly = this.RespDesc.ReadOnly = !Enable_SW;

            if (Enable_SW)
                gvwQuestion.Enabled = gvwQuestion.Enabled = !Enable_SW; // PbDelete.Visible = Pb_Add_Cust.Visible = Pb_Edit_Cust.Visible = 
            else
                gvwQuestion.Enabled = !Enable_SW;

        }

        private void Check_Answers_for_CustCode(string QuesCode)
        {
            string FldCode = gvwQuestion.CurrentRow.Cells["CustomKey"].Value.ToString();
            DataSet ds = Captain.DatabaseLayer.FieldControlsDB.Browse_FLDCNTLHIE(Privileges.Program, null, FldCode, null, null, null, null);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                CmbType.Enabled = false;
            else
                CmbType.Enabled = true;
        }
        string RespType = "0";
        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RespType = ((ListItem)CmbType.SelectedItem).Value.ToString();
            _errorProvider.SetError(label13, null);
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
            OrgCustResp.Clear();
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            if (!((ListItem)CmbType.SelectedItem).Value.ToString().Equals("0"))
                _errorProvider.SetError(CmbType, null);

            switch (RespType)
            {
                case "C":
                case "D": FurtreDtPanel.Visible = false;
                    // NumericPanel.Visible = false;
                    CustRespPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(474, 147);
                    break;
                case "N": CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = false;
                    // NumericPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
                case "T": //NumericPanel.Visible = false;
                    CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
                default: //NumericPanel.Visible = false;
                    CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = false;
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
            }

            if (RespType == "D" || RespType == "C")
                CustRespPanel.Visible = true;
            else
                CustRespPanel.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvwQuestion.Rows.Count > 0)
            {
                bool Can_delete = false, Tmptrue = true;
                foreach (DataGridViewRow dr in gvwQuestion.Rows)
                {
                    if (dr.Selected)
                    {                      
                            Can_delete = true;
                    }
                }
                if (Can_delete)
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nSelected Custom Question(S) ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Sel_CustQues);
                else
                    MessageBox.Show("Please Select at least One Custom Question to Delete", "CAP Systems", MessageBoxButtons.OK);
            }
        }


        private void Delete_Sel_CustQues(DialogResult dialogResult)
        {
            bool Tmptrue = true, Delete_Statue = true, Any_Rec_deleted = false;
            //Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;

            //if (senderForm != null)
            //{
                if (DialogResult.Yes == dialogResult)
                {
                    foreach (DataGridViewRow dr in gvwQuestion.Rows)
                    {
                       if(dr.Selected)
                           if (!(_model.FieldControls.DeleteCUSTFLDS(Privileges.Program.Trim() + dr.Cells["CustomKey"].Value.ToString(), Privileges.Program.Trim())))
                           {
                               Delete_Statue = false;
                               CommonFunctions.MessageBoxDisplay("Question Used in Track Master Maintenance Screen");
                           }

                            Any_Rec_deleted = true;
                        }
                    }
                    if (Any_Rec_deleted)
                    {
                        //if (Delete_Statue)
                        //   // MessageBox.Show("Selected Custom Question(S) Deleted Successfully", "CAP Systems", MessageBoxButtons.OK);
                        //else
                        //    MessageBox.Show("Unsuccessful Delete ", "CAP Systems", MessageBoxButtons.OK);
                        if (strIndex != 0)
                            strIndex = strIndex - 1;
                        Edited_Cust_Code = string.Empty;
                        FillCustomGrid();

                        gvwQuestion.Rows[strIndex].Selected = true;
                        gvwQuestion.CurrentCell = gvwQuestion.Rows[strIndex].Cells[1];
                        //gvwQuestion.CurrentPage = strPageIndex;
                        gvwQuestion_SelectionChanged(gvwQuestion, EventArgs.Empty);
                    }
                }
        //}
        int selIndex = 0;
        private void gvwQuestion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == gviEdit.Index)
                {
                    if (gvwQuestion.Rows.Count > 0)
                    {
                        if (Cust_SCR_Mode == "Add")
                            gvwQuestion_SelectionChanged(gvwQuestion, EventArgs.Empty);

                        Btn_Save.Visible = Btn_Cancel.Visible = true;
                        btnAdd.Visible = btnCancel.Visible = btnSave.Visible = false;
                        Enable_Disable_Cust_Controls(true);
                        Check_Answers_for_CustCode(gvwQuestion.CurrentRow.Cells["CustomKey"].Value.ToString());
                        Cust_SCR_Mode = "Edit";
                    }
                    selIndex = gvwQuestion.CurrentRow.Index;
                }
                if (e.ColumnIndex == gviDel.Index)
                {
                    if (gvwQuestion.Rows.Count > 0)
                    {
                        bool Can_delete = false, Tmptrue = true;
                        foreach (DataGridViewRow dr in gvwQuestion.Rows)
                        {
                            if (dr.Selected)
                            {
                                Can_delete = true;
                            }
                        }
                        if (Can_delete)
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nSelected Custom Question(S) ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Sel_CustQues);
                        else
                            MessageBox.Show("Please Select at least One Custom Question to Delete", "CAP Systems", MessageBoxButtons.OK);
                    }

                }
            }
        }

        private void gvwQuestion_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvcheckdetails.Index)
            {
                if (gvwQuestion.Rows.Count > 0)
                {
                    gvwQuestion.CellValueChanged -= new DataGridViewCellEventHandler(gvwQuestion_CellValueChanged);
                    bool boolchecdetails = Convert.ToBoolean(gvwQuestion.Rows[e.RowIndex].Cells["gvcheckdetails"].Value == null ? false : gvwQuestion.Rows[e.RowIndex].Cells["gvcheckdetails"].Value);
                    foreach (DataGridViewRow item in gvwQuestion.Rows)
                    {
                        if (Convert.ToBoolean(item.Cells["gvcheckdetails"].Value) == true)
                        {
                            item.Cells["gvcheckdetails"].Value = false;
                        }
                    }
                    
                    if (boolchecdetails)
                        gvwQuestion.Rows[e.RowIndex].Cells["gvcheckdetails"].Value = true;
                    gvwQuestion.CellValueChanged += new DataGridViewCellEventHandler(gvwQuestion_CellValueChanged);
                }
            }
        }

        #region Added Excel icon in form to generate questions in excel by Vikash on 05/22/2024 as per "Head Start Enhancements" document

        private void HSS00133QuestionView_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Visible == true)
            {
                if (e.Tool.Name == "tlExcel")
                {
                    DataSet dsQues_Tasks = Captain.DatabaseLayer.FieldControlsDB.BROWSE_TMM_QUESTIONS_TASKS("HSS00133",string.Empty,"Q");
                    DataTable dtQues_Tasks = dsQues_Tasks.Tables[0];

                    PrintQuestionsinExcel(dtQues_Tasks);
                }
            }

        }

        string Random_Filename = null;
        private void PrintQuestionsinExcel(DataTable dtQues_Tasks)
        {
            #region PDF Name

            string PdfName = "Track_Master_Maintenance_Ques" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".xlsx";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xlsx";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xlsx";

            #endregion

            #region Excel Data
            int rowIndex = 0;
            int colIndex = 0;
            int totalTasks = 0;
            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties styles = new DevExpress_Excel_Properties();
                    styles.sxlbook = wb;
                    styles.sxlTitleFont = "calibri";
                    styles.sxlbodyFont = "calibri";

                    styles.getDevexpress_Excel_Properties();

                    wb.Unit = DevExpress.Office.DocumentUnit.Point;

                    if (dtQues_Tasks.Rows.Count > 0)
                    {
                        DevExpress.Spreadsheet.Worksheet ques_Sheet = wb.Worksheets[0];
                        ques_Sheet.Name = "Questions";
                        //ques_Sheet.ActiveView.ShowGridlines = false;

                        rowIndex = 1;
                        colIndex = 1;

                        ques_Sheet.Columns[1].Width = 60;
                        ques_Sheet.Columns[2].Width = 400;

                        ques_Sheet.Rows[rowIndex][colIndex].Value = "Question Code";
                        ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                        colIndex++;

                        ques_Sheet.Rows[rowIndex][colIndex].Value = "Field Description";
                        ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                        colIndex++;
                        ques_Sheet.FreezeRows(rowIndex);

                        int maxRepeatedTasks = Convert.ToInt32(dtQues_Tasks.Rows[0]["COL_COUNT"].ToString()); 
                            ////dtQues_Tasks.AsEnumerable()
                            //          .GroupBy(row => row.Field<string>("CUST_CODE"))
                            //          .Max(group => group.Count());

                        for (int i = 0; i < maxRepeatedTasks; i++)
                        {
                            ques_Sheet.Columns[colIndex].Width = 50;
                            ques_Sheet.Columns[colIndex + 1].Width = 200;

                            ques_Sheet.Rows[rowIndex][colIndex].Value = "Task_" + (i + 1) + " Code";
                            ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                            colIndex++;

                            ques_Sheet.Rows[rowIndex][colIndex].Value = "Task_" + (i + 1) + " Description";
                            ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                            colIndex++;
                            ques_Sheet.FreezeRows(rowIndex);
                        }

                        var distinctQues = dtQues_Tasks.AsEnumerable()
                                                   .GroupBy(row => row.Field<string>("CUST_CODE"))
                                                   .Select(group => group.First())
                                                   .CopyToDataTable();

                        foreach (DataRow drDistinct in distinctQues.Rows)
                        {
                            colIndex = 1;
                            rowIndex++;

                            ques_Sheet.Rows[rowIndex][colIndex].Value = drDistinct["CUST_CODE"].ToString().Trim();
                            ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                            colIndex++;

                            ques_Sheet.Rows[rowIndex][colIndex].Value = drDistinct["CUST_DESC"].ToString().Trim();
                            ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                            colIndex++;

                            DataRow[] drQues = dtQues_Tasks.Select("TRCK_CUSTQ_CODES = '" + drDistinct["CUST_CODE"] + "'");

                            if (drQues.Length > 0)
                            {
                                DataTable dtDistinct = drQues.CopyToDataTable();

                                var distinctTracks = dtDistinct.AsEnumerable()
                                                               .GroupBy(row => row.Field<string>("TRCK_TASK"))
                                                               .Select(group => group.First())
                                                               .CopyToDataTable();

                                DataView dv = distinctTracks.DefaultView;
                                dv.Sort = "TRCK_TASK";
                                distinctTracks = dv.ToTable();

                                foreach (DataRow drQues_Task in distinctTracks.Rows)
                                {
                                    ques_Sheet.Columns[colIndex].Width = 50;
                                    ques_Sheet.Columns[colIndex + 1].Width = 200;

                                    ques_Sheet.Rows[rowIndex][colIndex].Value = drQues_Task["TRCK_TASK"].ToString().Trim();
                                    ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                    colIndex++;

                                    ques_Sheet.Rows[rowIndex][colIndex].Value = drQues_Task["TRCK_TASK_DESC"].ToString().Trim();
                                    ques_Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                    colIndex++;
                                }
                            }
                        }
                    }

                    wb.Sheets.ActiveSheet = wb.Worksheets[0];
                    wb.SaveDocument(PdfName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                    try
                    {
                        string localFilePath = PdfName;

                        FileInfo fiDownload = new FileInfo(localFilePath);

                        string name = fiDownload.Name;
                        using (FileStream fileStream = fiDownload.OpenRead())
                        {
                            Application.Download(fileStream, name);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }

            #endregion
        }

        #endregion
    }




}
