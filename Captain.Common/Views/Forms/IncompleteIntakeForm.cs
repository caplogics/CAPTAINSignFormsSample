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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class IncompleteIntakeForm : Form
    {
        private CaptainModel _model = null;
        public IncompleteIntakeForm(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseForm;
          
            //ToolTip tooltipadd = new ToolTip();
            //tooltipadd.SetToolTip(PbAdd, "Add Incomplete Intakes");
            //ToolTip tooltipEdit = new ToolTip();
            //tooltipEdit.SetToolTip(PbEdit, "Edit Incomplete Intakes");
            //ToolTip tooltipdelete = new ToolTip();
            //tooltipdelete.SetToolTip(PbDelete, "Delete Incomplete Intakes");
            Privileges = privileges;
            this.Text = "Client Intake – Incomplete Intakes";
            _model = new CaptainModel();
            fillGriddata();
            EnableDisableviewMode(true);
           
            
        }

        List<CustomQuestionsEntity> listIntakeIncompletdata = new List<CustomQuestionsEntity>();
        void fillGriddata()
        {
            listIntakeIncompletdata = new List<CustomQuestionsEntity>();
            List<CommonEntity> listIncompleteIncome = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "09997", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
            DataSet ds = Captain.DatabaseLayer.CaseSnpData.CAPS_INTKINCOMP_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dritem in ds.Tables[0].Rows)
                {
                    CustomQuestionsEntity custentity = new CustomQuestionsEntity();
                    custentity.ACTAGENCY = dritem["INTK_AGENCY"].ToString();
                    custentity.ACTDEPT = dritem["INTK_DEPT"].ToString();
                    custentity.ACTPROGRAM = dritem["INTK_PROGRAM"].ToString();
                    custentity.ACTYEAR = dritem["INTK_YEAR"].ToString();
                    custentity.ACTAPPNO = dritem["INTK_APP"].ToString();
                    custentity.ACTCODE = dritem["INTK_INCOMP"].ToString();
                    custentity.ACTALPHARESP = dritem["INTK_DETAILS"].ToString();                  
                    custentity.lstcdate = dritem["INTK_DATE_LSTC"].ToString();
                    custentity.lstcoperator = dritem["INTK_LSTC_OPERATOR"].ToString();
                    custentity.adddate = dritem["INTK_DATE_ADD"].ToString();
                    custentity.addoperator = dritem["INTK_ADD_OPERATOR"].ToString();

                    listIntakeIncompletdata.Add(custentity);
                }
            }
            if (listIncompleteIncome.Count > 0)
            {
                gvwIncompletedata.SelectionChanged -= gvwIncompletedata_SelectionChanged;
                gvwIncompletedata.Rows.Clear();
                bool boolexist = false;
                int index = 0;
                foreach (CommonEntity dr in listIncompleteIncome)
                {

                    string resDesc = dr.Code.ToString().Trim();
                    CustomQuestionsEntity intakeincompletdata = listIntakeIncompletdata.Find(u => u.ACTCODE.Trim() == dr.Code.ToString().Trim());
                    if (intakeincompletdata != null)
                    {
                        index = gvwIncompletedata.Rows.Add(true, dr.Desc, dr.Code, intakeincompletdata.ACTALPHARESP, intakeincompletdata.ACTMULTRESP, intakeincompletdata.ACTNUMRESP);
                    }
                    else
                    {
                        index = gvwIncompletedata.Rows.Add(false, dr.Desc, dr.Code, string.Empty, string.Empty, string.Empty);
                    }

                }
                if (gvwIncompletedata.Rows.Count > 0)
                {
                    gvwIncompletedata.Rows[0].Selected = true;
                    gvwIncompletedata_SelectionChanged(gvwIncompletedata, new EventArgs());
                }
                gvwIncompletedata.SelectionChanged += gvwIncompletedata_SelectionChanged;
            }


        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        private void txtLine1_Leave(object sender, EventArgs e)
        {
            if (gvwIncompletedata.Rows.Count > 0)
            {
                if (gvwIncompletedata.SelectedCells.Count > 0)
                {

                    gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine1"].Value = txtLine1.Text;
                }
            }
        }

        private void txtLine2_Leave(object sender, EventArgs e)
        {
            if (gvwIncompletedata.Rows.Count > 0)
            {
                if (gvwIncompletedata.SelectedCells.Count > 0)
                {

                    gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine2"].Value = txtLine2.Text;
                }
            }
        }

        private void txtLine3_Leave(object sender, EventArgs e)
        {
            if (gvwIncompletedata.Rows.Count > 0)
            {
                if (gvwIncompletedata.SelectedCells.Count > 0)
                {

                    gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine3"].Value = txtLine3.Text;
                }
            }
        }

        private void gvwIncompletedata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwIncompletedata.Rows.Count > 0)
            {
                if (gvwIncompletedata.SelectedCells.Count > 0)
                {
                    //if (gvwIncompletedata.SelectedCells[0].ColumnIndex == gvchkSelect.Index)
                    //{
                    if (gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvchkSelect"].Value != null && Convert.ToBoolean(gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvchkSelect"].Value) == true)
                    {
                        txtLine1.Text = gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine1"].Value.ToString();
                        txtLine2.Text = gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine2"].Value.ToString();
                        txtLine3.Text = gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine3"].Value.ToString();
                        pnlDetails.Visible = true;
                    }
                    else
                    {
                        txtLine1.Text = string.Empty;
                        txtLine2.Text = string.Empty;
                        txtLine3.Text = string.Empty;
                        gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine1"].Value = string.Empty;
                        gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine2"].Value = string.Empty;
                        gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine3"].Value = string.Empty;
                        pnlDetails.Visible = false;
                    }
                    //  }


                }

            }
        }

        public string propIntakedata = string.Empty;
        private void btnOk_Click(object sender, EventArgs e)
        {
            string selectedCode = string.Empty;
            propIntakedata = string.Empty;
            CustomQuestionsEntity custentity = new CustomQuestionsEntity();
            custentity.ACTAGENCY = BaseForm.BaseAgency;
            custentity.ACTDEPT = BaseForm.BaseDept;
            custentity.ACTPROGRAM = BaseForm.BaseProg;
            custentity.ACTYEAR = BaseForm.BaseYear;
            custentity.ACTAPPNO = BaseForm.BaseApplicationNo;
            custentity.Mode = "DELETE";
            if (_model.CaseMstData.CAPS_INTKINCOMP_INSUPDEL(custentity))
            {
            }
            bool boolform = false;
            foreach (DataGridViewRow gvRows in gvwIncompletedata.Rows)
            {
                if (gvRows.Cells["gvchkSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvchkSelect"].Value) == true)
                {


                    custentity.ACTCODE = gvRows.Cells["gvtCode"].Value.ToString();
                    custentity.ACTALPHARESP = gvRows.Cells["gvtLine1"].Value.ToString();
                    custentity.ACTMULTRESP = gvRows.Cells["gvtLine2"].Value.ToString();
                    custentity.ACTNUMRESP = gvRows.Cells["gvtLine3"].Value.ToString();
                    custentity.addoperator = BaseForm.UserID;
                    custentity.Mode = "";
                    if (_model.CaseMstData.CAPS_INTKINCOMP_INSUPDEL(custentity))
                    {

                    }
                }
            }
          
            fillGriddata();          

            EnableDisableviewMode(false);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           
                    this.Close();
        
        }

        private void gvwIncompletedata_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwIncompletedata.Rows.Count > 0)
            {
                if (gvwIncompletedata.SelectedRows.Count > 0)
                {


                    if (gvwIncompletedata.SelectedRows[0].Cells["gvchkSelect"].Value != null && Convert.ToBoolean(gvwIncompletedata.SelectedRows[0].Cells["gvchkSelect"].Value) == true)
                    {
                        txtLine1.Text = gvwIncompletedata.SelectedRows[0].Cells["gvtLine1"].Value.ToString();
                        txtLine2.Text = gvwIncompletedata.SelectedRows[0].Cells["gvtLine2"].Value.ToString();
                        txtLine3.Text = gvwIncompletedata.SelectedRows[0].Cells["gvtLine3"].Value.ToString();
                        pnlDetails.Visible = true;
                    }
                    else
                    {
                        txtLine1.Text = string.Empty;
                        txtLine2.Text = string.Empty;
                        txtLine3.Text = string.Empty;
                        gvwIncompletedata.SelectedRows[0].Cells["gvtLine1"].Value = string.Empty;
                        gvwIncompletedata.SelectedRows[0].Cells["gvtLine2"].Value = string.Empty;
                        gvwIncompletedata.SelectedRows[0].Cells["gvtLine3"].Value = string.Empty;
                        pnlDetails.Visible = false;
                    }


                    //if (gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvchkSelect"].Value != null && Convert.ToBoolean(gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvchkSelect"].Value) == true)
                    //{
                    //    txtLine1.Text = gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine1"].Value.ToString();
                    //    txtLine2.Text = gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine2"].Value.ToString();
                    //    txtLine3.Text = gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine3"].Value.ToString();
                    //    pnlDetails.Visible = true;
                    //}
                    //else
                    //{
                    //    txtLine1.Text = string.Empty;
                    //    txtLine2.Text = string.Empty;
                    //    txtLine3.Text = string.Empty;
                    //    gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine1"].Value = string.Empty;
                    //    gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine2"].Value = string.Empty;
                    //    gvwIncompletedata.Rows[gvwIncompletedata.SelectedCells[0].RowIndex].Cells["gvtLine3"].Value = string.Empty;
                    //    pnlDetails.Visible = false;
                    //}


                }

            }
        }


        private void OnDeleteMessageBoxMstClicked(object sender, EventArgs e)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            //if (messageBoxWindow.DialogResult == DialogResult.Yes)
            //{

            //}

            EnableDisableviewMode(false);
        }


     




   
        private void EnableDisableviewMode(bool booltruefalse)
        {
            txtLine1.ReadOnly = booltruefalse;
            txtLine2.ReadOnly = booltruefalse;
            txtLine3.ReadOnly = booltruefalse;
            gvwIncompletedata.ReadOnly = booltruefalse;
           
        }


    }
}