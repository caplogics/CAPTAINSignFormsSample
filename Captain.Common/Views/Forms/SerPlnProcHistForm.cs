using System;
using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.DatabaseLayer;
using System.Data;

namespace Captain.Common.Views.Forms
{
    public partial class SerPlnProcHistForm : Form
    {
        private CaptainModel _model = null;
        public BaseForm _baseForm { get; set; }
        string _strKeyName = "";
        string _ScreenCode = "";
        string _UserID = "";
        public SerPlnProcHistForm(BaseForm baseform,string KeyName, string ScreenCode, string UserID)
        {
            InitializeComponent();
            _baseForm = baseform;
            _ScreenCode= ScreenCode;
            _strKeyName = KeyName;
            _UserID = UserID;
            _model = new CaptainModel();
            this.Text = "Comminucation History";
            FillHistGrid();
        }

        void FillHistGrid()
        {
           DataTable dtHist =  TmsApcn.GETPROCHIST(_ScreenCode, _strKeyName,"");
            if (dtHist.Rows.Count > 0)
            {


                foreach(DataRow _dr in dtHist.Rows)
                {
                    dgvProcHist.Rows.Add(_dr["PRH_DATE"].ToString(), _dr["PRH_USER_ID"].ToString(), _dr["PRH_short_Notes"].ToString());
                    if (_dr["PRH_SEQ"].ToString() == "1")
                    {
                        lblHeadmsg.Text = "Process Started by " + _dr["PRH_USER_ID"].ToString() + " on " + Convert.ToDateTime(_dr["PRH_DATE"].ToString()).ToString("MM/dd/yyyy");
                    }
                }
            }
        }
    }
}
