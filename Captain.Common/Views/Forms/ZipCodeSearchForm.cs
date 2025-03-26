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
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ZipCodeSearchForm : Form
    {
        CaptainModel _model = null;

        public ZipCodeSearchForm(PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "ZIP Code Search" +/* privileges.Program.Trim() + */" - Search By ZIP";
            //txtZipCode.Validator = TextBoxValidation.IntegerValidator;
            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
            if (zipcodeEntity.Count > 0)
            {
                zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));
                zipcodeEntity = zipcodeEntity.OrderBy(u => Convert.ToInt64(u.Zcrzip)).ToList();
            }
            foreach (ZipCodeEntity zipdetails in zipcodeEntity)
            {
                string zipPlus = zipdetails.Zcrplus4.ToString();
                zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                int rowIndex = gvwCustomer.Rows.Add(SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity, zipdetails.Zcrstate.ToString(), zipdetails.TownSHip, zipdetails.County);
                gvwCustomer.Rows[rowIndex].Tag = zipdetails;
                CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwCustomer);
            }
            if(gvwCustomer.Rows.Count > 0)
            gvwCustomer.Rows[0].Selected = true;
            fillcombo();
        }

        public ZipCodeSearchForm(PrivilegeEntity privileges,string strZipcode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "ZIP Code Search" +/* privileges.Program.Trim() + */" - Search By ZIP";
            //txtZipCode.Validator = TextBoxValidation.IntegerValidator;
            if(strZipcode=="00000")
                strZipcode = string.Empty;                
            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(strZipcode, string.Empty, string.Empty, string.Empty);
            if (zipcodeEntity.Count > 0)
            {
                zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));
                zipcodeEntity = zipcodeEntity.OrderBy(u => Convert.ToInt64(u.Zcrzip)).ToList();
            }

            txtZipCode.Text = strZipcode;
            foreach (ZipCodeEntity zipdetails in zipcodeEntity)
            {
                string zipPlus = zipdetails.Zcrplus4.ToString();
                zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                int rowIndex = gvwCustomer.Rows.Add(SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity, zipdetails.Zcrstate.ToString(), zipdetails.TownSHip, zipdetails.County);
                gvwCustomer.Rows[rowIndex].Tag = zipdetails;
                CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwCustomer);
            }

            if (gvwCustomer.Rows.Count > 0)
                gvwCustomer.Rows[0].Selected = true;
                
            fillcombo();
        }

        public ZipCodeSearchForm(PrivilegeEntity privileges, string strZipcode,string strcityName)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = privileges.Program.Trim() + " - City Search";
          
           // lblHeader.Text = "Search By City";
            //txtZipCode.Validator = TextBoxValidation.IntegerValidator;
            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, strcityName.Trim(), string.Empty, string.Empty);
            if (zipcodeEntity.Count > 0)
                zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));

            txtCity.Text = strcityName.Trim();
            foreach (ZipCodeEntity zipdetails in zipcodeEntity)
            {
                string zipPlus = zipdetails.Zcrplus4.ToString();
                zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                int rowIndex = gvwCustomer.Rows.Add(SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity, zipdetails.Zcrstate.ToString(), zipdetails.TownSHip, zipdetails.County);
                gvwCustomer.Rows[rowIndex].Tag = zipdetails;
                CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwCustomer);
            }
            if(gvwCustomer.Rows.Count > 0)
                gvwCustomer.Rows[0].Selected = true;
            fillcombo();
        }      
        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRow()
        {
            string ZcrZipID = null;
            if (gvwCustomer != null)
            {
                foreach (DataGridViewRow dr in gvwCustomer.SelectedRows)
                { 
                    if (dr.Selected)
                    {
                        ZipCodeEntity zipdetails = dr.Tag as ZipCodeEntity;
                        if (zipdetails != null)
                        {
                            string zipPlus = zipdetails.Zcrplus4;
                            zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                            ZcrZipID = SetLeadingZeros(zipdetails.Zcrzip);
                            ZcrZipID = ZcrZipID + zipPlus.ToString();
                            break;
                        }
                    }
                }
            }
            return ZcrZipID;
        }
        public ZipCodeEntity GetSelectedZipCodedetails()
        {
            ZipCodeEntity zipdetails = null;
            if (gvwCustomer != null)
            {
                foreach (DataGridViewRow dr in gvwCustomer.SelectedRows)
                {
                    if (dr.Selected)
                    {
                         zipdetails = dr.Tag as ZipCodeEntity;                       
                    }
                }
            }
            return zipdetails;
        }

        private void fillcombo()
        {
            // List<CommonEntity> Township = _model.ZipCodeAndAgency.GetTownship();
            List<CommonEntity> Township = _model.lookupDataAccess.GetTownship(); //CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetTownship();

            foreach (CommonEntity township in Township)
            {
                cmbTownship.Items.Add(new ListItem(township.Desc, township.Code));
            }
            cmbTownship.Items.Insert(0, new ListItem("All", "0"));
            cmbTownship.SelectedIndex = 0;

            List<CommonEntity> Country = _model.ZipCodeAndAgency.GetCounty();
            foreach (CommonEntity country in Country)
            {
                cmbCounty.Items.Add(new ListItem(country.Desc, country.Code));
            }
            cmbCounty.Items.Insert(0, new ListItem("All", "0"));
            cmbCounty.SelectedIndex = 0;

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

        private void onSearch_Click(object sender, EventArgs e)
        {
            gvwCustomer.Rows.Clear();
            string zipcode = txtZipCode.Text;
            string city = txtCity.Text.ToString();
            string township = string.Empty;
            string county = string.Empty;
            if (!((ListItem)cmbTownship.SelectedItem).Value.ToString().Equals("0"))
            {
                township = ((ListItem)cmbTownship.SelectedItem).Value.ToString();
            }
            if (!((ListItem)cmbCounty.SelectedItem).Value.ToString().Equals("0"))
            {
                county = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
            }

            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(zipcode, city, township, county);

            if (zipcodeEntity.Count > 0)
                zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));

            if (zipcodeEntity.Count > 0)
            {
                zipcodeEntity = zipcodeEntity.OrderBy(u => Convert.ToInt64(u.Zcrzip)).ToList();
                foreach (ZipCodeEntity zipdetails in zipcodeEntity)
                {
                    string zipPlus = zipdetails.Zcrplus4.ToString();
                    zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    int rowIndex = gvwCustomer.Rows.Add(SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity, zipdetails.Zcrstate.ToString(), zipdetails.TownSHip, zipdetails.County);
                    gvwCustomer.Rows[rowIndex].Tag = zipdetails;
                    CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwCustomer);
                }
                if (gvwCustomer.Rows.Count > 0)
                    gvwCustomer.Rows[0].Selected = true;
            }
            else
            {
                AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();  
        }

        private void gvwCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (gvwCustomer.Rows.Count > 0)
            {
                if (gvwCustomer.SelectedRows[0].Selected)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();  
                }
            }
        }
    }
}