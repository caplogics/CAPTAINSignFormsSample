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
    public partial class ProgressNotes_Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

       // public ApplicationNameControl applicationNameControl = new ApplicationNameControl();


        #endregion

        public ProgressNotes_Form(BaseForm baseForm, string mode, PrivilegeEntity privileges, string prog_field_name_key)
        {
            InitializeComponent();
            Refresh_Control = string.Empty;
            BaseForm = baseForm;
            Mode = mode;
            Privileges = privileges;
            //ProgNotes_Entity = prognotes_entity;
            Prog_Field_Name_Key = prog_field_name_key;
            Form_Sub_Code = string.Empty;

            _model = new CaptainModel();

            //if (mode.Equals("Add"))
                this.Text = privileges.PrivilegeName + " Progress Notes - Add";
            ////else
            ////    this.Text = privileges.Program + " Progress Notes - Edit"; 

            GetSelectedProgram();
            Get_Prog_Notes();
            Txt_ProgText.Dock = DockStyle.Fill;
        }

        public ProgressNotes_Form(BaseForm baseForm, string mode, PrivilegeEntity privileges, string prog_field_name_key, string ScreenName, string AppNo, string AppName)
        {
            InitializeComponent();
            Refresh_Control = string.Empty;
            BaseForm = baseForm;
            Mode = mode;
            Privileges = privileges;
            //privileges.Program = ScreenName;
            //ProgNotes_Entity = prognotes_entity;
            Prog_Field_Name_Key = prog_field_name_key;
            Form_Sub_Code = string.Empty;

            _model = new CaptainModel();

            this.Text = privileges.PrivilegeName + " Progress Notes - Add";


            GetSelectedProgramAppName(AppNo, AppName);
            Get_Prog_Notes(ScreenName);
            Txt_ProgText.Dock = DockStyle.Fill;
        }

        public ProgressNotes_Form(BaseForm baseForm, string mode, PrivilegeEntity privileges, string prog_field_name_key, string form_sub_code)
        {
            InitializeComponent();
            Refresh_Control = string.Empty;
            BaseForm = baseForm;
            Mode = mode;
            Privileges = privileges;
            //ProgNotes_Entity = prognotes_entity;
            Prog_Field_Name_Key = prog_field_name_key;
            Form_Sub_Code = form_sub_code;

            _model = new CaptainModel();

            //if (mode.Equals("Add"))
            this.Text = privileges.PrivilegeName + " Progress Notes - Add";
            ////else
            ////    this.Text = privileges.Program + " Progress Notes - Edit"; 

            GetSelectedProgram();
            Get_Prog_Notes();

            Txt_ProgText.Dock = DockStyle.Fill;
        }

        public ProgressNotes_Form(BaseForm baseForm, string mode, PrivilegeEntity privileges, string prog_field_name_key, string form_sub_code, string ScreenName, string AppNo, string AppName)
        {
            InitializeComponent();
            Refresh_Control = string.Empty;
            BaseForm = baseForm;
            Mode = mode;
            Privileges = privileges;
            //privileges.Program = ScreenName;
            Prog_Field_Name_Key = prog_field_name_key;
            Form_Sub_Code = form_sub_code;

            _model = new CaptainModel();

            this.Text = privileges.PrivilegeName + " Progress Notes - Add";

            GetSelectedProgramAppName(AppNo, AppName);
            Get_Prog_Notes(ScreenName);

            Txt_ProgText.Dock = DockStyle.Fill;
        }

        public ProgressNotes_Form(BaseForm baseForm, string mode, PrivilegeEntity privileges, string prog_field_name_key,List<string> ProgFieldKeys,string Operation,List<CommonEntity> CAList)
        {
            InitializeComponent();
            Refresh_Control = string.Empty;
            BaseForm = baseForm;
            Mode = mode;
            Privileges = privileges;
            //ProgNotes_Entity = prognotes_entity;
            Prog_Field_Name_Key = prog_field_name_key;
            Form_Sub_Code = string.Empty;
            Prog_Fileds = ProgFieldKeys;
            Param = Operation;
            SelCA_List = CAList;

            _model = new CaptainModel();

            //if (mode.Equals("Add"))
            this.Text = privileges.PrivilegeName + " Progress Notes - Add";
            ////else
            ////    this.Text = privileges.Program + " Progress Notes - Edit"; 

            gvCA.Visible = true;
            //this.gvCA.Location = new System.Drawing.Point(0, 25);
            Txt_ProgText.Visible = false;

            CommonEntity CAMSLIST = SelCA_List.Find(u => u.Extension.Equals("MS"));
            if(CAMSLIST!=null)
                label3.Text = "Select Service/Outcome(s) for this Progress Note";
            else
                label3.Text = "Select Service(s) for this Progress Note";

            //btnAdd.Visible = false;
            chkSelectAll.Visible = true;
            chkSelectAll.Checked = false;
            GetSelectedProgram();
            FillCAGrid();
            //Get_Prog_Notes();

            //this.Size = new Size(this.Width, this.Height - Txt_ProgText.Height);
            gvCA.Dock=DockStyle.Fill;
        }

        string Param = string.Empty;
        private void ProgressNotes_Form_Load(object sender, EventArgs e)
        {
            Txt_ProgEdit.Enabled = false;
            if (Privileges.AddPriv.Equals("false") &&
                Privileges.ChangePriv.Equals("false"))
            {
              btnAdd.Visible= false;
            }
        }


        void RefreshCaseNotes()
        {
            GetSelectedProgram();
            Get_Prog_Notes();
            Txt_ProgEdit.Clear();
        }

        private void GetSelectedProgram()
        {
            //if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
            //{
            //    MainMenuControl mainMenuControl = (BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl);
            //    Hierarchy = mainMenuControl.Agency + mainMenuControl.Dept + mainMenuControl.Program;
            //    Year = "    ";
            //    if (!string.IsNullOrEmpty(mainMenuControl.ProgramYear))
            //        Year = mainMenuControl.ProgramYear;
                App_No = LblAppNo.Text = BaseForm.BaseApplicationNo;
                App_Name = LblApp_Name.Text = BaseForm.BaseApplicationName;
            //}
        }

        private void GetSelectedProgramAppName(string Appno,string AppName)
        {
            //if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
            //{
            //    MainMenuControl mainMenuControl = (BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl);
            //    Hierarchy = mainMenuControl.Agency + mainMenuControl.Dept + mainMenuControl.Program;
            //    Year = "    ";
            //    if (!string.IsNullOrEmpty(mainMenuControl.ProgramYear))
            //        Year = mainMenuControl.ProgramYear;
            App_No = LblAppNo.Text = Appno;
            App_Name = LblApp_Name.Text = AppName;
            //}
        }

        private void Get_Prog_Notes()
        {

            string Prog_Code = Privileges.Program;
            if (Prog_Code.ToUpper() != "CASE2001")
            {
                if (Prog_Field_Name_Key.Contains("CA"))
                    Form_Sub_Code = "CA";
                else
                    if (Prog_Field_Name_Key.Contains("MS"))
                    Form_Sub_Code = "MS";

                switch (Form_Sub_Code)
                {
                    case "CA":
                        Prog_Code = "CASE00063";
                        break;
                    case "MS":
                        Prog_Code = "CASE00064";
                        break;
                    case "CONT":
                        if (Privileges.Program == "CASE1006")
                            Prog_Code = "CASE10061";
                        else
                            Prog_Code = "CASE00061";
                        break;
                    case "REFERRED":
                        Prog_Code = "CASE00065";
                        break;
                    default:
                        if (!(Prog_Code.Contains("EMS")))
                        {
                            if (Privileges.Program == "CASE1006")
                                Prog_Code = "CASE10061";
                            else
                                Prog_Code = "CASE00061";
                        }
                        break;
                }
            }
            ProgNotes_Entity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Prog_Code, Prog_Field_Name_Key); //Hierarchy + Year + App_No + Prog_Field_Name_Key);

            if (ProgNotes_Entity.Count > 0)
                Txt_ProgText.Text = ProgNotes_Entity[0].Data;
        }

        private void Get_Prog_Notes(string Screename)
        {

            string Prog_Code = Screename;
            if (Prog_Code.ToUpper() != "CASE2001")
            {
                if (Prog_Field_Name_Key.Contains("CA"))
                    Form_Sub_Code = "CA";
                else
                    if (Prog_Field_Name_Key.Contains("MS"))
                        Form_Sub_Code = "MS";

                switch (Form_Sub_Code)
                {
                    case "CA": Prog_Code = "CASE00063"; break;
                    case "MS": Prog_Code = "CASE00064"; break;
                    case "CONT":
                        if (Screename == "CASE1006") Prog_Code = "CASE10061";
                        else Prog_Code = "CASE00061"; break;
                    case "REFERRED": Prog_Code = "CASE00065"; break;
                    default: if (!(Prog_Code.Contains("EMS"))) { if (Screename == "CASE1006") Prog_Code = "CASE10061";  else Prog_Code = "CASE00061"; } break;
                }
            }
            ProgNotes_Entity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Prog_Code, Prog_Field_Name_Key); //Hierarchy + Year + App_No + Prog_Field_Name_Key);

            if (ProgNotes_Entity.Count > 0)
                Txt_ProgText.Text = ProgNotes_Entity[0].Data;
        }

        private void FillCAGrid()
        {
            if(SelCA_List.Count>0)
            {
                int rowIndex = 0;
                SelCA_List = SelCA_List.OrderBy(u => u.Extension.Trim()).ThenBy(u=> u.Desc.Trim()).ToList();
                foreach (CommonEntity Entity in SelCA_List)
                {
                    rowIndex = gvCA.Rows.Add(false, Entity.Code.Trim(), Entity.Desc.Trim(), Entity.Hierarchy.Trim(),Entity.Extension.Trim());
                    if(Entity.Extension=="MS")
                    {
                        gvCA.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }

            }
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Hierarchy { get; set; }

        public string Year { get; set; }

        public string App_No { get; set; }

        public string App_Name { get; set; }

        public string Prog_Field_Name_Key { get; set; }

        public string Form_Sub_Code { get; set; }

        public List<CaseNotesEntity> ProgNotes_Entity { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<string> Prog_Fileds { get; set; }

        public List<CommonEntity> SelCA_List { get; set; }


        public DataGridViewContentAlignment Alignment { get; set; }

        #endregion

        string Sql_SP_Result_Message = string.Empty;

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (Param == "QuickPost")
            {
                if (!string.IsNullOrEmpty(Txt_ProgEdit.Text.Trim()))
                {
                    //Added by Sudheer on 04/29/2021
                    bool IsSelected = false;
                    if (gvCA.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow dr in gvCA.Rows)
                        {
                            if (dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
                            {
                                IsSelected = true; break;
                            }
                        }
                    }

                    if(!IsSelected)
                    {
                        MessageBox.Show("No selection has been made. Would you like to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question /*,Add_PROGNotes_For_CAMS, true*/);
                    }
                    else
                    {
                        if (gvCA.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow dr in gvCA.Rows)
                            {
                                if (dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
                                {
                                    CaseNotesEntity NotesDetails = new CaseNotesEntity();
                                    if (dr.Cells["CAMS_Type"].Value.ToString() == "CA")
                                        NotesDetails.ScreenName = "CASE00063";
                                    else if (dr.Cells["CAMS_Type"].Value.ToString() == "MS")
                                        NotesDetails.ScreenName = "CASE00064";
                                    NotesDetails.FieldName = dr.Cells["Notes_key"].Value.ToString();
                                    //caseNotesDetails.AppliCationNo = App_No;
                                    NotesDetails.AppliCationNo = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ") + BaseForm.BaseApplicationNo;
                                    string strmiddlename = BaseForm.UserProfile.MI == string.Empty ? "" : " " + BaseForm.UserProfile.MI;
                                    //**NotesDetails.Data = Txt_ProgEdit.Text + "\n ____________________________Note Added by: " + BaseForm.UserProfile.LastName + " " + BaseForm.UserProfile.FirstName + strmiddlename + " on ";
                                    //NotesDetails.Data = "Added by: " + BaseForm.UserProfile.LastName + " " + BaseForm.UserProfile.FirstName + strmiddlename + " "+DateTime.Now.ToString()+"" + "\n"  + Txt_ProgEdit.Text;

                                    NotesDetails.Data =  Txt_ProgEdit.Text +" - "+"Added by: " + BaseForm.UserProfile.LastName + " " + BaseForm.UserProfile.FirstName + strmiddlename + " " + DateTime.Now.ToString();

                                    NotesDetails.LstcOperation = NotesDetails.AddOperator = BaseForm.UserID;
                                    NotesDetails.Mode = "I";

                                    _model.SPAdminData.UpdatePROGNOTES(NotesDetails, "Insert", out Sql_SP_Result_Message);
                                }
                            }

                            btnAdd.Visible = true;
                            Btn_Save.Visible = false;
                            Btn_Cancel.Visible = false;
                            Txt_ProgEdit.Enabled = false;
                            RefreshCaseNotes();
                        }
                    }
                }
                else
                    AlertBox.Show("Progress Notes Text should not be Blank or Empty",MessageBoxIcon.Warning);
            }
            else
            {

                if (!string.IsNullOrEmpty(Txt_ProgEdit.Text.Trim()))
                {
                    CaseNotesEntity caseNotesDetails = new CaseNotesEntity();

                    if (Prog_Field_Name_Key.Contains("CA"))
                        Form_Sub_Code = "CA";
                    else
                        if (Prog_Field_Name_Key.Contains("MS"))
                        Form_Sub_Code = "MS";

                    string Prog_Code = Privileges.Program;
                    if (Prog_Code.ToUpper() != "CASE2001")
                    {
                        switch (Form_Sub_Code)
                        {
                            case "CA": Prog_Code = "CASE00063"; break;
                            case "MS": Prog_Code = "CASE00064"; break;
                            case "CONT": if (Privileges.Program == "CASE1006") Prog_Code = "CASE10061";
                                    else Prog_Code = "CASE00061"; break;
                            case "REFERRED": Prog_Code = "CASE00065"; break;
                            default: if (!(Prog_Code.Contains("EMS"))) { if (Privileges.Program == "CASE1006") Prog_Code = "CASE10061"; else Prog_Code = "CASE00061"; } break;
                        }
                    }

                    caseNotesDetails.ScreenName = Prog_Code;



                    //caseNotesDetails.FieldName = (Hierarchy + Year + App_No + Prog_Field_Name_Key).Trim();
                    caseNotesDetails.FieldName = (Prog_Field_Name_Key).Trim();
                    //caseNotesDetails.AppliCationNo = App_No;
                    caseNotesDetails.AppliCationNo = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ") + BaseForm.BaseApplicationNo;
                    string strmiddlename = BaseForm.UserProfile.MI == string.Empty ? "" : " " + BaseForm.UserProfile.MI;
                    //**caseNotesDetails.Data = Txt_ProgEdit.Text + "\n ____________________________Note Added by: " + BaseForm.UserProfile.LastName + " " + BaseForm.UserProfile.FirstName + strmiddlename + " on ";

                    //caseNotesDetails.Data = "Added by: " + BaseForm.UserProfile.LastName + " " + BaseForm.UserProfile.FirstName + strmiddlename + " "+DateTime.Now.ToString()+" " + "\n" + Txt_ProgEdit.Text;
                    caseNotesDetails.Data = Txt_ProgEdit.Text + " - " + "Added by: " + BaseForm.UserProfile.LastName + " " + BaseForm.UserProfile.FirstName + strmiddlename + " " + DateTime.Now.ToString();

                    caseNotesDetails.LstcOperation = caseNotesDetails.AddOperator = BaseForm.UserID;
                    caseNotesDetails.Mode = "I";

                    if (_model.SPAdminData.UpdatePROGNOTES(caseNotesDetails, "Insert", out Sql_SP_Result_Message))
                    {


                        btnAdd.Visible = true;
                        Btn_Save.Visible = false;
                        Btn_Cancel.Visible = false;
                        Txt_ProgEdit.Enabled = false;
                        RefreshCaseNotes();
                        //this.DialogResult = DialogResult.OK;
                        //this.Close();
                    }
                    else
                    {
                        AlertBox.Show("Exception: " + Sql_SP_Result_Message, MessageBoxIcon.Warning);
                    }
                }
                else
                    AlertBox.Show("Progress Notes Text should not be Blank or Empty", MessageBoxIcon.Warning);
            }
        }


        private void Add_PROGNotes_For_CAMS(object sender, EventArgs e)
        {
            Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            if (senderForm != null)
            {
                if (senderForm.DialogResult.ToString() == "Yes")
                {
                   
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }


        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Btn_Print_Click(object sender, EventArgs e)
        {
            CaseNotesPrintPreview caseNotesPrintPreview = new CaseNotesPrintPreview(BaseForm, Privileges,BaseForm.BaseYear,BaseForm.BaseApplicationNo,BaseForm.BaseApplicationName);
            caseNotesPrintPreview.StartPosition = FormStartPosition.CenterScreen;
            caseNotesPrintPreview.ShowDialog();
        }

        private void Txt_ProgText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Txt_ProgEdit.Clear();
            btnAdd.Visible = true;
            Btn_Save.Visible = false;
            Btn_Cancel.Visible = false;
            Txt_ProgEdit.Enabled = false;
            Refresh_Control = "Close";
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            if(Mode == "Add")
                this.DialogResult = DialogResult.OK;
            else if(Mode!="Add")
            {
                if (ProgNotes_Entity.Count>0)
                {
                    if (!string.IsNullOrEmpty(ProgNotes_Entity[0].Data.Trim()))
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            Refresh_Control = "Close";
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {           
            btnAdd.Visible = false;
            Btn_Save.Visible = true;
            Btn_Cancel.Visible = true;            
            Txt_ProgEdit.Enabled = true;
            Txt_ProgEdit.Focus();
        }

        Wisej.Web.Form _progressNotesForm;
        string Refresh_Control = string.Empty;
        private void ProgressNotes_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Btn_Save.Visible)
            {
                if (string.IsNullOrEmpty(Refresh_Control))
                {

                    DialogResult result = MessageBox.Show("Record not saved. Are you sure want to close?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question/*, MessageBoxHandlerFormClosed, true*/);
                    _progressNotesForm = (Wisej.Web.Form)sender;
                    if (result == DialogResult.Yes)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;

                    }
                }
            }

        }

       
       
        private void MessageBoxHandlerFormClosed(object sender, EventArgs e)
        {
            // Get Wisej.Web.Form object that called MessageBox
            Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;
            // senderForm.FormClosing -= SampleTestForm_FormClosing;
            if (senderForm != null)
            {
                // Set DialogResult value of the Form as a text for label
                if (senderForm.DialogResult.ToString().ToUpper() == "YES")
                {
                    _progressNotesForm.FormClosing -= ProgressNotes_Form_FormClosing;
                    _progressNotesForm.Close();
                }
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if(chkSelectAll.Checked)
            {
                foreach (DataGridViewRow  gvrow in gvCA.Rows)
                {
                    gvrow.Cells["CA_Sel"].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow gvrow in gvCA.Rows)
                {
                    gvrow.Cells["CA_Sel"].Value = false;
                }
            }
        }
    }
}