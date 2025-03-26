#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls.Base;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ApplicationDetailsControl : UserControl
    {
        private CaptainModel _model = null;
        public BaseForm BaseForm { get; set; }
        public string strAgency { get; set; }
        public string strDept { get; set; }
        public string strProg { get; set; }
        public string strYear { get; set; }
        public string strApplicationNo { get; set; }
        
        public ApplicationDetailsControl(BaseForm baseForm)
        {
            InitializeComponent();           
            _model = new CaptainModel();
            BaseForm = baseForm;
        }

        public void ClearGridData()
        {
            dataGridAppNo.Rows.Clear();
        }



        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        public string FillGridData(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            //Agency = "03";
            //Dept = "01";
            //Program = "01";
            //Year = "2012";
            //AppNo = "20110310";
            string strCheck = Checkdata(Agency, Dept, Program);
            List<CaseMstEntity> caseMstList = _model.CaseMstData.GetCaseMstadpyn(Agency, Dept, Program, Year, AppNo);
            List<CaseSnpEntity> caseSnpList = _model.CaseMstData.GetCaseSnpadpyn(Agency, Dept, Program, Year, AppNo);
            BaseForm.BaseCaseMstListEntity = caseMstList;
            BaseForm.BaseCaseSnpEntity = caseSnpList;
            BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A"; 
            //dataGridAppNo.Rows.Clear();
            //foreach (CaseSnpEntity caseSnp in caseSnpList)
            //{
            //    string name = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, strNameFormat);
            //    string strAltDate = LookupDataAccess.Getdate(caseSnp.AltBdate);
            //    //string strSsno = LookupDataAccess.GetCardNo(caseSnp.Ssno, "1");
            //    int rowIndex = dataGridAppNo.Rows.Add(name, strAltDate);
            //    dataGridAppNo.Rows[rowIndex].Tag = caseSnp;
            //    dataGridAppNo.ItemsPerPage = 100;
            //}
            return strCheck;

        }
        private string Checkdata(string Agency, string Dept, string Program)
        {

            HierarchyEntity HierarchyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyName(Agency, "**", "**");
            BaseForm.BaseHierarchyCnFormat = string.Empty;
            BaseForm.BaseHierarchyCwFormat = string.Empty;
            if (HierarchyEntity != null)
            {
                strNameFormat = HierarchyEntity.CNFormat.ToString();
                strVerfierFormat = HierarchyEntity.CWFormat.ToString();
                BaseForm.BaseHierarchyCnFormat = HierarchyEntity.CNFormat.ToString();
                BaseForm.BaseHierarchyCwFormat = HierarchyEntity.CWFormat.ToString();
            }           
            return strNameFormat;
        }
                
    }
}