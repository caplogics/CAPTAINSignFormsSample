#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Model.Data;
using Captain.Common.Exceptions;
using System.Diagnostics;
using NPOI.SS.Formula.Functions;
#endregion


namespace Captain.Common.Views.UserControls
{
    public partial class Vendor_Maintainance : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string[] strCode = null;
        public int strIndex = 0;
        public int strPageIndex = 1;
        #endregion
        public Vendor_Maintainance(BaseForm baseform, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileges = privileges;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            //if(rbNumber.Checked.Equals(true))
            //    txtNum.Validator = TextBoxValidation.IntegerValidator;

            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            
            fillCmbFuelTypes();
            fillCmbVendorTypes();

            FillVendorGrid(Added_Edited_VendorCode);

            PopulateToolbar(oToolbarMnustrip);
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public ToolBarButton ToolBarPrint { get; set; }

        public bool IsSaveValid { get; set; }

        public AgencyControlEntity propAgencyControlDetails { get; set; }

        #endregion


        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (toolBar.Controls.Count == 0)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Add Vendor";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add"; //@"Resources\Images\16x16\addItem.png"; 
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Vendor";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";//@"Resources\Images\16X16\editItem.png";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Vendor";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";//@"Resources\Images\16X16\delete.png";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarPrint = new ToolBarButton();
                //ToolBarPrint.Tag = "Print";
                //ToolBarPrint.ToolTipText = "Print";
                //ToolBarPrint.Enabled = true;
                //ToolBarPrint.Image = new IconResourceHandle(Consts.Icons16x16.Print);
                //ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help";//@"Resources\Images\16X16\help.png";
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }
            if (Privileges.AddPriv.Equals("false"))
                ToolBarNew.Enabled = false;
            if (Privileges.ChangePriv.Equals("false"))
                ToolBarEdit.Enabled = false;
            if (Privileges.DelPriv.Equals("false"))
                ToolBarDel.Enabled = false;



            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                //ToolBarPrint,
                ToolBarHelp
            });

        }

        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();

            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null) { return; }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:
                        Added_Edited_VendorCode = string.Empty;
                        VendorMaintenance_Form Vendor_Form_Add = new VendorMaintenance_Form(BaseForm, "Add", "", gvVendor.Rows.Count, Privileges);
                        Vendor_Form_Add.FormClosed += new FormClosedEventHandler(Vendor_AddForm_Closed);
                        Vendor_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                        Vendor_Form_Add.ShowDialog();

                        break;
                    case Consts.ToolbarActions.Edit:
                        if (gvVendor.Rows.Count > 0)
                        {
                            VendorMaintenance_Form Vendor_Form_Edit = new VendorMaintenance_Form(BaseForm, "Edit", gvVendor.CurrentRow.Cells["Number"].Value.ToString().Trim(),gvVendor.Rows.Count, Privileges);
                            Vendor_Form_Edit.FormClosed += new FormClosedEventHandler(Vendor_AddForm_Closed);
                            Vendor_Form_Edit.StartPosition = FormStartPosition.CenterScreen;
                            Vendor_Form_Edit.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (gvVendor.Rows.Count > 0)
                        {
                            strIndex = gvVendor.SelectedRows[0].Index;
                            //strPageIndex = gvVendor.CurrentPage;
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Vendor Number: " + gvVendor.CurrentRow.Cells["Number"].Value.ToString().Trim(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Vendor_Row);
                        }
                        break;
                    case Consts.ToolbarActions.Print:

                        break;
                    case Consts.ToolbarActions.Help:
                            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }



        DataTable dt = new DataTable();
        private void fillCmbVendorTypes()
        {
            cmbSource.Items.Clear();
            DataSet ds = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB("08004");
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("All Sources", "**"));
            foreach (DataRow dr in dt.Rows)
            {
                listItem.Add(new ListItem(dr["LookUpDesc"].ToString(), dr["Code"].ToString()));
            }
            cmbSource.Items.AddRange(listItem.ToArray());
            cmbSource.SelectedIndex = 0;
        }

        private void fillCmbFuelTypes()
        {
            CmbFuelType.Items.Clear();   
            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.VendorType, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); 

            CmbFuelType.Items.Insert(0, new ListItem("All", "**"));
            CmbFuelType.SelectedIndex = 0;
            foreach (CommonEntity Resident in commonEntity)
            {
                ListItem li = new ListItem(Resident.Desc, Resident.Code);
                CmbFuelType.Items.Add(li);                
            }

        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            FillVendorGrid(string.Empty);
        }



      
        CaseVDD1Entity Vdd1_Entity = new CaseVDD1Entity(true);
        CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
        List<CaseVDD1Entity> Vdd1list; List<CASEVDDEntity> CaseVddlist;

        public void FillVendorGrid(string Sel_Number)
        {
            gvVendor.Rows.Clear();
            int rowIndex = 0; int RowCnt = 0;
            int Sel_Vendor_Index = 0;
            
            //if (Mode == "Add")
            //{
            //    Vdd1_Entity.Type = Added_Edited_Type.Trim();
            //    SetComboBoxValue(CmbFuelType, Added_Edited_Type);
            //}
            //else if (Mode == "Edit")
            //{
            //    Vdd1_Entity.Type = Added_Edited_Type.Trim();
            //    SetComboBoxValue(CmbFuelType, Added_Edited_Type);
            //}
            //else
            if (((ListItem)CmbFuelType.SelectedItem).Value.ToString().Trim() != "**")
                Vdd1_Entity.Type = ((ListItem)CmbFuelType.SelectedItem).Value.ToString().Trim();
            else
                Vdd1_Entity.Type = null;
            
            Vdd1list = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (CaseVddlist.Count > 0)
            {
                List<CASEVDDEntity> SelectedVddList = new List<CASEVDDEntity>();

               
                    SelectedVddList = CaseVddlist.FindAll(u => u.Code.ToUpper().Contains(txtNum.Text.ToUpper().Trim()) || u.Name.ToUpper().Contains(txtNum.Text.ToUpper().Trim())
                    || u.Addr1.ToUpper().Contains(txtNum.Text.ToUpper().Trim()) || u.Addr2.ToUpper().Contains(txtNum.Text.ToUpper().Trim()) || u.Addr3.ToUpper().Contains(txtNum.Text.ToUpper().Trim()));

                if (SelectedVddList.Count > 0 && BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                {
                    if(BaseForm.BaseAdminAgency!="**")
                        SelectedVddList = SelectedVddList.FindAll(u => u.VDD_Agency == BaseForm.BaseAdminAgency);
                }

                SelectedVddList = SelectedVddList.OrderBy(u => u.Name.Trim()).ToList();

                CaseVddlist = SelectedVddList;
            }



            string strState = string.Empty;
            if (propAgencyControlDetails != null)
            {
                strState = propAgencyControlDetails.State.ToString();
            }
            //if (strState.ToUpper() == "TX")
            //{
               
            //        foreach (CASEVDDEntity dr in CaseVddlist)
            //        {
            //        //rowIndex = gvVendor.Rows.Add(dr.Code.Trim(), dr.Name.Trim(), dr.Name.Trim() + " " + dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.Name.Trim());
            //        rowIndex = gvVendor.Rows.Add(dr.Code.Trim(), dr.Name.Trim(), dr.Addr1.Trim() + " " + dr.Addr2.Trim() + " " + dr.Addr3.Trim(), dr.City.Trim(), dr.State.Trim(), SetLeadingZeros(dr.Zip.Trim()), dr.Name.Trim());
            //        if (dr.Active == "I")
            //                    gvVendor.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

            //                RowCnt++;  
            //        }

                
            //    if (RowCnt > 0)
            //    {
            //        if (string.IsNullOrEmpty(Sel_Number))
            //            gvVendor.Rows[0].Tag = 0;
            //        else
            //        {
            //            gvVendor.CurrentCell = gvVendor.Rows[Sel_Vendor_Index].Cells[1];

            //            int scrollPosition = 0;
            //            scrollPosition = gvVendor.CurrentCell.RowIndex;
            //            //int CurrentPage = (scrollPosition / gvVendor.ItemsPerPage);
            //            //CurrentPage++;
            //            //gvVendor.CurrentPage = CurrentPage;
            //            //gvVendor.FirstDisplayedScrollingRowIndex = scrollPosition;
            //        }
            //        //ToolBarEdit.Enabled = true; ToolBarDel.Enabled = true;
            //    }
            //    else
            //    {
            //        //ToolBarEdit.Enabled = false; ToolBarDel.Enabled = false;
            //    }

            //}
            //else
            {
                foreach (CaseVDD1Entity Entity in Vdd1list)
                {

                    if (((ListItem)cmbSource.SelectedItem).Value.ToString().Trim() != "**")
                    {
                        if ((Entity.FUEL_TYPE1.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE2.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
                            (Entity.FUEL_TYPE3.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE4.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
                            (Entity.FUEL_TYPE5.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE6.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
                            (Entity.FUEL_TYPE7.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE8.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) ||
                            (Entity.FUEL_TYPE9.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()) || (Entity.FUEL_TYPE10.Trim() == ((ListItem)cmbSource.SelectedItem).Value.ToString().Trim()))
                        {

                            foreach (CASEVDDEntity dr in CaseVddlist)
                            {
                                if (dr.Code.Trim() == Entity.Code.Trim())
                                {
                                    string Zip = string.Empty;
                                    if (dr.Zip.Trim().Length > 5)
                                        Zip = dr.Zip.Substring(0, 5) + "-" + dr.Zip.Substring(5, dr.Zip.Length - 5);
                                    else
                                        Zip = SetLeadingZeros(dr.Zip.Trim());

                                    string Addresses = string.Empty;
                                    Addresses = dr.Addr1.Trim();
                                    if (!string.IsNullOrEmpty(Addresses.Trim())) Addresses = Addresses + " " + dr.Addr2.Trim(); else Addresses = dr.Addr2.Trim();
                                    if (!string.IsNullOrEmpty(Addresses.Trim())) Addresses = Addresses + " " + dr.Addr3.Trim(); else Addresses = dr.Addr3.Trim();

                                    rowIndex = gvVendor.Rows.Add(dr.Code.Trim(),  dr.Name.Trim() , Addresses, dr.City.Trim(),dr.State.Trim(), Zip, Entity.IndexBy.Trim());
                                    if (dr.Active == "I")
                                        gvVendor.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                                    string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + Entity.Add_Date.ToString() + "\n" +
                                            "Modified By  : " + Entity.Lsct_Operator.ToString().Trim() + " on " + Entity.Lstc_Date.ToString();
                                    foreach (DataGridViewCell cell in gvVendor.Rows[rowIndex].Cells)
                                    {
                                        cell.ToolTipText = toolTipText;
                                    }
                                    if (Sel_Number.Trim() == Entity.Code)
                                        Sel_Vendor_Index = RowCnt;
                                    RowCnt++;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (CASEVDDEntity dr in CaseVddlist)
                        {
                            if (dr.Code.Trim() == Entity.Code.Trim())
                            {
                                string Zip = string.Empty;
                                if (dr.Zip.Trim().Length > 5)
                                    Zip = dr.Zip.Substring(0, 5) + "-" + dr.Zip.Substring(5, dr.Zip.Length - 5);
                                else
                                    Zip = SetLeadingZeros(dr.Zip.Trim());

                                string Addresses = string.Empty;
                                Addresses = dr.Addr1.Trim();
                                if (!string.IsNullOrEmpty(Addresses.Trim())) Addresses = Addresses + " " + dr.Addr2.Trim(); else Addresses = dr.Addr2.Trim();
                                if (!string.IsNullOrEmpty(Addresses.Trim())) Addresses = Addresses + " " + dr.Addr3.Trim(); else Addresses = dr.Addr3.Trim();

                                rowIndex = gvVendor.Rows.Add(dr.Code.Trim(), dr.Name.Trim() , Addresses, dr.City.Trim(), dr.State.Trim(), Zip, Entity.IndexBy.Trim());
                                if (dr.Active == "I")
                                    gvVendor.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                                string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + Entity.Add_Date.ToString() + "\n" +
                                        "Modified By  : " + Entity.Lsct_Operator.ToString().Trim() + " on " + Entity.Lstc_Date.ToString();
                                foreach (DataGridViewCell cell in gvVendor.Rows[rowIndex].Cells)
                                {
                                    cell.ToolTipText = toolTipText;
                                }
                                if (Sel_Number.Trim() == Entity.Code)
                                    Sel_Vendor_Index = RowCnt;
                                RowCnt++;
                            }
                        }

                    }
                    if (RowCnt > 0)
                    {
                        if (string.IsNullOrEmpty(Sel_Number))
                            gvVendor.Rows[0].Tag = 0;
                        else
                        {
                            gvVendor.CurrentCell = gvVendor.Rows[Sel_Vendor_Index].Cells[0];

                            int scrollPosition = 0;
                            scrollPosition = gvVendor.CurrentCell.RowIndex;
                            //int CurrentPage = (scrollPosition / gvVendor.ItemsPerPage);
                            //CurrentPage++;
                            //gvVendor.CurrentPage = CurrentPage;
                            //gvVendor.FirstDisplayedScrollingRowIndex = scrollPosition;
                        }
                        //ToolBarEdit.Enabled = true; ToolBarDel.Enabled = true;
                    }
                    else
                    {
                        //ToolBarEdit.Enabled = false; ToolBarDel.Enabled = false;
                    }

                }
            }
            if (gvVendor.Rows.Count > 0)
            {
                //if(!string.IsNullOrEmpty(Sel_Number.Trim()))
                //    gvVendor.Rows[0].Selected = true;
                //else
                //{
                    gvVendor.Rows[Sel_Vendor_Index].Selected = true;
                //}
            }
        }

        string Added_Edited_VendorCode = string.Empty; string Added_Edited_Type = string.Empty; string Mode = string.Empty;
        private void Vendor_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            VendorMaintenance_Form form = sender as VendorMaintenance_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Vendor_Code();
                Added_Edited_VendorCode = From_Results[0];
                Mode = From_Results[1];
                Added_Edited_Type = From_Results[2];
                if (From_Results[1].Equals("Add"))
                {
                    AlertBox.Show("Vendor Details Inserted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight); //MessageBox.Show("Vendor Details Inserted Successfully...", "CAPTAIN");
                }
                else
                    AlertBox.Show("Vendor Details Updated Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);//MessageBox.Show("Vendor Details Updated Successfully...", "CAPTAIN");
                FillVendorGrid(Added_Edited_VendorCode);
                
            }
        }


        public void Delete_Vendor_Row(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
                if (dialogresult == DialogResult.Yes)
                {
                    string strmsg = string.Empty; string strSqlmsg = string.Empty;
                    CASEVDDEntity vddEntity = new CASEVDDEntity(true);
                    CaseVDD1Entity vdd1Entity = new CaseVDD1Entity(true);
                    vddEntity.Rec_Type = "D";
                    vddEntity.Code = gvVendor.CurrentRow.Cells["Number"].Value.ToString();
                    CaptainModel model = new CaptainModel();
                    Added_Edited_VendorCode = string.Empty;

                    //string code = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString();
                    if (_model.SPAdminData.UpdateCASEVDD(vddEntity, "Update", out strmsg, out strSqlmsg))
                    {
                        vdd1Entity.Rec_Type = "D";
                        vdd1Entity.Code = gvVendor.CurrentRow.Cells["Number"].Value.ToString();
                        _model.SPAdminData.UpdateCASEVDD1(vdd1Entity, "Update", out strSqlmsg);
                    //MessageBox.Show("Vendor Number for " + gvVendor.CurrentRow.Cells["Number"].Value.ToString() + " " + "Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    AlertBox.Show("Vendor Number: " + gvVendor.CurrentRow.Cells["Number"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    if (strIndex != 0)
                            strIndex = strIndex - 1;
                        FillVendorGrid(Added_Edited_VendorCode);
                    }
                    else
                        if (strmsg == "Already Exist")
                            AlertBox.Show("This Vendor has been used somewhere, so unable to Delete", MessageBoxIcon.Warning);
                        else
                            AlertBox.Show("Failed to Delete: " + gvVendor.CurrentRow.Cells["Number"]/*Unsuccessful Delete*/, MessageBoxIcon.Warning);
                }
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

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }

            return (TmpCode);
        }

    }

    }

