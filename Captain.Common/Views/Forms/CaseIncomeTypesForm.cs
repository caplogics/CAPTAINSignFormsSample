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

#endregion

namespace Captain.Common.Views.Forms
{  

    public partial class CaseIncomeTypesForm : Form
    {
        private CaptainModel _model = null;   
  
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public CaseMstEntity CaseMST { get; set; }
        public CaseSnpEntity propcasesnp { get; set; }
        public string propAlertCode { get; set; }
        public string propType { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        public string MenuPropertie { get; set; }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******"));
            return LookupValues;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
           
                //CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                //caseIncomeEntity.Agency = BaseForm.BaseAgency;
                //caseIncomeEntity.Dept = BaseForm.BaseDept;
                //caseIncomeEntity.Program = BaseForm.BaseProg;
                //caseIncomeEntity.Year = BaseForm.BaseYear;
                //caseIncomeEntity.App = BaseForm.BaseApplicationNo;
                //caseIncomeEntity.FamilySeq = propcasesnp.FamilySeq ;
                //caseIncomeEntity.Seq = string.Empty;
                //if (_model.CaseMstData.DeleteCaseIncome(caseIncomeEntity))
                //{

                //    foreach (DataGridViewRow gvRows in gvwAlertCode.Rows)
                //    {
                //        if (gvRows.Cells["Select"].Value != null && Convert.ToBoolean(gvRows.Cells["Select"].Value) == true)
                //        {
                //            caseIncomeEntity.Type = Convert.ToString(gvRows.Cells["AlertCode"].Value);
                //            caseIncomeEntity.Interval = "O";
                //            caseIncomeEntity.Val1 = string.Empty;
                //            caseIncomeEntity.Val2 = string.Empty;
                //            caseIncomeEntity.Val3 = string.Empty;
                //            caseIncomeEntity.Val4 = string.Empty;
                //            caseIncomeEntity.Val5 = string.Empty;
                //            caseIncomeEntity.Date1 = string.Empty;
                //            caseIncomeEntity.Date2 = string.Empty;
                //            caseIncomeEntity.Date3 = string.Empty;
                //            caseIncomeEntity.Date4 = string.Empty;
                //            caseIncomeEntity.Date5 = string.Empty;

                //            caseIncomeEntity.LstcOperator = BaseForm.UserID;
                //            caseIncomeEntity.AddOperator = BaseForm.UserID;
                //            caseIncomeEntity.Mode ="Type";
                //            //txtSub.Text;                
                //            if (_model.CaseMstData.InsertCaseIncome(caseIncomeEntity))
                //            { }
                //        }
                //    }
                //}

            

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int Sel_Count = 0;
        private void gvwAlertCode_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
     
        /// <summary>
        /// Case Income Types filling functionality
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="privileges"></param>
        /// <param name="strAlertCode"></param>
        /// <param name="caseMst"></param>
        public CaseIncomeTypesForm(BaseForm baseForm, PrivilegeEntity privileges, CaseSnpEntity casesnp)
        {
            InitializeComponent();
            BaseForm = baseForm;
            this.Text = "Income Types";
            Privileges = privileges;
            _model = new CaptainModel();
            propType = "Income";
            propcasesnp = casesnp;
            programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (Privileges.AddPriv.Equals("true") || Privileges.ChangePriv.Equals("true"))
            {
                btnOk.Enabled = true;
            }
            fillIncomes();
        }

        void fillIncomes()
        {
            List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
            List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, propcasesnp.FamilySeq);
            bool boolexist = false;
            foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
            {
                boolexist = false;
                if (caseIncomeList.Count > 0)
                {
                    CaseIncomeEntity caseincometypes = caseIncomeList.Find(u => u.Type.Trim() == agyEntity.agycode);
                    if (caseincometypes != null)
                        boolexist = true;
                }

                int index = gvwIncomeTypes.Rows.Add(boolexist, agyEntity.agydesc, agyEntity.agycode, "None");

                gvwIncomeTypes.Rows[index].Tag = agyEntity;

            }

        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if (gvwIncomeTypes.Rows.Count > 0)
            {
                if (gvwIncomeTypes.CurrentRow.Index >= 0)
                {
                   
                    //if (MenuPropertie != string.Empty)
                    //{
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Add Additional Income Type Row";
                        menuItem.Tag = gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index].Tag;
                        contextMenu1.MenuItems.Add(menuItem);
                   // }
                    
                }
            }
        }

        private void gvwIncomeTypes_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (gvwIncomeTypes.Rows.Count > 0)
            {
               
                if (gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index].Tag is AgyTabEntity)
                {
                    // int index1 = dataGridCaseIncome.Rows.Add();
                    //  DataGridViewRow row = dataGridCaseIncome.Rows[index1];
                    AgyTabEntity drow = gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index].Tag as AgyTabEntity;
                    DataGridViewRow row = (DataGridViewRow)gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index].Clone();
                    // DataGridViewRow row = new DataGridViewRow();
                    row.Cells[2].Value = drow.agycode;
                    row.Cells[1].Value = drow.agydesc;
                    row.Cells[4].Value = "None";
                   
                    gvwIncomeTypes.Rows.Insert(gvwIncomeTypes.CurrentRow.Index + 1, row);
                    gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index + 1].Tag = drow;                  
                }
                else if (gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index].Tag is CaseIncomeEntity)
                {
                    // int index1 = dataGridCaseIncome.Rows.Add();
                    //  DataGridViewRow row = dataGridCaseIncome.Rows[index1];
                    CaseIncomeEntity drow = gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index].Tag as CaseIncomeEntity;
                    DataGridViewRow row = (DataGridViewRow)gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index].Clone();
                    // DataGridViewRow row = new DataGridViewRow();
                    row.Cells[2].Value = drow.Type;
                    row.Cells[1].Value = LookupDataAccess.GetLookUpCode("00004", drow.Type);//, LookupDataAccess.ShowIncomeInterval(caseIncome.Interval)
                    row.Cells[3].Value = "None";
                   
                    gvwIncomeTypes.Rows.Insert(gvwIncomeTypes.CurrentRow.Index + 1, row);
                    gvwIncomeTypes.Rows[gvwIncomeTypes.CurrentRow.Index + 1].Tag = drow;
                   
                }
                //gvwIncomeTypes.RefreshEdit();
                gvwIncomeTypes.Update();

            }
        }

       
    }
}