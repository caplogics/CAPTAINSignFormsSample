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
using Captain.Common.Model.Objects;
using System.IO;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

#endregion


namespace Captain.Common.Views.Forms
{
    public partial class PIP00001DocumentViewForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
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
        public PIP00001DocumentViewForm(BaseForm baseForm, PrivilegeEntity privilieges, CaseSnpEntity snpentity, string strRegid, List<PIPDocEntity> _docentity, string _strPipId, List<CommonEntity> ImagesTypesOnly)
        {

            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseForm;
            Privileges = privilieges;
            this.Text = /*Privileges.Program + " - */"Drag Documents to CAPTAIN";
            strCheckUploadMode = string.Empty;
           
            _propSnpEntity = snpentity;
            _propPIP_REG_ID = strRegid;
            _propAgency = _propSnpEntity.Agency;
            _propDept = _propSnpEntity.Dept;
            _propProg = _propSnpEntity.Program;
            _propYear = _propSnpEntity.Year;
            _propApplNo = _propSnpEntity.App;
            _propDocentitylist = _docentity;
            _propPipId = _strPipId;
            //btnDragged.Visible = false;
            // propimagetypesCategory = _model.lookupDataAccess.GetImageNameConvention();
            strFolderName = _propSnpEntity.Agency + _propSnpEntity.Dept + _propSnpEntity.Program + _propSnpEntity.App;
            strImageFolderName = _model.lookupDataAccess.GetReportPath() + "\\ScannedImages";
            _propImagesTypesOnly = ImagesTypesOnly;
            if (Consts.Common.ServerLocation == string.Empty)
            {
                _propPIPIMGDOCPATH = @"D:\CABA-REPORTS\ScannedImages\01010400000003\0000001\";// @"\\CAPSYSAZ8\PIP_DOCS\" + BaseForm.BaseAgencyControlDetails.AgyShortName + @"00\DOCUPLDS\";

            }
            else
            {
                //_propPIPIMGDOCPATH = @"E:\\PIP_DOCS\";
                _propPIPIMGDOCPATH = @"\\CAPSYSAZ8\PIP_DOCS\";
            }

            PipdocumentFiles();
            CaptaindocumentFiles();

        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        List<CaseSnpEntity> casesnpImageEntity { get; set; }
        private string _propPIPIMGDOCPATH { get; set; }
        // List<CommonEntity> propimagetypesCategory { get; set; }
        public string _propAgency { get; set; }
        public string _propDept { get; set; }
        public string _propProg { get; set; }
        public string _propYear { get; set; }
        public string _propApplNo { get; set; }
        public List<PIPDocEntity> _propDocentitylist { get; set; }
        public string _propPipId { get; set; }       
        public string _propPIP_REG_ID { get; set; }
        public CaseSnpEntity _propSnpEntity { get; set; }
        List<CommonEntity> _propImagesTypesOnly = new List<CommonEntity>();
        void PipdocumentFiles()
        {
            string strDocument;
            string strImgType;
            string strSecurity;
            gvwPIPFiles.Rows.Clear();
            List<PIPDocEntity> pipdoclist = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == _propPipId.ToString() && u.PIPDOCUPLD_VERIFIED_STAT == "C" && u.PIPDOCUPLD_DRAGD_BY == string.Empty);
            foreach (PIPDocEntity item in pipdoclist)
            {

                strDocument = strImgType = strSecurity = string.Empty;

                CommonEntity commimgdoc = _propImagesTypesOnly.Find(u => u.Code.Trim() == item.PIPDOCUPLD_DOCTYPE.Trim());
                if (commimgdoc != null)
                {
                    strDocument = commimgdoc.Desc;
                    strSecurity = commimgdoc.Extension;
                    strImgType = commimgdoc.Code;

                    int intRowdex = gvwPIPFiles.Rows.Add(false, strDocument, item.PIPDOCUPLD_DOCNAME, strSecurity, strImgType);
                    gvwPIPFiles.Rows[intRowdex].Tag = item;
                }
            }
            if (gvwPIPFiles.Rows.Count > 0)
                btnUpload.Visible = true;
            else
                btnUpload.Visible = false;
        }

        void CaptaindocumentFiles()
        {

            if (_propSnpEntity != null)
                if (chkHouseHold.Checked == true)
                    strFullFolderName = strImageFolderName + "\\" + _propSnpEntity.Agency + _propSnpEntity.Dept + _propSnpEntity.Program + _propSnpEntity.App + "\\" + "0000000".Substring(0, 7 - _propSnpEntity.FamilySeq.Length) + _propSnpEntity.FamilySeq;
                else
                    strFullFolderName = strImageFolderName + "\\" + _propSnpEntity.Agency + _propSnpEntity.Dept + _propSnpEntity.Program + _propSnpEntity.App;


            DisplayDocumentName(strFullFolderName, _propSnpEntity.Agency , _propSnpEntity.Dept , _propSnpEntity.Program , _propSnpEntity.Year.Trim()  , _propSnpEntity.App, _propSnpEntity.FamilySeq);

        }

        public void DisplayDocumentName(string strFolderPath, string strAgency, string strDept, string strProgram, string strYear, string strApplicantNo, string strFamilySeq)
        {
            gvwCaptaindoc.Rows.Clear();
            DirectoryInfo objDir = new DirectoryInfo(strFolderPath);
            List<IMGUPLOGNEntity> imguplogEntitylist = _model.ChldMstData.GetImgUpLogList(strAgency,strDept,strProgram,strYear,strApplicantNo,strFamilySeq,string.Empty,string.Empty);
            imguplogEntitylist = imguplogEntitylist.FindAll(u => u.IMGLOG_DELETED_BY != string.Empty);
            if (Directory.Exists(strFolderPath))
            {

                foreach (FileInfo MyFile in objDir.GetFiles("*.*"))
                {
                    var Extension = MyFile.Name.Substring(MyFile.Name.LastIndexOf('.') + 1).ToLower();
                    if (Extension != "db")
                    {
                        int introwindex = gvwCaptaindoc.Rows.Add(MyFile.Name);
                        //gvwCaptaindoc.Rows[introwindex].ce

                        if (imguplogEntitylist.Count > 0)
                        {
                            IMGUPLOGNEntity imglogdata = imguplogEntitylist.Find(u => u.IMGLOG_UPLoadAs.ToUpper().Contains(MyFile.Name.ToUpper()));
                            if (imglogdata != null)
                            {
                                //listitem.ToolTipText = "Added By: " + imglogdata.IMGLOG_USERID + " on " + imglogdata.IMGLOG_TrnDate;
                            }
                        }

                    }
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHouseHold_CheckedChanged(object sender, EventArgs e)
        {
            CaptaindocumentFiles();
        }
        public string  _propFormStatus = string.Empty;
        private void btnUpload_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> SelectedgvRows = (from c in gvwPIPFiles.Rows.Cast<DataGridViewRow>().ToList()
                                                    where (((DataGridViewCheckBoxCell)c.Cells["gvchkSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                    select c).ToList();
            if (SelectedgvRows.Count > 0)
            {

                using (NetworkShareAccesser.Access("CAPSYSAZ8", "PIP_Docs", "ApplicationUser", "7!Enough32Distract$"))
                {
                    _propFormStatus = "Upload";
                    strFullFolderName = strImageFolderName + "\\" + _propSnpEntity.Agency + _propSnpEntity.Dept + _propSnpEntity.Program + _propSnpEntity.App + "\\" + "0000000".Substring(0, 7 - _propSnpEntity.FamilySeq.Length) + _propSnpEntity.FamilySeq;

                    string strFilterAgy = "00";
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        strFilterAgy = _propSnpEntity.Agency;
                    string strAz8Filename = string.Empty;// _propPIPIMGDOCPATH + BaseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + _propConformationNum + "\\" + strFileName;
                    foreach (DataGridViewRow gvritem in SelectedgvRows)
                    {
                        string strFileName = gvritem.Cells["gvtPIPFilename"].Value.ToString();
                        strAz8Filename = _propPIPIMGDOCPATH + BaseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + _propPIP_REG_ID;
                        PIPDocEntity pipdocentity = gvritem.Tag as PIPDocEntity;
                        UploadDocument(strFileName, strAz8Filename, pipdocentity.PIPDOCUPLD_ID, gvritem.Cells["gvtSecurity"].Value.ToString(), gvritem.Cells["gvtIMGTYPE"].Value.ToString());
                    }
                    _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propPIP_REG_ID,  "REGID");

                    PipdocumentFiles();
                    CaptaindocumentFiles();
                }
                AlertBox.Show("Moved Successfully");
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please select atleast one");
            }
        }
        private void UploadDocument(string strorginialFileName, string strAz8FileLocation, string strPIPUPLOADID, string strSecurity, string strImgType)
        {

            try
            {


                DirectoryInfo dir1 = new DirectoryInfo(strAz8FileLocation);

                string strMainFolderName = strImageFolderName + "\\" + _propSnpEntity.Agency + _propSnpEntity.Dept + _propSnpEntity.Program + _propSnpEntity.App;

                strFullFolderName = strImageFolderName + "\\" + _propSnpEntity.Agency + _propSnpEntity.Dept + _propSnpEntity.Program + _propSnpEntity.App + "\\" + "0000000".Substring(0, 7 - _propSnpEntity.FamilySeq.Length) + _propSnpEntity.FamilySeq;


                //DirectoryInfo dir2 = new DirectoryInfo(strFullFolderName);

                FileInfo[] Folder1Files = dir1.GetFiles();
                //FileInfo[] Folder2Files = dir2.GetFiles();
                var Extension = strorginialFileName.Substring(strorginialFileName.LastIndexOf('.') + 1).ToLower();
                strExtensionName = Extension;
                string[] strFileName = strorginialFileName.Split('.');
                string orginialFileName = strFileName[0];

                if (Folder1Files.Length > 0)
                {

                    foreach (FileInfo aFile in Folder1Files)
                    {
                        string newFileName = orginialFileName + "." + strExtensionName;
                        string strpathToCheck = strFullFolderName + "\\" + newFileName;

                        string tempfileName = "";
                        if (aFile.Name == strorginialFileName)
                        {
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
                                    strpathToCheck = strFullFolderName + "\\" + tempfileName;
                                    counter++;
                                }

                                newFileName = tempfileName;
                            }

                            var local = Path.Combine(strFullFolderName, newFileName);
                            var remote = Path.Combine(strAz8FileLocation, strorginialFileName);


                            // Try to create the directory.
                            if (!Directory.Exists(strMainFolderName))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(strMainFolderName);
                                if (!Directory.Exists(strFullFolderName))
                                {
                                    DirectoryInfo disub = Directory.CreateDirectory(strFullFolderName);

                                }

                            }
                            else
                            {
                                if (!Directory.Exists(strFullFolderName))
                                {
                                    DirectoryInfo disub = Directory.CreateDirectory(strFullFolderName);

                                }
                            }


                            File.Copy(remote, local, true);
                            File.Delete(remote);

                            PIPDATA.InsertUpdatePipDoc(BaseForm.BaseLeanDataBaseConnectionString, strPIPUPLOADID, string.Empty, BaseForm.UserID, newFileName, "UPDATEDRAG");
                            InsertDeleteImgUploagLog("U", strSecurity, strImgType, strorginialFileName, newFileName);// (_propSnpEntity.Agency + _propSnpEntity.Dept + _propSnpEntity.Program + _propSnpEntity.App + "\\" + "0000000".Substring(0, 7 - _propSnpEntity.FamilySeq.Length) + _propSnpEntity.FamilySeq) + "\\"

                        }


                    }
                }

            }
            catch (Exception ex)
            {

                CommonFunctions.MessageBoxDisplay(ex.Message);
            }
        }

       


        private void InsertDeleteImgUploagLog(string strOpertype, string strSecuritytype, string strType, string strimgFileName, string strimgloadas)
        {
            IMGUPLOGNEntity imglogentity = new IMGUPLOGNEntity();
            imglogentity.IMGLOG_AGY = _propAgency;
            imglogentity.IMGLOG_DEP = _propDept;
            imglogentity.IMGLOG_PROG = _propProg;
            imglogentity.IMGLOG_YEAR = _propYear;
            imglogentity.IMGLOG_APP = _propApplNo;
            imglogentity.IMGLOG_FAMILY_SEQ = _propSnpEntity.FamilySeq;
            imglogentity.IMGLOG_SCREEN = Privileges.Program;            
            imglogentity.IMGLOG_SECURITY = strSecuritytype;
            imglogentity.IMGLOG_TYPE = strType;
            imglogentity.IMGLOG_UPLoadAs = strimgloadas;
            imglogentity.IMGLOG_UPLOAD_BY = BaseForm.UserID;
            imglogentity.IMGLOG_ORIG_FILENAME = strimgFileName;
            imglogentity.MODE = strOpertype;
            _model.ChldMstData.InsertIMGUPLOG(imglogentity);
        }



    }
}