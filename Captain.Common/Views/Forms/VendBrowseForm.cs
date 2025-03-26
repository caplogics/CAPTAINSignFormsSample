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
using Captain.Common.Model.Data;
using Spire.Pdf.Grid;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class VendBrowseForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private string[] strCode = null;
        public int strIndex = 0;
        int selIndex = 0;
        #endregion

        public VendBrowseForm(BaseForm baseform, PrivilegeEntity privileges, string source_type)
        {
            InitializeComponent();
            propFormType = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2; 
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privileges;
            Source_Type = source_type;
            this.Text = /*Privileges.Program*/  "Vendor Browser";
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
            fillCmbSources();
            SetComboBoxValue(cmbSource, Source_Type);
            if (Privileges.ModuleCode == "05")
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                //this.btnSearch.Location = new System.Drawing.Point(794, 30);
                // fillVendors();
            }

        }

        string _LLRNAME = string.Empty;
        public VendBrowseForm(BaseForm baseform, PrivilegeEntity privileges, string source_type,string LLRNAME)
        {
            InitializeComponent();
            propFormType = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2;
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privileges;
            Source_Type = source_type;
            this.Text = /*Privileges.Program + */"Vendor Browser";

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
            }
            if (Privileges.ModuleCode == "05" || Privileges.ModuleCode == "10")
            {
                cmbSource.Visible = false; lblSource.Visible = false;
            }
            else
            {
                cmbSource.Visible = true; lblSource.Visible = true;
                gvtName2.HeaderText = "Contact Name";
            }
            fillCmbSources();
            SetComboBoxValue(cmbSource, Source_Type);
            if (Privileges.ModuleCode == "05")
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                //this.btnSearch.Location = new System.Drawing.Point(794, 30);
                // fillVendors();
            }

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
        //    this.Text = Privileges.Program + " -Vendor Browser";
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


        public VendBrowseForm(BaseForm baseform, PrivilegeEntity privileges, string source_type, string FormType, List<string> strVendorList)
        {
            InitializeComponent();
            propFormType = FormType;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2;
            _model = new CaptainModel();

            propVendorList = strVendorList;
            BaseForm = baseform;
            Privileges = privileges;
            Source_Type = source_type;
            this.Text = /*Privileges.Program + */"Vendor Browser";
            ////if (rbNum.Checked)
            ////{
            ////    this.txtName.Size = new System.Drawing.Size(78, 20);
            ////    this.txtName.MaxLength = 10;
            ////}

            //if (Privileges.ModuleCode == "05" || Privileges.ModuleCode == "10")
            //{
            //    cmbSource.Visible = false; lblSource.Visible = false;
            //}
            //else
            //{
            //    cmbSource.Enabled = false;
            //    cmbSource.Visible = true; lblSource.Visible = true;
            //}
            //fillCmbSources();
            //SetComboBoxValue(cmbSource, Source_Type);


           
                
            

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
                gvtName2.HeaderText = "Contact Name";
            }
            fillCmbSources();
            SetComboBoxValue(cmbSource, Source_Type);
            if (Privileges.ModuleCode == "05")
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                //this.btnSearch.Location = new System.Drawing.Point(794, 30);
                // fillVendors();
            }



            if (FormType == "MULTIPLE")
            {
                gvchkSel.Visible = true;
                gvtAddress.Width = 200;
                fillVendorsGrid();
            }
            else
            {
                cmbSource.Enabled = false;
                fillVendors();
            }


        }




        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Blank = "blank";//"Resources/Icons/16X16/BlankIcon.gif";
        string Img_Tick = "icon-gridtick";//"Resources/Icons/16X16/tick2.png";

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Source_Type { get; set; }
        public string propFormType = string.Empty;
        public List<string> propVendorList = new List<string>();

        //Added by Sudheer on 05/18/2021
        public PrivilegeEntity userPrivilege1 { get; set; }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (propFormType == "MULTIPLE")
            {
               fillVendorsGrid();
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


        List<CaseVDD1Entity> Vdd1list; List<CASEVDDEntity> CaseVddlist;
        private void fillVendors()
        {
            //gvwVendors.Visible = false;
            btnEdit.Visible = btnAdd.Visible  = false;            
            gvwVendor.Rows.Clear();
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

            string strsouce = string.Empty;
            if (((ListItem)cmbSource.SelectedItem).Value.ToString().Trim() != "**")
                strsouce = ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim();
            CaseVddlist = _model.SPAdminData.Vendor_Search(txtName.Text, strsouce);

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);

            CaseVddlist = CaseVddlist.OrderBy(u => u.Active).ToList();

            if (Privileges.ModuleCode == "05")
            {
                CaseVddlist = CaseVddlist.FindAll(u => u.Active != "I");
                
                foreach (CASEVDDEntity dr in CaseVddlist)
                {
                    ListViewItem Item = new ListViewItem();
                    Item.SubItems.Add(dr.Code.Trim());                  
                    Item.SubItems.Add(dr.Name.Trim());
                    Item.SubItems.Add(dr.Addr1.Trim());
                    Item.SubItems.Add(dr.Addr2.Trim() + " " + dr.Addr3.Trim());
                    Item.SubItems.Add(dr.Active.Trim());
                    if (dr.Active == "I")
                    {
                        Item.ForeColor = Color.Red;
                    }
                   // listView_Vendor.Items.Add(Item);

                  int rowgvindex=  gvwVendor.Rows.Add(false, dr.Code.Trim(), dr.Name.Trim(), dr.Addr1.Trim(), dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Active.Trim());

                    if (dr.Active == "I")
                    {
                        gvwVendor.Rows[rowgvindex].DefaultCellStyle.ForeColor = Color.Red;                        
                    }
                    rowIndex++;
                }
                lblTotNoRec.Text = rowIndex.ToString();
               
               
            }
            else
            {
                Vdd1list = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");
                foreach (CaseVDD1Entity Entity in Vdd1list)
                {
                    if (((ListItem)cmbSource.SelectedItem).Value.ToString().Trim() != "**")
                    {                        
                            foreach (CASEVDDEntity dr in CaseVddlist)
                            {
                                if (dr.Code.Trim() == Entity.Code.Trim())
                                {
                                  
                                    int rowgvindex = gvwVendor.Rows.Add(false, dr.Code.Trim(), dr.Name.Trim(), dr.Cont_Name.Trim(), dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Active.Trim());

                                    if (dr.Active == "I")
                                    {
                                        gvwVendor.Rows[rowgvindex].DefaultCellStyle.ForeColor = Color.Red;
                                    }

                                    rowIndex++;
                                }
                            }

                    }
                    else
                    {
                        foreach (CASEVDDEntity dr in CaseVddlist)
                        {
                            if (dr.Code.Trim() == Entity.Code.Trim())
                            {
                                ListViewItem Item = new ListViewItem();

                                Item.SubItems.Add(dr.Code.Trim());
                                Item.SubItems.Add(dr.Name.Trim());
                                //Item.SubItems.Add(dr.Addr1.Trim());
                                //Item.SubItems.Add(dr.Addr2.Trim() + " " + dr.Addr3.Trim());
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
                                //listView_Vendor.Items.Add(Item);
                                int rowgvindex = gvwVendor.Rows.Add(false, dr.Code.Trim(), dr.Name.Trim(), dr.Cont_Name.Trim(), dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Active.Trim());

                                if (dr.Active == "I")
                                {
                                    gvwVendor.Rows[rowgvindex].DefaultCellStyle.ForeColor = Color.Red;
                                }

                                rowIndex++;
                            }
                        }
                    }
                    lblTotNoRec.Text = rowIndex.ToString();
                }
                                
                //listView_Vendor.Columns[4].Visible = false;
            }
            this.gvwVendor.Sort(this.gvwVendor.Columns["gvtActive"], ListSortDirection.Ascending);
            if (gvwVendor.Rows.Count > 0)
            {
                gvwVendor.Rows[selIndex].Selected = true;
                gvwVendor.CurrentCell = gvwVendor.Rows[selIndex].Cells[1];
                lblTotal.Visible = true;
                lblTotNoRec.Visible = true;
                btnSelect.Visible = true;
                if (Privileges.Program == "TMS00201")
                {
                    cmbSource.Enabled = false;
                }
                else
                {
                    cmbSource.Enabled = true;
                }
                txtName.Enabled = true;
            }
            else
            {
                lblTotal.Visible = false;
                lblTotNoRec.Visible = false;
                btnSelect.Visible = false;

                if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                    AlertBox.Show("No Vendor found with this search text",MessageBoxIcon.Warning);
            }
            
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (propFormType == "MULTIPLE")
            {
                List<string> strVendorlist = GetVendorMultipleCodes();
                if (strVendorlist.Count > 0)
                {

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    //CommonFunctions.MessageBoxDisplay("Select atleast one Vendor");
                    AlertBox.Show("Select at least one Vendor.", MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (gvwVendor.Rows.Count > 0)
                {
                    if (gvwVendor.SelectedRows[0].Cells["gvtActive"].Value.ToString().Trim() == "I")
                    {
                        if (Privileges.Program.ToUpper() == "TMS00201")
                        {
                            AlertBox.Show("Not an Active Vendor.",MessageBoxIcon.Warning);
                        }
                        else
                        {
                            //Added by Sudheer on 01/15/24 based on the PCS.docx
                            AlertBox.Show("Not an Active Vendor.", MessageBoxIcon.Warning);
                            //MessageBox.Show("Selected Vendor is Inactive" + "\n" + "Are you sure want to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: Selected_Vendor_Row);
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
           // Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

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
                //this.txtName.Size = new System.Drawing.Size(402, 20);
                this.txtName.MaxLength = 32;
                txtName.Text = string.Empty;
            }
        }

        public string[] Get_Selected_Vendor()
        {
            string[] SelectVendor = new string[2];
            if (gvwVendor.Rows.Count > 0)
            {
                
                SelectVendor[0] =  gvwVendor.SelectedRows[0].Cells["gvtNumber"].Value.ToString().Trim();                
                SelectVendor[1] = gvwVendor.SelectedRows[0].Cells["gvtName"].Value.ToString().Trim();

            }
            return SelectVendor;
        }

        public List<string> GetVendorMultipleCodes()
        {
            List<string> strVendor = new List<string>();            

            List<DataGridViewRow> SelectedgvRows = (from c in gvwVendor.Rows.Cast<DataGridViewRow>().ToList()
                                                    where (((DataGridViewCheckBoxCell)c.Cells["gvchkSel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                    select c).ToList();
           
            foreach (DataGridViewRow item in SelectedgvRows)
            {
                strVendor.Add(item.Cells["gvtNumber"].Value.ToString());
            }
            return strVendor;
        }


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
           
            if(objArgs.KeyCode == Keys.Enter)
            fillVendors();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            //if (!string.IsNullOrEmpty(_LLRNAME.Trim()))
            //{

            string Source = string.Empty;
            if (((ListItem)cmbSource.SelectedItem).Value.ToString() != "**")
                Source = ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim();

                //VendorMaitainance_Form Vendor_Form_Add = new VendorMaitainance_Form(BaseForm, "Add", "", 1, userPrivilege1, Source, _LLRNAME);
                //Vendor_Form_Add.FormClosed += new FormClosedEventHandler(Vendor_AddForm_Closed);
                //Vendor_Form_Add.ShowDialog();
            //}
            //else
            //{
            //    VendorMaitainance_Form Vendor_Form_Add = new VendorMaitainance_Form(BaseForm, "Add", "", 1, userPrivilege1);
            //    Vendor_Form_Add.FormClosed += new FormClosedEventHandler(Vendor_AddForm_Closed);
            //    Vendor_Form_Add.ShowDialog();
            //}
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (gvwVendor.Rows.Count > 0)
            {
                //Added by Sudheer on 05/25/2021 for userPrivilege1

                //VendorMaitainance_Form Vendor_Form_Edit = new VendorMaitainance_Form(BaseForm, "Edit", gvwVendor.SelectedRows[0].Cells["gvtNumber"].Value.ToString().Trim(), 1, userPrivilege1);
                //Vendor_Form_Edit.FormClosed += new FormClosedEventHandler(Vendor_AddForm_Closed);
                //Vendor_Form_Edit.ShowDialog();
            }
        }
        string Added_Edited_VendorCode = string.Empty; string Added_Edited_Type = string.Empty; string Mode = string.Empty;
        private void Vendor_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            //VendorMaitainance_Form form = sender as VendorMaitainance_Form;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    string[] From_Results = new string[3];
            //    From_Results = form.GetSelected_Vendor_Code();
            //    Added_Edited_VendorCode = From_Results[0];
            //    Mode = From_Results[1];
            //    Added_Edited_Type = From_Results[2];
            //    if (From_Results[1].Equals("Add"))
            //    {
            //        btnSearch_Click(sender, e);
            //        MessageBox.Show("Vendor Details Inserted Successfully...", "CAPTAIN");
            //    }
            //    else
            //    {
            //        btnSearch_Click(sender, e);
            //        MessageBox.Show("Vendor Details Updated Successfully...", "CAPTAIN");
            //    }

            //}
        }



        private void fillVendorsGrid()
        {
           
            gvwVendor.Rows.Clear();
            gvwVendor.Visible = true;
            gvwVendor.BringToFront();
            int rowIndex = 0;
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVDD1Entity Vdd1_Entity = new CaseVDD1Entity(true);

            
            if (Privileges.ModuleCode == "09")
                Vdd1_Entity.Type = "01";
            else if (Privileges.ModuleCode == "10")
            {
                if (Source_Type == "99")
                    Vdd1_Entity.Type = "01";
                else
                    Vdd1_Entity.Type = "05";
            }

            //CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            //if (CaseVddlist.Count > 0)
            //{
            //    List<CASEVDDEntity> SelectedVddList = new List<CASEVDDEntity>();

            //    if (Privileges.ModuleCode == "05")
            //    {
            //        SelectedVddList = CaseVddlist.FindAll(u => u.Code.ToUpper().Contains(txtName.Text.ToUpper().Trim()) || u.Name.ToUpper().Contains(txtName.Text.ToUpper().Trim())
            //        || u.Addr1.ToUpper().Contains(txtName.Text.ToUpper().Trim()) || u.Addr2.ToUpper().Contains(txtName.Text.ToUpper().Trim()) || u.Addr3.ToUpper().Contains(txtName.Text.ToUpper().Trim()));
            //    }
            //    else
            //    {
            //        SelectedVddList = CaseVddlist.FindAll(u => u.Code.ToUpper().Contains(txtName.Text.ToUpper().Trim()) || u.Name.ToUpper().Contains(txtName.Text.ToUpper().Trim()) || u.Cont_Name.ToUpper().Contains(txtName.Text.ToUpper().Trim())
            //        || u.Addr1.ToUpper().Contains(txtName.Text.ToUpper().Trim()) || u.Addr2.ToUpper().Contains(txtName.Text.ToUpper().Trim()) || u.Addr3.ToUpper().Contains(txtName.Text.ToUpper().Trim()));
            //    }
                
            //    SelectedVddList = SelectedVddList.OrderBy(u => u.Name.Trim()).ToList();

            //    CaseVddlist = SelectedVddList;
            //}


            string strsouce = string.Empty;
            if (((ListItem)cmbSource.SelectedItem).Value.ToString().Trim() != "**")
                strsouce = ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim();
            CaseVddlist = _model.SPAdminData.Vendor_Search(txtName.Text, strsouce);

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);

            Vdd1list = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");

            foreach (CaseVDD1Entity Entity in Vdd1list)
            {
                if (((ListItem)cmbSource.SelectedItem).Value.ToString().Trim() != "**")
                {                   
                        foreach (CASEVDDEntity dr in CaseVddlist)
                        {
                            if (dr.Code.Trim() == Entity.Code.Trim())
                            {

                                int rowgvindex = gvwVendor.Rows.Add(false, dr.Code.Trim(), dr.Name.Trim(), dr.Cont_Name.Trim(), dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Active.Trim());
                                
                            if (dr.Active == "I")
                                {
                                    gvwVendor.Rows[rowgvindex].DefaultCellStyle.ForeColor = Color.Red;
                                }

                                rowIndex++;
                            }
                        }                   

                }
                else
                {
                    foreach (CASEVDDEntity dr in CaseVddlist)
                    {
                        if (dr.Code.Trim() == Entity.Code.Trim())
                        {
                            int rowgvindex = gvwVendor.Rows.Add(false, dr.Code.Trim(), dr.Name.Trim(), dr.Cont_Name.Trim(), dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Active.Trim());
                            if (dr.Active == "I")
                            {
                                gvwVendor.Rows[rowgvindex].DefaultCellStyle.ForeColor = Color.Red;
                            }
                            rowIndex++;
                        }
                    }
                }
                lblTotNoRec.Text = rowIndex.ToString();
            }
            this.gvwVendor.Sort(this.gvwVendor.Columns["gvtActive"], ListSortDirection.Ascending);
            foreach (DataGridViewRow item in gvwVendor.Rows)
            {
                if (propVendorList != null)
                {
                    if (propVendorList.Count > 0)
                    {
                        if (propVendorList.Contains(item.Cells["gvtNumber"].Value.ToString()))
                        {
                            //item.Cells["Ref_Sel"].Value = System.Drawing.Image.FromFile(Application.MapPath(Img_Tick));   //Vikash commented 01/05/2023 
                            item.Cells["gvchkSel"].Value = true;
                        }
                    }
                }
                else break;
            }

            if (gvwVendor.Rows.Count > 0)
            {
                gvwVendor.Rows[0].Selected = true;
                lblTotal.Visible = true;
                lblTotNoRec.Visible = true;
                btnSelect.Visible = true;

                cmbSource.Enabled = true;
                txtName.Enabled = true;
            }
            else
            {
                lblTotal.Visible = false;
                lblTotNoRec.Visible = false;
                btnSelect.Visible = false;

                if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                    AlertBox.Show("No Vendor found with this search text",MessageBoxIcon.Warning);
            }

        }

        private void gvwVendor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


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