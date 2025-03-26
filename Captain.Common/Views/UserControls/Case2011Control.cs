#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using CarlosAg.ExcelXmlWriter;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class Case2011Control : BaseUserControl
    {
        #region private variables
        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();

        public int strQuesIndex = 0;
        public int strGroupIndex = 0;
        public string SortColumn = string.Empty;
        public string SortOrder = string.Empty;
        public string strQuesID = string.Empty;

        #endregion
        public Case2011Control(BaseForm baseform, PrivilegeEntity Priviliages)
        {
            InitializeComponent();
          
            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = Priviliages;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            FormLoad(Added_Edited_RefralCode);
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

        public ToolBarButton ToolBarExcel { get; set; }

        public bool IsSaveValid { get; set; }

        public string propReportPath { get; set; }
        //public List<HierarchyEntity> SelectedHierarchies
        //{
        //    get
        //    {
        //        return _selectedHierarchies = (from b in gvSerivePlanHie.Rows.Cast<DataGridViewRow>().ToList()
        //                                       select ((DataGridViewRow)b).Tag as HierarchyEntity).ToList();

        //    }
        //}

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
                ToolBarNew.ToolTipText = "Add Agency Referral";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add"; //@"Resources\Images\16x16\addItem.png";   //new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Agency Referral";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";// @"Resources\Images\16X16\editItem.png"; //new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Agency Referral";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";// @"Resources\Images\16X16\delete.png"; //new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarPrint = new ToolBarButton();
                ToolBarPrint.Tag = "Print";
                ToolBarPrint.ToolTipText = "Print";
                ToolBarPrint.Enabled = true;
                ToolBarPrint.ImageSource = "captain-print";//@"Resources\Images\16X16\print.png";  //new IconResourceHandle(Consts.Icons16x16.Print);
                ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.ToolTipText = "Agency Referral Report in Excel";
                ToolBarExcel.Enabled = true;
                ToolBarExcel.ImageSource = "captain-excel"; //@"Resources\Images\16X16\msexcel.png";     //new IconResourceHandle(Consts.Icons16x16.MSExcel);
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help"; //@"Resources\Images\16X16\help.png"; //new IconResourceHandle(Consts.Icons16x16.Help);
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
                ToolBarPrint,
                ToolBarExcel,
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

                        if (gvAgencyReferal.SortOrder.ToString() != "None")
                        { SortColumn = gvAgencyReferal.SortedColumn.Name.ToString(); SortOrder = gvAgencyReferal.SortOrder.ToString(); }

                        Added_Edited_RefralCode = string.Empty;
                        Case2011Form AgencyReferalForm_Add = new Case2011Form(BaseForm, "Add", "", Privileges);
                        AgencyReferalForm_Add.FormClosed += new FormClosedEventHandler(Referal_AddForm_Closed);
                        AgencyReferalForm_Add.StartPosition = FormStartPosition.CenterScreen;
                        AgencyReferalForm_Add.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                        if (gvAgencyReferal.Rows.Count > 0)
                        {
                            if (gvAgencyReferal.SortOrder.ToString() != "None")
                            { SortColumn = gvAgencyReferal.SortedColumn.Name.ToString(); SortOrder = gvAgencyReferal.SortOrder.ToString(); }

                            Case2011Form AgencyReferalForm_Edit = new Case2011Form(BaseForm, "Edit", gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString(), Privileges);
                            AgencyReferalForm_Edit.FormClosed += new FormClosedEventHandler(Referal_AddForm_Closed);
                            AgencyReferalForm_Edit.StartPosition = FormStartPosition.CenterScreen;
                            AgencyReferalForm_Edit.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (gvAgencyReferal.Rows.Count > 0)
                        {
                            if (gvAgencyReferal.SortOrder.ToString() != "None")
                            { SortColumn = gvAgencyReferal.SortedColumn.Name.ToString(); SortOrder = gvAgencyReferal.SortOrder.ToString(); }

                            strIndex = gvAgencyReferal.SelectedRows[0].Index;
                            // strPageIndex = gvAgencyReferal.CurrentPage;
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Agency Referral with Code: " + gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Referral_Row);
                        }
                        break;
                    case Consts.ToolbarActions.Print:
                        On_SaveForm_Closed();
                        break;
                    case Consts.ToolbarActions.Excel:
                        if (gvAgencyReferal.Rows.Count > 0)
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

        List<CASEREFEntity> CaseRefList;
        private void FormLoad(string Sel_Code)
        {
            gvAgencyReferal.Rows.Clear();
            int rowIndex = 0; int RowCnt = 0; int Sel_CA_Index = 0;
            CASEREFEntity Search_Entity = new CASEREFEntity(true);
            if (rbInactive.Checked)
            {
                Search_Entity.Active = "N";
            }
            else if (rbActive.Checked)
                Search_Entity.Active = "Y";
            CaseRefList = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");
            if (CaseRefList.Count > 0)
            {
                foreach (CASEREFEntity Entity in CaseRefList)
                {
                    rowIndex = gvAgencyReferal.Rows.Add(Entity.Code, Entity.Name1.Trim(), Entity.Name2.Trim(), Entity.Street, Entity.City, Entity.State, Entity.NameIndex.Trim());
                    if (Entity.Active == "N")
                        gvAgencyReferal.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + Entity.Add_Date.ToString() + "\n" +
                                "Modified By  : " + Entity.Lsct_Operator.ToString().Trim() + " on " + Entity.Lstc_Date.ToString();
                    foreach (DataGridViewCell cell in gvAgencyReferal.Rows[rowIndex].Cells)
                    {
                        cell.ToolTipText = toolTipText;
                    }
                    if (Sel_Code.Trim() == Entity.Code)
                        Sel_CA_Index = RowCnt;
                    RowCnt++;
                }


            }

            if (RowCnt > 0)
            {
                if (string.IsNullOrEmpty(Sel_Code))
                {
                    gvAgencyReferal.Rows[0].Tag = 0;
                    gvAgencyReferal.Rows[0].Selected = true;
                }
                else
                {
                    gvAgencyReferal.CurrentCell = gvAgencyReferal.Rows[Sel_CA_Index].Cells[1];

                    int scrollPosition = 0;
                    scrollPosition = gvAgencyReferal.CurrentCell.RowIndex;
                    //int CurrentPage = (scrollPosition / gvAgencyReferal.ItemsPerPage);
                    //CurrentPage++;
                    //gvAgencyReferal.CurrentPage = CurrentPage;
                    //gvAgencyReferal.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
            }


        }

        string Added_Edited_RefralCode = string.Empty;
        private void Referal_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Case2011Form form = sender as Case2011Form;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[2];
                From_Results = form.GetSelected_Agency_Code();
                Added_Edited_RefralCode = From_Results[0];

                //if (From_Results[1].Equals("Add"))
                //    MessageBox.Show("Agency Referal Details inserted Successfully...", "CAPTAIN");
                //else
                //    MessageBox.Show("Agency Referal Details Updated Successfully...", "CAPTAIN");
                FormLoad(Added_Edited_RefralCode);

                if (!string.IsNullOrEmpty(SortOrder))
                {
                    gvAgencyReferal.SortedColumn.Name = SortColumn;
                    if (SortOrder == "Ascending")
                        this.gvAgencyReferal.Sort(this.gvAgencyReferal.Columns[SortColumn], ListSortDirection.Ascending);
                    else
                        this.gvAgencyReferal.Sort(this.gvAgencyReferal.Columns[SortColumn], ListSortDirection.Descending);
                }

                //btnSearch_Click(btnSearch, EventArgs.Empty);
            }
        }

        private void rbBoth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActive.Checked)
            {
                FormLoad(string.Empty);
                txtSearchAgyName.Clear();
            }
            else if (rbInactive.Checked)
            {
                FormLoad(string.Empty);
                txtSearchAgyName.Clear();
            }
            else
            {
                FormLoad(string.Empty);
                txtSearchAgyName.Clear();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            gvAgencyReferal.Rows.Clear();
            CASEREFEntity Search_Entity = new CASEREFEntity(true);
            if (rbActive.Checked)
                Search_Entity.Active = "Y";
            else if (rbInactive.Checked)
                Search_Entity.Active = "N";
            if (!string.IsNullOrEmpty(txtSearchAgyName.Text.Trim()))
                Search_Entity.Name1 = txtSearchAgyName.Text.Trim();
            if (!string.IsNullOrEmpty(txtNameIndex.Text.Trim()))
                Search_Entity.NameIndex = txtNameIndex.Text.Trim();
            Search_Entity.Rec_Type = "NAMESSEARCH";
            CaseRefList = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");

            if (CaseRefList.Count > 0)
            {
                foreach (CASEREFEntity Entity in CaseRefList)
                {
                    //if (Entity.Name1.ToString().Trim().Contains(txtSearchAgyName.Text.Trim()))
                    //{
                    rowIndex = gvAgencyReferal.Rows.Add(Entity.Code, Entity.Name1.Trim(), Entity.Name2.Trim(), Entity.Street.Trim(), Entity.City.Trim(), Entity.State, Entity.NameIndex.Trim());
                    if (Entity.Active == "N")
                        gvAgencyReferal.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + Entity.Add_Date.ToString() + "\n" +
                                "Modified By  : " + Entity.Lsct_Operator.ToString().Trim() + " on " + Entity.Lstc_Date.ToString();
                    foreach (DataGridViewCell cell in gvAgencyReferal.Rows[rowIndex].Cells)
                    {
                        cell.ToolTipText = toolTipText;
                    }
                    //}
                }
                //else if (rbInactive.Checked)
                //{
                //    if (Entity.Active == "N")
                //    {
                //        rowIndex = gvAgencyReferal.Rows.Add(Entity.Code, Entity.Name1, Entity.Street, Entity.City, Entity.State);
                //        string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + Entity.Add_Date.ToString() + "\n" +
                //                 "Modified By  : " + Entity.Lsct_Operator.ToString().Trim() + " on " + Entity.Lstc_Date.ToString();
                //        foreach (DataGridViewCell cell in gvAgencyReferal.Rows[rowIndex].Cells)
                //        {
                //            cell.ToolTipText = toolTipText;
                //        }
                //    }
                //}
                //else
                //{
                //    rowIndex = gvAgencyReferal.Rows.Add(Entity.Code, Entity.Name1, Entity.Street, Entity.City, Entity.State);
                //    string toolTipText = "Added By     : " + Entity.Add_Operator.ToString().Trim() + " on " + Entity.Add_Date.ToString() + "\n" +
                //                 "Modified By  : " + Entity.Lsct_Operator.ToString().Trim() + " on " + Entity.Lstc_Date.ToString();
                //    foreach (DataGridViewCell cell in gvAgencyReferal.Rows[rowIndex].Cells)
                //    {
                //        cell.ToolTipText = toolTipText;
                //    }
                //}
            }
        }

        public void Delete_Referral_Row(DialogResult dialogResult)
        {

            if (dialogResult == DialogResult.Yes)
            {
                string strmsg = string.Empty; string strSqlmsg = string.Empty;
                CASEREFEntity RefEntity = new CASEREFEntity(true);
                RefEntity.Rec_Type = "D";
                RefEntity.Code = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString();
                CASEREFSEntity refsEntity = new CASEREFSEntity();
                refsEntity.Rec_Type = "D";
                refsEntity.Code = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString();
                string RefsCode = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString().Trim();
                CaptainModel model = new CaptainModel();
                Added_Edited_RefralCode = string.Empty;

               // string SALID = string.Empty;
                int SelIndex = gvAgencyReferal.CurrentRow.Index;
                if (gvAgencyReferal.Rows.Count - 1 >= SelIndex + 1)
                    Added_Edited_RefralCode = gvAgencyReferal.Rows[SelIndex + 1].Cells["Code"].Value.ToString();
                else
                    Added_Edited_RefralCode = gvAgencyReferal.Rows[0].Cells["Code"].Value.ToString();


                //string code = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString();
                if (_model.SPAdminData.UpdateCASEREF(RefEntity, "Update", out strmsg, out strSqlmsg))
                {

                    _model.SPAdminData.Delete_CaseRefs(RefsCode);
                    AlertBox.Show("Agency Referral with Code: " + gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Agency Referral for " + gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString() + " " + "Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    if (strIndex != 0)
                        strIndex = strIndex - 1;
                    FormLoad(Added_Edited_RefralCode);

                    if (!string.IsNullOrEmpty(SortOrder))
                    {
                        gvAgencyReferal.SortedColumn.Name = SortColumn;
                        if (SortOrder == "Ascending")
                            this.gvAgencyReferal.Sort(this.gvAgencyReferal.Columns[SortColumn], ListSortDirection.Ascending);
                        else
                            this.gvAgencyReferal.Sort(this.gvAgencyReferal.Columns[SortColumn], ListSortDirection.Descending);
                    }

                }
                else
                    if (strmsg == "Already Exist")
                    AlertBox.Show("The selected code has been used. You cannot delete it", MessageBoxIcon.Warning);
                else
                    AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);
            }

        }

        private void gvAgencyReferal_DoubleClick(object sender, EventArgs e)
        {
            if (isHeader =="N")
            {
                Case2011Form AgencyReferalForm_Edit = new Case2011Form(BaseForm, "View", gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString(), Privileges);
                AgencyReferalForm_Edit.FormClosed += new FormClosedEventHandler(Referal_AddForm_Closed);
                AgencyReferalForm_Edit.StartPosition = FormStartPosition.CenterScreen;
                AgencyReferalForm_Edit.ShowDialog();
            }
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        int pageNumber = 1; string PdfName = "Pdf File";
        string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        string PrintText = null;

        private void button1_Click(object sender, EventArgs e)
        {
            int _timer = Convert.ToInt32(textBox1.Text);
            AlertBox.Show("Testing Alertbox timer", autoCloseDelay: _timer);
        }

        private void gvAgencyReferal_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        string isHeader = "Y";
        private void gvAgencyReferal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                isHeader = "N";
            else
                isHeader = "Y";

        }

        private void On_SaveForm_Closed()
        {
            Random_Filename = null;

            PdfName = "AgencyReferral_Report";//form.GetFileName();
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

            CASEREFEntity Search_Entity = new CASEREFEntity(true);
            if (rbInactive.Checked)
            {
                Search_Entity.Active = "N";
            }
            else if (rbActive.Checked)
                Search_Entity.Active = "Y";
            if (!string.IsNullOrEmpty(txtSearchAgyName.Text.Trim()))
                Search_Entity.Name1 = txtSearchAgyName.Text.Trim();
            if (!string.IsNullOrEmpty(txtNameIndex.Text.Trim()))
                Search_Entity.NameIndex = txtNameIndex.Text.Trim();
            CaseRefList = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");

            try
            {
                if (CaseRefList.Count > 0)
                {
                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 70f, 45f, 25f, 13f, 30f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 2;

                    PdfPCell Hdr = new PdfPCell(new Phrase("Agency Referral Database", TblFontBoldColor));
                    Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdr.FixedHeight = 15f;
                    Hdr.Colspan = 6;
                    Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(Hdr);

                    string[] Header = { "Code", "Agency Name", "Street", "City", "State", "Name Index" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.FixedHeight = 15f;
                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        table.AddCell(cell);
                    }

                    foreach (CASEREFEntity Entity in CaseRefList)
                    {
                        PdfPCell Cell1 = new PdfPCell(new Phrase(Entity.Code, TableFont));
                        Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell1);

                        PdfPCell Cell2 = new PdfPCell(new Phrase(Entity.Name1, TableFont));
                        Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell2);

                        PdfPCell Cell3 = new PdfPCell(new Phrase(Entity.Street, TableFont));
                        Cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell3);

                        PdfPCell Cell4 = new PdfPCell(new Phrase(Entity.City, TableFont));
                        Cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell4);

                        PdfPCell Cell5 = new PdfPCell(new Phrase(Entity.State, TableFont));
                        Cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell5);

                        PdfPCell Cell6 = new PdfPCell(new Phrase(Entity.NameIndex, TableFont));
                        Cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                        Cell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Cell6);

                    }

                    //PdfPCell Cell7 = new PdfPCell(new Phrase("", TableFont));
                    //Cell7.HorizontalAlignment = Element.ALIGN_LEFT;
                    //Cell7.Colspan = 6;
                    //Cell7.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                    //table.AddCell(Cell7);

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
            string PdfName = "AgencyReferral_Report";
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


            CASEREFEntity Search_Entity = new CASEREFEntity(true);
            if (rbInactive.Checked)
            {
                Search_Entity.Active = "N";
            }
            else if (rbActive.Checked)
                Search_Entity.Active = "Y";
            if (!string.IsNullOrEmpty(txtSearchAgyName.Text.Trim()))
                Search_Entity.Name1 = txtSearchAgyName.Text.Trim();
            if (!string.IsNullOrEmpty(txtNameIndex.Text.Trim()))
                Search_Entity.NameIndex = txtNameIndex.Text.Trim();
            CaseRefList = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");

            string data = null;
            //int i = 0;
            //int j = 0;


            Workbook book = new Workbook();

            Worksheet sheet; WorksheetCell cell; WorksheetRow Row0;

            this.GenerateStyles(book.Styles);

            sheet = book.Worksheets.Add("Sheet1");
            sheet.Table.DefaultRowHeight = 14.25F;

            sheet.Table.Columns.Add(80);
            sheet.Table.Columns.Add(200);
            sheet.Table.Columns.Add(250);
            sheet.Table.Columns.Add(150);
            sheet.Table.Columns.Add(75);
            sheet.Table.Columns.Add(150);

            Row0 = sheet.Table.Rows.Add();


            int excelcolumn = 0;

            try
            {

                //excelcolumn = excelcolumn + 5;

                //xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 15, System.Drawing.FontStyle.Bold);
                //xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                //xlWorkSheet.WriteCell(excelcolumn, 1, "Agency Referral Database");

                WorksheetCell R3Cell = Row0.Cells.Add("Agency Referral Database", DataType.String, "s94");
                R3Cell.MergeAcross = 5;


                if (CaseRefList.Count > 0)
                {

                    Row0 = sheet.Table.Rows.Add();

                    cell = Row0.Cells.Add("Code", DataType.String, "s94");
                    cell = Row0.Cells.Add("Agency Name", DataType.String, "s94");
                    cell = Row0.Cells.Add("Street", DataType.String, "s94");
                    cell = Row0.Cells.Add("City", DataType.String, "s94");
                    cell = Row0.Cells.Add("State", DataType.String, "s94");
                    cell = Row0.Cells.Add("Name Index", DataType.String, "s94");

                    //string[] Header = { "Code", "Agency Name", "Street", "City", "State", "Name Index" };
                    //for (int i = 0; i < Header.Length; ++i)
                    //{
                    //    //PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                    //    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    //cell.FixedHeight = 15f;
                    //    //cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    //    //table.AddCell(cell);

                    //    xlWorkSheet[excelcolumn, i].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //    xlWorkSheet[excelcolumn, i].Alignment = Alignment.Centered;
                    //    xlWorkSheet.WriteCell(excelcolumn, i, Header[i]);

                    //}

                    //excelcolumn = excelcolumn + 1;

                    foreach (CASEREFEntity Entity in CaseRefList)
                    {

                        Row0 = sheet.Table.Rows.Add();

                        cell = Row0.Cells.Add(Entity.Code.Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(Entity.Name1.Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(Entity.Street.Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(Entity.City.Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(Entity.State.Trim(), DataType.String, "s95");
                        cell = Row0.Cells.Add(Entity.NameIndex.Trim(), DataType.String, "s95");

                        //excelcolumn = excelcolumn + 1;

                        //xlWorkSheet.WriteCell(excelcolumn, 1, Entity.Code.Trim());
                        //xlWorkSheet.WriteCell(excelcolumn, 2, Entity.Name1.Trim());
                        //xlWorkSheet.WriteCell(excelcolumn, 3, Entity.Street.Trim());
                        //xlWorkSheet.WriteCell(excelcolumn, 4, Entity.City.Trim());
                        //xlWorkSheet.WriteCell(excelcolumn, 5, Entity.State.Trim());
                        //xlWorkSheet.WriteCell(excelcolumn, 6, Entity.NameIndex.Trim());


                    }

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = "AgencyReferral_Report.xls";

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



    }
}