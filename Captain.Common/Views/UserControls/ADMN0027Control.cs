/************************************************************************
 * Conversion On    :   01/18/2023      * Converted By     :   Kranthi
 * Modified On      :   01/18/2023      * Modified By      :   Kranthi
 * **********************************************************************/

#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.Drawing;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using DevExpress.XtraReports.UI;
using static Amazon.S3.Util.S3EventNotification;
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ADMN0027Control : BaseUserControl
    {
        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        private string strYear = "    ";
        private string strMode = Consts.Common.View;

        int strIndex = 0;
        int strGroupIndex = 0;
        int strQuestionIndex = 0;
        #endregion
        public ADMN0027Control(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            if (this.Width >= Application.Browser.Size.Width ||
             this.Height >= Application.Browser.Size.Height)
            {
                //this.WindowState = FormWindowState.Maximized;
                this.Width = Application.Browser.Size.Width;
                this.Height = Application.Browser.Size.Height;
            }

            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            propSearch_Entity = _model.SPAdminData.Browse_CASESP0List(null, null, null, null, null, null, null, null, null);
            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //if (ds.Tables.Count > 0)
            //{
            //    DataTable dt = ds.Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        Program_Year = dt.Rows[0]["DEP_YEAR"].ToString();
            //    }
            //}

            Program_Year = string.Empty;
            List<ProgramDefinitionEntity> CASEDEP_List = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty);
            if (CASEDEP_List.Count > 0)
            {
                ProgramDefinitionEntity DefEntity = CASEDEP_List.Find(u => u.DepYear.Trim() != string.Empty);
                if (DefEntity != null)
                {
                    Program_Year = DefEntity.DepYear;
                }
            }
            chkbxLumpsum.Checked = false;
            fillIntervalCombo();
            Fill_SP_DropDowns();
            

            txtNonVul50.Validator = TextBoxValidation.FloatValidator;
            txtNonVul75.Validator = TextBoxValidation.FloatValidator;
            txtNonVul150.Validator = TextBoxValidation.FloatValidator;
            txtVul50.Validator = TextBoxValidation.FloatValidator;
            txtVul75.Validator = TextBoxValidation.FloatValidator;
            txtVul150.Validator = TextBoxValidation.FloatValidator;

            // txtVulInv.Validator = TextBoxValidation.IntegerValidator;
            //txtNonVulInv.Validator = TextBoxValidation.IntegerValidator;

            txtVulBenType.Validator = TextBoxValidation.IntegerValidator;
            txtNONVulBenType.Validator = TextBoxValidation.IntegerValidator;
            txtSubRepID.Validator = TextBoxValidation.IntegerValidator;
            //if (!string.IsNullOrEmpty(Program_Year.Trim()))
            //{
            //    lblProgYear.Text = "Program Year: " + Program_Year;
            //    //CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, Program_Year, string.Empty, string.Empty);



            //}
            //else
            //{
            //    lblProgYear.Text = "Program Year: ";
            //}

            if (!string.IsNullOrEmpty(Program_Year))
            {

                YearCombo();
                if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                {
                    cmbYear.Visible = true; lblProgYear.Visible = true; //btnCopy.Visible = true;

                    VisibilityCopyButton();
                }
                else { cmbYear.Visible = false; lblProgYear.Visible = false; btnCopy.Visible = false; }

                if (Privileges.AddPriv.Equals("false") && Privileges.ChangePriv.Equals("false"))
                {
                    Btn_Save.Visible = false; btnCopy.Visible = false;
                }
                else if (Privileges.ChangePriv.Equals("false"))
                {
                    btnCopy.Visible = false;
                }

                FillControls();
            }
            else
            {
                CEAPCNTL_List = new List<CEAPCNTLEntity>();

                AlertBox.Show("Please define the Program Year for the Hierarchy");

            }

            ///** Max invoice control panel hide **/
            //panel6.Size = new System.Drawing.Size(this.Width, 32);
            ////panel11.Visible = false;
            ///** Max invoice control panel **/

            PopulateToolbar(oToolbarMnustrip);
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }
        public ToolBarButton ToolBarHistory
        {
            get; set;
        }
        public ToolBarButton ToolBarHelp { get; set; }

        public string Mode { get; set; }

        public string Program_Year { get; set; }

        #endregion

        List<CASESP0Entity> propSearch_Entity { get; set; }
        List<CEAPCNTLEntity> CEAPCNTL_List { get; set; }

        private void YearCombo()
        {
            //DataSet ds = DatabaseLayer.FuelControlDB.Browse_State_MedianInc_Guide();
            //DataTable dt = new DataTable();
            //if (ds != null)
            //    dt = ds.Tables[0];

            //DataView dv = new DataView(dt);
            //dv.RowFilter = "SMG_YEAR <>'CHAP'";
            //dv.Sort = "SMG_YEAR DESC";
            //dt = dv.ToTable();

            cmbYear.Items.Clear();
            this.cmbYear.SelectedIndexChanged -= new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            List<ListItem> listItem = new List<ListItem>();
            if (int.Parse(DateTime.Now.Month.ToString()) > 6)
                listItem.Add(new ListItem(DateTime.Now.AddYears(+1).Year.ToString(), DateTime.Now.AddYears(+1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-3).Year.ToString(), DateTime.Now.AddYears(-3).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-4).Year.ToString(), DateTime.Now.AddYears(-4).Year.ToString()));
            listItem.Add(new ListItem(DateTime.Now.AddYears(-5).Year.ToString(), DateTime.Now.AddYears(-5).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-6).Year.ToString(), DateTime.Now.AddYears(-6).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-7).Year.ToString(), DateTime.Now.AddYears(-7).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-8).Year.ToString(), DateTime.Now.AddYears(-8).Year.ToString()));
            //listItem.Add(new ListItem(DateTime.Now.AddYears(-9).Year.ToString(), DateTime.Now.AddYears(-9).Year.ToString()));
            cmbYear.Items.AddRange(listItem.ToArray());
            //cmbYear.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
            {
                CommonFunctions.SetComboBoxValue(cmbYear, Program_Year);

                this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            }
            else
                AlertBox.Show("Please define the Program Year for the Hierarchy");
            //CommonFunctions.SetComboBoxValue(cmbYear, BaseForm.BaseYear);



        }

        private void fillIntervalCombo()
        {
            cmbInterval.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Annual", "A"));
            listItem.Add(new ListItem("Semi-Annual", "S"));
            listItem.Add(new ListItem("Quarterly", "Q"));

            cmbInterval.Items.AddRange(listItem.ToArray());
        }

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
                ToolBarNew.ToolTipText = "New";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add"; //new  IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit"; // new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete"; // new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHistory = new ToolBarButton();
                ToolBarHistory.Tag = "History";
                ToolBarHistory.ToolTipText = "CEAP History";
                ToolBarHistory.Enabled = true;
                ToolBarHistory.ImageSource = "captain-caseHistory";
                ToolBarHistory.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHistory.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarPrint = new ToolBarButton();
                //ToolBarPrint.Tag = "Print";
                //ToolBarPrint.ToolTipText = "Eligibility Criteria Print";
                //ToolBarPrint.Enabled = true;
                //ToolBarPrint.Image = new IconResourceHandle(Consts.Icons16x16.Print);
                //ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help"; //new IconResourceHandle(Consts.Icons16x16.Help);
                //ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }

            if (Privileges.AddPriv.Equals("false"))
            {
                if (ToolBarNew != null) ToolBarNew.Enabled = false;
            }
            else
            {
                if (ToolBarNew != null) ToolBarNew.Enabled = true;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
            }
            else
            {
                if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
            }

            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = false;
            }
            else
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = true;
            }
            if (CEAPCNTL_List != null)
            {
                if (CEAPCNTL_List.Count > 0)
                {
                    ToolBarNew.Visible = false;
                    ToolBarEdit.Visible = true;
                    ToolBarDel.Visible = true;
                }
                else
                {
                    ToolBarEdit.Visible = false;
                    ToolBarDel.Visible = false;
                    ToolBarNew.Visible = false;
                    if (!string.IsNullOrEmpty(Program_Year.Trim()))
                        ToolBarNew.Visible = true;

                }
            }

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                ToolBarHistory,
                //ToolBarPrint,
                ToolBarHelp
            });

            //if (gvwHierachy.Rows.Count == 0)
            //{
            //    if (ToolBarEdit != null)
            //        ToolBarEdit.Enabled = false;
            //    if (ToolBarDel != null)
            //        ToolBarDel.Enabled = false;
            //    //if (ToolBarPrint != null)
            //    //    ToolBarPrint.Enabled = false;
            //}
        }

        /// <summary>
        /// Handles the toolbar button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        Mode = "Add";

                        if (!string.IsNullOrEmpty(((ListItem)cmbYear.SelectedItem).Value.ToString().Trim().Trim())) //Program_Year
                        {
                            panel1.Enabled = true;
                            pnlLIHWAP.Enabled = true;
                            panel11.Enabled = true;
                            dtpProgStart.Focus();

                            ToolBarNew.Enabled = false;
                            cmbYear.Enabled = false;

                            ///** Max invoice control panel hide **/
                            //panel6.Size = new System.Drawing.Size(this.Width, 32);
                            //panel11.Visible = false;
                            ///** Max invoice control panel **/
                        }
                        break;
                    case Consts.ToolbarActions.Edit:
                        Mode = "Edit";
                        if (CEAPCNTL_List.Count > 0)
                        {
                            panel1.Enabled = true;
                            pnlLIHWAP.Enabled = true;
                            panel11.Enabled = true;

                            ToolBarEdit.Enabled = false;
                            ToolBarDel.Enabled = false;
                            cmbYear.Enabled = false;

                            ///** Max invoice control panel show **/
                            //panel11.Visible = true;
                            //panel6.Size = new System.Drawing.Size(this.Width, 62);
                            ///** Max invoice control panel **/

                            dtpProgStart.Focus();
                        }
                        //if (gvwHierachy.Rows.Count > 0)
                        //{
                        //    ADMN0024Form EditForm = new ADMN0024Form(BaseForm, "Edit", Consts.LiheAllDetails.strMainType, gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString(), Privileges);
                        //    EditForm.FormClosed += new Form.FormClosedEventHandler(OnCase0009FromClosed);
                        //    EditForm.ShowDialog();
                        //}

                        break;
                    case Consts.ToolbarActions.Delete:
                        if (CEAPCNTL_List.Count > 0)
                        {
                            List<CASESPMEntity> CaseSPM_List = new List<CASESPMEntity>();
                            CASESPMEntity Search_CaseSPM_Entity = new CASESPMEntity(true);

                            Search_CaseSPM_Entity.year = ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim();//Program_Year;
                            CaseSPM_List = _model.SPAdminData.Browse_CASESPM(Search_CaseSPM_Entity, "Browse");
                            if (CaseSPM_List.Count > 0)
                            {
                                CaseSPM_List = CaseSPM_List.FindAll(u => u.service_plan.Equals(CEAPCNTL_List[0].CPCT_VUL_SP) || u.service_plan.Equals(CEAPCNTL_List[0].CPCT_NONVUL_SP));
                                if (CaseSPM_List.Count > 0)
                                {
                                    AlertBox.Show("There are dependecies to these Service Plans", MessageBoxIcon.Warning, null, ContentAlignment.BottomRight);
                                }
                                else
                                {
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                                }
                            }
                            else
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                        }

                        //if (gvwHierachy.Rows.Count > 0)
                        //{
                        //    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandler, true);
                        //}
                        break;
                    case Consts.ToolbarActions.History:
                        //CaseSnpEntity caseHistSnp = GetSelectedRow();
                        //if (caseHistSnp != null)
                        //{
                        //CEAP_HistoryForm historyForm = new CEAP_HistoryForm(BaseForm, Privileges, ((ListItem)cmbYear.SelectedItem).Text.ToString().Trim());
                        //historyForm.StartPosition = FormStartPosition.CenterScreen;
                        //historyForm.ShowDialog();
                        //}
                        break;
                    case Consts.ToolbarActions.Print:
                        //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false);
                        //pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveForm_Closed);
                        //pdfListForm.ShowDialog();
                        //PDFModule = "All";
                        //On_SaveForm_Closed();


                        break;
                    case Consts.ToolbarActions.Help:
                        //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Case2009");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                // ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }


        string ACR_SERV_Hies = string.Empty;
        private void Fill_SP_DropDowns()
        {
            this.CmbSP.SelectedIndexChanged -= new System.EventHandler(this.CmbSP_SelectedIndexChanged);
            this.CmbNonVulSP.SelectedIndexChanged -= new System.EventHandler(this.CmbNonVulSP_SelectedIndexChanged);
            CmbSP.Items.Clear(); CmbNonVulSP.Items.Clear(); cmbHCSP.Items.Clear();
            cmbP1Sp.Items.Clear();
            cmbP2Sp.Items.Clear();
            cmbP3Sp.Items.Clear();

            cmbP1CA1.Items.Clear();
            cmbP1CA2.Items.Clear();
            cmbP1CA3.Items.Clear();

            cmbP2CA1.Items.Clear();
            cmbP2CA2.Items.Clear();
            cmbP2CA3.Items.Clear();

            cmbP3CA1.Items.Clear();
            cmbP3CA2.Items.Clear();
            cmbP3CA3.Items.Clear();

            List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();
            ACR_SERV_Hies = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }

            if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            {
                if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, null, null, BaseForm.UserID);
                else
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, null, null, BaseForm.UserID);
            }
            else
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, BaseForm.BaseAgency, null, null);

            CmbSP.ColorMember = "FavoriteColor"; CmbNonVulSP.ColorMember = "FavoriteColor"; cmbHCSP.ColorMember = "FavoriteColor";
            cmbP1Sp.ColorMember = "FavoriteColor";
            cmbP2Sp.ColorMember = "FavoriteColor";
            cmbP3Sp.ColorMember = "FavoriteColor";

            //string strcmbFundsource = ((ListItem)cmbFundsource.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbFundsource.SelectedItem).Value.ToString();
            if (SP_Hierarchies.Count > 0)
            {
                bool SP_Exists = false, Allow_Dups = false;
                string Tmp_SP_Desc = null, Tmp_SP_Code = null, SP_Valid = null, SPM_Start_Date = " ", SP_DESC = " ", spm_posting_year = "";
                int Tmp_Sel_Index = 0, Itr_Index = 0;


                CmbSP.Items.Add(new Captain.Common.Utilities.ListItem("", "00"));
                CmbNonVulSP.Items.Add(new Captain.Common.Utilities.ListItem("", "00"));
                cmbHCSP.Items.Add(new Captain.Common.Utilities.ListItem("", "00"));
                cmbP1Sp.Items.Add(new Captain.Common.Utilities.ListItem("", "00"));
                cmbP2Sp.Items.Add(new Captain.Common.Utilities.ListItem("", "00"));
                cmbP3Sp.Items.Add(new Captain.Common.Utilities.ListItem("", "00"));

                foreach (CASESP1Entity Entity in SP_Hierarchies)  // 08122012
                {
                    SP_Exists = Allow_Dups = false;
                    Tmp_SP_Desc = null;
                    // SP_Valid = Entity.SP_validated;
                    SPM_Start_Date = " ";
                    if (Entity.SP_validated.ToUpper() == "Y" && Entity.Sp0_Active.ToUpper() == "Y")
                    {
                        SP_Valid = "Y";
                    }
                    else
                    {
                        SP_Valid = "N";
                    }

                    if (SP_Valid.ToUpper() == "Y")
                    {
                        //if ((Mode.Equals("Add") && !SP_Exists) || (Mode.Equals("Add") && Allow_Dups))// || (Mode.Equals("Edit") && SP_Exists))
                        //{
                        Tmp_SP_Code = "000000".Substring(0, (6 - Entity.Code.Length)) + Entity.Code;

                        if (propSearch_Entity.Count > 0)
                        {
                            CASESP0Entity casesp0data = propSearch_Entity.Find(u => u.Code == Entity.Code && u.Funds.Trim() != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code.Trim() == string.Empty);
                            if (casesp0data != null)
                            {
                                if (casesp0data.Sp0ReadOnly != "Y" && casesp0data.NoSPM == "Y")
                                {
                                    CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red)));
                                    CmbNonVulSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red)));
                                }
                                if (casesp0data.Sp0ReadOnly != "Y")
                                {
                                    cmbHCSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red)));

                                    cmbP1Sp.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red)));
                                    cmbP2Sp.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red)));
                                    cmbP3Sp.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Black : Color.Red)));
                                }
                            }
                        }
                        //}
                    }
                }
                //}
            }
            this.CmbSP.SelectedIndexChanged += new System.EventHandler(this.CmbSP_SelectedIndexChanged);
            //if (Mode.Equals("Add") && CmbSP.Items.Count == 1)
            if (CmbSP.Items.Count > 0)
            {
                CmbSP.SelectedIndex = 0;

            }
            this.CmbNonVulSP.SelectedIndexChanged += new System.EventHandler(this.CmbNonVulSP_SelectedIndexChanged);
            if (CmbNonVulSP.Items.Count > 0)
            {
                CmbNonVulSP.SelectedIndex = 0;

            }
            if (cmbHCSP.Items.Count > 0)
            {
                cmbHCSP.SelectedIndex = 0;
                cmbP1Sp.SelectedIndex = 0;
                cmbP2Sp.SelectedIndex = 0;
                cmbP3Sp.SelectedIndex = 0;
            }

        }

        private void FillControls()
        {
            if (cmbYear.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(((ListItem)cmbYear.SelectedItem).Value.ToString().Trim()))
                {
                    CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty);
                    if (CEAPCNTL_List.Count > 0)
                    {
                        _CEAP27Code = CEAPCNTL_List[0].CPCT_CODE;
                        _CEAP27Year = CEAPCNTL_List[0].CPCT_YEAR;



                        dtpProgStart.Text = CEAPCNTL_List[0].CPCT_PROG_START;
                        dtpProgStart.Checked = true;
                        dtpProgEnd.Text = CEAPCNTL_List[0].CPCT_PROG_END;
                        dtpProgEnd.Checked = true;

                        CommonFunctions.SetComboBoxValue(CmbSP, CEAPCNTL_List[0].CPCT_VUL_SP);
                        txtVul50.Text = CEAPCNTL_List[0].CPCT_VUL_50;
                        txtVul75.Text = CEAPCNTL_List[0].CPCT_VUL_75;
                        txtVul150.Text = CEAPCNTL_List[0].CPCT_VUL_150;
                        // txtVulInv.Text = CEAPCNTL_List[0].CPCT_VUL_MAX_INV;
                        _CEAPMaxVUL = CEAPCNTL_List[0].CPCT_VUL_MAX_INV;
                        CommonFunctions.SetComboBoxValue(cmbVulCA, CEAPCNTL_List[0].CPCT_VUL_PRIM_CA);

                        CommonFunctions.SetComboBoxValue(CmbNonVulSP, CEAPCNTL_List[0].CPCT_NONVUL_SP);
                        txtNonVul50.Text = CEAPCNTL_List[0].CPCT_NONVUL_50;
                        txtNonVul75.Text = CEAPCNTL_List[0].CPCT_NONVUL_75;
                        txtNonVul150.Text = CEAPCNTL_List[0].CPCT_NONVUL_150;
                        // txtNonVulInv.Text = CEAPCNTL_List[0].CPCT_NONVUL_MAX_INV;
                        _CEAPMaxNonVUL = CEAPCNTL_List[0].CPCT_NONVUL_MAX_INV;
                        CommonFunctions.SetComboBoxValue(cmbNonVulCA, CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA);


                        if (CEAPCNTL_List[0].CPCT_LUMP_SUM == "Y")
                        {
                            chkbxLumpsum.Checked = true;
                            CommonFunctions.SetComboBoxValue(cmbInterval, CEAPCNTL_List[0].CPCT_LUMP_INTRVAL);

                            if (CEAPCNTL_List[0].CPCT_USER_CNTL == "Y")
                                chkbUserCntrl.Checked = true;
                            else
                                chkbUserCntrl.Checked = false;
                        }
                        else
                        {
                            chkbxLumpsum.Checked = false;
                            chkbUserCntrl.Checked = false;
                        }

                        
                        CommonFunctions.SetComboBoxValue(cmbVulService, CEAPCNTL_List[0].CPCT_VUL_ARR_CA);
                        CommonFunctions.SetComboBoxValue(cmbNonVulService, CEAPCNTL_List[0].CPCT_NONVUL_ARR_CA);

                        txtVulSSI.Text = CEAPCNTL_List[0].CPCT_VUL_SSI;
                        txtVulMTVC.Text = CEAPCNTL_List[0].CPCT_VUL_MTVC;
                        txtNonVulSSI.Text = CEAPCNTL_List[0].CPCT_NONVUL_SSI;
                        txtNonVulMTVC.Text = CEAPCNTL_List[0].CPCT_NONVUL_MTVC;

                        //Added by Sudheer on 11/28/23
                        txtVulSNAP.Text = CEAPCNTL_List[0].CPCT_VUL_SNAP;
                        txtNVulSNAP.Text = CEAPCNTL_List[0].CPCT_NONVUL_SNAP;

                        // Vikash added on 05/06/2023
                        txtVulBenType.Text = CEAPCNTL_List[0].CPCT_VUL_BENTYPE;
                        txtNONVulBenType.Text = CEAPCNTL_List[0].CPCT_NONVUL_BENTYPE;
                        txtCrisisBenType.Text = CEAPCNTL_List[0].CPCT_CRISIS_BENTYPE;

                        _CEAPMethod = CEAPCNTL_List[0].CPCT_INV_METHOD;

                        if (!string.IsNullOrEmpty(CEAPCNTL_List[0].CPCT_SUBREP_ID.Trim()))
                            txtSubRepID.Text = "00000000000000".Substring(0, 14 - CEAPCNTL_List[0].CPCT_SUBREP_ID.Length) + CEAPCNTL_List[0].CPCT_SUBREP_ID;

                        CommonFunctions.SetComboBoxValue(cmbHCSP, CEAPCNTL_List[0].CPCT_HC_SP);
                        CommonFunctions.SetComboBoxValue(cmbHCService, CEAPCNTL_List[0].CPCT_HC_CA);
                        CommonFunctions.SetComboBoxValue(cmbP1Sp, CEAPCNTL_List[0].CPCT_P1_SP);
                        CommonFunctions.SetComboBoxValue(cmbP2Sp, CEAPCNTL_List[0].CPCT_P2_SP);
                        CommonFunctions.SetComboBoxValue(cmbP3Sp, CEAPCNTL_List[0].CPCT_P3_SP);

                        CommonFunctions.SetComboBoxValue(cmbP1CA1, CEAPCNTL_List[0].CPCT_P1_CA1);
                        CommonFunctions.SetComboBoxValue(cmbP1CA2, CEAPCNTL_List[0].CPCT_P1_CA2);
                        CommonFunctions.SetComboBoxValue(cmbP1CA3, CEAPCNTL_List[0].CPCT_P1_CA3);

                        CommonFunctions.SetComboBoxValue(cmbP2CA1, CEAPCNTL_List[0].CPCT_P2_CA1);
                        CommonFunctions.SetComboBoxValue(cmbP2CA2, CEAPCNTL_List[0].CPCT_P2_CA2);
                        CommonFunctions.SetComboBoxValue(cmbP2CA3, CEAPCNTL_List[0].CPCT_P2_CA3);

                        CommonFunctions.SetComboBoxValue(cmbP3CA1, CEAPCNTL_List[0].CPCT_P3_CA1);
                        CommonFunctions.SetComboBoxValue(cmbP3CA2, CEAPCNTL_List[0].CPCT_P3_CA2);
                        CommonFunctions.SetComboBoxValue(cmbP3CA3, CEAPCNTL_List[0].CPCT_P3_CA3);

                        txtCSBGContractNo.Text = CEAPCNTL_List[0].CPCT_CSBG_CONTRACT_NO.ToString();
                    }
                    else
                    {

                        dtpProgStart.Text = DateTime.Now.Date.ToString();
                        dtpProgStart.Checked = false;
                        dtpProgEnd.Text = DateTime.Now.Date.ToString();
                        dtpProgEnd.Checked = false;

                        CommonFunctions.SetComboBoxValue(CmbSP, "0");
                        txtVul50.Text = string.Empty;
                        txtVul75.Text = string.Empty;
                        txtVul150.Text = string.Empty;
                        //txtVulInv.Text = string.Empty;
                        CommonFunctions.SetComboBoxValue(cmbVulCA, "0");
                        CommonFunctions.SetComboBoxValue(cmbVulService, "0");

                        CommonFunctions.SetComboBoxValue(CmbNonVulSP, "0");
                        txtNonVul50.Text = string.Empty;
                        txtNonVul75.Text = string.Empty;
                        txtNonVul150.Text = string.Empty;
                        //txtNonVulInv.Text = string.Empty;
                        CommonFunctions.SetComboBoxValue(cmbNonVulCA, "0");

                        CommonFunctions.SetComboBoxValue(cmbNonVulService, "0");

                        txtVulSSI.Text = string.Empty;
                        txtVulMTVC.Text = string.Empty;
                        txtNonVulSSI.Text = string.Empty;
                        txtNonVulMTVC.Text = string.Empty;
                        //Added by Sudheer on 11/28/23
                        txtVulSNAP.Text = string.Empty;
                        txtNVulSNAP.Text = string.Empty;

                        CommonFunctions.SetComboBoxValue(cmbHCSP, "0");
                        CommonFunctions.SetComboBoxValue(cmbHCService, "0");
                      
                        chkbxLumpsum.Checked = false;

                        // Vikash added on 05/06/2023
                        txtVulBenType.Text = string.Empty;
                        txtNONVulBenType.Text = string.Empty;

                        txtCSBGContractNo.Text = string.Empty;
                    }

                    ///** Max invoice control panel hide **/
                    //panel6.Size = new System.Drawing.Size(this.Width, 32);
                    //panel11.Visible = false;
                    ///** Max invoice control panel **/
                }
            }
        }

        List<CASESP2Entity> SP_CAMS_Details = new List<CASESP2Entity>();
        List<CASESP2Entity> SP_NONVulCAMS_Details = new List<CASESP2Entity>();
        private void FillVulActivityCombo()
        {
            cmbVulCA.Items.Clear(); cmbVulService.Items.Clear();

            SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(((ListItem)CmbSP.SelectedItem).Value.ToString(), null, null, null, "CASE4006");

            if (SP_CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_CAMS_Details.FindAll(u => u.ServPlan == ((ListItem)CmbSP.SelectedItem).Value.ToString() && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    bool IsService = true;
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        //List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", "04", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, Entity.ServPlan, Entity.Branch, Entity.Curr_Grp.ToString(), Entity.CamCd.Trim(), "SP");
                        //propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == "04");

                        //if (propPMTFLDCNTLHEntity.Count == 0)
                        //{
                        //    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", "04", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "0", " ", "0", "          ", "hie");
                        //    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == "04");
                        //}

                        //if (propPMTFLDCNTLHEntity.Count > 0)
                        //{
                        //    PMTFLDCNTLHEntity PMFLDCnt = propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == "S00205");
                        //    if (PMFLDCnt != null)
                        //    {
                        //        if (PMFLDCnt.PMFLDH_ENABLED == "Y") IsService = true; else IsService = false;
                        //    }
                        //}

                        if (IsService)
                        {
                            cmbVulCA.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                            cmbVulService.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        }
                    }
                }
            }

            if (cmbVulCA.Items.Count > 0)
            {
                cmbVulCA.SelectedIndex = 0;
            }
            if (cmbVulService.Items.Count > 0)
            {
                cmbVulService.SelectedIndex = 0;
            }
        }

        private void FillNonVulActivityCombo()
        {
            cmbNonVulCA.Items.Clear(); cmbNonVulService.Items.Clear();

            SP_NONVulCAMS_Details = _model.SPAdminData.Browse_CASESP2(((ListItem)CmbNonVulSP.SelectedItem).Value.ToString(), null, null, null, "CASE4006");

            if (SP_CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_NONVulCAMS_Details.FindAll(u => u.ServPlan == ((ListItem)CmbNonVulSP.SelectedItem).Value.ToString() && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    bool IsService = true;
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        //List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", "04", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, Entity.ServPlan, Entity.Branch, Entity.Curr_Grp.ToString(), Entity.CamCd.Trim(), "SP");
                        //propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == "04");

                        //if (propPMTFLDCNTLHEntity.Count == 0)
                        //{
                        //    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", "04", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "0", " ", "0", "          ", "hie");
                        //    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == "04");
                        //}

                        //if (propPMTFLDCNTLHEntity.Count > 0)
                        //{
                        //    PMTFLDCNTLHEntity PMFLDCnt = propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == "S00205");
                        //    if (PMFLDCnt != null)
                        //    {
                        //        if (PMFLDCnt.PMFLDH_ENABLED == "Y") IsService = true; else IsService = false;
                        //    }
                        //}

                        if (IsService)
                        {
                            cmbNonVulCA.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                            cmbNonVulService.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        }
                    }
                }
            }

            if (cmbNonVulCA.Items.Count > 0)
            {
                cmbNonVulCA.SelectedIndex = 0;
            }
            if (cmbNonVulService.Items.Count > 0)
            {
                cmbNonVulService.SelectedIndex = 0;
            }
        }

        private void FillP1Services()
        {
            cmbP1CA1.Items.Clear();
            cmbP1CA2.Items.Clear();
            cmbP1CA3.Items.Clear();

            List<CASESP2Entity> SP_P1CAMS_Details = _model.SPAdminData.Browse_CASESP2(((ListItem)cmbP1Sp.SelectedItem).Value.ToString(), null, null, null, "CASE4006");

            if (SP_P1CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_P1CAMS_Details.FindAll(u => u.ServPlan == ((ListItem)cmbP1Sp.SelectedItem).Value.ToString() && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        cmbP1CA1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        cmbP1CA2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        cmbP1CA3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));

                    }
                }
            }

            if (cmbP1CA1.Items.Count > 0)
            {
                cmbP1CA1.SelectedIndex = 0;
                cmbP1CA2.SelectedIndex = 0;
                cmbP1CA3.SelectedIndex = 0;
            }
        }

        private void FillP2Services()
        {
            cmbP2CA1.Items.Clear();
            cmbP2CA2.Items.Clear();
            cmbP2CA3.Items.Clear();

            List<CASESP2Entity> SP_P2CAMS_Details = _model.SPAdminData.Browse_CASESP2(((ListItem)cmbP2Sp.SelectedItem).Value.ToString(), null, null, null, "CASE4006");

            if (SP_P2CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_P2CAMS_Details.FindAll(u => u.ServPlan == ((ListItem)cmbP2Sp.SelectedItem).Value.ToString() && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        cmbP2CA1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        cmbP2CA2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        cmbP2CA3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));

                    }
                }
            }

            if (cmbP2CA1.Items.Count > 0)
            {
                cmbP2CA1.SelectedIndex = 0;
                cmbP2CA2.SelectedIndex = 0;
                cmbP2CA3.SelectedIndex = 0;
            }
        }

        private void FillP3Services()
        {
            cmbP3CA1.Items.Clear();
            cmbP3CA2.Items.Clear();
            cmbP3CA3.Items.Clear();

            List<CASESP2Entity> SP_P3CAMS_Details = _model.SPAdminData.Browse_CASESP2(((ListItem)cmbP3Sp.SelectedItem).Value.ToString(), null, null, null, "CASE4006");

            if (SP_P3CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_P3CAMS_Details.FindAll(u => u.ServPlan == ((ListItem)cmbP3Sp.SelectedItem).Value.ToString() && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        cmbP3CA1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        cmbP3CA2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        cmbP3CA3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));

                    }
                }
            }

            if (cmbP3CA1.Items.Count > 0)
            {
                cmbP3CA1.SelectedIndex = 0;
                cmbP3CA2.SelectedIndex = 0;
                cmbP3CA3.SelectedIndex = 0;
            }
        }

        private void Btn_AutoPost_CA_Click(object sender, EventArgs e)
        {

        }

        private void Btn_AutoPost_MS_Click(object sender, EventArgs e)
        {

        }
        // public List<CEAPCNTLEntity> _CEAPINVEntity = null;
        CEAPCNTLEntity compareCEAPEntity = new CEAPCNTLEntity();
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (IsValidateForm())
            {
                CEAPCNTLEntity Entity = new CEAPCNTLEntity();

                if (Mode == "Add")
                {
                    Entity.CPCT_CODE = "1";
                    Entity.Rec_Type = "I";

                    if (_CEAPMethod != "")
                    {
                        if (_CEAPMethod == "1")
                        {
                            if (_CEAPMaxVUL == "" || _CEAPMaxNonVUL == "")
                            {
                                AlertBox.Show("Please enter Max Vulnarable & Non-Vulnarable Invoice Values", MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else if (_CEAPMethod == "2")
                        {
                            if (lstCeapInvData == null)
                            {
                                AlertBox.Show("Please enter priority Points", MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    else
                    {
                        AlertBox.Show("Please enter Max Vulnarable & Non-Vulnarable Invoice Values", MessageBoxIcon.Warning);
                        return;
                    }

                }
                else
                {
                    Entity.Rec_Type = "U";
                    Entity.CPCT_CODE = CEAPCNTL_List[0].CPCT_CODE;
                }

                Entity.CPCT_YEAR = ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim();//Program_Year;

                Entity.CPCT_PROG_START = dtpProgStart.Value.ToShortDateString();
                Entity.CPCT_PROG_END = dtpProgEnd.Value.ToShortDateString();

                Entity.CPCT_VUL_SP = ((ListItem)CmbSP.SelectedItem).Value.ToString();
                Entity.CPCT_VUL_50 = txtVul50.Text;
                Entity.CPCT_VUL_75 = txtVul75.Text;
                Entity.CPCT_VUL_150 = txtVul150.Text;

                Entity.CPCT_INV_METHOD = _CEAPMethod.ToString();
                Entity.CPCT_VUL_MAX_INV = "";
                Entity.CPCT_NONVUL_MAX_INV = "";
                if (Mode == "Add")
                {
                    if (_CEAPMethod == "1")
                    {
                        Entity.CPCT_VUL_MAX_INV = _CEAPMaxVUL;// txtVulInv.Text;
                        Entity.CPCT_NONVUL_MAX_INV = _CEAPMaxNonVUL; // txtNonVulInv.Text;
                    }
                }
                Entity.CPCT_VUL_PRIM_CA = ((ListItem)cmbVulCA.SelectedItem).Value.ToString();

                Entity.CPCT_NONVUL_SP = ((ListItem)CmbNonVulSP.SelectedItem).Value.ToString();
                Entity.CPCT_NONVUL_50 = txtNonVul50.Text;
                Entity.CPCT_NONVUL_75 = txtNonVul75.Text;
                Entity.CPCT_NONVUL_150 = txtNonVul150.Text;

                Entity.CPCT_NONVUL_PRIM_CA = ((ListItem)cmbNonVulCA.SelectedItem).Value.ToString();

                Entity.CPCT_LUMP_SUM = chkbxLumpsum.Checked ? "Y" : "N";
                Entity.CPCT_HC_CA = ((ListItem)cmbHCService.SelectedItem).Value.ToString();
                Entity.CPCT_VUL_ARR_CA= ((ListItem)cmbVulService.SelectedItem).Value.ToString();
                Entity.CPCT_NONVUL_ARR_CA= ((ListItem)cmbNonVulService.SelectedItem).Value.ToString();

                if (chkbxLumpsum.Checked)
                {
                    Entity.CPCT_LUMP_INTRVAL = ((ListItem)cmbInterval.SelectedItem).Value.ToString();

                    Entity.CPCT_USER_CNTL = chkbUserCntrl.Checked ? "Y" : "N";
                }
   

                Entity.CPCT_VUL_SSI = txtVulSSI.Text.Trim();
                Entity.CPCT_VUL_MTVC = txtVulMTVC.Text.Trim();
                Entity.CPCT_NONVUL_SSI = txtNonVulSSI.Text.Trim();
                Entity.CPCT_NONVUL_MTVC = txtNonVulMTVC.Text.Trim();

                //Added by Sudheer on 11/28/23
                Entity.CPCT_VUL_SNAP = txtVulSNAP.Text.Trim();
                Entity.CPCT_NONVUL_SNAP = txtNVulSNAP.Text.Trim();


                // Vikash added on 05/06/2023
                Entity.CPCT_VUL_BENTYPE = txtVulBenType.Text.Trim();
                Entity.CPCT_NONVUL_BENTYPE = txtNONVulBenType.Text.Trim();
                Entity.CPCT_CRISIS_BENTYPE = txtCrisisBenType.Text.Trim();

                Entity.CPCT_SUBREP_ID = txtSubRepID.Text.Trim();

                Entity.CPCT_HC_SP = ((ListItem)cmbHCSP.SelectedItem).Value.ToString();

                if(((ListItem)cmbP1Sp.SelectedItem).Value.ToString()!="00")
                {
                    Entity.CPCT_P1_SP = ((ListItem)cmbP1Sp.SelectedItem).Value.ToString();
                    Entity.CPCT_P1_CA1 = ((ListItem)cmbP1CA1.SelectedItem).Value.ToString();
                    Entity.CPCT_P1_CA2 = ((ListItem)cmbP1CA2.SelectedItem).Value.ToString();
                    Entity.CPCT_P1_CA3 = ((ListItem)cmbP1CA3.SelectedItem).Value.ToString();
                }
                if (((ListItem)cmbP2Sp.SelectedItem).Value.ToString() != "00")
                {
                    Entity.CPCT_P2_SP = ((ListItem)cmbP2Sp.SelectedItem).Value.ToString();
                    Entity.CPCT_P2_CA1 = ((ListItem)cmbP2CA1.SelectedItem).Value.ToString();
                    Entity.CPCT_P2_CA2 = ((ListItem)cmbP2CA2.SelectedItem).Value.ToString();
                    Entity.CPCT_P2_CA3 = ((ListItem)cmbP2CA3.SelectedItem).Value.ToString();
                }
                if (((ListItem)cmbP3Sp.SelectedItem).Value.ToString() != "00")
                {
                    Entity.CPCT_P3_SP = ((ListItem)cmbP3Sp.SelectedItem).Value.ToString();
                    Entity.CPCT_P3_CA1 = ((ListItem)cmbP3CA1.SelectedItem).Value.ToString();
                    Entity.CPCT_P3_CA2 = ((ListItem)cmbP3CA2.SelectedItem).Value.ToString();
                    Entity.CPCT_P3_CA3 = ((ListItem)cmbP3CA3.SelectedItem).Value.ToString();
                }
                Entity.CPCT_LSTC_OPERATOR = BaseForm.UserID;

                Entity.CPCT_CSBG_CONTRACT_NO = txtCSBGContractNo.Text.Trim();

                compareCEAPEntity = Entity;

                string Strmsg = string.Empty;

                if (_model.SPAdminData.UpdateCEAPCNTL(Entity, out Strmsg))
                {

                    if (Mode == "Add")
                    {
                        if (_CEAPMethod == "2")
                        {

                            CEAPINVEntity delEntity = new CEAPINVEntity();
                            delEntity.CPINV_CPCT_CODE = Strmsg;
                            delEntity.CPINV_YEAR = Entity.CPCT_YEAR;
                            delEntity.Mode = "DELETE";
                            _model.SPAdminData.InsupDelCEAPINV(delEntity);

                            foreach (CEAPINVEntity lstEntity in lstCeapInvData)
                            {
                                lstEntity.CPINV_CPCT_CODE = Strmsg;
                                if (_model.SPAdminData.InsupDelCEAPINV(lstEntity))
                                {

                                }
                            }
                        }

                        AlertBox.Show("Saved Successfully.");
                    }
                    else
                        AlertBox.Show("Updated Successfully.");

                    if (Mode == "Edit")
                        CheckHistoryTableData(compareCEAPEntity);

                    FillControls();
                    panel1.Enabled = false;
                    pnlLIHWAP.Enabled = false;
                    panel11.Enabled = false;
                    if (CEAPCNTL_List.Count > 0)
                    {
                        ToolBarNew.Visible = false;
                        ToolBarEdit.Visible = true;
                        ToolBarDel.Visible = true;
                    }
                    else
                    {
                        ToolBarEdit.Visible = false;
                        ToolBarDel.Visible = false;
                        ToolBarNew.Visible = true;
                    }

                    ToolBarNew.Enabled = true;
                    ToolBarEdit.Enabled = true;
                    ToolBarDel.Enabled = true;
                    cmbYear.Enabled = true;
                }

            }


        }
        public List<CEAPINVEntity> lstCeapInvData = null;
        private bool IsValidateForm()
        {
            bool isValid = true;

            if (dtpProgStart.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpProgStart, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpProgStart, null);
            }

            if (dtpProgEnd.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpProgEnd, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblProgEndDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpProgEnd, null);
            }

            if (dtpProgStart.Checked.Equals(true) && dtpProgEnd.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtpProgStart.Text) && (!string.IsNullOrEmpty(dtpProgEnd.Text)))
                {
                    if (Convert.ToDateTime(dtpProgStart.Text) > Convert.ToDateTime(dtpProgEnd.Text))
                    {
                        _errorProvider.SetError(dtpProgEnd, "It should be greater than Program start date ".Replace(Consts.Common.Colon, string.Empty));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpProgEnd, null);
                    }
                }
            }

            if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbHCSP.SelectedItem).Text.Trim())))
            {
                _errorProvider.SetError(cmbHCSP, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Service Plan".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbHCSP, null);

            if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Text.Trim())))
            {
                _errorProvider.SetError(CmbSP, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Service Plan".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbSP, null);

            if (string.IsNullOrEmpty(txtVul50.Text.Trim()))
            {
                _errorProvider.SetError(txtVul50, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label1.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtVul50, null);

            if (string.IsNullOrEmpty(txtVul75.Text.Trim()))
            {
                _errorProvider.SetError(txtVul75, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label2.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtVul75, null);

            if (string.IsNullOrEmpty(txtVul150.Text.Trim()))
            {
                _errorProvider.SetError(txtVul150, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label3.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtVul150, null);

            //if (string.IsNullOrEmpty(txtVulInv.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtVulInv, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVulMaxInvs.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else _errorProvider.SetError(txtVulInv, null);

            if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Text.Trim())))
            {
                if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbVulCA.SelectedItem).Text.Trim())))
                {
                    _errorProvider.SetError(cmbVulCA, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVulCA.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbVulCA, null);
            }

            if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Text.Trim())))
            {
                if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbVulService.SelectedItem).Text.Trim())))
                {
                    _errorProvider.SetError(cmbVulService, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVulCA.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbVulService, null);


                if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbVulCA.SelectedItem).Text.Trim())) && (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbVulService.SelectedItem).Text.Trim())))
                {
                    if(((Captain.Common.Utilities.ListItem)cmbVulCA.SelectedItem).Value.ToString().Trim()== ((Captain.Common.Utilities.ListItem)cmbVulService.SelectedItem).Value.ToString().Trim())
                    {
                        _errorProvider.SetError(cmbVulService, lblVulCA.Text.Trim() +" and "+ label6.Text.Trim() + " should not be same");
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(cmbVulService, null);
                }

            }


            if (string.IsNullOrEmpty(txtSubRepID.Text.Trim()))
            {
                _errorProvider.SetError(txtSubRepID, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSubRepID.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtSubRepID, null);

            if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbNonVulSP.SelectedItem).Text.Trim())))
            {
                _errorProvider.SetError(CmbNonVulSP, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Service Plan".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbNonVulSP, null);

            if (string.IsNullOrEmpty(txtNonVul50.Text.Trim()))
            {
                _errorProvider.SetError(txtNonVul50, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label9.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtNonVul50, null);

            if (string.IsNullOrEmpty(txtNonVul75.Text.Trim()))
            {
                _errorProvider.SetError(txtNonVul75, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label10.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtNonVul75, null);

            if (string.IsNullOrEmpty(txtNonVul150.Text.Trim()))
            {
                _errorProvider.SetError(txtNonVul150, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label11.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else _errorProvider.SetError(txtNonVul150, null);

            //if (string.IsNullOrEmpty(txtNonVulInv.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtNonVulInv, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNonVulMaximu.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else _errorProvider.SetError(txtNonVulInv, null);

            if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbNonVulSP.SelectedItem).Text.Trim())))
            {
                if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbNonVulCA.SelectedItem).Text.Trim())))
                {
                    _errorProvider.SetError(cmbNonVulCA, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNonVulCA.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbNonVulCA, null);
            }

            if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbNonVulSP.SelectedItem).Text.Trim())))
            {
                if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbNonVulService.SelectedItem).Text.Trim())))
                {
                    _errorProvider.SetError(cmbNonVulService, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNonVulCA.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbNonVulService, null);


                if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbNonVulCA.SelectedItem).Text.Trim())) && (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbNonVulService.SelectedItem).Text.Trim())))
                {
                    if (((Captain.Common.Utilities.ListItem)cmbNonVulCA.SelectedItem).Value.ToString().Trim() == ((Captain.Common.Utilities.ListItem)cmbNonVulService.SelectedItem).Value.ToString().Trim())
                    {
                        _errorProvider.SetError(cmbNonVulService, lblNonVulCA.Text.Trim() + " and " + label7.Text.Trim() + " should not be same");
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(cmbNonVulService, null);
                }

            }

            //if (BaseForm.BaseAgencyControlDetails.AgyShortName != "HCCAA" && BaseForm.BaseAgencyControlDetails.AgyShortName != "RMPC" && BaseForm.BaseAgencyControlDetails.AgyShortName != "CVCAA")
            //{

            //    if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP1Sp.SelectedItem).Text.Trim())))
            //    {
            //        _errorProvider.SetError(cmbP1Sp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP1.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //        _errorProvider.SetError(cmbP1Sp, null);

            //    if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP2Sp.SelectedItem).Text.Trim())))
            //    {
            //        _errorProvider.SetError(cmbP2Sp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //        _errorProvider.SetError(cmbP2Sp, null);

            //    if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP3Sp.SelectedItem).Text.Trim())))
            //    {
            //        _errorProvider.SetError(cmbP3Sp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP1.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //        _errorProvider.SetError(cmbP3Sp, null);

            //    if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP1Sp.SelectedItem).Text.Trim())))
            //    {
            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP1CA1.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP1CA1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP1CA1.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP1CA1, null);

            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP2CA1.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP2CA1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP2CA1.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP2CA1, null);

            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP3CA1.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP3CA1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP3CA1.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP3CA1, null);

            //    }

            //    if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP2Sp.SelectedItem).Text.Trim())))
            //    {
            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP1CA2.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP1CA2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP1CA2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP1CA2, null);

            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP2CA2.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP2CA2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP2CA2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP2CA2, null);

            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP3CA2.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP3CA2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP3CA2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP3CA2, null);

            //    }

            //    if ((!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP3Sp.SelectedItem).Text.Trim())))
            //    {
            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP1CA3.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP1CA3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP1CA3.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP1CA3, null);

            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP2CA3.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP2CA3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP2CA3.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP2CA3, null);

            //        if ((string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbP3CA3.SelectedItem).Text.Trim())))
            //        {
            //            _errorProvider.SetError(cmbP3CA3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblP3CA3.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            isValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(cmbP3CA3, null);
            //    }

                
                
            //}

            return isValid;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(cmbP1Sp, null);
            _errorProvider.SetError(cmbP2Sp, null);
            _errorProvider.SetError(cmbP3Sp, null);
            _errorProvider.SetError(cmbP1CA1, null);
            _errorProvider.SetError(cmbP1CA2, null);
            _errorProvider.SetError(cmbP1CA3, null);
            _errorProvider.SetError(cmbP2CA1, null);
            _errorProvider.SetError(cmbP2CA2, null);
            _errorProvider.SetError(cmbP2CA3, null);
            _errorProvider.SetError(cmbP3CA1, null);
            _errorProvider.SetError(cmbP3CA2, null);
            _errorProvider.SetError(cmbP3CA3, null);
            FillControls();
            panel1.Enabled = false;
            pnlLIHWAP.Enabled = false;
            panel11.Enabled = false;

            ToolBarNew.Enabled = true;
            ToolBarEdit.Enabled = true;
            ToolBarDel.Enabled = true;
            cmbYear.Enabled = true;

            ///** Max invoice control panel hide **/
            //panel6.Size = new System.Drawing.Size(this.Width, 32);
            //panel11.Visible = false;
            ///** Max invoice control panel **/
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void CmbSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbSP.Items.Count > 0)
            {
                FillVulActivityCombo();
            }
        }

        private void CmbNonVulSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbNonVulSP.Items.Count > 0)
            {
                FillNonVulActivityCombo();
            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                CEAPCNTLEntity Entity = new CEAPCNTLEntity();
                Entity.CPCT_CODE = CEAPCNTL_List[0].CPCT_CODE;
                Entity.CPCT_YEAR = CEAPCNTL_List[0].CPCT_YEAR;
                Entity.Rec_Type = "D";

                string StrMsg = string.Empty;
                string strDelItem = CEAPCNTL_List[0].CPCT_YEAR;

                if (_model.SPAdminData.UpdateCEAPCNTL(Entity, out StrMsg))
                {

                    AlertBox.Show("Year : " + strDelItem + " <br/> Deleted Successfully.", MessageBoxIcon.Information, null, ContentAlignment.BottomRight, 0, null, null, true);
                    FillControls();

                    Fill_SP_DropDowns();

                    if (CEAPCNTL_List.Count > 0)
                    {
                        ToolBarNew.Visible = false;
                        ToolBarEdit.Visible = true;
                        ToolBarDel.Visible = true;
                    }
                    else
                    {
                        ToolBarEdit.Visible = false;
                        ToolBarDel.Visible = false;
                        ToolBarNew.Visible = true;
                    }

                }

            }
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearErrorProvider();
            VisibilityCopyButton();
            _CEAPMethod = "";
            _CEAPMaxVUL = "";
            _CEAPMaxNonVUL = "";
            _CEAP27Code = "";
            _CEAP27Year = "";

            FillControls();

            if (CEAPCNTL_List.Count > 0)
            {
                ToolBarNew.Visible = false;
                ToolBarEdit.Visible = true;
                ToolBarDel.Visible = true;
            }
            else
            {
                Fill_SP_DropDowns();
                ToolBarEdit.Visible = false;
                ToolBarDel.Visible = false;
                ToolBarNew.Visible = true;
            }
        }

        private void VisibilityCopyButton()
        {
            if ((int.Parse(DateTime.Now.Month.ToString()) == 7 || int.Parse(DateTime.Now.Month.ToString()) == 8) && ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim() == DateTime.Now.Year.ToString().Trim())
            {
                btnCopy.Visible = true;
                if (Privileges.AddPriv.Equals("false")) { Btn_Save.Visible = false; btnCopy.Visible = false; }
                else if (Privileges.AddPriv.Equals("true")) { Btn_Save.Visible = true; btnCopy.Visible = true; }
            }
            else btnCopy.Visible = false;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (IsValidateForm())
            {
                CEAPCNTLEntity Entity = new CEAPCNTLEntity();

                if (Mode == "Add")
                {
                    Entity.CPCT_CODE = "1";
                    Entity.Rec_Type = "I";
                }
                else
                {
                    Entity.Rec_Type = "U";
                    Entity.CPCT_CODE = CEAPCNTL_List[0].CPCT_CODE;
                }

                Entity.CPCT_YEAR = (int.Parse(((ListItem)cmbYear.SelectedItem).Value.ToString()) + 1).ToString().Trim();//Program_Year;
                string NextYear = Entity.CPCT_YEAR;

                Entity.CPCT_PROG_START = dtpProgStart.Value.ToShortDateString();
                Entity.CPCT_PROG_END = dtpProgEnd.Value.ToShortDateString();

                Entity.CPCT_VUL_SP = ((ListItem)CmbSP.SelectedItem).Value.ToString();
                Entity.CPCT_VUL_50 = txtVul50.Text;
                Entity.CPCT_VUL_75 = txtVul75.Text;
                Entity.CPCT_VUL_150 = txtVul150.Text;
                Entity.CPCT_VUL_MAX_INV = _CEAPMaxVUL;// txtVulInv.Text; //01/31/2024:: Brian wants to remove this both valunarable and non-vulnarable max values from front end
                Entity.CPCT_VUL_PRIM_CA = ((ListItem)cmbVulCA.SelectedItem).Value.ToString();

                Entity.CPCT_LUMP_SUM = chkbxLumpsum.Checked ? "Y" : "N";
                if (chkbxLumpsum.Checked)
                {
                    Entity.CPCT_LUMP_INTRVAL = ((ListItem)cmbInterval.SelectedItem).Value.ToString();

                    Entity.CPCT_USER_CNTL = chkbUserCntrl.Checked ? "Y" : "N";
                }

                Entity.CPCT_HC_CA = ((ListItem)cmbHCService.SelectedItem).Value.ToString();
                Entity.CPCT_VUL_ARR_CA = ((ListItem)cmbVulService.SelectedItem).Value.ToString();
                Entity.CPCT_NONVUL_ARR_CA = ((ListItem)cmbNonVulService.SelectedItem).Value.ToString();

                Entity.CPCT_NONVUL_SP = ((ListItem)CmbNonVulSP.SelectedItem).Value.ToString();
                Entity.CPCT_NONVUL_50 = txtNonVul50.Text;
                Entity.CPCT_NONVUL_75 = txtNonVul75.Text;
                Entity.CPCT_NONVUL_150 = txtNonVul150.Text;
                Entity.CPCT_NONVUL_MAX_INV = _CEAPMaxNonVUL; // txtNonVulInv.Text; //01/31/2024:: Brian wants to remove this both valunarable and non-vulnarable max values from front end
                Entity.CPCT_NONVUL_PRIM_CA = ((ListItem)cmbNonVulCA.SelectedItem).Value.ToString();

                Entity.CPCT_VUL_SSI = txtVulSSI.Text.Trim();
                Entity.CPCT_VUL_MTVC = txtVulMTVC.Text.Trim();
                Entity.CPCT_NONVUL_SSI = txtNonVulSSI.Text.Trim();
                Entity.CPCT_NONVUL_MTVC = txtNonVulMTVC.Text.Trim();

                //Added by Sudheer on 11/28/23
                Entity.CPCT_VUL_SNAP = txtVulSNAP.Text.Trim();
                Entity.CPCT_NONVUL_SNAP = txtNVulSNAP.Text.Trim();

                Entity.CPCT_LSTC_OPERATOR = BaseForm.UserID;

                Entity.CPCT_INV_METHOD = _CEAPMethod;

                string Strmsg = string.Empty;

                if (_model.SPAdminData.UpdateCEAPCNTL(Entity, out Strmsg))
                {
                    /*****************************************************************************************************/
                    if (_CEAPMethod == "2")
                    {
                        if (_model.SPAdminData.CopytoNextYearCEAPINV(_CEAP27Year, _CEAP27Code, NextYear, Strmsg, BaseForm.UserID, "CPYFRM"))
                        {

                        }
                    }
                    /*****************************************************************************************************/
                    AlertBox.Show("Coppied to next year successfully.");
                    FillControls();
                    panel1.Enabled = false;
                    pnlLIHWAP.Enabled = false;
                    panel11.Enabled = false;
                    if (CEAPCNTL_List.Count > 0)
                    {
                        ToolBarNew.Visible = false;
                        ToolBarEdit.Visible = true;
                        ToolBarDel.Visible = true;
                    }
                    else
                    {
                        ToolBarEdit.Visible = false;
                        ToolBarDel.Visible = false;
                        ToolBarNew.Visible = true;
                    }
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbP1Sp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbP1Sp.Items.Count > 0)
            {
                FillP1Services();
            }
        }

        private void cmbP2Sp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbP2Sp.Items.Count > 0)
            {
                FillP2Services();
            }
        }

        private void cmbP3Sp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbP3Sp.Items.Count > 0)
            {
                FillP3Services();
            }
        }

        private void btnMaxInvCntrl_Click(object sender, EventArgs e)
        {
            //ADMN0027_MaxInvCntrlForm frm = new ADMN0027_MaxInvCntrlForm(BaseForm, Privileges, CEAPCNTL_List, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), Mode, this, BaseForm.BaseAgency,
            //    ((ListItem)CmbSP.SelectedItem).Value == null ? "00" : ((ListItem)CmbSP.SelectedItem).Value.ToString(),
            //    ((ListItem)cmbVulCA.SelectedItem) == null ? "00" : ((ListItem)cmbVulCA.SelectedItem).Value.ToString(),
            //    ((ListItem)CmbNonVulSP.SelectedItem).Value == null ? "00" : ((ListItem)CmbNonVulSP.SelectedItem).Value.ToString(),
            //    ((ListItem)cmbNonVulCA.SelectedItem) == null ? "00" : ((ListItem)cmbNonVulCA.SelectedItem).Value.ToString());
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.FormClosed += new FormClosedEventHandler(Frm_FormClosed); ;
            //frm.ShowDialog();
        }

        public string _CEAPMethod = "";
        public string _CEAPMaxVUL = "";
        public string _CEAPMaxNonVUL = "";
        private string _CEAP27Code = "";
        private string _CEAP27Year = "";
        //private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    ADMN0027_MaxInvCntrlForm form = sender as ADMN0027_MaxInvCntrlForm;
        //    if (Mode == "Edit")
        //    {

        //        if (form.DialogResult == DialogResult.OK)
        //        {
        //            FillControls();
        //            panel1.Enabled = false;
        //            pnlLIHWAP.Enabled = false;
        //            panel11.Enabled = false;
        //            if (CEAPCNTL_List.Count > 0)
        //            {
        //                ToolBarNew.Visible = false;
        //                ToolBarEdit.Visible = true;
        //                ToolBarDel.Visible = true;
        //            }
        //            else
        //            {
        //                ToolBarEdit.Visible = false;
        //                ToolBarDel.Visible = false;
        //                ToolBarNew.Visible = true;
        //            }

        //            ToolBarNew.Enabled = true;
        //            ToolBarEdit.Enabled = true;
        //            ToolBarDel.Enabled = true;
        //            cmbYear.Enabled = true;
        //        }
        //    }
        //    else if (Mode == "Add")
        //    {
        //        if (form.DialogResult == DialogResult.OK)
        //        {
        //            _CEAPMethod = form._ominvCEAPMethod.ToString();
        //            if (_CEAPMethod == "1")
        //            {
        //                _CEAPMaxVUL = form._ominvCEAPMaxVUL.ToString();
        //                _CEAPMaxNonVUL = form._ominvCEAPMaxNonVUL.ToString();
        //            }
        //            else if (_CEAPMethod == "2")
        //            {

        //                lstCeapInvData = form._ominvlstCEAPInvData;
        //            }

        //        }
        //    }

        //}

        private void cmbHCSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHCSP.Items.Count > 0)
            {
                FillHCServiceCombo();
            }
        }

        private void FillHCServiceCombo()
        {
            cmbHCService.Items.Clear(); 

            SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(((ListItem)cmbHCSP.SelectedItem).Value.ToString(), null, null, null, "CASE4006");

            if (SP_CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_CAMS_Details.FindAll(u => u.ServPlan == ((ListItem)cmbHCSP.SelectedItem).Value.ToString() && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    bool IsService = true;
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        if (IsService)
                        {
                            cmbHCService.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Black : Color.Red));
                        }
                    }
                }
            }

            if (cmbHCService.Items.Count > 0)
            {
                cmbHCService.SelectedIndex = 0;
            }
            
        }

        private void chkbxLumpsum_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxLumpsum.Checked)
            {
                cmbInterval.Visible = true;
                chkbUserCntrl.Visible = true;
            }
            else
            {
                cmbInterval.Visible = false;
                chkbUserCntrl.Checked = false;
                chkbUserCntrl.Visible = false;
            }
        }

        private void CheckHistoryTableData(CEAPCNTLEntity ceapcntlEntity)
        {
            string strHistoryDetails = "<XmlHistory>";
            bool boolHistory = false;
            bool boolAddressHistory = false;

            CEAPCNTLEntity ceapEntity = ceapcntlEntity;
            //CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, ((ListItem)cmbYear.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty);

            #region CEAP Control

            //Program Year
            if (ceapEntity.CPCT_YEAR != CEAPCNTL_List[0].CPCT_YEAR)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Program Year</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_YEAR + "</OldValue><NewValue>" + ceapEntity.CPCT_YEAR + "</NewValue></HistoryFields>";
            }

            //Program Start Date
            if (LookupDataAccess.Getdate(ceapEntity.CPCT_PROG_START.ToString()) != LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START))
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Program Start Date</FieldName><OldValue>" + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START) + "</OldValue><NewValue>" + LookupDataAccess.Getdate(ceapEntity.CPCT_PROG_START) + "</NewValue></HistoryFields>";
            }

            //Program End Date
            if (LookupDataAccess.Getdate(ceapEntity.CPCT_PROG_END) != LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END))
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Program End Date</FieldName><OldValue>" + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END) + "</OldValue><NewValue>" + LookupDataAccess.Getdate(ceapEntity.CPCT_PROG_END) + "</NewValue></HistoryFields>";
            }

            //Household Crisis Serv Plan
            if (ceapEntity.CPCT_HC_SP != CEAPCNTL_List[0].CPCT_HC_SP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Household Crisis Service Plan</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_HC_SP + "</OldValue><NewValue>" + ceapEntity.CPCT_HC_SP + "</NewValue></HistoryFields>";
            }

            //Post Usage Lump Sum
            if (ceapEntity.CPCT_LUMP_SUM != CEAPCNTL_List[0].CPCT_LUMP_SUM)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Post Usage Lump Sum</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_LUMP_SUM + "</OldValue><NewValue>" + ceapEntity.CPCT_LUMP_SUM + "</NewValue></HistoryFields>";
            }

            //Post Usage Lump Combo
            if (ceapEntity.CPCT_LUMP_INTRVAL != CEAPCNTL_List[0].CPCT_LUMP_INTRVAL)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Post Usage Lump Sum Interval</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_LUMP_INTRVAL + "</OldValue><NewValue>" + ceapEntity.CPCT_LUMP_INTRVAL + "</NewValue></HistoryFields>";
            }

            //User Controlled
            if (ceapEntity.CPCT_USER_CNTL != CEAPCNTL_List[0].CPCT_USER_CNTL)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>User Controlled</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_USER_CNTL + "</OldValue><NewValue>" + ceapEntity.CPCT_USER_CNTL + "</NewValue></HistoryFields>";
            }

            //Crisis Serv for this Plan
            if (ceapEntity.CPCT_HC_CA != CEAPCNTL_List[0].CPCT_HC_CA)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Crisis Service for this Plan</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_HC_CA + "</OldValue><NewValue>" + ceapEntity.CPCT_HC_CA + "</NewValue></HistoryFields>";
            }
            //Crisis Benefit Type
            if (ceapEntity.CPCT_CRISIS_BENTYPE != CEAPCNTL_List[0].CPCT_CRISIS_BENTYPE)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Crisis Benefit Type</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_CRISIS_BENTYPE + "</OldValue><NewValue>" + ceapEntity.CPCT_CRISIS_BENTYPE + "</NewValue></HistoryFields>";
            }

            #region Vulnerable

            //Serv Plan Vulnerable
            if (ceapEntity.CPCT_VUL_SP != CEAPCNTL_List[0].CPCT_VUL_SP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Service Plan (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_SP + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_SP + "</NewValue></HistoryFields>";
            }

            //State Benf Vulnerable
            if (ceapEntity.CPCT_VUL_BENTYPE != CEAPCNTL_List[0].CPCT_VUL_BENTYPE)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>State Benefit Detail Type (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_BENTYPE + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_BENTYPE + "</NewValue></HistoryFields>";
            }

            //0-50 Vulnerable
            if (ceapEntity.CPCT_VUL_50 != CEAPCNTL_List[0].CPCT_VUL_50)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (0-50) (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_50 + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_50 + "</NewValue></HistoryFields>";
            }

            //51-75 Vulnerable
            if (ceapEntity.CPCT_VUL_75 != CEAPCNTL_List[0].CPCT_VUL_75)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (51-75) (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_75 + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_75 + "</NewValue></HistoryFields>";
            }

            //75-150 Vulnerable
            if (ceapEntity.CPCT_VUL_150 != CEAPCNTL_List[0].CPCT_VUL_150)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (75-150) (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_150 + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_150 + "</NewValue></HistoryFields>";
            }

            //SSI Categ Vulnerable
            if (ceapEntity.CPCT_VUL_SSI != CEAPCNTL_List[0].CPCT_VUL_SSI)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (SSI Categorical) (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_SSI + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_SSI + "</NewValue></HistoryFields>";
            }

            //Means Travel Vulnerable
            if (ceapEntity.CPCT_VUL_MTVC != CEAPCNTL_List[0].CPCT_VUL_MTVC)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (Means Tested Veterans Categorical) (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_MTVC + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_MTVC + "</NewValue></HistoryFields>";
            }

            //SNAP CATG Vulnerable
            if (ceapEntity.CPCT_VUL_SNAP != CEAPCNTL_List[0].CPCT_VUL_SNAP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (SNAP Categorical) (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_SNAP + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_SNAP + "</NewValue></HistoryFields>";
            }

            //Prim Serv Vulnerable
            if (ceapEntity.CPCT_VUL_PRIM_CA != CEAPCNTL_List[0].CPCT_VUL_PRIM_CA)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Service for posting from usage (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_PRIM_CA + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_PRIM_CA + "</NewValue></HistoryFields>";
            }

            //Arrears Vulnerable
            if (ceapEntity.CPCT_VUL_ARR_CA != CEAPCNTL_List[0].CPCT_VUL_ARR_CA)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Arrears Service for this Plan (Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_VUL_ARR_CA + "</OldValue><NewValue>" + ceapEntity.CPCT_VUL_ARR_CA + "</NewValue></HistoryFields>";
            }

            #endregion

            #region Non-Vulnerable

            //Serv Plan Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_SP != CEAPCNTL_List[0].CPCT_NONVUL_SP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Service Plan (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_SP + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_SP + "</NewValue></HistoryFields>";
            }

            //State Benf Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_BENTYPE != CEAPCNTL_List[0].CPCT_NONVUL_BENTYPE)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>State Benefit Detail Type (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_BENTYPE + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_BENTYPE + "</NewValue></HistoryFields>";
            }

            //0-50 Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_50 != CEAPCNTL_List[0].CPCT_NONVUL_50)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (0-50) (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_50 + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_50 + "</NewValue></HistoryFields>";
            }

            //51-75 Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_75 != CEAPCNTL_List[0].CPCT_NONVUL_75)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (51-75) (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_75 + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_75 + "</NewValue></HistoryFields>";
            }

            //75-150 Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_150 != CEAPCNTL_List[0].CPCT_NONVUL_150)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (75-150) (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_150 + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_150 + "</NewValue></HistoryFields>";
            }

            //SSI Categ Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_SSI != CEAPCNTL_List[0].CPCT_NONVUL_SSI)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (SSI Categorical) (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_SSI + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_SSI + "</NewValue></HistoryFields>";
            }

            //Means Travel Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_MTVC != CEAPCNTL_List[0].CPCT_NONVUL_MTVC)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (Means Tested Veterans Categorical) (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_MTVC + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_MTVC + "</NewValue></HistoryFields>";
            }

            //SNAP CATG Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_SNAP != CEAPCNTL_List[0].CPCT_NONVUL_SNAP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Poverty OMB (SNAP Categorical) (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_SNAP + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_SNAP + "</NewValue></HistoryFields>";
            }

            //Prim Serv Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_PRIM_CA != CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Service for posting from usage (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_PRIM_CA + "</NewValue></HistoryFields>";
            }

            //Arrears Non-Vulnerable
            if (ceapEntity.CPCT_NONVUL_ARR_CA != CEAPCNTL_List[0].CPCT_NONVUL_ARR_CA)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Arrears Service for this Plan (Non-Vulnerable)</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_NONVUL_ARR_CA + "</OldValue><NewValue>" + ceapEntity.CPCT_NONVUL_ARR_CA + "</NewValue></HistoryFields>";
            }

            #endregion

            #endregion

            #region LIHWAP Control

            //P1 SP
            if (ceapEntity.CPCT_P1_SP != CEAPCNTL_List[0].CPCT_P1_SP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P1 Service Plan</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P1_SP + "</OldValue><NewValue>" + ceapEntity.CPCT_P1_SP + "</NewValue></HistoryFields>";
            }

            //P1 Service 1
            if (ceapEntity.CPCT_P1_CA1 != CEAPCNTL_List[0].CPCT_P1_CA1)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P1 Water/Waste Water</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P1_CA1 + "</OldValue><NewValue>" + ceapEntity.CPCT_P1_CA1 + "</NewValue></HistoryFields>";
            }

            //P1 Service 2
            if (ceapEntity.CPCT_P1_CA2 != CEAPCNTL_List[0].CPCT_P1_CA2)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P1 Multiple Services</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P1_CA2 + "</OldValue><NewValue>" + ceapEntity.CPCT_P1_CA2 + "</NewValue></HistoryFields>";
            }

            //P1 Service 3
            if (ceapEntity.CPCT_P1_CA3 != CEAPCNTL_List[0].CPCT_P1_CA3)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P1 Other Water Services</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P1_CA3 + "</OldValue><NewValue>" + ceapEntity.CPCT_P1_CA3 + "</NewValue></HistoryFields>";
            }

            //P2 SP
            if (ceapEntity.CPCT_P2_SP != CEAPCNTL_List[0].CPCT_P2_SP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P2 Service Plan</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P2_SP + "</OldValue><NewValue>" + ceapEntity.CPCT_P2_SP + "</NewValue></HistoryFields>";
            }

            //P2 Service 1
            if (ceapEntity.CPCT_P2_CA1 != CEAPCNTL_List[0].CPCT_P2_CA1)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P2 Water/Waste Water</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P2_CA1 + "</OldValue><NewValue>" + ceapEntity.CPCT_P2_CA1 + "</NewValue></HistoryFields>";
            }

            //P2 Service 2
            if (ceapEntity.CPCT_P2_CA2 != CEAPCNTL_List[0].CPCT_P2_CA2)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P2 Multiple Services</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P2_CA2 + "</OldValue><NewValue>" + ceapEntity.CPCT_P2_CA2 + "</NewValue></HistoryFields>";
            }

            //P2 Service 3
            if (ceapEntity.CPCT_P2_CA3 != CEAPCNTL_List[0].CPCT_P2_CA3)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P2 Other Water Services</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P2_CA3 + "</OldValue><NewValue>" + ceapEntity.CPCT_P2_CA3 + "</NewValue></HistoryFields>";
            }

            //P3 SP
            if (ceapEntity.CPCT_P3_SP != CEAPCNTL_List[0].CPCT_P3_SP)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P3 Service Plan</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P3_SP + "</OldValue><NewValue>" + ceapEntity.CPCT_P3_SP + "</NewValue></HistoryFields>";
            }

            //P3 Service 1
            if (ceapEntity.CPCT_P3_CA1 != CEAPCNTL_List[0].CPCT_P3_CA1)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P3 Water/Waste Water</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P3_CA1 + "</OldValue><NewValue>" + ceapEntity.CPCT_P3_CA1 + "</NewValue></HistoryFields>";
            }

            //P3 Service 2
            if (ceapEntity.CPCT_P3_CA2 != CEAPCNTL_List[0].CPCT_P3_CA2)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P3 Multiple Services</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P3_CA2 + "</OldValue><NewValue>" + ceapEntity.CPCT_P3_CA2 + "</NewValue></HistoryFields>";
            }

            //P3 Service 3
            if (ceapEntity.CPCT_P3_CA3 != CEAPCNTL_List[0].CPCT_P3_CA3)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>P3 Other Water Services</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_P3_CA3 + "</OldValue><NewValue>" + ceapEntity.CPCT_P3_CA3 + "</NewValue></HistoryFields>";
            }

            #endregion

            //Subs ID
            if (ceapEntity.CPCT_SUBREP_ID != CEAPCNTL_List[0].CPCT_SUBREP_ID)
            {
                boolHistory = true;

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Subrecipient ID</FieldName><OldValue>" + CEAPCNTL_List[0].CPCT_SUBREP_ID + "</OldValue><NewValue>" + ceapEntity.CPCT_SUBREP_ID + "</NewValue></HistoryFields>";
            }

            strHistoryDetails = strHistoryDetails + "</XmlHistory>";
            if (boolHistory)
            {
                CEAPHISTEntity ceapHistEntity = new CEAPHISTEntity();
                ceapHistEntity.CeapHistTblName = "CEAPCNTL";
                ceapHistEntity.CeapHistScreen = "ADMN0027";
                ceapHistEntity.CeapHistSubScr = Privileges.PrivilegeName;
                ceapHistEntity.CeapHistTblKey = BaseForm.BaseAgency + ((ListItem)cmbYear.SelectedItem).Text.ToString();
                ceapHistEntity.CeapHistLstcOperator = BaseForm.UserID;
                ceapHistEntity.CeapHistChanges = strHistoryDetails;

                _model.SPAdminData.InsertCeapHist(ceapHistEntity);
            }

        }
    }
}