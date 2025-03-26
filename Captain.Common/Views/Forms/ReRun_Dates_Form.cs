#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Model.Data;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ReRun_Dates_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private string[] strCode = null;
        public int strIndex = 0;
        #endregion

        public ReRun_Dates_Form(BaseForm baseform, PrivilegeEntity privileges, string agency, string Dept, string Prog, string Year_Value, string Form_Type,string VendorNo)
        {
            InitializeComponent();

            propFormType = Form_Type;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privileges;
            Agency = agency; Depart = Dept; Program = Prog; Year = Year_Value; Vendor = VendorNo;
            this.Text = /*Privileges.Program + " - " + Privileges.PrivilegeName.Trim() + " - " +*/ "Re-run Information";

            filldates();

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Agency { get; set; }
        public string Depart { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string propFormType { get; set; }
        public string Vendor { get; set; }
        
        #endregion


        private void filldates()
        {
            listDates.Items.Clear();
            //string Vendor=string.Empty;
            //if (propFormType == "TMSB0004")
            //    Vendor = "**********";
            List<NotesEntity> DatesList = _model.TmsApcndata.GetReRunDates(Agency, Depart, Program, Year, propFormType, Vendor);

            if (DatesList.Count > 0)
            {
                foreach (NotesEntity Entity in DatesList)
                {
                    /*ListViewItem Item = new ListViewItem();
                    Item.SubItems.Add(LookupDataAccess.Getdate(Entity.VDN_DATE_REPORTED.Trim()));
                    Item.SubItems.Add(Entity.VDN_LSTC_OPERATOR.Trim());
                    Item.SubItems.Add(Entity.VDN_DATE_SEQUENCE.Trim());
                    Item.SubItems.Add(Entity.AppCount.Trim());*/

                    //**listDates.Items.Add(Item);

                    dgvReRun.Rows.Add(LookupDataAccess.Getdate(Entity.VDN_DATE_REPORTED.Trim()), Entity.VDN_LSTC_OPERATOR.Trim(),Entity.VDN_DATE_SEQUENCE.Trim(),Entity.AppCount.Trim());
                    
                }
                //**listDates.SelectedIndex = 0;
            }
            else
                btnSelect.Visible = false;

            if(dgvReRun.Rows.Count>0)
                dgvReRun.Rows[0].Selected = true;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string[] Get_Selected()
        {
            string[] SelectedValues = new string[2];
            if (listDates.Items.Count > 0)
            {
                //SelectVendor = listView_Vendor.SelectedItem.ToString();
                //**SelectedValues[0] = listDates.SelectedItem.SubItems[0].Text.ToString().Trim();
                //**SelectedValues[1] = listDates.SelectedItem.SubItems[2].Text.ToString().Trim();

                SelectedValues[0] = listDates.SelectedItems[0].SubItems[0].Text.ToString().Trim();
                SelectedValues[1] = listDates.SelectedItems[0].SubItems[2].Text.ToString().Trim();

            }
            return SelectedValues;
        }

        // Added by Vikash on 05/22/2023 to use Data Grid View instead of ListView.
            public string[] New_Get_Selected()
            {
                string[] SelectReRunDte = new string[2];
                if (dgvReRun.Rows.Count > 0)
                {

                    SelectReRunDte[0] = dgvReRun.SelectedRows[0].Cells["gvDteRan"].Value.ToString().Trim();
                    SelectReRunDte[1] = dgvReRun.SelectedRows[0].Cells["gvSeq"].Value.ToString().Trim();

                }
                return SelectReRunDte;
            }
    }
}