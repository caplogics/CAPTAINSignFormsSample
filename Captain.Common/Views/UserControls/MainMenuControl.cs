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
using Captain.Common.Views.UserControls;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.Text.RegularExpressions;
using Captain.Common.Model.Parameters;
using Captain.DatabaseLayer;
using Captain.Common.Views.Controls.Compatibility;
using System.Globalization;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class MainMenuControl : BaseUserControl
    {
        public MainMenuControl(BaseForm baseForm)
            : base(baseForm)
        {
            InitializeComponent();
            _model = new CaptainModel();

            BaseForm = baseForm;
            Agency = BaseForm.BaseAgency;
            Dept = BaseForm.BaseDept;
            Program = BaseForm.BaseProg;
            ProgramYear = BaseForm.BaseYear;
            Old_AppNo = ApplicationNo = BaseForm.BaseApplicationNo;

            applicationNameControl = new ApplicationNameControl(BaseForm);

            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            if (propAgencyControlDetails != null)
            {
                if (propAgencyControlDetails.AllowClientINQ == "1")
                    Btn_Deep_Search.Visible = true;
                else Btn_Deep_Search.Visible = false;

            }
            else Btn_Deep_Search.Visible = false;
            //if (BaseForm.UserID == "SYSTEM")
            //    Btn_Deep_Search.Visible = true;
            //else
            //    Btn_Deep_Search.Visible = false;

            this.oToolbarMnustrip.Visible = false;
            pnlLeftblock.Width = 340;
        }


        bool Loading_Complete = false;
        string PrvPanelCode = null;
        string SearcgCategory = null;
        string SearchFor = null;
        string SearchCaseType = null;
        string SearchCaseWRK = null;
        string Hierarchy = null;
        string SearchHie = null;
        string SelAgency = null;
        string SelDept = null;
        string SelProg = null;
        string SelYear = null;
        string DefAgy = null;
        string DefDept = null;
        string DefProg = null;
        string DefYear = null;
        string DepYear = null;
        bool DefHieExist = false;
        string strAgencyName = null;
        string strDeptName = null;
        string strProgName = null;
        string strNameFormat = null, strCwFormat = null;
        string Old_AppNo = null;
        bool Adv_search = true;

        private CaptainModel _model = null;
        public ApplicationNameControl applicationNameControl;
        #region properties

        public string Agency { get; set; }

        public string Dept { get; set; }


        public string Program { get; set; }

     

        public string ProgramYear { get; set; }


        public string AgencyName { get; set; }


        public string DeptName { get; set; }


        public string ProgramName { get; set; }


        public string ApplicationNo { get; set; }

        public char AddPriv { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        public AgencyControlEntity propAgencyControlDetails { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
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

        private void GetDefaultHierarchy()
        {
            DefAgy = DefDept = DefProg = DefYear = null;
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetUserDefHierarchy(BaseForm.UserID);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DefAgy = dr["Agy"].ToString();
                    DefDept = dr["Dept"].ToString();
                    DefProg = dr["Prog"].ToString();
                    DefYear = dr["DefYear"].ToString();
                    Agency = DefAgy;
                    Dept = DefDept;
                    Program = DefProg;
                    if (!(String.IsNullOrEmpty(DefAgy)) && !(String.IsNullOrEmpty(DefDept)) && !(String.IsNullOrEmpty(DefProg)))
                        DefHieExist = true;
                }
            }
        }


        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAPTAIN", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }
            return (TmpCode);
        }

        public void ShowHierachyandApplNo(string strAgency, string strDept, string strProg, string strYear1, string strApplicationNo)
        {

            Agency = strAgency;
            Dept = strDept;
            Program = strProg;
            ProgramYear = strYear1;

            CaseMstEntity caseMstEntity = null;
            List<CaseSnpEntity> caseSnpEntity = null;
            string strYear = strYear1;
            string strApplNo = strApplicationNo;
            {
                caseMstEntity = _model.CaseMstData.GetCaseMST(strAgency, strDept, strProg, strYear1, strApplNo);
                if (caseMstEntity != null)
                {
                    strApplNo = caseMstEntity.ApplNo;
                    strYear = caseMstEntity.ApplYr;
                    ApplicationNo = strApplNo;
                }
            }


            if (string.IsNullOrEmpty(ProgramYear))
                ProgramYear = "    ";

            strAgencyName = strAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", strAgency, strDept, strProg);    // Yeswanth
            strDeptName = strDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", strAgency, strDept, strProg);
            strProgName = strProg + " - " + _model.lookupDataAccess.GetHierachyDescription("3", strAgency, strDept, strProg);
            AgencyName = strAgencyName;
            DeptName = strDeptName;
            ProgramName = strProgName;
            BaseForm.BaseAgency = strAgency;
            BaseForm.BaseDept = strDept;
            BaseForm.BaseProg = strProg;

            if (string.IsNullOrEmpty(strYear))
                strYear = "    ";

            BaseForm.BaseYear = strYear;

            ProgramDefinitionEntity ProgramDefinition = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (ProgramDefinition != null)
            {
                if (BaseForm.BaseYear.Trim() != ProgramDefinition.DepYear.Trim())
                {
                    applicationNameControl.lblMsg.Text = "** Current Program Year is " + ProgramDefinition.DepYear.Trim() + "... you are not in the Current Program Year **";
                    applicationNameControl.lblMsg.Visible = true;
                }
                else
                {
                    applicationNameControl.lblMsg.Visible = false;
                }
            }

            if (caseMstEntity != null)
            {
                caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strProg, strYear, strApplNo);
                BaseForm.BaseApplicationNo = ApplicationNo;

                if (Adv_search && BaseForm.BaseTopApplSelect == "Y")
                {
                    Get_Name_Format();
                    //TxtAppNo.Text = ApplicationNo;     // Yeswanth
                    //BtnSearchApp_Click(BtnSearchApp, EventArgs.Empty);

                    MainMenuControl mainMenuControl = BaseForm.GetBaseUserControlMainMenu() as MainMenuControl;
                    if (mainMenuControl != null)
                    {
                        //if (BaseForm.BaseTopApplSelect == "Y")
                        mainMenuControl.RefreshMainMenu();
                        //else
                        //{
                        //    BtnAddApp.Visible = false;
                        //    Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                        //    BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = true;
                        //}
                    }
                }
            }
            else   // Yeswanth
                caseSnpEntity = null;
            if (caseMstEntity == null)
            {
                BaseForm.BaseCaseMstListEntity = null;
                BaseForm.BaseCaseSnpEntity = null;
                BaseForm.BaseApplicationNo = string.Empty; // null; Modified by Yeswanth on 01052013
            }

            //BaseForm.BaseTopApplSelect = "Y";
            BaseForm.BaseTopApplSelect = !string.IsNullOrEmpty(strApplicationNo.Trim()) ? "Y" : "N";

            BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, strAgencyName, strDeptName, strProgName, ProgramYear.ToString(), string.Empty, !string.IsNullOrEmpty(strApplicationNo.Trim()) ? "Display" : string.Empty);
        }


        //Begining of Latest Code By Yeswanth

        private void On_ADV_SerachFormClosed(object sender, FormClosedEventArgs e)
        {
            AdvancedMainMenuSearch form = sender as AdvancedMainMenuSearch;
            if (form.DialogResult == DialogResult.OK)
            {
                if (BaseForm.BasePIPDragSwitch == "Y")
                {
                    if (BaseForm.BaseApplicationNo.Length > 1)
                    {
                        BaseForm.BaseTopApplSelect = "Y";
                        ShowHierachyandApplNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);

                    }
                }
                else
                {
                    string Selected_App_key = null, BaseForm_Priv_Hierarchy = null;
                    //                BaseForm_Priv_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
                    BaseForm_Priv_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear;

                    Selected_App_key = form.GetSelectedApplicant();
                    // setTreeviewhierachy(BaseForm.BusinessModuleID);
                    if (!string.IsNullOrEmpty(Selected_App_key))
                    {
                        MainPanel.Visible = true;

                        //if (BaseForm_Priv_Hierarchy.Trim() != Selected_App_key.Substring(0, 6))
                        if (BaseForm_Priv_Hierarchy.Trim() != Selected_App_key.Substring(0, 10).Trim())
                        {
                            BaseForm.BaseAgency = Selected_App_key.Substring(0, 2);
                            BaseForm.BaseDept = Selected_App_key.Substring(2, 2);
                            BaseForm.BaseProg = Selected_App_key.Substring(4, 2);
                            BaseForm.BaseYear = Selected_App_key.Substring(6, 4);
                            BaseForm.RefreshNavigationTabs(Selected_App_key.Substring(0, 2) + Selected_App_key.Substring(2, 2) + Selected_App_key.Substring(4, 2));
                            BaseForm.BaseTopApplSelect = "N";
                            TxtAppNo.Clear();
                            GvwAppHou.Rows.Clear();

                            //BaseForm.ContentTabs.TabPages.Clear();
                            BaseForm.strcurrentModuleID = BaseForm.BusinessModuleID;
                            BaseForm.frmHiEScren = "Y";
                            BaseForm.BuildMenuTree();
                        }
                        else
                        {
                            if (BaseForm.BaseTopApplSelect == "Y")
                            {
                                if ((Selected_App_key.Length > 10))
                                {
                                    if (BaseForm.BaseApplicationNo == Selected_App_key.Substring(10, 8))
                                        Selected_App_key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo;
                                }
                                else
                                {
                                    Selected_App_key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo;
                                }
                            }
                            else
                            {
                                TxtAppNo.Clear();
                                GvwAppHou.Rows.Clear();
                            }
                        }



                        if (Selected_App_key.Length > 10)
                        {
                            BaseForm.BaseTopApplSelect = "Y";
                            ShowHierachyandApplNo(Selected_App_key.Substring(0, 2), Selected_App_key.Substring(2, 2), Selected_App_key.Substring(4, 2), Selected_App_key.Substring(6, 4), Selected_App_key.Substring(10, 8));

                        }
                        else
                            ShowHierachyandApplNo(Selected_App_key.Substring(0, 2), Selected_App_key.Substring(2, 2), Selected_App_key.Substring(4, 2), Selected_App_key.Substring(6, 4), string.Empty);

                        if (BaseForm.BaseTopApplSelect == "Y" || !string.IsNullOrEmpty(BaseForm.BaseApplicationNo.Trim()))
                        {
                            BtnAddApp.Visible = false;
                            Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                            BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = true;
                        }

                        ProgramDefinitionEntity SprogramEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                        if (SprogramEntity != null)
                        {
                            //ApplicationNameControl applicationNameControl = new ApplicationNameControl(BaseForm);
                            if (BaseForm.BaseYear.Trim() != SprogramEntity.DepYear.Trim())
                            {
                                applicationNameControl.lblMsg.Text = "** Current Program Year is " + SprogramEntity.DepYear.Trim() + "... you are not in the Current Program Year **";
                                applicationNameControl.lblMsg.Visible = true;
                            }
                            else
                            {
                                applicationNameControl.lblMsg.Visible = false;
                            }
                        }

                    }
                }
                //      
                Adv_search = false;
            }
        }


        private void On_First_Add_App(object sender, FormClosedEventArgs e)
        {
            //MainMenuAddApplicantForm form = sender as MainMenuAddApplicantForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    string Selected_App_key = string.Empty;
            //    Selected_App_key = form.Get_Dragged_App_No();
            //    if (!string.IsNullOrEmpty(Selected_App_key))
            //    {
            //        MainPanel.Visible = true;

            //        BaseForm.BaseTopApplSelect = "N";
            //        TxtAppNo.Clear();
            //        GvwAppHou.Rows.Clear();

            //        if (Selected_App_key.Length >= 8)
            //        {
            //            BaseForm.BaseTopApplSelect = "Y";
            //            TxtAppNo.Text = Selected_App_key; // Selected_App_key.Substring(10, 8);
            //            TxtAppNo_LostFocus(TxtAppNo, EventArgs.Empty);
            //            // ShowHierachyandApplNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Selected_App_key);

            //            BtnAddApp.Visible = false;
            //            Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
            //            BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = true;
            //        }
            //    }
            //}
            Adv_search = false;
        }

        public void TxtAppNo_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtAppNo.Text))
            {
                Process_From_Scr = "MainMenu";
                TxtAppNo.Text = SetLeadingZeros(TxtAppNo.Text);
                BtnSearchApp_Click(BtnSearchApp, EventArgs.Empty);
                if (!string.IsNullOrEmpty(Old_AppNo))
                {
                    Adv_search = false;
                    ShowHierachyandApplNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, TxtAppNo.Text);
                }
            }
        }

        public string Process_From_Scr = "MainMenu";
        public void BtnSearchApp_Click(object sender, EventArgs e)
        {

            //string Gbl_Hierarchy = BaseForm.UserProfile.Agency + BaseForm.UserProfile.Dept + BaseForm.UserProfile.Prog + BaseForm.UserProfile.Year;
            //string Gbl_Hierarchy = Agency + Dept + Program + ProgramYear;
            string Gbl_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear, Mst_Active = "", Snp_Active = "";
            Old_AppNo = BaseForm.BaseApplicationNo;

            //ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(Agency, Dept, Program);
            //if (programEntity != null)
            //{
            //    ProgramDefinition = programEntity;
            //}

            //GvwAppHou.Rows.Clear();
            DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "ALL", null, null, TxtAppNo.Text, null, null, null, null, null, null, null, null, null, null, Gbl_Hierarchy, null, BaseForm.UserID, string.Empty, string.Empty, null);
            DataTable dt = ds.Tables[0];
            string RecentHierChanged = string.Empty;

            if (dt.Rows.Count > 0)
            {
                DataRow dr_Mst = dt.Rows[0];
                GvwAppHou.Rows.Clear();

                string Mst_App_Key = dr_Mst["Agency"].ToString() + dr_Mst["Dept"].ToString() + dr_Mst["Prog"].ToString() + dr_Mst["SnpYear"] + dr_Mst["AppNo"];    //RecKey

                //DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuGetHouseDetails(Mst_App_Key);
                //DataTable dt1 = ds1.Tables[0];
                GvwAppHou.Rows.Clear();
                try
                {
                    int TmpRows = 0;
                    //foreach (DataRow dr in dt1.Rows)

                    string TMPSnp_FName = "", TMPSnp_LName = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        int rowIndex = 0;
                        string TmpName = null, TmpAddress = null, TmpDOB = null, TmpUpdated = null, TmpSsn = null;
                        int TmpLength = 0;
                        char TmpSpace = ' ';

                        TmpName = LookupDataAccess.GetMemberName(dr["Fname"].ToString(), dr["MName"].ToString(), dr["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat.ToString());
                        TMPSnp_FName = dr["Fname"].ToString().Trim();
                        TMPSnp_LName = string.Empty;
                        TmpDOB = null;


                        TmpDOB = dr["DOB"].ToString();
                        if (string.IsNullOrEmpty(TmpDOB))
                        {
                            TMPSnp_LName = dr["Lname"].ToString().Trim();
                        }
                        //TmpDOB = dr["Dob"].ToString();
                        //TmpDOB = TmpDOB.Substring(4,2) + '/' + TmpDOB.Substring(1,2) + '/' + TmpDOB.Substring(7,2);
                        string[] time2 = Regex.Split(TmpDOB.ToString(), " ");
                        //if (!string.IsNullOrEmpty(time2[0].Trim()))
                        //    TmpDOB = Convert.ToDateTime(DateTime.ParseExact(time2[0].ToString(), "MM/dd/yyyy", null)).ToString();

                        if (!string.IsNullOrEmpty(time2[0].Trim())) TmpDOB = LookupDataAccess.Getdate(time2[0]);
                        else
                            TmpDOB = time2[0];

                        string TmpClientID = string.Empty;

                        if (!string.IsNullOrEmpty(dr["ClientID"].ToString()))
                            TmpClientID = dr["ClientID"].ToString();

                        //TmpName = dr["Fname"] + ", " + dr["Lname"] + "  " + dr["MName"];
                        TmpSsn = dr["Ssn"].ToString();

                        DataSet ds2 = Captain.DatabaseLayer.MainMenu.MainMenuOtherPrograms(TmpSsn, Mst_App_Key.Substring(0, 10), BaseForm.UserID, null, TMPSnp_FName, TMPSnp_LName, TmpDOB, TmpClientID);

                        DataTable dtR = new DataTable();
                        dtR = ds2.Tables[0].Clone();
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            int i = 0;
                            foreach (DataRow dr1 in ds2.Tables[0].Rows)
                            {
                                string strHie = dr1["Hierarchy"].ToString();
                                string strAgy = strHie.Substring(0, 6).Substring(0, 2);
                                if (strAgy == BaseForm.BaseAdminAgency)
                                {
                                    dtR.ImportRow(dr1);
                                }
                            }

                        }

                        DataTable dt2 = dtR;// ds2.Tables[0];

                        TmpLength = (9 - TmpSsn.Length);
                        for (int i = 0; i < TmpLength; i++)
                            TmpAddress += TmpSpace;
                        TmpSsn += TmpAddress;
                        //TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);
                        TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", string.Empty, string.Empty);
                        TmpDOB = null;


                        TmpDOB = dr["DOB"].ToString();
                        //TmpDOB = dr["Dob"].ToString();
                        //TmpDOB = TmpDOB.Substring(4,2) + '/' + TmpDOB.Substring(1,2) + '/' + TmpDOB.Substring(7,2);
                        string[] time = Regex.Split(TmpDOB.ToString(), " ");
                        if (!string.IsNullOrEmpty(time[0].Trim())) TmpDOB = LookupDataAccess.Getdate(time[0]);
                        else
                            TmpDOB = time[0];
                        TmpUpdated = null;
                        TmpUpdated = dr["SNP_DATE_LSTC"].ToString();
                        //TmpUpdated = dr["Updated"].ToString();

                        time = null;
                        time = Regex.Split(TmpUpdated.ToString(), " ");
                        TmpUpdated = time[0];
                        //TmpUpdated = TmpUpdated.Substring(3, 2) + '/' + TmpUpdated.Substring(0, 2) + '/' + TmpUpdated.Substring(6, 2);


                        //if (chkOtherProgram.Checked)
                        //{
                        //DataSet ds2 = Captain.DatabaseLayer.MainMenu.MainMenuOtherPrograms(TmpSsn, Mst_App_Key.Substring(0, 10), BaseForm.UserID, null, TMPSnp_FName, TMPSnp_LName, TmpDOB);
                        //DataTable dt2 = ds2.Tables[0];

                        int TmpOtherCount = 0;
                        TmpOtherCount = dt2.Rows.Count;
                        string[] TmpOtherProgstr = new string[TmpOtherCount];

                        string TmpOtherProgstr1 = null;
                        string TmpOtherProgstr2 = null;
                        // string TmpOtherProgstr3 = null;
                        if (dt2.Rows.Count > 0)
                        {
                            int i = 0;
                            foreach (DataRow dr1 in dt2.Rows)
                            {
                                TmpOtherProgstr[i] = dr1["Hierarchy"].ToString();
                                switch (i)
                                {
                                    case 0: TmpOtherProgstr1 = TmpOtherProgstr[i]; break;
                                    case 1: TmpOtherProgstr2 = TmpOtherProgstr[i]; break;
                                        //case 2: TmpOtherProgstr3 = TmpOtherProgstr[i]; break;
                                }
                                i++;
                            }

                            RecentHierChanged = dt2.Rows[0]["Hierarchy"].ToString();
                        }

                        rowIndex = GvwAppHou.Rows.Add(TmpName, TmpSsn, TmpDOB, TmpUpdated, TmpOtherProgstr1, TmpOtherProgstr2); //, TmpOtherProgstr3
                        GvwAppHou.Rows[rowIndex].Tag = dr;

                        //if (dr["FamSeq"].ToString() == dr_Mst["RecFamSeq"].ToString())
                        //    GvwAppHou.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                        if (dr["AppNo"].ToString().Substring(10, 1) == "A")
                            GvwAppHou.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromName("@captainBlue");

                        Mst_Active = dr["MST_ACTIVE_STATUS"].ToString().Trim();
                        Snp_Active = dr["AppStatus"].ToString().Trim();// != "A")

                        if (dr["AppNo"].ToString().Substring(10, 1) == "A" && Mst_Active != "Y")
                            GvwAppHou.Rows[rowIndex].Cells["ZCRZIP"].Style.ForeColor = Color.Red;

                        if (Snp_Active != "A")
                            GvwAppHou.Rows[rowIndex].Cells["ZCRZIP"].Style.ForeColor = Color.Red;



                        if (TmpOtherCount > 2)  //TmpOtherCount > 3
                        {
                            int AddOtherRows = (TmpOtherCount - 2) / 2;
                            int aa = (TmpOtherCount - 2) % 2;
                            int i = 0;
                            for (int R = 0; R < AddOtherRows;) //for (int i = 3; i < AddOtherRows; )
                            {
                                TmpOtherProgstr1 = TmpOtherProgstr[i + 2]; //TmpOtherProgstr1 = TmpOtherProgstr[i];
                                TmpOtherProgstr2 = TmpOtherProgstr[i + 1 + 2]; //TmpOtherProgstr2 = TmpOtherProgstr[i + 1];
                                                                               //TmpOtherProgstr3 = TmpOtherProgstr[i + 2];
                                rowIndex = GvwAppHou.Rows.Add(" ", " ", " ", " ", TmpOtherProgstr1, TmpOtherProgstr2); //, TmpOtherProgstr3
                                GvwAppHou.Rows[rowIndex].Tag = dr;
                                i += 2;
                                R++;
                            }
                            if (aa > 0)
                            {
                                TmpOtherProgstr1 = TmpOtherProgstr2 = null;
                                switch (aa)
                                {
                                    case 1: TmpOtherProgstr1 = TmpOtherProgstr[TmpOtherProgstr.Length - 1]; break;
                                    case 2:
                                        TmpOtherProgstr1 = TmpOtherProgstr[TmpOtherProgstr.Length - 1];
                                        TmpOtherProgstr2 = TmpOtherProgstr[TmpOtherProgstr.Length - 2]; break;
                                }
                                rowIndex = GvwAppHou.Rows.Add(" ", " ", " ", " ", TmpOtherProgstr1, TmpOtherProgstr2, " ");
                                GvwAppHou.Rows[rowIndex].Tag = dr;
                            }
                        }
                        //}
                        //else
                        //{
                        //    rowIndex = GvwAppHou.Rows.Add(TmpName, TmpSsn, TmpDOB, TmpUpdated, string.Empty, string.Empty); //, TmpOtherProgstr3
                        //    GvwAppHou.Rows[rowIndex].Tag = dr;
                        //    if (dr["AppNo"].ToString().Substring(10, 1) == "A")
                        //        GvwAppHou.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                        //    Mst_Active = dr["MST_ACTIVE_STATUS"].ToString().Trim();
                        //    Snp_Active = dr["AppStatus"].ToString().Trim();// != "A")

                        //    if (dr["AppNo"].ToString().Substring(10, 1) == "A" && Mst_Active != "Y")
                        //        GvwAppHou.Rows[rowIndex].Cells["ZCRZIP"].Style.ForeColor = Color.Red;

                        //    if (Snp_Active != "A")
                        //        GvwAppHou.Rows[rowIndex].Cells["ZCRZIP"].Style.ForeColor = Color.Red;
                        //}
                        TmpRows++;
                    }

                    if (TmpRows > 0)
                    {
                        GvwAppHou.Rows[0].Tag = 0; Old_AppNo = TxtAppNo.Text;
                        //if (!Adv_search)
                        //ShowHierachyandApplNo(Agency, Dept, Program, ProgramYear, TxtAppNo.Text);
                        GvwAppHou.Rows[0].Selected = true;

                    }


                    //else
                    //{
                    //    if(AddPriv == 'Y')
                    //        BtnAddApp.Visible = true;

                    //    Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                    //    BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = false;
                    //}

                }
                catch (Exception ex) { }

                // CaseSnpEntity snpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                btnRecent.Visible = false;
                string strMostRecentSwitch = BaseForm.BaseAgencyControlDetails.MostRecentintake;
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    AgencyControlEntity AgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(BaseForm.BaseAgency);
                    if (AgencyControlDetails != null)
                        strMostRecentSwitch = AgencyControlDetails.MostRecentintake;
                }

                if (strMostRecentSwitch == "Y")
                {

                    string strLastName = string.Empty;
                    if (string.IsNullOrEmpty(dt.Rows[0]["DOB"].ToString()))
                    {
                        strLastName = dt.Rows[0]["Lname"].ToString();
                    }
                    DataSet dsRecent = Captain.DatabaseLayer.MainMenu.MainMenuSearchEMS("APP", "APP", null, null, null, strLastName, dt.Rows[0]["Fname"].ToString(), null, null, null, null, null, null, null, null, null, LookupDataAccess.Getdate(dt.Rows[0]["DOB"].ToString()), BaseForm.UserID, "Single", string.Empty, string.Empty);

                    if (dsRecent.Tables[0].Rows.Count > 0)
                    {
                        if ((Convert.ToDateTime(dt.Rows[0]["SNP_DATE_LSTC"].ToString()) < Convert.ToDateTime(dsRecent.Tables[0].Rows[0]["SNP_DATE_LSTC"].ToString())) && dsRecent.Tables[0].Rows[0]["AGENCY"].ToString() == BaseForm.BaseAgency) //kranthi 05/20/2023:: Newly added checking the base agency in the recnt record
                        {
                            if (!(dsRecent.Tables[0].Rows[0]["AGENCY"].ToString() == BaseForm.BaseAgency && dsRecent.Tables[0].Rows[0]["DEPT"].ToString() == BaseForm.BaseDept && dsRecent.Tables[0].Rows[0]["PROG"].ToString() == BaseForm.BaseProg && dsRecent.Tables[0].Rows[0]["SnpYear"].ToString().Trim() == BaseForm.BaseYear.Trim() && dsRecent.Tables[0].Rows[0]["APPLICANTNO"].ToString() == BaseForm.BaseApplicationNo))
                            {
                                btnRecent.Visible = true;
                                List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, dsRecent.Tables[0].Rows[0]["Agency"].ToString() + dsRecent.Tables[0].Rows[0]["Dept"].ToString() + dsRecent.Tables[0].Rows[0]["Prog"].ToString(), BaseForm.UserID, BaseForm.BaseAdminAgency);
                                string strProgramName = string.Empty;
                                if (programEntityList.Count > 0)
                                    strProgramName = programEntityList[0].ProgramName;

                                btnRecent.Text = LookupDataAccess.Getdate(dsRecent.Tables[0].Rows[0]["SNP_DATE_LSTC"].ToString()) + " -- Intake from [" + dsRecent.Tables[0].Rows[0]["Agency"].ToString() + dsRecent.Tables[0].Rows[0]["Dept"].ToString() + dsRecent.Tables[0].Rows[0]["Prog"].ToString() + dsRecent.Tables[0].Rows[0]["SnpYear"].ToString().Trim() + " - " + strProgramName + "] Application # " + dsRecent.Tables[0].Rows[0]["APPLICANTNO"].ToString();

                            }
                        }
                    }
                }

            }
            else
            {
                if (Process_From_Scr == "CASE2001")
                {
                    // Kranthi Commented on 02/06/2023 :: Document-NCCAAtodo.docx :: When hierachy is changing the message is automatically coming 
                    //MessageBox.Show("No Records Found in Selected Hierarchy : '"
                    //              + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + ("  " + BaseForm.BaseYear).Trim() + "'", "CAPTAIN", MessageBoxButtons.OK);

                    if (AddPriv == 'Y')
                        BtnAddApp.Visible = false; // Aug 22 2018 changed true to false

                    Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                    BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(TxtAppNo.Text.Trim()))
                    {
                        AlertBox.Show("App# : " + TxtAppNo.Text + " Does Not Exist", MessageBoxIcon.Warning, null, ContentAlignment.MiddleCenter);
                        TxtAppNo.Clear();
                    }
                    if (BaseForm.BaseTopApplSelect == "Y")
                        TxtAppNo.Text = Old_AppNo;
                }
            }
        }

        private void BtnAdv_Search_Click(object sender, EventArgs e)
        {
            Adv_search = true;
            BaseForm.BasePIPDragSwitch = "N";
            AdvancedMainMenuSearch advancedMainMenuSearch = new AdvancedMainMenuSearch(BaseForm, true, true);
            advancedMainMenuSearch.StartPosition = FormStartPosition.CenterScreen;
            advancedMainMenuSearch.FormClosed += new FormClosedEventHandler(On_ADV_SerachFormClosed);
            advancedMainMenuSearch.ShowDialog();
        }

        private void Get_Name_Format()
        {

            //DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        strNameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
            //        strCwFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            //    }
            //}
            HierarchyEntity hiername = BaseForm.BaseCaseHierachyListEntity.Find(u => u.Agency.Trim() == Agency && u.Dept.Trim() == string.Empty && u.Prog.Trim() == string.Empty);
            if (hiername != null)
            {
                strNameFormat = hiername.CNFormat.ToString();
                strCwFormat = hiername.CWFormat.ToString();
            }
        }

        private void TxtAppNo_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            if (objArgs.KeyCode == Keys.Enter)
                if (!string.IsNullOrEmpty(TxtAppNo.Text))
                    TxtAppNo_LostFocus(TxtAppNo, EventArgs.Empty);
        }

        private void BtnAddApp_Click(object sender, EventArgs e)
        {
            //MainMenuAddApplicantForm AddAppForm = new MainMenuAddApplicantForm(BaseForm, 'N', null, null, null, null);
            //MainMenuAddApplicantForm AddAppForm = new MainMenuAddApplicantForm(BaseForm, 'N', string.Empty, string.Empty, string.Empty, string.Empty);
            //AddAppForm.FormClosed += new FormClosedEventHandler(On_First_Add_App);
            //AddAppForm.ShowDialog();
        }

        private void Get_ClientIntake_Priv()
        {
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetPrivilizes_byScrCode(BaseForm.UserID, BaseForm.BusinessModuleID, "CASE2001");
            DataTable dt = ds.Tables[0];
            string TmpHie = null, Current_HieTo_Compare = Agency + Dept + Program;
            //bool All_Hie_Exists = false;
            //char All_Hie_Add_Priv = 'N';
            if (dt.Rows.Count > 0)
            {
                AddPriv = 'U';

                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 1: Current_HieTo_Compare = Agency + Dept + "**"; break;
                        case 2: Current_HieTo_Compare = Agency + "****"; break;
                        case 3: Current_HieTo_Compare = "******"; break;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        TmpHie = dr["EFR_HIERARCHY"].ToString();

                        if (TmpHie.Substring(0, 2) == Current_HieTo_Compare.Substring(0, 2) &&
                            TmpHie.Substring(2, 2) == Current_HieTo_Compare.Substring(2, 2) &&
                            TmpHie.Substring(4, 2) == Current_HieTo_Compare.Substring(4, 2))
                        //dr["Hie_Used_Flag"].ToString() == "N")
                        { AddPriv = char.Parse(dr["EFR_ADD_PRIV"].ToString()); break; }
                    }
                    if (AddPriv == 'Y' || AddPriv == 'N')
                        break;
                }

                BtnAddApp.Visible = false;
                Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = true;

                CaseMstEntity Tmp_caseMstEntity = new CaseMstEntity();
                Tmp_caseMstEntity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);
                if (Tmp_caseMstEntity == null)//&& AddPriv == 'Y'
                {
                    if (AddPriv == 'Y' && _model.lookupDataAccess.CheckDefaultHierachy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.UserID) == Consts.Common.Exists)
                        BtnAddApp.Visible = false;// Aug 22 2018 changed true to false

                    Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                    BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = false;
                }
            }
        }

        public void Set_DefHie_as_BaseHie(string Agency, string Dept, string Prog, string Year)
        {
            TxtAppNo.Clear();
            GvwAppHou.Rows.Clear();
            ShowHierachyandApplNo(Agency, Dept, Prog, Year, string.Empty);
            BaseForm.RefreshNavigationTabs(Agency + Dept + Prog);
        }

        private void TxtAppNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void Navigation_Click(object sender, EventArgs e)
        {
            string Navigation_Option = string.Empty;
            bool Rec_Exists = false;
            if (sender == Btn_First)
                Navigation_Option = "First";
            else if (sender == BtnP10)
                Navigation_Option = "RR";
            else if (sender == BtnNxt)
            {
                if (BaseForm.BaseTopApplSelect == "N")
                    Navigation_Option = "First";
                else
                    Navigation_Option = "Next";
            }
            else if (sender == BtnPrev)
                if (BaseForm.BaseTopApplSelect == "N")
                    Navigation_Option = "First";
                else
                    Navigation_Option = "Previous";
            else if (sender == BtnN10)
                Navigation_Option = "FF";
            else if (sender == BtnLast)
                Navigation_Option = "Last";


            DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenu_Navigate_App(BaseForm.UserID, Navigation_Option,
                                                        BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Rec_Exists = true;
                    TxtAppNo.Text = ds.Tables[0].Rows[0]["MST_APP_NO"].ToString();
                    TxtAppNo_LostFocus(TxtAppNo, EventArgs.Empty);
                }
            }

            if (!Rec_Exists)
            {
                string Error_Msg = string.Empty;
                switch (Navigation_Option)
                {
                    case "First":
                    case "RR":
                    case "Previous": Error_Msg = "You are Already at the First Record"; break;

                    case "Next":
                    case "FF":
                    case "Last": Error_Msg = "You are Already at the Last Record"; break;
                }
                AlertBox.Show(Error_Msg, MessageBoxIcon.Warning, null, ContentAlignment.MiddleCenter);
            }
        }


        public void RefreshMainMenu()
        {
            if (BaseForm.BaseApplicationNo != null)
            {
                if (BaseForm.BaseApplicationNo.Trim() != string.Empty && TxtAppNo.Text.Trim() != string.Empty) // Murali added New design 03/24/2015
                {
                    //if (BaseForm.BaseApplicationNo.Trim() != TxtAppNo.Text.Trim())  // kranthi 08/14/2024 :: FSCAAWisej.docx :: Commented the below condition because the Mainmenu search is not refreshing when adding/deleting the members from client intake.
                    //{
                    Process_From_Scr = "CASE2001";
                    GvwAppHou.Rows.Clear();
                    TxtAppNo.Text = BaseForm.BaseApplicationNo;     // Yeswanth
                    if (BaseForm.BaseTopApplSelect != null)
                        BaseForm.BaseTopApplSelect = "Y";
                    BtnSearchApp_Click(BtnSearchApp, EventArgs.Empty);
                    //}
                }
                else
                {
                    if (BaseForm.BaseTopApplSelect == "Y")
                    {
                        RefreshMainMenuClientIntake();
                    }
                }
            }
            else
            {
                if (BaseForm.BaseTopApplSelect == "Y")
                {
                    RefreshMainMenuClientIntake();
                }
            }
        }
        public void RefreshClearControl()
        {
            TxtAppNo.Clear();
            GvwAppHou.Rows.Clear();
        }

        public void RefreshMainMenuClientIntake()
        {
            if (BaseForm.BaseApplicationNo != null)
            {
                if (BaseForm.BaseApplicationNo.Trim() != TxtAppNo.Text.Trim()) // Murali added New design 03/24/2015
                {
                    Process_From_Scr = "CASE2001";
                    GvwAppHou.Rows.Clear();
                    TxtAppNo.Text = BaseForm.BaseApplicationNo;     // Yeswanth
                    BtnSearchApp_Click(BtnSearchApp, EventArgs.Empty);
                }
            }
            else
            {
                Process_From_Scr = "CASE2001";
                GvwAppHou.Rows.Clear();
                TxtAppNo.Text = string.Empty;
                BtnSearchApp_Click(BtnSearchApp, EventArgs.Empty);
            }
        }

        private void MainMenuControl_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Agency))
            {
                Get_Name_Format();

                Get_ClientIntake_Priv();
                //TxtAppNo.Validator = TextBoxValidation.IntegerValidator;
            }
            else
                this.Visible = false;


            if (!string.IsNullOrEmpty(ApplicationNo.Trim()) || BaseForm.BaseTopApplSelect == "Y")
            {
                BtnAddApp.Visible = false;
                Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = true;
            }

            ProgramDefinitionEntity ProgramDefinition = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (ProgramDefinition != null)
            {
                if (BaseForm.BaseYear.Trim() != ProgramDefinition.DepYear.Trim())
                {
                    applicationNameControl.lblMsg.Text = "** Current Program Year is " + ProgramDefinition.DepYear.Trim() + "... you are not in the Current Program Year **";
                    applicationNameControl.lblMsg.Visible = true;
                }
                else
                {
                    applicationNameControl.lblMsg.Visible = false;
                }
            }
        }

        public void Refresh_NavigationVisibility()
        {
            if (!string.IsNullOrEmpty(Agency))
            {
                Get_Name_Format();

                Get_ClientIntake_Priv();
                //TxtAppNo.Validator = TextBoxValidation.IntegerValidator;
            }
            else
                this.Visible = false;
        }

        private void Btn_Deep_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BaseForm.BaseApplicationNo))
            {
                //_privilegeEntity = privilegeEntity;
                if (BaseForm.BaseTopApplSelect == "Y")
                {
                    MMDeeperSearchForm MainDeeper = new MMDeeperSearchForm(BaseForm);
                    MainDeeper.StartPosition = FormStartPosition.CenterScreen;
                    MainDeeper.ShowDialog();
                }
                else
                    CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
            }
            else
                CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);

        }



        //  End of Latest Code By Yeswanth 

        private void setTreeviewhierachy(string BusinessModuleID)
        {
            switch (BusinessModuleID)
            {
                case Consts.Applications.Code.Administration:
                    InitializeModule("MiddleBanner.gif", TreeType.Administration);
                    break;
                case Consts.Applications.Code.HeadStart:
                    InitializeModule("MiddleBanner.gif", TreeType.HeadStart);
                    break;
                case Consts.Applications.Code.CaseManagement:
                    InitializeModule("MiddleBanner.gif", TreeType.CaseManagement);
                    break;
                case Consts.Applications.Code.EnergyRI:
                    InitializeModule("MiddleBanner.gif", TreeType.EnergyRI);
                    break;
                case Consts.Applications.Code.EnergyCT:
                    InitializeModule("MiddleBanner.gif", TreeType.EnergyCT);
                    break;
                case Consts.Applications.Code.EmergencyAssistance:
                    InitializeModule("MiddleBanner.gif", TreeType.EmergencyAssistance);
                    break;
                case Consts.Applications.Code.AccountsReceivable:
                    InitializeModule("MiddleBanner.gif", TreeType.AccountsReceivable);
                    break;
                case Consts.Applications.Code.HousingWeatherization:
                    InitializeModule("MiddleBanner.gif", TreeType.HousingWeatherization);
                    break;
                case Consts.Applications.Code.DashBoard:
                    InitializeModule("MiddleBanner.gif", TreeType.DashBoard);
                    break;
                case Consts.Applications.Code.HealthyStart:
                    InitializeModule("MiddleBanner.gif", TreeType.HealthyStart);
                    break;
            }
        }

        private void GvwAppHou_Click(object sender, EventArgs e)
        {

        }

        private void btnRecent_Click(object sender, EventArgs e)
        {
            //PIPUpdateApplicantForm pipupdateForm = new PIPUpdateApplicantForm(BaseForm, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            //pipupdateForm.FormClosed += new FormClosedEventHandler(PipupdateForm_FormClosed);
            //pipupdateForm.StartPosition = FormStartPosition.CenterScreen;
            //pipupdateForm.ShowDialog();
        }

        private void PipupdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //PIPUpdateApplicantForm form = sender as PIPUpdateApplicantForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    TxtAppNo_LostFocus(sender, e);
            //}
        }
        private void btncapcher_Click(object sender, EventArgs e)
        {
            List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID("01", BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);

            if (userPrivilege.Count > 0)
            {

                //PrivilegeEntity CaseVerPrivileges = userPrivilege.Find(u => u.Program == "TMS00009");
                //SampleTestForm form = new SampleTestForm(BaseForm, CaseVerPrivileges, string.Empty);
                //form.ShowDialog();
            }
        }

        private void InitializeModule(string headerTitleImage, TreeType treeViewType)
        {
            TreeViewControllerParameter treeViewControllerParameter = null;

            // pnlApplicationHeaderImage.BackgroundImage = new ImageResourceHandle(headerTitleImage);
            //  pnlApplicationHeaderImage.Width = 30;
            BaseForm.NavigationTreeView.Items.Clear();
            string HIE = HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg; ;


            treeViewControllerParameter = new TreeViewControllerParameter()
            {
                TreeType = treeViewType,
                NavigationBar = BaseForm.NavigationTreeView,
                Hierarchy = HIE
            };

            BaseForm.TreeViewController.Initialize(treeViewControllerParameter);
        }




    }
}