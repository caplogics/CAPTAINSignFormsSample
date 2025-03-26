#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
using Wisej.Design;
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
using System.Text.RegularExpressions;
using System.IO;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Captain.Common.Views.Controls.Compatibility;

#endregion
namespace Captain.Common.Views.UserControls
{
    public partial class RNG00001 : BaseUserControl
    {
        #region private variables
        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Blank = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Tick = "icon-gridtick";
        string Img_Blank = "blank";

        #endregion
        public RNG00001(BaseForm baseform, PrivilegeEntity Priviliages)
        {
            InitializeComponent();

            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = Priviliages;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            SetGridColsOnPrivileges();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";
            txtfrom.Validator = TextBoxValidation.IntegerValidator;
            txtTo.Validator = TextBoxValidation.IntegerValidator;
            FormLoad();

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

        public bool IsSaveValid { get; set; }
        public string propReportPath { get; set; }

        //public List<HierarchyEntity> SelectedHierarchies
        //{
        //    get
        //    {
        //        return _selectedHierarchies = (from b in gvSerivePlanHie.Rows.Cast<DataGridViewRow>().ToList()
        //                                       select ((DataGridViewRow)b).Tag as HierarchyEntity).ToList();

        //    }
        //}

        #endregion
        string strFolderPath = string.Empty;
        string Fdate = null, Tdate = null;

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
                ToolBarNew.ToolTipText = "Add Outcome Range";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Outcome Range";

                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Outcome Range";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarPrint = new ToolBarButton();
                ToolBarPrint.Tag = "Print";
                ToolBarPrint.ToolTipText = "Print";
                ToolBarPrint.Enabled = true;
                ToolBarPrint.ImageSource = "captain-print";
                ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

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
                ToolBarPrint,
                ToolBarHelp
            });

            if (gvRdateRange.Rows.Count == 0)
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = false;
            }

        }

        string PdfMode = null, PrintType = null; string tab_Page = "RPerfMeasures";
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
                        if (tab_Page == "RPerfMeasures")
                        {
                            RPerformanceMeasureForm Add_PMDate = new RPerformanceMeasureForm(BaseForm, "Add", "Code", string.Empty, string.Empty, string.Empty, string.Empty, "AddMode", Privileges, "RPerfMeasures");
                            Add_PMDate.FormClosed += new FormClosedEventHandler(RDate_AddForm_Closed);
                            Add_PMDate.StartPosition = FormStartPosition.CenterScreen;
                            Add_PMDate.ShowDialog();
                        }
                        else if (tab_Page == "Demographics")
                        {
                            Selmode = "Add";
                            btnSave.Visible = true;
                            btnCancel.Visible = true;
                            txtfrom.Enabled = false;
                            txtTo.Enabled = false;
                            lblARTo.Visible = false;
                            btnSave.Text = "Save";
                            if (dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString().Trim() == "I")
                                dgvDemographics.Enabled = false;

                        }
                        else if (tab_Page == "RServices")
                        {
                            RPerformanceMeasureForm Add_PMDate = new RPerformanceMeasureForm(BaseForm, "Add", "Code", string.Empty, string.Empty, string.Empty, string.Empty, "AddMode", Privileges, "RServices");
                            Add_PMDate.FormClosed += new FormClosedEventHandler(SRDate_AddForm_Closed);
                            Add_PMDate.StartPosition = FormStartPosition.CenterScreen;
                            Add_PMDate.ShowDialog();
                        }

                        break;
                    case Consts.ToolbarActions.Edit:
                        if (tab_Page == "RPerfMeasures")
                        {
                            if (gvRdateRange.Rows.Count > 0)
                            {
                                RPerformanceMeasureForm Edit_PMDate = new RPerformanceMeasureForm(BaseForm, "Edit", "Code", string.Empty, string.Empty, gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim(), gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim(), "AddMode", Privileges, "RPerfMeasures");
                                //DateRange_PMeasures Edit_PMDate = new DateRange_PMeasures(BaseForm, "Edit", gvRdateRange.CurrentRow.Cells["RFrom_Date"].Value.ToString().Trim(), gvRdateRange.CurrentRow.Cells["RTo_Date"].Value.ToString().Trim(), Privileges);
                                Edit_PMDate.FormClosed += new FormClosedEventHandler(RDate_AddForm_Closed);
                                Edit_PMDate.StartPosition = FormStartPosition.CenterScreen;
                                Edit_PMDate.ShowDialog();
                            }
                        }
                        else if (tab_Page == "Demographics")
                        {
                            Selmode = "Edit";
                            btnSave.Visible = true;
                            btnCancel.Visible = true;
                            txtfrom.Enabled = true;
                            txtTo.Enabled = true;
                            if (dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString().Trim() == "I")
                                dgvDemographics.Enabled = false;
                            btnSave.Text = "&Update";
                        }
                        else if (tab_Page == "RServices")
                        {
                            if (gvSRdateRange.Rows.Count > 0)
                            {
                                RPerformanceMeasureForm Edit_PMDate = new RPerformanceMeasureForm(BaseForm, "Edit", "Code", string.Empty, string.Empty, gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim(), gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim(), "AddMode", Privileges, "RServices");
                                Edit_PMDate.FormClosed += new FormClosedEventHandler(SRDate_AddForm_Closed);
                                Edit_PMDate.StartPosition = FormStartPosition.CenterScreen;
                                Edit_PMDate.ShowDialog();
                            }
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        //Selmode = "Delete";

                        if (tab_Page == "RPerfMeasures")
                        {
                            if (gvRdateRange.Rows.Count > 0)
                            {
                                //int RowCnt = 0;
                                RGroupList = _model.SPAdminData.Browse_RNGGrp(gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim(), gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim(), null, null, null,BaseForm.UserID, BaseForm.BaseAdminAgency);
                                if (RGroupList.Count > 1)
                                    MessageBox.Show("This having Domain So unable to Delete this code", "CAPTAIN", MessageBoxButtons.OK);
                                else
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_CAMS_Row);
                            }
                        }
                        else if (tab_Page == "Demographics")
                        {
                            List<RNG4AsocEntity> RNG4Entity;
                            RNG4Entity = _model.SPAdminData.Browse_RNG4Assoc(dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString().Trim(), dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString().Trim());
                            if (RNG4Entity.Count > 0)
                            {
                                strIndex = dgvDemographics.SelectedRows[0].Index;
                                //strPageIndex = dgvDemographics.CurrentPage;
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Demogrphics with code - " + dgvDemographics.CurrentRow.Cells["Demographics"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_CAMS_Row);
                            }
                            else
                                MessageBox.Show("No Association for this Demographics", "CAPTAIN");
                        }
                        if (tab_Page == "RServices")
                        {
                            if (gvSRdateRange.Rows.Count > 0)
                            {
                                //int RowCnt = 0;
                                SRGroupList = _model.SPAdminData.Browse_RNGSRGrp(gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim(), gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim(), null, null, null);
                                if (SRGroupList.Count > 1)
                                    MessageBox.Show("This having Group, so unable to Delete this code", "CAPTAIN", MessageBoxButtons.OK);
                                else
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_CAMS_Row);
                            }
                        }

                        break;
                    case Consts.ToolbarActions.Print:
                        //if (tab_Page == "CA")
                        //{
                        //    if (gvCriticalActivity.Rows.Count > 0)
                        //    {
                        //        ADMN4016PrintForm printForm = new ADMN4016PrintForm(BaseForm, "Print", tab_Page, Privileges);
                        //        printForm.ShowDialog();
                        //    }
                        //}
                        //else if (tab_Page == "MS")
                        //{
                        //    if (GrvMS.Rows.Count > 0)
                        //    {
                        //        ADMN4016PrintForm printForm = new ADMN4016PrintForm(BaseForm, "Print", tab_Page, Privileges);
                        //        printForm.ShowDialog();
                        //    }
                        //}
                        if (tab_Page == "Demographics")
                        {
                            On_SaveForm_Closed();
                        }
                        else if (tab_Page == "RPerfMeasures")
                        {
                            On_SaveForm_Closed();
                        }
                        else if (tab_Page == "RServices")
                        {
                            On_SaveForm_Closed();
                        }
                        //else if (tab_Page == "RankingCategories")
                        //{
                        //    List<RNKCRIT1Entity> RankHieList;
                        //    RankHieList = _model.SPAdminData.GetRNKCRIT(null);
                        //    if (RankHieList.Count > 0)
                        //    {
                        //        RankDefPDF PDFForm = new RankDefPDF(BaseForm, Privileges);
                        //        PDFForm.ShowDialog();
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("No records Found", "CAPTAIN");
                        //    }
                        //}
                        //else if (tab_Page == "Groups")
                        //{
                        //    On_Preass_SaveForm_Closed();
                        //}
                        break;
                    case Consts.ToolbarActions.Help:
                        //if (tab_Page == "PerformanceAss")
                        //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Performance");
                        //if (tab_Page == "Demographics")
                        //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN0016_Demo");
                        //else
                        //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN0016");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }


        private void SetGridColsOnPrivileges()
        {
            //int TmpCount = 3;
            if (Privileges.AddPriv.Equals("false"))
            {
                gvRGroup.Columns[3].ReadOnly = true;
                this.RAddGroupImg.ReadOnly = true;
                //this.RImg_add.ReadOnly = true;


            }
            if (Privileges.ChangePriv.Equals("false"))
            {
                //this.Img_Edit.ReadOnly = true;
                this.REditTable.ReadOnly = true;
                gvRTable.Columns[3].ReadOnly = true;
                //if (Privileges.AddPriv.Equals("false"))
                //    this.dataGridViewImageColumn2.ReadOnly = true;
                //GvRankCat.Columns[3].ReadOnly = true;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvRTable.Columns[4].ReadOnly = true;
                this.RDelTable.ReadOnly = true;
                //GvPointRngCat.Columns[6].ReadOnly = true;
                //this.dataGridViewImageColumn1.ReadOnly = true;
            }
        }


        public void FormLoad()
        {
            //DSave.Visible = false;
            //DCancel.Visible = false;
            //txtTo.Enabled = false;
            //txtfrom.Enabled = false;
            //FillCritiactivityGrid(Added_Edited_CACode);
            //FillMilestoneGrid(Added_Edited_MSCode);
            //FillComboCategories();
            ////fillComboRefDate();
            //FillDateRange_Grid();
            FillRDateRange_Grid();
            FillSRDateRange_Grid();
            //FillPerformanceGroups(Added_Edited_GroupCode);
            //fillPerformTable(Added_Edited_TableCode);
            //FillAgencyCombo();
            //FillRanksGv(Added_Edited_RankCode);
            //FillPreassGroups(strPreassGroupcode);
            FillCategories();
        }

        DataSet dscategories = DatabaseLayer.SPAdminDB.Get_RNG4CATG();

        private void FillCategories()
        {
            // dtCat = dscategories.Tables[0];
            this.cmbCategories.SelectedIndexChanged -= new System.EventHandler(this.cmbCategories_SelectedIndexChanged);
            cmbCategories.Items.Clear();
            List<RepListItem> listItem = new List<RepListItem>();

            DataRow[] drCat = dscategories.Tables[0].Select("RNG4CATG_ACTIVE='A' AND RNG4CATG_DEM <> 'NULL'");
            if (drCat.Length > 0)
            {
                for (int x = 0; x < drCat.Length; x++)
                {
                    cmbCategories.Items.Add(new RepListItem(drCat[x]["RNG4CATG_DESC"].ToString(), drCat[x]["RNG4CATG_CODE"].ToString(), drCat[x]["RNG4CATG_AGY_TYPE"].ToString()));
                }
            }
            //foreach (DataRow drcat in dscategories.Tables[0].Rows)
            //{
            //    if (string.IsNullOrWhiteSpace(drcat["CSB4CATG_DEM"].ToString()) && drcat["CSB4CATG_ACTIVE"].ToString().Trim() == "A")
            //        cmbCategories.Items.Add(new RepListItem(drcat["CSB4CATG_DESC"].ToString(), drcat["CSB4CATG_CODE"].ToString(), drcat["CSB4CATG_AGY_TYPE"].ToString()));
            //}
            //if (dscategories.Tables[0].Rows.Count > 0)

            this.cmbCategories.SelectedIndexChanged += new System.EventHandler(this.cmbCategories_SelectedIndexChanged);
            if (cmbCategories.Items.Count > 0)
                cmbCategories.SelectedIndex = 0;
        }

        string Cat_Agytype = null;
        string cat_code = null;
        string Temp_Agy_Codes = null; string Temp_Code = null;
        string Added_Edited_DemoCode = string.Empty;
        string Selmode = "View";
        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            Added_Edited_DemoCode = string.Empty;
            _errorProvider.SetError(txtTo, null);
            Cat_Agytype = ((RepListItem)cmbCategories.SelectedItem).ID.ToString();
            cat_code = ((RepListItem)cmbCategories.SelectedItem).Value.ToString();

            fillGvDemo(Added_Edited_DemoCode);
            btnSave.Visible = false;
            btnCancel.Visible = false;
            txtfrom.Enabled = false;
            txtTo.Enabled = false;
            lblARTo.Visible = false;
            Selmode = "View";
        }

        string IncCodes = string.Empty, EMPCodes = string.Empty, NCSCodes = string.Empty;
        private void Label13columnvalues(DataTable dtAssoc)
        {
            IncCodes = string.Empty; EMPCodes = string.Empty; NCSCodes = string.Empty; string Assoc_Code = string.Empty;
            DataRow[] drAssoc = dtAssoc.Select("RNG4ASOC_CATG_CODE='I' and RNG4ASOC_DEMO_CODE='a'");
            if (drAssoc.Length > 0)
                Assoc_Code = drAssoc[0]["RNG4ASOC_AGYTAB_CODES"].ToString();

            string[] ascode = Assoc_Code.Split(' ');
            for (int j = 0; j < ascode.Length; j++)
            {
                if (!string.IsNullOrEmpty(ascode[j].ToString().Trim()))
                    EMPCodes += ascode[j].ToString().Trim() + ",";
            }
            if (!string.IsNullOrEmpty(EMPCodes.Trim())) EMPCodes = EMPCodes.Substring(0, EMPCodes.Length - 1);

            DataTable dt13b = new DataTable();
            DataView dv = new DataView(dtAssoc);
            dv.RowFilter = "RNG4ASOC_CATG_CODE='J'";
            dt13b = dv.ToTable();
            Assoc_Code = string.Empty;
            if (dt13b.Rows.Count > 0)
            {
                foreach (DataRow dr in dt13b.Rows)
                {
                    Assoc_Code = Assoc_Code + dr["RNG4ASOC_AGYTAB_CODES"].ToString();
                    //INCASOC += dr["RNG4ASOC_AGYTAB_CODES"].ToString();
                }
                string[] incode = Assoc_Code.Split(' ');
                for (int j = 0; j < incode.Length; j++)
                {
                    if (!string.IsNullOrEmpty(incode[j].ToString().Trim()))
                        IncCodes += incode[j].ToString().Trim() + ",";
                }
            }
            if (!string.IsNullOrEmpty(IncCodes.Trim())) IncCodes = IncCodes.Substring(0, IncCodes.Length - 1);

            DataTable dt13b1 = new DataTable();//dtAssoc.Select("RNG4ASOC_CATG_CODE='" + drcat1[x]["RNG4CATG_CODE"].ToString().Trim() + "' and RNG4ASOC_DEMO_CODE='a'");
            DataView dv1 = new DataView(dtAssoc);
            dv1.RowFilter = "RNG4ASOC_CATG_CODE='K'";
            dt13b1 = dv1.ToTable();
            Assoc_Code = string.Empty;
            if (dt13b1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt13b1.Rows)
                {
                    Assoc_Code = Assoc_Code + dr["RNG4ASOC_AGYTAB_CODES"].ToString();
                    //INCASOC = dr["RNG4ASOC_AGYTAB_CODES"].ToString();
                }
                string[] incode = Assoc_Code.Split(' ');
                for (int j = 0; j < incode.Length; j++)
                {
                    if (!string.IsNullOrEmpty(incode[j].ToString().Trim()))
                        NCSCodes += incode[j].ToString().Trim() + ",";
                }
                if (!string.IsNullOrEmpty(NCSCodes.Trim())) NCSCodes = NCSCodes.Substring(0, NCSCodes.Length - 1);
            }

        }


        DataTable dtAssoc = new DataTable();
        private void fillGvDemo(string Sel_Demo_Code)
        {

            DataSet dsAssoc = DatabaseLayer.SPAdminDB.Get_RNGASS(null, null);
            if (dsAssoc.Tables.Count > 0)
                dtAssoc = dsAssoc.Tables[0];
            dgvDemographics.Rows.Clear();
            //dtCat = dscategories.Tables[0]; 
            int RowCnt = 0, sel_Dem_Index = 0;
            string desc = null, col_code = null, Assoc_Code = null, cat_subCode = null;

            DataRow[] drcat1 = dscategories.Tables[0].Select("RNG4CATG_CODE='" + cat_code + "' AND RNG4CATG_DEM <> '' ");
            //if(cat_code=="I") 
            Label13columnvalues(dtAssoc);
            for (int x = 0; x < drcat1.Length; x++)
            {
                col_code = desc = cat_subCode = null;
                Assoc_Code = "Not Exists";
                desc = drcat1[x]["RNG4CATG_DESC"].ToString().Trim();
                col_code = drcat1[x]["RNG4CATG_CODE"].ToString().Trim();
                cat_subCode = drcat1[x]["RNG4CATG_DEM"].ToString().Trim();


                if (dtAssoc.Rows.Count > 0)
                {
                    if (col_code == "I" && cat_subCode == "b")
                    {
                        DataRow[] drAssoc = dtAssoc.Select("RNG4ASOC_CATG_CODE='" + drcat1[x]["RNG4CATG_CODE"].ToString().Trim() + "' and RNG4ASOC_DEMO_CODE='a'");
                        if (drAssoc.Length > 0)
                            Assoc_Code = drAssoc[0]["RNG4ASOC_AGYTAB_CODES"].ToString();

                        DataTable dt13b = new DataTable();//dtAssoc.Select("RNG4ASOC_CATG_CODE='" + drcat1[x]["RNG4CATG_CODE"].ToString().Trim() + "' and RNG4ASOC_DEMO_CODE='a'");
                        DataView dv = new DataView(dtAssoc);
                        dv.RowFilter = "RNG4ASOC_CATG_CODE='J'";
                        dt13b = dv.ToTable();
                        string INCASOC = string.Empty;
                        if (dt13b.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt13b.Rows)
                                Assoc_Code = Assoc_Code + dr["RNG4ASOC_AGYTAB_CODES"].ToString();
                        }

                    }
                    else if (col_code == "I" && cat_subCode == "e")
                    {
                        DataTable dt13b = new DataTable();//dtAssoc.Select("RNG4ASOC_CATG_CODE='" + drcat1[x]["RNG4CATG_CODE"].ToString().Trim() + "' and RNG4ASOC_DEMO_CODE='a'");
                        DataView dv = new DataView(dtAssoc);
                        dv.RowFilter = "RNG4ASOC_CATG_CODE='J'";
                        dt13b = dv.ToTable();
                        if (dt13b.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt13b.Rows)
                            {
                                Assoc_Code = Assoc_Code + dr["RNG4ASOC_AGYTAB_CODES"].ToString();

                            }
                        }
                    }
                    else if (col_code == "I" && cat_subCode == "h")
                    {
                        DataTable dt13b = new DataTable();//dtAssoc.Select("RNG4ASOC_CATG_CODE='" + drcat1[x]["RNG4CATG_CODE"].ToString().Trim() + "' and RNG4ASOC_DEMO_CODE='a'");
                        DataView dv = new DataView(dtAssoc);
                        dv.RowFilter = "RNG4ASOC_CATG_CODE='K'";
                        dt13b = dv.ToTable();
                        if (dt13b.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt13b.Rows)
                            {
                                Assoc_Code = Assoc_Code + dr["RNG4ASOC_AGYTAB_CODES"].ToString();
                            }
                        }
                    }
                    else
                    {
                        DataRow[] drAssoc = dtAssoc.Select("RNG4ASOC_CATG_CODE='" + drcat1[x]["RNG4CATG_CODE"].ToString().Trim() + "' and RNG4ASOC_DEMO_CODE='" + drcat1[x]["RNG4CATG_DEM"].ToString().Trim() + "'");
                        if (drAssoc.Length > 0)
                            Assoc_Code = drAssoc[0]["RNG4ASOC_AGYTAB_CODES"].ToString();
                    }
                }

                int rowdemo = dgvDemographics.Rows.Add(desc, col_code, cat_subCode, Assoc_Code);
                if (Sel_Demo_Code == cat_subCode)
                    sel_Dem_Index = RowCnt;
                RowCnt++;


            }
            if (RowCnt > 0)
            {
                //if (string.IsNullOrEmpty(Sel_Demo_Code))
                //    dgvDemographics.Rows[0].Tag = 0;
                //else
                //{
                    //sel_Dem_Index = 0;
                    dgvDemographics.CurrentCell = dgvDemographics.Rows[sel_Dem_Index].Cells[0];
                dgvDemographics.Rows[sel_Dem_Index].Selected = true;

                int scrollPosition = 0;
                scrollPosition = dgvDemographics.CurrentCell.RowIndex;
                //int CurrentPage = (scrollPosition / dgvDemographics.ItemsPerPage);
                //CurrentPage++;
                //dgvDemographics.CurrentPage = CurrentPage;
                //dgvDemographics.FirstDisplayedScrollingRowIndex = scrollPosition;
                string CellValue = dgvDemographics.CurrentRow.Cells["Demographics"].Value.ToString();
                dgvDemographics_SelectionChanged(dgvDemographics, EventArgs.Empty);
                //}
            }
        }

        string Value_13g = string.Empty;
        private void dgvDemographics_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDemographics.Rows.Count > 0)
            {
                string Col_Code = dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString();

                DataTable dtselAssoc = new DataTable();
                if (dtAssoc.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtAssoc);
                    dv.RowFilter = "RNG4ASOC_CATG_CODE='" + Col_Code + "'";
                    dtselAssoc = dv.ToTable();
                }

                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                searchAgytabs.Tabs_Type = Cat_Agytype;
                List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                if (dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString() == "I")
                {

                    List<AGYTABSEntity> commonentity = new List<AGYTABSEntity>();
                    if (AgyTabs_List.Count > 0)
                    {
                        commonentity = AgyTabs_List.FindAll(u => u.Code_Desc.ToUpper().Trim().Contains("NONE"));
                        if (commonentity.Count > 0)
                        {
                            foreach (AGYTABSEntity Entity in commonentity)
                            {
                                Value_13g += SetLeadingSpaces(Entity.Table_Code.ToString());
                            }
                        }
                    }

                }


                AGYTABSEntity searchNCB = new AGYTABSEntity(true);
                searchNCB.Tabs_Type = "00037";
                List<AGYTABSEntity> AgyTabs_NCB_List = _model.AdhocData.Browse_AGYTABS(searchNCB);

                //DataSet dsAgency = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Cat_Agytype);
                //DataTable dtAgency = dsAgency.Tables[0];
                //DataSet dsAss = DatabaseLayer.SPAdminDB.Get_CSASS(null, null);
                //DataTable dtAss = dsAss.Tables[0];
                dgvRAgencies.Rows.Clear();

                if (dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString() == "I" && (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "b" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "e" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "h"))
                {
                    dgvRAgencies.Visible = true;
                    if (ToolBarNew != null) ToolBarNew.Enabled = false;
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null) ToolBarDel.Enabled = false;

                    FillDemoGraphControls();
                    Temp_Agy_Codes = null;
                    if (!(dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString()).Equals("Not Exists"))
                        Temp_Agy_Codes = dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString();
                    string descAgency = null;
                    string Code = null;
                    int rowAgency = 0;

                    string[] Temp_Code = null;
                    //if (!string.IsNullOrEmpty(Temp_Agy_Codes))
                    //{
                    //    Temp_Code = SplitByAppication(Temp_Agy_Codes, 5);
                    //}
                    if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "b" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "d")
                    {
                        if (!string.IsNullOrEmpty(EMPCodes))
                        {
                            rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Income for Employment", string.Empty, "N");
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);

                            Temp_Code = EMPCodes.Split(',');
                        }
                        foreach (AGYTABSEntity drAgency in AgyTabs_List)
                        {

                            descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                            Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                            bool Code_Exists = false;
                            bool Can_Continue = true;

                            if (Temp_Code != null)
                            {
                                for (int i = 0; i < Temp_Code.Length; i++)
                                {
                                    if (drAgency.Table_Code.ToString().Trim() == Temp_Code[i].Trim())
                                    {
                                        Code_Exists = true; break;
                                    }
                                }
                            }
                            if (Code_Exists)
                                rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");
                            //else
                            //    rowAgency = dgvRAgencies.Rows.Add(Img_Blank, descAgency, Code, "N");


                        }
                    }

                    if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "b" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "f" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "e")
                    {
                        if (!string.IsNullOrEmpty(IncCodes))
                        {
                            if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() != "e")
                            {
                                rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Other Income Sources", string.Empty, "N");
                                dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                                dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                                dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);
                            }
                            Temp_Code = IncCodes.Split(',');
                        }
                        foreach (AGYTABSEntity drAgency in AgyTabs_List)
                        {

                            descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                            Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                            bool Code_Exists = false;
                            bool Can_Continue = true;

                            if (Temp_Code != null)
                            {
                                for (int i = 0; i < Temp_Code.Length; i++)
                                {
                                    if (drAgency.Table_Code.ToString().Trim() == Temp_Code[i].Trim())
                                    {
                                        Code_Exists = true; break;
                                    }
                                }
                            }
                            if (Code_Exists)
                                rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");
                            //else
                            //    rowAgency = dgvRAgencies.Rows.Add(Img_Blank, descAgency, Code, "N");

                        }
                    }

                    if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "d" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "f" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "h")
                    {
                        if (!string.IsNullOrEmpty(NCSCodes))
                        {
                            if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() != "h")
                            {
                                rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Non-Cash Benefits", string.Empty, "N");
                                dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                                dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                                dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);
                            }
                            Temp_Code = NCSCodes.Split(',');
                        }
                        foreach (AGYTABSEntity drAgency in AgyTabs_NCB_List)
                        {

                            descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                            Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                            bool Code_Exists = false;
                            bool Can_Continue = true;

                            if (Temp_Code != null)
                            {
                                for (int i = 0; i < Temp_Code.Length; i++)
                                {
                                    if (drAgency.Table_Code.ToString().Trim() == Temp_Code[i].Trim())
                                    {
                                        Code_Exists = true; break;
                                    }
                                }
                            }
                            if (Code_Exists)
                                rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");
                            //else
                            //    rowAgency = dgvRAgencies.Rows.Add(Img_Blank, descAgency, Code, "N");

                        }
                    }

                    if (dgvRAgencies.Rows.Count > 0)
                    {
                        int index = 0;
                        dgvRAgencies.CurrentCell = dgvRAgencies.Rows[index].Cells[0];
                        dgvRAgencies.Rows[index].Selected = true;
                    }

                    //lbl13col.Visible = true; lbl15col.Visible = false;
                    //if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "b") lbl13col.Text = "(" + EMPCodes.Trim() + ") and" + " (" + IncCodes.Trim() + ")";
                    //if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "e") lbl13col.Text = "(" + IncCodes.Trim() + ") ";
                    //if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "h") lbl13col.Text = "(" + NCSCodes.Trim() + ") ";

                }
                else if (dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString() == "I" && (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c" ||
                    dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "d" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "f"))
                {
                    dgvRAgencies.Visible = true;
                    if (ToolBarNew != null) ToolBarNew.Enabled = false;
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null) ToolBarDel.Enabled = false;

                    FillDemoGraphControls();
                    Temp_Agy_Codes = null;
                    if (!(dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString()).Equals("Not Exists"))
                        Temp_Agy_Codes = dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString();
                    string descAgency = null;
                    string Code = null;
                    int rowAgency = 0;

                    string[] Temp_Code = null;
                    //if (!string.IsNullOrEmpty(Temp_Agy_Codes))
                    //{
                    //    Temp_Code = SplitByAppication(Temp_Agy_Codes, 5);
                    //}
                    if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "b" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "d")
                    {
                        if (!string.IsNullOrEmpty(EMPCodes))
                        {
                            rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Income for Employment", string.Empty, "N");
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);

                            Temp_Code = EMPCodes.Split(',');
                        }
                        foreach (AGYTABSEntity drAgency in AgyTabs_List)
                        {

                            descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                            Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                            bool Code_Exists = false;
                            bool Can_Continue = true;

                            if (Temp_Code != null)
                            {
                                for (int i = 0; i < Temp_Code.Length; i++)
                                {
                                    if (drAgency.Table_Code.ToString().Trim() == Temp_Code[i].Trim())
                                    {
                                        Code_Exists = true; break;
                                    }
                                }
                            }
                            if (Code_Exists)
                                rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");

                        }
                    }

                    if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "b" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "f")
                    {
                        if (!string.IsNullOrEmpty(IncCodes))
                        {
                            rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Other Income Sources", string.Empty, "N");
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);
                            Temp_Code = IncCodes.Split(',');
                        }
                        foreach (AGYTABSEntity drAgency in AgyTabs_List)
                        {

                            descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                            Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                            bool Code_Exists = false;
                            bool Can_Continue = true;

                            if (Temp_Code != null)
                            {
                                for (int i = 0; i < Temp_Code.Length; i++)
                                {
                                    if (drAgency.Table_Code.ToString().Trim() == Temp_Code[i].Trim())
                                    {
                                        Code_Exists = true; break;
                                    }
                                }
                            }
                            if (Code_Exists)
                                rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");


                        }
                    }

                    if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "d" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c" || dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "f")
                    {
                        if (!string.IsNullOrEmpty(NCSCodes))
                        {
                            rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Non-Cash Benefits", string.Empty, "N");
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                            dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);
                            Temp_Code = NCSCodes.Split(',');
                        }
                        foreach (AGYTABSEntity drAgency in AgyTabs_NCB_List)
                        {

                            descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                            Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                            bool Code_Exists = false;
                            bool Can_Continue = true;

                            if (Temp_Code != null)
                            {
                                for (int i = 0; i < Temp_Code.Length; i++)
                                {
                                    if (drAgency.Table_Code.ToString().Trim() == Temp_Code[i].Trim())
                                    {
                                        Code_Exists = true; break;
                                    }
                                }
                            }
                            if (Code_Exists)
                                rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");
                        }
                    }




                    //lbl13col.Visible = true;//lbl15col.Visible = true;
                    //if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "c") { lbl13col.Text = "(" + EMPCodes.Trim() + ") and" + " (" + IncCodes.Trim() + ")"; lbl15col.Visible = true; lbl15col.Text = "and" + " (" + NCSCodes.Trim() + ")"; }
                    //if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "d") { lbl13col.Text = "(" + EMPCodes.Trim() + ") and" + " (" + NCSCodes.Trim() + ")"; lbl15col.Visible = false; }
                    //if (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "f") { lbl13col.Text = "(" + IncCodes.Trim() + ")"; lbl15col.Visible = true; lbl15col.Text = "and" + " (" + NCSCodes.Trim() + ")"; }
                }
                else if (dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString() == "I" && (dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString() == "g"))
                {
                    dgvRAgencies.Visible = true;
                    if (ToolBarNew != null) ToolBarNew.Enabled = false;
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null) ToolBarDel.Enabled = false;

                    FillDemoGraphControls();
                    Temp_Agy_Codes = null;
                    if (!(dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString()).Equals("Not Exists"))
                        Temp_Agy_Codes = dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString();
                    string descAgency = null;
                    string Code = null;
                    int rowAgency = 0;

                    string[] Temp_Code = null;

                    //if (!string.IsNullOrEmpty(IncCodes))
                    //{
                    rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Other Income Sources", string.Empty, "N");
                    dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                    dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                    dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);
                    //Temp_Code = IncCodes.Split(',');
                    //}
                    foreach (AGYTABSEntity drAgency in AgyTabs_List)
                    {

                        descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                        Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                        //bool Code_Exists = false;
                        //bool Can_Continue = true;

                        if (descAgency.ToUpper().Trim().Contains("NONE"))
                            rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");

                    }
                    if (!string.IsNullOrEmpty(NCSCodes))
                    {
                        rowAgency = dgvRAgencies.Rows.Add(Img_Blank, "Non-Cash Benefits", string.Empty, "N");
                        dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                        dgvRAgencies.Rows[rowAgency].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                        dgvRAgencies.Rows[rowAgency].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F);
                        Temp_Code = NCSCodes.Split(',');
                    }
                    foreach (AGYTABSEntity drAgency in AgyTabs_NCB_List)
                    {

                        descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                        Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                        //bool Code_Exists = false;
                        //bool Can_Continue = true;

                        if (descAgency.ToUpper().Trim().Contains("NONE"))
                            rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");

                    }

                }
                else
                {
                    if (Privileges.AddPriv.Equals("true") && ToolBarNew != null) ToolBarNew.Enabled = true; else if (ToolBarNew != null) ToolBarNew.Enabled = false;
                    if (Privileges.ChangePriv.Equals("true") && ToolBarEdit != null) ToolBarEdit.Enabled = true; else if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                    if (Privileges.DelPriv.Equals("true") && ToolBarDel != null) ToolBarDel.Enabled = true; else if (ToolBarDel != null) ToolBarDel.Enabled = false;

                    dgvRAgencies.Visible = true; //lbl13col.Visible = false; lbl15col.Visible = false;

                    FillDemoGraphControls();
                    Temp_Agy_Codes = null;
                    if (!(dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString()).Equals("Not Exists"))
                        Temp_Agy_Codes = dgvDemographics.CurrentRow.Cells["Ass_Code"].Value.ToString();
                    string descAgency = null;
                    string Code = null;
                    int rowAgency = 0;

                    string[] Temp_Code = null;

                    if (!string.IsNullOrEmpty(Temp_Agy_Codes))
                    {
                        Temp_Code = SplitByAppication(Temp_Agy_Codes, 5);
                    }

                    foreach (AGYTABSEntity drAgency in AgyTabs_List)
                    {

                        descAgency = drAgency.Code_Desc.Trim(); //drAgency["LookUpDesc"].ToString().Trim();
                        Code = drAgency.Table_Code.Trim(); //drAgency["Code"].ToString().Trim();

                        bool Code_Exists = false;
                        bool Can_Continue = true;

                        if (Temp_Code != null)
                        {
                            for (int i = 0; i < Temp_Code.Length; i++)
                            {
                                if (drAgency.Table_Code.ToString().Trim() == Temp_Code[i].Trim())
                                {
                                    Code_Exists = true; break;
                                }
                            }
                        }
                        if (Code_Exists)
                            rowAgency = dgvRAgencies.Rows.Add(Img_Tick, descAgency, Code, "Y");
                        else
                            rowAgency = dgvRAgencies.Rows.Add(Img_Blank, descAgency, Code, "N");

                        if (dtselAssoc.Rows.Count > 0)
                        {
                            foreach (DataRow drassoc in dtselAssoc.Rows)
                            {
                                string SelAssocCode = string.Empty; string[] Temp = null; bool IsSelect = false;
                                if (!string.IsNullOrEmpty(drassoc["RNG4ASOC_AGYTAB_CODES"].ToString().Trim()))
                                    SelAssocCode = drassoc["RNG4ASOC_AGYTAB_CODES"].ToString();

                                if (!string.IsNullOrEmpty(SelAssocCode))
                                {
                                    Temp = SplitByAppication(SelAssocCode, 5);
                                }
                                if (Temp != null)
                                {
                                    for (int i = 0; i < Temp.Length; i++)
                                    {
                                        if (Code.Trim() == Temp[i].Trim())
                                        {
                                            IsSelect = true; break;
                                        }
                                    }
                                }

                                if (IsSelect)
                                {
                                    dgvRAgencies.Rows[rowAgency].DefaultCellStyle.ForeColor = Color.Green;
                                    break;
                                }
                            }
                        }
                        rowAgency++;
                    }

                    if (dgvRAgencies.Rows.Count > 0)
                    {
                        int index = 0;
                        dgvRAgencies.CurrentCell = dgvRAgencies.Rows[index].Cells[0];
                        dgvRAgencies.Rows[index].Selected = true;
                    }
                }
            }
        }

        private void FillDemoGraphControls()
        {
            string code = dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString();
            string sub_code = dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString();

            List<CSB4AsocEntity> CSb4Entity;
            CSb4Entity = _model.SPAdminData.Browse_CSB4Assoc(code, sub_code);
            if (CSb4Entity.Count > 0)
            {
                CSB4AsocEntity drAssoc = CSb4Entity[0];
                if (!string.IsNullOrEmpty(drAssoc.AgeFrm.ToString()))
                    txtfrom.Text = drAssoc.AgeFrm.ToString();
                else
                    txtfrom.Text = string.Empty;
                if (!string.IsNullOrEmpty(drAssoc.AgeTo.ToString()))
                    txtTo.Text = drAssoc.AgeTo.ToString();
                else
                    txtTo.Text = string.Empty;
            }
            else
            {
                txtfrom.Text = string.Empty;
                txtTo.Text = string.Empty;
            }
        }
        private string[] SplitByAppication(string s, int split)
        {
            //Like using List because I can just add to it 
            List<string> list = new List<string>();
            // Integer Division
            int TimesThroughTheLoop = s.Length / split;
            for (int i = 0; i < TimesThroughTheLoop; i++)
            {
                list.Add(s.Substring(i * split, split));
            }
            // Pickup the end of the string
            if (TimesThroughTheLoop * split != s.Length)
            {
                list.Add(s.Substring(TimesThroughTheLoop * split));
            }
            return list.ToArray();
        }



        string Added_Edited_Date = string.Empty;
        private void FillRDateRange_Grid()
        {
            //DataSet dsCsb16 = DatabaseLayer.SPAdminDB.Get_CSB16();
            //DataTable dtCsb16;
            //dtCsb16 = dsCsb16.Tables[0];
            this.gvRdateRange.SelectionChanged -= new System.EventHandler(this.gvRdateRange_SelectionChanged);
            gvRdateRange.Rows.Clear(); int row_Index = 0;
            this.gvRdateRange.SelectionChanged += new System.EventHandler(this.gvRdateRange_SelectionChanged);
            int sel_Ind = 0; int Row_Cnt = 0;


            int rowIndex = 0; int rowCnt = 0; int Sel_Group_Index = 0;
            //RefTdate = Refdate.Split(',');

            List<RCsb14GroupEntity> CODEList = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null,BaseForm.UserID,BaseForm.BaseAdminAgency);
            if (CODEList.Count > 0)
            {
                CODEList = CODEList.OrderBy(u => u.Agency).ThenBy(u => u.Code).ToList();
                foreach (RCsb14GroupEntity GrpEnt in CODEList)
                {
                    if (string.IsNullOrWhiteSpace(GrpEnt.GrpCode.Trim()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.Trim()))
                    {
                        string Code = GrpEnt.Code.ToString();
                        string desc = GrpEnt.GrpDesc.ToString();
                        string Hiename = string.Empty;
                        if (GrpEnt.Agency.Trim() == "**") Hiename = "All Agencies";
                        DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(GrpEnt.Agency, string.Empty, string.Empty);
                        if (ds_Hie_Name.Tables.Count > 0)
                        {
                            if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                            {
                                if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                    Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                ////hierchyEntity = new HierarchyEntity(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Hiename);

                                //if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                //    ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                //if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                //    ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";

                                ////hierchyEntity = new HierarchyEntity(ds_Hie_Name.Tables[0].Rows[0], "CASEHIE");
                            }
                        }

                        rowIndex = gvRdateRange.Rows.Add(Code, GrpEnt.Agency.Trim() + "-" + Hiename.Trim(), desc, LookupDataAccess.Getdate(GrpEnt.OFdate.ToString()), LookupDataAccess.Getdate(GrpEnt.OTdate.ToString()), GrpEnt.Agency.Trim());

                        string toolTipText = "Added By     : " + GrpEnt.AddOperator.ToString().Trim() + " on " + GrpEnt.DateAdd.ToString() + "\n" +
                                    "Modified By  : " + GrpEnt.LSTCOperator.ToString().Trim() + " on " + GrpEnt.DateLSTC.ToString();

                        foreach (DataGridViewCell cell in gvRdateRange.Rows[rowIndex].Cells)
                        {
                            cell.ToolTipText = toolTipText;
                        }
                        //if (Sel_Grp_Cd.Trim() == Code)
                        //    Sel_Group_Index = rowCnt;
                        rowCnt++;
                        //gvGroup.Rows[rowIndex].Tag = GroupList;

                    }
                }
            }

            if (rowCnt > 0)
            {
                //if (string.IsNullOrEmpty(Added_Edited_Date))
                //    gvRdateRange.Rows[0].Tag = 0;
                //else
                //{
                this.gvRdateRange.SelectionChanged -= new System.EventHandler(this.gvRdateRange_SelectionChanged);
                gvRdateRange.CurrentCell = gvRdateRange.Rows[sel_Ind].Cells[1];
                gvRdateRange.Rows[sel_Ind].Selected = true;

                int scrollPosition = 0;
                scrollPosition = gvRdateRange.CurrentCell.RowIndex;
                //int CurrentPage = (scrollPosition / gvRdateRange.ItemsPerPage);
                //CurrentPage++;
                //gvRdateRange.CurrentPage = CurrentPage;
                //gvRdateRange.FirstDisplayedScrollingRowIndex = scrollPosition;
                this.gvRdateRange.SelectionChanged += new System.EventHandler(this.gvRdateRange_SelectionChanged);
                gvRdateRange_SelectionChanged(gvRdateRange, EventArgs.Empty);
                //}

                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = true;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = true;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = true;

            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = false;
            }

            if (gvRdateRange.Rows.Count > 0)
            {
                if (Privileges.AddPriv.Equals("false"))
                {
                    gvRdateRange.Columns["RAddGroup"].Visible = false;
                }
                else
                {
                    gvRdateRange.Columns["RAddGroup"].Visible = true;
                }
            }

        }
        private void FillSRDateRange_Grid()
        {
            //DataSet dsCsb16 = DatabaseLayer.SPAdminDB.Get_CSB16();
            //DataTable dtCsb16;
            //dtCsb16 = dsCsb16.Tables[0];
            this.gvSRdateRange.SelectionChanged -= new System.EventHandler(this.gvSRdateRange_SelectionChanged);
            gvSRdateRange.Rows.Clear(); int row_Index = 0;
            this.gvSRdateRange.SelectionChanged += new System.EventHandler(this.gvSRdateRange_SelectionChanged);
            int sel_Ind = 0; int Row_Cnt = 0;


            int rowIndex = 0; int rowCnt = 0; int Sel_Group_Index = 0;
            //RefTdate = Refdate.Split(',');

            List<SRCsb14GroupEntity> CODEList = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null);
            if (CODEList.Count > 0)
            {
                CODEList = CODEList.OrderBy(u => u.Agency).ThenBy(u => u.Code).ToList();
                foreach (SRCsb14GroupEntity GrpEnt in CODEList)
                {
                    if (string.IsNullOrWhiteSpace(GrpEnt.GrpCode.Trim()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.Trim()))
                    {
                        string Code = GrpEnt.Code.ToString();
                        string desc = GrpEnt.GrpDesc.ToString();
                        string Hiename = string.Empty;
                        if (GrpEnt.Agency.Trim() == "**") Hiename = "All Agencies";
                        DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(GrpEnt.Agency, string.Empty, string.Empty);
                        if (ds_Hie_Name.Tables.Count > 0)
                        {
                            if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                            {
                                if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                    Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                ////hierchyEntity = new HierarchyEntity(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Hiename);

                                //if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                //    ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                //if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                //    ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";

                                ////hierchyEntity = new HierarchyEntity(ds_Hie_Name.Tables[0].Rows[0], "CASEHIE");
                            }
                        }
                        rowIndex = gvSRdateRange.Rows.Add(Code, GrpEnt.Agency.Trim() + "-" + Hiename.Trim(), desc, LookupDataAccess.Getdate(GrpEnt.OFdate.ToString()), LookupDataAccess.Getdate(GrpEnt.OTdate.ToString()), GrpEnt.Agency.Trim());

                        string toolTipText = "Added By     : " + GrpEnt.AddOperator.ToString().Trim() + " on " + GrpEnt.DateAdd.ToString() + "\n" +
                                    "Modified By  : " + GrpEnt.LSTCOperator.ToString().Trim() + " on " + GrpEnt.DateLSTC.ToString();

                        foreach (DataGridViewCell cell in gvSRdateRange.Rows[rowIndex].Cells)
                        {
                            cell.ToolTipText = toolTipText;
                        }
                        //if (Sel_Grp_Cd.Trim() == Code)
                        //    Sel_Group_Index = rowCnt;
                        rowCnt++;
                        //gvGroup.Rows[rowIndex].Tag = GroupList;

                    }
                }
            }

            if (rowCnt > 0)
            {
                //if (string.IsNullOrEmpty(Added_Edited_Date))
                //    gvSRdateRange.Rows[0].Tag = 0;
                //else
                //{
                this.gvSRdateRange.SelectionChanged -= new System.EventHandler(this.gvSRdateRange_SelectionChanged);
                gvSRdateRange.CurrentCell = gvSRdateRange.Rows[sel_Ind].Cells[1];
                gvSRdateRange.Rows[sel_Ind].Selected = true;

                int scrollPosition = 0;
                scrollPosition = gvSRdateRange.CurrentCell.RowIndex;
                //int CurrentPage = (scrollPosition / gvSRdateRange.ItemsPerPage);
                //CurrentPage++;
                //gvSRdateRange.CurrentPage = CurrentPage;
                //gvSRdateRange.FirstDisplayedScrollingRowIndex = scrollPosition;
                this.gvSRdateRange.SelectionChanged += new System.EventHandler(this.gvSRdateRange_SelectionChanged);
                gvSRdateRange_SelectionChanged(gvSRdateRange, EventArgs.Empty);
                //}

                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = true;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = true;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = true;

            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = false;
            }

            if (gvSRdateRange.Rows.Count > 0)
            {
                if (Privileges.AddPriv.Equals("false"))
                {
                    gvSRdateRange.Columns["SRAddGroup"].Visible = false;
                }
                else
                {
                    gvSRdateRange.Columns["SRAddGroup"].Visible = true;
                }
            }

        }

        List<RCsb14GroupEntity> RGroupList;
        private void FillRPerformanceGroups(string Sel_Grp_Cd)
        {
            gvRGroup.Rows.Clear();
            int rowIndex = 0; int rowCnt = 0; int Sel_Group_Index = 0;
            //RefTdate = Refdate.Split(',');

            RGroupList = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
            if (RGroupList.Count > 0)
            {
                foreach (RCsb14GroupEntity GrpEnt in RGroupList)
                {
                    if (GrpEnt.Code.ToString() == gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim() && GrpEnt.Agency.ToString() == gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim() && !string.IsNullOrWhiteSpace(GrpEnt.GrpCode.ToString()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.ToString()))
                    {
                        string Code = GrpEnt.GrpCode.ToString();
                        string desc = GrpEnt.GrpDesc.ToString();
                        rowIndex = gvRGroup.Rows.Add(Code, desc);

                        string toolTipText = "Added By     : " + GrpEnt.AddOperator.ToString().Trim() + " on " + GrpEnt.DateAdd.ToString() + "\n" +
                                    "Modified By  : " + GrpEnt.LSTCOperator.ToString().Trim() + " on " + GrpEnt.DateLSTC.ToString();

                        foreach (DataGridViewCell cell in gvRGroup.Rows[rowIndex].Cells)
                        {
                            cell.ToolTipText = toolTipText;
                        }
                        if (Sel_Grp_Cd.Trim() == Code)
                            Sel_Group_Index = rowCnt;
                        rowCnt++;
                        ////gvGroup.Rows[rowIndex].Tag = GroupList;
                        //gvRGroup.ItemsPerPage = 20;
                    }
                }

                if (rowCnt > 0)
                {
                    //if (string.IsNullOrEmpty(Sel_Grp_Cd))
                    //{
                    //    //Sel_Group_Index = 0;
                    //    //gvGroup.CurrentCell = gvGroup.Rows[Sel_Group_Index].Cells[1];
                    //    gvGroup.Rows[0].Tag = 0;
                    //}
                    //else
                    //{

                    this.gvRGroup.SelectionChanged -= new System.EventHandler(this.gvRGroup_SelectionChanged);
                    gvRGroup.CurrentCell = gvRGroup.Rows[Sel_Group_Index].Cells[0];
                    gvRGroup.Rows[Sel_Group_Index].Selected = true;

                    int scrollPosition = 0;
                    scrollPosition = gvRGroup.CurrentCell.RowIndex;
                    //int CurrentPage = (scrollPosition / gvRGroup.ItemsPerPage);
                    //CurrentPage++;
                    //gvRGroup.CurrentPage = CurrentPage;
                    //gvRGroup.FirstDisplayedScrollingRowIndex = scrollPosition;
                    this.gvRGroup.SelectionChanged += new System.EventHandler(this.gvRGroup_SelectionChanged);
                    gvRGroup_SelectionChanged(gvRGroup, EventArgs.Empty);

                    //}
                    //btnCpyFrm.Visible = true;
                    //if (ToolBarEdit != null)
                    //    ToolBarEdit.Enabled = true;
                    //if (ToolBarDel != null)
                    //    ToolBarDel.Enabled = true;
                    //if (ToolBarPrint != null)
                    //    ToolBarPrint.Enabled = true;

                }
                else
                {
                    //btnCpyFrm.Visible = false;
                    //if (ToolBarEdit != null)
                    //    ToolBarEdit.Enabled = false;
                    //if (ToolBarDel != null)
                    //    ToolBarDel.Enabled = false;
                    //if (ToolBarPrint != null)
                    //    ToolBarPrint.Enabled = false;
                }
            }

            if (Privileges.AddPriv.Equals("false"))
            {
                gvRGroup.Columns["RAddGroupImg"].Visible = false;

            }
            else
            {
                gvRGroup.Columns["RAddGroupImg"].Visible = true;
            }
            if (Privileges.ChangePriv.Equals("false"))
            {
                gvRGroup.Columns["REdit_Group"].Visible = false;

            }
            else
            {
                gvRGroup.Columns["REdit_Group"].Visible = true;
            }
            if (Privileges.DelPriv.Equals("false"))
                gvRGroup.Columns["RDel_Group"].Visible = false;
            else
                gvRGroup.Columns["RDel_Group"].Visible = true;

        }
        List<SRCsb14GroupEntity> SRGroupList;
        private void FillSRPerformanceGroups(string Sel_Grp_Cd)
        {
            gvSRGroup.Rows.Clear();
            int rowIndex = 0; int rowCnt = 0; int Sel_Group_Index = 0;
            //RefTdate = Refdate.Split(',');

            SRGroupList = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null);
            if (SRGroupList.Count > 0)
            {
                foreach (SRCsb14GroupEntity GrpEnt in SRGroupList)
                {
                    if (GrpEnt.Code.ToString() == gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim() && GrpEnt.Agency.ToString() == gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim() && !string.IsNullOrWhiteSpace(GrpEnt.GrpCode.ToString()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.ToString()))
                    {
                        string Code = GrpEnt.GrpCode.ToString();
                        string desc = GrpEnt.GrpDesc.ToString();
                        rowIndex = gvSRGroup.Rows.Add(Code, desc);

                        string toolTipText = "Added By     : " + GrpEnt.AddOperator.ToString().Trim() + " on " + GrpEnt.DateAdd.ToString() + "\n" +
                                    "Modified By  : " + GrpEnt.LSTCOperator.ToString().Trim() + " on " + GrpEnt.DateLSTC.ToString();

                        foreach (DataGridViewCell cell in gvSRGroup.Rows[rowIndex].Cells)
                        {
                            cell.ToolTipText = toolTipText;
                        }
                        if (Sel_Grp_Cd.Trim() == Code)
                            Sel_Group_Index = rowCnt;
                        rowCnt++;
                        //gvGroup.Rows[rowIndex].Tag = GroupList;
                        //gvSRGroup.ItemsPerPage = 20;
                    }
                }

                if (rowCnt > 0)
                {
                    //if (string.IsNullOrEmpty(Sel_Grp_Cd))
                    //{
                    //    //Sel_Group_Index = 0;
                    //    //gvGroup.CurrentCell = gvGroup.Rows[Sel_Group_Index].Cells[1];
                    //    gvGroup.Rows[0].Tag = 0;
                    //}
                    //else
                    //{

                    this.gvSRGroup.SelectionChanged -= new System.EventHandler(this.gvSRGroup_SelectionChanged);
                    gvSRGroup.CurrentCell = gvSRGroup.Rows[Sel_Group_Index].Cells[0];
                    gvSRGroup.Rows[Sel_Group_Index].Selected = true;

                    int scrollPosition = 0;
                    scrollPosition = gvSRGroup.CurrentCell.RowIndex;
                    //int CurrentPage = (scrollPosition / gvSRGroup.ItemsPerPage);
                    //CurrentPage++;
                    //gvSRGroup.CurrentPage = CurrentPage;
                    //gvSRGroup.FirstDisplayedScrollingRowIndex = scrollPosition;
                    this.gvSRGroup.SelectionChanged += new System.EventHandler(this.gvSRGroup_SelectionChanged);
                    gvSRGroup_SelectionChanged(gvSRGroup, EventArgs.Empty);

                    //}
                    //btnCpyFrm.Visible = true;
                    //if (ToolBarEdit != null)
                    //    ToolBarEdit.Enabled = true;
                    //if (ToolBarDel != null)
                    //    ToolBarDel.Enabled = true;
                    //if (ToolBarPrint != null)
                    //    ToolBarPrint.Enabled = true;

                }
                else
                {
                    //btnCpyFrm.Visible = false;
                    //if (ToolBarEdit != null)
                    //    ToolBarEdit.Enabled = false;
                    //if (ToolBarDel != null)
                    //    ToolBarDel.Enabled = false;
                    //if (ToolBarPrint != null)
                    //    ToolBarPrint.Enabled = false;
                }
            }

            if (Privileges.AddPriv.Equals("false"))
            {
                gvSRGroup.Columns["SRAddGroupImg"].Visible = false;

            }
            else
            {
                gvSRGroup.Columns["SRAddGroupImg"].Visible = true;
            }
            if (Privileges.ChangePriv.Equals("false"))
            {
                gvSRGroup.Columns["SREdit_Group"].Visible = false;

            }
            else
            {
                gvSRGroup.Columns["SREdit_Group"].Visible = true;
            }
            if (Privileges.DelPriv.Equals("false"))
                gvSRGroup.Columns["SRDel_Group"].Visible = false;
            else
                gvSRGroup.Columns["SRDel_Group"].Visible = true;

        }

        private void fillRPerformTable(string Sel_Tbl_Cd)
        {
            gvRTable.Rows.Clear();
            int rowIndex = 0; int rowCnt = 0; int Sel_Table_Index = 0;

            //RefTdate = Refdate.Split(',');

            List<RCsb14GroupEntity> tbllist;
            tbllist = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);

            if (tbllist.Count > 0)
            {
                string Grp_Cd = gvRGroup.CurrentRow.Cells["RGroup_Code"].Value.ToString();
                List<RCsb14GroupEntity> Seltbllist = tbllist.FindAll(u => u.GrpCode.Trim().Equals(Grp_Cd));
                Seltbllist = Seltbllist.OrderBy(u => u.TblCode.Trim()).ToList();
                if (Seltbllist.Count > 0)
                {
                    foreach (RCsb14GroupEntity tblEnt in Seltbllist)
                    {
                        if ((tblEnt.Code.ToString() == gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim()) && (tblEnt.Agency.ToString() == gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim()) && tblEnt.GrpCode.ToString() == Grp_Cd && !string.IsNullOrWhiteSpace(tblEnt.TblCode.ToString()))
                        {

                            string TblCd = tblEnt.TblCode.ToString();
                            string TblDesc = tblEnt.GrpDesc.ToString();
                            rowIndex = gvRTable.Rows.Add(TblCd, Grp_Cd, TblDesc);
                            string toolTipText = "Added By     : " + tblEnt.AddOperator.ToString().Trim() + " on " + tblEnt.DateAdd.ToString() + "\n" +
                                     "Modified By  : " + tblEnt.LSTCOperator.ToString().Trim() + " on " + tblEnt.DateLSTC.ToString();

                            foreach (DataGridViewCell cell in gvRTable.Rows[rowIndex].Cells)
                            {
                                cell.ToolTipText = toolTipText;
                            }

                            if (Sel_Tbl_Cd.Trim() == TblCd)
                                Sel_Table_Index = rowCnt;
                            rowCnt++;
                            gvRTable.Rows[rowIndex].Tag = tbllist;
                        }
                    }

                    if (rowCnt > 0)
                    {
                        //if (string.IsNullOrEmpty(Sel_Tbl_Cd))
                        //    gvRTable.Rows[0].Tag = 0;
                        //else
                        //{
                            gvRTable.CurrentCell = gvRTable.Rows[Sel_Table_Index].Cells[0];
                            gvRTable.Rows[Sel_Table_Index].Selected = true;

                            int scrollPosition = 0;
                            scrollPosition = gvRTable.CurrentCell.RowIndex;
                            //int CurrentPage = (scrollPosition / gvRTable.ItemsPerPage);
                            //CurrentPage++;
                            //gvRTable.CurrentPage = CurrentPage;
                            //gvRTable.FirstDisplayedScrollingRowIndex = scrollPosition;
                       // }
                    }
                }


            }

            if (Privileges.ChangePriv.Equals("false"))
                gvRTable.Columns["REditTable"].Visible = false;
            else
                gvRTable.Columns["REditTable"].Visible = true;
            if (Privileges.DelPriv.Equals("false"))
                gvRTable.Columns["RDelTable"].Visible = false;
            else
                gvRTable.Columns["RDelTable"].Visible = true;

        }
        private void fillSRPerformTable(string Sel_Tbl_Cd)
        {
            gvSRTable.Rows.Clear();
            int rowIndex = 0; int rowCnt = 0; int Sel_Table_Index = 0;

            //RefTdate = Refdate.Split(',');

            List<SRCsb14GroupEntity> Stbllist;
            Stbllist = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null);

            if (Stbllist.Count > 0)
            {
                string Grp_Cd = gvSRGroup.CurrentRow.Cells["SRGroup_Code"].Value.ToString();
                List<SRCsb14GroupEntity> Seltbllist = Stbllist.FindAll(u => u.GrpCode.Trim().Equals(Grp_Cd));
                Seltbllist = Seltbllist.OrderBy(u => u.TblCode.Trim()).ToList();
                if (Seltbllist.Count > 0)
                {
                    foreach (SRCsb14GroupEntity tblEnt in Seltbllist)
                    {
                        if ((tblEnt.Code.ToString() == gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim()) && (tblEnt.Agency.ToString() == gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim()) && tblEnt.GrpCode.ToString() == Grp_Cd && !string.IsNullOrWhiteSpace(tblEnt.TblCode.ToString()))
                        {

                            string TblCd = tblEnt.TblCode.ToString();
                            string TblDesc = tblEnt.GrpDesc.ToString();
                            rowIndex = gvSRTable.Rows.Add(TblCd, Grp_Cd, TblDesc);
                            string toolTipText = "Added By     : " + tblEnt.AddOperator.ToString().Trim() + " on " + tblEnt.DateAdd.ToString() + "\n" +
                                     "Modified By  : " + tblEnt.LSTCOperator.ToString().Trim() + " on " + tblEnt.DateLSTC.ToString();

                            foreach (DataGridViewCell cell in gvSRTable.Rows[rowIndex].Cells)
                            {
                                cell.ToolTipText = toolTipText;
                            }

                            if (Sel_Tbl_Cd.Trim() == TblCd)
                                Sel_Table_Index = rowCnt;
                            rowCnt++;
                            gvSRTable.Rows[rowIndex].Tag = Stbllist;
                        }
                    }

                    if (rowCnt > 0)
                    {
                    //    if (string.IsNullOrEmpty(Sel_Tbl_Cd))
                    //    gvSRTable.Rows[0].Tag = 0;
                    //else
                   //{
                        gvSRTable.CurrentCell = gvSRTable.Rows[Sel_Table_Index].Cells[0];
                        gvSRTable.Rows[Sel_Table_Index].Selected = true;
                        int scrollPosition = 0;
                        scrollPosition = gvSRTable.CurrentCell.RowIndex;
                       //int CurrentPage = (scrollPosition / gvSRTable.ItemsPerPage);
                       //CurrentPage++;
                       //gvSRTable.CurrentPage = CurrentPage;
                       //gvSRTable.FirstDisplayedScrollingRowIndex = scrollPosition;
                   //}
                    }
                }


            }

            if (Privileges.ChangePriv.Equals("false"))
                gvSRTable.Columns["SREditTable"].Visible = false;
            else
                gvSRTable.Columns["SREditTable"].Visible = true;
            if (Privileges.DelPriv.Equals("false"))
                gvSRTable.Columns["SRDelTable"].Visible = false;
            else
                gvSRTable.Columns["SRDelTable"].Visible = true;

        }


        public void DeleteRGroupRow(DialogResult dialogresult)
        {
            //Gizmox.WebGUI.Forms.Form senderform = (Gizmox.WebGUI.Forms.Form)sender;
            string Strmsg = string.Empty;
            //if (senderform != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                string strmsg = string.Empty;
                string grp_cd = gvRGroup.CurrentRow.Cells["RGroup_Code"].Value.ToString();
                //string Tbl_cd=gvGroup.CurrentRow.Cells["Tbl_code"].Value.ToString();
                if (Captain.DatabaseLayer.SPAdminDB.DeleteRNGGRP(Fdate, Tdate, grp_cd, "Null", "Group", out strmsg))
                {
                    AlertBox.Show("Outcome Indicators Domain with code: " + gvRGroup.CurrentRow.Cells["RGroup_Code"].Value.ToString() + "\nDeleted Successfully", MessageBoxIcon.Error, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Performance Group For " + gvGroup.CurrentRow.Cells["Group_Code"].Value.ToString() + " " + "Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    //if (strIndex != 0)
                    //    strIndex = strIndex - 1;
                    FillRPerformanceGroups(Added_Edited_GroupCode);//RefreshGroupGrid();
                    if (gvRGroup.Rows.Count > 0)
                        fillRPerformTable(Added_Edited_TableCode);
                }
                else
                    if (strmsg == "Already Exist")
                {
                    
                    MessageBox.Show("You must delete all associated Groups before deleting the Domain.", "CAPTAIN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Failed to Delete", "CAPTAIN", MessageBoxButtons.OK);

            }
            //}
        }
        public void DeleteSRGroupRow(DialogResult dialogresult)
        {
            //Gizmox.WebGUI.Forms.Form senderform = (Gizmox.WebGUI.Forms.Form)sender;
            string Strmsg = string.Empty;
            //if (senderform != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                string strmsg = string.Empty;
                string grp_cd = gvSRGroup.CurrentRow.Cells["SRGroup_Code"].Value.ToString();
                //string Tbl_cd=gvGroup.CurrentRow.Cells["Tbl_code"].Value.ToString();
                if (Captain.DatabaseLayer.SPAdminDB.DeleteRNGSRGRP(Fdate, Tdate, grp_cd, "Null", "Group", out strmsg))
                {
                    AlertBox.Show("Service Group with code: " + gvSRGroup.CurrentRow.Cells["SRGroup_Code"].Value.ToString() + "\nDeleted Successfully", MessageBoxIcon.Error, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Performance Group For " + gvGroup.CurrentRow.Cells["Group_Code"].Value.ToString() + " " + "Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    //if (strIndex != 0)
                    //    strIndex = strIndex - 1;
                    FillSRPerformanceGroups(Added_Edited_GroupCode);//RefreshGroupGrid();
                    if (gvSRGroup.Rows.Count > 0)
                        fillSRPerformTable(Added_Edited_TableCode);
                }
                else
                    if (strmsg == "Already Exist")
                    MessageBox.Show("You must delete all associated Tables before deleting the Group.", "CAPTAIN", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Failed to Delete", "CAPTAIN", MessageBoxButtons.OK);

            }
            //}
        }

        public void RDeleteTableRow(DialogResult dialogresult)
        {
            //Gizmox.WebGUI.Forms.Form senderform = (Gizmox.WebGUI.Forms.Form)sender;
            string Strmsg = string.Empty;
            //if (senderform != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                string G_Cd = gvRTable.CurrentRow.Cells["RGrp_cd"].Value.ToString();
                string T_cd = gvRTable.CurrentRow.Cells["RTable_Code"].Value.ToString();
                if (Captain.DatabaseLayer.SPAdminDB.DeleteRNGGRP(Fdate, Tdate, G_Cd, T_cd, "Table", out Strmsg))
                {
                    AlertBox.Show("Group with code: " + gvRTable.CurrentRow.Cells["RTable_Code"].Value.ToString() + "\nDeleted Successfully", MessageBoxIcon.Error, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Table Row for " + gvTable.CurrentRow.Cells["Table_Code"].Value.ToString() + "  Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    if (strIndex != 0)
                        strIndex = strIndex - 1;
                    RefreshRTableGrid();
                }
                else
                    MessageBox.Show("Failed to Delete", "CAPTAIN", MessageBoxButtons.OK);

            }
            //}
        }
        public void SRDeleteTableRow(DialogResult dialogresult)
        {
            //Gizmox.WebGUI.Forms.Form senderform = (Gizmox.WebGUI.Forms.Form)sender;
            string Strmsg = string.Empty;
            //if (senderform != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                string G_Cd = gvSRTable.CurrentRow.Cells["SRGrp_cd"].Value.ToString();
                string T_cd = gvSRTable.CurrentRow.Cells["SRTable_Code"].Value.ToString();
                if (Captain.DatabaseLayer.SPAdminDB.DeleteRNGSRGRP(Fdate, Tdate, G_Cd, T_cd, "Table", out Strmsg))
                {
                    AlertBox.Show("Table with code: " + gvSRTable.CurrentRow.Cells["SRTable_Code"].Value.ToString() + "\nDeleted Successfully", MessageBoxIcon.Error, null, ContentAlignment.BottomRight);
                    //MessageBox.Show("Table Row for " + gvTable.CurrentRow.Cells["Table_Code"].Value.ToString() + "  Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    if (strIndex != 0)
                        strIndex = strIndex - 1;
                    RefreshSRTableGrid();
                }
                else
                    MessageBox.Show("Failed to Delete", "CAPTAIN", MessageBoxButtons.OK);

            }
            //}
        }
        public void RefreshSRTableGrid()
        {
            fillSRPerformTable(Added_Edited_TableCode);
            if (gvSRTable.Rows.Count != 0)
            {
                gvSRTable.CurrentCell = gvSRTable.Rows[strIndex].Cells[0];
                gvSRTable.Rows[strIndex].Selected = true;
                //gvTable.Rows[strIndex].Selected = true;
                //gvSRTable.CurrentPage = strPageIndex;
            }
        }
        public void RefreshRTableGrid()
        {
            fillRPerformTable(Added_Edited_TableCode);
            if (gvRTable.Rows.Count != 0)
            {
                gvRTable.CurrentCell = gvRTable.Rows[strIndex].Cells[0];
                gvRTable.Rows[strIndex].Selected = true;
                //gvRTable.CurrentPage = strPageIndex;
            }
        }

        private void gvDateRange_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (Privileges.AddPriv.Equals("true"))
            //{
            //    if (e.ColumnIndex == 2 && e.RowIndex != -1)
            //    {
            //        Added_Edited_GroupCode = string.Empty;
            //        RPerformanceMeasureForm PmForm_Add = new RPerformanceMeasureForm(BaseForm, "Add", "Group", "GrpCd", "TblCd", Fdate, Tdate, "addmode", Privileges);
            //        PmForm_Add.FormClosed += new FormClosedEventHandler(RGroup_AddForm_Closed);
            //        PmForm_Add.StartPosition = FormStartPosition.CenterScreen;
            //        PmForm_Add.ShowDialog();
            //    }
            //}
        }

        private void gvDateRange_SelectionChanged(object sender, EventArgs e)
        {

        }

        string Added_Edited_GroupCode = string.Empty;
        private void RGroup_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            RPerformanceMeasureForm form = sender as RPerformanceMeasureForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Group_Code();
                Added_Edited_GroupCode = From_Results[0];

                if (From_Results[1].Equals("Add"))
                    AlertBox.Show("Domain Details Inserted Successfully");
                else
                    AlertBox.Show("Domain Details Updated Successfully");
                FillRPerformanceGroups(Added_Edited_GroupCode);
                fillRPerformTable(Added_Edited_TableCode);
            }
        }
        private void SRGroup_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            RPerformanceMeasureForm form = sender as RPerformanceMeasureForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Group_Code();
                Added_Edited_GroupCode = From_Results[0];

                if (From_Results[1].Equals("Add"))
                    AlertBox.Show("Group Details Inserted Successfully");
                else
                    AlertBox.Show("Group Details Updated Successfully");
                FillSRPerformanceGroups(Added_Edited_GroupCode);
                fillSRPerformTable(Added_Edited_TableCode);
            }
        }
        string Added_Edited_TableCode = string.Empty;
        private void SRTable_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            RPerformanceMeasureForm form = sender as RPerformanceMeasureForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Group_Code();
                Added_Edited_TableCode = From_Results[1];
                Added_Edited_GroupCode = From_Results[0];
                if (From_Results[2].Equals("Add"))
                    AlertBox.Show("Table Details Inserted Successfully");
                else
                    AlertBox.Show("Table Details Updated Successfully");
                FillSRPerformanceGroups(Added_Edited_GroupCode);
                fillSRPerformTable(Added_Edited_TableCode);
            }
        }

        public void Delete_CAMS_Row(DialogResult dialogresult)
        {
            //Gizmox.WebGUI.Forms.Form senderform = (Gizmox.WebGUI.Forms.Form)sender;

            //if (senderform != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                if (tab_Page == "RPerfMeasures")
                {
                    string strmsg = string.Empty;
                    RCsb14GroupEntity Search_Entity = new RCsb14GroupEntity(true);
                    Search_Entity.Code = gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim();
                    Search_Entity.Agency = gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim();
                    Search_Entity.Mode = "D";
                    //string grp_cd = gvGroup.CurrentRow.Cells["Group_Code"].Value.ToString();
                    //string Tbl_cd=gvGroup.CurrentRow.Cells["Tbl_code"].Value.ToString();
                    if (Captain.DatabaseLayer.SPAdminDB.DeleteRNGGRP(gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim(), gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim(), string.Empty, "Null", "Code", out strmsg))
                    {
                        AlertBox.Show("Deleted Successfully", MessageBoxIcon.Error, null, System.Drawing.ContentAlignment.BottomRight);
                        // MessageBox.Show("Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                        //if (strIndex != 0)
                        //    strIndex = strIndex - 1;
                        FillRDateRange_Grid();
                        FillRPerformanceGroups(Added_Edited_GroupCode);//RefreshGroupGrid();
                        fillRPerformTable(Added_Edited_TableCode);
                    }
                    else
                        if (strmsg == "Already Exist")
                        MessageBox.Show("This Date Range having Domains, so unable to Delete.", "CAPTAIN", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Failed to Delete", "CAPTAIN", MessageBoxButtons.OK);
                }
                else if (tab_Page == "Demographics")
                {
                    string Colcode = dgvDemographics.CurrentRow.Cells["Col_Code"].Value.ToString();
                    string SubCode = dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString();
                    if (Captain.DatabaseLayer.SPAdminDB.DeleteRNG4ASOC(Colcode, SubCode))
                        MessageBox.Show("DemoGraphic Association For" + /*GvDemoGraph.CurrentRow.Cells["Col_Code"].Value.ToString() +*/ " " + dgvDemographics.CurrentRow.Cells["Demographics"].Value.ToString() + " " + "Deleted Successfully", "CAPTAIN", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Failed to Delete", "CAPTAIN", MessageBoxButtons.OK);
                    Added_Edited_DemoCode = string.Empty;
                    RefreshGridAgy();
                }
                else if (tab_Page == "RServices")
                {
                    string strmsg = string.Empty;
                    SRCsb14GroupEntity Search_Entity = new SRCsb14GroupEntity(true);
                    Search_Entity.Code = gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim();
                    //Search_Entity.REF_TDATE = gvRdateRange.CurrentRow.Cells["RTo_Date"].Value.ToString().Trim();
                    Search_Entity.Mode = "D";
                    //string grp_cd = gvGroup.CurrentRow.Cells["Group_Code"].Value.ToString();
                    //string Tbl_cd=gvGroup.CurrentRow.Cells["Tbl_code"].Value.ToString();
                    if (Captain.DatabaseLayer.SPAdminDB.DeleteRNGSRGRP(gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim(), gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim(), string.Empty, "Null", "Code", out strmsg))
                    {
                        AlertBox.Show("Deleted Successfully", MessageBoxIcon.Error, null, System.Drawing.ContentAlignment.BottomRight);
                        //if (strIndex != 0)
                        //    strIndex = strIndex - 1;
                        FillSRDateRange_Grid();
                        FillSRPerformanceGroups(Added_Edited_GroupCode);//RefreshGroupGrid();
                        fillSRPerformTable(Added_Edited_TableCode);
                    }
                    else
                        if (strmsg == "Already Exist")
                        MessageBox.Show("This Date Range having Groups, so unable to Delete.", "CAPTAIN", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Failed to Delete", "CAPTAIN", MessageBoxButtons.OK);
                }
            }
            //}
        }



        public void RefreshGridAgy()
        {
            fillGvDemo(Added_Edited_DemoCode);
            FillDemoGraphControls();
            if (dgvRAgencies.Rows.Count != 0)
            {
                dgvRAgencies.CurrentCell = dgvRAgencies.Rows[strIndex].Cells[1];
                dgvRAgencies.Rows[strIndex].Selected = true;
                //dgvRAgencies.CurrentPage = strPageIndex;
            }
        }
        private void RDate_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            RPerformanceMeasureForm form = sender as RPerformanceMeasureForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[2];
                From_Results = form.GetSelected_Group_Code();
                Added_Edited_Date = From_Results[0];

                if (From_Results[1].Equals("Add"))
                    AlertBox.Show("Detials Inserted Successfully");
                else
                    AlertBox.Show("Detials Updated Successfully");//("Updated Successfully");
                //fillComboRefDate();
                FillRDateRange_Grid();
            }
        }
        private void SRDate_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            RPerformanceMeasureForm form = sender as RPerformanceMeasureForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[2];
                From_Results = form.GetSelected_Group_Code();
                Added_Edited_Date = From_Results[0];

                if (From_Results[1].Equals("Add"))
                    AlertBox.Show("Detials Inserted Successfully");
                else
                    AlertBox.Show("Detials Updated Successfully");//("Updated Successfully");
                //fillComboRefDate();
                FillSRDateRange_Grid();
            }
        }

        private void gvRTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.ChangePriv.Equals("true"))
            {
                if (e.ColumnIndex == 3 && e.RowIndex != -1)
                {
                    strIndex = gvRTable.SelectedRows[0].Index;
                    //strPageIndex = gvRTable.CurrentPage;
                    string code = gvRTable.CurrentRow.Cells["RGrp_cd"].Value.ToString();
                    RPerformanceMeasureForm PMForm = new RPerformanceMeasureForm(BaseForm, "Edit", "Table", gvRTable.CurrentRow.Cells["RGrp_cd"].Value.ToString(), gvRTable.CurrentRow.Cells["RTable_Code"].Value.ToString(), Fdate, gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim(), "EditMode", Privileges, "RPerfMeasures");
                    PMForm.FormClosed += new FormClosedEventHandler(RTable_AddForm_Closed);
                    PMForm.StartPosition = FormStartPosition.CenterScreen;
                    PMForm.ShowDialog();
                }
            }
            if (Privileges.DelPriv.Equals("true"))
            {
                if (e.ColumnIndex == 4 && e.RowIndex != -1)
                {
                    strIndex = gvRTable.SelectedRows[0].Index;
                    //strPageIndex = gvRTable.CurrentPage;
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Group with code - " + gvRTable.CurrentRow.Cells["RTable_code"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: RDeleteTableRow);
                }
            }
        }
        private void gvSRTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.ChangePriv.Equals("true"))
            {
                if (e.ColumnIndex == 3 && e.RowIndex != -1)
                {
                    strIndex = gvSRTable.SelectedRows[0].Index;
                    //strPageIndex = gvSRTable.CurrentPage;
                    string code = gvSRTable.CurrentRow.Cells["SRGrp_cd"].Value.ToString();
                    RPerformanceMeasureForm PMForm = new RPerformanceMeasureForm(BaseForm, "Edit", "Table", gvSRTable.CurrentRow.Cells["SRGrp_cd"].Value.ToString(), gvSRTable.CurrentRow.Cells["SRTable_Code"].Value.ToString(), Fdate, gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim(), "EditMode", Privileges, "RServices");
                    PMForm.FormClosed += new FormClosedEventHandler(SRTable_AddForm_Closed);
                    PMForm.StartPosition = FormStartPosition.CenterScreen;
                    PMForm.ShowDialog();
                }
            }
            if (Privileges.DelPriv.Equals("true"))
            {
                if (e.ColumnIndex == 4 && e.RowIndex != -1)
                {
                    strIndex = gvSRTable.SelectedRows[0].Index;
                    //strPageIndex = gvSRTable.CurrentPage;
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Table with code - " + gvSRTable.CurrentRow.Cells["SRTable_code"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: SRDeleteTableRow);
                }
            }
        }
        private void gvRdateRange_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.AddPriv.Equals("true"))
            {
                if (e.ColumnIndex == 6 && e.RowIndex != -1)
                {
                    Added_Edited_GroupCode = string.Empty;
                    RPerformanceMeasureForm PmForm_Add = new RPerformanceMeasureForm(BaseForm, "Add", "Group", "GrpCd", "TblCd", Fdate, Tdate, "addmode", Privileges, "RPerfMeasures");
                    PmForm_Add.FormClosed += new FormClosedEventHandler(RGroup_AddForm_Closed);
                    PmForm_Add.StartPosition = FormStartPosition.CenterScreen;
                    PmForm_Add.ShowDialog();
                }
            }
        }

        string Refdate = null;
        private void gvRdateRange_SelectionChanged(object sender, EventArgs e)
        {
            Added_Edited_GroupCode = "Empty"; //Added_Edited_TableCode = string.Empty;
            Refdate = gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim();
            //RefTdate = Refdate.Split(',');
            Fdate = gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim();
            Tdate = gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim();
            FillRPerformanceGroups(Added_Edited_GroupCode);
            if (Added_Edited_GroupCode != "Empty")
                fillRPerformTable(Added_Edited_TableCode);
        }
        private void gvSRdateRange_SelectionChanged(object sender, EventArgs e)
        {
            Added_Edited_GroupCode = "Empty"; //Added_Edited_TableCode = string.Empty;
            Refdate = gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim();
            //RefTdate = Refdate.Split(',');
            Fdate = gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim();
            Tdate = gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim();
            FillSRPerformanceGroups(Added_Edited_GroupCode);
            if (Added_Edited_GroupCode != "Empty")
                fillSRPerformTable(Added_Edited_TableCode);
        }

        private void gvRGroup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.AddPriv.Equals("true"))
            {
                if (e.ColumnIndex == 5 && e.RowIndex != -1)
                {
                    Added_Edited_GroupCode = string.Empty;
                    RPerformanceMeasureForm PMForm = new RPerformanceMeasureForm(BaseForm, "Add", "Table", gvRGroup.CurrentRow.Cells["RGroup_Code"].Value.ToString(), "TableCd", Fdate, gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim(), "AddMode", Privileges, "RPerfMeasures");
                    PMForm.FormClosed += new FormClosedEventHandler(RTable_AddForm_Closed);
                    PMForm.StartPosition = FormStartPosition.CenterScreen;
                    PMForm.ShowDialog();
                }
                if (e.ColumnIndex == 3 && e.RowIndex != -1)
                {
                    RPerformanceMeasureForm PmForm_Edit = new RPerformanceMeasureForm(BaseForm, "Edit", "Group", gvRGroup.CurrentRow.Cells["RGroup_Code"].Value.ToString(), "TblCd", Fdate, gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim(), "EditGrp", Privileges, "RPerfMeasures");
                    PmForm_Edit.FormClosed += new FormClosedEventHandler(RGroup_AddForm_Closed);
                    PmForm_Edit.StartPosition = FormStartPosition.CenterScreen;
                    PmForm_Edit.ShowDialog();
                }
                if (e.ColumnIndex == 4 && e.RowIndex != -1)
                {
                    if (gvRGroup.Rows.Count > 0)
                    {
                        strIndex = gvRGroup.SelectedRows[0].Index;
                        //strPageIndex = gvRGroup.CurrentPage;
                        int RowCnt = 0;
                        RGroupList = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                        if (RGroupList.Count > 0)
                        {
                            foreach (RCsb14GroupEntity GrpEnt in RGroupList)
                            {
                                if (GrpEnt.Code.ToString() == gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString().Trim() && GrpEnt.Agency.ToString() == gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString().Trim() && GrpEnt.GrpCode.ToString().Trim() == gvRGroup.CurrentRow.Cells["RGroup_code"].Value.ToString().Trim())
                                {
                                    RowCnt++;
                                }
                            }
                        }
                        if (RowCnt > 1)
                            MessageBox.Show("You must delete all associated Groups before deleting the Domain.", "CAPTAIN", MessageBoxButtons.OK);
                        else
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Domain with code - " + gvRGroup.CurrentRow.Cells["RGroup_code"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: DeleteRGroupRow);
                    }
                }
            }
        }

        private void gvRGroup_SelectionChanged(object sender, EventArgs e)
        {
            if (gvRGroup.Rows.Count > 0)
            {
                Added_Edited_TableCode = string.Empty;
                fillRPerformTable(Added_Edited_TableCode);
            }
        }
        private void gvSRGroup_SelectionChanged(object sender, EventArgs e)
        {
            if (gvSRGroup.Rows.Count > 0)
            {
                Added_Edited_TableCode = string.Empty;
                fillSRPerformTable(Added_Edited_TableCode);
            }
        }

        private void txtTo_LostFocus(object sender, EventArgs e)
        {

        }

        private void txtfrom_LostFocus(object sender, EventArgs e)
        {

        }


        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedIndex == 0)
            {
                tab_Page = "RPerfMeasures";
                //this.PMeasures.Size = new System.Drawing.Size(698, 386);
                ToolBarPrint.Enabled = true;
                ToolBarNew.ToolTipText = "Add Date Range";
                ToolBarEdit.ToolTipText = "Edit Date Range";
                ToolBarDel.ToolTipText = "Delete Date Range";
                FillRDateRange_Grid();

                if (gvRdateRange.Rows.Count == 0)
                {
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = false;
                    if (ToolBarPrint != null)
                        ToolBarPrint.Enabled = false;
                }
                else
                {
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = true;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = true;
                    if (ToolBarPrint != null)
                        ToolBarPrint.Enabled = true;
                }
            }
            else if (tabControl2.SelectedIndex == 1)
            {
                tab_Page = "Demographics";
                //this.DemographicsAssociations.Size = new System.Drawing.Size(698, 386);
                ToolBarPrint.Enabled = true;
                ToolBarNew.ToolTipText = "Define Demographics Associations";
                ToolBarEdit.ToolTipText = "Edit Demographics Associations";
                ToolBarDel.ToolTipText = "Delete Demographic Associations";

                if (dgvDemographics.Rows.Count == 0)
                {
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = false;
                    if (ToolBarPrint != null)
                        ToolBarPrint.Enabled = false;
                }
                else
                {
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = true;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = true;
                    if (ToolBarPrint != null)
                        ToolBarPrint.Enabled = true;
                }
            }
            else if (tabControl2.SelectedIndex == 2)
            {
                tab_Page = "RServices";
                //this.PMeasures.Size = new System.Drawing.Size(698, 386);
                ToolBarPrint.Enabled = true;
                ToolBarNew.ToolTipText = "Add Date Range";
                ToolBarEdit.ToolTipText = "Edit Date Range";
                ToolBarDel.ToolTipText = "Delete Date Range";

                FillSRDateRange_Grid();

                if (gvRdateRange.Rows.Count == 0)
                {
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = false;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = false;
                    if (ToolBarPrint != null)
                        ToolBarPrint.Enabled = false;
                }
                else
                {
                    if (ToolBarEdit != null)
                        ToolBarEdit.Enabled = true;
                    if (ToolBarDel != null)
                        ToolBarDel.Enabled = true;
                    if (ToolBarPrint != null)
                        ToolBarPrint.Enabled = true;
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtfrom.Text.Trim())) txtfrom.Text = "0";
                if (string.IsNullOrEmpty(txtTo.Text.Trim())) txtTo.Text = "999";
                if (ValidateForm())
                {
                    string Column = dgvDemographics.CurrentRow.Cells["Cat_subCode"].Value.ToString();
                    //string Agy_Code=GvAgency.CurrentRow.Cells["Agy_Code"].Value.ToString();
                    string Agy_Code = null;

                    // CaptainModel model = new CaptainModel();
                    RNG4AsocEntity RNGAssEntity = new RNG4AsocEntity();

                    RNGAssEntity.RNGCatCode = cat_code.Substring(0, 1).ToString();
                    RNGAssEntity.RNGDemoCd = Column;
                    RNGAssEntity.RNGAgeFrm = txtfrom.Text;
                    RNGAssEntity.RNGAgeTo = txtTo.Text;
                    RNGAssEntity.RNGlstcOperator = BaseForm.UserID;
                    RNGAssEntity.RNGaddoperator = BaseForm.UserID;
                    foreach (DataGridViewRow dr in dgvRAgencies.Rows)
                    {
                        if (dr.Cells["Img_code"].Value.ToString() == "Y")
                            Agy_Code += SetLeadingSpaces(dr.Cells["Agy_Code"].Value.ToString());
                    }
                    RNGAssEntity.RNGAgytabCds = Agy_Code;
                    RNGAssEntity.RNGMode = Selmode;


                    if (_model.SPAdminData.InsertUpdateRNG4Assoc(RNGAssEntity))
                    {
                        //if (strCrIndex != 0)
                        //    strCrIndex = strCrIndex - 1;

                        Added_Edited_DemoCode = RNGAssEntity.RNGDemoCd;
                        dgvRAgencies.Enabled = true;
                        dgvDemographics.Enabled = true;

                        if (RNGAssEntity.RNGCatCode == "I")
                        {
                            RNGAssEntity.RNGDemoCd = "g";
                            RNGAssEntity.RNGAgeFrm = txtfrom.Text;
                            RNGAssEntity.RNGAgeTo = txtTo.Text;
                            RNGAssEntity.RNGlstcOperator = BaseForm.UserID;
                            RNGAssEntity.RNGaddoperator = BaseForm.UserID;

                            RNGAssEntity.RNGAgytabCds = Value_13g;
                            RNGAssEntity.RNGMode = Selmode;

                            _model.SPAdminData.InsertUpdateRNG4Assoc(RNGAssEntity);
                        }


                        fillGvDemo(Added_Edited_DemoCode);

                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        txtfrom.Enabled = false;
                        txtTo.Enabled = false;
                        lblARTo.Visible = false;
                        if (Selmode == "Add")
                            AlertBox.Show("DemoGraphics Details Inserted Successfully");
                        else
                            AlertBox.Show("DemoGraphics Details Updated Successfully");
                        dgvDemographics_SelectionChanged(sender, e);
                        Selmode = "View";
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        public bool ValidateForm()
        {
            bool isValid = true;
            if (!string.IsNullOrEmpty(txtTo.Text) && (!string.IsNullOrEmpty(txtfrom.Text)))
            {
                if (int.Parse(txtTo.Text) < int.Parse(txtfrom.Text))
                {
                    _errorProvider.SetError(txtTo, string.Format(lblFrom.Text + " Value should not be greater than To Value.".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
            }
            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                if (int.Parse(txtTo.Text.Trim()) < 0)
                {
                    _errorProvider.SetError(txtTo, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtTo, null);
            }
            if (!string.IsNullOrEmpty(txtfrom.Text))
            {
                if (int.Parse(txtfrom.Text.Trim()) < 0)
                {
                    _errorProvider.SetError(txtfrom, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtTo, null);
            }
            if ((string.IsNullOrEmpty(txtfrom.Text) && !string.IsNullOrEmpty(txtTo.Text)) || (!string.IsNullOrEmpty(txtfrom.Text) && string.IsNullOrEmpty(txtTo.Text)))
            {
                _errorProvider.SetError(txtTo, string.Format(lblFrom.Text + "  or To Value is Required.".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTo, null);
            }

            IsSaveValid = isValid;

            return (isValid);
        }
        private string SetLeadingSpaces(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 4: TmpCode = TmpCode + " "; break;
                case 3: TmpCode = TmpCode + "  "; break;
                case 2: TmpCode = TmpCode + "   "; break;
                case 1: TmpCode = TmpCode + "    "; break;
            }
            return (TmpCode);
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            dgvDemographics.Enabled = true;
            fillGvDemo(Added_Edited_DemoCode);
            btnSave.Visible = false;
            btnCancel.Visible = false;
            txtfrom.Enabled = false;
            txtTo.Enabled = false;
            lblARTo.Visible = false;

            _errorProvider.SetError(txtTo, null);
            Selmode = "View";
        }

        private void dgvRAgencies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRAgencies.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0 && (Selmode.Equals("Add") || Selmode.Equals("Edit")))
                {
                    if (dgvRAgencies.CurrentRow.Cells["Img_code"].Value.ToString() == "Y")
                    {
                        dgvRAgencies.CurrentRow.Cells["Check"].Value = Img_Blank;
                        dgvRAgencies.CurrentRow.Cells["Img_code"].Value = "N";
                    }
                    else
                    {
                        dgvRAgencies.CurrentRow.Cells["Check"].Value = Img_Tick;
                        dgvRAgencies.CurrentRow.Cells["Img_code"].Value = "Y";
                    }
                }
            }
        }

        private void gvSRdateRange_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.AddPriv.Equals("true"))
            {
                if (e.ColumnIndex == 6 && e.RowIndex != -1)
                {
                    Added_Edited_GroupCode = string.Empty;
                    RPerformanceMeasureForm PmForm_Add = new RPerformanceMeasureForm(BaseForm, "Add", "Group", "GrpCd", "TblCd", Fdate, gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim(), "addmode", Privileges, "RServices");
                    PmForm_Add.FormClosed += new FormClosedEventHandler(SRGroup_AddForm_Closed);
                    PmForm_Add.StartPosition = FormStartPosition.CenterScreen;
                    PmForm_Add.ShowDialog();
                }
            }
        }
        private void RTable_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            RPerformanceMeasureForm form = sender as RPerformanceMeasureForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Group_Code();
                Added_Edited_TableCode = From_Results[1];
                Added_Edited_GroupCode = From_Results[0];
                if (From_Results[2].Equals("Add"))
                    AlertBox.Show("Group Details Inserted Successfully");
                else
                    AlertBox.Show("Group Details Updated Successfully");
                FillRPerformanceGroups(Added_Edited_GroupCode);
                fillRPerformTable(Added_Edited_TableCode);
            }
        }
        private void gvSRGroup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.AddPriv.Equals("true"))
            {
                if (e.ColumnIndex == 5 && e.RowIndex != -1)
                {
                    Added_Edited_GroupCode = string.Empty;
                    RPerformanceMeasureForm PMForm = new RPerformanceMeasureForm(BaseForm, "Add", "Table", gvSRGroup.CurrentRow.Cells["SRGroup_Code"].Value.ToString(), "TableCd", Fdate, gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim(), "AddMode", Privileges, "RServices");
                    PMForm.FormClosed += new FormClosedEventHandler(SRTable_AddForm_Closed);
                    PMForm.StartPosition = FormStartPosition.CenterScreen;
                    PMForm.ShowDialog();
                }
                if (e.ColumnIndex == 3 && e.RowIndex != -1)
                {
                    RPerformanceMeasureForm PmForm_Edit = new RPerformanceMeasureForm(BaseForm, "Edit", "Group", gvSRGroup.CurrentRow.Cells["SRGroup_Code"].Value.ToString(), "TblCd", Fdate, gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim(), "EditGrp", Privileges, "RServices");
                    PmForm_Edit.FormClosed += new FormClosedEventHandler(SRGroup_AddForm_Closed);
                    PmForm_Edit.StartPosition = FormStartPosition.CenterScreen;
                    PmForm_Edit.ShowDialog();
                }
                if (e.ColumnIndex == 4 && e.RowIndex != -1)
                {
                    if (gvSRGroup.Rows.Count > 0)
                    {
                        strIndex = gvSRGroup.SelectedRows[0].Index;
                        //strPageIndex = gvSRGroup.CurrentPage;
                        int RowCnt = 0;
                        SRGroupList = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null);
                        if (SRGroupList.Count > 0)
                        {
                            foreach (SRCsb14GroupEntity GrpEnt in SRGroupList)
                            {
                                if (GrpEnt.Code.ToString() == gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString().Trim() && GrpEnt.Agency.ToString() == gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString().Trim() && GrpEnt.GrpCode.ToString().Trim() == gvSRGroup.CurrentRow.Cells["SRGroup_code"].Value.ToString().Trim())
                                {
                                    RowCnt++;
                                }
                            }
                        }
                        if (RowCnt > 1)
                            MessageBox.Show("You must delete all associated Tables before deleting the Group.", "CAPTAIN", MessageBoxButtons.OK);
                        else
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Group with code - " + gvSRGroup.CurrentRow.Cells["SRGroup_code"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: DeleteSRGroupRow);
                    }
                }
            }
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        int pageNumber = 1; string PdfName = "Pdf File";
        string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        string PrintText = null;
        private void On_SaveForm_Closed()//object sender, FormClosedEventArgs e)
        {
            Random_Filename = null;

            PdfName = tab_Page.ToString() + "Report";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            cb = writer.DirectContent;
            //string Priv_Scr = null;
            //document = new Document(iTextSharp.text.PageSize.A4.Rotate());

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 11, 1, BaseColor.BLUE);


            if (tab_Page == "Demographics")
            {
                try
                {
                    DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetRNGCAT();
                    DataTable dt = ds.Tables[0];
                    DataSet dsAssoc = DatabaseLayer.ADMNB001DB.ADMNB001_GetRNG4ASSOC();
                    DataTable dtAssoc = dsAssoc.Tables[0];

                    string PrivAgyDesc = null;
                    string privTable = null;

                    string AgyType = null, Temp_Agy_Codes = null, Temp_Code = null;
                    if (dt.Rows.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(7);
                        table.TotalWidth = 560f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 33f, 60f, 14f, 11f, 10f, 8f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.HeaderRows = 2;

                        PdfPCell Hdr = new PdfPCell(new Phrase("Demographics Associations", TblFontBoldColor));
                        Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                        Hdr.FixedHeight = 15f;
                        Hdr.Colspan = 7;
                        Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Hdr);

                        string[] Header = { "CSBG Category", "Description Codes Used", "Age From", "Age To", "Table#", "Code", "Desc" };
                        for (int i = 0; i < Header.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.FixedHeight = 15f;
                            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            table.AddCell(cell);
                        }

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (PrivAgyDesc != dr["RNG4CATG_DESC"].ToString())
                            {
                                PdfPCell AGY_DESC = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                                AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(AGY_DESC);
                                PrivAgyDesc = dr["RNG4CATG_DESC"].ToString();
                                AgyType = dr["RNG4CATG_AGY_TYPE"].ToString();
                            }
                            else
                            {
                                PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(AGY_DESC);
                            }
                            bool First = true;
                            DataSet dsAgyCodes = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(AgyType);
                            DataTable dtAgyCodes = new DataTable();
                            List<AGYTABSEntity> AgyTabs_List = new List<AGYTABSEntity>();
                            if (dr["RNG4CATG_CODE"].ToString() == "N")
                            {
                                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                                searchAgytabs.Tabs_Type = AgyType;
                                AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);
                            }
                            else
                            {
                                if (dsAgyCodes.Tables.Count > 0) dtAgyCodes = dsAgyCodes.Tables[0];
                            }
                            string AgycodesTemp = string.Empty; string NotAgycodes = string.Empty;
                            if (dr["RNG4CATG_CODE"].ToString() != "I")
                            {
                                foreach (DataRow drAssoc in dtAssoc.Rows)
                                {
                                    if (drAssoc["RNG4ASOC_CATG_CODE"].ToString() == dr["RNG4CATG_CODE"].ToString())
                                    {
                                        if (!First)
                                        {
                                            PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                            AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                            AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(AGY_DESC);
                                        }

                                        PdfPCell CSB4CATG_DESC = new PdfPCell(new Phrase(drAssoc["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                                        CSB4CATG_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                        CSB4CATG_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(CSB4CATG_DESC);

                                        PdfPCell AgeFrom = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_FROM"].ToString().Trim(), TableFont));
                                        AgeFrom.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        AgeFrom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(AgeFrom);

                                        PdfPCell Ageto = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_TO"].ToString().Trim(), TableFont));
                                        Ageto.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        Ageto.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Ageto);

                                        Temp_Agy_Codes = drAssoc["RNG4ASOC_AGYTAB_CODES"].ToString();

                                        //PrivAgyDesc = null;
                                        if (privTable != dr["TableUsed"].ToString())
                                        {
                                            if (!string.IsNullOrEmpty(dr["TableUsed"].ToString().Trim()))
                                            {
                                                PdfPCell TableUsed = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(0, 5).ToString(), TableFont));
                                                TableUsed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(TableUsed);

                                                PdfPCell TableUsed1 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(6, 1).ToString(), TableFont));
                                                TableUsed1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                TableUsed1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(TableUsed1);

                                                PdfPCell TableUsed2 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(8, 1).ToString(), TableFont));
                                                TableUsed2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                TableUsed2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(TableUsed2);

                                                privTable = dr["TableUsed"].ToString();
                                            }
                                            else
                                            {
                                                PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                                TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                                TableUsed.Colspan = 3;
                                                TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(TableUsed);
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                            TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            TableUsed.Colspan = 3;
                                            TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(TableUsed);
                                        }

                                        if (dr["RNG4CATG_CODE"].ToString() == "N")
                                        {
                                            foreach (AGYTABSEntity drAgency in AgyTabs_List)
                                            {
                                                bool Can_Continue = true;
                                                if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 1)
                                                {
                                                    for (int i = 0; Can_Continue;)
                                                    {

                                                        if (Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i).Length < 5)
                                                            Temp_Code = Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i);
                                                        else
                                                            Temp_Code = Temp_Agy_Codes.Substring(i, 5);
                                                        if (drAgency.Table_Code.ToString().Trim() == Temp_Code.Trim())
                                                        {
                                                            if (!string.IsNullOrEmpty(AgycodesTemp.Trim()))
                                                            {
                                                                if (AgycodesTemp.Contains(drAgency.Table_Code.ToString().Trim()))
                                                                { }
                                                                else
                                                                {
                                                                    if (NotAgycodes.Contains(drAgency.Table_Code.ToString().Trim()))
                                                                    {
                                                                        NotAgycodes = NotAgycodes.Replace(drAgency.Table_Code.ToString().Trim(), "");
                                                                        AgycodesTemp += drAgency.Table_Code.ToString().Trim() + " ";
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (NotAgycodes.Contains(drAgency.Table_Code.ToString().Trim()))
                                                                {
                                                                    NotAgycodes = NotAgycodes.Replace(drAgency.Table_Code.ToString().Trim(), "");
                                                                    AgycodesTemp += drAgency.Table_Code.ToString().Trim() + " ";
                                                                }
                                                                else AgycodesTemp += drAgency.Table_Code.ToString().Trim() + " ";
                                                            }
                                                            PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(Space);

                                                            PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                            Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Code.Colspan = 6;
                                                            Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(Code);

                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                                                            {
                                                                if (NotAgycodes.Contains(drAgency.Table_Code.ToString().Trim()))
                                                                { }
                                                                else
                                                                {
                                                                    if (AgycodesTemp.Contains(drAgency.Table_Code.ToString().Trim())) { }
                                                                    else
                                                                        NotAgycodes += drAgency.Table_Code.ToString().Trim() + " ";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (AgycodesTemp.Contains(drAgency.Table_Code.ToString().Trim())) { }
                                                                else
                                                                    NotAgycodes += drAgency.Table_Code.ToString().Trim() + " ";
                                                            }
                                                        }

                                                        i += 5;
                                                        if (i >= Temp_Agy_Codes.Length)
                                                            Can_Continue = false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (DataRow drAgyCodes in dtAgyCodes.Rows)
                                            {
                                                //bool Code_Exists = false;
                                                bool Can_Continue = true;
                                                if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 1)
                                                {
                                                    for (int i = 0; Can_Continue;)
                                                    {

                                                        if (Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i).Length < 5)
                                                            Temp_Code = Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i);
                                                        else
                                                            Temp_Code = Temp_Agy_Codes.Substring(i, 5);
                                                        if (drAgyCodes["Code"].ToString().Trim() == Temp_Code.Trim())
                                                        {
                                                            if (!string.IsNullOrEmpty(AgycodesTemp.Trim()))
                                                            {
                                                                if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                { }
                                                                else
                                                                {
                                                                    if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                    {
                                                                        NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                        AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                {
                                                                    NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                    AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                }
                                                                else AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                            }
                                                            PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(Space);

                                                            PdfPCell Code = new PdfPCell(new Phrase("    " + drAgyCodes["Code"].ToString().Trim() + "  " + drAgyCodes["LookUpDesc"].ToString().Trim(), TableFont));
                                                            Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Code.Colspan = 6;
                                                            Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(Code);

                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                                                            {
                                                                if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                { }
                                                                else
                                                                {
                                                                    if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                                    else
                                                                        NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                                else
                                                                    NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                            }
                                                        }

                                                        i += 5;
                                                        if (i >= Temp_Agy_Codes.Length)
                                                            Can_Continue = false;
                                                    }
                                                }
                                            }
                                        }

                                        PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                        Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Space3.Colspan = 7;
                                        Space3.FixedHeight = 15f;
                                        Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Space3);
                                        First = false;
                                    }

                                }
                            }
                            else
                            {
                                if (dr["RNG4CATG_CODE"].ToString() == "I")
                                {
                                    DataTable SrcInc = dscategories.Tables[0];
                                    DataView dv = new DataView(SrcInc);
                                    dv.RowFilter = "RNG4CATG_CODE='I'AND RNG4CATG_DEM<>''";// AND RNG4CATG_DEM<>'a' AND RNG4CATG_DEM<>'g' AND RNG4CATG_DEM<>'i'";
                                    SrcInc = dv.ToTable();

                                    List<AGYTABSEntity> AgyTabs_Incomes = new List<AGYTABSEntity>();
                                    List<AGYTABSEntity> AgyTabs_NCB = new List<AGYTABSEntity>();
                                    AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                                    searchAgytabs.Tabs_Type = "00004";
                                    AgyTabs_Incomes = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                                    AGYTABSEntity searchAgyNCB = new AGYTABSEntity(true);
                                    searchAgyNCB.Tabs_Type = "00037";
                                    AgyTabs_NCB = _model.AdhocData.Browse_AGYTABS(searchAgyNCB);

                                    foreach (DataRow drsrcinc in SrcInc.Rows)
                                    {
                                        if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "a" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "g" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "i")
                                        {
                                            DataTable dtsrcAsc = dtAssoc;
                                            DataView dvsrc = new DataView(dtsrcAsc);
                                            dvsrc.RowFilter = "RNG4ASOC_CATG_CODE='I' AND RNG4ASOC_DEMO_CODE='" + drsrcinc["RNG4CATG_DEM"].ToString().Trim() + "'";
                                            dtsrcAsc = dvsrc.ToTable();

                                            foreach (DataRow drAssoc in dtsrcAsc.Rows)
                                            {
                                                if (drAssoc["RNG4ASOC_CATG_CODE"].ToString() == dr["RNG4CATG_CODE"].ToString())
                                                {
                                                    if (!First)
                                                    {
                                                        PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                                        AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(AGY_DESC);
                                                    }

                                                    PdfPCell CSB4CATG_DESC = new PdfPCell(new Phrase(drAssoc["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                                                    CSB4CATG_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    CSB4CATG_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(CSB4CATG_DESC);

                                                    PdfPCell AgeFrom = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_FROM"].ToString().Trim(), TableFont));
                                                    AgeFrom.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    AgeFrom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(AgeFrom);

                                                    PdfPCell Ageto = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_TO"].ToString().Trim(), TableFont));
                                                    Ageto.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Ageto.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(Ageto);

                                                    Temp_Agy_Codes = drAssoc["RNG4ASOC_AGYTAB_CODES"].ToString();

                                                    //PrivAgyDesc = null;
                                                    if (privTable != dr["TableUsed"].ToString())
                                                    {
                                                        if (!string.IsNullOrEmpty(dr["TableUsed"].ToString().Trim()))
                                                        {
                                                            PdfPCell TableUsed = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(0, 5).ToString(), TableFont));
                                                            TableUsed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                            TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(TableUsed);

                                                            PdfPCell TableUsed1 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(6, 1).ToString(), TableFont));
                                                            TableUsed1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                            TableUsed1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(TableUsed1);

                                                            PdfPCell TableUsed2 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(8, 1).ToString(), TableFont));
                                                            TableUsed2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                            TableUsed2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(TableUsed2);

                                                            privTable = dr["TableUsed"].ToString();
                                                        }
                                                        else
                                                        {
                                                            PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                                            TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            TableUsed.Colspan = 3;
                                                            TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            table.AddCell(TableUsed);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                                        TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        TableUsed.Colspan = 3;
                                                        TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(TableUsed);
                                                    }

                                                    foreach (DataRow drAgyCodes in dtAgyCodes.Rows)
                                                    {
                                                        //bool Code_Exists = false;
                                                        bool Can_Continue = true;
                                                        if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 1)
                                                        {
                                                            for (int i = 0; Can_Continue;)
                                                            {

                                                                if (Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i).Length < 5)
                                                                    Temp_Code = Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i);
                                                                else
                                                                    Temp_Code = Temp_Agy_Codes.Substring(i, 5);
                                                                if (drAgyCodes["Code"].ToString().Trim() == Temp_Code.Trim())
                                                                {
                                                                    if (!string.IsNullOrEmpty(AgycodesTemp.Trim()))
                                                                    {
                                                                        if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                        { }
                                                                        else
                                                                        {
                                                                            if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                            {
                                                                                NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                                AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                        {
                                                                            NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                            AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                        }
                                                                        else AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                    }
                                                                    PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Space);

                                                                    PdfPCell Code = new PdfPCell(new Phrase("    " + drAgyCodes["Code"].ToString().Trim() + "  " + drAgyCodes["LookUpDesc"].ToString().Trim(), TableFont));
                                                                    Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Code.Colspan = 6;
                                                                    Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Code);

                                                                }
                                                                else
                                                                {
                                                                    if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                                                                    {
                                                                        if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                        { }
                                                                        else
                                                                        {
                                                                            if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                                            else
                                                                                NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                                        else
                                                                            NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                    }
                                                                }

                                                                i += 5;
                                                                if (i >= Temp_Agy_Codes.Length)
                                                                    Can_Continue = false;
                                                            }
                                                        }
                                                    }


                                                    PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                                    Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Space3.Colspan = 7;
                                                    Space3.FixedHeight = 15f;
                                                    Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(Space3);
                                                    First = false;


                                                }

                                            }
                                        }
                                        else
                                        {
                                            PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                            AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                            AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(AGY_DESC);

                                            PdfPCell CSB4CATG_DESC = new PdfPCell(new Phrase(drsrcinc["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                                            CSB4CATG_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                            CSB4CATG_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(CSB4CATG_DESC);

                                            PdfPCell AgeFrom = new PdfPCell(new Phrase("0", TableFont));
                                            AgeFrom.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            AgeFrom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(AgeFrom);

                                            PdfPCell Ageto = new PdfPCell(new Phrase("999", TableFont));
                                            Ageto.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Ageto.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Ageto);

                                            PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                            TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            TableUsed.Colspan = 3;
                                            TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(TableUsed);


                                            if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "b" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "c" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "d")
                                            {
                                                if (!string.IsNullOrEmpty(EMPCodes.Trim()))
                                                {
                                                    PdfPCell AGY_DESC1 = new PdfPCell(new Phrase("", TableFont));
                                                    AGY_DESC1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    AGY_DESC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(AGY_DESC1);

                                                    PdfPCell Space = new PdfPCell(new Phrase("Income for Employment", TblFontBoldColor));
                                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Space.Colspan = 6;
                                                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(Space);



                                                    string[] Temp_Codes = EMPCodes.Split(',');

                                                    foreach (AGYTABSEntity drAgency in AgyTabs_Incomes)
                                                    {

                                                        if (Temp_Codes != null)
                                                        {
                                                            for (int i = 0; i < Temp_Codes.Length; i++)
                                                            {
                                                                if (drAgency.Table_Code.ToString().Trim() == Temp_Codes[i].Trim())
                                                                {
                                                                    PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                                                    Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Space1);

                                                                    PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                                    Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Code.Colspan = 6;
                                                                    Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Code);
                                                                }
                                                            }
                                                        }

                                                    }

                                                }

                                            }

                                            if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "b" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "c" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "e" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "f")
                                            {
                                                if (!string.IsNullOrEmpty(IncCodes.Trim()))
                                                {
                                                    if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() != "e")
                                                    {
                                                        PdfPCell AGY_DESC1 = new PdfPCell(new Phrase("", TableFont));
                                                        AGY_DESC1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        AGY_DESC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(AGY_DESC1);

                                                        PdfPCell Space = new PdfPCell(new Phrase("Other Income Sources", TblFontBoldColor));
                                                        Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        Space.Colspan = 6;
                                                        Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(Space);
                                                    }
                                                    string[] Temp_Codes = IncCodes.Split(',');

                                                    foreach (AGYTABSEntity drAgency in AgyTabs_Incomes)
                                                    {

                                                        if (Temp_Codes != null)
                                                        {
                                                            for (int i = 0; i < Temp_Codes.Length; i++)
                                                            {
                                                                if (drAgency.Table_Code.ToString().Trim() == Temp_Codes[i].Trim())
                                                                {
                                                                    PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Space);

                                                                    PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                                    Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Code.Colspan = 6;
                                                                    Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Code);
                                                                }
                                                            }
                                                        }

                                                    }

                                                }

                                            }

                                            if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "d" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "c" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "h" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "f")
                                            {
                                                if (!string.IsNullOrEmpty(NCSCodes.Trim()))
                                                {
                                                    if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() != "h")
                                                    {
                                                        PdfPCell AGY_DESC1 = new PdfPCell(new Phrase("", TableFont));
                                                        AGY_DESC1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        AGY_DESC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(AGY_DESC1);

                                                        PdfPCell Space = new PdfPCell(new Phrase("Non-Cash Benefits", TblFontBoldColor));
                                                        Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        Space.Colspan = 6;
                                                        Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(Space);
                                                    }

                                                    string[] Temp_Codes = NCSCodes.Split(',');

                                                    foreach (AGYTABSEntity drAgency in AgyTabs_NCB)
                                                    {

                                                        if (Temp_Codes != null)
                                                        {
                                                            for (int i = 0; i < Temp_Codes.Length; i++)
                                                            {
                                                                if (drAgency.Table_Code.ToString().Trim() == Temp_Codes[i].Trim())
                                                                {
                                                                    PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Space);

                                                                    PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                                    Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Code.Colspan = 6;
                                                                    Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    table.AddCell(Code);
                                                                }
                                                            }
                                                        }

                                                    }

                                                }

                                            }

                                            PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                            Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Space3.Colspan = 7;
                                            Space3.FixedHeight = 15f;
                                            Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Space3);
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                            {
                                PdfPCell Space = new PdfPCell(new Phrase(" ", TableFont));
                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space);

                                PdfPCell Code = new PdfPCell(new Phrase("Not Assoc  ", TableFont));
                                Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                Code.Colspan = 6;
                                Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Code);
                                string[] tmpCd = NotAgycodes.Split(' ');
                                for (int i = 0; i < tmpCd.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                                    {
                                        if (dr["RNG4CATG_CODE"].ToString() == "N")
                                        {
                                            AGYTABSEntity row = AgyTabs_List.Find(u => u.Table_Code.ToString().Trim().Equals(tmpCd[i].ToString().Trim()));
                                            if (row != null)
                                            {
                                                PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Space1);

                                                PdfPCell Code2 = new PdfPCell(new Phrase("    " + row.Table_Code.ToString().Trim() + "  " + row.Code_Desc.ToString().Trim(), TableFont));
                                                Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Code2.Colspan = 6;
                                                Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Code2);
                                            }
                                        }
                                        else
                                        {
                                            DataRow[] row = dtAgyCodes.Select("Code='" + tmpCd[i].ToString() + "'");
                                            if (row != null && row.Length > 0)
                                            {
                                                PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Space1);

                                                PdfPCell Code2 = new PdfPCell(new Phrase("    " + row[0]["Code"].ToString().Trim() + "  " + row[0]["LookUpDesc"].ToString().Trim(), TableFont));
                                                Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Code2.Colspan = 6;
                                                Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Code2);
                                            }
                                        }
                                    }
                                }
                                PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space3.Colspan = 7;
                                Space3.FixedHeight = 15f;
                                Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space3);
                                First = false;
                            }
                            else if (string.IsNullOrEmpty(AgycodesTemp.Trim()))
                            {
                                PdfPCell Space = new PdfPCell(new Phrase(" ", TableFont));
                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space);

                                PdfPCell Code = new PdfPCell(new Phrase("Not Assoc  ", TableFont));
                                Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                Code.Colspan = 6;
                                Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Code);
                                //string[] tmpCd = NotAgycodes.Split(' ');
                                if (dr["RNG4CATG_CODE"].ToString() == "N")
                                {
                                    if (AgyTabs_List.Count > 0)
                                    {
                                        foreach (AGYTABSEntity row in AgyTabs_List)
                                        {
                                            PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                            Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Space1);

                                            PdfPCell Code2 = new PdfPCell(new Phrase("    " + row.Table_Code.ToString().Trim() + "  " + row.Code_Desc.ToString().Trim(), TableFont));
                                            Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Code2.Colspan = 6;
                                            Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Code2);
                                        }
                                    }
                                }
                                else
                                {
                                    if (dtAgyCodes.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in dtAgyCodes.Rows)
                                        {
                                            PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                            Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Space1);

                                            PdfPCell Code2 = new PdfPCell(new Phrase("    " + row["Code"].ToString().Trim() + "  " + row["LookUpDesc"].ToString().Trim(), TableFont));
                                            Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Code2.Colspan = 6;
                                            Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Code2);
                                        }
                                    }
                                }

                                PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space3.Colspan = 7;
                                Space3.FixedHeight = 15f;
                                Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space3);
                                //PrintRec(" ", 150);
                                First = false;
                            }



                        }
                        document.Add(table);
                    }
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            }
            else if (tab_Page == "RPerfMeasures")
            {
                //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                //document.SetPageSize(iTextSharp.text.PageSize.A4);
                //PdfWriter writer = PdfWriter.GetInstance(document, fs);
                //document.Open();
                //cb = writer.DirectContent;

                try
                {
                    string empty = " ";
                    DataSet ds = DatabaseLayer.SPAdminDB.Get_RNGGRP(gvRdateRange.CurrentRow.Cells["RCode"].Value.ToString(), gvRdateRange.CurrentRow.Cells["gvAgy_Code"].Value.ToString(), null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                    DataTable dt = ds.Tables[0];
                    DataView dvgrp = new DataView(dt);
                    dvgrp.Sort = "RNGGRP_GROUP_CODE";//+string.Empty+"'";
                    dt = dvgrp.ToTable();

                    //DataSet dsCsbRa = DatabaseLayer.SPAdminDB.Get_CSB14RA(gvDateRange.CurrentRow.Cells["From_Date"].Value.ToString(), gvDateRange.CurrentRow.Cells["To_Date"].Value.ToString(), null, null);
                    //DataTable dtCsbRa = dsCsbRa.Tables[0];

                    List<MSMASTEntity> MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

                    if (dt.Rows.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(6);
                        table.TotalWidth = 540f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 15f, 13f, 75f, 6f, 12f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.HeaderRows = 2;

                        PdfPCell Hdr = new PdfPCell(new Phrase("Outcome Indicator Association", TblFontBoldColor));
                        Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                        Hdr.FixedHeight = 15f;
                        Hdr.Colspan = 6;
                        Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Hdr);

                        string Rdesc = gvRdateRange.CurrentRow.Cells["RDesc"].Value.ToString().Trim();
                        PdfPCell ref_fdate = new PdfPCell(new Phrase(Rdesc, TblFontBoldColor));
                        ref_fdate.HorizontalAlignment = Element.ALIGN_CENTER;
                        ref_fdate.Colspan = 6;
                        ref_fdate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(ref_fdate);


                        string UsedGoals = string.Empty;
                        string temp_code = null; string AgyGoal_code = null; int j = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            string Grp_List = null; string group = null;

                            if (!string.IsNullOrEmpty(dr["RNGGRP_group_code"].ToString().Trim()) && string.IsNullOrEmpty(dr["RNGGRP_TABLE_CODE"].ToString().Trim()))
                            {
                                PdfPCell Space_code = new PdfPCell(new Phrase("", TblFontBold));
                                Space_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space_code.Colspan = 6;
                                Space_code.FixedHeight = 10f;
                                Space_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space_code);

                                PdfPCell group_code = new PdfPCell(new Phrase("Domain : " + dr["RNGGRP_group_code"].ToString().Trim(), TblFontBold));
                                group_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                group_code.Colspan = 2;
                                group_code.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(group_code);

                                PdfPCell group_desc = new PdfPCell(new Phrase(dr["RNGGRP_desc"].ToString().Trim(), TblFontBold));
                                group_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                //group_desc.Colspan = 2;
                                group_desc.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(group_desc);

                                PdfPCell group_Space = new PdfPCell(new Phrase("", TableFont));
                                group_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                group_Space.Colspan = 3;
                                group_Space.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(group_Space);


                                //PdfPCell cell1 = new PdfPCell(new Phrase("Headers & Results", TblFontBold));
                                //cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //cell1.FixedHeight = 15f;
                                //cell1.Colspan = 6;
                                //cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(cell1);

                                group = dr["RNGGRP_GROUP_CODE"].ToString();
                                string RefFdate = dr["RNGGRP_CODE"].ToString();
                                string RefTdate = dr["RNGGRP_AGENCY"].ToString();
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr1"].ToString()) && dr["RNGGRP_cnt_incld1"].ToString() == "True")
                                //    Grp_List = "1";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr2"].ToString()) && dr["RNGGRP_cnt_incld2"].ToString() == "True")
                                //    Grp_List += "2";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr3"].ToString()) && dr["RNGGRP_cnt_incld3"].ToString() == "True")
                                //    Grp_List += "3";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr4"].ToString()) && dr["RNGGRP_cnt_incld4"].ToString() == "True")
                                //    Grp_List += "4";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr5"].ToString()) && dr["RNGGRP_cnt_incld5"].ToString() == "True")
                                //    Grp_List += "5";

                                //for (int i = 0; i < Grp_List.Length; i++)
                                //{
                                //    if (Grp_List.Substring(i, 1) == "1")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr1"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else if (Grp_List.Substring(i, 1) == "2")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr2"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else if (Grp_List.Substring(i, 1) == "3")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr3"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else if (Grp_List.Substring(i, 1) == "4")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr4"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr5"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }

                                //    foreach (DataRow drCsbRa in dtCsbRa.Rows)
                                //    {
                                //        if (Convert.ToDateTime(drCsbRa["CSB14RA_REF_FDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_fdate"].ToString()) && Convert.ToDateTime(drCsbRa["CSB14RA_REF_TDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_tdate"].ToString()) && drCsbRa["CSB14RA_GROUP_CODE"].ToString().Trim() == dr["csb14grp_group_code"].ToString().Trim())
                                //        {
                                //            string Grp = Grp_List.Substring(i, 1);
                                //            if (drCsbRa["CSB14RA_COUNT_CODE"].ToString() == Grp_List.Substring(i, 1))
                                //            {
                                //                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                //                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //                //Space1.Colspan = 2;
                                //                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                table.AddCell(Space1);

                                //                PdfPCell RESULT_CODE = new PdfPCell(new Phrase(drCsbRa["CSB14RA_RESULT_CODE"].ToString().Trim(), TableFont));
                                //                RESULT_CODE.HorizontalAlignment = Element.ALIGN_LEFT;
                                //                //RESULT_CODE.Colspan = 2;
                                //                RESULT_CODE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                table.AddCell(RESULT_CODE);

                                //                PdfPCell RESULT_Desc = new PdfPCell(new Phrase(drCsbRa["CSB14RA_DESC"].ToString().Trim(), TableFont));
                                //                RESULT_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                //                RESULT_Desc.Colspan = 4;
                                //                RESULT_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                table.AddCell(RESULT_Desc);
                                //            }
                                //        }
                                //    }
                                //}


                                string Goal = null; string goaldesc = null;
                                DataSet dsGrp = DatabaseLayer.SPAdminDB.Get_RNGGRP(RefFdate, RefTdate, group, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                                DataTable dtGrp = dsGrp.Tables[0];
                                DataView dv = new DataView(dtGrp);
                                dv.Sort = "RNGGRP_TABLE_CODE";//+string.Empty+"'";
                                dtGrp = dv.ToTable();
                                if (dtGrp.Rows.Count > 1)
                                {
                                    PdfPCell TbHeader = new PdfPCell(new Phrase("Table/Goals", TblFontBold));
                                    TbHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TbHeader.FixedHeight = 15f;
                                    TbHeader.Colspan = 2;
                                    TbHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(TbHeader);

                                    string[] Header = { "Description", "Ind", "Budget", "CC" };
                                    for (int i = 0; i < Header.Length; ++i)
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.FixedHeight = 15f;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                }
                                foreach (DataRow drGrp in dtGrp.Rows)
                                {
                                    if (!string.IsNullOrWhiteSpace(drGrp["RNGGRP_TABLE_CODE"].ToString()))
                                    {
                                        temp_code = drGrp["RNGGRP_goal_codes"].ToString();
                                        List<RNGGAEntity> GoalEntity = _model.SPAdminData.Browse_RNGGA(RefFdate, RefTdate, group, drGrp["RNGGRP_TABLE_CODE"].ToString(), null);

                                        if (!string.IsNullOrEmpty(drGrp["RNGGRP_table_code"].ToString()))
                                        {
                                            PdfPCell table_code = new PdfPCell(new Phrase(drGrp["RNGGRP_table_code"].ToString().Trim(), TableFont));
                                            table_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                            table_code.Colspan = 2;
                                            table_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(table_code);

                                            PdfPCell desc = new PdfPCell(new Phrase(drGrp["RNGGRP_desc"].ToString().Trim(), TableFont));
                                            desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            desc.FixedHeight = 12f;
                                            desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(desc);

                                            PdfPCell cnt_indicator = new PdfPCell(new Phrase(drGrp["RNGGRP_cnt_indicator"].ToString().Trim(), TableFont));
                                            cnt_indicator.HorizontalAlignment = Element.ALIGN_CENTER;
                                            cnt_indicator.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(cnt_indicator);

                                            PdfPCell achieve = new PdfPCell(new Phrase(drGrp["RNGGRP_expect_achieve"].ToString().Trim(), TableFont));
                                            achieve.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            achieve.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(achieve);

                                            PdfPCell calc_cost = new PdfPCell(new Phrase(drGrp["RNGGRP_calc_cost"].ToString().Trim(), TableFont));
                                            calc_cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            calc_cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(calc_cost);

                                            if (GoalEntity.Count > 0)
                                            {
                                                foreach (RNGGAEntity GEntity in GoalEntity)
                                                {
                                                    foreach (MSMASTEntity Entity in MSList)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(GEntity.GoalCode.Trim()))
                                                        {
                                                            if (Entity.Code.ToString().Trim() == GEntity.GoalCode.Trim())
                                                            {
                                                                Goal = Entity.Code;
                                                                goaldesc = Entity.Desc;
                                                                UsedGoals += Entity.Code.Trim() + " ";
                                                                //Entity.Sel_SW = true;
                                                                //Ststus_Exists = Entity.Sel_WS;
                                                                PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                                                Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                //Space1.Colspan = 3;
                                                                Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(Space2);

                                                                PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                                                stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(stone_code);

                                                                PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                                                stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                stone_desc.Colspan = 2;
                                                                stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(stone_desc);

                                                                PdfPCell achieve1 = new PdfPCell(new Phrase(GEntity.Budget.Trim(), TableFont));
                                                                achieve1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                                achieve1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(achieve1);

                                                                PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                                                Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(Space3);

                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                            }


                                        }
                                    }
                                }

                                if (table.Rows.Count > 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }
                            }

                        }

                        string NotUsedGoals = string.Empty;
                        foreach (MSMASTEntity Entity in MSList)
                        {
                            NotUsedGoals += Entity.Code.Trim() + " ";
                        }

                        if (!string.IsNullOrEmpty(UsedGoals.Trim()))
                        {

                            string[] tmpCd = UsedGoals.Split(' ');
                            for (int i = 0; i < tmpCd.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                                {

                                    if (NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                        NotUsedGoals = NotUsedGoals.Replace(tmpCd[i].ToString().Trim(), "");

                                    //foreach (MSMASTEntity Entity in MSList)
                                    //{
                                    //    if (Entity.Code.Trim() != tmpCd[i].ToString().Trim())
                                    //    {
                                    //        if (string.IsNullOrEmpty(NotUsedGoals.Trim()))
                                    //        {
                                    //            NotUsedGoals = Entity.Code.Trim() + " ";

                                    //        }
                                    //        else
                                    //        {
                                    //            if (NotUsedGoals.Contains(Entity.Code.Trim()))
                                    //            {
                                    //                //NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                    //            }
                                    //            else
                                    //            {
                                    //                NotUsedGoals += Entity.Code.Trim() + " ";
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if(NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                    //            NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                    //    }
                                    //}
                                }
                            }

                            if (!string.IsNullOrEmpty(NotUsedGoals.Trim()))
                            {
                                document.NewPage();

                                PdfPTable table1 = new PdfPTable(2);
                                table1.TotalWidth = 500f;
                                table1.WidthPercentage = 100;
                                table1.LockedWidth = true;
                                float[] widths1 = new float[] { 25f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                table1.SetWidths(widths1);
                                table1.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell Header = new PdfPCell(new Phrase("List of Unassociated Goals", TblFontBoldColor));
                                Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header.FixedHeight = 15f;
                                Header.Colspan = 2;
                                Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(Header);

                                PdfPCell stone_head = new PdfPCell(new Phrase("MS Code", TblFontBold));
                                stone_head.HorizontalAlignment = Element.ALIGN_LEFT;
                                stone_head.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(stone_head);

                                PdfPCell stone_descH = new PdfPCell(new Phrase("Description", TblFontBold));
                                stone_descH.HorizontalAlignment = Element.ALIGN_LEFT;
                                //stone_desc.Colspan = 4;
                                stone_descH.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(stone_descH);

                                string[] tmpCd1 = NotUsedGoals.Split(' ');
                                Array.Sort(tmpCd1);
                                for (int i = 0; i < tmpCd1.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(tmpCd1[i].ToString()))
                                    {
                                        foreach (MSMASTEntity Entity in MSList)
                                        {
                                            if (Entity.Code.Trim() == tmpCd1[i].ToString().Trim())
                                            {
                                                PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                                stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table1.AddCell(stone_code);

                                                PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                                stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                //stone_desc.Colspan = 4;
                                                stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table1.AddCell(stone_desc);
                                                break;
                                            }
                                        }
                                    }
                                }
                                document.Add(table1);
                            }

                        }
                        //document.Add(table);
                    }

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }


            }
            else if (tab_Page == "RServices")
            {
                //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                //document.SetPageSize(iTextSharp.text.PageSize.A4);
                //PdfWriter writer = PdfWriter.GetInstance(document, fs);
                //document.Open();
                //cb = writer.DirectContent;

                try
                {
                    string empty = " ";
                    DataSet ds = DatabaseLayer.SPAdminDB.Get_RNGSRGRP(gvSRdateRange.CurrentRow.Cells["SRCode"].Value.ToString(), gvSRdateRange.CurrentRow.Cells["gvSRAgy_Code"].Value.ToString(), null, null, null);
                    DataTable dt = ds.Tables[0];
                    DataView dvgrp = new DataView(dt);
                    dvgrp.Sort = "RNGSRGRP_GROUP_CODE";//+string.Empty+"'";
                    dt = dvgrp.ToTable();

                    //DataSet dsCsbRa = DatabaseLayer.SPAdminDB.Get_CSB14RA(gvDateRange.CurrentRow.Cells["From_Date"].Value.ToString(), gvDateRange.CurrentRow.Cells["To_Date"].Value.ToString(), null, null);
                    //DataTable dtCsbRa = dsCsbRa.Tables[0];

                    List<CAMASTEntity> CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);

                    if (dt.Rows.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(6);
                        table.TotalWidth = 540f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 15f, 13f, 75f, 6f, 12f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.HeaderRows = 2;

                        PdfPCell Hdr = new PdfPCell(new Phrase("Services Association", TblFontBoldColor));
                        Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                        Hdr.FixedHeight = 15f;
                        Hdr.Colspan = 6;
                        Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Hdr);

                        string SRDesc = gvSRdateRange.CurrentRow.Cells["SRDesc"].Value.ToString().Trim();
                        PdfPCell ref_fdate = new PdfPCell(new Phrase(SRDesc, TblFontBoldColor));
                        ref_fdate.HorizontalAlignment = Element.ALIGN_CENTER;
                        ref_fdate.Colspan = 6;
                        ref_fdate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(ref_fdate);


                        string UsedGoals = string.Empty;
                        string temp_code = null; string AgyGoal_code = null; int j = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            string Grp_List = null; string group = null;

                            if (!string.IsNullOrEmpty(dr["RNGSRgrp_group_code"].ToString().Trim()) && string.IsNullOrEmpty(dr["RNGSRgrp_table_code"].ToString().Trim()))
                            {
                                PdfPCell Space_code = new PdfPCell(new Phrase("", TblFontBold));
                                Space_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space_code.Colspan = 6;
                                Space_code.FixedHeight = 10f;
                                Space_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space_code);

                                PdfPCell group_code = new PdfPCell(new Phrase("Group : " + dr["RNGSRgrp_group_code"].ToString().Trim(), TblFontBold));
                                group_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                group_code.Colspan = 2;
                                group_code.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(group_code);

                                PdfPCell group_desc = new PdfPCell(new Phrase(dr["RNGSRgrp_desc"].ToString().Trim(), TblFontBold));
                                group_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                //group_desc.Colspan = 2;
                                group_desc.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(group_desc);

                                PdfPCell group_Space = new PdfPCell(new Phrase("", TableFont));
                                group_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                group_Space.Colspan = 3;
                                group_Space.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(group_Space);


                                //PdfPCell cell1 = new PdfPCell(new Phrase("Headers & Results", TblFontBold));
                                //cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //cell1.FixedHeight = 15f;
                                //cell1.Colspan = 6;
                                //cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(cell1);

                                group = dr["RNGSRgrp_group_code"].ToString();
                                string RefFdate = dr["RNGSRGRP_CODE"].ToString();
                                string RefTdate = dr["RNGSRGRP_AGENCY"].ToString();
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr1"].ToString()) && dr["RNGGRP_cnt_incld1"].ToString() == "True")
                                //    Grp_List = "1";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr2"].ToString()) && dr["RNGGRP_cnt_incld2"].ToString() == "True")
                                //    Grp_List += "2";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr3"].ToString()) && dr["RNGGRP_cnt_incld3"].ToString() == "True")
                                //    Grp_List += "3";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr4"].ToString()) && dr["RNGGRP_cnt_incld4"].ToString() == "True")
                                //    Grp_List += "4";
                                //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr5"].ToString()) && dr["RNGGRP_cnt_incld5"].ToString() == "True")
                                //    Grp_List += "5";

                                //for (int i = 0; i < Grp_List.Length; i++)
                                //{
                                //    if (Grp_List.Substring(i, 1) == "1")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr1"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else if (Grp_List.Substring(i, 1) == "2")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr2"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else if (Grp_List.Substring(i, 1) == "3")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr3"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else if (Grp_List.Substring(i, 1) == "4")
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr4"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }
                                //    else
                                //    {
                                //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr5"].ToString().Trim(), TableFont));
                                //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //        hdr1.Colspan = 6;
                                //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        table.AddCell(hdr1);
                                //    }

                                //    foreach (DataRow drCsbRa in dtCsbRa.Rows)
                                //    {
                                //        if (Convert.ToDateTime(drCsbRa["CSB14RA_REF_FDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_fdate"].ToString()) && Convert.ToDateTime(drCsbRa["CSB14RA_REF_TDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_tdate"].ToString()) && drCsbRa["CSB14RA_GROUP_CODE"].ToString().Trim() == dr["csb14grp_group_code"].ToString().Trim())
                                //        {
                                //            string Grp = Grp_List.Substring(i, 1);
                                //            if (drCsbRa["CSB14RA_COUNT_CODE"].ToString() == Grp_List.Substring(i, 1))
                                //            {
                                //                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                //                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //                //Space1.Colspan = 2;
                                //                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                table.AddCell(Space1);

                                //                PdfPCell RESULT_CODE = new PdfPCell(new Phrase(drCsbRa["CSB14RA_RESULT_CODE"].ToString().Trim(), TableFont));
                                //                RESULT_CODE.HorizontalAlignment = Element.ALIGN_LEFT;
                                //                //RESULT_CODE.Colspan = 2;
                                //                RESULT_CODE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                table.AddCell(RESULT_CODE);

                                //                PdfPCell RESULT_Desc = new PdfPCell(new Phrase(drCsbRa["CSB14RA_DESC"].ToString().Trim(), TableFont));
                                //                RESULT_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                //                RESULT_Desc.Colspan = 4;
                                //                RESULT_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                table.AddCell(RESULT_Desc);
                                //            }
                                //        }
                                //    }
                                //}


                                string Goal = null; string goaldesc = null;
                                DataSet dsGrp = DatabaseLayer.SPAdminDB.Get_RNGSRGRP(RefFdate, RefTdate, group, null, null);
                                DataTable dtGrp = dsGrp.Tables[0];
                                DataView dv = new DataView(dtGrp);
                                dv.Sort = "RNGSRgrp_table_code";//+string.Empty+"'";
                                dtGrp = dv.ToTable();
                                if (dtGrp.Rows.Count > 1)
                                {
                                    PdfPCell TbHeader = new PdfPCell(new Phrase("Table/Services", TblFontBold));
                                    TbHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TbHeader.FixedHeight = 15f;
                                    TbHeader.Colspan = 2;
                                    TbHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(TbHeader);

                                    string[] Header = { "Description", "Ind", "Budget", "CC" };
                                    for (int i = 0; i < Header.Length; ++i)
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.FixedHeight = 15f;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                }
                                foreach (DataRow drGrp in dtGrp.Rows)
                                {
                                    if (!string.IsNullOrWhiteSpace(drGrp["RNGSRgrp_table_code"].ToString()))
                                    {
                                        //temp_code = drGrp["RNGSRGRP_goal_codes"].ToString();
                                        List<RNGSRGAEntity> GoalEntity = _model.SPAdminData.Browse_RNGSRGA(RefFdate, RefTdate, group, drGrp["RNGSRgrp_table_code"].ToString(), null);

                                        if (!string.IsNullOrEmpty(drGrp["RNGSRgrp_table_code"].ToString()))
                                        {
                                            PdfPCell table_code = new PdfPCell(new Phrase(drGrp["RNGSRgrp_table_code"].ToString().Trim(), TableFont));
                                            table_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                            table_code.Colspan = 2;
                                            table_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(table_code);

                                            PdfPCell desc = new PdfPCell(new Phrase(drGrp["RNGSRgrp_desc"].ToString().Trim(), TableFont));
                                            desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            desc.FixedHeight = 12f;
                                            desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(desc);

                                            PdfPCell cnt_indicator = new PdfPCell(new Phrase(drGrp["RNGSRgrp_cnt_indicator"].ToString().Trim(), TableFont));
                                            cnt_indicator.HorizontalAlignment = Element.ALIGN_CENTER;
                                            cnt_indicator.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(cnt_indicator);

                                            PdfPCell achieve = new PdfPCell(new Phrase(drGrp["RNGSRgrp_expect_achieve"].ToString().Trim(), TableFont));
                                            achieve.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            achieve.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(achieve);

                                            PdfPCell calc_cost = new PdfPCell(new Phrase(drGrp["RNGSRgrp_calc_cost"].ToString().Trim(), TableFont));
                                            calc_cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            calc_cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(calc_cost);

                                            if (GoalEntity.Count > 0)
                                            {
                                                foreach (RNGSRGAEntity GEntity in GoalEntity)
                                                {
                                                    foreach (CAMASTEntity Entity in CAList)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(GEntity.GoalCode.Trim()))
                                                        {
                                                            if (Entity.Code.ToString().Trim() == GEntity.GoalCode.Trim())
                                                            {
                                                                Goal = Entity.Code;
                                                                goaldesc = Entity.Desc;
                                                                UsedGoals += Entity.Code.Trim() + " ";
                                                                //Entity.Sel_SW = true;
                                                                //Ststus_Exists = Entity.Sel_WS;
                                                                PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                                                Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                //Space1.Colspan = 3;
                                                                Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(Space2);

                                                                PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                                                stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(stone_code);

                                                                PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                                                stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                stone_desc.Colspan = 2;
                                                                stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(stone_desc);

                                                                PdfPCell achieve1 = new PdfPCell(new Phrase(GEntity.Budget.Trim(), TableFont));
                                                                achieve1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                                achieve1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(achieve1);

                                                                PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                                                Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                table.AddCell(Space3);

                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                            }


                                        }
                                    }
                                }

                                if (table.Rows.Count > 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }
                            }

                        }

                        string NotUsedGoals = string.Empty;
                        foreach (CAMASTEntity Entity in CAList)
                        {
                            NotUsedGoals += Entity.Code.Trim() + " ";
                        }

                        if (!string.IsNullOrEmpty(UsedGoals.Trim()))
                        {

                            string[] tmpCd = UsedGoals.Split(' ');
                            for (int i = 0; i < tmpCd.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                                {

                                    if (NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                        NotUsedGoals = NotUsedGoals.Replace(tmpCd[i].ToString().Trim(), "");

                                    //foreach (MSMASTEntity Entity in MSList)
                                    //{
                                    //    if (Entity.Code.Trim() != tmpCd[i].ToString().Trim())
                                    //    {
                                    //        if (string.IsNullOrEmpty(NotUsedGoals.Trim()))
                                    //        {
                                    //            NotUsedGoals = Entity.Code.Trim() + " ";

                                    //        }
                                    //        else
                                    //        {
                                    //            if (NotUsedGoals.Contains(Entity.Code.Trim()))
                                    //            {
                                    //                //NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                    //            }
                                    //            else
                                    //            {
                                    //                NotUsedGoals += Entity.Code.Trim() + " ";
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if(NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                    //            NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                    //    }
                                    //}
                                }
                            }

                            if (!string.IsNullOrEmpty(NotUsedGoals.Trim()))
                            {
                                document.NewPage();

                                PdfPTable table1 = new PdfPTable(2);
                                table1.TotalWidth = 500f;
                                table1.WidthPercentage = 100;
                                table1.LockedWidth = true;
                                float[] widths1 = new float[] { 25f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                table1.SetWidths(widths1);
                                table1.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell Header = new PdfPCell(new Phrase("List of Unassociated Services", TblFontBoldColor));
                                Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header.FixedHeight = 15f;
                                Header.Colspan = 2;
                                Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(Header);

                                PdfPCell stone_head = new PdfPCell(new Phrase("CA Code", TblFontBold));
                                stone_head.HorizontalAlignment = Element.ALIGN_LEFT;
                                stone_head.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(stone_head);

                                PdfPCell stone_descH = new PdfPCell(new Phrase("Description", TblFontBold));
                                stone_descH.HorizontalAlignment = Element.ALIGN_LEFT;
                                //stone_desc.Colspan = 4;
                                stone_descH.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(stone_descH);

                                string[] tmpCd1 = NotUsedGoals.Split(' ');
                                Array.Sort(tmpCd1);
                                for (int i = 0; i < tmpCd1.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(tmpCd1[i].ToString()))
                                    {
                                        foreach (CAMASTEntity Entity in CAList)
                                        {
                                            if (Entity.Code.Trim() == tmpCd1[i].ToString().Trim())
                                            {
                                                PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                                stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table1.AddCell(stone_code);

                                                PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                                stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                //stone_desc.Colspan = 4;
                                                stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table1.AddCell(stone_desc);
                                                break;
                                            }
                                        }
                                    }
                                }
                                document.Add(table1);
                            }

                        }
                        //document.Add(table);
                    }

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }


            }

            document.Close();
            fs.Close();
            fs.Dispose();
            pageNumber = 1;

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            //}
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

    }
}