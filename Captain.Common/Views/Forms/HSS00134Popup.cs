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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00134Popup : Form
    {
        public HSS00134Popup(BaseForm baseForm, PrivilegeEntity privilieges,List<ChldMediEntity> chldMediList)
        {
            InitializeComponent();
            gvwTaskDetails.BringToFront();
            gvwEnrl.Visible = false;
            propPopupType = string.Empty;
            foreach (ChldMediEntity mediEntity in chldMediList)
            {
                int rowIndex = gvwTaskDetails.Rows.Add(mediEntity.SEQ == "0" ? string.Empty : string.Empty, mediEntity.YEAR, LookupDataAccess.Getdate(mediEntity.SBCB_DATE), LookupDataAccess.Getdate(mediEntity.ADDRESSED_DATE), LookupDataAccess.Getdate(mediEntity.COMPLETED_DATE), LookupDataAccess.Getdate(mediEntity.FOLLOWUP_DATE), LookupDataAccess.Getdate(mediEntity.DIAGNOSIS_DATE), mediEntity.SEQ, mediEntity.TASK);
                gvwTaskDetails.Rows[rowIndex].Tag = mediEntity;              
            }
        }

        public HSS00134Popup(BaseForm baseForm, PrivilegeEntity privilieges, List<ChldMediEntity> chldMediList,string strRegular)
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(401, 100);
            gvwTaskDetails.BringToFront();
            gvwEnrl.Visible = false;
            propPopupType = string.Empty;
            foreach (ChldMediEntity mediEntity in chldMediList)
            {
                int rowIndex = gvwTaskDetails.Rows.Add(mediEntity.SEQ == "0" ? string.Empty : string.Empty,mediEntity.YEAR, LookupDataAccess.Getdate(mediEntity.SBCB_DATE), LookupDataAccess.Getdate(mediEntity.ADDRESSED_DATE), LookupDataAccess.Getdate(mediEntity.COMPLETED_DATE), LookupDataAccess.Getdate(mediEntity.FOLLOWUP_DATE), LookupDataAccess.Getdate(mediEntity.DIAGNOSIS_DATE), mediEntity.SEQ, mediEntity.TASK);
                gvwTaskDetails.Rows[rowIndex].Tag = mediEntity;
            }
            btnSelect.Visible = false;
        }


        public HSS00134Popup(BaseForm baseForm, PrivilegeEntity privilieges, List<CaseEnrlEntity> caseEnrlList)
        {
            InitializeComponent();
            propPopupType = "CaseEnrl";
            gvwEnrl.BringToFront();
            gvwTaskDetails.Visible = false;
            foreach (CaseEnrlEntity enrlEntity in caseEnrlList)
            {
                int rowIndex = gvwEnrl.Rows.Add(enrlEntity.FundHie, LookupDataAccess.Getdate(enrlEntity.Enrl_Date), enrlEntity.Site, enrlEntity.Room, enrlEntity.AMPM);
                gvwEnrl.Rows[rowIndex].Tag = enrlEntity;
            }
        }


        public ChldMediEntity chldmediDetails { get; set; }
        public CaseEnrlEntity caseEnrlDetails { get; set; }
        public string propPopupType { get; set; }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (propPopupType == "CaseEnrl")
            {
                foreach (DataGridViewRow item in gvwEnrl.Rows)
                {
                    if (item.Selected)
                    {
                        caseEnrlDetails = gvwEnrl.SelectedRows[0].Tag as CaseEnrlEntity;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
                }
            
            }
            else
            {
                foreach (DataGridViewRow item in gvwTaskDetails.Rows)
                {
                    if (item.Selected)
                    {
                        chldmediDetails = gvwTaskDetails.SelectedRows[0].Tag as ChldMediEntity;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
                }
            }
        }

       
    }
}