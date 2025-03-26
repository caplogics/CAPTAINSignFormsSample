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
using DevExpress.XtraRichEdit.Model;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMN0020_CAMSForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        PrivilegeEntity _privileges = null;
        #endregion


        public ADMN0020_CAMSForm(BaseForm baseForm, string mode, string sp_code, PrivilegeEntity privileges, ADMN0020Form Form_Name, string branch_code) //(BaseForm baseForm, string mode, string hierarchy, string year, string SP_Code,  PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Calling_Form = Form_Name;
            Branch_Code = branch_code;
            _privileges = privileges;
            //M_Hierarchy = hierarchy;
            Mode = mode;
            SP_Code = sp_code;

           DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
                Member_Activity = dsAgency.Tables[0].Rows[0]["ACR_MEM_ACTIVTY"].ToString().Trim();

            if (Member_Activity == "Y") this.tabControl1.TabPages[1].Hide();

            switch (Mode)
            {
                case "Edit":
                    this.Text = privileges.PrivilegeName + " - Edit Service/Outcome Details";
                    break;
                case "Add":
                    this.Text = privileges.PrivilegeName + " - Add Service/Outcome Details";
                    break;
                case "Delete":
                    this.Text = privileges.PrivilegeName + " - Delete Service/Outcome Details";
                    break;
            }
            FillSearchFor();
            Fill_SP_Branchs();
            if (SP_Header_rec != null)
            {
                Fill_SP_CA_Details_Grid();
                Fill_CA_Grid();
                Fill_MS_Grid();
                Fill_Results_Grid(string.Empty);
                Fill_Funding_Grid(string.Empty);
                Fill_Issues_Grid(string.Empty);
            }
            else
                BtnAddDetails.Visible = false;
        }

        //Added by Sudheer on 05/17/2021
        public ADMN0020_CAMSForm(BaseForm baseForm, string mode, string sp_code, PrivilegeEntity privileges, ADMN1020Form Form_Name, string branch_code) //(BaseForm baseForm, string mode, string hierarchy, string year, string SP_Code,  PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Calling_Form1 = Form_Name;
            Branch_Code = branch_code;

            //M_Hierarchy = hierarchy;
            Mode = mode;
            SP_Code = sp_code;
            FormName = "ADMN10020";

            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
                Member_Activity = dsAgency.Tables[0].Rows[0]["ACR_MEM_ACTIVTY"].ToString().Trim();

            if (Member_Activity == "Y") this.tabControl1.TabPages[1].Hide();

            switch (Mode)
            {
                case "Edit":
                    this.Text = privileges.PrivilegeName + " - Edit Service/Outcome Details";
                    break;
                case "Add":
                    this.Text = privileges.PrivilegeName + " - Add Service/Outcome Details";
                    break;
                case "Delete":
                    this.Text = privileges.PrivilegeName + " - Delete Service/Outcome Details";
                    break;
            }
            FillSearchFor();
            Fill_SP_Branchs();
            if (SP_Header_rec != null)
            {
                Fill_SP_CA_Details_Grid();
                Fill_CA_Grid();
                Fill_MS_Grid();
                Fill_Results_Grid(string.Empty);
                Fill_Funding_Grid(string.Empty);
                Fill_Issues_Grid(string.Empty);
            }
            else
                BtnAddDetails.Visible = false;
        }



        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string SP_Code { get; set; }

        public ADMN0020Form Calling_Form { get; set; }

        public ADMN1020Form Calling_Form1 { get; set; }

        //public string M_Hierarchy { get; set; }

        ////public string M_HieDesc { get; set; }

        public string FormName { get; set; }

        public string SchSite { get; set; }

        public string SchDate { get; set; }

        public string SchType { get; set; }

        public string Branch_Code { get; set; }

        public string Member_Activity { get; set; }


        public PrivilegeEntity Privileges { get; set; }


        #endregion

        string SPOut_Err_Procedure, SPOut_Err_Number, SPOut_Err_Message, SPOut_Err_Line;

        CASESP0Entity SP_Header_rec = new CASESP0Entity();
        private void Fill_SP_Branchs()
        {
            SP_Header_rec = _model.SPAdminData.Browse_CASESP0(SP_Code, null, null, null, null, null, null, null, null);

            if (SP_Header_rec != null)
            {
                this.Text = SP_Header_rec.Desc.ToString().Trim();

                CmbBranch.Items.Clear();
                List<ListItem> listItem = new List<ListItem>();

                if (!string.IsNullOrEmpty(SP_Header_rec.BpDesc.ToString().Trim()))
                    listItem.Add(new ListItem(SP_Header_rec.BpDesc.ToString().Trim(), "P"));

                if (!string.IsNullOrEmpty(SP_Header_rec.B1Desc.ToString().Trim()))
                    listItem.Add(new ListItem(SP_Header_rec.B1Desc.ToString().Trim(), "1"));

                if (!string.IsNullOrEmpty(SP_Header_rec.B2Desc.ToString().Trim()))
                    listItem.Add(new ListItem(SP_Header_rec.B2Desc.ToString().Trim(), "2"));

                if (!string.IsNullOrEmpty(SP_Header_rec.B3Desc.ToString().Trim()))
                    listItem.Add(new ListItem(SP_Header_rec.B3Desc.ToString().Trim(), "3"));

                if (!string.IsNullOrEmpty(SP_Header_rec.B4Desc.ToString().Trim()))
                    listItem.Add(new ListItem(SP_Header_rec.B4Desc.ToString().Trim(), "4"));

                if (!string.IsNullOrEmpty(SP_Header_rec.B5Desc.ToString().Trim()))
                    listItem.Add(new ListItem(SP_Header_rec.B5Desc.ToString().Trim(), "5"));

                if (listItem.Count > 0)
                {
                    this.CmbBranch.SelectedIndexChanged -= new System.EventHandler(this.CmbBranch_SelectedIndexChanged);
                    CmbBranch.Items.AddRange(listItem.ToArray());

                    if (!string.IsNullOrEmpty(Branch_Code)) // 99999
                        SetComboBoxValue(CmbBranch, Branch_Code);
                    else
                        CmbBranch.SelectedIndex = 0;
                    this.CmbBranch.SelectedIndexChanged += new System.EventHandler(this.CmbBranch_SelectedIndexChanged);
                }
            }
        }

        private void FillSearchFor()
        {
            cmbSearch.Items.Clear();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            if(Member_Activity=="Y")
                listItem.Add(new Captain.Common.Utilities.ListItem("Service/Outcome", "C"));
            else
                listItem.Add(new Captain.Common.Utilities.ListItem("Service", "C"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Results", "R"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Fund", "F"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Legal Issues", "L"));

            cmbSearch.Items.AddRange(listItem.ToArray());
            cmbSearch.SelectedIndex = 0;

        }

        List<CAMASTEntity> CAMASTList;
        private void Fill_CA_Grid()
        {
            CAMASTList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            Fill_Selected_CAs(null);
        }

        public string Get_SP_Existance()
        {
            string SP_exists = "SP Not Exists";
            if (SP_Header_rec != null)
                SP_exists = "SP Exists";

            return SP_exists;
        }
        private void Fill_Selected_CAs(string Search_Text)
        {
            bool CA_Exists = false, Fill_All = true;
            int rowIndex = 0;
            if (!string.IsNullOrEmpty(Search_Text))
                Fill_All = false;
            else
                Search_Text = string.Empty;


            if (CAMASTList.Count > 0)
            {
                CAGrid.Rows.Clear();
                foreach (CAMASTEntity Entity in CAMASTList)
                {
                    rowIndex = 0;
                    CA_Exists = false;

                    foreach (CASESP2Entity dt_CAMS in table_SP_CAMS1)
                    {
                        if (dt_CAMS.Type1 == "CA" && (dt_CAMS.Branch == ((ListItem)CmbBranch.SelectedItem).Value.ToString()) &&
                            (dt_CAMS.CamCd.Trim() == Entity.Code.ToString().Trim()))
                        {
                            CA_Exists = true; Entity.Can_Delete = false; break;
                        }
                    }

                    if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim()) || Fill_All)
                    {
                        if (CA_Exists)
                            rowIndex = CAGrid.Rows.Add(Img_Saved, Entity.Desc.ToString(), Entity.Code.ToString(), "Y", "Y");
                        else
                        {
                            if (Entity.Sel_SW)
                            {
                                rowIndex = CAGrid.Rows.Add(Img_Tick, Entity.Desc.ToString(), Entity.Code.ToString(), "Y", "N");
                                CAGrid.Rows[rowIndex].Cells["CAImage"].Style.ForeColor = Color.LimeGreen;
                            }
                            else
                            {
                                rowIndex = CAGrid.Rows.Add(Img_Blank, Entity.Desc.ToString(), Entity.Code.ToString(), "N", "N");
                                CAGrid.Rows[rowIndex].Cells["CAImage"].Style.ForeColor = Color.Gray;
                            }
                        }

                        if (Entity.Active == false.ToString())
                        {
                            CAGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;// Color.Red;
                            CAGrid.Rows[rowIndex].Cells["CAImage"].Style.ForeColor = Color.Red;
                        } 
                    }
                }
                if (CAGrid.RowCount > 0)
                {
                    CAGrid.Rows[0].Tag = 0;
                    CAGrid.Rows[0].Selected = true;
                }
            }
        }

        List<MSMASTEntity> MSMASTList;
        private void Fill_MS_Grid()
        {
            MSMASTList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
            Fill_Selected_MSs(null);
        }

        private void Fill_Selected_MSs(string Search_Text)
        {
            if (MSMASTList.Count > 0)
            {
                MSGrid.Rows.Clear();
                bool MS_Exists = false, Fill_All = true;
                int rowIndex = 0;
                if (!string.IsNullOrEmpty(Search_Text))
                    Fill_All = false;
                else
                    Search_Text = string.Empty;


                foreach (MSMASTEntity Entity in MSMASTList)
                {
                    rowIndex = 0;
                    MS_Exists = false;

                    foreach (CASESP2Entity dt_CAMS in table_SP_CAMS1)
                    {
                        if (dt_CAMS.Type1.ToString() == "MS" && (dt_CAMS.Branch == ((ListItem)CmbBranch.SelectedItem).Value.ToString()) &&
                            ((dt_CAMS.CamCd.ToString().Trim()) == (Entity.Code.ToString().Trim())))
                        {
                            MS_Exists = true; Entity.Can_Delete = false; break;
                        }
                    }

                    if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim()) || Fill_All)
                    {
                        if (MS_Exists)
                            rowIndex = MSGrid.Rows.Add(Img_Saved, Entity.Desc.ToString(), Entity.Code.ToString(), "Y", "Y");
                        else
                        {
                            if (Entity.Sel_SW)
                            {
                                rowIndex = MSGrid.Rows.Add(Img_Tick, Entity.Desc.ToString(), Entity.Code.ToString(), "Y", "N");
                                MSGrid.Rows[rowIndex].Cells["MSImage"].Style.ForeColor = Color.LimeGreen;
                            }
                            else
                            {
                                rowIndex = MSGrid.Rows.Add(Img_Blank, Entity.Desc.ToString(), Entity.Code.ToString(), "N", "N");
                                MSGrid.Rows[rowIndex].Cells["MSImage"].Style.ForeColor = Color.Gray;
                            }

                        }

                        if (Entity.Active == false.ToString())
                            MSGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;  // Color.Red;

                    }
                }
                if (MSGrid.RowCount > 0)
                {
                    MSGrid.Rows[0].Selected = true;
                    MSGrid.Rows[0].Tag = 0;
                }
            }
        }

        //List<CommonEntity> ResultsList;
        List<SPCommonEntity> ResultsList;
        private void Fill_Results_Grid(string Search_Text)
        {
            try
            {
                ResultsList = _model.SPAdminData.Get_AgyRecs("Results");
                //Ethnicity = filterByHIE(Ethnicity);
                ResultsGrid.Rows.Clear();
                bool Ststus_Exists = false; string Status_List = SP_Header_rec.Status.Trim();
                int Pos = 0, rowIndex = 0, Disp = 4;

                bool Fill_All = true;
                if (!string.IsNullOrEmpty(Search_Text))
                    Fill_All = false;
                else
                    Search_Text = string.Empty;

                foreach (SPCommonEntity Entity in ResultsList)
                {
                    Ststus_Exists = false; Pos = 0;
                    for (int i = 0; Pos < Status_List.Length; i++)
                    {
                        Disp = 4;
                        if (Status_List.Substring(Pos, Status_List.Length - Pos).Length < 4)
                            Disp = Status_List.Substring(Pos, Status_List.Length - Pos).Length;

                        if (Entity.Code == Status_List.Substring(Pos, Disp).Trim())
                        {
                            Ststus_Exists = true; break;
                        }
                        Pos += Disp;
                    }

                    if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim()) || Fill_All)
                    {
                        if (Ststus_Exists)
                            rowIndex = ResultsGrid.Rows.Add(true, Entity.Desc, Entity.Code, "Y");//Img_Tick
                        else
                            rowIndex = ResultsGrid.Rows.Add(false, Entity.Desc, Entity.Code, "N");//Img_Blank

                        if (Entity.Active != "Y")
                            ResultsGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                if (ResultsGrid.RowCount > 0)
                {
                    ResultsGrid.Rows[0].Selected = true;                    
                }
            }
            catch (Exception ex) { }
        }

        //List<CommonEntity> FundingList;
        List<SPCommonEntity> FundingList;
        private void Fill_Funding_Grid(string Search_Text)
        {
            try
            {
                FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");

                FundGrid.Rows.Clear();
                bool Fund_Exists = false, Fill_All = true; ; string Fund_List = SP_Header_rec.Funds.Trim();
                int Pos = 0, Disp = 10, rowIndex = 0;

                if (!string.IsNullOrEmpty(Search_Text))
                    Fill_All = false;
                else
                    Search_Text = string.Empty;

                foreach (SPCommonEntity Entity in FundingList)
                {
                    Fund_Exists = false; Pos = 0;
                    for (int i = 0; Pos < Fund_List.Length; i++)
                    {
                        //if (Entity.Code == SP_Header_rec.Funds.Substring(Pos, 4).Trim())
                        //{
                        //    Fund_Exists = true; break;
                        //}
                        //Pos += 4;
                        Disp = 10;
                        if (Fund_List.Substring(Pos, (Fund_List.Length - Pos)).Length < 10)
                            Disp = Fund_List.Substring(Pos, Fund_List.Length - Pos).Length;

                        if (Entity.Code == Fund_List.Substring(Pos, Disp).Trim())
                        {
                            Fund_Exists = true; break;
                        }
                        Pos += Disp;
                    }

                    if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim()) || Fill_All)
                    {
                        if (Fund_Exists)
                            rowIndex = FundGrid.Rows.Add(true, Entity.Desc, Entity.Code, "Y");//Img_Tick
                        else
                            rowIndex = FundGrid.Rows.Add(false, Entity.Desc, Entity.Code, "N");//Img_Blank

                        if (Entity.Active != "Y")
                            FundGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                if (FundGrid.RowCount > 0)
                {
                    FundGrid.Rows[0].Selected = true;
                }
            }
            catch (Exception ex) { }
        }

        List<SPCommonEntity> IssuesList;
        private void Fill_Issues_Grid(string Search_Text)
        {
            try
            {
                IssuesList = _model.SPAdminData.Get_AgyRecs("Issues");

                LegalGrid.Rows.Clear();
                bool Issue_Exists = false; string Legal_List = SP_Header_rec.Legals.Trim();
                int Pos = 0, rowIndex = 0, Disp = 2;
                bool Fill_All = true;
                if (!string.IsNullOrEmpty(Search_Text))
                    Fill_All = false;
                else
                    Search_Text = string.Empty;

                foreach (SPCommonEntity Entity in IssuesList)
                {
                    Issue_Exists = false; Pos = 0;
                    for (int i = 0; Pos < Legal_List.Length; i++)
                    {
                        Disp = 2;
                        if (Legal_List.Substring(Pos, Legal_List.Length - Pos).Length < 2)
                            Disp = Legal_List.Substring(Pos, Legal_List.Length - Pos).Length;

                        if (Entity.Code == Legal_List.Substring(Pos, Disp).Trim())
                        {
                            Issue_Exists = true; break;
                        }
                        Pos += Disp;
                    }

                    if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim()) || Fill_All)
                    {
                        if (Issue_Exists)
                            rowIndex = LegalGrid.Rows.Add(true, Entity.Desc, Entity.Code, "Y");//Img_Tick
                        else
                            rowIndex = LegalGrid.Rows.Add(false, Entity.Desc, Entity.Code, "N");//Img_Blank

                        if (Entity.Active != "Y")
                            LegalGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                if (LegalGrid.RowCount > 0)
                {
                    LegalGrid.Rows[0].Selected = true;
                }
            }
            catch (Exception ex) { }


        }

        //private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        //{
        //    string HIE = M_Hierarchy;
        //    //if (LookupValues.Exists(u => u.Hierarchy.Equals(HIE)))
        //    //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(HIE)).ToList();
        //    //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")))
        //    //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")).ToList();
        //    //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")))
        //    //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")).ToList();
        //    //else
        //    LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(M_Hierarchy.Substring(0, 4) + "**") || u.ListHierarchy.Contains(M_Hierarchy.Substring(0, 2) + "****") || u.ListHierarchy.Contains("******")).ToList();

        //    return LookupValues;
        //}


        //string Img_Saved = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("save.gif");
        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Tick = "icon-gridtick"; //"Resources/Images/tick.ico";
        string Img_Blank = "radiobutton";//"blank"; //"Resources/Icons/16X16/Blank.JPG";
        string Img_Saved = "icon-save";

        private void Hepl_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN0020_CAMSForm");
        }

        private void ResultsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ResultsGrid.Rows.Count > 0)
            {
                //if (!Results_Header_Clicked)
                //{
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {
                        if (ResultsGrid.CurrentRow.Cells["RslSel_SW"].Value.ToString() == "Y")
                        {
                            ResultsGrid.CurrentRow.Cells["ResultsImage"].Value = false;//Img_Blank;
                            ResultsGrid.CurrentRow.Cells["RslSel_SW"].Value = "N";
                            //foreach (SPCommonEntity Entity in ResultsList)                                  // Don't Delete
                            //{
                            //    if (Entity.Code == ResultsGrid.CurrentRow.Cells["RslCode"].Value.ToString())
                            //        Entity.Sel_WS = false;
                            //}
                        }
                        else
                        {
                            ResultsGrid.CurrentRow.Cells["ResultsImage"].Value = true;//Img_Tick;
                            ResultsGrid.CurrentRow.Cells["RslSel_SW"].Value = "Y";
                            ////foreach (SPCommonEntity Entity in ResultsList)                              // Don't Delete
                            ////{
                            ////    if (Entity.Code == ResultsGrid.CurrentRow.Cells["RslCode"].Value.ToString())
                            ////        Entity.Sel_WS = true;
                            ////}
                        }
                    }
                }
                //}
                //Results_Header_Clicked = false;
            }
        }

        private void FundGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (FundGrid.Rows.Count > 0)
            {
                //if (!Fund_Header_Clicked)
                //{
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {
                        if (FundGrid.CurrentRow.Cells["FundSel_SW"].Value.ToString() == "Y")
                        {
                            FundGrid.CurrentRow.Cells["FundImage"].Value = false;//Img_Blank;
                            FundGrid.CurrentRow.Cells["FundSel_SW"].Value = "N";
                        }
                        else
                        {
                            FundGrid.CurrentRow.Cells["FundImage"].Value = true;//Img_Tick;
                            FundGrid.CurrentRow.Cells["FundSel_SW"].Value = "Y";
                        }
                    }
                }
                //}
                //Fund_Header_Clicked = false;
            }
        }

        private void LegalGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (LegalGrid.Rows.Count > 0)
            {
                //if (!Issues_Header_Clicked)
                //{
                if (e.RowIndex > -1)
                {
                    if (LegalGrid.CurrentRow.Cells["LegalSel_SW"].Value.ToString() == "Y")
                    {
                        LegalGrid.CurrentRow.Cells["LegalImage"].Value = false;//Img_Blank;
                        LegalGrid.CurrentRow.Cells["LegalSel_SW"].Value = "N";
                    }
                    else
                    {
                        LegalGrid.CurrentRow.Cells["LegalImage"].Value = true;//Img_Tick;
                        LegalGrid.CurrentRow.Cells["LegalSel_SW"].Value = "Y";
                    }
                }
                //}
                //Issues_Header_Clicked = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void CAGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CAGrid.Rows.Count > 0)
            {
                if (CAGrid.SelectedRows[0].Selected)
                {
                    // Murali commented two lines there is not used colindex and rowidx on 03/15/2021
                   // int ColIdx = CAGrid.CurrentCell.ColumnIndex;
                  //  int RowIdx = CAGrid.CurrentCell.RowIndex;

                    string strCACode = CAGrid.CurrentRow.Cells["CACode"].Value == null ? string.Empty : CAGrid.CurrentRow.Cells["CACode"].Value.ToString();
                    string strcandel = CAGrid.CurrentRow.Cells["CA_Can_Del"].Value == null ? string.Empty : CAGrid.CurrentRow.Cells["CA_Can_Del"].Value.ToString();
                    if (!CAMS_Header_Clicked && strcandel == "N")
                    {
                        if (e.RowIndex > -1)
                        {
                            if (e.ColumnIndex == 0 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                            {
                                if (CAGrid.CurrentRow.Cells["CA_SW"].Value.ToString() == "Y")
                                {
                                    CAGrid.CurrentRow.Cells["CAImage"].Value = Img_Blank;
                                    CAGrid.CurrentRow.Cells["CAImage"].Style.ForeColor = Color.Gray;
                                    CAGrid.CurrentRow.Cells["CA_SW"].Value = "N";

                                    foreach (CAMASTEntity Entity in CAMASTList)
                                    {
                                        if (Entity.Code == strCACode)
                                            Entity.Sel_SW = false;
                                    }
                                }
                                else
                                {
                                    CAGrid.CurrentRow.Cells["CAImage"].Value = Img_Tick;
                                    CAGrid.CurrentRow.Cells["CAImage"].Style.ForeColor = Color.LimeGreen;

                                    if (CAGrid.CurrentRow.DefaultCellStyle.ForeColor.Name == "Red")
                                    {
                                        CAGrid.CurrentRow.Cells["CAImage"].Style.ForeColor = Color.Red;
                                    }

                                    CAGrid.CurrentRow.Cells["CA_SW"].Value = "Y";

                                    foreach (CAMASTEntity Entity in CAMASTList)
                                    {
                                        if (Entity.Code == strCACode)
                                            Entity.Sel_SW = true;
                                    }
                                }
                            }
                        }
                    }
                    CAMS_Header_Clicked = false;
                }

                //else
            }
        }

        bool CAMS_Header_Clicked = false;
        private void MSGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MSGrid.Rows.Count > 0)
            {
                if (MSGrid.SelectedRows[0].Selected)
                {
                    string strMSCode = MSGrid.CurrentRow.Cells["MSCode"].Value == null ? string.Empty : MSGrid.CurrentRow.Cells["MSCode"].Value.ToString();
                    string strMScandel = MSGrid.CurrentRow.Cells["MS_Can_Del"].Value == null ? string.Empty : MSGrid.CurrentRow.Cells["MS_Can_Del"].Value.ToString();

                    if (!CAMS_Header_Clicked && strMScandel == "N")
                    {
                        if (e.RowIndex > -1)
                        {
                            if (e.ColumnIndex == 0 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                            {
                                if (MSGrid.CurrentRow.Cells["MS_SW"].Value.ToString() == "Y")
                                {
                                    MSGrid.CurrentRow.Cells["MSImage"].Value = Img_Blank;
                                    MSGrid.CurrentRow.Cells["MSImage"].Style.ForeColor = Color.Gray;
                                    MSGrid.CurrentRow.Cells["MS_SW"].Value = "N";

                                    foreach (MSMASTEntity Entity in MSMASTList)
                                    {
                                        if (Entity.Code == strMSCode)
                                            Entity.Sel_SW = false;
                                    }
                                }
                                else
                                {
                                    MSGrid.CurrentRow.Cells["MSImage"].Value = Img_Tick;
                                    MSGrid.CurrentRow.Cells["MSImage"].Style.ForeColor = Color.LimeGreen;
                                    MSGrid.CurrentRow.Cells["MS_SW"].Value = "Y";

                                    foreach (MSMASTEntity Entity in MSMASTList)
                                    {
                                        if (Entity.Code == strMSCode)
                                            Entity.Sel_SW = true;
                                    }
                                }
                            }
                        }
                    }
                    CAMS_Header_Clicked = false;
                }
            }
        }



        List<CASESP2Entity> table_SP_CAMS1;
        private void Fill_SP_CA_Details_Grid()
        {
            table_SP_CAMS1 = _model.SPAdminData.Browse_CASESP2(SP_Code, null, null, null);
        }

        private void CAGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
                CAMS_Header_Clicked = true;
        }

        bool Results_Header_Clicked = false, Fund_Header_Clicked = false, Issues_Header_Clicked = false;
        private void ResultsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (sender == ResultsGrid)
                Results_Header_Clicked = true;

            if (sender == FundGrid)
                Fund_Header_Clicked = true;

            if (sender == LegalGrid)
                Issues_Header_Clicked = true;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string srearch_str = string.Empty;

            if (!string.IsNullOrEmpty(TxtSearch.Text.Trim()))
                srearch_str = (TxtSearch.Text.Trim()).ToUpper();

            if (((ListItem)cmbSearch.SelectedItem).Value.ToString() == "C")
            {
                if (tabControl1.SelectedIndex == 0)
                    Fill_Selected_CAs(srearch_str);
                else
                    Fill_Selected_MSs(srearch_str);
            }
            else if (((ListItem)cmbSearch.SelectedItem).Value.ToString() == "R")
            {
                Fill_Results_Grid(srearch_str);
                
            }
            else if (((ListItem)cmbSearch.SelectedItem).Value.ToString() == "F")
            {
                Fill_Funding_Grid(srearch_str);
                
            }
            else if (((ListItem)cmbSearch.SelectedItem).Value.ToString() == "L")
            {
                Fill_Issues_Grid(srearch_str);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtSearch.Clear();
        }

        string Sql_SP_Result_Message = string.Empty;

        private void ADMN0020_CAMSForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps("ADMN0020", 2, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void BtnAddDetails_Click(object sender, EventArgs e)
        {
            Get_SP0_Switch_Arrays();

            bool CAMS_Added = Get_SP1_CAMS_Arrays();

            int New_SP_Code = 0;
            string SP_Success_Status = string.Empty;

            if (CAMS_Added)
                SP_Header_rec.Validate = "N";

            //if (SP_Header_rec.Allow_Add_Branch == "Y")
            //    SP_Header_rec.Allow_Add_Branch = "1";
            //else
            //    SP_Header_rec.Allow_Add_Branch = "0";

            //_model.SPAdminData.UpdateCASESP0(SP_Header_rec, out New_SP_Code, out SPOut_Err_Procedure, out SPOut_Err_Number, out SPOut_Err_Message, out SPOut_Err_Line);
            //_model.SPAdminData.UpdateCASESP0(SP_Header_rec, out New_SP_Code);

            CAMS_Added = _model.SPAdminData.UpdateCASESP0(SP_Header_rec, out New_SP_Code, out Sql_SP_Result_Message); // 10292012

            if (CAMS_Added)
            {
                //ADMN0020Form ADMN20Form1 = sender  as ADMN0020Form;
                //Added by Sudheer on 05/17/2021
                if(FormName== "ADMN10020")
                {
                    Calling_Form1.Refresh_Edit_Mode();
                }
                else
                    Calling_Form.Refresh_Edit_Mode();
                //ADMN0020Form ADMN20Form = Form  as ADMN0020Form;
                //if (ADMN20Form != null)
                //    ADMN20Form.Refresh_Edit_Mode();
                //this.Close();
                AlertBox.Show("SP Details Saved Successfully");
            }
            else
                AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
            this.Close();
        }


        private void Get_SP0_Switch_Arrays()
        {
            string Tmp_Code = null;

            if (ResultsGrid.RowCount > 0)
            {
                SP_Header_rec.Status = string.Empty;
                foreach (DataGridViewRow dr in ResultsGrid.Rows)
                {
                    if (dr.Cells["RslSel_SW"].Value.ToString() == "Y")
                    {
                        Tmp_Code = string.Empty;
                        Tmp_Code = dr.Cells["RslCode"].Value.ToString();
                        if (Tmp_Code.Length < 4)
                            Tmp_Code = Tmp_Code + "    ".Substring(0, (4 - (Tmp_Code.Length)));

                        SP_Header_rec.Status += Tmp_Code;
                    }
                }
            }

            if (FundGrid.RowCount > 0)
            {
                SP_Header_rec.Funds = string.Empty;
                foreach (DataGridViewRow dr in FundGrid.Rows)
                {
                    if (dr.Cells["FundSel_SW"].Value.ToString() == "Y")
                    {
                        Tmp_Code = string.Empty;
                        Tmp_Code = dr.Cells["FundCode"].Value.ToString();
                        //if (Tmp_Code.Length < 4)
                        //    Tmp_Code = Tmp_Code + "      ".Substring(0, (4 - (Tmp_Code.Length)));

                        if (Tmp_Code.Length < 10)
                            Tmp_Code = Tmp_Code + "          ".Substring(0, (10 - (Tmp_Code.Length)));

                        SP_Header_rec.Funds += Tmp_Code;
                    }
                }
            }

            if (LegalGrid.RowCount > 0)
            {
                SP_Header_rec.Legals = string.Empty;
                foreach (DataGridViewRow dr in LegalGrid.Rows)
                {
                    if (dr.Cells["LegalSel_SW"].Value.ToString() == "Y")
                    {
                        Tmp_Code = string.Empty;
                        Tmp_Code = dr.Cells["LegalCode"].Value.ToString();
                        if (Tmp_Code.Length < 2)
                            Tmp_Code = Tmp_Code + "  ".Substring(0, (2 - (Tmp_Code.Length)));

                        SP_Header_rec.Legals += Tmp_Code;
                    }
                }
            }
        }

        private bool Get_SP1_CAMS_Arrays()
        {
            bool CAMS_Added = false;

            if (CAMASTList.Count > 0)
            {
                CASESP2Entity SP2Entity = new CASESP2Entity();
                SP2Entity.Rec_Type = "I";
                SP2Entity.ServPlan = SP_Code;
                SP2Entity.Branch = (((ListItem)CmbBranch.SelectedItem).Value.ToString());
                SP2Entity.Orig_Grp = 0;
                SP2Entity.Type1 = "CA";
                SP2Entity.SP2_CAMS_Active = "A";
                foreach (CAMASTEntity Entity in CAMASTList)
                {
                    if (Entity.Sel_SW && Entity.Can_Delete)
                    {
                        SP2Entity.SP2_Auto_Post = Entity.AutoPost;
                        if (string.IsNullOrEmpty(SP2Entity.SP2_Auto_Post.Trim()))
                            SP2Entity.SP2_Auto_Post = "N";
                        SP2Entity.CamCd = Entity.Code;
                        SP2Entity.Row = 0;
                        SP2Entity.Curr_Grp = 0;
                        SP2Entity.lstcOperator = BaseForm.UserID;

                        if (_model.SPAdminData.UpdateCASESP2(SP2Entity, string.Empty, out Sql_SP_Result_Message))
                            CAMS_Added = true;

                        //DataSet ds = new DataSet();
                        //ds = _model.SPAdminData.UpdateCASESP2(SP2Entity, string.Empty); // 10292012
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    if (ds.Tables[0].Rows[0]["SP_Result"].ToString() == "false")
                        //    {
                        //        //SPOut_Err_Message = "Record already deleted by another User!!!";
                        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Err_Code"].ToString()))
                        //        {
                        //            SPOut_Err_Message = CommonFunctions.Get_SQlException_Desc(Mode, ds.Tables[0].Rows[0]["Err_Msg"].ToString(), "CASESP1");
                        //            CAMS_Added = false;
                        //        }
                        //    }
                        //    //else
                        //    //    Save_result = true;
                        //}

                    }

                }
            }

            if (MSMASTList.Count > 0)
            {
                CASESP2Entity SP2Entity = new CASESP2Entity();
                SP2Entity.Rec_Type = "I";
                SP2Entity.ServPlan = SP_Code;
                SP2Entity.Branch = (((ListItem)CmbBranch.SelectedItem).Value.ToString());
                SP2Entity.Orig_Grp = 0;
                SP2Entity.Type1 = "MS";
                SP2Entity.SP2_CAMS_Active = "A";
                foreach (MSMASTEntity Entity in MSMASTList)
                {
                    if (Entity.Sel_SW && Entity.Can_Delete)
                    {
                        SP2Entity.SP2_Auto_Post = Entity.AutoPost;
                        if (string.IsNullOrEmpty(SP2Entity.SP2_Auto_Post.Trim()))
                            SP2Entity.SP2_Auto_Post = "N";
                        SP2Entity.CamCd = Entity.Code;
                        SP2Entity.Row = 0;
                        SP2Entity.Curr_Grp = 0;
                        SP2Entity.lstcOperator = BaseForm.UserID;

                        if (_model.SPAdminData.UpdateCASESP2(SP2Entity, string.Empty, out Sql_SP_Result_Message))
                            CAMS_Added = true;

                        //DataSet ds = new DataSet();
                        //ds = _model.SPAdminData.UpdateCASESP2(SP2Entity, string.Empty); // 10292012
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    if (ds.Tables[0].Rows[0]["SP_Result"].ToString() == "false")
                        //    {
                        //        //SPOut_Err_Message = "Record already deleted by another User!!!";
                        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Err_Code"].ToString()))
                        //        {
                        //            SPOut_Err_Message = CommonFunctions.Get_SQlException_Desc(Mode, ds.Tables[0].Rows[0]["Err_Msg"].ToString(), "CASESP1");
                        //            CAMS_Added = false;
                        //        }
                        //    }
                        //    //else
                        //    //    Save_result = true;
                        //}

                    }

                }
            }
            return CAMS_Added;
        }

        private void CmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtSearch.Clear();
            foreach (CAMASTEntity Entity in CAMASTList)
            {
                Entity.Sel_SW = false; Entity.Can_Delete = true;
            }
            foreach (MSMASTEntity Entity in MSMASTList)
            {
                Entity.Sel_SW = false; Entity.Can_Delete = true;
            }

            Fill_Selected_CAs(null);
            Fill_Selected_MSs(null);
        }


        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
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
    }
}