#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AdminControlMessageForm : Form
    {

        private CaptainModel _model = null;
        public AdminControlMessageForm(BaseForm baseForm, string strMsg, string strClientReq, string strVerReq,bool boolintake,bool boolcustom,bool boolincomever,string strClientDisMsg,string strCustomDisMsg,string strVerDisMsg,string strincompletMsg,bool boolcaseincome,string strCaseIncomeMsg)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            txtMsg.BackColor = System.Drawing.Color.Transparent;
            txtMsg.BorderStyle = Wisej.Web.BorderStyle.None;
            //txtMsg.BorderWidth = new Wisej.Web.BorderWidth(0);
            txtMsg.Enabled = false;                   
            propClientReq = strClientReq;
            propVerReq = strVerReq;
            txtMsg.Text =   strMsg;
            txtMsg.ScrollBars = ScrollBars.None;
            boolIntake = boolintake;
            boolCustom = boolcustom;
            boolIncomeVer = boolincomever;
            propClientDisMsg = strClientDisMsg;
            propCustomDisMsg = strCustomDisMsg;
            propVerDisMsg = strVerDisMsg;
            propIncompleteMsg = strincompletMsg;
            propCaseIncomeMsg = strCaseIncomeMsg;
            boolCaseIncome = boolcaseincome;
        }

        public AdminControlMessageForm(string strClientReq, string strVerReq)
        {
            InitializeComponent();           
            btnShow.Visible = false;
            propClientReq = strClientReq;
            propVerReq = strVerReq;

            string strClientData = string.Empty;
            string strVerData = string.Empty;
            if (strClientReq != string.Empty)
                strClientData = "Client Intake Required Fill Details : \n\n";
            if (strVerReq != string.Empty)
                strVerData = "\n\n Income Verification Required Fill Details : \n\n";
            txtMsg.Text = strClientData + propClientReq + strVerData + strVerReq  ;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public BaseForm BaseForm { get; set; }
        public string propClientReq { get; set; }
        public string propVerReq { get; set; }
        public bool boolIntake { get; set; }
        public bool boolCustom { get; set; }
        public bool boolIncomeVer { get; set; }
        public bool boolCaseIncome { get; set; }
        string propClientDisMsg { get; set; }
        string propCustomDisMsg { get; set; }
        string propVerDisMsg { get; set; }
        string propIncompleteMsg { get; set; }
        string propCaseIncomeMsg { get; set; }
        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Close();
            CaseNotesPrintPreview objform = new CaseNotesPrintPreview(BaseForm, propClientReq, propVerReq, boolIntake, boolCustom, boolIncomeVer, propClientDisMsg, propCustomDisMsg, propVerDisMsg, propIncompleteMsg, boolCaseIncome, propCaseIncomeMsg);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.ShowDialog();
        }

        private void spacer1_Click(object sender, EventArgs e)
        {

        }
    }
}