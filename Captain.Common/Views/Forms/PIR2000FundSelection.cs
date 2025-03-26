#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PIR2000FundSelection : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public PIR2000FundSelection(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            this.Text = /*privileges.Program + " - */"Funds Selection" ;
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            propPirCntl = _model.EnrollData.GetPirCntlData();
            foreach (CommonEntity item in propfundingSource)
            {
                 int rowIndex =   gvwFunds.Rows.Add(item.Code, item.Desc, string.Empty);
                 if (propPirCntl.Count > 0)
                 {
                     PirCntl pircntl = propPirCntl.Find(u => u.PIRCNTL_FUND_CODE == item.Code);
                     if (pircntl != null)
                     {
                         gvwFunds.Rows[rowIndex].Cells["gvtType"].Value = pircntl.PIRCNTL_FUND_TYPE;
                     }
                 }
                gvwFunds.Rows[0].Selected = true;
            }
           
        }

        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<PirCntl> propPirCntl { get; set; }


        #endregion

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if (gvwFunds.Rows.Count > 0)
            {
                string strselectvalue = gvwFunds.SelectedRows[0].Cells["gvtType"].Value == null ? string.Empty : gvwFunds.SelectedRows[0].Cells["gvtType"].Value.ToString();
                contextMenu1.MenuItems.Clear();
                MenuItem menuLst = new MenuItem();
                menuLst.Text = "HS Fund";
                menuLst.Tag = "HS";
                if (strselectvalue == "HS")
                    menuLst.Checked = true;
                contextMenu1.MenuItems.Add(menuLst);
                MenuItem menuLst5 = new MenuItem();
                menuLst5.Text = "HS 2 Fund";
                menuLst5.Tag = "HS2";
                if (strselectvalue == "HS2")
                    menuLst5.Checked = true;
                contextMenu1.MenuItems.Add(menuLst5);
                MenuItem menuLst2 = new MenuItem();
                menuLst2.Text = "EHS Fund";
                menuLst2.Tag = "EHS";
                if (strselectvalue == "EHS")
                    menuLst2.Checked = true;
                contextMenu1.MenuItems.Add(menuLst2);
                MenuItem menuLst4 = new MenuItem();
                menuLst4.Text = "EHSCCEP Fund";
                menuLst4.Tag = "EHSCCEP";
                if (strselectvalue == "EHSCCEP")
                    menuLst4.Checked = true;
                contextMenu1.MenuItems.Add(menuLst4);
                
                MenuItem menuLst3 = new MenuItem();
                menuLst3.Text = "Clear";
                menuLst3.Tag = "";
                contextMenu1.MenuItems.Add(menuLst3);
            }
        }

        private void gvwFunds_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (gvwFunds.Rows.Count > 0)
            {
                gvwFunds.SelectedRows[0].Cells["gvtType"].Value = objArgs.MenuItem.Tag;              
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PirCntl pircntlEntiry = new PirCntl();
            pircntlEntiry.Mode = "DELETE";
            _model.EnrollData.InsertDelPirCntl(pircntlEntiry);

            foreach (DataGridViewRow item in gvwFunds.Rows)
            {
                pircntlEntiry.PIRCNTL_FUND_CODE = item.Cells["gvtFundCode"].Value.ToString();
                pircntlEntiry.PIRCNTL_FUND_NAME = item.Cells["gvtFundDesc"].Value.ToString();
                pircntlEntiry.PIRCNTL_FUND_TYPE = item.Cells["gvtType"].Value.ToString();
                pircntlEntiry.PIRCNTL_ADD_OPERATOR = BaseForm.UserID;
                pircntlEntiry.PIRCNTL_LSTC_OPERATOR = BaseForm.UserID;
                pircntlEntiry.Mode = string.Empty;
                _model.EnrollData.InsertDelPirCntl(pircntlEntiry);

            }

            this.Close();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}