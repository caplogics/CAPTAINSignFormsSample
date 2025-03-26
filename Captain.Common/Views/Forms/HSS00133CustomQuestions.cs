#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00133CustomQuestions : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public HSS00133CustomQuestions(BaseForm baseForm, string mode,  PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
           
            this.Text = privilegeEntity.Program + " - " + Consts.Common.Add;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            FillAllCombos();
            CmbType.SelectedIndex = 0;
            //MtxtSeq.Validator = TextBoxValidation.IntegerMaskValidator;          
            TxtCode.Validator = TextBoxValidation.IntegerValidator;
            //MtxtSeq.Validator = TextBoxValidation.IntegerMaskValidator;
           
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

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        int Cust_PrivRow_Index = 0;
        string Edited_Cust_Code = string.Empty;
        List<CustRespEntity> OrgCustResp = new List<CustRespEntity>();
        private void Btn_Save_Click(object sender, EventArgs e)
        {
           
            if (ValidateCustControls())
            {
                CaptainModel model = new CaptainModel();
                CustfldsEntity CustDetails = new CustfldsEntity();
                //if (Cust_SCR_Mode == "Edit")
                //{
                //    List<CustfldsEntity> Entity = model.FieldControls.GetSelCustDetails(GetSelectedCustomKey());
                //    CustDetails = Entity[0];
                //    CustDetails.UpdateType = "U";
                //    CustDetails.CustSeq = SetLeadingZeros(MtxtSeq.Text);
                //    CustDetails.CustDesc = TxtQuesDesc.Text;
                //    CustDetails.Question = TxtAbbr.Text;
                //    CustDetails.RespType = ((ListItem)CmbType.SelectedItem).Value.ToString();
                //    CustDetails.MemAccess = ((ListItem)CmbAccess.SelectedItem).Value.ToString();
                //    CustDetails.CustSeq = MtxtSeq.Text;
                //    CustDetails.Equalto = CustDetails.Greater = CustDetails.Less = "0";

                //    CustDetails.Active = "I";
                //    if (CbActive.Checked)
                //        CustDetails.Active = "A";

                //    CustDetails.Alpha = null;
                //    CustDetails.Other = null;
                //    CustDetails.ChdOpr = BaseForm.UserID;
                //}
                //else
                //{
                    CustDetails.UpdateType = "I";
                    CustDetails.ScrCode = "HSS00133";
                    CustDetails.CustCode = "C" + TxtCode.Text;
                    CustDetails.CustSeq = string.Empty;
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Question = TxtAbbr.Text;
                    CustDetails.RespType = ((ListItem)CmbType.SelectedItem).Value.ToString(); ;
                    CustDetails.MemAccess = string.Empty;
                    CustDetails.CustSeq = MtxtSeq.Text;
                    CustDetails.Equalto = CustDetails.Greater = CustDetails.Less = "0";

                    CustDetails.FutureDate = "N";
                    CustDetails.Active = "I";
                    if (CbActive.Checked)
                        CustDetails.Active = "A";


                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.AddOpr = BaseForm.UserID;
                    CustDetails.ChdOpr = BaseForm.UserID;

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
                       
                        this.Close();
                    }
                    else
                    {
                        ////if (CustDetails.UpdateType == "U")
                        ////    MessageBox.Show("UnSuccessful Record Updated ", "CAP Systems", MessageBoxButtons.OK);
                        ////else
                        ////    MessageBox.Show("UnSuccessful Record Insert ", "CAP Systems", MessageBoxButtons.OK);
                    }
               // }
            }
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
                _errorProvider.SetError(CustRespGrid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Response Grid".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                label13.Visible = false;
                _errorProvider.SetError(CustRespGrid, null);
            }

           // isValid = CheckDupCustResps(false);

            return isValid;
        }

        DataGridViewRow PrivRow;
        bool CustResp_lod_complete = false;
      

        private void Clear_CustRespGrid()
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
        }

        private void CustRespGrid_SelectionChanged(object sender, EventArgs e)
        {
        //    bool CanSave = true, Delete_SW = false;

        //    //if (PrivRow == null)
        //    //    PrivRow.Tag = 0; 
        //    //if (Cust_SCR_Mode != "View")
        //    //{
        //        if (PrivRow != null && PrivRow.Index >= 0)
        //        {
        //            if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) ||
        //                string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()))
        //            {

        //                if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) &&
        //                    string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
        //                    !string.IsNullOrEmpty(PrivRow.Cells["Empty_Row"].EditedFormattedValue.ToString()))
        //                {
        //                    MessageBox.Show("This Response Will be Deleted", "CapSystems", MessageBoxButtons.OK);
        //                    Delete_SW = true;
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Please Fill 'Code' and 'Description'", "CAP Systems", MessageBoxButtons.OK); CanSave = false;
        //                }
        //            }


        //            if (CanSave && CheckDupCustResps(Delete_SW))
        //            {
        //                if (!(string.IsNullOrEmpty(PrivRow.Cells["Type"].EditedFormattedValue.ToString())))
        //                {
        //                    foreach (CustRespEntity Entity in OrgCustResp)
        //                    {
        //                        if (PrivRow.Cells["CustSeq"].Value.ToString() == Entity.RespSeq)
        //                        {
        //                            if (!Delete_SW)
        //                            {
        //                                Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
        //                                Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
        //                            }
        //                            else
        //                            {
        //                                this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);

        //                                //if (!CustRespGrid.Rows[this.CustRespGrid.SelectedRows[0].Index].IsNewRow)
        //                                CustRespGrid.Rows.RemoveAt(PrivRow.Index);
        //                                //CustRespGrid.Rows.RemoveAt(this.CustRespGrid.SelectedRows[0].Index);

        //                                this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);

        //                                Entity.RecType = "D";
        //                            }

        //                            Entity.Changed = "Y";
        //                            // PrivRow.Cells["Changed"].Value = "Y";
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {

        //                    if (!string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
        //                        !string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()))
        //                    {
        //                        PrivRow.Cells["Type"].Value = "I";
        //                        PrivRow.Cells["Empty_Row"].Value = "N";
        //                        PrivRow.Cells["CustSeq"].Value = (OrgCustResp.Count + 1).ToString();

        //                        CustRespEntity New_Entity = new CustRespEntity();
        //                        New_Entity.RecType = "I";
        //                        New_Entity.ScrCode = "HSS00133";                               
                                
        //                            if (!string.IsNullOrEmpty(TxtCode.Text.Trim()))
        //                                New_Entity.ResoCode = "C" + TxtCode.Text.Trim();
                                
        //                        New_Entity.RespSeq = (OrgCustResp.Count + 1).ToString();
        //                        New_Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
        //                        New_Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
        //                        New_Entity.AddOpr = BaseForm.UserID;
        //                        New_Entity.ChgOpr = BaseForm.UserID;
        //                        New_Entity.Changed = "Y";

        //                        OrgCustResp.Add(new CustRespEntity(New_Entity));
        //                    }
        //                }
        //            }
        //        }

        //        if (CustRespGrid.Rows.Count >= 0)
        //        {
        //            DataGridViewRow row = CustRespGrid.SelectedRows[0];
        //            PrivRow = row;
        //        }
        //    //}
        }


        private bool CheckDupCustResps(bool Delete_SW)
        {
            bool ReturnVal = true;
            if (!Delete_SW)
            {
                string TestCode, TestDesc, TmpSelCode = null, TmpSelDesc = null;
                TmpSelCode = PrivRow.Cells["RespCode"].Value.ToString();
                TmpSelDesc = PrivRow.Cells["RespDesc"].Value.ToString();
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

        string RespType = "0";
        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RespType = ((ListItem)CmbType.SelectedItem).Value.ToString();
            _errorProvider.SetError(label13, null);
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
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

        private void CustRespGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (CustRespGrid.Rows.Count > 1)
            {
                CustRespGrid.CellValueChanged -= new DataGridViewCellEventHandler(CustRespGrid_CellValueChanged);
                if (e.ColumnIndex == RespCode.Index)
                {
                    int introwindex = CustRespGrid.CurrentCell.RowIndex;
                    string TestSelCode = Convert.ToString(CustRespGrid.Rows[introwindex].Cells["RespCode"].Value);
                    string TestCode, TestDesc;
                    int i = 0;
                    foreach (DataGridViewRow dr in CustRespGrid.Rows)
                    {
                        TestCode = TestDesc = string.Empty;
                        if (!(string.IsNullOrEmpty(dr.Cells["RespCode"].FormattedValue.ToString())))
                            TestCode = dr.Cells["RespCode"].Value.ToString().Trim();
                        if (!(string.IsNullOrEmpty(dr.Cells["RespDesc"].FormattedValue.ToString())))
                            TestDesc = dr.Cells["RespDesc"].Value.ToString().Trim();
                        if (TestSelCode == TestCode)
                        {
                            i++;                           
                        }
                        if (i > 1)
                        {
                            MessageBox.Show("Response Code '" + "'" + TestSelCode + "' Already Exists", "CAP Systems", MessageBoxButtons.OK);
                            dr.Cells["RespCode"].Value = string.Empty;
                            break;
                        }                        
                    }                    
                    
                  
                }
                if (e.ColumnIndex == RespDesc.Index)
                {
                    int introwindex = CustRespGrid.CurrentCell.RowIndex;
                    string TestSelCode = Convert.ToString(CustRespGrid.Rows[introwindex].Cells["RespDesc"].Value);
                    string  TestDesc;
                    int i = 0;
                    foreach (DataGridViewRow dr in CustRespGrid.Rows)
                    {
                         TestDesc = string.Empty;                       
                        if (!(string.IsNullOrEmpty(dr.Cells["RespDesc"].FormattedValue.ToString())))
                            TestDesc = dr.Cells["RespDesc"].Value.ToString().Trim();
                        if (TestSelCode == TestDesc)
                        {
                            i++;
                        }
                        if (i > 1)
                        {
                            MessageBox.Show("Response Desc '" + "'" + TestSelCode + "' Already Exists", "CAP Systems", MessageBoxButtons.OK);
                            dr.Cells["RespDesc"].Value = string.Empty;
                            break;
                        }
                    }  

                }
                CustRespGrid.CellValueChanged += new DataGridViewCellEventHandler(CustRespGrid_CellValueChanged);
            }
        }

        private void CustCntlPanel_Click(object sender, EventArgs e)
        {

        }

        
    }
}