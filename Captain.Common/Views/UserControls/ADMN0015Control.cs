#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Model.Data;
using Captain.Common.Exceptions;
using System.Diagnostics;
using System.Linq;
//using System.Windows.Forms;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ADMN0015Control : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string[] strCode = null;
        public int strIndex = 0;
        public int strPageIndex = 1;
        #endregion

        public ADMN0015Control(BaseForm baseform, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileges = privileges;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            hierarchyEntity = _model.lookupDataAccess.GetHierarchyByUserID(null, "I", string.Empty);
            fillYearCombo();
            FormLoad();
            //fillHieGrid();
            //fillCasesiteGrid();

            PopulateToolbar(oToolbarMnustrip);

        }

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

            if (toolBar.Controls.Count == 0)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Add Site";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Site";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Site";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                
                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Help";
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
                //ToolBarPrint,
                ToolBarHelp
            });

            if (gvHie.Rows.Count == 0)
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = false;
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
                        Added_Edited_SiteCode = string.Empty; Added_Edited_HieCode=string.Empty;
                        if (gvHie.Rows.Count > 0)
                        {
                            if (((ListItem)cmbYear.SelectedItem).Text.Trim() == "MASTER")
                            {
                                ADMN0015Form Site_Form_Add = new ADMN0015Form(BaseForm, "Add", "Site", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Trim(), string.Empty, string.Empty, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "", gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), Privileges);
                                Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Add.ShowDialog();
                            }
                            else
                            {
                                ADMN0015Form Site_Form_Add = new ADMN0015Form(BaseForm, "Add", "Site", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "", gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), Privileges);
                                Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Add.ShowDialog();
                            }
                        }
                        else
                        {
                            ADMN0015Form Site_Form_Add = new ADMN0015Form(BaseForm, "Add", "Site", string.Empty, string.Empty, string.Empty, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), "", string.Empty, string.Empty, Privileges);
                            Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                            Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                            Site_Form_Add.ShowDialog();
                        }

                        break;
                    case Consts.ToolbarActions.Edit:
                        if (gvHie.Rows.Count > 0)
                        {
                            if (((ListItem)cmbYear.SelectedItem).Text.Trim() == "MASTER")
                            {
                                ADMN0015Form Site_Form_Add = new ADMN0015Form(BaseForm, "Edit", "Site", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Trim(), string.Empty, string.Empty, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), Privileges);
                                Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Add.ShowDialog();
                            }
                            else
                            {
                                ADMN0015Form Site_Form_Edit = new ADMN0015Form(BaseForm, "Edit", "Site", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), Privileges);
                                Site_Form_Edit.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                                Site_Form_Edit.StartPosition = FormStartPosition.CenterScreen;
                                Site_Form_Edit.ShowDialog();
                            }
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (gvHie.Rows.Count > 0)
                        {
                            strIndex = gvSite.SelectedRows[0].Index;
                            //strPageIndex = gvSite.CurrentPage;
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Site Number: " + gvSite.CurrentRow.Cells["SiteNo"].Value.ToString().Trim(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Site_Row);
                        }
                        break;
                    case Consts.ToolbarActions.Print:

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

        private void fillYearCombo()
        {
            cmbYear.Items.Clear();

            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("MASTER", "    "));
            listItem.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-3).Year.ToString(), DateTime.Now.AddYears(-3).Year.ToString()));
            cmbYear.Items.AddRange(listItem.ToArray());
            cmbYear.SelectedIndex = 0;
        }

        private void FormLoad()
        {
            if (((ListItem)cmbYear.SelectedItem).Text == "MASTER")
            {
                this.City.Width = 195;
                pnlRoom.Visible = false;
                this.gvHie.Size = new System.Drawing.Size(90, 347);
                this.pnlHierarchy.Size = new System.Drawing.Size(102, 351);
                this.gvSite.Size = new System.Drawing.Size(513, 346);
                this.pnlSite.Size = new System.Drawing.Size(520, 351);
                gvSite.Columns["Image_Add"].Visible = false;
            }
            else
            {
                this.City.Width = 157;
                pnlRoom.Visible = true;
                gvSite.Columns["Image_Add"].Visible = false;
                this.gvHie.Size = new System.Drawing.Size(90, 174);
                this.pnlHierarchy.Size = new System.Drawing.Size(102, 178);
                this.gvSite.Size = new System.Drawing.Size(513, 173);
                this.pnlSite.Size = new System.Drawing.Size(520, 178);
            }
        }
        public void fillHieGrid(string HieCode)
        {
            this.gvHie.SelectionChanged -= new System.EventHandler(this.gvHie_SelectionChanged);
            gvHie.Rows.Clear();
            this.gvHie.SelectionChanged += new System.EventHandler(this.gvHie_SelectionChanged);
            int rowIndex = 0; int rowCnt = 0;
            int Sel_HieIndex = 0; string Hier = string.Empty;
            DataSet dsHie = DatabaseLayer.CaseMst.GetCaseSiteHie(((ListItem)cmbYear.SelectedItem).Value.ToString(),BaseForm.UserID,BaseForm.BaseAdminAgency);
            DataTable dtHie = new DataTable();
            if (dsHie.Tables.Count > 0)
                dtHie = dsHie.Tables[0];
            if (dtHie.Rows.Count > 0)
            {
                foreach (DataRow drHie in dtHie.Rows)
                {
                    string CaseHie = string.Empty;
                    string SiteHie = string.Empty;
                    
                    if (((ListItem)cmbYear.SelectedItem).Value.ToString() == "")
                    {
                        SiteHie = drHie["SITE_Hier"].ToString();
                        /*string*/ Hier = drHie["SITE_Hier"].ToString().Trim();
                        if (Hier.Length == 2)
                            CaseHie = Hier + "-" + "**" + "-" + "**";
                    }
                    else
                    {
                        CaseHie = drHie["SITE_Hier"].ToString().Substring(0, 2).Trim() + "-" + drHie["SITE_Hier"].ToString().Substring(2, 2).Trim() + "-" + drHie["SITE_Hier"].ToString().Substring(4, 2).Trim();
                        SiteHie = drHie["SITE_Hier"].ToString();
                    }
                    HierarchyEntity hierachysubEntity = hierarchyEntity.Find(u => u.Code.Equals(CaseHie));
                    if (hierachysubEntity != null)
                    {
                        rowIndex = gvHie.Rows.Add(CaseHie, SiteHie, hierachysubEntity.HirarchyName);
                    }
                    if (cmbYear.SelectedIndex == 0)
                    {
                        CaseHie = Hier;
                        if (HieCode.Trim() == CaseHie) /*.ToString().Substring(0, 2).Trim()*/
                            Sel_HieIndex = rowCnt;
                    }
                    else
                    {
                        if (HieCode.Trim() == CaseHie)
                            Sel_HieIndex = rowCnt;
                    }
                    rowCnt++;
                }
                
            }

            if (gvHie.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(HieCode))
                {
                    gvHie.Rows[0].Tag = 0;
                    gvHie.CurrentCell = gvHie.Rows[0].Cells[0];
                    gvHie.Rows[0].Selected = true;
                }
                else
                {
                    this.gvHie.SelectionChanged -= new System.EventHandler(this.gvHie_SelectionChanged);
                    gvHie.CurrentCell = gvHie.Rows[Sel_HieIndex].Cells[0];
                    gvHie.Rows[Sel_HieIndex].Selected = true;

                    int scrollPosition = 0;
                    scrollPosition = gvHie.CurrentCell.RowIndex;
                    //int CurrentPage = (scrollPosition / gvHie.ItemsPerPage);
                    //CurrentPage++;
                    //gvHie.CurrentPage = CurrentPage;
                    //gvHie.FirstDisplayedScrollingRowIndex = scrollPosition;
                    this.gvHie.SelectionChanged += new System.EventHandler(this.gvHie_SelectionChanged);
                    gvHie_SelectionChanged(gvHie, EventArgs.Empty);
                }
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = true;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = true;
                //if (ToolBarPrint != null)
                //    ToolBarPrint.Enabled = true;
            }
            else
            {
                gvSite.Rows.Clear();
                gvRoom.Rows.Clear();
                
                if (gvHie.Rows.Count == 0)
                {
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = false;
                    //if (ToolBarPrint != null)
                    //    ToolBarPrint.Enabled = false;
                }
            }
        }

        List<CaseSiteEntity> Site_List;
        private void fillCasesiteGrid(string Sel_SiteCd)
        {
            this.gvSite.SelectionChanged -= new System.EventHandler(this.gvSite_SelectionChanged);
            gvSite.Rows.Clear();
            this.gvSite.SelectionChanged += new System.EventHandler(this.gvSite_SelectionChanged);
            int rowIndex = 0; int RowCnt = 0;
            int Sel_Site_Index = 0;
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            if (gvHie.Rows.Count > 0)
            {
                string Agency = string.Empty; string Dept = string.Empty; string Prog = string.Empty;
                string Hierarchy = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Trim();
                if (((ListItem)cmbYear.SelectedItem).Text.Trim() == "MASTER")
                    Agency = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Trim();
                else
                {
                    Agency = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2).Trim();
                    Dept = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                    Prog = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                }

                Search_Entity.SiteAGENCY = Agency;
                Search_Entity.SiteDEPT = Dept; Search_Entity.SitePROG = Prog;
                Search_Entity.SiteYEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                Search_Entity.SiteROOM = "0000";

                if (Search_Entity.SiteYEAR == "")
                {
                    this.City.Width = 195; this.SiteName.Width = 237;
                    pnlRoom.Visible = false;
                    this.gvHie.Size = new System.Drawing.Size(90, 347);
                    this.pnlHierarchy.Size = new System.Drawing.Size(102, 351);
                    this.gvSite.Size = new System.Drawing.Size(513, 346);
                    this.pnlSite.Size = new System.Drawing.Size(520, 351);
                    gvSite.Columns["Image_Add"].Visible = false;
                }
                else
                {
                    this.City.Width = 157;
                    //this.SiteName.Width = 230;
                    pnlRoom.Visible = true;
                    gvSite.Columns["Image_Add"].Visible = false;
                    this.gvHie.Size = new System.Drawing.Size(90, 174);
                    this.pnlHierarchy.Size = new System.Drawing.Size(102, 178);
                    this.gvSite.Size = new System.Drawing.Size(513, 173);
                    this.pnlSite.Size = new System.Drawing.Size(520, 178);
                }

                Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                if (Site_List.Count > 0)
                {
                    Site_List = Site_List.OrderByDescending(u => u.SiteACTIVE).ToList();
                    foreach (CaseSiteEntity Entity in Site_List)
                    {
                        rowIndex = gvSite.Rows.Add(Entity.SiteNUMBER.Trim(), Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim());
                        if(Entity.SiteACTIVE=="N")
                            gvSite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        string toolTipText = "Added By     : " + Entity.SiteADD_OPERATOR.ToString().Trim() + " on " + Entity.SiteDATE_ADD.ToString() + "\n" +
                                            "Modified By  : " + Entity.SiteLSTC_OPERATOR.ToString().Trim() + " on " + Entity.SiteDATE_LSTC.ToString();
                        foreach (DataGridViewCell cell in gvSite.Rows[rowIndex].Cells)
                        {
                            cell.ToolTipText = toolTipText;
                        }
                        if (Sel_SiteCd.Trim() == Entity.SiteNUMBER)
                            Sel_Site_Index = RowCnt;
                        RowCnt++;

                    }
                }
                
            }

            //MessageBox.Show("Jayaram" , "fillCasesiteGrid");
            
            if (gvSite.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(Sel_SiteCd))
                {
                    gvSite.Rows[0].Tag = 0;
                    gvSite.Rows[0].Selected = true;
                }
                else
                {
                    this.gvSite.SelectionChanged -= new System.EventHandler(this.gvSite_SelectionChanged);
                    gvSite.CurrentCell = gvSite.Rows[Sel_Site_Index].Cells[1];
                    gvSite.Rows[Sel_Site_Index].Selected = true;

                    int scrollPosition = 0;
                    scrollPosition = gvSite.CurrentCell.RowIndex;
                    //int CurrentPage = (scrollPosition / gvSite.ItemsPerPage);
                    //CurrentPage++;
                    //gvSite.CurrentPage = CurrentPage;
                    //gvSite.FirstDisplayedScrollingRowIndex = scrollPosition;
                    this.gvSite.SelectionChanged += new System.EventHandler(this.gvSite_SelectionChanged);
                    gvSite_SelectionChanged(gvSite, EventArgs.Empty);
                }
            }

            if (Privileges.AddPriv.Equals("false"))
                gvSite.Columns["Image_Add"].Visible = false;
            else
            {
                if(((ListItem)cmbYear.SelectedItem).Text!="MASTER")
                    gvSite.Columns["Image_Add"].Visible = true;
            }
        }


        private void fillCaseSite_Roomgrid(string RoomCd)//string Site_cd
        {
            gvRoom.Rows.Clear();
            int rowIndex = 0; int RowCnt = 0;
            int Sel_Site_Index = 0;

            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            if (gvHie.Rows.Count > 0)
            {
                string Agency = string.Empty; string Dept = string.Empty; string Prog = string.Empty;
                string Hierarchy = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Trim();
                if (((ListItem)cmbYear.SelectedItem).Text.Trim() == "MASTER")
                    Agency = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Trim();
                else
                {
                    Agency = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2).Trim();
                    Dept = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                    Prog = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                }
                string Site = gvSite.CurrentRow.Cells["SiteNo"].Value.ToString().Trim();

                Search_Entity.SiteAGENCY = Agency;
                Search_Entity.SiteDEPT = Dept; Search_Entity.SitePROG = Prog;
                Search_Entity.SiteYEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                Search_Entity.SiteNUMBER = Site;
                Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                if (Site_List.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in Site_List)
                    {
                        if (Entity.SiteROOM != "0000")
                        {
                            string Language = "---None---";
                            if (!string.IsNullOrEmpty(Entity.SiteLANGUAGE))
                            {
                                DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                                DataTable dtLang = dsLang.Tables[0];
                                foreach (DataRow drLang in dtLang.Rows)
                                {
                                    if (Entity.SiteLANGUAGE.Trim() == drLang["Code"].ToString().Trim())
                                    {
                                        Language = drLang["LookUpDesc"].ToString().Trim(); break;
                                    }
                                }
                            }

                            rowIndex = gvRoom.Rows.Add(Entity.SiteROOM.Trim(), Entity.SiteAM_PM.Trim(), Language, LookupDataAccess.Getdate(Entity.SiteCLASS_START.Trim()), LookupDataAccess.Getdate(Entity.SiteCLASS_END.Trim()), LookupDataAccess.GetTime(Entity.SiteSTART_TIME.Trim()), LookupDataAccess.GetTime(Entity.SiteEND_TIME.Trim()));
                            string toolTipText = "Added By     : " + Entity.SiteADD_OPERATOR.ToString().Trim() + " on " + Entity.SiteDATE_ADD.ToString() + "\n" +
                                                "Modified By  : " + Entity.SiteLSTC_OPERATOR.ToString().Trim() + " on " + Entity.SiteDATE_LSTC.ToString();
                            foreach (DataGridViewCell cell in gvRoom.Rows[rowIndex].Cells)
                            {
                                cell.ToolTipText = toolTipText;
                            }
                            if (RoomCd.Trim() == Entity.SiteROOM)
                                Sel_Site_Index = RowCnt;
                            RowCnt++;
                        }
                    }
                    //gvRoom.Rows[0].Tag = 0;
                }
                if (gvRoom.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(RoomCd))
                    {
                        gvRoom.Rows[0].Tag = 0;
                        gvRoom.Rows[0].Selected = true;
                    }
                    else
                    {
                        gvRoom.CurrentCell = gvRoom.Rows[Sel_Site_Index].Cells[1];
                        gvRoom.Rows[Sel_Site_Index].Selected = true;

                        int scrollPosition = 0;
                        scrollPosition = gvRoom.CurrentCell.RowIndex;
                        //int CurrentPage = (scrollPosition / gvRoom.ItemsPerPage);
                        //CurrentPage++;
                        //gvRoom.CurrentPage = CurrentPage;
                        //gvRoom.FirstDisplayedScrollingRowIndex = scrollPosition;
                    }
                }
            }

            if (Privileges.ChangePriv.Equals("false"))
                gvRoom.Columns["Img_Edit"].Visible = false;
            else
                gvRoom.Columns["Img_Edit"].Visible = true;
            if (Privileges.DelPriv.Equals("false"))
                gvRoom.Columns["Img_Del"].Visible = false;
            else
                gvRoom.Columns["Img_Del"].Visible = true;
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Added_Edited_HieCode = string.Empty;
            fillHieGrid(Added_Edited_HieCode);
        }

        private void gvHie_SelectionChanged(object sender, EventArgs e)
        {
            fillCasesiteGrid(Added_Edited_SiteCode);
        }

        string Added_Edited_SiteCode = string.Empty; string Added_Edited_HieCode = string.Empty; 
        private void Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            ADMN0015Form form = sender as ADMN0015Form;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Code();
                Added_Edited_SiteCode = From_Results[0];
                Added_Edited_HieCode = From_Results[2];
                //Mode = From_Results[1];
                //Added_Edited_Type = From_Results[2];
                if (From_Results[1].Equals("Add"))
                {
                    AlertBox.Show("Site Details Saved Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Site Details Inserted Successfully...", "CAP Systems");
                }
                else
                    AlertBox.Show("Site Details Updated Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                //MessageBox.Show("Site Details Updated Successfully...", "CAP Systems");

                fillHieGrid(Added_Edited_HieCode);
                
            }
        }
        string Added_Edited_RoomCode = string.Empty;
        private void Room_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            ADMN0015Form form = sender as ADMN0015Form;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[4];
                From_Results = form.GetSelected_Room_Code();
                Added_Edited_RoomCode = From_Results[0];
                Added_Edited_SiteCode = From_Results[2];
                Added_Edited_HieCode = From_Results[3];
                //Mode = From_Results[1];
                //Added_Edited_Type = From_Results[2];
                if (From_Results[1].Equals("Add"))
                {
                    AlertBox.Show("Room Details Saved Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Room Details Inserted Successfully...", "CAP Systems");
                }
                else
                {
                    AlertBox.Show("Room Details Updated Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Room Details Updated Successfully...", "CAP Systems");
                }
                //FillVendorGrid(Added_Edited_VendorCode);
                
                fillHieGrid(Added_Edited_HieCode);
                //fillCasesiteGrid(Added_Edited_SiteCode);
                //fillCaseSite_Roomgrid(Added_Edited_RoomCode);
            }
        }



        public void Delete_Site_Row(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
                if (dialogresult == DialogResult.Yes)
                {
                    string strmsg = string.Empty; string strSqlmsg = string.Empty;
                    CaseSiteEntity Entity = new CaseSiteEntity(true);
                    Entity.Row_Type = "D";
                    if (((ListItem)cmbYear.SelectedItem).Text.Trim() == "MASTER")
                    {
                        Entity.SiteAGENCY = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2);
                        Entity.SiteDEPT = "  ";
                        Entity.SitePROG = "  ";
                    }
                    else
                    {
                        Entity.SiteAGENCY = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2);
                        Entity.SiteDEPT = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                        Entity.SitePROG = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                    }
                    if(string.IsNullOrEmpty(((ListItem)cmbYear.SelectedItem).Value.ToString().Trim()))
                        Entity.SiteYEAR = "    ";
                    else
                        Entity.SiteYEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                    Entity.SiteNUMBER = gvSite.CurrentRow.Cells["SiteNo"].Value.ToString();
                    Entity.SiteROOM = "0000"; Entity.SiteAM_PM = " ";
                    CaptainModel model = new CaptainModel();
                    Added_Edited_SiteCode = string.Empty;
                    Added_Edited_HieCode = gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString();
                    
                    //string code = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString();
                    if (_model.CaseMstData.UpdateCASESITE(Entity, "Update", out strmsg, out strSqlmsg))
                    {

                    AlertBox.Show("Site Number: " + gvSite.CurrentRow.Cells["SiteNo"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Site Number for " + gvSite.CurrentRow.Cells["SiteNo"].Value.ToString() + " " + "Deleted Successfully", "CAP Systems", MessageBoxButtons.OK);
                        if (strIndex != 0)
                            strIndex = strIndex - 1;
                        fillHieGrid(Added_Edited_HieCode);
                        //fillCasesiteGrid(Added_Edited_SiteCode);
                        //fillCaseSite_Roomgrid(Added_Edited_RoomCode);
                    }
                    else
                    {
                    if (strmsg == "CHLDATTM")
                        AlertBox.Show("Unable to Delete! \nThis Site has been used in Site Schedule", MessageBoxIcon.Warning);
                    else if (strmsg == "CASESITE")
                            AlertBox.Show("Unable to Delete! \nThis Site has been used in Site/Room Maintenance", MessageBoxIcon.Warning);
                        else if (strmsg == "CASEMS")
                            AlertBox.Show("Unable to Delete! \nThis Site has been used in Service Posting for Outcome", MessageBoxIcon.Warning);
                        else if (strmsg == "CASEACT")
                            AlertBox.Show("Unable to Delete! \nThis Site has been used in Service Posting for Service", MessageBoxIcon.Warning);
                        else if (strmsg == "CASESNP" || strmsg == "CASEMST")
                            AlertBox.Show("Unable to Delete! \nThis Site has been used in Client Intake", MessageBoxIcon.Warning);
                        else if (strmsg == "CASEENRL")
                            AlertBox.Show("Unable to Delete! \nThis Site has been used in Enrollment", MessageBoxIcon.Warning);
                        else if (strmsg == "CHLDATTN")
                            AlertBox.Show("Unable to Delete! \nThis Site has been used in Attendance Posting", MessageBoxIcon.Warning);
                        else
                            AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);
                    }
                }
            //}
        }


        public void Delete_Room_Row(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
                if (dialogresult == DialogResult.Yes)
                {
                    string strmsg = string.Empty; string strSqlmsg = string.Empty;
                    CaseSiteEntity Entity = new CaseSiteEntity(true);
                    Entity.Row_Type = "D";
                    Entity.SiteAGENCY = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2);
                    Entity.SiteDEPT = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                    Entity.SitePROG = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                    Entity.SiteYEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                    Entity.SiteNUMBER = gvSite.CurrentRow.Cells["SiteNo"].Value.ToString();
                    Entity.SiteROOM = gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString(); Entity.SiteAM_PM = gvRoom.CurrentRow.Cells["Site_AmPm"].Value.ToString();
                    CaptainModel model = new CaptainModel();
                    Added_Edited_SiteCode = gvSite.CurrentRow.Cells["SiteNo"].Value.ToString();
                    Added_Edited_HieCode = gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString();
                    Added_Edited_RoomCode = string.Empty;
                    //string code = gvAgencyReferal.CurrentRow.Cells["Code"].Value.ToString();
                    if (_model.CaseMstData.UpdateCASESITE(Entity, "Update", out strmsg, out strSqlmsg))
                    {

                    AlertBox.Show("Room Number: " + gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Room Number for " + gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString() + " " + "Deleted Successfully", "CAP Systems", MessageBoxButtons.OK);
                        if (strIndex != 0)
                            strIndex = strIndex - 1;
                        fillHieGrid(Added_Edited_HieCode);
                        //fillCasesiteGrid(Added_Edited_SiteCode);
                        //fillCaseSite_Roomgrid(Added_Edited_RoomCode);
                    }
                    else
                        if (strmsg == "Already Exist")
                            AlertBox.Show("This Room has been used somewhere, so unable to Delete", MessageBoxIcon.Warning);
                        else
                            AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);
                }
            //}
        }


        private void gvSite_SelectionChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbYear.SelectedItem).Value.ToString() != "")
               fillCaseSite_Roomgrid(Added_Edited_RoomCode);//Added_Edited_SiteCode
        }

        private void gvSite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1) 
            {
                if (Privileges.AddPriv.Equals("true"))
                {
                    Added_Edited_RoomCode = string.Empty;
                    ADMN0015Form Room_Form = new ADMN0015Form(BaseForm, "Add", "Room", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), "RoomCd","", Privileges);
                    Room_Form.FormClosed += new FormClosedEventHandler(Room_AddForm_Closed);
                    Room_Form.StartPosition = FormStartPosition.CenterScreen;
                    Room_Form.ShowDialog();
                }
            }
        }

        private void gvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.ChangePriv.Equals("true"))
            {
                if (e.ColumnIndex == 7 && e.RowIndex != -1)
                {
                    //Added_Edited_RoomCode = string.Empty;
                    ADMN0015Form Room_Form = new ADMN0015Form(BaseForm, "Edit", "Room", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString().Trim(), gvRoom.CurrentRow.Cells["Site_AmPm"].Value.ToString().Trim(), Privileges);
                    Room_Form.FormClosed += new FormClosedEventHandler(Room_AddForm_Closed);
                    Room_Form.StartPosition = FormStartPosition.CenterScreen;
                    Room_Form.ShowDialog();
                }
            }
            if (Privileges.DelPriv.Equals("true"))
            {
                if (e.ColumnIndex == 8 && e.RowIndex != -1)
                {
                    strIndex = gvSite.SelectedRows[0].Index;
                    //strPageIndex = gvSite.CurrentPage;
                    string Message = string.Empty;
                    List<CaseEnrlEntity> Enroll_List = new List<CaseEnrlEntity>();
                    CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
                    Search_Entity.Join_Mst_Snp = "Y";
                    Search_Entity.Agy = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2);
                    Search_Entity.Dept = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                    Search_Entity.Prog = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                    Search_Entity.Year = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                    Search_Entity.Rec_Type = "H";
                    Search_Entity.Site = gvSite.CurrentRow.Cells["SiteNo"].Value.ToString();
                    Search_Entity.Room = gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString();
                    Search_Entity.AMPM = gvRoom.CurrentRow.Cells["Site_AmPm"].Value.ToString();

                    Enroll_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");
                    Enroll_List = Enroll_List.FindAll(u => u.Status.Trim() != "T");
                    if (Enroll_List.Count > 0) Message = "Client enrolled so you can not delete";

                    //if (string.IsNullOrEmpty(Message.Trim()))
                    //{
                    //    List<ChldAttnEntity> App_Attn_List = new List<ChldAttnEntity>();
                    //    App_Attn_List = _model.ChldAttnData.GetChldAttnDetails(gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString(), string.Empty, gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString(), gvRoom.CurrentRow.Cells["Site_AmPm"].Value.ToString(), string.Empty, string.Empty);
                    //    if (App_Attn_List.Count > 0)
                    //        Message = "Client having ";
                    //}

                    if (string.IsNullOrEmpty(Message.Trim()))
                    {
                        List<SiteScheduleEntity> SchduleList = new List<SiteScheduleEntity>();
                        SiteScheduleEntity site_Search = new SiteScheduleEntity(true);
                        site_Search.ATTM_AGENCY = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2);
                        site_Search.ATTM_DEPT = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2);
                        site_Search.ATTM_PROG = gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2);
                        site_Search.ATTM_YEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                        site_Search.ATTM_SITE = gvSite.CurrentRow.Cells["SiteNo"].Value.ToString();
                        site_Search.ATTM_ROOM = gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString();
                        site_Search.ATTM_AMPM = gvRoom.CurrentRow.Cells["Site_AmPm"].Value.ToString();
                        SchduleList = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                        if (SchduleList.Count > 0) Message = "This Room is Schduled so unable to delete";
                    }
                    if (string.IsNullOrEmpty(Message.Trim()))
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Room Number: " + gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Room_Row);
                    else
                        AlertBox.Show(Message, MessageBoxIcon.Warning);
                }
            }
        }

        private void gvSite_DoubleClick(object sender, EventArgs e)
        {
            if (((ListItem)cmbYear.SelectedItem).Text.Trim() == "MASTER")
            {
                ADMN0015Form Site_Form_Add = new ADMN0015Form(BaseForm, "View", "Site", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Trim(), string.Empty, string.Empty, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), Privileges);
                Site_Form_Add.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                Site_Form_Add.StartPosition = FormStartPosition.CenterScreen;
                Site_Form_Add.ShowDialog();
            }
            else
            {
                ADMN0015Form Site_Form_Edit = new ADMN0015Form(BaseForm, "View", "Site", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), gvHie.CurrentRow.Cells["Hier_Desc"].Value.ToString(), gvHie.CurrentRow.Cells["Hierarchy"].Value.ToString(), Privileges);
                Site_Form_Edit.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                Site_Form_Edit.StartPosition = FormStartPosition.CenterScreen;
                Site_Form_Edit.ShowDialog();
            }
        }

        private void gvRoom_DoubleClick(object sender, EventArgs e)
        {
            ADMN0015Form Room_Form = new ADMN0015Form(BaseForm, "View", "Room", gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(0, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(2, 2), gvHie.CurrentRow.Cells["Site_Hierarchy"].Value.ToString().Substring(4, 2), ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), gvSite.CurrentRow.Cells["SiteNo"].Value.ToString(), gvRoom.CurrentRow.Cells["Site_Room"].Value.ToString().Trim(), gvRoom.CurrentRow.Cells["Site_AmPm"].Value.ToString().Trim(), Privileges);
            Room_Form.FormClosed += new FormClosedEventHandler(Room_AddForm_Closed);
            Room_Form.StartPosition = FormStartPosition.CenterScreen;
            Room_Form.ShowDialog();
        }


    }
}