using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using System;
using System.IO;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HSS00134_BrowseForm : Form
    {
        private string strFolderName = string.Empty;
        private string strTempFolderName = string.Empty;
        private string strImageFolderName = string.Empty;
        public BaseForm _baseForm { get; set; }
        public PrivilegeEntity PrivilegesEntity { get; set; }
        private CaptainModel _model = null;
        public string _isUploaded = "N";
        public HSS00134_BrowseForm(BaseForm baseForm, PrivilegeEntity privilieges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _baseForm = baseForm;
            PrivilegesEntity = privilieges;
            strFolderName = _baseForm.BaseAgency + _baseForm.BaseDept + _baseForm.BaseProg + _baseForm.BaseApplicationNo;
            strTempFolderName = _model.lookupDataAccess.GetReportPath() + "\\Temp\\ScannedImages\\" + strFolderName; 
           // strImageFolderName = _model.lookupDataAccess.GetReportPath() + "\\ScannedImages";
            _isUploaded = "N";
        }

        public string _str_O_FileName = "";
        public string _str_R_FileName = "";
        public string _strFileType = "";
        public long _filecontent = 0;
        public string _tempFilePath = "";
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
            //FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            //foreach (FileInfo MyFile in MyFiles)
            //{
            //    if (MyFile.Exists)
            //    {
            //        MyFile.Delete();
            //    }
            //}


            var mobjResult = e.Files[0];

            string strName = _str_O_FileName = mobjResult.FileName;
            string strType = _strFileType = Path.GetExtension(strName.ToString().ToLower()); //getFileFormat(mobjResult.ContentType);
            long strr = _filecontent = mobjResult.ContentLength;
            
            string strModifyfilename = mobjResult.FileName;
            _str_R_FileName = strModifyfilename.Replace(',', '_').Replace('&', '_').Replace('$', '_').Replace('#', '_').Replace('/', '_').Replace("'", "_").Replace('{', '_').Replace('}', '_').Replace('@', '_').Replace('%', '_').Replace('/', '_').Replace('?', '_');

            _tempFilePath = strTempFolderName;
            mobjResult.SaveAs(strTempFolderName + "\\" + strName);
            _isUploaded = "Y";
            this.Close();
        }

        //string getFileFormat(string formatType)
        //{
        //    string contenttype = "";
        //    switch (formatType)
        //    {
        //        case ".doc":
        //            contenttype = "application/vnd.ms-word";
        //            break;
        //        case ".docx":
        //            contenttype = "application/vnd.ms-word";
        //            break;
        //        case "application/vnd.ms-excel":
        //            contenttype = ".xls";
        //            break;
        //        //case "application/vnd.ms-excel":
        //        //    contenttype = ".xlsx";
        //        //    break;
        //        case ".jpg":
        //            contenttype = "image/jpg";
        //            break;
        //        case ".png":
        //            contenttype = "image/png";
        //            break;
        //        case ".gif":
        //            contenttype = "image/gif";
        //            break;
        //        case ".pdf":
        //            contenttype = "application/pdf";
        //            break;
        //    }

        //    return contenttype;
        //}
    }
}
