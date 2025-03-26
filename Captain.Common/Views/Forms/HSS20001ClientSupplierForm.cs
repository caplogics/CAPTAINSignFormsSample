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
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Interfaces;
using System.IO;
using DevExpress.DataAccess.DataFederation;
using DevExpress.XtraReports.UI;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS20001ClientSupplierForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private string[] strCode = null;
        public int strIndex = 0;
        #endregion

        public HSS20001ClientSupplierForm(BaseForm baseform, PrivilegeEntity privileges)
        {

            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2;
            _model = new CaptainModel();
            Mode = string.Empty;

            propSeq = string.Empty;
            BaseForm = baseform;
            Privileges = privileges;
            this.Text = Privileges.PrivilegeName + "- View";
            new ToolTip().SetToolTip(picAddClient, "Add Supplier");

            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                PropAcctSoft = dsAgency.Tables[0].Rows[0]["ACR_VEND_ACCT_SOFT_EDIT"].ToString().Trim();
            }

            fillCmbSources();
            fillVendors();
            boolchangemod = false;
            fillGridShow();
            List<FldcntlHieEntity> CntlEntity = _model.FieldControls.GetFLDCNTLHIE("TMS00201", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "FLDCNTL");
            FLDCNTLHieEntity = CntlEntity;
            EnableControl(false);
            propProgramDefinition = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            btnCancel.Text = "Close";

            

            DataSet ds = DatabaseLayer.AgyTab.GetAgyTabDetails("08550");
            if (ds.Tables[0].Rows.Count > 0)
                AgyList = ds.Tables[0];

            

            if (Privileges.AddPriv.Equals("false"))
            {
                picAddClient.Visible = false;

            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                gvwSupplierDetails.Columns["gviEdit"].Visible = false;

            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvwSupplierDetails.Columns["gviDel"].Visible = false;

            }

            if (PropAcctSoft=="Y" && PrimaryPayFor == "04") picAddClient.Visible = false; else picAddClient.Visible = true;


            /***/
            lblSupplierType.Location = new System.Drawing.Point(600, 7);
            lblTypeReq.Location = new System.Drawing.Point(590, 6);

        }

        private void btnvendor_Click(object sender, EventArgs e)
        {
            if (PayValidateForm())
            {

                if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() != "0")
                {
                    VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, ((ListItem)cmbPayfor.SelectedItem).Value.ToString(), string.Empty,null);
                    Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
                    Vendor_Browse.FormClosed += On_Vendor_Browse_Closed;
                    Vendor_Browse.ShowDialog();
                }
            }
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public List<CommonEntity> proplistPayfor { get; set; }
        public string propSeq { get; set; }
        public string Source_Type { get; set; }
        public string Mode { get; set; }
        public ProgramDefinitionEntity propProgramDefinition { get; set; }
        public string propVendorCode { get; set; }
        public string propAccountNo { get; set; }
        public string PayforDefault { get; set; }
        public List<FldcntlHieEntity> FLDCNTLHieEntity { get; set; }
        public bool boolchangemod { get; set; }
        public string PropAcctSoft { get; set; }
        public string PrimaryPayFor { get; set; }
        public DataTable AgyList { get; set; }
        #endregion

        //string Img_Blank = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blann.jpeg");
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        //string Img_Blank = "blank";
        //string Img_Tick = "Resources/Images/tick2.png";

        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = Consts.Icons.ico_Tick;
        private void On_Vendor_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                _errorProvider.SetError(txtVendorId, null);
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();
                txtVendorId.Text = Vendor_Details[0].Trim();
                txtVendorName.Text = Vendor_Details[1].Trim();

                txtVendorId_TextChanged(sender, e);
            }
        }

        private void fillcmbPayfor()
        {
            cmbPayfor.SelectedIndexChanged -= cmbPayfor_SelectedIndexChanged;

            cmbPayfor.Items.Clear();
            cmbPayfor.ColorMember = "FavoriteColor";
            PayforDefault = string.Empty;
            proplistPayfor = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "08004", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            cmbPayfor.Items.Insert(0, new ListItem("Select One", "0"));
            cmbPayfor.SelectedIndex = 0;
            cmbSupplierType.Items.Clear();
            cmbSupplierType.Items.Add(new ListItem("Primary", "P"));
            cmbSupplierType.Items.Add(new ListItem("Secondary", "S"));
            cmbSupplierType.Items.Insert(0, new ListItem("Select One", "0"));
            cmbSupplierType.SelectedIndex = 0;
            foreach (CommonEntity pauforStatus in proplistPayfor)
            {
                ListItem li = new ListItem(pauforStatus.Desc, pauforStatus.Code, pauforStatus.Active, pauforStatus.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbPayfor.Items.Add(li);
                if (pauforStatus.Default.Equals("Y"))
                {
                    cmbPayfor.SelectedItem = li;
                    PayforDefault = pauforStatus.Code;
                }
            }
            cmbPayfor.SelectedIndexChanged += cmbPayfor_SelectedIndexChanged;
        }

        private void fillCmbSources()
        {
            fillcmbPayfor();

            //proplistPayfor = _model.lookupDataAccess.GetLookkupFronAGYTAB("08004");

            //foreach (CommonEntity dr in proplistPayfor)
            //{
            //    cmbPayfor.Items.Add(new ListItem(dr.Desc, dr.Code));
            //}
            //cmbPayfor.Items.Insert(0, new ListItem("Select One", "0"));
            //cmbPayfor.SelectedIndex = 0;
            cmbBilling.SelectedIndexChanged -= new EventHandler(cmbBilling_SelectedIndexChanged);
            cmbBilling.Items.Add(new ListItem("   ", "0"));
            cmbBilling.SelectedIndex = 0;
            int rowIndex = 0;
            foreach (CaseSnpEntity item in BaseForm.BaseCaseSnpEntity)
            {
                rowIndex++;
                if (item.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq)
                    cmbBilling.Items.Add(new ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "A"));
                else
                    cmbBilling.Items.Add(new ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "M"));
            }
            cmbBilling.Items.Add(new ListItem("3rd Party Billing", "T","T","T"));
            cmbBilling.SelectedIndexChanged += new EventHandler(cmbBilling_SelectedIndexChanged);
        }
        List<CaseVDD1Entity> Vdd1list; List<CASEVDDEntity> CaseVddlist;
        private void fillVendors()
        {


            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVDD1Entity Vdd1_Entity = new CaseVDD1Entity(true);


            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");
            Vdd1list = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");

        }


        private void cmbBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbBilling.SelectedItem).Value.ToString() != "0")
            {
                if (((ListItem)cmbBilling.SelectedItem).Value.ToString() == "T")
                {

                    if (Mode == "Add")
                    {
                        txtFirst.Text = string.Empty;
                        txtLast.Text = string.Empty;

                        txtFirst.Enabled = true;
                        txtLast.Enabled = true;
                    }
                    if (Mode == "Edit")
                    {
                        txtFirst.Enabled = true;
                        txtLast.Enabled = true;
                    }
                }
                else
                {
                    CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == ((ListItem)cmbBilling.SelectedItem).ID.ToString());
                    if (casesnp != null)
                    {
                        txtFirst.Enabled = false;
                        txtLast.Enabled = false;
                        txtFirst.Text = casesnp.NameixFi;
                        txtLast.Text = casesnp.NameixLast;
                    }
                }
            }
        }

        string PassFail = string.Empty;
        private void btnSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(cmbPayfor, null);
            if (ValidateForm())
            {
                btnSave.Enabled = false;
                //if (txtAccountNo.Text.Length == 14)
                //    txtAccountNo.Text = txtAccountNo.Text + "      ";
                //if (txtAccountNo.Text.Length == 15)
                //    txtAccountNo.Text = txtAccountNo.Text + "     ";
                //if (txtAccountNo.Text.Length == 16)
                //    txtAccountNo.Text = txtAccountNo.Text + "    ";
                //if (txtAccountNo.Text.Length == 17)
                //    txtAccountNo.Text = txtAccountNo.Text + "   ";
                //if (txtAccountNo.Text.Length == 18)
                //    txtAccountNo.Text = txtAccountNo.Text + "  ";
                //if (txtAccountNo.Text.Length == 19)
                //    txtAccountNo.Text = txtAccountNo.Text + " ";
                propAccountNo = txtAccountNo.Text.Trim();
                propVendorCode = txtVendorId.Text;
                if (CheckAccountNo())
                {
                    bool boolsucess = true; string AccFormat = string.Empty;
                    CaseVDD1Entity casevdd1 = Vdd1list.Find(u => u.Code == txtVendorId.Text);
                    AccFormat = string.Empty;
                    if (casevdd1 != null)
                        AccFormat = casevdd1.ACCT_FORMAT;

                    if (txtAccountNo.Text.Trim() != string.Empty)
                    {
                        List<LiheApvEntity> liheapvdetails = _model.LiheAllData.GetLiheAppvadpyas(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, txtVendorId.Text, txtAccountNo.Text, string.Empty);
                        liheapvdetails = liheapvdetails.FindAll(u => u.LPV_APP_NO != BaseForm.BaseApplicationNo);
                        if (liheapvdetails.Count > 0)
                        {
                            boolsucess = false;
                            CASEVDDEntity casevdd = CaseVddlist.Find(u => u.Code == txtVendorId.Text);
                            string strVendorName = string.Empty;
                            if (casevdd != null)
                                strVendorName = casevdd.Name;
                            CommonFunctions.MessageBoxDisplay("Account# entered is assigned to " + strVendorName + " APP# " + liheapvdetails[0].LPV_APP_NO);
                            btnSave.Enabled = true;

                        }
                    }
                    else
                    {
                        //CaseVDD1Entity casevdd = Vdd1list.Find(u => u.Code == txtVendorId.Text);
                        //AccFormat = string.Empty;
                        //if (casevdd != null)
                        //    AccFormat = casevdd.ACCT_FORMAT;
                    }
                    if (boolsucess)
                    {

                        LiheApvEntity liheapv = new LiheApvEntity();
                        liheapv.LPV_AGENCY = BaseForm.BaseAgency;
                        liheapv.LPV_DEPT = BaseForm.BaseDept;
                        liheapv.LPV_PROGRAM = BaseForm.BaseProg;
                        liheapv.LPV_YEAR = BaseForm.BaseYear;
                        liheapv.LPV_APP_NO = BaseForm.BaseApplicationNo;
                        if (BaseForm.BaseCaseMstListEntity[0].HeatIncRent.Trim() == "2" && ((ListItem)cmbSupplierType.SelectedItem).Value.ToString().Trim() == "P")
                        {
                            liheapv.LPV_VENDOR = string.Empty;
                        }
                        else
                        {
                            liheapv.LPV_VENDOR = txtVendorId.Text;
                        }
                        //if (Mode.Equals("Add"))
                        //{
                        //    liheapv.LPV_PRIMARY_CODE = "S";
                        //}
                        //else
                        //{
                        liheapv.LPV_PRIMARY_CODE = ((ListItem)cmbSupplierType.SelectedItem).Value.ToString();// txtPrimary.Text;
                        // }
                        liheapv.LPV_PAYMENT_FOR = ((ListItem)cmbPayfor.SelectedItem).Value.ToString();
                        liheapv.LPV_LSTC_OPERATOR = BaseForm.UserID;
                        liheapv.LPV_ADD_OPERATOR = BaseForm.UserID;
                        liheapv.LPV_BILL_FNAME = txtFirst.Text;
                        liheapv.LPV_BILL_LNAME = txtLast.Text;
                        if (((ListItem)cmbBilling.SelectedItem).Value.ToString() != "0")
                            liheapv.LPV_BILLNAME_TYPE = ((ListItem)cmbBilling.SelectedItem).ValueDisplayCode.ToString();
                        liheapv.LPV_ACCOUNT_NO = txtAccountNo.Text.Trim();

                        if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                        {
                            if (chkReverify.Checked == true)
                                liheapv.LPV_REVERIFY = "Y";
                            else
                                liheapv.LPV_REVERIFY = "N";
                            if (Mode.ToUpper().Equals("ADD"))
                            {
                                if (txtAccountNo.Text.Trim() == string.Empty)
                                {
                                    liheapv.LPV_REVERIFY = "N";
                                }
                            }

                            ////Added by Sudheer on 07/24/2020
                            //if(PropAcctSoft=="Y")
                            //{
                            //    if (string.IsNullOrEmpty(PassFail.Trim())) PassFail = "P";
                            //    if (txtAccountNo.Text.Trim() == string.Empty && AccFormat == string.Empty)
                            //    {
                            //        PassFail = "F";
                            //    }
                            //    else if (txtAccountNo.Text.Trim() != string.Empty && AccFormat == string.Empty)
                            //        PassFail = "";


                            //}
                            //liheapv.LVR_FAILED_ACCOUNT_EDIT = PassFail;

                            if (PropAcctSoft == "Y") { chkReverify.Visible = false;lblReverified.Visible = false; }

                        }
                        //else
                        //{
                        //    if (txtAccountNo.Text.Trim() == string.Empty && (AccFormat == "12" || AccFormat == "13" || AccFormat == "03" || AccFormat == "10" || AccFormat == "11"))
                        //    {
                        //        PassFail = "F";
                        //    }
                        //    else PassFail = "";

                        //    liheapv.LPV_REVERIFY = "N";
                        //    liheapv.LVR_FAILED_ACCOUNT_EDIT = PassFail;
                        //}

                        ///Added by Sudheer on 08/06/2020
                        if (PropAcctSoft == "Y")
                        {
                            if (AccFormat == "12" || AccFormat == "13" || AccFormat == "03" || AccFormat == "10" || AccFormat == "11")
                            {
                                if (string.IsNullOrEmpty(PassFail.Trim())) PassFail = "P";
                                if (txtAccountNo.Text.Trim() == string.Empty)
                                    PassFail = "F";
                            }
                            else
                            {
                                if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                                {
                                    if (txtAccountNo.Text.Trim() == string.Empty)
                                        PassFail = "F";
                                }
                                else
                                    PassFail = "";
                            }
                        }
                        liheapv.LVR_FAILED_ACCOUNT_EDIT = PassFail;




                        liheapv.Mode = Mode;
                        liheapv.LPV_SEQ = propSeq;
                        if (_model.LiheAllData.InsertUpdateDelLiheApv(liheapv))
                        {
                            btnSave.Visible = false;
                            btnCancel.Text = "Close";
                            Mode = "";
                            EnableControl(false);
                            if (Privileges.AddPriv.Equals("false"))
                            {
                                picAddClient.Visible = false;

                            }
                            else
                                picAddClient.Visible = true;
                            gvwSupplierDetails.Enabled = true;
                            boolchangemod = true;
                            fillGridShow();
                        }
                        btnSave.Enabled = true;
                    }
                }

            }
        }

        private void fillGridShow()
        {
            gvwSupplierDetails.SelectionChanged -= new EventHandler(gvwSupplierDetails_SelectionChanged);
            List<LiheApvEntity> liheapvdetails = _model.LiheAllData.GetLiheAppvadpyas(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty, string.Empty);
            //Added by Sudheer on 07/29/2020
            if (liheapvdetails.Count > 0)
                liheapvdetails = liheapvdetails.OrderBy(u => u.LPV_PRIMARY_CODE).ToList();
            gvwSupplierDetails.Rows.Clear();
            string strPayforDesc = string.Empty; int SelInd = 0; int rowIndex = 0;
            foreach (LiheApvEntity item in liheapvdetails)
            {
                CASEVDDEntity vddName = CaseVddlist.Find(u => u.Code.Trim() == item.LPV_VENDOR.Trim());
                if (vddName != null)
                {
                    item.LPV_VENDORNAME = vddName.Name;
                }
                else
                {
                    ////COMMENTED BY SUDHEER ON 11/29/2023 AS PER BRIAN'S MAIL
                    //string strfileAGY = BaseForm.BaseAgencyControlDetails.AgyShortName;
                    //DataTable dtDSSXMLData = DSSXMLData.DSSXMLMID_GET(BaseForm.BaseDSSXMLDBConnString, strfileAGY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, "BYAPPNO");

                    
                    //if (dtDSSXMLData.Rows.Count>0)
                    //{
                    //    System.Data.DataTable dtZipFiles = DSSXMLData.getZippedFiles(BaseForm.BaseDSSXMLDBConnString, strfileAGY, "", "", "C", dtDSSXMLData.Rows[0]["DXM_APPID"].ToString(), "BYAPPNO");
                    //    string XMLData = dtZipFiles.Rows[0]["CTZ_XML_FILE"].ToString().Trim();
                    //    StringReader stringReader = new StringReader(XMLData);

                    //    DataSet dsXMLData = new DataSet();
                    //    if (dtZipFiles.Rows[0]["CTZ_XML_FILE"].ToString() != "")
                    //        dsXMLData.ReadXml(stringReader);
                    //    if (dsXMLData.Tables.Count > 0)
                    //    {
                    //        if (item.LPV_PRIMARY_CODE == "P")
                    //        {
                    //            if (BaseForm.BaseCaseMstListEntity[0].Source == "01" || BaseForm.BaseCaseMstListEntity[0].Source == "03" || BaseForm.BaseCaseMstListEntity[0].Source == "07")
                    //            {
                    //                if (!string.IsNullOrEmpty(dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_primary_heat_source_fuel_dealer__c"].ToString().Trim()))
                    //                {
                    //                    item.LPV_VENDORNAME = dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_primary_heat_source_fuel_dealer__c"].ToString().Trim();
                    //                    //lpventity.DSSLPV_SEQ = "1";

                    //                    //lpventity.DSSLPV_PRIM_CODE = "P";
                    //                    //lpventity.DSSLPV_PAYMENT_FOR = Sourcecd;
                    //                    //CASEVDDEntity VddEntity = propCasevddlist.Find(u => u.Name.ToUpper() == dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_primary_heat_source_fuel_dealer__c"].ToString().Trim().ToUpper());
                    //                    //if (VddEntity != null)
                    //                    //{
                    //                    //    VendorCode = VddEntity.Code;
                    //                    //    lpventity.DSSLPV_VEND_CODE = VendorCode;


                    //                    //    if (!string.IsNullOrEmpty(lpventity.DSSLPV_ACCT_NO.Trim()))
                    //                    //        txtVendorId_TextChanged(txtVendorId, EventArgs.Empty);
                    //                    //}
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (!string.IsNullOrEmpty(dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_your_primary_heat_source_company__c"].ToString().Trim()))
                    //                {
                    //                    item.LPV_VENDORNAME = dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_your_primary_heat_source_company__c"].ToString().Trim();
                    //                    //lpventity.DSSLPV_SEQ = "1";
                    //                    //lpventity.DSSLPV_PRIM_CODE = "P";
                    //                    //lpventity.DSSLPV_PAYMENT_FOR = Sourcecd;
                    //                    //CASEVDDEntity Entity = propCasevddlist.Find(u => u.Name.ToUpper() == dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_your_primary_heat_source_company__c"].ToString().Trim().ToUpper());
                    //                    //if (Entity != null)
                    //                    //{
                    //                    //    VendorCode = Entity.Code;
                    //                    //    lpventity.DSSLPV_VEND_CODE = VendorCode;

                    //                    //    if (!string.IsNullOrEmpty(lpventity.DSSLPV_ACCT_NO.Trim()))
                    //                    //        txtVendorId_TextChanged(txtVendorId, EventArgs.Empty);

                    //                    //}
                    //                }
                    //            }


                    //        }
                    //        else
                    //        {
                    //            if (!string.IsNullOrEmpty(dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_Electric_Company__c"].ToString().Trim()))
                    //                item.LPV_VENDORNAME = dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_Electric_Company__c"].ToString().Trim();
                    //        }
                    //    }


                            

                    //}
                    //else
                        item.LPV_VENDORNAME = "Heat included in rent";
                }

                string AccFormat = string.Empty;
                CaseVDD1Entity vdd1 = Vdd1list.Find(u => u.Code.Trim() == item.LPV_VENDOR.Trim());
                if (vdd1 != null)
                    AccFormat = vdd1.ACCT_FORMAT.Trim();

                CommonEntity payName = proplistPayfor.Find(u => u.Code.Trim() == item.LPV_PAYMENT_FOR.Trim());
                if (payName != null)
                    strPayforDesc = payName.Desc;
                else
                    strPayforDesc = string.Empty;

                if (item.LPV_PRIMARY_CODE == "P")
                    PrimaryPayFor = item.LPV_PAYMENT_FOR.Trim();

                if(PropAcctSoft=="Y")
                    rowIndex = gvwSupplierDetails.Rows.Add(item.LPV_SEQ, (item.LPV_PRIMARY_CODE == "P" ? "Primary Supplier" : "Secondary Supplier"), item.LPV_VENDORNAME, strPayforDesc, item.LPV_ACCOUNT_NO.Trim(), item.LPV_PRIMARY_CODE, item.LPV_PAYMENT_FOR,item.LVR_FAILED_ACCOUNT_EDIT);
                else
                    rowIndex = gvwSupplierDetails.Rows.Add(item.LPV_SEQ, (item.LPV_PRIMARY_CODE == "P" ? "Primary Supplier" : "Secondary Supplier"), item.LPV_VENDORNAME, strPayforDesc, item.LPV_ACCOUNT_NO.Trim(), item.LPV_PRIMARY_CODE, item.LPV_PAYMENT_FOR);
                gvwSupplierDetails.Rows[rowIndex].Tag = item;
                if (item.LPV_PRIMARY_CODE == "P" && boolchangemod == true)
                {
                    BaseForm.BaseCaseMstListEntity[0].Source = item.LPV_PAYMENT_FOR.Trim();
                }
                if (item.LPV_PAYMENT_FOR.Trim() == "02" || item.LPV_PAYMENT_FOR.Trim() == "04")
                {
                    if (item.LPV_REVERIFY == "Y")
                    {
                        gvwSupplierDetails.Rows[rowIndex].Cells["gvtSelected"].Value = Img_Tick;// System.Drawing.Image.FromFile(Application.MapPath(Img_Tick));
                    }
                    else
                    {
                        gvwSupplierDetails.Rows[rowIndex].Cells["gvtSelected"].Value = Img_Blank; // System.Drawing.Image.FromFile(Application.MapPath(Img_Blank));
                    }


                }
                else
                {
                    gvwSupplierDetails.Rows[rowIndex].Cells["gvtSelected"].Value = Img_Blank; // System.Drawing.Image.FromFile(Application.MapPath(Img_Blank));
                }

                if (AccFormat=="03" || AccFormat=="10"|| AccFormat=="11" || AccFormat=="12" ||AccFormat=="13")
                {
                    if (PropAcctSoft == "Y")
                    {
                        DataGridViewCellStyle style = new DataGridViewCellStyle();
                        style.Font = new Font(gvwSupplierDetails.Font, FontStyle.Bold);

                        //gvtAccountNo
                        if (item.LVR_FAILED_ACCOUNT_EDIT == "F")
                        {
                            style.ForeColor = Color.Red;
                            //gvwSupplierDetails.Rows[rowIndex].Cells["gvtAccountNo"].Style = style;
                        }
                        else if (item.LVR_FAILED_ACCOUNT_EDIT == "P")
                        {
                            style.ForeColor = Color.Green;
                            //gvwSupplierDetails.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                        }
                        gvwSupplierDetails.Rows[rowIndex].Cells["gvtAccountNo"].Style = style;
                    }
                }
                if (PropAcctSoft == "Y" && item.LPV_PRIMARY_CODE == "P")
                    SelInd = rowIndex;
                CommonFunctions.setTooltip(rowIndex, item.LPV_ADD_OPERATOR, item.LPV_DATE_ADD, item.LPV_LSTC_OPERATOR, item.LPV_DATE_LSTC, gvwSupplierDetails);
            }

            if (gvwSupplierDetails.Rows.Count > 0)
            {
                gvwSupplierDetails.Rows[0].Selected = true;
                if (PropAcctSoft == "Y" && PrimaryPayFor == "04")  picAddClient.Visible = false;  else picAddClient.Visible = true;
                if (PropAcctSoft == "Y") { this.gvPassFail.Visible = true; this.gvtSelected.Visible = false; } else { this.gvPassFail.Visible = false;this.gvtSelected.Visible = true; }
            }
            else { picAddClient.Visible = true; PrimaryPayFor = string.Empty; }

            gvwSupplierDetails.SelectionChanged += new EventHandler(gvwSupplierDetails_SelectionChanged);
            gvwSupplierDetails_SelectionChanged(gvwSupplierDetails, new EventArgs());

        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (lblPayforReq.Visible && ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbPayfor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPayfor.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbPayfor, null);
            }


            if (BaseForm.BaseCaseMstListEntity[0].HeatIncRent.Trim() == "2" && gvwSupplierDetails.Rows.Count == 0)
            {
                _errorProvider.SetError(txtVendorId, null);
            }
            else
            {
                if (BaseForm.BaseCaseMstListEntity[0].HeatIncRent.Trim() == "2" && ((ListItem)cmbSupplierType.SelectedItem).Value.ToString().ToUpper().Trim() == "P")
                {
                    _errorProvider.SetError(txtVendorId, null);
                }
                else
                {
                    if (lblvendorReq.Visible && String.IsNullOrEmpty(txtVendorId.Text.Trim()))
                    {
                        _errorProvider.SetError(txtVendorId, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVendor.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {

                        _errorProvider.SetError(txtVendorId, null);
                    }
                }
            }
            if (lblBillingReq.Visible && ((ListItem)cmbBilling.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbBilling, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBillingName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbBilling, null);
            }

            if (lblAccountReq.Visible && String.IsNullOrEmpty(txtAccountNo.Text.Trim()))
            {
                _errorProvider.SetError(txtAccountNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccount.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtAccountNo, null);
            }

            if (((ListItem)cmbSupplierType.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbSupplierType, "Supplier Type is Require");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbSupplierType, null);
            }

            if (Mode.ToUpper() == "ADD")
            {
                if (gvwSupplierDetails.Rows.Count > 0)
                {
                    bool boolprimaryexist = false;
                    foreach (DataGridViewRow item in gvwSupplierDetails.Rows)
                    {
                        if (item.Cells["gvtPrimaryCode"].Value.ToString().ToUpper().Trim() == "P")
                        {
                            boolprimaryexist = true;
                            break;
                        }
                    }
                    if (boolprimaryexist)
                    {
                        if (((ListItem)cmbSupplierType.SelectedItem).Value.ToString() == "P")
                        {
                            _errorProvider.SetError(cmbSupplierType, "Only One Primary Supplier allowed");
                            isValid = false;
                        }
                        else
                        {
                            if (((ListItem)cmbSupplierType.SelectedItem).Value.ToString() == "0")
                            {
                                _errorProvider.SetError(cmbSupplierType, "Supplier Type is Require");
                                isValid = false;
                            }
                            else
                            {
                                _errorProvider.SetError(cmbSupplierType, null);
                            }
                        }
                    }
                }
            }



            //if (Mode.ToUpper() == "ADD")
            //{
            //    if (gvwSupplierDetails.Rows.Count > 0)
            //    {
            //        bool boolprimaryexist = false;
            //        foreach (DataGridViewRow item in gvwSupplierDetails.Rows)
            //        {
            //            if (item.Cells["gvtPrimaryCode"].Value.ToString().ToUpper().Trim() == "P")
            //            {
            //                boolprimaryexist = true;
            //                break;
            //            }
            //        }
            //        if (boolprimaryexist)
            //        {
            //            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() != "04")
            //            {
            //                _errorProvider.SetError(cmbPayfor, "The Secondary vendor must ALWAYS be ELECTRIC");
            //                isValid = false;
            //            }
            //            else
            //            {
            //                _errorProvider.SetError(cmbPayfor, null);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (Mode.ToUpper() == "EDIT")
            //    {
            //        if (gvwSupplierDetails.Rows.Count > 1)
            //        {
            //            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() != "04" && txtPrimary.Text.ToUpper() == "S")
            //            {
            //                _errorProvider.SetError(cmbPayfor, "The Secondary vendor must ALWAYS be ELECTRIC");
            //                isValid = false;
            //            }
            //            else
            //            {
            //                _errorProvider.SetError(cmbPayfor, null);
            //            }
            //        }
            //    }
            //}

            return (isValid);
        }

        private bool PayValidateForm()
        {
            bool isValid = true;

            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbPayfor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPayfor.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbPayfor, null);
            }
            return isValid;
        }

        private void picAddScale_Click(object sender, EventArgs e)
        {
            Mode = "Add";

            txtAccountNo.Text = string.Empty;
            txtFirst.Text = string.Empty;
            txtLast.Text = string.Empty;
            txtVendorId.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            txtPrimary.Text = string.Empty;
            lblMsg.Visible = false;
            fillcmbPayfor();
            if (gvwSupplierDetails.Rows.Count == 0)
            {
                if (BaseForm.BaseCaseMstListEntity[0].Source.Trim() != string.Empty)
                {
                    CommonFunctions.SetComboBoxValue(cmbPayfor, BaseForm.BaseCaseMstListEntity[0].Source.Trim().ToString());
                }

            }
            
            cmbBilling.SelectedIndex = 0;
            cmbSupplierType.SelectedIndex = 0;
            propSeq = string.Empty;
            btnSave.Visible = true;
            btnCancel.Text = "Cancel";
            EnableControl(false);
            EnableDisableControls();
            chkReverify.Enabled = false;
            chkReverify.Checked = true;
            lblReverified.Text = string.Empty;
            if (PropAcctSoft == "Y") { chkReverify.Visible = false;lblReverified.Visible = false; }
            DisplayVendorName();

            //Added by sudheer on 07/31/2020 as per the document Master Rules for changes to 2001 and tms81
            if (PropAcctSoft == "Y" && (PrimaryPayFor=="02" || PrimaryPayFor == "01" || PrimaryPayFor == "03" || PrimaryPayFor == "05" || PrimaryPayFor == "06" || PrimaryPayFor == "07" || PrimaryPayFor == "09"))
            {
                CommonFunctions.SetComboBoxValue(cmbPayfor, "04");
                cmbPayfor.Enabled = false;
                CommonFunctions.SetComboBoxValue(cmbSupplierType, "S");
            }
            else cmbPayfor.Enabled = true;

            cmbPayfor.Focus();
            gvwSupplierDetails.Enabled = false;


        }

        private void ClearControls()
        {
            txtAccountNo.Text = string.Empty;
            txtFirst.Text = string.Empty;
            txtLast.Text = string.Empty;
            txtVendorId.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            cmbPayfor.SelectedIndex = 0;
            cmbBilling.SelectedIndex = 0;
            cmbSupplierType.SelectedIndex = 0;
        }

        public void DisplayVendorName()
        {

            if (BaseForm.BaseCaseMstListEntity[0].HeatIncRent == "2")
            {
                lblvendorReq.Visible = false;
                lblVendor.Visible = false;
                btnvendor.Visible = false;
                txtVendorId.Visible = false;
                txtVendorName.Text = "Heat included in rent";
            }
            else
            {
                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.Tms00201.Vendor:
                            if (enabled) { if (required) lblvendorReq.Visible = true; } else { lblvendorReq.Visible = false; }
                            break;
                    }
                }
                lblVendor.Visible = true;
                btnvendor.Visible = true;
                txtVendorId.Visible = true;
                txtVendorName.Text = "";
            }
        }

        private void EnableControl(bool boolfalse)
        {
            txtAccountNo.Enabled = boolfalse;
            cmbBilling.Enabled = boolfalse;
            cmbPayfor.Enabled = boolfalse;
            cmbSupplierType.Enabled = boolfalse;
            btnvendor.Enabled = boolfalse;
            chkReverify.Enabled = boolfalse;
            if (boolfalse == false)
                chkReverify.Enabled = false;
        }

        private void gvwSupplierDetails_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwSupplierDetails.Rows.Count > 0)
            {
                cmbPayfor.SelectedIndexChanged -= new EventHandler(cmbPayfor_SelectedIndexChanged);
                cmbSupplierType.SelectedIndexChanged -= new EventHandler(cmbSupplierType_SelectedIndexChanged);

                foreach (DataGridViewRow item in gvwSupplierDetails.Rows)
                {
                    if (item.Selected)
                    {
                        EnableControl(false);
                        LiheApvEntity liheapvdetails = item.Tag as LiheApvEntity;
                        if (liheapvdetails != null)
                        {
                            txtFirst.Text = liheapvdetails.LPV_BILL_FNAME;
                            txtLast.Text = liheapvdetails.LPV_BILL_LNAME;
                            txtVendorId.Text = liheapvdetails.LPV_VENDOR;
                            txtVendorName.Text = liheapvdetails.LPV_VENDORNAME;
                            txtAccountNo.Text = liheapvdetails.LPV_ACCOUNT_NO.Trim();
                            txtPrimary.Text = liheapvdetails.LPV_PRIMARY_CODE.ToString();
                            propSeq = liheapvdetails.LPV_SEQ;
                            strpayfor = liheapvdetails.LPV_PAYMENT_FOR;
                            if (liheapvdetails.LPV_PAYMENT_FOR == "02" || liheapvdetails.LPV_PAYMENT_FOR == "04")
                            {
                                chkReverify.Visible = true;
                                if (liheapvdetails.LPV_REVERIFY == "Y")
                                {
                                    chkReverify.Checked = true;
                                    lblReverified.Text = "Re Verified By " + liheapvdetails.LPV_VERIFIED_BY + " On " + LookupDataAccess.Getdate(liheapvdetails.LPV_VERIFY_DATE);
                                    lblReverified.ForeColor = System.Drawing.Color.Green;
                                }
                                else
                                {
                                    chkReverify.Checked = false;
                                    lblReverified.Text = "Not Re Verified. ";
                                    lblReverified.ForeColor = System.Drawing.Color.Red;
                                }
                                if (PropAcctSoft == "Y") { chkReverify.Visible = false; lblReverified.Visible = false; }
                            }
                            else
                            {
                                chkReverify.Visible = false;
                                lblReverified.Text = string.Empty;
                            }

                            if ((gvwSupplierDetails.Rows[0].Cells["gvPassFail"].Value.ToString() == "P" || gvwSupplierDetails.Rows[0].Cells["gvPassFail"].Value.ToString().Trim() == ""))
                                lblMsg.Visible = false;
                            else
                                lblMsg.Visible = true;

                            CommonFunctions.SetComboBoxValue(cmbPayfor, liheapvdetails.LPV_PAYMENT_FOR);
                            CommonFunctions.SetComboBoxValue(cmbSupplierType, liheapvdetails.LPV_PRIMARY_CODE);
                            CommonFunctions.SetComboBoxValue(cmbBilling, "0");
                            if (liheapvdetails.LPV_BILLNAME_TYPE == "T")
                            {
                                CommonFunctions.SetComboBoxValue(cmbBilling, "T");
                                if (Mode != "Add" && Mode != "Edit")
                                {
                                    txtFirst.Enabled = false;
                                    txtLast.Enabled = false;
                                }
                            }
                            else
                            {
                                CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == liheapvdetails.LPV_BILL_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == liheapvdetails.LPV_BILL_LNAME.Trim().ToUpper());
                                if (casesnp != null)
                                    CommonFunctions.SetComboBoxValue(cmbBilling, liheapvdetails.LPV_BILL_FNAME.Trim().ToUpper() + liheapvdetails.LPV_BILL_LNAME.Trim().ToUpper());                                
                            }
                        }
                    }

                }
                
                cmbPayfor.SelectedIndexChanged += new EventHandler(cmbPayfor_SelectedIndexChanged);
                cmbSupplierType.SelectedIndexChanged += new EventHandler(cmbSupplierType_SelectedIndexChanged);
                txtVendorId_TextChanged(sender, e);
            }
            else
            {
                txtAccountNo.Text = string.Empty;
                txtFirst.Text = string.Empty;
                txtLast.Text = string.Empty;
                txtVendorId.Text = string.Empty;
                txtVendorName.Text = string.Empty;
                lblReverified.Text = string.Empty;
                fillcmbPayfor();
                if (gvwSupplierDetails.Rows.Count == 0)
                {
                    if (BaseForm.BaseCaseMstListEntity[0].Source.Trim() != string.Empty)
                    {
                        CommonFunctions.SetComboBoxValue(cmbPayfor, BaseForm.BaseCaseMstListEntity[0].Source.Trim().ToString());
                    }

                }
                cmbBilling.SelectedIndex = 0;

            }
        }

        private void gvwSupplierDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gviEdit.Index && e.RowIndex != -1)
            {
                Mode = "Edit";
                EnableControl(false);
                EnableDisableControls();
                cmbSupplierType.Enabled = false;
                picAddClient.Visible = false;
                btnSave.Visible = true;
                btnCancel.Text = "Cancel";
                string PrimaryCode = string.Empty;
                foreach (DataGridViewRow item in gvwSupplierDetails.Rows)
                {
                    if (item.Selected)
                    {
                        LiheApvEntity liheapvdetails = item.Tag as LiheApvEntity;
                        if (liheapvdetails != null)
                        {
                            PrimaryCode = liheapvdetails.LPV_PRIMARY_CODE;
                            if (liheapvdetails.LPV_REVERIFY == "Y")
                            {
                                chkReverify.Enabled = false;
                            }
                            else
                            {
                                chkReverify.Enabled = true;
                            }

                            if (PropAcctSoft == "Y") { chkReverify.Visible = false; lblReverified.Visible = false; }
                        }
                    }
                }

                if (Mode == "Edit" && ((ListItem)cmbBilling.SelectedItem).Value.ToString() == "T")
                {
                    txtFirst.Enabled = true;
                    txtLast.Enabled = true;
                }
                if (PropAcctSoft == "Y" && PrimaryCode!="P" && (PrimaryPayFor == "01" || PrimaryPayFor == "02" || PrimaryPayFor == "03" || PrimaryPayFor == "05" || PrimaryPayFor == "06" || PrimaryPayFor == "07" || PrimaryPayFor == "09"))
                {
                    CommonFunctions.SetComboBoxValue(cmbPayfor, "04");
                    cmbPayfor.Enabled = false;
                }
                else cmbPayfor.Enabled = true;

                cmbPayfor.Focus();
                gvwSupplierDetails.Enabled = false;

            }
            else if (e.ColumnIndex == gviDel.Index && e.RowIndex != -1)
            {

                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: DeleteClientSupplier);

            }
        }

        private void DeleteClientSupplier(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                if (DialogResult.Yes == dialogResult)
                {
                    LiheApvEntity liheapv = new LiheApvEntity();
                    liheapv.LPV_AGENCY = BaseForm.BaseAgency;
                    liheapv.LPV_DEPT = BaseForm.BaseDept;
                    liheapv.LPV_PROGRAM = BaseForm.BaseProg;
                    liheapv.LPV_YEAR = BaseForm.BaseYear;
                    liheapv.LPV_APP_NO = BaseForm.BaseApplicationNo;
                    liheapv.Mode = "Delete";
                    liheapv.LPV_SEQ = propSeq;
                    liheapv.LPV_VENDOR = txtVendorId.Text;
                    if (_model.LiheAllData.InsertUpdateDelLiheApv(liheapv))
                    {
                        btnSave.Visible = false;
                        EnableControl(false);
                        gvwSupplierDetails.Enabled = true;
                        fillGridShow();
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("An open payment exists for vendor # " + txtVendorId.Text + " . Supplier record may not be deleted. ");
                    }

                }
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Close")
            {
                this.Close();
            }
            else
            {
                _errorProvider.SetError(cmbPayfor, null);
                _errorProvider.SetError(cmbBilling, null);
                _errorProvider.SetError(txtVendorId, null);
                _errorProvider.SetError(txtAccountNo, null);
                _errorProvider.SetError(cmbSupplierType, null);
                if (Privileges.AddPriv.Equals("false"))
                {
                    picAddClient.Visible = false;

                }
                else
                    picAddClient.Visible = true;
                btnSave.Visible = false;
                btnCancel.Text = "Close";
                Mode = "";
                EnableControl(false);
                gvwSupplierDetails.Enabled = true;
                fillGridShow();
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            if (gvwSupplierDetails.Rows.Count > 0)
            {
                if (!(Privileges.ChangePriv.Equals("false")))
                {
                    contextMenu1.MenuItems.Clear();
                    foreach (DataGridViewRow item in gvwSupplierDetails.Rows)
                    {
                        if (item.Selected)
                        {
                            propSeq = item.Cells["gvtSeq"].Value.ToString();
                            if (item.Cells["gvtPrimaryCode"].Value.ToString() != "P") //&& item.Cells["gvtPayforcode"].Value.ToString() != "04"
                            {

                                MenuItem menuLst = new MenuItem();
                                menuLst.Text = "Primary Supplier";
                                menuLst.Tag = "P";
                                contextMenu1.MenuItems.Add(menuLst);
                                break;
                            }
                            //else {
                            //    MenuItem menuLst = new MenuItem();
                            //    menuLst.Text = "Secondary Supplier";
                            //    menuLst.Tag = "A";
                            //    contextMenu1.MenuItems.Add(menuLst);
                            //    break;
                            //}
                        }
                    }
                }
                else
                {
                    contextMenu1.MenuItems.Clear();
                }
            }
        }

        private void gvwSupplierDetails_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (gvwSupplierDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in gvwSupplierDetails.Rows)
                {
                    if (item.Selected)
                    {
                        LiheApvEntity liheapvdetails = item.Tag as LiheApvEntity;
                        LiheApvEntity liheapv = new LiheApvEntity();
                        liheapv.LPV_AGENCY = BaseForm.BaseAgency;
                        liheapv.LPV_DEPT = BaseForm.BaseDept;
                        liheapv.LPV_PROGRAM = BaseForm.BaseProg;
                        liheapv.LPV_YEAR = BaseForm.BaseYear;
                        liheapv.LPV_APP_NO = BaseForm.BaseApplicationNo;
                        liheapv.LPV_PRIMARY_CODE = "P";
                        liheapv.LPV_LSTC_OPERATOR = BaseForm.UserID;
                        liheapv.LPV_PAYMENT_FOR = liheapvdetails.LPV_PAYMENT_FOR.ToString();
                        liheapv.Mode = "Primary";
                        liheapv.LPV_SEQ = propSeq;
                        if (_model.LiheAllData.InsertUpdateDelLiheApv(liheapv))
                        {
                            btnSave.Visible = false;
                            EnableControl(false);
                            gvwSupplierDetails.Enabled = true;
                            boolchangemod = true;
                            fillGridShow();
                        }
                    }
                }

            }
        }

        private bool CheckAccountNo()
        {
            bool boolAccount = true;
            string strAccountFormat = string.Empty;
            string strValidate10ACNo = string.Empty;
            string strValidate10ACNo2 = string.Empty;
            _errorProvider.SetError(txtAccountNo, null);
            double validNumber;
            if (propProgramDefinition != null)
            {
                CaseVDD1Entity vdd1entity = Vdd1list.Find(u => u.Code == txtVendorId.Text);
                if (vdd1entity != null)
                {
                    strAccountFormat = vdd1entity.ACCT_FORMAT.ToString();
                }
                if (propProgramDefinition.State.ToString().ToUpper() == "RI")
                {
                }
                else if (propProgramDefinition.State.ToString().ToUpper() == "CT")
                {

                    if (propAccountNo.Length >= 14)
                    {
                        strValidate10ACNo = propAccountNo.Substring(0, 14);
                    }
                    if (strAccountFormat == "10")
                    {
                        //if (propAccountNo.Length >= 14)
                        //{
                        //    string s = propAccountNo.Substring(0, 3);
                        //    string st = propAccountNo.Substring(3, 1);
                        //    string st1 = propAccountNo.Substring(4, 7);
                        //    string st23 = propAccountNo.Substring(11, 1);
                        //    string sfdf = propAccountNo.Substring(12, 4);
                        //    string sffds = propAccountNo.Substring(16, 4);
                        //    if (propAccountNo.Length >= 20)
                        //    {
                        //        if (double.TryParse(strValidate10ACNo, out validNumber) && propAccountNo.Substring(14, 6).Trim() == string.Empty)
                        //        {
                        //            strValidate10ACNo = strValidate10ACNo;
                        //            boolAccount = ValidationAccount("10", propAccountNo, string.Empty, true);
                        //        }
                        //        else if (double.TryParse(propAccountNo.Substring(0, 3), out validNumber) && propAccountNo.Substring(3, 1) == "-" && double.TryParse(propAccountNo.Substring(4, 7), out validNumber) && propAccountNo.Substring(11, 1) == "-" && double.TryParse(propAccountNo.Substring(12, 4), out validNumber) && propAccountNo.Substring(16, 4).Trim() == string.Empty)
                        //        {
                        //            propAccountNo = propAccountNo.Substring(0, 3) + propAccountNo.Substring(4, 7) + propAccountNo.Substring(12, 4) + propAccountNo.Substring(16, 4);
                        //            boolAccount = ValidationAccount("10", propAccountNo, string.Empty, true);
                        //        }
                        //        else
                        //        {
                        //            boolAccount = ValidationAccount("10", propAccountNo, string.Empty, false);
                        //        }
                        //    }
                        //}
                        //else
                        //    boolAccount = ValidationAccount("10", propAccountNo, "01", false);
                    }
                    else if (strAccountFormat == "11")
                    {
                        //if (propAccountNo.Length >= 14)
                        //{
                        //    if (double.TryParse(strValidate10ACNo, out validNumber))
                        //    {
                        //        boolAccount = ValidationAccount("11", propAccountNo, string.Empty, true);
                        //    }
                        //    else
                        //    {
                        //        boolAccount = ValidationAccount("11", propAccountNo, string.Empty, false);
                        //    }
                        //}
                        //else
                        //    boolAccount = ValidationAccount("11", propAccountNo, "01", false);
                    }
                    else if (strAccountFormat == "12")
                    {
                        if (propAccountNo.Length >= 14)
                        {
                            if (double.TryParse(strValidate10ACNo, out validNumber))
                            {
                                boolAccount = ValidationAccount("12", propAccountNo, string.Empty, true);
                            }
                            else
                            {
                                boolAccount = ValidationAccount("12", propAccountNo, "01", false);
                            }
                        }
                        else
                            boolAccount = ValidationAccount("12", propAccountNo, "01", false);
                    }
                    else if (strAccountFormat == "13")
                    {
                        if (propAccountNo.Length >= 14)
                        {
                            if (double.TryParse(strValidate10ACNo, out validNumber))
                            {
                                boolAccount = ValidationAccount("13", propAccountNo, string.Empty, true);
                            }
                            else
                            {
                                boolAccount = ValidationAccount("13", propAccountNo, "01", false);
                            }
                        }
                        else
                            boolAccount = ValidationAccount("13", propAccountNo, "01", false);
                    }
                    else if (strAccountFormat == "03")
                    {
                          boolAccount = ValidationAccount("03", propAccountNo, string.Empty,true);
                    }
                    else
                    {
                        //boolAccount = ValidationAccount("", propAccountNo, string.Empty);
                    }

                    //  }
                }
            }
            if (boolAccount == false)
            {
                btnSave.Enabled = true;
            }

            return boolAccount;
        }

        private bool ValidationAccount(string strFormat, string AccountNo, string strerr, bool boolValue)
        {
            bool boolvalidaccount = true;
            int validNo;
            double validNumber;
            if (strFormat == "10")
            {
                if (propAccountNo.Length >= 3)
                {
                    if (double.TryParse(propAccountNo.Substring(0, 3).ToString(), out validNumber))
                    {
                        validNo = Convert.ToInt32(propAccountNo.Substring(0, 3));
                        if (validNo >= 040 && validNo <= 049)
                        {
                            if (propAccountNo.Length >= 14 && boolValue == true)
                            {
                                if (!(CalculationAccount(AccountNo)))
                                {
                                    //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                                    if (PropAcctSoft == "Y" && (strFormat== "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                                    {
                                        PassFail = "F";
                                    }
                                    else
                                    {
                                        _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                                        boolvalidaccount = false;
                                    }
                                }
                            }
                            else
                            {
                                //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                                if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                                {
                                    PassFail = "F";
                                }
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "Invalid Format");
                                    boolvalidaccount = false;
                                }
                            }

                        }
                        else
                        {
                            //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))

                            if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                            {
                                PassFail = "F";
                            }
                            else
                            {
                                _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 040 - 049 ");
                                boolvalidaccount = false;
                            }
                        }
                    }
                    else
                    {
                        //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                        {
                            PassFail = "F";
                        }
                        else
                        {
                            _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 040 - 049 ");
                            boolvalidaccount = false;
                        }
                    }
                }
                else
                {
                    //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                    if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                    {
                        PassFail = "F";
                    }
                    else
                    {
                        _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 040 - 049 ");
                        boolvalidaccount = false;
                    }
                }
            }
            else if (strFormat == "11")
            {
                if (propAccountNo.Length >= 3)
                {
                    if (double.TryParse(propAccountNo.Substring(0, 3).ToString(), out validNumber))
                    {
                        validNo = Convert.ToInt32(propAccountNo.Substring(0, 3));
                        if (validNo >= 050 && validNo <= 059)
                        {
                            string straccountno = propAccountNo;
                            if (straccountno.Trim().Length == 14)
                            {
                                if (propAccountNo.Length >= 14 && boolValue == true)
                                {
                                    if (!(CalculationAccount(AccountNo)))
                                    {
                                        //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                                        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                                        {
                                            PassFail = "F";
                                        }
                                        else
                                        {
                                            _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                                            boolvalidaccount = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                                    if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                                    {
                                        PassFail = "F";
                                    }
                                    else
                                    {
                                        _errorProvider.SetError(txtAccountNo, "Invalid Format");
                                        boolvalidaccount = false;
                                    }
                                }
                            }
                            else
                            {
                                //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                                if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                                {
                                    PassFail = "F";
                                }
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "Invalid Format");
                                    boolvalidaccount = false;
                                }
                            }
                        }
                        else
                        {
                            //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                            if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                            {
                                PassFail = "F";
                            }
                            else
                            {
                                _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 050 - 059 ");
                                boolvalidaccount = false;
                            }
                        }
                    }
                    else
                    {
                        //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                        {
                            PassFail = "F";
                        }
                        else
                        {
                            _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 050 - 059 ");
                            boolvalidaccount = false;
                        }
                    }
                }
                else
                {
                    //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                    if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                    {
                        PassFail = "F";
                    }
                    else
                    {
                        _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 050 - 059 ");
                        boolvalidaccount = false;
                    }
                }
            }
            else if (strFormat == "12")
            {
                if (propAccountNo.Trim().Length == 14 && boolValue == true)
                {
                    if (propAccountNo.Trim().Length >= 2)
                    {
                        //if (double.TryParse(propAccountNo.Substring(0, 2).ToString(), out validNumber))
                        //{
                            string validNo1 = (propAccountNo.Trim().Substring(0, 2));
                        if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                        {
                            if (validNo1 != "01")
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "12"))
                                    PassFail = "F";
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "Invalid Format");
                                    boolvalidaccount = false;
                                }
                            }
                            else
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "12"))
                                    PassFail = "P";
                            }

                        }
                        else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                        {
                            //string AGY_A_Name = string.Empty;
                            //if (AgyList.Rows.Count > 0)
                            //{
                            //    foreach (DataRow dr in AgyList.Rows)
                            //    {
                            //        if (!string.IsNullOrEmpty(dr["AGY_9"].ToString().Trim()))
                            //        {
                            //            if (dr["AGY_9"].ToString().Substring(0, 10).Trim() == txtVendorId.Text.Trim())
                            //            {
                            //                AGY_A_Name = dr["AGY_9"].ToString().Substring(10, (dr["AGY_9"].ToString().Length - 10)).Trim();
                            //            }
                            //        }
                            //    }
                            //}

                            if (validNo1 != "05")
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "12"))
                                    PassFail = "F";
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "Invalid Format");
                                    boolvalidaccount = false;
                                }
                            }
                            else
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "12"))
                                    PassFail = "P";
                            }
                        }
                            //else
                            //{
                            //    if (!(CalculationAccount(AccountNo)))
                            //    {
                            //        //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                            //        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                            //        {
                            //            PassFail = "F";
                            //        }
                            //        else
                            //        {
                            //            _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                            //            boolvalidaccount = false;
                            //        }
                            //    }
                            //}
                        //}
                    }
                    else
                    {
                        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                        {
                            PassFail = "F";
                        }
                        else
                        {
                            _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                            boolvalidaccount = false;
                        }
                    }
                }
                else
                {
                    if (PropAcctSoft == "Y" && (strFormat == "12"))
                    {
                        PassFail = "F";
                    }
                    else
                    {
                        _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                        boolvalidaccount = false;
                    }
                }


                //if (propAccountNo.Length >= 3)
                //{
                //    if (double.TryParse(propAccountNo.Substring(0, 3).ToString(), out validNumber))
                //    {
                //        validNo = Convert.ToInt32(propAccountNo.Substring(0, 3));
                //        if (validNo >= 010 && validNo <= 019)
                //        {
                //            if (propAccountNo.Length >= 14 && boolValue == true)
                //            {
                //                if (!(CalculationAccount(AccountNo)))
                //                {
                //                    //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                //                    if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                //                    {
                //                        PassFail = "F";
                //                    }
                //                    else
                //                    {
                //                        _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                //                        boolvalidaccount = false;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                //                if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                //                {
                //                    PassFail = "F";
                //                }
                //                else
                //                {
                //                    _errorProvider.SetError(txtAccountNo, "Invalid Format");
                //                    boolvalidaccount = false;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                //            if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                //            {
                //                PassFail = "F";
                //            }
                //            else
                //            {
                //                _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 010 - 019 ");
                //                boolvalidaccount = false;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                //        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                //        {
                //            PassFail = "F";
                //        }
                //        else
                //        {
                //            _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 010 - 019 ");
                //            boolvalidaccount = false;
                //        }
                //    }
                //}
                //else
                //{
                //    //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                //    if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                //    {
                //        PassFail = "F";
                //    }
                //    else
                //    {
                //        _errorProvider.SetError(txtAccountNo, "1 - 3 Must be 010 - 019 ");
                //        boolvalidaccount = false;
                //    }
                //}
            }
            else if (strFormat == "13")
            {
                if (propAccountNo.Trim().Length == 14 && boolValue == true)
                {
                    if (propAccountNo.Trim().Length >= 2)
                    {
                        //if (double.TryParse(propAccountNo.Substring(0, 2).ToString(), out validNumber))
                        //{
                           string validNo1 = (propAccountNo.Trim().Substring(0, 2));
                        if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                        {
                            if (validNo1 != "01")
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "13"))
                                    PassFail = "F";
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "Invalid Format");
                                    boolvalidaccount = false;
                                }
                            }
                            else
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "13"))
                                    PassFail = "P";
                            }


                        }
                        else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                        {
                            if (validNo1 != "04")
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "13"))
                                    PassFail = "F";
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "Invalid Format");
                                    boolvalidaccount = false;
                                }
                            }
                            else
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "13"))
                                    PassFail = "P";
                            }
                        }
                            //else
                            //{
                            //    if (!(CalculationAccount(AccountNo)))
                            //    {
                            //        //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                            //        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
                            //        {
                            //            PassFail = "F";
                            //        }
                            //        else
                            //        {
                            //            _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                            //            boolvalidaccount = false;
                            //        }
                            //    }
                            //}
                        //}
                    }
                    else
                    {
                        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "13" || strFormat == "03"))
                        {
                            PassFail = "F";
                        }
                        else
                        {
                            _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                            boolvalidaccount = false;
                        }
                    }
                }
                else
                {
                    if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" ||strFormat==""|| strFormat == "13" || strFormat == "03"))
                    {
                        PassFail = "F";
                    }
                    else
                    {
                        _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                        boolvalidaccount = false;
                    }
                }

            }
            else if (strFormat == "03")
            {
                //Added by Sudheer on 08/02/2020 based on the TMS201_AcctNo_Validation
                if(propAccountNo.Trim().Length==11)
                {
                    if (double.TryParse(propAccountNo.Trim().Substring(0, 2).ToString(), out validNumber))
                    {
                        validNo = Convert.ToInt32(propAccountNo.Trim().Substring(0, 2));
                        if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                        {
                            if(validNo==51)
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "03"))
                                {
                                    PassFail = "P";
                                }
                            }
                            else
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "03"))
                                {
                                    PassFail = "F";
                                }
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "1 - 2 Must be 51 ");
                                    boolvalidaccount = false;
                                }
                            }
                        }
                        else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                        {
                            if (validNo == 57)
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "03"))
                                {
                                    PassFail = "P";
                                }
                            }
                            else
                            {
                                if (PropAcctSoft == "Y" && (strFormat == "03"))
                                {
                                    PassFail = "F";
                                }
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "1 - 2 Must be 57 ");
                                    boolvalidaccount = false;
                                }
                            }
                        }
                        else
                        {
                            if (!(CalculationAccount(AccountNo)))
                            {
                                //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                                if (PropAcctSoft == "Y" && (strFormat == "03"))
                                {
                                    PassFail = "F";
                                }
                                else
                                {
                                    _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
                                    boolvalidaccount = false;
                                }
                            }
                        }

                    }
                }
                else
                {
                    if (PropAcctSoft == "Y" && (strFormat == "03"))
                    {
                        PassFail = "F";
                    }
                    else
                    {
                        _errorProvider.SetError(txtAccountNo, "Account Number length must be 11 digits");
                        boolvalidaccount = false;
                    }
                }


            //    if (propAccountNo.Length >= 14)
            //    {
            //        if (!(CalculationAccount(AccountNo)))
            //        {
            //            //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
            //            if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
            //            {
            //                PassFail = "F";
            //            }
            //            else
            //            {
            //                _errorProvider.SetError(txtAccountNo, "Invalid Acct Number");
            //                boolvalidaccount = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
            //        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))
            //        {
            //            PassFail = "F";
            //        }
            //        else
            //        {
            //            _errorProvider.SetError(txtAccountNo, "Invalid Format");
            //            boolvalidaccount = false;
            //        }
            //    }

            //}
            //else
            //{
            //    if (strerr == "01")
            //    {
            //        if (PropAcctSoft == "Y" && (strFormat == "10" || strFormat == "11" || strFormat == "12" || strFormat == "03"))//if (PropAcctSoft == "Y" && (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02" || ((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
            //        {
            //            PassFail = "F";
            //        }
            //        else
            //        {
            //            _errorProvider.SetError(txtAccountNo, "Invalid Format");
            //            boolvalidaccount = false;
            //        }
            //    }
            //    else
            //    {

            //    }
            }

            return boolvalidaccount;

        }

        private bool CalculationAccount(string strAccountNo)
        {
            bool boolvalidAccount = false;
            int a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14;
            int L1, l2, l3, l4, l5, l7, l8;
            decimal l6;
            string l2value = "2121212121212";
            if (strAccountNo.Length >= 14)
            {
                a1 = Convert.ToInt32(strAccountNo.Substring(0, 1)) * Convert.ToInt32(l2value.Substring(0, 1));
                if (a1 > 9)
                {
                    a1 = Convert.ToInt32(a1.ToString().Substring(0, 1)) + Convert.ToInt32(a1.ToString().Substring(1, 1));
                }
                a2 = Convert.ToInt32(strAccountNo.Substring(1, 1)) * Convert.ToInt32(l2value.Substring(1, 1));
                if (a2 > 9)
                {
                    a2 = Convert.ToInt32(a2.ToString().Substring(0, 1)) + Convert.ToInt32(a2.ToString().Substring(1, 1));
                }
                a3 = Convert.ToInt32(strAccountNo.Substring(2, 1)) * Convert.ToInt32(l2value.Substring(2, 1));
                if (a3 > 9)
                {
                    a3 = Convert.ToInt32(a3.ToString().Substring(0, 1)) + Convert.ToInt32(a3.ToString().Substring(1, 1));
                }
                a4 = Convert.ToInt32(strAccountNo.Substring(3, 1)) * Convert.ToInt32(l2value.Substring(3, 1));
                if (a4 > 9)
                {
                    a4 = Convert.ToInt32(a4.ToString().Substring(0, 1)) + Convert.ToInt32(a4.ToString().Substring(1, 1));
                }
                a5 = Convert.ToInt32(strAccountNo.Substring(4, 1)) * Convert.ToInt32(l2value.Substring(4, 1));
                if (a5 > 9)
                {
                    a5 = Convert.ToInt32(a5.ToString().Substring(0, 1)) + Convert.ToInt32(a5.ToString().Substring(1, 1));
                }
                a6 = Convert.ToInt32(strAccountNo.Substring(5, 1)) * Convert.ToInt32(l2value.Substring(5, 1));
                if (a6 > 9)
                {
                    a6 = Convert.ToInt32(a6.ToString().Substring(0, 1)) + Convert.ToInt32(a6.ToString().Substring(1, 1));
                }
                a7 = Convert.ToInt32(strAccountNo.Substring(6, 1)) * Convert.ToInt32(l2value.Substring(6, 1));
                if (a7 > 9)
                {
                    a7 = Convert.ToInt32(a7.ToString().Substring(0, 1)) + Convert.ToInt32(a7.ToString().Substring(1, 1));
                }
                a8 = Convert.ToInt32(strAccountNo.Substring(7, 1)) * Convert.ToInt32(l2value.Substring(7, 1));
                if (a8 > 9)
                {
                    a8 = Convert.ToInt32(a8.ToString().Substring(0, 1)) + Convert.ToInt32(a8.ToString().Substring(1, 1));
                }
                a9 = Convert.ToInt32(strAccountNo.Substring(8, 1)) * Convert.ToInt32(l2value.Substring(8, 1));
                if (a9 > 9)
                {
                    a9 = Convert.ToInt32(a9.ToString().Substring(0, 1)) + Convert.ToInt32(a9.ToString().Substring(1, 1));
                }
                a10 = Convert.ToInt32(strAccountNo.Substring(9, 1)) * Convert.ToInt32(l2value.Substring(9, 1));
                if (a10 > 9)
                {
                    a10 = Convert.ToInt32(a10.ToString().Substring(0, 1)) + Convert.ToInt32(a10.ToString().Substring(1, 1));
                }
                a11 = Convert.ToInt32(strAccountNo.Substring(10, 1)) * Convert.ToInt32(l2value.Substring(10, 1));
                if (a11 > 9)
                {
                    a11 = Convert.ToInt32(a11.ToString().Substring(0, 1)) + Convert.ToInt32(a11.ToString().Substring(1, 1));
                }
                a12 = Convert.ToInt32(strAccountNo.Substring(11, 1)) * Convert.ToInt32(l2value.Substring(11, 1));
                if (a12 > 9)
                {
                    a12 = Convert.ToInt32(a12.ToString().Substring(0, 1)) + Convert.ToInt32(a12.ToString().Substring(1, 1));
                }
                a13 = Convert.ToInt32(strAccountNo.Substring(12, 1)) * Convert.ToInt32(l2value.Substring(12, 1));
                if (a13 > 9)
                {
                    a13 = Convert.ToInt32(a13.ToString().Substring(0, 1)) + Convert.ToInt32(a13.ToString().Substring(1, 1));
                }


                l5 = (a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13);
                l6 = Convert.ToDecimal(l5) / 10;

                decimal l6remaindder = Convert.ToDecimal(l5) % 10;

                //if(l6.ToString().Substring(l6.ToString().IndexOf(".")+1).Length>0)
                if (l6remaindder > 0)
                {
                    //string strssss = l6.ToString().IndexOf(".").ToString() ;

                    // string strvalue = l6.ToString().Substring(l6.ToString().IndexOf(".") + 1, 1);

                    l7 = 10 - Convert.ToInt32(l6.ToString().Substring(l6.ToString().IndexOf(".") + 1, 1));
                    l8 = l7;
                }
                else
                    l8 = 0;
                int strno = Convert.ToInt32(strAccountNo.Substring(13, 1));
                if (l8 == Convert.ToInt32(strAccountNo.Substring(13, 1)))
                {
                    boolvalidAccount = true;
                }
            }
            return boolvalidAccount;

        }

        private void txtAccountNo_Leave(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (txtAccountNo.Text != string.Empty)
                {
                    //if (txtAccountNo.Text.Length == 14)
                    //    txtAccountNo.Text = txtAccountNo.Text + "      ";
                    //if (txtAccountNo.Text.Length == 15)
                    //    txtAccountNo.Text = txtAccountNo.Text + "     ";
                    //if (txtAccountNo.Text.Length == 16)
                    //    txtAccountNo.Text = txtAccountNo.Text + "    ";
                    //if (txtAccountNo.Text.Length == 17)
                    //    txtAccountNo.Text = txtAccountNo.Text + "   ";
                    //if (txtAccountNo.Text.Length == 18)
                    //    txtAccountNo.Text = txtAccountNo.Text + "  ";
                    //if (txtAccountNo.Text.Length == 19)
                    //    txtAccountNo.Text = txtAccountNo.Text + " ";
                    propAccountNo = txtAccountNo.Text.Trim();
                    propVendorCode = txtVendorId.Text;
                    if (CheckAccountNo())
                    {
                        List<LiheApvEntity> liheapvdetails = _model.LiheAllData.GetLiheAppvadpyas(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, txtVendorId.Text, txtAccountNo.Text, string.Empty);
                        liheapvdetails = liheapvdetails.FindAll(u => u.LPV_APP_NO != BaseForm.BaseApplicationNo);
                        if (liheapvdetails.Count > 0)
                        {
                            CASEVDDEntity casevdd = CaseVddlist.Find(u => u.Code == txtVendorId.Text);
                            string strVendorName = string.Empty;
                            if (casevdd != null)
                                strVendorName = casevdd.Name;
                            CommonFunctions.MessageBoxDisplay("Account# entered is assigned to " + strVendorName + " APP# " + liheapvdetails[0].LPV_APP_NO);

                        }
                    }
                }

            }

        }

        private void EnableDisableControls()
        {

            foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
            {
                bool required = entity.Req.Equals("Y") ? true : false;
                bool enabled = entity.Enab.Equals("Y") ? true : false;

                switch (entity.FldCode)
                {
                    case Consts.Tms00201.Vendor:
                        if (enabled) { btnvendor.Enabled = lblVendor.Enabled = true; if (required) lblvendorReq.Visible = true; } else { btnvendor.Enabled = lblVendor.Enabled = false; lblvendorReq.Visible = false; }
                        break;
                    case Consts.Tms00201.Payfor:
                        if (enabled) { cmbPayfor.Enabled = lblPayfor.Enabled = true; if (required) lblPayforReq.Visible = true; } else { cmbPayfor.Enabled = lblPayfor.Enabled = false; lblPayforReq.Visible = false; }
                        break;
                    case Consts.Tms00201.BillingName:
                        if (enabled) { cmbBilling.Enabled = lblBillingName.Enabled = true; if (required) lblBillingReq.Visible = true; } else { cmbBilling.Enabled = lblBillingName.Enabled = false; lblBillingReq.Visible = false; }
                        break;
                    case Consts.Tms00201.Account:
                        if (enabled) { txtAccountNo.Enabled = lblAccount.Enabled = true; if (required) lblAccountReq.Visible = true; } else { txtAccountNo.Enabled = lblAccount.Enabled = false; lblAccountReq.Visible = false; }
                        break;
                }
                cmbSupplierType.Enabled = true;
            }
        }

        private void HSS20001ClientSupplierForm_Load(object sender, EventArgs e)
        {
            EnableDisableControls();
            if (!FLDCNTLHieEntity.Exists(u => u.Enab.Equals("Y")))
            {
                CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                gvwSupplierDetails.Enabled = false;
                picAddClient.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                EnableControl(false);
            }
        }
        string strpayfor = string.Empty;
        private void cmbPayfor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() != "0")
            {
                if (strpayfor != ((ListItem)cmbPayfor.SelectedItem).Value.ToString())
                {
                    strpayfor = ((ListItem)cmbPayfor.SelectedItem).Value.ToString();
                    txtVendorId.Text = string.Empty;
                    txtVendorName.Text = string.Empty;
                }

                if ((((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02") || (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04"))
                {

                    if (Mode.ToUpper() == "EDIT")
                    {
                        if (gvwSupplierDetails.Rows.Count > 0)
                        {
                            LiheApvEntity liheapvdetails = gvwSupplierDetails.SelectedRows[0].Tag as LiheApvEntity;
                            if (liheapvdetails != null)
                            {
                                if (liheapvdetails.LPV_PAYMENT_FOR.Trim() == (((ListItem)cmbPayfor.SelectedItem).Value.ToString()))
                                {
                                    txtFirst.Text = liheapvdetails.LPV_BILL_FNAME;
                                    txtLast.Text = liheapvdetails.LPV_BILL_LNAME;
                                    txtVendorId.Text = liheapvdetails.LPV_VENDOR;
                                    txtVendorName.Text = liheapvdetails.LPV_VENDORNAME;
                                    txtAccountNo.Text = liheapvdetails.LPV_ACCOUNT_NO;
                                    txtPrimary.Text = liheapvdetails.LPV_PRIMARY_CODE.ToString();
                                    propSeq = liheapvdetails.LPV_SEQ;
                                    strpayfor = liheapvdetails.LPV_PAYMENT_FOR;

                                    chkReverify.Visible = true;
                                    if (liheapvdetails.LPV_REVERIFY == "Y")
                                    {
                                        chkReverify.Checked = true;
                                    }
                                    else
                                    {
                                        chkReverify.Checked = false;
                                    }
                                }
                            }
                        }
                        chkReverify.Visible = true;
                        chkReverify.Enabled = true;

                        if (PropAcctSoft == "Y") { chkReverify.Visible = false; lblReverified.Visible = false; }
                    }
                    else
                    {
                        if (Mode.ToUpper() == "ADD")
                        {
                            chkReverify.Visible = false;
                        }
                    }

                }
                else
                {
                    chkReverify.Visible = false;
                    lblReverified.Text = string.Empty;

                }
            }
        }

        private void cmbSupplierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSupplierType.Items.Count > 0)
            {
                if (((ListItem)cmbSupplierType.SelectedItem).Value.ToString() != "0")
                {
                    if (Mode.ToUpper() == "ADD")
                    {


                        if (BaseForm.BaseCaseMstListEntity[0].HeatIncRent == "2")
                        {
                            if (((ListItem)cmbSupplierType.SelectedItem).Value.ToString() == "P")
                            {
                                lblvendorReq.Visible = false;
                                lblVendor.Visible = false;
                                btnvendor.Visible = false;
                                txtVendorId.Visible = false;
                                txtVendorName.Text = "Heat included in rent";
                            }
                            else
                            {
                                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                                {
                                    bool required = entity.Req.Equals("Y") ? true : false;
                                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                                    switch (entity.FldCode)
                                    {
                                        case Consts.Tms00201.Vendor:
                                            if (enabled) { if (required) lblvendorReq.Visible = true; } else { lblvendorReq.Visible = false; }
                                            break;
                                    }
                                }
                                lblVendor.Visible = true;
                                btnvendor.Visible = true;
                                txtVendorId.Visible = true;
                                txtVendorName.Text = "";
                            }
                        }

                    }
                    txtVendorId_TextChanged(sender, e);
                }
            }

        }

        private void txtVendorId_TextChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() != "0" && !string.IsNullOrEmpty(txtVendorId.Text.Trim()))
            {
                string strAccountFormat = string.Empty; lblMsg.Text = string.Empty;
                CaseVDD1Entity vdd1entity = Vdd1list.Find(u => u.Code == txtVendorId.Text);
                if (vdd1entity != null)
                {
                    strAccountFormat = vdd1entity.ACCT_FORMAT.ToString();
                }

                if (Mode.ToUpper() == "ADD")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    switch (strAccountFormat)
                    {
                        case "12":
                            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 05";
                                lblMsg.Visible = true;
                            }
                            else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 01"; lblMsg.Visible = true;
                            }
                            break;
                        case "13":
                            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 04"; lblMsg.Visible = true;
                            }
                            else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 01"; lblMsg.Visible = true;
                            }
                            break;
                        case "03":
                            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 11 digit number starting with 57"; lblMsg.Visible = true;
                            }
                            else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 11 digit number starting with 51"; lblMsg.Visible = true;
                            }
                            break;
                    }
                }
                else if (gvwSupplierDetails.SelectedRows[0].Cells["gvPassFail"].Value.ToString() == "F")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    switch (strAccountFormat)
                    {
                        case "12":
                            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 05";
                                lblMsg.Visible = true;
                            }
                            else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 01"; lblMsg.Visible = true;
                            }
                            break;
                        case "13":
                            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 04"; lblMsg.Visible = true;
                            }
                            else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 14 digit number starting with 01"; lblMsg.Visible = true;
                            }
                            break;
                        case "03":
                            if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "02")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 11 digit number starting with 57"; lblMsg.Visible = true;
                            }
                            else if (((ListItem)cmbPayfor.SelectedItem).Value.ToString() == "04")
                            {
                                lblMsg.Text = "Account number format for this vendor requires a 11 digit number starting with 51"; lblMsg.Visible = true;
                            }
                            break;
                    }
                }
            }
            if (string.IsNullOrEmpty(lblMsg.Text.Trim())) lblMsg.Visible = false;
        }
        
    }
}