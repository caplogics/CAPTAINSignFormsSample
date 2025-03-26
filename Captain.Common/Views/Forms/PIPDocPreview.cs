using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using DevExpress.Utils.VisualEffects;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Wisej.Web;
using static Syncfusion.XlsIO.Parser.Biff_Records.TextWithFormat;

namespace Captain.Common.Views.Forms
{
    public partial class PIPDocPreview : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        public string strFormCloseRefresh = string.Empty;
        private string strCheckUploadMode = string.Empty;
        string _dssAgyFolder = "";
        string _frmScreen = "";
        string _movetoCap = "";
        private string strImageFolderName = string.Empty;

        // for DSS XML Screen
        public PIPDocPreview(BaseForm baseForm, string CAPAgy, string strAppNo,string capAppno, string frmScreen)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            _baseForm = baseForm;
            _dssAgyFolder = CAPAgy;
            _frmScreen = frmScreen;
            propApplicantNumber = strAppNo;
            _movetoCap = capAppno;

            strImageFolderName = _model.lookupDataAccess.GetReportPath() + "\\ScannedImages";
            pnlAppDetails.Visible = false;
            pnldataGridCaseSnp.Visible = false;

            loadDocsGrid();
        }
        void loadDocsGrid()
        {

            string AppFilesPath = _model.lookupDataAccess.GetReportPath() + "\\" + @"DSSEXTRACTFILES\" + _dssAgyFolder + "\\" + propApplicantNumber + "\\";
            gvwFiles.Rows.Clear();
            System.Data.DataTable dtDocs = DSSXMLData.DSSXMLDOCS_GET(_baseForm.BaseDSSXMLDBConnString, propApplicantNumber, "ALL");
            List<AgyTabEntity> AgyTabImgTypesEntity = _model.Agytab.GetAgencyTableCodes("00026");
            if (dtDocs.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(_movetoCap))
                {
                    this.gvtFDragged.Visible = true;btnDragtoCap.Visible = true;
                    int UpdCnt = 0;
                    foreach (DataRow dr in dtDocs.Rows)
                    {
                        string strFileDate = dr["DSSXML_DOC_FILE_DATE"].ToString();
                        if (strFileDate != "")
                            strFileDate = Convert.ToDateTime(strFileDate).ToString("MM/dd/yyyy");

                       

                        string folderName = dr["FOLDERNAME"].ToString();

                        string strdocType = "unknown";
                        List<AgyTabEntity> lstdoctype1 = AgyTabImgTypesEntity.Where(x => x.agy2 == dr["DSSXML_DOC_TYPE"].ToString()).ToList();
                        if (lstdoctype1.Count > 0)
                            strdocType = lstdoctype1[0].agy8;

                        string strUploadDate = dr["DSSXML_DOC_DRAGGED_DATE"].ToString();
                        if (strUploadDate != "")
                        {
                            strUploadDate = Convert.ToDateTime(strUploadDate).ToString("MM/dd/yyyy");

                        }
                        else //if(strdocType!= "unknown") 
                            UpdCnt++;


                        gvwFiles.Rows.Add(strdocType, dr["DSSXML_DOC_NAME"].ToString(), strFileDate, "", "", "", strUploadDate, Path.Combine(AppFilesPath + folderName + "\\", dr["DSSXML_DOC_OG_NAME"].ToString()), dr["DSSXML_DOC_SECURITY"].ToString(), dr["DSSXML_DOC_TYPE"].ToString(), dr["DSSXML_DOC_OG_NAME"].ToString());

                        if(UpdCnt>0) btnDragtoCap.Visible = true; else btnDragtoCap.Visible = false;

                    }
                }
                else 
                {
                    this.gvtFDragged.Visible = false; btnDragtoCap.Visible = false;

                    foreach (DataRow dr in dtDocs.Rows)
                    {
                        string strFileDate = dr["DSSXML_DOC_FILE_DATE"].ToString();
                        if (strFileDate != "")
                            strFileDate = Convert.ToDateTime(strFileDate).ToString("MM/dd/yyyy");

                        string folderName = dr["FOLDERNAME"].ToString();

                        string strdocType = "unknown";
                        List<AgyTabEntity> lstdoctype1 = AgyTabImgTypesEntity.Where(x => x.agy2 == dr["DSSXML_DOC_TYPE"].ToString()).ToList();
                        if (lstdoctype1.Count > 0)
                            strdocType = lstdoctype1[0].agy8;


                        gvwFiles.Rows.Add(strdocType, dr["DSSXML_DOC_NAME"].ToString(), strFileDate, "", "", "", "", Path.Combine(AppFilesPath + folderName + "\\", dr["DSSXML_DOC_OG_NAME"].ToString()), dr["DSSXML_DOC_SECURITY"].ToString(), dr["DSSXML_DOC_TYPE"].ToString(), dr["DSSXML_DOC_OG_NAME"].ToString());
                    }
                }
                if (gvwFiles.Rows.Count > 0)
                    gvwFiles.Rows[0].Selected = true;

            }
        }

        public PIPDocPreview(string strHierchy, BaseForm baseForm, string strAgency, string strDept, string strProgram, string strYear, string strConfNum, string strpEmailId, string strRegID, string strAppNo)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            _baseForm = baseForm;
            _propRegid = string.Empty;
            propHierchy = strHierchy;
            propAgency = strAgency; ;
            propDept = strDept;
            propProgram = strProgram;
            propYear = strYear;
            propClientRulesSwitch = "N";
            _propPIP_REG_ID = strRegID;
            propApplicantNumber = strAppNo;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(_baseForm.UserID, "3", propAgency, propDept, " ");
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];
                }
            }

            propimagetypesCategory = _model.lookupDataAccess.GetImageNameConvention();

            //    string strImages = "CM ";
            if (!string.IsNullOrEmpty(_baseForm.UserProfile.ImageTypes.Trim()))
            {
                string[] strImageTypes = _baseForm.UserProfile.ImageTypes.Trim().Split(' ');//strImages.Split(' '); //
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

            if (Consts.Common.ServerLocation == string.Empty)
            {
                _propPIPIMGDOCPATH = @"D:\CABA-REPORTS\ScannedImages\01010400000003\0000001\";// @"\\CAPSYSAZ8\PIP_DOCS\" + BaseForm.BaseAgencyControlDetails.AgyShortName + @"00\DOCUPLDS\";

            }
            else
            {
                _propPIPIMGDOCPATH = @"\\CAPSYSAZ8\PIP_DOCS\";
            }

            lblConf2.Text = strConfNum;
            lblEmail2.Text = strpEmailId;
            lblAppnum.Text = "App#: " + propAgency + propDept + propProgram + " " + (propYear == string.Empty ? "    " : propYear) + " " + propApplicantNumber;

            FillTopGrid();
        }

        DataTable User_Hie_Acc_List = new DataTable();
        public string propHierchy;
        public string propAgency;
        public string propDept;
        public string propProgram;
        public string propYear;
        public string propSSN;
        public string propApplicantNumber;
        public string propFirstName;
        public string propLastName;
        public string propDob;
        List<CommonEntity> propimagetypesCategory { get; set; }
        public DataTable _propPIPData { get; set; }
        public DataTable _propCasesnpsubdt { get; set; }
        public string propLeanUserId = string.Empty;
        public string propLeanServices = string.Empty;
        public string _propRegid = string.Empty;
        public BaseForm _baseForm { get; set; }
        string propClientRulesSwitch { get; set; }
        string _propPIP_REG_ID { get; set; }
        private string _propPIPIMGDOCPATH { get; set; }

        public List<PIPDocEntity> _propDocentitylist = new List<PIPDocEntity>();
        string strFormLoad = string.Empty;
        private void FillTopGrid()
        {
            string strEmailId = lblEmail2.Text;
            if (strFormLoad != string.Empty)
                strEmailId = string.Empty;

            propSSN = string.Empty;
            string strFilterAgy = string.Empty;
            if (_baseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                strFilterAgy = propAgency;
            string strresult = CheckLeanIntakeData(string.Empty, lblConf2.Text, strEmailId, string.Empty, _baseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);
            dataGridCaseSnp.Rows.Clear();

            string pipdocumentcount = string.Empty;
            string strPIPId = string.Empty;
            string strRegid = string.Empty;

            List<PIPCAPLNK> pipcaplnkdata = PIPDATA.GetPIPCAPLNK(_baseForm.BaseLeanDataBaseConnectionString, propAgency, propDept, propProgram, propYear, propApplicantNumber, _baseForm.BaseAgencyControlDetails.AgyShortName, string.Empty, _propPIP_REG_ID);


            if (strresult == string.Empty)
            {
                DataTable dt = GetPIPIntakeData(string.Empty, lblConf2.Text, strEmailId, string.Empty, _baseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);
                List<CommonEntity> Relation = _model.lookupDataAccess.GetRelationship();

                if (dt.Rows.Count > 0)
                {
                    _propPIPData = dt;
                    _propRegid = dt.Rows[0]["PIPREG_ID"].ToString().Trim();
                    foreach (DataRow dritem in dt.Rows)
                    {
                        //List<CaseSnpEntity> propcasesnplist = _model.CaseMstData.GetCaseSnpadpyn(propAgency, propDept, propProgram, propYear, propApplicantNumber);
                        //foreach (CaseSnpEntity snpEntity in propcasesnplist)
                        //{ 
                        //    if (pipcaplnkdata.Count > 0)
                        //    {
                        //        PIPCAPLNK piplinksdata = pipcaplnkdata.Find(u => u.PIPCAPLNK_FAMILY_SEQ == snpEntity.FamilySeq);

                        //        List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == piplinksdata.PIPCAPLNK_PIP_ID && u.PIPDOCUPLD_LINK_DOC == string.Empty);
                        //        strPIPId = piplinksdata.PIPCAPLNK_PIP_ID;
                        //        pipdocumentcount = pipdoccount.Count.ToString();
                        //    }
                        //}
                        _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(_baseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propRegid, "REGID");
                        List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dritem["PIP_ID"].ToString() && u.PIPDOCUPLD_LINK_DOC == string.Empty);
                        strPIPId = dritem["PIP_ID"].ToString();
                        pipdocumentcount = pipdoccount.Count.ToString();


                        string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), _baseForm.BaseHierarchyCnFormat);
                        string memberCode = string.Empty;
                        CommonEntity rel = Relation.Find(u => u.Code.Equals(dritem["PIP_MEMBER_CODE"].ToString()));
                        if (rel != null) memberCode = rel.Desc;
                        string DOB = string.Empty;
                        if (dritem["PIP_DOB"].ToString() != string.Empty)
                            DOB = CommonFunctions.ChangeDateFormat(dritem["PIP_DOB"].ToString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        int rowIndex = dataGridCaseSnp.Rows.Add(ApplicantName, dritem["PIP_SSN"].ToString(), DOB, memberCode, pipdocumentcount, string.Empty, string.Empty, strPIPId, _propRegid);
                        dataGridCaseSnp.Rows[rowIndex].Tag = dritem;
                        if (dritem["PIP_FAM_SEQ"].ToString() == "0")
                        {
                            propSSN = dritem["PIP_SSN"].ToString();
                            propFirstName = dritem["PIP_FNAME"].ToString();
                            propLastName = dritem["PIP_LNAME"].ToString();
                            propDob = dritem["PIP_DOB"].ToString();
                            propLeanUserId = dritem["PIP_REG_ID"].ToString();
                            propLeanServices = dritem["PIP_SERVICES"].ToString();
                            dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                            if (strFormLoad != string.Empty)
                            {
                                if (dritem["PIP_TOWNSHIP"].ToString().Trim() != string.Empty)
                                {

                                    List<ZipCodeEntity> propzipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, dritem["PIP_TOWNSHIP"].ToString().Trim(), string.Empty);
                                    if (propzipcodeEntity.FindAll(u => u.Zcrcitycode.Trim() == dritem["PIP_TOWNSHIP"].ToString().Trim()).Count == 0)
                                    {
                                        CommonFunctions.MessageBoxDisplay("Customer not in a town/city the agency services");
                                    }
                                }
                            }
                        }
                    }
                    if (dataGridCaseSnp.Rows.Count > 0)
                        dataGridCaseSnp.Rows[0].Selected = true;
                }

            }

        }

        string CheckLeanIntakeData(string struserid, string strLcode, string strEmail, string strMode, string strDBName, string strAgency)
        {
            string strLocstatus = string.Empty;
            try
            {


                SqlConnection con = new SqlConnection(_baseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (struserid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", struserid);
                    if (strLcode != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_CONFNO", strLcode);
                    if (strEmail != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_EMAIL", strEmail);
                    cmd.Parameters.AddWithValue("@AGENCY", strDBName);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgency);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    // Start getting the RetrunValue and Output from Stored Procedure
                    strLocstatus = cmd.Parameters["@Msg"].Value.ToString();
                    //if (IntReturn == 1)
                    //{
                    //    outputParam = cmd.Parameters["@OutputParam"].Value;
                    //    inoutParam = cmd.Parameters["@InputOutputParam"].Value;
                    //}

                }
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return strLocstatus;
        }

        DataTable GetPIPIntakeData(string struserid, string strLcode, string strEmail, string strMode, string strDBName, string strAgency)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(_baseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (struserid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", struserid);
                    cmd.Parameters.AddWithValue("@PIP_CONFNO", strLcode);

                    if (strEmail != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_EMAIL", strEmail);
                    cmd.Parameters.AddWithValue("@AGENCY", strDBName);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgency);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 10);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);

            }
            return dt;

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strPreviewFile = "";

            if (gvwFiles.Rows.Count > 0)
            {
                if (gvwFiles.SelectedRows[0].Selected)
                {
                    if (_frmScreen == "TMS00141")
                    {
                        string strFilePath = gvwFiles.SelectedRows[0].Cells["docpath"].Value.ToString();

                        FileInfo fi = new FileInfo(strFilePath);
                        string strExtension = fi.Extension;

                        if ((strExtension.ToUpper() == ".PDF") || (strExtension.ToUpper() == ".TXT"))
                        {
                            PdfViewerNewForm objfrm = new PdfViewerNewForm(strFilePath);
                            objfrm.StartPosition = FormStartPosition.CenterScreen;
                            objfrm.ShowDialog();
                        }
                        else if ((strExtension.ToUpper() == ".DOC") || (strExtension.ToUpper() == ".XLS") || (strExtension.ToUpper() == "DOCX") || (strExtension.ToUpper() == "XLSX"))
                        {
                            //CommonFunctions.MessageBoxDisplay("you can't preview this file.Please wait downloaded this file");
                            FileDownloadGateway downloadGateway = new FileDownloadGateway();
                            downloadGateway.Filename = strPreviewFile;

                            downloadGateway.SetContentType(DownloadContentType.OctetStream);

                            downloadGateway.StartFileDownload(new ContainerControl(), strFilePath);
                        }
                        else
                        {
                            PdfViewerNewForm objfrm = new PdfViewerNewForm(strFilePath);
                            objfrm.StartPosition = FormStartPosition.CenterScreen;
                            objfrm.ShowDialog();
                        }


                    }
                    else
                    {
                        using (NetworkShareAccesser.Access("CAPSYSAZ8", "PIP_Docs", "ApplicationUser", "7!Enough32Distract$"))
                        {
                            string strFilterAgy = "00";
                            if (_baseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                                strFilterAgy = _baseForm.BaseAgency;
                            strPreviewFile = gvwFiles.SelectedRows[0].Cells["gvtFileName"].Value.ToString();
                            int Count = strPreviewFile.Length;
                            string strExtension = (strPreviewFile.Substring(Count - 4, 4));
                            if ((strExtension.ToUpper() == ".PDF") || (strExtension.ToUpper() == ".TXT"))
                            {


                                PdfViewerNewForm objfrm = new PdfViewerNewForm(_propPIPIMGDOCPATH + _baseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString() + "\\" + strPreviewFile);
                                objfrm.StartPosition = FormStartPosition.CenterScreen;
                                objfrm.ShowDialog();
                            }
                            else if ((strExtension.ToUpper() == ".DOC") || (strExtension.ToUpper() == ".XLS") || (strExtension.ToUpper() == "DOCX") || (strExtension.ToUpper() == "XLSX"))
                            {
                                //CommonFunctions.MessageBoxDisplay("you can't preview this file.Please wait downloaded this file");
                                FileDownloadGateway downloadGateway = new FileDownloadGateway();
                                downloadGateway.Filename = strPreviewFile;

                                downloadGateway.SetContentType(DownloadContentType.OctetStream);

                                downloadGateway.StartFileDownload(new ContainerControl(), _propPIPIMGDOCPATH + _baseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString() + "\\" + strPreviewFile);
                            }
                            else
                            {
                                PdfViewerNewForm objfrm = new PdfViewerNewForm(_propPIPIMGDOCPATH + _baseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString() + "\\" + strPreviewFile);
                                objfrm.StartPosition = FormStartPosition.CenterScreen;
                                objfrm.ShowDialog();
                            }
                        }
                    }
                }
            }
        }

        List<CommonEntity> _propImagesTypesOnly = new List<CommonEntity>();
        private void dataGridCaseSnp_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridCaseSnp.SelectedRows.Count > 0)
            {
                gvwFiles.Rows.Clear();
                string strDocument = string.Empty;
                int intRowdex;
                if (dataGridCaseSnp.Rows.Count > 0)
                {
                    if (dataGridCaseSnp.SelectedRows[0].Selected)
                    {
                        _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(_baseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, dataGridCaseSnp.SelectedRows[0].Cells["gvtRegId"].Value.ToString(), "REGID");
                        List<PIPDocEntity> pipdoclist = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dataGridCaseSnp.SelectedRows[0].Cells["gvtPIPID"].Value.ToString() && u.PIPDOCUPLD_LINK_DOC == string.Empty);
                        foreach (PIPDocEntity item in pipdoclist)
                        {

                            strDocument = string.Empty;
                            CommonEntity commimgdoc = _propImagesTypesOnly.Find(u => u.Code.Trim() == item.PIPDOCUPLD_DOCTYPE.Trim());
                            if (commimgdoc != null)
                            {
                                strDocument = commimgdoc.Desc;
                                intRowdex = gvwFiles.Rows.Add(strDocument, item.PIPDOCUPLD_DOCNAME, LookupDataAccess.Getdate(item.PIPDOCUPLD_DATE_ADD));
                                gvwFiles.Rows[intRowdex].Tag = item;
                            }
                        }
                    }
                    if (gvwFiles.Rows.Count > 0)
                        gvwFiles.Rows[0].Selected = true;
                }

            }
        }

        private void btnDragtoCap_Click(object sender, EventArgs e)
        {
            if(gvwFiles.Rows.Count > 0)
            {
                //CASEMSEntity caseMstEntity = null;
                //caseMstEntity = _model.CaseMstData.GetCaseMST(_movetoCap.Substring(0, 2).ToString(), _movetoCap.Substring(2, 2).ToString(), _movetoCap.Substring(4, 2).ToString(), _movetoCap.Substring(6, 4).ToString(), _movetoCap.Substring(10, 8).ToString());

                List<CaseMstEntity> casemstlist = _model.CaseMstData.GetCaseMstAll(_movetoCap.Substring(0, 2), _movetoCap.Substring(2, 2), _movetoCap.Substring(4, 2), _movetoCap.Substring(6, 4), _movetoCap.Substring(10, 8), string.Empty, string.Empty, string.Empty, string.Empty, "MSTALLSNP");

                string FamSeq = string.Empty;
                if (casemstlist.Count > 0)
                    FamSeq = casemstlist[0].FamilySeq;

                string AppFilesPath = _model.lookupDataAccess.GetReportPath() + "\\" + @"DSSEXTRACTFILES\" + _dssAgyFolder + "\\" + propApplicantNumber + "\\";

                string strMainFolderName = strImageFolderName + "\\" + _movetoCap.Substring(0, 2) + _movetoCap.Substring(2, 2) + _movetoCap.Substring(4, 2) + _movetoCap.Substring(10, 8);

                string strFullFolderName = strImageFolderName + "\\" + _movetoCap.Substring(0,2) + _movetoCap.Substring(2, 2) + _movetoCap.Substring(4, 2) + _movetoCap.Substring(10, 8) + "\\" + "0000000".Substring(0, 7 - FamSeq.Length) + FamSeq;

                foreach (DataGridViewRow row in gvwFiles.Rows) 
                {
                    if (string.IsNullOrEmpty(row["gvtFDragged"].Value.ToString()))
                    {
                        string DocPath = row["docpath"].Value.ToString();

                        string strpathToCheck = strFullFolderName + "\\" + row["gvtFileName"].Value.ToString(); 

                        if(!string.IsNullOrEmpty(row["gvtFileName"].Value.ToString().Trim()))
                        {
                            //if (System.IO.File.Exists(strpathToCheck))
                            //{

                            //}

                            


                            //Try to create the directory.
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

                            File.Copy(DocPath, strpathToCheck, true);
                            ////File.Delete(DocPath);

                            if (!string.IsNullOrEmpty(row["docType"].Value.ToString().Trim()))
                            {
                                InsertDeleteImgUploagLog("U", row["docSecurity"].Value.ToString(), row["docType"].Value.ToString(), row["docOGName"].Value.ToString(), row["gvtFileName"].Value.ToString(),FamSeq);

                                var _flagimgdocs = DSSXMLData.insupDelDXMLDocs(_baseForm.BaseDSSXMLDBConnString, propApplicantNumber, row["docSecurity"].Value.ToString(), row["docType"].Value.ToString(),
                                            row["docOGName"].Value.ToString(), string.Empty, string.Empty, string.Empty, string.Empty,"Y", "UPDATE");
                            }

                        }

                        
                    }
                }
                loadDocsGrid();
            }
        }

        private void InsertDeleteImgUploagLog(string strOpertype, string strSecuritytype, string strType, string strimgFileName, string strimgloadas,string FamSeq)
        {
            IMGUPLOGNEntity imglogentity = new IMGUPLOGNEntity();
            imglogentity.IMGLOG_AGY = _baseForm.BaseAgency;
            imglogentity.IMGLOG_DEP = _baseForm.BaseDept;
            imglogentity.IMGLOG_PROG = _baseForm.BaseProg;
            imglogentity.IMGLOG_YEAR = _baseForm.BaseYear;
            imglogentity.IMGLOG_APP = _movetoCap.Substring(10,8);
            imglogentity.IMGLOG_FAMILY_SEQ = FamSeq;
            imglogentity.IMGLOG_SCREEN = "CASE2001";
            imglogentity.IMGLOG_SECURITY = strSecuritytype;
            imglogentity.IMGLOG_TYPE = strType;
            imglogentity.IMGLOG_UPLoadAs = strimgloadas;
            imglogentity.IMGLOG_UPLOAD_BY = _baseForm.UserID;
            imglogentity.IMGLOG_ORIG_FILENAME = strimgFileName;
            imglogentity.MODE = strOpertype;
            _model.ChldMstData.InsertIMGUPLOG(imglogentity);
        }

    }
}
