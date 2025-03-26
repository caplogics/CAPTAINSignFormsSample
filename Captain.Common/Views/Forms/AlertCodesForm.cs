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
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AlertCodesForm : Form
    {
        private CaptainModel _model = null;
        public AlertCodesForm(string alertCodes)
        {
            InitializeComponent();
            _model = new CaptainModel();
            SetAlertCodes(alertCodes);
        }

        public string GetSelectedAlertCodes
        {
            get
            {
                string alertCodes = string.Empty;
                foreach (ListItem li in checkedListBox.CheckedItems)
                {
                    if (!alertCodes.Equals(string.Empty))
                    {
                        alertCodes += " ";
                    }
                    alertCodes += li.Value;
                }
                return alertCodes;
            }
        }

        private void SetAlertCodes(string AlertCodes)
        {
            List<string> alertCodesList = new List<string>();
            if (!string.IsNullOrEmpty(AlertCodes))
            {
                string[] alertCodes = AlertCodes.Split(' ');
                for (int i = 0; i < alertCodes.Length; i++)
                {
                    alertCodesList.Add(alertCodes.GetValue(i).ToString());
                }
            }
            List<CommonEntity> listCodes = _model.lookupDataAccess.GetAlertCodes();

            foreach (CommonEntity alertCodes in listCodes)
            {                
                bool flag = false;
                if (alertCodesList.Contains(alertCodes.Code))
                {
                    flag = true;
                }
                checkedListBox.Items.Add(new ListItem(alertCodes.Desc.Trim(), alertCodes.Desc), flag);
            }
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}