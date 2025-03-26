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
using Captain.Common.Views.UserControls;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RNG_Goalservices : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public RNG_Goalservices(BaseForm baseform, string mode, string Reffdate, string RefTdate, string gridType, string Grpcd, string tblcd, string services, string referDesc,string progSw,List<RNGGoalHEntity> rnggoalsHie, PrivilegeEntity priviliages,string pprsw)
        {
            InitializeComponent();
            BaseForm = baseform;

            Privileage = priviliages;
            _model = new CaptainModel();
            Mode = mode;
            GridType = gridType;
            refFdate = Reffdate;
            refTdate = RefTdate;
            ReferDesc = referDesc;
            groupCd = Grpcd;
            tablecd = tblcd;
            Services = services;
            PPR_SW = pprsw;
            ProgramSw = progSw;
            RNGGoalPrograms = rnggoalsHie;



            if (Services == "MS")
            {
                this.Text = "Outcome Indicators - Outcome Association";//"Outcome Indicators - Goal Association";
                this.Goals.HeaderText = "Detail Outcomes";
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
            }
            else
            {
                this.Text = "Service Measures - Service Association";

                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                this.Goals.HeaderText = "Detail Services";//"Critical Activity";
            }
            lblRefDt.Text = refFdate.ToString() + "  " + ReferDesc.ToString();
            FillComboGrps();

            if (progSw == "Y") this.Budget.ReadOnly = true;
            else this.Budget.ReadOnly = false;

            //FillTables();
            //fillGvGoals();
            Mode = "Edit";
        }

        public RNG_Goalservices(BaseForm baseform, string mode, string Reffdate, string RefTdate, string gridType, string Grpcd, string tblcd, string services, string referDesc, string progSw, PrivilegeEntity priviliages,string pprsw)
        {
            InitializeComponent();
            BaseForm = baseform;

            Privileage = priviliages;
            _model = new CaptainModel();
            Mode = mode;
            GridType = gridType;
            refFdate = Reffdate;
            refTdate = RefTdate;
            ReferDesc = referDesc;
            groupCd = Grpcd;
            tablecd = tblcd;
            Services = services;
            PPR_SW = pprsw;
            ProgramSw = progSw;
            

            if (Services == "MS")
            {
                this.Text = "Outcome Indicators - Goal Association";
                this.Goals.HeaderText = "Detail Outcomes";
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
            }
            else
            {
                this.Text = "Service Measures - Service Association";

                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                this.Goals.HeaderText = "Detail Services";//"Critical Activity";
            }
            lblRefDt.Text = refFdate.ToString() + "  " + ReferDesc.ToString();
            FillComboGrps();

            if (progSw == "Y") this.Budget.ReadOnly = true;
            else this.Budget.ReadOnly = false;

            //FillTables();
            //fillGvGoals();
            Mode = "Edit";
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileage { get; set; }

        public string Mode { get; set; }

        public string GridType { get; set; }

        public string refFdate { get; set; }

        public string refTdate { get; set; }

        public string ReferDesc { get; set; }

        public string groupCd { get; set; }

        public string tablecd { get; set; }

        public string Services { get; set; }

        public bool IsSaveValid { get; set; }

        public string ProgramSw { get; set; }
        public string PPR_SW { get; set; }

        public List<RNGGoalHEntity> RNGGoalPrograms { get; set; }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Performance");
        }
        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        //string Img_Tick = "Resources/Images/tick.ico";
        //string Img_Blank = "Resources/Icons/16X16/Blank.JPG";
        string Img_Tick = "icon-gridtick";//"icon-done?color=#01a601";
        string Img_Blank = "blank";

        private void FillComboGrps()
        {
            string UserAgency = string.Empty;
            //if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            //{
            //    HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
            //    if (SelHie != null)
            //        UserAgency = "**";
            //}

            if (Services == "MS")
            {
                CmbGrp.Items.Clear();
                List<RepListItem> listItem = new List<RepListItem>();
                List<RCsb14GroupEntity> grpcombo;
                //int Sel_grp_index = 0, temp_loopCount = 0;
                grpcombo = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID,string.Empty);
                if (grpcombo.Count > 0)
                {
                    grpcombo = grpcombo.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == refTdate);
                }
                if (grpcombo.Count > 0)
                {
                    foreach (RCsb14GroupEntity drgrp in grpcombo)
                    {
                        if (drgrp.Code.ToString() == refFdate && drgrp.Agency.ToString() == refTdate && !string.IsNullOrWhiteSpace(drgrp.GrpCode.ToString()) && string.IsNullOrWhiteSpace(drgrp.TblCode.ToString()))
                        {
                            string Tmp_Desc = string.Empty;//String.Format("{0,-10}", drgrp.GrpCode.ToString().Trim() + String.Format("{0,15}", " - " + drgrp.GrpDesc.ToString().Trim()));
                            Tmp_Desc = String.Format("{0,-10}", drgrp.GrpCode.Trim()) + " - " + drgrp.GrpDesc.ToString().Trim();
                            CmbGrp.Items.Add(new RepListItem(Tmp_Desc, drgrp.GrpCode.ToString().Trim()));
                        }

                        //////if (LookupDataAccess.Getdate(drgrp.RefFDate.ToString()) == refFdate && LookupDataAccess.Getdate(drgrp.RefTDate.ToString()) == refTdate && drgrp.GrpCode.ToString().Trim() == groupCd.Trim() && string.IsNullOrWhiteSpace(drgrp.TblCode.ToString()))
                        //////{
                        //////    //i = int.Parse(groupCd.ToString().Trim());
                        //////    //Sel_grp_index = temp_loopCount;
                        //////    SetComboBoxValue(CmbGrp, drgrp.GrpCode.ToString().Trim());
                        //////}
                        //temp_loopCount++;
                    }
                    if (CmbGrp.Items.Count > 0)
                        SetComboBoxValue(CmbGrp, groupCd.Trim());
                    else
                        CmbTbl.Enabled = false;
                    //CmbGrp.SelectedIndex = Sel_grp_index;
                }
            }
            else if (Services == "CA")
            {
                CmbGrp.Items.Clear();
                List<RepListItem> listItem = new List<RepListItem>();
                List<SRCsb14GroupEntity> grpcombo;
                //int Sel_grp_index = 0, temp_loopCount = 0;
                grpcombo = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);
                if (grpcombo.Count > 0)
                {
                    grpcombo = grpcombo.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == refTdate);
                }
                if (grpcombo.Count > 0)
                {
                    foreach (SRCsb14GroupEntity drgrp in grpcombo)
                    {
                        if (drgrp.Code.ToString() == refFdate && drgrp.Agency.ToString() == refTdate && !string.IsNullOrWhiteSpace(drgrp.GrpCode.ToString()) && string.IsNullOrWhiteSpace(drgrp.TblCode.ToString()))
                        {
                            string Tmp_Desc = string.Empty;//String.Format("{0,-10}", drgrp.GrpCode.ToString().Trim() + String.Format("{0,15}", " - " + drgrp.GrpDesc.ToString().Trim()));
                            Tmp_Desc = String.Format("{0,-10}", drgrp.GrpCode.Trim()) + " - " + drgrp.GrpDesc.ToString().Trim();
                            CmbGrp.Items.Add(new RepListItem(Tmp_Desc, drgrp.GrpCode.ToString().Trim()));
                        }

                        //////if (LookupDataAccess.Getdate(drgrp.RefFDate.ToString()) == refFdate && LookupDataAccess.Getdate(drgrp.RefTDate.ToString()) == refTdate && drgrp.GrpCode.ToString().Trim() == groupCd.Trim() && string.IsNullOrWhiteSpace(drgrp.TblCode.ToString()))
                        //////{
                        //////    //i = int.Parse(groupCd.ToString().Trim());
                        //////    //Sel_grp_index = temp_loopCount;
                        //////    SetComboBoxValue(CmbGrp, drgrp.GrpCode.ToString().Trim());
                        //////}
                        //temp_loopCount++;
                    }
                    if (CmbGrp.Items.Count > 0)
                        SetComboBoxValue(CmbGrp, groupCd.Trim());
                    else
                        CmbTbl.Enabled = false;
                    //CmbGrp.SelectedIndex = Sel_grp_index;
                }
            }

        }
        string Group = null; string Table = null;
        private void FillTables()
        {
            string UserAgency = string.Empty;
            //if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            //{
            //    HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
            //    if (SelHie != null)
            //        UserAgency = "**";
            //}

            if (Services == "MS")
            {
                this.CmbTbl.SelectedIndexChanged -= new System.EventHandler(this.CmbTbl_SelectedIndexChanged);
                CmbTbl.Items.Clear();
                Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
                List<RepListItem> listItem = new List<RepListItem>();
                List<RCsb14GroupEntity> tblCombo;
                tblCombo = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);
                if (tblCombo.Count > 0)
                {
                    tblCombo = tblCombo.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == refTdate);
                }
                tblCombo = tblCombo.FindAll(u => u.Code.Equals(refFdate) && u.Agency.Equals(refTdate) && u.GrpCode == Group);
                tblCombo = tblCombo.OrderBy(u => u.TblCode).ToList();
                
                if (tblCombo.Count > 0)
                {
                    foreach (RCsb14GroupEntity drtbl in tblCombo)
                    {
                        //if ((LookupDataAccess.Getdate(drtbl.RefFDate.ToString()) == refFdate) && (LookupDataAccess.Getdate(drtbl.RefTDate.ToString()) == refTdate) && drtbl.GrpCode.ToString() == Group && !string.IsNullOrWhiteSpace(drtbl.TblCode.ToString()))
                        if (!string.IsNullOrWhiteSpace(drtbl.TblCode.ToString()))
                        {
                            string Tmp_Desc = string.Empty;
                            string Temp_Desc = String.Format("{0,-10} ", drtbl.TblCode.ToString().Trim() + " - " + drtbl.GrpDesc.ToString().Trim());
                            CmbTbl.Items.Add(new RepListItem(Temp_Desc, drtbl.TblCode.ToString()));

                        }
                    }
                    this.CmbTbl.SelectedIndexChanged += new System.EventHandler(this.CmbTbl_SelectedIndexChanged);
                    if (CmbTbl.Items.Count == 0)
                    {
                        GvGoals.Rows.Clear(); CmbTbl.Enabled = false; btnSave.Visible = false;
                        MessageBox.Show("This Domain is not having any Groups", "Goals/Service Association");
                    }
                    else
                    {
                        if (groupCd.Trim() != Group)
                            CmbTbl.SelectedIndex = 0;
                        else
                            SetComboBoxValue(CmbTbl, tablecd.ToString());
                        btnSave.Visible = true; CmbTbl.Enabled = true;
                        //btnSave.Text = "&Change";
                    }


                }
            }
            else if (Services == "CA")
            {
                this.CmbTbl.SelectedIndexChanged -= new System.EventHandler(this.CmbTbl_SelectedIndexChanged);
                CmbTbl.Items.Clear();
                Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
                List<RepListItem> listItem = new List<RepListItem>();
                List<SRCsb14GroupEntity> tblCombo;
                tblCombo = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);
                if (tblCombo.Count > 0)
                {
                    tblCombo = tblCombo.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == refTdate);
                }
                tblCombo = tblCombo.FindAll(u => u.Code.Equals(refFdate) && u.Agency.Equals(refTdate) && u.GrpCode == Group);
                tblCombo = tblCombo.OrderBy(u => u.TblCode).ToList();
               
                if (tblCombo.Count > 0)
                {
                    foreach (SRCsb14GroupEntity drtbl in tblCombo)
                    {
                        //if ((LookupDataAccess.Getdate(drtbl.RefFDate.ToString()) == refFdate) && (LookupDataAccess.Getdate(drtbl.RefTDate.ToString()) == refTdate) && drtbl.GrpCode.ToString() == Group && !string.IsNullOrWhiteSpace(drtbl.TblCode.ToString()))
                        if (!string.IsNullOrWhiteSpace(drtbl.TblCode.ToString()))
                        {
                            string Tmp_Desc = string.Empty;
                            string Temp_Desc = String.Format("{0,-10} ", drtbl.TblCode.ToString().Trim() + " - " + drtbl.GrpDesc.ToString().Trim());
                            CmbTbl.Items.Add(new RepListItem(Temp_Desc, drtbl.TblCode.ToString()));

                        }
                    }
                    this.CmbTbl.SelectedIndexChanged += new System.EventHandler(this.CmbTbl_SelectedIndexChanged);
                    if (CmbTbl.Items.Count == 0)
                    {
                        GvGoals.Rows.Clear(); CmbTbl.Enabled = false; btnSave.Visible = false;
                        MessageBox.Show("This Group is not having any Tables", "Goals/Service Association");
                    }
                    else
                    {
                        if (groupCd.Trim() != Group)
                            CmbTbl.SelectedIndex = 0;
                        else
                            SetComboBoxValue(CmbTbl, tablecd.ToString());
                        btnSave.Visible = true; CmbTbl.Enabled = true;
                        //btnSave.Text = "&Change";
                    }


                }
            }
        }

        private void CmbGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Group = groupCd;
            //Group = ((RepListItem)CmbGrp.SelectedValue).Value.ToString();
            FillTables(); // Yeswanth
            //CmbTbl.SelectedIndex = 0;

        }

        //List<SPCommonEntity> GoalsList;
        List<MSMASTEntity> MSList;
        List<CAMASTEntity> CAList;
        List<RCsb14GroupEntity> GrpEntity;
        List<RNGGAEntity> GoalEntity;
        List<SRCsb14GroupEntity> SGrpEntity;
        List<RNGSRGAEntity> SGoalEntity;


        string AgyGoal_code = null;
        private void fillGvGoals1()
        {
            Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
            Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
            bool Can_Continue = true, Fill_All = true;
            string Goal = null; string goaldesc = null;
            int i = 0; string Search_Text = string.Empty;
            string temp_code = null;

            if (Services == "MS")
            {

                MSList = MSList.OrderByDescending(u => u.Active).ThenBy(u=>u.Desc.Trim()).ToList();
                this.Goals.HeaderText = "Detail Outcomes";
                this.Budget.HeaderText = "Target#";

                GvGoals.Rows.Clear();
                Fill_All = true;
                if (!string.IsNullOrEmpty(TxtSearch.Text.Trim())) { Search_Text = TxtSearch.Text.Trim(); Fill_All = false; }
                else
                    Search_Text = string.Empty;
                if (PPR_SW == "Y")
                {
                    this.RNG_SP.Visible = true; RNG_SP.ShowInVisibilityMenu = true;
                    this.Goals.Width = 260;
                    this.RNG_SP.Width = 220;

                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_ServicePlanFromSerOutcome(string.Empty, "MS", BaseForm.BaseAdminAgency, "CAMS");
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        

                        if (dt.Rows.Count > 0)
                        {
                            int rIndex = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (((dr["SEROUT_DESC"].ToString().ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All))
                                {
                                    rIndex = GvGoals.Rows.Add(Img_Blank, dr["SP_DESC"].ToString(), dr["SEROUT_DESC"].ToString(), dr["SP2_CAMS_CODE"].ToString(), "N", "", dr["SP2_SERVICEPLAN"].ToString().Trim());
                                    if (dr["SP_ACTIVE"].ToString().Trim() == "I")
                                        GvGoals.Rows[rIndex].DefaultCellStyle.ForeColor = Color.Red;
                                }
                            }

                            foreach (RNGGAEntity GAEntity in GoalEntity)
                            {
                                int rowIndex = 0;string ForColr = string.Empty;
                                foreach (DataGridViewRow row in GvGoals.Rows)
                                {
                                    if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim()==GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                                    {
                                        if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                                            ForColr = "R";
                                        //GvGoals.Rows.RemoveAt(rowIndex);
                                        //GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.SP_desc.Trim(), GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                                        row.Cells["Check"].Value = Img_Tick;
                                        row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                                        row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                                        row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                                        row.Cells["Img_Code"].Value = "Y";
                                        if(!string.IsNullOrEmpty(GAEntity.Budget))
                                            row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                                        else
                                            row.Cells["Budget"].Value = "0";
                                        row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                                        if (ForColr == "R")
                                        {
                                            GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                            ForColr = string.Empty;
                                        }
                                        break;
                                    }
                                    rowIndex++;
                                }
                            }

                        }
                    }
                }
                else
                {
                    this.RNG_SP.Visible = false; RNG_SP.ShowInVisibilityMenu = false;
                    this.Goals.Width = 480;

                    List<MSMASTEntity> SelMSList= new List<MSMASTEntity>();
                    if (!string.IsNullOrEmpty(Search_Text.Trim()))
                    {
                        SelMSList = MSList.FindAll(u => u.Desc.Trim().ToUpper().Contains(Search_Text.Trim().ToUpper()));
                    }
                    else
                        SelMSList = MSList;

                    SelMSList=SelMSList.OrderByDescending(u=>u.Active).ThenBy(u=>u.Desc).ToList();

                    int rowInd = 0;
                    foreach (MSMASTEntity Entity in SelMSList)
                    {
                        //if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All)
                        //{
                            rowInd = GvGoals.Rows.Add(Img_Blank, string.Empty, Entity.Desc, Entity.Code, "N", "", "0");

                            if (Entity.Active == "False")
                                GvGoals.Rows[rowInd].DefaultCellStyle.ForeColor = Color.Red;
                        //}
                    }

                    foreach (RNGGAEntity GAEntity in GoalEntity)
                    {
                        int rowIndex = 0;
                        string ForColr = string.Empty;
                        foreach (DataGridViewRow row in GvGoals.Rows)
                        {
                            if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim() == GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                            {
                                if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                                    ForColr = "R";
                                //GvGoals.Rows.RemoveAt(rowIndex);
                                //GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.SP_desc.Trim(), GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                                row.Cells["Check"].Value = Img_Tick;
                                row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                                row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                                row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                                row.Cells["Img_Code"].Value = "Y";
                                if (!string.IsNullOrEmpty(GAEntity.Budget))
                                    row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                                else
                                    row.Cells["Budget"].Value = "0";
                                row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                                if (ForColr == "R")
                                {
                                    GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    ForColr = string.Empty;
                                }
                                break;
                            }
                            rowIndex++;
                        }
                    }
                }


                //this.GvGoals.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellValueChanged);
                //foreach (RNGGAEntity GAEntity in GoalEntity)
                //{
                //    int rowIndex = 0;
                //    foreach (DataGridViewRow row in GvGoals.Rows)
                //    {
                //        if (row.Cells[3].Value.ToString() == GAEntity.GoalCode.Trim().ToString() && GAEntity.Sel_Switch == true)
                //        {
                //            if (RNGGoalPrograms.Count > 0)
                //            {
                //                int Targets = 0;
                //                List<RNGGoalHEntity> SelTargets = new List<RNGGoalHEntity>();
                //                SelTargets = RNGGoalPrograms.FindAll(u => u.RNGGAH_GOAL_CODE.Trim() == GAEntity.GoalCode.Trim());
                //                SelTargets = SelTargets.FindAll(u => u.RNGGAH_TARGET.Trim() != "");
                //                if (SelTargets.Count > 0)
                //                {
                //                    Targets = SelTargets.Sum(u => int.Parse(u.RNGGAH_TARGET.Trim()));

                //                    GAEntity.Budget = Targets.ToString();
                //                }
                //            }

                //            GvGoals.Rows.RemoveAt(rowIndex);
                //            GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget);
                //            //GvGoals.Rows.Add(Img_Tick, GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget);
                //            //GAEntity.Mode = GAEntity.Budget;
                //            //GvGoals.Rows[rowIndex].Cells["Check"].Value = Img_Tick;
                //            //GvGoals.Rows[rowIndex].Cells["Img_Code"].Value = "Y";
                //            //GvGoals.Rows[rowIndex].Cells["Budget"].Value = GAEntity.Mode;

                //            break;
                //        }
                //        rowIndex++;
                //    }
                //    this.GvGoals.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellValueChanged);
                //}

                //if (GvGoals.Rows.Count > 0)
                //{
                //    GvGoals.CurrentCell = GvGoals.Rows[0].Cells[0];
                //}

                textBox1.Text = GvGoals.Rows.Count.ToString();
            }

            else if (Services == "CA")
            {
                //SGrpEntity = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, Group, Table, null);
                //SGoalEntity = _model.SPAdminData.Browse_RNGSRGA(refFdate, refTdate, Group, Table, null);


                CAList = CAList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc.Trim()).ToList();
                this.Goals.HeaderText = "Detail Services";//"Critical Activity";
                this.Budget.HeaderText = "Target#";
                GvGoals.Rows.Clear();
                if (PPR_SW == "Y")
                {
                    this.RNG_SP.Visible = true; RNG_SP.ShowInVisibilityMenu = true;
                    this.Goals.Width = 260;
                    this.RNG_SP.Width = 220;
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_ServicePlanFromSerOutcome(string.Empty, "CA", BaseForm.BaseAdminAgency, "CAMS");
                    Fill_All = true;
                    if (!string.IsNullOrEmpty(TxtSearch.Text.Trim())) { Search_Text = TxtSearch.Text.Trim(); Fill_All = false; }
                    else
                        Search_Text = string.Empty;
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            int rIndex = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (((dr["SEROUT_DESC"].ToString().ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All))
                                {
                                    rIndex = GvGoals.Rows.Add(Img_Blank, dr["SP_DESC"].ToString(), dr["SEROUT_DESC"].ToString(), dr["SP2_CAMS_CODE"].ToString(), "N", "", dr["SP2_SERVICEPLAN"].ToString().Trim());
                                    if (dr["SP_ACTIVE"].ToString().Trim() == "I")
                                        GvGoals.Rows[rIndex].DefaultCellStyle.ForeColor = Color.Red;
                                }
                            }

                            foreach (RNGSRGAEntity GAEntity in SGoalEntity)
                            {
                                int rowIndex = 0; string ForColr = string.Empty;
                                foreach (DataGridViewRow row in GvGoals.Rows)
                                {
                                    if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim()==GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                                    {
                                        if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                                            ForColr = "R";
                                        //GvGoals.Rows.RemoveAt(rowIndex);
                                        //GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.SP_desc.Trim(), GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                                        row.Cells["Check"].Value = Img_Tick;
                                        row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                                        row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                                        row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                                        row.Cells["Img_Code"].Value = "Y";
                                        if (!string.IsNullOrEmpty(GAEntity.Budget))
                                            row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                                        else
                                            row.Cells["Budget"].Value = "0";
                                        row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                                        if (ForColr == "R")
                                        {
                                            GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                            ForColr = string.Empty;
                                        }
                                            
                                        break;
                                    }
                                    rowIndex++;
                                }
                            }

                        }
                    }
                }
                else
                {
                    this.RNG_SP.Visible = false; RNG_SP.ShowInVisibilityMenu = false;
                    this.Goals.Width = 480;
                    Fill_All = true;
                    if (!string.IsNullOrEmpty(TxtSearch.Text.Trim())) { Search_Text = TxtSearch.Text.Trim(); Fill_All = false; }
                    else
                        Search_Text = string.Empty;

                    int rowInd = 0;
                    foreach (CAMASTEntity Entity in CAList)
                    {
                        if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All)
                        {
                            rowInd= GvGoals.Rows.Add(Img_Blank, string.Empty, Entity.Desc, Entity.Code, "N", "", "0");

                            if (Entity.Active == "False")
                                GvGoals.Rows[rowInd].DefaultCellStyle.ForeColor = Color.Red;
                        }

                    }

                    foreach (RNGSRGAEntity GAEntity in SGoalEntity)
                    {
                        int rowIndex = 0; string ForColr = string.Empty;
                        foreach (DataGridViewRow row in GvGoals.Rows)
                        {
                            if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim() == GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                            {
                                if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                                    ForColr = "R";
                                //GvGoals.Rows.RemoveAt(rowIndex);
                                //GvGoals.Rows.Insert(rowIndex, Img_Tick, string.Empty, GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                                row.Cells["Check"].Value = Img_Tick;
                                row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                                row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                                row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                                row.Cells["Img_Code"].Value = "Y";
                                if (!string.IsNullOrEmpty(GAEntity.Budget))
                                    row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                                else
                                    row.Cells["Budget"].Value = "0";
                                row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                                if (ForColr == "R")
                                {
                                    GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    ForColr = string.Empty;
                                }
                                break;
                            }
                            rowIndex++;
                        }
                    }

                }

                
            }

        }

        private void fillGvGoals()
        {
            Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
            Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
            bool Can_Continue = true, Fill_All = true;
            string Goal = null; string goaldesc = null;
            int i = 0; string Search_Text = string.Empty;
            string temp_code = null;
            string Agency=string.Empty;
            if (refTdate == "**") Agency = BaseForm.BaseAdminAgency;else Agency=refTdate.ToString();

            if (Services == "MS")
            {
                MSList = MSList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc.Trim()).ToList();
                this.Goals.HeaderText = "Detail Outcomes";
                this.Budget.HeaderText = "Target#";

                GvGoals.Rows.Clear();
                Fill_All = true;
                if (!string.IsNullOrEmpty(TxtSearch.Text.Trim())) { Search_Text = TxtSearch.Text.Trim(); Fill_All = false; }
                else
                    Search_Text = string.Empty;
                if (PPR_SW == "Y")
                {
                    this.RNG_SP.Visible = true; RNG_SP.ShowInVisibilityMenu = true;
                    this.Goals.Width = 260;
                    this.RNG_SP.Width = 220;

                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_ServicePlanFromSerOutcome(string.Empty, "MS", Agency, "CAMS");
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];


                        if (dt.Rows.Count > 0)
                        {
                            int rIndex = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (((dr["SEROUT_DESC"].ToString().ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All))
                                {
                                    rIndex = GvGoals.Rows.Add(Img_Blank, dr["SP_DESC"].ToString(), dr["SEROUT_DESC"].ToString(), dr["SP2_CAMS_CODE"].ToString(), "N", "", dr["SP2_SERVICEPLAN"].ToString().Trim());
                                    if (dr["SP_ACTIVE"].ToString().Trim() == "I")
                                        GvGoals.Rows[rIndex].DefaultCellStyle.ForeColor = Color.Red;
                                }
                            }

                            foreach (RNGGAEntity GAEntity in GoalEntity)
                            {
                                int rowIndex = 0; string ForColr = string.Empty;
                                foreach (DataGridViewRow row in GvGoals.Rows)
                                {
                                    if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim() == GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                                    {
                                        if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                                            ForColr = "R";
                                        //GvGoals.Rows.RemoveAt(rowIndex);
                                        //GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.SP_desc.Trim(), GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                                        row.Cells["Check"].Value = Img_Tick;
                                        row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                                        row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                                        row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                                        row.Cells["Img_Code"].Value = "Y";
                                        if (!string.IsNullOrEmpty(GAEntity.Budget))
                                            row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                                        else
                                            row.Cells["Budget"].Value = "0";
                                        row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                                        if (ForColr == "R")
                                        {
                                            GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                            ForColr = string.Empty;
                                        }
                                        break;
                                    }
                                    rowIndex++;
                                }
                            }

                        }
                    }
                }
                else
                {
                    this.RNG_SP.Visible = false; RNG_SP.ShowInVisibilityMenu = false;
                    this.Goals.Width = 480;

                    List<MSMASTEntity> SelMSList = new List<MSMASTEntity>();
                    if (!string.IsNullOrEmpty(Search_Text.Trim()))
                    {
                        SelMSList = MSList.FindAll(u => u.Desc.Trim().ToUpper().Contains(Search_Text.Trim().ToUpper()));
                    }
                    else
                        SelMSList = MSList;

                    SelMSList = SelMSList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();

                    int rowInd = 0;
                    foreach (MSMASTEntity Entity in SelMSList)
                    {
                        //if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All)
                        //{

                        if(GoalEntity.Count>0)
                        {
                            RNGGAEntity GAEntity=GoalEntity.Find(u=>u.GoalCode.Trim()==Entity.Code.Trim() && u.Sel_Switch==true);
                            if(GAEntity!=null)
                                rowInd = GvGoals.Rows.Add(Img_Tick, string.Empty, Entity.Desc, Entity.Code, "N", GAEntity.Budget, GAEntity.SerSP);
                            else
                                rowInd = GvGoals.Rows.Add(Img_Blank, string.Empty, Entity.Desc, Entity.Code, "N", "", "0");
                        }
                        else
                            rowInd = GvGoals.Rows.Add(Img_Blank, string.Empty, Entity.Desc, Entity.Code, "N", "", "0");

                        if (Entity.Active == "False")
                            GvGoals.Rows[rowInd].DefaultCellStyle.ForeColor = Color.Red;
                        //}
                    }

                    //foreach (RNGGAEntity GAEntity in GoalEntity)
                    //{
                    //    int rowIndex = 0;
                    //    string ForColr = string.Empty;
                    //    foreach (DataGridViewRow row in GvGoals.Rows)
                    //    {
                    //        if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim() == GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                    //        {
                    //            if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                    //                ForColr = "R";
                    //            //GvGoals.Rows.RemoveAt(rowIndex);
                    //            //GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.SP_desc.Trim(), GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                    //            row.Cells["Check"].Value = Img_Tick;
                    //            row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                    //            row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                    //            row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                    //            row.Cells["Img_Code"].Value = "Y";
                    //            if (!string.IsNullOrEmpty(GAEntity.Budget))
                    //                row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                    //            else
                    //                row.Cells["Budget"].Value = "0";
                    //            row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                    //            if (ForColr == "R")
                    //            {
                    //                GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //                ForColr = string.Empty;
                    //            }
                    //            break;
                    //        }
                    //        rowIndex++;
                    //    }
                    //}
                }


                //this.GvGoals.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellValueChanged);
                //foreach (RNGGAEntity GAEntity in GoalEntity)
                //{
                //    int rowIndex = 0;
                //    foreach (DataGridViewRow row in GvGoals.Rows)
                //    {
                //        if (row.Cells[3].Value.ToString() == GAEntity.GoalCode.Trim().ToString() && GAEntity.Sel_Switch == true)
                //        {
                //            if (RNGGoalPrograms.Count > 0)
                //            {
                //                int Targets = 0;
                //                List<RNGGoalHEntity> SelTargets = new List<RNGGoalHEntity>();
                //                SelTargets = RNGGoalPrograms.FindAll(u => u.RNGGAH_GOAL_CODE.Trim() == GAEntity.GoalCode.Trim());
                //                SelTargets = SelTargets.FindAll(u => u.RNGGAH_TARGET.Trim() != "");
                //                if (SelTargets.Count > 0)
                //                {
                //                    Targets = SelTargets.Sum(u => int.Parse(u.RNGGAH_TARGET.Trim()));

                //                    GAEntity.Budget = Targets.ToString();
                //                }
                //            }

                //            GvGoals.Rows.RemoveAt(rowIndex);
                //            GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget);
                //            //GvGoals.Rows.Add(Img_Tick, GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget);
                //            //GAEntity.Mode = GAEntity.Budget;
                //            //GvGoals.Rows[rowIndex].Cells["Check"].Value = Img_Tick;
                //            //GvGoals.Rows[rowIndex].Cells["Img_Code"].Value = "Y";
                //            //GvGoals.Rows[rowIndex].Cells["Budget"].Value = GAEntity.Mode;

                //            break;
                //        }
                //        rowIndex++;
                //    }
                //    this.GvGoals.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellValueChanged);
                //}

                textBox1.Text = GvGoals.Rows.Count.ToString();
            }

            else if (Services == "CA")
            {
                //SGrpEntity = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, Group, Table, null);
                //SGoalEntity = _model.SPAdminData.Browse_RNGSRGA(refFdate, refTdate, Group, Table, null);


                CAList = CAList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc.Trim()).ToList();
                this.Goals.HeaderText = "Detail Services";//"Critical Activity";
                this.Budget.HeaderText = "Target#";
                GvGoals.Rows.Clear();
                if (PPR_SW == "Y")
                {
                    this.RNG_SP.Visible = true; RNG_SP.ShowInVisibilityMenu = true;
                    this.Goals.Width = 260;
                    this.RNG_SP.Width = 220;
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_ServicePlanFromSerOutcome(string.Empty, "CA", Agency, "CAMS");
                    Fill_All = true;
                    if (!string.IsNullOrEmpty(TxtSearch.Text.Trim())) { Search_Text = TxtSearch.Text.Trim(); Fill_All = false; }
                    else
                        Search_Text = string.Empty;
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            int rIndex = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (((dr["SEROUT_DESC"].ToString().ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All))
                                {
                                    rIndex = GvGoals.Rows.Add(Img_Blank, dr["SP_DESC"].ToString(), dr["SEROUT_DESC"].ToString(), dr["SP2_CAMS_CODE"].ToString(), "N", "", dr["SP2_SERVICEPLAN"].ToString().Trim());
                                    if (dr["SP_ACTIVE"].ToString().Trim() == "I")
                                        GvGoals.Rows[rIndex].DefaultCellStyle.ForeColor = Color.Red;
                                }
                            }

                            foreach (RNGSRGAEntity GAEntity in SGoalEntity)
                            {
                                int rowIndex = 0; string ForColr = string.Empty;
                                foreach (DataGridViewRow row in GvGoals.Rows)
                                {
                                    if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim() == GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                                    {
                                        if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                                            ForColr = "R";
                                        //GvGoals.Rows.RemoveAt(rowIndex);
                                        //GvGoals.Rows.Insert(rowIndex, Img_Tick, GAEntity.SP_desc.Trim(), GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                                        row.Cells["Check"].Value = Img_Tick;
                                        row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                                        row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                                        row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                                        row.Cells["Img_Code"].Value = "Y";
                                        if (!string.IsNullOrEmpty(GAEntity.Budget))
                                            row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                                        else
                                            row.Cells["Budget"].Value = "0";
                                        row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                                        if (ForColr == "R")
                                        {
                                            GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                            ForColr = string.Empty;
                                        }

                                        break;
                                    }
                                    rowIndex++;
                                }
                            }

                        }
                    }
                }
                else
                {
                    this.RNG_SP.Visible = false; RNG_SP.ShowInVisibilityMenu = false;
                    this.Goals.Width = 480;
                    Fill_All = true;
                    if (!string.IsNullOrEmpty(TxtSearch.Text.Trim())) { Search_Text = TxtSearch.Text.Trim(); Fill_All = false; }
                    else
                        Search_Text = string.Empty;

                    int rowInd = 0;
                    foreach (CAMASTEntity Entity in CAList)
                    {
                        if (((Entity.Desc).ToUpper()).Contains(Search_Text.Trim().ToUpper()) || Fill_All)
                        {
                            rowInd = GvGoals.Rows.Add(Img_Blank, string.Empty, Entity.Desc, Entity.Code, "N", "", "0");

                            if (Entity.Active == "False")
                                GvGoals.Rows[rowInd].DefaultCellStyle.ForeColor = Color.Red;
                        }

                    }

                    foreach (RNGSRGAEntity GAEntity in SGoalEntity)
                    {
                        int rowIndex = 0; string ForColr = string.Empty;
                        foreach (DataGridViewRow row in GvGoals.Rows)
                        {
                            if (row.Cells[3].Value.ToString().Trim() == GAEntity.GoalCode.Trim() && row.Cells[6].Value.ToString().Trim() == GAEntity.SerSP.Trim() && GAEntity.Sel_Switch == true)
                            {
                                if (GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor == Color.Red)
                                    ForColr = "R";
                                //GvGoals.Rows.RemoveAt(rowIndex);
                                //GvGoals.Rows.Insert(rowIndex, Img_Tick, string.Empty, GAEntity.Desc, GAEntity.GoalCode, "Y", GAEntity.Budget, GAEntity.SerSP);
                                row.Cells["Check"].Value = Img_Tick;
                                row.Cells["RNG_SP"].Value = GAEntity.SP_desc.Trim();
                                row.Cells["Goals"].Value = GAEntity.Desc.Trim();
                                row.Cells["Agy_Code"].Value = GAEntity.GoalCode.Trim();
                                row.Cells["Img_Code"].Value = "Y";
                                if (!string.IsNullOrEmpty(GAEntity.Budget))
                                    row.Cells["Budget"].Value = GAEntity.Budget.Trim();
                                else
                                    row.Cells["Budget"].Value = "0";
                                row.Cells["SP_Code"].Value = GAEntity.SerSP.Trim();
                                if (ForColr == "R")
                                {
                                    GvGoals.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    ForColr = string.Empty;
                                }
                                break;
                            }
                            rowIndex++;
                        }
                    }

                }


            }

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        List<RNGGoalHEntity> SelProgs = new List<RNGGoalHEntity>();
        private void CmbTbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Table = tablecd.ToString();
            Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
            Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
            if (Services == "MS")
            {
                GoalEntity = _model.SPAdminData.Browse_RNGGA(refFdate, refTdate, Group, Table, null);
                SelProgs   = _model.SPAdminData.Browse_RNGGAH(refFdate, refTdate, groupCd, Table, string.Empty);

                List< RCsb14GroupEntity> TblCntrl = _model.SPAdminData.Browse_RNGGrp(refFdate, refTdate, groupCd, Table, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if(TblCntrl.Count>0)
                {
                    if (TblCntrl[0].Prog_Switch=="Y") this.Budget.ReadOnly = true; else this.Budget.ReadOnly = false;
                }


                //if (SelProgs.Count > 0) this.Budget.ReadOnly = true; else this.Budget.ReadOnly = false;

                if (PPR_SW == "Y")
                {
                    GoalEntity = GoalEntity.FindAll(u => u.SerSP != "0");
                }
                else
                {
                    GoalEntity = GoalEntity.FindAll(u => u.SerSP == "0");
                }

                foreach (RNGGAEntity item in GoalEntity)
                {
                    item.Sel_Switch = true;
                }
            }
            else if (Services == "CA")
            {               
                SGoalEntity = _model.SPAdminData.Browse_RNGSRGA(refFdate, refTdate, Group, Table, null);
                if (PPR_SW == "Y")
                {
                    SGoalEntity = SGoalEntity.FindAll(u => u.SerSP != "0");
                }
                else
                {
                    SGoalEntity = SGoalEntity.FindAll(u => u.SerSP == "0");
                }
                foreach (RNGSRGAEntity item in SGoalEntity)
                {
                    item.Sel_Switch = true;
                }

            }
           
            fillGvGoals();
        }

        int BudgetValue = 0;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "&Save")
            {
                if (GvGoals.Rows.Count > 0)
                {
                    try
                    {
                        Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
                        Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
                        if (Services == "MS")
                        {
                            Captain.DatabaseLayer.SPAdminDB.DeleteRNGGA(refFdate, refTdate, Group, Table, string.Empty,string.Empty);

                            CaptainModel model = new CaptainModel();
                            RNGGAEntity GoalsEntity = new RNGGAEntity();

                            GoalsEntity.Code = refFdate.ToString();
                            GoalsEntity.Agency = refTdate.ToString();
                            GoalsEntity.TblCode = Table.ToString();
                            GoalsEntity.GrpCode = Group.ToString();
                            GoalsEntity.Mode = "Add";
                            //GoalsEntity.CntCode = ((RepListItem)CmbRsltHead.SelectedItem).Value.ToString();
                            GoalsEntity.LSTCOperator = BaseForm.UserID;
                            BudgetValue = 0;
                            //foreach (DataGridViewRow dr in GvGoals.Rows)
                            //{
                            //    if (dr.Cells["Img_Code"].Value.ToString() == "Y")
                            //    {
                            //        GoalsEntity.GoalCode = dr.Cells["Agy_Code"].Value.ToString();
                            //        GoalsEntity.Desc = dr.Cells["Goals"].Value.ToString();
                            //        if (dr.Cells["Budget"].Value == null) GoalsEntity.Budget = "0";
                            //        else GoalsEntity.Budget = dr.Cells["Budget"].Value.ToString();
                            //        if (!string.IsNullOrEmpty(GoalsEntity.Budget.Trim()))
                            //            BudgetValue += int.Parse(GoalsEntity.Budget.ToString());
                            //        _model.SPAdminData.InsertUpdateRNGGA(GoalsEntity);
                            //    }

                            //}

                            foreach (RNGGAEntity item in GoalEntity)
                            {
                                if (item.Sel_Switch.ToString().ToUpper() == "TRUE")
                                {
                                    GoalsEntity.GoalCode = item.GoalCode.ToString();
                                    GoalsEntity.Desc = item.Desc.ToString();
                                    if (PPR_SW == "Y") { GoalsEntity.SerSP = item.SerSP; }
                                    if (string.IsNullOrEmpty(item.Budget)) GoalsEntity.Budget = "0";
                                    else GoalsEntity.Budget = item.Budget.ToString();
                                    if (!string.IsNullOrEmpty(GoalsEntity.Budget.Trim()))
                                        BudgetValue += int.Parse(GoalsEntity.Budget.ToString());
                                    _model.SPAdminData.InsertUpdateRNGGA(GoalsEntity);
                                }
                            }

                            RCsb14GroupEntity TblEntity = new RCsb14GroupEntity();

                            TblEntity.Code = refFdate;
                            TblEntity.Agency = refTdate;
                            TblEntity.GrpCode = groupCd;
                            //TblEntity.GrpDesc = txtDesc.Text;
                            TblEntity.TblCode = Table.ToString();
                            TblEntity.ExAchev = BudgetValue.ToString();
                            //TblEntity.CntIndic = ((ListItem)cmbCntInd.SelectedItem).Value.ToString();
                            //if (chkbCalcCosts.Checked)
                            //    TblEntity.CalCost = "1";
                            //else
                            //    TblEntity.CalCost = "0";
                            //TblEntity.UseSer = chkbUseSer.Checked ? "1" : "0";
                            //TblEntity.Duplicate = chkbDupl.Checked ? "1" : "0";
                            //TblEntity.Disable = chkbDisabled.Checked ? "1" : "0";
                            //TblEntity.AFrom = txtAgeFrm.Text;
                            //TblEntity.Ato = txtAgeTo.Text;
                            TblEntity.LSTCOperator = BaseForm.UserID;
                            TblEntity.AddOperator = BaseForm.UserID;
                            TblEntity.Mode = "Goal";
                            _model.SPAdminData.InsertUpdateRNGGrp(TblEntity);
                            AlertBox.Show("Goal Code Association Saved Successfullly");
                        }
                        else if (Services == "CA")
                        {
                            Captain.DatabaseLayer.SPAdminDB.DeleteRNGSRGA(refFdate, refTdate, Group, Table, string.Empty,string.Empty);

                            CaptainModel model = new CaptainModel();
                            RNGSRGAEntity GoalsEntity = new RNGSRGAEntity();

                            GoalsEntity.Code = refFdate.ToString();
                            GoalsEntity.Agency = refTdate;
                            GoalsEntity.TblCode = Table.ToString();
                            GoalsEntity.GrpCode = Group.ToString();
                            GoalsEntity.Mode = "Add";
                            //GoalsEntity.CntCode = ((RepListItem)CmbRsltHead.SelectedItem).Value.ToString();
                            GoalsEntity.LSTCOperator = BaseForm.UserID;
                            BudgetValue = 0;
                            //foreach (DataGridViewRow dr in GvGoals.Rows)
                            //{
                            //    if (dr.Cells["Img_Code"].Value.ToString() == "Y")
                            //    {
                            //        GoalsEntity.GoalCode = dr.Cells["Agy_Code"].Value.ToString();
                            //        GoalsEntity.Desc = dr.Cells["Goals"].Value.ToString();
                            //        if (dr.Cells["Budget"].Value == null) GoalsEntity.Budget = "0";
                            //        else GoalsEntity.Budget = dr.Cells["Budget"].Value.ToString();
                            //        if (!string.IsNullOrEmpty(GoalsEntity.Budget.Trim()))
                            //            BudgetValue += int.Parse(GoalsEntity.Budget.ToString());
                            //        _model.SPAdminData.InsertUpdateRNGSRGA(GoalsEntity);
                            //    }

                            //}

                            foreach (RNGSRGAEntity item in SGoalEntity)
                            {
                                if (item.Sel_Switch.ToString().ToUpper() == "TRUE")
                                {
                                    GoalsEntity.GoalCode = item.GoalCode.ToString();
                                    GoalsEntity.Desc = item.Desc.ToString();
                                    if (PPR_SW == "Y") { GoalsEntity.SerSP = item.SerSP; }
                                    if (string.IsNullOrEmpty(item.Budget)) GoalsEntity.Budget = "0";
                                    else GoalsEntity.Budget = item.Budget.ToString();
                                    if (!string.IsNullOrEmpty(GoalsEntity.Budget.Trim()))
                                        BudgetValue += int.Parse(GoalsEntity.Budget.ToString());
                                    _model.SPAdminData.InsertUpdateRNGSRGA(GoalsEntity);
                                }
                            }

                            SRCsb14GroupEntity TblEntity = new SRCsb14GroupEntity();

                            TblEntity.Code = refFdate;
                            //TblEntity.RefTDate = refTdate;
                            TblEntity.GrpCode = Group.ToString();
                            //TblEntity.GrpDesc = txtDesc.Text;
                            TblEntity.TblCode = Table.ToString();
                            TblEntity.ExAchev = BudgetValue.ToString();
                            //TblEntity.CntIndic = ((ListItem)cmbCntInd.SelectedItem).Value.ToString();
                            //if (chkbCalcCosts.Checked)
                            //    TblEntity.CalCost = "1";
                            //else
                            //    TblEntity.CalCost = "0";
                            //TblEntity.UseSer = chkbUseSer.Checked ? "1" : "0";
                            //TblEntity.Duplicate = chkbDupl.Checked ? "1" : "0";
                            //TblEntity.Disable = chkbDisabled.Checked ? "1" : "0";
                            //TblEntity.AFrom = txtAgeFrm.Text;
                            //TblEntity.Ato = txtAgeTo.Text;
                            TblEntity.LSTCOperator = BaseForm.UserID;
                            TblEntity.AddOperator = BaseForm.UserID;
                            TblEntity.Mode = "CA";
                            _model.SPAdminData.InsertUpdateRNGSRGrp(TblEntity);
                            AlertBox.Show("Service Code Association Saved Successfullly");
                        }

                        if (strCrIndex != 0)
                            strCrIndex = strCrIndex - 1;
                        GvGoals.Enabled = true;
                        GvGoals.ReadOnly = false;
                        //btnSave.Text = "Ch&ange";

                        fillGvGoals();
                        //if(Services == "MS")
                        //    AlertBox.Show("Goal Code Association Saved Successfullly");
                        //else if(Services == "CA")
                        //    AlertBox.Show("Service Code Association Saved Successfullly");

                        //this.DialogResult = DialogResult.OK;
                        //this.Close();
                        if (GvGoals.Rows.Count != 0)
                        {
                            GvGoals.CurrentCell = GvGoals.Rows[strCrIndex].Cells[0];
                            GvGoals.Rows[strCrIndex].Selected = true;
                        }
                    }
                    catch (Exception ex) { }

                }

                string Agy_Goal_code = null;

            }
            else
            {
                if (GvGoals.Rows.Count > 0)
                {
                    //btnSave.Text = "S&ave";
                    Mode = "Edit";
                }
            }

        }

        private string SetLeadingSpaces(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 9: TmpCode = TmpCode + " "; break;
                case 8: TmpCode = TmpCode + "  "; break;
                case 7: TmpCode = TmpCode + "   "; break;
                case 6: TmpCode = TmpCode + "    "; break;
                case 5: TmpCode = TmpCode + "     "; break;
                case 4: TmpCode = TmpCode + "      "; break;
                case 3: TmpCode = TmpCode + "       "; break;
                case 2: TmpCode = TmpCode + "        "; break;
                case 1: TmpCode = TmpCode + "         "; break;
            }
            return (TmpCode);
        }

        private void GvGoals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GvGoals.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {
                        if (GvGoals.CurrentRow.Cells["Img_Code"].Value.ToString() == "Y")
                        {
                            GvGoals.CurrentRow.Cells["Check"].Value = Img_Blank;
                            GvGoals.CurrentRow.Cells["Img_Code"].Value = "N";
                            if (Services == "MS")
                            {
                                foreach (RNGGAEntity item in GoalEntity)
                                {
                                    if (GvGoals.CurrentRow.Cells[3].Value.ToString().Trim() == item.GoalCode.Trim().ToString() && item.SerSP.Trim() == GvGoals.CurrentRow.Cells[6].Value.ToString().Trim())
                                    {
                                        item.Sel_Switch = false;
                                        item.Budget = GvGoals.CurrentRow.Cells["Budget"].Value == null ? "0" : GvGoals.CurrentRow.Cells["Budget"].Value.ToString();
                                    }
                                }
                            }
                            else
                            {
                                foreach (RNGSRGAEntity item in SGoalEntity)
                                {
                                    if (GvGoals.CurrentRow.Cells[3].Value.ToString().Trim() == item.GoalCode.Trim().ToString() && item.SerSP.Trim() == GvGoals.CurrentRow.Cells[6].Value.ToString().Trim())
                                    {
                                        item.Sel_Switch = false;
                                        item.Budget = GvGoals.CurrentRow.Cells["Budget"].Value == null ? "0" : GvGoals.CurrentRow.Cells["Budget"].Value.ToString();
                                    }
                                }

                            }
                        }
                        else
                        {
                            GvGoals.CurrentRow.Cells["Check"].Value = Img_Tick;
                            GvGoals.CurrentRow.Cells["Img_Code"].Value = "Y";
                            if (Services == "MS")
                            {
                                if (GoalEntity.FindAll(u => u.GoalCode.Trim().ToString() == GvGoals.CurrentRow.Cells[3].Value.ToString().Trim() && u.SerSP.Trim() == GvGoals.CurrentRow.Cells[6].Value.ToString().Trim()).Count > 0)
                                {
                                    foreach (RNGGAEntity item in GoalEntity)
                                    {
                                        if (GvGoals.CurrentRow.Cells[3].Value.ToString().Trim() == item.GoalCode.Trim().ToString())
                                        {
                                            item.Sel_Switch = true;
                                            item.Budget = GvGoals.CurrentRow.Cells["Budget"].Value == null ? "0" : GvGoals.CurrentRow.Cells["Budget"].Value.ToString();
                                        }
                                    }

                                }
                                else
                                {
                                    GoalEntity.Add(new Model.Objects.RNGGAEntity(string.Empty, string.Empty, string.Empty, GvGoals.CurrentRow.Cells[3].Value.ToString().Trim(), GvGoals.CurrentRow.Cells[2].Value.ToString(), GvGoals.CurrentRow.Cells[6].Value.ToString(), GvGoals.CurrentRow.Cells[1].Value.ToString()));
                                }
                            }
                            else
                            {
                                if (SGoalEntity.FindAll(u => u.GoalCode.Trim().ToString() == GvGoals.CurrentRow.Cells[3].Value.ToString().Trim() && u.SerSP.Trim() == GvGoals.CurrentRow.Cells[6].Value.ToString().Trim()).Count > 0)
                                {
                                    foreach (RNGSRGAEntity item in SGoalEntity)
                                    {
                                        if (GvGoals.CurrentRow.Cells[3].Value.ToString().Trim() == item.GoalCode.Trim().ToString())
                                        {
                                            item.Sel_Switch = true;
                                            if (string.IsNullOrEmpty(GvGoals.CurrentRow.Cells["Budget"].Value.ToString().Trim())) item.Budget = "0";
                                            else
                                                item.Budget = GvGoals.CurrentRow.Cells["Budget"].Value == null ? "0" : GvGoals.CurrentRow.Cells["Budget"].Value.ToString();
                                        }
                                    }

                                }
                                else
                                {

                                    SGoalEntity.Add(new Model.Objects.RNGSRGAEntity(string.Empty, string.Empty, string.Empty, GvGoals.CurrentRow.Cells[3].Value.ToString().Trim(), GvGoals.CurrentRow.Cells[2].Value.ToString(), GvGoals.CurrentRow.Cells[6].Value.ToString(), GvGoals.CurrentRow.Cells[1].Value.ToString(), GvGoals.CurrentRow.Cells[5].Value.ToString().Trim()));
                                }

                            }

                        }
                    }
                    if (e.ColumnIndex == 4 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {

                    }
                }
            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (RepListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void GvGoals_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex != -1 && Mode!=null)
            {
                if (Mode.Equals("Add") || Mode.Equals("Edit"))
                {
                    if (GvGoals.SelectedRows.Count > 0)
                    {
                        int intcolindex = GvGoals.CurrentCell.ColumnIndex;
                        int introwindex = GvGoals.CurrentCell.RowIndex;

                        string strCurrectValue = Convert.ToString(GvGoals.Rows[introwindex].Cells["Budget"].Value); 
                        string Points = Convert.ToString(GvGoals.Rows[introwindex].Cells["Budget"].Value);

                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            //boolcellstatus = false; IsValid = false;
                            this.GvGoals.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellValueChanged);
                            GvGoals.Rows[introwindex].Cells["Budget"].Value = string.Empty;
                            this.GvGoals.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellValueChanged);

                        }
                        else
                        {
                            if (Services == "MS")
                            {
                                foreach (RNGGAEntity item in GoalEntity)
                                {
                                    if (GvGoals.Rows[introwindex].Cells[3].Value.ToString().Trim() == item.GoalCode.Trim().ToString() && GvGoals.Rows[introwindex].Cells[6].Value.ToString().Trim()==item.SerSP.Trim())
                                    {
                                        item.Budget = strCurrectValue;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (RNGSRGAEntity item in SGoalEntity)
                                {
                                    if (GvGoals.Rows[introwindex].Cells[3].Value.ToString().Trim() == item.GoalCode.Trim().ToString() && GvGoals.Rows[introwindex].Cells[6].Value.ToString().Trim() == item.SerSP.Trim())
                                    {
                                        item.Budget = strCurrectValue;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        private bool ValidateForm()
        {
            bool Isvalidated = true;

            foreach (DataGridViewRow dr in GvGoals.Rows)
            {

            }


            return Isvalidated;
        }

        private void RNG_Goalservices_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string srearch_str = string.Empty;

            if (!string.IsNullOrEmpty(TxtSearch.Text.Trim()))
                srearch_str = (TxtSearch.Text.Trim()).ToUpper();

            fillGvGoals();
        }
    }
}