#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class FieldControlForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion


        public FieldControlForm(BaseForm baseForm, string mode, string Scrcode, string Hie, string strPrivilegesName, int seltab_index)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Mode = mode;
            Seltab_Tndex = seltab_index;
            propFormType = string.Empty;

            switch (Seltab_Tndex)
            {
                case 0: tabcontrol1.TabPages[1].Hidden = true; break;
                case 1:
                    tabcontrol1.TabPages[0].Hidden = true;
                    TxtHierarchy.Visible = TxtHieDeSC.Visible = PBHierarchy.Visible = false;
                    break;
            }

            if (mode.Equals("Edit"))
            {
                if (Hie != "Custom_Edit")
                    ScrHierarchy = Hie;
                else
                {
                    tabcontrol1.TabPages[0].Hidden = true;
                    Seltab_Tndex = 1;
                }
            }

            Get_Cntl_From_HIE = "";
            if (Mode == "Add" && Hie != null)
            {
                if (Hie != "Custom_Edit")
                    Get_Cntl_From_HIE = Hie;
            }

            ScrCode = Scrcode;
            UserName = BaseForm.UserID;
            // lblHeader.Text = strPrivilegesName;
            _model = new CaptainModel();


            tabcontrol1.SelectedIndex = Seltab_Tndex;

            //Fillcmbsearchby();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            txtSeq.Validator = TextBoxValidation.IntegerValidator;

            TxtEqual.Validator = TextBoxValidation.FloatValidator;
            TxtGreat.Validator = TextBoxValidation.FloatValidator;
            TxtLess.Validator = TextBoxValidation.FloatValidator;
            TxtCode.Validator = TextBoxValidation.IntegerValidator;
            //txtSeq.Validator = TextBoxValidation.IntegerMaskValidator;
            //SetDataGridsColumns();

            FillAllCombos();
            SetComboBoxValue(CmbScreen, ScrCode);
            if (((ListItem)CmbScreen.SelectedItem).ValueDisplayCode == "Y")
                FillCustomGrid();
            else
                tabcontrol1.TabPages[1].Hidden = true;
            //FillCustomGrid();


            TmpGrid.Rows.Clear();

            if (Hie != "Custom_Edit")
                AlignSplitContainerControls();

            TxtCode.Enabled = false;

            if (Mode.Equals("Edit"))
                this.Text = "Field Control Maintenance"/*ScrCode*/ + " - Edit";
            else
            {
                this.Text = "Field Control Maintenance"/*ScrCode*/ + " - Add";

                TxtCode.Text = "New";
                TxtCode.Focus();
                CbActive.Checked = true;
                FillHierarchyGrid();

                if (Seltab_Tndex == 0)
                    PBHierarchy.Visible = true;
            }

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(PBHierarchy, "Hierarchy Selection");
            //tooltip.SetToolTip(Hepl, "Help");
            if (ScrCode == "CASE2003")
            {
                CbShareAll.Enabled = true;
                CbShareLeft.Enabled = true;
                //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "COI" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "PROACT")
                //{
                CbShareAll.Enabled = false;
                CbShareLeft.Enabled = false;
                Shared.ReadOnly = true;
                Share.ReadOnly = true;
                int TmpCount = 0;
                foreach (DataGridViewRow dr in FLDCNTLGrid.Rows)
                {
                    dr.Cells["Shared"].Value = false;
                    ChgFldcntlArr[TmpCount, 2] = "N";
                    TmpCount++;
                }
                // }
            }

        }

        public FieldControlForm(BaseForm baseForm, string mode, string Scrcode, string Hie, string strPrivilegesName, int seltab_index, string strFormType)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Mode = mode;
            Seltab_Tndex = seltab_index;
            propFormType = "PROGRAMDEFINATION";

            tabcontrol1.TabPages[1].Hidden = true;
            TxtHierarchy.Visible = TxtHieDeSC.Visible = PBHierarchy.Visible = false;



            ScrCode = Scrcode;
            UserName = BaseForm.UserID;
            // lblHeader.Text = strPrivilegesName;
            _model = new CaptainModel();

            this.Width = 430;
            // tabcontrol1.SelectedIndex = Seltab_Tndex;

            //Fillcmbsearchby();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            //txtSeq.Validator = TextBoxValidation.IntegerMaskValidator;
            //TxtEqual.Validator = TextBoxValidation.FloatValidator;
            //TxtGreat.Validator = TextBoxValidation.FloatValidator;
            //TxtLess.Validator = TextBoxValidation.FloatValidator;
            //TxtCode.Validator = TextBoxValidation.IntegerValidator;
            //txtSeq.Validator = TextBoxValidation.IntegerMaskValidator;


        }


        private void AlignSplitContainerControls()
        {
            AddColumnsFLDGrids();
            if (Mode.Equals("Edit"))
            {
                TxtHieDeSC.Text = "All Hierarchies";
                TxtHierarchy.Text = ScrHierarchy.Substring(0, 2) + '-' + ScrHierarchy.Substring(2, 2) + '-' + ScrHierarchy.Substring(4, 2);
                if (ScrHierarchy != "******")
                {
                    DataSet Prog = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(ScrHierarchy.Substring(0, 2), ScrHierarchy.Substring(2, 2), ScrHierarchy.Substring(4, 2));
                    if (Prog.Tables[0].Rows.Count > 0)
                        TxtHieDeSC.Text = "   " + (Prog.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                    else
                        TxtHieDeSC.Text = "Description Not Defined";
                }

                CbEnableAll.Text = "Keep Original";
                this.dataGridViewTextBoxColumn1.Width = 250;//225;
                this.dataGridViewTextBoxColumn2.Width = 270;//225;
                this.dataGridViewTextBoxColumn1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                this.dataGridViewTextBoxColumn2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                this.CbFldValidation.Location = new Point(205, 3);
                this.CbEnableAll.Location = new Point(323, 3);
                this.CbShareAll.Location = new Point(436, 3);
                this.dataGridViewTextBoxColumn2.Visible = false;
                RightPanel.Visible = true;
                this.Size = new Size(1085, 500);
                this.LeftPanel.Size = new Size(535, 344);
                this.FLDCNTLGrid.Size = new Size(535, 344);
                this.pnlBtnUpdate.Padding = new Padding(5, 5, 545, 5);
                // this.BtnUpdate.Location = new Point(345,5);//(224, 399);
                FillFLDCNTLGrid_Edit(ScrHierarchy);
                FillRemainingGrid();
                PbFLDRight.Visible = true;
            }
            else
            {
                TxtHierarchy.Clear();
                TxtHieDeSC.Clear();
                RightPanel.Visible = false;
                PbFLDLeft.Visible = false;
                LblACtFields.Visible = false;
                PbFLDRight.Visible = false;
                this.dataGridViewTextBoxColumn1.Width = 610;
                this.dataGridViewTextBoxColumn1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                this.dataGridViewTextBoxColumn2.Visible = true;
                this.dataGridViewTextBoxColumn2.Width = 146;
                this.Size = new Size(1085, 500);
                this.LeftPanel.Size = new Size(1085, 500);//(900, 427);
                this.FLDCNTLGrid.Size = new Size(1085, 500);
                this.pnlBtnUpdate.Padding = new Padding(5, 5, 15, 5);
                // this.BtnUpdate.Location = new System.Drawing.Point(670, 399);
                this.CbFldValidation.Location = new Point(740, 3);//(608, 7);
                this.CbEnableAll.Location = new Point(860, 3);//(710, 7);
                this.CbShareAll.Location = new Point(985, 3);//(816, 7);
                FillFLDCNTLGrid_Add();
            }
        }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string ScrCode { get; set; }

        public string SelectColName { get; set; }

        public string SelectChildCode { get; set; }

        public bool IsSaveValid { get; set; }

        public string ScrHierarchy { get; set; }

        public string Get_Cntl_From_HIE { get; set; }

        public string UserName { get; set; }

        public int Seltab_Tndex { get; set; }

        public string propFormType { get; set; }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                TmpGrid.Rows.Clear();
                return _selectedHierarchies = (from c in TmpGrid.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }


        bool Tab1_Loading_Complete = false;
        bool Tab2_Loading_Complete = false;
        bool CustResp_lod_complete = false;
        bool STAT_Controls_exists = false;
        bool CUST_Controls_exists = false;
        DataGridViewRow PrivRow;
        int Cust_PrivRow_Index = 0;
        string OperTab = "Static";
        string RespType = null;
        int TotHieRecCnt = 0;
        string[,] ChgFldcntlArr = null;
        string[,] ChgRemFldcntlArr = null;
        string Org_FldValidation = null;
        List<FLDDESCHieEntity> OrgFldcntHieEntity = new List<FLDDESCHieEntity>();
        List<FLDCNTLHIEAddEntity> FldcntAddEntity = new List<FLDCNTLHIEAddEntity>();
        List<CustRespEntity> OrgCustResp = new List<CustRespEntity>();

        List<FLDSCRSEntity> FLDSCRS_List = new List<FLDSCRSEntity>();
        private void FillAllCombos()
        {
            CmbScreen.Items.Clear();
            List<ListItem> listItem2 = new List<ListItem>();
            //listItem.Add(new ListItem("Form Title                                    -  SCR-CODE", SCR-CODE, Empty, Custom_Filedls_Flag));

            FLDSCRSEntity Search_Entity = new FLDSCRSEntity(true);
            Search_Entity.Called_By = "CASE0008";
            FLDSCRS_List = _model.FieldControls.Browse_FLDSCRS(Search_Entity);
            string Tmp_Desc = string.Empty;

            foreach (FLDSCRSEntity Entity in FLDSCRS_List)
            {
                if ((Entity.Scr_Code == "CASE2001" && Entity.Scr_Sub_Code == "00") || Entity.Scr_Code != "CASE2001")
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-30}", Entity.Scr_Desc) + String.Format("{0,8}", " - " + Entity.Scr_Code);
                    CmbScreen.Items.Add(new ListItem(Tmp_Desc, Entity.Scr_Code, Entity.Scr_Desc, Entity.Cust_Ques_SW));
                }
            }


            //listItem2.Add(new ListItem("Client Intake                                -  CASE2001", "CASE2001", " ", "Y"));
            //listItem2.Add(new ListItem("Income Entry                               -  CASINCOM", "CASINCOM", " ", "N"));
            //listItem2.Add(new ListItem("Income Verification                      -  CASE2003", "CASE2003", " ", "N"));
            //listItem2.Add(new ListItem("Contact Posting                           -  CASE0006", "CASE0061", " ", "Y"));
            //listItem2.Add(new ListItem("Critical Activity Posting                -  CASE0006", "CASE0062", " ", "Y"));
            //listItem2.Add(new ListItem("Milestone Posting                             -  CASE0006", "CASE0063", " ", "N"));
            //listItem2.Add(new ListItem("Medical/Emergency                          -  CASE2330", "CASE2330", "Medical/Emergency", "N"));
            //listItem2.Add(new ListItem("Track Master Maintenance               -  HSS00133", "HSS00133", " ", "Y"));
            //CmbScreen.Items.AddRange(listItem2.ToArray());

            CmbType.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            //listItem.Add(new ListItem("Check Box", "C"));
            listItem.Add(new ListItem("Date", "T"));
            listItem.Add(new ListItem("Drop Down", "D"));
            listItem.Add(new ListItem("Numeric", "N"));
            listItem.Add(new ListItem("Text", "X"));
            CmbType.Items.AddRange(listItem.ToArray());

            CmbAccess.Items.Clear();
            List<ListItem> listItem1 = new List<ListItem>();
            listItem1.Add(new ListItem("Select One", "0"));
            listItem1.Add(new ListItem("All Members", "*"));
            listItem1.Add(new ListItem("Applicants", "A"));
            listItem1.Add(new ListItem("Household", "H"));
            CmbAccess.Items.AddRange(listItem1.ToArray());
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void FillHierarchyGrid()
        {
            CaptainModel model = new CaptainModel();
            List<FLDCNTLEntity> Cust = model.FieldControls.GetFLDCNTLByScrCode(ScrCode, null);
            if (Cust.Count > 0)
            {
                HierGrid.Rows.Clear();
                int rowIndex = 0;
                foreach (FLDCNTLEntity Entity in Cust)
                {
                    string TmpHieDesc = "All Hierarchies";
                    string TmpHie = null;
                    if (Entity.ScrHie != "******")
                    {
                        DataSet Prog = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Entity.ScrHie.Substring(0, 2), Entity.ScrHie.Substring(2, 2), Entity.ScrHie.Substring(4, 2));
                        if (Prog.Tables[0].Rows.Count > 0)
                            TmpHieDesc = (Prog.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        else
                            TmpHieDesc = "Description Not Defined";

                        if (Get_Cntl_From_HIE == Entity.ScrHie)
                            Copy_From_Hie = TmpHieDesc;
                    }
                    rowIndex = HierGrid.Rows.Add(Entity.ScrHie, TmpHieDesc, Entity.ScrHie);
                    //TxtHierarchy.Text = Entity.ScrHie.Substring(0, 2) + '-' + Entity.ScrHie.Substring(2, 2) +  '-'  + Entity.ScrHie.Substring(4, 2);
                    //TxtHieDeSC.Text = TmpHieDesc;
                }
                HierGrid.Rows[rowIndex].Tag = 0;
                Tab1_Loading_Complete = true;

                //FillFLDCNTLGrid_Edit(GetSelectedHierarchy());
            }
        }

        private string GetSelectedHierarchy()
        {
            string RecordKey = null;
            if (HierGrid != null)
            {
                try
                {
                    RecordKey = HierGrid.CurrentRow.Cells["HieKey"].Value.ToString();
                }
                catch (Exception ex) { }
            }
            return RecordKey;
        }


        private void FillFLDCNTLGrid_Edit(string Hierarchy)
        {
            GetScrHieValidStatus();
            FLDCNTLGrid.Rows.Clear();
            CaptainModel model = new CaptainModel();
            List<FLDDESCHieEntity> Cust = model.FieldControls.GetStatCustDescByScrCodeHIE("CASE0008", ScrCode, Hierarchy);
            ChgFldcntlArr = new string[Cust.Count + 1, 4];
            if (Cust.Count > 0)
            {
                int rowIndex = 0;
                int RowCount = 0;
                bool Enabled, Required, Shared;
                OrgFldcntHieEntity = Cust;
                foreach (FLDDESCHieEntity Entity in Cust)
                {
                    Enabled = Required = Shared = false;
                    if (Entity.Enab == "Y" || Entity.Enab == "y")
                        Enabled = true;
                    if (Entity.Req == "Y" || Entity.Req == "y")
                        Required = true;
                    if (Entity.Shared == "Y" || Entity.Shared == "y")
                        Shared = true;

                    //if (Mode == "Add")
                    //    Enabled = Required = Shared = false;

                    ChgFldcntlArr[RowCount, 0] = Entity.Enab;
                    ChgFldcntlArr[RowCount, 1] = Entity.Req;
                    ChgFldcntlArr[RowCount, 2] = Entity.Shared;
                    ChgFldcntlArr[RowCount, 3] = Entity.FldCode;

                    rowIndex = FLDCNTLGrid.Rows.Add(Entity.FldDesc, Entity.SubScr, Enabled, Required, Shared, Entity.FldCode);
                    if (Entity.FldCode.Substring(1, 1) == "A" || Entity.FldCode.Substring(1, 1) == "a")
                    {
                        FLDCNTLGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  // Color.Red;
                        FLDCNTLGrid.Rows[rowIndex].Cells["Enabled"].ReadOnly = true;
                        FLDCNTLGrid.Rows[rowIndex].Cells["Required"].ReadOnly = true;
                    }

                    RowCount++;
                    if (Entity.FldCode.Substring(0, 1) == "C" || Entity.FldCode.Substring(0, 1) == "c")
                        FLDCNTLGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                }
                if (RowCount > 0)
                {
                    FLDCNTLGrid.Rows[rowIndex].Tag = 0;
                    CbEnableAll.Visible = true;
                    CbShareAll.Visible = true;
                    STAT_Controls_exists = true;


              


                    //foreach (DataRow dr in FLDCNTLGrid.Rows)
                    //{
                    //    if (FLDCNTLGrid.CurrentRow.Cells["Gd1Code"].Value.ToString().Substring(1, 1) == "A")
                    //        FLDCNTLGrid.CurrentRow.Cells["Gd1Code"].ReadOnly = true;
                    //}
                }
                TotHieRecCnt = RowCount;
                //if (RowCount > 50)
                //    this.FLDCNTLGrid.Size = new System.Drawing.Size(434, 366);
                //else
                //    this.FLDCNTLGrid.Size = new System.Drawing.Size(417, 366);
            }
            FLDCNTLGrid.Columns["Gd1Code"].DisplayIndex = 0;
        }

        private void GetScrHieValidStatus()
        {
            //CaptainModel model = new CaptainModel();
            List<FLDCNTLEntity> Cust = _model.FieldControls.GetFLDCNTLByScrCode(ScrCode, ScrHierarchy);
            if (Cust.Count > 0)
            {
                FLDCNTLEntity Entity = new FLDCNTLEntity();
                Entity = Cust[0];
                Org_FldValidation = Entity.Valid;
                if (Entity.Valid == "Y")
                    CbFldValidation.Checked = true;
            }
        }


        private void FillFLDCNTLGrid_Add()
        {

            CmbType.SelectedIndex = 0;
            CmbAccess.SelectedIndex = 0;
            FLDCNTLGrid.Rows.Clear();
            STAT_Controls_exists = false;
            CaptainModel model = new CaptainModel();
            List<FLDCNTLHIEAddEntity> Cust = model.FieldControls.GetStatCustDescByScrCode("CASE0008", ScrCode);
            ChgFldcntlArr = new string[Cust.Count + 1, 4];
            if (Cust.Count > 0)
            {
                int rowIndex = 0;
                int RowCount = 0;
                FldcntAddEntity = Cust;
                foreach (FLDCNTLHIEAddEntity Entity in Cust)
                {
                    Entity.ScrCode = ScrCode;
                    Entity.Enab = Entity.Req = Entity.Shared = "N";

                    ChgFldcntlArr[RowCount, 0] = ChgFldcntlArr[RowCount, 1] = ChgFldcntlArr[RowCount, 2] = "N";
                    ChgFldcntlArr[RowCount, 3] = Entity.FldCode;

                    rowIndex = FLDCNTLGrid.Rows.Add(Entity.FldDesc.Trim(), Entity.SubScr.Trim(), false, false, false, Entity.FldCode);
                    if (Entity.FldCode.Substring(1, 1) == "A" || Entity.FldCode.Substring(1, 1) == "a")
                    {
                        FLDCNTLGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  //Color.Red;
                        FLDCNTLGrid.Rows[rowIndex].Cells["Enabled"].ReadOnly = true;
                        FLDCNTLGrid.Rows[rowIndex].Cells["Required"].ReadOnly = true;
                        FLDCNTLGrid.Rows[rowIndex].Cells["Enabled"].Value = true;
                        FLDCNTLGrid.Rows[rowIndex].Cells["Required"].Value = true;
                        ChgFldcntlArr[RowCount, 0] = ChgFldcntlArr[RowCount, 1] = "Y";
                    }


                    if (Entity.FldCode.Substring(0, 1) == "C" || Entity.FldCode.Substring(0, 1) == "c")
                        FLDCNTLGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                    RowCount++;
                }
                if (RowCount > 0)
                {
                    FLDCNTLGrid.Rows[rowIndex].Tag = 0;
                    CbEnableAll.Visible = true;
                    CbShareAll.Visible = true;
                    STAT_Controls_exists = true;

                   
                }
                TotHieRecCnt = RowCount;
           
                ////if (RowCount > 50)
                this.FLDCNTLGrid.Size = new System.Drawing.Size(885, 368);
                //else
                //    this.FLDCNTLGrid.Size = new System.Drawing.Size(880, 366);
            }
            FLDCNTLGrid.Columns["Gd1Code"].DisplayIndex = 0;
        }


        private void FillRemainingGrid()
        {
            DataSet ds = Captain.DatabaseLayer.FieldControlsDB.GetLeftStatCustDescByScrCodeHIE(ScrCode, ScrHierarchy);
            DataTable dt = ds.Tables[0];
            RemainingGrid.Rows.Clear();
            int rowIndex = 0;
            int RowCount = 0;
            string Code = null;
            ChgRemFldcntlArr = new string[dt.Rows.Count, 4];
            foreach (DataRow dr in dt.Rows)
            {
                Code = dr["Code"].ToString();
                rowIndex = RemainingGrid.Rows.Add(dr["FDesc"].ToString().Trim(), dr["Sub"], false, false, false, Code);
                if (Code.Substring(0, 1) == "C" || Code.Substring(0, 1) == "c")
                    RemainingGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                ChgRemFldcntlArr[RowCount, 0] = "N";
                ChgRemFldcntlArr[RowCount, 1] = "N";
                ChgRemFldcntlArr[RowCount, 2] = "N";
                ChgRemFldcntlArr[RowCount, 3] = Code;


                if (Code.Substring(1, 1) == "A" || Code.Substring(1, 1) == "a")
                {
                    RemainingGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  //Color.Red;
                    RemainingGrid.Rows[rowIndex].Cells["Enab"].ReadOnly = true;
                    RemainingGrid.Rows[rowIndex].Cells["Req"].ReadOnly = true;
                    RemainingGrid.Rows[rowIndex].Cells["Enab"].Value = true;
                    RemainingGrid.Rows[rowIndex].Cells["Req"].Value = true;
                    ChgRemFldcntlArr[RowCount, 0] = "Y";
                    ChgRemFldcntlArr[RowCount, 1] = "Y";
                }

                RowCount++;
            }
            if (RowCount > 0)
            {
                RemainingGrid.Rows[rowIndex].Tag = 0;
                CbEnabLeft.Visible = true;
                CbShareLeft.Visible = true;
                STAT_Controls_exists = true;
            }
            RemainingGrid.Columns["Gd2Code"].DisplayIndex = 0;
            //if (RowCount > 50)
            //    this.RemainingGrid.Size = new System.Drawing.Size(434, 361);
            //else
            //    this.RemainingGrid.Size = new System.Drawing.Size(417, 361);

        }


        private void FillCustomGrid()
        {
            CustomGrid.Rows.Clear();
            CaptainModel model = new CaptainModel();
            List<CustfldsEntity> Cust = model.FieldControls.GetCUSTFLDSByScrCode(ScrCode, "CUSTFLDS", string.Empty);
            int RowCount = 0;
            if (Cust.Count > 0)
            {
                CustCntlPanel.Visible = true;
                int rowIndex = 0;
                string TmpType = null, MemAccess = null;
                foreach (CustfldsEntity Entity in Cust)
                {
                    TmpType = MemAccess = null;
                    TmpType = setResponseType(Entity.RespType);
                    MemAccess = setMemAccess(Entity.MemAccess);

                    rowIndex = CustomGrid.Rows.Add(Entity.CustCode, Entity.CustDesc.Trim(), TmpType, MemAccess, Entity.CustSeq, Entity.CustCode);

                    if (Entity.Active != "A")
                        CustomGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                    RowCount++;
                }
                if (RowCount > 0)
                {
                    int CurrentPage = 1;
                    if (Cust_PrivRow_Index > 0)
                    {
                        //CurrentPage = (Cust_PrivRow_Index / CustomGrid.ItemsPerPage);
                        //CurrentPage++;
                        //CustomGrid.CurrentPage = CurrentPage;
                        //CustomGrid.CurrentCell = CustomGrid.Rows[Cust_PrivRow_Index].Cells[1];
                    }
                    else
                    {
                        CustomGrid.CurrentCell = CustomGrid.Rows[0].Cells[1];
                        CustomGrid.Rows[rowIndex].Tag = 0;
                    }

                    Tab2_Loading_Complete = true;
                    CUST_Controls_exists = true;
                    FillCustomControls(GetSelectedCustomKey());

                }
            }
            else
                CustResp_lod_complete = true;
            //    CustCntlPanel.Visible = false;
        }

        private string setResponseType(string Type)
        {
            string TmpType = null;
            switch (Type)
            {
                case "D": TmpType = "Drop Down"; break;
                case "N": TmpType = "Numeric"; break;
                case "A":
                case "X": TmpType = "Text"; break;
                case "T": TmpType = "Date"; break;
                case "C": TmpType = "Check Box"; break;
            }

            return (TmpType);
        }


        private string setMemAccess(string Type)
        {
            string TmpType = null;
            switch (Type)
            {
                case "*": TmpType = "All Members"; break;
                case "A": TmpType = "Applicants"; break;
                case "H": TmpType = "Household"; break;
            }
            return (TmpType);
        }

        private void FillCustomControls(string RecKey)
        {
            CustResp_lod_complete = false;
            CaptainModel model = new CaptainModel();
            List<CustfldsEntity> Entity = model.FieldControls.GetSelCustDetails(RecKey);
            if (Mode == "Edit")
            {
                if (Entity.Count > 0)
                {
                    CustfldsEntity Cust = Entity[0];
                    TxtCode.Text = Cust.CustCode.Substring(1, 5);
                    txtSeq.Text = SetLeadingZeros(Cust.CustSeq);
                    TxtQuesDesc.Text = Cust.CustDesc;
                    TxtAbbr.Text = Cust.Question;
                    TxtEqual.Text = Cust.Equalto;
                    TxtGreat.Text = Cust.Greater;
                    TxtLess.Text = Cust.Less;
                    if (Cust.FutureDate == "Y")
                        CbFutureDt.Checked = true;
                    else
                        CbFutureDt.Checked = false;

                    if (Cust.Active == "A")
                        CbActive.Checked = true;
                    else
                        CbActive.Checked = false;

                    SetComboBoxValue(CmbType, Cust.RespType);
                    SetComboBoxValue(CmbAccess, Cust.MemAccess);

                    Check_Answers_for_CustCode(RecKey);
                    if (Cust.RespType == "D" || Cust.RespType == "C")
                        FillCustRespGrid(RecKey);
                    else
                    {
                        CustRespPanel.Visible = false;
                        CustResp_lod_complete = true;
                    }
                }
            }
            else
            {
                TxtCode.Clear();
                TxtCode.Text = "New";
                txtSeq.Clear();
                TxtQuesDesc.Clear();
                TxtAbbr.Clear();
                CmbType.SelectedIndex = 0;
                CmbAccess.SelectedIndex = 0;
                CustResp_lod_complete = true;
                CbActive.Checked = true;
            }
        }

        private void Check_Answers_for_CustCode(string QuesCode)
        {
            string FldCode = CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString();
            DataSet ds = Captain.DatabaseLayer.FieldControlsDB.Browse_FLDCNTLHIE(ScrCode, null, FldCode, null, null, null, null);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                CmbType.Enabled = CmbAccess.Enabled = false;
            else
                CmbType.Enabled = CmbAccess.Enabled = true;
        }

        private string GetSelectedCustomKey()
        {
            string RecordKey = null;
            if (CustomGrid != null)
            {
                try
                {
                    RecordKey = CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString();
                }
                catch (Exception ex) { }
            }
            return RecordKey;
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAPTAIN", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }
            return (TmpCode);
        }

        private void FillCustRespGrid(string CustCode)
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
            CaptainModel model = new CaptainModel();
            List<CustRespEntity> CustResp = model.FieldControls.GetCustRespByCustCode(CustCode);
            if (CustResp.Count > 0)
            {
                CustRespPanel.Visible = true;
                int rowIndex = 0;
                int RowCount = 0;
                foreach (CustRespEntity Entity in CustResp)
                {
                    //rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc);  
                    rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc.Trim(), Entity.RecType, Entity.Changed, Entity.RespSeq, "N");
                    RowCount++;
                }
                if (RowCount > 0)
                {
                    OrgCustResp = CustResp;
                    CustResp_lod_complete = true;
                    CustRespGrid.Rows[rowIndex].Tag = 0;
                    // PrivRow = CustRespGrid.SelectedRows[0];
                }

                //if (RowCount > 3)
                //    this.CustRespGrid.Size = new System.Drawing.Size(284, 131);
                //else
                //    this.CustRespGrid.Size = new System.Drawing.Size(266, 131);

            }
            CustResp_lod_complete = true;
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
        }

        private void CustomGrid_SelectionChanged(object sender, EventArgs e)
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);

            if (Mode.Equals("Edit"))
                Clear_Cus_ErrorProviders();

            if (Tab2_Loading_Complete)
                FillCustomControls(GetSelectedCustomKey());
        }


        private void Clear_Cus_ErrorProviders()
        {
            _errorProvider.SetError(lblReqHie, null);
            _errorProvider.SetError(label19, null);
            _errorProvider.SetError(CmbAccess, null);
            _errorProvider.SetError(CmbType, null);
            _errorProvider.SetError(txtSeq, null);
        }

        private void HierGrid_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void tabcontrol1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabcontrol1.SelectedIndex == 1)
            {
                BtnAdd.Text = "&Define New Custom Controls";
                PBHierarchy.Visible = false;
            }
            else
            {
                BtnAdd.Text = "&Define Controls for New Hierarchy";
                if (Mode == "Add")
                    PBHierarchy.Visible = true;
            }
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RespType = ((ListItem)CmbType.SelectedItem).Value.ToString();
            _errorProvider.SetError(label19, null);
            if (!((ListItem)CmbType.SelectedItem).Value.ToString().Equals("0"))
                _errorProvider.SetError(CmbType, null);

            switch (RespType)
            {
                case "C":
                case "D":
                    FurtreDtPanel.Visible = false;
                    NumericPanel.Visible = false;
                    CustRespPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(474, 147);
                    break;
                case "N":
                    CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = false;
                    NumericPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
                case "T":
                    NumericPanel.Visible = false;
                    CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
                default:
                    NumericPanel.Visible = false;
                    CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = false;
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
            }

            if (RespType == "D" || RespType == "C")
                CustRespPanel.Visible = true;
            else
                CustRespPanel.Visible = false;
        }

        private void txtSeq_LostFocus(object sender, EventArgs e)
        {
            txtSeq.Text = SetLeadingZeros(txtSeq.Text);
            CheckForDupCustCodeOrSeq("Sequence", txtSeq.Text);
            if (!string.IsNullOrEmpty(txtSeq.Text))
                _errorProvider.SetError(txtSeq, null);
        }

        private void TxtCode_LostFocus(object sender, EventArgs e)
        {
            //TxtCode.Text = "C" + SetLeadingZeros(TxtCode.Text);


            //////TxtCode.Text = SetLeadingZeros(TxtCode.Text);                             Yeswanth 08032012
            //////CheckForDupCustCodeOrSeq("Custom Code", TxtCode.Text);
        }

        private void CheckForDupCustCodeOrSeq(string Type, string CmpStr)
        {
            string CurRowCode = null;
            bool CmpResult = false;
            foreach (DataGridViewRow dr in CustomGrid.Rows)
            {
                if (Type == "Sequence")
                    CurRowCode = SetLeadingZeros(dr.Cells["Sequence"].Value.ToString());
                else
                    CurRowCode = dr.Cells["Code"].Value.ToString();

                if (CmpStr == CurRowCode)
                {
                    CmpResult = true;
                    //switch(Type)
                    //{   case "Sequence": txtSeq.Clear(); break;
                    //    default:         TxtCode.Clear(); break;
                    //}
                    break;
                }
            }
            if (CmpResult)
                MessageBox.Show(Type + ": " + CmpStr + " Already exists ", "CAPTAIN", MessageBoxButtons.OK);
        }


        private void CbEnableAll_CheckedChanged(object sender, EventArgs e)
        {
            if (Mode.Equals("Add"))
            {
                if (CbEnableAll.Checked)
                {
                    int TmpCount = 0;
                    foreach (DataGridViewRow dr in FLDCNTLGrid.Rows)
                    {
                        dr.Cells["Enabled"].Value = true;
                        ChgFldcntlArr[TmpCount, 0] = "Y";
                        TmpCount++;
                    }
                }
                else
                    SetOrgFLDCNTLArrValues();
            }
            else
            {
                if (CbEnableAll.Checked)
                    SetOrgFLDCNTLArrValues();
            }
        }

        private void SetOrgFLDCNTLArrValues()
        {
            int i = 0;
            CbShareAll.Checked = false;
            foreach (DataGridViewRow dr in FLDCNTLGrid.Rows)
            {
                if (Mode.Equals("Add"))
                {
                    if ((dr.Cells["Gd1Code"].Value.ToString()).Substring(1, 1) == "A")
                    {
                        dr.Cells["Enabled"].Value = true;
                        dr.Cells["Required"].Value = true;
                        ChgFldcntlArr[i, 0] = ChgFldcntlArr[i, 1] = "Y";
                    }
                    else
                    {
                        dr.Cells["Enabled"].Value = dr.Cells["Required"].Value = false;
                        ChgFldcntlArr[i, 0] = ChgFldcntlArr[i, 1] = "N";
                    }
                    dr.Cells["Shared"].Value = false;
                    ChgFldcntlArr[i, 2] = "N";

                }
                else
                {
                    if (OrgFldcntHieEntity[i].Enab == "Y" || OrgFldcntHieEntity[i].Enab == "y")
                    {
                        dr.Cells["Enabled"].Value = true; ChgFldcntlArr[i, 0] = "Y";
                    }
                    else
                    {
                        dr.Cells["Enabled"].Value = false; ChgFldcntlArr[i, 0] = "N";
                    }
                    if (OrgFldcntHieEntity[i].Req == "Y" || OrgFldcntHieEntity[i].Req == "y")
                    {
                        dr.Cells["Required"].Value = true; ChgFldcntlArr[i, 1] = "Y";
                    }
                    else
                    {
                        dr.Cells["Required"].Value = false; ChgFldcntlArr[i, 1] = "N";
                    }
                    if (OrgFldcntHieEntity[i].Shared == "Y" || OrgFldcntHieEntity[i].Shared == "y")
                    {
                        dr.Cells["Shared"].Value = true; ChgFldcntlArr[i, 2] = "Y";
                    }
                    else
                    {
                        dr.Cells["Shared"].Value = false; ChgFldcntlArr[i, 2] = "N";
                    }
                }
                i++;
            }
        }

        private void SetOrgRemainingFLDCNTLArrValues()
        {
            int TmpCount = 0;
            CbShareLeft.Checked = false;
            foreach (DataGridViewRow dr in RemainingGrid.Rows)
            {
                if (dr.Cells["Gd2Code"].Value.ToString().Substring(1, 1) != "A" &&
                    dr.Cells["Gd2Code"].Value.ToString().Substring(1, 1) != "a")
                {
                    dr.Cells["Enab"].Value = false;
                    dr.Cells["Req"].Value = false;
                    dr.Cells["Share"].Value = false;

                    ChgRemFldcntlArr[TmpCount, 0] = "N";
                    ChgRemFldcntlArr[TmpCount, 1] = "N";
                    ChgRemFldcntlArr[TmpCount, 2] = "N";
                }
                TmpCount++;
            }
        }

        private void CbShareAll_CheckedChanged(object sender, EventArgs e)
        {
            int TmpCount = 0;
            if (CbShareAll.Checked)
            {
                if (Mode.Equals("Add"))
                    CbEnableAll.Checked = true;
                else
                    CbEnableAll.Checked = false;
                foreach (DataGridViewRow dr in FLDCNTLGrid.Rows)
                {
                    dr.Cells["Enabled"].Value = true;
                    dr.Cells["Shared"].Value = true;
                    ChgFldcntlArr[TmpCount, 0] = "Y";
                    ChgFldcntlArr[TmpCount, 2] = "Y";
                    TmpCount++;
                }
            }
            else
            {
                foreach (DataGridViewRow dr in FLDCNTLGrid.Rows)
                {
                    dr.Cells["Shared"].Value = false;
                    ChgFldcntlArr[TmpCount, 2] = "N";
                    TmpCount++;
                }
            }
        }


        private void FLDCNTLGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool CanProceed = true;
            if (String.IsNullOrEmpty(TxtHierarchy.Text))
                MessageBox.Show("Please Select Hierarchy and then Proceed", "CAPTAIN", MessageBoxButtons.OK);
            //if (FLDCNTLGrid.CurrentRow.Cells["Gd1Code"].Value.ToString().Substring(1, 1) == "A" && e.ColumnIndex != 4 )
            //    CanProceed = false;

            if (FLDCNTLGrid.Rows.Count > 0 && CanProceed)
            {
                int ColIdx = 0;
                int RowIdx = 0;
                

                string Tmp = "false";
                if (e.RowIndex > -1 && e.ColumnIndex>-1)
                {
                    ColIdx = FLDCNTLGrid.CurrentCell.ColumnIndex;
                    RowIdx = FLDCNTLGrid.CurrentCell.RowIndex;

                    switch (e.ColumnIndex)
                    {
                        case 2:
                            Tmp = FLDCNTLGrid.CurrentRow.Cells["Enabled"].Value.ToString();
                            if (Tmp == "True")
                                ChgFldcntlArr[RowIdx, 0] = "Y";
                            else
                            {
                                ChgFldcntlArr[RowIdx, 0] = "N";
                                ChgFldcntlArr[RowIdx, 1] = "N";
                                FLDCNTLGrid.CurrentRow.Cells["Required"].Value = false;
                                ChgFldcntlArr[RowIdx, 2] = "N";
                                FLDCNTLGrid.CurrentRow.Cells["Shared"].Value = false;
                            }
                            break;
                        case 3:
                            Tmp = FLDCNTLGrid.CurrentRow.Cells["Required"].Value.ToString();
                            if (Tmp == "True")
                            {
                                ChgFldcntlArr[RowIdx, 0] = "Y";
                                FLDCNTLGrid.CurrentRow.Cells["Enabled"].Value = true;
                                ChgFldcntlArr[RowIdx, 1] = "Y";
                            }
                            else
                                ChgFldcntlArr[RowIdx, 1] = "N";
                            break;
                        case 4:
                            Tmp = FLDCNTLGrid.CurrentRow.Cells["Shared"].Value.ToString();
                            if (Tmp == "True")
                            {
                                ChgFldcntlArr[RowIdx, 0] = "Y";
                                FLDCNTLGrid.CurrentRow.Cells["Enabled"].Value = true;
                                //ChgFldcntlArr[RowIdx, 1] = "Y";
                                //FLDCNTLGrid.CurrentRow.Cells["Required"].Value = true;
                                ChgFldcntlArr[RowIdx, 2] = "Y";
                            }
                            else
                                ChgFldcntlArr[RowIdx, 2] = "N";
                            break;
                    }
                }
            }
        }

        private void RemainingGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RemainingGrid.Rows.Count > 0)
            {
                int ColIdx = 0;
                int RowIdx = 0;

                

                string Tmp = "false";
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    ColIdx = RemainingGrid.CurrentCell.ColumnIndex;
                    RowIdx = RemainingGrid.CurrentCell.RowIndex;

                    switch (e.ColumnIndex)
                    {
                        case 2:
                            Tmp = RemainingGrid.CurrentRow.Cells["Enab"].Value.ToString();
                            if (Tmp == "True")
                                ChgRemFldcntlArr[RowIdx, 0] = "Y";
                            else
                            {
                                ChgRemFldcntlArr[RowIdx, 0] = "N";
                                ChgRemFldcntlArr[RowIdx, 1] = "N";
                                RemainingGrid.CurrentRow.Cells["Req"].Value = false;
                                ChgRemFldcntlArr[RowIdx, 2] = "N";
                                RemainingGrid.CurrentRow.Cells["Share"].Value = false;
                            }
                            break;
                        case 3:
                            Tmp = RemainingGrid.CurrentRow.Cells["Req"].Value.ToString();
                            if (Tmp == "True")
                            {
                                ChgRemFldcntlArr[RowIdx, 0] = "Y";
                                RemainingGrid.CurrentRow.Cells["Enab"].Value = true;
                                ChgRemFldcntlArr[RowIdx, 1] = "Y";
                            }
                            else
                                ChgRemFldcntlArr[RowIdx, 1] = "N";
                            break;
                        case 4:
                            Tmp = RemainingGrid.CurrentRow.Cells["Share"].Value.ToString();
                            if (Tmp == "True")
                            {
                                ChgRemFldcntlArr[RowIdx, 0] = "Y";
                                RemainingGrid.CurrentRow.Cells["Enab"].Value = true;
                                //ChgRemFldcntlArr[RowIdx, 1] = "Y";
                                //RemainingGrid.CurrentRow.Cells["Req"].Value = true;
                                ChgRemFldcntlArr[RowIdx, 2] = "Y";
                            }
                            else
                                ChgRemFldcntlArr[RowIdx, 2] = "N";
                            break;
                    }
                }
            }
        }

        private bool ValidateCustControls()
        {
            bool isValid = true;

            //////if (Mode.Equals("Add") && (String.IsNullOrEmpty(TxtCode.Text)))          // 08032012
            //////{
            //////    _errorProvider.SetError(TxtCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //////    isValid = false;
            //////}
            //////else
            //////    _errorProvider.SetError(TxtCode, null);

            if ((String.IsNullOrEmpty(txtSeq.Text)))
            {
                _errorProvider.SetError(txtSeq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSeq.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtSeq, null);

            if (((ListItem)CmbType.SelectedItem).Value.ToString().Equals("0"))
            {
                _errorProvider.SetError(CmbType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblType.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbType, null);

            if (((ListItem)CmbAccess.SelectedItem).Value.ToString().Equals("0"))
            {
                _errorProvider.SetError(CmbAccess, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccess.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbAccess, null);

            if (string.IsNullOrEmpty(TxtQuesDesc.Text.Trim()))
            {
                _errorProvider.SetError(TxtQuesDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblquesDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtQuesDesc, null);


            if (!(CustRespGrid.Rows.Count > 1) && (RespType == "C" || RespType == "D"))
            {
                label19.Text = " "; label19.Visible = true;
                _errorProvider.SetError(label19, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Response Grid".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                label19.Visible = false;
                _errorProvider.SetError(label19, null);
            }

            return isValid;
        }

        private bool FldcntlAddValidate()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(TxtHieDeSC.Text))
            {
                lblReqHie.Visible = true;
                _errorProvider.SetError(lblReqHie, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Hierarchy".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(lblReqHie, null);

            return isValid;
        }


        private bool CheckEditAttributes()
        {
            bool CanSave = false;
            int TmpCnt = 0;
            int TmpChgCnt = 0;
            foreach (FLDDESCHieEntity Entity in OrgFldcntHieEntity)
            {
                if (Entity.Enab != ChgFldcntlArr[TmpCnt, 0] ||
                    Entity.Req != ChgFldcntlArr[TmpCnt, 1] ||
                    Entity.Shared != ChgFldcntlArr[TmpCnt, 2]) //&&
                //Entity.FldCode == ChgFldcntlArr[TmpCnt, 3])
                {
                    Entity.Enab = ChgFldcntlArr[TmpCnt, 0];
                    Entity.Req = ChgFldcntlArr[TmpCnt, 1];
                    Entity.Shared = ChgFldcntlArr[TmpCnt, 2];
                    Entity.Changed = "Y";
                    TmpChgCnt++;
                }
                TmpCnt++;
            }
            if (TmpChgCnt > 0)
                CanSave = true;
            //else
            //    MessageBox.Show("No Attribute is Changed", "CAPTAIN", MessageBoxButtons.OK);

            return CanSave;
        }

        private bool CheckAddAttributes()
        {
            bool CanSave = false;
            int TmpCnt = 0;
            int TmpChgCnt = 0;
            foreach (FLDCNTLHIEAddEntity Entity in FldcntAddEntity)
            {
                if (Entity.Enab != ChgFldcntlArr[TmpCnt, 0] ||
                    Entity.Req != ChgFldcntlArr[TmpCnt, 1] ||
                    Entity.Shared != ChgFldcntlArr[TmpCnt, 2]) //&&
                //Entity.FldCode == ChgFldcntlArr[TmpCnt, 3])
                {
                    Entity.Enab = ChgFldcntlArr[TmpCnt, 0];
                    Entity.Req = ChgFldcntlArr[TmpCnt, 1];
                    Entity.Shared = ChgFldcntlArr[TmpCnt, 2];
                    Entity.Changed = "Y";
                    TmpChgCnt++;
                }
                TmpCnt++;
            }
            if (TmpChgCnt > 0)
                CanSave = true;
            //else
            //    MessageBox.Show("No Attribute is Changed", "CAPTAIN", MessageBoxButtons.OK);

            return CanSave;
        }

        private void BtnAddCust_Click(object sender, EventArgs e)
        {
            if (CustomGrid.RowCount > 0)
                Cust_PrivRow_Index = CustomGrid.CurrentCell.RowIndex;
            if (ValidateCustControls())
            {
                CaptainModel model = new CaptainModel();
                CustfldsEntity CustDetails = new CustfldsEntity();
                List<CustfldsEntity> Entity = model.FieldControls.GetSelCustDetails(GetSelectedCustomKey());
                if (Entity.Count > 0 && Mode == "Edit")
                {
                    CustDetails = Entity[0];
                    CustDetails.UpdateType = "U";
                    CustDetails.CustSeq = SetLeadingZeros(txtSeq.Text);
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Question = TxtAbbr.Text;
                    CustDetails.RespType = RespType;
                    CustDetails.MemAccess = ((ListItem)CmbAccess.SelectedItem).Value.ToString();
                    CustDetails.CustSeq = txtSeq.Text;
                    CustDetails.Equalto = "0";
                    CustDetails.Greater = "0";
                    CustDetails.Less = "0";

                    CustDetails.Active = "I";
                    if (CbActive.Checked)
                        CustDetails.Active = "A";

                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.ChdOpr = BaseForm.UserID;
                }
                else
                {
                    CustDetails.UpdateType = "I";
                    CustDetails.ScrCode = ScrCode;
                    CustDetails.CustCode = "C" + TxtCode.Text;
                    CustDetails.CustSeq = txtSeq.Text;
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Question = TxtAbbr.Text;
                    CustDetails.RespType = RespType;
                    CustDetails.MemAccess = ((ListItem)CmbAccess.SelectedItem).Value.ToString();
                    CustDetails.CustSeq = txtSeq.Text;
                    CustDetails.Equalto = "0";
                    CustDetails.Greater = "0";
                    CustDetails.Less = "0";

                    CustDetails.Active = "I";
                    if (CbActive.Checked)
                        CustDetails.Active = "A";


                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.AddOpr = BaseForm.UserID;
                    CustDetails.ChdOpr = BaseForm.UserID;
                }
                switch (RespType)
                {
                    case "C":
                    case "D":
                        FurtreDtPanel.Visible = false;
                        NumericPanel.Visible = false;
                        CustRespPanel.Visible = true; break;
                    case "N":
                        if (!(string.IsNullOrEmpty(TxtEqual.Text)))
                            CustDetails.Equalto = TxtEqual.Text;
                        if (!(string.IsNullOrEmpty(TxtGreat.Text)))
                            CustDetails.Greater = TxtGreat.Text;
                        if (!(string.IsNullOrEmpty(TxtLess.Text)))
                            CustDetails.Less = TxtLess.Text;
                        break;
                    case "T":
                        CustDetails.FutureDate = "Y";
                        if (CbFutureDt.Checked)
                            CustDetails.FutureDate = "Y"; break;
                }


                string New_CUST_Code_Code = "NewCustCode";

                if (model.FieldControls.UpdateCUSTFLDS(CustDetails, out New_CUST_Code_Code))
                {
                    if (CustDetails.UpdateType == "U")
                        AlertBox.Show("Custom Question Updated Successfully");
                       // MessageBox.Show("Custom Question Updated Successfully ", "CAPTAIN", MessageBoxButtons.OK);
                    else
                    {
                        AlertBox.Show("Custom Question Inserted Successfully");
                        //MessageBox.Show("Custom Question Inserted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        Cust_PrivRow_Index = CustomGrid.Rows.Count;
                    }

                    CustResp_lod_complete = Tab2_Loading_Complete = false;
                    if (CustDetails.RespType == "C" || CustDetails.RespType == "D")
                        model.FieldControls.UpdateCUSTRESP(OrgCustResp, New_CUST_Code_Code);
                    FillCustomGrid();
                }
                else
                {
                    if (CustDetails.UpdateType == "U")
                        MessageBox.Show("Failed to Update Record", "CAPTAIN", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Failed to Insert Record", "CAPTAIN", MessageBoxButtons.OK);
                }
            }
        }

        private void CmbAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!((ListItem)CmbAccess.SelectedItem).Value.ToString().Equals("0"))
                _errorProvider.SetError(CmbAccess, null);
        }

        private void PBHierarchy_Click(object sender, EventArgs e)
        {
            TmpGrid.Rows.Clear();

            ////HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, "Master", null);
            ////hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            ////hierarchieSelectionForm.ShowDialog();


            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Master", "I", "*", "R");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection addForm = new HierarchieSelection(BaseForm, string.Empty, "Master", "I", "*", "R", BaseForm.UserID);
            addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            addForm.StartPosition = FormStartPosition.CenterScreen;
            addForm.ShowDialog();

        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            //HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            //HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            HierarchieSelection form = sender as HierarchieSelection;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;

            bool HieExist = false;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    int Tmp = HierGrid.Rows.Count;
                    string Tmpstr = null;
                    string Tmpstr1 = null;
                    foreach (DataGridViewRow dr in HierGrid.Rows)
                    {
                        Tmpstr = dr.Cells["HieKey"].Value.ToString();
                        Tmpstr1 = row.Code.Substring(0, 2) + row.Code.Substring(3, 2) + row.Code.Substring(6, 2);
                        if (Tmpstr == Tmpstr1)
                            HieExist = true;
                    }
                    if (!HieExist)
                    {
                        TxtHierarchy.Text = row.Code;  //.Substring(0, 2) + '-' + row.Code.Substring(2, 2) + '-' + row.Code.Substring(4, 2)
                        TxtHieDeSC.Text = "   " + row.HirarchyName.ToString();
                        //_errorProvider.SetError(PBHierarchy, null);
                        lblReqHie.Visible = false;
                        _errorProvider.SetError(lblReqHie, null);

                    }
                    else
                    {
                        TxtHierarchy.Clear(); TxtHieDeSC.Clear();
                        MessageBox.Show("Field Controls are Already Defined for Hierarchy '" + row.Code + "' !!!", "CAPTAIN", MessageBoxButtons.OK);
                    }
                    if (HieExist)
                        break;
                }
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            bool Save = false;
            if (Mode.Equals("Edit"))
                Save = UpdateEditedControls();
            else
                Save = UpdateNewControls();
            if (Save)
            {
                string TempHie = null;
                FLDCNTLMaintenanceControl FLDControl = BaseForm.GetBaseUserControl() as FLDCNTLMaintenanceControl;
                if (FLDControl != null)
                {
                    TempHie = TxtHierarchy.Text.Substring(0, 2) + TxtHierarchy.Text.Substring(3, 2) + TxtHierarchy.Text.Substring(6, 2);
                    FLDControl.Refresh(ScrCode, TempHie);
                }
                this.Close();
                AlertBox.Show("Record(s) Updated Successfully");
                //MessageBox.Show("Record(s) Updated Successfully", "CAPTAIN", MessageBoxButtons.OK);
            }
        }

        private bool UpdateEditedControls()
        {
            char valid = 'N';
            if (CbFldValidation.Checked)
                valid = 'Y';
            bool saved = false;
            CaptainModel model = new CaptainModel();

            string strExcludeMem = string.Empty;
            string strExcludeMem2 = string.Empty;



            if (CheckEditAttributes())
            {
                FLDDESCHieEntity fldMem = OrgFldcntHieEntity.Find(u => u.FldCode == "S00460");
                if (fldMem != null)
                {
                    strExcludeMem = fldMem.Enab;
                }
                FLDDESCHieEntity fldMem2 = OrgFldcntHieEntity.Find(u => u.FldCode == "S00461");
                if (fldMem2 != null)
                {
                    strExcludeMem2 = fldMem2.Enab;
                }
                if (strExcludeMem == "Y" && strExcludeMem2 == "Y")
                {
                    MessageBox.Show("Allowed only one excluede member", "CAPTAIN", MessageBoxButtons.OK);
                }
                else
                {
                    if (model.FieldControls.UpdateFLDCNTLHIE(OrgFldcntHieEntity, UserName, valid))
                    {
                        saved = true;
                       // AlertBox.Show("Record(s) Updated Successfully");
                        //MessageBox.Show("Record(s) Updated Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("Failed to Update Record(s)", "CAPTAIN", MessageBoxButtons.OK);
                }
            }
            else
            {
                FLDDESCHieEntity fldMem = OrgFldcntHieEntity.Find(u => u.FldCode == "S00460");
                if (fldMem != null)
                {
                    strExcludeMem = fldMem.Enab;
                }
                FLDDESCHieEntity fldMem2 = OrgFldcntHieEntity.Find(u => u.FldCode == "S00461");
                if (fldMem2 != null)
                {
                    strExcludeMem2 = fldMem2.Enab;
                }
            }
            //else
            //    MessageBox.Show("No Attribute is Changed", "CAPTAIN", MessageBoxButtons.OK);

            bool CanSave = false;
            for (int i = 0; i < (ChgRemFldcntlArr.Length / 4); i++)
            {
                if (ChgRemFldcntlArr[i, 0] == "Y" || ChgRemFldcntlArr[i, 1] == "Y" || ChgRemFldcntlArr[i, 2] == "Y")
                {
                    CanSave = true;
                    break;
                }
            }

            if (CanSave)
            {
                if (ChgRemFldcntlArr.Length > 4)
                {
                    for (int i = 0; i < (ChgRemFldcntlArr.Length / 4); i++)
                    {
                        if (ChgRemFldcntlArr[i, 0] == "Y" || ChgRemFldcntlArr[i, 1] == "Y" || ChgRemFldcntlArr[i, 2] == "Y")
                        {
                            if (ChgRemFldcntlArr[i, 3].ToString() == "S00460")
                            {
                                strExcludeMem = ChgRemFldcntlArr[i, 0].ToString();
                            }
                            if (ChgRemFldcntlArr[i, 3].ToString() == "S00461")
                            {
                                strExcludeMem2 = ChgRemFldcntlArr[i, 0].ToString();
                            }
                        }
                    }
                }
                if (strExcludeMem == "Y" && strExcludeMem2 == "Y")
                {
                    MessageBox.Show("Allowed only one excluede member", "CAPTAIN", MessageBoxButtons.OK);
                }
                else
                {
                    string TempHie = null;
                    TempHie = TxtHierarchy.Text.Substring(0, 2) + TxtHierarchy.Text.Substring(3, 2) + TxtHierarchy.Text.Substring(6, 2);
                    if (model.FieldControls.InsertInactiveFLDCNTLHIE(ScrCode, TempHie, ChgRemFldcntlArr, UserName, valid))
                    {
                        saved = true;
                       // AlertBox.Show("Record(s) Updated Successfully");
                        //MessageBox.Show("Record(s) Updated is Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("Failed to Update Record(s)", "CAPTAIN", MessageBoxButtons.OK);
                }
            }

            if (!CheckEditAttributes() && !CanSave && Org_FldValidation != valid.ToString())
            {
                string TmpHie = null;
                TmpHie = TxtHierarchy.Text.Substring(0, 2) + TxtHierarchy.Text.Substring(3, 2) + TxtHierarchy.Text.Substring(6, 2);

                if (model.FieldControls.UpdateFLDCNTL("U", ScrCode, TmpHie, valid.ToString(), BaseForm.UserID))
                {
                   // AlertBox.Show("Record(s) Updated Successfully");
                   // MessageBox.Show("Record(s) Update is Successfully", "CAPTAIN", MessageBoxButtons.OK); saved = true;
                }
                else
                    MessageBox.Show("Failed to Update Record(s)", "CAPTAIN", MessageBoxButtons.OK);
            }


            return saved;
        }

        private bool UpdateNewControls()
        {
            char valid = 'N';
            if (CbFldValidation.Checked)
                valid = 'Y';
            bool saved = false;
            if (FldcntlAddValidate() && CheckAddAttributes())
            {
                string strExcludeMem = string.Empty;
                string strExcludeMem2 = string.Empty;

                FLDCNTLHIEAddEntity fldMem = FldcntAddEntity.Find(u => u.FldCode == "S00460");
                if (fldMem != null)
                {
                    strExcludeMem = fldMem.Enab;
                }
                FLDCNTLHIEAddEntity fldMem2 = FldcntAddEntity.Find(u => u.FldCode == "S00461");
                if (fldMem2 != null)
                {
                    strExcludeMem2 = fldMem2.Enab;
                }
                if (strExcludeMem == "Y" && strExcludeMem2 == "Y")
                {
                    MessageBox.Show("Allowed only one excluede member", "CAPTAIN", MessageBoxButtons.OK);
                }
                else
                {
                    string SelHierarchy = null;
                    SelHierarchy = TxtHierarchy.Text.Substring(0, 2) + TxtHierarchy.Text.Substring(3, 2) + TxtHierarchy.Text.Substring(6, 2);

                    CaptainModel model = new CaptainModel();
                    if (model.FieldControls.InsertFLDCNTLHIE(FldcntAddEntity, SelHierarchy, UserName, valid))
                    {
                        saved = true;
                       // AlertBox.Show("Record(s) Inserted Successfully");
                       // MessageBox.Show("Record(s) Inserted Successfully ", "CAPTAIN", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("Failed to Update Record(s)", "CAPTAIN", MessageBoxButtons.OK);
                }
            }
            return saved;
        }

        private void CbEnabLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (CbEnabLeft.Checked)
            {
                int TmpCount = 0;
                foreach (DataGridViewRow dr in RemainingGrid.Rows)
                {
                    dr.Cells["Enab"].Value = true;
                    ChgRemFldcntlArr[TmpCount, 0] = "Y";
                    TmpCount++;
                }
            }
            else
                SetOrgRemainingFLDCNTLArrValues();
        }

        private void CbShareLeft_CheckedChanged(object sender, EventArgs e)
        {
            int TmpCount = 0;
            if (CbShareLeft.Checked)
            {
                CbEnabLeft.Checked = true;
                foreach (DataGridViewRow dr in RemainingGrid.Rows)
                {
                    dr.Cells["Enab"].Value = true;
                    dr.Cells["Share"].Value = true;
                    ChgRemFldcntlArr[TmpCount, 0] = "Y";
                    ChgRemFldcntlArr[TmpCount, 2] = "Y";
                }
            }
            else
            {
                foreach (DataGridViewRow dr in RemainingGrid.Rows)
                {
                    dr.Cells["Share"].Value = false;
                    ChgRemFldcntlArr[TmpCount, 2] = "N";
                    TmpCount++;
                }
            }
        }

        //private void PrepareCustResponses(string CustCode)
        //{
        //    CaptainModel model = new CaptainModel();
        //    List<CustRespEntity> PassCustResponses = new List<CustRespEntity>();
        //    CustRespEntity Cust = new CustRespEntity();
        //    string[,] PassRespArr = new string[CustRespGrid.Rows.Count, 2];
        //    PassRespArr = null;
        //    int TmpCount = 0;
        //    foreach (DataGridViewRow dr in CustRespGrid.Rows)
        //    {
        //        if (!(String.IsNullOrEmpty(dr.Cells["RespCode"].Value.ToString())) &&
        //           !(String.IsNullOrEmpty(dr.Cells["RespDesc"].Value.ToString())))
        //        {
        //            //Cust.ScrCode = string.Empty;
        //            Cust.ResoCode = string.Empty;
        //            Cust.RespSeq = string.Empty;
        //            Cust.RespDesc = string.Empty;
        //            Cust.DescCode = string.Empty;
        //            Cust.AddOpr = string.Empty;
        //            Cust.AddOpr = string.Empty;

        //            //Cust.ScrCode  = ScrCode;
        //            Cust.ResoCode = CustCode;
        //            Cust.RespSeq = (TmpCount + 1).ToString();
        //            Cust.RespDesc = dr.Cells["RespDesc"].Value.ToString();
        //            Cust.DescCode = dr.Cells["RespCode"].Value.ToString();
        //            Cust.AddOpr = BaseForm.UserID;
        //            Cust.AddOpr   = BaseForm.UserID;

        //            PassCustResponses.Add(Cust);
        //            TmpCount++;
        //        }
        //    }

        //    if (TmpCount > 0)
        //        model.FieldControls.UpdateCUSTRESP(PassCustResponses);
        //}

        private void FieldControlForm_Leave(object sender, EventArgs e)
        {
            string TempHie = null;
            FLDCNTLMaintenanceControl FLDControl = BaseForm.GetBaseUserControl() as FLDCNTLMaintenanceControl;
            if (FLDControl != null)
            {
                TempHie = TxtHierarchy.Text.Substring(0, 2) + TxtHierarchy.Text.Substring(3, 2) + TxtHierarchy.Text.Substring(6, 2);
                FLDControl.Refresh(ScrCode, TempHie);
            }
            this.Close();
            FLDControl.Refresh(ScrCode, TempHie);
        }


        ////private void CustRespGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        ////{
        ////    bool Can_Proceed = true;
        ////    if (Mode.Equals("Edit") && CustRespGrid.CurrentRow.Cells["CAMS_Det_SW"].EditedFormattedValue.ToString() == "Y")
        ////    {
        ////        if (string.IsNullOrEmpty(CustRespGrid.CurrentRow.Cells["BranchDef"].EditedFormattedValue.ToString()))
        ////        {
        ////            CustRespGrid.CurrentRow.Cells["BranchDef"].Value = CustRespGrid.CurrentRow.Cells["Dup_Def"].Value.ToString();
        ////            MessageBox.Show("CA/MS Details Exists for this Branch \n You can not delete this Branch Description", "CAPTAIN");
        ////            CustRespGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Black;
        ////            Can_Proceed = false;
        ////        }
        ////        else
        ////            CustRespGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
        ////    }

        ////    if (Can_Proceed)
        ////    {
        ////        if (!string.IsNullOrEmpty(CustRespGrid.CurrentRow.Cells["BranchDef"].EditedFormattedValue.ToString()))
        ////        {
        ////            if (CustRespGrid.CurrentRow.Cells["BranchDef"].Value.ToString() != CustRespGrid.CurrentRow.Cells["Dup_Def"].Value.ToString())
        ////            {
        ////                if (CustRespGrid.CurrentRow.Cells["Dup_Def"].Value.ToString() == " ")
        ////                    CustRespGrid.CurrentRow.Cells["Row_status"].Value = "A";
        ////                else
        ////                    CustRespGrid.CurrentRow.Cells["Row_status"].Value = "Y";

        ////                if (CustRespGrid.CurrentRow.Cells["Branch_Code"].Value.ToString() == "P")
        ////                    _errorProvider.SetError(CustRespGrid, null);

        ////                CustRespGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Blue;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            if (!string.IsNullOrEmpty(CustRespGrid.CurrentRow.Cells["Dup_Def"].Value.ToString()))
        ////            {
        ////                CustRespGrid.CurrentRow.Cells["Row_status"].Value = "D";
        ////                CustRespGrid.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
        ////            }
        ////        }
        ////    }
        ////}



        private void CustRespGrid_SelectionChanged(object sender, EventArgs e)
        {
            bool CanSave = true, Delete_SW = false;

            //if (PrivRow == null)
            //    PrivRow.Tag = 0; 

            if (CustResp_lod_complete && PrivRow != null)
            {
                if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) ||
                    string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()))
                {

                    if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) &&
                        string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
                        !string.IsNullOrEmpty(PrivRow.Cells["Empty_Row"].EditedFormattedValue.ToString()))
                    {
                        MessageBox.Show("This Response Will be Delated", "CapSystems", MessageBoxButtons.OK);
                        Delete_SW = true;
                    }
                    else
                    {
                        MessageBox.Show("Please Fill 'Code' and 'Description' ", "CAPTAIN", MessageBoxButtons.OK); CanSave = false;
                    }
                }


                if (CanSave && CheckDupCustResps(Delete_SW))
                {
                    if (!(string.IsNullOrEmpty(PrivRow.Cells["Type"].EditedFormattedValue.ToString())))
                    {
                        foreach (CustRespEntity Entity in OrgCustResp)
                        {
                            if (PrivRow.Cells["CustSeq"].Value.ToString() == Entity.RespSeq)
                            {
                                if (!Delete_SW)
                                {
                                    Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                                    Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                }
                                else
                                {
                                    this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);

                                    //if (!CustRespGrid.Rows[this.CustRespGrid.SelectedRows[0].Index].IsNewRow)
                                    CustRespGrid.Rows.RemoveAt(PrivRow.Index);
                                    //CustRespGrid.Rows.RemoveAt(this.CustRespGrid.SelectedRows[0].Index);

                                    this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);

                                    Entity.RecType = "D";
                                }

                                Entity.Changed = "Y";
                                // PrivRow.Cells["Changed"].Value = "Y";
                                break;
                            }
                        }
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
                            !string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()))
                        {
                            PrivRow.Cells["Type"].Value = "I";
                            PrivRow.Cells["Empty_Row"].Value = "N";
                            PrivRow.Cells["CustSeq"].Value = (OrgCustResp.Count + 1).ToString();

                            CustRespEntity New_Entity = new CustRespEntity();
                            New_Entity.RecType = "I";
                            New_Entity.ScrCode = ScrCode;
                            if (Mode.Equals("Edit"))
                                New_Entity.ResoCode = CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString();
                            else
                            {
                                if (!string.IsNullOrEmpty(TxtCode.Text.Trim()))
                                    New_Entity.ResoCode = "C" + TxtCode.Text.Trim();
                            }
                            New_Entity.RespSeq = (OrgCustResp.Count + 1).ToString();
                            New_Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                            New_Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                            New_Entity.AddOpr = BaseForm.UserID;
                            New_Entity.ChgOpr = BaseForm.UserID;
                            New_Entity.Changed = "Y";

                            OrgCustResp.Add(new CustRespEntity(New_Entity));
                        }
                    }
                }
            }

            if (CustRespGrid.Rows.Count >= 0)
            {
                DataGridViewRow row = CustRespGrid.SelectedRows[0];
                PrivRow = row;
            }

        }

        private bool CheckDupCustResps(bool Delete_SW)
        {
            bool ReturnVal = true;
            if (!Delete_SW)
            {
                string TestCode, TestDesc, TmpSelCode = null, TmpSelDesc = null;
                TmpSelCode = PrivRow.Cells["RespCode"].Value.ToString();
                TmpSelDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                foreach (DataGridViewRow dr in CustRespGrid.Rows)
                {
                    TestCode = TestDesc = null;
                    if (!(string.IsNullOrEmpty(dr.Cells["RespCode"].FormattedValue.ToString())))
                        TestCode = dr.Cells["RespCode"].Value.ToString().Trim();
                    if (!(string.IsNullOrEmpty(dr.Cells["RespDesc"].FormattedValue.ToString())))
                        TestDesc = dr.Cells["RespDesc"].Value.ToString().Trim();

                    if (TmpSelCode == TestCode && PrivRow != dr)
                    {
                        ReturnVal = false;
                        MessageBox.Show("Response Code '" + "'" + TmpSelCode + "' Already Exists", "CAPTAIN", MessageBoxButtons.OK);
                        break;
                    }
                    if (TmpSelDesc == TestDesc && PrivRow != dr)
                    {
                        ReturnVal = false;
                        MessageBox.Show("Code Description '" + TmpSelDesc + "' Already Exists", "CAPTAIN", MessageBoxButtons.OK);
                        break;
                    }
                }
            }
            return ReturnVal;
        }

        private void PbFLDRight_Click(object sender, EventArgs e)
        {
            PbFLDRight.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 610;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle.WrapMode = DataGridViewTriState.True; 
            this.dataGridViewTextBoxColumn2.Visible = true;
            this.dataGridViewTextBoxColumn2.Width = 175;
            this.LeftPanel.Size = new System.Drawing.Size(1075, 500);
            this.FLDCNTLGrid.Size = new System.Drawing.Size(1075, 500);
            // this.BtnUpdate.Location = new System.Drawing.Point(670, 399);
            this.pnlBtnUpdate.Padding = new Padding(5, 5, 15, 5);
            this.CbFldValidation.Location = new System.Drawing.Point(743, 3);
            this.CbEnableAll.Location = new System.Drawing.Point(866, 3);
            this.CbShareAll.Location = new System.Drawing.Point(985, 3);
            this.PbFLDLeft.Location = new System.Drawing.Point(1035, 5);
            PbFLDLeft.Visible = true;
            this.dataGridViewTextBoxColumn2.ShowInVisibilityMenu = true;
            
        }


        //private void PbFLDRight_Click(object sender, EventArgs e)
        //{
        //    PbFLDRight.Visible = false;
        //    this.dataGridViewTextBoxColumn1.Width = 530;
        //    this.dataGridViewTextBoxColumn2.Visible = true;
        //    this.dataGridViewTextBoxColumn2.Width = 146;
        //    this.LeftPanel.Size = new System.Drawing.Size(900, 427);
        //    this.FLDCNTLGrid.Size = new System.Drawing.Size(885, 366);
        //    this.BtnUpdate.Location = new System.Drawing.Point(670, 399);
        //    this.CbFldValidation.Location = new System.Drawing.Point(608, 7);
        //    this.CbEnableAll.Location = new System.Drawing.Point(710, 7);
        //    this.CbShareAll.Location = new System.Drawing.Point(816, 7);
        //}



        private void PbFLDLeft_Click(object sender, EventArgs e)
        {
            PbFLDLeft.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 250;//250;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.CbFldValidation.Location = new System.Drawing.Point(200, 3);
            this.CbEnableAll.Location = new System.Drawing.Point(318, 3);
            this.CbShareAll.Location = new System.Drawing.Point(434, 3);
            this.dataGridViewTextBoxColumn2.Visible = false;
            this.FLDCNTLGrid.Size = new System.Drawing.Size(535, 460);
            this.LeftPanel.Size = new System.Drawing.Size(535, 487);
            //this.LeftPanel.SendToBack();
            this.pnlBtnUpdate.Padding = new Padding(5, 5, 545, 5);
            // this.BtnUpdate.Location = new System.Drawing.Point(224, 399);
            PbFLDRight.Visible = true;
            this.dataGridViewTextBoxColumn2.ShowInVisibilityMenu = false;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.Width = 270;//250;
        }


        //private void UpdateCUSTRESPTable()
        //{
        //    CaptainModel model = new CaptainModel();
        //    foreach (DataGridViewRow dr in CustRespGrid.Rows)
        //    {
        //        if (!(string.IsNullOrEmpty(dr.Cells["RespCode"].FormattedValue.ToString())))
        //        {
        //            dr.Cells["RespCode"].Value.ToString();

        //        }


        //        if (Type == "Sequence")
        //            CurRowCode = SetLeadingZeros(dr.Cells["Sequence"].Value.ToString());
        //        else
        //            CurRowCode = dr.Cells["Code"].Value.ToString();

        //        if (CmpStr == CurRowCode)
        //        {
        //            CmpResult = true;
        //            switch (Type)
        //            {
        //                case "Sequence": txtSeq.Clear(); break;
        //                default: TxtCode.Clear(); break;
        //            }
        //            break;
        //        }
        //    }
        //    model.FieldControls.UpdateCUSTRESP(OrgCustResp);
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    string filePath = @"E:\Field Control SQL_Tables.pdf";
        //    string adobeReaderPath = @"C:\Program Files\Adobe\Reader 10.0\Reader\AcroRd32.exe";
        //    System.Diagnostics.Process.Start(adobeReaderPath, filePath);
        //}

        private void AddColumnsFLDGrids()
        {
            this.FLDCNTLGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Enabled,
            this.Required,
            this.Shared,
            this.Gd1Code});

            this.RemainingGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Enab,
            this.Req,
            this.Share,
            this.Gd2Code});
        }

        private void PbRemLeft_Click(object sender, EventArgs e)
        {
            PbRemLeft.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 560;
            this.dataGridViewTextBoxColumn4.Visible = true;
            this.dataGridViewTextBoxColumn4.Width = 117;
            this.RightPanel.Size = new System.Drawing.Size(900, 427);
            this.RemainingGrid.Location = new System.Drawing.Point(0, 1);
            this.RemainingGrid.Size = new System.Drawing.Size(885, 368);
            //this.BtnUpdate.Location = new System.Drawing.Point(670, 399);
            //this.CbFldValidation.Location = new System.Drawing.Point(608, 7);
            //this.CbEnableAll.Location = new System.Drawing.Point(710, 7);
            //this.CbShareAll.Location = new System.Drawing.Point(816, 7);
            //this.PbFLDLeft.Location = new System.Drawing.Point(870, 31);
            //PbFLDLeft.Visible = true;
        }

        string Copy_From_Hie = "";
        private void FieldControlForm_Load(object sender, EventArgs e)
        {
            if (!(STAT_Controls_exists) && Seltab_Tndex == 0)
            {
                string[] Name = Regex.Split(((ListItem)CmbScreen.SelectedItem).Text.ToString(), "-");
                string Scr_Name = Name[0].Trim();

                MessageBox.Show("Static Controls are Not Defined for '" + Scr_Name + "'", "CAPTAIN", MessageBoxButtons.OK);
                CbFldValidation.Visible = false;
                BtnUpdate.Visible = false;
            }

            if (Mode == "Add" && !string.IsNullOrEmpty(Get_Cntl_From_HIE.Trim()))
            {
                string Tmp_Hie_str = Get_Cntl_From_HIE, Prog_Name = "";
                if (Tmp_Hie_str.Length == 6)
                {
                    Tmp_Hie_str = Tmp_Hie_str.Substring(0, 2) + "-" + Tmp_Hie_str.Substring(2, 2) + "-" + Tmp_Hie_str.Substring(4, 2);
                    if (Get_Cntl_From_HIE != "******")
                    {
                        //DataSet Prog = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Get_Cntl_From_HIE.Substring(0, 2), Get_Cntl_From_HIE.Substring(2, 2), Get_Cntl_From_HIE.Substring(4, 2));
                        //if (Prog.Tables[0].Rows.Count > 0)
                        //    Prog_Name = "   " + (Prog.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        Prog_Name = Copy_From_Hie;
                    }
                    //else
                    //    Prog_Name = "All Hierarchies";
                }
                MessageBox.Show("Do you want to Copy Field Controls from " + Tmp_Hie_str + "  " + Prog_Name + " ...?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: this.Get_From_Sel_Hie);
            }


            if (!(CUST_Controls_exists) && Mode.Equals("Add"))
            {
                CustCntlPanel.Visible = true;
            }
        }

        private void Get_From_Sel_Hie(DialogResult dialogResult)
        {

            if (dialogResult == DialogResult.Yes)
            {
                //FillFLDCNTLGrid_Edit(Get_Cntl_From_HIE);

                CaptainModel model = new CaptainModel();
                List<FLDDESCHieEntity> Cust = model.FieldControls.GetStatCustDescByScrCodeHIE("CASE0008", ScrCode, Get_Cntl_From_HIE);
                //ChgFldcntlArr = new string[Cust.Count + 1, 4];
                if (Cust.Count > 0 && ChgFldcntlArr.Length > 4)
                {
                    OrgFldcntHieEntity = Cust;
                    foreach (FLDDESCHieEntity Entity in Cust)
                    {
                        for (int i = 0; i < (ChgFldcntlArr.Length / 4); i++)
                        {
                            if (ChgFldcntlArr[i, 3] == Entity.FldCode)
                            {
                                ChgFldcntlArr[i, 0] = Entity.Enab;
                                ChgFldcntlArr[i, 1] = Entity.Req;
                                ChgFldcntlArr[i, 2] = Entity.Shared;
                                //ChgFldcntlArr[i, 3] = Entity.FldCode;
                                break;
                            }
                        }

                        foreach (DataGridViewRow dr in FLDCNTLGrid.Rows)
                        {
                            if (Entity.FldCode == dr.Cells["Gd1Code"].Value.ToString())
                            {
                                if (Entity.Enab == "Y" || Entity.Enab == "y")
                                    dr.Cells["Enabled"].Value = true;
                                else
                                    dr.Cells["Enabled"].Value = false;

                                if (Entity.Req == "Y" || Entity.Req == "y")
                                    dr.Cells["Required"].Value = true;
                                else
                                    dr.Cells["Required"].Value = false;

                                if (Entity.Shared == "Y" || Entity.Shared == "y")
                                    dr.Cells["Shared"].Value = true;
                                else
                                    dr.Cells["Shared"].Value = false;

                                break;
                            }
                        }

                    }
                }
            }

        }


        private void FieldControlForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FLDCNTLMaintenanceControl FLDControl = BaseForm.GetBaseUserControl() as FLDCNTLMaintenanceControl;
            if (FLDControl != null)
                FLDControl.RefreshCUST(ScrCode);
        }

        private void Hepl_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Field Control Maintenance");
            // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Case 2008_Add");

        }

        private void FieldControlForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "TL_HELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps("CASE0008", 1, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
            }
        }
    }
}



