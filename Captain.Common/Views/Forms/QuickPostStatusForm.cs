using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using DevExpress.CodeParser;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.OpenXmlFormats;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using Wisej.Web;
//using static Spire.Pdf.General.Render.Decode.Jpeg2000.j2k.codestream.HeaderInfo;
using DevExpress.DataAccess.Wizard;
using Captain.Common.Utilities;
using log4net.Repository.Hierarchy;
using DevExpress.XtraReports.UI;
using System.Linq;

namespace Captain.Common.Views.Forms
{
    public partial class QuickPostStatusForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public QuickPostStatusForm(BaseForm baseForm, PrivilegeEntity privileges, List<CASEACTEntity> CA_template_list, List<CASEMSEntity> MS_template_list,CASEACTEntity CaseactEntity,List<CAOBOEntity> oboEntity, string hierarchy)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Privileges=privileges;
            CA_Template_List=CA_template_list;
            MS_Template_List = MS_template_list;
            Pass_CA_Entity=CaseactEntity;
            OBOEntity = oboEntity;
            Hierarchy = hierarchy;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.Icon = null;


            if (CA_Template_List.Count>0 || MS_Template_List.Count>0)
            {
                FillGrid();
            }

        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public List<CASEACTEntity> CA_Template_List { get; set; }

        public List<CASEMSEntity> MS_Template_List { get; set; }

        public CASEACTEntity Pass_CA_Entity { get; set; }

        public List<CAOBOEntity> OBOEntity { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Hierarchy { get; set; }

        public string Mode { get; set; }

        public int StrRowIndex { get; set; }

        #endregion


        private void FillGrid()
        {
            if (CA_Template_List.Count > 0 || MS_Template_List.Count > 0)
            {
                this.gvServices.SelectionChanged -= new System.EventHandler(this.gvServices_SelectionChanged);
                gvServices.Rows.Clear();

                this.gvServices.SelectionChanged += new System.EventHandler(this.gvServices_SelectionChanged);

                if (CA_Template_List.Count > 0)
                {
                    foreach(CASEACTEntity entity in CA_Template_List)
                    {
                        string Status = string.Empty;string Fund = string.Empty,Amount=string.Empty;
                        if (entity.IsSave == "Y") { Status = "Posted"; Fund = entity.Fund1;Amount = entity.Cost; } else Status = "Not Yet Posted";

                        int rowIndex = gvServices.Rows.Add(false,entity.ACT_Code, entity.CADesc.Trim(),Fund,Amount, Status,entity.IsTemplate);

                        if (entity.IsSave == "Y")
                        {
                            gvServices.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;

                            gvServices.Rows[rowIndex].Cells["Sel"].ReadOnly=true;
                        }

                    }
                }

                if (MS_Template_List.Count > 0)
                {
                    foreach (CASEMSEntity entity in MS_Template_List)
                    {
                        string Status = string.Empty; string Fund = string.Empty, Amount = string.Empty;
                        if (entity.IsSave == "Y") { Status = "Posted"; Fund = entity.MS_Fund1;  } else Status = "Not Yet Posted";

                        int rowIndex = gvServices.Rows.Add(false,entity.MS_Code, entity.MSDesc.Trim(),Fund,Amount, Status, entity.IsTemplate);
                    }
                }

                if(gvServices.Rows.Count>0)
                {
                    if(Mode=="Edit")
                        gvServices.CurrentCell = gvServices.Rows[StrRowIndex].Cells[2];
                    else
                        gvServices.CurrentCell = gvServices.Rows[0].Cells[2];
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        int Sel_Count = 0;
        private void gvServices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvServices.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = gvServices.CurrentCell.ColumnIndex;
                int RowIdx = gvServices.CurrentCell.RowIndex;

                switch (e.ColumnIndex)
                {
                    case 0:
                        string Tmp = "false";
                        Tmp = gvServices.SelectedRows[0].Cells["Sel"].Value.ToString();

                        if (Tmp == "True")
                        {

                            gvServices.SelectedRows[0].Cells["Sel"].Value = true;
                            Sel_Count++;
                            pnlSave.Visible = true; pnlBtns.Enabled = false; //btnOk.Visible = false; 
                        }
                        else if (gvServices.SelectedRows[0].DefaultCellStyle.ForeColor!=Color.Green)
                        {
                            Sel_Count--;
                            pnlSave.Visible = false; pnlBtns.Enabled = true; //btnOk.Visible = true;
                        }

                        //if (Sel_Count > 0) { pnlSave.Visible = true; btnOk.Visible = false; } else { pnlSave.Visible = false; btnOk.Visible = true; }

                            break;
                }
            }
        }
        string Sql_SP_Result_Message = string.Empty; string Current_CA_Seq = "1";
        private void btnSave_Click(object sender, EventArgs e)
        {
            Mode = "";
            if(gvServices.Rows.Count>0)
            {
                foreach(DataGridViewRow dr in gvServices.Rows)
                {
                    if (dr.Cells["Sel"].Value.ToString()=="True")
                    {
                        //this.DialogResult = DialogResult.Yes;
                        IsSaveFunction(DialogResult.Yes);
                        break;
                        //MessageBox.Show("Do you wnat Proceed with existing Service " + Pass_CA_Entity.CADesc.Trim(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: IsSaveFunction);
                        //break;
                        ////MessageBox.Show("Do you want Proceed this with existing Service " + Pass_CA_Entity.CADesc.Trim(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: IsSaveFunction);
                        
                    }
                }

                
            }
        }

        private void IsSaveFunction(DialogResult dialogResult)
        {
            if(dialogResult==DialogResult.Yes)
            {
                if (gvServices.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in gvServices.Rows)
                    {
                        if (dr.Cells["Sel"].Value.ToString() == "True")
                        {
                            Pass_CA_Entity = CA_Template_List.Find(u => u.IsTemplate == "T");
                            if (Pass_CA_Entity != null)
                            {
                                CASEACTEntity Entity = CA_Template_List.Find(u => u.ACT_Code.Trim() == dr.Cells["Code"].Value.ToString().Trim());
                                if (Entity != null)
                                {
                                    Entity.Rec_Type = "I";

                                    //Entity.Branch = Pass_CA_Entity.Branch;
                                    //Entity.Group = Pass_CA_Entity.Group;
                                    ////Type = Entity.Type;
                                    ////Entity.ACT_Code = Pass_CA_Entity.ACT_Code;
                                    Entity.ACT_ID = "1";
                                    Entity.ACT_Date = Pass_CA_Entity.ACT_Date;
                                    Entity.ACT_Seq = Pass_CA_Entity.ACT_Seq;
                                    Entity.Site = Pass_CA_Entity.Site;
                                    Entity.Fund1 = Pass_CA_Entity.Fund1;
                                    Entity.Fund2 = Pass_CA_Entity.Fund2;
                                    Entity.Fund3 = Pass_CA_Entity.Fund3;
                                    Entity.Caseworker = Pass_CA_Entity.Caseworker;
                                    Entity.Vendor_No = Pass_CA_Entity.Vendor_No;
                                    Entity.Check_Date = Pass_CA_Entity.Check_Date;
                                    Entity.Check_No = Pass_CA_Entity.Check_No;
                                    Entity.Cost = Pass_CA_Entity.Cost;
                                    Entity.Followup_On = Pass_CA_Entity.Followup_On;
                                    Entity.Followup_Comp = Pass_CA_Entity.Followup_Comp;
                                    Entity.Followup_By = Pass_CA_Entity.Followup_By;
                                    Entity.Refer_Data = Pass_CA_Entity.Refer_Data;
                                    Entity.Cust_Code1 = Pass_CA_Entity.Cust_Code1;
                                    Entity.Cust_Value1 = Pass_CA_Entity.Cust_Value1;
                                    Entity.Cust_Code2 = Pass_CA_Entity.Cust_Code2;
                                    Entity.Cust_Value2 = Pass_CA_Entity.Cust_Value2;
                                    Entity.Cust_Code3 = Pass_CA_Entity.Cust_Code3;
                                    Entity.Cust_Value3 = Pass_CA_Entity.Cust_Value3;
                                    Entity.Cust_Code4 = Pass_CA_Entity.Cust_Code4;
                                    Entity.Cust_Value4 = Pass_CA_Entity.Cust_Value4;
                                    Entity.Cust_Code5 = Pass_CA_Entity.Cust_Code5;
                                    Entity.Cust_Value5 = Pass_CA_Entity.Cust_Value5;
                                    Entity.Bulk = Pass_CA_Entity.Bulk;
                                    Entity.Act_PROG = Pass_CA_Entity.Act_PROG;
                                    Entity.Lstc_Date = Pass_CA_Entity.Lstc_Date;
                                    Entity.Lsct_Operator = Pass_CA_Entity.Lsct_Operator;
                                    Entity.Add_Date = Pass_CA_Entity.Add_Date;
                                    Entity.Add_Operator = Pass_CA_Entity.Add_Operator;
                                    Entity.Notes_Count = Pass_CA_Entity.Notes_Count;
                                    Entity.UOM = Pass_CA_Entity.UOM;
                                    Entity.Units = Pass_CA_Entity.Units;
                                    Entity.VOUCHNO = Pass_CA_Entity.VOUCHNO;
                                    Entity.Curr_Grp = Pass_CA_Entity.Curr_Grp;
                                    Entity.ActSeek_Date = Pass_CA_Entity.ActSeek_Date;
                                    Entity.CA_OBF = Pass_CA_Entity.CA_OBF;
                                    //Entity.ACT_TrigCode = Pass_CA_Entity.ACT_TrigCode;
                                    //Entity.ACT_TrigDate = Pass_CA_Entity.ACT_TrigDate;
                                    //Entity.ACT_TrigDateSeq = Pass_CA_Entity.ACT_TrigDateSeq;

                                    Entity.Rate = Pass_CA_Entity.Rate;
                                    Entity.Amount = Pass_CA_Entity.Amount;
                                    Entity.Amount2 = Pass_CA_Entity.Amount2;
                                    Entity.Amount3 = Pass_CA_Entity.Amount3;
                                    Entity.UOM2 = Pass_CA_Entity.UOM2;
                                    Entity.Units2 = Pass_CA_Entity.Units2;
                                    Entity.UOM3 = Pass_CA_Entity.UOM3;
                                    Entity.Units3 = Pass_CA_Entity.Units3;


                                    Entity.BillngType = Pass_CA_Entity.BillngType;
                                    Entity.BillngFname = Pass_CA_Entity.BillngFname;
                                    Entity.BillngLname = Pass_CA_Entity.BillngLname;
                                    Entity.PaymentNo = Pass_CA_Entity.PaymentNo;

                                    Entity.BillingPeriod = Pass_CA_Entity.BillingPeriod;
                                    Entity.Account = Pass_CA_Entity.Account;
                                    Entity.ArrearsAmt = Pass_CA_Entity.ArrearsAmt;
                                    Entity.LVL1Apprval = Pass_CA_Entity.LVL1Apprval;
                                    Entity.LVL1AprrvalDate = Pass_CA_Entity.LVL1AprrvalDate;
                                    Entity.LVL2Apprval = Pass_CA_Entity.LVL2Apprval;
                                    Entity.LVL2ApprvalDate = Pass_CA_Entity.LVL2ApprvalDate;
                                    Entity.SentPmtUser = Pass_CA_Entity.SentPmtUser;
                                    Entity.SentPmtDate = Pass_CA_Entity.SentPmtDate;
                                    Entity.BundleNo = Pass_CA_Entity.BundleNo;

                                    Entity.CA_Source = Pass_CA_Entity.CA_Source;
                                    Entity.Elec_Other = Pass_CA_Entity.Elec_Other;

                                    Entity.BDC_ID = Pass_CA_Entity.BDC_ID;
                                    Entity.BenefitReason = Pass_CA_Entity.BenefitReason;


                                    string Operatipn_Mode = "Insert";

                                    if (Pass_CA_Entity.Rec_Type == "U")
                                        Operatipn_Mode = "Update";

                                    int New_CAID = 1, New_CA_Seq = 1;
                                    if (_model.SPAdminData.UpdateCASEACT2(Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                                    {
                                        Entity.ACT_ID = New_CAID.ToString();
                                        Entity.ACT_Seq = Current_CA_Seq = New_CA_Seq.ToString();

                                        if (OBOEntity.Count > 0)
                                        {
                                            foreach (CAOBOEntity OEntity in OBOEntity)
                                            {
                                                OEntity.ID = Entity.ACT_ID;
                                                OEntity.Seq = "1";

                                                _model.SPAdminData.UpdateCAOBO(OEntity, "Insert", out Sql_SP_Result_Message);
                                            }
                                        }

                                    }

                                    Entity.IsSave = "Y";
                                }

                            }
                        }
                    }

                    if (CA_Template_List.Count > 0)
                    {
                        CASEACTEntity ActEntity = CA_Template_List.Find(u => u.IsSave != "Y");
                        if (ActEntity == null)
                        {

                            if (Pass_CA_Entity.Rec_Type == "I")
                            {
                                if (BaseForm.BaseAgencyControlDetails.ProgressNotesSwitch.ToUpper() == "Y")
                                    MessageBox.Show("Posting Successful \n Do you want to add Progress Notes?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_PROGNotes_For_CAMS);
                                else
                                {
                                    AlertBox.Show("Posting Successful");//MessageBox.Show("Activity Posting Successful", "CAP Systems");

                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                            }
                            else
                            {
                                AlertBox.Show("Posting Updated Successfully");//MessageBox.Show("Activity Posting Updated Successfully", "CAP Systems");


                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        else
                        {
                            FillGrid();
                            pnlSave.Visible = false; pnlBtns.Enabled = true; //btnOk.Visible = true;
                        }
                    }
                }
             }
        }

        private void Add_PROGNotes_For_CAMS(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                switch ("CA")
                {
                    case "CA": CASE4006_CAMSForm_ToolClick(); break;        //new ToolClickEventArgs(Tools["pbNotes"])
                        //case "MS": Pb_Notes_Click(Pb_MS_Notes, EventArgs.Empty); break;
                }
                //Get_PROG_Notes_Status();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void CASE4006_CAMSForm_ToolClick()
        {
            
            //else if (e.Tool.Name == "tlCaseNotes")
            //{
                string Notes_Field_Name = null;

                //if (CAMS_FLG == "CA")
                    Notes_Field_Name = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan.Trim() + Pass_CA_Entity.SPM_Seq + Pass_CA_Entity.Branch.Trim() +
                            Pass_CA_Entity.Group.ToString() + "CA" + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq + Pass_CA_Entity.ACT_ID;
                //else
                //    Notes_Field_Name = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no + Pass_MS_Entity.Service_plan.Trim() + Pass_MS_Entity.SPM_Seq + Pass_MS_Entity.Branch.Trim() +
                //            Pass_MS_Entity.Group.ToString() + "MS" + Pass_MS_Entity.MS_Code.Trim() + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Act_Date.Value.ToShortDateString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                List<string> list = new List<string>();
                List<CommonEntity> SelEntity = new List<CommonEntity>();
            //if (CAMS_FLG == "CA")
            //{

            //if (Mode == "Add")
            //{
            if (CA_Template_List.Count > 0)
            {
                foreach (CASEACTEntity Entity in CA_Template_List)
                {
                    if (Entity.IsSave == "Y")
                    {
                        string Notes = string.Empty;
                        Notes = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan.Trim() + Pass_CA_Entity.SPM_Seq + Entity.Branch.ToString() +
                                        Entity.Group.ToString() + "CA" + Entity.ACT_Code.ToString().Trim() + Entity.ACT_Seq.ToString() + Entity.ACT_ID.ToString();

                        list.Add(Notes);

                        //string CAMS_Desc = string.Empty;
                        //CAMS_Desc = SP_CAMS_Details.Find(u => u.Type1 == "CA" && u.CamCd.Trim() == Entity.ACT_Code.Trim()).CAMS_Desc.Trim();

                        SelEntity.Add(new CommonEntity(Entity.ACT_Code.ToString().Trim(), Entity.CADesc.Trim(), Notes, "CA"));
                    }
                }
            }

                        //if (MS_Template_List.Count > 0)
                        //{
                        //    if (CA_Template_List.Count > 0)
                        //    {
                        //        foreach (CASEMSEntity Entity in MS_Template_List)
                        //        {
                        //            string Notes = string.Empty;
                        //            Notes = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan.Trim() + Pass_CA_Entity.SPM_Seq + Entity.Branch +
                        //                    Entity.Group + "MS" + Entity.MS_Code.Trim() + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Act_Date.Value.ToShortDateString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);


                        //            list.Add(Notes);

                        //            string CAMS_Desc = string.Empty;
                        //            CAMS_Desc = SP_CAMS_Details.Find(u => u.Type1 == "CA" && u.CamCd.Trim() == Entity.MS_Code.Trim()).CAMS_Desc.Trim();

                        //            SelEntity.Add(new CommonEntity(Entity.MS_Code.ToString().Trim(), Entity.MSDesc.Trim(), Notes, "CA"));
                        //        }
                        //    }
                        //}
                    //}
                //}

                
                    ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Add", Privileges, Notes_Field_Name, list, "QuickPost", SelEntity);
                    //Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                    Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                    Prog_Form.ShowDialog();

                this.Close();
                
            //}
            

        }

        public List<CASEACTEntity> GetLatestCAs()
        {
            List<CASEACTEntity> entity = new List<CASEACTEntity>();
            if(CA_Template_List.Count>0)
            {
                //CASEACTEntity ActEntity = CA_Template_List.Find(u => u.IsSave != "Y");
                //if (ActEntity == null)
                //{
                //    entity = ActEntity;
                //}
                entity = CA_Template_List;
            }

            return entity;
        }

        public CASEACTEntity GetSelectedCA()
        {
            CASEACTEntity entity = new CASEACTEntity();
            if (CA_Template_List.Count > 0)
            {

                StrRowIndex = gvServices.CurrentRow.Index;

                CASEACTEntity Entity = CA_Template_List.Find(u => u.ACT_Code.Trim() == gvServices.CurrentRow.Cells["Code"].Value.ToString().Trim());

                if(entity != null)
                {
                    entity = Entity;
                }
                //CASEACTEntity ActEntity = CA_Template_List.Find(u => u.IsSave != "Y");
                //if (ActEntity == null)
                //{
                //    entity = ActEntity;
                //}
                //entity = CA_Template_List;
            }

            return entity;
        }

        private void gvServices_SelectionChanged(object sender, EventArgs e)
        {
            if (gvServices.Rows.Count > 0)
            {
                string Tmp = "false";
                Tmp = gvServices.SelectedRows[0].Cells["Sel"].Value.ToString();

                foreach(DataGridViewRow dr in gvServices.Rows)
                {
                    if (dr.Cells["Sel"].Value.ToString() == "True")
                    {
                        Tmp = "True"; break;
                    }
                }

                if (Tmp == "True")
                {
                    pnlSave.Visible = true; pnlBtns.Enabled = false; //btnOk.Visible = false;
                }
                else
                {
                    pnlSave.Visible = false; pnlBtns.Enabled = true; //btnOk.Visible = true;
                }
            }
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            Mode = "";
            if (gvServices.Rows.Count > 0)
            {
                int rowIndex = gvServices.CurrentRow.Index;
                Pass_CA_Entity = CA_Template_List.Find(u => u.IsTemplate == "T");

                if(Pass_CA_Entity!=null)
                {
                    CASEACTEntity Entity = CA_Template_List.Find(u => u.ACT_Code.Trim() == gvServices.CurrentRow.Cells["Code"].Value.ToString().Trim());

                    if (Entity != null)
                    {
                        if(Entity.IsSave!="Y")
                        {
                            Entity.Rec_Type = "I";

                            //Entity.Branch = Pass_CA_Entity.Branch;
                            //Entity.Group = Pass_CA_Entity.Group;
                            ////Type = Entity.Type;
                            ////Entity.ACT_Code = Pass_CA_Entity.ACT_Code;
                            Entity.ACT_ID = "1";
                            Entity.ACT_Date = Pass_CA_Entity.ACT_Date;
                            Entity.ACT_Seq = Pass_CA_Entity.ACT_Seq;
                            Entity.Site = Pass_CA_Entity.Site;
                            Entity.Fund1 = Pass_CA_Entity.Fund1;
                            Entity.Fund2 = Pass_CA_Entity.Fund2;
                            Entity.Fund3 = Pass_CA_Entity.Fund3;
                            Entity.Caseworker = Pass_CA_Entity.Caseworker;
                            Entity.Vendor_No = Pass_CA_Entity.Vendor_No;
                            Entity.Check_Date = Pass_CA_Entity.Check_Date;
                            Entity.Check_No = Pass_CA_Entity.Check_No;
                            Entity.Cost = Pass_CA_Entity.Cost;
                            Entity.Followup_On = Pass_CA_Entity.Followup_On;
                            Entity.Followup_Comp = Pass_CA_Entity.Followup_Comp;
                            Entity.Followup_By = Pass_CA_Entity.Followup_By;
                            Entity.Refer_Data = Pass_CA_Entity.Refer_Data;
                            Entity.Cust_Code1 = Pass_CA_Entity.Cust_Code1;
                            Entity.Cust_Value1 = Pass_CA_Entity.Cust_Value1;
                            Entity.Cust_Code2 = Pass_CA_Entity.Cust_Code2;
                            Entity.Cust_Value2 = Pass_CA_Entity.Cust_Value2;
                            Entity.Cust_Code3 = Pass_CA_Entity.Cust_Code3;
                            Entity.Cust_Value3 = Pass_CA_Entity.Cust_Value3;
                            Entity.Cust_Code4 = Pass_CA_Entity.Cust_Code4;
                            Entity.Cust_Value4 = Pass_CA_Entity.Cust_Value4;
                            Entity.Cust_Code5 = Pass_CA_Entity.Cust_Code5;
                            Entity.Cust_Value5 = Pass_CA_Entity.Cust_Value5;
                            Entity.Bulk = Pass_CA_Entity.Bulk;
                            Entity.Act_PROG = Pass_CA_Entity.Act_PROG;
                            Entity.Lstc_Date = Pass_CA_Entity.Lstc_Date;
                            Entity.Lsct_Operator = Pass_CA_Entity.Lsct_Operator;
                            Entity.Add_Date = Pass_CA_Entity.Add_Date;
                            Entity.Add_Operator = Pass_CA_Entity.Add_Operator;
                            Entity.Notes_Count = Pass_CA_Entity.Notes_Count;
                            Entity.UOM = Pass_CA_Entity.UOM;
                            Entity.Units = Pass_CA_Entity.Units;
                            Entity.VOUCHNO = Pass_CA_Entity.VOUCHNO;
                            Entity.Curr_Grp = Pass_CA_Entity.Curr_Grp;
                            Entity.ActSeek_Date = Pass_CA_Entity.ActSeek_Date;
                            Entity.CA_OBF = Pass_CA_Entity.CA_OBF;
                            //Entity.ACT_TrigCode = Pass_CA_Entity.ACT_TrigCode;
                            //Entity.ACT_TrigDate = Pass_CA_Entity.ACT_TrigDate;
                            //Entity.ACT_TrigDateSeq = Pass_CA_Entity.ACT_TrigDateSeq;

                            Entity.Rate = Pass_CA_Entity.Rate;
                            Entity.Amount = Pass_CA_Entity.Amount;
                            Entity.Amount2 = Pass_CA_Entity.Amount2;
                            Entity.Amount3 = Pass_CA_Entity.Amount3;
                            Entity.UOM2 = Pass_CA_Entity.UOM2;
                            Entity.Units2 = Pass_CA_Entity.Units2;
                            Entity.UOM3 = Pass_CA_Entity.UOM3;
                            Entity.Units3 = Pass_CA_Entity.Units3;


                            Entity.BillngType = Pass_CA_Entity.BillngType;
                            Entity.BillngFname = Pass_CA_Entity.BillngFname;
                            Entity.BillngLname = Pass_CA_Entity.BillngLname;
                            Entity.PaymentNo = Pass_CA_Entity.PaymentNo;

                            Entity.BillingPeriod = Pass_CA_Entity.BillingPeriod;
                            Entity.Account = Pass_CA_Entity.Account;
                            Entity.ArrearsAmt = Pass_CA_Entity.ArrearsAmt;
                            Entity.LVL1Apprval = Pass_CA_Entity.LVL1Apprval;
                            Entity.LVL1AprrvalDate = Pass_CA_Entity.LVL1AprrvalDate;
                            Entity.LVL2Apprval = Pass_CA_Entity.LVL2Apprval;
                            Entity.LVL2ApprvalDate = Pass_CA_Entity.LVL2ApprvalDate;
                            Entity.SentPmtUser = Pass_CA_Entity.SentPmtUser;
                            Entity.SentPmtDate = Pass_CA_Entity.SentPmtDate;
                            Entity.BundleNo = Pass_CA_Entity.BundleNo;

                            Entity.CA_Source = Pass_CA_Entity.CA_Source;
                            Entity.Elec_Other = Pass_CA_Entity.Elec_Other;

                            Entity.BDC_ID = Pass_CA_Entity.BDC_ID;
                            Entity.BenefitReason = Pass_CA_Entity.BenefitReason;

                            Pass_CA_Entity.Rec_Type = Entity.Rec_Type;
                            string Operatipn_Mode = "Insert";

                            if (Pass_CA_Entity.Rec_Type == "U")
                                Operatipn_Mode = "Update";

                            int New_CAID = 1, New_CA_Seq = 1;
                            if (_model.SPAdminData.UpdateCASEACT2(Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                            {
                                Entity.ACT_ID = New_CAID.ToString();
                                Entity.ACT_Seq = Current_CA_Seq = New_CA_Seq.ToString();

                                if (OBOEntity.Count > 0)
                                {
                                    foreach (CAOBOEntity OEntity in OBOEntity)
                                    {
                                        OEntity.ID = Entity.ACT_ID;
                                        OEntity.Seq = "1";

                                        _model.SPAdminData.UpdateCAOBO(OEntity, "Insert", out Sql_SP_Result_Message);
                                    }
                                }

                            }

                            Entity.IsSave = "Y";
                        }
                        
                    }

                }
                if (CA_Template_List.Count > 0)
                {
                    CASEACTEntity ActEntity = CA_Template_List.Find(u => u.IsSave != "Y");
                    if (ActEntity == null)
                    {

                        if (Pass_CA_Entity.Rec_Type == "I")
                        {
                            if (BaseForm.BaseAgencyControlDetails.ProgressNotesSwitch.ToUpper() == "Y")
                                MessageBox.Show("Posting Successful \n Do you want to add Progress Notes?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_PROGNotes_For_CAMS);
                            else
                            {
                                AlertBox.Show("Posting Successful");//MessageBox.Show("Activity Posting Successful", "CAP Systems");

                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        else
                        {
                            AlertBox.Show("Posting Updated Successfully");//MessageBox.Show("Activity Posting Updated Successfully", "CAP Systems");


                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        FillGrid();
                        pnlSave.Visible = false; pnlBtns.Enabled = true; //btnOk.Visible = true;
                        if(gvServices.Rows.Count > rowIndex+1)
                        {
                            //gvServices.Rows[rowIndex + 1].Tag = 0;
                            gvServices.CurrentCell = gvServices.Rows[rowIndex + 1].Cells[2];
                            
                        }
                        else
                        {
                            //gvServices.Rows[0].Tag = 0;

                            gvServices.CurrentCell = gvServices.Rows[0].Cells[2];
                        }
                    }
                }


            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Mode = "";

            if (CA_Template_List.Count > 0)
            {
                CASEACTEntity ActEntity = CA_Template_List.Find(u => u.IsSave != "Y");
                if (ActEntity != null)
                {
                    if (BaseForm.BaseAgencyControlDetails.ProgressNotesSwitch.ToUpper() == "Y")
                        MessageBox.Show("Posting Successful \n Do you want to add Progress Notes?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_PROGNotes_For_CAMS);
                    else
                    {
                        AlertBox.Show("Posting Successful");//MessageBox.Show("Activity Posting Successful", "CAP Systems");

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            Mode = "Edit";
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
