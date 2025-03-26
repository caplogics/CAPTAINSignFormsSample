#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.UserControls;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CAPrices_Form : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();
        private string strAgency = string.Empty;
        private string strDept = string.Empty;
        private string strProgram = string.Empty;
        private string strYear = string.Empty;
        private string strApplNo = string.Empty;
        private string strMatCode = string.Empty;
        private string strMode = string.Empty;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        private string strCaseWorkerDefaultCode = "0";
        private string strCaseWorkerDefaultStartCode = "0";
        private string strMaxDate = string.Empty;

        int strIndex = 0;

        public CAPrices_Form(BaseForm baseForm, PrivilegeEntity privilieges, string cacode, string cadesc)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            CADesc = cadesc;
            CACode = cacode;
            strMode = string.Empty;
            txtUnitPrice.Validator = TextBoxValidation.FloatValidator;
            
            CAPRICESEntity Search_Entity = new CAPRICESEntity();
            Search_Entity.Code = cacode;
            //Search_Entity.Scale_Code = "0";
            

           
            
            

            
            //LblHeader.Text = Privileges.PrivilegeName;
            this.Text = Privileges.Program + " - Service Unit Price";
            lblService.Text = "Service: " + CADesc.Trim();

            FillGridData();
            //if (Privileges.AddPriv.Equals("false"))
            //{
            //    gvwService.AllowUserToAddRows = false;
            //}
            //else
            //{
            //    gvwService.AllowUserToAddRows = true;
            //}

            if (Privileges.ChangePriv.Equals("false"))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvwService.Columns["Delete"].Visible = false;
                // gvwAssessmentDetails.Columns[gvwAssessmentDetails.ColumnCount - 2].Width = 200;
            }
            else
            {
                gvwService.Columns["Delete"].Visible = true;

            }
            btnSave.Text = "&Save";

        }

        
        #region properties
        public BaseForm BaseForm { get; set; }
        public string CACode { get; set; }
        public string MatAgy { get; set; }
        public string MatDept { get; set; }
        public string MatProg { get; set; }
        public string MatYear { get; set; }
        public string CADesc { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        public List<MATDEFDTEntity> Sel_Date_List { get; set; }
        public List<CAPRICESEntity> propCAPricesEntity { get; set; }
        public List<MATDEFBMEntity> MATADEFBMentity { get; set; }
        public string Type { get; set; }
        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }



        private void btnSave_Click(object sender, EventArgs e)
        {

            if (ValidateForm())
            {
                string strMsg = string.Empty;
                CAPRICESEntity Entity = new CAPRICESEntity();
                Entity.Code = CACode;
                Entity.FDate = dtAssessmentDate.Value.ToShortDateString();
                Entity.TDate = dtpEndDate.Value.ToShortDateString();
                Entity.UnitPrice = txtUnitPrice.Text.Trim();

                Entity.lstcOperator = BaseForm.UserID;
                if (lblAddEdit.Text.Trim()=="Add") strMode = "I";

                if(strMode!="I")
                    Entity.CAP_ID= gvwService.SelectedRows[0].Cells["CAP_ID"].Value.ToString();

                Entity.Mode = strMode;
                //txtSub.Text;                
                bool boolSucess = _model.SPAdminData.InsertCAPrices(Entity, out strMsg);
                if (strMsg == "Exists")
                {
                    CommonFunctions.MessageBoxDisplay(Consts.Messages.Datealreadyexists);
                }
                if (boolSucess == true)
                {
                    FillGridData();
                    if (gvwService.Rows.Count != 0)
                    {
                        gvwService.Rows[strIndex].Selected = true;
                        gvwService.CurrentCell = gvwService.Rows[strIndex].Cells[1];

                    }
                    gvwAssessmentDetails_SelectionChanged(sender, e);
                }

            }


        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (dtAssessmentDate.Checked == false)
            {
                _errorProvider.SetError(dtAssessmentDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtAssessmentDate, null);
            }

            if (dtpEndDate.Checked == false)
            {
                _errorProvider.SetError(dtpEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEndDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpEndDate, null);
            }


            //if (strMode == "I" )
            //{
            if (dtAssessmentDate.Checked.Equals(true) && dtpEndDate.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtAssessmentDate.Text) && (!string.IsNullOrEmpty(dtpEndDate.Text)))
                {
                    if (Convert.ToDateTime(dtAssessmentDate.Text) >= Convert.ToDateTime(dtpEndDate.Text))
                    {
                        _errorProvider.SetError(dtpEndDate, "It should be greater than start date ".Replace(Consts.Common.Colon, string.Empty));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpEndDate, null);
                    }

                    if (propCAPricesEntity.Count > 0)
                    {
                        foreach (CAPRICESEntity Entity in propCAPricesEntity)
                        {
                            if ((strMode == "U" && Entity.CAP_ID != gvwService.SelectedRows[0].Cells["CAP_ID"].Value.ToString()) || strMode == "I")
                            {
                                if (((Convert.ToDateTime(Entity.FDate) >= Convert.ToDateTime(dtAssessmentDate.Value) && Convert.ToDateTime(Entity.FDate) <= Convert.ToDateTime(Entity.FDate))
                                && (Convert.ToDateTime(Entity.FDate) >= Convert.ToDateTime(Entity.FDate) && Convert.ToDateTime(Entity.FDate) <= Convert.ToDateTime(dtpEndDate.Value))) ||
                                ((Convert.ToDateTime(Entity.TDate) >= Convert.ToDateTime(dtAssessmentDate.Value) && Convert.ToDateTime(Entity.TDate) <= Convert.ToDateTime(Entity.TDate))
                                && (Convert.ToDateTime(Entity.TDate) >= Convert.ToDateTime(Entity.TDate) && Convert.ToDateTime(Entity.TDate) <= Convert.ToDateTime(dtpEndDate.Value))))
                                {
                                    _errorProvider.SetError(dtpEndDate, "Range already exists".Replace(Consts.Common.Colon, string.Empty));
                                    isValid = false;
                                }
                            }
                        }
                    }
                }
            }
            //}



            if (string.IsNullOrEmpty(txtUnitPrice.Text.Trim()))
            {
                _errorProvider.SetError(txtUnitPrice, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTot.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtUnitPrice, null);
            }



            //if ((cmbCaseWorker.SelectedItem == null || ((ListItem)cmbCaseWorker.SelectedItem).Text == Consts.Common.SelectOne))
            //{
            //    _errorProvider.SetError(cmbCaseWorker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseWorker.Text));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(cmbCaseWorker, null);
            //}


            return (isValid);
        }

        private void FillGridData()
        {
            
            this.gvwService.SelectionChanged -= new System.EventHandler(this.gvwAssessmentDetails_SelectionChanged);
            gvwService.Rows.Clear();
            propCAPricesEntity = _model.SPAdminData.Browse_CAPrices(CACode, string.Empty, string.Empty);
            if (propCAPricesEntity.Count>0)
            {
                foreach(CAPRICESEntity Entity in propCAPricesEntity)
                {
                    string strFDate = LookupDataAccess.Getdate(Entity.FDate);
                    string strTDaate = LookupDataAccess.Getdate(Entity.TDate);

                    int rowIndex = gvwService.Rows.Add(strFDate, strTDaate, Entity.UnitPrice.Trim(),Entity.CAP_ID);
                    gvwService.Rows[rowIndex].Tag = Entity;
                    //gvwService.ItemsPerPage = 100;
                }
            }
            this.gvwService.SelectionChanged += new System.EventHandler(this.gvwAssessmentDetails_SelectionChanged);
            if (gvwService.Rows.Count > 0)
            {
               // gvwService.Rows[0].Tag = 0;
                gvwService.Rows[0].Selected = true;
            }
            gvwAssessmentDetails_SelectionChanged(gvwService, EventArgs.Empty);
            
                


        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void gvwAssessmentDetails_SelectionChanged(object sender, EventArgs e)
        {
            if(gvwService.Rows.Count>1)
            {
                if (gvwService.SelectedRows[0].Tag is CAPRICESEntity)
                {
                    CAPRICESEntity row = gvwService.SelectedRows[0].Tag as CAPRICESEntity;
                    strIndex = gvwService.SelectedRows[0].Index;
                    if (row != null)
                    {
                        dtAssessmentDate.Checked = true; dtAssessmentDate.Text = row.FDate;
                        dtpEndDate.Checked = true;dtpEndDate.Text = row.TDate;
                        txtUnitPrice.Text = row.UnitPrice.Trim();
                        lblAddEdit.Text = "Edit";

                        strMode = "U";

                        EnableAllcontrols();
                    }
                    else
                        EnableAllcontrols(true);
                }
                else
                    EnableAllcontrols(true);
            }
            else
            {
                EnableAllcontrols(true);
            }
        }


        private void cmbCaseWorker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void EnableAllcontrols()
        {

            //cmbCaseWorker.Enabled = false;
            dtAssessmentDate.Enabled = true; dtpEndDate.Enabled = true;
            
            //btnSave.Visible = false;
            btnCancel.Text = "&Close";
        }

        private void EnableAllcontrols(bool booltrue)
        {
            //cmbCaseWorker.Enabled = booltrue;
            dtAssessmentDate.Enabled = booltrue; dtpEndDate.Enabled = true;
            btnSave.Visible = booltrue; txtUnitPrice.Text = string.Empty;
            btnCancel.Text = "&Close";
            strMode = "I";
            lblAddEdit.Text = "Add";
        }

        private void gvwAssessmentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == Delete.Index && e.RowIndex != -1)
            {
                if (gvwService.SelectedRows.Count > 0)
                {
                    if (Type == "Matrix")
                    {
                        if (gvwService.Rows.Count > 1)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);
                        }

                    }
                    else
                    {
                        if (gvwService.SelectedRows[0].Tag is CAPRICESEntity)
                        {
                            CAPRICESEntity row = gvwService.SelectedRows[0].Tag as CAPRICESEntity;
                            if (row != null)
                            {
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);

                            }
                        }
                    }
                }
            }
        }


        private void OnDeleteMessageBoxClicked(DialogResult dialogresult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogresult == DialogResult.Yes)
            {
                if(gvwService.Rows.Count>0)
                {
                    
                    string strMsg = string.Empty;
                    CAPRICESEntity Entity = new CAPRICESEntity();
                    Entity.CAP_ID= gvwService.SelectedRows[0].Cells["CAP_ID"].Value.ToString();
                    Entity.Code = CACode;
                    Entity.FDate= gvwService.SelectedRows[0].Cells["FDate"].Value.ToString();
                    Entity.TDate = gvwService.SelectedRows[0].Cells["LDate"].Value.ToString();
                    Entity.UnitPrice = gvwService.SelectedRows[0].Cells["Unit_Price"].Value.ToString();
                    Entity.Mode = "D";

                    bool boolSucess = _model.SPAdminData.InsertCAPrices(Entity, out strMsg);
                    if (strMsg == "Exists")
                    {
                        CommonFunctions.MessageBoxDisplay("Applicant Assessment date can't be deleted");
                    }
                    if (boolSucess == true)
                    {

                        if (strIndex != 0)
                            strIndex = strIndex - 1;
                        FillGridData();
                        if (gvwService.Rows.Count != 0)
                        {
                            gvwService.Rows[strIndex].Selected = true;
                            gvwService.CurrentCell = gvwService.Rows[strIndex].Cells[1];

                        }
                        gvwAssessmentDetails_SelectionChanged(gvwService,EventArgs.Empty);

                    }
                }
            }
        }

        private void MAT00003AssessmentDate_Load(object sender, EventArgs e)
        {
           
        }

       
        public List<MATDEFDTEntity> GetSelected_MatDates()
        {
            List<MATDEFDTEntity> Sel_Date_List = new List<MATDEFDTEntity>();
            MATDEFDTEntity Add_Entity = new MATDEFDTEntity();
            string Datelist = string.Empty;
            if (gvwService.Rows.Count > 1)
            {
                foreach (DataGridViewRow dr in gvwService.Rows)
                {
                    string strvalue = dr.Cells["date"].Value == null ? string.Empty : dr.Cells["date"].Value.ToString().Trim();
                    //Add_Entity.Rec_Type = "I";

                    if (!string.IsNullOrEmpty(strvalue))
                    {
                        Add_Entity.MatCode = dr.Cells["CaseWorker"].Value.ToString();
                        Add_Entity.MatDate = dr.Cells["Date"].Value.ToString();
                        Sel_Date_List.Add(new MATDEFDTEntity(Add_Entity));
                        Datelist += dr.Cells["Date"].Value.ToString();
                    }
                }
            }
            return Sel_Date_List;
        }
        bool First = true;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (dtAssessmentDate.Checked == true)
            {
                string strdate = LookupDataAccess.Getdate(dtAssessmentDate.Text);
                bool boolValue = true;
                foreach (DataGridViewRow dr in gvwService.Rows)
                {
                    string strvalue = dr.Cells["date"].Value == null ? string.Empty : dr.Cells["date"].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(strvalue))
                    {
                        if (dr.Cells["Date"].Value.ToString() == strdate)
                        {
                            MessageBox.Show("Already added", "CAPTAIN");
                            boolValue = false;
                        }
                    }
                }
                if (boolValue)
                {
                    gvwService.Rows.Add(strdate, strMatCode);
                    First = false;
                }

            }
            dtAssessmentDate.Checked = false;
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }
    }
}