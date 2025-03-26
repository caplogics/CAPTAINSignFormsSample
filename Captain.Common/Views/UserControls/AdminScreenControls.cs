#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
//using Wisej.Web.Design;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using Wisej.Web;
using DevExpress.XtraRichEdit.Model;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class AdminScreenControls : BaseUserControl
    {

        #region private variables

        private CaptainModel _model = null;
        private PrivilegesControl _screenPrivileges = null;
        private ErrorProvider _errorProvider = null;

        #endregion
        public AdminScreenControls(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            InitializeComponent();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            PopulateToolbar(oToolbarMnustrip);
            FillScreenCombo();

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }


        #endregion
        string ScrCode = null;
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
                ToolBarNew.ToolTipText = "Add Incomplete Intake Controls ";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Incomplete Intake Controls";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Incomplete Intake Controls";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "FieldControls Help";
                ToolBarHelp.Enabled = true;
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
                ToolBarHelp
            });
        }

        public void Refreshdata()
        {
            FillScreenCombo();
        }

        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();
            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null) { return; }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:

                        ScreenControlAssignmentForm screencontrolassignmentForm = new ScreenControlAssignmentForm(BaseForm, "Add", ScrCode, GetScafldsHierarchy(string.Empty), Privileges.PrivilegeName, 0);
                        screencontrolassignmentForm.FormClosed += new Wisej.Web.FormClosedEventHandler(screencontrolassignmentForm_FormClosed);
                        screencontrolassignmentForm.StartPosition = FormStartPosition.CenterScreen;
                        screencontrolassignmentForm.ShowDialog();

                        break;
                    case Consts.ToolbarActions.Edit:

                        ScreenControlAssignmentForm EditscreencontrolassignmentForm = new ScreenControlAssignmentForm(BaseForm, "Edit", ScrCode, GetScafldsHierarchy("Edit"), Privileges.PrivilegeName, 0);
                        EditscreencontrolassignmentForm.FormClosed += new Wisej.Web.FormClosedEventHandler(screencontrolassignmentForm_FormClosed);
                        EditscreencontrolassignmentForm.StartPosition = FormStartPosition.CenterScreen;
                        EditscreencontrolassignmentForm.ShowDialog();

                        break;
                    case Consts.ToolbarActions.Delete:

                        if (gvwCateHierchy.Rows.Count > 0)
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nIncomplete Intake Controls: " + gvwCateHierchy.SelectedRows[0].Cells["gvtCatHierchy"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Sel_Hierarchy);
                        break;

                    case Consts.ToolbarActions.Help:
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }
        void screencontrolassignmentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FillScreenHieGrid(ScrCode);
        }
        private void FillScreenHieGrid(string scrSCCode)
        {
            this.gvwCateHierchy.SelectionChanged -= new EventHandler(gvwCateHierchy_SelectionChanged);
            gvwCateHierchy.Rows.Clear();
            this.gvwCateHierchy.SelectionChanged += new EventHandler(gvwCateHierchy_SelectionChanged);
            gvwCateDetails.Rows.Clear();
            int selectedRowindex = 0;
            List<ScaFldsHieEntity> oScaFldsHieEntity = _model.FieldControls.GETSCAFLDSHIEDATA(scrSCCode, string.Empty, string.Empty);
            if (oScaFldsHieEntity.Count > 0)
            {
                string UserAgency = string.Empty;
                if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
                {
                    HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
                    if (SelHie != null)
                        UserAgency = "**";
                }

                if (BaseForm.BaseAdminAgency != "**")
                    oScaFldsHieEntity = oScaFldsHieEntity.FindAll(u => u.ScrHie.Substring(0, 2).Equals(BaseForm.BaseAdminAgency) || u.ScrHie.Substring(0, 2).Equals("**"));
                
                var scFldsEntityList = oScaFldsHieEntity.Select(u => u.ScrHie).Distinct().ToList();

                if (scFldsEntityList.Count > 0)
                {
                    foreach (var item in scFldsEntityList)
                    {
                        string TmpHieDesc = "All Hierarchies";
                        string TmpHie = null;
                        if (item.ToString() != "******")
                        {
                            DataSet Prog = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(item.Substring(0, 2), item.Substring(2, 2), item.Substring(4, 2));
                            if (Prog.Tables[0].Rows.Count > 0)
                                TmpHieDesc = (Prog.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                            else
                                TmpHieDesc = "Description Not Defined";
                        }

                        int rowindex = gvwCateHierchy.Rows.Add(item, TmpHieDesc);
                        if (RecordKey == item)
                        {
                            selectedRowindex = rowindex;
                        }
                    }
                    gvwCateHierchy.CurrentCell = gvwCateHierchy.Rows[selectedRowindex].Cells[0];
                    gvwCateHierchy.Rows[selectedRowindex].Selected = true;
                }
            }
        }

        private void Delete_Sel_Hierarchy(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                ScaFldsHieEntity oScaFldsHieEntity = new ScaFldsHieEntity();
                oScaFldsHieEntity.RecType = "Del";
                oScaFldsHieEntity.ScrCode = ScrCode;
                oScaFldsHieEntity.ScrHie = gvwCateHierchy.SelectedRows[0].Cells["gvtCatHierchy"].Value.ToString();
                oScaFldsHieEntity.ScahCode = string.Empty;
                oScaFldsHieEntity.Active = string.Empty;
                oScaFldsHieEntity.Sel = string.Empty;

                if (_model.FieldControls.InsertUpdateSCAFLDSHIE(oScaFldsHieEntity))
                {
                    AlertBox.Show("Incomplete Intake Controls for " + gvwCateHierchy.SelectedRows[0].Cells["gvtCatHierchy"].Value.ToString() + " Deleted Successfully");
                    FillScreenHieGrid(ScrCode);
                }
                else
                    AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);
            }
            //}
        }
        List<FLDSCRSEntity> FLDSCRS_List = new List<FLDSCRSEntity>();
        private void FillScreenCombo()
        {
            this.CmbScreen.SelectedIndexChanged -= new System.EventHandler(this.CmbScreen_SelectedIndexChanged);
            CmbScreen.Items.Clear();

            FLDSCRSEntity Search_Entity = new FLDSCRSEntity(true);
            Search_Entity.Called_By = "ADMNCNTL";
            FLDSCRS_List = _model.FieldControls.Browse_FLDSCRS(Search_Entity);
            string Tmp_Desc = string.Empty;

            foreach (FLDSCRSEntity Entity in FLDSCRS_List)
            {
                //if ((Entity.Scr_Code == "CASE2001" && Entity.Scr_Sub_Code == "00") || Entity.Scr_Code != "CASE2001")
                //{
                Tmp_Desc = string.Empty;
                Tmp_Desc = String.Format("{0,-30}", Entity.Scr_Desc) + String.Format("{0,8}", " - " + Entity.Scr_Code);
                CmbScreen.Items.Add(new ListItem(Tmp_Desc, Entity.Scr_Code, Entity.Scr_Desc, Entity.Cust_Ques_SW));
                //}
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
            {
                CmbScreen.SelectedIndex = 0;
                CmbScreen_SelectedIndexChanged(CmbScreen, new EventArgs());
            }

            this.CmbScreen.SelectedIndexChanged += new System.EventHandler(this.CmbScreen_SelectedIndexChanged);

        }

        string Cust_SCR_Mode = "View";
        private void CmbScreen_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScrCode = ((ListItem)CmbScreen.SelectedItem).Value.ToString();
            //this.CmbScreen.SelectedIndexChanged -= new System.EventHandler(this.CmbScreen_SelectedIndexChanged);
            FillScreenHieGrid(ScrCode);
            //this.CmbScreen.SelectedIndexChanged += new System.EventHandler(this.CmbScreen_SelectedIndexChanged);
            if (ToolBarNew != null) { ToolBarNew.Visible = true; }
            if (ToolBarEdit != null) ToolBarEdit.Visible = true;
            if (ToolBarDel != null) ToolBarDel.Visible = true;

        }
        string RecordKey = null;
        private string GetScafldsHierarchy(string mode) // 08162012
        {
            try
            {
                if (gvwCateHierchy.Rows.Count > 0)
                {
                    RecordKey = gvwCateHierchy.SelectedRows[0].Cells["gvtCatHierchy"].Value.ToString();
                }
            }
            catch (Exception ex) { }

            return RecordKey;
        }


        private void gvwCateHierchy_SelectionChanged(object sender, EventArgs e)
        {
            gvwCateDetails.Rows.Clear();
            if (gvwCateHierchy.Rows.Count > 0)
            {
                List<ScaFldsHieEntity> ScaFldsHiedata = _model.FieldControls.GETSCAFLDSHIEDATA(((ListItem)CmbScreen.SelectedItem).Value.ToString(), string.Empty, gvwCateHierchy.SelectedRows[0].Cells["gvtCatHierchy"].Value.ToString());
                foreach (ScaFldsHieEntity iTem in ScaFldsHiedata)
                {
                    gvwCateDetails.Rows.Add(iTem.scaDesc, iTem.Sel == "Y" ? true : false, iTem.ScahCode, iTem.Active == "Y" ? true : false, iTem.Msg);
                    gvwCateDetails.Rows[0].Selected = true;
                }

            }
        }

    }
}