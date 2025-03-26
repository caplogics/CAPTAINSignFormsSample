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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AgencySwithcesHierchy : Form
    {
        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public AgencySwithcesHierchy(BaseForm baseForm, string mode, PrivilegeEntity privilege,  string streditzip,  string strTmsB20, string strCApVoucher, string strTaxexemption,string strPaymentvoucher,string strMostRecent)
        {
            InitializeComponent();
            //pictureBox1.Location = new Point(6, 4);
            //lblHeader.Location = new Point(79, 12);
            //pictureBox2.Location = new Point(302, 15);
            //panel1.Location = new Point(0, 0);
            BaseForm = baseForm;
            PrivilegEntity = privilege;
            Mode = mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            FillCombo();
           // this.Size = new System.Drawing.Size(320,155);

            if (BaseForm.BaseAgencyControlDetails.PIPSwitch=="I")
            {
                pnlCheckRecent.Visible = true;  chkmostrecent.Visible = true;               
            }

            //lblHeader.Text = "Agency Switches";
            if (Mode.Equals("Add"))
            {
                this.Text = "Agency Switches"/*privilege.Program */+ " - Add";
                this.Size = new System.Drawing.Size(320, 155);
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    this.Size = new System.Drawing.Size(320, 190);
            }
            else
            {
                this.Text = "Agency Switches"/*privilege.Program*/ + " - Edit";
                this.Size = new Size(320, 155);
                TmsB20 = strTmsB20;
                CommonFunctions.SetComboBoxValue(cmbTMSB, TmsB20);
                if(BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    this.Size = new System.Drawing.Size(320, 190);
            }
           
            EditZip = streditzip;
           
            TmsB20 = strTmsB20;
            CAPVoucher = strCApVoucher;
            TaxExemption = strTaxexemption;
            PaymentVoucherNumber = strPaymentvoucher;
           
            if (EditZip == "1")
                chkEditZipandCity.Checked = true;
           
            if (CAPVoucher.ToUpper() == "Y")
                chkcapvoucher.Checked = true;

            if (strMostRecent.ToUpper() == "Y")
                chkmostrecent.Checked = true;


            txtExemption.Text = TaxExemption;
            txtPaymentVouch.Text = PaymentVoucherNumber;
        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegEntity { get; set; }

        public string Mode { get; set; }


        
        public string EditZip { get; set; }
       
        public string TmsB20 { get; set; }
        public string CAPVoucher { get; set; }
        public string TaxExemption { get; set; }
        public string PaymentVoucherNumber { get; set; }
        public string MostRecent { get; set; }

        private void FillCombo()
        {

            List<CommonEntity> commonappl = _model.lookupDataAccess.GetCapAppl();
            commonappl = commonappl.FindAll(u => u.Code == "08");
            if (commonappl.Count > 0)
            {
                lblTmsb.Visible = true;
                cmbTMSB.Visible = true;
                pnlTMSB0020.Visible = true;
                this.Size = new System.Drawing.Size(320, 210);
            }
            else
            {
                lblTmsb.Visible = false;
                cmbTMSB.Visible = false;
                pnlTMSB0020.Visible = false;
                this.Size = new System.Drawing.Size(320, 155);
            }
            cmbTMSB.Items.Add(new ListItem("Check Process Mode", "C"));
            cmbTMSB.Items.Add(new ListItem("Export/Import Mode", "E"));
            cmbTMSB.SelectedIndex = 0;


        }



        private void OnHelpClick(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (chkEditZipandCity.Checked == true)
                EditZip = "1";
            else
                EditZip = "0";

            if (chkcapvoucher.Checked == true)
                CAPVoucher = "Y";
            else
                CAPVoucher = "N";

            
            TmsB20 = ((ListItem)cmbTMSB.SelectedItem).Value.ToString();
            TaxExemption = txtExemption.Text;
            PaymentVoucherNumber = txtPaymentVouch.Text;
            MostRecent = chkmostrecent.Checked == true ? "Y" : "N";
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void chkcapvoucher_CheckedChanged(object sender, EventArgs e)
        {

            if (chkcapvoucher.Checked)
            {
                pnlPayVouch.Visible = true;
                lblTaxexp.Visible = true;
                txtExemption.Visible = true;
                this.Size = new Size(320, 220);
                //txtPaymentVouch.Visible = true;
                //this.pnlPayVouch.Location = new System.Drawing.Point(8, 171);
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    this.Size = new System.Drawing.Size(320, 250);
            }
            else
            {
                pnlPayVouch.Visible = false;
                lblTaxexp.Visible = false;
                txtExemption.Visible = false;
                txtExemption.Text = string.Empty;
                txtPaymentVouch.Text = string.Empty;
                this.Size = new Size(320,155);
                //txtPaymentVouch.Visible = false;
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    this.Size = new System.Drawing.Size(320, 190);
            }

        }
    }
}