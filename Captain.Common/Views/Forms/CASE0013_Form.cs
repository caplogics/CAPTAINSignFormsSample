#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0013_Form : Form
    {
        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public CASE0013_Form(BaseForm baseForm, string form_display_mode, List<PARTNEREFEntity> sel_REFS_List, string strdate, string strReferfromto, string strMode,PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            //propReportPath = _model.lookupDataAccess.GetReportPath();

            BaseForm = baseForm;
            Privileges = privileges;
            Form_Display_Mode = form_display_mode;
            Referfromto = strReferfromto;
            Sel_REFS_List = null;
            ActRefsList = sel_REFS_List;
            ReferDate = strdate;
            Mode = strMode;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetPrivilizes_byScrCode(BaseForm.UserID, "01", "ADMN0011");
            dtScreen = ds.Tables[0];

            this.Text = "Referred From/To Details" + " - " + Mode;

            Get_Partner_Data();
            FillCombo();

            if (Mode == "Edit")
            {
                if (dtScreen.Rows.Count > 0 && !string.IsNullOrEmpty(txtAgencyName.Text.Trim()))
                {
                    if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "F")
                        btnAddRep.Visible = true;
                }

                this.RefDet_Grid.AllowUserToAddRows = false;
                fillRefferdGrid();
            }
            
        }

        DataTable dtScreen = new DataTable();

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Form_Display_Mode { get; set; }

        public string CAMS_Desc { get; set; }

        public string Hierarchy { get; set; }

        public string code { get; set; }

        public List<ACTREFSEntity> Sel_REFS_List { get; set; }

        public List<PARTNEREFEntity> ActRefsList { get; set; }

        public List<CASEREFSEntity> Sel_CASEREFS_List { get; set; }

        public List<CaseVDD1Entity> Sel_CASEVDD1_List { get; set; }

        public CASEMSEntity Pass_MS_Entity { get; set; }

        public string ReferDate { get; set; }

        public string Referfromto { get; set; }

        public string Mode { get; set; }

        public string FormName { get; set; }

        public List<AGCYPARTEntity> propPartner_List { get; set; }
        public List<AGCYSEREntity> AGYSERList { get; set; }
        public List<AGCYREPEntity> AgyRepList { get; set; }
        public List<CommonEntity> commonServices { get; set; }

        #endregion
        

        private void Get_Partner_Data()
        {
            AGCYPARTEntity Search_Entity = new AGCYPARTEntity(true);
            propPartner_List = _model.SPAdminData.Browse_AgencyPartner(Search_Entity, "Browse");

            AGCYSEREntity SearchSub_Entity = new AGCYSEREntity(true);
            AGYSERList = _model.SPAdminData.Browse_AGCYServices(SearchSub_Entity, "Browse");

            AGCYREPEntity SearchRep_Entity = new AGCYREPEntity(true);
            AgyRepList = _model.SPAdminData.Browse_AGCYREP(SearchRep_Entity, "Browse");

            commonServices = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "37000", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            
        }

        private void fillRefferdGrid()
        {
            RefDet_Grid.Rows.Clear();
            int rowIndex = 0;
            if(ActRefsList.Count>0)
            {
                calDate.Enabled = false;
                cmbReferFromTo.Enabled = false;
                Pb_Ref_Agency.Enabled = false;

                calDate.Text = ReferDate.ToString();
                CommonFunctions.SetComboBoxValue(cmbReferFromTo, Referfromto);
                txtCode.Text = ActRefsList[0].Code;
                string AgencyDesc = string.Empty;
                if (propPartner_List.Count > 0)
                {
                    AgencyDesc = propPartner_List.Find(u => u.Code.Trim().Equals(txtCode.Text.Trim())).Name.Trim();
                }
                txtAgencyName.Text = AgencyDesc.Trim();

                if(Mode=="Edit")
                {
                    if (dtScreen.Rows.Count > 0 && !string.IsNullOrEmpty(txtAgencyName.Text.Trim()))
                    {
                        if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "F")
                            btnAddRep.Visible = true;
                    }
                }

                fillRefferedGridData(((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim());
            }
        }

        private void FillCombo()
        {
            cmbReferFromTo.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem(" ", "0"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Referred From", "F"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Referred To", "T"));
            cmbReferFromTo.Items.AddRange(listItem.ToArray());
            cmbReferFromTo.SelectedIndex = 0;

            //AGCYPARTEntity Search_Entity = new AGCYPARTEntity(true);
            //propPartner_List = _model.SPAdminData.Browse_AgencyPartner(Search_Entity, "Browse");

            //List<Captain.Common.Utilities.ListItem> listItem1 = new List<Captain.Common.Utilities.ListItem>();
            //listItem1.Add(new Captain.Common.Utilities.ListItem("   ", "0"));

            //if(propPartner_List.Count>0)
            //{
            //    foreach (AGCYPARTEntity Entity in propPartner_List)
            //        listItem1.Add(new Captain.Common.Utilities.ListItem(Entity.Name.Trim(), Entity.Code.Trim()));
            //}

            //cmbAgencyName.Items.AddRange(listItem1.ToArray());
            //cmbAgencyName.SelectedIndex = 0;

            //listItem1.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString().Trim(), dr["SITE_ACTIVE"].ToString().Trim(), (dr["SITE_ACTIVE"].ToString().Trim().Equals("Y") ? Color.Green : Color.Red)));
        }

        List<CommonEntity> MembersEntity = new List<CommonEntity>();
        private void FillRepCombo()
        {
            DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbAgyRep"];
            MembersEntity = new List<CommonEntity>();
            MembersEntity.Add(new CommonEntity("0", " ", string.Empty));
            List<AGCYREPEntity> SelRepList = new List<AGCYREPEntity>();
            if (!string.IsNullOrEmpty(txtAgencyName.Text.Trim()) && AgyRepList.Count>0)
            {
                if(!string.IsNullOrEmpty(txtAgencyName.Text.Trim()))
                {
                    string Name = null;
                    SelRepList = AgyRepList.FindAll(u => u.PartCode.Trim().Equals(txtCode.Text.Trim()));
                    if (SelRepList.Count > 0)
                    {
                        SelRepList= SelRepList.OrderBy(u=>u.FName).ThenBy(u=>u.LName).ToList();

                        foreach (AGCYREPEntity Entity in SelRepList)
                        {
                            Name = null;
                            if (!string.IsNullOrEmpty(Entity.FName.Trim()))
                                Name = Entity.FName.Trim();
                            if (!string.IsNullOrEmpty(Entity.LName.Trim()))
                                Name = Name + " " + Entity.LName.Trim();

                            MembersEntity.Add(new CommonEntity(Entity.RepCode, Name));
                        }
                    }
                }
            }

            cb.DataSource = MembersEntity;
            cb.DisplayMember = "Desc";
            cb.ValueMember = "Code";
            //CssStyle = "border-radius:0px; border:0px solid #FF0000;";
            //cb.Items.Add(new ListItem(cb.DataSource.ToString(), CssStyle));
            cb = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbAgyRep"];
        }

        List<CommonEntity> CategoryList = new List<CommonEntity>();
        List<CommonEntity> ServiceList = new List<CommonEntity>();
        private void FillCategoriescombo()
        {
            DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbCatg"];
            CategoryList = new List<CommonEntity>();

            if (AGYSERList.Count > 0)
            {
                List<AGCYSEREntity> SelCategories = AGYSERList.FindAll(u => u.PartCode.Equals(txtCode.Text.Trim()));

                if (SelCategories.Count > 0)
                {
                    foreach (AGCYSEREntity Entity in SelCategories)
                    {
                        string Desc = string.Empty;
                        Desc = commonServices.Find(u => u.Code.Trim().Equals(Entity.Category.Trim())).Desc.Trim();

                        CategoryList.Add(new CommonEntity(Entity.Category.Trim()+Entity.SerCode, Desc.Trim()+" - "+Entity.Service.Trim()));
                    }

                }

                cb.DataSource = CategoryList;
                cb.DisplayMember = "Desc";
                cb.ValueMember = "Code";
                cb = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbCatg"];
            }


            List<CommonEntity> AgyTabs_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0071", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);

            if (AgyTabs_List.Count > 0)
            {
                DataGridViewComboBoxColumn cbstatus = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbStatus"];
                AgyTabs_List = AgyTabs_List.OrderBy(u => u.Desc.Trim()).ToList();


                cbstatus.DataSource = AgyTabs_List;
                cbstatus.DisplayMember = "Desc";
                cbstatus.ValueMember = "Code";
                cbstatus = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbStatus"];
            }

        }

        //private void FillServicescombo(string Category)
        //{
        //    DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbService"];
        //    ServiceList = new List<CommonEntity>();

        //    if (AGYSERList.Count > 0)
        //    {
        //        List<AGCYSEREntity> SelCategories = AGYSERList.FindAll(u => u.PartCode.Equals(txtCode.Text.Trim()) && u.Category.Equals(Category));

        //        if (SelCategories.Count > 0)
        //        {
        //            foreach (AGCYSEREntity Entity in SelCategories)
        //            {
        //                //string Desc = string.Empty;
        //                //Desc = commonServices.Find(u => u.Code.Trim().Equals(Entity.Category.Trim())).Desc.Trim();

        //                ServiceList.Add(new CommonEntity(Entity.SerCode.Trim(), Entity.Service.Trim()));
        //            }

        //        }

        //        cb.DataSource = ServiceList;
        //        cb.DisplayMember = "Desc";
        //        cb.ValueMember = "Code";
        //        cb = (DataGridViewComboBoxColumn)this.RefDet_Grid.Columns["cmbService"];
        //    }


        //}

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool isValid()
        {
            bool isValid = true;

            if (calDate.Checked == false)
            {
                _errorProvider.SetError(calDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(calDate, null);
            }

            if ((cmbReferFromTo.SelectedItem == null || ((ListItem)cmbReferFromTo.SelectedItem).Value == "0"))
            {
                _errorProvider.SetError(cmbReferFromTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReferFromto.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbReferFromTo, null);

                if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString() == "F")
                {
                    if (RefDet_Grid.Rows.Count > 1)
                    {
                        int RepCnt = 0;
                        foreach (DataGridViewRow dr in RefDet_Grid.Rows)
                        {
                            string Rep = dr.Cells["cmbAgyRep"].Value == null ? string.Empty : dr.Cells["cmbAgyRep"].Value.ToString().Trim();
                            
                            if (string.IsNullOrEmpty(Rep.Trim()))
                            {
                                RepCnt++;
                                if (RepCnt > 1)
                                {
                                    isValid = false;
                                    AlertBox.Show("Please select Agency Representative", MessageBoxIcon.Warning);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        isValid = false;
                        AlertBox.Show("Please select Agency Representative", MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    if (RefDet_Grid.Rows.Count > 1)
                    {
                        int catgcnt = 0,sercnt=0;
                        foreach (DataGridViewRow dr in RefDet_Grid.Rows)
                        {
                            string catg = dr.Cells["cmbCatg"].Value == null ? string.Empty : dr.Cells["cmbCatg"].Value.ToString().Trim();
                            string status = dr.Cells["cmbStatus"].Value == null ? string.Empty : dr.Cells["cmbStatus"].Value.ToString().Trim();
                            string RefDate = dr.Cells["RefStat_Date"].Value == null ? string.Empty : dr.Cells["RefStat_Date"].Value.ToString().Trim();
                            if (string.IsNullOrEmpty(catg.Trim()))
                            {
                                catgcnt++;
                                if (catgcnt > 1)
                                {
                                    isValid = false;
                                    AlertBox.Show("Please select Category", MessageBoxIcon.Warning);
                                    break;
                                }
                            }

                            //if(string.IsNullOrEmpty(service.Trim()))
                            //{
                            //    sercnt++;
                            //    if (sercnt > 1)
                            //    {
                            //        isValid = false;
                            //        MessageBox.Show("Please select Service");
                            //        break;
                            //    }
                            //}
                            
                            if(!string.IsNullOrEmpty(status.Trim()))
                            {
                                if (string.IsNullOrEmpty(RefDate.Trim()))
                                {
                                    isValid = false;
                                    AlertBox.Show("Please fill the Status Date", MessageBoxIcon.Warning);
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {
                        isValid = false;
                        AlertBox.Show("Please select Category", MessageBoxIcon.Warning);
                        
                    }
                }

            }

            if (string.IsNullOrEmpty(txtAgencyName.Text.Trim()))
            {
                _errorProvider.SetError(Pb_Ref_Agency, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAgencyName.Text));
                isValid = false;
            }
            else
                _errorProvider.SetError(Pb_Ref_Agency, null);


            return isValid;
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<PARTNEREFEntity> GetSelected_Partner_Entity()
        {
            List<PARTNEREFEntity> Sele_REFS_List = new List<PARTNEREFEntity>();
            PARTNEREFEntity Add_Entity = new PARTNEREFEntity();

            
            Add_Entity.Type= ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString();
            

            foreach (DataGridViewRow dr in RefDet_Grid.Rows)
            {
                if(Add_Entity.Type=="F")
                {
                    
                    string Rep = dr.Cells["cmbAgyRep"].Value == null ? string.Empty : dr.Cells["cmbAgyRep"].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(Rep.Trim()))
                    {
                        Add_Entity.Rec_Type = "I";
                        Add_Entity.Date = calDate.Value.ToString();
                        Add_Entity.Code = txtCode.Text.Trim();
                        string RefStatDate = dr.Cells["RefStat_Date"].Value == null ? string.Empty : dr.Cells["RefStat_Date"].Value.ToString().Trim();
                        Add_Entity.RefExpDate = RefStatDate;
                        Add_Entity.REPRESENTATIVE = Rep.Trim();
                        string RefID = dr.Cells["gvRefID"].Value == null ? string.Empty : dr.Cells["gvRefID"].Value.ToString().Trim();
                        Add_Entity.RefID = RefID;

                        Sele_REFS_List.Add(new PARTNEREFEntity(Add_Entity));
                    }
                }
                else if(Add_Entity.Type=="T")
                {
                    
                    string catg = dr.Cells["cmbCatg"].Value == null ? string.Empty : dr.Cells["cmbCatg"].Value.ToString().Trim();
                    //string Service = dr.Cells["cmbService"].Value == null ? string.Empty : dr.Cells["cmbService"].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(catg.Trim()))
                    {
                        Add_Entity.Rec_Type = "I";
                        Add_Entity.Date = calDate.Value.ToString();
                        Add_Entity.Code = txtCode.Text.Trim();

                        Add_Entity.Category = catg.Substring(0,2).Trim();
                        //Add_Entity.Service = Service.Trim();
                        Add_Entity.Service = catg.Substring(2,catg.Length-2).Trim();

                        string Status = dr.Cells["cmbStatus"].Value == null ? string.Empty : dr.Cells["cmbStatus"].Value.ToString().Trim();
                        Add_Entity.Status = Status;
                        string RefStatDate = dr.Cells["RefStat_Date"].Value == null ? string.Empty : dr.Cells["RefStat_Date"].Value.ToString().Trim();
                        Add_Entity.StatusDate= RefStatDate;
                        
                        string RefID = dr.Cells["gvRefID"].Value == null ? string.Empty : dr.Cells["gvRefID"].Value.ToString().Trim();
                        Add_Entity.RefID = RefID;

                        Sele_REFS_List.Add(new PARTNEREFEntity(Add_Entity));
                    }
                }

                //if (dr.Cells["Selected"].Value.ToString() == "Y")
                //{
                //    Add_Entity.Rec_Type = "I";
                //    Add_Entity.Code = dr.Cells["Code"].Value.ToString();
                //    Add_Entity.Date = calDate.Value.ToString();
                //    Add_Entity.Type = ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString();
                //    //Add_Entity.NameIndex = dr.Cells["gvtNameIndex"].Value == null ? string.Empty : dr.Cells["gvtNameIndex"].Value.ToString();
                //    //Add_Entity.Service = null;
                //    //Add_Entity.Seq = null;
                //    //if (ActRefsList.Count > 0)
                //    //{
                //    //    ACTREFSEntity actconnectedentity = ActRefsList.Find(u => u.Code == Add_Entity.Code);
                //    //    if (actconnectedentity != null)
                //    //        Add_Entity.Connected = actconnectedentity.Connected;
                //    //    else
                //    //        Add_Entity.Connected = "N";
                //    //}
                //    //else
                //    //{
                //    //    Add_Entity.Connected = "N";
                //    //}
                //    Sele_REFS_List.Add(new ACTREFSEntity(Add_Entity));
                //}
            }

            return Sele_REFS_List;
        }

        private void cmbReferFromTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Equals("0"))
            {
                if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "F")
                {
                    RefDet_Grid.CellValueChanged -= new DataGridViewCellEventHandler(RefDet_Grid_CellValueChanged);
                    this.cmbAgyRep.Visible = true;
                    cmbAgyRep.ShowInVisibilityMenu = true;
                    this.cmbCatg.Visible = false;
                    cmbCatg.ShowInVisibilityMenu = false;
                    this.cmbStatus.Visible = false;
                    cmbStatus.ShowInVisibilityMenu = false;
                    //this.cmbService.Visible = false;
                    this.RefStat_Date.HeaderText = "Referral Expiration Date";
                    this.cmbAgyRep.Width = 400;
                    this.RefStat_Date.Width = 220;//180
                    pnlDetails.Visible = true;
                    btnAddRep.Text = "Add Representative";
                    
                    

                    RefDet_Grid.CellValueChanged += new DataGridViewCellEventHandler(RefDet_Grid_CellValueChanged);
                    cmbAgencyName_SelectedIndexChanged(sender, e);

                }
                else if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "T")
                {
                    RefDet_Grid.CellValueChanged -= new DataGridViewCellEventHandler(RefDet_Grid_CellValueChanged);
                    this.cmbAgyRep.Visible = false;
                    cmbAgyRep.ShowInVisibilityMenu = false;
                    this.cmbCatg.Visible = true;
                    cmbCatg.ShowInVisibilityMenu = true;
                    this.cmbStatus.Visible = true;
                    cmbStatus.ShowInVisibilityMenu = true;
                    
                    this.RefStat_Date.HeaderText = "Status Date";
                    //this.cmbCatg.Width = 200;
                    //this.cmbStatus.Width = 240;
                    //this.RefStat_Date.Width = 180;//180//220

                    this.cmbCatg.Width = 280;
                    this.cmbStatus.Width = 230;
                    this.RefStat_Date.Width = 120;//180//220
                    //this.cmbService.Width = 140;
                    pnlDetails.Visible = true;
                    //btnAddRep.Text = "Add Services";
                    btnAddRep.Visible = false;

                    


                    RefDet_Grid.CellValueChanged += new DataGridViewCellEventHandler(RefDet_Grid_CellValueChanged);


                    cmbAgencyName_SelectedIndexChanged(sender, e);
                }
                else
                    pnlDetails.Visible = false;
            }
        }

        private void cmbAgencyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAgencyName.Text.Trim()) && ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() != "0")
            {
                if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "F")
                {
                    FillRepCombo();
                    RefDet_Grid.Rows.Clear();
                    RefDet_Grid.AllowUserToAddRows = true;
                    RefDet_Grid.Rows[0].Selected = true;
                }
                else if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "T")
                {
                    FillCategoriescombo();
                    RefDet_Grid.Rows.Clear();
                    RefDet_Grid.AllowUserToAddRows = true;
                    RefDet_Grid.Rows[0].Selected = true;
                }
                else
                    RefDet_Grid.AllowUserToAddRows = false;
            }
        }

        private void fillRefferedGridData(string Type)
        {
            RefDet_Grid.Rows.Clear();
            int rowIndex = 0;
            if(ActRefsList.Count>0)
            {
                List<PARTNEREFEntity> selPartnerRefList = ActRefsList.FindAll(u => u.Type.Equals(Type.Trim()));

                //selPartnerRefList = selPartnerRefList.OrderByDescending(u => u.REPRESENTATIVE).ToList();

                if (selPartnerRefList.Count > 0)
                {
                    if (Type == "F")
                    {
                        foreach (PARTNEREFEntity Entity in selPartnerRefList)
                        {
                            rowIndex = RefDet_Grid.Rows.Add(Entity.REPRESENTATIVE, string.Empty, string.Empty, LookupDataAccess.Getdate(Entity.RefExpDate.Trim()), Entity.Code, Entity.RefID);
                        }
                        //rowIndex=RefDet_Grid.Rows.Add()
                    }
                    else if (Type == "T")
                    {
                        foreach (PARTNEREFEntity Entity in selPartnerRefList)
                        {
                            rowIndex = RefDet_Grid.Rows.Add(string.Empty, Entity.Category.Trim()+Entity.Service.Trim(), Entity.Status.Trim(), LookupDataAccess.Getdate(Entity.StatusDate.Trim()), Entity.Code, Entity.RefID);
                        }
                    }

                    if(RefDet_Grid.Rows.Count>0)
                    {
                        this.RefDet_Grid.AllowUserToAddRows = true;
                        RefDet_Grid.Rows[0].Selected = true;
                    }
                }

            }
            

        }

        private void RefDet_Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (RefDet_Grid.Rows.Count > 0)
            {

                //if(e.ColumnIndex==cmbCatg.Index)
                //{
                //    bool boolvalid = false;
                //    int introwindex = RefDet_Grid.CurrentCell.RowIndex;
                //    string FamSeq = string.Empty;

                //    if(AGYSERList.Count>0)
                //    {
                //        if (!string.IsNullOrEmpty(RefDet_Grid.Rows[introwindex].Cells["cmbCatg"].Value.ToString().Trim()))
                //        {
                //            string strCAValue = RefDet_Grid.Rows[introwindex].Cells["cmbCatg"].Value == null ? string.Empty : RefDet_Grid.Rows[introwindex].Cells["cmbCatg"].Value.ToString();
                //            //FillServicescombo(strCAValue);
                //        }

                //        //FillServicescombo()
                //    }

                //}

                //RefDet_Grid.CellValueChanged -= new DataGridViewCellEventHandler(RefDet_Grid_CellValueChanged);
                if (e.ColumnIndex == RefStat_Date.Index)
                {
                   
                    int introwindex = RefDet_Grid.CurrentCell.RowIndex;
                    if (!string.IsNullOrEmpty(RefDet_Grid.Rows[introwindex].Cells["RefStat_Date"].Value.ToString().Trim()))
                    {


                        string strIntervalValue = Convert.ToString(RefDet_Grid.Rows[introwindex].Cells["RefStat_Date"].Value);
                        int intcolumnindex = RefDet_Grid.CurrentCell.ColumnIndex;
                        string strCurrectValue = Convert.ToString(RefDet_Grid.Rows[introwindex].Cells[intcolumnindex].Value);
                        strCurrectValue = strCurrectValue.Replace("_", "").Trim();
                        strCurrectValue = strCurrectValue.Replace(" ", "").Trim();
                        RefDet_Grid.Rows[introwindex].Cells["RefStat_Date"].Selected = true;

                        if ((!string.IsNullOrEmpty(strCurrectValue)) && strCurrectValue.Trim() != "/  /")
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                            {
                                try
                                {

                                    if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                                    {
                                        //AlertBox.Show("01/01/1800 below date not except", MessageBoxIcon.Warning);
                                        AlertBox.Show("Date below 01/01/1800 is not accepted", MessageBoxIcon.Warning);
                                        RefDet_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;

                                    }
                                    else if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime(calDate.Text))
                                    {
                                        //CommonFunctions.MessageBoxDisplay(calDate.Text.Trim() + " below date not except");
                                        AlertBox.Show("Date below " + calDate.Text.Trim() + " is not accepted", MessageBoxIcon.Warning);
                                        RefDet_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;

                                    }
                                    else
                                    {
                                        RefDet_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                        AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);

                                    }

                                }
                                catch (Exception)
                                {
                                    RefDet_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);

                                }


                            }
                            else
                            {
                                bool booldatevalid = true;

                                string IsLeap = "N";
                                if (DateTime.IsLeapYear(int.Parse(strCurrectValue.Substring(6, 4))))
                                    IsLeap = "Y";
                                

                                if ((strCurrectValue.ToString().Substring(0, 2) == "02") && ((IsLeap=="N" && strCurrectValue.ToString().Substring(3, 2) == "29") || strCurrectValue.ToString().Substring(3, 2) == "30" || strCurrectValue.ToString().Substring(3, 2) == "31"))
                                {
                                    RefDet_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);
                                    //booldatevalid = false;
                                }
                                else if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime(calDate.Text))
                                {
                                    AlertBox.Show("Date below " + calDate.Text.Trim() + " is not accepted");
                                    RefDet_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;

                                }

                            }
                        }
                    }

                }
            }
        }

        private void Pb_Ref_Agency_Click(object sender, EventArgs e)
        {
            ////string AgencyCode = string.Empty;
            ////if (!string.IsNullOrEmpty(txtAgencyName.Text.Trim())) AgencyCode = txtAgencyName.Text.Trim();

            ////AgencyReferral_SubForm Ref_Form = new AgencyReferral_SubForm("Detail", AgencyCode, string.Empty, string.Empty, "Add", "Partner");
            //////Ref_Form.FormClosed += new Form.FormClosedEventHandler(On_Referral_Select_Closed);
            ////Ref_Form.ShowDialog();
            //AgencyPartner_Search Vendor_Browse = new AgencyPartner_Search(BaseForm, Privileges);
            //Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Partner_Browse_Closed);
            //Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
            //Vendor_Browse.ShowDialog();
        }

        //private void On_Partner_Browse_Closed(object sender, FormClosedEventArgs e)
        //{
        //    AgencyPartner_Search form = sender as AgencyPartner_Search;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        string[] Agency_Details = new string[2];
        //        Agency_Details = form.Get_Selected_Vendor();

        //        txtCode.Text = Agency_Details[0].Trim();
        //        txtAgencyName.Text = Agency_Details[1].Trim();

        //        if (dtScreen.Rows.Count > 0 && !string.IsNullOrEmpty(txtAgencyName.Text.Trim()))
        //        {
        //            if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "F")
        //                btnAddRep.Visible = true;
        //        }

        //    }
        //}

        private void txtAgencyName_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtAgencyName.Text.Trim()))
            {
                if (!string.IsNullOrEmpty(txtAgencyName.Text.Trim()) && ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() != "0")
                {
                    if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "F")
                    {
                        FillRepCombo();
                        RefDet_Grid.Rows.Clear();
                        RefDet_Grid.AllowUserToAddRows = true;
                    }
                    else if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "T")
                    {
                        FillCategoriescombo();
                        RefDet_Grid.Rows.Clear();
                        RefDet_Grid.AllowUserToAddRows = true;
                    }
                    else
                        RefDet_Grid.AllowUserToAddRows = false;
                }
            }
        }

        string Sql_SP_Result_Message = string.Empty;
        private void RefDet_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Privileges.DelPriv.Equals("true"))
                {
                    if (this.gvDelete.Index == e.ColumnIndex && e.RowIndex != -1)
                    {
                        if (RefDet_Grid.Rows.Count > 1)
                        {
                            string RefID = RefDet_Grid.CurrentRow.Cells["gvRefID"].Value == null ? string.Empty : RefDet_Grid.CurrentRow.Cells["gvRefID"].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(RefID.Trim()))
                            {
                                PARTNEREFEntity SelPartner = new PARTNEREFEntity(true);
                                if (ActRefsList.Count > 0)
                                    SelPartner = ActRefsList.Find(u => u.RefID.Equals(RefID));
                                if (SelPartner != null)
                                {
                                    SelPartner.Rec_Type = "DS";
                                    _model.SPAdminData.UpdatePartnerRef(SelPartner, "Delete", out Sql_SP_Result_Message);

                                    int rowIndex = RefDet_Grid.CurrentRow.Cells["gvDelete"].RowIndex;
                                    RefDet_Grid.Rows.RemoveAt(rowIndex);

                                }

                            }
                            else
                            {
                                int rowIndex = RefDet_Grid.CurrentRow.Cells["gvDelete"].RowIndex;
                                RefDet_Grid.Rows.RemoveAt(rowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void btnAddRep_Click(object sender, EventArgs e)
        {
            if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "F")
            {
                AGCYREPEntity selRepEntity = new AGCYREPEntity();
                string RepAddCode = "1";
                if (AgyRepList.Count > 0)
                {
                    List<AGCYREPEntity> SelRepList = AgyRepList.FindAll(u => u.PartCode == txtCode.Text.Trim());

                    if (SelRepList.Count > 0)
                    {
                        RepAddCode = SelRepList.Max(u => u.RepCode);
                        RepAddCode = (int.Parse(RepAddCode) + 1).ToString();
                    }
                }

                AGCYPARTEntity SelPartner = propPartner_List.Find(u => u.Code.Equals(txtCode.Text.Trim()));

                List<CommonEntity> Address = new List<CommonEntity>();
                if(SelPartner!=null)
                    Address.Add(new CommonEntity(SelPartner.Street.Trim(), SelPartner.City.Trim(), SelPartner.State.Trim(), SelPartner.Zip, SelPartner.Phone, string.Empty, string.Empty));


                //AGCYREPSUB_Form AgencyPartnerForm_Add = new AGCYREPSUB_Form(BaseForm, "Add", "Rep", txtCode.Text, txtAgencyName.Text.Trim(), "", selRepEntity, RepAddCode, Privileges, Address);
                //AgencyPartnerForm_Add.FormClosed += new FormClosedEventHandler(Referal_AddForm_Closed);
                //AgencyPartnerForm_Add.StartPosition = FormStartPosition.CenterScreen;
                //AgencyPartnerForm_Add.ShowDialog();
            }
            else if (((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Trim() == "T")
            {
                AGCYSEREntity selSubEntity = new AGCYSEREntity();
                string SubAddCode = "1";
                if (AGYSERList.Count > 0)
                {
                    List<AGCYSEREntity> SelServices = AGYSERList.FindAll(u => u.PartCode.Equals(txtCode.Text.Trim()));

                    if (SelServices.Count > 0)
                    {
                        SubAddCode = SelServices.Max(u => u.SerCode);
                        SubAddCode = (int.Parse(SubAddCode) + 1).ToString();
                    }
                }

                //AGCYServices_Form AgencyPartnerForm_Add = new AGCYServices_Form(BaseForm, "Add", txtCode.Text, txtAgencyName.Text.Trim(), "", selSubEntity, SubAddCode, Privileges, AGYSERList);
                //AgencyPartnerForm_Add.FormClosed += new FormClosedEventHandler(SubLoc_AddForm_Closed);
                //AgencyPartnerForm_Add.StartPosition = FormStartPosition.CenterScreen;
                //AgencyPartnerForm_Add.ShowDialog();
            }
        }

        string Added_Edited_RefralCode = string.Empty;
        //private void Referal_AddForm_Closed(object sender, FormClosedEventArgs e)
        //{
        //    AGCYREPSUB_Form form = sender as AGCYREPSUB_Form;
        //    if (form.DialogResult == DialogResult.OK)
        //    {

        //        Get_Partner_Data();

        //        cmbAgencyName_SelectedIndexChanged(sender, e);

        //        if (Mode == "Edit")
        //        {
        //            this.RefDet_Grid.AllowUserToAddRows = false;
        //            fillRefferdGrid();
        //        }
        //        //if (txtCode.Text == "New")
        //        //{
        //        //    AgcyRepData = form.GetRepData();
        //        //    if (AgcyRepData.Rec_Type == "I")
        //        //        AddRepList.Add(AgcyRepData);
        //        //    else
        //        //    {
        //        //        if (AddRepList.Count > 0)
        //        //        {
        //        //            foreach (AGCYREPEntity Entity in AddRepList)
        //        //            {
        //        //                if (Entity.AddRepcode.Trim() == AgcyRepData.AddRepcode.Trim())
        //        //                { AddRepList.Remove(Entity); break; }
        //        //            }
        //        //            AddRepList.Add(AgcyRepData);
        //        //        }
        //        //    }
        //        //}
        //        ////string[] From_Results = new string[2];
        //        ////From_Results = form.GetSelected_Agency_Code();
        //        ////Added_Edited_RefralCode = From_Results[0];
        //        //Getdata();
        //        //FillGridRep();

        //        ////FormLoad(Added_Edited_RefralCode);
        //        ////btnSearch_Click(btnSearch, EventArgs.Empty);
        //    }
        //}

        //private void SubLoc_AddForm_Closed(object sender, FormClosedEventArgs e)
        //{
        //    AGCYServices_Form form = sender as AGCYServices_Form;
        //    if (form.DialogResult == DialogResult.OK)
        //    {

        //        Get_Partner_Data();

        //        cmbAgencyName_SelectedIndexChanged(sender, e);

        //        if (Mode == "Edit")
        //        {
        //            this.RefDet_Grid.AllowUserToAddRows = false;
        //            fillRefferdGrid();
        //        }

        //        //if (txtCode.Text == "New")
        //        //{
        //        //    AgcySubData = form.GetSubData();

        //        //    if (AgcySubData.Rec_Type == "I")
        //        //        AddSubList.Add(AgcySubData);
        //        //    else
        //        //    {
        //        //        if (AddSubList.Count > 0)
        //        //        {
        //        //            foreach (AGCYSEREntity Entity in AddSubList)
        //        //            {
        //        //                if (Entity.AddSubCode.Trim() == AgcySubData.AddSubCode.Trim())
        //        //                { AddSubList.Remove(Entity); break; }
        //        //            }
        //        //            AddSubList.Add(AgcySubData);

        //        //        }
        //        //    }
        //        //}
        //        ////string[] From_Results = new string[2];
        //        ////From_Results = form.GetSelected_Agency_Code();
        //        ////Added_Edited_RefralCode = From_Results[0];
        //        //Getdata();
        //        //FillGridSubLocations();

        //        //FormLoad(Added_Edited_RefralCode);
        //        //btnSearch_Click(btnSearch, EventArgs.Empty);
        //    }
        //}

        private void RefDet_Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}