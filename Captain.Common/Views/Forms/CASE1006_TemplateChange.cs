#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
//using Gizmox.WebGUI.Common.Resources;


#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE1006_TemplateChange : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public CASE1006_TemplateChange(BaseForm baseform,string Spcode, string Templcode,string TemplDesc,string SerCode,string SerDesc, string SerDate, TemplateEntity TempEntity, CASESP0Entity sp_header_rec, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileges = privileges;
            TemplateCode = Templcode;
            TemplateDesc = TemplDesc;
            SP_Header_Rec = sp_header_rec;
            ActivityCode = SerCode; ActivityDesc = SerDesc; ActivityDate = SerDate;
            BulkTemplate = TempEntity; SPCode = Spcode;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            this.Text = "BulkPosting Template";

            PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);
            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            BillPeriodEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00202", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "Add");
            CategoryCode = string.Empty;
            if (programEntity != null)
            {
                CategoryCode = programEntity.DepSerpostPAYCAT.Trim();
                
                if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                {
                    if (string.IsNullOrEmpty(CategoryCode.Trim()))
                    {
                        if (PPC_List.Count > 0)
                        {
                            PPC_List = PPC_List.FindAll(u => u.Ext_1 != "");
                            if (PPC_List.Count > 0)
                            {
                                foreach (Agy_Ext_Entity Entity in PPC_List)
                                {
                                    if (!string.IsNullOrEmpty(Entity.Ext_1.Trim()))
                                    {
                                        if (BaseForm.BaseAgency == Entity.Ext_1.Substring(0, 2))
                                        {
                                            CategoryCode = Entity.Ext_2.Trim();
                                        }
                                    }
                                }
                            }

                        }

                    }
                }

            }
            if (CategoryCode == "01") { pnlCatg1.Visible = true; pnlCatg2.Visible = false; pnlCatg3.Visible = false; this.pnlCatg1.Location = new System.Drawing.Point(1, 149); }
            else if (CategoryCode == "02") { pnlCatg2.Visible = true; pnlCatg1.Visible = false; this.pnlCatg2.Location = new System.Drawing.Point(1, 149); }
            else if (CategoryCode == "03") { pnlCatg3.Visible = true; pnlCatg1.Visible = false; pnlCatg2.Visible = false; this.pnlCatg3.Location = new System.Drawing.Point(1, 149); }
            else { pnlCatg1.Visible = true; pnlCatg2.Visible = false; pnlCatg3.Visible = false; this.pnlCatg1.Location = new System.Drawing.Point(1, 149); }

            //pnlCatg1.Visible = true;

            Fill_DropDowns();
            Fill_Program_Combo();
            Get_Vendor_List();

            FillData();

            this.Size = new System.Drawing.Size(914, 267);
            this.panel1.Size = new System.Drawing.Size(912, 264);
            

        }

        List<Agy_Ext_Entity> PPC_List = new List<Agy_Ext_Entity>();
        List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = new List<PMTFLDCNTLHEntity>();
        List<CommonEntity> BillPeriodEntity = new List<CommonEntity>();
        #region properties

        public BaseForm BaseForm { get; set; }

        public string SPCode { get; set; }

        public string CategoryCode { get; set; }

        public string TemplateCode { get; set; }

        public string TemplateDesc { get; set; }

        public string ActivityCode { get; set; }

        public string ActivityDesc { get; set; }

        public string ActivityDate { get; set; }

        public CASESP0Entity SP_Header_Rec { get; set; }

        public List<CATemplateEntity> TemplateList { get; set; }

        public List<TemplateEntity> BulkTempList { get; set; }

        public TemplateEntity BulkTemplate { get; set; }

        public SERVSTOPEntity SERVStopEntity { get; set; }
        
        public string Mode { get; set; }

        public string SP_Sequence { get; set; }
        
        public PrivilegeEntity Privileges { get; set; }

        #endregion

        private void Fill_DropDowns()
        {
            Fill_Sites();
            Fill_CaseWorker();
            Fill_Funding();
            Fill_UOM();

        }

        List<Captain.Common.Utilities.ListItem> listItemSite = new List<Captain.Common.Utilities.ListItem>();
        private void Fill_Sites()
        {

            listItemSite.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));


            DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable Sites_Table = ds.Tables[0];
                if (Sites_Table.Rows.Count > 0)
                {
                    //if (Mode.Equals("Add"))
                    //{
                    DataView dv = new DataView(Sites_Table);
                    dv.RowFilter = "SITE_ACTIVE='Y'";
                    Sites_Table = dv.ToTable();
                    //}

                    foreach (DataRow dr in Sites_Table.Rows)
                        listItemSite.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString().Trim(), dr["SITE_ACTIVE"].ToString().Trim(), (dr["SITE_ACTIVE"].ToString().Trim().Equals("Y") ? Color.Black : Color.Red)));

                }
                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity(); List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                    foreach (HierarchyEntity Entity in userHierarchy)
                    {
                        if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        { hierarchyEntity = null; }
                    }

                    if (hierarchyEntity != null)
                    {
                        List<Captain.Common.Utilities.ListItem> listItemSite = new List<Captain.Common.Utilities.ListItem>();
                        listItemSite.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));
                        if (hierarchyEntity.Sites.Length > 0)
                        {
                            string[] Sites = hierarchyEntity.Sites.Split(',');

                            for (int i = 0; i < Sites.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                {
                                    foreach (Captain.Common.Utilities.ListItem casesite in listItemSite) //Site_List)//ListcaseSiteEntity)
                                    {
                                        if (Sites[i].ToString() == casesite.Value.ToString())
                                        {
                                            listItemSite.Add(casesite);
                                            //break;
                                        }
                                        // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                    }
                                }
                            }
                            //strsiteRoomNames = hierarchyEntity.Sites;
                            listItemSite = listItemSite;
                        }

                    }
                }
            }
        }

        List<Captain.Common.Utilities.ListItem> listItemWorker = new List<Captain.Common.Utilities.ListItem>();
        private void Fill_CaseWorker()
        {

            DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(BaseForm.BaseAgency, "**", "**");
            string strNameFormat = null, strCwFormat = null;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            }


            //Cmb_MS_Worker.Items.Clear(); Cmb_MS_Worker.ColorMember = "FavoriteColor";



            listItemWorker.Add(new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        if (dr["PWH_INACTIVE"].ToString().Trim() == "N")
                            listItemWorker.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Black));
                    }

                   
                }
            }
        }

        List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
        private void Fill_Funding()
        {
            CmbFunding1.Items.Clear(); CmbFunding1.ColorMember = "FavoriteColor";

            FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
            if (!string.IsNullOrEmpty(SP_Header_Rec.Funds))
            {
                bool Fund_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0;

                
                string Funds_Str = SP_Header_Rec.Funds;
                int Tmp_Curr_Fund_Len = 0;

                foreach (SPCommonEntity Entity in FundingList)
                {

                    Fund_Exists = false; Pos = 0;
                    for (int i = 0; Pos < SP_Header_Rec.Funds.Length; i++)
                    {

                        Tmp_Curr_Fund_Len = (Funds_Str.Substring(Pos, Funds_Str.Substring(Pos, (Funds_Str.Length - Pos)).Length)).Length;

                        if (Entity.Code == SP_Header_Rec.Funds.Substring(Pos, (Tmp_Curr_Fund_Len >= 10 ? 10 : Tmp_Curr_Fund_Len)).Trim())
                        {
                            Fund_Exists = true; break;
                        }
                        Pos += 10;

                    }

                    if (Fund_Exists)
                    {
                        CmbFunding1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                        if (CategoryCode == "03")
                        {
                            CmbCat3Funding1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                            CmbCat3Funding2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                            CmbCat3Funding3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                        }
                        Tmp_Loop_Cnt++;
                    }
                }

            }


            CmbFunding1.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            CmbFunding1.SelectedIndex = 0;
            if (CategoryCode == "03")
            {
                CmbCat3Funding1.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
                CmbCat3Funding2.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
                CmbCat3Funding3.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));

                CmbCat3Funding1.SelectedIndex = CmbCat3Funding2.SelectedIndex = CmbCat3Funding3.SelectedIndex = 0;
            }
        }

        List<SPCommonEntity> UOMList = new List<SPCommonEntity>();
        private void Fill_UOM()
        {
            Cmb_UOM.Items.Clear(); Cmb_UOM.ColorMember = "FavoriteColor";
            
            UOMList = _model.SPAdminData.Get_AgyRecs_WithFilter("UOM", "A");

            if (UOMList.Count > 0)
            {
                UOMList = UOMList.FindAll(u => (u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                
                UOMList = UOMList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            foreach (SPCommonEntity Entity in UOMList)
            {
                Cmb_UOM.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                if (CategoryCode == "03")
                {
                    Cmb_Cat3UOM.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                    Cmb_Cat3UOM2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                    Cmb_Cat3UOM3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                }
                
            }
            Cmb_UOM.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            if (CategoryCode == "03")
            {
                Cmb_Cat3UOM.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
                Cmb_Cat3UOM2.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
                Cmb_Cat3UOM3.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));

                Cmb_Cat3UOM.SelectedIndex = 0; Cmb_Cat3UOM2.SelectedIndex = 0; Cmb_Cat3UOM3.SelectedIndex = 0;
            }

            Cmb_UOM.SelectedIndex = 0;

        }

        private void PbVendor_Click(object sender, EventArgs e)
        {
            VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, "**");
            Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Vendor_Browse_Closed);
            Vendor_Browse.ShowDialog();
        }

        private void On_Vendor_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();

                Txt_VendNo.Text = Vendor_Details[0].Trim();
                Text_VendName.Text = Vendor_Details[1].Trim();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FillData()
        {
            if(BulkTemplate!=null)
            {
                //lblService.Text = "Service: "+ActivityDesc;
                lblTempName.Text = TemplateDesc;
                //lblDate.Text = "Date: "+ActivityDate;

                txtService.Text = ActivityDesc.Trim();
                txtActDate.Text = ActivityDate;

                LblProgram.Text = "Program: ";
                if (!string.IsNullOrEmpty(BulkTemplate.BTPL_PROG.Trim()) && !BulkTemplate.BTPL_PROG.Contains("**"))
                    txtProg.Text= Set_SP_Program_Text(BulkTemplate.BTPL_PROG.Trim());
                //LblProgram.Text = "Program: " + Set_SP_Program_Text(BulkTemplate.BTPL_PROG.Trim());

                if (listItemSite.Count>0)
                {
                    if(!string.IsNullOrEmpty(BulkTemplate.BTPL_SITE.Trim()))
                    {
                        foreach (Captain.Common.Utilities.ListItem Entity in listItemSite)
                        {
                            if(BulkTemplate.BTPL_SITE.Trim()==Entity.Value.ToString())
                            {
                                //lblSiteca.Text = "Site: " + Entity.Text.Trim();
                                txtSite.Text = Entity.Text.Trim();
                                break;
                            }
                        }
                    }
                }

                if (listItemWorker.Count > 0)
                {
                    if (!string.IsNullOrEmpty(BulkTemplate.BTPL_WORKER.Trim()))
                    {
                        foreach (Captain.Common.Utilities.ListItem Entity in listItemWorker)
                        {
                            if (BulkTemplate.BTPL_WORKER.Trim() == Entity.Value.ToString())
                            {
                                //lblCaseworca.Text = "Caseworker: " + Entity.Text.Trim();
                                txtCaseWorker.Text= Entity.Text.Trim();
                                break;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(BulkTemplate.BTPL_OBF.Trim()))
                {
                    switch(BulkTemplate.BTPL_OBF.Trim())
                    {
                        case "1": txtBenefit.Text = "Applicant";break;//lblBenefit.Text = "Benefitting From: Applicant"; break;
                        case "2": txtBenefit.Text = "All Household Members"; break;//lblBenefit.Text = "Benefitting From: All Household Members"; break;
                        case "3": txtBenefit.Text = "Selected Household Members"; break;//lblBenefit.Text = "Benefitting From: Selected Household Members"; break;
                    }
                }


                    if (CategoryCode == "03")
                {
                    CommonFunctions.SetComboBoxValue(CmbCat3Funding1, BulkTemplate.BTPL_FUND);
                    CommonFunctions.SetComboBoxValue(CmbCat3Funding2, BulkTemplate.BTPL_FUND2);
                    CommonFunctions.SetComboBoxValue(CmbCat3Funding3, BulkTemplate.BTPL_FUND3);

                    CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM, BulkTemplate.BTPL_UOM);
                    CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM2, BulkTemplate.BTPL_UOM2);
                    CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM3, BulkTemplate.BTPL_UOM3);

                    Txt_Cat3Cost.Text = BulkTemplate.BTPL_AMOUNT;
                    Txt_Cat3Cost2.Text = BulkTemplate.BTPL_AMOUNT2;
                    Txt_Cat3Cost3.Text = BulkTemplate.BTPL_AMOUNT3;
                    txtCat3Total.Text = BulkTemplate.BTPL_TOTAL;
                }
                else if (CategoryCode == "02")
                {
                    txtCat2BillPCode.Text = BulkTemplate.BTPL_BILL_PERIOD;
                    if (!string.IsNullOrEmpty(BulkTemplate.BTPL_BILL_PERIOD.Trim()))
                    {
                        ListcommonEntity = new List<CommonEntity>();
                        string[] CountyList = BulkTemplate.BTPL_BILL_PERIOD.Split(',');
                        if (CountyList.Length > 0)
                        {
                            string BillPeriodDesc = string.Empty;
                            foreach (string Cont in CountyList)
                            {
                                ListcommonEntity.Add(new CommonEntity(Cont, string.Empty));

                                if (BillPeriodEntity.Count > 0)
                                {
                                    foreach (CommonEntity CEntity in BillPeriodEntity)
                                    {
                                        if (CEntity.Code.Trim() == Cont.Trim())
                                        {
                                            BillPeriodDesc += CEntity.Desc.Trim() + ", ";
                                            break;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(BillPeriodDesc.Trim()))
                                txtCat2_BillPeriod.Text = BillPeriodDesc.Substring(0, BillPeriodDesc.Length - 2);
                        }



                    }
                    if (!string.IsNullOrEmpty(BulkTemplate.BTPL_VEND_ACCT.Trim()))
                        txtcat2_Acct.Text = BulkTemplate.BTPL_VEND_ACCT.Trim();
                    if (!string.IsNullOrEmpty(BulkTemplate.BTPL_ARREARS.Trim()))
                        txtCat2ArrearsAmt.Text = BulkTemplate.BTPL_ARREARS;
                    if (!string.IsNullOrEmpty(BulkTemplate.BTPL_TOTAL.Trim()))
                        txtCat2_Amount.Text = BulkTemplate.BTPL_TOTAL;
                }
                else
                {
                    CommonFunctions.SetComboBoxValue(CmbFunding1, BulkTemplate.BTPL_FUND);
                    CommonFunctions.SetComboBoxValue(Cmb_UOM, BulkTemplate.BTPL_UOM);

                    txtRate.Text = BulkTemplate.BTPL_RATE;
                    Txt_Cost.Text = BulkTemplate.BTPL_TOTAL;
                    Txt_Units.Text = BulkTemplate.BTPL_UNITS;
                }

                Txt_VendNo.Text = BulkTemplate.BTPL_VENDOR;
                Text_VendName.Text = Get_Vendor_Name(BulkTemplate.BTPL_VENDOR);
            }
        }

        private string Get_Vendor_Name(string Vendor)
        {
            string Vend_Name = string.Empty;
            foreach (CASEVDDEntity Entity in CaseVddlist)
            {
                if (Entity.Code == Vendor)
                {
                    Vend_Name = Entity.Name.Trim(); break;
                }
            }

            return Vend_Name;
        }

        private string Set_SP_Program_Text(string Prog_Code)
        {
            string Tmp_Hierarchy = "";
            Sel_CAMS_Program = "";

            foreach (HierarchyEntity Ent in SP_Programs_List)
            {
                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                if (Prog_Code == Tmp_Hierarchy)
                {
                    Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                    break;
                }
            }

            return Sel_CAMS_Program;
        }

        string Tmp_SPM_Sequence = "1", Sel_CAMS_Program = "";
        List<HierarchyEntity> SP_Programs_List = new List<HierarchyEntity>();
        private void Fill_Program_Combo()
        {
            string ACR_SERV_Hies = "N";
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
                ACR_SERV_Hies = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim();

            int TmpRows = 1;
            SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, SPCode, ACR_SERV_Hies);
            LblProgram.Text = string.Empty;
            if (SP_Programs_List.Count > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                {
                    string Tmp_Hierarchy = " ";
                    int ProgIndex = 0; bool DefHieExist = false;
                    try
                    {
                        foreach (HierarchyEntity Ent in SP_Programs_List)
                        {
                            Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                            //if (Pass_CA_Entity.Rec_Type == "I" && ProgIndex == 0)
                            //{
                                if (BulkTemplate.BTPL_PROG== Tmp_Hierarchy)
                                {
                                    Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                                    DefHieExist = true;
                                    ProgIndex = TmpRows;
                                    break;
                                }


                            //}
                            TmpRows++;
                        }
                        //}
                    }
                    catch (Exception ex) { }

                    if (TmpRows > 0)
                    {
                        LblProgram.Text = Sel_CAMS_Program;
                    }
                }

            }
            else
                MessageBox.Show("Programs Are Not Defined", "CAP Systems");
        }

        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);
        }

        public TemplateEntity GetTemplatedata()
        {
            TemplateEntity TEntity = new TemplateEntity();
            TEntity = BulkTemplate;
            TEntity.BTPL_VENDOR = Txt_VendNo.Text.Trim();
            if (CategoryCode == "03")
            {
                TEntity.BTPL_FUND = ((ListItem)CmbCat3Funding1.SelectedItem).Value.ToString();
                TEntity.BTPL_FUND2 = ((ListItem)CmbCat3Funding2.SelectedItem).Value.ToString();
                TEntity.BTPL_FUND3 = ((ListItem)CmbCat3Funding3.SelectedItem).Value.ToString();

                TEntity.BTPL_UOM = ((ListItem)Cmb_Cat3UOM.SelectedItem).Value.ToString();
                TEntity.BTPL_UOM2 = ((ListItem)Cmb_Cat3UOM2.SelectedItem).Value.ToString();
                TEntity.BTPL_UOM3 = ((ListItem)Cmb_Cat3UOM3.SelectedItem).Value.ToString();

                TEntity.BTPL_AMOUNT = Txt_Cat3Cost.Text.Trim();
                TEntity.BTPL_AMOUNT2 = Txt_Cat3Cost2.Text.Trim();
                TEntity.BTPL_AMOUNT3 = Txt_Cat3Cost3.Text.Trim();
                TEntity.BTPL_TOTAL = txtCat3Total.Text.Trim();
            }
            else if (CategoryCode == "02")
            {
                TEntity.BTPL_BILL_PERIOD = txtCat2BillPCode.Text;
                TEntity.BTPL_VEND_ACCT = txtcat2_Acct.Text;
                TEntity.BTPL_ARREARS = txtCat2ArrearsAmt.Text;
                TEntity.BTPL_TOTAL = txtCat2_Amount.Text;
            }
            else
            {
                TEntity.BTPL_FUND = ((ListItem)CmbFunding1.SelectedItem).Value.ToString();

                TEntity.BTPL_UOM = ((ListItem)Cmb_UOM.SelectedItem).Value.ToString();
                TEntity.BTPL_UNITS = Txt_Units.Text.Trim();
                TEntity.BTPL_RATE = txtRate.Text.Trim();
                TEntity.BTPL_TOTAL = Txt_Cost.Text.Trim();
            }

            return TEntity;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<CommonEntity> ListcommonEntity { get; set; }
        
        private void btnCat2BillPeriod_Click(object sender, EventArgs e)
        {
            string BundleEnable = string.Empty;
            BundleEnable = "N";
            SelectZipSiteCountyForm countyform = new SelectZipSiteCountyForm(BaseForm, ListcommonEntity, "BillingPeriod", BundleEnable, string.Empty, string.Empty);
            countyform.FormClosed += new FormClosedEventHandler(SelectBillFormClosed);
            countyform.ShowDialog();
        }

        private void SelectBillFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.FormType == "Billing")
                {
                    ListcommonEntity = form.SelectedCountyEntity;
                    if (ListcommonEntity.Count > 0)
                    {
                        string County = string.Empty; string BillPeriodDesc = string.Empty;
                        foreach (CommonEntity Entity in ListcommonEntity)
                        {
                            County += Entity.Code.Trim() + ",";
                            BillPeriodDesc += Entity.Desc.Trim() + ", ";
                        }
                        txtCat2BillPCode.Text = County.Substring(0, County.Length - 1);
                        txtCat2_BillPeriod.Text = BillPeriodDesc.Substring(0, BillPeriodDesc.Length - 2);
                    }
                }


            }
        }


        private void Txt_Cost3_TextChanged(object sender, EventArgs e)
        {
            float V1 = 0, V2 = 0, V3 = 0;
            if (!string.IsNullOrEmpty(Txt_Cat3Cost.Text.Trim())) V1 = float.Parse(Txt_Cat3Cost.Text.Trim());
            if (!string.IsNullOrEmpty(Txt_Cat3Cost2.Text.Trim())) V2 = float.Parse(Txt_Cat3Cost2.Text.Trim());
            if (!string.IsNullOrEmpty(Txt_Cat3Cost3.Text.Trim())) V3 = float.Parse(Txt_Cat3Cost3.Text.Trim());

            if ((V1 + V2 + V3) > 0)
                txtCat3Total.Text = (V1 + V2 + V3).ToString("0.00");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void Cmb_UOM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}