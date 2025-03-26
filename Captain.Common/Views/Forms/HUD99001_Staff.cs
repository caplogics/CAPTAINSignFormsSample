using Captain.Common.Exceptions;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using Captain.DatabaseLayer;
using DevExpress.Utils.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD99001_Staff : Form
    {

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public HUD99001_Staff(string form_Type, BaseForm baseForm, PrivilegeEntity privilege)
        {
            InitializeComponent();

            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _form_Type = form_Type;
            _baseForm = baseForm;
            _privilegeEntity = privilege;


            txtCnctZIP.Validator = TextBoxValidation.IntegerValidator;
            txtCounZIP.Validator = TextBoxValidation.IntegerValidator;

            txtCounRate.Validator = TextBoxValidation.CustomDecimalValidation13dot2;

            commonEntity_Lang = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "00353", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, "Add");
            commonEntity_Services = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "99025", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, "Add");

            Get_Form(_form_Type);

            fill_Staff_Mem_Grid();

            fillContactType();

            fillContactTitle();

            fillBillingMetod();

            Get_Contact_Details();

            Get_Counselor_Details();
        }

        private void Get_Counselor_Details()
        {
            fillBillingMetod();

      
            this.dgvHUDCouns.SelectionChanged -= new System.EventHandler(this.dgvHUDCouns_SelectionChanged);
            dgvHUDCouns.Rows.Clear();

            if (dgvStaffMem.Rows.Count > 0)
                staffEntity = _model.HUDCNTLData.GetHUDSTAFF(_baseForm.BaseAdminAgency, string.Empty, "2");
            //staffEntity = _model.HUDCNTLData.GetHUDSTAFF(_baseForm.BaseAdminAgency, dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim(), "2");

            int rowIndex = 0;

            if (staffEntity != null)
            {
                staffEntity = staffEntity.OrderBy(u => u.Staff_code).ToList();

                foreach (HUDSTAFFEntity entity in staffEntity)
                {
                    string billItems = string.Empty;

                    if (BillMethod.Count > 0 && entity.Bill_method.ToString().Trim() != "0")
                        billItems = BillMethod.Find(u => u.Code == entity.Bill_method.ToString().Trim()).Desc.ToString();

                    MaskedTextBox mskContactPhn = new MaskedTextBox();
                    mskContactPhn.Mask = "999-000-0000";
                    if (!string.IsNullOrEmpty(entity.Phone.ToString().Trim()))
                        mskContactPhn.Text = entity.Phone.ToString().Trim();

                    string _StaffCounsName = "";
                    List<DataGridViewRow> rowlst = dgvStaffMem.Rows.Where(x => x.Cells["Column2"].Value.ToString() == entity.Staff_code).ToList();
                    if (rowlst.Count > 0)
                        _StaffCounsName = rowlst[0].Cells["Column0"].Value.ToString() + " " + rowlst[0].Cells["Column1"].Value.ToString();

                    //rowIndex = dgvHUDCouns.Rows.Add(dgvStaffMem.SelectedRows[0].Cells["Column0"].Value.ToString().Trim() + " " + dgvStaffMem.SelectedRows[0].Cells["Column1"].Value.ToString().Trim(), entity.Rate, billItems, mskContactPhn.Text, entity.Email, entity.Seq.ToString().Trim(), staffMasterBtn);
                    rowIndex = dgvHUDCouns.Rows.Add(_StaffCounsName, entity.Rate, billItems, mskContactPhn.Text, entity.Email, entity.Seq.ToString().Trim(), staffMasterBtn, entity.Staff_code, rowlst[0].Cells["Column0"].Value.ToString(), rowlst[0].Cells["Column1"].Value.ToString());
                    dgvHUDCouns.Rows[rowIndex].Tag = entity;
                }

                this.dgvHUDCouns.SelectionChanged += new System.EventHandler(this.dgvHUDCouns_SelectionChanged);

                if (dgvHUDCouns.Rows.Count > 0)
                {
                    //dgvHUDCouns.Rows[0].Selected = true;

                    if (CounsRowIndex == dgvHUDCouns.Rows.Count)
                        CounsRowIndex = CounsRowIndex - 1;

                    dgvHUDCouns.Rows[CounsRowIndex].Selected = true;
                    dgvHUDCouns.CurrentCell = dgvHUDCouns.Rows[CounsRowIndex].Cells[1];

                    fillCounselors();
                }
                else
                {
                    ClearControls();
                    pbCounsDel.Visible = false;
                    pbCounsEdit.Visible = false;

                    lblCounsName.Text = "Counselor Name";
                }
            }
        }

        private void Get_Contact_Details()
        {
            fillContactType();

            fillContactTitle();
            this.dgvHUDContacts.SelectionChanged -= new System.EventHandler(this.dgvHUDContacts_SelectionChanged);
            dgvHUDContacts.Rows.Clear();

            if (dgvStaffMem.Rows.Count > 0)
                staffEntity = _model.HUDCNTLData.GetHUDSTAFF(_baseForm.BaseAdminAgency, string.Empty, "1");
                //staffEntity = _model.HUDCNTLData.GetHUDSTAFF(_baseForm.BaseAdminAgency, dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim(), "1");
            int rowIndex = 0;

            if (staffEntity != null)
            {
                staffEntity = staffEntity.OrderBy(u => u.Staff_code).ToList();

                foreach (HUDSTAFFEntity entity in staffEntity)
                {
                    string Contact_Type = string.Empty;

                    if (CnctType.Count > 0 && entity.Cont_type.ToString().Trim() != "0")
                        Contact_Type = CnctType.Find(u => u.Code == entity.Cont_type.ToString().Trim()).Desc.ToString();

                    string Contact_Title = string.Empty;

                    if (CnctType.Count > 0 && entity.Cont_title.ToString().Trim() != "0")
                        Contact_Title = CnctTitle.Find(u => u.Code == entity.Cont_title.ToString().Trim()).Desc.ToString();

                    MaskedTextBox mskContactPhn = new MaskedTextBox();
                    mskContactPhn.Mask = "999-000-0000";
                    if (!string.IsNullOrEmpty(entity.Phone.ToString().Trim()))
                        mskContactPhn.Text = entity.Phone.ToString().Trim();

                    string _StaffContName = "";
                    List<DataGridViewRow> rowlst = dgvStaffMem.Rows.Where(x => x.Cells["Column2"].Value.ToString() == entity.Staff_code).ToList();
                    if (rowlst.Count > 0)
                        _StaffContName = rowlst[0].Cells["Column0"].Value.ToString() + " " + rowlst[0].Cells["Column1"].Value.ToString();

                    //rowIndex = dgvHUDContacts.Rows.Add(dgvStaffMem.SelectedRows[0].Cells["Column0"].Value.ToString().Trim() + " " + dgvStaffMem.SelectedRows[0].Cells["Column1"].Value.ToString().Trim(), Contact_Type, Contact_Title, mskContactPhn.Text, entity.Email.ToString().Trim(), entity.Seq.ToString().Trim(), staffMasterBtn);
                    rowIndex = dgvHUDContacts.Rows.Add(_StaffContName, Contact_Type, Contact_Title, mskContactPhn.Text, entity.Email.ToString().Trim(), entity.Seq.ToString().Trim(), staffMasterBtn, entity.Staff_code, rowlst[0].Cells["Column0"].Value.ToString(), rowlst[0].Cells["Column1"].Value.ToString());
                    dgvHUDContacts.Rows[rowIndex].Tag = entity;
                }
                this.dgvHUDContacts.SelectionChanged += new System.EventHandler(this.dgvHUDContacts_SelectionChanged);
                if (dgvHUDContacts.Rows.Count > 0)
                {
                    //dgvHUDContacts.Rows[0].Selected = true;

                    if (ContactsRowIndex == dgvHUDContacts.Rows.Count)
                        ContactsRowIndex = ContactsRowIndex - 1;

                    dgvHUDContacts.Rows[ContactsRowIndex].Selected = true;
                    dgvHUDContacts.CurrentCell = dgvHUDContacts.Rows[ContactsRowIndex].Cells[1];

                    fillContacts();
                }
                else
                {
                    ClearControls();
                    pbCnctDel.Visible = false;
                    pbCnctEdit.Visible = false;

                    lblCnctName.Text = "Contact Name";
                }
            }
        }

        private void fillContacts()
        {
            if(dgvHUDContacts.Rows.Count > 0) 
            {
                HUDSTAFFEntity staffEntity= dgvHUDContacts.SelectedRows[0].Tag as HUDSTAFFEntity;
                if (staffEntity != null)
                {
                    CommonFunctions.SetComboBoxValue(cmbCnctType, staffEntity.Cont_type);
                    CommonFunctions.SetComboBoxValue(cmbCnctTitle, staffEntity.Cont_title);
                    txtCnctCity.Text = staffEntity.City;
                    txtCnctEmail.Text = staffEntity.Email;
                    txtCnctState.Text = staffEntity.State;
                    txtCnctStreet.Text = staffEntity.Street;
                    txtCnctZIP.Text = staffEntity.ZIP;
                    mtxtCnctSSN.Text = staffEntity.SSN;
                    mtxtCnctWPhn.Text = staffEntity.Phone;

                }
                
            }
        }

        List<CommonEntity> commonEntity_Lang = new List<CommonEntity>();
        List<CommonEntity> commonEntity_Services = new List<CommonEntity>();
        private void fillCounselors()
        {
            if (dgvHUDCouns.Rows.Count > 0)
            {
                //commonEntity_Lang.Clear();
                //commonEntity_Services.Clear();

                HUDSTAFFEntity staffEntity = dgvHUDCouns.SelectedRows[0].Tag as HUDSTAFFEntity;
                if (staffEntity != null)
                {
                    txtCounRate.Text = staffEntity.Rate;
                    CommonFunctions.SetComboBoxValue(cmbCounBilMethod, staffEntity.Bill_method);
                    txtCounFHA.Text = staffEntity.FHA_ID;

                    if(staffEntity.Active == "A") chkbCounActive.Checked = true; else chkbCounActive.Checked = false;
                    if(staffEntity.Cert == "Y") chkbCounCertified.Checked = true; else chkbCounCertified.Checked= false;

                    txtCounStreet.Text = staffEntity.Street;
                    txtCounCity.Text = staffEntity.City;
                    txtCounState.Text = staffEntity.State;
                    txtCounZIP.Text = staffEntity.ZIP;
                    mtxtCounWrkPhn.Text = staffEntity.Phone;
                    txtCounEmail.Text = staffEntity.Email;
                    ListcommonEntity_Sel_Lang = new List<CommonEntity>();
                    string[] lang = null;
                    string _langDesc = string.Empty;
                    if (!string.IsNullOrEmpty(staffEntity.Lang))
                    {
                        lang = staffEntity.Lang.Split(',');
                        
                        if (lang.Length > 0)
                        {
                            foreach (string language in lang)
                            {
                                List<CommonEntity> _lang = commonEntity_Lang.Where(x => x.Code == language).ToList();

                                if (_lang.Count > 0)
                                {
                                    _langDesc += _lang[0].Desc + ", ";
                                }
                                ListcommonEntity_Sel_Lang.Add( new CommonEntity(_lang[0].Code, _lang[0].Desc ) );    
                            }
                        }
                    }
                    Lang_Code = staffEntity.Lang;
                    txtCounLang.Text = _langDesc.Trim().TrimEnd(',');

                    ListcommonEntity_Sel_Service = new List<CommonEntity>();
                    string[] serv = null;
                    string _servDesc = string.Empty;
                    if (!string.IsNullOrEmpty(staffEntity.Service))
                    {
                        serv = staffEntity.Service.Split(',');

                        if (serv.Length > 0)
                        {
                            foreach (string services in serv)
                            {
                                List<CommonEntity> _serv = commonEntity_Services.Where(x => x.Code == services).ToList();

                                if (_serv.Count > 0)
                                {
                                    _servDesc += _serv[0].Desc + ", ";
                                }
                                ListcommonEntity_Sel_Service.Add(new CommonEntity(_serv[0].Code, _serv[0].Desc));
                            }
                        }
                    }
                    Serv_Code = staffEntity.Service;
                    txtCounServ.Text = _servDesc.Trim().TrimEnd(',');
                }

            }
        }

        List<CommonEntity> CnctType = new List<CommonEntity>();
        private void fillContactType()
        {
            cmbCnctType.Items.Clear();
            cmbCnctType.ColorMember = "FavoriteColor";

            CnctType = _model.lookupDataAccess.GetLookkupFronAGYTAB("99022");

            cmbCnctType.Items.Add(new ListItem("", "0"));

            foreach (CommonEntity entity in CnctType)
            {
                cmbCnctType.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbCnctType.Items.Count > 0)
                cmbCnctType.SelectedIndex = 0;
        }

        List<CommonEntity> CnctTitle = new List<CommonEntity>();
        private void fillContactTitle()
        {
            cmbCnctTitle.Items.Clear();
            cmbCnctTitle.ColorMember = "FavoriteColor";

            CnctTitle = _model.lookupDataAccess.GetLookkupFronAGYTAB("99023");

            cmbCnctTitle.Items.Add(new ListItem("", "0"));

            foreach (CommonEntity entity in CnctTitle)
            {
                cmbCnctTitle.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbCnctTitle.Items.Count > 0)
                cmbCnctTitle.SelectedIndex = 0;
        }

        List<CommonEntity> BillMethod = new List<CommonEntity>();
        private void fillBillingMetod()
        {
            cmbCounBilMethod.Items.Clear();
            BillMethod.Clear();
            BillMethod.AddRange(new List<CommonEntity>
                                {
                new CommonEntity("0",""),
                                    new CommonEntity("1", "Hourly" ),
                                    new CommonEntity("2", "Fixed" ),
                                });


            foreach (CommonEntity entity in BillMethod)
            {
                cmbCounBilMethod.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbCounBilMethod.Items.Add(new ListItem("Hourly", "1"));
            //cmbCounBilMethod.Items.Add(new ListItem("Fixed", "2"));

            if (cmbCounBilMethod.Items.Count > 0)
                cmbCounBilMethod.SelectedIndex = 0;
        }

        #region Properties

        public BaseForm _baseForm
        {
            get;
            set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get;
            set;
        }

        public string _form_Type
        {
            get;
            set;
        }

        public DataSet STAFF_Data
        {
            get;
            set;
        }

        public List<HUDSTAFFEntity> staffEntity
        {
            get; set;
        }
        public List<CommonEntity> ListcommonEntity_Sel_Lang
        {
            get; set;
        }
        public List<CommonEntity> ListcommonEntity_Sel_Service
        {
            get; set;
        }
        #endregion

        string img_Add = "captain-add";
        string staffMasterBtn = "sP_btn_dot";


        int StaffRowIndex = 0;
        int ContactsRowIndex = 0;
        int CounsRowIndex = 0;

        List<STAFFMSTEntity> STAFFMST_List = new List<STAFFMSTEntity>();
        private void fill_Staff_Mem_Grid()
        {
            dgvStaffMem.Rows.Clear();

            this.dgvStaffMem.SelectionChanged -= new System.EventHandler(this.dgvStaffMem_SelectionChanged);

            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
            STAFFMST_List = STAFFMST_List.OrderBy(u => u.Active).ToList();

            if (STAFFMST_List.Count > 0)
            {
                foreach (STAFFMSTEntity staffdetails in STAFFMST_List)
                {
                    int rowIndex = dgvStaffMem.Rows.Add(staffdetails.Staff_Code, LookupDataAccess.GetMemberName(staffdetails.First_Name, staffdetails.Middle_Name, staffdetails.Last_Name, string.Empty), staffdetails.HNo.Trim() + " " + staffdetails.Street.Trim() + " " + staffdetails.Suffix.Trim() + " " + staffdetails.City.Trim() + ", " + staffdetails.State.Trim() + "  " + SetLeadingZeros(staffdetails.Zip.Trim()) + " - " + "0000".Substring(0, 4 - staffdetails.Zip_Plus.Length) + staffdetails.Zip_Plus, img_Add, staffdetails.First_Name, staffdetails.Last_Name, staffdetails.Staff_Code.Trim());

                    dgvStaffMem.Rows[rowIndex].Tag = staffdetails;

                    if (!staffdetails.Active.Equals("A"))
                        dgvStaffMem.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (dgvStaffMem.Rows.Count > 0)
            {
                dgvStaffMem.Rows[StaffRowIndex].Selected = true;
                dgvStaffMem.CurrentCell = dgvStaffMem.Rows[StaffRowIndex].Cells[1];
            }
            this.dgvStaffMem.SelectionChanged += new System.EventHandler(this.dgvStaffMem_SelectionChanged);
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seqlen = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seqlen)
            {
                case 4:
                    TmpCode = "0" + TmpCode;
                    break;
                case 3:
                    TmpCode = "00" + TmpCode;
                    break;
                case 2:
                    TmpCode = "000" + TmpCode;
                    break;
                case 1:
                    TmpCode = "0000" + TmpCode;
                    break;
            }

            return (TmpCode);
        }

        private void Get_Form(string form_Type)
        {
            if (form_Type == "C")
            {
                this.Text = "HUD Contacts";

                pnlHUDCouns.Visible = false;
                pnlCounslers.Visible = false;

                pnlContacts.Visible = true;
                pnlHUDContacts.Visible = true;
                pnlHUDContacts.Dock = DockStyle.Fill;

                this.Size = new System.Drawing.Size(this.Width, 520);//690);

                this.dataGridViewImageColumn2.HeaderText = "Add Contact";

                pnlMiddle.Enabled = true;
                pnlContacts.Enabled = true;
                pnlContactButtons.Enabled = true;

                pnlCnctType.Enabled = false;
                pnlCnctAdd1.Enabled = false;
                pnlCnctAdd2.Enabled = false;
                pnlCnctWPhn.Enabled = false;
            }

            if (form_Type == "CO")
            {
                this.Text = "HUD Counselors";
                pnlHUDCouns.Visible = true;
                pnlCounslers.Visible = true;
                pnlCounslers.Dock = DockStyle.Fill;

                pnlContacts.Visible = false;
                pnlHUDContacts.Visible = false;

                //this.Size = new System.Drawing.Size(this.Width, 780);

                this.Size = new System.Drawing.Size(this.Width, 610);//780);

                this.dataGridViewImageColumn2.HeaderText = "Add Counselor";

                pnlMiddle.Enabled = true;
                pnlCounslers.Enabled = true;
                pnlCounsButtons.Enabled = true;

                pnlCounRate.Enabled = false;
                pnlCounPhone.Enabled = false;
                pnlCounActive.Enabled = false;
                pnlCounWorkAdd.Enabled = false;

                pnlCounLangSpoken.Enabled = false;
                pnlCounServProv.Enabled = false;
            }
        }

        private void pbCnctEdit_Click(object sender, EventArgs e)
        {
            cmbCnctType.Focus();
            _staffmode = "UPDATE";

            pnlMiddle.Enabled = true; pnlContacts.Enabled = true;
            pnlContactButtons.Enabled = true;

            pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctWPhn.Enabled = pnlCnctSave.Enabled = true;

            btnCnctCancel.Visible = btnCnctSave.Visible = true;
        }

        private void pbCounsEdit_Click(object sender, EventArgs e)
        {
            txtCounRate.Focus();
            _staffmode = "UPDATE";

            pnlCounsButtons.Enabled = false;
            pbCounsDel.Visible = pbCounsEdit.Visible = false;

            btnCounCancel.Visible = btnCounSave.Visible = true;

            pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounWorkAdd.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounSave.Enabled = true;

            pnlCounLangSpoken.Enabled = true;
            pnlCounServProv.Enabled = true;
        }

        private void dgvStaffMem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStaffMem.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 3)
                {
                    _staffmode = "INSERT";
                    
                    pnlMiddle.Enabled = true;
                    if (_form_Type == "C")
                    {
                        pnlMiddle.Enabled = true;
                        pnlContacts.Enabled = true;
                        pnlContactButtons.Enabled = true;

                        pbCnctEdit.Visible = pbCnctDel.Visible = false;

                        btnCnctCancel.Visible = btnCnctSave.Visible = true;

                        pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctSave.Enabled = pnlCnctWPhn.Enabled = true;

                        cmbCnctType.Focus();
                    }
                    else
                    {
                        pnlMiddle.Enabled = true;
                        pnlCounslers.Enabled = true;
                        pnlCounsButtons.Enabled = true;

                        pbCounsEdit.Visible = pbCounsDel.Visible = false;
                        btnCounCancel.Visible = btnCounSave.Visible = true;

                        pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounWorkAdd.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounSave.Enabled = true;

                        pnlCounLangSpoken.Enabled = true;
                        pnlCounServProv.Enabled = true;

                        txtCounRate.Focus();
                    }
                    ClearControls();
                }
        }
        }
        
        private void ClearControls()
        {
            if (_form_Type == "C")
            {

                txtCnctCity.Text = string.Empty;
                txtCnctEmail.Text = string.Empty;
                txtCnctState.Text = string.Empty;
                txtCnctStreet.Text = string.Empty;
                txtCnctZIP.Text = string.Empty;
                mtxtCnctSSN.Text = string.Empty;
                mtxtCnctWPhn.Text = string.Empty;

                cmbCnctType.SelectedIndex = 0;
                cmbCnctTitle.SelectedIndex = 0;
            }
            else
            {
                txtCounRate.Text = string.Empty;
                txtCounFHA.Text = string.Empty;
                txtCounStreet.Text = string.Empty;
                txtCounCity.Text = string.Empty;
                txtCounState.Text = string.Empty;
                txtCounZIP.Text = string.Empty;
                txtCounEmail.Text = string.Empty;
                mtxtCounWrkPhn.Text = string.Empty;
                chkbCounActive.Checked = false;
                chkbCounCertified.Checked = false;

                txtCounServ.Text = string.Empty;
                txtCounLang.Text = string.Empty;

                if (ListcommonEntity_Sel_Lang != null)
                    ListcommonEntity_Sel_Lang.Clear();

                if (ListcommonEntity_Sel_Service != null)
                    ListcommonEntity_Sel_Service.Clear();

                cmbCounBilMethod.SelectedIndex = 0;
            }
        }

        private void dgvHUDContacts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvHUDContacts.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 6)
                {
                    StaffRowIndex = dgvStaffMem.CurrentRow.Index;

                    ContactsRowIndex = dgvHUDContacts.CurrentRow.Index;

                    STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
                    Search_STAFFMST.Agency = _baseForm.BaseAgency;
                    Search_STAFFMST.Year = _baseForm.BaseYear;
                    if (dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim() != string.Empty)
                        Search_STAFFMST.Staff_Code = dgvHUDContacts.SelectedRows[0].Cells["Column5"].Value.ToString().Trim();//dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim();
                    if (dgvStaffMem.SelectedRows[0].Cells["Column1"].Value.ToString().Trim() != string.Empty)
                        Search_STAFFMST.Last_Name = dgvHUDContacts.SelectedRows[0].Cells["Column8"].Value.ToString().Trim();//dgvStaffMem.SelectedRows[0].Cells["Column1"].Value.ToString().Trim();

                    List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");

                    //STFMST10Form editUserForm = new STFMST10Form(_baseForm, "Edit", dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim(), _privilegeEntity, dgvStaffMem.SelectedRows[0].Tag as STAFFMSTEntity);
                    STFMST10Form editUserForm = new STFMST10Form(_baseForm, "Edit", dgvHUDContacts.SelectedRows[0].Cells["Column5"].Value.ToString().Trim(), _privilegeEntity, STAFFMST_List[0]);
                    editUserForm.FormClosed += new FormClosedEventHandler(On_Form_Closed);
                    editUserForm.StartPosition = FormStartPosition.CenterScreen;
                    editUserForm.ShowDialog();
                }
            }
        }

        private void dgvHUDCouns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvHUDCouns.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 6)
                {

                    StaffRowIndex = dgvStaffMem.CurrentRow.Index;
                    CounsRowIndex = dgvHUDCouns.CurrentRow.Index;

                    STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
                    Search_STAFFMST.Agency = _baseForm.BaseAgency;
                    Search_STAFFMST.Year = _baseForm.BaseYear;
                    if (dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim() != string.Empty)
                        Search_STAFFMST.Staff_Code = dgvHUDCouns.SelectedRows[0].Cells["Column6"].Value.ToString().Trim();//dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim();
                    if (dgvStaffMem.SelectedRows[0].Cells["Column1"].Value.ToString().Trim() != string.Empty)
                        Search_STAFFMST.Last_Name = dgvHUDCouns.SelectedRows[0].Cells["Column12"].Value.ToString().Trim();//dgvStaffMem.SelectedRows[0].Cells["Column1"].Value.ToString().Trim();

                    List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");

                    //STFMST10Form editUserForm = new STFMST10Form(_baseForm, "Edit", dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim(), _privilegeEntity, dgvStaffMem.SelectedRows[0].Tag as STAFFMSTEntity);
                    //STFMST10Form editUserForm = new STFMST10Form(_baseForm, "Edit", dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim(), _privilegeEntity, STAFFMST_List[0]);
                    STFMST10Form editUserForm = new STFMST10Form(_baseForm, "Edit", dgvHUDCouns.SelectedRows[0].Cells["Column6"].Value.ToString().Trim(), _privilegeEntity, STAFFMST_List[0]);
                    editUserForm.FormClosed += new FormClosedEventHandler(On_Form_Closed);
                    editUserForm.StartPosition = FormStartPosition.CenterScreen;
                    editUserForm.ShowDialog();
                }
            }
        }

        private void On_Form_Closed(object sender, FormClosedEventArgs e)
        {
            this.dgvStaffMem.SelectionChanged -= new System.EventHandler(this.dgvStaffMem_SelectionChanged);
            fill_Staff_Mem_Grid();

            //if (_form_Type == "CO")
            //    Get_Counselor_Details();
            //else
            //    Get_Contact_Details();

            dgvStaffMem_SelectionChanged(dgvStaffMem, e);

            this.dgvStaffMem.SelectionChanged += new System.EventHandler(this.dgvStaffMem_SelectionChanged);
        }

        private bool Validate_Contact_Details()
        {
            bool isValid = true;
            _errorProvider.SetError(cmbCnctType, null);
            _errorProvider.SetError(cmbCnctTitle, null);
            _errorProvider.SetError(mtxtCnctSSN, null);
            _errorProvider.SetError(txtCnctStreet, null);
            _errorProvider.SetError(txtCnctCity, null);
            _errorProvider.SetError(txtCnctZIP, null);
            _errorProvider.SetError(txtCnctState, null);
            _errorProvider.SetError(mtxtCnctWPhn, null);
            _errorProvider.SetError(txtCnctEmail, null);

            if (((Captain.Common.Utilities.ListItem)cmbCnctType.SelectedItem).Value.ToString().Trim() == "")
            {
                _errorProvider.SetError(cmbCnctType, "Contact Type is Required");
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbCnctType, null);

            if (((Captain.Common.Utilities.ListItem)cmbCnctTitle.SelectedItem).Value.ToString().Trim() == "")
            {
                _errorProvider.SetError(cmbCnctTitle, "Contact Title is Required");
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbCnctTitle, null);

            if (mtxtCnctSSN.Text != "   -  -")
            {
                if (mtxtCnctSSN.Text.Length > 0 && mtxtCnctSSN.Text.Length < 9)
                {
                    _errorProvider.SetError(mtxtCnctSSN, "Please Enter Contact SSN in Correct Format");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(mtxtCnctSSN, null);
            }
            else
            {
                if (mtxtCnctSSN.Text == "   -  -")
                {
                    _errorProvider.SetError(mtxtCnctSSN, "Contact SSN is Required");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(mtxtCnctSSN, null);
            }

            if (string.IsNullOrEmpty(txtCnctStreet.Text.Trim()))
            {
                _errorProvider.SetError(txtCnctStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCnctStreet.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCnctStreet, null);

            if (string.IsNullOrEmpty(txtCnctCity.Text.Trim()))
            {
                _errorProvider.SetError(txtCnctCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCnctCity.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCnctCity, null);

            if (string.IsNullOrEmpty(txtCnctZIP.Text.Trim()))
            {
                _errorProvider.SetError(txtCnctZIP, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCnctZIP.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCnctZIP, null);

            if (string.IsNullOrEmpty(txtCnctState.Text.Trim()))
            {
                _errorProvider.SetError(txtCnctState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCnctState.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCnctState, null);

            if (txtCnctEmail.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtCnctEmail.Text, Consts.StaticVars.EmailValidatingString))
                {
                    _errorProvider.SetError(txtCnctEmail, Consts.Messages.PleaseEnterEmail);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtCnctEmail, null);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtCnctEmail.Text.Trim()))
                {
                    _errorProvider.SetError(txtCnctEmail, "Work Email Address is Required");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtCnctEmail, null);
            }

            if (mtxtCnctWPhn.Text != "" && mtxtCnctWPhn.Text != "   -   -")
            {
                if (mtxtCnctWPhn.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(mtxtCnctWPhn, "Please enter a valid Work Phone#");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(mtxtCnctWPhn, null);
                }
            }
            else
            {
                if (mtxtCnctWPhn.Text == "   -   -")
                {
                    _errorProvider.SetError(mtxtCnctWPhn, "Work Phone# is Required");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(mtxtCnctWPhn, null);
            }

            if (staffEntity.Count > 0 && _staffmode == "INSERT")
            {
                staffEntity = staffEntity.Where(x => x.Agy == _baseForm.BaseAdminAgency && x.Staff_code == dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim() && x.Cont_type == ((Captain.Common.Utilities.ListItem)cmbCnctType.SelectedItem).Value.ToString().Trim()).ToList();
                
                if (staffEntity.Count > 0)
                {
                    isValid = false;
                    AlertBox.Show("Staff member already exists for the same contact type", MessageBoxIcon.Warning);
                }
            }


            return isValid;
        }

        string _staffmode = string.Empty;
        int prevSeq = 0;
        private void btnCnctSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(cmbCnctType, null);
            _errorProvider.SetError(cmbCnctTitle, null);
            _errorProvider.SetError(mtxtCnctSSN, null);
            _errorProvider.SetError(txtCnctStreet, null);
            _errorProvider.SetError(txtCnctCity, null);
            _errorProvider.SetError(txtCnctZIP, null);
            _errorProvider.SetError(txtCnctState, null);
            _errorProvider.SetError(mtxtCnctWPhn, null);
            _errorProvider.SetError(txtCnctEmail, null);

            try
            {
                string seq = "1";
                if (Validate_Contact_Details())
                {
                    HUDSTAFFEntity hUDStaffEntity = new HUDSTAFFEntity();
                    //if (((Captain.Common.Utilities.ListItem)cmbCnctType.SelectedItem).Value.ToString().Trim() != prevSeq.ToString())
                    //{
                    hUDStaffEntity.Agy = _baseForm.BaseAdminAgency;

                    if (_staffmode == "INSERT")
                        hUDStaffEntity.Staff_code = dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim();
                    else
                        hUDStaffEntity.Staff_code = dgvHUDContacts.SelectedRows[0].Cells["Column5"].Value.ToString().Trim();
                    hUDStaffEntity.Type = "1";

                    if (_staffmode == "INSERT")
                        seq = "1";
                    else
                        seq = dgvHUDContacts.SelectedRows[0].Cells["Column3"].Value.ToString().Trim();//**dgvStaffMem.SelectedRows[0].Cells["Column3"].Value.ToString().Trim();  //Column3  

                    //if (((Captain.Common.Utilities.ListItem)cmbCnctType.SelectedItem).Value.ToString().Trim() == "1")
                    //    seq = 1;
                    //else if (((Captain.Common.Utilities.ListItem)cmbCnctType.SelectedItem).Value.ToString().Trim() == "2")
                    //    seq = 2;
                    //else
                    //    seq = 3;

                    hUDStaffEntity.Seq = seq;

                    hUDStaffEntity.Cont_type = ((Captain.Common.Utilities.ListItem)cmbCnctType.SelectedItem).Value.ToString().Trim();
                    hUDStaffEntity.Cont_title = ((Captain.Common.Utilities.ListItem)cmbCnctTitle.SelectedItem).Value.ToString().Trim();
                    hUDStaffEntity.SSN = mtxtCnctSSN.Text == "" ? "" : mtxtCnctSSN.Text.Trim().Replace("-", "");
                    hUDStaffEntity.Street = txtCnctStreet.Text == "" ? "" : txtCnctStreet.Text.Trim();
                    hUDStaffEntity.City = txtCnctCity.Text == "" ? "" : txtCnctCity.Text.Trim();
                    hUDStaffEntity.State = txtCnctState.Text == "" ? "" : txtCnctState.Text.Trim();
                    hUDStaffEntity.ZIP = txtCnctZIP.Text == "" ? "" : txtCnctZIP.Text.Trim();
                    hUDStaffEntity.Phone = mtxtCnctWPhn.Text == "" ? "" : mtxtCnctWPhn.Text.Trim().Replace("-", "");
                    hUDStaffEntity.Email = txtCnctEmail.Text == "" ? "" : txtCnctEmail.Text.Trim();

                    hUDStaffEntity.Add_Operator = _baseForm.UserID;
                    hUDStaffEntity.Lstc_Operator = _baseForm.UserID;

                    if (_model.HUDCNTLData.InsertUpdateHUDSTAFF(hUDStaffEntity, _staffmode.ToUpper()))
                    {
                        if (_staffmode.ToUpper() == "INSERT")
                            AlertBox.Show("Saved Successfully");
                        else
                            AlertBox.Show("Updated Successfully");

                        pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctWPhn.Enabled = pnlCnctSave.Enabled = false;

                        pnlMiddle.Enabled = true;
                        pnlContacts.Enabled = true;
                        pnlContactButtons.Enabled = true;

                        pbCnctEdit.Visible = pbCnctDel.Visible = true;

                        btnCnctCancel.Visible = btnCnctSave.Visible = false;

                        if (dgvHUDContacts.Rows.Count > 0)
                            ContactsRowIndex = dgvHUDContacts.CurrentRow.Index;
                    }
                    //prevSeq = seq;

                    Get_Contact_Details();
                    //}
                    //else
                    //{
                    //    AlertBox.Show("Staff member already exists for the same contact type", MessageBoxIcon.Warning);
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void btnCnctCancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(cmbCnctType, null);
            _errorProvider.SetError(cmbCnctTitle, null);
            _errorProvider.SetError(mtxtCnctSSN, null);
            _errorProvider.SetError(txtCnctStreet, null);
            _errorProvider.SetError(txtCnctCity, null);
            _errorProvider.SetError(txtCnctZIP, null);
            _errorProvider.SetError(txtCnctState, null);
            _errorProvider.SetError(mtxtCnctWPhn, null);
            _errorProvider.SetError(txtCnctEmail, null);

            if (dgvHUDContacts.Rows.Count > 0)
            {
                pnlMiddle.Enabled = true;
                pnlContacts.Enabled = true;
                pnlContactButtons.Enabled = true;

                pbCnctEdit.Visible = pbCnctDel.Visible = true;

                btnCnctCancel.Visible = btnCnctSave.Visible = false;

                pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctSave.Enabled = pnlCnctWPhn.Enabled = false;

                fillContacts();
            }
            else
            {
                ClearControls();
                pbCnctEdit.Visible = pbCnctDel.Visible = false;
                btnCnctCancel.Visible = btnCnctSave.Visible = false;

                pnlMiddle.Enabled = true;
                pnlContacts.Enabled = true;
                pnlContactButtons.Enabled = true;

                pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctSave.Enabled = pnlCnctWPhn.Enabled = false;

                lblCnctName.Text = "Contact Name";
            }
        }

        private bool Validate_Counselor_Details()
        {
            bool isValid = true;
            _errorProvider.SetError(txtCounRate, null);
            _errorProvider.SetError(cmbCounBilMethod, null);
            _errorProvider.SetError(txtCounFHA, null);
            _errorProvider.SetError(txtCounStreet, null);
            _errorProvider.SetError(txtCounCity, null);
            _errorProvider.SetError(txtCounZIP, null);
            _errorProvider.SetError(txtCounState, null);
            _errorProvider.SetError(mtxtCounWrkPhn, null);
            _errorProvider.SetError(txtCounEmail, null);

            if (string.IsNullOrEmpty(txtCounRate.Text.Trim()))
            {
                _errorProvider.SetError(txtCounRate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounsRate.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCounRate, null);

            if (((Captain.Common.Utilities.ListItem)cmbCounBilMethod.SelectedItem).Value.ToString().Trim() == "")
            {
                _errorProvider.SetError(cmbCounBilMethod, "Contact Type is Required");
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbCounBilMethod, null);

            if (string.IsNullOrEmpty(txtCounFHA.Text.Trim()))
            {
                _errorProvider.SetError(txtCounFHA, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounFHA.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCounFHA, null);

            if (string.IsNullOrEmpty(txtCounStreet.Text.Trim()))
            {
                _errorProvider.SetError(txtCounStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounStreet.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCounStreet, null);

            if (string.IsNullOrEmpty(txtCounCity.Text.Trim()))
            {
                _errorProvider.SetError(txtCounCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounCity.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCounCity, null);

            if (string.IsNullOrEmpty(txtCounZIP.Text.Trim()))
            {
                _errorProvider.SetError(txtCounZIP, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounZIP.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCounZIP, null);

            if (string.IsNullOrEmpty(txtCounState.Text.Trim()))
            {
                _errorProvider.SetError(txtCounState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounState.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCounState, null);

            if (txtCounEmail.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtCounEmail.Text, Consts.StaticVars.EmailValidatingString))
                {
                    _errorProvider.SetError(txtCounEmail, Consts.Messages.PleaseEnterEmail);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtCounEmail, null);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtCounEmail.Text.Trim()))
                {
                    _errorProvider.SetError(txtCounEmail, "Work Email Address is Required");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtCounEmail, null);
            }

            if (mtxtCounWrkPhn.Text != "" && mtxtCounWrkPhn.Text != "   -   -")
            {
                if (mtxtCounWrkPhn.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(mtxtCounWrkPhn, "Please enter a valid Work Phone#");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(mtxtCounWrkPhn, null);
                }
            }
            else
            {
                if (mtxtCounWrkPhn.Text == "   -   -")
                {
                    _errorProvider.SetError(mtxtCounWrkPhn, "Work Phone# is Required");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(mtxtCounWrkPhn, null);
            }

            if (string.IsNullOrEmpty(txtCounLang.Text))
            {
                _errorProvider.SetError(pbLangSel, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounLangSpoken.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(pbLangSel, null);

            if (string.IsNullOrEmpty(txtCounServ.Text))
            {
                _errorProvider.SetError(pbServProv, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounServ.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(pbServProv, null);


            return isValid;
        }

        private void btnCounSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtCounRate, null);
            _errorProvider.SetError(cmbCounBilMethod, null);
            _errorProvider.SetError(txtCounFHA, null);
            _errorProvider.SetError(txtCounStreet, null);
            _errorProvider.SetError(txtCounCity, null);
            _errorProvider.SetError(txtCounZIP, null);
            _errorProvider.SetError(txtCounState, null);
            _errorProvider.SetError(mtxtCounWrkPhn, null);
            _errorProvider.SetError(txtCounEmail, null);

            try
            {
                string seq = "1";
                if (Validate_Counselor_Details())
                {
                    HUDSTAFFEntity hUDStaffEntity = new HUDSTAFFEntity();

                    hUDStaffEntity.Agy = _baseForm.BaseAdminAgency;
                    if (_staffmode == "INSERT")
                        hUDStaffEntity.Staff_code = dgvStaffMem.SelectedRows[0].Cells["Column2"].Value.ToString().Trim();
                    else
                        hUDStaffEntity.Staff_code = dgvHUDCouns.SelectedRows[0].Cells["Column6"].Value.ToString().Trim();

                    hUDStaffEntity.Type = "2";

                    if (_staffmode == "INSERT")
                        seq = "1";
                    else
                        seq = dgvHUDCouns.SelectedRows[0].Cells["Column4"].Value.ToString().Trim();

                    hUDStaffEntity.Seq = seq;

                    hUDStaffEntity.Rate = txtCounRate.Text == "" ? "" : txtCounRate.Text.Trim();
                    hUDStaffEntity.Bill_method = ((Captain.Common.Utilities.ListItem)cmbCounBilMethod.SelectedItem).Value.ToString().Trim();
                    hUDStaffEntity.FHA_ID = txtCounFHA.Text == "" ? "" : txtCounFHA.Text.Trim();

                    hUDStaffEntity.Active = chkbCounActive.Checked ? "A" : "I";
                    hUDStaffEntity.Cert = chkbCounCertified.Checked ? "Y" : "N";

                    hUDStaffEntity.Street = txtCounStreet.Text == "" ? "" : txtCounStreet.Text.Trim();
                    hUDStaffEntity.City = txtCounCity.Text == "" ? "" : txtCounCity.Text.Trim();
                    hUDStaffEntity.State = txtCounState.Text == "" ? "" : txtCounState.Text.Trim();
                    hUDStaffEntity.ZIP = txtCounZIP.Text == "" ? "" : txtCounZIP.Text.Trim();
                    hUDStaffEntity.Phone = mtxtCounWrkPhn.Text == "" ? "" : mtxtCounWrkPhn.Text.Trim().Replace("-", "");
                    hUDStaffEntity.Email = txtCounEmail.Text == "" ? "" : txtCounEmail.Text.Trim();

                    hUDStaffEntity.Lang = Lang_Code;
                    hUDStaffEntity.Service = Serv_Code;

                    hUDStaffEntity.Add_Operator = _baseForm.UserID;
                    hUDStaffEntity.Lstc_Operator = _baseForm.UserID;

                    if (_model.HUDCNTLData.InsertUpdateHUDSTAFF(hUDStaffEntity, _staffmode.ToUpper()))
                    {
                        if (_staffmode.ToUpper() == "INSERT")
                            AlertBox.Show("Saved Successfully");
                        else
                            AlertBox.Show("Updated Successfully");

                        pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounWorkAdd.Enabled = false;
                        pnlCounSave.Enabled = false;
                        pnlCounsButtons.Enabled = true;

                        pbCounsEdit.Visible = pbCounsDel.Visible = true;
                        btnCounCancel.Visible = btnCounSave.Visible = false;

                        pnlCounLangSpoken.Enabled = false;
                        pnlCounServProv.Enabled = false;

                        if(dgvHUDCouns.Rows.Count > 0)
                         CounsRowIndex = dgvHUDCouns.CurrentRow.Index;
                    }
                    Get_Counselor_Details();
                }
            }
            catch(Exception ex) { }
        }

        private void btnCounCancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtCounRate, null);
            _errorProvider.SetError(cmbCounBilMethod, null);
            _errorProvider.SetError(txtCounFHA, null);
            _errorProvider.SetError(txtCounStreet, null);
            _errorProvider.SetError(txtCounCity, null);
            _errorProvider.SetError(txtCounZIP, null);
            _errorProvider.SetError(txtCounState, null);
            _errorProvider.SetError(mtxtCounWrkPhn, null);
            _errorProvider.SetError(txtCounEmail, null);

            if (dgvHUDCouns.Rows.Count > 0)
            {
                pnlMiddle.Enabled = true;
                pnlCounslers.Enabled = true;
                pnlCounsButtons.Enabled = true;

                pbCounsEdit.Visible = pbCounsDel.Visible = true;
                btnCounCancel.Visible = btnCounSave.Visible = false;

                pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounWorkAdd.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounSave.Enabled = false;

                pnlCounLangSpoken.Enabled = false;
                pnlCounServProv.Enabled = false;

                fillCounselors();
            }
            else
            {
                ClearControls();
                btnCounCancel.Visible = btnCounSave.Visible = false;
                pbCounsDel.Visible = pbCounsEdit.Visible = false;

                pnlMiddle.Enabled = true;
                pnlCounslers.Enabled = true;
                pnlCounsButtons.Enabled = true;

                pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounWorkAdd.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounSave.Enabled = false;

                lblCounsName.Text = "Counselor Name";
            }           
        }

        private void pbLangSel_Click(object sender, EventArgs e)
        {
            SelectZipSiteCountyForm countyform = new SelectZipSiteCountyForm(_baseForm, ListcommonEntity_Sel_Lang, "Lang");
            countyform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
            countyform.StartPosition = FormStartPosition.CenterScreen;
            countyform.ShowDialog();
        }

        string Lang_Code = string.Empty;
        string Serv_Code = string.Empty;
        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;

            if (form.DialogResult == DialogResult.OK)
            {
                if (form.FormType == "Lang")
                {
                    txtCounLang.Text = string.Empty;
                    ListcommonEntity_Sel_Lang = form.SelectedTargetEntity;
                    Lang_Code = string.Empty;
                    foreach (CommonEntity entity in ListcommonEntity_Sel_Lang)
                    {
                        txtCounLang.Text = txtCounLang.Text + entity.Desc + ", ";

                        Lang_Code = Lang_Code + entity.Code + ",";
                    }
                    txtCounLang.Text = txtCounLang.Text.Trim().TrimEnd(',');
                    Lang_Code = Lang_Code.Trim().TrimEnd(',');
                }
                else if (form.FormType == "Services")
                {
                    txtCounServ.Text = string.Empty;
                    ListcommonEntity_Sel_Service = form.SelectedTargetEntity;
                    Serv_Code = string.Empty;
                    foreach (CommonEntity entity in ListcommonEntity_Sel_Service)
                    {
                        txtCounServ.Text = txtCounServ.Text + entity.Desc + ", ";

                        Serv_Code = Serv_Code + entity.Code + ",";
                    }
                    txtCounServ.Text = txtCounServ.Text.Trim().TrimEnd(',');
                    Serv_Code = Serv_Code.Trim().TrimEnd(',');
                }
            }
        }

        private void pbServProv_Click(object sender, EventArgs e)
        {
            SelectZipSiteCountyForm countyform = new SelectZipSiteCountyForm(_baseForm, ListcommonEntity_Sel_Service, "Services");
            countyform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
            countyform.StartPosition = FormStartPosition.CenterScreen;
            countyform.ShowDialog();
        }

        private void dgvStaffMem_SelectionChanged(object sender, EventArgs e)
        {
            if (_form_Type == "C")
            {
                Get_Contact_Details();

                if (dgvHUDContacts.Rows.Count > 0)
                {
                    pnlMiddle.Enabled = true;
                    pnlContacts.Enabled = true;
                    pnlContactButtons.Enabled = true;

                    pbCnctEdit.Visible = pbCnctDel.Visible = true;

                    btnCnctCancel.Visible = btnCnctSave.Visible = false;

                    pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctSave.Enabled = pnlCnctWPhn.Enabled = false;
                }
                else
                {
                    pbCnctEdit.Visible = pbCnctDel.Visible = false;
                    btnCnctCancel.Visible = btnCnctSave.Visible = false;

                    pnlMiddle.Enabled = true;
                    pnlContacts.Enabled = true;
                    pnlContactButtons.Enabled = true;

                    pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctSave.Enabled = pnlCnctWPhn.Enabled = false;
                }
            }
            if (_form_Type == "CO")
            {

                Get_Counselor_Details();

                if (dgvHUDCouns.Rows.Count > 0)
                {
                    pnlMiddle.Enabled = true;
                    pnlCounslers.Enabled = true;
                    pnlCounsButtons.Enabled = true;

                    pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounWorkAdd.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounSave.Enabled = false;

                    pbCounsDel.Visible = pbCounsEdit.Visible = true;
                    btnCounCancel.Visible = btnCounSave.Visible = false;

                    pnlCounLangSpoken.Enabled = false;
                    pnlCounServProv.Enabled = false;
                }
                else
                {
                    pbCounsDel.Visible = pbCounsEdit.Visible = false;
                    btnCounCancel.Visible = btnCounSave.Visible = false;

                    pnlMiddle.Enabled = true;
                    pnlCounslers.Enabled = true;
                    pnlCounsButtons.Enabled = true;

                    pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounWorkAdd.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounSave.Enabled = false;
                }
            }
        }

        private void dgvHUDContacts_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvHUDContacts.Rows.Count > 0)
            {
                fillContacts();

                lblCnctName.Text = string.Empty;
                if (dgvHUDContacts.SelectedRows[0].Cells["Column7"].Value.ToString().Trim() != "" || dgvHUDContacts.SelectedRows[0].Cells["Column8"].Value.ToString().Trim() != "")
                    lblCnctName.Text = "Contact Name" + ": " + dgvHUDContacts.SelectedRows[0].Cells["Column7"].Value.ToString().Trim() + " " + dgvHUDContacts.SelectedRows[0].Cells["Column8"].Value.ToString().Trim();
                else
                    lblCnctName.Text = "Contact Name";
            }
        }

        private void dgvHUDCouns_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHUDCouns.Rows.Count > 0)
            {
                fillCounselors();

                lblCounsName.Text = string.Empty;
                if (dgvHUDCouns.SelectedRows[0].Cells["Column11"].Value.ToString().Trim() != "" || dgvHUDCouns.SelectedRows[0].Cells["Column12"].Value.ToString().Trim() != "")
                    lblCounsName.Text = "Counselor Name" + ": " + dgvHUDCouns.SelectedRows[0].Cells["Column11"].Value.ToString().Trim() + " " + dgvHUDCouns.SelectedRows[0].Cells["Column12"].Value.ToString().Trim();
                else
                    lblCounsName.Text = "Counselor Name";
            }
        }

        private void pbCounsDel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Counselor_Record);
        }

        private void Delete_Counselor_Record(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                HUDSTAFFEntity hUDSTAFFEntity = new HUDSTAFFEntity();

                hUDSTAFFEntity.Agy = _baseForm.BaseAdminAgency;
                hUDSTAFFEntity.Staff_code = dgvHUDCouns.SelectedRows[0].Cells["Column6"].Value.ToString().Trim();
                hUDSTAFFEntity.Type = "2";
                hUDSTAFFEntity.Seq = dgvHUDCouns.CurrentRow.Cells["Column4"].Value.ToString().Trim();

                if (_model.HUDCNTLData.InsertUpdateHUDSTAFF(hUDSTAFFEntity, ""))
                {
                    AlertBox.Show("Deleted Successfully");
                    Get_Counselor_Details();
                }
            }
        }

        private void pbCnctDel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Contact_Record);
        }

        private void Delete_Contact_Record(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                HUDSTAFFEntity hUDSTAFFEntity = new HUDSTAFFEntity();

                hUDSTAFFEntity.Agy = _baseForm.BaseAdminAgency;
                hUDSTAFFEntity.Staff_code = dgvHUDContacts.SelectedRows[0].Cells["Column5"].Value.ToString().Trim();
                hUDSTAFFEntity.Type = "1";
                hUDSTAFFEntity.Seq = dgvHUDContacts.CurrentRow.Cells["Column3"].Value.ToString().Trim();

                if (_model.HUDCNTLData.InsertUpdateHUDSTAFF(hUDSTAFFEntity, ""))
                {
                    AlertBox.Show("Deleted Successfully");
                    Get_Contact_Details();
                }
            }
        }

        STAFFMSTEntity selEntity = new STAFFMSTEntity();
        public string _selStaff = "";
        int selStaffMem = 0;
        private void Staff_Mem_FormClosed(object sender, FormClosedEventArgs e)
        {
            HUD99001_Staff_Members form = sender as HUD99001_Staff_Members;

            if (form.DialogResult == DialogResult.OK)
            {
                selEntity = form.selStaffMem;

                selStaffMem = form.currerntIndex;

                _selStaff = form.SelectedStaffMem();

                if (dgvStaffMem.Rows.Count > 0)
                {
                    DataGridViewRow drstaff = dgvStaffMem.Rows.FirstOrDefault(x => x.Cells["Column2"].Value.Equals(_selStaff));

                    if (drstaff != null)
                    {
                        drstaff.Selected = true;

                        dgvStaffMem.CurrentCell = drstaff.Cells[1];

                        dgvStaffMem_SelectionChanged(this, EventArgs.Empty);
                    }
                }

                if (_form_Type == "C")
                {
                    pnlMiddle.Enabled = true;
                    pnlContacts.Enabled = true;
                    pnlContactButtons.Enabled = true;

                    pbCnctEdit.Visible = pbCnctDel.Visible = false;

                    btnCnctCancel.Visible = btnCnctSave.Visible = true;

                    pnlCnctType.Enabled = pnlCnctAdd1.Enabled = pnlCnctAdd2.Enabled = pnlCnctSave.Enabled = pnlCnctWPhn.Enabled = true;

                    cmbCnctType.Focus();

                    lblCnctName.Text = string.Empty;
                    if (_staffmode == "INSERT")
                    {
                        lblCnctName.Text = "Contact Name" + ": " + selEntity.First_Name + " " + selEntity.Last_Name;
                    }
                }
                else
                {
                    pnlMiddle.Enabled = true;
                    pnlCounslers.Enabled = true;
                    pnlCounsButtons.Enabled = true;

                    pbCounsEdit.Visible = pbCounsDel.Visible = false;
                    btnCounCancel.Visible = btnCounSave.Visible = true;

                    pnlCounRate.Enabled = pnlCounActive.Enabled = pnlCounWorkAdd.Enabled = pnlCounCity.Enabled = pnlCounPhone.Enabled = pnlCounSave.Enabled = true;

                    pnlCounLangSpoken.Enabled = true;
                    pnlCounServProv.Enabled = true;

                    txtCounRate.Focus();

                    lblCounsName.Text = string.Empty;
                    if (_staffmode == "INSERT")
                    {
                        lblCounsName.Text = "Counselor Name" + ": " + selEntity.First_Name + " " + selEntity.Last_Name;
                    }
                }

                if (_staffmode == "INSERT")
                    ClearControls();
            }
        }

        private void pbCnctAdd_Click(object sender, EventArgs e)
        {
            _staffmode = "INSERT";

            HUD99001_Staff_Members staff_mem_form = new HUD99001_Staff_Members(_baseForm, _privilegeEntity/*, _privilegeEntity.Program*/);
            staff_mem_form.FormClosed += new FormClosedEventHandler(Staff_Mem_FormClosed);
            staff_mem_form.StartPosition = FormStartPosition.CenterScreen;
            staff_mem_form.ShowDialog();
        }

        private void pbCounsAdd_Click(object sender, EventArgs e)
        {
            _staffmode = "INSERT";

            HUD99001_Staff_Members staff_mem_form = new HUD99001_Staff_Members(_baseForm, _privilegeEntity/*, _privilegeEntity.Program*/);
            staff_mem_form.FormClosed += new FormClosedEventHandler(Staff_Mem_FormClosed);
            staff_mem_form.StartPosition = FormStartPosition.CenterScreen;
            staff_mem_form.ShowDialog();
        }
    }
}
