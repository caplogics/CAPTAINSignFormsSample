#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Model.Data;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Wisej.Web;
using DevExpress.XtraRichEdit.Unicode;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class Site_ScheduleControl : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string[] strCode = null;
        public int strIndex = 0;
        public int strPageIndex = 1;
        #endregion
        public Site_ScheduleControl(BaseForm baseform, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileges = privileges;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            GetDataTable();
            fillSortCombo();

            if (Privileges.ModuleCode == "02")
            {
                cmbYear.Visible = false;
                panel3.Visible = false; lblYear.Visible = false;
               // this.panel1.Location = new System.Drawing.Point(0, 36);
               // this.Size = new System.Drawing.Size(655, 337);
                //this.panel2.Size = new System.Drawing.Size(652, 37);
                if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                {
                    AlertBox.Show("Year Should not be blank for this hierarchy in Program Definition", MessageBoxIcon.Warning);
                    cmbSort.Enabled = false;

                }
                else
                {
                    dsHieDesc = DatabaseLayer.UserAccess.GetHierachyDesc("3", BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString());
                    fillgvSiteSchedule();
                }
            }
            else
            {
                hierarchyEntity = _model.lookupDataAccess.GetHierarchyByUserID(null, "I", string.Empty);
               // this.panel1.Location = new System.Drawing.Point(99, 36);
                //this.Size = new System.Drawing.Size(755, 337);
               // this.panel2.Size = new System.Drawing.Size(750, 37);
                cmbYear.Visible = true;
                panel3.Visible = true; lblYear.Visible = true;
                fillYearCombo();
            }




            PopulateToolbar(oToolbarMnustrip);

            //fillHieGrid();
            //fillgvSiteSchedule();
        }
        string dsHieDesc = string.Empty;
        //string dsHierar = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public ToolBarButton ToolBarPrint { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        public bool IsSaveValid { get; set; }


        #endregion


        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (ToolBarNew == null)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Add Site Schedule";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = Consts.Icons.ico_Add;
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Site Schedule";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = Consts.Icons.ico_Edit;
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Site Schedule";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = Consts.Icons.ico_Delete;
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarPrint = new ToolBarButton();
                //ToolBarPrint.Tag = "Print";
                //ToolBarPrint.ToolTipText = "Print";
                //ToolBarPrint.Enabled = true;
                //ToolBarPrint.Image = new IconResourceHandle(Consts.Icons16x16.Print);
                //ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = Consts.Icons.ico_Help;
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
               // ToolBarPrint,
                ToolBarHelp
            });

            if (Privileges.ModuleCode == "02" && string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            {
                ToolBarNew.Enabled = false; ToolBarEdit.Enabled = false; ToolBarDel.Enabled = false;
            }
            else if (Privileges.ModuleCode == "02")
            {
                ProgramDefinitionEntity programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency.Trim(), BaseForm.BaseDept.Trim(), BaseForm.BaseProg.Trim());
                if (programDefinitionList.DepYear == BaseForm.BaseYear.Trim())
                {
                    if (string.IsNullOrEmpty(programDefinitionList.StartMonth.Trim()) || string.IsNullOrEmpty(programDefinitionList.EndMonth.Trim()))
                    {
                        AlertBox.Show("Start Month and End Month should not be blank in Program Definition", MessageBoxIcon.Warning);
                        ToolBarNew.Enabled = false; ToolBarEdit.Enabled = false; ToolBarDel.Enabled = false;
                    }
                    else
                    {
                        if (dt.Rows.Count == 0)
                        {
                            ToolBarNew.Enabled = true; ToolBarEdit.Enabled = false; ToolBarDel.Enabled = false;
                        }
                        else
                        {
                            ToolBarEdit.Enabled = true; ToolBarDel.Enabled = true;
                        }
                    }
                }
            }
            else if (Privileges.ModuleCode == "01")
            {
                if (dt.Rows.Count == 0)
                {
                    ToolBarNew.Enabled = false; ToolBarEdit.Enabled = false; ToolBarDel.Enabled = false;
                }
            }
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
                        //Added_Edited_SiteCode = string.Empty; Added_Edited_HieCode = string.Empty;
                        if (Privileges.ModuleCode == "02")
                        {
                            Siteschedule_Form Site_Form_Add = new Siteschedule_Form(BaseForm, "Add", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dsHieDesc, string.Empty, string.Empty, string.Empty, Privileges, string.Empty);
                            Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                            Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                            Site_Form_Add.ShowDialog();
                        }
                        else
                        {
                            if (gvHie.Rows.Count > 0)
                            {
                                Siteschedule_Form Site_Form_Add = new Siteschedule_Form(BaseForm, "Add", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), string.Empty, string.Empty, Privileges, string.Empty);
                                Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Add.ShowDialog();
                            }
                            else
                            {
                                Siteschedule_Form Site_Form_Add = new Siteschedule_Form(BaseForm, "Add", string.Empty, string.Empty, string.Empty, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Privileges, string.Empty);
                                Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Add.ShowDialog();
                            }
                        }

                        break;
                    case Consts.ToolbarActions.Edit:
                        if (Privileges.ModuleCode == "01")
                        {
                            if (gvHie.Rows.Count > 0)
                            {
                                Siteschedule_Form Site_Form_Edit = new Siteschedule_Form(BaseForm, "Edit", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvsiteSchedule.CurrentRow.Cells["Month_No"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Site"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Room"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Am_Pm"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Fund"].Value.ToString(), gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Sch_Type"].Value.ToString().Trim(), gvsiteSchedule.CurrentRow.Cells["Calander_Year"].Value.ToString().Trim(), Privileges, gvsiteSchedule.CurrentRow.Cells["Attm_ID"].Value.ToString().Trim());
                                Site_Form_Edit.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Edit.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Edit.ShowDialog();
                            }
                        }
                        else
                        {
                            if (gvsiteSchedule.Rows.Count > 0)
                            {
                                Siteschedule_Form Site_Form_Edit = new Siteschedule_Form(BaseForm, "Edit", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), gvsiteSchedule.CurrentRow.Cells["Month_No"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Site"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Room"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Am_Pm"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Fund"].Value.ToString(), dsHieDesc, BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg, gvsiteSchedule.CurrentRow.Cells["Sch_Type"].Value.ToString().Trim(), gvsiteSchedule.CurrentRow.Cells["Calander_Year"].Value.ToString().Trim(), Privileges, gvsiteSchedule.CurrentRow.Cells["Attm_ID"].Value.ToString().Trim());
                                Site_Form_Edit.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Edit.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Edit.ShowDialog();
                            }
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (Privileges.ModuleCode == "01")
                        {
                            if (gvHie.Rows.Count > 0)
                            {
                                strIndex = gvsiteSchedule.SelectedRows[0].Index;
                                //strPageIndex = gvsiteSchedule.CurrentPage;
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "This Site Schedule ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_SiteSchedule_Row);
                            }
                        }
                        else
                        {
                            if (gvsiteSchedule.Rows.Count > 0)
                            {
                                strIndex = gvsiteSchedule.SelectedRows[0].Index;
                                //strPageIndex = gvsiteSchedule.CurrentPage;
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "This Site Schedule ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_SiteSchedule_Row);
                            }
                        }
                        break;
                    case Consts.ToolbarActions.Print:

                        break;
                    case Consts.ToolbarActions.Help:
                        //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSS20135");
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

        private void Launch_by_Privilige()
        {

        }


        private void fillYearCombo()
        {
            cmbYear.Items.Clear();

            List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("MASTER", "    "));
            listItem.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-3).Year.ToString(), DateTime.Now.AddYears(-3).Year.ToString()));
            cmbYear.Items.AddRange(listItem.ToArray());
            cmbYear.SelectedIndex = 1;
        }

        private void fillSortCombo()
        {
            cmbSort.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            ////listItem.Add(new ListItem("MASTER", "    "));
            listItem.Add(new ListItem("Schedule Type,Month", "0"));
            //listItem.Add(new ListItem("Site and Room,Month", "0"));
            listItem.Add(new ListItem("Fund Source,Month", "1"));
            listItem.Add(new ListItem("Site,Month", "2"));
            //listItem.Add(new ListItem("Month,Schedule Type", "3"));
            listItem.Add(new ListItem("Month,Site and Room", "3"));
            this.cmbSort.SelectedIndexChanged -= new System.EventHandler(this.cmbSort_SelectedIndexChanged);
            cmbSort.Items.AddRange(listItem.ToArray());
            cmbSort.SelectedIndex = 0;
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbSort_SelectedIndexChanged);
        }

        private void fillHieGrid(string HieCode)
        {
            gvHie.Rows.Clear();
            int rowIndex = 0; int rowCnt = 0;
            int Sel_HieIndex = 0;

            DataSet dsHie = DatabaseLayer.CaseMst.GetCaseSiteHieforSchedule(((ListItem)cmbYear.SelectedItem).Value.ToString());
            DataTable dtHie = new DataTable();
            if (dsHie.Tables.Count > 0)
                dtHie = dsHie.Tables[0];
            if (dtHie.Rows.Count > 0)
            {
                foreach (DataRow drHie in dtHie.Rows)
                {
                    string CaseHie = string.Empty;
                    string SiteHie = string.Empty;
                    CaseHie = drHie["SITE_Hier"].ToString().Substring(0, 2).Trim() + "-" + drHie["SITE_Hier"].ToString().Substring(2, 2).Trim() + "-" + drHie["SITE_Hier"].ToString().Substring(4, 2).Trim();
                    SiteHie = drHie["SITE_Hier"].ToString();

                    HierarchyEntity hierachysubEntity = hierarchyEntity.Find(u => u.Code.Equals(CaseHie));
                    if (hierachysubEntity != null)
                    {
                        rowIndex = gvHie.Rows.Add(CaseHie, SiteHie, hierachysubEntity.HirarchyName);
                    }
                    if (HieCode.Trim() == CaseHie)
                        Sel_HieIndex = rowCnt;
                    rowCnt++;
                }
            }

            if (rowCnt > 0)
            {
                if (string.IsNullOrEmpty(HieCode))
                    gvHie.Rows[0].Tag = 0;
                else
                {
                    gvHie.CurrentCell = gvHie.Rows[Sel_HieIndex].Cells[0];

                    int scrollPosition = 0;
                    scrollPosition = gvHie.CurrentCell.RowIndex;
                  //  int CurrentPage = (scrollPosition / gvHie.ItemsPerPage);
                   // //CurrentPage++;
                   // gvHie.CurrentPage = CurrentPage;
                   // gvHie.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
            }
            else
                cmbSort.Enabled = false;
        }



        // List<Temp_SiteScheduleEntity> Temp_List = new List<Temp_SiteScheduleEntity>();
        private void fillgvSiteSchedule()
        {
            gvsiteSchedule.Rows.Clear(); int rowCnt = 0;
            int Sel_SchIndex = 0;
            List<SiteScheduleEntity> sitelist = new List<SiteScheduleEntity>();
            SiteScheduleEntity SearchEntity = new SiteScheduleEntity(true);
            List<CaseSiteEntity> SiteName_List = new List<CaseSiteEntity>();
            CaseSiteEntity Search_Site = new CaseSiteEntity(true);
            DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            DataTable dtLang = new DataTable(); int rowIndex = 0;
            if (dsLang.Tables.Count > 0)
                dtLang = dsLang.Tables[0];
            if (Privileges.ModuleCode == "01")
            {
                dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
                if (dsLang.Tables.Count > 0)
                    dtLang = dsLang.Tables[0];
            }
            //  Temp_SiteScheduleEntity Temp_Site = new Temp_SiteScheduleEntity(true);
            if (Privileges.ModuleCode == "01")
            {
                if (gvHie.Rows.Count > 0)
                {
                    string Hierarchy = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString();
                    string Agency = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2).Trim();
                    string Dept = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                    string Prog = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                    SearchEntity.ATTM_AGENCY = Agency.Trim();
                    SearchEntity.ATTM_DEPT = Dept.Trim();
                    SearchEntity.ATTM_PROG = Prog.Trim();
                    if (Privileges.ModuleCode == "02")
                        SearchEntity.ATTM_YEAR = BaseForm.BaseYear.Trim();
                    else
                        SearchEntity.ATTM_YEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                    sitelist = _model.SPAdminData.Browse_CHILDATTM(SearchEntity, "Browse");

                    if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                        HierarchyEntity hierarchyEntity = new HierarchyEntity();
                        foreach (HierarchyEntity Entity in userHierarchy)
                        {
                            if (Entity.InActiveFlag == "N")
                            {
                                if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                                { hierarchyEntity = null; }
                            }
                        }

                        if (hierarchyEntity != null)
                        {
                            if (hierarchyEntity.Sites.Length > 0)
                            {
                                string[] Sites = hierarchyEntity.Sites.Split(',');
                                List<SiteScheduleEntity> Selsites = new List<SiteScheduleEntity>();
                                for (int i = 0; i < Sites.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                    {
                                        foreach (SiteScheduleEntity casesite in sitelist) //Site_List)//ListcaseSiteEntity)
                                        {
                                            if (Sites[i].ToString() == casesite.ATTM_SITE)
                                            {
                                                Selsites.Add((casesite));
                                                //break;
                                            }
                                            // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                        }
                                    }
                                }
                                sitelist = Selsites;
                                //strsiteRoomNames = hierarchyEntity.Sites;
                            }
                        }
                    }
                }
            }
            else
            {
                //string Hierarchy = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString();
                //string Agency = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2).Trim();
                //string Dept = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                //string Prog = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                SearchEntity.ATTM_AGENCY = BaseForm.BaseAgency.Trim();
                SearchEntity.ATTM_DEPT = BaseForm.BaseDept.Trim();
                SearchEntity.ATTM_PROG = BaseForm.BaseProg.Trim();
                SearchEntity.ATTM_YEAR = BaseForm.BaseYear.Trim();
                sitelist = _model.SPAdminData.Browse_CHILDATTM(SearchEntity, "Browse");

                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
                    foreach (HierarchyEntity Entity in userHierarchy)
                    {
                        if (Entity.InActiveFlag == "N")
                        {
                            if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg.Trim())
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                            { hierarchyEntity = null; }
                        }
                    }

                    if (hierarchyEntity != null)
                    {
                        if (hierarchyEntity.Sites.Length > 0)
                        {
                            string[] Sites = hierarchyEntity.Sites.Split(',');
                            List<SiteScheduleEntity> Selsites = new List<SiteScheduleEntity>();
                            for (int i = 0; i < Sites.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                {
                                    foreach (SiteScheduleEntity casesite in sitelist) //Site_List)//ListcaseSiteEntity)
                                    {
                                        if (Sites[i].ToString() == casesite.ATTM_SITE)
                                        {
                                            Selsites.Add((casesite));
                                            //break;
                                        }
                                        // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                    }
                                }
                            }
                            sitelist = Selsites;
                            //strsiteRoomNames = hierarchyEntity.Sites;
                        }
                    }
                }
            }
            if (sitelist.Count > 0)
            {
                string Month = string.Empty; string Cal_Month = "01"; string Calander_Year = string.Empty;
                foreach (SiteScheduleEntity Entity in sitelist)
                {
                    Month = GetMonth(Entity.ATTM_MONTH.Trim());
                    Cal_Month = Entity.ATTM_MONTH.Trim();
                    Cal_Month = Cal_Month.Length == 1 ? "0" + Cal_Month : Cal_Month;
                    Calander_Year = Entity.ATTM_CALENDER_YEAR.Substring(2, 2).ToString();
                    string Sch_Type = string.Empty; string Name_site = " ";
                    if (string.IsNullOrEmpty(Entity.ATTM_SITE.Trim()) && string.IsNullOrEmpty(Entity.ATTM_ROOM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_AMPM.Trim()))
                    {
                        Sch_Type = "1Master";
                    }
                    else if (!string.IsNullOrEmpty(Entity.ATTM_SITE.Trim()) && string.IsNullOrEmpty(Entity.ATTM_ROOM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_AMPM.Trim()))
                    {
                        Sch_Type = "2Site";
                    }
                    else
                        Sch_Type = "3Room";
                    if (!string.IsNullOrEmpty(Entity.ATTM_SITE.Trim()))
                    {
                        Search_Site.SiteAGENCY = Entity.ATTM_AGENCY.Trim();
                        Search_Site.SiteDEPT = Entity.ATTM_DEPT.Trim();
                        Search_Site.SitePROG = Entity.ATTM_PROG.Trim();
                        Search_Site.SiteYEAR = Entity.ATTM_YEAR.Trim();
                        Search_Site.SiteNUMBER = Entity.ATTM_SITE.Trim();
                        Search_Site.SiteROOM = "0000";
                        //Search_Site.SiteAM_PM = "";
                        SiteName_List = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");
                        if (SiteName_List.Count > 0)
                            Name_site = SiteName_List[0].SiteNAME.Trim();
                    }
                    string Fund = string.Empty;
                    if (dtLang.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtLang.Rows)
                        {
                            if (dr["Code"].ToString().Trim() == Entity.ATTM_FUND.Trim())
                            {
                                Fund = dr["LookUpDesc"].ToString().Trim(); break;
                            }
                        }
                    }

                    //Temp_Site.MONTH = Cal_Month.Trim(); Temp_Site.SCH_Type = Sch_Type.Trim(); Temp_Site.SITE = Entity.ATTM_SITE;
                    //Temp_Site.Site_Name = Name_site.Trim(); Temp_Site.ROOM = Entity.ATTM_ROOM.Trim(); Temp_Site.AMPM = Entity.ATTM_AMPM.Trim();
                    //Temp_Site.FUND = Fund.Trim(); Temp_Site.MONTH_No = Entity.ATTM_MONTH.Trim(); Temp_Site.ATTM_ADD_OPERATOR = Entity.ATTM_ADD_OPERATOR.ToString().Trim();
                    //Temp_Site.ATTM_DATE_ADD = Entity.ATTM_DATE_ADD.ToString(); Temp_Site.ATTM_LSTC_OPERATOR = Entity.ATTM_LSTC_OPERATOR.ToString().Trim(); Temp_Site.ATTM_DATE_LSTC = Entity.ATTM_DATE_LSTC.ToString();
                    //Temp_List.Add(new Temp_SiteScheduleEntity(Temp_Site));
                    rowIndex = gvsiteSchedule.Rows.Add(Cal_Month + "-" + Month + "'" + Calander_Year, Sch_Type, Entity.ATTM_SITE, Name_site, Entity.ATTM_ROOM, Entity.ATTM_AMPM, Fund, Entity.ATTM_MONTH, Entity.ATTM_FUND, Entity.ATTM_CALENDER_YEAR.Trim(), Entity.ATTM_DATE_ADD.ToString(), Entity.ATTM_ADD_OPERATOR.ToString().Trim(), Entity.ATTM_DATE_LSTC.ToString(), Entity.ATTM_LSTC_OPERATOR.ToString().Trim(), Entity.ATTM_ID);
                    string toolTipText = "Added By     : " + Entity.ATTM_ADD_OPERATOR.ToString().Trim() + " on " + Entity.ATTM_DATE_ADD.ToString() + "\n" +
                                        "Modified By  : " + Entity.ATTM_LSTC_OPERATOR.ToString().Trim() + " on " + Entity.ATTM_DATE_LSTC.ToString();
                    foreach (DataGridViewCell cell in gvsiteSchedule.Rows[rowIndex].Cells)
                    {
                        cell.ToolTipText = toolTipText;
                    }
                    if (Added_Edited_Type == "Room")
                    {
                        if ((int.Parse(Added_Edited_Month.Trim())).ToString() == Entity.ATTM_MONTH.Trim() && Added_Edited_Site.Trim() == Entity.ATTM_SITE.Trim() &&
                            Added_Edited_Room.Trim() == Entity.ATTM_ROOM.Trim() && Added_Edited_AMPM.Trim() == Entity.ATTM_AMPM.Trim())
                            Sel_SchIndex = rowCnt;
                    }
                    else if (Added_Edited_Type == "Site")
                    {
                        if ((int.Parse(Added_Edited_Month.Trim())).ToString() == Entity.ATTM_MONTH.Trim() && Added_Edited_Site.Trim() == Entity.ATTM_SITE.Trim())
                            Sel_SchIndex = rowCnt;
                    }
                    else if (Added_Edited_Type == "Master")
                    {
                        if ((int.Parse(Added_Edited_Month.Trim())).ToString() == Entity.ATTM_MONTH.Trim())
                            Sel_SchIndex = rowCnt;
                    }

                    rowCnt++;
                    //}

                    //gvsiteSchedule.Rows[0].Tag = 0;
                }
                //gvsiteSchedule.DataSource = sitelist;
            }
            dataRows_Sort();

            //dt = gvsiteSchedule.DataSource as DataTable;
            if (dt != null)
            {
                DataView dv = new DataView(dt);
                if (((ListItem)cmbSort.SelectedItem).Value.ToString().Trim() == "0")
                    dv.Sort = "Sch_Type,Month";
                else if (((ListItem)cmbSort.SelectedItem).Value.ToString().Trim() == "1")
                    dv.Sort = "Fund,Month";
                else if (((ListItem)cmbSort.SelectedItem).Value.ToString().Trim() == "2")
                    dv.Sort = "Site,Month";
                else if (((ListItem)cmbSort.SelectedItem).Value.ToString().Trim() == "3")
                    dv.Sort = "Month,Site,Room";
                gvsiteSchedule.Rows.Clear();
                dt = dv.ToTable(); rowCnt = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string Schdule = string.Empty;
                    if (dr["Sch_Type"].ToString().Trim() == "1Master") Schdule = "Master";
                    else if (dr["Sch_Type"].ToString().Trim() == "2Site") Schdule = "Site";
                    else if (dr["Sch_Type"].ToString().Trim() == "3Room") Schdule = "Room";
                    rowIndex = gvsiteSchedule.Rows.Add(dr["Month"].ToString().Trim(), Schdule.Trim(), dr["Site"].ToString().Trim(), dr["Site_Name"].ToString().Trim(), dr["Room"].ToString().Trim(), dr["AmPm"].ToString().Trim(), dr["Fund_Source"].ToString().Trim(), dr["Month_No"].ToString().Trim(), dr["Fund"].ToString().Trim(), dr["Calander_Year"].ToString().Trim(), dr["Add_Date"].ToString(), dr["Add_Operator"].ToString().Trim(), dr["Lstc_Date"].ToString(), dr["Lstc_Operator"].ToString().Trim(), dr["ATTM_ID"].ToString().Trim());

                    string toolTipText = "Added By     : " + dr["Add_Operator"].ToString().Trim() + " on " + dr["Add_Date"].ToString() + "\n" +
                                        "Modified By  : " + dr["Lstc_Operator"].ToString().Trim() + " on " + dr["Lstc_Date"].ToString();
                    foreach (DataGridViewCell cell in gvsiteSchedule.Rows[rowIndex].Cells)
                    {
                        cell.ToolTipText = toolTipText;
                    }
                    if (Added_Edited_Type == "Room")
                    {
                        if ((int.Parse(Added_Edited_Month.Trim())).ToString() == dr["Month_No"].ToString().Trim() && Added_Edited_Site.Trim() == dr["Site"].ToString().Trim() &&
                            Added_Edited_Room.Trim() == dr["Room"].ToString().Trim() && Added_Edited_AMPM.Trim() == dr["AmPm"].ToString().Trim() && Added_Edited_Fund == dr["Fund"].ToString().Trim())
                            Sel_SchIndex = rowCnt;
                    }
                    else if (Added_Edited_Type == "Site")
                    {
                        //if ((int.Parse(Added_Edited_Month.Trim())).ToString() == dr["Month_No"].ToString().Trim() && Added_Edited_Site.Trim() == dr["Site"].ToString().Trim())
                        if ((int.Parse(Added_Edited_Month.Trim())).ToString() == dr["Month_No"].ToString().Trim() && Added_Edited_Site.Trim() == dr["Site"].ToString().Trim() &&
                            Added_Edited_Room.Trim() == dr["Room"].ToString().Trim() && Added_Edited_AMPM.Trim() == dr["AmPm"].ToString().Trim() && Added_Edited_Fund == dr["Fund"].ToString().Trim())
                            Sel_SchIndex = rowCnt;
                    }
                    else if (Added_Edited_Type == "Master")
                    {
                        if ((int.Parse(Added_Edited_Month.Trim())).ToString() == dr["Month_No"].ToString().Trim() && Added_Edited_Site.Trim() == dr["Site"].ToString().Trim() &&
                            Added_Edited_Room.Trim() == dr["Room"].ToString().Trim() && Added_Edited_AMPM.Trim() == dr["AmPm"].ToString().Trim() && Added_Edited_Fund == dr["Fund"].ToString().Trim())
                            //if ((int.Parse(Added_Edited_Month.Trim())).ToString() == dr["Month_No"].ToString().Trim())
                            Sel_SchIndex = rowCnt;
                    }

                    rowCnt++;
                }

            }

            //if (Temp_List.Count > 0)
            //{
            //    Temp_List.Sort();
            //    foreach (Temp_SiteScheduleEntity Entity in Temp_List)
            //    {
            //        rowIndex = gvsiteSchedule.Rows.Add(Entity.MONTH, Entity.SCH_Type, Entity.SITE, Entity.Site_Name, Entity.ROOM, Entity.AMPM, Entity.FUND, Entity.MONTH_No);
            //        string toolTipText = "Added By     : " + Entity.ATTM_ADD_OPERATOR.ToString().Trim() + " on " + Entity.ATTM_DATE_ADD.ToString() + "\n" +
            //                        "Modified By  : " + Entity.ATTM_LSTC_OPERATOR.ToString().Trim() + " on " + Entity.ATTM_DATE_LSTC.ToString();
            //        foreach (DataGridViewCell cell in gvsiteSchedule.Rows[rowIndex].Cells)
            //        {
            //            cell.ToolTipText = toolTipText;
            //        }
            //        if (Added_Edited_Type == "Room")
            //        {
            //            if ((int.Parse(Added_Edited_Month.Trim())).ToString() == Entity.MONTH_No.Trim() && Added_Edited_Site.Trim() == Entity.SITE.Trim() &&
            //                Added_Edited_Room.Trim() == Entity.ROOM.Trim() && Added_Edited_AMPM.Trim() == Entity.AMPM.Trim())
            //                Sel_SchIndex = rowCnt;
            //        }
            //        else if (Added_Edited_Type == "Site")
            //        {
            //            if ((int.Parse(Added_Edited_Month.Trim())).ToString() == Entity.MONTH_No.Trim() && Added_Edited_Site.Trim() == Entity.SITE.Trim())
            //                Sel_SchIndex = rowCnt;
            //        }
            //        else if (Added_Edited_Type == "Master")
            //        {
            //            if ((int.Parse(Added_Edited_Month.Trim())).ToString() == Entity.MONTH_No.Trim())
            //                Sel_SchIndex = rowCnt;
            //        }

            //        rowCnt++;
            //    }
            //}
            if (rowCnt > 0)
            {
                if (string.IsNullOrEmpty(Added_Edited_Type))
                {
                    //gvsiteSchedule.Rows[0].Tag = 0;
                    gvsiteSchedule.CurrentCell = gvsiteSchedule.Rows[0].Cells[0];
                    gvsiteSchedule.Rows[0].Selected = true;
                }
                else
                {
                    gvsiteSchedule.CurrentCell = gvsiteSchedule.Rows[Sel_SchIndex].Cells[0];

                    int scrollPosition = 0;
                    scrollPosition = gvsiteSchedule.CurrentCell.RowIndex;
                    // int CurrentPage = (scrollPosition / gvsiteSchedule.ItemsPerPage);
                    // CurrentPage++;
                    // gvsiteSchedule.CurrentPage = CurrentPage;
                    // gvsiteSchedule.FirstDisplayedScrollingRowIndex = scrollPosition;
                }


                if (gvsiteSchedule.Rows.Count == 1) {
                    gvsiteSchedule.Rows[0].Selected = true;
                }

                if (Privileges.ModuleCode == "01")
                {
                    if (ToolBarNew != null)
                        ToolBarNew.Enabled = true;
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = true;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = true;
                    cmbSort.Enabled = true;
                }
            }
            else
            {
                cmbSort.Enabled = false;
                if (Privileges.ModuleCode == "01")
                {
                    if (ToolBarNew != null)
                        ToolBarNew.Enabled = false;
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = false;
                    cmbSort.Enabled = true;

                }
            }


        }

        DataTable dt = new DataTable();
        public void GetDataTable()
        {
            dt.Columns.Add(new DataColumn("Month", typeof(string)));
            dt.Columns.Add(new DataColumn("Sch_Type", typeof(string)));
            dt.Columns.Add(new DataColumn("Site", typeof(string)));
            dt.Columns.Add(new DataColumn("Site_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Room", typeof(string)));
            dt.Columns.Add(new DataColumn("AmPm", typeof(string)));
            dt.Columns.Add(new DataColumn("Fund", typeof(string)));
            dt.Columns.Add(new DataColumn("Calander_Year", typeof(string)));
            dt.Columns.Add(new DataColumn("Fund_Source", typeof(string)));
            dt.Columns.Add(new DataColumn("Month_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Add_Operator", typeof(string)));
            dt.Columns.Add(new DataColumn("Lstc_Operator", typeof(string)));
            dt.Columns.Add(new DataColumn("Add_Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Lstc_Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("ATTM_ID", typeof(string)));
        }
        private void dataRows_Sort()
        {
            dt.Rows.Clear();
            foreach (DataGridViewRow row in gvsiteSchedule.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["Month"] = row.Cells["Month"].Value.ToString();
                dr["Sch_Type"] = row.Cells["Sch_Type"].Value.ToString();
                dr["Site"] = row.Cells["Site"].Value.ToString();
                dr["Site_Name"] = row.Cells["Site_Name"].Value.ToString();
                dr["Room"] = row.Cells["Room"].Value.ToString();
                dr["AmPm"] = row.Cells["Am_Pm"].Value.ToString();
                dr["Month_No"] = row.Cells["Month_No"].Value.ToString();
                dr["Fund"] = row.Cells["Fund"].Value.ToString();
                dr["Calander_Year"] = row.Cells["Calander_Year"].Value.ToString();
                dr["Fund_Source"] = row.Cells["Funding_Source"].Value.ToString();
                dr["Add_Date"] = row.Cells["Add_Date"].Value.ToString();
                dr["Add_Operator"] = row.Cells["Add_Operator"].Value.ToString();
                dr["Lstc_Date"] = row.Cells["Lstc_Date"].Value.ToString();
                dr["Lstc_Operator"] = row.Cells["Lstc_Operator"].Value.ToString();
                dr["ATTM_ID"] = row.Cells["Attm_ID"].Value.ToString();
                dt.Rows.Add(dr);

                //gvsiteSchedule.Rows.Add(dr["Month"].ToString().Trim(), dr["Sch_Type"].ToString().Trim(), dr["Site"].ToString().Trim(), dr["Site_Name"].ToString().Trim(), dr["Room"].ToString().Trim(), dr["AmPm"].ToString().Trim(), dr["Fund"].ToString().Trim(), dr["Month_No"].ToString().Trim());

            }
        }

        private string GetMonth(String month)
        {
            string month_name = null;
            switch (month)
            {
                case "1": month_name = "January"; break;
                case "2": month_name = "February"; break;
                case "3": month_name = "March"; break;
                case "4": month_name = "April"; break;
                case "5": month_name = "May"; break;
                case "6": month_name = "June"; break;
                case "7": month_name = "July"; break;
                case "8": month_name = "August"; break;
                case "9": month_name = "September"; break;
                case "10": month_name = "October"; break;
                case "11": month_name = "November"; break;
                case "12": month_name = "December"; break;
            }
            return month_name;
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Added_Edited_HieCode = string.Empty;
            fillHieGrid(string.Empty);
        }

        private void gvHie_SelectionChanged(object sender, EventArgs e)
        {
            fillgvSiteSchedule();
        }

        string Added_Edited_Site = string.Empty; string Added_Edited_HieCode = string.Empty;
        string Added_Edited_Type = string.Empty; string Added_Edited_Room = string.Empty;
        string Added_Edited_AMPM = string.Empty; string Added_Edited_Month = string.Empty; string Added_Edited_Fund = string.Empty;
        private void Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Siteschedule_Form form = sender as Siteschedule_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[8];
                From_Results = form.GetSelected_Schedule_Code();
                Added_Edited_Site = From_Results[0];
                Added_Edited_Room = From_Results[2];
                Added_Edited_AMPM = From_Results[3];
                //Added_Edited_HieCode=From_Results[4]
                Added_Edited_Month = From_Results[5];
                Added_Edited_Type = From_Results[6];
                Added_Edited_Fund = From_Results[7];
                //if (From_Results[1].Equals("Add"))
                //{
                //    MessageBox.Show("Site Schedule Saved Successfully...", "CAP Systems");
                //}
                //else
                //    MessageBox.Show("Site Schedule Updated Successfully...", "CAP Systems");

                if (Privileges.ModuleCode == "01")
                    fillHieGrid(Added_Edited_HieCode);
                else
                    fillgvSiteSchedule();
                //Mode = From_Results[1];
                //Added_Edited_Type = From_Results[2];

                PopulateToolbar(oToolbarMnustrip);
            }
        }

        public void Delete_SiteSchedule_Row(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                List<SiteScheduleEntity> listSchedule = new List<SiteScheduleEntity>();
                string strmsg = string.Empty; string strSqlmsg = string.Empty;
                SiteScheduleEntity Entity = new SiteScheduleEntity(true);
                if (Privileges.ModuleCode == "01")
                {
                    Entity.ATTM_AGENCY = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2);
                    Entity.ATTM_DEPT = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                    Entity.ATTM_PROG = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                    Entity.ATTM_YEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                }
                else if (Privileges.ModuleCode == "02")
                {
                    Entity.ATTM_AGENCY = BaseForm.BaseAgency.Trim();
                    Entity.ATTM_DEPT = BaseForm.BaseDept.Trim();
                    Entity.ATTM_PROG = BaseForm.BaseProg.Trim();
                    Entity.ATTM_YEAR = BaseForm.BaseYear.Trim();
                }

                if (!string.IsNullOrEmpty(gvsiteSchedule.CurrentRow.Cells["Site"].Value.ToString().Trim()))
                    Entity.ATTM_SITE = gvsiteSchedule.CurrentRow.Cells["Site"].Value.ToString();
                else
                    Entity.ATTM_SITE = "    ";
                if (!string.IsNullOrEmpty(gvsiteSchedule.CurrentRow.Cells["Room"].Value.ToString().Trim()))
                    Entity.ATTM_ROOM = gvsiteSchedule.CurrentRow.Cells["Room"].Value.ToString();
                else
                    Entity.ATTM_ROOM = "    ";
                if (!string.IsNullOrEmpty(gvsiteSchedule.CurrentRow.Cells["Am_Pm"].Value.ToString().Trim()))
                    Entity.ATTM_AMPM = gvsiteSchedule.CurrentRow.Cells["Am_Pm"].Value.ToString();
                else
                    Entity.ATTM_AMPM = " ";
                Entity.ATTM_MONTH = gvsiteSchedule.CurrentRow.Cells["Month_No"].Value.ToString();
                Entity.ATTM_FUND = gvsiteSchedule.CurrentRow.Cells["Fund"].Value.ToString().Trim();
                listSchedule = _model.SPAdminData.Browse_CHILDATTM(Entity, "Browse");
                if (listSchedule.Count > 0)
                {
                    Entity.ATTM_ID = listSchedule[0].ATTM_ID.Trim();
                }
                Entity.Row_Type = "D";
                CaptainModel model = new CaptainModel();
                int TempID = 0;
                if (Privileges.ModuleCode == "01")
                    Added_Edited_HieCode = gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString();
                Added_Edited_Type = "";
                //string code = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString();
                if (_model.SPAdminData.UpdateCHLDATTM(Entity, "Update", out strmsg, out strSqlmsg, out TempID))
                {

                    AlertBox.Show("Site Schedule Deleted Successfully");
                    if (strIndex != 0)
                        strIndex = strIndex - 1;
                    if (Privileges.ModuleCode == "01")
                        fillHieGrid(Added_Edited_HieCode);
                    fillgvSiteSchedule();
                }
                else
                    if (strmsg == "Already Exist")
                    AlertBox.Show("Failed to Delete! \nThis Site has been used somewhere", MessageBoxIcon.Warning);
                else
                    AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);

               PopulateToolbar(oToolbarMnustrip);
            }
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillgvSiteSchedule();
        }

        private void gvsiteSchedule_DoubleClick(object sender, EventArgs e)
        {
            if (Privileges.ModuleCode == "01")
            {
                Siteschedule_Form Site_Form_Edit = new Siteschedule_Form(BaseForm, "View", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvsiteSchedule.CurrentRow.Cells["Month_No"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Site"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Room"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Am_Pm"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Fund"].Value.ToString(), gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Sch_Type"].Value.ToString().Trim(), gvsiteSchedule.CurrentRow.Cells["Calander_Year"].Value.ToString().Trim(), Privileges, gvsiteSchedule.CurrentRow.Cells["Attm_ID"].Value.ToString().Trim());
                Site_Form_Edit.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                Site_Form_Edit.StartPosition = FormStartPosition.CenterScreen;
                Site_Form_Edit.ShowDialog();

            }
            else
            {
                Siteschedule_Form Site_Form_Edit = new Siteschedule_Form(BaseForm, "View", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), gvsiteSchedule.CurrentRow.Cells["Month_No"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Site"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Room"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Am_Pm"].Value.ToString(), gvsiteSchedule.CurrentRow.Cells["Fund"].Value.ToString(), dsHieDesc, BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg, gvsiteSchedule.CurrentRow.Cells["Sch_Type"].Value.ToString().Trim(), gvsiteSchedule.CurrentRow.Cells["Calander_Year"].Value.ToString().Trim(), Privileges, gvsiteSchedule.CurrentRow.Cells["Attm_ID"].Value.ToString().Trim());
                Site_Form_Edit.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                Site_Form_Edit.StartPosition = FormStartPosition.CenterScreen;
                Site_Form_Edit.ShowDialog();

            }
        }

    }
}