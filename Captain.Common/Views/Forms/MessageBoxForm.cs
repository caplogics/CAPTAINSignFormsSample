#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Captain.Common.Model.Objects;
using System.IO;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MessageBoxForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private string strMode = Consts.Common.View;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        CaptainModel _model = null;
        
        #endregion
        public MessageBoxForm(PrivilegeEntity _Privileges, IncomepleteIntakeControl intakecontrol,bool boolEngery)
        {
            InitializeComponent();
            Privileges = _Privileges;
            this.Text =  /*Privileges.PrivilegeName.Trim() +*/ "Print";        
            _propintakecontrol = intakecontrol;
            btnEnergyAssis.Visible = boolEngery;
        }

        IncomepleteIntakeControl _propintakecontrol;
        private void btnUpdatedt_Click(object sender, EventArgs e)
        {

            //_propintakecontrol.printHistory();
            //_propintakecontrol.On_PrintLetter("Update");
            //// this.Hide();
            Update = string.Empty;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        string Update = string.Empty;

        //public string propReportPath { get; set; }

        private void btnPreview_Click(object sender, EventArgs e)
        {                  
            Update = "Update";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string GetValue()
        {
            string Value = string.Empty;

            Value = Update;

            return Value;
        }

        private void btnEnergyAssis_Click(object sender, EventArgs e)
        {
            Update = "EnergyAssistance";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        
    }
}