#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MembersGridForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion


        public MembersGridForm(BaseForm baseform, string hierarchy,string year, string CAMS_desc, CASEACTEntity pass_entity, PrivilegeEntity privileges, List<CASEACTEntity> CA_template_list,string CA_Benefit, List<CAOBOEntity> CA_OBO_List)
        {
            InitializeComponent();
            BaseForm = baseform;
            Hierarchy = hierarchy;
            Pass_CA_Entity = pass_entity;
            Privileges = privileges;
            CA_Template_List = CA_template_list;
            CAOBF = CA_Benefit;
            CAMS_Desc = CAMS_desc;
            Sel_CA_OBO = CA_OBO_List;
            Year = year;

            this.Text = "Benefiting From";

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            if (CAMS_desc.Length > 60)
                this.Text = CAMS_desc.Substring(0, 60);
            else
                this.Text = CAMS_desc.Trim();

            cmb_CA_Benefit.Enabled = false;

            if (Sel_CA_OBO == null)
                Sel_CA_OBO = new List<CAOBOEntity>();

            lblBenefit.Text = "Service Recipient";

            Fill_CA_Benefiting_From();
            Fill_CA_Members_DropDown();
            Set_Members_CA_Grid_As_Benefit_Change(false, CAOBF);


        }

        public MembersGridForm(BaseForm baseform, string hierarchy, string year, string CAMS_desc,string Code,string branch,string group, CASEACTEntity pass_entity, PrivilegeEntity privileges, List<CASEACTEntity> CA_template_list, string CA_Benefit, List<CAOBOEntity> CA_OBO_List,string Type)
        {
            InitializeComponent();
            BaseForm = baseform;
            Hierarchy = hierarchy;
            Pass_CA_Entity = pass_entity;
            Privileges = privileges;
            CA_Template_List = CA_template_list;
            CAOBF = CA_Benefit;
            CAMS_Desc = CAMS_desc;
            Sel_CA_OBO = CA_OBO_List;
            Year = year;
            CAMSCode = Code; Branch = branch;Group = group; CodeType = Type;

            this.Text = "Benefiting From";

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            if (CAMS_desc.Length > 60)
                this.Text = CAMS_desc.Substring(0, 60);
            else
                this.Text = CAMS_desc.Trim();

            cmb_CA_Benefit.Enabled = true;

            if(CodeType == "CA")
                lblBenefit.Text = "Service Recipient";
            else
                lblBenefit.Text = "Outcome Recipient";

            Fill_CA_Benefiting_From();
            Fill_CA_Members_DropDown();
            Set_Members_CA_Grid_As_Benefit_Change(false, CAOBF);


        }

        #region properties

        public BaseForm BaseForm { get; set; }
        public CASEACTEntity Pass_CA_Entity { get; set; }
        public string Hierarchy { get; set; }
        public List<CASEACTEntity> CA_Template_List { get; set; }
        public string CAOBF { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string CAMS_Desc { get; set; }
        List<CAOBOEntity> Sel_CA_OBO { get; set; }
        public string Year { get; set; }
        public string CAMSCode { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        public string CodeType { get; set; }

        #endregion

        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        private void Fill_CA_Benefiting_From()
        {
            //this.cmb_CA_Benefit.SelectedIndexChanged -= new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);

            cmb_CA_Benefit.Items.Clear();
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Applicant", "1"));
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("All Household Members", "2"));
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Selected Household Members", "3"));


            //this.cmb_CA_Benefit.SelectedIndexChanged += new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);
            if(CAOBF=="1") cmb_CA_Benefit.SelectedIndex = 0; else if (CAOBF == "2") cmb_CA_Benefit.SelectedIndex = 1; else if (CAOBF == "3") cmb_CA_Benefit.SelectedIndex = 2;
            //if (Pass_CA_Entity.Rec_Type == "I")
            //    cmb_CA_Benefit.SelectedIndex = 1;
            

        }

        private void Fill_CA_Members_DropDown()
        {
            CA_Members_Grid.Rows.Clear();
            DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "All", null, null, Pass_CA_Entity.App_no, null, null, null, null, null, null, null, null, null, null, Hierarchy + Year, null, BaseForm.UserID, string.Empty, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<CommonEntity> Relation;
                    Relation = _model.lookupDataAccess.GetRelationship();

                    int rowIndex = 0;
                    string Name = null, TmpSsn = null, Relation_Desc = null; string dob = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Name = TmpSsn = Relation_Desc = dob = null;

                        Name = dr["Fname"].ToString().Trim() + " " + dr["MName"].ToString() + " " + dr["Lname"].ToString().Trim();
                        TmpSsn = dr["Ssn"].ToString();
                        if (!string.IsNullOrEmpty(TmpSsn))
                            TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);

                        dob = dr["DOB"].ToString();
                        if (!string.IsNullOrEmpty(dob))
                            dob = LookupDataAccess.Getdate(dob).ToString();

                        foreach (CommonEntity Relationship in Relation)
                        {
                            if (Relationship.Code.Equals(dr["Mem_Code"].ToString().Trim()))
                            {
                                Relation_Desc = Relationship.Desc; break;
                            }
                        }



                        rowIndex = CA_Members_Grid.Rows.Add(false, Name, dob, Relation_Desc,  dr["RecFamSeq"].ToString(), dr["ClientID"].ToString(), dr["AppNo"].ToString().Substring(10, 1), "N", dr["AppStatus"].ToString(), dr["SNP_EXCLUDE"].ToString());

                        if (dr["AppStatus"].ToString() != "A")
                            CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        if (dr["SNP_EXCLUDE"].ToString() != "N")
                            CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;


                        if (dr["AppNo"].ToString().Substring(10, 1) == "A")
                        {
                            if (dr["AppStatus"].ToString() != "A")
                                CA_Members_Grid.Rows[rowIndex].Cells["CA_Mem_Name"].Style.ForeColor = Color.Blue;
                            else
                                CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        //Members_Grid.Rows[rowIndex].Tag = dr;


                    }
                }
            }
            //Get_CAOBO_Data();
            if(CA_Members_Grid.Rows.Count>0)
                CA_Members_Grid.Rows[0].Selected = true;
        }

        List<CAOBOEntity> CAOBO_List = new List<CAOBOEntity>();
        CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
        private void Get_CAOBO_Data()
        {

            Search_CAOBO_Entity.ID = Pass_CA_Entity.ACT_ID;
            if (CA_Template_List.Count > 0 && Pass_CA_Entity.Rec_Type == "I")
                Search_CAOBO_Entity.ID = CA_Template_List[0].ACT_ID;

            Search_CAOBO_Entity.Seq = Search_CAOBO_Entity.CLID = Search_CAOBO_Entity.Fam_Seq = null;

            CAOBO_List = _model.SPAdminData.Browse_CAOBO(Search_CAOBO_Entity, "Browse");
        }

        private void Set_Members_CA_Grid_As_Benefit_Change(bool Set_Mem_On_Combo, string OBF_Type)
        {
            if (CA_Members_Grid.Rows.Count > 0)
            {
                string Mem_Status = "M";
                this.CA_Sel.ReadOnly = true;
                switch (OBF_Type)
                {
                    case "1": Mem_Status = "A"; break;
                    case "2": Mem_Status = "M"; break;
                    case "3": Mem_Status = "Y"; this.CA_Sel.ReadOnly = false; break;
                }


                if (Set_Mem_On_Combo)//(Pass_MS_Entity.Rec_Type == "I" && !Set_Mem_From_OBO)
                {
                    int Row_index = 0;
                    foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
                    {
                        switch (Mem_Status)
                        {
                            case "A":
                                if (dr.Cells["CA_AppSw"].Value.ToString() == Mem_Status)
                                {
                                    if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
                                        dr.Cells["CA_Sel"].Value = true;
                                    // Members_Grid.Rows[Row_index].DefaultCellStyle.ForeColor = Color.Blue;
                                }
                                else
                                    dr.Cells["CA_Sel"].Value = false;
                                break;
                            case "M":
                                if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
                                    dr.Cells["CA_Sel"].Value = true;
                                break;
                            default:
                                //if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
                                //    dr.Cells["CA_Sel"].Value = true;
                                dr.Cells["CA_Sel"].Value = false;
                                break;
                        }
                        Row_index++;
                    }
                }
                else
                    Set_Members_FromCAOBO();
            }

        }

        private void Set_Members_FromCAOBO()
        {
            //if (Sel_CA_OBO.Count > 1)
            //{
            //    foreach (CAOBOEntity Entity in Sel_CA_OBO)
            //    {
            //        foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
            //        {
            //            if (Entity.CLID == dr.Cells["CA_CLID"].Value.ToString() &&
            //                Entity.Fam_Seq == dr.Cells["CA_Mem_Seq"].Value.ToString())
            //            {
            //                dr.Cells["CA_Sel"].Value = true;
            //                dr.Cells["Is_CAOBO_Rec"].Value = "Y";
            //                break;
            //            }
            //            //else
            //            //{
            //            //    dr.Cells["MS_Sel"].Value = false;
            //            //    dr.Cells["Is_OBO_Rec"].Value = "N";
            //            //    break;
            //            //}
            //        }
            //    }
            //}
            if (Sel_CA_OBO.Count > 0 )
            {
                foreach (CAOBOEntity Entity in Sel_CA_OBO)
                {
                    foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
                    {
                        if (Entity.CLID == dr.Cells["CA_CLID"].Value.ToString() &&
                            Entity.Fam_Seq == dr.Cells["CA_Mem_Seq"].Value.ToString())
                        {
                            dr.Cells["CA_Sel"].Value = true;
                            dr.Cells["Is_CAOBO_Rec"].Value = "Y";
                            break;
                        }
                    }
                }
            }
            else
            {
                string Mem_Status = "M";
                this.CA_Sel.ReadOnly = true;

                switch (CAOBF)
                {
                    case "1": Mem_Status = "A"; break;
                    case "2": Mem_Status = "M"; break;
                    case "3": Mem_Status = "Y"; this.CA_Sel.ReadOnly = false; break;
                }

                int Row_index = 0;
                foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
                {
                    switch (Mem_Status)
                    {
                        case "A":
                            if (dr.Cells["CA_AppSw"].Value.ToString() == Mem_Status)
                            {
                                if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
                                    dr.Cells["CA_Sel"].Value = true;
                                // Members_Grid.Rows[Row_index].DefaultCellStyle.ForeColor = Color.Blue;
                            }
                            else
                                dr.Cells["CA_Sel"].Value = false;
                            break;
                        case "M":
                            if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
                                dr.Cells["CA_Sel"].Value = true;
                            break;
                        default:
                            //if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
                            //    dr.Cells["CA_Sel"].Value = true;
                            dr.Cells["CA_Sel"].Value = false;
                            break;
                    }
                    Row_index++;
                }
            }
        }

        public List<CAOBOEntity> GetMemberRecords()
        {
            List<CAOBOEntity> MembersData = new List<CAOBOEntity>();

            if(CA_Members_Grid.Rows.Count>0)
            {
                foreach(DataGridViewRow dr in CA_Members_Grid.Rows)
                {
                    CAOBOEntity Entity = new CAOBOEntity();
                    if(dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
                    {
                        Entity.ID = Pass_CA_Entity.ACT_ID;
                        Entity.CLID = dr.Cells["CA_CLID"].Value.ToString();
                        Entity.Fam_Seq = dr.Cells["CA_Mem_Seq"].Value.ToString();

                        MembersData.Add(new CAOBOEntity(Entity));
                    }
                }
            }

            return MembersData;
        }


        public List<CAOBOEntity> GetRecordsForMembers()
        {
            List<CAOBOEntity> MembersData = new List<CAOBOEntity>();

            if (CA_Members_Grid.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
                {
                    CAOBOEntity Entity = new CAOBOEntity();
                    if (dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
                    {
                        Entity.ID = Pass_CA_Entity.ACT_ID;
                        Entity.CLID = dr.Cells["CA_CLID"].Value.ToString();
                        Entity.Fam_Seq = dr.Cells["CA_Mem_Seq"].Value.ToString();
                        Entity.Type = CodeType;
                        Entity.Code = CAMSCode;
                        Entity.Branch = Branch;
                        Entity.Group = Group;
                        Entity.BenefitFrom = ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();

                        MembersData.Add(new CAOBOEntity(Entity));
                    }
                }
            }

            return MembersData;
        }

        private void Btn_MS_Save_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool IsValidate()
        {
            bool isValid = true;
            bool AtLeast_One_Mem_Selected = true;
            //if (lblBenefitReq.Visible && ((ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString() == "3")
            //if (((ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString() == "3")
            {
                AtLeast_One_Mem_Selected = false;
                foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
                {
                    if (dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
                    {
                        AtLeast_One_Mem_Selected = true;
                        break;
                    }
                }
            }

            if (AtLeast_One_Mem_Selected)
                _errorProvider.SetError(cmb_CA_Benefit, null);
            else
            {
                _errorProvider.SetError(cmb_CA_Benefit, "Select Atleast One Household Member".Replace(Consts.Common.Colon, string.Empty));
                isValid = false;
            }

            return (isValid);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CA_Members_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CA_Members_Grid.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    if (CA_Members_Grid.CurrentRow.Cells["CA_Active_Sw"].Value.ToString() != "A")
                    {
                        CA_Members_Grid.CurrentRow.Cells["CA_Sel"].Value = false;
                        MessageBox.Show("Member is Inactive", "CAP Systems");
                        return;
                    }

                    if (CA_Members_Grid.CurrentRow.Cells["CA_Exclude_Sw"].Value.ToString() == "Y")
                    {
                        CA_Members_Grid.CurrentRow.Cells["CA_Sel"].Value = false;
                        MessageBox.Show("Member is Excluded", "CAP Systems");
                    }
                }
            }
        }

        private void cmb_CA_Benefit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Text.ToString()))
                Set_Members_CA_Grid_As_Benefit_Change(true, ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString());
        }
    }
}