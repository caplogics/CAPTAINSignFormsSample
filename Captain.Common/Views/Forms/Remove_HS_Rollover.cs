using Captain.Common.Interfaces;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class Remove_HS_Rollover : Form
    {
        PrivilegeEntity _priviliges = null;
        BaseForm _baseform = null;
        public Remove_HS_Rollover(BaseForm basefrom, PrivilegeEntity priviliges)
        {
            InitializeComponent();

            this.Text = "Remove HS Rollover";
            _baseform = basefrom;
            _priviliges = priviliges;

            if (!string.IsNullOrEmpty(_baseform.BaseYear))
                lblYear.Text = lblYear.Text + ": " + _baseform.BaseYear; 

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (chkbApp.Checked || chkbIncome.Checked || chkbMember.Checked || chkbChild.Checked || chkbEnrollment.Checked || chkbCond.Checked || chkbMailingAdd.Checked || chkbIncVer.Checked
                || chkbCusQues.Checked || chldEmergncy.Checked || chkbBio.Checked || chkbSiteandRoom.Checked)
            {
                MessageBox.Show("Are you sure want to Delete selected Table(s)", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo,onclose: RemoveRollover);
            }
            else
                AlertBox.Show("Please select atleast One Table to Delete", MessageBoxIcon.Warning);
        }

        private void RemoveRollover(DialogResult dialogresult)
        {
            if (dialogresult == DialogResult.Yes)
            {
                DataSet ds = DELETE_HSS_ROLLOVER(_baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, _baseform.BaseYear);
                this.Close();
                AlertBox.Show("The selected Table(s) data has been Deleted from the " + lblYear.Text);
            }
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet DELETE_HSS_ROLLOVER(string Agency, string Dept, string Prog, string Year)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[DELETE_HSS_ROLLOVER]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@Agency", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlDept = new SqlParameter("@Dept", Dept);
            dbCmd.Parameters.Add(sqlDept);

            SqlParameter sqlProg = new SqlParameter("@Prog", Prog);
            dbCmd.Parameters.Add(sqlProg);

            SqlParameter sqlYear = new SqlParameter("@Year", Year);
            dbCmd.Parameters.Add(sqlYear);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }
    }
}
