#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class FuelControl : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        public string strTaskCount = string.Empty;
        #endregion
        public FuelControl(BaseForm baseform, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            //this.Text = privileges.Program + " - Funding Selection";
            BaseForm = baseform;
            string Module = privileges.ModuleCode.Trim();
            Privileges = privileges;
            //List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(code, BaseForm.UserID, string.Empty);
            List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BaseForm.BusinessModuleID, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
            Privileges = userPrivilege.Find(u => u.ModuleCode.Equals(Module) && u.Program.Equals(privileges.Program));

            this.Text = "Fuel Control";
            txtIncome.Validator = TextBoxValidation.FloatValidator;
            txtL1_Vul.Validator = TextBoxValidation.FloatValidator;
            txtL1_Nonvul.Validator = TextBoxValidation.FloatValidator;
            Txt_Ass_O.Validator = TextBoxValidation.FloatValidator;
            Txt_Ass_R.Validator = TextBoxValidation.FloatValidator;


            YearCombo();
            if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
            {
                cmbYear.Visible = true; lblYear.Visible = true; //btnCopy.Visible = true;

                VisibilityCopyButton();
            }
            else { cmbYear.Visible = false; lblYear.Visible = false; btnCopy.Visible = false; }

            if (Privileges.AddPriv.Equals("false") && Privileges.ChangePriv.Equals("false"))
            {
                btnSave.Visible = false; btnCopy.Visible = false;
            }
            else if (Privileges.ChangePriv.Equals("false"))
            {
                btnCopy.Visible = false;
            }


            FillControls();
        }

        #region properties
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public List<FUELCNTLEntity> Fuelcntl { get; set; }

        #endregion

        private void YearCombo()
        {
            DataSet ds = DatabaseLayer.FuelControlDB.Browse_State_MedianInc_Guide();
            DataTable dt = new DataTable();
            if (ds != null)
                dt = ds.Tables[0];

            DataView dv = new DataView(dt);
            dv.RowFilter = "SMG_YEAR <>'CHAP'";
            dv.Sort = "SMG_YEAR DESC";
            dt = dv.ToTable();

            cmbYear.Items.Clear();
            this.cmbYear.SelectedIndexChanged -= new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            List<ListItem> listItem = new List<ListItem>();
            if (int.Parse(DateTime.Now.Month.ToString()) > 6)
                listItem.Add(new ListItem(DateTime.Now.AddYears(+1).Year.ToString(), DateTime.Now.AddYears(+1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-3).Year.ToString(), DateTime.Now.AddYears(-3).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-4).Year.ToString(), DateTime.Now.AddYears(-4).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-5).Year.ToString(), DateTime.Now.AddYears(-5).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-6).Year.ToString(), DateTime.Now.AddYears(-6).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-7).Year.ToString(), DateTime.Now.AddYears(-7).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-8).Year.ToString(), DateTime.Now.AddYears(-8).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-9).Year.ToString(), DateTime.Now.AddYears(-9).Year.ToString()));
            cmbYear.Items.AddRange(listItem.ToArray());
            //cmbYear.SelectedIndex = 0;
            CommonFunctions.SetComboBoxValue(cmbYear, dt.Rows[0]["SMG_YEAR"].ToString().Trim());
            //CommonFunctions.SetComboBoxValue(cmbYear, BaseForm.BaseYear);

            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);

        }

        private void txtIncome_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIncome.Text.Trim()))
            {
                _errorProvider.SetError(txtIncome, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIncome.Text.Replace(Consts.Common.Colon, string.Empty)));
            }
            else if (!string.IsNullOrEmpty(txtIncome.Text.Trim()))
            {
               decimal incomvalue= Convert.ToDecimal(txtIncome.Text.Trim());

                if (txtIncome.Text.Trim().Length > 6)
                {
                    if (!txtIncome.Text.Contains("."))
                        txtIncome.Text = txtIncome.Text.Substring(0, 6);
                }
                _errorProvider.SetError(txtIncome, null);
                MessageBox.Show("You want to fill the SMI Guidelines automatically?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: FillAutomaticValues);
            }
        }
        string ModeChange = string.Empty; string Priv_Income = string.Empty;
        public void FillAutomaticValues(DialogResult dialogresult)
        {

            string IncomeValue = float.Parse(txtIncome.Text.Trim()).ToString("0.00");


            gvIncome.Enabled = true;

            ModeChange = "Values";
            this.Col1.ReadOnly = false; this.Col2.ReadOnly = false; this.Col3.ReadOnly = false; this.Col4.ReadOnly = false;
            this.Col5.ReadOnly = false; this.Col6.ReadOnly = false; this.Col7.ReadOnly = false; this.Col8.ReadOnly = false;
            if (dialogresult == DialogResult.Yes) //(senderform.DialogResult.ToString() == "Yes")
            {
                gvIncome.Rows.Clear();
                //ModeChange = string.Empty;
                //this.Col1.ReadOnly = true; this.Col2.ReadOnly = true; this.Col3.ReadOnly = true; this.Col4.ReadOnly = true;
                //this.Col5.ReadOnly = true; this.Col6.ReadOnly = true; this.Col7.ReadOnly = true; this.Col8.ReadOnly = true;
                int rowIndex = gvIncome.Rows.Add("Income", (float.Parse(IncomeValue) * 0.52f).ToString("0.00"), (float.Parse(IncomeValue) * 0.68f).ToString("0.00"), (float.Parse(IncomeValue) * 0.84f).ToString("0.00"),
                    (float.Parse(IncomeValue)).ToString("0.00"), (float.Parse(IncomeValue) * 1.16f).ToString("0.00"), (float.Parse(IncomeValue) * 1.32f).ToString("0.00"),
                    (float.Parse(IncomeValue) * 1.35f).ToString("0.00"), (float.Parse(IncomeValue) * 1.38f).ToString("0.00"));

                Priv_Income = IncomeValue;
            }
            else
            {
                if (gvIncome.Rows.Count > 0)
                { }
                else gvIncome.Rows.Add("Income", "0.00", "0.00", "0.00", (float.Parse(IncomeValue)).ToString("0.00"), "0.00", "0.00", "0.00", "0.00");

                //if (IncomeValue == Priv_Income)
                //{
                //    gvIncome.Rows.Clear();
                //    ModeChange = string.Empty;
                //    int rowIndex = gvIncome.Rows.Add("Income", (float.Parse(IncomeValue) * 0.52f).ToString("0.00"), (float.Parse(IncomeValue) * 0.68f).ToString("0.00"), (float.Parse(IncomeValue) * 0.84f).ToString("0.00"),
                //        (float.Parse(IncomeValue)).ToString("0.00"), (float.Parse(IncomeValue) * 1.16f).ToString("0.00"), (float.Parse(IncomeValue) * 1.32f).ToString("0.00"),
                //        (float.Parse(IncomeValue) * 1.35f).ToString("0.00"), (float.Parse(IncomeValue) * 1.38f).ToString("0.00"));
                //}
                //else
                //{
                //    ModeChange = "Values";
                //    //int rowIndex = gvIncome.Rows.Add("Income", (float.Parse(Priv_Income) * 0.52f).ToString("0.00"), (float.Parse(Priv_Income) * 0.68f).ToString("0.00"), (float.Parse(Priv_Income) * 0.84f).ToString("0.00"),
                //    //    (float.Parse(Priv_Income)).ToString("0.00"), (float.Parse(Priv_Income) * 1.16f).ToString("0.00"), (float.Parse(Priv_Income) * 1.32f).ToString("0.00"),
                //    //    (float.Parse(Priv_Income) * 1.35f).ToString("0.00"), (float.Parse(Priv_Income) * 1.38f).ToString("0.00"));
                //}

            }

            if (gvIncome.Rows.Count > 0)
            {
                gvIncome.Rows[0].Selected = true;
            }

        }

        private void VisibilityCopyButton()
        {
            if ((int.Parse(DateTime.Now.Month.ToString()) == 7 || int.Parse(DateTime.Now.Month.ToString()) == 8 || int.Parse(DateTime.Now.Month.ToString()) == 9) && ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim() == DateTime.Now.Year.ToString().Trim())
            {
                btnCopy.Visible = true;
                if (Privileges.AddPriv.Equals("false")) { btnSave.Visible = false; btnCopy.Visible = false; }
                else if (Privileges.AddPriv.Equals("true")) { btnSave.Visible = true; btnCopy.Visible = true; }
            }
            else btnCopy.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidateForm())
            {
                if (IsValidateGrid())
                {
                    if (gvIncome.Rows.Count > 0)
                    {
                        string Mode = "I";
                        DataSet ds = DatabaseLayer.FuelControlDB.Browse_State_MedianInc_Guide();
                        DataTable dt = new DataTable();
                        if (ds != null)
                            dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dgr in dt.Rows)
                            {
                                if (dgr["SMG_Year"].ToString().Trim() == ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                                    Mode = "U";
                            }
                        }

                        foreach (DataGridViewRow dr in gvIncome.Rows)
                        {
                            DatabaseLayer.FuelControlDB.UpdateState_MedianInc_Guide(Mode, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), txtIncome.Text.ToString().Trim(),
                                dr.Cells["Col1"].Value.ToString().Trim(), dr.Cells["Col2"].Value.ToString().Trim(), dr.Cells["Col3"].Value.ToString().Trim(),
                                dr.Cells["Col4"].Value.ToString().Trim(), dr.Cells["Col5"].Value.ToString().Trim(), dr.Cells["Col6"].Value.ToString().Trim(),
                                dr.Cells["Col7"].Value.ToString().Trim(), dr.Cells["Col8"].Value.ToString().Trim(), BaseForm.UserID);
                        }

                        //DatabaseLayer.FuelControlDB.UpdateState_MedianInc_Guide("U", "CHAP", txtChap.Text.Trim(), "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", BaseForm.UserID);
                    }

                    string RowType = "I";
                    DataSet dsFuel = DatabaseLayer.FuelControlDB.Browse_FuelCntl(((ListItem)cmbYear.SelectedItem).Value.ToString().Trim());
                    DataTable dtFuel = new DataTable();
                    if (dsFuel != null)
                        dtFuel = dsFuel.Tables[0];
                    if (dtFuel.Rows.Count > 0)
                    {
                        RowType = "U";

                    }
                    if (dtpAEnd.Checked == true && dtpAStart.Checked == true)
                        DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "D", "A", dtpAStart.Text.Trim(), dtpAEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                    if (dtpBEnd.Checked == true && dtpBFrom.Checked == true)
                        DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "D", "B", dtpBFrom.Text.Trim(), dtpBEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                    if (dtpCEnd.Checked == true && dtpCStart.Checked == true)
                        DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "D", "C", dtpCStart.Text.Trim(), dtpCEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                    if (dtpSEnd.Checked == true && dtpSStart.Checked == true)
                        DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "D", "S", dtpSStart.Text.Trim(), dtpSEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                    if (!string.IsNullOrEmpty(txtL1_Vul.Text.Trim()) && !string.IsNullOrEmpty(txtL1_Nonvul.Text.Trim()))
                    {
                        DataView dv = new DataView(dtFuel);
                        dv.RowFilter = "FCNTL_BEN_TYPE='A'";
                        dtFuel = dv.ToTable();
                        if (dtFuel.Rows.Count > 0)
                            DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "A", "98", string.Empty, string.Empty, txtL1_Vul.Text.Trim(), txtL1_Nonvul.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                        else
                            DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "A", "98", string.Empty, string.Empty, txtL1_Vul.Text.Trim(), txtL1_Nonvul.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                    }

                    //Overrage Save routine
                    if (!string.IsNullOrEmpty(Txt_Ass_O.Text.Trim()) && !string.IsNullOrEmpty(Txt_Ass_R.Text.Trim()))
                    {
                        DataTable dtFuel_Ovr = new DataTable();
                        if (dsFuel != null)
                        {
                            dtFuel_Ovr = dsFuel.Tables[0];

                            DataView dv = new DataView(dtFuel_Ovr);
                            dv.RowFilter = "FCNTL_BEN_TYPE='O'";
                            dtFuel_Ovr = dv.ToTable();
                            if (dtFuel_Ovr.Rows.Count > 0)
                                DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "O", "O", string.Empty, string.Empty, Txt_Ass_O.Text.Trim(), Txt_Ass_R.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                            else
                                DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "O", "O", string.Empty, string.Empty, Txt_Ass_O.Text.Trim(), Txt_Ass_R.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                        }
                    }



                    //gvAwards.Rows.Clear();
                    if (gvAwards.Rows.Count > 0)
                    {
                        //DatabaseLayer.FuelControlDB.UpdateFuelCntl("D", "B", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty,string.Empty);

                        //int Count = gvAwards.Rows.Count; int CntVariable = 1;
                        foreach (DataGridViewRow Entity in gvAwards.Rows)
                        {
                            if (Entity.Cells["Check"].Value != null && Convert.ToBoolean(Entity.Cells["Check"].Value) == true)
                            {
                                if (Entity.Cells["Sel_Switch"].Value.ToString() == "Y")
                                {
                                    DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "B", Entity.Cells["Award"].Value.ToString().Trim(), string.Empty, string.Empty,
                                        Entity.Cells["V1"].Value.ToString().Trim(), Entity.Cells["V2"].Value.ToString().Trim(), Entity.Cells["V3"].Value.ToString().Trim(), Entity.Cells["V4"].Value.ToString().Trim(),
                                        Entity.Cells["V5"].Value.ToString().Trim(), Entity.Cells["NV1"].Value.ToString().Trim(), Entity.Cells["NV2"].Value.ToString().Trim(),
                                        Entity.Cells["NV3"].Value.ToString().Trim(), Entity.Cells["NV4"].Value.ToString().Trim(), Entity.Cells["NV5"].Value.ToString().Trim(), BaseForm.UserID, BaseForm.UserID);
                                    //CntVariable++;
                                }
                            }
                            else
                            {
                                DatabaseLayer.FuelControlDB.UpdateFuelCntl("D", ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "B", Entity.Cells["Award"].Value.ToString().Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            }
                        }
                    }
                    else
                    {
                        //int Count = gvAwards.Rows.Count; int CntVariable=1;
                        foreach (DataGridViewRow Entity in gvAwards.Rows)
                        {
                            //if (CntVariable <= Count-1)
                            //{
                            DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "B", Entity.Cells["Award"].Value.ToString().Trim(), string.Empty, string.Empty,
                                    Entity.Cells["V1"].Value.ToString().Trim(), Entity.Cells["V2"].Value.ToString().Trim(), Entity.Cells["V3"].Value.ToString().Trim(), Entity.Cells["V4"].Value.ToString().Trim(),
                                    Entity.Cells["V5"].Value.ToString().Trim(), Entity.Cells["NV1"].Value.ToString().Trim(), Entity.Cells["NV2"].Value.ToString().Trim(),
                                    Entity.Cells["NV3"].Value.ToString().Trim(), Entity.Cells["NV4"].Value.ToString().Trim(), Entity.Cells["NV5"].Value.ToString().Trim(), BaseForm.UserID, BaseForm.UserID);
                            //CntVariable++;
                            //}
                        }
                    }

                    FillControls();
                    AlertBox.Show("Saved Successfully");
                }
                else
                {
                    AlertBox.Show("B1,R1,U1 and C are Mandatory", MessageBoxIcon.Warning);
                }
            }
            //else
            //{
            //    if (gvIncome.Rows.Count > 0)
            //    {


            //        foreach (DataGridViewRow dr in gvIncome.Rows)
            //        {
            //            //if (!string.IsNullOrEmpty(dr.Cells["Col1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(dr.Cells["Col2"].Value.ToString().Trim()) &&
            //            //    !string.IsNullOrEmpty(dr.Cells["Col3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(dr.Cells["Col4"].Value.ToString().Trim()) &&
            //            //    !string.IsNullOrEmpty(dr.Cells["Col5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(dr.Cells["Col6"].Value.ToString().Trim()) &&
            //            //    !string.IsNullOrEmpty(dr.Cells["Col7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(dr.Cells["Col8"].Value.ToString().Trim()))
            //            //{

            //                DatabaseLayer.FuelControlDB.UpdateState_MedianInc_Guide("U", ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), txtIncome.Text.ToString().Trim(),
            //                    dr.Cells["Col1"].Value.ToString().Trim(), dr.Cells["Col2"].Value.ToString().Trim(), dr.Cells["Col3"].Value.ToString().Trim(),
            //                    dr.Cells["Col5"].Value.ToString().Trim(), dr.Cells["Col4"].Value.ToString().Trim(), dr.Cells["Col6"].Value.ToString().Trim(),
            //                    dr.Cells["Col7"].Value.ToString().Trim(), dr.Cells["Col8"].Value.ToString().Trim());
            //            //}
            //            //else
            //            //{
            //            //    MessageBox.Show("Fill All the Values");
            //            //}
            //        }
            //    }
            //}
        }

        private bool IsValidateForm()
        {
            bool isValid = true;

            if (gvIncome.Rows.Count > 0)
            {
                //if(string.IsNullOrEmpty(gvIncome.Rows[0].Cells[1].Value.ToString().Trim()))
                //    isValid=false;
                //|| string.IsNullOrEmpty(gvIncome.Rows[0].Cells[2].Value.ToString().Trim()) ||
                //string.IsNullOrEmpty(gvIncome.Rows[0].Cells[3].Value.ToString().Trim()) ||string.IsNullOrEmpty(gvIncome.Rows[0].Cells[4].Value.ToString().Trim()) ||
                //string.IsNullOrEmpty(gvIncome.Rows[0].Cells[5].Value.ToString().Trim()) || string.IsNullOrEmpty(gvIncome.Rows[0].Cells[6].Value.ToString().Trim()) ||
                //string.IsNullOrEmpty(gvIncome.Rows[0].Cells[7].Value.ToString().Trim()) || string.IsNullOrEmpty(gvIncome.Rows[0].Cells[8].Value.ToString().Trim()))
                //isValid=false;
            }

            if (string.IsNullOrEmpty(txtIncome.Text.Trim()))
            {
                _errorProvider.SetError(txtIncome, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIncome.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtIncome, null);
            }


            if (dtpAStart.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpAStart, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblA.Text + " " + lblAStart.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpAStart, null);
            }

            if (dtpAEnd.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpAEnd, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblA.Text + " " + lblAEnd.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpAEnd, null);
            }

            if (dtpAStart.Checked.Equals(true) && dtpAEnd.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtpAStart.Text) && (!string.IsNullOrEmpty(dtpAEnd.Text)))
                {
                    if (Convert.ToDateTime(dtpAStart.Text) > Convert.ToDateTime(dtpAEnd.Text))
                    {
                        _errorProvider.SetError(dtpAEnd, "It should be greater than start date ".Replace(Consts.Common.Colon, string.Empty));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpAEnd, null);
                    }
                }
            }

            if (dtpBFrom.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpBFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblB.Text + " " + lblBStart.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpBFrom, null);
            }

            if (dtpBEnd.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpBEnd, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblB.Text + " " + lblBEnd.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpBEnd, null);
            }

            if (dtpBFrom.Checked.Equals(true) && dtpBEnd.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtpBFrom.Text) && (!string.IsNullOrEmpty(dtpBEnd.Text)))
                {
                    if (Convert.ToDateTime(dtpBFrom.Text) > Convert.ToDateTime(dtpBEnd.Text))
                    {
                        _errorProvider.SetError(dtpBEnd, "It should be greater than start date ".Replace(Consts.Common.Colon, string.Empty));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpBEnd, null);
                    }
                }
            }

            if (dtpCStart.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpCStart, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblC.Text + " " + lblCStart.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpCStart, null);
            }

            if (dtpCEnd.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpCEnd, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblC.Text + " " + lblCEnd.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpCEnd, null);
            }

            if (dtpCStart.Checked.Equals(true) && dtpCEnd.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtpCStart.Text) && (!string.IsNullOrEmpty(dtpCEnd.Text)))
                {
                    if (Convert.ToDateTime(dtpCStart.Text) > Convert.ToDateTime(dtpCEnd.Text))
                    {
                        _errorProvider.SetError(dtpCEnd, "It should be greater than start date ".Replace(Consts.Common.Colon, string.Empty));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpCEnd, null);
                    }
                }
            }

            if (dtpSStart.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpSStart, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblS.Text + " " + lblSStart.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpSStart, null);
            }

            if (dtpSEnd.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpSEnd, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblS.Text + " " + lblSEnd.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpSEnd, null);
            }

            if (dtpSStart.Checked.Equals(true) && dtpSEnd.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtpSStart.Text) && (!string.IsNullOrEmpty(dtpSEnd.Text)))
                {
                    if (Convert.ToDateTime(dtpSStart.Text) > Convert.ToDateTime(dtpSEnd.Text))
                    {
                        _errorProvider.SetError(dtpSEnd, "It should be greater than start date ".Replace(Consts.Common.Colon, string.Empty));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpSEnd, null);
                    }
                }
            }

            if (string.IsNullOrEmpty(txtL1_Vul.Text.Trim()))
            {
                _errorProvider.SetError(txtL1_Vul, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTot.Text + " " + lblTot.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtL1_Vul, null);

            if (string.IsNullOrEmpty(txtL1_Nonvul.Text.Trim()))
            {
                _errorProvider.SetError(txtL1_Nonvul, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTot.Text + " " + lblTot.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtL1_Nonvul, null);

            if (string.IsNullOrEmpty(Txt_Ass_O.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Ass_O, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), (label10.Text + " " + label11.Text).Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(Txt_Ass_O, null);

            if (string.IsNullOrEmpty(Txt_Ass_R.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Ass_R, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), (label10.Text + " " + label12.Text).Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(Txt_Ass_R, null);


            return (isValid);
        }

        private bool IsValidateGrid()
        {
            bool isValid = true;
            if (gvAwards.Rows.Count > 0)
            {
                int rowIndex = 0;
                foreach (DataGridViewRow dr in gvAwards.Rows)
                {
                    if (rowIndex == 0 && dr.Cells["Check"].Value.ToString() == "False")
                        isValid = false;
                    if (rowIndex == 1 && dr.Cells["Check"].Value.ToString() == "False")
                        isValid = false;
                    if (rowIndex == 2 && dr.Cells["Check"].Value.ToString() == "False")
                        isValid = false;
                    if (rowIndex == 3 && dr.Cells["Check"].Value.ToString() == "False")
                        isValid = false;
                    rowIndex++;
                    if (rowIndex > 3)
                        break;
                }
            }

            return (isValid);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvIncome_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gvIncome.Rows.Count > 0)
            {
                bool boolcellstatus = true;
                if (ModeChange == "Values")
                {
                    int intcolindex = gvIncome.CurrentCell.ColumnIndex;
                    int introwindex = gvIncome.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvIncome.Rows[introwindex].Cells[intcolindex].Value);
                    //string strCol1 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col1"].Value);
                    //string strCol2 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col2"].Value);
                    //string strCol3 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col3"].Value);
                    //string strCol4 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col4"].Value);
                    //string strCol5 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col5"].Value);
                    //string strCol6 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col6"].Value);
                    //string strCol7 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col7"].Value);
                    //string strCol8 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col8"].Value);


                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col1"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col1"].Value = "0.00";
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col1"].Value = "0.00";
                        }
                    }

                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col2"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col2"].Value = "0.00";//string.Empty;
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                            gvIncome.Rows[introwindex].Cells["Col2"].Value = "0.00";
                    }

                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col3"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col3"].Value = "0.00";//string.Empty;
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                            gvIncome.Rows[introwindex].Cells["Col3"].Value = "0.00";
                    }

                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col4"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col4"].Value = "0.00";//string.Empty;
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                            gvIncome.Rows[introwindex].Cells["Col4"].Value = "0.00";
                    }

                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col5"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col5"].Value = "0.00";//string.Empty;
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                            gvIncome.Rows[introwindex].Cells["Col5"].Value = "0.00";
                    }

                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col6"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col6"].Value = "0.00";//string.Empty;
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                            gvIncome.Rows[introwindex].Cells["Col6"].Value = "0.00";
                    }

                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col7"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col7"].Value = "0.00";//string.Empty;
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                            gvIncome.Rows[introwindex].Cells["Col7"].Value = "0.00";
                    }

                    if (gvIncome.Columns[e.ColumnIndex].Name.Equals("Col8"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                        {
                            gvIncome.Rows[introwindex].Cells["Col8"].Value = "0.00";//string.Empty;
                            //MessageBox.Show(Consts.Messages.PleaseEnterDecimals);
                            //boolcellstatus = false;
                        }
                        else if (strCurrectValue == string.Empty)
                            gvIncome.Rows[introwindex].Cells["Col8"].Value = "0.00";
                    }
                }
            }
        }

        private void gvAwards_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gvAwards.Rows.Count > 0)
            {
                bool boolcellstatus = true;

                int intcolindex = gvAwards.CurrentCell.ColumnIndex;
                int introwindex = gvAwards.CurrentCell.RowIndex;

                string strCurrectValue = Convert.ToString(gvAwards.Rows[introwindex].Cells[intcolindex].Value);
                //string strCol1 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col1"].Value);
                //string strCol2 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col2"].Value);
                //string strCol3 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col3"].Value);
                //string strCol4 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col4"].Value);
                //string strCol5 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col5"].Value);
                //string strCol6 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col6"].Value);
                //string strCol7 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col7"].Value);
                //string strCol8 = Convert.ToString(gvIncome.Rows[introwindex].Cells["Col8"].Value);
                FUELCNTLEntity Entity = Fuelcntl.Find(u => u.Benefit_Type.Equals("B") && u.Award_Type.Trim().Equals(gvAwards.Rows[introwindex].Cells["Award"].Value.ToString().Trim()));

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("V1"))
                {
                    //if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.FiveDecimalRange6String) && strCurrectValue != string.Empty)
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        //gvAwards.Rows[introwindex].Cells["V1"].Value = "0.00000";
                        gvAwards.Rows[introwindex].Cells["V1"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L1_Vulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["V1"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("V2"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["V2"].Value = "0.00";
                        MessageBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L2_Vulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["V2"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("V3"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["V3"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L3_Vulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["V3"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("V4"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["V4"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L4_Vulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["V4"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("V5"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["V5"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L5_Vulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["V5"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("NV1"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["NV1"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L1_NonVulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["NV1"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("NV2"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["NV2"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L2_NonVulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["NV2"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("NV3"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["NV3"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L3_NonVulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["NV3"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("NV4"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["NV4"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L4_NonVulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["NV4"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

                if (gvAwards.Columns[e.ColumnIndex].Name.Equals("NV5"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalRange6String) && strCurrectValue != string.Empty)
                    {
                        gvAwards.Rows[introwindex].Cells["NV5"].Value = "0.00";
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals + " or Enter 6 digit number");
                        boolcellstatus = false;
                    }
                    else
                    {
                        if (Entity != null)
                        {
                            if (Entity.L5_NonVulner.ToString().Trim() != gvAwards.Rows[introwindex].Cells["NV5"].Value.ToString().Trim())
                                gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                        }
                        else
                            gvAwards.Rows[introwindex].Cells["Sel_Switch"].Value = "Y";
                    }
                }

            }
        }


        private void FillControls()
        {
            DataSet dsIncome = DatabaseLayer.FuelControlDB.Browse_State_MedianInc_Guide();
            DataTable dtIncome = new DataTable();
            gvIncome.Rows.Clear();
            if (dsIncome != null)
                dtIncome = dsIncome.Tables[0];
            if (dsIncome != null)
            {
                dtIncome = dsIncome.Tables[0];
                if (dtIncome.Rows.Count > 0)
                {
                    foreach (DataRow row in dtIncome.Rows)
                    {
                        if (((ListItem)cmbYear.SelectedItem).Text.ToString().Trim() == row["SMG_YEAR"].ToString().Trim())
                        {
                            txtIncome.Text = row["SMG_ELIG_INCOME"].ToString().Trim(); Priv_Income = row["SMG_ELIG_INCOME"].ToString().Trim();
                            gvIncome.Enabled = true;
                            gvIncome.Rows.Add("Income", row["SMG_HSIZE_1"].ToString().Trim(), row["SMG_HSIZE_2"].ToString().Trim()
                                , row["SMG_HSIZE_3"].ToString().Trim(), row["SMG_HSIZE_4"].ToString().Trim(), row["SMG_HSIZE_5"].ToString().Trim(),
                                row["SMG_HSIZE_6"].ToString().Trim(), row["SMG_HSIZE_7"].ToString().Trim(), row["SMG_HSIZE_8"].ToString().Trim());

                            //txtIncome.Text = dtIncome.Rows[0]["SMG_ELIG_INCOME"].ToString().Trim(); Priv_Income = dtIncome.Rows[0]["SMG_ELIG_INCOME"].ToString().Trim();
                            //gvIncome.Enabled = true;
                            //gvIncome.Rows.Add("Income", dtIncome.Rows[0]["SMG_HSIZE_1"].ToString().Trim(), dtIncome.Rows[0]["SMG_HSIZE_2"].ToString().Trim()
                            //    , dtIncome.Rows[0]["SMG_HSIZE_3"].ToString().Trim(), dtIncome.Rows[0]["SMG_HSIZE_4"].ToString().Trim(), dtIncome.Rows[0]["SMG_HSIZE_5"].ToString().Trim(),
                            //    dtIncome.Rows[0]["SMG_HSIZE_6"].ToString().Trim(), dtIncome.Rows[0]["SMG_HSIZE_7"].ToString().Trim(), dtIncome.Rows[0]["SMG_HSIZE_8"].ToString().Trim());

                            CommonFunctions.SetComboBoxValue(cmbYear, row["SMG_Year"].ToString().Trim());
                            if (Privileges.ChangePriv.Equals("false")) { btnSave.Visible = false; }
                            else if (Privileges.ChangePriv.Equals("true")) { btnSave.Visible = true; }
                        }

                    }
                }
            }

            FillAwards();
            if(gvIncome.Rows.Count>0)
                gvIncome.Rows[0].Selected = true;
            //DataSet dsAwards = DatabaseLayer.FuelControlDB.Browse_FuelCntl();
            //DataTable dtAwards = new DataTable();

                //if (dsAwards != null)
                //{
                //    dtAwards = dsAwards.Tables[0];
                //    if (dtAwards.Rows.Count > 0)
                //    {
                //        Fuelcntl.Clear();
                //        foreach (DataRow Row in dtAwards.Rows)
                //        {
                //            if (Row["FCNTL_BEN_TYPE"].ToString() == "B" && Row["FCNTL_YEAR"].ToString().Trim()==((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                //                Fuelcntl.Add(new FUELCNTLEntity(Row));
                //        }

                //        List<FUELCNTLEntity> FuelCntlList = new List<FUELCNTLEntity>();
                //        foreach (DataRow Row in dtAwards.Rows)
                //        {
                //            if (Row["FCNTL_YEAR"].ToString().Trim() == ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                //                FuelCntlList.Add(new FUELCNTLEntity(Row));
                //        }

                //        gvAwards.Rows.Clear(); int rowIndex = 0;
                //        if (FuelCntlList.Count > 0)
                //        {
                //            foreach (FUELCNTLEntity Entity in FuelCntlList)
                //            {
                //                if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "B")
                //                {
                //                    dtpBFrom.Checked = true; dtpBFrom.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                //                    dtpBEnd.Checked = true; dtpBEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                //                }
                //                else if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "A")
                //                {
                //                    dtpAStart.Checked = true; dtpAStart.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                //                    dtpAEnd.Checked = true; dtpAEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                //                }
                //                else if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "C")
                //                {
                //                    dtpCStart.Checked = true; dtpCStart.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                //                    dtpCEnd.Checked = true; dtpCEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                //                }
                //                else if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "S")
                //                {
                //                    dtpSStart.Checked = true; dtpSStart.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                //                    dtpSEnd.Checked = true; dtpSEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                //                }
                //                else if (Entity.Benefit_Type.ToString().Trim() == "B")
                //                {
                //                    rowIndex = gvAwards.Rows.Add(Entity.Award_Type.ToString().Trim(), float.Parse(Entity.L1_Vulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L2_Vulner.ToString().Trim()).ToString("0.00000"),
                //                        float.Parse(Entity.L3_Vulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L4_Vulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L5_Vulner.ToString().Trim()).ToString("0.00000"),
                //                        float.Parse(Entity.L1_NonVulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L2_NonVulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L3_NonVulner.ToString().Trim()).ToString("0.00000"),
                //                        float.Parse(Entity.L4_NonVulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L5_NonVulner.ToString().Trim()).ToString("0.00000"), "N");

                //                    string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + LookupDataAccess.Getdate(Entity.Add_Date.ToString().Trim()) + "\n" +
                //                         "Modified By  : " + Entity.Lstc_Operator.ToString().Trim() + " on " + LookupDataAccess.Getdate(Entity.Lstc_Date.ToString().Trim());
                //                    foreach (DataGridViewCell cell in gvAwards.Rows[rowIndex].Cells)
                //                    {
                //                        cell.ToolTipText = toolTipText;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            gvAwards.Rows.Add("B1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //            gvAwards.Rows.Add("C", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //            gvAwards.Rows.Add("R1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //            gvAwards.Rows.Add("S1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //            gvAwards.Rows.Add("S2", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //            gvAwards.Rows.Add("S3", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //            gvAwards.Rows.Add("U1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //            //gvAwards.Rows.Add("S4", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                //        }
                //        //foreach (DataRow dr in dtAwards.Rows)
                //        //{
                //        //    if (dr["FCNTL_YEAR"].ToString().Trim() == ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                //        //    {
                //        //        if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "B")
                //        //        {
                //        //            dtpBFrom.Checked = true; dtpBFrom.Text = dr["FCNTL_SDate"].ToString().Trim();
                //        //            dtpBEnd.Checked = true; dtpBEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                //        //        }
                //        //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "A")
                //        //        {
                //        //            dtpAStart.Checked = true; dtpAStart.Text = dr["FCNTL_SDate"].ToString().Trim();
                //        //            dtpAEnd.Checked = true; dtpAEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                //        //        }
                //        //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "C")
                //        //        {
                //        //            dtpCStart.Checked = true; dtpCStart.Text = dr["FCNTL_SDate"].ToString().Trim();
                //        //            dtpCEnd.Checked = true; dtpCEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                //        //        }
                //        //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "S")
                //        //        {
                //        //            dtpSStart.Checked = true; dtpSStart.Text = dr["FCNTL_SDate"].ToString().Trim();
                //        //            dtpSEnd.Checked = true; dtpSEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                //        //        }
                //        //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "B")
                //        //        {
                //        //            rowIndex = gvAwards.Rows.Add(dr["FCNTL_Award"].ToString().Trim(), dr["FCNTL_L1_Vul"].ToString().Trim(), dr["FCNTL_L2_VuL"].ToString().Trim(),
                //        //                dr["FCNTL_L3_Vul"].ToString().Trim(), dr["FCNTL_L4_Vul"].ToString().Trim(), dr["FCNTL_L5_Vul"].ToString().Trim(),
                //        //                dr["FCNTL_L1_NonVul"].ToString().Trim(), dr["FCNTL_L2_NonVul"].ToString().Trim(), dr["FCNTL_L3_NonVul"].ToString().Trim(),
                //        //                dr["FCNTL_L4_NonVul"].ToString().Trim(), dr["FCNTL_L5_NonVul"].ToString().Trim(), "N");

                //        //            string toolTipText = "Added By     : " + dr["FCNTL_ADD_OPERATOR"].ToString().Trim() + " on " + dr["FCNTL_DATE_ADD"].ToString() + "\n" +
                //        //                 "Modified By  : " + dr["FCNTL_LSTC_OPERATOR"].ToString().Trim() + " on " + dr["FCNTL_DATE_LSTC"].ToString();
                //        //            foreach (DataGridViewCell cell in gvAwards.Rows[rowIndex].Cells)
                //        //            {
                //        //                cell.ToolTipText = toolTipText;
                //        //            }
                //        //        }
                //        //    }
                //        //}
                //    }
                //    //else
                //    //{
                //    //    gvAwards.Rows.Add("B1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //    gvAwards.Rows.Add("C", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //    gvAwards.Rows.Add("R1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //    gvAwards.Rows.Add("S1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //    gvAwards.Rows.Add("S2", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //    gvAwards.Rows.Add("S3", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //    gvAwards.Rows.Add("U1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //    //gvAwards.Rows.Add("S4", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                //    //}
                //}
        }

        private void FillAwards()
        {
            Txt_Ass_O.Clear(); Txt_Ass_R.Clear();

            DataSet dsAwards = DatabaseLayer.FuelControlDB.Browse_FuelCntl(string.Empty);
            DataTable dtAwards = new DataTable();

            if (dsAwards != null)
            {
                Fuelcntl = new List<FUELCNTLEntity>();
                dtAwards = dsAwards.Tables[0];
                if (dtAwards.Rows.Count > 0)
                {
                    if (Fuelcntl != null)
                        Fuelcntl.Clear();
                    foreach (DataRow Row in dtAwards.Rows)
                    {
                        if (Row["FCNTL_BEN_TYPE"].ToString() == "B" && Row["FCNTL_YEAR"].ToString().Trim() == ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                            Fuelcntl.Add(new FUELCNTLEntity(Row));
                    }

                    List<FUELCNTLEntity> FuelCntlList = new List<FUELCNTLEntity>();
                    foreach (DataRow Row in dtAwards.Rows)
                    {
                        if (Row["FCNTL_YEAR"].ToString().Trim() == ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                            FuelCntlList.Add(new FUELCNTLEntity(Row));
                    }

                    gvAwards.Rows.Clear();
                    //gvAwards.Rows.Add("B1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", true, "Y");
                    //gvAwards.Rows.Add("R1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", true, "Y");
                    //gvAwards.Rows.Add("U1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", true, "Y");
                    //gvAwards.Rows.Add("C", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", true, "Y");
                    //gvAwards.Rows.Add("S1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                    //gvAwards.Rows.Add("S2", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                    //gvAwards.Rows.Add("S3", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                    //gvAwards.Rows.Add("S4", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                    //gvAwards.Rows.Add("S5", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                    //gvAwards.Rows.Add("S6", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                    //gvAwards.Rows.Add("S7", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                    gvAwards.Rows.Add("B1", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", true, "Y");
                    gvAwards.Rows.Add("R1", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", true, "Y");
                    gvAwards.Rows.Add("U1", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", true, "Y");

                    if(!string.IsNullOrEmpty(((ListItem)cmbYear.SelectedItem).Text.ToString().Trim()))
                    {
                        if (int.Parse(((ListItem)cmbYear.SelectedItem).Text.ToString().Trim()) >= 2023)
                        {
                            gvAwards.Rows.Add("C1", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", true, "Y");
                            gvAwards.Rows.Add("C2", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("C3", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("C4", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("C5", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("C6", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("C7", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("C8", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");

                        }
                        else
                        {
                            gvAwards.Rows.Add("C", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", true, "Y");
                            gvAwards.Rows.Add("S1", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("S2", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("S3", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("S4", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("S5", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("S6", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                            gvAwards.Rows.Add("S7", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", false, "N");
                        }
                    }

                    

                    if (FuelCntlList.Count > 0)
                    {
                        foreach (FUELCNTLEntity Entity in FuelCntlList)
                        {
                            if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "B")
                            {
                                dtpBFrom.Checked = true; dtpBFrom.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                                dtpBEnd.Checked = true; dtpBEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                            }
                            else if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "A")
                            {
                                dtpAStart.Checked = true; dtpAStart.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                                dtpAEnd.Checked = true; dtpAEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                            }
                            else if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "C")
                            {
                                dtpCStart.Checked = true; dtpCStart.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                                dtpCEnd.Checked = true; dtpCEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                            }
                            else if (Entity.Benefit_Type.ToString().Trim() == "D" && Entity.Award_Type.ToString().Trim() == "S")
                            {
                                dtpSStart.Checked = true; dtpSStart.Text = Entity.Start_Date.ToString().Trim();// dr["FCNTL_SDate"].ToString().Trim();
                                dtpSEnd.Checked = true; dtpSEnd.Text = Entity.End_Date.ToString().Trim();//dr["FCNTL_EDate"].ToString().Trim();
                            }
                            else if (Entity.Benefit_Type.ToString().Trim() == "B")
                            {
                                int rowIndex = 0;
                                foreach (DataGridViewRow dr in gvAwards.Rows)
                                {
                                    if (Entity.Award_Type.ToString().Trim() == dr.Cells["Award"].Value.ToString().Trim())
                                    {
                                        this.gvAwards.CellValueChanged -= gvAwards_CellValueChanged;
                                        dr.Cells["Award"].Value = Entity.Award_Type.ToString().Trim();
                                        //dr.Cells["V1"].Value = float.Parse(Entity.L1_Vulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["V2"].Value = float.Parse(Entity.L2_Vulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["V3"].Value = float.Parse(Entity.L3_Vulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["V4"].Value = float.Parse(Entity.L4_Vulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["V5"].Value = float.Parse(Entity.L5_Vulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["NV1"].Value = float.Parse(Entity.L1_NonVulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["NV2"].Value = float.Parse(Entity.L2_NonVulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["NV3"].Value = float.Parse(Entity.L3_NonVulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["NV4"].Value = float.Parse(Entity.L4_NonVulner.ToString().Trim()).ToString("0.00000");
                                        //dr.Cells["NV5"].Value = float.Parse(Entity.L5_NonVulner.ToString().Trim()).ToString("0.00000");
                                        dr.Cells["V1"].Value = float.Parse(Entity.L1_Vulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["V2"].Value = float.Parse(Entity.L2_Vulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["V3"].Value = float.Parse(Entity.L3_Vulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["V4"].Value = float.Parse(Entity.L4_Vulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["V5"].Value = float.Parse(Entity.L5_Vulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["NV1"].Value = float.Parse(Entity.L1_NonVulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["NV2"].Value = float.Parse(Entity.L2_NonVulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["NV3"].Value = float.Parse(Entity.L3_NonVulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["NV4"].Value = float.Parse(Entity.L4_NonVulner.ToString().Trim()).ToString("0.00");
                                        dr.Cells["NV5"].Value = float.Parse(Entity.L5_NonVulner.ToString().Trim()).ToString("0.00");


                                        dr.Cells["Check"].Value = true; dr.Cells["Sel_Switch"].Value = "N";

                                        string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + LookupDataAccess.Getdate(Entity.Add_Date.ToString().Trim()) + "\n" +
                                     "Modified By  : " + Entity.Lstc_Operator.ToString().Trim() + " on " + LookupDataAccess.Getdate(Entity.Lstc_Date.ToString().Trim());
                                        foreach (DataGridViewCell cell in gvAwards.Rows[rowIndex].Cells)
                                        {
                                            cell.ToolTipText = toolTipText;
                                        }
                                        this.gvAwards.CellValueChanged +=  gvAwards_CellValueChanged;
                                        break;
                                    }
                                    rowIndex++;
                                }

                                //rowIndex = gvAwards.Rows.Add(Entity.Award_Type.ToString().Trim(), float.Parse(Entity.L1_Vulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L2_Vulner.ToString().Trim()).ToString("0.00000"),
                                //    float.Parse(Entity.L3_Vulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L4_Vulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L5_Vulner.ToString().Trim()).ToString("0.00000"),
                                //    float.Parse(Entity.L1_NonVulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L2_NonVulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L3_NonVulner.ToString().Trim()).ToString("0.00000"),
                                //    float.Parse(Entity.L4_NonVulner.ToString().Trim()).ToString("0.00000"), float.Parse(Entity.L5_NonVulner.ToString().Trim()).ToString("0.00000"), true, "N");

                                //if(Entity.Award_Type.ToString().Trim()==gvAwards.Rows[0].Cells["Award"].Value.ToString())

                            }
                            else if (Entity.Benefit_Type.ToString().Trim() == "A")
                            {
                                txtL1_Vul.Text = float.Parse(Entity.L1_Vulner.ToString().Trim()).ToString("0.00");
                                txtL1_Nonvul.Text = float.Parse(Entity.L2_Vulner.ToString().Trim()).ToString("0.00");
                            }
                            else if (Entity.Benefit_Type.ToString().Trim() == "O")
                            {
                                Txt_Ass_O.Text = float.Parse(Entity.L1_Vulner.ToString().Trim()).ToString("0.00");
                                Txt_Ass_R.Text = float.Parse(Entity.L2_Vulner.ToString().Trim()).ToString("0.00");
                            }
                        }
                    }
                    else
                    {
                        dtpBFrom.Value = DateTime.Today; dtpBEnd.Value = DateTime.Today; dtpBFrom.Checked = false; dtpBEnd.Checked = false;
                        dtpAStart.Value = DateTime.Today; dtpAEnd.Value = DateTime.Today; dtpAStart.Checked = false; dtpAEnd.Checked = false;
                        dtpCStart.Value = DateTime.Today; dtpCEnd.Value = DateTime.Today; dtpCStart.Checked = false; dtpCEnd.Checked = false;
                        dtpSStart.Value = DateTime.Today; dtpSEnd.Value = DateTime.Today; dtpSStart.Checked = false; dtpSEnd.Checked = false;

                        //gvAwards.Rows.Add("B1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                        //gvAwards.Rows.Add("R1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                        //gvAwards.Rows.Add("U1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                        //gvAwards.Rows.Add("C", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000",false, "N");
                        //gvAwards.Rows.Add("S1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000",false, "N");
                        //gvAwards.Rows.Add("S2", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false,"N");
                        //gvAwards.Rows.Add("S3", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000",false, "N");
                        //gvAwards.Rows.Add("S4", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                        //gvAwards.Rows.Add("S5", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                        //gvAwards.Rows.Add("S6", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");
                        //gvAwards.Rows.Add("S7", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", false, "N");

                        //gvAwards.Rows.Add("S4", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    //foreach (DataRow dr in dtAwards.Rows)
                    //{
                    //    if (dr["FCNTL_YEAR"].ToString().Trim() == ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                    //    {
                    //        if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "B")
                    //        {
                    //            dtpBFrom.Checked = true; dtpBFrom.Text = dr["FCNTL_SDate"].ToString().Trim();
                    //            dtpBEnd.Checked = true; dtpBEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                    //        }
                    //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "A")
                    //        {
                    //            dtpAStart.Checked = true; dtpAStart.Text = dr["FCNTL_SDate"].ToString().Trim();
                    //            dtpAEnd.Checked = true; dtpAEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                    //        }
                    //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "C")
                    //        {
                    //            dtpCStart.Checked = true; dtpCStart.Text = dr["FCNTL_SDate"].ToString().Trim();
                    //            dtpCEnd.Checked = true; dtpCEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                    //        }
                    //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "D" && dr["FCNTL_Award"].ToString().Trim() == "S")
                    //        {
                    //            dtpSStart.Checked = true; dtpSStart.Text = dr["FCNTL_SDate"].ToString().Trim();
                    //            dtpSEnd.Checked = true; dtpSEnd.Text = dr["FCNTL_EDate"].ToString().Trim();
                    //        }
                    //        else if (dr["FCNTL_BEN_TYPE"].ToString().Trim() == "B")
                    //        {
                    //            rowIndex = gvAwards.Rows.Add(dr["FCNTL_Award"].ToString().Trim(), dr["FCNTL_L1_Vul"].ToString().Trim(), dr["FCNTL_L2_VuL"].ToString().Trim(),
                    //                dr["FCNTL_L3_Vul"].ToString().Trim(), dr["FCNTL_L4_Vul"].ToString().Trim(), dr["FCNTL_L5_Vul"].ToString().Trim(),
                    //                dr["FCNTL_L1_NonVul"].ToString().Trim(), dr["FCNTL_L2_NonVul"].ToString().Trim(), dr["FCNTL_L3_NonVul"].ToString().Trim(),
                    //                dr["FCNTL_L4_NonVul"].ToString().Trim(), dr["FCNTL_L5_NonVul"].ToString().Trim(), "N");

                    //            string toolTipText = "Added By     : " + dr["FCNTL_ADD_OPERATOR"].ToString().Trim() + " on " + dr["FCNTL_DATE_ADD"].ToString() + "\n" +
                    //                 "Modified By  : " + dr["FCNTL_LSTC_OPERATOR"].ToString().Trim() + " on " + dr["FCNTL_DATE_LSTC"].ToString();
                    //            foreach (DataGridViewCell cell in gvAwards.Rows[rowIndex].Cells)
                    //            {
                    //                cell.ToolTipText = toolTipText;
                    //            }
                    //        }
                    //    }
                    //}
                }
                if (gvAwards.Rows.Count > 0) {
                    gvAwards.Rows[0].Selected = true;
                }
                //else
                //{
                //    gvAwards.Rows.Add("B1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    gvAwards.Rows.Add("C", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    gvAwards.Rows.Add("R1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    gvAwards.Rows.Add("S1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    gvAwards.Rows.Add("S2", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    gvAwards.Rows.Add("S3", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    gvAwards.Rows.Add("U1", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "0.00000", "N");
                //    //gvAwards.Rows.Add("S4", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                //}
            }
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearErrorProvider(); VisibilityCopyButton();
            DataSet dsIncome = DatabaseLayer.FuelControlDB.Browse_State_MedianInc_Guide();
            DataTable dtIncome = new DataTable();
            if (dsIncome != null)
                dtIncome = dsIncome.Tables[0];
            if (dtIncome.Rows.Count > 0)
            {
                gvIncome.Rows.Clear(); bool First = false;
                foreach (DataRow dr in dtIncome.Rows)
                {
                    if (dr["SMG_Year"].ToString().Trim() == ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim())
                    {
                        First = true;
                        gvIncome.Enabled = true; txtIncome.Text = dr["SMG_ELIG_INCOME"].ToString().Trim();
                        gvIncome.Rows.Add("Income", dr["SMG_HSIZE_1"].ToString().Trim(), dr["SMG_HSIZE_2"].ToString().Trim()
                        , dr["SMG_HSIZE_3"].ToString().Trim(), dr["SMG_HSIZE_4"].ToString().Trim(), dr["SMG_HSIZE_5"].ToString().Trim(),
                        dr["SMG_HSIZE_6"].ToString().Trim(), dr["SMG_HSIZE_7"].ToString().Trim(), dr["SMG_HSIZE_8"].ToString().Trim());

                        if (Privileges.ChangePriv.Equals("false")) { btnSave.Visible = false; }
                        else if (Privileges.ChangePriv.Equals("true")) { btnSave.Visible = true; }
                        break;
                    }

                }
                if (!First)
                {
                    txtIncome.Text = string.Empty;
                    if (Privileges.AddPriv.Equals("false")) { btnSave.Visible = false; btnCopy.Visible = false; }
                    else if (Privileges.AddPriv.Equals("true")) { btnSave.Visible = true; btnCopy.Visible = true; }
                    VisibilityCopyButton();
                }
                //if (gvIncome.Rows.Count > 0) btnCopy.Visible = true; else btnCopy.Visible = false;
                FillAwards();

                if(gvIncome.Rows.Count>0)
                    gvIncome.Rows[0].Selected = true;
            }
            //FillControls();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (gvIncome.Rows.Count > 0)
            {
                string Mode = "I";
                DataSet ds = DatabaseLayer.FuelControlDB.Browse_State_MedianInc_Guide();
                DataTable dt = new DataTable();
                if (ds != null)
                    dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dgr in dt.Rows)
                    {
                        if (dgr["SMG_Year"].ToString().Trim() == (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim())
                            Mode = "U";
                    }
                }

                foreach (DataGridViewRow dr in gvIncome.Rows)
                {
                    DatabaseLayer.FuelControlDB.UpdateState_MedianInc_Guide(Mode, (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), txtIncome.Text.ToString().Trim(),
                        dr.Cells["Col1"].Value.ToString().Trim(), dr.Cells["Col2"].Value.ToString().Trim(), dr.Cells["Col3"].Value.ToString().Trim(),
                        dr.Cells["Col4"].Value.ToString().Trim(), dr.Cells["Col5"].Value.ToString().Trim(), dr.Cells["Col6"].Value.ToString().Trim(),
                        dr.Cells["Col7"].Value.ToString().Trim(), dr.Cells["Col8"].Value.ToString().Trim(), BaseForm.UserID);
                }

                string RowType = "I";
                DataSet dsFuel = DatabaseLayer.FuelControlDB.Browse_FuelCntl((int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim());
                DataTable dtFuel = new DataTable();
                if (dsFuel != null)
                    dtFuel = dsFuel.Tables[0];
                if (dtFuel.Rows.Count > 0)
                {
                    RowType = "U";

                }
                if (dtpAEnd.Checked == true && dtpAStart.Checked == true)
                    DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "D", "A", dtpAStart.Text.Trim(), dtpAEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                if (dtpBEnd.Checked == true && dtpBFrom.Checked == true)
                    DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "D", "B", dtpBFrom.Text.Trim(), dtpBEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                if (dtpCEnd.Checked == true && dtpCStart.Checked == true)
                    DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "D", "C", dtpCStart.Text.Trim(), dtpCEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                if (dtpSEnd.Checked == true && dtpSStart.Checked == true)
                    DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "D", "S", dtpSStart.Text.Trim(), dtpSEnd.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                if (!string.IsNullOrEmpty(txtL1_Vul.Text.Trim()) && !string.IsNullOrEmpty(txtL1_Nonvul.Text.Trim()))
                {
                    DataView dv = new DataView(dtFuel);
                    dv.RowFilter = "FCNTL_BEN_TYPE='A'";
                    DataTable dtVul = new DataTable();
                    dtVul = dv.ToTable();
                    if (dtVul.Rows.Count > 0)
                        DatabaseLayer.FuelControlDB.UpdateFuelCntl("U", (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "A", "98", string.Empty, string.Empty, txtL1_Vul.Text.Trim(), txtL1_Nonvul.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                    else
                        DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "A", "98", string.Empty, string.Empty, txtL1_Vul.Text.Trim(), txtL1_Nonvul.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                }

                //Overrage Save routine
                if (!string.IsNullOrEmpty(Txt_Ass_O.Text.Trim()) && !string.IsNullOrEmpty(Txt_Ass_R.Text.Trim()))
                {
                    DataTable dtFuel_Ovr = new DataTable();
                    if (dsFuel != null)
                    {
                        dtFuel_Ovr = dsFuel.Tables[0];

                        DataView dv = new DataView(dtFuel_Ovr);
                        dv.RowFilter = "FCNTL_BEN_TYPE='O'";
                        dtFuel_Ovr = dv.ToTable();
                        if (dtFuel_Ovr.Rows.Count > 0)
                            DatabaseLayer.FuelControlDB.UpdateFuelCntl(RowType, (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "O", "O", string.Empty, string.Empty, Txt_Ass_O.Text.Trim(), Txt_Ass_R.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                        else
                            DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "O", "O", string.Empty, string.Empty, Txt_Ass_O.Text.Trim(), Txt_Ass_R.Text.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, BaseForm.UserID);
                    }
                }


                //gvAwards.Rows.Clear();
                if (gvAwards.Rows.Count > 0)
                {
                    //DatabaseLayer.FuelControlDB.UpdateFuelCntl("D", "B", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty,string.Empty);

                    //int Count = gvAwards.Rows.Count; int CntVariable = 1;
                    foreach (DataGridViewRow Entity in gvAwards.Rows)
                    {
                        if (Entity.Cells["Check"].Value != null && Convert.ToBoolean(Entity.Cells["Check"].Value) == true)
                        {
                            DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "B", Entity.Cells["Award"].Value.ToString().Trim(), string.Empty, string.Empty,
                                Entity.Cells["V1"].Value.ToString().Trim(), Entity.Cells["V2"].Value.ToString().Trim(), Entity.Cells["V3"].Value.ToString().Trim(), Entity.Cells["V4"].Value.ToString().Trim(),
                                Entity.Cells["V5"].Value.ToString().Trim(), Entity.Cells["NV1"].Value.ToString().Trim(), Entity.Cells["NV2"].Value.ToString().Trim(),
                                Entity.Cells["NV3"].Value.ToString().Trim(), Entity.Cells["NV4"].Value.ToString().Trim(), Entity.Cells["NV5"].Value.ToString().Trim(), BaseForm.UserID, BaseForm.UserID);
                            //CntVariable++;

                        }
                        else
                        {
                            DatabaseLayer.FuelControlDB.UpdateFuelCntl("D", (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "B", Entity.Cells["Award"].Value.ToString().Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow Entity in gvAwards.Rows)
                    {
                        DatabaseLayer.FuelControlDB.UpdateFuelCntl("I", (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim(), "B", Entity.Cells["Award"].Value.ToString().Trim(), string.Empty, string.Empty,
                                Entity.Cells["V1"].Value.ToString().Trim(), Entity.Cells["V2"].Value.ToString().Trim(), Entity.Cells["V3"].Value.ToString().Trim(), Entity.Cells["V4"].Value.ToString().Trim(),
                                Entity.Cells["V5"].Value.ToString().Trim(), Entity.Cells["NV1"].Value.ToString().Trim(), Entity.Cells["NV2"].Value.ToString().Trim(),
                                Entity.Cells["NV3"].Value.ToString().Trim(), Entity.Cells["NV4"].Value.ToString().Trim(), Entity.Cells["NV5"].Value.ToString().Trim(), BaseForm.UserID, BaseForm.UserID);
                    }
                }
            }
        }

        private void gvAwards_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (Fuelcntl.Count > 0)
            //{
            if (e.ColumnIndex == 11 && e.RowIndex != -1)
            {
                if (e.RowIndex == 0 || e.RowIndex == 1 || e.RowIndex == 2 || e.RowIndex == 3)
                {
                    gvAwards.CurrentRow.Cells["Check"].Value = true;
                }
            }
            //}
        }

        private void ClearErrorProvider()
        {
            _errorProvider.SetError(txtIncome, null);
            _errorProvider.SetError(dtpSStart, null);
            _errorProvider.SetError(dtpSEnd, null);
            _errorProvider.SetError(dtpCEnd, null);
            _errorProvider.SetError(dtpCStart, null);
            _errorProvider.SetError(dtpBEnd, null);
            _errorProvider.SetError(dtpBFrom, null);
            _errorProvider.SetError(dtpAEnd, null);
            _errorProvider.SetError(dtpAStart, null);
        }

    }
}