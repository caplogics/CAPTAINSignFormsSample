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
using Spire.Pdf.Grid;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AgencyReferral_SubForm : Form
    {

        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public AgencyReferral_SubForm(string form_display_mode, List<ACTREFSEntity> sel_REFS_List, string strdate, string strReferfromto, string strMode, BaseForm baseform)//List<CASEREFSEntity> sel_REFS_List)
        {
            InitializeComponent();
            //lblHeader.Text = "Agency Referrals";
            _model = new CaptainModel();
            Form_Display_Mode = form_display_mode;
            Referfromto = strReferfromto;
            Sel_REFS_List = null;
            ActRefsList = sel_REFS_List;
            ReferDate = strdate;
            Mode = strMode;
            BaseForm = baseform;
            FillCombo();
            pnlAgyNameSearch.Visible = false;
            //Referral Selection

            if (Form_Display_Mode.Equals("Detail"))
            {
                // this.Size = new System.Drawing.Size(689, 485);

                this.Ref_Sel.Visible = true;
                this.Street.Visible = true;
                this.City.Visible = true;
                this.State.Visible = true;
                this.gvtNameIndex.Visible = true;
                this.Name2.Visible = true;

                this.Text = "Agency Referrals";
                //this.Active.Visible = true;
                pnlAgyNameSearch.Visible = true;

                //Added by Sudheer on 06/08/2021
                if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CCA" && Mode == "Add")
                {
                    this.lblCustomer.Visible = true;
                    this.cmbName.Visible = true;
                    FillNameCombo();
                }

            }
            else
            {
                this.Name.Width = 362;
                ////this.RefGrid.Size = new System.Drawing.Size(454, 390);
                ////this.PnlSubform.Size = new System.Drawing.Size(464, 427);
                ////this.Save_Panel.Location = new System.Drawing.Point(318, 1);
                ////this.RefGrid.Location = new Point(2,8);
                //this.Name2.Visible = false;
                lblDate.Visible = false;
                lblDateReq.Visible = false;
                lblReferFromto.Visible = false;
                lblRefFromToReq.Visible = false;
                calDate.Visible = false;
                pnlDateName.Visible = false;
                cmbReferFromTo.Visible = false;
                this.Text = "Agency Referrals";
                this.Size = new Size(470, 435);
                pnlAgyNameSearch.Visible = true;
            }



            if (Mode == Consts.Common.Edit)
            {
                calDate.Enabled = false;
                cmbReferFromTo.Enabled = false;
                calDate.Value = Convert.ToDateTime(ReferDate);
                CommonFunctions.SetComboBoxValue(cmbReferFromTo, Referfromto);
                Sel_REFS_List = sel_REFS_List.FindAll(u => u.Date.Equals(ReferDate) && u.Type.Equals(Referfromto));
                Sel_Count = Sel_REFS_List.Count;
            }
            Fill_Agency_Referrals(Sel_REFS_List);
            if (Mode == Consts.Common.Add)
            {
                calDate.Text = DateTime.Today.ToShortDateString();
                calDate.Enabled = true;
                calDate_ValueChanged(calDate, new EventArgs());
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }

        public AgencyReferral_SubForm(string form_display_mode, string agencyCode, string strdate, string strReferfromto, string strMode, string strPartner)//List<CASEREFSEntity> sel_REFS_List)
        {
            InitializeComponent();
            //lblHeader.Text = "Agency Partner";
            _model = new CaptainModel();
            Form_Display_Mode = form_display_mode;
            Referfromto = strReferfromto;
            Sel_REFS_List = null;
            AgencyCode = agencyCode;
            //ActRefsList = sel_REFS_List;
            ReferDate = strdate;
            Mode = strMode;
            FormName = strPartner;
            FillCombo();
            pnlAgyNameSearch.Visible = false;
            //cmbReferFromTo.Visible = false;
            //Referral Selection

            if (Form_Display_Mode.Equals("Detail"))
            {
                // this.Size = new System.Drawing.Size(689, 485);

                this.Ref_Sel.Visible = true;
                this.Street.Visible = true;
                this.City.Visible = true;
                this.State.Visible = true;
                this.gvtNameIndex.Visible = false;
                this.Name2.Visible = false;
                lblDate.Visible = false;
                lblDateReq.Visible = false;
                lblReferFromto.Visible = false;
                lblRefFromToReq.Visible = false;
                calDate.Visible = false;
                cmbReferFromTo.Visible = false;
                pnlAgyNameSearch.Visible = true;

                this.Text = "CASE0013 - Agency Partner";
                //this.Active.Visible = true;
                pnlAgyNameSearch.Visible = true;
            }
            else
            {
                this.Name.Width = 362;
                this.RefGrid.Size = new System.Drawing.Size(454, 390);
                this.PnlSubform.Size = new System.Drawing.Size(464, 427);
                this.Save_Panel.Location = new System.Drawing.Point(318, 1);
                this.RefGrid.Location = new Point(2, 8);
                this.Name2.Visible = false;
                lblDate.Visible = false;
                lblDateReq.Visible = false;
                lblReferFromto.Visible = false;
                lblRefFromToReq.Visible = false;
                calDate.Visible = false;
                cmbReferFromTo.Visible = false;
                this.Text = "CASE0013 - Agency Partner";
                this.Size = new System.Drawing.Size(470, 430);
                pnlAgyNameSearch.Visible = true;
            }



            if (Mode == Consts.Common.Edit)
            {
                calDate.Enabled = false;
                cmbReferFromTo.Enabled = false;
                calDate.Value = Convert.ToDateTime(ReferDate);
                CommonFunctions.SetComboBoxValue(cmbReferFromTo, Referfromto);
                //Sel_REFS_List = sel_REFS_List.FindAll(u => u.Date.Equals(ReferDate) && u.Type.Equals(Referfromto));
                Sel_Count = Sel_REFS_List.Count;
            }
            Fill_Agency_Partner(Sel_REFS_List);
            if (Mode == Consts.Common.Add)
            {
                calDate.Text = DateTime.Today.ToShortDateString();
                calDate.Enabled = true;
                calDate_ValueChanged(calDate, new EventArgs());
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }


        //Sudheer
        public AgencyReferral_SubForm(string form_display_mode, List<CASEREFSEntity> sel_Refs_List, string Code)
        {
            InitializeComponent();
            //lblHeader.Text = "Services";
            _model = new CaptainModel();
            Form_Display_Mode = form_display_mode;
            Sel_CASEREFS_List = sel_Refs_List;
            code = Code;
            this.Ref_Sel.Visible = true;
            this.Name2.Visible = false;
            this.Name.Width = 380;
            this.Code.Visible = false;
            //this.RefGrid.ItemsPerPage = 100;

            //this.panel1.Location = new System.Drawing.Point(2, 1);
            this.RefGrid.Size = new System.Drawing.Size(452, 380);
            this.PnlSubform.Size = new System.Drawing.Size(464, 460);
            // this.Save_Panel.Location = new System.Drawing.Point(318, 1);

            this.Text = "Agency Referral Database - Select Services";
            this.Name.HeaderText = "Services";
            this.Size = new System.Drawing.Size(469, 468);
            this.RefGrid.Location = new Point(3, 2);

            pnlDateName.Visible = false;
            pnlAgencySearch.Visible = false;

            lblDate.Visible = false;
            lblDateReq.Visible = false;
            lblReferFromto.Visible = false;
            lblRefFromToReq.Visible = false;
            calDate.Visible = false;
            cmbReferFromTo.Visible = false;
            Fill_Services();
            if (Sel_CASEREFS_List.Count > 0)
            {
                Sel_Count = Sel_CASEREFS_List.Count;
            }
        }

        //Browse AgencyReferral Form
        public AgencyReferral_SubForm(string form_display_mode, string strMode)//List<CASEREFSEntity> sel_REFS_List)
        {
            InitializeComponent();
            //lblHeader.Text = "Agency Referrals";
            _model = new CaptainModel();
            Form_Display_Mode = form_display_mode;
            Mode = strMode;
            //Referral Selection

            if (Form_Display_Mode.Equals("Browse"))
            {
                // this.Size = new System.Drawing.Size(689, 485);
                lblDate.Visible = false;
                lblDateReq.Visible = false;
                lblReferFromto.Visible = false;
                lblRefFromToReq.Visible = false;
                calDate.Visible = false;
                cmbReferFromTo.Visible = false;
                pnlDateName.Visible = false;
                this.Ref_Sel.Visible = false;
                this.Street.Visible = true;
                this.City.Visible = true;
                this.State.Visible = true;
                this.gvtNameIndex.Visible = true;
                this.Name2.Visible = false;
                this.Text = "Browse - Agency Referrals";
                pnlAgencySearch.Visible = true;
                FormLoad();
                RefGrid.MultiSelect = false;
                BtnCancel.Visible = false;
                BtnSave.Text = "Select";
                BtnSave.Location = new Point(76, 1);

                //this.Active.Visible = true;
            }




            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }

        public List<CASEREFEntity> propCASEREF_List { get; set; }
        public List<AGCYPARTEntity> propPartner_List { get; set; }
        private void FillCombo()
        {
            cmbReferFromTo.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem(" ", "0"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Referred From", "F"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Referred To", "T"));
            cmbReferFromTo.Items.AddRange(listItem.ToArray());
            cmbReferFromTo.SelectedIndex = 0;
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public string Form_Display_Mode { get; set; }

        public string AgencyCode { get; set; }

        public string CAMS_Desc { get; set; }

        public string Hierarchy { get; set; }

        public string code { get; set; }

        public List<ACTREFSEntity> Sel_REFS_List { get; set; }

        public List<ACTREFSEntity> ActRefsList { get; set; }

        public List<CASEREFSEntity> Sel_CASEREFS_List { get; set; }

        public List<CaseVDD1Entity> Sel_CASEVDD1_List { get; set; }

        public CASEMSEntity Pass_MS_Entity { get; set; }

        public string ReferDate { get; set; }

        public string Referfromto { get; set; }

        public string Mode { get; set; }

        public string FormName { get; set; }

        #endregion

        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Blank = "blank";
        //string Img_Tick = "Resources/Images/tick.ico";
        string Img_Tick = "icon-gridtick";//"icon-done?color=#01a601";

        //ToolBarNew.Enabled = true;
        // ToolBarNew.ImageSource = Consts.Icons16x16.AddItem;






        private void Fill_Agency_Referrals(List<ACTREFSEntity> ActrefsEntityList)
        {

            CASEREFEntity Search_Entity = new CASEREFEntity();


            Search_Entity.Code = Search_Entity.Name1 = Search_Entity.Name2 = Search_Entity.Code = Search_Entity.IndexBy = null;
            Search_Entity.Street = Search_Entity.City = Search_Entity.State = Search_Entity.Zip = Search_Entity.Zip_Plus = null;
            Search_Entity.Area = Search_Entity.Excgange = Search_Entity.Telno = Search_Entity.Active = Search_Entity.Cont_Fname = null;
            Search_Entity.Cont_Lname = Search_Entity.Cont_Area = Search_Entity.Cont_Exchange = Search_Entity.Cont_TelNO = Search_Entity.Fax_Area = null;

            Search_Entity.Long_Distance = Search_Entity.Fax_Exchange = Search_Entity.Fax_Telno = Search_Entity.Outside = Search_Entity.Category = null;
            Search_Entity.County = Search_Entity.From_Hrs = Search_Entity.To_Hrs = Search_Entity.Sec = Search_Entity.Lstc_Date = null;
            Search_Entity.Lsct_Operator = Search_Entity.Add_Date = Search_Entity.Add_Operator = null;

            propCASEREF_List = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");
            RefGrid.Rows.Clear();
            if (propCASEREF_List.Count > 0)
            {
                //RefGrid.Rows.Add("00000", "None");
                int rowIndex = 0;
                if (Form_Display_Mode.Equals("Detail"))
                {
                    bool Sel_Ref = false;

                    foreach (CASEREFEntity Entity in propCASEREF_List)
                    {
                        Sel_Ref = false;
                        if (ActrefsEntityList != null)
                        {
                            foreach (ACTREFSEntity Entity1 in ActrefsEntityList)
                            {
                                if (Entity1.Code == Entity.Code)
                                {
                                    Sel_Ref = true; break;
                                }
                            }
                        }

                        string Name = string.Empty;
                        if (!string.IsNullOrEmpty(Entity.Name2.Trim()))
                            Name = Entity.Name2.Trim();
                        else Name = Entity.Name1.Trim();

                        if (Sel_Ref)
                            rowIndex = RefGrid.Rows.Add(true, Entity.Code, Entity.Name1.Trim(), Entity.Name2.Trim(), Entity.Street.Trim(), Entity.City.Trim(), Entity.State.Trim(), Entity.NameIndex, Entity.Active.Trim(), "Y");
                        else
                            rowIndex = RefGrid.Rows.Add(false, Entity.Code, Entity.Name1.Trim(), Entity.Name2.Trim(), Entity.Street.Trim(), Entity.City.Trim(), Entity.State.Trim(), Entity.NameIndex, Entity.Active.Trim(), "N");

                        if (Entity.Active.Trim() == "N")
                            RefGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                else
                {
                    rowIndex = RefGrid.Rows.Add(false, "00000", "None", " ", " ", " ", " ", " ", "N");
                    foreach (CASEREFEntity Entity in propCASEREF_List)
                    {
                        rowIndex = RefGrid.Rows.Add(false, Entity.Code, Entity.Name1.Trim(), Entity.Name2.Trim(), Entity.Street.Trim(), Entity.City.Trim(), Entity.State.Trim(), Entity.NameIndex, Entity.Active.Trim(), "N");
                        if (Entity.Active == "N")
                            RefGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }

                }
            }
            if (RefGrid.Rows.Count > 0)
            {
                RefGrid.Rows[0].Selected = true;

                if (!Form_Display_Mode.Equals("Detail"))
                {
                    this.Name2.Visible = false;
                }

            }
        }

        private void Fill_Agency_Partner(List<ACTREFSEntity> ActrefsEntityList)
        {
            AGCYPARTEntity Search_Entity = new AGCYPARTEntity(true);
            propPartner_List = _model.SPAdminData.Browse_AgencyPartner(Search_Entity, "Browse");


            RefGrid.Rows.Clear();
            if (propPartner_List.Count > 0)
            {
                //RefGrid.Rows.Add("00000", "None");
                int rowIndex = 0;
                if (Form_Display_Mode.Equals("Detail"))
                {
                    bool Sel_Ref = false;

                    foreach (AGCYPARTEntity Entity in propPartner_List)
                    {
                        Sel_Ref = false;
                        if (ActrefsEntityList != null)
                        {
                            foreach (ACTREFSEntity Entity1 in ActrefsEntityList)
                            {
                                if (Entity1.Code == Entity.Code)
                                {
                                    Sel_Ref = true; break;
                                }
                            }
                        }

                        string Name = string.Empty;
                        //if (!string.IsNullOrEmpty(Entity.Name2.Trim()))
                        //    Name = Entity.Name2.Trim();
                        //else Name = Entity.Name1.Trim();

                        if (Sel_Ref)
                            rowIndex = RefGrid.Rows.Add(true, Entity.Code, Entity.Name.Trim(), string.Empty, Entity.Street.Trim(), Entity.City.Trim(), Entity.State.Trim(), string.Empty, Entity.Active.Trim(), "Y");
                        else
                            rowIndex = RefGrid.Rows.Add(false, Entity.Code, Entity.Name.Trim(), string.Empty, Entity.Street.Trim(), Entity.City.Trim(), Entity.State.Trim(), string.Empty, Entity.Active.Trim(), "N");

                        if (Entity.Active.Trim() == "N")
                            RefGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                else
                {
                    rowIndex = RefGrid.Rows.Add(false, "00000", "None", " ", " ", " ", " ", " ", "N");
                    foreach (AGCYPARTEntity Entity in propPartner_List)
                    {
                        rowIndex = RefGrid.Rows.Add(false, Entity.Code, Entity.Name.Trim(), string.Empty, Entity.Street.Trim(), Entity.City.Trim(), Entity.State.Trim(), string.Empty, Entity.Active.Trim(), "N");
                        //if (Entity.Active == "N")
                        //    RefGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }

                }
            }
        }

        private void Fill_Services()
        {
            DataSet dsServices = DatabaseLayer.ADMNB001DB.ADMNB001_GetServices();
            DataTable dtServices = dsServices.Tables[0];

            //RefGrid.Rows.Add(Img_Blank, "00000", "None", " ", " ", " ", " ", "N");
            if (dtServices.Rows.Count > 0)
            {
                foreach (DataRow dr in dtServices.Rows)
                {
                    bool Sel_Ref = false;
                    int rowIndex = 0;
                    foreach (CASEREFSEntity Entity in Sel_CASEREFS_List)
                    {
                        if (Entity.Service.Trim() == dr["INQ_CODE"].ToString().Trim())
                        {
                            Sel_Ref = true;
                            rowIndex = RefGrid.Rows.Add(true, dr["INQ_CODE"].ToString().Trim(), dr["INQ_DESC"].ToString().Trim(), "", "", "", "", "", "", "Y");
                        }
                    }

                    if (!Sel_Ref)
                        rowIndex = RefGrid.Rows.Add(false, dr["INQ_CODE"].ToString().Trim(), dr["INQ_DESC"].ToString().Trim(), "", "", "", "", "", "", "N");
                }
            }
        }

        //Added by Sudheer on 06/08/2021
        private void FillNameCombo()
        {
            cmbName.Items.Clear();
            if (BaseForm.BaseCaseSnpEntity != null)
            {
                CaseSnpEntity casesnpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                List<CaseSnpEntity> caseSnpMembers = BaseForm.BaseCaseSnpEntity.FindAll(u => u.FamilySeq != BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                if (casesnpApplicant != null)
                {
                    string name = LookupDataAccess.GetMemberName(casesnpApplicant.NameixFi, casesnpApplicant.NameixMi, casesnpApplicant.NameixLast, BaseForm.BaseHierarchyCnFormat);

                    listItem.Add(new Captain.Common.Utilities.ListItem(name, casesnpApplicant.FamilySeq));
                }

                foreach (CaseSnpEntity caseSnp in caseSnpMembers)
                {
                    string name = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, BaseForm.BaseHierarchyCnFormat);
                    listItem.Add(new Captain.Common.Utilities.ListItem(name, casesnpApplicant.FamilySeq));
                }

                cmbName.Items.AddRange(listItem.ToArray());
                cmbName.SelectedIndex = 0;
            }
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (Form_Display_Mode == "Detail")
            {
                if (isValid())
                {
                    if (Sel_Count == 0)
                    {
                        //CommonFunctions.MessageBoxDisplay("At least one refered details selected");
                        AlertBox.Show("Please select at least one agency before saving", MessageBoxIcon.Warning);
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            else if (Form_Display_Mode == "Browse")
            {
                bool boolItemSelect = false;
                foreach (DataGridViewRow item in RefGrid.Rows)
                {
                    if (item.Selected)
                    {
                        boolItemSelect = true;
                        break;
                    }
                }
                if (boolItemSelect)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    //CommonFunctions.MessageBoxDisplay("Select Agecncy Referal Code");
                    AlertBox.Show("Select Agecncy Referal Code", MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (RefGrid.CurrentRow.Cells["Active"].Value.ToString() == "N")
                {
                    MessageBox.Show("Selected Referral is Inactive" + "\n" + "Are you sure want to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Selected_Referral_Row);
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        public void Selected_Referral_Row(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool isValid()
        {
            bool isValid = true;


            if (calDate.Checked == false)
            {
                _errorProvider.SetError(calDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(calDate, null);
            }

            //if (FormName != "Partner")
            //{
            if ((cmbReferFromTo.SelectedItem == null || ((ListItem)cmbReferFromTo.SelectedItem).Value == "0"))
            {
                _errorProvider.SetError(cmbReferFromTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReferFromto.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbReferFromTo, null);
            }
            //}

            return isValid;

        }

        public string[] GetSelected_Referral()
        {
            string[] SelRef = new string[2];

            if (!(string.IsNullOrEmpty(RefGrid.CurrentRow.Cells["Name"].Value.ToString())))
            {
                SelRef[0] = RefGrid.CurrentRow.Cells["Name"].Value.ToString();
                SelRef[1] = RefGrid.CurrentRow.Cells["Code"].Value.ToString();
            }

            return SelRef;
        }

        public List<ACTREFSEntity> GetSelected_Referral_Entity()
        {
            List<ACTREFSEntity> Sele_REFS_List = new List<ACTREFSEntity>();
            ACTREFSEntity Add_Entity = new ACTREFSEntity();
            foreach (DataGridViewRow dr in RefGrid.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    Add_Entity.Rec_Type = "I";
                    Add_Entity.Code = dr.Cells["Code"].Value.ToString();
                    Add_Entity.Date = calDate.Value.ToString();
                    Add_Entity.Type = ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString();
                    Add_Entity.NameIndex = dr.Cells["gvtNameIndex"].Value == null ? string.Empty : dr.Cells["gvtNameIndex"].Value.ToString();
                    //Add_Entity.Service = null;
                    //Add_Entity.Seq = null;
                    if (ActRefsList.Count > 0)
                    {
                        ACTREFSEntity actconnectedentity = ActRefsList.Find(u => u.Code == Add_Entity.Code);
                        if (actconnectedentity != null)
                            Add_Entity.Connected = actconnectedentity.Connected;
                        else
                            Add_Entity.Connected = "N";
                    }
                    else
                    {
                        Add_Entity.Connected = "N";
                    }
                    Sele_REFS_List.Add(new ACTREFSEntity(Add_Entity));
                }
            }

            return Sele_REFS_List;
        }


        public List<ACTREFSEntity> GetSelected_Partner_Entity()
        {
            List<ACTREFSEntity> Sele_REFS_List = new List<ACTREFSEntity>();
            ACTREFSEntity Add_Entity = new ACTREFSEntity();
            foreach (DataGridViewRow dr in RefGrid.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    Add_Entity.Rec_Type = "I";
                    Add_Entity.Code = dr.Cells["Code"].Value.ToString();
                    Add_Entity.Date = calDate.Value.ToString();
                    Add_Entity.Type = ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString();
                    //Add_Entity.NameIndex = dr.Cells["gvtNameIndex"].Value == null ? string.Empty : dr.Cells["gvtNameIndex"].Value.ToString();
                    //Add_Entity.Service = null;
                    //Add_Entity.Seq = null;
                    //if (ActRefsList.Count > 0)
                    //{
                    //    ACTREFSEntity actconnectedentity = ActRefsList.Find(u => u.Code == Add_Entity.Code);
                    //    if (actconnectedentity != null)
                    //        Add_Entity.Connected = actconnectedentity.Connected;
                    //    else
                    //        Add_Entity.Connected = "N";
                    //}
                    //else
                    //{
                    //    Add_Entity.Connected = "N";
                    //}
                    Sele_REFS_List.Add(new ACTREFSEntity(Add_Entity));
                }
            }

            return Sele_REFS_List;
        }


        public List<CASEREFSEntity> GetSelected_Services()
        {
            List<CASEREFSEntity> sele_CASEREFS_List = new List<CASEREFSEntity>();
            CASEREFSEntity Add_Entity = new CASEREFSEntity();
            foreach (DataGridViewRow dr in RefGrid.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    Add_Entity.Rec_Type = "I";
                    Add_Entity.Service = dr.Cells["Code"].Value.ToString();
                    sele_CASEREFS_List.Add(new CASEREFSEntity(Add_Entity));
                }
            }
            return sele_CASEREFS_List;
        }

        public List<CASEREFSEntity> GetAdd_Del_Selected_Services()
        {
            List<CASEREFSEntity> selected_CASEREFS_List = new List<CASEREFSEntity>();
            CASEREFSEntity Sel_Entity = new CASEREFSEntity();
            foreach (DataGridViewRow dr in RefGrid.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    Sel_Entity.Rec_Type = "I";
                    Sel_Entity.Service = dr.Cells["Code"].Value.ToString();
                    selected_CASEREFS_List.Add(new CASEREFSEntity(Sel_Entity));
                }
                else
                {
                    Sel_Entity.Rec_Type = "D";
                    Sel_Entity.Service = dr.Cells["Code"].Value.ToString();
                    selected_CASEREFS_List.Add(new CASEREFSEntity(Sel_Entity));
                }
            }
            return selected_CASEREFS_List;
        }

        public string GetCustomerName()
        {
            string SelRef = string.Empty;

            if (!string.IsNullOrEmpty(((ListItem)cmbName.SelectedItem).Text.Trim()))
            {
                SelRef = ((ListItem)cmbName.SelectedItem).Text.Trim();
                //SelRef[1] = RefGrid.CurrentRow.Cells["Code"].Value.ToString();
            }

            return SelRef;
        }


        int Sel_Count = 0;
        private void RefGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RefGrid.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (RefGrid.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            RefGrid.CurrentRow.Cells["Ref_Sel"].Value = false;//Img_Blank;
                            RefGrid.CurrentRow.Cells["Selected"].Value = "N";
                            Sel_Count--;
                        }
                        else
                        {
                            RefGrid.CurrentRow.Cells["Ref_Sel"].Value = true;//Img_Tick;
                            RefGrid.CurrentRow.Cells["Selected"].Value = "Y";
                            if (RefGrid.CurrentRow.Cells["Active"].Value.ToString() == "N" && Form_Display_Mode == "Detail")
                            {
                                RefGrid.CurrentRow.Cells["Ref_Sel"].Value = false;//Img_Blank;
                                //MessageBox.Show("Selected Referral is Inactive" + "\n" + "Are you sure want to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, Selected_AgyReferral_Row);
                            }
                            Sel_Count++;
                            if (Sel_Count > 30 && Form_Display_Mode == "ChldTrck")
                            {
                                RefGrid.CurrentRow.Cells["Ref_Sel"].Value = false;//Img_Blank;
                                RefGrid.CurrentRow.Cells["Selected"].Value = "N";
                                Sel_Count--;
                                AlertBox.Show("You may not select more than 30 services", MessageBoxIcon.Warning);
                            }

                        }
                    }
                }
            }
        }

        public void Selected_AgyReferral_Row(object sender, EventArgs e)
        {
            Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            if (senderform != null)
            {
                if (senderform.DialogResult.ToString() == "Yes")
                {
                    RefGrid.CurrentRow.Cells["Ref_Sel"].Value = true;//Img_Tick;
                }
            }
        }


        private void pnlHeader_Click(object sender, EventArgs e)
        {

        }

        private void RefGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Form_Display_Mode.Equals("Short"))
                BtnSave_Click(BtnSave, EventArgs.Empty);
        }

        private void calDate_ValueChanged(object sender, EventArgs e)
        {
            if (!((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Equals("0"))
            {
                Referfromto = ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString();
                ReferDate = calDate.Value.Date.ToString();
                List<ACTREFSEntity> actRefsEntityList = ActRefsList.FindAll(u => u.Date.Equals(calDate.Value.Date.ToString()) && u.Type.Equals(((ListItem)cmbReferFromTo.SelectedItem).Value.ToString()));
                if (FormName == "Partner")
                    Fill_Agency_Partner(actRefsEntityList);
                else
                    Fill_Agency_Referrals(actRefsEntityList);
            }
        }

        private void cmbReferFromTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!((ListItem)cmbReferFromTo.SelectedItem).Value.ToString().Equals("0"))
            {
                Referfromto = ((ListItem)cmbReferFromTo.SelectedItem).Value.ToString();
                ReferDate = calDate.Value.Date.ToString();
                List<ACTREFSEntity> actRefsEntityList = ActRefsList.FindAll(u => u.Date.Equals(calDate.Value.Date.ToString()) && u.Type.Equals(((ListItem)cmbReferFromTo.SelectedItem).Value.ToString()));
                if (FormName == "Partner")
                    Fill_Agency_Partner(actRefsEntityList);
                else
                    Fill_Agency_Referrals(actRefsEntityList);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FormLoad();
        }

        List<CASEREFEntity> CaseRefList;
        private void FormLoad()
        {
            int rowIndex = 0;
            RefGrid.Rows.Clear();
            CASEREFEntity Search_Entity = new CASEREFEntity(true);

            Search_Entity.Name1 = txtSearchAgyName.Text.Trim();
            Search_Entity.NameIndex = txtNameIndex.Text.Trim();
            CaseRefList = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");

            if (CaseRefList.Count > 0)
            {
                foreach (CASEREFEntity Entity in CaseRefList)
                {

                    rowIndex = RefGrid.Rows.Add(false, Entity.Code, Entity.Name1, Entity.Street, Entity.City, Entity.State, Entity.NameIndex);
                    if (Entity.Active.Equals("N"))
                        RefGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    CommonFunctions.setTooltip(rowIndex, Entity.Add_Operator.ToString().Trim(), Entity.Add_Date.ToString().Trim(), Entity.Lsct_Operator.ToString().Trim(), Entity.Lstc_Date.ToString().Trim(), RefGrid);
                }
            }
            if (RefGrid.Rows.Count > 0)
                RefGrid.Rows[0].Selected = true;

        }
        int intNext;
        private void btnNextSearch_Click(object sender, EventArgs e)
        {
            if (txtSearchName.Text.Trim() != string.Empty)
            {
                if (propCASEREF_List.Count > 0)
                {
                    List<CASEREFEntity> CASEREFEntitylist = propCASEREF_List.FindAll(x => x.Name1.ToUpper().Contains(txtSearchName.Text.ToUpper()) || x.Name2.ToUpper().Contains(txtSearchName.Text.ToUpper()));

                    RefGrid.ClearSelection();
                    intNext = 0;
                    if (CASEREFEntitylist.Count > 0)
                    {
                        foreach (DataGridViewRow item in RefGrid.Rows)
                        {
                            if (CASEREFEntitylist[0].Code.Trim() == Convert.ToString(item.Cells["Code"].Value))
                            {
                                //gvwCustomer.Update();
                                int i = item.Index;
                                // int intscroolindex = gvwCustomer.FirstDisplayedScrollingRowIndex;
                                //int CurrentPage = (i / RefGrid.ItemsPerPage);
                                //CurrentPage++;
                                //RefGrid.CurrentPage = CurrentPage;
                                //RefGrid.FirstDisplayedScrollingRowIndex = i;

                                RefGrid.CurrentCell = RefGrid.Rows[i].Cells[1];
                                RefGrid.ScrollCellIntoView(RefGrid.Rows[i].Cells[1]);

                                RefGrid.Rows[i].Selected = true;

                                break;

                            }
                        }
                    }
                    else
                    {
                        AlertBox.Show("Search String Not Found", MessageBoxIcon.Warning);

                    }
                    // txtFinddesc.Text = string.Empty;
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (txtSearchName.Text.Trim() != string.Empty)
            {
                bool Prev = false;

                foreach (DataGridViewRow item in RefGrid.Rows)
                {

                    if (item.Cells["Name"].Value.ToString().ToUpper().Trim().Contains(txtSearchName.Text.Trim().ToUpper()))
                    {
                        int i = item.Index;

                        if (intFindNext == item.Index)
                        {
                            // int CurrentPage = (FindPrev / RefGrid.ItemsPerPage);
                            // CurrentPage++;
                            //RefGrid.CurrentPage = CurrentPage;
                            RefGrid.CurrentCell = RefGrid.Rows[FindPrev].Cells[1];
                            // RefGrid.FirstDisplayedScrollingRowIndex = FindPrev;
                            RefGrid.Rows[FindPrev].Selected = true;
                            intFindNext = FindPrev; Prev = true;

                            break;
                        }
                        FindPrev = item.Index;

                    }
                }

                if (!Prev)
                {
                    /// int CurrentPage = (FindPrev / RefGrid.ItemsPerPage);
                    // CurrentPage++;
                    // RefGrid.CurrentPage = CurrentPage;
                    RefGrid.CurrentCell = RefGrid.Rows[FindPrev].Cells[1];
                    // RefGrid.FirstDisplayedScrollingRowIndex = FindPrev;
                    RefGrid.Rows[FindPrev].Selected = true;
                    intFindNext = FindPrev; Prev = true;
                }
            }
        }

        int intNext1 = 0; int intFindNext = 0; bool boolNxt = false; int FindPrev = 0;
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (txtSearchName.Text.Trim() != string.Empty)
            {
                if (RefGrid.Rows.Count > 0)
                {
                    foreach (DataGridViewRow item in RefGrid.Rows)
                    {
                        if (item.Cells["Name"].Value.ToString().ToUpper().Trim().Contains(txtSearchName.Text.Trim().ToUpper())) //if (txtCAMSSearch.Text.Trim().Contains(Convert.ToString(item.Cells["SP2_Desc"].Value).Trim()))
                        {

                            int i = item.Index;

                            if (intFindNext == 0 && !boolNxt)
                            {
                                // int CurrentPage = (i / RefGrid.ItemsPerPage);
                                //CurrentPage++;
                                // RefGrid.CurrentPage = CurrentPage;
                                RefGrid.CurrentCell = RefGrid.Rows[i].Cells[1];
                                // RefGrid.FirstDisplayedScrollingRowIndex = i;
                                RefGrid.Rows[i].Selected = true;
                                intFindNext = i;
                                break;
                            }
                            else
                            {
                                if (i > intFindNext)
                                {
                                    // int CurrentPage = (i / RefGrid.ItemsPerPage);
                                    //CurrentPage++;
                                    // RefGrid.CurrentPage = CurrentPage;
                                    RefGrid.CurrentCell = RefGrid.Rows[i].Cells[1];
                                    // RefGrid.FirstDisplayedScrollingRowIndex = i;
                                    RefGrid.Rows[i].Selected = true;
                                    intFindNext = i;
                                    break;
                                }
                            }

                        }
                    }
                    boolNxt = true;
                }
            }

        }

        private void AgencyReferral_SubForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS("CASE0006", 2, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }

        private void RefGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}