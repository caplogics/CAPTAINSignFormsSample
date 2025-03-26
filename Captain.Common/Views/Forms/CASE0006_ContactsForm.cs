/************************************************************************
 * Conversion On            :   11/25/2022
 * Converted By             :   Kranthi
 * Latest Modification On   :   11/25/2022
 * **********************************************************************/
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
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0006_ContactsForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public CASE0006_ContactsForm(BaseForm baseForm, string mode, CASECONTEntity Entity, PrivilegeEntity privileges, string hierarchy, string year, string app_no, List<FldcntlHieEntity> fldcntlcontactEntity)
        {
            InitializeComponent();

            _model = new CaptainModel();
            BaseForm = baseForm;
            Pass_Entity = Entity;
            Mode = mode;
            Hierarchy = hierarchy;
            pnlFollowup.Visible = false;
            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().Trim().ToUpper() == "Y")
            {
                pnlFollowup.Visible = true;
            }

            Year = "    ";
            if (!string.IsNullOrEmpty(year))
                Year = year;
            App_No = app_no;

            Mode = mode;

            Privileges = privileges;


            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            RbDuration_CheckedChanged(RbDuration, new EventArgs());
            CntlContactEntity = fldcntlcontactEntity;

            SP_Programs_List = _model.HierarchyAndPrograms.GetCaseHierarchy("PROGRAM", BaseForm.BaseAgency, BaseForm.BaseDept);
            Get_App_MST_Details();

            Fill_CaseWorker();
            Fill_From_AGYTAB();
            Fill_Custom_Questions();
            Fill_Members_DropDown();

            Fill_Agency_Referral_List();

            Get_PROG_Notes_Status();

            //Cont_Date.Text = DateTime.Today.ToShortDateString();
            //dtFollowup.Text = DateTime.Today.ToShortDateString();
            //dtCompleted.Text = DateTime.Today.ToShortDateString();
            

            Cmb_How.SelectedIndexChanged -= new EventHandler(Cmb_How_SelectedIndexChanged);
            if (Mode.Equals("Add"))
            {
                Tools["pbNotes"].Visible = false;
                //Pb_Notes.Visible = false;
                this.Text = privileges.PrivilegeName + " - Add Contact";

                
            }
            else
            {
                this.Text = privileges.PrivilegeName + " - Edit Contact";
                //Cont_Date.Enabled = false;
                Fill_Contact_Controls();
            }


            TxtNoof_Contact.Validator = TextBoxValidation.IntegerValidator;

            ToolTip tooltip = new ToolTip();

            tooltip.SetToolTip(PbReferral2, "Agency Referral Search for 'Bill To'");
           // tooltip.SetToolTip(Hepl, "Help");
            EnableDisableControls();
            Cmb_How.SelectedIndexChanged += new EventHandler(Cmb_How_SelectedIndexChanged);
            //dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = "MM dd yyyy hh mm ss"; 

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string SP_Code { get; set; }

        public CASECONTEntity Pass_Entity { get; set; }

        public List<FldcntlHieEntity> CntlContactEntity { get; set; }

        public List<FldcntlHieEntity> CntlCAEntity { get; set; }

        public List<FldcntlHieEntity> CntlMSEntity { get; set; }

        public string Hierarchy { get; set; }

        ////public string M_HieDesc { get; set; }

        public string Year { get; set; }

        public string App_No { get; set; }

        public string SchSite { get; set; }

        public string SchDate { get; set; }

        public string SchType { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public List<HierarchyEntity> SP_Programs_List { get; set; }


        #endregion

        string Sql_SP_Result_Message = string.Empty;

        private bool Validate_Form()
        {
            bool isValid = true;

            if (!Cont_Date.Checked) //((ListItem)CmbSP.SelectedItem).Value.ToString())
            {
                _errorProvider.SetError(Cont_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblContactDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Cont_Date, null);

            _errorProvider.SetError(label13, null);
            if (RbTime.Checked)
            {
                bool Time_Test = true;
                //if (DT_Dur_From.Checked && !DT_Dur_To.Checked) //((ListItem)CmbSP.SelectedItem).Value.ToString())
                //{
                //    label13.Text = " "; label13.Visible = true;

                //    _errorProvider.SetError(label13, string.Format("'From time' sholud not be greater than 'To Time'"));

                //    //_errorProvider.SetError(label13, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "'From time' sholud not be greater than 'To Time'".Replace(Consts.Common.Colon, string.Empty)));
                //    Time_Test = false;
                //}

                if (DT_Dur_From.Checked && DT_Dur_To.Checked) //((ListItem)CmbSP.SelectedItem).Value.ToString())
                {
                    if (DT_Dur_From.Value > DT_Dur_To.Value)
                    {
                        label13.Text = " "; label13.Visible = true;
                        _errorProvider.SetError(label13, string.Format("'From time' sholud not be greater than 'To Time'"));
                        Time_Test = false;
                    }
                }
                else
                {
                    label13.Text = " "; label13.Visible = true;
                    _errorProvider.SetError(label13, string.Format("'From time' sholud not be greater than 'To Time'"));
                }

                if (!Time_Test)
                    isValid = false;
                else
                    _errorProvider.SetError(label13, null);
            }
            else
            {
                if (!DT_Duration.Checked)
                {
                    label13.Text = " "; label13.Visible = true;
                    _errorProvider.SetError(label13, string.Format("Please provide Duration"));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(label13, null);
            }




            //if ((((ListItem)Cmb_Members.SelectedItem).Value.ToString() != "0") && (((ListItem)Cmb_Members.SelectedItem).DefaultValue.ToString() == "Y"))
            //{
            //    _errorProvider.SetError(Cmb_Members, "Selected Household Member is \n either Inactive or Excluded".Replace(Consts.Common.Colon, string.Empty));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(Cmb_Members, null);

            return isValid;
        }

        List<CASEREFEntity> CASEREF_List = new List<CASEREFEntity>();
        private void Fill_Agency_Referral_List()
        {
            CASEREFEntity Search_Entity = new CASEREFEntity();


            Search_Entity.Code = Search_Entity.Name1 = Search_Entity.Name2 = Search_Entity.Code = Search_Entity.IndexBy = null;
            Search_Entity.Street = Search_Entity.City = Search_Entity.State = Search_Entity.Zip = Search_Entity.Zip_Plus = null;
            Search_Entity.Area = Search_Entity.Excgange = Search_Entity.Telno = Search_Entity.Active = Search_Entity.Cont_Fname = null;
            Search_Entity.Cont_Lname = Search_Entity.Cont_Area = Search_Entity.Cont_Exchange = Search_Entity.Cont_TelNO = Search_Entity.Fax_Area = null;

            Search_Entity.Long_Distance = Search_Entity.Fax_Exchange = Search_Entity.Fax_Telno = Search_Entity.Outside = Search_Entity.Category = null;
            Search_Entity.County = Search_Entity.From_Hrs = Search_Entity.To_Hrs = Search_Entity.Sec = Search_Entity.Lstc_Date = null;
            Search_Entity.Lsct_Operator = Search_Entity.Add_Date = Search_Entity.Add_Operator = null;

            CASEREF_List = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");
        }



        List<CustfldsEntity> Cust;
        private void Fill_Custom_Questions()
        {
            Cust_Grid.Rows.Clear();
            Cust = _model.FieldControls.GetCUSTFLDSByScrCodeContact("CASE0061", "FLDCNTLHIE", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);

            CustRespEntity Search_Entity = new CustRespEntity(true);
            Search_Entity.ScrCode = "CASE0061";
            CustResp_List = _model.FieldControls.Browse_CUSTRESP(Search_Entity, "Browse");
            if (Cust.Count > 0)
            {
                int rowIndex = 0, Tmp_Row_Cnt = 1;
                bool DropDown_Exists = false;
                string Tmp_Cust_Resp = null, Tmp_Cust_Resp_Code = null, Tmp_Cust_Resp_Desc = "";
                foreach (CustfldsEntity Entity in Cust)
                {
                    if (Tmp_Row_Cnt < 4)
                    {
                        Tmp_Cust_Resp = " ";
                        if (Mode.Equals("Edit"))
                        {
                            if (Entity.CustCode.Equals(Pass_Entity.Cust1_Code))
                                Tmp_Cust_Resp = Pass_Entity.Cust1_Value;
                            else
                                if (Entity.CustCode.Equals(Pass_Entity.Cust2_Code))
                                Tmp_Cust_Resp = Pass_Entity.Cust2_Value;
                            else
                                    if (Entity.CustCode.Equals(Pass_Entity.Cust3_Code))
                                Tmp_Cust_Resp = Pass_Entity.Cust3_Value;
                        }

                        Tmp_Cust_Resp_Desc = Tmp_Cust_Resp;
                        if (Entity.RespType == "D")
                        {
                            foreach (CustRespEntity Ent in CustResp_List)
                            {
                                if (Entity.CustCode == Ent.ResoCode && Tmp_Cust_Resp == Ent.DescCode)
                                {
                                    Tmp_Cust_Resp_Desc = Ent.RespDesc;
                                    break;
                                }
                            }
                        }
                        if (Entity.RespType == "C")
                        {


                            string custQuestionResp = string.Empty;
                            List<CustRespEntity> custReponseEntity = CustResp_List.FindAll(u => u.ResoCode == Entity.CustCode);
                            if (custReponseEntity.Count > 0)
                            {
                                string response1 = Tmp_Cust_Resp;
                                if (!string.IsNullOrEmpty(response1))
                                {
                                    string[] arrResponse = null;
                                    if (response1.IndexOf(',') > 0)
                                    {
                                        arrResponse = response1.Split(',');
                                    }
                                    else if (!response1.Equals(string.Empty))
                                    {
                                        arrResponse = new string[] { response1 };
                                    }
                                    foreach (string stringitem in arrResponse)
                                    {

                                        CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
                                        if (custRespEntity != null)
                                        {
                                            custQuestionResp += custRespEntity.RespDesc + ", ";
                                        }
                                    }
                                }
                            }

                            if (custQuestionResp.Length > 1)
                            {
                                custQuestionResp = custQuestionResp.Trim();
                                if ((custQuestionResp.Substring(custQuestionResp.Length - 1)) == ",")
                                {
                                    custQuestionResp = custQuestionResp.Remove(custQuestionResp.Length - 1, 1);
                                }
                            }

                            Tmp_Cust_Resp_Desc = custQuestionResp;
                        }

                        rowIndex = Cust_Grid.Rows.Add((Entity.custReqData.ToString() == "Y" ? "*" : ""), Entity.CustDesc, Tmp_Cust_Resp_Desc, Entity.RespType, Entity.CustCode, Tmp_Cust_Resp);
                        Cust_Grid.Rows[rowIndex].Cells["gvtCustReq"].Style.ForeColor = Color.Red;
                        set_Cust_Grid_Tooltip(rowIndex, Entity.RespType);

                        if (Entity.RespType == "C" || Entity.RespType == "D")
                        {
                            DropDown_Exists = true;
                            Cust_Grid.Rows[rowIndex].Cells["Ques"].Style.ForeColor = Color.Blue;
                            Cust_Grid.Rows[rowIndex].Cells["Resp"].Style.ForeColor = Color.Blue;
                            Cust_Grid.Rows[rowIndex].Cells["Resp"].ReadOnly = true;
                        }
                    }
                    else
                        break;

                    Tmp_Row_Cnt++;
                }
                //set_CustGrid_Rows_On_Questions(DropDown_Exists);
            }
        }



        private void set_Cust_Grid_Tooltip(int rowIndex, string Type)
        {
            string Type_Desc = "Not Defined";
            switch (Type)
            {
                case "C": Type_Desc = "Check Box"; break;
                case "D": Type_Desc = "DropDown"; break;
                case "T": Type_Desc = "Date"; break;
                case "N": Type_Desc = "Numeric"; break;
                case "X": Type_Desc = "Text"; break;

            }

            string toolTipText = "Question Type : " + Type_Desc;

            foreach (DataGridViewCell cell in Cust_Grid.Rows[rowIndex].Cells)
                cell.ToolTipText = toolTipText;
        }


        List<CustRespEntity> CustResp_List = new List<CustRespEntity>();
        private void set_CustGrid_Rows_On_Questions(bool DropDown_Exists)
        {
            CustRespEntity Search_Entity = new CustRespEntity();

            Search_Entity.ScrCode = "CASE0061";
            Search_Entity.RecType = Search_Entity.ResoCode = Search_Entity.RespSeq = null;
            Search_Entity.RespDesc = Search_Entity.DescCode = Search_Entity.AddDate = Search_Entity.AddOpr = null;
            Search_Entity.ChgDate = Search_Entity.ChgOpr = Search_Entity.Changed = null;

            if (DropDown_Exists)
                CustResp_List = _model.FieldControls.Browse_CUSTRESP(Search_Entity, "Browse");

            //CustResp = _model.FieldControls.GetCustRespByScrCustCode("CASE4006");

            //foreach (DataGridViewRow dr in Cust_Grid.Rows)
            //{
            //    if (dr.Cells["Type"].Value == "C" ||
            //        dr.Cells["Type"].Value == "D" ||
            //        dr.Cells["Type"].Value == "C")
            //        dr.Cells["Resp"].ReadOnly = true;
            //    else
            //        dr.Cells["Resp"].ReadOnly = false;
            //}
        }


        private void Fill_Members_DropDown()
        {
            //List<CaseSnpEntity> caseSNPEntity = _model.CaseMstData.GetCaseSnpDetails(Hierarchy.Substring(0, 2), Hierarchy.Substring(2, 2), Hierarchy.Substring(4, 2), Year, App_No);
            List<CaseSnpEntity> caseSNPEntity = _model.CaseMstData.GetCaseSnpDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);

            Cmb_Members.Items.Clear();
            Cmb_Members.ColorMember = "FavoriteColor";

            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0", " ", Color.White, " "));

            foreach (CaseSnpEntity Entity in caseSNPEntity)
                listItem.Add(new ListItem(LookupDataAccess.GetMemberName(Entity.NameixFi.Trim(), Entity.NameixMi, Entity.NameixLast.Trim(), BaseForm.BaseHierarchyCnFormat.ToString()),
                                            Entity.FamilySeq, Entity.Status, (Entity.Status.Equals("I") || Entity.Exclude.Equals("Y")) ? Color.Red : Color.Black, Entity.Exclude));

            //foreach (CaseSnpEntity Entity in caseSNPEntity)
            //    listItem.Add(new ListItem(Entity.NameixFi.Trim() + " " + Entity.NameixMi + " " + Entity.NameixLast.Trim(), Entity.FamilySeq, Entity.Status, (Entity.Status.Equals("I") || Entity.Exclude.Equals("Y")) ? Color.Red : Color.Green, Entity.Exclude));

            Cmb_Members.Items.AddRange(listItem.ToArray());

            Cmb_Members.SelectedIndex = 0;
        }

        private void Hepl_Click(object sender, EventArgs e)
        {
            // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE0006_Contact");
        }


        int New_Cont_Seq = 0;
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (isValidate())
            {
                Pass_Entity.Cont_Date = Cont_Date.Value.ToShortDateString();

                Pass_Entity.Contact_Name = Pass_Entity.Refer_From = Pass_Entity.BillTO = Pass_Entity.Contact_No = Pass_Entity.Time =
                Pass_Entity.Duration_Type = Pass_Entity.Duration = Pass_Entity.Time_Ends = Pass_Entity.Time_Starts = null;

                Pass_Entity.Contact_Name = Txt_Cont_Name.Text.Trim();
                //if (((ListItem)Cmb_Members.SelectedItem).Value.ToString() != "0")
                //{
                //Pass_Entity.Contact_Name = ((ListItem)Cmb_Members.SelectedItem).Text.ToString();
                Pass_Entity.Contact_Name = Txt_Cont_Name.Text.Trim();
                if (Pass_Entity.Rec_Type == "I")
                    Pass_Entity.Seq = "0";
                ////}

                //Pass_Entity.Contact_Name = TxtCont_Name.Text ;
                //Pass_Entity.Refer_From = TxtRefer.Text;
                //Pass_Entity.BillTO = TxtBillTo.Text;


                Pass_Entity.Refer_From = TxtRef_Code1.Text;
                Pass_Entity.BillTO = TxtBillTo_Code.Text;
                Pass_Entity.Contact_No = TxtNoof_Contact.Text;
                if (RbDuration.Checked)
                {
                    Pass_Entity.Duration_Type = "1";

                    if (DT_Duration.Checked)
                        Pass_Entity.Duration = DT_Duration.Value.ToString("HH:mm:ss");
                    //Pass_Entity.Duration = DT_Duration.Text.ToString();
                }
                else if (RbTime.Checked)
                {
                    Pass_Entity.Duration_Type = "2";
                    if (DT_Dur_From.Checked)
                        Pass_Entity.Time_Starts = DT_Dur_From.Value.ToString("HH:mm:ss");
                    //Pass_Entity.Time_Starts = DT_Dur_From.Text.ToString();

                    if (DT_Dur_To.Checked)
                        Pass_Entity.Time_Ends = DT_Dur_To.Value.ToString("HH:mm:ss");
                    //Pass_Entity.Time_Ends = DT_Dur_To.Text.ToString();

                    if (DT_Dur_From.Checked && DT_Dur_To.Checked)
                    {
                        DT_Duration.Text = (DT_Dur_To.Value - DT_Dur_From.Value).ToString();
                        Pass_Entity.Duration = DT_Duration.Value.ToString("HH:mm:ss");
                        //Pass_Entity.Duration = Pass_Entity.Duration.ToString("HH:mm:ss");
                    }
                }


                Pass_Entity.CaseWorker = Pass_Entity.How_Where = Pass_Entity.BillTo_UOM = null;
                Pass_Entity.Language = Pass_Entity.Interpreter = null;

                if (((ListItem)Cmb_Worker.SelectedItem).Value.ToString() != "0")
                    Pass_Entity.CaseWorker = ((ListItem)Cmb_Worker.SelectedItem).Value.ToString().Trim();

                if (((ListItem)Cmb_How.SelectedItem).Value.ToString() != "0")
                    Pass_Entity.How_Where = ((ListItem)Cmb_How.SelectedItem).Value.ToString().Trim();
                if (((ListItem)Cmb_BillUOM.SelectedItem).Value.ToString() != "0")
                    Pass_Entity.BillTo_UOM = ((ListItem)Cmb_BillUOM.SelectedItem).Value.ToString().Trim();
                if (((ListItem)CmbLanguage.SelectedItem).Value.ToString() != "0")
                    Pass_Entity.Language = ((ListItem)CmbLanguage.SelectedItem).Value.ToString().Trim();
                if (((ListItem)CmbInter.SelectedItem).Value.ToString() != "0")
                    Pass_Entity.Interpreter = ((ListItem)CmbInter.SelectedItem).Value.ToString().Trim();

                Pass_Entity.Cont_Program = (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()) ? Txt_CA_Program.Text.Substring(0, 6) : "");

                string Operatipn_Mode = "Iseert";

                if (Mode.Equals("Add"))
                    Pass_Entity.Rec_Type = "I";
                else
                    Operatipn_Mode = "Update";

                //if (Pass_Entity.Duration_Type == "2")
                //{
                //    Pass_Entity.Time_Starts = DT_Dur_From.Text.ToString();
                //    Pass_Entity.Time_Ends = DT_Dur_To.Text.ToString();
                //}


                Pass_Entity.Lsct_Operator = BaseForm.UserID;

                Pass_Entity.Cust1_Code = Pass_Entity.Cust1_Code =
                Pass_Entity.Cust2_Code = Pass_Entity.Cust2_Code =
                Pass_Entity.Cust3_Code = Pass_Entity.Cust3_Code = null;
                Pass_Entity.FollowuponDate = Pass_Entity.FollowupCompleteDate = string.Empty;
                if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().Trim().ToUpper() == "Y")
                {
                    if (dtFollowup.Checked)
                    {
                        Pass_Entity.FollowuponDate = dtFollowup.Value.ToShortDateString();
                    }
                    if (dtCompleted.Checked)
                    {
                        Pass_Entity.FollowupCompleteDate = dtCompleted.Value.ToShortDateString();
                    }
                }
                int Tmp_Cust_Cnt = 1;
                string Curr_Ques_Type = "";
                foreach (DataGridViewRow dr in Cust_Grid.Rows)
                {
                    Curr_Ques_Type = dr.Cells["Type"].Value.ToString();
                    switch (Tmp_Cust_Cnt)
                    {
                        case 1:
                            if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                            {
                                Pass_Entity.Cust1_Code = dr.Cells["Code"].Value.ToString();
                                if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                    Pass_Entity.Cust1_Value = dr.Cells["Resp_Code"].Value.ToString();
                                else
                                    Pass_Entity.Cust1_Value = dr.Cells["Resp"].Value.ToString();
                            }
                            break;
                        case 2:
                            if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                            {
                                Pass_Entity.Cust2_Code = dr.Cells["Code"].Value.ToString();
                                if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                    Pass_Entity.Cust2_Value = dr.Cells["Resp_Code"].Value.ToString();
                                else
                                    Pass_Entity.Cust2_Value = dr.Cells["Resp"].Value.ToString();
                            }
                            break;
                        case 3:
                            if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                            {
                                Pass_Entity.Cust3_Code = dr.Cells["Code"].Value.ToString();
                                if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                    Pass_Entity.Cust3_Value = dr.Cells["Resp_Code"].Value.ToString();
                                else
                                    Pass_Entity.Cust3_Value = dr.Cells["Resp"].Value.ToString();
                            }
                            break;
                    }
                    Tmp_Cust_Cnt++;
                }


                if (_model.SPAdminData.UpdateCASECONT(Pass_Entity, Privileges.Program + "1", Operatipn_Mode, out New_Cont_Seq, out Sql_SP_Result_Message))
                {
                    switch (Pass_Entity.Rec_Type)
                    {
                        case "I":
                            Pass_Entity.Seq = New_Cont_Seq.ToString();
                            Btn_Save.Visible = Btn_Cancel.Visible = false;
                            if (BaseForm.BaseAgencyControlDetails.ProgressNotesSwitch.ToUpper() == "Y")
                            {
                                MessageBox.Show("Contact Inserted Successfully \n Do you want to add Progress Notes?", Consts.Common.ApplicationCaption,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_PROGNotes_For_Contact);
                            }
                            else
                            {
                                AlertBox.Show("Contact details Inserted Successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }

                            break;
                        case "U":
                            AlertBox.Show("Contact details Updated Successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            break;
                    }
                }
                else
                    MessageBox.Show("Exception: " + Sql_SP_Result_Message, "CAP Systems");
            }
        }

        private void Add_PROGNotes_For_Contact(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                //Pb_Notes_Click(Pb_Notes, EventArgs.Empty);
                CASE0006_ContactsForm_ToolClick(this.Tools, new ToolClickEventArgs(Tools["pbNotes"]));
            }
            else
            {
                AlertBox.Show("Contact details Inserted Successfully");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void Fill_CaseWorker()
        {
            //DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Pass_Entity.Agency, Pass_Entity.Dept, Pass_Entity.Program);
            DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Pass_Entity.Agency, "**", "**");
            string strNameFormat = null, strCwFormat = null;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            }

            Cmb_Worker.Items.Clear();
            Cmb_Worker.ColorMember = "FavoriteColor";

            List<ListItem> listItem = new List<ListItem>();

            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, Pass_Entity.Agency, Pass_Entity.Dept, Pass_Entity.Program);
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                        listItem.Add(new ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Black));

                    //(

                    Cmb_Worker.Items.AddRange(listItem.ToArray());
                }
            }
            Cmb_Worker.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
            Cmb_Worker.SelectedIndex = 0;

            //if (Mode.Equals("Add") && (!string.IsNullOrEmpty(App_MST_Entity.IntakeWorker.Trim())))  // Kathy Asked to Get Password CaseWorker as Default on 04252014
            //    SetComboBoxValue(Cmb_Worker, App_MST_Entity.IntakeWorker.Trim());
            //else
            //    Cmb_Worker.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(BaseForm.UserProfile.CaseWorker.Trim()))
                SetComboBoxValue(Cmb_Worker, BaseForm.UserProfile.CaseWorker);
            else
                Cmb_Worker.SelectedIndex = 0;
        }

        private void Fill_From_AGYTAB()
        {
            Cmb_How.Items.Clear();
            Cmb_How.ColorMember = "FavoriteColor";
            List<CommonEntity> commonHowwhere = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CONTACTHOWHERE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            Cmb_How.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
            Cmb_How.ColorMember = "FavoriteColor";
            Cmb_How.SelectedIndex = 0;
            foreach (CommonEntity howwhere in commonHowwhere)
            {
                ListItem li = new ListItem(howwhere.Desc, howwhere.Code, howwhere.Active, howwhere.Active.Equals("Y") ? Color.Black : Color.Red);
                Cmb_How.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && howwhere.Default.Equals("Y")) Cmb_How.SelectedItem = li;

            }



            //List<SPCommonEntity> AgyCommon_List = new List<SPCommonEntity>();
            //AgyCommon_List = _model.SPAdminData.Get_AgyRecs("HowWhere");
            //foreach (SPCommonEntity Entity in AgyCommon_List)
            //    Cmb_How.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y")) ? Color.Green : Color.Red));

            //Cmb_How.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
            //Cmb_How.SelectedIndex = 0;


            Cmb_BillUOM.Items.Clear();
            Cmb_BillUOM.ColorMember = "FavoriteColor";
            //AgyCommon_List.Clear();
            //AgyCommon_List = new List<SPCommonEntity>();
            //AgyCommon_List = _model.SPAdminData.Get_AgyRecs("UOM");
            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.UOMTABLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetJobTitle();
            commonEntity = filterByHIE(commonEntity);
            foreach (CommonEntity Entity in commonEntity)
                Cmb_BillUOM.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y")) ? Color.Black : Color.Red));

            Cmb_BillUOM.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
            Cmb_BillUOM.SelectedIndex = 0;



            CmbLanguage.Items.Clear();
            CmbLanguage.ColorMember = "FavoriteColor";
            List<CommonEntity> LanguagesList = new List<CommonEntity>();
            LanguagesList = _model.lookupDataAccess.GetPrimaryLanguage();
            foreach (CommonEntity Entity in LanguagesList)
                CmbLanguage.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y")) ? Color.Black : Color.Red));

            CmbLanguage.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));

            //CmbLanguage.SelectedIndex = 0;

            if (Mode.Equals("Add") && (!string.IsNullOrEmpty(App_MST_Entity.Language.Trim())))
                SetComboBoxValue(CmbLanguage, App_MST_Entity.Language.Trim());
            else
                CmbLanguage.SelectedIndex = 0;


            AgencyControlEntity AgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");


            CmbInter.Items.Clear();
            CmbInter.Items.Add(new ListItem(" ", "0"));
            if ((BaseForm.BaseAgency == "01" && BaseForm.BaseDept == "01" && BaseForm.BaseProg == "52") && AgencyControlDetails.State == "TX")
            {
                CmbInter.Items.Add(new ListItem("ASL Interpreter", "1"));
                CmbInter.Items.Add(new ListItem("CDI", "2"));
                CmbInter.Items.Add(new ListItem("Tactile Interpreter", "3"));
                CmbInter.Items.Add(new ListItem("Trilingual", "4"));
                CmbInter.Items.Add(new ListItem("None", "5"));
            }
            else
            {
                CmbInter.Items.Add(new ListItem("No", "N"));
                CmbInter.Items.Add(new ListItem("Yes", "Y"));
                CmbInter.Items.Add(new ListItem("Unknown", "U"));
            }
            CmbInter.SelectedIndex = 0;
        }
        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {

            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            //if (LookupValues.Exists(u => u.Hierarchy.Equals(HIE)))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(HIE)).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")).ToList();
            //else
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();

            return LookupValues;
        }

        private string Get_Referral_Desc(string Ref_Code)
        {
            string Ref_Desc = null;
            foreach (CASEREFEntity Entity in CASEREF_List)
            {
                if (Entity.Code == Ref_Code)
                {
                    Ref_Desc = Entity.Name1; break;
                }
            }

            return Ref_Desc;
        }


        private void Fill_Contact_Controls()
        {
            Cont_Date.Value = Convert.ToDateTime(Pass_Entity.Cont_Date);
            TxtRef_Code1.Text = Pass_Entity.Refer_From;

            TxtBillTo.Text = Get_Referral_Desc(Pass_Entity.BillTO);
            TxtBillTo_Code.Text = Pass_Entity.BillTO;

            if ((!string.IsNullOrEmpty(TxtBillTo.Text)))
                Cmb_BillUOM.Enabled = true;

            TxtNoof_Contact.Text = Pass_Entity.Contact_No;
            if (!string.IsNullOrEmpty(Pass_Entity.Duration_Type))
            {
                if (Pass_Entity.Duration_Type == "1")
                {
                    RbDuration.Checked = true;

                    if (!string.IsNullOrEmpty(Pass_Entity.Duration))
                    {
                        DT_Duration.Text = Pass_Entity.Duration;
                        DT_Duration.Checked = true;
                    }
                }
                else if (Pass_Entity.Duration_Type == "2")
                {
                    RbTime.Checked = true;
                    if (!string.IsNullOrEmpty(Pass_Entity.Time_Starts))
                    {
                        DT_Dur_From.Text = Pass_Entity.Time_Starts;
                        DT_Dur_From.Checked = true;
                    }

                    if (!string.IsNullOrEmpty(Pass_Entity.Time_Ends))
                    {
                        DT_Dur_To.Text = Pass_Entity.Time_Ends;
                        DT_Dur_To.Checked = true;
                    }

                    if (!string.IsNullOrEmpty(Pass_Entity.Duration))
                    {
                        DT_Duration.Text = Pass_Entity.Duration;
                        //DT_Duration.Checked = true;
                    }
                }
            }
            this.Cmb_Members.SelectedIndexChanged -= new System.EventHandler(this.Cmb_Members_SelectedIndexChanged);
            SetComboBoxValue(Cmb_Members, Pass_Entity.Contact_Name);
            this.Cmb_Members.SelectedIndexChanged += new System.EventHandler(this.Cmb_Members_SelectedIndexChanged);
            Txt_Cont_Name.Text = Pass_Entity.Contact_Name;

            SetComboBoxValue(Cmb_Worker, Pass_Entity.CaseWorker);
            SetComboBoxValue(Cmb_How, Pass_Entity.How_Where);
            SetComboBoxValue(Cmb_BillUOM, Pass_Entity.BillTo_UOM);
            SetComboBoxValue(CmbLanguage, Pass_Entity.Language);
            SetComboBoxValue(CmbInter, Pass_Entity.Interpreter);
            if (Pass_Entity.Cont_Program.Trim() != string.Empty)
            {
                Txt_CA_Program.Text = Set_SP_Program_Text(Pass_Entity.Cont_Program);
            }

            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().Trim().ToUpper() == "Y")
            {
                if (Pass_Entity.FollowuponDate != string.Empty)
                {
                    dtFollowup.Value = Convert.ToDateTime(Pass_Entity.FollowuponDate);
                    dtFollowup.Checked = true;
                }
                if (Pass_Entity.FollowupCompleteDate != string.Empty)
                {
                    dtCompleted.Value = Convert.ToDateTime(Pass_Entity.FollowupCompleteDate);
                    dtCompleted.Checked = true;
                }
            }
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

        private void Cmb_Members_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Cmb_Members.Items.Count > 0)
                {
                    string strMembers = ((ListItem)Cmb_Members.SelectedItem).Value == null ? string.Empty : ((ListItem)Cmb_Members.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strMembers))
                    {
                        bool Can_Add_Name = true;

                        if (((ListItem)Cmb_Members.SelectedItem).Value.ToString() != "0")
                        {
                            if (((ListItem)Cmb_Members.SelectedItem).ID.ToString() == "I")
                            {
                                Can_Add_Name = false;
                                //MessageBox.Show("Household Member is Inactive and \n cannot be selected as the contact name", "CAP Systems", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                MessageBox.Show("Household Member is Inactive do you want to continue..", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    onclose: Delete_Selected_Contact);
                            }

                            if (((ListItem)Cmb_Members.SelectedItem).DefaultValue.ToString() == "Y")
                            {
                                Can_Add_Name = false;
                                //MessageBox.Show("Household Member is Excluded  and \n cannot be selected as the contact name", "CAP Systems", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                MessageBox.Show("Household Member is Excluded do you want to continue", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    onclose: Delete_Selected_Contact);
                            }

                            if (Can_Add_Name)
                                Txt_Cont_Name.Text = ((ListItem)Cmb_Members.SelectedItem).Text.ToString();
                            //else
                            //    Txt_Cont_Name.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Delete_Selected_Contact(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                Txt_Cont_Name.Text = ((ListItem)Cmb_Members.SelectedItem).Text.ToString();
            }
        }


        bool Referral1_Clicked = true;
        private void Pb_Copy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtBillTo.Text))
                Cmb_BillUOM.Enabled = true;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cont_Date_CheckedChanged(object sender, EventArgs e)
        {
            if (Cont_Date.Checked)
                _errorProvider.SetError(Cont_Date, null);
        }

        private void contextMenu3_Popup(object sender, EventArgs e)
        {
            contextMenu3.MenuItems.Clear();
            if (Cust_Grid.Rows.Count > 0)
            {
                if ((Cust_Grid.CurrentRow.Cells["Type"].Value.ToString() == "D"))
                {
                    List<PopUp_Menu_L1_Entity> listItem = new List<PopUp_Menu_L1_Entity>();

                    foreach (CustRespEntity Entity in CustResp_List)
                    {
                        if (Cust_Grid.CurrentRow.Cells["Code"].Value.ToString() == Entity.ResoCode)
                        {
                            MenuItem Resp_Menu = new MenuItem();
                            Resp_Menu.Text = Entity.RespDesc;
                            Resp_Menu.Tag = Entity.DescCode;
                            contextMenu3.MenuItems.Add(Resp_Menu);

                            if (Cust_Grid.CurrentRow.Cells["Resp"].Value.ToString() == Entity.RespDesc)
                                Resp_Menu.Checked = true;
                        }
                    }
                    MenuItem Resp_Menu1 = new MenuItem();
                    Resp_Menu1.Text = "Clear Response";
                    Resp_Menu1.Tag = "CLRRSP";
                    contextMenu3.MenuItems.Add(Resp_Menu1);
                }
                else if (Cust_Grid.CurrentRow.Cells["Type"].Value.ToString().Equals("C"))
                {
                    string strfieldCode = Cust_Grid.CurrentRow.Cells["Code"].Value != null ? Cust_Grid.CurrentRow.Cells["Code"].Value.ToString() : string.Empty;
                    string response = Cust_Grid.CurrentRow.Cells["Resp_Code"].Value != null ? Cust_Grid.CurrentRow.Cells["Resp_Code"].Value.ToString() : string.Empty;
                    PrivilegeEntity privileges = new PrivilegeEntity();
                    privileges.AddPriv = "true";
                    AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, strfieldCode);
                    objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                    objform.StartPosition = FormStartPosition.CenterScreen;
                    objform.ShowDialog();
                }
            }
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Cust_Grid.CurrentRow.Cells["Resp_Code"].Value = form.propAlertCode;

                string custQuestionResp = string.Empty;
                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE0061", form.propFieldCode);
                if (custReponseEntity.Count > 0)
                {
                    string response1 = form.propAlertCode;
                    if (!string.IsNullOrEmpty(response1))
                    {
                        string[] arrResponse = null;
                        if (response1.IndexOf(',') > 0)
                        {
                            arrResponse = response1.Split(',');
                        }
                        else if (!response1.Equals(string.Empty))
                        {
                            arrResponse = new string[] { response1 };
                        }
                        foreach (string stringitem in arrResponse)
                        {

                            CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
                            if (custRespEntity != null)
                            {
                                custQuestionResp += custRespEntity.RespDesc + ", ";
                            }
                        }
                    }
                }

                if (custQuestionResp.Length > 1)
                {
                    custQuestionResp = custQuestionResp.Trim();
                    if ((custQuestionResp.Substring(custQuestionResp.Length - 1)) == ",")
                    {
                        custQuestionResp = custQuestionResp.Remove(custQuestionResp.Length - 1, 1);
                    }
                }
                Cust_Grid.CurrentRow.Cells["Resp"].Value = custQuestionResp;
            }
        }


        private void Cust_Grid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is string)
            {
                if (objArgs.MenuItem.Tag != "CLRRSP")
                {
                    Cust_Grid.CurrentRow.Cells["Resp"].Value = objArgs.MenuItem.Text;
                    Cust_Grid.CurrentRow.Cells["Resp_Code"].Value = objArgs.MenuItem.Tag;
                }
                else
                    Cust_Grid.CurrentRow.Cells["Resp_Code"].Value = Cust_Grid.CurrentRow.Cells["Resp"].Value = " ";
            }
        }

        private void RbDuration_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == RbDuration)
            {
                if (RbDuration.Checked)
                {
                    DT_Dur_From.Checked = DT_Dur_To.Checked =
                    DT_Dur_From.Enabled = DT_Dur_To.Enabled = false;
                }
            }
            else
            {
                if (RbTime.Checked)
                {
                    DT_Duration.Text = "00:00";
                    DT_Duration.Checked = DT_Duration.Enabled = false;
                }
            }
            if (CntlContactEntity != null)
            {

                foreach (FldcntlHieEntity entity in CntlContactEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE0006.StartTime:
                            if (RbTime.Checked)
                            {
                                DT_Duration.Text = "00:00";
                                DT_Duration.Checked = DT_Duration.Enabled = false;
                                if (enabled) { DT_Dur_From.Enabled = true; } else { DT_Dur_From.Enabled = false; }
                            }
                            break;
                        case Consts.CASE0006.EndTime:
                            if (RbTime.Checked)
                            {
                                if (enabled) { DT_Dur_To.Enabled = true; } else { DT_Dur_To.Enabled = false; }
                            }
                            break;
                        case Consts.CASE0006.Duration:
                            if (RbDuration.Checked)
                            {
                                DT_Dur_From.Checked = DT_Dur_To.Checked = DT_Dur_From.Enabled = DT_Dur_To.Enabled = false;
                                if (enabled) { DT_Duration.Enabled = true; } else { RbDuration.Enabled = false; }
                            }
                            break;

                    }

                }
            }


        }

        private void On_PROGNOTES_Closed(object sender, FormClosedEventArgs e)
        {
            ProgressNotes_Form form = sender as ProgressNotes_Form;
            this.DialogResult = DialogResult.OK;
            if (form.DialogResult == DialogResult.OK)
            {
                Get_PROG_Notes_Status();

                this.DialogResult = DialogResult.OK;
                //this.Close();
            }
        }



        private void Get_PROG_Notes_Status()
        {
            List<CaseNotesEntity> caseNotesEntity = new List<CaseNotesEntity>();

            //caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, Hierarchy + Year + App_No + "CONT" + Pass_Entity.Seq);
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program + "1", Hierarchy + Year + App_No + "0000".Substring(0, (4 - Pass_Entity.Seq.Length)) + Pass_Entity.Seq);
            Tools["pbNotes"].ImageSource = Consts.Icons.ico_CaseNotes_New;

            if (caseNotesEntity.Count > 0)
            {
                if (Pass_Entity.Rec_Type == "I")
                    Tools["pbNotes"].Visible = true;

                Tools["pbNotes"].ImageSource = Consts.Icons.ico_CaseNotes_View;
            }
        }

        CaseMstEntity App_MST_Entity = new CaseMstEntity();
        private void Get_App_MST_Details()
        {
            //App_MST_Entity = _model.CaseMstData.GetCaseMST(Hierarchy.Substring(0, 2), Hierarchy.Substring(2, 2), Hierarchy.Substring(4, 2), Year, App_No);
            App_MST_Entity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
        }

        public string GetSelected_Contact_Code()
        {
            string Added_Edited_ContCode = string.Empty;

            Added_Edited_ContCode = New_Cont_Seq.ToString();

            return Added_Edited_ContCode;
        }

        private void Cust_Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal number;
            DateTime Compare_Date;

            switch (Cust_Grid.CurrentRow.Cells["Type"].Value.ToString())
            {
                case "N":
                    if ((!(Decimal.TryParse(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out number))) &&
                    !(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())))
                    {
                        MessageBox.Show("Please Enter Decimal Response", "CAP Systems", MessageBoxButtons.OK);
                        Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    break;

                case "X":
                case "A":
                    if ((string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Trim())))
                    {
                        MessageBox.Show("Please Provide Valid Data", "CAP Systems", MessageBoxButtons.OK);
                        Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    break;

                case "T":
                    if (!(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString())))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), Consts.StaticVars.DateFormatMMDDYYYY))
                        {
                            try
                            {
                                if (DateTime.Parse(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < Convert.ToDateTime("01/01/1800"))
                                {
                                    MessageBox.Show("01/01/1800 below date not except", "CAP Systems");
                                    Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                                }
                                else
                                    MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, "CAP Systems");
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Please Enter Valid Date Format MM/DD/YYYY", "CAP Systems");
                                Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                            }
                        }
                    }
                    break;
            }
        }

        private void EnableDisableControls()
        {


            if (!CntlContactEntity.Exists(u => u.Enab.Equals("Y")))
            {
                //MessageBox.Show("Field controls not defined for this program");
                Btn_Save.Enabled = false;
            }

            foreach (FldcntlHieEntity entity in CntlContactEntity)
            {
                bool required = entity.Req.Equals("Y") ? true : false;
                bool enabled = entity.Enab.Equals("Y") ? true : false;

                switch (entity.FldCode)
                {
                    case Consts.CASE0006.ContactName:
                        if (enabled) { lblContactName.Enabled = Cmb_Members.Enabled = Txt_Cont_Name.Enabled = true; if (required) lblReqContactName.Visible = true; } else { lblContactName.Enabled = Cmb_Members.Enabled = Txt_Cont_Name.Enabled = false; lblReqContactName.Visible = false; }
                        break;
                    case Consts.CASE0006.ContCaseWorker:
                        if (enabled) { lblCaseWorker.Enabled = Cmb_Worker.Enabled = true; if (required) lblReqCaseWorker.Visible = true; } else { lblCaseWorker.Enabled = Cmb_Worker.Enabled = false; lblReqCaseWorker.Visible = false; }
                        break;
                    case Consts.CASE0006.ofContacts:
                        if (enabled) { lblOfContacts.Enabled = TxtNoof_Contact.Enabled = true; if (required) lblReqOfContacts.Visible = true; } else { lblOfContacts.Enabled = TxtNoof_Contact.Enabled = false; lblReqOfContacts.Visible = false; }
                        break;
                    case Consts.CASE0006.DurationType:
                        if (enabled) { RbDuration.Enabled = RbTime.Enabled = true; if (required) lblDurationTypeReq.Visible = true; } else { RbDuration.Enabled = RbTime.Enabled = lblDurationTypeReq.Visible = false; }
                        break;
                    case Consts.CASE0006.StartTime:
                        if (enabled) { if (required) lblReqForm.Visible = true; } else { lblReqForm.Visible = false; }
                        break;
                    case Consts.CASE0006.EndTime:
                        if (enabled) { if (required) lblReqTo.Visible = true; } else { lblReqTo.Visible = false; }
                        break;
                    case Consts.CASE0006.Duration:
                        if (enabled) { if (required) lblReqDuration.Visible = true; } else { lblReqDuration.Visible = false; }
                        break;
                    case Consts.CASE0006.HowWhere:
                        if (enabled) { Cmb_How.Enabled = lblHowWhere.Enabled = true; if (required) lblReqHowWhere.Visible = true; } else { Cmb_How.Enabled = lblHowWhere.Enabled = false; lblReqHowWhere.Visible = false; }
                        break;
                    case Consts.CASE0006.LanguageContactSpeaks:
                        if (enabled) { CmbLanguage.Enabled = lblLanguageSpeaks.Enabled = true; if (required) lblReqLanguageSpeaks.Visible = true; } else { CmbLanguage.Enabled = lblLanguageSpeaks.Enabled = false; lblReqLanguageSpeaks.Visible = false; }
                        break;
                    case Consts.CASE0006.InterpreterNeed:
                        if (enabled) { CmbInter.Enabled = lblInterpreterNeeded.Enabled = true; if (required) lblReqInterpreterNeeded.Visible = true; } else { CmbInter.Enabled = lblInterpreterNeeded.Enabled = false; lblReqInterpreterNeeded.Visible = false; }
                        break;
                    //case Consts.CASE0006.ReferredFrom:
                    //    if (enabled) { TxtRefer.Enabled = lblReferedForm.Enabled = true; if (required) lblReqReferedForm.Visible = true; } else { TxtRefer.Enabled = lblReferedForm.Enabled = false; lblReqReferedForm.Visible = false; }
                    //    break;
                    case Consts.CASE0006.BillTo:
                        if (enabled) { TxtBillTo.Enabled = lblBillTo.Enabled = panel_Referral2.Visible = true; if (required) lblReqBillTo.Visible = true; } else { TxtBillTo.Enabled = lblBillTo.Enabled = panel_Referral2.Visible = false; lblReqBillTo.Visible = false; }
                        break;
                    case Consts.CASE0006.BillUnit:
                        if (enabled) { Cmb_BillUOM.Enabled = lblBillUnit.Enabled = true; if (required) lblReqBillUnit.Visible = true; } else { Cmb_BillUOM.Enabled = lblBillUnit.Enabled = false; lblReqBillUnit.Visible = false; }
                        break;
                    case Consts.CASE0006.ContProgram:
                        if (enabled) { Pb_CA_Prog.Visible = LblProgram.Enabled = true; if (required) LblProgramReq.Visible = true; } else { Pb_CA_Prog.Visible = Pb_CA_Prog.Visible = LblProgram.Enabled = false; LblProgramReq.Visible = false; }
                        break;
                        //case Consts.CASE0006.ContactDate:
                        //    if (enabled) { Cont_Date.Enabled = lblContactDate.Enabled = true; if (required) lblReqContactDate.Visible = true; } else { Cont_Date.Enabled = lblContactDate.Enabled = false; lblReqContactDate.Visible = false; }
                        //    break;
                        //case Consts.CASE2001.InitialDate:
                        //    if (enabled) { dtpInitialDate.Enabled = lblInitialDate.Enabled = true; if (required) lblInitialDateReq.Visible = true; } else { dtpInitialDate.Enabled = lblInitialDate.Enabled = false; lblInitialDateReq.Visible = false; }
                        //    break;

                }



            }
        }

        private bool isValidate()
        {
            bool isValid = true;

            _errorProvider.SetError(label13, null);
            _errorProvider.SetError(Cust_Grid, null);
            if (lblReqContactName.Visible && String.IsNullOrEmpty(Txt_Cont_Name.Text))
            {
                _errorProvider.SetError(Cmb_Members, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblContactName.Text));
                
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(Cmb_Members, null);
            }
            if (LblProgramReq.Visible && String.IsNullOrEmpty(Txt_CA_Program.Text))
            {
                _errorProvider.SetError(Txt_CA_Program, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblProgram.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(Txt_CA_Program, null);
            }



            if (lblReqCaseWorker.Visible && (Cmb_Worker.SelectedItem == null || (string.IsNullOrEmpty(((ListItem)Cmb_Worker.SelectedItem).Text.Trim()))))
            {
                _errorProvider.SetError(Cmb_Worker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseWorker.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(Cmb_Worker, null);
            }


            if (lblReqOfContacts.Visible && string.IsNullOrEmpty(TxtNoof_Contact.Text.Trim()))
            {
                _errorProvider.SetError(TxtNoof_Contact, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOfContacts.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(TxtNoof_Contact, null);
            }

            if (lblReqOfContacts.Visible && !string.IsNullOrEmpty(TxtNoof_Contact.Text.Trim()))
            {
                if (int.Parse(TxtNoof_Contact.Text) <= 0)
                {
                    _errorProvider.SetError(TxtNoof_Contact, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOfContacts.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(TxtNoof_Contact, null);
            }

            if (lblReqContactDate.Visible && Cont_Date.Checked == false)
            {
                _errorProvider.SetError(Cont_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblContactDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(Cont_Date, null);
            }
            if (lblReqBillTo.Visible && string.IsNullOrEmpty(TxtBillTo.Text.Trim()))
            {
                _errorProvider.SetError(TxtBillTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBillTo.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(TxtBillTo, null);
            }
            if (lblReqBillUnit.Visible && (Cmb_BillUOM.SelectedItem == null || (string.IsNullOrEmpty(((ListItem)Cmb_BillUOM.SelectedItem).Text.Trim()))))
            {
                _errorProvider.SetError(Cmb_BillUOM, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBillUnit.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(Cmb_BillUOM, null);
            }


            if (lblReqInterpreterNeeded.Visible && (CmbInter.SelectedItem == null || (string.IsNullOrEmpty(((ListItem)CmbInter.SelectedItem).Text.Trim()))))
            {
                _errorProvider.SetError(CmbInter, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblInterpreterNeeded.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(CmbInter, null);
            }
            if (lblReqLanguageSpeaks.Visible && (CmbLanguage.SelectedItem == null || (string.IsNullOrEmpty(((ListItem)CmbLanguage.SelectedItem).Text.Trim()))))
            {
                _errorProvider.SetError(CmbLanguage, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLanguageSpeaks.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(CmbLanguage, null);
            }
            if (lblReqHowWhere.Visible && (Cmb_How.SelectedItem == null || (string.IsNullOrEmpty(((ListItem)Cmb_How.SelectedItem).Text.Trim()))))
            {
                _errorProvider.SetError(Cmb_How, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHowWhere.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(Cmb_How, null);
            }

            _errorProvider.SetError(label13, null);
            _errorProvider.SetError(DT_Duration, null);
            _errorProvider.SetError(lblDurationTypeReq, null);
            if (lblReqDuration.Visible)
            {
                if (RbDuration.Checked)
                {
                    string strdruation = DT_Duration.Value.ToString("HH:mm:ss");
                    if (lblReqDuration.Visible)
                    {
                        if (strdruation == "00:00:00")
                        {
                            _errorProvider.SetError(DT_Duration, string.Format("Please provide Duration Time Greater Than Zero"));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(DT_Duration, null);
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(DT_Duration, null);
                    }

                }
                else
                {
                    _errorProvider.SetError(DT_Duration, string.Format("Please select Duration"));
                    isValid = false;
                }
            }
            if (RbDuration.Checked == false && RbTime.Checked == false)
            {
                if (lblDurationTypeReq.Visible == true)
                {
                    _errorProvider.SetError(lblDurationTypeReq, "Please select Duration or Time");
                    isValid = false;
                }
            }

            bool Time_Test = true;
            if (lblReqForm.Visible || lblReqTo.Visible)
            {

                if (RbTime.Checked)
                {
                    if (DT_Dur_From.Checked && DT_Dur_To.Checked)
                    {
                        if (DT_Dur_From.Value > DT_Dur_To.Value)
                        {
                            label13.Text = " "; label13.Visible = true;
                            _errorProvider.SetError(label13, string.Format("'From time' sholud not be greater than 'To Time'"));
                            Time_Test = false;
                        }
                        else
                        {
                            string strtime = string.Empty;


                            TimeSpan timspandiff = (DT_Dur_To.Value - DT_Dur_From.Value);
                            strtime = timspandiff.ToString();
                            if (strtime == "00:00:00")
                            {
                                label13.Text = " "; label13.Visible = true;
                                _errorProvider.SetError(label13, string.Format("Please Provoide From and To Time Different greater than zero"));
                                Time_Test = false;
                            }
                            //Pass_Entity.Duration = Pass_Entity.Duration.ToString("HH:mm:ss");

                        }
                    }
                    else
                    {
                        label13.Text = " "; label13.Visible = true;
                        _errorProvider.SetError(label13, string.Format("Please Provoide From and To Time"));
                        Time_Test = false;
                    }
                }
                else
                {
                    label13.Text = " "; label13.Visible = true;
                    _errorProvider.SetError(label13, string.Format("Please Provoide From and To Time"));
                    Time_Test = false;
                }
                if (!Time_Test)
                    isValid = false;
                else
                    _errorProvider.SetError(label13, null);
            }
            else
            {
                if (RbTime.Checked)
                {
                    if (DT_Dur_From.Checked && DT_Dur_To.Checked) //((ListItem)CmbSP.SelectedItem).Value.ToString())
                    {
                        if (DT_Dur_From.Value > DT_Dur_To.Value)
                        {
                            label13.Text = " "; label13.Visible = true;
                            _errorProvider.SetError(label13, string.Format("'From time' sholud not be greater than 'To Time'"));
                            Time_Test = false;
                        }
                    }
                    else
                    {
                        label13.Text = " "; label13.Visible = true;
                        _errorProvider.SetError(label13, string.Format("'From time' sholud not be greater than 'To Time'"));
                    }
                }
                if (!Time_Test)
                    isValid = false;
                else
                    _errorProvider.SetError(label13, null);
            }
            _errorProvider.SetError(dtCompleted, null);
            _errorProvider.SetError(dtFollowup, null);
            //if (dtFollowup.Checked)
            //{
            //    if (Convert.ToDateTime(dtFollowup.Value) > DateTime.Now.Date)
            //    {
            //        _errorProvider.SetError(dtFollowup, "Future Date is not allowed");
            //        isValid = false;
            //    }
            //}
            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString() == "Y")
            {
                if (dtCompleted.Checked)
                {
                    if (Convert.ToDateTime(dtCompleted.Value) > DateTime.Now.Date)
                    {
                        _errorProvider.SetError(dtCompleted, "Future Date is not allowed");
                        isValid = false;
                    }
                }
                //if ((dtFollowup.Checked) && (dtCompleted.Checked))
                //{
                //    if (Convert.ToDateTime(dtFollowup.Value) > Convert.ToDateTime(dtCompleted.Value))
                //    {
                //        _errorProvider.SetError(dtCompleted, "Complete Date can’t be less than Follow up On Date");
                //        isValid = false;
                //    }
                //}
                //else
                //{
                if (dtCompleted.Checked == true && dtFollowup.Checked == false)
                {
                    _errorProvider.SetError(dtFollowup, "Followup Date is Required");
                    isValid = false;
                }
                //}
            }
            foreach (DataGridViewRow dataGridViewRow in Cust_Grid.Rows)
            {

                if (dataGridViewRow.Cells["gvtCustReq"].Value.ToString() == "*")
                {
                    string inputValue = string.Empty;
                    inputValue = dataGridViewRow.Cells["Resp"].Value != null ? dataGridViewRow.Cells["Resp"].Value.ToString() : string.Empty;
                    if (inputValue.Trim() == string.Empty)
                    {
                        _errorProvider.SetError(Cust_Grid, "Please enter answers to required questions");
                        //CommonFunctions.MessageBoxDisplay("Please enter answers to required questions");
                        isValid = false;
                        break;
                    }
                }
            }

            return isValid;
        }

        private void CASE0006_ContactsForm_Load(object sender, EventArgs e)
        {
            if (!Mode.Equals(Consts.Common.View))
            {
                if (!CntlContactEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                    Btn_Save.Enabled = false;
                }

            }
            if (Mode.Equals(Consts.Common.Edit))
            {
                //Cont_Date.Enabled = false;
                //Cmb_Members.Focus();
                Cont_Date.Focus();
            }
            if (Mode.Equals(Consts.Common.Add))
            {
                Cont_Date.Focus();
            }
        }


        private void PbReferra2_Click(object sender, EventArgs e)
        {

            //List<CASEREFSEntity> sel_REFS_entity = new List<CASEREFSEntity>();
            List<ACTREFSEntity> sel_REFS_entity = new List<ACTREFSEntity>();
            AgencyReferral_SubForm Ref_Form = new AgencyReferral_SubForm("Short", sel_REFS_entity, string.Empty, string.Empty, string.Empty, BaseForm);
            Ref_Form.FormClosed += new FormClosedEventHandler(On_Referral_Select_Closed);
            Ref_Form.StartPosition = FormStartPosition.CenterScreen;
            Ref_Form.ShowDialog();
        }


        private void On_Referral_Select_Closed(object sender, FormClosedEventArgs e)
        {
            string[] SelRef_Name = new string[2];

            AgencyReferral_SubForm form = sender as AgencyReferral_SubForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelRef_Name = form.GetSelected_Referral();

                TxtBillTo.Text = SelRef_Name[0];
                TxtBillTo_Code.Text = SelRef_Name[1];
                if (!string.IsNullOrEmpty(TxtBillTo.Text))
                    Cmb_BillUOM.Enabled = true;

            }
        }

        private void Cmb_How_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Cmb_How.Items.Count > 0)
                {
                    string strHowwhere = ((ListItem)Cmb_How.SelectedItem).Value == null ? string.Empty : ((ListItem)Cmb_How.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strHowwhere))
                    {
                        if (((ListItem)Cmb_How.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)Cmb_How.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show("Inactive How/Where", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Pb_CA_Prog_Click(object sender, EventArgs e)
        {
            string Sel_Prog = (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()) ? Txt_CA_Program.Text.Substring(0, 6) : "");
            string ACR_SerPlan_HIE = "N";
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                ACR_SerPlan_HIE = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.ToString();
            }

            if (ACR_SerPlan_HIE == "Y")
            {
                HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, "1", ACR_SerPlan_HIE);
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.ShowDialog();
            }
            else
            {
                HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, "CASEHIE");
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                hierarchieSelectionForm.StartPosition= FormStartPosition.CenterScreen;
                hierarchieSelectionForm.ShowDialog();
            }
        }
        string Sel_Contact_Program = "";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                Sel_Contact_Program = form.Selected_SerPlan_Prog();


                Txt_CA_Program.Text = Sel_Contact_Program;

                //SetComboBoxValue(Cmb_Program, Sel_Prog);
            }
        }

        private string Set_SP_Program_Text(string Prog_Code)
        {
            string Tmp_Hierarchy = "";
            Sel_Contact_Program = "";

            foreach (HierarchyEntity Ent in SP_Programs_List)
            {
                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                if (Prog_Code == Tmp_Hierarchy)
                {
                    Sel_Contact_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                    break;
                }
            }

            return Sel_Contact_Program;
        }

        private void CASE0006_ContactsForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "pbNotes")
            {
                ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, Mode, Privileges, Hierarchy + Year + App_No + "0000".Substring(0, (4 - Pass_Entity.Seq.Length)) + Pass_Entity.Seq);
                Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                Prog_Form.ShowDialog();
            }
            else if (e.Tool.Name == "Help")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 3, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
            }

        }
    }
}