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
//using Gizmox.WebGUI.Common.Resources;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ServiceCustomQuestionsForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion


        public ServiceCustomQuestionsForm(BaseForm baseform, string hierarchy, string year, string CAMS_desc, CASEACTEntity pass_entity, PrivilegeEntity privileges, List<CASEACTEntity> CA_template_list, string CA_Benefit, List<CAOBOEntity> CA_OBO_List)
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

            //cmb_CA_Benefit.Enabled = false;

            if (Sel_CA_OBO == null)
                Sel_CA_OBO = new List<CAOBOEntity>();

            //Fill_CA_Benefiting_From();
            //Fill_CA_Members_DropDown();
            //Set_Members_CA_Grid_As_Benefit_Change(false, CAOBF);
            //Fill_Custom_Questions();
            Fill_SAL_Custom_Questions();

        }

        public ServiceCustomQuestionsForm(BaseForm baseform, string hierarchy, string year, string CAMS_desc, string Code, string branch, string group, CASEACTEntity pass_entity, PrivilegeEntity privileges, List<CASEACTEntity> CA_template_list, string Type)
        {
            InitializeComponent();
            BaseForm = baseform;
            Hierarchy = hierarchy;
            Pass_CA_Entity = pass_entity;
            Privileges = privileges;
            CA_Template_List = CA_template_list;

            CAMS_Desc = CAMS_desc;

            Year = year;
            CAMSCode = Code; Branch = branch; Group = group; CodeType = Type;

            this.Text = "Custom Questions";

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            if (CAMS_desc.Length > 60)
                this.Text = CAMS_desc.Substring(0, 60);
            else
                this.Text = CAMS_desc.Trim();

            //cmb_CA_Benefit.Enabled = true;



            //Fill_CA_Benefiting_From();
            //Fill_CA_Members_DropDown();
            //Set_Members_CA_Grid_As_Benefit_Change(false, CAOBF);
            //Fill_Custom_Questions();
            Fill_SAL_Custom_Questions();


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
        public List<SalquesEntity> SALQUESEntity { get; set; }
        public List<SalqrespEntity> SALQUESRespEntity { get; set; }

        #endregion

        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        List<CustfldsEntity> Cust;
        private void Fill_Custom_Questions()
        {
            Cust_Grid.Rows.Clear();
            Cust = _model.FieldControls.GetCUSTFLDSByScrCode("CASE0063", "FLDCNTLHIE", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
            List<CustomQuestionsEntity> custQuestions = _model.FieldControls.GetCustomQuestions("CASE0063", "*", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "", "ALL", "P");

            CustRespEntity Search_Entity = new CustRespEntity(true);
            Search_Entity.ScrCode = "CASE0063";
            CustResp_List = _model.FieldControls.Browse_CUSTRESP(Search_Entity, "Browse");

            //if (Pass_CA_Entity.Rec_Type == "I" && CA_Template_List.Count > 0)
            //{
            //    Pass_CA_Entity.Cust_Code1 = CA_Template_List[0].Cust_Code1;
            //    Pass_CA_Entity.Cust_Code2 = CA_Template_List[0].Cust_Code2;
            //    Pass_CA_Entity.Cust_Code3 = CA_Template_List[0].Cust_Code3;
            //}

            if (Cust.Count > 0)
            {
                int rowIndex = 0, Tmp_Row_Cnt = 0;
                bool DropDown_Exists = false, IS_Que_Req = false;
                string Tmp_Cust_Resp = null, Tmp_Cust_Resp_Code = null, Tmp_Cust_Resp_Desc = "";
                foreach (CustfldsEntity Entity in Cust)
                {
                    Tmp_Cust_Resp = " ";
                    if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code1))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value1;
                    else
                        if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code2))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value2;
                    else
                            if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code3))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value3;
                    else
                                if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code4))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value4;
                    else
                                    if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code5))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value5;

                    Tmp_Cust_Resp_Desc = Tmp_Cust_Resp;
                    if (Entity.RespType == "D" || Entity.RespType == "C")
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

                    IS_Que_Req = false;
                    foreach (CustomQuestionsEntity Cust_Fld in custQuestions)
                    {
                        if (Entity.CustCode == Cust_Fld.CUSTCODE && Cust_Fld.CUSTREQUIRED == "Y")
                        {
                            IS_Que_Req = true;
                            break;
                        }
                    }

                    rowIndex = Cust_Grid.Rows.Add(Entity.CustDesc, Tmp_Cust_Resp_Desc, (IS_Que_Req ? "*" : ""), Entity.RespType, Entity.CustCode, Tmp_Cust_Resp);
                    Tmp_Row_Cnt++;

                    set_Cust_Grid_Tooltip(rowIndex, Entity.RespType);

                    if (Entity.RespType == "C" || Entity.RespType == "D")
                    {
                        DropDown_Exists = true;
                        Cust_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        Cust_Grid.Rows[rowIndex].Cells["Resp"].ReadOnly = true;
                    }

                    if (Tmp_Row_Cnt == 5) //changed from 3 to 5 On 11/20/2014
                        break;
                }
                //set_CustGrid_Rows_On_Questions(DropDown_Exists);
            }
        }

        private void Fill_SAL_Custom_Questions()
        {
            SaldefEntity Search_saldef_Entity = new SaldefEntity(true);

            List<SaldefEntity> SALDEF = _model.SALDEFData.Browse_SALDEF(Search_saldef_Entity, "Browse", BaseForm.UserID, BaseForm.BaseAdminAgency);
            List<SaldefEntity> SALDEFEntity = new List<SaldefEntity>();
            if (SALDEF.Count > 0)
            {

                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                {
                    if (!string.IsNullOrEmpty(Pass_CA_Entity.Act_PROG.Trim()))
                    {
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG) || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 4) + "**") || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 2) + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST != "Y");
                        if (SALDEFEntity.Count == 0)
                            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG) || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 4) + "**") || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 2) + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                        if (SALDEFEntity.Count == 0)
                            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG) || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 4) + "**") || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 2) + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Equals(string.Empty) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                    }
                    else
                    {
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Contains(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST != "Y");
                        if (SALDEFEntity.Count == 0)
                            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                        if (SALDEFEntity.Count == 0)
                            SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Equals(string.Empty) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                    }

                }
                else
                {
                    SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Contains(Pass_CA_Entity.ACT_Code.Trim()) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST != "Y");
                    if (SALDEFEntity.Count == 0)
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                    if (SALDEFEntity.Count == 0)
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Equals(string.Empty) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                    //SALDEF = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_TYPE.Equals("S"));
                }

                //if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol=="Y")
                //{
                //    SALDEF = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG) || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0, 4) + "**") || u.SALD_HIE.Contains(Pass_CA_Entity.Act_PROG.Substring(0,2) + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
                //}
                //else
                //    SALDEF = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_SPS.Contains(Pass_CA_Entity.Service_plan.Trim()) && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");
            }

            SALQUESEntity = new List<SalquesEntity>();
            if (SALDEFEntity.Count > 0)
            {
                SalquesEntity Search_Salques_Entity = new SalquesEntity(true);
                Search_Salques_Entity.SALQ_SALD_ID = SALDEFEntity[0].SALD_ID;
                SALQUESEntity = _model.SALDEFData.Browse_SALQUES(Search_Salques_Entity, "Browse");

                if (SALQUESEntity.Count > 0) SALQUESEntity = SALQUESEntity.OrderBy(u => Convert.ToInt32(u.SALQ_GRP_SEQ)).ThenBy(u => Convert.ToInt32(u.SALQ_SEQ)).ThenBy(u => Convert.ToInt32(u.SALQ_GRP_CODE)).ToList();
            }


            SalqrespEntity Search_Salqresp_Entity = new SalqrespEntity(true);
            SALQUESRespEntity = _model.SALDEFData.Browse_SALQRESP(Search_Salqresp_Entity, "Browse");

            if (SALQUESEntity.Count > 0)
            {
                int rowIndex = 0, Tmp_Row_Cnt = 0;
                bool DropDown_Exists = false, IS_Que_Req = false;
                string Tmp_Cust_Resp = null, Tmp_Cust_Resp_Code = null, Tmp_Cust_Resp_Desc = "";


                /**********************************************/
                if (Cust_Grid.Columns.Count == 0)
                {
                    int y = 0;
                    if (SALQUESEntity.Count == 1)
                    {
                        if (SALQUESEntity[0].SALQ_SEQ != "0")
                            y = 0;
                        else
                            y = 1;
                    }
                    if (y == 0)
                    {
                        Cust_Grid.Columns.Add("Ques");
                        Cust_Grid.Columns.Add("Resp");
                        Cust_Grid.Columns.Add("CA_Cust_Req");
                        Cust_Grid.Columns.Add("Type");
                        Cust_Grid.Columns.Add("Code");
                        Cust_Grid.Columns.Add("Resp_Code");
                    }
                }
                /**********************************************/



                foreach (SalquesEntity Entity in SALQUESEntity)
                {

                    if (Entity.SALQ_SEQ != "0")
                    {
                        rowIndex = Cust_Grid.Rows.Add();

                        #region ColumnHeaders
                        for (int x = 0; x < 6; x++)
                        {
                            int HeadcolIndex = x;


                            DataGridViewCell gvCell = new DataGridViewCell();
                            this.Cust_Grid[HeadcolIndex, rowIndex] = gvCell;
                            this.Cust_Grid[HeadcolIndex, rowIndex].Value = string.Empty;
                            gvCell.Style.Padding = new System.Windows.Forms.Padding(2);
                            this.Cust_Grid.Columns[HeadcolIndex].Visible = false;
                            this.Cust_Grid.Columns[HeadcolIndex].HeaderText = "";

                            if (x == 0)
                            {
                                this.Cust_Grid.Columns[HeadcolIndex].Visible = true;
                                this.Cust_Grid.Columns[HeadcolIndex].HeaderText = "Question";
                                this.Cust_Grid.Columns[HeadcolIndex].Width = 260;
                                this.Cust_Grid.Columns[HeadcolIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                                this.Cust_Grid.Columns[HeadcolIndex].HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            }
                            if (x == 1)
                            {
                                this.Cust_Grid.Columns[HeadcolIndex].Visible = true;
                                this.Cust_Grid.Columns[HeadcolIndex].HeaderText = "Response";
                                this.Cust_Grid.Columns[HeadcolIndex].Width = 280;
                            }
                            if (x == 2)
                            {
                                this.Cust_Grid.Columns[HeadcolIndex].Visible = true;
                                this.Cust_Grid.Columns[HeadcolIndex].HeaderText = "Req";
                                this.Cust_Grid.Columns[HeadcolIndex].Width = 54;
                                this.Cust_Grid.Columns[HeadcolIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                this.Cust_Grid.Columns[HeadcolIndex].DefaultCellStyle.ForeColor = Color.Red;
                            }
                        }
                        #endregion


                        Tmp_Cust_Resp = " ";
                        if (Entity.SALQ_ID.Equals(Pass_CA_Entity.Cust_Code1))
                            Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value1;
                        else
                            if (Entity.SALQ_ID.Equals(Pass_CA_Entity.Cust_Code2))
                            Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value2;
                        else
                                if (Entity.SALQ_ID.Equals(Pass_CA_Entity.Cust_Code3))
                            Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value3;
                        else
                                    if (Entity.SALQ_ID.Equals(Pass_CA_Entity.Cust_Code4))
                            Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value4;
                        else
                                        if (Entity.SALQ_ID.Equals(Pass_CA_Entity.Cust_Code5))
                            Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value5;

                        Tmp_Cust_Resp_Desc = Tmp_Cust_Resp;
                        if (Entity.SALQ_TYPE == "D")
                        {
                            foreach (SalqrespEntity Ent in SALQUESRespEntity)
                            {
                                if (Entity.SALQ_ID == Ent.SALQR_Q_ID && Tmp_Cust_Resp == Ent.SALQR_CODE)
                                {
                                    Tmp_Cust_Resp_Desc = Ent.SALQR_DESC;
                                    break;
                                }
                            }
                        }
                        else if (Entity.SALQ_TYPE == "C")
                        {
                            string custQuestionResp = string.Empty;
                            List<SalqrespEntity> selRespEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID));

                            if (selRespEntity.Count > 0)
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
                                        SalqrespEntity custRespEntity = selRespEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                                        if (custRespEntity != null)
                                        {
                                            custQuestionResp += custRespEntity.SALQR_DESC + ", ";
                                            //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                        }

                                    }

                                    Tmp_Cust_Resp_Desc = custQuestionResp;
                                }
                            }
                        }

                        IS_Que_Req = false;
                        //foreach (CustomQuestionsEntity Cust_Fld in custQuestions)
                        //{
                        //    if (Entity.CustCode == Cust_Fld.CUSTCODE && Cust_Fld.CUSTREQUIRED == "Y")
                        //    {
                        //        IS_Que_Req = true;
                        //        break;
                        //    }
                        //}
                        if (Entity.SALQ_REQ.Trim() == "Y") IS_Que_Req = true;

                        Cust_Grid.Rows[rowIndex].Cells["Ques"].Value = Entity.SALQ_DESC;
                        /*************************************************************************************************/
                        /************************************** MULTI CONTROLS ***************************************************/
                        /*************************************************************************************************/

                        if (Entity.SALQ_TYPE == "X")
                        {
                            DataGridViewTextBoxCell TextBoxCell = new DataGridViewTextBoxCell();
                            this.Cust_Grid["Resp", rowIndex] = TextBoxCell;
                            this.Cust_Grid["Resp", rowIndex].Value = "";
                            this.Cust_Grid["Resp", rowIndex].ToolTipText = "Question Type: Text";
                            TextBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";

                            Cust_Grid.Rows[rowIndex].Cells["Resp"].Value = Tmp_Cust_Resp_Desc;
                        }

                        if (Entity.SALQ_TYPE == "T")
                        {
                            DataGridViewDateTimePickerCell Response = new DataGridViewDateTimePickerCell();
                            Response.Format = DateTimePickerFormat.Short;
                            Response.Style.BackgroundImageSource = "icon-calendar";
                            Response.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                            this.Cust_Grid["Resp", rowIndex] = Response;
                            this.Cust_Grid["Resp", rowIndex].Value = string.Empty;
                            this.Cust_Grid["Resp", rowIndex].ToolTipText = "Question Type: Date";
                            Response.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";

                            if (Tmp_Cust_Resp_Desc.Trim() != "")
                                Cust_Grid.Rows[rowIndex].Cells["Resp"].Value = Convert.ToDateTime(Tmp_Cust_Resp_Desc.Trim()).ToString("MM/dd/yyyy");
                            else
                                Cust_Grid.Rows[rowIndex].Cells["Resp"].Value = Tmp_Cust_Resp_Desc;

                        }
                        if (Entity.SALQ_TYPE == "N")
                        {
                            DataGridViewTextBoxCell Response = new DataGridViewTextBoxCell();
                            this.Cust_Grid["Resp", rowIndex] = Response;
                            this.Cust_Grid["Resp", rowIndex].Value = string.Empty;
                            this.Cust_Grid["Resp", rowIndex].ToolTipText = "Question Type: Numeric";
                            Response.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";

                            Cust_Grid.Rows[rowIndex].Cells["Resp"].Value = Tmp_Cust_Resp_Desc;
                        }
                        if (Entity.SALQ_TYPE == "D")
                        {
                            DataGridViewComboBoxCell ComboBoxCell = new DataGridViewComboBoxCell();
                            ComboBoxCell.Style.BackgroundImageSource = "combo-arrow";
                            ComboBoxCell.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                            ComboBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px; ";

                            this.Cust_Grid["Resp", rowIndex] = ComboBoxCell;
                            this.Cust_Grid["Resp", rowIndex].ToolTipText = "Question Type: Drop down";

                            List<SalqrespEntity> SelQuesResp = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(Entity.SALQ_ID.ToString()));
                            if (SelQuesResp.Count > 0)
                            {
                                ComboBoxCell.DataSource = SelQuesResp;
                                ComboBoxCell.DisplayMember = "SALQR_DESC";
                                ComboBoxCell.ValueMember = "SALQR_CODE";
                            }

                            if (Tmp_Cust_Resp_Desc.Trim() != "")
                            {
                                if (SelQuesResp.Count > 0)
                                {
                                    List<SalqrespEntity> SelVal = SelQuesResp.FindAll(x => x.SALQR_CODE.Trim().ToString() == Tmp_Cust_Resp_Desc.ToString());
                                    if (SelVal.Count > 0)
                                    {
                                        Tmp_Cust_Resp_Desc = SelVal[0].SALQR_DESC.Trim();
                                    }
                                }
                            }

                            Cust_Grid.Rows[rowIndex].Cells["Resp"].Value = Tmp_Cust_Resp_Desc;

                        }
                        if (Entity.SALQ_TYPE == "C")
                        {
                            DataGridViewButtonCell Response = new DataGridViewButtonCell();
                            Response.Style.ForeColor = System.Drawing.Color.White;
                            this.Cust_Grid["Resp", rowIndex] = Response;
                            this.Cust_Grid["Resp", rowIndex].Value = string.Empty;
                            this.Cust_Grid["Resp", rowIndex].ToolTipText = "Question Type: Check Box";

                            Cust_Grid.Rows[rowIndex].Cells["Resp"].Value = Tmp_Cust_Resp_Desc;

                        }




                        Cust_Grid.Rows[rowIndex].Cells["CA_Cust_Req"].Value = (IS_Que_Req ? "*" : "");
                        Cust_Grid.Rows[rowIndex].Cells["Type"].Value = Entity.SALQ_TYPE;
                        Cust_Grid.Rows[rowIndex].Cells["Code"].Value = Entity.SALQ_ID;
                        Cust_Grid.Rows[rowIndex].Cells["Resp_Code"].Value = Tmp_Cust_Resp;
                        /*************************************************************************************************/
                        /*************************************************************************************************/


                        //rowIndex = Cust_Grid.Rows.Add(Entity.SALQ_DESC, Tmp_Cust_Resp_Desc, (IS_Que_Req ? "*" : ""), Entity.SALQ_TYPE, Entity.SALQ_ID, Tmp_Cust_Resp);
                        Tmp_Row_Cnt++;

                        set_Cust_Grid_Tooltip(rowIndex, Entity.SALQ_TYPE);

                        //if (Entity.SALQ_TYPE == "C" || Entity.SALQ_TYPE == "D")
                        //{
                        //    DropDown_Exists = true;
                        //    Cust_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        //    Cust_Grid.Rows[rowIndex].Cells["Resp"].ReadOnly = true;
                        //}

                        if (Tmp_Row_Cnt == 5) //changed from 3 to 5 On 11/20/2014
                            break;
                    }
                }
            }

        }

        List<CustRespEntity> CustResp_List;
        private void set_CustGrid_Rows_On_Questions(bool DropDown_Exists)
        {
            CustRespEntity Search_Entity = new CustRespEntity();

            Search_Entity.ScrCode = "CASE0063";
            Search_Entity.RecType = Search_Entity.ResoCode = Search_Entity.RespSeq = null;
            Search_Entity.RespDesc = Search_Entity.DescCode = Search_Entity.AddDate = Search_Entity.AddOpr = null;
            Search_Entity.ChgDate = Search_Entity.ChgOpr = Search_Entity.Changed = Search_Entity.ScrCode = null;

            if (DropDown_Exists)
                CustResp_List = _model.FieldControls.Browse_CUSTRESP(Search_Entity, "Browse");

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


        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if (Cust_Grid.Rows.Count > 0)
            {
                if (Cust_Grid.CurrentRow.Cells["Type"].Value.ToString() == "D")
                {
                    List<PopUp_Menu_L1_Entity> listItem = new List<PopUp_Menu_L1_Entity>();

                    foreach (SalqrespEntity Entity in SALQUESRespEntity)
                    {
                        if (Cust_Grid.CurrentRow.Cells["Code"].Value.ToString() == Entity.SALQR_Q_ID)
                        {
                            MenuItem Resp_Menu = new MenuItem();
                            Resp_Menu.Text = Entity.SALQR_DESC;
                            Resp_Menu.Tag = Entity.SALQR_CODE;
                            contextMenu1.MenuItems.Add(Resp_Menu);

                            if (Cust_Grid.CurrentRow.Cells["Resp"].Value.ToString() == Entity.SALQR_DESC)
                                Resp_Menu.Checked = true;
                        }
                    }
                    MenuItem Resp_Menu1 = new MenuItem();
                    Resp_Menu1.Text = "Clear Response";
                    Resp_Menu1.Tag = "CLRRSP";
                    contextMenu1.MenuItems.Add(Resp_Menu1);
                }
                else if (Cust_Grid.CurrentRow.Cells["Type"].Value.ToString() == "C")
                {
                    string response = Cust_Grid.SelectedRows[0].Cells[5].Value != null ? Cust_Grid.SelectedRows[0].Cells[5].Value.ToString() : string.Empty;
                    //gvQuestions.SelectedRows[0].Cells[8].Tag != null ? gvQuestions.SelectedRows[0].Cells[8].Tag.ToString() : string.Empty;
                    PrivilegeEntity privileges = new PrivilegeEntity();
                    privileges.AddPriv = "true";
                    AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, Cust_Grid.CurrentRow.Cells["Code"].Value.ToString(), Cust_Grid.CurrentRow.Cells["Ques"].Value.ToString(), string.Empty);
                    objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                    objform.ShowDialog();
                }
            }
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            //AlertCodeForm form = sender as AlertCodeForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    gvwCustomQuestions.SelectedRows[0].Cells[3].Tag = form.propAlertCode;
            //    gvwCustomQuestions.SelectedRows[0].Cells[3].Value = form.propAlertCode;
            //}
            // //  txtAlertCodes.Text = form.propAlertCode;

            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Cust_Grid.SelectedRows[0].Cells[5].Tag = form.propAlertCode;

                string custQuestionResp = string.Empty;
                SalqrespEntity Search_entity = new SalqrespEntity(true);
                Search_entity.SALQR_Q_ID = Cust_Grid.SelectedRows[0].Cells["Code"].Value.ToString();

                List<SalqrespEntity> SALResponseEntity = _model.SALDEFData.Browse_SALQRESP(Search_entity, "Browse");
                if (SALResponseEntity.Count > 0)
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

                            SalqrespEntity custRespEntity = SALResponseEntity.Find(u => u.SALQR_CODE.Trim().Equals(stringitem.Trim()));
                            if (custRespEntity != null)
                            {
                                custQuestionResp += custRespEntity.SALQR_DESC + ", ";
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
                    //Cust_Grid.SelectedRows[0].Cells["Ques_Delete"].Value = DeleteImage;
                }
                Cust_Grid.SelectedRows[0].Cells[1].Value = custQuestionResp;
                Cust_Grid.SelectedRows[0].Cells[5].Value = form.propAlertCode;

            }

        }













        //private void Fill_CA_Benefiting_From()
        //{
        //    //this.cmb_CA_Benefit.SelectedIndexChanged -= new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);

        //    cmb_CA_Benefit.Items.Clear();
        //    cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Applicant", "1"));
        //    cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("All Household Members", "2"));
        //    cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Selected Household Members", "3"));


        //    //this.cmb_CA_Benefit.SelectedIndexChanged += new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);
        //    if(CAOBF=="1") cmb_CA_Benefit.SelectedIndex = 0; else if (CAOBF == "2") cmb_CA_Benefit.SelectedIndex = 1; else if (CAOBF == "3") cmb_CA_Benefit.SelectedIndex = 2;
        //    //if (Pass_CA_Entity.Rec_Type == "I")
        //    //    cmb_CA_Benefit.SelectedIndex = 1;


        //}

        //private void Fill_CA_Members_DropDown()
        //{
        //    CA_Members_Grid.Rows.Clear();
        //    DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "All", null, null, Pass_CA_Entity.App_no, null, null, null, null, null, null, null, null, null, null, Hierarchy + Year, null, BaseForm.UserID, string.Empty, string.Empty);
        //    if (ds.Tables.Count > 0)
        //    {
        //        DataTable dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            List<CommonEntity> Relation;
        //            Relation = _model.lookupDataAccess.GetRelationship();

        //            int rowIndex = 0;
        //            string Name = null, TmpSsn = null, Relation_Desc = null;
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                Name = TmpSsn = Relation_Desc = null;

        //                Name = dr["Fname"].ToString().Trim() + " " + dr["MName"].ToString() + " " + dr["Lname"].ToString().Trim();
        //                TmpSsn = dr["Ssn"].ToString();
        //                if (!string.IsNullOrEmpty(TmpSsn))
        //                    TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);


        //                foreach (CommonEntity Relationship in Relation)
        //                {
        //                    if (Relationship.Code.Equals(dr["Mem_Code"].ToString().Trim()))
        //                    {
        //                        Relation_Desc = Relationship.Desc; break;
        //                    }
        //                }



        //                rowIndex = CA_Members_Grid.Rows.Add(false, Name, Relation_Desc, TmpSsn, dr["RecFamSeq"].ToString(), dr["ClientID"].ToString(), dr["AppNo"].ToString().Substring(10, 1), "N", dr["AppStatus"].ToString(), dr["SNP_EXCLUDE"].ToString());

        //                if (dr["AppStatus"].ToString() != "A")
        //                    CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

        //                if (dr["SNP_EXCLUDE"].ToString() != "N")
        //                    CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;


        //                if (dr["AppNo"].ToString().Substring(10, 1) == "A")
        //                {
        //                    if (dr["AppStatus"].ToString() != "A")
        //                        CA_Members_Grid.Rows[rowIndex].Cells["CA_Mem_Name"].Style.ForeColor = Color.Blue;
        //                    else
        //                        CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
        //                }
        //                //Members_Grid.Rows[rowIndex].Tag = dr;


        //            }
        //        }
        //    }
        //    //Get_CAOBO_Data();
        //}

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

        //private void Set_Members_CA_Grid_As_Benefit_Change(bool Set_Mem_On_Combo, string OBF_Type)
        //{
        //    if (CA_Members_Grid.Rows.Count > 0)
        //    {
        //        string Mem_Status = "M";
        //        this.CA_Sel.ReadOnly = true;
        //        switch (OBF_Type)
        //        {
        //            case "1": Mem_Status = "A"; break;
        //            case "2": Mem_Status = "M"; break;
        //            case "3": Mem_Status = "Y"; this.CA_Sel.ReadOnly = false; break;
        //        }


        //        if (Set_Mem_On_Combo)//(Pass_MS_Entity.Rec_Type == "I" && !Set_Mem_From_OBO)
        //        {
        //            int Row_index = 0;
        //            foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //            {
        //                switch (Mem_Status)
        //                {
        //                    case "A":
        //                        if (dr.Cells["CA_AppSw"].Value.ToString() == Mem_Status)
        //                        {
        //                            if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                                dr.Cells["CA_Sel"].Value = true;
        //                            // Members_Grid.Rows[Row_index].DefaultCellStyle.ForeColor = Color.Blue;
        //                        }
        //                        else
        //                            dr.Cells["CA_Sel"].Value = false;
        //                        break;
        //                    case "M":
        //                        if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                            dr.Cells["CA_Sel"].Value = true;
        //                        break;
        //                    default:
        //                        //if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                        //    dr.Cells["CA_Sel"].Value = true;
        //                        dr.Cells["CA_Sel"].Value = false;
        //                        break;
        //                }
        //                Row_index++;
        //            }
        //        }
        //        else
        //            Set_Members_FromCAOBO();
        //    }

        //}

        //private void Set_Members_FromCAOBO()
        //{
        //    //if (Sel_CA_OBO.Count > 1)
        //    //{
        //    //    foreach (CAOBOEntity Entity in Sel_CA_OBO)
        //    //    {
        //    //        foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //    //        {
        //    //            if (Entity.CLID == dr.Cells["CA_CLID"].Value.ToString() &&
        //    //                Entity.Fam_Seq == dr.Cells["CA_Mem_Seq"].Value.ToString())
        //    //            {
        //    //                dr.Cells["CA_Sel"].Value = true;
        //    //                dr.Cells["Is_CAOBO_Rec"].Value = "Y";
        //    //                break;
        //    //            }
        //    //            //else
        //    //            //{
        //    //            //    dr.Cells["MS_Sel"].Value = false;
        //    //            //    dr.Cells["Is_OBO_Rec"].Value = "N";
        //    //            //    break;
        //    //            //}
        //    //        }
        //    //    }
        //    //}
        //    if (Sel_CA_OBO.Count > 0 )
        //    {
        //        foreach (CAOBOEntity Entity in Sel_CA_OBO)
        //        {
        //            foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //            {
        //                if (Entity.CLID == dr.Cells["CA_CLID"].Value.ToString() &&
        //                    Entity.Fam_Seq == dr.Cells["CA_Mem_Seq"].Value.ToString())
        //                {
        //                    dr.Cells["CA_Sel"].Value = true;
        //                    dr.Cells["Is_CAOBO_Rec"].Value = "Y";
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string Mem_Status = "M";
        //        this.CA_Sel.ReadOnly = true;

        //        switch (CAOBF)
        //        {
        //            case "1": Mem_Status = "A"; break;
        //            case "2": Mem_Status = "M"; break;
        //            case "3": Mem_Status = "Y"; this.CA_Sel.ReadOnly = false; break;
        //        }

        //        int Row_index = 0;
        //        foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //        {
        //            switch (Mem_Status)
        //            {
        //                case "A":
        //                    if (dr.Cells["CA_AppSw"].Value.ToString() == Mem_Status)
        //                    {
        //                        if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                            dr.Cells["CA_Sel"].Value = true;
        //                        // Members_Grid.Rows[Row_index].DefaultCellStyle.ForeColor = Color.Blue;
        //                    }
        //                    else
        //                        dr.Cells["CA_Sel"].Value = false;
        //                    break;
        //                case "M":
        //                    if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                        dr.Cells["CA_Sel"].Value = true;
        //                    break;
        //                default:
        //                    //if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                    //    dr.Cells["CA_Sel"].Value = true;
        //                    dr.Cells["CA_Sel"].Value = false;
        //                    break;
        //            }
        //            Row_index++;
        //        }
        //    }
        //}

        //public List<CAOBOEntity> GetMemberRecords()
        //{
        //    List<CAOBOEntity> MembersData = new List<CAOBOEntity>();

        //    if(CA_Members_Grid.Rows.Count>0)
        //    {
        //        foreach(DataGridViewRow dr in CA_Members_Grid.Rows)
        //        {
        //            CAOBOEntity Entity = new CAOBOEntity();
        //            if(dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
        //            {
        //                Entity.ID = Pass_CA_Entity.ACT_ID;
        //                Entity.CLID = dr.Cells["CA_CLID"].Value.ToString();
        //                Entity.Fam_Seq = dr.Cells["CA_Mem_Seq"].Value.ToString();

        //                MembersData.Add(new CAOBOEntity(Entity));
        //            }
        //        }
        //    }

        //    return MembersData;
        //}


        //public List<CAOBOEntity> GetRecordsForMembers()
        //{
        //    List<CAOBOEntity> MembersData = new List<CAOBOEntity>();

        //    if (CA_Members_Grid.Rows.Count > 0)
        //    {
        //        foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //        {
        //            CAOBOEntity Entity = new CAOBOEntity();
        //            if (dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
        //            {
        //                Entity.ID = Pass_CA_Entity.ACT_ID;
        //                Entity.CLID = dr.Cells["CA_CLID"].Value.ToString();
        //                Entity.Fam_Seq = dr.Cells["CA_Mem_Seq"].Value.ToString();
        //                Entity.Type = CodeType;
        //                Entity.Code = CAMSCode;
        //                Entity.Branch = Branch;
        //                Entity.Group = Group;
        //                Entity.BenefitFrom = ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();

        //                MembersData.Add(new CAOBOEntity(Entity));
        //            }
        //        }
        //    }

        //    return MembersData;
        //}

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
            foreach (DataGridViewRow dataGridViewRow in Cust_Grid.Rows)
            {

                if (dataGridViewRow.Cells["CA_Cust_Req"].Value.ToString() == "*")
                {
                    string inputValue = string.Empty;
                    inputValue = dataGridViewRow.Cells["Resp"].Value != null ? dataGridViewRow.Cells["Resp"].Value.ToString() : string.Empty;
                    if (inputValue.Trim() == string.Empty)
                    {
                        //CommonFunctions.MessageBoxDisplay("Please Provide Response for Required Custom Question(s)");
                        AlertBox.Show("Custom Question requires a response", MessageBoxIcon.Warning);
                        isValid = false;
                        break;
                    }
                }
            }

            return (isValid);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void CA_Members_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (CA_Members_Grid.Rows.Count > 0)
        //    {
        //        if (e.ColumnIndex == 0 && e.RowIndex != -1)
        //        {
        //            if (CA_Members_Grid.CurrentRow.Cells["CA_Active_Sw"].Value.ToString() != "A")
        //            {
        //                CA_Members_Grid.CurrentRow.Cells["CA_Sel"].Value = false;
        //                MessageBox.Show("Member is Inactive", "CAP Systems");
        //                return;
        //            }

        //            if (CA_Members_Grid.CurrentRow.Cells["CA_Exclude_Sw"].Value.ToString() == "Y")
        //            {
        //                CA_Members_Grid.CurrentRow.Cells["CA_Sel"].Value = false;
        //                MessageBox.Show("Member is Excluded", "CAP Systems");
        //            }
        //        }
        //    }
        //}

        //private void cmb_CA_Benefit_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Text.ToString()))
        //        Set_Members_CA_Grid_As_Benefit_Change(true, ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString());
        //}

        private void Cust_Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal number;
            DateTime Compare_Date = DateTime.Today;
            if (e.ColumnIndex == 1)
            {
                switch (Cust_Grid.CurrentRow.Cells["Type"].Value.ToString())
                {
                    case "N":
                        if (Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            if ((!(Decimal.TryParse(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out number))) &&
                        !(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())))
                            {
                                AlertBox.Show("Please Enter Decimal Response", MessageBoxIcon.Warning);
                                Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                            }
                        }
                        break;

                    case "X":
                    case "A":
                        if (Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            if ((string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Trim())))
                            {
                                AlertBox.Show("Please Provide Valid Data", MessageBoxIcon.Warning);
                                Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                            }
                        }
                        break;

                    case "T":
                        //if (!(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())) &&
                        //      (!(Convert.ToDateTime(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out DateTime.Today))))
                        //{
                        //    MessageBox.Show("Please Provide Valid Data", "CAP Systems", MessageBoxButtons.OK);
                        //    Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                        //}
                        if (!(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString())))
                        {
                            string strDate = Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                            if (strDate != "" && strDate != "Date Entered" && strDate != "*")
                            {
                                strDate = Convert.ToDateTime(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()).ToString("MM/dd/yyyy");
                                if (!System.Text.RegularExpressions.Regex.IsMatch(strDate, Consts.StaticVars.DateFormatMMDDYYYY))
                                {
                                    try
                                    {
                                        if (DateTime.Parse(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < Convert.ToDateTime("01/01/1800"))
                                        {
                                            AlertBox.Show("01/01/1800 below date is not accepted", MessageBoxIcon.Warning);
                                            Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                                        }
                                        else
                                            AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                                    }
                                    catch (Exception)
                                    {
                                        AlertBox.Show("Please Enter Valid Date Format MM/DD/YYYY", MessageBoxIcon.Warning);
                                        Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void CA_Cust_Grid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is string)
            {
                if (objArgs.MenuItem.Tag != "CLRRSP")
                {
                    Cust_Grid.CurrentRow.Cells["Resp"].Value = objArgs.MenuItem.Text;
                    Cust_Grid.CurrentRow.Cells["Resp_Code"].Value = objArgs.MenuItem.Tag;
                }
                else
                {
                    Cust_Grid.CurrentRow.Cells["Resp"].Value = " ";
                    Cust_Grid.CurrentRow.Cells["Resp_Code"].Value = " ";
                }
            }
        }

        public string[] GetSelected_MS_Code()
        {
            string[] Added_Edited_QuesCode = new string[15] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            int Tmp_Cust_Cnt = 1;
            string Curr_Ques_Type = "";
            if (Cust_Grid.Rows.Count > 0)
            {
                this.Cust_Grid.Sort(this.Cust_Grid.Columns["Code"], ListSortDirection.Ascending);
            }
            foreach (DataGridViewRow dr in Cust_Grid.Rows)
            {
                Curr_Ques_Type = dr.Cells["Type"].Value.ToString();
                switch (Tmp_Cust_Cnt)
                {
                    case 1:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString().Trim()))
                        {
                            Added_Edited_QuesCode[0] = dr.Cells["Code"].Value.ToString();
                            Added_Edited_QuesCode[10] = dr.Cells["CA_Cust_Req"].Value.ToString();
                            if (Curr_Ques_Type == "C")
                                Added_Edited_QuesCode[1] = dr.Cells["Resp_Code"].Value.ToString().Trim();
                            else
                                Added_Edited_QuesCode[1] = dr.Cells["Resp"].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(Added_Edited_QuesCode[1].Trim()))
                                Added_Edited_QuesCode[1] = "";
                        }
                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString().Trim()))
                        {
                            Added_Edited_QuesCode[2] = dr.Cells["Code"].Value.ToString();
                            Added_Edited_QuesCode[11] = dr.Cells["CA_Cust_Req"].Value.ToString();
                            if (Curr_Ques_Type == "C")
                                Added_Edited_QuesCode[3] = dr.Cells["Resp_Code"].Value.ToString().Trim();
                            else
                                Added_Edited_QuesCode[3] = dr.Cells["Resp"].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(Added_Edited_QuesCode[3].Trim()))
                                Added_Edited_QuesCode[3] = "";
                        }
                        break;
                    case 3:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString().Trim()))
                        {
                            Added_Edited_QuesCode[4] = dr.Cells["Code"].Value.ToString();
                            Added_Edited_QuesCode[12] = dr.Cells["CA_Cust_Req"].Value.ToString();
                            if (Curr_Ques_Type == "C")
                                Added_Edited_QuesCode[5] = dr.Cells["Resp_Code"].Value.ToString().Trim();
                            else
                                Added_Edited_QuesCode[5] = dr.Cells["Resp"].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(Added_Edited_QuesCode[5].Trim()))
                                Added_Edited_QuesCode[5] = "";
                        }
                        break;
                    case 4:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString().Trim()))
                        {
                            Added_Edited_QuesCode[6] = dr.Cells["Code"].Value.ToString();
                            Added_Edited_QuesCode[13] = dr.Cells["CA_Cust_Req"].Value.ToString();
                            if (Curr_Ques_Type == "C")
                                Added_Edited_QuesCode[7] = dr.Cells["Resp_Code"].Value.ToString().Trim();
                            else
                                Added_Edited_QuesCode[7] = dr.Cells["Resp"].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(Added_Edited_QuesCode[7].Trim()))
                                Added_Edited_QuesCode[7] = "";
                        }
                        break;
                    case 5:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString().Trim()))
                        {
                            Added_Edited_QuesCode[8] = dr.Cells["Code"].Value.ToString();
                            Added_Edited_QuesCode[14] = dr.Cells["CA_Cust_Req"].Value.ToString();
                            if (Curr_Ques_Type == "C")
                                Added_Edited_QuesCode[9] = dr.Cells["Resp_Code"].Value.ToString().Trim();
                            else
                                Added_Edited_QuesCode[9] = dr.Cells["Resp"].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(Added_Edited_QuesCode[9].Trim()))
                                Added_Edited_QuesCode[9] = "";
                        }
                        break;
                }
                Tmp_Cust_Cnt++;
            }


            return Added_Edited_QuesCode;
        }

        private void Cust_Grid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].CellRenderer == "ButtonCell" && e.RowIndex != -1)
                {

                    string response = Cust_Grid.Rows[e.RowIndex].Cells["Resp"].Value != null ? Cust_Grid.Rows[e.RowIndex].Cells["Resp"].Value.ToString() : string.Empty;
                    //gvQuestions.SelectedRows[0].Cells[8].Tag != null ? gvQuestions.SelectedRows[0].Cells[8].Tag.ToString() : string.Empty;
                    PrivilegeEntity privileges = new PrivilegeEntity();
                    privileges.AddPriv = "true";
                    AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, Cust_Grid.Rows[e.RowIndex].Cells["Code"].Value.ToString(), Cust_Grid.Rows[e.RowIndex].Cells["Ques"].Value.ToString(), string.Empty);
                    objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                    objform.StartPosition = FormStartPosition.CenterScreen;
                    objform.ShowDialog();
                }
            }
        }
    }
}