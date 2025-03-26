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
    public partial class CASEVERHISTORY : Form
    {
        private CaptainModel _model = null;

        #region Properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity privilege { get; set; }     
        public ProgramDefinitionEntity ProgramDefination { get; set; }
        public CaseMstEntity CaseMSTEntity { get; set; }
        public ProgramDefinitionEntity programDefination { get; set; }
        public List<HierarchyEntity> hierarchyEntity { get; set; }
        public List<CommonEntity> MealEntity { get; set; }
        public string propProgramheadswitch { get; set; }
        #endregion
        public CASEVERHISTORY(BaseForm baseForm,PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            privilege = privilegeEntity;
            //this.Text = privilege.PrivilegeName.Trim() + " - " + "Income Verification History";
            this.Text = "Income Verification History Details";
            fillGridData();
        }

        private void fillGridData()
        {



            List<CaseVerEntity> caseVerList = _model.CaseMstData.GetCASEVeradpyalst(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty);
            dataGridCaseIncomeVer.Rows.Clear();
         
            foreach (CaseVerEntity caseVer in caseVerList)
            {

                string strAltDate = LookupDataAccess.Getdate(caseVer.VerifyDate);
               // VerifyCheckDate = LookupDataAccess.Getdate(caseVerList[0].VerifyDate);
                int rowIndex = dataGridCaseIncomeVer.Rows.Add(strAltDate, caseVer.IncomeAmount, caseVer.NoInhh, caseVer.VerOmb + "%", caseVer.VerCmi + "%", caseVer.VerSmi + "%", caseVer.VerHud + "%");
                
                dataGridCaseIncomeVer.Rows[rowIndex].Tag = caseVer;
                
                CommonFunctions.setTooltip(rowIndex, caseVer.AddOperator, caseVer.DateAdd, caseVer.LstcOperator, caseVer.DateLstc, dataGridCaseIncomeVer);
            }
            if (caseVerList.Count == 0)
            {

            }
            else
            {

            }

        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Application.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2001_Income");
        }
    }
}