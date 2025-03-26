#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
//using Wisej.Web.Design;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Controls.Compatibility;


using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using CarlosAg.ExcelXmlWriter;
using System.IO;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ADMN0013 : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private GridControl _serviceHierarchy = null;
        CaptainModel _model = null;
        #endregion
        public string strZipCode { get; set; }
        public int strIndex = 0;
        public int strPageIndex = 1;
        public string strDeletMsg = string.Empty;
        public void fillcombo()
        {
            List<CommonEntity> Township = _model.lookupDataAccess.GetTownship();//_model.ZipCodeAndAgency.GetTownship();
            foreach (CommonEntity township in Township)
            {
                cmbTownship.Items.Add(new Utilities.ListItem(township.Desc, township.Code));
            }
            cmbTownship.Items.Insert(0, new Utilities.ListItem("All", "0"));
            cmbTownship.SelectedIndex = 0;

            List<CommonEntity> Country = _model.ZipCodeAndAgency.GetCounty();
            foreach (CommonEntity country in Country)
            {
                cmbCounty.Items.Add(new Utilities.ListItem(country.Desc, country.Code));
            }
            cmbCounty.Items.Insert(0, new Utilities.ListItem("All", "0"));
            cmbCounty.SelectedIndex = 0;

        }

        public ADMN0013(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            txtZipCode.Validator = TextBoxValidation.IntegerValidator;
            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
            if (zipcodeEntity.Count > 0)
                //int selectedRow = 0;
            zipcodeEntity = zipcodeEntity.OrderBy(u => Convert.ToInt64(u.Zcrzip)).ToList();
            foreach (ZipCodeEntity zipdetails in zipcodeEntity)
            {
                string zipPlus = zipdetails.Zcrplus4.ToString();
                zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                int rowIndex = gvwCustomer.Rows.Add(SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity, zipdetails.Zcrstate.ToString(), zipdetails.TownSHip, zipdetails.County, zipdetails.Zcrintakecode);
                gvwCustomer.Rows[rowIndex].Tag = zipdetails;
                if (zipdetails.InActive.ToString() == "Y")
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
               // selectedRow = rowIndex;
                CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwCustomer);
            }
            fillcombo();
            if(gvwCustomer.Rows.Count>0)
                 gvwCustomer.Rows[0].Selected = true;

            TabControls();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            PopulateToolbar(oToolbarMnustrip);
        }

        private void TabControls()
        {
            _serviceHierarchy = new GridControl(BaseForm, "Intake", null);
            _serviceHierarchy.Dock = DockStyle.Fill;

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string propReportPath { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarPrint { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

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
                ToolBarNew.ToolTipText = "Add New ZIP Code";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";//new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit ZIP Code";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";//new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete ZIP Code";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";//new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarPrint = new ToolBarButton();
                ToolBarPrint.Tag = "Print";
                ToolBarPrint.ToolTipText = "Print";
                ToolBarPrint.Enabled = true;
                ToolBarPrint.ImageSource = "captain-print";//new IconResourceHandle(Consts.Icons16x16.Print);
                ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.ToolTipText = "ZIP Code Report in Excel";
                ToolBarExcel.Enabled = true;
                ToolBarExcel.ImageSource = "captain-excel"; //new IconResourceHandle(Consts.Icons16x16.MSExcel);
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "ZIP Code Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help"; //new IconResourceHandle(Consts.Icons16x16.Help);
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }

            if (Privileges.AddPriv.Equals("false"))
            {
                if (ToolBarNew != null) ToolBarNew.Enabled = false;
            }
            else
            {
                if (ToolBarNew != null) ToolBarNew.Enabled = true;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
            }
            else
            {
                if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
            }

            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = false;
            }
            else
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = true;
            }

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                ToolBarPrint,
                ToolBarExcel,
                ToolBarHelp
            });
        }

        /// <summary>
        /// Handles the toolbar button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        if (gvwCustomer.SortOrder.ToString() != "None")
                        { SortColumn = gvwCustomer.SortedColumn.Name.ToString(); SortOrder = gvwCustomer.SortOrder.ToString(); }
                        ZipCodeForm addUserForm = new ZipCodeForm(BaseForm, "Add", null, Privileges);
                        addUserForm.StartPosition= FormStartPosition.CenterScreen;
                        addUserForm.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                        string selectedRowZcrZip = GetSelectedRow();
                        if (selectedRowZcrZip != null)
                        {
                            if (gvwCustomer.SortOrder.ToString() != "None")
                            { SortColumn = gvwCustomer.SortedColumn.Name.ToString(); SortOrder = gvwCustomer.SortOrder.ToString(); }

                            ZipCodeForm editUserForm = new ZipCodeForm(BaseForm, "Edit", selectedRowZcrZip, Privileges);
                            editUserForm.StartPosition = FormStartPosition.CenterScreen;
                            editUserForm.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        selectedRowZcrZip = GetSelectedRow();
                        if (selectedRowZcrZip != null)
                        {
                            if (gvwCustomer.SortOrder.ToString() != "None")
                            { SortColumn = gvwCustomer.SortedColumn.Name.ToString(); SortOrder = gvwCustomer.SortOrder.ToString(); }
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nZIP Code: " + selectedRowZcrZip, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandler);
                        }
                        break;
                    case Consts.ToolbarActions.Print:
                        On_SaveForm_Closed();
                        break;
                    case Consts.ToolbarActions.Excel:
                        if (gvwCustomer.Rows.Count > 0)
                            On_SaveExcelClosed();
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

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
          //  Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    string strZipcodeALL = GetSelectedRow();
                    string strZipCode = strZipcodeALL.Substring(0, 5).ToString();
                    string strZipPlus = strZipcodeALL.Substring(6, 4).ToString().TrimStart('0');
                    if (strZipPlus == string.Empty)
                        strZipPlus = "0";
                    ZipCodeEntity zipCodeDetails = new ZipCodeEntity();
                    zipCodeDetails.Zcrzip = strZipCode;
                    zipCodeDetails.Zcrplus4 = strZipPlus;
                    zipCodeDetails.Mode = "Del";

                string ZipCodeID = string.Empty;
                int SelIndex = gvwCustomer.CurrentRow.Index;
                if (gvwCustomer.Rows.Count - 1 >= SelIndex + 1)
                    ZipCodeID = gvwCustomer.Rows[SelIndex + 1].Cells["ZCRZIP"].Value.ToString();
                else
                    ZipCodeID = gvwCustomer.Rows[0].Cells["ZCRZIP"].Value.ToString();

                if (_model.ZipCodeAndAgency.InsertUpdateDelZIPCODE(zipCodeDetails))
                {
                    AlertBox.Show("ZIP Code: " + gvwCustomer.CurrentRow.Cells["ZCRZIP"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    if (strIndex != 0)
                        strIndex = strIndex - 1;

                    if (ZipCodeID.Contains("-")) {

                        ZipCodeID= ZipCodeID.Split('-')[0];
                    }
                    RefreshGrid(ZipCodeID);
                }
                else
                {
                    AlertBox.Show("You can’t delete this Zipcode, as there are Dependencies", MessageBoxIcon.Warning);
                }
            }
            //}
        }

        public int strQuesIndex = 0;
        public int strGroupIndex = 0;
        public string SortColumn = string.Empty;
        public string SortOrder = string.Empty;
        public string strQuesID = string.Empty;
        public void RefreshGrid()
        {
            strDeletMsg = "Delete";
            onSearch_Click(onSearch, new EventArgs());
            if (gvwCustomer.Rows.Count != 0)
            {
                gvwCustomer.Rows[strIndex].Selected = true;
                gvwCustomer.CurrentCell = gvwCustomer.Rows[strIndex].Cells[1];
               // gvwCustomer.CurrentPage = strPageIndex;
            }

            if (!string.IsNullOrEmpty(SortOrder))
            {
                gvwCustomer.SortedColumn.Name = SortColumn;
                if (SortOrder == "Ascending")
                    this.gvwCustomer.Sort(this.gvwCustomer.Columns[SortColumn], ListSortDirection.Ascending);
                else
                    this.gvwCustomer.Sort(this.gvwCustomer.Columns[SortColumn], ListSortDirection.Descending);
            }
        }

        public void RefreshGrid(string strZipcode)
        {
            strDeletMsg = "Delete";
            //onSearch_Click(onSearch, new EventArgs());
            gvwCustomer.Rows.Clear();

            string zipcode = txtZipCode.Text;
            string city = txtCity.Text.ToString();
            string township = string.Empty;
            string county = string.Empty;
            if (!((Utilities.ListItem)cmbTownship.SelectedItem).Value.ToString().Equals("0"))
            {
                township = ((Utilities.ListItem)cmbTownship.SelectedItem).Value.ToString();
            }
            if (!((Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString().Equals("0"))
            {
                county = ((Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString();
            }

            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(zipcode, city, township, county);
            if (zipcodeEntity.Count > 0)
            {
                zipcodeEntity = zipcodeEntity.OrderBy(u => Convert.ToInt64(u.Zcrzip)).ToList();
                foreach (ZipCodeEntity zipdetails in zipcodeEntity)
                {
                    string zipPlus = zipdetails.Zcrplus4.ToString();
                    zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    string strzip = SetLeadingZeros(zipdetails.Zcrzip.ToString());
                    string chkstrzip = strzip + "-" + zipPlus;
                    int rowIndex = gvwCustomer.Rows.Add(strzip + "-" + zipPlus, zipdetails.Zcrcity, zipdetails.Zcrstate.ToString(), zipdetails.TownSHip, zipdetails.County, zipdetails.Zcrintakecode);
                    gvwCustomer.Rows[rowIndex].Tag = zipdetails;
                    if (zipdetails.InActive.ToString() == "Y")
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwCustomer);
                    if (strZipcode == strzip)
                    {
                        strIndex = rowIndex;
                    }
                }
                if (gvwCustomer.Rows.Count > 0)
                    gvwCustomer.Rows[0].Selected = true;
            }
            else
            {
                if (strDeletMsg == string.Empty)
                    AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
            if (!string.IsNullOrEmpty(SortOrder))
            {
                gvwCustomer.SortedColumn.Name = SortColumn;
                if (SortOrder == "Ascending")
                    this.gvwCustomer.Sort(this.gvwCustomer.Columns[SortColumn], ListSortDirection.Ascending);
                else
                    this.gvwCustomer.Sort(this.gvwCustomer.Columns[SortColumn], ListSortDirection.Descending);
            }

            strDeletMsg = string.Empty;
            if (gvwCustomer.Rows.Count != 0)
            {
                gvwCustomer.Rows[strIndex].Selected = true;
                gvwCustomer.CurrentCell = gvwCustomer.Rows[strIndex].Cells[1];
               // gvwCustomer.CurrentPage = ((strIndex + gvwCustomer.ItemsPerPage) / gvwCustomer.ItemsPerPage);
            }
           
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRow()
        {
            string ZcrZipID = null;
            if (gvwCustomer != null)
            {
                foreach (DataGridViewRow dr in gvwCustomer.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        strIndex = gvwCustomer.SelectedRows[0].Index;
                       // strPageIndex = gvwCustomer.CurrentPage;
                        ZipCodeEntity zipdetails = dr.Tag as ZipCodeEntity;
                        if (zipdetails != null)
                        {
                            string zipPlus = zipdetails.Zcrplus4;
                            zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                            ZcrZipID = SetLeadingZeros(zipdetails.Zcrzip);
                            ZcrZipID = ZcrZipID +"-"+ zipPlus.ToString();
                            break;
                        }
                    }
                }
            }
            return ZcrZipID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (Utilities.ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void SetImageTypes(string ImageTypes)
        {
            if (ImageTypes == null || ImageTypes.Equals(string.Empty)) { return; }
            string[] imageTypes = ImageTypes.Split(' ');
            List<string> imageTypeList = new List<string>();

            for (int i = 0; i < imageTypes.Length; i++)
            {
                imageTypeList.Add(imageTypes.GetValue(i).ToString());
            }

            foreach (Utilities.ListItem li in cmbImageTypes.comboBox.Items)
            {
                if (imageTypeList.Contains(li.Value.ToString()))
                {
                    cmbImageTypes.InternalListBox.Items.Add(li);
                }
            }
            cmbImageTypes.SetEditable(false);
        }

        private void onSearch_Click(object sender, EventArgs e)
        {
            //  gvwCustomer.SetPropertyValue(gvwCustomer.SelectedRows[0].Selected.ToString(), 6);
            gvwCustomer.Rows.Clear();

            string zipcode = txtZipCode.Text;
            string city = txtCity.Text.ToString();
            string township = string.Empty;
            string county = string.Empty;
            if (!((Utilities.ListItem)cmbTownship.SelectedItem).Value.ToString().Equals("0"))
            {
                township = ((Utilities.ListItem)cmbTownship.SelectedItem).Value.ToString();
            }
            if (!((Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString().Equals("0"))
            {
                county = ((Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString();
            }

            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(zipcode, city, township, county);
            if (zipcodeEntity.Count > 0)
            {
                zipcodeEntity = zipcodeEntity.OrderBy(u => Convert.ToInt64(u.Zcrzip)).ToList();
                foreach (ZipCodeEntity zipdetails in zipcodeEntity)
                {
                    string zipPlus = zipdetails.Zcrplus4.ToString();
                    zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    int rowIndex = gvwCustomer.Rows.Add(SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity, zipdetails.Zcrstate.ToString(), zipdetails.TownSHip, zipdetails.County, zipdetails.Zcrintakecode);
                    gvwCustomer.Rows[rowIndex].Tag = zipdetails;
                    if (zipdetails.InActive.ToString() == "Y")
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwCustomer);
                }
                if (gvwCustomer.Rows.Count > 0)
                    gvwCustomer.Rows[0].Selected = true;
            }
            else
            {
                if (strDeletMsg == string.Empty)
                    //AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Error, null, ContentAlignment.BottomRight);
                AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
            strDeletMsg = string.Empty;
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

        private void txtZipCode_Leave(object sender, EventArgs e)
        {
            //strZipCode =  txtZipCode.Text;
            //strZipCode = strZipCode.TrimStart('0'); 

            //txtZipCode.Text = SetLeadingZeros(txtZipCode.Text);
        }

        #region Reportpdfandexcel


        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        int pageNumber = 1; string PdfName = "Pdf File";
        string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        string PrintText = null;
        private void On_SaveForm_Closed()
        {
            Random_Filename = null;

            PdfName = "Zipcodefilemaintenance_Report";
            
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            cb = writer.DirectContent;
            //string Priv_Scr = null;
            //document = new Document(iTextSharp.text.PageSize.A4.Rotate());

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 12, 1, BaseColor.BLUE);

           
            try
            {
                if (gvwCustomer.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(5);
                    table.TotalWidth = 520f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 80f, 100f, 80f, 100f, 100f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 2;

                    PdfPCell Hdr = new PdfPCell(new Phrase("ZIP Code File Maintenance", TblFontBoldColor));
                    Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdr.FixedHeight = 15f;
                    Hdr.Colspan = 5;
                    Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(Hdr);

                    string[] Header = { "ZIP Code", "City", "State", "Township", "County" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 15f;
                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        table.AddCell(cell);
                    }

                    foreach (DataGridViewRow gvrow in gvwCustomer.Rows)
                    {
                        
                        PdfPCell Cell1 = new PdfPCell(new Phrase(gvrow.Cells["ZCRZIP"].Value.ToString().Trim(), TableFont));
                        Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell1);

                        PdfPCell Cell2 = new PdfPCell(new Phrase(gvrow.Cells["ZCRCITY"].Value.ToString().Trim(), TableFont));
                        Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell2);

                        PdfPCell Cell3 = new PdfPCell(new Phrase(gvrow.Cells["ZCRSTATE"].Value.ToString().Trim(), TableFont));
                        Cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell3);

                        PdfPCell Cell4 = new PdfPCell(new Phrase(gvrow.Cells["ZCRCITYCODE"].Value.ToString().Trim(), TableFont));
                        Cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell4);

                        PdfPCell Cell5 = new PdfPCell(new Phrase(gvrow.Cells["ZCRCOUNTY"].Value.ToString().Trim(), TableFont));
                        Cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell5);
                        
                    }
                                        

                    document.Add(table);

                }
                else
                {
                    document.Add(new Paragraph("No Records to define to this Criteria............................................... "));
                }

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

            document.Close();
            fs.Close();
            fs.Dispose();


            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }


        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }


        private void On_SaveExcelClosed()
        {

            //PdfListForm form = sender as PdfListForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            string PdfName = "Zipcodefilemaintenance_Report";
            //PdfName = form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }


            try
            {
                string Tmpstr = PdfName + ".xls";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xls";


            

            string data = null;
            


            Workbook book = new Workbook();

            Worksheet sheet; WorksheetCell cell; WorksheetRow Row0;

            this.GenerateStyles(book.Styles);

            sheet = book.Worksheets.Add("Sheet1");
            sheet.Table.DefaultRowHeight = 14.25F;

            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(150);
            sheet.Table.Columns.Add(80);
            sheet.Table.Columns.Add(150);
            sheet.Table.Columns.Add(150);
            

            Row0 = sheet.Table.Rows.Add();


            int excelcolumn = 0;

            try
            {

              

                WorksheetCell R3Cell = Row0.Cells.Add("Zip Code File Maintenance", DataType.String, "s94");
                R3Cell.MergeAcross = 4;


                if (gvwCustomer.Rows.Count > 0)
                {

                    Row0 = sheet.Table.Rows.Add();

                    cell = Row0.Cells.Add("Zip Code", DataType.String, "s94");
                    cell = Row0.Cells.Add("City", DataType.String, "s94");
                    cell = Row0.Cells.Add("State", DataType.String, "s94");
                    cell = Row0.Cells.Add("Township", DataType.String, "s94");
                    cell = Row0.Cells.Add("County", DataType.String, "s94");
                    
                    
                    foreach (DataGridViewRow gvrow in gvwCustomer.Rows)
                    {

                        Row0 = sheet.Table.Rows.Add();
                        
                        cell = Row0.Cells.Add(gvrow.Cells["ZCRZIP"].Value.ToString().Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(gvrow.Cells["ZCRCITY"].Value.ToString().Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(gvrow.Cells["ZCRSTATE"].Value.ToString().Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(gvrow.Cells["ZCRCITYCODE"].Value.ToString().Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(gvrow.Cells["ZCRCOUNTY"].Value.ToString().Trim(), DataType.String, "s95");
                                               

                    }

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = "Zipcodefilemaintenance_Report.xls";

                //// downloadGateway.Version = file.Version;

                //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

                FileInfo fiDownload = new FileInfo(PdfName);
                /// Need to check for file exists, is local file, is allow to read, etc...
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }

            }
            catch (Exception ex) { }


            //}
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.NumberFormat = "0%";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Arial";
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s21.NumberFormat = "0%";
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Interior.Color = "#D8D8D8";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Interior.Color = "#D8D8D8";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Interior.Color = "#D8D8D8";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.NumberFormat = "0%";
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Interior.Color = "#D8D8D8";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Interior.Color = "#D8D8D8";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s28.NumberFormat = "0%";
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.NumberFormat = "0%";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s69.NumberFormat = "0%";
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.NumberFormat = "0%";
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s72.NumberFormat = "0%";
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.NumberFormat = "0%";
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.NumberFormat = "0%";
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.NumberFormat = "0%";
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.NumberFormat = "0%";
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.NumberFormat = "0%";
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Arial";
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.Bold = true;
            s82.Font.FontName = "Arial";
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.NumberFormat = "0%";
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Arial";
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s84.NumberFormat = "0%";
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Arial";
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.NumberFormat = "0%";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Arial";
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s87.NumberFormat = "0%";
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s90.NumberFormat = "0%";
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Font.Bold = true;
            s92.Font.Italic = true;
            s92.Font.FontName = "Arial";
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s92.NumberFormat = "0%";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.Italic = true;
            s93.Font.FontName = "Arial";
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Arial";
            s94.Font.Color = "#000000";
            s94.Interior.Color = "#B0C4DE";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Top;
            s94.Alignment.WrapText = true;
            s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s95B
            // -----------------------------------------------
            WorksheetStyle s95B = styles.Add("s95B");
            s95B.Font.FontName = "Arial";
            s95B.Font.Bold = true;
            s95B.Font.Color = "#0000FF";
            s95B.Interior.Color = "#FFFFFF";
            s95B.Interior.Pattern = StyleInteriorPattern.Solid;
            s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95B.Alignment.WrapText = true;
            s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //  s95R
            // -----------------------------------------------
            WorksheetStyle s95R = styles.Add("s95R");
            s95R.Font.FontName = "Arial";
            //s95R.Font.Bold = true;
            s95R.Font.Color = "#FF0000";
            s95R.Interior.Color = "#FFFFFF";
            s95R.Interior.Pattern = StyleInteriorPattern.Solid;
            s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95R.Alignment.WrapText = true;
            s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Font.Bold = true;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.Bold = true;
            s97.Font.FontName = "Arial";
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.NumberFormat = "0%";
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Arial";
            s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Arial";
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.NumberFormat = "0%";
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.NumberFormat = "0%";
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.NumberFormat = "0%";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Arial";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.NumberFormat = "0%";
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.NumberFormat = "0%";
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.NumberFormat = "0%";
        }


        #endregion

    }
}