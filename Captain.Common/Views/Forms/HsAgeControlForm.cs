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
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HsAgeControlForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion
        public HsAgeControlForm(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Mode = mode;
            this.Text = "School Age Control ";
            _model = new CaptainModel();
            FillGrid();
        }
        public BaseForm BaseForm { get; set; }
        public string Mode { get; set; }

        private void FillGrid()
        {
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetMonths();
            commonEntity.Insert(0, new CommonEntity("0", "  "));
            DataGridViewComboBoxColumn cbmonth = (DataGridViewComboBoxColumn)this.gvwSchoolDetails.Columns["gvcMonth"];
            DataGridViewComboBoxColumn cbDays = (DataGridViewComboBoxColumn)this.gvwSchoolDetails.Columns["gvcDay"];
            DataGridViewComboBoxColumn cbYears = (DataGridViewComboBoxColumn)this.gvwSchoolDetails.Columns["gvcYear"];
            cbmonth.DataSource = commonEntity;
            cbmonth.DisplayMember = "DESC";
            cbmonth.ValueMember = "CODE";
            //cbmonth = (DataGridViewComboBoxColumn)this.gvwSchoolDetails.Columns["gvcMonth"];

            List<CommonEntity> commonYears = new List<CommonEntity>();
            commonYears.Add(new CommonEntity("0", "    "));
            commonYears.Add(new CommonEntity("01", "This Program Year"));
            commonYears.Add(new CommonEntity("02", "Next Year"));

            cbYears.DataSource = commonYears;
            cbYears.DisplayMember = "DESC";
            cbYears.ValueMember = "CODE";

            cbDays.DataSource = fillMonthDays();
            cbDays.DisplayMember = "DESC";
            cbDays.ValueMember = "CODE";

            List<AgyTabEntity> SchoolDetails = _model.Agytab.GetAgencyTableCodes("03003");
            foreach (AgyTabEntity item in SchoolDetails)
            {
                gvwSchoolDetails.Rows.Add(item.agy3, item.agy7, item.agy4, item.agy5, item.agy6, item.agycode);
            }
        }


        private List<CommonEntity> fillMonthDays()
        {
            int intDays = 31;
            List<CommonEntity> commonDays = new List<CommonEntity>();
            for (int i = 1; i <= intDays; i++)
            {
                string strdays = i.ToString();
                strdays = "00".Substring(0, 2 - strdays.Length) + strdays;
                commonDays.Add(new CommonEntity(strdays, strdays));
            }
            commonDays.Insert(0, (new CommonEntity("0", " ")));
            return commonDays;
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Validation())
            {

                bool booltrue = false;

                foreach (DataGridViewRow item in gvwSchoolDetails.Rows)
                {
                    AgyTabEntity AgyDetails = new AgyTabEntity();

                    AgyDetails.agycode = item.Cells["gvtCode1"].Value.ToString();
                    if (item.Cells["gvcMonth"].Value != null)
                        AgyDetails.agy4 = item.Cells["gvcMonth"].Value.ToString();
                    if (item.Cells["gvcDay"].Value != null)
                        AgyDetails.agy5 = item.Cells["gvcDay"].Value.ToString();
                    if (item.Cells["gvcYear"].Value != null)
                        AgyDetails.agy6 = item.Cells["gvcYear"].Value.ToString();
                    AgyDetails.agytype = "03003";
                    AgyDetails.agyRowtype = "HSAGE";
                    AgyDetails.agylstcoperator = BaseForm.UserID;
                    if (_model.Agytab.UpdateAGYTAB(AgyDetails))
                    {
                        booltrue = true;
                    }
                }
                if (booltrue == true)
                {
                    this.Close();
                }

            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please select Current Date");
            }
        }


        private bool Validation()
        {
            bool boolvalidation = true;
            string strMonth = string.Empty;
            foreach (DataGridViewRow item in gvwSchoolDetails.Rows)
            {
                item.Cells["gvtDesc"].Style.ForeColor = Color.Black;
                if (item.Cells["gvcMonth"].Value != null)
                    strMonth = item.Cells["gvcMonth"].Value.ToString();
                if (strMonth == "2")
                {
                    if (item.Cells["gvcDay"].Value != null)
                    {
                        if (Convert.ToInt32(item.Cells["gvcDay"].Value) > 29)
                        {
                            boolvalidation = false;
                            item.Cells["gvtDesc"].Style.ForeColor = Color.Red;
                            gvwSchoolDetails.Update();
                        }
                    }
                }
                else if (strMonth == "4" || strMonth == "6" || strMonth == "9" || strMonth == "11")
                {
                    if (item.Cells["gvcDay"].Value != null)
                    {
                        if (Convert.ToInt32(item.Cells["gvcDay"].Value) > 30)
                        {
                            boolvalidation = false;
                            item.Cells["gvtDesc"].Style.ForeColor = Color.Red;
                            gvwSchoolDetails.Update();
                        }
                    }
                }
            }
            return boolvalidation;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}