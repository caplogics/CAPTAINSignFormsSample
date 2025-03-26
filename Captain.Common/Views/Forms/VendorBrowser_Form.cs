#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class VendorBrowser_From : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private string[] strCode = null;
        public int strIndex = 0;
        #endregion

        public VendorBrowser_From(BaseForm baseform, PrivilegeEntity privileges, string source_type)
        {
            InitializeComponent();
            propFormType = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privileges;
            Source_Type = source_type;
            this.Text = Privileges.PrivilegeName + " - Vendor Browse";
            _LLRNAME = string.Empty;

            //ADDED BY SUDHEER ON 05/25/2021
            userPrivilege1 = new PrivilegeEntity();
            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID("01", BaseForm.UserID, string.Empty);
                if (userPrivilege.Count > 0)
                {
                    userPrivilege1 = userPrivilege.Find(u => u.Program == "TMS00009");
                    //if (userPrivilege1 != null)
                    //{
                    //    if (userPrivilege1.AddPriv.ToUpper() == "TRUE")
                    //    { btnAdd.Visible = true; }
                    //    if (userPrivilege1.ChangePriv.ToUpper() == "TRUE")
                    //    { btnEdit.Visible = true; }
                    //}
                }
            }
            if (Privileges.ModuleCode == "05" || Privileges.ModuleCode == "10")
            {
                cmbSource.Visible = false; lblSource.Visible = false;
            }
            else
            {
                cmbSource.Visible = true; lblSource.Visible = true;
            }
            fillcomboxhardcode();
            fillCmbSources();
            SetComboBoxValue(cmbSource, Source_Type);
            if (Privileges.ModuleCode == "05")
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
               // this.btnSearch.Location = new System.Drawing.Point(794, 30);
                // fillVendors();
            }

        }

        string _LLRNAME = string.Empty;
        public VendorBrowser_From(BaseForm baseform, PrivilegeEntity privileges, string source_type, string LLRNAME)
        {
            InitializeComponent();
            propFormType = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privileges;
            Source_Type = source_type;
            this.Text = Privileges.PrivilegeName + " - Vendor Browse";

            if (!string.IsNullOrEmpty(LLRNAME.Trim()))
            {
                _LLRNAME = LLRNAME;

                txtName.Text = LLRNAME;
                txtName.Enabled = false;
                cmbSource.Enabled = false;
            }

            //ADDED BY SUDHEER ON 05/25/2021
            userPrivilege1 = new PrivilegeEntity();
            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID("01", BaseForm.UserID, string.Empty);
                if (userPrivilege.Count > 0)
                {
                    userPrivilege1 = userPrivilege.Find(u => u.Program == "TMS00009");
                    //if (userPrivilege1 != null)
                    //{
                    //    if (userPrivilege1.AddPriv.ToUpper() == "TRUE")
                    //    { btnAdd.Visible = true; }
                    //    if (userPrivilege1.ChangePriv.ToUpper() == "TRUE")
                    //    { btnEdit.Visible = true; }
                    //}
                }

                List<PrivilegeEntity> userPrivileges = _model.UserProfileAccess.GetScreensByUserID(BaseForm.BusinessModuleID, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
                SerVendPrivileges = Privileges;
                SerVendPrivileges = userPrivileges.Find(u => u.Program == "CASE9016");
            }
            if (Privileges.ModuleCode == "05" || Privileges.ModuleCode == "10")
            {
                cmbSource.Visible = false; lblSource.Visible = false;
            }
            else
            {
                cmbSource.Visible = true; lblSource.Visible = true;
                listView_Vendor.Columns[2].Text = "Contact Name";
            }
            fillcomboxhardcode();
            fillCmbSources();
            Fillcombos();
            SetComboBoxValue(cmbSource, Source_Type);
            if(!string.IsNullOrEmpty(Source_Type.Trim()))
                cmbSource.Enabled = false;
            if (Privileges.ModuleCode == "05")
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                this.btnSearch.Location = new System.Drawing.Point(794, 30);
                // fillVendors();
            }
            btnSearch_Click(btnSearch, EventArgs.Empty);

        }

        //public Test1(BaseForm baseform, PrivilegeEntity privileges, string source_type, string FormType)
        //{
        //    InitializeComponent();
        //    propFormType = FormType;
        //    _errorProvider = new ErrorProvider(this);
        //    _errorProvider.BlinkRate = 3;
        //    _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
        //    _errorProvider.Icon = null;
        //    _model = new CaptainModel();

        //    BaseForm = baseform;
        //    Privileges = privileges;
        //    Source_Type = source_type;
        //    this.Text = Privileges.Program + " -Vendor Browse";
        //    //if (rbNum.Checked)
        //    //{
        //    //    this.txtName.Size = new System.Drawing.Size(78, 20);
        //    //    this.txtName.MaxLength = 10;
        //    //}

        //    if (Privileges.ModuleCode == "05" || Privileges.ModuleCode == "10")
        //    {
        //        cmbSource.Visible = false; lblSource.Visible = false;
        //    }
        //    else
        //    {
        //        cmbSource.Enabled = false;
        //        cmbSource.Visible = true; lblSource.Visible = true;
        //    }
        //    fillCmbSources();
        //    SetComboBoxValue(cmbSource, Source_Type);




        //    if (FormType == "MULTIPLE")
        //    {
        //        //fillVendorsGrid();
        //    }
        //    else
        //    {
        //        fillVendors();
        //    }


        //}


        public VendorBrowser_From(BaseForm baseform, PrivilegeEntity privileges, string source_type, string FormType, List<string> strVendorList)
        {
            InitializeComponent();
            propFormType = FormType;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            propVendorList = strVendorList;
            BaseForm = baseform;
            Privileges = privileges;
            Source_Type = source_type;
            this.Text = Privileges.PrivilegeName + " - Vendor Browse";
            //if (rbNum.Checked)
            //{
            //    this.txtName.Size = new System.Drawing.Size(78, 20);
            //    this.txtName.MaxLength = 10;
            //}

            if (Privileges.ModuleCode == "05" || Privileges.ModuleCode == "10")
            {
                cmbSource.Visible = false; lblSource.Visible = false;
            }
            else
            {
                cmbSource.Enabled = false;
                cmbSource.Visible = true; lblSource.Visible = true;
            }
            fillCmbSources();
            SetComboBoxValue(cmbSource, Source_Type);



            if (FormType == "MULTIPLE")
            {
                //fillVendorsGrid();
            }
            else
            {
                fillVendors();
            }


        }




        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Blank = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");
        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = Consts.Icons.ico_Tick;



        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Source_Type { get; set; }
        public string propFormType { get; set; }
        public List<string> propVendorList { get; set; }

        //Added by Sudheer on 05/18/2021
        public PrivilegeEntity userPrivilege1 { get; set; }

        public PrivilegeEntity SerVendPrivileges { get; set; }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (propFormType == "MULTIPLE")
            {
                //fillVendorsGrid();
            }
            else
            {
                fillVendors();
            }
        }

        private void fillCmbSources()
        {
            cmbSource.Items.Clear();
            DataSet ds = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB("08004");
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("All Sources", "**"));
            foreach (DataRow dr in dt.Rows)
            {
                listItem.Add(new ListItem(dr["LookUpDesc"].ToString().Trim(), dr["Code"].ToString().Trim()));
            }
            cmbSource.Items.AddRange(listItem.ToArray());
            cmbSource.SelectedIndex = 0;
        }


        void fillcomboxhardcode()
        {
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("No", "N"));
            listItem.Add(new ListItem("Yes", "Y"));
            Cmb1099.Items.AddRange(listItem.ToArray());
            Cmb1099.SelectedIndex = 0;

            List<ListItem> listItem1 = new List<ListItem>();
            listItem1.Add(new ListItem("", "0"));
            listItem1.Add(new ListItem("SS#", "S"));
            listItem1.Add(new ListItem("EIN", "E"));
            cmbEinSSN.Items.AddRange(listItem1.ToArray());
            cmbEinSSN.SelectedIndex = 0;


        }

        private void Fillcombos()
        {
            CmbVendorType.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();


            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.VendorType, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);

            CmbVendorType.Items.Insert(0, new ListItem("Select One", "0"));
            CmbVendorType.SelectedIndex = 0;
            foreach (CommonEntity Resident in commonEntity)
            {
                ListItem li = new ListItem(Resident.Desc, Resident.Code);
                CmbVendorType.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Resident.Default.Equals("Y")) CmbVendorType.SelectedItem = li;
            }

        }


        List<CaseVDD1Entity> Vdd1list; List<CASEVDDEntity> CaseVddlist;
        private void fillVendors()
        {
            //gvwVendors.Visible = false;
            btnEdit.Visible = btnAdd.Visible = false;
            listView_Vendor.Visible = true;
            listView_Vendor.BringToFront();
            this.listView_Vendor.SelectedIndexChanged -= new System.EventHandler(this.listView_Vendor_SelectedIndexChanged);
            listView_Vendor.Items.Clear();
            int rowIndex = 0;
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVDD1Entity Vdd1_Entity = new CaseVDD1Entity(true);

            //if (rbNum.Checked)
            //    Search_Entity.Code = txtName.Text.Trim();
            //else
            //Search_Entity.Name = txtName.Text.Trim();

            if (Privileges.ModuleCode == "09")
                Vdd1_Entity.Type = "01";
            else if (Privileges.ModuleCode == "10")
            {
                if (Source_Type == "99")
                    Vdd1_Entity.Type = "01";
                else
                    Vdd1_Entity.Type = "05";
            }
            Vdd1list = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");
            // Added murali on 07/30/2021 
            string strsouce = string.Empty;
            if (((ListItem)cmbSource.SelectedItem).Value.ToString().Trim() != "**")
                strsouce = ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim();
            CaseVddlist = _model.SPAdminData.Vendor_Search(txtName.Text, strsouce);

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);

            CaseVddlist = CaseVddlist.OrderBy(u => u.Active).ToList();

            if (CaseVddlist.Count > 0)
            {

                foreach (CASEVDDEntity dr in CaseVddlist)
                {
                    ListViewItem Item = new ListViewItem(dr.Code.Trim());
                    //Item.SubItems.Add(dr.Code.Trim());
                    Item.SubItems.Add(dr.Name.Trim());
                    //commented by Sudheer on 05/28/2021
                    //Item.SubItems.Add(dr.Addr1.Trim());
                    //Item.SubItems.Add(dr.Addr2.Trim() + " " + dr.Addr3.Trim());

                    //Added by Sudheer on 05/28/2021
                    Item.SubItems.Add(dr.Cont_Name.Trim());
                    Item.SubItems.Add(dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim());

                    Item.SubItems.Add(dr.Active.Trim());
                    if (dr.Active == "I")
                    {
                        Item.ForeColor = Color.Red;
                    }
                    listView_Vendor.Items.Add(Item);

                    rowIndex++;

                }

                lblTotNoRec.Text = rowIndex.ToString();
            }

            listView_Vendor.Columns[4].Visible = false;

            this.listView_Vendor.SelectedIndexChanged += new System.EventHandler(this.listView_Vendor_SelectedIndexChanged);
            //**listView_Vendor.SelectedIndex = 0;
            panel3.Enabled = false;
            listView_Vendor.Enabled = true;
            if (listView_Vendor.Items.Count > 0)
            {
                lblTotal.Visible = true;
                lblTotNoRec.Visible = true;
                btnSelect.Visible = true;

                //cmbSource.Enabled = true;
                txtName.Enabled = true;
                listView_Vendor.SelectedIndex = 0;


                listView_Vendor_SelectedIndexChanged(listView_Vendor, new EventArgs());
            }
            else
            {
                lblTotal.Visible = false;
                lblTotNoRec.Visible = false;
                btnSelect.Visible = false;
                ClearControls();

                if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                    MessageBox.Show("'No Vendor found with this search text");

            }
            if (Privileges.ModuleCode != "05")
            {
                if (listView_Vendor.Items.Count > 0)
                {
                    
                    if (SerVendPrivileges!=null)
                    {
                        if (chkbW9.Checked == true)
                        { btnEdit.Visible = false; }
                        else if (SerVendPrivileges.ChangePriv.ToUpper() == "TRUE") btnEdit.Visible = true; else btnEdit.Visible = false;

                        if (SerVendPrivileges.AddPriv.ToUpper() == "TRUE")
                        {
                            if (txtName.Enabled == true)
                            {
                                btnAdd.Visible = true;
                               
                            }
                            else btnAdd.Visible = false;
                        }
                        else btnAdd.Visible = false;

                    }



                    ////if (userPrivilege1 != null)
                    ////{
                    ////if (userPrivilege1.ChangePriv.ToUpper() == "TRUE")
                    //if (chkbW9.Checked == true)
                    //{ btnEdit.Visible = false; }
                    //else { if (Privileges.ChangePriv.ToUpper() == "TRUE") btnEdit.Visible = true; else btnEdit.Visible = false; }

                    ////if (userPrivilege1.AddPriv.ToUpper() == "TRUE")
                    ////{
                    //if (txtName.Enabled == true)
                    //{
                    //    if (Privileges.AddPriv.ToUpper() == "TRUE")
                    //        btnAdd.Visible = true;
                    //    else btnAdd.Visible = false;
                    //}
                    ////}
                    ////}
                }
                else
                {
                    //if (userPrivilege1 != null)
                    //{
                    //    if (userPrivilege1.AddPriv.ToUpper() == "TRUE")
                    //    {
                    //        //btnAdd.Visible = true;
                    btnAdd.Visible = false;
                    if(SerVendPrivileges!=null)
                    {
                        if(SerVendPrivileges.AddPriv.ToUpper()=="TRUE")
                        {
                            btnAdd_Click(btnAdd, new EventArgs());
                        }
                    }
                    
                    //    }
                    //}
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (propFormType == "MULTIPLE")
            {

                {
                    CommonFunctions.MessageBoxDisplay("Select atleast one vendor");
                }
            }
            else
            {
                if (listView_Vendor.Items.Count > 0)
                {
                    //Krantih//if (listView_Vendor.SelectedItem.SubItems[4].Text.ToString().Trim() == "I")

                        if (listView_Vendor.SelectedItems[0].SubItems[4].Text.ToString().Trim() == "I")
                    {
                        if (Privileges.Program.ToUpper() == "TMS00201")
                        {
                            CommonFunctions.MessageBoxDisplay("Not An Active Vendor");
                        }
                        else
                        {
                            //Added by Sudheer on 01/15/24 based on the PCS.docx
                            CommonFunctions.MessageBoxDisplay("Not An Active Vendor");
                            //MessageBox.Show("Selected Vendor is Inactive" + "\n" + "Are you sure want to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Selected_Vendor_Row);
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }


        public void Selected_Vendor_Row(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
                if (dialogResult == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            //}
        }

        private void rbName_CheckedChanged(object sender, EventArgs e)
        {
            //if (rbNum.Checked)
            //{
            //    this.txtName.Size = new System.Drawing.Size(78, 20);
            //    this.txtName.MaxLength = 10;
            //    txtName.Text = string.Empty;
            //}
            //else
            {
                this.txtName.Size = new System.Drawing.Size(402, 20);
                this.txtName.MaxLength = 32;
                txtName.Text = string.Empty;
            }
        }

        public string[] Get_Selected_Vendor()
        {
            string[] SelectVendor = new string[2];
            if (listView_Vendor.Items.Count > 0)
            {
                //SelectVendor = listView_Vendor.SelectedItem.ToString();
                //kranthi//SelectVendor[0] = listView_Vendor.SelectedItem.SubItems[0].Text.ToString().Trim();

                SelectVendor[0] = listView_Vendor.SelectedItems[0].Text.ToString().Trim();

                //Commented by Sudheer on 05/26/2021
                //SelectVendor[1] = listView_Vendor.SelectedItem.SubItems[3].Text.ToString().Trim();
                
                //Kranthi//SelectVendor[1] = listView_Vendor.SelectedItem.SubItems[1].Text.ToString().Trim();

                SelectVendor[1] = listView_Vendor.SelectedItems[0].SubItems[1].Text.ToString().Trim();

            }
            return SelectVendor;
        }

        //public List<string> GetVendorMultipleCodes()
        //{
        //    List<string> strVendor = new List<string>();
        //    List<DataGridViewRow> SelectedgvRows = (from c in gvwVendors.Rows.Cast<DataGridViewRow>().ToList()
        //                                            where (c.Cells["Selected"].Value.ToString().ToUpper().Equals("Y"))
        //                                            select c).ToList();
        //    foreach (DataGridViewRow item in SelectedgvRows)
        //    {
        //        strVendor.Add(item.Cells["gvtNumber"].Value.ToString());
        //    }
        //    return strVendor;
        //}

        private void txtName_Leave(object sender, EventArgs e)
        {
            //if (rbNum.Checked)
            //{
            //    if (!string.IsNullOrEmpty(txtName.Text) || (!string.IsNullOrWhiteSpace(txtName.Text)))
            //    {
            //        string Number = txtName.Text.Trim();
            //        txtName.Text = "0000000000".Substring(0, 10 - Number.Length) + Number.Trim();
            //    }
            //}
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            //fillVendors();
        }

        private void txtName_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            fillVendors();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            //if (!string.IsNullOrEmpty(_LLRNAME.Trim()))
            //{
            Mode = "Add";

            ClearControls();

            string Source = string.Empty;
            if (((ListItem)cmbSource.SelectedItem).Value.ToString() != "**")
                Source = ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim();


            //VendorMaitainance_Form Vendor_Form_Add = new VendorMaitainance_Form(BaseForm, "Add", "", 1, userPrivilege1, Source, _LLRNAME);
            //Vendor_Form_Add.FormClosed += new Form.FormClosedEventHandler(Vendor_AddForm_Closed);
            //Vendor_Form_Add.ShowDialog();

            List<CASEVDDEntity> CaseVddlist;
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            string RepAddCode = "1";
            //if (CaseVddlist.Count > 0)
            //{
            //    RepAddCode = CaseVddlist.Max(u => u.Code);
            //    RepAddCode = (int.Parse(RepAddCode) + 1).ToString();
            //}
            //txtCode.Text = "0000000000".Substring(0, 10 - RepAddCode.Length) + RepAddCode;
            txtCode.Text = "New Vendor";

            //string vendorcode = System.Guid.NewGuid().ToString();
            //txtCode.Text = "L000"+ vendorcode.Substring(0, 6).ToUpper();
            txtCode.Enabled = false;



            if (!string.IsNullOrEmpty(_LLRNAME.Trim()))
            {
                CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetLandlordadpya(BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(), BaseForm.BaseYear.ToString(), BaseForm.BaseApplicationNo.ToString(), "Landlord");
                if (caseDiffDetails != null)
                {

                    ChkbActive.Checked = true;
                    txtVendName.Text = caseDiffDetails.IncareFirst + " " + caseDiffDetails.IncareLast;
                    txtContName.Text = caseDiffDetails.IncareFirst + " " + caseDiffDetails.IncareLast;
                    txtCity.Text = caseDiffDetails.City;

                    string Address1 = string.Empty;
                    if (!string.IsNullOrEmpty(caseDiffDetails.Hn.Trim())) Address1 = "H.No: " + caseDiffDetails.Hn.Trim();
                    if (!string.IsNullOrEmpty(caseDiffDetails.Apt.Trim())) { Address1 = Address1.Trim() == "" ? "Apt: " + caseDiffDetails.Apt.Trim() : Address1 + ", Apt: " + caseDiffDetails.Apt.Trim(); }
                    if (!string.IsNullOrEmpty(caseDiffDetails.Flr.Trim())) { Address1 = Address1.Trim() == "" ? "Flr: " + caseDiffDetails.Flr.Trim() : Address1 + ", Flr: " + caseDiffDetails.Flr.Trim(); }

                    txtAddr1.Text = Address1.Trim(); //caseDiffDetails.Hn + " " + caseDiffDetails.Street +" " + caseDiffDetails.Suffix + " " + caseDiffDetails.Apt + " " + caseDiffDetails.Flr;

                    if (caseDiffDetails.Phone != string.Empty)
                    {
                        maskPhone.Text = caseDiffDetails.Phone;
                    }
                    //maskFax.Text = caseDiffDetails.Fax.Trim();
                    txtState.Text = caseDiffDetails.State;
                    txtStreet.Text = caseDiffDetails.Street + " " + caseDiffDetails.Suffix;
                    txtZip.Text = caseDiffDetails.Zip;
                    txtZipPlus.Text = caseDiffDetails.ZipPlus;
                    txtZip.Text = "00000".Substring(0, 5 - caseDiffDetails.Zip.Length) + caseDiffDetails.Zip;
                    txtZipPlus.Text = "0000".Substring(0, 4 - caseDiffDetails.ZipPlus.Length) + caseDiffDetails.ZipPlus;

                }
            }
            else
            {
                txtVendName.Text = txtName.Text;
                txtContName.Text = txtName.Text;
            }

            CommonFunctions.SetComboBoxValue(CmbVendorType, "02");
            txtFuelType.Text = Source;
            ChkbActive.Checked = true;
            ChkbActive.Enabled = false;

            panel3.Enabled = true;
            listView_Vendor.Enabled = false;
            btnSelect.Visible = false;

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listView_Vendor.Items.Count > 0)
            {
                Mode = "Edit";
                //Added by Sudheer on 05/25/2021 for userPrivilege1

                //VendorMaitainance_Form Vendor_Form_Edit = new VendorMaitainance_Form(BaseForm, "Edit", listView_Vendor.SelectedItem.SubItems[0].Text.ToString().Trim(), 1, userPrivilege1);
                //Vendor_Form_Edit.FormClosed += new Form.FormClosedEventHandler(Vendor_AddForm_Closed);
                //Vendor_Form_Edit.ShowDialog();
                ChkbActive.Enabled = true;
                panel3.Enabled = true;
                listView_Vendor.Enabled = false;
                txtCode.Enabled = false;
                btnSelect.Visible = false;
            }
        }
        string Added_Edited_VendorCode = string.Empty; string Added_Edited_Type = string.Empty; string Mode = string.Empty;
        //Commented by Kranthi Vardhan on 11/28/2022 not used this function in this form.
        //private void Vendor_AddForm_Closed(object sender, FormClosedEventArgs e)
        //{
        //    VendorMaitainance_Form form = sender as VendorMaitainance_Form;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        string[] From_Results = new string[3];
        //        From_Results = form.GetSelected_Vendor_Code();
        //        Added_Edited_VendorCode = From_Results[0];
        //        Mode = From_Results[1];
        //        Added_Edited_Type = From_Results[2];
        //        if (From_Results[1].Equals("Add"))
        //        {
        //            btnSearch_Click(sender, e);
        //            MessageBox.Show("Vendor Details Inserted Successfully...", "CAP Systems");
        //        }
        //        else
        //        {
        //            btnSearch_Click(sender, e);
        //            MessageBox.Show("Vendor Details Updated Successfully...", "CAP Systems");
        //        }

        //    }
        //}

        string strmsgGrp = string.Empty; string SqlMsg = string.Empty; string Type = string.Empty;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CASEVDDEntity Entity = new CASEVDDEntity();
                    CaseVDD1Entity Vdd1Entity = new CaseVDD1Entity();

                    CaptainModel Model = new CaptainModel();
                    if (Mode == "Edit")
                    {
                        Entity.Rec_Type = "U";
                        Vdd1Entity.Rec_Type = "U";
                    }
                    else
                    {
                        Entity.Rec_Type = "I";
                    }
                    Entity.Code = txtCode.Text;
                    //if (txtCode.Text == "New")
                    //{
                    //    Entity.Rec_Type = "I";
                    //    Entity.Code = "1";
                    //}
                    ////Entity.Active = ChkbActive.Checked ? "A" : "I";
                    if (ChkbActive.Checked)
                        Entity.Active = "A";
                    else
                        Entity.Active = "I";
                    Entity.Name = txtVendName.Text.Trim();
                    Entity.Addr1 = txtStreet.Text.Trim();
                    Entity.Addr2 = txtAddr1.Text.Trim();
                    Entity.Addr3 = txtAddr2.Text.Trim();
                    Entity.City = txtCity.Text.Trim();
                    Entity.State = txtState.Text.Trim();
                    Entity.Zip = txtZip.Text.Trim() + txtZipPlus.Text.Trim();
                    string[] phonebreak = Regex.Split(maskPhone.Text, "-");
                    Entity.Phone = maskPhone.Text;//phonebreak[0].Trim() + phonebreak[1].Trim()+phonebreak[2].Trim();
                    string[] FaxBreak = Regex.Split(maskFax.Text, "-");
                    Entity.Fax = maskFax.Text;//FaxBreak[0].Trim() + FaxBreak[1].Trim()+FaxBreak[2].Trim();
                    Entity.Cont_Name = txtContName.Text.Trim();
                    string[] ContTelPhone = Regex.Split(maskContPhone.Text, "-");
                    Entity.Cont_Phone = maskContPhone.Text;//ContTelPhone[0].Trim() + ContTelPhone[1].Trim() + ContTelPhone[2].Trim();
                    //Entity.Vdd1099 = ((ListItem)Cmb1099.SelectedItem).Value.ToString().Trim();

                    //Entity.Email = txtEmail.Text.Trim();

                    if (_model.SPAdminData.UpdateCASEVDD(Entity, "Update", out strmsgGrp, out SqlMsg))
                    {
                        if (Mode == "Add")
                        {
                            string msgSql = string.Empty;
                            if (Entity.Rec_Type == "I")
                                Vdd1Entity.Rec_Type = "I";
                            Vdd1Entity.Code = strmsgGrp.Trim();
                            //Vdd1Entity.IndexBy = txtIndexBy.Text.Trim();
                            if (((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim() != "0")
                            {
                                //Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                                Vdd1Entity.Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                            }
                            //Vdd1Entity.SVENDOR_CODE = txtSecCode.Text.Trim();
                            //Vdd1Entity.BULK_CODE = ((ListItem)cmbFixedMargin.SelectedItem).Value.ToString().Trim();

                            if (!string.IsNullOrEmpty(txtFuelType.Text))
                            {
                                int i = 1;
                                for (int j = 0; j < txtFuelType.Text.Trim().Length;)
                                {
                                    string Temp_Fuel = txtFuelType.Text.Substring(j, 2).Trim();
                                    if (i == 1)
                                        Vdd1Entity.FUEL_TYPE1 = Temp_Fuel.Trim();
                                    else if (i == 2)
                                        Vdd1Entity.FUEL_TYPE2 = Temp_Fuel.Trim();
                                    else if (i == 3)
                                        Vdd1Entity.FUEL_TYPE3 = Temp_Fuel.Trim();
                                    else if (i == 4)
                                        Vdd1Entity.FUEL_TYPE4 = Temp_Fuel.Trim();
                                    else if (i == 5)
                                        Vdd1Entity.FUEL_TYPE5 = Temp_Fuel.Trim();
                                    else if (i == 6)
                                        Vdd1Entity.FUEL_TYPE6 = Temp_Fuel.Trim();
                                    else if (i == 7)
                                        Vdd1Entity.FUEL_TYPE7 = Temp_Fuel.Trim();
                                    else if (i == 8)
                                        Vdd1Entity.FUEL_TYPE8 = Temp_Fuel.Trim();
                                    else if (i == 9)
                                        Vdd1Entity.FUEL_TYPE9 = Temp_Fuel.Trim();
                                    else
                                        Vdd1Entity.FUEL_TYPE10 = Temp_Fuel.Trim();
                                    i++;
                                    j += 2;
                                }
                            }
                            //}
                            //Vdd1Entity.CYCLE = ((ListItem)CmbCycle.SelectedItem).Value.ToString().Trim();
                            //Vdd1Entity.ELEC_TRANSFER = ((ListItem)cmbElecTrans.SelectedItem).Value.ToString().Trim();
                            //Vdd1Entity.SSNO = maskEIN.Text.Trim();
                            //Vdd1Entity.ACCT_FORMAT = txtAccFormat.Text.Trim();
                            Vdd1Entity.AR = "N";
                            //Vdd1Entity.USE_VENDOR_CODE = txtParVendor.Text.Trim();
                            //Vdd1Entity.Name2 = txtParVenName.Text.Trim();
                            Vdd1Entity.Lsct_Operator = BaseForm.UserID;
                            Vdd1Entity.Add_Operator = BaseForm.UserID;
                            _model.SPAdminData.UpdateCASEVDD1(Vdd1Entity, "Update", out msgSql);
                            //this.DialogResult = DialogResult.OK;
                            //this.Close();
                            panel3.Enabled = false;
                            fillVendors();
                        }
                        else
                        {
                            string msgSql = string.Empty;
                            if (Entity.Rec_Type == "U")
                                Vdd1Entity.Rec_Type = "U";
                            Vdd1Entity.Code = txtCode.Text.Trim();
                            //Vdd1Entity.IndexBy = txtIndexBy.Text.Trim();
                            if (((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim() != "0")
                            {
                                //Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                                Vdd1Entity.Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                            }
                            //Vdd1Entity.SVENDOR_CODE = txtSecCode.Text.Trim();
                            //Vdd1Entity.BULK_CODE = ((ListItem)cmbFixedMargin.SelectedItem).Value.ToString().Trim();

                            if (!string.IsNullOrEmpty(txtFuelType.Text))
                            {
                                int i = 1;
                                for (int j = 0; j < txtFuelType.Text.Trim().Length;)
                                {
                                    string Temp_Fuel = txtFuelType.Text.Substring(j, 2).Trim();
                                    if (i == 1)
                                        Vdd1Entity.FUEL_TYPE1 = Temp_Fuel.Trim();
                                    else if (i == 2)
                                        Vdd1Entity.FUEL_TYPE2 = Temp_Fuel.Trim();
                                    else if (i == 3)
                                        Vdd1Entity.FUEL_TYPE3 = Temp_Fuel.Trim();
                                    else if (i == 4)
                                        Vdd1Entity.FUEL_TYPE4 = Temp_Fuel.Trim();
                                    else if (i == 5)
                                        Vdd1Entity.FUEL_TYPE5 = Temp_Fuel.Trim();
                                    else if (i == 6)
                                        Vdd1Entity.FUEL_TYPE6 = Temp_Fuel.Trim();
                                    else if (i == 7)
                                        Vdd1Entity.FUEL_TYPE7 = Temp_Fuel.Trim();
                                    else if (i == 8)
                                        Vdd1Entity.FUEL_TYPE8 = Temp_Fuel.Trim();
                                    else if (i == 9)
                                        Vdd1Entity.FUEL_TYPE9 = Temp_Fuel.Trim();
                                    else
                                        Vdd1Entity.FUEL_TYPE10 = Temp_Fuel.Trim();
                                    i++;
                                    j += 2;
                                }
                            }
                            //}
                            //Vdd1Entity.CYCLE = ((ListItem)CmbCycle.SelectedItem).Value.ToString().Trim();
                            //Vdd1Entity.ELEC_TRANSFER = ((ListItem)cmbElecTrans.SelectedItem).Value.ToString().Trim();
                            //Vdd1Entity.SSNO = maskEIN.Text.Trim();
                            //Vdd1Entity.ACCT_FORMAT = txtAccFormat.Text.Trim();
                            Vdd1Entity.AR = "N";
                            //Vdd1Entity.USE_VENDOR_CODE = txtParVendor.Text.Trim();
                            //Vdd1Entity.Name2 = txtParVenName.Text.Trim();
                            Vdd1Entity.Lsct_Operator = BaseForm.UserID;
                            Vdd1Entity.Add_Operator = BaseForm.UserID;

                            _model.SPAdminData.UpdateCASEVDD1(Vdd1Entity, "Update", out msgSql);
                            //this.DialogResult = DialogResult.OK;
                            //this.Close();
                            panel3.Enabled = false;
                            fillVendors();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool IsSaveValid { get; set; }
        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtCode.Text.Trim()) || string.IsNullOrWhiteSpace(txtCode.Text.Trim()))
            {
                _errorProvider.SetError(txtCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                if (Mode == "Add")
                {
                    List<CASEVDDEntity> CaseVddlist;
                    CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
                    CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");
                    if (CaseVddlist.Count > 0)
                    {
                        CASEVDDEntity ExistVendor = CaseVddlist.Find(u => u.Code.Equals(txtCode.Text.Trim()));
                        if (ExistVendor != null)
                        {
                            _errorProvider.SetError(txtCode, string.Format("Vendor Code Already Exists".Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                        }
                        else
                            _errorProvider.SetError(txtCode, null);
                    }
                }
                else
                    _errorProvider.SetError(txtCode, null);
            }
            else
            {
                _errorProvider.SetError(txtCode, null);
            }

            if (string.IsNullOrEmpty(txtVendName.Text) || string.IsNullOrWhiteSpace(txtVendName.Text))
            {
                _errorProvider.SetError(txtVendName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtName, null);

            if (string.IsNullOrEmpty(txtStreet.Text) || string.IsNullOrWhiteSpace(txtStreet.Text))
            {
                _errorProvider.SetError(txtStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStreet.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtStreet, null);

            if (string.IsNullOrEmpty(txtCity.Text) || string.IsNullOrWhiteSpace(txtCity.Text))
            {
                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCity, null);

            if (string.IsNullOrEmpty(txtState.Text) || string.IsNullOrWhiteSpace(txtState.Text))
            {
                _errorProvider.SetError(txtState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtState, null);

            if (string.IsNullOrEmpty(txtZip.Text) || string.IsNullOrWhiteSpace(txtZip.Text))
            {
                _errorProvider.SetError(btnZipSearch, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZip.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtZip, null);

            if (string.IsNullOrEmpty(txtFuelType.Text) || string.IsNullOrWhiteSpace(txtFuelType.Text))
            {
                _errorProvider.SetError(btnFuelTypes, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFuelType.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtFuelType, null);
            //if (((ListItem)cmbElecTrans.SelectedItem).Value.ToString().Trim() == "Y")
            //{
            //    if (string.IsNullOrEmpty(txtAccFormat.Text) || string.IsNullOrWhiteSpace(txtAccFormat.Text))
            //    {
            //        _errorProvider.SetError(txtAccFormat, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccFormat.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //        _errorProvider.SetError(txtAccFormat, null);
            //}
            //if (((ListItem)Cmb1099.SelectedItem).Value.ToString().Trim() == "Y")
            //{
            //    if (string.IsNullOrEmpty(maskEIN.Text) || string.IsNullOrWhiteSpace(maskEIN.Text))
            //    {
            //        _errorProvider.SetError(maskEIN, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEIN.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //        _errorProvider.SetError(maskEIN, null);
            //}

            //if (txtEmail.Text.Trim().Length > 0)
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, Consts.StaticVars.EmailValidatingString))
            //    {
            //        _errorProvider.SetError(txtEmail, Consts.Messages.PleaseEnterEmail);
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtEmail, null);
            //    }
            //}

            IsSaveValid = isValid;
            return (isValid);
        }

        private void listView_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Vendor.Items.Count > 0)
            {
                if (CaseVddlist.Count > 0)
                {
                   //kranthi// CASEVDDEntity drVdd = CaseVddlist.Find(u => u.Code == listView_Vendor.SelectedItem.SubItems[0].Text.ToString().Trim());
                  //kranthi//  CaseVDD1Entity drVdd1 = Vdd1list.Find(u => u.Code == listView_Vendor.SelectedItem.SubItems[0].Text.ToString().Trim());

                    CASEVDDEntity drVdd = CaseVddlist.Find(u => u.Code == listView_Vendor.SelectedItems[0].Text.ToString().Trim());
                    CaseVDD1Entity drVdd1 = Vdd1list.Find(u => u.Code == listView_Vendor.SelectedItems[0].Text.ToString().Trim());

                    if (drVdd != null)
                    {
                        txtCode.Text = drVdd.Code.Trim();
                        if (drVdd.Active.Equals("A")) ChkbActive.Checked = true;
                        else ChkbActive.Checked = false;
                        txtVendName.Text = drVdd.Name.Trim();
                        //txtSecCode.Text = drVdd1.SVENDOR_CODE.Trim();
                        //txtIndexBy.Text = drVdd1.IndexBy.Trim();
                        txtStreet.Text = drVdd.Addr1.Trim();
                        txtAddr1.Text = drVdd.Addr2.Trim();
                        txtAddr2.Text = drVdd.Addr3.Trim();
                        txtCity.Text = drVdd.City.Trim();
                        txtState.Text = drVdd.State.Trim();
                        if(!string.IsNullOrEmpty(drVdd.Zip.Trim()))
                        {if(drVdd.Zip.Length>1)
                            txtZip.Text = drVdd.Zip.Substring(0, 5).Trim();
                            if (drVdd.Zip.Length > 5)
                                txtZipPlus.Text = drVdd.Zip.Substring(5, 4).Trim();
                        }
                        
                        maskPhone.Text = drVdd.Phone.Trim();
                        maskFax.Text = drVdd.Fax.Trim();
                        txtContName.Text = drVdd.Cont_Name.Trim();
                        maskContPhone.Text = drVdd.Cont_Phone.Trim();
                        //txtEmail.Text = drVdd.Email.Trim();
                        //txtParVendor.Text = drVdd1.USE_VENDOR_CODE.Trim();
                        //txtParVenName.Text = drVdd1.Name2.Trim();
                        //txtAccFormat.Text = drVdd1.ACCT_FORMAT.Trim();
                        //if (drVdd1.AR.Equals("Y")) chkbContAgree.Checked = true;
                        //else chkbContAgree.Checked = false;
                        //SetComboBoxValue(cmbElecTrans, drVdd1.ELEC_TRANSFER.Trim());
                        //SetComboBoxValue(cmbFixedMargin, drVdd1.BULK_CODE.Trim());
                        SetComboBoxValue(CmbVendorType, drVdd1.Type.Trim());

                        Cmb1099.SelectedIndex = 0;
                        cmbEinSSN.SelectedIndex = 0;
                        SetComboBoxValue(Cmb1099, drVdd.Vdd1099.Trim());
                        SetComboBoxValue(cmbEinSSN, drVdd1.EINSSN_TYPE.Trim());
                        if (drVdd1.EINSSN_TYPE.ToString() == "S")
                        {
                            maskEIN.Mask = "000-00-0000";
                            maskEIN.MaxLength = 11;
                        }
                        else
                        {
                            maskEIN.Mask = "";
                            maskEIN.MaxLength = 9;
                        }

                        maskEIN.Text = drVdd1.SSNO.Trim();
                        txtFuelType.Text = drVdd1.FUEL_TYPE1.Trim() + drVdd1.FUEL_TYPE2.Trim() + drVdd1.FUEL_TYPE3.Trim() + drVdd1.FUEL_TYPE4.Trim() + drVdd1.FUEL_TYPE5.Trim() +
                            drVdd1.FUEL_TYPE6.Trim() + drVdd1.FUEL_TYPE7.Trim() + drVdd1.FUEL_TYPE8.Trim() + drVdd1.FUEL_TYPE9.Trim() + drVdd1.FUEL_TYPE10.Trim();
                        //maskEIN.Text = drVdd1.SSNO.Trim();
                        //SetComboBoxValue(CmbCycle, drVdd1.CYCLE.Trim());

                        if (drVdd.W9.Trim() == "Y")
                        {
                            chkbW9.Checked = true;
                            txtFirst.Text = drVdd.FName.Trim();
                            txtLast.Text = drVdd.LName.Trim();

                            lblFirst.Visible = true; lblLast.Visible = true;
                            txtFirst.Visible = true;
                            txtLast.Visible = true;

                            btnEdit.Visible = false;
                        }
                        else
                        {
                            if(SerVendPrivileges!=null)
                            { chkbW9.Checked = false; if (SerVendPrivileges.ChangePriv.ToUpper() == "TRUE") btnEdit.Visible = true; else btnEdit.Visible = false; }
                            
                            lblFirst.Visible = false; lblLast.Visible = false;
                            txtFirst.Visible = false;
                            txtLast.Visible = false;
                        }
                    }

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panel3.Enabled = false;
            listView_Vendor.Enabled = true;
        }

        private void ClearControls()
        {
            txtCode.Text = string.Empty;
            ChkbActive.Checked = false;
            txtVendName.Text = string.Empty;
            //txtSecCode.Text = drVdd1.SVENDOR_CODE.Trim();
            //txtIndexBy.Text = drVdd1.IndexBy.Trim();
            txtStreet.Text = string.Empty;
            txtAddr1.Text = string.Empty;
            txtAddr2.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZip.Text = string.Empty;
            txtZipPlus.Text = string.Empty;
            maskPhone.Text = string.Empty;
            maskFax.Text = string.Empty;
            txtContName.Text = string.Empty;
            maskContPhone.Text = string.Empty;
            //txtEmail.Text = drVdd.Email.Trim();
            //txtParVendor.Text = drVdd1.USE_VENDOR_CODE.Trim();
            //txtParVenName.Text = drVdd1.Name2.Trim();
            //txtAccFormat.Text = drVdd1.ACCT_FORMAT.Trim();
            //if (drVdd1.AR.Equals("Y")) chkbContAgree.Checked = true;
            //else chkbContAgree.Checked = false;
            //SetComboBoxValue(cmbElecTrans, drVdd1.ELEC_TRANSFER.Trim());
            //SetComboBoxValue(cmbFixedMargin, drVdd1.BULK_CODE.Trim());
            //SetComboBoxValue(CmbVendorType, drVdd1.Type.Trim());
           // SetComboBoxValue(Cmb1099, drVdd.Vdd1099.Trim());
            txtFuelType.Text = string.Empty;
            Cmb1099.SelectedIndex = 0;
            cmbEinSSN.SelectedIndex = 0;
            maskEIN.Text = "";

        }

        private void btnZipSearch_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.ShowDialog();
        }

        private void OnZipCodeFormClosed(object sender, FormClosedEventArgs e)
        {
            btnZipSearch.Enabled = true;
            //btnCitySearch.Enabled = true;
            ZipCodeSearchForm form = sender as ZipCodeSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                ZipCodeEntity zipcodedetais = form.GetSelectedZipCodedetails();
                if (zipcodedetais != null)
                {
                    string zipPlus = zipcodedetais.Zcrplus4;
                    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    txtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                    txtState.Text = zipcodedetais.Zcrstate;
                    txtCity.Text = zipcodedetais.Zcrcity;
                    //SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                    //SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);

                }
            }
            // btnCitySearch.Focus();
        }

        List<CaseVDD1Entity> Sel_Vdd1_List = new List<CaseVDD1Entity>();
        private void btnFuelTypes_Click(object sender, EventArgs e)
        {
            List<CaseVDD1Entity> sel_CASEVdd1_entity = new List<CaseVDD1Entity>();
            FuelTypes_Form fuel_form = new FuelTypes_Form("FuelType", txtFuelType.Text);
            fuel_form.FormClosed += new FormClosedEventHandler(On_Form_Select_Closed);
            fuel_form.ShowDialog();
        }

        private void On_Form_Select_Closed(object sender, FormClosedEventArgs e)
        {

            string Sql_MSg = string.Empty;
            FuelTypes_Form form = sender as FuelTypes_Form;
            string Fuel_List = string.Empty;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_Vdd1_List = form.GetSelected_FuelTypes();
                Fuel_List = form.GetSelected_Fuels();
                txtFuelType.Text = Fuel_List.Trim();
            }
        }



        //private void fillVendorsGrid()
        //{
        //    listView_Vendor.Visible = false;     
        //    gvwVendors.Rows.Clear();
        //    gvwVendors.Visible = true;
        //    gvwVendors.BringToFront();
        //    int rowIndex = 0;
        //    CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
        //    CaseVDD1Entity Vdd1_Entity = new CaseVDD1Entity(true);

        //    if (rbNum.Checked)
        //        Search_Entity.Code = txtName.Text.Trim();
        //    else
        //        Search_Entity.Name = txtName.Text.Trim();

        //    if (Privileges.ModuleCode == "09")
        //        Vdd1_Entity.Type = "01";
        //    else if (Privileges.ModuleCode == "10")
        //    {
        //        if (Source_Type == "99")
        //            Vdd1_Entity.Type = "01";
        //        else
        //            Vdd1_Entity.Type = "05";
        //    }

        //    CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");
        //    Vdd1list = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");

        //    foreach (CaseVDD1Entity Entity in Vdd1list)
        //    {
        //        if (((ListItem)cmbSource.SelectedItem).Value.ToString().Trim() != "**")
        //        {
        //            if ((Entity.FUEL_TYPE1.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE2.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
        //                (Entity.FUEL_TYPE3.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE4.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
        //                (Entity.FUEL_TYPE5.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE6.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
        //                (Entity.FUEL_TYPE7.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE8.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
        //                (Entity.FUEL_TYPE9.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE10.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()))
        //            {
        //                foreach (CASEVDDEntity dr in CaseVddlist)
        //                {
        //                    if (dr.Code.Trim() == Entity.Code.Trim())
        //                    {

        //                        gvwVendors.Rows.Add(Img_Blank, dr.Code.Trim(), Entity.IndexBy.Trim(), dr.Name.Trim() + " " + dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Name.Trim(), dr.Active.Trim(), "N");                                    
        //                        rowIndex++;
        //                    }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            foreach (CASEVDDEntity dr in CaseVddlist)
        //            {
        //                if (dr.Code.Trim() == Entity.Code.Trim())
        //                {
        //                    gvwVendors.Rows.Add(Img_Blank, dr.Code.Trim(), Entity.IndexBy.Trim(), dr.Name.Trim() + " " + dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Name.Trim(), dr.Active.Trim(), "N"); 
        //                    rowIndex++;
        //                }
        //            }
        //        }
        //        lblTotNoRec.Text = rowIndex.ToString();
        //    }

        //    foreach (DataGridViewRow item in gvwVendors.Rows)
        //    {
        //        if (propVendorList != null)
        //        {
        //            if (propVendorList.Contains(item.Cells["gvtNumber"].Value.ToString()))
        //            {
        //                item.Cells["Ref_Sel"].Value = Img_Tick;
        //                item.Cells["Selected"].Value = "Y";
        //            }
        //        }
        //        else break;
        //    }
        //}

        //private void gvwVendors_CellClick(object sender, DataGridViewCellEventArgs e)
        //{

        //    if (gvwVendors.Rows.Count > 0)
        //    {
        //        if (e.ColumnIndex == 0)
        //        {
        //            if (gvwVendors.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
        //            {
        //                gvwVendors.CurrentRow.Cells["Ref_Sel"].Value = Img_Blank;
        //                gvwVendors.CurrentRow.Cells["Selected"].Value = "N";
        //            }
        //            else
        //            {
        //                gvwVendors.CurrentRow.Cells["Ref_Sel"].Value = Img_Tick;
        //                gvwVendors.CurrentRow.Cells["Selected"].Value = "Y";
        //            }
        //        }
        //    }

        //}

    }
}