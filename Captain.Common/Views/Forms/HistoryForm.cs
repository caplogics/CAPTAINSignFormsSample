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
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HistoryForm : Form
    {
        private CaptainModel _model = null;
        public HistoryForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, CaseSnpEntity caseSnp)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            this.Text = privilegeEntity.PrivilegeName + "- History";
            StrKey = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + caseSnp.FamilySeq;
            GetHistory();
        }

        public HistoryForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, string strScreenName)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            this.Text = privilegeEntity.PrivilegeName + "- History";
            StrKey = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo;
            gvtVendor.HeaderText = " ";
            gvtVendor.Visible = false;
            gvtSubscr.HeaderText = "Operation";
            //gvtSubscr.Visible = false;
            GetIncompleteIntakeHistory();
        }

        public HistoryForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, CTTRIGCRITEntity TrigEntity, string TblName)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            this.Text = privilegeEntity.PrivilegeName + "- History";
            StrKey = TrigEntity.TRIGCRITCode; //+ TrigEntity.TRIGCRITGroupCode+ TrigEntity.TRIGCRITGroupSeq ;
            GetHistory(TblName);
        }
        public HistoryForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, List<ListItem> listemsdata,string strocstatus)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            this.Text = "Bypassed invoices during the " + strocstatus;
            lblChanges.Visible = true;
            lblChanges.Location = new Point(4, 398);
            dataGridChangeFieds.Visible = false;
            //+ TrigEntity.TRIGCRITGroupCode+ TrigEntity.TRIGCRITGroupSeq ;
            this.Size = new Size(666, 422);
            this.dataGridHistory.Size = new Size(657, 375);
            this.DateTime.HeaderText = "Applicant";
            this.DateTime.Width = 100;
            this.ChangedBy.HeaderText = "Service";
            this.ChangedBy.Width = 70;
            this.gvtSubscr.HeaderText = "Fund";
            this.gvtSubscr.Width = 70;
            this.Seq.HeaderText = "Site";
            this.Seq.Width = 45;
            btnExit.Location = new Point(585, 396);
            this.Seq.Visible = true;

            GetEMSBypassHistory(listemsdata);
        }

        #region Properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string StrKey { get; set; }
        #endregion

        public void GetHistory()
        {
            dataGridHistory.SelectionChanged -= new EventHandler(dataGridHistory_SelectionChanged);
            List<CaseHistEntity> caseHistList = _model.CaseMstData.GetCaseHistDetails("CASEMST", StrKey, "CASE2001");
            if (caseHistList.Count > 0)
            {
                //caseHistList = caseHistList.OrderByDescending(u => Convert.ToDateTime(u.DateLstc.Trim())).ToList();
                foreach (CaseHistEntity histdr in caseHistList)
                {
                    int index = dataGridHistory.Rows.Add(histdr.DateLstc, histdr.LstcOperator, histdr.HistSubScr, histdr.HistSeqNo);
                    dataGridHistory.Rows[index].Tag = histdr;
                }
            }
            dataGridHistory.SelectionChanged += new EventHandler(dataGridHistory_SelectionChanged);


            //************KRANTHI on 11/28/2022  for clientintake History screen these 4 fields are not usable**********************///
            dataGridHistory.Columns["gvtVendor"].Visible = false;
            dataGridHistory.Columns["gvtInvdate"].Visible = false;
            dataGridHistory.Columns["gvtAmount"].Visible = false;
            dataGridHistory.Columns["gvtdetails"].Visible = false;

            if(dataGridHistory.Rows.Count>0)
            {
                dataGridHistory.CurrentCell = dataGridHistory.Rows[0].Cells[1];
                dataGridHistory.Rows[0].Selected = true;
            }

        }

        public void GetIncompleteIntakeHistory()
        {
            dataGridHistory.SelectionChanged -= new EventHandler(dataGridHistory_SelectionChanged);
            List<CaseHistEntity> caseHistList = _model.CaseMstData.GetCaseHistDetails("INTKINCOMP", StrKey, "CASE2005");
            foreach (CaseHistEntity histdr in caseHistList)
            {
                int index = dataGridHistory.Rows.Add(histdr.DateLstc, histdr.LstcOperator, histdr.HistSubScr, histdr.HistSeqNo, string.Empty);
                dataGridHistory.Rows[index].Tag = histdr;
            }
            dataGridHistory.SelectionChanged += new EventHandler(dataGridHistory_SelectionChanged);

            if (dataGridHistory.Rows.Count > 0)
            {
                dataGridHistory.CurrentCell = dataGridHistory.Rows[0].Cells[1];
                dataGridHistory.Rows[0].Selected = true;
            }
        }

        public void GetHistory(string TblName)
        {
            dataGridHistory.SelectionChanged -= new EventHandler(dataGridHistory_SelectionChanged);
            List<CaseHistEntity> caseHistList = _model.CaseMstData.GetCaseHistDetails("CTTRIGCRIT", StrKey, "TMSTRIGG");
            foreach (CaseHistEntity histdr in caseHistList)
            {
                int index = dataGridHistory.Rows.Add(histdr.DateLstc, histdr.LstcOperator, histdr.HistSubScr, histdr.HistSeqNo);
                dataGridHistory.Rows[index].Tag = histdr;
            }
            dataGridHistory.SelectionChanged += new EventHandler(dataGridHistory_SelectionChanged);
        }

        public void GetEMSBypassHistory(List<ListItem> listemsdata)
        {
            dataGridHistory.SelectionChanged -= new EventHandler(dataGridHistory_SelectionChanged);

            foreach (ListItem histdr in listemsdata)
            {
                int index = dataGridHistory.Rows.Add(histdr.Text, histdr.ID, histdr.Value,  histdr.ValueDisplayCode,  histdr.ScreenCode, histdr.ScreenType, histdr.Amount, histdr.Details);
                dataGridHistory.Rows[index].Tag = histdr;
            }
            dataGridHistory.SelectionChanged += new EventHandler(dataGridHistory_SelectionChanged);
        }


        private void dataGridHistory_SelectionChanged(object sender, EventArgs e)
        {

            dataGridChangeFieds.Rows.Clear();
            foreach (DataGridViewRow item in dataGridHistory.Rows)
            {
                if (item.Selected)
                {
                    CaseHistEntity caseHistDetails = item.Tag as CaseHistEntity;
                    if (caseHistDetails != null)
                    {
                        DataTable dt = CommonFunctions.Convert_XMLstring_To_Datatable(caseHistDetails.HistChanges);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dataGridChangeFieds.Rows.Add(dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2]);
                        }
                    }
                    else
                    {
                        ListItem listHistDetails = item.Tag as ListItem;
                        if (listHistDetails != null)
                            lblChanges.Text = item.Cells["gvtdetails"].Value.ToString();

                    }
                }

            }
            if (dataGridChangeFieds.Rows.Count > 0)
                dataGridChangeFieds.Rows[0].Selected = true;
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            dataGridHistory_SelectionChanged(sender, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridChangeFieds_Click(object sender, EventArgs e)
        {

        }
    }
}