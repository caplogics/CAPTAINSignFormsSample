using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using DevExpress.CodeParser;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSpreadsheet.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class Invoice_Documents : Form
    {

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        private string strTempFolderName = string.Empty;
        private string strImageFolderName = string.Empty;
        private string strFolderName = string.Empty;

        public Invoice_Documents(BaseForm baseform, PrivilegeEntity privilegentity, CASESPMEntity SPM_Entity, string invoiceType,string Service)
        {
            InitializeComponent();

            baseForm = baseform;
            privilegeEntity = privilegentity;
            serType= Service;
            _model = new CaptainModel();

            strFolderName = baseForm.BaseAgency + baseForm.BaseDept + baseForm.BaseProg + baseForm.BaseApplicationNo;
            strTempFolderName = _model.lookupDataAccess.GetReportPath() + "\\Temp\\Invoices\\" + strFolderName;
            string strYear = string.Empty;
            if (!string.IsNullOrEmpty(baseForm.BaseYear.Trim())) strYear = baseForm.BaseYear; else strYear = baseForm.BaseAgency + baseForm.BaseDept + baseForm.BaseProg;

            strImageFolderName = _model.lookupDataAccess.GetReportPath() + "\\INVDOCS\\" + strYear + "\\" + strFolderName;

            propReportPath = _model.lookupDataAccess.GetReportPath();

            spm_entity = SPM_Entity;
            invType = invoiceType;

            

            Fill_InvoiceDoc_Grid();
        }

        string Img_Del = Consts.Icons.ico_Delete;
        string Img_Blank = Consts.Icons.ico_Blank;

        List<INVDOCLOGEntity> INVDocs = new List<INVDOCLOGEntity>();
        private void Fill_InvoiceDoc_Grid()
        {
            dgvInvDocs.Rows.Clear();

            InvDoclogEntitylist = _model.ChldMstData.GetInvDocsLogList(baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, baseForm.BaseYear, baseForm.BaseApplicationNo, spm_entity.service_plan, spm_entity.Seq, string.Empty, string.Empty);

            if (string.IsNullOrEmpty(serType.Trim()))
            {
                if (invType == "P")
                    INVDocs = InvDoclogEntitylist.FindAll(u => u.INVDOC_VEND_TYPE == "P" && u.INVDOC_DELETED_BY == string.Empty && u.INVDOC_DATE_DELETED == string.Empty);
                else if (invType == "S")
                    INVDocs = InvDoclogEntitylist.FindAll(u => u.INVDOC_VEND_TYPE == "S" && u.INVDOC_DELETED_BY == string.Empty && u.INVDOC_DATE_DELETED == string.Empty);

                INVDocs = INVDocs.FindAll(u => u.INVDOC_SERVICE.Equals(""));
            }
            else
                INVDocs = InvDoclogEntitylist.FindAll(u=>u.INVDOC_SERVICE.Trim()==serType.Trim() && u.INVDOC_DELETED_BY == string.Empty && u.INVDOC_DATE_DELETED == string.Empty);

            int rowIndex = 0;

            foreach (INVDOCLOGEntity Entity in INVDocs)
            {
                //if(!string.IsNullOrEmpty(Entity.INVDOC_BUNDLE.Trim()))
                //    rowIndex = dgvInvDocs.Rows.Add(Entity.INVDOC_ORIG_NAME, Entity.INVDOC_DATE_UPLOAD == "" ? "" : LookupDataAccess.Getdate(Entity.INVDOC_DATE_UPLOAD), Entity.INVDOC_UPLOAD_BY, Img_Blank, Entity.INVDOC_ID);
                //else
                    rowIndex = dgvInvDocs.Rows.Add(Entity.INVDOC_ORIG_NAME, Entity.INVDOC_DATE_UPLOAD == "" ? "" : LookupDataAccess.Getdate(Entity.INVDOC_DATE_UPLOAD), Entity.INVDOC_UPLOAD_BY, Img_Del, Entity.INVDOC_ID);
                
                //.Split('.').First()

                //dgvInvDocs.Rows[rowIndex].Tag = Entity;

            }

            if (dgvInvDocs.Rows.Count > 0)
            {
                dgvInvDocs.Rows[0].Selected = true;
                dgvInvDocs.CurrentCell = dgvInvDocs.Rows[0].Cells[1];
            }

        }

        #region Properties

        public BaseForm baseForm { get; set; }
        public PrivilegeEntity privilegeEntity { get; set; }
        public List<INVDOCLOGEntity> InvDoclogEntitylist
        {
            get; set;
        }
        public CASESPMEntity spm_entity
        {
            get; set;
        }
        public string propReportPath
        {
            get; set;
        }
        public string invType
        {
            get; set;
        }

        public string serType { get; set; }

        #endregion

        string doc_Name = string.Empty;
        private void dgvInvDocs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInvDocs.Rows.Count > 0)
            {
                doc_Name = strImageFolderName + "\\" + dgvInvDocs.CurrentRow.Cells["gvInvDocName"].Value.ToString();

                if (e.RowIndex != -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (baseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                        {
                            PdfViewerNewForm objfrm = new PdfViewerNewForm(doc_Name);
                            objfrm.StartPosition = FormStartPosition.CenterScreen;
                            objfrm.ShowDialog();
                        }
                        else
                        {
                            FrmViewer objfrm = new FrmViewer(doc_Name);
                            objfrm.StartPosition = FormStartPosition.CenterScreen;
                            objfrm.ShowDialog();
                        }
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        if (dgvInvDocs.CurrentRow.Cells["gvdel"].Value.ToString()!= "blank")
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_InvDoc);
                    }
                }
            }
        }

        private void Delete_InvDoc(DialogResult dialogResult)
        {
            if (DialogResult.Yes == dialogResult)
            {
                INVDOCLOGEntity imglogentity = new INVDOCLOGEntity();
                imglogentity.INVDOC_ID = dgvInvDocs.CurrentRow.Cells["gvInvDocID"].Value.ToString();
                imglogentity.INVDOC_AGENCY = baseForm.BaseAgency;
                imglogentity.INVDOC_DEPT = baseForm.BaseDept;
                imglogentity.INVDOC_PROGRAM = baseForm.BaseProg;
                imglogentity.INVDOC_YEAR = baseForm.BaseYear;
                imglogentity.INVDOC_APP = baseForm.BaseApplicationNo;

                imglogentity.INVDOC_SERVICEPLAN = spm_entity.service_plan;
                imglogentity.INVDOC_SPM_SEQ = spm_entity.Seq;


                imglogentity.INVDOC_VEND_TYPE = invType;
                imglogentity.INVDOC_SEQ = "1";

                if (invType == "P")
                    imglogentity.INVDOC_VENDOR = spm_entity.SPM_Vendor;
                else if (invType == "S")
                    imglogentity.INVDOC_VENDOR = spm_entity.SPM_Gas_Vendor;

                imglogentity.INVDOC_UPLOAD_BY = baseForm.UserID;
                imglogentity.INVDOC_DELETED_BY = baseForm.UserID;
                imglogentity.INVDOC_DATE_DELETED = DateTime.Now.ToString();
                imglogentity.MODE = "DELETE";

                if (_model.ChldMstData.InsertINVDOCLOG(imglogentity))
                {
                    AlertBox.Show("Deleted Successfully");

                    Fill_InvoiceDoc_Grid();
                }
            }
        }
    }
}
