#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using System.IO;
using Captain.Common.Views.UserControls;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Configuration;
//using System.Windows.Forms;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PIP00001Form : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        private string strTempFolderName = string.Empty;
        private string strImageFolderName = string.Empty;
        private string strFolderName = string.Empty;
        private string strSubFolderName = string.Empty;
        private string strFullFolderName = string.Empty;
        private string strMainFolderName = string.Empty;
        private string strExtensionName = string.Empty;
        private string strDeleteEnable = string.Empty;
        private string strCheckUploadMode = string.Empty;
        private OpenFileDialog objUpload;
        List<CaseMstEntity> caseMstEntityList;
        List<CaseSnpEntity> caseSnpEntityList;
        DirectoryInfo MyDir;

        public PIP00001Form(BaseForm baseForm, PrivilegeEntity privilieges, string strAgency, string strDept, string strProg, string strYear, string strApp, string strConf, string strRegid)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            strFormCloseRefresh = string.Empty;
            BaseForm = baseForm;
            Privileges = privilieges;
            this.Text = /*Privileges.Program + " - */"Document Verification and Sending Emails";
            strCheckUploadMode = string.Empty;
            _propConformationNum = strConf;
            _propPIP_REG_ID = strRegid;
            _propAgency = strAgency;
            _propDept = strDept;
            _propProg = strProg;
            _propYear = strYear;
            _propApplNo = strApp;

            //btnDragged.Visible = false;
            propimagetypesCategory = _model.lookupDataAccess.GetImageNameConvention();

            //    string strImages = "CM ";
            if (!string.IsNullOrEmpty(BaseForm.UserProfile.ImageTypes.Trim()))
            {
                string[] strImageTypes = BaseForm.UserProfile.ImageTypes.Trim().Split(' ');//strImages.Split(' '); //
                bool boolAddExclude = false;
                foreach (string incomeType in strImageTypes)
                {
                    List<CommonEntity> ImagesNamesTypes = propimagetypesCategory.FindAll(u => u.Extension.Trim().ToUpper() == incomeType.Trim().ToUpper());
                    foreach (CommonEntity item in ImagesNamesTypes)
                    {
                        _propImagesTypesOnly.Add(new CommonEntity(item.Code, item.Desc, item.Hierarchy, item.Extension, string.Empty, string.Empty));
                    }
                }
            }

            if (privilieges != null)
            {

                if (privilieges.AddPriv.Equals("false"))
                {
                    btnAdd.Visible = false;
                }
                if (privilieges.ChangePriv.Equals("false"))
                {
                    gviVedit.Visible = false;
                }
                if (privilieges.DelPriv.Equals("false"))
                {
                    gvIDel.Visible = false;
                }

            }

            //if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            //{
            //    cmbEmailAgency.Visible = true;
            //    lblAgency.Visible = true;
            //    DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once

            //    if (ds.Tables.Count > 0)
            //    {
            //        foreach (DataRow drhiechy in ds.Tables[0].Rows)
            //        {
            //            cmbEmailAgency.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
            //        }
            //    }
            //    //if (userHierarchy.Count > 0)
            //    //    cmbEmailAgency.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
            //    cmbEmailAgency.SelectedIndex = 0;
            //    CommonFunctions.SetComboBoxValue(cmbEmailAgency, BaseForm.BaseAgency);
            //}

            if (Consts.Common.ServerLocation == string.Empty)
            {
                _propPIPIMGDOCPATH = @"D:\CABA-REPORTS\ScannedImages\01010400000003\0000001\";// @"\\CAPSYSAZ8\PIP_DOCS\" + BaseForm.BaseAgencyControlDetails.AgyShortName + @"00\DOCUPLDS\";

            }
            else
            {
                if (Consts.Common.ServerLocation == @"\\SYSTEM-1\" || Consts.Common.ServerLocation == @"\\CLSYS-1\" || Consts.Common.ServerLocation == @"\\CLSYS-2\" || Consts.Common.ServerLocation == @"\\CLSYS-5\")
                    _propPIPIMGDOCPATH = @"E:\\PIP_DOCS\";
                else
                    _propPIPIMGDOCPATH = @"\\CAPSYSAZ8\PIP_DOCS\";
            }
            cmbDocStatus.Items.Add(new ListItem("", ""));
            cmbDocStatus.Items.Add(new ListItem("Completed", "C"));
            cmbDocStatus.Items.Add(new ListItem("Incomplete", "I"));
            cmbDocStatus.SelectedIndex = 0;
            lblAppnum.Text = "App#: " + _propAgency + _propDept + _propProg + " " + (_propYear == string.Empty ? "    " : _propYear) + " " + _propApplNo;
            // FillIntakeData();


        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        List<CaseSnpEntity> casesnpImageEntity { get; set; }
        private string _propPIPIMGDOCPATH { get; set; }
        string _propConformationNum { get; set; }
        string _propPIP_REG_ID { get; set; }
        List<CommonEntity> propimagetypesCategory { get; set; }
        public string _propAgency { get; set; }
        public string _propDept { get; set; }
        public string _propProg { get; set; }
        public string _propYear { get; set; }
        public string _propApplNo { get; set; }
        List<CommonEntity> _propImagesTypesOnly = new List<CommonEntity>();


        string EditImage = Utilities.Consts.Icons.ico_Edit;
        string AddImage = Consts.Icons.ico_Add;

        //Gizmox.WebGUI.Common.Resources.ResourceHandle EditImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("EditIcon.gif");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle AddImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("AddItem.gif");

        private void dataGridCaseSnp_SelectionChanged(object sender, EventArgs e)
        {

            if (dataGridCaseSnp.SelectedRows.Count > 0)
            {
                strDeleteEnable = string.Empty;
                gvwFiles.SelectionChanged -= new EventHandler(gvwFiles_SelectionChanged);
                gvwVerification.SelectionChanged -= new EventHandler(gvwVerification_SelectionChanged);
                gvwFiles.Rows.Clear();
                gvwVerification.Rows.Clear();
                cmbDocStatus.SelectedIndex = 0;
                txtRemarks.Text = string.Empty;
                cmbDocStatus.Enabled = false;
                txtRemarks.Enabled = false;
                btnOk.Visible = false;
                btnCancel.Text = "&Close";
                // btnEmailSend.Visible = btnEmailHist.Visible = false;
                string strDocument = string.Empty;
                int intRowdex;
                if (dataGridCaseSnp.Rows.Count > 0)
                {

                    if (dataGridCaseSnp.SelectedRows[0].Selected)
                    {
                        if (_propPIP_REG_ID != string.Empty)
                        {
                            //if (dtPipEmail.Rows.Count > 0)
                            //{
                            //    btnEmailHist.Visible = true;
                            //    //DataView dvview = dtPipEmail.AsDataView();
                            //    //dvview.RowFilter = "PIPMAILH_UPD_ID='U'";
                            //    //if (dvview.Count > 0)
                            //    //{
                            //    //    btnMessages.Visible = true;
                            //    //}
                            //}
                        }
                        List<PIPDocEntity> pipdoclist = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value.ToString() && u.PIPDOCUPLD_LINK_DOC == string.Empty);
                        foreach (PIPDocEntity item in pipdoclist)
                        {

                            strDocument = string.Empty;
                            CommonEntity commimgdoc = _propImagesTypesOnly.Find(u => u.Code.Trim() == item.PIPDOCUPLD_DOCTYPE.Trim());
                            if (commimgdoc != null)
                            {
                                strDocument = commimgdoc.Desc;
                                intRowdex = gvwFiles.Rows.Add(strDocument, item.PIPDOCUPLD_DOCNAME, LookupDataAccess.Getdate(item.PIPDOCUPLD_DATE_ADD), item.PIPDOCUPLD_VERIFIED_BY, LookupDataAccess.Getdate(item.PIPDOCUPLD_DATE_VERIFIED), LookupDataAccess.Getdate(item.PIPDOCUPLD_MAIL_ON), LookupDataAccess.Getdate(item.PIPDOCUPLD_DATE_DRAGD));
                                gvwFiles.Rows[intRowdex].Tag = item;
                            }
                        }
                        List<PIPDocEntity> pipsingdoclist = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value.ToString() && u.PIPDOCUPLD_VERIFIED_STAT == "C" && u.PIPDOCUPLD_DRAGD_BY == string.Empty);
                        if (pipsingdoclist.Count > 0)
                        {
                            btnDragged.Enabled = true;
                        }
                        else
                        {
                            btnDragged.Enabled = false;
                        }


                        strDeleteEnable = "View";

                        if (gvwFiles.Rows.Count == 0)
                        {
                            if (pipdoclist.Count > 0)
                            {
                                CommonFunctions.MessageBoxDisplay("No authorization for this document");
                            }
                        }

                    }
                }

                if (gvwFiles.Rows.Count > 0)
                {
                    gvwFiles.Rows[0].Selected = true;
                    gvwFiles_SelectionChanged(sender, e);
                    btnEmailSend.Visible = true;
                }
                else
                {
                    btnPreview.Enabled = false;
                    btnDragged.Enabled = false;
                    btnAdd.Visible = false;


                }
                gvwFiles.SelectionChanged += new EventHandler(gvwFiles_SelectionChanged);


            }



        }


        //public void DisplayDocumentName(string strFolderPath, string strdeleteEnable, string strApplicantNo)
        //{
        //    listViewPdf.SelectedIndexChanged -= new EventHandler(listViewPdf_SelectedIndexChanged);
        //    listViewPdf.Visible = true;
        //    listViewPdf.Items.Clear();
        //    DirectoryInfo objDir = new DirectoryInfo(strFolderPath);
        //    List<IMGUPLOGEntity> imguplogEntitylist = _model.ChldMstData.GetImgUpLogList(string.Empty, strApplicantNo, string.Empty);
        //    imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_OPERATN != "D");
        //    if (Directory.Exists(strFolderPath))
        //    {

        //        foreach (FileInfo MyFile in objDir.GetFiles("*.*"))
        //        {
        //            var Extension = MyFile.Name.Substring(MyFile.Name.LastIndexOf('.') + 1).ToLower();
        //            if (Extension != "db")
        //            {
        //                ListViewItem listitem = new ListViewItem(new string[] { MyFile.Name });
        //                if (imguplogEntitylist.Count > 0)
        //                {
        //                    IMGUPLOGEntity imglogdata = imguplogEntitylist.Find(u => u.IMGLOG_ULoadAs.ToUpper().Contains(MyFile.Name.ToUpper()));
        //                    if (imglogdata != null)
        //                    {
        //                        listitem.ToolTipText = "Added By: " + imglogdata.IMGLOG_USERID + " on " + imglogdata.IMGLOG_TrnDate;
        //                    }
        //                }

        //                listViewPdf.Items.Add(listitem);
        //            }
        //        }
        //    }
        //    if (listViewPdf.Items.Count > 0)
        //    {
        //        if (strdeleteEnable == "View")
        //            btnDelete.Enabled = true;
        //        else
        //            btnDelete.Enabled = false;
        //    }
        //    else
        //        btnDelete.Enabled = false;
        //    listViewPdf.SelectedIndexChanged += new EventHandler(listViewPdf_SelectedIndexChanged);
        //}





        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strPreviewFile = "";

            if (gvwFiles.Rows.Count > 0)
            {
                if (gvwFiles.SelectedRows[0].Selected)
                {
                    using (NetworkShareAccesser.Access("CAPSYSAZ8", "PIP_Docs", "ApplicationUser", "7!Enough32Distract$"))
                    {
                        string strFilterAgy = "00";
                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                            strFilterAgy = BaseForm.BaseAgency;
                        strPreviewFile = gvwFiles.SelectedRows[0].Cells["gvtFileName"].Value.ToString();
                        int Count = strPreviewFile.Length;
                        string strExtension = (strPreviewFile.Substring(Count - 4, 4));
                        if ((strExtension.ToUpper() == ".PDF") || (strExtension.ToUpper() == ".TXT"))
                        {


                            PdfViewerNewForm objfrm = new PdfViewerNewForm(_propPIPIMGDOCPATH + BaseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString() + "\\" + strPreviewFile);
                            objfrm.StartPosition= FormStartPosition.CenterScreen;
                            objfrm.ShowDialog();
                        }
                        else if ((strExtension.ToUpper() == ".DOC") || (strExtension.ToUpper() == ".XLS") || (strExtension.ToUpper() == "DOCX") || (strExtension.ToUpper() == "XLSX"))
                        {
                            //CommonFunctions.MessageBoxDisplay("you can't preview this file.Please wait downloaded this file");
                            FileDownloadGateway downloadGateway = new FileDownloadGateway();
                            downloadGateway.Filename = strPreviewFile;

                            downloadGateway.SetContentType(DownloadContentType.OctetStream);

                            downloadGateway.StartFileDownload(new ContainerControl(), _propPIPIMGDOCPATH + BaseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString() + "\\" + strPreviewFile);
                        }
                        else
                        {
                            PdfViewerNewForm objfrm = new PdfViewerNewForm(_propPIPIMGDOCPATH + BaseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString() + "\\" + strPreviewFile);
                            objfrm.StartPosition = FormStartPosition.CenterScreen;
                            objfrm.ShowDialog();
                        }
                    }
                }
            }
        }

        private void btnDragged_Click(object sender, EventArgs e)
        {
            if (dataGridCaseSnp.Rows.Count > 0)
            {
                string strPipId = dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value.ToString();
                PIP00001DocumentViewForm _pipdocumentview = new PIP00001DocumentViewForm(BaseForm, Privileges, dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity, _propPIP_REG_ID, _propDocentitylist, strPipId, _propImagesTypesOnly);
                _pipdocumentview.FormClosed += new FormClosedEventHandler(_pipdocumentview_FormClosed);
                _pipdocumentview.StartPosition = FormStartPosition.CenterScreen;
                _pipdocumentview.ShowDialog();

            }
        }

        private void _pipdocumentview_FormClosed(object sender, FormClosedEventArgs e)
        {
            PIP00001DocumentViewForm form = sender as PIP00001DocumentViewForm;
            if (form._propFormStatus != string.Empty)
            {
                FillIntakeData();
            }
        }

        //private void MessageBoxHandler(object sender, EventArgs e)
        //{
        //    // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
        //    string strOnlyFile = string.Empty;
        //    Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;
        //    string strDeleteFile = "";
        //    if (senderForm != null)
        //    {
        //        // Set DialogResult value of the Form as a text for label
        //        if (senderForm.DialogResult.ToString() == "Yes")
        //        {
        //            foreach (DataGridViewRow item in dataGridCaseSnp.Rows)
        //            {
        //                if (item.Selected)
        //                {
        //                    if (dataGridCaseSnp.SelectedRows[0].Tag is CaseSnpEntity)
        //                    {
        //                        var selectedItems = listViewPdf.SelectedItems;
        //                        if (selectedItems.Count > 0)
        //                        {
        //                            strDeleteFile = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
        //                        }

        //                        CaseSnpEntity casesnpimag = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
        //                        if (chkHouseHold.Checked == true)
        //                        {
        //                            strFullFolderName = strImageFolderName + "\\" + casesnpimag.Agency + casesnpimag.Dept + casesnpimag.Program + casesnpimag.Year.ToString().Trim() + casesnpimag.App + "\\" + "0000000".Substring(0, 7 - casesnpimag.FamilySeq.Length) + casesnpimag.FamilySeq;
        //                            strOnlyFile = casesnpimag.Agency + casesnpimag.Dept + casesnpimag.Program + casesnpimag.Year.ToString().Trim() + casesnpimag.App + "\\" + "0000000".Substring(0, 7 - casesnpimag.FamilySeq.Length) + casesnpimag.FamilySeq;
        //                        }
        //                        else
        //                        {
        //                            strFullFolderName = strImageFolderName + "\\" + casesnpimag.Agency + casesnpimag.Dept + casesnpimag.Program + casesnpimag.Year.ToString().Trim() + casesnpimag.App;
        //                            strOnlyFile = casesnpimag.Agency + casesnpimag.Dept + casesnpimag.Program + casesnpimag.Year.ToString().Trim() + casesnpimag.App;
        //                        }
        //                        strMainFolderName = strImageFolderName + "\\" + casesnpimag.Agency + casesnpimag.Dept + casesnpimag.Program + casesnpimag.Year.ToString().Trim() + casesnpimag.App;

        //                        MyDir = new DirectoryInfo(strFullFolderName);
        //                        FileInfo[] MyFiles = MyDir.GetFiles("*.*");
        //                        foreach (FileInfo MyFile in MyFiles)
        //                        {
        //                            if (MyFile.Exists)
        //                            {
        //                                if (strDeleteFile == MyFile.Name)
        //                                {
        //                                    MyFile.Delete();
        //                                    InsertDeleteImgUploagLog("D", strDeleteFile, strOnlyFile + "\\" + strDeleteFile);
        //                                }
        //                            }
        //                        }

        //                        DisplayDocumentName(strFullFolderName, strDeleteEnable, casesnpimag.Agency + casesnpimag.Dept + casesnpimag.Program + (casesnpimag.Year.Trim() == string.Empty ? "    " : casesnpimag.Year) + casesnpimag.App);
        //                        string strImageCout = string.Empty;
        //                        MyDir = new DirectoryInfo(strFullFolderName);
        //                        if (MyDir.Exists)
        //                        {
        //                            FileInfo[] MyFiles1 = MyDir.GetFiles("*.*");
        //                            strImageCout = MyFiles1.Length.ToString();
        //                        }
        //                        dataGridCaseSnp.SelectedRows[0].Cells["gvtimgCount"].Value = strImageCout;
        //                        listViewPdf_SelectedIndexChanged(sender, e);
        //                        break;
        //                    }
        //                }
        //            }

        //        }
        //    }
        //}

        private void InsertDeleteImgUploagLog(string strOpertype, string strSecuritytype, string strType, string strimgFileName, string strimgloadas)
        {
            IMGUPLOGNEntity imglogentity = new IMGUPLOGNEntity();
            imglogentity.IMGLOG_AGY = _propAgency;
            imglogentity.IMGLOG_DEP = _propDept;
            imglogentity.IMGLOG_PROG = _propProg;
            imglogentity.IMGLOG_YEAR = _propYear;
            imglogentity.IMGLOG_APP = _propApplNo;
            imglogentity.IMGLOG_FAMILY_SEQ = "";
            imglogentity.IMGLOG_SCREEN = Privileges.Program;
            imglogentity.IMGLOG_SECURITY = strSecuritytype;
            imglogentity.IMGLOG_TYPE = strType;
            imglogentity.IMGLOG_UPLoadAs = strimgloadas;
            imglogentity.IMGLOG_UPLOAD_BY = BaseForm.UserID;
            imglogentity.IMGLOG_ORIG_FILENAME = strimgFileName;
            imglogentity.MODE = strOpertype;
            _model.ChldMstData.InsertIMGUPLOG(imglogentity);
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {


        //        using (NetworkShareAccesser.Access("CAPSYSAZ8", "PIP_Docs", "ApplicationUser", "7!Enough32Distract$"))
        //        {

        //            //listViewPdf.SelectedIndexChanged -= new EventHandler(listViewPdf_SelectedIndexChanged);

        //            //listViewPdf.Items.Clear();

        //            gvwFiles.SelectionChanged -= new EventHandler(gvwFiles_SelectionChanged);
        //            gvwFiles.Rows.Clear();
        //            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
        //            FileInfo[] file = dir.GetFiles();//here you get all file of the network shared folder

        //            //here you perform your operation

        //            string propReportPath = _model.lookupDataAccess.GetReportPath();

        //            foreach (FileInfo filepath in file)
        //            {
        //                //ListViewItem listitem = new ListViewItem(new string[] { filepath.Name });


        //                //listViewPdf.Items.Add(listitem);

        //                gvwFiles.Rows.Add(true, filepath.Name);
        //                // string fileName = Path.GetFileName(filepath.Name);

        //                //File.Copy(textBox1.Text + filepath.Name, propReportPath + "\\" + BaseForm.UserID + "\\" + fileName);

        //                // CommonFunctions.MessageBoxDisplay(txtEmailId.Text + filepath.Name);
        //                // CommonFunctions.MessageBoxDisplay(propReportPath + "\\SYSTEM\\" + fileName);
        //            }

        //            //if (listViewPdf.Items.Count > 0)
        //            //{

        //            //    listViewPdf.SelectedIndexChanged += new EventHandler(listViewPdf_SelectedIndexChanged);
        //            //}

        //            gvwFiles.SelectionChanged += new EventHandler(gvwFiles_SelectionChanged);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        CommonFunctions.MessageBoxDisplay(ex.Message);
        //    }
        //}


        private void gvwFiles_SelectionChanged(object sender, EventArgs e)
        {

            if (gvwFiles.Rows.Count > 0)
            {
                btnPreview.Enabled = true;
                if (Privileges.AddPriv.Equals("true"))
                {
                    btnAdd.Visible = true;
                }
                btnEmailSend.Visible = true;

                if (gvwFiles.SelectedRows[0].Selected)
                {
                    gvtVRemarks.Width = 480;
                    if (Privileges.ChangePriv.Equals("true"))
                    {
                        gviVedit.Visible = true;
                    }
                    if (Privileges.DelPriv.Equals("true"))
                    {
                        gvIDel.Visible = true;
                    }
                    string strDragged = gvwFiles.SelectedRows[0].Cells["gvtFDragged"].Value == null ? string.Empty : gvwFiles.SelectedRows[0].Cells["gvtFDragged"].Value.ToString();
                    if (!string.IsNullOrEmpty(strDragged))
                    {
                        gvtVRemarks.Width = 550;
                        btnAdd.Visible = false;
                        gviVedit.Visible = false;
                        gvIDel.Visible = false;
                        btnPreview.Enabled = false;
                    }
                    else
                    {

                    }

                    gvwVerification.SelectionChanged -= new EventHandler(gvwVerification_SelectionChanged);
                    PIPDocEntity pipdocentity = gvwFiles.SelectedRows[0].Tag as PIPDocEntity;
                    if (pipdocentity != null)
                    {
                        gvwVerification.Rows.Clear();
                        txtRemarks.Text = string.Empty;
                        cmbDocStatus.SelectedIndex = 0;

                        List<PIPDocVerEntity> pipdocVerlist = PIPDATA.GETPIPDOCVER(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, pipdocentity.PIPDOCUPLD_ID, string.Empty);
                        foreach (PIPDocVerEntity docveritem in pipdocVerlist)
                        {
                            int rowindex = gvwVerification.Rows.Add(LookupDataAccess.Getdate(docveritem.PIPDOCVER_DATE), docveritem.PIPDOCVER_BY, (docveritem.PIPDOCVER_STATUS == "C" ? "Completed" : "Incomplete"), docveritem.PIPDOCVER_Remarks, LookupDataAccess.Getdate(docveritem.PIPDOCVER_MAIL_ON), docveritem.PIPDOCVER_ID);
                            gvwVerification.Rows[rowindex].Tag = docveritem;
                        }
                        if (pipdocentity.PIPDOCUPLD_SECURITY.ToUpper() == "DEC")
                        {
                            gvtVRemarks.Width = 550;
                            btnAdd.Visible = false;
                            gviVedit.Visible = false;
                            gvIDel.Visible = false;
                        }


                    }

                    if (gvwVerification.Rows.Count > 0)
                    {
                        gvwVerification.Rows/*SelectedRows*/[0].Selected = true;
                        gvwVerification_SelectionChanged(sender, e);
                    }
                    else
                    {
                        btnOk.Visible = false;
                        //if (dtPipEmail.Rows.Count > 0)
                        //{
                        //    btnEmailHist.Visible = true;
                        //}
                        cmbDocStatus.SelectedIndex = 0;
                        txtRemarks.Text = string.Empty;
                        btnEmailSend.Visible = true;
                        cmbDocStatus.Enabled = false;
                        txtRemarks.Enabled = false;
                        btnCancel.Text = "&Close";

                    }
                    gvwVerification.SelectionChanged += new EventHandler(gvwVerification_SelectionChanged);
                }



                //    pnlHtml.Controls.Clear();
                //    if (gvwFiles.SelectedRows[0].Selected)
                //    {
                //        //pnlHtml.Controls.Clear();



                //        string strFileName = gvwFiles.SelectedRows[0].Cells["gvtFileName"].Value.ToString();
                //        var Extension = strFileName.Substring(strFileName.LastIndexOf('.') + 1).ToLower();
                //        bool booldisplay = true;
                //        switch (Extension)
                //        {
                //            case "doc":
                //                booldisplay = false;
                //                break;
                //            case "xls":
                //                booldisplay = false;
                //                break;
                //            case "xlsx":
                //                booldisplay = false;
                //                break;
                //            case "docx":
                //                booldisplay = false;
                //                break;
                //        }

                //        if (booldisplay)
                //        {
                //            pnlHtml.Controls.Clear();
                //            //PreviewControl priview = new PreviewControl(strFullFolderName + "\\" + strFileName, "ImageView");
                //            PreviewControl priview = new PreviewControl(textBox1.Text + "\\" + strFileName, "ImageView");
                //            priview.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
                //            pnlHtml.Controls.Add(priview);
                //        }





                //    }
            }
            else
            {
                btnAdd.Visible = false;
                btnOk.Visible = false;
                //if (dtPipEmail.Rows.Count > 0)
                //{
                //    btnEmailHist.Visible = true;
                //}
                cmbDocStatus.SelectedIndex = 0;
                txtRemarks.Text = string.Empty;
                btnEmailSend.Visible = true;
                cmbDocStatus.Enabled = false;
                txtRemarks.Enabled = false;
                btnCancel.Text = "&Close";
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (gvwFiles.Rows.Count > 0)
        //    {
        //        using (NetworkShareAccesser.Access("CAPSYSAZ8", "PIP_Docs", "ApplicationUser", "7!Enough32Distract$"))
        //        {



        //            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
        //            FileInfo[] file = dir.GetFiles();//here you get all file of the network shared folder

        //            //here you perform your operation

        //            string propReportPath = _model.lookupDataAccess.GetReportPath();
        //            propReportPath = propReportPath + "\\" + BaseForm.UserID + "\\";

        //            int i = 0;

        //            foreach (FileInfo filepath in file)
        //            {


        //                foreach (DataGridViewRow gvitem in gvwFiles.Rows)
        //                {
        //                    if (gvitem.Cells["gvtChkSelect"].Value.ToString().ToUpper() == "TRUE")
        //                    {
        //                        if (filepath.Name == gvitem.Cells["gvtFileName"].Value.ToString())
        //                        {
        //                            var local = Path.Combine(propReportPath, filepath.Name);
        //                            var remote = Path.Combine(textBox1.Text, filepath.Name);
        //                            i = i + 1;
        //                            if (File.Exists(local))
        //                            {
        //                                File.Delete(local);

        //                                File.Copy(remote, local, true);
        //                            }
        //                            else
        //                            {
        //                                File.Copy(remote, local, true);
        //                            }

        //                            break;
        //                        }
        //                    }

        //                }


        //            }
        //            if (i > 0)
        //            {

        //                CommonFunctions.MessageBoxDisplay("Total " + i.ToString() + " Files copied");
        //            }
        //            else
        //            {
        //                CommonFunctions.MessageBoxDisplay("Please select atleast one file");
        //            }
        //        }

        //    }
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    string propReportPath = _model.lookupDataAccess.GetReportPath();
        //    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
        //    pdfListForm.ShowDialog();
        //}

        public List<PIPDocEntity> _propDocentitylist = new List<PIPDocEntity>();
        DataTable dtPIPIntake = new DataTable();
        DataTable dtPipEmail = new DataTable();

        private void FillIntakeData()
        {
            string strFilterAgy = string.Empty;
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                strFilterAgy = _propAgency;//BaseForm.BaseAgency.ToString();
            dataGridCaseSnp.SelectionChanged -= new EventHandler(dataGridCaseSnp_SelectionChanged);
            gvwFiles.SelectionChanged -= new EventHandler(gvwFiles_SelectionChanged);
            gvwVerification.SelectionChanged -= new EventHandler(gvwVerification_SelectionChanged);
            dataGridCaseSnp.Rows.Clear();
            gvwFiles.Rows.Clear();
            gvwVerification.Rows.Clear();
            // btnEmailSend.Visible = btnEmailHist.Visible = false;
            txtRemarks.Text = string.Empty;
            cmbDocStatus.SelectedIndex = 0;



            List<PIPCAPLNK> pipcaplnkdata = PIPDATA.GetPIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, _propAgency, _propDept, _propProg, _propYear, _propApplNo, BaseForm.BaseAgencyControlDetails.AgyShortName, string.Empty, _propPIP_REG_ID);// GetPIPIntakeData(string.Empty, txtTokenNumber.Text, txtEmailId.Text, "DOC", BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);

            if (pipcaplnkdata.Count > 0)
            {
                string strImageCout = string.Empty;

                lblConf2.Text = _propConformationNum;

                DataTable dt = PIPDATA.GetPIPIntakeSearchData(BaseForm.BaseLeanDataBaseConnectionString, _propPIP_REG_ID, string.Empty, string.Empty, "DOC", BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);
                dtPIPIntake = dt;
                if (dtPIPIntake.Rows.Count > 0)
                {

                    lblEmail2.Text = dtPIPIntake.Rows[0]["PIPREG_EMAIL"].ToString();
                    //dtPipEmail = PIPDATA.PIPEMAILHIST(BaseForm.BaseLeanDataBaseConnectionString, _propPIP_REG_ID.ToString());
                    //if (dtPipEmail.Rows.Count > 0)
                    //{
                    //    btnEmailHist.Visible = true;

                    //}
                }
                _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propPIP_REG_ID, "REGID");

            }

            List<CommonEntity> Relation = _model.lookupDataAccess.GetRelationship();
            string pipdocumentcount = string.Empty;
            string pipVerifiedcount = string.Empty;
            string pipDraggedcount = string.Empty;
            string strPIPId = string.Empty;
            string strRegid = string.Empty;
            List<CaseMstEntity> propcasemstlist = _model.CaseMstData.GetCaseMstadpyn(_propAgency, _propDept, _propProg, _propYear, _propApplNo);
            List<CaseSnpEntity> propcasesnplist = _model.CaseMstData.GetCaseSnpadpyn(_propAgency, _propDept, _propProg, _propYear, _propApplNo);


            foreach (CaseSnpEntity snpEntity in propcasesnplist)
            {
                if (propcasemstlist[0].FamilySeq.Equals(snpEntity.FamilySeq))
                {

                    string ApplicantName = LookupDataAccess.GetMemberName(snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast, BaseForm.BaseHierarchyCnFormat);//snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();

                    string SSNum = LookupDataAccess.GetCardNo(snpEntity.Ssno, "1", string.Empty, snpEntity.SsnReason);
                    pipdocumentcount = string.Empty;
                    pipVerifiedcount = string.Empty;
                    pipDraggedcount = string.Empty;
                    strPIPId = string.Empty;
                    if (pipcaplnkdata.Count > 0)
                    {
                        PIPCAPLNK piplinksdata = pipcaplnkdata.Find(u => u.PIPCAPLNK_FAMILY_SEQ == snpEntity.FamilySeq);
                        if (piplinksdata != null)
                        {
                            List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == piplinksdata.PIPCAPLNK_PIP_ID && u.PIPDOCUPLD_LINK_DOC == string.Empty);
                            strPIPId = piplinksdata.PIPCAPLNK_PIP_ID;
                            pipdocumentcount = pipdoccount.Count.ToString();
                            if (pipdoccount.Count > 0)
                            {
                                pipVerifiedcount = pipdoccount.FindAll(u => u.PIPDOCUPLD_VERIFIED_STAT == "C").Count.ToString();
                                pipDraggedcount = pipdoccount.FindAll(u => u.PIPDOCUPLD_DATE_DRAGD != string.Empty).Count.ToString();
                            }

                        }
                    }
                    string memberCode = string.Empty;
                    CommonEntity rel = Relation.Find(u => u.Code.Equals(snpEntity.MemberCode));
                    if (rel != null) memberCode = rel.Desc;

                    int rowIndex = dataGridCaseSnp.Rows.Add(ApplicantName, SSNum, LookupDataAccess.Getdate(snpEntity.AltBdate), memberCode, pipdocumentcount, pipVerifiedcount, pipDraggedcount, strPIPId, _propPIP_REG_ID);// pipdoccount.Count.ToString(), dritem["PIP_ID"].ToString(), dritem["PIP_CONFNO"].ToString());
                    if (dataGridCaseSnp.Rows.Count > 0) dataGridCaseSnp.Rows[0].Selected = true;


                    dataGridCaseSnp.Rows[rowIndex].Tag = snpEntity;
                    dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                }
            }


            foreach (CaseSnpEntity snpEntity in propcasesnplist)
            {
                if (!propcasemstlist[0].FamilySeq.Equals(snpEntity.FamilySeq))
                {
                    string ApplicantName = LookupDataAccess.GetMemberName(snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast, BaseForm.BaseHierarchyCnFormat);//snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();

                    string SSNum = LookupDataAccess.GetCardNo(snpEntity.Ssno, "1", string.Empty, snpEntity.SsnReason);
                    pipdocumentcount = string.Empty;
                    pipVerifiedcount = string.Empty;
                    pipDraggedcount = string.Empty;
                    strPIPId = string.Empty;
                    if (pipcaplnkdata.Count > 0)
                    {
                        PIPCAPLNK piplinksdata = pipcaplnkdata.Find(u => u.PIPCAPLNK_FAMILY_SEQ == snpEntity.FamilySeq);
                        if (piplinksdata != null)
                        {
                            List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == piplinksdata.PIPCAPLNK_PIP_ID && u.PIPDOCUPLD_LINK_DOC == string.Empty);
                            strPIPId = piplinksdata.PIPCAPLNK_PIP_ID;
                            pipdocumentcount = pipdoccount.Count.ToString();
                            if (pipdoccount.Count > 0)
                            {
                                pipVerifiedcount = pipdoccount.FindAll(u => u.PIPDOCUPLD_VERIFIED_STAT == "C").Count.ToString();
                                pipDraggedcount = pipdoccount.FindAll(u => u.PIPDOCUPLD_DATE_DRAGD != string.Empty).Count.ToString();
                            }
                        }
                    }
                    string memberCode = string.Empty;
                    CommonEntity rel = Relation.Find(u => u.Code.Equals(snpEntity.MemberCode));
                    if (rel != null) memberCode = rel.Desc;

                    int rowIndex = dataGridCaseSnp.Rows.Add(ApplicantName, SSNum, LookupDataAccess.Getdate(snpEntity.AltBdate), memberCode, pipdocumentcount, pipVerifiedcount, pipDraggedcount, strPIPId, _propPIP_REG_ID);// pipdoccount.Count.ToString(), dritem["PIP_ID"].ToString(), dritem["PIP_CONFNO"].ToString());
                    if (dataGridCaseSnp.Rows.Count > 0) dataGridCaseSnp.Rows[0].Selected = true;
                    dataGridCaseSnp.Rows[rowIndex].Tag = snpEntity;
                }
            }




            //if (dt.Rows.Count > 0)
            //{
            //    string strImageCout = string.Empty;

            //    _propConformationNum = dt.Rows[0]["PIP_CONFNO"].ToString();

            //    _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, dt.Rows[0]["PIP_CONFNO"].ToString(), string.Empty, "CONFNO");

            //    foreach (DataRow dritem in dt.Rows)
            //    {

            //        strImageCout = string.Empty;
            //        //DirectoryInfo MyDir = new DirectoryInfo(strFullFolderName);
            //        //if (MyDir.Exists)
            //        //{
            //        //    FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            //        //    strImageCout = MyFiles.Length.ToString();
            //        //}

            //        List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dritem["PIP_ID"].ToString());
            //        string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);
            //        string memberCode = string.Empty;
            //        CommonEntity rel = Relation.Find(u => u.Code.Equals(dritem["PIP_MEMBER_CODE"].ToString()));
            //        if (rel != null) memberCode = rel.Desc;
            //        string DOB = string.Empty;
            //        if (dritem["PIP_DOB"].ToString() != string.Empty)
            //            DOB = CommonFunctions.ChangeDateFormat(dritem["PIP_DOB"].ToString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
            //        int rowIndex = dataGridCaseSnp.Rows.Add(ApplicantName, dritem["PIP_SSN"].ToString(), DOB, memberCode, pipdoccount.Count.ToString(), dritem["PIP_ID"].ToString(), dritem["PIP_CONFNO"].ToString());
            //        dataGridCaseSnp.Rows[rowIndex].Tag = dritem;



            //        if (dritem["PIP_FAM_SEQ"].ToString() == "0")
            //        {
            //            dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

            //        }
            //    }
            if (dataGridCaseSnp.Rows.Count > 0)
            {
                dataGridCaseSnp.SelectionChanged += new EventHandler(dataGridCaseSnp_SelectionChanged);
                dataGridCaseSnp_SelectionChanged(dataGridCaseSnp, new EventArgs());
            }

            //}
            //else
            //{

            //    CommonFunctions.MessageBoxDisplay("Confirmation Number or Email ID doesn't existed");
            //}


        }

        private bool isValidate()
        {
            bool isValid = true;


            //if (String.IsNullOrEmpty(txtTokenNumber.Text.Trim()) && String.IsNullOrEmpty(txtEmailId.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtTokenNumber, "Please enter Confirmation Number or Email id");
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(txtTokenNumber, null);
            //}
            return isValid;
        }

        private void panel3_Click(object sender, EventArgs e)
        {

        }


        public string strFormCloseRefresh = string.Empty;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text.ToUpper() == "&CLOSE")
            {
                this.Close();

            }
            else
            {
                _errorProvider.SetError(txtRemarks, null);
                _errorProvider.SetError(cmbDocStatus, null);
                btnOk.Visible = false;
                //if (dtPipEmail.Rows.Count > 0)
                //{
                //    btnEmailHist.Visible = true;

                //}
                btnEmailSend.Visible = true;
                cmbDocStatus.Enabled = false;
                txtRemarks.Enabled = false;
                btnCancel.Text = "&Close";
                strMode = string.Empty;
                gvwFiles_SelectionChanged(sender, e);
            }
        }
        string strinPIPDOCVERID = string.Empty;

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (isSaveValidate())
            {
                if (gvwFiles.Rows.Count > 0)
                {
                    if (gvwFiles.SelectedRows[0].Selected)
                    {

                        PIPDocEntity pipdocdata = gvwFiles.SelectedRows[0].Tag as PIPDocEntity;
                        if (pipdocdata != null)
                        {
                            if (PIPDATA.InsertUpdatePipDoc(BaseForm.BaseLeanDataBaseConnectionString, pipdocdata.PIPDOCUPLD_ID, ((ListItem)cmbDocStatus.SelectedItem).Value.ToString(), BaseForm.UserID, txtRemarks.Text, "UPDATEVER") > 0)
                            {
                                string _strPIPID = dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value == null ? string.Empty : dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value.ToString();
                                strFormCloseRefresh = "Y";
                                PIPDATA.InsertUpdatePipDocVer(BaseForm.BaseLeanDataBaseConnectionString, strinPIPDOCVERID, pipdocdata.PIPDOCUPLD_ID, ((ListItem)cmbDocStatus.SelectedItem).Value.ToString(), BaseForm.UserID, txtRemarks.Text, strMode, _strPIPID, _propPIP_REG_ID);
                                _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propPIP_REG_ID, "REGID");
                                gvwFiles_SelectionChanged(sender, e);
                                DraggedButtonEnableOption();
                                //dataGridCaseSnp_SelectionChanged(dataGridCaseSnp, new EventArgs());
                                strMode = string.Empty;
                                //if (Privileges.AddPriv.Equals("true"))
                                //{
                                //    btnAdd.Visible = true;
                                //}
                            }

                        }
                    }
                    AlertBox.Show("Saved Successfully");
                }

            }
        }

        private bool isSaveValidate()
        {
            bool isValid = true;

            _errorProvider.SetError(txtRemarks, null);
            if (((ListItem)cmbDocStatus.SelectedItem).Value.ToString() == "")
            {
                _errorProvider.SetError(cmbDocStatus, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerification.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbDocStatus, null);
            }

            if (((ListItem)cmbDocStatus.SelectedItem).Value.ToString() == "I")
            {
                if (string.IsNullOrEmpty(txtRemarks.Text))
                {
                    _errorProvider.SetError(txtRemarks, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRemarks.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtRemarks, null);
                }
            }
            return isValid;
        }
        string _propstrUpddocid = string.Empty;
        private void btnEmailSend_Click(object sender, EventArgs e)
        {
            if (dataGridCaseSnp.Rows.Count > 0)
            {
                //DataRow dr = (dataGridCaseSnp.SelectedRows[0].Tag as DataRow);
                if (dataGridCaseSnp.SelectedRows[0].Selected)
                {
                    string strConformNum = dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value == null ? string.Empty : dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString();

                    PIP0001SendMailForm form = new PIP0001SendMailForm(BaseForm, Privileges, lblConf2.Text, lblEmail2.Text, _propDocentitylist, dtPIPIntake, _propPIP_REG_ID);
                    form.FormClosed += new Wisej.Web.FormClosedEventHandler(objform_FormClosed);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                    //if (strConformNum != string.Empty)
                    //{
                    //    DataTable dt = PIPDATA.GETPIPREG(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, _propPIP_REG_ID, string.Empty, string.Empty, string.Empty, "ID");


                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        try
                    //        {
                    //            DataTable dtcontent = PIPDATA.PIPMAILS_GET(BaseForm.BaseLeanDataBaseConnectionString, dt.Rows[0]["PIPREG_AGENCY"].ToString(), dt.Rows[0]["PIPREG_AGY"].ToString(), "DOV", string.Empty);

                    //            if (dtcontent.Rows.Count > 0)
                    //            {
                    //                MailMessage mail = new MailMessage();
                    //                mail.To.Add(dt.Rows[0]["PIPREG_EMAIL"].ToString());
                    //                mail.From = new MailAddress(ConfigurationManager.AppSettings["UserName"].ToString());
                    //                mail.Subject = ConfigurationManager.AppSettings["Subject"].ToString();

                    //                _propstrUpddocid = string.Empty;
                    //                mail.Body = createEmailBody(string.Empty, dt.Rows[0]["PIPREG_FNAME"].ToString(), dt.Rows[0]["PIPREG_LNAME"].ToString(), txtRemarks.Text, dtcontent);
                    //                mail.IsBodyHtml = true;

                    //                SmtpClient smtp = new SmtpClient();

                    //                smtp.Host = ConfigurationManager.AppSettings["Host"];

                    //                smtp.EnableSsl = true;

                    //                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                    //                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"]; //reading from web.config  

                    //                NetworkCred.Password = ConfigurationManager.AppSettings["Password"]; //reading from web.config  

                    //                smtp.UseDefaultCredentials = true;

                    //                smtp.Credentials = NetworkCred;

                    //                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]); //reading from web.config  
                    //                if (_propstrUpddocid != string.Empty)
                    //                {
                    //                    smtp.Send(mail);
                    //                    string[] strIds = _propstrUpddocid.Split(',');
                    //                    foreach (string stritem in strIds)
                    //                    {
                    //                        if (stritem != string.Empty)
                    //                        {
                    //                            PIPDATA.InsertUpdatePipDoc(BaseForm.BaseLeanDataBaseConnectionString, stritem, string.Empty, BaseForm.UserID, string.Empty, "EMAIL");
                    //                        }
                    //                    }

                    //                    PIPDATA.InsertUpdatePIPEmailHistory(BaseForm.BaseLeanDataBaseConnectionString, dt.Rows[0]["PIPREG_ID"].ToString(), _propstrUpddocid, BaseForm.UserID);

                    //                    _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propConformationNum, string.Empty, "CONFNO");
                    //                    dataGridCaseSnp_SelectionChanged(dataGridCaseSnp, new EventArgs());

                    //                    CommonFunctions.MessageBoxDisplay("Success");
                    //                }
                    //                else
                    //                {
                    //                    CommonFunctions.MessageBoxDisplay("This verification document files already sent the mail");
                    //                }
                    //            }
                    //            else
                    //            {
                    //                CommonFunctions.MessageBoxDisplay("Please Add Document Verification content in PIP Administration Form");
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {

                    //            CommonFunctions.MessageBoxDisplay("Error " + ex.Message);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    CommonFunctions.MessageBoxDisplay("This applicant Pip data not existed");
                    //}
                }
            }
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            PIP0001SendMailForm form = sender as PIP0001SendMailForm;
            if (form.strMailsender != string.Empty)
            {
                dtPipEmail = PIPDATA.PIPEMAILHIST(BaseForm.BaseLeanDataBaseConnectionString, _propPIP_REG_ID.ToString());
                _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propPIP_REG_ID, "REGID");
                dataGridCaseSnp_SelectionChanged(dataGridCaseSnp, new EventArgs());
            }

        }

        private string createEmailBody(string userName, string strFirsName, string strLastName, string message, DataTable dtcontent)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Application.MapPath("~\\Resources\\pipdocuploademail.html")))
            {
                body = reader.ReadToEnd();

            }

            string strMainLogopath = string.Empty;
            string CompanyName = string.Empty;
            string Address = string.Empty;
            string City = string.Empty;
            string State = string.Empty;
            string Country = string.Empty;
            string Pin = string.Empty;
            string strImagurl = string.Empty;
            string Message = string.Empty;
            string Prefix = string.Empty;
            string strName = string.Empty;
            string verfymaillink = string.Empty;
            string h2title = string.Empty;


            //// string selLanguage = Master.SelectedLanguage
            //verfymaillink = "<a  onmousedown='return false'  style='padding:10px; font-size:18px; background-color:#123dcc; border-radius:8px; color:#fff; text-decoration:none; text-transform:none; font-weight:bold;' href='https://pip.capsystems.com/PIP20/Regstatus.aspx?ln=en&dnurl=" + Session["DNUrl"].ToString() + "&typ=A&rud=" + userid + "'>Verify your email</a>";

            ////strMainLogopath = "http://capsystems.com/images/PIPlogos/" + Session["Agency"].ToString().ToUpper() + "MAINLOGO.png";
            //strMainLogopath = WebConfigurationManager.AppSettings["AGENCYLOGOSPATH"] + Session["Agency"].ToString().ToUpper() + "_" + Session["AgyCode"].ToString() + "_LOGO.png";


            //body = body.Replace("{bgcolor}", Session["bg-color-css"].ToString());
            //body = body.Replace("{imagelogo}", strMainLogopath);

            string strtblContent = string.Empty;

            Message = dtcontent.Rows[0]["PIPMAIL_CONTENT"].ToString();
            CompanyName = dtcontent.Rows[0]["PIPMAIL_SENDER_NAME"].ToString();
            Address = dtcontent.Rows[0]["PIPMAIL_SENDER_ADDR"].ToString();

            Prefix = "Dear ";
            strName = dtcontent.Rows[0]["PIPMAIL_NAME_FORMAT"].ToString();
            City = string.Empty;
            State = string.Empty;
            Country = string.Empty;
            Pin = string.Empty;
            if (strName == "1")
                userName = strLastName;
            if (strName == "2")
                userName = strFirsName;
            if (strName == "3")
                userName = strLastName + " " + strFirsName;
            if (strName == "4")
                userName = strFirsName + " " + strLastName;


            StringBuilder strcontent = new StringBuilder();
            foreach (DataRow dritem in dtPIPIntake.Rows)
            {



                List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dritem["PIP_ID"].ToString());
                string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);

                if (pipdoccount.Count > 0)
                {
                    int intfirst = 0;
                    foreach (PIPDocEntity drpipdoc in pipdoccount)
                    {
                        if (drpipdoc.PIPDOCUPLD_VERIFIED_STAT != string.Empty && drpipdoc.PIPDOCUPLD_MAIL_ON == string.Empty)
                        {
                            _propstrUpddocid = _propstrUpddocid + drpipdoc.PIPDOCUPLD_ID + ",";
                            if (intfirst == 0)
                            {
                                strcontent.Append("<tr><td>" + ApplicantName + "</td>");
                                strcontent.Append("<td>" + drpipdoc.PIPDOCUPLD_DOCNAME + "</td>");
                                if (drpipdoc.PIPDOCUPLD_VERIFIED_STAT == "P")
                                {
                                    strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:20px; color:#40ba0c'>&#10003;</span></td>");
                                }
                                else
                                {
                                    strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:25px; color:#ff0000'> &#215;</span></td>");
                                }
                                strcontent.Append("<td>" + drpipdoc.PIPDOCUPLD_REMARKS + "</td></tr>");
                                intfirst = intfirst + 1;
                            }
                            else
                            {

                                strcontent.Append("<tr><td>  </td>");
                                strcontent.Append("<td>" + drpipdoc.PIPDOCUPLD_DOCNAME + "</td>");
                                if (drpipdoc.PIPDOCUPLD_VERIFIED_STAT == "P")
                                {
                                    strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:20px; color:#40ba0c'>&#10003;</span></td>");
                                }
                                else
                                {
                                    strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:25px; color:#ff0000'> &#215;</span></td>");
                                }
                                strcontent.Append("<td>" + drpipdoc.PIPDOCUPLD_REMARKS + "</td></tr>");
                            }
                        }

                    }
                }

            }

            //replacing the required things  

            body = body.Replace("{Prefix}", Prefix);
            body = body.Replace("{Name}", userName);
            body = body.Replace("{CompanyName}", CompanyName);
            body = body.Replace("{Message}", Message);

            body = body.Replace("{tbcontent}", strcontent.ToString());
            body = body.Replace("{Address}", Address);
            body = body.Replace("{City}", City);
            body = body.Replace("{State}", State);

            body = body.Replace("{Country}", Country);
            body = body.Replace("{Pin}", Pin);


            return body;

        }

        private void btnEmailHist_Click(object sender, EventArgs e)
        {

            if (dataGridCaseSnp.Rows.Count > 0)
            {

                if (dataGridCaseSnp.SelectedRows[0].Selected)
                {
                    PIP00001EmailHistoryForm emailhistory = new PIP00001EmailHistoryForm(BaseForm, _propDocentitylist, dtPIPIntake, string.Empty, _propPIP_REG_ID);
                    emailhistory.StartPosition = FormStartPosition.CenterScreen;
                    emailhistory.ShowDialog();
                }
            }
        }

        private void PIP00001Form_Load(object sender, EventArgs e)
        {
            FillIntakeData();
            if (_propConformationNum == string.Empty)
            {
                CommonFunctions.MessageBoxDisplay("This Applicant is not registered in Public Intake Portal");
                lblTokenNo.Visible = lblConf2.Visible = lblEmailId.Visible = lblEmail2.Visible = false;
            }

        }
        string strMode = string.Empty;
        private void gvwVerification_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gviVedit.Index && e.RowIndex != -1)
            {
                if (e.RowIndex == 0)
                {
                    string strDragged = gvwFiles.SelectedRows[0].Cells["gvtFDragged"].Value == null ? string.Empty : gvwFiles.SelectedRows[0].Cells["gvtFDragged"].Value.ToString();
                    if (string.IsNullOrEmpty(strDragged))
                    {
                        strMode = "Edit";
                        strinPIPDOCVERID = gvwVerification.SelectedRows[0].Cells["gvtVerID"].Value == null ? string.Empty : gvwVerification.SelectedRows[0].Cells["gvtVerID"].Value.ToString();
                        cmbDocStatus.Enabled = true;
                        txtRemarks.Enabled = true;
                        btnOk.Visible = true;
                        btnEmailSend.Visible = false;//btnEmailHist.Visible =
                        btnCancel.Text = "&Cancel";
                        btnAdd.Visible = false;
                    }
                }
            }
            if (e.ColumnIndex == gvIDel.Index && e.RowIndex != -1)
            {
                string strDragged = gvwVerification.SelectedRows[0].Cells["gvtVEmail"].Value == null ? string.Empty : gvwVerification.SelectedRows[0].Cells["gvtVEmail"].Value.ToString();
                if (string.IsNullOrEmpty(strDragged))
                {

                    strinPIPDOCVERID = gvwVerification.SelectedRows[0].Cells["gvtVerID"].Value == null ? string.Empty : gvwVerification.SelectedRows[0].Cells["gvtVerID"].Value.ToString();
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: OnDeleteMessageBoxClicked);

                }

            }
        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
           // MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {

                PIPDATA.InsertUpdatePipDocVer(BaseForm.BaseLeanDataBaseConnectionString, strinPIPDOCVERID, string.Empty, string.Empty, string.Empty, string.Empty, "Delete", string.Empty, string.Empty);
                gvwFiles_SelectionChanged(gvwFiles,EventArgs.Empty/*sender, e*/);
                AlertBox.Show("Deleted Successfully");
            }

        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            strMode = "Add";
            cmbDocStatus.SelectedIndex = 0;
            txtRemarks.Text = string.Empty;
            cmbDocStatus.Enabled = true;
            txtRemarks.Enabled = true;
            btnOk.Visible = true;
            btnEmailSend.Visible = false;//btnEmailHist.Visible =
            btnCancel.Text = "&Cancel";
            btnAdd.Visible = false;
        }

        private void gvwVerification_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwFiles.Rows.Count > 0)
            {
                if (gvwVerification.Rows.Count > 0)
                {
                    PIPDocVerEntity pipdocverentity = (gvwVerification.SelectedRows[0].Tag as PIPDocVerEntity);
                    if (pipdocverentity != null)
                    {
                        CommonFunctions.SetComboBoxValue(cmbDocStatus, pipdocverentity.PIPDOCVER_STATUS);
                        txtRemarks.Text = pipdocverentity.PIPDOCVER_Remarks;
                        btnOk.Visible = false;
                        //if (dtPipEmail.Rows.Count > 0)
                        //{
                        //    btnEmailHist.Visible = true;
                        //}
                        btnEmailSend.Visible = true;
                        cmbDocStatus.Enabled = false;
                        txtRemarks.Enabled = false;
                        btnCancel.Text = "&Close";

                    }
                }
                else
                {
                    btnOk.Visible = false;
                    //if (dtPipEmail.Rows.Count > 0)
                    //{
                    //    btnEmailHist.Visible = true;
                    //}
                    cmbDocStatus.SelectedIndex = 0;
                    txtRemarks.Text = string.Empty;
                    btnEmailSend.Visible = true;
                    cmbDocStatus.Enabled = false;
                    txtRemarks.Enabled = false;
                    btnCancel.Text = "&Close";
                }
            }
        }

        private void cmbDocStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDocStatus.Items.Count > 0)
            {
                if (((ListItem)cmbDocStatus.SelectedItem).Value.ToString() == "I")
                    lblRemarksreq.Visible = true;
                else
                    lblRemarksreq.Visible = false;
            }
        }

        private void gvwFiles_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (gvwFiles.Rows.Count > 0)
            {

                if (objArgs.MenuItem.Tag.ToString() == "C")
                {

                    PIPDocEntity pipdocdata = gvwFiles.SelectedRows[0].Tag as PIPDocEntity;
                    if (pipdocdata != null)
                    {
                        if (PIPDATA.InsertUpdatePipDoc(BaseForm.BaseLeanDataBaseConnectionString, pipdocdata.PIPDOCUPLD_ID, "C", BaseForm.UserID, string.Empty, "UPDATEVER") > 0)
                        {
                            string _strPIPID = dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value == null ? string.Empty : dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value.ToString();
                            strFormCloseRefresh = "Y";
                            PIPDATA.InsertUpdatePipDocVer(BaseForm.BaseLeanDataBaseConnectionString, strinPIPDOCVERID, pipdocdata.PIPDOCUPLD_ID, "C", BaseForm.UserID, string.Empty, "Add", _strPIPID, _propPIP_REG_ID);
                            _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propPIP_REG_ID, "REGID");
                            gvwFiles_SelectionChanged(gvwFiles, new EventArgs());
                            gvwFiles.SelectedRows[0].Cells["gvtFVerifiedBy"].Value = BaseForm.UserID;
                            gvwFiles.SelectedRows[0].Cells["gvtFVerifiedOn"].Value = LookupDataAccess.Getdate(DateTime.Now.Date.ToShortDateString());
                            pipdocdata.PIPDOCUPLD_VERIFIED_STAT = "C";
                            pipdocdata.PIPDOCUPLD_VERIFIED_BY = BaseForm.UserID;
                            pipdocdata.PIPDOCUPLD_DATE_VERIFIED = LookupDataAccess.Getdate(DateTime.Now.Date.ToShortDateString());
                            gvwFiles.SelectedRows[0].Tag = pipdocdata;
                            DraggedButtonEnableOption();
                            strMode = string.Empty;

                        }

                    }
                }


            }
        }

        private void contextComplete_Popup(object sender, EventArgs e)
        {
            contextComplete.MenuItems.Clear();
            if (gvwFiles.Rows.Count > 0)
            {                
                PIPDocEntity pipdocdata = gvwFiles.SelectedRows[0].Tag as PIPDocEntity;
                if (pipdocdata != null)
                {
                    if (pipdocdata.PIPDOCUPLD_VERIFIED_STAT != "C" || pipdocdata.PIPDOCUPLD_VERIFIED_BY == string.Empty)
                    {                        
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Complete";
                        menuItem.Tag = "C";
                        contextComplete.MenuItems.Add(menuItem);
                    }
                }
            }
        }

        private void DraggedButtonEnableOption()
        {
            //_propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propPIP_REG_ID, "REGID");
            //_propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propPIP_REG_ID, "REGID");
            List<PIPCAPLNK> pipcaplnkdata = PIPDATA.GetPIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, _propAgency, _propDept, _propProg, _propYear, _propApplNo, BaseForm.BaseAgencyControlDetails.AgyShortName, string.Empty, _propPIP_REG_ID);// GetPIPIntakeData(string.Empty, txtTokenNumber.Text, txtEmailId.Text, "DOC", BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);

            string pipdocumentcount = string.Empty;
            string pipVerifiedcount = string.Empty;
            string pipDraggedcount = string.Empty;
            string strPIPId = string.Empty;
            string strRegid = string.Empty;
            List<CaseMstEntity> propcasemstlist = _model.CaseMstData.GetCaseMstadpyn(_propAgency, _propDept, _propProg, _propYear, _propApplNo);
            List<CaseSnpEntity> propcasesnplist = _model.CaseMstData.GetCaseSnpadpyn(_propAgency, _propDept, _propProg, _propYear, _propApplNo);
            if (pipcaplnkdata.Count > 0)
            {
                CaseSnpEntity snpEntity = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
                if (snpEntity != null)
                {
                    PIPCAPLNK piplinksdata = pipcaplnkdata.Find(u => u.PIPCAPLNK_FAMILY_SEQ == snpEntity.FamilySeq);
                    if (piplinksdata != null)
                    {
                        List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == piplinksdata.PIPCAPLNK_PIP_ID && u.PIPDOCUPLD_LINK_DOC == string.Empty);
                        strPIPId = piplinksdata.PIPCAPLNK_PIP_ID;
                        pipdocumentcount = pipdoccount.Count.ToString();
                        if (pipdoccount.Count > 0)
                        {
                            pipVerifiedcount = pipdoccount.FindAll(u => u.PIPDOCUPLD_VERIFIED_STAT == "C").Count.ToString();
                            pipDraggedcount = pipdoccount.FindAll(u => u.PIPDOCUPLD_DATE_DRAGD != string.Empty).Count.ToString();
                        }
                        dataGridCaseSnp.SelectedRows[0].Cells["gvtVerifiedCount"].Value = pipVerifiedcount;
                        dataGridCaseSnp.SelectedRows[0].Cells["gvtDraggedCount"].Value = pipDraggedcount;

                        List<PIPDocEntity> pipsingdoclist = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value.ToString() && u.PIPDOCUPLD_VERIFIED_STAT == "C" && u.PIPDOCUPLD_DRAGD_BY == string.Empty);
                        if (pipsingdoclist.Count > 0)
                        {
                            btnDragged.Enabled = true;
                        }
                        else
                        {
                            btnDragged.Enabled = false;
                        }
                    }
                }
            }

        }
    }
}