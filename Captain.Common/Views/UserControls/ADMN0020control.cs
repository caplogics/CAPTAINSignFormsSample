#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
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
using CarlosAg.ExcelXmlWriter;
using Captain.Common.Views.Controls.Compatibility;
using System.Reflection.Emit;
using DevExpress.XtraCharts.Native;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ADMN0020control : BaseUserControl
    {

        #region private variables

        private CaptainModel _model = null;
        private PrivilegesControl _screenPrivileges = null;

        #endregion

        public ADMN0020control(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            propReportPath = _model.lookupDataAccess.GetReportPath();

            DataSet dsmem = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsmem != null && dsmem.Tables[0].Rows.Count > 0)
                Member_Activity = dsmem.Tables[0].Rows[0]["ACR_MEM_ACTIVTY"].ToString().Trim();

            if (Member_Activity == "Y") { this.Category.Visible = true; this.PlanDescription.Width = 397; } 
            else this.Category.Visible = false;
            
            caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("DEPT", BaseForm.BaseAdminAgency, string.Empty, string.Empty, BaseForm.BaseAdminAgency == "**" ? string.Empty : BaseForm.BaseAdminAgency);
            
            FillBranchCombo();

            Txt_SPCode.Validator = TextBoxValidation.IntegerValidator;
            GetAgencyDetails();
            btnSearch_Click(btnSearch, EventArgs.Empty);

            PopulateToolbar(oToolbarMnustrip);
            
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarPrint { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }

        //public ToolBarButton ToolBarCaseNotes { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }
        public string propReportPath { get; set; }

        public DataSet dsAgency { get; set; }

        public DataTable dtAgency { get; set; }

        public string Member_Activity { get; set; }



        //public List<CaseNotesEntity> caseNotesEntity { get; set; }

        #endregion

        List<HierarchyEntity> caseHierarchy = new List<HierarchyEntity>();

        string SPOut_Err_Procedure, SPOut_Err_Number, SPOut_Err_Message, SPOut_Err_Line;

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
                ToolBarNew.ToolTipText = "Add Service Plan";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Service Plan";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Service Plan";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarPrint = new ToolBarButton();
                ToolBarPrint.Tag = "Print";
                ToolBarPrint.ToolTipText = "Print Service Plan Report";
                ToolBarPrint.Enabled = true;
                ToolBarPrint.ImageSource = "captain-print";
                ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarExcel = new ToolBarButton();
                //ToolBarExcel.Tag = "Excel";
                //ToolBarExcel.ToolTipText = "Service Plan Report in Excel";
                //ToolBarExcel.Enabled = true;
                //ToolBarExcel.ImageSource = "captain-excel";
                //ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarCaseNotes = new ToolBarButton();
                //ToolBarCaseNotes.Tag = "CaseNotes";
                //ToolBarCaseNotes.ToolTipText = "Note/ISP button";
                //ToolBarCaseNotes.Enabled = true;
                //ToolBarCaseNotes.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarCaseNotes.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Service Plan Help";
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

            //ShowCaseNotesImages();

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                ToolBarPrint,
               // ToolBarExcel,
                //ToolBarCaseNotes,
                ToolBarHelp
            });
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
                    // BaseForm baseForm, string mode, string hierarchy,string year, string hieDesc, string site, string schDate, string type,
                    case Consts.ToolbarActions.New:
                        if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                        {
                            ADMN1020Form ADMNB0020_Add = new ADMN1020Form(BaseForm, "Add", "New SP", Privileges);  //10222012
                            ADMNB0020_Add.FormClosed += new FormClosedEventHandler(SP_AddForm_Closed1);
                            ADMNB0020_Add.StartPosition= FormStartPosition.CenterScreen;
                            ADMNB0020_Add.ShowDialog();
                        }
                        else
                        {
                            ADMN0020Form ADMNB0020_Add = new ADMN0020Form(BaseForm, "Add", "New SP", Privileges);  //10222012
                            ADMNB0020_Add.FormClosed += new FormClosedEventHandler(SP_AddForm_Closed);
                            ADMNB0020_Add.StartPosition = FormStartPosition.CenterScreen;
                            ADMNB0020_Add.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Edit:
                        if (SPGrid.Rows.Count > 0)
                        {
                            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                            {
                                ADMN1020Form ADMNB0020_Edit = new ADMN1020Form(BaseForm, "Edit", SPGrid.SelectedRows[0].Cells["Code"].Value.ToString(), Privileges);
                                ADMNB0020_Edit.FormClosed += new FormClosedEventHandler(SP_AddForm_Closed1);
                                ADMNB0020_Edit.StartPosition = FormStartPosition.CenterScreen;
                                ADMNB0020_Edit.ShowDialog();
                            }
                            else
                            {
                                ADMN0020Form ADMNB0020_Edit = new ADMN0020Form(BaseForm, "Edit", SPGrid.SelectedRows[0].Cells["Code"].Value.ToString(), Privileges);
                                ADMNB0020_Edit.FormClosed += new FormClosedEventHandler(SP_AddForm_Closed);
                                ADMNB0020_Edit.StartPosition = FormStartPosition.CenterScreen;
                                ADMNB0020_Edit.ShowDialog();
                            }
                        }
                        break;
                    case Consts.ToolbarActions.Delete:

                        // TMS00110_Delete = new (BaseForm, "Delete", MainMenuHierarchy, MainMenuYear, Pass_Site, Pass_Date, Privileges); //string site, string schDate, 
                        //TMS00110_Delete.ShowDialog();
                        if (SPGrid.Rows.Count > 0) //(CanDeleteHeaderCode())
                        {

                            if (CanDeleteSP())
                            {
                                if (DeleteSPMCheck())
                                {
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nService Plan: " + SPGrid.CurrentRow.Cells["Code"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);
                                }
                                else
                                {
                                    AlertBox.Show("You cannot Delete Service Plan: '" + SPGrid.CurrentRow.Cells["Code"].Value.ToString() + "' \n Service plan exist for some applicants", MessageBoxIcon.Warning);
                                }
                            }
                            else
                                AlertBox.Show("You cannot Delete Service Plan: '" + SPGrid.CurrentRow.Cells["Code"].Value.ToString() + "' \n Service/Outcome(s) are Associated with this Service Plan", MessageBoxIcon.Warning);
                        }
                        //else
                        //    MessageBox.Show("Please Select 'Site' \n To Delete Appointment", "CAP Systems", MessageBoxButtons.OK);
                        break;
                    case Consts.ToolbarActions.Print:
                        if (SPGrid.Rows.Count > 0)
                        {
                            ADMN20PDF PDFForm = new ADMN20PDF(BaseForm, Privileges, SPGrid.CurrentRow.Cells["Code"].Value.ToString(), SPGrid.CurrentRow.Cells["PlanDescription"].Value.ToString(),Member_Activity);
                            PDFForm.StartPosition = FormStartPosition.CenterParent;
                            PDFForm.ShowDialog();
                        }
                        //   On_SaveForm_Closed();
                        break;
                    case Consts.ToolbarActions.Excel:
                        if (SPGrid.Rows.Count > 0)
                            On_SaveExcelForm_Closed();
                        break;
                    //case Consts.ToolbarActions.CaseNotes:
                    //        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, SPGrid.CurrentRow.Cells["Code"].Value.ToString());
                    //        CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, SPGrid.CurrentRow.Cells["Code"].Value.ToString(), SPGrid.CurrentRow.Cells["PlanDescription"].Value.ToString(),string.Empty);
                    //        caseNotes.FormClosed += new Form.FormClosedEventHandler(OnCaseNotesFormClosed);
                    //        caseNotes.ShowDialog();
                    //    break;
                    case Consts.ToolbarActions.Help:
                        if(BtnValidate.Visible == true)
                            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 3, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        else
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

        private void GetAgencyDetails()
        {
            dsAgency = DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(BaseForm.BaseAgency, null, null, null, null, null, null);
            dtAgency = dsAgency.Tables[0];
        }


        //public string GetSelectedRow()
        //{
        //    string salId = null;
        //    if (SPGrid != null)
        //    {
        //        foreach (DataGridViewRow dr in SPGrid.SelectedRows)
        //        {
        //            if (dr.Selected)
        //            {
        //                strIndex = SPGrid.SelectedRows[0].Index;
        //                // strPageIndex = gvwServices.CurrentPage;
        //                SaldefEntity Saldefdetails = dr.Tag as SaldefEntity;
        //                if (Saldefdetails != null)
        //                {
        //                    salId = Saldefdetails.SALD_ID;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return salId;
        //}

        private bool CanDeleteSP()
        {
            bool can_delete = true;
            DataSet ds_Hie = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP2(SPGrid.CurrentRow.Cells["Code"].Value.ToString(), null, null, null);
            DataTable dt_Hie = new DataTable();
            if (ds_Hie.Tables.Count > 0)
            {
                if (ds_Hie.Tables[0].Rows.Count > 0)
                    can_delete = false;
            }

            return can_delete;
        }


        private bool DeleteSPMCheck()
        {
            bool can_delete = true;
            CASESPMEntity Search_Entity = new CASESPMEntity();
            Search_Entity.agency = null;
            Search_Entity.dept = null;
            Search_Entity.program = null;
            //Search_Entity.year = Year;
            Search_Entity.year = null;                          // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = "DELSPM";

            Search_Entity.service_plan = SPGrid.CurrentRow.Cells["Code"].Value.ToString();

            Search_Entity.caseworker = Search_Entity.site = null;
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = Search_Entity.Def_Program =
            Search_Entity.Bulk_Post = null;

            List<CASESPMEntity> CASESPM_SP_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse",string.Empty);
            if (CASESPM_SP_List.Count > 0)
            {
                can_delete = false;
            }
            return can_delete;
        }

        //private void ShowCaseNotesImages()
        //{
        //    string strYear = "    ";
        //    if (!string.IsNullOrEmpty(BaseForm.BaseYear))
        //    {
        //        strYear = BaseForm.BaseYear;
        //    }
        //    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, SPGrid.CurrentRow.Cells["Code"].Value.ToString());
        //    if (ToolBarCaseNotes != null)
        //    {
        //        if (caseNotesEntity.Count > 0)
        //        {
        //            ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_View;
        //        }
        //        else
        //        {
        //            ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_New;
        //        }
        //    }
        //    if (!(SPGrid.Rows.Count > 0)) ToolBarCaseNotes.Enabled = false; else ToolBarCaseNotes.Enabled = true;

        //}

        //private void OnCaseNotesFormClosed(object sender, FormClosedEventArgs e)
        //{
        //    CaseNotes form = sender as CaseNotes;

        //    //if (form.DialogResult == DialogResult.OK)
        //    //{
        //    string strYear = "    ";
        //    if (!string.IsNullOrEmpty(BaseForm.BaseYear))
        //    {
        //        strYear = BaseForm.BaseYear;
        //    }
        //    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, SPGrid.CurrentRow.Cells["Code"].Value.ToString());
        //    if (caseNotesEntity.Count > 0)
        //    {
        //        ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_View;
        //    }
        //    else
        //    {
        //        ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_New;
        //    }
        //    caseNotesEntity = caseNotesEntity;

        //    //}
        //}



        private void OnDeleteMessageBoxClicked(DialogResult dialogresult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogresult == DialogResult.Yes)
            {
                if (DatabaseLayer.SPAdminDB.Delete_CASESP0(SPGrid.CurrentRow.Cells["Code"].Value.ToString()))
                {
                    //**MessageBox.Show("Service Plan '" + SPGrid.CurrentRow.Cells["Code"].Value.ToString() + "' Deleted Successfully", "CAP Systems", MessageBoxButtons.OK);
                    AlertBox.Show("Service Plan '" + SPGrid.CurrentRow.Cells["Code"].Value.ToString() + "' Deleted Successfully");
                    btnSearch_Click(btnSearch, EventArgs.Empty);
                }
                else
                    AlertBox.Show("Please Select 'Site' to Delete Appointment", MessageBoxIcon.Warning);
            }
        }

        //int SP_Grid_Rows_Count = 0;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SPGrid.SelectionChanged -= new System.EventHandler(this.SPGrid_SelectionChanged);
            SPGrid.Rows.Clear();
            //SP_Grid_Rows_Count = 0;
            Btn_Update_Grps.Visible = BtnValidate.Visible = false;

            string Tmp_Br_Code = null, Tmp_Add_Date = null, Tmp_Lstc_Date = null, Tmp_Status = string.Empty;

            if (((Captain.Common.Utilities.ListItem)cmbBranch.SelectedItem).Value.ToString() != "0")
                Tmp_Br_Code = ((Captain.Common.Utilities.ListItem)cmbBranch.SelectedItem).Value.ToString();

            Tmp_Status = ((Captain.Common.Utilities.ListItem)Cmb_Status.SelectedItem).Value.ToString();
            if (Tmp_Status == "0")
                Tmp_Status = null;

            //if (LstcDate.Checked)
            //    Tmp_Lstc_Date = LstcDate.Value.ToShortDateString();
            //if (AddDate.Checked)
            //    Tmp_Add_Date = AddDate.Value.ToShortDateString();

            DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP0(Txt_SPCode.Text, TxtSPDesc.Text, Tmp_Status, Tmp_Br_Code, Valid_SW, null, null, null, null);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];
            int TmpCount = 0, REfresh_Sel_Index = 0;

            List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();
            SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, null, null, null);

            if (dt.Rows.Count > 0)
            {
                string Tmp_SPCoce = null, Lstc_Date = null, Add_Date = null;
                foreach (DataRow dr in dt.Rows)
                {
                    int rowIndex = 0;
                    Tmp_SPCoce = dr["sp0_servicecode"].ToString();
                    Tmp_SPCoce = "000000".Substring(0, (6 - Tmp_SPCoce.Length)) + Tmp_SPCoce;

                    Add_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dr["sp0_date_add"].ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    Lstc_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dr["sp0_date_lstc"].ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    //rowIndex = SPGrid.Rows.Add(Tmp_SPCoce, dr["sp0_description"].ToString(), Convert.ToDateTime(dr["sp0_date_add"].ToString()).ToShortDateString(), Convert.ToDateTime(dr["sp0_date_lstc"].ToString()).ToShortDateString(), dr["sp0_validated"].ToString());
                    CASESP1Entity SP1Entity = new CASESP1Entity();
                    if(BaseForm.BaseAdminAgency != "**")
                        SP1Entity = SP_Hierarchies.Find(u => u.Code == dr["sp0_servicecode"].ToString() && u.Agency == BaseForm.BaseAdminAgency);
                    else
                        SP1Entity = SP_Hierarchies.Find(u => u.Code == dr["sp0_servicecode"].ToString());

                    if (SP1Entity!=null || BaseForm.BaseAdminAgency=="**")
                    {
                        string Dept = string.Empty;
                        if (caseHierarchy.Count > 0)
                        {
                            if (SP1Entity != null)
                            {
                                HierarchyEntity HieEnt = caseHierarchy.Find(u => u.Agency == SP1Entity.Agency && u.Dept == SP1Entity.Dept && u.Prog == string.Empty);
                                if (HieEnt != null) Dept = HieEnt.HirarchyName;
                            }
                        }

                        if (Member_Activity == "Y")
                        {
                            string Category = string.Empty;
                            if (dr["SP0_Category"].ToString() != "0") Category = dr["SP0_Category"].ToString();
                            rowIndex = SPGrid.Rows.Add(Tmp_SPCoce, dr["sp0_description"].ToString(),Dept, Category, Add_Date, Lstc_Date, dr["sp0_validated"].ToString());
                        }
                        else
                            rowIndex = SPGrid.Rows.Add(Tmp_SPCoce, dr["sp0_description"].ToString(),Dept, string.Empty, Add_Date, Lstc_Date, dr["sp0_validated"].ToString());

                        set_SPGridTooltip(rowIndex, dr);

                        if (!string.IsNullOrEmpty(Refresh_SP_Code.Trim()))
                        {
                            if (Refresh_SP_Code == Tmp_SPCoce)
                                REfresh_Sel_Index = TmpCount;
                        }

                        if (dr["sp0_validated"].ToString() == "N" || dr["sp0_validated"].ToString() == "n")
                            SPGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue; //Color.Peru; //Color.DarkTurquoise;

                        if (dr["SP0_ACTIVE"].ToString() == "N")
                            SPGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red; //Color.Peru; //Color.DarkTurquoise;

                        TmpCount++;

                    }
                    
                }
                if (TmpCount > 0)
                {
                    
                    if (SPGrid.Rows.Count > REfresh_Sel_Index)
                    {
                        // SPGrid.Rows[REfresh_Sel_Index].Selected = true;
                        SPGrid.CurrentCell = SPGrid.Rows[REfresh_Sel_Index].Cells[1];

                        int scrollPosition = 0;
                        scrollPosition = SPGrid.CurrentCell.RowIndex;
                        //int CurrentPage = (scrollPosition / SPGrid.ItemsPerPage);
                        //CurrentPage++;
                        //SPGrid.CurrentPage = CurrentPage;
                        //SPGrid.FirstDisplayedScrollingRowIndex = scrollPosition;
                    }
                    else
                    {
                        // SPGrid.Rows[0].Selected = true;
                        SPGrid.CurrentCell = SPGrid.Rows[0].Cells[1];

                        int scrollPosition = 0;
                        scrollPosition = SPGrid.CurrentCell.RowIndex;
                        //int CurrentPage = (scrollPosition / SPGrid.ItemsPerPage);
                        //CurrentPage++;
                        //SPGrid.CurrentPage = CurrentPage;
                        //SPGrid.FirstDisplayedScrollingRowIndex = scrollPosition;

                    }
                    
                   
                    Btn_Update_Grps.Visible = false;
                    if (SPGrid.CurrentRow.Cells["ValidSW"].Value != null)
                    {
                        if (SPGrid.CurrentRow.Cells["ValidSW"].Value.ToString() == "N")
                            BtnValidate.Visible = true;
                        else if (BaseForm.UserID == "JAKE")
                            Btn_Update_Grps.Visible = true;
                    }
                    else if (BaseForm.UserID == "JAKE")
                        Btn_Update_Grps.Visible = true;

                    Refresh_SP_Code = string.Empty; REfresh_Sel_Index = 0;
                }
            }
            else
                AlertBox.Show("Records does not exist with the selected Search Criteria", MessageBoxIcon.Warning);
            SPGrid.SelectionChanged += new System.EventHandler(this.SPGrid_SelectionChanged);
        }


        private void set_SPGridTooltip(int rowIndex, DataRow dr)
        {
            string toolTipText = "Added By     : " + dr["sp0_add_operator"].ToString().Trim() + " on " + dr["sp0_date_add"].ToString().ToString() + "\n" +
                                 "Modified By  : " + dr["sp0_lstc_operator"].ToString().Trim() + " on " + dr["sp0_date_lstc"].ToString().ToString();

            foreach (DataGridViewCell cell in SPGrid.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }


        private void FillBranchCombo()
        {
            Cmb_Status.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem1 = new List<Captain.Common.Utilities.ListItem>();
            listItem1.Add(new Captain.Common.Utilities.ListItem("All", "0"));
            listItem1.Add(new Captain.Common.Utilities.ListItem("Active", "Y"));
            listItem1.Add(new Captain.Common.Utilities.ListItem("Inactive", "N"));
            Cmb_Status.Items.AddRange(listItem1.ToArray());
            Cmb_Status.SelectedIndex = 0;

            cmbBranch.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("All", "0"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Branch Primary", "P"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Branch 1", "1"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Branch 2", "2"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Branch 3", "3"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Branch 4", "4"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Branch 5", "5"));
            cmbBranch.Items.AddRange(listItem.ToArray());
            cmbBranch.SelectedIndex = 0;

            CmbSPValid.Items.Clear();
            listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("All", "0"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Valid", "Y"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Invalid", "N"));
            //listItem.Add(new Captain.Common.Utilities.ListItem("To Validate", "N"));
            //listItem.Add(new Captain.Common.Utilities.ListItem("Validated", "Y"));
            CmbSPValid.Items.AddRange(listItem.ToArray());
            CmbSPValid.SelectedIndex = 0;
        }

        private void Txt_SPCode_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_SPCode.Text) && Txt_SPCode.Text.Length < 6)
                Txt_SPCode.Text = int.Parse(Txt_SPCode.Text) != 0 ? "000000".Substring(0, (6 - Txt_SPCode.Text.Length)) + Txt_SPCode.Text : string.Empty;

        }

        string Valid_SW = null;
        private void CmbSPValid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Valid_SW = null;
            if (((Captain.Common.Utilities.ListItem)CmbSPValid.SelectedItem).Value.ToString() != "0")
                Valid_SW = ((Captain.Common.Utilities.ListItem)CmbSPValid.SelectedItem).Value.ToString();

        }

        private void SPGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (SPGrid.Rows.Count > 0)
            {
                Btn_Update_Grps.Visible = false;
                if (Valid_SW != "N")
                {
                    if (SPGrid.CurrentRow.Cells["ValidSW"].Value != null)
                    {
                        if (SPGrid.CurrentRow.Cells["ValidSW"].Value.ToString() == "N")
                            BtnValidate.Visible = true;
                        else
                        {
                            BtnValidate.Visible = false;
                            if (BaseForm.UserID == "JAKE")
                                Btn_Update_Grps.Visible = true;
                        }
                    }
                }

                //ShowCaseNotesImages();
            }

        }

        public void Refresh()
        {
            btnSearch_Click(btnSearch, EventArgs.Empty);
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            ValiDate_SP();
        }

        string Sql_SP_Result_Message = string.Empty;
        private void ValiDate_SP()
        {

            List<CASESP2Entity> CAMA_Details;
            CASESP0Entity SP_Header_Rec;
            string SP_Code = SPGrid.CurrentRow.Cells["Code"].Value.ToString();
            CAMA_Details = _model.SPAdminData.Browse_CASESP2(SP_Code, null, null, null);
            SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SP_Code, null, null, null, null, null, null, null, null);

            if ((CAMA_Details.Count == 0 && SP_Header_Rec.Allow_Add_Branch == "N"))
            {
                //MessageBox.Show("Please Select CA/MS For this Service Plan To Validate", "CAP Systems");
                AlertBox.Show("Please select Service/Outcome for this Service Plan to Validate", MessageBoxIcon.Warning);
                return;
            }


            bool[,] Grp_Valid_Array;
            if (CAMA_Details.Count > 0 || SP_Header_Rec.Allow_Add_Branch == "Y")
            {
                string Error_Msg = string.Empty, Duplicate_Error = "";
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

                            if (Grps_Exists_In_Sel_Branch)
                            {
                                if (CA_Count > 0)
                                    Grp_Valid_Array[i, 0] = true;
                                //commented by sudheer on 11/05/2020
                                //else
                                //    Error_Msg += "        Group-" + (i + 1).ToString() + " in Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Should Contain At least One CA \n";

                                if (MS_Count > 0)
                                    Grp_Valid_Array[i, 1] = true;
                                //commented by sudheer on 11/05/2020
                                //else
                                //    Error_Msg += "        Group-" + (i + 1).ToString() + " in Branch-" + Branch_List.Substring(Branch_Pos, 1) + " Should Contain At least One MS \n";
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
                        int New_SP_Code = 0;
                        
                        if (_model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out Sql_SP_Result_Message))
                        {
                            Refresh_SP_Code = SP_Code;
                            btnSearch_Click(btnSearch, EventArgs.Empty);
                        }
                    }
                    else
                        AlertBox.Show((!string.IsNullOrEmpty(Error_Msg) ? " Group mismatch List:\n" + Error_Msg + "\n" : "") + (!string.IsNullOrEmpty(Duplicate_Error) ? " Duplicates List:\n" + Duplicate_Error : ""), MessageBoxIcon.Warning);
                }
                else
                {
                    SP_Header_Rec.Validate = "Y"; SP_Header_Rec.lstcOperator = BaseForm.UserID;
                    int New_SP_Code = 0;
                    string SP_Success_Status = string.Empty;

                    if (_model.SPAdminData.UpdateCASESP0(SP_Header_Rec, out New_SP_Code, out Sql_SP_Result_Message))
                    {
                        //MessageBox.Show("Service Plan-" + SP_Code + " is Validated And Updated", "CAP Systems"); // Lisa Asked to Comment on 01072013
                        Refresh_SP_Code = SP_Code;
                        btnSearch_Click(btnSearch, EventArgs.Empty);
                    }
                    else
                        AlertBox.Show("Unable to Update Service Plan - " + SP_Code + "\n" +
                                         "Exception: " + Sql_SP_Result_Message, MessageBoxIcon.Warning);
                }
            }
        }


        string Refresh_SP_Code = string.Empty;
        private void SP_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            ADMN0020Form form = sender as ADMN0020Form;
            if (form.DialogResult == DialogResult.OK)
            {
                //Refresh_SP_Code = SPGrid.CurrentRow.Cells["Code"].Value.ToString();
                Refresh_SP_Code = form.Get_Current_SP_Code();

                btnSearch_Click(btnSearch, EventArgs.Empty);
            }
        }

        //Added by Sudheer on 05/20/2021
        private void SP_AddForm_Closed1(object sender, FormClosedEventArgs e)
        {
            ADMN1020Form form = sender as ADMN1020Form;
            if (form.DialogResult == DialogResult.OK)
            {
                //Refresh_SP_Code = SPGrid.CurrentRow.Cells["Code"].Value.ToString();
                Refresh_SP_Code = form.Get_Current_SP_Code();

                btnSearch_Click(btnSearch, EventArgs.Empty);
            }
        }


        // Begin Report Section

        string Agency = null;
        string Random_Filename = null; string PdfName = null; PdfContentByte cb;
        private void On_SaveForm_Closed()
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

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            cb = writer.DirectContent;
            string Name = BaseForm.BaseApplicationName;
            string AppNo = BaseForm.BaseApplicationNo;

            BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);

            DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(SPGrid.CurrentRow.Cells["Code"].Value.ToString().Trim(), null, null, null, null, null, null, null, null);
            DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];

            PdfPTable Header = new PdfPTable(1);
            Header.TotalWidth = 500f;
            Header.WidthPercentage = 100;
            Header.LockedWidth = true;
            float[] Headerwidths = new float[] { 90f };
            Header.SetWidths(Headerwidths);
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            Header.SpacingAfter = 10f;


            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 500f;
            table.WidthPercentage = 100;
            table.LockedWidth = true;
            float[] widths = new float[] { 15f, 80f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;

            int X_Pos, Y_Pos;
            DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(SPGrid.CurrentRow.Cells["Code"].Value.ToString().Trim(), null, null, null);
            DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];
            //dr["sp2_cams_code"].ToString().Trim()
            List<CAMASTEntity> CAList;
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            List<MSMASTEntity> MSList;
            MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

            //DataSet dsCAMAST = DatabaseLayer.SPAdminDB.Browse_CAMAST(null, null, null, null);
            //DataTable dtCAMAST=dsCAMAST.Tables[0];

            //DataSet MSMast = DatabaseLayer.SPAdminDB.Browse_MSMAST(null,null, null, null, null);
            //DataTable DtMSMast=MSMast.Tables[0];
            bool First = true; string MS_Type = string.Empty;
            string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null; ;
            if (dtSP_CaseSP2.Rows.Count > 0)
            {
                DataView dv = dtSP_CaseSP2.DefaultView;
                dv.Sort = "sp2_branch DESC";
                dtSP_CaseSP2 = dv.ToTable();
                Y_Pos = 770;
                X_Pos = 55;
                //cb.BeginText();
                int Count = 0;
                foreach (DataRow dr in dtSP_CaseSP2.Rows)
                {
                    SP_Plan_desc = drSP_Services["sp0_description"].ToString().Trim();
                    Branch = dr["sp2_branch"].ToString().Trim();
                    if (Branch != Priv_Branch)
                    {
                        //cb.EndText();
                        document.Add(table);
                        //if (Count > 0)
                        //{
                        //    for (int i = 0; i <= Count; i++)
                        //    {
                        //        table.DeleteRow(i);
                        //    }
                        //}
                        table.DeleteBodyRows();

                        //table.DeleteRow(1);

                        document.NewPage();
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

                        }
                        Y_Pos = 770;
                        X_Pos = 55;
                        table.SpacingBefore = 10f;

                        PdfPCell Sp_Plan = new PdfPCell(new Phrase("Service :" + SP_Plan_desc.Trim(), fc1));
                        Sp_Plan.HorizontalAlignment = Element.ALIGN_CENTER;
                        Sp_Plan.Colspan = 2;
                        Sp_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Sp_Plan);

                        PdfPCell ServiceDesc = new PdfPCell(new Phrase("Branch :" + Service_desc.Trim(), TblFontBold));
                        ServiceDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                        ServiceDesc.Colspan = 2;
                        ServiceDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(ServiceDesc);

                        string[] col = { "Type", "Description" };
                        for (int i = 0; i < col.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        Priv_Branch = Branch;
                        First = false;
                    }
                    string CAMSType = dr["sp2_type"].ToString();

                    if (CAMSType == "CA")
                    {
                        foreach (CAMASTEntity drCAMAST in CAList)
                        {
                            if (drCAMAST.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                            {
                                CAMSDesc = drCAMAST.Desc.ToString().Trim(); break;
                            }
                            else
                                CAMSDesc = "";
                        }

                        //foreach(DataRow drCAMAST in dtCAMAST.Rows)
                        //{
                        //    if(drCAMAST["CA_CODE"].ToString().Trim()==dr["sp2_cams_code"].ToString().Trim())
                        //    {
                        //        CAMSDesc = drCAMAST["CA_DESC"].ToString();break;
                        //    }
                        //    else
                        //        CAMSDesc="";
                        //}

                        //CAMSDesc = drCAMAST["CA_DESC"].ToString();

                        PdfPCell RowType = new PdfPCell(new Phrase("Service", TableFont));
                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                        table.AddCell(RowType);

                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                        table.AddCell(RowDesc);
                    }
                    else
                    {
                        string Type_Desc = string.Empty;
                        foreach (MSMASTEntity drMSMast in MSList)
                        {
                            if (drMSMast.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                            {
                                CAMSDesc = drMSMast.Desc.ToString().Trim();
                                MS_Type = drMSMast.Type1.ToString();
                                if (MS_Type == "M")
                                    Type_Desc = "Milestone";
                                else Type_Desc = "Outcome";
                                break;
                            }
                            else
                                CAMSDesc = "";
                        }


                      
                        PdfPCell RowType = new PdfPCell(new Phrase(Type_Desc, TblFontBold));
                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                        table.AddCell(RowType);

                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TblFontBold));
                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                        table.AddCell(RowDesc);
                    }
                }
                Count++;

            }
            document.Add(Header);
            if (table.Rows.Count > 0)
                document.Add(table);
            else
            {
                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 15);
                cb.ShowTextAligned(Element.ALIGN_CENTER, "Critical Activities and MileStones are not Defined for this Service Plan", 300, 600, 0);
                cb.EndText();
            }
            document.Close();
            fs.Close();
            fs.Dispose();

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
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void On_Delete_PDF_File()
        {
            System.IO.File.Delete(PdfName);
        }

        // End Report Section..............................

        private string Get_Depliidate_CAMS_IN_Branch(int Curr_Grp)
        {
            string Error = "", CAMS_Codes_List = "";
           

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

        private void ADMN0020control_Load(object sender, EventArgs e)
        {
            //ShowCaseNotesImages();
        }

        private void Btn_Update_Grps_Click(object sender, EventArgs e)
        {
            if (SPGrid.Rows.Count > 0)
            {
                if (_model.SPAdminData.Fix_Current_Groups_In_CAMS_Postings(SPGrid.CurrentRow.Cells["Code"].Value.ToString(), out Sql_SP_Result_Message))
                {
                    AlertBox.Show("Groups Updated Successfully");
                }
            }
        }


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



            string SPCode = string.Empty;
            SPCode = SPGrid.CurrentRow.Cells["Code"].Value.ToString().Trim();

            DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(null, null, null, null, null, null, null, null, null);
            //DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];



            DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(null, null, null, null);
            DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];

            List<CAMASTEntity> CAList;
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            List<MSMASTEntity> MSList;
            MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

            bool First = true; string MS_Type = string.Empty;
            string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null;
            bool SPNum = true; Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;

            string PrvRepName = ""; int i = 0; int k = 1; string Pri_SP = string.Empty;

            foreach (DataRow drSP_Services in dsSP_Services.Tables[0].Rows)
            {
                if (dtSP_CaseSP2.Rows.Count > 0)
                {
                    DataView dv = dtSP_CaseSP2.DefaultView;
                    dv.RowFilter = "sp2_serviceplan=" + drSP_Services["sp0_servicecode"].ToString();
                    dv.Sort = "sp2_branch DESC";
                    DataTable CaseSP2 = dv.ToTable();
                    
                    if (drSP_Services["sp0_servicecode"].ToString().Trim() != Pri_SP.Trim())
                    {
                        string ReportName = drSP_Services["sp0_description"].ToString();//"Sheet1";
                        ReportName = ReportName.Replace("/", "").Replace(":","");

                        ReportName = drSP_Services["sp0_servicecode"].ToString().Trim() + "_" + ReportName.Trim();
                        k++;

                        if (ReportName.Length >= 31)
                        {
                            ReportName = ReportName.Substring(0, 31);
                        }

                        if (PrvRepName == ReportName)
                        {
                            i++;
                            ReportName = ReportName.Substring(0, ReportName.Length - 2) + i.ToString();
                        }
                        PrvRepName = ReportName;

                        //if (Count > 1) ReportName = "Sheet" + Count.ToString();

                        sheet = book.Worksheets.Add(ReportName);
                        sheet.Table.DefaultRowHeight = 14.25F;

                        sheet.Table.Columns.Add(200);
                        sheet.Table.Columns.Add(200);
                        sheet.Table.Columns.Add(250);
                        sheet.Table.Columns.Add(52);
                        if (Member_Activity == "Y")
                            sheet.Table.Columns.Add(75);
                        sheet.Table.Columns.Add(75);
                        sheet.Table.Columns.Add(75);

                        Row0 = sheet.Table.Rows.Add();

                        SPNum = true;

                        //if (!SPNum)
                        //{
                        //sheet = book.Worksheets.Add(drSP_Services["sp0_description"].ToString().Trim());
                        //}

                        cell = Row0.Cells.Add("SPM", DataType.String, "s94");
                        cell = Row0.Cells.Add("Branch", DataType.String, "s94");
                        cell = Row0.Cells.Add("Description", DataType.String, "s94");
                        cell = Row0.Cells.Add("Type", DataType.String, "s94");
                        if (Member_Activity == "Y")
                            cell = Row0.Cells.Add("Category", DataType.String, "s94");
                        cell = Row0.Cells.Add("Add Date", DataType.String, "s94");
                        cell = Row0.Cells.Add("Change Date", DataType.String, "s94");

                        bool CAMS = true; Branch = null; Priv_Branch = null; First = true;
                        if (CaseSP2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in CaseSP2.Rows)
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

                                    //cell.MergeAcross = 1;

                                    //cell = Row0.Cells.Add("Service :" + SP_Plan_desc.Trim(), DataType.String, "s95B");

                                    //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_add"].ToString().Trim()), DataType.String, "s95B");
                                    //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_lstc"].ToString().Trim()), DataType.String, "s95B");

                                    //PdfPCell Sp_Plan = new PdfPCell(new Phrase("Service :" + SP_Plan_desc.Trim(), fc1));
                                    //Sp_Plan.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //Sp_Plan.Colspan = 2;
                                    //Sp_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Sp_Plan);

                                    if (SPNum)
                                        cell = Row0.Cells.Add("Service :" + drSP_Services["sp0_description"].ToString().Trim(), DataType.String, "s96");
                                    else cell = Row0.Cells.Add("", DataType.String, "s96");
                                    //Row0 = sheet.Table.Rows.Add();
                                    if (!CAMS) Row0.Cells.Add("", DataType.String, "s96");
                                    else cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                    //cell.MergeAcross = 1;

                                    //cell = Row0.Cells.Add("", DataType.String, "s96");
                                    //cell = Row0.Cells.Add("", DataType.String, "s96");

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
                                    Priv_Branch = Branch;
                                    First = false; SPNum = false;
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
                                        cell = Row0.Cells.Add("Service", DataType.String, "s95R");
                                        if (Member_Activity == "Y")
                                            cell = Row0.Cells.Add(drSP_Services["SP0_Category"].ToString().Trim(), DataType.String, "s95R");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95R");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95R");

                                    }
                                    else
                                    {
                                        cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95");
                                        cell = Row0.Cells.Add("Service", DataType.String, "s95");
                                        if (Member_Activity == "Y")
                                            cell = Row0.Cells.Add(drSP_Services["SP0_Category"].ToString().Trim(), DataType.String, "s95");
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

                                    CAMS = false;
                                }
                            }

                        }
                        else
                        {
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

                        }
                        Count++;

                        Pri_SP = drSP_Services["sp0_servicecode"].ToString();
                    }
                }

            }


           

            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            //FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //downloadGateway.Filename = "SPREPAPP_Report.xls";

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


    }
}