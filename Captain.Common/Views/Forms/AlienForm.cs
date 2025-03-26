using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.DatabaseLayer;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class AlienForm : _iForm
    {
        public BaseForm _baseForm { get; set; }
        public PrivilegeEntity _privilegeEntity { get; set; }
        public string _AppNo = "";
        AlienTXData oAlienTXData = null;
        public AlienForm(BaseForm baseForm, PrivilegeEntity privileges, string AppNo,string SentApp)
        {
            InitializeComponent();
            oAlienTXData = new AlienTXData();
            _baseForm = baseForm;
            _privilegeEntity = privileges;
            _AppNo=AppNo;
            this.Text = "SAVE Form";
            if (SentApp == "Y") lblEmail.Text = "Email: " + baseForm.BaseCaseMstListEntity[0].Email.Trim(); else lblEmail.Text = "";
            LoadAlignUsers();
        }

        void LoadAlignUsers()
        {
            DataTable _dtAlienVer = AlienTXDB.GET_ALIENVER("", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg,_baseForm.BaseYear, _AppNo, "GET");
            List<CommonEntity> ResidentEntity = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, Consts.AgyTab.RESIDENTCODES,
                   _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg); //_model.lookupDataAccess.GetResident();

            List<CommonEntity> ReasonEnity = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0010", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg); //_model.AdhocData.Browse_AGYTABS(searchAgytabs);


            List<CaseSnpEntity> _lstSnpEntity = _baseForm.BaseCaseSnpEntity.FindAll(u => u.Status.Trim().ToUpper() == "A");
            foreach (CaseSnpEntity SNPitem in _lstSnpEntity)
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                
                string _Agy = SNPitem.Agency;
                string _Dept = SNPitem.Dept;
                string _Prog = SNPitem.Program;
                string _Year = SNPitem.Year;
                string _Appno = SNPitem.App;
                string _FamSeq = SNPitem.FamilySeq;
                string _Name = LookupDataAccess.GetMemberName(ti.ToTitleCase(SNPitem.NameixFi.ToLower()), "", ti.ToTitleCase(SNPitem.NameixLast.ToLower()), "1");
                string _DOB = string.Empty;
                if(!string.IsNullOrEmpty(SNPitem.AltBdate.Trim()))
                    _DOB = Convert.ToDateTime(SNPitem.AltBdate).ToString("MM/dd/yyyy");
                string _Age = SNPitem.Age;
                string _Resident = "";// SNPitem.ResidentDesc;

                List<CommonEntity> Residentlst = ResidentEntity.FindAll(x => x.Code == SNPitem.Resident).ToList();
                if (Residentlst.Count > 0)
                    _Resident = Residentlst[0].Desc;

                string _strCitizen = "";
                string _strIsIdentity = "";

                if (_dtAlienVer.Rows.Count > 0) {

                    DataRow[] drFamMem = _dtAlienVer.Select("ALN_VER_FAM_SEQ='" + _FamSeq + "'");
                    if (drFamMem.Length > 0)
                    {
                        _strCitizen = drFamMem[0]["ALN_VER_CITIZEN"].ToString();
                        _strIsIdentity = drFamMem[0]["ALN_VER_IDENT"].ToString();
                    }

                }

                string _ssn = "000-00-0000";
                if(SNPitem.Ssno!="")
                    _ssn = LookupDataAccess.GetPhoneSsnNoFormat(SNPitem.Ssno);
                string _ssnReason = ""; 
                List<CommonEntity> Reasonlst = ReasonEnity.FindAll(x => x.Code == SNPitem.SsnReason).ToList();
                if (Reasonlst.Count > 0)
                    _ssnReason = Reasonlst[0].Desc;
                //  gvAlienUsers.Rows.Add(_Agy, _Dept, _Prog,_Year,_Appno,_FamSeq, _Name, _DOB, _Age, _Resident, _strCitizen, _strIsIdentity);
                gvAlienUsers.Rows.Add(_Agy, _Dept, _Prog, _Year, _Appno, _FamSeq, _Name,_ssn, _ssnReason, _Resident, _DOB, _Age, _strCitizen, _strIsIdentity);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            bool isSaveFlag = false;
            foreach(DataGridViewRow _dRow in gvAlienUsers.Rows)
            {
                string _Agy = _dRow.Cells["colAgy"].Value.ToString();
                string _Dept = _dRow.Cells["colDept"].Value.ToString();
                string _Prog = _dRow.Cells["ColProg"].Value.ToString();
                string _Year = _dRow.Cells["ColYear"].Value.ToString();
                string _Appno = _dRow.Cells["ColAppno"].Value.ToString();
                string _FamSeq = _dRow.Cells["ColFamSeq"].Value.ToString();
                string _strCitizen = _dRow.Cells["ColCitzen"].Value == null ? "" : _dRow.Cells["ColCitzen"].Value.ToString();
                string _strIsIdentity = _dRow.Cells["Colidentify"].Value == null ? "" : _dRow.Cells["Colidentify"].Value.ToString();
                string _Operator = _baseForm.UserID;

                isSaveFlag = oAlienTXData.INSUPDEL_ALIENVER("",_Agy, _Dept, _Prog, _Year,_AppNo,_FamSeq,_strCitizen, _strIsIdentity, _Operator,"ADD");
            }
            if (isSaveFlag)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }
    }
}
