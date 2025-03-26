#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class EnrollHierarchiesForm : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public EnrollHierarchiesForm(BaseForm baseForm, string mode, DepEnrollHierachiesEntity enrollEntity,List<string> ListHierchyes)
        {
            InitializeComponent();
            _model = new CaptainModel();
            Mode = mode;
            BaseForm = baseForm;
            EnrollEntity = enrollEntity;
            txtNofoSlots.Validator = TextBoxValidation.IntegerValidator;
            this.Text = "Enroll Hierachies - " + Mode ;
            PropListHierchyes = ListHierchyes;
            if (enrollEntity != null)
            {
                Agency = enrollEntity.Agency;
                Dept = enrollEntity.Dept;
                Program = enrollEntity.Program;

            }
            if (Mode.Equals("Edit"))
            {
                PbHierarchies.Visible = false;
                fillEnrollForm();
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public DepEnrollHierachiesEntity EnrollEntity { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Program { get; set; }

        public string SEQ { get; set; }

        public List<string> PropListHierchyes { get; set; }

        private void fillEnrollForm()
        {
            //List<CaseDepContactEntity> contactEntity = _model.HierarchyAndPrograms.GetCASEDEPContacts(ContactEntity);
            if (EnrollEntity != null)
            {
                txtHierarchy.Text = EnrollEntity.DepEnrollCode.Trim();
                txtHierachydesc.Text = EnrollEntity.DepEnrollDesc.Trim();
                txtNofoSlots.Text = EnrollEntity.Nofoslots;
                if (EnrollEntity.StartDate != string.Empty)
                {
                    dtstartDate.Value = Convert.ToDateTime(EnrollEntity.StartDate);
                    dtstartDate.Checked = true;
                }
                if (EnrollEntity.Enddate != string.Empty)
                {
                    dtEndDate.Value = Convert.ToDateTime(EnrollEntity.Enddate);
                    dtEndDate.Checked = true;
                }
                         
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtHierarchy.Text.Trim()))
            {
                _errorProvider.SetError(txtHierachydesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHierarchy.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                string strHierarchy = txtHierarchy.Text;
                strHierarchy = strHierarchy.Substring(0, 2) + strHierarchy.Substring(3, 2) + strHierarchy.Substring(6, 2); ;

                if (Mode.Equals("Add"))
                {
                    if (PropListHierchyes.Contains(strHierarchy))
                    {
                        _errorProvider.SetError(txtHierachydesc, "Hierachy already exist");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtHierachydesc, null);
                    }
                }
            }

            if (String.IsNullOrEmpty(txtNofoSlots.Text.Trim()))
            {
                _errorProvider.SetError(txtNofoSlots, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNofoSlots.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtNofoSlots, null);
            }

            

            if (dtstartDate.Checked==false)
            {
                _errorProvider.SetError(dtstartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtstartDate, null);
            }

            if (dtEndDate.Checked == false)
            {
                _errorProvider.SetError(dtEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEndDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtEndDate, null);
            }

            if (dtstartDate.Checked.Equals(true) && dtEndDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtstartDate.Text))
                {
                    _errorProvider.SetError(dtstartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Start Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtstartDate, null);
                }
                if (string.IsNullOrWhiteSpace(dtEndDate.Text))
                {
                    _errorProvider.SetError(dtEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "End Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtEndDate, null);
                }
            }

            if (dtstartDate.Checked.Equals(true) && dtEndDate.Checked.Equals(true))
            {
                if (Convert.ToDateTime(dtstartDate.Text) > Convert.ToDateTime(dtEndDate.Text))
                {
                    _errorProvider.SetError(dtstartDate, "Start Date should be less than or equal to End Date");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(dtstartDate, null);
            }

            return (isValid);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValidateForm())
                {
                    DepEnrollHierachiesEntity EnrollEntityList = new DepEnrollHierachiesEntity();
                    EnrollEntityList.Agency = Agency;
                    EnrollEntityList.Dept = Dept;
                    EnrollEntityList.Program = Program;                   
                    EnrollEntityList.DepEnrollCode = txtHierarchy.Text;
                    EnrollEntityList.DepEnrollDesc = txtHierachydesc.Text;
                    string strHierarchy =  txtHierarchy.Text;
                    EnrollEntityList.Hierachies = strHierarchy.Substring(0, 2) + strHierarchy.Substring(3, 2) + strHierarchy.Substring(6, 2); ;
                    EnrollEntityList.StartDate = dtstartDate.Value.ToShortDateString();
                    EnrollEntityList.Enddate = dtEndDate.Value.ToShortDateString();
                    EnrollEntityList.Nofoslots = txtNofoSlots.Text;
                    if (Mode.Equals("Edit")) EnrollEntityList.Mode = "Update";
                    else EnrollEntityList.Mode = "Insert";
                    EnrollEntityList.DepAddOperator = BaseForm.UserID;
                    EnrollEntityList.DepLstcOperator = BaseForm.UserID;

                    //if (_model.HierarchyAndPrograms.InsertCaseDepContact(contactEntity))
                    //{
                    //AddProgramContactForm programControl = BaseForm.GetBaseUserControl() as AddProgramContactForm;
                    //if (programControl != null)
                    //{
                    //    programControl.RefreshContactGrid();
                    //}
                    EnrollEntity = EnrollEntityList;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Master",string.Empty,"A","I");
            HierarchieSelection addForm = new HierarchieSelection(BaseForm, string.Empty, "Master", "E", "A", "I",BaseForm.UserID);
            addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            addForm.StartPosition = FormStartPosition.CenterScreen;
            addForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            //HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    hierarchy += row.Agency + row.Dept + row.Prog;
                    txtHierarchy.Text = row.Code;
                    txtHierachydesc.Text = row.HirarchyName;                   
                }
            }

        }

      
    }
}