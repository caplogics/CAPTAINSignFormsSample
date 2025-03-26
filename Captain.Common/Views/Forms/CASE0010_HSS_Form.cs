#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;

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
//using Gizmox.WebGUI.Common.Resources;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Drawing;
using Captain.Common.Interfaces;
using DevExpress.Web.Internal.XmlProcessor;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0010_HSS_Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;

        #endregion

        public CASE0010_HSS_Form(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Privileges = privileges;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            this.Text = privileges.Program + " - Edit";
            Fill_Funding_Grid();
        }

        public CASE0010_HSS_Form(string Scr_Code, string sel_mode, bool Selected_All, List<MSMASTEntity> sel_MS_list, List<CAMASTEntity> sel_CA_list)
        {
            InitializeComponent();

            //BaseForm = baseForm;
            //Privileges = privileges;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Sel_Mode = sel_mode; ScreenCode = Scr_Code;
            Sel_CAList = sel_CA_list;
            Sel_MSList = sel_MS_list;

            this.Text = (sel_mode == "CA" ? "Service Selection" : "Outcome Selection");
           // this.Text = Scr_Code + (sel_mode == "CA" ? " - CA Selection" : "MS Selection");
          //  label2.Text = (sel_mode == "CA" ? " CA Selection" : "MS Selection");

            if(Scr_Code=="RNGB0004")
            {
                lblSearch.Text = (sel_mode == "CA" ? "Search Services" : "Search Outcomes");
                //lblSearch.Text= (sel_mode == "CA" ? "Search CA" : "Search MS");
            }

            // CAMS_Selection_Panel.Location = new System.Drawing.Point(1, 1);
            this.Size = new Size(575, 491);//(575,530);//(460, 418);
            CAMS_Selection_Panel.Visible = true;
            if (Selected_All)
                Rb_All.Checked = true;
            else
            {
                CAMS_Grid.Enabled = Rb_Sel.Checked = true;
                pnlSearch.Visible = true;
            }

            if (Sel_Mode == "CA")
                Fill_CA_Grid(Sel_CAList);
            else
                Fill_MS_Grid(Sel_MSList);
        }

        public CASE0010_HSS_Form(BaseForm baseForm, string Scr_Code, string sel_mode, bool Selected_All, List<MSMASTEntity> sel_MS_list, List<CAMASTEntity> sel_CA_list,string Agency,string Dept,string Program,string UserID)
        {
            InitializeComponent();

            BaseForm = baseForm;
            //Privileges = privileges;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Sel_Mode = sel_mode; ScreenCode = Scr_Code;
            Sel_CAList = sel_CA_list;
            Sel_MSList = sel_MS_list;
            strAgency = Agency;strDept = Dept;strProg = Program;
            strUserId = UserID;

            this.Text = (sel_mode == "CA" ? "Service Selection" : "Outcome Selection");
            // this.Text = Scr_Code + (sel_mode == "CA" ? " - CA Selection" : "MS Selection");
            //  label2.Text = (sel_mode == "CA" ? " CA Selection" : "MS Selection");

            if (Scr_Code == "RNGB0004")
            {
                lblSearch.Text = (sel_mode == "CA" ? "Search Services" : "Search Outcomes");
                //lblSearch.Text= (sel_mode == "CA" ? "Search CA" : "Search MS");
            }

            // CAMS_Selection_Panel.Location = new System.Drawing.Point(1, 1);
            //this.Size = new Size(575, 491);//(575,530);//(460, 418);
            this.Size = new Size(690, 491);
            this.dataGridViewTextBoxColumn2.Width = 428;
            this.Active.Width = 85;//130;
            CAMS_Selection_Panel.Visible = true;

            if (Sel_Mode == "CA")
                Fill_CA_Grid(Sel_CAList);
            else
                Fill_MS_Grid(Sel_MSList);

            if (Selected_All)
            {
                Rb_All.Checked = true;
                if (Rb_All.Checked)
                {
                    pnlSearch.Visible = false;
                    txtCAMSSearch.Clear();
                    foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                    {
                        dr.Cells["CAMS_Sel"].ReadOnly = true;
                        dr.Cells["CAMS_Sel"].Value = true;//false;
                    }
                }
            }
            else
            {
                Rb_Sel.Checked = true;
                //pnlSearch.Visible = true;
                if (Rb_Sel.Checked)
                {
                    txtCAMSSearch.Clear();
                    if (ScreenCode == "RNGB0004") pnlSearch.Visible = true;
                    foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                    {
                        dr.Cells["CAMS_Sel"].ReadOnly = false;
                        dr.Cells["CAMS_Sel"].Value = false;
                    }

                    if (Sel_Mode == "CA")
                    {
                        CAMS_Grid.Rows.Clear();
                        Fill_CA_Grid(Sel_CAList);
                        tooltip.SetToolTip(btnCAMSSearch, "Select Service(s)");
                    }
                    else if (Sel_Mode == "MS")
                    {
                        CAMS_Grid.Rows.Clear();
                        Fill_MS_Grid(Sel_MSList);
                        tooltip.SetToolTip(btnCAMSSearch, "Select Outcome(s)");
                    }
                }
            }
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string ScreenCode { get; set; }

        public string Sel_Mode { get; set; }

        public string strAgency { get; set; }
        public string strDept { get; set; }
        public string strProg { get; set; }

        public string strUserId { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        public List<CAMASTEntity> Sel_CAList { get; set; }
        public List<MSMASTEntity> Sel_MSList { get; set; }

        #endregion

        List<CommonEntity> Funding_List = new List<CommonEntity>();
        private void Fill_Funding_Grid()
        {
            Funding_List = _model.lookupDataAccess.GetFundingSources();
            
            foreach (CommonEntity Entity in Funding_List)
                GD_Fund.Rows.Add(Entity.Code, Entity.Desc);
        }


        List<CAMASTEntity> CA_Mast_List = new List<CAMASTEntity>();
        private void Fill_CA_Grid(List<CAMASTEntity> sel_list)
        {
            bool CA_Selected = false;
            CAMS_Grid.Rows.Clear();

            if (ScreenCode == "RNGB0004")
            {
                string ACR_SERV_Hies = string.Empty;
                if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
                {
                    if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                        ACR_SERV_Hies = "S";
                }

                if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
                {
                    //if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                        CA_Mast_List = _model.SPAdminData.CAP_SEROUTCOMES(strAgency, strDept, strProg, "CA",strUserId);
                    //else
                    //    CA_Mast_List = _model.SPAdminData.CAP_SEROUTCOMES(strAgency, string.Empty, string.Empty, "CA");
                }
                else
                    CA_Mast_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
            }
            else
                CA_Mast_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
            List<CAMASTEntity> CAList = new List<CAMASTEntity>();
            if (txtCAMSSearch.Text.Trim() != string.Empty)
                CAList = CA_Mast_List.FindAll(x => x.Desc.ToUpper().Contains(txtCAMSSearch.Text.ToUpper()));
            else CAList = CA_Mast_List;

            //if (CAList.Count > 0)
            //    CAList = CAList.OrderBy(u => u.Active).ThenBy(u => u.Desc.Trim()).ToList();
            int row_Index = 0;

            #region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

            //List<CAMASTEntity> _checkedlst = CAList.Where(row => row.Sel_SW = (sel_list.Exists(u => u.Code.Equals(row.Code))) ? true : false).ToList();
            //List<CAMASTEntity> _uncheckedlst = CAList.Where(row => row.Sel_SW == false).ToList();

            //foreach (CAMASTEntity Entity in _checkedlst)
            //{
            //    CA_Selected = false;
            //    foreach (CAMASTEntity ent in sel_list)
            //    {
            //        if (ent.Code.Trim() == Entity.Code.Trim())
            //        {
            //            CA_Selected = true; break;
            //        }
            //    }
            //    if (Entity.Active.Trim() == "True") Entity.Active = "A"; else if (Entity.Active.Trim() == "False") Entity.Active = "I";

            //    row_Index = CAMS_Grid.Rows.Add((CA_Selected ? true : false), Entity.Code.Trim(), Entity.Desc.Trim(), Entity.Active.Trim() == "A" ? "Active" : "Inactive");
            //    if (Entity.Active.Trim() == "I")
            //        CAMS_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            //}

            //foreach (CAMASTEntity Entity in _uncheckedlst)
            //{
            //    CA_Selected = false;
            //    foreach (CAMASTEntity ent in sel_list)
            //    {
            //        if (ent.Code.Trim() == Entity.Code.Trim())
            //        {
            //            CA_Selected = true; break;
            //        }
            //    }
            //    if (Entity.Active.Trim() == "True") Entity.Active = "A"; else if (Entity.Active.Trim() == "False") Entity.Active = "I";

            //    row_Index = CAMS_Grid.Rows.Add((CA_Selected ? true : false), Entity.Code.Trim(), Entity.Desc.Trim(), Entity.Active.Trim() == "A" ? "Active" : "Inactive");
            //    if (Entity.Active.Trim() == "I")
            //        CAMS_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            //}

            #endregion

            if (sel_list != null && sel_list.Count > 0)
            {
                //FundingList.ForEach(item => item.Active = (Entity_List.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
                CAList.ForEach(item => item.Sel_SW = (sel_list.Exists(u => u.Code.Equals(item.Code))) ? true : false);
            }

            if (CAList.Count > 0)
                CAList = CAList.OrderByDescending(u=>u.Sel_SW).ThenBy(u => u.Active).ThenBy(u => u.Desc.Trim()).ToList();

            foreach (CAMASTEntity Entity in CAList)
            {
                CA_Selected = false;
                foreach (CAMASTEntity ent in sel_list)
                {
                    if (ent.Code.Trim() == Entity.Code.Trim())
                    {
                        CA_Selected = true; break;
                    }
                }
                if (Entity.Active.Trim() == "True") Entity.Active = "A"; else if (Entity.Active.Trim() == "False") Entity.Active = "I";

                row_Index =CAMS_Grid.Rows.Add((CA_Selected ? true : false),Entity.Code.Trim(), Entity.Desc.Trim(),Entity.Active.Trim()=="A"?"Active":"Inactive");
                if (Entity.Active.Trim() == "I")
                    CAMS_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }

        }

        List<MSMASTEntity> MS_Mast_List = new List<MSMASTEntity>();
        private void Fill_MS_Grid(List<MSMASTEntity> sel_list)
        {
            bool MS_Selected = false;
            CAMS_Grid.Rows.Clear();
            if (ScreenCode == "RNGB0004")
            {
                string ACR_SERV_Hies = string.Empty;
                if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
                {
                    if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                        ACR_SERV_Hies = "S";
                }

                if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
                {
                    //if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                        MS_Mast_List = _model.SPAdminData.CAP_SEROUTCOMES_MS(strAgency, strDept, strProg, "MS", strUserId);
                    //else
                    //    MS_Mast_List = _model.SPAdminData.CAP_SEROUTCOMES_MS(strAgency, string.Empty, string.Empty, "MS", strUserId);
                }
                else
                    MS_Mast_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
            }
            else
                MS_Mast_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
            List<MSMASTEntity> MSList = new List<MSMASTEntity>();
            if (txtCAMSSearch.Text.Trim() != string.Empty)
                MSList = MS_Mast_List.FindAll(x => x.Desc.ToUpper().Contains(txtCAMSSearch.Text.ToUpper()));
            else MSList = MS_Mast_List;

            if (MSList.Count > 0)
                MSList = MSList.OrderBy(u => u.Active).ThenBy(u => u.Desc.Trim()).ToList();
            int row_Index = 0;

            #region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

            //List<MSMASTEntity> _checkedlst = MSList.Where(row => row.Sel_SW = (sel_list.Exists(u => u.Code.Equals(row.Code))) ? true : false).ToList();
            //List<MSMASTEntity> _uncheckedlst = MSList.Where(row => row.Sel_SW == false).ToList();

            //foreach (MSMASTEntity Entity in _checkedlst)
            //{
            //    MS_Selected = false;
            //    foreach (MSMASTEntity ent in sel_list)
            //    {
            //        if (ent.Code.Trim() == Entity.Code.Trim())
            //        {
            //            MS_Selected = true; break;
            //        }
            //    }

            //    if (Entity.Active.Trim() == "True") Entity.Active = "A"; else if (Entity.Active.Trim() == "False") Entity.Active = "I";

            //    row_Index = CAMS_Grid.Rows.Add((MS_Selected ? true : false), Entity.Code.Trim(), Entity.Desc.Trim(), Entity.Active.Trim() == "A" ? "Active" : "Inactive");
            //    if (Entity.Active.Trim() == "I")
            //        CAMS_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            //}

            //foreach (MSMASTEntity Entity in _uncheckedlst)
            //{
            //    MS_Selected = false;
            //    foreach (MSMASTEntity ent in sel_list)
            //    {
            //        if (ent.Code.Trim() == Entity.Code.Trim())
            //        {
            //            MS_Selected = true; break;
            //        }
            //    }

            //    if (Entity.Active.Trim() == "True") Entity.Active = "A"; else if (Entity.Active.Trim() == "False") Entity.Active = "I";

            //    row_Index = CAMS_Grid.Rows.Add((MS_Selected ? true : false), Entity.Code.Trim(), Entity.Desc.Trim(), Entity.Active.Trim() == "A" ? "Active" : "Inactive");
            //    if (Entity.Active.Trim() == "I")
            //        CAMS_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            //}

            #endregion

            if (sel_list != null && sel_list.Count > 0)
            {
                //FundingList.ForEach(item => item.Active = (Entity_List.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
                MSList.ForEach(item => item.Sel_SW = (sel_list.Exists(u => u.Code.Equals(item.Code))) ? true : false);
            }

            if (MSList.Count > 0)
                MSList = MSList.OrderByDescending(u => u.Sel_SW).ThenBy(u => u.Active).ThenBy(u => u.Desc.Trim()).ToList();

            foreach (MSMASTEntity Entity in MSList)
            {
                MS_Selected = false;
                foreach (MSMASTEntity ent in sel_list)
                {
                    if (ent.Code.Trim() == Entity.Code.Trim())
                    {
                        MS_Selected = true; break;
                    }
                }

                if (Entity.Active.Trim() == "True") Entity.Active = "A"; else if (Entity.Active.Trim() == "False") Entity.Active = "I";

                row_Index =CAMS_Grid.Rows.Add((MS_Selected ? true : false), Entity.Code.Trim(), Entity.Desc.Trim(), Entity.Active.Trim() == "A" ? "Active" : "Inactive");
                if(Entity.Active.Trim()=="I")
                    CAMS_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
        }

        public List<MSMASTEntity> Get_Sel_MS_List()
        {
            List<MSMASTEntity> Sel_List = new List<MSMASTEntity>();

            if (Rb_Sel.Checked)
            {
                if (ScreenCode == "RNGB0004")
                {
                    if (Sel_Mode == "MS" && Sel_MSList.Count > 0)
                        Sel_List = Sel_MSList;
                    else
                    {
                        foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                        {
                            if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                            {
                                foreach (MSMASTEntity Entity in MS_Mast_List)
                                {
                                    if (Entity.Code.Trim() == dr.Cells["CAMS_Code"].Value.ToString())
                                    {
                                        Sel_List.Add(new MSMASTEntity(Entity));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                    {
                        if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                        {
                            foreach (MSMASTEntity Entity in MS_Mast_List)
                            {
                                if (Entity.Code.Trim() == dr.Cells["CAMS_Code"].Value.ToString())
                                {
                                    Sel_List.Add(new MSMASTEntity(Entity));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return Sel_List;
        }

        public List<CAMASTEntity> Get_Sel_CA_List()
        {
            List<CAMASTEntity> Sel_List = new List<CAMASTEntity>();

            if (Rb_Sel.Checked)
            {
                if (ScreenCode == "RNGB0004")
                {
                    if(Sel_Mode=="CA" && Sel_CAList.Count>0)
                        Sel_List = Sel_CAList;
                    else
                    {
                        foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                        {
                            if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                            {
                                foreach (CAMASTEntity Entity in CA_Mast_List)
                                {
                                    if (Entity.Code.Trim() == dr.Cells["CAMS_Code"].Value.ToString())
                                    {
                                        Sel_List.Add(new CAMASTEntity(Entity));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                    {
                        if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                        {
                            foreach (CAMASTEntity Entity in CA_Mast_List)
                            {
                                if (Entity.Code.Trim() == dr.Cells["CAMS_Code"].Value.ToString())
                                {
                                    Sel_List.Add(new CAMASTEntity(Entity));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return Sel_List;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool Can_Close = false;
            if (Rb_Sel.Checked)
            {
                if(Sel_Mode=="CA") { if (Sel_CAList.Count > 0) Can_Close = true; }
                if (Sel_Mode == "MS") { if (Sel_MSList.Count > 0) Can_Close = true; }

                if (!Can_Close)
                {
                    foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                    {
                        if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                        {
                            Can_Close = true;
                            break;
                        }
                    }
                }
            }
            else
                Can_Close = true;

            if (Rb_Sel.Checked && !Can_Close)
                AlertBox.Show("Please select at least One " + (Sel_Mode == "CA" ? "Service" : "Outcome"),MessageBoxIcon.Warning);

            if (Can_Close)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void Rb_All_Click(object sender, EventArgs e)
        {
            if (Rb_All.Checked)
            {
                pnlSearch.Visible = false;
                txtCAMSSearch.Clear();
                Sel_CAList.Clear();
                Sel_MSList.Clear();
                //**CAMS_Grid.Enabled = false;
                if (Sel_Mode == "CA")
                {
                    CAMS_Grid.Rows.Clear();
                    Fill_CA_Grid(Sel_CAList);
                    tooltip.SetToolTip(btnCAMSSearch, "Select Service(s)");
                }
                else if (Sel_Mode == "MS")
                {
                    CAMS_Grid.Rows.Clear();
                    Fill_MS_Grid(Sel_MSList);
                    tooltip.SetToolTip(btnCAMSSearch, "Select Outcome(s)");
                }

                foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                {
                    dr.Cells["CAMS_Sel"].ReadOnly = true;
                    dr.Cells["CAMS_Sel"].Value = true;//false;
                }
            }
        }

    private void Rb_Sel_Click(object sender, EventArgs e)
    {
        if (Rb_Sel.Checked)
        {
            txtCAMSSearch.Clear();
            //**CAMS_Grid.Enabled = true;
            if (ScreenCode == "RNGB0004") pnlSearch.Visible = true;
            foreach (DataGridViewRow dr in CAMS_Grid.Rows)
            {
                dr.Cells["CAMS_Sel"].ReadOnly = false;
                dr.Cells["CAMS_Sel"].Value = false;
            }

            if (Sel_Mode == "CA")
            {
                CAMS_Grid.Rows.Clear();
                Fill_CA_Grid(Sel_CAList);
                tooltip.SetToolTip(btnCAMSSearch, "Select Service(s)");
            }
            else if (Sel_Mode == "MS")
            {
                CAMS_Grid.Rows.Clear();
                Fill_MS_Grid(Sel_MSList);
                tooltip.SetToolTip(btnCAMSSearch, "Select Outcome(s)");
            }
    
        }    
    }
        ToolTip tooltip = new ToolTip();
        public List<CAMASTEntity> Find_SP_CAMS_Details { get; set; }
        public List<MSMASTEntity> Find_MS_Details { get; set; }
        private void btnCAMSSearch_Click(object sender, EventArgs e)
        {

            if (Sel_Mode == "CA")
            {
                Fill_CA_Grid(Sel_CAList);
                tooltip.SetToolTip(btnCAMSSearch, "Select Service(s)");
            }
            else if (Sel_Mode == "MS")
            {
                Fill_MS_Grid(Sel_MSList);
                tooltip.SetToolTip(btnCAMSSearch, "Select Outcome(s)");
            }

            if (txtCAMSSearch.Text.Trim() != string.Empty)
            {
                

                //if (Sel_Mode == "CA")
                //{
                //    Find_SP_CAMS_Details = CA_Mast_List.FindAll(x => x.Desc.ToUpper().Contains(txtCAMSSearch.Text.ToUpper()));

                    //    if (Find_SP_CAMS_Details.Count > 0)
                    //    {
                    //        bool IsFind = false;
                    //        foreach (CAMASTEntity Entity in Find_SP_CAMS_Details)
                    //        {
                    //            foreach (DataGridViewRow item in CAMS_Grid.Rows)
                    //            {
                    //                //if (Find_SP_CAMS_Details[0].CamCd.Trim() == Convert.ToString(item.Cells["SP2_CAMS_Code"].Value))//if (Convert.ToString(item.Cells["SP2_CAMS_Code"].Value).Contains(txtCAMSSearch.Text.Trim()))
                    //                if (Entity.Code.Trim() == Convert.ToString(item.Cells["CAMS_Code"].Value))
                    //                {
                    //                    //gvwCustomer.Update();
                    //                    int i = item.Index;

                    //                    // int intscroolindex = gvwCustomer.FirstDisplayedScrollingRowIndex;
                    //                    int CurrentPage = (i / CAMS_Grid.ItemsPerPage);
                    //                    CurrentPage++;
                    //                    CAMS_Grid.CurrentPage = CurrentPage;
                    //                    CAMS_Grid.CurrentCell = CAMS_Grid.Rows[i].Cells[1];
                    //                    CAMS_Grid.FirstDisplayedScrollingRowIndex = i;
                    //                    CAMS_Grid.Rows[i].Selected = true;
                    //                    //intFindNext = i; boolNxt = true; FindPrev = i;
                    //                    IsFind = true;
                    //                    break;

                    //                }
                    //            }

                    //            if (IsFind) break;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    Find_MS_Details = MS_Mast_List.FindAll(x => x.Desc.ToUpper().Contains(txtCAMSSearch.Text.ToUpper()));

                    //    if (Find_MS_Details.Count > 0)
                    //    {
                    //        bool IsFind = false;
                    //        foreach (MSMASTEntity Entity in Find_MS_Details)
                    //        {
                    //            foreach (DataGridViewRow item in CAMS_Grid.Rows)
                    //            {
                    //                //if (Find_SP_CAMS_Details[0].CamCd.Trim() == Convert.ToString(item.Cells["SP2_CAMS_Code"].Value))//if (Convert.ToString(item.Cells["SP2_CAMS_Code"].Value).Contains(txtCAMSSearch.Text.Trim()))
                    //                if (Entity.Code.Trim() == Convert.ToString(item.Cells["CAMS_Code"].Value))
                    //                {
                    //                    //gvwCustomer.Update();
                    //                    int i = item.Index;

                    //                    // int intscroolindex = gvwCustomer.FirstDisplayedScrollingRowIndex;
                    //                    int CurrentPage = (i / CAMS_Grid.ItemsPerPage);
                    //                    CurrentPage++;
                    //                    CAMS_Grid.CurrentPage = CurrentPage;
                    //                    CAMS_Grid.CurrentCell = CAMS_Grid.Rows[i].Cells[1];
                    //                    CAMS_Grid.FirstDisplayedScrollingRowIndex = i;
                    //                    CAMS_Grid.Rows[i].Selected = true;
                    //                    //intFindNext = i; boolNxt = true; FindPrev = i;
                    //                    IsFind = true;
                    //                    break;

                    //                }
                    //            }

                    //            if (IsFind) break;
                    //        }
                    //    }

                    //}




            }
        }

        private void CAMS_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ScreenCode == "RNGB0004")
            {
                if (CAMS_Grid.Rows.Count > 0 && e.RowIndex != -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (CAMS_Grid.CurrentRow.Cells["CAMS_Sel"].Value.ToString() == "True")
                        {
                            if (Sel_Mode == "CA")
                            {
                                if (Sel_CAList.Count > 0)
                                {
                                    CAMASTEntity CAEntity = Sel_CAList.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                    if (CAEntity == null)
                                    {
                                        CAMASTEntity SelEntity = CA_Mast_List.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                        if (SelEntity != null)
                                            Sel_CAList.Add(SelEntity);
                                    }
                                }
                                else
                                {
                                    CAMASTEntity SelEntity = CA_Mast_List.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                    if (SelEntity != null)
                                        Sel_CAList.Add(SelEntity);
                                }
                            }
                            else if (Sel_Mode == "MS")
                            {
                                if (Sel_MSList.Count > 0)
                                {
                                    MSMASTEntity CAEntity = Sel_MSList.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                    if (CAEntity == null)
                                    {
                                        MSMASTEntity SelEntity = MS_Mast_List.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                        if (SelEntity != null)
                                            Sel_MSList.Add(SelEntity);
                                    }
                                }
                                else
                                {
                                    MSMASTEntity SelEntity = MS_Mast_List.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                    if (SelEntity != null)
                                        Sel_MSList.Add(SelEntity);
                                }
                            }


                        }
                        else
                        {
                            if (Sel_Mode == "CA")
                            {
                                if (Sel_CAList.Count > 0)
                                {
                                    CAMASTEntity CAEntity = Sel_CAList.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                    if (CAEntity != null)
                                        Sel_CAList.Remove(CAEntity);
                                }
                            }
                            else if (Sel_Mode == "MS")
                            {
                                if (Sel_MSList.Count > 0)
                                {
                                    MSMASTEntity CAEntity = Sel_MSList.Find(u => u.Code.Equals(CAMS_Grid.CurrentRow.Cells["CAMS_Code"].Value.ToString()));
                                    if (CAEntity != null)
                                        Sel_MSList.Remove(CAEntity);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnenroll_Click(object sender, EventArgs e)
        {

        }

        private void CAMS_Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void GD_Fund_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}