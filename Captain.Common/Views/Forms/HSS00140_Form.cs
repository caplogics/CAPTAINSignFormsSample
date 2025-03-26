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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00140_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        public HSS00140_Form(BaseForm baseform, string mode,string ChldBM_Number,string Route, string AppNo, string FName, string LName, string Phone,  PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Mode = mode;
            App_No = AppNo; First_Name = FName;
            Last_Name = LName;Number=ChldBM_Number; Route_Id=Route;
            TelePhone = Phone; //Route_Id = Route; Year = Route_Year;
            Privileges = privileges;
            txtAppNo.Validator = TextBoxValidation.IntegerValidator;
            FormLoad();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string App_No { get; set; }

        public string Year { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string TelePhone { get; set; }

        public string Route_Id { get; set; }

        public string Number { get; set; }

        public string strHiearchy { get; set; }

        public List<CaseSnpEntity> CaseSnpAllList { get; set; }

        public List<CaseMstEntity> CaseMstAllList { get; set; }

        public CaseMstEntity CaseMST { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion
        List<CaseMstSnpEntity> AppList = new List<CaseMstSnpEntity>();

        private void FormLoad()
        {
            this.Text = /*Privileges.Program*/"Bus Client Placement" + " - " + Mode;
            if (Mode == "Edit")
            {
                btnAppSearch.Enabled = false;
                txtAppNo.Enabled = false;
                if (!string.IsNullOrEmpty(TelePhone))
                {
                    string[] phonebreak = Regex.Split(TelePhone, "-");
                    TelePhone = phonebreak[0] + phonebreak[1] + phonebreak[2];
                }
                FillBusAppControls();
            }
            else
            {
                btnAppSearch.Enabled = true; txtAppNo.Enabled = true;
            }
            //AppList = _model.CaseMstData.GetSSNSearch("APP", null, null, null, null, null, null, null, null, null, "N", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            //if (CaseSnpEntityList != null)
            //{
            //    foreach (CaseMstSnpEntity CaseSnp in CaseSnpEntityList)
            //    {
            //        if (CaseSnp.Agency == BaseForm.BaseAgency.Trim() && CaseSnp.Dept == BaseForm.BaseDept && CaseSnp.Program == BaseForm.BaseProg && (CaseSnp.Year == BaseForm.BaseYear || CaseSnp.Year == BaseForm.BaseYear.Trim()))
            //        {
            //            AppList.Add(CaseSnp);
            //        }
            //    }
            //}
        }

        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSS00140");
        }

        private void btnAppSearch_Click(object sender, EventArgs e)
        {
            SSNSearchForm SSNSearchForm = new SSNSearchForm(BaseForm, "HSS00140", CaseSnpAllList, ProgramDefinition, CaseMST,"H","APP");
            SSNSearchForm.FormClosed += new FormClosedEventHandler(OnSearchFormClosed);
            SSNSearchForm.StartPosition = FormStartPosition.CenterScreen;
            SSNSearchForm.ShowDialog();
        }

        private void OnSearchFormClosed(object sender, FormClosedEventArgs e)
        {
            SSNSearchForm form = sender as SSNSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                CaseMstSnpEntity selectedSsn = form.GetSelectedRow();
                if (selectedSsn != null)
                {
                    List<BUSCEntity> App_list;
                    BUSCEntity SearchEntity = new BUSCEntity(true);
                    SearchEntity.BUSC_AGENCY = BaseForm.BaseAgency.Trim(); SearchEntity.BUSC_DEPT = BaseForm.BaseDept.Trim();
                    SearchEntity.BUSC_PROG = BaseForm.BaseProg.Trim(); SearchEntity.BUSC_NUMBER = Number.Trim();
                    SearchEntity.BUSC_ROUTE = Route_Id.Trim();
                    SearchEntity.BUSC_YEAR = BaseForm.BaseYear; SearchEntity.BUSC_CHILD = selectedSsn.ApplNo.Substring(0, 8);
                    App_list = _model.SPAdminData.Browse_ChldBUSC(SearchEntity, "Browse");
                    List<CaseEnrlEntity> caseEnrlList = _model.CaseSumData.GetCaseEnrlDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, selectedSsn.ApplNo.Substring(0, 8), string.Empty, string.Empty);
                    caseEnrlList = caseEnrlList.FindAll(u => u.Status.Equals("E"));
                    if (App_list.Count > 0)
                    {
                        AlertBox.Show("Client already exists", MessageBoxIcon.Warning);
                    }
                    else
                    {
                        txtAppNo.Text = selectedSsn.ApplNo.Substring(0, 8);
                        txtFName.Text = selectedSsn.NameixFi.ToString().Trim();
                        txtLName.Text = selectedSsn.NameixLast.ToString().Trim();
                        maskedTextBox1.Text = selectedSsn.Area+selectedSsn.Phone;
                        if (caseEnrlList.Count == 0) AlertBox.Show("The Child you are placing on this Route does not have a current Enroll Status", MessageBoxIcon.Warning);
                        this.InvokeFocusCommand(dtpPickUp);
                    }

                }
            }
        }

        
        private void FillBusAppControls()
        {
            List<BUSCEntity> App_list;
            BUSCEntity SearchEntity = new BUSCEntity(true);
            SearchEntity.BUSC_AGENCY = BaseForm.BaseAgency.Trim(); SearchEntity.BUSC_DEPT = BaseForm.BaseDept.Trim();
            SearchEntity.BUSC_PROG = BaseForm.BaseProg.Trim(); SearchEntity.BUSC_NUMBER = Number.Trim();
            SearchEntity.BUSC_ROUTE = Route_Id.Trim(); 
            SearchEntity.BUSC_YEAR = BaseForm.BaseYear; SearchEntity.BUSC_CHILD=App_No.Trim();
            App_list = _model.SPAdminData.Browse_ChldBUSC(SearchEntity, "Browse");

            if (App_list.Count > 0)
            {
                BUSCEntity Entity = App_list[0];
                txtAppNo.Text = Entity.BUSC_CHILD.Trim();
                txtFName.Text = First_Name.Trim();
                txtLName.Text = Last_Name.Trim();
                maskedTextBox1.Text = TelePhone.Trim();
                if (!string.IsNullOrEmpty(Entity.BUSC_PICKUP.Trim()))
                {
                    dtpPickUp.Text = Entity.BUSC_PICKUP.ToString();
                    dtpPickUp.Checked = true;
                }
                if (!string.IsNullOrEmpty(Entity.BUSC_HOME.Trim()))
                {
                    dtpDropoff.Text = Entity.BUSC_HOME.ToString();
                    dtpDropoff.Checked = true;
                }
                txtComments.Text = Entity.BUSC_COMMENTS.Trim();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CaptainModel Model = new CaptainModel();
                    string SqlMsg = string.Empty;
                    string msg = string.Empty;

                    BUSCEntity Entity = new BUSCEntity();
                    if (Mode == "Edit")
                    {
                        Entity.row_Type = "U";
                        //Entity.ChldBM_Count = BM_Count.Trim();
                    }
                    else
                    {
                        Entity.row_Type = "I";
                        //Entity.ChldBM_Count = "0";
                    }
                    Entity.BUSC_AGENCY = BaseForm.BaseAgency.Trim();
                    Entity.BUSC_DEPT = BaseForm.BaseDept.Trim();
                    Entity.BUSC_PROG = BaseForm.BaseProg.Trim();
                    Entity.BUSC_NUMBER = Number.Trim();
                    //if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                    //    Baseyear = "    ";
                    //else Baseyear = BaseForm.BaseYear.Trim();
                    Entity.BUSC_YEAR = BaseForm.BaseYear;
                    Entity.BUSC_ROUTE = Route_Id;
                    Entity.BUSC_CHILD = txtAppNo.Text;
                    if (dtpPickUp.Checked.Equals(true))
                        Entity.BUSC_PICKUP = dtpPickUp.Value.ToString("HH:mm:ss");
                    if (dtpDropoff.Checked.Equals(true))
                        Entity.BUSC_HOME = dtpDropoff.Value.ToString("HH:mm:ss");
                    //Entity.BUSC_APPLICATION = txtArea.Text.Trim();
                    Entity.BUSC_COMMENTS = txtComments.Text.Trim();
                    Entity.AddOperator = BaseForm.UserID;
                    Entity.LstcOperator = BaseForm.UserID;

                    _model.SPAdminData.UpdateChldBusC(Entity, "Update", out msg, out SqlMsg);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            { }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtAppNo.Text.Trim()))
            {
                _errorProvider.SetError(btnAppSearch, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAppNo.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(btnAppSearch, null);

            if (dtpPickUp.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpPickUp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPickup.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(dtpPickUp, null);

            if (dtpDropoff.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpDropoff, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDropoff.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(dtpDropoff, null);

            if (dtpPickUp.Checked.Equals(true) && dtpDropoff.Checked.Equals(true))
            {
                if (Convert.ToDateTime(LookupDataAccess.GetTime(dtpPickUp.Value.ToString())) > Convert.ToDateTime(LookupDataAccess.GetTime(dtpDropoff.Value.ToString())))
                {
                    _errorProvider.SetError(dtpPickUp, string.Format("Pickup time Should not be Prior to Dropoff time" .Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(dtpPickUp, null);
            }

            IsSaveValid = isValid;
            return (isValid);
        }

        public string[] GetSelected_App()
        {
            string[] Added_Edited_App = new string[3];//[4];

            //**Added_Edited_App[0] = Number;
            //**Added_Edited_App[1] = Route_Id;
            Added_Edited_App[0/*2*/] = txtAppNo.Text.Trim();
            Added_Edited_App[1/*3*/] = Mode;

            return Added_Edited_App;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAppNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAppNo.Text.Trim()))
            {
                txtAppNo.Text = SetLeadingZeros(txtAppNo.Text);
                List<CaseMstEntity> AppList = _model.CaseMstData.GetCaseMstAll(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, txtAppNo.Text, null, null, null, null, "MSTALLSNP");
                List<CaseEnrlEntity> caseEnrlList = _model.CaseSumData.GetCaseEnrlDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, txtAppNo.Text, string.Empty, string.Empty);
                caseEnrlList = caseEnrlList.FindAll(u => u.Status.Equals("E"));
                if (AppList.Count>0)
                {
                    List<CaseMstEntity> SelApplist = AppList.FindAll(u => u.ApplNo.Substring(0, 8).Trim().Equals(txtAppNo.Text.Trim()));
                    if (SelApplist.Count > 0)
                    {
                        List<BUSCEntity> App_list;
                        BUSCEntity SearchEntity = new BUSCEntity(true);
                        SearchEntity.BUSC_AGENCY = BaseForm.BaseAgency.Trim(); SearchEntity.BUSC_DEPT = BaseForm.BaseDept.Trim();
                        SearchEntity.BUSC_PROG = BaseForm.BaseProg.Trim(); SearchEntity.BUSC_NUMBER = Number.Trim();
                        SearchEntity.BUSC_ROUTE = Route_Id.Trim();
                        SearchEntity.BUSC_YEAR = BaseForm.BaseYear; SearchEntity.BUSC_CHILD = SelApplist[0].ApplNo.Substring(0, 8);
                        App_list = _model.SPAdminData.Browse_ChldBUSC(SearchEntity, "Browse");
                        if (App_list.Count > 0)
                        {
                            AlertBox.Show("Client already exists", MessageBoxIcon.Warning);
                        }
                        else
                        {
                            //txtAppNo.Text = SelApplist[0].ApplNo.Substring(0, 8);
                            txtFName.Text = SelApplist[0].FirstName.ToString().Trim();
                            txtLName.Text = SelApplist[0].LastName.ToString().Trim();
                            maskedTextBox1.Text = SelApplist[0].Area + SelApplist[0].Phone;
                            if (caseEnrlList.Count == 0) AlertBox.Show("The Child you are placing on this Route does not have a current Enroll Status", MessageBoxIcon.Warning);
                            this.InvokeFocusCommand(dtpPickUp);
                        }
                    }
                    else
                    {
                        txtAppNo.Clear(); txtFName.Clear(); txtLName.Clear(); maskedTextBox1.Clear();
                        AlertBox.Show("Applicant does not exist.", MessageBoxIcon.Warning);
                        this.InvokeFocusCommand(txtAppNo);
                    }
                }
                else
                {
                    txtAppNo.Clear(); txtFName.Clear(); txtLName.Clear(); maskedTextBox1.Clear();
                    AlertBox.Show("Applicant does not exist.", MessageBoxIcon.Warning);
                    this.InvokeFocusCommand(txtAppNo);
                }
                
                //if (AppList.Count > 0)
                //{
                //    List<CaseMstSnpEntity> SelApplist = AppList.FindAll(u => u.ApplNo.Substring(0,8).Trim().Equals(txtAppNo.Text.Trim()));
                //    if (SelApplist.Count > 0)
                //    {
                //        List<BUSCEntity> App_list;
                //        BUSCEntity SearchEntity = new BUSCEntity(true);
                //        SearchEntity.BUSC_AGENCY = BaseForm.BaseAgency.Trim(); SearchEntity.BUSC_DEPT = BaseForm.BaseDept.Trim();
                //        SearchEntity.BUSC_PROG = BaseForm.BaseProg.Trim(); SearchEntity.BUSC_NUMBER = Number.Trim();
                //        SearchEntity.BUSC_ROUTE = Route_Id.Trim();
                //        SearchEntity.BUSC_YEAR = BaseForm.BaseYear; SearchEntity.BUSC_CHILD = SelApplist[0].ApplNo.Substring(0, 8);
                //        App_list = _model.SPAdminData.Browse_ChldBUSC(SearchEntity, "Browse");
                //        if (App_list.Count > 0)
                //        {
                //            MessageBox.Show("Client Already Exists", "CAP Systems");
                //        }
                //        else
                //        {
                //            //txtAppNo.Text = SelApplist[0].ApplNo.Substring(0, 8);
                //            txtFName.Text = SelApplist[0].NameixFi.ToString().Trim();
                //            txtLName.Text = SelApplist[0].NameixLast.ToString().Trim();
                //            maskedTextBox1.Text = SelApplist[0].Area + SelApplist[0].Phone;
                //            this.InvokeFocusCommand(dtpPickUp);
                //        }
                //    }
                //    else
                //    {
                //        txtAppNo.Clear(); txtFName.Clear(); txtLName.Clear(); maskedTextBox1.Clear();
                //        MessageBox.Show("Applicant Does not exist", "CAP Systems");
                //        this.InvokeFocusCommand(txtAppNo);
                //    }
                //}
            }
        }


        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }
            return (TmpCode);
        }

        private void InvokeFocusCommand(Control objControl)
        {
            //Commented by Vikash 03/10/2023

            //IApplicationContext objApplicationContext = this.Context as IApplicationContext;
            //if (objApplicationContext != null)
            //{
            //    objApplicationContext.SetFocused(objControl, true);
            //}
        }
    }
}