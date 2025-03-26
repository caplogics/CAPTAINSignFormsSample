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
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SSBGTrigAssns_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public SSBGTrigAssns_Form(BaseForm baseform, PrivilegeEntity privileges, string strtype, string pass_assoc_mode, string id, string goal, string obj, string Trig_Sp_code, string Trig_Sp_Seq, string strHierchyCode, List<SSBGAssnsEntity> pass_CAMS_List, string GoalName, string ObjName, string SPName, List<SSBGSerPlanEntity> pass_SerPlan_List)
        {
            InitializeComponent();
            this.Text = "SSBG Associations";
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            BaseForm = baseform;
            Privileges = privileges;
            SSBG_ID = id;
            SSBGGoal = goal; SSBGobj = obj;
            //Txt_Trig_Name.Text = Pass_Trig_Name = trig_Name;
            SSBG_Seq = Trig_Sp_Seq;
            Pass_CAMS_SP_Code = Pass_Trig_SP_Code = Trig_Sp_code;
            Pass_Assoc_Mode = pass_assoc_mode;
            Type = strtype;
            //Pass_Assoc_Name = Txt_Assoc_Name.Text = trig_Asoc_Name.Trim();
            //DateCombo = comboDate;
            TrigProgram = strHierchyCode;
            Pass_SerPlan_List = pass_SerPlan_List;
            Pass_CAMS_List = pass_CAMS_List;
            Goal_Name = GoalName; Obj_Name = ObjName; SP_Name = SPName;

            //DataSet dsAgycntl = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            //if (dsAgycntl != null && dsAgycntl.Tables[0].Rows.Count > 0)
                XMLHier = TrigProgram;//dsAgycntl.Tables[0].Rows[0]["ACR_XML_HIERARCHY"].ToString().Trim();
            if (!string.IsNullOrEmpty(XMLHier.Trim()))
            {
                XMLAgy = XMLHier.Substring(0, 2).ToString();
                XMLDept = XMLHier.Substring(3, 2).ToString();
                XMLProg = XMLHier.Substring(6, 2).ToString();
            }

            if (Type == Consts.LiheAllDetails.strMainType)
            {
                //**Trig_Assoc_Panel.Location = new System.Drawing.Point(2, 2);
                this.Size = new System.Drawing.Size(620, 400);//(472, 432);
                Trig_Assoc_Panel.Visible = true;

                txtGoal.Text = Goal_Name; txtObj.Text = Obj_Name;

                //if (Pass_Assoc_Mode == Consts.Common.Edit)
                //{
                //    string trigPrg = string.Empty;
                //    if (!string.IsNullOrEmpty(TrigProgram.Trim())) trigPrg = TrigProgram.Substring(0, 2) + "-" + TrigProgram.Substring(2, 2) + "-" + TrigProgram.Substring(4, 2);
                //    //List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("ALL");
                //    List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "Reports");
                //    HierarchyEntity hierar = caseHierarchy.Find(u => u.Code.Equals(trigPrg));
                //    if (hierar != null) txtGoal.Text = TrigProgram + "-" + hierar.HirarchyName;
                //    SP_Grid.Enabled = false;
                //}

                Fill_SP_Grid();
            }
            else
            {
                //**CAMS_Panel.Location = new System.Drawing.Point(2, 2);
                this.Size = new System.Drawing.Size(900, 400);//(626, 276);
                //this.Size = new System.Drawing.Size(513, 276);
                CAMS_Panel.Visible = true;

                txtAGoal.Text = Goal_Name; txtAObj.Text = Obj_Name; txtSerPlan.Text = SP_Name;

                //string trigPrg = string.Empty;
                //if (!string.IsNullOrEmpty(TrigProgram.Trim())) trigPrg = TrigProgram.Substring(0, 2) + "-" + TrigProgram.Substring(2, 2) + "-" + TrigProgram.Substring(4, 2);
                ////List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy(string.Empty);
                //List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(privileges.UserID, "I", "Reports");
                //HierarchyEntity hierar = caseHierarchy.Find(u => u.Code.Equals(trigPrg));
                //if (hierar != null) txtTrigProgram.Text = TrigProgram + "-" + hierar.HirarchyName;

                Fill_SP_CAMS_Details_Grid();
            }

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string SSBG_ID { get; set; }

        public string Pass_Trig_Name { get; set; }
        public string Type { get; set; }


        public string SSBG_Seq { get; set; }
        public string SSBGGoal{ get; set; }
        public string SSBGobj { get; set; }

        public string Goal_Name { get; set; }
        public string Obj_Name { get; set; }
        public string SP_Name { get; set; }

        public string Pass_Trig_SP_Code { get; set; }

        public string Pass_Assoc_Mode { get; set; }

        public string Pass_CAMS_SP_Code { get; set; }

        public string Pass_Assoc_Name { get; set; }

        public string DateCombo { get; set; }

        public string TrigProgram { get; set; }

        public string Hist_Scr_Mode { get; set; }

        public List<SSBGAssnsEntity> Pass_CAMS_List { get; set; }

        public List<SSBGSerPlanEntity> Pass_SerPlan_List { get; set; }

        public string strSeqCode { get; set; }


        #endregion


        string XMLHier = string.Empty; string XMLAgy = string.Empty, XMLDept = string.Empty, XMLProg = string.Empty;
        private void Fill_SP_Grid()
        {
            //XMLHier = string.Empty; XMLAgy = string.Empty; XMLDept = string.Empty; XMLProg = string.Empty;
            List<SSBGGOALSEntity> SSBGGoalsall = _model.CaseSumData.GetSSBGGoals();
            List<SSBGGOALSEntity> caseEligGroups = SSBGGoalsall.FindAll(u => u.SBGGID.Equals(SSBG_ID) && u.SBGGCode.Equals(SSBGGoal.ToString()) && u.SBGGSeq.Equals(SSBGobj.ToString()) && !u.SBGGSeq.Equals("0"));

            string CountType = string.Empty;
            if (caseEligGroups.Count > 0) { if (caseEligGroups[0].SBGGCountType.ToString() == "S" || caseEligGroups[0].SBGGCountType.ToString() == "C" || caseEligGroups[0].Type2.ToString() == "S" || caseEligGroups[0].Type2.ToString() == "C") CountType = "S"; }

            DataSet dsspm = Captain.DatabaseLayer.CaseSum.Browse_SSBGSPMS(XMLAgy, XMLDept, XMLProg);

            DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP1("", XMLAgy, XMLDept, XMLProg);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];
            int TmpCount = 0, REfresh_Sel_Index = 0;

            if (dt.Rows.Count > 0)
            {
                string Tmp_SPCoce = null, Lstc_Date = null, Add_Date = null;
                bool Sel_SP = false; bool SPM = false;
                foreach (DataRow dr in dt.Rows)
                {
                    int rowIndex = 0;
                    //Tmp_SPCoce = dr["sp0_servicecode"].ToString();
                    //Tmp_SPCoce = "000000".Substring(0, (6 - Tmp_SPCoce.Length)) + Tmp_SPCoce;

                    //Add_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dr["sp0_date_add"].ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    //Lstc_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dr["sp0_date_lstc"].ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    //rowIndex = SPGrid.Rows.Add(Tmp_SPCoce, dr["sp0_description"].ToString(), Convert.ToDateTime(dr["sp0_date_add"].ToString()).ToShortDateString(), Convert.ToDateTime(dr["sp0_date_lstc"].ToString()).ToShortDateString(), dr["sp0_validated"].ToString());
                    Sel_SP = false; SPM = false;

                    if (CountType == "S" && dsspm.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drspm in dsspm.Tables[0].Rows)
                        {
                            if (dr["sp0_servicecode"].ToString().Trim() == drspm["SPM_SERVICE_PLAN"].ToString().Trim())
                            {
                                SPM = true; break;
                            }
                        }
                    }

                    if (Pass_Assoc_Mode == Consts.Common.Edit)
                    {
                        foreach (SSBGSerPlanEntity ent1 in Pass_SerPlan_List)
                        {
                            if (ent1.SBSPSerPlan.Trim() == dr["sp0_servicecode"].ToString())
                            {
                                Sel_SP = true; break;
                            }
                        }

                    }
                    //if (Pass_Trig_SP_Code == dr["sp0_servicecode"].ToString())
                    //    Sel_SP = true;
                    if (CountType == "S" && SPM)
                        rowIndex = SP_Grid.Rows.Add(Sel_SP, dr["sp0_description"].ToString(), dr["sp0_servicecode"].ToString(), Lstc_Date, dr["SP0_ACTIVE"].ToString(), dr["SP0_ALLOW_DUPS"].ToString(), dr["SP0_ALLOW_ADLBRANCH"].ToString(), dr["sp0_validated"].ToString());
                    else if (CountType != "S")
                        rowIndex = SP_Grid.Rows.Add(Sel_SP, dr["sp0_description"].ToString(), dr["sp0_servicecode"].ToString(), Lstc_Date, dr["SP0_ACTIVE"].ToString(), dr["SP0_ALLOW_DUPS"].ToString(), dr["SP0_ALLOW_ADLBRANCH"].ToString(), dr["sp0_validated"].ToString());
                    //set_SPGridTooltip(rowIndex, dr);

                    //if (!string.IsNullOrEmpty(Refresh_SP_Code.Trim()))
                    //{
                    //    if (Refresh_SP_Code == Tmp_SPCoce)
                    //        REfresh_Sel_Index = TmpCount;
                    //}

                    //if (dr["sp0_validated"].ToString() == "N" || dr["sp0_validated"].ToString() == "n")
                    //    SPGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue; //Color.Peru; //Color.DarkTurquoise;

                    //if (dr["SP0_ACTIVE"].ToString() == "N")
                    //    SPGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red; //Color.Peru; //Color.DarkTurquoise;

                    TmpCount++;
                }
            }
        }

        List<SSBGSerPlanEntity> SSBGSerPlans = new List<SSBGSerPlanEntity>();
        List<CASESP2Entity> CAMA_Details = new List<CASESP2Entity>();
        private void Fill_SP_CAMS_Details_Grid()
        {
            CAMA_Details.Clear();
            CAMA_Details = _model.SPAdminData.Browse_CASESP2(Pass_CAMS_SP_Code, null, null, null);
            bool CAMS_Selected = false;
            int rowIndex = 0;
            DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP1(Pass_CAMS_SP_Code, XMLAgy, XMLDept, XMLProg);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            foreach (CASESP2Entity Entity in CAMA_Details)
            {
                //if (Entity.Branch == BranchGrid.CurrentRow.Cells["Branch_Code"].Value.ToString())
                {
                    //Tmp_Entity.ServPlan = Entity.ServPlan;
                    //Tmp_Entity.Branch = Entity.Branch;
                    //Tmp_Entity.Orig_Grp = Entity.Orig_Grp;
                    //Tmp_Entity.Type1 = Entity.Type1;
                    //Tmp_Entity.CamCd = Entity.CamCd;
                    //Tmp_Entity.Curr_Grp = Entity.Curr_Grp;
                    //Tmp_Entity.Row = Entity.Row;
                    //Tmp_Entity.DateLstc = Entity.DateLstc;
                    //Tmp_Entity.lstcOperator = Entity.lstcOperator;
                    //Tmp_Entity.Dateadd = Entity.Dateadd;
                    //Tmp_Entity.addoperator = Entity.addoperator;
                    //Tmp_Entity.Rec_Type = Entity.Rec_Type;
                    //Tmp_Entity.CAMS_Desc = Entity.CAMS_Desc;
                    //Tmp_Entity.Shift_Count = Entity.Shift_Count;
                    //Tmp_Entity.CAMS_Post_Count = Entity.CAMS_Post_Count;
                    //Tmp_Entity.SP2_CAMS_Active = Entity.SP2_CAMS_Active;
                    //Tmp_Entity.CAMS_Active = Entity.CAMS_Active;
                    //Tmp_Entity.Existing_CAMS = Entity.Existing_CAMS;
                    //Tmp_Entity.SP2_Auto_Post = Entity.SP2_Auto_Post;

                    //Branch_CAMS_Details.Add(new CASESP2Entity(Tmp_Entity));
                    //Autopost_Disp = (Entity.SP2_Auto_Post == "Y" ? "A" : "");

                    //rowIndex = CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, Autopost_Disp, Entity.Row, Entity.Type1, Entity.Curr_Grp, Entity.Orig_Grp, Entity.CamCd, Entity.CAMS_Post_Count, Entity.CAMS_Post_Count, (Entity.CAMS_Active == "False" ? "N" : Entity.SP2_CAMS_Active), Entity.Existing_CAMS, Entity.SP2_Auto_Post);

                    CAMS_Selected = false; string BranchDesc = string.Empty,Group=string.Empty;
                    if (Pass_Assoc_Mode == Consts.Common.Edit)
                    {
                        foreach (SSBGAssnsEntity ent1 in Pass_CAMS_List)
                        {
                            if (ent1.SBAS_CAMS_Type.Trim() == Entity.Type1.Trim() && ent1.SBAS_CAMS_Code.Trim() == Entity.CamCd.Trim() &&
                               ent1.SBASBranch.Trim() == Entity.Branch.Trim() && ent1.SBAS_org_Grp.Trim() == Entity.Orig_Grp.ToString().Trim())
                            {
                                CAMS_Selected = true; break;
                            }
                        }

                    }
                    if (dt.Rows.Count > 0)
                    {
                        //DataTable dtBranch = new DataTable();
                        //DataView dv = new DataView(dt);
                        //dv.RowFilter = "SP0_SERVICECODE=" + Entity.ServPlan + "AND (SP0_PBRANCH_CODE=" + Entity.Branch + " OR SP0_BRANCH1_CODE= " + Entity.Branch + " OR SP0_BRANCH2_CODE= " + Entity.Branch + " OR SP0_BRANCH3_CODE= " + Entity.Branch + " OR SP0_BRANCH4_CODE= " + Entity.Branch + " OR SP0_BRANCH5_CODE= " + Entity.Branch+")";
                        //dtBranch = dv.ToTable();

                        foreach (DataRow dr in dt.Rows)
                        {
                            switch (Entity.Branch)
                            {
                                case "P": if (dr["SP0_PBRANCH_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_PBRANCH_DESC"].ToString(); break;
                                case "1": if (dr["SP0_BRANCH1_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH1_DESC"].ToString(); break;
                                case "2": if (dr["SP0_BRANCH2_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH2_DESC"].ToString(); break;
                                case "3": if (dr["SP0_BRANCH3_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH3_DESC"].ToString(); break;
                                case "4": if (dr["SP0_BRANCH4_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH4_DESC"].ToString(); break;
                                case "5": if (dr["SP0_BRANCH5_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH5_DESC"].ToString(); break;
                            }
                        }
                    }

                    rowIndex = CAMS_Grid.Rows.Add(CAMS_Selected, BranchDesc, Entity.Curr_Grp, Entity.CAMS_Desc, Entity.Type1, Entity.Row, Entity.Branch, Entity.CamCd.Trim(), Entity.Orig_Grp);

                    if (Entity.CAMS_Active == "False" || Entity.SP2_CAMS_Active != "A") //(Entity.SP2_CAMS_Active == "I" || string.IsNullOrEmpty(Entity.SP2_CAMS_Active.Trim())))
                        CAMS_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;  // Color.Red;
                }
            }
        }

        private void Pb_SPM_Prog_Click(object sender, EventArgs e)
        {

        }

        private void SP_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (SP_Grid.Rows.Count > 0)
            //{
            //    int ColIdx = 0;
            //    int RowIdx = 0;
            //    ColIdx = SP_Grid.CurrentCell.ColumnIndex;
            //    RowIdx = SP_Grid.CurrentCell.RowIndex;

            //    if (e.ColumnIndex == 0 && e.RowIndex != -1)
            //    {
            //        foreach (DataGridViewRow dr in SP_Grid.Rows)
            //        {
            //            if (dr.Cells["Sp_Code"].Value.ToString() != SP_Grid.CurrentRow.Cells["Sp_Code"].Value.ToString())
            //                dr.Cells["SP_Sel"].Value = false;
            //        }
            //    }
            //}
        }

        string Sql_SP_Result_Message = string.Empty;
        private void Btn_Assoc_Save_Click(object sender, EventArgs e)
        {
            if (Validate_SP_Selection())
            {
                //string Sel_SP = ""; //string Seq = string.Empty;
                //foreach (DataGridViewRow dr in SP_Grid.Rows)
                //{
                //    if (dr.Cells["SP_Sel"].Value.ToString() == true.ToString())
                //        Sel_SP = dr.Cells["Sp_Code"].Value.ToString();
                //}

                //if (_model.CaseSumData.UpdateSSBGSerPlan(Pass_Assoc_Mode == Consts.Common.Add ? "I" : "U", SSBG_ID, SSBGGoal, SSBGobj, Sel_SP, Pass_Assoc_Mode == Consts.Common.Add ? "1" : SSBG_Seq, Privileges.UserID, out Sql_SP_Result_Message))
                //{
                    
                //        this.DialogResult = DialogResult.OK;
                //    this.Close();
                //}

                StringBuilder Xml_HistTo_Pass = new StringBuilder();
                Xml_HistTo_Pass.Append("<Rows>");

                foreach (DataGridViewRow dr in SP_Grid.Rows)
                {
                    if (dr.Cells["SP_Sel"].Value.ToString() == true.ToString())
                    {
                        Xml_HistTo_Pass.Append("<Row SBSP_ID = \"" + SSBG_ID + "\" SBSP_GOAL = \"" + SSBGGoal + "\" SBSP_OBJ = \"" + SSBGobj + "\" SBSP_SERPLAN = \"" + dr.Cells["Sp_Code"].Value.ToString().Trim() + "\" SBSP_SEQ = \"" + "1" + "\" SBSP_LSTC_OPERATOR = \"" + Privileges.UserID + "\"/>");
                    }
                }

                Xml_HistTo_Pass.Append("</Rows>");
                if (_model.CaseSumData.UpdateSSBGSerPlan("U", SSBG_ID, SSBGGoal, SSBGobj, "", "1", Xml_HistTo_Pass.ToString(), Privileges.UserID, out Sql_SP_Result_Message))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

            }
            else
            {
                bool can_Save = true, SP_Selected = false;
                foreach (DataGridViewRow dr in SP_Grid.Rows)
                {
                    if (dr.Cells["SP_Sel"].Value.ToString() == true.ToString())
                    {
                        SP_Selected = true;
                        break;
                    }
                }
                if (!SP_Selected)
                {
                    AlertBox.Show("Please Select a Service Plan", MessageBoxIcon.Warning);
                }
            }
        }

        private bool Validate_SP_Selection()
        {
            bool can_Save = true, SP_Selected = false;
            foreach (DataGridViewRow dr in SP_Grid.Rows)
            {
                if (dr.Cells["SP_Sel"].Value.ToString() == true.ToString())
                    SP_Selected = true;
            }
            if (!SP_Selected)
            {
                can_Save = false;
                _errorProvider.SetError(SP_Grid, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Service Paln".Replace(Consts.Common.Colon, string.Empty)));
            }
            else
                _errorProvider.SetError(SP_Grid, null);


            //if (string.IsNullOrEmpty(Txt_SPM_Program.Text.Trim()))
            //{
            //    can_Save = false;
            //    _errorProvider.SetError(Pb_SPM_Prog, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Program".Replace(Consts.Common.Colon, string.Empty)));
            //}
            //else
            //    _errorProvider.SetError(Pb_SPM_Prog, null);


            //if (string.IsNullOrEmpty(Txt_Assoc_Name.Text.Trim()))
            //{
            //    can_Save = false;
            //    _errorProvider.SetError(Txt_Assoc_Name, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Association Name".Replace(Consts.Common.Colon, string.Empty)));
            //}
            //else
            //    _errorProvider.SetError(Txt_Assoc_Name, null);


            return can_Save;
        }

        private bool Validate_CAMS_Selection()
        {
            bool can_Save = true, SP_Selected = false;
            foreach (DataGridViewRow dr in CAMS_Grid.Rows)
            {
                if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                    SP_Selected = true;
            }
            if (!SP_Selected)
            {
                can_Save = false;
                _errorProvider.SetError(txtAGoal, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "CA/MS".Replace(Consts.Common.Colon, string.Empty)));
            }
            else
                _errorProvider.SetError(txtAGoal, null);


            return can_Save;
        }

        private void Btn_Save_CAMS_Click(object sender, EventArgs e)
        {
            if (Validate_CAMS_Selection())
            {
                StringBuilder Xml_HistTo_Pass = new StringBuilder();
                Xml_HistTo_Pass.Append("<Rows>");

                foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                {
                    if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                    {
                        Xml_HistTo_Pass.Append("<Row SBAS_ID = \"" + SSBG_ID + "\" SBAS_GOAL = \"" + SSBGGoal + "\" SBAS_OBJ = \"" + SSBGobj + "\" SBAS_SERPLAN = \"" + Pass_CAMS_SP_Code + "\" SBAS_SEQ = \"" + SSBG_Seq + "\" SBAS_BRANCH = \"" + dr.Cells["CAMS_Branch"].Value.ToString().Trim() + "\" SBAS_ORIG_GRP = \"" + dr.Cells["CAMS_CurrGrp"].Value.ToString().Trim() + "\" SBAS_CAMS = \"" + dr.Cells["CAMS_Type"].Value.ToString().Trim() + "\" SBAS_CAMS_CODE = \"" + dr.Cells["CAMS_cams_Code"].Value.ToString().Trim() + "\" SBAS_LSTC_OPERATOR = \"" + Privileges.UserID + "\"/>");
                    }
                }

                Xml_HistTo_Pass.Append("</Rows>");
                if (_model.CaseSumData.UpdateSSBGAssns("U", SSBG_ID, SSBGGoal, SSBGobj, Pass_CAMS_SP_Code,SSBG_Seq,"", "1", "", "", Xml_HistTo_Pass.ToString(), Privileges.UserID, out Sql_SP_Result_Message))
                {
                    //if (Pass_Assoc_Mode == Consts.Common.Edit)
                    //{
                    //    StringBuilder Trig_Old_Hist = new StringBuilder();
                    //    StringBuilder Trig_New_Hist = new StringBuilder();
                    //    Trig_Old_Hist.Append("<Rows>");
                    //    Trig_New_Hist.Append("<Rows>");

                    //    foreach (TrigAssns_Entity ent1 in Pass_CAMS_List)
                    //    {
                    //        Trig_Old_Hist.Append("<Row CAMS_SP = \"" + Pass_CAMS_SP_Code + "\" CAMS_CODE = \"" + ent1.Trig_CAMS_Code.Trim() + "\" CAMS_TYPE = \"" + ent1.Trig_CAMS_Type.Trim() + "\" CAMS_BRANCH = \"" + ent1.Trig_Branch.Trim() + "\" CAMS_GROUP = \"" + ent1.Trig_org_Grp.Trim() + "\"/>");
                    //    }

                    //    foreach (DataGridViewRow dr in CAMS_Grid.Rows)
                    //    {
                    //        if (dr.Cells["CAMS_Sel"].Value.ToString() == true.ToString())
                    //        {
                    //            Trig_New_Hist.Append("<Row CAMS_SP = \"" + Pass_CAMS_SP_Code + "\" CAMS_CODE = \"" + dr.Cells["CAMS_cams_Code"].Value.ToString().Trim() + "\" CAMS_TYPE = \"" + dr.Cells["CAMS_Type"].Value.ToString().Trim() + "\" CAMS_BRANCH = \"" + dr.Cells["CAMS_Branch"].Value.ToString().Trim() + "\" CAMS_GROUP = \"" + dr.Cells["CAMS_Org_Grp"].Value.ToString().Trim() + "\"/>");
                    //        }
                    //    }

                    //    Trig_Old_Hist.Append("</Rows>");
                    //    Trig_New_Hist.Append("</Rows>");


                    //    _model.LiheAllData.Update_Trig_Assns_History(Pass_Trig_ID, Pass_Trig_Seq, Trig_Old_Hist.ToString(), Trig_New_Hist.ToString(), Privileges.UserID, out Sql_SP_Result_Message); // Rao 555
                    //}

                    //CTTRIGCRITEntity SP_Header_Rec = new CTTRIGCRITEntity();
                    //SP_Header_Rec.TRIGCRITCode = Pass_Trig_ID; SP_Header_Rec.TRIGCRITGroupCode = "TH";
                    //SP_Header_Rec.TRIGCRITGroupDesc = Pass_Trig_Name; SP_Header_Rec.Type = "Trigger";
                    //SP_Header_Rec.TRIGCRITValidated = "N"; SP_Header_Rec.LstcOperator = Privileges.UserID; SP_Header_Rec.Mode = "Edit"; SP_Header_Rec.TRIGCRITGroupSeq = "0";
                    //string strMsg = string.Empty;

                    //if (_model.LiheAllData.InsertUpdateDelCTTRIGCRIT(SP_Header_Rec, out strMsg))
                    //{
                    //    CTTriggersControl TrigControl = BaseForm.GetBaseUserControl() as CTTriggersControl;
                    //    if (TrigControl != null)
                    //    {
                    //        TrigControl.RefreshGrid(Pass_Trig_ID);
                    //        this.DialogResult = DialogResult.OK;
                    //    }
                    //    this.Close();
                    //}

                    //this.DialogResult = DialogResult.OK;
                    //SSBG_Associations_Form TrigControl = Form as SSBG_Associations_Form;
                    //if (TrigControl != null)
                    //{
                    //    //TrigControl.RefreshSPs(Pass_Trig_ID);
                    //    TrigControl.RefreshAssocGrid();
                    //    //strSeqCode = Pass_Trig_Seq;
                        this.DialogResult = DialogResult.OK;
                    //}
                    this.Close();
                }
            }
        }
    }
}