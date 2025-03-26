#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Wisej.Web;
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
using Captain.Common.Views.Controls.Compatibility;
//using Gizmox.WebGUI.Common.Resources;


#endregion
namespace Captain.Common.Views.Forms
{
    public partial class CASE1006_TemplateForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public CASE1006_TemplateForm(BaseForm baseform, string SP_code, string SP_sequence, string SP_start_date, List<ListItem> selected_CAMS_list, CASESP0Entity sp_header_rec, string branchCode, List<TemplateEntity> selTemplatelist, string Tplcode, CommonEntity centity, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileges = privileges;
            SP_Code = SP_code;
            SP_Sequence = SP_sequence;
            Selected_CAMS_List = selected_CAMS_list;
            //TemplateList = selTemplatelist;
            BulkTempList = selTemplatelist;
            SP_Start_Date = SP_start_date;
            SP_Header_Rec = sp_header_rec;
            BranchCode = branchCode;
            PropTemplateCode = Tplcode;
            CritEntity = centity;

            this.Text = "Template Definition";

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);
            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            BillPeriodEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00202", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "Add");
            CategoryCode = string.Empty;
            if (programEntity != null)
            {
                CategoryCode = programEntity.DepSerpostPAYCAT.Trim();
                //propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", CategoryCode, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, Pass_CA_Entity.Service_plan, Pass_CA_Entity.Branch, Pass_CA_Entity.Group, Pass_CA_Entity.ACT_Code.Trim(), "SP");
                //propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == CategoryCode);

                //if (propPMTFLDCNTLHEntity.Count == 0)
                //{
                //    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", CategoryCode, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "0", " ", "0", "          ", "hie");
                //    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == CategoryCode);
                //}

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

            //Kranthi//
            if (CategoryCode == "01")
            {
                pnlCatg1.Visible = true; pnlCatg2.Visible = false; pnlCatg3.Visible = false;
                // this.pnlCatg1.Location = new System.Drawing.Point(0, 102); 
            }
            else if (CategoryCode == "02")
            {
                pnlCatg2.Visible = true; pnlCatg1.Visible = false;
                //this.pnlCatg2.Location = new System.Drawing.Point(0, 102); 
            }
            else if (CategoryCode == "03")
            {
                pnlCatg3.Visible = true; pnlCatg1.Visible = false; pnlCatg2.Visible = false;
                //this.pnlCatg3.Location = new System.Drawing.Point(0, 102); 
            }
            else
            {
                pnlCatg1.Visible = true; pnlCatg2.Visible = false; pnlCatg3.Visible = false;
                //this.pnlCatg1.Location = new System.Drawing.Point(0, 102);
            }

            if (pnlCatg1.Visible == false)
                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - pnlCatg1.Height);
            if (pnlCatg2.Visible == false)
                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - pnlCatg2.Height);
            if (pnlCatg3.Visible == false)
                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - pnlCatg3.Height);

            SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(SP_Code, null, null, null);

            SP_CAMS_Details = SP_CAMS_Details.FindAll(u => u.Branch == BranchCode);


            Fill_DropDowns();
            Fill_Program_Combo();
            FillServicescombo();

            if (!string.IsNullOrEmpty(PropTemplateCode.Trim())) CommonFunctions.SetComboBoxValue(cmbTemplate, PropTemplateCode);

            Get_Vendor_List();

            FillTemplates();

            List<SERVSTOPEntity> SERVSTOPList = new List<SERVSTOPEntity>();
            SERVSTOPList = _model.CaseSumData.GetSERVSTOPDet(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty);
            if (SERVSTOPList.Count == 0)
            {
                SERVSTOPList = _model.CaseSumData.GetSERVSTOPDet(BaseForm.BaseAgency, BaseForm.BaseDept, "**", string.Empty, string.Empty);
                if (SERVSTOPList.Count == 0)
                {
                    SERVSTOPList = _model.CaseSumData.GetSERVSTOPDet(BaseForm.BaseAgency, "**", "**", string.Empty, string.Empty);
                    if (SERVSTOPList.Count == 0)
                        SERVSTOPList = _model.CaseSumData.GetSERVSTOPDet("**", "**", "**", string.Empty, string.Empty);
                }
            }
            if (SERVSTOPList.Count > 0)
            {
                SERVStopEntity = SERVSTOPList[0];
                //SERVSTOPList.Find(u => Convert.ToDateTime(u.TDate.Trim()) >= Convert.ToDateTime(DateTime.Now.ToShortDateString()) && Convert.ToDateTime(u.FDate.Trim()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                //if (Entity != null) IsServiceStop = true;
            }

            Txt_Units.Validator = TextBoxValidation.IntegerValidator;
            Txt_Cost.Validator = TextBoxValidation.FloatValidator;

            //**Kranthi***********************************************************//
            // this.panel3.Size = new System.Drawing.Size(975, 483);
            // this.panel1.Size = new System.Drawing.Size(972, 268);
            // this.panel2.Size = new System.Drawing.Size(972, 213);
            // this.panel4.Size = new System.Drawing.Size(972, 217);
            //  this.Size = new System.Drawing.Size(975, 486);
            //*********************************************************************//

            //this.panel3.Size = new System.Drawing.Size(980, 464);
            //this.panel1.Size = new System.Drawing.Size(972, 250);
            //this.panel2.Size = new System.Drawing.Size(972, 213);
            //this.panel4.Size = new System.Drawing.Size(972, 197);
            //this.Form.Size = new System.Drawing.Size(975, 466);

            //if (TemplateList.Count > 0)
            //    FillGrid();

        }

        List<Agy_Ext_Entity> PPC_List = new List<Agy_Ext_Entity>();
        List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = new List<PMTFLDCNTLHEntity>();
        #region properties

        public BaseForm BaseForm { get; set; }

        public string CategoryCode { get; set; }

        public List<FldcntlHieEntity> CntlCAEntity { get; set; }

        public List<FldcntlHieEntity> CntlMSEntity { get; set; }

        public List<ListItem> Selected_CAMS_List { get; set; }

        public List<CASESP2Entity> SP_CAMS_Details { get; set; }

        public PrivilegeEntity CaseSerPrivileges { get; set; }

        public List<CATemplateEntity> TemplateList { get; set; }

        public List<TemplateEntity> BulkTempList { get; set; }

        public SERVSTOPEntity SERVStopEntity { get; set; }

        public CommonEntity CritEntity { get; set; }

        public string CAMS_FLG { get; set; }

        public string BranchCode { get; set; }

        public string CAMS_Desc { get; set; }

        public string Hierarchy { get; set; }

        public string SP_Code { get; set; }

        public string Year { get; set; }

        public string MST_Site { get; set; }

        public string Mode { get; set; }

        public string SP_Sequence { get; set; }

        public string MST_Intakeworker { get; set; }

        public string SP_Start_Date { get; set; }

        public string strCAOBO { get; set; }

        public string PropTemplateCode { get; set; }

        public CASEACTEntity Pass_CA_Entity { get; set; }

        public CASEMSEntity Pass_MS_Entity { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public CASESP0Entity SP_Header_Rec { get; set; }

        #endregion

        private void Fill_DropDowns()
        {
            Fill_Sites();
            Fill_CaseWorker();

            Fill_Funding();
            Fill_UOM();

            BenefitFrom();


        }

        private void FillTemplates()
        {
            cmbTemplate.Items.Clear();
            List<TemplateEntity> TempList = _model.SPAdminData.Browse_Templates(string.Empty, string.Empty, SP_Code, string.Empty, BranchCode, string.Empty);
            TempList = TempList.FindAll(u => u.BTPL_ADD_OPERATOR.Trim() == BaseForm.UserID.Trim());//.OrderByDescending(u => u.BTPL_DATE_LSTC).ToList();

            int Count = 0;
            cmbTemplate.Items.Add(new Captain.Common.Utilities.ListItem("Add New Template", "0"));
            if (TempList.Count > 0)
            {
                TempList = TempList.OrderByDescending(u => u.BTPL_DATE_LSTC).ToList();
                var DistTemp = TempList.Select(u => u.BTPL_CODE).Distinct().ToList();  //TempList.Select(u => new { u.BTPL_CODE, u.BTPL_DESC }).Distinct();//
                if (DistTemp.Count > 0)
                {

                    foreach (var item in DistTemp)
                    {
                        TemplateEntity Template = TempList.Find(u => u.BTPL_CODE == item);
                        if (Template != null)
                        {
                            cmbTemplate.Items.Add(new Captain.Common.Utilities.ListItem(Template.BTPL_DESC.Trim(), Template.BTPL_CODE));
                            Count++;
                        }
                    }
                }
            }

            //cmbTemplate.Items.Add(new Captain.Common.Utilities.ListItem("Applicant", "1"));
            //cmbTemplate.Items.Add(new Captain.Common.Utilities.ListItem("All Household Members", "2"));
            //cmbTemplate.Items.Add(new Captain.Common.Utilities.ListItem("Selected Household Members", "3"));
            if (Count > 0) cmbTemplate.SelectedIndex = 1;
            else
                cmbTemplate.SelectedIndex = 0;

        }

        private void Fill_Sites()
        {
            CmbSite.Items.Clear();
            //Cmb_MS_Site.Items.Clear();
            //Cmb_Bulk_Site.Items.Clear();

            CmbSite.ColorMember = "FavoriteColor";
            //Cmb_MS_Site.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));


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
                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString().Trim(), dr["SITE_ACTIVE"].ToString().Trim(), (dr["SITE_ACTIVE"].ToString().Trim().Equals("Y") ? Color.Black : Color.Red)));

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
                                    foreach (Captain.Common.Utilities.ListItem casesite in listItem) //Site_List)//ListcaseSiteEntity)
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
                            listItem = listItemSite;
                        }

                    }
                }
            }
            //if (CAMS_FLG == "CA")
            CmbSite.Items.AddRange(listItem.ToArray());


            //switch (CAMS_FLG)
            //{
            //    case "CA":
            if (!string.IsNullOrEmpty(BaseForm.UserProfile.Site.Trim()) && BaseForm.UserProfile.Site.Trim() != "****")
            {
                CommonFunctions.SetComboBoxValue(CmbSite, BaseForm.UserProfile.Site.Trim().Substring(2, BaseForm.UserProfile.Site.Trim().Length - 2));
            }
            else if (!string.IsNullOrEmpty(MST_Site))
                CommonFunctions.SetComboBoxValue(CmbSite, MST_Site);
            //else if (!string.IsNullOrEmpty(SPM_Site))
            //    CommonFunctions.SetComboBoxValue(CmbSite, SPM_Site);
            else CmbSite.SelectedIndex = 0;

            //if (Pass_CA_Entity.Rec_Type == "I") //!string.IsNullOrEmpty(MST_Site))
            //{
            //    if (CA_Template_List.Count == 0)
            //    {
            //        if (!string.IsNullOrEmpty(BaseForm.UserProfile.Site.Trim()) && BaseForm.UserProfile.Site.Trim() != "****")
            //        {
            //            CommonFunctions.SetComboBoxValue(CmbSite, BaseForm.UserProfile.Site.Trim().Substring(2, BaseForm.UserProfile.Site.Trim().Length - 2));
            //        }
            //        else if (!string.IsNullOrEmpty(MST_Site))
            //            CommonFunctions.SetComboBoxValue(CmbSite, MST_Site);
            //        else if (!string.IsNullOrEmpty(SPM_Site))
            //            CommonFunctions.SetComboBoxValue(CmbSite, SPM_Site);
            //        else CmbSite.SelectedIndex = 0;
            //    }
            //}
            //else
            //    CmbSite.SelectedIndex = 0;
            //break;

            //}
        }

        private void Fill_CaseWorker()
        {

            DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(BaseForm.BaseAgency, "**", "**");
            string strNameFormat = null, strCwFormat = null;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            }

            CmbWorker.Items.Clear(); CmbWorker.ColorMember = "FavoriteColor";
            //Cmb_MS_Worker.Items.Clear(); Cmb_MS_Worker.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();

            listItem.Add(new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        if (dr["PWH_INACTIVE"].ToString().Trim() == "N")
                            listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Black));
                    }

                    //if (CAMS_FLG == "CA")
                    CmbWorker.Items.AddRange(listItem.ToArray());
                    //else
                    //    Cmb_MS_Worker.Items.AddRange(listItem.ToArray());
                }
            }
            //switch (CAMS_FLG)
            //{
            //    case "CA":
            //        //if (Pass_CA_Entity.Rec_Type == "I") // !string.IsNullOrEmpty(MST_Intakeworker))
            //{

            if (!string.IsNullOrEmpty(BaseForm.UserProfile.CaseWorker))
                CommonFunctions.SetComboBoxValue(CmbWorker, BaseForm.UserProfile.CaseWorker);

            //}

            else
                CmbWorker.SelectedIndex = 0;
            //        break;

            //}
        }

        List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
        private void Fill_Funding()
        {
            CmbFunding1.Items.Clear(); CmbFunding1.ColorMember = "FavoriteColor";

            if (!string.IsNullOrEmpty(SP_Header_Rec.Funds))
            {
                bool Fund_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0;

                FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
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
                        //if (Mode == "Edit" || (Mode == "Add" && Entity.Active.Equals("Y")))
                        //{
                        CmbFunding1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                        if (CategoryCode == "03")
                        {
                            CmbCat3Funding1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                            CmbCat3Funding2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                            CmbCat3Funding3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red)));
                        }
                        Tmp_Loop_Cnt++;
                        //}
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
                //if (Mode.ToUpper() == "ADD")
                //{
                UOMList = UOMList.FindAll(u => (u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                //}
                //else
                //{
                //    UOMList = UOMList.FindAll(u => u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();
                //}

                UOMList = UOMList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            foreach (SPCommonEntity Entity in UOMList)
            {
                //if (Mode == "Edit" || (Mode == "Add" && Entity.Active.Equals("Y")))
                //{
                Cmb_UOM.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                if (CategoryCode == "03")
                {
                    Cmb_Cat3UOM.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                    Cmb_Cat3UOM2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                    Cmb_Cat3UOM3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                }
                //}
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

        private void BenefitFrom()
        {
            cmb_CA_Benefit.Items.Clear();
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Applicant", "1"));
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("All Household Members", "2"));
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Selected Household Members", "3"));

            cmb_CA_Benefit.SelectedIndex = 0;

        }

        string Tmp_SPM_Sequence = "1", Sel_CAMS_Program = "";
        List<HierarchyEntity> SP_Programs_List = new List<HierarchyEntity>();
        private void Fill_Program_Combo()
        {
            //List<CASESPMEntity> CASESPM_List = new List<CASESPMEntity>();
            //CASESPMEntity Search_Entity = new CASESPMEntity(true);

            //if (Pass_CA_Entity.Rec_Type == "I")
            //{
            //    Search_Entity.agency = BaseForm.BaseAgency;
            //    Search_Entity.dept = BaseForm.BaseDept;
            //    Search_Entity.program = BaseForm.BaseProg;
            //    Search_Entity.app_no = BaseForm.BaseApplicationNo;
            //    Search_Entity.service_plan = Pass_CA_Entity.Service_plan;

            //    CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
            //}

            string ACR_SERV_Hies = "N";
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
                ACR_SERV_Hies = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim();

            int TmpRows = 1;
            SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, SP_Code, ACR_SERV_Hies);
            Txt_CA_Program.Clear();
            if (SP_Programs_List.Count > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                {
                    string Tmp_Hierarchy = " ";
                    int ProgIndex = 0; bool DefHieExist = false;
                    try
                    {
                        //if (CASESPM_List.Count > 0)
                        //{
                        //    foreach (HierarchyEntity Ent in SP_Programs_List)
                        //    {
                        //        Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                        //        if (Pass_CA_Entity.Rec_Type == "I" && ProgIndex == 0)
                        //        {
                        //            foreach (CASESPMEntity Entity in CASESPM_List)
                        //            {
                        //                if (Entity.Def_Program == Tmp_Hierarchy)
                        //                {
                        //                    Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                        //                    DefHieExist = true;
                        //                    ProgIndex = TmpRows;
                        //                    break;
                        //                }
                        //            }

                        //        }
                        //        TmpRows++;
                        //    }
                        //}
                        //else
                        //{
                        foreach (HierarchyEntity Ent in SP_Programs_List)
                        {
                            Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                            if (Pass_CA_Entity.Rec_Type == "I" && ProgIndex == 0)
                            {
                                if (Pass_CA_Entity.Act_PROG == Tmp_Hierarchy)
                                {
                                    Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                                    DefHieExist = true;
                                    ProgIndex = TmpRows;
                                    break;
                                }


                            }
                            TmpRows++;
                        }
                        //}
                    }
                    catch (Exception ex) { }

                    if (TmpRows > 0)
                    {
                        Txt_CA_Program.Text = Sel_CAMS_Program;
                    }
                }

            }
            else
                MessageBox.Show("Programs Are Not Defined", "CAP Systems");
        }

        List<CASESP2Entity> CA_Details = new List<CASESP2Entity>();
        private void FillServicescombo()
        {
            //DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvService"];

            if (SP_CAMS_Details.Count > 0)
            {
                CA_Details = new List<CASESP2Entity>();

                CA_Details = SP_CAMS_Details.FindAll(u => u.Type1.Equals("CA"));
                CA_Details = CA_Details.FindAll(u => u.CAMS_Active.Equals("True") && u.SP2_CAMS_Active.Equals("A"));

                List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                listItem.Add(new Captain.Common.Utilities.ListItem("   ", "0"));

                if (CA_Details.Count > 0)
                {
                    foreach (CASESP2Entity dr in CA_Details)
                        listItem.Add(new Captain.Common.Utilities.ListItem(dr.CAMS_Desc, dr.CamCd, dr.Branch, dr.Orig_Grp.ToString()));
                }
                cmbService.Items.AddRange(listItem.ToArray());

            }
        }

        string strmsgGrp = string.Empty; string SqlMsg = string.Empty;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (btnAdd.Text == "Save")
                {
                    TemplateEntity TEntity = new TemplateEntity();

                    TEntity.Rec_Type = "I";
                    if (((ListItem)cmbTemplate.SelectedItem).Value.ToString() == "0")
                    {
                        TEntity.BTPL_DESC = txtTemplate.Text.Trim();
                        TEntity.BTPL_CODE = "0";
                    }
                    else
                    {
                        TEntity.BTPL_DESC = ((ListItem)cmbTemplate.SelectedItem).Text.ToString();
                        TEntity.BTPL_CODE = ((ListItem)cmbTemplate.SelectedItem).Value.ToString();
                    }
                    TEntity.BTPL_SERVICEPLAN = SP_Code;
                    TEntity.BTPL_SPM_SEQ = SP_Sequence;
                    TEntity.BTPL_BRANCH = ((ListItem)cmbService.SelectedItem).ID.ToString();
                    TEntity.BTPL_GROUP = ((ListItem)cmbService.SelectedItem).ValueDisplayCode.ToString();
                    TEntity.BTPL_CACODE = ((ListItem)cmbService.SelectedItem).Value.ToString();
                    TEntity.BTPL_DATE = Act_Date.Text.ToString();
                    if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))
                        TEntity.BTPL_PROG = Txt_CA_Program.Text.Substring(0, 6);

                    TEntity.BTPL_SITE = ((ListItem)CmbSite.SelectedItem).Value.ToString();
                    TEntity.BTPL_WORKER = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
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

                    TEntity.BTPL_OBF = ((ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();

                    TEntity.BTPL_SPM = chkbSPM.Checked == true ? "Y" : "N";
                    TEntity.BTPL_ALLOW_POST = chkbAllowPost.Checked == true ? "Y" : "N";

                    TEntity.BTPL_ADD_OPERATOR = BaseForm.UserID;
                    TEntity.BTPL_LSTC_OPERATOR = BaseForm.UserID;

                    if (CritEntity != null)
                    {
                        TEntity.BTPL_SER_FDATE = CritEntity.Code;
                        TEntity.BTPL_SER_TDATE = CritEntity.Desc;
                        TEntity.BTPL_SER_SITE = CritEntity.Hierarchy;
                        TEntity.BTPL_SER_SORT = CritEntity.Extension;

                        TEntity.BTPL_SER_CASETYPE = CritEntity.Pyear;
                        TEntity.BTPL_SER_TYPE = CritEntity.PAgency;
                        if (CritEntity.PAgency.Trim() == "E")
                            TEntity.BTPL_SER_ESTATUS = CritEntity.PDept;
                        else if (CritEntity.PAgency.Trim() == "C")
                        {
                            TEntity.BTPL_SER_QUESTION = CritEntity.Pprog;
                            TEntity.BTPL_SER_RESPONSE = CritEntity.Active;
                        }
                    }

                    //CATemplateEntity Entity = new CATemplateEntity();

                    //Entity.CADesc = ((ListItem)cmbService.SelectedItem).Text.ToString();
                    //Entity.CAMS_Code = ((ListItem)cmbService.SelectedItem).Value.ToString();
                    //Entity.Template_date = Act_Date.Text.ToString();
                    //Entity.Benefit_from = ((ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();
                    //Entity.Branch= ((ListItem)cmbService.SelectedItem).ID.ToString();
                    //Entity.Group = ((ListItem)cmbService.SelectedItem).ValueDisplayCode.ToString();

                    //if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))
                    //    Entity.Program = Txt_CA_Program.Text.Substring(0, 6);

                    //Entity.Site = ((ListItem)CmbSite.SelectedItem).Value.ToString();
                    //Entity.CaseWorker = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
                    //Entity.Vendor = Txt_VendNo.Text.Trim();
                    //Entity.Fund = ((ListItem)CmbFunding1.SelectedItem).Value.ToString();

                    //Entity.UOM = ((ListItem)Cmb_UOM.SelectedItem).Value.ToString();
                    //Entity.Units = Txt_Units.Text.Trim();
                    //Entity.Rate = txtRate.Text.Trim();
                    //Entity.Amount = Txt_Cost.Text.Trim();

                    int Seq = 0; string AddCode = string.Empty;
                    if (BulkTempList.Count > 0) { AddCode = BulkTempList.Max(u => u.BTPL_CASEQ); Seq = (int.Parse(AddCode) + 1); }
                    else Seq = 1;

                    //Entity.CA_Seq = Seq.ToString();

                    //TemplateList.Add(new CATemplateEntity(Entity));

                    TEntity.BTPL_CASEQ = Seq.ToString();



                    if (_model.SPAdminData.UpdateBULKTEMPLATE(TEntity, "UPDATE", out strmsgGrp, out SqlMsg))
                    {
                        ClearData();
                        panel4.Enabled = false;
                        chkbAllowPost.Enabled = false; chkbSPM.Enabled = false;
                        btnOK.Visible = true;
                        FillTemplates();

                        CommonFunctions.SetComboBoxValue(cmbTemplate, strmsgGrp);




                    }

                    //int rowIndex = SP_CAMS_Grid.Rows.Add(Entity.CADesc.Trim(), Entity.Template_date, ((ListItem)CmbFunding1.SelectedItem).Text.ToString(), ((ListItem)Cmb_UOM.SelectedItem).Text.ToString(), Txt_Units.Text, txtRate.Text, Txt_Cost.Text.Trim(), Entity.CAMS_Code,Entity.CA_Seq, Entity.Benefit_from);


                }
                else if (btnAdd.Text == "Update")
                {
                    if (BulkTempList.Count > 0)
                    {
                        TemplateEntity Entity = BulkTempList.Find(u => u.BTPL_CACODE == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && Convert.ToDateTime(u.BTPL_DATE.Trim()) == Convert.ToDateTime(SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString().Trim()) && u.BTPL_CASEQ == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
                        if (Entity != null)
                        {
                            TemplateEntity TEntity = new TemplateEntity();

                            TEntity.Rec_Type = "U";
                            TEntity.BTPL_ID = SP_CAMS_Grid.SelectedRows[0].Cells["gvTempID"].Value.ToString();
                            TEntity.BTPL_DESC = ((ListItem)cmbTemplate.SelectedItem).Text.ToString();
                            TEntity.BTPL_CODE = ((ListItem)cmbTemplate.SelectedItem).Value.ToString();
                            TEntity.BTPL_SERVICEPLAN = SP_Code;
                            TEntity.BTPL_SPM_SEQ = SP_Sequence;
                            TEntity.BTPL_BRANCH = ((ListItem)cmbService.SelectedItem).ID.ToString();
                            TEntity.BTPL_GROUP = ((ListItem)cmbService.SelectedItem).ValueDisplayCode.ToString();
                            TEntity.BTPL_CACODE = ((ListItem)cmbService.SelectedItem).Value.ToString();
                            TEntity.BTPL_DATE = Act_Date.Text.ToString();
                            if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))
                                TEntity.BTPL_PROG = Txt_CA_Program.Text.Substring(0, 6);

                            TEntity.BTPL_SITE = ((ListItem)CmbSite.SelectedItem).Value.ToString();
                            TEntity.BTPL_WORKER = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
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
                            //TEntity.BTPL_FUND = ((ListItem)CmbFunding1.SelectedItem).Value.ToString();

                            //TEntity.BTPL_UOM = ((ListItem)Cmb_UOM.SelectedItem).Value.ToString();
                            //TEntity.BTPL_UNITS = Txt_Units.Text.Trim();
                            //TEntity.BTPL_RATE = txtRate.Text.Trim();
                            //TEntity.BTPL_AMOUNT = Txt_Cost.Text.Trim();
                            TEntity.BTPL_OBF = ((ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();

                            TEntity.BTPL_SPM = chkbSPM.Checked == true ? "Y" : "N";
                            TEntity.BTPL_ALLOW_POST = chkbAllowPost.Checked == true ? "Y" : "N";

                            TEntity.BTPL_ADD_OPERATOR = BaseForm.UserID;
                            TEntity.BTPL_LSTC_OPERATOR = BaseForm.UserID;

                            if (CritEntity != null)
                            {
                                TEntity.BTPL_SER_FDATE = CritEntity.Code;
                                TEntity.BTPL_SER_TDATE = CritEntity.Desc;
                                TEntity.BTPL_SER_SITE = CritEntity.Hierarchy;
                                TEntity.BTPL_SER_SORT = CritEntity.Extension;

                                TEntity.BTPL_SER_CASETYPE = CritEntity.Pyear;
                                TEntity.BTPL_SER_TYPE = CritEntity.PAgency;
                                if (CritEntity.PAgency.Trim() == "E")
                                    TEntity.BTPL_SER_ESTATUS = CritEntity.PDept;
                                else if (CritEntity.PAgency.Trim() == "C")
                                {
                                    TEntity.BTPL_SER_QUESTION = CritEntity.Pprog;
                                    TEntity.BTPL_SER_RESPONSE = CritEntity.Active;
                                }
                            }

                            //Entity.CADesc = ((ListItem)cmbService.SelectedItem).Text.ToString();
                            //Entity.CAMS_Code = ((ListItem)cmbService.SelectedItem).Value.ToString();
                            //Entity.Template_date = Act_Date.Text.ToString();
                            //Entity.Benefit_from = ((ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();
                            //Entity.Branch = ((ListItem)cmbService.SelectedItem).ID.ToString();
                            //Entity.Group = ((ListItem)cmbService.SelectedItem).ValueDisplayCode.ToString();

                            //if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))
                            //    Entity.Program = Txt_CA_Program.Text.Substring(0, 6);

                            //Entity.Site = ((ListItem)CmbSite.SelectedItem).Value.ToString();
                            //Entity.CaseWorker = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
                            //Entity.Vendor = Txt_VendNo.Text.Trim();
                            //Entity.Fund = ((ListItem)CmbFunding1.SelectedItem).Value.ToString();

                            //Entity.UOM = ((ListItem)Cmb_UOM.SelectedItem).Value.ToString();
                            //Entity.Units = Txt_Units.Text.Trim();
                            //Entity.Rate = txtRate.Text.Trim();
                            //Entity.Amount = Txt_Cost.Text.Trim();

                            //int Seq = 0; string AddCode = string.Empty;
                            //if (TemplateList.Count > 0) { AddCode = TemplateList.Max(u => u.CA_Seq); Seq = (int.Parse(AddCode) + 1); }
                            //else Seq = 1;

                            //Entity.CA_Seq = Seq.ToString();
                            if (_model.SPAdminData.UpdateBULKTEMPLATE(TEntity, "UPDATE", out strmsgGrp, out SqlMsg))
                            {
                                BulkTempList = _model.SPAdminData.Browse_Templates(string.Empty, ((ListItem)cmbTemplate.SelectedItem).Value.ToString(), SP_Code, SP_Sequence, BranchCode, string.Empty);
                                BulkTempList = BulkTempList.FindAll(u => u.BTPL_ADD_OPERATOR.Trim() == BaseForm.UserID.Trim());

                                SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value = Act_Date.Text.ToString();
                                SP_CAMS_Grid.SelectedRows[0].Cells["gvFund"].Value = ((ListItem)CmbFunding1.SelectedItem).Text.ToString();
                                SP_CAMS_Grid.SelectedRows[0].Cells["gvUOM"].Value = ((ListItem)Cmb_UOM.SelectedItem).Text.ToString();
                                SP_CAMS_Grid.SelectedRows[0].Cells["gvUnits"].Value = Txt_Units.Text.ToString();
                                SP_CAMS_Grid.SelectedRows[0].Cells["gvRate"].Value = txtRate.Text.ToString();
                                SP_CAMS_Grid.SelectedRows[0].Cells["gvAmount"].Value = Txt_Cost.Text.ToString();

                                SP_CAMS_Grid.Update();

                                SP_CAMS_Grid_SelectionChanged(sender, e);
                            }

                            btnAdd.Text = "Save"; cmbService.Enabled = true;
                            ClearData(); panel4.Enabled = false;
                            chkbAllowPost.Enabled = false; chkbSPM.Enabled = false;
                            btnOK.Visible = true;
                        }
                    }
                }
            }
        }

        //private void btnAdd_Click1(object sender, EventArgs e)
        //{
        //    if (ValidateForm())
        //    {
        //        if (btnAdd.Text == "Save")
        //        {
        //            TemplateEntity TEntity = new TemplateEntity();

        //            CATemplateEntity Entity = new CATemplateEntity();

        //            Entity.CADesc = ((ListItem)cmbService.SelectedItem).Text.ToString();
        //            Entity.CAMS_Code = ((ListItem)cmbService.SelectedItem).Value.ToString();
        //            Entity.Template_date = Act_Date.Text.ToString();
        //            Entity.Benefit_from = ((ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();
        //            Entity.Branch = ((ListItem)cmbService.SelectedItem).ID.ToString();
        //            Entity.Group = ((ListItem)cmbService.SelectedItem).ValueDisplayCode.ToString();

        //            if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))
        //                Entity.Program = Txt_CA_Program.Text.Substring(0, 6);

        //            Entity.Site = ((ListItem)CmbSite.SelectedItem).Value.ToString();
        //            Entity.CaseWorker = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
        //            Entity.Vendor = Txt_VendNo.Text.Trim();
        //            Entity.Fund = ((ListItem)CmbFunding1.SelectedItem).Value.ToString();

        //            Entity.UOM = ((ListItem)Cmb_UOM.SelectedItem).Value.ToString();
        //            Entity.Units = Txt_Units.Text.Trim();
        //            Entity.Rate = txtRate.Text.Trim();
        //            Entity.Amount = Txt_Cost.Text.Trim();

        //            int Seq = 0; string AddCode = string.Empty;
        //            if (TemplateList.Count > 0) { AddCode = TemplateList.Max(u => u.CA_Seq); Seq = (int.Parse(AddCode) + 1); }
        //            else Seq = 1;

        //            Entity.CA_Seq = Seq.ToString();


        //            TemplateList.Add(new CATemplateEntity(Entity));

        //            int rowIndex = SP_CAMS_Grid.Rows.Add(Entity.CADesc.Trim(), Entity.Template_date, ((ListItem)CmbFunding1.SelectedItem).Text.ToString(), ((ListItem)Cmb_UOM.SelectedItem).Text.ToString(), Txt_Units.Text, txtRate.Text, Txt_Cost.Text.Trim(), Entity.CAMS_Code, Entity.CA_Seq, Entity.Benefit_from);

        //            ClearData();
        //        }
        //        else if (btnAdd.Text == "Update")
        //        {
        //            if (TemplateList.Count > 0)
        //            {
        //                CATemplateEntity Entity = TemplateList.Find(u => u.CAMS_Code == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && u.Template_date == SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString() && u.CA_Seq == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
        //                if (Entity != null)
        //                {
        //                    //TemplateList.Remove(Entity);

        //                    Entity.CADesc = ((ListItem)cmbService.SelectedItem).Text.ToString();
        //                    Entity.CAMS_Code = ((ListItem)cmbService.SelectedItem).Value.ToString();
        //                    Entity.Template_date = Act_Date.Text.ToString();
        //                    Entity.Benefit_from = ((ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();
        //                    Entity.Branch = ((ListItem)cmbService.SelectedItem).ID.ToString();
        //                    Entity.Group = ((ListItem)cmbService.SelectedItem).ValueDisplayCode.ToString();

        //                    if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))
        //                        Entity.Program = Txt_CA_Program.Text.Substring(0, 6);

        //                    Entity.Site = ((ListItem)CmbSite.SelectedItem).Value.ToString();
        //                    Entity.CaseWorker = ((ListItem)CmbWorker.SelectedItem).Value.ToString();
        //                    Entity.Vendor = Txt_VendNo.Text.Trim();
        //                    Entity.Fund = ((ListItem)CmbFunding1.SelectedItem).Value.ToString();

        //                    Entity.UOM = ((ListItem)Cmb_UOM.SelectedItem).Value.ToString();
        //                    Entity.Units = Txt_Units.Text.Trim();
        //                    Entity.Rate = txtRate.Text.Trim();
        //                    Entity.Amount = Txt_Cost.Text.Trim();

        //                    //int Seq = 0; string AddCode = string.Empty;
        //                    //if (TemplateList.Count > 0) { AddCode = TemplateList.Max(u => u.CA_Seq); Seq = (int.Parse(AddCode) + 1); }
        //                    //else Seq = 1;

        //                    //Entity.CA_Seq = Seq.ToString();

        //                    SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value = Entity.Template_date;
        //                    SP_CAMS_Grid.SelectedRows[0].Cells["gvFund"].Value = ((ListItem)CmbFunding1.SelectedItem).Text.ToString();
        //                    SP_CAMS_Grid.SelectedRows[0].Cells["gvUOM"].Value = ((ListItem)Cmb_UOM.SelectedItem).Text.ToString();
        //                    SP_CAMS_Grid.SelectedRows[0].Cells["gvUnits"].Value = Txt_Units.Text.ToString();
        //                    SP_CAMS_Grid.SelectedRows[0].Cells["gvRate"].Value = txtRate.Text.ToString();
        //                    SP_CAMS_Grid.SelectedRows[0].Cells["gvAmount"].Value = Txt_Cost.Text.ToString();

        //                    SP_CAMS_Grid.Update();

        //                    //CommonFunctions.SetComboBoxValue(cmbService, Entity.CAMS_Code);
        //                    //Act_Date.Text = Entity.Template_date.ToString();
        //                    //CommonFunctions.SetComboBoxValue(cmb_CA_Benefit, Entity.Benefit_from);
        //                    //CommonFunctions.SetComboBoxValue(CmbFunding1, Entity.Fund);
        //                    //CommonFunctions.SetComboBoxValue(CmbSite, Entity.Site);
        //                    //CommonFunctions.SetComboBoxValue(CmbWorker, Entity.CaseWorker);
        //                    //CommonFunctions.SetComboBoxValue(Cmb_UOM, Entity.UOM);

        //                    //txtRate.Text = Entity.Rate;
        //                    //Txt_Cost.Text = Entity.Amount;
        //                    //Txt_Units.Text = Entity.Units;
        //                    //Txt_VendNo.Text = Entity.Vendor;
        //                    //Text_VendName.Text = Get_Vendor_Name(Entity.Vendor);

        //                    //if (!string.IsNullOrEmpty(Entity.Program.Trim()) && !Entity.Program.Contains("**"))
        //                    //    Txt_CA_Program.Text = Set_SP_Program_Text(Entity.Program.Trim());

        //                    //TemplateList.(new CATemplateEntity(Entity));

        //                    btnAdd.Text = "Save"; cmbService.Enabled = true;
        //                    ClearData();
        //                }
        //            }
        //        }
        //    }
        //}


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

        private bool ValidateForm()
        {
            bool IsValidate = true;

            if (((ListItem)cmbService.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbService, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblService.Text.Replace(Consts.Common.Colon, string.Empty)));
                IsValidate = false;
            }
            else
                _errorProvider.SetError(cmbService, null);

            if (Act_Date.Checked == false)
            {
                _errorProvider.SetError(Act_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                IsValidate = false;
            }
            else
            {

                if (!(BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))
                {
                    if (SERVStopEntity != null && Act_Date.Checked)
                    {
                        if (Convert.ToDateTime(SERVStopEntity.TDate.Trim()) >= Act_Date.Value && Convert.ToDateTime(SERVStopEntity.FDate.Trim()) <= Act_Date.Value)
                        {
                            _errorProvider.SetError(Act_Date, string.Format(" " + lblActivityDate.Text + " Should not be between " + LookupDataAccess.Getdate(SERVStopEntity.FDate.Trim()) + " and " + LookupDataAccess.Getdate(SERVStopEntity.TDate.Trim()).Replace(Consts.Common.Colon, string.Empty)));
                            IsValidate = false;
                        }
                        else
                        {
                            _errorProvider.SetError(Act_Date, null);
                        }

                    }
                }
            }

            return IsValidate;
        }

        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);
        }

        private void SP_CAMS_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = SP_CAMS_Grid.CurrentCell.ColumnIndex;
                int RowIdx = SP_CAMS_Grid.CurrentCell.RowIndex;

                switch (e.ColumnIndex)
                {
                    case 11:
                        if (BulkTempList.Count > 0)
                        {
                            ///CATemplateEntity Entity = TemplateList.Find(u => u.CAMS_Code == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && u.Template_date==SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString() && u.CA_Seq == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
                            TemplateEntity Entity = BulkTempList.Find(u => u.BTPL_CACODE == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && Convert.ToDateTime(u.BTPL_DATE.Trim()) == Convert.ToDateTime(SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString().Trim()) && u.BTPL_CASEQ == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
                            if (Entity != null)
                            {
                                panel4.Enabled = true; btnOK.Visible = false; chkbSPM.Enabled = true; chkbAllowPost.Enabled = true;
                                //CommonFunctions.SetComboBoxValue(cmbService, Entity.BTPL_CACODE);
                                //Act_Date.Text = Entity.BTPL_DATE.ToString();
                                //if (!string.IsNullOrEmpty(Entity.BTPL_DATE.ToString().Trim())) Act_Date.Checked = true;
                                //CommonFunctions.SetComboBoxValue(cmb_CA_Benefit, Entity.BTPL_OBF);
                                //CommonFunctions.SetComboBoxValue(CmbFunding1, Entity.BTPL_FUND);
                                //CommonFunctions.SetComboBoxValue(CmbSite, Entity.BTPL_SITE);
                                //CommonFunctions.SetComboBoxValue(CmbWorker, Entity.BTPL_WORKER);
                                //CommonFunctions.SetComboBoxValue(Cmb_UOM, Entity.BTPL_UOM);

                                //txtRate.Text = Entity.BTPL_RATE;
                                //Txt_Cost.Text = Entity.BTPL_AMOUNT;
                                //Txt_Units.Text = Entity.BTPL_UNITS;
                                //Txt_VendNo.Text = Entity.BTPL_VENDOR;
                                //Text_VendName.Text = Get_Vendor_Name(Entity.BTPL_VENDOR);

                                //if (!string.IsNullOrEmpty(Entity.BTPL_PROG.Trim()) && !Entity.BTPL_PROG.Contains("**"))
                                //    Txt_CA_Program.Text = Set_SP_Program_Text(Entity.BTPL_PROG.Trim());

                                ////CommonFunctions.SetComboBoxValue(cmbService, Entity.CAMS_Code);
                                ////Act_Date.Text = Entity.Template_date.ToString();
                                ////if (!string.IsNullOrEmpty(Entity.Template_date.ToString().Trim())) Act_Date.Checked = true;
                                ////CommonFunctions.SetComboBoxValue(cmb_CA_Benefit, Entity.Benefit_from);
                                ////CommonFunctions.SetComboBoxValue(CmbFunding1, Entity.Fund);
                                ////CommonFunctions.SetComboBoxValue(CmbSite, Entity.Site);
                                ////CommonFunctions.SetComboBoxValue(CmbWorker, Entity.CaseWorker);
                                ////CommonFunctions.SetComboBoxValue(Cmb_UOM, Entity.UOM);

                                ////txtRate.Text = Entity.Rate;
                                ////Txt_Cost.Text = Entity.Amount;
                                ////Txt_Units.Text = Entity.Units;
                                ////Txt_VendNo.Text = Entity.Vendor;
                                ////Text_VendName.Text = Get_Vendor_Name(Entity.Vendor);

                                ////if (!string.IsNullOrEmpty(Entity.Program.Trim()) && !Entity.Program.Contains("**"))
                                ////    Txt_CA_Program.Text = Set_SP_Program_Text(Entity.Program.Trim());

                                btnAdd.Text = "Update";

                                cmbService.Enabled = false;
                            }
                        }

                        break;
                    case 12:
                        if (BulkTempList.Count > 0)
                        {
                            //CATemplateEntity Entity = TemplateList.Find(u => u.CAMS_Code == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && u.Template_date == SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString() && u.CA_Seq == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
                            TemplateEntity Entity = BulkTempList.Find(u => u.BTPL_ID == SP_CAMS_Grid.CurrentRow.Cells["gvTempID"].Value.ToString());
                            if (Entity != null)
                            {
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Service- " + SP_CAMS_Grid.CurrentRow.Cells["gvService"].Value.ToString().Trim(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Room_Row);

                                //SP_CAMS_Grid.Rows.RemoveAt(this.SP_CAMS_Grid.SelectedRows[0].Index);

                                //FillGrid();

                            }
                        }
                        break;
                }
            }

        }

        public void Delete_Room_Row(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;


            //if (senderform != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                string strmsg = string.Empty; string strSqlmsg = string.Empty;
                TemplateEntity Entity = new TemplateEntity(true);
                Entity.Rec_Type = "D";
                Entity.BTPL_ID = SP_CAMS_Grid.CurrentRow.Cells["gvTempID"].Value.ToString();

                //CATemplateEntity DEntity = TemplateList.Find(u => u.CAMS_Code == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && u.Template_date == SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString() && u.CA_Seq == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
                //if(DEntity!=null)
                //    TemplateList.Remove(DEntity);


                if (_model.SPAdminData.UpdateBULKTEMPLATE(Entity, "Update", out strmsg, out strSqlmsg))
                {

                    MessageBox.Show(SP_CAMS_Grid.CurrentRow.Cells["gvService"].Value.ToString() + " " + "Deleted Successfully", "CAP Systems", MessageBoxButtons.OK);


                    FillGrid();
                    if (BulkTempList.Count == 0)
                        FillTemplates();

                }
                else
                    if (strmsg == "Already Exist")
                    MessageBox.Show("This Service has used Somewhere So unable to Delete.", "CAP Systems", MessageBoxButtons.OK);
                else
                    MessageBox.Show("UnSuccessfull Delete", "CAP Systems", MessageBoxButtons.OK);
            }
            //}
        }


        private void ClearData()
        {
            CommonFunctions.SetComboBoxValue(cmbService, "0");
            Act_Date.Checked = false;
            if (CategoryCode == "03")
            {
                CommonFunctions.SetComboBoxValue(CmbCat3Funding1, "0");
                CommonFunctions.SetComboBoxValue(CmbCat3Funding2, "0");
                CommonFunctions.SetComboBoxValue(CmbCat3Funding3, "0");

                CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM, "0");
                CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM2, "0");
                CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM3, "0");

                Txt_Cat3Cost.Text = string.Empty; Txt_Cat3Cost2.Text = string.Empty; Txt_Cat3Cost3.Text = string.Empty; txtCat3Total.Text = string.Empty;
            }
            else if (CategoryCode == "02")
            {
                txtCat2_BillPeriod.Text = string.Empty;
                txtCat2BillPCode.Text = string.Empty;
                txtcat2_Acct.Text = string.Empty;
                txtCat2ArrearsAmt.Text = string.Empty;
                txtCat2_Amount.Text = string.Empty;
            }
            else
            {
                Txt_Cost.Text = string.Empty;
                Txt_Units.Text = string.Empty;
                txtRate.Text = string.Empty;
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

        private void Pb_CA_Prog_Click(object sender, EventArgs e)
        {
            string ProgCA = string.Empty;
            if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim())) ProgCA = Txt_CA_Program.Text.Substring(0, 6);
            string Sel_Prog = ProgCA, Sel_SerPlan = SP_Code;
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, Sel_SerPlan, string.Empty);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                Sel_CAMS_Program = form.Selected_SerPlan_Prog();

                //if (CAMS_FLG == "CA")
                Txt_CA_Program.Text = Sel_CAMS_Program;
                //else
                //    Txt_MS_Program.Text = Sel_CAMS_Program;


                //SetComboBoxValue(Cmb_Program, Sel_Prog);
            }
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FillGrid()
        {
            this.SP_CAMS_Grid.SelectionChanged -= new System.EventHandler(this.SP_CAMS_Grid_SelectionChanged);

            SP_CAMS_Grid.Rows.Clear();

            string TemplCode = string.Empty;
            if (((ListItem)cmbTemplate.SelectedItem).Value.ToString() != "0") TemplCode = ((ListItem)cmbTemplate.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(TemplCode.Trim()))
            {
                BulkTempList = _model.SPAdminData.Browse_Templates(string.Empty, TemplCode, SP_Code, string.Empty, BranchCode, string.Empty);
                BulkTempList = BulkTempList.FindAll(u => u.BTPL_ADD_OPERATOR.Trim() == BaseForm.UserID.Trim());

                if (BulkTempList.Count > 0)
                {
                    int rowIndex = 0;
                    foreach (TemplateEntity Entity in BulkTempList)
                    {

                        string FundName = string.Empty;
                        if (FundingList.Count > 0)
                        {

                            if (!string.IsNullOrEmpty(Entity.BTPL_FUND.Trim()))
                            {
                                SPCommonEntity FEnt = FundingList.Find(u => u.Code.Trim() == Entity.BTPL_FUND.Trim());
                                if (FEnt != null) FundName = FEnt.Desc.Trim();
                            }
                        }

                        string UOMDesc = string.Empty;
                        if (UOMList.Count > 0)
                        {

                            if (!string.IsNullOrEmpty(Entity.BTPL_UOM.Trim()))
                            {
                                SPCommonEntity FEnt = UOMList.Find(u => u.Code.Trim() == Entity.BTPL_UOM.Trim());
                                if (FEnt != null) UOMDesc = FEnt.Desc.Trim();
                            }
                        }

                        string CADesc = string.Empty;
                        if (SP_CAMS_Details.Count > 0)
                        {
                            CASESP2Entity SP2Entity = SP_CAMS_Details.Find(u => u.CamCd.Trim() == Entity.BTPL_CACODE.Trim() && u.Type1 == "CA");
                            if (SP2Entity != null) CADesc = SP2Entity.CAMS_Desc.Trim();
                        }

                        string Amount = string.Empty;
                        Amount = Entity.BTPL_TOTAL.Trim();


                        rowIndex = SP_CAMS_Grid.Rows.Add(CADesc.Trim(), LookupDataAccess.Getdate(Entity.BTPL_DATE.Trim()), FundName, UOMDesc, Entity.BTPL_UNITS.ToString(), Entity.BTPL_RATE, Amount, Entity.BTPL_CACODE, Entity.BTPL_CASEQ, Entity.BTPL_OBF, Entity.BTPL_ID);

                        rowIndex++;
                    }

                    if (SP_CAMS_Grid.Rows.Count > 0)
                    {
                        //this.SP_CAMS_Grid.SelectionChanged -= new System.EventHandler(this.SP_CAMS_Grid_SelectionChanged);
                        SP_CAMS_Grid.Update();
                        SP_CAMS_Grid.CurrentCell = SP_CAMS_Grid.Rows[0].Cells[1];

                        this.SP_CAMS_Grid.SelectionChanged += new System.EventHandler(this.SP_CAMS_Grid_SelectionChanged);
                        SP_CAMS_Grid_SelectionChanged(SP_CAMS_Grid, EventArgs.Empty);
                    }

                }

            }

        }

        //private void FillGrid()
        //{
        //    SP_CAMS_Grid.Rows.Clear();


        //    BulkTempList= _model.SPAdminData.Browse_Templates(string.Empty,string.Empty, SP_Code,SP_Sequence,BranchCode,string.Empty);

        //    if (TemplateList.Count>0)
        //    {
        //        int rowIndex = 0;
        //        foreach (CATemplateEntity Entity in TemplateList)
        //        {

        //            string FundName = string.Empty;
        //            if (FundingList.Count > 0)
        //            {

        //                if (!string.IsNullOrEmpty(Entity.Fund.Trim()))
        //                {
        //                    SPCommonEntity FEnt = FundingList.Find(u => u.Code.Trim() == Entity.Fund.Trim());
        //                    if (FEnt != null) FundName = FEnt.Desc.Trim();
        //                }
        //            }

        //            string UOMDesc = string.Empty;
        //            if (UOMList.Count > 0)
        //            {

        //                if (!string.IsNullOrEmpty(Entity.UOM.Trim()))
        //                {
        //                    SPCommonEntity FEnt = UOMList.Find(u => u.Code.Trim() == Entity.UOM.Trim());
        //                    if (FEnt != null) UOMDesc = FEnt.Desc.Trim();
        //                }
        //            }

        //            rowIndex = SP_CAMS_Grid.Rows.Add(Entity.CADesc.Trim(), Entity.Template_date, FundName, UOMDesc, Entity.Units.ToString(), Entity.Rate, Entity.Amount, Entity.CAMS_Code, Entity.CA_Seq,Entity.Benefit_from);

        //            rowIndex++;
        //        }

        //        if(SP_CAMS_Grid.Rows.Count > 0)
        //        {
        //            //this.SP_CAMS_Grid.SelectionChanged -= new System.EventHandler(this.SP_CAMS_Grid_SelectionChanged);
        //            SP_CAMS_Grid.Update();
        //            SP_CAMS_Grid.CurrentCell = SP_CAMS_Grid.Rows[0].Cells[1];
        //        }

        //    }
        //}

        public List<Captain.Common.Utilities.ListItem> Getdata()
        {

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();

            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                var Services = SP_CAMS_Grid.Rows.Cast<DataGridViewRow>()
                           .Where(x => !x.IsNewRow)                   // either..
                                                                      //.Where(x => x.Cells["gvcService"].Value != null) //..or or both
                           .Select(x => x.Cells["gvCACode"].Value.ToString().Trim())
                           .Distinct()
                           .ToList();

                if (Services.Count > 0)
                {
                    foreach (var entity in Services)
                    {
                        foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
                        {
                            string Service = dr.Cells["gvCACode"].Value == null ? string.Empty : dr.Cells["gvCACode"].Value.ToString().Trim();
                            if (entity.Trim() == Service.Trim())
                            {
                                CASESP2Entity SelRec = CA_Details.Find(u => u.CamCd.Trim() == Service);

                                if (SelRec != null)
                                {
                                    listItem.Add(new Captain.Common.Utilities.ListItem("CA" + SelRec.CamCd.Trim() + SelRec.Orig_Grp + SelRec.Branch.Trim(), string.Empty, dr.Cells["gvDate"].Value.ToString().Trim(), string.Empty));

                                }

                            }
                        }
                    }
                }

            }

            return listItem;

        }

        private void cmbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbTemplate.SelectedItem).Value.ToString() == "0")
            {
                ClearAllData();
                txtTemplate.Visible = true;
                chkbSPM.Checked = false;
                chkbAllowPost.Checked = false;
                FillGrid();
                panel4.Enabled = false;
            }
            else
            {
                txtTemplate.Visible = false;

                FillGrid();
                panel4.Enabled = false;

            }
        }

        public List<CATemplateEntity> GetTotaldata()
        {

            //List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();

            //List<DataGridViewRow> SelectedGvrows = (from c in SP_CAMS_Grid.Rows.Cast<DataGridViewRow>().ToList()
            //                                          where (((DataGridViewCheckBoxCell)c.Cells["gvService"]).Value.ToString()!=string.Empty)
            //                                          select (DataGridViewRow)c).ToList();

            List<CATemplateEntity> SelTempList = new List<CATemplateEntity>();

            if (TemplateList.Count > 0)
            {
                SelTempList = TemplateList;
            }

            //if (SP_CAMS_Grid.Rows.Count > 0)
            //{
            //    var Services = SP_CAMS_Grid.Rows.Cast<DataGridViewRow>()
            //               .Where(x => !x.IsNewRow)
            //               .Select(x => x.Cells["gvCACode"].Value.ToString().Trim())
            //               .Distinct()
            //               .ToList();

            //    if (Services.Count > 0)
            //    {
            //        foreach (var entity in Services)
            //        {
            //            foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
            //            {
            //                string Service = dr.Cells["gvCACode"].Value == null ? string.Empty : dr.Cells["gvCACode"].Value.ToString().Trim();
            //                if (entity.Trim() == Service.Trim())
            //                {
            //                    CASESP2Entity SelRec = CA_Details.Find(u => u.CamCd.Trim() == Service);

            //                    if (SelRec != null)
            //                    {
            //                        CATemplateEntity TemplateEntity = new CATemplateEntity();

            //                        TemplateEntity.CADesc = dr.Cells["gvService"].ValueText.ToString().Trim();
            //                        TemplateEntity.Benefit_from = dr.Cells["gvBenefit"].Value.ToString().Trim();
            //                        TemplateEntity.Template_date = dr.Cells["gvDate"].Value.ToString().Trim();
            //                        TemplateEntity.Posting_Date = string.Empty;
            //                        TemplateEntity.Remarks = string.Empty;
            //                        TemplateEntity.Add_edit = string.Empty;
            //                        TemplateEntity.SP2_Type = SelRec.Type1;
            //                        TemplateEntity.CAMS_Code = SelRec.CamCd;
            //                        TemplateEntity.SP2_Operation = "C";
            //                        TemplateEntity.Branch = BranchCode;
            //                        TemplateEntity.Group = SelRec.Orig_Grp.ToString();
            //                        TemplateEntity.Notes_Sw = string.Empty;
            //                        TemplateEntity.Notes_Key = string.Empty;
            //                        TemplateEntity.CA_Seq = string.Empty;
            //                        TemplateEntity.Dup_desc = SelRec.CAMS_Desc;
            //                        TemplateEntity.SP2_Year = string.Empty;
            //                        TemplateEntity.CAMS_Active_Stat = SelRec.CAMS_Active;
            //                        TemplateEntity.SP2_ID = string.Empty;
            //                        TemplateEntity.Mem_List = string.Empty;
            //                        TemplateEntity.Dup_MS_Date = "N";
            //                        TemplateEntity.AddedXml = "N";
            //                        TemplateEntity.Exp_Post_Date = string.Empty;
            //                        TemplateEntity.Can_Post = "N";
            //                        TemplateEntity.Post_type = string.Empty;
            //                        TemplateEntity.Vendor = dr.Cells["gvVendor"].ValueText.ToString().Trim();
            //                        TemplateEntity.Fund = dr.Cells["gvFund"].ValueText.ToString().Trim();
            //                        TemplateEntity.UOM = dr.Cells["gvUOM"].ValueText.ToString().Trim();
            //                        TemplateEntity.Units = dr.Cells["gvUnits"].ValueText.ToString().Trim();
            //                        TemplateEntity.Rate = dr.Cells["gvRate"].ValueText.ToString().Trim();
            //                        TemplateEntity.Amount = dr.Cells["gvAmount"].ValueText.ToString().Trim();



            //                        SelTempList.Add(new CATemplateEntity(TemplateEntity));

            //                    }

            //                }
            //            }
            //        }
            //    }

            //}


            return SelTempList;

        }

        private void Pb_Add_CA_Click(object sender, EventArgs e)
        {
            if (((ListItem)cmbTemplate.SelectedItem).Value.ToString() == "0")
            {
                if (string.IsNullOrEmpty(txtTemplate.Text.Trim()))
                    MessageBox.Show("First fill Template Name");
                else
                {
                    ClearAllData();
                    panel4.Enabled = true;
                    btnOK.Visible = false;
                    chkbSPM.Enabled = true;
                    chkbAllowPost.Enabled = true;
                }

            }
            else
            {
                ClearAllData();
                panel4.Enabled = true;
                btnOK.Visible = false;
                chkbSPM.Enabled = true;
                chkbAllowPost.Checked = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
            CommonFunctions.SetComboBoxValue(CmbFunding1, "0");
            CommonFunctions.SetComboBoxValue(CmbSite, "0");
            CommonFunctions.SetComboBoxValue(CmbWorker, "0");
            CommonFunctions.SetComboBoxValue(Cmb_UOM, "0");
            Txt_VendNo.Text = string.Empty; Text_VendName.Text = string.Empty;
            panel4.Enabled = false;
            btnOK.Visible = true; chkbSPM.Enabled = false; chkbAllowPost.Enabled = false;
            if (SP_CAMS_Grid.Rows.Count > 0)
                SP_CAMS_Grid_SelectionChanged(sender, e);
        }

        private void ClearAllData()
        {
            ClearData();
            CommonFunctions.SetComboBoxValue(CmbFunding1, "0");
            CommonFunctions.SetComboBoxValue(CmbSite, "0");
            CommonFunctions.SetComboBoxValue(CmbWorker, "0");
            CommonFunctions.SetComboBoxValue(Cmb_UOM, "0");
            Txt_VendNo.Text = string.Empty; Text_VendName.Text = string.Empty;
            panel4.Enabled = false;
            btnOK.Visible = true;
        }

        private void SP_CAMS_Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                if (BulkTempList.Count > 0)
                {
                    ///CATemplateEntity Entity = TemplateList.Find(u => u.CAMS_Code == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && u.Template_date==SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString() && u.CA_Seq == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
                    //TemplateEntity Entity = BulkTempList.Find(u => u.BTPL_CACODE == SP_CAMS_Grid.SelectedRows[0].Cells["gvCACode"].Value.ToString() && Convert.ToDateTime(u.BTPL_DATE.Trim()) == Convert.ToDateTime(SP_CAMS_Grid.SelectedRows[0].Cells["gvDate"].Value.ToString().Trim()) && u.BTPL_CASEQ == SP_CAMS_Grid.SelectedRows[0].Cells["gvCASeq"].Value.ToString());
                    TemplateEntity Entity = BulkTempList.Find(u => u.BTPL_ID == SP_CAMS_Grid.SelectedRows[0].Cells["gvTempID"].Value.ToString());
                    if (Entity != null)
                    {
                        //panel3.Enabled = true; btnOK.Visible = false;
                        CommonFunctions.SetComboBoxValue(cmbService, Entity.BTPL_CACODE);
                        Act_Date.Text = Entity.BTPL_DATE.ToString();
                        if (!string.IsNullOrEmpty(Entity.BTPL_DATE.ToString().Trim())) Act_Date.Checked = true;
                        CommonFunctions.SetComboBoxValue(cmb_CA_Benefit, Entity.BTPL_OBF);

                        CommonFunctions.SetComboBoxValue(CmbSite, Entity.BTPL_SITE);
                        CommonFunctions.SetComboBoxValue(CmbWorker, Entity.BTPL_WORKER);

                        if (CategoryCode == "03")
                        {
                            CommonFunctions.SetComboBoxValue(CmbCat3Funding1, Entity.BTPL_FUND);
                            CommonFunctions.SetComboBoxValue(CmbCat3Funding2, Entity.BTPL_FUND2);
                            CommonFunctions.SetComboBoxValue(CmbCat3Funding3, Entity.BTPL_FUND3);

                            CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM, Entity.BTPL_UOM);
                            CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM2, Entity.BTPL_UOM2);
                            CommonFunctions.SetComboBoxValue(Cmb_Cat3UOM3, Entity.BTPL_UOM3);

                            Txt_Cat3Cost.Text = Entity.BTPL_AMOUNT;
                            Txt_Cat3Cost2.Text = Entity.BTPL_AMOUNT2;
                            Txt_Cat3Cost3.Text = Entity.BTPL_AMOUNT3;
                            txtCat3Total.Text = Entity.BTPL_TOTAL;
                        }
                        else if (CategoryCode == "02")
                        {
                            txtCat2BillPCode.Text = Entity.BTPL_BILL_PERIOD;
                            if (!string.IsNullOrEmpty(Entity.BTPL_BILL_PERIOD.Trim()))
                            {
                                ListcommonEntity = new List<CommonEntity>();
                                string[] CountyList = Entity.BTPL_BILL_PERIOD.Split(',');
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
                            if (!string.IsNullOrEmpty(Entity.BTPL_VEND_ACCT.Trim()))
                                txtcat2_Acct.Text = Entity.BTPL_VEND_ACCT.Trim();
                            if (!string.IsNullOrEmpty(Entity.BTPL_ARREARS.Trim()))
                                txtCat2ArrearsAmt.Text = Entity.BTPL_ARREARS;
                            if (!string.IsNullOrEmpty(Entity.BTPL_TOTAL.Trim()))
                                txtCat2_Amount.Text = Entity.BTPL_TOTAL;
                        }
                        else
                        {
                            CommonFunctions.SetComboBoxValue(CmbFunding1, Entity.BTPL_FUND);
                            CommonFunctions.SetComboBoxValue(Cmb_UOM, Entity.BTPL_UOM);

                            txtRate.Text = Entity.BTPL_RATE;
                            Txt_Cost.Text = Entity.BTPL_TOTAL;
                            Txt_Units.Text = Entity.BTPL_UNITS;
                        }

                        Txt_VendNo.Text = Entity.BTPL_VENDOR;
                        Text_VendName.Text = Get_Vendor_Name(Entity.BTPL_VENDOR);

                        if (Entity.BTPL_SPM == "Y") chkbSPM.Checked = true; else chkbSPM.Checked = false;
                        if (Entity.BTPL_ALLOW_POST == "Y") chkbAllowPost.Checked = true; else chkbAllowPost.Checked = false;

                        if (!string.IsNullOrEmpty(Entity.BTPL_PROG.Trim()) && !Entity.BTPL_PROG.Contains("**"))
                            Txt_CA_Program.Text = Set_SP_Program_Text(Entity.BTPL_PROG.Trim());


                    }
                }
            }
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            float V1 = 0, V2 = 0, V3 = 0;
            if (!string.IsNullOrEmpty(Txt_Units.Text.Trim())) V1 = float.Parse(Txt_Units.Text.Trim());
            if (!string.IsNullOrEmpty(txtRate.Text.Trim())) V2 = float.Parse(txtRate.Text.Trim());
            //if (!string.IsNullOrEmpty(Txt_Cost3.Text.Trim())) V3 = float.Parse(Txt_Cost3.Text.Trim());

            if (!string.IsNullOrEmpty(Txt_Units.Text.Trim()) && !string.IsNullOrEmpty(txtRate.Text.Trim()))
                Txt_Cost.Text = (V1 * V2).ToString("0.00");
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

        private void Cmb_UOM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public List<CommonEntity> ListcommonEntity { get; set; }
        List<CommonEntity> BillPeriodEntity = new List<CommonEntity>();
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


        public string GetTemplateCode()
        {
            string TempCode = string.Empty;
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                if (((ListItem)cmbTemplate.SelectedItem).Value.ToString() != "0")
                    TempCode = ((ListItem)cmbTemplate.SelectedItem).Value.ToString();
            }

            return TempCode;
        }


    }
}