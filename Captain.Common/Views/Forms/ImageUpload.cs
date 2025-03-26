#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using System.IO;
using System.Web;
using Captain.Common.Views.UserControls;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ImageUpload : Form
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



        public ImageUpload(BaseForm baseForm, PrivilegeEntity privilieges, ProgramDefinitionEntity programEntity, string MainScreencode)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2;
            _model = new CaptainModel();
            _propMainScreen = MainScreencode;
            propprogramEntity = programEntity;
            BaseForm = baseForm;
            Privileges = privilieges;
            this.Text = /*MainScreencode + " - */"Image Upload";
            strCheckUploadMode = string.Empty;
            uploadBrowser.Visible = false;
            btnDelete.Visible = false;


            if (privilieges != null)
            {
                if (privilieges.DelPriv.Equals("true"))
                {
                    btnDelete.Visible = true;
                }
                if (privilieges.AddPriv.Equals("true") || privilieges.ChangePriv.Equals("true"))
                {
                    rdoUPloadMode.Visible = true;
                }
                else
                {
                    rdoUPloadMode.Visible = false;
                }
            }

            FillSnpDetails();
            strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
            strTempFolderName = _model.lookupDataAccess.GetReportPath() + "\\Temp\\ScannedImages\\" + strFolderName; ; //"\\\\cap-dev\\C-Drive\\CapSystemsimages\\Temp"; //"C:\\CapsystemsImages\\Temp";//
            strImageFolderName = _model.lookupDataAccess.GetReportPath() + "\\ScannedImages";// "\\\\cap-dev\\C-Drive\\CapSystemsimages";//"C:\\CapsystemsImages";//
                                                                                             // uploadControl2.UploadTempFilePath = strTempFolderName;


        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        List<CaseSnpEntity> casesnpImageEntity { get; set; }
        public ProgramDefinitionEntity propprogramEntity { get; set; }
        public string _propMainScreen { get; set; }
        private void FillSnpDetails()
        {
            dataGridCaseSnp.SelectionChanged -= new EventHandler(dataGridCaseSnp_SelectionChanged);
            dataGridCaseSnp.Rows.Clear();
            List<CommonEntity> Relation = _model.lookupDataAccess.GetRelationship();
            if (BaseForm.BaseCaseSnpEntity != null)
            {
                //CaseSnpEntity casesnpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                //List<CaseSnpEntity> caseSnpMembers = BaseForm.BaseCaseSnpEntity.FindAll(u => u.FamilySeq != BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                //int rowIndex = 0;
                //if (casesnpApplicant != null)
                //{
                //    string memberCode = string.Empty;
                //    CommonEntity rel = Relation.Find(u => u.Code.Equals(casesnpApplicant.MemberCode));
                //    if (rel != null) memberCode = rel.Desc;
                //    string name = LookupDataAccess.GetMemberName(casesnpApplicant.NameixFi, casesnpApplicant.NameixMi, casesnpApplicant.NameixLast, BaseForm.BaseHierarchyCnFormat);
                //    rowIndex = dataGridCaseSnp.Rows.Add(caseSnp.FamilySeq, name, memberCode, caseSnp.Ssno, caseSnp.M_Code);
                //    if (casesnpApplicant.Status.Trim() != "A")
                //        dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //    if (casesnpApplicant.Status.Trim() == "A")
                //        dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //    if (casesnpApplicant.Status.Trim() != "A")
                //        dataGridCaseSnp.Rows[rowIndex].Cells["MemberName"].Style.ForeColor = Color.Blue;
                //    if (casesnpApplicant.Exclude == "Y")
                //        dataGridCaseSnp.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;
                //    dataGridCaseSnp.Rows[rowIndex].Tag = casesnpApplicant;
                //}
                foreach (CaseSnpEntity caseSnp in BaseForm.BaseCaseSnpEntity)
                {
                    int rowIndex;
                    string memberCode = string.Empty;
                    CommonEntity rel = Relation.Find(u => u.Code.Equals(caseSnp.MemberCode));
                    if (rel != null) memberCode = rel.Desc;
                    string name = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, BaseForm.BaseHierarchyCnFormat);
                    string strAltDate = LookupDataAccess.Getdate(caseSnp.AltBdate);
                    //string strSsno = LookupDataAccess.GetCardNo(caseSnp.Ssno, "1", ProgramDefinition.SSNReasonFlag.Trim(), caseSnp.SsnReason);
                    rowIndex = dataGridCaseSnp.Rows.Add(caseSnp.FamilySeq, name, memberCode, caseSnp.Ssno, caseSnp.M_Code);
                    dataGridCaseSnp.Rows[rowIndex].Tag = caseSnp;
                    if (caseSnp.Status.Trim() != "A")
                        dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    if (caseSnp.M_Code == "A")
                    {
                        if (caseSnp.Status.Trim() == "A")
                            dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        if (caseSnp.Status.Trim() != "A")
                            dataGridCaseSnp.Rows[rowIndex].Cells["MemberName"].Style.ForeColor = Color.Blue;
                    }
                    if (caseSnp.Exclude == "Y")
                        dataGridCaseSnp.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;

                    CommonFunctions.setTooltip(rowIndex, caseSnp.AddOperator, caseSnp.DateAdd, caseSnp.LstcOperator, caseSnp.DateLstc, dataGridCaseSnp);
                }
                dataGridCaseSnp.Sort(dataGridCaseSnp.Columns["M_Code"], ListSortDirection.Ascending);
            }

            dataGridCaseSnp.SelectionChanged += new EventHandler(dataGridCaseSnp_SelectionChanged);
        }

        private void dataGridCaseSnp_SelectionChanged(object sender, EventArgs e)
        {
            dataGridHierchys.SelectionChanged -= new EventHandler(dataGridHierchys_SelectionChanged);
            dataGridHierchys.Rows.Clear();
            txtUploadFileName.Text = string.Empty;
            txtUploadAs.Text = string.Empty;
            if (dataGridCaseSnp.SelectedRows.Count > 0)
            {

                foreach (DataGridViewRow item in dataGridCaseSnp.Rows)
                {
                    if (item.Selected)
                    {
                        List<CaseSnpEntity> casesnpfilterEntity;
                        string strImageCout = string.Empty;
                        string strHierchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear;
                        casesnpImageEntity = _model.CaseMstData.GetCaseSnpImageUploadDetails(dataGridCaseSnp.SelectedRows[0].Cells["SSNNO"].Value.ToString(), strHierchy, BaseForm.UserID, string.Empty, string.Empty, string.Empty, BaseForm.BaseApplicationNo, item.Cells["SNP_FAMILY_SEQ"].Value.ToString(), string.Empty);
                        if (rdoUPloadMode.Checked == true)
                        {
                            casesnpfilterEntity = casesnpImageEntity.FindAll(u => u.Hierachy.Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) && u.App.Equals(BaseForm.BaseApplicationNo) && u.Year.ToString().Trim().Equals(BaseForm.BaseYear.Trim()));
                            casesnpfilterEntity = casesnpfilterEntity.OrderBy(u => u.Hierachy).ToList();
                            panel2.Enabled = true;

                        }
                        else
                        {
                            casesnpfilterEntity = casesnpImageEntity;
                            casesnpfilterEntity = casesnpfilterEntity.OrderBy(u => u.Hierachy).ToList();
                            panel2.Enabled = false;
                        }
                        foreach (CaseSnpEntity caseImage in casesnpfilterEntity)
                        {
                            string strNewFamseq = string.Empty;
                            if (chkHouseHold.Checked == true)
                            {
                                strNewFamseq = caseImage.AltFamilySeq;
                                strFullFolderName = strImageFolderName + "\\" + caseImage.Hierachy + caseImage.App + "\\" + "0000000".Substring(0, 7 - caseImage.FamilySeq.Length) + caseImage.FamilySeq;
                            }
                            else
                            {
                                strFullFolderName = strImageFolderName + "\\" + caseImage.Hierachy + caseImage.App;
                            }
                            strImageCout = string.Empty;


                            List<IMGUPLOGNEntity> imguplogEntitylist = _model.ChldMstData.GetImgUpLogList(caseImage.Agency, caseImage.Dept, caseImage.Program, caseImage.Year, caseImage.App, strNewFamseq, string.Empty, string.Empty);
                            if (imguplogEntitylist.Count > 0)
                            {
                                if (chkHouseHold.Checked)
                                    imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ != string.Empty);
                                else
                                    imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ == string.Empty);
                                strImageCout = imguplogEntitylist.Count.ToString();
                            }
                            //DirectoryInfo MyDir = new DirectoryInfo(strFullFolderName);
                            //if (MyDir.Exists)
                            //{
                            //    FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                            //    strImageCout = MyFiles.Length.ToString();
                            //}
                            //int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                            //dataGridHierchys.Rows[index].Tag = caseImage;

                            //if (propprogramEntity.DepYear != string.Empty)
                            //{
                            if (BaseForm.BaseYear.Trim() != string.Empty)
                            {
                                if (BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg == caseImage.Hierachy)
                                {
                                    //if (propprogramEntity.DepYear == caseImage.Year)
                                    //{
                                    if (BaseForm.BaseYear.Trim() == caseImage.Year.Trim())
                                    {
                                        int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                                        dataGridHierchys.Rows[index].Tag = caseImage;
                                    }
                                    //else
                                    //{
                                    //    int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                                    //    dataGridHierchys.Rows[index].Tag = caseImage;
                                    //}
                                }
                                else
                                {
                                    int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                                    dataGridHierchys.Rows[index].Tag = caseImage;
                                }
                            }
                            else
                            {
                                int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                                dataGridHierchys.Rows[index].Tag = caseImage;
                            }
                        }
                        break;
                    }
                }

            }

            if (dataGridHierchys.Rows.Count > 0)
            {
                dataGridHierchys.Rows[0].Selected = true;
            }
            dataGridHierchys.SelectionChanged += new EventHandler(dataGridHierchys_SelectionChanged);

            dataGridHierchys_SelectionChanged(sender, e);
            if (dataGridHierchys.Rows.Count == 0)
                btnDelete.Enabled = false;
        }

        public void dataGridHierchys_SelectionChanged(object sender, EventArgs e)
        {
            gvwDoclist.Rows.Clear();
            // pnlHtml.Controls.Clear();
            strDeleteEnable = string.Empty;

            if (dataGridHierchys.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in dataGridHierchys.Rows)
                {
                    if (item.Selected)
                    {
                        if (dataGridHierchys.SelectedRows[0].Tag is CaseSnpEntity)
                        {
                            CaseSnpEntity casesnpimag = dataGridHierchys.SelectedRows[0].Tag as CaseSnpEntity;
                            if (chkHouseHold.Checked == true)
                                strFullFolderName = strImageFolderName + "\\" + casesnpimag.Hierachy + casesnpimag.App + "\\" + "0000000".Substring(0, 7 - casesnpimag.FamilySeq.Length) + casesnpimag.FamilySeq;
                            else
                                strFullFolderName = strImageFolderName + "\\" + casesnpimag.Hierachy + casesnpimag.App;

                            if (casesnpimag.Hierachy + casesnpimag.App == BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo)
                            {
                                //panel2.Enabled = true;
                                strDeleteEnable = "View";
                            }
                            //else
                            //panel2.Enabled = false;

                            DisplayDocumentName(strFullFolderName, strDeleteEnable, casesnpimag.Agency, casesnpimag.Dept, casesnpimag.Program, casesnpimag.Year, casesnpimag.App, casesnpimag.AltFamilySeq);
                            break;
                        }
                    }
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(cmbImageType, null);
            if (string.IsNullOrEmpty(txtUploadFileName.Text))
            {
                _errorProvider.SetError(txtUploadFileName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblImageTypes.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtUploadFileName, null);
            }
            if (((ListItem)cmbImageType.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbImageType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblImageTypes.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbImageType, null);
            }
            if (txtUploadAs.Text.Contains(",") || txtUploadAs.Text.Contains("&") || txtUploadAs.Text.Contains("$") || txtUploadAs.Text.Contains("#") || txtUploadAs.Text.Contains("/") || txtUploadAs.Text.Contains("'")
                || txtUploadAs.Text.Contains("{") || txtUploadAs.Text.Contains("}") || txtUploadAs.Text.Contains("@") || txtUploadAs.Text.Contains("%") || txtUploadAs.Text.Contains("/") || txtUploadAs.Text.Contains("?"))
            {
                CommonFunctions.MessageBoxDisplay(@"File / Image Name should not contain special characters(, &/  # ‘ $ @ \ { } %) ? ");
                isValid = false;
            }

            return isValid;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                bool boolImageUpload = false;
                //string strFamilySeq = string.Empty;
                var Extension = txtUploadFileName.Text.Substring(txtUploadFileName.Text.LastIndexOf('.') + 1).ToLower();
                strExtensionName = Extension;
                string strUploadFolderName = string.Empty;
                string strOnlyFileName = string.Empty;
                string strUploadMainFolderName = string.Empty;
                if (Extension == "jpg" || Extension == "jpeg" || Extension == "gif" || Extension == "bmp" || Extension == "doc" || Extension == "pdf" || Extension == "xls" || Extension == "docx" || Extension == "xlsx" || Extension=="png")
                {
                    boolImageUpload = true; // Valid file type
                }
                if (boolImageUpload)
                {

                    foreach (DataGridViewRow item in dataGridCaseSnp.Rows)
                    {
                        if (item.Selected)
                        {
                            if (dataGridCaseSnp.SelectedRows[0].Tag is CaseSnpEntity)
                            {
                                CaseSnpEntity casesnpimag = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
                                if (chkHouseHold.Checked == true)
                                {
                                    string strFamilySeq = "0000000".Substring(0, 7 - casesnpimag.FamilySeq.Length) + casesnpimag.FamilySeq;
                                    strUploadFolderName = strImageFolderName + "\\" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo + "\\" + strFamilySeq;
                                    strOnlyFileName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo + "\\" + strFamilySeq;
                                }
                                else
                                {
                                    strUploadFolderName = strImageFolderName + "\\" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                                    strOnlyFileName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                                }
                                strUploadMainFolderName = strImageFolderName + "\\" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
                                break;

                            }
                        }
                    }


                    try
                    {
                        // Determine whether the directory exists.

                        if (Directory.Exists(strUploadMainFolderName))
                        {
                            if (Directory.Exists(strUploadFolderName))
                            {
                                createLogos(strUploadFolderName, strOnlyFileName);
                            }
                            else
                            {
                                DirectoryInfo di = Directory.CreateDirectory(strUploadFolderName);
                                createLogos(strUploadFolderName, strOnlyFileName);
                            }
                        }
                        else
                        {
                            // Try to create the directory.
                            DirectoryInfo di = Directory.CreateDirectory(strUploadMainFolderName);
                            if (Directory.Exists(strUploadFolderName))
                            {
                                createLogos(strUploadFolderName, strOnlyFileName);
                            }
                            else
                            {
                                DirectoryInfo disub = Directory.CreateDirectory(strUploadFolderName);
                                createLogos(strUploadFolderName, strOnlyFileName);
                            }
                        }
                        foreach (DataGridViewRow item in dataGridHierchys.Rows)
                        {
                            if (item.Cells["Hierarchy"].Value.ToString().Trim() == BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)
                            {

                                string strImageCout = string.Empty;
                                //DirectoryInfo MyDir = new DirectoryInfo(strUploadFolderName);
                                //if (MyDir.Exists)
                                //{
                                //    FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                                //    strImageCout = MyFiles.Length.ToString();
                                //}

                                string strNewFamseq = string.Empty;
                                CaseSnpEntity casesnpimag = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
                                if (chkHouseHold.Checked == true)
                                {
                                    strNewFamseq = casesnpimag.FamilySeq;
                                }

                                List<IMGUPLOGNEntity> imguplogEntitylist = _model.ChldMstData.GetImgUpLogList(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, strNewFamseq, string.Empty, string.Empty);
                                if (imguplogEntitylist.Count > 0)
                                {
                                    if (chkHouseHold.Checked)
                                        imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ != string.Empty);
                                    else
                                        imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ == string.Empty);
                                    strImageCout = imguplogEntitylist.Count.ToString();
                                }
                                item.Cells["ImagesCount"].Value = strImageCout;
                                dataGridHierchys.Rows[item.Index].Selected = true;
                                break;
                            }
                        }
                        dataGridHierchys_SelectionChanged(sender, e);
                        // DisplayDocumentName(strUploadFolderName, strDeleteEnable, BaseForm.BaseAgency , BaseForm.BaseDept , BaseForm.BaseProg ,  BaseForm.BaseYear , BaseForm.BaseApplicationNo,BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                        txtUploadFileName.Text = string.Empty;
                        txtUploadAs.Text = string.Empty;
                        cmbImageType.SelectedIndex = 0;
                        btnUpload.Enabled = false;

                    }
                    catch (Exception ex)
                    {
                        // Console.WriteLine("The process failed: {0}", ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("upload file image or doc or pdf");
                }

            }

        }

        private void createLogos(string strFPath, string strOnlyFileName)
        {

            DirectoryInfo dir1 = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            DirectoryInfo dir2 = new DirectoryInfo(strFPath);

            FileInfo[] Folder1Files = dir1.GetFiles();
            FileInfo[] Folder2Files = dir2.GetFiles();
            string[] strorgsplitFile = txtUploadFileName.Text.Split('.');
            string orginialFileName = string.Empty;
            if (strorgsplitFile.Length > 0)
                orginialFileName = strorgsplitFile[0];
            if (txtUploadAs.Text != string.Empty)
            {
                strorgsplitFile = txtUploadAs.Text.Split('.');
                if (strorgsplitFile.Length > 0)
                    orginialFileName = strorgsplitFile[0]; //((ListItem)cmbImageType.SelectedItem).Text.ToString();
            }
            if (Folder1Files.Length > 0)
            {
                foreach (FileInfo aFile in Folder1Files)
                {
                    string newFileName = orginialFileName + "." + strExtensionName;
                    string strpathToCheck = strFPath + "\\" + newFileName;

                    string tempfileName = "";
                    // Check to see if a file already exists with the
                    // same name as the file to upload.        
                    if (System.IO.File.Exists(strpathToCheck))
                    {
                        int counter = 1;
                        while (System.IO.File.Exists(strpathToCheck))
                        {
                            // if a file with this name already exists,
                            // prefix the filename with a number.
                            tempfileName = orginialFileName + counter.ToString() + "." + strExtensionName;
                            strpathToCheck = strFPath + "\\" + tempfileName;
                            counter++;
                        }

                        newFileName = tempfileName;
                    }

                    if (aFile.Name == txtUploadFileName.Text)
                    {
                        File.Move(strTempFolderName + "\\" + aFile.Name, strFPath + "\\" + newFileName);
                        InsertDeleteImgUploagLog(string.Empty, string.Empty, txtUploadFileName.Text, newFileName, string.Empty, string.Empty);
                    }

                }
            }

            else
            {
                MessageBox.Show("Upload new image");
                return;
            }


        }

        public void DisplayDocumentName(string strFolderPath, string strdeleteEnable, string strAgency, string strDept, string strProgram, string strYear, string strApplicantNo, string strFamilySeq)
        {
            gvwDoclist.Rows.Clear();
            DirectoryInfo objDir = new DirectoryInfo(strFolderPath);
            string strNewFamseq = string.Empty;
            if (chkHouseHold.Checked)
                strNewFamseq = strFamilySeq;
            List<IMGUPLOGNEntity> imguplogEntitylist = _model.ChldMstData.GetImgUpLogList(strAgency, strDept, strProgram, strYear, strApplicantNo, strNewFamseq, string.Empty, string.Empty);

            if (chkHouseHold.Checked)
                imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ != string.Empty);
            else
                imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ == string.Empty);

            //if (Directory.Exists(strFolderPath))
            //{

            //    foreach (FileInfo MyFile in objDir.GetFiles("*.*"))
            //    {
            //        var Extension = MyFile.Name.Substring(MyFile.Name.LastIndexOf('.') + 1).ToLower();
            //        if (Extension != "db")
            //        {
            //            ListViewItem listitem = new ListViewItem(new string[] { MyFile.Name });
            //            if (imguplogEntitylist.Count > 0)
            //            {
            //                IMGUPLOGNEntity imglogdata = imguplogEntitylist.Find(u => u.IMGLOG_ULoadAs.ToUpper().Contains(MyFile.Name.ToUpper()));
            //                if (imglogdata != null)
            //                {
            //                    listitem.ToolTipText = "Added By: " + imglogdata.IMGLOG_USERID + " on " + imglogdata.IMGLOG_TrnDate;
            //                }
            //            }

            //            listViewPdf.Items.Add(listitem);
            //        }
            //    }
            //}
            //if (listViewPdf.Items.Count > 0)
            //{
            //    if (strdeleteEnable == "View")
            //        btnDelete.Enabled = true;
            //    else
            //        btnDelete.Enabled = false;
            //}
            //else
            //    btnDelete.Enabled = false;



            if (imguplogEntitylist.Count > 0)
            {
                string[] strImageTypes = BaseForm.UserProfile.ImageTypes.Trim().Split(' ');
                foreach (string IMGType in strImageTypes)
                {

                    if (IMGType != string.Empty)
                    {
                        List<IMGUPLOGNEntity> imglogdata = imguplogEntitylist.FindAll(u => u.IMGLOG_SECURITY.ToUpper() == IMGType.ToUpper());
                        string strdesc = string.Empty;
                        foreach (IMGUPLOGNEntity item in imglogdata)
                        {
                            strdesc = string.Empty;
                            ListViewItem listitem = new ListViewItem();

                            CommonEntity commondesc = propImagesTypesOnly.Find(u => u.Code == item.IMGLOG_TYPE);
                            if (commondesc != null)
                                strdesc = commondesc.Desc.Trim();
                            int introwindex = gvwDoclist.Rows.Add(strdesc, item.IMGLOG_UPLoadAs.Trim(), LookupDataAccess.Getdate(item.IMGLOG_DATE_UPLOAD.Trim()), item.IMGLOG_SECURITY.Trim(), item.IMGLOG_ID.Trim(), Convert.ToDateTime(item.IMGLOG_DATE_UPLOAD.Trim()), item.IMGLOG_TYPE.ToString());

                            CommonFunctions.setTooltip(introwindex, item.IMGLOG_UPLOAD_BY, item.IMGLOG_DATE_UPLOAD, item.IMGLOG_LSTC_OPERATOR, item.IMGLOG_DATE_LSTC, gvwDoclist);


                        }
                    }


                }
            }
            if (gvwDoclist.Rows.Count > 0)
            {
                gvwDoclist.Rows[0].Selected = true;
                gvwDoclist.Sort(gvwDoclist.Columns["gvtdocdate1"], ListSortDirection.Descending);
                btnPreview.Enabled = true;
                if (strdeleteEnable == "View")
                    btnDelete.Enabled = true;
                else
                    btnDelete.Enabled = false;
            }
            else
            {
                btnPreview.Enabled = false;
                btnDelete.Enabled = false;
            }


        }

        string strsort = "ASC";
        private void gvwData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == gvtdocDate.Index)
            {
                if (strsort == "ASC")
                {
                    strsort = "DESC";
                    gvwDoclist.Sort(gvwDoclist.Columns["gvtdocdate1"], ListSortDirection.Ascending);

                }
                else
                {
                    strsort = "ASC";
                    gvwDoclist.Sort(gvwDoclist.Columns["gvtdocdate1"], ListSortDirection.Descending);
                }
            }
        }


        public static void setImgTooltip(int rowIndex, string updateOperator, string updateDate, DataGridView datagridview)
        {

            string toolTipText = "Uploaded by " + updateOperator + " on " + updateDate;

            foreach (DataGridViewCell cell in datagridview.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

            DirectoryInfo MyDir = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    MyFile.Delete();
                }
            }

            OpenFileDialog objOFD = sender as OpenFileDialog;
            objUpload = objOFD;
            if (objOFD == null)
            {
                return;
            }

            if (objOFD.FileNames.Length == 0)
            {
                return;
            }

            string value = objOFD.FileName;
            string strfileName = Path.GetFileName(value);

            txtUploadFileName.Text = strfileName;

            string strModifyfilename = strfileName;

            strModifyfilename = strModifyfilename.Replace(',', '_').Replace('&', '_').Replace('$', '_').Replace('#', '_').Replace('/', '_').Replace("'", "_").Replace('{', '_').Replace('}', '_').Replace('@', '_').Replace('%', '_').Replace('/', '_').Replace('?', '_');
            txtUploadAs.Text = strModifyfilename;

            if (txtUploadFileName.TextLength > 0)
                btnUpload.Enabled = true;
            else
                btnUpload.Enabled = true;

            //string ImageTempFile = strTempFolderName + "\\" + txtUploadFileName.Text;

            var Extension = txtUploadFileName.Text.Substring(txtUploadFileName.Text.LastIndexOf('.') + 1).ToLower();
            string ImageTempFile = strTempFolderName + "\\Certificates." + Extension;

            var objFile = objOFD.GetRoot(objOFD.FileName).OpenFileStream(value, FileMode.Open, FileAccess.Read);
            int nFileLen = (int)objFile.Length;
            if (nFileLen == 0)
            {
                // lblOutput.Text = "No file was uploaded.";
                return;
            }

            byte[] myData = new Byte[nFileLen];
            objFile.Read(myData, 0, nFileLen);

            //string sFilename = System.IO.Path.GetFileName(objFile.FileName);

            System.IO.FileStream newFile = new FileStream(ImageTempFile, FileMode.Create);

            newFile.Write(myData, 0, myData.Length);
            newFile.Close();
        }
        List<CommonEntity> propImagesTypesOnly = new List<CommonEntity>();
        private void GetImageUpload()
        {
            cmbImageType.Items.Clear();
            List<CommonEntity> ImageNameConvention = _model.lookupDataAccess.GetImageNameConvention();
            ImageNameConvention = filterByHIE(ImageNameConvention);
            List<CommonEntity> ImagesNamesTypes;

            //    string strImages = "CM ";
            if (!string.IsNullOrEmpty(BaseForm.UserProfile.ImageTypes.Trim()))
            {
                string[] strImageTypes = BaseForm.UserProfile.ImageTypes.Trim().Split(' ');//strImages.Split(' '); //
                bool boolAddExclude = false;
                foreach (string incomeType in strImageTypes)
                {
                    ImagesNamesTypes = ImageNameConvention.FindAll(u => u.Extension.Trim().ToUpper() == incomeType.Trim().ToUpper());
                    foreach (CommonEntity item in ImagesNamesTypes)
                    {
                        propImagesTypesOnly.Add(new CommonEntity(item.Code, item.Desc, item.Hierarchy, item.Extension, string.Empty, string.Empty));
                    }
                }
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("No Image Types assigned in Admn0005  \n Contact System Administrator");
            }
            cmbImageType.Items.Insert(0, new ListItem("Select One", "0"));
            cmbImageType.ColorMember = "FavoriteColor";
            cmbImageType.SelectedIndex = 0;
            propImagesTypesOnly = propImagesTypesOnly.OrderBy(u => u.Desc).ToList();
            foreach (CommonEntity imageName in propImagesTypesOnly)
            {
                ListItem li = new ListItem(imageName.Desc, imageName.Code, imageName.Extension, imageName.Code);
                cmbImageType.Items.Add(li);
                //if (imageName.Default.Equals("Y")) cmbImageType.SelectedItem = li;
            }
            if (ImageNameConvention.Count == 0)
                CommonFunctions.MessageBoxDisplay("Table 000026-2-3-8 not defined for useing Imaging Module \n Contact System Administrator");

        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();
            return LookupValues;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string strDeleteFile = "";
            if (gvwDoclist.Rows.Count > 0)
            {
                strDeleteFile = gvwDoclist.SelectedRows[0].Cells["gvtAssigndoc"].Value.ToString();
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nFile " + strDeleteFile, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strPreviewFile = "";

            if (gvwDoclist.Rows.Count > 0)
            {
                strPreviewFile = gvwDoclist.SelectedRows[0].Cells["gvtAssigndoc"].Value.ToString();
                string strFolderPath = strFullFolderName;
                int Count = strPreviewFile.Length;
                string strExtension = (strPreviewFile.Substring(Count - 4, 4));
                if ((strExtension.ToUpper() == ".DOC") || (strExtension.ToUpper() == ".XLS") || (strExtension.ToUpper() == "DOCX") || (strExtension.ToUpper() == "XLSX"))
                {
                    //CommonFunctions.MessageBoxDisplay("you can't preview this file.Please wait downloaded this file");
                    //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                    //downloadGateway.Filename = strPreviewFile;

                    //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                    //downloadGateway.StartFileDownload(new ContainerControl(), strFolderPath + "\\" + strPreviewFile);

                    FileInfo fiDownload = new FileInfo(strFolderPath + "\\" + strPreviewFile);
                    /// Need to check for file exists, is local file, is allow to read, etc...
                    string name = fiDownload.Name;
                    using (FileStream fileStream = fiDownload.OpenRead())
                    {
                        Application.Download(fileStream, name);
                    }
                }
                else
                {

                    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                    {

                        PdfViewerNewForm objfrm = new PdfViewerNewForm(strFolderPath + "\\" + strPreviewFile);
                        objfrm.StartPosition = FormStartPosition.CenterScreen;
                        objfrm.ShowDialog();
                    }
                    else
                    {
                        FrmViewer objfrm = new FrmViewer(strFolderPath + "\\" + strPreviewFile);
                        objfrm.StartPosition = FormStartPosition.CenterScreen;

                        objfrm.ShowDialog();
                    }
                }
            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            string strOnlyFile = string.Empty;
            string strDeleteFile = "";

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in gvwDoclist.Rows)
                {
                    if (item.Selected)
                    {
                        if (dataGridHierchys.SelectedRows[0].Tag is CaseSnpEntity)
                        {
                            strDeleteFile = item.Cells["gvtAssigndoc"].Value == null ? string.Empty : item.Cells["gvtAssigndoc"].Value.ToString();

                            CaseSnpEntity casesnpimag = dataGridHierchys.SelectedRows[0].Tag as CaseSnpEntity;
                            if (chkHouseHold.Checked == true)
                            {
                                strFullFolderName = strImageFolderName + "\\" + casesnpimag.Hierachy + casesnpimag.App + "\\" + "0000000".Substring(0, 7 - casesnpimag.FamilySeq.Length) + casesnpimag.FamilySeq;
                                strOnlyFile = casesnpimag.Hierachy + casesnpimag.App + "\\" + "0000000".Substring(0, 7 - casesnpimag.FamilySeq.Length) + casesnpimag.FamilySeq;
                            }
                            else
                            {
                                strFullFolderName = strImageFolderName + "\\" + casesnpimag.Hierachy + casesnpimag.App;
                                strOnlyFile = casesnpimag.Hierachy + casesnpimag.App;
                            }
                            strMainFolderName = strImageFolderName + "\\" + casesnpimag.Hierachy + casesnpimag.App;

                            MyDir = new DirectoryInfo(strFullFolderName);
                            FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                            foreach (FileInfo MyFile in MyFiles)
                            {
                                if (MyFile.Exists)
                                {
                                    if (strDeleteFile == MyFile.Name)
                                    {
                                        System.GC.Collect();
                                        System.GC.WaitForPendingFinalizers();
                                        MyFile.Delete();
                                        string strId = item.Cells["gvtdocId"].Value.ToString();
                                        InsertDeleteImgUploagLog(strId, "DELETE", strDeleteFile, strDeleteFile, string.Empty, string.Empty);// strOnlyFile + "\\" + 
                                    }
                                }
                            }

                            DisplayDocumentName(strFullFolderName, strDeleteEnable, casesnpimag.Agency, casesnpimag.Dept, casesnpimag.Program, casesnpimag.Year, casesnpimag.App, casesnpimag.AltFamilySeq);
                            string strImageCout = string.Empty;
                            //MyDir = new DirectoryInfo(strFullFolderName);
                            //if (MyDir.Exists)
                            //{
                            //    FileInfo[] MyFiles1 = MyDir.GetFiles("*.*");
                            //    strImageCout = MyFiles1.Length.ToString();
                            //}
                            string strNewFamseq = string.Empty;
                            if (chkHouseHold.Checked == true)
                            {
                                strNewFamseq = casesnpimag.FamilySeq;
                            }

                            List<IMGUPLOGNEntity> imguplogEntitylist = _model.ChldMstData.GetImgUpLogList(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, strNewFamseq, string.Empty, string.Empty);
                            if (imguplogEntitylist.Count > 0)
                            {
                                if (chkHouseHold.Checked)
                                    imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ != string.Empty);
                                else
                                    imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ == string.Empty);
                                strImageCout = imguplogEntitylist.Count.ToString();
                            }

                            dataGridHierchys.SelectedRows[0].Cells["ImagesCount"].Value = strImageCout;

                            break;
                        }
                    }
                }

            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Related Files(*.BMP;*.JPG;*.GIF;*.DOC;*.DOCX;*.XLS;*.XLSX;*.PDF;*.PNG)|*.BMP;*.JPG;*.GIF;*.DOC;*.DOCX;*.XLS;*.XLSX;*.PDF;*.PNG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowDialog(this);
        }

        private void listViewPdf_SelectedIndexChanged(object sender, EventArgs e)
        {
            //pnlHtml.Controls.Clear();
            //if (listViewPdf.Items.Count > 0)
            //{
            //    var selectedItems = listViewPdf.SelectedItems;
            //    if (selectedItems.Count > 0)
            //    {
            //        string strFileName = listViewPdf.Items[listViewPdf.SelectedIndex].SubItems[0].Text.ToString();
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
            //            PreviewControl priview = new PreviewControl(strFullFolderName + "\\" + strFileName, "ImageView");
            //            priview.Dock = Wisej.Web.DockStyle.Fill;
            //            pnlHtml.Controls.Add(priview);
            //        }

            //    }

            //}
        }

        private void ImageUpload_Load(object sender, EventArgs e)
        {
            GetImageUpload();
            dataGridCaseSnp.Sort(dataGridCaseSnp.Columns["M_Code"], ListSortDirection.Ascending);
            if (dataGridCaseSnp.Rows.Count > 0) dataGridCaseSnp.Rows[0].Selected = true;
            dataGridCaseSnp_SelectionChanged(sender, e);
        }

        private void chkHouseHold_CheckedChanged(object sender, EventArgs e)
        {
            dataGridCaseSnp_SelectionChanged(sender, e);
        }

        private void rdoUPloadMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoUPloadMode.Checked)
            {
                btnUpload.Visible = uploadBrowser.Visible = true;
                FillHierachyTypesList();
                if (dataGridHierchys.Rows.Count > 0)
                {
                    dataGridHierchys.Focus();
                    dataGridHierchys.Rows[0].Selected = true;
                }


            }
            else
            {
                btnUpload.Visible = uploadBrowser.Visible = false;
                if (dataGridHierchys.Rows.Count > 0)
                {
                    dataGridHierchys.Focus();
                    dataGridHierchys.Rows[0].Selected = true;
                }
            }
        }


        public void FillHierachyTypesList()
        {
            dataGridHierchys.SelectionChanged -= new EventHandler(dataGridHierchys_SelectionChanged);
            dataGridHierchys.Rows.Clear();
            txtUploadFileName.Text = string.Empty;
            txtUploadAs.Text = string.Empty;
            if (dataGridCaseSnp.SelectedRows.Count > 0)
            {

                foreach (DataGridViewRow item in dataGridCaseSnp.Rows)
                {
                    if (item.Selected)
                    {
                        List<CaseSnpEntity> casesnpfilterEntity;
                        string strImageCout = string.Empty;
                        string strHierchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear;
                        casesnpImageEntity = _model.CaseMstData.GetCaseSnpImageUploadDetails(dataGridCaseSnp.SelectedRows[0].Cells["SSNNO"].Value.ToString(), strHierchy, BaseForm.UserID, string.Empty, string.Empty, string.Empty, BaseForm.BaseApplicationNo, item.Cells["SNP_FAMILY_SEQ"].Value.ToString(), string.Empty);

                        if (rdoUPloadMode.Checked == true)
                        {
                            casesnpfilterEntity = casesnpImageEntity.FindAll(u => u.Hierachy.Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) && u.App.Equals(BaseForm.BaseApplicationNo) && u.Year.ToString().Trim().Equals(BaseForm.BaseYear.Trim()));
                            casesnpfilterEntity = casesnpfilterEntity.OrderBy(u => u.Hierachy).ToList();
                            panel2.Enabled = true;
                            btnUpload.Visible = uploadBrowser.Visible = true;

                        }
                        else
                        {
                            casesnpfilterEntity = casesnpImageEntity;
                            casesnpfilterEntity = casesnpfilterEntity.OrderBy(u => u.Hierachy).ToList();
                            panel2.Enabled = false;
                            uploadBrowser.Visible = false;
                            btnUpload.Visible = false;
                            btnUpload.Enabled = false;
                        }
                        // int index =0;
                        string strNewFamseq = string.Empty;
                        foreach (CaseSnpEntity caseImage in casesnpfilterEntity)
                        {
                            strNewFamseq = string.Empty;
                            if (chkHouseHold.Checked == true)
                            {
                                strNewFamseq = caseImage.AltFamilySeq;
                                strFullFolderName = strImageFolderName + "\\" + caseImage.Hierachy + caseImage.App + "\\" + "0000000".Substring(0, 7 - caseImage.FamilySeq.Length) + caseImage.FamilySeq;
                            }
                            else
                                strFullFolderName = strImageFolderName + "\\" + caseImage.Hierachy + caseImage.App;
                            strImageCout = string.Empty;


                            List<IMGUPLOGNEntity> imguplogEntitylist = _model.ChldMstData.GetImgUpLogList(caseImage.Agency, caseImage.Dept, caseImage.Program, caseImage.Year, caseImage.App, strNewFamseq, string.Empty, string.Empty);
                            if (imguplogEntitylist.Count > 0)
                            {
                                if (chkHouseHold.Checked)
                                    imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ != string.Empty);
                                else
                                    imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY == string.Empty && u.IMGLOG_FAMILY_SEQ == string.Empty);
                                strImageCout = imguplogEntitylist.Count.ToString();
                            }

                            //DirectoryInfo MyDir = new DirectoryInfo(strFullFolderName);
                            //if (MyDir.Exists)
                            //{
                            //    FileInfo[] MyFiles = MyDir.GetFiles("*.*");
                            //    strImageCout = MyFiles.Length.ToString();
                            //}
                            //if (propprogramEntity.DepYear != string.Empty)
                            //{
                            if (BaseForm.BaseYear.Trim() != string.Empty)
                            {
                                if (BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg == caseImage.Hierachy && BaseForm.BaseApplicationNo == caseImage.App)
                                {
                                    //if (propprogramEntity.DepYear == caseImage.Year)
                                    //{
                                    if (BaseForm.BaseYear.Trim() == caseImage.Year.Trim())
                                    {
                                        int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                                        dataGridHierchys.Rows[index].Tag = caseImage;
                                    }
                                }
                                else
                                {
                                    int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                                    dataGridHierchys.Rows[index].Tag = caseImage;
                                }
                            }
                            else
                            {
                                int index = dataGridHierchys.Rows.Add(caseImage.Hierachy, caseImage.Year, caseImage.App, caseImage.ProgrameName, strImageCout == "0" ? "" : strImageCout, caseImage.HierachyStatus, caseImage.FamilySeq);
                                dataGridHierchys.Rows[index].Tag = caseImage;
                            }

                        }
                    }
                }

            }

            dataGridHierchys.SelectionChanged += new EventHandler(dataGridHierchys_SelectionChanged);
            dataGridHierchys_SelectionChanged(dataGridHierchys, new EventArgs());
            if (dataGridHierchys.Rows.Count == 0)
                btnDelete.Enabled = false;

        }

        private void rdoViewMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoViewMode.Checked)
            {
                FillHierachyTypesList();
            }
            else
            { }
        }


        private void InsertDeleteImgUploagLog(string strIMGId, string strOpertype, string strimgFileName, string strimgloadas, string strsecuirtyType, string strImageType)
        {
            //IMGUPLOGEntity imglogentity = new IMGUPLOGEntity();
            //imglogentity.IMGLOG_USERID = BaseForm.UserID;
            //imglogentity.IMGLOG_APP = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo;
            //imglogentity.IMGLOG_SCREEN =  Privileges.Program;
            //imglogentity.IMGLOG_OPERATN = strOpertype;
            //imglogentity.IMGLOG_FNAME = strimgFileName;
            //imglogentity.IMGLOG_ULoadAs = strimgloadas;
            //_model.ChldMstData.InsertIMGUPLOG(imglogentity);
            IMGUPLOGNEntity imglogentity = new IMGUPLOGNEntity();
            imglogentity.IMGLOG_ID = strIMGId;
            imglogentity.IMGLOG_AGY = BaseForm.BaseAgency;
            imglogentity.IMGLOG_DEP = BaseForm.BaseDept;
            imglogentity.IMGLOG_PROG = BaseForm.BaseProg;
            imglogentity.IMGLOG_YEAR = BaseForm.BaseYear;
            imglogentity.IMGLOG_APP = BaseForm.BaseApplicationNo;
            CaseSnpEntity casesnpimag = dataGridHierchys.SelectedRows[0].Tag as CaseSnpEntity;
            if (chkHouseHold.Checked == true)
            {
                imglogentity.IMGLOG_FAMILY_SEQ = casesnpimag.FamilySeq;
            }
            else
                imglogentity.IMGLOG_FAMILY_SEQ = string.Empty;
            imglogentity.IMGLOG_SCREEN = _propMainScreen;

            if (strOpertype == string.Empty)
            {
                imglogentity.IMGLOG_SECURITY = ((ListItem)cmbImageType.SelectedItem).ID.ToString();
                imglogentity.IMGLOG_TYPE = ((ListItem)cmbImageType.SelectedItem).Value.ToString();
            }
            else
            {
                imglogentity.IMGLOG_SECURITY = strsecuirtyType;
                imglogentity.IMGLOG_TYPE = strImageType;
            }
            imglogentity.IMGLOG_UPLoadAs = strimgloadas;
            imglogentity.IMGLOG_UPLOAD_BY = BaseForm.UserID;
            imglogentity.IMGLOG_ORIG_FILENAME = strimgFileName;
            imglogentity.MODE = strOpertype;
            _model.ChldMstData.InsertIMGUPLOG(imglogentity);

        }

        private void uploadBrowser_Uploaded(object sender, UploadedEventArgs e)
        {
            DirectoryInfo MyDir = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    MyFile.Delete();
                }
            }


            //string value = uploadControl2.Name;
            //string[] lines = value.Split('\\');
            //string strfileName = string.Empty;
            //foreach (string line in lines)
            //{
            //    strfileName = line;
            //}
            //txtUploadFileName.Text = strfileName;

            if (txtUploadFileName.TextLength > 0)
                btnUpload.Enabled = true;
            else
                btnUpload.Enabled = true;

            ////string ImageTempFile = strTempFolderName + "\\" + txtUploadFileName.Text;
            //UploadControl objOFD = sender as UploadControl;
            //uploadControl2 = objOFD;
            //if (objOFD == null)
            //{
            //    return;
            //}
            //Gizmox.WebGUI.Common.Resources.FileHandle objFile = null;
            //if (objOFD.[0] == null || !(objOFD.Files[0] is Gizmox.WebGUI.Common.Resources.FileHandle))
            //{
            //    return;
            //}
            //  objFile = objOFD.Files[0] as Gizmox.WebGUI.Common.Resources.FileHandle;


            var mobjResult = e.Files[0];
            // Adds record to the list about uploaded file
            //mobjResult.Name, mobjResult.TempFileFullName, mobjResult.Type, mobjResult.Size;


            //string strTempFileName = mobjResult.TempFileFullName; // TODO: WIsej;
            string strName = mobjResult.FileName;
            string strType = mobjResult.ContentType;
            long strr = mobjResult.ContentLength;
            //var Extension = "pdf"; //txtUploadFileName.Text.Substring(txtUploadFileName.Text.LastIndexOf('.') + 1).ToLower();
            //string ImageTempFile = strTempFolderName + "\\Certificates." + Extension;

            //int nFileLen = (int)mobjResult.Size;
            //if (nFileLen == 0)
            //{
            //    // lblOutput.Text = "No file was uploaded.";
            //    return;
            //}

            //byte[] myData = new Byte[nFileLen];
            //// objFile.InputStream.Read(myData, 0, nFileLen);

            ////string sFilename = System.IO.Path.GetFileName(objFile.FileName);
            //// string sFilename = objFile.FileNam
            //// int file_append = 0;

            ////.
            //System.IO.FileStream newFile = new FileStream(ImageTempFile, FileMode.Create);

            //newFile.Write(myData, 0, myData.Length);
            //newFile.Close();
            txtUploadFileName.Text = mobjResult.FileName;
            string strModifyfilename = mobjResult.FileName;
            strModifyfilename = strModifyfilename.Replace(',', '_').Replace('&', '_').Replace('$', '_').Replace('#', '_').Replace('/', '_').Replace("'", "_").Replace('{', '_').Replace('}', '_').Replace('@', '_').Replace('%', '_').Replace('/', '_').Replace('?', '_');
            txtUploadAs.Text = strModifyfilename;
            //File.Move(mobjResult.FileName, strTempFolderName + "\\" + mobjResult.FileName); //Alaa: This one here always throws an exception, delete this line if it's the right way
            mobjResult.SaveAs(strTempFolderName + "\\" + strName);
        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        private void gvwDoclist_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (gvwDoclist.Rows.Count > 0)
            {
                if (objArgs.MenuItem.Tag is CommonEntity)
                {
                    CommonEntity dr = (CommonEntity)objArgs.MenuItem.Tag as CommonEntity;
                    if (dr != null)
                    {

                        string strId = gvwDoclist.SelectedRows[0].Cells["gvtdocId"].Value.ToString();
                        InsertDeleteImgUploagLog(strId, "UPDATE", string.Empty, string.Empty, dr.Extension, dr.Code);// strOnlyFile + "\\" + 
                    }

                }
            }
        }

        private void contextmenuTypes_Popup(object sender, EventArgs e)
        {
            if (gvwDoclist.Rows.Count > 0)
            {
                contextmenuTypes.MenuItems.Clear();
                foreach (CommonEntity imageName in propImagesTypesOnly)
                {

                    MenuItem menuItem = new MenuItem();

                    menuItem.Text = imageName.Desc;
                    menuItem.Tag = imageName;
                    contextmenuTypes.MenuItems.Add(menuItem);
                    string strcode = gvwDoclist.SelectedRows[0].Cells["gvtimgType"].Value != null ? gvwDoclist.SelectedRows[0].Cells["gvtimgType"].Value.ToString() : string.Empty;

                    if (imageName.Code.Trim() == strcode.ToString().Trim())
                    {
                        menuItem.Checked = true;
                    }
                }
            }
            else
            {
                contextmenuTypes.MenuItems.Clear();
            }
        }

        private void ImageUpload_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }
    }

}