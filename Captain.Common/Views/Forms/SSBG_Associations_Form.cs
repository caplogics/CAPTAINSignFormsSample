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
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SSBG_Associations_Form : Form
    {
        #region private variables

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public SSBG_Associations_Form(BaseForm baseForm, PrivilegeEntity privilieges, string strtype, string strID, string strHierchyCode,string goalcd,string objcd)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseForm;
            Privileges = privilieges;
            this.Text = Privileges.PrivilegeName;//Privileges.Program;
            strType = strtype;
            ID = strID;
            //CodeSeq = strSeq;
            GroupCode = "0";
            //TypeCode = strTypeCode;
            strHierachycode = strHierchyCode;
            Goalcode = goalcd;
            ObjectCode = objcd;

            //DataSet dsAgycntl = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            //if (dsAgycntl != null && dsAgycntl.Tables[0].Rows.Count > 0)
            XMLHier = strHierachycode;//dsAgycntl.Tables[0].Rows[0]["ACR_XML_HIERARCHY"].ToString().Trim();
            if (!string.IsNullOrEmpty(XMLHier.Trim()))
            {
                XMLAgy = XMLHier.Substring(0, 2).ToString();
                XMLDept = XMLHier.Substring(3, 2).ToString();
                XMLProg = XMLHier.Substring(6, 2).ToString();
                //XMLAgy = XMLHier.Substring(0, 2).ToString();
                //XMLDept = XMLHier.Substring(2, 2).ToString();
                //XMLProg = XMLHier.Substring(4, 2).ToString();
            }



            fillGoalcombo(Goalcode);
            fillObjectiveCombo(ObjectCode);
        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string strType { get; set; }

        public string ID { get; set; }
        public string CodeSeq { get; set; }
        public string TypeCode { get; set; }
        public string QuesCode { get; set; }
        public string Goalcode { get; set; }
        public string ObjectCode { get; set; }
        public string strHierachycode { get; set; }
        public string Mode { get; set; }
        public string GroupCode { get; set; }

        public string XMLHier { get; set; }
        public string XMLAgy { get; set; }
        public string XMLDept { get; set; }
        public string XMLProg { get; set; }


        #endregion

        private void fillGoalcombo(string Goal)
        {
            List<SSBGGOALSEntity> SSBGGoalsall = _model.CaseSumData.GetSSBGGoals();
            List<SSBGGOALSEntity> caseEligGroups = SSBGGoalsall.FindAll(u => u.SBGGID.Equals(ID) && u.SBGGSeq.Equals("0"));
            int Tmp_Count = 0; int rowIndex = 0; int selCnt = 0;
            if (strType == "SP")
            {
                cmbSPGoals.Items.Clear();
                //cmbSPGoals.Items.Insert(0, new ListItem("    ", "0"));
                if (caseEligGroups.Count > 0)
                {
                    foreach (SSBGGOALSEntity Entity in caseEligGroups)
                    {
                        rowIndex=cmbSPGoals.Items.Add(new ListItem(Entity.SBGGDesc, Entity.SBGGCode, Entity.SBGGCountType, Entity.Type2, string.Empty, string.Empty));
                        if (Entity.SBGGCode.Trim() == Goal.Trim())
                            selCnt = rowIndex;
                        Tmp_Count++;
                    }

                    cmbSPGoals.SelectedIndex = selCnt;
                }
                if(Tmp_Count==0) 
                {
                    cmbSPGoals.Items.Insert(0, new ListItem("    ", "0"));
                    cmbSPGoals.SelectedIndex = selCnt;
                }
                else
                {
                    if (((ListItem)cmbSPGoals.SelectedItem).ID.ToString() == "S" || ((ListItem)cmbSPGoals.SelectedItem).ID.ToString() == "C" || ((ListItem)cmbSPGoals.SelectedItem).ValueDisplayCode.ToString() == "S" || ((ListItem)cmbSPGoals.SelectedItem).ValueDisplayCode.ToString() == "C")
                        this.CAMS.Hide();
                    else this.CAMS.Show();
                }
            }
            else if (strType == "CAMS")
            {
                cmbGoals.Items.Clear();
                //cmbGoals.Items.Insert(0, new ListItem("    ", "0"));
                if (caseEligGroups.Count > 0)
                {
                    foreach (SSBGGOALSEntity Entity in caseEligGroups)
                    {
                        rowIndex=cmbGoals.Items.Add(new ListItem(Entity.SBGGDesc, Entity.SBGGCode, Entity.SBGGCountType, Entity.Type2, string.Empty, string.Empty));
                        if (Entity.SBGGCode.Trim() == Goal.Trim())
                            selCnt = rowIndex;
                        Tmp_Count++;
                    }
                    cmbGoals.SelectedIndex = selCnt;
                }
                if(Tmp_Count==0) 
                {
                    cmbGoals.Items.Insert(0, new ListItem("    ", "0"));
                    cmbGoals.SelectedIndex = selCnt;
                }
            }
        }

        private void fillObjectiveCombo(string Object)
        {
            List<SSBGGOALSEntity> SSBGGoalsall = _model.CaseSumData.GetSSBGGoals();

            int Tmp_Count = 0; int rowIndex = 0; int selCnt = 0;
            if (strType == "SP")
            {
                List<SSBGGOALSEntity> caseEligGroups = SSBGGoalsall.FindAll(u => u.SBGGID.Equals(ID) && u.SBGGCode.Equals(((ListItem)cmbSPGoals.SelectedItem).Value.ToString()) && !u.SBGGSeq.Equals("0"));
                cmbSPObj.Items.Clear();
                //cmbSPObj.Items.Insert(0, new ListItem("    ", "0"));
                if (caseEligGroups.Count > 0)
                {
                    foreach (SSBGGOALSEntity Entity in caseEligGroups)
                    {
                        rowIndex = cmbSPObj.Items.Add(new ListItem(Entity.SBGGDesc, Entity.SBGGSeq, Entity.SBGGCountType, Entity.Type2, string.Empty,string.Empty));
                        if (Entity.SBGGSeq.Trim() == Object.Trim())
                            selCnt = rowIndex;

                        Tmp_Count++;
                    }
                    cmbSPObj.SelectedIndex = selCnt;
                }

                if (Tmp_Count == 0)
                {
                    cmbSPObj.Items.Insert(0, new ListItem("    ", "0"));
                    cmbSPObj.SelectedIndex = selCnt;
                }
                else
                {
                    if (((ListItem)cmbSPGoals.SelectedItem).ID.ToString() == "S" || ((ListItem)cmbSPGoals.SelectedItem).ID.ToString() == "C" || ((ListItem)cmbSPGoals.SelectedItem).ValueDisplayCode.ToString() == "S" || ((ListItem)cmbSPGoals.SelectedItem).ValueDisplayCode.ToString() == "C")
                        this.CAMS.Hide();
                    else this.CAMS.Show();
                }
            }
            else if (strType == "CAMS")
            {
                List<SSBGGOALSEntity> caseEligGroups = SSBGGoalsall.FindAll(u => u.SBGGID.Equals(ID) && u.SBGGCode.Equals(((ListItem)cmbGoals.SelectedItem).Value.ToString()) && !u.SBGGSeq.Equals("0"));
                cmbObjective.Items.Clear();
                //cmbObjective.Items.Insert(0, new ListItem("    ", "0"));
                if (caseEligGroups.Count > 0)
                {
                    foreach (SSBGGOALSEntity Entity in caseEligGroups)
                    {
                        if((Entity.SBGGCountType.Trim()!="S" && Entity.SBGGCountType.Trim()!="C") || (Entity.Type2.Trim()!="S" && Entity.Type2.Trim()!="C"))
                            rowIndex=cmbObjective.Items.Add(new ListItem(Entity.SBGGDesc, Entity.SBGGSeq));
                        if (Entity.SBGGSeq.Trim() == Object.Trim())
                            selCnt = rowIndex;
                        Tmp_Count++;
                    }
                    cmbObjective.SelectedIndex = selCnt;
                }
                if(Tmp_Count==0) 
                {
                    cmbObjective.Items.Insert(0, new ListItem("    ", "0"));
                    cmbObjective.SelectedIndex = selCnt;
                    gvwCAMS.Rows.Clear();
                }
            }

        }

        private void fillcomboSP()
        {
            SSBGSerPlans = _model.CaseSumData.Browse_SSBGSerplan(ID, string.Empty, string.Empty, string.Empty);
            List<SSBGSerPlanEntity> SSBGSelSerPlans = SSBGSerPlans.FindAll(u => u.SBSPGoal.Equals(((ListItem)cmbGoals.SelectedItem).Value.ToString()) && u.SBSPOBJ.Equals(((ListItem)cmbObjective.SelectedItem).Value.ToString()));
            cmbSerPlan.Items.Clear();
            if (SSBGSelSerPlans.Count > 0)
            {
                foreach (SSBGSerPlanEntity Entity in SSBGSelSerPlans)
                {
                    cmbSerPlan.Items.Add(new ListItem(Entity.SBSPSPName.Trim(), Entity.SBSPSerPlan, Entity.SBSPSeq, string.Empty));
                }
                cmbSerPlan.SelectedIndex = 0;
            }
            else
            {
                gvwCAMS.Rows.Clear();
            }
        }

        List<SSBGSerPlanEntity> SSBGSerPlans = new List<SSBGSerPlanEntity>();
        List<SSBGAssnsEntity> SSBGCAMS_List = new List<SSBGAssnsEntity>();
        public void RefreshAssocGrid()
        {
            SSBGSerPlans = _model.CaseSumData.Browse_SSBGSerplan(ID, string.Empty,string.Empty, string.Empty);
            SSBGCAMS_List = _model.CaseSumData.Browse_SSBGAssns(ID, string.Empty,string.Empty,string.Empty, string.Empty);

            if (strType == "SP")
                Fill_Assoc_Grid();
            else if (strType == "CAMS")
                FillCAMSGrid();
        }

        private void Fill_Assoc_Grid()
        {
            gvSerPlan.Rows.Clear();

            SSBGSerPlans = _model.CaseSumData.Browse_SSBGSerplan(ID, string.Empty, string.Empty, string.Empty);
            List<SSBGSerPlanEntity> SSBGSelSerPlans = SSBGSerPlans.FindAll(u=>u.SBSPGoal.Equals(((ListItem)cmbSPGoals.SelectedItem).Value.ToString()) && u.SBSPOBJ.Equals(((ListItem)cmbSPObj.SelectedItem).Value.ToString()));
            if (SSBGSelSerPlans.Count > 0)
            {
                int rowIndex = 0, row_Cnt = 0;
                foreach (SSBGSerPlanEntity Entity in SSBGSelSerPlans)
                {
                    
                    rowIndex = gvSerPlan.Rows.Add(Entity.SBSPSeq, Entity.SBSPSPName, Entity.SBSPSerPlan,Entity.SBSP_Assoc_CAMS_Cnt);

                    set_SPSEQ_GridTooltip(rowIndex, Entity);

                    row_Cnt++;
                }
                if (row_Cnt > 0)
                {
                    Pb_Edit_Assoc.Visible = true; Pb_Delete_Assoc.Visible = true; Pb_Add_Assoc.Visible = Pb_Add_Assoc.Visible = false;
                }
                else
                {
                    Pb_Add_Assoc.Visible = true; Pb_Edit_Assoc.Visible = false; Pb_Delete_Assoc.Visible = false;
                }

                //if (gvSerPlan.Rows.Count > 0)
                //    FillCAMSGrid();
            }
            else
            {
                Pb_Add_Assoc.Visible = true; Pb_Edit_Assoc.Visible = false; Pb_Delete_Assoc.Visible = false;
            }
            if(gvSerPlan.Rows.Count > 0)
                gvSerPlan.Rows[0].Selected = true;
        }

        private void FillCAMSGrid()
        {
            gvwCAMS.Rows.Clear();

            DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP1(((ListItem)cmbSerPlan.SelectedItem).Value.ToString(), XMLAgy, XMLDept, XMLProg);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];
            SSBGCAMS_List = _model.CaseSumData.Browse_SSBGAssns(ID, string.Empty,string.Empty,string.Empty, string.Empty);
            List<SSBGAssnsEntity> SSBGSelAssns = SSBGCAMS_List.FindAll(u => u.SBASGoal.Equals(((ListItem)cmbGoals.SelectedItem).Value.ToString()) && u.SBASOBJ.Equals(((ListItem)cmbObjective.SelectedItem).Value.ToString()) && u.SBASSerPlan.Equals(((ListItem)cmbSerPlan.SelectedItem).Value.ToString()) && u.SBASSeq.Equals(((ListItem)cmbSerPlan.SelectedItem).ID.ToString()));//, string.Empty);
            List<CASESP2Entity> SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(null, null, null, null);
            SP_CAMS_Details = SP_CAMS_Details.FindAll(u => u.ServPlan.Equals(((ListItem)cmbSerPlan.SelectedItem).Value.ToString()));

            if (SSBGSelAssns.Count > 0)
            {
                int rowIndex = 0, row_Cnt = 0;
                foreach (SSBGAssnsEntity Entity in SSBGSelAssns)
                {
                    string BranchDesc = string.Empty; string CurrGrp = string.Empty;
                    if (dt.Rows.Count > 0)
                    {
                        //DataTable dtBranch = new DataTable();
                        //DataView dv = new DataView(dt);
                        //dv.RowFilter = "SP0_SERVICECODE=" + Entity.ServPlan + "AND (SP0_PBRANCH_CODE=" + Entity.Branch + " OR SP0_BRANCH1_CODE= " + Entity.Branch + " OR SP0_BRANCH2_CODE= " + Entity.Branch + " OR SP0_BRANCH3_CODE= " + Entity.Branch + " OR SP0_BRANCH4_CODE= " + Entity.Branch + " OR SP0_BRANCH5_CODE= " + Entity.Branch+")";
                        //dtBranch = dv.ToTable();

                        foreach (DataRow dr in dt.Rows)
                        {
                            switch (Entity.SBASBranch)
                            {
                                case "P": if (dr["SP0_PBRANCH_CODE"].ToString() == Entity.SBASBranch) BranchDesc = dr["SP0_PBRANCH_DESC"].ToString(); break;
                                case "1": if (dr["SP0_BRANCH1_CODE"].ToString() == Entity.SBASBranch) BranchDesc = dr["SP0_BRANCH1_DESC"].ToString(); break;
                                case "2": if (dr["SP0_BRANCH2_CODE"].ToString() == Entity.SBASBranch) BranchDesc = dr["SP0_BRANCH2_DESC"].ToString(); break;
                                case "3": if (dr["SP0_BRANCH3_CODE"].ToString() == Entity.SBASBranch) BranchDesc = dr["SP0_BRANCH3_DESC"].ToString(); break;
                                case "4": if (dr["SP0_BRANCH4_CODE"].ToString() == Entity.SBASBranch) BranchDesc = dr["SP0_BRANCH4_DESC"].ToString(); break;
                                case "5": if (dr["SP0_BRANCH5_CODE"].ToString() == Entity.SBASBranch) BranchDesc = dr["SP0_BRANCH5_DESC"].ToString(); break;
                            }
                        }
                    }

                    if (SP_CAMS_Details.Count > 0)
                    {
                        List<CASESP2Entity> SelSpCAMS = SP_CAMS_Details.FindAll(u => u.Branch.ToString().Trim().Equals(Entity.SBASBranch.Trim()) && u.CamCd.ToString().Trim().Equals(Entity.SBAS_CAMS_Code.Trim()) && u.Orig_Grp.ToString().Trim().Equals(Entity.SBAS_org_Grp.ToString().Trim()));
                        if (SelSpCAMS.Count > 0)
                            CurrGrp = SelSpCAMS[0].Curr_Grp.ToString();

                    }
                    if (string.IsNullOrEmpty(CurrGrp.Trim())) CurrGrp = Entity.SBAS_org_Grp;

                    rowIndex = gvwCAMS.Rows.Add(BranchDesc, CurrGrp, Entity.SBAS_CAMS_Type, Entity.CAMS_DESC, Entity.SBAS_CAMS_Code, Entity.SBASBranch);
                    set_CAMS_GridTooltip(rowIndex, Entity);


                    row_Cnt++;
                }
                if (row_Cnt > 0)
                {
                    Pb_CAMS_Add.Visible = Pb_CAMS_Edit.Visible = Pb_CAMS_Delete.Visible = true;
                }
                else
                {
                    Pb_CAMS_Add.Visible = true; Pb_CAMS_Edit.Visible = Pb_CAMS_Delete.Visible = false;
                }
            }
            else { Pb_CAMS_Add.Visible = true; Pb_CAMS_Edit.Visible = Pb_CAMS_Delete.Visible = false; }

            if (gvwCAMS.Rows.Count > 0)
                gvwCAMS.Rows[0].Selected = true;
        }

        private void set_SPSEQ_GridTooltip(int rowIndex, SSBGSerPlanEntity ent)
        {
            string toolTipText = "Added By     : " + ent.AddOperator.Trim() + " on " + ent.DateAdd + "\n" +
                                 "Modified By  : " + ent.LstcOperator.Trim() + " on " + ent.DateLstc.Trim();

            foreach (DataGridViewCell cell in gvSerPlan.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        private void set_CAMS_GridTooltip(int rowIndex, SSBGAssnsEntity ent)
        {
            string toolTipText = "Added By     : " + ent.AddOperator.Trim() + " on " + ent.DateAdd + "\n" +
                                 "Modified By  : " + ent.LstcOperator.Trim() + " on " + ent.DateLstc.Trim();

            foreach (DataGridViewCell cell in gvwCAMS.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void picGroupAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void txtSeq_LostFocus(object sender, EventArgs e)
        {

        }

        private void txtGrpCode_LostFocus(object sender, EventArgs e)
        {

        }

        private void gvMat_Groups_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvMat_Groups_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void picAddQues_Click(object sender, EventArgs e)
        {

        }

        private void btnQSave_Click(object sender, EventArgs e)
        {

        }

        private void btnQCancel_Click(object sender, EventArgs e)
        {

        }

        private void cmbQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbMemAccess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtEqualTo_Leave(object sender, EventArgs e)
        {

        }

        private void txtGreaterthan_Leave(object sender, EventArgs e)
        {

        }

        private void txtLessthan_Leave(object sender, EventArgs e)
        {

        }

        private void gvwQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvwQuestions_SelectionChanged(object sender, EventArgs e)
        {

        }


        List<SSBGAssnsEntity> Sel_Assoc_CAMS_List = new List<SSBGAssnsEntity>();
        private List<SSBGAssnsEntity> Get_Sele_Assoc_CAMS()
        {
            Sel_Assoc_CAMS_List.Clear();
            if (gvwCAMS.Rows.Count > 0 )// && Trigger_Cnt > 0)
            {
                string Curr_Trig_ID = ID,Curr_Goal=((ListItem)cmbGoals.SelectedItem).Value.ToString();
                string Curr_Obj=((ListItem)cmbObjective.SelectedItem).Value.ToString();
                //string Curr_Seq = Trig_Assoc_Grid.CurrentRow.Cells["Trig_Seq"].Value.ToString().Trim();
                string Curr_SP = ((ListItem)cmbSerPlan.SelectedItem).Value.ToString();
                if (SSBGCAMS_List.Count > 0)
                {
                    int rowIndex = 0, row_Cnt = 0;
                    foreach (SSBGAssnsEntity ent in SSBGCAMS_List)
                    {
                        if (Curr_Trig_ID == ent.SBASID && Curr_Goal == ent.SBASGoal && Curr_Obj == ent.SBASOBJ && ent.SBASSerPlan == Curr_SP && ent.SBASSeq == ((ListItem)cmbSerPlan.SelectedItem).ID.ToString())
                        {
                            Sel_Assoc_CAMS_List.Add(new SSBGAssnsEntity(ent));
                            //rowIndex = Trig_CAMS_Grid.Rows.Add(ent.Trig_Branch, ent.Trig_CAMS_Type, ent.CAMS_DESC);
                            //row_Cnt++;
                        }
                    }

                    if (row_Cnt > 0)
                    {
                        Pb_Add_Assoc.Visible = Pb_Add_Assoc.Visible = true;
                    }
                }
            }

            return Sel_Assoc_CAMS_List;
        }

        List<SSBGSerPlanEntity> Sel_Assoc_SerPlan_List = new List<SSBGSerPlanEntity>();
        private List<SSBGSerPlanEntity> Get_Sele_Assoc_SerPlan()
        {
            Sel_Assoc_SerPlan_List.Clear();
            if (gvSerPlan.Rows.Count > 0)// && Trigger_Cnt > 0)
            {
                string Curr_Trig_ID = ID, Curr_Goal = ((ListItem)cmbSPGoals.SelectedItem).Value.ToString();
                string Curr_Obj = ((ListItem)cmbSPObj.SelectedItem).Value.ToString();
                //string Curr_Seq = Trig_Assoc_Grid.CurrentRow.Cells["Trig_Seq"].Value.ToString().Trim();
                //string Curr_SP = ((ListItem)cmbSerPlan.SelectedItem).Value.ToString();
                if (SSBGSerPlans.Count > 0)
                {
                    int rowIndex = 0, row_Cnt = 0;
                    foreach (SSBGSerPlanEntity ent in SSBGSerPlans)
                    {
                        if (Curr_Trig_ID == ent.SBSPID && Curr_Goal == ent.SBSPGoal && Curr_Obj == ent.SBSPOBJ)//&& ent.SBASSerPlan == Curr_SP && ent.SBASSeq == ((ListItem)cmbSerPlan.SelectedItem).ID.ToString())
                        {
                            Sel_Assoc_SerPlan_List.Add(new SSBGSerPlanEntity(ent));
                            //rowIndex = Trig_CAMS_Grid.Rows.Add(ent.Trig_Branch, ent.Trig_CAMS_Type, ent.CAMS_DESC);
                            //row_Cnt++;
                        }
                    }

                    if (row_Cnt > 0)
                    {
                        Pb_Add_Assoc.Visible = Pb_Add_Assoc.Visible = true;
                    }
                }
            }

            return Sel_Assoc_SerPlan_List;
        }

        private void Pb_Add_Assoc_Click(object sender, EventArgs e)
        {
            SSBGTrigAssns_Form Assn_form = new SSBGTrigAssns_Form(BaseForm, Privileges, Consts.LiheAllDetails.strMainType, "Add", ID, ((ListItem)cmbSPGoals.SelectedItem).Value.ToString(), ((ListItem)cmbSPObj.SelectedItem).Value.ToString(), string.Empty, string.Empty, strHierachycode, Sel_Assoc_CAMS_List, ((ListItem)cmbSPGoals.SelectedItem).Text.ToString(), ((ListItem)cmbSPObj.SelectedItem).Text.ToString(),string.Empty,Get_Sele_Assoc_SerPlan());
            Assn_form.FormClosed += new FormClosedEventHandler(Refresh_On_Assoc_Save);
            Assn_form.StartPosition = FormStartPosition.CenterScreen;
            Assn_form.ShowDialog();
        }

        private void Refresh_On_Assoc_Save(object sender, FormClosedEventArgs e)
        {
            SSBGTrigAssns_Form form = sender as SSBGTrigAssns_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                SSBGSerPlans = _model.CaseSumData.Browse_SSBGSerplan(ID, string.Empty, string.Empty, string.Empty);
                SSBGCAMS_List = _model.CaseSumData.Browse_SSBGAssns(ID, string.Empty, string.Empty, string.Empty, string.Empty);
                if (strType == "SP")
                    Fill_Assoc_Grid();
                else if (strType == "CAMS")
                    FillCAMSGrid();
            }
        }

        private void Pb_Edit_Assoc_Click(object sender, EventArgs e)
        {
            SSBGTrigAssns_Form Assn_form = new SSBGTrigAssns_Form(BaseForm, Privileges, Consts.LiheAllDetails.strMainType, "Edit", ID, ((ListItem)cmbSPGoals.SelectedItem).Value.ToString(), ((ListItem)cmbSPObj.SelectedItem).Value.ToString(), gvSerPlan.CurrentRow.Cells["SP_Code"].Value.ToString().Trim(), gvSerPlan.CurrentRow.Cells["SP_Seq"].Value.ToString().Trim(), strHierachycode, Sel_Assoc_CAMS_List, ((ListItem)cmbSPGoals.SelectedItem).Text.ToString(), ((ListItem)cmbSPObj.SelectedItem).Text.ToString(), string.Empty,Get_Sele_Assoc_SerPlan());
            Assn_form.FormClosed += new FormClosedEventHandler(Refresh_On_Assoc_Save);
            Assn_form.StartPosition = FormStartPosition.CenterScreen;
            Assn_form.ShowDialog();
        }

        private void Pb_Delete_Assoc_Click(object sender, EventArgs e)
        {
            string Curr_Trig_Seq = gvSerPlan.CurrentRow.Cells["SP_Code"].Value.ToString().Trim() + " With Seq " + gvSerPlan.CurrentRow.Cells["SP_Seq"].Value.ToString().Trim();
            int Trig_CAMS_Cnt = int.Parse(gvSerPlan.CurrentRow.Cells["CAMS_CNT"].Value.ToString().Trim());
            if (Trig_CAMS_Cnt == 0)
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "Service Plan Association: " + Curr_Trig_Seq, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: OnDelete_Assoc_MessageBoxClicked);
            else
                AlertBox.Show("You cannot delete Associations with Service/Outcome", MessageBoxIcon.Warning);//CAMS
        }

         string Trigger_Mode = "", Sql_SP_Result_Message = string.Empty;
        private void OnDelete_Assoc_MessageBoxClicked(DialogResult dialogResult)
        {
            //**MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
                string Trig_ID = gvSerPlan.CurrentRow.Cells["SP_Code"].Value.ToString().Trim();
                string Curr_Seq = gvSerPlan.CurrentRow.Cells["SP_Seq"].Value.ToString().Trim();
                string Curr_SP = gvSerPlan.CurrentRow.Cells["SP_Code"].Value.ToString().Trim();
                //int Trig_CAMS_Cnt = int.Parse(Trig_Assoc_Grid.CurrentRow.Cells["CAMS_CNT"].Value.ToString().Trim());

                if (_model.CaseSumData.UpdateSSBGSerPlan("D", ID,((ListItem)cmbSPGoals.SelectedItem).Value.ToString(),((ListItem)cmbSPObj.SelectedItem).Value.ToString(),Curr_SP, Curr_Seq,"", null, out Sql_SP_Result_Message)) // Rao 555
                {
                    Fill_Assoc_Grid();
                }
                else
                    AlertBox.Show("Service Plan not deleted ", MessageBoxIcon.Warning);
            }
        }

        private void Pb_CAMS_Add_Click(object sender, EventArgs e)
        {
            SSBGTrigAssns_Form CAMS_form = new SSBGTrigAssns_Form(BaseForm, Privileges, Consts.LiheAllDetails.strSubType, "Add", ID, ((ListItem)cmbGoals.SelectedItem).Value.ToString(), ((ListItem)cmbObjective.SelectedItem).Value.ToString(), ((ListItem)cmbSerPlan.SelectedItem).Value.ToString(), ((ListItem)cmbSerPlan.SelectedItem).ID.ToString(), strHierachycode, Get_Sele_Assoc_CAMS(), ((ListItem)cmbGoals.SelectedItem).Text.ToString(), ((ListItem)cmbObjective.SelectedItem).Text.ToString(), ((ListItem)cmbSerPlan.SelectedItem).Text.ToString(),Sel_Assoc_SerPlan_List);
            CAMS_form.FormClosed += new FormClosedEventHandler(Refresh_On_Assoc_Save);
            CAMS_form.StartPosition = FormStartPosition.CenterScreen;
            CAMS_form.ShowDialog();
        }

        private void Pb_CAMS_Edit_Click(object sender, EventArgs e)
        {
            SSBGTrigAssns_Form CAMS_form = new SSBGTrigAssns_Form(BaseForm, Privileges, Consts.LiheAllDetails.strSubType, "Edit", ID, ((ListItem)cmbGoals.SelectedItem).Value.ToString(), ((ListItem)cmbObjective.SelectedItem).Value.ToString(), ((ListItem)cmbSerPlan.SelectedItem).Value.ToString(), ((ListItem)cmbSerPlan.SelectedItem).ID.ToString(), strHierachycode, Get_Sele_Assoc_CAMS(), ((ListItem)cmbGoals.SelectedItem).Text.ToString(), ((ListItem)cmbObjective.SelectedItem).Text.ToString(), ((ListItem)cmbSerPlan.SelectedItem).Text.ToString(), Sel_Assoc_SerPlan_List);
            CAMS_form.FormClosed += new FormClosedEventHandler(Refresh_On_Assoc_Save);
            CAMS_form.StartPosition = FormStartPosition.CenterScreen;
            CAMS_form.ShowDialog();
        }

        private void Pb_CAMS_Delete_Click(object sender, EventArgs e)
        {
            if (gvwCAMS.Rows.Count > 0)
            {
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected CA/MS Record  ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: OnDelete_CAMS_MessageBoxClicked);

                //if (gvwCAMS.CurrentRow.Cells["SP_CAMS_Type"].Value.ToString().Trim() == "CA")
                //{
                //    TRIGSummaryEntity SearchEntity = new TRIGSummaryEntity();
                //    SearchEntity.Trig_Code = ID;
                //    SearchEntity.Trig_Date = gvSummary.CurrentRow.Cells["RanDate"].Value.ToString();
                //    //SearchEntity.Trig_Date_Seq = gvSummary.CurrentRow.Cells["Seq"].Value.ToString();

                //    List<TRIGDetailEntity> DetailList = _model.SPAdminData.Browse_TrigDetails(SearchEntity, "A");

                //    if (DetailList.Count > 0)
                //    {
                //        DetailList = DetailList.FindAll(u => u.Code.Trim().Equals(Trig_CAMS_Grid.CurrentRow.Cells["SP_CAMS_Code"].Value.ToString().Trim()) && u.SP_Code.Equals(Trig_Assoc_Grid.CurrentRow.Cells["Trig_SP"].Value.ToString().Trim()) && u.SPM_SEQ.Equals(Trig_Assoc_Grid.CurrentRow.Cells["Trig_Seq"].Value.ToString().Trim()) && u.Branch.Equals(Trig_CAMS_Grid.CurrentRow.Cells["Trig_SP_Branch"].Value.ToString().Trim()));
                //       if (DetailList.Count > 0)
                //           MessageBox.Show("Can't deleted");
                //       else
                //           MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected CA/MS Record  ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, OnDelete_CAMS_MessageBoxClicked, true);
                //    }
                //    else 
                //        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected CA/MS Record  ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, OnDelete_CAMS_MessageBoxClicked, true);
                //}
                //if (Trig_CAMS_Grid.CurrentRow.Cells["SP_CAMS_Type"].Value.ToString().Trim() == "MS")
                //{
                //    TRIGSummaryEntity SearchEntity = new TRIGSummaryEntity();
                //    SearchEntity.Trig_Code = gvTriggers.CurrentRow.Cells["Trig_Code"].Value.ToString().Trim();
                //    //SearchEntity.Trig_Date = gvSummary.CurrentRow.Cells["RanDate"].Value.ToString();
                //    //SearchEntity.Trig_Date_Seq = gvSummary.CurrentRow.Cells["Seq"].Value.ToString();

                //    List<TRIGDetailEntity> DetailList = _model.SPAdminData.Browse_TrigDetails(SearchEntity, "M");

                //    if (DetailList.Count > 0)
                //    {
                //        DetailList = DetailList.FindAll(u => u.Code.Trim().Equals(Trig_CAMS_Grid.CurrentRow.Cells["SP_CAMS_Code"].Value.ToString().Trim()) && u.SP_Code.Equals(Trig_Assoc_Grid.CurrentRow.Cells["Trig_SP"].Value.ToString().Trim()) && u.SPM_SEQ.Equals(Trig_Assoc_Grid.CurrentRow.Cells["Trig_Seq"].Value.ToString().Trim()) && u.Branch.Equals(Trig_CAMS_Grid.CurrentRow.Cells["Trig_SP_Branch"].Value.ToString().Trim()));
                //        if (DetailList.Count > 0)
                //            MessageBox.Show("Can't deleted");
                //        else
                //            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected CA/MS Record  ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, OnDelete_CAMS_MessageBoxClicked, true);
                //    }
                //    else
                //        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected CA/MS Record  ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, OnDelete_CAMS_MessageBoxClicked, true);
                //}


                //if (Trig_CAMS_Cnt == 0)
                //MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected CA/MS Record  ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, OnDelete_CAMS_MessageBoxClicked, true);
                //else
                //    MessageBox.Show("You can not Delete Associations with CAMS", "CAP Systems", MessageBoxButtons.OK);


            }
        }


        private void OnDelete_CAMS_MessageBoxClicked(DialogResult dialogResult)
        {
            //**MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult/*messageBoxWindow.DialogResult*/ == DialogResult.Yes)
            {
                //string Curr_Trig_ID = gvTriggers.CurrentRow.Cells["Trig_Code"].Value.ToString().Trim();
                //string CT_Trigger_Name = gvTriggers.CurrentRow.Cells["Trig_Desc"].Value.ToString().Trim();
                //string Curr_SP = Trig_Assoc_Grid.CurrentRow.Cells["Trig_SP"].Value.ToString().Trim();
                //string Curr_Assoc_Name = Trig_Assoc_Grid.CurrentRow.Cells["Trig_Assoc_Name"].Value.ToString().Trim();
                //string Trig_SP_Seq = Trig_Assoc_Grid.CurrentRow.Cells["Trig_Seq"].Value.ToString().Trim();

                string Branch = gvwCAMS.CurrentRow.Cells["Trig_SP_Branch"].Value.ToString().Trim();
                string CAMS_TYpe = gvwCAMS.CurrentRow.Cells["SP_CAMS_Type"].Value.ToString().Trim();
                string CAMS_Code = gvwCAMS.CurrentRow.Cells["SP_CAMS_Code"].Value.ToString().Trim();
                string Org_Grp = gvwCAMS.CurrentRow.Cells["SP_CAMS_Ogr_Grp"].Value.ToString().Trim();

                if (_model.CaseSumData.UpdateSSBGAssns("D", ID, ((ListItem)cmbGoals.SelectedItem).Value.ToString(), ((ListItem)cmbObjective.SelectedItem).Value.ToString(), ((ListItem)cmbSerPlan.SelectedItem).Value.ToString(), ((ListItem)cmbSerPlan.SelectedItem).ID.ToString(), Branch, Org_Grp, CAMS_TYpe, CAMS_Code, "", Privileges.UserID, out Sql_SP_Result_Message))
                {
                    //CTTRIGCRITEntity SP_Header_Rec = new CTTRIGCRITEntity();
                    //SP_Header_Rec.TRIGCRITCode = Curr_Trig_ID; SP_Header_Rec.TRIGCRITGroupCode = "TH";
                    //SP_Header_Rec.TRIGCRITGroupDesc = CT_Trigger_Name; SP_Header_Rec.Type = "Trigger";
                    //SP_Header_Rec.TRIGCRITValidated = "N"; SP_Header_Rec.LstcOperator = Privileges.UserID; SP_Header_Rec.Mode = "Edit"; SP_Header_Rec.TRIGCRITGroupSeq = "0";
                    //string strMsg = string.Empty;
                    //if (_model.LiheAllData.InsertUpdateDelCTTRIGCRIT(SP_Header_Rec, out strMsg))
                    //{
                    //    //MessageBox.Show("Service Plan-" + SP_Code + " is Validated And Updated", "CAP Systems"); // Lisa Asked to Comment on 01072013
                    //    RefreshGrid(Curr_Trig_ID);
                    //    BtnValidate.Visible = true;
                    //}
                    FillCAMSGrid();
                    //RefreshSPs(Curr_Trig_ID);
                }
                else
                    AlertBox.Show("Service/Outcome is not deleted", MessageBoxIcon.Warning);
            }
        }

        private void cmbSPGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSPGoals.Items.Count > 0)
            {
                fillObjectiveCombo(Object);
            }

        }
        string Object = string.Empty;
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                strType = "SP";
                //string Goal = ((ListItem)cmbSPGoals.SelectedItem).Value.ToString();
                //Object = ((ListItem)cmbSPObj.SelectedItem).Value.ToString(); 
                fillGoalcombo(string.Empty);
                //fillGroupsgrid();
                //Group_Controls_Fill();
                Mode = string.Empty;

            }
            else if (tabControl1.SelectedIndex == 1)
            {
                strType = "CAMS";
                //pnlQuestions.Visible = true;
                //picAddQues.Enabled = true;
                //string Goal = ((ListItem)cmbGoals.SelectedItem).Value.ToString();
                //Object = ((ListItem)cmbObjective.SelectedItem).Value.ToString();
                fillGoalcombo(string.Empty);
                //FillComboType();
                Mode = string.Empty;
                //txtQues_Code.Enabled = false;
                //Questions_Controls_Fill();
            }
        }

        private void cmbSPObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSPObj.Items.Count > 0)
            {
                if (((ListItem)cmbSPObj.SelectedItem).ID.ToString() == "S" || ((ListItem)cmbSPObj.SelectedItem).ID.ToString() == "C" || ((ListItem)cmbSPObj.SelectedItem).ValueDisplayCode.ToString() == "S" || ((ListItem)cmbSPObj.SelectedItem).ValueDisplayCode.ToString() == "C")
                    this.CAMS.Hide();
                else this.CAMS.Show();
                //if (caseEligGroups[0].SBGGCountType.ToString() == "S" || caseEligGroups[0].SBGGCountType.ToString() == "C" || caseEligGroups[0].Type2.ToString() == "S" || caseEligGroups[0].Type2.ToString() == "C") CountType = "S"; }
                Fill_Assoc_Grid();
            }
        }

        private void cmbGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGoals.Items.Count > 0)
            {
                fillObjectiveCombo(Object);
            }
            else
            {
                gvwCAMS.Rows.Clear();
            }
        }

        private void cmbObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbObjective.Items.Count > 0)
            {
                fillcomboSP();
            }
            else
            {
                gvwCAMS.Rows.Clear();
            }
        }

        private void cmbSerPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSerPlan.Items.Count > 0)
            {
                FillCAMSGrid();
            }
            else
            {
                gvwCAMS.Rows.Clear();
            }
        }




    }
}