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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class GoalServiceAssociation : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public GoalServiceAssociation(BaseForm baseform,string mode,string Reffdate,string RefTdate, string gridType,string Grpcd,string tblcd, string services,PrivilegeEntity priviliages)
        {
            InitializeComponent();
            BaseForm = baseform;
            
            Privileage = priviliages;
            _model = new CaptainModel();
            mode = Mode;
            GridType = gridType;
            refFdate = Reffdate;
            refTdate = RefTdate;
            groupCd = Grpcd;
            tablecd = tblcd;
            Services = services;
            this.Text = Privileage.Program + "- Goal/Service Association";
            if(Services=="MS")
                this.Goals.HeaderText = "Outcomes";
            else
                this.Goals.HeaderText = "Critical Activity";
            lblRefDt.Text = refFdate.ToString() + "  " + refTdate.ToString();
            FillComboGrps();
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

        public string groupCd { get; set; }

        public string tablecd { get; set; }

        public string Services { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Performance");
        }
        // string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Tick = "Resources/Images/tick.ico";
        string Img_Blank = "Resources/Icons/16X16/Blank.JPG";

        private void FillComboGrps()
        {
            CmbGrp.Items.Clear();
            List<RepListItem> listItem = new List<RepListItem>();
            List<Csb14GroupEntity> grpcombo;
            //int Sel_grp_index = 0, temp_loopCount = 0;
            grpcombo = _model.SPAdminData.Browse_CSB14Grp(null, null, null, null, null);
            if (grpcombo.Count > 0)
            {
                foreach (Csb14GroupEntity drgrp in grpcombo)
                {
                    if (LookupDataAccess.Getdate(drgrp.RefFDate.ToString()) == refFdate && LookupDataAccess.Getdate(drgrp.RefTDate.ToString()) == refTdate && string.IsNullOrWhiteSpace(drgrp.TblCode.ToString()))
                    {
                        string Tmp_Desc = string.Empty;//String.Format("{0,-10}", drgrp.GrpCode.ToString().Trim() + String.Format("{0,15}", " - " + drgrp.GrpDesc.ToString().Trim()));
                        Tmp_Desc = String.Format("{0,-10}", drgrp.GrpCode.Trim()) +  " - " + drgrp.GrpDesc.ToString().Trim();
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
        string Group = null; string Table = null;
        private void FillTables()
        {
            CmbTbl.Items.Clear();
            Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
            List<RepListItem> listItem = new List<RepListItem>();
            List<Csb14GroupEntity> tblCombo;
            tblCombo = _model.SPAdminData.Browse_CSB14Grp(null, null, null, null, null);
            tblCombo = tblCombo.FindAll(u => LookupDataAccess.Getdate(u.RefFDate).Equals(refFdate) && LookupDataAccess.Getdate(u.RefTDate).Equals(refTdate) && u.GrpCode == Group);
            tblCombo = tblCombo.OrderBy(u => u.TblCode).ToList();
            
            if (tblCombo.Count > 0)
            {
                foreach (Csb14GroupEntity drtbl in tblCombo)
                {
                    //if ((LookupDataAccess.Getdate(drtbl.RefFDate.ToString()) == refFdate) && (LookupDataAccess.Getdate(drtbl.RefTDate.ToString()) == refTdate) && drtbl.GrpCode.ToString() == Group && !string.IsNullOrWhiteSpace(drtbl.TblCode.ToString()))
                    if(!string.IsNullOrWhiteSpace(drtbl.TblCode.ToString()))
                    {
                        string Tmp_Desc = string.Empty;
                        string Temp_Desc = String.Format("{0,-10} ", drtbl.TblCode.ToString().Trim() + " - " + drtbl.GrpDesc.ToString().Trim());
                        CmbTbl.Items.Add(new RepListItem(Temp_Desc, drtbl.TblCode.ToString()));
                        
                    }
                }
                if (CmbTbl.Items.Count == 0)
                {
                    GvGoals.Rows.Clear(); CmbTbl.Enabled = false; btnSave.Visible=false;
                    MessageBox.Show("This Group not having any tables","Goals/Service Association");
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
        List<Csb14GroupEntity> GrpEntity;
        string AgyGoal_code = null;
        private void fillGvGoals()
        {
            Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
            Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
            GrpEntity = _model.SPAdminData.Browse_CSB14Grp(refFdate, refTdate, Group, Table, null);
            //GoalsList = _model.SPAdminData.Get_AgyRecs("Goals");
            MSList = _model.SPAdminData.Browse_MSMAST(null,null,null,null,null);
            MSList = MSList.OrderBy(u => u.Desc.Trim()).ToList();
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            if(Services=="CA")
                this.Goals.HeaderText = "Critical Activity";
            else
                this.Goals.HeaderText = "Goals";
            GvGoals.Rows.Clear();
            bool Can_Continue = true;
            string Goal = null; string goaldesc = null;
            int i = 0;
            string temp_code = null;
            
            foreach (Csb14GroupEntity dr in GrpEntity)
            {
                if (dr.UseSer.ToString().Trim() == "True")
                {
                    Services = "CA";
                    this.Goals.HeaderText = "Critical Activity";
                }
                else
                {
                    Services = "MS";
                    this.Goals.HeaderText = "Goals";
                }
                temp_code = dr.GoalCds.ToString();
                if (!string.IsNullOrWhiteSpace(temp_code) && temp_code.Length >= 1)
                {
                    //Ststus_Exists = false;
                    for (i = 0; Can_Continue; )
                    {
                        if((temp_code.Substring(i,temp_code.Length-i).Length)<10)
                            AgyGoal_code = temp_code.Substring(i, temp_code.Substring(i, temp_code.Length - i).Length);
                        else
                            AgyGoal_code = temp_code.Substring(i, 10);
                        if (Services == "MS")
                        {
                            foreach (MSMASTEntity Entity in MSList)
                            {
                                Goal = Entity.Code;
                                goaldesc = Entity.Desc;
                                if (!string.IsNullOrWhiteSpace(temp_code))
                                {
                                    if (Entity.Code.ToString().Trim() == AgyGoal_code.ToString().Trim())
                                    {
                                        Entity.Sel_SW = true;
                                        //Ststus_Exists = Entity.Sel_WS;
                                    }

                                }
                            }
                        }
                        else
                        {
                            foreach (CAMASTEntity Entity in CAList)
                            {
                                Goal = Entity.Code;
                                goaldesc = Entity.Desc;
                                if (!string.IsNullOrWhiteSpace(temp_code))
                                {
                                    if (Entity.Code.ToString().Trim() == AgyGoal_code.ToString().Trim() && dr.UseSer.ToString().Trim()=="True")
                                    {
                                        Entity.Sel_SW = true;
                                        //Ststus_Exists = Entity.Sel_WS;
                                    }

                                }
                            }
                        }

                        i += 10;
                        if (i >= temp_code.Length)
                        Can_Continue = false;
                    }
                }
            }
            if (Services == "MS")
            {
                foreach (MSMASTEntity Entity in MSList)
                {
                    if (Entity.Sel_SW)
                        GvGoals.Rows.Add(Img_Tick, Entity.Desc, Entity.Code, "Y");
                    else
                        GvGoals.Rows.Add(Img_Blank, Entity.Desc, Entity.Code, "N");
                }
            }
            else
            {
                foreach (CAMASTEntity Entity in CAList)
                {
                    if (Entity.Sel_SW)
                        GvGoals.Rows.Add(Img_Tick, Entity.Desc, Entity.Code, "Y");
                    else
                        GvGoals.Rows.Add(Img_Blank, Entity.Desc, Entity.Code, "N");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CmbTbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Table = tablecd;
            fillGvGoals();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                string Agy_Goal_code = null;
                Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
                Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
                try
                {
                    CaptainModel model = new CaptainModel();
                    Csb14GroupEntity entity = new Csb14GroupEntity();

                    entity.RefFDate = refFdate.ToString();
                    entity.RefTDate = refTdate.ToString();
                    entity.GrpCode = Group.ToString();
                    entity.TblCode = Table.ToString();

                    foreach (DataGridViewRow dr in GvGoals.Rows)
                    {
                        if (dr.Cells["Img_Code"].Value.ToString() == "Y")
                            Agy_Goal_code += SetLeadingSpaces(dr.Cells["Agy_Code"].Value.ToString());
                    }
                    entity.GoalCds = Agy_Goal_code;

                    if (_model.SPAdminData.UpdateCSB14GRP_Goals(entity))
                    {
                        if (strCrIndex != 0)
                            strCrIndex = strCrIndex - 1;
                        GvGoals.Enabled = true;
                        GvGoals.ReadOnly = false;
                        //btnSave.Text = "Ch&ange";

                        fillGvGoals();

                        MessageBox.Show("Service Code Association Saved Successfull", "CAPTAIN");
                        if (GvGoals.Rows.Count != 0)
                        {
                            GvGoals.Rows[strCrIndex].Selected = true;
                        }

                        //Mode = "View";
                    }
                }
                catch (Exception ex)
                {
                }
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
                        }
                        else
                        {
                            GvGoals.CurrentRow.Cells["Check"].Value = Img_Tick;
                            GvGoals.CurrentRow.Cells["Img_Code"].Value = "Y";
                        }
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

    }
}