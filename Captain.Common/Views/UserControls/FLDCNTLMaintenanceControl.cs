#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class FLDCNTLMaintenanceControl : BaseUserControl
    {

        #region private variables

        private CaptainModel _model = null;
        private PrivilegesControl _screenPrivileges = null;
        private ErrorProvider _errorProvider = null;

        #endregion

        public FLDCNTLMaintenanceControl(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            InitializeComponent();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Pb_Add_Cust, "Add Custom Question");
            tooltip.SetToolTip(Pb_Edit_Cust, "Edit Custom Question");
            tooltip.SetToolTip(PbDelete, "Delete Custom Question(s)");

            txtSeq.Validator = TextBoxValidation.IntegerValidator;


            TxtEqual.Validator = TextBoxValidation.FloatValidator;
            TxtGreat.Validator = TextBoxValidation.FloatValidator;
            TxtLess.Validator = TextBoxValidation.FloatValidator;
            TxtCode.Validator = TextBoxValidation.IntegerValidator;
            FillScreenCombo();
            FillHierarchyGrid();
            FillAllCombos();
            FillCustomGrid();
            PopulateToolbar(oToolbarMnustrip);
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }
        public string propReportPath { get; set; }

        List<PreassessQuesEntity> preassessChildEntity { get; set; }
        List<RankCatgEntity> proppreassGroupsData { get; set; }

        #endregion

        bool Tab1_Loading_Complete = false;
        bool Tab2_Loading_Complete = false;
        string ScrCode = null;
        string Selmode = "View";
        string SelHier = null;
        int Hierarchy_PrivRow_Index = 0;

        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (toolBar.Controls.Count == 0)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Define Field Controls";
                ToolBarNew.Enabled = true;
                //ToolBarNew.Image = new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Field Controls";
                ToolBarEdit.Enabled = true;
                //  ToolBarEdit.Image = new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.ImageSource = "captain-edit";
                // ToolBarNew.ImageSource = Consts.Icons16x16.EditIcon;
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Field Controls";
                ToolBarDel.Enabled = true;
                // ToolBarDel.Image = new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.ToolTipText = "Field Controls Report in Excel";
                ToolBarExcel.Enabled = true;
                //ToolBarExcel.Image = new IconResourceHandle(Consts.Icons16x16.MSExcel);
                ToolBarExcel.ImageSource = "captain-excel";
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Field Controls Help";
                ToolBarHelp.Enabled = true;
                //ToolBarHelp.Image = new IconResourceHandle(Consts.Icons16x16.Help);.
                ToolBarHelp.ImageSource = "icon-help";
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }
            if (Privileges.AddPriv.Equals("false"))
                ToolBarNew.Enabled = false;
            if (Privileges.ChangePriv.Equals("false"))
                ToolBarEdit.Enabled = false;
            if (Privileges.DelPriv.Equals("false"))
                ToolBarDel.Enabled = false;



            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                ToolBarExcel,
                ToolBarHelp
            });
        }

        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();
            Hierarchy_PrivRow_Index = 0;
            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null) { return; }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:
                        if (tabcontrol1.SelectedIndex == 0)
                        {
                            FieldControlForm AddFieldControls = new FieldControlForm(BaseForm, "Add", ScrCode, GetSelectedHierarchy(string.Empty), Privileges.PrivilegeName, 0);
                            Selmode = "Add";
                            AddFieldControls.StartPosition = FormStartPosition.CenterScreen;
                            AddFieldControls.ShowDialog();
                        }
                        else if (tabcontrol1.SelectedIndex == 1)
                        {
                            Pb_Add_Cust_Click(Pb_Add_Cust, EventArgs.Empty);
                        }
                        break;
                    case Consts.ToolbarActions.Edit:
                        if (tabcontrol1.SelectedIndex == 0)
                        {
                            if (HierGrid.Rows.Count > 0)
                            {
                                FieldControlForm EditFieldControls = new FieldControlForm(BaseForm, "Edit", ScrCode, GetSelectedHierarchy("Edit"), Privileges.PrivilegeName, 0);
                                Selmode = "Edit";
                                EditFieldControls.StartPosition = FormStartPosition.CenterScreen;
                                EditFieldControls.ShowDialog();
                            }
                        }
                        else if (tabcontrol1.SelectedIndex == 1)
                        {
                            Pb_Edit_Cust_Click(Pb_Edit_Cust, EventArgs.Empty);
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (tabcontrol1.SelectedIndex == 0)
                        {
                            if (HierGrid.Rows.Count > 0)
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nField Controls for Hierarchy: " + HierGrid.CurrentRow.Cells["Hierarchy"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: this.Delete_Sel_Hierarchy);
                        }
                        else if (tabcontrol1.SelectedIndex == 1)
                        {
                            BtnDeleteCust_Click(BtnDeleteCust, EventArgs.Empty);
                        }


                        //if (_model.FieldControls.DeleteFLDCNTLHIE(ScrCode, GetSelectedHierarchy()))
                        //    MessageBox.Show("Field Controls for " + HierGrid.CurrentRow.Cells["Hierarchy"].Value.ToString() + "Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        //else
                        //    MessageBox.Show("UnSuccessful Delete", "CAPTAIN", MessageBoxButtons.OK);
                        break;
                    case Consts.ToolbarActions.Excel:
                        if (HierGrid.Rows.Count > 0)
                            On_SaveExcelForm_Closed();
                        break;

                    //if (CanDeleteHeaderCode())
                    //    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, OnDeleteMessageBoxClicked);
                    //else
                    //    MessageBox.Show("You can not Delete Table 'with Active Items' ", "CAPTAIN", MessageBoxButtons.OK);
                    //break;
                    case Consts.ToolbarActions.Help:
                        //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Case 2008");
                        if (tabcontrol1.SelectedIndex == 0)
                            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        if (tabcontrol1.SelectedIndex == 1)
                            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 2, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }



        private void Delete_Sel_Hierarchy(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                if (_model.FieldControls.DeleteFLDCNTLHIE(ScrCode, GetSelectedHierarchy(string.Empty)))
                {
                    AlertBox.Show("Field Controls for " + HierGrid.CurrentRow.Cells["Hierarchy"].Value.ToString() + "\nDeleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //  MessageBox.Show("Field Controls for " + HierGrid.CurrentRow.Cells["Hierarchy"].Value.ToString() + "Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);

                    CmbScreen_SelectedIndexChanged(CmbScreen, new EventArgs());
                }
                else
                    AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);
            }
        }

        public void Refreshdata()
        {
            FillScreenCombo();
            FillHierarchyGrid();
            FillAllCombos();
            FillCustomGrid();
        }

        private void FillHierarchyGrid()
        {
            List<FLDCNTLEntity> Cust = _model.FieldControls.GetFLDCNTLByScrCode(ScrCode, null);
            if (Cust.Count > 0)
            {
                string UserAgency = string.Empty;
                if(BaseForm.BaseAgencyuserHierarchys.Count>0)
                {
                    HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
                    if (SelHie != null)
                        UserAgency = "**";
                }

                if (BaseForm.BaseAdminAgency != "**")
                    Cust = Cust.FindAll(u => u.ScrHie.Substring(0, 2).ToString() == BaseForm.BaseAdminAgency || u.ScrHie.Substring(0, 2).ToString() == UserAgency);

                HierGrid.Rows.Clear();
                int rowIndex = 0;
                int RowCnt = 0;
                int selindex = 0;
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
                            TmpHieDesc = "Description not Defined";
                    }

                    if ((Selmode.Equals("Add") || Selmode.Equals("Edit")) && SelHier == Entity.ScrHie)
                        selindex = RowCnt;
                    TmpHie = Entity.ScrHie.Substring(0, 2) + '-' + Entity.ScrHie.Substring(2, 2) + '-' + Entity.ScrHie.Substring(4, 2);
                    rowIndex = HierGrid.Rows.Add(TmpHie, TmpHieDesc, Entity.ScrHie);
                    setFLDTooltip(rowIndex, Entity);
                    RowCnt++;
                }

                int CurrentPage = 1;
                if (RowCnt > 0)
                {
                    HierGrid.CurrentCell = HierGrid.Rows[Hierarchy_PrivRow_Index].Cells[1];
                    //CurrentPage = (Hierarchy_PrivRow_Index / HierGrid.ItemsPerPage);
                    //CurrentPage++;
                    //HierGrid.CurrentPage = CurrentPage;
                    //HierGrid.CurrentCell = HierGrid.Rows[Hierarchy_PrivRow_Index].Cells[1];
                }
                //else
                //    HierGrid.CurrentCell = HierGrid.Rows[0].Cells[1];
                //HierGrid.Rows[rowIndex].Tag = 0;
                //HierGrid.Rows[selindex].Selected = true;
                //HierGrid.Rows[selindex].Selected = true;
                Tab1_Loading_Complete = true;
                FillFLDCNTLGrid(GetSelectedHierarchy(string.Empty));
                //SelHier = null;
            }
            else
                AlertBox.Show("Field Controls are not Defined for ''" + ((ListItem)CmbScreen.SelectedItem).ID.ToString() + "'' in Any Hierarchy", MessageBoxIcon.Warning);
            Hierarchy_PrivRow_Index = 0;
        }


        private void FillCustomGrid()
        {
            CustomGrid.Rows.Clear();
            //BtnDeleteCust.Visible = false;
            PbDelete.Visible = Pb_Edit_Cust.Visible = false;

            //if (Privileges.AddPriv.Equals("true"))
            //    Pb_Add_Cust.Visible = true;

            List<CustfldsEntity> Cust = _model.FieldControls.GetCUSTFLDSByScrCode(ScrCode, "CUSTFLDS", string.Empty);
            if (Cust.Count > 0)
            {
                CustCntlPanel.Visible = true;
                int rowIndex = 0, Selection_Index = 0;
                foreach (CustfldsEntity Entity in Cust)
                {
                    string TmpType = null;
                    TmpType = setResponseType(Entity.RespType);
                    rowIndex = CustomGrid.Rows.Add(Entity.CustCode, Entity.CustDesc, TmpType, Entity.CustSeq, false, Entity.CustCode);
                    setCUSTTooltip(rowIndex, Entity);

                    if (Edited_Cust_Code == Entity.CustCode)
                        Selection_Index = rowIndex;

                    if (Entity.Active != "A")
                        CustomGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                }

                CustomGrid.CurrentCell = CustomGrid.Rows[Selection_Index].Cells[1];

                int scrollPosition = 0;
                //scrollPosition = CustomGrid.CurrentCell.RowIndex;
                //int CurrentPage = (scrollPosition / CustomGrid.ItemsPerPage);
                //CurrentPage++;
                //CustomGrid.CurrentPage = CurrentPage;
                //CustomGrid.FirstDisplayedScrollingRowIndex = scrollPosition;


                //CustomGrid.Rows[rowIndex].Tag = 0;
                Tab2_Loading_Complete = true;
                //BtnDeleteCust.Visible = true;
                //if (Privileges.DelPriv.Equals("true"))
                //    PbDelete.Visible = true;
                //else
                //  this.Delete.ReadOnly = true;

                //if (Privileges.ChangePriv.Equals("true"))
                //    Pb_Edit_Cust.Visible = true;

                FillCustomControls(GetSelectedCustomKey());
            }
            else
                CustCntlPanel.Visible = false;
        }


        private void FillScreenControlAssignmentGrid()
        {

        }

        List<FLDSCRSEntity> FLDSCRS_List = new List<FLDSCRSEntity>();
        private void FillScreenCombo()
        {
            this.CmbScreen.SelectedIndexChanged -= new System.EventHandler(this.CmbScreen_SelectedIndexChanged);
            CmbScreen.Items.Clear();

            FLDSCRSEntity Search_Entity = new FLDSCRSEntity(true);
            Search_Entity.Called_By = "CASE0008";
            FLDSCRS_List = _model.FieldControls.Browse_FLDSCRS(Search_Entity);
            string Tmp_Desc = string.Empty;
            //List<CommonEntity> PAYMENTService = _model.lookupDataAccess.GetPaymentCategoryServicee();
            //PAYMENTService = PAYMENTService.FindAll(u => u.Extension.Trim() != string.Empty);

            List<Agy_Ext_Entity> PAYMENTService = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);
            PAYMENTService = PAYMENTService.FindAll(u => u.Ext_1.Trim() != string.Empty);

            foreach (FLDSCRSEntity Entity in FLDSCRS_List)
            {
                if ((Entity.Scr_Code == "CASE2001" && Entity.Scr_Sub_Code == "00") || Entity.Scr_Code != "CASE2001")
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-30}", Entity.Scr_Desc) + String.Format("{0,8}", " - " + Entity.Scr_Code);
                    if (Entity.Scr_Code.ToUpper().Contains("PAYCAT"))
                    {
                        if (PAYMENTService.Count > 0)
                        {
                            Agy_Ext_Entity compayment = PAYMENTService.Find(u => "PAYCAT" + u.Ext_2.Trim() == Entity.Scr_Code.Trim());
                            if (compayment != null)
                            {
                                CmbScreen.Items.Add(new ListItem(Tmp_Desc, Entity.Scr_Code, Entity.Scr_Desc, Entity.Cust_Ques_SW));
                            }
                        }
                    }
                    else
                    {
                        CmbScreen.Items.Add(new ListItem(Tmp_Desc, Entity.Scr_Code, Entity.Scr_Desc, Entity.Cust_Ques_SW));
                    }
                }
            }




            //List<ListItem> listItem = new List<ListItem>();
            ////listItem.Add(new ListItem("Form Title                                    -  SCR-CODE", SCR-CODE, Form Title, Custom_Filedls_Flag));
            //listItem.Add(new ListItem("Client Intake                                    -  CASE2001", "CASE2001", "Client Intake", "Y"));
            //listItem.Add(new ListItem("Income Entry                                   -  CASINCOM", "CASINCOM", "Income Entry", "N"));
            //listItem.Add(new ListItem("Income Verification                          -  CASE2003", "CASE2003", "Income Verification", "N"));
            //listItem.Add(new ListItem("Contact Posting                               -  CASE0006", "CASE0061", "Contact Posting", "Y"));
            //listItem.Add(new ListItem("Critical Activity Posting                    -  CASE0006", "CASE0062", "Critical Activity Posting", "Y"));
            //listItem.Add(new ListItem("Milestone Posting                             -  CASE0006", "CASE0063", "Milestone Posting", "N"));
            //listItem.Add(new ListItem("Medical/Emergency                          -  CASE2330", "CASE2330", "Medical/Emergency", "N"));
            //listItem.Add(new ListItem("Track Master Maintenance               -  HSS00133", "HSS00133", "Track Master Maintenance", "Y"));
            //CmbScreen.Items.AddRange(listItem.ToArray());
            if (CmbScreen.Items.Count > 0)
                CmbScreen.SelectedIndex = 0;
            ScrCode = "CASE2001";

            if (BaseForm.BaseAgencyControlDetails.SpanishSwitch == "Y")
            {
                if (ScrCode == "CASE2001")
                {
                    btnSpanish.Visible = false;
                }
                else
                {
                    btnSpanish.Visible = false;
                }
            }

            this.CmbScreen.SelectedIndexChanged += new System.EventHandler(this.CmbScreen_SelectedIndexChanged);

        }


        private void FillAllCombos()
        {
            this.CmbType.SelectedIndexChanged -= new System.EventHandler(this.CmbType_SelectedIndexChanged);
            CmbType.Items.Clear();
            this.CmbType.SelectedIndexChanged += new System.EventHandler(this.CmbType_SelectedIndexChanged);
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            listItem.Add(new ListItem("Check Box", "C"));
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

            //List<PreassessQuesEntity> preassessquesentity = _model.FieldControls.GetPreassessData("MASTER");
            List<CommonEntity> preassessquesentity = _model.lookupDataAccess.GetDimension();
            cmbPreassDimension.Items.Clear();
            cmbPreassDimension.Items.Insert(0, new ListItem("Select One", "0"));
            //  cmbPreassDimension.ColorMember = "FavoriteColor";
            cmbPreassDimension.SelectedIndex = 0;
            foreach (CommonEntity item in preassessquesentity)
            {
                ListItem li = new ListItem(item.Desc, item.Code, item.Active, item.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbPreassDimension.Items.Add(li);
            }

            //List<CommonEntity> commonfunds = _model.lookupDataAccess.GetAgyFunds();

            //cmbFunds.Items.Insert(0, new ListItem("Select One", "0"));
            //cmbFunds.ColorMember = "FavoriteColor";
            //cmbFunds.SelectedIndex = 0;
            //foreach (CommonEntity item in commonfunds)
            //{
            //    ListItem li = new ListItem(item.Desc, item.Code, item.Active, item.Active.Equals("Y") ? Color.Green : Color.Red);
            //    cmbFunds.Items.Add(li);
            //}

            proppreassGroupsData = _model.SPAdminData.Browse_PreassGroups();

            List<RankCatgEntity> preassGroupsData = proppreassGroupsData.FindAll(u => string.IsNullOrEmpty(u.SubCode.Trim()));
            cmbGroups.Items.Insert(0, new ListItem("Select One", "0"));
            cmbGroups.SelectedIndex = 0;
            foreach (RankCatgEntity groupitem in preassGroupsData)
            {
                ListItem li = new ListItem(groupitem.Desc, groupitem.Code);
                cmbGroups.Items.Add(li);
            }

            preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
            //foreach (PreassessQuesEntity item in preassessquesentity)
            //{           
            //    cmbPreassGroup.Items.Add(new ListItem(item.PREDIMENTION_DESC, item.PREDIMENTION_ID));
            //}
            //cmbPreassGroup.Items.Insert(0, new ListItem("", "0"));
            //cmbPreassGroup.SelectedIndex = 0;

        }

        private void SetComboBoxValue(Wisej.Web.ComboBox comboBox, string value)
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


        private void CustomGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (Tab2_Loading_Complete)
                FillCustomControls(GetSelectedCustomKey());
        }

        private void FillCustomControls(string RecKey)
        {
            List<CustfldsEntity> Entity = _model.FieldControls.GetSelCustDetails(RecKey);
            if (Entity.Count > 0)
            {
                CustfldsEntity Cust = Entity[0];
                TxtCode.Text = Cust.CustCode;
                txtSeq.Text = SetLeadingZeros(Cust.CustSeq);
                TxtQuesDesc.Text = Cust.CustDesc;
                TxtAbbr.Text = Cust.Question;
                TxtType.Text = setResponseType(Cust.RespType);
                TxtAccess.Text = setMemAccess(Cust.MemAccess);
                SetComboBoxValue(CmbType, Cust.RespType);
                SetComboBoxValue(CmbAccess, Cust.MemAccess);
                if (ScrCode == "PREASSES")
                {
                    PreassessQuesEntity preassquesdata = preassessChildEntity.Find(u => u.PRECHILD_QID == Cust.CustCode);
                    if (preassquesdata != null)
                        SetComboBoxValue(cmbPreassDimension, preassquesdata.PRECHILD_DID); //.PadLeft(2,'0')
                    else
                        SetComboBoxValue(cmbPreassDimension, "0");
                    RankCatgEntity rankdata = proppreassGroupsData.Find(u => u.Code == Cust.Other && string.IsNullOrEmpty(u.SubCode.Trim()));
                    if (rankdata != null)
                        SetComboBoxValue(cmbGroups, Cust.Other);
                    else
                        SetComboBoxValue(cmbGroups, "0");


                }
                if (ScrCode == "EMS00030")
                {
                    txtFunds.Text = Cust.Other;
                    chkEmsRequired.Checked = Cust.Sub_Screen == "Y" ? true : false;
                }
                TxtEqual.Text = Cust.Equalto;
                TxtGreat.Text = Cust.Greater;
                TxtLess.Text = Cust.Less;

                CustRespPanel.Visible = NumericPanel.Visible = FurtreDtPanel.Visible = false;
                pnlSeqAbbQuesEqual.Size = new Size(714, 150); CustRespPanel.Size = new Size(424, 150); CustCntlPanel.Size = new Size(1130, 200); Size = new Size(1130, 600);
                if (Cust.FutureDate == "Y")
                    CbFDate.Checked = true;
                else
                    CbFDate.Checked = false;

                if (Cust.Active == "A")
                    CbActive.Checked = true;
                else
                    CbActive.Checked = false;

                switch (Cust.RespType)
                {
                    case "D":
                    case "C": FillCustRespGrid(RecKey); break;
                    case "N":
                        NumericPanel.Visible = true;
                        pnlSeqAbbQuesEqual.Size = new Size(714, 190); CustRespPanel.Size = new Size(424, 190); CustCntlPanel.Size = new Size(1130, 230);
                        Size = new Size(1130, 600); break;
                    case "T": FurtreDtPanel.Visible = true; break;
                }

                //Check_Answers_for_CustCode(RecKey);
                if (Cust.RespType == "D" || Cust.RespType == "C")
                    FillCustRespGrid(RecKey);
                else
                    CustRespPanel.Visible = false;

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

        private string GetSelectedHierarchy(string mode) // 08162012
        {
            string RecordKey = null;
            if (HierGrid != null)
            {
                try
                {
                    if (HierGrid.Rows.Count > 0)
                    {
                        Hierarchy_PrivRow_Index = HierGrid.CurrentCell.RowIndex;
                        RecordKey = HierGrid.CurrentRow.Cells["HieKey"].Value.ToString();
                    }
                }
                catch (Exception ex) { }
            }

            if (string.IsNullOrEmpty(RecordKey) && mode.Equals("Edit"))
            {
                if (CustomGrid.Rows.Count > 0)
                    RecordKey = "Custom_Edit";
            }

            return RecordKey;
        }

        private void HierGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (Tab1_Loading_Complete && HierGrid.Rows.Count > 0)
                FillFLDCNTLGrid(GetSelectedHierarchy(string.Empty)); //FLDCNTLGrid

        }

        private void FillFLDCNTLGrid(string Hierarchy)
        {

            FLDCNTLGrid.Rows.Clear();
            List<FLDDESCHieEntity> Cust = _model.FieldControls.GetStatCustDescByScrCodeHIE(Privileges.Program, ScrCode, Hierarchy);
            if (Cust.Count > 0)
            {
                int rowIndex = 0;
                int RowCount = 0;
                int SelectedrowIndex = 0;
                bool Enabled, Required, Shared;
                foreach (FLDDESCHieEntity Entity in Cust)
                {
                    //string enab, Req, Shr;
                    //enab = Req= Shr= "No";
                    //if (Entity.Enab == "Y" || Entity.Enab == "y")
                    //    enab = "Yes";
                    //if (Entity.Req == "Y" || Entity.Req == "y")
                    //    Req = "Yes";
                    //if (Entity.Shared == "Y" || Entity.Shared == "y")
                    //    Shr = "Yes";

                    //rowIndex = FLDCNTLGrid.Rows.Add(Entity.FldDesc, Entity.SubScr, enab, Req, Shr);

                    Enabled = Required = Shared = false;
                    if (Entity.Enab == "Y" || Entity.Enab == "y")
                        Enabled = true;
                    if (Entity.Req == "Y" || Entity.Req == "y")
                        Required = true;
                    if (Entity.Shared == "Y" || Entity.Shared == "y")
                        Shared = true;
                    rowIndex = FLDCNTLGrid.Rows.Add(Entity.FldDesc, Entity.SubScr, Enabled, Required, Shared);
                    SelectedrowIndex = rowIndex;
                    RowCount++;
                    if (Entity.FldCode.Substring(1, 1) == "A" || Entity.FldCode.Substring(1, 1) == "a")
                        FLDCNTLGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed; //Color.Peru; //Color.DarkTurquoise;

                    //FLDCNTLGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                    if (Entity.FldCode.Substring(0, 1) == "C" || Entity.FldCode.Substring(0, 1) == "c")
                        FLDCNTLGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                }
                FLDCNTLGrid.Rows[rowIndex].Tag = 0;
                //if(RowCount > 13)
                //    this.FLDCNTLGrid.Size = new System.Drawing.Size(467, 326); 
                //else
                //    this.FLDCNTLGrid.Size = new System.Drawing.Size(450, 326); 
                if (FLDCNTLGrid.Rows.Count > 0)
                    FLDCNTLGrid.Rows[0].Selected = true;
            }

        }

        DataGridViewRow PrivRow, ZeroRow_Index;
        bool CustResp_lod_complete = false;
        private void FillCustRespGrid(string CustCode)
        {
            //List<CustRespEntity> CustResp = _model.FieldControls.GetCustRespByCustCode(CustCode);
            //CustRespGrid.Rows.Clear();
            //if (CustResp.Count > 0)
            //{
            //    CustRespPanel.Visible = true;
            //    int rowIndex = 0;
            //    int RowCount = 0;
            //    foreach (CustRespEntity Entity in CustResp)
            //    {
            //        rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc);
            //        RowCount++;
            //    }
            //    CustRespGrid.Rows[rowIndex].Tag = 0;

            //    //if (RowCount > 3)
            //    //    this.CustRespGrid.Size = new System.Drawing.Size(284, 131);
            //    //else
            //    //    this.CustRespGrid.Size = new System.Drawing.Size(266, 131);

            //}


            Clear_CustRespGrid();
            OrgCustResp.Clear();
            CaptainModel model = new CaptainModel();
            List<CustRespEntity> CustResp = model.FieldControls.GetCustRespByCustCode(CustCode, "FIELDCONTROL");
            if (CustResp.Count > 0)
            {
                CustRespPanel.Visible = true;
                int rowIndex = 0;
                int RowCount = 0;
                foreach (CustRespEntity Entity in CustResp)
                {
                    //rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc);  
                    rowIndex = CustRespGrid.Rows.Add(Entity.DescCode, Entity.RespDesc.Trim(), Entity.RecType, Entity.Changed, Entity.RespSeq, "N", Entity.Points, Entity.status, Entity.RespDefault == "Y" ? true : false);
                    if (Entity.RspStatus.ToUpper() == "I")
                    {
                        CustRespGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }

                    RowCount++;
                }
                if (RowCount > 0)
                {
                    OrgCustResp = CustResp;
                    CustResp_lod_complete = true;
                    CustRespGrid.Rows[rowIndex].Tag = 0;
                    ZeroRow_Index = PrivRow = CustRespGrid.SelectedRows[0];
                }

                //if (RowCount > 3)
                //    this.CustRespGrid.Size = new System.Drawing.Size(284, 131);
                //else
                //    this.CustRespGrid.Size = new System.Drawing.Size(266, 131);

            }
            CustResp_lod_complete = true;
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
        }

        private void CmbScreen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cust_SCR_Mode != "View")
                Btn_Cancel_Click(Btn_Cancel, EventArgs.Empty);

            ScrCode = ((ListItem)CmbScreen.SelectedItem).Value.ToString();
            this.HierGrid.SelectionChanged -= new System.EventHandler(this.HierGrid_SelectionChanged);
            HierGrid.Rows.Clear();
            this.HierGrid.SelectionChanged += new System.EventHandler(this.HierGrid_SelectionChanged);
            FLDCNTLGrid.Rows.Clear();
            txtFunds.Visible = btnFunds.Visible = false;
            tabcontrol1.TabPages[0].Hidden = false;
            chkEmsRequired.Visible = false;
            if (BaseForm.BaseAgencyControlDetails.SpanishSwitch == "Y")
            {
                if (ScrCode == "CASE2001")
                {
                    btnSpanish.Visible = false;
                }
                else
                {
                    btnSpanish.Visible = false;
                }
            }
            if (ScrCode == "EMS00030")
            {
                Clear_CustRespGrid();
                OrgCustResp.Clear();
                this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
                ToolBarNew.Visible = true;
                ToolBarEdit.Visible = true;
                ToolBarDel.Visible = true;
                chkEmsRequired.Visible = true;
                if (((ListItem)CmbScreen.SelectedItem).ValueDisplayCode == "Y")
                {
                    tabcontrol1.TabPages[0].Hidden = true;
                    tabcontrol1.TabPages[1].Hidden = false;
                    CmbAccess.Visible = false;
                    cmbPreassDimension.Visible = false;
                    txtFunds.Visible = btnFunds.Visible = true;
                    cmbGroups.Visible = false;
                    lblGroup.Visible = false;
                    this.lblAccess.Location = new Point(173, 8);
                    lblAccess.Text = "Fund";
                    btnDimensions.Visible = false;
                    tabPage2.Text = "EMS service posting Definition";
                    this.RespDesc.Width = 220;
                    this.gvtPoints.Visible = false;
                    FillCustomGrid();
                    if (BaseForm.UserID != "JAKE")
                    {
                        ToolBarNew.Visible = false;
                        ToolBarEdit.Visible = false;
                        ToolBarDel.Visible = false;
                    }

                    tabcontrol1.SelectedTab.TabControl.SelectedIndex = 1;
                }

            }
            else
            {
                Hierarchy_PrivRow_Index = 0;
                FillHierarchyGrid();
                this.lblAccess.Location = new System.Drawing.Point(173, 8);
                Clear_CustRespGrid();
                OrgCustResp.Clear();
                this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
                ToolBarNew.Visible = true;
                ToolBarEdit.Visible = true;
                ToolBarDel.Visible = true;

                if (((ListItem)CmbScreen.SelectedItem).ValueDisplayCode == "Y")
                {

                    if (ScrCode == "PREASSES")
                    {
                        tabcontrol1.TabPages[1].Hidden = false;
                        CmbAccess.Visible = false;
                        cmbPreassDimension.Visible = true;
                        cmbPreassDimension.Location = new Point(250, 4);
                        cmbGroups.Visible = true;
                        lblGroup.Visible = true;
                        // txtFunds.Visible = false;
                        lblAccess.Location = new Point(173, 8);
                        lblAccess.Text = "Dimension";
                        lblAccess.Size = new Size(70, 14);
                        lblReqAccess.Location = new Point(233, 5);
                        btnDimensions.Visible = true;
                        tabPage2.Text = "Pre Assessment Definition";
                        this.RespDesc.Width = 220;
                        this.gvtPoints.Visible = true;
                        this.gvtPoints.ShowInVisibilityMenu = true;
                        this.gvtDefault.Visible = true;
                        this.gvtDefault.ShowInVisibilityMenu = true;
                        FillCustomGrid();
                        if (BaseForm.UserID != "JAKE")
                        {
                            btnDimensions.Visible = false;
                            ToolBarNew.Visible = false;
                            ToolBarEdit.Visible = false;
                            ToolBarDel.Visible = false;
                        }

                    }
                    else
                    {
                        tabcontrol1.TabPages[1].Hidden = false;
                        CmbAccess.Visible = true;
                        cmbPreassDimension.Visible = false;
                        cmbGroups.Visible = false;
                        lblGroup.Visible = false;
                        btnDimensions.Visible = false;
                        lblAccess.Location = new Point(173, 8);
                        lblAccess.Text = "Access";
                        lblAccess.Size = new Size(41, 14);
                        lblReqAccess.Location = new Point(211, 5);
                        tabPage2.Text = "Custom Fields Definition";
                        this.RespDesc.Width = 220;
                        this.gvtPoints.Visible = false;
                        this.gvtPoints.ShowInVisibilityMenu = false;
                        this.gvtDefault.Visible = false;
                        this.gvtDefault.ShowInVisibilityMenu = false;
                        FillCustomGrid();
                    }
                    //tabcontrol1.TabIndex = tabcontrol1.SelectedIndex = 0;

                }
                else
                {
                    tabcontrol1.TabPages[1].Hidden = true;
                    tabcontrol1.SelectedIndex = 0;
                }
                tabcontrol1.TabIndex = tabcontrol1.SelectedIndex;
            }
        }

        private void menuItemDel_Click(object sender, EventArgs e)
        {

        }

        public void Refresh(string ScreenCode, string Hierarchy)
        {
            SelHier = Hierarchy;
            SetComboBoxValue(CmbScreen, ScreenCode);
            HierGrid.Rows.Clear();
            FLDCNTLGrid.Rows.Clear();
            FillHierarchyGrid();
        }

        public void RefreshCUST(string ScreenCode)
        {
            SetComboBoxValue(CmbScreen, ScreenCode);
            FillCustomGrid();
        }


        private void tabcontrol1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabcontrol1.SelectedIndex == 0)
            {
                ToolBarNew.ToolTipText = "Define Field Controls";
                ToolBarEdit.ToolTipText = "Edit Field Controls";
                ToolBarDel.ToolTipText = "Delete Field Controls";
                ToolBarHelp.ToolTipText = "Field Controls Help";

                this.tabcontrol1.Size = new System.Drawing.Size(1018, 414);//474); 
                Btn_Cancel_Click(Btn_Cancel, EventArgs.Empty);
                ToolBarExcel.Visible = true;
            }
            else if (tabcontrol1.SelectedIndex == 1)
            {
                ToolBarNew.ToolTipText = "Add Custom Question";
                ToolBarEdit.ToolTipText = "Edit Custom Question";
                ToolBarDel.ToolTipText = "Delete Custom Question(s)";
                this.tabcontrol1.Size = new System.Drawing.Size(779, 414);//474); 
                ToolBarExcel.Visible = false;
            }
        }

        private void setFLDTooltip(int rowIndex, FLDCNTLEntity Entity)
        {
            string toolTipText = "Added By     : " + Entity.AddOpr.Trim() + " on " + Entity.AddDate.ToString() + "\n" +
                                 "Modified By  : " + Entity.ChgOpr.Trim() + " on " + Entity.ChgDate.ToString();

            foreach (DataGridViewCell cell in HierGrid.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        private void setCUSTTooltip(int rowIndex, CustfldsEntity Entity)
        {
            string toolTipText = "Added By     : " + Entity.AddOpr.Trim() + " on " + Entity.AddDate.ToString() + "\n" +
                                 "Modified By  : " + Entity.ChdOpr.Trim() + " on " + Entity.ChgDate.ToString();

            foreach (DataGridViewCell cell in CustomGrid.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        private void BtnDeleteCust_Click(object sender, EventArgs e)
        {
            if (CustomGrid.Rows.Count > 0)
            {
                bool Can_delete = false, Tmptrue = true;
                foreach (DataGridViewRow dr in CustomGrid.Rows)
                {
                    if (dr.Cells["Delete"].Value.ToString() == Tmptrue.ToString())
                        Can_delete = true;
                }
                if (Can_delete)
                {
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nSelected Custom Question(s) ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Sel_CustQues);
                }
                else
                    AlertBox.Show("Please select atleast One Custom Question to Delete", MessageBoxIcon.Warning);
            }
        }

        private void Delete_Sel_CustQues(DialogResult dialogResult)
        {
            bool Tmptrue = true, Delete_Statue = true, Any_Rec_deleted = false;
            // Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            if (dialogResult == DialogResult.Yes)
            {
                foreach (DataGridViewRow dr in CustomGrid.Rows)
                {
                    if (dr.Cells["Delete"].Value.ToString() == Tmptrue.ToString())
                    {
                        if (!(_model.FieldControls.DeleteCUSTFLDS(ScrCode + dr.Cells["CustomKey"].Value.ToString(), ScrCode)))
                            Delete_Statue = false;

                        Any_Rec_deleted = true;
                    }
                }
                if (Any_Rec_deleted)
                {
                    if (Delete_Statue)
                        AlertBox.Show("Selected Custom Question(s) Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    // MessageBox.Show("Selected Custom Question(s) Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    else
                        AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);

                    Edited_Cust_Code = string.Empty;
                    FillCustomGrid();
                }
            }
            // }
        }


        private void CustomGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CustomGrid.Rows.Count > 0 && e.ColumnIndex != -1)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 4)
                    {
                        string FldCode = CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString();
                        DataSet ds = Captain.DatabaseLayer.FieldControlsDB.Browse_FLDCNTLHIE(ScrCode, null, FldCode, null, null, null, null);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (ScrCode == "PREASSES")
                            {
                                AlertBox.Show("'" + FldCode + "' can not be Deleted.\n This Question is already is being answered.", MessageBoxIcon.Warning);
                            }
                            else
                            {
                                AlertBox.Show("'" + FldCode + "' can not be Deleted.\n This Custom Field is being used in another screen.", MessageBoxIcon.Warning);
                            }
                            CustomGrid.CurrentRow.Cells["Delete"].Value = false;
                        }
                    }
                }
            }
        }


        private void Clear_Cust_Controls()
        {
            txtSeq.Clear();
            TxtEqual.Clear();
            TxtGreat.Clear();
            TxtLess.Clear();
            TxtCode.Clear();
            TxtQuesDesc.Clear();
            TxtAbbr.Clear();
            CbActive.Checked = true;
            Clear_CustRespGrid();
            OrgCustResp.Clear();
            chkEmsRequired.Checked = false;

            SetComboBoxValue(CmbType, "0");
            SetComboBoxValue(CmbAccess, "0");
            txtFunds.Clear();
            SetComboBoxValue(cmbPreassDimension, "0");
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
        }

        private void Clear_CustRespGrid()
        {
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
        }

        private void Enable_Disable_Cust_Controls(bool Enable_SW)
        {
            if (!Enable_SW)
            {
                Cust_SCR_Mode = "View";
                label13.Visible = false;
                _errorProvider.SetError(label13, null);
                _errorProvider.SetError(TxtQuesDesc, null);
                _errorProvider.SetError(CmbAccess, null);
                _errorProvider.SetError(CmbType, null);
                _errorProvider.SetError(txtSeq, null);
                _errorProvider.SetError(cmbPreassDimension, null);
                _errorProvider.SetError(cmbGroups, null);
                _errorProvider.SetError(txtFunds, null);
                cmbPreassDimension.Enabled = false;
                cmbPreassDimension.Location = new Point(250, 4);
                txtFunds.Enabled = btnFunds.Enabled = false;


            }

            Btn_Cancel.Visible = Btn_Save.Visible = Enable_SW;
            txtSeq.Enabled = TxtEqual.Enabled = TxtGreat.Enabled = TxtLess.Enabled =
            TxtCode.Enabled = TxtQuesDesc.Enabled = TxtAbbr.Enabled = CbActive.Enabled =
            CmbAccess.Enabled = CmbType.Enabled = CbFDate.Enabled = cmbGroups.Enabled = btnFunds.Enabled = chkEmsRequired.Enabled = Enable_SW;

            this.CustRespGrid.AllowUserToAddRows = Enable_SW;
            this.RespCode.ReadOnly = this.RespDesc.ReadOnly = this.gvtPoints.ReadOnly = this.gvtDefault.ReadOnly = !Enable_SW;


            if (Enable_SW)
                CustomGrid.Enabled = CustomGrid.Enabled = !Enable_SW; // PbDelete.Visible = Pb_Add_Cust.Visible = Pb_Edit_Cust.Visible = 
            else
                CustomGrid.Enabled = !Enable_SW;

        }

        string Cust_SCR_Mode = "View";
        private void Pb_Add_Cust_Click(object sender, EventArgs e)
        {
            if (CustomGrid.Rows.Count == 0)
                CustCntlPanel.Visible = true;

            txtFunds.Enabled = btnFunds.Enabled = cmbPreassDimension.Enabled = true; cmbGroups.Enabled = true;
            Btn_Save.Visible = Btn_Cancel.Visible = true;
            btnPrint.Visible = false;
            Enable_Disable_Cust_Controls(true);
            Clear_Cust_Controls();
            Cust_SCR_Mode = "Add";

            //FieldControlForm AddFieldControls = new FieldControlForm(BaseForm, "Add", ScrCode, GetSelectedHierarchy(string.Empty), Privileges.PrivilegeName, tabcontrol1.SelectedIndex);
            //Selmode = "Add";
            //AddFieldControls.ShowDialog();

        }

        private void Pb_Edit_Cust_Click(object sender, EventArgs e)
        {
            if (CustomGrid.Rows.Count > 0)
            {
                if (Cust_SCR_Mode == "Add")
                    CustomGrid_SelectionChanged(CustomGrid, EventArgs.Empty);

                Btn_Save.Visible = Btn_Cancel.Visible = true;
                btnPrint.Visible = false;
                Enable_Disable_Cust_Controls(true);
                Check_Answers_for_CustCode(CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString());
                Cust_SCR_Mode = "Edit";
                List<PreassessQuesEntity> PreassQuesData = _model.FieldControls.GetPreassessData(CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString());
                if (PreassQuesData.Count > 0)
                {
                    cmbPreassDimension.Enabled = false;
                    cmbPreassDimension.Location = new Point(250, 4);
                }
                else
                {
                    cmbPreassDimension.Enabled = true;
                    cmbPreassDimension.Location = new Point(250, 4);
                }
                //FieldControlForm EditFieldControls = new FieldControlForm(BaseForm, "Edit", ScrCode, GetSelectedHierarchy("Edit"), Privileges.PrivilegeName, tabcontrol1.SelectedIndex);
                //Selmode = "Edit";
                //EditFieldControls.ShowDialog();
            }
        }

        int Cust_PrivRow_Index = 0;
        string Edited_Cust_Code = string.Empty;
        List<CustRespEntity> OrgCustResp = new List<CustRespEntity>();
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (CustomGrid.RowCount > 0)
                Cust_PrivRow_Index = CustomGrid.CurrentCell.RowIndex;
            if (ValidateCustControls())
            {
                CaptainModel model = new CaptainModel();
                CustfldsEntity CustDetails = new CustfldsEntity();
                if (Cust_SCR_Mode == "Edit")
                {
                    List<CustfldsEntity> Entity = model.FieldControls.GetSelCustDetails(GetSelectedCustomKey());
                    CustDetails = Entity[0];
                    CustDetails.UpdateType = "U";
                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.CustSeq = SetLeadingZeros(txtSeq.Text);
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Question = TxtAbbr.Text;
                    CustDetails.RespType = ((ListItem)CmbType.SelectedItem).Value.ToString();
                    CustDetails.MemAccess = ((ListItem)CmbAccess.SelectedItem).Value.ToString();
                    CustDetails.CustSeq = txtSeq.Text;
                    CustDetails.Equalto = CustDetails.Greater = CustDetails.Less = "0";

                    CustDetails.Active = "I";
                    if (CbActive.Checked)
                        CustDetails.Active = "A";


                    CustDetails.ChdOpr = BaseForm.UserID;
                }
                else
                {
                    CustDetails.UpdateType = "I";
                    CustDetails.Alpha = null;
                    CustDetails.Other = null;
                    CustDetails.ScrCode = ScrCode;
                    CustDetails.CustCode = "C" + TxtCode.Text;
                    CustDetails.CustSeq = txtSeq.Text;
                    CustDetails.CustDesc = TxtQuesDesc.Text;
                    CustDetails.Question = TxtAbbr.Text;
                    CustDetails.RespType = ((ListItem)CmbType.SelectedItem).Value.ToString(); ;
                    if ((ScrCode != "PREASSES") && (ScrCode != "EMS00030"))
                    {
                        CustDetails.MemAccess = ((ListItem)CmbAccess.SelectedItem).Value.ToString();
                    }
                    else
                    {
                        CustDetails.MemAccess = "A";
                    }

                    CustDetails.CustSeq = txtSeq.Text;
                    CustDetails.Equalto = CustDetails.Greater = CustDetails.Less = "0";

                    CustDetails.FutureDate = "N";
                    CustDetails.Active = "I";
                    if (CbActive.Checked)
                        CustDetails.Active = "A";


                    CustDetails.AddOpr = BaseForm.UserID;
                    CustDetails.ChdOpr = BaseForm.UserID;
                }
                switch (((ListItem)CmbType.SelectedItem).Value.ToString())
                {
                    case "C":
                    case "D":
                        FurtreDtPanel.Visible = NumericPanel.Visible = false; pnlSeqAbbQuesEqual.Size = new Size(714, 150); CustRespPanel.Size = new Size(424, 150); CustCntlPanel.Size = new Size(1130, 200); Size = new Size(1130, 600); break;
                    case "N":
                        if (!(string.IsNullOrEmpty(TxtEqual.Text)))
                            CustDetails.Equalto = TxtEqual.Text;
                        if (!(string.IsNullOrEmpty(TxtGreat.Text)))
                            CustDetails.Greater = TxtGreat.Text;
                        if (!(string.IsNullOrEmpty(TxtLess.Text)))
                            CustDetails.Less = TxtLess.Text;
                        break;
                    case "T":
                        if (CbFDate.Checked)
                            CustDetails.FutureDate = "Y"; break;
                }

                if (ScrCode == "PREASSES")
                {
                    CustDetails.Other = ((ListItem)cmbGroups.SelectedItem).Value.ToString();
                }
                if (ScrCode == "EMS00030")
                {
                    CustDetails.Other = txtFunds.Text;
                    CustDetails.Sub_Screen = chkEmsRequired.Checked == true ? "Y" : "N";
                }
                string New_CUST_Code_Code = "NewCustCode";

                if (model.FieldControls.UpdateCUSTFLDS(CustDetails, out New_CUST_Code_Code))
                {
                    if (CustDetails.UpdateType == "U")
                    {
                        if (ScrCode == "PREASSES")
                        {
                            AlertBox.Show("Pre-Assessment Question Updated Successfully");
                            //MessageBox.Show("Pre-Assessment question Updated Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        }
                        else if (ScrCode == "EMS00030")
                        {
                            AlertBox.Show("EMS service posting Updated Successfully");
                            //MessageBox.Show("EMS service posting Updated Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        }
                        else
                            AlertBox.Show("Custom Question Updated Successfully");
                        // MessageBox.Show("Custom Question Updated Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    }
                    else
                    {
                        if (ScrCode == "PREASSES")
                        {
                            AlertBox.Show("Pre-Assessment Question Inserted Successfully");
                            //MessageBox.Show("Pre-Assessment question Inserted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        }
                        else if (ScrCode == "EMS00030")
                        {
                            AlertBox.Show("EMS service posting Inserted Successfully");
                            // MessageBox.Show("EMS service posting Inserted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        }
                        else
                        {
                            AlertBox.Show("Custom Question Inserted Successfully");
                            // MessageBox.Show("Custom Question Inserted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        }

                        Cust_PrivRow_Index = CustomGrid.Rows.Count;
                    }
                    Edited_Cust_Code = New_CUST_Code_Code;
                    if (ScrCode == "PREASSES")
                    {
                        _model.FieldControls.InsertUpdatePrechild(New_CUST_Code_Code, ((ListItem)cmbPreassDimension.SelectedItem).Value.ToString(), string.Empty, string.Empty, txtSeq.Text, string.Empty, string.Empty, "ADD");
                        preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
                    }

                    //CustResp_lod_complete = Tab2_Loading_Complete = false;
                    Tab2_Loading_Complete = false;
                    if (CustDetails.RespType == "C" || CustDetails.RespType == "D")
                        model.FieldControls.UpdateCUSTRESP(OrgCustResp, New_CUST_Code_Code);
                    Cust_SCR_Mode = "View";
                    FillCustomGrid();

                    Btn_Save.Visible = Btn_Cancel.Visible = false;
                    btnPrint.Visible = true;
                    //if (Privileges.DelPriv.Equals("true"))
                    //    PbDelete.Visible = true;

                    //if (Privileges.AddPriv.Equals("true"))
                    //    Pb_Add_Cust.Visible = true;

                    //if (Privileges.ChangePriv.Equals("true"))
                    //    Pb_Edit_Cust.Visible = true;

                    Enable_Disable_Cust_Controls(false);
                }
                else
                {
                    if (CustDetails.UpdateType == "U")
                        AlertBox.Show("Failed to Update Record", MessageBoxIcon.Warning);
                    else
                        AlertBox.Show("Failed to Insert Record", MessageBoxIcon.Warning);
                }
            }
        }



        private bool ValidateCustControls()
        {
            bool isValid = true;

            if ((String.IsNullOrEmpty(txtSeq.Text)))
            {
                _errorProvider.SetError(txtSeq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSeq.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtSeq, null);


            if (((ListItem)CmbType.SelectedItem).Value.ToString().Equals("0"))
            {
                _errorProvider.SetError(CmbType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label4.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbType, null);

            if (ScrCode == "PREASSES")
            {
                if (((ListItem)cmbPreassDimension.SelectedItem).Value.ToString().Equals("0"))
                {
                    _errorProvider.SetError(cmbPreassDimension, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccess.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbPreassDimension, null);
            }
            else if (ScrCode == "EMS00030")
            {
                if ((String.IsNullOrEmpty(txtFunds.Text)))
                {
                    _errorProvider.SetError(txtFunds, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccess.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtFunds, null);
            }
            else
            {
                if (((ListItem)CmbAccess.SelectedItem).Value.ToString().Equals("0"))
                {
                    _errorProvider.SetError(CmbAccess, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccess.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbAccess, null);
            }
            if (ScrCode == "PREASSES")
            {
                if (((ListItem)cmbGroups.SelectedItem).Value.ToString().Equals("0"))
                {
                    _errorProvider.SetError(cmbGroups, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGroup.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbGroups, null);
            }

            if (string.IsNullOrEmpty(TxtQuesDesc.Text.Trim()))
            {
                _errorProvider.SetError(TxtQuesDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQues.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtQuesDesc, null);


            if (!(CustRespGrid.Rows.Count > 1) && (((ListItem)CmbType.SelectedItem).Value.ToString().Equals("C") || ((ListItem)CmbType.SelectedItem).Value.ToString().Equals("D")))
            {
                label13.Text = " "; label13.Visible = true;
                _errorProvider.SetError(label13, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Response Grid".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                label13.Visible = false;
                _errorProvider.SetError(label13, null);
            }

            return isValid;
        }

        private void CustRespGrid_SelectionChanged(object sender, EventArgs e)
        {
            bool CanSave = true, Delete_SW = false;

            //if (PrivRow == null)
            //    PrivRow.Tag = 0; 
            //DataGridViewRow row = CustRespGrid.SelectedRows[0];
            if (CustRespGrid.Rows.Count > 0)
            {
                if (Cust_SCR_Mode != "View")
                {
                    if (CustResp_lod_complete && PrivRow != null && PrivRow.Index >= 0)
                    {
                        if (PrivRow.Cells["RespCode"].Value != null && PrivRow.Cells["RespDesc"].Value != null)
                        {
                            if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) ||
                                string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()))
                            {
                                if ((PrivRow.Cells["gvtExistData"].Value == null ? string.Empty : PrivRow.Cells["gvtExistData"].Value.ToString()) != "Y")
                                {

                                    if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) &&
                                        string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
                                        !string.IsNullOrEmpty(PrivRow.Cells["Empty_Row"].EditedFormattedValue.ToString()))
                                    {
                                        AlertBox.Show("This Response will be Deleted", MessageBoxIcon.Warning);
                                        Delete_SW = true;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("Please Fill 'Code' and 'Description'", "CAPTAIN", MessageBoxButtons.OK); 
                                        CanSave = false;
                                    }
                                }
                                else
                                {
                                    Delete_SW = true;
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
                                                if (Entity.status == "Y")
                                                {
                                                    if (Entity.DescCode != PrivRow.Cells["RespCode"].Value)
                                                    {
                                                        PrivRow.Cells["RespCode"].Value = Entity.DescCode;
                                                        AlertBox.Show("This code is already used", MessageBoxIcon.Warning);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                                        if (ScrCode == "PREASSES")
                                                        {
                                                            Entity.Points = PrivRow.Cells["gvtPoints"].Value == null ? string.Empty : PrivRow.Cells["gvtPoints"].Value.ToString();
                                                            Entity.RespDefault = PrivRow.Cells["gvtDefault"].Value == null ? "N" : PrivRow.Cells["gvtDefault"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Entity.Points = string.Empty;
                                                    Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                                                    Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                                    if (ScrCode == "PREASSES")
                                                    {
                                                        Entity.Points = PrivRow.Cells["gvtPoints"].Value == null ? string.Empty : PrivRow.Cells["gvtPoints"].Value.ToString();
                                                        Entity.RespDefault = PrivRow.Cells["gvtDefault"].Value == null ? "N" : PrivRow.Cells["gvtDefault"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Entity.status == "Y")
                                                {
                                                    PrivRow.Cells["RespCode"].Value = Entity.DescCode;
                                                    PrivRow.Cells["RespDesc"].Value = Entity.RespDesc;
                                                    if (ScrCode == "PREASSES")
                                                    {
                                                        PrivRow.Cells["gvtPoints"].Value = Entity.Points;
                                                        PrivRow.Cells["gvtDefault"].Value = Entity.RespDefault == "Y" ? true : false;
                                                    }
                                                }
                                                this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);

                                                //if (!CustRespGrid.Rows[this.CustRespGrid.SelectedRows[0].Index].IsNewRow)

                                                if (PrivRow.Index == 0 && CustRespGrid.Rows.Count == 2)
                                                {
                                                    CustRespGrid.Rows[PrivRow.Index].ReadOnly = true;
                                                    //CustRespGrid.CurrentCell = CustRespGrid.Rows[1].Cells[0];
                                                    //PrivRow = CustRespGrid.Rows[1];
                                                    //CustRespGrid.Rows.RemoveAt(0);
                                                }
                                                else
                                                {
                                                    if (Entity.status == "Y")
                                                    {
                                                        AlertBox.Show("This code is already used", MessageBoxIcon.Warning);
                                                    }
                                                    else
                                                    {
                                                        CustRespGrid.Rows.RemoveAt(PrivRow.Index);
                                                        Entity.RecType = "D";
                                                    }
                                                }

                                                //CustRespGrid.Rows.RemoveAt(this.CustRespGrid.SelectedRows[0].Index);

                                                this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
                                                CustRespGrid.Rows[CustRespGrid.CurrentRow.Index].Update();

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

                                        int Next_Seq = 0, Existing_Seq = 0;
                                        foreach (CustRespEntity ent in OrgCustResp)
                                        {
                                            if (!string.IsNullOrEmpty(ent.RespSeq.Trim()))
                                            {
                                                Existing_Seq = int.Parse(ent.RespSeq.Trim());
                                                if (Existing_Seq > Next_Seq)
                                                {
                                                    Next_Seq = Existing_Seq;
                                                }
                                            }
                                        }
                                        Next_Seq++;
                                        //PrivRow.Cells["CustSeq"].Value = (OrgCustResp.Count + 1).ToString();
                                        PrivRow.Cells["CustSeq"].Value = Next_Seq.ToString();

                                        CustRespEntity New_Entity = new CustRespEntity();
                                        New_Entity.RecType = "I";
                                        New_Entity.ScrCode = ScrCode;
                                        if (Cust_SCR_Mode.Equals("Edit"))
                                            New_Entity.ResoCode = CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString();
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(TxtCode.Text.Trim()))
                                                New_Entity.ResoCode = "C" + TxtCode.Text.Trim();
                                        }
                                        //New_Entity.RespSeq = (OrgCustResp.Count + 1).ToString();
                                        New_Entity.RespSeq = Next_Seq.ToString();
                                        New_Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                        New_Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                                        New_Entity.AddOpr = BaseForm.UserID;
                                        New_Entity.ChgOpr = BaseForm.UserID;
                                        New_Entity.Changed = "Y";
                                        New_Entity.Points = string.Empty;
                                        if (ScrCode == "PREASSES")
                                        {

                                            New_Entity.Points = PrivRow.Cells["gvtPoints"].Value == null ? string.Empty : PrivRow.Cells["gvtPoints"].Value.ToString();
                                            New_Entity.RespDefault = PrivRow.Cells["gvtDefault"].Value == null ? "N" : PrivRow.Cells["gvtDefault"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                                        }

                                        OrgCustResp.Add(new CustRespEntity(New_Entity));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) ||
                                string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()))
                            {
                                if ((PrivRow.Cells["gvtExistData"].Value == null ? string.Empty : PrivRow.Cells["gvtExistData"].Value.ToString()) != "Y")
                                {

                                    if (string.IsNullOrEmpty(PrivRow.Cells["RespCode"].EditedFormattedValue.ToString()) &&
                                        string.IsNullOrEmpty(PrivRow.Cells["RespDesc"].EditedFormattedValue.ToString()) &&
                                        !string.IsNullOrEmpty(PrivRow.Cells["Empty_Row"].EditedFormattedValue.ToString()))
                                    {
                                        AlertBox.Show("This Response will be Deleted", MessageBoxIcon.Warning);
                                        Delete_SW = true;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("Please Fill 'Code' and 'Description'", "CAPTAIN", MessageBoxButtons.OK); 
                                        CanSave = false;
                                    }
                                }
                                else
                                {
                                    Delete_SW = true;
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
                                                if (Entity.status == "Y")
                                                {
                                                    if (Entity.DescCode != PrivRow.Cells["RespCode"].Value)
                                                    {
                                                        PrivRow.Cells["RespCode"].Value = Entity.DescCode;
                                                        AlertBox.Show("This code is already used", MessageBoxIcon.Warning);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                                        if (ScrCode == "PREASSES")
                                                        {
                                                            Entity.Points = PrivRow.Cells["gvtPoints"].Value == null ? string.Empty : PrivRow.Cells["gvtPoints"].Value.ToString();
                                                            Entity.RespDefault = PrivRow.Cells["gvtDefault"].Value == null ? "N" : PrivRow.Cells["gvtDefault"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Entity.Points = string.Empty;
                                                    Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                                                    Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                                    if (ScrCode == "PREASSES")
                                                    {
                                                        Entity.Points = PrivRow.Cells["gvtPoints"].Value == null ? string.Empty : PrivRow.Cells["gvtPoints"].Value.ToString();
                                                        Entity.RespDefault = PrivRow.Cells["gvtDefault"].Value == null ? "N" : PrivRow.Cells["gvtDefault"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Entity.status == "Y")
                                                {
                                                    PrivRow.Cells["RespCode"].Value = Entity.DescCode;
                                                    PrivRow.Cells["RespDesc"].Value = Entity.RespDesc;
                                                    if (ScrCode == "PREASSES")
                                                    {
                                                        PrivRow.Cells["gvtPoints"].Value = Entity.Points;
                                                        PrivRow.Cells["gvtDefault"].Value = Entity.RespDefault == "Y" ? true : false;
                                                    }
                                                }
                                                this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);

                                                //if (!CustRespGrid.Rows[this.CustRespGrid.SelectedRows[0].Index].IsNewRow)

                                                if (PrivRow.Index == 0 && CustRespGrid.Rows.Count == 2)
                                                {
                                                    CustRespGrid.Rows[PrivRow.Index].ReadOnly = true;
                                                    //CustRespGrid.CurrentCell = CustRespGrid.Rows[1].Cells[0];
                                                    //PrivRow = CustRespGrid.Rows[1];
                                                    //CustRespGrid.Rows.RemoveAt(0);
                                                }
                                                else
                                                {
                                                    if (Entity.status == "Y")
                                                    {
                                                        AlertBox.Show("This code is already used", MessageBoxIcon.Warning);
                                                    }
                                                    else
                                                    {
                                                        CustRespGrid.Rows.RemoveAt(PrivRow.Index);
                                                        Entity.RecType = "D";
                                                    }
                                                }

                                                //CustRespGrid.Rows.RemoveAt(this.CustRespGrid.SelectedRows[0].Index);

                                                this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
                                                CustRespGrid.Rows[CustRespGrid.CurrentRow.Index].Update();

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

                                        int Next_Seq = 0, Existing_Seq = 0;
                                        foreach (CustRespEntity ent in OrgCustResp)
                                        {
                                            if (!string.IsNullOrEmpty(ent.RespSeq.Trim()))
                                            {
                                                Existing_Seq = int.Parse(ent.RespSeq.Trim());
                                                if (Existing_Seq > Next_Seq)
                                                {
                                                    Next_Seq = Existing_Seq;
                                                }
                                            }
                                        }
                                        Next_Seq++;
                                        //PrivRow.Cells["CustSeq"].Value = (OrgCustResp.Count + 1).ToString();
                                        PrivRow.Cells["CustSeq"].Value = Next_Seq.ToString();

                                        CustRespEntity New_Entity = new CustRespEntity();
                                        New_Entity.RecType = "I";
                                        New_Entity.ScrCode = ScrCode;
                                        if (Cust_SCR_Mode.Equals("Edit"))
                                            New_Entity.ResoCode = CustomGrid.CurrentRow.Cells["CustomKey"].Value.ToString();
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(TxtCode.Text.Trim()))
                                                New_Entity.ResoCode = "C" + TxtCode.Text.Trim();
                                        }
                                        //New_Entity.RespSeq = (OrgCustResp.Count + 1).ToString();
                                        New_Entity.RespSeq = Next_Seq.ToString();
                                        New_Entity.RespDesc = PrivRow.Cells["RespDesc"].Value.ToString();
                                        New_Entity.DescCode = PrivRow.Cells["RespCode"].Value.ToString();
                                        New_Entity.AddOpr = BaseForm.UserID;
                                        New_Entity.ChgOpr = BaseForm.UserID;
                                        New_Entity.Changed = "Y";
                                        New_Entity.Points = string.Empty;
                                        if (ScrCode == "PREASSES")
                                        {

                                            New_Entity.Points = PrivRow.Cells["gvtPoints"].Value == null ? string.Empty : PrivRow.Cells["gvtPoints"].Value.ToString();
                                            New_Entity.RespDefault = PrivRow.Cells["gvtDefault"].Value == null ? "N" : PrivRow.Cells["gvtDefault"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                                        }

                                        OrgCustResp.Add(new CustRespEntity(New_Entity));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //try
            //{
            if (CustRespGrid.Rows.Count >= 0)
            {
                //DataGridViewRow row = CustRespGrid.SelectedRows[0];
                DataGridViewRow row = CustRespGrid.CurrentRow;
                PrivRow = row;
            }
            //}
            //catch (Exception ex)
            //{
            //    PrivRow = ZeroRow_Index;
            //}
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
                        AlertBox.Show("Response Code '" + "'" + TmpSelCode + "' already exists", MessageBoxIcon.Warning);
                        break;
                    }
                    if (TmpSelDesc == TestDesc && PrivRow != dr)
                    {
                        ReturnVal = false;
                        AlertBox.Show("Code Description '" + TmpSelDesc + "' already exists", MessageBoxIcon.Warning);
                        break;
                    }
                }
            }
            return ReturnVal;
        }


        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Cust_SCR_Mode = "View";
            //if (Privileges.DelPriv.Equals("true"))
            //    PbDelete.Visible = true;

            //if (Privileges.AddPriv.Equals("true"))
            //    Pb_Add_Cust.Visible = true;

            //if (Privileges.ChangePriv.Equals("true"))
            //    Pb_Edit_Cust.Visible = true;
            btnPrint.Visible = true;
            Enable_Disable_Cust_Controls(false);

            if (CustomGrid.Rows.Count > 0)
                CustomGrid_SelectionChanged(CustomGrid, EventArgs.Empty);
            else
                CustCntlPanel.Visible = false;

        }

        string RespType = "0";
        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RespType = ((ListItem)CmbType.SelectedItem).Value.ToString();
            _errorProvider.SetError(label13, null);
            this.CustRespGrid.SelectionChanged -= new System.EventHandler(this.CustRespGrid_SelectionChanged);
            CustRespGrid.Rows.Clear();
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            if (!((ListItem)CmbType.SelectedItem).Value.ToString().Equals("0"))
                _errorProvider.SetError(CmbType, null);

            switch (RespType)
            {
                case "C":
                case "D":
                    FurtreDtPanel.Visible = false;
                    NumericPanel.Visible = false;
                    pnlSeqAbbQuesEqual.Size = new Size(714, 150); CustRespPanel.Size = new Size(424, 150); CustCntlPanel.Size = new Size(1130, 200);
                    this.Size = new Size(1130, 600);
                    CustRespPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(474, 147);
                    break;
                case "N":
                    CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = false;
                    NumericPanel.Visible = true;
                    pnlSeqAbbQuesEqual.Size = new Size(714, 190); CustRespPanel.Size = new Size(424, 190); CustCntlPanel.Size = new Size(1130, 230);
                    Size = new Size(1130, 600);
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
                case "T":
                    NumericPanel.Visible = false; pnlSeqAbbQuesEqual.Size = new Size(714, 150); CustRespPanel.Size = new Size(424, 150); CustCntlPanel.Size = new Size(1130, 200); Size = new Size(1130, 600);
                    CustRespPanel.Visible = false;
                    FurtreDtPanel.Visible = true;
                    //this.BtnAddCust.Location = new System.Drawing.Point(476, 81);
                    break;
                default:
                    NumericPanel.Visible = false; pnlSeqAbbQuesEqual.Size = new Size(714, 150); CustRespPanel.Size = new Size(424, 150); CustCntlPanel.Size = new Size(1130, 200); Size = new Size(1130, 600);
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

        private void btnDimensions_Click(object sender, EventArgs e)
        {

            PreassesDimentionForm preassdimetion = new PreassesDimentionForm(BaseForm);
            preassdimetion.StartPosition = FormStartPosition.CenterScreen;
            preassdimetion.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (CustomGrid.Rows.Count > 0)
            {
                if (CustomGrid.SelectedRows[0].Selected)
                {
                    string strRespType = string.Empty;
                    if (CustomGrid.SelectedRows[0].Cells["dataGridViewTextBoxColumn8"].Value.ToString().Trim().ToUpper() == "TEXT")
                        strRespType = "X";
                    if (CustomGrid.SelectedRows[0].Cells["dataGridViewTextBoxColumn8"].Value.ToString().Trim().ToUpper() == "CHECK BOX")
                        strRespType = "C";
                    if (CustomGrid.SelectedRows[0].Cells["dataGridViewTextBoxColumn8"].Value.ToString().Trim().ToUpper() == "DATE")
                        strRespType = "T";
                    if (CustomGrid.SelectedRows[0].Cells["dataGridViewTextBoxColumn8"].Value.ToString().Trim().ToUpper() == "DROP DOWN")
                        strRespType = "D";
                    if (CustomGrid.SelectedRows[0].Cells["dataGridViewTextBoxColumn8"].Value.ToString().Trim().ToUpper() == "NUMERIC")
                        strRespType = "N";

                    FldcntlRespDetails form = new FldcntlRespDetails(TxtCode.Text, TxtQuesDesc.Text, strRespType, BaseForm, ((ListItem)CmbScreen.SelectedItem).Value.ToString());
                    form.FormClosed += new FormClosedEventHandler(form_FormClosed);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                }

            }
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Tab2_Loading_Complete)
                FillCustomControls(GetSelectedCustomKey());
        }

        private void btnFunds_Click(object sender, EventArgs e)
        {
            AlertCodeForm objform = new AlertCodeForm(BaseForm, Privileges, txtFunds.Text, Consts.AgyTab.FUNDCODS, "Funds");
            objform.FormClosed += new FormClosedEventHandler(objform_FundFormClosed);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.ShowDialog();
        }

        private void btnSpanish_Click(object sender, EventArgs e)
        {
            SpanishCustomQuestions spanishForm = new SpanishCustomQuestions(BaseForm, Privileges, TxtCode.Text, "CUSTOMQUESTIONS");
            spanishForm.StartPosition = FormStartPosition.CenterScreen;
            spanishForm.ShowDialog();
        }

        void objform_FundFormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            txtFunds.Text = form.propAlertCode;
        }

        string Agency = null;
        string Random_Filename = null; string PdfName = null;
        private void On_SaveExcelForm_Closed()
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "FieldsControlMaintenancereport";

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
                string Tmpstr = PdfName + ".pdf";
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


            Worksheet sheet; WorksheetCell cell; WorksheetRow Row0;
            string ReportName = "Sheet1";
            if (HierGrid.Rows.Count > 0)
            {
                sheet = book.Worksheets.Add(ReportName);
                sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(300);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);

                Row0 = sheet.Table.Rows.Add();

                cell = Row0.Cells.Add("Hierarchy", DataType.String, "s94");
                cell = Row0.Cells.Add("Hierarchy Description", DataType.String, "s94");
                cell = Row0.Cells.Add("Field Description", DataType.String, "s94");
                cell = Row0.Cells.Add("Sub Screen", DataType.String, "s94");
                cell = Row0.Cells.Add("Enabled", DataType.String, "s94");
                cell = Row0.Cells.Add("Required", DataType.String, "s94");
                cell = Row0.Cells.Add("Shared", DataType.String, "s94");
                Row0 = sheet.Table.Rows.Add();


                cell = Row0.Cells.Add(HierGrid.SelectedRows[0].Cells["Hierarchy"].Value.ToString(), DataType.String, "s96");
                cell = Row0.Cells.Add(HierGrid.SelectedRows[0].Cells["HieDesc"].Value.ToString(), DataType.String, "s96");
                cell = Row0.Cells.Add("", DataType.String, "s96");
                cell = Row0.Cells.Add("", DataType.String, "s96");
                cell = Row0.Cells.Add("", DataType.String, "s96");
                cell = Row0.Cells.Add("", DataType.String, "s96");
                cell = Row0.Cells.Add("", DataType.String, "s96");

                foreach (DataGridViewRow fldrows in FLDCNTLGrid.Rows)
                {
                    Row0 = sheet.Table.Rows.Add();

                    cell = Row0.Cells.Add("", DataType.String, "s96");
                    cell = Row0.Cells.Add("", DataType.String, "s96");

                    cell = Row0.Cells.Add(fldrows.Cells["dataGridViewTextBoxColumn1"].Value == null ? string.Empty : fldrows.Cells["dataGridViewTextBoxColumn1"].Value.ToString(), DataType.String, "s96");
                    cell = Row0.Cells.Add(fldrows.Cells["dataGridViewTextBoxColumn2"].Value == null ? string.Empty : fldrows.Cells["dataGridViewTextBoxColumn2"].Value.ToString(), DataType.String, "s96");

                    string strEnabled = fldrows.Cells["Enabled"].Value.ToString();
                    string strRequired = fldrows.Cells["Required"].Value.ToString();
                    string strShared = fldrows.Cells["Shared"].Value.ToString();
                    cell = Row0.Cells.Add(strEnabled.ToUpper() == "TRUE" ? "Y" : "N", DataType.String, "s96");
                    cell = Row0.Cells.Add(strRequired.ToUpper() == "TRUE" ? "Y" : "N", DataType.String, "s96");
                    cell = Row0.Cells.Add(strShared.ToUpper() == "TRUE" ? "Y" : "N", DataType.String, "s96");

                }


            }


            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            //FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //downloadGateway.Filename = "FieldsControlMaintenancereport.xls";

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

        }

        private void Pb_Help_Cust_Click(object sender, EventArgs e)
        {

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
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
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
            s96.Font.Bold = false;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //s96.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");

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



    }
}