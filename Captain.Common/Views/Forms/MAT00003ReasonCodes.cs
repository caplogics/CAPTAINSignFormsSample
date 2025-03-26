/************************************************************************
 * Conversion On    :   12/14/2022      * Converted By     :   Kranthi
 * Modified On      :   12/14/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MAT00003ReasonCodes : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();

        private string strMatCode = string.Empty;
        private string strMode = string.Empty;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;




        int strIndex = 0;
        public MAT00003ReasonCodes(BaseForm baseForm, PrivilegeEntity privilieges, string matCode, string sclCode, string ssDate, List<MATASMTEntity> MataSmtSvalues, MATASMTEntity MatasmtPreviousDate, string strDateType)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            ChangeValue = 0;
            Privileges = privilieges;
            strMatCode = matCode;
            strMode = string.Empty;
            ScoreSheet = string.Empty;
            SSDate = ssDate;
            ScrCode = sclCode;
            MatCode = matCode;
            propDateType = strDateType;
            matasmtScaleTypeSDetails = MataSmtSvalues;
            mataSmtPreviousDate = MatasmtPreviousDate;
            HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(BaseForm.BaseAgency, "**", "**");
            if (HierarchyEntity != null)
            {
                strNameFormat = HierarchyEntity.CNFormat.ToString();
                strVerfierFormat = HierarchyEntity.CWFormat.ToString();
            }


            fillcombo();
            dtAssessmentDate.Text = SSDate;
            dtAssessmentDate.Enabled = false;
            cmbMatrix.Enabled = false;
            cmbScale.Enabled = false;
            CommonFunctions.SetComboBoxValue(cmbMatrix, MatCode);
            cmbMatrix_SelectedIndexChanged(cmbMatrix, new EventArgs());
            CommonFunctions.SetComboBoxValue(cmbScale, ScrCode);
            List<MATAPDTSEntity> matapdtsList = _model.MatrixScalesData.GETMatapdts(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, MatCode, string.Empty, strVerfierFormat);

            MATASMTEntity MatResponcebypass = matasmtScaleTypeSDetails.Find(u => u.QuesCode.Equals("0") && u.Type.Equals("S"));
            if (MatResponcebypass != null)
            {
                if (!string.IsNullOrEmpty(MatResponcebypass.Change))
                    ChangeValue = Convert.ToInt32(MatResponcebypass.Change);
                if (ChangeValue > 0)
                    lblChangeDimesion.Text = "Change in Dimension +" + ChangeValue;
                else
                    lblChangeDimesion.Text = "Change in Dimension " + ChangeValue;
                if (propDateType == "A")
                {
                    MATAPDTSEntity matastype = matapdtsList.Find(u => LookupDataAccess.Getdate(u.SSDate).Equals(LookupDataAccess.Getdate(MatResponcebypass.SSDate)));

                    gvwReasons.Rows.Add(matastype.Name, LookupDataAccess.Getdate(MatResponcebypass.SSDate), MatResponcebypass.Points);
                }
                else
                {
                    gvwReasons.Rows.Add(string.Empty, LookupDataAccess.Getdate(MatResponcebypass.SSDate), MatResponcebypass.Points);
                }
                MATAPDTSEntity matasPrevioustype = matapdtsList.Find(u => LookupDataAccess.Getdate(u.SSDate).Equals(LookupDataAccess.Getdate(mataSmtPreviousDate.SSDate)));
                if (matasPrevioustype != null)
                {
                    gvwReasons.Rows.Add(matasPrevioustype.Name, LookupDataAccess.Getdate(mataSmtPreviousDate.SSDate), mataSmtPreviousDate.Points);
                    if (ChangeValue > 0)
                        Get_Reasons_List("P");
                    else
                        Get_Reasons_List("N");
                }
                //string strSixreason = MatResponcebypass.SixReasons;
                //if (strSixreason.Length == 18)
                //{
                //    CommonFunctions.SetComboBoxValue(cmbReason1, strSixreason.Substring(0, 3));
                //    CommonFunctions.SetComboBoxValue(cmbReason2, strSixreason.Substring(3, 3));
                //    CommonFunctions.SetComboBoxValue(cmbReason3, strSixreason.Substring(6, 3));
                //    CommonFunctions.SetComboBoxValue(cmbReason4, strSixreason.Substring(9, 3));
                //    CommonFunctions.SetComboBoxValue(cmbReason5, strSixreason.Substring(12, 3));
                //    CommonFunctions.SetComboBoxValue(cmbReason6, strSixreason.Substring(15, 3));
                //}


            }

            this.Text = Privileges.Program + " - Reason Codes";
        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        public List<MATASMTEntity> matasmtRespcode { get; set; }
        public List<MATASMTEntity> matasmtScaleDetails { get; set; }
        public List<MATASMTEntity> matasmtScaleTypeSDetails { get; set; }
        public MATASMTEntity mataSmtPreviousDate { get; set; }
        public List<MATDEFBMEntity> MATADEFBMentity { get; set; }
        public string MatCode { get; set; }
        public string ScrCode { get; set; }
        public string ScoreSheet { get; set; }
        public string SSDate { get; set; }
        public string QuestionType { get; set; }
        public string GroupCode { get; set; }
        public int ChangeValue { get; set; }
        public string propDateType { get; set; }

        //  public string ScrCode { get; set; }
        #endregion

        List<MATREASNEntity> Reasons_List = new List<MATREASNEntity>();
        private void Get_Reasons_List(string strPNType)
        {
            cmbReason1.Items.Clear();
            cmbReason2.Items.Clear();
            cmbReason3.Items.Clear();
            cmbReason4.Items.Clear();
            cmbReason5.Items.Clear();
            cmbReason6.Items.Clear();
            MATREASNEntity Search_Entity = new MATREASNEntity(true);
            Search_Entity.MatCode = MatCode;
            Search_Entity.Scl_Code = ScrCode;
            Search_Entity.PN = strPNType;
            Reasons_List = _model.MatrixScalesData.Browse_MATREASN(Search_Entity, "Browse");
            if (Reasons_List.Count > 0)
            {
                foreach (MATREASNEntity matReason in Reasons_List)
                {
                    cmbReason1.Items.Add(new ListItem(matReason.Desc, matReason.Code));
                    cmbReason2.Items.Add(new ListItem(matReason.Desc, matReason.Code));
                    cmbReason3.Items.Add(new ListItem(matReason.Desc, matReason.Code));
                    cmbReason4.Items.Add(new ListItem(matReason.Desc, matReason.Code));
                    cmbReason5.Items.Add(new ListItem(matReason.Desc, matReason.Code));
                    cmbReason6.Items.Add(new ListItem(matReason.Desc, matReason.Code));
                }
                cmbReason1.Items.Insert(0, new ListItem("None", "000"));
                cmbReason1.SelectedIndex = 0;
                cmbReason2.Items.Insert(0, new ListItem("None", "000"));
                cmbReason2.SelectedIndex = 0;
                cmbReason3.Items.Insert(0, new ListItem("None", "000"));
                cmbReason3.SelectedIndex = 0;
                cmbReason4.Items.Insert(0, new ListItem("None", "000"));
                cmbReason4.SelectedIndex = 0;
                cmbReason5.Items.Insert(0, new ListItem("None", "000"));
                cmbReason5.SelectedIndex = 0;
                cmbReason6.Items.Insert(0, new ListItem("None", "000"));
                cmbReason6.SelectedIndex = 0;
            }
            else
            {
                cmbReason1.Items.Insert(0, new ListItem("None", "000"));
                cmbReason1.SelectedIndex = 0;
                cmbReason2.Items.Insert(0, new ListItem("None", "000"));
                cmbReason2.SelectedIndex = 0;
                cmbReason3.Items.Insert(0, new ListItem("None", "000"));
                cmbReason3.SelectedIndex = 0;
                cmbReason4.Items.Insert(0, new ListItem("None", "000"));
                cmbReason4.SelectedIndex = 0;
                cmbReason5.Items.Insert(0, new ListItem("None", "000"));
                cmbReason5.SelectedIndex = 0;
                cmbReason6.Items.Insert(0, new ListItem("None", "000"));
                cmbReason6.SelectedIndex = 0;
            }
        }
        public void fillcombo()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Scale_Code = "0";
            List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");

            foreach (MATDEFEntity matdef in matdefEntity)
            {
                cmbMatrix.Items.Add(new ListItem(matdef.Desc, matdef.Mat_Code, matdef.Score, string.Empty));
            }
            cmbMatrix.Items.Insert(0, new ListItem("    ", "0"));
            cmbMatrix.SelectedIndex = 0;

        }

        private void cmbMatrix_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMatrix.SelectedIndex != 0)
            {
                cmbScale.Items.Clear();
                MatCode = ((ListItem)cmbMatrix.SelectedItem).Value.ToString();
                ScoreSheet = ((ListItem)cmbMatrix.SelectedItem).ID.ToString();
                List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Get_Matdef_MatCode(((ListItem)cmbMatrix.SelectedItem).Value.ToString());
                foreach (MATDEFEntity matdef in matdefEntity)
                {
                    cmbScale.Items.Add(new ListItem(matdef.Desc, matdef.Scale_Code));
                }
                cmbScale.Items.Insert(0, new ListItem("    ", "0"));
                cmbScale.SelectedIndex = 0;
            }
            else
            {
                cmbScale.Items.Clear();
                cmbScale.Items.Insert(0, new ListItem("    ", "0"));
                cmbScale.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                MATASMTEntity matasmtentity = new MATASMTEntity();
                matasmtentity.Agency = BaseForm.BaseAgency;
                matasmtentity.Dept = BaseForm.BaseDept;
                matasmtentity.Program = BaseForm.BaseProg;
                matasmtentity.Year = BaseForm.BaseYear;
                matasmtentity.App = BaseForm.BaseApplicationNo;
                matasmtentity.MatCode = MatCode;
                matasmtentity.SclCode = ScrCode;
                matasmtentity.Type = "S";
                matasmtentity.SSDate = dtAssessmentDate.Value.ToString();
                matasmtentity.SixReasons = ((ListItem)cmbReason1.SelectedItem).Value.ToString() + ((ListItem)cmbReason2.SelectedItem).Value.ToString() + ((ListItem)cmbReason3.SelectedItem).Value.ToString() + ((ListItem)cmbReason4.SelectedItem).Value.ToString() + ((ListItem)cmbReason5.SelectedItem).Value.ToString() + ((ListItem)cmbReason6.SelectedItem).Value.ToString();
                if (ChangeValue > 0)
                    matasmtentity.Pn = "P";
                else if (ChangeValue == 0)
                    matasmtentity.Pn = string.Empty;
                else
                    matasmtentity.Pn = "N";
                matasmtentity.QuesCode = "0";
                matasmtentity.AddOperator = BaseForm.UserID;
                matasmtentity.LstcOperator = BaseForm.UserID;
                matasmtentity.Mode = "Reason";

                if (_model.MatrixScalesData.InsertUpdateDelMatasmt(matasmtentity))
                {
                    this.Close();
                }
                AlertBox.Show("Saved Successfully");
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            string strReason1 = string.Empty;
            string strReason2 = string.Empty;
            string strReason3 = string.Empty;
            string strReason4 = string.Empty;
            string strReason5 = string.Empty;
            string strReason6 = string.Empty;
            if (cmbReason1.Items.Count > 0)
                strReason1 = ((ListItem)cmbReason1.SelectedItem).Value.ToString();
            if (cmbReason2.Items.Count > 0)
                strReason2 = ((ListItem)cmbReason2.SelectedItem).Value.ToString();
            if (cmbReason3.Items.Count > 0)
                strReason3 = ((ListItem)cmbReason3.SelectedItem).Value.ToString();
            if (cmbReason4.Items.Count > 0)
                strReason4 = ((ListItem)cmbReason4.SelectedItem).Value.ToString();
            if (cmbReason5.Items.Count > 0)
                strReason5 = ((ListItem)cmbReason5.SelectedItem).Value.ToString();
            if (cmbReason6.Items.Count > 0)
                strReason6 = ((ListItem)cmbReason6.SelectedItem).Value.ToString();
            if (ChangeValue != 0)
            {
                if ((((ListItem)cmbReason1.SelectedItem).Value.ToString() == "000") && (((ListItem)cmbReason2.SelectedItem).Value.ToString() == "000") && (((ListItem)cmbReason3.SelectedItem).Value.ToString() == "000") && (((ListItem)cmbReason4.SelectedItem).Value.ToString() == "000") && (((ListItem)cmbReason5.SelectedItem).Value.ToString() == "000") && (((ListItem)cmbReason6.SelectedItem).Value.ToString() == "000"))
                {
                    _errorProvider.SetError(lblChangeDTitle, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblChangeDTitle.Text));
                    isValid = false;
                }
                else
                {

                    if (strReason1 != "000" && strReason1 == strReason2)
                    {
                        _errorProvider.SetError(lblChangeDTitle, "Duplicate Reason has been selected");
                        isValid = false;
                    }

                    if (strReason1 != "000" && strReason1 == strReason3)
                    {
                        _errorProvider.SetError(lblChangeDTitle, "Duplicate Reason has been selected");
                        isValid = false;
                    }

                    if (strReason1 != "000" && strReason1 == strReason4)
                    {
                        _errorProvider.SetError(lblChangeDTitle, "Duplicate Reason has been selected");
                        isValid = false;
                    }

                    if (strReason1 != "000" && strReason1 == strReason5)
                    {
                        _errorProvider.SetError(lblChangeDTitle, "Duplicate Reason has been selected");
                        isValid = false;
                    }

                    if (strReason1 != "000" && strReason1 == strReason6)
                    {
                        _errorProvider.SetError(lblChangeDTitle, "Duplicate Reason has been selected");
                        isValid = false;
                    }
                    if (isValid)
                        _errorProvider.SetError(lblChangeDTitle, null);

                }
            }
            if (dtAssessmentDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtAssessmentDate.Text))
                {
                    _errorProvider.SetError(dtAssessmentDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtAssessmentDate, null);
                }
            }
            return (isValid);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MAT00003ReasonCodes_Load(object sender, EventArgs e)
        {
            cmbReason1.Focus();
        }

    }
}