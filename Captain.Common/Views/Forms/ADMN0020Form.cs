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
using CarlosAg.ExcelXmlWriter;
using System.IO;
using System.Security.Principal;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMN0020Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion


        public ADMN0020Form(BaseForm baseForm, string mode, string sp_code, PrivilegeEntity privileges) //(BaseForm baseForm, string mode, string hierarchy, string year, string sp_code, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            Mode = mode;
            //M_Hierarchy = hierarchy;
            //M_Year = year;
            SP_Code = sp_code;
            //LblHeader.Text = privileges.PrivilegeName;
            _model = new CaptainModel();


            Initialize_database_Exception_Handling_Parameters();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 0;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.Icon = null;
            propReportPath = _model.lookupDataAccess.GetReportPath();

            //if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            //{
            //    HierarchyGrid.Size = new Size(231, 85);
            //    lblServiceCatdesc.Visible = true;
            //}
            //else
            //{
            //HierarchyGrid.Size = new Size(231, 114);
            lblServiceCatdesc.Visible = false;
            //}


            GetAgencyDetails();

            Member_Activity = "N"; string ShortName = string.Empty;
            DataSet dsAgency = new DataSet();

            dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                Member_Activity = dsAgency.Tables[0].Rows[0]["ACR_MEM_ACTIVTY"].ToString().Trim();
                ShortName = dsAgency.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();
            }

            if (ShortName == "STDC" || ShortName == "PCS" || ShortName == "NCCAA" || ShortName == "FTW" || ShortName == "CSNT" || ShortName == "BVCOG") { Cb_Allow_Add_Branch.Visible = false; cbNoSPM.Visible = true; cbNoSPM.Location = new System.Drawing.Point(218, 3); }
            else { cbNoSPM.Visible = false; Cb_Allow_Add_Branch.Visible = true; }

            if (Member_Activity == "Y")
                filldropdowns();

            fillBenefitCombo();
            fillCountTypeCombo();
            txtcode.Text = SP_Code;
            switch (Mode)
            {
                case "Edit":
                    this.Text = privileges.PrivilegeName + " - Edit";
                    if (Member_Activity == "N")
                    {
                        this.panel1.Size = new System.Drawing.Size(860, 75);
                        // this.BranchPanel.Location = new System.Drawing.Point(1, 124);//1, 119
                        // this.SpDetailsPanel.Location = new System.Drawing.Point(1, 237);
                        // this.panel4.Location = new System.Drawing.Point(1, 440);//1,280
                        //this.Size = new System.Drawing.Size(860, 500);
                        this.Size = new System.Drawing.Size(/*972*/1020, 500);

                        lblCategory.Visible = false; cmbCategory.Visible = false; label2.Visible = false;
                        lblPartner.Visible = false; cmbPartner.Visible = false;
                    }
                    else
                    {
                        lblCategory.Visible = true; cmbCategory.Visible = true; label2.Visible = true;
                        lblPartner.Visible = true; cmbPartner.Visible = true;
                    }
                    Fill_SP_Branch_Hier();
                    BtnAddDetails.Visible = true;

                    //contextMenu1_Popup(contextMenu1, EventArgs.Empty);
                    break;
                case "Add":
                    SP_Code = "0";
                    this.Text = privileges.PrivilegeName + " - Add";
                    SpDetailsPanel.Visible = false;
                    if (Member_Activity == "N")
                    {
                        this.panel1.Size = new System.Drawing.Size(860, 75);//666, 64
                                                                            // this.BranchPanel.Location = new System.Drawing.Point(1, 124);//1, 119
                                                                            // this.SpDetailsPanel.Location = new System.Drawing.Point(1, 237); //1, 232

                        //this.BranchPanel.Size = new System.Drawing.Size(606, 160);
                        // this.BranchGrid.Size = new System.Drawing.Size(336, 154);
                        // this.HierarchyGrid.Size = new System.Drawing.Size(222, 154);
                        // this.panel4.Location = new System.Drawing.Point(1, 285);//1,280
                        //this.Size = new System.Drawing.Size(860,290);//(670, 315)
                        this.Size = new System.Drawing.Size(972, 290);
                    }
                    else if (Member_Activity == "Y")
                    {
                        lblCategory.Visible = true; cmbCategory.Visible = true; label2.Visible = true;
                        lblPartner.Visible = true; cmbPartner.Visible = true;
                        //this.panel1.Size = new System.Drawing.Size(860, 113);
                        //this.BranchPanel.Size = new System.Drawing.Size(606, 160);
                        //this.BranchGrid.Size = new System.Drawing.Size(336, 154);
                        this.HierarchyGrid.Size = new System.Drawing.Size(320, 154);
                        //this.panel4.Location = new System.Drawing.Point(1, 311);
                        //this.Size = new System.Drawing.Size(670, 346);//(610, 315)
                        this.Size = new System.Drawing.Size(860, 340);

                        chkbBulk.Visible = false;
                    }
                    PbDelete.Visible = false;
                    Cb_Active.Checked = true;
                    Add_Branches();
                    break;
                case "Delete":
                    this.Text = privileges.Program + " - Delete";
                    break;
            }


            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(PbDelete, "Delete Selected Service/Outcome");
            tooltip.SetToolTip(PbHierarchies, "Select Service Hierarchies");
            // tooltip.SetToolTip(Hepl, "Screen Help");
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string SP_Code { get; set; }
        public string propReportPath { get; set; }

        //public string M_Hierarchy { get; set; }

        ////public string M_HieDesc { get; set; }

        //public string M_Year { get; set; }

        public string SchSite { get; set; }

        public string SchDate { get; set; }

        public string SchType { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public DataSet dsAgency { get; set; }

        public DataTable dtAgency { get; set; }

        public string Member_Activity { get; set; }


        public DataGridViewContentAlignment Alignment { get; set; }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }

        #endregion

        private void filldropdowns()
        {
            List<ListItem> listType = new List<ListItem>();
            cmbCategory.Items.Clear();
            listType = new List<ListItem>();
            listType.Add(new ListItem("Select One", "0"));
            listType.Add(new ListItem("Category 1", "1"));
            listType.Add(new ListItem("Category 2", "2"));
            listType.Add(new ListItem("Category 3", "3"));
            listType.Add(new ListItem("Category 4", "4"));
            listType.Add(new ListItem("Category 5", "5"));

            cmbCategory.Items.AddRange(listType.ToArray());
            cmbCategory.SelectedIndex = 0;

            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0087", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);

            cmbPartner.Items.Insert(0, new ListItem("", "0"));
            cmbPartner.SelectedIndex = 0;
            foreach (CommonEntity organization in commonEntity)
            {
                ListItem li = new ListItem(organization.Desc, organization.Code);
                cmbPartner.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && organization.Default.Equals("Y")) cmbPartner.SelectedItem = li;
            }
        }

        private void fillBenefitCombo()
        {
            List<CommonEntity> AgyTabs_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0038", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            if (AgyTabs_List.Count > 0)
            {
                DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.SPDetailsGrid.Columns["OBF"];
                AgyTabs_List = AgyTabs_List.OrderBy(u => Convert.ToInt32(u.Code.Trim())).ToList();

                //BedEntity = AgyTabs_List;

                cb.DataSource = AgyTabs_List;
                cb.DisplayMember = "Desc";
                cb.ValueMember = "Code";
                cb = (DataGridViewComboBoxColumn)this.SPDetailsGrid.Columns["OBF"];
            }
        }

        private void fillCountTypeCombo()
        {
            //List<ListItem> listType = new List<ListItem>();

            //listType = new List<ListItem>();
            //listType.Add(new ListItem("Average", "A"));
            //listType.Add(new ListItem("Sum", "S"));
            //listType.Add(new ListItem("End of Month", "E"));
            ////listType.Add(new ListItem("Category 3", "3"));
            ////listType.Add(new ListItem("Category 4", "4"));
            ////listType.Add(new ListItem("Category 5", "5"));


            List<CommonEntity> AgyTabs_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0102", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            if (AgyTabs_List.Count > 0)
            {
                DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.SPDetailsGrid.Columns["gvCountType"];
                //AgyTabs_List = AgyTabs_List.OrderBy(u => Convert.ToInt32(u.Code.Trim())).ToList();

                //BedEntity = AgyTabs_List;

                cb.DataSource = AgyTabs_List;
                cb.DisplayMember = "Desc";
                cb.ValueMember = "Code";
                cb = (DataGridViewComboBoxColumn)this.SPDetailsGrid.Columns["gvCountType"];
            }
        }

        private void GetAgencyDetails()
        {
            dsAgency = DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(BaseForm.BaseAgency, null, null, null, null, null, null);
            dtAgency = dsAgency.Tables[0];
        }

        string SPOut_Err_Procedure, SPOut_Err_Number, SPOut_Err_Message, SPOut_Err_Line;

        int Sel_Del_Count = 0;
        bool Hie_Changed_FLG = false;
        string UpdateHierarchiesToTable = null;
        private void Hepl_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN0020_Form");

        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            ////HierarchieSelectionForm addForm = new HierarchieSelectionForm(BaseForm, SelectedHierarchies, "Service", null, "Add");
            ////addForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            ////addForm.ShowDialog();

            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Service", "I", "*", "R", Privileges);
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, SelectedHierarchies, "Service", "I", "*", "I", Privileges);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();

        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;
            //TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedItem.Tag as TagClass;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                int Rows_Cnt = 0;
                // HierarchyGrid.Rows.Clear();
                Hie_Changed_FLG = true;
                HierarchyGrid.Rows.Clear();
                if (selectedHierarchies.Count > 0)
                {
                    HierarchyGrid.Rows.Clear();
                    string Agy = "**", Dept = "**", Prog = "**";
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        Agy = Dept = Prog = "**";
                        if (!string.IsNullOrEmpty(row.Agency.Trim()))
                            Agy = row.Agency.Trim();

                        if (!string.IsNullOrEmpty(row.Prog.Trim()))
                            Prog = row.Prog.Trim();

                        if (!string.IsNullOrEmpty(row.Dept.Trim()))
                            Dept = row.Dept.Trim();

                        int rowIndex = HierarchyGrid.Rows.Add(row.Code + "  " + row.HirarchyName.ToString(), Agy + Dept + Prog);
                        HierarchyGrid.Rows[rowIndex].Tag = row;
                        Rows_Cnt++;

                        //hierarchy += row.Agency + row.Dept + row.Prog;
                        hierarchy += row.Code.Substring(0, 2) + row.Code.Substring(3, 2) + row.Code.Substring(6, 2);
                    }

                    if (HierarchyGrid.RowCount > 0)
                    {
                        _errorProvider.SetError(HierarchyGrid, null);

                    }
                    if (HierarchyGrid.Rows.Count > 0)
                    {
                        HierarchyGrid.CurrentCell = HierarchyGrid.Rows[0].Cells[0];
                        HierarchyGrid.Rows[0].Selected = true;
                    }


                    set_Hie_Grid_Size(Rows_Cnt);
                }
            }
        }

        private void set_Hie_Grid_Size(int Rows_Cnt)
        {
            if (Mode == "Add")
            {
                if (Rows_Cnt > 6)
                    this.Hier.Width = 303;
                else
                    this.Hier.Width = 316;
            }
            else
            {
                if (Rows_Cnt > 4)
                    this.Hier.Width = 303;
                else
                    this.Hier.Width = 316;
            }
        }



        private void BtnAddDetails_Click(object sender, EventArgs e)
        {
            string Branch_Code = null;

            if (BranchGrid.CurrentRow.Cells["Dup_Def"].EditedFormattedValue.ToString() != " ")
                Branch_Code = BranchGrid.CurrentRow.Cells["Branch_Code"].EditedFormattedValue.ToString();

            ADMN0020_CAMSForm ADMN0020_CAMS_Form = new ADMN0020_CAMSForm(BaseForm, Mode, SP_Code, Privileges, this, Branch_Code);//(BaseForm, Mode, M_Hierarchy, M_Year, SP_Code, Privileges);
            ADMN0020_CAMS_Form.FormClosed += new FormClosedEventHandler(Add_Edit_CAMS_Closed);
            ADMN0020_CAMS_Form.StartPosition = FormStartPosition.CenterScreen;
            ADMN0020_CAMS_Form.ShowDialog();
        }

        private void Add_Edit_CAMS_Closed(object sender, FormClosedEventArgs e)
        {
            ADMN0020_CAMSForm form = sender as ADMN0020_CAMSForm;
            if (form.DialogResult != DialogResult.OK)
            {
                Fill_Branch_CAMS_Details_Grid();
                if (form.Get_SP_Existance() == "SP Not Exists")
                    AlertBox.Show("Service Plan '" + SP_Code + "' is already deleted by another user!!!", MessageBoxIcon.Warning);
            }
        }



        //   Begining of Form Validation 

        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(Txtdesc.Text.Trim()))
            {
                //_errorProvider.SetIconAlignment(this.Txtdesc, ErrorIconAlignment.MiddleRight);
                _errorProvider.SetError(Txtdesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Service Plan Description".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Txtdesc, null);

            if (Mode.Equals("Add") && (BranchGrid.Rows[0].Cells["Row_status"].Value.ToString() != "A") ||
               (Mode.Equals("Edit") && BranchGrid.Rows[0].Cells["Row_status"].Value.ToString() == "D"))
            {
                _errorProvider.SetError(BranchGrid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Primary Branch".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(BranchGrid, null);

            if (HierarchyGrid.RowCount < 1)
            {
                _errorProvider.SetError(HierarchyGrid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Atleast One Hierarchy ".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(HierarchyGrid, null);

            return (isValid);
        }

        //   End of Form Validation 


        CASESP0Entity SP_Header_Rec;
        private void Fill_SP_Branch_Hier()
        {
            SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SP_Code, null, null, null, null, null, null, null, null);

            //Commented by Sudheer on 10/02/2018
            if (SP_Header_Rec.Validate == "N")
                BtnValidate.Visible = true;

            Txtdesc.Text = SP_Header_Rec.Desc.Trim();
            //this.SpDesc.HeaderText = SP_Header_Rec.Desc.Trim();

            if (Member_Activity == "Y")
            {
                CommonFunctions.SetComboBoxValue(cmbCategory, SP_Header_Rec.Category.Trim());
                CommonFunctions.SetComboBoxValue(cmbPartner, SP_Header_Rec.PartnerPostingType.Trim());
            }

            if (SP_Header_Rec.Active == "Y")
                Cb_Active.Checked = true;

            if (SP_Header_Rec.Allow_Add_Branch == "Y")
                Cb_Allow_Add_Branch.Checked = true;

            if (SP_Header_Rec.Allow_Duplicates == "Y")
                Cb_Allow_Dulpicates.Checked = true;

            if (SP_Header_Rec.Auto_Post_SP == "Y")
                Cb_Auto_Post.Checked = true;

            if (SP_Header_Rec.Sp0ReadOnly == "Y")
                chkReadOnly.Checked = true;
            else
                chkReadOnly.Checked = false;

            if (SP_Header_Rec.Sp0Notes == "Y")
                chkCaseNotes.Checked = true;
            else
                chkCaseNotes.Checked = false;

            if (SP_Header_Rec.NoSPM == "Y")
                cbNoSPM.Checked = true;

            if (SP_Header_Rec.BulkEntry == "Y")
                chkbBulk.Checked = true;

            Fill_SP_Hierarchies_Grid();
            Fill_SP_CAMS_Details_Grid();
            Fill_Branch_Grid();



        }

        private void Fill_Branch_Grid()
        {
            this.BranchGrid.SelectionChanged -= new System.EventHandler(this.BranchGrid_SelectionChanged);
            // "E"  Bracch Exists
            // "N"  Bracch Not Exists
            // "Y"  Bracch Changed
            // "A"  Bracch Added
            // "D"  Bracch Deleted
            BranchGrid.Rows.Clear();

            if (!string.IsNullOrEmpty(SP_Header_Rec.BpCode.Trim()))
                BranchGrid.Rows.Add("Branch Primary *", SP_Header_Rec.BpDesc.Trim(), SP_Header_Rec.BpDesc.Trim(), "E", CAMS_Exists_For_Branch(SP_Header_Rec.BpCode), "P");
            else
                BranchGrid.Rows.Add("Branch Primary *", " ", " ", "N", "N", "P");
            if (!string.IsNullOrEmpty(SP_Header_Rec.B1Code.Trim()))
                BranchGrid.Rows.Add("Branch 1", SP_Header_Rec.B1Desc.Trim(), SP_Header_Rec.B1Desc.Trim(), "E", CAMS_Exists_For_Branch(SP_Header_Rec.B1Code), "1");
            else
                BranchGrid.Rows.Add("Branch 1", " ", " ", "N", "N", "1");
            if (!string.IsNullOrEmpty(SP_Header_Rec.B2Code.Trim()))
                BranchGrid.Rows.Add("Branch 2", SP_Header_Rec.B2Desc.Trim(), SP_Header_Rec.B2Desc.Trim(), "E", CAMS_Exists_For_Branch(SP_Header_Rec.B2Code), "2");
            else
                BranchGrid.Rows.Add("Branch 2", " ", " ", "N", "N", "2");
            if (!string.IsNullOrEmpty(SP_Header_Rec.B3Code.Trim()))
                BranchGrid.Rows.Add("Branch 3", SP_Header_Rec.B3Desc.Trim(), SP_Header_Rec.B3Desc.Trim(), "E", CAMS_Exists_For_Branch(SP_Header_Rec.B3Code), "3");
            else
                BranchGrid.Rows.Add("Branch 3", " ", " ", "N", "N", "3");
            if (!string.IsNullOrEmpty(SP_Header_Rec.B4Code.Trim()))
                BranchGrid.Rows.Add("Branch 4", SP_Header_Rec.B4Desc.Trim(), SP_Header_Rec.B4Desc.Trim(), "E", CAMS_Exists_For_Branch(SP_Header_Rec.B4Code), "4");
            else
                BranchGrid.Rows.Add("Branch 4", " ", " ", "N", "N", "4");
            if (!string.IsNullOrEmpty(SP_Header_Rec.B5Code.Trim()))
                BranchGrid.Rows.Add("Branch 5", SP_Header_Rec.B5Desc.Trim(), SP_Header_Rec.B5Desc.Trim(), "E", CAMS_Exists_For_Branch(SP_Header_Rec.B5Code), "5");
            else
                BranchGrid.Rows.Add("Branch 5", " ", " ", "N", "N", "5");

            if (BranchGrid.Rows.Count > 0)
            {
                BranchGrid.CurrentCell = BranchGrid.Rows[0].Cells[1];
                BranchGrid.Rows[0].Selected = true;
                //BranchGrid_CellEndEdit()
            }
            this.BranchGrid.SelectionChanged += new System.EventHandler(this.BranchGrid_SelectionChanged);
        }

        private string CAMS_Exists_For_Branch(string Branch_Code)
        {
            string ret_Value = "N";

            foreach (CASESP2Entity Entity in CAMA_Details)
            {
                if (Entity.Branch == Branch_Code)
                {
                    ret_Value = "Y"; break;
                }
            }

            return ret_Value;
        }

        List<CASESP1Entity> SP_Hierarchies;
        private void Fill_SP_Hierarchies_Grid()
        {
            SP_Hierarchies = new List<CASESP1Entity>();
            SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(SP_Code, null, null, null);
            if (SP_Hierarchies.Count > 0)
            {
                string Hiename = null;
                HierarchyGrid.Rows.Clear();


                int Rows_Cnt = 0;
                ///// Murali added Category service description on 05/21/2021
                //if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                //{
                //    if ((SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog).Contains("**"))
                //    {
                //        lblServiceCatdesc.Text = "";
                //    }
                //    else
                //    {
                //        ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(SP_Hierarchies[0].Agency, SP_Hierarchies[0].Dept, SP_Hierarchies[0].Prog);
                //        if (programEntity != null)
                //        {
                //            List<CommonEntity> PAYMENTService = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00201", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
                //            if (PAYMENTService.Count > 0)
                //            {
                //                CommonEntity comservice = PAYMENTService.Find(u => u.Code == programEntity.DepSerpostPAYCAT);
                //                if (comservice != null)
                //                    lblServiceCatdesc.Text = programEntity.DepSerpostPAYCAT + " - " + comservice.Desc;
                //            }

                //        }
                //    }

                //}
                foreach (CASESP1Entity Entity in SP_Hierarchies)
                {
                    int rowIndex = 0;
                    HierarchyEntity hierchyEntity = null;

                    if (Entity.Agency + Entity.Dept + Entity.Prog == "******")
                    {
                        Hiename = "All Hierarchies";
                        hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                    }
                    else
                        Hiename = "Not Defined in CASEHIE";

                    DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Entity.Agency, Entity.Dept, Entity.Prog);
                    if (ds_Hie_Name.Tables.Count > 0)
                    {
                        if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                            //hierchyEntity = new HierarchyEntity(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Hiename);

                            if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                            if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";

                            hierchyEntity = new HierarchyEntity(ds_Hie_Name.Tables[0].Rows[0], "CASEHIE");
                        }
                    }

                    rowIndex = HierarchyGrid.Rows.Add(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog + "  " + Hiename, Entity.Agency + Entity.Dept + Entity.Prog);
                    HierarchyGrid.Rows[rowIndex].Tag = hierchyEntity;
                    Rows_Cnt++;

                    if (SP_Header_Rec.Default_Prog == Entity.Agency + Entity.Dept + Entity.Prog)
                        HierarchyGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                }
                if (HierarchyGrid.Rows.Count > 0)
                {
                    HierarchyGrid.CurrentCell = HierarchyGrid.Rows[0].Cells[0];
                    HierarchyGrid.Rows[0].Selected = true;
                }
                set_Hie_Grid_Size(Rows_Cnt);
            }
        }

        List<CASESP2Entity> CAMA_Details = new List<CASESP2Entity>();
        private void Fill_SP_CAMS_Details_Grid()
        {
            CAMA_Details.Clear();
            CAMA_Details = _model.SPAdminData.Browse_CASESP2(SP_Code, null, null, null);

            //Fill_Branch_CAMS_Details_Grid();
        }


        private void Fill_Branch_CAMS_Details_Grid()
        {
            Btn_Validate_Branch.Visible = PbDelete.Visible = Del_Col_Header_Clicked = false; Sel_Del_Count = 0;
            BtnValidate.Enabled = BtnSaveSP.Enabled = true;
            Branch_CAMS_Delete_List.Clear();

            if (!string.IsNullOrEmpty(BranchGrid.CurrentRow.Cells["BranchDef"].EditedFormattedValue.ToString().Trim()))
                this.SpDesc.HeaderText = BranchGrid.CurrentRow.Cells["Dup_Def"].Value.ToString().Trim();
            else
                this.SpDesc.HeaderText = "Branch Not Defined";

            if (chkbBulk.Checked)
            {
                SPDetailsGrid.Columns["gvCountType"].Visible = true;
                SPDetailsGrid.Columns["gvCountType"].ShowInVisibilityMenu = true;
                SPDetailsGrid.Columns["SpDesc"].Width = 370;
            }
            else
            {
                SPDetailsGrid.Columns["gvCountType"].Visible = false;
                SPDetailsGrid.Columns["gvCountType"].ShowInVisibilityMenu = false;
                SPDetailsGrid.Columns["SpDesc"].Width = 546;//490;
            }

            //else
            //{

            //    if (string.IsNullOrEmpty(BranchGrid.CurrentRow.Cells["Dup_Def"].EditedFormattedValue.ToString()))

            //    this.SpDesc.HeaderText = " ";
            //}

            this.SPDetailsGrid.SelectionChanged -= new System.EventHandler(this.SPDetailsGrid_SelectionChanged);

            SPDetailsGrid.Rows.Clear();

            if (CAMA_Details.Count > 0)
            {
                int rowIndex = 0;
                contextMenu1.MenuItems.Clear();

                int i = 0;
                foreach (MenuItem m in contextMenu1.MenuItems)
                {
                    contextMenu1.MenuItems.RemoveAt(i);
                }


                SPDetailsGrid.Rows.Clear();
                pictureBox2.Visible = false;

                Branch_CAMS_Details.Clear();
                string Autopost_Disp = "";
                CASESP2Entity Tmp_Entity = new CASESP2Entity();
                foreach (CASESP2Entity Entity in CAMA_Details)
                {
                    rowIndex = 0;

                    if (Entity.Branch == BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString())
                    {
                        Tmp_Entity.ServPlan = Entity.ServPlan;
                        Tmp_Entity.Branch = Entity.Branch;
                        Tmp_Entity.Orig_Grp = Entity.Orig_Grp;
                        Tmp_Entity.Type1 = Entity.Type1;
                        Tmp_Entity.CamCd = Entity.CamCd;
                        Tmp_Entity.Curr_Grp = Entity.Curr_Grp;
                        Tmp_Entity.Row = Entity.Row;
                        Tmp_Entity.DateLstc = Entity.DateLstc;
                        Tmp_Entity.lstcOperator = Entity.lstcOperator;
                        Tmp_Entity.Dateadd = Entity.Dateadd;
                        Tmp_Entity.addoperator = Entity.addoperator;
                        Tmp_Entity.Rec_Type = Entity.Rec_Type;
                        Tmp_Entity.CAMS_Desc = Entity.CAMS_Desc;
                        Tmp_Entity.Shift_Count = Entity.Shift_Count;
                        Tmp_Entity.CAMS_Post_Count = Entity.CAMS_Post_Count;
                        Tmp_Entity.SP2_CAMS_Active = Entity.SP2_CAMS_Active;
                        Tmp_Entity.CAMS_Active = Entity.CAMS_Active;
                        Tmp_Entity.Existing_CAMS = Entity.Existing_CAMS;
                        Tmp_Entity.SP2_Auto_Post = Entity.SP2_Auto_Post;
                        Tmp_Entity.SP2_OBF = Entity.SP2_OBF;
                        Tmp_Entity.SP2_COUNT_TYPE = Entity.SP2_COUNT_TYPE;

                        Branch_CAMS_Details.Add(new CASESP2Entity(Tmp_Entity));
                        Autopost_Disp = (Entity.SP2_Auto_Post == "Y" ? "A" : "");

                        string TypeChange = string.Empty;
                        if (Entity.Type1 == "CA") TypeChange = "Ser"; else if (Entity.Type1 == "MS") TypeChange = "Out";

                        rowIndex = SPDetailsGrid.Rows.Add(false, Entity.CAMS_Desc, Entity.CamCd, TypeChange, Autopost_Disp, Entity.Curr_Grp, Entity.Row, Entity.Type1, Entity.Orig_Grp, Entity.CAMS_Post_Count, Entity.CAMS_Post_Count, (Entity.CAMS_Active == "False" ? "N" : Entity.SP2_CAMS_Active), Entity.Existing_CAMS, Entity.SP2_Auto_Post, Entity.SP2_OBF, Entity.SP2_COUNT_TYPE);

                        if (Entity.CAMS_Active == "False" || Entity.SP2_CAMS_Active != "A") //(Entity.SP2_CAMS_Active == "I" || string.IsNullOrEmpty(Entity.SP2_CAMS_Active.Trim())))
                            SPDetailsGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;  // Color.Red;
                    }
                }
                if (SPDetailsGrid.RowCount > 0)
                {
                    this.SpDesc.HeaderText = BranchGrid.CurrentRow.Cells["BranchDef"].Value.ToString().Trim();
                    SPDetailsGrid.Rows[0].Tag = 0;
                    Prepare_Menu_items();
                }
                else
                {
                    //this.SpDesc.HeaderText = " ";
                    //MessageBox.Show("No CA/MS are Defined for this Branch", "CAP Systems");
                }
                if (SPDetailsGrid.Rows.Count > 0)
                    SPDetailsGrid.Rows[0].Selected = true;


                if (SPDetailsGrid.Rows.Count >= 9)
                    pictureBox2.Visible = true;

                this.SPDetailsGrid.SelectionChanged += new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
            }

        }

        private void SPDetailsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SPDetailsGrid.Rows.Count > 0)// && !Btn_Validate_Branch.Visible)
            {
                int ColIdx = 0;
                int RowIdx = 0;
                ColIdx = SPDetailsGrid.CurrentCell.ColumnIndex;
                RowIdx = SPDetailsGrid.CurrentCell.RowIndex;

                //if (!Del_Col_Header_Clicked)
                //{
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    bool Can_Delete = true;
                    //if (SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString() == "CA")
                    //{
                    if (int.Parse(SPDetailsGrid.CurrentRow.Cells["CA_Post_Count"].Value.ToString()) > 0)
                        Can_Delete = false;
                    //}
                    //else
                    //{
                    //    if (int.Parse(SPDetailsGrid.CurrentRow.Cells["MS_Post_Count"].Value.ToString()) > 0)
                    //        Can_Delete = false;
                    //}


                    if (Can_Delete)
                    {
                        string Tmp = "false";
                        switch (e.ColumnIndex)
                        {
                            case 0:
                                Tmp = SPDetailsGrid.CurrentRow.Cells["Del"].Value.ToString();
                                if (Tmp == "True")
                                {
                                    SPDetailsGrid.CurrentRow.Cells["Del"].Value = true;
                                    Sel_Del_Count++;
                                }
                                else
                                {
                                    SPDetailsGrid.CurrentRow.Cells["Del"].Value = false;
                                    if (Sel_Del_Count > 0)
                                        Sel_Del_Count--;
                                }
                                break;
                        }
                    }
                    else
                    {
                        SPDetailsGrid.CurrentRow.Cells["Del"].Value = false;

                        if (SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString() == "CA")
                            AlertBox.Show("Selected Service having postings in CASEACT", MessageBoxIcon.Warning);
                        else
                            AlertBox.Show("Selected Outcome having postings in CASEMS", MessageBoxIcon.Warning);
                    }

                    if (Sel_Del_Count > 0 && Privileges.DelPriv.Equals("true"))// && !Row_Shifted)
                        PbDelete.Visible = true;
                    else
                        PbDelete.Visible = false;

                    //Del_Col_Header_Clicked = false;
                }

                // }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (Sel_Del_Count < 1)
                AlertBox.Show("Please Select atleast one Service/Outcome to Delete", MessageBoxIcon.Warning);
            else if (Btn_Validate_Branch.Visible)
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "  selected Service/Outcome(s)", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Selected_CAMS_1);
            else
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "  selected Service/Outcome(s)", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Selected_CAMS);
        }

        private void Delete_Selected_CAMS(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                bool Delete_Statue = true;
                string Branch_Code = BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString(), CAMS_Type = null, CAMS_Code = null;
                int Row_Seq = 0, Org_Group = 0;
                foreach (DataGridViewRow dr in SPDetailsGrid.Rows)
                {
                    if (dr.Cells["Del"].Value.ToString() == true.ToString() && dr.Cells["Existing_CA_MS_SW"].Value.ToString() == "Y")
                    {
                        Row_Seq = int.Parse(dr.Cells["Sequence"].Value.ToString());
                        Org_Group = int.Parse(dr.Cells["CAMA_Org_Grp"].Value.ToString());
                        CAMS_Code = dr.Cells["CAMS_Code"].Value.ToString();
                        CAMS_Type = dr.Cells["Type"].Value.ToString();
                        //if (CAMS_Type == "Ser") CAMS_Type = "CA"; else if (CAMS_Type == "Out") CAMS_Type = "MS";


                        if (!(DatabaseLayer.SPAdminDB.Delete_CASESP2(int.Parse(SP_Code), Branch_Code, Org_Group, CAMS_Type, CAMS_Code)))
                            Delete_Statue = false;
                    }
                }

                if (Delete_Statue)
                {
                    AlertBox.Show("Selected Service/Outcome(s) Deleted Successfully");
                    //MessageBox.Show("Selected CA/MS(s) Deleted Successfully", "CAP Systems"); // Lisa Asked to Comment on 01072013
                    if (Sel_Del_Count < SPDetailsGrid.Rows.Count)
                        Rerrange_Seq_Groups();
                    //Sel_Del_Count = 0;
                    Sel_Del_Count = 0;
                    Del_Col_Header_Clicked = false;
                    Fill_SP_Branch_Hier();

                    Refresh_ADMN20_Control = true;

                    ////Fill_SP_CAMS_Details_Grid();
                    ////Fill_Branch_CAMS_Details_Grid();
                    ////Fill_Branch_Grid();
                }
                PbDelete.Visible = false;
            }
            //}
        }

        List<CASESP2Entity> Branch_CAMS_Delete_List = new List<CASESP2Entity>();
        private void Delete_Selected_CAMS_1(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                bool Delete_Statue = true;
                string Branch_Code = BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString(), CAMS_Type = null, CAMS_Code = null;
                int Row_Seq = 0, Org_Group = 0;

                List<CASESP2Entity> TMP_Branch_CAMS_Details = new List<CASESP2Entity>();
                TMP_Branch_CAMS_Details = Branch_CAMS_Details;
                foreach (DataGridViewRow dr in SPDetailsGrid.Rows)
                {
                    if (dr.Cells["Del"].Value.ToString() == true.ToString())
                    {
                        Row_Seq = int.Parse(dr.Cells["Sequence"].Value.ToString());
                        Org_Group = int.Parse(dr.Cells["CAMA_Org_Grp"].Value.ToString());
                        CAMS_Code = dr.Cells["CAMS_Code"].Value.ToString();
                        CAMS_Type = dr.Cells["Type"].Value.ToString();
                        //if (CAMS_Type == "Ser") CAMS_Type = "CA"; else if (CAMS_Type == "Out") CAMS_Type = "MS";
                        //if (dr.Cells["Existing_CA_MS_SW"].Value.ToString() == "Y")
                        //{
                        //    if (!(DatabaseLayer.SPAdminDB.Delete_CASESP2(int.Parse(SP_Code), Branch_Code, Org_Group, CAMS_Type, CAMS_Code)))
                        //        Delete_Statue = false;
                        //}

                        foreach (CASESP2Entity Entity in TMP_Branch_CAMS_Details)
                        {
                            if (CAMS_Code.Trim() == Entity.CamCd.Trim() && CAMS_Type == Entity.Type1.Trim() && Org_Group == Entity.Orig_Grp && Row_Seq == Entity.Row)
                            {
                                Branch_CAMS_Delete_List.Add(new CASESP2Entity(Entity));
                                Branch_CAMS_Details.Remove(Entity); break;
                            }
                        }
                    }
                }

                if (Delete_Statue)
                    Fill_Branch_Grid_With_New_CAMSDetails_After_Delete();
                AlertBox.Show("Selected Service/Outcome(s) Deleted Successfully");
                PbDelete.Visible = false;
                Sel_Del_Count = 0;
            }
            //}
        }



        private void Add_Branches()
        {
            this.BranchGrid.SelectionChanged -= new System.EventHandler(this.BranchGrid_SelectionChanged);
            BranchGrid.Rows.Add("Branch Primary *", " ", " ", "N", "N", "P");
            BranchGrid.Rows.Add("Branch 1", " ", " ", "N", "N", "1");
            BranchGrid.Rows.Add("Branch 2", " ", " ", "N", "N", "2");
            BranchGrid.Rows.Add("Branch 3", " ", " ", "N", "N", "3");
            BranchGrid.Rows.Add("Branch 4", " ", " ", "N", "N", "4");
            BranchGrid.Rows.Add("Branch 5", " ", " ", "N", "N", "5");
            if (BranchGrid.Rows.Count > 0)
            {
                BranchGrid.CurrentCell = BranchGrid.Rows[0].Cells[1];
                BranchGrid.Rows[0].Selected = true;
            }
        }


        List<PopUp_Menu_L1_Entity> listItem_L1_New = new List<PopUp_Menu_L1_Entity>();
        List<PopUp_Menu_L2_Entity> listItem_L2_New = new List<PopUp_Menu_L2_Entity>();
        List<PopUp_Menu_L3_Entity> listItem_L3_New = new List<PopUp_Menu_L3_Entity>();

        List<PopUp_Menu_L1_Entity> listItem_L1_Menu = new List<PopUp_Menu_L1_Entity>();
        List<PopUp_Menu_L2_Entity> listItem_L2_Menu = new List<PopUp_Menu_L2_Entity>();
        List<PopUp_Menu_L3_Entity> listItem_L3_Menu = new List<PopUp_Menu_L3_Entity>();

        private void Prepare_Menu_items()
        {
            bool Group_Exists = false;
            listItem_L1_New.Clear();
            listItem_L2_New.Clear();
            listItem_L3_New.Clear();

            if (CAMA_Details.Count > 0)
            {
                foreach (CASESP2Entity Entity in CAMA_Details)
                {
                    if (BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString() == Entity.Branch)
                    {

                        Group_Exists = false;

                        foreach (PopUp_Menu_L2_Entity Ent_2 in listItem_L2_New)
                        {
                            if (Ent_2.Grp_Code.Equals(Entity.Curr_Grp))
                                Group_Exists = true;
                        }

                        if (!Group_Exists)
                            listItem_L2_New.Add(new PopUp_Menu_L2_Entity(" ", Entity.Curr_Grp));    // Rao

                        listItem_L3_New.Add(new PopUp_Menu_L3_Entity(" ", Entity, 0));    // Rao
                    }
                }
            }

            listItem_L1_New.Add(new PopUp_Menu_L1_Entity("M", "Select to Move"));
            //if (listItem_L2_New.Count > 1)
            listItem_L1_New.Add(new PopUp_Menu_L1_Entity("C", "Select to Copy"));
        }


        int Priv_Group = int.MaxValue, Curr_Group;
        bool Move_Copy_Rec_Sel = false;
        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            listItem_L1_Menu.Clear();
            //if (Priv_Group != Curr_Group)
            bool Can_paste_CAMS = true;
            if (SPDetailsGrid.Rows.Count > 0)
            {
                if (!Move_Copy_Rec_Sel)
                {

                    foreach (PopUp_Menu_L1_Entity Ent_1 in listItem_L1_New)
                    {
                        MenuItem Menu_L1 = new MenuItem();
                        Menu_L1.Text = Ent_1.Cat_Desc;
                        Menu_L1.Tag = Ent_1.Cat_Code;
                        //contextMenu1.MenuItems.Add(Menu_L1);
                        listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity(Ent_1.Cat_Code, Ent_1.Cat_Desc));
                    }
                }
                else
                {
                    if (Sel_Move_Copy_Rec_Details[0] == SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim() &&
                             Sel_Move_Copy_Rec_Details[1] == SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim() &&
                             Sel_Move_Copy_Rec_Details[4] == SPDetailsGrid.CurrentRow.Cells["Group"].Value.ToString() &&
                             Sel_Move_Copy_Rec_Details[2] == SPDetailsGrid.CurrentRow.Cells["CAMA_Org_Grp"].Value.ToString().Trim() &&
                             Sel_Move_Copy_Rec_Details[3] == SPDetailsGrid.CurrentRow.Cells["Sequence"].Value.ToString().Trim())
                    {
                        Can_paste_CAMS = false;
                        listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("U", "Unselect"));
                    }
                    else
                    {

                        if (Can_paste_CAMS)
                        {
                            listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("P", "Paste ABOVE"));
                            listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("B", "paste BELOW"));
                        }
                    }
                }

                string Active_Status = SPDetailsGrid.CurrentRow.Cells["Active_Stat"].Value.ToString();
                if (Active_Status != "N")
                {
                    if (Active_Status == "A")
                        listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("I Inactive", "Inactive"));
                    else
                        listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("A Active", "Active"));
                }

                if (SP_Header_Rec.Auto_Post_SP == "Y")
                {
                    string Autopost = SPDetailsGrid.CurrentRow.Cells["Auto_Post"].Value.ToString();
                    if (Autopost == "Y")
                        listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("N Remove Auto Post", "Remove Auto Post"));
                    else
                        listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("Y Auto Post", "Auto Post"));
                }

                foreach (PopUp_Menu_L1_Entity Ent_1 in listItem_L1_Menu)
                {
                    MenuItem Menu_L1 = new MenuItem();
                    Menu_L1.Text = Ent_1.Cat_Desc;
                    Menu_L1.Tag = Ent_1.Cat_Code;
                    contextMenu1.MenuItems.Add(Menu_L1);
                }
                System.Windows.Forms.VScrollBar vScroller = new System.Windows.Forms.VScrollBar();


                //contextMenu1.Show(SPDetailsGrid, new Point(1, 10));
            }
        }

        private void Prepare_Move_Related_Groups()
        {
            //bool Can_Add_Move_T0_Menu = false, Can_Add_Copy_T0_Menu = false;
            contextMenu1.MenuItems[0].MenuItems.Clear();
            foreach (PopUp_Menu_L2_Entity Ent_2 in listItem_L2_Menu)
            {
                foreach (PopUp_Menu_L3_Entity Ent_3 in listItem_L3_Menu)
                {
                    if (Ent_2.Cat_Code == "M" && Ent_2.Cat_Code == Ent_3.Cat_Code && Ent_2.Grp_Code == Ent_3.Curr_Grp)
                    {
                        MenuItem Menu_Move_L2 = new MenuItem();
                        Menu_Move_L2.Text = Ent_2.Grp_Desc;
                        Menu_Move_L2.Tag = "M " + Ent_2.Grp_Code;

                        contextMenu1.MenuItems[0].MenuItems.Add(Menu_Move_L2); break;
                    }
                }
            }
        }

        private void Prepare_Copy_Related_Groups()
        {
            //bool Can_Add_Move_T0_Menu = false, Can_Add_Copy_T0_Menu = false;
            contextMenu1.MenuItems[1].MenuItems.Clear();
            foreach (PopUp_Menu_L2_Entity Ent_2 in listItem_L2_Menu)
            {
                foreach (PopUp_Menu_L3_Entity Ent_3 in listItem_L3_Menu)
                {
                    if (Ent_2.Cat_Code == "C" && Ent_2.Cat_Code == Ent_3.Cat_Code && Ent_2.Grp_Code == Ent_3.Curr_Grp &&
                        Ent_2.Grp_Code.ToString() != SPDetailsGrid.CurrentRow.Cells["Group"].Value.ToString().Trim())
                    {
                        MenuItem Menu_Move_L2 = new MenuItem();
                        Menu_Move_L2.Text = Ent_2.Grp_Desc;
                        Menu_Move_L2.Tag = "C " + Ent_2.Grp_Code;

                        contextMenu1.MenuItems[1].MenuItems.Add(Menu_Move_L2); break;
                    }
                }
            }
        }

        private bool Can_Add_Group_to_Menu(int Grp_Code, string Category)
        {
            bool Can_Add = true;

            int i = 0;

            if (Category.Equals("M"))
            {
                if (SPDetailsGrid.CurrentRow.Cells["Group"].Value.ToString() != Grp_Code.ToString())
                {
                    foreach (CASESP2Entity Entity in Branch_CAMS_Details) // CAMA_Details) //////Branch_CAMS_Details) 
                    {

                        if (Grp_Code == Entity.Curr_Grp &&
                            Entity.CamCd.Trim() == SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim() &&
                            Entity.Type1 == SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim())// &&
                        {
                            Can_Add = false; break;
                        }
                    }
                }
                else
                {
                    foreach (CASESP2Entity Entity in Branch_CAMS_Details)
                    {
                        if (Grp_Code == Entity.Curr_Grp)
                            i++;
                    }
                    if (i <= 1)
                        Can_Add = false;
                }
            }
            return Can_Add;
        }

        private void SPDetailsGrid_SelectionChanged(object sender, EventArgs e)
        {
            //ColIdx = FLDCNTLGrid.CurrentCell.ColumnIndex;
            //RowIdx = FLDCNTLGrid.CurrentCell.RowIndex;
            Curr_Group = int.Parse(SPDetailsGrid.CurrentRow.Cells["Group"].Value.ToString());

            ////if (Priv_Group != Curr_Group)
            contextMenu1_Popup(contextMenu1, EventArgs.Empty);

            ////Priv_Group = Curr_Group;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {


            //Added by Sudheer on 01/31/2021
            pictureBox2.Visible = false;
            BranchPanel.Visible = false;

            if (Member_Activity == "N")
            {
                lblCategory.Visible = false; label2.Visible = false; cmbCategory.Visible = false;
                lblPartner.Visible = false; cmbPartner.Visible = false;
                this.panel1.Size = new System.Drawing.Size(860, 75);
                // this.SpDetailsPanel.Location = new System.Drawing.Point(1, 124);
                // this.SpDetailsPanel.Size = new System.Drawing.Size(666, 317);
                //  this.SPDetailsGrid.Location = new System.Drawing.Point(3, 2);
                // this.SPDetailsGrid.Size = new System.Drawing.Size(641, 310);
                // this.pictureBox3.Location = new System.Drawing.Point(646, 2);
                // this.panel4.Location = new System.Drawing.Point(1, 440);
            }
            else
            {
                lblCategory.Visible = true; label2.Visible = true; cmbCategory.Visible = true;
                lblPartner.Visible = true; cmbPartner.Visible = true;
                this.panel1.Size = new System.Drawing.Size(860, 113);
                // this.SpDetailsPanel.Location = new System.Drawing.Point(1, 151);
                // this.SpDetailsPanel.Size = new System.Drawing.Size(666, 317);
                // this.SPDetailsGrid.Location = new System.Drawing.Point(3, 2);
                //this.SPDetailsGrid.Size = new System.Drawing.Size(641, 310);
                // this.pictureBox3.Location = new System.Drawing.Point(646, 2);
            }
            pictureBox3.Visible = true;

        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

            //Added by Sudheer on 01/30/2021
            pictureBox3.Visible = false;
            BranchPanel.Visible = true;
            if (Member_Activity == "N")
            {
                lblCategory.Visible = false; label2.Visible = false; cmbCategory.Visible = false;
                lblPartner.Visible = false; cmbPartner.Visible = false;
                this.panel1.Size = new System.Drawing.Size(860, 75);

                //this.SpDetailsPanel.Location = new System.Drawing.Point(1, 237);
                //this.SpDetailsPanel.Size = new System.Drawing.Size(666, 201);
                //this.SPDetailsGrid.Location = new System.Drawing.Point(3, 2);
                //this.SPDetailsGrid.Size = new System.Drawing.Size(641, 194);
                if (SPDetailsGrid.Rows.Count >= 9)
                    pictureBox2.Visible = true;
                // this.panel4.Location = new System.Drawing.Point(1, 440);
                // this.Size = new System.Drawing.Size(670, 474);
            }
            else
            {
                lblCategory.Visible = true; label2.Visible = true; cmbCategory.Visible = true;
                lblPartner.Visible = true; cmbPartner.Visible = true;
                this.panel1.Size = new System.Drawing.Size(860, 113);

                //this.SpDetailsPanel.Location = new System.Drawing.Point(1, 264);
                //this.SpDetailsPanel.Size = new System.Drawing.Size(666, 201);
                //this.SPDetailsGrid.Location = new System.Drawing.Point(3, 2);
                //this.SPDetailsGrid.Size = new System.Drawing.Size(641, 194);
                if (SPDetailsGrid.Rows.Count >= 9)
                    pictureBox2.Visible = true;
                //this.Size = new System.Drawing.Size(670, 501);
            }

        }

        string Sel_Opr_Move_Copy = "", gbl_Move_AboveBelow = "";
        string[] Sel_Move_Copy_Rec_Details = new string[5];
        string[] CAMS_Move_Copy_Details = new string[4];
        private void SPDetailsGrid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            CAMS_Move_Copy_Details[0] = CAMS_Move_Copy_Details[1] = CAMS_Move_Copy_Details[2] = CAMS_Move_Copy_Details[3] = null;
            string[] Split_Array = new string[2];

            if (objArgs.MenuItem.Tag is string)
            {
                Split_Array = Regex.Split(objArgs.MenuItem.Tag.ToString(), " ");
                CAMS_Move_Copy_Details[0] = Split_Array[0];         // Move to Sequence         --  Active/Inactive Code  
                //CAMS_Move_Copy_Details[1] = Split_Array[1];         // Selected Record type     --  Active/Inactive Desc 

                if ((Split_Array[0] == "Y" || Split_Array[0] == "N") && (Split_Array[1] == "Remove" || Split_Array[1] == "Auto"))
                {
                    Set_Reset_Auto_Post_Option(CAMS_Move_Copy_Details[0]);
                    return;
                }


                if (CAMS_Move_Copy_Details[0] == "U")
                {
                    SPDetailsGrid.Rows[SPDetailsGrid.CurrentRow.Index].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    Move_Copy_Rec_Sel = false;
                    Sel_Opr_Move_Copy = "";
                    return;
                }

                if ((CAMS_Move_Copy_Details[0] == "M" || CAMS_Move_Copy_Details[0] == "C") && !Move_Copy_Rec_Sel)
                {
                    Sel_Move_Copy_Rec_Details[0] = Sel_Move_Copy_Rec_Details[1] = Sel_Move_Copy_Rec_Details[2] = Sel_Move_Copy_Rec_Details[3] = Sel_Move_Copy_Rec_Details[4] = Sel_Opr_Move_Copy = "";
                    //    CAMS_Code                    CAMS_Type                      CAMS_Org_Group                 CAMS_Sequence                  CAMS_Curr_Gropu
                    Sel_Move_Copy_Rec_Details[0] = SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim();
                    Sel_Move_Copy_Rec_Details[1] = SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim();
                    Sel_Move_Copy_Rec_Details[2] = SPDetailsGrid.CurrentRow.Cells["CAMA_Org_Grp"].Value.ToString().Trim();
                    Sel_Move_Copy_Rec_Details[3] = SPDetailsGrid.CurrentRow.Cells["Sequence"].Value.ToString();
                    Sel_Move_Copy_Rec_Details[4] = SPDetailsGrid.CurrentRow.Cells["Group"].Value.ToString();
                    Sel_Opr_Move_Copy = CAMS_Move_Copy_Details[0];
                    SPDetailsGrid.Rows[SPDetailsGrid.CurrentRow.Index].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                    Move_Copy_Rec_Sel = true;
                    return;
                }


                if (CAMS_Move_Copy_Details[0] != "A" && CAMS_Move_Copy_Details[0] != "I")   // To Allow only Move Or Copy Operations
                {
                    Split_Array = null;
                    Split_Array = Regex.Split(objArgs.MenuItem.Tag.ToString(), " ");

                    //Split_Array = Regex.Split(objArgs.MenuItem.Parent.Tag.ToString(), " ");
                    //CAMS_Move_Copy_Details[2] = Split_Array[0];        // Operation to Perform (Move 'M' / Coyp 'C')
                    //CAMS_Move_Copy_Details[3] = Split_Array[1];        // Current Group of Selected Record

                    string Move_Copy_Above_Below_Flag = CAMS_Move_Copy_Details[0];
                    gbl_Move_AboveBelow = Move_Copy_Above_Below_Flag;
                    CAMS_Move_Copy_Details[0] = SPDetailsGrid.CurrentRow.Cells["Sequence"].Value.ToString();


                    CAMS_Move_Copy_Details[2] = Sel_Opr_Move_Copy;        // Operation to Perform (Move 'M' / Coyp 'C')
                    CAMS_Move_Copy_Details[3] = SPDetailsGrid.CurrentRow.Cells["Group"].Value.ToString();        // Current Group of Selected Record

                    Move_Copy_SelectedCAMS();



                    if (Row_Shifted)
                    {

                        gbl_Move_AboveBelow = "";

                        Fill_Branch_Grid_With_New_CAMSDetails();

                        Move_Copy_Rec_Sel = false;
                        SP_Header_Rec.Validate = "N";
                        BtnValidate.Visible = true;
                        Refresh_ADMN20_Control = true;
                    }
                }
                else
                {
                    foreach (CASESP2Entity Ent in Branch_CAMS_Details) // rao 06282014
                    {
                        if (Ent.CamCd.Trim() == SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim() &&
                            Ent.Type1 == SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim() &&
                            Ent.Orig_Grp.ToString() == SPDetailsGrid.CurrentRow.Cells["CAMA_Org_Grp"].Value.ToString().Trim())
                        {
                            Ent.SP2_CAMS_Active = CAMS_Move_Copy_Details[0];
                            Ent.lstcOperator = BaseForm.UserID;
                            if (_model.SPAdminData.UpdateCASESP2(Ent, string.Empty, out Sql_SP_Result_Message))
                            {
                                SPDetailsGrid.CurrentRow.Cells["Active_Stat"].Value = CAMS_Move_Copy_Details[0];
                                if (CAMS_Move_Copy_Details[0] == "A")
                                    SPDetailsGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Black;
                                else
                                    SPDetailsGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void Set_Reset_Auto_Post_Option(string Set)
        {
            foreach (CASESP2Entity Ent in Branch_CAMS_Details) // rao 06282014
            {
                if (Ent.CamCd.Trim() == SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim() &&
                    Ent.Type1 == SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim() &&
                    Ent.Orig_Grp.ToString() == SPDetailsGrid.CurrentRow.Cells["CAMA_Org_Grp"].Value.ToString().Trim())
                {
                    Ent.SP2_Auto_Post = Set;
                    Ent.lstcOperator = BaseForm.UserID;
                    if (_model.SPAdminData.UpdateCASESP2(Ent, string.Empty, out Sql_SP_Result_Message))
                    {
                        SPDetailsGrid.CurrentRow.Cells["Auto_Post"].Value = Set;

                        if (Set == "Y")
                            SPDetailsGrid.CurrentRow.Cells["Auto_Post_Disp"].Value = "A";
                        else
                            SPDetailsGrid.CurrentRow.Cells["Auto_Post_Disp"].Value = "";



                    }
                    break;
                }
            }

        }

        private void Fill_Branch_Grid_With_New_CAMSDetails()
        {
            PbDelete.Visible = BtnValidate.Enabled = BtnSaveSP.Enabled = false;
            Sel_Del_Count = 0;
            Btn_Validate_Branch.Visible = true;
            this.SPDetailsGrid.SelectionChanged -= new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
            int rowIndex = 0, Tmp_Row_Cnt = 0;
            string Autopost_Disp = "";

            SPDetailsGrid.Rows.Clear();

            if (Branch_CAMS_Details.Count > 0 && Row_Shifted) //CAMA_Details 
            {
                foreach (CASESP2Entity Entity in Branch_CAMS_Details) //CAMA_Details
                {
                    Autopost_Disp = (Entity.SP2_Auto_Post == "Y" ? "A" : "");

                    string TypeChange = string.Empty;
                    if (Entity.Type1 == "CA") TypeChange = "Ser"; else if (Entity.Type1 == "MS") TypeChange = "Out";

                    rowIndex = SPDetailsGrid.Rows.Add(false, Entity.CAMS_Desc, Entity.CamCd, TypeChange, Autopost_Disp, Entity.Curr_Grp, Entity.Row, Entity.Type1, Entity.Orig_Grp, Entity.CAMS_Post_Count, Entity.CAMS_Post_Count, (Entity.CAMS_Active == "False" ? "N" : Entity.SP2_CAMS_Active), Entity.Existing_CAMS, Entity.SP2_Auto_Post, Entity.SP2_OBF, Entity.SP2_COUNT_TYPE);

                    if (Entity.CAMS_Active == "False" || Entity.SP2_CAMS_Active != "A")
                        SPDetailsGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;  // Color.Red;

                    Tmp_Row_Cnt++;
                }
            }

            if (SPDetailsGrid.RowCount > 0 && Tmp_Row_Cnt > 0)
            {
                this.SpDesc.HeaderText = BranchGrid.CurrentRow.Cells["BranchDef"].Value.ToString().Trim();
                SPDetailsGrid.CurrentCell = SPDetailsGrid.Rows[(int.Parse(CAMS_Move_Copy_Details[0]))].Cells[1];
                SPDetailsGrid.Rows[0].Tag = 0;
                Prepare_Menu_items();
            }

            if (SPDetailsGrid.Rows.Count >= 9)
                pictureBox2.Visible = true;

            this.SPDetailsGrid.SelectionChanged += new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
        }

        private void Fill_Branch_Grid_With_New_CAMSDetails_After_Delete()
        {
            PbDelete.Visible = BtnValidate.Enabled = BtnSaveSP.Enabled = false;
            Sel_Del_Count = 0;
            Btn_Validate_Branch.Visible = true;
            this.SPDetailsGrid.SelectionChanged -= new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
            int rowIndex = 0, Tmp_Row_Cnt = 0;

            SPDetailsGrid.Rows.Clear();

            string Priv_Type = null;
            int Tmp_loop_Cnt = 0, Tmp_Grp_Cnt = 1;
            foreach (CASESP2Entity Entity in Branch_CAMS_Details)
            {
                if (Tmp_loop_Cnt > 0)
                {
                    if (Entity.Type1 == "CA" && Priv_Type == "MS")
                        Tmp_Grp_Cnt++;
                }

                Entity.Curr_Grp = Tmp_Grp_Cnt;
                Tmp_loop_Cnt++;
                Priv_Type = Entity.Type1;
            }

            if (Branch_CAMS_Details.Count > 0)
            {
                string Autopost_Disp = "";
                this.SPDetailsGrid.SelectionChanged -= new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
                SPDetailsGrid.Rows.Clear();
                pictureBox2.Visible = false;
                Tmp_loop_Cnt = 1;
                foreach (CASESP2Entity Entity in Branch_CAMS_Details) ////// CAMA_Details) //////Branch_CAMS_Details
                {
                    Entity.Row = Tmp_loop_Cnt;
                    Autopost_Disp = (Entity.SP2_Auto_Post == "Y" ? "A" : "");

                    string TypeChange = string.Empty;
                    if (Entity.Type1 == "CA") TypeChange = "Ser"; else if (Entity.Type1 == "MS") TypeChange = "Out";

                    rowIndex = SPDetailsGrid.Rows.Add(false, Entity.CAMS_Desc, Entity.CamCd, TypeChange, Autopost_Disp, Entity.Curr_Grp, Entity.Row, Entity.Type1, Entity.Orig_Grp, Entity.CAMS_Post_Count, Entity.CAMS_Post_Count, (Entity.CAMS_Active == "False" ? "N" : Entity.SP2_CAMS_Active), Entity.Existing_CAMS, Entity.SP2_Auto_Post, Entity.SP2_OBF, Entity.SP2_COUNT_TYPE);

                    Tmp_loop_Cnt++;
                }
                SPDetailsGrid.Rows[0].Tag = 0;
                if (SPDetailsGrid.Rows.Count >= 9)
                    pictureBox2.Visible = true;
                Prepare_Menu_items();

                this.SPDetailsGrid.SelectionChanged += new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
            }

            this.SPDetailsGrid.SelectionChanged += new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
        }


        bool Row_Shifted = false;
        string[] Target_Move_Copy_Rec_Details = new string[5];
        private void Move_Copy_SelectedCAMS()
        {

            if (CAMA_Details.Count > 0)
            {
                Target_Move_Copy_Rec_Details[0] = SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim();
                Target_Move_Copy_Rec_Details[1] = SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim();
                Target_Move_Copy_Rec_Details[2] = SPDetailsGrid.CurrentRow.Cells["CAMA_Org_Grp"].Value.ToString().Trim();
                Target_Move_Copy_Rec_Details[3] = SPDetailsGrid.CurrentRow.Cells["Sequence"].Value.ToString();
                Target_Move_Copy_Rec_Details[4] = SPDetailsGrid.CurrentRow.Cells["Group"].Value.ToString();

                List<CASESP2Entity> New_CAMA_Details = new List<CASESP2Entity>();

                Row_Shifted = false;
                CASESP2Entity New_Rec_To_CopyMove = new CASESP2Entity();
                foreach (CASESP2Entity Entity in Branch_CAMS_Details) // CAMA_Details) //////Branch_CAMS_Details) 
                {
                    //if (Entity.CamCd.Trim() == SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim() &&
                    //    Entity.Type1 == SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim()             &&
                    //    Entity.Orig_Grp.ToString() == SPDetailsGrid.CurrentRow.Cells["CAMA_Org_Grp"].Value.ToString().Trim())
                    if (Entity.CamCd.Trim() == Sel_Move_Copy_Rec_Details[0] &&
                        Entity.Type1 == Sel_Move_Copy_Rec_Details[1] &&
                        Entity.Orig_Grp.ToString() == Sel_Move_Copy_Rec_Details[2])
                    {
                        //New_Rec_To_CopyMove = Entity;

                        New_Rec_To_CopyMove.ServPlan = Entity.ServPlan;
                        New_Rec_To_CopyMove.Branch = Entity.Branch;
                        New_Rec_To_CopyMove.Type1 = Entity.Type1;
                        New_Rec_To_CopyMove.CamCd = Entity.CamCd;
                        New_Rec_To_CopyMove.Row = Entity.Row;
                        New_Rec_To_CopyMove.DateLstc = Entity.DateLstc;
                        New_Rec_To_CopyMove.SP2_CAMS_Active = Entity.SP2_CAMS_Active;
                        New_Rec_To_CopyMove.CAMS_Active = Entity.CAMS_Active;
                        New_Rec_To_CopyMove.CAMS_Post_Count = Entity.CAMS_Post_Count;
                        New_Rec_To_CopyMove.lstcOperator = Entity.lstcOperator;
                        New_Rec_To_CopyMove.Dateadd = Entity.Dateadd;
                        New_Rec_To_CopyMove.addoperator = Entity.addoperator;
                        New_Rec_To_CopyMove.SP2_Auto_Post = (Entity.SP2_Auto_Post.Trim() != "Y" ? "N" : Entity.SP2_Auto_Post.Trim());
                        New_Rec_To_CopyMove.SP2_OBF = Entity.SP2_OBF;
                        New_Rec_To_CopyMove.SP2_COUNT_TYPE = Entity.SP2_COUNT_TYPE;
                        if (CAMS_Move_Copy_Details[2] == "C") //if (CAMS_Move_Copy_Details[2] == "C")
                        {
                            New_Rec_To_CopyMove.Rec_Type = "I";
                            New_Rec_To_CopyMove.SP2_CAMS_Active = "A";
                            New_Rec_To_CopyMove.CAMS_Post_Count = "0";
                            New_Rec_To_CopyMove.Existing_CAMS = "N";
                            New_Rec_To_CopyMove.Orig_Grp = int.Parse(CAMS_Move_Copy_Details[3]);
                            New_Rec_To_CopyMove.SP2_Auto_Post = (Entity.SP2_Auto_Post.Trim() != "Y" ? "N" : Entity.SP2_Auto_Post.Trim());
                            if (int.Parse(CAMS_Move_Copy_Details[3]) == Entity.Orig_Grp)
                                New_Rec_To_CopyMove.Orig_Grp++;
                        }
                        else
                        {
                            New_Rec_To_CopyMove.Rec_Type = "U";
                            New_Rec_To_CopyMove.Orig_Grp = Entity.Orig_Grp;
                        }
                        New_Rec_To_CopyMove.CAMS_Desc = Entity.CAMS_Desc;
                        New_Rec_To_CopyMove.Shift_Count = Entity.Shift_Count;


                        SP_Header_Rec.Validate = "N";
                        break;
                    }
                }


                bool Can_Add_Rec = true; ;
                int Tmp_loop_Cnt = 1, Tmp_Grp_Cnt = 1;
                foreach (CASESP2Entity Entity in Branch_CAMS_Details)
                {
                    Entity.Curr_Grp = 0; Can_Add_Rec = true;


                    if (Entity.CamCd.Trim() == Target_Move_Copy_Rec_Details[0] && Entity.Type1.Trim() == Target_Move_Copy_Rec_Details[1] &&
                       Entity.Orig_Grp.ToString() == Target_Move_Copy_Rec_Details[2] && Entity.Row.ToString() == Target_Move_Copy_Rec_Details[3])// &&
                                                                                                                                                 //Entity.Curr_Grp.ToString() == Target_Move_Copy_Rec_Details[4])
                    {
                        if (gbl_Move_AboveBelow == "B")
                        {
                            New_CAMA_Details.Add(new CASESP2Entity(Entity));
                            New_CAMA_Details.Add(new CASESP2Entity(New_Rec_To_CopyMove)); Row_Shifted = true;
                            CAMS_Move_Copy_Details[0] = (New_CAMA_Details.Count - 1).ToString();
                            Can_Add_Rec = false;
                        }
                        else
                        {
                            New_CAMA_Details.Add(new CASESP2Entity(New_Rec_To_CopyMove));
                            CAMS_Move_Copy_Details[0] = (New_CAMA_Details.Count - 1).ToString();
                            New_CAMA_Details.Add(new CASESP2Entity(Entity)); Row_Shifted = true;
                            Can_Add_Rec = false;
                        }
                    }

                    if (Entity.CamCd.Trim() == Sel_Move_Copy_Rec_Details[0] && Entity.Type1.Trim() == Sel_Move_Copy_Rec_Details[1] &&
                       Entity.Orig_Grp.ToString() == Sel_Move_Copy_Rec_Details[2] && Entity.Row.ToString() == Sel_Move_Copy_Rec_Details[3] &&
                       CAMS_Move_Copy_Details[2] == "M")
                        Can_Add_Rec = false;

                    if (Can_Add_Rec)
                        New_CAMA_Details.Add(new CASESP2Entity(Entity));

                    Entity.Shift_Count = Tmp_loop_Cnt++;
                }

                if (!Row_Shifted && (Branch_CAMS_Details.Count < int.Parse(CAMS_Move_Copy_Details[0])))   // Adds to entity when moved from Lower Sequence to Heigher Sequence
                { New_CAMA_Details.Add(new CASESP2Entity(New_Rec_To_CopyMove)); Row_Shifted = true; }


                string Priv_Type = null;
                Tmp_loop_Cnt = 0;
                foreach (CASESP2Entity Entity in New_CAMA_Details)
                {
                    if (Tmp_loop_Cnt > 0)
                    {
                        if (Entity.Type1 == "CA" && Priv_Type == "MS")
                            Tmp_Grp_Cnt++;
                    }

                    Entity.Curr_Grp = Tmp_Grp_Cnt;
                    Tmp_loop_Cnt++;
                    Priv_Type = Entity.Type1;
                }


                Branch_CAMS_Details = New_CAMA_Details;
                if (Branch_CAMS_Details.Count > 0)
                {
                    string Autopost_Disp = "";
                    this.SPDetailsGrid.SelectionChanged -= new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
                    SPDetailsGrid.Rows.Clear();
                    pictureBox2.Visible = false;
                    Tmp_loop_Cnt = 1;
                    foreach (CASESP2Entity Entity in Branch_CAMS_Details) ////// CAMA_Details) //////Branch_CAMS_Details
                    {
                        int rowIndex = 0;

                        Entity.Row = Tmp_loop_Cnt;
                        Autopost_Disp = (Entity.SP2_Auto_Post == "Y" ? "A" : "");

                        string TypeChange = string.Empty;
                        if (Entity.Type1 == "CA") TypeChange = "Ser"; else if (Entity.Type1 == "MS") TypeChange = "Out";

                        //rowIndex = SPDetailsGrid.Rows.Add(false, Entity.CAMS_Desc, Entity.Row, Entity.Type1, Entity.Curr_Grp, Entity.Orig_Grp, Entity.CamCd, Entity.SP2_CAMS_Active, Entity.Existing_CAMS);
                        rowIndex = SPDetailsGrid.Rows.Add(false, Entity.CAMS_Desc, Entity.CamCd, TypeChange, Autopost_Disp, Entity.Curr_Grp, Entity.Row, Entity.Type1, Entity.Orig_Grp, Entity.CAMS_Post_Count, Entity.CAMS_Post_Count, (Entity.CAMS_Active == "False" ? "N" : Entity.SP2_CAMS_Active), Entity.Existing_CAMS, Entity.SP2_Auto_Post, Entity.SP2_OBF, Entity.SP2_COUNT_TYPE);

                        Tmp_loop_Cnt++;
                    }
                    SPDetailsGrid.Rows[0].Tag = 0;
                    if (SPDetailsGrid.Rows.Count >= 9)
                        pictureBox2.Visible = true;
                    Prepare_Menu_items();

                    this.SPDetailsGrid.SelectionChanged += new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
                }
            }
        }

        private bool Update_SP2_Move_Copy()
        {
            bool Saved = false;
            if (Branch_CAMS_Details.Count > 0 && Row_Shifted) //CAMA_Details      // Commented on 07/29/2014 to save all move_Cpoy operations at a time 
            {
                foreach (CASESP2Entity Entity in Branch_CAMS_Details) //CAMA_Details
                {
                    if (SPDetailsGrid.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow dr in SPDetailsGrid.Rows)
                        {
                            if (Entity.CamCd.Trim() == dr["CAMS_Code"].Value.ToString().Trim())
                            {
                                string OBF = dr.Cells["OBF"].Value == null ? string.Empty : dr.Cells["OBF"].Value.ToString().Trim();
                                if (!string.IsNullOrEmpty(OBF.Trim()))
                                    Entity.SP2_OBF = OBF;
                                string CountType = string.Empty;
                                if (chkbBulk.Checked)
                                {
                                    CountType = dr.Cells["gvCountType"].Value == null ? string.Empty : dr.Cells["gvCountType"].Value.ToString().Trim();
                                    if (!string.IsNullOrEmpty(CountType.Trim()))
                                        Entity.SP2_COUNT_TYPE = CountType;
                                }
                                else
                                    Entity.SP2_COUNT_TYPE = CountType;
                                break;
                            }
                        }
                    }


                    if (_model.SPAdminData.UpdateCASESP2(Entity, "RowChange", out Sql_SP_Result_Message))
                        Saved = true;
                }
            }

            return Saved;
        }


        private void BtnSaveSP_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                //if (boolServiceCateValidateHie())
                //{
                //Initialize_database_Exception_Handling_Parameters();
                if (Mode.Equals("Edit"))
                {
                    bool SP2_Saved = false;

                    if (Row_Shifted)
                    {
                        Delete_CAMS_If_Any_from_MoveCopy();
                        SP2_Saved = Update_SP2_Move_Copy();
                    }
                    else
                    {
                        if (BaseForm.BaseAgencyControlDetails.CAOBO == "Y")
                        {
                            Row_Shifted = true;
                            SP2_Saved = Update_SP2_Move_Copy();
                        }
                    }


                    bool Branch_Saved = Update_CASESP0_Branches();

                    if (Hie_Changed_FLG)
                        Update_CASESP1_Hierarchies();

                    bool CAMS_Saved = true;

                    if (SP2_Saved)
                    {
                        Branch_CAMS_Delete_List.Clear();
                        Fill_SP_CAMS_Details_Grid();
                        Fill_Branch_CAMS_Details_Grid();
                        Row_Shifted = false;


                    }

                    if (CAMS_Saved && Branch_Saved)
                    {
                        // Lbl_Saved.Visible = 
                        Refresh_ADMN20_Control = true;
                        //tm = new Timer();
                        //Lbl_Timer.Interval = 10000;
                        //Lbl_Timer.Tick += new EventHandler(Lbl_Timer_Tick);
                        //Lbl_Timer.Start();
                        //MessageBox.Show("Service Plan Updated Successfully", "CAP Systems", MessageBoxButtons.OK); // Lisa Asked to Comment on 01072013
                        //Set_Branch_Desc_and_DupDesc();
                        AlertBox.Show("Service Plan Updated Successfully");
                    }
                }
                else
                {

                    SP_Header_Rec = new CASESP0Entity();

                    SP_Header_Rec.Rec_Type = "I";
                    SP_Header_Rec.Code = "0";
                    SP_Header_Rec.Desc = Txtdesc.Text.Trim();
                    SP_Header_Rec.Status = null;
                    SP_Header_Rec.Legals = null;
                    SP_Header_Rec.Funds = null;
                    SP_Header_Rec.Validate = SP_Header_Rec.Active = SP_Header_Rec.Auto_Post_SP = "N";
                    SP_Header_Rec.lstcOperator = BaseForm.UserID;

                    if (Cb_Active.Checked)
                        SP_Header_Rec.Active = "Y";

                    SP_Header_Rec.Allow_Add_Branch = SP_Header_Rec.Allow_Duplicates = "N";
                    if (Cb_Allow_Add_Branch.Checked)
                        SP_Header_Rec.Allow_Add_Branch = "Y";

                    if (Cb_Allow_Dulpicates.Checked)
                        SP_Header_Rec.Allow_Duplicates = "Y";

                    if (Cb_Auto_Post.Checked)
                        SP_Header_Rec.Auto_Post_SP = "Y";

                    if (chkReadOnly.Checked)
                        SP_Header_Rec.Sp0ReadOnly = "Y";
                    else
                        SP_Header_Rec.Sp0ReadOnly = "N";

                    if (cbNoSPM.Checked)
                        SP_Header_Rec.NoSPM = "Y";
                    else SP_Header_Rec.NoSPM = "N";



                    bool Result = Update_CASESP0_Branches();

                    if (Result)
                    {
                        if (Hie_Changed_FLG)
                            Update_CASESP1_Hierarchies();

                        Refresh_ADMN20_Control = true;
                        //ADMN0020control ADMN20Control = BaseForm.GetBaseUserControl() as ADMN0020control;
                        //if (ADMN20Control != null)
                        //    ADMN20Control.Refresh();
                        this.Close();
                        AlertBox.Show("Service Plan Inserted Successfully");
                        //MessageBox.Show("Service Plan Inserted Successfully", "CAP Systems", MessageBoxButtons.OK); // Lisa Asked to Comment on 01072013
                    }
                    else
                        AlertBox.Show("Exception: " + Sql_SP_Result_Message, MessageBoxIcon.Warning);

                }
            }
            //}
        }


        private void Delete_CAMS_If_Any_from_MoveCopy()
        {
            foreach (CASESP2Entity Entity in Branch_CAMS_Delete_List)
                DatabaseLayer.SPAdminDB.Delete_CASESP2(int.Parse(SP_Code), Entity.Branch, Entity.Orig_Grp, Entity.Type1, Entity.CamCd);
        }

        private void Update_CASESP1_Hierarchies()
        {
            CASESP1Entity Entity = new CASESP1Entity();
            string Hierarchy = null;
            bool Hierarchy_Saved = true, Old_Hierarchy = false, Delete_Hierarchy = true;

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            foreach (DataGridViewRow dr in HierarchyGrid.Rows)
            {
                Hierarchy = dr.Cells["Hie_Code"].Value.ToString();
                str.Append("<Row AGENCY = \"" + Hierarchy.Substring(0, 2) + "\" DEPT = \"" + Hierarchy.Substring(2, 2) + "\" PROG = \"" + Hierarchy.Substring(4, 2) + "\" />");
            }
            str.Append("</Rows>");

            Entity.Rec_Type = "I"; Entity.Code = (Mode.Equals("Edit") ? SP_Code : New_SP_Code.ToString()); Entity.lstcOperator = BaseForm.UserID;
            if (!_model.SPAdminData.UpdateCASESP1(Entity, str.ToString(), out Sql_SP_Result_Message))
                Hierarchy_Saved = false;



            Hie_Changed_FLG = false;
        }


        string Sql_SP_Result_Message = string.Empty;
        int New_SP_Code = 0;
        private bool Update_CASESP0_Branches()
        {
            int Tmp_loop_cnt = 0;
            bool Save_result = false, Can_Edit_Record = true;

            if (!Row_Shifted && Mode.Equals("Edit"))
            {
                CASESP0Entity Search_Entity = new CASESP0Entity();
                Search_Entity = _model.SPAdminData.Browse_CASESP0(SP_Code, null, null, null, null, null, null, null, null);
                if (Search_Entity != null)
                    SP_Header_Rec = Search_Entity;
                else
                { AlertBox.Show("Service Plan: '" + SP_Code + "' is already deleted by another User!!!", MessageBoxIcon.Warning); Can_Edit_Record = false; }
            }

            if (Mode.Equals("Add"))
                SP_Header_Rec.BpCode = SP_Header_Rec.BpDesc = SP_Header_Rec.B1Code = SP_Header_Rec.B1Desc =
                SP_Header_Rec.B2Code = SP_Header_Rec.B2Desc = SP_Header_Rec.B3Code = SP_Header_Rec.B3Desc =
                SP_Header_Rec.B4Code = SP_Header_Rec.B4Desc = SP_Header_Rec.B5Code = SP_Header_Rec.B5Desc = string.Empty;

            if (Can_Edit_Record)
            {
                SP_Header_Rec.Desc = Txtdesc.Text.Trim();
                foreach (DataGridViewRow dr in BranchGrid.Rows)
                {
                    switch (dr.Cells["Row_status"].Value.ToString())
                    {
                        case "Y":
                        case "A":
                            switch (Tmp_loop_cnt)
                            {
                                case 0: SP_Header_Rec.BpCode = "P"; SP_Header_Rec.BpDesc = dr.Cells["BranchDef"].Value.ToString(); break;
                                case 1: SP_Header_Rec.B1Code = "1"; SP_Header_Rec.B1Desc = dr.Cells["BranchDef"].Value.ToString(); break;
                                case 2: SP_Header_Rec.B2Code = "2"; SP_Header_Rec.B2Desc = dr.Cells["BranchDef"].Value.ToString(); break;
                                case 3: SP_Header_Rec.B3Code = "3"; SP_Header_Rec.B3Desc = dr.Cells["BranchDef"].Value.ToString(); break;
                                case 4: SP_Header_Rec.B4Code = "4"; SP_Header_Rec.B4Desc = dr.Cells["BranchDef"].Value.ToString(); break;
                                case 5: SP_Header_Rec.B5Code = "5"; SP_Header_Rec.B5Desc = dr.Cells["BranchDef"].Value.ToString(); break;
                            }
                            dr.Cells["Dup_Def"].Value = dr.Cells["BranchDef"].Value.ToString();
                            dr.Cells["Row_status"].Value = "Y";
                            break;
                        case "N":
                        case "D":
                            switch (Tmp_loop_cnt)
                            {
                                case 0: SP_Header_Rec.BpCode = string.Empty; SP_Header_Rec.BpDesc = string.Empty; break;
                                case 1: SP_Header_Rec.B1Code = string.Empty; SP_Header_Rec.B1Desc = string.Empty; break;
                                case 2: SP_Header_Rec.B2Code = string.Empty; SP_Header_Rec.B2Desc = string.Empty; break;
                                case 3: SP_Header_Rec.B3Code = string.Empty; SP_Header_Rec.B3Desc = string.Empty; break;
                                case 4: SP_Header_Rec.B4Code = string.Empty; SP_Header_Rec.B4Desc = string.Empty; break;
                                case 5: SP_Header_Rec.B5Code = string.Empty; SP_Header_Rec.B5Desc = string.Empty; break;
                            }
                            break;
                    }
                    Tmp_loop_cnt++;

                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }


                SP_Header_Rec.Active = "N";
                SP_Header_Rec.Allow_Add_Branch = SP_Header_Rec.Allow_Duplicates = SP_Header_Rec.Auto_Post_SP = "N";
                if (Cb_Active.Checked)
                    SP_Header_Rec.Active = "Y";

                if (Cb_Allow_Add_Branch.Checked)
                    SP_Header_Rec.Allow_Add_Branch = "Y";

                if (Cb_Allow_Dulpicates.Checked)
                    SP_Header_Rec.Allow_Duplicates = "Y";

                if (Cb_Auto_Post.Checked)
                    SP_Header_Rec.Auto_Post_SP = "Y";

                if (chkReadOnly.Checked)
                    SP_Header_Rec.Sp0ReadOnly = "Y";
                else
                    SP_Header_Rec.Sp0ReadOnly = "N";

                if (chkCaseNotes.Checked)
                    SP_Header_Rec.Sp0Notes = "Y";
                else
                    SP_Header_Rec.Sp0Notes = "N";

                if (cbNoSPM.Checked)
                    SP_Header_Rec.NoSPM = "Y";
                else
                    SP_Header_Rec.NoSPM = "N";

                if (chkbBulk.Checked)
                    SP_Header_Rec.BulkEntry = "Y";
                else
                    SP_Header_Rec.BulkEntry = "N";



                if (Member_Activity == "Y")
                {
                    SP_Header_Rec.Category = ((ListItem)cmbCategory.SelectedItem).Value.ToString();
                    if (((ListItem)cmbPartner.SelectedItem).Value.ToString().Trim() != "0")
                        SP_Header_Rec.PartnerPostingType = ((ListItem)cmbPartner.SelectedItem).Value.ToString().Trim();
                }
                else
                    SP_Header_Rec.Category = "0";

                SP_Header_Rec.lstcOperator = BaseForm.UserID;

                Row_Shifted = false;
                //Initialize_database_Exception_Handling_Parameters();

                //return _model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out SPOut_Err_Procedure, out SPOut_Err_Number, out SPOut_Err_Message, out SPOut_Err_Line); // 10292012
                //return _model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code); // 10292012

                if (_model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out Sql_SP_Result_Message))
                    Save_result = true;
                //else
                //    MessageBox.Show(Sql_SP_Result_Message, "CAP Systems");
            }

            return Save_result;
        }


        private void BranchGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool Can_Proceed = true;
            if (Mode.Equals("Edit") && BranchGrid.CurrentRow.Cells["CAMS_Det_SW"].EditedFormattedValue.ToString() == "Y")
            {
                if (string.IsNullOrEmpty(BranchGrid.CurrentRow.Cells["BranchDef"].EditedFormattedValue.ToString().Trim()))
                {
                    BranchGrid.CurrentRow.Cells["BranchDef"].Value = BranchGrid.CurrentRow.Cells["Dup_Def"].Value.ToString();
                    AlertBox.Show("Service/Outcome details exists for this Branch, \n you cannot delete this Branch Description", MessageBoxIcon.Warning);
                    BranchGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Black;
                    Can_Proceed = false;
                }
                else
                    BranchGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
            }

            if (Can_Proceed)
            {
                if (!string.IsNullOrEmpty(BranchGrid.CurrentRow.Cells["BranchDef"].EditedFormattedValue.ToString().Trim()))
                {
                    if (BranchGrid.CurrentRow.Cells["BranchDef"].Value.ToString() != BranchGrid.CurrentRow.Cells["Dup_Def"].Value.ToString())
                    {
                        if (BranchGrid.CurrentRow.Cells["Dup_Def"].Value.ToString() == " ")
                            BranchGrid.CurrentRow.Cells["Row_status"].Value = "A";
                        else
                            BranchGrid.CurrentRow.Cells["Row_status"].Value = "Y";

                        if (BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString() == "P")
                            _errorProvider.SetError(BranchGrid, null);

                        BranchGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(BranchGrid.CurrentRow.Cells["Dup_Def"].Value.ToString().Trim()))
                    {
                        BranchGrid.CurrentRow.Cells["Row_status"].Value = "D";
                        BranchGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }


        bool Sel_All_To_Delete = false, Del_Col_Header_Clicked = false;
        private void SPDetailsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (SPDetailsGrid.Rows.Count > 0)
            {
                Del_Col_Header_Clicked = false;
                if (e.ColumnIndex == 0)
                {
                    switch (Sel_All_To_Delete)
                    {
                        case true:
                            if (SPDetailsGrid.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow dr in SPDetailsGrid.Rows)
                                    dr.Cells["Del"].Value = false;
                            }
                            PbDelete.Visible = false;
                            Sel_All_To_Delete = Del_Col_Header_Clicked = false;
                            Sel_Del_Count = 0;
                            break;

                        case false:
                            if (SPDetailsGrid.Rows.Count > 0)
                            {
                                int i = 0;
                                foreach (DataGridViewRow dr in SPDetailsGrid.Rows)
                                {
                                    if (int.Parse(dr.Cells["CA_Post_Count"].Value.ToString()) == 0)
                                    {
                                        dr.Cells["Del"].Value = true;
                                        i++;
                                    }
                                }
                                Sel_Del_Count = i;
                            }

                            if (Privileges.DelPriv.Equals("true"))
                                PbDelete.Visible = true;

                            Sel_All_To_Delete = Del_Col_Header_Clicked = true;
                            break;
                    }

                }
            }
        }

        private void BranchPanel_Click(object sender, EventArgs e)
        {


        }

        List<CASESP2Entity> Branch_CAMS_Details = new List<CASESP2Entity>();
        private void BranchGrid_SelectionChanged(object sender, EventArgs e)
        {
            Row_Shifted = Move_Copy_Rec_Sel = false;
            Fill_Branch_CAMS_Details_Grid();
        }

        private void Txtdesc_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txtdesc.Text))
                _errorProvider.SetError(Txtdesc, null);
        }


        bool Refresh_ADMN20_Control = false;
        public void Refresh_Edit_Mode()
        {
            Refresh_ADMN20_Control = true;
            Fill_SP_Branch_Hier();
            //Fill_SP_CAMS_Details_Grid();
            //Fill_Branch_Grid();

            BtnAddDetails.Visible = true;
        }

        private void Rerrange_Seq_Groups()
        {
            this.SPDetailsGrid.SelectionChanged -= new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
            if (Branch_CAMS_Details.Count > 0)
            {
                string Autopost_Disp = "";
                int Tmp_loop_Cnt = 1, Tmp_Grp_Cnt = 1, rowIndex = 0;
                string Priv_Type = null;


                string Cur_Grp = SPDetailsGrid.CurrentRow.Cells["CAMA_Org_Grp"].Value.ToString().Trim();
                string Type = SPDetailsGrid.CurrentRow.Cells["Type"].Value.ToString().Trim();
                string Tmp_CAMS_Code = SPDetailsGrid.CurrentRow.Cells["CAMS_Code"].Value.ToString().Trim();

                SPDetailsGrid.Rows.Clear();
                Branch_CAMS_Details.Clear();
                Branch_CAMS_Details = _model.SPAdminData.Browse_CASESP2(SP_Code, BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString(), null, null);

                pictureBox2.Visible = false;
                foreach (CASESP2Entity Entity in Branch_CAMS_Details)
                {
                    if (Tmp_loop_Cnt > 0)
                    {
                        if (Entity.Type1 == "CA" && Priv_Type == "MS")
                            Tmp_Grp_Cnt++;
                    }

                    Entity.Curr_Grp = Tmp_Grp_Cnt;
                    Entity.Row = Tmp_loop_Cnt;
                    rowIndex = 0;

                    string TypeChange = string.Empty;
                    if (Entity.Type1 == "CA") TypeChange = "Ser"; else if (Entity.Type1 == "MS") TypeChange = "Out";

                    Autopost_Disp = (Entity.SP2_Auto_Post == "Y" ? "A" : "");
                    //rowIndex = SPDetailsGrid.Rows.Add(false, Entity.CAMS_Desc, Entity.Row, Entity.Type1, Entity.Curr_Grp, Entity.Orig_Grp, Entity.CamCd, Entity.SP2_CAMS_Active, Entity.Existing_CAMS);
                    rowIndex = SPDetailsGrid.Rows.Add(false, Entity.CAMS_Desc, Entity.CamCd, TypeChange, Autopost_Disp, Entity.Curr_Grp, Entity.Row, Entity.Type1, Entity.Orig_Grp, Entity.CAMS_Post_Count, Entity.CAMS_Post_Count, (Entity.CAMS_Active == "False" ? "N" : Entity.SP2_CAMS_Active), Entity.Existing_CAMS, Entity.SP2_Auto_Post, Entity.SP2_OBF, Entity.SP2_COUNT_TYPE);
                    if (Entity.CAMS_Active == "False" || Entity.SP2_CAMS_Active != "A") //(Entity.SP2_CAMS_Active == "I" || string.IsNullOrEmpty(Entity.SP2_CAMS_Active.Trim())))
                        SPDetailsGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;  // Color.Red;

                    Tmp_loop_Cnt++;

                    Priv_Type = Entity.Type1;
                }

                if (Branch_CAMS_Details.Count > 0) //CAMA_Details
                {
                    foreach (CASESP2Entity Entity in Branch_CAMS_Details) //CAMA_Details
                    {
                        if (_model.SPAdminData.UpdateCASESP2(Entity, "RowChange", out Sql_SP_Result_Message))
                            Row_Shifted = true;



                    }
                }
            }
            //SPDetailsGrid.Rows.Clear();

            this.SPDetailsGrid.SelectionChanged += new System.EventHandler(this.SPDetailsGrid_SelectionChanged);
        }


        private void ValiDate_SP()
        {
            bool[,] Grp_Valid_Array;
            if (CAMA_Details.Count > 0 || Cb_Allow_Add_Branch.Checked)
            {
                string Error_Msg = string.Empty, Duplicate_Error = string.Empty;
                if (CAMA_Details.Count > 0)
                {
                    int Total_Branches = 0, Total_Groups = 0, PRiv_Grp = 0;

                    List<CASESP2Entity> Sel_Branch_CAMS_Details = new List<CASESP2Entity>();
                    string Priv_Branch = string.Empty, Branch_List = string.Empty;
                    foreach (CASESP2Entity Entity in CAMA_Details)
                    {
                        if (Entity.Branch != Priv_Branch)
                        {
                            Branch_List += Entity.Branch;
                            Total_Branches++;
                        }
                        Priv_Branch = Entity.Branch;
                    }


                    int CA_Count = 0, MS_Count = 0;
                    bool Grps_Exists_In_Sel_Branch = false; string Dup_Group_List = "";
                    for (int Branch_Pos = 0; Branch_Pos < Total_Branches; Branch_Pos++)
                    {
                        foreach (CASESP2Entity Entity in CAMA_Details)
                        {
                            if (Entity.Branch == Branch_List.Substring(Branch_Pos, 1))
                            {
                                if (Entity.Curr_Grp != PRiv_Grp)
                                    Total_Groups++;

                                Priv_Branch = Entity.Branch;
                                PRiv_Grp = Entity.Curr_Grp;
                            }
                        }

                        // Get Branch related CAMS into new Entity list
                        Sel_Branch_CAMS_Details = CAMA_Details.FindAll(u => u.Branch.Equals(Branch_List.Substring(Branch_Pos, 1)));
                        Grp_Valid_Array = new bool[Total_Groups, 2];
                        for (int i = 0; i < Total_Groups; i++)
                        {
                            Grps_Exists_In_Sel_Branch = false;
                            Grp_Valid_Array[i, 0] = Grp_Valid_Array[i, 1] = false;
                            CA_Count = MS_Count = 0;

                            Dup_Group_List = "";
                            foreach (CASESP2Entity Entity in Sel_Branch_CAMS_Details)
                            {
                                if (Entity.Curr_Grp == (i + 1) && Branch_List.Substring(Branch_Pos, 1) == Entity.Branch)
                                {
                                    if (!Dup_Group_List.Contains(Entity.Curr_Grp.ToString()))
                                    {
                                        Duplicate_Error += Get_Depliidate_CAMS_IN_SP(Entity.Curr_Grp, Sel_Branch_CAMS_Details);
                                        Dup_Group_List += Entity.Curr_Grp.ToString() + ",";
                                    }
                                    if (Entity.Type1 == "CA")
                                        CA_Count++;
                                    else
                                        MS_Count++;

                                    Grps_Exists_In_Sel_Branch = true;
                                }
                            }

                            foreach (CASESP2Entity Entity in CAMA_Details)
                            {
                                if (Entity.Curr_Grp == (i + 1) && Branch_List.Substring(Branch_Pos, 1) == Entity.Branch)
                                {
                                    if (Entity.Type1 == "CA")
                                        CA_Count++;
                                    else
                                        MS_Count++;

                                    Grps_Exists_In_Sel_Branch = true;
                                }
                            }

                            //commented by sudheer on 11/05/2020
                            if (Grps_Exists_In_Sel_Branch)
                            {
                                if (CA_Count > 0)
                                    Grp_Valid_Array[i, 0] = true;
                                //commented by sudheer on 11/05/2020
                                //else
                                //    //Error_Msg += "Branch-" + Branch_List.Substring(Branch_Pos, 1) + "  Group-" + (i + 1).ToString() + " Should Contain At least One CA \n";
                                //    Error_Msg += "        Group-" + (i + 1).ToString() + " in Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Should Contain At least One CA \n";

                                if (Member_Activity == "N")
                                {
                                    if (MS_Count > 0)
                                        Grp_Valid_Array[i, 1] = true;
                                    //commented by sudheer on 11/05/2020
                                    //else
                                    //    //Error_Msg += "Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Group-" + (i + 1).ToString() + " Should Contain At least One MS \n";
                                    //    Error_Msg += "        Group-" + (i + 1).ToString() + " in Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Should Contain At least One MS \n";
                                }
                            }
                        }
                    }
                }



                if (!string.IsNullOrEmpty(Error_Msg) || !string.IsNullOrEmpty(Duplicate_Error))
                {
                    if (dtAgency.Rows[0]["ACR_ROMA_SWITCH"].ToString() == "Y")
                    {
                        AlertBox.Show((!string.IsNullOrEmpty(Error_Msg) ? " Group mismatch List:\n" + Error_Msg + "\n" : "") + (!string.IsNullOrEmpty(Duplicate_Error) ? " Duplicates List:\n" + Duplicate_Error : ""), MessageBoxIcon.Warning);
                        SP_Header_Rec.Validate = "Y"; SP_Header_Rec.lstcOperator = BaseForm.UserID;

                        if (Cb_Active.Checked)
                            SP_Header_Rec.Active = "Y";

                        if (Cb_Allow_Add_Branch.Checked)
                            SP_Header_Rec.Allow_Add_Branch = "Y";

                        if (_model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out Sql_SP_Result_Message))
                        {
                            //MessageBox.Show("Service Plan-" + SP_Code + " is Validated And Updated", "CAP Systems"); // Lisa Asked to Comment on 01072013
                            BtnValidate.Visible = false;
                        }
                    }
                    else
                        AlertBox.Show((!string.IsNullOrEmpty(Error_Msg) ? " Group mismatch List:\n" + Error_Msg + "\n" : "") + (!string.IsNullOrEmpty(Duplicate_Error) ? " Duplicates List:\n" + Duplicate_Error : ""), MessageBoxIcon.Warning);
                }
                else
                {
                    SP_Header_Rec.Validate = "Y"; SP_Header_Rec.lstcOperator = BaseForm.UserID;

                    if (Cb_Active.Checked)
                        SP_Header_Rec.Active = "Y";

                    if (Cb_Allow_Add_Branch.Checked)
                        SP_Header_Rec.Allow_Add_Branch = "Y";

                    if (_model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out Sql_SP_Result_Message))
                    {
                        //MessageBox.Show("Service Plan-" + SP_Code + " is Validated And Updated", "CAP Systems"); // Lisa Asked to Comment on 01072013
                        BtnValidate.Visible = false;
                    }
                    else
                        AlertBox.Show("Unable to Update Service Plan - " + SP_Code + "\n" +
                                         "Exception: " + Sql_SP_Result_Message, MessageBoxIcon.Warning);
                }
            }
        }


        private void ValiDate_Branch()
        {
            bool[,] Grp_Valid_Array;
            if (CAMA_Details.Count > 0 || Cb_Allow_Add_Branch.Checked)
            {
                string Error_Msg = string.Empty, Duplicate_Error = string.Empty;
                if (Branch_CAMS_Details.Count > 0)
                {
                    int Total_Branches = 0, Total_Groups = 0, PRiv_Grp = 0;
                    string Priv_Branch = string.Empty, Branch_List = string.Empty;
                    //foreach (CASESP2Entity Entity in CAMA_Details)
                    //{
                    //    if (Entity.Branch != Priv_Branch)
                    //    {
                    //        Branch_List += Entity.Branch;
                    //        Total_Branches++;
                    //    }
                    //    Priv_Branch = Entity.Branch;
                    //}
                    Total_Branches = 1;
                    Branch_List = BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString().Trim();

                    int CA_Count = 0, MS_Count = 0;
                    bool Grps_Exists_In_Sel_Branch = false; string Dup_Group_List = "";
                    for (int Branch_Pos = 0; Branch_Pos < Total_Branches; Branch_Pos++)
                    {
                        foreach (CASESP2Entity Entity in Branch_CAMS_Details)
                        {
                            if (Entity.Branch == Branch_List.Substring(Branch_Pos, 1))
                            {
                                if (Entity.Curr_Grp != PRiv_Grp)
                                    Total_Groups++;

                                Priv_Branch = Entity.Branch;
                                PRiv_Grp = Entity.Curr_Grp;
                            }
                        }

                        Grp_Valid_Array = new bool[Total_Groups, 2];
                        for (int i = 0; i < Total_Groups; i++)
                        {
                            Grps_Exists_In_Sel_Branch = false;
                            Grp_Valid_Array[i, 0] = Grp_Valid_Array[i, 1] = false;
                            CA_Count = MS_Count = 0;

                            foreach (CASESP2Entity Entity in Branch_CAMS_Details)
                            {
                                if (Entity.Curr_Grp == (i + 1) && Branch_List.Substring(Branch_Pos, 1) == Entity.Branch)
                                {
                                    if (!Dup_Group_List.Contains(Entity.Curr_Grp.ToString()))
                                    {
                                        Duplicate_Error += Get_Depliidate_CAMS_IN_Branch(Entity.Curr_Grp);
                                        Dup_Group_List += Entity.Curr_Grp.ToString() + ",";
                                    }
                                    if (Entity.Type1 == "CA")
                                        CA_Count++;
                                    else
                                        MS_Count++;

                                    Grps_Exists_In_Sel_Branch = true;
                                }
                            }

                            if (Grps_Exists_In_Sel_Branch)
                            {
                                if (CA_Count > 0)
                                    Grp_Valid_Array[i, 0] = true;

                                //Commented by Sudheer on 11/05/2020
                                //else
                                //    //Error_Msg += "Branch-" + Branch_List.Substring(Branch_Pos, 1) + "  Group-" + (i + 1).ToString() + " Should Contain At least One CA \n";
                                //    Error_Msg += "        Group-" + (i + 1).ToString() + " in Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Should Contain At least One CA \n";

                                if (Member_Activity == "N")
                                {
                                    if (MS_Count > 0)
                                        Grp_Valid_Array[i, 1] = true;
                                    //Commented by Sudheer on 11/05/2020

                                    //else
                                    //    //Error_Msg += "Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Group-" + (i + 1).ToString() + " Should Contain At least One MS \n";
                                    //    Error_Msg += "        Group-" + (i + 1).ToString() + " in Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Should Contain At least One MS \n";
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Error_Msg) || !string.IsNullOrEmpty(Duplicate_Error))
                    //MessageBox.Show((!string.IsNullOrEmpty(Error_Msg) ? " Group missmatch List:\n" + Error_Msg + "\n" : "") + (!string.IsNullOrEmpty(Duplicate_Error) ? " Duplicates List:\n" + Duplicate_Error : ""), "CAP Systems");
                    AlertBox.Show((!string.IsNullOrEmpty(Error_Msg) ? Error_Msg + "\n" : "") + (!string.IsNullOrEmpty(Duplicate_Error) ? " Duplicates List:\n" + Duplicate_Error : ""), MessageBoxIcon.Warning);
                else
                {
                    Btn_Validate_Branch.Visible = false;
                    BtnSaveSP.Enabled = true; //BtnValidate.Enabled = true;
                }
                //else
                //{
                //    SP_Header_Rec.Validate = "Y"; SP_Header_Rec.lstcOperator = BaseForm.UserID;

                //    if (Cb_Active.Checked)
                //        SP_Header_Rec.Active = "Y";

                //    if (Cb_Allow_Add_Branch.Checked)
                //        SP_Header_Rec.Allow_Add_Branch = "Y";

                //    if (_model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out Sql_SP_Result_Message))
                //    {
                //        //MessageBox.Show("Service Plan-" + SP_Code + " is Validated And Updated", "CAP Systems"); // Lisa Asked to Comment on 01072013
                //        BtnValidate.Visible = false;
                //    }
                //    else
                //        MessageBox.Show("Unable to Update Service Plan-" + SP_Code + "\n" +
                //                         "Exception : " + Sql_SP_Result_Message, "CAP Systems");
                //}
            }
        }

        private string Get_Depliidate_CAMS_IN_Branch(int Curr_Grp)
        {
            string Error = "", CAMS_Codes_List = "";
            List<CASESP2Entity> Grp_CAMS_Deails = new List<CASESP2Entity>();
            List<CASESP2Entity> Tmp_CAMS_Deails = new List<CASESP2Entity>();
            Grp_CAMS_Deails = Branch_CAMS_Details.FindAll(u => u.Curr_Grp.Equals(Curr_Grp));
            foreach (CASESP2Entity Entity in Grp_CAMS_Deails)
            {
                if (!CAMS_Codes_List.Contains(Entity.CamCd.Trim() + Entity.Type1.Trim()))
                {
                    Tmp_CAMS_Deails = Grp_CAMS_Deails.FindAll(u => (u.CamCd.Equals(Entity.CamCd) && u.Type1.Equals(Entity.Type1)));
                    if (Tmp_CAMS_Deails.Count > 1)
                        Error += "        " + Entity.Type1 + " - ''" + Entity.CAMS_Desc.Trim() + "'' is duplicated in Group-" + Curr_Grp.ToString() + "\n";

                    CAMS_Codes_List += Entity.CamCd.Trim() + Entity.Type1.Trim() + ",";
                }
            }

            return Error;
        }


        private string Get_Depliidate_CAMS_IN_SP(int Curr_Grp, List<CASESP2Entity> CAMS_List)
        {
            string Error = "", CAMS_Codes_List = "";
            List<CASESP2Entity> Grp_CAMS_Deails = new List<CASESP2Entity>();
            List<CASESP2Entity> Tmp_CAMS_Deails = new List<CASESP2Entity>();
            Grp_CAMS_Deails = CAMS_List.FindAll(u => u.Curr_Grp.Equals(Curr_Grp));
            foreach (CASESP2Entity Entity in Grp_CAMS_Deails)
            {
                if (!CAMS_Codes_List.Contains(Entity.CamCd.Trim() + Entity.Type1.Trim()))
                {
                    Tmp_CAMS_Deails = Grp_CAMS_Deails.FindAll(u => (u.CamCd.Equals(Entity.CamCd) && u.Type1.Equals(Entity.Type1)));
                    if (Tmp_CAMS_Deails.Count > 1)
                        Error += "        " + Entity.Type1 + " - ''" + Entity.CAMS_Desc.Trim() + "'' is duplicated in Group-" + Curr_Grp.ToString() + " of Branch- " + Entity.Branch + "\n";

                    CAMS_Codes_List += Entity.CamCd.Trim() + Entity.Type1.Trim() + ",";
                }
            }

            return Error;
        }


        private void BtnValidate_Click(object sender, EventArgs e)
        {
            if (CAMA_Details.Count > 0 || Cb_Allow_Add_Branch.Checked)
                ValiDate_SP();
            else
                AlertBox.Show("Please Select Service/Outcome for this Service Plan to Validate", MessageBoxIcon.Warning);
        }

        private void ADMN0020Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Refresh_ADMN20_Control)
                this.DialogResult = DialogResult.OK;
        }

        public string Get_Current_SP_Code()
        {
            string Curr_SP_Code = New_SP_Code.ToString();

            Curr_SP_Code = "000000".Substring(0, (6 - Curr_SP_Code.Length)) + Curr_SP_Code;
            return Curr_SP_Code;
        }


        public bool Get_Refresh_Control_Status()
        {
            return Refresh_ADMN20_Control;
        }



        private void Initialize_database_Exception_Handling_Parameters()
        {
            SPOut_Err_Procedure = string.Empty;
            SPOut_Err_Number = "00";
            SPOut_Err_Message = Consts.Common.Success;
            SPOut_Err_Line = "0";
        }

        private void contextMenu2_Popup(object sender, EventArgs e)
        {
            if (HierarchyGrid.Rows.Count > 0)
            {
                contextMenu2.MenuItems.Clear();
                MenuItem Menu_L1 = new MenuItem();
                Menu_L1.Text = "Set as Default Hierarchy";
                Menu_L1.Tag = "D";
                contextMenu2.MenuItems.Add(Menu_L1);
            }
        }

        private void HierarchyGrid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string[] Split_Array = new string[2];

            if (objArgs.MenuItem.Tag is string)
            {
                CASESP0Entity Search_Entity = new CASESP0Entity();
                Search_Entity = _model.SPAdminData.Browse_CASESP0(SP_Code, null, null, null, null, null, null, null, null);
                if (Search_Entity != null)
                    SP_Header_Rec = Search_Entity;

                SP_Header_Rec.Default_Prog = HierarchyGrid.CurrentRow.Cells["Hie_Code"].Value.ToString();
                SP_Header_Rec.lstcOperator = BaseForm.UserID;

                if (SP_Header_Rec.Allow_Add_Branch == "Y")
                    SP_Header_Rec.Allow_Add_Branch = "Y";
                else
                    SP_Header_Rec.Allow_Add_Branch = "N";


                Row_Shifted = false;
                if (_model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out Sql_SP_Result_Message))
                {
                    AlertBox.Show("Service Plan Default Hierarchy Updated Successfully", MessageBoxIcon.Warning);
                    Fill_SP_Hierarchies_Grid();
                }
            }
        }

        private void Lbl_Timer_Tick(object sender, EventArgs e)
        {
            //Lbl_Saved.Visible = false;
        }

        private void Btn_Validate_Branch_Click(object sender, EventArgs e)
        {
            ValiDate_Branch();
        }

        private void chkbBulk_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbBulk.Checked)
            {
                SPDetailsGrid.Columns["gvCountType"].Visible = true;
                SPDetailsGrid.Columns["gvCountType"].ShowInVisibilityMenu = true;
                SPDetailsGrid.Columns["SpDesc"].Width = 370;
            }
            else
            {
                SPDetailsGrid.Columns["gvCountType"].Visible = false;
                SPDetailsGrid.Columns["gvCountType"].ShowInVisibilityMenu = false;
                SPDetailsGrid.Columns["SpDesc"].Width = 546;//490;
            }
        }

        private void ADMN0020Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tlHelp")
                Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 1, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            else if (e.Tool.Name == "tlExcel")
            {
                On_SaveExcelForm_Closed();
            }
        }

        private void PicExcel_Click(object sender, EventArgs e)
        {
            On_SaveExcelForm_Closed();
        }

        string Agency = null;
        string Random_Filename = null; string PdfName = null;
        private void On_SaveExcelForm_Closed()
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "SPREPAPP_" + "Report";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }
            try
            {
                string Tmpstr = PdfName + ".xls";//".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xls";

            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);

            Worksheet sheet = book.Worksheets.Add("Data");
            sheet.Table.DefaultRowHeight = 14.25F;

            DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(SP_Code.Trim(), null, null, null, null, null, null, null, null);
            DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];

            sheet.Table.Columns.Add(200);
            sheet.Table.Columns.Add(200);
            sheet.Table.Columns.Add(250);
            sheet.Table.Columns.Add(52);
            sheet.Table.Columns.Add(75);
            sheet.Table.Columns.Add(75);

            DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(SP_Code.Trim(), null, null, null);
            DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];

            List<CAMASTEntity> CAList;
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            List<MSMASTEntity> MSList;
            MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

            bool First = true; string MS_Type = string.Empty;
            string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null; ;
            if (dtSP_CaseSP2.Rows.Count > 0)
            {
                DataView dv = dtSP_CaseSP2.DefaultView;
                dv.Sort = "sp2_branch DESC";
                dtSP_CaseSP2 = dv.ToTable();

                WorksheetRow Row0 = sheet.Table.Rows.Add();

                WorksheetCell cell;
                cell = Row0.Cells.Add("SPM", DataType.String, "s94");
                cell = Row0.Cells.Add("Branch", DataType.String, "s94");
                cell = Row0.Cells.Add("Description", DataType.String, "s94");
                cell = Row0.Cells.Add("Type", DataType.String, "s94");
                cell = Row0.Cells.Add("Add Date", DataType.String, "s94");
                cell = Row0.Cells.Add("Change Date", DataType.String, "s94");

                int Count = 0; bool CAMS = true, SPNum = true; ;
                foreach (DataRow dr in dtSP_CaseSP2.Rows)
                {
                    SP_Plan_desc = drSP_Services["sp0_description"].ToString().Trim();
                    Branch = dr["sp2_branch"].ToString().Trim();
                    if (Branch != Priv_Branch)
                    {
                        Row0 = sheet.Table.Rows.Add();
                        ////cb.EndText();
                        //document.Add(table);
                        //table.DeleteBodyRows();

                        ////table.DeleteRow(1);

                        //document.NewPage();
                        if (Branch != "P")
                            First = false;
                        //cb.BeginText();
                        string Service_desc = drSP_Services["sp0_pbranch_desc"].ToString();
                        if (!First)
                        {
                            if (Branch.Trim() == drSP_Services["sp0_branch1_code"].ToString().Trim())
                                Service_desc = drSP_Services["sp0_branch1_desc"].ToString();
                            else if (Branch.Trim() == drSP_Services["sp0_branch2_code"].ToString().Trim())
                                Service_desc = drSP_Services["sp0_branch2_desc"].ToString();
                            else if (Branch.Trim() == drSP_Services["sp0_branch3_code"].ToString().Trim())
                                Service_desc = drSP_Services["sp0_branch3_desc"].ToString();
                            else if (Branch.Trim() == drSP_Services["sp0_branch4_code"].ToString().Trim())
                                Service_desc = drSP_Services["sp0_branch4_desc"].ToString();
                            else if (Branch.Trim() == drSP_Services["sp0_branch5_code"].ToString().Trim())
                                Service_desc = drSP_Services["sp0_branch5_desc"].ToString();
                            else if (Branch.Trim() == "9")
                                Service_desc = "Additional Branch";

                            Row0 = sheet.Table.Rows.Add();

                        }
                        CAMS = true;
                        //cell = Row0.Cells.Add("Service :" + SP_Plan_desc.Trim(), DataType.String, "s95B");
                        //cell.MergeAcross = 1;

                        ////cell = Row0.Cells.Add("Service :" + SP_Plan_desc.Trim(), DataType.String, "s95B");

                        //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_add"].ToString().Trim()), DataType.String, "s95B");
                        //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_lstc"].ToString().Trim()), DataType.String, "s95B");

                        //PdfPCell Sp_Plan = new PdfPCell(new Phrase("Service :" + SP_Plan_desc.Trim(), fc1));
                        //Sp_Plan.HorizontalAlignment = Element.ALIGN_CENTER;
                        //Sp_Plan.Colspan = 2;
                        //Sp_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //table.AddCell(Sp_Plan);

                        //Row0 = sheet.Table.Rows.Add();
                        //cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                        //cell.MergeAcross = 1;

                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");

                        if (SPNum)
                            cell = Row0.Cells.Add("Service :" + drSP_Services["sp0_description"].ToString().Trim(), DataType.String, "s96");
                        else cell = Row0.Cells.Add("", DataType.String, "s96");
                        //Row0 = sheet.Table.Rows.Add();
                        if (!CAMS) Row0.Cells.Add("", DataType.String, "s96");
                        else cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");

                        //PdfPCell ServiceDesc = new PdfPCell(new Phrase("Branch :" + Service_desc.Trim(), TblFontBold));
                        //ServiceDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                        //ServiceDesc.Colspan = 2;
                        //ServiceDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //table.AddCell(ServiceDesc);

                        //string[] col = { "Type", "Description" };
                        //for (int i = 0; i < col.Length; ++i)
                        //{
                        //    PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //    table.AddCell(cell);
                        //}
                        Priv_Branch = Branch; SPNum = false;
                        First = false;
                    }
                    string CAMSType = dr["sp2_type"].ToString();

                    if (CAMSType == "CA")
                    {
                        bool Active = true;
                        foreach (CAMASTEntity drCAMAST in CAList)
                        {
                            if (drCAMAST.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                            {
                                CAMSDesc = drCAMAST.Desc.ToString().Trim();
                                if (drCAMAST.Active == "False") Active = false;
                                break;
                            }
                            else
                                CAMSDesc = "";
                        }
                        if (!CAMS)
                        {
                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                        }

                        if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                        {
                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95R");
                            if (CAMSType.Trim() == "CA")
                                CAMSType = "Service";
                            cell = Row0.Cells.Add(CAMSType.Trim(), DataType.String, "s95R");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95R");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95R");

                        }
                        else
                        {
                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95");
                            if (CAMSType.Trim() == "CA")
                                CAMSType = "Service";
                            cell = Row0.Cells.Add(CAMSType.Trim(), DataType.String, "s95");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95");
                        }



                        //PdfPCell RowType = new PdfPCell(new Phrase(CAMSType.Trim(), TableFont));
                        //RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                        //RowType.Border = iTextSharp.text.Rectangle.BOX;
                        //table.AddCell(RowType);

                        //PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                        //RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                        //RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                        //table.AddCell(RowDesc);
                        CAMS = false;
                    }
                    else
                    {
                        string Type_Desc = string.Empty; bool Active = true;
                        foreach (MSMASTEntity drMSMast in MSList)
                        {
                            if (drMSMast.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                            {
                                CAMSDesc = drMSMast.Desc.ToString().Trim();
                                MS_Type = drMSMast.Type1.ToString();
                                if (drMSMast.Active == "False") Active = false;
                                //if (MS_Type == "M")
                                //    Type_Desc = "Milestone";
                                //else 
                                Type_Desc = "Outcome";
                                break;
                            }
                            else
                                CAMSDesc = "";
                        }

                        if (!CAMS)
                        {
                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                        }

                        if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                        {
                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95R");
                            cell = Row0.Cells.Add(Type_Desc, DataType.String, "s95R");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95R");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95R");

                        }
                        else
                        {
                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95");
                            cell = Row0.Cells.Add(Type_Desc, DataType.String, "s95");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95");
                        }

                        //PdfPCell RowType = new PdfPCell(new Phrase(Type_Desc, TblFontBold));
                        //RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                        //RowType.Border = iTextSharp.text.Rectangle.BOX;
                        //table.AddCell(RowType);

                        //PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TblFontBold));
                        //RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                        //RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                        //table.AddCell(RowDesc);
                        CAMS = false;
                    }
                }
                Count++;

            }
            else
            {
                WorksheetRow Row0 = sheet.Table.Rows.Add();

                WorksheetCell cell;
                cell = Row0.Cells.Add("SPM", DataType.String, "s94");
                cell = Row0.Cells.Add("Branch", DataType.String, "s94");
                cell = Row0.Cells.Add("Description", DataType.String, "s94");
                cell = Row0.Cells.Add("Type", DataType.String, "s94");
                cell = Row0.Cells.Add("Add Date", DataType.String, "s94");
                cell = Row0.Cells.Add("Change Date", DataType.String, "s94");

                Row0 = sheet.Table.Rows.Add();
                cell = Row0.Cells.Add("Service :" + drSP_Services["sp0_description"].ToString().Trim(), DataType.String, "s96");
                string Service_desc = drSP_Services["sp0_pbranch_desc"].ToString();
                cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");

                if (!string.IsNullOrEmpty(drSP_Services["sp0_branch1_code"].ToString().Trim()))
                {
                    Service_desc = drSP_Services["sp0_branch1_desc"].ToString();
                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                    {
                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                    }
                }
                if (!string.IsNullOrEmpty(drSP_Services["sp0_branch2_code"].ToString().Trim()))
                {
                    Service_desc = drSP_Services["sp0_branch2_desc"].ToString();
                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                    {
                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                    }
                }
                if (!string.IsNullOrEmpty(drSP_Services["sp0_branch3_code"].ToString().Trim()))
                {
                    Service_desc = drSP_Services["sp0_branch3_desc"].ToString();
                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                    {
                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                    }
                }
                if (!string.IsNullOrEmpty(drSP_Services["sp0_branch4_code"].ToString().Trim()))
                {
                    Service_desc = drSP_Services["sp0_branch4_desc"].ToString();
                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                    {
                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                    }
                }
                if (!string.IsNullOrEmpty(drSP_Services["sp0_branch5_code"].ToString().Trim()))
                {
                    Service_desc = drSP_Services["sp0_branch5_desc"].ToString();
                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                    {
                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                    }
                }
                //if (Branch.Trim() == "9")
                //{
                //    Service_desc = "Additional Branch";
                //    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                //    {
                //        Row0 = sheet.Table.Rows.Add();
                //        cell = Row0.Cells.Add("", DataType.String, "s96");
                //        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                //    }
                //}
            }
            //document.Add(Header);
            //if (table.Rows.Count > 0)
            //    document.Add(table);
            //else
            //{
            //    cb.BeginText();
            //    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 15);
            //    cb.ShowTextAligned(Element.ALIGN_CENTER, "Critical Activities and MileStones are not Defined for this Service Plan", 300, 600, 0);
            //    cb.EndText();
            //}
            //document.Close();
            //fs.Close();
            //fs.Dispose();

            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            //FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //downloadGateway.Filename = "SPREPAPPForm_Report.xls";

            //// downloadGateway.Version = file.Version;

            //downloadGateway.SetContentType(DownloadContentType.OctetStream);

            //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

            FileInfo fiDownload = new FileInfo(PdfName);
            /// Need to check for file exists, is local file, is allow to read, etc...
            string name = fiDownload.Name;
            using (FileStream fileStream = fiDownload.OpenRead())
            {
                Application.Download(fileStream, name);
            }

            //On_Delete_PDF_File();

            //if (LookupDataAccess.FriendlyName().Contains("2012"))
            //{
            //    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            //    objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}
            //else
            //{
            //    FrmViewer objfrm = new FrmViewer(PdfName);
            //    objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.NumberFormat = "0%";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Arial";
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s21.NumberFormat = "0%";
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Interior.Color = "#D8D8D8";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Interior.Color = "#D8D8D8";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Interior.Color = "#D8D8D8";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.NumberFormat = "0%";
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Interior.Color = "#D8D8D8";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Interior.Color = "#D8D8D8";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s28.NumberFormat = "0%";
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.NumberFormat = "0%";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s69.NumberFormat = "0%";
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.NumberFormat = "0%";
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s72.NumberFormat = "0%";
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.NumberFormat = "0%";
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.NumberFormat = "0%";
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.NumberFormat = "0%";
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.NumberFormat = "0%";
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.NumberFormat = "0%";
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Arial";
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.Bold = true;
            s82.Font.FontName = "Arial";
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.NumberFormat = "0%";
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Arial";
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s84.NumberFormat = "0%";
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Arial";
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.NumberFormat = "0%";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Arial";
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s87.NumberFormat = "0%";
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s90.NumberFormat = "0%";
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Font.Bold = true;
            s92.Font.Italic = true;
            s92.Font.FontName = "Arial";
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s92.NumberFormat = "0%";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.Italic = true;
            s93.Font.FontName = "Arial";
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Arial";
            s94.Font.Color = "#000000";
            s94.Interior.Color = "#B0C4DE";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Top;
            s94.Alignment.WrapText = true;
            s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s95B
            // -----------------------------------------------
            WorksheetStyle s95B = styles.Add("s95B");
            s95B.Font.FontName = "Arial";
            s95B.Font.Bold = true;
            s95B.Font.Color = "#0000FF";
            s95B.Interior.Color = "#FFFFFF";
            s95B.Interior.Pattern = StyleInteriorPattern.Solid;
            s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95B.Alignment.WrapText = true;
            s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //  s95R
            // -----------------------------------------------
            WorksheetStyle s95R = styles.Add("s95R");
            s95R.Font.FontName = "Arial";
            //s95R.Font.Bold = true;
            s95R.Font.Color = "#FF0000";
            s95R.Interior.Color = "#FFFFFF";
            s95R.Interior.Pattern = StyleInteriorPattern.Solid;
            s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95R.Alignment.WrapText = true;
            s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Font.Bold = true;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.Bold = true;
            s97.Font.FontName = "Arial";
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.NumberFormat = "0%";
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Arial";
            s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Arial";
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.NumberFormat = "0%";
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.NumberFormat = "0%";
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.NumberFormat = "0%";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Arial";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.NumberFormat = "0%";
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.NumberFormat = "0%";
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.NumberFormat = "0%";
        }

        ///// Murali added Hierchy wise validation on 05/21/2021
        //bool boolServiceCateValidateHie()
        //{
        //    bool boolHieValidate = true;

        //    if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
        //    {
        //        bool boolok = false;
        //        if (HierarchyGrid.Rows.Count > 0)
        //        {
        //            List<ListItem> _listcat = new List<ListItem>();
        //            string Agy = "**", Dept = "**", Prog = "**";
        //            string strServiceCatcode = string.Empty;
        //            bool boolAllHie = false;
        //            foreach (DataGridViewRow dr in HierarchyGrid.Rows)
        //            {
        //                if (dr.Cells["Hie_Code"].Value.ToString().Contains("**"))
        //                {
        //                    boolAllHie = true;
        //                    break;
        //                }
        //            }
        //            if (boolAllHie)
        //            {
        //                foreach (DataGridViewRow dr in HierarchyGrid.Rows)
        //                {
        //                    string Hierarchy = dr.Cells["Hie_Code"].Value.ToString();

        //                    Agy = Hierarchy.Substring(0, 2).Trim();
        //                    Dept = Hierarchy.Substring(2, 2);
        //                    Prog = Hierarchy.Substring(4, 2);



        //                    if (Agy != "**" && Dept != "**" && Prog != "**")
        //                    {
        //                        ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(Agy, Dept, Prog);
        //                        if (programEntity != null)
        //                        {
        //                            _listcat.Add(new ListItem(programEntity.DepSerpostPAYCAT));
        //                        }
        //                    }
        //                }
        //                if (_listcat.FindAll(u => u.Text.Trim() == "").Count == _listcat.Count)
        //                {
        //                    boolok = true;
        //                }
        //            }
        //            else
        //            {
        //                foreach (DataGridViewRow dr in HierarchyGrid.Rows)
        //                {
        //                    string Hierarchy = dr.Cells["Hie_Code"].Value.ToString();

        //                    Agy = Hierarchy.Substring(0, 2).Trim();
        //                    Dept = Hierarchy.Substring(2, 2);
        //                    Prog = Hierarchy.Substring(4, 2);

        //                    ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(Agy, Dept, Prog);
        //                    if (programEntity != null)
        //                    {
        //                        _listcat.Add(new ListItem(programEntity.DepSerpostPAYCAT));
        //                    }

        //                }

        //                if (_listcat.FindAll(u => u.Text == "01").Count == SelectedHierarchies.Count)
        //                {
        //                    boolok = true;
        //                }
        //                if (_listcat.FindAll(u => u.Text == "02").Count == SelectedHierarchies.Count)
        //                {
        //                    boolok = true;
        //                }
        //                if (_listcat.FindAll(u => u.Text.Trim() == "").Count == SelectedHierarchies.Count)
        //                {
        //                    boolok = true;
        //                }
        //            }
        //            if (boolok)
        //            {

        //                boolHieValidate = true;
        //            }
        //            else
        //            {
        //                boolHieValidate = false;
        //                CommonFunctions.MessageBoxDisplay("All Selected Hierarchies are not in Same Category");
        //            }
        //        }
        //    }
        //    return boolHieValidate;
        //}


    }
}