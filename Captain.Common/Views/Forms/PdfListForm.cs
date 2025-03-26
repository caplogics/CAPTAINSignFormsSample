#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using System.IO;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Captain.Common.Model.Data;
using Microsoft.SqlServer.Server;
using DevExpress.Utils.Extensions;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PdfListForm : Form
    {
        string strFileName = "";
        string strLength = "";
        string strLastWritetime = "";
        string strDeleteType = "";
        string strFolderPath = string.Empty;
        DirectoryInfo MyDir;
        public PdfListForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, bool preview)
        {
            try
            {


                InitializeComponent();
                this.Text = privilegeEntity.PrivilegeName;
                BaseForm = baseForm;
                //lblHeader.Text = privilegeEntity.PrivilegeName;
                Preview = preview;
                strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";
                //fillListView(); 
                fillPreviewRpts();
                EnableButtons();

            }
            catch (Exception ex)
            {
                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
                //CommonFunctions.MessageBoxDisplay(ex.Message);
            }
        }
        public PdfListForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, bool preview, string FolderPath)
        {
            try
            {


                InitializeComponent();
                this.Text = privilegeEntity.PrivilegeName;
                BaseForm = baseForm;
                //lblHeader.Text = privilegeEntity.PrivilegeName;
                Preview = preview;
                strFolderPath = FolderPath + BaseForm.UserID + "\\";

               // fillListView(); 
                fillPreviewRpts();
                EnableButtons();
            }
            catch (StackOverflowException ex)
            { }
            catch (Exception ex)
            {
                AlertBox.Show(ex.Message,MessageBoxIcon.Error);
                //CommonFunctions.MessageBoxDisplay(ex.Message);
            }

        }
        public PdfListForm(string strUserId, bool preview, string FolderPath)
        {
            try
            {


                InitializeComponent();
                this.Text = "Report Preview";

                //lblHeader.Text = privilegeEntity.PrivilegeName;
                Preview = preview;
                strFolderPath = FolderPath + strUserId + "\\";
                //fillListView(); 
                fillPreviewRpts();
                EnableButtons();
            }
            catch (StackOverflowException ex)
            { }
            catch (Exception ex)
            {

                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }

        }
        public PdfListForm(BaseForm baseForm)
        {
            try
            {


                InitializeComponent();
                this.Text = "Excel Merge Preview";
                CaptainModel _model = new CaptainModel();
                string FolderPath = _model.lookupDataAccess.GetReportPath();
                BaseForm = baseForm;
                //lblHeader.Text = privilegeEntity.PrivilegeName;
                Preview = true;
                strFolderPath = FolderPath + BaseForm.UserID + "\\MERGEFILES\\";
                //fillListView(); 
                fillPreviewRpts();
                EnableButtons();
               if(dgvPrvRpts.Rows.Count > 0) 
                  btnMerge.Visible = true;
            }
            catch (StackOverflowException ex)
            { }
            catch (Exception ex)
            {

                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }

        }
        public PdfListForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, bool preview, string FolderPath, string TextFormat)
        {
            try
            {

                InitializeComponent();
                this.Text = privilegeEntity.PrivilegeName;
                BaseForm = baseForm;
                Format = TextFormat;
                //lblHeader.Text = privilegeEntity.PrivilegeName;
                Preview = preview;
                strFolderPath = FolderPath + BaseForm.UserID + "\\";
                //fillListView(); 
                fillPreviewRpts();
                EnableButtons();
            }
            catch (Exception ex)
            {

                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }
        }

        public BaseForm BaseForm { get; set; }

        public bool Preview { get; set; }

        public string Format { get; set; }

        private void OnHelpClick(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Application.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Captain");
        }

        List<DataGridViewRow> selRowCount;
        private void btnDelete_Click(object sender, EventArgs e)

        {
            /***********************************************************************************************
             * LISTVIEW
            /***********************************************************************************************/
            //string strDeleteFile = "";
            //// var selectedItems = listViewPdf.SelectedItems;
            //var selectedItems = listViewPdf.CheckedItems;

            //if (selectedItems.Count > 0)
            //{
            //    bool boolmark = false;
            //    foreach (ListViewItem item in listViewPdf.Items)
            //    {
            //        if (item.Checked)
            //        {
            //            //if (item.SubItems[3].Text == "*")
            //            //{
            //            boolmark = true;
            //            strDeleteType = "Mark";
            //            break;

            //            // }
            //        }
            //    }

            //    strDeleteFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
            //    if (boolmark == true)
            //        MessageBox.Show("Are you sure you want to delete selected file(s)?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
            //    else
            //        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nFile " + strDeleteFile, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);

            //}
            //else
            //{
            //    AlertBox.Show("Please check-off atleast one file to delete", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
            //}
            /***********************************************************************************************/

            selRowCount = (from c in dgvPrvRpts.Rows.Cast<DataGridViewRow>().ToList()
                           where (((DataGridViewCheckBoxCell)c.Cells["chkSel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                           select c).ToList();
            if (selRowCount.Count > 0)
            {
                bool boolmark = false;
                strDeleteType = "Mark";
                if (selRowCount.Count > 1)
                    boolmark = true;

                if (boolmark == true)
                    MessageBox.Show("Are you sure you want to delete selected file(s)?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                else
                {
                    string strDeleteFile = "";
                    strDeleteFile = selRowCount[0].Cells[1].Value.ToString();
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nFile " + strDeleteFile, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                }
            }
            else
            {
                AlertBox.Show("Please check-off at least One File to Delete", MessageBoxIcon.Warning, null, ContentAlignment.BottomRight);
            }
        }
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            strDeleteType = "ALL";
            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "All Files", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            #region ListView
            //string strPreviewFile = "";
            //var selectedItems = listViewPdf.SelectedItems;
            //try
            //{
            //    if (selectedItems.Count > 0)
            //    {
            //        if (listViewPdf.Items[listViewPdf.SelectedIndex].Selected)
            //            strPreviewFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text == null ? string.Empty : listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();

            //        if (!string.IsNullOrEmpty(strPreviewFile))
            //        {
            //            int Count = strPreviewFile.Length;
            //            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            //            {
            //                if (strPreviewFile.Substring(Count - 4, 4) == ".pdf")
            //                {
            //                    PdfViewerNewForm objfrm = new PdfViewerNewForm(strFolderPath + "\\" + strPreviewFile);
            //                    objfrm.StartPosition = FormStartPosition.CenterScreen;

            //                    objfrm.ShowDialog();

            //                }
            //                else if (strPreviewFile.Substring(Count - 4, 4) == ".xls")
            //                {
            //                    CommonFunctions.MessageBoxDisplay("You can't preview xls, please download it.");
            //                }
            //                else if (strPreviewFile.Substring(Count - 4, 4) == ".txt")
            //                {
            //                    PdfViewerNewForm objfrm = new PdfViewerNewForm(strFolderPath + "\\" + strPreviewFile);
            //                    objfrm.StartPosition = FormStartPosition.CenterScreen;

            //                    objfrm.ShowDialog();
            //                }
            //            }
            //            else
            //            {

            //                if (strPreviewFile.Substring(Count - 4, 4) == ".pdf")
            //                {
            //                    FrmViewer objfrm = new FrmViewer(strFolderPath + "\\" + strPreviewFile);
            //                    objfrm.StartPosition = FormStartPosition.CenterScreen;

            //                    objfrm.ShowDialog();
            //                }
            //                else if (strPreviewFile.Substring(Count - 4, 4) == ".xls")
            //                {
            //                    //FrmViewer objfrm = new FrmViewer(strFolderPath + "\\" + strPreviewFile);
            //                    //objfrm.StartPosition = FormStartPosition.CenterScreen;

            //                    //objfrm.ShowDialog();
            //                    //OpenExcelFile(strPreviewFile);
            //                    CommonFunctions.MessageBoxDisplay("You can't preview xls, please download it.");
            //                }
            //                else if (strPreviewFile.Substring(Count - 4, 4) == ".txt")
            //                {
            //                    FrmViewer objfrm = new FrmViewer(strFolderPath + "\\" + strPreviewFile);
            //                    objfrm.StartPosition = FormStartPosition.CenterScreen;

            //                    objfrm.ShowDialog();
            //                    //Process.Start("notepad.exe", strFolderPath + strPreviewFile);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{


            //}
            #endregion

            string strPreviewFile = "";
            var selectedItems = dgvPrvRpts.CurrentRow.Selected;
            try
            {
                if (selectedItems)
                {
                    //if (listViewPdf.Items[listViewPdf.SelectedIndex].Selected)
                    //    strPreviewFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text == null ? string.Empty : listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();

                    strPreviewFile = dgvPrvRpts.CurrentRow.Cells[1].Value == null ? string.Empty : dgvPrvRpts.CurrentRow.Cells[1].Value.ToString();

                    if (!string.IsNullOrEmpty(strPreviewFile))
                    {
                        int Count = strPreviewFile.Length;
                        if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                        {
                            if (strPreviewFile.Substring(Count - 4, 4) == ".pdf")
                            {
                                PdfViewerNewForm objfrm = new PdfViewerNewForm(strFolderPath + "\\" + strPreviewFile);
                                objfrm.StartPosition = FormStartPosition.CenterScreen;

                                objfrm.ShowDialog();

                            }
                            else if (strPreviewFile.Substring(Count - 4, 4) == ".xls")
                            {
                                AlertBox.Show("You can't Preview '.xls' extension reports, please download it.", MessageBoxIcon.Warning);
                            }
                            else if (strPreviewFile.Substring(Count - 5, 5) == ".xlsx")
                            {
                                AlertBox.Show("You can't Preview '.xlsx' extension reports, please download it.", MessageBoxIcon.Warning);
                            }
                            else if (strPreviewFile.Substring(Count - 4, 4) == ".txt")
                            {
                                /*PdfViewerNewForm objfrm = new PdfViewerNewForm(strFolderPath + "\\" + strPreviewFile);
                                objfrm.StartPosition = FormStartPosition.CenterScreen;

                                objfrm.ShowDialog();*/
                                AlertBox.Show("You can't Preview '.txt' extension reports, please download it.", MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {

                            if (strPreviewFile.Substring(Count - 4, 4) == ".pdf")
                            {
                                FrmViewer objfrm = new FrmViewer(strFolderPath + "\\" + strPreviewFile);
                                objfrm.StartPosition = FormStartPosition.CenterScreen;

                                objfrm.ShowDialog();
                            }
                            else if (strPreviewFile.Substring(Count - 4, 4) == ".xls")
                            {
                                //FrmViewer objfrm = new FrmViewer(strFolderPath + "\\" + strPreviewFile);
                                //objfrm.StartPosition = FormStartPosition.CenterScreen;

                                //objfrm.ShowDialog();
                                //OpenExcelFile(strPreviewFile);
                                AlertBox.Show("You can't Preview '.xls' extension reports, please download it.", MessageBoxIcon.Warning);
                            }
                            else if (strPreviewFile.Substring(Count - 5, 5) == ".xlsx")
                            {
                                AlertBox.Show("You can't Preview '.xlsx' extension reports, please download it.", MessageBoxIcon.Warning);
                            }
                            else if (strPreviewFile.Substring(Count - 4, 4) == ".txt")
                            {
                                /*FrmViewer objfrm = new FrmViewer(strFolderPath + "\\" + strPreviewFile);
                                objfrm.StartPosition = FormStartPosition.CenterScreen;

                                objfrm.ShowDialog();*/
                                AlertBox.Show("You can't Preview '.txt' extension reports, please download it.", MessageBoxIcon.Warning);
                                //Process.Start("notepad.exe", strFolderPath + strPreviewFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }


        }
        //private Excel.Application ExcelObj = null;
        //private void OpenExcelFile(string FileName)
        //{
        //    ExcelObj = new Excel.Application();
        //    ExcelObj.Visible = true;
        //    Excel.Workbook theWorkbook = ExcelObj.Workbooks.Open(strFolderPath + "\\" + FileName, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true);
        //    // get the collection of sheets in the workbook
        //    Excel.Sheets sheets = theWorkbook.Worksheets;
        //    // get the first and only worksheet from the collection of worksheets
        //    Excel.Worksheet worksheet = (Excel.Worksheet)sheets.get_Item(1);
        //}

        //kranthi::11/24/2022
        //private void fillListView()
        //{
        //    MyDir = new DirectoryInfo(strFolderPath);
        //    if (MyDir.Exists == true)
        //    {
        //        if (Preview)
        //            Format = "*";

        //        FileInfo[] MyFiles = MyDir.GetFiles("*." + Format.ToLower());
        //        this.listViewPdf.SelectedIndexChanged -= new System.EventHandler(this.listViewPdf_SelectedIndexChanged);
        //        listViewPdf.Items.Clear();
        //        this.listViewPdf.SelectedIndexChanged += new System.EventHandler(this.listViewPdf_SelectedIndexChanged);
        //        foreach (FileInfo MyFile in MyFiles)
        //        {
        //            strFileName = MyFile.Name;
        //            strLength = (MyFile.Length / 1024).ToString();
        //            strLastWritetime = MyFile.LastWriteTime.ToShortDateString() + " " + MyFile.LastWriteTime.ToShortTimeString();
        //            listViewPdf.Items.Add(new ListViewItem(new string[] { strFileName, strLength + "  KB", strLastWritetime, string.Empty }));

        //        }

        //        //if (listViewPdf.Items.Count > 0)
        //        //{
        //        //    for (int i = 0; i < listViewPdf.Items.Count; i++)
        //        //    {
        //        //        if (listViewPdf.Items[i].Index % 2 == 0)
        //        //            listViewPdf.Items[i].BackColor = Color.White;
        //        //        else
        //        //            listViewPdf.Items[i].BackColor = Color.LightCyan;
        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        MyDir.Create();
        //    }
        //}


        private void fillPreviewRpts()
        {
            MyDir = new DirectoryInfo(strFolderPath);
            if (MyDir.Exists == true)
            {
                if (Preview)
                    Format = "*";

                //FileInfo[] MyFiles = MyDir.GetFiles("*." + Format.ToLower());
                var MyFiles = MyDir.GetFiles("*." + Format.ToLower()).OrderByDescending(f => f.LastWriteTime).ToList();
                this.dgvPrvRpts.SelectionChanged -= new System.EventHandler(this.dgvPrvRpts_SelectionChanged);
                dgvPrvRpts.Rows.Clear();
              

                foreach (FileInfo MyFile in MyFiles)
                {
                    strFileName = MyFile.Name;
                    string strextension = MyFile.Name.Split('.').Last();
                    strLength = (MyFile.Length / 1024).ToString();
                    strLastWritetime = MyFile.LastWriteTime.ToShortDateString() + " " + MyFile.LastWriteTime.ToShortTimeString();

                   //** dgvPrvRpts.Rows.Add(false, strFileName, strextension, strLength + "  KB", Convert.ToDateTime(strLastWritetime).ToString("MM/dd/yyyy hh:mm tt"), string.Empty);
                    dgvPrvRpts.Rows.Add(false, strFileName, strextension, strLength, "KB", Convert.ToDateTime(strLastWritetime).ToString("MM/dd/yyyy hh:mm tt"), string.Empty);
                }
            }
            else
            {
                MyDir.Create();
            }
            if (dgvPrvRpts.Rows.Count > 0)
            {
                btnMerge.Visible = true;
                dgvPrvRpts.CurrentCell = dgvPrvRpts.Rows[0].Cells[1];
                dgvPrvRpts.Rows[0].Selected = true;
            }
            else
                btnMerge.Visible = false;

            this.dgvPrvRpts.SelectionChanged += new System.EventHandler(this.dgvPrvRpts_SelectionChanged);
        }

        private void MessageBoxHandler(DialogResult dialogresult)
        {
            #region LISTVIEW
            /***************************************************************************
             * * LISTVIEW * *
             * ************************************************************************/
            //string strDeleteFile = "";
            //if (dialogresult == DialogResult.Yes)
            //{
            //    if (strDeleteType == "ALL")
            //    {
            //        MyDir = new DirectoryInfo(strFolderPath);
            //        FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            //        foreach (FileInfo MyFile in MyFiles)
            //        {
            //            if (MyFile.Exists)
            //            {
            //                MyFile.Delete();
            //            }
            //        }
            //        fillListView(); fillPreviewRpts();
            //        if (listViewPdf.Items.Count > 0)
            //        {
            //            listViewPdf.Items[0].Selected = true;
            //            btnDownload.Visible = true;
            //            btnPreview.Visible = true;
            //            btnDelete.Visible = true;
            //        }
            //        else
            //        {
            //            btnDownload.Visible = false;
            //            btnPreview.Visible = false;
            //            btnDelete.Visible = false;
            //        }
            //        AlertBox.Show("The selected file(s) is Successfullty Deleted", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
            //    }
            //    else if (strDeleteType == "Mark")
            //    {
            //        foreach (ListViewItem item in listViewPdf.Items)
            //        {
            //            //if (item.SubItems[3].Text == "*")
            //            //{
            //            if (item.Checked)
            //            {
            //                strDeleteFile = item.SubItems[0].Text.ToString();
            //                MyDir = new DirectoryInfo(strFolderPath);
            //                FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            //                foreach (FileInfo MyFile in MyFiles)
            //                {
            //                    if (MyFile.Exists)
            //                    {
            //                        if (strDeleteFile == MyFile.Name)
            //                        {
            //                            MyFile.Delete();
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        fillListView();
            //        fillPreviewRpts();
            //        if (listViewPdf.Items.Count > 0)
            //        {
            //            listViewPdf.Items[0].Selected = true;
            //            btnDownload.Visible = true;
            //            btnPreview.Visible = true;
            //            btnDelete.Visible = true;
            //        }
            //        else
            //        {
            //            btnDownload.Visible = false;
            //            btnPreview.Visible = false;
            //            btnDelete.Visible = false;
            //        }
            //        AlertBox.Show("The selected file(s) is Successfullty Deleted", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
            //    }
            //    else if (strDeleteType == "Download")
            //    {
            //        var selectedItems = listViewPdf.SelectedItems;
            //        if (selectedItems.Count > 0)
            //        {
            //            strDeleteFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();

            //            //FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //            //downloadGateway.Filename = strDeleteFile;

            //            //// downloadGateway.Version = file.Version;

            //            //downloadGateway.SetContentType(DownloadContentType.OctetStream);

            //            //downloadGateway.StartFileDownload(new ContainerControl(), strFolderPath + "\\" + strDeleteFile);

            //            try
            //            {
            //                string localFilePath = strFolderPath + "\\" + strDeleteFile;
            //                /// Need to check for null value of localFilePath, etc...
            //                FileInfo fiDownload = new FileInfo(localFilePath);
            //                /// Need to check for file exists, is local file, is allow to read, etc...
            //                string name = fiDownload.Name;
            //                using (FileStream fileStream = fiDownload.OpenRead())
            //                {
            //                    Application.Download(fileStream, name);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //            }

            //        }
            //        AlertBox.Show("Selected File(s) is downloaded Successfully");
            //    }
            //    else
            //    {
            //        var selectedItems = listViewPdf.SelectedItems;
            //        if (selectedItems.Count > 0)
            //        {
            //            strDeleteFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
            //        }
            //        MyDir = new DirectoryInfo(strFolderPath);
            //        FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            //        foreach (FileInfo MyFile in MyFiles)
            //        {
            //            if (MyFile.Exists)
            //            {
            //                if (strDeleteFile == MyFile.Name)
            //                {
            //                    MyFile.Delete();
            //                }
            //            }
            //        }

            //        fillListView(); fillPreviewRpts();
            //        if (listViewPdf.Items.Count > 0)
            //        {
            //            listViewPdf.Items[0].Selected = true;
            //            btnDownload.Visible = true;
            //            btnPreview.Visible = true;
            //            btnDelete.Visible = true;
            //        }
            //        else
            //        {
            //            btnDownload.Visible = false;
            //            btnPreview.Visible = false;
            //            btnDelete.Visible = false;
            //        }

            //    }
            //}

            #endregion
            string strDeleteFile = "";
            if (dialogresult == DialogResult.Yes)
            {
                #region Delete ALL Files
                if (strDeleteType == "ALL")
                {
                    MyDir = new DirectoryInfo(strFolderPath);
                    FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                    foreach (FileInfo MyFile in MyFiles)
                    {
                        if (MyFile.Exists)
                        {
                            MyFile.Delete();
                        }
                    }
                    //fillListView(); 
                    fillPreviewRpts();
                    if (dgvPrvRpts.Rows.Count > 0)
                    {
                        dgvPrvRpts.Rows[0].Selected = true;
                        btnDownload.Visible = true;
                        btnPreview.Visible = true;
                        btnDelete.Visible = true;
                    }
                    else
                    {
                        btnDownload.Visible = false;
                        btnPreview.Visible = false;
                        btnDelete.Visible = false;
                    }
                    AlertBox.Show("The selected file(s) is Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                }
                #endregion

                #region Delete Checked Files
                else if (strDeleteType == "Mark")
                {
                    if (selRowCount.Count > 0)
                    {

                        foreach (DataGridViewRow dvRow in selRowCount)
                        {

                            strDeleteFile = dvRow.Cells[1].Value.ToString();
                            MyDir = new DirectoryInfo(strFolderPath);
                            FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                            foreach (FileInfo MyFile in MyFiles)
                            {
                                if (MyFile.Exists)
                                {
                                    if (strDeleteFile == MyFile.Name)
                                    {
                                        MyFile.Delete();
                                    }
                                }
                            }
                        }
                    }


                    // fillListView();
                    fillPreviewRpts();
                    if (dgvPrvRpts.Rows.Count > 0)
                    {
                        dgvPrvRpts.Rows[0].Selected = true;
                        btnDownload.Visible = true;
                        btnPreview.Visible = true; btnMerge.Visible = false;
                        btnDelete.Visible = true;
                    }
                    else
                    {
                        btnDownload.Visible = false;
                        btnPreview.Visible = false;
                        btnDelete.Visible = false;
                    }
                    AlertBox.Show("The selected file(s) is Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                }
                #endregion

                #region Download Files
                else if (strDeleteType == "Download")
                {


                    var selectedItems = dgvPrvRpts.CurrentRow.Selected;     //listViewPdf.SelectedItems;
                    if (selectedItems)
                    {
                        strDeleteFile = dgvPrvRpts.CurrentRow.Cells[1].Value.ToString();      //listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();

                        try
                        {
                            string localFilePath = strFolderPath + "\\" + strDeleteFile;
                            /// Need to check for null value of localFilePath, etc...
                            FileInfo fiDownload = new FileInfo(localFilePath);
                            /// Need to check for file exists, is local file, is allow to read, etc...
                            string name = fiDownload.Name;
                            using (FileStream fileStream = fiDownload.OpenRead())
                            {
                                Application.Download(fileStream, name);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        AlertBox.Show("File Downloaded Successfully");
                    }
                    else
                        AlertBox.Show("Please select a file to Download", MessageBoxIcon.Warning);

                }
                #endregion

                #region Delete Selected single File
                else
                {
                    var selectedItems = dgvPrvRpts.CurrentRow.Selected; //listViewPdf.SelectedItems;
                    if (selectedItems)
                    {
                        strDeleteFile = dgvPrvRpts.CurrentRow.Cells[1].Value.ToString(); // listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
                    }
                    MyDir = new DirectoryInfo(strFolderPath);
                    FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                    foreach (FileInfo MyFile in MyFiles)
                    {
                        if (MyFile.Exists)
                        {
                            if (strDeleteFile == MyFile.Name)
                            {
                                MyFile.Delete();
                            }
                        }
                    }

                    //fillListView(); 
                    fillPreviewRpts();
                    if (dgvPrvRpts.Rows.Count > 0)
                    {
                        dgvPrvRpts.Rows[0].Selected = true;
                        btnDownload.Visible = true;
                        btnPreview.Visible = true;
                        btnDelete.Visible = true;
                    }
                    else
                    {
                        btnDownload.Visible = false;
                        btnPreview.Visible = false;
                        btnDelete.Visible = false;
                    }

                }
                #endregion
            }
        }

        private void EnableButtons()
        {
            if (!Preview)
            {
                pnlbuttons.Visible = false;
                btnDelete.Visible = false;
                btnDeleteAll.Visible = false;
                btnPreview.Visible = false;
                btnMerge.Visible = false; spacer1.Visible = false;
                btnDownload.Visible = false;

                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - pnlbuttons.Height);
                //this.SavePanel.Location = new System.Drawing.Point(1, 269);
                //this.SavePanel.Size = new System.Drawing.Size(537, 56);
                SavePanel.Visible = true;

                if (Format == "Text")
                {
                    CbmFileType.SelectedIndex = 1;
                    Format = "TXT";
                }
                else if (Format == "XLS" || Format == "XLSX")
                    CbmFileType.SelectedIndex = 2;
                else
                    CbmFileType.SelectedIndex = 0;

                // this.Font = new System.Drawing.Font("default", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Text = "Save As";//lblHeader.Text = "Save As";
                //PdfName.Text = "&nbsp;&nbsp;Name";
                //kranthi::11/24/2022// listViewPdf.CheckBoxes = false;

                dgvPrvRpts.Columns["chkSel"].Visible = false;
                dgvPrvRpts.Columns["FileExtension"].Visible = false;
                dgvPrvRpts.Columns["FileName"].Width = 250/*265*/ + 25 + 60;
            }
            else
            {
                btnMerge.Visible = false; spacer1.Visible = false;
                //btnDownload.Location = new System.Drawing.Point(480, 5);
                this.Size = new System.Drawing.Size(this.Size.Width,395 /*this.Size.Height - SavePanel.Height*/);
                //kranthi::11/24/2022// listViewPdf.CheckBoxes = true;

                dgvPrvRpts.Columns["chkSel"].Visible = false;
                dgvPrvRpts.Columns["FileExtension"].Visible = true;

                //PdfName.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name";

                //kranthi::11/24/2022// 
                //if (listViewPdf.Items.Count > 0)
                //{
                //    listViewPdf.Items[0].Selected = true;
                //    btnDownload.Visible = true;
                //    btnPreview.Visible = true;
                //    btnDelete.Visible = true;
                //}
                //else
                //{
                //    btnDownload.Visible = false;
                //    btnPreview.Visible = false;
                //    btnDelete.Visible = false;
                //}

                if (dgvPrvRpts.Rows.Count > 0)
                {
                    dgvPrvRpts.Rows[0].Selected = true;
                    btnDownload.Visible = true;
                    btnPreview.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    btnDownload.Visible = false;
                    btnPreview.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }

        public string GetFileName()
        {
            string SelAppKey = null;

            if (!(string.IsNullOrEmpty(TxtFileName.Text)))
                SelAppKey = TxtFileName.Text;

            return SelAppKey;
        }

        //kranthi::11/24/2022// 
        private void listViewPdf_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Format == "TXT")
            //{
            //    string[] Name = Regex.Split(listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString(), ".txt");
            //    TxtFileName.Text = Name[0];
            //}
            //else if (Format == "XLS")
            //{
            //    string[] Name = Regex.Split(listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString(), ".xls");
            //    TxtFileName.Text = Name[0];
            //}
            //else
            //{
            //    string[] Name = Regex.Split(listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString(), ".pdf");
            //    TxtFileName.Text = Name[0];
            //}

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(TxtFileName.Text.Trim())))
                AlertBox.Show("Please give a File Name", MessageBoxIcon.Warning);
            else
            {
                Regex r = new Regex(@"[~`!@#$%^&*()-+=|\{}':;.,<>/?]");
                if (r.IsMatch(TxtFileName.Text))
                {

                    AlertBox.Show(@"A file name can not contain any of the following characters: ~`!@#$%^&*()-+=|\{}':;.,<>/?[]""", MessageBoxIcon.Warning);

                }
                else
                {
                    if ((TxtFileName.Text.Contains(@"\")) || (TxtFileName.Text.Contains("/")) || (TxtFileName.Text.Contains('"')) || (TxtFileName.Text.Contains("<")) || (TxtFileName.Text.Contains(">")))
                    {
                        AlertBox.Show(@"A file name can not contain any of the following characters: ~`!@#$%^&*()-+=|\{}':;.,<>/?[]""", MessageBoxIcon.Warning);
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }


        private void listViewPdf_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            //kranthi::11/24/2022

            //if (objArgs.MenuItem.Tag == "*")
            //{
            //    listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[3].Text = "*";
            //    listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[3].ForeColor = Color.Red;
            //}
            //else if (objArgs.MenuItem.Tag == "MA")
            //{
            //    foreach (ListViewItem item in listViewPdf.Items)
            //    {
            //        item.SubItems[3].Text = "*";
            //        item.SubItems[3].ForeColor = Color.Red;

            //    }
            //}
            //else if (objArgs.MenuItem.Tag == "UA")
            //{
            //    foreach (ListViewItem item in listViewPdf.Items)
            //    {
            //        item.SubItems[3].Text = string.Empty;
            //    }
            //}
            //else
            //{
            //    listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[3].Text = string.Empty;
            //}

        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            //if (Preview)
            //{
            //    if (listViewPdf.Items.Count > 0)
            //    {
            //        contextMenu1.MenuItems.Clear();
            //        if (listViewPdf.SelectedItems.Count > 0)
            //        {
            //            if (listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[3].Text.ToString() == "*")
            //            {
            //                MenuItem menuItem = new MenuItem();
            //                menuItem.Text = "Unmark";
            //                menuItem.Tag = "";
            //                contextMenu1.MenuItems.Add(menuItem);
            //                menuItem = new MenuItem();
            //                menuItem.Text = "Mark All";
            //                menuItem.Tag = "MA";
            //                contextMenu1.MenuItems.Add(menuItem);
            //                menuItem = new MenuItem();
            //                menuItem.Text = "Unmark All";
            //                menuItem.Tag = "UA";
            //                contextMenu1.MenuItems.Add(menuItem);
            //            }
            //            else
            //            {
            //                MenuItem menuItem = new MenuItem();
            //                menuItem.Text = "Mark";
            //                menuItem.Tag = "*";
            //                contextMenu1.MenuItems.Add(menuItem);
            //                menuItem = new MenuItem();
            //                menuItem.Text = "Mark All";
            //                menuItem.Tag = "MA";
            //                contextMenu1.MenuItems.Add(menuItem);
            //                menuItem = new MenuItem();
            //                menuItem.Text = "Unmark All";
            //                menuItem.Tag = "UA";
            //                contextMenu1.MenuItems.Add(menuItem);
            //            }

            //        }
            //    }
            //}

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            //Kranthi::11/24/2022//

            //string strPreviewFile = "";
            //var selectedItems = listViewPdf.SelectedItems;
            //if (selectedItems.Count > 0)
            //{
            //    strDeleteType = "Download";
            //    strPreviewFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
            //    MessageBox.Show("Are you sure you want to download this file " + strPreviewFile, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);

            //}

            string strPreviewFile = "";
            var selectedItems = dgvPrvRpts.CurrentRow.Selected;
            if (selectedItems)
            {
                strDeleteType = "Download";
                strPreviewFile = dgvPrvRpts.CurrentRow.Cells[1].Value.ToString(); //listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
                MessageBox.Show("Are you sure you want to download this file?" + strPreviewFile, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);

            }


            ////  var selectedItems = listViewPdf.SelectedItems;
            //  if (selectedItems.Count > 0)
            //  {
            //      strPreviewFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
            //      FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //      downloadGateway.Filename = strPreviewFile;

            //      // downloadGateway.Version = file.Version;

            //      downloadGateway.SetContentType(DownloadContentType.OctetStream);

            //      downloadGateway.StartFileDownload(new ContainerControl(),  strFolderPath + "\\" + strPreviewFile);
            //  }
        }


        public string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public string FriendlyName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            //Kranthi::11/24/2022

            //intsourceName = 0;
            //List<ListItem> SourceFilePaths = new List<ListItem>();
            //bool boolmsg = false;
            //if (listViewPdf.SelectedItems.Count >= 1)
            //{
            //    int intseq = 0;
            //    //string name = listViewPdf.SelectedItems[0].Name.ToString();
            //    foreach (ListViewItem item in listViewPdf.Items)
            //    {
            //        if (item.Selected == true)
            //        {
            //            if (item.Text.Contains(".xls"))
            //            {
            //                intseq = intseq + 1;
            //                boolmsg = true;
            //                SourceFilePaths.Add(new ListItem(item.Text.ToString(), intseq.ToString(), string.Empty, strFolderPath + item.Text.ToString()));//.Add(strFolderPath + item.Text.ToString());
            //            }
            //        }
            //    }
            //}
            //DataGridView dataGrid = new DataGridView();
            intsourceName = 0;
            List<ListItem> SourceFilePaths = new List<ListItem>();
            bool boolmsg = false;
            selRowCount = (from c in dgvPrvRpts.Rows.Cast<DataGridViewRow>().ToList()
                           where (((DataGridViewCheckBoxCell)c.Cells["chkSel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                           select c).ToList();
            if (selRowCount.Count >= 1)
            {
                int intseq = 0;
                //string name = listViewPdf.SelectedItems[0].Name.ToString();
                foreach (DataGridViewRow dvRow in selRowCount)
                {

                    if (dvRow.Cells[1].Value.ToString().Contains(".xls"))
                    {
                        intseq = intseq + 1;
                        boolmsg = true;
                        SourceFilePaths.Add(new ListItem(dvRow.Cells[1].Value.ToString(), intseq.ToString(), string.Empty, strFolderPath + dvRow.Cells[1].Value.ToString()));
                        //dataGrid.Rows.Add(new ListItem(dvRow.Cells[0].Value.ToString(), intseq.ToString(), string.Empty, strFolderPath + dvRow.Cells[0].Value.ToString()));
                    }

                }
            }


            //MergeExcelForm mergeForm = new MergeExcelForm(SourceFilePaths);
            //mergeForm.FormClosed += new FormClosedEventHandler(mergeForm_FormClosed);
            //mergeForm.StartPosition = FormStartPosition.CenterScreen;
            //mergeForm.ShowDialog();

        }

        void mergeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
//            MergeExcelForm form = sender as MergeExcelForm;
//            if (form.DialogResult == DialogResult.OK)
//            {
//                List<ListItem> listDetails = form.listNames;
//                if (listDetails.Count > 0)
//                {
//                    listDetails = listDetails.OrderBy(u => Convert.ToInt32(u.Value)).ToList();
//                    DoMerge(listDetails, true);
///*                    fillPreviewRpts(); */ fillListView();
//                }
//            }
        }
        private void fillListView()
        {
            MyDir = new DirectoryInfo(strFolderPath);
            if (MyDir.Exists == true)
            {
                FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                this.dgvPrvRpts.SelectionChanged -= new System.EventHandler(this.dgvPrvRpts_SelectionChanged);
                dgvPrvRpts.Rows.Clear();//**dgvPrvRpts.Items.Clear();
                this.dgvPrvRpts.SelectionChanged += new System.EventHandler(this.dgvPrvRpts_SelectionChanged);
                foreach (FileInfo MyFile in MyFiles)
                {
                    strFileName = MyFile.Name;
                    strLength = (MyFile.Length / 1024).ToString();
                    strLastWritetime = MyFile.LastWriteTime.ToShortDateString() + " " + MyFile.LastWriteTime.ToShortTimeString();
                   //** dgvPrvRpts.Rows.Add(new ListViewItem(new string[] { strFileName, strLength + "  KB", strLastWritetime, string.Empty }));
                    dgvPrvRpts.Rows.Add(new ListViewItem(new string[] { strFileName, strLength, "KB", strLastWritetime, string.Empty }));
                }
            }
            else
            {
                MyDir.Create();
            }
        }


        int intsourceName = 0;
        void DoMerge(List<ListItem> _sourceFiles, bool boolmsg)
        {
            bool b = false;
            int i = 0;

            foreach (ListItem strFile in _sourceFiles)
            {
                i = i + 1;
                intsourceName = intsourceName + 1;
                NPOICOPY(strFile.ValueDisplayCode, i, intsourceName);
            }
            if (boolmsg)
                AlertBox.Show("Merge Successful, File Name is : MergeExcelSheets.xls");
        }
        HSSFWorkbook product = new HSSFWorkbook();
        void NPOICOPY(string filename, int X, int intsourceName)
        {

            byte[] byteArray = File.ReadAllBytes(filename);

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, (int)byteArray.Length);

                HSSFWorkbook book1 = new HSSFWorkbook(stream);
                if (X == 1)
                {
                    product = new HSSFWorkbook();
                }
                for (int i = 0; i < book1.NumberOfSheets; i++)
                {
                    HSSFSheet sheet1 = book1.GetSheetAt(i) as HSSFSheet;
                    sheet1.CopyTo(product, sheet1.SheetName + intsourceName.ToString(), true, true);
                }
                string strName = "\\MergeExcelSheets.xls";
                using (FileStream fs = new FileStream((strFolderPath + strName), FileMode.Create, FileAccess.Write))
                {
                    product.Write(fs);

                }

            }


        }

        private void dgvPrvRpts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrvRpts.SelectedRows.Count > 0)
            {
                TxtFileName.Text = dgvPrvRpts.SelectedRows[0].Cells["FileName"].Value.ToString().Split('.')[0].ToString();

                if (Format == "TXT")
                {
                    string[] Name = Regex.Split(dgvPrvRpts.SelectedRows[0]["FileName"].Value.ToString(), ".txt");
                    TxtFileName.Text = Name[0];
                }
                else if (Format == "XLS")
                {
                    string[] Name = Regex.Split(dgvPrvRpts.SelectedRows[0]["FileName"].Value.ToString(), ".xls");
                    TxtFileName.Text = Name[0];
                }
                else if (Format == "XLSX")
                {
                    string[] Name = Regex.Split(dgvPrvRpts.SelectedRows[0]["FileName"].Value.ToString(), ".xlsx");
                    TxtFileName.Text = Name[0];
                }
                else
                {
                    string[] Name = Regex.Split(dgvPrvRpts.SelectedRows[0]["FileName"].Value.ToString(), ".pdf");
                    TxtFileName.Text = Name[0];
                }
            }
        }

        private void dgvPrvRpts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnShowDelOptions.Visible = true;
            pnlDelOptions.Visible = false;
            pnlDownldOptions.Visible = true;
            dgvPrvRpts.Columns["chkSel"].Visible = false;
        }

        private void btnShowDelOptions_Click(object sender, EventArgs e)
        {
            btnShowDelOptions.Visible = false;
            pnlDelOptions.Visible = true;
            pnlDownldOptions.Visible = false;
            dgvPrvRpts.Columns["chkSel"].Visible = true;
            dgvPrvRpts.Rows.ForEach(x => x["chksel"].Value = false);
        }
    }


}