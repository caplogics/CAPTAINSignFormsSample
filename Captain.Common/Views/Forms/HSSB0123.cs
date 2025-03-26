#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
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
using System.Xml;
using System.IO;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB0123 : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public HSSB0123(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();

            this.Text = /*Privileges.Program + " - " +*/ Privileges.PrivilegeName.Trim();
            txtFrom.Validator = TextBoxValidation.IntegerValidator;
            txtTo.Validator = TextBoxValidation.IntegerValidator;
            txtBmiFrm.Validator = TextBoxValidation.FloatValidator;
            txtBmiTo.Validator = TextBoxValidation.FloatValidator;
            FillPercentages();
            FillActive_Inactive();
            TracksSelctionList();
        }

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Calling_ID { get; set; }

        public string Calling_UserID { get; set; }

        public string propReportPath { get; set; }

        public DataTable propDatatableBMI { get; set; }

        public DataTable propDatatableWeightAge { get; set; }

        public DataTable propDatatableWeightStat { get; set; }

        public DataTable propDatatableLenghtAge { get; set; }

        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<ChldTrckEntity> propTasks { get; set; }
        public List<ChldTrckEntity> TrackList { get; set; }
        public List<ChldMediEntity> propMediTasks { get; set; }
        
        #endregion

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        string Program_Year;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                Txt_HieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
                Get_NameFormat_For_Agencirs(Agy);
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
            {
                FillYearCombo(Agy, Dept, Prog, Year);
                rdoMultipleSites.Enabled = true;
            }
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(910, 25);
                //rdoMultipleSites.Enabled = false;
            }
        }

        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencirs(string Agency)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }
            }

        }

        
        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear)) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }

            Agency = Agy; Depart = Dept; Program = Prog; strYear = Year;

            //fillBusCombo(Agency, Depart, Program, strYear);
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(830, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(910, 25);
        }


        private void FillActive_Inactive()
        {
            cmbActive.Items.Clear();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("Active", "Y"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Inactive", "N"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Both", "B"));

            cmbActive.Items.AddRange(listItem.ToArray());
            cmbActive.SelectedIndex = 2;
        }
        
        
        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = string.Empty;
                Save_Entity.Card_3 = string.Empty;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(rbHeight, null);
            _errorProvider.SetError(rbWeight, null);
            _errorProvider.SetError(rdoMultipleSites, null);
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }


        private void TracksSelctionList()
        {
            TrackList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
            SeltrackList = TrackList.FindAll(u => u.GCHARTCODE.Trim().Equals("HT") && u.GCHARTSEL.Trim().Equals("Y"));
            SeltrackWeightList = TrackList.FindAll(u => u.GCHARTCODE.Trim().Equals("WT") && u.GCHARTSEL.Trim().Equals("Y"));
            SelTaskOtherList = TrackList.FindAll(u => u.GCHARTCODE.Trim().Equals("HC") && u.GCHARTSEL.Trim().Equals("Y"));

            if (SeltrackList.Count > 0) rbHeight.Checked = true; else rbHeight.Checked = false;
            if (SeltrackWeightList.Count > 0) rbWeight.Checked = true; else rbWeight.Checked = false;
            if (SelTaskOtherList.Count > 0) rbHeadCircum.Checked = true; else rbHeadCircum.Checked = false;
        }
       
        List<ChldTrckEntity> SeltrackList = new List<ChldTrckEntity>();
        private void TaskForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SeltrackList = form.GetSelectedTracks();
            }
        }

        
        List<ChldTrckEntity> SeltrackWeightList = new List<ChldTrckEntity>();
        private void TaskWeightForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SeltrackWeightList = form.GetSelectedTracks();
            }
        }

        

        List<ChldTrckEntity> SelTaskOtherList = new List<ChldTrckEntity>();
        private void TaskOtherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelTaskOtherList = form.GetSelectedTracks();
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    
                    rdoMultipleSites.Checked = false; 
                    gvChart.Rows.Clear();
                }
            }
        }

        private void rbFundSel_Click(object sender, EventArgs e)
        {
            if (rbFundSel.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        private void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelFundingList = form.GetSelectedFundings();
            }
        }

        private void rdoMultipleSites_Click(object sender, EventArgs e)
        {
            if (rdoMultipleSites.Checked == true)
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", Sel_REFS_List, "Report", Agency.Trim(), Depart.Trim(), Program.Trim(), Program_Year, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(On_Room_Select_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
        }

        List<CaseSiteEntity> Sel_REFS_List = new List<CaseSiteEntity>();
        private void On_Room_Select_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_REFS_List = form.GetSelected_Sites();
                //if (Sel_REFS_List.Count > 0)
                //{
                //    CommonFunctions.SetComboBoxValue(cmbEnrlStatus, "E");
                //    cmbEnrlStatus.Enabled = false;
                //}
            }
        }

        private void rbHeight_Click(object sender, EventArgs e)
        {
            if (rbHeight.Checked == true)
            {
                HSSB2111FundForm TaskForm = new HSSB2111FundForm(BaseForm, SeltrackList, Privileges, "0000", string.Empty, "HT");
                TaskForm.FormClosed += new FormClosedEventHandler(TaskForm_FormClosed);
                TaskForm.StartPosition = FormStartPosition.CenterScreen;
                TaskForm.ShowDialog();
            }
        }

        private void rbWeight_Click(object sender, EventArgs e)
        {
            if (rbWeight.Checked == true)
            {
                HSSB2111FundForm TaskWeightForm = new HSSB2111FundForm(BaseForm, SeltrackWeightList, Privileges, "0000", string.Empty, "WT");
                TaskWeightForm.FormClosed += new FormClosedEventHandler(TaskWeightForm_FormClosed);
                TaskWeightForm.StartPosition = FormStartPosition.CenterScreen;
                TaskWeightForm.ShowDialog();
            }
        }

        private void rbHeadCircum_Click(object sender, EventArgs e)
        {
            if (rbHeadCircum.Checked == true)
            {
                HSSB2111FundForm TaskHeadForm = new HSSB2111FundForm(BaseForm, SelTaskOtherList, Privileges, "0000", string.Empty, "HC");
                TaskHeadForm.FormClosed += new FormClosedEventHandler(TaskOtherForm_FormClosed);
                TaskHeadForm.StartPosition = FormStartPosition.CenterScreen;
                TaskHeadForm.ShowDialog();
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (rdoMultipleSites.Checked == true)
            {
                if (Sel_REFS_List.Count == 0)
                {
                    _errorProvider.SetError(rdoMultipleSites, "Please Select at least One Site");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoMultipleSites, null);
                }
            }
            else
            {
                if (Sel_REFS_List.Count == 0)
                {
                    _errorProvider.SetError(rdoMultipleSites, "Please Select at least One Site");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoMultipleSites, null);
                }
            }

            
            if (rbFundSel.Checked == true)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(rbFundSel, "Please Select at least One Site");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rbFundSel, null);
                }
            }

            if (rbHeight.Checked == true)
            {
                if (SeltrackList.Count == 0)
                {
                    _errorProvider.SetError(rbHeight, "Please Select at least One Height Task");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rbHeight, null);
                }
            }
            else
            {
                if (SeltrackList.Count == 0)
                {
                    _errorProvider.SetError(rbHeight, "Please Select at least One Height Task");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rbHeight, null);
                }
            }

            if (rbWeight.Checked == true)
            {
                if (SeltrackWeightList.Count == 0)
                {
                    _errorProvider.SetError(rbWeight, "Please Select at least One Weight Task");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rbWeight, null);
                }
            }
            else
            {
                if (SeltrackWeightList.Count == 0)
                {
                    _errorProvider.SetError(rbWeight, "Please Select at least One Weight Task");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rbWeight, null);
                }
            }

            /*if (!string.IsNullOrEmpty(txtFrom.Text.Trim()) && !string.IsNullOrEmpty(txtTo.Text.Trim()))
            {
                if (int.Parse(txtFrom.Text) > int.Parse(txtTo.Text))
                {
                    _errorProvider.SetError(txtTo, string.Format("From Age May Not Be Greater Than To Age ".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtTo, null);
                }
            }*/

            if (string.IsNullOrEmpty(txtFrom.Text.Trim()) || string.IsNullOrEmpty(txtTo.Text.Trim()))
            {
                if (string.IsNullOrWhiteSpace(txtFrom.Text) || string.IsNullOrWhiteSpace(txtTo.Text))
                {
                    _errorProvider.SetError(txtTo, "Please enter the missing From/To Children Age");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtTo, null);
                }
            }
            else
            {
                if (Convert.ToInt32(txtFrom.Text) > -1 && Convert.ToInt32(txtTo.Text) > -1)
                {
                    if (Convert.ToInt32(txtFrom.Text) > Convert.ToInt32(txtTo.Text))
                    {
                        _errorProvider.SetError(txtTo, "'To Age' should be Greater than or Equal to 'From Age'");
                        isValid = false;
                    }
                }
                else
                {
                    _errorProvider.SetError(txtTo, "Values of Children Age cannot be Negative");
                    isValid = false;
                }
            }
                
            if (!string.IsNullOrEmpty(txtBmiFrm.Text.Trim()) && !string.IsNullOrEmpty(txtBmiTo.Text.Trim()))
            {
                if (float.Parse(txtBmiFrm.Text) > float.Parse(txtBmiTo.Text))
                {
                    _errorProvider.SetError(txtBmiTo, string.Format("BMI From may not be Greater than BMI To".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtBmiTo, null);
                }
            }

            return (isValid);
        }

        string strsiteRoomNames = string.Empty; string strFundingCodes = string.Empty; string strTasks = string.Empty;
        string Htasks = string.Empty, Wtasks = string.Empty, OthTasks = string.Empty;
        private void btnGenGrid_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                string Year = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                if (rdoMultipleSites.Checked == true)
                {
                    strsiteRoomNames = string.Empty;
                    foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                    {
                        if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                        strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                    }
                }
                else
                {
                    CaseSiteEntity searchEntity = new CaseSiteEntity(true);
                    searchEntity.SiteAGENCY = Agency; searchEntity.SiteDEPT = Depart; searchEntity.SitePROG = Program;
                    searchEntity.SiteYEAR = Year;
                    List<CaseSiteEntity> AllSites = _model.CaseMstData.Browse_CASESITE(searchEntity, "Browse");
                    if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                        HierarchyEntity hierarchyEntity = new HierarchyEntity();
                        foreach (HierarchyEntity Entity in userHierarchy)
                        {
                            if (Entity.InActiveFlag == "N")
                            {
                                if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == Program)
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                                { hierarchyEntity = null; strsiteRoomNames = "A"; }
                            }
                        }

                        if (hierarchyEntity != null)
                        {
                            if (hierarchyEntity.Sites.Length > 0)
                            {
                                string[] Sites = hierarchyEntity.Sites.Split(',');

                                for (int i = 0; i < Sites.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                    {
                                        foreach (CaseSiteEntity casesite in AllSites) //Site_List)//ListcaseSiteEntity)
                                        {
                                            if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                            {
                                                if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                                                strsiteRoomNames += casesite.SiteNUMBER + casesite.SiteROOM + casesite.SiteAM_PM;
                                                //break;
                                            }
                                            // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                        }
                                    }
                                }
                                //strsiteRoomNames = hierarchyEntity.Sites;
                            }
                            else
                                strsiteRoomNames = "A";
                        }


                    }
                    else
                        strsiteRoomNames = "A";
                }


                //string strFundingCodes = string.Empty;
                if (rbFundSel.Checked == true)
                {
                    strFundingCodes = string.Empty;
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }
                else
                {
                    strFundingCodes = "A";
                }

                strTasks = string.Empty;
                if (rbHeight.Checked == true)
                {
                    Htasks = string.Empty;
                    foreach (ChldTrckEntity SelTasks in SeltrackList)
                    {
                        if (!Htasks.Equals(string.Empty)) Htasks += ",";
                        Htasks += SelTasks.TASK;
                    }
                }
                if (rbWeight.Checked == true)
                {
                    Wtasks = string.Empty;
                    foreach (ChldTrckEntity SelTasks in SeltrackWeightList)
                    {
                        if (!Wtasks.Equals(string.Empty)) Wtasks += ",";
                        Wtasks += SelTasks.TASK;
                    }
                }

                if (rbHeadCircum.Checked == true)
                {
                    OthTasks = string.Empty;
                    foreach (ChldTrckEntity SelTasks in SelTaskOtherList)
                    {
                        if (!OthTasks.Equals(string.Empty)) OthTasks += " ";
                        OthTasks += SelTasks.TASK;
                    }
                }

                strTasks = Htasks + "," + Wtasks + "," + OthTasks;

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string AppNo = string.Empty; string BaseAge = string.Empty; string Seq = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                string Active_stat = string.Empty;
                if (rdoTodayDate.Checked == true) BaseAge = "T"; else BaseAge = "K";
                if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString().Trim() == "Y")
                    Active_stat = "Y";
                else if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString().Trim() == "N")
                    Active_stat = "N";
                else Active_stat = "B";
                if (rdoName.Checked == true) Seq = "N"; else Seq = "A";
                DataSet ds = DatabaseLayer.ChldTrckDB.GetHSSB0123_Report(Agency, Depart, Program, Year, AppNo, BaseAge, strsiteRoomNames, strFundingCodes, Active_stat, strTasks, txtFrom.Text.Trim(), txtTo.Text.Trim(), txtBmiFrm.Text.Trim(), txtBmiTo.Text.Trim(), Seq);
                DataTable dt = ds.Tables[0];

                if(ds.Tables[1].Rows.Count>0)
                    ErrorTable = ds.Tables[1];

                gvChart.Rows.Clear();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int rowIndex = gvChart.Rows.Add(false, dr["APPNO"].ToString().Trim(), dr["LNAME"].ToString().Trim() + "," + dr["FNAME"].ToString().Trim(), LookupDataAccess.Getdate(dr["HEIGHT1"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT1"].ToString().Trim()), LookupDataAccess.Getdate(dr["HEIGHT2"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT2"].ToString().Trim()), LookupDataAccess.Getdate(dr["HEIGHT3"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT3"].ToString().Trim()), LookupDataAccess.Getdate(dr["HEIGHT4"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT4"].ToString().Trim()), LookupDataAccess.Getdate(dr["HEIGHT5"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT5"].ToString().Trim()), LookupDataAccess.Getdate(dr["HEIGHT6"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT6"].ToString().Trim()), LookupDataAccess.Getdate(dr["HEIGHT7"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT7"].ToString().Trim()), LookupDataAccess.Getdate(dr["HEIGHT8"].ToString().Trim()),
                            LookupDataAccess.Getdate(dr["WEIGHT8"].ToString().Trim()), dr["BMI1"].ToString().Trim(), dr["BMI2"].ToString().Trim(),
                            dr["BMI3"].ToString().Trim(), dr["BMI4"].ToString().Trim(), dr["BMI5"].ToString().Trim(), dr["BMI6"].ToString().Trim(),
                            dr["BMI7"].ToString().Trim(), dr["BMI8"].ToString().Trim(), dr["H1"].ToString().Trim(), dr["H2"].ToString().Trim(), dr["H3"].ToString().Trim(),
                            dr["H4"].ToString().Trim(), dr["H5"].ToString().Trim(), dr["H6"].ToString().Trim(), dr["H7"].ToString().Trim(), dr["H8"].ToString().Trim(),
                            dr["W1"].ToString().Trim(), dr["W2"].ToString().Trim(), dr["W3"].ToString().Trim(), dr["W4"].ToString().Trim(), dr["W5"].ToString().Trim(),
                            dr["W6"].ToString().Trim(), dr["W7"].ToString().Trim(), dr["W8"].ToString().Trim(), dr["APP_SEQ"].ToString().Trim(), dr["AGE"].ToString().Trim(),
                            LookupDataAccess.Getdate(dr["DOB"].ToString().Trim()), dr["SEX"].ToString().Trim(), dr["ESITE"].ToString().Trim(), dr["ROOM"].ToString().Trim(),
                            dr["AMPM"].ToString().Trim(), dr["HAGE1"].ToString().Trim(), dr["WAGE1"].ToString().Trim(), dr["HAGE2"].ToString().Trim(), dr["WAGE2"].ToString().Trim()
                            , dr["HAGE3"].ToString().Trim(), dr["WAGE3"].ToString().Trim(), dr["HAGE4"].ToString().Trim(), dr["WAGE4"].ToString().Trim(), dr["HAGE5"].ToString().Trim()
                            , dr["WAGE5"].ToString().Trim(), dr["HAGE6"].ToString().Trim(), dr["WAGE6"].ToString().Trim(), dr["HAGE7"].ToString().Trim(), dr["WAGE7"].ToString().Trim()
                            , dr["HAGE8"].ToString().Trim(), dr["WAGE8"].ToString().Trim());
                           //, dr["HA1"].ToString().Trim(),dr["WA1"].ToString().Trim(), dr["HA2"].ToString().Trim(), dr["WA2"].ToString().Trim()
                           //, dr["HA3"].ToString().Trim(), dr["WA3"].ToString().Trim(), dr["HA4"].ToString().Trim(), dr["WA4"].ToString().Trim(), dr["HA5"].ToString().Trim()
                           //, dr["WA5"].ToString().Trim(), dr["HA6"].ToString().Trim(), dr["WA6"].ToString().Trim(), dr["HA7"].ToString().Trim(), dr["WA7"].ToString().Trim()
                           //, dr["HA8"].ToString().Trim(), dr["WA8"].ToString().Trim(), dr["HTASK1"].ToString().Trim(), dr["WTASK1"].ToString().Trim(), dr["HTASK2"].ToString().Trim(), dr["WTASK2"].ToString().Trim()
                           //, dr["HTASK3"].ToString().Trim(), dr["WTASK3"].ToString().Trim(), dr["HTASK4"].ToString().Trim(), dr["WTASK4"].ToString().Trim(), dr["HTASK5"].ToString().Trim()
                           //, dr["WTASK5"].ToString().Trim(), dr["HTASK6"].ToString().Trim(), dr["WTASK6"].ToString().Trim(), dr["HTASK7"].ToString().Trim(), dr["WTASK7"].ToString().Trim()
                           //, dr["HTASK8"].ToString().Trim(), dr["WTASK8"].ToString().Trim());

                       gvChart.Rows[rowIndex].Tag = dr;
                    }

                    if (gvChart.Rows.Count > 0)
                    {
                        gvChart.Rows[0].Tag = 0;
                        btnGenerateCharts.Enabled = true; rdoAllChild.Enabled = true; rdoSelChild.Enabled = true;
                    }
                    else
                    {
                        btnGenerateCharts.Enabled = false; rdoAllChild.Enabled = false; rdoSelChild.Enabled = false;
                    }

                    PrintErrorPdf();
                }
            }
        }

        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);


            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string strRepSeq = rdoApp.Checked == true ? "A" : "N";
            string Active = ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString();
            string strSite = rdoMultipleSites.Checked == true ? "M" : "A";
            string strHTask = rbHeight.Checked == true ? "1" : "2";
            string strWTask = rbWeight.Checked == true ? "1" : "2";
            string strOTHTask = rbHeadCircum.Checked == true ? "1" : "2";
            string strShowTask = string.Empty;




            string strBaseAge = rdoTodayDate.Checked ? "T" : "K";
            string strFundSource = rbFundAll.Checked ? "Y" : "N";
            string strsiteRoomNames = string.Empty;
            if (rdoMultipleSites.Checked == true)
            {
                foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                {
                    if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                    strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                }
            }

            string strTasks = string.Empty; string Htask = string.Empty, Wtask = string.Empty, Othtask = string.Empty;
            if (rbHeight.Checked == true)
            {
                foreach (ChldTrckEntity SelTasks in SeltrackList)
                {
                    if (!Htask.Equals(string.Empty)) Htask += ",";
                    Htask += SelTasks.TASK;
                }
            }
            if (rbWeight.Checked == true)
            {
                foreach (ChldTrckEntity SelTasks in SeltrackWeightList)
                {
                    if (!Wtask.Equals(string.Empty)) Wtask += ",";
                    Wtask += SelTasks.TASK;
                }
            }
            if (rbHeadCircum.Checked == true)
            {
                foreach (ChldTrckEntity SelTasks in SelTaskOtherList)
                {
                    if (!Othtask.Equals(string.Empty)) Othtask += ",";
                    Othtask += SelTasks.TASK;
                }
            }

            strTasks = Htask +","+ Wtask +","+ Othtask;

            string strFundingCodes = string.Empty;
            if (rbFundSel.Checked == true)
            {
                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }
            }



            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program + "\" RepSeq = \"" + strRepSeq +
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\" Height = \"" + strHTask + "\" Weight = \"" + strWTask + "\" Other = \"" + strOTHTask + 
                            "\" Active = \"" + Active + "\" Heighttasks = \"" + Htask + "\" weighttasks = \"" + Wtask + "\" Othertasks = \"" + Othtask +
                            "\" AgeFrom = \"" + txtFrom.Text + "\" AgeTo = \"" + txtTo.Text +
                            "\" BMIFrom = \"" + txtBmiFrm.Text + "\" BMITo = \"" + txtBmiTo.Text +
                            "\" BaseAge = \"" + strBaseAge + "\" FundedSource = \"" + strFundSource + "\" ShowTask = \"" + strShowTask + 
                             "\" SiteNames = \"" + strsiteRoomNames  + "\" FundingCode = \"" + strFundingCodes + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(cmbActive, dr["Active"].ToString());

                if (dr["Site"].ToString() == "M")
                {
                    rdoMultipleSites.Checked = true;
                    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart;
                    Search_Entity.SitePROG = Program; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                    propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                    propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteROOM.Trim() != "0000");
                    Sel_REFS_List.Clear();
                    string strSites = dr["SiteNames"].ToString();
                    List<string> siteList = new List<string>();
                    if (strSites != null)
                    {
                        string[] sitesRooms = strSites.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            CaseSiteEntity siteDetails = propCaseSiteEntity.Find(u => u.SiteNUMBER + u.SiteROOM + u.SiteAM_PM == sitesRooms.GetValue(i).ToString());
                            Sel_REFS_List.Add(siteDetails);
                        }
                    }
                }


                Sel_REFS_List = Sel_REFS_List;



                //if (dr["FundedDays"].ToString() == "Y")
                //    rdoAllFundYes.Checked = true;
                //else
                //    rdoAllFundNo.Checked = true;
                if (dr["FundedSource"].ToString() == "Y")
                    rbFundAll.Checked = true;
                else
                {
                    rbFundSel.Checked = true;
                    SelFundingList.Clear();
                    string strFunds = dr["FundingCode"].ToString();
                    List<string> siteList1 = new List<string>();
                    if (strFunds != null)
                    {
                        string[] FundCodes = strFunds.Split(',');
                        for (int i = 0; i < FundCodes.Length; i++)
                        {
                            CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                            SelFundingList.Add(fundDetails);
                        }
                    }
                    SelFundingList = SelFundingList;
                }

                List<ChldTrckEntity> chldTrckList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);

                if(dr["Height"].ToString()=="1")
                {
                    rbHeight.Checked = true;
                   //rbSelTasks.Checked = true;
                    SeltrackList.Clear();
                    string strTasks = dr["Heighttasks"].ToString();
                    List<string> trckList = new List<string>();
                    if (strTasks != null)
                    {
                        string[] sitesRooms = strTasks.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            ChldTrckEntity trckDetails = chldTrckList.Find(u => u.TASK == sitesRooms.GetValue(i).ToString());
                            SeltrackList.Add(trckDetails);
                        }
                    }
                }
                SeltrackList = SeltrackList;

                if (dr["Weight"].ToString() == "1")
                {
                    rbWeight.Checked = true;
                    //rbSelTasks.Checked = true;
                    SeltrackWeightList.Clear();
                    string strTasks = dr["weighttasks"].ToString();
                    List<string> trckList = new List<string>();
                    if (strTasks != null)
                    {
                        string[] sitesRooms = strTasks.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            ChldTrckEntity trckDetails = chldTrckList.Find(u => u.TASK == sitesRooms.GetValue(i).ToString());
                            SeltrackWeightList.Add(trckDetails);
                        }
                    }
                }
                SeltrackWeightList = SeltrackWeightList;

                if (dr["Other"].ToString() == "1")
                {
                    rbHeadCircum.Checked = true;    //**rbWeight.Checked = true;
                    //rbSelTasks.Checked = true;
                    SelTaskOtherList.Clear();
                    string strTasks = dr["Othertasks"].ToString();
                    List<string> trckList = new List<string>();
                    if (strTasks != null)
                    {
                        string[] sitesRooms = strTasks.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            ChldTrckEntity trckDetails = chldTrckList.Find(u => u.TASK == sitesRooms.GetValue(i).ToString());
                            SelTaskOtherList.Add(trckDetails);
                        }
                    }
                }
                SelTaskOtherList = SelTaskOtherList;

                txtBmiFrm.Text = dr["BMIFrom"].ToString();
                txtBmiTo.Text = dr["BMITo"].ToString();
                txtFrom.Text = dr["AgeFrom"].ToString();
                txtTo.Text = dr["AgeTo"].ToString();

                if (dr["BaseAge"].ToString().Trim() == "T")
                    rdoTodayDate.Checked = true;
                else
                    rdoKindergartenDate.Checked = true;

                if (dr["RepSeq"].ToString().Trim() == "A")
                    rdoApp.Checked = true;
                else
                    rdoName.Checked = true;

            }
        }



        PdfContentByte cb;
        //int X_Pos, Y_Pos;

        DataTable ErrorTable = new DataTable();
        string strFolderPath = string.Empty; float X_Pos, Y_Pos;
        string Random_Filename = null; string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
        private void On_Exist_PDF()
        {
            string weightChart = string.Empty; string Heightchart = string.Empty; string BMIchart = string.Empty; string AppName = string.Empty;
            int MONTHS = 0; string RepAppName = string.Empty;

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            //ErrorTable = CreateTableErrorValues();
            string Data = string.Empty;
            if (rdoSelChild.Checked == true) 
            {
                foreach (DataGridViewRow Entity in gvChart.Rows)
                {
                    Data = string.Empty;
                    if (Entity.Cells["dgvChk"].Value != null && Convert.ToBoolean(Entity.Cells["dgvChk"].Value) == true)
                    {
                        AppName = Entity.Cells["NAME"].Value.ToString().Trim();
                        if (Entity.Cells["Seq"].Value.ToString().Trim() == "1")
                            RepAppName = Entity.Cells["NAME"].Value.ToString().Trim().Replace(",","_");
                        else RepAppName = Entity.Cells["NAME"].Value.ToString().Trim().Replace(",", "_") + "_" + (Convert.ToInt32(Entity.Cells["Seq"].Value.ToString().Trim()) - 1).ToString();

                        #region Define the Age and Height Charts

                        if (string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim())
                            && string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim())
                            && string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim())
                            && string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                        {
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString()))
                                MONTHS = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString()))
                                MONTHS = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                            if (MONTHS > 36)
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                    Heightchart = Application.MapPath("~\\Resources\\Pdf\\boys_ht_2-20_yr.pdf");
                                else Heightchart = Application.MapPath("~\\Resources\\Pdf\\girls_ht_2-20_yr.pdf");
                            }
                            else
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                    Heightchart = Application.MapPath("~\\Resources\\Pdf\\boys_ht_0-36_mo.pdf");
                                else Heightchart = Application.MapPath("~\\Resources\\Pdf\\girls_ht_0-36_mo.pdf");
                            }

                            if (!string.IsNullOrEmpty(Heightchart))
                            {
                                PdfReader Hreader = new PdfReader(Heightchart);
                                string PdfName = "ht.pdf";
                                PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                                PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                                Hstamper.Writer.SetPageSize(PageSize.A4);
                                PdfContentByte cb = Hstamper.GetOverContent(1);
                                cb.SetFontAndSize(bf_times, 9);
                                //cb.SetColorFill(BaseColor.CYAN.Darker());
                                cb.BeginText();
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Height", 90, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 150, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 200, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 350, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Height", 390, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                cb.EndText();
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Report Date : " + DateTime.Now.ToShortDateString()), 50, 790, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim()), 350, 790, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("App# : " + Entity.Cells["APPNO"].Value.ToString().Trim()), 50, 777, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Name : " + AppName), 350, 777, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim())), 50, 764, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age  : " + Entity.Cells["AGE"].Value.ToString().Trim()+" Years"), 350, 764, 0);

                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age"), 50, 100, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Height"), 90, 100, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date"), 150, 100, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Percentile"), 200, 100, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age"), 350, 100, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Height"), 390, 100, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date"), 450, 100, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Percentile"), 500, 100, 0);

                                cb.SetFontAndSize(bfTimes, 10);


                                float X_Axis = 0, Y_Axis = 0;
                                if (MONTHS > 36)
                                {
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                    {
                                        cb.SetColorFill(BaseColor.BLUE);
                                        X_Pos = 104.1f; Y_Pos = 150.8f;
                                        X_Axis = 22.1f; Y_Axis = 11.66f;
                                    }
                                    else
                                    {
                                        cb.SetColorFill(BaseColor.RED);
                                        X_Pos = 108.8f; Y_Pos = 149.8f;
                                        X_Axis = 22f; Y_Axis = 11.67f;
                                    }

                                    float pos_x; float pos_y;
                                    float H1Months = 0; float H1answer = 0;

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if(!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);

                                        }
                                    }
                                    
                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);

                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Height_Percentage = 0; string HPercent = "0";
                                    if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H1"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if(Count==1) U_Pos += 300;
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H2"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H3"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 1 || Count == 3) { U_Pos += 300; }
                                            //Count++;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H4"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 1 || Count==3) { U_Pos += 300; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H5"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 1 || Count == 3 || Count==5) { U_Pos += 300; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H6"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count==6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H7"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count==7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H8"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6 || Count==8){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }

                                }
                                else
                                {
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                    {
                                        cb.SetColorFill(BaseColor.BLUE);
                                        X_Pos = 100.5f; Y_Pos = 149.3f;
                                        X_Axis = 11.2f; Y_Axis = 22.5f;
                                    }
                                    else
                                    {
                                        cb.SetColorFill(BaseColor.RED);
                                        X_Pos = 99.3f; Y_Pos = 148.5f;
                                        X_Axis = 11.2f; Y_Axis = 22.5f;
                                    }

                                    float pos_x; float pos_y;
                                    float H1Months = 0; float H1answer = 0;

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());


                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;

                                        if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Height_Percentage = 0; string HPercent = "0";
                                    if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H1"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if(Count==1) U_Pos += 300;
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H2"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H3"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count==3) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H4"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count == 3) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count==4){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H5"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            { U_Pos += 300; }
                                            if (Count == 1 || Count == 3 || Count==5) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H6"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count==6){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H7"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count==7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["H8"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6 || Count==8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }
                                }
                                Hstamper.Close();
                            }

                        }

                        #endregion

                        #region Define Age and Weight Charts
                        if (string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim())
                            && string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim())
                            && string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim())
                            && string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                        {
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString()))
                                MONTHS = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString()))
                                MONTHS = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                            else if (string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());

                            if (MONTHS > 36)
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                    weightChart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_2-20_yr.pdf");
                                else weightChart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_2-20_yr.pdf");
                            }
                            else
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                    weightChart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_0-36_mo.pdf");
                                else weightChart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_0-36_mo.pdf");
                            }

                            if (!string.IsNullOrEmpty(weightChart))
                            {
                                PdfReader Wreader = new PdfReader(weightChart);
                                string PdfName = "wt.pdf";
                                PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                                PdfStamper Wstamper = new PdfStamper(Wreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                                Wstamper.Writer.SetPageSize(PageSize.A4);
                                PdfContentByte cb = Wstamper.GetOverContent(1);

                                cb.SetFontAndSize(bf_times, 9);
                                //cb.SetColorFill(BaseColor.CYAN.Darker());
                                cb.BeginText();
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 90, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 150, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 200, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 350, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 390, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                cb.EndText();
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Report Date : " + DateTime.Now.ToShortDateString()), 50, 790, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim()), 350, 790, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("App# : " + Entity.Cells["APPNO"].Value.ToString().Trim()), 50, 777, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Name : " + AppName), 350, 777, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim())), 50, 764, 0);
                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years"), 350, 764, 0);

                                cb.SetFontAndSize(bfTimes, 10);
                                float X_Axis = 0, Y_Axis = 0;
                                if (MONTHS > 36)
                                {
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                    {
                                        cb.SetColorFill(BaseColor.BLUE);
                                        X_Pos = 100.5f; Y_Pos = 147.8f;
                                        X_Axis = 22.28f; Y_Axis = 2.65f;
                                    }
                                    else
                                    {
                                        cb.SetColorFill(BaseColor.RED);
                                        X_Pos = 94.8f; Y_Pos = 153.3f;
                                        X_Axis = 22.7f; Y_Axis = 2.625f;
                                    }

                                    float pos_x; float pos_y;
                                    float H1Months = 0; float H1answer = 0;

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;

                                            pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Weight_percentile = 0; string WTAge_Percent = "0";
                                    if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W1"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++; 
                                            if(Count==1) U_Pos += 300;
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W2"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                            //Count++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W3"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            Count++;
                                            if (Count == 1 || Count==3) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //Count++;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W4"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count==4){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W5"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count==5) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W6"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count==6){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W7"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count==7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W8"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6 || Count==8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                    {
                                        cb.SetColorFill(BaseColor.BLUE);
                                        X_Pos = 92.5f; Y_Pos = 151f;
                                        X_Axis = 11.47f; Y_Axis = 15.54f;
                                    }
                                    else
                                    {
                                        cb.SetColorFill(BaseColor.RED);
                                        X_Pos = 91.5f; Y_Pos = 147f;
                                        X_Axis = 11.48f; Y_Axis = 15.63f;
                                    }

                                    float pos_x; float pos_y;
                                    float H1Months = 0; float H1answer = 0;

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());


                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                            H1answer = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());

                                            pos_x = (H1Months) * X_Axis + X_Pos;
                                            pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Weight_percentile = 0; string WTAge_Percent = "0";
                                    if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W1"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;

                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10); 
                                            Count++;
                                            if (Count == 1) U_Pos += 300;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W2"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;

                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W3"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count==3) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W4"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count==4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W5"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count==5) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W6"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count==6){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W7"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count==7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["W8"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths;
                                            Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6 || Count==8){ U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                }
                                Wstamper.Close();
                            }

                        }
                        #endregion

                        #region Define the BMI Charts...
                        if (!string.IsNullOrEmpty(Heightchart) && !string.IsNullOrEmpty(weightChart))
                        {
                            MONTHS = 0;
                            if ((!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim())))
                                MONTHS = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                            else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                                MONTHS = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());

                            if (((Entity.Cells["BMI1"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim())) ||
                                ((Entity.Cells["BMI2"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim())) ||
                                ((Entity.Cells["BMI3"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim())) ||
                                ((Entity.Cells["BMI4"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim())) ||
                                ((Entity.Cells["BMI5"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim())) ||
                                ((Entity.Cells["BMI6"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim())) ||
                                ((Entity.Cells["BMI7"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim())) ||
                                ((Entity.Cells["BMI8"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim())))
                            {
                                // Define the BMI Charts...
                                if (MONTHS > 24)
                                {
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        BMIchart = Application.MapPath("~\\Resources\\Pdf\\boys_bmi.pdf");
                                    else BMIchart = Application.MapPath("~\\Resources\\Pdf\\girls_bmi.pdf");

                                    PdfReader Wreader = new PdfReader(BMIchart);
                                    string PdfName = "bmi.pdf";
                                    PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                                    PdfStamper Wstamper = new PdfStamper(Wreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                                    Wstamper.Writer.SetPageSize(PageSize.A4);
                                    PdfContentByte cb = Wstamper.GetOverContent(1);

                                    cb.SetFontAndSize(bf_times, 9);
                                    //cb.SetColorFill(BaseColor.CYAN.Darker());
                                    cb.BeginText();
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "BMI", 90, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 150, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 200, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 350, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "BMI", 390, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                    cb.EndText();

                                    cb.SetFontAndSize(bfTimes, 10);
                                    float X_Axis = 0, Y_Axis = 0;
                                    
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            cb.SetColorFill(BaseColor.BLUE);
                                            X_Pos = 83.8f; Y_Pos = 149.5f;
                                            X_Axis = 23.05f; Y_Axis = 22.48f;
                                        }
                                        else
                                        {
                                            cb.SetColorFill(BaseColor.RED);
                                            X_Pos = 83.8f; Y_Pos = 150.8f;
                                            X_Axis = 23f; Y_Axis = 22.45f;
                                        }

                                        float pos_x; float pos_y;
                                        float H1Months = 0; float H1answer = 0;

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                        {
                                            pos_x = 0; pos_y = 0;
                                            H1answer = float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim());
                                            if (H1answer > 0)
                                            {
                                                H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                                H1Months = H1Months / 12;

                                                pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                                pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                                cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                                cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                                cb.EndText();
                                                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                            }
                                        }

                                        int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float BMI_Perc = 0; string Per_BMI = "0";
                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;

                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10); 
                                                Count++;
                                                if (Count == 1) U_Pos += 300;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;
                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                                if (Count == 1) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 0) { U_Pos = 50; }
                                                Count++;
                                                if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;
                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                                
                                                Count++;
                                                if (Count == 1 || Count == 3 ) { U_Pos += 300; }
                                                else if (Count == 0) { U_Pos = 50; }
                                                else if (Count == 2 ) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;
                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                                //if (Count == 1 || Count == 3) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                                //else if (Count == 0) { U_Pos = 50; }
                                                //else if (Count == 2) { U_Pos += 300; }
                                                Count++;
                                                if (Count == 1 || Count == 3) { U_Pos += 300; }
                                                else if (Count == 0) { U_Pos = 50; }
                                                else if (Count == 2 || Count==4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;
                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                                //if (Count == 1 || Count == 3) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                                //else if (Count == 0) { U_Pos = 50; }
                                                //else if (Count == 2 || Count == 4) { U_Pos += 300; }
                                                Count++;
                                                if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                                else if (Count == 0) { U_Pos = 50; }
                                                else if (Count == 2 || Count == 4 ) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;
                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                                //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                                //else if (Count == 0) { U_Pos = 50; }
                                                //else if (Count == 2 || Count == 4) { U_Pos += 300; }
                                                Count++;
                                                if (Count == 1 || Count == 3 || Count == 5 ) { U_Pos += 300; }
                                                else if (Count == 0) { U_Pos = 50; }
                                                else if (Count == 2 || Count == 4 || Count == 6 ) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;
                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI.ToString(), U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                                //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                                //else if (Count == 0) { U_Pos = 50; }
                                                //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 300; }
                                                Count++;
                                                if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                                else if (Count == 0) { U_Pos = 50; }
                                                else if (Count == 2 || Count == 4 || Count == 6 ) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim()))
                                        {
                                            if (float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()) > 0)
                                            {
                                                CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                                H1Months = CalMonths / 12;
                                                BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                                if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                                cb.SetFontAndSize(bf_times, 9);
                                                cb.BeginText();
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                                cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                                //if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                                //else if (Count == 0) { U_Pos = 50; }
                                                //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 300; }
                                                Count++;
                                                if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                                else if (Count == 0) { U_Pos = 50; }
                                                else if (Count == 2 || Count == 4 || Count == 6 || Count == 8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            }
                                        }

                                    
                                    Wstamper.Close();
                                }
                        

                        #region Define Weight Length Charts
                                if (MONTHS < 36 && MONTHS>0 )
                                {
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        BMIchart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_ln_0-36_mo.pdf");
                                    else BMIchart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_ln_0-36_mo.pdf");

                                    PdfReader Wreader = new PdfReader(BMIchart);
                                    string PdfName = "wt_ln.pdf";
                                    PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                                    PdfStamper Wstamper = new PdfStamper(Wreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                                    Wstamper.Writer.SetPageSize(PageSize.A4);
                                    PdfContentByte cb = Wstamper.GetOverContent(1);

                                    cb.SetFontAndSize(bf_times, 9);
                                    //cb.SetColorFill(BaseColor.CYAN.Darker());
                                    cb.BeginText();
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 90, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Stature", 140, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 190, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 250, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 300, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 340, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Stature", 390, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                    cb.EndText();

                                    cb.SetFontAndSize(bfTimes, 10);
                                    float X_Axis = 0, Y_Axis = 0;
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                    {
                                        cb.SetColorFill(BaseColor.BLUE);
                                        X_Pos = 113.4f; Y_Pos = 172f;
                                        X_Axis = 16.8f; Y_Axis = 11.035f;
                                    }
                                    else
                                    {
                                        cb.SetColorFill(BaseColor.RED);
                                        X_Pos = 114.3f; Y_Pos = 172f;
                                        X_Axis = 16.83f; Y_Axis = 11.05f;
                                    }

                                    float pos_x; float pos_y;
                                    float H1Months = 0; float H1answer = 0;
                                    float lnHeight = 0; float lnWeight = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                    {
                                        pos_x = 0; pos_y = 0;
                                        H1answer = float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim());
                                        if (H1answer > 0)
                                        {
                                            H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                            H1Months = H1Months / 12;
                                            lnHeight = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                            lnWeight = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());

                                            if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 92.8f; X_Axis = 21.5f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 92.3f; X_Axis = 21.1f;
                                                pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                                pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                            }
                                            cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                            cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                            cb.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;


                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10); 
                                            Count++;
                                            if (Count == 1) U_Pos += 250;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 250; else if (Count == 0) { U_Pos = 50; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count==3){ U_Pos += 250; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            
                                            Count++;
                                            if (Count == 1 || Count == 3) { U_Pos += 250; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count==4)  { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            //if (Count == 1 || Count == 3) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //else if (Count == 0) { U_Pos = 50; }
                                            //else if (Count == 2 || Count == 4) { U_Pos += 250; }
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count==5) { U_Pos += 250; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //else if (Count == 0) { U_Pos = 50; }
                                            //else if (Count == 2 || Count == 4) { U_Pos += 250; }
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 250; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count==6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //else if (Count == 0) { U_Pos = 50; }
                                            //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 250; }
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count==7) { U_Pos += 250; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                            H1Months = CalMonths / 12;

                                            cb.SetFontAndSize(bf_times, 9);
                                            cb.BeginText();
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                            cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                            //if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                            //else if (Count == 0) { U_Pos = 50; }
                                            //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 250; }
                                            Count++;
                                            if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 250; }
                                            else if (Count == 0) { U_Pos = 50; }
                                            else if (Count == 2 || Count == 4 || Count == 6 || Count==8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        }
                                    }

                                    Wstamper.Close();
                                }
                                #endregion

                        #region Define Weight Stature charts

                                if (MONTHS > 0)
                                {
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        BMIchart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_st.pdf");
                                    else BMIchart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_st.pdf");

                                    PdfReader WSreader = new PdfReader(BMIchart);
                                    string PdfName1 = "wt_st.pdf";
                                    PdfName1 = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName1;
                                    PdfStamper WSstamper = new PdfStamper(WSreader, new FileStream(PdfName1, FileMode.Create, FileAccess.Write));
                                    WSstamper.Writer.SetPageSize(PageSize.A4);
                                    PdfContentByte cbWS = WSstamper.GetOverContent(1);

                                    cbWS.SetFontAndSize(bf_times, 9);
                                    //cb.SetColorFill(BaseColor.CYAN.Darker());
                                    cbWS.BeginText();
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 90, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Length", 140, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Date", 190, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 250, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Age", 300, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 340, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Length", 390, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                    cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                    cbWS.EndText();

                                    cbWS.SetFontAndSize(bfTimes, 10);
                                    float X_Axis1 = 0; float Y_Axis1 = 0;
                                    if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                    {
                                        cbWS.SetColorFill(BaseColor.BLUE);
                                        X_Pos = 111.3f; Y_Pos = 187.9f;
                                        X_Axis1 = 23.05f; Y_Axis1 = 9.175f;
                                    }
                                    else
                                    {
                                        cbWS.SetColorFill(BaseColor.RED);
                                        X_Pos = 112f; Y_Pos = 187f;
                                        X_Axis1 = 23.05f; Y_Axis1 = 9.175f;
                                    }

                                    float pos_x1; float pos_y1;
                                    float H1Months1 = 0; float H1answer1 = 0;
                                    float lnHeight1 = 0; float lnWeight1 = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1-16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1-16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                               pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                    {
                                        pos_x1 = 0; pos_y1 = 0;
                                        H1answer1 = float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim());
                                        if (H1answer1 > 0)
                                        {
                                            H1Months1 = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                            H1Months1 = H1Months1 / 12;
                                            lnHeight1 = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                            lnWeight1 = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());

                                            if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                            {
                                                X_Pos = 95.5f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                            {
                                                X_Pos = 94.8f; X_Axis1 = 16.5f;
                                                pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            else
                                            {
                                                pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                                pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                            }
                                            cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                            cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                            cbWS.EndText();
                                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                        }
                                    }

                                    int U_Pos1 = 50; int V_Pos1 = 85; float CalMonths1 = 0; int Count1 = 0; float WeightStat_percent = 0; string WeightStat = "0";
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;

                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H1"].Value.ToString().Trim()), float.Parse(Entity.Cells["W1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);

                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10); 
                                            Count1++;
                                            if (Count1 == 1) U_Pos1 += 250;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;

                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H2"].Value.ToString().Trim()), float.Parse(Entity.Cells["W2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                            
                                            Count1++;
                                            if (Count1 == 2) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; } else if (Count1 == 1) U_Pos1 += 250;  else if (Count1 == 0) { U_Pos1 = 50; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;
                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H3"].Value.ToString().Trim()), float.Parse(Entity.Cells["W3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                            
                                            Count1++;
                                            if (Count1 == 1 || Count1==3) { U_Pos1 += 250; }
                                            else if (Count1 == 0) { U_Pos1 = 50; }
                                            else if (Count1 == 2){ U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;
                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H4"].Value.ToString().Trim()), float.Parse(Entity.Cells["W4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                            //if (Count1 == 1 || Count1 == 3) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                            //else if (Count1 == 0) { U_Pos1 = 50; }
                                            //else if (Count1 == 2) { U_Pos1 += 250; }
                                            Count1++;
                                            if (Count1 == 1 || Count1 == 3) { U_Pos1 += 250; }
                                            else if (Count1 == 0) { U_Pos1 = 50; }
                                            else if (Count1 == 2 || Count1==4) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;
                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H5"].Value.ToString().Trim()), float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                            //if (Count1 == 1 || Count1 == 3) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                            //else if (Count1 == 0) { U_Pos1 = 50; }
                                            //else if (Count1 == 2 || Count1 == 4) { U_Pos1 += 250; }
                                            Count1++;
                                            if (Count1 == 1 || Count1 == 3 || Count1==5) { U_Pos1 += 250; }
                                            else if (Count1 == 0) { U_Pos1 = 50; }
                                            else if (Count1 == 2 || Count1 == 4) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;
                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H6"].Value.ToString().Trim()), float.Parse(Entity.Cells["W6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                            //if (Count1 == 1 || Count1 == 3 || Count1 == 5) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                            //else if (Count1 == 0) { U_Pos1 = 50; }
                                            //else if (Count1 == 2 || Count1 == 4) { U_Pos1 += 250; }
                                            Count1++;
                                            if (Count1 == 1 || Count1 == 3 || Count1 == 5) { U_Pos1 += 250; }
                                            else if (Count1 == 0) { U_Pos1 = 50; }
                                            else if (Count1 == 2 || Count1 == 4 || Count1==6) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;
                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H7"].Value.ToString().Trim()), float.Parse(Entity.Cells["W7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                            //if (Count1 == 1 || Count1 == 3 || Count1 == 5) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                            //else if (Count1 == 0) { U_Pos1 = 50; }
                                            //else if (Count1 == 2 || Count1 == 4 || Count1 == 6) { U_Pos1 += 250; }
                                            Count1++;
                                            if (Count1 == 1 || Count1 == 3 || Count1 == 5 || Count1==7) { U_Pos1 += 250; }
                                            else if (Count1 == 0) { U_Pos1 = 50; }
                                            else if (Count1 == 2 || Count1 == 4 || Count1 == 6) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim()))
                                    {
                                        if (float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()) > 0)
                                        {
                                            CalMonths1 = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                            H1Months1 = CalMonths1 / 12;
                                            WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H8"].Value.ToString().Trim()), float.Parse(Entity.Cells["W8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                            if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                            cbWS.SetFontAndSize(bf_times, 9);
                                            cbWS.BeginText();
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                            cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                            cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                            //if (Count1 == 1 || Count1 == 3 || Count1 == 5 || Count1 == 7) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                            //else if (Count1 == 0) { U_Pos1 = 50; }
                                            //else if (Count1 == 2 || Count1 == 4 || Count1 == 6) { U_Pos1 += 250; }
                                            Count1++;
                                            if (Count1 == 1 || Count1 == 3 || Count1 == 5 || Count1 == 7) { U_Pos1 += 250; }
                                            else if (Count1 == 0) { U_Pos1 = 50; }
                                            else if (Count1 == 2 || Count1 == 4 || Count1 == 6 || Count1==8) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        }
                                    }

                                    WSstamper.Close();
                                }

                                #endregion
                            }

                        }

                        #endregion

                        //DataaddtoErrortable(Entity);
                        
                    }
                }
                AlertBox.Show("Charts Generated Successfully");
            }
            else if (rdoAllChild.Checked == true)
            {
                foreach (DataGridViewRow Entity in gvChart.Rows)
                {
                    AppName = Entity.Cells["NAME"].Value.ToString().Trim();
                    if (Entity.Cells["Seq"].Value.ToString().Trim() == "1")
                        RepAppName = Entity.Cells["NAME"].Value.ToString().Trim().Replace(",", "_");
                    else RepAppName = Entity.Cells["NAME"].Value.ToString().Trim().Replace(",", "_") + "_" + (Convert.ToInt32(Entity.Cells["Seq"].Value.ToString().Trim()) - 1).ToString();
                    #region Define the Age and Height Charts

                    if (string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim())
                        && string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim())
                        && string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim())
                        && string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                    {
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString()))
                            MONTHS = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString()))
                            MONTHS = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                        if (MONTHS > 36)
                        {
                            if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                Heightchart = Application.MapPath("~\\Resources\\Pdf\\boys_ht_2-20_yr.pdf");
                            else Heightchart = Application.MapPath("~\\Resources\\Pdf\\girls_ht_2-20_yr.pdf");
                        }
                        else
                        {
                            if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                Heightchart = Application.MapPath("~\\Resources\\Pdf\\boys_ht_0-36_mo.pdf");
                            else Heightchart = Application.MapPath("~\\Resources\\Pdf\\girls_ht_0-36_mo.pdf");
                        }

                        if (!string.IsNullOrEmpty(Heightchart))
                        {
                            PdfReader Hreader = new PdfReader(Heightchart);
                            string PdfName = "ht.pdf";
                            PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                            Hstamper.Writer.SetPageSize(PageSize.A4);
                            PdfContentByte cb = Hstamper.GetOverContent(1);
                            cb.SetFontAndSize(bf_times, 9);
                            //cb.SetColorFill(BaseColor.CYAN.Darker());
                            cb.BeginText();
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Height", 90, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 150, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 200, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 350, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Height", 390, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                            cb.EndText();
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Report Date : " + DateTime.Now.ToShortDateString()), 50, 790, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim()), 350, 790, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("App# : " + Entity.Cells["APPNO"].Value.ToString().Trim()), 50, 777, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Name : " + AppName), 350, 777, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim())), 50, 764, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age  : " + Entity.Cells["AGE"].Value.ToString().Trim()+" Years"), 350, 764, 0);

                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age"), 50, 100, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Height"), 90, 100, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date"), 150, 100, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Percentile"), 200, 100, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age"), 350, 100, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Height"), 390, 100, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date"), 450, 100, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Percentile"), 500, 100, 0);

                            cb.SetFontAndSize(bfTimes, 10);


                            float X_Axis = 0, Y_Axis = 0;
                            if (MONTHS > 36)
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                {
                                    cb.SetColorFill(BaseColor.BLUE);
                                    X_Pos = 104.1f; Y_Pos = 150.8f;
                                    X_Axis = 22.1f; Y_Axis = 11.66f;
                                }
                                else
                                {
                                    cb.SetColorFill(BaseColor.RED);
                                    X_Pos = 108.8f; Y_Pos = 149.8f;
                                    X_Axis = 22f; Y_Axis = 11.67f;
                                }

                                float pos_x; float pos_y;
                                float H1Months = 0; float H1answer = 0;

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);

                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);

                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 28) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Height_Percentage = 0; string HPercent = "0";
                                if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H1"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1) U_Pos += 300;
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H2"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H3"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        //Count++;
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H4"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H5"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H6"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H7"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H8"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6 || Count == 8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }

                            }
                            else
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                {
                                    cb.SetColorFill(BaseColor.BLUE);
                                    X_Pos = 100.5f; Y_Pos = 149.3f;
                                    X_Axis = 11.2f; Y_Axis = 22.5f;
                                }
                                else
                                {
                                    cb.SetColorFill(BaseColor.RED);
                                    X_Pos = 99.3f; Y_Pos = 148.5f;
                                    X_Axis = 11.2f; Y_Axis = 22.5f;
                                }

                                float pos_x; float pos_y;
                                float H1Months = 0; float H1answer = 0;

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());


                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;

                                    if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 16) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Height_Percentage = 0; string HPercent = "0";
                                if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H1"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1) U_Pos += 300;
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H2"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H3"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H4"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H5"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        { U_Pos += 300; }
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H6"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H7"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["H8"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Height_Percentage = Calculate_LengthAge((int)CalMonths, float.Parse(Entity.Cells["H8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Height_Percentage > 0) HPercent = Height_Percentage.ToString("0.00"); else HPercent = Height_Percentage.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, CalMonths.ToString(), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, HPercent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6 || Count == 8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }
                            }
                            Hstamper.Close();
                        }

                    }

                    #endregion

                    #region Define Age and Weight Charts
                    if (string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim())
                        && string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim())
                        && string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim())
                        && string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                    {
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString()))
                            MONTHS = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString()))
                            MONTHS = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                        else if (string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());

                        if (MONTHS > 36)
                        {
                            if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                weightChart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_2-20_yr.pdf");
                            else weightChart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_2-20_yr.pdf");
                        }
                        else
                        {
                            if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                weightChart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_0-36_mo.pdf");
                            else weightChart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_0-36_mo.pdf");
                        }

                        if (!string.IsNullOrEmpty(weightChart))
                        {
                            PdfReader Wreader = new PdfReader(weightChart);
                            string PdfName = "wt.pdf";
                            PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                            PdfStamper Wstamper = new PdfStamper(Wreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                            Wstamper.Writer.SetPageSize(PageSize.A4);
                            PdfContentByte cb = Wstamper.GetOverContent(1);

                            cb.SetFontAndSize(bf_times, 9);
                            //cb.SetColorFill(BaseColor.CYAN.Darker());
                            cb.BeginText();
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 90, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 150, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 200, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 350, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 390, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                            cb.EndText();
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Report Date : " + DateTime.Now.ToShortDateString()), 50, 790, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim()), 350, 790, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("App# : " + Entity.Cells["APPNO"].Value.ToString().Trim()), 50, 777, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Name : " + AppName), 350, 777, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim())), 50, 764, 0);
                            //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years"), 350, 764, 0);

                            cb.SetFontAndSize(bfTimes, 10);
                            float X_Axis = 0, Y_Axis = 0;
                            if (MONTHS > 36)
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                {
                                    cb.SetColorFill(BaseColor.BLUE);
                                    X_Pos = 100.5f; Y_Pos = 147.8f;
                                    X_Axis = 22.28f; Y_Axis = 2.65f;
                                }
                                else
                                {
                                    cb.SetColorFill(BaseColor.RED);
                                    X_Pos = 94.8f; Y_Pos = 153.3f;
                                    X_Axis = 22.7f; Y_Axis = 2.625f;
                                }

                                float pos_x; float pos_y;
                                float H1Months = 0; float H1answer = 0;

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Weight_percentile = 0; string WTAge_Percent = "0";
                                if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W1"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1) U_Pos += 300;
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W2"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                        //Count++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W3"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //Count++;
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W4"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W5"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W6"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W7"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W8"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6 || Count == 8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                            }
                            else
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                {
                                    cb.SetColorFill(BaseColor.BLUE);
                                    X_Pos = 92.5f; Y_Pos = 151f;
                                    X_Axis = 11.47f; Y_Axis = 15.54f;
                                }
                                else
                                {
                                    cb.SetColorFill(BaseColor.RED);
                                    X_Pos = 91.5f; Y_Pos = 147f;
                                    X_Axis = 11.48f; Y_Axis = 15.63f;
                                }

                                float pos_x; float pos_y;
                                float H1Months = 0; float H1answer = 0;

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());


                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                        H1answer = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());

                                        pos_x = (H1Months) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 3) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float Weight_percentile = 0; string WTAge_Percent = "0";
                                if (!string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W1"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W1AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;

                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1) U_Pos += 300;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W2"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W2AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;

                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W3"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W3AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W4"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W4AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W5"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W5AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W6"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W6AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W7"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W7AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["W8"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["W8AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths;
                                        Weight_percentile = Calculate_WeightAge((int)CalMonths, float.Parse(Entity.Cells["W8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (Weight_percentile > 0) WTAge_Percent = Weight_percentile.ToString("0.00"); else WTAge_Percent = Weight_percentile.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString().Trim(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, WTAge_Percent, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6 || Count == 8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                            }
                            Wstamper.Close();
                        }

                    }
                    #endregion

                    #region Define the BMI Charts...
                    if (!string.IsNullOrEmpty(Heightchart) && !string.IsNullOrEmpty(weightChart))
                    {
                        MONTHS = 0;
                        if ((!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim())))
                            MONTHS = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                        else if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
                            MONTHS = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());

                        if (((Entity.Cells["BMI1"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim())) ||
                            ((Entity.Cells["BMI2"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim())) ||
                            ((Entity.Cells["BMI3"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim())) ||
                            ((Entity.Cells["BMI4"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim())) ||
                            ((Entity.Cells["BMI5"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim())) ||
                            ((Entity.Cells["BMI6"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim())) ||
                            ((Entity.Cells["BMI7"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim())) ||
                            ((Entity.Cells["BMI8"].Value.ToString().Trim()) != "0" || !string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim())))
                        {
                            // Define the BMI Charts...
                            if (MONTHS > 24)
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                    BMIchart = Application.MapPath("~\\Resources\\Pdf\\boys_bmi.pdf");
                                else BMIchart = Application.MapPath("~\\Resources\\Pdf\\girls_bmi.pdf");

                                PdfReader Wreader = new PdfReader(BMIchart);
                                string PdfName = "bmi.pdf";
                                PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                                PdfStamper Wstamper = new PdfStamper(Wreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                                Wstamper.Writer.SetPageSize(PageSize.A4);
                                PdfContentByte cb = Wstamper.GetOverContent(1);

                                cb.SetFontAndSize(bf_times, 9);
                                //cb.SetColorFill(BaseColor.CYAN.Darker());
                                cb.BeginText();
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "BMI", 90, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 150, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 200, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 350, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "BMI", 390, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                cb.EndText();

                                cb.SetFontAndSize(bfTimes, 10);
                                float X_Axis = 0, Y_Axis = 0;

                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                {
                                    cb.SetColorFill(BaseColor.BLUE);
                                    X_Pos = 83.8f; Y_Pos = 149.5f;
                                    X_Axis = 23.05f; Y_Axis = 22.48f;
                                }
                                else
                                {
                                    cb.SetColorFill(BaseColor.RED);
                                    X_Pos = 83.8f; Y_Pos = 150.8f;
                                    X_Axis = 23f; Y_Axis = 22.45f;
                                }

                                float pos_x; float pos_y;
                                float H1Months = 0; float H1answer = 0;

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;

                                        pos_x = (H1Months - 2) * X_Axis + X_Pos;
                                        pos_y = (H1answer - 10) * Y_Axis + Y_Pos;
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0; float BMI_Perc = 0; string Per_BMI = "0";
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1) U_Pos += 300;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        if (Count == 1) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 0) { U_Pos = 50; }
                                        Count++;
                                        if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 300; else if (Count == 0) { U_Pos = 50; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2) { U_Pos += 300; }
                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4) { U_Pos += 300; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4) { U_Pos += 300; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI.ToString(), U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 300; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;
                                        BMI_Perc = Calculate_BMI((int)CalMonths, float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (BMI_Perc > 0) Per_BMI = BMI_Perc.ToString("0.00"); else Per_BMI = BMI_Perc.ToString();
                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()).ToString("0.00"), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 100, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Per_BMI, U_Pos + 150, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 300; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 300; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6 || Count == 8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }


                                Wstamper.Close();
                            }


                            #region Define Weight Length Charts
                            if (MONTHS < 36 && MONTHS > 0)
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                    BMIchart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_ln_0-36_mo.pdf");
                                else BMIchart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_ln_0-36_mo.pdf");

                                PdfReader Wreader = new PdfReader(BMIchart);
                                string PdfName = "wt_ln.pdf";
                                PdfName = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName;
                                PdfStamper Wstamper = new PdfStamper(Wreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
                                Wstamper.Writer.SetPageSize(PageSize.A4);
                                PdfContentByte cb = Wstamper.GetOverContent(1);

                                cb.SetFontAndSize(bf_times, 9);
                                //cb.SetColorFill(BaseColor.CYAN.Darker());
                                cb.BeginText();
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 90, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Stature", 140, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 190, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 250, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", 300, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 340, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Stature", 390, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                cb.EndText();

                                cb.SetFontAndSize(bfTimes, 10);
                                float X_Axis = 0, Y_Axis = 0;
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                {
                                    cb.SetColorFill(BaseColor.BLUE);
                                    X_Pos = 113.4f; Y_Pos = 172f;
                                    X_Axis = 16.8f; Y_Axis = 11.035f;
                                }
                                else
                                {
                                    cb.SetColorFill(BaseColor.RED);
                                    X_Pos = 114.3f; Y_Pos = 172f;
                                    X_Axis = 16.83f; Y_Axis = 11.05f;
                                }

                                float pos_x; float pos_y;
                                float H1Months = 0; float H1answer = 0;
                                float lnHeight = 0; float lnWeight = 0;
                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                {
                                    pos_x = 0; pos_y = 0;
                                    H1answer = float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim());
                                    if (H1answer > 0)
                                    {
                                        H1Months = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months = H1Months / 12;
                                        lnHeight = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                        lnWeight = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());

                                        if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 92.8f; X_Axis = 21.5f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else if (lnHeight < 19 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 92.3f; X_Axis = 21.1f;
                                            pos_x = (lnHeight - 18) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x = (lnHeight - 19) * X_Axis + X_Pos;
                                            pos_y = (lnWeight) * Y_Axis + Y_Pos;
                                        }
                                        cb.Circle(pos_x, pos_y, 2f); cb.Fill();
                                        cb.BeginText(); cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x + 10, pos_y - 3, 0);
                                        cb.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                int U_Pos = 50; int V_Pos = 85; float CalMonths = 0; int Count = 0;
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;


                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        Count++;
                                        if (Count == 1) U_Pos += 250;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; } else if (Count == 1) U_Pos += 250; else if (Count == 0) { U_Pos = 50; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 250; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);

                                        Count++;
                                        if (Count == 1 || Count == 3) { U_Pos += 250; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4) { U_Pos += 250; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 250; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4) { U_Pos += 250; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5) { U_Pos += 250; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3 || Count == 5) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 250; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 250; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months = CalMonths / 12;

                                        cb.SetFontAndSize(bf_times, 9);
                                        cb.BeginText();
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, H1Months.ToString("0.00"), U_Pos, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString(), U_Pos + 40, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString(), U_Pos + 90, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos + 140, V_Pos, 0);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "", U_Pos + 200, V_Pos, 0);
                                        cb.EndText(); cb.SetFontAndSize(bf_times, 10);
                                        //if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                        //else if (Count == 0) { U_Pos = 50; }
                                        //else if (Count == 2 || Count == 4 || Count == 6) { U_Pos += 250; }
                                        Count++;
                                        if (Count == 1 || Count == 3 || Count == 5 || Count == 7) { U_Pos += 250; }
                                        else if (Count == 0) { U_Pos = 50; }
                                        else if (Count == 2 || Count == 4 || Count == 6 || Count == 8) { U_Pos = 50; V_Pos = V_Pos - 15; }
                                    }
                                }

                                Wstamper.Close();
                            }
                            #endregion

                            #region Define Weight Stature charts

                            if (MONTHS > 0)
                            {
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                    BMIchart = Application.MapPath("~\\Resources\\Pdf\\boys_wt_st.pdf");
                                else BMIchart = Application.MapPath("~\\Resources\\Pdf\\girls_wt_st.pdf");

                                PdfReader WSreader = new PdfReader(BMIchart);
                                string PdfName1 = "wt_st.pdf";
                                PdfName1 = propReportPath + BaseForm.UserID + "\\" + RepAppName + "_" + PdfName1;
                                PdfStamper WSstamper = new PdfStamper(WSreader, new FileStream(PdfName1, FileMode.Create, FileAccess.Write));
                                WSstamper.Writer.SetPageSize(PageSize.A4);
                                PdfContentByte cbWS = WSstamper.GetOverContent(1);

                                cbWS.SetFontAndSize(bf_times, 9);
                                //cb.SetColorFill(BaseColor.CYAN.Darker());
                                cbWS.BeginText();
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Report Date : " + DateTime.Now.ToShortDateString(), 50, 790, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Site/Room/AM/PM : " + Entity.Cells["SITE"].Value.ToString() + " " + Entity.Cells["ROOM"].Value.ToString() + " " + Entity.Cells["AMPM"].Value.ToString().Trim(), 350, 790, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "App# : " + Entity.Cells["APPNO"].Value.ToString().Trim(), 50, 777, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Name : " + AppName, 350, 777, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "DOB  : " + LookupDataAccess.Getdate(Entity.Cells["DOB"].Value.ToString().Trim()), 50, 764, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Age  : " + Entity.Cells["AGE"].Value.ToString().Trim() + " Years", 350, 764, 0);

                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Age", 50, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 90, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Length", 140, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Date", 190, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 250, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Age", 300, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Weight", 340, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Length", 390, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Date", 450, 100, 0);
                                cbWS.ShowTextAligned(Element.ALIGN_LEFT, "Percentile", 500, 100, 0);
                                cbWS.EndText();

                                cbWS.SetFontAndSize(bfTimes, 10);
                                float X_Axis1 = 0; float Y_Axis1 = 0;
                                if (Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                {
                                    cbWS.SetColorFill(BaseColor.BLUE);
                                    X_Pos = 111.3f; Y_Pos = 187.9f;
                                    X_Axis1 = 23.05f; Y_Axis1 = 9.175f;
                                }
                                else
                                {
                                    cbWS.SetColorFill(BaseColor.RED);
                                    X_Pos = 112f; Y_Pos = 187f;
                                    X_Axis1 = 23.05f; Y_Axis1 = 9.175f;
                                }

                                float pos_x1; float pos_y1;
                                float H1Months1 = 0; float H1answer1 = 0;
                                float lnHeight1 = 0; float lnWeight1 = 0;
                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H1"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W1"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H2"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W2"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT2"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H3"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W3"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT3"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H4"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W4"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT4"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H5"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W5"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT5"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H6"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W6"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT6"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H7"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W7"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT7"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
                                {
                                    pos_x1 = 0; pos_y1 = 0;
                                    H1answer1 = float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim());
                                    if (H1answer1 > 0)
                                    {
                                        H1Months1 = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months1 = H1Months1 / 12;
                                        lnHeight1 = float.Parse(Entity.Cells["H8"].Value.ToString().Trim());
                                        lnWeight1 = float.Parse(Entity.Cells["W8"].Value.ToString().Trim());

                                        if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "M")
                                        {
                                            X_Pos = 95.5f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else if (lnHeight1 < 31 && Entity.Cells["SEX"].Value.ToString().Trim() == "F")
                                        {
                                            X_Pos = 94.8f; X_Axis1 = 16.5f;
                                            pos_x1 = (lnHeight1 - 30) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        else
                                        {
                                            pos_x1 = (lnHeight1 - 31) * X_Axis1 + X_Pos;
                                            pos_y1 = (lnWeight1 - 16) * Y_Axis1 + Y_Pos;
                                        }
                                        cbWS.Circle(pos_x1, pos_y1, 2f); cbWS.Fill();
                                        cbWS.BeginText(); cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["WEIGHT8"].Value.ToString().Trim()), pos_x1 + 10, pos_y1 - 3, 0);
                                        cbWS.EndText();
                                        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim())), pos_x + 10, pos_y, 0);
                                    }
                                }

                                int U_Pos1 = 50; int V_Pos1 = 85; float CalMonths1 = 0; int Count1 = 0; float WeightStat_percent = 0; string WeightStat = "0";
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI1"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI1"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H1AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;

                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H1"].Value.ToString().Trim()), float.Parse(Entity.Cells["W1"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W1"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H1"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT1"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);

                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                        Count1++;
                                        if (Count1 == 1) U_Pos1 += 250;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI2"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI2"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H2AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;

                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H2"].Value.ToString().Trim()), float.Parse(Entity.Cells["W2"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W2"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H2"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT2"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);

                                        Count1++;
                                        if (Count1 == 2) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; } else if (Count1 == 1) U_Pos1 += 250; else if (Count1 == 0) { U_Pos1 = 50; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI3"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI3"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H3AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;
                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H3"].Value.ToString().Trim()), float.Parse(Entity.Cells["W3"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W3"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H3"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT3"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);

                                        Count1++;
                                        if (Count1 == 1 || Count1 == 3) { U_Pos1 += 250; }
                                        else if (Count1 == 0) { U_Pos1 = 50; }
                                        else if (Count1 == 2) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                    }
                                }

                                if (!string.IsNullOrEmpty(Entity.Cells["BMI4"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI4"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H4AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;
                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H4"].Value.ToString().Trim()), float.Parse(Entity.Cells["W4"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W4"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H4"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT4"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                        //if (Count1 == 1 || Count1 == 3) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        //else if (Count1 == 0) { U_Pos1 = 50; }
                                        //else if (Count1 == 2) { U_Pos1 += 250; }
                                        Count1++;
                                        if (Count1 == 1 || Count1 == 3) { U_Pos1 += 250; }
                                        else if (Count1 == 0) { U_Pos1 = 50; }
                                        else if (Count1 == 2 || Count1 == 4) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI5"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI5"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H5AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;
                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H5"].Value.ToString().Trim()), float.Parse(Entity.Cells["W5"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W5"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H5"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT5"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                        //if (Count1 == 1 || Count1 == 3) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        //else if (Count1 == 0) { U_Pos1 = 50; }
                                        //else if (Count1 == 2 || Count1 == 4) { U_Pos1 += 250; }
                                        Count1++;
                                        if (Count1 == 1 || Count1 == 3 || Count1 == 5) { U_Pos1 += 250; }
                                        else if (Count1 == 0) { U_Pos1 = 50; }
                                        else if (Count1 == 2 || Count1 == 4) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI6"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI6"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H6AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;
                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H6"].Value.ToString().Trim()), float.Parse(Entity.Cells["W6"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W6"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H6"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT6"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                        //if (Count1 == 1 || Count1 == 3 || Count1 == 5) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        //else if (Count1 == 0) { U_Pos1 = 50; }
                                        //else if (Count1 == 2 || Count1 == 4) { U_Pos1 += 250; }
                                        Count1++;
                                        if (Count1 == 1 || Count1 == 3 || Count1 == 5) { U_Pos1 += 250; }
                                        else if (Count1 == 0) { U_Pos1 = 50; }
                                        else if (Count1 == 2 || Count1 == 4 || Count1 == 6) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI7"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI7"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H7AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;
                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H7"].Value.ToString().Trim()), float.Parse(Entity.Cells["W7"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W7"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H7"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT7"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                        //if (Count1 == 1 || Count1 == 3 || Count1 == 5) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        //else if (Count1 == 0) { U_Pos1 = 50; }
                                        //else if (Count1 == 2 || Count1 == 4 || Count1 == 6) { U_Pos1 += 250; }
                                        Count1++;
                                        if (Count1 == 1 || Count1 == 3 || Count1 == 5 || Count1 == 7) { U_Pos1 += 250; }
                                        else if (Count1 == 0) { U_Pos1 = 50; }
                                        else if (Count1 == 2 || Count1 == 4 || Count1 == 6) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.Cells["BMI8"].Value.ToString().Trim()))
                                {
                                    if (float.Parse(Entity.Cells["BMI8"].Value.ToString().Trim()) > 0)
                                    {
                                        CalMonths1 = int.Parse(Entity.Cells["H8AGE"].Value.ToString().Trim());
                                        H1Months1 = CalMonths1 / 12;
                                        WeightStat_percent = Calculate_WeightStat(float.Parse(Entity.Cells["H8"].Value.ToString().Trim()), float.Parse(Entity.Cells["W8"].Value.ToString().Trim()), Entity.Cells["SEX"].Value.ToString().Trim());
                                        if (WeightStat_percent > 0) WeightStat = WeightStat_percent.ToString("0.00"); else WeightStat = WeightStat_percent.ToString();
                                        cbWS.SetFontAndSize(bf_times, 9);
                                        cbWS.BeginText();
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, CalMonths1.ToString(), U_Pos1, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["W8"].Value.ToString(), U_Pos1 + 40, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, Entity.Cells["H8"].Value.ToString(), U_Pos1 + 90, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, LookupDataAccess.Getdate(Entity.Cells["HEIGHT8"].Value.ToString().Trim()), U_Pos1 + 140, V_Pos1, 0);
                                        cbWS.ShowTextAligned(Element.ALIGN_LEFT, WeightStat, U_Pos1 + 200, V_Pos1, 0);
                                        cbWS.EndText(); cbWS.SetFontAndSize(bf_times, 10);
                                        //if (Count1 == 1 || Count1 == 3 || Count1 == 5 || Count1 == 7) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                        //else if (Count1 == 0) { U_Pos1 = 50; }
                                        //else if (Count1 == 2 || Count1 == 4 || Count1 == 6) { U_Pos1 += 250; }
                                        Count1++;
                                        if (Count1 == 1 || Count1 == 3 || Count1 == 5 || Count1 == 7) { U_Pos1 += 250; }
                                        else if (Count1 == 0) { U_Pos1 = 50; }
                                        else if (Count1 == 2 || Count1 == 4 || Count1 == 6 || Count1 == 8) { U_Pos1 = 50; V_Pos1 = V_Pos1 - 15; }
                                    }
                                }

                                WSstamper.Close();
                            }

                            #endregion
                        }

                    }

                    #endregion

                    //DataaddtoErrortable(Entity);
                }
                AlertBox.Show("Charts Generated Successfully");
            }
            
        }

        public double TotalMonths(DateTime start, DateTime end)
        {
            //return (start.Year * 12 + start.Month) - (end.Year * 12 + end.Month);
            return (start.Subtract(end).Days / (365.25/ 12));
        }

        private void btnGenerateCharts_Click(object sender, EventArgs e)
        {
            if (rdoSelChild.Checked == true)
            {
                bool ISSelect = false;
                foreach (DataGridViewRow dr in gvChart.Rows)
                {
                    if (dr.Cells["dgvChk"].Value != null && Convert.ToBoolean(dr.Cells["dgvChk"].Value) == true)
                    {
                        ISSelect = true; break;
                    }
                }
                if (ISSelect)
                    On_Exist_PDF();
                else
                    AlertBox.Show("Please Select at least one Child", MessageBoxIcon.Warning);
            }
            else
                On_Exist_PDF();
        }

        private void btnChartPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void FillPercentages()
        {
            string OpenPath, contents;
            int tabSize = 15;
            string[] arInfo;
            string line;
            // Create new DataTable.
            DataTable tableBMI = CreateTableBMI(); //DataTable tableWeightAge = CreateTableWeightAge();
            DataRow row;
            try
            {
                OpenPath = _model.lookupDataAccess.GetReportPath() + "\\bmiageINFO.txt"; //@"E:\PIRQUEST-20121.DAT"; ;
                string FILENAME = OpenPath;
                //Get a StreamReader class that can be used to read the file
                StreamReader objStreamReader;
                objStreamReader = File.OpenText(FILENAME);

                while ((line = objStreamReader.ReadLine()) != null)
                {
                    contents = line.Replace(("").PadRight(tabSize, ' '), "\t");
                    // define which character is seperating fields
                    char[] textdelimiter = { ' ' };

                    arInfo = contents.Split(textdelimiter);

                    row = tableBMI.NewRow();
                    if (arInfo.Length > 0)
                    {
                        row["BMI-SEX"] = arInfo[0].ToString();
                        row["BMI-Agemos"] = arInfo[1].ToString();
                        row["BMI-L"] = arInfo[2].ToString();
                        row["BMI-M"] = arInfo[3].ToString();
                        row["BMI-S"] = arInfo[4].ToString();
                        row["BMI-P3"] = arInfo[5].ToString();
                        row["BMI-P5"] = arInfo[6].ToString();
                        row["BMI-P10"] = arInfo[7].ToString();
                        row["BMI-P25"] = arInfo[8].ToString();
                        row["BMI-P50"] = arInfo[9].ToString();
                        row["BMI-P75"] = arInfo[10].ToString();
                        row["BMI-P85"] = arInfo[11].ToString();
                        row["BMI-P90"] = arInfo[12].ToString();
                        row["BMI-P95"] = arInfo[13].ToString();
                        row["BMI-P97"] = arInfo[14].ToString();

                        tableBMI.Rows.Add(row);
                    }
                }
                objStreamReader.Close();
                propDatatableBMI = tableBMI;

                
                //Create a table For Weight Age percentile
                tableBMI = CreateTableWeightAge();
                OpenPath = _model.lookupDataAccess.GetReportPath() + "\\wtageinf.txt"; //@"E:\PIRQUEST-20121.DAT"; ;
                FILENAME = OpenPath;
                //Get a StreamReader class that can be used to read the file
                //StreamReader objStreamReader;
                objStreamReader = File.OpenText(FILENAME);

                while ((line = objStreamReader.ReadLine()) != null)
                {
                    contents = line.Replace(("").PadRight(tabSize, ' '), "\t");
                    // define which character is seperating fields
                    char[] textdelimiter = { ' ' };

                    arInfo = contents.Split(textdelimiter);

                    row = tableBMI.NewRow();
                    if (arInfo.Length > 0)
                    {
                        row["WTAGE-SEX"] = arInfo[0].ToString();
                        row["WTAGE-Agemos"] = arInfo[1].ToString();
                        row["WTAGE-L"] = arInfo[2].ToString();
                        row["WTAGE-M"] = arInfo[3].ToString();
                        row["WTAGE-S"] = arInfo[4].ToString();
                        row["WTAGE-P3"] = arInfo[5].ToString();
                        row["WTAGE-P5"] = arInfo[6].ToString();
                        row["WTAGE-P10"] = arInfo[7].ToString();
                        row["WTAGE-P25"] = arInfo[8].ToString();
                        row["WTAGE-P50"] = arInfo[9].ToString();
                        row["WTAGE-P75"] = arInfo[10].ToString();
                        row["WTAGE-P90"] = arInfo[11].ToString();
                        row["WTAGE-P95"] = arInfo[12].ToString();
                        row["WTAGE-P97"] = arInfo[13].ToString();

                        tableBMI.Rows.Add(row);
                    }
                }
                objStreamReader.Close();
                propDatatableWeightAge = tableBMI;


                //Create a table For Weight Stature Percentile
                tableBMI = CreateTableWeightStat();
                OpenPath = _model.lookupDataAccess.GetReportPath() + "\\WtStat.txt"; //@"E:\PIRQUEST-20121.DAT"; ;
                FILENAME = OpenPath;
                //Get a StreamReader class that can be used to read the file
                //StreamReader objStreamReader;
                objStreamReader = File.OpenText(FILENAME);

                while ((line = objStreamReader.ReadLine()) != null)
                {
                    contents = line.Replace(("").PadRight(tabSize, ' '), "\t");
                    // define which character is seperating fields
                    char[] textdelimiter = { ' ' };

                    arInfo = contents.Split(textdelimiter);

                    row = tableBMI.NewRow();
                    if (arInfo.Length > 0)
                    {
                        row["WTSTAT-SEX"] = arInfo[0].ToString();
                        row["WTSTAT-HEIGHT"] = arInfo[1].ToString();
                        row["WTSTAT-L"] = arInfo[2].ToString();
                        row["WTSTAT-M"] = arInfo[3].ToString();
                        row["WTSTAT-S"] = arInfo[4].ToString();
                        row["WTSTAT-P3"] = arInfo[5].ToString();
                        row["WTSTAT-P5"] = arInfo[6].ToString();
                        row["WTSTAT-P10"] = arInfo[7].ToString();
                        row["WTSTAT-P25"] = arInfo[8].ToString();
                        row["WTSTAT-P50"] = arInfo[9].ToString();
                        row["WTSTAT-P75"] = arInfo[10].ToString();
                        row["WTSTAT-P85"] = arInfo[11].ToString();
                        row["WTSTAT-P90"] = arInfo[12].ToString();
                        row["WTSTAT-P95"] = arInfo[13].ToString();
                        row["WTSTAT-P97"] = arInfo[14].ToString();

                        tableBMI.Rows.Add(row);
                    }
                }
                objStreamReader.Close();
                propDatatableWeightStat = tableBMI;


                tableBMI = CreateTableLengthAge();
                OpenPath = _model.lookupDataAccess.GetReportPath() + "\\HTAge.Txt"; //@"E:\PIRQUEST-20121.DAT"; ;
                FILENAME = OpenPath;
                //Get a StreamReader class that can be used to read the file
                //StreamReader objStreamReader;
                objStreamReader = File.OpenText(FILENAME);

                while ((line = objStreamReader.ReadLine()) != null)
                {
                    contents = line.Replace(("").PadRight(tabSize, ' '), "\t");
                    // define which character is seperating fields
                    char[] textdelimiter = { ' ' };

                    arInfo = contents.Split(textdelimiter);

                    row = tableBMI.NewRow();
                    if (arInfo.Length > 0)
                    {
                        row["LenAge-SEX"] = arInfo[0].ToString();
                        row["LenAge-Agemos"] = arInfo[1].ToString();
                        row["LenAge-L"] = arInfo[2].ToString();
                        row["LenAge-M"] = arInfo[3].ToString();
                        row["LenAge-S"] = arInfo[4].ToString();
                        row["LenAge-P3"] = arInfo[5].ToString();
                        row["LenAge-P5"] = arInfo[6].ToString();
                        row["LenAge-P10"] = arInfo[7].ToString();
                        row["LenAge-P25"] = arInfo[8].ToString();
                        row["LenAge-P50"] = arInfo[9].ToString();
                        row["LenAge-P75"] = arInfo[10].ToString();
                        row["LenAge-P90"] = arInfo[11].ToString();
                        row["LenAge-P95"] = arInfo[12].ToString();
                        row["LenAge-P97"] = arInfo[13].ToString();

                        tableBMI.Rows.Add(row);
                    }
                }
                objStreamReader.Close();
                propDatatableLenghtAge = tableBMI;
               
            }
            catch (Exception ex)
            {
                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }
        }


        private DataTable CreateTableBMI()
        {
            try
            {
                DataTable table = new DataTable();

                // Declare DataColumn and DataRow variables.
                DataColumn column;

                // Create new DataColumn, set DataType, ColumnName
                // and add to DataTable.    
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-SEX";
                table.Columns.Add(column);

                // Create second column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "BMI-Agemos";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-L";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-M";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-S";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P3";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P5";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P10";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P25";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P50";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P75";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P85";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P90";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P95";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "BMI-P97";
                table.Columns.Add(column);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DataTable CreateTableWeightAge()
        {
            try
            {
                DataTable table = new DataTable();

                // Declare DataColumn and DataRow variables.
                DataColumn column;

                // Create new DataColumn, set DataType, ColumnName
                // and add to DataTable.    
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-SEX";
                table.Columns.Add(column);

                // Create second column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "WTAGE-Agemos";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-L";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-M";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-S";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P3";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P5";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P10";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P25";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P50";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P75";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P90";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P95";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTAGE-P97";
                table.Columns.Add(column);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DataTable CreateTableWeightStat()
        {
            try
            {
                DataTable table = new DataTable();

                // Declare DataColumn and DataRow variables.
                DataColumn column;

                // Create new DataColumn, set DataType, ColumnName
                // and add to DataTable.    
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-SEX";
                table.Columns.Add(column);

                // Create second column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "WTSTAT-HEIGHT";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-L";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-M";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-S";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P3";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P5";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P10";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P25";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P50";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P75";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P85";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P90";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P95";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "WTSTAT-P97";
                table.Columns.Add(column);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DataTable CreateTableLengthAge()
        {
            try
            {
                DataTable table = new DataTable();

                // Declare DataColumn and DataRow variables.
                DataColumn column;

                // Create new DataColumn, set DataType, ColumnName
                // and add to DataTable.    
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-SEX";
                table.Columns.Add(column);

                // Create second column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "LenAge-Agemos";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-L";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-M";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-S";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P3";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P5";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P10";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P25";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P50";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P75";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P90";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P95";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "LenAge-P97";
                table.Columns.Add(column);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //private DataTable CreateTableErrorValues()
        //{
        //    try
        //    {
        //        DataTable table = new DataTable();

        //        // Declare DataColumn and DataRow variables.
        //        DataColumn column;

        //        // Create new DataColumn, set DataType, ColumnName
        //        // and add to DataTable.    
        //        column = new DataColumn();
        //        column.DataType = System.Type.GetType("System.String");
        //        column.ColumnName = "App#";
        //        table.Columns.Add(column);

        //        // Create second column.
        //        column = new DataColumn();
        //        column.DataType = Type.GetType("System.String");
        //        column.ColumnName = "Child's Name";
        //        table.Columns.Add(column);

        //        column = new DataColumn();
        //        column.DataType = System.Type.GetType("System.String");
        //        column.ColumnName = "DOB";
        //        table.Columns.Add(column);

        //        column = new DataColumn();
        //        column.DataType = System.Type.GetType("System.String");
        //        column.ColumnName = "Uninterpreted/Missing Data";
        //        table.Columns.Add(column);

        //        return table;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}



        private float Calculate_BMI(int Age, float BMI,string Sex)
        {
            float Percentage_BMI = 0; 
            if (BMI > 0)
            {
                //BMI = float.Parse(BMI.ToString());
                string strBMI = BMI.ToString();
                if(strBMI.Length>5)
                    strBMI = strBMI.Substring(0, 5);
                BMI = float.Parse(strBMI.ToString().Trim());
                float BMI_Age = Age;
                BMI_Age = Age + 0.5f;
                foreach (DataRow dr in propDatatableBMI.Rows)
                {
                    if (dr["BMI-SEX"].ToString().Trim() == Sex.Trim() && float.Parse(dr["BMI-Agemos"].ToString().Trim())==BMI_Age)
                    {
                        if (BMI < float.Parse(dr["BMI-P3"].ToString().Trim()))
                        {
                            Percentage_BMI = 0; break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P3"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P5"].ToString().Trim()))
                        {
                            Percentage_BMI = 3 + ((BMI - float.Parse(dr["BMI-P3"].ToString().Trim())) / ((float.Parse(dr["BMI-P5"].ToString().Trim()) - float.Parse(dr["BMI-P3"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P5"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P10"].ToString().Trim()))
                        {
                            Percentage_BMI = 5 + ((BMI - float.Parse(dr["BMI-P5"].ToString().Trim())) / ((float.Parse(dr["BMI-P10"].ToString().Trim()) - float.Parse(dr["BMI-P5"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P10"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P25"].ToString().Trim()))
                        {
                            Percentage_BMI = 10 + ((BMI - float.Parse(dr["BMI-P10"].ToString().Trim())) / ((float.Parse(dr["BMI-P25"].ToString().Trim()) - float.Parse(dr["BMI-P10"].ToString().Trim())) / 15));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P25"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P50"].ToString().Trim()))
                        {
                            Percentage_BMI = 25 + ((BMI - float.Parse(dr["BMI-P25"].ToString().Trim())) / ((float.Parse(dr["BMI-P50"].ToString().Trim()) - float.Parse(dr["BMI-P25"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P50"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P75"].ToString().Trim()))
                        {
                            Percentage_BMI = 50 + ((BMI - float.Parse(dr["BMI-P50"].ToString().Trim())) / ((float.Parse(dr["BMI-P75"].ToString().Trim()) - float.Parse(dr["BMI-P50"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P75"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P85"].ToString().Trim()))
                        {
                            Percentage_BMI = 75 + ((BMI - float.Parse(dr["BMI-P75"].ToString().Trim())) / ((float.Parse(dr["BMI-P85"].ToString().Trim()) - float.Parse(dr["BMI-P75"].ToString().Trim())) / 10));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P85"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P90"].ToString().Trim()))
                        {
                            Percentage_BMI = 85 + ((BMI - float.Parse(dr["BMI-P85"].ToString().Trim())) / ((float.Parse(dr["BMI-P90"].ToString().Trim()) - float.Parse(dr["BMI-P85"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P90"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P95"].ToString().Trim()))
                        {
                            Percentage_BMI = 90 + ((BMI - float.Parse(dr["BMI-P90"].ToString().Trim())) / ((float.Parse(dr["BMI-P95"].ToString().Trim()) - float.Parse(dr["BMI-P90"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P95"].ToString().Trim()) && BMI < float.Parse(dr["BMI-P97"].ToString().Trim()))
                        {
                            Percentage_BMI = 95 + ((BMI - float.Parse(dr["BMI-P95"].ToString().Trim())) / ((float.Parse(dr["BMI-P97"].ToString().Trim()) - float.Parse(dr["BMI-P95"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (BMI > float.Parse(dr["BMI-P97"].ToString().Trim()))
                        {
                            Percentage_BMI = 97 + ((BMI - float.Parse(dr["BMI-P97"].ToString().Trim())) / ((float.Parse(dr["BMI-P97"].ToString().Trim()) - float.Parse(dr["BMI-P95"].ToString().Trim())) / 2));
                            break;
                        }
                    }
                }
            }
            return Percentage_BMI;
        }

        private void HSSB0123_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void rbFundAll_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(rbFundSel, null);
            SelFundingList.Clear();
        }

        private float Calculate_WeightAge(int Age, float Weight, string Sex)
        {
            float Percentage_WeightAge = 0;
            if (Weight > 0)
            {
                float BMI_Age = Age;
                BMI_Age = Age + 0.5f;
                Weight = Weight * 0.45359237f;
                foreach (DataRow dr in propDatatableWeightAge.Rows)
                {
                    if (dr["WTAGE-SEX"].ToString().Trim() == Sex.Trim() && float.Parse(dr["WTAGE-Agemos"].ToString().Trim()) == BMI_Age)
                    {
                        if (Weight < float.Parse(dr["WTAGE-P3"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 0; break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P3"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P5"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 3 + ((Weight - float.Parse(dr["WTAGE-P3"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P5"].ToString().Trim()) - float.Parse(dr["WTAGE-P3"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P5"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P10"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 5 + ((Weight - float.Parse(dr["WTAGE-P5"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P10"].ToString().Trim()) - float.Parse(dr["WTAGE-P5"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P10"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P25"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 10 + ((Weight - float.Parse(dr["WTAGE-P10"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P25"].ToString().Trim()) - float.Parse(dr["WTAGE-P10"].ToString().Trim())) / 15));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P25"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P50"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 25 + ((Weight - float.Parse(dr["WTAGE-P25"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P50"].ToString().Trim()) - float.Parse(dr["WTAGE-P25"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P50"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P75"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 50 + ((Weight - float.Parse(dr["WTAGE-P50"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P75"].ToString().Trim()) - float.Parse(dr["WTAGE-P50"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P75"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P90"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 75 + ((Weight - float.Parse(dr["WTAGE-P75"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P90"].ToString().Trim()) - float.Parse(dr["WTAGE-P75"].ToString().Trim())) / 15));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P90"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P95"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 90 + ((Weight - float.Parse(dr["WTAGE-P90"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P95"].ToString().Trim()) - float.Parse(dr["WTAGE-P90"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P95"].ToString().Trim()) && Weight < float.Parse(dr["WTAGE-P97"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 95 + ((Weight - float.Parse(dr["WTAGE-P95"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P97"].ToString().Trim()) - float.Parse(dr["WTAGE-P95"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTAGE-P97"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 97 + ((Weight - float.Parse(dr["WTAGE-P97"].ToString().Trim())) / ((float.Parse(dr["WTAGE-P97"].ToString().Trim()) - float.Parse(dr["WTAGE-P95"].ToString().Trim())) / 2));
                            break;
                        }
                    }
                }
            }
            return Percentage_WeightAge;
        }

        private float Calculate_WeightStat(float Height, float Weight, string Sex)
        {
            float Percentage_BMI = 0;
            if (Weight > 0)
            {
                Weight = Weight * 0.45359237f;
                float Height_Stat = Height * 2.54f;
                Height_Stat = (int)Height_Stat + 0.5f;
                foreach (DataRow dr in propDatatableWeightStat.Rows)
                {
                    if (dr["WTSTAT-SEX"].ToString().Trim() == Sex.Trim() && float.Parse(dr["WTSTAT-HEIGHT"].ToString().Trim()) == Height_Stat)
                    {
                        if (Weight < float.Parse(dr["WTSTAT-P3"].ToString().Trim()))
                        {
                            Percentage_BMI = 0; break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P3"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P5"].ToString().Trim()))
                        {
                            Percentage_BMI = 3 + ((Weight - float.Parse(dr["WTSTAT-P3"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P5"].ToString().Trim()) - float.Parse(dr["WTSTAT-P3"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P5"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P10"].ToString().Trim()))
                        {
                            Percentage_BMI = 5 + ((Weight - float.Parse(dr["WTSTAT-P5"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P10"].ToString().Trim()) - float.Parse(dr["WTSTAT-P5"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P10"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P25"].ToString().Trim()))
                        {
                            Percentage_BMI = 10 + ((Weight - float.Parse(dr["WTSTAT-P10"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P25"].ToString().Trim()) - float.Parse(dr["WTSTAT-P10"].ToString().Trim())) / 15));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P25"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P50"].ToString().Trim()))
                        {
                            Percentage_BMI = 25 + ((Weight - float.Parse(dr["WTSTAT-P25"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P50"].ToString().Trim()) - float.Parse(dr["WTSTAT-P25"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P50"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P75"].ToString().Trim()))
                        {
                            Percentage_BMI = 50 + ((Weight - float.Parse(dr["WTSTAT-P50"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P75"].ToString().Trim()) - float.Parse(dr["WTSTAT-P50"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P75"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P85"].ToString().Trim()))
                        {
                            Percentage_BMI = 75 + ((Weight - float.Parse(dr["WTSTAT-P75"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P85"].ToString().Trim()) - float.Parse(dr["WTSTAT-P75"].ToString().Trim())) / 10));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P85"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P90"].ToString().Trim()))
                        {
                            Percentage_BMI = 85 + ((Weight - float.Parse(dr["WTSTAT-P85"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P90"].ToString().Trim()) - float.Parse(dr["WTSTAT-P85"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P90"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P95"].ToString().Trim()))
                        {
                            Percentage_BMI = 90 + ((Weight - float.Parse(dr["WTSTAT-P90"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P95"].ToString().Trim()) - float.Parse(dr["WTSTAT-P90"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P95"].ToString().Trim()) && Weight < float.Parse(dr["WTSTAT-P97"].ToString().Trim()))
                        {
                            Percentage_BMI = 95 + ((Weight - float.Parse(dr["WTSTAT-P95"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P97"].ToString().Trim()) - float.Parse(dr["WTSTAT-P95"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (Weight > float.Parse(dr["WTSTAT-P97"].ToString().Trim()))
                        {
                            Percentage_BMI = 97 + ((Weight - float.Parse(dr["WTSTAT-P97"].ToString().Trim())) / ((float.Parse(dr["WTSTAT-P97"].ToString().Trim()) - float.Parse(dr["WTSTAT-P95"].ToString().Trim())) / 2));
                            break;
                        }
                    }
                }
            }
            return Percentage_BMI;
        }

        private float Calculate_LengthAge(int Age, float Height, string Sex)
        {
            float Percentage_WeightAge = 0;
            if (Height > 0)
            {
                float BMI_Age = Age;
                BMI_Age = Age + 0.5f;
                Height = Height * 2.54f;
                foreach (DataRow dr in propDatatableLenghtAge.Rows)
                {
                    if (dr["LenAge-SEX"].ToString().Trim() == Sex.Trim() && float.Parse(dr["LenAge-Agemos"].ToString().Trim()) == BMI_Age)
                    {
                        if (Height < float.Parse(dr["LenAge-P3"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 0; break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P3"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P5"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 3 + ((Height - float.Parse(dr["LenAge-P3"].ToString().Trim())) / ((float.Parse(dr["LenAge-P5"].ToString().Trim()) - float.Parse(dr["LenAge-P3"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P5"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P10"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 5 + ((Height - float.Parse(dr["LenAge-P5"].ToString().Trim())) / ((float.Parse(dr["LenAge-P10"].ToString().Trim()) - float.Parse(dr["LenAge-P5"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P10"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P25"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 10 + ((Height - float.Parse(dr["LenAge-P10"].ToString().Trim())) / ((float.Parse(dr["LenAge-P25"].ToString().Trim()) - float.Parse(dr["LenAge-P10"].ToString().Trim())) / 15));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P25"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P50"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 25 + ((Height - float.Parse(dr["LenAge-P25"].ToString().Trim())) / ((float.Parse(dr["LenAge-P50"].ToString().Trim()) - float.Parse(dr["LenAge-P25"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P50"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P75"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 50 + ((Height - float.Parse(dr["LenAge-P50"].ToString().Trim())) / ((float.Parse(dr["LenAge-P75"].ToString().Trim()) - float.Parse(dr["LenAge-P50"].ToString().Trim())) / 25));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P75"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P90"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 75 + ((Height - float.Parse(dr["LenAge-P75"].ToString().Trim())) / ((float.Parse(dr["LenAge-P90"].ToString().Trim()) - float.Parse(dr["LenAge-P75"].ToString().Trim())) / 15));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P90"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P95"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 90 + ((Height - float.Parse(dr["LenAge-P90"].ToString().Trim())) / ((float.Parse(dr["LenAge-P95"].ToString().Trim()) - float.Parse(dr["LenAge-P90"].ToString().Trim())) / 5));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P95"].ToString().Trim()) && Height < float.Parse(dr["LenAge-P97"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 95 + ((Height - float.Parse(dr["LenAge-P95"].ToString().Trim())) / ((float.Parse(dr["LenAge-P97"].ToString().Trim()) - float.Parse(dr["LenAge-P95"].ToString().Trim())) / 2));
                            break;
                        }
                        else if (Height > float.Parse(dr["LenAge-P97"].ToString().Trim()))
                        {
                            Percentage_WeightAge = 97 + ((Height - float.Parse(dr["LenAge-P97"].ToString().Trim())) / ((float.Parse(dr["LenAge-P97"].ToString().Trim()) - float.Parse(dr["LenAge-P95"].ToString().Trim())) / 2));
                            break;
                        }
                    }
                }
            }
            return Percentage_WeightAge;
        }

        private void txtBmiFrm_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBmiFrm.Text.Trim()))
                txtBmiFrm.Text = "0.00";
            else if (!string.IsNullOrEmpty(txtBmiFrm.Text.Trim()))
            {
                if (txtBmiFrm.Text.Trim().Substring(0, 1) == ".")
                   txtBmiFrm.Text= txtBmiFrm.Text.Trim().Replace(".", "0.");
            }
        }

        private void txtBmiTo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBmiTo.Text.Trim()))
                txtBmiTo.Text = "0.00";
            else if (!string.IsNullOrEmpty(txtBmiTo.Text.Trim()))
            {
                if (txtBmiTo.Text.Trim().Substring(0, 1) == ".")
                    txtBmiTo.Text = txtBmiTo.Text.Trim().Replace(".", "0.");
            }
        }

        private void txtFrom_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFrom.Text.Trim()))
                txtFrom.Text = "0";
        }

        private void txtTo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTo.Text.Trim()))
                txtTo.Text = "0";
        }

        //private void DataaddtoErrortable(DataGridViewRow Entity)
        //{
        //    string Data = string.Empty;
        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //        if (!string.IsNullOrEmpty(Entity.Cells["H1"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W1"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H1"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W1"].Value.ToString().Trim()) == 0)
        //            {
        //                string Val = string.Empty;
                        
        //                if (float.Parse(Entity.Cells["H1"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA1"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT1"].Value.ToString().Trim() + " " + Entity.Cells["HTASK1"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W1"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA1"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT1"].Value.ToString().Trim() + " " + Entity.Cells["WTASK1"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
        //        {

        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT1"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["WEIGHT1"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT1"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT1"].Value.ToString().Trim() + " " + "WEIGHT [M]";

                        
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //        if (!string.IsNullOrEmpty(Entity.Cells["H2"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W2"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H2"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W2"].Value.ToString().Trim()) == 0)
        //            {
        //                string Val = string.Empty;
        //                if (float.Parse(Entity.Cells["H2"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA2"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT2"].Value.ToString().Trim() + " " + Entity.Cells["HTASK2"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W2"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA2"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT2"].Value.ToString().Trim() + " " + Entity.Cells["WTASK2"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
        //        {

        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT2"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["WEIGHT2"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT2"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT2"].Value.ToString().Trim() + " " + "WEIGHT [M]";

                        
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //        if (!string.IsNullOrEmpty(Entity.Cells["H3"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W3"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H3"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W3"].Value.ToString().Trim()) == 0)
        //            {
        //                string Val = string.Empty;

        //                if (float.Parse(Entity.Cells["H3"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA3"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT3"].Value.ToString().Trim() + " " + Entity.Cells["HTASK3"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W3"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA3"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT3"].Value.ToString().Trim() + " " + Entity.Cells["WTASK3"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
        //        {

        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT3"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["WEIGHT3"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT3"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT3"].Value.ToString().Trim() + " " + "WEIGHT [M]";

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty; 
        //        if (!string.IsNullOrEmpty(Entity.Cells["H4"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W4"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H4"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W4"].Value.ToString().Trim()) == 0)
        //            {
        //                string Val = string.Empty;
        //                //char TempChar = '4';
        //                if (float.Parse(Entity.Cells["H4"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA4"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT4"].Value.ToString().Trim() + " " + Entity.Cells["HTASK4"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W4"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA4"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT4"].Value.ToString().Trim() + " " + Entity.Cells["WTASK4"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
        //        {
        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT4"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["WEIGHT4"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT4"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT4"].Value.ToString().Trim() + " " + "WEIGHT [M]";

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //        if (!string.IsNullOrEmpty(Entity.Cells["H5"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W5"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H5"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W5"].Value.ToString().Trim()) == 0)
        //            {
        //                 string Val = string.Empty;
        //                if (float.Parse(Entity.Cells["H5"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA5"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT5"].Value.ToString().Trim() + " " + Entity.Cells["HTASK5"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W5"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA5"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT5"].Value.ToString().Trim() + " " + Entity.Cells["WTASK5"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
        //        {
        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT5"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["WEIGHT5"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT5"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT5"].Value.ToString().Trim() + " " + "WEIGHT [M]";

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //        if (!string.IsNullOrEmpty(Entity.Cells["H6"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W6"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H6"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W6"].Value.ToString().Trim()) == 0)
        //            {
        //                 string Val = string.Empty;
        //                //char TempChar = '6';
        //                if (float.Parse(Entity.Cells["H6"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA6"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT6"].Value.ToString().Trim() + " " + Entity.Cells["HTASK6"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W6"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA6"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT6"].Value.ToString().Trim() + " " + Entity.Cells["WTASK6"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
        //        {

        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT6"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["WEIGHT6"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT6"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT6"].Value.ToString().Trim() + " " + "WEIGHT [M]";

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //        if (!string.IsNullOrEmpty(Entity.Cells["H7"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W7"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H7"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W7"].Value.ToString().Trim()) == 0)
        //            {
        //                 string Val = string.Empty;
        //                //char TempChar = '7';
        //                if (float.Parse(Entity.Cells["H7"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA7"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT7"].Value.ToString().Trim() + " " + Entity.Cells["HTASK7"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W7"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA7"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT7"].Value.ToString().Trim() + " " + Entity.Cells["WTASK7"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
        //        {

        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT7"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["WEIGHT7"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT7"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT7"].Value.ToString().Trim() + " " + "WEIGHT [M]";

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()) || !string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
        //    {
        //        DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //        if (!string.IsNullOrEmpty(Entity.Cells["H8"].Value.ToString().Trim()) && !string.IsNullOrEmpty(Entity.Cells["W8"].Value.ToString().Trim()))
        //        {
        //            if (float.Parse(Entity.Cells["H8"].Value.ToString().Trim()) == 0 || float.Parse(Entity.Cells["W8"].Value.ToString().Trim()) == 0)
        //            {
        //                 string Val = string.Empty;
        //                if (float.Parse(Entity.Cells["H8"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["HA8"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data = Entity.Cells["HEIGHT8"].Value.ToString().Trim() + " " + Entity.Cells["HTASK8"].Value.ToString().Trim() + " [U] " + Val;
        //                }
        //                if (float.Parse(Entity.Cells["W8"].Value.ToString().Trim()) == 0)
        //                {
        //                    string[] Temp = Entity.Cells["WA8"].Value.ToString().Trim().Split('"');
        //                    if (Temp.Length > 0)
        //                    {
        //                        for (int i = 0; i < Temp.Length; i++)
        //                            Val = Temp[i] + "'";
        //                    }
        //                    Data += Entity.Cells["WEIGHT8"].Value.ToString().Trim() + " " + Entity.Cells["WTASK8"].Value.ToString().Trim() + " [U] " + Val;
        //                }

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()) || string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
        //        {

        //            if (string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()) && string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
        //            {
        //            }
        //            else
        //            {
        //                //DataRow dr = ErrorTable.NewRow(); Data = string.Empty;
        //                if (string.IsNullOrEmpty(Entity.Cells["HEIGHT8"].Value.ToString().Trim()))
        //                    Data = Entity.Cells["WEIGHT8"].Value.ToString().Trim() + " " + "HEIGHT [M]";
        //                if (string.IsNullOrEmpty(Entity.Cells["WEIGHT8"].Value.ToString().Trim()))
        //                    Data += Entity.Cells["HEIGHT8"].Value.ToString().Trim() + " " + "WEIGHT [M]";

        //                //dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //                //dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //                //dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //                //dr.SetField("Uninterpreted/Missing Data", Data);
        //                //ErrorTable.Rows.Add(dr);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(Data.Trim()))
        //        {
        //            dr.SetField("App#", Entity.Cells["APPNO"].Value.ToString().Trim());
        //            dr.SetField("Child's Name", Entity.Cells["NAME"].Value.ToString().Trim());
        //            dr.SetField("DOB", Entity.Cells["DOB"].Value.ToString().Trim());
        //            dr.SetField("Uninterpreted/Missing Data", Data);
        //            ErrorTable.Rows.Add(dr);
        //        }

        //    }

            
        //}

        private void PrintErrorPdf()
        {
            if (ErrorTable.Rows.Count > 0)
            {
                string PdfName = "Pdf File";
                PdfName = "GrowthChartErrors.pdf";

                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

                FileStream fs = new FileStream(PdfName, FileMode.Create);

                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
               
                cb = writer.DirectContent;

                try
                {
                    PdfPTable table = new PdfPTable(4);
                    table.TotalWidth = 500f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 40f, 15f, 70f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;

                    string[] HeaderSeq4 = { "App #", "Child's Name", "DOB", "ServiceUninterpreted/Missing Data" };
                    for (int i = 0; i < HeaderSeq4.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        table.AddCell(cell);
                    }

                    foreach (DataRow dr in ErrorTable.Rows)
                    {
                        PdfPCell S1 = new PdfPCell(new Phrase(dr["APPNO"].ToString().Trim(), TableFont));
                        S1.HorizontalAlignment = Element.ALIGN_LEFT;
                        S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S1);

                        PdfPCell S2 = new PdfPCell(new Phrase(dr["NAME"].ToString().Trim(), TableFont));
                        S2.HorizontalAlignment = Element.ALIGN_LEFT;
                        S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S2);

                        PdfPCell S3 = new PdfPCell(new Phrase(dr["DOB"].ToString().Trim(), TableFont));
                        S3.HorizontalAlignment = Element.ALIGN_LEFT;
                        S3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S3);

                        PdfPCell S4 = new PdfPCell(new Phrase(dr["DATA"].ToString().Trim(), TableFont));
                        S4.HorizontalAlignment = Element.ALIGN_LEFT;
                        S4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S4);
                    }
                    document.Add(table);
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();

            }
        }




    }
}